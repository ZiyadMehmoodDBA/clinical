using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.ERA
{
    public class STC
    {

        public string ClaimNumber { get; set; }
        public string ChargeAmount { get; set; }
        public string PaidAmount { get; set; }
        public string ClaimCategoryCode { get; set; }
        public string ClaimStatusCode { get; set; }
        public bool isAccepted { get; set; }
        public string EDIReportId { get; set; }
        public string rejectionReason { get; set; }
    }

    public class EDIReport
    {

        public Int64 EDIReportId { get; set; }
        public string EDIText { get; set; }
        public Int64 TotalRejected { get; set; }
        public Int64 TotalAccepted { get; set; }
    }
}
