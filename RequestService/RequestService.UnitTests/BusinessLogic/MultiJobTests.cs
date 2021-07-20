using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using NUnit.Framework;
using RequestService.Handlers.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestService.UnitTests.BusinessLogic
{
    public class MultiJobTests
    {
        private MultiJobs _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new MultiJobs();
        }

        
        [Test]
        public void WhenVoluteerRequiredQuestionAnswered_MultiJobsAdded()
        {
            int numberOfVolunteers = 5;

            HelpRequestDetail helpRequestDetail = new HelpRequestDetail()
            {
                HelpRequest = new HelpRequest()
                {

                },
                NewJobsRequest = new NewJobsRequest()
                {
                    Jobs = new List<Job>()
                    {
                        new Job()
                        {
                            StartDate = DateTime.Now,
                            NotBeforeDate = DateTime.Now,
                            HealthCritical = false,
                            SupportActivity = SupportActivities.Shopping,
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 17,
                                    Name = "How many volunteers are required?",
                                   Answer = numberOfVolunteers.ToString()
                                }
                            }
                        }
                    }
                }
            };
            _classUnderTest.AddMultiVolunteers(helpRequestDetail);
            Assert.AreEqual(numberOfVolunteers, helpRequestDetail.NewJobsRequest.Jobs.Count);
        }

        [Test]
        public void WhenVoluteerRequiredQuestionNotAnswered_NoMultiJobsAdded()
        {
            List<Job> jobs = new List<Job>()
            {
                new Job()
                {
                    HealthCritical = false,
                    SupportActivity = SupportActivities.Shopping
                }
            };

            int jobCount = jobs.Count;

            HelpRequestDetail helpRequestDetail = new HelpRequestDetail()
            {
                HelpRequest = new HelpRequest()
                {

                },
                NewJobsRequest = new NewJobsRequest()
                {
                    Jobs = jobs
                }
            };

            _classUnderTest.AddMultiVolunteers(helpRequestDetail);
            Assert.AreEqual(jobCount, helpRequestDetail.NewJobsRequest.Jobs.Count);
        }

        [Test]
        [TestCase(2, Frequency.Weekly)]
        [TestCase(2, Frequency.Fortnightly)]
        [TestCase(2, Frequency.EveryFourWeeks)]
        [TestCase(2, Frequency.Daily)]
        [TestCase(5, Frequency.Weekly)]
        public void WhenRepeatFrequencyPassedIn_MultiJobsAdded(int numberOfRepeats, Frequency frequency)
        {
            List<Job> jobs = new List<Job>()
            {
                new Job()
                {
                    StartDate = DateTime.Now,
                    NotBeforeDate = DateTime.Now,
                    HealthCritical = false,
                    SupportActivity = SupportActivities.Shopping,
                    RepeatFrequency = frequency,
                    NumberOfRepeats = numberOfRepeats
                }
            };
            int jobCount = jobs.Count;

            NewJobsRequest request = new NewJobsRequest()
            {
                Jobs = jobs
            };
            _classUnderTest.AddRepeats(request, DateTime.UtcNow);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Count);            
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Select(x => x.StartDate).Distinct().Count());
        }

        [TestCase(5, Frequency.Weekly)]
        public void WhenRepeatFrequencyPassedIn_MultiJobsAdded2(int numberOfRepeats, Frequency frequency)
        {
            List<Job> jobs = new List<Job>()
            {
                new Job()
                {
                    StartDate = DateTime.Now,
                    NotBeforeDate = DateTime.Now.AddDays(-3),
                    HealthCritical = false,
                    SupportActivity = SupportActivities.Shopping,
                    RepeatFrequency = frequency,
                    NumberOfRepeats = numberOfRepeats
                }
            };
            int jobCount = jobs.Count;

            NewJobsRequest request = new NewJobsRequest()
            {
                Jobs = jobs
            };
            _classUnderTest.AddRepeats(request, DateTime.UtcNow);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Count);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Select(x => x.StartDate).Distinct().Count());
        }

        [TestCase(5, Frequency.Weekly)]
        [TestCase(2, Frequency.EveryFourWeeks)]
        public void WhenShiftRepeatFrequencyPassedIn_MultiRequestsAdded(int numberOfRepeats, Frequency frequency)
        {
            List<Job> jobs = new List<Job>()
            {
                new Job()
                {
                    StartDate = DateTime.Now,
                    NotBeforeDate = DateTime.Now.AddDays(-3),
                    HealthCritical = false,
                    SupportActivity = SupportActivities.VaccineSupport,
                    RepeatFrequency = frequency,
                    NumberOfRepeats = numberOfRepeats
                },
                new Job()
                {
                    StartDate = DateTime.Now,
                    NotBeforeDate = DateTime.Now.AddDays(-3),
                    HealthCritical = false,
                    SupportActivity = SupportActivities.VaccineSupport,
                    RepeatFrequency = frequency,
                    NumberOfRepeats = numberOfRepeats
                }
            };
            int jobCount = jobs.Count;

            List<HelpRequestDetail> helpRequestDetails = new List<HelpRequestDetail>()
            {
                new HelpRequestDetail()
                {
                    HelpRequest = new HelpRequest(),
                    NewJobsRequest = new NewJobsRequest()
                    {
                       Jobs = jobs
                    }
                }
            };
            
            _classUnderTest.AddShiftRepeats(helpRequestDetails, numberOfRepeats);
            Assert.AreEqual(numberOfRepeats, helpRequestDetails.Count);
            Assert.AreEqual(jobCount * numberOfRepeats, helpRequestDetails.Sum(x => x.NewJobsRequest.Jobs.Count));
        }

    }
}