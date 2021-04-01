using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool AddRepeats(NewJobsRequest request)
        {
            List<Job> repeatJobs = new List<Job>();

            DateTime now = DateTime.UtcNow;

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
                                notBeforeDate = now.AddDays(loopCount);
                                startDate = now.AddDays(loopCount);                                    
                                break;
                            case Frequency.Weekly:
                            case Frequency.Fortnightly:
                            case Frequency.EveryFourWeeks:
                                dueDateType = DueDateType.Before;
                                notBeforeDate = now.AddDays(daysToAdd);
                                startDate = now.AddDays(daysToAdd+3);
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
    }
}
