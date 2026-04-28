using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.CarePlan
{
    public class CarePlanHealthConcernsModel
    {
        public string HealthConcernsId { get; set; }
        public string CarePlanId { get; set; }
        public string Concerns_ICD9Code { get; set; }
        public string Concerns_ICD9CodeDescription { get; set; }
        public string Concerns_ICD10Code { get; set; }
        public string Concerns_ICD10CodeDescription { get; set; }
        public string Concerns_SNOMEDID { get; set; }
        public string Concerns_SNOMEDDescription { get; set; }   
        public string ConcernsComments { get; set; }      
        public string ConcernsDate { get; set; }
        public string ConcernsStatus { get; set; }
        public string ConcernsStatusValue { get; set; }

        public string Observation_ICD9Code { get; set; }
        public string Observation_ICD9CodeDescription { get; set; }
        public string Observation_ICD10Code { get; set; }
        public string Observation_ICD10CodeDescription { get; set; }
        public string Observation_SNOMEDID { get; set; }
        public string Observation_SNOMEDDescription { get; set; }
        public string ObservationComments { get; set; }
        public string ObservationDate { get; set; }        
        public string ObservationPatientPriority { get; set; }
        public string ObservationProviderPriority { get; set; }

        public string Risk_ICD9Code { get; set; }
        public string Risk_ICD9CodeDescription { get; set; }
        public string Risk_ICD10Code { get; set; }
        public string Risk_ICD10CodeDescription { get; set; }
        public string Risk_SNOMEDID { get; set; }
        public string Risk_SNOMEDDescription { get; set; }
        public string RiskComments { get; set; }
        public string RiskDate { get; set; }
        public string RiskStatus { get; set; }
        public string RiskPatientPriority { get; set; }
        public string RiskProviderPriority { get; set; }
        public string RiskStatusValue { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Concerns_ICDId { get; set; }     
        public string Observation_ICDId { get; set; }
        public string Risk_ICDId { get; set; }           
        public string PatientId { get; set; }
        public string IsNoteLinked { get; set; }
        public string NotesId { get; set; }

    }
}
