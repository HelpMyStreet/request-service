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

namespace RequestService.Handlers
{
    public class GetShiftRequestsByFilterHandler : IRequestHandler<GetShiftRequestsByFilterRequest, GetShiftRequestsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;

        public GetShiftRequestsByFilterHandler(IRepository repository, IGroupService groupService)
        {
            _repository = repository;
            _groupService = groupService;
        }

        public async Task<GetShiftRequestsByFilterResponse> Handle(GetShiftRequestsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetShiftRequestsByFilterResponse response = null;

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

            var requestSummaries = _repository.GetShiftRequestsByFilter(request, referringGroups);

            if (requestSummaries != null)
            {
                response = new GetShiftRequestsByFilterResponse()
                {
                    RequestSummaries = requestSummaries
                };
            }
            else
            {
                throw new Exception("Get Shift requests should never return a null. In the case where no shifts are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
