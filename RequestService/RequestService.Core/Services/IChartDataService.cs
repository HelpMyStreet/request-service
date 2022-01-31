using HelpMyStreet.Contracts.ReportService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public interface IChartDataService
    {
        Task<List<DataPoint>> GetActivitiesByMonth(int groupId, DateTime minDate, DateTime maxDate);
        Task<List<DataPoint>> RequestVolumeByDueDateAndRecentStatus(int groupId, DateTime minDate, DateTime maxDate);
        Task<List<DataPoint>> RequestVolumeByActivity(int groupId, DateTime minDate, DateTime maxDate);
        Task<List<DataPoint>> RecentActiveVolunteersByVolumeAcceptedRequests(int groupId, DateTime minDate, DateTime maxDate);
    }
}
