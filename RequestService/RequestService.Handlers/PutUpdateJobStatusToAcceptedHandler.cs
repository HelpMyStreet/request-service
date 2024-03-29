﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System.Collections.Generic;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;

namespace RequestService.Handlers
{
    public class PutUpdateJobStatusToAcceptedHandler : IRequestHandler<PutUpdateJobStatusToAcceptedRequest, PutUpdateJobStatusToAcceptedResponse>
    {
        private readonly IRepository _repository;
        private readonly IJobService _jobService;
        private readonly ICommunicationService _communicationService;
        public PutUpdateJobStatusToAcceptedHandler(IRepository repository, IJobService jobService, ICommunicationService communicationService)
        {
            _repository = repository;
            _jobService = jobService;
            _communicationService = communicationService;
        }

        public async Task<PutUpdateJobStatusToAcceptedResponse> Handle(PutUpdateJobStatusToAcceptedRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobStatusToAcceptedResponse response = new PutUpdateJobStatusToAcceptedResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };

            if (_repository.JobHasStatus(request.JobID, JobStatuses.Accepted))
            {
                response.Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus;
            }
            else
            {                
                bool hasPermission = (request.CreatedByUserID == request.VolunteerUserID || await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.CreatedByUserID, false, cancellationToken));

                if (hasPermission)
                {
                    if (!_repository.VolunteerHasAlreadyJobForThisRequestWithThisStatus(request.JobID, request.VolunteerUserID, JobStatuses.Accepted))
                    {
                        var result = await _repository.UpdateJobStatusAcceptedAsync(request.JobID, request.CreatedByUserID, request.VolunteerUserID, cancellationToken);
                        response.Outcome = result;

                        if (result == UpdateJobStatusOutcome.Success)
                        {
                            await _communicationService.RequestCommunication(
                            new RequestCommunicationRequest()
                            {
                                CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                                JobID = request.JobID,
                                AdditionalParameters = new Dictionary<string, string>()
                                {
                                { "FieldUpdated","Status" }
                                }
                            },
                            cancellationToken);
                        }
                    }
                    else
                    {
                        response.Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus;
                    }
                }
            }
            return response;
        }
    }
}
