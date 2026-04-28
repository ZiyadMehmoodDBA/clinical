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
    public class Admin_PatientEligibilityService
    {
        private BLLAdminPatientEligibilityService BLLAdminPatientEligibilityServiceObj = null;
        public Admin_PatientEligibilityService()
        {
            BLLAdminPatientEligibilityServiceObj = new BLLAdminPatientEligibilityService();
        }
        #region Singleton
        private static Admin_PatientEligibilityService _obj = null;
        public static Admin_PatientEligibilityService Instance()
        {
            if (_obj == null)
                _obj = new Admin_PatientEligibilityService();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// LoadPatientEligibilityService
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="PatientEligibilityServiceID"></param>
        /// <param name="rpp"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        private string LoadPatientEligibilityService(string fieldsJson, Int64 PatientEligibilityServiceID, int rpp, int PageNumber)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    DSPatientEligibilityService dsPatientEligibilityService = null;
                    BLObject<DSPatientEligibilityService> obj;

                    var ddlClearingHouse = searchedfieldsJson["ddlClearingHouse"] == "" ? null : searchedfieldsJson["ddlClearingHouse"];
                    var ddlEntity = searchedfieldsJson["ddlEntity"] == "" ? null : searchedfieldsJson["ddlEntity"];
                    var ddlScheduleDays = searchedfieldsJson["ddlScheduleDays"] == "" ? null : searchedfieldsJson["ddlScheduleDays"];
                    var ddlInterval = searchedfieldsJson["ddlInterval"] == "" ? null : searchedfieldsJson["ddlInterval"];
                    var activeStatus = searchedfieldsJson["chkIsActice"] == "" ? null : searchedfieldsJson["chkIsActice"];
                    // var EntitiyId = searchedfieldsJson["ddlEntity"] == "" ? null : searchedfieldsJson["ddlEntity"]; //MDVSession.Current.EntityId;

                    obj = BLLAdminPatientEligibilityServiceObj.LoadPatientEligibilityService(0, ddlEntity, ddlClearingHouse, ddlScheduleDays, ddlInterval, null, activeStatus, rpp, PageNumber);

                    dsPatientEligibilityService = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                PatientEligibilityServiceCount = dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows.Count,
                                iTotalDisplayRecords = dsPatientEligibilityService.PatientEligibilityService.Rows[0][dsPatientEligibilityService.PatientEligibilityService.RecordCountColumn],
                                PatientEligibilityServiceLoad_JSON = MDVUtility.JSON_DataTable(dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName]),
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
        /// SavePatientEligibilityService
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <returns></returns>
        private string SavePatientEligibilityService(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPatientEligibilityService dsPatientEligibilityService = new DSPatientEligibilityService();
                    DSPatientEligibilityService.PatientEligibilityServiceRow dr = dsPatientEligibilityService.PatientEligibilityService.NewPatientEligibilityServiceRow();

                    dr.PatientEligibilityServiceID = -1;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                        dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlScheduleDays"]))
                        dr.ScheduleDays = MDVUtility.ToInt32(SearchedfieldsJSON["ddlScheduleDays"]);

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
                    table.Columns.Add("ScheduleDays", typeof(string));
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
                        table.Rows.Add(j, dr.EntityId.ToString(), dr.UserId.ToString(), dr.ClearingHouseId.ToString(), dr.ScheduleDays, dr.Mode.ToString(), thetime.ToString(), dr.IntervalHours.ToString(), dr.IntervalMinutes.ToString(), dr.CreatedBy.ToString(), DateTime.Parse(dr.CreatedOn.ToString()), dr.ModifiedBy.ToString(), DateTime.Parse(dr.ModifiedOn.ToString()), dr.IsActive);
                    }


                    ///////////////////

                    #region Database Insertion
                    dsPatientEligibilityService.PatientEligibilityService.AddPatientEligibilityServiceRow(dr);

                    foreach (DataRow dr_ in table.Rows)
                    {
                        // if (/* some condition */)
                        dsPatientEligibilityService.PatientEligibilityService.Rows.Add(dr_.ItemArray);
                    }

                    BLObject<DSPatientEligibilityService> objPatientEligibilityService = BLLAdminPatientEligibilityServiceObj.InsertPatientEligibilityService(dsPatientEligibilityService);
                    dsPatientEligibilityService = objPatientEligibilityService.Data;
                    if (objPatientEligibilityService.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PatientEligibilityServiceId = dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows[0][dsPatientEligibilityService.PatientEligibilityService.PatientEligibilityServiceIDColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objPatientEligibilityService.Message
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
        /// FillPatientEligibilityService
        /// </summary>
        /// <param name="PatientEligibilityServiceId"></param>
        /// <returns></returns>
        private string FillPatientEligibilityService(Int64 PatientEligibilityServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientEligibilityServiceId)))
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
                        DSPatientEligibilityService dsPatientEligibilityService = null;
                        BLObject<DSPatientEligibilityService> objPatientEligibilityService = BLLAdminPatientEligibilityServiceObj.LoadPatientEligibilityService(PatientEligibilityServiceId, null, null, null, null, null, null);
                        if (objPatientEligibilityService.Data != null)
                        {
                            dsPatientEligibilityService = objPatientEligibilityService.Data;
                            if (dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlClearingHouse", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.ClearingHouseIdColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.EntityIdColumn.ColumnName])},
                            { "ddlScheduleDays", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.ScheduleDaysColumn.ColumnName])},
                            { "ddlInterval", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.ModeColumn.ColumnName])},
                            { "txtTime", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.TimeColumn.ColumnName])},
                            { "txtHours", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.IntervalHoursColumn.ColumnName])},
                            { "txtMinutes", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.IntervalMinutesColumn.ColumnName])},
                            { "chkIsActive", MDVUtility.ToStr(dr[dsPatientEligibilityService.PatientEligibilityService.IsActiveColumn.ColumnName])}

                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    PatientEligibilityServiceFill_JSON = js.Serialize(keyValues)
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
                                Message = objPatientEligibilityService.Message
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
        /// UpdatePatientEligibilityService
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="PatientEligibilityServiceId"></param>
        /// <returns></returns>
        private string UpdatePatientEligibilityService(string fieldsJSON, Int64 PatientEligibilityServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPatientEligibilityService dsPatientEligibilityService = new DSPatientEligibilityService();
                    BLObject<DSPatientEligibilityService> objPatientEligibilityService = BLLAdminPatientEligibilityServiceObj.LoadPatientEligibilityService(PatientEligibilityServiceId, null, null, null, null, null, null);
                    dsPatientEligibilityService = objPatientEligibilityService.Data;
                    foreach (DSPatientEligibilityService.PatientEligibilityServiceRow dr in dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClearingHouse"]))
                            dr.ClearingHouseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClearingHouse"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlScheduleDays"]))
                            dr.ScheduleDays = MDVUtility.ToInt32(SearchedfieldsJSON["ddlScheduleDays"]);
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

                    if (dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPatientEligibilityService> objPatientEligibilityService_ = BLLAdminPatientEligibilityServiceObj.UpdatePatientEligibilityService(dsPatientEligibilityService);
                        dsPatientEligibilityService = objPatientEligibilityService_.Data;
                        if (objPatientEligibilityService_.Data != null)
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
                                Message = objPatientEligibilityService.Message
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
        /// DeletePatientEligibilityService
        /// </summary>
        /// <param name="PatientEligibilityServiceId"></param>
        /// <returns></returns>
        private string DeletePatientEligibilityService(Int64 PatientEligibilityServiceId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientEligibilityServiceId)))
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
                        BLObject<string> objPatientEligibilityService = BLLAdminPatientEligibilityServiceObj.DeletePatientEligibilityService(MDVUtility.ToStr(PatientEligibilityServiceId));

                        if (objPatientEligibilityService.Data == "")
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
                                Message = objPatientEligibilityService.Data
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
        /// UpdatePatientEligibilityServiceIsActive
        /// </summary>
        /// <param name="PatientEligibilityServiceId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        private string UpdatePatientEligibilityServiceIsActive(Int64 PatientEligibilityServiceId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Eligibility Service", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPatientEligibilityService dsPatientEligibilityService = null;
                    BLObject<DSPatientEligibilityService> obj = BLLAdminPatientEligibilityServiceObj.LoadPatientEligibilityService(PatientEligibilityServiceId, null, null, null, null, null, null);
                    dsPatientEligibilityService = obj.Data;
                    if (dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatientEligibilityService.Tables[dsPatientEligibilityService.PatientEligibilityService.TableName].Rows[0];
                        dr[dsPatientEligibilityService.PatientEligibilityService.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientEligibilityService> objPatientEligibilityService = BLLAdminPatientEligibilityServiceObj.UpdatePatientEligibilityService(dsPatientEligibilityService);
                        string successMsg;
                        if (objPatientEligibilityService.Data != null)
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
                                Message = objPatientEligibilityService.Message
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
                case "SEARCH_PATIENT_ELIGIBILITY_SERVICE":
                    {
                        string fieldsJSON = context.Request["PatientEligibilityServiceData"];
                        Int64 PatientEligibilityServiceID = MDVUtility.ToInt64(context.Request["PatientEligibilityServiceID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadPatientEligibilityService(fieldsJSON, PatientEligibilityServiceID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_PATIENT_ELIGIBILITY_SERVICE":
                    {
                        string fieldsJSON = context.Request["PatientEligibilityServiceData"];
                        string strJSONData = SavePatientEligibilityService(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_ELIGIBILITY_SERVICE":
                    {
                        string strPatientEligibilityServiceId = context.Request["PatientEligibilityServiceID"];
                        string strJSONData = FillPatientEligibilityService(MDVUtility.ToInt64(strPatientEligibilityServiceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_ELIGIBILITY_SERVICE":
                    {
                        string strPatientEligibilityServiceId = context.Request["PatientEligibilityServiceID"];
                        string strJSONData = DeletePatientEligibilityService(MDVUtility.ToInt64(strPatientEligibilityServiceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_ELIGIBILITY_SERVICE":
                    {
                        string fieldsJSON = context.Request["PatientEligibilityServiceData"];
                        Int64 PatientEligibilityServiceID = MDVUtility.ToInt64(context.Request["PatientEligibilityServiceID"]);
                        string strJSONData = UpdatePatientEligibilityService(fieldsJSON, PatientEligibilityServiceID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_ELIGIBILITY_SERVICE_ACTIVE_INACTIVE":
                    {
                        Int64 PatientEligibilityServiceID = MDVUtility.ToInt64(context.Request["PatientEligibilityServiceID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePatientEligibilityServiceIsActive(PatientEligibilityServiceID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}