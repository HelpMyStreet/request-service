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

            List<Job> duplicatedJobs = new List<Job>();

            foreach (Job j in request.NewJobsRequest.Jobs)
            {
                //check if number of volunteer question has been asked
                var numberOfSlotsQuestion = j.Questions?.Where(x => x.Id == (int)Questions.NumberOfSlots).FirstOrDefault();

                if (numberOfSlotsQuestion != null)
                {
                    int numberOfSlots = Convert.ToInt32(numberOfSlotsQuestion.Answer);
                    if (numberOfSlots > 1)
                    {
                        for (int i = 0; i < (numberOfSlots - 1); i++)
                        {
                            duplicatedJobs.Add(new Job()
                            {
                                HealthCritical = j.HealthCritical,
                                DueDateType = j.DueDateType,
                                SupportActivity = j.SupportActivity,
                                StartDate = j.StartDate,
                                EndDate = j.EndDate,
                                Questions = j.Questions,
                                DueDays = j.DueDays,
                            });
                        }
                    }
                }
            }

            if(duplicatedJobs.Count>0)
            {
                request.NewJobsRequest.Jobs.AddRange(duplicatedJobs);
            }


            foreach (Job j in request.NewJobsRequest.Jobs)
            {
                //add a guid for each job
                if (j.Guid == Guid.Empty)
                {
                    j.Guid = Guid.NewGuid();
                }
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

                    if(failedChecks)
                    {
                        //check if user has request submitter role with parant group
                        var userRoles = await _groupService.GetUserRoles(request.HelpRequest.CreatedByUserId, cancellationToken);

                        var group = await _groupService.GetGroup(request.HelpRequest.ReferringGroupId);
                        int? parentGroupId = group.Group.ParentGroupId;

                        if (parentGroupId.HasValue && userRoles.UserGroupRoles.ContainsKey(parentGroupId.Value))
                        {
                            failedChecks = !(userRoles.UserGroupRoles[parentGroupId.Value].Contains((int)GroupRoles.RequestSubmitter));                            
                        }
                    }
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

            if (!string.IsNullOrEmpty(request.HelpRequest.Recipient?.Address?.Postcode))
            {
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
                bool? suppressRecipientPersonalDetails = formVariant.SuppressRecipientPersonalDetails;

                Job firstJob = request.NewJobsRequest.Jobs.First();
                var suppressRecipientPersonalDetailsQuestion = firstJob.Questions?.Where(x => x.Id == (int)Questions.SuppressRecipientPersonalDetails).FirstOrDefault();

                if(suppressRecipientPersonalDetailsQuestion != null)
                {
                    suppressRecipientPersonalDetails = Convert.ToBoolean(suppressRecipientPersonalDetailsQuestion.Answer);
                }

                if(!suppressRecipientPersonalDetails.HasValue)
                {
                    return new PostNewRequestForHelpResponse
                    {
                        RequestID = -1,
                        Fulfillable = Fulfillable.Rejected_ConfigurationError
                    };
                }

                requestId = await _repository.NewHelpRequestAsync(request, response.Fulfillable, formVariant.RequestorDefinedByGroup, suppressRecipientPersonalDetails);
                response.RequestID = requestId;

                if (response.RequestID == 0)
                {
                    throw new Exception("Error in saving request");
                }

                foreach (NewTaskAction newTaskAction in actions.RequestTaskActions.Keys)
                {
                    List<int> actionAppliesToIds = actions.RequestTaskActions[newTaskAction];

                    switch (newTaskAction)
                    {
                        case NewTaskAction.MakeAvailableToGroups:
                            foreach (int i in actionAppliesToIds)
                            {
                                await _repository.AddRequestAvailableToGroupAsync(requestId, i, cancellationToken);
                            }
                            break;
                        case NewTaskAction.NotifyGroupAdmins:
                            foreach (int groupId in actionAppliesToIds)
                            {
                                await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                                {
                                    GroupID = groupId,
                                    CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.NewTaskPendingApprovalNotification },
                                    JobID = null,
                                    RequestID = requestId
                                }, cancellationToken);
                            }
                            break;
                        case NewTaskAction.SendRequestorConfirmation:
                            Dictionary<string, string> additionalParameters = new Dictionary<string, string>
                                {
                                    { "PendingApproval", (!actions.RequestTaskActions.ContainsKey(NewTaskAction.SetStatusToOpen)).ToString() }
                                };
                            await _communicationService.RequestCommunication(new RequestCommunicationRequest()
                            {
                                CommunicationJob = new CommunicationJob { CommunicationJobType = CommunicationJobTypes.RequestorTaskConfirmation },
                                JobID = null,
                                AdditionalParameters = additionalParameters,
                                RequestID = requestId
                            }, cancellationToken);
                            break;
                        case NewTaskAction.SetStatusToOpen:
                            await _repository.UpdateAllJobStatusToOpenForRequestAsync(requestId, -1, cancellationToken);
                            break;
                    }                            
                    
                }

                foreach (Guid guid in actions.Actions.Keys)
                {
                    int jobID = GetJobIDFromGuid(request,guid);
                    foreach (NewTaskAction newTaskAction in actions.Actions[guid].TaskActions.Keys)
                    {
                        List<int> actionAppliesToIds = actions.Actions[guid].TaskActions[newTaskAction];                        

                        switch (newTaskAction)
                        {
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
                else
                {
                    throw exc;
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
