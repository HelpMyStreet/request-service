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
using RequestService.Core.Exceptions;

namespace RequestService.Handlers
{
    public class PostNewShiftsHandler : IRequestHandler<PostNewShiftsRequest, PostNewShiftsResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        private readonly ICommunicationService _communicationService;
        public PostNewShiftsHandler(
            IRepository repository,          
            IGroupService groupService,
            ICommunicationService communicationService)
        {
            _repository = repository;
            _groupService = groupService;
            _communicationService = communicationService;
        }

        public async Task<PostNewShiftsResponse> Handle(PostNewShiftsRequest request, CancellationToken cancellationToken)
        {
            PostNewShiftsResponse response = new PostNewShiftsResponse();

            var requestId = await _repository.GetRequestIDFromGuid(request.Guid);

            if (requestId > 0)
            {
                return new PostNewShiftsResponse()
                {
                    RequestID = requestId,
                    Fulfillable = Fulfillable.Accepted_ManualReferral
                };
            }

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

            var actions = _groupService.GetNewShiftActions(new HelpMyStreet.Contracts.GroupService.Request.GetNewShiftActionsRequest()
            {
                ReferringGroupId = request.ReferringGroupId,
                Source = request.Source
            }, cancellationToken).Result;

            if (actions == null)
            {
                throw new Exception("No new shift actions returned");
            }

            try
            {
                var requestID = await _repository.NewShiftsRequestAsync(request, response.Fulfillable, formVariant.RequestorDefinedByGroup);
                response.RequestID = requestID;

                if (response.RequestID == 0)
                {
                    throw new Exception("Error in saving request");
                }

                foreach (NewTaskAction newTaskAction in actions.TaskActions.Keys)
                {
                    List<int> actionAppliesToIds = actions.TaskActions[newTaskAction];
                    if (actionAppliesToIds == null) { continue; }

                    switch (newTaskAction)
                    {
                        case NewTaskAction.MakeAvailableToGroups:
                            foreach (int i in actionAppliesToIds)
                            {
                                await _repository.AddRequestAvailableToGroupAsync(requestID, i, cancellationToken);                                
                            }
                            break;

                        //case NewTaskAction.NotifyMatchingVolunteers:
                        //    foreach (int groupId in actionAppliesToIds)
                        //    {
                        //        bool commsSent = await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                        //        {
                        //            GroupID = groupId,
                        //            CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.SendNewTaskNotification },
                        //            RequestID = requestID,                                    
                        //        }, cancellationToken);
                        //        await _repository.UpdateCommunicationSentAsync(response.RequestID, commsSent, cancellationToken);
                        //    }
                        //    break;

                        //case NewTaskAction.AssignToVolunteer:
                        //    foreach (int userId in actionAppliesToIds)
                        //    {
                        //        await _repository.UpdateJobStatusInProgressAsync(jobID, request.HelpRequest.CreatedByUserId, userId, cancellationToken);
                        //    }

                        //    // For now, this only happens with a DIY request
                        //    response.Fulfillable = Fulfillable.Accepted_DiyRequest;
                        //    break;
                        //case NewTaskAction.SetStatusToOpen:
                        //    await _repository.UpdateJobStatusOpenAsync(jobID, -1, cancellationToken);
                        //    break;
                        //case NewTaskAction.NotifyGroupAdmins:
                        //    foreach (int groupId in actionAppliesToIds)
                        //    {
                        //        await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                        //        {
                        //            GroupID = groupId,
                        //            CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.NewTaskPendingApprovalNotification },
                        //            JobID = jobID
                        //        }, cancellationToken);
                        //    }
                        //    break;
                        //case NewTaskAction.SendRequestorConfirmation:
                        //    Dictionary<string, string> additionalParameters = new Dictionary<string, string>
                        //    {
                        //        { "PendingApproval", (!actions.Actions[guid].TaskActions.ContainsKey(NewTaskAction.SetStatusToOpen)).ToString() }
                        //    };
                        //    await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                        //    {
                        //        CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.RequestorTaskConfirmation },
                        //        JobID = jobID,
                        //        AdditionalParameters = additionalParameters,
                        //    }, cancellationToken);
                        //    break;
                    }
                }
            }
            catch (DuplicateException exc)
            {
                requestId = await _repository.GetRequestIDFromGuid(request.Guid);

                if (requestId > 0)
                {
                    return new PostNewShiftsResponse()
                    {
                        RequestID = requestId,
                        Fulfillable = Fulfillable.Accepted_ManualReferral
                    };
                }
                else
                {
                    throw exc;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
                            
            return response;
        }
    }
}
