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

namespace RequestService.Handlers
{
    public class PutUpdateJobStatusToOpenHandler : IRequestHandler<PutUpdateJobStatusToOpenRequest, PutUpdateJobStatusToOpenResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IJobService _jobService;
        public PutUpdateJobStatusToOpenHandler(IRepository repository, ICommunicationService communicationService, IJobService jobService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _jobService = jobService;
        }

        public async Task<PutUpdateJobStatusToOpenResponse> Handle(PutUpdateJobStatusToOpenRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobStatusToOpenResponse response = new PutUpdateJobStatusToOpenResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };
            if (_repository.JobHasStatus(request.JobID, JobStatuses.Open))
            {
                response.Outcome = UpdateJobStatusOutcome.AlreadyInThisStatus;
            }
            else
            {
                bool newToOpen = _repository.JobHasStatus(request.JobID, JobStatuses.New);

                bool hasPermission = await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.CreatedByUserID, !newToOpen, cancellationToken);
                GetJobDetailsResponse jobDetails = _repository.GetJobDetails(request.JobID);

                bool emailSent = false;

                if (hasPermission)
                {
                    if (jobDetails.JobSummary.JobStatus == JobStatuses.AppliedFor)
                    {
                        response.Outcome = await _repository.UpdateJobStatusToRejectedAsync(request.JobID, request.CreatedByUserID, cancellationToken);
                        if (response.Outcome != UpdateJobStatusOutcome.Success)
                        {
                            return response;
                        }

                        emailSent = await _communicationService.RequestCommunication(
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

                    var result = await _repository.UpdateJobStatusOpenAsync(request.JobID, request.CreatedByUserID, cancellationToken);
                    response.Outcome = result;

                    if (result == UpdateJobStatusOutcome.Success)
                    {
                        if (!emailSent)
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

                        if (newToOpen)
                        {
                            //TODO: Potentially, call Group Service here, to make following actions configurable (to mirror call to GetNewRequestActions in PostNewRequestForHelp)
                            
                            var jobSummary = _repository.GetJobSummary(request.JobID);

                            foreach (int groupId in jobSummary.JobSummary.Groups)
                            {
                                await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                                {
                                    GroupID = groupId,
                                    CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendNewTaskNotification },
                                    JobID = request.JobID
                                }, cancellationToken);
                            }
                        }
                    }
                }
            }
            return response;
        }
    }
}
