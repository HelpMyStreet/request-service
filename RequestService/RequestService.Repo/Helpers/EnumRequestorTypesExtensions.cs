using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumRequestorTypesExtensions
    {
        public static void SetEnumRequestorTypeData(this EntityTypeBuilder<EnumRequestorTypes> entity)        
        {
            var requestorTypes = Enum.GetValues(typeof(RequestorType)).Cast<RequestorType>();

            foreach (var requestorType in requestorTypes)
            {
                entity.HasData(new EnumRequestorTypes { Id = (int)requestorType, Name = requestorType.ToString() });
            }
        }
    }
}
