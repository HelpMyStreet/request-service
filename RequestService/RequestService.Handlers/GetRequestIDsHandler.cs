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
    public class GetRequestIDsHandler : IRequestHandler<GetRequestIDsRequest, GetRequestIDsResponse>
    {
        private readonly IRepository _repository;
        
        public GetRequestIDsHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetRequestIDsResponse> Handle(GetRequestIDsRequest request, CancellationToken cancellationToken)
        {
            return new GetRequestIDsResponse()
            {
                JobIDsToRequestIDs = await _repository.GetAllRequestIDs(request.JobIDs)
            };
        }

    }
}
