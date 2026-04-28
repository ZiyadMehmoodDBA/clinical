using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Report
{
   public class ProviderMonthlyPayment
    {
        public string  FaciltyName { get; set; }
        public string Insurance { get; set; }
        public string CPTCode { get; set; }
        public string FullName { get; set; }
        public string AccountNumber { get; set; }
        public string ChargeCapId { get; set; }
        public string InsurancePlanId { get; set; }
        public string PatientId { get; set; }
        public string FacilityId { get; set; }
        public string VisitId { get; set; }
        public string ServiceDate { get; set; }
        public string PaidDate { get; set; }
        public string ClaimNumber { get; set; }
        public string Fee { get; set; }
        public string PatientPaid { get; set; }
        public string PaymentPaid { get; set; }
        public string InsurancePaidAmt { get; set; }
        public string TotalAdj { get; set; }
    }
}
