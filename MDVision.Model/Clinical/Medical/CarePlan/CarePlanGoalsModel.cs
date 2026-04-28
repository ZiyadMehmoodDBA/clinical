using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.CarePlan
{
    public class CarePlanGoalsModel
    {
        public string GoalId { get; set; }
        public string CarePlanId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string CPTSNOMEDID { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string GoalComments { get; set; }
        public string GoalValue { get; set; }
        public string GoalDate { get; set; }
        public string GoalStatus { get; set; }
        public string PatientPriority { get; set; }
        public string ProviderPriority { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ICDId { get; set; }
        public string CPTId { get; set; }
        public string GoalStatusValue { get; set; }
        public string GoalName { get; set; }
        public string PatientId { get; set; }
        public string IsNoteLinked { get; set; }
        public string NotesId { get; set; }
        public string GoalCode { get; set; }
        public string GoalDescription { get; set; }
        public string ShowCPTCode { get; set; }
        public string ProviderId { get; set; }
    }
}
