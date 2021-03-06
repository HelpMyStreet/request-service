﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public partial class ActivityQuestions
    {
        public int ActivityId { get; set; }
        public int QuestionId { get; set; }
        public int RequestFormVariantId { get; set; }
        public int RequestFormStageId { get; set; }
        public bool Required { get; set; }
        public int Order { get; set; }
        public string Location { get; set; }
        public string Subtext { get; set; }
        public string PlaceholderText { get; set; }
        public virtual Question Question { get; set; }
    }
}
