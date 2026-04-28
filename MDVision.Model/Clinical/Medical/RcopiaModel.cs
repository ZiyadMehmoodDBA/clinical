using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical
{
    public class RcopiaModel
    {

        public string PatientId { get; set; }
        public string StartupScreen { get; set; }
        public string commandType { get; set; }
        public string WebBrowserURL { get; set; }
        public string EngineUploadURL { get; set; }
        public string EngineDownloadURL { get; set; }

        public string AllergyLastUpdateDate { get; set; }
        public string MedicationLastUpdateDate { get; set; }
        public string PrescriptionLastUpdateDate { get; set; }
        public bool IsPatientLastUpdateInfo { get; set; }
        public string MedicationLastUpdateDateForLIMP { get; set; }
        public string PrescriptionLastUpdateDateForLIMP { get; set; }
        public string UserId { get; set; }
        public string NotesId { get; set; }
        public string ComeFrom { get; set; }

        public int URLID { get; set; }
        public string CustomerRegCode { get; set; }
        public string RcopiaScretkey { get; set; }
        public string RcopiaANS { get; set; }
        public string RcopiaANSbackup { get; set; }
        public string RcopiaVendorUsername { get; set; }
        public string RcopiaVendorPassword { get; set; }
        public string RcopiaPortalSystemName { get; set; }
        public string RcopiaPracticeUserName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
