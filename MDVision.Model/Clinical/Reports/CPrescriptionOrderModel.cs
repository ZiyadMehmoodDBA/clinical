using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
   public class CPrescriptionOrderModel
    {
        public string AccountNumber { get; set; }
        public bool IncludeInactivePatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public long ProviderId { get; set; }
        public bool MedicationAND { get; set; }
        public string PrescriptionStatus { get; set; }
        public string Medication { get; set; }
        public string Pharmacy { get; set; }
        public string OrderNo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }

   public class CPrescriptionOrderFillModel
    {
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string PatStatus { get; set; }
        public string Provider { get; set; }
        public string Medication { get; set; }
        public string Pharmacy { get; set; }
        public string PrescriptionStatus { get; set; }
        public string Refill { get; set; }
        public string OrderNo { get; set; }
        public string PrescribedOn { get; set; }
        public long PatientId { get; set; }
    }
}
