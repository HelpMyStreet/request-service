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
        private readonly ICommunicationService _communicationService;
        public PutUpdateJobQuestionHandler(IRepository repository, IJobService jobService, ICommunicationService communicationService)
        {
            _repository = repository;
            _jobService = jobService;
            _communicationService = communicationService;
        }

        public async Task<PutUpdateJobQuestionResponse> Handle(PutUpdateJobQuestionRequest request, CancellationToken cancellationToken)
        {
            PutUpdateJobQuestionResponse response = new PutUpdateJobQuestionResponse()
            {
                Outcome = UpdateJobOutcome.Unauthorized
            };

            bool hasPermission = await _jobService.HasPermissionToChangeJobAsync(request.JobID, request.AuthorisedByUserID, cancellationToken);

            if (hasPermission)
            {
                var jobDetails = _repository.GetJobDetails(request.JobID);

                if (jobDetails == null)
                {
                    return response;
                }

                if (jobDetails.JobSummary.JobStatus != JobStatuses.Cancelled && jobDetails.JobSummary.JobStatus != JobStatuses.Done)
                {
                    if (jobDetails.JobSummary.Questions.Count(x => x.Id == request.QuestionID) == 1)
                    {
                        var fieldChanged = ((Questions)request.QuestionID).ToString();
                        var oldAnswer = jobDetails.JobSummary.Questions.First(x => x.Id == request.QuestionID).Answer;
                        var result = await _repository.UpdateJobQuestion(request.JobID, request.QuestionID, request.Answer, cancellationToken);

                        if(result == UpdateJobOutcome.Success)
                        {
                            await _repository.UpdateHistory(
                                requestId: jobDetails.JobSummary.RequestID,
                                createdByUserId: request.AuthorisedByUserID,
                                fieldChanged: ((Questions)request.QuestionID).ToString(),
                                oldValue: oldAnswer,
                                newValue: request.Answer,
                                questionId: request.QuestionID,
                                jobId: request.JobID
                                );

                            await _communicationService.RequestCommunication(
                            new RequestCommunicationRequest()
                            {
                                CommunicationJob = new CommunicationJob() { CommunicationJobType = CommunicationJobTypes.SendTaskStateChangeUpdate },
                                JobID = request.JobID,
                                AdditionalParameters = new Dictionary<string, string>()
                                {
                                    { "FieldUpdated",fieldChanged },
                                    { "OldValue",oldAnswer },
                                    { "NewValue",request.Answer }
                                }
                            },
                            cancellationToken);
                        }

                        response.Outcome = result;
                    }
                }
            }
            
            return response;
        }
    }
}
