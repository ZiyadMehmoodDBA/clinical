/// Author: ZeeshanAK
/// Purpose:  Created to handel Break The Glass
/// Date : April 16, 2016


using MDVision.Business.BCommon;
using MDVision.IEHR.Controls.Patient;
using MDVision.IEHR.EMR.Helpers.Patient;
using MDVision.IEHR.EMR.Model.Patient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.EMR.Services
{
    public class PatientBreakTheGlassController : ApiController
    {
        [HttpPost]
        public string BreakTheGlass(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PatientBreakTheGlassModel model = ser.Deserialize<PatientBreakTheGlassModel>(MDVUtility.ToStr(AllData["data"]));

            PatientBreakTheGlassHelper helperBTG = new PatientBreakTheGlassHelper();


            if (model.commandType.ToLower() == "restrict_user_load")
            {
                response = helperBTG.restrictUserLoad(model);
            }
            else if (model.commandType.ToLower() == "restrict_user_save")
            {
                response = helperBTG.restrictUserSave(model);
            }
            else if (model.commandType.ToLower() == "restrict_user_delete")
            {
                response = helperBTG.deletePatientBreakGlass(model);
            }
            else if (model.commandType.ToLower() == "update_user_btg")
            {
                response = helperBTG.updateUserBreakGlass(model);
            }
            else if (model.commandType.ToLower() == "save_breakglass_reason")
            {
                response = helperBTG.breakTheGlassReasonSave(model);
            }

            
            return response;
        }
    }
}