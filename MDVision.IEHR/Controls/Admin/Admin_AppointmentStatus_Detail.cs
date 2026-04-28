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
    public class Admin_AppointmentStatus_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_AppointmentStatus_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_AppointmentStatus_Detail _obj = null;
        public static Admin_AppointmentStatus_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_AppointmentStatus_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the appointment status.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        private string SaveAppointmentStatus(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.AppointmentStatusRow dr = dsSchedule.AppointmentStatus.NewAppointmentStatusRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"].Trim();
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.Color = SearchedfieldsJSON["txtColor"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsSchedule.AppointmentStatus.AddAppointmentStatusRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertAppointmentStatus(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            AppointmentId = dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows[0][dsSchedule.AppointmentStatus.AppointmentIdColumn.ColumnName]
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
        /// Updates the appointment status.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdateAppointmentStatus(string fieldsJSON, Int64 AppointmentId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    //DSScheduleSetup.AppointmentStatusRow dr = dsSchedule.AppointmentStatus.NewAppointmentStatusRow();
                    BLObject<DSScheduleSetup> objLoad = BLLScheduleObj.LoadAppointmentStatus(AppointmentId, null, null, null);
                    dsSchedule = objLoad.Data;

                    foreach (DSScheduleSetup.AppointmentStatusRow dr in dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows)
                    {
                        //dr.AppointmentId = AppointmentId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"].TrimEnd().TrimStart();
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.Color = SearchedfieldsJSON["txtColor"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }



                    #region Database Updation
                    //dsSchedule.AppointmentStatus.AddAppointmentStatusRow(dr);
                    //dsSchedule.AppointmentStatus.AcceptChanges();

                    if (dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count > 0)
                    {
                        //dsSchedule.AppointmentStatus.Rows[0].SetModified();
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.UpdateAppointmentStatus(dsSchedule);
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
        /// Fills the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <returns>System.String.</returns>
        private string FillAppointmentStatus(Int64 AppointmentId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadAppointmentStatus(AppointmentId, null, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsSchedule.AppointmentStatus.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsSchedule.AppointmentStatus.DescriptionColumn.ColumnName])},
                            { "txtColor", MDVUtility.ToStr(dr[dsSchedule.AppointmentStatus.ColorColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsSchedule.AppointmentStatus.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    AppointmentStatusFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <returns>System.String.</returns>
        private string DeleteAppointmentStatus(Int64 AppointmentId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(AppointmentId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteAppointmentStatus(MDVUtility.ToStr(AppointmentId));
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
        /// <param name="AppointmentId">The holidays identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>System.String.</returns>
        private string UpdateAppointmentStatusIsActive(Int64 AppointmentId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointment Status", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadAppointmentStatus(AppointmentId, null, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.AppointmentStatus.TableName].Rows[0];
                        dr[dsSchedule.AppointmentStatus.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objAppointmentStatus = BLLScheduleObj.UpdateAppointmentStatus(dsSchedule);
                        string successMsg;
                        if (objAppointmentStatus.Data != null)
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
                                Message = objAppointmentStatus.Message
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
                case "SAVE_APPOINTMENT_STATUS":
                    {
                        string fieldsJSON = context.Request["AppointmentStatusData"];
                        string strJSONData = SaveAppointmentStatus(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_APPOINTMENT_STATUS":
                    {
                        string strAppointmentId = context.Request["AppointmentID"];
                        string strJSONData = FillAppointmentStatus(MDVUtility.ToInt64(strAppointmentId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_APPOINTMENT_STATUS":
                    {
                        string strAppointmentId = context.Request["AppointmentID"];
                        string strJSONData = DeleteAppointmentStatus(MDVUtility.ToInt64(strAppointmentId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_APPOINTMENT_STATUS":
                    {
                        string fieldsJSON = context.Request["AppointmentStatusData"];
                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        string strJSONData = UpdateAppointmentStatus(fieldsJSON, AppointmentID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_APPOINTMENT_STATUS_ACTIVE_INACTIVE":
                    {
                        Int64 AppointmentID = MDVUtility.ToInt64(context.Request["AppointmentID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateAppointmentStatusIsActive(AppointmentID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}