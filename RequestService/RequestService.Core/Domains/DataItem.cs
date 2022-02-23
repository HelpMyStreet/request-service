using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Domains
{
    public class DataItem
    {
        public string Series { get; set; }
        public DateTime Date { get; set; }
        public List<SubDataItem> SubDataItems { get; set; }
    }
}
