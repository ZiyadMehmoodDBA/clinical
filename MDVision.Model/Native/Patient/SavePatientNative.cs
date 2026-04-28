using MDVision.Model.Native;
using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
    public class SavePatientNative: NativeBaseModel
    {
        public SavePatientNative()
        {
            patientDemographic_JSON = new PatientDemographicModel();
           
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }

        //public IList<PatientInsuranceModel> patientInsurance_JSON { get; set; }

        public string PrefCommunicationId { get; set; }
        public string ScndPrefCommunicationId { get; set; }
        public bool PatientStatement { get; set; }
        public bool CommunicationOptout { get; set; }
        public PatientDemographicModel patientDemographic_JSON { get; set; }
      

    }
}
