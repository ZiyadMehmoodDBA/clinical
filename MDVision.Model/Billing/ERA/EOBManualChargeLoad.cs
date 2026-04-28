using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.ERA
{
   public class EOBManualChargeLoad
    {
        public int VisitId { get; set; }
        public int ChargeCapId { get; set; }
        public string DOSFrom { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public decimal Fee { get; set; }
        public decimal Billed { get; set; }
        public decimal InsCharges { get; set; }
        public decimal PatCharges { get; set; }
        public decimal Copay { get; set; }
        public decimal AllowedAmt { get; set; }
        public string PatientInsuranceId { get; set; }
        public int PatientId { get; set; }
        public string VisitInsuranceId { get; set; }

    }
}
