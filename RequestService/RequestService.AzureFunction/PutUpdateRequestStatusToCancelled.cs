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
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HelpMyStreet.Utils.Utils;
using System.Threading;

namespace RequestService.AzureFunction
{
    public class PutUpdateRequestStatusToCancelled
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PutUpdateRequestStatusToCancelled> _logger;

        public PutUpdateRequestStatusToCancelled(IMediator mediator, ILoggerWrapper<PutUpdateRequestStatusToCancelled> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PutUpdateRequestStatusToCancelled")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PutUpdateRequestStatusToCancelledResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]
            [RequestBodyType(typeof(PutUpdateRequestStatusToCancelledRequest), "put update request status to cancelled")] PutUpdateRequestStatusToCancelledRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PutUpdateRequestStatusToCancelled started");
                PutUpdateRequestStatusToCancelledResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<PutUpdateRequestStatusToCancelledResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PutUpdateRequestStatusToCancelled", exc);
                return new ObjectResult(ResponseWrapper<PutUpdateRequestStatusToCancelledResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
