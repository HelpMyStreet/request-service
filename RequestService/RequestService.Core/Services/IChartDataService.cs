using HelpMyStreet.Contracts.ReportService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public interface IChartDataService
    {
        Task<List<DataPoint>> GetActivitiesByMonth(int groupId);
        Task<List<DataPoint>> RequestVolumeByDueDateAndRecentStatus(int groupId);
        Task<List<DataPoint>> RequestVolumeByActivity(int groupId);
        Task<List<DataPoint>> RecentActiveVolunteersByVolumeAcceptedRequests(int groupId);
    }
}
