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
    public class TimedNewRequestNotification
    {
        private readonly ICommunicationService _commmunicationService;

        public TimedNewRequestNotification(ICommunicationService commmunicationService)
        {
            _commmunicationService = commmunicationService;
        }

        [FunctionName("TimedNewRequestNotification")]
        public async Task Run([TimerTrigger("%TimedNewRequestNotificationCronExpression%")] TimerInfo timerInfo, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"TimedNewRequestNotification started at: {DateTime.Now}");
                await _commmunicationService.RequestCommunication(new RequestCommunicationRequest()
                {
                    CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendNewRequestNotification}
                }, cancellationToken);
                log.LogInformation($"TimedNewRequestNotification completed at: {DateTime.Now}");

            }
            catch (Exception ex)
            {
                log.LogError($"Unhandled error in TimedNewRequestNotification {ex}");
            }

        }
    }
}