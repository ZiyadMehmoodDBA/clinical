using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Clinical.History
{
    public class FamilyHxDiseaseModel
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
    }
}
