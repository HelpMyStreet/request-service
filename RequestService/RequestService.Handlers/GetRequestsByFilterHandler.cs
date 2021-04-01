using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using HelpMyStreet.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using RequestService.Core.Exceptions;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.AddressService.Request;
using HelpMyStreet.Utils.EqualityComparers;

namespace RequestService.Handlers
{
    public class GetRequestsByFilterHandler : IRequestHandler<GetRequestsByFilterRequest, GetRequestsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        private readonly IAddressService _addressService;
        private readonly IJobFilteringService _jobFilteringService;
        private IEqualityComparer<JobBasic> _jobBasicDedupeWithDate_EqualityComparer;

        public GetRequestsByFilterHandler(IRepository repository, IGroupService groupService, IAddressService addressService, IJobFilteringService jobFilteringService)
        {
            _repository = repository;
            _groupService = groupService;
            _addressService = addressService;
            _jobFilteringService = jobFilteringService;
            _jobBasicDedupeWithDate_EqualityComparer = new JobBasicDedupeWithDate_EqualityComparer();
        }

        public async Task<GetRequestsByFilterResponse> Handle(GetRequestsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetRequestsByFilterResponse response = null;

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
                if (!(request.JobStatuses?.JobStatuses.Count() == 1 && request.JobStatuses?.JobStatuses.First() == JobStatuses.Open))
                {
                    throw new InvalidFilterException();
                }
            }

            if (request.AllocatedToUserId.HasValue && request.JobStatuses?.JobStatuses.Count() > 0)
            {
                List<JobStatuses> invalidStatuses = new List<JobStatuses>()
                {
                    JobStatuses.Cancelled,
                    JobStatuses.New,
                    JobStatuses.Open
                };

                if (request.JobStatuses.JobStatuses.Any(status => invalidStatuses.Contains(status)))
                {
                    throw new InvalidFilterException();
                }
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

            var requestSummaries = _repository.GetRequestsByFilter(request, referringGroups);

            if (requestSummaries.Count>0)
            {
                await AddPostCodeForLocations(requestSummaries);

                await _jobFilteringService.FilterAllRequests(
                requestSummaries,
                request.Postcode,
                request.DistanceInMiles,
                request.ActivitySpecificSupportDistancesInMiles,
                cancellationToken);

                if (request.ExcludeSiblingsOfJobsAllocatedToUserID.HasValue)
                {
                    var allocatedJobsToUsers = _repository.GetUserJobs(request.ExcludeSiblingsOfJobsAllocatedToUserID.Value);
                    if (allocatedJobsToUsers != null && allocatedJobsToUsers.Count() > 0)
                    {
                        foreach (RequestSummary rs in requestSummaries.ToList())
                        {
                            int jobSummaryMatch = rs.JobSummaries.Count(s => !allocatedJobsToUsers.Contains(s, _jobBasicDedupeWithDate_EqualityComparer));
                            int shiftJobMatch = rs.ShiftJobs.Count(s => !allocatedJobsToUsers.Contains(s, _jobBasicDedupeWithDate_EqualityComparer));

                            if(jobSummaryMatch == rs.JobSummaries.Count && shiftJobMatch == rs.ShiftJobs.Count)
                            {
                                requestSummaries.Remove(rs);
                            }
                        
                        }
                    }
                }
            }

            if (requestSummaries != null)
            {
                response = new GetRequestsByFilterResponse()
                {
                    RequestSummaries = requestSummaries
                };
            }
            else
            {
                throw new Exception("Get requests should never return a null. In the case where no requests are relevant to the filter, an empty list should be returned");
            }
            return response;
        }

        private async Task AddPostCodeForLocations(List<RequestSummary> allRequests)
        {
            //add postcode for locations if locations exist
            List<Location> distinctLocations = allRequests
                .Where(x => x.Shift?.Location != null)
                .Select(x => x.Shift.Location)
                .Distinct().ToList();

            List<LocationDetails> locationDetails = new List<LocationDetails>();

            if (distinctLocations.Count > 0)
            {
                GetLocationsRequest locationRequest = new GetLocationsRequest()
                {
                    LocationsRequests = new HelpMyStreet.Contracts.AddressService.Request.LocationsRequest()
                    {
                        Locations = distinctLocations.ToList()
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
                foreach (RequestSummary rs in allRequests.Where(x => x.Shift?.Location != null))
                {
                    var location = locationDetails.First(x => x.Location == rs.Shift.Location);

                    if (location != null)
                    {
                        rs.PostCode = location.Address.Postcode;
                    }
                }
            }
        }
    }
}
