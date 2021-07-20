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
using RequestService.Core.Dto;
using Newtonsoft.Json;
using System;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Exceptions;
using HelpMyStreet.Utils.Extensions;
using RequestService.Handlers.BusinessLogic;
using System.Text;
using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;

namespace RequestService.Handlers
{
    public class PostRequestForHelpHandler : IRequestHandler<PostRequestForHelpRequest, PostRequestForHelpResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IGroupService _groupService;
        private readonly IOptionsSnapshot<ApplicationConfig> _applicationConfig;
        private readonly IMultiJobs _multiJobs;
        public PostRequestForHelpHandler(
            IRepository repository,
            IUserService userService,
            IAddressService addressService,
            ICommunicationService communicationService,
            IGroupService groupService,
            IOptionsSnapshot<ApplicationConfig> applicationConfig,
            IMultiJobs multiJobs)
        {
            _repository = repository;
            _userService = userService;
            _addressService = addressService;
            _communicationService = communicationService;
            _groupService = groupService;
            _applicationConfig = applicationConfig;
            _multiJobs = multiJobs;
        }

        private void AddMultiAndRepeats(PostRequestForHelpRequest request)
        {
            var firstHelpRequestDetail = request.HelpRequestDetails.First();
            var firstJob = firstHelpRequestDetail.NewJobsRequest.Jobs.First();
            var requestType = firstJob.SupportActivity.RequestType();

            if (requestType == RequestType.Shift &&
                firstJob.NumberOfRepeats > 1 &&
                firstJob.RepeatFrequency != Frequency.Once)
            {
                _multiJobs.AddShiftRepeats(request.HelpRequestDetails, firstJob.NumberOfRepeats);
            }
            else
            {
                _multiJobs.AddMultiVolunteers(firstHelpRequestDetail.NewJobsRequest);
                _multiJobs.AddRepeats(firstHelpRequestDetail.NewJobsRequest, DateTime.UtcNow);
            }

            foreach (HelpRequestDetail helpRequestDetail in request.HelpRequestDetails)
            {
                foreach (Job j in helpRequestDetail.NewJobsRequest.Jobs)
                {
                    //add a guid for each job
                    if (j.Guid == Guid.Empty)
                    {
                        j.Guid = Guid.NewGuid();
                    }
                };
            }
        }

        private void HandleASAP(PostRequestForHelpRequest request)
        {
            foreach (Job j in request.HelpRequestDetails.First().NewJobsRequest.Jobs.Where(x => x.DueDateType == DueDateType.ASAP))
            {
                DateTime now = DateTime.UtcNow;
                j.NotBeforeDate = now;
                switch (j.RepeatFrequency)
                {
                    case Frequency.Once:
                        j.StartDate = now.AddDays(3);
                        break;
                    case Frequency.Daily:
                        j.StartDate = now;
                        break;
                    case Frequency.Weekly:
                    case Frequency.Fortnightly:
                    case Frequency.EveryFourWeeks:
                        j.StartDate = now.AddDays(3);
                        break;
                    default:
                        throw new Exception($"Invalid Frequency for DueDate.ASAP {j.RepeatFrequency}");
                }
            };
        }

        private async Task<bool> FailedChecks(bool accessRestrictedByRole, HelpRequest helpRequest, CancellationToken cancellationToken)
        {
            bool failedChecks = false;
            if (accessRestrictedByRole)
            {
                failedChecks = helpRequest.CreatedByUserId == 0;

                if (!failedChecks)
                {
                    var groupMember = await _groupService.GetGroupMember(new HelpMyStreet.Contracts.GroupService.Request.GetGroupMemberRequest()
                    {
                        AuthorisingUserId = helpRequest.CreatedByUserId,
                        UserId = helpRequest.CreatedByUserId,
                        GroupId = helpRequest.ReferringGroupId
                    });

                    failedChecks = !groupMember.UserInGroup.GroupRoles.Contains(GroupRoles.RequestSubmitter);

                    if (failedChecks)
                    {
                        //check if user has request submitter role with parant group
                        var userRoles = await _groupService.GetUserRoles(helpRequest.CreatedByUserId, cancellationToken);

                        var group = await _groupService.GetGroup(helpRequest.ReferringGroupId);
                        int? parentGroupId = group.Group.ParentGroupId;

                        if (parentGroupId.HasValue && userRoles.UserGroupRoles.ContainsKey(parentGroupId.Value))
                        {
                            failedChecks = !(userRoles.UserGroupRoles[parentGroupId.Value].Contains((int)GroupRoles.RequestSubmitter));
                        }
                    }
                }


            }
            return failedChecks;
        }

        private void CopyRequestorAsRecipient(HelpRequest request, bool requestorDefinedByGroup, RequestPersonalDetails requestPersonalDetails )
        {
            if (requestorDefinedByGroup && requestPersonalDetails != null)
            {
                request.Requestor = requestPersonalDetails;
            }
            else
            {
                request.Requestor.Address.Postcode = HelpMyStreet.Utils.Utils.PostcodeFormatter.FormatPostcode(request.Requestor.Address.Postcode);

                if (request.RequestorType == RequestorType.Myself)
                {
                    request.Recipient = request.Requestor;
                }
                else
                {
                    request.Recipient.Address.Postcode = HelpMyStreet.Utils.Utils.PostcodeFormatter.FormatPostcode(request.Recipient.Address.Postcode);
                }
            }
        }

        private async Task<bool> InvalidPostCode(HelpRequest helpRequest, CancellationToken cancellationToken)
        {
            bool returnValue = false;
            if (!string.IsNullOrEmpty(helpRequest.Recipient?.Address?.Postcode))
            {
                string postcode = helpRequest.Recipient.Address.Postcode;

                var postcodeValid = await _addressService.IsValidPostcode(postcode, cancellationToken);

                if (!postcodeValid || postcode.Length > 10)
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }

        private PostRequestForHelpResponse GetPostRequestForHelpResponse(List<int> requestIDs, Fulfillable fulfillable)
        {
            return new PostRequestForHelpResponse()
            {
                RequestIDs = requestIDs,
                Fulfillable = fulfillable
            };
        }

        private async Task<bool?> SuppressRecipientPersonalDetails(HelpRequestDetail helpRequestDetail,  GetRequestHelpFormVariantResponse formVariant)
        {
            bool? suppressRecipientPersonalDetails = formVariant.SuppressRecipientPersonalDetails;

            Job firstJob = helpRequestDetail.NewJobsRequest.Jobs.First();
            var suppressRecipientPersonalDetailsQuestion = firstJob.Questions?.Where(x => x.Id == (int)Questions.SuppressRecipientPersonalDetails).FirstOrDefault();

            if (suppressRecipientPersonalDetailsQuestion != null)
            {
                suppressRecipientPersonalDetails = suppressRecipientPersonalDetailsQuestion.Answer.First().ToString() == "Y";
            }

            return suppressRecipientPersonalDetails;
        }

        private async Task<int> ProcessRequest(HelpRequestDetail helpRequestDetail, 
            GetRequestHelpFormVariantResponse formVariant, 
            Fulfillable fulfillable,
            bool suppressRecipientPersonalDetails, 
            bool multiVolunteers,
            bool repeat, CancellationToken cancellationToken)
        {
            var actions = _groupService.GetNewRequestActions(new GetNewRequestActionsRequest()
            {
                HelpRequest = helpRequestDetail.HelpRequest,
                NewJobsRequest = helpRequestDetail.NewJobsRequest
            }, cancellationToken).Result;

            if (actions == null)
            {
                throw new Exception("No new request actions returned");
            }

            return  await _repository.NewHelpRequestAsync(new PostNewRequestForHelpRequest()
            {
                HelpRequest = helpRequestDetail.HelpRequest,
                NewJobsRequest = helpRequestDetail.NewJobsRequest
            }, fulfillable, formVariant.RequestorDefinedByGroup, suppressRecipientPersonalDetails, multiVolunteers, repeat);
        }

        public async Task<PostRequestForHelpResponse> Handle(PostRequestForHelpRequest request, CancellationToken cancellationToken)
        {
            PostRequestForHelpResponse response = new PostRequestForHelpResponse()
            {
                RequestIDs = new List<int>()
            };
            var firstHelpRequestDetail = request.HelpRequestDetails.First();
            bool invalidPostCode = await InvalidPostCode(firstHelpRequestDetail.HelpRequest, cancellationToken);

            if(invalidPostCode)
            {
                return GetPostRequestForHelpResponse(new List<int> { -1 }, Fulfillable.Rejected_InvalidPostcode);
            }

            bool multiVolunteers = firstHelpRequestDetail.MultiVolunteer;
            bool repeat = firstHelpRequestDetail.Repeat;

            HandleASAP(request);
            AddMultiAndRepeats(request);
           

            var json = JsonConvert.SerializeObject(request);

            var requestId = await _repository.GetRequestIDFromGuid(firstHelpRequestDetail.HelpRequest.Guid);

            if (requestId > 0)
            {
                return GetPostRequestForHelpResponse(new List<int> { requestId }, Fulfillable.Accepted_ManualReferral);
            }

            var formVariant = await _groupService.GetRequestHelpFormVariant(firstHelpRequestDetail.HelpRequest.ReferringGroupId, firstHelpRequestDetail.HelpRequest.Source, cancellationToken);

            if (formVariant == null)
            {
                return GetPostRequestForHelpResponse(new List<int> { -1 }, Fulfillable.Rejected_ConfigurationError);
            }

            var failedChecks = await FailedChecks(formVariant.AccessRestrictedByRole, firstHelpRequestDetail.HelpRequest, cancellationToken);

            if(failedChecks)
            {
                return GetPostRequestForHelpResponse(new List<int> { -1 }, Fulfillable.Rejected_Unauthorised);
            }

            CopyRequestorAsRecipient(firstHelpRequestDetail.HelpRequest, formVariant.RequestorDefinedByGroup, formVariant.RequestorPersonalDetails);

            // Currently indicates standard "passed to volunteers" result
            response.Fulfillable = Fulfillable.Accepted_ManualReferral;

            bool? suppressRecipientPersonalDetails = await SuppressRecipientPersonalDetails(firstHelpRequestDetail, formVariant);

            if (!suppressRecipientPersonalDetails.HasValue)
            {
                return GetPostRequestForHelpResponse(new List<int> { -1 }, Fulfillable.Rejected_ConfigurationError);
            }

            foreach (HelpRequestDetail helpRequestDetail in request.HelpRequestDetails)
            {
               response.RequestIDs.Add(await ProcessRequest(helpRequestDetail, formVariant, response.Fulfillable, suppressRecipientPersonalDetails.Value, multiVolunteers, repeat, cancellationToken));
            }
            

            return response;
        }
    }
}
   