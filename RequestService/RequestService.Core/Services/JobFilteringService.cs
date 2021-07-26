using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public class JobFilteringService : IJobFilteringService
    {
        private readonly IJobService _jobService;
        private readonly IGroupService _groupService;

        private static List<GroupSupportActivityRadius> _groupSupportActivityRadii;
        //private List<GroupSupportActivityRadius> _groupSupportActivityRadii;

        public JobFilteringService(IJobService jobService, IGroupService groupService)
        {
            _jobService = jobService;
            _groupService = groupService;

            if(_groupSupportActivityRadii == null)
            {
                _groupSupportActivityRadii = _groupService.GetAllGroupSupportActivityRadii(CancellationToken.None).Result;
            }
        }

        public async Task<List<JobHeader>> FilterJobHeaders(
            List<JobHeader> jobs,            
            string postcode,
            double? distanceInMiles, 
            Dictionary<SupportActivities, double?> activitySpecificSupportDistancesInMiles,            
            CancellationToken cancellationToken)
        {
            bool applyDistanceFilter = false;
            //if postcode has been pased calculate distance between volunteer postcode and jobs
            if (!string.IsNullOrEmpty(postcode))
            {
                jobs = await _jobService.AttachedDistanceToJobHeaders(postcode, jobs, cancellationToken);
                applyDistanceFilter = true;
            }

            if (jobs == null)
            {
                // For now, return no jobs to avoid breaking things downstream
                return new List<JobHeader>();
            }

            if (applyDistanceFilter)
            {
                jobs = jobs.Where(w => w.DistanceInMiles <= GetSupportDistanceForActivity(w.SupportActivity, distanceInMiles, activitySpecificSupportDistancesInMiles))
                        .ToList();
            }

            return jobs;
        }

        public async Task<List<JobDTO>> FilterAllJobs(
            List<JobDTO> jobs,
            string postcode,
            CancellationToken cancellationToken)
        {
            bool applyDistanceFilter = false;
            //if postcode has been pased calculate distance between volunteer postcode and jobs
            if (!string.IsNullOrEmpty(postcode))
            {
                jobs = await _jobService.AttachedDistanceToAllJobs(postcode, jobs, cancellationToken);
                applyDistanceFilter = true;
            }

            if (jobs == null)
            {
                // For now, return no jobs to avoid breaking things downstream
                return new List<JobDTO>();
            }

            if (applyDistanceFilter)
            {
                jobs = jobs.Where(w => w.DistanceInMiles <= GetSupportRadiusForActivity((Groups) w.ReferringGroupID, w.SupportActivity, cancellationToken))
                    .ToList();
            }

            return jobs;
        }

        private double? GetSupportRadiusForActivity(Groups group, SupportActivities supportActivity, CancellationToken cancellationToken)
        {
            double? result = 20d;

            if (_groupSupportActivityRadii != null)
            {
                var radius = _groupSupportActivityRadii.FirstOrDefault(x => x.Group == group && x.SupportActivity == supportActivity);

                if (radius != null)
                {
                    result = radius.SupportRadiusMiles;
                }
            }
            return result;
        }

        private double GetSupportDistanceForActivity(SupportActivities supportActivity, double? distanceInMiles, Dictionary<SupportActivities, double?> activitySpecificSupportDistancesInMiles)
        {
            if (activitySpecificSupportDistancesInMiles != null && activitySpecificSupportDistancesInMiles.ContainsKey(supportActivity))
            {
                return activitySpecificSupportDistancesInMiles[supportActivity] ?? int.MaxValue;
            }
            else
            {
                return distanceInMiles ?? int.MaxValue;
            }
        }
    }
}
