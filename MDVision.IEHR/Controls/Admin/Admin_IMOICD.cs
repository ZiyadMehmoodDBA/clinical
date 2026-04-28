using System;
using System.Web;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.IEHR.Controls.CommonControls;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_IMOICD
    {
        #region Singleton
        private static Admin_IMOICD _obj = null;
        public static Admin_IMOICD Instance()
        {
            return _obj ?? (_obj = new Admin_IMOICD());
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the ICD Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_ICD":
                    {
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        string entityId = context.Request["entityID"];
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string icd = context.Request["ICD"] == "" ? "0" : context.Request["ICD"];
                        //string strJSONData = LoadICD(fieldsJSON, ICDID, pageNo, rpp);
                        string strJsonData = IMO.GetAllIMOICDCodes(entityId, icd, pageNo, rpp, false);
                         
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            } 
        }
        #endregion
    }
}