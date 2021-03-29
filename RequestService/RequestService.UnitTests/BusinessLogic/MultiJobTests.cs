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

            NewJobsRequest request = new NewJobsRequest()
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
            };
            _classUnderTest.AddMultiVolunteers(request);
            Assert.AreEqual(numberOfVolunteers, request.Jobs.Count);
            Assert.AreEqual(numberOfVolunteers-1, request.Jobs.Count(x=> x.Multi==true));
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
            NewJobsRequest request = new NewJobsRequest()
            {
                Jobs = jobs
            };
            _classUnderTest.AddMultiVolunteers(request);
            Assert.AreEqual(jobCount, request.Jobs.Count);
            Assert.AreEqual(0, request.Jobs.Count(x => x.Multi == true));
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
            _classUnderTest.AddRepeats(request);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Count);            
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Select(x => x.StartDate).Distinct().Count());
            Assert.AreEqual((jobCount * numberOfRepeats) - jobCount, request.Jobs.Count(x => x.Repeat == true));
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
            _classUnderTest.AddRepeats(request);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Count);
            Assert.AreEqual(jobCount * numberOfRepeats, request.Jobs.Select(x => x.StartDate).Distinct().Count());
            Assert.AreEqual((jobCount * numberOfRepeats)- jobCount, request.Jobs.Count(x => x.Repeat == true));
        }

    }
}