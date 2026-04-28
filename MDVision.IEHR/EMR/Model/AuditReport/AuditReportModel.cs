/* Author:  Muhammad Arshad
 * Created Date: 14/04/2016
 * OverView: Created for AuditReport Model
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.AuditReport
{
    public class AuditReportModel
    {
        public string AuditReportId { get; set; }
        public string PatientId { get; set; }
        public string ComponentName { get; set; }
        public string AuditUser { get; set; }
        public string AuditUserName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AuditAction { get; set; }
        public string commandType { get; set; }

        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }

        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string UserType { get; set; }
        public string AuditUserIds { get; set; }
    }
}