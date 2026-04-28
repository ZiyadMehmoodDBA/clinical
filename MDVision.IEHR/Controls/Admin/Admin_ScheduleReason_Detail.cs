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
    public class Admin_ScheduleReason_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ScheduleReason_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ScheduleReason_Detail _obj = null;
        public static Admin_ScheduleReason_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ScheduleReason_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the schedule reason.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        private string SaveScheduleReason(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.ScheduleReasonsRow dr = dsSchedule.ScheduleReasons.NewScheduleReasonsRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.Duration = SearchedfieldsJSON["txtDuration"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsSchedule.ScheduleReasons.AddScheduleReasonsRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertReasons(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ScheduleReasonId = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows[0][dsSchedule.ScheduleReasons.ScheduleReasonIdColumn.ColumnName]
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
        /// Updates the schedule reason.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ScheduleReasonId">The schedule reason identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdateScheduleReason(string fieldsJSON, Int64 ScheduleReasonId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    //DSScheduleSetup.ScheduleReasonsRow dr = dsSchedule.ScheduleReasons.NewScheduleReasonsRow();
                    BLObject<DSScheduleSetup> objLoad = BLLScheduleObj.LoadReasons(ScheduleReasonId, null, null, null, null);
                    dsSchedule = objLoad.Data;

                    foreach (DSScheduleSetup.ScheduleReasonsRow dr in dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows)
                    {
                        //dr.ScheduleReasonId = ScheduleReasonId;
                        dr.ShortName = SearchedfieldsJSON["txtShortName"];
                        dr.Description = SearchedfieldsJSON["txtDescription"];
                        dr.Duration = SearchedfieldsJSON["txtDuration"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsSchedule.ScheduleReasons.AddScheduleReasonsRow(dr);
                    // dsSchedule.ScheduleReasons.AcceptChanges();

                    if (dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count > 0)
                    {
                        //dsSchedule.ScheduleReasons.Rows[0].SetModified();
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.UpdateReasons(dsSchedule);
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
        /// Fills the schedule reason.
        /// </summary>
        /// <param name="ScheduleReasonId">The schedule reason identifier.</param>
        /// <returns>System.String.</returns>
        private string FillScheduleReason(Int64 ScheduleReasonId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleReasonId)))
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
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadReasons(ScheduleReasonId, null, null, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.DescriptionColumn.ColumnName])},
                            { "txtDuration", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.DurationColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.EntityIdColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.IsActiveColumn.ColumnName])}
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ScheduleReasonFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the schedule reason.
        /// </summary>
        /// <param name="ScheduleReasonId">The schedule reason identifier.</param>
        /// <returns>System.String.</returns>
        private string DeleteScheduleReason(Int64 ScheduleReasonId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleReasonId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteReasons(MDVUtility.ToStr(ScheduleReasonId));
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
        /// Updates the schedule reason is active.
        /// </summary>
        /// <param name="ScheduleReasonId">The schedule reason identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>System.String.</returns>
        private string UpdateScheduleReasonIsActive(Int64 ScheduleReasonId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Schedule Reason", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadReasons(ScheduleReasonId, null, null, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows[0];
                        dr[dsSchedule.ScheduleReasons.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objScheduleReason = BLLScheduleObj.UpdateReasons(dsSchedule);
                        string successMsg;
                        if (objScheduleReason.Data != null)
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
                                Message = objScheduleReason.Message
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
        /// Handle the ScheduleReason Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_SCHEDULE_REASON":
                    {
                        string fieldsJSON = context.Request["ScheduleReasonData"];
                        string strJSONData = SaveScheduleReason(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SCHEDULE_REASON":
                    {
                        string strScheduleReasonId = context.Request["ScheduleReasonID"];
                        string strJSONData = FillScheduleReason(MDVUtility.ToInt64(strScheduleReasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SCHEDULE_REASON":
                    {
                        string strScheduleReasonId = context.Request["ScheduleReasonID"];
                        string strJSONData = DeleteScheduleReason(MDVUtility.ToInt64(strScheduleReasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHEDULE_REASON":
                    {
                        string fieldsJSON = context.Request["ScheduleReasonData"];
                        Int64 ScheduleReasonID = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        string strJSONData = UpdateScheduleReason(fieldsJSON, ScheduleReasonID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHEDULE_REASON_ACTIVE_INACTIVE":
                    {
                        Int64 ScheduleReasonID = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateScheduleReasonIsActive(ScheduleReasonID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}