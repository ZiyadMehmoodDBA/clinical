using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SurgicalHistoryDiseaseLoad
    {

         public string DiseaseId { get; set; }

        public string SurgicalHxId { get; set; }

        public string ICD9Code { get; set; }

        public string ICD9CodeDescription { get; set; }

        public string ICD10Code { get; set; }

        public string ICD10CodeDescription { get; set; }

        public string SNOMEDID { get; set; }

        public string SNOMEDDescription { get; set; }

        public string LexiCode { get; set; }

        public string LexiCodeDescription { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }

        public string StatusId { get; set; }

        public string Location { get; set; }

        public string SurgeryDate { get; set; }

        public string AgeAtSurgery { get; set; }

        public string SurgeryReason { get; set; }



        public string OrderingProviderId { get; set; }

        public string PerformingProviderId { get; set; }

        public string Comments { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        

        public string ModifiedOn { get; set; }

        public string SoapText { get; set; }
          public string OrderingProviderName { get; set; }
    public string PerformingProviderName { get; set; }
    
        
        public string CPTSNOMEDID { get; set; }

        public string CPTSNOMEDDescription { get; set; }

        public string FreeTextProcedure { get; set; }
        public string FreeTextCPT { get; set; }

    }
}