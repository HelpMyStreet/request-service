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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class PutUpdateJobQuestionHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<IJobService> _jobService;

        private PutUpdateJobQuestionHandler _classUnderTest;
        private PutUpdateJobQuestionRequest _request;
        private UpdateJobStatusOutcome _updateJobStatusOutcome;
        private bool _hasPermission;
        private GetJobDetailsResponse _getJobDetailsResponse;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupJobService();
            _classUnderTest = new PutUpdateJobQuestionHandler(_repository.Object,_jobService.Object);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.UpdateJobQuestion(
                It.IsAny<int>(), 
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _updateJobStatusOutcome);

            _repository.Setup(x => x.GetJobDetails(It.IsAny<int>()))
                .Returns(() => _getJobDetailsResponse);

        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();
            _jobService.Setup(x => x.HasPermissionToChangeJobAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _hasPermission);
        }

        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.Open, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.New, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.Cancelled, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.Accepted, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.Done, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.InProgress, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.Open, 2, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.New, 2, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, false, JobStatuses.Open, 1, 0, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, false, JobStatuses.New, 1, 0, 0)]
        [Test]
        public async Task TestUpdateDueDate(UpdateJobStatusOutcome outcome, bool hasPermission, JobStatuses jobStatus, int questionId, int numberOfCallsToGetJobdetails, int numberOfCallsToUpdateQuestion)
        {
            _updateJobStatusOutcome = outcome;
            _hasPermission = hasPermission;
            _request = new PutUpdateJobQuestionRequest
            {
                JobID = 1,
                QuestionID = 1,
                AuthorisedByUserID = 2,
                Answer = "Answer"
            };
            _getJobDetailsResponse = new GetJobDetailsResponse()
            {
                JobSummary = new JobSummary()
                {
                    JobStatus = jobStatus,
                    Questions = new System.Collections.Generic.List<Question>()
                    {
                        new Question()
                        {
                            Id = questionId
                        }
                    }
                }
            };
  
            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            _jobService.Verify(x => x.HasPermissionToChangeJobAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            _repository.Verify(x => x.GetJobDetails(It.IsAny<int>()), Times.Exactly(numberOfCallsToGetJobdetails));
            _repository.Verify(x => x.UpdateJobQuestion(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(numberOfCallsToUpdateQuestion));
            Assert.AreEqual(outcome, response.Outcome);
        }
    }
}