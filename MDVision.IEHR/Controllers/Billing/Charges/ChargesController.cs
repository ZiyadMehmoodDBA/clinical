using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing;
using MDVision.IEHR.Controls.Billing.Charges;
using MDVision.IEHR.Controls.Billing.ERA;
using MDVision.IEHR.Model.Billing.Charges;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.Charges
{
    public class ChargesController : ApiController
    {
        [HttpPost]
         public string Charge(JObject objData) {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ChargeModel model = ser.Deserialize<ChargeModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_ChargeSearch.Instance().SearchCharge(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_bill_charge")
                {
                    string response = null;
                    response = Bill_ERA_Charge.Instance().SearchCharge(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "delete_bill_charge")
                {
                    string response = null;
                    response = Bill_ChargeSearch.Instance().DeleteCharge(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string EDIReport(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                EDIReportModel model = ser.Deserialize<EDIReportModel>(MDVUtility.ToStr(objData["data"]));


               if (model.CommandType.ToLower() == "get_latest_reports")
                {
                    string response = null;
                    response = Bill_EDIReport.Instance().GetLatestReports(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "delete_edi_reports")
                {
                    string response = null;
                    response = Bill_EDIReport.Instance().DeleteEDIReport(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        public string EDIReportSearch(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                EDIReportModel model = ser.Deserialize<EDIReportModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_EDIReport.Instance().LoadEDIReports(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }


            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }

    
}
