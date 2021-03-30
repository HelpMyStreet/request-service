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

namespace RequestService.Handlers
{
    public class GetRequestsByFilterHandler : IRequestHandler<GetRequestsByFilterRequest, GetRequestsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        private readonly IAddressService _addressService;

        public GetRequestsByFilterHandler(IRepository repository, IGroupService groupService, IAddressService addressService)
        {
            _repository = repository;
            _groupService = groupService;
            _addressService = addressService;
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
    }
}
