using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MDVision.Model.AuditableEvents
{
    public class ActivityLog
    {
        public string DateTo { get; set; }
        public string DateFrom { get; set; }
        public string AccountNo { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }
        public string SSN { get; set; }
        public string EmergencyAccess { get; set; }
        public string Status { get; set; }
        public string commandType { get; set; }
        public string PatientId { get; set; }
        public string Patient { get; set; }
        public string User { get; set; }
        public string ModuleName { get; set; }
        public string DateAndTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string ColumnKeyId { get; set; }
        public string UserId { get; set; }
        public string ProfileName { get; set; }
        public string SubProfileName { get; set; }
        public string DBAuditAction { get; set; }
        public string Field { get; set; }
        public string PreviousValue { get; set; }
        public string CurrentValue { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string UserName { get; set; }
        public string Facility { get; set; }
        public string EntityId { get; set; }
        public string DeviceId { get; set; }
        public string FacilityName { get; set; }
        public string EntityName { get; set; }
    }
    public class ActivityLogUser
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string UserName { get; set; }
        public string AdminUser { get; set; }
        public string Action { get; set; }
        public string Field { get; set; }
        public string OriginalValue { get; set; }
        public string CurrentValue { get; set; }
        public string RecordCount { get; set; }
    }

    public class LookupRoles
    {
        public string LookupId { get; set; }
        public string Name { get; set; }

    }

}