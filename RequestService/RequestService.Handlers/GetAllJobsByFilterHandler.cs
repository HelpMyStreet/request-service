using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System.Collections.Generic;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Exceptions;
using System.Net.Http;
using System.Linq;
using RequestService.Core.Dto;
using HelpMyStreet.Contracts.AddressService.Request;
using System;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.EqualityComparers;

namespace RequestService.Handlers
{
    public class GetAllJobsByFilterHandler : IRequestHandler<GetAllJobsByFilterRequest, GetAllJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IAddressService _addressService;
        private readonly IJobFilteringService _jobFilteringService;
        private readonly IGroupService _groupService;
        private IEqualityComparer<JobBasic> _jobBasicDedupe_EqualityComparer;

        public GetAllJobsByFilterHandler(
            IRepository repository,
            IAddressService addressService,
            IJobFilteringService jobFilteringService,
            IGroupService groupService)
        {
            _repository = repository;
            _addressService = addressService;
            _jobFilteringService = jobFilteringService;
            _groupService = groupService;
            _jobBasicDedupe_EqualityComparer = new JobBasicDedupe_EqualityComparer();
        }

        private async Task AddPostCodeForLocations(List<JobDTO> allJobs)
        {
            //add postcode for locations if locations exist
            List<Location?> distinctLocations = allJobs
                .Where(x => x.Location != null)
                .Select(x => x.Location)
                .Distinct().ToList();
            List<LocationDetails> locationDetails = new List<LocationDetails>();

            if (distinctLocations.Count > 0)
            {
                GetLocationsRequest locationRequest = new GetLocationsRequest()
                {
                    LocationsRequests = new HelpMyStreet.Contracts.AddressService.Request.LocationsRequest()
                    {
                        Locations = distinctLocations.Select(x => x.Value).ToList()
                    }
                };
                var details = await _addressService.GetLocations(locationRequest);

                if (details != null)
                {
                    locationDetails = details.LocationDetails;
                }
            }

            if (locationDetails.Count > 0)
            {
                foreach (JobDTO j in allJobs.Where(x => x.Location != null))
                {
                    var location = locationDetails.First(x => x.Location == j.Location);

                    if (location != null)
                    {
                        j.PostCode = location.Address.Postcode;
                    }
                }
            }
        }

        public async Task<GetAllJobsByFilterResponse> Handle(GetAllJobsByFilterRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Postcode))
            {
                request.Postcode = HelpMyStreet.Utils.Utils.PostcodeFormatter.FormatPostcode(request.Postcode);

                try
                {
                    var postcodeValid = await _addressService.IsValidPostcode(request.Postcode, cancellationToken);
                }
                catch (HttpRequestException)
                {
                    throw new PostCodeException();
                }
            }

            if (request.ExcludeSiblingsOfJobsAllocatedToUserID.HasValue)
            {
                if (request.JobStatuses?.JobStatuses.Count() == 0 || !request.JobStatuses.JobStatuses.Contains(JobStatuses.Open))
                {
                    throw new InvalidFilterException();
                }
            }

            GetAllJobsByFilterResponse result = new GetAllJobsByFilterResponse() { JobSummaries = new List<JobSummary>(), ShiftJobs = new List<ShiftJob>() };

            if (request.Groups != null && request.Groups.Groups.Count == 0)
            {
                return result;
            }

            List<int> referringGroups = new List<int>();

            if (request.ReferringGroupID.HasValue)
            {
                referringGroups.Add(request.ReferringGroupID.Value);

                if (request.IncludeChildGroups)
                {
                    var childGroups = await _groupService.GetChildGroups(request.ReferringGroupID.Value);

                    if (childGroups.ChildGroups.Count > 0)
                    {
                        referringGroups.AddRange(childGroups.ChildGroups.Select(x => x.GroupId).ToList());
                    }

                }
            }

            List<JobDTO> allJobs = _repository.GetAllJobs(request, referringGroups);

            if (allJobs.Count == 0)
                return result;

            await AddPostCodeForLocations(allJobs);

            allJobs = await _jobFilteringService.FilterAllJobs(
                allJobs,
                request.Postcode,
                request.DistanceInMiles,
                request.ActivitySpecificSupportDistancesInMiles,
                cancellationToken);

            if (request.ExcludeSiblingsOfJobsAllocatedToUserID.HasValue)
            {
                var userRequests = allJobs.Where(x => x.VolunteerUserID == request.ExcludeSiblingsOfJobsAllocatedToUserID.Value).Distinct(_jobBasicDedupe_EqualityComparer);
                allJobs = allJobs.Where(s =>  !userRequests.Contains(s,_jobBasicDedupe_EqualityComparer)).ToList();
            }

            List<JobSummary> jobSummaries = new List<JobSummary>();

            allJobs.Where(x => x.RequestType == RequestType.Task)
            .ToList()
            .ForEach(job => jobSummaries.Add(new JobSummary()
            {
                DateRequested = job.DateRequested,
                VolunteerUserID = job.VolunteerUserID,
                DateStatusLastChanged = job.DateStatusLastChanged,
                ReferringGroupID = job.ReferringGroupID,
                DistanceInMiles = job.DistanceInMiles,
                PostCode = job.PostCode,
                IsHealthCritical = job.IsHealthCritical,
                DueDate = job.DueDate,
                SupportActivity = job.SupportActivity,
                JobStatus = job.JobStatus,
                JobID = job.JobID,
                Archive = job.Archive,
                Reference = job.Reference,
                DueDateType = job.DueDateType,
                RequestID = job.RequestID,
                RequestType = job.RequestType,
                RequestorDefinedByGroup = job.RequestorDefinedByGroup
            }));

            List<ShiftJob> shiftJobs = new List<ShiftJob>();

            allJobs.Where(x => x.RequestType == RequestType.Shift)
            .ToList()
            .ForEach(job => shiftJobs.Add(new ShiftJob()
            {
                DateRequested = job.DateRequested,
                VolunteerUserID = job.VolunteerUserID,
                ReferringGroupID = job.ReferringGroupID,
                SupportActivity = job.SupportActivity,
                JobStatus = job.JobStatus,
                JobID = job.JobID,
                RequestID = job.RequestID,
                RequestType = job.RequestType,
                Location = job.Location.Value,
                ShiftLength = job.ShiftLength.Value,
                StartDate = job.StartDate.Value,
                DistanceInMiles = job.DistanceInMiles
            }));

            result = new GetAllJobsByFilterResponse()
            {
                JobSummaries = jobSummaries,
                ShiftJobs = shiftJobs
            };
            return result;
        }

    }
}
