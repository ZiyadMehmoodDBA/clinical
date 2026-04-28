using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.Model.Clinical.HPI;
using MDVision.Model.Clinical.Templates;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class HPIController : ApiController
    {
        [HttpPost]
        public string HPI(JObject AllData)
        {
            string response = null;
            HPIHelper hpiHelper = new HPIHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            HPITemplateModel templatemodel = ser.Deserialize<HPITemplateModel>(MDVUtility.ToStr(AllData["data"]));

            if (templatemodel.commandType.ToLower() == "load_hpi_templates")
            {
                response = hpiHelper.loadHPITemplates(templatemodel);
            }
            else if (templatemodel.commandType.ToLower() == "save_hpi_template")
            {
                response = hpiHelper.insertUpdateHPITemplate(templatemodel);
            }

            else if (templatemodel.commandType.ToLower() == "load_hpitemplate_fill")
            {
                response = hpiHelper.FillHPITemplate(templatemodel);
            }
            else if (templatemodel.commandType.ToLower() == "load_hpi_symptoms")
            {
                response = hpiHelper.LookupSymptoms(1);
            }
            else if (templatemodel.commandType.ToLower() == "load_hpi_symptom_findings")
            {
                response = null;
                HPITemplateSymptomsModel modelSym = ser.Deserialize<HPITemplateSymptomsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.LoadHPISymptomsFindings(MDVUtility.ToLong(modelSym.HPITemplateId), MDVUtility.ToLong(modelSym.HPISymptomId));
            }
            else if (templatemodel.commandType.ToLower() == "load_hpi_symptom_findings_detail")
            {
                response = null;
                HPITemplateSymptomFindingsModel modelSym = ser.Deserialize<HPITemplateSymptomFindingsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.LoadHPISymptomsFindingsDetail(MDVUtility.ToLong(modelSym.HPISymptomsDetailId), MDVUtility.ToLong(modelSym.HPITemplateSymptomId));
            }
            else if (templatemodel.commandType.ToLower() == "load_hpitemplate_forprovider")
            {
                response = null;
                response = hpiHelper.loadHPIForProvider(MDVUtility.ToInt64(templatemodel.ProviderId));
            }
            else if (templatemodel.commandType.ToLower() == "fill_hpi_for_notes")
            {
                response = null;
                HPITemplateSymptomsModel modeltemSym = ser.Deserialize<HPITemplateSymptomsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.loadHPITempSymptomFindingsForNotes(MDVUtility.ToLong(modeltemSym.NotesId));
            }
            else if (templatemodel.commandType.ToLower() == "removetemplate_symptom")
            {
                response = null;
                HPITemplateSymptomsModel modeltemSys = ser.Deserialize<HPITemplateSymptomsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.deleteHPITemplateSymptom(modeltemSys.HPITemplateSymptomId);
            }
            else if (templatemodel.commandType.ToLower() == "delete_hpi_symptom_finding")
            {
                response = null;
                HPITemplateSymptomFindingsModel modeltemSys = ser.Deserialize<HPITemplateSymptomFindingsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.deleteHPISymptomFinding(modeltemSys.HPITemplateSympFindingId);
            }
            else if (templatemodel.commandType.ToLower() == "load_hpi_symptom_findings_note")
            {
                response = null;
                HPINotesFindings modeltemSys = ser.Deserialize<HPINotesFindings>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.loadHPITempSymptomFindingNote(MDVUtility.ToLong(modeltemSys.HPITemplateId), MDVUtility.ToLong(modeltemSys.HPISymptomId), MDVUtility.ToLong(modeltemSys.NotesId));
            }
            else if (templatemodel.commandType.ToLower() == "load_hpi_template_symptoms")
            {
                response = null;
                response = hpiHelper.loadHPITemplateSymptoms(MDVUtility.ToLong(templatemodel.HPITemplateId), MDVUtility.ToLong(templatemodel.EntityId), 1, null);
            }
            else if (templatemodel.commandType.ToLower() == "save_hpi_notesfinding")
            {
                response = null;
                response = hpiHelper.saveHPIComplaintNotesFinding(templatemodel.NotesFindingsData, templatemodel.NotesId);
            }
            else if (templatemodel.commandType.ToLower() == "is_hpi_complaint")
            {
                response = null;
                HPINotesFindings modeltemSys = ser.Deserialize<HPINotesFindings>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.isHPIComplaint(MDVUtility.ToLong(modeltemSys.NotesId));
            }
            else if (templatemodel.commandType == "DELETE_HPITemplate")
            {
                response = null;
                response = hpiHelper.deleteHPITemplate(MDVUtility.ToLong(templatemodel.HPITemplateId));

            }
            else if (templatemodel.commandType.ToLower() == "update_hpitemplate_active_inactive")
            {
                response = null;
                response = hpiHelper.HPITemplateIsActive(MDVUtility.ToLong(templatemodel.HPITemplateId), MDVUtility.ToLong(templatemodel.IsActive));
            }
            else if (templatemodel.commandType == "Insert_Template_Symptom_Assosication")
            {
                response = null;
                HPITemplateSymptomsModel modeltem = ser.Deserialize<HPITemplateSymptomsModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.associateHPISymptomAndTemplate(modeltem);
            }
            else if (templatemodel.commandType == "Insert_Findings_Symptom_Assosication")
            {
                response = null;
                HPISymptomFindingModel modeltem = ser.Deserialize<HPISymptomFindingModel>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.associateHPIFindingAndSymptom(modeltem);
            }
            else if (templatemodel.commandType == "Update_HPI_Selected_Findings_Desc_For_Notes")
            {
                response = null;
                HPINotesFindings modelSym = ser.Deserialize<HPINotesFindings>(MDVUtility.ToStr(AllData["data"]));
                response = hpiHelper.UpdateHPINotesSymptomFindingDesc(MDVUtility.ToLong(modelSym.HPINotesFindingsId), MDVUtility.ToStr(modelSym.Desc));
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
        public string HPIFindings(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            HPIFindingsModel model = ser.Deserialize<HPIFindingsModel>(MDVUtility.ToStr(AllData["data"]));
            HPIHelper hpiHelper = new HPIHelper();

            if (model.commandType.ToLower() == "load_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "SEARCH")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.LoadHPIFindings(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "insert_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "ADD")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.SaveHPIFindings(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "update_hpi_findings" || model.commandType.ToLower() == "active_inactive_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "EDIT")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.UpdateHPIFindings(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
            }
            else if (model.commandType.ToLower() == "fill_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "SEARCH")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.FillHPIFindings(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }

            else if (model.commandType.ToLower() == "delete_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "DELETE")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.DeleteHPIFindings(model.HPIFindingsId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "delete_hpi_symptom_finding")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "DELETE")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.DeleteHPISymptomFindings(model.HPISymptomFindingsId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "lookup_hpi_findings")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Findings", "SEARCH")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.LookupHPIFindings(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
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

        public string HPISymptoms(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            HPISymptomsModel model = ser.Deserialize<HPISymptomsModel>(MDVUtility.ToStr(AllData["data"]));
            HPIHelper hpiHelper = new HPIHelper();

            if (model.commandType.ToLower() == "load_hpi_symptoms")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Symptoms", "SEARCH")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.LoadHPISymptoms(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "fill_hpi_symptoms")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Symptoms", "SEARCH")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.FillHPISymptoms(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "delete_hpi_symptoms")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Symptoms", "DELETE")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.DeleteHPISymptoms(model.HPISymptomsId);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "insert_hpi_symptoms")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Symptoms", "ADD")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.SaveHPISymptoms(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "update_hpi_symptoms" || model.commandType.ToLower() == "active_inactive_hpi_symptoms")
            {
                //string privilegeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("HPI Symptoms", "EDIT")).ToString();
                string privilegeMessage = "";
                if (string.IsNullOrEmpty(privilegeMessage))
                {
                    response = hpiHelper.UpdateHPISymptoms(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegeMessage
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