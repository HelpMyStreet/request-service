using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public class LogRequestEvent
    {
        public int RequestId { get; set; }
        public int? JobId { get; set; }
        public int UserId { get; set; }
        public DateTime DateRequested { get; set; }
        public byte RequestEventId { get; set; }

        public virtual Request Request { get; set; }
    }
}
