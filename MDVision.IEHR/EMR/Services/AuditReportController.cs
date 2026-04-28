/* Author:  Muhammad Arshad
 * Created Date: 14/04/2016
 * OverView: Created to handel AuditReport
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Helpers.Clinical.AuditReport;
using MDVision.IEHR.EMR.Model.AuditReport;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class AuditReportController : ApiController
    {
        [HttpPost]
        // Author:  Muhammad Arshad
        // Created Date: 12/10/2015
        //OverView: Entry point for AuditReport
        public string AuditReport(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            AuditReportModel modelAuditReport = ser.Deserialize<AuditReportModel>(MDVUtility.ToStr(AllData["data"]));


            AuditReportHelper helperAuditReport = new AuditReportHelper();

            if (modelAuditReport.commandType.ToUpper() == "FILL_AUDITREPORT")
            {
                response = null;
                response = helperAuditReport.LoadAuditReport(modelAuditReport, MDVUtility.ToInt32(modelAuditReport.PageNumber), MDVUtility.ToInt32(modelAuditReport.RowsPerPage));

            }
            else if(modelAuditReport.commandType.ToUpper() == "FILL_AUDITREPORT_USER")
            {
                response = null;
                response = helperAuditReport.LoadUserAuditReport(modelAuditReport, MDVUtility.ToInt32(modelAuditReport.PageNumber), MDVUtility.ToInt32(modelAuditReport.RowsPerPage));

            }
            else if (modelAuditReport.commandType.ToUpper() == "PREVIEW_AUDITREPORT")
            {
                response = helperAuditReport.previewAuditReport(modelAuditReport);
            }
            return response;
        }
    }
}