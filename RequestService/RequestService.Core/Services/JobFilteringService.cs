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

        public JobFilteringService(IJobService jobService, IGroupService groupService)
        {
            _jobService = jobService;
            _groupService = groupService;
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
                // First, ensure cache is fresh in an orderly fashion
                var groups = jobs.Select(j => j.ReferringGroupID).Distinct();

                foreach (int groupId in groups)
                {
                    var activities = jobs.Where(j => j.ReferringGroupID.Equals(groupId)).Select(j => j.SupportActivity).Distinct();

                    foreach (SupportActivities activity in activities)
                    {
                        _ = GetSupportDistanceForActivity(groupId, activity, cancellationToken);
                    }
                }

                // Then hammer the cache, not the Group Service
                jobs = jobs.Where(w => w.DistanceInMiles <= GetSupportDistanceForActivity(w.ReferringGroupID, w.SupportActivity, cancellationToken))
                    .ToList();
            }

            return jobs;
        }

        private double? GetSupportDistanceForActivity(int groupId, SupportActivities supportActivity, CancellationToken cancellationToken)
        {
            return _groupService.GetGroupSupportActivityRadius(groupId, supportActivity, cancellationToken).Result;
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

        public async Task FilterAllRequests(List<RequestSummary> requests, string postcode, double? distanceInMiles, Dictionary<SupportActivities, double?> activitySpecificSupportDistancesInMiles, CancellationToken cancellationToken)
        {
            //if postcode has been pased calculate distance between volunteer postcode and jobs
            if (!string.IsNullOrEmpty(postcode))
            {
                await _jobService.AttachedDistanceToAllRequests(postcode, requests, cancellationToken);

                foreach(RequestSummary rs in requests.ToList())
                {
                    int jobsExceedingDistance = rs.JobSummaries.Count(w => rs.DistanceInMiles > GetSupportDistanceForActivity(w.SupportActivity, distanceInMiles, activitySpecificSupportDistancesInMiles));
                    int shiftsExceedingDistance = rs.ShiftJobs.Count(w => rs.DistanceInMiles > GetSupportDistanceForActivity(w.SupportActivity, distanceInMiles, activitySpecificSupportDistancesInMiles));

                    if (jobsExceedingDistance == rs.JobSummaries.Count && shiftsExceedingDistance == rs.ShiftJobs.Count)
                    {
                        //Either all jobs or shifts exceed the distance for given requests.Therefore remove request from list
                        requests.Remove(rs);
                    }
                }
            }
        }
    }
}
