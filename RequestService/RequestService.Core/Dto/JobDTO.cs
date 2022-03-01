using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Dto
{    
    public class JobDTO : JobBasic
    {
        public DateTime DateRequested { get; set; }
        public DateTime DateStatusLastChanged { get; set; }
        public double DistanceInMiles { get; set; }
        public string PostCode { get; set; }
        public bool IsHealthCritical { get; set; }
        public DateTime DueDate { get; set; }
        public bool? Archive { get; set; }
        public string Reference { get; set; }
        public DueDateType DueDateType { get; set; }
        public bool RequestorDefinedByGroup { get; set; }
        public Location? Location { get; set; }
        public int? ShiftLength { get; set; }
        public DateTime? StartDate { get; set; }
        public string SpecificSupportActivity { get; set; }
    }
}
