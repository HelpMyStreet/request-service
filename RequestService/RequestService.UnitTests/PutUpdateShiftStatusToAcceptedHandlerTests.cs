using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Utils.Enums;
using Moq;
using NUnit.Framework;
using RequestService.Core.Exceptions;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PutUpdateShiftStatusToAcceptedHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IJobService> _jobService;
        private PutUpdateShiftStatusToAcceptedHandler _classUnderTest;
        private PutUpdateShiftStatusToAcceptedRequest _request;
        private bool _hasPermission = true;
        private int _jobId;
        private int _newJobId;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupCommunicationService();
            SetupJobService();
            _classUnderTest = new PutUpdateShiftStatusToAcceptedHandler(_repository.Object, _communicationService.Object, _jobService.Object);
        }


        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
            _communicationService.Setup(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();
            _jobService.Setup(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _hasPermission);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.UpdateRequestStatusToAccepted(
                It.IsAny<int>(),
                It.IsAny<SupportActivities>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())) 
                .Returns(()=> _newJobId);

            _repository.Setup(x => x.VolunteerAlreadyAcceptedShift(
                It.IsAny<int>(),
                It.IsAny<SupportActivities>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _jobId);
                

        }

        [Test]
        public async Task WhenVolunteerAndRequestedByIsTheSame_ReturnsTrue()
        {
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 1,
                RequestID = 1,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = SupportActivities.VolunteerSupport },
                VolunteerUserID = 1
            };
            _jobId = 0;
            _newJobId = 1;
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(UpdateJobStatusOutcome.Success, response.Outcome);
        }

        [Test]
        public async Task WhenSuccessfullyChangingJobStatusToAccepted_ReturnsTrue()
        {
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 1,
                RequestID = 1,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = SupportActivities.VolunteerSupport},
                VolunteerUserID = 2
            };
            _jobId = 0;
            _newJobId = 1;
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(UpdateJobStatusOutcome.Success, response.Outcome);
        }

        [Test]
        public async Task WhenUnSuccessfullyChangingJobStatusToAccepted_ReturnsFalse()
        {
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 1,
                RequestID = 1,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = SupportActivities.VolunteerSupport },
                VolunteerUserID = 1
            };
            _jobId = 1;
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.AlreadyInThisStatus, response.Outcome);
        }

        [Test]
        public async Task WhenVolunteerDoesNotHavePermission_ReturnsUnauthorised()
        {
            _hasPermission = false;
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 1,
                RequestID = 1,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = SupportActivities.VolunteerSupport },
                VolunteerUserID = 2
            };
            _jobId = 0;
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.Unauthorized, response.Outcome);
        }

        [Test]
        public async Task WhenRequestStatusIsAlreadyAccepted_ReturnsAlreadyInThisStatus()
        {

            _hasPermission = true;
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 1,
                RequestID = 1,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = SupportActivities.VolunteerSupport },
                VolunteerUserID = 1
            };
            _jobId = 1;
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(UpdateJobStatusOutcome.AlreadyInThisStatus, response.Outcome);
        }

        [Test]
        public async Task WhenVolunteerAlreadyAssignedForRequestAndSupportActivity_ReturnsNoLongerAvailable()
        {
            _hasPermission = true;
            int requestID = 1;
            SupportActivities activity = SupportActivities.VolunteerSupport;
            int volunteerUserID = 1;
            _request = new PutUpdateShiftStatusToAcceptedRequest
            {
                CreatedByUserID = 2,
                RequestID = requestID,
                SupportActivity = new SingleSupportActivityRequest() { SupportActivity = activity },
                VolunteerUserID = volunteerUserID
            };
            _jobId = 0;

            _repository.Setup(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Throws(new UnableToUpdateShiftException($"Unable to UpdateShiftStatus for RequestID:{requestID} SupportActivity:{activity} Volunteer:{volunteerUserID}"));

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _repository.Verify(x => x.VolunteerAlreadyAcceptedShift(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobService.Verify(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.UpdateRequestStatusToAccepted(It.IsAny<int>(), It.IsAny<SupportActivities>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(UpdateJobStatusOutcome.NoLongerAvailable, response.Outcome);
        }


    }
}