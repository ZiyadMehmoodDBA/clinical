using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Schedule
{
    public class ProviderAppointmentPrint
    {
        public int AppointmentId { get; set; }
        public string FacilityName { get; set; }
        public string ProviderName { get; set; }
        public string AppointmentDate { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Comments { get; set; }
        public string Reason { get; set; }

        public string DOB { get; set; }
        public string PatientType { get; set; }
        public string Phone { get; set; }
        public string VisitType { get; set; }
        public string PatBal { get; set; }
        public string InsBal { get; set; }
        public string InsurancePlan { get; set; }
        public string Copay { get; set; }
        public string SubscriberId { get; set; }
        public string GroupId { get; set; }
        public string SchStatusId { get; set; }
        public string SchStatus { get; set; }
        public string ResourceName { get; set; }
    }
}
