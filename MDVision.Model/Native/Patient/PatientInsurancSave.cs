using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
    public class PatientInsurancSave : NativeBaseModel
    {
        public PatientInsurancSave()
        {
            patientInsurance_JSON = new List<PatientInsuranceModel>();


        }

        public IList<PatientInsuranceModel> patientInsurance_JSON { get; set; }
    }
}
