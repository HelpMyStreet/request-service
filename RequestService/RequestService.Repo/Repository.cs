﻿using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using RequestService.Core.Dto;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SupportActivities = RequestService.Repo.EntityFramework.Entities.SupportActivities;
using Microsoft.Data.SqlClient;
using HelpMyStreet.Utils.Utils;
using RequestService.Core.Domains;
using RequestService.Core.Exceptions;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.EqualityComparers;
using HelpMyStreet.Contracts.ReportService;

namespace RequestService.Repo
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;
        private IEqualityComparer<JobBasic> _jobBasicDedupeWithDate_EqualityComparer;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _jobBasicDedupeWithDate_EqualityComparer = new JobBasicDedupeWithDate_EqualityComparer();
        }

        public async Task<string> GetRequestPostCodeAsync(int requestId, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                return request.PostCode;
            }
            return null;
        }

        public async Task<int> CreateRequestAsync(string postCode, CancellationToken cancellationToken)
        {
            Request request = new Request
            {
                PostCode = postCode,
                DateRequested = DateTime.Now,
                IsFulfillable = false,
                CommunicationSent = false,
            };

            _context.Request.Add(request);
            await _context.SaveChangesAsync(cancellationToken);
            return request.Id;
        }


        public async Task UpdateFulfillmentAsync(int requestId, Fulfillable fulfillable, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                request.FulfillableStatus = (byte)fulfillable;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateCommunicationSentAsync(int requestId, bool communicationSent, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                request.CommunicationSent = communicationSent;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdatePersonalDetailsAsync(PersonalDetailsDto dto, CancellationToken cancellationToken)
        {
            var personalDetails = new EntityFramework.Entities.PersonalDetails
            {
                RequestId = dto.RequestID,
                FurtherDetails = dto.FurtherDetails,
                OnBehalfOfAnother = dto.OnBehalfOfAnother,
                RequestorEmailAddress = dto.RequestorEmailAddress,
                RequestorFirstName = dto.RequestorFirstName,
                RequestorLastName = dto.RequestorLastName,
                RequestorPhoneNumber = dto.RequestorPhoneNumber,
            };
            _context.PersonalDetails.Add(personalDetails);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddSupportActivityAsync(SupportActivityDTO dto, CancellationToken cancellationToken)
        {
            List<SupportActivities> activties = new List<SupportActivities>();
            foreach (var activtity in dto.SupportActivities)
            {
                activties.Add(new SupportActivities
                {
                    RequestId = dto.RequestID,
                    ActivityId = (int)activtity
                });
            }

            _context.SupportActivities.AddRange(activties);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<ReportItem> GetDailyReport()
        {
            List<ReportItem> response = new List<ReportItem>();
            List<DailyReport> result = _context.DailyReport
                                .FromSqlRaw("TwoHourlyReport")
                                .ToList();
            if (result != null)
            {
                foreach (DailyReport dailyReport in result)
                {
                    response.Add(new ReportItem()
                    {
                        Section = dailyReport.Section,
                        Last2Hours = dailyReport.Last2Hours,
                        Today = dailyReport.Today,
                        SinceLaunch = dailyReport.SinceLaunch
                    });
                }
            }

            return response;
        }

        private Person GetPersonFromPersonalDetails(RequestPersonalDetails requestPersonalDetails)
        {
            if (requestPersonalDetails != null)
            {
                return new Person()
                {
                    FirstName = requestPersonalDetails.FirstName,
                    LastName = requestPersonalDetails.LastName,
                    EmailAddress = requestPersonalDetails.EmailAddress,
                    AddressLine1 = requestPersonalDetails.Address.AddressLine1,
                    AddressLine2 = requestPersonalDetails.Address.AddressLine2,
                    AddressLine3 = requestPersonalDetails.Address.AddressLine3,
                    Locality = requestPersonalDetails.Address.Locality,
                    Postcode = PostcodeFormatter.FormatPostcode(requestPersonalDetails.Address.Postcode),
                    MobilePhone = requestPersonalDetails.MobileNumber ?? string.Empty,
                    OtherPhone = requestPersonalDetails.OtherNumber ?? string.Empty,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<int> AddHelpRequestDetailsAsync(HelpRequestDetail helpRequestDetail, Fulfillable fulfillable, bool requestorDefinedByGroup, RequestHelpFormVariant requestHelpFormVariant, bool? suppressRecipientPersonalDetails, IEnumerable<int> availableToGroups, bool setStatusToOpen)
        {
            Person requester = GetPersonFromPersonalDetails(helpRequestDetail.HelpRequest.Requestor);
            Person recipient;

            if (helpRequestDetail.HelpRequest.RequestorType == RequestorType.Myself)
            {
                recipient = requester;
            }
            else
            {
                recipient = GetPersonFromPersonalDetails(helpRequestDetail.HelpRequest.Recipient);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (requester != null)
                    {
                        _context.Person.Add(requester);
                    }

                    if (recipient != null)
                    {
                        _context.Person.Add(recipient);
                    }

                    RequestType requestType = helpRequestDetail.NewJobsRequest.Jobs.First().SupportActivity.RequestType();

                    Request newRequest = new Request()
                    {
                        Guid = helpRequestDetail.HelpRequest.Guid,
                        ReadPrivacyNotice = helpRequestDetail.HelpRequest.ReadPrivacyNotice,
                        SpecialCommunicationNeeds = helpRequestDetail.HelpRequest.SpecialCommunicationNeeds,
                        AcceptedTerms = helpRequestDetail.HelpRequest.AcceptedTerms,
                        OtherDetails = helpRequestDetail.HelpRequest.OtherDetails,
                        OrganisationName = helpRequestDetail.HelpRequest.OrganisationName,
                        PostCode = PostcodeFormatter.FormatPostcode(helpRequestDetail.HelpRequest.Recipient?.Address?.Postcode ?? string.Empty),
                        PersonIdRecipientNavigation = recipient,
                        PersonIdRequesterNavigation = requester,
                        RequestorType = (byte)helpRequestDetail.HelpRequest.RequestorType,
                        FulfillableStatus = (byte)fulfillable,
                        CreatedByUserId = helpRequestDetail.HelpRequest.CreatedByUserId,
                        ReferringGroupId = helpRequestDetail.HelpRequest.ReferringGroupId,
                        Source = helpRequestDetail.HelpRequest.Source,
                        RequestorDefinedByGroup = requestorDefinedByGroup,
                        RequestType = (byte)requestType,
                        Archive = false,
                        SuppressRecipientPersonalDetail = suppressRecipientPersonalDetails,                        
                        MultiVolunteer = helpRequestDetail.VolunteerCount>1,
                        Repeat = helpRequestDetail.Repeat,
                        ParentGuid = helpRequestDetail.HelpRequest.ParentGuid,
                        Language = helpRequestDetail.HelpRequest.Language
                    };

                    var firstJob = helpRequestDetail.NewJobsRequest.Jobs.First();

                    if (!helpRequestDetail.HelpRequest.ParentGuid.HasValue)
                    {
                        _context.RequestSubmission.Add(new RequestSubmission()
                        {
                            Request = newRequest,
                            NumberOfRepeats = firstJob.NumberOfRepeats > 1 ? firstJob.NumberOfRepeats : (int?) null,
                            FreqencyId = (byte)firstJob.RepeatFrequency
                        });
                    }

                    if (requestType == RequestType.Shift)
                    {
                        var locationQuestion = firstJob.Questions.Where(x => x.Id == (int)Questions.Location).First();

                        if (locationQuestion == null)
                        {
                            throw new Exception("Missing location question");
                        }

                        int locationId = Convert.ToInt32(locationQuestion.AddtitonalData.Where(x => x.Value == locationQuestion.Answer).First().Key);

                        _context.Shift.Add(new EntityFramework.Entities.Shift()
                        {
                            Request = newRequest,
                            StartDate = firstJob.StartDate.Value,
                            ShiftLength = (int)(firstJob.EndDate.Value - firstJob.StartDate.Value).TotalMinutes,
                            LocationId = locationId
                        });
                    }

                    foreach (HelpMyStreet.Utils.Models.Job job in helpRequestDetail.NewJobsRequest.Jobs)
                    {
                        String reference = null;
                        if(requestHelpFormVariant == RequestHelpFormVariant.VitalsForVeterans)
                        {
                            var question = job.Questions.Where(x => x.Id == (int)Questions.AgeUKReference).FirstOrDefault();
                            if(question!=null)
                            {
                                reference = $"Age UK Ref {  question.Answer}";
                            }
                        }
                        else if (requestHelpFormVariant == RequestHelpFormVariant.UkraineRefugees_RequestSubmitter)
                        {
                            var questionAdults = job.Questions.Where(x => x.Id == (int)Questions.GroupSizeAdults).FirstOrDefault();
                            var questionChildren = job.Questions.Where(x => x.Id == (int)Questions.GroupSizeChildren).FirstOrDefault();
                            var questionPets = job.Questions.Where(x => x.Id == (int)Questions.GroupSizePets).FirstOrDefault();

                            if(questionAdults!=null)
                            {
                                var adults = Convert.ToInt32(questionAdults.Answer);
                                if(adults>0)
                                {
                                    reference = adults == 1 ? "1 Adult" : $"{adults} Adults";
                                }
                            }

                            if (questionChildren != null)
                            {
                                var children = Convert.ToInt32(questionChildren.Answer);
                                if (children > 0)
                                {
                                    if(!string.IsNullOrEmpty(reference))
                                    {
                                        reference += " + ";
                                    }
                                    reference = children == 1 ? $"{reference}1 Child" : $"{reference}{children} Children";
                                }
                            }

                            if (questionPets != null)
                            {
                                var pets = Convert.ToInt32(questionPets.Answer);
                                if (pets > 0)
                                {
                                    if (!string.IsNullOrEmpty(reference))
                                    {
                                        reference += " + ";
                                    }
                                    reference = pets == 1 ? $"{reference}1 Pet" : $"{reference}{pets} Pets";
                                }
                            }

                        }

                        EntityFramework.Entities.Job EFcoreJob = new EntityFramework.Entities.Job()
                        {
                            NewRequest = newRequest,
                            IsHealthCritical = job.HealthCritical,
                            SupportActivityId = (byte)job.SupportActivity,
                            DueDate = job.DueDateType == DueDateType.OpenUntil ? job.EndDate.Value : job.StartDate.Value,
                            DueDateTypeId = (byte)job.DueDateType,
                            JobStatusId = (byte)JobStatuses.New,
                            NotBeforeDate = job.NotBeforeDate,
                            SpecificSupportActivity = job.Questions.Where(x => x.Id == (int)Questions.SelectActivity).FirstOrDefault()?.Answer,
                            Reference = reference,
                        };
                        _context.Job.Add(EFcoreJob);
                        await _context.SaveChangesAsync();
                        job.JobID = EFcoreJob.Id;

                        foreach (var question in job.Questions)
                        {
                            _context.JobQuestions.Add(new JobQuestions
                            {
                                Job = EFcoreJob,
                                QuestionId = question.Id,
                                Answer = question.Answer
                            });
                        }

                        AddJobStatus(
                            EFcoreJob,
                            helpRequestDetail.HelpRequest.CreatedByUserId,
                            null,
                            JobStatuses.New,
                            JobStatusChangeReasonCodes.UserChange
                            );

                        if(setStatusToOpen)
                        {
                            AddJobStatus(
                            EFcoreJob,
                            helpRequestDetail.HelpRequest.CreatedByUserId,
                            null,
                            JobStatuses.Open,
                            JobStatusChangeReasonCodes.AutoProgressNewToOpen
                            );
                        }


                        foreach (int i in availableToGroups)
                        {
                            _context.JobAvailableToGroup.Add(new JobAvailableToGroup()
                            {
                                GroupId = i,
                                Job = EFcoreJob
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return newRequest.Id;
                }
                catch (Exception exc)
                {
                    if (exc.InnerException.Message.StartsWith("Cannot insert duplicate key row in object 'Request.Request' with unique index 'UC_Guid'"))
                    {
                        transaction.Rollback();
                        throw new DuplicateException();
                    }

                }
            }
            throw new Exception("Unable to save request");
        }

        public async Task<int> NewShiftsRequestAsync(PostNewShiftsRequest postNewShiftsRequest, Fulfillable fulfillable, RequestPersonalDetails requestorPersonalDetails)
        {
            Person requester = GetPersonFromPersonalDetails(requestorPersonalDetails);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Request request = new Request()
                    {
                        OtherDetails = postNewShiftsRequest.OtherDetails,
                        PostCode = "",
                        RequestorType = (byte)RequestorType.Organisation,
                        FulfillableStatus = (byte)fulfillable,
                        CreatedByUserId = postNewShiftsRequest.CreatedByUserId,
                        ReferringGroupId = postNewShiftsRequest.ReferringGroupId,
                        Source = postNewShiftsRequest.Source,
                        RequestorDefinedByGroup = true,
                        PersonIdRequesterNavigation = requester,
                        RequestType = (byte)RequestType.Shift
                    };

                    _context.Shift.Add(new EntityFramework.Entities.Shift()
                    {
                        Request = request,
                        StartDate = postNewShiftsRequest.StartDate,
                        ShiftLength = postNewShiftsRequest.ShiftLength,
                        LocationId = (int)postNewShiftsRequest.Location.Location
                    });

                    foreach (var item in postNewShiftsRequest.SupportActivitiesCount)
                    {
                        for (int i = 0; i < item.Count; i++)
                        {
                            EntityFramework.Entities.Job EFcoreJob = new EntityFramework.Entities.Job()
                            {
                                NewRequest = request,
                                IsHealthCritical = false,
                                SupportActivityId = (byte)item.SupportActivity,
                                DueDate = new DateTime(1900, 1, 1),
                                DueDateTypeId = (byte)DueDateType.SpecificStartAndEndTimes,
                                JobStatusId = (byte)JobStatuses.Open,
                            };
                            _context.Job.Add(EFcoreJob);
                            AddJobStatus(
                                EFcoreJob, 
                                postNewShiftsRequest.CreatedByUserId, 
                                null, 
                                JobStatuses.New, 
                                JobStatusChangeReasonCodes.UserChange);
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return request.Id;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            throw new Exception("Unable to save shift request");
        }

        private void AddJobStatus(EntityFramework.Entities.Job job, int? createdByUserID, int? volunteerUserID, JobStatuses jobStatus, JobStatusChangeReasonCodes jobStatusChangeReasonCode)
        {
            job.JobStatusId = (byte)jobStatus;
            _context.RequestJobStatus.Add(new RequestJobStatus()
            {
                Job = job,
                CreatedByUserId = createdByUserID,
                VolunteerUserId = volunteerUserID,
                JobStatusId = (byte) jobStatus,
                JobStatusChangeReasonCodeId = (byte)jobStatusChangeReasonCode
            });
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusOpenAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte openJobStatus = (byte)JobStatuses.Open;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != openJobStatus)
                {
                    job.JobStatusId = openJobStatus;
                    job.VolunteerUserId = null;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Open, JobStatusChangeReasonCodes.UserChange);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusCancelledAsync(int jobID, int createdByUserID, JobStatusChangeReasonCodes jobStatusChangeReasonCode, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte cancelledJobStatus = (byte)JobStatuses.Cancelled;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();

            if (job != null)
            {
                if (job.JobStatusId != cancelledJobStatus)
                {
                    job.JobStatusId = cancelledJobStatus;
                    job.VolunteerUserId = null;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Cancelled, jobStatusChangeReasonCode);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusInProgressAsync(int jobID, int createdByUserID, int volunteerUserID, JobStatusChangeReasonCodes jobStatusChangeReasonCode, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte inProgressJobStatus = (byte)JobStatuses.InProgress;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();

            if (job != null)
            {
                if (job.JobStatusId != inProgressJobStatus)
                {
                    job.JobStatusId = inProgressJobStatus;
                    job.VolunteerUserId = volunteerUserID;
                    AddJobStatus(job, createdByUserID, volunteerUserID, JobStatuses.InProgress, jobStatusChangeReasonCode);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                }
                else
                {
                    if (job.VolunteerUserId == volunteerUserID)
                    {
                        response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                    }
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusAppliedForAsync(int jobID, int createdByUserID, int volunteerUserID, JobStatusChangeReasonCodes jobStatusChangeReasonCode, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte appliedForJobStatus = (byte)JobStatuses.AppliedFor;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();

            if (job != null)
            {
                if (job.JobStatusId != appliedForJobStatus)
                {
                    job.JobStatusId = appliedForJobStatus;
                    job.VolunteerUserId = volunteerUserID;
                    AddJobStatus(job, createdByUserID, volunteerUserID, JobStatuses.AppliedFor, jobStatusChangeReasonCode);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                }
                else
                {
                    if (job.VolunteerUserId == volunteerUserID)
                    {
                        response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                    }
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusDoneAsync(int jobID, int createdByUserID, JobStatusChangeReasonCodes jobStatusChangeReasonCode, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte doneJobStatus = (byte)JobStatuses.Done;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != doneJobStatus)
                {
                    job.JobStatusId = doneJobStatus;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Done, jobStatusChangeReasonCode);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                    else
                    {
                        response = UpdateJobStatusOutcome.BadRequest;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusNewAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte newJobStatus = (byte)JobStatuses.New;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != newJobStatus)
                {
                    job.JobStatusId = newJobStatus;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.New, JobStatusChangeReasonCodes.UserChange);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                    else
                    {
                        response = UpdateJobStatusOutcome.BadRequest;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusAcceptedAsync(int jobID, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte acceptedJobStatus = (byte)JobStatuses.Accepted;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();

            if (job != null)
            {
                if (((HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId).RequestType() == RequestType.Shift)
                {
                    if (job.JobStatusId != acceptedJobStatus)
                    {
                        job.JobStatusId = acceptedJobStatus;
                        job.VolunteerUserId = volunteerUserID; ;
                        AddJobStatus(job, createdByUserID, volunteerUserID, JobStatuses.Accepted, JobStatusChangeReasonCodes.UserChange);
                        int result = await _context.SaveChangesAsync(cancellationToken);
                        if (result == 2)
                        {
                            response = UpdateJobStatusOutcome.Success;
                        }
                    }
                    else
                    {
                        response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                    }
                }
            }
            return response;
        }

        public async Task<UpdateJobOutcome> UpdateJobQuestion(int jobID, int questionId, string answer, CancellationToken cancellationToken)
        {
            UpdateJobOutcome response = UpdateJobOutcome.BadRequest;
            var job = _context.JobQuestions.Where(w => w.JobId == jobID && w.QuestionId == questionId).FirstOrDefault();

            if (job != null)
            {
                job.Answer = answer;
                int result = await _context.SaveChangesAsync(cancellationToken);

                if (result == 1)
                {
                    response = UpdateJobOutcome.Success;
                }
            }
            return response;
        }

        public List<JobSummary> GetJobsAllocatedToUser(int volunteerUserID)
        {
            byte jobStatusID_InProgress = (byte)JobStatuses.InProgress;
            byte requestType_task = (byte)RequestType.Task;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.NewRequest)
                                    .ThenInclude(i => i.Shift)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.VolunteerUserId == volunteerUserID
                                                && w.JobStatusId == jobStatusID_InProgress
                                                && w.NewRequest.RequestType == requestType_task
                                            ).ToList();

            return GetJobSummaries(jobSummaries);

        }

        public List<JobSummary> GetUserJobs(int volunteerUserID)
        {
            byte jobStatusID_Cancelled = (byte)JobStatuses.Cancelled;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.NewRequest)
                                    .ThenInclude(i => i.Shift)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.VolunteerUserId == volunteerUserID
                                                && w.JobStatusId != jobStatusID_Cancelled
                                            ).ToList();

            return GetJobSummaries(jobSummaries);

        }

        public List<JobSummary> GetJobsInProgressSummaries()
        {
            byte jobStatusID_InProgress = (byte)JobStatuses.InProgress;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.JobAvailableToGroup)
                                    .Include(i => i.NewRequest)
                                    .ThenInclude(i => i.Shift)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.JobStatusId == jobStatusID_InProgress
                                            ).ToList();
            return GetJobSummaries(jobSummaries);
        }

        public List<JobSummary> GetJobsByStatusesSummaries(List<JobStatuses> jobStatuses)
        {
            List<byte> statuses = new List<byte>();

            foreach (JobStatuses js in jobStatuses)
            {
                statuses.Add((byte)js);
            }

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.JobAvailableToGroup)
                                    .Include(i => i.NewRequest)
                                    .ThenInclude(i => i.Shift)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => statuses.Contains(w.JobStatusId.Value)
                                            ).ToList();
            return GetJobSummaries(jobSummaries);
        }

        private List<byte> ConvertSupportActivities(List<HelpMyStreet.Utils.Enums.SupportActivities> supportActivities)
        {
            List<byte> activities = new List<byte>();

            foreach (HelpMyStreet.Utils.Enums.SupportActivities sa in supportActivities)
            {
                activities.Add((byte)sa);
            }
            return activities;
        }

        private List<byte> ConvertJobStatuses(List<JobStatuses> jobStatuses)
        {
            List<byte> statuses = new List<byte>();

            foreach (JobStatuses sa in jobStatuses)
            {
                statuses.Add((byte)sa);
            }
            return statuses;
        }

        private SqlParameter GetParameter(string parameterName, int? Id)
        {
            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = System.Data.SqlDbType.Int,
                SqlValue = Id ?? 0
            };
        }

        private SqlParameter GetSupportActivitiesAsSqlParameter(List<HelpMyStreet.Utils.Enums.SupportActivities> supportActivities)
        {
            string sqlValue = string.Empty;
            if (supportActivities?.Count > 0)
            {
                sqlValue = string.Join(",", supportActivities.Cast<int>().ToArray());
            }
            return new SqlParameter()
            {
                ParameterName = "@SupportActivities",
                SqlDbType = System.Data.SqlDbType.VarChar,
                SqlValue = sqlValue
            };
        }

        private SqlParameter GetReferringGroupsAsSqlParameter(List<int> referringGroups)
        {
            string sqlValue = string.Empty;
            if (referringGroups?.Count > 0)
            {
                sqlValue = string.Join(",", referringGroups.ToArray());
            }
            return new SqlParameter()
            {
                ParameterName = "@ReferringGroups",
                SqlDbType = System.Data.SqlDbType.VarChar,
                SqlValue = sqlValue
            };
        }

        private SqlParameter GetJobStatusesAsSqlParameter(List<JobStatuses> jobStatuses)
        {
            string sqlValue = string.Empty;
            if (jobStatuses?.Count > 0)
            {
                sqlValue = string.Join(",", jobStatuses.Cast<int>().ToArray());
            }
            return new SqlParameter()
            {
                ParameterName = "@JobStatuses",
                SqlDbType = System.Data.SqlDbType.VarChar,
                SqlValue = sqlValue
            };
        }

        private SqlParameter GetGroupsAsSqlParameter(List<int> groups)
        {
            string sqlValue = string.Empty;
            if (groups?.Count > 0)
            {
                sqlValue = string.Join(",", groups);
            }

            return new SqlParameter()
            {
                ParameterName = "@Groups",
                SqlDbType = System.Data.SqlDbType.VarChar,
                SqlValue = sqlValue
            };
        }

        private SqlParameter GetRequestTypeAsSqlParameter(List<RequestType> requestTypes)
        {
            string sqlValue = string.Empty;
            if (requestTypes?.Count > 0)
            {
                sqlValue = string.Join(",", requestTypes.Cast<int>().ToArray());
            }

            return new SqlParameter()
            {
                ParameterName = "@RequestTypes",
                SqlDbType = System.Data.SqlDbType.VarChar,
                SqlValue = sqlValue
            };
        }


        public List<JobHeader> GetJobHeaders(GetJobsByFilterRequest request, List<int> referringGroups)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = GetParameter("@UserID", request.UserID);
            parameters[1] = GetSupportActivitiesAsSqlParameter(request.SupportActivities?.SupportActivities);
            parameters[2] = GetReferringGroupsAsSqlParameter(referringGroups);
            parameters[3] = GetJobStatusesAsSqlParameter(request.JobStatuses?.JobStatuses);
            parameters[4] = GetGroupsAsSqlParameter(request.Groups?.Groups);

            IQueryable<QueryJobHeader> jobHeaders = _context.JobHeader
                                .FromSqlRaw("EXECUTE [Request].[GetJobsByFilter] @UserID=@UserID,@SupportActivities=@SupportActivities,@ReferringGroups=@ReferringGroups,@JobStatuses=@JobStatuses,@Groups=@Groups", parameters);

            List<JobHeader> response = new List<JobHeader>();
            foreach (QueryJobHeader j in jobHeaders)
            {
                response.Add(new JobHeader()
                {
                    VolunteerUserID = j.VolunteerUserID,
                    JobID = j.JobID,
                    Archive = j.Archive.Value,
                    DateRequested = j.DateRequested,
                    DateStatusLastChanged = j.DateStatusLastChanged,
                    DistanceInMiles = j.DistanceInMiles,
                    DueDate = j.DueDate,
                    IsHealthCritical = j.IsHealthCritical,
                    JobStatus = (JobStatuses)j.JobStatusID,
                    PostCode = j.PostCode,
                    ReferringGroupID = j.ReferringGroupID,
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)j.SupportActivityID,
                    Reference = j.Reference,
                    DueDateType = (DueDateType)j.DueDateTypeId,
                    RequestID = j.RequestID,
                    RequestType = (RequestType)j.RequestType,
                    RequestorDefinedByGroup = j.RequestorDefinedByGroup,
                    NotBeforeDate = j.NotBeforeDate
                });
            }
            return response;
        }

        public List<JobDTO> GetAllFilteredJobs(GetAllJobsByFilterRequest request, List<int> referringGroups)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = GetParameter("@UserID", request.AllocatedToUserId);
            parameters[1] = GetSupportActivitiesAsSqlParameter(request.SupportActivities?.SupportActivities);
            parameters[2] = GetReferringGroupsAsSqlParameter(referringGroups);
            parameters[3] = GetJobStatusesAsSqlParameter(request.JobStatuses?.JobStatuses);
            parameters[4] = GetGroupsAsSqlParameter(request.Groups?.Groups);
            parameters[5] = GetRequestTypeAsSqlParameter(request.RequestType?.RequestTypes);

            IQueryable<QueryAllJobs> allJobs = _context.QueryAllJobs
                                .FromSqlRaw("EXECUTE [Request].[GetAllJobsByFilter] @UserID=@UserID,@SupportActivities=@SupportActivities,@ReferringGroups=@ReferringGroups,@JobStatuses=@JobStatuses,@Groups=@Groups,@RequestTypes=@RequestTypes", parameters);

            List<JobDTO> response = new List<JobDTO>();
            foreach (QueryAllJobs j in allJobs)
            {
                response.Add(new JobDTO()
                {
                    VolunteerUserID = j.VolunteerUserID,
                    JobID = j.JobID,
                    Archive = j.Archive,
                    DateRequested = j.DateRequested,
                    DateStatusLastChanged = j.DateStatusLastChanged,
                    DistanceInMiles = j.DistanceInMiles,
                    DueDate = j.DueDate,
                    IsHealthCritical = j.IsHealthCritical,
                    JobStatus = (JobStatuses)j.JobStatusID,
                    PostCode = j.PostCode,
                    ReferringGroupID = j.ReferringGroupID,
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)j.SupportActivityID,
                    Reference = j.Reference,
                    DueDateType = (DueDateType)j.DueDateTypeId,
                    RequestID = j.RequestID,
                    RequestType = (RequestType)j.RequestType,
                    RequestorDefinedByGroup = j.RequestorDefinedByGroup,
                    Location = j.LocationId.HasValue ? (Location?)j.LocationId.Value : null,
                    StartDate = j.StartDate,
                    ShiftLength = j.ShiftLength,
                    NotBeforeDate = j.NotBeforeDate,
                    SpecificSupportActivity = j.SpecificSupportActivity
                });
            }

            return response;
        }

        private JobBasic MapEFJobToBasic(EntityFramework.Entities.Job job)
        {
            DateTime dueDate = job.NewRequest.Shift?.StartDate != null ? job.NewRequest.Shift.StartDate : job.DueDate;
            return new JobBasic()
            {                
                DueDate = dueDate,
                JobID = job.Id,
                VolunteerUserID = job.VolunteerUserId,
                JobStatus = (JobStatuses)job.JobStatusId,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,                               
                ReferringGroupID = job.NewRequest.ReferringGroupId,
                DateStatusLastChanged = job.RequestJobStatus.Max(x => x.DateCreated),
                DateRequested = job.NewRequest.DateRequested,
                Archive = job.NewRequest.Archive.Value,
                DueDateType = (DueDateType)job.DueDateTypeId,
                RequestID = job.NewRequest.Id,
                RequestType = (RequestType)job.NewRequest.RequestType,
                SuppressRecipientPersonalDetail = job.NewRequest.SuppressRecipientPersonalDetail,
                NotBeforeDate = job.NotBeforeDate,                
            };
        }

        private List<JobBasic> GetJobBasics(List<EntityFramework.Entities.Job> jobs)
        {
            List<JobBasic> response = new List<JobBasic>();
            foreach (EntityFramework.Entities.Job j in jobs)
            {
                response.Add(MapEFJobToBasic(j));
            }
            return response;
        }

        private JobSummary MapEFJobToSummary(EntityFramework.Entities.Job job)
        {
            DateTime dueDate = job.NewRequest.Shift?.StartDate != null ? job.NewRequest.Shift.StartDate : job.DueDate;
            return new JobSummary()
            {
                IsHealthCritical = job.IsHealthCritical,
                DueDate = dueDate,
                Details = job.Details,
                JobID = job.Id,
                VolunteerUserID = job.VolunteerUserId,
                JobStatus = (JobStatuses)job.JobStatusId,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,
                PostCode = job.NewRequest.PostCode,
                Questions = MapToQuestions(job.JobQuestions),
                ReferringGroupID = job.NewRequest.ReferringGroupId,
                Groups = job.JobAvailableToGroup.Select(x => x.GroupId).ToList(),
                RecipientOrganisation = job.NewRequest.OrganisationName,
                DateStatusLastChanged = job.RequestJobStatus.Max(x => x.DateCreated),
                DueDays = (dueDate.ToUKFromUTCTime().Date - DateTime.UtcNow.ToUKFromUTCTime().Date).Days,
                DateRequested = job.NewRequest.DateRequested,
                RequestorType = (RequestorType)job.NewRequest.RequestorType,
                Archive = job.NewRequest.Archive.Value,
                DueDateType = (DueDateType)job.DueDateTypeId,
                RequestorDefinedByGroup = job.NewRequest.RequestorDefinedByGroup,
                RequestID = job.NewRequest.Id,
                RequestType = (RequestType)job.NewRequest.RequestType,
                SuppressRecipientPersonalDetail = job.NewRequest.SuppressRecipientPersonalDetail,
                NotBeforeDate = job.NotBeforeDate,
                SpecificSupportActivity = job.SpecificSupportActivity
            };
        }

        private RequestSummary MapEFRequestToSummary(Request request)
        {
            RequestSummary result = null;
            if (request != null)
            {
                HelpMyStreet.Utils.Models.Shift shift = null;
                if (request.Shift != null)
                {
                    shift = new HelpMyStreet.Utils.Models.Shift()
                    {
                        StartDate = request.Shift.StartDate,
                        ShiftLength = request.Shift.ShiftLength,
                        RequestID = request.Id,
                        Location = (Location)request.Shift.LocationId
                    };
                }

                List<JobSummary> jobSummaries = new List<JobSummary>();
                List<ShiftJob> shiftJobs = new List<ShiftJob>();

                RequestType requestType = (RequestType)request.RequestType;

                switch (requestType)
                {
                    case RequestType.Task:
                        jobSummaries = request.Job.Select(job => new JobSummary()
                        {
                            IsHealthCritical = job.IsHealthCritical,
                            DueDate = job.DueDate,
                            JobID = job.Id,
                            VolunteerUserID = job.VolunteerUserId,
                            JobStatus = (JobStatuses)job.JobStatusId,
                            SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,
                            PostCode = job.NewRequest.PostCode,
                            ReferringGroupID = job.NewRequest.ReferringGroupId,
                            Groups = job.JobAvailableToGroup.Select(x => x.GroupId).ToList(),
                            RecipientOrganisation = job.NewRequest.OrganisationName,
                            DateStatusLastChanged = job.RequestJobStatus.Max(x => x.DateCreated),
                            DueDays = (job.DueDate.ToUKFromUTCTime().Date - DateTime.UtcNow.ToUKFromUTCTime().Date).Days,
                            DateRequested = job.NewRequest.DateRequested,
                            RequestorType = (RequestorType)job.NewRequest.RequestorType,
                            Archive = job.NewRequest.Archive.Value,
                            DueDateType = (DueDateType)job.DueDateTypeId,
                            RequestorDefinedByGroup = job.NewRequest.RequestorDefinedByGroup,
                            RequestID = job.NewRequest.Id,
                            RequestType = (RequestType)job.NewRequest.RequestType,
                            Reference = job.Reference,
                            SuppressRecipientPersonalDetail = job.NewRequest.SuppressRecipientPersonalDetail,
                            NotBeforeDate = job.NotBeforeDate,
                            Questions = MapToQuestions(job.JobQuestions),
                            SpecificSupportActivity = job.SpecificSupportActivity
                        }).ToList();
                        break;
                    case RequestType.Shift:
                        shiftJobs = request.Job.Select(job => new ShiftJob()
                        {
                            ReferringGroupID = request.ReferringGroupId,
                            JobID = job.Id,
                            VolunteerUserID = job.VolunteerUserId,
                            SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,
                            JobStatus = (JobStatuses)job.JobStatusId,
                            RequestType = (RequestType)request.RequestType,
                            RequestID = request.Id,
                            Location = (Location)request.Shift.LocationId,
                            StartDate = request.Shift.StartDate,
                            ShiftLength = request.Shift.ShiftLength,
                            DueDate = request.Shift.StartDate,
                            NotBeforeDate = job.NotBeforeDate,
                        }).ToList();
                        break;
                    default:
                        throw new Exception($"Unknown Request Type { requestType.ToString()}");
                }

                result = new RequestSummary()
                {
                    Shift = shift,
                    ReferringGroupID = request.ReferringGroupId,
                    Source = request.Source,
                    RequestType = (RequestType)request.RequestType,
                    RequestID = request.Id,
                    MultiVolunteer = request.MultiVolunteer,
                    Repeat = request.Repeat,
                    DateRequested = request.DateRequested,
                    PostCode = request.PostCode,
                    JobSummaries = jobSummaries,
                    ShiftJobs = shiftJobs,
                    SuppressRecipientPersonalDetail = jobSummaries.Any(js => js.SuppressRecipientPersonalDetail.GetValueOrDefault(false)),
                };
                return result;
            }
            else
            {
                throw new Exception($"Error  mapping EFReqest to Summary");
            }
        }

        private List<JobSummary> GetJobSummaries(List<EntityFramework.Entities.Job> jobs)
        {
            List<JobSummary> response = new List<JobSummary>();
            foreach (EntityFramework.Entities.Job j in jobs)
            {
                response.Add(MapEFJobToSummary(j));
            }
            return response;
        }

        private List<HelpMyStreet.Utils.Models.Question> MapToQuestions(ICollection<JobQuestions> questions)
        {
            return questions.Select(x => new HelpMyStreet.Utils.Models.Question
            {
                Id = x.QuestionId,
                Answer = x.Answer,
                Name = x.Question.Name,
                //Required = x.Question.Required,
                Type = (QuestionType)x.Question.QuestionType,
                AddtitonalData = JsonConvert.DeserializeObject<List<AdditonalQuestionData>>(x.Question.AdditionalData),
            }).ToList();
        }

        private RequestPersonalDetails GetPerson(Person person)
        {
            if (person != null)
            {
                return new RequestPersonalDetails()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    EmailAddress = person.EmailAddress,
                    MobileNumber = person.MobilePhone,
                    OtherNumber = person.OtherPhone,
                    Address = new Address()
                    {
                        AddressLine1 = person.AddressLine1,
                        AddressLine2 = person.AddressLine2,
                        AddressLine3 = person.AddressLine3,
                        Locality = person.Locality,
                        Postcode = person.Postcode
                    }
                };
            }
            else
            {
                //for shifts the recipient will be null
                return null;
            }
        }

        public GetJobDetailsResponse GetJobDetails(int jobID)
        {
            GetJobDetailsResponse response = new GetJobDetailsResponse();
            var efJob = _context.Job
                        .Include(i => i.RequestJobStatus)
                        .Include(i => i.JobQuestions)
                        .ThenInclude(rq => rq.Question)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i => i.PersonIdRecipientNavigation)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i => i.PersonIdRequesterNavigation)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i => i.Shift)
                        .Where(w => w.Id == jobID).FirstOrDefault();

            if (efJob == null)
            {
                return response;
            }

            bool isArchived = false;
            if (efJob.NewRequest.Archive.HasValue)
            {
                isArchived = efJob.NewRequest.Archive.Value;
            }

            var updateHistory = _context.UpdateHistory
                .Where(x => x.JobId == jobID)
                .Select(x => new HelpMyStreet.Contracts.RequestService.Response.UpdateHistory()
                {
                    FieldChanged = x.FieldChanged,
                    CreatedByUserID = x.CreatedByUserId,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    DateCreated = x.DateCreated,
                    QuestionID = x.QuestionId
                })
                .ToList();

            response = new GetJobDetailsResponse()
            {
                JobSummary = MapEFJobToSummary(efJob),
                Recipient = isArchived ? null : GetPerson(efJob.NewRequest.PersonIdRecipientNavigation),
                Requestor = isArchived ? null : GetPerson(efJob.NewRequest.PersonIdRequesterNavigation),
                History = GetJobStatusHistory(efJob.RequestJobStatus.ToList()),
                RequestSummary = MapEFRequestToSummary(efJob.NewRequest),
                UpdateHistory = updateHistory
            };

            return response;
        }

        public Task<List<LatitudeAndLongitudeDTO>> GetLatitudeAndLongitudes(List<string> postCodes, CancellationToken cancellationToken)
        {
            var postcodeDetails = (from a in _context.Postcode
                                   where postCodes.Any(p => p == a.Postcode)
                                   select new LatitudeAndLongitudeDTO
                                   {
                                       Postcode = a.Postcode,
                                       Latitude = Convert.ToDouble(a.Latitude),
                                       Longitude = Convert.ToDouble(a.Longitude)
                                   }).ToListAsync(cancellationToken);

            return postcodeDetails;
        }

        public List<StatusHistory> GetJobStatusHistory(int jobID)
        {
            return _context.RequestJobStatus.Where(x => x.JobId == jobID)
                .Select(x => new StatusHistory
                {
                    JobStatus = (JobStatuses)x.JobStatusId,
                    StatusDate = x.DateCreated,
                    VolunteerUserID = x.VolunteerUserId,
                    CreatedByUserID = x.CreatedByUserId
                }).ToList();
        }

        public List<StatusHistory> GetJobStatusHistory(List<RequestJobStatus> requestJobStatus)
        {
            return requestJobStatus
                .Select(x => new StatusHistory
                {
                    JobStatus = (JobStatuses)x.JobStatusId,
                    StatusDate = x.DateCreated,
                    VolunteerUserID = x.VolunteerUserId,
                    CreatedByUserID = x.CreatedByUserId,
                    JobStatusChangeReasonCode = (JobStatusChangeReasonCodes?) x.JobStatusChangeReasonCodeId
                }).ToList();
        }

        public async Task<List<int>> GetGroupsForJobAsync(int jobID, CancellationToken cancellationToken)
        {
            return _context.JobAvailableToGroup.Where(x => x.JobId == jobID)
                .Select(x => x.GroupId).ToList();
        }

        public async Task<List<int>> GetGroupsForRequestAsync(int requestID, CancellationToken cancellationToken)
        {
            return _context.JobAvailableToGroup
                        .Include(i => i.Job)
                        .Where(x => x.Job.RequestId == requestID)
                        .Select(x => x.GroupId).Distinct().ToList();
        }

        public async Task<int> GetReferringGroupIDForJobAsync(int jobID, CancellationToken cancellationToken)
        {
            var job = await _context.Job
                .Include(x => x.NewRequest)
                .FirstAsync(x => x.Id == jobID);

            if (job != null)
            {
                return job.NewRequest.ReferringGroupId;
            }
            else
            {
                throw new Exception($"Unable to get Referring GroupID for Job {jobID}");
            }
        }

        public async Task<int> GetReferringGroupIDForRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            var request = await _context.Request
                .FirstAsync(x => x.Id == requestId);

            if (request != null)
            {
                return request.ReferringGroupId;
            }
            else
            {
                throw new Exception($"Unable to get Referring GroupID for Request {requestId}");
            }
        }

        public void ArchiveOldRequests(int daysSinceJobRequested, int daysSinceJobStatusChanged)
        {
            DateTime dtExpire = DateTime.Now.AddDays(-daysSinceJobRequested);
            var requests = _context.Request
                .Include(x => x.Job)
                .ThenInclude(x => x.RequestJobStatus)
                .Where(x => (x.Archive ?? false == false)
                && ((x.PersonIdRecipient.HasValue || x.PersonIdRequester.HasValue))
                && x.DateRequested < dtExpire)
                .ToList();

            foreach (Request r in requests)
            {
                int inActiveCount = 0;

                foreach (EntityFramework.Entities.Job j in r.Job)
                {
                    if ((JobStatuses)j.JobStatusId.Value == JobStatuses.Done || (JobStatuses)j.JobStatusId.Value == JobStatuses.Cancelled)
                    {
                        if (j.RequestJobStatus.Min(x => (DateTime.Now.Date - x.DateCreated.Date).TotalDays > daysSinceJobStatusChanged))
                        {
                            inActiveCount++;
                        }
                    }
                }

                if (inActiveCount == r.Job.Count)
                {
                    r.Archive = true;
                    _context.Request.Update(r);
                }
            }
            _context.SaveChanges();
        }

        public GetJobSummaryResponse GetJobSummary(int jobID)
        {
            GetJobSummaryResponse response = new GetJobSummaryResponse();
            var efJob = _context.Job
                        .Include(i => i.RequestJobStatus)
                        .Include(i => i.JobQuestions)
                        .ThenInclude(rq => rq.Question)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i => i.Shift)
                        .Where(w => w.Id == jobID).FirstOrDefault();

            if (efJob != null)
            {
                response = new GetJobSummaryResponse()
                {
                    JobSummary = MapEFJobToSummary(efJob),
                    RequestID = efJob.NewRequest.Id,
                    RequestType = (RequestType)efJob.NewRequest.RequestType,
                    RequestSummary = MapEFRequestToSummary(efJob.NewRequest)
                };
            }
            return response;
        }

        public bool JobHasStatus(int jobID, JobStatuses status)
        {
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job.JobStatusId == (byte)status)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<HelpMyStreet.Utils.Models.Question>> GetQuestionsForActivity(HelpMyStreet.Utils.Enums.SupportActivities activity, RequestHelpFormVariant requestHelpFormVariant, RequestHelpFormStage requestHelpFormStage, CancellationToken cancellationToken)
        {
            return _context.ActivityQuestions
                        .Include(x => x.Question)
                        .Where(x => x.ActivityId == (int)activity && x.RequestFormVariantId == (int)requestHelpFormVariant && x.RequestFormStageId == (int)requestHelpFormStage)
                        .OrderBy(x=> x.Order)
                        .Select(x => new HelpMyStreet.Utils.Models.Question
                        {
                            Id = x.Question.Id,
                            Name = x.Question.Name,
                            Required = x.Required,
                            SubText = x.Subtext,
                            Location = x.Location,
                            PlaceholderText = x.PlaceholderText,
                            Type = (QuestionType)x.Question.QuestionType,
                            AddtitonalData = x.Question.AdditionalData != null ? JsonConvert.DeserializeObject<List<AdditonalQuestionData>>(x.Question.AdditionalData) : new List<AdditonalQuestionData>(),
                            AnswerContainsSensitiveData = x.Question.AnswerContainsSensitiveData,
                            AdditionalDataSource = (AdditionalDataSource)x.Question.AdditionalDataSource
                        }).ToList();
        }

        public bool JobIsInProgressWithSameVolunteerUserId(int jobID, int? volunteerUserID)
        {
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job.JobStatusId == (byte)JobStatuses.InProgress && job.VolunteerUserId == volunteerUserID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UpdateJobOutcome> UpdateJobDueDateAsync(int jobID, int authorisedByUserID, DateTime dueDate, CancellationToken cancellationToken)
        {
            UpdateJobOutcome response = UpdateJobOutcome.BadRequest;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.DueDate != dueDate)
                {
                    job.DueDate = dueDate;
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 1)
                    {
                        response = UpdateJobOutcome.Success;
                    }
                    else
                    {
                        response = UpdateJobOutcome.BadRequest;
                    }
                }
                else
                {
                    response = UpdateJobOutcome.AlreadyInThisState;
                }
            }
            return response;
        }

        public GetRequestDetailsResponse GetRequestDetails(int requestID)
        {
            GetRequestDetailsResponse response = new GetRequestDetailsResponse();

            var request = _context.Request
                .Include(i => i.PersonIdRecipientNavigation)
                .Include(i => i.PersonIdRequesterNavigation)
                .Include(i => i.Shift)
                .Include(i => i.Job)
                    .ThenInclude(i => i.RequestJobStatus)
                .Include(i => i.Job)
                    .ThenInclude(i => i.JobQuestions)
                    .ThenInclude(rq => rq.Question)
                .Where(x => x.Id == requestID)
                .FirstOrDefault();

            if (request == null)
            {
                return response;
            }

            bool isArchived = false;
            if (request.Archive.HasValue)
            {
                isArchived = request.Archive.Value;
            }

            response.RequestSummary = MapEFRequestToSummary(request);




            if (request.PersonIdRecipient.HasValue)
            {
                response.Recipient = isArchived ? null : GetPerson(request.PersonIdRecipientNavigation);
            }

            if (request.PersonIdRequester.HasValue)
            {
                response.Requestor = isArchived ? null : GetPerson(request.PersonIdRequesterNavigation);
            }
            return response;
        }
        public GetRequestSummaryResponse GetRequestSummary(int requestID)
        {
            GetRequestSummaryResponse response = new GetRequestSummaryResponse();

            var request = _context.Request
                .Include(i => i.Shift)
                .Include(i => i.Job)
                    .ThenInclude(i => i.RequestJobStatus)
                .Include(i => i.Job)
                    .ThenInclude(i => i.JobQuestions)
                    .ThenInclude(rq => rq.Question)
                .Where(x => x.Id == requestID)
                .First();

            if (request != null)
            {
                response.RequestSummary = MapEFRequestToSummary(request);
            }

            return response;
        }

        private int GetVolunteerCountForGivenRequestIDAndSupportActivity(int requestID, HelpMyStreet.Utils.Enums.SupportActivities activity, int volunteerUserID)
        {
            byte supportActivity = (byte)activity;
            byte jobStatusAccepted = (byte)JobStatuses.Accepted;

            return _context.Job.Count(x => x.RequestId == requestID
                                && x.SupportActivityId == supportActivity
                                && x.JobStatusId == jobStatusAccepted
                                && x.VolunteerUserId == volunteerUserID);
        }

        public int UpdateRequestStatusToAccepted(int requestID, HelpMyStreet.Utils.Enums.SupportActivities activity, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken)
        {
            byte supportActivity = (byte)activity;
            byte jobStatusOpen = (byte)JobStatuses.Open;
            byte requestTypeShift = (byte)RequestType.Shift;

            var job = _context.Job
                .Include(i => i.NewRequest)
                .FirstOrDefault(x => x.SupportActivityId == supportActivity
                && x.RequestId == requestID
                && x.JobStatusId == jobStatusOpen
                && x.NewRequest.RequestType == requestTypeShift);

            if (job != null)
            {
                //final check before adding volunteer to make sure that for given requestID and support activity
                //that the volunteer has not accepted a shift
                int count = GetVolunteerCountForGivenRequestIDAndSupportActivity(requestID, activity, volunteerUserID);

                if (count == 0)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        job.JobStatusId = (byte)JobStatuses.Accepted;
                        job.VolunteerUserId = volunteerUserID;
                        AddJobStatus(job, createdByUserID, volunteerUserID, JobStatuses.Accepted, JobStatusChangeReasonCodes.UserChange);
                        _context.SaveChanges();

                        count = GetVolunteerCountForGivenRequestIDAndSupportActivity(requestID, activity, volunteerUserID);

                        if (count < 2)
                        {
                            transaction.CommitAsync();
                            return job.Id;
                        }
                        else
                        {
                            transaction.RollbackAsync();
                            throw new UnableToUpdateShiftException($"Unable to UpdateShiftStatus for RequestID:{requestID} SupportActivity:{activity} Volunteer:{volunteerUserID}");
                        }
                    }
                }
                else
                {
                    throw new UnableToUpdateShiftException($"Unable to UpdateShiftStatus for RequestID:{requestID} SupportActivity:{activity} Volunteer:{volunteerUserID}");
                }
            }
            else
            {
                throw new UnableToUpdateShiftException($"Unable to UpdateShiftStatus for RequestID { requestID}");
            }

        }

        public async Task<int> VolunteerAlreadyAcceptedShift(int requestID, HelpMyStreet.Utils.Enums.SupportActivities activity, int volunteerUserID, CancellationToken cancellationToken)
        {
            byte supportActivity = (byte)activity;
            byte jobStatusAccepted = (byte)JobStatuses.Accepted;
            byte requestTypeShift = (byte)RequestType.Shift;

            var job = _context.Job
                .Include(i => i.NewRequest)
                .FirstOrDefault(x => x.VolunteerUserId == volunteerUserID
                && x.SupportActivityId == supportActivity
                && x.RequestId == requestID
                && x.JobStatusId == jobStatusAccepted
                && x.NewRequest.RequestType == requestTypeShift);

            if (job != null)
            {
                return job.Id;
            }
            else
            {
                return -1;
            }
        }

        public async Task<int> GetRequestIDFromGuid(Guid guid)
        {
            //check if guid already exists
            var request = _context.Request.FirstOrDefault(x => x.Guid == guid);

            if (request != null)
            {
                return request.Id;
            }
            else
            {
                return -1;
            }
        }

        public List<ShiftJob> GetUserShiftJobsByFilter(GetUserShiftJobsByFilterRequest request)
        {
            byte requestTypeShift = (byte)RequestType.Shift;
            var jobs = _context.Job
                .Include(i => i.NewRequest)
                .ThenInclude(i => i.Shift)
                .Where(x => x.VolunteerUserId == request.VolunteerUserId && x.NewRequest.RequestType == requestTypeShift);

            if (jobs == null)
            {
                throw new Exception($"GetUserShiftJobsByFilter returned null for user id {request.VolunteerUserId}");
            }

            if (jobs.Count() == 0)
            {
                return new List<ShiftJob>();
            }

            if (request.JobStatusRequest.JobStatuses.Count > 0)
            {
                jobs = jobs.Where(x => request.JobStatusRequest.JobStatuses.Contains((JobStatuses)x.JobStatusId));
            };

            if (request.DateFrom.HasValue)
            {
                jobs = jobs.Where(x => x.NewRequest.Shift.StartDate.AddMinutes(x.NewRequest.Shift.ShiftLength) >= request.DateFrom.Value);
            }

            if (request.DateTo.HasValue)
            {
                jobs = jobs.Where(x => x.NewRequest.Shift.StartDate <= request.DateTo.Value);
            }

            return jobs.Select(x => new ShiftJob()
            {
                Location = (Location)x.NewRequest.Shift.LocationId,
                ReferringGroupID = x.NewRequest.ReferringGroupId,
                JobID = x.Id,
                RequestID = x.NewRequest.Id,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)x.SupportActivityId,
                JobStatus = (JobStatuses)x.JobStatusId,
                StartDate = x.NewRequest.Shift.StartDate,
                ShiftLength = x.NewRequest.Shift.ShiftLength,
                VolunteerUserID = x.VolunteerUserId,
                DateRequested = x.NewRequest.DateRequested,
                RequestType = (RequestType)x.NewRequest.RequestType
            }).ToList();
        }

        public List<ShiftJob> GetOpenShiftJobsByFilter(GetOpenShiftJobsByFilterRequest request, List<int> referringGroups)
        {
            byte requestTypeShift = (byte)RequestType.Shift;
            byte jobstatusOpen = (byte)JobStatuses.Open;
            var jobs = _context.Job
                .Include(i => i.NewRequest)
                .ThenInclude(i => i.Shift)
                .Include(i => i.JobAvailableToGroup)
                .Where(x => x.NewRequest.RequestType == requestTypeShift
                && x.JobStatusId == jobstatusOpen);

            if (jobs == null || jobs.Count() == 0)
            {
                return new List<ShiftJob>();
            }

            if (request.ExcludeSiblingsOfJobsAllocatedToUserID.HasValue)
            {
                //TODO
            }

            if (request.SupportActivities?.SupportActivities.Count > 0)
            {
                jobs = jobs.Where(x => request.SupportActivities.SupportActivities.Contains((HelpMyStreet.Utils.Enums.SupportActivities)x.SupportActivityId));
            };

            if (referringGroups.Count > 0)
            {
                jobs = jobs.Where(x => referringGroups.Contains(x.NewRequest.ReferringGroupId));
            }

            if (request.Groups?.Groups.Count > 0)
            {
                jobs = jobs.Where(x => x.JobAvailableToGroup.Any(a => request.Groups.Groups.Contains(a.GroupId)));
            }

            if (request.Locations?.Locations.Count > 0)
            {
                jobs = jobs.Where(x => request.Locations.Locations.Contains((Location)x.NewRequest.Shift.LocationId));
            }

            if (request.DateFrom.HasValue)
            {
                jobs = jobs.Where(x => x.NewRequest.Shift.StartDate.AddMinutes(x.NewRequest.Shift.ShiftLength) >= request.DateFrom.Value);
            }

            if (request.DateTo.HasValue)
            {
                jobs = jobs.Where(x => x.NewRequest.Shift.StartDate <= request.DateTo.Value);
            }

            return jobs.Select(x => new ShiftJob()
            {
                Location = (Location)x.NewRequest.Shift.LocationId,
                ReferringGroupID = x.NewRequest.ReferringGroupId,
                JobID = x.Id,
                RequestID = x.NewRequest.Id,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)x.SupportActivityId,
                JobStatus = (JobStatuses)x.JobStatusId,
                StartDate = x.NewRequest.Shift.StartDate,
                ShiftLength = x.NewRequest.Shift.ShiftLength,
                VolunteerUserID = x.VolunteerUserId,
                DateRequested = x.NewRequest.DateRequested,
                RequestType = (RequestType)x.NewRequest.RequestType
            }).ToList();
        }

        public List<RequestSummary> GetShiftRequestsByFilter(GetShiftRequestsByFilterRequest request, List<int> referringGroups)
        {
            byte requestTypeShift = (byte)RequestType.Shift;

            var requests = _context.Request
                .Include(i => i.Shift)
                .Include(i => i.Job)
                .ThenInclude(i => i.JobAvailableToGroup)
                .Where(x => x.RequestType == requestTypeShift);

            if (requests == null || requests.Count() == 0)
            {
                return new List<RequestSummary>();
            }

            if (referringGroups.Count > 0)
            {
                requests = requests.Where(x => referringGroups.Contains(x.ReferringGroupId));
            }

            if (request.Groups?.Groups.Count > 0)
            {
                requests = requests.Where(x => x.Job.SelectMany(x => x.JobAvailableToGroup).Any(a => request.Groups.Groups.Contains(a.GroupId)));
            }

            if (request.Locations?.Locations.Count > 0)
            {
                requests = requests.Where(x => request.Locations.Locations.Contains((Location)x.Shift.LocationId));
            }

            if (request.DateFrom.HasValue)
            {
                requests = requests.Where(x => x.Shift.StartDate.AddMinutes(x.Shift.ShiftLength) >= request.DateFrom.Value);
            }

            if (request.DateTo.HasValue)
            {
                requests = requests.Where(x => x.Shift.StartDate <= request.DateTo.Value);
            }

            var results = requests.ToList();
            return results.Select(x => MapEFRequestToSummary(x)).ToList();
        }

        public async Task<List<int>> UpdateRequestStatusToCancelledAsync(int requestId, int createdByUserID, JobStatusChangeReasonCodes jobStatusChangeReasonCode, CancellationToken cancellationToken)
        {
            List<int> result = new List<int>();

            byte cancelledJobStatus = (byte)JobStatuses.Cancelled;

            var jobs = _context.Job.Where(w => w.RequestId == requestId && w.JobStatusId != cancelledJobStatus);

            if (jobs == null)
            {
                //No jobs need to be cancelled
                return result;
            }

            foreach (EntityFramework.Entities.Job job in jobs)
            {
                job.JobStatusId = cancelledJobStatus;
                job.VolunteerUserId = null;
                AddJobStatus(job, createdByUserID, null, JobStatuses.Cancelled, jobStatusChangeReasonCode);
                result.Add(job.Id);
            }
            await _context.SaveChangesAsync(cancellationToken);

            if (jobs.Count() == 0)
            {
                return result;
            }
            else
            {
                throw new Exception($"Error when updating request status to cancelled for requestId={requestId}");
            }
        }

        public async Task<List<int>> UpdateRequestStatusToDoneAsync(int requestId, int createdByUserID, CancellationToken cancellationToken)
        {
            List<int> result = new List<int>();

            byte byteDoneJobStatus = (byte)JobStatuses.Done;
            byte byteCancelledJobStatus = (byte)JobStatuses.Cancelled;

            var jobs = _context.Job.Where(w => w.RequestId == requestId && w.JobStatusId != byteCancelledJobStatus && w.JobStatusId != byteDoneJobStatus);

            if (jobs == null)
            {
                //No jobs need to be changed
                return result;
            }

            foreach (EntityFramework.Entities.Job job in jobs)
            {
                if (job.JobStatusId == (byte)JobStatuses.New || job.JobStatusId == (byte)JobStatuses.Open)
                {
                    job.JobStatusId = byteCancelledJobStatus;
                    job.VolunteerUserId = null;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Cancelled, JobStatusChangeReasonCodes.UserChange);
                    result.Add(job.Id);
                }

                if (job.JobStatusId == (byte)JobStatuses.Accepted || job.JobStatusId == (byte)JobStatuses.InProgress)
                {
                    job.JobStatusId = byteDoneJobStatus;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Done, JobStatusChangeReasonCodes.UserChange);
                    result.Add(job.Id);
                }

            }
            await _context.SaveChangesAsync(cancellationToken);

            if (jobs.Count() == 0)
            {
                return result;
            }
            else
            {
                throw new Exception($"Error when updating request status to done for requestId={requestId}");
            }
        }

        private List<EntityFramework.Entities.Job> GetJobsWhereShiftStartDateHasPassed(JobStatuses jobStatus)
        {
            byte jobStatusID = (byte)jobStatus;
            byte dueDateTypeSpecificStartAndEndTimesID = (byte)DueDateType.SpecificStartAndEndTimes;
            DateTime now = DateTime.Now;

            var jobs = _context.Job
                            .Include(i => i.NewRequest)
                            .ThenInclude(i => i.Shift)
                            .Where(x => x.JobStatusId == jobStatusID
                            && x.DueDateTypeId == dueDateTypeSpecificStartAndEndTimesID
                            && x.NewRequest.Shift.StartDate < now
                            ).ToList();
            return jobs;
        }

        private List<EntityFramework.Entities.Job> GetJobsWhereShiftsHaveEnded(List<EntityFramework.Entities.Job> jobs)
        {
            return jobs.Where(x => x.NewRequest.Shift.StartDate.AddMinutes(x.NewRequest.Shift.ShiftLength) < DateTime.Now).ToList();
        }
        public async Task UpdateInProgressFromAccepted(JobStatusChangeReasonCodes jobStatusChangeReasonCode)
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.Accepted);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusInProgressAsync(job.Id, -1, job.VolunteerUserId.Value, jobStatusChangeReasonCode, CancellationToken.None);
                }
            }

        }

        public async Task UpdateJobsToDoneFromInProgress(JobStatusChangeReasonCodes jobStatusChangeReasonCode)
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.InProgress);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusDoneAsync(job.Id, -1, jobStatusChangeReasonCode, CancellationToken.None);
                }
            }
        }

        public async Task UpdateJobsToCancelledFromNewOrOpen(JobStatusChangeReasonCodes jobStatusChangeReasonCode)
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.New);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusCancelledAsync(job.Id, -1, jobStatusChangeReasonCode, CancellationToken.None);
                }
            }

            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.Open);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusCancelledAsync(job.Id, -1, JobStatusChangeReasonCodes.AutoProgressingShifts, CancellationToken.None);
                }
            }
        }

        public List<RequestSummary> GetRequestsByFilter(GetRequestsByFilterRequest request, List<int> referringGroups)
        {
            IQueryable<Request> requests = _context.Request
               .Include(i => i.Shift)
               .Include(i => i.Job)
               .ThenInclude(i => i.JobAvailableToGroup)
               .Include(i => i.Job)
               .ThenInclude(i => i.RequestJobStatus);

            if (requests == null || requests.Count() == 0)
            {
                return new List<RequestSummary>();
            }

            if (referringGroups.Count > 0)
            {
                requests = requests.Where(x => referringGroups.Contains(x.ReferringGroupId));
            }

            if (request.Groups?.Groups.Count > 0)
            {
                requests = requests.Where(x => x.Job.SelectMany(x => x.JobAvailableToGroup).Any(a => request.Groups.Groups.Contains(a.GroupId)));
            }

            if (request.RequestType?.RequestTypes.Count > 0)
            {
                requests = requests.Where(x => request.RequestType.RequestTypes.Contains((RequestType)x.RequestType));
            };

            var results = requests.ToList();

            return results.Select(x => MapEFRequestToSummary(x)).ToList();
        }

        public List<RequestSummary> GetAllRequests(List<int> RequestIDs)
        {
            var allRequests = _context.Request
                .Include(i => i.Shift)
                .Include(i => i.Job)
                    .ThenInclude(i => i.JobAvailableToGroup)
                .Include(i => i.Job)                
                    .ThenInclude(i => i.RequestJobStatus)
                .Include(i => i.Job)
                    .ThenInclude(i => i.JobQuestions)
                    .ThenInclude(rq => rq.Question)
                .Where(w => RequestIDs.Contains(w.Id)).ToList();

            return allRequests.Select(x => MapEFRequestToSummary(x)).ToList();
        }

        /// <summary>
        /// Return true if volunteeer has already a similar job defined by JobBasicDedupeWithDate_EqualityComparer 
        /// with the status that we are trying to allocate this job to
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="volunteerUserId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool VolunteerHasAlreadyJobForThisRequestWithThisStatus(int jobId, int volunteerUserId, JobStatuses status)
        {
            var job = _context.Job.Where(x => x.Id == jobId)
                .Select(x => new JobBasic()
                {
                    JobID = x.Id,
                    RequestID = x.RequestId,
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)x.SupportActivityId,
                    DueDateType = (DueDateType)x.DueDateTypeId,
                    DueDate = x.DueDate,
                    NotBeforeDate = x.NotBeforeDate
                })
                .First();

            if(job==null)
            {
                throw new Exception($"Unable to retrieve details for job {jobId}");
            }

            byte jobStatus = (byte)status;

            var userJobs = _context.Job.Where(x => x.RequestId == job.RequestID && x.VolunteerUserId == volunteerUserId && x.JobStatusId == jobStatus)
                .Select(x => new JobBasic()
                {
                    JobID = x.Id,
                    RequestID = x.RequestId,
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)x.SupportActivityId,
                    DueDateType = (DueDateType)x.DueDateTypeId,
                    DueDate = x.DueDate,
                    NotBeforeDate = x.NotBeforeDate
                }).ToList();

            if(userJobs.Contains(job, _jobBasicDedupeWithDate_EqualityComparer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<int>> GetOverdueRepeatJobs()
        {
            byte jobStatusNew = (byte)JobStatuses.New;
            byte jobStatusOpen = (byte)JobStatuses.Open;

            DateTime sixHoursAgo = DateTime.UtcNow.AddHours(-6);

            return _context.Job
                .Include(x => x.NewRequest)
                .Where(x => x.NewRequest.Repeat == true
                            && x.DueDate < sixHoursAgo
                            && (x.JobStatusId == jobStatusNew || x.JobStatusId == jobStatusOpen)
                        )
                .Select(x => x.Id);       
        }

        public async Task<Dictionary<int, int>> GetAllRequestIDs(List<int> JobIDs)
        {
            Dictionary<int, int> ids = new Dictionary<int, int>();
            _context.Job.Where(x => JobIDs.Contains(x.Id))
                .ToList()
                .ForEach(value => ids.Add(value.Id, value.RequestId));

            return ids;
        }

        public async Task<bool> LogRequestEvent(int requestId, int? jobId, int userId, RequestEvent requestEvent)
        {
            bool success = false;

            _context.LogRequestEvent.Add(new LogRequestEvent()
            {
                RequestId = requestId,
                JobId = jobId,
                UserId = userId,
                RequestEventId = (byte) requestEvent
            });
            var result = await _context.SaveChangesAsync();

            if(result == 1)
            {
                success = true;
            }

            return success;
        }

        public async Task UpdateHistory(int requestId, int createdByUserId, string fieldChanged, string oldValue, string newValue, int? questionId, int jobId = 0)
        {
            _context.UpdateHistory.Add(new Repo.EntityFramework.Entities.UpdateHistory()
            {
                RequestId = requestId,
                JobId = jobId,
                FieldChanged = fieldChanged,
                OldValue = oldValue,
                NewValue = newValue,
                CreatedByUserId = createdByUserId,
                QuestionId = questionId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRequest(int requestId, CancellationToken cancellationToken)
        {
            var request = _context.Request
                            .Include(i=> i.PersonIdRecipientNavigation)
                            .Include(i=> i.PersonIdRequesterNavigation)
                            .Include(i=> i.Shift)
                            .Include(i => i.RequestSubmission)
                            .Include(i => i.UpdateHistory)
                            .Include(i => i.LogRequestEvent)
                            .Include(i => i.SupportActivities)
                            .Include(i => i.Job)
                            .ThenInclude(i=> i.RequestJobStatus)
                            .Include(i=> i.Job)
                            .ThenInclude(i=> i.JobQuestions)
                            .Include(i => i.Job)
                            .ThenInclude(i => i.JobAvailableToGroup)
                            .First(x => x.Id == requestId);

            int? personId_recipient = request.PersonIdRecipient;
            int? personId_requestor = request.PersonIdRequester;

            _context.Remove(request);

            var result =_context.SaveChanges();

            if(result>0)
            {
                var persons = _context.Person.Where(x => x.Id == personId_recipient.Value || x.Id == personId_requestor).ToList();
                _context.RemoveRange(persons);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }            
        }

        public async Task<IEnumerable<SupportActivityCount>> GetCompletedActivitiesCount(IEnumerable<int> groups)
        {
            Byte jobstatus_done = (byte)JobStatuses.Done;

            return _context.Job
                .Include(i => i.NewRequest)
                .Where
                (
                    x => x.JobStatusId == jobstatus_done &&
                    (groups.Count() == 0 || groups.Contains(x.NewRequest.ReferringGroupId))                    
                )
                .GroupBy(p => p.SupportActivityId)
                .Select(g => new SupportActivityCount
                {
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities) g.Key,
                    Value = g.Count()
                });            
        }

        public async Task<IEnumerable<SupportActivityCount>> GetActivitiesCompletedLastXDaysCount(IEnumerable<int> groups, int days)
        {
            DateTime dtLessThanXDays = DateTime.UtcNow.Date.AddDays(-days);

            Byte jobstatus_done = (byte)JobStatuses.Done;

            return _context.RequestJobStatus
                .Include(i => i.Job)
                .ThenInclude(i => i.NewRequest)
                .Where
                (
                    x => x.JobStatusId == jobstatus_done &&
                    (groups.Count() == 0 || groups.Contains(x.Job.NewRequest.ReferringGroupId))
                    && x.DateCreated > dtLessThanXDays
                )
                .GroupBy(p => p.Job.SupportActivityId)
                .Select(g => new SupportActivityCount
                {
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)g.Key,
                    Value = g.Count()
                });
        }

        public async Task<IEnumerable<SupportActivityCount>> GetRequestsAddedLastXDaysCount(IEnumerable<int> groups, int days)
        {
            DateTime dtLessThanXDays = DateTime.UtcNow.Date.AddDays(-days);

            return _context.Job
                .Include(i => i.NewRequest)
                .Where
                (
                    x => x.NewRequest.DateRequested > dtLessThanXDays &&
                    (groups.Count() == 0 || groups.Contains(x.NewRequest.ReferringGroupId))
                )
                .GroupBy(p => p.SupportActivityId)
                .Select(g => new SupportActivityCount
                {
                    SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)g.Key,
                    Value = g.Count()
                });
        }

        public async Task<int> OpenJobCount(IEnumerable<int> groups)
        {
            Byte jobstatus_open = (byte)JobStatuses.Open;

            return _context.Job
                .Count
                (
                    x => x.JobStatusId == jobstatus_open && 
                    (groups.Count() == 0 || groups.Contains(x.NewRequest.ReferringGroupId))
                );
        }
        
        public async Task<IEnumerable<int>> GetJobsPastDueDate(JobStatuses jobStatus, int days)
        {
            byte requestType_task = (byte)RequestType.Task;
            DateTime dt = DateTime.UtcNow.Date.AddDays(-days);

            return _context.Job
                .Include(x => x.NewRequest)
                .Where(x => x.JobStatusId == (byte) jobStatus
                && x.NewRequest.RequestType == requestType_task
                && (x.DueDate < dt)
                )
                .Select(x => x.Id);
        }
		
        public async Task<List<JobBasic>> GetActivitiesByMonth(IEnumerable<int> groups, DateTime minDate, DateTime maxDate)
        {
            return GetJobBasics(_context.Job
                    .Include(i => i.RequestJobStatus)
                    .Include(i => i.NewRequest)
                    .Where(x => groups.Contains(x.NewRequest.ReferringGroupId) && x.NewRequest.DateRequested >= minDate && x.NewRequest.DateRequested <= maxDate)
                    .ToList());
        }

        public async Task<List<JobBasic>> RequestVolumeByDueDateAndRecentStatus(IEnumerable<int> groups, DateTime minDate, DateTime maxDate)
        {
            return GetJobBasics(_context.Job
                    .Include(i => i.RequestJobStatus)
                    .Include(i => i.NewRequest)
                    .Where(x => groups.Contains(x.NewRequest.ReferringGroupId) && x.DueDate >= minDate && x.DueDate <= maxDate)
                    .ToList());  
        }

        public async Task<List<JobBasic>> RequestVolumeByActivity(IEnumerable<int> groups, DateTime minDate, DateTime maxDate)
        {
            return GetJobBasics(_context.Job
                    .Include(i => i.RequestJobStatus)
                    .Include(i => i.NewRequest)
                    .Where(x => groups.Contains(x.NewRequest.ReferringGroupId) && x.DueDate >= minDate && x.DueDate <= maxDate)
                    .ToList());
        }

        public async Task<List<int?>> RecentActiveVolunteersByVolumeAcceptedRequests(IEnumerable<int> groups, DateTime minDate, DateTime maxDate)
        {
            byte jobStatus_InProgress = (byte)JobStatuses.InProgress;
            byte jobStatus_Accepted = (byte)JobStatuses.Accepted;
            byte jobStatus_Done = (byte)JobStatuses.Done;

            return _context.Job
                  .Include(i => i.NewRequest)
                  .Where(x => groups.Contains(x.NewRequest.ReferringGroupId) && x.DueDate >= minDate && x.DueDate <= maxDate &&
                  (x.JobStatusId == jobStatus_InProgress || x.JobStatusId == jobStatus_Accepted || x.JobStatusId == jobStatus_Done))
                  .Select(s => s.VolunteerUserId)
                  .ToList();
	    }

        public List<int> GetRequestsIdsForGroup(List<int> referringGroups)
        {
            return _context.Request
                .Where(x => referringGroups.Contains(x.ReferringGroupId))
                .Select(x => x.Id).ToList();
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusToApprovedAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte approvedJobStatus = (byte)JobStatuses.Approved;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != approvedJobStatus)
                {
                    job.JobStatusId = approvedJobStatus;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Approved,JobStatusChangeReasonCodes.UserChange);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                    else
                    {
                        response = UpdateJobStatusOutcome.BadRequest;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusToRejectedAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte rejectedJobStatus = (byte)JobStatuses.Rejected;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != rejectedJobStatus)
                {
                    job.JobStatusId = rejectedJobStatus;
                    AddJobStatus(job, createdByUserID, null, JobStatuses.Rejected, JobStatusChangeReasonCodes.UserChange);
                    int result = _context.SaveChanges();
                    if (result == 2)
                    {
                        response = UpdateJobStatusOutcome.Success;
                    }
                    else
                    {
                        response = UpdateJobStatusOutcome.BadRequest;
                    }
                }
                else
                {
                    response = UpdateJobStatusOutcome.AlreadyInThisStatus;
                }
            }
            return response;
        }
    }
}
