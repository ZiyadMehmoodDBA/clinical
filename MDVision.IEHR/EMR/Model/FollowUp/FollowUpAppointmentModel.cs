using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FollowUp
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
        public string SlotType { get; set; }
        public string SlotValue { get; set; }
        public string FromTimeSlots { get; set; }
        public string ToTimeSlots { get; set; }
        public string VisitId { get; set; }

    }
}