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
    public class Admin_EDIClaimStatusInsurance_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIClaimStatusInsurance_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIClaimStatusInsurance_Detail _obj = null;
        public static Admin_EDIClaimStatusInsurance_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIClaimStatusInsurance_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the edi claim status insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEDIClaimStatusInsurance(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.EDIClaimStatusInsuranceRow dr = dsEDI.EDIClaimStatusInsurance.NewEDIClaimStatusInsuranceRow();

                    dr.EDIStatusInsurance = SearchedfieldsJSON["txtClaimStatusInsurance"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                    dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                    dr.TaxId = MDVUtility.ToStr(SearchedfieldsJSON["chkTaxID"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.EDIClaimStatusInsurance.AddEDIClaimStatusInsuranceRow(dr);
                    BLObject<DSEDI> obj = BLLAdminEDIObj.InsertEDIClaimStatusInsurance(dsEDI);
                    dsEDI = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDIClaimStatusInsuranceId = dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows[0][dsEDI.EDIClaimStatusInsurance.EDIClaimStatusIDColumn.ColumnName]
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
        /// Updates the edi claim status insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIClaimStatusInsuranceId">The edi claim status insurance identifier.</param>
        /// <returns></returns>
        private string UpdateEDIClaimStatusInsurance(string fieldsJSON, Int64 EDIClaimStatusInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //DSEDI.EDIClaimStatusInsuranceRow dr = dsEDI.EDIClaimStatusInsurance.NewEDIClaimStatusInsuranceRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadEDIClaimStatusInsurance(EDIClaimStatusInsuranceId, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.EDIClaimStatusInsuranceRow dr in dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows)
                    {
                        //dr.EDIClaimStatusID = EDIClaimStatusInsuranceId;
                        dr.EDIStatusInsurance = SearchedfieldsJSON["txtClaimStatusInsurance"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                        dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                        dr.TaxId = MDVUtility.ToStr(SearchedfieldsJSON["chkTaxID"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEDI.EDIClaimStatusInsurance.AddEDIClaimStatusInsuranceRow(dr);
                    //dsEDI.EDIClaimStatusInsurance.AcceptChanges();

                    if (dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows.Count > 0)
                    {
                        //dsEDI.EDIClaimStatusInsurance.Rows[0].SetModified();
                        BLObject<DSEDI> obj = BLLAdminEDIObj.UpdateEDIClaimStatusInsurance(dsEDI);
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
        /// Fills the edi claim status insurance.
        /// </summary>
        /// <param name="EDIClaimStatusInsuranceId">The edi claim status insurance identifier.</param>
        /// <returns></returns>
        private string FillEDIClaimStatusInsurance(Int64 EDIClaimStatusInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIClaimStatusInsuranceId)))
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
                        DSEDI dsEDI = null;
                        BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIClaimStatusInsurance(EDIClaimStatusInsuranceId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsEDI = obj.Data;
                            if (dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlClearinghouse", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.ClearingHouseIdColumn.ColumnName])},
                            { "txtClaimStatusInsurance", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.EDIStatusInsuranceColumn.ColumnName])},
                            { "txtPayorId", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.PayorIdColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.PhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.PhoneExtColumn.ColumnName])},
                            { "chkTaxID", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.TaxIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsEDI.EDIClaimStatusInsurance.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    EDIClaimStatusInsuranceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the edi claim status insurance.
        /// </summary>
        /// <param name="EDIClaimStatusInsuranceId">The edi claim status insurance identifier.</param>
        /// <returns></returns>
        private string DeleteEDIClaimStatusInsurance(Int64 EDIClaimStatusInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIClaimStatusInsuranceId)))
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
                        BLObject<string> obj = BLLAdminEDIObj.DeleteEDIClaimStatusInsurance(MDVUtility.ToStr(EDIClaimStatusInsuranceId));
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

        /// <summary>
        /// Updates the edi claim status insurance is active.
        /// </summary>
        /// <param name="EDIClaimStatusInsuranceId">The edi claim status insurance identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateEDIClaimStatusInsuranceIsActive(Int64 EDIClaimStatusInsuranceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Claim Status Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIClaimStatusInsurance(EDIClaimStatusInsuranceId, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.EDIClaimStatusInsurance.TableName].Rows[0];
                        dr[dsEDI.EDIClaimStatusInsurance.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objEDIClaimStatusInsurance = BLLAdminEDIObj.UpdateEDIClaimStatusInsurance(dsEDI);
                        string successMsg;
                        if (objEDIClaimStatusInsurance.Data != null)
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
                                Message = objEDIClaimStatusInsurance.Message
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
        /// Handle the EDI Claim Status Insurance Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_EDI_CLAIM_STATUS_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIClaimStatusInsuranceData"];
                        string strJSONData = SaveEDIClaimStatusInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EDI_CLAIM_STATUS_INSURANCE":
                    {
                        string EDIClaimStatusInsuranceID = context.Request["EDIClaimStatusInsuranceID"];
                        string strJSONData = FillEDIClaimStatusInsurance(MDVUtility.ToInt64(EDIClaimStatusInsuranceID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_CLAIM_STATUS_INSURANCE":
                    {
                        string EDIClaimStatusInsuranceID = context.Request["EDIClaimStatusInsuranceID"];
                        string strJSONData = DeleteEDIClaimStatusInsurance(MDVUtility.ToInt64(EDIClaimStatusInsuranceID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_CLAIM_STATUS_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIClaimStatusInsuranceData"];
                        Int64 EDIClaimStatusInsuranceID = MDVUtility.ToInt64(context.Request["EDIClaimStatusInsuranceID"]);
                        string strJSONData = UpdateEDIClaimStatusInsurance(fieldsJSON, EDIClaimStatusInsuranceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_CLAIM_STATUS_INSURANCE_ACTIVE_INACTIVE":
                    {
                        Int64 EDIClaimStatusInsuranceID = MDVUtility.ToInt64(context.Request["EDIClaimStatusInsuranceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDIClaimStatusInsuranceIsActive(EDIClaimStatusInsuranceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}