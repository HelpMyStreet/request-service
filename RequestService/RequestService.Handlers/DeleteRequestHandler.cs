using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using System.Linq;
using System;

namespace RequestService.Handlers
{
    public class DeleteRequestHandler : IRequestHandler<DeleteRequestRequest, DeleteRequestResponse>
    {
        private readonly IRepository _repository;
        public DeleteRequestHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteRequestResponse> Handle(DeleteRequestRequest request, CancellationToken cancellationToken)
        {
            bool success = false;

            var req = _repository.GetRequestDetails(request.RequestID);

            if(req !=null)
            {
                if(req.RequestSummary.PostCode== request.Postcode)
                {
                    success = await _repository.DeleteRequest(request.RequestID,cancellationToken);
                }    
            }

            return new DeleteRequestResponse() { Success = success };
        }
    }
}
