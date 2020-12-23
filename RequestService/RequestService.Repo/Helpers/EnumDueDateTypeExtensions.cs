using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumDueDateTypeExtensions
    {
        public static void SetEnumDueDateTypeData(this EntityTypeBuilder<EnumDueDateTypes> entity)        
        {
            var dueDateTypes = Enum.GetValues(typeof(DueDateType)).Cast<DueDateType>();

            foreach (var dueDateType in dueDateTypes)
            {
                entity.HasData(new EnumDueDateTypes { Id = (int)dueDateType, Name = dueDateType.ToString() });
            }
        }
    }
}
