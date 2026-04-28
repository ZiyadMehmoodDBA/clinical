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
    public class Admin_PlaceOfService
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_PlaceOfService()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_PlaceOfService _obj = null;
        public static Admin_PlaceOfService Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlaceOfService();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the place of service.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="POSID">The posid.</param>
        /// <returns></returns>
        private string LoadPlaceOfService(string fieldsJSON, Int64 POSID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Place Of Service", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    string txtCode = null;
                    string txtDescription = null;
                    string IsActive = null;
                    if (SearchedfieldsJSON != null)
                    {
                        if (SearchedfieldsJSON.ContainsKey("txtCode") && !String.IsNullOrEmpty(SearchedfieldsJSON["txtCode"]))
                            txtCode = SearchedfieldsJSON["txtCode"];

                        if (SearchedfieldsJSON.ContainsKey("txtDiscription") && !String.IsNullOrEmpty(SearchedfieldsJSON["txtDiscription"]))
                            txtDescription = SearchedfieldsJSON["txtDiscription"];

                        if (SearchedfieldsJSON.ContainsKey("ddlActive") && !String.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                            IsActive = SearchedfieldsJSON["ddlActive"];
                    }

                    DSCodes dsCode = null;
                    BLObject<DSCodes> objPOS;
                    if (SearchedfieldsJSON == null)
                        objPOS = BLLAdminCodesObj.LoadPlaceOfService(POSID, null, null, IsActive, PageNumber, RowsPerPage);
                    else
                        objPOS = BLLAdminCodesObj.LoadPlaceOfService(POSID, txtCode, txtDescription, IsActive, PageNumber, RowsPerPage);

                    dsCode = objPOS.Data;
                    if (objPOS.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PlaceOfServiceCount = dsCode.Tables[dsCode.PlaceOfService.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCode.PlaceOfService.Rows.Count > 0) ? dsCode.PlaceOfService.Rows[0][dsCode.PlaceOfService.RecordCountColumn.ColumnName] : 0,
                            PlaceOfServiceLoad_JSON = MDVUtility.JSON_DataTable(dsCode.Tables[dsCode.PlaceOfService.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PlaceOfServiceCount = 0,
                            Message = objPOS.Message
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
        /// Handle the Place Of Service Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PLACE_OF_SERVICE":
                    {
                        string fieldsJSON = context.Request["PlaceOfServiceData"];
                        Int64 PlaceOfServiceID = MDVUtility.ToInt64(context.Request["PlaceOfServiceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPlaceOfService(fieldsJSON, PlaceOfServiceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}