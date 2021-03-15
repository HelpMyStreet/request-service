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
    public class PutUpdateRequestStatusToDone
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PutUpdateRequestStatusToDoneRequest> _logger;

        public PutUpdateRequestStatusToDone(IMediator mediator, ILoggerWrapper<PutUpdateRequestStatusToDoneRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PutUpdateRequestStatusToDone")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PutUpdateRequestStatusToDoneResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]
            [RequestBodyType(typeof(PutUpdateRequestStatusToDoneRequest), "put update request status to done")] PutUpdateRequestStatusToDoneRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PutUpdateRequestStatusToDone started");
                PutUpdateRequestStatusToDoneResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<PutUpdateRequestStatusToDoneResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PutUpdateRequestStatusToDone", exc);
                return new ObjectResult(ResponseWrapper<PutUpdateRequestStatusToDoneResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
