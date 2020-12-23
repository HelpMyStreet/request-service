using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumRequestTypeExtensions
    {
        public static void SetEnumRequestTypeData(this EntityTypeBuilder<EnumRequestTypes> entity)        
        {
            var requestTypes = Enum.GetValues(typeof(RequestType)).Cast<RequestType>();

            foreach (var requestType in requestTypes)
            {
                entity.HasData(new EnumRequestTypes { Id = (int)requestType, Name = requestType.ToString() });
            }
        }
    }
}
