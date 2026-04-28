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
    public class Admin_ProcedureFeeSchedule
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_ProcedureFeeSchedule()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_ProcedureFeeSchedule _obj = null;
        public static Admin_ProcedureFeeSchedule Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProcedureFeeSchedule();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string LoadProcedureFeeSchedule(string fieldsJSON, Int64 ProcedureFeeSchID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fee Group Plan CPT", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsCode = null;
                    BLObject<DSFeeSchedule> objFS;
                    if (SearchedfieldsJSON == null)
                        objFS = BLLFeeScheduleObj.LoadFeeGroupProceduralSchedule(ProcedureFeeSchID, null, null, null, null);
                    else
                        objFS = BLLFeeScheduleObj.LoadFeeGroupProceduralSchedule(ProcedureFeeSchID, SearchedfieldsJSON["ddlFeeGroup"], SearchedfieldsJSON["ddlPlanFeeLink"], SearchedfieldsJSON["txtCPTCode"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);

                    dsCode = objFS.Data;
                    if (objFS.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureFeeScheduleCount = dsCode.Tables[dsCode.FeeGroupProceduralSchedule.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsCode.FeeGroupProceduralSchedule.Rows.Count > 0) ? dsCode.FeeGroupProceduralSchedule.Rows[0][dsCode.FeeGroupProceduralSchedule.RecordCountColumn.ColumnName] : 0,
                            ProcedureFeeScheduleLoad_JSON = MDVUtility.JSON_DataTable(dsCode.Tables[dsCode.FeeGroupProceduralSchedule.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureFeeScheduleCount = 0,
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
        /// Handle the Procedure Fee Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PROCEDURE_FEE_SCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProcedureFeeScheduleData"];
                        Int64 ProcedureFeeScheduleID = MDVUtility.ToInt64(context.Request["ProcedureFeeScheduleID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadProcedureFeeSchedule(fieldsJSON, ProcedureFeeScheduleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}