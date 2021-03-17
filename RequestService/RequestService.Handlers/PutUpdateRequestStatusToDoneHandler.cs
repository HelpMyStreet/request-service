﻿using MediatR;
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
    public class PutUpdateRequestStatusToDoneHandler : IRequestHandler<PutUpdateRequestStatusToDoneRequest, PutUpdateRequestStatusToDoneResponse>
    {
        private readonly IRepository _repository;
        private readonly IJobService _jobService;
        private readonly ICommunicationService _communicationService;
        public PutUpdateRequestStatusToDoneHandler(IRepository repository, IJobService jobService, ICommunicationService communicationService)
        {
            _repository = repository;
            _jobService = jobService;
            _communicationService = communicationService;
        }

        public async Task<PutUpdateRequestStatusToDoneResponse> Handle(PutUpdateRequestStatusToDoneRequest request, CancellationToken cancellationToken)
        {
            PutUpdateRequestStatusToDoneResponse response = new PutUpdateRequestStatusToDoneResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };

            bool hasPermission = await _jobService.HasPermissionToChangeRequestAsync(request.RequestID, request.CreatedByUserID, cancellationToken);

            if(hasPermission)
            {
                var jobs =  await _repository.UpdateRequestStatusToDoneAsync(request.RequestID, request.CreatedByUserID, cancellationToken);

                foreach (int jobId in jobs)
                {
                    await _communicationService.RequestCommunication(
                    new RequestCommunicationRequest()
                    {
                        CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                        JobID = jobId,
                        AdditionalParameters = new Dictionary<string, string>()
                        {
                            { "FieldUpdated","Status" }
                        }
                    },
                    cancellationToken);
                }
                
                response.Outcome = UpdateJobStatusOutcome.Success;
            }
            else
            {
                response.Outcome = UpdateJobStatusOutcome.Unauthorized;
            }

            return response;
        }
    }
}
