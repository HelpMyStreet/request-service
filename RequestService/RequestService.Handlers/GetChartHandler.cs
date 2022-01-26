
using RequestService.Core.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpMyStreet.Contracts;
using RequestService.Core.Domains;
using HelpMyStreet.Utils.Extensions;
using System.Linq;
using System;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.ReportService.Request;
using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.ReportService;

namespace RequestService.Handlers
{
    public class GetChartHandler : IRequestHandler<GetChartRequest, GetChartResponse>
    {
        private readonly IRepository _repository;

        public GetChartHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetChartResponse> Handle(GetChartRequest request, CancellationToken cancellationToken)
        {
            GetChartResponse response = new GetChartResponse()
            {
                Chart = new Chart()
            };

            switch(request.Chart.Chart)
            {
                case HelpMyStreet.Utils.Enums.Charts.ActivitiesByMonth:
                    var activitiesByMonth = await _repository.GetActivitiesByMonth(request.GroupId);
                    response.Chart = activitiesByMonth;
                    break;
                case HelpMyStreet.Utils.Enums.Charts.RequestVolumeByDueDateAndRecentStatus:
                    var requestVolumes = await _repository.RequestVolumeByDueDateAndRecentStatus(request.GroupId);
                    response.Chart = requestVolumes;
                    break;
                case HelpMyStreet.Utils.Enums.Charts.RequestVolumeByActivityType:
                    var volumeByActivity = await _repository.RequestVolumeByActivity(request.GroupId);
                    response.Chart = volumeByActivity;
                    break;
                case HelpMyStreet.Utils.Enums.Charts.RecentlyActiveVolunteersByVolumeOfAcceptedRequests:
                    var recentActiveVolunteers = await _repository.RecentActiveVolunteersByVolumeAcceptedRequests(request.GroupId);
                    response.Chart = recentActiveVolunteers;
                    break;
                default:
                    throw new Exception($"Unknown chart type { request.Chart.Chart}");
            }

            return response;
        }
    }
}
