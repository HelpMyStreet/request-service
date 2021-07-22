using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System.Collections.Generic;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Exceptions;
using System.Net.Http;
using System.Linq;
using RequestService.Core.Dto;
using HelpMyStreet.Contracts.AddressService.Request;
using System;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.EqualityComparers;

namespace RequestService.Handlers
{
    public class GetAllRequestsHandler : IRequestHandler<GetAllRequestsRequest, GetAllRequestsResponse>
    {
        private readonly IRepository _repository;
        
        public GetAllRequestsHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAllRequestsResponse> Handle(GetAllRequestsRequest request, CancellationToken cancellationToken)
        {
            return new GetAllRequestsResponse()
            {
                RequestSummaries = _repository.GetAllRequests(request.RequestIDs)
            };
        }

    }
}
