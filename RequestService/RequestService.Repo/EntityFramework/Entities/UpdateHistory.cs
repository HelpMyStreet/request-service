using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public class UpdateHistory
    {
        public int RequestId { get; set; }
        public int JobId { get; set; }
        public DateTime DateCreated { get; set; }
        public string FieldChanged { get; set; }
        public int? QuestionId { get; set; }
        public int CreatedByUserId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual Request Request { get; set; }
    }
}
