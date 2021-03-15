using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public interface IJobFilteringService
    {
        Task<List<JobHeader>> FilterJobHeaders(
            List<JobHeader> jobs,
            string postcode, 
            double? distanceInMiles, 
            Dictionary<SupportActivities, double?> activitySpecificSupportDistancesInMiles,            
            CancellationToken cancellationToken);

        Task<List<JobDTO>> FilterAllJobs(
            List<JobDTO> jobs,
            string postcode,
            double? distanceInMiles,
            Dictionary<SupportActivities, double?> activitySpecificSupportDistancesInMiles,
            CancellationToken cancellationToken);
    }
}
