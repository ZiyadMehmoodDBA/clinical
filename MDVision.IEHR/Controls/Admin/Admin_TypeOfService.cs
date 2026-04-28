using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_TypeOfService
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_TypeOfService()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_TypeOfService _obj = null;
        public static Admin_TypeOfService Instance()
        {
            if (_obj == null)
                _obj = new Admin_TypeOfService();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadTypeOfService(string fieldsJSON, string TOSID, int PageNumber, int RowsPerPage)
       {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Type Of Service", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    string IsActive = null;

                    DSCodes dsCode = null;
                    BLObject<DSCodes> objTOS;
                    if (SearchedfieldsJSON == null)
                        objTOS = BLLAdminCodesObj.LoadTypeOfService(0, MDVUtility.ToStr(TOSID), null, null, null);
                    else
                    {
                        if (SearchedfieldsJSON.ContainsKey("ddlActive") && !String.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                            IsActive = SearchedfieldsJSON["ddlActive"];
                        objTOS = BLLAdminCodesObj.LoadTypeOfService(0, SearchedfieldsJSON["txtCode"], SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    }
                    dsCode = objTOS.Data;
                    if (objTOS.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            TypeOfServiceCount = dsCode.Tables[dsCode.TypeOfService.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCode.TypeOfService.Rows.Count > 0) ? dsCode.TypeOfService.Rows[0][dsCode.TypeOfService.RecordCountColumn.ColumnName] : 0,
                            TypeOfServiceLoad_JSON = MDVUtility.JSON_DataTable(dsCode.Tables[dsCode.TypeOfService.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TypeOfServiceCount = 0,
                            Message = objTOS.Message
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
        /// Handle the Type Of Service Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_TYPE_OF_SERVICE":
                    {   
                        string fieldsJSON = context.Request["TypeOfServiceData"];
                        string TypeOfServiceID = context.Request["TypeOfServiceID"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadTypeOfService(fieldsJSON, TypeOfServiceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}