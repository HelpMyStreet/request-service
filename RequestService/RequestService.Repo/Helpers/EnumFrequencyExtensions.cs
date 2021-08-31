
using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumFrequencyExtensions
    {
        public static void SetEnumFrequencyData(this EntityTypeBuilder<EnumFrequency> entity)        
        {
            var frequencies = Enum.GetValues(typeof(Frequency)).Cast<Frequency>();

            foreach (var frequency in frequencies)
            {
                entity.HasData(new EnumFrequency { Id = (int)frequency, Name = frequency.ToString() });
            }
        }
    }
}
