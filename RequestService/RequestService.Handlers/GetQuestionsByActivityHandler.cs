using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Core.Dto;
using RequestService.Core.Services;
using System.Collections.Generic;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Models;
using System.Linq;
using System.Collections;
using Microsoft.Net.Http.Headers;
using HelpMyStreet.Utils.Enums;
using System;
using HelpMyStreet.Contracts.AddressService.Request;
using Newtonsoft.Json;

namespace RequestService.Handlers
{
    public class GetQuestionsByActivityHandler : IRequestHandler<GetQuestionsByActivitiesRequest, GetQuestionsByActivtiesResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;
        private readonly IAddressService _addressService;
        public GetQuestionsByActivityHandler(
            IRepository repository, IGroupService groupService, IAddressService addressService)
        {
            _repository = repository;
            _groupService = groupService;
            _addressService = addressService;
        }

        private async Task<List<AdditonalQuestionData>> GetAdditonalQuestionDataForGroupLocationSource(int? groupId)
        {
            List<AdditonalQuestionData> data = new List<AdditonalQuestionData>();
            if (groupId.HasValue)
            {
                var groupLocations = await _groupService.GetGroupLocations(groupId.Value);
                if (groupLocations.Locations.Count() > 0)
                {
                    GetLocationsRequest locationRequest = new GetLocationsRequest()
                    {
                        LocationsRequests = new HelpMyStreet.Contracts.AddressService.Request.LocationsRequest()
                        {
                            Locations = groupLocations.Locations.ToList()
                        }
                    };
                    var details = await _addressService.GetLocations(locationRequest);
                    if (details != null)
                    {
                        data = details.LocationDetails.Select(x => new AdditonalQuestionData() { Key = ((int) x.Location).ToString(), Value = x.ShortName }).ToList();
                    }
                }
            }
            return data;
        }

        public async Task<GetQuestionsByActivtiesResponse> Handle(GetQuestionsByActivitiesRequest request, CancellationToken cancellationToken)
        {
            if(request.ActivitesRequest.Activities.Count!=1)
            {
                throw new System.Exception("Expecting only one activity");
            }
            
            var selectedActivity = request.ActivitesRequest.Activities.First();

            List<Question> questions = await _repository.GetQuestionsForActivity(selectedActivity, request.RequestHelpFormVariantRequest.RequestHelpFormVariant, request.RequestHelpFormStageRequest.RequestHelpFormStage, cancellationToken);

            var additonalSourceQuestions = questions.Where(x => x.AdditionalDataSource.HasValue).ToList();

            foreach(Question q in additonalSourceQuestions)
            {
                switch(q.AdditionalDataSource.Value)
                {
                    case AdditionalDataSource.GroupLocation:
                        q.AddtitonalData = await GetAdditonalQuestionDataForGroupLocationSource(request.GroupId);     
                        break;
                    default:
                        throw new Exception($"Unknown source {q.AdditionalDataSource.Value}");
                }
            }

            GetQuestionsByActivtiesResponse response = new GetQuestionsByActivtiesResponse();

            Dictionary<HelpMyStreet.Utils.Enums.SupportActivities, List<Question>> dict = new Dictionary<HelpMyStreet.Utils.Enums.SupportActivities, List<Question>>
            {
                { selectedActivity, questions }
            };

            response.SupportActivityQuestions = dict;

            return response;
        }
    }
}
