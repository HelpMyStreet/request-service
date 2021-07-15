using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Services;
using System.Collections.Generic;
using System.Linq;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.CommunicationService.Request;
using Microsoft.Extensions.Options;
using RequestService.Core.Config;
using HelpMyStreet.Contracts.RequestService.Response;
using RequestService.Core.Dto;
using Newtonsoft.Json;
using System;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Exceptions;
using HelpMyStreet.Utils.Extensions;
using RequestService.Handlers.BusinessLogic;

namespace RequestService.Handlers
{
    public class PostRequestForHelpHandler : IRequestHandler<PostRequestForHelpRequest, PostRequestForHelpResponse>
    {
        private readonly IRepository _repository;
        private readonly ICommunicationService _communicationService;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IGroupService _groupService;
        private readonly IOptionsSnapshot<ApplicationConfig> _applicationConfig;
        private readonly IMultiJobs _multiJobs;
        public PostRequestForHelpHandler(
            IRepository repository,
            IUserService userService,
            IAddressService addressService,
            ICommunicationService communicationService,
            IGroupService groupService,
            IOptionsSnapshot<ApplicationConfig> applicationConfig,
            IMultiJobs multiJobs)
        {
            _repository = repository;
            _userService = userService;
            _addressService = addressService;
            _communicationService = communicationService;
            _groupService = groupService;
            _applicationConfig = applicationConfig;
            _multiJobs = multiJobs;
        }

        public async Task<PostRequestForHelpResponse> Handle(PostRequestForHelpRequest request, CancellationToken cancellationToken)
        {
            var firstHelpRequestDetail = request.HelpRequestDetails.First();
            var firstJob = firstHelpRequestDetail.NewJobsRequest.Jobs.First();
            var requestType = firstJob.SupportActivity.RequestType();

            if (requestType == RequestType.Shift && 
                firstJob.NumberOfRepeats>1 && 
                firstJob.RepeatFrequency != Frequency.Once)
            {
                //Need to create multiple shifts
            }



            return new PostRequestForHelpResponse()
            {
                RequestIDs = new List<int>(),
                Fulfillable = Fulfillable.Accepted_ManualReferral
            };
        }
    }
}
   