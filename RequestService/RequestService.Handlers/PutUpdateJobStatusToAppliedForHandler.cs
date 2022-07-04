using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using System.Linq;
using System;
using HelpMyStreet.Contracts.GroupService.Request;
using System.Collections.Generic;

namespace RequestService.Handlers
{
    public class PutUpdateJobStatusToAppliedForHandler : IRequestHandler<PutUpdateJobStatusToAppliedForRequest, PutUpdateJobStatusToAppliedForResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IGroupService _groupService;
        private readonly IJobService _jobService;
        private const int ADMIN_USERID = -1;

        public PutUpdateJobStatusToAppliedForHandler(IRepository repository, ICommunicationService communicationService, IGroupService groupService, IJobService jobService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _groupService = groupService;
            _jobService = jobService;
        }

        public async Task<PutUpdateJobStatusToAppliedForResponse> Handle(PutUpdateJobStatusToAppliedForRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobStatusToAppliedForResponse response = new PutUpdateJobStatusToAppliedForResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };

            bool hasPermission = (request.CreatedByUserID == request.VolunteerUserID || await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.CreatedByUserID, false, cancellationToken));

            if(!hasPermission)
            {
                return response;
            }

            if (_repository.JobIsInProgressWithSameVolunteerUserId(request.JobID, request.VolunteerUserID) || 
                (_repository.VolunteerHasAlreadyJobForThisRequestWithThisStatus(request.JobID, request.VolunteerUserID, JobStatuses.AppliedFor)))
            {
                response.Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus;
                return response;
            }
            
            var jobDetails = _repository.GetJobDetails(request.JobID);
            var volunteerGroups = await _groupService.GetUserGroups(request.VolunteerUserID, cancellationToken);
            var jobGroups = await _repository.GetGroupsForJobAsync(request.JobID, cancellationToken);
            int referringGroupId = await _repository.GetReferringGroupIDForJobAsync(request.JobID, cancellationToken);

            if (volunteerGroups == null || jobGroups == null)
            {
                throw new Exception("volunteerGroups or jobGroup is null");
            }

            bool jobGroupContainsVolunteerGroups = jobGroups.Any(volunteerGroups.Groups.Contains);

            if (!jobGroupContainsVolunteerGroups)
            {
                response.Outcome = UpdateJobStatusOutcome.BadRequest;
                return response;
            }

            var groupMember = await _groupService.GetGroupMember(new GetGroupMemberRequest()
            {
                AuthorisingUserId = request.CreatedByUserID,
                UserId = request.VolunteerUserID,
                GroupId = referringGroupId
            });

            var groupActivityCredentials = await _groupService.GetGroupActivityCredentials(new GetGroupActivityCredentialsRequest()
            {
                GroupId = referringGroupId,
                SupportActivityType = new SupportActivityType() { SupportActivity = jobDetails.JobSummary.SupportActivity }
            });

            bool hasValidCredentials = true;

            foreach (List<int> c in groupActivityCredentials.CredentialSets)
            {
                if (hasValidCredentials)
                {
                    hasValidCredentials = groupMember
                        .UserInGroup
                        .ValidCredentials
                        .Any(a => c.Contains(a));
                }
                else
                {
                    break;
                }
            }

            if (!hasValidCredentials)
            {
                response.Outcome = UpdateJobStatusOutcome.BadRequest;
                return response;
            }

            var result = await _repository.UpdateJobStatusAppliedForAsync(request.JobID, request.CreatedByUserID, request.VolunteerUserID, JobStatusChangeReasonCodes.UserChange, cancellationToken);
            response.Outcome = result;

            if (result == UpdateJobStatusOutcome.Success)
            {
                await _communicationService.RequestCommunication(
                    new RequestCommunicationRequest()
                    {
                        CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.TaskAppliedForNotification },
                        JobID = request.JobID,
                        GroupID = referringGroupId,
                        RequestID = jobDetails.RequestSummary.RequestID
                    },
                    cancellationToken);
            }
                       
            return response;
        }
    }
}
