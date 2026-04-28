using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_LedgerAccount_Detail
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_LedgerAccount_Detail()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Admin_LedgerAccount_Detail _obj = null;
        public static Admin_LedgerAccount_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_LedgerAccount_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Ledger Account.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveLedgerAccount(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPaymentSetup dsLedgerAccount = new DSPaymentSetup();
                    DSPaymentSetup.LedgerAccountRow dr = dsLedgerAccount.LedgerAccount.NewLedgerAccountRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstType"]))
                        dr.Type = MDVUtility.ToInt64(SearchedfieldsJSON["lstType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstApplyTo"]))
                        dr.ApplyTo = MDVUtility.ToInt64(SearchedfieldsJSON["lstApplyTo"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstSystemCategory"]))
                        dr.SystemCategory = MDVUtility.ToInt64(SearchedfieldsJSON["lstSystemCategory"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    #region Database Insertion
                    dsLedgerAccount.LedgerAccount.AddLedgerAccountRow(dr);
                    BLObject<DSPaymentSetup> obj = BLLBillingObj.InsertLedgerAccount(ref dsLedgerAccount);
                    dsLedgerAccount = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            LedgerAccountId = dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows[0][dsLedgerAccount.LedgerAccount.LedgerAccountIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the Ledger Account.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="LedgerAccountId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateLedgerAccount(string fieldsJSON, Int64 LedgerAccountId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPaymentSetup dsLedgerAccount = new DSPaymentSetup();
                    //DSPaymentSetup.LedgerAccountRow dr = dsLedgerAccount.LedgerAccount.NewLedgerAccountRow();
                    BLObject<DSPaymentSetup> objLoad = BLLBillingObj.LoadLedgerAccount(LedgerAccountId, null, null, null, null);

                    if (objLoad.Data != null)
                    {
                        dsLedgerAccount = objLoad.Data;

                        DSPaymentSetup.LedgerAccountRow dr = (DSPaymentSetup.LedgerAccountRow)dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows[0];

                        //dr.LedgerAccountId = LedgerAccountId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstType"]))
                            dr.Type = MDVUtility.ToInt64(SearchedfieldsJSON["lstType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstApplyTo"]))
                            dr.ApplyTo = MDVUtility.ToInt64(SearchedfieldsJSON["lstApplyTo"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstSystemCategory"]))
                            dr.SystemCategory = MDVUtility.ToInt64(SearchedfieldsJSON["lstSystemCategory"]);

                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);


                        #region Database Updation
                        //dsLedgerAccount.LedgerAccount.AddLedgerAccountRow(dr);
                        //dsLedgerAccount.LedgerAccount.AcceptChanges();

                        if (dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows.Count > 0)
                        {
                            //dsLedgerAccount.LedgerAccount.Rows[0].SetModified();
                            BLObject<DSPaymentSetup> obj = BLLBillingObj.UpdateLedgerAccount(ref dsLedgerAccount);
                            if (obj.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    message = Common.AppPrivileges.Update_Message
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
                            Message = objLoad.Message,
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

        /// <summary>
        /// Fills the Ledger Account.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillLedgerAccount(Int64 LedgerAccountId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(LedgerAccountId)))
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
                        DSPaymentSetup dsLedgerAccount = null;
                        BLObject<DSPaymentSetup> obj = BLLBillingObj.LoadLedgerAccount(LedgerAccountId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsLedgerAccount = obj.Data;
                            if (dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtShortName", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.ShortNameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.DescriptionColumn.ColumnName])},
                          { "lstType", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.TypeColumn.ColumnName])},
                          { "lstApplyTo", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.ApplyToColumn.ColumnName])},
                          { "lstSystemCategory", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.SystemCategoryColumn.ColumnName])},
                          { "ChkIsActive", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.IsActiveColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsLedgerAccount.LedgerAccount.EntityIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    LedgerAccountFill_JSON = js.Serialize(keyValues),
                                    LedgerAccountLoad_JSON = MDVUtility.JSON_DataTable(dsLedgerAccount.Tables[dsLedgerAccount.LedgerAccount.TableName]),
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
                                Message = obj.Message
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

        /// <summary>
        /// Deletes the Ledger Account.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Ledger Account identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteLedgerAccount(Int64 LedgerAccountId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(LedgerAccountId)))
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
                        BLObject<string> obj = BLLBillingObj.DeleteLedgerAccount(MDVUtility.ToStr(LedgerAccountId));
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

        private string UpdateLedgerAccountIsActive(Int64 LedgerAccountId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Ledger Account", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPaymentSetup dsCode = null;
                    BLObject<DSPaymentSetup> obj = BLLBillingObj.LoadLedgerAccount(LedgerAccountId, null, null, null, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.LedgerAccount.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.LedgerAccount.TableName].Rows[0];
                        dr[dsCode.LedgerAccount.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPaymentSetup> objUser = BLLBillingObj.UpdateLedgerAccount(ref dsCode);
                        string successMsg;
                        if (objUser.Data != null)
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
                                Message = objUser.Message
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Procedure Category Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_LEDGER_ACCOUNT":
                    {
                        string fieldsJSON = context.Request["LedgerAccountData"];
                        string strJSONData = SaveLedgerAccount(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_LEDGER_ACCOUNT":
                    {
                        string strLedgerAccountId = context.Request["LedgerAccountID"];
                        string strJSONData = FillLedgerAccount(MDVUtility.ToInt64(strLedgerAccountId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_LEDGER_ACCOUNT":
                    {
                        string strLedgerAccountId = context.Request["LedgerAccountID"];
                        string strJSONData = DeleteLedgerAccount(MDVUtility.ToInt64(strLedgerAccountId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_LEDGER_ACCOUNT":
                    {
                        string fieldsJSON = context.Request["LedgerAccountData"];
                        Int64 LedgerAccountID = MDVUtility.ToInt64(context.Request["LedgerAccountID"]);
                        string strJSONData = UpdateLedgerAccount(fieldsJSON, LedgerAccountID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_LEDGER_ACCOUNT_ACTIVE_INACTIVE":
                    {
                        Int64 LedgerAccountID = MDVUtility.ToInt64(context.Request["LedgerAccountID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateLedgerAccountIsActive(LedgerAccountID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}