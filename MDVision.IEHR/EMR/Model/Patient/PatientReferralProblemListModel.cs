using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Patient
{
    public class PatientReferralProblemListModel
    {
        public string ReferralProblemId { get; set; }

        public string ReferralId { get; set; }

        public string ProblemId { get; set; }

        public string Description { get; set; }
        public string ICD10 { get; set; }

        public string IsActive { get; set; }
        public string commandType { get; set; }
        public string ProblemName { get; set; }       
        
    }
}