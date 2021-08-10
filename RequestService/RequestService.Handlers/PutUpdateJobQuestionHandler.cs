using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using System.Collections.Generic;
using System.Linq;

namespace RequestService.Handlers
{
    public class PutUpdateJobQuestionHandler : IRequestHandler<PutUpdateJobQuestionRequest, PutUpdateJobQuestionResponse>
    {
        private readonly IRepository _repository;
        private readonly IJobService _jobService;
        public PutUpdateJobQuestionHandler(IRepository repository, IJobService jobService)
        {
            _repository = repository;
            _jobService = jobService;
        }

        public async Task<PutUpdateJobQuestionResponse> Handle(PutUpdateJobQuestionRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobQuestionResponse response = new PutUpdateJobQuestionResponse()
            {
                Outcome = UpdateJobStatusOutcome.Unauthorized
            };

            bool hasPermission = await _jobService.HasPermissionToChangeJobAsync(request.JobID, request.AuthorisedByUserID, cancellationToken);

            if (hasPermission)
            {
                var jobDetails = _repository.GetJobDetails(request.JobID);

                if (jobDetails == null)
                {
                    return response;
                }

                if (jobDetails.JobSummary.JobStatus == JobStatuses.New || jobDetails.JobSummary.JobStatus == JobStatuses.Open)
                {
                    if (jobDetails.JobSummary.Questions.Count(x => x.Id == request.QuestionID) == 1)
                    {
                        var result = await _repository.UpdateJobQuestion(request.JobID, request.QuestionID, request.Answer, cancellationToken);
                        response.Outcome = result;
                    }
                }
            }
            
            return response;
        }
    }
}
