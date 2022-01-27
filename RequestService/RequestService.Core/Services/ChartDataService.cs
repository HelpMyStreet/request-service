using HelpMyStreet.Contracts.ReportService;
using HelpMyStreet.Utils.Extensions;
using RequestService.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public class ChartDataService: IChartDataService
    {
        private readonly IRepository _repository;

        public ChartDataService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DataPoint>> GetActivitiesByMonth(int groupId)
        {
            DateTime dt = DateTime.UtcNow.Date.AddYears(-1);

            var dataItems = await _repository.GetActivitiesByMonth(groupId, dt);

            //Get distinct activities from list
            var supportActivities = dataItems.Select(x => (HelpMyStreet.Utils.Enums.SupportActivities)x.Series).Distinct().ToList();

            List<DataPoint> dataPoints = new List<DataPoint>();

            //Populate chartitems with support activities for each month
            while (dt < DateTime.UtcNow.Date)
            {
                supportActivities.ForEach(sa =>
                {
                    dataPoints.Add(new DataPoint() { Value = 0, XAxis = $"{dt:yyyy}-{dt:MM}", Series = sa.FriendlyNameShort() });
                });
                dt = dt.AddMonths(1);
            }

            var groupedChartItems = dataItems.GroupBy(g => new { Series = ((HelpMyStreet.Utils.Enums.SupportActivities)g.Series).FriendlyNameShort(), date = $"{g.Date:yyyy}-{g.Date:MM}" })
                    .Select(s => new DataPoint
                    {
                        Value = s.Count(),
                        Series = s.Key.Series,
                        XAxis = s.Key.date
                    }).ToList();

            //override chart items with actual values form the dataset.
            dataPoints.ForEach(item =>
            {
                var matchedItem = groupedChartItems.FirstOrDefault(x => x.XAxis == item.XAxis && x.Series == item.Series);

                if (matchedItem != null)
                {
                    item.Value = matchedItem.Value;
                }
            });

            return dataPoints;
        
        }

        public async Task<List<DataPoint>> RecentActiveVolunteersByVolumeAcceptedRequests(int groupId)
        {
            DateTime dt = DateTime.UtcNow.Date.AddYears(-1);
            var datapoints = await _repository.RecentActiveVolunteersByVolumeAcceptedRequests(groupId, dt);
            throw new NotImplementedException();
        }

        public async Task<List<DataPoint>> RequestVolumeByActivity(int groupId)
        {
            DateTime dt = DateTime.UtcNow.Date.AddYears(-1);
            var datapoints = await _repository.RequestVolumeByActivity(groupId, dt);
            throw new NotImplementedException();
        }

        public async Task<List<DataPoint>> RequestVolumeByDueDateAndRecentStatus(int groupId)
        {
            DateTime dt = DateTime.UtcNow.Date.AddYears(-1);
            var datapoints = await _repository.RequestVolumeByDueDateAndRecentStatus(groupId, dt);
            throw new NotImplementedException();
        }
    }
}
