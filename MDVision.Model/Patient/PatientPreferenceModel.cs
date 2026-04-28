using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
 public   class PatientPreferenceModel
    {
        //public string PrefCommModeId { get; set; }
        //public string PreferredPrimaryContactId { get; set; }
        public PatientPreferenceModel()
        {

            lstChangedColumns = new List<ChangedColumnsNative>();
            DataChangeRequest = new List<DataChangeRequest>();
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }

        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string PrefCommunicationId { get; set; }

        public string ScndPrefCommunicationId { get; set; }

        public string PatientStatement { get; set; }


        public string CommunicationOptout { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string listChangedColumns { get; set; }
        
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }




    }
}
