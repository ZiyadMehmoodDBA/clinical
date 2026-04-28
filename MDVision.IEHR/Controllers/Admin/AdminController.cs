using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Admin;
using MDVision.IEHR.Model.Admin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Admin
{
    public class AdminController : ApiController
    {
        [HttpPost]
        public string SearchRefProviderKendoMultiSelect(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ReferringProviderModel model = ser.Deserialize<ReferringProviderModel>(MDVUtility.ToStr(objData["data"]));
                {
                    string response = null;
                    response = Admin_ReferringProvider_Detail.Instance().SearchReferringProvider(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}