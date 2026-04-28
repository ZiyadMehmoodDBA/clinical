using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.ERA
{
    public class Admin_ERAAdjustmentCodes
    {
        private BLLERA BLLERAObj = null;
        public Admin_ERAAdjustmentCodes()
        {
            BLLERAObj = new BLLERA();
        }
        #region Singleton
        private static Admin_ERAAdjustmentCodes _obj = null;
        public static Admin_ERAAdjustmentCodes Instance()
        {
            if (_obj == null)
                _obj = new Admin_ERAAdjustmentCodes();
            return _obj;
        }
        #endregion


        #region Private Functions
        /// <summary>
        /// Load all the Ledger Account for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string Load_ERAADJUSTMENT_CODES(string fieldsJSON, Int64 ERAAdjCodeId, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSERA dsERAAdjCode = null;
                    BLObject<DSERA> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLERAObj.LoadERAAdjustmentCode(ERAAdjCodeId, 0, 0, 0, 0, null);
                    else
                    {
                        long ClaimAdjGroupCodeId = string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimAdjustmentGroupCode"].ToString()) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimAdjustmentGroupCode"].ToString());
                        long ClaimAdjReasonCodesId = string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimAdjustmentReasonCodes"].ToString()) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimAdjustmentReasonCodes"].ToString());
                        long ClearinghouseId = string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"].ToString()) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"].ToString());
                        long ERAActionId = string.IsNullOrEmpty(SearchedfieldsJSON["ddlActions"].ToString()) ? 0 : MDVUtility.ToInt64(SearchedfieldsJSON["ddlActions"].ToString().Split('_')[0]);
                        String IsActive = SearchedfieldsJSON["ddlActive"].ToString();
                        obj = BLLERAObj.LoadERAAdjustmentCode(ERAAdjCodeId, ClaimAdjGroupCodeId, ClaimAdjReasonCodesId, ClearinghouseId, ERAActionId, IsActive, PageNumber, RowsPerPage);
                    }
                    dsERAAdjCode = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ERAAdjustmentCodesCount = dsERAAdjCode.Tables[dsERAAdjCode.ERAAdjustmentCode.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsERAAdjCode.ERAAdjustmentCode.Rows.Count > 0) ? dsERAAdjCode.ERAAdjustmentCode.Rows[0][dsERAAdjCode.ERAAdjustmentCode.RecordCountColumn.ColumnName] : 0,
                            ERAAdjustmentCodesLoad_JSON = MDVUtility.JSON_DataTable(dsERAAdjCode.Tables[dsERAAdjCode.ERAAdjustmentCode.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ERAAdjustmentCodesCount = 0,
                            Message = obj.Message
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_ERA_ADJUSTMENT_CODES":
                    {
                        string fieldsJSON = context.Request["ERAAdjustmentCodesData"];
                        Int64 ERAAdjCodeId = MDVUtility.ToInt64(context.Request["ERAAdjCodeId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = Load_ERAADJUSTMENT_CODES(fieldsJSON, ERAAdjCodeId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                    //case "DELETE_REASON":
                    //    {
                    //        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                    //        string strJSONData = DeleteReason(reasonId);

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;

                    //case "UPDATE_REASON_ACTIVE_INACTIVE":
                    //    {
                    //        Int64 reasonId = MDVUtility.ToInt64(context.Request["reasonId"]);
                    //        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                    //        string strJSONData = ReasonUpdateActiveInactive(reasonId, IsActive);

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //    break;

                    //case "SAVE_REASON":
                    //    {
                    //        string fieldsJson = context.Request["SectionData"];
                    //        string strJsonData = SaveReason(fieldsJson);

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJsonData);
                    //    }
                    //    break;
            }
        }
        #endregion
    }
}