using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Moq;
using NUnit.Framework;
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
    public class RepeatTests
    {
        private Mock<IRepository> _repository;
        private Mock<IAddressService> _adddressService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IGroupService> _groupService;
        private Mock<IMultiJobs> _multiJobs;
        private PostRequestForHelpHandler _classUnderTest;

        private int _requestId;
        private int _newRequestId;
        private bool _validPostcode;
        private GetRequestHelpFormVariantResponse _formVariantResponse;
        private GetNewRequestActionsSimplifiedResponse _getNewRequestActionsSimplifiedResponse;
        private GetGroupMemberResponse _getGroupMemberResponse;
        private GetUserRolesResponse _getUserRolesResponse;
        private GetGroupResponse _getGroupResponse;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupAddressService();
            SetupCommunicationService();
            SetupGroupService();
            SetupMultiObjects();
            _classUnderTest = new PostRequestForHelpHandler(
                _repository.Object,                
                _adddressService.Object,
                _communicationService.Object,
                _groupService.Object,
                _multiJobs.Object
                );
        }
        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.GetRequestIDFromGuid(It.IsAny<Guid>()))
                .ReturnsAsync(() => _requestId);

            _repository.Setup(x => x.AddHelpRequestDetailsAsync(
                It.IsAny<HelpRequestDetail>(),
                It.IsAny<Fulfillable>(),
                It.IsAny<bool>(),                
                It.IsAny<bool?>(),
                It.IsAny<IEnumerable<int>>()
                ))
                .ReturnsAsync(() => _newRequestId);
        }

        private void SetupAddressService()
        {
            _adddressService = new Mock<IAddressService>();
            _adddressService.Setup(x => x.IsValidPostcode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _validPostcode);
        }

        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
        }

        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();

            _groupService.Setup(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _formVariantResponse);

            _groupService.Setup(x => x.GetNewRequestActionsSimplified(It.IsAny<GetNewRequestActionsSimplifiedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getNewRequestActionsSimplifiedResponse);

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
        }

        [Test]
        public async Task GivenSingleTaskJustOnceASAP_WhenRequestSubmitted_ThenSingleJobGetsCreated()
        {
            _validPostcode = true;
            _requestId = -1;
            _newRequestId = 1;
            Guid guid = Guid.NewGuid();
            _getNewRequestActionsSimplifiedResponse = new GetNewRequestActionsSimplifiedResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsSimplifiedResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsSimplifiedResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                TargetGroups = TargetGroups.GenericGroup
            };

            var request = new PostRequestForHelpRequest()
            {
                HelpRequestDetails = new List<HelpRequestDetail>()
                {
                   new HelpRequestDetail()
                   {
                       HelpRequest = new HelpRequest()
                       {
                           Guid = guid,
                           Recipient = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = "NG1 6DQ"
                               }
                           },
                           Requestor = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = "NG1 6DQ"
                               }
                           }
                       },
                       NewJobsRequest = new NewJobsRequest()
                       {
                           Jobs = new List<Job>()
                           {
                               new Job()
                               {
                                   RepeatFrequency = Frequency.Once,
                                   DueDateType = DueDateType.ASAP,
                                   SupportActivity = SupportActivities.Shopping
                               }
                           }
                       }
                   }
                }
            };
            var response = await _classUnderTest.Handle(request, new CancellationToken());
            Assert.AreEqual(1, response.RequestIDs.Count);
            Assert.AreEqual(1, request.HelpRequestDetails.Count);
            Assert.AreEqual(1, request.HelpRequestDetails[0].NewJobsRequest.Jobs.Count);
            Assert.AreEqual(DateTime.Now.Date.AddDays(3), request.HelpRequestDetails[0].NewJobsRequest.Jobs[0].StartDate.Value.Date);
        }

        [Test]
        [TestCase(true)]
        public async Task GivenSingleTaskFrequencyDailyRequiredOnceStartingASAP_WhenRequestSubmitted_ThenSingleJobGetsCreated(bool hasFormVariantResponse)
        {
            _validPostcode = true;
            _requestId = -1;
            _newRequestId = 1;
            if (hasFormVariantResponse)
            {
                _formVariantResponse = new GetRequestHelpFormVariantResponse()
                {
                    AccessRestrictedByRole = false,
                    RequestorDefinedByGroup = false,
                    RequestHelpFormVariant = RequestHelpFormVariant.Default,
                    TargetGroups = TargetGroups.GenericGroup,
                    SuppressRecipientPersonalDetails = true
                };
            }
            else
            {
                _formVariantResponse = null;
            }

            Guid guid = Guid.NewGuid();
            _getNewRequestActionsSimplifiedResponse = new GetNewRequestActionsSimplifiedResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsSimplifiedResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsSimplifiedResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
            _getNewRequestActionsSimplifiedResponse.RequestTaskActions.Add(NewTaskAction.MakeAvailableToGroups, new List<int>() { 1 });

            var request = new PostRequestForHelpRequest()
            {
                HelpRequestDetails = new List<HelpRequestDetail>()
                {
                   new HelpRequestDetail()
                   {
                       HelpRequest = new HelpRequest()
                       {
                           Guid = guid,
                           Recipient = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = "NG1 6DQ"
                               }
                           },
                           Requestor = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = "NG1 6DQ"
                               }
                           }
                       },
                       NewJobsRequest = new NewJobsRequest()
                       {
                           Jobs = new List<Job>()
                           {
                               new Job()
                               {
                                   RepeatFrequency = Frequency.Daily,
                                   DueDateType = DueDateType.ASAP,
                                   SupportActivity = SupportActivities.Shopping,
                                   NumberOfRepeats = 1
                               }
                           }
                       }
                   }
                }
            };
            var response = await _classUnderTest.Handle(request, new CancellationToken());
            Assert.AreEqual(1, response.RequestIDs.Count);
            Assert.AreEqual(1, request.HelpRequestDetails.Count);
            Assert.AreEqual(1, request.HelpRequestDetails[0].NewJobsRequest.Jobs.Count);
            Assert.AreEqual(DateTime.Now.Date, request.HelpRequestDetails[0].NewJobsRequest.Jobs[0].StartDate.Value.Date);
        }

        [Test]
        [TestCase(false, Fulfillable.Rejected_InvalidPostcode, 0, false, 0, null)]
        [TestCase(true, Fulfillable.Accepted_ManualReferral, 1, true, 1, false)]
        public async Task Test(bool validPostCode, Fulfillable fulfillable, int newRequestId, bool hasFormVariantResponse, int timesGetRequestIDFromGuid, bool? accessRestrictedByRole)
        {
            _validPostcode = validPostCode;
            _newRequestId = newRequestId;
            int groupId = -1;
            string postCode = "NG1 6DQ";
            string source = string.Empty;

            if (hasFormVariantResponse)
            {
                _formVariantResponse = new GetRequestHelpFormVariantResponse()
                {
                    AccessRestrictedByRole = accessRestrictedByRole.Value,
                    RequestorDefinedByGroup = false,
                    RequestHelpFormVariant = RequestHelpFormVariant.Default,
                    TargetGroups = TargetGroups.GenericGroup,
                    SuppressRecipientPersonalDetails = true
                };
            }
            else
            {
                _formVariantResponse = null;
            }


            Guid guid = Guid.NewGuid();
            var request = new PostRequestForHelpRequest()
            {
                HelpRequestDetails = new List<HelpRequestDetail>()
                {
                   new HelpRequestDetail()
                   {
                       HelpRequest = new HelpRequest()
                       {
                           Guid = guid,
                           Recipient = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           Requestor = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           ReferringGroupId = groupId,
                           Source = source
                       },
                       NewJobsRequest = new NewJobsRequest()
                       {
                           Jobs = new List<Job>()
                           {
                               new Job()
                               {
                                   RepeatFrequency = Frequency.Daily,
                                   DueDateType = DueDateType.ASAP,
                                   SupportActivity = SupportActivities.Shopping,
                                   NumberOfRepeats = 1
                               }
                           }
                       }
                   }
                }
            };

            _getNewRequestActionsSimplifiedResponse = new GetNewRequestActionsSimplifiedResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsSimplifiedResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsSimplifiedResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
            _getNewRequestActionsSimplifiedResponse.RequestTaskActions.Add(NewTaskAction.MakeAvailableToGroups, new List<int>() { 1 });

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            _adddressService.Verify(x => x.IsValidPostcode(postCode, It.IsAny<CancellationToken>()), Times.Exactly(1));
            _repository.Verify(x => x.GetRequestIDFromGuid(guid), Times.Exactly(timesGetRequestIDFromGuid));
            _groupService.Verify(x => x.GetRequestHelpFormVariant(groupId, source, It.IsAny<CancellationToken>()), hasFormVariantResponse ? Times.Once() : Times.Never());
            Assert.AreEqual(fulfillable, response.Fulfillable);
            int i = 1;

        }


        [Test]
        [TestCase(false, 1, GroupRoles.RequestSubmitter, -1, 0, 0, 0)]
        [TestCase(true, 1, GroupRoles.RequestSubmitter, -1, 1, 0, 0)]
        [TestCase(true, 0, GroupRoles.RequestSubmitter, -1, 0, 0, 0)]
        [TestCase(true, 1, GroupRoles.TaskAdmin, -2, 1, 1, 1)]
        [TestCase(true, 1, GroupRoles.TaskAdmin, null, 1, 1, 1)]
        public async Task Handle_FailedChecks(bool accessRestrictedByRole, int createdByUserId, GroupRoles? groupRole, int parentGroupId,
            int callGetGroupMemberCount, int callGetUserRolesCount, int callGetGroupCount
            )
        {
            _validPostcode = true;
            _requestId = -1;
            _newRequestId = 1;
            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = accessRestrictedByRole,
                RequestorDefinedByGroup = false,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup,
                SuppressRecipientPersonalDetails = true
            };
            int groupId = -1;
            string postCode = "NG1 6DQ";
            string source = string.Empty;

            Guid guid = Guid.NewGuid();
            var request = new PostRequestForHelpRequest()
            {
                HelpRequestDetails = new List<HelpRequestDetail>()
                {
                   new HelpRequestDetail()
                   {
                       HelpRequest = new HelpRequest()
                       {
                           Guid = guid,
                           Recipient = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           Requestor = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           ReferringGroupId = groupId,
                           Source = source,
                           CreatedByUserId = createdByUserId
                       },
                       NewJobsRequest = new NewJobsRequest()
                       {
                           Jobs = new List<Job>()
                           {
                               new Job()
                               {
                                   RepeatFrequency = Frequency.Daily,
                                   DueDateType = DueDateType.ASAP,
                                   SupportActivity = SupportActivities.Shopping,
                                   NumberOfRepeats = 1
                               }
                           }
                       }
                   }
                }
            };
            
            _getNewRequestActionsSimplifiedResponse = new GetNewRequestActionsSimplifiedResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsSimplifiedResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsSimplifiedResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
            _getNewRequestActionsSimplifiedResponse.RequestTaskActions.Add(NewTaskAction.MakeAvailableToGroups, new List<int>() { 1 });

            _getGroupMemberResponse = new GetGroupMemberResponse()
            {
                UserInGroup = new UserInGroup()
                {
                    GroupRoles = new List<GroupRoles>() { groupRole.Value }
                }
            };

            _getUserRolesResponse = new GetUserRolesResponse()
            {
                UserGroupRoles = new Dictionary<int, List<int>>
                {
                    { groupId, new List<int>{(int) GroupRoles.RequestSubmitter } }
                }
            };

            _getGroupResponse = new GetGroupResponse()
            {
                Group = new Group()
                {
                    ParentGroupId = parentGroupId
                }
            };

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            _adddressService.Verify(x => x.IsValidPostcode(postCode, It.IsAny<CancellationToken>()), Times.Exactly(1));
            _repository.Verify(x => x.GetRequestIDFromGuid(guid), Times.Exactly(1));
            _groupService.Verify(x => x.GetRequestHelpFormVariant(groupId, source, It.IsAny<CancellationToken>()), Times.Once());
            _groupService.Verify(x => x.GetGroupMember(It.IsAny<GetGroupMemberRequest>()), Times.Exactly(callGetGroupMemberCount));
            _groupService.Verify(x => x.GetUserRoles(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(callGetUserRolesCount));
            _groupService.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(callGetGroupCount));
        }

        [Test]
        [TestCase("NG1 6DQ", true, Fulfillable.Accepted_ManualReferral)]
        [TestCase("NG1 6", false, Fulfillable.Rejected_InvalidPostcode)]
        public async Task Handle_InvalidPostCode(string postCode, bool validPostcode, Fulfillable fulfillable)
        {
            Guid guid = Guid.NewGuid();
            int groupId = -1;
            string source = "";
            var request = new PostRequestForHelpRequest()
            {
                HelpRequestDetails = new List<HelpRequestDetail>()
                {
                   new HelpRequestDetail()
                   {
                       HelpRequest = new HelpRequest()
                       {
                           Guid = guid,
                           Recipient = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           Requestor = new RequestPersonalDetails()
                           {
                               Address = new Address()
                               {
                                   Postcode = postCode
                               }
                           },
                           ReferringGroupId = groupId,
                           Source = source
                       },
                       NewJobsRequest = new NewJobsRequest()
                       {
                           Jobs = new List<Job>()
                           {
                               new Job()
                               {
                                   RepeatFrequency = Frequency.Daily,
                                   DueDateType = DueDateType.ASAP,
                                   SupportActivity = SupportActivities.Shopping,
                                   NumberOfRepeats = 1
                               }
                           }
                       }
                   }
                }
            };
            _validPostcode = validPostcode;

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = false,
                RequestorDefinedByGroup = false,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup,
                SuppressRecipientPersonalDetails = true
            };

            _getNewRequestActionsSimplifiedResponse = new GetNewRequestActionsSimplifiedResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsSimplifiedResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsSimplifiedResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
            _getNewRequestActionsSimplifiedResponse.RequestTaskActions.Add(NewTaskAction.MakeAvailableToGroups, new List<int>() { 1 });

            _newRequestId = 1;

            var response = await _classUnderTest.Handle(request, new CancellationToken());
            _adddressService.Verify(x => x.IsValidPostcode(postCode, It.IsAny<CancellationToken>()), Times.Exactly(1));
            _repository.Verify(x => x.GetRequestIDFromGuid(guid), validPostcode ? Times.Once() : Times.Never());
            Assert.AreEqual(fulfillable, response.Fulfillable);

        }
    }
}