using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using System.Linq;
using System;

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
            var requestDetails = _repository.GetRequestDetails(request.RequestID);

            if(requestDetails.RequestSummary==null)
            {
                throw new Exception($"Unable to find request details for {request.RequestID}");
            }

            var volunteers = requestDetails.RequestSummary.JobBasics
                .Where(x => x.VolunteerUserID.HasValue)
                .Select(x => x.VolunteerUserID)
                .Distinct().ToList();

            bool hasPermission = await _jobService.HasPermissionToChangeRequestAsync(request.RequestID, request.AuthorisedByUserID, cancellationToken);
            GetRequestDetailsResponse response = null;
            
            if (hasPermission || volunteers.Contains(request.AuthorisedByUserID))
            {
                response = requestDetails;
            }
            return response;
        }
    }
}
