using HelpMyStreet.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Dto
{    
    public class JobDTO
    {
        public int? VolunteerUserID { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateStatusLastChanged { get; set; }
        public int ReferringGroupID { get; set; }
        public double DistanceInMiles { get; set; }
        public string PostCode { get; set; }
        public bool IsHealthCritical { get; set; }
        public DateTime DueDate { get; set; }
        public SupportActivities SupportActivity { get; set; }
        public JobStatuses JobStatus { get; set; }
        public int JobID { get; set; }
        public bool? Archive { get; set; }
        public string Reference { get; set; }
        public DueDateType DueDateType { get; set; }
        public int RequestID { get; set; }
        public RequestType RequestType { get; set; }
        public bool RequestorDefinedByGroup { get; set; }
        public  Location Location { get; set; }
        public int ShiftLength { get; set; }
        public DateTime StartDate { get; set; }
    }
}
