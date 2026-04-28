using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
   public class PatientEmergencyContactModel:NativeBaseModel


    {
        public PatientEmergencyContactModel()
        {

            lstChangedColumns = new List<ChangedColumnsNative>();
            DataChangeRequest = new List<DataChangeRequest>();
            //patientInsurance_JSON = new List<PatientInsuranceModel>();
        }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string DOB { get; set; }
        public string EmailAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Zip { get; set; }
        public string ZipExt { get; set; }
        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }
        public string WorkPhext { get; set; }
        public string CellNo { get; set; }
        public string FaxNo { get; set; }
        public string IsPrimary { get; set; }
        public string IsActive { get; set; }
        public string RelationShipId { get; set; }
        public string Relation_text { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string ContactId { get; set; }
        public string RecordCount { get; set; }

        public string listChangedColumns { get; set; }
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
