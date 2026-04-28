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
    public class Admin_EDITaxIDSetup_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDITaxIDSetup_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDITaxIDSetup_Detail _obj = null;
        public static Admin_EDITaxIDSetup_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDITaxIDSetup_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the edi tax identifier setup.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEDITaxIDSetup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDITaxIDSetup = new DSEDI();
                    DSEDI.EDITaxIDSetupRow dr = dsEDITaxIDSetup.EDITaxIDSetup.NewEDITaxIDSetupRow();

                    dr.TaxID = SearchedfieldsJSON["txtTaxID"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                        dr.ClearinghouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSubmittterSetup"]))
                        dr.SubmitterSetupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSubmittterSetup"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReceiverSetup"]))
                        dr.ReceiverId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReceiverSetup"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDITaxIDSetup.EDITaxIDSetup.AddEDITaxIDSetupRow(dr);
                    BLObject<DSEDI> obj = BLLAdminEDIObj.InsertEDITaxIDSetup(dsEDITaxIDSetup);
                    dsEDITaxIDSetup = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDITaxIDSetupId = dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows[0][dsEDITaxIDSetup.EDITaxIDSetup.EDITaxIDSetupIdColumn.ColumnName]
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
        /// Updates the EDITaxIDSetup Link.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProcedureCategoryId">The EDITaxIDSetup identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateEDITaxIDSetup(string fieldsJSON, Int64 EDITaxIDSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDITaxIDSetup = new DSEDI();
                    //DSEDI.EDITaxIDSetupRow dr = dsEDITaxIDSetup.EDITaxIDSetup.NewEDITaxIDSetupRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadEDITaxIDSetup(EDITaxIDSetupId, null, null, null, null);
                    dsEDITaxIDSetup = objLoad.Data;
                    foreach (DSEDI.EDITaxIDSetupRow dr in dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows)
                    {
                        //dr.EDITaxIDSetupId = EDITaxIDSetupId;
                        dr.TaxID = SearchedfieldsJSON["txtTaxID"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                            dr.ClearinghouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSubmittterSetup"]))
                            dr.SubmitterSetupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSubmittterSetup"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReceiverSetup"]))
                            dr.ReceiverId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReceiverSetup"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEDITaxIDSetup.EDITaxIDSetup.AddEDITaxIDSetupRow(dr);
                    //dsEDITaxIDSetup.AcceptChanges();

                    if (dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows.Count > 0)
                    {
                        //dsEDITaxIDSetup.EDITaxIDSetup.Rows[0].SetModified();
                        BLObject<DSEDI> obj = BLLAdminEDIObj.UpdateEDITaxIDSetup(dsEDITaxIDSetup);
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
        /// Fills the EDITaxIDSetup.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillEDITaxIDSetup(Int64 EDITaxIDSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDITaxIDSetupId)))
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
                        DSEDI dsEDITaxIDSetup = null;
                        BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDITaxIDSetup(EDITaxIDSetupId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsEDITaxIDSetup = obj.Data;
                            if (dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtTaxID", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.TaxIDColumn.ColumnName])},
                          { "ddlClearingHouse", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.ClearinghouseIdColumn.ColumnName])},
                          { "ddlEntity", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.EntityIdColumn.ColumnName])},
                          { "ddlSubmittterSetup", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.SubmitterSetupIdColumn.ColumnName])},
                          { "ddlReceiverSetup", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.ReceiverIdColumn.ColumnName])},
                          { "chkActive", MDVUtility.ToStr(dr[dsEDITaxIDSetup.EDITaxIDSetup.IsActiveColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    EDITaxIDSetupFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the EDITaxIDSetup.
        /// </summary>
        /// <param name="ProcedureCategoryId">The Plan Fee Link identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteEDITaxIDSetup(Int64 EDITaxIDSetupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDITaxIDSetupId)))
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
                        BLObject<string> obj = BLLAdminEDIObj.DeleteEDITaxIDSetup(MDVUtility.ToStr(EDITaxIDSetupId));
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

        private string UpdateEDITaxIDSetupIsActive(Int64 EDITaxIDSetupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Tax ID Setup", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDITaxIDSetup = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDITaxIDSetup(EDITaxIDSetupId, null, null, null, null);
                    dsEDITaxIDSetup = obj.Data;
                    if (dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDITaxIDSetup.Tables[dsEDITaxIDSetup.EDITaxIDSetup.TableName].Rows[0];
                        dr[dsEDITaxIDSetup.EDITaxIDSetup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objUser = BLLAdminEDIObj.UpdateEDITaxIDSetup(dsEDITaxIDSetup);
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
        /// Handle the Plan Fee Link Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_EDI_TAX_ID_SETUP":
                    {
                        string fieldsJSON = context.Request["EDITaxIDSetupData"];
                        string strJSONData = SaveEDITaxIDSetup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EDI_TAX_ID_SETUP":
                    {
                        string strEDITaxIDSetupId = context.Request["EDITaxIDSetupID"];
                        string strJSONData = FillEDITaxIDSetup(MDVUtility.ToInt64(strEDITaxIDSetupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_TAX_ID_SETUP":
                    {
                        string strEDITaxIDSetupId = context.Request["EDITaxIDSetupID"];
                        string strJSONData = DeleteEDITaxIDSetup(MDVUtility.ToInt64(strEDITaxIDSetupId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_TAX_ID_SETUP":
                    {
                        string fieldsJSON = context.Request["EDITaxIDSetupData"];
                        Int64 EDITaxIDSetupID = MDVUtility.ToInt64(context.Request["EDITaxIDSetupID"]);
                        string strJSONData = UpdateEDITaxIDSetup(fieldsJSON, EDITaxIDSetupID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_TAX_ID_SETUP_ACTIVE_INACTIVE":
                    {
                        Int64 EDITaxIDSetupID = MDVUtility.ToInt64(context.Request["EDITaxIDSetupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDITaxIDSetupIsActive(EDITaxIDSetupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}