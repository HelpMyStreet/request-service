using HelpMyStreet.Contracts.RequestService.Request;
using System;
using System.Collections.Generic;

namespace RequestService.Handlers.BusinessLogic
{
    public interface IMultiJobs
    {
        public bool AddMultiVolunteers(NewJobsRequest request);
        public bool AddRepeats(NewJobsRequest request, DateTime startDateTime);
        public void AddShiftRepeats(List<HelpRequestDetail> helpRequestDetails, int repeatCount);

    }
}
