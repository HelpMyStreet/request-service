using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public class Shift
    {
        public int RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public int ShiftLength { get; set; }

        public virtual Request Request { get; set; }
    }
}
