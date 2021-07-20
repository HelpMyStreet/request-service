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
    public class PostRequestForHelp
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PostRequestForHelp> _logger;

        public PostRequestForHelp(IMediator mediator, ILoggerWrapper<PostRequestForHelp> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PostRequestForHelp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PostRequestForHelpResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(PostRequestForHelpRequest), "post request for help request")] PostRequestForHelpRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PostRequestForHelp started");
                PostRequestForHelpResponse response = await _mediator.Send(req, cancellationToken);                
                return new OkObjectResult(ResponseWrapper<PostRequestForHelpResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PostRequestForHelp", exc);
                return new ObjectResult(ResponseWrapper<PostRequestForHelpResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
