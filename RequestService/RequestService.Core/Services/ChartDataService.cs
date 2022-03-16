using HelpMyStreet.Contracts.ReportService;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using Microsoft.Extensions.Internal;
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
        private readonly IGroupService _groupService;
        private readonly ISystemClock _mockableDateTime;

        public ChartDataService(IRepository repository, IGroupService groupService, ISystemClock mockableDateTime)
        {
            _repository = repository;
            _groupService = groupService;
            _mockableDateTime = mockableDateTime;
        }

        private async Task<List<int>> GetGroups(int groupId)
        {
            List<int> groups = new List<int>()
            {
                groupId
            };
            
            var childGroups = await _groupService.GetChildGroups(groupId);
            groups.AddRange(childGroups.ChildGroups.Select(sm => sm.GroupId));

            return groups;
        }

        public async Task<List<DataPoint>> GetActivitiesByMonth(int groupId, DateTime minDate, DateTime maxDate)
        {
            var groups = await GetGroups(groupId);
            var dataItems = await _repository.GetActivitiesByMonth(groups, minDate, maxDate);

            //Get distinct activities from list
            var supportActivities = dataItems.Select(x => x.SupportActivity.FriendlyNameShort()).Distinct().ToList();

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, maxDate, supportActivities);
            GroupAndReplaceValuesForKnown(
                dataPoints, 
                dataItems.Select(x => new DataItem() 
                { 
                    Date = x.DateRequested.Date, 
                    Series = x.SupportActivity.FriendlyNameShort()
                }).ToList());
            return dataPoints;
        }

        public async Task<List<DataPoint>> RecentActiveVolunteersByVolumeAcceptedRequests(int groupId, DateTime minDate, DateTime maxDate)
        {
            var groups = await GetGroups(groupId);
            var dataItems = await _repository.RecentActiveVolunteersByVolumeAcceptedRequests(groups, minDate, maxDate);

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

        public async Task<List<DataPoint>> RequestVolumeByActivity(int groupId, DateTime minDate, DateTime maxDate)
        {
            var groups = await GetGroups(groupId);
            var dataItems = await _repository.RequestVolumeByActivity(groups, minDate, maxDate);

            //Get distinct activities from list
            var supportActivities = dataItems.Select(x => x.SupportActivity.FriendlyNameShort()).Distinct().ToList();

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, maxDate, supportActivities);
            GroupAndReplaceValuesForKnown(
               dataPoints,
               dataItems.Select(x => new DataItem()
               {
                   Date = x.DueDate,
                   Series = x.SupportActivity.FriendlyNameShort()
               }).ToList());

            return dataPoints;
        }

        public async Task<List<DataPoint>> RequestVolumeByDueDateAndRecentStatus(int groupId, DateTime minDate, DateTime maxDate)
        {
            var groups = await GetGroups(groupId);
            var jobSummaries = await _repository.RequestVolumeByDueDateAndRecentStatus(groups, minDate, maxDate);

            var dataItems = jobSummaries.Select(x => new DataItem()
            {
                Series = x.JobStatus.FriendlyName(),
                Date = x.DueDate
            }).ToList();

            dataItems.Where(x =>
                (x.Series!= JobStatuses.Done.FriendlyName() && x.Series != JobStatuses.Cancelled.FriendlyName())
                && x.Date < _mockableDateTime.UtcNow)
                .ToList()
                .ForEach(item =>
                {
                    item.Series = "Overdue";
                });

            dataItems.Where(x => x.Series == JobStatuses.InProgress.FriendlyName())
                .ToList()
                .ForEach(item =>
                {
                    item.Series = JobStatuses.Accepted.FriendlyName();
                });

            var allJobStatuses = new List<string>
                {
                    JobStatuses.New.FriendlyName(),
                    JobStatuses.Open.FriendlyName(),
                    JobStatuses.Accepted.FriendlyName(),
                    JobStatuses.Done.FriendlyName(),
                    "Overdue",
                    JobStatuses.Cancelled.FriendlyName()
                };

            List<DataPoint> dataPoints = PopulateListWithDefaultValues(minDate, maxDate, allJobStatuses);
            GroupAndReplaceValuesForKnown(dataPoints, dataItems);

            return dataPoints;
        }

        private void GroupAndReplaceValuesForKnown(List<DataPoint> dataPoints, List<DataItem> dataItems)
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

        private List<DataPoint> PopulateListWithDefaultValues(DateTime minDate, DateTime maxDate, IEnumerable<string> series)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            while (minDate <= maxDate)
            {
                series.ToList().ForEach(sa =>
                {
                    dataPoints.Add(new DataPoint() { Value = 0, XAxis = $"{minDate:yyyy}-{minDate:MM}", Series = sa });
                });
                minDate = minDate.AddMonths(1);
            }

            return dataPoints;
        }
    }
}
