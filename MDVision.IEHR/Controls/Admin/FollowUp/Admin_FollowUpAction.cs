using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.FollowUp
{
    public class Admin_FollowUpAction
    {
        private BLLAdminFollowUp BLLAdminFollowUpObj = null;
        public Admin_FollowUpAction()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }

        #region Singleton
        private static Admin_FollowUpAction _obj = null;
        public static Admin_FollowUpAction Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpAction();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads The Reason
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        private string SaveFollowUpAction(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUpAction = new DSFollowUp();
                DSFollowUp.FollowupActionRow dr = dsFollowUpAction.FollowupAction.NewFollowupActionRow();

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"])))
                    dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                    dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlType"])))
                    dr.ARTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlType"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlLetter"])))
                    dr.LetterId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLetter"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtSuspendAfterDays"])))
                    dr.SuspendedDays = MDVUtility.ToStr(SearchedfieldsJSON["txtSuspendAfterDays"]);


                dr.IsActive = Convert.ToBoolean(SearchedfieldsJSON["chkIsActive"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlAutoAction"])))
                    dr.AutoActionid = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAutoAction"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlLedgerAccount"])))
                    dr.LedgerAccountid = MDVUtility.ToInt64(SearchedfieldsJSON["ddlLedgerAccount"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlNextAction"])))
                    dr.NextActionId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextAction"]);

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFollowUpAction.FollowupAction.AddFollowupActionRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertFollowUpAction(dsFollowUpAction);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ActionId = dsFollowUpAction.Tables[dsFollowUpAction.FollowupAction.TableName].Rows[0][dsFollowUpAction.FollowupAction.FollowupActionIdColumn.ColumnName],
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
        private string UpdateFollowUpAction(string fieldsJSON, Int64 ActionId)
        {

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUpAction = new DSFollowUp();
                BLObject<DSFollowUp> objLoad = BLLAdminFollowUpObj.LoadFollowUpAction(ActionId, "", "", 0, "", 1, 15);
                dsFollowUpAction = objLoad.Data;
                foreach (DSFollowUp.FollowupActionRow dr in dsFollowUpAction.Tables[dsFollowUpAction.FollowupAction.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"])))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlType"])))
                        dr.ARTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlType"]);
                    else
                        dr[dsFollowUpAction.FollowupAction.ARTypeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlLetter"])))
                        dr.LetterId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLetter"]);
                    else
                        dr[dsFollowUpAction.FollowupAction.LetterIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtSuspendAfterDays"])))
                        dr.SuspendedDays = MDVUtility.ToStr(SearchedfieldsJSON["txtSuspendAfterDays"]);


                    dr.IsActive = Convert.ToBoolean(SearchedfieldsJSON["chkIsActive"]);

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlAutoAction"])))
                        dr.AutoActionid = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAutoAction"]);
                    else
                        dr[dsFollowUpAction.FollowupAction.AutoActionidColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlLedgerAccount"])))
                        dr.LedgerAccountid = MDVUtility.ToInt64(SearchedfieldsJSON["ddlLedgerAccount"]);
                    else
                        dr[dsFollowUpAction.FollowupAction.LedgerAccountidColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlNextAction"])))
                        dr.NextActionId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNextAction"]);
                    else
                        dr[dsFollowUpAction.FollowupAction.NextActionIdColumn] = DBNull.Value;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsFollowUpAction.Tables[dsFollowUpAction.FollowupAction.TableName].Rows.Count > 0)
                {
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.UpdateFollowUpAction(dsFollowUpAction);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Update_Message
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
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
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
        private string LoadFollowUpAction(string fieldsJSON, Int64 ActionId, int PageNumber, int RowsPerPage)
        {

            string description = null, shortName = null, isActive = null;
            Int64 ARType = 0;


            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);


                if (SearchedfieldsJSON["txtShortName"] != "")
                    shortName = SearchedfieldsJSON["txtShortName"];

                if (SearchedfieldsJSON["txtDiscription"] != "")
                    description = SearchedfieldsJSON["txtDiscription"];

                if (MDVUtility.ToStr(SearchedfieldsJSON["ddlType"]) != "")
                    ARType = MDVUtility.ToInt64(SearchedfieldsJSON["ddlType"]);

                if (SearchedfieldsJSON["ddlActive"] != "")
                    isActive = MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]);


                DSFollowUp dsFollowUp = null;
                BLObject<DSFollowUp> obj;
                obj = BLLAdminFollowUpObj.LoadFollowUpAction(ActionId, shortName, description, ARType, isActive, PageNumber, RowsPerPage);
                dsFollowUp = obj.Data;

                if (obj.Data != null)
                {
                    if (dsFollowUp.Tables[dsFollowUp.FollowupAction.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActionCount = dsFollowUp.Tables[dsFollowUp.FollowupAction.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFollowUp.FollowupAction.Rows[0][dsFollowUp.FollowupAction.RecordCountColumn.ColumnName],
                            FollowUpActionLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupAction.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActionCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ActionCount = 0,
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
        private string DeleteFollowUpAction(long actionId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(actionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteFollowUpAction(MDVUtility.ToLong(actionId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        private string FillFollowUpAction(Int64 actionId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(actionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSFollowUp dsAction = null;
                    BLObject<DSFollowUp> objLoad = BLLAdminFollowUpObj.LoadFollowUpAction(actionId, "", "", 0, "", 1, 15);
                    if (objLoad.Data != null)
                    {
                        dsAction = objLoad.Data;
                        if (dsAction.Tables[dsAction.FollowupAction.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAction.Tables[dsAction.FollowupAction.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsAction.FollowupAction.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsAction.FollowupAction.DescriptionColumn.ColumnName])},
                            { "ddlType", MDVUtility.ToStr(dr[dsAction.FollowupAction.ARTypeIdColumn.ColumnName])},
                            { "ddlLetter", MDVUtility.ToStr(dr[dsAction.FollowupAction.LetterIdColumn.ColumnName])},
                            { "txtSuspendAfterDays", MDVUtility.ToStr(dr[dsAction.FollowupAction.SuspendedDaysColumn.ColumnName])},
                            { "chkIsActive",  Convert.ToBoolean(dr[dsAction.FollowupAction.IsActiveColumn.ColumnName]).ToString()},
                            { "ddlAutoAction", MDVUtility.ToStr(dr[dsAction.FollowupAction.AutoActionidColumn.ColumnName])},
                            { "ddlLedgerAccount", MDVUtility.ToStr(dr[dsAction.FollowupAction.LedgerAccountidColumn.ColumnName])},
                            { "ddlNextAction", MDVUtility.ToStr(dr[dsAction.FollowupAction.NextActionIdColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ActionLoad_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objLoad.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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

        private string ActionUpdateActiveInactive(Int64 FollowupActionId, Int64 isActive)
        {

            try
            {
                DSFollowUp dsAction = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadFollowUpAction(FollowupActionId, "", "", 0, "", 1, 15);

                if (obj.Data != null)
                {
                    dsAction = obj.Data;
                    if (dsAction.Tables[dsAction.FollowupAction.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsAction.Tables[dsAction.FollowupAction.TableName].Rows[0];
                        dr[dsAction.FollowupAction.IsActiveColumn.ColumnName] = isActive;

                        BLObject<DSFollowUp> objAction = BLLAdminFollowUpObj.UpdateFollowUpAction(dsAction);
                        string successMsg;
                        if (objAction.Data != null)
                        {
                            if (isActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objAction.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
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
                case "SEARCH_ACTION":
                    {
                        string fieldsJSON = context.Request["actionData"];
                        Int64 actionId = MDVUtility.ToInt64(context.Request["actionId"]);
                        int pageNo = MDVUtility.ToInt(context.Request["pageNo"]);
                        int recordPerPage = MDVUtility.ToInt(context.Request["recordPerPage"]);
                        string strJSONData = LoadFollowUpAction(fieldsJSON, actionId, pageNo, recordPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_ACTION":
                    {
                        Int64 actioId = MDVUtility.ToInt64(context.Request["actionId"]);
                        string strJSONData = DeleteFollowUpAction(actioId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "SAVE_ACTION":
                    {
                        string fieldsJson = context.Request["actionData"];
                        string strJsonData = SaveFollowUpAction(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_ACTION":
                    {
                        string fieldsJson = context.Request["actionData"];
                        Int64 reasonId = MDVUtility.ToInt64(context.Request["actionID"]);
                        string strJsonData = UpdateFollowUpAction(fieldsJson, reasonId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_ACTION":
                    {
                        string actionId = context.Request["actionId"];
                        string strJsonData = FillFollowUpAction(MDVUtility.ToInt64(actionId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "UPDATE_ACTION_ACTIVE_INACTIVE":
                    {
                        Int64 FollowupActionId = MDVUtility.ToInt64(context.Request["FollowupActionId"]);
                        Int64 isActive = MDVUtility.ToInt64(context.Request["isActive"]);
                        string strJSONData = ActionUpdateActiveInactive(FollowupActionId, isActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}