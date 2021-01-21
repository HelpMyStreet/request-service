using RequestService.Core.Interfaces.Repositories;
using System;

namespace RequestService.Core.Services
{
    public class ManageRequestsService : IManageRequestsService
    {
        private readonly IRepository _repository;

        public ManageRequestsService(IRepository repository)
        {
            _repository = repository;
        }
        
        public void ManageRequests()
        {
            _repository.UpdateInProgressFromAccepted();
            _repository.UpdateJobsToDoneFromInProgressOrAccepted();
            _repository.UpdateJobsToCancelledFromNewOrOpen();
        }
    }
}
