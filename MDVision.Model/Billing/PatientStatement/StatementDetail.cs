using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.PatientStatement
{
    public class StatementDetail
    {
        public Int64 PatientId { get; set; }
        public decimal Charges { get; set; }
        public Int64 ChargeCapId { get; set; }
        public string Procedure { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal PatBalance { get; set; }
        public decimal Paid { get; set; }
        public int Age { get; set; }
        public Int64 VisitId { get; set; }
        public string ClaimNumber { get; set; }
        public string Units { get; set; }
        public Int64 ChargeProviderId { get; set; }
        public string LedgerType { get; set; }
        public string FullName { get; set; }
        public string PlanPriority { get; set; }
        public Int64 ChargeFacilityId { get; set; }
        public string NxtPlanPriority { get; set; }
        public int LedgerAccountId { get; set; }
    }
}
