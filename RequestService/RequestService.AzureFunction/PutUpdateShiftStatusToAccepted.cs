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
    public class PutUpdateShiftStatusToAccepted
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PutUpdateShiftStatusToAcceptedRequest> _logger;

        public PutUpdateShiftStatusToAccepted(IMediator mediator, ILoggerWrapper<PutUpdateShiftStatusToAcceptedRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PutUpdateShiftStatusToAccepted")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PutUpdateShiftStatusToAcceptedResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]
            [RequestBodyType(typeof(PutUpdateShiftStatusToAcceptedRequest), "put update shift status to accepted request")] PutUpdateShiftStatusToAcceptedRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("PutUpdateShiftStatusToAccepted started");
                PutUpdateShiftStatusToAcceptedResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<PutUpdateShiftStatusToAcceptedResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in PutUpdateShiftStatusToAccepted", exc);
                return new ObjectResult(ResponseWrapper<PutUpdateShiftStatusToAcceptedResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
