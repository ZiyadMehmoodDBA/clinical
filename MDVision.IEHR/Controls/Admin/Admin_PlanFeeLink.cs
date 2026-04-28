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
    public class Admin_PlanFeeLink
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        public Admin_PlanFeeLink()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
        }
        #region Singleton
        private static Admin_PlanFeeLink _obj = null;
        public static Admin_PlanFeeLink Instance()
        {
            if (_obj == null)
                _obj = new Admin_PlanFeeLink();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the PlanFeeLink for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadPlanFeeLink(string fieldsJSON, Int64 PlanFeeLinkID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Plan Fee Link", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSFeeSchedule dsPlanFeeLink = null;
                    BLObject<DSFeeSchedule> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLFeeScheduleObj.LoadPlanFeeLink(PlanFeeLinkID, null, null, null, null);
                    else
                        obj = BLLFeeScheduleObj.LoadPlanFeeLink(PlanFeeLinkID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                    dsPlanFeeLink = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PlanFeeLinkCount = dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsPlanFeeLink.PlanFeeLink.Rows.Count > 0) ? dsPlanFeeLink.PlanFeeLink.Rows[0][dsPlanFeeLink.PlanFeeLink.RecordCountColumn.ColumnName] : 0,
                            PlanFeeLinkLoad_JSON = MDVUtility.JSON_DataTable(dsPlanFeeLink.Tables[dsPlanFeeLink.PlanFeeLink.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PlanFeeLinkCount = 0,
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
        /// Handle the Procedure Category Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PLAN_FEE_LINK":
                    {
                        string fieldsJSON = context.Request["PlanFeeLinkData"];
                        Int64 PlanFeeLinkID = MDVUtility.ToInt64(context.Request["PlanFeeLinkID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPlanFeeLink(fieldsJSON, PlanFeeLinkID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}