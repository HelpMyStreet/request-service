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
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace RequestService.AzureFunction
{
    public class GetShiftRequestsByFilter
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetShiftRequestsByFilter> _logger;

        public GetShiftRequestsByFilter(IMediator mediator, ILoggerWrapper<GetShiftRequestsByFilter> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetShiftRequestsByFilter")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetShiftRequestsByFilterResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]            
            [RequestBodyType(typeof(GetShiftRequestsByFilterRequest), "Get Shift Requests By Filter request")] GetShiftRequestsByFilterRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetShiftRequestsByFilter started");
                GetShiftRequestsByFilterResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetShiftRequestsByFilterResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception occured in GetShiftRequestsByFilter", exc);
                _logger.LogError(exc.ToString(),exc);
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetShiftRequestsByFilter", exc);
                return new ObjectResult(ResponseWrapper<GetShiftRequestsByFilterResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
