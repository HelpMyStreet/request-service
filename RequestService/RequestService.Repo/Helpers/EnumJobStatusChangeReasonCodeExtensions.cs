using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Linq;

namespace RequestService.Repo.Helpers
{
    public static class EnumJobStatusChangeReasonCodeExtensions
    {
        public static void SetEnumJobStatusChangeReasonCodeData(this EntityTypeBuilder<EnumJobStatusChangeReasonCodes> entity)        
        {
            var reasonCodes = Enum.GetValues(typeof(JobStatusChangeReasonCodes)).Cast<JobStatusChangeReasonCodes>();

            foreach (var reasonCode in reasonCodes)
            {
                entity.HasData(new EnumJobStatusChangeReasonCodes { Id = (int)reasonCode, Name = reasonCode.ToString(), TriggersStatusChangeEmail = reasonCode.TriggersStatusChangeEmail() });
            }
        }
    }
}
