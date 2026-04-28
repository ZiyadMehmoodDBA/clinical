using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing.Claims
{
    public class EDIReviewReportModel
    {
        public string BatchNumber { get; set; }
        public string ClearingHouse { get; set; }
        public string Comments { get; set; }
        public string Active { get; set; }
        public string TextView { get; set; }
        public string CommandType { get; set; }
        public string EDIReportID { get; set; }
        public string CheckNumber { get; set; }
        public string HtmlView { get; set; }
        public string ReportType { get; set; }
        public string EDIBatchNumber { get; set; }
       
    }
}