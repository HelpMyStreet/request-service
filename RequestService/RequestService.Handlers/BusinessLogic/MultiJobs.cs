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

            foreach (Job j in request.Jobs)
            {
                int diffBetweenDueAndNotBefore = (j.NotBeforeDate.Value.Date - j.StartDate.Value.Date).Days;

                for (int loopCount = 1; loopCount < j.NumberOfRepeats; loopCount++)
                {
                    DateTime? startDate = j.StartDate.HasValue ? j.StartDate.Value.AddDays((j.RepeatFrequency.FrequencyDays() * loopCount)) : (DateTime?)null;
                    DateTime? endDate = j.EndDate.HasValue ? j.EndDate.Value.AddDays((j.RepeatFrequency.FrequencyDays() * loopCount)) : (DateTime?)null;                    

                    repeatJobs.Add(new Job()
                    {
                        HealthCritical = j.HealthCritical,
                        DueDateType = j.DueDateType,
                        SupportActivity = j.SupportActivity,
                        StartDate = startDate,
                        EndDate = endDate,
                        Questions = j.Questions,
                        NumberOfRepeats = j.NumberOfRepeats,
                        RepeatFrequency = j.RepeatFrequency,
                        NotBeforeDate = startDate.Value.AddDays(diffBetweenDueAndNotBefore)
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
