using MDVision.Model.Native;
using MDVision.WebAPI.Filters;
using MDVision.WebAPI.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MDVision.WebAPI.Controllers
{
    [SetSessionFromHeader]
    [RoutePrefix("api/Privileges")]
    public class PrivilegesController : ApiController
    {
        private PrivilegesHelper privilegesHelper;
        public PrivilegesController()
        {
            privilegesHelper = new PrivilegesHelper();
        }

        [HttpGet]
        [Route("GetUserPrivileges")]
        public string GetUserPrivileges()
        {
            return JsonConvert.SerializeObject(privilegesHelper.GetUserPrivileges());
        }
        //[HttpGet]
        //[Route("GetPushNotification")]
        //public string GetPushNotification()
        //{
        //      return JsonConvert.SerializeObject(privilegesHelper.pushMessage());
        //   // return "";
        //}
        [HttpPost]
        [Route("LogOut")]
        public string UserLogOut(string deviceId)
        {
           
            try
            {
                return privilegesHelper.Logout(deviceId);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
