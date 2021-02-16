using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.UserService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Moq;
using NUnit.Framework;
using Polly.Timeout;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PutUpdateJobStatusToInProgressHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IGroupService> _groupService;
        private Mock<IUserService> _userService;
        private Mock<IJobService> _jobService;

        private PutUpdateJobStatusToInProgressHandler _classUnderTest;
        private PutUpdateJobStatusToInProgressRequest _request;
        private UpdateJobStatusOutcome _updateJobStatusOutcome;
        private GetUserGroupsResponse _getUserGroupsReponse;        
        private List<int> _getGroupsForJobResponse;
        private int _referringGroupId;
        private PostAssignRoleResponse _postAssignRoleResponse;
        private bool _isSameAsProposed = false;
        private GetGroupMemberResponse _getGroupMemberResponse;
        private GetGroupActivityCredentialsResponse _getGroupActivityCredentialsResponse;
        private GetJobDetailsResponse _getJobDetailsResponse;
        private bool _hasPermission;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupCommunicationService();
            SetupGroupService();
            SetupJobService();
            _classUnderTest = new PutUpdateJobStatusToInProgressHandler(_repository.Object, _communicationService.Object,_groupService.Object, _jobService.Object);
        }

        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
            _communicationService.Setup(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.UpdateJobStatusInProgressAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _updateJobStatusOutcome);

            _repository.Setup(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _getGroupsForJobResponse);

            _repository.Setup(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _referringGroupId);

            _repository.Setup(x => x.JobIsInProgressWithSameVolunteerUserId(
                It.IsAny<int>(),
                It.IsAny<int?>()
                )).Returns(() => _isSameAsProposed);

            _repository.Setup(x => x.GetJobDetails(It.IsAny<int>()))
                .Returns(() => _getJobDetailsResponse);

        }

        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();
            _groupService.Setup(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _getUserGroupsReponse);

            _postAssignRoleResponse = new PostAssignRoleResponse() { Outcome = GroupPermissionOutcome.Success };

            _groupService.Setup(x => x.PostAssignRole(It.IsAny<PostAssignRoleRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _postAssignRoleResponse);

            _groupService.Setup(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()))
                .ReturnsAsync(() => _getGroupMemberResponse);

            _groupService.Setup(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()))
                .ReturnsAsync(() => _getGroupActivityCredentialsResponse);
        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();

            _jobService.Setup(x => x.HasPermissionToChangeStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))                                     
                .ReturnsAsync(() => _hasPermission);
        }

        //[Test]
        //public async Task WhenSuccessfullyChangingJobStatusToInProgress_ReturnsTrue()
        //{
        //    _success = true;
        //    _getUserGroupsReponse = new GetUserGroupsResponse()
        //    {
        //        Groups = new List<int>()
        //        {
        //            1
        //        }
        //    };
        //    _getGroupsForJobResponse = new List<int>()
        //    {
        //        1
        //    };
        //    _referringGroupId = 1;
        //    _getUserRolesResponse = new GetUserRolesResponse()
        //    {
        //        UserGroupRoles = new Dictionary<int, List<int>>()
        //    };



        //    _request = new PutUpdateJobStatusToInProgressRequest
        //    {
        //        CreatedByUserID = 1,
        //        JobID = 1,
        //        VolunteerUserID = 1
        //    };
        //    var response = await _classUnderTest.Handle(_request, CancellationToken.None);

        //    Assert.AreEqual(UpdateJobStatusOutcome.Success, response.Outcome);
        //}

        //[Test]
        //public async Task WhenUnSuccessfullyChangingJobStatusToInProgress_ReturnsBadRequest()
        //{
        //    _success = false;
        //    _getUserGroupsReponse = new GetUserGroupsResponse()
        //    {
        //        Groups = new List<int>()
        //        {
        //            1
        //        }
        //    };
        //    _getGroupsForJobResponse = new List<int>()
        //    {
        //        1
        //    };
        //    _referringGroupId = 1;

        //    Dictionary<int, List<int>> roles = new Dictionary<int, List<int>>();
        //    roles.Add(1, new List<int>() { (int) GroupRoles.TaskAdmin });

        //    _getUserRolesResponse = new GetUserRolesResponse()
        //    {
        //        UserGroupRoles = roles
        //     };
        //    _request = new PutUpdateJobStatusToInProgressRequest
        //    {
        //        CreatedByUserID = 1,
        //        JobID = 1,
        //        VolunteerUserID = 1
        //    };
        //    var response = await _classUnderTest.Handle(_request, CancellationToken.None);
        //    Assert.AreEqual(UpdateJobStatusOutcome.BadRequest, response.Outcome);
        //}

        [Test]
        public async Task WhenUserIsNotVerified_ReturnsBadRequest()
        {
            _updateJobStatusOutcome =  UpdateJobStatusOutcome.BadRequest;
            _request = new PutUpdateJobStatusToInProgressRequest
            {
                CreatedByUserID = 1,
                JobID = 1,
                VolunteerUserID = 1
            };

            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobID = 1,
                    SupportActivity = SupportActivities.Shopping
                }
            };

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    GroupId = 1,
                    UserId = 1,
                    GroupRoles = new List<GroupRoles>() { GroupRoles.Member },
                    ValidCredentials = new List<int>()
                }
            };

            _getGroupActivityCredentialsResponse = new GetGroupActivityCredentialsResponse()
            {
                CredentialSets = new List<List<int>> { new List<int>() { -1 } }
            };

            _getUserGroupsReponse = new GetUserGroupsResponse()
            {
                Groups = new List<int>()
                {
                    1
                }
            };

            _getGroupsForJobResponse = new List<int>()
            {
                1
            };

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.JobIsInProgressWithSameVolunteerUserId(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            _groupService.Verify(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);            
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Once);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Once);

            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),It.IsAny<CancellationToken>()), Times.Never);
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.BadRequest, response.Outcome);
        }

        [Test]
        public async Task WhenjobGroupDoesNotContainsVolunteerGroups_ReturnsBadRequest()
        {
            _updateJobStatusOutcome = UpdateJobStatusOutcome.BadRequest;
            _request = new PutUpdateJobStatusToInProgressRequest
            {
                CreatedByUserID = 1,
                JobID = 1,
                VolunteerUserID = 1
            };

            _getUserGroupsReponse = new GetUserGroupsResponse()
            {
                Groups = new List<int>()
                {
                    1
                }
            };

            _getGroupsForJobResponse = new List<int>()
            {
                2
            };

            _referringGroupId = 1;

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.JobIsInProgressWithSameVolunteerUserId(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            _groupService.Verify(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Never);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Never);            
            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.BadRequest, response.Outcome);
        }


        [Test]
        public async Task WhenCreatedByUserIsDifferentToVolunteerUserIDAndNotTaskAdmin_ReturnsUnauthorized()
        {
            _updateJobStatusOutcome =  UpdateJobStatusOutcome.Unauthorized;
            _request = new PutUpdateJobStatusToInProgressRequest
            {
                CreatedByUserID = 1,
                JobID = 1,
                VolunteerUserID = 2
            };
            _getUserGroupsReponse = new GetUserGroupsResponse()
            {
                Groups = new List<int>()
                {
                    1
                }
            };

            _getGroupsForJobResponse = new List<int>()
            {
                1
            };

            _referringGroupId = 1;

            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobID = 1,
                    SupportActivity = SupportActivities.Shopping
                }
            };

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    GroupId = 1,
                    UserId = 1,
                    GroupRoles = new List<GroupRoles>() { GroupRoles.Member },
                    ValidCredentials = new List<int>() { -1 }
                }
            };

            _getGroupActivityCredentialsResponse = new GetGroupActivityCredentialsResponse()
            {
                CredentialSets = new List<List<int>> { new List<int>() { -1 }}
            };

            _hasPermission = false;

            Dictionary<int, List<int>> roles = new Dictionary<int, List<int>>();
            roles.Add(1, new List<int>() { (int) GroupRoles.Member });
            
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.JobIsInProgressWithSameVolunteerUserId(It.IsAny<int>(), It.IsAny<int?>()), Times.Never);
            _groupService.Verify(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Never);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Never);
            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.Unauthorized, response.Outcome);
        }

        [Test]
        public async Task WhenCreatedByUserIsDifferentToVolunteerUserIDAndTaskAdmin_ReturnsSuccess()
        {
            _updateJobStatusOutcome = UpdateJobStatusOutcome.Success;
            _request = new PutUpdateJobStatusToInProgressRequest
            {
                CreatedByUserID = 1,
                JobID = 1,
                VolunteerUserID = 2
            };
            _getUserGroupsReponse = new GetUserGroupsResponse()
            {
                Groups = new List<int>()
                {
                    1
                }
            };

            _getGroupsForJobResponse = new List<int>()
            {
                1
            };

            _referringGroupId = 1;

            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobID = 1,
                    SupportActivity = SupportActivities.Shopping
                }
            };

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    GroupId = 1,
                    UserId = 1,
                    GroupRoles = new List<GroupRoles>() { GroupRoles.Member },
                    ValidCredentials = new List<int>() { -1 }
                }
            };

            _getGroupActivityCredentialsResponse = new GetGroupActivityCredentialsResponse()
            {
                CredentialSets = new List<List<int>> { new List<int>() { -1 } }
            };

            _hasPermission = true;


            Dictionary<int, List<int>> roles = new Dictionary<int, List<int>>();
            roles.Add(1, new List<int>() { (int)GroupRoles.TaskAdmin });

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.JobIsInProgressWithSameVolunteerUserId(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            _groupService.Verify(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);            
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Once);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Once);
            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(UpdateJobStatusOutcome.Success, response.Outcome);
        }

        [TestCase(GroupRoles.TaskAdmin)]
        [TestCase(GroupRoles.Member)]
        [TestCase(GroupRoles.Owner)]
        [TestCase(GroupRoles.RequestSubmitter)]
        [TestCase(GroupRoles.UserAdmin)]
        public async Task WhenCreatedByUserIsSameAsVolunteerUserID_ReturnsSuccess(GroupRoles role)
        {
            _updateJobStatusOutcome =  UpdateJobStatusOutcome.Success;
            _request = new PutUpdateJobStatusToInProgressRequest
            {
                CreatedByUserID = 1,
                JobID = 1,
                VolunteerUserID = 1
            };
            _getUserGroupsReponse = new GetUserGroupsResponse()
            {
                Groups = new List<int>()
                {
                    1
                }
            };

            _getGroupsForJobResponse = new List<int>()
            {
                1
            };

            _referringGroupId = 1;

            Dictionary<int, List<int>> roles = new Dictionary<int, List<int>>();
            roles.Add(1, new List<int>() { (int)role });
            
            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobID = 1,
                    SupportActivity = SupportActivities.Shopping
                }
            };

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    GroupId = 1,
                    UserId = 1,
                    GroupRoles = new List<GroupRoles>() { GroupRoles.Member },
                    ValidCredentials = new List<int>() { -1 }
                }
            };

            _getGroupActivityCredentialsResponse = new GetGroupActivityCredentialsResponse()
            {
                CredentialSets = new List<List<int>> { new List<int>() { -1 } }
            };

            _hasPermission = true;
            
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.JobIsInProgressWithSameVolunteerUserId(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            _groupService.Verify(x => x.GetUserGroups(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Once);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Once);
            _repository.Verify(x => x.GetGroupsForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetReferringGroupIDForJobAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);            
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Once);
            _groupService.Verify(x => x.GetGroupActivityCredentials(It.IsAny<GetGroupActivityCredentialsRequest>()), Times.Once);
            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(UpdateJobStatusOutcome.Success, response.Outcome);
        }
    }
}