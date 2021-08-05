using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public class RequestSubmission
    {
        public int RequestId { get; set; }
        public byte FreqencyId { get; set; }
        public int? NumberOfRepeats { get; set; }
        public virtual Request Request { get; set; }
    }
}
