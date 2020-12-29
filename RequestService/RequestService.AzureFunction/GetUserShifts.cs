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
using RequestService.Core.Exceptions;
using System.Net;
using HelpMyStreet.Utils.Utils;
using System.Threading;

namespace RequestService.AzureFunction
{
    public class GetUserShifts
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetUserShiftsRequest> _logger;

        public GetUserShifts(IMediator mediator, ILoggerWrapper<GetUserShiftsRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetUserShifts")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetUserShiftsResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            GetUserShiftsRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetUserShifts started");
                GetUserShiftsResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetUserShiftsResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetUserShifts", exc);
                return new ObjectResult(ResponseWrapper<GetUserShiftsResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
