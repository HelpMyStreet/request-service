using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;
using Newtonsoft.Json;
using RequestService.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;

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
        
        public void ManageRequests()
        {
            _repository.UpdateInProgressFromAccepted();
            _repository.UpdateJobsToDoneFromInProgress();
            _repository.UpdateJobsToCancelledFromNewOrOpen();

            var jobs =_repository.GetOverdueRepeatJobs();


            foreach (int jobId in jobs)
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
        }
    }
}
