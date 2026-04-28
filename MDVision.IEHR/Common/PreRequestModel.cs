using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Common
{
    public class PreRequestModel
    {
        public bool IsLogIn { get; set; }
        public string RedirectSet { get; set; }
        public PreRequestModel()
        {
            IsLogIn = false;
        }
    }

    public static class MDVisionConstants
    {
        public static string RequestModel = "RequestModel";
        public static string ResponseModel = "ResponseModel";
    }
}