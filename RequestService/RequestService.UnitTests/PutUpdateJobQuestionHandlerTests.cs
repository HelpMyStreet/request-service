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
        private Mock<ICommunicationService> _communicationService;

        private PutUpdateJobQuestionHandler _classUnderTest;
        private PutUpdateJobQuestionRequest _request;
        private UpdateJobOutcome _updateJobOutcome;
        private bool _hasPermission;
        private GetJobDetailsResponse _getJobDetailsResponse;
        private bool _sendCommunication;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupJobService();
            SetupCommunicationService();
            _classUnderTest = new PutUpdateJobQuestionHandler(_repository.Object, _jobService.Object, _communicationService.Object) ;
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.UpdateJobQuestion(
                It.IsAny<int>(), 
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=> _updateJobOutcome);

            _repository.Setup(x => x.GetJobDetails(It.IsAny<int>()))
                .Returns(() => _getJobDetailsResponse);

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
            _communicationService.Setup(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _sendCommunication);
        }

        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.Open, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.New, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.BadRequest, true, JobStatuses.Cancelled, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.Accepted, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.BadRequest, true, JobStatuses.Done, 1, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.BadRequest, true, JobStatuses.InProgress, Questions.SuppressRecipientPersonalDetails, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Success, true, JobStatuses.InProgress, 1, 1, 1)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.Open, 2, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, true, JobStatuses.New, 2, 1, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, false, JobStatuses.Open, 1, 0, 0)]
        [TestCase(UpdateJobStatusOutcome.Unauthorized, false, JobStatuses.New, 1, 0, 0)]
        [Test]
        public async Task TestUpdateJobQuestions(UpdateJobOutcome outcome, bool hasPermission, JobStatuses jobStatus, int questionId, int numberOfCallsToGetJobdetails, int numberOfCallsToUpdateQuestion)
        {
            _updateJobOutcome = outcome;
            _hasPermission = hasPermission;
            _request = new PutUpdateJobQuestionRequest
            {
                JobID = 1,
                QuestionID = questionId,
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
            _repository.Verify(x => x.UpdateHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int>()), Times.Exactly(numberOfCallsToUpdateQuestion));
            _communicationService.Verify(x => x.RequestCommunication(It.IsAny<RequestCommunicationRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(_updateJobOutcome == UpdateJobOutcome.Success ? 1 : 0));

            Assert.AreEqual(outcome, response.Outcome);
        }
    }
}