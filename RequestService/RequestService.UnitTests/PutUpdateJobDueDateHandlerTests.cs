using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Moq;
using NUnit.Framework;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PutUpdateJobDueDateHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IJobService> _jobService;

        private PutUpdateJobDueDateHandler _classUnderTest;
        private PutUpdateJobDueDateRequest _request;
        private GetJobDetailsResponse _getJobDetailsResponse;
        private UpdateJobOutcome _updateJobOutcome;
        private bool _hasPermission = true;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupCommunicationService();
            SetupJobService();
            _classUnderTest = new PutUpdateJobDueDateHandler(_repository.Object, _communicationService.Object,_jobService.Object);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();

            _repository.Setup(x => x.GetJobDetails(It.IsAny<int>()))
               .Returns(() => _getJobDetailsResponse);

            _repository.Setup(x => x.UpdateJobDueDateAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _updateJobOutcome);

            _repository.Setup(x => x.UpdateHistory(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int>()
                ));
        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();
            _jobService.Setup(x => x.HasPermissionToChangeJobAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _hasPermission);
        }

        private void SetupCommunicationService()
        {
            _communicationService = new Mock<ICommunicationService>();
            _communicationService.Setup(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        }

        [TestCase(UpdateJobOutcome.BadRequest, -1, JobStatuses.Open, 0, 0, 0)]
        [TestCase(UpdateJobOutcome.Unauthorized, 1, JobStatuses.Cancelled, 1, 1, 0)]
        [TestCase(UpdateJobOutcome.Unauthorized, 1, JobStatuses.Done, 1, 1, 0)]
        [TestCase(UpdateJobOutcome.Success, 1, JobStatuses.Open, 1, 1, 1)]
        [TestCase(UpdateJobOutcome.Success, 1, JobStatuses.New, 1, 1, 1)]
        [TestCase(UpdateJobOutcome.Success, 1, JobStatuses.InProgress, 1, 1, 1)]
        [TestCase(UpdateJobOutcome.Success, 1, JobStatuses.Accepted, 1, 1, 1)]
        [Test]
        public async Task WhenSuccessfullyChangingJobDueDate_ReturnsTrue(
            UpdateJobOutcome outcome, 
            int daysToAdd, 
            JobStatuses jobStatus, 
            int callsToPermission, 
            int callsToGetJobDetails, 
            int callsToUpdateDueDate)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            _updateJobOutcome = outcome;
            _request = new PutUpdateJobDueDateRequest
            { 
                AuthorisedByUserID = 2,
                JobID = 1,
                DueDate = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(daysToAdd),info)

            };

            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobStatus = jobStatus,
                    DueDate = TimeZoneInfo.ConvertTime(new DateTime(2021, 1, 1, 9, 0, 0), info)
                }
            };

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _jobService.Verify(x => x.HasPermissionToChangeJobAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(callsToPermission));
            _repository.Verify(x => x.GetJobDetails(It.IsAny<int>()), Times.Exactly(callsToGetJobDetails));            
            _repository.Verify(x => x.UpdateJobDueDateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Exactly(callsToUpdateDueDate));
            _repository.Verify(x => x.UpdateHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int>()), Times.Exactly(_updateJobOutcome == UpdateJobOutcome.Success ? 1 : 0));
            
            Assert.AreEqual(outcome, response.Outcome);
        }
        
    }
}