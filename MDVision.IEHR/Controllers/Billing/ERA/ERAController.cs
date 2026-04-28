using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing.ERA;
using MDVision.IEHR.Model.Billing.ERA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.ERA
{
    public class ERAController : ApiController
    {
        [HttpPost]
        public string ERA(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ERAModel model = ser.Deserialize<ERAModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search_era")
                {
                    string response = null;
                    response = Bill_ERA.Instance().LoadERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "era_update_active_inactive")
                {
                    string response = null;
                    response = Bill_ERA.Instance().UpdateERAIsActive(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "download_era")
                {
                    string response = null;
                    response = Bill_ERA.Instance().DownloadERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "delete_era")
                {
                    string response = null;
                    response = Bill_ERA.Instance().DeleteERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_electronic_eob")
                {
                    string response = null;
                    response = Bill_ERA.Instance().ElectronicEOB(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string ERADetail(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ERAModel model = ser.Deserialize<ERAModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "fill_era")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().FillERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "fill_era_detail")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().FillERADetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_linked_history")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().FillERALinkedHistory(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "delete_era_detail")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().DeleteERADetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_era")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().UpdateERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "save_era_detail")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().SaveERA(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "post_payment")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().PostPayment(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "voided_claim_exist")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().IsVoidedClaimExist(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "post_charges_payment")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().PostChargesPayment(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_eracharge_detail")
                {
                    string response = null;
                    response = Bill_ERACharge_Detail.Instance().FillChargeDetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_era_detail")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().UpdateERADetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "link_era_detail")
                {
                    string response = null;
                    response = Bill_ERA_Detail.Instance().LinkERADetail(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
