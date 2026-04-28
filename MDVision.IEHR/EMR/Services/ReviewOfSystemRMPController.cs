using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.Templates;
using MDVision.Model.Clinical.ReviewOfSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class ReviewOfSystemRMPController : ApiController
    {
        public string ROSCharatristics(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSCharacteristics model = ser.Deserialize<ROSCharacteristics>(MDVUtility.ToStr(AllData["data"]));

            ReviewOfSystemHelper helperReviewOfSystem = new ReviewOfSystemHelper();

            if (model.commandType.ToLower() == "load_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.loadROSCharatrisctic(model);
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

            else if (model.commandType.ToLower() == "insert_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.InsertROSCharatrisctic(model);
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
            else if (model.commandType.ToLower() == "delete_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.deleteROSCharatristics(model.ROSCharacteristicsId);
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
            else if (model.commandType.ToLower() == "active_inactive_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.updateROSCharatristics(model);
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
            else if (model.commandType.ToLower() == "fill_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.fillROSCharatristics(model);
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
            else if (model.commandType.ToLower() == "update_reviewofsystem_charatristics")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSCharatristics ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewOfSystem.updateROSCharatristics(model);
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
            else if (model.commandType.ToLower() == "lookup_reviewofsystem_charatristics")
            {
                    response = helperReviewOfSystem.lookupROSCharatristics(model);
               

            }
            else if (model.commandType.ToLower() == "insert_reviewofsystem_charatristics_updatesystems")
            {
                
                    response = helperReviewOfSystem.InsertROSCharatriscticandUpadatesystem(model);
          

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
        public string ROSSystem(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSCharacteristics model = ser.Deserialize<ROSCharacteristics>(MDVUtility.ToStr(AllData["data"]));
            ReviewOfSystemHelper helperReviewofSystem = new ReviewOfSystemHelper();

            if (model.commandType.ToLower() == "load_reviewofsystems_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.loadROSSystem(model);
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
            else if (model.commandType.ToLower() == "fill_reviewofsystems_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.fill_reviewofsystems_system(model);
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
            else if (model.commandType.ToLower() == "active_inactive_reviewofsystem_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.ActiveInActive_ROSSystem(model.ROSSystemId, model.IsActive);
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
            else if (model.commandType.ToLower() == "delete_reviewofsystem_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.delete_reviewofsystem_system(model.ROSSystemId);
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
            else if (model.commandType.ToLower() == "insert_reviewofsystem_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.insert_reviewofsystem_system(model);
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
            else if (model.commandType.ToLower() == "update_reviewofsystem_system")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Clinical Questionnaire_ROSSystems ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.update_reviewofsystem_system(model);
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
            else if (model.commandType.ToLower() == "load_ros_systems_lookup")
            {
                    response = helperReviewofSystem.lookupROSSystems(model);
              

            }
            else if (model.commandType.ToLower() == "load_reviewofsystem_system_charatristics")
            {
                
                response = helperReviewofSystem.load_reviewofsystem_system_charatristics(model);
               

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


        // ROS Revamp Methods
        public string ROSRevampTemplate(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            PhysicalExamTemplateModel model = ser.Deserialize<PhysicalExamTemplateModel>(MDVUtility.ToStr(AllData["data"]));
            ReviewOfSystemHelper helperReviewofSystem = new ReviewOfSystemHelper();
            // PhysicalExamHelper helperPhysicalExam1 = new PhysicalExamHelper();
            if (model.commandType == "Load_ROSRevamp_Template")
            {
                response = null;
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId));
                //response = helperPhysicalExam.loadPhysicalExamTemplateNew(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), MDVUtility.ToInt32(model.IsActive));
                response = helperReviewofSystem.loadROSRevampTemplates(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), MDVUtility.ToInt32(model.IsActive));


            }

            return response;
        }

        public string ReviewofSystemRevampTemplate(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ROSTemplateModel model = ser.Deserialize<ROSTemplateModel>(MDVUtility.ToStr(AllData["data"]));
            ReviewOfSystemHelper helperReviewofSystem = new ReviewOfSystemHelper();

            if (model.commandType.ToLower() == "insert_reviewofsystem_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System Revamp ", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.insert_reviewofsystem_template(model);
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
            else if (model.commandType.ToLower() == "delete_reviewofsystem_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System Revamp ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.delete_reviewofsystem_template(model.TemplateId);
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
            else if (model.commandType.ToLower() == "delete_reviewofsystem_template_note")
            {
               
                    response = helperReviewofSystem.delete_reviewofsystem_template_note(model.TemplateId,model.NoteId);
             
            }
            else if (model.commandType.ToLower() == "update_reviewofsystem_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System Revamp ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.update_reviewofsystem_template(model);
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
            else if (model.commandType.ToLower() == "select_reviewofsystem_template")
            {
               
                    response = helperReviewofSystem.select_reviewofsystem_template(model.TemplateId, model.NoteId);
              

            }
            else if (model.commandType.ToLower() == "select_reviewofsystem_template_note")
            {
                
                    response = helperReviewofSystem.select_reviewofsystem_template_note(model.TemplateId, model.NoteId, model.ROSSystemId);
                

            }
            else if (model.commandType.ToLower() == "activeinactive_reviewofsystem_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Template_Review of System Revamp ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = helperReviewofSystem.activeinactive_reviewofsystem_template(model.TemplateId, model.IsActive);
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
            else if (model.commandType.ToLower() == "load_reviewofsystem_lookups")
            {
                    response = helperReviewofSystem.loadROSLookUps();
            }
            else if (model.commandType.ToLower() == "load_reviewofsystem_patientinfo")
            {
                response = helperReviewofSystem.load_ROSRevampTemplates_Note(model);

            }
           else if (model.commandType.ToLower() == "insert_reviewofsystem_template_note")
            {
                response = helperReviewofSystem.Insert_reviewofsystem_Template_Note(model);
            }
            else if (model.commandType.ToLower() == "toggle_characteristics_note")
            {
                response = helperReviewofSystem.toggle_Characteristics_note(model.TemplateId,model.ROSSystemId,model.ROSCharacteristicsId,model.NoteId, model.IsPositive);
             

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
