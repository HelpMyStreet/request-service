
using RequestService.Core.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpMyStreet.Contracts;
using RequestService.Core.Domains;
using HelpMyStreet.Utils.Extensions;
using System.Linq;
using HelpMyStreet.Utils.Models;

namespace RequestService.Handlers
{
    public class GetNewsTickerHandler : IRequestHandler<NewsTickerRequest, NewsTickerResponse>
    {
        private readonly IRepository _repository;

        public GetNewsTickerHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<NewsTickerResponse> Handle(NewsTickerRequest request, CancellationToken cancellationToken)
        {
            NewsTickerResponse response = new NewsTickerResponse()
            {
                Messages = new List<NewsTickerMessage>()
            };

            var completedActivities = await _repository.GetCompletedActivitiesCount(request.GroupId);
            var completedActivitiesToday = await _repository.GetActivitiesCompletedLastXDaysCount(request.GroupId, 1);
            var requestsAddedWithinLastWeek = await _repository.GetRequestsAddedLastXDaysCount(request.GroupId, 7);
            var openJobCount = await _repository.OpenJobCount(request.GroupId);

            foreach (SupportActivityCount item in completedActivities.Where(x=> x.Value>10).OrderByDescending(x => x.Value).Take(3))
            {
                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = item.Value,
                    SupportActivity = item.SupportActivity,
                    Message = $"**{ item.Value }** {item.SupportActivity.FriendlyNameShort()} { item.SupportActivity.RequestType().ToString() }  completed"
                });
            };

            var totalTasks = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Task)?.Sum(x => x.Value);
            var totalShifts = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Shift)?.Sum(x => x.Value);
            
            if(totalTasks>0)
            {
                var maxTaskCount =  completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Task)?.Max(x => x.Value);

                if (totalTasks > 20 && totalTasks > (maxTaskCount * 1.1))
                {
                    response.Messages.Add(new NewsTickerMessage()
                    {
                        Value = totalTasks,
                        Message = $"**{ totalTasks }** requests completed"
                    });
                }
            }

            if(totalShifts > 0)
            {
                var maxShiftCount = completedActivities.Where(x => x.SupportActivity.RequestType() == HelpMyStreet.Utils.Enums.RequestType.Shift)?.Max(x => x.Value);
                if (totalShifts > 20 && totalShifts > (maxShiftCount * 1.1))
                {
                    response.Messages.Add(new NewsTickerMessage()
                    {
                        Value = totalShifts,
                        Message = $"**{ totalShifts }** shifts completed"
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
                    Message = $"**{totalRequestsAddedThisWeek}** new requests added this week"
                });
            }

            if (openJobCount > 1)
            {
                response.Messages.Add(new NewsTickerMessage()
                {
                    Value = openJobCount,
                    Message = $"**{openJobCount}** open jobs waiting for a volunteer"
                });
            }

            return response;
        }
    }
}
