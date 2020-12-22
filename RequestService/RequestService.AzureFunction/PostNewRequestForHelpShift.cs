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
    public class PostNewRequestForHelpShift
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PostNewRequestForHelpShiftRequest> _logger;

        public PostNewRequestForHelpShift(IMediator mediator, ILoggerWrapper<PostNewRequestForHelpShiftRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PostNewRequestForHelpShift")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PostNewRequestForHelpShiftResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(PostNewRequestForHelpShiftRequest), "post new request for help shift request")] PostNewRequestForHelpShiftRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PostNewRequestForHelpShift started");
                PostNewRequestForHelpShiftResponse response = await _mediator.Send(req, cancellationToken);                
                return new OkObjectResult(ResponseWrapper<PostNewRequestForHelpShiftResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PostNewRequestForHelpShift", exc);
                return new ObjectResult(ResponseWrapper<PostNewRequestForHelpShiftResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
