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
    public class GetRequestDetailsHandler : IRequestHandler<GetRequestDetailsRequest, GetRequestDetailsResponse>
    {
        private readonly IRepository _repository;
        private readonly IJobService _jobService;
        public GetRequestDetailsHandler(IRepository repository, IJobService jobService)
        {
            _repository = repository;
            _jobService = jobService;
        }

        public async Task<GetRequestDetailsResponse> Handle(GetRequestDetailsRequest request, CancellationToken cancellationToken)
        {
            bool hasPermission = await _jobService.HasPermissionToViewRequestAsync(request.RequestID, request.AuthorisedByUserID, cancellationToken);
            GetRequestDetailsResponse response = null;
            
            if (hasPermission)
            {
                response = _repository.GetRequestDetails(request.RequestID);
            }
            return response;
        }
    }
}
