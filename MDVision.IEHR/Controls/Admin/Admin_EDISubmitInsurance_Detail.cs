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
    public class Admin_EDISubmitInsurance_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDISubmitInsurance_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDISubmitInsurance_Detail _obj = null;
        public static Admin_EDISubmitInsurance_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDISubmitInsurance_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the edi submit insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEDISubmitInsurance(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.EDISubmitInsuranceRow dr = dsEDI.EDISubmitInsurance.NewEDISubmitInsuranceRow();

                    dr.SubmitInsuranceName = SearchedfieldsJSON["txtSubmitInsuranceName"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                    dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                    dr.AdmissionDateRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkAdmissionDateRequired"]) == "True" ? true : false;
                    dr.AnesthesiaByMins = MDVUtility.ToStr(SearchedfieldsJSON["chkAnesthesiaByMinutes"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.EDISubmitInsurance.AddEDISubmitInsuranceRow(dr);
                    BLObject<DSEDI> obj = BLLAdminEDIObj.InsertEDISubmitInsurance(dsEDI);
                    dsEDI = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDISubmitInsuranceId = dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows[0][dsEDI.EDISubmitInsurance.EDISubmitIDColumn.ColumnName]
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
        /// Updates the edi submit insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDISubmitInsuranceId">The edi submit insurance identifier.</param>
        /// <returns></returns>
        private string UpdateEDISubmitInsurance(string fieldsJSON, Int64 EDISubmitInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //DSEDI.EDISubmitInsuranceRow dr = dsEDI.EDISubmitInsurance.NewEDISubmitInsuranceRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadEDISubmitInsurance(EDISubmitInsuranceId, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.EDISubmitInsuranceRow dr in dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows)
                    {
                        //dr.EDISubmitID = EDISubmitInsuranceId;
                        dr.SubmitInsuranceName = SearchedfieldsJSON["txtSubmitInsuranceName"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                        dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                        dr.AdmissionDateRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkAdmissionDateRequired"]) == "True" ? true : false;
                        dr.AnesthesiaByMins = MDVUtility.ToStr(SearchedfieldsJSON["chkAnesthesiaByMinutes"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEDI.EDISubmitInsurance.AddEDISubmitInsuranceRow(dr);
                    //dsEDI.EDISubmitInsurance.AcceptChanges();

                    if (dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows.Count > 0)
                    {
                        //dsEDI.EDISubmitInsurance.Rows[0].SetModified();
                        BLObject<DSEDI> obj = BLLAdminEDIObj.UpdateEDISubmitInsurance(dsEDI);
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
        /// Fills the edi submit insurance.
        /// </summary>
        /// <param name="EDISubmitInsuranceId">The edi submit insurance identifier.</param>
        /// <returns></returns>
        private string FillEDISubmitInsurance(Int64 EDISubmitInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDISubmitInsuranceId)))
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
                        BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDISubmitInsurance(EDISubmitInsuranceId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsEDI = obj.Data;
                            if (dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlClearinghouse", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.ClearingHouseIdColumn.ColumnName])},
                            { "txtSubmitInsuranceName", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.SubmitInsuranceNameColumn.ColumnName])},
                            { "txtPayorId", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.PayorIdColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.PhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.PhoneExtColumn.ColumnName])},
                            { "chkAdmissionDateRequired", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.AdmissionDateRequiredColumn.ColumnName])},
                            { "chkAnesthesiaByMinutes", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.AnesthesiaByMinsColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsEDI.EDISubmitInsurance.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    EDISubmitInsuranceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the edi submit insurance.
        /// </summary>
        /// <param name="EDISubmitInsuranceId">The edi submit insurance identifier.</param>
        /// <returns></returns>
        private string DeleteEDISubmitInsurance(Int64 EDISubmitInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDISubmitInsuranceId)))
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
                        BLObject<string> obj = BLLAdminEDIObj.DeleteEDISubmitInsurance(MDVUtility.ToStr(EDISubmitInsuranceId));
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
        /// Updates the edi submit insurance is active.
        /// </summary>
        /// <param name="EDISubmitInsuranceId">The edi submit insurance identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateEDISubmitInsuranceIsActive(Int64 EDISubmitInsuranceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Submit Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDISubmitInsurance(EDISubmitInsuranceId, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.EDISubmitInsurance.TableName].Rows[0];
                        dr[dsEDI.EDISubmitInsurance.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objEDISubmitInsurance = BLLAdminEDIObj.UpdateEDISubmitInsurance(dsEDI);
                        string successMsg;
                        if (objEDISubmitInsurance.Data != null)
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
                                Message = objEDISubmitInsurance.Message
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
        /// Handle the EDI Submit Insurance Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_EDI_SUBMIT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDISubmitInsuranceData"];
                        string strJSONData = SaveEDISubmitInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EDI_SUBMIT_INSURANCE":
                    {
                        string EDISubmitInsuranceID = context.Request["EDISubmitInsuranceID"];
                        string strJSONData = FillEDISubmitInsurance(MDVUtility.ToInt64(EDISubmitInsuranceID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_SUBMIT_INSURANCE":
                    {
                        string EDISubmitInsuranceId = context.Request["EDISubmitInsuranceID"];
                        string strJSONData = DeleteEDISubmitInsurance(MDVUtility.ToInt64(EDISubmitInsuranceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SUBMIT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDISubmitInsuranceData"];
                        Int64 EDISubmitInsuranceID = MDVUtility.ToInt64(context.Request["EDISubmitInsuranceID"]);
                        string strJSONData = UpdateEDISubmitInsurance(fieldsJSON, EDISubmitInsuranceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SUBMIT_INSURANCE_ACTIVE_INACTIVE":
                    {
                        Int64 EDISubmitInsuranceID = MDVUtility.ToInt64(context.Request["EDISubmitInsuranceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDISubmitInsuranceIsActive(EDISubmitInsuranceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}