using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Dashboard
{
   public class MobileAppRequestModel 
    {
        public string PatientId { get; set; }
        public string AppointmentId { get; set; }
        public string DimmyPatientId { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string Provider { get; set; }
        public string Facility { get; set; }
        public string Status { get; set; }
        public string RequestReceivedAt { get; set; }
        public string RequestReceivedFor { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentReason { get; set; }
        public string RejectReason { get; set; }
        public string DBTableName { get; set; }
        public string TimeFrom { get; set; }
        public string RejectionReason { get; set; }
        public string RecordCount { get; set; }
    }
}
