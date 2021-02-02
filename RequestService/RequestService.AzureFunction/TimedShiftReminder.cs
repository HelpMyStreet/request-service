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
    public class TimedShiftReminder
    {
        private readonly ICommunicationService _commmunicationService;

        public TimedShiftReminder(ICommunicationService commmunicationService)
        {
            _commmunicationService = commmunicationService;
        }

        [FunctionName("TimedShiftReminder")]
        public async Task Run([TimerTrigger("%TimedShiftReminderCronExpression%")] TimerInfo timerInfo, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"TimedShiftReminder started at: {DateTime.Now}");
                await _commmunicationService.RequestCommunication(new RequestCommunicationRequest()
                {
                    CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendShiftReminder}
                }, cancellationToken);
                log.LogInformation($"TimedShiftReminder completed at: {DateTime.Now}");

            }
            catch (Exception ex)
            {
                log.LogError($"Unhandled error in TimedShiftReminder {ex}");
            }

        }
    }
}