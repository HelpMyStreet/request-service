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
    public class GetRequestIDsForGroup
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetRequestIDsForGroup> _logger;

        public GetRequestIDsForGroup(IMediator mediator, ILoggerWrapper<GetRequestIDsForGroup> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("GetRequestIDsForGroup")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetRequestIDsForGroupResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]            
            [RequestBodyType(typeof(GetRequestIDsForGroupRequest), "Get RequestIDs For Group Request")]
            GetRequestIDsForGroupRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetRequestIDsForGroup started");
                GetRequestIDsForGroupResponse response = await _mediator.Send(req, cancellationToken); 
                return new OkObjectResult(ResponseWrapper<GetRequestIDsForGroupResponse, RequestServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogErrorAndNotifyNewRelic("Exception occured in GetRequestIDsForGroup", exc);
                return new ObjectResult(ResponseWrapper<GetRequestIDsForGroupResponse, RequestServiceErrorCode>.CreateUnsuccessfulResponse(RequestServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };                
            }
        }
    }
}
