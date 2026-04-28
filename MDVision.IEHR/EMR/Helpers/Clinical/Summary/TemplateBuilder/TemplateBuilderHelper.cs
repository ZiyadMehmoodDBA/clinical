using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.BusinessWrapper;
using MDVision.IEHR.EMR.Model.Clinical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;


namespace MDVision.IEHR.Controls.Clinical
{
    public class TemplateBuilderHelper
    {
        private static TemplateBuilderHelper _instance = null;
        public static TemplateBuilderHelper Instance()
        {
            if (_instance == null)
                _instance = new TemplateBuilderHelper();
            return _instance;
        }
        public string SearchQuestion(TemplateBuilderModel model, Int64 questionId, Int64 rpp, Int64 PageNumber)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj;

                var QuestionId = 0;// Utility.ToInt64(searchedfieldsJson["hfQuestionId"]);

                var shortName = model.ShortName;
                var questionName = model.questionName;
                var activeStatus = model.isActive;
                
                var questionType = Utility.ToInt64(model.questionType) <= 0 ? null : "1";

                obj = BusinessWrapper.Clinical.BusinessObj.LoadClinicalQuestion(QuestionId, shortName, questionName, activeStatus, model.questionType, rpp, PageNumber);
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
                            QuestionLoad_JSON = Utility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            iTotalDisplayRecords = 0,
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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string SaveQuestion(TemplateBuilderModel model, string files)
        {
            try
            {
                DSTemplateBuilder dsQuestion = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalQuestionsRow dr = dsQuestion.ClinicalQuestions.NewClinicalQuestionsRow();

                if (!string.IsNullOrEmpty(model.hiddenQuestionID))
                    dr.QuestionId = Utility.ToInt64(model.hiddenQuestionID);

                if (!string.IsNullOrEmpty(model.questionType))
                    dr.QuestionTypeId = Utility.ToInt64(model.questionType);

                if (!string.IsNullOrEmpty(model.ShortName))
                    dr.ShortName = model.ShortName;

                dr.IsNewLine = model.NewLine == "True";

                if (!string.IsNullOrEmpty(model.questionName))
                    dr.Description = model.questionName;

                Int64 questionTypeId = dr.QuestionTypeId;

                #region Text Field
                if (questionTypeId == 1)
                {
                    if (!string.IsNullOrEmpty(model.textlength))
                        dr.TextLength = model.textlength;

                    if (!string.IsNullOrEmpty(model.textCase))
                        dr.TextCase = model.textCase;

                    if (!string.IsNullOrEmpty(model.sentence))
                        dr.Sentence = model.sentence;

                    if (!string.IsNullOrEmpty(model.sentenceDropDown))
                    {
                        var getTags = GetTokens(model.sentence);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }

                    dr.FieldType = model.singleLine == "True";

                    dr.IsAutoComplete = model.autoComplete == "True";

                }
                #endregion

                #region Radio Field
                else if (questionTypeId == 2)
                {
                    if (!string.IsNullOrEmpty(model.label1))
                        dr.BoolTrueDisplay = model.label1;

                    if (!string.IsNullOrEmpty(model.label2))
                        dr.BoolFalseDisplay = model.label2;

                    if (!string.IsNullOrEmpty(model.defaultValue))
                        dr.DefaultValue = model.defaultValue;

                    if (!string.IsNullOrEmpty(model.sentenceForLabel1))
                        dr.SentenceTrue = model.sentenceForLabel1;

                    if (!string.IsNullOrEmpty(model.sentenceForLabel2))
                        dr.SentenceFalse = model.sentenceForLabel2;

                    if (!string.IsNullOrEmpty(model.sentenceForLabel1))
                    {
                        var getTags = GetTokens(model.sentenceForLabel1);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTagTrue = sentenceTags;
                    }
                    if (!string.IsNullOrEmpty(model.sentenceForLabel2))
                    {
                        var getTags = GetTokens(model.sentenceForLabel2);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTagFalse = sentenceTags;
                    }
                }
                #endregion

                #region DropDown Field
                else if (questionTypeId == 3)
                {
                    if (!string.IsNullOrEmpty(model.areaElements))
                        dr.DisplayText = Regex.Replace(model.areaElements, "\n", ",");

                    if (!string.IsNullOrEmpty(model.sentenceDropDown))
                        dr.Sentence = Utility.ToStr(model.sentenceDropDown);

                    if (!string.IsNullOrEmpty(model.sentenceDropDown))
                    {
                        var getTags = GetTokens(model.sentenceDropDown);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Fraction Field
                else if (questionTypeId == 5)
                {
                    if (!string.IsNullOrEmpty(model.questionFarctionLabel1))
                        dr.FractionFieldLabel1 = model.questionFarctionLabel1;

                    if (!string.IsNullOrEmpty(model.questionFarctionLabel2))
                        dr.FractionFieldLabel2 = model.questionFarctionLabel2;

                    if (!string.IsNullOrEmpty(model.sentenceFraction))
                        dr.Sentence = model.sentenceFraction;

                    if (!string.IsNullOrEmpty(model.sentenceFraction))
                    {
                        var getTags = GetTokens(model.sentenceFraction);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Number Field
                else if (questionTypeId == 10)
                {
                    if (!string.IsNullOrEmpty(model.questionNumberDefault))
                        dr.DefaultValue = model.questionNumberDefault;

                    if (!string.IsNullOrEmpty(model.questionNumberMinLength))
                        dr.MinValue = model.questionNumberMinLength;

                    if (!string.IsNullOrEmpty(model.questionNumberMaxLength))
                        dr.MaxValue = model.questionNumberMaxLength;

                    if (!string.IsNullOrEmpty(model.questionNumberPrecision))
                        dr.NumberPrecesion = model.questionNumberPrecision;

                    if (!string.IsNullOrEmpty(model.sentenceNumber))
                        dr.Sentence = model.sentenceNumber;

                    if (!string.IsNullOrEmpty(model.sentenceNumber))
                    {
                        var getTags = GetTokens(model.sentenceNumber);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Date Field
                else if (questionTypeId == 6)
                {
                    if (!string.IsNullOrEmpty(model.questionDateformat))
                        dr.DateTimeFormat = model.questionDateformat;

                    if (!string.IsNullOrEmpty(model.questionDateformatDefaultValue))
                        dr.DefaultValue = model.questionDateformatDefaultValue;

                    if (!string.IsNullOrEmpty(model.sentenceDate))
                        dr.Sentence = model.sentenceDate;

                    if (!string.IsNullOrEmpty(model.sentenceDate))
                    {
                        var getTags = GetTokens(model.sentenceDate);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                #region Time Field
                else if (questionTypeId == 7)
                {
                    if (!string.IsNullOrEmpty(model.questionTimeFormat))
                        dr.DateTimeFormat = model.questionTimeFormat;

                    if (!string.IsNullOrEmpty(model.questionTimeDefaultValue))
                        dr.DefaultValue = model.questionTimeDefaultValue;

                    if (!string.IsNullOrEmpty(model.sentenceTime))
                        dr.Sentence = model.sentenceTime;

                    if (!string.IsNullOrEmpty(model.sentenceTime))
                    {
                        var getTags = GetTokens(model.sentenceTime);
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

                    if (!string.IsNullOrEmpty(model.uploadFile))
                        dr.FilePath = model.uploadFile;

                    if (!string.IsNullOrEmpty(model.sentenceImage))
                        dr.Sentence = model.sentenceImage;

                    if (!string.IsNullOrEmpty(model.sentenceImage))
                    {
                        var getTags = GetTokens(model.sentenceImage);
                        string sentenceTags = string.Join(",", getTags.ToArray());
                        dr.SentenceTag = sentenceTags;
                    }
                }
                #endregion

                dr.IsActive = true;
                dr.CreatedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsQuestion.ClinicalQuestions.AddClinicalQuestionsRow(dr);
                BLObject<DSTemplateBuilder> obj = BusinessWrapper.Clinical.BusinessObj.InsertQuestions(dsQuestion);
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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string FillQuestion(Int64 questionId)
        {
            try
            {
                if (string.IsNullOrEmpty(Utility.ToStr(questionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = Utility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSTemplateBuilder dsQuestion = null;
                    BLObject<DSTemplateBuilder> obj = BusinessWrapper.Clinical.BusinessObj.LoadClinicalQuestion(questionId, null, null, null, null, 1, 1);
                    dsQuestion = obj.Data;
                    if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows[0];

                        string CommasToNewLine = Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DisplayTextColumn.ColumnName]).Replace(",", Environment.NewLine);
                        TemplateBuilderModel model = new TemplateBuilderModel();

                        var keyValues = new Dictionary<string, string>
                        {
                            { "ShortName", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.ShortNameColumn.ColumnName])},
                            { "questionType", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.QuestionTypeIdColumn.ColumnName])},
                            { "NewLine", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.IsNewLineColumn.ColumnName])},
                            { "questionName", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DescriptionColumn.ColumnName])},

                            //Text Fields
                            { "singleLine", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.FieldTypeColumn.ColumnName])},
                            { "textlength", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.TextLengthColumn.ColumnName])},
                            { "textCase", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.TextCaseColumn.ColumnName])},
                            { "autoComplete", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.IsAutoCompleteColumn.ColumnName])},
                            { "predeifined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentence", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            //Radio Fields
                            { "label1", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.BoolTrueDisplayColumn.ColumnName])},
                            { "label2", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.BoolFalseDisplayColumn.ColumnName])},
                            { "defaultValue", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            //{ "predefined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            //{ "predefined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceForLabel1", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTrueColumn.ColumnName])},
                            { "sentenceForLabel2", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceFalseColumn.ColumnName])},

                            //Drop Down Fields
                            { "areaElements", CommasToNewLine},
                            { "predefined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceDropDown", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Fraction Field
                            { "questionFarctionLabel1", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.FractionFieldLabel1Column.ColumnName])},
                            { "questionFarctionLabel2", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.FractionFieldLabel2Column.ColumnName])},
                            //{ "prefedined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceFraction", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Number Field
                            { "questionNumberDefault", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            { "questionNumberMinLength", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.MinValueColumn.ColumnName])},
                            { "questionNumberMaxLength", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.MaxValueColumn.ColumnName])},
                            { "questionNumberPrecision", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.NumberPrecesionColumn.ColumnName])},
                            //{ "prefedined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceNumber", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Date Field
                            { "questionDateformat", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn.ColumnName])},
                            { "questionDateformatDefaultValue", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            //{ "prefedined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceDate", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Time Field
                            { "questionTimeFormat", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn.ColumnName])},
                            { "questionTimeDefaultValue", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.DefaultValueColumn.ColumnName])},
                            //{ "prefedined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceTime", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},

                            // Image Field 
                            
                            { "totalFiles", "1 file(s) selected"},
                            { "uploadFile", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.FilePathColumn.ColumnName])},
                            { "prefedined", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceTagColumn.ColumnName])},
                            { "sentenceImage", Utility.ToStr(dr[dsQuestion.ClinicalQuestions.SentenceColumn.ColumnName])},
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        string imageBase64 = "";
                        byte[] imageByteArr = dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName] as byte[];
                        if (imageByteArr != null)
                        {
                            imageBase64 = "data:" + dr[dsQuestion.ClinicalQuestions.FileTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName] as byte[]);
                        }
                        keyValues.Add("imagePreview", imageBase64);
                        var response = new
                        {
                            status = true,
                            QuestionLoad_JSON = js.Serialize(keyValues)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string UpdateQuestion(TemplateBuilderModel model, Int64 questionId, string files)
        {
            try
            {
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj = null;
                obj = BusinessWrapper.Clinical.BusinessObj.LoadClinicalQuestion(questionId, null, null, null, null, 1, 1);
                dsQuestion = obj.Data;
                if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    foreach (DataRow dr in dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.ShortName))
                            dr[dsQuestion.ClinicalQuestions.ShortNameColumn] = model.ShortName;
                        if (!string.IsNullOrEmpty(model.questionType))
                            dr[dsQuestion.ClinicalQuestions.QuestionTypeIdColumn] = model.questionType;
                        if (!string.IsNullOrEmpty(model.questionName))
                            dr[dsQuestion.ClinicalQuestions.DescriptionColumn] = model.questionName;
                        //if (searchedfieldsJson.ContainsKey("chkNewLine"))
                            dr[dsQuestion.ClinicalQuestions.IsNewLineColumn] = model.NewLine == "True" ? true : false;

                        Int64 QuestionType = Utility.ToInt64(model.questionType);

                        #region Text Field
                        if (QuestionType == 1)
                        {
                            if (!string.IsNullOrEmpty(model.textlength))
                                dr[dsQuestion.ClinicalQuestions.TextLengthColumn] = model.textlength;

                            if (!string.IsNullOrEmpty(model.textCase))
                                dr[dsQuestion.ClinicalQuestions.TextCaseColumn] = model.textCase;

                            //if (searchedfieldsJson.ContainsKey("chkAutoComplete"))
                                dr[dsQuestion.ClinicalQuestions.IsAutoCompleteColumn] = model.autoComplete == "True";

                                if (!string.IsNullOrEmpty(model.sentence))
                                    dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentence;

                            if (!string.IsNullOrEmpty(model.sentenceDropDown))
                            {
                                var getTags = GetTokens(Utility.ToStr(model.sentence));
                                string sentenceTags = string.Join(",", getTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = sentenceTags;
                            }
                            //if (searchedfieldsJson.ContainsKey("radSingleLine"))
                                dr[dsQuestion.ClinicalQuestions.FieldTypeColumn] = model.singleLine == "True";
                        }
                        #endregion

                        #region Radio Field
                        else if (QuestionType == 2)
                        {

                            if (!string.IsNullOrEmpty(model.label1))
                                dr[dsQuestion.ClinicalQuestions.BoolTrueDisplayColumn] = model.label1;

                            if (!string.IsNullOrEmpty(model.label2))
                                dr[dsQuestion.ClinicalQuestions.BoolFalseDisplayColumn] = model.label2;

                            if (!string.IsNullOrEmpty(model.defaultValue))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = model.defaultValue;

                            if (!string.IsNullOrEmpty(model.sentenceForLabel1))
                                dr[dsQuestion.ClinicalQuestions.SentenceTrueColumn] = model.sentenceForLabel1;

                            if (!string.IsNullOrEmpty(model.sentenceForLabel2))
                                dr[dsQuestion.ClinicalQuestions.SentenceFalseColumn] = model.sentenceForLabel2;

                            if (!string.IsNullOrEmpty(model.sentenceForLabel1))
                            {
                                var GetTags = GetTokens(model.sentenceForLabel1);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagTrueColumn] = SentenceTags;
                            }

                            if (!string.IsNullOrEmpty(model.sentenceForLabel2))
                            {
                                var GetTags = GetTokens(model.sentenceForLabel2);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagFalseColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region DropDown Field
                        else if (QuestionType == 3)
                        {
                            if (!string.IsNullOrEmpty(model.areaElements))
                                dr[dsQuestion.ClinicalQuestions.DisplayTextColumn] = model.areaElements;

                            if (!string.IsNullOrEmpty(model.sentenceDropDown))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceDropDown;

                            if (!string.IsNullOrEmpty(model.sentenceDropDown))
                            {
                                var GetTags = GetTokens(model.sentenceDropDown);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Fraction Field
                        else if (QuestionType == 5)
                        {
                            if (!string.IsNullOrEmpty(model.questionFarctionLabel1))
                                dr[dsQuestion.ClinicalQuestions.FractionFieldLabel1Column] = model.questionFarctionLabel1;

                            if (!string.IsNullOrEmpty(model.questionFarctionLabel2))
                                dr[dsQuestion.ClinicalQuestions.FractionFieldLabel2Column] = model.questionFarctionLabel2;

                            if (!string.IsNullOrEmpty(model.sentenceFraction))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceFraction;

                            if (!string.IsNullOrEmpty(model.sentenceFraction))
                            {
                                var GetTags = GetTokens(model.sentenceFraction);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Number Field
                        else if (QuestionType == 10)
                        {
                            if (!string.IsNullOrEmpty(model.questionNumberDefault))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = model.questionNumberDefault;

                            if (!string.IsNullOrEmpty(model.questionNumberMinLength))
                                dr[dsQuestion.ClinicalQuestions.MinValueColumn] = model.questionNumberMinLength;

                            if (!string.IsNullOrEmpty(model.questionNumberMaxLength))
                                dr[dsQuestion.ClinicalQuestions.MaxValueColumn] = model.questionNumberMaxLength;

                            if (!string.IsNullOrEmpty(model.questionNumberPrecision))
                                dr[dsQuestion.ClinicalQuestions.NumberPrecesionColumn] = model.questionNumberPrecision;

                            if (!string.IsNullOrEmpty(model.sentenceNumber))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceNumber;

                            if (!string.IsNullOrEmpty(model.sentenceNumber))
                            {
                                var GetTags = GetTokens(model.sentenceNumber);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Date Field
                        else if (QuestionType == 6)
                        {
                            if (!string.IsNullOrEmpty(model.questionDateformat))
                                dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn] = model.questionDateformat;

                            if (!string.IsNullOrEmpty(model.questionDateformatDefaultValue))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = model.questionDateformatDefaultValue;

                            if (!string.IsNullOrEmpty(model.sentenceDate))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceDate;

                            if (!string.IsNullOrEmpty(model.sentenceDate))
                            {
                                var GetTags = GetTokens(model.sentenceDate);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion

                        #region Time Field
                        else if (QuestionType == 7)
                        {
                            if (!string.IsNullOrEmpty(model.questionTimeFormat))
                                dr[dsQuestion.ClinicalQuestions.DateTimeFormatColumn] = model.questionTimeFormat;

                            if (!string.IsNullOrEmpty(model.questionTimeDefaultValue))
                                dr[dsQuestion.ClinicalQuestions.DefaultValueColumn] = model.questionTimeDefaultValue;

                            if (!string.IsNullOrEmpty(model.sentenceTime))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceTime;

                            if (!string.IsNullOrEmpty(model.sentenceTime))
                            {
                                var GetTags = GetTokens(model.sentenceTime);
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

                            if (!string.IsNullOrEmpty(model.uploadFile))
                                dr[dsQuestion.ClinicalQuestions.FilePathColumn] = model.uploadFile;

                            if (!string.IsNullOrEmpty(model.sentenceImage))
                                dr[dsQuestion.ClinicalQuestions.SentenceColumn] = model.sentenceImage;

                            if (!string.IsNullOrEmpty(model.sentenceImage))
                            {
                                var GetTags = GetTokens(model.sentenceImage);
                                string SentenceTags = string.Join(",", GetTags.ToArray());
                                dr[dsQuestion.ClinicalQuestions.SentenceTagColumn] = SentenceTags;
                            }
                        }
                        #endregion


                        dr[dsQuestion.ClinicalQuestions.ModifiedByColumn] = Utility.DecryptFrom64(Common.AppConfig.AppUserName);
                        dr[dsQuestion.ClinicalQuestions.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSTemplateBuilder> objQuestion = null;

                    objQuestion = BusinessWrapper.Clinical.BusinessObj.UpdateQuestions(dsQuestion);

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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string DeleteQuestion(string questionId)
        {
            try
            {
                if (string.IsNullOrEmpty(questionId))
                {
                    var response = new
                    {
                        status = false,
                        Message = Utility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BusinessWrapper.Clinical.BusinessObj.DeleteQuestion(questionId);
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
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private List<String> GetTokens(String str)
        {
            Regex regex = new Regex(@"(?<=\{{)[^}]*(?=\}})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);
            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
        }
    }
}