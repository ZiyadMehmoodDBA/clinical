using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.Clinical;
using MDVision.IEHR.EMR.Model.Templates;
using Newtonsoft.Json;
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

namespace MDVision.IEHR.EMR.Services
{
    public class TemplateBuilderController : ApiController
    {

        [HttpPost]
        public string ClinicalQuestion(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            TemplateBuilderModel model = ser.Deserialize<TemplateBuilderModel>(MDVUtility.ToStr(AllData["data"]));

            TemplateBuilderHelper helperTemplateBuilder = new TemplateBuilderHelper();


            if (model.commandType.ToLower() == "search_question")
            {
                response = null;
                response = helperTemplateBuilder.SearchQuestion(model, MDVUtility.ToInt32(model.questionID), MDVUtility.ToInt32(model.rpp), MDVUtility.ToInt32(model.PageNo));
            }
            else if (model.commandType.ToLower() == "save_question")
            {
                response = null;
                response = helperTemplateBuilder.SaveQuestion(model, model.file);
            }
            else if (model.commandType.ToLower() == "fill_question")
            {
                response = null;
                response = helperTemplateBuilder.FillQuestion(MDVUtility.ToInt64(model.questionID));
            }
            else if (model.commandType.ToLower() == "update_question")
            {
                response = null;
                response = helperTemplateBuilder.UpdateQuestion(model, MDVUtility.ToInt64(model.questionID), model.file);
            }
            else if (model.commandType.ToLower() == "delete_question")
            {
                response = null;
                response = helperTemplateBuilder.DeleteQuestion(model.questionID);
            }
            return response;

        }
        [HttpPost]
        public string ClinicalLetterTemplate(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            LetterTemplateModel model = ser.Deserialize<LetterTemplateModel>(MDVUtility.ToStr(AllData["data"]));

            LetterTemplateHelper helperLT = new LetterTemplateHelper();


            if (model.commandType.ToLower() == "search_letter_templates")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Letter", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLT.loadLetterTemplates(MDVUtility.ToInt64(model.TemplateLetterId), model.IsActive, MDVUtility.ToStr(model.Name), MDVUtility.ToStr(model.Description), MDVUtility.ToInt32(model.ddlCategory), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "get_patletter_forsoap")
            {
               response = helperLT.getPatLettersForSoap(model.TemplateLetterIds, MDVUtility.ToLong(model.PatientId));
            }
            else if (model.commandType.ToLower() == "attach_pat_letter_with_notes")
            {
                response = helperLT.attachPatLetterWithNote(model.TemplateLetterIds, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "detach_pat_letter_with_notes")
            {
                response = helperLT.detachLabOrderFromNotes(model.TemplateLetterIds, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "save_template_letter")//
            {
                /*response = null;
                response = helperLT.SaveTemplateLetter(model);*/
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Letter", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperLT.SaveTemplateLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "delete_letter_template")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Letter", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLT.deleteLetterTemplate(model.TemplateLetterId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "get_detail_of_template_letter")
            {
                response = null;
                response = helperLT.GetTemplateLetterData(model.TemplateLetterId);
            }
            else if (model.commandType.ToLower() == "update_template_letter")//
            {
                /*response = null;
                response = helperLT.UpdateTemplateLetter(model);*/
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Letter", "ADIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperLT.UpdateTemplateLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "activeinactive_template_letter")
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Letter", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = helperLT.ActiveInactiveTemplateLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }
            return response;

        }
        /// <summary>
        /// Author : Khaleel Ur Rehman.
        /// Purpose : To Handle operations for Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="AllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string PatientTemplateLetter(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            LetterTemplateModel model = ser.Deserialize<LetterTemplateModel>(MDVUtility.ToStr(AllData["data"]));

            PatientTemplateHelper helper = new PatientTemplateHelper();

            string privilegasMessage = "";
            if (model.commandType.ToLower() == "search_patient_templateletter")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Letter", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.loadPatientTemplateLetter(MDVUtility.ToInt64(model.Patient_Letter_Id), MDVUtility.ToInt64(model.PatientId), model.IsActive, MDVUtility.ToStr(model.Name), MDVUtility.ToStr(model.Description), MDVUtility.ToInt32(model.ddlCategory), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage),model.VisitDate, model.VisitDateTo, MDVUtility.ToInt64(model.NotesId));
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "get_content_of_letter")
            {
                response = null;
                response = helper.GetPatientLetterContent(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.TemplateLetterId), model.mode, MDVUtility.ToInt64(model.Patient_Letter_Id), model.ProviderId, model.LabOrderResultId == null ? 0 : Convert.ToInt64(model.LabOrderResultId) , model.LOINC);

            }
            else if (model.commandType.ToLower() == "sign_pat_letter")
            {
                response = null;
                response = helper.SignPatLetter(MDVUtility.ToInt64(model.NotesId), model.SignedText, MDVUtility.ToInt64(model.PatientId));
            }
            else if (model.commandType.ToLower() == "merge_base64_content")
            {
                response = null;
                response = helper.MergeTwoBase64Content(model.LabLetterBase64, model.LabResultBase64);

            }
            else if (model.commandType.ToLower() == "delete_patient_letter")
            {

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Letter", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.deletePatientLetter(MDVUtility.ToInt64(model.Patient_Letter_Id));
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "activeinactive_patient_letter")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Letter", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = null;
                    response = helper.ActiveInactivePatientLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "save_patient_letter")
            {
                response = null;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Letter", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helper.SavePatientLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "update_patient_letter")
            {
                response = null;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Letter", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helper.UpdatePatientLetter(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }

            return response;
        }

        [HttpPost]
        public string ProviderNoteTemplate(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ProviderNoteTemplateModel model = ser.Deserialize<ProviderNoteTemplateModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "search_notes_template")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Provider Note Template", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ProviderNoteTemplateHelper.Instance().loadNoteTemplate(model);
                }

            }
            else if (model.commandType.ToLower() == "update_clinical_notes_template_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Provider Note Template", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ProviderNoteTemplateHelper.Instance().updateClinical_NotesTemplateIsActive(model.NotesTemplateId, model.IsActive, model.EntityId);
                }

            }
            else if (model.commandType.ToLower() == "delete_clinical_notes_template")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Provider Note Template", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ROSTemplateId = MDVUtility.ToInt64(model.NotesTemplateId);
                    response = ProviderNoteTemplateHelper.Instance().deleteClinical_NotesTemplate(ROSTemplateId);
                }
            }
            else if (model.commandType.ToLower() == "save_note_template")
            {
                privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Provider Note Template", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ProviderNoteTemplateHelper.Instance().SaveProviderNoteTemplate(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }

            else if (model.commandType.ToLower() == "update_note_template")
            {
                privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Provider Note Template", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ProviderNoteTemplateHelper.Instance().updateNotesTemplateInfo(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (JsonConvert.SerializeObject(responseObj));
                }
            }

            else if (model.commandType.ToLower() == "save_note_type")
            {
                response = ProviderNoteTemplateHelper.Instance().InsertNewNoteType(model.NoteTypeText);
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }
    }
}