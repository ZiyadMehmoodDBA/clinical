using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Dashboard
{
    public class DTCMModel
    {
        public string PatientId { get; set; }
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public string DateOfAppointment { get; set; }
        public string DischargeDate { get; set; }
        public string Insurance { get; set; }
        public string RecordCount { get; set; }
    }
}
