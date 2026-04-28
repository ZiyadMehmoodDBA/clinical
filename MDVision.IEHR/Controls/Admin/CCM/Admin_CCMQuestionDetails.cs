using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MDVision.IEHR.Common;
namespace MDVision.IEHR.Controls.Admin.CCM
{
    public class Admin_CCMQuestionDetails
    {

        private BLLCCM BLLCCMObj = null;
        private BLLAdminHL7 BLLAdminHL7Obj = null;
        public Admin_CCMQuestionDetails()
        {
            BLLCCMObj = new BLLCCM();
        }

        #region Singleton
        private static Admin_CCMQuestionDetails _obj = null;
        public static Admin_CCMQuestionDetails Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMQuestionDetails();
            return _obj;
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// CommandHandler
        /// </summary>
        /// <param name="context"></param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "DELETE_QUESTION":
                    {
                        string QuestionId = MDVUtility.ToStr(context.Request["QuestionId"]);
                        string strJSONData = DeleteQuestion(QuestionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_SUB_QUESTIONS":
                    {
                        string QuestionId = MDVUtility.ToStr(context.Request["QuestionId"]);
                        string strJSONData = LoadSubQuestions(QuestionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_SECTION_QUESTIONS":
                    {
                        string SectionId = MDVUtility.ToStr(context.Request["SectionId"]);
                        string strJSONData = LoadSectionQuestions(SectionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        private string LoadSectionQuestions(string SectionId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                DSCCM dsCCM = null;
                BLObject<DSCCM> obj = null;
                obj = BLLCCMObj.LoadSectionQuestions(SectionId);
                if (obj.Data != null)
                {
                    dsCCM = obj.Data;
                    if (dsCCM.Tables[dsCCM.Templates.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SectionQuestions_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.SectionQuestions.TableName]),
                            SectionQuestionsCount = dsCCM.Tables[dsCCM.SectionQuestions.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            templateCount = 0,
                            Message = AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        templateCount = 0,
                        Message = AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string LoadSubQuestions(string QuestionId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                DSCCM dsCCM = null;
                BLObject<DSCCM> obj = null;
                obj = BLLCCMObj.LoadSubQuestion(QuestionId);
                if (obj.Data != null)
                {
                    dsCCM = obj.Data;
                    if (dsCCM.Tables[dsCCM.Templates.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SubQuestions_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.SubQuestions.TableName]),
                            SubQuestionsCount = dsCCM.Tables[dsCCM.SubQuestions.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            templateCount = 0,
                            Message = AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        templateCount = 0,
                        Message = AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeleteQuestion(string QuestionId)
        {
            try
            {
                if (string.IsNullOrEmpty(QuestionId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLCCMObj.DeleteQuestion(QuestionId);
                    if (obj.Data != null && obj.Data == "")
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

    }
}