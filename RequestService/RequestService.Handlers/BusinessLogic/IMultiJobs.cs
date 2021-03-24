using HelpMyStreet.Contracts.RequestService.Request;

namespace RequestService.Handlers.BusinessLogic
{
    public interface IMultiJobs
    {
        public void AddMultiVolunteers(NewJobsRequest request);
        public void AddRepeats(NewJobsRequest request);
    }
}
