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
        private const int ADMIN_USERID = -1;
        public GetRequestDetailsHandler(IRepository repository, IJobService jobService)
        {
            _repository = repository;
            _jobService = jobService;
        }

        public async Task<GetRequestDetailsResponse> Handle(GetRequestDetailsRequest request, CancellationToken cancellationToken)
        {
            bool hasPermission = true;
            GetRequestDetailsResponse response = null;

            //if (request.AuthorisedByUserID != ADMIN_USERID)
            //{
            //    hasPermission = await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.UserID, true, cancellationToken);
            //}

            if (hasPermission)
            {
                response = _repository.GetRequestDetails(request.RequestID);
            }
            return response;
        }
    }
}
