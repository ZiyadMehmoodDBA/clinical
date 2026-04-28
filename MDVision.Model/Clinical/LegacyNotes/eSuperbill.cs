using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class eSuperbill
    {

        public string Type { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public float Units { get; set; }
        public DateTime DOSFrom { get; set; }
        public DateTime DOSTo { get; set; }
        public string BillingInfoTimeId { get; set; }
        public string BillingType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ICD10 { get; set; }
        public string ICD10Description { get; set; }
        public int ProblemOrder { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }

    }
}
