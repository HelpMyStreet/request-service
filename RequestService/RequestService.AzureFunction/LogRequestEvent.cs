using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using System;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.Shared;
using Microsoft.AspNetCore.Http;
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HelpMyStreet.Utils.Utils;
using System.Threading;

namespace RequestService.AzureFunction
{
    public class LogRequestEvent
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<LogRequestEventRequest> _logger;

        public LogRequestEvent(IMediator mediator, ILoggerWrapper<LogRequestEventRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("LogRequestEvent")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LogRequestEventResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(LogRequestEventRequest), "log request event")] LogRequestEventRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("LogRequestEvent started");
                LogRequestEventResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<LogRequestEventResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in LogRequestEvent", exc);
                return new ObjectResult(ResponseWrapper<LogRequestEventResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
