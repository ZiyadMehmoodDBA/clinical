using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Model.Admin.Reminders;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls
{
    public class Admin_RemindersTemplates
    {
         private BLLAdmin BLLAdminObj = null;
         public Admin_RemindersTemplates()
        {
            BLLAdminObj = new BLLAdmin();
        }
        #region Singleton

        private static Admin_RemindersTemplates _instance = null;
        public static Admin_RemindersTemplates Instance()
        {
            if (_instance == null)
                _instance = new Admin_RemindersTemplates();
            return _instance;
        }

        #endregion

        #region Functions
        public string SearchRemindersTemplates(RemindersTemplateModel model){
            try
            {
                DSReminders dsReminders = null;
                BLObject<DSReminders> obj = null;

                long ReminderstemplateId = !string.IsNullOrEmpty(model.RemindersTemplateId) ? MDVUtility.ToLong(model.RemindersTemplateId) : 0;
                string ProvidersIds = !string.IsNullOrEmpty(model.ProviderIds) ? MDVUtility.ToStr(model.ProviderIds) : "";
                string Reminderstemplatename = !string.IsNullOrEmpty(model.Name) ? MDVUtility.ToStr(model.Name) : "";
                long TemplatetypeId = !string.IsNullOrEmpty(model.TemplateTypeId) ? MDVUtility.ToLong(model.TemplateTypeId) : 0;
                if(model.IsActive == "True")
                   model.IsActive = "1" ;
                obj = BLLAdminObj.loadRemindersTemplate(ReminderstemplateId, ProvidersIds, Reminderstemplatename, TemplatetypeId, MDVUtility.ToInt32(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToStr(model.ReminderTemplateType));

                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    if (dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RemindersTemplateCount = dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsReminders.RemindersTemplate.Rows.Count > 0) ? dsReminders.RemindersTemplate.Rows[0][dsReminders.RemindersTemplate.RecordCountColumn.ColumnName] : 0,
                            RemindersTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsReminders.Tables[dsReminders.RemindersTemplate.TableName]),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string DeleteRemindersTemplates(RemindersTemplateModel model)
        {
            try
            {
                long PatStmtID = MDVUtility.ToLong(model.RemindersTemplateId);
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
                    BLObject<string> obj = BLLAdminObj.DeleteRemindersTemplate(PatStmtID);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SaveRemindersTemplate(RemindersTemplateModel model)
        {
            try
            {
                DSReminders dsReminders = new DSReminders();
                DSReminders.RemindersTemplateRow dr = dsReminders.RemindersTemplate.NewRemindersTemplateRow();
                dr.RemindersTemplateId = -1;
                dr.RemindersTemplateName = model.TemplateName;
                dr.TemplateTypeId = MDVUtility.ToInt64(model.TemplateTypeId);
                dr.IsActive = model.IsActive == "1" ? true : false;
                //dr.ProviderIds = model.ProviderIds;

                //if (model.PEDataTemptId == -1)
                //{
                //    dr.PEDataTemptId = MDVUtility.ToLong(DBNull.Value);
                //}
                //else
                //{
                //    dr.PEDataTemptId = model.PEDataTemptId;
                //}

               // dr.EntityId = MDVUtility.ToInt64(model.EntityId != null ? MDVUtility.ToInt64(model.EntityId) : MDVUtility.ToInt64(MDVSession.Current.EntityId));
                //dr.NotesTagNameIds = model.NotesTagNameIds;
                dr.HTMLTemplate = model.HTMLTemplate;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.ReminderTemplateType = model.ReminderTemplateType;
                dr.HTMLTemplateWithIds = model.HTMLTemplateWithIds;
                dsReminders.RemindersTemplate.AddRemindersTemplateRow(dr);
                #region Database Insertion
                BLObject<DSReminders> obj = BLLAdminObj.insertRemindersTemplate(dsReminders);
                dsReminders = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        ReminderTemplateId = dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows[0][dsReminders.RemindersTemplate.RemindersTemplateIdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateRemindersTemplate(RemindersTemplateModel model)
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

                      //  if (!string.IsNullOrEmpty(model.RemindersTemplateId))
                         //   dr.RemindersTemplateId = MDVUtility.ToInt64(model.RemindersTemplateId);


                        if (!string.IsNullOrEmpty(model.TemplateName))
                            dr.RemindersTemplateName = model.TemplateName;
                        if (!string.IsNullOrEmpty(model.TemplateTypeId))
                        {
                            dr.TemplateTypeId = MDVUtility.ToInt64(model.TemplateTypeId);
                        }

                        dr.HTMLTemplate = model.HTMLTemplate;
                        //if (!string.IsNullOrEmpty(model.ProviderIds))
                        //{
                        //    dr.ProviderIds = model.ProviderIds;
                        //}

                      
                        if (!string.IsNullOrEmpty(model.IsActive))
                        {
                            dr.IsActive = model.IsActive == "1" ? true : false;
                        }
                      //  dr.EntityId = MDVUtility.ToInt64(model.EntityId);
  
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.HTMLTemplateWithIds = model.HTMLTemplateWithIds;
                    }
                    #endregion
                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSReminders> obj = BLLAdminObj.UpdateRemindersTemplate(dsReminders);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            //return "";
        }

        public string FillRemindersTemplates(RemindersTemplateModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.RemindersTemplateId))
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
                    obj = BLLAdminObj.loadRemindersTemplate(MDVUtility.ToInt64(model.RemindersTemplateId), "", "", 0, null, 0, 0);
                    dsReminders = obj.Data;
                    if (dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReminders.Tables[dsReminders.RemindersTemplate.TableName].Rows[0];
                        RemindersTemplateModel modeldata = new RemindersTemplateModel();

                        modeldata.RemindersTemplateId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.RemindersTemplateIdColumn.ColumnName]);
                        modeldata.TemplateName = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.RemindersTemplateNameColumn.ColumnName]);
                        modeldata.TemplateTypeId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.TemplateTypeIdColumn.ColumnName]);
                        modeldata.HTMLTemplate = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.HTMLTemplateColumn.ColumnName]);
                        modeldata.ReminderTemplateType = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.ReminderTemplateTypeColumn.ColumnName]);
                        modeldata.IsActive = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.IsActiveColumn.ColumnName]);
                        modeldata.EntityId = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.EntityIdColumn.ColumnName]);
                        modeldata.HTMLTemplateWithIds = MDVUtility.ToStr(dr[dsReminders.RemindersTemplate.HTMLTemplateWithIdsColumn.ColumnName]);
                     

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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string InsertNewNoteType(string NewNoteType)
        {
            try
            {
                DSReminders dsNoteType = new DSReminders();
                DSReminders.RemindersTemplateTypeRow dr = dsNoteType.RemindersTemplateType.NewRemindersTemplateTypeRow();
                dr.RemindersTemplateTypeId = -1;
                dr.ShortName = NewNoteType;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsNoteType.RemindersTemplateType.AddRemindersTemplateTypeRow(dr);


                #region Database Insertion
                BLObject<DSReminders> obj = BLLAdminObj.InsertNewNoteType(dsNoteType);
                dsNoteType = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        RemindersTemplateId = dsNoteType.Tables[dsNoteType.RemindersTemplateType.TableName].Rows[0][dsNoteType.RemindersTemplateType.RemindersTemplateTypeIdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}