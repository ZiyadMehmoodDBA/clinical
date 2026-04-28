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
using MDVision.Model.CCM;
using MDVision.Model.Lookups;
using MDVision.IEHR.Common;
namespace MDVision.IEHR.Controls.Admin.CCM
{
    public class Admin_CCMTemplateDetails
    {

        private BLLCCM BLLCCMObj = null;
        private BLLAdminHL7 BLLAdminHL7Obj = null;
        public Admin_CCMTemplateDetails()
        {
            BLLCCMObj = new BLLCCM();
        }

        #region Singleton
        private static Admin_CCMTemplateDetails _obj = null;
        public static Admin_CCMTemplateDetails Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMTemplateDetails();
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
                case "FILL_CCM_TEMPLATE":
                    {
                        string fieldsJSON = context.Request["CCMTemplateData"];
                        long tempID = MDVUtility.ToInt64(context.Request["TemplateId"]);
                        string strJSONData = FillCCMTemplate(tempID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_TEMPLATE":
                    {
                        string fieldsJSON = context.Request["CCMTemplateData"];
                        long TemplateId = MDVUtility.ToInt64(context.Request["TemplateId"]);
                        int TotalSections = MDVUtility.ToInt32(context.Request["TotalSections"]);
                        int TotalQuestions = MDVUtility.ToInt32(context.Request["TotalQuestions"]);
                        int TotalSubQuestions = MDVUtility.ToInt32(context.Request["TotalSubQuestions"]);
                        string strJSONData = SaveCCMTemplate(fieldsJSON, TemplateId, TotalSections, TotalQuestions, TotalSubQuestions);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_TEMPLATE":
                    {
                        string fieldsJSON = context.Request["CCMTemplateData"];
                        string tempID = MDVUtility.ToStr(context.Request["TemplateId"]);
                        string strJSONData = "";//FillCCMTemplate(tempID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOOKUP_ICDGROUP":
                    {
                        string strJSONData = LookupICDGroup();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

        #region Private Functions
        private string SaveCCMTemplate(string fieldsJSON, Int64 TemplateId, int TotalSections, int TotalQuestions, int TotalSubQuestions)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(MDVUtility.ToStr(fieldsJSON));

                DSCCM dsCCM = null;
                BLObject<DSCCM> objCCM = null;
                objCCM = BLLCCMObj.FillTemplate(TemplateId);
                dsCCM = objCCM.Data;
                if (dsCCM.Tables[dsCCM.Templates.TableName].Rows.Count < 1)
                {
                    DSCCM.TemplatesRow drTemplate = dsCCM.Templates.NewTemplatesRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["TemplateTitle"]))
                        drTemplate.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["TemplateTitle"]);
                    else
                        drTemplate[dsCCM.Templates.ShortNameColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["Description"]))
                        drTemplate.Description = MDVUtility.ToStr(SearchedfieldsJSON["Description"]);
                    else
                        drTemplate[dsCCM.Templates.DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ICDgroupIds"]))
                        drTemplate.ICDGroupIds = MDVUtility.ToStr(SearchedfieldsJSON["ICDgroupIds"]);
                    else
                        drTemplate[dsCCM.Templates.ICDGroupIdsColumn] = DBNull.Value;
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["IsActive"]))
                        drTemplate.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;
                    //else
                      //  drTemplate[dsCCM.Templates.IsActiveColumn] = DBNull.Value;

                    drTemplate.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplate.CreatedOn = DateTime.Now;
                    drTemplate.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplate.ModifiedOn = DateTime.Now;
                    dsCCM.Templates.AddTemplatesRow(drTemplate);
                }
                else // Update Templates
                {
                    foreach (DSCCM.TemplatesRow drTemplate in dsCCM.Tables[dsCCM.Templates.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["TemplateTitle"]))
                            drTemplate.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["TemplateTitle"]);
                        else
                            drTemplate[dsCCM.Templates.ShortNameColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["Description"]))
                            drTemplate.Description = MDVUtility.ToStr(SearchedfieldsJSON["Description"]);
                        else
                            drTemplate[dsCCM.Templates.DescriptionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ICDgroupIds"]))
                            drTemplate.ICDGroupIds = MDVUtility.ToStr(SearchedfieldsJSON["ICDgroupIds"]);
                        else
                            drTemplate[dsCCM.Templates.ICDGroupIdsColumn] = DBNull.Value;
                       // if (!string.IsNullOrEmpty(SearchedfieldsJSON["IsActive"]))
                            drTemplate.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;
                       // else
                         //   drTemplate[dsCCM.Templates.IsActiveColumn] = DBNull.Value;
                        drTemplate.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drTemplate.ModifiedOn = DateTime.Now;
                    }
                }
                for (int index = 0; index <= TotalSections; index++)
                {
                    DSCCM.SectionsRow drSections = dsCCM.Sections.NewSectionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ShortName"]))
                        drSections.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["ShortName"]);
                    else
                        drSections[dsCCM.Templates.ShortNameColumn] = DBNull.Value;

                    dsCCM.Sections.AddSectionsRow(drSections);
                }
                for (int index = 0; index <= TotalQuestions; index++)
                {
                    DSCCM.QuestionsRow dr = dsCCM.Questions.NewQuestionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionDescription"]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["QuestionDescription"]);
                    else
                        dr[dsCCM.Questions.DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionHTML"]))
                        dr.QuestionHTML = MDVUtility.ToStr(SearchedfieldsJSON["QuestionHTML"]);
                    else
                        dr[dsCCM.Questions.QuestionHTMLColumn] = DBNull.Value;
                    dsCCM.Questions.AddQuestionsRow(dr);
                }
                for (int index = 0; index <= TotalQuestions; index++)
                {
                    DSCCM.QuestionsRow dr = dsCCM.Questions.NewQuestionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionDescription"]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["QuestionDescription"]);
                    else
                        dr[dsCCM.Questions.DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionHTML"]))
                        dr.QuestionHTML = MDVUtility.ToStr(SearchedfieldsJSON["QuestionHTML"]);
                    else
                        dr[dsCCM.Questions.QuestionHTMLColumn] = DBNull.Value;
                    dsCCM.Questions.AddQuestionsRow(dr);
                }
                #region Database Insertion

                BLObject<DSCCM> obj =null ;//BLLCCMObj.InsertUpdateTemplates(dsCCM);
                if (obj.Data != null)
                {
                    long SectionId = 0;
                    TemplateId = MDVUtility.ToInt64(dsCCM.Tables[dsCCM.Templates.TableName].Rows[0][dsCCM.Templates.TemplateIdColumn.ColumnName]);
                     if (TemplateId > 0)
                     {
                         for (int index = 0; index <= TotalSections; index++)
                         {
                             DSCCM.SectionsRow drSections = dsCCM.Sections.NewSectionsRow();
                             if (!string.IsNullOrEmpty(SearchedfieldsJSON["ShortName"]))
                                 drSections.ShortName = MDVUtility.ToInt64(SearchedfieldsJSON["ShortName"]);
                             else
                                 drSections[dsCCM.Templates.ShortNameColumn] = DBNull.Value;

                             dsCCM.Sections.AddSectionsRow(drSections);
                         }
                        DSCCM.QuestionsRow drQuestions = BuildQuestionRow(fieldsJSON, dsCCM, TotalQuestions);
                    }
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        TemplateId = TemplateId,
                        SectionId = SectionId
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion

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

        private string FillCCMTemplate(long TemplateId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                DSCCM dsCCM = null;
                BLObject<DSCCM> obj = null;
                obj = BLLCCMObj.FillTemplate(TemplateId);
                if (obj.Data != null)
                {
                    dsCCM = obj.Data;
                    if (dsCCM.Tables[dsCCM.Templates.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            Template_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.Templates.TableName]),
                            Sections_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.Sections.TableName]),
                            Questions_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.Questions.TableName]),
                            SubQuestions_JSON = MDVUtility.JSON_DataTable(dsCCM.Tables[dsCCM.SubQuestions.TableName]),
                            TemplateCount = dsCCM.Tables[dsCCM.Templates.TableName].Rows.Count,
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

        private string LookupICDGroup()
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {

                BLObject<List<ICDGroupLookupModel>> obj = BLLCCMObj.LookupICDGroup();
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        icdGroupList_JSON = ser.Serialize(obj.Data),
                        icdGroupCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        icdGroupList_JSON = "[]",
                        icdGroupCount = 0,
                        iTotalDisplayRecords = 0
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
        private DSCCM.QuestionsRow BuildQuestionRow(string fieldsJSON, DSCCM dsCCM, int TotalQuestions)
        {
            DSCCM.QuestionsRow dr = dsCCM.Questions.NewQuestionsRow();
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            #region QuestionRows
            for (int index = 0; index <= TotalQuestions; index++)
            {
            }
            #endregion
            return dr;
        }

        #endregion
    }
}