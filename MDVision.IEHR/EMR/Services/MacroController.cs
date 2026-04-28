using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.Model.Clinical.Macros;

namespace MDVision.IEHR.EMR.Services
{
    public class MacroController : ApiController
    {
        [HttpPost]
        public string Macro(JObject AllData)
        {
            string response = null;


            JavaScriptSerializer ser = new JavaScriptSerializer();
            MacroModel model = JsonConvert.DeserializeObject<MacroModel>(MDVUtility.ToStr(AllData["data"]));
            MacroHelper helper = new MacroHelper();
            if (model.commandType == "Save_Macro")
            {
                response = helper.insertMacro(model);
            }
            else if (model.commandType == "Search_MacroDetailsForNotes")
            {
                response = helper.SearchDetailsForNotes(model);
            }
            else if (model.commandType == "Get_Macro")
            {
                response = helper.showMacro(model);
            }
            else if (model.commandType == "Delete_Macro")
            {
                response = helper.deleteMacro(model);

            }
            else if (model.commandType == "Edit_Macro")
            {
                response = helper.updateMacro(model);

            }
            return response;
        }
    }
}
