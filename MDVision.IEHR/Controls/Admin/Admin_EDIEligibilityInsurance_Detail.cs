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
    public class Admin_EDIEligibilityInsurance_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIEligibilityInsurance_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIEligibilityInsurance_Detail _obj = null;
        public static Admin_EDIEligibilityInsurance_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIEligibilityInsurance_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the edi eligibility insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEDIEligibilityInsurance(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.EDIEligibilityInsuranceRow dr = dsEDI.EDIEligibilityInsurance.NewEDIEligibilityInsuranceRow();

                    dr.EligibilityInsuranceName = SearchedfieldsJSON["txtEligibilityInsuranceName"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                    dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                    dr.IsSubscriber = MDVUtility.ToStr(SearchedfieldsJSON["chkSubscriber"]) == "True" ? true : false;
                    dr.IsDependent = MDVUtility.ToStr(SearchedfieldsJSON["chkDependent"]) == "True" ? true : false;
                    dr.TaxId = MDVUtility.ToStr(SearchedfieldsJSON["chkTaxID"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsEDI.EDIEligibilityInsurance.AddEDIEligibilityInsuranceRow(dr);
                    BLObject<DSEDI> obj = BLLAdminEDIObj.InsertEDIEligibilityInsurance(dsEDI);
                    dsEDI = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            EDIEligibilityInsuranceId = dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows[0][dsEDI.EDIEligibilityInsurance.EDIEligibilityIDColumn.ColumnName]
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
        /// Updates the edi eligibility insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIEligibilityInsuranceId">The edi eligibility insurance identifier.</param>
        /// <returns></returns>
        private string UpdateEDIEligibilityInsurance(string fieldsJSON, Int64 EDIEligibilityInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //DSEDI.EDIEligibilityInsuranceRow dr = dsEDI.EDIEligibilityInsurance.NewEDIEligibilityInsuranceRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadEDIEligibilityInsurance(EDIEligibilityInsuranceId, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.EDIEligibilityInsuranceRow dr in dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows)
                    {
                        //dr.EDIEligibilityID = EDIEligibilityInsuranceId;
                        dr.EligibilityInsuranceName = SearchedfieldsJSON["txtEligibilityInsuranceName"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearinghouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearinghouse"]);
                        dr.PayorId = SearchedfieldsJSON["txtPayorId"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                        dr.IsSubscriber = MDVUtility.ToStr(SearchedfieldsJSON["chkSubscriber"]) == "True" ? true : false;
                        dr.IsDependent = MDVUtility.ToStr(SearchedfieldsJSON["chkDependent"]) == "True" ? true : false;
                        dr.TaxId = MDVUtility.ToStr(SearchedfieldsJSON["chkTaxID"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsEDI.EDIEligibilityInsurance.AddEDIEligibilityInsuranceRow(dr);
                    //dsEDI.EDIEligibilityInsurance.AcceptChanges();

                    if (dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows.Count > 0)
                    {
                        //dsEDI.EDIEligibilityInsurance.Rows[0].SetModified();
                        BLObject<DSEDI> obj = BLLAdminEDIObj.UpdateEDIEligibilityInsurance(dsEDI);
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
        /// Fills the edi eligibility insurance.
        /// </summary>
        /// <param name="EDIEligibilityInsuranceId">The edi eligibility insurance identifier.</param>
        /// <returns></returns>
        private string FillEDIEligibilityInsurance(Int64 EDIEligibilityInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIEligibilityInsuranceId)))
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
                        BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIEligibilityInsurance(EDIEligibilityInsuranceId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsEDI = obj.Data;
                            if (dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlClearinghouse", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.ClearingHouseIdColumn.ColumnName])},
                            { "txtEligibilityInsuranceName", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.EligibilityInsuranceNameColumn.ColumnName])},
                            { "txtPayorId", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.PayorIdColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.PhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.PhoneExtColumn.ColumnName])},
                            { "chkTaxID", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.TaxIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.IsActiveColumn.ColumnName])},
                            { "chkSubscriber", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.IsSubscriberColumn.ColumnName])},
                            { "chkDependent", MDVUtility.ToStr(dr[dsEDI.EDIEligibilityInsurance.IsDependentColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    EDIEligibilityInsuranceFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the edi eligibility insurance.
        /// </summary>
        /// <param name="EDIEligibilityInsuranceId">The edi eligibility insurance identifier.</param>
        /// <returns></returns>
        private string DeleteEDIEligibilityInsurance(Int64 EDIEligibilityInsuranceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIEligibilityInsuranceId)))
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
                        BLObject<string> obj = BLLAdminEDIObj.DeleteEDIEligibilityInsurance(MDVUtility.ToStr(EDIEligibilityInsuranceId));
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
        /// Updates the edi eligibility insurance is active.
        /// </summary>
        /// <param name="EDIEligibilityInsuranceId">The edi eligibility insurance identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateEDIEligibilityInsuranceIsActive(Int64 EDIEligibilityInsuranceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Eligibility Insurance", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIEligibilityInsurance(EDIEligibilityInsuranceId, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.EDIEligibilityInsurance.TableName].Rows[0];
                        dr[dsEDI.EDIEligibilityInsurance.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objEDIEligibilityInsurance = BLLAdminEDIObj.UpdateEDIEligibilityInsurance(dsEDI);
                        string successMsg;
                        if (objEDIEligibilityInsurance.Data != null)
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
                                Message = objEDIEligibilityInsurance.Message
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
        /// Handle the EDI Eligibility Insurance Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_EDI_ELIGIBILITY_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIEligibilityInsuranceData"];
                        string strJSONData = SaveEDIEligibilityInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EDI_ELIGIBILITY_INSURANCE":
                    {
                        string EDIEligibilityInsuranceID = context.Request["EDIEligibilityInsuranceID"];
                        string strJSONData = FillEDIEligibilityInsurance(MDVUtility.ToInt64(EDIEligibilityInsuranceID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_ELIGIBILITY_INSURANCE":
                    {
                        string EDIEligibilityInsuranceID = context.Request["EDIEligibilityInsuranceID"];
                        string strJSONData = DeleteEDIEligibilityInsurance(MDVUtility.ToInt64(EDIEligibilityInsuranceID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_ELIGIBILITY_INSURANCE":
                    {
                        string fieldsJSON = context.Request["EDIEligibilityInsuranceData"];
                        Int64 EDIEligibilityInsuranceID = MDVUtility.ToInt64(context.Request["EDIEligibilityInsuranceID"]);
                        string strJSONData = UpdateEDIEligibilityInsurance(fieldsJSON, EDIEligibilityInsuranceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_ELIGIBILITY_INSURANCE_ACTIVE_INACTIVE":
                    {
                        Int64 EDIEligibilityInsuranceID = MDVUtility.ToInt64(context.Request["EDIEligibilityInsuranceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDIEligibilityInsuranceIsActive(EDIEligibilityInsuranceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}