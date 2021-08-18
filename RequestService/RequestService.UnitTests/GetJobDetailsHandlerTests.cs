using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.UserService.Response;
using HelpMyStreet.Utils.Models;
using Moq;
using NUnit.Framework;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using RequestService.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public class MultiJobTests
    {
        private Mock<IRepository> _repository;
        private Mock<IJobService> _jobService;
        private Mock<IUserService> _userService;

        private GetJobDetailsHandler _classUnderTest;
        private GetJobDetailsRequest _request;
        private GetJobDetailsResponse _response;
        private GetUserByIDResponse _user;

        private bool _permission;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupJobService();
            SetupUserService();
            _classUnderTest = new GetJobDetailsHandler(_repository.Object,_jobService.Object, _userService.Object);
            _response = new GetJobDetailsResponse()
            {
                 JobSummary = new HelpMyStreet.Utils.Models.JobSummary()
                 {
                     Details = "DETAILS",
                     JobID = 1,
                 },
                 RequestSummary = new RequestSummary()
                 {
                     JobSummaries = new System.Collections.Generic.List<JobSummary>()
                     {
                         new JobSummary()
                         {
                             JobID = 1
                         }
                     },
                 }
            };
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.GetJobDetails(It.IsAny<int>())).Returns(()=>_response);
        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();
            _jobService.Setup(x => x.HasPermissionToChangeStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _permission);
        }

        private void SetupUserService()
        {
            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetUser(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _user);
        }

        [Test]
        public async Task WhenPassesInKnownJobID_ReturnsDetails()
        {
            _permission = true;
            _request = new GetJobDetailsRequest
            {
                JobID = 1
            };
            _user = new GetUserByIDResponse()
            { 
                User = new User()
                {
                    PostalCode = "PostCode"
                }
            };

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            Assert.AreEqual(_request.JobID, response.JobSummary.JobID);
            Assert.AreEqual(_response, response);
        }

    }
}