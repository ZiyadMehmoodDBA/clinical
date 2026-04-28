using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.Medical.ProblemLists
{

    public class ProblemDetailXML
    {
        public ProblemDetailXML()
        {

            ProblemDetails = new List<ProblemDetailModel>();
        }
        public List<ProblemDetailModel> ProblemDetails { get; set; }
        public string ProblemListId { get; set; }
        public string ProblemDetailId { get; set; }
        public string CancerDiagnosisDate { get; set; }
        public string CancerEffectiveDate { get; set; }
        public string CancerClinicalDiagnosisDate { get; set; }
        public string CancerIsActive { get; set; }
        public string PrimarySiteId { get; set; }
        public string HistologicTypeId { get; set; }
        public string PrimarySite { get; set; }
        public string HistologicType { get; set; }
        public string NKOClinical { get; set; }
        public string NKOPathologic { get; set; }
        public string DiseaseDiscription { get; set; }


    }
    public class ProblemDetailModel
    {
        public string ProblemListId { get; set; }
        public string ProblemDetailId { get; set; }
        public string TNMSystemCodeId { get; set; }
        public string ValueSetName { get; set; }
    }
}