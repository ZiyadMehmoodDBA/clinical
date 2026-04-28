using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SurgicalHxModel 
    {
        public string SurgicalHxId { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string SurgicalHxDate { get; set; }
        public string SurgicalHxUnremarkable { get; set; }
        public string SurgicalHxComments { get; set; }
        public string commandType { get; set; }
        public string SurgicalHxType { get; set; }
        public long NotesId { get; set; }
        public long DiseaseId { get; set; }
        public string JSON { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string AddFromMobile { get; set; }

        public List<SurgicalHxDiseaseModel> SurgicalDiseaseList { get; set; }
    }
}