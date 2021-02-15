using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;

namespace RequestService.Handlers
{
    public class GetRequestSummaryHandler : IRequestHandler<GetRequestSummaryRequest, GetRequestSummaryResponse>
    {
        private readonly IRepository _repository;
        public GetRequestSummaryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetRequestSummaryResponse> Handle(GetRequestSummaryRequest request, CancellationToken cancellationToken)
        {
            return _repository.GetRequestSummary(request.RequestID);            
        }
    }
}
