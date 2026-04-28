using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Patient
{
    public class PatientReferralProcedureModel
    {
        public string ReferralProcedureId { get; set; }
        public string ReferralId { get; set; }
        public string Procedure { get; set; }
        public string Urgency { get; set; }
        public string Urgency_text { get; set; }
        public string commandType { get; set; }
        public string SoapText { get; set; }
        public string ProcedureIds { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string ShowCPTCode { get; set; }
            
    }
}