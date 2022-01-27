using HelpMyStreet.Contracts.ReportService;
using RequestService.Core.Domains;
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
            DateTime minDate = DateTime.UtcNow.Date.AddYears(-1);

            var dataItems = await _repository.GetActivitiesByMonth(groupId, minDate);

            //Get distinct activities from list
            var supportActivities = dataItems.Select(x => x.Series).Distinct().ToList();

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, supportActivities);            
            GroupAndReplaceValuesForKnown(dataPoints, dataItems);
            return dataPoints;
        }

        public async Task<List<DataPoint>> RecentActiveVolunteersByVolumeAcceptedRequests(int groupId)
        {
            DateTime minDate = DateTime.UtcNow.Date.AddMonths(-13);
            var dataItems = await _repository.RecentActiveVolunteersByVolumeAcceptedRequests(groupId, minDate);

            Dictionary<string, (int minValue, int maxValue)> dictCategories = new Dictionary<string, (int minValue, int maxValue)>();
            dictCategories.Add("1 accepted request", (1, 1));
            dictCategories.Add("2-3 accepted requests", (2, 3));
            dictCategories.Add("4-5 accepted requests", (4, 5));
            dictCategories.Add("6-9 accepted requests", (6, 9));
            dictCategories.Add("10+ accepted requests", (10, int.MaxValue));

            List<DataPoint> dataPoints = new List<DataPoint>();

            var groupedByUser = dataItems.GroupBy(x => x.Value)
                        .Select(s => new
                        {
                           UserID = s.Key,
                           Count = s.Count()
                        });

            foreach (var item in dictCategories)
            {
                dataPoints.Add(new DataPoint()
                {
                    XAxis = item.Key,
                    Value = groupedByUser.Count(x=> x.Count >= item.Value.minValue && x.Count <= item.Value.maxValue),
                    Series = "Dataset 1"
                });
            }

            return dataPoints;
        }

        public async Task<List<DataPoint>> RequestVolumeByActivity(int groupId)
        {
            DateTime minDate = DateTime.UtcNow.Date.AddMonths(-13);
            var dataItems = await _repository.RequestVolumeByActivity(groupId, minDate);

            //Get distinct activities from list
            var supportActivities = dataItems.Select(x => x.Series).Distinct();

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, supportActivities);
            GroupAndReplaceValuesForKnown(dataPoints, dataItems);

            return dataPoints;
        }

        public async Task<List<DataPoint>> RequestVolumeByDueDateAndRecentStatus(int groupId)
        {
            DateTime minDate = DateTime.UtcNow.Date.AddMonths(-13);
            var dataItems = await _repository.RequestVolumeByDueDateAndRecentStatus(groupId, minDate);

            dataItems.Where(x =>
                (x.Series.ToLower() != "done" && x.Series.ToLower() != "cancelled")
                && x.Date < DateTime.UtcNow.Date)
                .ToList()
                .ForEach(item =>
                {
                    item.Series = "Overdue";
                });

            dataItems.Where(x => x.Series == "In Progress")
                .ToList()
                .ForEach(item =>
                {
                    item.Series = "Accepted";
                });

            var allJobStatuses = new List<string>
                {
                    "Pending Approval",
                    "Open",
                    "Accepted",
                    "Done",
                    "Overdue",
                    "Cancelled"
                };

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, allJobStatuses);
            GroupAndReplaceValuesForKnown(dataPoints, dataItems);

            return dataPoints;
        }

        private void GroupAndReplaceValuesForKnown(List<DataPoint> dataPoints, IEnumerable<DataItem> dataItems)
        {
            var groupedChartItems = dataItems.GroupBy(g => new { g.Series, date = $"{g.Date:yyyy}-{g.Date:MM}" })
                    .Select(s => new DataPoint
                    {
                        Value = s.Count(),
                        Series = s.Key.Series,
                        XAxis = s.Key.date
                    });

            //override chart items with actual values form the dataset.
            dataPoints.ForEach(item =>
            {
                var matchedItem = groupedChartItems.FirstOrDefault(x => x.XAxis == item.XAxis && x.Series == item.Series);

                if (matchedItem != null)
                {
                    item.Value = matchedItem.Value;
                }
            });
        }

        private List<DataPoint> PopulateListWithDefaultValues(DateTime dt, IEnumerable<string> series)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            while (dt <= DateTime.UtcNow.Date)
            {
                series.ToList().ForEach(sa =>
                {
                    dataPoints.Add(new DataPoint() { Value = 0, XAxis = $"{dt:yyyy}-{dt:MM}", Series = sa });
                });
                dt = dt.AddMonths(1);
            }

            return dataPoints;
        }
    }
}
