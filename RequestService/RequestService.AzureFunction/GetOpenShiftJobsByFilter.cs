﻿using System.Threading.Tasks;
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
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace RequestService.AzureFunction
{
    public class GetOpenShiftJobsByFilter
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetUserShiftJobsByFilter> _logger;

        public GetOpenShiftJobsByFilter(IMediator mediator, ILoggerWrapper<GetUserShiftJobsByFilter> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetOpenShiftJobsByFilter")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetOpenShiftJobsByFilterResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]            
            [RequestBodyType(typeof(GetOpenShiftJobsByFilterRequest), "Get Open Shift Jobs By Filter request")] GetOpenShiftJobsByFilterRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetOpenShiftJobsByFilter started");
                GetOpenShiftJobsByFilterResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetOpenShiftJobsByFilterResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetOpenShiftJobsByFilterResponse", exc);
                return new ObjectResult(ResponseWrapper<GetOpenShiftJobsByFilterResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
