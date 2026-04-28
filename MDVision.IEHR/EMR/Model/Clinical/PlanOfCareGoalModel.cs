using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class PlanOfCareGoalModel
    {
        public string GoalId { get; set; }
        public string PlanOfCareId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
        public string GoalText { get; set; }
        public string Instructions { get; set; }
        public string ClinicalInstructions { get; set; }
        public string FutureScheduleAppointments { get; set; }
        public string PatientDecisionAid { get; set; }
    }
}