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

namespace RequestService.Handlers
{
    public class GetJobsByFilterHandler : IRequestHandler<GetJobsByFilterRequest, GetJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IAddressService _addressService;
        private readonly IJobFilteringService _jobFilteringService;
        private readonly IGroupService _groupService;
        public GetJobsByFilterHandler(
            IRepository repository,
            IAddressService addressService,
            IJobFilteringService jobFilteringService,
            IGroupService groupService)
        {
            _repository = repository;
            _addressService = addressService;
            _jobFilteringService = jobFilteringService;
            _groupService = groupService;
        }

        public async Task<GetJobsByFilterResponse> Handle(GetJobsByFilterRequest request, CancellationToken cancellationToken)
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
        
            GetJobsByFilterResponse result = new GetJobsByFilterResponse() { JobHeaders = new List<JobHeader>() };
            
            if(request.Groups != null && request.Groups.Groups.Count == 0)
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

            List<JobHeader> jobHeaders = _repository.GetJobHeaders(request, referringGroups);

            if (jobHeaders.Count == 0)
                return result;

            jobHeaders = await _jobFilteringService.FilterJobHeaders(
                jobHeaders,
                request.Postcode, 
                request.DistanceInMiles, 
                request.ActivitySpecificSupportDistancesInMiles,
                cancellationToken);

            result = new GetJobsByFilterResponse()
            {
                JobHeaders = jobHeaders
            };
            return result;
        }

    }
}
