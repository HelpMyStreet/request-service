using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Dto;
using RequestService.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;

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
            }
            return response;
        }
    }
}
