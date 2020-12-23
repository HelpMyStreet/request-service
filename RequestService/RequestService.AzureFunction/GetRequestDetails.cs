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
    public class GetRequestDetails
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetRequestDetailsRequest> _logger;

        public GetRequestDetails(IMediator mediator, ILoggerWrapper<GetRequestDetailsRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetRequestDetails")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetRequestDetailsResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            [RequestBodyType(typeof(GetJobDetailsRequest), "Get request details request")] GetRequestDetailsRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetJobDetails started");
                GetRequestDetailsResponse response = await _mediator.Send(req,cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetRequestDetailsResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetRequestDetails", exc);
                return new ObjectResult(ResponseWrapper<GetRequestDetailsResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
