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
    public class GetOpenShiftJobsByFilterHandler : IRequestHandler<GetOpenShiftJobsByFilterRequest, GetOpenShiftJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        public GetOpenShiftJobsByFilterHandler(IRepository repository, IGroupService groupService)
        {
            _repository = repository;
            _groupService = groupService;
        }

        public async Task<GetOpenShiftJobsByFilterResponse> Handle(GetOpenShiftJobsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetOpenShiftJobsByFilterResponse response = null;

            List<int> referringGroups = new List<int>();

            if(request.ReferringGroupID.HasValue)
            {
                referringGroups.Add(request.ReferringGroupID.Value);

                if (request.IncludeChildGroups)
                {
                    var childGroups = await _groupService.GetChildGroups(request.ReferringGroupID.Value);

                    if(childGroups.ChildGroups.Count>0)
                    {
                        referringGroups.AddRange(childGroups.ChildGroups.Select(x => x.GroupId).ToList());
                    }

                }
            }

            var shiftjobs = _repository.GetOpenShiftJobsByFilter(request, referringGroups);

            if (shiftjobs != null)
            {
                response = new GetOpenShiftJobsByFilterResponse()
                {
                    ShiftJobs = shiftjobs
                };
            }
            else
            {
                throw new Exception("Get Open Shift requests should never return a null. In the case where no shifts are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
