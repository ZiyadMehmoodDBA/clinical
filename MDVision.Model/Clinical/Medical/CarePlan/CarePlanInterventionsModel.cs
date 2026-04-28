using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.CarePlan
{
    public class CarePlanInterventionsModel
    {
        public string InterventionId { get; set; }
        public string CarePlanId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string InterventionComments { get; set; }
        public string InterventionDate { get; set; }
        public string InterventionStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ICDId { get; set; }    
        public string InterventionStatusValue { get; set; }
        public string MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string GoalIds { get; set; }
        public string MedicationIds { get; set; }
        public string InterventionName { get; set; }
        public string PatientId { get; set; }
        public string IsNoteLinked { get; set; }
        public string NotesId { get; set; }
        public string OrderTestLOINCId { get; set; }
        public string LOINCCode { get; set; }
        public string LOINCDescription { get; set; }

    }
}
