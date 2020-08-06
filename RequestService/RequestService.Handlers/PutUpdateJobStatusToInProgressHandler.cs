﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using System.Linq;
using HelpMyStreet.Utils.Models;
using System;

namespace RequestService.Handlers
{
    public class PutUpdateJobStatusToInProgressHandler : IRequestHandler<PutUpdateJobStatusToInProgressRequest, PutUpdateJobStatusToInProgressResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public PutUpdateJobStatusToInProgressHandler(IRepository repository, ICommunicationService communicationService, IGroupService groupService, IUserService userService)
        {
            _repository = repository;
            _communicationService = communicationService;
            _groupService = groupService;
            _userService = userService;
        }

        public async Task<PutUpdateJobStatusToInProgressResponse> Handle(PutUpdateJobStatusToInProgressRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobStatusToInProgressResponse response = new PutUpdateJobStatusToInProgressResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };

            var user = await _userService.GetUser(request.VolunteerUserID, cancellationToken);

            if(!user.User.IsVerified ?? false || user==null)
            {
                response.Outcome = UpdateJobStatusOutcome.BadRequest;
                return response;
            }


            var volunteerGroups = await _groupService.GetUserGroups(request.VolunteerUserID, cancellationToken);
            var jobGroups = await _repository.GetGroupsForJobAsync(request.JobID, cancellationToken);
            int? referringGroupId = await _repository.GetReferringGroupIDForJobAsync(request.JobID, cancellationToken);

            if (volunteerGroups == null || jobGroups == null)
            {
                throw new System.Exception("volunteerGroups or jobGroup is null");
            }

            if (!referringGroupId.HasValue)
            {
                throw new Exception($"Unable to retrieve referring groupId for jobID:{request.JobID}");
            }

            bool jobGroupContainsVolunteerGroups = jobGroups.Any(volunteerGroups.Groups.Contains);

            if (!jobGroupContainsVolunteerGroups)
            {
                response.Outcome = UpdateJobStatusOutcome.BadRequest;
                return response;
            }

            var userRoles = await _groupService.GetUserRoles(request.CreatedByUserID, cancellationToken);

            if (request.CreatedByUserID == request.VolunteerUserID || userRoles.UserGroupRoles[referringGroupId.Value].Contains((int)GroupRoles.TaskAdmin))
            {
                var result = await _repository.UpdateJobStatusInProgressAsync(request.JobID, request.CreatedByUserID, request.VolunteerUserID, cancellationToken);

                if (result)
                {
                    response.Outcome = UpdateJobStatusOutcome.Success;
                    await _communicationService.RequestCommunication(
                        new RequestCommunicationRequest()
                        {
                            CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                            JobID = request.JobID
                        },
                        cancellationToken);
                }
                else
                {
                    response.Outcome = UpdateJobStatusOutcome.BadRequest;
                }
            }
            else
            {
                response.Outcome = UpdateJobStatusOutcome.Unauthorized;
                return response;
            }
            return response;
        }
    }
}
