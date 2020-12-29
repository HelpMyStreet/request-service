using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using HelpMyStreet.Utils.Models;

namespace RequestService.Handlers
{
    public class GetUserShiftsHandler : IRequestHandler<GetUserShiftsRequest, GetUserShiftsResponse>
    {
        private readonly IRepository _repository;
        public GetUserShiftsHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUserShiftsResponse> Handle(GetUserShiftsRequest request, CancellationToken cancellationToken)
        {
            GetUserShiftsResponse response = null;

            var shifts = _repository.GetUserShifts(request);

            if(shifts != null)
            {
                response = new GetUserShiftsResponse()
                {
                    Shifts = shifts
                };
            }

            //if (request.UserID != ADMIN_USERID)
            //{
            //    hasPermission = await _jobService.HasPermissionToChangeStatusAsync(request.JobID, request.UserID, true, cancellationToken);
            //}

            //if (hasPermission)
            //{
            //    response = _repository.GetJobDetails(request.JobID);
            //}
            return response;
        }
    }
}
