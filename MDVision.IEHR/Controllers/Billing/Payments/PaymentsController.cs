using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing;
using MDVision.IEHR.Controls.Billing.Payments;
using MDVision.IEHR.Model.Billing.Payments;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.Payments
{
    public class PaymentsController : ApiController
    {
        public string response { get; set; }
        [HttpPost]
        public string PaymentPosting(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                PaymentPostingModel model = ser.Deserialize<PaymentPostingModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_PaymentPosting.Instance().SearchCharge(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "save")
                {
                    string response = null;
                    response = Bill_PaymentPosting.Instance().SavePatientPayment(model, model.PaymentType);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "is_check_posted")
                {
                    string response = null;
                    response = Bill_PaymentPosting.Instance().IsCheckAlreadyPosted(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string GetPaymentBatch(JObject objData)
        {
            string BatchNumber = HttpContext.Current.Request.Params["data"];
            MDVisionLookups ob = new MDVisionLookups();
            response = ob.GetPaymentBatch(BatchNumber);
            return response;

        }
    }
}
