using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Schedule
{
    public class PatientFutureAppointment
    {
        public int AppointmentId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string InsurancePlan { get; set; }
        public string Provider { get; set; }
        public string Facility { get; set; }
        public string CopayType { get; set; }
        public string Copay { get; set; }
    }
}
