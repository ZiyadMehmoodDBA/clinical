using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Batch.CQM
{
    public class CQMProblemSearch
    {
        public string GroupId { get; set; }
        public string ProviderId { get; set; }
        public string NPI { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TIN { get; set; }
        public string ProviderTypeId { get; set; }
        public string Address { get; set; }
        public string PatientId { get; set; }
        public string InsurancePlan { get; set; }
        public string EthnicityIds { get; set; }
        public string RaceIds { get; set; }
        public string AgeCondition_text { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Sex_text { get; set; }
        public List<ProblemsList> Problems { get; set; }
        public string ProblemListXML { get; set; }
        public string CQMId { get; set; }
    }
    public class ProblemsList
    {
        public string ICD10 { get; set; }
        public string SnomedCode { get; set; }
    }
}
