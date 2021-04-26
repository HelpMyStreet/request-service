using HelpMyStreet.Contracts.ReportService.Response;
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
using System.Security.Cryptography.X509Certificates;
using RequestService.Core.Domains.Entities;
using Polly.Caching;
using HelpMyStreet.Utils.Extensions;
using AutoMapper.Configuration.Conventions;
using HelpMyStreet.Utils.EqualityComparers;

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
            List<DailyReport> result = _context.DailyReport.ToList();

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

        public async Task<int> NewHelpRequestAsync(PostNewRequestForHelpRequest postNewRequestForHelpRequest, Fulfillable fulfillable, bool requestorDefinedByGroup, bool multiVolunteer, bool repeat)
        {

            Person requester = GetPersonFromPersonalDetails(postNewRequestForHelpRequest.HelpRequest.Requestor);
            Person recipient;

            if (postNewRequestForHelpRequest.HelpRequest.RequestorType == RequestorType.Myself)
            {
                recipient = requester;
            }
            else
            {
                recipient = GetPersonFromPersonalDetails(postNewRequestForHelpRequest.HelpRequest.Recipient);
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

                    RequestType requestType = postNewRequestForHelpRequest.NewJobsRequest.Jobs.First().SupportActivity.RequestType();

                    Request newRequest = new Request()
                    {
                        Guid = postNewRequestForHelpRequest.HelpRequest.Guid,
                        ReadPrivacyNotice = postNewRequestForHelpRequest.HelpRequest.ReadPrivacyNotice,
                        SpecialCommunicationNeeds = postNewRequestForHelpRequest.HelpRequest.SpecialCommunicationNeeds,
                        AcceptedTerms = postNewRequestForHelpRequest.HelpRequest.AcceptedTerms,
                        OtherDetails = postNewRequestForHelpRequest.HelpRequest.OtherDetails,
                        OrganisationName = postNewRequestForHelpRequest.HelpRequest.OrganisationName,
                        PostCode = PostcodeFormatter.FormatPostcode(postNewRequestForHelpRequest.HelpRequest.Recipient?.Address?.Postcode ?? string.Empty),
                        PersonIdRecipientNavigation = recipient,
                        PersonIdRequesterNavigation = requester,
                        RequestorType = (byte)postNewRequestForHelpRequest.HelpRequest.RequestorType,
                        FulfillableStatus = (byte)fulfillable,
                        CreatedByUserId = postNewRequestForHelpRequest.HelpRequest.CreatedByUserId,
                        ReferringGroupId = postNewRequestForHelpRequest.HelpRequest.ReferringGroupId,
                        Source = postNewRequestForHelpRequest.HelpRequest.Source,
                        RequestorDefinedByGroup = requestorDefinedByGroup,
                        RequestType = (byte)requestType,
                        Archive = false,
                        MultiVolunteer = multiVolunteer,
                        Repeat = repeat
                    };

                    if (requestType == RequestType.Shift)
                    {
                        HelpMyStreet.Utils.Models.Job shiftJob = postNewRequestForHelpRequest.NewJobsRequest.Jobs.First();

                        var locationQuestion = shiftJob.Questions.Where(x => x.Id == (int)Questions.Location).First();

                        if (locationQuestion == null)
                        {
                            throw new Exception("Missing location question");
                        }

                        int locationId = Convert.ToInt32(locationQuestion.AddtitonalData.Where(x => x.Value == locationQuestion.Answer).First().Key);

                        _context.Shift.Add(new EntityFramework.Entities.Shift()
                        {
                            Request = newRequest,
                            StartDate = shiftJob.StartDate.Value,
                            ShiftLength = (int)(shiftJob.EndDate.Value - shiftJob.StartDate.Value).TotalMinutes,
                            LocationId = locationId
                        });
                    }

                    foreach (HelpMyStreet.Utils.Models.Job job in postNewRequestForHelpRequest.NewJobsRequest.Jobs)
                    {
                        EntityFramework.Entities.Job EFcoreJob = new EntityFramework.Entities.Job()
                        {
                            NewRequest = newRequest,
                            IsHealthCritical = job.HealthCritical,
                            SupportActivityId = (byte)job.SupportActivity,
                            DueDate = job.StartDate.Value,
                            DueDateTypeId = (byte)job.DueDateType,
                            JobStatusId = (byte)JobStatuses.New,
                            NotBeforeDate = job.NotBeforeDate,
                            Reference = job.Questions.Where(x => x.Id == (int)Questions.AgeUKReference).FirstOrDefault()?.Answer
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

                        _context.RequestJobStatus.Add(new RequestJobStatus()
                        {
                            DateCreated = DateTime.Now,
                            JobStatusId = (byte)JobStatuses.New,
                            Job = EFcoreJob,
                            CreatedByUserId = postNewRequestForHelpRequest.HelpRequest.CreatedByUserId,
                        });
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
                            _context.RequestJobStatus.Add(new RequestJobStatus()
                            {
                                DateCreated = DateTime.Now,
                                JobStatusId = (byte)JobStatuses.Open,
                                Job = EFcoreJob,
                                CreatedByUserId = postNewShiftsRequest.CreatedByUserId,
                            });
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

        private void AddJobStatus(int jobID, int? createdByUserID, int? volunteerUserID, byte jobStatus)
        {
            _context.RequestJobStatus.Add(new RequestJobStatus()
            {
                CreatedByUserId = createdByUserID,
                VolunteerUserId = volunteerUserID,
                JobId = jobID,
                JobStatusId = jobStatus
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
                    AddJobStatus(jobID, createdByUserID, null, openJobStatus);
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

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusCancelledAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
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
                    AddJobStatus(jobID, createdByUserID, null, cancelledJobStatus);
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

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusInProgressAsync(int jobID, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken)
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
                    AddJobStatus(jobID, createdByUserID, volunteerUserID, inProgressJobStatus);
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

        public async Task<UpdateJobStatusOutcome> UpdateJobStatusDoneAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            UpdateJobStatusOutcome response = UpdateJobStatusOutcome.BadRequest;
            byte doneJobStatus = (byte)JobStatuses.Done;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != doneJobStatus)
                {
                    job.JobStatusId = doneJobStatus;
                    AddJobStatus(jobID, createdByUserID, null, doneJobStatus);
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
                    AddJobStatus(jobID, createdByUserID, null, newJobStatus);
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
                if (job.JobStatusId != acceptedJobStatus)
                {
                    job.JobStatusId = acceptedJobStatus;
                    job.VolunteerUserId = volunteerUserID; ;
                    AddJobStatus(jobID, createdByUserID, volunteerUserID, acceptedJobStatus);
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
                    NotBeforeDate = j.NotBeforeDate
                });
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
                DueDays = (dueDate.Date - DateTime.UtcNow.Date).Days,
                DateRequested = job.NewRequest.DateRequested,
                RequestorType = (RequestorType)job.NewRequest.RequestorType,
                Archive = job.NewRequest.Archive.Value,
                DueDateType = (DueDateType)job.DueDateTypeId,
                RequestorDefinedByGroup = job.NewRequest.RequestorDefinedByGroup,
                RequestID = job.NewRequest.Id,
                RequestType = (RequestType)job.NewRequest.RequestType,
                NotBeforeDate = job.NotBeforeDate
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
                            DueDays = (job.DueDate.Date - DateTime.UtcNow.Date).Days,
                            DateRequested = job.NewRequest.DateRequested,
                            RequestorType = (RequestorType)job.NewRequest.RequestorType,
                            Archive = job.NewRequest.Archive.Value,
                            DueDateType = (DueDateType)job.DueDateTypeId,
                            RequestorDefinedByGroup = job.NewRequest.RequestorDefinedByGroup,
                            RequestID = job.NewRequest.Id,
                            RequestType = (RequestType)job.NewRequest.RequestType,
                            NotBeforeDate = job.NotBeforeDate
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
                            NotBeforeDate = job.NotBeforeDate
                        }).ToList();
                        break;
                    default:
                        throw new Exception($"Unknown Request Type { requestType.ToString()}");
                }

                result = new RequestSummary()
                {
                    Shift = shift,
                    ReferringGroupID = request.ReferringGroupId,
                    RequestType = (RequestType)request.RequestType,
                    RequestID = request.Id,
                    MultiVolunteer = request.MultiVolunteer,
                    Repeat = request.Repeat,
                    DateRequested = request.DateRequested,
                    PostCode = request.PostCode,
                    JobSummaries = jobSummaries,
                    ShiftJobs = shiftJobs
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

            response = new GetJobDetailsResponse()
            {
                JobSummary = MapEFJobToSummary(efJob),
                Recipient = isArchived ? null : GetPerson(efJob.NewRequest.PersonIdRecipientNavigation),
                Requestor = isArchived ? null : GetPerson(efJob.NewRequest.PersonIdRequesterNavigation),
                History = GetJobStatusHistory(efJob.RequestJobStatus.ToList()),
                RequestSummary = MapEFRequestToSummary(efJob.NewRequest)
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

        public async Task AddJobAvailableToGroupAsync(int jobID, int groupID, CancellationToken cancellationToken)
        {
            _context.JobAvailableToGroup.Add(new JobAvailableToGroup()
            {
                GroupId = groupID,
                JobId = jobID
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRequestAvailableToGroupAsync(int requestID, int groupID, CancellationToken cancellationToken)
        {
            _context.Job.Where(x => x.RequestId == requestID)
                .ToList()
                .ForEach(v =>
                {
                    _context.JobAvailableToGroup.Add(new JobAvailableToGroup()
                    {
                        GroupId = groupID,
                        JobId = v.Id
                    });
                });

            await _context.SaveChangesAsync(cancellationToken);
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
                    CreatedByUserID = x.CreatedByUserId
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
                        AddJobStatus(job.Id, createdByUserID, volunteerUserID, (byte)JobStatuses.Accepted);
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

        public async Task<bool> UpdateAllJobStatusToOpenForRequestAsync(int requestId, int createdByUserID, CancellationToken cancellationToken)
        {
            byte openJobStatus = (byte)JobStatuses.Open;
            var jobs = _context.Job.Where(w => w.RequestId == requestId && w.JobStatusId != openJobStatus);

            if (jobs == null)
            {
                //Not throwing an error as requestid might not exist or no not open jobs exist for request id
                return false;
            }

            foreach (EntityFramework.Entities.Job job in jobs)
            {
                job.JobStatusId = openJobStatus;
                job.VolunteerUserId = null;
                AddJobStatus(job.Id, createdByUserID, null, openJobStatus);
            }
            int result = await _context.SaveChangesAsync(cancellationToken);

            if (jobs.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<int>> UpdateRequestStatusToCancelledAsync(int requestId, int createdByUserID, CancellationToken cancellationToken)
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
                AddJobStatus(job.Id, createdByUserID, null, cancelledJobStatus);
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
                    AddJobStatus(job.Id, createdByUserID, null, byteCancelledJobStatus);
                    result.Add(job.Id);
                }

                if (job.JobStatusId == (byte)JobStatuses.Accepted || job.JobStatusId == (byte)JobStatuses.InProgress)
                {
                    job.JobStatusId = byteDoneJobStatus;
                    AddJobStatus(job.Id, createdByUserID, null, byteDoneJobStatus);
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
        public async Task UpdateInProgressFromAccepted()
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.Accepted);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusInProgressAsync(job.Id, -1, job.VolunteerUserId.Value, CancellationToken.None);
                }
            }

        }

        public async Task UpdateJobsToDoneFromInProgress()
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.InProgress);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusDoneAsync(job.Id, -1, CancellationToken.None);
                }
            }
        }

        public async Task UpdateJobsToCancelledFromNewOrOpen()
        {
            List<EntityFramework.Entities.Job> jobs = new List<EntityFramework.Entities.Job>();
            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.New);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusCancelledAsync(job.Id, -1, CancellationToken.None);
                }
            }

            jobs = GetJobsWhereShiftStartDateHasPassed(JobStatuses.Open);
            jobs = GetJobsWhereShiftsHaveEnded(jobs);

            if (jobs != null)
            {
                foreach (EntityFramework.Entities.Job job in jobs)
                {
                    await UpdateJobStatusCancelledAsync(job.Id, -1, CancellationToken.None);
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

        public List<int> GetOverdueRepeatJobs()
        {
            List<int> response = new List<int>();
            byte jobStatusNew = (byte)JobStatuses.New;
            byte jobStatusOpen = (byte)JobStatuses.Open;
            byte dueDateTypeASAP = (byte)DueDateType.ASAP;

            DateTime now = DateTime.UtcNow;

            response = _context.Job
                .Include(x => x.NewRequest)
                .Where(x => x.NewRequest.Repeat == true 
                            && x.DueDate< now 
                            && (x.JobStatusId == jobStatusNew || x.JobStatusId == jobStatusOpen) 
                            && x.DueDateTypeId!= dueDateTypeASAP )
                .Select(x => x.Id)
                .ToList();

            return response;
        }
    }
}
