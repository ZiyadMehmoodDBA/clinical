using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native
{
  public  class DataChangeRequest
    {
        public string HealthStatusId { get; set; }
        public long DbAuditId { get; set; }
        public string columnName { get; set; }
        public string DBTableName { get; set; }
        public string ColumnKeyId { get; set; }      
        public string ColumnKeyName { get; set; } 
        public long? PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public long SocialHxId { get; set; }
        public long MiscHxId { get; set; }
        public long HospitalizationHxId { get; set; }
        public long SurgicalHxId { get; set; }
        public long MedicalHxId { get; set; }
        public long BirthHxId { get; set; }
        public long FamilyHxId { get; set; }
        public long EntityId { get; set; }
        public string OriginalValueDisplay { get; set; }
        public string CurrentValueDisplay { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsSynced { get; set; }

        public string UserName { get; set; }
      
        public long UserId { get; set; }

        public string test { get; set; }

        public long FamilyMemberId { get; set; }

        public string MemberId { get; set; }

        //public string HealthStatus { get; set; }


    }
}
