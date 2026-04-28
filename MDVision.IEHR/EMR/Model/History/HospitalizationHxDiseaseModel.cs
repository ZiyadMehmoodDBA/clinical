using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class HospitalizationHxDiseaseModel
    {
        public HospitalizationHxDiseaseModel()
        {


            DataChangeRequest = new List<DataChangeRequest>();
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }
        public string HospitalizationDiseaseHospital { get; set; }

        public string DiseaseId { get; set; }
        public string HospitalizationHxId { get; set; }
        public string FreeTextICD { get; set; }

        public string ICDID { get; set; }

        public string CPTID { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
        public string HospitalizationDiseaseFromDate { get; set; }
        public string HospitalizationDiseaseToDate { get; set; }
        public string HospitalizationDiseaseOnset { get; set; }
        public string HospitalizationDiseaseDurationLength { get; set; }
        public string HospitalizationDiseaseDurationPeriod { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeId { get; set; }
        public string CPTDescription { get; set; }
        public string HospitalizationDiseaseStatus { get; set; }
        public string HospitalizationDiseaseStayDuration { get; set; }
        public string HospitalizationDiseaseStayId { get; set; }
        public string HospitalizationDiseaseStayText { get; set; }
        public string HospitalizationDiseaseAdmissionDate { get; set; }
        public string HospitalizationDiseaseDischargeDate { get; set; }
        public string HospitalizationDiseaseHospitalId { get; set; }

        public string HospitalizationDiseaseHospitalText { get; set; }
        public string HospitalizationDiseaseComments { get; set; }
        public string commandType { get; set; }

        public string HospitalizationDiseaseDurationPeriodText { get; set; }
        public string HospitalizationDiseaseTestResultText { get; set; }
        public string HospitalizationDiseaseStatusText { get; set; }
        public string HospitalizationDiseaseSeverityText { get; set; }
        public string HospitalizationDiseasePatternText { get; set; }
        public string HospitalizationDiseaseAgggravatedByText { get; set; }

        public string CPTSNOMEDID { get; set; }
        public string CPTSNOMEDDescription { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddedFromMobileApp { get; set; }
        public List<HospitalizationHxDiseaseModel> HospitalizationDiseaseList { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }

    }
}