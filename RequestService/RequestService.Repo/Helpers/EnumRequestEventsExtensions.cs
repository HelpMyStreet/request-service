
using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumRequestEventsExtensions
    {
        public static void SetEnumRequestEventsData(this EntityTypeBuilder<EnumRequestEvents> entity)        
        {
            var requestEvents = Enum.GetValues(typeof(RequestEvent)).Cast<RequestEvent>();

            foreach (var requestEvent in requestEvents)
            {
                entity.HasData(new EnumRequestEvents { Id = (int)requestEvent, Name = requestEvent.ToString() });
            }
        }
    }
}
