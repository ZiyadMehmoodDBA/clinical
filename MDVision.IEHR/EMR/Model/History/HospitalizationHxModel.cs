using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class HospitalizationHxModel
    {
        public HospitalizationHxModel()
        {

            
         
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }
        public string HospitalizationHxId { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string HospitalizationHxDate { get; set; }
        public string HospitalizationHxUnremarkable { get; set; }
        public string HospitalizationHxComments { get; set; }
        public string HospitalizationHxType { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
        public long DiseaseId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddedFromMobileApp { get; set; }
        public List<HospitalizationHxDiseaseModel> HospitalizationDiseaseList { get; set; }
       
    }
}