using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.BusinessWrapper; 


namespace MDVision.IEHR.Controls.Clinical
{
    public class Clinical_Section
    {
        #region Singleton

        private static Clinical_Section _instance = null;
        public static Clinical_Section Instance()
        {
            if (_instance == null)
                _instance = new Clinical_Section();
            return _instance;
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions
        /// <summary>
        /// Load User methods.
        /// </summary>
        /// <param name="context">The context.</param>
        private string LoadUser(string fieldsJSON, Int64 UserID)
        {

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                BLObject<DSUsers> objUser;
                DSUsers dsEntity = null;
                if (SearchedfieldsJSON == null)

                    objUser = AdminSecurity.BusinessObj.LoadUser(UserID, null, null, null, null, null, null);
                else

                    objUser = AdminSecurity.BusinessObj.LoadUser(UserID, SearchedfieldsJSON["txtUserName"], SearchedfieldsJSON["lstEntityId"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["chkIsActice"], null);

                dsEntity = objUser.Data;

                var response = new
                {
                    status = true,
                    UserCount = dsEntity.Tables[dsEntity.Users.TableName].Rows.Count,
                    UserLoad_JSON = Utility.JSON_DataTable(dsEntity.Tables[dsEntity.Users.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Public Functions

        #endregion

        #region Control Events
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SEARCH_SECTION":
                    {

                        string fieldsJSON = context.Request["UserData"];
                        Int64 UserID = Utility.ToInt64(context.Request["UserID"]);
                        string strJSONData = LoadUser(fieldsJSON, UserID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}