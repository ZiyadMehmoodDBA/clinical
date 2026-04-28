using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.CarePlan
{
   public class CarePlanOutcomesModel
    {
        public string OutcomesId { get; set; }
        public string CarePlanId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string OutcomeComments { get; set; }
        public string OutcomeValue { get; set; }
        public string OutcomeDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ICDId { get; set; }    
        public string GoalIds { get; set; }
        public string InterventionIds { get; set; }
        public string PatientId { get; set; }
        public string IsNoteLinked { get; set; }
        public string NotesId { get; set; }
        public string GoalsStatus { get; set; }
        public string OrderTestLOINCId { get; set; }
        public string LOINCCode { get; set; }
        public string LOINCDescription { get; set; }

    }
}
