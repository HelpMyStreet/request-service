using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.UserService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RequestService.Core.Config;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using RequestService.Handlers.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PostNewRequestForHelpHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<IUserService> _userService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IAddressService> _adddressService;
        private Mock<IGroupService> _groupService;
        private Mock<IMultiJobs> _multiJobs;
        private PostNewRequestForHelpHandler _classUnderTest;
        private Mock<IOptionsSnapshot<ApplicationConfig>> _applicationConfig;
        private int requestId;
        private bool _validPostcode;
        private int _championCount;
        private bool _emailSent;
        private GetNewRequestActionsResponse _getNewRequestActionsResponse;
        private GetVolunteersByPostcodeAndActivityResponse _getVolunteersByPostcodeAndActivityResponse;
        private GetGroupMembersResponse _getGroupMembersResponse;
        private GetRequestHelpFormVariantResponse _formVariantResponse;
        private GetGroupMemberResponse _getGroupMemberResponse;
        private GetUserRolesResponse _getUserRolesResponse;
        private GetGroupResponse _getGroupResponse;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupAddressService();
            SetupCommunicationService();
            SetupApplicationConfig();
            SetupUserService();
            SetupGroupService();
            SetupMultiObjects();
            _classUnderTest = new PostNewRequestForHelpHandler(_repository.Object, _userService.Object, _adddressService.Object, _communicationService.Object, _groupService.Object, _applicationConfig.Object, _multiJobs.Object);
        }
        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
            _communicationService.Setup(x => x.SendEmail(It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _emailSent);

        }
        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.NewHelpRequestAsync(
                It.IsAny<PostNewRequestForHelpRequest>(),
                It.IsAny<Fulfillable>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()
                ))
                .ReturnsAsync(() => requestId);
            _repository.Setup(x => x.UpdateCommunicationSentAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        }
        private void SetupApplicationConfig()
        {
            _applicationConfig = new Mock<IOptionsSnapshot<ApplicationConfig>>();
            _applicationConfig.SetupGet(x => x.Value).Returns(new ApplicationConfig
            {
                ManualReferName = "test",
                ManualReferEmail = "manual@test.com",
                EmailBaseUrl = "helpmystreettest",
                FaceMaskChunkSize = 10
            });
        }

        private void SetupUserService()
        {
            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetChampionCountByPostcode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _championCount);
            _userService.Setup(x => x.GetHelpersByPostcodeAndTaskType(It.IsAny<string>(), It.IsAny<List<SupportActivities>>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _getVolunteersByPostcodeAndActivityResponse);
        }

        private void SetupAddressService()
        {
            _adddressService = new Mock<IAddressService>();
            _adddressService.Setup(x => x.IsValidPostcode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _validPostcode);
        }

        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();
            Dictionary<Guid, TaskAction> actions = new Dictionary<Guid, TaskAction>();
            actions.Add(Guid.NewGuid(), new TaskAction()
            {
                TaskActions = new Dictionary<NewTaskAction, List<int>>()
            });

            _getNewRequestActionsResponse = new GetNewRequestActionsResponse()
            {
                Actions = actions
            };


            _groupService.Setup(x => x.GetNewRequestActions(It.IsAny<GetNewRequestActionsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getNewRequestActionsResponse);

            _getGroupMembersResponse = new GetGroupMembersResponse()
            {
                Users = new List<int>() { 1, 2 }
            };

            _groupService.Setup(x => x.GetGroupMembers(It.IsAny<int>()))
                .ReturnsAsync(() => _getGroupMembersResponse);

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = false,
                RequestorDefinedByGroup = false,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup
            };

            _groupService.Setup(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _formVariantResponse);

            _groupService.Setup(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()))
                .ReturnsAsync(() => _getGroupMemberResponse);

            _groupService.Setup(x => x.GetUserRoles(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getUserRolesResponse);

            _groupService.Setup(x => x.GetGroup(It.IsAny<int>()))
                .ReturnsAsync(() => _getGroupResponse);
                
        }

        private void SetupMultiObjects()
        {
            _multiJobs = new Mock<IMultiJobs>();

            _multiJobs.Setup(x => x.AddMultiVolunteers(It.IsAny<NewJobsRequest>()));
            //.Returns((PostNewRequestForHelpRequest r) => r);

            _multiJobs.Setup(x => x.AddRepeats(It.IsAny<NewJobsRequest>()));
                //.Returns((PostNewRequestForHelpRequest r) => r);
        }

        [Test]
        public async Task WhenIPostDiyRequest_FullfiableStatusGetSetToDiy()
        {
            requestId = 1;
            _validPostcode = true;
            _emailSent = true;
            Guid guid = Guid.NewGuid();
            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Requestor = new RequestPersonalDetails
                    {
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    },
                    VolunteerUserId = 1,
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            Guid = guid,
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };

           
            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            Assert.AreEqual(Fulfillable.Accepted_DiyRequest, response.Fulfillable);
        }


        [Test]
        public async Task WhenIPostDiyRequest_IOnlySendConfirmationEMail()
        {
            _validPostcode = true;
            _emailSent = true;

            Guid guid = Guid.NewGuid();


            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Requestor = new RequestPersonalDetails
                    {
                        EmailAddress = "test",
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    },
                    VolunteerUserId = 1,
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            Guid = guid,
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };
            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });

            await _classUnderTest.Handle(request, new CancellationToken());
            _communicationService.Verify(x => x.SendEmailToUsersAsync(It.IsAny<SendEmailToUsersRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.UpdateJobStatusInProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _userService.Verify(x => x.GetHelpersByPostcodeAndTaskType(It.IsAny<string>(), It.IsAny <List<SupportActivities>>(), It.IsAny<CancellationToken>()), Times.Never);
        }



        [Test]
        public async Task WhenIPostRequest_WithInvalidPostcode_IGetRejected()
        {            
            _validPostcode = false;

            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Requestor = new RequestPersonalDetails
                    {
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    }
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            Assert.AreEqual(Fulfillable.Rejected_InvalidPostcode, response.Fulfillable);     
        }

        [Test]
        public async Task WhenIPostRequest_WithNoChampions_IGetManualReer()
        {
            requestId = 1;
            _validPostcode = true;
            _championCount = 0;
            _emailSent = true;

            Guid guid = Guid.NewGuid();
            _getVolunteersByPostcodeAndActivityResponse = new GetVolunteersByPostcodeAndActivityResponse
            {
                Volunteers = new List<VolunteerSummary>
                {
                    new VolunteerSummary
                    {
                        UserID = 1,
                        DistanceInMiles = 1,
                    }
                }
            };
            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Requestor = new RequestPersonalDetails
                    {
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    }
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            Guid = guid,
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };

            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.MakeAvailableToGroups, new List<int>() { 1 });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.NotifyMatchingVolunteers, new List<int>() { 1 });


            var response = await _classUnderTest.Handle(request, new CancellationToken());
            Assert.AreEqual(Fulfillable.Accepted_ManualReferral, response.Fulfillable);
        }

        [Test]
        public async Task WhenRequestorDefinedByGroup_Populate()
        {
            _validPostcode = true;
            _emailSent = true;
            Guid guid = Guid.NewGuid();
            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Recipient = new RequestPersonalDetails
                    {
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    },
                    VolunteerUserId = 1,
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            Guid = guid,
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = false,
                RequestorDefinedByGroup = true,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup,
                RequestorPersonalDetails = new RequestPersonalDetails()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EmailAddress = "Email",
                    MobileNumber = "Mobile",
                    OtherNumber = "Other"
                }
            };

           

            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            _groupService.Verify(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(_formVariantResponse.RequestorPersonalDetails, request.HelpRequest.Requestor);
            Assert.AreEqual(Fulfillable.Accepted_DiyRequest, response.Fulfillable);
        }

        [Test]
        [TestCase(1, GroupRoles.Member, Fulfillable.Rejected_Unauthorised, 1)]
        [TestCase(1, GroupRoles.Volunteer, Fulfillable.Rejected_Unauthorised, 1)]
        [TestCase(1, GroupRoles.Owner, Fulfillable.Rejected_Unauthorised, 1)]
        [TestCase(1, GroupRoles.TaskAdmin, Fulfillable.Rejected_Unauthorised, 1)]
        [TestCase(1, GroupRoles.UserAdmin, Fulfillable.Rejected_Unauthorised, 1)]
        [TestCase(1, GroupRoles.RequestSubmitter, Fulfillable.Accepted_DiyRequest, 1)]
        [TestCase(0, GroupRoles.Member, Fulfillable.Rejected_Unauthorised, 0)]
        public async Task WhenAccessRestrictedByRole_ReturnsCorrectResponse(int createdByUserId, GroupRoles role, Fulfillable fulfillable, int timesGroupMemberCalled)
        {
            requestId = 1;
            _validPostcode = true;
            _emailSent = true;
            int referringGroupId = -20;

            Guid guid = Guid.NewGuid();
            var request = new PostNewRequestForHelpRequest
            {
                HelpRequest = new HelpRequest
                {
                    RequestorType = RequestorType.Myself,
                    Recipient = new RequestPersonalDetails
                    {
                        Address = new Address
                        {
                            Postcode = "test",
                        }
                    },
                    VolunteerUserId = 1,
                    CreatedByUserId = createdByUserId,
                    ReferringGroupId = referringGroupId
                },
                NewJobsRequest = new NewJobsRequest
                {
                    Jobs = new List<Job>
                    {
                        new Job
                        {
                            Guid = guid,
                            HealthCritical = true,
                            DueDays = 5,
                            SupportActivity = SupportActivities.Shopping
                        }
                    }
                }
            };

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = true,
                RequestorDefinedByGroup = true,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup,
                RequestorPersonalDetails = new RequestPersonalDetails()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EmailAddress = "Email",
                    MobileNumber = "Mobile",
                    OtherNumber = "Other"
                }
            };

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    UserId = 1,
                    GroupId = 1,
                    GroupRoles = new List<GroupRoles>() { role }
                }
            };

            _getGroupResponse = new GetGroupResponse()
            {
                Group = new Group()
                {
                    GroupId = referringGroupId
                }
            };

            _getUserRolesResponse = new GetUserRolesResponse()
            {
                UserGroupRoles = new Dictionary<int, List<int>>()
            };

            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            _groupService.Verify(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Exactly(timesGroupMemberCalled));
            Assert.AreEqual(fulfillable, response.Fulfillable);
        }
    }
}