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
    public class PutUpdateJobStatusToAccepted
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PutUpdateJobStatusToAccepted> _logger;

        public PutUpdateJobStatusToAccepted(IMediator mediator, ILoggerWrapper<PutUpdateJobStatusToAccepted> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PutUpdateJobStatusToAccepted")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PutUpdateJobStatusToAcceptedResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]
            [RequestBodyType(typeof(PutUpdateJobStatusToAcceptedRequest), "put update job status to accepted request")] PutUpdateJobStatusToAcceptedRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PutUpdateJobStatusToAcceptedRequest started");
                PutUpdateJobStatusToAcceptedResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<PutUpdateJobStatusToAcceptedResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PutUpdateJobStatusToAcceptedRequest", exc);
                return new ObjectResult(ResponseWrapper<PutUpdateJobStatusToAcceptedResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
