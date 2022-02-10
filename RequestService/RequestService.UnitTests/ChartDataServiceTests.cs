using HelpMyStreet.Contracts.AddressService.Response;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.ReportService;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using Microsoft.Extensions.Internal;
using Moq;
using NUnit.Framework;
using RequestService.Core.Domains;
using RequestService.Core.Dto;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Core.Utils;

namespace RequestService.UnitTests
{
    public class ChartDataServiceTests
    {
        private Mock<IRepository> _repository;
        private Mock<ISystemClock> _mockableDateTime;
        private Mock<IGroupService> _groupService;
        private List<int?> _volunteers;
        private List<JobBasic> _jobBasics;
        private GetChildGroupsResponse _childGroupResponse;
        private ChartDataService _classUnderTest;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            SetupGroupService();

            _mockableDateTime = new Mock<ISystemClock>();
            _mockableDateTime.Setup(x => x.UtcNow).Returns(new DateTime(2022, 1, 20, 1, 45, 00, 00, DateTimeKind.Utc));

            _classUnderTest = new ChartDataService(_repository.Object, _groupService.Object,_mockableDateTime.Object);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();

            _jobBasics = new List<JobBasic>()
            {
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.Shopping, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1) },
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.CollectingPrescriptions, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1)},
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.DogWalking, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1)},
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.Shopping, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1)},
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.CollectingPrescriptions, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1)},
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.DogWalking, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,1,1)},
                new JobBasic(){DateRequested = new DateTime(2021,2,1), SupportActivity = SupportActivities.Shopping, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,2,1)},
                new JobBasic(){DateRequested = new DateTime(2021,2,1), SupportActivity = SupportActivities.CollectingPrescriptions, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,2,1)},
                new JobBasic(){DateRequested = new DateTime(2021,3,1), SupportActivity = SupportActivities.DogWalking, JobStatus = JobStatuses.Open, DueDate = new DateTime(2021,3,1)},
                new JobBasic(){DateRequested = new DateTime(2021,1,1), SupportActivity = SupportActivities.Shopping, JobStatus = JobStatuses.Cancelled, DueDate = new DateTime(2021,1,1) },
                new JobBasic(){DateRequested = new DateTime(2022,1,1), SupportActivity = SupportActivities.Shopping, JobStatus = JobStatuses.Accepted, DueDate = new DateTime(2022,1,31) },
            };

            _repository.Setup(x => x.GetActivitiesByMonth(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _jobBasics);

            _repository.Setup(x => x.RecentActiveVolunteersByVolumeAcceptedRequests(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volunteers);

            _repository.Setup(x => x.RequestVolumeByActivity(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _jobBasics);

            _repository.Setup(x => x.RequestVolumeByDueDateAndRecentStatus(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _jobBasics);

            _childGroupResponse = new GetChildGroupsResponse()
            {
                ChildGroups = new List<Group>()
                {
                    new Group()
                    {
                        GroupId = -1
                    }
                }
            };
        }

        private void SetupGroupService()
        {
            _groupService = new Mock<IGroupService>();
            _groupService.Setup(x => x.GetChildGroups(It.IsAny<int>()))
                .ReturnsAsync(() => _childGroupResponse);

        }

        [Test]
        public async Task ActivitiesByMonth_Check()
        {
            int groupId = -1;
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime maxDate = new DateTime(2022, 1, 31);

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-01"), 3);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-03"), 1);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-03"), 0);
     
            List<DataPoint> result = await _classUnderTest.GetActivitiesByMonth(groupId, minDate, maxDate);

            foreach(var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x=> x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }
    
        [Test]
        public async Task RequestVolumeByActivity_Check()
        {
            int groupId = -1;
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 31);

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-01"), 3);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-03"), 1);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-03"), 0);

            List<DataPoint> result = await _classUnderTest.RequestVolumeByActivity(groupId, minDate, maxDate);

            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task RequestVolumeByDueDateAndRecentStatus_Check()
        {
            int groupId = -1;
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 28);

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();

            expectedOutcome.Add(("Overdue", "2021-01"), 6);
            expectedOutcome.Add(("Cancelled", "2021-01"), 1);
            expectedOutcome.Add(("Accepted", "2022-01"), 1);

            List<DataPoint> result = await _classUnderTest.RequestVolumeByDueDateAndRecentStatus(groupId, minDate, maxDate);
            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task RecentActiveVolunteersByVolumeAcceptedRequests_Check()
        {
            int groupId = -1;
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 28);

            _volunteers = new List<int?>()
            {
                1,1,2,2,2,2,3,3,3,3,3,3,3,4,5,6,6,6,6,6,6,6,6,6,6
            };

            Dictionary<string, int> expectedOutcome = new Dictionary<string, int>();
            expectedOutcome.Add("1 accepted request", 2);
            expectedOutcome.Add("2-3 accepted requests",1);
            expectedOutcome.Add("4-5 accepted requests", 1);
            expectedOutcome.Add("6-9 accepted requests", 1);
            expectedOutcome.Add("10+ accepted requests", 1);


            List<DataPoint> result = await _classUnderTest.RecentActiveVolunteersByVolumeAcceptedRequests(groupId, minDate, maxDate);
            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.XAxis == item.Key).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }
    }
}