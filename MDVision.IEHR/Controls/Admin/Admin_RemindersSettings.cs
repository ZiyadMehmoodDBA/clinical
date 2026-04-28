using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Model.Admin.Reminders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_RemindersSettings
    {
        private BLLAdmin BLLAdminObj = null;
        public Admin_RemindersSettings()
        {
            BLLAdminObj = new BLLAdmin();
        }
        #region Singleton

        private static Admin_RemindersSettings _instance = null;
        public static Admin_RemindersSettings Instance()
        {
            if (_instance == null)
                _instance = new Admin_RemindersSettings();
            return _instance;
        }

        #endregion

        #region EMAIL Settings Functions
        public string SearchRemindersSettings(RemindersSettingModel model)
        {
            try
            {
                DSReminders dsReminders = null;
                BLObject<DSReminders> obj = null;

                long ProviderId = !string.IsNullOrEmpty(model.Provider) ? MDVUtility.ToLong(model.Provider) : 0;
                // long EmailId = !string.IsNullOrEmpty(model.ReminderEmailSettingId) ? MDVUtility.ToLong(model.ReminderEmailSettingId) : 0;



                obj = BLLAdminObj.loadSettings(MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(model.ReminderEmailSettingId), model.IsActive);

                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    if (dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows.Count > 0 || dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count > 0 || dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RemindersEmailSettingsCount = dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows.Count,
                            RemindersEmailSettingsLoad_JSON = MDVUtility.JSON_DataTable(dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName]),
                            RemindersTextSettingCount = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count,
                            RemindersTextSettingLoad_JSON = MDVUtility.JSON_DataTable(dsReminders.Tables[dsReminders.ReminderTextSetting.TableName]),
                            RemindersVoiceSettingCount = dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows.Count,
                            RemindersVoiceSettingLoad_JSON = MDVUtility.JSON_DataTable(dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string DeleteRemindersEmailSettings(RemindersSettingModel model)
        {
            try
            {
                long PatStmtID = MDVUtility.ToLong(model.ReminderEmailSettingId);
                if (PatStmtID == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminObj.deleteEmailSettings(PatStmtID);
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

        public string SaveRemindersEmailSettings(RemindersSettingModel model)
        {
            try
            {
                DSReminders dsReminders = new DSReminders();
                DSReminders.ReminderEmailSettingRow dr = dsReminders.ReminderEmailSetting.NewReminderEmailSettingRow();
                dr.ReminderEmailSettingId = -1;
                dr.SenderEmail = model.CallIDNumber;
                dr.MessageTemplate = model.HTMLTemplate;
                dr.Facilityids = model.FacilityIds;
                if (!string.IsNullOrEmpty(model.IsActive))
                {
                    dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                }

                dr.ProviderId = MDVUtility.ToLong(model.Provider);

                //if (model.PEDataTemptId == -1)
                //{
                //    dr.PEDataTemptId = MDVUtility.ToLong(DBNull.Value);
                //}
                //else
                //{
                //    dr.PEDataTemptId = model.PEDataTemptId;
                //}

                // dr.MessageTemplate = MDVUtility.ToInt64(model.EntityId != null ? MDVUtility.ToInt64(model.EntityId) : MDVUtility.ToInt64(MDVSession.Current.EntityId));
                //dr.NotesTagNameIds = model.NotesTagNameIds;
                //dr.MessageTemplate = model.HTMLTemplate;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.DeliveryDateTime = model.EmailDelivery;
                dsReminders.ReminderEmailSetting.AddReminderEmailSettingRow(dr);
                #region Database Insertion
                BLObject<DSReminders> obj = BLLAdminObj.insertEmailSettings(dsReminders);
                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        ReminderEmailSettingId = dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows[0][dsReminders.ReminderEmailSetting.ReminderEmailSettingIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                #endregion

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

        public string UpdateRemindersEmailSettings(RemindersSettingModel model)
        {
            try
            {

                if (MDVUtility.ToInt64(model.ReminderEmailSettingId) > 0)
                {
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;

                    BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersEmailSettings(MDVUtility.ToInt64(0), MDVUtility.ToInt64(model.ReminderEmailSettingId));
                    dsReminders = objLoad.Data;
                    foreach (DSReminders.ReminderEmailSettingRow dr in dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.CallIDNumber))
                            dr.SenderEmail = model.CallIDNumber;
                        if (!string.IsNullOrEmpty(model.Provider))
                        {
                            dr.ProviderId = MDVUtility.ToInt64(model.Provider);
                        }

                        dr.MessageTemplate = model.HTMLTemplate;

                        if (!string.IsNullOrEmpty(model.IsActive))
                        {
                            dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                        }

                        dr.Facilityids = model.FacilityIds;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.DeliveryDateTime = model.EmailDelivery;
                    }
                    #endregion
                    #region Database Updation

                    if (dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows.Count > 0)
                    {
                        BLObject<DSReminders> obj = BLLAdminObj.updateEmailSettings(dsReminders);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                        Message = "Record not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
            //return "";
        }
        public string ActiveInactiveTemplates(RemindersTemplateModel model)
        {
            try
            {

                if (MDVUtility.ToInt64(model.RemindersTemplateId) > 0)
                {
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;

                    BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersTemplate(MDVUtility.ToInt64(model.RemindersTemplateId), "", "", 0, null, 0, 0);
                    dsReminders = objLoad.Data;
                    foreach (DSReminders.RemindersTemplateRow dr in dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.IsActive))
                        {
                            dr.IsActive = model.IsActive == "1" ? true : false;
                        }
                        //dr.EntityId = MDVUtility.ToInt64(model.EntityId);

                        //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.ModifiedOn = DateTime.Now;
                    }
                    #endregion
                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSReminders> obj = BLLAdminObj.UpdateRemindersTemplate(dsReminders);
                        string successMsg;
                        if (obj.Data != null)
                        {

                            if (model.IsActive == "0")
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
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
                        Message = "Record not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string FillRemindersemail(RemindersSettingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ReminderEmailSettingId))
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

                    #region Binding DataSet Information
                    DSReminders dsReminders = null;
                    BLObject<DSReminders> obj = null;
                    obj = BLLAdminObj.loadRemindersEmailSettings(MDVUtility.ToInt64(model.Provider), MDVUtility.ToInt64(model.ReminderEmailSettingId));
                    dsReminders = obj.Data;
                    if (dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows[0];
                        RemindersSettingModel modeldata = new RemindersSettingModel();

                        // modeldata.ReminderEmailSettingId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.RemindersTemplateIdColumn.ColumnName]);
                        modeldata.CallIDNumber = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.SenderEmailColumn.ColumnName]);
                        modeldata.Provider = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.ProviderIdColumn.ColumnName]);
                        //modeldata.EmailTemplate = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.MessageTemplateColumn.ColumnName]);
                        modeldata.HTMLTemplate = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.MessageTemplateColumn.ColumnName]);
                        modeldata.IsActive = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.IsActiveColumn.ColumnName]);
                        // modeldata.EntityId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.EntityIdColumn.ColumnName]);
                        modeldata.EmailDelivery = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.DeliveryDateTimeColumn.ColumnName]);
                        modeldata.IsActive = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.IsActiveColumn.ColumnName]);
                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                        modeldata.FacilityIds = string.Join(",", drs.Where(p => p.ReminderId == MDVUtility.ToLong(dr[dsReminders.ReminderEmailSetting.ReminderEmailSettingIdColumn.ColumnName]) && p.ReminderType == "Email").Select(n => n.FacilityId).ToArray());

                        #endregion
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            RemindersTemplateFill_JSON = js.Serialize(modeldata),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        #region "Text Setting Functions"

        public string SaveRemindersTextSettings(RemindersSettingModel model)
        {
            try
            {
                DSReminders dsReminders = new DSReminders();
                DSReminders.ReminderTextSettingRow dr = dsReminders.ReminderTextSetting.NewReminderTextSettingRow();
                dr.ReminderTextSettingId = -1;
                dr.CallIDNumber = model.SMSCallerName;
                dr.MessageTemplate = model.AppReminderMessage;

                if (!string.IsNullOrEmpty(model.IsActive))
                {
                    dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                }

                dr.IsReminderFailover = model.ChkVoiceReminderFailover == "True" ? true : false;
                dr.ProviderId = MDVUtility.ToInt64(model.TextSettingsProvider);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.ConfirmationMessage = model.AppConfirmationMessage;
                dr.CancellationMessage = model.AppCancellationMessage;
                dr.DeliveryDateTime = model.TextDelivery;
                dr.TimeZone = MDVUtility.ToStr(ConfigurationManager.AppSettings["ReminderCall_TimeZone"]);
                dr.TimeZoneId = MDVUtility.ToLong(1);
                dr.Facilityids = model.FacilityIds;

                dsReminders.ReminderTextSetting.AddReminderTextSettingRow(dr);
                #region Database Insertion
                BLObject<DSReminders> obj = BLLAdminObj.insertRemindersTextSettings(dsReminders);
                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        ReminderTextSettingId = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows[0][dsReminders.ReminderTextSetting.ReminderTextSettingIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                #endregion

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

        public string SearchRemindersTextSettings(RemindersSettingModel model)
        {
            try
            {
                DSReminders dsReminders = null;
                BLObject<DSReminders> obj = null;
                obj = BLLAdminObj.loadRemindersTextSettings(0, MDVUtility.ToInt64(model.ReminderTextSettingId));
                if (obj.Data != null)
                {
                    dsReminders = obj.Data;
                    if (dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RemindersTextSettingzCount = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count,
                            RemindersTextSettingzLoad_JSON = MDVUtility.JSON_DataTable(dsReminders.Tables[dsReminders.ReminderTextSetting.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string FillRemindersText(RemindersSettingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ReminderTextSettingId))
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
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;
                    BLObject<DSReminders> obj = null;
                    obj = BLLAdminObj.loadRemindersTextSettings(0, MDVUtility.ToInt64(model.ReminderTextSettingId));
                    dsReminders = obj.Data;
                    if (dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows[0];
                        RemindersSettingModel modeldata = new RemindersSettingModel();

                        // modeldata.ReminderEmailSettingId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.RemindersTemplateIdColumn.ColumnName]);
                        modeldata.SMSCallerName = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.CallIDNumberColumn.ColumnName]);
                        modeldata.DisableVoiceReminderFailover = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.IsReminderFailoverColumn.ColumnName]);
                        //modeldata.EmailTemplate = MDVUtility.ToStr(dr[dsReminders.ReminderEmailSetting.MessageTemplateColumn.ColumnName]);
                        modeldata.AppReminderMessage = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.MessageTemplateColumn.ColumnName]);
                        modeldata.TextSettingsProvider = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.ProviderIdColumn.ColumnName]);
                        // modeldata.EntityId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.EntityIdColumn.ColumnName]);
                        modeldata.AppConfirmationMessage = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.ConfirmationMessageColumn.ColumnName]);
                        modeldata.AppCancellationMessage = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.CancellationMessageColumn.ColumnName]);
                        modeldata.TextDelivery = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.DeliveryDateTimeColumn.ColumnName]);
                        //modeldata.TextTimeZone = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.TimeZoneIdColumn.ColumnName]);
                        modeldata.ChkVoiceReminderFailover = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.IsReminderFailoverColumn.ColumnName]);
                        modeldata.IsActive = MDVUtility.ToStr(dr[dsReminders.ReminderTextSetting.IsActiveColumn.ColumnName]);
                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                        modeldata.FacilityIds = string.Join(",", drs.Where(p => p.ReminderId == MDVUtility.ToLong(dr[dsReminders.ReminderTextSetting.ReminderTextSettingIdColumn.ColumnName]) && p.ReminderType == "Text").Select(n => n.FacilityId).ToArray());

                        #endregion
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            RemindersTemplateFill_JSON = js.Serialize(modeldata),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        public string DeleteRemindersTextSettings(RemindersSettingModel model)
        {
            try
            {
                long ReminderTextSettingId = MDVUtility.ToLong(model.ReminderTextSettingId);
                if (ReminderTextSettingId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminObj.deleteRemindersTextSettings(ReminderTextSettingId);
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

        public string UpdateRemindersTextSettings(RemindersSettingModel model)
        {
            try
            {

                if (MDVUtility.ToInt64(model.ReminderTextSettingId) > 0)
                {
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;

                    BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersTextSettings(0, MDVUtility.ToInt64(model.ReminderTextSettingId));
                    dsReminders = objLoad.Data;
                    foreach (DSReminders.ReminderTextSettingRow dr in dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.RepliesEmail))
                            dr.RepliesEmail = model.RepliesEmail;
                        if (!string.IsNullOrEmpty(model.TextSettingsProvider))
                        {
                            dr.ProviderId = MDVUtility.ToInt64(model.TextSettingsProvider);
                        }
                        dr.MessageTemplate = model.AppReminderMessage;
                        dr.CallIDNumber = model.SMSCallerName;
                        if (!string.IsNullOrEmpty(model.ChkVoiceReminderFailover))
                        {
                            dr.IsReminderFailover = model.ChkVoiceReminderFailover == "True" ? true : false;
                        }
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        if (!string.IsNullOrEmpty(model.IsActive))
                        {
                            dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                        }

                        dr.Facilityids = model.FacilityIds;
                        dr.ConfirmationMessage = model.AppConfirmationMessage;
                        dr.CancellationMessage = model.AppCancellationMessage;
                        dr.DeliveryDateTime = model.TextDelivery;
                        //dr.TimeZone = model.TextTimeZone_RefValue ;
                        //dr.TimeZoneId = MDVUtility.ToLong(model.TextTimeZone);
                    }
                    #endregion
                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSReminders> obj = BLLAdminObj.updateRemindersTextSettings(dsReminders);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
            //return "";
        }

        #endregion

        #region "Voice Setting Functions"

        public string SaveRemindersVoiceSettings(RemindersSettingModel model)
        {
            try
            {

                DSReminders dsReminders = new DSReminders();
                DSReminders.ReminderVoiceSettingRow dr = dsReminders.ReminderVoiceSetting.NewReminderVoiceSettingRow();
                dr.ReminderVoiceSettingId = -1;
                dr.CallIDNumber = model.CallIDNumberforVoice;
                dr.MessageTemplate = model.HTMLTemplate;
                dr.AppConfirmationKey = model.ConfirmationKey;
                dr.AppCancelKey = model.CancelKey;
                dr.DialInPin = model.PIN;
                dr.TextToSpeech = model.Texttospeech;
                dr.Facilityids = model.FacilityIds;
                if (!string.IsNullOrEmpty(model.IsActive))
                {
                    dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                }
                dr.RepeatMessage = model.RepeatMessage == "true" ? true : false;
                dr.ProviderId = MDVUtility.ToInt64(model.ProviderVoiceSetting);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.DeliveryDateTime = model.VoiceDelivery;
                dr.TimeZone = MDVUtility.ToStr(ConfigurationManager.AppSettings["ReminderCall_TimeZone"]);
                dr.TimeZoneId = MDVUtility.ToLong(1);
                dr.RemindersTemplateId = MDVUtility.ToInt64(model.Template);

                dsReminders.ReminderVoiceSetting.AddReminderVoiceSettingRow(dr);
                #region Database Insertion
                BLObject<DSReminders> obj = BLLAdminObj.insertRemindersVoiceSettings(dsReminders);
                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        ReminderVoiceSettingId = dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows[0][dsReminders.ReminderVoiceSetting.ReminderVoiceSettingIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                #endregion

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


        public string FillRemindersVoice(RemindersSettingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ReminderVoiceSettingId))
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
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;
                    BLObject<DSReminders> obj = null;
                    obj = BLLAdminObj.loadRemindersVoiceSettings(0, MDVUtility.ToInt64(model.ReminderVoiceSettingId));
                    dsReminders = obj.Data;
                    if (dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows[0];
                        RemindersSettingModel modeldata = new RemindersSettingModel();

                        modeldata.CallIDNumberforVoice = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.CallIDNumberColumn.ColumnName]);
                        modeldata.ConfirmationKey = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.AppConfirmationKeyColumn.ColumnName]);
                        modeldata.RepeatMessage = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.RepeatMessageColumn.ColumnName]);
                        modeldata.PIN = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.DialInPinColumn.ColumnName]);
                        modeldata.HTMLTemplate = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.MessageTemplateColumn.ColumnName]);
                        modeldata.Texttospeech = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.TextToSpeechColumn.ColumnName]);
                        modeldata.CancelKey = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.AppCancelKeyColumn.ColumnName]);
                        modeldata.ProviderVoiceSetting = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.ProviderIdColumn.ColumnName]);
                        modeldata.VoiceDelivery = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.DeliveryDateTimeColumn.ColumnName]);
                        //modeldata.VoiceTimeZone = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.TimeZoneIdColumn.ColumnName]);
                        modeldata.Template = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.RemindersTemplateIdColumn.ColumnName]);
                        modeldata.IsActive = MDVUtility.ToStr(dr[dsReminders.ReminderVoiceSetting.IsActiveColumn.ColumnName]);
                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                        modeldata.FacilityIds = string.Join(",", drs.Where(p => p.ReminderId == MDVUtility.ToLong(dr[dsReminders.ReminderVoiceSetting.ReminderVoiceSettingIdColumn.ColumnName]) && p.ReminderType == "Voice").Select(n => n.FacilityId).ToArray());

                        #endregion
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            RemindersTemplateFill_JSON = js.Serialize(modeldata),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        public string DeleteRemindersVoiceSettings(RemindersSettingModel model)
        {
            try
            {
                long ReminderVoiceSettingId = MDVUtility.ToLong(model.ReminderVoiceSettingId);
                if (ReminderVoiceSettingId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLAdminObj.deleteRemindersVoiceSettings(ReminderVoiceSettingId);
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

        public string UpdateRemindersVoiceSettings(RemindersSettingModel model)
        {
            try
            {

                if (MDVUtility.ToInt64(model.ReminderVoiceSettingId) > 0)
                {
                    #region Binding DataSet Information
                    DSReminders dsReminders = null;

                    BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersVoiceSettings(0, MDVUtility.ToInt64(model.ReminderVoiceSettingId));
                    dsReminders = objLoad.Data;
                    foreach (DSReminders.ReminderVoiceSettingRow dr in dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.CallIDNumberforVoice))
                            dr.CallIDNumber = model.CallIDNumberforVoice;

                        if (!string.IsNullOrEmpty(model.ConfirmationKey))
                            dr.AppConfirmationKey = model.ConfirmationKey;

                        if (!string.IsNullOrEmpty(model.CancelKey))
                            dr.AppCancelKey = model.CancelKey;

                        if (!string.IsNullOrEmpty(model.Texttospeech))
                            dr.TextToSpeech = model.Texttospeech;

                        if (!string.IsNullOrEmpty(model.PIN))
                            dr.DialInPin = model.PIN;

                        if (!string.IsNullOrEmpty(model.ProviderVoiceSetting))
                        {
                            dr.ProviderId = MDVUtility.ToInt64(model.ProviderVoiceSetting);
                        }

                        if (!string.IsNullOrEmpty(model.IsActive))
                        {
                            dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;
                        }

                        dr.MessageTemplate = model.HTMLTemplate;
                        if (!string.IsNullOrEmpty(model.RepeatMessage))
                        {
                            dr.RepeatMessage = model.RepeatMessage == "true" ? true : false;
                        }
                        dr.Facilityids = model.FacilityIds;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.DeliveryDateTime = model.VoiceDelivery;
                        //dr.TimeZone = model.VoiceTimeZone_RefValue;
                        //dr.TimeZoneId = MDVUtility.ToLong(model.VoiceTimeZone);
                        dr.RemindersTemplateId = MDVUtility.ToInt64(model.Template);
                    }
                    #endregion
                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSReminders> obj = BLLAdminObj.updateRemindersVoiceSettings(dsReminders);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
            //return "";
        }

        public string UpdateRemindersSettingsActiveInactive(RemindersSettingModel model)
        {

            try
            {

                if (MDVUtility.ToInt64(model.SettingId) > 0 && !string.IsNullOrEmpty(model.FormName))
                {
                    DSReminders dsReminders = null;
                    bool status = false;
                    string message = "Record not found.";

                    switch (model.FormName)
                    {
                        case "voice":
                            {
                                BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersVoiceSettings(0, MDVUtility.ToInt64(model.SettingId));
                                if (objLoad.Data != null)
                                {
                                    dsReminders = objLoad.Data;
                                    foreach (DSReminders.ReminderVoiceSettingRow dr in dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows)
                                    {
                                        dr.IsActive = model.IsActive == "1" ? true : false;
                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        dr.ModifiedOn = DateTime.Now;
                                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                                        dr.Facilityids = string.Join(",", drs.Where(p => p.ReminderId == dr.ReminderVoiceSettingId && p.ReminderType == "Voice").Select(n => n.FacilityId).ToArray());
                                    }
                                    BLObject<DSReminders> obj = BLLAdminObj.updateRemindersVoiceSettings(dsReminders);
                                    if (obj.Data != null)
                                    {
                                        status = true;
                                    }
                                    else
                                    {
                                        message = obj.Message;
                                        status = false;
                                    }
                                }
                                else
                                {
                                    status = false;
                                    message = "Record not found.";
                                }
                                
                            }
                            break;
                        case "text":
                            {
                                BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersTextSettings(0, MDVUtility.ToInt64(model.SettingId));
                                if (objLoad.Data != null)
                                {
                                    dsReminders = objLoad.Data;
                                    foreach (DSReminders.ReminderTextSettingRow dr in dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows)
                                    {
                                        dr.IsActive = model.IsActive == "1" ? true : false;
                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        dr.ModifiedOn = DateTime.Now;
                                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                                        dr.Facilityids = string.Join(",", drs.Where(p => p.ReminderId == dr.ReminderTextSettingId && p.ReminderType == "Text").Select(n => n.FacilityId).ToArray());
                                    }
                                    BLObject<DSReminders> obj = BLLAdminObj.updateRemindersTextSettings(dsReminders);
                                    if (obj.Data != null)
                                    {
                                        status = true;
                                    }
                                    else
                                    {
                                        message = obj.Message;
                                        status = false;
                                    }
                                }
                                else
                                {
                                    status = false;
                                    message = "Record not found.";
                                }
                            }
                            break;
                        case "email":
                            {
                                BLObject<DSReminders> objLoad = BLLAdminObj.loadRemindersEmailSettings(0, MDVUtility.ToInt64(model.SettingId));
                                if (objLoad.Data != null)
                                {
                                    dsReminders = objLoad.Data;
                                    foreach (DSReminders.ReminderEmailSettingRow dr in dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows)
                                    {
                                        dr.IsActive = model.IsActive == "1" ? true : false;
                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        dr.ModifiedOn = DateTime.Now;
                                        DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                                        dr.Facilityids = string.Join(",", drs.Where(p => p.ReminderId == dr.ReminderEmailSettingId && p.ReminderType == "Email").Select(n => n.FacilityId).ToArray());
                                    }
                                    BLObject<DSReminders> obj = BLLAdminObj.updateEmailSettings(dsReminders);
                                    if (obj.Data != null)
                                    {
                                        status = true;
                                    }
                                    else
                                    {
                                        message = obj.Message;
                                        status = false;
                                    }
                                }
                                else
                                {
                                    status = false;
                                    message = "Record not found.";
                                }
                            }
                            break;
                    }


                    if (status)
                    {
                        if (model.IsActive == "0")
                            message = Common.AppPrivileges.Inactive_Message;
                        else
                            message = Common.AppPrivileges.Active_Message;

                        var response = new
                        {
                            status = true,
                            Message = message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                        

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Record not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
    }
}
