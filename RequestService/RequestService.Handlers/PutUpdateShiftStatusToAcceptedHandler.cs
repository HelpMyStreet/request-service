using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using System.Collections.Generic;
using System.Linq;
using RequestService.Core.Exceptions;
using System;

namespace RequestService.Handlers
{
    public class PutUpdateShiftStatusToAcceptedHandler : IRequestHandler<PutUpdateShiftStatusToAcceptedRequest, PutUpdateShiftStatusToAcceptedResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IJobService _jobService;
        private readonly IGroupService _groupService;
        public PutUpdateShiftStatusToAcceptedHandler(IRepository repository, ICommunicationService communicationService, IJobService jobService, IGroupService groupService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _jobService = jobService;
            _groupService = groupService;
        }

        public async Task<PutUpdateShiftStatusToAcceptedResponse> Handle(PutUpdateShiftStatusToAcceptedRequest request, CancellationToken cancellationToken)
        {
            PutUpdateShiftStatusToAcceptedResponse response = new PutUpdateShiftStatusToAcceptedResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized,
                JobID = -1
            };

            var requestDetails = _repository.GetRequestDetails(request.RequestID);
            var volunteerGroups = await _groupService.GetUserGroups(request.VolunteerUserID, cancellationToken);
            var requestGroups = await _repository.GetGroupsForRequestAsync(request.RequestID, cancellationToken);

            if (volunteerGroups == null || requestGroups == null)
            {
                throw new Exception("volunteerGroups or requestGroups is null");
            }

            bool jobGroupContainsVolunteerGroups = requestGroups.Any(volunteerGroups.Groups.Contains);

            if (!jobGroupContainsVolunteerGroups)
            {
                response.Outcome = UpdateJobStatusOutcome.BadRequest;
                return response;
            }

            int existingJobId = await _repository.VolunteerAlreadyAcceptedShift(request.RequestID, request.SupportActivity.SupportActivity, request.VolunteerUserID, cancellationToken);

            if (existingJobId > 0)
            {
                response = new PutUpdateShiftStatusToAcceptedResponse()
                {
                    Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus,
                    JobID = existingJobId
                };
                return response;
            }
            else
            {                
                try
                {
                    bool hasPermission = (request.CreatedByUserID == request.VolunteerUserID || await _jobService.HasPermissionToChangeRequestAsync(request.RequestID, request.CreatedByUserID, cancellationToken));

                    if (hasPermission)
                    {
                        var jobId = _repository.UpdateRequestStatusToAccepted(request.RequestID, request.SupportActivity.SupportActivity, request.CreatedByUserID, request.VolunteerUserID, cancellationToken);

                        if (jobId > 0)
                        {
                            return new PutUpdateShiftStatusToAcceptedResponse()
                            {
                                JobID = jobId,
                                Outcome = UpdateJobStatusOutcome.Success
                            };
                        }
                    }
                }
                catch (UnableToUpdateShiftException)
                {
                    return new PutUpdateShiftStatusToAcceptedResponse()
                    {
                        Outcome = UpdateJobStatusOutcome.NoLongerAvailable,
                        JobID = -1
                    };
                }
            }
            return response;
        }
    }
}
