using HelpMyStreet.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core
{
    public static class SupportActivityExtensions2
    {
        public static string Category(this SupportActivities activity)
        {
            return activity switch
            {
                //Befriending Purple
                SupportActivities.Shopping => "General", //light blue
                SupportActivities.FaceMask => "Specialist", // dark blue
                SupportActivities.CheckingIn => "Befriending",
                SupportActivities.CollectingPrescriptions => "General",
                SupportActivities.Errands => "General",
                SupportActivities.DogWalking => "Specialist",
                SupportActivities.MealPreparation => "Specialist",
                SupportActivities.PhoneCalls_Friendly => "Befriending",
                SupportActivities.PhoneCalls_Anxious => "Befriending",
                SupportActivities.HomeworkSupport => "Specialist",
                SupportActivities.WellbeingPackage => "General",
                SupportActivities.CommunityConnector => "Befriending",
                SupportActivities.ColdWeatherArmy => "General",
                SupportActivities.MealsToYourDoor => "General",
                SupportActivities.MealtimeCompanion => "Befriending",
                SupportActivities.EmergencySupport => "Specialist",
                SupportActivities.PracticalSupport => "General",
                SupportActivities.InPersonBefriending => "Befriending",
                SupportActivities.BankStaffVaccinator => "Specialist",
                _ => "Specialist",
            };
        }
    }
}
