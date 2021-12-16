﻿using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;
using Newtonsoft.Json;
using RequestService.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public class ManageRequestsService : IManageRequestsService
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;

        public ManageRequestsService(IRepository repository, ICommunicationService communicationService)
        {
            _repository = repository;
            _communicationService = communicationService;
        }
        
        private void CancelJob(int jobId)
        {
            var result = _repository.UpdateJobStatusCancelledAsync(jobId, -1, CancellationToken.None).Result;

            if (result == HelpMyStreet.Utils.Enums.UpdateJobStatusOutcome.Success)
            {
                _communicationService.RequestCommunication(
                new RequestCommunicationRequest()
                {
                    CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                    JobID = jobId,
                    AdditionalParameters = new Dictionary<string, string>()
                    {
                                { "FieldUpdated","Status" }
                    }
                },
                CancellationToken.None);
            }
        }
        
        public async Task ManageRequests()
        {
            await _repository.UpdateInProgressFromAccepted();
            await _repository.UpdateJobsToDoneFromInProgress();
            await _repository.UpdateJobsToCancelledFromNewOrOpen();

            var jobs = await _repository.GetOverdueRepeatJobs();
            jobs.ToList().ForEach(job => CancelJob(job));

            var jobsPastDueDate = await _repository.GetJobsPastDueDate(14);
            jobsPastDueDate.ToList().ForEach(job => CancelJob(job));
        }
    }
}
