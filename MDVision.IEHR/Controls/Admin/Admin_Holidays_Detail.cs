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
    public class Admin_Holidays_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_Holidays_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_Holidays_Detail _obj = null;
        public static Admin_Holidays_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Holidays_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the holidays.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        private string SaveHolidays(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.ScheduleHolidaysRow dr = dsSchedule.ScheduleHolidays.NewScheduleHolidaysRow();

                    dr.HolidayOn = MDVUtility.ToDateTime(SearchedfieldsJSON["holidayDate"]);
                    dr.HolidayDescription = SearchedfieldsJSON["txtHoliday"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsSchedule.ScheduleHolidays.AddScheduleHolidaysRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertHolidays(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            HolidaysId = dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows[0][dsSchedule.ScheduleHolidays.ScheduleHolidayIdColumn.ColumnName]
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
        /// Updates the holidays.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="HolidaysId">The holidays identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdateHolidays(string fieldsJSON, Int64 HolidaysId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    //DSScheduleSetup.ScheduleHolidaysRow dr = dsSchedule.ScheduleHolidays.NewScheduleHolidaysRow();
                    BLObject<DSScheduleSetup> objLoad = BLLScheduleObj.LoadHolidays(HolidaysId, null, null, null, null);
                    dsSchedule = objLoad.Data;
                    foreach (DSScheduleSetup.ScheduleHolidaysRow dr in dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows)
                    {
                        //dr.ScheduleHolidayId = HolidaysId;
                        dr.HolidayOn = MDVUtility.ToDateTime(SearchedfieldsJSON["holidayDate"]);
                        dr.HolidayDescription = SearchedfieldsJSON["txtHoliday"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsSchedule.ScheduleHolidays.AddScheduleHolidaysRow(dr);
                    //dsSchedule.ScheduleHolidays.AcceptChanges();

                    if (dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows.Count > 0)
                    {
                        //dsSchedule.ScheduleHolidays.Rows[0].SetModified();
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.UpdateHolidays(dsSchedule);
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
        /// Fills the holidays.
        /// </summary>
        /// <param name="HolidaysId">The holidays identifier.</param>
        /// <returns>System.String.</returns>
        private string FillHolidays(Int64 HolidaysId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(HolidaysId)))
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
                        DSScheduleSetup dsSchedule = null;
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadHolidays(HolidaysId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "holidayDate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.ScheduleHolidays.HolidayOnColumn.ColumnName]).ToShortDateString())},
                            { "txtHoliday", MDVUtility.ToStr(dr[dsSchedule.ScheduleHolidays.HolidayDescriptionColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsSchedule.ScheduleHolidays.EntityIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsSchedule.ScheduleHolidays.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    HolidaysFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the holidays.
        /// </summary>
        /// <param name="HolidaysId">The holidays identifier.</param>
        /// <returns>System.String.</returns>
        private string DeleteHolidays(Int64 HolidaysId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(HolidaysId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteHolidays(MDVUtility.ToStr(HolidaysId));
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
        /// Updates the holidays is active.
        /// </summary>
        /// <param name="HolidaysId">The holidays identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>System.String.</returns>
        private string UpdateHolidaysIsActive(Int64 HolidaysId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Holidays", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadHolidays(HolidaysId, null, null, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleHolidays.TableName].Rows[0];
                        dr[dsSchedule.ScheduleHolidays.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objHolidays = BLLScheduleObj.UpdateHolidays(dsSchedule);
                        string successMsg;
                        if (objHolidays.Data != null)
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
                                Message = objHolidays.Message
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

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_HOLIDAYS":
                    {
                        string fieldsJSON = context.Request["HolidaysData"];
                        string strJSONData = SaveHolidays(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_HOLIDAYS":
                    {
                        string strHolidaysId = context.Request["HolidaysID"];
                        string strJSONData = FillHolidays(MDVUtility.ToInt64(strHolidaysId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_HOLIDAYS":
                    {
                        string strHolidaysId = context.Request["HolidaysID"];
                        string strJSONData = DeleteHolidays(MDVUtility.ToInt64(strHolidaysId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_HOLIDAYS":
                    {
                        string fieldsJSON = context.Request["HolidaysData"];
                        Int64 HolidaysID = MDVUtility.ToInt64(context.Request["HolidaysID"]);
                        string strJSONData = UpdateHolidays(fieldsJSON, HolidaysID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_HOLIDAYS_ACTIVE_INACTIVE":
                    {
                        Int64 HolidaysID = MDVUtility.ToInt64(context.Request["HolidaysID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateHolidaysIsActive(HolidaysID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}