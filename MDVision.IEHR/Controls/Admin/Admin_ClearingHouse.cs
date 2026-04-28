using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ClearingHouse
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_ClearingHouse()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_ClearingHouse _obj = null;
        public static Admin_ClearingHouse Instance()
        {
            if (_obj == null)
                _obj = new Admin_ClearingHouse();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the clearing house.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ClearingHouseID">The clearing house identifier.</param>
        /// <returns></returns>
        private string LoadClearingHouse(string fieldsJSON, Int64 ClearingHouseID, int pageNo, int rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = null;
                    BLObject<DSEDI> objEDI;
                    if (SearchedfieldsJSON == null)
                        objEDI = BLLAdminEDIObj.LoadClearingHouse(ClearingHouseID, null, null, null, null);
                    else
                        objEDI = BLLAdminEDIObj.LoadClearingHouse(ClearingHouseID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["ddlClearingHouseType"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);

                    dsEDI = objEDI.Data;
                    if (objEDI.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ClearingHouseCount = dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEDI.ClearingHouse.Rows.Count > 0) ? dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.RecordCountColumn.ColumnName] : 0,
                            ClearingHouseLoad_JSON = MDVUtility.JSON_DataTable(dsEDI.Tables[dsEDI.ClearingHouse.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClearingHouseCount = 0,
                            Message = objEDI.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Clearing House Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CLEARING_HOUSE":
                    {
                        string fieldsJSON = context.Request["ClearingHouseData"];
                        Int64 ClearingHouseID = MDVUtility.ToInt64(context.Request["ClearingHouseID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadClearingHouse(fieldsJSON, ClearingHouseID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}