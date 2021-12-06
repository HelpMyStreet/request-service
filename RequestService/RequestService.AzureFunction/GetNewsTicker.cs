﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using System;
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.Shared;
using Microsoft.AspNetCore.Http;
using HelpMyStreet.Contracts;

namespace RequestService.AzureFunction
{
    public class GetNewsTicker
    {
        private readonly IMediator _mediator;

        public GetNewsTicker(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetNewsTicker")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NewsTickerResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            [RequestBodyType(typeof(NewsTickerRequest), "Get News Ticker Request")] NewsTickerRequest req,
            ILogger log)
        {
            try
            {
                NewsTickerResponse response = await _mediator.Send(req);

                return new OkObjectResult(ResponseWrapper<NewsTickerResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                return new ObjectResult(ResponseWrapper<NewsTickerResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
