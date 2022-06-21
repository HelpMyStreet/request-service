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
using System;

namespace RequestService.Handlers
{
    public class GetRequestIDsForGroupHandler : IRequestHandler<GetRequestIDsForGroupRequest, GetRequestIDsForGroupResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        public GetRequestIDsForGroupHandler(
            IRepository repository,
            IGroupService groupService)
        {
            _repository = repository;
            _groupService = groupService;
        }

        public async Task<GetRequestIDsForGroupResponse> Handle(GetRequestIDsForGroupRequest request, CancellationToken cancellationToken)
        {
            GetRequestIDsForGroupResponse response = null;

            List<int> referringGroups = new List<int>()
            {
                request.GroupID
            };

            if (request.IncludeChildGroups)
            {
                var childGroups = await _groupService.GetChildGroups(request.GroupID);

                if (childGroups.ChildGroups.Count > 0)
                {
                    referringGroups.AddRange(childGroups.ChildGroups.Select(x => x.GroupId).ToList());
                }

            }
            

            var requestIds = _repository.GetRequestsIdsForGroup(referringGroups);

            if (requestIds != null)
            {
                response = new GetRequestIDsForGroupResponse()
                {
                    RequestIds = requestIds
                };
            }
            else
            {
                throw new Exception("Get request ids for group should never return a null. In the case where no requests are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
