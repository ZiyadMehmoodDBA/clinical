/*
    Author: Muhammad Azhar Shahzad
    Creation Date: November 18,2015
    OverView:This File Is created for Clinical Notes
*/
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.Model.Clinical.Notes;
using MDVision.IEHR.EMR.Model.Medical;
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
using MDVision.IEHR.EMR.Model.Clinical;
using MDVision.Common.Shared;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Common;
using MDVision.IEHR.Security;
using MDVision.Model.Clinical.Treatment;
using MDVision.IEHR.EMR.Helpers.Clinical.Treatment;

namespace MDVision.IEHR.EMR.Services
{
    public class ClinicalNotesController : ApiController
    {


        [HttpPost]
        public string ClinicalNotes(JObject AllData)
        {
            string response = string.Empty;
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            if (!RequestModel.IsLogIn)
            {
                var responseObj = new
                {
                    status = false
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;

            ClinicalNotesFillModel model = new ClinicalNotesFillModel();
            List<CQMModel> lstcqmmodel = new List<CQMModel>();
            CQMModel cqmmodel = new CQMModel();
            VBPModel vbpmodel = new VBPModel();

            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<ClinicalNotesFillModel>(MDVUtility.ToStr(AllData["data"]));
                    cqmmodel = ser.Deserialize<CQMModel>(MDVUtility.ToStr(AllData["data"]));
                    vbpmodel = ser.Deserialize<VBPModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {
                    model = new ClinicalNotesFillModel();
                }
            }

            #region support variables.

            string privilegasMessage = string.Empty;
            Int64 PatientID = MDVUtility.ToInt64(model.PatientId);
            Int64 NotesID = MDVUtility.ToInt64(model.NotesId);
            Int32 PageNumber = MDVUtility.ToInt32(model.PageNumber);
            Int32 RowsPerPage = MDVUtility.ToInt32(model.RowsPerPage);
            Int64 AppointmentID = MDVUtility.ToInt64(model.AppointmentID);
            Int64 IsActive = MDVUtility.ToInt64(model.IsActive);
            Int64 VisitId = MDVUtility.ToInt64(model.VisitId);
            Int64 TemplateId = MDVUtility.ToInt64(model.TemplateId);
            Int32 isPhoneEncounter = 0;
            string NoteStatus = model.NoteStatus;

            if (model.isPhoneEncounter == true)
            {
                isPhoneEncounter = 1;
            }

            if (string.IsNullOrEmpty(NoteStatus))
            {
                NoteStatus = null;
            }

            #endregion

            if (cqmmodel.commandType.ToLower() == "save_cqm_reasoning")
            {
                response = ClinicalNotesHelper.Instance().saveCQMReasonValue(cqmmodel);
            }
            if (cqmmodel.commandType.ToLower() == "save_vbp_reasoning")
            {
                response = ClinicalNotesHelper.Instance().saveVBPReasonValue(vbpmodel);
            }
            if(cqmmodel.commandType.ToLower() == "save_vbp_depression")
            {
                response = ClinicalNotesHelper.Instance().saveVBPDepressionValue(vbpmodel);
            }
            if (model.commandType.ToLower() == "load_cqm_with_reasoning")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    Int64 reportType = MDVUtility.ToInt64(model.reportType);
                    Int32 eitherDetail = MDVUtility.ToInt32(model.eitherDetail);
                    Int64 NoteId = MDVUtility.ToInt32(model.NotesId);

                    response = ClinicalNotesHelper.Instance().loadCQMWWithReasoning(ProviderId, model.from, model.to, model.VisitId, model.PatientId, reportType, model.cqmId, eitherDetail, NoteId);

                }
            }
            if (model.commandType.ToLower() == "load_vbp_with_reasoning")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    Int64 reportType = MDVUtility.ToInt64(model.reportType);
                    Int32 eitherDetail = MDVUtility.ToInt32(model.eitherDetail);
                    Int64 NoteId = MDVUtility.ToInt32(model.NotesId);

                    response = ClinicalNotesHelper.Instance().loadCQMWWithReasoning(ProviderId, model.from, model.to, model.VisitId, model.PatientId, reportType, model.cqmId, eitherDetail, NoteId, "1");

                }
            }
            if (model.commandType.ToLower() == "load_provider_vbp_measures")
            {
                Int64 ProviderId = MDVUtility.ToInt32(model.ProviderId);
                response = ClinicalNotesHelper.Instance().loadProviderVBPMeasures(ProviderId);
            }
            if (model.commandType.ToLower() == "load_vbp_score")
            {
                Int64 NoteId = MDVUtility.ToInt32(model.NotesId);
                response = ClinicalNotesHelper.Instance().loadVBPScore(NoteId, model.MeasureNumber);
            }
            if (model.commandType.ToLower() == "load_vbp_measurequestionnaireanswers")
            {
                response = ClinicalNotesHelper.Instance().loadVBPMeasureQuestionnaireAnswers(cqmmodel);
            }
            if (model.commandType.ToLower() == "load_patients_cqm")
            {
                response = ClinicalNotesHelper.Instance().loadCQMPatientData(model.PatientIds);
            }
            if (model.commandType.ToLower() == "load_patient_bmi")
            {
                response = ClinicalNotesHelper.Instance().loadPatientBMI(cqmmodel);
            }
            if (model.commandType.ToLower() == "get_patient_recent_note")
            {
                response = ClinicalNotesHelper.Instance().getPatientRecentNote(cqmmodel);
            }
            if (model.commandType.ToLower() == "load_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    // response = ClinicalNotesHelper.Instance().loadClinical_Notes_Obsolete(PatientID, NotesID, 0, PageNumber, RowsPerPage, isPhoneEncounter, NoteStatus);
                    response = ClinicalNotesHelper.Instance().loadClinical_Notes(PatientID, NotesID, 0, PageNumber, RowsPerPage, isPhoneEncounter, NoteStatus);
                }
            }
            if (model.commandType.ToLower() == "load_clinical_notes_dates")
            {
                response = ClinicalNotesHelper.Instance().GetNotesDates(model);
            }

            if (model.commandType.ToLower() == "search_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().searchClinical_Notes(PatientID, NotesID, 0, PageNumber, RowsPerPage, isPhoneEncounter);
                }
            }
            if (model.commandType.ToLower() == "search_clinical_notes_casereporting")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().searchClinical_NotesCaseReport(PatientID, NotesID, 0, PageNumber, RowsPerPage, isPhoneEncounter);
                }
            }
            if (model.commandType.ToLower() == "search_notecomponentsname")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().loadNoteComponentsName(MDVUtility.ToInt64(model.NotesId));
                return response;
            }
            if (model.commandType.ToLower() == "fill_patient_info")
            {
                response = ClinicalNotesHelper.Instance().FillPatientInfo(PatientID);
            }
            else if (model.commandType.ToLower() == "save_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "ADD")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().saveClinical_Notes(model, PatientID);
                }
            }
            //else if (model.commandType.ToLower() == "fill_clinical_notes")
            //{
            //    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "VIEW")).ToString();

            //    if (string.IsNullOrEmpty(privilegasMessage))
            //    {
            //        response = ClinicalNotesHelper.Instance().fillClinical_Notes(NotesID, PatientID);
            //    }
            //}
            else if (model.commandType.ToLower() == "fill_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "VIEW")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().fillClinical_Note_By_Id(NotesID, PatientID);
                }
            }
            else if (model.commandType.ToLower() == "load_clinical_progress_note")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "VIEW")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().loadClinical_Note_By_Id(NotesID, PatientID, MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.OrderSetId), MDVUtility.ToBool(model.IsPreviousNoteROS), MDVUtility.ToBool(model.IsPreviousNotePE), MDVUtility.ToBool(model.IsPreviousNoteComplaints), MDVUtility.ToBool(model.IsPreviousNoteProblems));
                }
            }
            else if (model.commandType.ToLower() == "get_clinical_note_component_html")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "VIEW")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().getClinical_Note_Component_HTML(NotesID, model.ComponentName);
                }
            }
            else if (model.commandType.ToLower() == "update_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().updateClinical_Notes(model, NotesID, PatientID, AppointmentID);
                }
            }
            else if (model.commandType.ToLower() == "checkifanyordersetisassociatedwithnote")
            {
                response = ClinicalNotesHelper.Instance().IsOrderSetAssociatedWithNote(model.NotesId);
            }
            else if (model.commandType.ToLower() == "update_visit_type")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 PatientVisitTypeId = model.PatientVisitType == "" ? 0 : Convert.ToInt64(model.PatientVisitType);
                    response = ClinicalNotesHelper.Instance().updateVisitType(NotesID, PatientVisitTypeId, AppointmentID);
                }
            }
            else if (model.commandType.ToLower() == "unsign_note")
            {
                response = ClinicalNotesHelper.Instance().unsignClinical_Notes(NotesID);

            }
            else if (model.commandType.ToLower() == "load_template_html")
            {
                long UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                response = ClinicalNotesHelper.Instance().loadNoteWithNewTemplate(NotesID, PatientID, TemplateId, UserId, MDVUtility.ToInt64(model.ProviderId), model.ComponentsIdsString);
            }
            else if (model.commandType.ToLower() == "remove_components")
            {
                response = ClinicalNotesHelper.Instance().removeComponents(model, NotesID);
            }
            else if (model.commandType.ToLower() == "update_clinical_notes_progressnotehtml")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().updateClinical_Notes_progressNoteHTML(model, NotesID, PatientID, AppointmentID);
                }
            }
            else if (model.commandType.ToLower() == "load_clinical_note_info")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().loadClinical_NoteInfo(NotesID);
                }
            }
            else if (model.commandType.ToLower() == "get_clinical_note_component_access")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().getClinical_NoteComponentAccess(NotesID, model.ComponentName, model.NoteAccessTime);
                }
            }
            else if (model.commandType.ToLower() == "clinical_note_component_access_action")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().Clinical_NoteComponentAccessAction(NotesID, model.ComponentName, model.Action, model.PriorUserId, model.PriorUserName);
                }
            }
            else if (model.commandType.ToLower() == "remove_user_note_access")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().Clinical_RemoveUserNoteAccess(model.IsComponentOnly, model.ComponentName);
                }
            }
            else if (model.commandType.ToLower() == "check_note_is_accessd_by_others")
            {
                response = ClinicalNotesHelper.Instance().Clinical_NoteAccessByOthers(model.NotesId);
            }
            else if (model.commandType.ToLower() == "remove_users_against_note")
            {
                response = ClinicalNotesHelper.Instance().RemoveUsersAgainstNote(model.NotesId);
            }
            else if (model.commandType.ToLower() == "update_clinical_notes_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().updateClinical_NotesIsActive(NotesID, PageNumber, RowsPerPage, IsActive);
                }
            }
            else if (model.commandType.ToLower() == "delete_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().deleteClinical_Notes(NotesID);
                }
            }
            else if (model.commandType.ToLower() == "detach_vitalsign_from_notes")
            {
                response = ClinicalNotesHelper.Instance().detach_VitalSign_From_Notes(model.VitalSignId, PatientID, VisitId, NotesID);
            }
            else if (model.commandType.ToLower() == "attach_vitalsign_from_notes")
            {
                response = ClinicalNotesHelper.Instance().attach_VitalSign_From_Notes(model.VitalSignId, PatientID, VisitId, NotesID);
            }

            else if (model.commandType.ToLower() == "fill_linked_appointment_notes")
            {
                response = ClinicalNotesHelper.Instance().getLinkedAppointment_Notes(PatientID, MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "copy_previous_note_patient")
            {
                response = ClinicalNotesHelper.Instance().copyPreviousNote_Patient(PatientID);
            }
            else if (model.commandType.ToLower() == "preview_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().previewClinical_Notes(NotesID);
                }
            }
            else if (model.commandType.ToLower() == "print_clinical_notes")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "PRINT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ClinicalNotesHelper.Instance().printClinical_Notes(NotesID);
                }
            }
            else if (model.commandType.ToLower() == "create_notes_hl7")
            {
                response = ClinicalNotesHelper.Instance().Create_Notes_HL7(PatientID, NotesID, model.SyndromicType, MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType.ToLower() == "reasons_lookup_autocomplete")
            {
                response = ClinicalNotesHelper.Instance().ResonsAutoComplete(model.ShortName);
            }
            else if (model.commandType.ToLower() == "get_amendment_data")
            {
                response = ClinicalNotesHelper.Instance().GetAmendmentData(MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "get_amendment_data_for_report")
            {
                response = ClinicalNotesHelper.Instance().GetAmendmentDataReport(MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "get_modifiednotecount")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().GetModifiedNoteCount(MDVUtility.ToLong(model.UserId));
            }
            else if (model.commandType.ToLower() == "detach_documentsfromnote")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().Detach_DocumentsFromNote(model.PatientDocumentIds, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "detach_document_from_notes")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().DetachPatientDocumentFromNote(MDVUtility.ToLong(model.PatientDocumentId), MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "check_clinical_notes_conponents_data")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().CheckNoteSessionData(MDVUtility.ToLong(model.NotesId), model.IsDirectRollBack);
            }

            else if (model.commandType.ToLower() == "getassociatedattachmentsofnote")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().GetAssociatedAttachmentsOfNote(MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "isnoteattachmentexists")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().NoteAttachmentExists(MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "saveandattachorderreport")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().SaveAndAttachOrderReport(model);//(MDVUtility.ToStr(model.OrderIds), MDVUtility.ToStr(model.OrderType), MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType.ToLower() == "sign_note")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    Int64 NoteId = MDVUtility.ToInt64(model.NotesId);
                    VisitId = MDVUtility.ToInt64(model.VisitId);
                    string VisitDate = MDVUtility.ToStr(model.VisitDate);
                    string FromCCM = MDVUtility.ToStr(model.FromCCM);
                    bool IsFromProgressNote = MDVUtility.ToBool(model.IsFromProgressNote);
                    bool ConfirmSign = MDVUtility.ToBool(model.ConfirmSign);

                    response = ClinicalNotesHelper.Instance().Sign_Note(NoteId, FromCCM, VisitId, VisitDate, ProviderId, IsFromProgressNote, ConfirmSign, model.NoteMissingDataReason);

                }
            }
            else if (model.commandType.ToLower() == "sign_note_multiple")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    string FromCCM = MDVUtility.ToStr(model.FromCCM);
                    bool IsFromProgressNote = MDVUtility.ToBool(model.IsFromProgressNote);
                    response = ClinicalNotesHelper.Instance().Sign_Note_Multiple(model.NotesIds, FromCCM, IsFromProgressNote, model.NoteMissingDataReason);
                }
            }
            else if (model.commandType.ToLower() == "note_ready_to_sign")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    string FromCCM = MDVUtility.ToStr(model.FromCCM);
                    bool IsFromProgressNote = MDVUtility.ToBool(model.IsFromProgressNote);
                    response = ClinicalNotesHelper.Instance().NoteReadytoSign_multiple(model.NotesIds, FromCCM, IsFromProgressNote);
                }
            }
            else if (model.commandType.ToLower() == "sign_note_get")
            {
                response = ClinicalNotesHelper.Instance().Sign_Not_get(model.NotesIds);
            }
            else if (model.commandType.ToLower() == "update_isnonbillable_notes")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().UpdateIsNonBillableInfo(MDVUtility.ToInt64(model.NotesId),model.IsNonBilable);
            }
            else if (model.commandType.ToLower() == "get_is_nonbillable_info")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().GetIsNonBillableInfo(MDVUtility.ToInt64(model.NotesId));
            }
            else if (model.commandType.ToLower() == "savediagnosticresultinpatdocs")
            {
                response = null;
                response = ClinicalNotesHelper.Instance().SaveDiagnosticResultInPatDocs(model);
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

        public string NotesExtraInfo(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ClinicalNotesExtraInfoModel model = ser.Deserialize<ClinicalNotesExtraInfoModel>(MDVUtility.ToStr(AllData["data"]));

            if (model.commandType.ToLower() == "search_notesextrainfo")
            {
                response = ClinicalNotesHelper.Instance().SearchNotesExtraInfo(model);
            }
            else if (model.commandType.ToLower() == "save_notesextrainfo")
            {
                response = ClinicalNotesHelper.Instance().SaveNotesExtraInfo(model);
            }
            else if (model.commandType.ToLower() == "update_notesextrainfo")
            {
                response = ClinicalNotesHelper.Instance().UpdateNotesExtraInfo(model);
            }

            if (model.commandType.ToLower() == "search_notesaccess")
            {
                response = ClinicalNotesHelper.Instance().SearchNotesAccess(model);
            }
            else if (model.commandType.ToLower() == "save_notesaccess")
            {
                response = ClinicalNotesHelper.Instance().SaveNotesAccess(model);
            }
            else if (model.commandType.ToLower() == "update_notesaccess")
            {
                response = ClinicalNotesHelper.Instance().UpdateNotesAccess(model);
            }
            return response;
        }

        [HttpPost]
        public string GetNotesTemplateData(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //ClinicalNotesExtraInfoModel model = ser.Deserialize<ClinicalNotesExtraInfoModel>(MDVUtility.ToStr(AllData["data"]));

            dynamic model = System.Web.Helpers.Json.Decode(MDVUtility.ToStr(AllData["data"]));




            return response;
        }

        public string NotesComponent(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;

            NoteComponentModel model = ser.Deserialize<NoteComponentModel>(MDVUtility.ToStr(AllData["data"]));


            if (model.commandType.ToLower() == "insert_note_component")
            {
                response = ClinicalNotesHelper.Instance().insertNoteComponent(model);
            }
            else if (model.commandType.ToLower() == "update_note_component")
            {
                response = ClinicalNotesHelper.Instance().updateNoteComponent(model);
            }
            else if (model.commandType.ToLower() == "load_note_components")
            {
                response = ClinicalNotesHelper.Instance().loadNoteComponents(model);
            }
            else if (model.commandType.ToLower() == "delete_note_component")
            {
                response = ClinicalNotesHelper.Instance().deleteNoteComponent(model);
            }

            else if (model.commandType.ToLower() == "set_notecomponents_order")
            {
                response = ClinicalNotesHelper.Instance().setNoteComponentsOrder(model.NoteComponentIds);
            }
            else if (model.commandType.ToLower() == "insert_note_components_bulk")
            {
                response = ClinicalNotesHelper.Instance().insertNoteComponentsBulk(model.NoteComponentist,model.IsNoteUpdate);
            }
            return response;
        }

        [HttpPost]
        public string NotesComponentAudit(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            NoteComponentAuditModel model = ser.Deserialize<NoteComponentAuditModel>(MDVUtility.ToStr(AllData["data"]));


            if (model.commandType.ToLower() == "load_note_component_audit")
            {
                response = ClinicalNotesHelper.Instance().loadNoteComponentAudit(model);
            }

            return response;

        }

        [HttpPost]
        public string LoadLegacyNoteAndRenderTemplate(NotesComponentViewModel AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            response = ClinicalNotesHelper.Instance().LoadLegacyNoteAndRenderTemplate(AllData.NotesComponents, AllData.ExcludedImages, AllData.IsSaveDiagnosticResult, AllData.NotesPreviewStyle);
            return response;
        }

        public string NotesTemplateDataSelect(CommonSearch objCommonSearch)
        {
            string response = string.Empty;
            response = ClinicalNotesHelper.Instance().NotesTemplateDataSelect(objCommonSearch);
            return response;
        }
        [HttpPost]
        public string Treatment(JObject AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            TreatmentPlanModel model = ser.Deserialize<TreatmentPlanModel>(MDVUtility.ToStr(AllData["data"]));
            TreatmentHelper Helper_Treatment = new TreatmentHelper();
            if (model.commandType.ToLower() == "save_treatment")
            {
                response = null;
                response = Helper_Treatment.SaveTreatment(model);
            }

            else if (model.commandType.ToLower() == "detach_treatment_from_notes")
            {
                response = null;
                response = Helper_Treatment.detachTreatment(MDVUtility.ToInt64(model.NoteId));
            }
            else if (model.commandType.ToLower() == "search_treatment")
            {
                response = null;
                response = Helper_Treatment.LoadTreatment(MDVUtility.ToInt64(model.NoteId));
            }
            return response;
        }
        public string CreateNotesPDFFixesFiles(NotesComponentDataFixModel AllData)
        {
            string response = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            response = ClinicalNotesHelper.Instance().CreateNotesPDFFixesFiles(AllData.NotesIds);
            return response;
        }

        public string CreateNotesPDFFixesFilesV2()
        {
            var notesList = ClinicalNotesHelper.Instance().loadClinicalSignedNotesForPDFFix();

            foreach (NotesComponentDataFixModel note in notesList)
            {
                string response = string.Empty;
                ClinicalNotesHelper cnh = new ClinicalNotesHelper();
                response = cnh.CreateNotesPDFFixesFiles(note.NotesIds);

                if (response.Contains("true"))
                {
                    cnh.updateClinicalSignedNotesForPDFFix(note.NotesIds, "");
                }
                else
                {
                    cnh.updateClinicalSignedNotesForPDFFix(note.NotesIds, "failed");
                }
            }

            if (notesList.Count > 0)
            {
                var response = new
                {
                    status = true,
                    Message = notesList.Count + " Notes processed."
                };
                return (JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = "No record found."
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
    }
}