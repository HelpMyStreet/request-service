
using RequestService.Core.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpMyStreet.Contracts;
using RequestService.Core.Domains;
using HelpMyStreet.Utils.Extensions;
using System.Linq;
using System;
using HelpMyStreet.Utils.Models;
using RequestService.Core.Services;

namespace RequestService.Handlers
{
    public class GetNewsTickerHandler : IRequestHandler<NewsTickerRequest, NewsTickerResponse>
    {
        private readonly IRepository _repository;
        private readonly IGroupService _groupService;

        public GetNewsTickerHandler(IRepository repository, IGroupService groupService)
        {
            _repository = repository;
            _groupService = groupService;
        }

        public async Task<NewsTickerResponse> Handle(NewsTickerRequest request, CancellationToken cancellationToken)
        {
            NewsTickerResponse response = new NewsTickerResponse()
            {
                Messages = new List<NewsTickerMessage>()
            };

            List<int> groups = new List<int>();

            if(request.GroupId.HasValue)
            {
                groups.Add(request.GroupId.Value);
                var childGroups = await _groupService.GetChildGroups(request.GroupId.Value);
                groups.AddRange(childGroups.ChildGroups.Select(sm => sm.GroupId));
            }

            var completedActivities = await _repository.GetCompletedActivitiesCount(groups);
            var completedActivitiesToday = await _repository.GetActivitiesCompletedLastXDaysCount(groups, 1);
            var requestsAddedWithinLastWeek = await _repository.GetRequestsAddedLastXDaysCount(groups, 7);
            var openJobCount = await _repository.OpenJobCount(groups);

            foreach (SupportActivityCount item in completedActivities.Where(x=> x.Value>10).OrderByDescending(x => x.Value).Take(3))
            {
                string strRequestType = item.SupportActivity.RequestType().FriendlyName(Convert.ToInt32(item.Value));
                string strSupportActivity = item.SupportActivity.FriendlyNameShort().ToLower();

                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = item.Value,
                    SupportActivity = item.SupportActivity,
                    Message = $"**{item.Value:n0} {strSupportActivity}** {strRequestType} completed"
                });
            };

            var totalRequests = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Task)?.Sum(x => x.Value);
            var totalShifts = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Shift)?.Sum(x => x.Value);
            
            if(totalRequests>0)
            {
                var maxTaskCount =  completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Task)?.Max(x => x.Value);

                if (totalRequests > 20 && totalRequests > (maxTaskCount * 1.1))
                {
                    string strRequestType = HelpMyStreet.Utils.Enums.RequestType.Task.FriendlyName(Convert.ToInt32(totalRequests));
                    response.Messages.Add(new NewsTickerMessage()
                    {
                        Value = totalRequests,
                        Message = $"**{totalRequests:n0}** {strRequestType} completed"
                    });
                }
            }

            if(totalShifts > 0)
            {
                var maxShiftCount = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Shift)?.Max(x => x.Value);
                if (totalShifts > 20 && totalShifts > (maxShiftCount * 1.1))
                {
                    string strRequestType = HelpMyStreet.Utils.Enums.RequestType.Shift.FriendlyName(Convert.ToInt32(totalShifts));
                    response.Messages.Add(new NewsTickerMessage()
                    {
                        Value = totalShifts,
                        Message = $"**{totalShifts:n0}** {strRequestType} completed"
                    });
                }
            }

            foreach (SupportActivityCount item in completedActivitiesToday)
            {
                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = item.Value,
                    SupportActivity = item.SupportActivity,
                    Message = $"Completed today: **{ item.SupportActivity.FriendlyNameShort() }**"
                });
            };

            var totalRequestsAddedThisWeek = requestsAddedWithinLastWeek.Sum(x => x.Value);

            if (totalRequestsAddedThisWeek >1)
            {
                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = totalRequestsAddedThisWeek,
                    Message = $"**{totalRequestsAddedThisWeek:n0}** new requests added this week"
                });
            }

            if (openJobCount > 1)
            {
                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = openJobCount,
                    Message = $"**{openJobCount:n0}** open jobs waiting for a volunteer"
                });
            }

            return response;
        }
    }
}
