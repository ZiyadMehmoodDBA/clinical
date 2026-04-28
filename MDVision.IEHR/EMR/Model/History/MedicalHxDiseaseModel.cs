using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class MedicalHxDiseaseModel
    {
        // Start 12/01/2016 Muhammad Irfan properties for disease 
        public MedicalHxDiseaseModel()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
        }


        public string DiseaseId { get; set; }
        public string MedicalHxId { get; set; }
        public string FreeTextICD { get; set; }
        public string ICDID { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
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
        public string commandType { get; set; }
        // End 12/01/2016 Muhammad Irfan properties for disease
        //Start//14/01/2016//Ahmad Raza//added properties for dropdown values' text
        public string MedicalDiseaseDurationPeriodText { get; set; }
        public string MedicalDiseaseTestResultText { get; set; }
        public string MedicalDiseaseStatusText { get; set; }
        public string MedicalDiseaseSeverityText { get; set; }
        public string MedicalDiseasePatternText { get; set; }
        public string MedicalDiseaseAgggravatedByText { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string AddedFromMobileApp { get; set; }
        //End//14/01/2016//Ahmad Raza//added properties for dropdown values' text

        public string JSON { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }

    }
}