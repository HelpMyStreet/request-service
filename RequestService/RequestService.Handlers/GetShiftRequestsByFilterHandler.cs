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
    public class GetShiftRequestsByFilterHandler : IRequestHandler<GetShiftRequestsByFilterRequest, GetShiftRequestsByFilterResponse>
    {
        private readonly IRepository _repository;
        public GetShiftRequestsByFilterHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetShiftRequestsByFilterResponse> Handle(GetShiftRequestsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetShiftRequestsByFilterResponse response = null;

            var shiftRequests = _repository.GetShiftRequestsByFilter(request);

            if (shiftRequests != null)
            {
                response = new GetShiftRequestsByFilterResponse()
                {
                    ShiftRequests = shiftRequests
                };
            }
            else
            {
                throw new Exception("Get Shift requests should never return a null. In the case where no shifts are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
