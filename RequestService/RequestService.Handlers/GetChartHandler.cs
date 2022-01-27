using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.ReportService.Request;
using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.ReportService;
using HelpMyStreet.Utils.Enums;

namespace RequestService.Handlers
{
    public class GetChartHandler : IRequestHandler<GetChartRequest, GetChartResponse>
    {
        private readonly IChartDataService _chartService;

        public GetChartHandler(IChartDataService chartService)
        {
            _chartService = chartService;
        }

        public async Task<GetChartResponse> Handle(GetChartRequest request, CancellationToken cancellationToken)
        {
            GetChartResponse response = new GetChartResponse()
            {
                Chart = new Chart()
            };

            List<DataPoint> dataPoints;

            switch (request.Chart.Chart)
            {
                case Charts.ActivitiesByMonth:
                    dataPoints = await _chartService.GetActivitiesByMonth(request.GroupId);
                    return new GetChartResponse()
                    {
                        Chart = new Chart()
                        {
                            XAxisName = "Month",
                            YAxisName = "Count",
                            DataPoints = dataPoints
                        }
                    };
                case Charts.RequestVolumeByDueDateAndRecentStatus:
                    dataPoints = await _chartService.RequestVolumeByDueDateAndRecentStatus(request.GroupId);
                    return new GetChartResponse()
                    {
                        Chart = new Chart()
                        {
                            XAxisName = "Month",
                            YAxisName = "Count",
                            DataPoints = dataPoints
                        }
                    };
                case Charts.RequestVolumeByActivityType:
                    dataPoints = await _chartService.RequestVolumeByActivity(request.GroupId);
                    return new GetChartResponse()
                    {
                        Chart = new Chart()
                        {
                            XAxisName = "Month",
                            YAxisName = "Count",
                            DataPoints = dataPoints
                        }
                    };
                case Charts.RecentlyActiveVolunteersByVolumeOfAcceptedRequests:
                    dataPoints = await _chartService.RecentActiveVolunteersByVolumeAcceptedRequests(request.GroupId);
                    return new GetChartResponse()
                    {
                        Chart = new Chart()
                        {
                            DataPoints = dataPoints
                        }
                    };
                default:
                    throw new Exception($"Unknown chart type { request.Chart.Chart}");
            }
        }
    }
}
