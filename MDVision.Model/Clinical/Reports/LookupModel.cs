using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class LookupModel
    {
        public Int64 LookupTypeId { get; set; }
        public string  LookUpType { get; set; }
        public Int64 Id { get; set; }
        public string Name { get; set; }
    }
}
