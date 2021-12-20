using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
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

        private void CancelJob(int jobId, JobStatusChangeReasonCodes jobStatusChangeReasonCode)
        {
            var result = _repository.UpdateJobStatusCancelledAsync(jobId, -1, jobStatusChangeReasonCode, CancellationToken.None).Result;

            if (result == UpdateJobStatusOutcome.Success)
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

        private void NotifyInProgressPastDueDate(int jobId)
        {
            _communicationService.RequestCommunication(
            new RequestCommunicationRequest()
            {
                CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.InProgressReminder },
                JobID = jobId
            },
            CancellationToken.None); 
        }

        public async Task ManageRequests()
        {
            await _repository.UpdateInProgressFromAccepted(JobStatusChangeReasonCodes.AutoProgressingShifts);
            await _repository.UpdateJobsToDoneFromInProgress(JobStatusChangeReasonCodes.AutoProgressingShifts);
            await _repository.UpdateJobsToCancelledFromNewOrOpen(JobStatusChangeReasonCodes.AutoProgressingShifts);

            var jobs = await _repository.GetOverdueRepeatJobs();
            jobs.ToList().ForEach(job => CancelJob(job, JobStatusChangeReasonCodes.AutoProgressingOverdueRepeats));

            var jobsPastDueDate = await _repository.GetJobsPastDueDate(JobStatuses.Open, 14);
            jobsPastDueDate.ToList().ForEach(job => CancelJob(job, JobStatusChangeReasonCodes.AutoProgressingJobsPastDueDates));

            var jobsInProgressPastDueDate = await _repository.GetJobsPastDueDate(JobStatuses.InProgress, 3);
            jobsInProgressPastDueDate.ToList().ForEach(job => NotifyInProgressPastDueDate(job));

        }
    }
}
