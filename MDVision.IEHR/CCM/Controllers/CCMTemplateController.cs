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
using MDVision.Model.CCM;

namespace MDVision.IEHR.EMR.Services
{
    public class CCMTemplateController : ApiController
    {

        [HttpPost]
        public string CCMTemplate(JObject AllData)
        {
            string response = null;
            CCMTemplateHelper ccmTemplateHelper = new CCMTemplateHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            var JSONdata = ser.Deserialize<dynamic>(MDVUtility.ToStr(AllData["data"]));
            if (JSONdata["commandType"].ToLower() == "fill_ccm_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    long TemplateId = MDVUtility.ToInt64(JSONdata["TemplateId"]);
                    response = ccmTemplateHelper.FillCCMTemplate(TemplateId);
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
            else if (JSONdata["commandType"].ToLower() == "update_ccm_template")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    long TemplateId = MDVUtility.ToInt64(JSONdata["TemplateId"]);
                    int TotalSections = MDVUtility.ToInt32(JSONdata["TotalSections"]);
                    int TotalQuestions = MDVUtility.ToInt32(JSONdata["TotalQuestions"]);
                    int TotalSubQuestions = MDVUtility.ToInt32(JSONdata["TotalSubQuestions"]);
                    response = ccmTemplateHelper.UpdateCCMTemplate(JSONdata, TemplateId, TotalSections, TotalQuestions, TotalSubQuestions);
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
            else if (JSONdata["commandType"].ToLower() == "lookup_icdgroup")
            {
                {
                    response = ccmTemplateHelper.LookupICDGroup();
                }

            }
            else if (JSONdata["commandType"].ToLower() == "save_ccm_template")
            {
                long TemplateId = MDVUtility.ToInt64(JSONdata["TemplateId"]);
                string privilegasMessage = string.Empty;
                if (TemplateId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {

                    int TotalSections = MDVUtility.ToInt32(JSONdata["TotalSections"]);
                    int TotalQuestions = MDVUtility.ToInt32(JSONdata["TotalQuestions"]);
                    int TotalSubQuestions = MDVUtility.ToInt32(JSONdata["TotalSubQuestions"]);

                    response = ccmTemplateHelper.SaveCCMTemplate(JSONdata, TemplateId, TotalSections, TotalQuestions, TotalSubQuestions);
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
            else if (JSONdata["commandType"].ToLower() == "delete_ccm_template")
            {
                string TemplateId = MDVUtility.ToStr(JSONdata["TemplateId"]);
                string privilegasMessage = string.Empty;

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "DELETE")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.DeleteCCMTemplate(TemplateId);
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
            else if (JSONdata["commandType"].ToLower() == "save_ccm_section")
            {
                long TemplateId = MDVUtility.ToInt64(JSONdata["TemplateId"]);
                long SectionId = MDVUtility.ToInt64(JSONdata["SectionId"]);
                string privilegasMessage = string.Empty;
                if (SectionId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.SaveCCMSection(JSONdata, TemplateId, SectionId);
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
            else if (JSONdata["commandType"].ToLower() == "delete_ccm_section")
            {
                string SectionId = MDVUtility.ToStr(JSONdata["SectionId"]);
                string privilegasMessage = string.Empty;

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "DELETE")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.DeleteCCMSection(SectionId);
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
            else if (JSONdata["commandType"].ToLower() == "select_ccm_sect_quests")
            {
                string SectionId = MDVUtility.ToStr(JSONdata["SectionId"]);
                string privilegasMessage = string.Empty;

                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "SEARCH")).ToString();

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.LoadCCMSectionQuestions(SectionId);
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
            else if (JSONdata["commandType"].ToLower() == "save_ccm_question")
            {
                long QuestionId = MDVUtility.ToInt64(JSONdata["QuestionId"]);
                long SectionId = MDVUtility.ToInt64(JSONdata["SectionId"]);
                string privilegasMessage = string.Empty;
                if (QuestionId > 0)
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "EDIT")).ToString();
                }
                else
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "ADD")).ToString();
                }

                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.SaveCCMQuestion(JSONdata, SectionId, QuestionId);
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
            else if (JSONdata["commandType"].ToLower() == "delete_ccm_question")
            {
                string QuestionId = MDVUtility.ToStr(JSONdata["QuestionId"]);
                string privilegasMessage = string.Empty;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.DeleteCCMQuestion(QuestionId);
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
            else if (JSONdata["commandType"].ToLower() == "ccm_template_active_inactive")
            {
                string TemplateId = MDVUtility.ToStr(JSONdata["TemplateId"]);
                long IsActive = MDVUtility.ToInt64(JSONdata["IsActive"]);

                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.ActiveInActiveTemplate(TemplateId, IsActive);
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


            else if (JSONdata["commandType"].ToLower() == "select_ccm_sub_quests")
            {
                string QuestionId = MDVUtility.ToStr(JSONdata["QuestionId"]);
                string privilegasMessage = string.Empty;
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.LoadCCMSubQuestions(QuestionId);
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
            else if (JSONdata["commandType"].ToLower() == "update_ccm_question_order")
            {
                string privilegasMessage = string.Empty;
                string QuestionIds = MDVUtility.ToStr(JSONdata["QuestionIds"]);
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management_Templates ", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ccmTemplateHelper.UpdateCCMQuestionOrder(QuestionIds);
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