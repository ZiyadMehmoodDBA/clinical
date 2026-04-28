using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls;
using MDVision.IEHR.Controls.Admin;
using MDVision.IEHR.Model.Admin.Reminders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Admin.Reminders
{
    public class RemindersController : ApiController
    {
        [HttpPost]
        public string RemindersTemplate(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RemindersTemplateModel templatesModel = ser.Deserialize<RemindersTemplateModel>(MDVUtility.ToStr(objData["data"]));

                if (templatesModel.commandType.ToLower() == "save_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().SaveRemindersTemplate(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (templatesModel.commandType.ToLower() == "update_reminder_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().UpdateRemindersTemplate(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (templatesModel.commandType.ToLower() == "save_note_type")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().InsertNewNoteType(MDVUtility.ToStr(templatesModel.NoteTypeText));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }


            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
        [HttpPost]
        public string Reminders(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RemindersTemplateModel templatesModel = ser.Deserialize<RemindersTemplateModel>(MDVUtility.ToStr(objData["data"]));

                if (templatesModel.commandType.ToLower() == "search_reminders_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().SearchRemindersTemplates(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (templatesModel.commandType.ToLower() == "delete_reminders_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().DeleteRemindersTemplates(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (templatesModel.commandType.ToLower() == "fill_reminders_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().FillRemindersTemplates(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (templatesModel.commandType.ToLower() == "activeinactive_reminders_template")
                {
                    string response = null;
                    response = Admin_RemindersTemplates.Instance().ActiveInactiveTemplates(templatesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string Settings(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RemindersSettingModel SettingsModel = ser.Deserialize<RemindersSettingModel>(MDVUtility.ToStr(objData["data"]));

                if (SettingsModel.commandType.ToLower() == "search_reminders_template")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().SearchRemindersSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "delete_email_reminders_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().DeleteRemindersEmailSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

                if (SettingsModel.commandType.ToLower() == "save_email_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().SaveRemindersEmailSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "update_email_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().UpdateRemindersEmailSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "fill_email_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().FillRemindersemail(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "save_text_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().SaveRemindersTextSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "delete_text_reminders_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().DeleteRemindersTextSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "fill_text_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().FillRemindersText(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "update_text_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().UpdateRemindersTextSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "delete_voice_reminders_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().DeleteRemindersVoiceSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "save_voice_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().SaveRemindersVoiceSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "update_voice_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().UpdateRemindersVoiceSettings(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "fill_voice_settings")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().FillRemindersVoice(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "update_reminders_settings_activeinactive")
                {
                    string response = null;
                    response = Admin_RemindersSettings.Instance().UpdateRemindersSettingsActiveInactive(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string QuickReminder(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RemindersQuickModel SettingsModel = ser.Deserialize<RemindersQuickModel>(MDVUtility.ToStr(objData["data"]));

                if (SettingsModel.commandType.ToLower() == "send_quick_voice")
                {
                    string response = null;
                    response = Admin_Reminders_Detail.Instance().SaveQuickVoiceReminder(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "send_quick_sms")
                {
                    string response = null;
                    response = Admin_Reminders_Detail.Instance().SaveQuickSMSReminder(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "send_quick_email")
                {
                    string response = null;
                    response = Admin_Reminders_Detail.Instance().SaveQuickEmailReminder(SettingsModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (SettingsModel.commandType.ToLower() == "get_html_string")
                {
                    string response = null;
                    response = Admin_Reminders_Detail.Instance().getHTMLString(SettingsModel); 
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
