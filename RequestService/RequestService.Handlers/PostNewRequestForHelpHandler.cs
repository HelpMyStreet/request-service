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
using RequestService.Core.Dto;
using Newtonsoft.Json;
using System;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Exceptions;

namespace RequestService.Handlers
{
    public class PostNewRequestForHelpHandler : IRequestHandler<PostNewRequestForHelpRequest, PostNewRequestForHelpResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IGroupService _groupService;
        private readonly IOptionsSnapshot<ApplicationConfig> _applicationConfig;
        public PostNewRequestForHelpHandler(
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

        public async Task<PostNewRequestForHelpResponse> Handle(PostNewRequestForHelpRequest request, CancellationToken cancellationToken)
        {
            PostNewRequestForHelpResponse response = new PostNewRequestForHelpResponse();

            var requestId = await _repository.GetRequestIDFromGuid(request.HelpRequest.Guid);

            if (requestId > 0)
            {
                return new PostNewRequestForHelpResponse()
                {
                    RequestID = requestId,
                    Fulfillable = Fulfillable.Accepted_ManualReferral
                };
            }

            //add a guid for each job
            foreach (Job j in request.NewJobsRequest.Jobs)
            {
                j.Guid = Guid.NewGuid();
            };

            var formVariant = await _groupService.GetRequestHelpFormVariant(request.HelpRequest.ReferringGroupId, request.HelpRequest.Source, cancellationToken);

            if (formVariant == null)
            {
                return new PostNewRequestForHelpResponse
                {
                    RequestID = -1,
                    Fulfillable = Fulfillable.Rejected_ConfigurationError
                };
            }

            if (formVariant.AccessRestrictedByRole)
            {
                bool failedChecks = request.HelpRequest.CreatedByUserId == 0;

                if (!failedChecks)
                {
                    var groupMember = await _groupService.GetGroupMember(new HelpMyStreet.Contracts.GroupService.Request.GetGroupMemberRequest()
                    {
                        AuthorisingUserId = request.HelpRequest.CreatedByUserId,
                        UserId = request.HelpRequest.CreatedByUserId,
                        GroupId = request.HelpRequest.ReferringGroupId
                    });

                    failedChecks = !groupMember.UserInGroup.GroupRoles.Contains(GroupRoles.RequestSubmitter);
                }

                if (failedChecks)
                {
                    return new PostNewRequestForHelpResponse
                    {
                        RequestID = -1,
                        Fulfillable = Fulfillable.Rejected_Unauthorised
                    };
                }

            }

            if (formVariant.RequestorDefinedByGroup && formVariant.RequestorPersonalDetails != null)
            {
                request.HelpRequest.Requestor = formVariant.RequestorPersonalDetails;
            }
            else
            {
                CopyRequestorAsRecipient(request);
            }
            string postcode = request.HelpRequest.Recipient.Address.Postcode;

            var postcodeValid = await _addressService.IsValidPostcode(postcode, cancellationToken);

            if (!postcodeValid || postcode.Length > 10)
            {
                return new PostNewRequestForHelpResponse
                {
                    RequestID = -1,
                    Fulfillable = Fulfillable.Rejected_InvalidPostcode
                };
            }

            // Currently indicates standard "passed to volunteers" result
            response.Fulfillable = Fulfillable.Accepted_ManualReferral;

            var actions = _groupService.GetNewRequestActions(new HelpMyStreet.Contracts.GroupService.Request.GetNewRequestActionsRequest()
            {
                HelpRequest = request.HelpRequest,
                NewJobsRequest = request.NewJobsRequest
            }, cancellationToken).Result;

            if (actions == null)
            {
                throw new Exception("No new request actions returned");
            }

            try
            {
                var result = await _repository.NewHelpRequestAsync(request, response.Fulfillable, formVariant.RequestorDefinedByGroup);
                response.RequestID = result;

                if (response.RequestID == 0)
                {
                    throw new Exception("Error in saving request");
                }

                foreach (Guid guid in actions.Actions.Keys)
                {
                    int jobID = GetJobIDFromGuid(request,guid);
                    foreach (NewTaskAction newTaskAction in actions.Actions[guid].TaskActions.Keys)
                    {
                        List<int> actionAppliesToIds = actions.Actions[guid].TaskActions[newTaskAction];
                        if (actionAppliesToIds == null) { continue; }

                        switch (newTaskAction)
                        {
                            case NewTaskAction.MakeAvailableToGroups:
                                foreach (int i in actionAppliesToIds)
                                {
                                    await _repository.AddJobAvailableToGroupAsync(jobID, i, cancellationToken);
                                }
                                break;

                            case NewTaskAction.NotifyMatchingVolunteers:
                                foreach (int groupId in actionAppliesToIds)
                                {
                                    bool commsSent = await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                                    {
                                        GroupID = groupId,
                                        CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.SendNewTaskNotification },
                                        JobID = jobID
                                    }, cancellationToken);
                                    await _repository.UpdateCommunicationSentAsync(response.RequestID, commsSent, cancellationToken);
                                }
                                break;

                            case NewTaskAction.AssignToVolunteer:
                                foreach (int userId in actionAppliesToIds)
                                {
                                    await _repository.UpdateJobStatusInProgressAsync(jobID, request.HelpRequest.CreatedByUserId, userId, cancellationToken);
                                }

                                // For now, this only happens with a DIY request
                                response.Fulfillable = Fulfillable.Accepted_DiyRequest;
                                break;
                            case NewTaskAction.SetStatusToOpen:
                                await _repository.UpdateJobStatusOpenAsync(jobID, -1, cancellationToken);
                                break;
                            case NewTaskAction.NotifyGroupAdmins:
                                foreach (int groupId in actionAppliesToIds)
                                {
                                    await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                                    {
                                        GroupID = groupId,
                                        CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.NewTaskPendingApprovalNotification },
                                        JobID = jobID
                                    }, cancellationToken);
                                }
                                break;
                            case NewTaskAction.SendRequestorConfirmation:
                                Dictionary<string, string> additionalParameters = new Dictionary<string, string>
                            {
                                //{ "PendingApproval", (!actions.Actions.Keys.Contains((int)NewTaskAction.SetStatusToOpen)).ToString() }
                                { "PendingApproval", (!actions.Actions[guid].TaskActions.ContainsKey(NewTaskAction.SetStatusToOpen)).ToString() }
                            };
                                await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                                {
                                    CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.RequestorTaskConfirmation },
                                    JobID = jobID,
                                    AdditionalParameters = additionalParameters,
                                }, cancellationToken);
                                break;
                        }
                    }
                }
            }
            catch(DuplicateException exc)
            {
                requestId = await _repository.GetRequestIDFromGuid(request.HelpRequest.Guid);

                if (requestId > 0)
                {
                    return new PostNewRequestForHelpResponse()
                    {
                        RequestID = requestId,
                        Fulfillable = Fulfillable.Accepted_ManualReferral
                    };
                }
            }
            catch(Exception exc)
            {
                throw exc;
            }
            return response;
        }

        private int GetJobIDFromGuid(PostNewRequestForHelpRequest request, Guid guid)
        {
            var job = request.NewJobsRequest.Jobs.FirstOrDefault(x => x.Guid == guid);
            if(job != null)
            {
                return job.JobID;
            }
            else
            {
                throw new Exception($"Unable to Get Job ID From Guid {guid}");
            }
        }
    }
}
