using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
    public class LoadMuiltPatientAddress : IBaseModel
    {

        public string NetWorkIP { get; set; }
        public string Ext { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string AccountNo { get; set; }
        public string EntityId { get; set; }

        public string PatientId { get; set; }
        public string Address { get; set; }

        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string errorMessage { get; set; }
        public string Contacts { get; set; }
        public string RelationShipId { get; set; }
        public string RelationShipName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string SignaturePic { get; set; }
        public string ScheduledTime { get; set; }
        public string EstimatedTime { get; set; }
        public string PatientAhead { get; set; }
        public string PatientProfileImagePath { get; set; }
        public string MobileNumber { get; set; }



        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            Address = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Address", incommingColumnList));
            Contacts = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PhoneNumber", incommingColumnList));
            RelationShipId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Id", incommingColumnList));
            RelationShipName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Description", incommingColumnList));
            ScheduledTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ScheduledTime", incommingColumnList));
            EstimatedTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EstimatedTime", incommingColumnList));
            PatientAhead = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientAhead", incommingColumnList));
            PatientProfileImagePath = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientProfileImagePath", incommingColumnList));
        }
    }
}
