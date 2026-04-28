using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Schedule
{
   public class AppointmentCopayAlert
    {
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string InsurancePlan { get; set; }
        public string ProviderName { get; set; }
        public string FacilityName { get; set; }
        public string Copayment { get; set; }
        public string RemainingBalance { get; set; }
        public string VisitId { get; set; }
        public bool IsCopayAlert { get; set; }
    }
}
