using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_EDIServiceHandle
    {
        private BLLAdminEDI BLLAdminEDIObj = null;
        public Admin_EDIServiceHandle()
        {
            BLLAdminEDIObj = new BLLAdminEDI();
        }
        #region Singleton
        private static Admin_EDIServiceHandle _obj = null;
        public static Admin_EDIServiceHandle Instance()
        {
            if (_obj == null)
                _obj = new Admin_EDIServiceHandle();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// LoadEDIServiceHandle
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="EDIServiceHandleID"></param>
        /// <param name="rpp"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        private string LoadEDIServiceHandle(string fieldsJson, Int64 EDIServiceHandleID, int rpp, int PageNumber)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    DSEDI dsEDIService = null;
                    BLObject<DSEDI> obj;

                    var ddlClearingHouse = searchedfieldsJson["ddlClearingHouse"] == "" ? null : searchedfieldsJson["ddlClearingHouse"];
                    var ddlEntity = searchedfieldsJson["ddlEntity"] == "" ? null : searchedfieldsJson["ddlEntity"];
                    var ddlCase = searchedfieldsJson["ddlCase"] == "" ? null : searchedfieldsJson["ddlCase"];
                    var ddlInterval = searchedfieldsJson["ddlInterval"] == "" ? null : searchedfieldsJson["ddlInterval"];
                    var activeStatus = searchedfieldsJson["chkIsActice"] == "" ? null : searchedfieldsJson["chkIsActice"];
                    // var EntitiyId = searchedfieldsJson["ddlEntity"] == "" ? null : searchedfieldsJson["ddlEntity"]; //MDVSession.Current.EntityId;

                    obj = BLLAdminEDIObj.LoadEDIServiceHandle(0, ddlEntity, ddlClearingHouse, ddlCase, ddlInterval, null, activeStatus, rpp, PageNumber);

                    dsEDIService = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsEDIService.Tables[dsEDIService.EDIServiceHandle.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                EDIServiceHandleCount = dsEDIService.Tables[dsEDIService.EDIServiceHandle.TableName].Rows.Count,
                                iTotalDisplayRecords = dsEDIService.EDIServiceHandle.Rows[0][dsEDIService.EDIServiceHandle.RecordCountColumn],
                                EDIServiceHandleLoad_JSON = MDVUtility.JSON_DataTable(dsEDIService.Tables[dsEDIService.EDIServiceHandle.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = 0,
                                iTotalDisplayRecords = 0,
                                Message = "No EDI Service Schedule Found."
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

        /// <summary>
        /// SaveEDIServiceHandle
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <returns></returns>
        private string SaveEDIServiceHandle(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
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

                    var startTime = SearchedfieldsJSON["txtTime"].Trim();
                    string AMPM = startTime.ToString();
                    long hours_, minutes_;
                    var AMPM_ = AMPM.Substring(AMPM.Length - 2);
                    if (AMPM_ == "PM")
                    {
                        //AMPM = AMPM.Substring(0, 1);
                        hours_ = long.Parse(AMPM.Substring(0, 1)) + 12;
                    }
                    else
                    {
                        hours_ = long.Parse(AMPM.Substring(0, 1));
                        //hours_ = long.Parse(AMPM);
                    }
                    if (AMPM.Substring(2, 3).Length == 3)
                        AMPM = AMPM.Substring(3, 2);
                    else
                        AMPM = AMPM.Substring(2, 3);

                    minutes_ = long.Parse(AMPM);

                    DateTime dt__ = DateTime.Parse(SearchedfieldsJSON["txtTime"].ToString());
                    var aa_ = DateTime.Now.TimeOfDay;
                    aa_ = dt__.TimeOfDay;
                    var thetime_ = aa_.Hours + ":" + aa_.Minutes + " ";

                    var min_ = aa_.Minutes.ToString().Length == 1 ? "0" + aa_.Minutes.ToString() : aa_.Minutes.ToString();
                    var hrs_ = aa_.Hours.ToString().Length == 1 ? "0" + aa_.Hours.ToString() : aa_.Hours.ToString();
                    thetime_ = hrs_.ToString() + ":" + min_.ToString() + ":00";

                    dr.Time = thetime_.ToString();//SearchedfieldsJSON["txtTime"].Trim();

                    dr.IntervalHours = SearchedfieldsJSON["txtHours"].Trim();
                    dr.IntervalMinutes = SearchedfieldsJSON["txtMinutes"].Trim();


                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);


                    ///////////////////

                    long Hours, Minutes;

                    if (dr.IntervalHours.ToString() != "") Hours = long.Parse(dr.IntervalHours.ToString()); else Hours = 0;

                    if (dr.IntervalMinutes.ToString() != "") Minutes = long.Parse(dr.IntervalMinutes); else Minutes = 0;
                    var Time = dr.Time.ToString();
                    Int64 iterator = 0;

                    if (Minutes == 0 && Hours != 0)
                        iterator = 24 / Hours;
                    else if (Hours == 0 && Minutes != 0)
                        iterator = (24 * 60) / (Minutes);
                    else if (Hours != 0 && Minutes != 0)
                        iterator = (24 * 60) / ((Hours * 60) + Minutes);


                    DataTable table = new DataTable("EDiService");
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("EntityId", typeof(string));
                    table.Columns.Add("UserId", typeof(string));
                    table.Columns.Add("ClearingHouseId", typeof(string));
                    table.Columns.Add("Case", typeof(string));
                    table.Columns.Add("Mode", typeof(string));
                    table.Columns.Add("Time", typeof(string));
                    table.Columns.Add("IntervalHours", typeof(string));
                    table.Columns.Add("IntervalMinutes", typeof(string));


                    table.Columns.Add("CreatedBy", typeof(string));
                    table.Columns.Add("CreatedOn", typeof(DateTime));
                    table.Columns.Add("ModifiedBy", typeof(string));
                    table.Columns.Add("ModifiedOn", typeof(DateTime));
                    table.Columns.Add("IsActive", typeof(string));

                    DateTime dt_ = DateTime.Parse(dr.Time.ToString());
                    var aa = DateTime.Now.TimeOfDay;
                    var thetime = "";

                    iterator = (dr.IntervalMinutes.ToString() == "" || dr.IntervalMinutes.ToString() == "0") ? iterator = (iterator - 1) : iterator = (iterator);

                    int j = -1;
                    for (int i = 0; i < iterator; i++)
                    {


                        dt_ = dt_.AddHours(long.Parse(dr.IntervalHours.ToString()));
                        dt_ = dt_.AddMinutes(long.Parse(dr.IntervalMinutes.ToString()));

                        aa = dt_.TimeOfDay;
                        thetime = aa.Hours + ":" + aa.Minutes + " ";
                        if (long.Parse(aa.Hours.ToString()) > 12)
                        {
                            var min = aa.Minutes.ToString().Length == 1 ? "0" + aa.Minutes.ToString() : aa.Minutes.ToString();
                            var hrs = aa.Hours.ToString().Length == 1 ? "0" + aa.Hours.ToString() : aa.Hours.ToString();
                            thetime = hrs.ToString() + ":" + min.ToString() + ":00"; //(long.Parse(aa.Hours.ToString()) - 12) + ":" + aa.Minutes + " PM";
                        }
                        else
                        {
                            var min = aa.Minutes.ToString().Length == 1 ? "0" + aa.Minutes.ToString() : aa.Minutes.ToString();
                            var hrs = aa.Hours.ToString().Length == 1 ? "0" + aa.Hours.ToString() : aa.Hours.ToString();
                            thetime = hrs.ToString() + ":" + min.ToString() + ":00";//thetime = aa.Hours + ":" + aa.Minutes + " AM";
                        }
                        j--;
                        table.Rows.Add(j, dr.EntityId.ToString(), dr.UserId.ToString(), dr.ClearingHouseId.ToString(), dr.Case.ToString(), dr.Mode.ToString(), thetime.ToString(), dr.IntervalHours.ToString(), dr.IntervalMinutes.ToString(), dr.CreatedBy.ToString(), DateTime.Parse(dr.CreatedOn.ToString()), dr.ModifiedBy.ToString(), DateTime.Parse(dr.ModifiedOn.ToString()), dr.IsActive);
                    }


                    ///////////////////

                    #region Database Insertion
                    dsEDI.EDIServiceHandle.AddEDIServiceHandleRow(dr);

                    foreach (DataRow dr_ in table.Rows)
                    {
                        // if (/* some condition */)
                        dsEDI.EDIServiceHandle.Rows.Add(dr_.ItemArray);
                    }

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
        /// FillEDIServiceHandle
        /// </summary>
        /// <param name="EDIServiceHandleId"></param>
        /// <returns></returns>
        private string FillEDIServiceHandle(Int64 EDIServiceHandleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIServiceHandleId)))
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
                        BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null, null, null, null);
                        if (objEDIServiceHandle.Data != null)
                        {
                            dsEDI = objEDIServiceHandle.Data;
                            if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlClearingHouse", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ClearingHouseIdColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.EntityIdColumn.ColumnName])},
                            { "ddlCase", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.CaseColumn.ColumnName])},
                            { "ddlInterval", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.ModeColumn.ColumnName])},
                            { "txtTime", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.TimeColumn.ColumnName])},
                            { "txtHours", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IntervalHoursColumn.ColumnName])},
                            { "txtMinutes", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IntervalMinutesColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsEDI.EDIServiceHandle.IsActiveColumn.ColumnName])}

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    EDIServiceHandleFill_JSON = js.Serialize(keyValues)
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
                                Message = objEDIServiceHandle.Message
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
        /// UpdateEDIServiceHandle
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="EDIServiceHandleId"></param>
        /// <returns></returns>
        private string UpdateEDIServiceHandle(string fieldsJSON, Int64 EDIServiceHandleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSEDI dsEDI = new DSEDI();
                    BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null, null, null, null);
                    dsEDI = objEDIServiceHandle.Data;
                    foreach (DSEDI.EDIServiceHandleRow dr in dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                            dr.Case = MDVUtility.ToStr(SearchedfieldsJSON["ddlCase"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInterval"]))
                            dr.Mode = MDVUtility.ToStr(SearchedfieldsJSON["ddlInterval"]);

                        if (dr.Mode == "Interval")
                        {
                            dr.IntervalHours = MDVUtility.ToStr(SearchedfieldsJSON["txtHours"]);
                            dr.IntervalMinutes = MDVUtility.ToStr(SearchedfieldsJSON["txtMinutes"]);
                        }
                        else
                        {
                            dr.IntervalHours = "";
                            dr.IntervalMinutes = "";
                        }
                        dr.Time = MDVUtility.ToStr(SearchedfieldsJSON["txtTime"]);


                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation

                    if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
                    {
                        BLObject<DSEDI> objEDIServiceHandle_ = BLLAdminEDIObj.UpdateEDIServiceHandle(dsEDI);
                        dsEDI = objEDIServiceHandle_.Data;
                        if (objEDIServiceHandle_.Data != null)
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
                                Message = objEDIServiceHandle.Message
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
        /// DeleteEDIServiceHandle
        /// </summary>
        /// <param name="EDIServiceHandleId"></param>
        /// <returns></returns>
        private string DeleteEDIServiceHandle(Int64 EDIServiceHandleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIServiceHandleId)))
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
                        BLObject<string> objEDIServiceHandle = BLLAdminEDIObj.DeleteEDIServiceHandle(MDVUtility.ToStr(EDIServiceHandleId));

                        if (objEDIServiceHandle.Data == "")
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
                                Message = objEDIServiceHandle.Data
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
        /// UpdateEDIServiceHandleIsActive
        /// </summary>
        /// <param name="EDIServiceHandleId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        private string UpdateEDIServiceHandleIsActive(Int64 EDIServiceHandleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("EDI Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSEDI dsEDI = null;
                    BLObject<DSEDI> obj = BLLAdminEDIObj.LoadEDIServiceHandle(EDIServiceHandleId, null, null, null, null, null, null);
                    dsEDI = obj.Data;
                    if (dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsEDI.Tables[dsEDI.EDIServiceHandle.TableName].Rows[0];
                        dr[dsEDI.EDIServiceHandle.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSEDI> objEDIServiceHandle = BLLAdminEDIObj.UpdateEDIServiceHandle(dsEDI);
                        string successMsg;
                        if (objEDIServiceHandle.Data != null)
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
                                Message = objEDIServiceHandle.Message
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
        /// CommandHandler
        /// </summary>
        /// <param name="context"></param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EDI_SERVICE_HANDLE":
                    {
                        string fieldsJSON = context.Request["EDIServiceHandleData"];
                        Int64 EDIServiceHandleID = MDVUtility.ToInt64(context.Request["EDIServiceHandleID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadEDIServiceHandle(fieldsJSON, EDIServiceHandleID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

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
                        string strJSONData = FillEDIServiceHandle(MDVUtility.ToInt64(strEDIServiceHandleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_SERVICE_HANDLE":
                    {
                        string strEDIServiceHandleId = context.Request["EDIServiceHandleID"];
                        string strJSONData = DeleteEDIServiceHandle(MDVUtility.ToInt64(strEDIServiceHandleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SERVICE_HANDLE":
                    {
                        string fieldsJSON = context.Request["EDIServiceHandleData"];
                        Int64 EDIServiceHandleID = MDVUtility.ToInt64(context.Request["EDIServiceHandleID"]);
                        string strJSONData = UpdateEDIServiceHandle(fieldsJSON, EDIServiceHandleID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_SERVICE_HANDLE_ACTIVE_INACTIVE":
                    {
                        Int64 EDIServiceHandleID = MDVUtility.ToInt64(context.Request["EDIServiceHandleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEDIServiceHandleIsActive(EDIServiceHandleID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}