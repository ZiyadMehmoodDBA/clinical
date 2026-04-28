using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class LabOrder
    {
        public Int64 LabOrderId { get; set; }
        public string Type { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string ICD10 { get; set; }
        public string ICD10Description { get; set; }
        public string Urgency { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SoapText { get; set; }

    }
}
