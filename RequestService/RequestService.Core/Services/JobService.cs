﻿using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using HelpMyStreet.Utils.Utils;
using RequestService.Core.Dto;
using RequestService.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserService.Core.Utils;

namespace RequestService.Core.Services
{
    public class JobService : IJobService
    {
        private readonly IRepository _repository;
        private readonly IDistanceCalculator _distanceCalculator;
        private readonly IGroupService _groupService;

        public JobService(
            IDistanceCalculator distanceCalculator,
            IRepository repository,
            IGroupService groupService)
        {
            _repository = repository;
            _distanceCalculator = distanceCalculator;
            _groupService = groupService;
        }

        public async Task<List<JobSummary>> AttachedDistanceToJobSummaries(string volunteerPostCode, List<JobSummary> jobSummaries, CancellationToken cancellationToken)
        {
            if (jobSummaries.Count == 0)
            {
                return null;
            }

            volunteerPostCode = PostcodeFormatter.FormatPostcode(volunteerPostCode);

            List<string> distinctPostCodes = jobSummaries.Select(d => d.PostCode).Distinct().Select(x => PostcodeFormatter.FormatPostcode(x)).ToList();

            if (!distinctPostCodes.Contains(volunteerPostCode))
            {
                distinctPostCodes.Add(volunteerPostCode);
            }

            var postcodeCoordinatesResponse = await _repository.GetLatitudeAndLongitudes(distinctPostCodes, cancellationToken);

            if (postcodeCoordinatesResponse == null)
            {
                return null;
            }

            var volunteerPostcodeCoordinates = postcodeCoordinatesResponse.Where(w => w.Postcode == volunteerPostCode).FirstOrDefault();
            if (volunteerPostcodeCoordinates == null)
            {
                return null;
            }

            foreach (JobSummary jobSummary in jobSummaries)
            {
                var jobPostcodeCoordinates = postcodeCoordinatesResponse.Where(w => w.Postcode == jobSummary.PostCode).FirstOrDefault();
                if (jobPostcodeCoordinates != null)
                {
                    jobSummary.DistanceInMiles = _distanceCalculator.GetDistanceInMiles(volunteerPostcodeCoordinates.Latitude, volunteerPostcodeCoordinates.Longitude, jobPostcodeCoordinates.Latitude, jobPostcodeCoordinates.Longitude);
                }
            }
            return jobSummaries;
        }

        public async Task<List<JobHeader>> AttachedDistanceToJobHeaders(string volunteerPostCode, List<JobHeader> jobHeaders, CancellationToken cancellationToken)
        {
            if (jobHeaders.Count == 0)
            {
                return null;
            }
              
            volunteerPostCode = PostcodeFormatter.FormatPostcode(volunteerPostCode);

            foreach (JobHeader jobHeader in jobHeaders)
            {
                jobHeader.PostCode = PostcodeFormatter.FormatPostcode(jobHeader.PostCode);
            }

            List<string> distinctPostCodes = jobHeaders.Select(d => d.PostCode).Distinct().ToList();

            if (!distinctPostCodes.Contains(volunteerPostCode))
            {
                distinctPostCodes.Add(volunteerPostCode);
            }

            var postcodeCoordinatesResponse = await _repository.GetLatitudeAndLongitudes(distinctPostCodes, cancellationToken);

            if (postcodeCoordinatesResponse == null)
            {
                return null;
            }

            var volunteerPostcodeCoordinates = postcodeCoordinatesResponse.Where(w => w.Postcode == volunteerPostCode).FirstOrDefault();
            if (volunteerPostcodeCoordinates == null)
            {
                return null;
            }

            foreach (JobHeader jobHeader in jobHeaders)
            {
                var jobPostcodeCoordinates = postcodeCoordinatesResponse.Where(w => w.Postcode == jobHeader.PostCode).FirstOrDefault();
                if (jobPostcodeCoordinates != null)
                {
                    jobHeader.DistanceInMiles = _distanceCalculator.GetDistanceInMiles(volunteerPostcodeCoordinates.Latitude, volunteerPostcodeCoordinates.Longitude, jobPostcodeCoordinates.Latitude, jobPostcodeCoordinates.Longitude);
                }
                else
                {
                    jobHeader.DistanceInMiles = double.MaxValue;
                }
            }
            return jobHeaders;
        }

        public async Task<bool> HasPermissionToChangeJobAsync(int jobID, int authorisedByUserID, CancellationToken cancellationToken)
        {
            var jobDetails = _repository.GetJobDetails(jobID);

            if (jobDetails == null)
            {
                throw new Exception($"Unable to retrieve job details for jobID:{jobID}");
            }

            int referringGroupId = await _repository.GetReferringGroupIDForJobAsync(jobID, cancellationToken);

            var userRoles = await _groupService.GetUserRoles(authorisedByUserID, cancellationToken);

            if (userRoles.UserGroupRoles[referringGroupId].Contains((int)GroupRoles.TaskAdmin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> HasPermissionToChangeStatusAsync(int jobID, int createdByUserID, bool allowVolunteerUserId, CancellationToken cancellationToken)
        {
            var jobDetails = _repository.GetJobDetails(jobID);

            if (jobDetails == null)
            {
                throw new Exception($"Unable to retrieve job details for jobID:{jobID}");
            }

            if (allowVolunteerUserId && createdByUserID == jobDetails.JobSummary.VolunteerUserID)
            {
                return true;
            }

            int referringGroupId = await _repository.GetReferringGroupIDForJobAsync(jobID, cancellationToken);

            var userRoles = await _groupService.GetUserRoles(createdByUserID, cancellationToken);

            if (userRoles.UserGroupRoles[referringGroupId].Contains((int)GroupRoles.TaskAdmin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> HasPermissionToViewRequestAsync(int requestID, int authorisedByUserID, CancellationToken cancellationToken)
        {
            var requestDetails = _repository.GetRequestDetails(requestID);

            if (requestDetails == null)
            {
                throw new Exception($"Unable to retrieve request details for requestID:{requestID}");
            }

            if(authorisedByUserID == -1)
            {
                return true;
            }

            int referringGroupId = await _repository.GetReferringGroupIDForRequestAsync(requestID, cancellationToken);

            var userRoles = await _groupService.GetUserRoles(authorisedByUserID, cancellationToken);

            if (userRoles.UserGroupRoles[referringGroupId].Contains((int)GroupRoles.TaskAdmin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
