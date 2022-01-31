using HelpMyStreet.Contracts.AddressService.Response;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.ReportService;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
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
        private List<DataItem> _activitiesByMonth;
        private List<DataItem> _volumeByActivity;
        private List<DataItem> _volumeByDueDateAndRecentStatus;
        private List<int?> _volunteers;
        private ChartDataService _classUnderTest;
        private Task<List<DataPoint>> result;

        [SetUp]
        public void Setup()
        {
            
            SetupRepository();
            
            _classUnderTest = new ChartDataService(_repository.Object);
        }

        private void SetupRepository()
        {
            _repository = new Mock<IRepository>();

            _repository.Setup(x => x.GetActivitiesByMonth(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _activitiesByMonth);

            _repository.Setup(x => x.RecentActiveVolunteersByVolumeAcceptedRequests(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volunteers);

            _repository.Setup(x => x.RequestVolumeByActivity(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volumeByActivity);

            _repository.Setup(x => x.RequestVolumeByDueDateAndRecentStatus(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volumeByDueDateAndRecentStatus);
        }

        [Test]
        public async Task ActivitiesByMonth_Check()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime maxDate = new DateTime(2022, 1, 31);

            _activitiesByMonth = new List<DataItem>()
            {
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,3,1), Series = SupportActivities.DogWalking.FriendlyNameShort()}
            };

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-01"), 2);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-02"), 1);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-03"), 1);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-03"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-04"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-05"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-06"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-07"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-08"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-09"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-10"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-11"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2021-12"), 0);
            expectedOutcome.Add((SupportActivities.Shopping.FriendlyNameShort(), "2022-01"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-03"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-04"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-05"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-06"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-07"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-08"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-09"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-10"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-11"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2021-12"), 0);
            expectedOutcome.Add((SupportActivities.CollectingPrescriptions.FriendlyNameShort(), "2022-01"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-02"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-04"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-05"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-06"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-07"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-08"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-09"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-10"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-11"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2021-12"), 0);
            expectedOutcome.Add((SupportActivities.DogWalking.FriendlyNameShort(), "2022-01"), 0);

            minDate = new DateTime(2021, 1, 1);
            List<DataPoint> result = await _classUnderTest.GetActivitiesByMonth(-1, minDate, maxDate);

            foreach(var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x=> x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task ActivitiesByMonth_Check2()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 31);

            _activitiesByMonth = new List<DataItem>()
            {
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,3,1), Series = SupportActivities.DogWalking.FriendlyNameShort()}
            };

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();
            _activitiesByMonth.Select(s => s.Series).Distinct().ToList()
                .ForEach(item =>
                {
                    startDate = minDate;
                    while (startDate <= maxDate)
                    {
                        var count = _activitiesByMonth.Count(x => x.Date == startDate && x.Series == item);
                        expectedOutcome.Add((item, $"{startDate:yyyy}-{startDate:MM}"), count);
                        startDate = startDate.AddMonths(1);
                    }
                });

            List<DataPoint> result = await _classUnderTest.GetActivitiesByMonth(-1, minDate, maxDate);

            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task RequestVolumeByActivity_Check()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 31);

            _volumeByActivity = new List<DataItem>()
            {
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = SupportActivities.DogWalking.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.Shopping.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,2,1), Series = SupportActivities.CollectingPrescriptions.FriendlyNameShort()},
                new DataItem(){Date = new DateTime(2021,3,1), Series = SupportActivities.DogWalking.FriendlyNameShort()}
            };

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();
            _volumeByActivity.Select(s => s.Series).Distinct().ToList()
                .ForEach(item =>
                {
                    startDate = minDate;
                    while (startDate <= maxDate)
                    {
                        var count = _volumeByActivity.Count(x => x.Date == startDate && x.Series == item);
                        expectedOutcome.Add((item, $"{startDate:yyyy}-{startDate:MM}"), count);
                        startDate = startDate.AddMonths(1);
                    }
                });

            List<DataPoint> result = await _classUnderTest.RequestVolumeByActivity(-1, minDate, maxDate);

            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task RequestVolumeByDueDateAndRecentStatus_Check()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime startDate = minDate;
            DateTime maxDate = new DateTime(2022, 1, 28);

            _volumeByDueDateAndRecentStatus = new List<DataItem>()
            {
                new DataItem(){Date = new DateTime(2021,1,1), Series = JobStatuses.Open.FriendlyName()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = JobStatuses.Open.FriendlyName()},
                new DataItem(){Date = new DateTime(2021,1,1), Series = JobStatuses.Cancelled.FriendlyName()},
                new DataItem(){Date = new DateTime(2022,1,31), Series = JobStatuses.InProgress.FriendlyName()}
            };

            Dictionary<(string series, string xAxis), double> expectedOutcome = new Dictionary<(string xAxis, string series), double>();

            expectedOutcome.Add(("Overdue", "2021-01"), 2);
            expectedOutcome.Add(("Cancelled", "2021-01"), 1);
            expectedOutcome.Add(("Accepted", "2022-01"), 1);

            List<DataPoint> result = await _classUnderTest.RequestVolumeByDueDateAndRecentStatus(-1, minDate, maxDate);
            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }

        [Test]
        public async Task RecentActiveVolunteersByVolumeAcceptedRequests_Check()
        {
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


            List<DataPoint> result = await _classUnderTest.RecentActiveVolunteersByVolumeAcceptedRequests(-1, minDate, maxDate);
            foreach (var item in expectedOutcome)
            {
                var actual = result.Where(x => x.XAxis == item.Key).Select(x => x.Value).First();
                Assert.AreEqual(actual, item.Value);
            }
        }
    }
}