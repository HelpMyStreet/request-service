using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Domains;
using RequestService.Core.Domains.Entities;
using RequestService.Core.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        Task UpdateInProgressFromAccepted();
        Task UpdateJobsToDoneFromInProgress();
        Task UpdateJobsToCancelledFromNewOrOpen();
        Task<List<int>> UpdateRequestStatusToCancelledAsync(int requestId, int createdByUserID, CancellationToken cancellationToken);
        Task<List<int>> UpdateRequestStatusToDoneAsync(int requestId, int createdByUserID, CancellationToken cancellationToken);
        Task<bool> UpdateAllJobStatusToOpenForRequestAsync(int requestId, int createdByUserID, CancellationToken cancellationToken);
        List<RequestSummary> GetShiftRequestsByFilter(GetShiftRequestsByFilterRequest request, List<int> referringGroups);
        List<RequestSummary> GetRequestsByFilter(GetRequestsByFilterRequest request, List<int> referringGroups);        
        List<ShiftJob> GetOpenShiftJobsByFilter(GetOpenShiftJobsByFilterRequest request, List<int> referringGroups);
        List<ShiftJob> GetUserShiftJobsByFilter(GetUserShiftJobsByFilterRequest request);
        int UpdateRequestStatusToAccepted(int requestID, SupportActivities activity, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken);
        Task<int> VolunteerAlreadyAcceptedShift(int requestID, SupportActivities activity, int volunteerUserID, CancellationToken cancellationToken);
        Task<int> GetReferringGroupIDForJobAsync(int jobID, CancellationToken cancellationToken);
        Task<int> GetReferringGroupIDForRequestAsync(int requestID, CancellationToken cancellationToken);
        Task<List<int>> GetGroupsForJobAsync(int jobID, CancellationToken cancellationToken);
        Task<List<int>> GetGroupsForRequestAsync(int requestID, CancellationToken cancellationToken);
        Task AddJobAvailableToGroupAsync(int jobID, int groupID, CancellationToken cancellationToken);
        Task AddRequestAvailableToGroupAsync(int requestID, int groupID, CancellationToken cancellationToken);
        GetJobDetailsResponse GetJobDetails(int jobID);
        GetRequestDetailsResponse GetRequestDetails(int requestID);
        GetRequestSummaryResponse GetRequestSummary(int requestID);
        GetJobSummaryResponse GetJobSummary(int jobID);
        List<StatusHistory> GetJobStatusHistory(int jobID);
        List<JobSummary> GetJobsInProgressSummaries();
        List<JobSummary> GetJobsAllocatedToUser(int volunteerUserID);
        List<JobSummary> GetUserJobs(int volunteerUserID);
        Task<UpdateJobStatusOutcome> UpdateJobStatusAcceptedAsync(int jobID, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken);
        Task<UpdateJobStatusOutcome> UpdateJobStatusCancelledAsync(int jobID, int createdByUserID, CancellationToken cancellationToken);
        Task<UpdateJobStatusOutcome> UpdateJobStatusOpenAsync(int jobID, int createdByUserID, CancellationToken cancellationToken);
        Task<UpdateJobStatusOutcome> UpdateJobStatusInProgressAsync(int jobID, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken);
        Task<UpdateJobStatusOutcome> UpdateJobStatusDoneAsync(int jobID, int createdByUserID, CancellationToken cancellationToken);
        Task<UpdateJobStatusOutcome> UpdateJobStatusNewAsync(int jobID, int createdByUserID, CancellationToken cancellationToken);
        Task<UpdateJobOutcome> UpdateJobDueDateAsync(int jobID, int authorisedByUserID, DateTime dueDate, CancellationToken cancellationToken);
        Task<int> NewHelpRequestAsync(PostNewRequestForHelpRequest postNewRequestForHelpRequest, Fulfillable fulfillable, bool requestorDefinedByGroup, bool multiVolunteer, bool repeat);
        Task<int> GetRequestIDFromGuid(Guid guid);
        Task<int> NewShiftsRequestAsync(PostNewShiftsRequest PostNewShiftsRequest, Fulfillable fulfillable, RequestPersonalDetails requestorPersonalDetails );
        List<ReportItem> GetDailyReport();
        Task<int> CreateRequestAsync(string postCode, CancellationToken cancellationToken);
        Task UpdateFulfillmentAsync(int requestId, Fulfillable fulfillable, CancellationToken cancellationToken);
        Task AddSupportActivityAsync(SupportActivityDTO dto, CancellationToken cancellationToken);
        Task UpdatePersonalDetailsAsync(PersonalDetailsDto dto, CancellationToken cancellationToken);
        Task<string> GetRequestPostCodeAsync(int requestId, CancellationToken cancellationToken);
        Task UpdateCommunicationSentAsync(int requestId, bool communicationSent, CancellationToken cancellationToken);
        Task<List<LatitudeAndLongitudeDTO>> GetLatitudeAndLongitudes(List<string> postCodes, CancellationToken cancellationToken);
        List<JobSummary> GetJobsByStatusesSummaries(List<JobStatuses> jobStatuses);
        List<JobHeader> GetJobHeaders(GetJobsByFilterRequest request, List<int> referringGroups);
        List<JobDTO> GetAllFilteredJobs(GetAllJobsByFilterRequest request, List<int> referringGroups);
        void ArchiveOldRequests(int daysSinceJobRequested, int daysSinceJobStatusChanged);        
        bool JobHasStatus(int jobID, JobStatuses status);
        bool JobIsInProgressWithSameVolunteerUserId(int jobID, int? volunteerUserID);
        Task<List<Question>> GetQuestionsForActivity(SupportActivities activity, RequestHelpFormVariant requestHelpFormVariant, RequestHelpFormStage requestHelpFormStage, CancellationToken cancellationToken);
        List<RequestSummary> GetAllRequests(List<int> RequestIDs);
    }
}
