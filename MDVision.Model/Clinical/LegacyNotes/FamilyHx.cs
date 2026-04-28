using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class FamilyHx
    {
        public Int64 FamilyHxId { get; set; }
        public string ICD9Code { get; set; }
        public string Relation { get; set; }
        public string IsRelativeDied { get; set; }
        public string BirthYear { get; set; }
        public string AgeAtDeath { get; set; }
        public string AgeAtDiagnosis { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string Comments { get; set; }
        public string HealthStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool bUnremarkable { get; set; }

    }
}
