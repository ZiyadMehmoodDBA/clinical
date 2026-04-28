using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Text.RegularExpressions;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Clinical
{
    public class Clinical_Question
    {
         private BLLClinical BLLClinicalObj = null;
         public Clinical_Question()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton

        private static Clinical_Question _instance = null;
        public static Clinical_Question Instance()
        {
            if (_instance == null)
                _instance = new Clinical_Question();
            return _instance;
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        /// <summary>
        /// Searches the Clinical Question.
        /// </summary>
        /// <returns></returns>
        private string SearchQuestion(string fieldsJson, Int64 questionId,Int64 rpp, Int64 PageNumber)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj;

                var QuestionId = MDVUtility.ToInt64(searchedfieldsJson["hfQuestionId"]);
                if (QuestionId <= 0)
                {
                    searchedfieldsJson["hfQuestionId"] = 0;
                }
                var shortName = searchedfieldsJson["txtshortName"] == "" ? null : searchedfieldsJson["txtshortName"];
                var questionName = searchedfieldsJson["txtQuestiontName"] == "" ? null : searchedfieldsJson["txtQuestiontName"];
                var activeStatus = searchedfieldsJson["chkIsActice"] == "" ? null : searchedfieldsJson["chkIsActice"];
                //searchedfieldsJson["lstTypeId"]
                var questionType = MDVUtility.ToInt64(searchedfieldsJson["lstTypeId"]) <= 0 ? null : "1";

                obj = BLLClinicalObj.LoadClinicalQuestion(QuestionId, shortName, questionName, activeStatus, searchedfieldsJson["lstTypeId"], rpp, PageNumber);

                //BLObject<DSTemplateBuilder> obj;
                //if (searchedfieldsJson == null || sectionId != 0)
                //    obj = BLLClinicalObj.LoadClinicalSection(sectionId, null, null, null, null, null, null, rpp, pageNo);
                //else
                //{
                //    obj = BLLClinicalObj.LoadClinicalSection(sectionId, searchedfieldsJson["txtSectionShortName"], searchedfieldsJson["txtSectionDescription"], searchedfieldsJson["txtSectionTitle"], searchedfieldsJson.ContainsKey("ddlsectionType") ? string.IsNullOrEmpty(searchedfieldsJson["ddlsectionType"]) ? null : MDVUtility.ToStr(searchedfieldsJson["ddlsectionType"]) : null, searchedfieldsJson.ContainsKey("Specialitydd") ? string.IsNullOrEmpty(searchedfieldsJson["Specialitydd"]) ? null : MDVUtility.ToStr(searchedfieldsJson["Specialitydd"]) : null, searchedfieldsJson["ddlActive"], rpp, pageNo);
                //}

                dsQuestion = obj.Data;
                if (obj.Data != null)
                {
                    if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            QuestionCount = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            iTotalDisplayRecords = dsQuestion.ClinicalQuestions.Rows[0][dsQuestion.ClinicalQuestions.RecordCountColumn],
                            QuestionLoad_JSON = MDVUtility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            iTotalDisplayRecords=0,
                            Message = "No Question Found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Inserts the Clinical Question.
        /// </summary>
        /// <returns></returns>SaveQuestion
        private string SaveQuestion(string fieldsJson, string files)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSTemplateBuilder dsQuestion = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalQuestionsRow dr = dsQuestion.ClinicalQuestions.NewClinicalQuestionsRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfQuestionId"]))
                    dr.QuestionId = MDVUtility.ToInt64(searchedfieldsJson["hfQuestionId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["questionType"]))
                    dr.QuestionTypeId = MDVUtility.ToInt64(searchedfieldsJson["questionType"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                    dr.ShortName = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);

                dr.IsNewLine = MDVUtility.ToStr(searchedfieldsJson["chkNewLine"]) == "True";

                if (!string.IsNullOrEmpty(searchedfieldsJson["questionName"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["questionName"]);

                Int64 questionTypeId = dr.QuestionTypeId;

                #region Text Field
                if (questionTypeId == 1)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["TextLength"]))
                        dr.TextLength = MDVUtility.ToStr(searchedfieldsJson["TextLength"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["TextCase"]))
                        dr.TextCase = MDVUtility.ToStr(searchedfieldsJson["TextCase"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["sentencetxtField"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["sentencetxtField"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["sentencetxtField"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }

                    dr.FieldType = MDVUtility.ToStr(searchedfieldsJson["radSingleLine"]) == "True";

                    dr.IsAutoComplete = MDVUtility.ToStr(searchedfieldsJson["chkAutoComplete"]) == "True";

                }
                #endregion

                #region Radio Field
                else if (questionTypeId == 2)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionRadioLabel1"]))
                        dr.BoolTrueDisplay = MDVUtility.ToStr(searchedfieldsJson["questionRadioLabel1"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionRadioLabel2"]))
                        dr.BoolFalseDisplay = MDVUtility.ToStr(searchedfieldsJson["questionRadioLabel2"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["radDefaultValue"]))
                        dr.DefaultValue = MDVUtility.ToStr(searchedfieldsJson["radDefaultValue"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel1"]))
                        dr.SentenceTrue = MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel1"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel2"]))
                        dr.SentenceFalse = MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel2"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel1"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel1"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTagTrue = sentenceTags;
                    }
                    if (!string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel2"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel2"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTagFalse = sentenceTags;
                    }
                }
                #endregion

                #region DropDown Field
                else if (questionTypeId == 3)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["txtareaElementsDropDown"]))
                        dr.DisplayText = MDVUtility.ToStr(Regex.Replace(searchedfieldsJson["txtareaElementsDropDown"], "\n", ","));

                    if (!string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["Sentencedrpdwn"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["Sentencedrpdwn"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Fraction Field
                else if (questionTypeId == 5)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionFarctionLabel1"]))
                        dr.FractionFieldLabel1 = MDVUtility.ToStr(searchedfieldsJson["questionFarctionLabel1"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionFarctionLabel2"]))
                        dr.FractionFieldLabel2 = MDVUtility.ToStr(searchedfieldsJson["questionFarctionLabel2"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceFraction"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["SentenceFraction"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceFraction"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceFraction"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Number Field
                else if (questionTypeId == 10)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionNumberDefaulttxt"]))
                        dr.DefaultValue = MDVUtility.ToStr(searchedfieldsJson["questionNumberDefaulttxt"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionNumberMinLengthtxt"]))
                        dr.MinValue = MDVUtility.ToStr(searchedfieldsJson["questionNumberMinLengthtxt"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionNumberMaxLengthtxt"]))
                        dr.MaxValue = MDVUtility.ToStr(searchedfieldsJson["questionNumberMaxLengthtxt"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionNumberPrecisiontxt"]))
                        dr.NumberPrecesion = MDVUtility.ToStr(searchedfieldsJson["questionNumberPrecisiontxt"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceNumber"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["SentenceNumber"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceNumber"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceNumber"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Date Field
                else if (questionTypeId == 6)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionDateFormat_text"]))
                        dr.DateTimeFormat = MDVUtility.ToStr(searchedfieldsJson["questionDateFormat_text"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionDateDefaultValue_text"]))
                        dr.DefaultValue = MDVUtility.ToStr(searchedfieldsJson["questionDateDefaultValue_text"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceDate"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["SentenceDate"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceDate"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceDate"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Time Field
                else if (questionTypeId == 7)
                {
                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionTimeFormat"]))
                        dr.DateTimeFormat = MDVUtility.ToStr(searchedfieldsJson["questionTimeFormat"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["questionTimeDefaultValue"]))
                        dr.DefaultValue = MDVUtility.ToStr(searchedfieldsJson["questionTimeDefaultValue"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceTime"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["SentenceTime"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceTime"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceTime"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Image Field
                else if (questionTypeId == 9)
                {
                    string strBase64 = files.Split(',')[1];
                    strBase64 = strBase64.Replace(' ', '+');
                    byte[] currentFileStream = Convert.FromBase64String(strBase64);

                    dr.ImageStream = currentFileStream.Length > 0 ? currentFileStream : null;

                    string FileName = files.Split(',')[0].Split(':')[1].Split(';')[0];
                    dr.FileType = FileName;

                    if (!string.IsNullOrEmpty(searchedfieldsJson["uploadFilePH"]))
                        dr.FilePath = MDVUtility.ToStr(searchedfieldsJson["uploadFilePH"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceImage"]))
                        dr.Sentence = MDVUtility.ToStr(searchedfieldsJson["SentenceImage"]);

                    if (!string.IsNullOrEmpty(searchedfieldsJson["SentenceImage"]))
                    {
                        var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceImage"]));
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsQuestion.ClinicalQuestions.AddClinicalQuestionsRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertQuestions(dsQuestion);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        //FIXME
                        message = Common.AppPrivileges.Save_Message,
                        MessageId = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows[0][dsQuestion.ClinicalQuestions.QuestionIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes the Clinical Question.
        /// </summary>
        /// <param name="questionId">The Question identifier.</param>
        /// <returns></returns>
        private string DeleteQuestion(string questionId)
        {
            try
            {
                if (string.IsNullOrEmpty(questionId))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteQuestion(questionId);
                    if (obj.Data == "")
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Updates the Question is active.
        /// </summary>
        /// <param name="questionId">The Question identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns></returns>
        private string UpdateQuestionIsActive(Int64 questionId, Int64 isActive)
        {
            try
            {
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, null, null, null,1,1);
                dsQuestion = obj.Data;
                if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows[0];
                    dr[dsQuestion.ClinicalQuestions.IsActiveColumn.ColumnName] = isActive;

                    BLObject<DSTemplateBuilder> objQuestion = BLLClinicalObj.UpdateQuestions(dsQuestion);
                    if (objQuestion.Data != null)
                    {
                        var successMsg = isActive == 0 ? Common.AppPrivileges.Inactive_Message : Common.AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            message = successMsg
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objQuestion.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Updates the Question.
        /// </summary>
        /// <returns></returns>
        private string UpdateQuestion(string fieldsJson, Int64 questionId, string files)
        {
            try
            {
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, null, null, null,1,1);
                dsQuestion = obj.Data;
                if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("ShortName") && !string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                            dr[dsQuestion.ClinicalQuestions.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);
                        if (searchedfieldsJson.ContainsKey("questionType") && !string.IsNullOrEmpty(searchedfieldsJson["questionType"]))
                            dr[dsQuestion.ClinicalQuestions.QuestionTypeIdColumn] = MDVUtility.ToStr(searchedfieldsJson["questionType"]);
                        if (searchedfieldsJson.ContainsKey("questionName") && !string.IsNullOrEmpty(searchedfieldsJson["questionName"]))
                            dr[dsQuestion.ClinicalQuestions.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["questionName"]);
                        if (searchedfieldsJson.ContainsKey("chkNewLine"))
                            dr[dsQuestion.ClinicalQuestions.IsNewLineColumn] = MDVUtility.ToStr(searchedfieldsJson["chkNewLine"]) == "True" ? true : false;

                        Int64 QuestionType = MDVUtility.ToInt64(searchedfieldsJson["questionType"]);

                        #region Text Field
                        if (QuestionType == 1)
                        {
                            if (searchedfieldsJson.ContainsKey("TextLength") && !string.IsNullOrEmpty(searchedfieldsJson["TextLength"]))
                                dr[dsQuestion.ClinicalQuestions.TextLengthColumn] = MDVUtility.ToStr(searchedfieldsJson["TextLength"]);

                            if (searchedfieldsJson.ContainsKey("TextCase") && !string.IsNullOrEmpty(searchedfieldsJson["TextCase"]))
                                dr[dsQuestion.ClinicalQuestions.TextCaseColumn] = MDVUtility.ToStr(searchedfieldsJson["TextCase"]);

                            if (searchedfieldsJson.ContainsKey("chkAutoComplete"))
                                dr[dsQuestion.ClinicalQuestions.IsAutoCompleteColumn] = MDVUtility.ToStr(searchedfieldsJson["chkAutoComplete"]) == "True";

                            if (searchedfieldsJson.ContainsKey("sentencetxtField") && !string.IsNullOrEmpty(searchedfieldsJson["sentencetxtField"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["sentencetxtField"]);

                            if (searchedfieldsJson.ContainsKey("Sentencedrpdwn") && !string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                            {
                                var getTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["sentencetxtField"]));
                                string sentenceTags = string.Join(",", getTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = sentenceTags;
                            }
                            if (searchedfieldsJson.ContainsKey("radSingleLine"))
                                dr[dsQuestion.ClinicalQuestions.FieldTypeColumn] = MDVUtility.ToStr(searchedfieldsJson["radSingleLine"]) == "True";
                        }
                        #endregion

                        #region Radio Field
                        else if (QuestionType == 2)
                        {

                            if (searchedfieldsJson.ContainsKey("questionRadioLabel1") && !string.IsNullOrEmpty(searchedfieldsJson["questionRadioLabel1"]))
                                dr[dsQuestion.ClinicalQuestions.BoolTrueDisplayColumn] = MDVUtility.ToStr(searchedfieldsJson["questionRadioLabel1"]);

                            if (searchedfieldsJson.ContainsKey("questionRadioLabel2") && !string.IsNullOrEmpty(searchedfieldsJson["questionRadioLabel2"]))
                                dr[dsQuestion.ClinicalQuestions.BoolFalseDisplayColumn] = MDVUtility.ToStr(searchedfieldsJson["questionRadioLabel2"]);

                            if (searchedfieldsJson.ContainsKey("radDefaultValue") && !string.IsNullOrEmpty(searchedfieldsJson["radDefaultValue"]))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = MDVUtility.ToStr(searchedfieldsJson["radDefaultValue"]);

                            if (searchedfieldsJson.ContainsKey("radSentenceForLabel1") && !string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel1"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceTrueColumn] = MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel1"]);

                            if (searchedfieldsJson.ContainsKey("radSentenceForLabel2") && !string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel2"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceFalseColumn] = MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel2"]);

                            if (searchedfieldsJson.ContainsKey("radSentenceForLabel1") && !string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel1"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel1"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagTrueColumn] = SentenceTags;
                            }

                            if (searchedfieldsJson.ContainsKey("radSentenceForLabel2") && !string.IsNullOrEmpty(searchedfieldsJson["radSentenceForLabel2"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["radSentenceForLabel2"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagFalseColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region DropDown Field
                        else if (QuestionType == 3)
                        {
                            if (searchedfieldsJson.ContainsKey("txtareaElementsDropDown") && !string.IsNullOrEmpty(searchedfieldsJson["txtareaElementsDropDown"]))
                                dr[dsQuestion.ClinicalQuestions.DisplayTextColumn] = MDVUtility.ToStr(searchedfieldsJson["txtareaElementsDropDown"]);

                            if (searchedfieldsJson.ContainsKey("Sentencedrpdwn") && !string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["Sentencedrpdwn"]);

                            if (searchedfieldsJson.ContainsKey("Sentencedrpdwn") && !string.IsNullOrEmpty(searchedfieldsJson["Sentencedrpdwn"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["Sentencedrpdwn"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Fraction Field
                        else if (QuestionType == 5)
                        {
                            if (searchedfieldsJson.ContainsKey("questionFarctionLabel1") && !string.IsNullOrEmpty(searchedfieldsJson["questionFarctionLabel1"]))
                                dr[dsQuestion.ClinicalQuestions.FractionFieldLabel1Column] = MDVUtility.ToStr(searchedfieldsJson["questionFarctionLabel1"]);

                            if (searchedfieldsJson.ContainsKey("questionFarctionLabel2") && !string.IsNullOrEmpty(searchedfieldsJson["questionFarctionLabel2"]))
                                dr[dsQuestion.ClinicalQuestions.FractionFieldLabel2Column] = MDVUtility.ToStr(searchedfieldsJson["questionFarctionLabel2"]);

                            if (searchedfieldsJson.ContainsKey("SentenceFraction") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceFraction"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["SentenceFraction"]);

                            if (searchedfieldsJson.ContainsKey("SentenceFraction") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceFraction"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceFraction"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Number Field
                        else if (QuestionType == 10)
                        {
                            if (searchedfieldsJson.ContainsKey("questionNumberDefaulttxt") && !string.IsNullOrEmpty(searchedfieldsJson["questionNumberDefaulttxt"]))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = MDVUtility.ToStr(searchedfieldsJson["questionNumberDefaulttxt"]);

                            if (searchedfieldsJson.ContainsKey("questionNumberMinLengthtxt") && !string.IsNullOrEmpty(searchedfieldsJson["questionNumberMinLengthtxt"]))
                                dr[dsQuestion.ClinicalQuestions.MinValueColumn] = MDVUtility.ToStr(searchedfieldsJson["questionNumberMinLengthtxt"]);

                            if (searchedfieldsJson.ContainsKey("questionNumberMaxLengthtxt") && !string.IsNullOrEmpty(searchedfieldsJson["questionNumberMaxLengthtxt"]))
                                dr[dsQuestion.ClinicalQuestions.MaxValueColumn] = MDVUtility.ToStr(searchedfieldsJson["questionNumberMaxLengthtxt"]);

                            if (searchedfieldsJson.ContainsKey("questionNumberPrecisiontxt") && !string.IsNullOrEmpty(searchedfieldsJson["questionNumberPrecisiontxt"]))
                                dr[dsQuestion.ClinicalQuestions.NumberPrecesionColumn] = MDVUtility.ToStr(searchedfieldsJson["questionNumberPrecisiontxt"]);

                            if (searchedfieldsJson.ContainsKey("SentenceNumber") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceNumber"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["SentenceNumber"]);

                            if (searchedfieldsJson.ContainsKey("SentenceNumber") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceNumber"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceNumber"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Date Field
                        else if (QuestionType == 6)
                        {
                            if (searchedfieldsJson.ContainsKey("questionDateFormat_text") && !string.IsNullOrEmpty(searchedfieldsJson["questionDateFormat_text"]))
                                dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn] = MDVUtility.ToStr(searchedfieldsJson["questionDateFormat_text"]);

                            if (searchedfieldsJson.ContainsKey("questionDateDefaultValue_text") && !string.IsNullOrEmpty(searchedfieldsJson["questionDateDefaultValue_text"]))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = MDVUtility.ToStr(searchedfieldsJson["questionDateDefaultValue_text"]);

                            if (searchedfieldsJson.ContainsKey("SentenceDate") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceDate"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["SentenceDate"]);

                            if (searchedfieldsJson.ContainsKey("SentenceDate") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceDate"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceDate"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Time Field
                        else if (QuestionType == 7)
                        {
                            if (searchedfieldsJson.ContainsKey("questionTimeFormat") && !string.IsNullOrEmpty(searchedfieldsJson["questionTimeFormat"]))
                                dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn] = MDVUtility.ToStr(searchedfieldsJson["questionTimeFormat"]);

                            if (searchedfieldsJson.ContainsKey("questionTimeDefaultValue") && !string.IsNullOrEmpty(searchedfieldsJson["questionTimeDefaultValue"]))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = MDVUtility.ToStr(searchedfieldsJson["questionTimeDefaultValue"]);

                            if (searchedfieldsJson.ContainsKey("SentenceTime") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceTime"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["SentenceTime"]);

                            if (searchedfieldsJson.ContainsKey("SentenceTime") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceTime"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceTime"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Image Field
                        else if (QuestionType == 9)
                        {
                            string strBase64 = files.Split(',')[1];
                            strBase64 = strBase64.Replace(' ', '+');
                            byte[] currentFileStream = Convert.FromBase64String(strBase64);
                            string FileName = files.Split(',')[0].Split(':')[1].Split(';')[0];

                            dr[dsQuestion.ClinicalQuestions.ImageStreamColumn] = currentFileStream.Length > 0 ? currentFileStream : null;

                            dr[dsQuestion.ClinicalQuestions.FileTypeColumn] = FileName;

                            if (searchedfieldsJson.ContainsKey("uploadFilePH") && !string.IsNullOrEmpty(searchedfieldsJson["uploadFilePH"]))
                                dr[dsQuestion.ClinicalQuestions.FilePathColumn] = MDVUtility.ToStr(searchedfieldsJson["uploadFilePH"]);

                            if (searchedfieldsJson.ContainsKey("SentenceImage") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceImage"]))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = MDVUtility.ToStr(searchedfieldsJson["SentenceImage"]);

                            if (searchedfieldsJson.ContainsKey("SentenceImage") && !string.IsNullOrEmpty(searchedfieldsJson["SentenceImage"]))
                            {
                                var GetTags = GetTokens(MDVUtility.ToStr(searchedfieldsJson["SentenceImage"]));
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion


                        dr[dsQuestion.ClinicalQuestions.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsQuestion.ClinicalQuestions.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSTemplateBuilder> objQuestion = null;

                    objQuestion = BLLClinicalObj.UpdateQuestions(dsQuestion);

                    if (objQuestion.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objQuestion.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fills the Question.
        /// </summary>
        /// <param name="questionId">The Question identifier.</param>
        /// <returns></returns>
        private string FillQuestion(Int64 questionId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(questionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSTemplateBuilder dsQuestion = null;
                    BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, null, null, null,1,1);
                    dsQuestion = obj.Data;
                    if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows[0];

                        string CommasToNewLine = MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DisplayTextColumn.ColumnName]).Replace(",", Environment.NewLine);

                        var keyValues = new Dictionary<string, string>
                        {
                            { "ShortName", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.ShortNameColumn.ColumnName])},
                            { "questionType", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.QuestionTypeIdColumn.ColumnName])},
                            { "chkNewLine", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.IsNewLineColumn.ColumnName])},
                            { "questionName", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DescriptionColumn.ColumnName])},

                            //Text Fields
                            { "radSingleLine", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.FieldTypeColumn.ColumnName])},
                            { "TextLength", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.TextLengthColumn.ColumnName])},
                            { "TextCase", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.TextCaseColumn.ColumnName])},
                            { "chkAutoComplete", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.IsAutoCompleteColumn.ColumnName])},
                            { "txtPredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentencetxtField", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            //Radio Fields
                            { "questionRadioLabel1", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.BoolTrueDisplayColumn.ColumnName])},
                            { "questionRadioLabel2", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.BoolFalseDisplayColumn.ColumnName])},
                            { "radDefaultValue", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            { "radPredefinedTrue", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "radPredefinedFalse", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "radSentenceForLabel1", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTrueColumn.ColumnName])},
                            { "radSentenceForLabel2", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceFalseColumn.ColumnName])},

                            //Drop Down Fields
                            { "txtareaElementsDropDown", CommasToNewLine},
                            { "drpPredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "Sentencedrpdwn", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Fraction Field
                            { "questionFarctionLabel1", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.FractionFieldLabel1Column.ColumnName])},
                            { "questionFarctionLabel2", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.FractionFieldLabel2Column.ColumnName])},
                            { "fractionPredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "SentenceFraction", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Number Field
                            { "questionNumberDefaulttxt", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            { "questionNumberMinLengthtxt", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.MinValueColumn.ColumnName])},
                            { "questionNumberMaxLengthtxt", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.MaxValueColumn.ColumnName])},
                            { "questionNumberPrecisiontxt", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.NumberPrecesionColumn.ColumnName])},
                            { "numberPredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "SentenceNumber", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Date Field
                            { "questionDateFormat_text", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn.ColumnName])},
                            { "questionDateDefaultValue_text", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            { "datePredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "SentenceDate", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Time Field
                            { "questionTimeFormat", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn.ColumnName])},
                            { "questionTimeDefaultValue", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            { "timePredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "SentenceTime", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Image Field 
                            
                            { "totalFiles", "1 file(s) selected"},
                            { "uploadFilePH", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.FilePathColumn.ColumnName])},
                            { "imgPredefined", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "SentenceImage", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        string imageBase64 = "";
                        byte[] imageByteArr = dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName] as byte[];
                        if (imageByteArr != null)
                        {
                            imageBase64 = "data:" + dr[dsQuestion.ClinicalQuestions.FileTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName] as byte[]);
                        }
                        keyValues.Add("imdIDPreview", imageBase64);
                        var response = new
                        {
                            status = true,
                            QuestionLoad_JSON = js.Serialize(keyValues)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Get Tags out of sentence Field.
        /// </summary>
        /// <returns></returns>
        private List<String> GetTokens(String str)
        {
            Regex regex = new Regex(@"(?<=\{{)[^}]*(?=\}})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);
            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
        }

        #endregion

        #region Public Functions

        #endregion

        #region Control Events
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_QUESTION":
                    {
                        //string fieldsJson = context.Request["QuestionData"];
                        //Int64 questionId = MDVUtility.ToInt64(context.Request["hfQuestionId"]);
                        //Int64 rpp, PageNumber;
                        //string strJsonData = SearchQuestion(fieldsJson, questionId,0, 0);
                        string fieldsJson = context.Request["QuestionData"];
                        // Int64 questionId = MDVUtility.ToInt64(context.Request["hfQuestionId"]);

                        Int64 questionId = MDVUtility.ToInt64(context.Request["questionID"]);

                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = SearchQuestion(fieldsJson, questionId, rpp, pageNo);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "SAVE_QUESTION":
                    {
                        string fieldsJson = context.Request["QuestionData"];
                        string file = MDVUtility.ToStr(context.Request["file"]);
                        string strJsonData = SaveQuestion(fieldsJson, file);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "UPDATE_QUESTION":
                    {
                        string fieldsJson = context.Request["QuestionData"];
                        Int64 questionId = MDVUtility.ToInt64(context.Request["QuestionID"]);
                        string file = MDVUtility.ToStr(context.Request["file"]);
                        string strJsonData = UpdateQuestion(fieldsJson, questionId, file);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "DELETE_QUESTION":
                    {
                        String questionId = context.Request["questionID"];
                        string strJsonData = DeleteQuestion(questionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "UPDATE_QUESTION_ACTIVE_INACTIVE":
                    {
                        Int64 questionId = MDVUtility.ToInt64(context.Request["QuestionID"]);
                        Int64 isActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJsonData = UpdateQuestionIsActive(questionId, isActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "FILL_QUESTION":
                    {
                        string strQuestionId = context.Request["QuestionId"];
                        string strJsonData = FillQuestion(MDVUtility.ToInt64(strQuestionId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }

        #endregion

    }
}