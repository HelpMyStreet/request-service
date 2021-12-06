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
    public class PutUpdateJobQuestion
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PutUpdateJobQuestion> _logger;

        public PutUpdateJobQuestion(IMediator mediator, ILoggerWrapper<PutUpdateJobQuestion> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PutUpdateJobQuestion")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PutUpdateJobQuestionResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]
            [RequestBodyType(typeof(PutUpdateJobQuestionRequest), "put update job question request")] PutUpdateJobQuestionRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PutUpdateJobQuestion started");
                PutUpdateJobQuestionResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<PutUpdateJobQuestionResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PutUpdateJobQuestion", exc);
                return new ObjectResult(ResponseWrapper<PutUpdateJobQuestionResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
