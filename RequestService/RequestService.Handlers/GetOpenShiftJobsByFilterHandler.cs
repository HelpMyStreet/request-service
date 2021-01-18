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
using System;

namespace RequestService.Handlers
{
    public class GetOpenShiftJobsByFilterHandler : IRequestHandler<GetOpenShiftJobsByFilterRequest, GetOpenShiftJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        public GetOpenShiftJobsByFilterHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetOpenShiftJobsByFilterResponse> Handle(GetOpenShiftJobsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetOpenShiftJobsByFilterResponse response = null;

            var shiftjobs = _repository.GetOpenShiftJobsByFilter(request);

            if (shiftjobs != null)
            {
                response = new GetOpenShiftJobsByFilterResponse()
                {
                    ShiftJobs = shiftjobs
                };
            }
            else
            {
                throw new Exception("Get Open Shift requests should never return a null. In the case where no shifts are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
