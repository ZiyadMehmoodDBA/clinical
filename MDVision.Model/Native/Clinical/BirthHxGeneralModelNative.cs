using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Clinical
{
    public class BirthHxGeneralModelNative
    {

        public BirthHxGeneralModelNative()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
            lstChangedColumns = new List<ChangedColumnsNative>();
        }
        public string GeneralId { get; set; }
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string HospitalName { get; set; }
        public string PatientDOB { get; set; }
        public string LengthStayatHospital { get; set; }
        public string DateAdmitted { get; set; }
        public string ObstetricianName { get; set; }
        public string PediatricianName { get; set; }
        public string ResponsiblePhysicianId { get; set; }
        public string GeneralComments { get; set; }
        public string SoapText { get; set; }

        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string listChangedColumns { get; set; }
        public string ResponsiblePhysicianId_text { get; set; }

        public List<ChangedColumnsNative> lstChangedColumns { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
