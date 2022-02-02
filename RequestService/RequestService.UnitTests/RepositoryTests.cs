﻿using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RequestService.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestService.UnitTests
{
    public struct SampleData
    {
        public int Id { get; set; }
        public bool IsRepeat { get; set; }
        public DateTime DueDate { get; set; }
        public JobStatuses JobStatus { get; set; }
    }
    
    public class RepositoryTests
    {
        private ApplicationDbContext _applicationDbContext;
        private Repository _classUnderTest;
        private List<SampleData> _sampleData;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationDbContext = new ApplicationDbContext(options);
            _applicationDbContext.Database.EnsureCreated();
            PopulateSampleData();
            BulkPopulateJobAndRequestData();
        }

        private void PopulateSampleData()
        {
            _sampleData = new List<SampleData>
            {
                GetSampleData(1, true, DateTime.Now.AddHours(-7), JobStatuses.New),
                GetSampleData(2, true, DateTime.Now.AddHours(-6), JobStatuses.New),
                GetSampleData(3, true, DateTime.Now.AddHours(-5), JobStatuses.New),
                GetSampleData(4, false, DateTime.Now.AddHours(-7), JobStatuses.New),
                GetSampleData(5, false, DateTime.Now.AddHours(-6), JobStatuses.New),
                GetSampleData(6, false, DateTime.Now.AddHours(-5), JobStatuses.New),
                GetSampleData(7, true, DateTime.Now.AddHours(-7), JobStatuses.New),
                GetSampleData(8, true, DateTime.Now.AddHours(-7), JobStatuses.Open)
            };
        }

        private SampleData GetSampleData(int id, bool isRepeat, DateTime dueDate, JobStatuses status)
        {
            return new SampleData()
            {
                Id = id,
                IsRepeat = isRepeat,
                DueDate = dueDate,
                JobStatus = status
            };
        }

        private void BulkPopulateJobAndRequestData()
        {
            foreach (SampleData sd in _sampleData)
            {
                PopulateJobAndRequestData(sd.Id, sd.IsRepeat, sd.DueDate, sd.JobStatus);
            }
            _applicationDbContext.SaveChanges();
        }

        private void PopulateJobAndRequestData(int id, bool isRepeat, DateTime dueDate, JobStatuses status)
        {
            _applicationDbContext.Job.Add(new Repo.EntityFramework.Entities.Job()
            {
                Id = id,
                DueDate = dueDate,
                JobStatusId = (byte)status,
                RequestId = id
            });

            _applicationDbContext.Request.Add(new Repo.EntityFramework.Entities.Request()
            {
                Guid = Guid.NewGuid(),
                Id = id,
                Repeat = isRepeat,
                PostCode = "NG1 6DQ"
            });
        }

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new Repository(_applicationDbContext);
        }

        [Test]
        public async Task CalculateOverdueRepeatJobs()
        {
            var actualOverdueJobs = await _classUnderTest.GetOverdueRepeatJobs();

            DateTime sixHoursAgo = DateTime.UtcNow.AddHours(-6);

            var expectedOverdueJobs = _sampleData
                .Where(x => (x.JobStatus == JobStatuses.New || x.JobStatus == JobStatuses.Open)
                && x.IsRepeat == true
                && x.DueDate < sixHoursAgo
                )
                .Select(x => x.Id);

            Assert.AreEqual(expectedOverdueJobs, actualOverdueJobs);
        }
    }
}
