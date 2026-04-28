using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Business.AppointmentReminders;
using MDVision.Business.AppointmentReminders.Response;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Patient;
using MDVision.DataAccess.DAL.Schedule;
using System.Text.RegularExpressions;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using System.Data;
using MDVision.DataAccess.DAL.CCM;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using MDVision.Business.BCommon.SFTP;
using MDVision.Model.Admin.Reminder;

namespace MDVision.Business.BLL
{


    public class BLLAdmin
    {
        #region Variable

        #endregion
        #region Constructors
        public BLLAdmin()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }
        public BLLAdmin(SharedVariable SharedVariable)
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 08/03/2016

        #region "Template Letter"

        #region "Letter Lookups"
        public BLObject<DSClinicalLetterTemplateLookup> GetLetterCategory()
        {
            try
            {
                DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
                ds = new DALTemplateLetter().GetClinicalTemplateLetterCategory();
                return new BLObject<DSClinicalLetterTemplateLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetLetterCategory", ex);
                return new BLObject<DSClinicalLetterTemplateLookup>(null, ex.Message);
            }
        }
        public BLObject<DSClinicalLetterTemplateLookup> GetLetterTagCategory()
        {
            try
            {
                DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
                ds = new DALTemplateLetter().GetClinicalTemplateLetterTagCategory();
                return new BLObject<DSClinicalLetterTemplateLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetLetterTagCategory", ex);
                return new BLObject<DSClinicalLetterTemplateLookup>(null, ex.Message);
            }
        }
        public BLObject<DSClinicalLetterTemplateLookup> GetLetterTagName(int TagCategoryNameId)
        {
            try
            {
                DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
                ds = new DALTemplateLetter().GetClinicalTemplateLetterTagCategoryName(TagCategoryNameId);
                return new BLObject<DSClinicalLetterTemplateLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetLetterTagName", ex);
                return new BLObject<DSClinicalLetterTemplateLookup>(null, ex.Message);
            }
        }

        #endregion

        #region CRUD for Letter Templates

        public BLObject<DSLetter> loadLetterTemplates(long TemplateLetterId, bool? isActive, string name, string description, int categoryId, int pageNo, int rpp)
        {
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-515
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALTemplateLetter().loadLetterTemplates(TemplateLetterId, isActive, name, description, categoryId, pageNo, rpp);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadLetterTemplates", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 02/03/2016
        //OverView: Methods "InsertTemplateLetter" for save Template letter 
        public BLObject<DSLetter> InsertTemplateLetter(DSLetter ds)
        {
            try
            {
                ds = new DALTemplateLetter().InsertTemplateLetter(ds);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertTemplateLetter", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }
        public BLObject<string> deleteLetterTemplate(string TemplateLetterId)
        {
            try
            {
                TemplateLetterId = new DALTemplateLetter().deleteLetterTemplate(TemplateLetterId);

                return new BLObject<string>(TemplateLetterId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::deleteLetterTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/03/2016
        //OverView: Methods "GetTemplateLetterData" for Get Template letter Data
        public BLObject<DSLetter> GetTemplateLetterData(long TemplateLetterId)
        {
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALTemplateLetter().GetTemplateLetterData(TemplateLetterId);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetTemplateLetterData", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/03/2016
        //OverView: Methods "UpdateTemplateLetter" for Update Template letter Data
        public BLObject<DSLetter> UpdateTemplateLetter(DSLetter ds)
        {
            try
            {
                ds = new DALTemplateLetter().UpdateTemplateLetter(ds);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::UpdateTemplateLetter", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }

        public BLObject<DSLetter> LoadPatLettersForSoap(string patletterIds, long patientId)
        {
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALTemplateLetter().LoadPatLettersForSoap(patletterIds, patientId);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::LoadPatLettersForSoap", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }
        public BLObject<DSLetter> AttachPatLettersWithNotes(string patletterIds, long noteId)
        {
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALTemplateLetter().AttachPatLettersWithNotes(patletterIds, noteId);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::AttachPatLettersWithNotes", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }
        public BLObject<string> DetachPatLettersWithNotes(string patletterIds, long notesId)
        {
            try
            {
                var PatLetterId = new DALTemplateLetter().DetachPatLettersWithNotes(patletterIds, notesId);
                return new BLObject<string>(PatLetterId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DetachPatLettersWithNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #endregion

        #region "Reminders Template"

        #region CRUD for "Reminders Template"
        public BLObject<DSReminders> loadRemindersTemplate(long reminderstemplateId, string providersIds, string reminderstemplatename, long templatetypeId, int? isActive, int pageNumber, int rowsPerPage, string ReminderTemplateType = "")
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().loadRemindersTemplate(reminderstemplateId, providersIds, reminderstemplatename, templatetypeId, isActive, pageNumber, rowsPerPage, ReminderTemplateType);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadRemindersTemplate", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> insertRemindersTemplate(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().insertRemindersTemplate(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertRemindersTemplate", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteRemindersTemplate(long reminderstemplateId)
        {
            try
            {
                string message = "";
                message = new DALReminders().deleteRemindersTemplate(reminderstemplateId);

                return new BLObject<string>(message);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::deleteRemindersTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSReminders> UpdateRemindersTemplate(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().updateRemindersTemplate(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::UpdateRemindersTemplate", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }

        }
        #endregion

        #region CRUD for "Email Settings"
        public BLObject<DSReminders> loadSettings(long ProviderId, long settingID, string IsActive = "")
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().loadEmailSettings(ProviderId, settingID, IsActive);
                ds.Merge(new DALReminders().loadRemindersTextSettings(ProviderId, settingID, IsActive));
                ds.Merge(new DALReminders().loadRemindersVoiceSettings(ProviderId, settingID, IsActive));
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        public BLObject<DSReminders> loadRemindersEmailSettings(long ProviderId, long ReminderEmailSettingId)
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().loadEmailSettings(ProviderId, ReminderEmailSettingId);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadRemindersEmailSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        public BLObject<DSReminders> loadQuickEmailReminder(long ProviderId)
        {

            try
            {
                DSReminders ds = new DSReminders();
                // ds = new DALReminders().loadEmailSettings(ProviderId, settingID);
                ds = new DALReminders().loadQuickEmail(ProviderId);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> insertEmailSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().insertEmailSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertEmailSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<string> deleteEmailSettings(long emailsettingID)
        {
            try
            {
                string message = "";
                message = new DALReminders().deleteEmailSettings(emailsettingID);

                return new BLObject<string>(message);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::deleteEmailSettings", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSReminders> updateEmailSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().updateEmailSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::updateEmailSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }

        }
        #endregion

        #region "Reminders Template Text Settings"

        public BLObject<DSReminders> loadRemindersTextSettings(long ProviderId, long ReminderTextSettingId)
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().loadRemindersTextSettings(ProviderId, ReminderTextSettingId);
                //if (isPatient == "1")
                //{
                //    ds.Merge(new DALPatient().FillPatientPMS(MDVUtility.ToInt64(PatientId), "Demographics"));
                //    ds.Merge(new DALReminders().loadRemindersVoiceSettings(ProviderId, 0));
                //}

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ProviderId"></param>
        /// <param name="ReminderTextSettingId"></param>
        /// <param name="isPatient"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public BLObject<DSReminders> loadProviderRemindersSettings(SharedVariable SharedVariable, long ProviderId, string PatientId)
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders(SharedVariable).loadRemindersTextSettings(SharedVariable, ProviderId, 0, "1");
                ds.Merge(new DALReminders(SharedVariable).loadRemindersVoiceSettings(SharedVariable, ProviderId, 0, "1"));
                ds.Merge(new DALReminders(SharedVariable).loadEmailSettings(SharedVariable, ProviderId, 0, "1"));
                ds.Merge(new DALPatient(SharedVariable).FillPatientPMS(SharedVariable, MDVUtility.ToInt64(PatientId), "Demographics"));

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::loadRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        public BLObject<DSReminders> insertRemindersTextSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().insertRemindersTextSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<string> deleteRemindersTextSettings(long ReminderTextSettingId)
        {
            try
            {
                string message = "";
                message = new DALReminders().deleteRemindersTextSettings(ReminderTextSettingId);

                return new BLObject<string>(message);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::deleteRemindersTextSettings", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSReminders> updateRemindersTextSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().updateRemindersTextSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::updateRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }

        }
        #endregion

        #region "Reminders Template Voice Settings"

        public BLObject<DSReminders> loadRemindersVoiceSettings(long ProviderId, long ReminderVoiceSettingId)
        {

            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().loadRemindersVoiceSettings(ProviderId, ReminderVoiceSettingId);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::loadRemindersVoiceSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> insertRemindersVoiceSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().insertRemindersVoiceSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<string> deleteRemindersVoiceSettings(long ReminderVoiceSettingId)
        {
            try
            {
                string message = "";
                message = new DALReminders().deleteRemindersVoiceSettings(ReminderVoiceSettingId);

                return new BLObject<string>(message);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::deleteRemindersTextSettings", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSReminders> updateRemindersVoiceSettings(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().updateRemindersVoiceSettings(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::updateRemindersTextSettings", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }

        }

        public BLObject<DSReminders> InsertQuickVoiceReminder(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().InsertQuickVoiceReminder(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertQuickVoiceReminder", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> InsertQuickSMSReminder(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().InsertQuickSMSReminder(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertQuickSMSReminder", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> UpdateQuickSMSReminder(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().UpdateQuickSMSReminder(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::UpdateQuickSMSReminder", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> LoadQuickSMSReminder(long QuickSMSReminderId, string IsProcessed, long PatientId, long AppointmentId, long ProviderId, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSReminders ds = new DSReminders();
                ds = new DALReminders().LoadQuickSMSReminder(QuickSMSReminderId, IsProcessed, PatientId, AppointmentId, ProviderId, PageNumber, RowsPerPage);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::LoadQuickSMSReminder", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        #endregion

        #region " Reminders "

        public BLObject<bool> SendQuickSMSReminder(DSReminders ds)
        {
            bool IsSuccess = false;
            string Message = "";
            long QuickSMSReminderId = 0;
            long QuickSMSReminderId_Guarantor = 0;
            string GuarantorNumber = "";
            GuarantorNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.GuarantorPhNumberColumn.ColumnName].ToString();

            try
            {
                ds = new DALReminders().InsertQuickSMSReminder(ds);
                if (ds.QuickSMSReminder.Rows.Count > 0)
                {
                    DSReminders.QuickSMSReminderRow dr = (DSReminders.QuickSMSReminderRow)ds.QuickSMSReminder.Rows[0];
                    QuickSMSReminderId = dr.QuickSMSReminderId;

                    if (QuickSMSReminderId > 0)
                    {
                        AppointmentReminder obj = new AppointmentReminder();
                        response res_obj = obj.SendTextReminder(QuickSMSReminderId, dr.PhoneNumber, dr.MessageTemplate, dr.RequestDelivery, dr.ProviderId,dr.CalleeName);
                        Response_text res_text = res_obj.status.text.FirstOrDefault(p => p.id == "SMS" + QuickSMSReminderId);
                        if (res_text != null)
                        {
                            if (res_text.result == "success")
                            {
                                IsSuccess = true;
                            }
                            else
                            {
                                IsSuccess = false;
                                if (!string.IsNullOrEmpty(res_text.error.message))
                                    Message = res_text.error.message;
                                else
                                    Message = res_text.result;

                                new DALReminders().DeleteQuickSMSReminder(QuickSMSReminderId);
                            }
                        }
                        else
                            throw new Exception("Error while sending sms.");
                    }
                    else
                        throw new Exception("Error while sending sms.");
                    if (GuarantorNumber != "")
                    {
                        DSReminders dsReminders_Guarantor = new DSReminders();
                        DSReminders.QuickSMSReminderRow dr_guarantor = dsReminders_Guarantor.QuickSMSReminder.NewQuickSMSReminderRow();

                        dr_guarantor.CalleeName = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.CalleeNameColumn.ColumnName].ToString();
                        dr_guarantor.PhoneNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.PhoneNumberColumn.ColumnName].ToString();
                        dr_guarantor.MessageTemplate = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.MessageTemplateColumn.ColumnName].ToString();
                        dr_guarantor.IsProcessed = false;
                        dr_guarantor.RequestDelivery = MDVUtility.ToDateTime(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.RequestDeliveryColumn.ColumnName].ToString());
                        dr_guarantor.TimeZone = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.TimeZoneColumn.ColumnName].ToString();
                        dr_guarantor.TimeZoneId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.TimeZoneIdColumn.ColumnName].ToString());
                        dr_guarantor.AppointmentId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.AppointmentIdColumn.ColumnName].ToString());
                        dr_guarantor.PatientId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.PatientIdColumn.ColumnName].ToString());
                        dr_guarantor.ProviderId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.ProviderIdColumn.ColumnName].ToString());
                        dr_guarantor.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr_guarantor.CreatedOn = DateTime.Now;
                        dr_guarantor.GuarantorPhNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.GuarantorPhNumberColumn.ColumnName].ToString();
                        dsReminders_Guarantor.QuickSMSReminder.AddQuickSMSReminderRow(dr_guarantor);

                        dsReminders_Guarantor = new DALReminders().InsertQuickSMSReminder(dsReminders_Guarantor);

                        DSReminders.QuickSMSReminderRow dr_guarantor_ = (DSReminders.QuickSMSReminderRow)dsReminders_Guarantor.QuickSMSReminder.Rows[0];
                        QuickSMSReminderId_Guarantor = dr_guarantor_.QuickSMSReminderId;

                        if (QuickSMSReminderId_Guarantor > 0)
                        {
                            AppointmentReminder obj = new AppointmentReminder();
                            response res_obj = obj.SendTextReminder(QuickSMSReminderId_Guarantor, dr_guarantor_.GuarantorPhNumber, dr_guarantor_.MessageTemplate, dr_guarantor_.RequestDelivery, dr.ProviderId,dr.CalleeName);
                            Response_text res_text = res_obj.status.text.FirstOrDefault(p => p.id == "SMS" + QuickSMSReminderId_Guarantor);
                            if (res_text != null)
                            {
                                if (res_text.result == "success")
                                {
                                    IsSuccess = true;
                                }
                                else
                                {
                                    IsSuccess = false;
                                    if (!string.IsNullOrEmpty(res_text.error.message))
                                        Message = res_text.error.message;
                                    else
                                        Message = res_text.result;

                                    new DALReminders().DeleteQuickSMSReminder(QuickSMSReminderId_Guarantor);
                                }
                            }
                            else
                                throw new Exception("Error while sending sms.");
                        }

                    }
                }

                return new BLObject<bool>(IsSuccess, Message);
            }
            catch (Exception ex)
            {
                if (QuickSMSReminderId > 0)
                    new DALReminders().DeleteQuickSMSReminder(QuickSMSReminderId);
                if (QuickSMSReminderId_Guarantor > 0)
                    new DALReminders().DeleteQuickSMSReminder(QuickSMSReminderId_Guarantor);

                MDVLogger.BLLErrorLog("BLLAdmin::SendQuickSMSReminder", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<bool> SendQuickSMSReminder(SharedVariable SharedVariable, DSReminders ds)
        {


            try
            {
                bool IsPatientReminderSent = false;
                string Message = "";
                bool IsGuarantorReminderSent = false;
                long QuickSMSReminderId = 0;
                long QuickSMSReminderId_Guarantor = 0;
                string GuarantorNumber = "";
                GuarantorNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.GuarantorPhNumberColumn.ColumnName].ToString();


                try
                {
                    #region " Patient Reminder"

                    ds = new DALReminders(SharedVariable).InsertQuickSMSReminder(SharedVariable, ds);
                    if (ds.QuickSMSReminder.Rows.Count > 0)
                    {
                        DSReminders.QuickSMSReminderRow dr = (DSReminders.QuickSMSReminderRow)ds.QuickSMSReminder.Rows[0];
                        QuickSMSReminderId = dr.QuickSMSReminderId;
                        if (QuickSMSReminderId > 0)
                        {
                            AppointmentReminder obj = new AppointmentReminder();
                            response res_obj = obj.SendTextReminder(QuickSMSReminderId, dr.PhoneNumber, dr.MessageTemplate, dr.RequestDelivery, dr.ProviderId,dr.CalleeName, SharedVariable);
                            Response_text res_text = res_obj.status.text.FirstOrDefault(p => p.id == "SMS" + QuickSMSReminderId);
                            if (res_text != null)
                            {
                                if (res_text.result == "success")
                                {
                                    IsPatientReminderSent = true;
                                }
                                else
                                {
                                    IsPatientReminderSent = false;
                                    if (!string.IsNullOrEmpty(res_text.error.message))
                                        Message = res_text.error.message;
                                    else
                                        Message = res_text.result;

                                    throw new Exception(Message);
                                }
                            }
                            else
                                throw new Exception("Error while sending sms.");
                        }
                        else
                            throw new Exception("Error while sending sms.");
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (QuickSMSReminderId > 0)
                        new DALReminders(SharedVariable).DeleteQuickSMSReminder(SharedVariable, QuickSMSReminderId);

                    throw ex;
                }

                try
                {
                    #region " Guarantor Reminder"

                    if (!string.IsNullOrEmpty(MDVUtility.FormatPhoneNumber(GuarantorNumber)))
                    {
                        DSReminders dsReminders_Guarantor = new DSReminders();
                        DSReminders.QuickSMSReminderRow dr_guarantor = dsReminders_Guarantor.QuickSMSReminder.NewQuickSMSReminderRow();

                        dr_guarantor.CalleeName = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.CalleeNameColumn.ColumnName].ToString();
                        dr_guarantor.PhoneNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.PhoneNumberColumn.ColumnName].ToString();
                        dr_guarantor.MessageTemplate = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.MessageTemplateColumn.ColumnName].ToString();
                        dr_guarantor.IsProcessed = false;
                        dr_guarantor.RequestDelivery = MDVUtility.ToDateTime(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.RequestDeliveryColumn.ColumnName].ToString());
                        dr_guarantor.TimeZone = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.TimeZoneColumn.ColumnName].ToString();
                        dr_guarantor.TimeZoneId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.TimeZoneIdColumn.ColumnName].ToString());
                        dr_guarantor.AppointmentId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.AppointmentIdColumn.ColumnName].ToString());
                        dr_guarantor.PatientId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.PatientIdColumn.ColumnName].ToString());
                        dr_guarantor.ProviderId = MDVUtility.ToLong(ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.ProviderIdColumn.ColumnName].ToString());
                        dr_guarantor.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                        dr_guarantor.CreatedOn = DateTime.Now;
                        dr_guarantor.GuarantorPhNumber = ds.Tables[ds.QuickSMSReminder.TableName].Rows[0][ds.QuickSMSReminder.GuarantorPhNumberColumn.ColumnName].ToString();
                        dsReminders_Guarantor.QuickSMSReminder.AddQuickSMSReminderRow(dr_guarantor);

                        dsReminders_Guarantor = new DALReminders(SharedVariable).InsertQuickSMSReminder(SharedVariable, dsReminders_Guarantor);

                        DSReminders.QuickSMSReminderRow dr_guarantor_ = (DSReminders.QuickSMSReminderRow)dsReminders_Guarantor.QuickSMSReminder.Rows[0];
                        QuickSMSReminderId_Guarantor = dr_guarantor_.QuickSMSReminderId;

                        if (QuickSMSReminderId_Guarantor > 0)
                        {
                            AppointmentReminder obj = new AppointmentReminder();
                            response res_obj = obj.SendTextReminder(QuickSMSReminderId_Guarantor, dr_guarantor_.GuarantorPhNumber, dr_guarantor_.MessageTemplate, dr_guarantor_.RequestDelivery, dr_guarantor_.ProviderId,dr_guarantor_.CalleeName, SharedVariable);
                            Response_text res_text = res_obj.status.text.FirstOrDefault(p => p.id == "SMS" + QuickSMSReminderId_Guarantor);
                            if (res_text != null)
                            {
                                if (res_text.result == "success")
                                {
                                    IsGuarantorReminderSent = true;
                                }
                                else
                                {
                                    IsGuarantorReminderSent = false;
                                    if (!string.IsNullOrEmpty(res_text.error.message))
                                        Message = res_text.error.message;
                                    else
                                        Message = res_text.result;

                                    throw new Exception(Message);
                                }
                            }
                            else
                                throw new Exception("Error while sending sms.");
                        }
                        else
                            throw new Exception("Error while sending sms to Guarantor.");
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (QuickSMSReminderId_Guarantor > 0)
                        new DALReminders(SharedVariable).DeleteQuickSMSReminder(SharedVariable, QuickSMSReminderId_Guarantor);

                    MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickSMSReminder::Windows Service::Guarantor Reminder", ex);
                }


                if (IsPatientReminderSent || IsGuarantorReminderSent)
                {
                    return new BLObject<bool>(true, Message);
                }
                else
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickSMSReminder::Windows Service::", new Exception(Message));
                    return new BLObject<bool>(false, Message);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickSMSReminder::Windows Service", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }

        public BLObject<bool> SendQuickEmailReminder(DSReminders ds)
        {
            bool IsSuccess = false;
            string Message = "";
            long QuickEmailReminderId = 0;

            try
            {
                ds = new DALReminders().InsertQuickEmailReminder(ds);
                if (ds.QuickEmailReminder.Rows.Count > 0)
                {
                    DSReminders.QuickEmailReminderRow dr = (DSReminders.QuickEmailReminderRow)ds.QuickEmailReminder.Rows[0];
                    QuickEmailReminderId = dr.QuickEmailReminderId;

                    if (QuickEmailReminderId > 0)
                    {
                        AppointmentReminder obj = new AppointmentReminder();
                        response res_obj = obj.SendEmailReminder(QuickEmailReminderId, dr.RequestDelivery, dr.FromName, dr.PatientName, dr.ToEmail, dr.Subject, dr.AppointmentDate, dr.MessageTemplate, dr.ProviderId);
                        Response_email res_email = res_obj.status.email.FirstOrDefault(p => p.id == "EMAIL" + QuickEmailReminderId);
                        if (res_email != null)
                        {
                            if (res_email.result == "success")
                            {
                                IsSuccess = true;
                            }
                            else
                            {
                                IsSuccess = false;
                                if (!string.IsNullOrEmpty(res_email.error.message))
                                    Message = res_email.error.message;
                                else
                                    Message = res_email.result;

                                throw new Exception(Message);
                            }
                        }
                        else
                            throw new Exception("Error while sending email.");

                    }
                    else
                        throw new Exception("Error while sending email.");
                }

                return new BLObject<bool>(IsSuccess, Message);
            }
            catch (Exception ex)
            {
                if (QuickEmailReminderId > 0)
                    new DALReminders().DeleteQuickEmailReminder(QuickEmailReminderId);

                MDVLogger.BLLErrorLog("BLLAdmin::SendQuickEmailReminder", ex);
                return new BLObject<bool>(false, ex.Message);

            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<bool> SendQuickEmailReminder(SharedVariable SharedVariable, DSReminders ds)
        {
            bool IsSuccess = false;
            string Message = "";
            long QuickEmailReminderId = 0;

            try
            {
                ds = new DALReminders(SharedVariable).InsertQuickEmailReminder(SharedVariable, ds);
                if (ds.QuickEmailReminder.Rows.Count > 0)
                {
                    DSReminders.QuickEmailReminderRow dr = (DSReminders.QuickEmailReminderRow)ds.QuickEmailReminder.Rows[0];
                    QuickEmailReminderId = dr.QuickEmailReminderId;

                    if (QuickEmailReminderId > 0)
                    {
                        AppointmentReminder obj = new AppointmentReminder();
                        response res_obj = obj.SendEmailReminder(QuickEmailReminderId, dr.RequestDelivery, dr.FromName, dr.PatientName, dr.ToEmail, dr.Subject, dr.AppointmentDate, dr.MessageTemplate, dr.ProviderId, SharedVariable);
                        Response_email res_email = res_obj.status.email.FirstOrDefault(p => p.id == "EMAIL" + QuickEmailReminderId);
                        if (res_email != null)
                        {
                            if (res_email.result == "success")
                            {
                                IsSuccess = true;
                            }
                            else
                            {
                                IsSuccess = false;
                                if (!string.IsNullOrEmpty(res_email.error.message))
                                    Message = res_email.error.message;
                                else
                                    Message = res_email.result;

                                throw new Exception(Message);
                            }
                        }
                        else
                            throw new Exception("Error while sending email.");

                    }
                    else
                        throw new Exception("Error while sending email.");
                }

                return new BLObject<bool>(IsSuccess, Message);
            }
            catch (Exception ex)
            {
                if (QuickEmailReminderId > 0)
                    new DALReminders(SharedVariable).DeleteQuickEmailReminder(SharedVariable, QuickEmailReminderId);

                MDVLogger.BLLErrorLog("BLLAdmin::SendQuickEmailReminder::Windows Service", ex);
                return new BLObject<bool>(false, ex.Message);

            }

        }

        public BLObject<bool> SendQuickVoiceReminder(DSReminders ds)
        {
            //TESTING
            //SharedVariable SharedVariable = new SharedVariable();

            //SharedVariable.ClientId = MDVSession.Current.ClientId;
            //SharedVariable.EntityId = MDVSession.Current.EntityId;
            //SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;
            //SharedVariable.UserName = "mdvision";
            //SharedVariable.AppPassWord = "Password1!";
            //SharedVariable.AppUserId = MDVSession.Current.AppUserId;

            //SendPreferenceBasedReminders(SharedVariable);
            //GETCallReminderFollowUp(SharedVariable);
            ////TESTING

            //return new BLObject<bool>(false, "Testing Success.");

            bool IsSuccess = false;
            string Message = "";
            long QuickVoiceReminderId = 0;
            long QuickVoiceReminderId_Guarantor = 0;
            string GuarantorNumber = "";
            GuarantorNumber = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.GuarantorPhNumberColumn.ColumnName].ToString();
            try
            {
                ds = new DALReminders().InsertQuickVoiceReminder(ds);
                if (ds.QuickVoiceReminder.Rows.Count > 0)
                {
                    DSReminders.QuickVoiceReminderRow dr = (DSReminders.QuickVoiceReminderRow)ds.QuickVoiceReminder.Rows[0];
                    QuickVoiceReminderId = dr.QuickVoiceReminderId;

                    if (QuickVoiceReminderId > 0)
                    {
                        AppointmentReminder obj = new AppointmentReminder();
                        response res_obj = obj.SendCallReminder(QuickVoiceReminderId, dr.RequestDelivery, dr.PhoneNumber, dr.MessageTemplate, dr.ProviderId,dr.CalleeName);
                        Response_call res_call = res_obj.status.call.FirstOrDefault(p => p.id == "CALL" + QuickVoiceReminderId);
                        if (res_call != null)
                        {
                            if (res_call.result == "success")
                            {
                                IsSuccess = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(res_call.error.message))
                                    Message = res_call.error.message;
                                else
                                    Message = res_call.result;

                                new DALReminders().DeleteQuickVoiceReminder(QuickVoiceReminderId);
                            }
                        }
                        else
                            throw new Exception("Error while placing call.");

                    }
                    else
                        throw new Exception("Error while placing call.");
                }

                if (GuarantorNumber != "")
                {
                    DSReminders dsReminders_Guarantor = new DSReminders();
                    DSReminders.QuickVoiceReminderRow dr_guarantor = dsReminders_Guarantor.QuickVoiceReminder.NewQuickVoiceReminderRow();

                    dr_guarantor.CalleeName = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.CalleeNameColumn.ColumnName].ToString();
                    dr_guarantor.PhoneNumber = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.PhoneNumberColumn.ColumnName].ToString();
                    dr_guarantor.MessageTemplate = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.MessageTemplateColumn.ColumnName].ToString();
                    dr_guarantor.IsProcessed = false;
                    dr_guarantor.RequestDelivery = MDVUtility.ToDateTime(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.RequestDeliveryColumn.ColumnName].ToString());
                    dr_guarantor.TimeZone = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.TimeZoneColumn.ColumnName].ToString();
                    dr_guarantor.TimeZoneId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.TimeZoneIdColumn.ColumnName].ToString());
                    dr_guarantor.AppointmentId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.AppointmentIdColumn.ColumnName].ToString());
                    dr_guarantor.PatientId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.PatientIdColumn.ColumnName].ToString());
                    dr_guarantor.ProviderId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.ProviderIdColumn.ColumnName].ToString());
                    dr_guarantor.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr_guarantor.CreatedOn = DateTime.Now;
                    //dr_guarantor.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //dr_guarantor.ModifiedOn = DateTime.Now;
                    dr_guarantor.GuarantorPhNumber = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.GuarantorPhNumberColumn.ColumnName].ToString();
                    dsReminders_Guarantor.QuickVoiceReminder.AddQuickVoiceReminderRow(dr_guarantor);

                    dsReminders_Guarantor = new DALReminders().InsertQuickVoiceReminder(dsReminders_Guarantor);

                    DSReminders.QuickVoiceReminderRow dr_guarantor_ = (DSReminders.QuickVoiceReminderRow)dsReminders_Guarantor.QuickVoiceReminder.Rows[0];
                    QuickVoiceReminderId_Guarantor = dr_guarantor_.QuickVoiceReminderId;

                    if (QuickVoiceReminderId_Guarantor > 0)
                    {
                        AppointmentReminder obj = new AppointmentReminder();
                        response res_obj = obj.SendCallReminder(QuickVoiceReminderId_Guarantor, dr_guarantor_.RequestDelivery, dr_guarantor_.GuarantorPhNumber, dr_guarantor_.MessageTemplate, dr_guarantor_.ProviderId,dr_guarantor_.CalleeName);
                        Response_call res_call = res_obj.status.call.FirstOrDefault(p => p.id == "CALL" + QuickVoiceReminderId_Guarantor);
                        if (res_call != null)
                        {
                            if (res_call.result == "success")
                            {
                                IsSuccess = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(res_call.error.message))
                                    Message = res_call.error.message;
                                else
                                    Message = res_call.result;

                                new DALReminders().DeleteQuickVoiceReminder(QuickVoiceReminderId_Guarantor);
                            }
                        }
                        else
                            throw new Exception("Error while placing call.");

                    }
                    else
                        throw new Exception("Error while placing call.");

                }

                return new BLObject<bool>(IsSuccess, Message);
            }
            catch (Exception ex)
            {
                if (QuickVoiceReminderId > 0)
                    new DALReminders().DeleteQuickVoiceReminder(QuickVoiceReminderId);
                if (QuickVoiceReminderId_Guarantor > 0)
                    new DALReminders().DeleteQuickVoiceReminder(QuickVoiceReminderId_Guarantor);

                MDVLogger.BLLErrorLog("BLLAdmin::SendQuickVoiceReminder", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<bool> SendQuickVoiceReminder(SharedVariable SharedVariable, DSReminders ds)
        {


            try
            {
                bool IsPatientReminderSent = false;
                string Message = "";
                bool IsGuarantorReminderSent = false;
                long QuickVoiceReminderId = 0;
                long QuickVoiceReminderId_Guarantor = 0;
                string GuarantorNumber = "";
                GuarantorNumber = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.GuarantorPhNumberColumn.ColumnName].ToString();

                try
                {
                    #region " Patient Reminder "

                    ds = new DALReminders(SharedVariable).InsertQuickVoiceReminder(SharedVariable, ds);
                    if (ds.QuickVoiceReminder.Rows.Count > 0)
                    {


                        DSReminders.QuickVoiceReminderRow dr = (DSReminders.QuickVoiceReminderRow)ds.QuickVoiceReminder.Rows[0];
                        QuickVoiceReminderId = dr.QuickVoiceReminderId;

                        if (QuickVoiceReminderId > 0)
                        {
                            AppointmentReminder obj = new AppointmentReminder();
                            response res_obj = obj.SendCallReminder(QuickVoiceReminderId, dr.RequestDelivery, dr.PhoneNumber, dr.MessageTemplate, dr.ProviderId,dr.CalleeName, SharedVariable);
                            Response_call res_call = res_obj.status.call.FirstOrDefault(p => p.id == "CALL" + QuickVoiceReminderId);
                            if (res_call != null)
                            {
                                if (res_call.result == "success")
                                {
                                    IsPatientReminderSent = true;
                                }
                                else
                                {
                                    IsPatientReminderSent = false;
                                    if (!string.IsNullOrEmpty(res_call.error.message))
                                        Message = res_call.error.message;
                                    else
                                        Message = res_call.result;

                                    throw new Exception(Message);
                                }
                            }
                            else
                                throw new Exception("Error while placing call.");

                        }
                        else
                            throw new Exception("Error while placing call.");

                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (QuickVoiceReminderId > 0)
                        new DALReminders(SharedVariable).DeleteQuickVoiceReminder(SharedVariable, QuickVoiceReminderId);

                    throw ex;
                }

                try
                {
                    #region " Guarantor Reminder "

                    if (!string.IsNullOrEmpty(MDVUtility.FormatPhoneNumber(GuarantorNumber)))
                    {
                        DSReminders dsReminders_Guarantor = new DSReminders();
                        DSReminders.QuickVoiceReminderRow dr_guarantor = dsReminders_Guarantor.QuickVoiceReminder.NewQuickVoiceReminderRow();

                        dr_guarantor.CalleeName = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.CalleeNameColumn.ColumnName].ToString();
                        dr_guarantor.PhoneNumber = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.PhoneNumberColumn.ColumnName].ToString();
                        dr_guarantor.MessageTemplate = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.MessageTemplateColumn.ColumnName].ToString();
                        dr_guarantor.IsProcessed = false;
                        dr_guarantor.RequestDelivery = MDVUtility.ToDateTime(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.RequestDeliveryColumn.ColumnName].ToString());
                        dr_guarantor.TimeZone = ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.TimeZoneColumn.ColumnName].ToString();
                        dr_guarantor.TimeZoneId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.TimeZoneIdColumn.ColumnName].ToString());
                        dr_guarantor.AppointmentId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.AppointmentIdColumn.ColumnName].ToString());
                        dr_guarantor.PatientId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.PatientIdColumn.ColumnName].ToString());
                        dr_guarantor.ProviderId = MDVUtility.ToLong(ds.Tables[ds.QuickVoiceReminder.TableName].Rows[0][ds.QuickVoiceReminder.ProviderIdColumn.ColumnName].ToString());
                        dr_guarantor.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                        dr_guarantor.CreatedOn = DateTime.Now;
                        dr_guarantor.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                        dr_guarantor.ModifiedOn = DateTime.Now;
                        dr_guarantor.GuarantorPhNumber = GuarantorNumber;
                        dsReminders_Guarantor.QuickVoiceReminder.AddQuickVoiceReminderRow(dr_guarantor);

                        dsReminders_Guarantor = new DALReminders(SharedVariable).InsertQuickVoiceReminder(SharedVariable, dsReminders_Guarantor);

                        DSReminders.QuickVoiceReminderRow dr_guarantor_ = (DSReminders.QuickVoiceReminderRow)dsReminders_Guarantor.QuickVoiceReminder.Rows[0];
                        QuickVoiceReminderId_Guarantor = dr_guarantor_.QuickVoiceReminderId;

                        if (QuickVoiceReminderId_Guarantor > 0)
                        {
                            AppointmentReminder obj = new AppointmentReminder();
                            response res_obj = obj.SendCallReminder(QuickVoiceReminderId_Guarantor, dr_guarantor_.RequestDelivery, dr_guarantor_.GuarantorPhNumber, dr_guarantor_.MessageTemplate, dr_guarantor_.ProviderId,dr_guarantor_.CalleeName, SharedVariable);
                            Response_call res_call = res_obj.status.call.FirstOrDefault(p => p.id == "CALL" + QuickVoiceReminderId_Guarantor);
                            if (res_call != null)
                            {
                                if (res_call.result == "success")
                                {
                                    IsGuarantorReminderSent = true;
                                }
                                else
                                {
                                    IsGuarantorReminderSent = false;
                                    if (!string.IsNullOrEmpty(res_call.error.message))
                                        Message = res_call.error.message;
                                    else
                                        Message = res_call.result;

                                    throw new Exception(Message);
                                }
                            }
                            else
                                throw new Exception("Error while placing call.");

                        }
                        else
                            throw new Exception("Error while placing call.");

                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (QuickVoiceReminderId_Guarantor > 0)
                        new DALReminders(SharedVariable).DeleteQuickVoiceReminder(SharedVariable, QuickVoiceReminderId_Guarantor);

                    MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickVoiceReminder::Windows Service::Guarantor Reminder", ex);
                }

                if (IsPatientReminderSent || IsGuarantorReminderSent)
                {
                    return new BLObject<bool>(true, Message);
                }
                else
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickVoiceReminder::Windows Service::", new Exception(Message));
                    return new BLObject<bool>(false, Message);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendQuickVoiceReminder::Windows Service", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }


        public bool SendConfirmationOrCancellationMessage(long QuickSMSReminderId, string PhoneNumber, string Message, DateTime RequestDelivery, long ProviderId,string CalleeName, SharedVariable SharedVariable)
        {
            AppointmentReminder obj = new AppointmentReminder();
            response res_obj = obj.SendTextReminder(QuickSMSReminderId, PhoneNumber, Message, RequestDelivery, ProviderId, CalleeName, SharedVariable, "CSMS");
            Response_text res_text = res_obj.status.text.FirstOrDefault(p => p.id == "CSMS" + QuickSMSReminderId);
            if (res_text.result == "success")
                return false;
            else
                return true;
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public BLObject<DSReminders> GETSMSReminderFollowUp(SharedVariable SharedVariable)
        {
            try
            {

                /* Text Response <status> Tag
                    new: the message is awaiting its specified delivery time
                    sent: the message has been sent to the SMS gateway, and is presumed delivered
                    canceled: the message has been canceled and will not be delivered */

                //Load SMS Reminders that are not Processed yet.
                DSReminders ds = new DALReminders(SharedVariable).LoadQuickSMSReminder(SharedVariable, 0, "false", 0, 0, 0, 1, 1000);
                List<AppointmentReminderStatus> AppointmentStatus_List = new List<AppointmentReminderStatus>();

                List<RemidnerProviderModel> reminder_list = new List<RemidnerProviderModel>();
                foreach (var item in ds.QuickSMSReminder.ToList())
                {
                    if (reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId) != null)
                    {
                        reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId).ReminderIds.Add(item.QuickSMSReminderId);
                    }
                    else
                    {
                        RemidnerProviderModel model = new RemidnerProviderModel();
                        model.ProviderId = item.ProviderId;
                        model.ReminderIds.Add(item.QuickSMSReminderId);
                        reminder_list.Add(model);
                    }

                }

                if (reminder_list.Count > 0)
                {
                    //Get Text Response
                    AppointmentReminder obj = new AppointmentReminder();
                    response response = obj.GetTextReminderResponse(SharedVariable, reminder_list);
                    bool IsQuickSMSChange = false;

                    foreach (DSReminders.QuickSMSReminderRow dr in ds.QuickSMSReminder.Rows)
                    {
                        Response_text res = response.status.text.FirstOrDefault(p => p.id == "SMS" + dr.QuickSMSReminderId);

                        if (res != null)
                        {
                            dr.ModifiedBy = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                            dr.ModifiedOn = DateTime.Now;
                            dr.ReminderSetBy = "MDVision.ReminderService";

                            if (res.status == "new")
                                dr.Status = "Waiting";
                            else if (res.status == "sent" || res.status == "delivered")
                                dr.Status = "Successful";
                            else if (res.status == "failed" || res.status == "error" || res.status == "canceled")
                                dr.Status = "Failed";
                            else if (!string.IsNullOrEmpty(res.status))
                                dr.Status = res.status;

                            if (!string.IsNullOrEmpty(res.error.message))
                                dr.Message = res.error.message;
                            else
                                dr.Message = MDVUtility.ToStr(res.message);

                            try
                            {
                                if (!string.IsNullOrEmpty(res.delivery))
                                    dr.ResponseDelivery = MDVUtility.ToDateTime(res.delivery);
                                else
                                    dr.ResponseDelivery = DateTime.Now;
                            }
                            catch (Exception)
                            { }

                            string patient_response = string.Empty;

                            if (res.status == "sent" || res.status == "delivered")
                            {
                                dr.IsProcessed = true;
                                patient_response = "no response";

                                if (MDVUtility.ToInt(ReminderConfiguration.Confirm) == MDVUtility.ToInt(res.reply.message))
                                {
                                    dr.KeyPress = MDVUtility.ToInt16(res.reply.message);
                                    patient_response = "confirm";

                                    if (!string.IsNullOrEmpty(res.reply.receipt))
                                        dr.ResponseDelivery = MDVUtility.ToDateTime(res.reply.receipt);
                                    else
                                        dr.ResponseDelivery = DateTime.Now;

                                    // sent  confirmation a Messages
                                    if (!string.IsNullOrEmpty(dr.ConfirmationMessage))
                                    {
                                        BLObject<bool> obj_ = new BLObject<bool>();
                                        obj_ = getHTMLString(SharedVariable, dr.ConfirmationMessage, dr.AppointmentId);
                                        if (obj_.Data == false)
                                            throw new Exception(obj_.Message);

                                        string TextTemplate = obj_.Message;
                                        SendConfirmationOrCancellationMessage(dr.QuickSMSReminderId, dr.PhoneNumber, TextTemplate, DateTime.Now, dr.ProviderId,dr.CalleeName, SharedVariable);

                                    }


                                }
                                else if (MDVUtility.ToInt(ReminderConfiguration.Cancel) == MDVUtility.ToInt(res.reply.message))
                                {
                                    dr.KeyPress = MDVUtility.ToInt16(res.reply.message);
                                    patient_response = "cancel";

                                    if (!string.IsNullOrEmpty(res.reply.receipt))
                                        dr.ResponseDelivery = MDVUtility.ToDateTime(res.reply.receipt);
                                    else
                                        dr.ResponseDelivery = DateTime.Now;

                                    // sent  cancellation a Messages
                                    //if (!string.IsNullOrEmpty(dr.CancellationMessage))
                                    //SendConfirmationOrCancellationMessage(dr.QuickSMSReminderId, dr.PhoneNumber, dr.CancellationMessage, DateTime.Now, dr.TimeZone, MDVUtility.FormatPhoneNumber(dr.CalleeName));
                                }

                            }
                            else if (res.status == "failed" || res.status == "error" || res.status == "canceled")
                            {
                                dr.IsProcessed = true;
                                patient_response = "no response";
                            }

                            if (!string.IsNullOrEmpty(patient_response))
                            {
                                AppointmentReminderStatus obj_ = new AppointmentReminderStatus();
                                obj_.AppointmentId = dr.AppointmentId;
                                obj_.Status = patient_response;
                                AppointmentStatus_List.Add(obj_);
                            }


                            IsQuickSMSChange = true;
                        }

                    }

                    // Update Quick SMS Reminder
                    if (IsQuickSMSChange)
                        ds = new DALReminders(SharedVariable).UpdateQuickSMSReminder(SharedVariable, ds);

                    // Update Appointments
                    if (AppointmentStatus_List.Count > 0)
                    {
                        string AppointmentsXMLString = MDVUtility.GetXmlOfObject(typeof(List<AppointmentReminderStatus>), AppointmentStatus_List);
                        AppointmentsXMLString = new DALAppointment(SharedVariable).UpdateAppointmentStatusFromReminder(SharedVariable, AppointmentsXMLString);
                    }
                }

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::GETSMSReminderFollowUp::Windows Service", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public BLObject<DSReminders> GETCallReminderFollowUp(SharedVariable SharedVariable)
        {
            try
            {
                /* Call Response <status> Tag
                   new: the call is awaiting its specified delivery time
                   queued: the call is in the middle of being delivered (or will be within a few seconds)
                   retry: call was not picked up after 60 seconds of ringing, and the system will be trying again later
                   made: the call was successfully delivered
                   failed: the call was not delivered. it was either not picked up, and the system has exhausted all retries, or it was canceled. */


                DSReminders ds = new DSReminders();
                ds.Merge(new DALReminders(SharedVariable).LoadQuickVoiceReminder(SharedVariable, 0, "false", 0, 0, 0,true, 1, 10000));
                List<AppointmentReminderStatus> AppointmentStatus_List = new List<AppointmentReminderStatus>();

                List<RemidnerProviderModel> reminder_list = new List<RemidnerProviderModel>();
                foreach (var item in ds.QuickVoiceReminder.ToList())
                {
                    if (reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId) != null)
                    {
                        reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId).ReminderIds.Add(item.QuickVoiceReminderId);
                    }
                    else
                    {
                        RemidnerProviderModel model = new RemidnerProviderModel();
                        model.ProviderId = item.ProviderId;
                        model.ReminderIds.Add(item.QuickVoiceReminderId);
                        reminder_list.Add(model);
                    }

                }

                if (reminder_list.Count > 0)
                {
                    AppointmentReminder obj = new AppointmentReminder();
                    response response = obj.GetCallReminderResponse(SharedVariable, reminder_list);
                    bool IsQuickCallChange = false;

                    foreach (DSReminders.QuickVoiceReminderRow dr in ds.QuickVoiceReminder.Rows)
                    {
                        Response_call res = response.status.call.FirstOrDefault(p => p.id == "CALL" + dr.QuickVoiceReminderId);

                        if (res != null)
                        {
                            dr.ModifiedBy = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                            dr.ModifiedOn = DateTime.Now;
                            dr.ReminderSetBy = "MDVision.ReminderService";

                            if (res.status == "new" || res.status == "queued" || res.status == "retry")
                                dr.Status = "Waiting";
                            else if (res.status == "made")
                                dr.Status = "Successful";
                            else if (res.status == "failed" || res.status == "error" || res.status == "canceled")
                                dr.Status = "Failed";
                            else if (!string.IsNullOrEmpty(res.status))
                                dr.Status = res.status;


                            if (!string.IsNullOrEmpty(res.error.message))
                                dr.Message = res.error.message;
                            else
                                dr.Message = MDVUtility.ToStr(res.message);

                            try
                            {
                                if (!string.IsNullOrEmpty(res.delivery))
                                    dr.ResponseDelivery = MDVUtility.ToDateTime(res.delivery);
                                else
                                    dr.ResponseDelivery = DateTime.Now;
                            }
                            catch (Exception)
                            { }

                            if (!string.IsNullOrEmpty(res.tries))
                                dr.ReTries = MDVUtility.ToInt16(res.tries);
                            else
                                dr.ReTries = 0;

                            string patient_response = string.Empty;

                            if (res.status == "made" || res.status == "failed" || res.status == "error" || res.status == "canceled")
                            {
                                dr.IsProcessed = true;
                                patient_response = "no response";

                                if (res.status == "made" && dr.ReTries < 3 && string.IsNullOrEmpty(res.keypress))
                                {
                                    dr.IsProcessed = false;
                                    patient_response = "no response";
                                }

                            }

                            if (!string.IsNullOrEmpty(res.keypress))
                            {
                                dr.IsProcessed = true;
                                dr.KeyPress = MDVUtility.ToInt16(res.keypress);

                                if (MDVUtility.ToInt(ReminderConfiguration.Confirm) == MDVUtility.ToInt(res.keypress))
                                    patient_response = "confirm";
                                else if (MDVUtility.ToInt(ReminderConfiguration.Cancel) == MDVUtility.ToInt(res.keypress))
                                    patient_response = "cancel";
                            }

                            if (!string.IsNullOrEmpty(patient_response))
                            {
                                AppointmentReminderStatus obj_ = new AppointmentReminderStatus();
                                obj_.AppointmentId = dr.AppointmentId;
                                obj_.Status = patient_response;
                                AppointmentStatus_List.Add(obj_);
                            }

                            IsQuickCallChange = true;
                        }
                    }


                    // Update Quick CALL Reminder
                    if (IsQuickCallChange)
                        ds = new DALReminders(SharedVariable).UpdateQuickVoiceReminder(SharedVariable, ds);

                    // Update Appointments
                    if (AppointmentStatus_List.Count > 0)
                    {
                        string AppointmentsXMLString = MDVUtility.GetXmlOfObject(typeof(List<AppointmentReminderStatus>), AppointmentStatus_List);
                        AppointmentsXMLString = new DALAppointment(SharedVariable).UpdateAppointmentStatusFromReminder(SharedVariable, AppointmentsXMLString);
                    }
                }

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::GETCallReminderFollowUp::Windows Service", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public BLObject<DSReminders> GETEmailReminderFollowUp(SharedVariable SharedVariable)
        {
            try
            {


                DSReminders ds = new DSReminders();
                ds.Merge(new DALReminders(SharedVariable).LoadQuickEmailReminder(SharedVariable, 0, "false", 0, 0, 0, 1, 10000));
                List<AppointmentReminderStatus> AppointmentStatus_List = new List<AppointmentReminderStatus>();

                List<RemidnerProviderModel> reminder_list = new List<RemidnerProviderModel>();
                foreach (var item in ds.QuickEmailReminder.ToList())
                {
                    if (reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId) != null)
                    {
                        reminder_list.FirstOrDefault(p => p.ProviderId == item.ProviderId).ReminderIds.Add(item.QuickEmailReminderId);
                    }
                    else
                    {
                        RemidnerProviderModel model = new RemidnerProviderModel();
                        model.ProviderId = item.ProviderId;
                        model.ReminderIds.Add(item.QuickEmailReminderId);
                        reminder_list.Add(model);
                    }

                }

                if (reminder_list.Count > 0)
                {
                    AppointmentReminder obj = new AppointmentReminder();
                    response response = obj.GetEmailReminderResponse(SharedVariable, reminder_list);

                    bool IsQuickEmailChange = false;

                    foreach (DSReminders.QuickEmailReminderRow dr in ds.QuickEmailReminder.Rows)
                    {
                        Response_email res = response.status.email.FirstOrDefault(p => p.id == "EMAIL" + dr.QuickEmailReminderId);

                        if (res != null && res.action.Count > 0)
                        {
                            dr.ModifiedBy = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                            dr.ModifiedOn = DateTime.Now;
                            dr.ReminderSetBy = "MDVision.ReminderService";

                            // find status 1- confirm 2- cancel 3- delivered 4- sent
                            string status = string.Empty;
                            string response_date = string.Empty;
                            Int32 PatientResponse = -1;
                            if (res.action.FirstOrDefault(p => p.type == "confirm") != null)
                            {
                                PatientResponse = 1;
                                response_date = res.action.FirstOrDefault(p => p.type == "confirm").Value;
                            }
                            else if (res.action.FirstOrDefault(p => p.type == "cancel") != null)
                            {
                                PatientResponse = 2;
                                response_date = res.action.FirstOrDefault(p => p.type == "cancel").Value;
                            }


                            if (res.action.FirstOrDefault(p => p.type == "deliver") != null)
                            {
                                status = "delivered";
                                response_date = res.action.FirstOrDefault(p => p.type == "deliver").Value;
                            }
                            else if (res.action.FirstOrDefault(p => p.type == "sent") != null)
                            {
                                status = "sent";
                                response_date = res.action.FirstOrDefault(p => p.type == "sent").Value;
                            }
                            else if (res.action.FirstOrDefault(p => p.type == "canceled") != null)
                            {
                                status = "canceled";
                                response_date = res.action.FirstOrDefault(p => p.type == "canceled").Value;
                            }
                            else if (res.action.FirstOrDefault(p => p.type == "failed") != null)
                            {
                                status = "failed";
                                response_date = res.action.FirstOrDefault(p => p.type == "failed").Value;
                            }


                            if (status == "new")
                                dr.Status = "Waiting";
                            else if (status == "sent" || status == "delivered")
                                dr.Status = "Successful";
                            else if (status == "failed" || status == "error" || status == "canceled")
                                dr.Status = "Failed";
                            else if (!string.IsNullOrEmpty(status))
                                dr.Status = status;

                            try
                            {
                                if (!string.IsNullOrEmpty(response_date))
                                    dr.ResponseDelivery = MDVUtility.ToDateTime(response_date);
                                else
                                    dr.ResponseDelivery = DateTime.Now;
                            }
                            catch (Exception)
                            { }

                            if (res.error != null && !string.IsNullOrEmpty(res.error.message))
                                dr.Message = res.error.message;
                            else
                                dr.Message = MDVUtility.ToStr(res.message);

                            if (PatientResponse > 0)
                                dr.KeyPress = PatientResponse;

                            string patient_response = string.Empty;
                            if (PatientResponse == 1 || PatientResponse == 2)
                            {
                                dr.IsProcessed = true;
                                if (PatientResponse == 1)
                                {
                                    patient_response = "confirm";
                                }
                                else if (PatientResponse == 2)
                                {
                                    patient_response = "cancel";
                                }
                            }
                            else if (status == "failed" || status == "error" || status == "canceled")
                            {
                                dr.IsProcessed = true;
                                patient_response = "no response";
                            }


                            if (!string.IsNullOrEmpty(patient_response))
                            {
                                AppointmentReminderStatus obj_ = new AppointmentReminderStatus();
                                obj_.AppointmentId = dr.AppointmentId;
                                obj_.Status = patient_response;
                                AppointmentStatus_List.Add(obj_);
                            }

                            IsQuickEmailChange = true;
                        }
                    }

                    // Update Quick Email Reminder
                    if (IsQuickEmailChange)
                        ds = new DALReminders(SharedVariable).UpdateQuickEmailReminder(SharedVariable, ds);

                    // Update Appointments
                    if (AppointmentStatus_List.Count > 0)
                    {
                        string AppointmentsXMLString = MDVUtility.GetXmlOfObject(typeof(List<AppointmentReminderStatus>), AppointmentStatus_List);
                        AppointmentsXMLString = new DALAppointment(SharedVariable).UpdateAppointmentStatusFromReminder(SharedVariable, AppointmentsXMLString);
                    }
                }

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::GETEmailReminderFollowUp::Windows Service", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        public BLObject<DSReminders> SendPreferenceBasedReminders(SharedVariable SharedVariable, long AppointmentId = 0)
        {
            string steps = "1:: start for Entity" + SharedVariable.EntityId + " Client :" + SharedVariable.ClientId + " \n";
            bool IsAnyAppointmentChange = false;
            DSReminders ds = new DSReminders();

            string AppointmentId_ = null;
            if (AppointmentId > 0)
                AppointmentId_ = AppointmentId.ToString();

            steps += "2:: load appointments: AppointmentId_:(" + AppointmentId_ + ") \n";

            //Load Patient Appointments
            DSAppointment dsAppontments = new DALAppointment(SharedVariable).LoadAppointment(SharedVariable, AppointmentId_, "false", true);

            try
            {

                string Loaded_AppointmentIds = string.Empty;
                foreach (var dr_Appointment in dsAppontments.PatientAppointments)
                {
                    BLObject<DSReminders> obj = null;
                    DSReminders dsReminders_ = new DSReminders();
                    DSReminders dsReminders = null;
                    DSPatient dsPatient = new DSPatient();
                    DSAppointment dsAppointment = new DSAppointment();
                    bool IsSent = false;

                    Loaded_AppointmentIds += MDVUtility.ToStr(dr_Appointment.AppointmentId);

                    steps += "3:: ProviderRemindersSettings Loaded\n";
                    ReminderSetting setting_model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, dr_Appointment.ProviderId);
                    if (setting_model != null)
                    {
                        obj = loadProviderRemindersSettings(SharedVariable, dr_Appointment.ProviderId, MDVUtility.ToStr(dr_Appointment.PatientId));
                        if (obj.Data != null)
                        {
                            dsReminders = obj.Data;
                            if (dsReminders.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                            {

                                string communicationOptOut = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommunicationOptoutColumn.ColumnName].ToString();
                                steps += "5:: communicationOptOut:: (" + communicationOptOut.ToLower() + ") \n";
                                if (communicationOptOut.ToLower() != "true")
                                {
                                    #region " Send Reminders "

                                    string isNotToSend = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommunicationOptoutColumn.ColumnName].ToString();
                                    string prefCommunication = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PrefCommunicationNameColumn.ColumnName].ToString();
                                    string CommunicatewithGuarantor = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommunicatewithGuarantorColumn.ColumnName].ToString();
                                    string GuarantorId = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GuarantorIdColumn.ColumnName].ToString();
                                    string cellNumber = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CellNoColumn.ColumnName].ToString();
                                    string homeNumber = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.HomePhoneNoColumn.ColumnName].ToString();
                                    string workNumber = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.WorkPhoneNoColumn.ColumnName].ToString();
                                    string emailAddress = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.EmailAddressColumn.ColumnName].ToString();
                                    string guarantorRelation = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GuarantorRelationTextColumn.ColumnName].ToString();
                                    string guarantorNumber = dsReminders.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GuarantorPhoneNumberColumn.ColumnName].ToString();

                                    DSReminders.ReminderSettingsFacilityRow[] drs = (DSReminders.ReminderSettingsFacilityRow[])(dsReminders.Tables[dsReminders.ReminderSettingsFacility.TableName].Select("1=1"));//.AsEnumerable().FirstOrDefault().ItemArray);
                                    string voiceFacilityIds = string.Join(",", drs.Where(p => p.ReminderType == "Voice").Select(n => n.FacilityId).ToArray());
                                    string textFacilityIds = string.Join(",", drs.Where(p => p.ReminderType == "Text").Select(n => n.FacilityId).ToArray());
                                    string emailFacilityIds = string.Join(",", drs.Where(p => p.ReminderType == "Email").Select(n => n.FacilityId).ToArray());

                                    string patientContactNum = "";

                                    if (cellNumber != "")
                                        patientContactNum = cellNumber;
                                    else if (cellNumber == "" && homeNumber != "")
                                        patientContactNum = homeNumber;
                                    else if (cellNumber == "" && homeNumber == "" && workNumber != "")
                                        patientContactNum = workNumber;


                                    steps += "5::  Reminder prefCommunication:: (" + prefCommunication.ToLower() + ") patientContactNum::(" + patientContactNum + ") \n";

                                    #region " SMS Reminder "

                                    if (prefCommunication.ToLower() == "text" && patientContactNum != "" && dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows.Count > 0 && textFacilityIds.Contains(dr_Appointment.FacilityId.ToString()))
                                    {
                                        steps += "6::  SMS Reminder SELECTED:: \n";

                                        string AppointmentDate = dr_Appointment.AppointmentDate.ToString();
                                        string AppointmentTime = dr_Appointment.TimeFrom.ToString();
                                        string deliveryTime = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows[0][dsReminders.ReminderTextSetting.DeliveryDateTimeColumn.ColumnName].ToString();
                                        //string TimeZone = dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows[0][dsReminders.ReminderTextSetting.TimeZoneColumn.ColumnName].ToString();
                                        string[] deliveryHours = deliveryTime.Split(' ');
                                        Int32 deliveryMinutes = 0;
                                        if (deliveryHours[1] == "Minute(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]);
                                        }
                                        else if (deliveryHours[1] == "Hour(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }
                                        else if (deliveryHours[1] == "Day(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 24) * 60;
                                            if (MDVUtility.ToDateTime(dr_Appointment.AppointmentDate).DayOfWeek == DayOfWeek.Monday && MDVUtility.ToInt32(deliveryHours[0]) < 2)
                                            {
                                                deliveryMinutes = (3 * 24) * 60;
                                            }
                                        }
                                        else if (deliveryHours[1] == "Week(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 168) * 60;
                                        }
                                        else
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }

                                        string[] date = AppointmentDate.Split(' ');
                                        string appDateTime = date[0] + " " + AppointmentTime;
                                        DateTime appDateNew = Convert.ToDateTime(appDateTime);
                                        appDateNew = appDateNew.AddMinutes(-deliveryMinutes);
                                        BLObject<bool> obj_ = new BLObject<bool>();
                                        obj_ = getHTMLString(SharedVariable, dsReminders.Tables[dsReminders.ReminderTextSetting.TableName].Rows[0][dsReminders.ReminderTextSetting.MessageTemplateColumn.ColumnName].ToString(), dr_Appointment.AppointmentId);
                                        if (obj_.Data == false)
                                            throw new Exception(obj_.Message);

                                        string TextTemplate = obj_.Message;
                                        // Send SMS Reminder
                                        DSReminders.QuickSMSReminderRow dr = dsReminders_.QuickSMSReminder.NewQuickSMSReminderRow();
                                        dr.CalleeName = MDVUtility.FormatPhoneNumber(dr_Appointment.FaclityPhoneNo);

                                        if (CommunicatewithGuarantor.ToLower() == "true" && guarantorRelation.ToLower() != "self")
                                            dr.PhoneNumber = null;
                                        else
                                            dr.PhoneNumber = patientContactNum;
                                        dr.MessageTemplate = TextTemplate;
                                        dr.IsProcessed = false;
                                        dr.RequestDelivery = appDateNew;
                                        dr.TimeZone = setting_model.TimeZone;
                                        dr.AppointmentId = MDVUtility.ToLong(dr_Appointment.AppointmentId);
                                        dr.PatientId = MDVUtility.ToLong(dr_Appointment.PatientId);
                                        dr.ProviderId = MDVUtility.ToLong(dr_Appointment.ProviderId);
                                        dr.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.CreatedOn = DateTime.Now;
                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.ModifiedOn = DateTime.Now;
                                        dr.ReminderSetBy = "MDVision.ReminderService";
                                        if (CommunicatewithGuarantor.ToLower() == "true" && GuarantorId != "")
                                        {
                                            if ((patientContactNum != guarantorNumber)
                                                && guarantorRelation.ToLower() != "self"
                                                && guarantorNumber != "")
                                            {
                                                dr.GuarantorPhNumber = guarantorNumber;
                                            }
                                        }

                                        steps += "7::  READY to sent SMS Reminder dr.PhoneNumber::(" + dr.PhoneNumber + ") \n";
                                        dsReminders_.QuickSMSReminder.AddQuickSMSReminderRow(dr);
                                        IsSent = SendQuickSMSReminder(SharedVariable, dsReminders_).Data;
                                        steps += "8::  READY to sent SMS Reminder IsSent::(" + IsSent + ") \n";

                                    }

                                    #endregion

                                    #region " Call Reminder "

                                    else if (prefCommunication.ToLower() == "phone" && patientContactNum != "" && dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows.Count > 0 && voiceFacilityIds.Contains(dr_Appointment.FacilityId.ToString()))
                                    {
                                        steps += "6::  VOICE Reminder SELECTED:: \n";

                                        string AppointmentDate = dr_Appointment.AppointmentDate.ToString();
                                        string AppointmentTime = dr_Appointment.TimeFrom.ToString();
                                        string deliveryTime = dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows[0][dsReminders.ReminderVoiceSetting.DeliveryDateTimeColumn.ColumnName].ToString();
                                        //string TimeZone = dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows[0][dsReminders.ReminderVoiceSetting.TimeZoneColumn.ColumnName].ToString();
                                        string[] deliveryHours = deliveryTime.Split(' ');
                                        Int32 deliveryMinutes = 0;
                                        if (deliveryHours[1] == "Minute(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]);
                                        }
                                        else if (deliveryHours[1] == "Hour(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }
                                        else if (deliveryHours[1] == "Day(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 24) * 60;

                                            if (MDVUtility.ToDateTime(dr_Appointment.AppointmentDate).DayOfWeek == DayOfWeek.Monday && MDVUtility.ToInt32(deliveryHours[0]) < 2)
                                            {
                                                deliveryMinutes = (3 * 24) * 60;
                                            }
                                        }
                                        else if (deliveryHours[1] == "Week(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 168) * 60;
                                        }
                                        else
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }

                                        

                                        string[] date = AppointmentDate.Split(' ');
                                        string appDateTime = date[0] + " " + AppointmentTime;
                                        DateTime appDateNew = Convert.ToDateTime(appDateTime);
                                        appDateNew = appDateNew.AddMinutes(-deliveryMinutes);
                                        BLObject<bool> obj_ = new BLObject<bool>();
                                        obj_ = getHTMLString(SharedVariable, dsReminders.Tables[dsReminders.ReminderVoiceSetting.TableName].Rows[0][dsReminders.ReminderVoiceSetting.MessageTemplateColumn.ColumnName].ToString(), dr_Appointment.AppointmentId);
                                        if (obj_.Data == false)
                                            throw new Exception(obj_.Message);

                                        string TextTemplate = obj_.Message;

                                        // Send Call Reminder
                                        DSReminders.QuickVoiceReminderRow dr = dsReminders_.QuickVoiceReminder.NewQuickVoiceReminderRow();
                                        dr.CalleeName = MDVUtility.FormatPhoneNumber(dr_Appointment.FaclityPhoneNo);

                                        if (CommunicatewithGuarantor.ToLower() == "true" && guarantorRelation.ToLower() != "self")
                                            dr.PhoneNumber = null;
                                        else
                                            dr.PhoneNumber = patientContactNum;
                                        dr.MessageTemplate = TextTemplate;
                                        dr.IsProcessed = false;
                                        dr.RequestDelivery = appDateNew;
                                        dr.TimeZone = setting_model.TimeZone;
                                        dr.AppointmentId = MDVUtility.ToLong(dr_Appointment.AppointmentId);
                                        dr.PatientId = MDVUtility.ToLong(dr_Appointment.PatientId);
                                        dr.ProviderId = MDVUtility.ToLong(dr_Appointment.ProviderId);
                                        dr.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.CreatedOn = DateTime.Now;
                                        dr.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.ModifiedOn = DateTime.Now;
                                        dr.ReminderSetBy = "MDVision.ReminderService";

                                        if (CommunicatewithGuarantor.ToLower() == "true" && GuarantorId != "")
                                        {
                                            if ((patientContactNum != guarantorNumber)
                                                && guarantorRelation.ToLower() != "self"
                                                && guarantorNumber != "")
                                            {
                                                dr.GuarantorPhNumber = guarantorNumber;
                                            }
                                        }
                                        else
                                        {
                                            dr.GuarantorPhNumber = null;
                                        }

                                        steps += "9::  READY to sent VOICE Reminder dr.PhoneNumber::(" + dr.PhoneNumber + ") \n";
                                        dsReminders_.QuickVoiceReminder.AddQuickVoiceReminderRow(dr);
                                        IsSent = SendQuickVoiceReminder(SharedVariable, dsReminders_).Data;
                                        steps += "10::  VOICE ReminderIsSent::(" + IsSent + ") \n";
                                    }

                                    #endregion

                                    #region " Email Reminder "

                                    else if (prefCommunication.ToLower() == "email" && emailAddress != "" && dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows.Count > 0 && emailFacilityIds.Contains(dr_Appointment.FacilityId.ToString()))
                                    {
                                        steps += "6::  EMAIL Reminder SELECTED:: \n";

                                        string AppointmentDate = dr_Appointment.AppointmentDate.ToString();
                                        string AppointmentTime = dr_Appointment.TimeFrom.ToString();
                                        string deliveryTime = dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows[0][dsReminders.ReminderEmailSetting.DeliveryDateTimeColumn.ColumnName].ToString();
                                        //string TimeZone = dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows[0][dsReminders.ReminderEmailSetting.TimeZoneColumn.ColumnName].ToString();
                                        string[] deliveryHours = deliveryTime.Split(' ');
                                        Int32 deliveryMinutes = 0;
                                        if (deliveryHours[1] == "Minute(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]);
                                        }
                                        else if (deliveryHours[1] == "Hour(s)")
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }
                                        else if (deliveryHours[1] == "Day(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 24) * 60;
                                            if (MDVUtility.ToDateTime(dr_Appointment.AppointmentDate).DayOfWeek == DayOfWeek.Monday && MDVUtility.ToInt32(deliveryHours[0]) < 2)
                                            {
                                                deliveryMinutes = (3 * 24) * 60;
                                            }
                                        }
                                        else if (deliveryHours[1] == "Week(s)")
                                        {
                                            deliveryMinutes = (MDVUtility.ToInt32(deliveryHours[0]) * 168) * 60;
                                        }
                                        else
                                        {
                                            deliveryMinutes = MDVUtility.ToInt32(deliveryHours[0]) * 60;
                                        }
                                        string[] date = AppointmentDate.Split(' ');
                                        string appDateTime = date[0] + " " + AppointmentTime;
                                        DateTime appDateNew = Convert.ToDateTime(appDateTime);
                                        appDateNew = appDateNew.AddMinutes(-deliveryMinutes);
                                        BLObject<bool> obj_ = new BLObject<bool>();
                                        obj_ = getHTMLString(SharedVariable, dsReminders.Tables[dsReminders.ReminderEmailSetting.TableName].Rows[0][dsReminders.ReminderEmailSetting.MessageTemplateColumn.ColumnName].ToString(), 4);
                                        if (obj_.Data == false)
                                            throw new Exception(obj_.Message);

                                        string TextTemplate = obj_.Message;

                                        DSReminders.QuickEmailReminderRow dr = dsReminders_.QuickEmailReminder.NewQuickEmailReminderRow();

                                        dr.RequestDelivery = appDateNew;
                                        dr.FromName = "MDVISION";
                                        dr.ToEmail = emailAddress;
                                        dr.Subject = "Subject Field";
                                        //dr.TimeZone = model.EmailTimeZone_RefValue;
                                        //dr.TimeZoneId = MDVUtility.ToLong(model.EmailTimeZone);
                                        dr.MessageTemplate = TextTemplate;
                                        dr.IsProcessed = false;
                                        dr.AppointmentId = MDVUtility.ToLong(dr_Appointment.AppointmentId);
                                        dr.PatientId = MDVUtility.ToLong(dr_Appointment.PatientId);
                                        dr.ProviderId = MDVUtility.ToLong(dr_Appointment.ProviderId);
                                        dr.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        dr.CreatedOn = DateTime.Now;
                                        dr.AppointmentDate = MDVUtility.StringToDate(date[0]);
                                        dr.PatientName = dr_Appointment.PatientName;
                                        dr.ReminderSetBy = "MDVision.ReminderService";

                                        steps += "11::  READY to sent EMAIL Reminder \n";
                                        dsReminders_.QuickEmailReminder.AddQuickEmailReminderRow(dr);
                                        IsSent = SendQuickEmailReminder(SharedVariable, dsReminders_).Data;
                                        steps += "12::  EMAIL Reminder IsSent::(" + IsSent + ") \n";
                                    }

                                    #endregion

                                    #endregion
                                }

                                if (IsSent)
                                {
                                    dr_Appointment.IsReminderSent = true;
                                    ds.Merge(dsReminders_);
                                    IsAnyAppointmentChange = true;
                                }
                            }
                            else
                            {
                                steps += "4:: NO PATIENT FOUND \n";
                            }
                        }
                        else
                        {
                            throw new Exception("Provider settings not found." + obj.Message);
                        }
                    }
                    else
                    {
                        throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
                    }

                }

                steps += "Loaded_AppointmentIds::(" + Loaded_AppointmentIds + ") IsAnyAppointmentChange:(" + IsAnyAppointmentChange + ")";

                //Update Appointment
                if (IsAnyAppointmentChange)
                    dsAppontments = new DALAppointment(SharedVariable).UpdateAppointment(SharedVariable, dsAppontments);

                Exception ex = new Exception("SUCCESS Exception");
                ex.Source = ex.Source + " :: CUSTOM STEPS SUCCESS:" + steps;
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendPreferenceBasedReminders", ex);

                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                //Update Appointment
                if (IsAnyAppointmentChange)
                    dsAppontments = new DALAppointment(SharedVariable).UpdateAppointment(SharedVariable, dsAppontments);

                ex.Source = ex.Source + " :: CUSTOM STEPS EXCEPTION:" + steps;
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::SendPreferenceBasedReminders::Windows Service", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        #endregion

        #region Lookups

        public BLObject<DSClinicalNoteTemplateLookup> GetRemindersTemplateTagCategory()
        {
            try
            {
                DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
                ds = new DALReminders().GetRemindersTemplateTagCategory();
                return new BLObject<DSClinicalNoteTemplateLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetRemindersTemplateTagCategory", ex);
                return new BLObject<DSClinicalNoteTemplateLookup>(null, ex.Message);
            }
        }

        #endregion

        #endregion

        #region Reminders Template Lookup
        public BLObject<DSRemindersLookup> GetRemindersTemplateType(string type)
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                ds = new DALReminders().GetRemindersTemplateType(type);

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetRemindersTemplateType", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }
        public BLObject<DSRemindersLookup> GetWeekDays()
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                //ds = new DALProvider().LookupProvider(Active);

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetWeekDays", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }
        public BLObject<DSRemindersLookup> GetReminderConfirmationKey()
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                ds = new DALReminders().GetReminderConfirmationKey();

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetReminderConfirmationKey", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }
        public BLObject<DSRemindersLookup> GetRemindersTextVoice()
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                ds = new DALReminders().GetRemindersTextVoice();

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetRemindersTextVoice", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }
        public BLObject<DSReminders> InsertNewNoteType(DSReminders ds)
        {
            try
            {
                ds = new DALReminders().InsertNewNoteType(ds);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertNewNoteType", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSRemindersLookup> GetRemindersType()
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                ds = new DALReminders().GetRemindersType();

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetRemindersType", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }
        public BLObject<DSClinicalNoteTemplateLookup> GetReminderTemplateTagName(Int32 NoteTagCategory)
        {
            try
            {
                DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
                ds = new DALReminders().GetReminderTemplateTagName(NoteTagCategory);

                return new BLObject<DSClinicalNoteTemplateLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetReminderTemplateTagName", ex);
                return new BLObject<DSClinicalNoteTemplateLookup>(null, ex.Message);
            }

        }
        public BLObject<DSRemindersLookup> GetReminderDeliveryDateTime()
        {
            try
            {
                DSRemindersLookup ds = new DSRemindersLookup();
                ds = new DALReminders().GetReminderDeliveryDateTime();

                return new BLObject<DSRemindersLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetReminderDeliveryDateTime", ex);
                return new BLObject<DSRemindersLookup>(null, ex.Message);
            }

        }

        #endregion

        #region "getHTMLString"

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="htmlContent"></param>
        /// <param name="AppointmentId"></param>
        /// <returns></returns>
        public BLObject<bool> getHTMLString(SharedVariable SharedVariable, string htmlContent, long AppointmentId)
        {
            try
            {
                string newString = "";

                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment(SharedVariable).LoadAppointment(SharedVariable, AppointmentId, "", "", "", "");
                if (ds.Tables[ds.PatientAppointments.TableName].Rows.Count > 0)
                {
                    Int64 refProviderId = MDVUtility.ToInt64((ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.RefProviderIdColumn.ColumnName]));
                    ds.Merge(new DALPatient(SharedVariable).FillPatient(SharedVariable, MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.PatientIdColumn.ColumnName]), "", ""));
                    ds.Merge(new DALProvider(SharedVariable).LoadProvider(SharedVariable, MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.ProviderIdColumn.ColumnName]), "", "", "", "", "", "", ""));
                    ds.Merge(new DALFacility(SharedVariable).LoadFacility(SharedVariable, MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.FacilityIdColumn.ColumnName]), "", "", "", "", ""));
                    if (refProviderId > 0)
                    {
                        ds.Merge(new DALReferringProvider(SharedVariable).LoadReferringProvider(SharedVariable, refProviderId, "", "", "", "", "", 1, 15));
                    }

                    newString = getFormattedString(htmlContent, ds, refProviderId);
                }

                return new BLObject<bool>(true, newString);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::getHTMLString", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }
        public string getHTMLString(string htmlContent, long AppointmentId)
        {
            try
            {
                string newString = "";

                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadAppointment(AppointmentId, "", "", "", "");

                if (ds.Tables[ds.PatientAppointments.TableName].Rows.Count > 0)
                {
                    Int64 refProviderId = MDVUtility.ToInt64((ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.RefProviderIdColumn.ColumnName]));
                    ds.Merge(new DALPatient().FillPatient(MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.PatientIdColumn.ColumnName]), "", ""));
                    ds.Merge(new DALProvider().LoadProvider(MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.ProviderIdColumn.ColumnName]), "", "", "", "", "", "", ""));
                    ds.Merge(new DALFacility().LoadFacility(MDVUtility.ToInt64(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.FacilityIdColumn.ColumnName]), "", "", "", "", ""));
                    if (refProviderId > 0)
                    {
                        ds.Merge(new DALReferringProvider().LoadReferringProvider(refProviderId, "", "", "", "", "", 1, 15));
                    }

                    newString = getFormattedString(htmlContent, ds, refProviderId);
                }

                return newString;

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::getHTMLString", ex);
                return (ex.Message);
            }

        }
        private string getFormattedString(string htmlString, DSAppointment ds, Int64 refProviderId)
        {
            DSPatient dsPatient = new DSPatient();
            DSProfile dsProfile = new DSProfile();
            string Appointment_date = (Convert.ToDateTime(ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentDateColumn.ColumnName])).ToString("dddd, MMMM dd, yyyy").ToString();
            int index = Appointment_date.LastIndexOf("th");
            if (index > 0)
                Appointment_date = Appointment_date.Remove(index, 2);

            htmlString = htmlString.Replace("<Appointment Date>", Appointment_date);
            htmlString = htmlString.Replace("<Appointment Time>", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.TimeFromColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Appointment Duration>", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.DurationColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Appointment Status>", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.AppointmentStatusColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Appointment VisitType>", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.VisitTypeColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Appointment Reason>", ds.Tables[ds.PatientAppointments.TableName].Rows[0][ds.PatientAppointments.ReasonColumn.ColumnName].ToString());
            //-----------------------------------------
            htmlString = htmlString.Replace("<Patient First Name>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient MI>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.MIColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Prefix>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PrefixColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Last Name>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Suffix>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.SuffixColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Gender>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GenderColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Date of Birth>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.DOBColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Address 1>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.Address1Column.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Address 2>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.Address2Column.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Home Phone>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.HomePhoneNoColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Work Phone>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.WorkPhoneNoColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Cell Number>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CellNoColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Preferred Language>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PrefCommunicationIdColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Patient Full Name>", ds.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FullNameColumn.ColumnName].ToString());
            //------------------------------------------
            htmlString = htmlString.Replace("<Provider First Name>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.FirstNameColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Last Name>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.LastNameColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Phone Number>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.PhoneNoColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Address>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.DirectAddressColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Email Address>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.EmailAddressColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Specialty>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.SpecialtyIdColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Cell Number>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.CellNoColumn.ColumnName].ToString());

            htmlString = htmlString.Replace("<Provider MI>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.MiddleInitialColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider NPI>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.NPIColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider City>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.CityColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider State>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.StateColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Zip Code>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.ZIPCodeColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Zip Code Ext>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.ZIPCodeExtColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Taxonomy Code>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.TaxonomyCodeColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider State License>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.StateColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Provider Fax>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.FaxColumn.ColumnName].ToString());
            //htmlString = htmlString.Replace("<Provider Visit Date>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.CellNoColumn.ColumnName].ToString());
            //htmlString = htmlString.Replace("<Provider Visit Time>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.CellNoColumn.ColumnName].ToString());
            //htmlString = htmlString.Replace("<Provider Visit Reason>", ds.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.CellNoColumn.ColumnName].ToString());

            //------------------------------------------
            htmlString = htmlString.Replace("<Facility Short Name>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.ShortNameColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility Phone Number>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.PhoneNoColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility Phone Number Ext>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.PhoneExtColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility Address>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.AddressColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility City>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.CityColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility State>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.StateColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility Email Address>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.EmailAddressColumn.ColumnName].ToString());
            htmlString = htmlString.Replace("<Facility WebSite URL>", ds.Tables[dsProfile.Facility.TableName].Rows[0][dsProfile.Facility.WebSiteURLColumn.ColumnName].ToString());
            //-------------------------------------------
            if (refProviderId > 0)
            {
                htmlString = htmlString.Replace("<Referring Provider First Name>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.FirstNameColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Last Name>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.LastNameColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Phone Number>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.PhoneNoColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Address>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.AddressColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Email Address>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.EmailAddressColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Specialty>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.SpecialtyIdColumn.ColumnName].ToString());
                htmlString = htmlString.Replace("<Referring Provider Cell Number>", ds.Tables[dsProfile.ReferringProvider.TableName].Rows[0][dsProfile.ReferringProvider.CellColumn.ColumnName].ToString());
            }


            return htmlString;
        }
        [Serializable]
        [XmlRoot("AppointmentStatus")]
        public class AppointmentReminderStatus
        {
            [XmlElement("AppointmentId")]
            public long AppointmentId { get; set; }
            [XmlElement("Status")]
            public string Status { get; set; }
        }

        #endregion

        #region CCM 

        public BLObject<DSCCM> loadTemplates(int TemplateId, string TempLookupName, string ShortName, string Description, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().LoadCCMTemplate(TemplateId, TempLookupName, ShortName, Description, IsActive, PageNumber = 1, RowsPerPage = 1000);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::Templates", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<string> ActiveInActiveTemplate(string templateId, long isactive)
        {
            try
            {
                templateId = new MDVision.DataAccess.DAL.CCM.DALCCM().ActiveInActiveTemplate(templateId, isactive);
                return new BLObject<string>(templateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> loadICDGroups(long ICDGroupId, string ShortName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCCM ds = new DSCCM();


                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().LoadCCMICDGroups(ICDGroupId, ShortName, IsActive, PageNumber = 1, RowsPerPage = 1000);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::ICDGroups", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> LoadCCMICDGroupsDetail(long ICDGroupId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().LoadCCMICDGroupsDetail(ICDGroupId, PageNumber = 1, RowsPerPage = 1000);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::LoadCCMICDGroupsDetail", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> loadCareTeams(long CareTeamId, string ShortName, string ProviderName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCCM ds = new DSCCM();
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().LoadCCMCareTeams(CareTeamId, ShortName, ProviderName, IsActive, PageNumber = 1, RowsPerPage = 1000);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public void saveCareManagers(Int64 CareTeamId, List<Int64> lstCareManager, IDBManager dbManager = null)
        {
            //try
            //{
            Int64 TeamID = CareTeamId;

            // insert caremanager here.
            DSCCM dsCareManager = new DSCCM();
            //      DataRow drCareTeam = dsCareManager.Tables[dsCareManager.CareTeams.TableName].Rows[0];
            // DataRow drCareManager;
            DSCCM.CareManagersRow drCareManager;


            foreach (Int64 CareManagerID in lstCareManager)
            {
                drCareManager = dsCareManager.CareManagers.NewCareManagersRow();
                drCareManager[dsCareManager.CareManagers.CareManagerIdColumn.ColumnName] = CareManagerID;
                drCareManager[dsCareManager.CareManagers.CareTeamIdColumn.ColumnName] = CareTeamId;
                dsCareManager.CareManagers.AddCareManagersRow(drCareManager);
            }
            if (dsCareManager.CareManagers.Rows.Count > 0)
            {
                //dsCharge.AcceptChanges();
                dsCareManager = new MDVision.DataAccess.DAL.CCM.DALCCM().SaveCCMCareManagers(ref dsCareManager, dbManager);

            }
            // return new BLObject<DSCCM>(ds);
            //}
            //catch (Exception ex)
            //{
            //    MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
            //   // return new BLObject<DSCCM>(null, ex.Message);
            //}
        }
        public void saveCareCoordinators(Int64 CareTeamId, List<Int64> lstCareCoordinator, IDBManager dbManager = null)
        {
            try
            {
                Int64 TeamID = CareTeamId;

                // insert caremanager here.
                DSCCM dsCareCoordinator = new DSCCM();
                //      DataRow drCareTeam = dsCareManager.Tables[dsCareManager.CareTeams.TableName].Rows[0];
                // DataRow drCareManager;
                DSCCM.CareCoordinatorsRow drCareCoordinator;


                foreach (Int64 CareCoordinatorID in lstCareCoordinator)
                {
                    drCareCoordinator = dsCareCoordinator.CareCoordinators.NewCareCoordinatorsRow();
                    drCareCoordinator[dsCareCoordinator.CareCoordinators.CareCoordinatorIdColumn.ColumnName] = CareCoordinatorID;
                    drCareCoordinator[dsCareCoordinator.CareCoordinators.CareTeamIdColumn.ColumnName] = CareTeamId;
                    dsCareCoordinator.CareCoordinators.AddCareCoordinatorsRow(drCareCoordinator);
                }
                if (dsCareCoordinator.CareCoordinators.Rows.Count > 0)
                {
                    //dsCharge.AcceptChanges();
                    dsCareCoordinator = new MDVision.DataAccess.DAL.CCM.DALCCM().SaveCCMCareCoordinators(ref dsCareCoordinator, dbManager);

                }
                // return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                // return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public void saveCareGivers(Int64 CareTeamId, List<Int64> lstCareGivers, IDBManager dbManager = null)
        {
            try
            {
                Int64 TeamID = CareTeamId;

                // insert caremanager here.
                DSCCM dsCareGivers = new DSCCM();
                //      DataRow drCareTeam = dsCareManager.Tables[dsCareManager.CareTeams.TableName].Rows[0];
                // DataRow drCareManager;
                DSCCM.CareGiversRow drCareGivers;


                foreach (Int64 CareGiverID in lstCareGivers)
                {
                    drCareGivers = dsCareGivers.CareGivers.NewCareGiversRow();
                    drCareGivers[dsCareGivers.CareGivers.CareGiverIdColumn.ColumnName] = CareGiverID;
                    drCareGivers[dsCareGivers.CareGivers.CareTeamIdColumn.ColumnName] = CareTeamId;
                    dsCareGivers.CareGivers.AddCareGiversRow(drCareGivers);
                }
                if (dsCareGivers.CareGivers.Rows.Count > 0)
                {
                    //dsCharge.AcceptChanges();
                    dsCareGivers = new MDVision.DataAccess.DAL.CCM.DALCCM().SaveCCMCareGivers(ref dsCareGivers, dbManager);

                }
                // return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                // return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> saveCareTeams(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<Int64> lstcareManger = new List<Int64>();
                List<Int64> lstcareCoordinator = new List<Int64>();
                List<Int64> lstcareGiver = new List<Int64>();

                foreach (DataRow drCareManager in ds.Tables[ds.CareManagers.TableName].Rows)
                {
                    lstcareManger.Add(Convert.ToInt64(drCareManager[ds.CareManagers.CareManagerIdColumn.ColumnName]));

                }
                foreach (DataRow drCareCoordinator in ds.Tables[ds.CareCoordinators.TableName].Rows)
                {
                    lstcareCoordinator.Add(Convert.ToInt64(drCareCoordinator[ds.CareCoordinators.CareCoordinatorIdColumn.ColumnName]));

                }
                foreach (DataRow drCareGivers in ds.Tables[ds.CareGivers.TableName].Rows)
                {
                    lstcareGiver.Add(Convert.ToInt64(drCareGivers[ds.CareGivers.CareGiverIdColumn.ColumnName]));
                }

                dbManager.BeginTransaction();
                ds = new DALCCM().SaveCCMCareTeam(ref ds, dbManager);

                // insert caremanager here.
                DataRow drCareTeam = ds.Tables[ds.CareTeams.TableName].Rows[0];

                Int64 CareTeamID = Convert.ToInt64(drCareTeam[ds.CareTeams.CareTeamIdColumn.ColumnName]);
                if (lstcareManger.Count > 0)
                {
                    saveCareManagers(CareTeamID, lstcareManger, dbManager);
                    // dsCareManager = new MDVision.DataAccess.DAL.CCM.DALCCM().SaveCCMCareManagers(ref dsCareManager);
                }
                if (lstcareCoordinator.Count > 0)
                {
                    saveCareCoordinators(CareTeamID, lstcareCoordinator, dbManager);
                }

                if (lstcareGiver.Count > 0)
                {
                    saveCareGivers(CareTeamID, lstcareGiver, dbManager);
                }

                dbManager.CommitTransaction();
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string updateCareTeams(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<Int64> lstcareManger = new List<Int64>();
                List<Int64> lstcareCoordinator = new List<Int64>();
                List<Int64> lstcareGiver = new List<Int64>();

                foreach (DataRow drCareManager in ds.Tables[ds.CareManagers.TableName].Rows)
                {
                    lstcareManger.Add(Convert.ToInt64(drCareManager[ds.CareManagers.CareManagerIdColumn.ColumnName]));

                }
                foreach (DataRow drCareCoordinator in ds.Tables[ds.CareCoordinators.TableName].Rows)
                {
                    lstcareCoordinator.Add(Convert.ToInt64(drCareCoordinator[ds.CareCoordinators.CareCoordinatorIdColumn.ColumnName]));

                }
                foreach (DataRow drCareGivers in ds.Tables[ds.CareGivers.TableName].Rows)
                {
                    lstcareGiver.Add(Convert.ToInt64(drCareGivers[ds.CareGivers.CareGiverIdColumn.ColumnName]));

                }

                dbManager.BeginTransaction();
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().UpdateCCMCareTeam(ref ds, dbManager);

                // insert caremanager here.
                DataRow drCareTeam = ds.Tables[ds.CareTeams.TableName].Rows[0];

                Int64 CareTeamID = Convert.ToInt64(drCareTeam[ds.CareTeams.CareTeamIdColumn.ColumnName]);
                if (lstcareManger.Count > 0)
                {
                    saveCareManagers(CareTeamID, lstcareManger, dbManager);
                    // dsCareManager = new MDVision.DataAccess.DAL.CCM.DALCCM().SaveCCMCareManagers(ref dsCareManager);

                }
                if (lstcareCoordinator.Count > 0)
                {
                    saveCareCoordinators(CareTeamID, lstcareCoordinator, dbManager);


                }
                if (lstcareGiver.Count > 0)
                {
                    saveCareGivers(CareTeamID, lstcareGiver, dbManager);


                }
                dbManager.CommitTransaction();
                return "Success";
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public BLObject<string> deleteCareTeams(string CareTeamId)
        {
            try
            {
                CareTeamId = new MDVision.DataAccess.DAL.CCM.DALCCM().DeleteCCMCareTeam(CareTeamId);
                return new BLObject<string>(CareTeamId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::CCMCareTeams", ex);
                return new BLObject<string>(ex.Message);
            }
        }
        public BLObject<string> ActiveInActiveCareTeam(string CareTeamId, long isactive)
        {
            try
            {
                CareTeamId = new MDVision.DataAccess.DAL.CCM.DALCCM().ActiveInActiveCareTeam(CareTeamId, isactive);
                return new BLObject<string>(CareTeamId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::ActiveInActiveCareTeam", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> InsertICDGroup(ref DSCCM ds)
        {
            try
            {
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().InsertICDGroup(ref ds);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<DSCCM> UpdateICDGroup(ref DSCCM ds)
        {
            try
            {
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().UpdateICDGroup(ref ds);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<string> ActiveInActiveICDGroup(string ICDGroupId, long isactive)
        {
            try
            {
                ICDGroupId = new MDVision.DataAccess.DAL.CCM.DALCCM().ActiveInActiveICDGroupICDGroup(ICDGroupId, isactive);
                return new BLObject<string>(ICDGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteICDGroup(string ICDGroupId)
        {
            try
            {
                ICDGroupId = new MDVision.DataAccess.DAL.CCM.DALCCM().DeleteICDGroup(ICDGroupId);
                return new BLObject<string>(ICDGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DeleteICDGroupMap", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSCCM> InsertICDGroupMap(ref DSCCM ds)
        {
            try
            {
                ds = new MDVision.DataAccess.DAL.CCM.DALCCM().InsertICDGroupMap(ref ds);
                return new BLObject<DSCCM>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::InsertICDGroupMap", ex);
                return new BLObject<DSCCM>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteICDGroupMap(string ICDGroupMapId)
        {
            try
            {
                ICDGroupMapId = new MDVision.DataAccess.DAL.CCM.DALCCM().DeleteICDGroupMap(ICDGroupMapId);
                return new BLObject<string>(ICDGroupMapId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::DeleteICDGroupMap", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #region TeleVox

        private static List<string> _fileNamesAllsFtp = new List<string>();
        private static string[] _fileNamesAll;
        private static string[] _fileNamesToDownLoad;


        #region Create Message

        public string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }
        public void LoadTeleVoxFutureAppointments(SharedVariable SharedVariable, string fTp, string userName, string userPassword, string fTpPortNo, string hostKey, string uploadFolderPath)
        {
            try
            {
                var fileName = Get8Digits() + "APPTS" + DateTime.Now.Year + ".txt";
                string strPath = Path.GetTempPath() + "TeleVoxReminderFilesPlaced\\";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                DSReminders ds = new DALReminders(SharedVariable).LoadTeleVoxFutureAppointments(SharedVariable);
                DSReminders dsReminders = new DSReminders();

                if (ds.TeleVoxRequestedData.Rows.Count > 0)
                {
                    StringBuilder messages = new StringBuilder();

                    for (var i = 0; i < ds.TeleVoxRequestedData.Rows.Count; i++)
                    {
                        var messageText = TeleVoxAppointmentReminder.CreateMessageText(ds.TeleVoxRequestedData, i);
                        messages.Append(messageText);
                        messages.Append(Environment.NewLine);
                        messages.Append(Environment.NewLine);

                        #region Log FilHal Commented

                        //if (!string.IsNullOrEmpty(messageText))
                        //{
                        //DSReminders.TeleVoxLogDetailRow dr = dsReminders.TeleVoxLogDetail.NewTeleVoxLogDetailRow();
                        //dr.TeleVoxLogDetailId = -1;
                        //dr.PatientAccountNumber = ds.TeleVoxRequestedData[i][0].ToString();
                        //dr.FileName = fileName;
                        //dr.MessageBody = messageText;
                        //dsReminders.TeleVoxLogDetail.AddTeleVoxLogDetailRow(dr);
                        //BLObject<DSReminders> obj = new BLLAdmin().InsertTeleVoxLogDetail(dsReminders, SharedVariable);
                        //}

                        #endregion
                    }

                    TeleVoxAppointmentReminder.CreateFile_(fileName, messages.ToString(), strPath);

                    var objsftp = new SFTP.clsSFTP(fTp, userName, userPassword, fTpPortNo, hostKey);
                    var status = objsftp.UserLoginAuthorization_SFTP(fTp, userName, userPassword, fTpPortNo, hostKey, true);
                    if (status)
                    {
                        var isUplaoded = TeleVoxAppointmentReminder.UplaodFile_SFTP(SharedVariable, fTp, userName, userPassword, int.Parse(fTpPortNo), hostKey, strPath, uploadFolderPath, fileName, DateTime.Now, true);

                        #region Log FilHal Commented
                        //    DSReminders.TeleVoxLogRow drlog = dsReminders.TeleVoxLog.NewTeleVoxLogRow();
                        //    drlog.TeleVoxLogId = -1;
                        //    drlog.FileName = fileName;
                        //    drlog.MessageBody = messages.ToString();

                        //    dsReminders.TeleVoxLog.AddTeleVoxLogRow(drlog);
                        //    BLObject<DSReminders> objj = new BLLAdmin(SharedVariable).InsertTeleVoxLog(dsReminders, SharedVariable); 
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdmin::LoadTeleVoxFutureAppointments", ex);
            }
        }
        public BLObject<DSReminders> InsertTeleVoxLog(DSReminders ds, SharedVariable SharedVariable)
        {
            try
            {
                ds = new DALReminders(SharedVariable).InsertTeleVoxLog(ds, SharedVariable);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertTeleVoxLog", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }
        public BLObject<DSReminders> InsertTeleVoxLogDetail(DSReminders ds, SharedVariable SharedVariable)
        {
            try
            {
                ds = new DALReminders().InsertTeleVoxLogDetail(ds, SharedVariable);
                return new BLObject<DSReminders>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::insertTeleVoxLogDetail", ex);
                return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        #endregion
        public static void FilterFileList()
        {
            try
            {
                int i = 0;
                System.Text.StringBuilder tempStr = new System.Text.StringBuilder();

                for (i = 0; i < _fileNamesAll.Length; i++)
                {
                    if (_fileNamesAll[i].Contains("pdf"))
                    {
                        if (_fileNamesAll[i].Trim().Substring(_fileNamesAll[i].Trim().Length - 3, 3).ToLower() == "pdf")
                        {
                            tempStr.Append(_fileNamesAll[i].Trim() + " ");
                        }
                    }

                    _fileNamesToDownLoad = tempStr.ToString().Trim().Split(' ');
                    if (_fileNamesToDownLoad.Length == 1)
                    {
                        if (_fileNamesToDownLoad[0].Trim().Length == 0)
                        {
                            _fileNamesToDownLoad = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTeleVoxAppointment(SharedVariable SharedVariable, string fTp, string userName, string userPassword, string fTpPortNo, string hostKey, string ftpFolderPath)
        {
            try
            {
                bool status = false; // shit
                SFTP.clsSFTP objsftp = new SFTP.clsSFTP(fTp, userName, userPassword, fTpPortNo.ToString(), hostKey);
                status = objsftp.UserLoginAuthorization_SFTP(fTp, userName, userPassword, fTpPortNo.ToString(), hostKey, true);
                string strPath = Path.GetTempPath() + "\\TeleVoxTempFiles";
                if (status)
                {
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    _fileNamesAllsFtp = objsftp.GetfilesTeleVox(fTp, userName, userPassword, hostKey, ftpFolderPath /*"Home:/OUT"*/, strPath);
                    _fileNamesAll = _fileNamesAllsFtp.ToArray();
                    FilterFileList();
                }

                for (int i = 0; i < _fileNamesAll.Count(); i++)
                {
                    IEnumerable<string> lines = File.ReadLines(strPath + "\\" + _fileNamesAll[i].Split('/').Last());
                    using (var sequenceEnum = lines.GetEnumerator())
                    {
                        while (sequenceEnum.MoveNext())
                        {
                            var appointmentResponse = sequenceEnum.Current;
                            var appointmentResponse_ = appointmentResponse.Split(',');

                            #region Love Birds

                            var teleVox = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("AppointmentDate", appointmentResponse_.ElementAtOrDefault(0) != null ? appointmentResponse_[0] : ""),
                            new KeyValuePair<string, string>("AppointmentTime", appointmentResponse_.ElementAtOrDefault(1) != null ? appointmentResponse_[1]: ""),
                            new KeyValuePair<string, string>("ClientsLastName", appointmentResponse_.ElementAtOrDefault(2) != null ? appointmentResponse_[2]: ""),
                            new KeyValuePair<string, string>("ClientsFirstName", appointmentResponse_.ElementAtOrDefault(3) != null ? appointmentResponse_[3]: ""),
                            new KeyValuePair<string, string>("HomePhoneNumber", appointmentResponse_.ElementAtOrDefault(4) != null ? appointmentResponse_[4]: ""),
                            new KeyValuePair<string, string>("MessageNumber", appointmentResponse_.ElementAtOrDefault(5) != null ? appointmentResponse_[5]: ""),
                            new KeyValuePair<string, string>("CallStatusDescription", appointmentResponse_.ElementAtOrDefault(6) != null ? appointmentResponse_[6]: ""),
                            new KeyValuePair<string, string>("ContactedDate", appointmentResponse_.ElementAtOrDefault(7) != null ? appointmentResponse_[7]: ""),
                            new KeyValuePair<string, string>("ContactedTime", appointmentResponse_.ElementAtOrDefault(8) != null ? appointmentResponse_[8]: ""),
                            new KeyValuePair<string, string>("MessageDescription", appointmentResponse_.ElementAtOrDefault(9) != null ? appointmentResponse_[9]: ""),
                            new KeyValuePair<string, string>("HouseCallsClientNumber", appointmentResponse_.ElementAtOrDefault(10) != null ? appointmentResponse_[10]: ""),
                            new KeyValuePair<string, string>("ClientsNickName", appointmentResponse_.ElementAtOrDefault(11) != null ? appointmentResponse_[11]: ""),
                            new KeyValuePair<string, string>("ClientsReferenceNumberfromImport", appointmentResponse_.ElementAtOrDefault(12) != null ? appointmentResponse_[12]: ""),
                            new KeyValuePair<string, string>("CallStatusCode", appointmentResponse_.ElementAtOrDefault(13) != null ? appointmentResponse_[13]: ""),
                            new KeyValuePair<string, string>("ProviderNumber", appointmentResponse_.ElementAtOrDefault(14) != null ? appointmentResponse_[14]: ""),
                            new KeyValuePair<string, string>("ProviderName", (appointmentResponse_.ElementAtOrDefault(15) != null && appointmentResponse_.ElementAtOrDefault(16) != null) ? appointmentResponse_[15] + ", " + appointmentResponse_[16] : ""),
                            new KeyValuePair<string, string>("LocationNumber", appointmentResponse_.ElementAtOrDefault(17) != null ? appointmentResponse_[17]: ""),
                            new KeyValuePair<string, string>("LocationName", appointmentResponse_.ElementAtOrDefault(18) != null ? appointmentResponse_[18]: ""),
                            new KeyValuePair<string, string>("ReasonNumber", appointmentResponse_.ElementAtOrDefault(19) != null ? appointmentResponse_[19]: ""),
                            new KeyValuePair<string, string>("ReasonName", appointmentResponse_.ElementAtOrDefault(20) != null ? appointmentResponse_[20]: ""),
                            new KeyValuePair<string, string>("AppointmentId", appointmentResponse_.ElementAtOrDefault(21) != null ? appointmentResponse_[21]: ""),
                            new KeyValuePair<string, string>("AppointmentNotes", appointmentResponse_.ElementAtOrDefault(22) != null ? appointmentResponse_[22]: ""),
                            new KeyValuePair<string, string>("AppointmentColumn", appointmentResponse_.ElementAtOrDefault(23) != null ? appointmentResponse_[23]: ""),
                            new KeyValuePair<string, string>("ClientsBirthDate", appointmentResponse_.ElementAtOrDefault(24) != null ? appointmentResponse_[24]: ""),
                            new KeyValuePair<string, string>("ClientNotes", appointmentResponse_.ElementAtOrDefault(25) != null ? appointmentResponse_[25]: ""),
                            new KeyValuePair<string, string>("NumberofCallAttempts", appointmentResponse_.ElementAtOrDefault(26) != null ? appointmentResponse_[26]: ""),
                            new KeyValuePair<string, string>("ClientAddress", appointmentResponse_.ElementAtOrDefault(27) != null ? appointmentResponse_[27]: ""),
                            new KeyValuePair<string, string>("ClientCity", appointmentResponse_.ElementAtOrDefault(28) != null ? appointmentResponse_[28]: ""),
                            new KeyValuePair<string, string>("ClientState", appointmentResponse_.ElementAtOrDefault(29) != null ? appointmentResponse_[29]: ""),
                            new KeyValuePair<string, string>("ClientZip", appointmentResponse_.ElementAtOrDefault(30) != null ? appointmentResponse_[30]: ""),
                            new KeyValuePair<string, string>("EmailAddress", appointmentResponse_.ElementAtOrDefault(31) != null ? appointmentResponse_[31]: ""),
                            new KeyValuePair<string, string>("EmailStatus", appointmentResponse_.ElementAtOrDefault(32) != null ? appointmentResponse_[32]: ""),
                            new KeyValuePair<string, string>("EmailedDate", appointmentResponse_.ElementAtOrDefault(33) != null ? appointmentResponse_[33]: ""),
                            new KeyValuePair<string, string>("EmailedTime", appointmentResponse_.ElementAtOrDefault(34) != null ? appointmentResponse_[34]: ""),
                            new KeyValuePair<string, string>("SMSPhone", appointmentResponse_.ElementAtOrDefault(35) != null ? appointmentResponse_[35]: ""),
                            new KeyValuePair<string, string>("SMSStatus", appointmentResponse_.ElementAtOrDefault(36) != null ? appointmentResponse_[36]: ""),
                            new KeyValuePair<string, string>("SMSDeliveredDate", appointmentResponse_.ElementAtOrDefault(37) != null ? appointmentResponse_[37]: ""),
                            new KeyValuePair<string, string>("SMSDeliveredTime", appointmentResponse_.ElementAtOrDefault(38) != null ? appointmentResponse_[38]: ""),
                            new KeyValuePair<string, string>("FileName", "accc.txt"),
                            new KeyValuePair<string, string>("MessageBody", appointmentResponse),
                            new KeyValuePair<string, string>("CreatedBy", "TeleVox")
                        };

                            #endregion

                            #region Let's mingle 

                            DSReminders dsReminder = new DSReminders();
                            DSReminders.TeleVoxMessageLogRow dr = dsReminder.TeleVoxMessageLog.NewTeleVoxMessageLogRow();
                            dr.TeleVoxMessageLogId = -1;
                            dr.AppointmentDate = teleVox.FirstOrDefault(v => v.Key.Equals("AppointmentDate")).Value.Replace("\"", "");
                            dr.AppointmentTime = teleVox.FirstOrDefault(v => v.Key.Equals("AppointmentTime")).Value.Replace("\"", "");
                            dr.ClientsLastName = teleVox.FirstOrDefault(v => v.Key.Equals("ClientsLastName")).Value.Replace("\"", "");
                            dr.ClientsFirstName = teleVox.FirstOrDefault(v => v.Key.Equals("ClientsFirstName")).Value.Replace("\"", "");
                            dr.HomePhoneNumber = teleVox.FirstOrDefault(v => v.Key.Equals("HomePhoneNumber")).Value.Replace("\"", "");
                            dr.MessageNumber = teleVox.FirstOrDefault(v => v.Key.Equals("MessageNumber")).Value.Replace("\"", "");
                            dr.CallStatusDescription = teleVox.FirstOrDefault(v => v.Key.Equals("CallStatusDescription")).Value.Replace("\"", "");
                            dr.ContactedDate = teleVox.FirstOrDefault(v => v.Key.Equals("ContactedDate")).Value.Replace("\"", "");
                            dr.ContactedTime = teleVox.FirstOrDefault(v => v.Key.Equals("ContactedTime")).Value.Replace("\"", "");
                            dr.MessageDescription = teleVox.FirstOrDefault(v => v.Key.Equals("MessageDescription")).Value.Replace("\"", "");
                            dr.HouseCallsClientNumber = teleVox.FirstOrDefault(v => v.Key.Equals("HouseCallsClientNumber")).Value.Replace("\"", "");
                            dr.ClientsNickName = teleVox.FirstOrDefault(v => v.Key.Equals("ClientsNickName")).Value.Replace("\"", "");
                            dr.ClientsReferenceNumberfromImport = teleVox.FirstOrDefault(v => v.Key.Equals("ClientsReferenceNumberfromImport")).Value.Replace("\"", "");
                            dr.CallStatusCode = teleVox.FirstOrDefault(v => v.Key.Equals("CallStatusCode")).Value.Replace("\"", "");
                            dr.ProviderNumber = teleVox.FirstOrDefault(v => v.Key.Equals("ProviderNumber")).Value.Replace("\"", "");
                            dr.ProviderName = teleVox.FirstOrDefault(v => v.Key.Equals("ProviderName")).Value.Replace("\"", "");
                            dr.LocationNumber = teleVox.FirstOrDefault(v => v.Key.Equals("LocationNumber")).Value.Replace("\"", "");
                            dr.LocationName = teleVox.FirstOrDefault(v => v.Key.Equals("LocationName")).Value.Replace("\"", "");
                            dr.ReasonNumber = teleVox.FirstOrDefault(v => v.Key.Equals("ReasonNumber")).Value.Replace("\"", "");
                            dr.ReasonName = teleVox.FirstOrDefault(v => v.Key.Equals("ReasonName")).Value.Replace("\"", "");
                            dr.AppointmentId = teleVox.FirstOrDefault(v => v.Key.Equals("AppointmentId")).Value.Replace("\"", "");
                            dr.AppointmentNotes = teleVox.FirstOrDefault(v => v.Key.Equals("AppointmentNotes")).Value.Replace("\"", "");
                            dr.AppointmentColumn = teleVox.FirstOrDefault(v => v.Key.Equals("AppointmentColumn")).Value.Replace("\"", "");
                            dr.ClientsBirthDate = teleVox.FirstOrDefault(v => v.Key.Equals("ClientsBirthDate")).Value.Replace("\"", "");
                            dr.ClientNotes = teleVox.FirstOrDefault(v => v.Key.Equals("ClientNotes")).Value.Replace("\"", "");
                            dr.NumberofCallAttempts = teleVox.FirstOrDefault(v => v.Key.Equals("NumberofCallAttempts")).Value.Replace("\"", "");
                            dr.ClientAddress = teleVox.FirstOrDefault(v => v.Key.Equals("ClientAddress")).Value.Replace("\"", "");
                            dr.ClientCity = teleVox.FirstOrDefault(v => v.Key.Equals("ClientCity")).Value.Replace("\"", "");
                            dr.ClientState = teleVox.FirstOrDefault(v => v.Key.Equals("ClientState")).Value.Replace("\"", "");
                            dr.ClientZip = teleVox.FirstOrDefault(v => v.Key.Equals("ClientZip")).Value.Replace("\"", "");
                            dr.EmailAddress = teleVox.FirstOrDefault(v => v.Key.Equals("EmailAddress")).Value.Replace("\"", "");
                            dr.EmailStatus = teleVox.FirstOrDefault(v => v.Key.Equals("EmailStatus")).Value.Replace("\"", "");
                            dr.EmailedDate = teleVox.FirstOrDefault(v => v.Key.Equals("EmailedDate")).Value.Replace("\"", "");
                            dr.EmailedTime = teleVox.FirstOrDefault(v => v.Key.Equals("EmailedTime")).Value.Replace("\"", "");
                            dr.SMSPhone = teleVox.FirstOrDefault(v => v.Key.Equals("SMSPhone")).Value.Replace("\"", "");
                            dr.SMSStatus = teleVox.FirstOrDefault(v => v.Key.Equals("SMSStatus")).Value.Replace("\"", "");
                            dr.SMSDeliveredDate = teleVox.FirstOrDefault(v => v.Key.Equals("SMSDeliveredDate")).Value.Replace("\"", "");
                            dr.SMSDeliveredTime = teleVox.FirstOrDefault(v => v.Key.Equals("SMSDeliveredTime")).Value.Replace("\"", "");
                            dr.FileName = teleVox.FirstOrDefault(v => v.Key.Equals("FileName")).Value.Replace("\"", "");
                            dr.MessageBody = teleVox.FirstOrDefault(v => v.Key.Equals("MessageBody")).Value.Replace("\"", "");
                            dr.CreatedBy = teleVox.FirstOrDefault(v => v.Key.Equals("CreatedBy")).Value.Replace("\"", "");
                            dsReminder.TeleVoxMessageLog.AddTeleVoxMessageLogRow(dr);

                            #endregion

                            #region Much ado about nothing

                            DSReminders ds = new DALReminders(SharedVariable).UpdateTeleVoxAppointments(SharedVariable, dsReminder);

                            #endregion

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::UpdateTeleVoxAppointments", ex);
                //return new BLObject<DSReminders>(null, ex.Message);
            }
        }

        #endregion
     }
}
