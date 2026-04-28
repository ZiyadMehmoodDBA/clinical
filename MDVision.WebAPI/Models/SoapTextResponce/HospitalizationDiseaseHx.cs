using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class HospitalizationDiseaseHx
    {
        public string HospitalizationDiseaseStayDuration { get; set; }
        public string HospitalizationDiseaseStayId { get; set; }
        public string HospitalizationDiseaseStatus { get; set; }
        public string HospitalizationDiseaseAdmissionDate { get; set; }
        public string HospitalizationDiseaseDischargeDate { get; set; }
        public string HospitalizationDiseaseHospital { get; set; }
        public string HospitalizationDiseaseComments { get; set; }
        public string CPT { get; set; }
        public string CPTCodeId { get; set; }
        public string CPTCode { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string CPTDescription { get; set; }
       
    }
}