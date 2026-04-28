using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing.Claims;
using MDVision.IEHR.Model.Billing.Claims;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.Claims
{
    public class EDIReviewReportController : ApiController
    {
        [HttpPost]
        public string EDIReviewReport(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                EDIReviewReportModel model = ser.Deserialize<EDIReviewReportModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "fill_edi_report")
                {
                    string response = null;
                    response = Bill_EDIReviewReport.Instance().FillEDIReport(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "fill_edi_batch_report")
                {
                    string response = null;
                    response = Bill_EDIReviewReport.Instance().FillBatchEDIReport(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "update_report")
                {
                    string response = null;
                    response = Bill_EDIReviewReport.Instance().UpdateReport(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "batch_control_detail")
                {
                    string response = null;
                    response = Bill_EDIReviewReport.Instance().GetBatchDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
