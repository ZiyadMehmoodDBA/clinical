using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.FollowUp
{
    public class Bill_FollowUpARCall
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_FollowUpARCall()
        {
            BLLBillingObj = new BLLBilling();
        }

        #region Singleton
        private static Bill_FollowUpARCall _obj = null;
        public static Bill_FollowUpARCall Instance()
        {
            if (_obj == null)
                _obj = new Bill_FollowUpARCall();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string SaveFollowUpARCall(string fieldsJSON, Int64 FollowUpARID, string FollowUpARType)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARCallRow dr = dsFollowUp.FollowupARCall.NewFollowupARCallRow();

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCallType"]))
                    dr.ARTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCallType"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAction"]))
                    dr.FollowupActionId = MDVUtility.ToLong(SearchedfieldsJSON["ddlAction"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReason"]))
                    dr.FollowupReasonId = MDVUtility.ToLong(SearchedfieldsJSON["ddlReason"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                    dr.StatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDuration"]))
                    dr.Duration = MDVUtility.ToStr(SearchedfieldsJSON["txtDuration"]);

                if (FollowUpARType.ToLower() == "insurance")
                {
                    dr.InsuranceARDtId = FollowUpARID;
                }
                else if (FollowUpARType.ToLower() == "patient")
                {
                    dr.PatientARDtId = FollowUpARID;
                }


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    dr.Comments = SearchedfieldsJSON["txtComments"];


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFollowUp.FollowupARCall.AddFollowupARCallRow(dr);

                BLObject<DSFollowUp> obj = BLLBillingObj.InsertFollowUpARCall(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        FollowUpARCalId = dsFollowUp.Tables[dsFollowUp.FollowupARCall.TableName].Rows[0][dsFollowUp.FollowupARCall.ARCallIdColumn.ColumnName],
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        private string LoadFollowUpARCall(Int64 FollowUpARCallID, string FollowUpARType, Int64 ActionId, Int64 ReasonId, Int64 InsurancePatientARDtId, Int32 PageNumber, Int32 RowsPerPage)
        {

            try
            {
                BLObject<DSFollowUp> obj = null;
                if (FollowUpARType.ToLower() == "patient")
                {
                    obj = BLLBillingObj.LoadFollowUpARCall(FollowUpARCallID, 0, ActionId, ReasonId, 0, InsurancePatientARDtId, PageNumber, RowsPerPage);
                }
                else
                {
                    obj = BLLBillingObj.LoadFollowUpARCall(FollowUpARCallID, 0, ActionId, ReasonId, InsurancePatientARDtId,0, PageNumber, RowsPerPage);
                }
               
                if (obj.Data != null && obj.Data.FollowupARCall.Rows.Count> 0)
                {

                    var response = new
                    {
                        status = true,
                        FollowupARCallCount = obj.Data.FollowupARCall.Rows.Count,
                        iTotalDisplayRecords = obj.Data.FollowupARCall.Rows[0][obj.Data.FollowupARCall.RecordCountColumn.ColumnName],
                        FollowupARCall_JSON = MDVUtility.JSON_DataTable(obj.Data.FollowupARCall)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FollowupARCallCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
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

                case "SAVE_FOLLOWUP_AR_CALL":
                    {
                        string JSONData = context.Request["FollowUpARCallData"];
                        Int64 FollowUpARID = MDVUtility.ToInt64(context.Request["FollowUpARID"]);
                        string FollowUpARType = context.Request["FollowUpARType"];

                        string strJsonData = SaveFollowUpARCall(JSONData, FollowUpARID, FollowUpARType);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "LOAD_FOLLOWUP_AR_CALL":
                    {
                        string JSONData = context.Request["FollowUpARCallData"];
                        Int64 FollowUpARCallID = MDVUtility.ToInt64(context.Request["FollowUpARCallID"]);
                        string FollowUpARType = context.Request["FollowUpARType"];
                        Int64 ActionId= MDVUtility.ToInt64(context.Request["ActionId"]);
                        Int64 ReasonId= MDVUtility.ToInt64(context.Request["ReasonId"]);
                        Int64 InsuranceARDtId = MDVUtility.ToInt64(context.Request["FollowUpARID"]);
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJsonData = LoadFollowUpARCall(FollowUpARCallID, FollowUpARType, ActionId, ReasonId, InsuranceARDtId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;


            }
        }
        #endregion
    }
}