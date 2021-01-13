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
    public class GetOpenShiftJobsByFilterHandler : IRequestHandler<GetOpenShiftJobsByFilterRequest, GetOpenShiftJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        public GetOpenShiftJobsByFilterHandler(IRepository repository)
        {
            _repository = repository;
        }

        private GetOpenShiftJobsByFilterResponse DummyResponse()
        {
            return new GetOpenShiftJobsByFilterResponse()
            {
                ShiftJobs = new System.Collections.Generic.List<ShiftJob>()
                {
                    new ShiftJob()
                    {
                        JobID = 1,
                        RequestID = 2,
                        VolunteerUserID = 20256,
                        Activity = HelpMyStreet.Utils.Enums.SupportActivities.HealthcareAssistant,
                        Location = HelpMyStreet.Utils.Enums.Location.Location1,
                        JobStatuses = HelpMyStreet.Utils.Enums.JobStatuses.Open,
                        StartDate = new System.DateTime(2021,1,20),
                        ShiftLength = 240                       
                    },
                    new ShiftJob()
                    {
                        JobID = 2,
                        RequestID = 2,
                        VolunteerUserID = 20232,
                        Activity = HelpMyStreet.Utils.Enums.SupportActivities.BackOfficeAdmin,
                        Location = HelpMyStreet.Utils.Enums.Location.Location1,
                        JobStatuses = HelpMyStreet.Utils.Enums.JobStatuses.Open,
                        StartDate = new System.DateTime(2021,1,20),
                        ShiftLength = 240
                    }
                }
            };
        }

        public async Task<GetOpenShiftJobsByFilterResponse> Handle(GetOpenShiftJobsByFilterRequest request, CancellationToken cancellationToken)
        {
            //return DummyResponse();
            GetOpenShiftJobsByFilterResponse response = null;

            var shiftjobs = _repository.GetOpenShiftJobsByFilter(request);

            if (shiftjobs != null)
            {
                response = new GetOpenShiftJobsByFilterResponse()
                {
                    ShiftJobs = shiftjobs
                };
            }
            return response;
        }
    }
}
