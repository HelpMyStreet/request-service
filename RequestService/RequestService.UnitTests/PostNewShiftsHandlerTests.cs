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
using RequestService.Core.Dto;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PostNewShiftsHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<IGroupService> _groupService;
        private Mock<ICommunicationService> _communicationService;
        private PostNewShiftsHandler _classUnderTest;
        private PostNewShiftsRequest _request;
        private int _requestId;
        private GetRequestHelpFormVariantResponse _formVariantResponse;
        private GetNewShiftActionsResponse _getNewShiftActionsResponse;


        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupGroupService();
            SetupCommunicationService();
            _classUnderTest = new PostNewShiftsHandler(_repository.Object, _groupService.Object, _communicationService.Object);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.NewShiftsRequestAsync(
                It.IsAny<PostNewShiftsRequest>(),
                It.IsAny<Fulfillable>(),
                It.IsAny<RequestPersonalDetails>()))
                .ReturnsAsync(() => _requestId);
        }
       
        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();

            _formVariantResponse = new GetRequestHelpFormVariantResponse()
            {
                AccessRestrictedByRole = false,
                RequestorDefinedByGroup = true,
                RequestHelpFormVariant = RequestHelpFormVariant.Default,
                TargetGroups = TargetGroups.GenericGroup
            };

            _groupService.Setup(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _formVariantResponse);

            _groupService.Setup(x => x.GetNewShiftActions(It.IsAny<GetNewShiftActionsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getNewShiftActionsResponse);
        }

        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
        }

        [Test]
        public async Task WhenSuccesfullyReturnsRequestId()
        {
            _requestId = 1;
            _request = new PostNewShiftsRequest()
            {
                CreatedByUserId = 1,
                Location = new SingleLocationRequest() { Location = Location.Location1 },
                OtherDetails = "OTHER DETAILS",
                ReferringGroupId = -7,
                Source = "a",
                ShiftLength = 10,
                StartDate = DateTime.Now,
                SupportActivitiesCount = new List<SupportActivityCountRequest>()
                {
                    new SupportActivityCountRequest()
                    {
                        SupportActivity = SupportActivities.Shopping,
                        Count = 2
                    },
                    new SupportActivityCountRequest()
                    {
                        SupportActivity = SupportActivities.CollectingPrescriptions,
                        Count = 5
                    }
                }
            };
            _getNewShiftActionsResponse = new GetNewShiftActionsResponse()
            {
                TaskActions = new Dictionary<NewTaskAction, List<int>>()
                {

                    { NewTaskAction.MakeAvailableToGroups, new List<int>() }
                }
            };
          
            var response = await _classUnderTest.Handle(_request, new CancellationToken());
            _groupService.Verify(x => x.GetRequestHelpFormVariant(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _classUnderTest = new PostNewShiftsHandler(_repository.Object, _groupService.Object, _communicationService.Object);
            Assert.AreEqual(_requestId, response.RequestID);
        }
    }
}