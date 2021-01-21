using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using RequestService.Core.Services;
using System.Threading;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;

namespace RequestService.AzureFunction
{
    public class TimedManageRequests
    {
        private readonly IManageRequestsService _manageRequestsService;
        
        public TimedManageRequests(IManageRequestsService manageRequestsService)
        {
            _manageRequestsService = manageRequestsService;
        }

        [FunctionName("TimedManageRequests")]
        public async Task Run([TimerTrigger("%TimedManageRequestsCronExpression%",RunOnStartup = true)] TimerInfo timerInfo, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"TimedManageRequests started at: {DateTime.Now}");
                _manageRequestsService.ManageRequests();
                log.LogInformation($"TimedManageRequests completed at: {DateTime.Now}");

            }
            catch (Exception ex)
            {
                log.LogError($"Unhandled error in TimedManageRequests {ex}");
            }

        }
    }
}