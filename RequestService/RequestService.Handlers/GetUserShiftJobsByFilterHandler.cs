﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.RequestService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Services;
using HelpMyStreet.Utils.Models;
using System;

namespace RequestService.Handlers
{
    public class GetUserShiftJobsByFilterHandler : IRequestHandler<GetUserShiftJobsByFilterRequest, GetUserShiftJobsByFilterResponse>
    {
        private readonly IRepository _repository;
        public GetUserShiftJobsByFilterHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUserShiftJobsByFilterResponse> Handle(GetUserShiftJobsByFilterRequest request, CancellationToken cancellationToken)
        {
            GetUserShiftJobsByFilterResponse response = null;

            var shiftjobs = _repository.GetUserShiftJobsByFilter(request);

            if (shiftjobs != null)
            {
                response = new GetUserShiftJobsByFilterResponse()
                {
                    ShiftJobs = shiftjobs
                };
            }
            else
            {
                throw new Exception("Get User shift should never return a null. In the case where no shifts are relevant to the filter, an empty list should be returned");
            }
            return response;
        }
    }
}
