using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class SurgicalHx
    {
        public Int64 SurgicalHxId { get; set; }
        public string CPTDescription { get; set; }
        public string SNOMEDDescription { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public string AgeAtSurgery { get; set; }
        public string SurgeryReason { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string OrderingProvider { get; set; }
        public string PerformingProvider { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool bUnremarkable { get; set; }

    }
}
