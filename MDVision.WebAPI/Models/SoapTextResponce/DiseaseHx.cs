using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class DiseaseHx
    {
       public string MedicalDiseaseFromDate { get; set; }
       public string MedicalDiseaseToDate { get; set; }
        public string MedicalDiseaseOnset { get; set; }
        public string MedicalDiseaseDurationLength { get; set; }
        public string MedicalDiseaseDurationPeriod { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeId { get; set; }
        public string CPTDescription { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string MedicalDiseaseTestResult { get; set; }
        public string MedicalDiseaseStatus { get; set; }
        public string MedicalDiseaseLocation { get; set; }
        public string MedicalDiseaseSeverity { get; set; }
        public string MedicalDiseasePattern { get; set; }
        public string MedicalDiseaseAgggravatedBy { get; set; }
        public string MedicalDiseaseComments { get; set; }
    }
}