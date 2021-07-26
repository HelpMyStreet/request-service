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
    public class GetRequestIDs
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetRequestIDs> _logger;

        public GetRequestIDs(IMediator mediator, ILoggerWrapper<GetRequestIDs> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetRequestIDs")]        
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            [RequestBodyType(typeof(GetRequestIDsRequest), "get job details request")] 
            GetRequestIDsRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetRequestIDs started");
                GetRequestIDsResponse response = await _mediator.Send(req, cancellationToken);
                return new OkObjectResult(ResponseWrapper<GetRequestIDsResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetRequestIDs", exc);
                return new ObjectResult(ResponseWrapper<GetRequestIDsResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
