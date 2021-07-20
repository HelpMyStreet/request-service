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
    public class PostNewShifts
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PostNewShifts> _logger;

        public PostNewShifts(IMediator mediator, ILoggerWrapper<PostNewShifts> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PostNewShifts")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PostNewShiftsResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(PostNewShiftsRequest), "post new request for help shift request")] PostNewShiftsRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PostNewShifts started");
                PostNewShiftsResponse response = await _mediator.Send(req, cancellationToken);                
                return new OkObjectResult(ResponseWrapper<PostNewShiftsResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PostNewShifts", exc);
                return new ObjectResult(ResponseWrapper<PostNewShiftsResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
