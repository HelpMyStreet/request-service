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
        public bool AddMultiVolunteers(NewJobsRequest request)
        {
            List<Job> duplicatedJobs = new List<Job>();

            foreach (Job j in request.Jobs)
            {
                //check if number of volunteer question has been asked
                var numberOfSlotsQuestion = j.Questions?.Where(x => x.Id == (int)Questions.NumberOfSlots).FirstOrDefault();

                if (numberOfSlotsQuestion != null)
                {
                    int numberOfSlots = Convert.ToInt32(numberOfSlotsQuestion.Answer);
                    for (int i = 1; i < numberOfSlots; i++)
                    {
                        duplicatedJobs.Add(new Job()
                        {
                            HealthCritical = j.HealthCritical,
                            DueDateType = j.DueDateType,
                            SupportActivity = j.SupportActivity,
                            StartDate = j.StartDate,
                            EndDate = j.EndDate,
                            Questions = j.Questions,                            
                            NumberOfRepeats = j.NumberOfRepeats,
                            RepeatFrequency = j.RepeatFrequency,
                            NotBeforeDate = j.NotBeforeDate
                        });
                    }
                }
            }

            if (duplicatedJobs.Count > 0)
            {
                request.Jobs.AddRange(duplicatedJobs);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddRepeats(NewJobsRequest request, DateTime startDateTime)
        {
            List<Job> repeatJobs = new List<Job>();
            foreach (Job j in request.Jobs)
            {
                for (int loopCount = 1; loopCount < j.NumberOfRepeats; loopCount++)
                {
                    var daysToAdd = j.RepeatFrequency.FrequencyDays() * loopCount;
                    DueDateType dueDateType = j.DueDateType;                    
                    DateTime? startDate = j.StartDate.HasValue ? j.StartDate.Value.AddDays(daysToAdd) : (DateTime?)null;
                    DateTime? endDate = j.EndDate.HasValue ? j.EndDate.Value.AddDays(daysToAdd) : (DateTime?)null;
                    DateTime? notBeforeDate = j.NotBeforeDate.HasValue ? j.NotBeforeDate.Value.AddDays(daysToAdd) : (DateTime?)null;

                    if (j.DueDateType == DueDateType.ASAP)
                    {                        
                        switch (j.RepeatFrequency)
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
                                startDate = startDateTime.AddDays(daysToAdd+3);
                                break;
                            default:
                                throw new Exception($"Invalid Frequency for DueDate.ASAP {j.RepeatFrequency}");
                        }
                        endDate = startDate;
                    }
                    
                    repeatJobs.Add(new Job()
                    {
                        HealthCritical = j.HealthCritical,
                        DueDateType = dueDateType,
                        SupportActivity = j.SupportActivity,
                        StartDate = startDate,
                        EndDate = endDate,
                        Questions = j.Questions,
                        NumberOfRepeats = j.NumberOfRepeats,
                        RepeatFrequency = j.RepeatFrequency,
                        NotBeforeDate = notBeforeDate
                    });
                }
            }

            if (repeatJobs.Count > 0)
            {
                request.Jobs.AddRange(repeatJobs);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddShiftRepeats(List<HelpRequestDetail> helpRequestDetails, int repeatCount)
        {
            HelpRequestDetail first = helpRequestDetails.First();
            AddMultiVolunteers(first.NewJobsRequest);

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
                        Guid = Guid.NewGuid()
                    },
                    NewJobsRequest = new NewJobsRequest()
                };

                foreach (Job j in first.NewJobsRequest.Jobs)
                {
                    var daysToAdd = j.RepeatFrequency.FrequencyDays() * loopCount;
                    DueDateType dueDateType = j.DueDateType;
                    DateTime? startDate = j.StartDate.HasValue ? j.StartDate.Value.AddDays(daysToAdd) : (DateTime?)null;
                    DateTime? endDate = j.EndDate.HasValue ? j.EndDate.Value.AddDays(daysToAdd) : (DateTime?)null;
                    DateTime? notBeforeDate = j.NotBeforeDate.HasValue ? j.NotBeforeDate.Value.AddDays(daysToAdd) : (DateTime?)null;

                    if (j.DueDateType == DueDateType.ASAP)
                    {
                        switch (j.RepeatFrequency)
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
                                throw new Exception($"Invalid Frequency for DueDate.ASAP {j.RepeatFrequency}");
                        }
                        endDate = startDate;
                    }

                    repeatJobs.Add(new Job()
                    {
                        HealthCritical = j.HealthCritical,
                        DueDateType = dueDateType,
                        SupportActivity = j.SupportActivity,
                        StartDate = startDate,
                        EndDate = endDate,
                        Questions = j.Questions,
                        NumberOfRepeats = j.NumberOfRepeats,
                        RepeatFrequency = j.RepeatFrequency,
                        NotBeforeDate = notBeforeDate
                    });
                }
                helpRequestDetail.NewJobsRequest.Jobs = repeatJobs;
                helpRequestDetails.Add(helpRequestDetail);
            }
        }
    }
}
