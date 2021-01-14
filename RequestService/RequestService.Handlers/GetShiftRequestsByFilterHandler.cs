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

            //var shiftjobs = _repository.GetOpenShiftJobsByFilter(request);

            //if (shiftjobs != null)
            //{
            //    response = new GetShiftRequestsByFilterResponse()
            //    {
            //        ShiftJobs = shiftjobs
            //    };
            //}
            return response;
        }
    }
}
