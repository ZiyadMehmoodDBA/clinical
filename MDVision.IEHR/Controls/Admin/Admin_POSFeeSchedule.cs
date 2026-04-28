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
    public class Admin_POSFeeSchedule
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_POSFeeSchedule()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_POSFeeSchedule _obj = null;
        public static Admin_POSFeeSchedule Instance()
        {
            if (_obj == null)
                _obj = new Admin_POSFeeSchedule();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadPOSFeeSchedule(string fieldsJSON, Int64 POSFeeSchID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT POS", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsFee = null;
                    BLObject<DSFeeSchedule> objFS;
                    if (SearchedfieldsJSON == null)
                        objFS = BLLFeeScheduleObj.LoadFeeGroupPOSSchedule(POSFeeSchID, null, null, null, null, null);
                    else
                        objFS = BLLFeeScheduleObj.LoadFeeGroupPOSSchedule(POSFeeSchID, SearchedfieldsJSON["ddlFeeGroup"], SearchedfieldsJSON["ddlPlanFeeLink"], SearchedfieldsJSON["txtCPTCode"], SearchedfieldsJSON["ddlPOSCode"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                    dsFee = objFS.Data;
                    if (objFS.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            POSFeeScheduleCount = dsFee.Tables[dsFee.FeeGroupPOSSchedule.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsFee.FeeGroupPOSSchedule.Rows.Count > 0) ? dsFee.FeeGroupPOSSchedule.Rows[0][dsFee.FeeGroupPOSSchedule.RecordCountColumn.ColumnName] : 0,
                            POSFeeScheduleLoad_JSON = MDVUtility.JSON_DataTable(dsFee.Tables[dsFee.FeeGroupPOSSchedule.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            POSFeeScheduleCount = 0,
                            Message = objFS.Message
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
        /// Handle the POS Fee Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_POS_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["POSFeeScheduleData"];
                        Int64 POSFeeScheduleID = MDVUtility.ToInt64(context.Request["POSFeeScheduleID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPOSFeeSchedule(fieldsJSON, POSFeeScheduleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}