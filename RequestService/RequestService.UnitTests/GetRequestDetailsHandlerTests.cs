using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
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
    public class GetRequestDetailsHandlerTests
    {
        private Mock<IRepository> _repository;
        private Mock<IJobService> _jobService;

        private GetRequestDetailsHandler _classUnderTest;
        private GetRequestDetailsRequest _request;
        private GetRequestDetailsResponse _response;

        private bool _permission;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupJobService();
            _classUnderTest = new GetRequestDetailsHandler(_repository.Object,_jobService.Object);
            _response = new GetRequestDetailsResponse()
            {
                RequestSummary = new HelpMyStreet.Utils.Models.RequestSummary()
                {
                    Shift = new HelpMyStreet.Utils.Models.Shift()
                    {
                        StartDate = DateTime.Now,
                        ShiftLength = 10
                    },
                    
                    JobSummaries = new System.Collections.Generic.List<HelpMyStreet.Utils.Models.JobSummary>()
                     {
                         new HelpMyStreet.Utils.Models.JobSummary()
                        {
                            JobID = 1,
                            SupportActivity = HelpMyStreet.Utils.Enums.SupportActivities.Shopping,
                            JobStatus = HelpMyStreet.Utils.Enums.JobStatuses.New
                         },
                        new HelpMyStreet.Utils.Models.JobSummary()
                        {
                            JobID = 1,
                            SupportActivity = HelpMyStreet.Utils.Enums.SupportActivities.CollectingPrescriptions,
                            JobStatus = HelpMyStreet.Utils.Enums.JobStatuses.New
                        }
                     }
                }
            };
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.GetRequestDetails(It.IsAny<int>())).Returns(()=>_response);
        }

        private void SetupJobService()
        {
            _jobService = new Mock<IJobService>();
            _jobService.Setup(x => x.HasPermissionToChangeRequestAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _permission);
        }

        [Test]
        public async Task WhenPassesInKnownRequestID_ReturnsDetails()
        {
            _permission = true;
            _request = new GetRequestDetailsRequest
            {
                RequestID = 1,
                AuthorisedByUserID = -1
            };

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            Assert.AreEqual(_response, response);
        }

        [Test]
        public async Task WhenPassesInKnownRequestIDButUserIsNotAuthorised_ReturnsNull()
        {
            _permission = false;
            _request = new GetRequestDetailsRequest
            {
                RequestID = 1,
                AuthorisedByUserID = -1
            };
            _response = null;

            var response = await _classUnderTest.Handle(_request, CancellationToken.None);
            Assert.AreEqual(_response, response);
        }

    }
}