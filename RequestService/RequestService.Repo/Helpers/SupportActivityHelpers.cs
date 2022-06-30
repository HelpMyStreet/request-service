using HelpMyStreet.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.Helpers
{
    public static class SupportActivityHelpers
    {
        public static bool ShowSelectActivityQuestion(this SupportActivities activity)
        {
            return activity switch
            {
                SupportActivities.Other => true,
                SupportActivities.AdvertisingRoles => true,
                _ => false
            };
        }

        public static bool MultiVolunteerQuestion(this SupportActivities activity)
        {
            return activity switch
            {
                SupportActivities.VolunteerSupport => true,
                SupportActivities.EmergencySupport => true,
                SupportActivities.AdvertisingRoles => true,
                _ => false
            };
        }
    }
}
