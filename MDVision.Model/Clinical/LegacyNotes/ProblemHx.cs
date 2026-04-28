using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class ProblemHx
    {
        public Int64 ProblemListId { get; set; }
        public string ICD10Description { get; set; }
        public string ICD10 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Severity { get; set; }
        public string ChronicityLevel { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }

    }
}
