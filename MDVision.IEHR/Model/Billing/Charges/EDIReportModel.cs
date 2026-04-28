using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Billing.Charges
{
    public class EDIReportModel
    {
        public string Clearinghouse { get; set; }
        public string ReportTitle { get; set; }
        public string DownloadDate { get; set; }
        public string FileName { get; set; }
        public string ReviewStatus { get; set; }
        public string EDIText { get; set; }
        public string IsERADeleted { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string CommandType { get; set; }
        public string EDIReportID { get; set; }
    }
}