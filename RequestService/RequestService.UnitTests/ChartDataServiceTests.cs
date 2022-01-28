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

            _repository.Setup(x => x.GetActivitiesByMonth(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _activitiesByMonth);

            _repository.Setup(x => x.RecentActiveVolunteersByVolumeAcceptedRequests(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volunteers);

            _repository.Setup(x => x.RequestVolumeByActivity(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volumeByActivity);

            _repository.Setup(x => x.RequestVolumeByDueDateAndRecentStatus(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _volumeByDueDateAndRecentStatus);
        }

        [Test]
        public async Task ActivitiesByMonth_Check_WhereCountGreaterThanZero()
        {
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

            List<DataPoint> result = await _classUnderTest.GetActivitiesByMonth(-1);

            foreach(var item in expectedOutcome)
            {
                var actual = result.Where(x => x.Series == item.Key.series && x.XAxis == item.Key.xAxis).Select(x=> x.Value).FirstOrDefault();
                Assert.AreEqual(actual, item.Value);
            }

        }
        
    }
}