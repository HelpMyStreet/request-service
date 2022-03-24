
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using HelpMyStreet.Utils.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Question = RequestService.Repo.EntityFramework.Entities.Question;
using SupportActivities = HelpMyStreet.Utils.Enums.SupportActivities;

namespace RequestService.Repo.Helpers
{
    public static class QuestionExtenstions
    {
        public static void SetQuestionData(this EntityTypeBuilder<Question> entity)
        {
            entity.HasData(new Question
            {
                Id = (int)Questions.SupportRequesting,
                Name = "Please tell us more about the help or support you're requesting",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = GetAdditionalData(Questions.SupportRequesting),
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.FaceMask_SpecificRequirements,
                Name = "Please tell us about any specific requirements (e.g. size, colour, style etc.)",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = GetAdditionalData(Questions.FaceMask_SpecificRequirements),
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.FaceMask_Amount,
                Name = "How many face coverings do you need?",
                QuestionType = (int)QuestionType.Number,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Amount),
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.FaceMask_Recipient,
                Name = "Who will be using the face coverings?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Recipient),
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.FaceMask_Cost,
                Name = "Are you able to pay the cost of materials for your face covering (usually £2 - £3 each)?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Cost),
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.IsHealthCritical,
                Name = "Is this request critical to someone's health or wellbeing?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.IsHealthCritical),
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.WillYouCompleteYourself,
                Name = "Will you complete this request yourself?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.WillYouCompleteYourself),
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.FtlosDonationInformation,
                Name = "Please donate to the For the Love of Scrubs GoFundMe <a href=\"https://www.gofundme.com/f/for-the-love-of-scrubs-face-coverings\" target=\"_blank\">here</a> to help pay for materials and to help us continue our good work. Recommended donation £3 - £4 per face covering.",
                QuestionType = (int)QuestionType.LabelOnly,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.CommunicationNeeds,
                Name = "Are there any communication needs that volunteers need to know about before they contact you or the person who needs help?",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.AnythingElseToTellUs,
                Name = "Is there anything else you would like to tell us about the request?",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.AgeUKReference,
                Name = "AgeUK Reference",
                QuestionType = (int)QuestionType.Text,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.Shopping_List,
                Name = "Please tell us what you need from the shop",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });
            entity.HasData(new Question
            {
                Id = (int)Questions.Prescription_PharmacyAddress,
                Name = "Where does the prescription need collecting from?",
                QuestionType = (int)QuestionType.Text,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.SensitiveInformation,
                Name = "Is there any other personal or sensitive information the volunteer needs to know to complete the request?",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = true
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.SpecialDietaryRequirements,
                Name = "Are there any special dietary requirements?",
                QuestionType = (int)QuestionType.MultiLineText,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.NumberOfSlots,
                Name = "How many volunteers are required?",
                QuestionType = (int)QuestionType.Number,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.Location,
                Name = "Where is the help needed?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false,
                AdditionalDataSource = (byte) AdditionalDataSource.GroupLocation
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.SuppressRecipientPersonalDetails,
                Name = "Would you like to hide the name and contact details of the person in need of help from prospective volunteers?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.SuppressRecipientPersonalDetails),
                AnswerContainsSensitiveData = false,
            });
          
          entity.HasData(new Question
            {
                Id = (int)Questions.RecipientAge,
                Name = "Age of the person needing help",
                QuestionType = (int)QuestionType.Text,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = true
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.GroupSizeAdults,
                Name = "How many adults need accommodation?",
                QuestionType = (int)QuestionType.Number,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.GroupSizeChildren,
                Name = "How many children need accommodation?",
                QuestionType = (int)QuestionType.Number,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.GroupSizePets,
                Name = "How many pets need accommodation?",
                QuestionType = (int)QuestionType.Number,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.PreferredLocation,
                Name = "Do you have a preferred location within the UK?",
                QuestionType = (int)QuestionType.Radio,
                AdditionalData = GetAdditionalData(Questions.PreferredLocation),
                AnswerContainsSensitiveData = false
            });

            entity.HasData(new Question
            {
                Id = (int)Questions.PreferredLanguage,
                Name = "What languages do you speak?",
                QuestionType = (int)QuestionType.Text,
                AdditionalData = string.Empty,
                AnswerContainsSensitiveData = false
            });
        }
        private static string GetAdditionalData(Questions question)
        {
            List<AdditonalQuestionData> additionalData = new List<AdditonalQuestionData>();
            switch (question)
            {
                case Questions.FaceMask_Recipient:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "keyworkers",
                            Value = "Key workers"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "somonekeyworkers",
                            Value = "Someone helping key workers stay safe in their role (e.g. care home residents, visitors etc.)"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "memberspublic",
                            Value = "Members of the public"
                        },
                    };
                    break;
                    
                case Questions.SuppressRecipientPersonalDetails:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "Yes",
                            Value = "Yes, hide the personal details"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "No",
                            Value = "No, make the personal details visible to volunteers who accept the request"
                        },
                    };
                    break;
                    
                case Questions.FaceMask_Cost:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "Yes",
                            Value = "Yes"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "No",
                            Value = "No"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "Contribution",
                            Value = "I can make a contribution"
                        },
                    };
                    break;
                case Questions.IsHealthCritical:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "true",
                            Value = "Yes"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "false",
                            Value = "No"
                        }
                    };
                    break;
                case Questions.WillYouCompleteYourself:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "true",
                            Value = "Yes"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "false",
                            Value = "No, please make it visible to other volunteers"
                        }
                    };
                    break;
                case Questions.PreferredLocation:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData { Key = "BB1 1AE", Value = "No preference"},
                        new AdditonalQuestionData { Key = "DH1 1AB", Value = "England, North East (inc. Newcastle, Sunderland, Gateshead)"},
                        new AdditonalQuestionData { Key = "M1 1AD", Value = "England, North West (inc. Liverpool, Manchester, Bolton)"},
                        new AdditonalQuestionData { Key = "YO1 0ET", Value = "England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)"},
                        new AdditonalQuestionData { Key = "NG1 6DQ", Value = "England, East Midlands (inc. Leicester, Nottingham, Derby)"},
                        new AdditonalQuestionData { Key = "B1 1QU", Value = "England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)"},
                        new AdditonalQuestionData { Key = "CB8 0AA", Value = "England, East of England (inc. Luton, Norwich, Southend-on-Sea)"},
                        new AdditonalQuestionData { Key = "SW1A 1AA", Value = "England, London"},
                        new AdditonalQuestionData { Key = "RH10 0AG", Value = "England, South East (inc. Southampton, Portsmouth, Brighton)"},
                        new AdditonalQuestionData { Key = "BA1 0AA", Value = "England, South West (inc. Bristol, Plymouth, Bournemouth)"},
                        new AdditonalQuestionData { Key = "BT1 1AA", Value = "Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)"},
                        new AdditonalQuestionData { Key = "PH1 1AA", Value = "Scotland (inc. Glasgow, Edinburgh, Aberdeen)"},
                        new AdditonalQuestionData { Key = "SY23 1AB", Value = "Wales (inc. Cardiff, Swansea, Newport)"}
                    };
                    break;
            }

            return JsonConvert.SerializeObject(additionalData);
        }
        public static void SetActivityQuestionData(this EntityTypeBuilder<ActivityQuestions> entity)
        {
            var requestFormVariants = Enum.GetValues(typeof(RequestHelpFormVariant)).Cast<RequestHelpFormVariant>();
            string subText_anythingElse = "This information will be visible to volunteers deciding whether to accept the request";

            foreach (var form in requestFormVariants)
            {
                foreach (var activity in GetSupportActivitiesForRequestFormVariant(form))
                {
                    if (activity == SupportActivities.VaccineSupport || activity == SupportActivities.BankStaffVaccinator)
                    {
                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.NumberOfSlots,
                            Location = "pos3",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.Location,
                            Location = "pos2",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });
                    }
                    else if (activity == SupportActivities.FaceMask)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.FaceMask_SpecificRequirements, Location = "pos2", Order = 2, RequestFormVariantId = (int)form, Required = false, PlaceholderText = "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", Subtext = "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" });
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.FaceMask_Amount, Location = "pos3", Order = 1, RequestFormVariantId = (int)form, Required = true, Subtext = "Remember they’re washable and reusable, so only request what you need between washes." });
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.FaceMask_Recipient, Location = "pos3", Order = 3, RequestFormVariantId = (int)form, Required = false });

                        if (form != RequestHelpFormVariant.FtLOS)
                        {
                            entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.FaceMask_Cost, Location = "pos3", Order = 4, RequestFormVariantId = (int)form, Required = false, Subtext = "Volunteers are providing their time and skills free of charge." });
                        }
                        else
                        {
                            entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.FtlosDonationInformation, Location = "pos3", Order = 4, RequestFormVariantId = (int)form, Required = false });
                        }
                    }
                    else if (activity == SupportActivities.ColdWeatherArmy)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.SupportRequesting, Location = "pos1", Order = 1, RequestFormVariantId = (int)form, Required = false, PlaceholderText = "Please be aware that information in this section is visible to prospective volunteers" });
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.AnythingElseToTellUs, Location = "details2", Order = 2, RequestFormVariantId = (int)form, Required = false, PlaceholderText = "For example, any special instructions for the volunteer.", Subtext = subText_anythingElse });
                    }
                    else if (activity == SupportActivities.Shopping)
                    {
                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.Shopping_List,
                            Location = "pos1",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.",
                            Subtext = "Make sure to include the size, brand, and any other important details"
                        });

                        string anythingElseToTellUs_placeholderText = form switch
                        {
                            RequestHelpFormVariant.Ruddington => "For example, let us know if you’re struggling to find help elsewhere.",
                            _ => "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box."
                        };
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.AnythingElseToTellUs, Location = "details2", Order = 2, RequestFormVariantId = (int)form, Required = false, PlaceholderText = anythingElseToTellUs_placeholderText, Subtext = subText_anythingElse });
                    }
                    else if (activity == SupportActivities.CollectingPrescriptions)
                    {
                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.Prescription_PharmacyAddress,
                            Location = "pos1",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham."
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Detail,
                            QuestionId = (int)Questions.AnythingElseToTellUs,
                            Location = "details2",
                            Order = 2,
                            RequestFormVariantId = (int)form,
                            Required = false,
                            PlaceholderText = "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.",
                            Subtext = subText_anythingElse
                        });
                    }
                    else if (activity == SupportActivities.MealsToYourDoor || activity == SupportActivities.MealtimeCompanion)
                    {
                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.SpecialDietaryRequirements,
                            Location = "pos2",
                            Order = 2,
                            RequestFormVariantId = (int)form,
                            Required = false,
                            PlaceholderText = "e.g. vegetarian, vegan, food intolerances, smaller portion size etc.",
                            Subtext = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Detail,
                            QuestionId = (int)Questions.AnythingElseToTellUs,
                            Location = "details2",
                            Order = 2,
                            RequestFormVariantId = (int)form,
                            Required = false,
                            PlaceholderText = "For example, any special instructions for the volunteer.",
                            Subtext = subText_anythingElse
                        });
                    }
                    else if (activity == SupportActivities.VolunteerSupport || activity == SupportActivities.EmergencySupport)
                    {
                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.NumberOfSlots,
                            Location = "pos1",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        { 
                            ActivityId = (int)activity, 
                            RequestFormStageId = (int)RequestHelpFormStage.Request, 
                            QuestionId = (int)Questions.SupportRequesting, 
                            Location = "pos1", 
                            Order = 2, 
                            RequestFormVariantId = (int)form, 
                            Required = false, 
                            PlaceholderText = "Please don’t include any sensitive details that aren’t needed in order for us to help you" 
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Detail,
                            QuestionId = (int)Questions.AnythingElseToTellUs,
                            Location = "details2",
                            Order = 2,
                            RequestFormVariantId = (int)form,
                            Required = false,
                            PlaceholderText = "For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.",
                            Subtext = subText_anythingElse
                        });

                    }
                    else if (activity == SupportActivities.Accommodation)
                    {
                        entity.HasData(new ActivityQuestions 
                        {  
                            ActivityId = (int)activity, 
                            RequestFormStageId = (int)RequestHelpFormStage.Request, 
                            QuestionId = (int)Questions.SupportRequesting, 
                            Location = "pos3", 
                            Order = 1, 
                            RequestFormVariantId = (int)form, 
                            Required = false, 
                            PlaceholderText = "Please be aware that information in this section is visible to prospective hosts",
                            Subtext = "We will show this information to potential hosts to help find the best match",
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.GroupSizeAdults,
                            Location = "pos1",
                            Order = 1,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.GroupSizeChildren,
                            Location = "pos1",
                            Order = 2,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.GroupSizePets,
                            Location = "pos1",
                            Order = 3,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.PreferredLocation,
                            Location = "pos1",
                            Order = 4,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });

                        entity.HasData(new ActivityQuestions
                        {
                            ActivityId = (int)activity,
                            RequestFormStageId = (int)RequestHelpFormStage.Request,
                            QuestionId = (int)Questions.PreferredLanguage,
                            Location = "pos2",
                            Order = 5,
                            RequestFormVariantId = (int)form,
                            Required = true,
                            PlaceholderText = string.Empty
                        });
                    }
                    else
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.SupportRequesting, Location = "pos1", Order = 1, RequestFormVariantId = (int)form, Required = false, PlaceholderText = "Please don’t include any sensitive details that aren’t needed in order for us to help you" });

                        string anythingElseToTellUs_placeholderText = form switch
                        {
                            RequestHelpFormVariant.Ruddington => "For example, let us know if you’re struggling to find help elsewhere.",
                            _ => "For example, any special instructions for the volunteer."
                        };
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.AnythingElseToTellUs, Location = "details2", Order = 2, RequestFormVariantId = (int)form, Required = false, PlaceholderText = anythingElseToTellUs_placeholderText, Subtext = subText_anythingElse });
                    }

                    if (form == RequestHelpFormVariant.VitalsForVeterans)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.AgeUKReference, Location = "pos1", Order = 2, RequestFormVariantId = (int)form, Required = false });
                    }

                    if ((form == RequestHelpFormVariant.Default || form == RequestHelpFormVariant.FaceMasks
                        || form == RequestHelpFormVariant.AgeConnectsCardiff_Public || form == RequestHelpFormVariant.AgeConnectsCardiff_RequestSubmitter
                        || form == RequestHelpFormVariant.AgeUKNottsBalderton || form == RequestHelpFormVariant.AgeUKNottsNorthMuskham
                        || form == RequestHelpFormVariant.Soutwell_Public || form == RequestHelpFormVariant.MeadowsCommunityHelpers_Public || form == RequestHelpFormVariant.MeadowsCommunityHelpers_RequestSubmitter) && activity != SupportActivities.FaceMask)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.IsHealthCritical, Location = "pos3", Order = 2, RequestFormVariantId = (int)form, Required = true });
                    }

                    if (form == RequestHelpFormVariant.DIY)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Request, QuestionId = (int)Questions.WillYouCompleteYourself, Location = "pos3", Order = 3, RequestFormVariantId = (int)form, Required = true });
                    }

                    if (!form.IsPublic() && form!=RequestHelpFormVariant.BostonGNS_RequestSubmitter)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.SuppressRecipientPersonalDetails, Location = "details1", Order = 1, Subtext = "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request.", RequestFormVariantId = (int)form, Required = true });
                    }

                    if (form == RequestHelpFormVariant.AgeConnectsCardiff_Public || form == RequestHelpFormVariant.AgeConnectsCardiff_RequestSubmitter)
                    {
                        entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.RecipientAge, Location = "details1", Order = 2, RequestFormVariantId = (int)form, Required = false,Subtext = "We use age to check which services we are able to provide. You can put an approximate age if you prefer." });
                    }

                    entity.HasData(new ActivityQuestions { ActivityId = (int)activity, RequestFormStageId = (int)RequestHelpFormStage.Detail, QuestionId = (int)Questions.SensitiveInformation, Location = "details2", Order = 3, RequestFormVariantId = (int)form, Required = false, PlaceholderText = "For example, a door entry code, or contact details for a friend / relative / caregiver.", Subtext = "We will only share this information with a volunteer after they have accepted your request" });
                }
            }
        }
        private static IEnumerable<SupportActivities> GetGenericSupportActivities()
        {
            IEnumerable<SupportActivities> activites = new List<SupportActivities>()
            {
                SupportActivities.CheckingIn,
                SupportActivities.CollectingPrescriptions,
                SupportActivities.Errands,
                SupportActivities.FaceMask,
                SupportActivities.HomeworkSupport,
                SupportActivities.MealPreparation,
                SupportActivities.Other,
                SupportActivities.PhoneCalls_Friendly,
                SupportActivities.Shopping
            };
            return activites;
        }
        private static IEnumerable<SupportActivities> GetSupportActivitiesForRequestFormVariant(RequestHelpFormVariant form)
        {
            IEnumerable<SupportActivities> activites;
            IEnumerable<SupportActivities> genericSupportActivities = GetGenericSupportActivities();

            switch (form)
            {
                case RequestHelpFormVariant.FtLOS:
                    activites = new List<SupportActivities>() { SupportActivities.FaceMask };
                    break;

                case RequestHelpFormVariant.VitalsForVeterans:
                    activites = new List<SupportActivities>() 
                    { 
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.Errands,
                        SupportActivities.Other,
                        SupportActivities.WellbeingPackage,
                        SupportActivities.VolunteerSupport
                    };
                    break;

                case RequestHelpFormVariant.Ruddington:
                    activites = new List<SupportActivities>(genericSupportActivities);
                    ((List<SupportActivities>)activites).Remove(SupportActivities.HomeworkSupport);
                    ((List<SupportActivities>)activites).Add(SupportActivities.VolunteerSupport);
                    ((List<SupportActivities>)activites).Add(SupportActivities.DogWalking);

                    break;

                case RequestHelpFormVariant.AgeUKWirral:
                    activites = new List<SupportActivities>
                    { 
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions, 
                        SupportActivities.Other,
                        SupportActivities.ColdWeatherArmy,
                        SupportActivities.VolunteerSupport
                    };
                    break;

                case RequestHelpFormVariant.AgeUKNottsBalderton:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.Other,
                        SupportActivities.DogWalking
                    };
                    break;

                case RequestHelpFormVariant.AgeUKSouthKentCoast_Public:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.MealtimeCompanion,
                        SupportActivities.MealsToYourDoor,
                        SupportActivities.Other
                    };
                    break;

                case RequestHelpFormVariant.AgeUKSouthKentCoast_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.MealtimeCompanion,
                        SupportActivities.MealsToYourDoor,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.Other
                    };
                    break;

                case RequestHelpFormVariant.AgeUKFavershamAndSittingbourne_Public:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.MealtimeCompanion,
                        SupportActivities.MealsToYourDoor,                        
                        SupportActivities.Other
                    };
                    break;

                case RequestHelpFormVariant.AgeUKFavershamAndSittingbourne_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.MealtimeCompanion,
                        SupportActivities.MealsToYourDoor,                        
                        SupportActivities.VolunteerSupport,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.AgeUKNorthWestKent_Public:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.MealsToYourDoor,                        
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.AgeUKNorthWestKent_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.MealsToYourDoor,                        
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.Other
                    };
                    break;

                case RequestHelpFormVariant.LincolnshireVolunteers:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.VaccineSupport,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.EmergencySupport
                    };
                    break;

                case RequestHelpFormVariant.Sandbox_RequestSubmitter:
                    activites = new List<SupportActivities>
                    {
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.Errands,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.VaccineSupport,
                        SupportActivities.Other,
                        SupportActivities.EmergencySupport,
                    };
                    break;

                case RequestHelpFormVariant.AgeConnectsCardiff_Public:
                    activites = new List<SupportActivities>
                    {
                        SupportActivities.Shopping,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.InPersonBefriending,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PracticalSupport,
                        SupportActivities.Other
                    };
                    break;

                case RequestHelpFormVariant.AgeConnectsCardiff_RequestSubmitter:
                    activites = new List<SupportActivities>
                    {
                        SupportActivities.Shopping,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.InPersonBefriending,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PracticalSupport,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.MeadowsCommunityHelpers_Public:
                    activites = new List<SupportActivities>
                    {
                        SupportActivities.Shopping,
                        SupportActivities.FaceMask,
                        SupportActivities.CheckingIn,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.Errands,
                        SupportActivities.DigitalSupport,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.BinDayAssistance,
                        SupportActivities.Covid19Help,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.MeadowsCommunityHelpers_RequestSubmitter:
                    activites = new List<SupportActivities>
                    {
                        SupportActivities.Shopping,
                        SupportActivities.FaceMask,
                        SupportActivities.CheckingIn,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.Errands,
                        SupportActivities.DigitalSupport,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.BinDayAssistance,
                        SupportActivities.Covid19Help,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.AgeUKNottsNorthMuskham:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.Errands,
                        SupportActivities.DogWalking,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.Mansfield_CVS:
                    activites = new List<SupportActivities>()
                    { SupportActivities.VaccineSupport};
                    break;
                case RequestHelpFormVariant.Soutwell_Public:
                    activites = new List<SupportActivities>()
                    { SupportActivities.Shopping,
                      SupportActivities.CollectingPrescriptions,
                      SupportActivities.Other};
                    break;
                case RequestHelpFormVariant.Default:
                case RequestHelpFormVariant.FaceMasks:                
                case RequestHelpFormVariant.DIY:
                    activites = genericSupportActivities; 
                    break;
                case RequestHelpFormVariant.ApexBankStaff_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.BankStaffVaccinator,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.AgeUKMidMersey_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.PracticalSupport,
                        SupportActivities.CheckingIn,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.Errands,
                        SupportActivities.WellbeingPackage,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.DogWalking,
                        SupportActivities.BinDayAssistance,
                        SupportActivities.DigitalSupport,
                        SupportActivities.InPersonBefriending,
                        SupportActivities.Covid19Help,
                        SupportActivities.VolunteerSupport,
                        SupportActivities.ColdWeatherArmy,
                        SupportActivities.SkillShare,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.BostonGNS_Public:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PracticalSupport,
                        SupportActivities.DogWalking,
                        SupportActivities.DigitalSupport,
                        SupportActivities.Other
                    };
                    break;
                case RequestHelpFormVariant.BostonGNS_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Shopping,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.CollectingPrescriptions,
                        SupportActivities.PracticalSupport,
                        SupportActivities.DogWalking,
                        SupportActivities.DigitalSupport,
                        SupportActivities.Other,
                        SupportActivities.VolunteerSupport
                    };
                    break;
                case RequestHelpFormVariant.UkraineRefugees_RequestSubmitter:
                    activites = new List<SupportActivities>()
                    {
                        SupportActivities.Accommodation,
                        SupportActivities.Shopping,
                        SupportActivities.PhoneCalls_Friendly,
                        SupportActivities.CheckingIn,
                        SupportActivities.Other
                    };
                    break;
                default:
                    activites = new List<SupportActivities>();
                    break;
            };

            return activites;
        }
    }
}
