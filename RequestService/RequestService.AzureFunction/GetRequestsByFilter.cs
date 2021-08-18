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
    public class GetRequestsByFilter
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetRequestsByFilter> _logger;

        public GetRequestsByFilter(IMediator mediator, ILoggerWrapper<GetRequestsByFilter> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetRequestsByFilter")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetRequestsByFilterResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]            
            //[RequestBodyType(typeof(GetRequestsByFilterRequest), "Get Requests By Filter request")] 
            GetRequestsByFilterRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetRequestsByFilter started");
                GetRequestsByFilterResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetRequestsByFilterResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetRequestsByFilter", exc);
                return new ObjectResult(ResponseWrapper<GetRequestsByFilterResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
