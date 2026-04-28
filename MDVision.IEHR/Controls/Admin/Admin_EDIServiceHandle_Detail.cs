using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using Newtonsoft.Json;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_EDIServiceHandle_Detail
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIServiceHandle_Detail()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIServiceHandle_Detail _obj = null;
        public static Admin_EDIServiceHandle_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIServiceHandle_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the clearing house.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>

        private string SaveEDIServiceHandle(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSEDI dsEDI = new DSEDI();
                DSEDI.EDIServiceHandleRow dr = dsEDI.EDIServiceHandle.NewEDIServiceHandleRow();

                dr.EDIServiceHandleID = -1;

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                    dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                    dr.Case = MDVUtility.ToStr(SearchedfieldsJSON["ddlCase"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInterval"]))
                    dr.Mode = MDVUtility.ToStr(SearchedfieldsJSON["ddlInterval"]);

                dr.Time = SearchedfieldsJSON["txtTime"].Trim();

                dr.IntervalHours = SearchedfieldsJSON["txtHours"].Trim();
                dr.IntervalMinutes = SearchedfieldsJSON["txtMinutes"].Trim();


                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                #region Database Insertion
                dsEDI.EDIServiceHandle.AddEDIServiceHandleRow(dr);
                BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.InsertEDIServiceHandle(dsEDI);
                dsEDI = objEDIServiceHandle.Data;
                if (objEDIServiceHandle.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        EDIServiceHandleId = dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows[0][dsEDI.EDIServiceHandle.EDIServiceHandleIDColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objEDIServiceHandle.Message
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
        /// <summary>
        /// Updates the clearing house.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EDIServiceHandleId">The clearing house identifier.</param>
        /// <returns></returns>
        //private string UpdateEDIServiceHandle(string fieldsJSON, Int64 EDIServiceHandleId)
        //{
        //    try
        //    {
        //        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

        //        DSEDI dsEDI = new DSEDI();
        //        //DSEDI.EDIServiceHandleRow dr = dsEDI.EDIServiceHandle.NewEDIServiceHandleRow();
        //        BLObject<DSEDI> objLoad = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null);
        //        dsEDI = objLoad.Data;
        //        foreach (DSEDI.EDIServiceHandleRow dr in dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows)
        //        {
        //            //dr.EDIServiceHandleId = EDIServiceHandleId;
        //            dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
        //            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEDIServiceHandleType"]))
        //                dr.TypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEDIServiceHandleType"]);
        //            dr.FTP = SearchedfieldsJSON["txtFTP"];
        //            dr.ClaimStatusAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimStatusAllowed"]) == "True" ? true : false;
        //            dr.ClaimSubmitAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkClaimSubmitAllowed"]) == "True" ? true : false;
        //            dr.EligibilityAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkEligibilityAllowed"]) == "True" ? true : false;
        //            dr.ElectronicEOBAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkElectronicEOBAllowed"]) == "True" ? true : false;
        //            dr.SecondaryAllowed = MDVUtility.ToStr(SearchedfieldsJSON["chkSecondaryAllowed"]) == "True" ? true : false;

        //            dr.IN_UPLOADED = MDVUtility.ToStr(SearchedfieldsJSON["txtINUpload"]);
        //            dr.IN_STATEMENTS = MDVUtility.ToStr(SearchedfieldsJSON["txtINStatements"]);
        //            dr.OUT_REPORTS = MDVUtility.ToStr(SearchedfieldsJSON["txtOUTReports"]);
        //            dr.OUT_277 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT277"]);
        //            dr.OUT_271 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT271"]);
        //            dr.OUT_997 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT997"]);
        //            dr.OUT_835 = MDVUtility.ToStr(SearchedfieldsJSON["txtOUT835"]);
        //            dr.FTP_PORTNO = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPPortNo"]);
        //            dr.URL = MDVUtility.ToStr(SearchedfieldsJSON["txtURL"]);
        //            //dr.FTP_HOSTKEY = MDVUtility.ToStr(SearchedfieldsJSON["txtFTPHostKey"]);

        //            //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            //dr.CreatedOn = DateTime.Now;
        //            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
        //            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.ModifiedOn = DateTime.Now;
        //            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
        //                dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
        //            dr.UserName = SearchedfieldsJSON["txtClearingHoueUserName"];
        //            dr.UserPassword = SearchedfieldsJSON["txtClearingHoueUserPassword"];
        //        }

        //        #region Database Updation
        //        //dsEDI.EDIServiceHandle.AddEDIServiceHandleRow(dr);
        //        //dsEDI.EDIServiceHandle.AcceptChanges();

        //        if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
        //        {
        //            //dsEDI.EDIServiceHandle.Rows[0].SetModified();
        //            BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.UpdateEDIServiceHandle(dsEDI);
        //            dsEDI = objEDIServiceHandle.Data;
        //            if (objEDIServiceHandle.Data != null)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Update_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objEDIServiceHandle.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = ""
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        /// <summary>
        /// Fills the clearing house.
        /// </summary>
        /// <param name="EDIServiceHandleId">The clearing house identifier.</param>
        /// <returns></returns>
        //private string FillEDIServiceHandle(Int64 EDIServiceHandleId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIServiceHandleId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            DSEDI dsEDI = null;
        //            BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null);
        //            dsEDI = objEDIServiceHandle.Data;
        //            if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
        //            {
        //                DataRow dr = dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows[0];
        //                var keyValues = new Dictionary<string, string>
        //                {
        //                    { "txtShortName", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ShortNameColumn.ColumnName])},
        //                    { "ddlEDIServiceHandleType", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.TypeIdColumn.ColumnName])},
        //                    { "chkClaimSubmitAllowed", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ClaimSubmitAllowedColumn.ColumnName])},
        //                    { "chkEligibilityAllowed", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.EligibilityAllowedColumn.ColumnName])},
        //                    { "chkClaimStatusAllowed", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ClaimStatusAllowedColumn.ColumnName])},
        //                    { "chkElectronicEOBAllowed", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ElectronicEOBAllowedColumn.ColumnName])},
        //                    { "chkSecondaryAllowed", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.SecondaryAllowedColumn.ColumnName])},
        //                    { "txtFTP", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.FTPColumn.ColumnName])},
        //                    { "ddlEntity", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.EntityIdColumn.ColumnName])},
        //                    { "txtClearingHoueUserName", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.UserNameColumn.ColumnName])},
        //                    { "txtClearingHoueUserPassword", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.UserPasswordColumn.ColumnName])},

        //                    { "txtINUpload", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IN_UPLOADEDColumn.ColumnName])},
        //                    { "txtINStatements", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IN_STATEMENTSColumn.ColumnName])},
        //                    { "txtOUTReports", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.OUT_REPORTSColumn.ColumnName])},
        //                    { "txtOUT277", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.OUT_277Column.ColumnName])},
        //                    { "txtOUT271", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.OUT_271Column.ColumnName])},
        //                    { "txtOUT997", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.OUT_997Column.ColumnName])},
        //                    { "txtOUT835", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.OUT_835Column.ColumnName])},
        //                    { "txtFTPPortNo", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.FTP_PORTNOColumn.ColumnName])},
        //                    { "txtURL", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.URLColumn.ColumnName])},
        //                    { "chkIsActive", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IsActiveColumn.ColumnName])}
        //                    //{ "txtFTPHostKey", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.FTP_HOSTKEYColumn.ColumnName])}

        //                };
        //                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //                var response = new
        //                {
        //                    status = true,
        //                    EDIServiceHandleFill_JSON = js.Serialize(keyValues)
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        /// <summary>
        /// Deletes the clearing house.
        /// </summary>
        /// <param name="EDIServiceHandleId">The clearing house identifier.</param>
        /// <returns></returns>
        //private string DeleteEDIServiceHandle(Int64 EDIServiceHandleId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIServiceHandleId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            BLObject<string> objEDIServiceHandle = BLLAdminEDIObj.DeleteEDIServiceHandle(MDVUtility.ToStr(EDIServiceHandleId));

        //            if (objEDIServiceHandle.Data == "")
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    Message = Common.AppPrivileges.Delete_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objEDIServiceHandle.Data
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        //private string UpdateEDIServiceHandleIsActive(Int64 EDIServiceHandleId, Int64 IsActive)
        //{
        //    try
        //    {
        //        DSEDI dsEDI = null;
        //        BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null);
        //        dsEDI = obj.Data;
        //        if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
        //        {
        //            DataRow dr = dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows[0];
        //            dr[dsEDI.EDIServiceHandle.IsActiveColumn.ColumnName] = IsActive;

        //            BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.UpdateEDIServiceHandle(dsEDI);
        //            string successMsg;
        //            if (objEDIServiceHandle.Data != null)
        //            {
        //                if (IsActive == 0)
        //                    successMsg = Common.AppPrivileges.Inactive_Message;
        //                else
        //                    successMsg = Common.AppPrivileges.Active_Message;
        //                var response = new
        //                {
        //                    status = true,
        //                    message = successMsg
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objEDIServiceHandle.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

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
                case "SAVE_EDI_SERVICE_HANDLE":
                    {
                        string fieldsJSON = context.Request["EDIServiceHandleData"];
                        string strJSONData = SaveEDIServiceHandle(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EDI_SERVICE_HANDLE":
                    {
                        string strEDIServiceHandleId = context.Request["EDIServiceHandleID"];
                        string strJSONData = "";//FillEDIServiceHandle(MDVUtility.ToInt64(strEDIServiceHandleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_SERVICE_HANDLE":
                    {
                        string strEDIServiceHandleId = context.Request["EDIServiceHandleID"];
                        string strJSONData = "";//DeleteEDIServiceHandle(MDVUtility.ToInt64(strEDIServiceHandleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SERVICE_HANDLE":
                    {
                        string fieldsJSON = context.Request["EDIServiceHandleData"];
                        Int64 EDIServiceHandleID = MDVUtility.ToInt64(context.Request["EDIServiceHandleID"]);
                        string strJSONData = "";//UpdateEDIServiceHandle(fieldsJSON, EDIServiceHandleID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SERVICE_HANDLE_ACTIVE_INACTIVE":
                    {
                        Int64 EDIServiceHandleID = MDVUtility.ToInt64(context.Request["EDIServiceHandleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = "";//UpdateEDIServiceHandleIsActive(EDIServiceHandleID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}