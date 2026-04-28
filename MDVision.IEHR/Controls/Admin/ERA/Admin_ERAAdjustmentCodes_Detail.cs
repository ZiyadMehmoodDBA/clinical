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

namespace MDVision.IEHR.Controls.Admin.ERA
{
    public class Admin_ERAAdjustmentCodes_Detail
    {
        private BLLERA BLLERAObj = null;
        public Admin_ERAAdjustmentCodes_Detail()
        {
            BLLERAObj = new BLLERA();
        }
        #region Singleton
        private static Admin_ERAAdjustmentCodes_Detail _obj = null;
        public static Admin_ERAAdjustmentCodes_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ERAAdjustmentCodes_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the ERA Adjustment Codes.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveERAAdjustmentCode(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSERA dsERAAdjustmentCode = new DSERA();
                    DSERA.ERAAdjustmentCodeRow dr = dsERAAdjustmentCode.ERAAdjustmentCode.NewERAAdjustmentCodeRow();
                    dr.ERAAdjCodeId = -1;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClaimAdjReasonCodeID"]))
                        dr.ClaimAdjReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["ClaimAdjReasonCodeID"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClaimAdjuGroupCodeID"]))
                        dr.ClaimAdjuGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ClaimAdjuGroupCodeID"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["PracticeId"]))
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["PracticeId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClearingHouseId"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ClearingHouseId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ERAActionId"]))
                        dr.ERAActionId = MDVUtility.ToInt64(SearchedfieldsJSON["ERAActionId"].ToString().Split('_')[0]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["LedgerAccountId"]))
                        dr.LedgerAccountId = MDVUtility.ToInt64(SearchedfieldsJSON["LedgerAccountId"]);


                    // dr.LedgerAccountId = string.IsNullOrWhiteSpace(SearchedfieldsJSON["LedgerAccountId"]);

                    dr.LedgerEntryComments = SearchedfieldsJSON["LedgerEntryComments"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsERAAdjustmentCode.ERAAdjustmentCode.AddERAAdjustmentCodeRow(dr);
                    BLObject<DSERA> obj = BLLERAObj.InsertERAAdjustmentCode(ref dsERAAdjustmentCode);
                    dsERAAdjustmentCode = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ERAAdjCodeId = dsERAAdjustmentCode.Tables[dsERAAdjustmentCode.ERAAdjustmentCode.TableName].Rows[0][dsERAAdjustmentCode.ERAAdjustmentCode.ERAAdjCodeIdColumn.ColumnName]
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
        /// Updates the ERA Adjustment Codes.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ERAAdjCodeId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateERAAdjustmentCode(string fieldsJSON, Int64 ERAAdjCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSERA dsCode = null;
                    BLObject<DSERA> obj = BLLERAObj.LoadERAAdjustmentCode(ERAAdjCodeId, 0, 0, 0, 0, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.ERAAdjustmentCode.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.ERAAdjustmentCode.TableName].Rows[0];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClaimAdjReasonCodeID"]))
                            dr[dsCode.ERAAdjustmentCode.ClaimAdjReasonIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["ClaimAdjReasonCodeID"]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.ClaimAdjReasonIdColumn.ColumnName] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClaimAdjuGroupCodeID"]))
                            dr[dsCode.ERAAdjustmentCode.ClaimAdjuGroupIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["ClaimAdjuGroupCodeID"]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.ClaimAdjuGroupIdColumn.ColumnName] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["PracticeId"]))
                            dr[dsCode.ERAAdjustmentCode.PracticeIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["PracticeId"]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.PracticeIdColumn.ColumnName] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ClearingHouseId"]))
                            dr[dsCode.ERAAdjustmentCode.ClearingHouseIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["ClearingHouseId"]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.ClearingHouseIdColumn.ColumnName] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ERAActionId"]))
                            dr[dsCode.ERAAdjustmentCode.ERAActionIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["ERAActionId"].ToString().Split('_')[0]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.ERAActionIdColumn.ColumnName] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["LedgerAccountId"]))
                            dr[dsCode.ERAAdjustmentCode.LedgerAccountIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["LedgerAccountId"]);
                        else
                        {
                            dr[dsCode.ERAAdjustmentCode.LedgerAccountIdColumn.ColumnName] = DBNull.Value;
                        }

                        dr[dsCode.ERAAdjustmentCode.LedgerEntryCommentsColumn.ColumnName] = SearchedfieldsJSON["LedgerEntryComments"];
                        dr[dsCode.ERAAdjustmentCode.IsActiveColumn.ColumnName] = MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;

                        dr[dsCode.ERAAdjustmentCode.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsCode.ERAAdjustmentCode.ModifiedOnColumn.ColumnName] = DateTime.Now;


                        BLObject<DSERA> objUpdated = BLLERAObj.UpdateERAAdjustmentCode(dsCode);
                        if (objUpdated.Data != null)
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
                                Message = objUpdated.Message
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
        /// Fills the ERA Adjustment Codes.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillERAAdjustmentCode(Int64 ERAAdjCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ERAAdjCodeId)))
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
                        DSERA dsERAAdjustmentCode = null;
                        BLObject<DSERA> obj = BLLERAObj.LoadERAAdjustmentCode(ERAAdjCodeId, 0, 0, 0, 0, null);
                        if (obj.Data != null)
                        {
                            dsERAAdjustmentCode = obj.Data;
                            if (dsERAAdjustmentCode.Tables[dsERAAdjustmentCode.ERAAdjustmentCode.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsERAAdjustmentCode.Tables[dsERAAdjustmentCode.ERAAdjustmentCode.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "ClaimAdjReasonCodeID", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.ClaimAdjReasonIdColumn.ColumnName])},
                          { "ClaimAdjuGroupCodeID", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.ClaimAdjuGroupIdColumn.ColumnName])},
                          { "ClearingHouseId", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.ClearingHouseIdColumn.ColumnName])},
                          { "ERAActionId", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.ERAActionIdColumn.ColumnName])},
                         // { "ERAAdjCode_Id", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.ERAAdjCodeIdColumn.ColumnName])},
                          { "IsActive", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.IsActiveColumn.ColumnName])},
                          { "PracticeId", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.PracticeIdColumn.ColumnName])},
                          { "LedgerEntryComments", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.LedgerEntryCommentsColumn.ColumnName])},
                          { "LedgerAccountId", MDVUtility.ToStr(dr[dsERAAdjustmentCode.ERAAdjustmentCode.LedgerAccountIdColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ERAAdjustmentCodeFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the ERA Adjustment Codes.
        /// </summary>
        /// <param name="ProcedureCategoryId">The ERA Adjustment Codes identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteERAAdjustmentCode(Int64 ERAAdjCodeId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ERAAdjCodeId)))
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
                        BLObject<string> obj = BLLERAObj.DeleteERAAdjustmentCode(MDVUtility.ToStr(ERAAdjCodeId));
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

        private string UpdateERAAdjustmentCodeIsActive(Int64 ERAAdjCodeId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ERA Adjustment Codes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSERA dsCode = null;
                    BLObject<DSERA> obj = BLLERAObj.LoadERAAdjustmentCode(ERAAdjCodeId, 0, 0, 0, 0, null);
                    dsCode = obj.Data;
                    if (dsCode.Tables[dsCode.ERAAdjustmentCode.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCode.Tables[dsCode.ERAAdjustmentCode.TableName].Rows[0];
                        dr[dsCode.ERAAdjustmentCode.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSERA> objUser = BLLERAObj.UpdateERAAdjustmentCode(dsCode);
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_ERAADJUSTMENT_CODES":
                    {
                        string fieldsJson = context.Request["ERAAdjustmentCodesData"];
                        string strJsonData = SaveERAAdjustmentCode(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_ERAADJUSTMENT_CODES":
                    {
                        string strERAAdjCodeId = context.Request["ERAAdjustmentCodesID"];
                        string strJSONData = FillERAAdjustmentCode(MDVUtility.ToInt64(strERAAdjCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_ERAADJUSTMENT_CODES":
                    {
                        string strERAAdjCodeId = context.Request["ERAAdjCodeId"];
                        string strJSONData = DeleteERAAdjustmentCode(MDVUtility.ToInt64(strERAAdjCodeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ERAADJUSTMENT_CODES":
                    {
                        string fieldsJSON = context.Request["ERAAdjustmentCodesData"];
                        Int64 ERAAdjCodeId = MDVUtility.ToInt64(context.Request["ERAAdjustmentCodesID"]);
                        string strJSONData = UpdateERAAdjustmentCode(fieldsJSON, ERAAdjCodeId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ERAADJUSTMENT_CODES_ACTIVE_INACTIVE":
                    {
                        Int64 ERAAdjCodeId = MDVUtility.ToInt64(context.Request["ERAAdjustmentCodesID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateERAAdjustmentCodeIsActive(ERAAdjCodeId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}