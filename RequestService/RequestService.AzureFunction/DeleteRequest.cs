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
    public class DeleteRequest
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<DeleteRequest> _logger;

        public DeleteRequest(IMediator mediator, ILoggerWrapper<DeleteRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("DeleteRequest")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DeleteRequestResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)]
            [RequestBodyType(typeof(GetRequestDetailsRequest), "delete request")] DeleteRequestRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("DeleteRequest started");
                DeleteRequestResponse response = await _mediator.Send(req,cancellationToken); 
                return new OkObjectResult(ResponseWrapper<DeleteRequestResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in DeleteRequest", exc);
                return new ObjectResult(ResponseWrapper<DeleteRequestResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}

