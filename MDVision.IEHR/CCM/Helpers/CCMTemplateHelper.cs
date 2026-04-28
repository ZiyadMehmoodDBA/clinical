using MDVision.Datasets;
using MDVision.Business.BCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Model;
using MDVision.Model.Lookups;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class CCMTemplateHelper
    {
        private BLLCCM BLLCCMObj = null;
        public CCMTemplateHelper()
        {
            BLLCCMObj = new BLLCCM();
        }
        public static CCMTemplateHelper _instance = null;
        public static CCMTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new CCMTemplateHelper();
            return _instance;
        }
        public string SaveCCMTemplate(dynamic SearchedfieldsJSON, Int64 TemplateId, int TotalSections, int TotalQuestions, int TotalSubQuestions)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                // var SearchedfieldsJSON = ser.Deserialize<dynamic>(MDVUtility.ToStr(fieldsJSON));

                DSCCM dsCCM = null;
                DSCCM dsSection = new DSCCM();
                DSCCM dsQuestions = new DSCCM();
                DSCCM dsSubQuestions = new DSCCM();
                //DSCCM dsTemplateSections = new DSCCM();
                //DSCCM dsSectionQuestions = null;
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
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["TempLookupId"])))
                        drTemplate.TempLookupId = MDVUtility.ToInt16(SearchedfieldsJSON["TempLookupId"]);
                    else
                        drTemplate[dsCCM.Templates.TempLookupIdColumn] = DBNull.Value;

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
                for (int index = 1; index <= TotalSections; index++)
                {
                    DSCCM.SectionsRow drSections = dsSection.Sections.NewSectionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ShortName" + index]))
                        drSections.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["ShortName" + index]);
                    else
                        drSections[dsSection.Sections.ShortNameColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["SectionId" + index]))
                        drSections.SectionIdForRef = MDVUtility.ToStr(SearchedfieldsJSON["SectionId" + index]);

                    dsSection.Sections.AddSectionsRow(drSections);
                }
                for (int index = 1; index <= TotalQuestions; index++)
                {
                    DSCCM.QuestionsRow dr = dsQuestions.Questions.NewQuestionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionDescription" + index]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["QuestionDescription" + index]);
                    else
                        dr[dsQuestions.Questions.DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionHTML" + index]))
                        dr.QuestionHTML = MDVUtility.ToStr(SearchedfieldsJSON["QuestionHTML" + index]);
                    else
                        dr[dsQuestions.Questions.QuestionHTMLColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestSectionId" + index]))
                        dr.SectionIdForRef = MDVUtility.ToStr(SearchedfieldsJSON["QuestSectionId" + index]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestIdForRef" + index]))
                        dr.QuestionIdForRef = MDVUtility.ToInt64(SearchedfieldsJSON["QuestIdForRef" + index]);
                    dsQuestions.Questions.AddQuestionsRow(dr);
                }
                for (int index = 1; index <= TotalSubQuestions; index++)
                {
                    DSCCM.SubQuestionsRow dr = dsSubQuestions.SubQuestions.NewSubQuestionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["SubQuestionDescription" + index]))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["SubQuestionDescription" + index]);
                    else
                        dr[dsSubQuestions.SubQuestions.DescriptionColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["SubQuestionHTML" + index]))
                        dr.QuestionHTML = MDVUtility.ToStr(SearchedfieldsJSON["SubQuestionHTML" + index]);
                    else
                        dr[dsSubQuestions.SubQuestions.QuestionHTMLColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["SubQuestIdForRef" + index]))
                        dr.QuestionIdForRef = MDVUtility.ToInt64(SearchedfieldsJSON["SubQuestIdForRef" + index]);

                    dsSubQuestions.SubQuestions.AddSubQuestionsRow(dr);
                }
                #region Database Insertion
                BLObject<DSCCM> obj = null;
                if (TemplateId > 0)
                {
                    obj = BLLCCMObj.UpdateTemplate(dsCCM);
                }
                else
                {
                    obj = BLLCCMObj.InsertTemplate(dsCCM, dsSection, dsQuestions, dsSubQuestions);
                }
                if (obj.Data != null)
                {
                    long SectionId = 0;
                    TemplateId = MDVUtility.ToInt64(dsCCM.Tables[dsCCM.Templates.TableName].Rows[0][dsCCM.Templates.TemplateIdColumn.ColumnName]);
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
        public string UpdateCCMTemplate(dynamic SearchedfieldsJSON, Int64 TemplateId, int TotalSections, int TotalQuestions, int TotalSubQuestions)
        {
            try
            {
                DSCCM dsCCM = null;
                BLObject<DSCCM> objCCM = null;
                objCCM = BLLCCMObj.FillTemplate(TemplateId);
                dsCCM = objCCM.Data;
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
                #region Database Insertion
                BLObject<DSCCM> obj = null;
                obj = BLLCCMObj.UpdateTemplate(dsCCM);

                if (obj.Data != null)
                {
                    long SectionId = 0;
                    TemplateId = MDVUtility.ToInt64(dsCCM.Tables[dsCCM.Templates.TableName].Rows[0][dsCCM.Templates.TemplateIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
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
        public string FillCCMTemplate(long TemplateId)
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
                            Template_JSON = dsCCM.Tables[dsCCM.Templates.TableName],
                            Sections_JSON = dsCCM.Tables[dsCCM.Sections.TableName],
                            Questions_JSON = dsCCM.Tables[dsCCM.SectionQuestions.TableName],
                            SubQuestions_JSON = dsCCM.Tables[dsCCM.SubQuestions.TableName],
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
        public string DeleteCCMTemplate(string TemplateId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<string> obj = BLLCCMObj.DeleteTemplate(TemplateId);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
                        TemplateId = TemplateId
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string LookupICDGroup()
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
        public string SaveCCMSection(dynamic SearchedfieldsJSON, Int64 TemplateId, Int64 SectionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                DSCCM dsCCM = null;
                BLObject<DSCCM> objCCM = null;
                objCCM = BLLCCMObj.LoadSection(SectionId);
                dsCCM = objCCM.Data;
                string displayMessage = "";
                if (dsCCM.Tables[dsCCM.Sections.TableName].Rows.Count < 1)
                {
                    DSCCM.SectionsRow dr = dsCCM.Sections.NewSectionsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ShortName"]))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["ShortName"]);
                    else
                        dr[dsCCM.Sections.ShortNameColumn] = DBNull.Value;
                    dsCCM.Sections.AddSectionsRow(dr);
                    displayMessage = Common.AppPrivileges.Save_Message;
                }
                else // Update Sections
                {
                    foreach (DSCCM.SectionsRow dr in dsCCM.Tables[dsCCM.Sections.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ShortName"]))
                            dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["ShortName"]);
                        else
                            dr[dsCCM.Sections.ShortNameColumn] = DBNull.Value;
                        displayMessage = Common.AppPrivileges.Update_Message;
                    }
                }
                BLObject<DSCCM> obj = null;
                #region Database Insertion
                if (SectionId > 0)
                    obj = BLLCCMObj.UpdateSection(dsCCM);
                else
                    obj = BLLCCMObj.InsertSection(dsCCM, TemplateId);
                if (obj.Data != null)
                {
                    SectionId = MDVUtility.ToInt64(dsCCM.Tables[dsCCM.Sections.TableName].Rows[0][dsCCM.Sections.SectionIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        Message = displayMessage,
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
        public string DeleteCCMSection(string SectionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<string> obj = BLLCCMObj.DeleteSection(SectionId);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
                        SectionId = SectionId
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string SaveCCMQuestion(dynamic SearchedfieldsJSON, Int64 SectionId, Int64 QuestionId)
        {
            try
            {
                string displayMessage = "";
                DSCCM dsCCM = null;
                BLObject<DSCCM> objCCM = null;
                objCCM = BLLCCMObj.LoadQuestion(QuestionId);
                dsCCM = objCCM.Data;
                if (dsCCM.Tables[dsCCM.Questions.TableName].Rows.Count < 1)
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

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ParentQuestId"]))
                        dr.ParentQuestId = MDVUtility.ToInt64(SearchedfieldsJSON["ParentQuestId"]);
                    dsCCM.Questions.AddQuestionsRow(dr);
                    displayMessage = Common.AppPrivileges.Save_Message;
                }
                else // Update Questions
                {
                    foreach (DSCCM.QuestionsRow dr in dsCCM.Tables[dsCCM.Questions.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionDescription"]))
                            dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["QuestionDescription"]);
                        else
                            dr[dsCCM.Questions.DescriptionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionHTML"]))
                            dr.QuestionHTML = MDVUtility.ToStr(SearchedfieldsJSON["QuestionHTML"]);
                        else
                            dr[dsCCM.Questions.QuestionHTMLColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ParentQuestId"]))
                            dr.ParentQuestId = MDVUtility.ToInt64(SearchedfieldsJSON["ParentQuestId"]);
                        displayMessage = Common.AppPrivileges.Update_Message;
                    }
                }
                BLObject<DSCCM> obj = null;
                #region Database Insertion
                if (QuestionId > 0)
                    obj = BLLCCMObj.UpdateQuestion(dsCCM);
                else
                    obj = BLLCCMObj.InsertQuestion(dsCCM, SectionId);
                if (obj.Data != null)
                {
                    QuestionId = MDVUtility.ToInt64(dsCCM.Tables[dsCCM.Questions.TableName].Rows[0][dsCCM.Questions.QuestionIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        QuestionId = QuestionId
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
        public string DeleteCCMQuestion(string QuestionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<string> obj = BLLCCMObj.DeleteQuestion(QuestionId);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
                        QuestionId = QuestionId
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string LoadCCMSectionQuestions(string SectionId)
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
                    if (dsCCM.Tables[dsCCM.SectionQuestions.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            Questions_JSON = dsCCM.Tables[dsCCM.SectionQuestions.TableName],
                            QuestionsCount = dsCCM.Tables[dsCCM.SectionQuestions.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            QuestionsCount = 0,
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
                        QuestionsCount = 0,
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
        public string LoadCCMSubQuestions(string QuestionId)
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
                    if (dsCCM.Tables[dsCCM.Questions.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SubQuestions_JSON = dsCCM.Tables[dsCCM.Questions.TableName],
                            SubQuestionsCount = dsCCM.Tables[dsCCM.Questions.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            SubQuestionsCount = 0,
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
                        SubQuestionsCount = 0,
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

        public string ActiveInActiveTemplate(string templateId, long isActive)
        {
            try
            {
                BLObject<string> obj = BLLCCMObj.ActiveInActiveTemplate(templateId, isActive);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
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
        public string UpdateCCMQuestionOrder(string QuestionIds)
        {
            try
            {
                BLObject<string> obj = BLLCCMObj.UpdateCCMQuestionOrder(QuestionIds);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
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
    }
}