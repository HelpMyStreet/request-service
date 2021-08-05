using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.UserService.Response;

namespace RequestService.Handlers
{
    public class GetJobDetailsHandler : IRequestHandler<GetJobDetailsRequest, GetJobDetailsResponse>
    {
        private readonly IRepository _repository;
        private readonly IJobService _jobService;
        private readonly IUserService _userService;
        private const int ADMIN_USERID = -1;
        public GetJobDetailsHandler(IRepository repository, IJobService jobService, IUserService userService)
        {
            _repository = repository;
            _jobService = jobService;
            _userService = userService;
        }

        public async Task<GetJobDetailsResponse> Handle(GetJobDetailsRequest request, CancellationToken cancellationToken)
        {
            bool hasPermission = true;
            GetJobDetailsResponse response = null;
            GetUserByIDResponse user = null;

            if (request.UserID != ADMIN_USERID)
            {
                hasPermission = await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.UserID, true, cancellationToken);
                user = await _userService.GetUser(request.UserID, cancellationToken);
            }

            if (hasPermission)
            {
                response = _repository.GetJobDetails(request.JobID);
                if (user != null)
                {
                    string postCode = user.User.PostalCode;
                    await _jobService.AttachedDistanceToJobSummaries(postCode, response.RequestSummary.JobSummaries, cancellationToken);

                    if (response.RequestSummary.JobSummaries.Count == 1)
                    {
                        response.JobSummary.DistanceInMiles = response.RequestSummary.JobSummaries[0].DistanceInMiles;
                        response.RequestSummary.DistanceInMiles = response.RequestSummary.JobSummaries[0].DistanceInMiles;
                    }
                }
            }
            return response;
        }
    }
}
