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
        public void AddMultiVolunteers(NewJobsRequest request)
        {
            List<Job> duplicatedJobs = new List<Job>();

            foreach (Job j in request.Jobs)
            {
                j.NotBeforeDate = DateTime.UtcNow.AddDays(j.DueDays);

                //check if number of volunteer question has been asked
                var numberOfSlotsQuestion = j.Questions?.Where(x => x.Id == (int)Questions.NumberOfSlots).FirstOrDefault();

                if (numberOfSlotsQuestion != null)
                {
                    int numberOfSlots = Convert.ToInt32(numberOfSlotsQuestion.Answer);
                    if (numberOfSlots > 1)
                    {
                        for (int i = 0; i < (numberOfSlots - 1); i++)
                        {
                            duplicatedJobs.Add(new Job()
                            {
                                HealthCritical = j.HealthCritical,
                                DueDateType = j.DueDateType,
                                SupportActivity = j.SupportActivity,
                                StartDate = j.StartDate,
                                EndDate = j.EndDate,
                                Questions = j.Questions,
                                DueDays = j.DueDays,
                                NumberOfRepeats = j.NumberOfRepeats,
                                RepeatFrequency = j.RepeatFrequency,
                                NotBeforeDate = j.NotBeforeDate
                            });
                        }
                    }
                }
            }

            if (duplicatedJobs.Count > 0)
            {
                request.Jobs.AddRange(duplicatedJobs);
            }
        }

        public void AddRepeats(NewJobsRequest request)
        {
            List<Job> repeatJobs = new List<Job>();

            foreach (Job j in request.Jobs)
            {
                j.NotBeforeDate = DateTime.UtcNow.AddDays(j.DueDays);

                for (int loopCount = 1; loopCount < j.NumberOfRepeats; loopCount++)
                {
                    int dueDays = j.DueDays + (j.RepeatFrequency.FrequencyDays() * loopCount);
                    DateTime? notBeforeDate = DateTime.UtcNow.AddDays(dueDays);
                    if (j.DueDateType == DueDateType.Before)
                    {
                        notBeforeDate = DateTime.UtcNow.AddDays(dueDays - j.DueDays);
                    }

                    repeatJobs.Add(new Job()
                    {
                        HealthCritical = j.HealthCritical,
                        DueDateType = j.DueDateType,
                        SupportActivity = j.SupportActivity,
                        StartDate = j.StartDate,
                        EndDate = j.EndDate,
                        Questions = j.Questions,
                        DueDays = dueDays,
                        NumberOfRepeats = j.NumberOfRepeats,
                        RepeatFrequency = j.RepeatFrequency,
                        NotBeforeDate = notBeforeDate
                    });
                }
            }

            if (repeatJobs.Count > 0)
            {
                request.Jobs.AddRange(repeatJobs);
            }
        }
    }
}
