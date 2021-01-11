﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System.Collections.Generic;
using System.Linq;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using System;

namespace RequestService.Handlers
{
    public class PostNewShiftsHandler : IRequestHandler<PostNewShiftsRequest, PostNewShiftsResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        public PostNewShiftsHandler(
            IRepository repository,          
            IGroupService groupService)
        {
            _repository = repository;
            _groupService = groupService;
        }

        public async Task<PostNewShiftsResponse> Handle(PostNewShiftsRequest request, CancellationToken cancellationToken)
        {
            PostNewShiftsResponse response = new PostNewShiftsResponse();

            var formVariant = await _groupService.GetRequestHelpFormVariant(request.ReferringGroupId, request.Source, cancellationToken);

            if (formVariant == null)
            {
                return new PostNewShiftsResponse
                {
                    RequestID = -1,
                    Fulfillable = Fulfillable.Rejected_ConfigurationError
                };
            }

            if (formVariant.AccessRestrictedByRole)
            {
                bool failedChecks = request.CreatedByUserId == 0;

                if (!failedChecks)
                {
                    var groupMember = await _groupService.GetGroupMember(new HelpMyStreet.Contracts.GroupService.Request.GetGroupMemberRequest()
                    {
                        AuthorisingUserId = request.CreatedByUserId,
                        UserId = request.CreatedByUserId,
                        GroupId = request.ReferringGroupId
                    });

                    failedChecks = !groupMember.UserInGroup.GroupRoles.Contains(GroupRoles.RequestSubmitter);
                }

                if (failedChecks)
                {
                    return new PostNewShiftsResponse
                    {
                        RequestID = -1,
                        Fulfillable = Fulfillable.Rejected_Unauthorised
                    };
                }

            }

            // Currently indicates standard "passed to volunteers" result
            response.Fulfillable = Fulfillable.Accepted_ManualReferral;

            var result = await _repository.NewShiftsRequestAsync(request, response.Fulfillable, formVariant.RequestorDefinedByGroup);
            response.RequestID = result;
            return response;
        }
    }
}
