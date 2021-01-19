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
    public class GetRequestSummary
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetRequestSummaryRequest> _logger;

        public GetRequestSummary(IMediator mediator, ILoggerWrapper<GetRequestSummaryRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetRequestSummary")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetRequestSummaryResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            [RequestBodyType(typeof(GetRequestSummaryRequest), "Get request summary request")] GetRequestSummaryRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetRequestSummary started");
                GetRequestSummaryResponse response = await _mediator.Send(req,cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetRequestSummaryResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetRequestSummary", exc);
                return new ObjectResult(ResponseWrapper<GetRequestSummaryResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
