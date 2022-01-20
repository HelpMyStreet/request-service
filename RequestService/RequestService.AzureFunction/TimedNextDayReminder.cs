using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using RequestService.Core.Services;
using System.Threading;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;

namespace RequestService.AzureFunction
{
    public class TimedNextDayReminder
    {
        private readonly ICommunicationService _commmunicationService;

        public TimedNextDayReminder(ICommunicationService commmunicationService)
        {
            _commmunicationService = commmunicationService;
        }

        [FunctionName("TimedNextDayReminder")]
        public async Task Run([TimerTrigger("%TimedNextDayReminderCronExpression%")] TimerInfo timerInfo, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"TimedNextDayReminder started at: {DateTime.Now}");
                await _commmunicationService.RequestCommunication(new RequestCommunicationRequest()
                {
                    CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.JobsDueTomorrow }
                }, cancellationToken);                
                log.LogInformation($"TimedNextDayReminder completed at: {DateTime.Now}");

            }
            catch (Exception ex)
            {
                log.LogError($"Unhandled error in TimedNextDayReminder {ex}");
            }

        }
    }
}