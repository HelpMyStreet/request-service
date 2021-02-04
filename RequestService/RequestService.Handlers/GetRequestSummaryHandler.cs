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
        private readonly IJobService _jobService;
        public GetRequestSummaryHandler(IRepository repository, IJobService jobService)
        {
            _repository = repository;
            _jobService = jobService;
        }

        public async Task<GetRequestSummaryResponse> Handle(GetRequestSummaryRequest request, CancellationToken cancellationToken)
        {
            bool hasPermission = await _jobService.HasPermissionToChangeRequestAsync(request.RequestID, request.AuthorisedByUserID, cancellationToken);
            GetRequestSummaryResponse response = null;
            
            if (hasPermission)
            {
                response = _repository.GetRequestSummary(request.RequestID);
            }
            return response;
        }
    }
}
