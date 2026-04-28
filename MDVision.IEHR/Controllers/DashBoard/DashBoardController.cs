using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Model.Dashboard;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.DashBoard
{

    public class DashBoardController : ApiController
    {

        [HttpPost]
        public string LoadDashBoard(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                DashBoardModel model = ser.Deserialize<DashBoardModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "load_dashboard")
                {
                    //string response = null;
                    var response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadDashBoardSettingsAsync(model);
                    //response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadDashBoardSettings(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response.ToString());
                }

                else if (model.CommandType.ToLower() == "load_messages")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPracticeMessage(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_direct_messages")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearcDirecthMessage(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_message_log")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadMessagesLog(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_app_by_status")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchAppointment(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_patient_appoitments")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPatientAppointment(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "loadportalapprequest")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPortalAppRequest(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "cancelapprequest")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().CancelAppRequest(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "confirmapprequest")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().ConfirmAppRequest(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "confirm_cancel_multiple_apprequest")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().AcceptRejectMultipleAppRequest(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }


                else if (model.CommandType.ToLower() == "load_ccm_enrollment_info")
                {
                    string response = null;
                    response = Controls.DashBoard.DashBoard.Instance().LoadCCMEnrollmentInfo(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_notes_draft_count")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadNotesDraftCount();
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_outgoing_direct_messages")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearcOutgoingDirecthMessage(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "load_demographic_label_data")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().DemographicLabelData(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

                else if (model.CommandType.ToLower() == "load_dashboard_appointment_labelcount")
                {
                    string response = null;
                    response = (string)MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadAppointmentNotesCount(null);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string SearchDashBoard(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                DashBoardModel model = ser.Deserialize<DashBoardModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "search_copay")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchCopay(MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_patient_charges")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPatientChanges(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_payment")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPayments(MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_tcm_patients")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchTCMPatients(Convert.ToInt64(model.PatientId),Convert.ToInt64(model.ProviderId), model.Status,MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_checkin_patients")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchCheckInPatients(model.Status, Convert.ToInt64( model.ProviderId),Convert.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_checkin_patients_request")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchCheckInPatientsRequest(model.Status, Convert.ToInt64(model.ProviderId), Convert.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_patients_signup_request")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().LoadPatientPortalSignupReq(model.Status,model.ProviderId,model.PageNumber,model.RowsPerPage);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_visits_note")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchVisitsNotes(model.VisitFrom, model.VisitTo, model.Status, model.NoteType, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), model.IsDraftNote,model.ProviderId,model.PatientId);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                //load visit encounter data for bulk sign
                else if (model.CommandType.ToLower() == "search_visits_note_bulksign")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchVisitsNotesBulkSign(model.VisitFrom, model.VisitTo, model.Status, model.NoteType, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), model.IsReadyOrMissing, model.ProviderId, model.PatientId, model.MissingInfo);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_modified_note")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchModifiedNotes(model.VisitFrom, model.VisitTo, MDVUtility.ToInt(model.Status),MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                
                else if (model.CommandType.ToLower() == "search_payment")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPayments(MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_documents")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchDashBoard(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_tasks")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchUserTask(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "search_activeaccounts")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().SearchPatientPortalAccounts(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }
            
            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string UpdateDashBoard(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                DashBoardModel model = ser.Deserialize<DashBoardModel>(MDVUtility.ToStr(objData["data"]));

                if (model.CommandType.ToLower() == "update_dashboard_kpi")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().UpdateKPIActiveInactive(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_dashboard_kpi_on_drop")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().UpdateKPI(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_notes_status")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().updateClinical_Notes(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);

                }
                else if (model.CommandType.ToLower() == "update_notes_cosign")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().updateClinicalNotesForCosign(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_notes_amendment")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().updateClinicalNotesForAmendment(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "modified_note_reviewed")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().ModifiedNoteReviewed(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "update_outgoing_direct_messages")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().UpdateOutgoingDirectMessageStatus(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                else if (model.CommandType.ToLower() == "patients_signup_delete_request")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().patientsSignupDeleteRequest(model.PatientId);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

                else if (model.CommandType.ToLower() == "patients_signup_approve_request")
                {
                    string response = null;
                    response = MDVision.IEHR.Controls.DashBoard.DashBoard.Instance().patientsSignupApproveRequest(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
