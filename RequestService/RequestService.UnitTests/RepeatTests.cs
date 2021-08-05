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
    public class RepeatTests
    {
        private Mock<IRepository> _repository;
        private Mock<IUserService> _userService;
        private Mock<IAddressService> _adddressService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IGroupService> _groupService;
        private Mock<IOptionsSnapshot<ApplicationConfig>> _applicationConfig;
        private Mock<IMultiJobs> _multiJobs;
        private PostRequestForHelpHandler _classUnderTest;

        private int _requestId;
        private int _newRequestId;
        private bool _validPostcode;
        private GetRequestHelpFormVariantResponse _formVariantResponse;
        private GetNewRequestActionsResponse _getNewRequestActionsResponse;
        //private int _championCount;
        //private bool _emailSent;
        //private GetVolunteersByPostcodeAndActivityResponse _getVolunteersByPostcodeAndActivityResponse;
        //private GetGroupMembersResponse _getGroupMembersResponse;

        //private GetGroupMemberResponse _getGroupMemberResponse;
        //private GetUserRolesResponse _getUserRolesResponse;
        //private GetGroupResponse _getGroupResponse;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupUserService();
            SetupAddressService();
            SetupCommunicationService();
            SetupApplicationConfig();

            SetupGroupService();
            SetupMultiObjects();
            _classUnderTest = new PostRequestForHelpHandler(
                _repository.Object,
                _userService.Object,
                _adddressService.Object,
                _communicationService.Object,
                _groupService.Object,
                _applicationConfig.Object,
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
                It.IsAny<bool?>()))
                .ReturnsAsync(() => _newRequestId);
        }

        private void SetupUserService()
        {
            _userService = new Mock<IUserService>();
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

        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = false,
                RequestorDefinedByGroup = false,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup,
                SuppressRecipientPersonalDetails = true
            };

            _groupService.Setup(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _formVariantResponse);

            _groupService.Setup(x => x.GetNewRequestActions(It.IsAny<GetNewRequestActionsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getNewRequestActionsResponse);
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
            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
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
        public async Task GivenSingleTaskFrequencyDailyRequiredOnceStartingASAP_WhenRequestSubmitted_ThenSingleJobGetsCreated()
        {
            _validPostcode = true;
            _requestId = -1;
            _newRequestId = 1;
            Guid guid = Guid.NewGuid();
            _getNewRequestActionsResponse = new GetNewRequestActionsResponse() { Actions = new Dictionary<Guid, TaskAction>(), RequestTaskActions = new Dictionary<NewTaskAction, List<int>>() };
            _getNewRequestActionsResponse.Actions.Add(guid, new TaskAction() { TaskActions = new Dictionary<NewTaskAction, List<int>>() });
            _getNewRequestActionsResponse.Actions[guid].TaskActions.Add(NewTaskAction.AssignToVolunteer, new List<int>() { 1 });
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
    }
}