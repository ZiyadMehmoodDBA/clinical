using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.FollowUp
{
    public class FollowUpAppointmentModel
    {
        public string Date { get; set; }
        public string AppointmentID { get; set; }
        public string Time { get; set; }
        public string Provider { get; set; }
        public string Facility { get; set; }
        public string Resource { get; set; }
        public string schreason { get; set; }
        public string Duration { get; set; }
        public string Comments { get; set; }
        public string ToComponentName { get; set; }
        public string commandType { get; set; }
        public string PatientId { get; set; }
        public string AppointmentDate { get; set; }
        public string TimeFrom { get; set; }
        public string Reason { get; set; }
        public string ProviderId { get; set; }
        public string FacilityId { get; set; }
        public string SlotTime { get; set; }
        public string BookedSlots { get; set; }
        public string FreeSlots { get; set; }
        public string SlotMinutes { get; set; }
    }
}
