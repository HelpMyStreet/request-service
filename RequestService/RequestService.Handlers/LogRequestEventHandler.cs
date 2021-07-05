using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using System.Collections.Generic;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;

namespace RequestService.Handlers
{
    public class LogRequestEventHandler : IRequestHandler<LogRequestEventRequest, LogRequestEventResponse>
    {
        private readonly IRepository _repository;
        public LogRequestEventHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<LogRequestEventResponse> Handle(LogRequestEventRequest request, CancellationToken cancellationToken)
        {
            bool success = await _repository.LogRequestEvent(request.RequestID, request.JobID, request.UserID, request.RequestEventRequest.RequestEvent);
            LogRequestEventResponse response = new LogRequestEventResponse()
            {
                Success = success
            };
            return response;
        }
    }
}
