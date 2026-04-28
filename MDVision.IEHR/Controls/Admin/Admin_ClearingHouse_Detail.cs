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
    public class Admin_ClearingHouse_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_ClearingHouse_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_ClearingHouse_Detail _obj = null;
        public static Admin_ClearingHouse_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ClearingHouse_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the clearing house.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveClearingHouse(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    DSEDI.ClearingHouseRow dr = dsEDI.ClearingHouse.NewClearingHouseRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouseType"]))
                        dr.TypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouseType"]);
                    dr.FTP = SearchedfieldsJSON["txtFTP"];
                    dr.ClaimStatusAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimStatusAllowed"]) == "True" ? true : false;
                    dr.ClaimSubmitAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimSubmitAllowed"]) == "True" ? true : false;
                    dr.EligibilityAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkEligibilityAllowed"]) == "True" ? true : false;
                    dr.ElectronicEOBAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronicEOBAllowed"]) == "True" ? true : false;
                    dr.SecondaryAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkSecondaryAllowed"]) == "True" ? true : false;

                dr.IN_UPLOADED = MDVUtility.ToStr(SearchedfieldsJSON["txtINUpload"]);
                dr.IN_STATEMENTS = MDVUtility.ToStr(SearchedfieldsJSON["txtINStatements"]);
                dr.OUT_REPORTS = MDVUtility.ToStr(SearchedfieldsJSON["txtOUTReports"]);
                dr.OUT_277 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT277"]);
                dr.OUT_271 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT271"]);
                dr.OUT_997 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT997"]);
                dr.OUT_835 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT835"]);
                dr.FTP_PORTNO = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPPortNo"]);
                dr.URL = MDVUtility.ToStr(SearchedfieldsJSON["txtURL"]);
                dr.FTP_HOSTKEY = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPHostKey"]);

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.UserName = SearchedfieldsJSON["txtClearingHoueUserName"];
                    dr.UserPassword = SearchedfieldsJSON["txtClearingHoueUserPassword"];

                    dr.PatientEligibilityUserName = SearchedfieldsJSON["txtPatientEligibilityUserName"];
                    dr.PatientEligibilityUserPassword = SearchedfieldsJSON["txtPatientEligibilityUserPassword"];

                    dr.ProfessionalClaimExtension = SearchedfieldsJSON["ddlProfessionalClaimExtension"];
                    dr.InstitutionalClaimExtension = SearchedfieldsJSON["ddlInstitutionalClaimExtension"];
                    dr.PatientStatementExtension = SearchedfieldsJSON["ddlPatientStatementExtension"];

                    #region Database Insertion
                    dsEDI.ClearingHouse.AddClearingHouseRow(dr);
                    BLObject<DSEDI> objClearingHouse = BLLAdminEDIObj.InsertClearingHouse(dsEDI);
                    dsEDI = objClearingHouse.Data;
                    if (objClearingHouse.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ClearingHouseId = dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows[0][dsEDI.ClearingHouse.ClearingHouseIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objClearingHouse.Message
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
        /// Updates the clearing house.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ClearingHouseId">The clearing house identifier.</param>
        /// <returns></returns>
        private string UpdateClearingHouse(string fieldsJSON, Int64 ClearingHouseId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    //DSEDI.ClearingHouseRow dr = dsEDI.ClearingHouse.NewClearingHouseRow();
                    BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadClearingHouse(ClearingHouseId, null, null, null, null);
                    dsEDI = objLoad.Data;
                    foreach (DSEDI.ClearingHouseRow dr in dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows)
                    {
                        //dr.ClearingHouseId = ClearingHouseId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouseType"]))
                            dr.TypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouseType"]);
                        dr.FTP = SearchedfieldsJSON["txtFTP"];
                        dr.ClaimStatusAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimStatusAllowed"]) == "True" ? true : false;
                        dr.ClaimSubmitAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimSubmitAllowed"]) == "True" ? true : false;
                        dr.EligibilityAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkEligibilityAllowed"]) == "True" ? true : false;
                        dr.ElectronicEOBAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronicEOBAllowed"]) == "True" ? true : false;
                        dr.SecondaryAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkSecondaryAllowed"]) == "True" ? true : false;

                    dr.IN_UPLOADED = MDVUtility.ToStr(SearchedfieldsJSON["txtINUpload"]);
                    dr.IN_STATEMENTS = MDVUtility.ToStr(SearchedfieldsJSON["txtINStatements"]);
                    dr.OUT_REPORTS = MDVUtility.ToStr(SearchedfieldsJSON["txtOUTReports"]);
                    dr.OUT_277 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT277"]);
                    dr.OUT_271 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT271"]);
                    dr.OUT_997 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT997"]);
                    dr.OUT_835 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT835"]);
                    dr.FTP_PORTNO = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPPortNo"]);
                    dr.URL = MDVUtility.ToStr(SearchedfieldsJSON["txtURL"]);

                    dr.FTP_HOSTKEY = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPHostKey"]);

                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.UserName = SearchedfieldsJSON["txtClearingHoueUserName"];
                        dr.UserPassword = SearchedfieldsJSON["txtClearingHoueUserPassword"];

                        dr.PatientEligibilityUserName = SearchedfieldsJSON["txtPatientEligibilityUserName"];
                        dr.PatientEligibilityUserPassword = SearchedfieldsJSON["txtPatientEligibilityUserPassword"];

                        dr.ProfessionalClaimExtension = SearchedfieldsJSON["ddlProfessionalClaimExtension"];
                        dr.InstitutionalClaimExtension = SearchedfieldsJSON["ddlInstitutionalClaimExtension"];
                        dr.PatientStatementExtension = SearchedfieldsJSON["ddlPatientStatementExtension"];
                    }

                    #region Database Updation
                    //dsEDI.ClearingHouse.AddClearingHouseRow(dr);
                    //dsEDI.ClearingHouse.AcceptChanges();

                    if (dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows.Count > 0)
                    {
                        //dsEDI.ClearingHouse.Rows[0].SetModified();
                        BLObject<DSEDI> objClearingHouse = BLLAdminEDIObj.UpdateClearingHouse(dsEDI);
                        dsEDI = objClearingHouse.Data;
                        if (objClearingHouse.Data != null)
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
                                Message = objClearingHouse.Message
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
        /// Fills the clearing house.
        /// </summary>
        /// <param name="ClearingHouseId">The clearing house identifier.</param>
        /// <returns></returns>
        private string FillClearingHouse(Int64 ClearingHouseId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ClearingHouseId)))
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
                        BLObject<DSEDI> objClearingHouse = BLLAdminEDIObj.LoadClearingHouse(ClearingHouseId, null, null, null, null);
                        if (objClearingHouse.Data != null)
                        {
                            dsEDI = objClearingHouse.Data;
                            if (dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.ShortNameColumn.ColumnName])},
                            { "ddlClearingHouseType", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.TypeIdColumn.ColumnName])},
                            { "chkClaimSubmitAllowed", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.ClaimSubmitAllowedColumn.ColumnName])},
                            { "chkEligibilityAllowed", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.EligibilityAllowedColumn.ColumnName])},
                            { "chkClaimStatusAllowed", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.ClaimStatusAllowedColumn.ColumnName])},
                            { "chkElectronicEOBAllowed", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.ElectronicEOBAllowedColumn.ColumnName])},
                            { "chkSecondaryAllowed", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.SecondaryAllowedColumn.ColumnName])},
                            { "txtFTP", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.FTPColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.EntityIdColumn.ColumnName])},
                            { "txtClearingHoueUserName", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.UserNameColumn.ColumnName])},
                            { "txtClearingHoueUserPassword", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.UserPasswordColumn.ColumnName])},

                            { "txtPatientEligibilityUserName", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.PatientEligibilityUserNameColumn.ColumnName])},
                            { "txtPatientEligibilityUserPassword", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.PatientEligibilityUserPasswordColumn.ColumnName])},

                            { "ddlProfessionalClaimExtension", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.ProfessionalClaimExtensionColumn.ColumnName])},
                            { "ddlInstitutionalClaimExtension", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.InstitutionalClaimExtensionColumn.ColumnName])},
                            { "ddlPatientStatementExtension", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.PatientStatementExtensionColumn.ColumnName])},

                            { "txtINUpload", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.IN_UPLOADEDColumn.ColumnName])},
                            { "txtINStatements", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.IN_STATEMENTSColumn.ColumnName])},
                            { "txtOUTReports", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.OUT_REPORTSColumn.ColumnName])},
                            { "txtOUT277", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.OUT_277Column.ColumnName])},
                            { "txtOUT271", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.OUT_271Column.ColumnName])},
                            { "txtOUT997", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.OUT_997Column.ColumnName])},
                            { "txtOUT835", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.OUT_835Column.ColumnName])},
                            { "txtFTPPortNo", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.FTP_PORTNOColumn.ColumnName])},
                            { "txtURL", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.URLColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.IsActiveColumn.ColumnName])},
                            { "txtFTPHostKey", MDVUtility.ToStr(dr[dsEDI.ClearingHouse.FTP_HOSTKEYColumn.ColumnName])}

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ClearingHouseFill_JSON = js.Serialize(keyValues)
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
                                Message = objClearingHouse.Message
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
        /// Deletes the clearing house.
        /// </summary>
        /// <param name="ClearingHouseId">The clearing house identifier.</param>
        /// <returns></returns>
        private string DeleteClearingHouse(Int64 ClearingHouseId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ClearingHouseId)))
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
                        BLObject<string> objClearingHouse = BLLAdminEDIObj.DeleteClearingHouse(MDVUtility.ToStr(ClearingHouseId));

                        if (objClearingHouse.Data == "")
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
                                Message = objClearingHouse.Data
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

        private string UpdateClearingHouseIsActive(Int64 ClearingHouseId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clearinghouse", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadClearingHouse(ClearingHouseId, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.ClearingHouse.TableName].Rows[0];
                        dr[dsEDI.ClearingHouse.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objClearingHouse = BLLAdminEDIObj.UpdateClearingHouse(dsEDI);
                        string successMsg;
                        if (objClearingHouse.Data != null)
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
                                Message = objClearingHouse.Message
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
        /// Handle the Clearing House Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_CLEARING_HOUSE":
                    {
                        string fieldsJSON = context.Request["ClearingHouseData"];
                        string strJSONData = SaveClearingHouse(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CLEARING_HOUSE":
                    {
                        string strClearingHouseId = context.Request["ClearingHouseID"];
                        string strJSONData = FillClearingHouse(MDVUtility.ToInt64(strClearingHouseId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_CLEARING_HOUSE":
                    {
                        string strClearingHouseId = context.Request["ClearingHouseID"];
                        string strJSONData = DeleteClearingHouse(MDVUtility.ToInt64(strClearingHouseId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CLEARING_HOUSE":
                    {
                        string fieldsJSON = context.Request["ClearingHouseData"];
                        Int64 ClearingHouseID = MDVUtility.ToInt64(context.Request["ClearingHouseID"]);
                        string strJSONData = UpdateClearingHouse(fieldsJSON, ClearingHouseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CLEARING_HOUSE_ACTIVE_INACTIVE":
                    {
                        Int64 ClearingHouseID = MDVUtility.ToInt64(context.Request["ClearingHouseID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateClearingHouseIsActive(ClearingHouseID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}