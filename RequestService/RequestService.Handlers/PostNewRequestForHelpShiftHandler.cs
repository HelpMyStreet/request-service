using MediatR;
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
    public class PostNewRequestForHelpShiftHandler : IRequestHandler<PostNewRequestForHelpShiftRequest, PostNewRequestForHelpShiftResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IGroupService _groupService;
        private readonly IOptionsSnapshot<ApplicationConfig> _applicationConfig;
        public PostNewRequestForHelpShiftHandler(
            IRepository repository,
            IUserService userService,
            IAddressService addressService,
            ICommunicationService communicationService,
            IGroupService groupService,
            IOptionsSnapshot<ApplicationConfig> applicationConfig)
        {
            _repository = repository;
            _userService = userService;
            _addressService = addressService;
            _communicationService = communicationService;
            _groupService = groupService;
            _applicationConfig = applicationConfig;
        }

        private void CopyRequestorAsRecipient(PostNewRequestForHelpRequest request)
        {
            request.HelpRequest.Requestor.Address.Postcode = HelpMyStreet.Utils.Utils.PostcodeFormatter.FormatPostcode(request.HelpRequest.Requestor.Address.Postcode);

            if (request.HelpRequest.RequestorType == RequestorType.Myself)
            {
                request.HelpRequest.Recipient = request.HelpRequest.Requestor;                
            }
            else
            {
                request.HelpRequest.Recipient.Address.Postcode = HelpMyStreet.Utils.Utils.PostcodeFormatter.FormatPostcode(request.HelpRequest.Recipient.Address.Postcode);
            }
        }

        public async Task<PostNewRequestForHelpShiftResponse> Handle(PostNewRequestForHelpShiftRequest request, CancellationToken cancellationToken)
        {
            PostNewRequestForHelpShiftResponse response = new PostNewRequestForHelpShiftResponse();

            var formVariant = await _groupService.GetRequestHelpFormVariant(request.ReferringGroupId, request.Source, cancellationToken);

            if (formVariant == null)
            {
                return new PostNewRequestForHelpShiftResponse
                {
                    RequestID = -1,
                    Fulfillable = Fulfillable.Rejected_ConfigurationError
                };
            }

            //if(formVariant.AccessRestrictedByRole)
            //{
            //    bool failedChecks = request.CreatedByUserId == 0;

            //    if(!failedChecks)
            //    {
            //        var groupMember = await _groupService.GetGroupMember(new HelpMyStreet.Contracts.GroupService.Request.GetGroupMemberRequest()
            //        {
            //            AuthorisingUserId = request.CreatedByUserId,
            //            UserId = request.CreatedByUserId,
            //            GroupId = request.ReferringGroupId
            //        });

            //        failedChecks = !groupMember.UserInGroup.GroupRoles.Contains(GroupRoles.RequestSubmitter);
            //    }

            //    if (failedChecks)
            //    {
            //        return new PostNewRequestForHelpShiftResponse
            //        {
            //            RequestID = -1,
            //            Fulfillable = Fulfillable.Rejected_Unauthorised
            //        };
            //    }

            //}

            // Currently indicates standard "passed to volunteers" result
            response.Fulfillable = Fulfillable.Accepted_ManualReferral;

            var result = await _repository.NewHelpShiftRequestAsync(request, response.Fulfillable, formVariant.RequestorDefinedByGroup);
            response.RequestID = result;
            return response;
        }
    }
}
