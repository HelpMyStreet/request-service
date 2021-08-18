using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace RequestService.Handlers.BusinessLogic
{
    public class MultiJobs : IMultiJobs
    {
        public void AddMultiVolunteers(HelpRequestDetail helpRequestDetail)
        {
            List<Job> duplicatedJobs = new List<Job>();
            Job job = helpRequestDetail.NewJobsRequest.Jobs.First();

            for (int i = 1; i < helpRequestDetail.VolunteerCount; i++)
            {
                duplicatedJobs.Add(new Job()
                {
                    HealthCritical = job.HealthCritical,
                    DueDateType = job.DueDateType,
                    SupportActivity = job.SupportActivity,
                    StartDate = job.StartDate,
                    EndDate = job.EndDate,
                    Questions = job.Questions,
                    NumberOfRepeats = job.NumberOfRepeats,
                    RepeatFrequency = job.RepeatFrequency,
                    NotBeforeDate = job.NotBeforeDate
                });
            }

            if (duplicatedJobs.Count > 0)
            {
                helpRequestDetail.NewJobsRequest.Jobs.AddRange(duplicatedJobs);
            }
        }

        public void AddRepeats(NewJobsRequest request, DateTime startDateTime)
        {
            List<Job> repeatJobs = new List<Job>();
            foreach (Job j in request.Jobs)
            {
                for (int loopCount = 1; loopCount < j.NumberOfRepeats; loopCount++)
                {
                    repeatJobs.Add(AddJob(loopCount, j, startDateTime));
                }
            }

            if (repeatJobs.Count > 0)
            {
                request.Jobs.AddRange(repeatJobs);
            }
        }

        private Job AddJob(int loopCount, Job job, DateTime startDateTime)
        {
            var daysToAdd = job.RepeatFrequency.FrequencyDays() * loopCount;
            DueDateType dueDateType = job.DueDateType;
            DateTime? startDate = job.StartDate.HasValue ? job.StartDate.Value.AddDays(daysToAdd) : (DateTime?)null;
            DateTime? endDate = job.EndDate.HasValue ? job.EndDate.Value.AddDays(daysToAdd) : (DateTime?)null;
            DateTime? notBeforeDate = job.NotBeforeDate.HasValue ? job.NotBeforeDate.Value.AddDays(daysToAdd) : (DateTime?)null;

            if (job.DueDateType == DueDateType.ASAP)
            {
                switch (job.RepeatFrequency)
                {
                    case Frequency.Daily:
                        dueDateType = DueDateType.On;
                        notBeforeDate = startDateTime.AddDays(loopCount);
                        startDate = startDateTime.AddDays(loopCount);
                        break;
                    case Frequency.Weekly:
                    case Frequency.Fortnightly:
                    case Frequency.EveryFourWeeks:
                        dueDateType = DueDateType.Before;
                        notBeforeDate = startDateTime.AddDays(daysToAdd);
                        startDate = startDateTime.AddDays(daysToAdd + 3);
                        break;
                    default:
                        throw new Exception($"Invalid Frequency for DueDate.ASAP {job.RepeatFrequency}");
                }
                endDate = startDate;
            }

            return new Job()
            {
                HealthCritical = job.HealthCritical,
                DueDateType = dueDateType,
                SupportActivity = job.SupportActivity,
                StartDate = startDate,
                EndDate = endDate,
                Questions = job.Questions,
                NumberOfRepeats = job.NumberOfRepeats,
                RepeatFrequency = job.RepeatFrequency,
                NotBeforeDate = notBeforeDate
            };
        }

        public void AddShiftRepeats(List<HelpRequestDetail> helpRequestDetails, int repeatCount)
        {
            HelpRequestDetail first = helpRequestDetails.First();

            DateTime startDateTime = DateTime.UtcNow;

            for (int loopCount = 1; loopCount < repeatCount; loopCount++)
            {
                List<Job> repeatJobs = new List<Job>();
                HelpRequestDetail helpRequestDetail = new HelpRequestDetail()
                {
                    HelpRequest = new HelpRequest()
                    {
                        ConsentForContact = first.HelpRequest.ConsentForContact,
                        AcceptedTerms = first.HelpRequest.AcceptedTerms,
                        CreatedByUserId = first.HelpRequest.CreatedByUserId,
                        OrganisationName = first.HelpRequest.OrganisationName,
                        OtherDetails = first.HelpRequest.OtherDetails,
                        ReadPrivacyNotice = first.HelpRequest.ReadPrivacyNotice,
                        Recipient = first.HelpRequest.Recipient,
                        ReferringGroupId = first.HelpRequest.ReferringGroupId,
                        Requestor = first.HelpRequest.Requestor,
                        RequestorType = first.HelpRequest.RequestorType,
                        Source = first.HelpRequest.Source,
                        SpecialCommunicationNeeds = first.HelpRequest.SpecialCommunicationNeeds,
                        VolunteerUserId = first.HelpRequest.VolunteerUserId,
                        Guid = Guid.NewGuid(),
                        ParentGuid = first.HelpRequest.Guid
                    },
                    NewJobsRequest = new NewJobsRequest()
                };

                foreach (Job j in first.NewJobsRequest.Jobs)
                {
                    repeatJobs.Add(AddJob(loopCount, j, startDateTime));
                }
                helpRequestDetail.NewJobsRequest.Jobs = repeatJobs;
                helpRequestDetails.Add(helpRequestDetail);
            }
        }
    }
}
