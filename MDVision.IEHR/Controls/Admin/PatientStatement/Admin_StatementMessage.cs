using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.Statement
{
    public class Admin_StatementMessage
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_StatementMessage()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton

        private static Admin_StatementMessage _instance = null;
        public static Admin_StatementMessage Instance()
        {
            if (_instance == null)
                _instance = new Admin_StatementMessage();
            return _instance;
        }

        #endregion

        #region Private Functions
        private string SaveStatementMessage(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "ADDUPDATE_STATEMENT_MESSAGE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPatientStatement dsStatementMessage = new DSPatientStatement();
                    DSPatientStatement.StatementMessageRow dr = dsStatementMessage.StatementMessage.NewStatementMessageRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Message = SearchedfieldsJSON["txtMessage"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsStatementMessage.StatementMessage.AddStatementMessageRow(dr);
                    BLObject<DSPatientStatement> obj = BLLBillingObj.InsertStatementMessage(dsStatementMessage);
                    dsStatementMessage = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            StatementMessageId = dsStatementMessage.Tables[dsStatementMessage.StatementMessage.TableName].Rows[0][dsStatementMessage.StatementMessage.StmtMsgIdColumn.ColumnName]
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


        private string SearchStatementMessage(string fieldsJSON, Int64 StatementMessageID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj;

                    string shortName = "";
                    string message = "";
                    string isActive = "";



                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtShortName"]))
                        shortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMessage"]))
                        message = MDVUtility.ToStr(SearchedfieldsJSON["txtMessage"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                        isActive = MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]);


                    obj = BLLBillingObj.LoadStatementMessage(StatementMessageID, shortName, message, isActive, PageNumber, RowsPerPage);


                    dsPatientStatement = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows.Count,
                                iTotalDisplayRecords = dsPatientStatement.StatementMessage.Rows[0][dsPatientStatement.StatementMessage.RecordCountColumn.ColumnName],
                                StatementMessageLoad_JSON = MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = 0,
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

        private string UpdateStatementMessageActive(Int64 StatementMessageId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj = BLLBillingObj.LoadStatementMessage(StatementMessageId, "", "", "", 1, 15);
                    dsPatientStatement = obj.Data;
                    if (dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows[0];
                        dr[dsPatientStatement.StatementMessage.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientStatement> objStatementMeesage = BLLBillingObj.UpdateStatementMessage(dsPatientStatement);
                        string successMsg;
                        if (objStatementMeesage.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objStatementMeesage.Message
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

        private string UpdateStatementMessage(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSPatientStatement> objStatementMessage;
                    DSPatientStatement dsPatientStatement = new DSPatientStatement();
                    objStatementMessage = BLLBillingObj.LoadStatementMessage(MDVUtility.ToInt64(SearchedfieldsJSON["hfStatementMessageId"]), "", "", "", 1, 15);

                    if (objStatementMessage.Data != null)
                    {
                        foreach (DSPatientStatement.StatementMessageRow dr in objStatementMessage.Data.Tables[dsPatientStatement.StatementMessage.TableName].Rows)
                        {
                            dr.ShortName = SearchedfieldsJSON["txtShortName"];
                            dr.Message = SearchedfieldsJSON["txtMessage"];
                            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dsPatientStatement = objStatementMessage.Data;

                        #region Database Update

                        BLObject<DSPatientStatement> obj = BLLBillingObj.UpdateStatementMessage(dsPatientStatement);

                        if (dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows.Count > 0)
                        {
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
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        private string DeleteStatementMessage(Int64 StatementMessageID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatementMessageID)))
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
                        BLObject<string> obj = BLLBillingObj.DeleteStatementMessage(MDVUtility.ToLong(StatementMessageID));
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


        private string FillStatementMessage(Int64 StatementMessageId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Message", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatementMessageId)))
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
                        DSPatientStatement dsPatientStatement = null;
                        BLObject<DSPatientStatement> obj = BLLBillingObj.LoadStatementMessage(StatementMessageId, "", "", "", 1, 15);
                        if (obj.Data != null)
                        {
                            dsPatientStatement = obj.Data;
                            if (dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsPatientStatement.Tables[dsPatientStatement.StatementMessage.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "hfStatementMessageId", MDVUtility.ToStr(dr[dsPatientStatement.StatementMessage.StmtMsgIdColumn.ColumnName])},
                          { "txtShortName", MDVUtility.ToStr(dr[dsPatientStatement.StatementMessage.ShortNameColumn.ColumnName])},
                          { "txtMessage", MDVUtility.ToStr(dr[dsPatientStatement.StatementMessage.MessageColumn.ColumnName])},
                          { "chkActive", MDVUtility.ToStr(dr[dsPatientStatement.StatementMessage.IsActiveColumn.ColumnName])},

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    StatementMessageFill_JSON = js.Serialize(keyValues)
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
                                Message = obj.Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        }

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
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SAVE_STATEMENT_MESSAGE":
                    {
                        string fieldsJSON = context.Request["StatementMessageData"];
                        string strJSONData = SaveStatementMessage(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "SEARCH_STATEMENT_MESSAGE":
                    {
                        string fieldsJSON = context.Request["StatementMessageData"];
                        string page = context.Request["page"];
                        int pageNo = MDVUtility.ToInt(context.Request["pageNo"]);
                        int recordPerPage = MDVUtility.ToInt(context.Request["recordPerPage"]);

                        Int64 StatementMessageID = MDVUtility.ToInt64(context.Request["StatementMessageID"]);
                        string strJSONData = SearchStatementMessage(fieldsJSON, StatementMessageID, pageNo, recordPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_STATEMENT_MESSAGE_ACTIVE_INACTIVE":
                    {
                        Int64 StatementMessageID = MDVUtility.ToInt64(context.Request["StatementMessageID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateStatementMessageActive(StatementMessageID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_STATEMENT_MESSAGE":
                    {
                        string fieldsJSON = context.Request["StatementMessageData"];
                        string strJSONData = UpdateStatementMessage(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_STATEMENT_MESSAGE":
                    {
                        string statementMessageId = context.Request["StatementMessageId"];
                        string strJSONData = DeleteStatementMessage(MDVUtility.ToInt64(statementMessageId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "FILL_STATEMENT_MESSAGE":
                    {

                        string StatementMessageID = context.Request["StatementMessageID"];
                        string strJSONData = FillStatementMessage(MDVUtility.ToInt64(StatementMessageID));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;




            }
        }





        #endregion
    }
}