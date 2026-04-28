using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MDVision.Business.BCommon;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.EMR.Model.CommonModels;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.EMR.Services
{
    public class FormPrivilegeController : ApiController
    {
        [HttpPost]
        public string AppPrivileges(JObject AllData)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            FormPrivilegeModel model = new FormPrivilegeModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<FormPrivilegeModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new FormPrivilegeModel();
                }

            }
            List<FormPrivileg_ResponseeModel> responseModel = new List<FormPrivileg_ResponseeModel>();
            string privilegasMessage = string.Empty;
            if (model != null && model.FormName.Count > 0 && model.Permission.Count > 0)
            {

                for (int i = 0; i < model.FormName.Count; i++)
                {
                    FormPrivileg_ResponseeModel responseObj = new FormPrivileg_ResponseeModel();
                    string[] permissionArray = model.Permission[i].Split(',');
                    for (int j = 0; j < permissionArray.Length; j++)
                    {
                        privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity(model.FormName[i], permissionArray[j])).ToString();

                        responseObj.FormName.Add(model.FormName[i]);
                        responseObj.Permission.Add(permissionArray[j]);
                        responseObj.privilegasMessage.Add(privilegasMessage);
                    }
                    responseModel.Add(responseObj);
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var response = new
            {
                status = true,
                responsePrivilages_JSON = js.Serialize(responseModel),
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }
    }
}