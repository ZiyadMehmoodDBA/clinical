using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Clinical.History
{
    public class MedicalHxDiseaseModel
    {
        public string DiseaseId { get; set; }
        public string MedicalHxId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }
        public string StatusId { get; set; }
        public string CPTSNOMEDID { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string SoapText { get; set; }
        public string Action { get; set; }
    }
}
