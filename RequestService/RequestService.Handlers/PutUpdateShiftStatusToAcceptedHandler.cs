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

namespace RequestService.Handlers
{
    public class PutUpdateShiftStatusToAcceptedHandler : IRequestHandler<PutUpdateShiftStatusToAcceptedRequest, PutUpdateShiftStatusToAcceptedResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IJobService _jobService;
        public PutUpdateShiftStatusToAcceptedHandler(IRepository repository, ICommunicationService communicationService, IJobService jobService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _jobService = jobService;
        }

        public async Task<PutUpdateShiftStatusToAcceptedResponse> Handle(PutUpdateShiftStatusToAcceptedRequest request, CancellationToken cancellationToken)
        {
            PutUpdateShiftStatusToAcceptedResponse response = new PutUpdateShiftStatusToAcceptedResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized,
                JobID = -1
            };

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
                catch (UnableToUpdateShiftException)
                {
                    return new PutUpdateShiftStatusToAcceptedResponse()
                    {
                        Outcome = UpdateJobStatusOutcome.NoLongerAvailable,
                        JobID = -1
                    };
                }
            }

            //if (_repository.JobHasStatus(request.JobID, JobStatuses.Open))
            //{
            //    response.Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus;
            //}
            //else
            //{
            //    bool newToOpen = _repository.JobHasStatus(request.JobID, JobStatuses.New);

            //    bool hasPermission = await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.CreatedByUserID, !newToOpen, cancellationToken);

            //    if (hasPermission)
            //    {
            //        var result = await _repository.UpdateJobStatusOpenAsync(request.JobID, request.CreatedByUserID, cancellationToken);
            //        response.Outcome = result;

            //        if (result == UpdateJobStatusOutcome.Success)
            //        {
            //            await _communicationService.RequestCommunication(
            //            new RequestCommunicationRequest()
            //            {
            //                CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
            //                JobID = request.JobID,
            //                AdditionalParameters = new Dictionary<string, string>()
            //                {
            //                    { "FieldUpdated","Status" }
            //                }
            //            },
            //            cancellationToken);

            //            if (newToOpen)
            //            {
            //                //TODO: Potentially, call Group Service here, to make following actions configurable (to mirror call to GetNewRequestActions in PostNewRequestForHelp)
                            
            //                var jobSummary = _repository.GetJobSummary(request.JobID);

            //                foreach (int groupId in jobSummary.JobSummary.Groups)
            //                {
            //                    await _communicationService.RequestCommunication(new RequestCommunicationRequest()
            //                    {
            //                        GroupID = groupId,
            //                        CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendNewTaskNotification },
            //                        JobID = request.JobID
            //                    }, cancellationToken);
            //                }
            //            }
            //        }
            //    }
            //}
            return response;
        }
    }
}
