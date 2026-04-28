using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class MedicalHx
    {
        public Int64 MedicalHxId { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string CPTCodeDescription { get; set; }
        public string Status { get; set; }
        public string TestResult { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Onset { get; set; }
        public string DurationLength { get; set; }
        public string Duration { get; set; }
        public string Severity { get; set; }
        public string Pattern { get; set; }
        public string AggravatedBy { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool bUnremarkable { get; set; }
        public string OverAllComments { get; set; }


    }
}
