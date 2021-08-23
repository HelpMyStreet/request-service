using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using System.Collections.Generic;
using System;

namespace RequestService.Handlers
{
    public class PutUpdateJobDueDateHandler : IRequestHandler<PutUpdateJobDueDateRequest, PutUpdateJobDueDateResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IJobService _jobService;
        public PutUpdateJobDueDateHandler(IRepository repository, ICommunicationService communicationService, IJobService jobService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _jobService = jobService;
        }

        public async Task<PutUpdateJobDueDateResponse> Handle(PutUpdateJobDueDateRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobDueDateResponse response = new PutUpdateJobDueDateResponse()
            {
                Outcome = UpdateJobOutcome.Unauthorized
            };

            if (request.DueDate < DateTime.UtcNow)
            {
                return response;
            }

            bool hasPermission = await _jobService.HasPermissionToChangeJobAsync(request.JobID.Value, request.AuthorisedByUserID.Value, cancellationToken);

            if (hasPermission)
            {
                var jobDetails = _repository.GetJobDetails(request.JobID.Value);

                if (
                    (jobDetails == null) || 
                    (jobDetails.JobSummary.JobStatus == JobStatuses.Done || jobDetails.JobSummary.JobStatus == JobStatuses.Cancelled)
                    )
                {
                    return response;
                }

                var oldValue = jobDetails.JobSummary.DueDate;
                var result = await _repository.UpdateJobDueDateAsync(request.JobID.Value, request.AuthorisedByUserID.Value, request.DueDate, cancellationToken);
                response.Outcome = result;

                if (result == UpdateJobOutcome.Success)
                {
                    response.Outcome = UpdateJobOutcome.Success;

                    await _repository.UpdateHistory(
                        requestId: jobDetails.JobSummary.RequestID,
                        createdByUserId: request.AuthorisedByUserID.Value,
                        fieldChanged: "Due Date",
                        oldValue: oldValue.ToString(),
                        newValue: request.DueDate.ToString(),
                        questionId: null,
                        jobId: request.JobID.Value);

                    await _communicationService.RequestCommunication(
                    new RequestCommunicationRequest()
                    {
                        CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                        JobID = request.JobID,
                        AdditionalParameters = new Dictionary<string, string>()
                        {
                            { "FieldUpdated","Due Date" },
                            { "OldValue", oldValue.ToString() },
                            { "NewValue", request.DueDate.ToString() }
                        }
                    },
                    cancellationToken);
                }
            }
            return response;
        }
    }
}
