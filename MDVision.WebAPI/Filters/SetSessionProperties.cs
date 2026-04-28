using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

namespace MDVision.WebAPI.Filters
{
    public class SetSessionPropertiesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string username = "";
            string entityId = "";
            long AppUserId = 0;
            if (actionContext.ActionArguments.ContainsKey("data"))
            {
                JObject data = actionContext.ActionArguments["data"] as JObject;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                NativeBaseModel model = ser.Deserialize<NativeBaseModel>(MDVUtility.ToStr(data["data"]));
                username = model.UserName;
                entityId = model.EntityId.ToString();
                AppUserId = model.UserId;
            }
            else if (actionContext.ActionArguments.ContainsKey("UserName") && actionContext.ActionArguments.ContainsKey("EntityId") )
            {
                username = actionContext.ActionArguments["UserName"] as string;
                entityId = Convert.ToString(actionContext.ActionArguments["EntityId"]);
             
            }
            else if(actionContext.ActionArguments.ContainsKey("UserName") && actionContext.ActionArguments.ContainsKey("EntityId")&& actionContext.ActionArguments.ContainsKey("UserId"))
            {
                username = actionContext.ActionArguments["UserName"] as string;
                entityId = Convert.ToString( actionContext.ActionArguments["EntityId"]) ;
                AppUserId = MDVUtility.ToLong(actionContext.ActionArguments["UserId"]);
            }

            if (actionContext.ActionArguments.ContainsKey("UserId"))
            {
                AppUserId = MDVUtility.ToLong(actionContext.ActionArguments["UserId"]);
            }
            MDVSession.Current.AppUserId = AppUserId;
            MDVSession.Current.AppUserName = username;
            MDVSession.Current.EntityId = entityId;

            if (ConfigurationManager.AppSettings.AllKeys.Contains("IMOHostName"))
            {
                MDVSession.Current.IMOHostName = ConfigurationManager.AppSettings["IMOHostName"];
            }
            else
            {
                MDVSession.Current.IMOHostName = "portal.e-imo.com";
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IMOICDPort"))
            {
                MDVSession.Current.IMOICDPort = ConfigurationManager.AppSettings["IMOICDPort"];
            }
            else
            {
                MDVSession.Current.IMOICDPort  = "42011";
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IMOCPTPort"))
            {
                MDVSession.Current.IMOCPTPort = ConfigurationManager.AppSettings["IMOCPTPort"];
            }
            else
            {
                MDVSession.Current.IMOCPTPort = "42045";
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IMOID"))
            {
                MDVSession.Current.IMO_ID = ConfigurationManager.AppSettings["IMOID"];
            }
            else
            {
                MDVSession.Current.IMO_ID = "25d3f24926ae1e7a";
            }

            

            //var data2 = actionContext.ControllerContext;

            //if(MDVSession.Current.AppUserName.Length <= 0)
            //    MDVSession.Current.AppUserName = "QWxp";
            //if(MDVSession.Current.EntityId.Length <= 0)
            //    MDVSession.Current.EntityId = "100";

        }
    }
}