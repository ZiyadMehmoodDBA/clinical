/*This DTO is used for BirthHx General in Birth History, 
  Author: Khaleel Ur Rehman
  Date: January 06, 2016*/
using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class BirthHxGeneralModel
    {

        public BirthHxGeneralModel()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
            lstChangedColumns = new List<ChangedColumnsNative>();
        }
        public long GeneralId { get; set; }
        public long BirthHxId { get; set; }
        public string HospitalName { get; set; }
        public string PatientDOB { get; set; }
        public string LengthStayatHospital { get; set; }
        public string DateAdmitted { get; set; }
        public string ObstetricianName { get; set; }
        public string PediatricianName { get; set; }
        public int? ResponsiblePhysicianId { get; set; }
        public string GeneralComments { get; set; }
        public string SoapText { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddFromMobile { get; set; }

        public string ResponsiblePhysicianId_text { get; set; }
        public string listChangedColumns { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }
    }
}
