using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
   public class PatientReminderLog
    {

        public string AppointmentDate { get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public string AppointmentStatus { get; set; }
        public string Status { get; set; }
        public string ReminderResponse { get; set; }
        public string ReminderType { get; set; }
    }
}
