using System.Threading.Tasks;
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
using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.ReportService.Request;

namespace RequestService.AzureFunction
{
    public class GetChart
    {
        private readonly IMediator _mediator;

        public GetChart(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetChart")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetChartResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            [RequestBodyType(typeof(GetChartRequest), "Get chart request")] GetChartRequest req,
            ILogger log)
        {
            try
            {
                GetChartResponse response = await _mediator.Send(req);

                return new OkObjectResult(ResponseWrapper<GetChartResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                return new ObjectResult(ResponseWrapper<GetChartResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
