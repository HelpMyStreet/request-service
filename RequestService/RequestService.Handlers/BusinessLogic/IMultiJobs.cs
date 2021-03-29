using HelpMyStreet.Contracts.RequestService.Request;

namespace RequestService.Handlers.BusinessLogic
{
    public interface IMultiJobs
    {
        public bool AddMultiVolunteers(NewJobsRequest request);
        public bool AddRepeats(NewJobsRequest request);
    }
}
