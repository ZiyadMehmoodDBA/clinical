/* Author: ZeeshanAK
 * OverView: Created to handel Custom Form
 * Date : September 20, 2016
 */

using MDVision.Business.BCommon;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Model;
using MDVision.IEHR.EMR.Helpers.Patient;

namespace MDVision.IEHR.EMR.Services
{
    public class CustomFormController : ApiController
    {

        [HttpPost]
        public string CustomForm(JObject AllData)
        {
            string response = null;
            CustomFormHelper customFormsHelper = new CustomFormHelper();
            PatientCustomFormHelper patientCustomFormsHelper = new PatientCustomFormHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            CustomFormModel model = ser.Deserialize<CustomFormModel>(MDVUtility.ToStr(AllData["data"]));
            PatientCustomFormModel patCustomModel = ser.Deserialize<PatientCustomFormModel>(MDVUtility.ToStr(AllData["data"]));

            if (model.commandType.ToLower() == "load_custom_forms")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.loadCustomForm(model);
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
            else if (model.commandType.ToLower() == "fill_custom_form")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.fillCustomForm(model);
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
            else if (model.commandType.ToLower() == "update_custom_forms")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.updateCustomForm(model);
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
            else if (model.commandType.ToLower() == "lookup_category")
            {
                {
                    response = customFormsHelper.LookupMedicationsReprot();
                }

            }
            else if (model.commandType.ToLower() == "search_ros_systems_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    //response = customFormsHelper.loadROSTemplates(model.IsActive, MDVUtility.ToInt64(model.CustomFormsId), MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage), MDVUtility.ToInt64(model.EntityId));
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
            else if (model.commandType.ToLower() == "update_custom_form_active_inactive")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.activeInactiveCustomForm(model.CustomFormId, model.IsActive);
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
            else if (model.commandType.ToLower() == "delete_custom_forms")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.deleteCustomForm(model.CustomFormId);
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
            else if (model.commandType.ToLower() == "save_custom_forms")
            {
                Int64 CustomFormsId = MDVUtility.ToInt64(model.CustomFormId);
                string privilegasMessage = string.Empty;
                if (CustomFormsId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = customFormsHelper.insertCustomForm(model);
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
            else if (model.commandType.ToLower() == "attach_custom_form_with_notes")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.attachCustomFormsWithNotes(model.CustomFormId, model.NoteId);
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
            else if (model.commandType.ToLower() == "detach_custom_form_from_notes")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.detachCustomFormFromNotes(model.CustomFormId, model.NoteId, model.CustomFormDocName);
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
            else if (model.commandType.ToLower() == "save_patient_custom_form")
            {
                string privilegasMessage = string.Empty;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.insertCustomForm(patCustomModel);
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
            else if (model.commandType.ToLower() == "get_provider_cpts")
            {
                response = patientCustomFormsHelper.GetProviderCPTs(MDVUtility.ToInt64(patCustomModel.CustomFormId), MDVUtility.ToInt64(patCustomModel.ProviderId));
            }
            else if (model.commandType.ToLower() == "get_patient_document_id")
            {
                response = patientCustomFormsHelper.GetPatientDocumentId(patCustomModel.CustomFormId);
            }
            else if (model.commandType.ToLower() == "get_report_header_footer")
            {
                response = patientCustomFormsHelper.getReportHeaderFooter(patCustomModel);
            }
            else if (model.commandType.ToLower() == "update_patient_custom_form")
            {
                string privilegasMessage = string.Empty;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.updatePatientCustomForm(patCustomModel);
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
            else if (model.commandType.ToLower() == "search_patient_customforms")
            {
                string privilegasMessage = string.Empty;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.loadPatientCustomForm(patCustomModel);
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
            else if (model.commandType.ToLower() == "delete_patient_custom_forms")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.deletePatientCustomForm(patCustomModel.CustomFormId);
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

            else if (model.commandType.ToLower() == "patient_custom_form_active_inactive")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.activeInactivePatientCustomForm(patCustomModel.PatientCustomFormId, patCustomModel.IsActive);
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
            else if (model.commandType.ToLower() == "fill_patient_custom_form")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CustomForm", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = patientCustomFormsHelper.fillPatientCustomForm(patCustomModel);
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
            else if (model.commandType.ToLower() == "delete_detach_problems")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.DeleteProblemList(model.ProblemListIds, model.NoteId);
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
            else if (model.commandType.ToLower() == "delete_detach_procedures")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.DeleteProcedures(model.ProcedureIds, model.NoteId);
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
            else if (model.commandType.ToLower() == "load_favoritelist_customforms")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.loadFavoriteListCustomForm(model);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }

        [HttpPost]
        public string GlobalQuestion(JObject AllData)
        {
            string response = null;
            CustomFormHelper customFormsHelper = new CustomFormHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            GlobalQuestionModel model = ser.Deserialize<GlobalQuestionModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "load_global_question")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.loadGlobalQuestion(model);
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
            else if (model.commandType.ToLower() == "delete_global_question")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.deleteGlobalQuestion(model.QuestionId);
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
            else if (model.commandType.ToLower() == "save_global_question")
            {
                Int64 QuestionId = MDVUtility.ToInt64(model.QuestionId);
                string privilegasMessage = string.Empty;
                if (QuestionId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = customFormsHelper.insertGlobalQuestion(model);
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

            else if (model.commandType.ToLower() == "update_global_question")
            {
                Int64 QuestionId = MDVUtility.ToInt64(model.QuestionId);
                string privilegasMessage = string.Empty;
                if (QuestionId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = customFormsHelper.updateGlobalQuestion(model);
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

            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        [HttpPost]
        public string GlobalQuestionGroup(JObject AllData)
        {
            string response = null;
            CustomFormHelper customFormsHelper = new CustomFormHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            GlobalQuestionGroupModel model = ser.Deserialize<GlobalQuestionGroupModel>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "load_global_question_group")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.loadGlobalQuestionGroup(model);
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
            else if (model.commandType.ToLower() == "load_global_question_group_savedglobally")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.loadGlobalQuestionGroupSavedGlobally(model);
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
            else if (model.commandType.ToLower() == "delete_global_question_group")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.deleteGlobalQuestionGroup(model.QuestionGroupId);
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
            else if (model.commandType.ToLower() == "save_global_question_group")
            {
                Int64 QuestionGroupId = MDVUtility.ToInt64(model.QuestionGroupId);
                string privilegasMessage = string.Empty;
                if (QuestionGroupId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = customFormsHelper.insertGlobalQuestionGroup(model);
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
            else if (model.commandType.ToLower() == "update_global_question_group")
            {
                Int64 QuestionGroupId = MDVUtility.ToInt64(model.QuestionGroupId);
                string privilegasMessage = string.Empty;
                if (QuestionGroupId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    response = customFormsHelper.updateGlobalQuestionGroup(model);
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
            else if (model.commandType.ToLower() == "fill_global_question_group")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Template_Custom Forms ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = customFormsHelper.fillGlobalQuestion(model);
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
            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
    }
}