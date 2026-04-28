using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class Procedure
    {
        public Int64 ProcedureId { get; set; }
        public string ICD10 { get; set; }
        public string ICD10Description { get; set; }
        public string SNOMEDDescription { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        public string Comments { get; set; }
    }
}
