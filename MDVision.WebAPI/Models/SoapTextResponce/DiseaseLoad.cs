using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class DiseaseLoad
    {
        public string DiseaseId { get; set; }
        public string FamilyHxId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string FamilyMemberId { get; set; }
        public string FreeTextICD { get; set; }

    }
}