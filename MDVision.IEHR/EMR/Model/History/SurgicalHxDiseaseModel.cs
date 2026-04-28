using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SurgicalHxDiseaseModel
    {
        public SurgicalHxDiseaseModel()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
        }
        public string DiseaseId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string CPTCode { get; set; }

        public string CPTID { get; set; }
        public string CPTCodeId { get; set; }
        public string CPTDescription { get; set; }
        public string PatientId { get; set; }
        public string SurgicalHxId { get; set; }
        public string SurgicalHxDate { get; set; }
        public string SurgicalHxUnremarkable { get; set; }
        public string Disease { get; set; }
        public string SurgicalStatus { get; set; }
        public string SurgicalStatusText { get; set; }
        public string SurgicalLocation { get; set; }
        public string SurgicalSurgeryDate { get; set; }
        public string AgeAtSurgery { get; set; }
        public string SurgicalReason { get; set; }
        public string SurgicalOrderingProvider { get; set; }
        public string SurgicalPerformingProvider { get; set; }
        public string SurgicalDiseaseComments { get; set; }
        public string SurgicalHxType { get; set; }
        public string PerformingProviderId { get; set; }
        public string OrderingProviderId { get; set; }

        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string FreeTextProcedure { get; set; }


        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddFromMobile { get; set; }
        //public List<SurgicalHxDiseaseModel> SurgicalHxDiseaseList { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }

    }

}