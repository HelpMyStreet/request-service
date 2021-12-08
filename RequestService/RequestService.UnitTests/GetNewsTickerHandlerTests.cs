using HelpMyStreet.Contracts;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.EqualityComparers;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using Moq;
using NUnit.Framework;
using RequestService.Core.Domains;
using RequestService.Core.Exceptions;
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
    public class GetNewsTickerHandlerTests
    {
        private Mock<IRepository> _repository;
        private GetNewsTickerHandler _classUnderTest;
        private List<SupportActivityCount> _completedActivities;
        private List<SupportActivityCount> _completedActivitiesCompletedLastXDays;
        private List<SupportActivityCount> _requestsAddedLastXDays;
        private int _openJobCount;
        private IEqualityComparer<NewsTickerMessage> _equalityComparer;

        [SetUp]
        public void Setup()
        {
            SetupRepository();
            _equalityComparer = new NewsTickerMessages_EqualityComparer();
            _classUnderTest = new GetNewsTickerHandler(_repository.Object);
        }

        private void SetupRepository()
        {
            _openJobCount = 0;
            _completedActivities = new List<SupportActivityCount>();
            _completedActivitiesCompletedLastXDays = new List<SupportActivityCount>();
            _requestsAddedLastXDays = new List<SupportActivityCount>();

            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.GetCompletedActivitiesCount(It.IsAny<int?>()))
                .ReturnsAsync(()=> _completedActivities);

            _repository.Setup(x => x.GetActivitiesCompletedLastXDaysCount(It.IsAny<int?>(), It.IsAny<int>()))
                .ReturnsAsync(() => _completedActivitiesCompletedLastXDays);

            _repository.Setup(x => x.GetRequestsAddedLastXDaysCount(It.IsAny<int?>(), It.IsAny<int>()))
                .ReturnsAsync(() => _requestsAddedLastXDays);

            _repository.Setup(x => x.OpenJobCount(It.IsAny<int?>()))
                .ReturnsAsync(() => _openJobCount);
        }

        [TestCase(20, 8, 15, 11, 15, 5)]
        [TestCase(20, 8, 15, 25, 0, 4)]
        [TestCase(8, 7, 6, 10, 0, 1)]
        [TestCase(6, 6, 6, 10, 0, 0)]
        [Test]
        public async Task WhenCompletedActivitiesExceedThreshold_Then_AddsToMessages(int shoppingCount, int facemaskCount, int homeworkSupportCount, int vaccineSupportCount, int bankStaffVaccinatorCount, int messageCount)
        {
            int? groupId = -3;
            _openJobCount = 0;
            _completedActivitiesCompletedLastXDays = new List<SupportActivityCount>();
            _requestsAddedLastXDays = new List<SupportActivityCount>();
            _completedActivities = new List<SupportActivityCount>()
            {
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.VaccineSupport,
                    Value = vaccineSupportCount
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.BankStaffVaccinator,
                    Value = bankStaffVaccinatorCount
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.Shopping,
                    Value = shoppingCount
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.FaceMask,
                    Value = facemaskCount
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.HomeworkSupport,
                    Value = homeworkSupportCount
                }
            };

            NewsTickerResponse response = await _classUnderTest.Handle(new NewsTickerRequest()
            {
                GroupId = groupId
            }, CancellationToken.None);

            _completedActivities.Where(x => x.Value > 10).OrderByDescending(x => x.Value).Take(3)
                .ToList()
                .ForEach(item =>
                {
                    Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
                    {
                        SupportActivity = item.SupportActivity,
                        Value = item.Value,
                        Message = $"**{ item.Value }** *{item.SupportActivity.FriendlyNameShort().ToLower()}* { item.SupportActivity.RequestType().FriendlyName(Convert.ToInt32(item.Value)) } completed"
                    }, _equalityComparer));

                });

            Assert.AreEqual(messageCount, response.Messages.Count);

            var totalRequests = shoppingCount + facemaskCount + homeworkSupportCount;

            if (totalRequests > 20)
            {
                Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
                {
                    Value = totalRequests,
                    Message = $"**{ totalRequests }** {RequestType.Task.FriendlyName(Convert.ToInt32(totalRequests))} completed"
                }, _equalityComparer));
            }

            var totalShifts = vaccineSupportCount;

            if (totalShifts > 20 && totalShifts > (totalShifts * 1.1))
            {
                Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
                {
                    Value = totalShifts,
                    Message = $"**{ totalShifts }** {RequestType.Task.FriendlyName(Convert.ToInt32(totalShifts))} completed"
                }, _equalityComparer));
            }

        }        

        [Test]
        public async Task WhenCompletedLastXDays_Then_AddsToMessages()
        {
            int? groupId = -3;
            _openJobCount = 0;
            _completedActivities = new List<SupportActivityCount>();
            _requestsAddedLastXDays = new List<SupportActivityCount>();
            _completedActivitiesCompletedLastXDays = new List<SupportActivityCount>()
            {
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.Shopping,
                    Value = 20
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.FaceMask,
                    Value = 8
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.HomeworkSupport,
                    Value = 15
                }
            };

            NewsTickerResponse response = await _classUnderTest.Handle(new NewsTickerRequest()
            {
                GroupId = groupId
            }, CancellationToken.None);

            _completedActivitiesCompletedLastXDays
                .ForEach(item =>
                {
                    Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
                    {
                        SupportActivity = item.SupportActivity,
                        Value = item.Value,
                        Message = $"Completed today: **{ item.SupportActivity.FriendlyNameShort() }**"
                    }, _equalityComparer));

                });
            
            Assert.AreEqual(_completedActivitiesCompletedLastXDays.Count, response.Messages.Count);
        }

        [Test]
        public async Task WhenRequestsAddedwithinLastWeek_Then_AddsToMessages()
        {
            int? groupId = -3;
            _openJobCount = 0;
            _completedActivities = new List<SupportActivityCount>();
            _completedActivitiesCompletedLastXDays = new List<SupportActivityCount>();
            _requestsAddedLastXDays = new List<SupportActivityCount>()
            {
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.Shopping,
                    Value = 20
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.FaceMask,
                    Value = 8
                },
                new SupportActivityCount()
                {
                    SupportActivity = SupportActivities.HomeworkSupport,
                    Value = 15
                }
            };

            NewsTickerResponse response = await _classUnderTest.Handle(new NewsTickerRequest()
            {
                GroupId = groupId
            }, CancellationToken.None);

            var totalRequestsAddedThisWeek = _requestsAddedLastXDays.Sum(x => x.Value);
            Assert.AreEqual(1, response.Messages.Count);

            Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
            {
                Value = totalRequestsAddedThisWeek,
                Message = $"**{totalRequestsAddedThisWeek}** new requests added this week"
            }, _equalityComparer));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [Test]
        public async Task WhenOpenJobCountGreater_Then_AddsToMessages(int openJobCount)
        {
            int? groupId = -3;
            _openJobCount = openJobCount;
            _completedActivities = new List<SupportActivityCount>();
            _completedActivitiesCompletedLastXDays = new List<SupportActivityCount>();
            _requestsAddedLastXDays = new List<SupportActivityCount>();

            NewsTickerResponse response = await _classUnderTest.Handle(new NewsTickerRequest()
            {
                GroupId = groupId
            }, CancellationToken.None);

            if (_openJobCount > 1)
            {
                Assert.AreEqual(true, response.Messages.Contains(new NewsTickerMessage()
                {
                    Value = openJobCount,
                    Message = $"**{openJobCount}** open jobs waiting for a volunteer"
                }, _equalityComparer));
            }

        }



    }
}