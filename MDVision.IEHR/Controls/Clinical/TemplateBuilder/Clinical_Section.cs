using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Clinical
{
    public class Clinical_Section
    {
        private BLLClinical BLLClinicalObj = null;
        public Clinical_Section()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton

        private static Clinical_Section _instance = null;
        public static Clinical_Section Instance()
        {
            return _instance ?? (_instance = new Clinical_Section());
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        /// <summary>
        /// Load Questions
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="rpp"></param>
        /// <param name="page_no"></param>
        private string SearchQuestion(string fieldsJson, Int64 rpp, Int64 page_no)
        {
            //if (fieldsJson == null) throw new ArgumentNullException("fieldsJson");
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                //var QuestionId = MDVUtility.ToInt64(searchedfieldsJson["hfQuestionId"]);
                //if (QuestionId <= 0)
                //{
                //    searchedfieldsJson["hfQuestionId"] = 0;
                //}
                var shortName = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];
                var questionName = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];
                //var activeStatus = searchedfieldsJson["chkIsActice"] == "" ? null : searchedfieldsJson["chkIsActice"];
                //var questionType = MDVUtility.ToInt64(searchedfieldsJson["lstTypeId"]) <= 0 ? null : searchedfieldsJson["lstTypeId"];

                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalQuestion(0, null, questionName, null, null, rpp, page_no);

                var dsQuestion = obj.Data;
                if (obj.Data != null)
                {
                    if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            QuestionCount = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            iTotalDisplayRecords = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            QuestionLoad_JSON = MDVUtility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            QuestionCount = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            iTotalDisplayRecords = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            QuestionLoad_JSON = MDVUtility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Search Section
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        private string SearchSection(string fieldsJson, Int64 pageNo, Int64 rpp, Int64 sectionId)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                BLObject<DSTemplateBuilder> obj;
                if (searchedfieldsJson == null || sectionId != 0)
                    obj = BLLClinicalObj.LoadClinicalSection(sectionId, null, null, null, null, null, null, rpp, pageNo);
                else
                {
                    obj = BLLClinicalObj.LoadClinicalSection(sectionId, searchedfieldsJson["txtSectionShortName"], searchedfieldsJson["txtSectionDescription"], searchedfieldsJson["txtSectionTitle"], searchedfieldsJson.ContainsKey("ddlsectionType") ? string.IsNullOrEmpty(searchedfieldsJson["ddlsectionType"]) ? null : MDVUtility.ToStr(searchedfieldsJson["ddlsectionType"]) : null, searchedfieldsJson.ContainsKey("Specialitydd") ? string.IsNullOrEmpty(searchedfieldsJson["Specialitydd"]) ? null : MDVUtility.ToStr(searchedfieldsJson["Specialitydd"]) : null, searchedfieldsJson["ddlActive"], rpp, pageNo);
                }
                var dsSection = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SectionCount = dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSection.ClinicalSection.Rows[0][dsSection.ClinicalSection.RecordCountColumn],
                            SectionLoad_JSON = MDVUtility.JSON_DataTable(dsSection.Tables[dsSection.ClinicalSection.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            SectionCount = dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count,
                            iTotalDisplayRecords = 0,
                            SectionLoad_JSON = MDVUtility.JSON_DataTable(dsSection.Tables[dsSection.ClinicalSection.TableName]),
                            Message = "No Section Found.",
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        MessageCount = 0,
                        SectionCount = dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count,
                        iTotalDisplayRecords = 0,
                        SectionLoad_JSON = MDVUtility.JSON_DataTable(dsSection.Tables[dsSection.ClinicalSection.TableName]),
                        Message = "No Section Found.",
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fill Question in List
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="rpp"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private string FillQuestion(Int64 questionId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(questionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, null, null, null, rpp, pageNo);
                    if (obj.Data != null)
                    {
                        var dsQuestion = obj.Data;
                        if (dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows[0];
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
                                { "txtareaElementsDropDown", MDVUtility.ToStr(dr[dsQuestion.ClinicalQuestions.DisplayTextColumn.ColumnName])},
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
                            JavaScriptSerializer js = new JavaScriptSerializer();

                            string imageBase64 = "";
                            byte[] imageByteArr = dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName] as byte[];
                            if (imageByteArr != null)
                            {
                                imageBase64 = "data:" + dr[dsQuestion.ClinicalQuestions.FileTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String((byte[])dr[dsQuestion.ClinicalQuestions.ImageStreamColumn.ColumnName]);
                            }
                            keyValues.Add("imdIDPreview", imageBase64);
                            var response = new
                            {
                                status = true,
                                QuestionLoad_JSON = js.Serialize(keyValues)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fills Section
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        private string FillSection(Int64 sectionId, Int64 pageNo, Int64 rpp)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(sectionId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalSection(sectionId, null, null, null, null, null, null, pageNo, rpp);
                    var dsSection = obj.Data;
                    if (dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSection.Tables[dsSection.ClinicalSection.TableName].Rows[0];
                        String HtmlDocument = MDVUtility.ToStr(dr[dsSection.ClinicalSection.HTMLTemplateColumn.ColumnName]);
                        string[] ImageSources = HtmlDocument.Split(new string[] { "src=" }, StringSplitOptions.None);
                        for (int i = 0; i < ImageSources.Length; i++)
                        {
                            if (ImageSources[i].Contains("data:image"))
                            {
                                string ImageString;
                                if (ImageSources[i].Contains(" alt="))
                                {
                                    ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                                    ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " alt=" + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                                }
                            }
                        }

                        HtmlDocument = String.Join("", ImageSources);

                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtSectionDetailShortName", MDVUtility.ToStr(dr[dsSection.ClinicalSection.ShortNameColumn.ColumnName])},
                            { "ddlSectionDetailType", MDVUtility.ToStr(dr[dsSection.ClinicalSection.SectionTypeIdColumn.ColumnName])},
                            { "txtSectionDetailTitle", MDVUtility.ToStr(dr[dsSection.ClinicalSection.SectionTitleColumn.ColumnName])},
                            //{ "ddlSectionDetailCanvasColumns", MDVUtility.ToStr(dr[dsSection.ClinicalSection.CanvasColumnsColumn.ColumnName])},
                            { "ddlSectionDetailSpecialty", MDVUtility.ToStr(dr[dsSection.ClinicalSection.SpecialityIdColumn.ColumnName])},
                            { "txtSectionDetailDescription", MDVUtility.ToStr(dr[dsSection.ClinicalSection.DescriptionColumn.ColumnName])},
                            { "chkSectionDetailActive", MDVUtility.ToStr(dr[dsSection.ClinicalSection.IsActiveColumn.ColumnName])},
                            { "canvasColumn", HtmlDocument},//Utility.ToStr(dr[dsSection.ClinicalSection.HTMLTemplateColumn.ColumnName])
                        };
                        JavaScriptSerializer js = new JavaScriptSerializer();

                        var response = new
                        {
                            status = true,
                            SectionLoad_JSON = js.Serialize(keyValues)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves The ClinicalSection
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveClinicalSection(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSTemplateBuilder dsClinicalSection = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalSectionRow dr = dsClinicalSection.ClinicalSection.NewClinicalSectionRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfSectionId"]))
                    dr.SectionId = MDVUtility.ToInt64(searchedfieldsJson["hfSectionId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailShortName"]))
                    dr.ShortName = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailShortName"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailTitle"]))
                    dr.SectionTitle = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailTitle"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailDescription"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkSectionDetailActive"]) == "True";

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailType"]))
                    dr.SectionTypeId = MDVUtility.ToInt64(searchedfieldsJson["ddlSectionDetailType"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailSpecialty"]))
                    dr.SpecialityId = MDVUtility.ToInt64(searchedfieldsJson["ddlSectionDetailSpecialty"]);

                //if (!string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailCanvasColumns"]))
                dr.CanvasColumns = null;// MDVUtility.ToStr(searchedfieldsJson["ddlSectionDetailCanvasColumns"]);

                dr.IsRecurring = false;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;


                #region Database Insertion
                dsClinicalSection.ClinicalSection.AddClinicalSectionRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertSection(dsClinicalSection);
                dsClinicalSection = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        MessageId = dsClinicalSection.Tables[dsClinicalSection.ClinicalSection.TableName].Rows[0][dsClinicalSection.ClinicalSection.SectionIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Delete the Section
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        private string DeleteSection(string sectionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sectionId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteSection(sectionId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Update active inactive status of Section
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private string UpdateSectionIsActive(Int64 sectionId, Int64 isActive)
        {
            try
            {
                var obj = BLLClinicalObj.LoadClinicalSection(sectionId, null, null, null, null, null, null, 1, 3);
                var dsSection = obj.Data;
                if (dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsSection.Tables[dsSection.ClinicalSection.TableName].Rows[0];
                    dr[dsSection.ClinicalSection.IsActiveColumn.ColumnName] = isActive;

                    BLObject<DSTemplateBuilder> objSection = BLLClinicalObj.UpdateSection(dsSection);
                    if (objSection.Data != null)
                    {
                        var successMsg = isActive == 0 ? AppPrivileges.Inactive_Message : AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            message = successMsg
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objSection.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Updates the Section
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        private string UpdateSection(string fieldsJson, Int64 sectionId, string HTMLTempalteSection)
        {
            try
            {
                var obj = BLLClinicalObj.LoadClinicalSection(sectionId, null, null, null, null, null, null, 1, 1);
                var dsSection = obj.Data;
                if (dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count > 0)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsSection.Tables[dsSection.ClinicalSection.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtSectionDetailShortName") && !string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailShortName"]))
                            dr[dsSection.ClinicalSection.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailShortName"]);
                        if (searchedfieldsJson.ContainsKey("ddlSectionDetailType") && !string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailType"]))
                            dr[dsSection.ClinicalSection.SectionTypeIdColumn] = MDVUtility.ToStr(searchedfieldsJson["ddlSectionDetailType"]);
                        if (searchedfieldsJson.ContainsKey("ddlSectionDetailSpecialty") && !string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailSpecialty"]))
                            dr[dsSection.ClinicalSection.SpecialityIdColumn] = MDVUtility.ToStr(searchedfieldsJson["ddlSectionDetailSpecialty"]);

                        if (searchedfieldsJson.ContainsKey("txtSectionDetailDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailDescription"]))
                            dr[dsSection.ClinicalSection.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailDescription"]);
                        //if (searchedfieldsJson.ContainsKey("ddlSectionDetailCanvasColumns") && !string.IsNullOrEmpty(searchedfieldsJson["ddlSectionDetailCanvasColumns"]))
                        //    dr[dsSection.ClinicalSection.CanvasColumnsColumn] = MDVUtility.ToStr(searchedfieldsJson["ddlSectionDetailCanvasColumns"]);
                        if (searchedfieldsJson.ContainsKey("txtSectionDetailTitle") && !string.IsNullOrEmpty(searchedfieldsJson["txtSectionDetailTitle"]))
                            dr[dsSection.ClinicalSection.SectionTitleColumn] = MDVUtility.ToStr(searchedfieldsJson["txtSectionDetailTitle"]);
                        if (searchedfieldsJson.ContainsKey("chkSectionDetailActive"))
                            dr[dsSection.ClinicalSection.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkSectionDetailActive"]) == "True";

                        if (!string.IsNullOrEmpty(HTMLTempalteSection))
                        {
                            if (HTMLTempalteSection != "undefined")
                            {
                                string[] ImageSources = HTMLTempalteSection.Split(new string[] { "src=" }, StringSplitOptions.None);
                                for (int i = 0; i < ImageSources.Length; i++)
                                {
                                    if (ImageSources[i].Contains("data:image"))
                                    {
                                        string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                                        ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " alt= " + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                                    }
                                }

                                HTMLTempalteSection = String.Join("", ImageSources);
                                dr[dsSection.ClinicalSection.HTMLTemplateColumn] = HTMLTempalteSection;
                            }
                        }

                        dr[dsSection.ClinicalSection.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsSection.ClinicalSection.ModifiedOnColumn] = DateTime.Now;
                    }

                    var objSection = BLLClinicalObj.UpdateSection(dsSection);

                    if (objSection.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objSection.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Load Question Group From Section
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="questionGroupId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        private string LoadQuestionGroup(string fieldsJSON, Int64 questionGroupId, Int64 pageNo, Int64 rpp)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                var shortName = (SearchedfieldsJSON["TextSearched"] == "" || SearchedfieldsJSON["TextSearched"] == "undefined") ? null : SearchedfieldsJSON["TextSearched"];
                var questionGroupName = (SearchedfieldsJSON["TextSearched"] == "" || SearchedfieldsJSON["TextSearched"] == "undefined") ? null : SearchedfieldsJSON["TextSearched"];

                DSTemplateBuilder dsQuestionGroup = null;
                BLObject<DSTemplateBuilder> obj;
                obj = BLLClinicalObj.LoadQuestionGroup(questionGroupId, pageNo, rpp, null, questionGroupName, 0, 0, null);
                dsQuestionGroup = obj.Data;
                if (obj.Data != null)
                {
                    if (dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            QuestionGroupCount = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = dsQuestionGroup.ClinicalQuestionGroup.Rows[0][dsQuestionGroup.ClinicalQuestionGroup.RecordCountColumn],
                            QuestionGroupFill_JSON = MDVUtility.JSON_DataTable(dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CPTCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        QUESTIONGROUPCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Save the QuestionGroup in QuestionGroupSection
        /// </summary>
        /// <param name="SectionID"></param>
        /// <param name="QuestoinGroupID"></param>
        /// <returns></returns>
        private string Save_Section_QuestionGroups(Int64 SectionID, Int64 QuestoinGroupID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                DSTemplateBuilder dsSectionQuestionGroup = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalSectionQuestionGroupRow dr = dsSectionQuestionGroup.ClinicalSectionQuestionGroup.NewClinicalSectionQuestionGroupRow();
                dr.QuesGroupSectionId = -1;
                dr.SectionID = SectionID;
                dr.QuestionGroupId = QuestoinGroupID;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsSectionQuestionGroup.ClinicalSectionQuestionGroup.AddClinicalSectionQuestionGroupRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertSectionQuestionGroup(dsSectionQuestionGroup);
                dsSectionQuestionGroup = obj.Data;
                if (obj.Data != null)
                {
                    String fieldsJson = "{TextSearched :''}";
                    var ResponseSearch = LoadQuestionGroup(fieldsJson, QuestoinGroupID, 1, 1);
                    var searchedfieldsJson = ser.Deserialize<dynamic>(ResponseSearch);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        QuestionGroupLoad_JSON = searchedfieldsJson["QuestionGroupFill_JSON"],
                        QuesGroupSectionId = dsSectionQuestionGroup.Tables[dsSectionQuestionGroup.ClinicalSectionQuestionGroup.TableName].Rows[0][dsSectionQuestionGroup.ClinicalSectionQuestionGroup.QuesGroupSectionIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    string message = obj.Message;
                    if (message.Contains("UNIQUE KEY"))
                        message = "Cannot insert duplicate QuestionGroup.";

                    var response = new
                    {
                        status = false,
                        Message = message
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

        /// <summary>
        /// Delete the QuestionGroup from QuestiongroupSection Table
        /// </summary>
        /// <param name="SectionQuestionGroupId"></param>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        private string Delete_QuestionGroup_From_Section(Int64 SectionQuestionGroupId, Int64 SectionId)
        {
            try
            {

                BLObject<string> obj = BLLClinicalObj.DeleteSectionQuestionGroup(SectionQuestionGroupId, SectionId);
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

        /// <summary>
        /// Load all the Questiongroups of Currrent Section
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="QuesGroupSectionId"></param>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        private string LoadSectionQuestionGroup(string fieldsJSON, Int64 QuesGroupSectionId, Int64 SectionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSTemplateBuilder dsSectionQuestionGroup = null;
                BLObject<DSTemplateBuilder> obj;
                obj = BLLClinicalObj.LoadSectionQuestionGroup(QuesGroupSectionId, SectionId);

                dsSectionQuestionGroup = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSectionQuestionGroup.Tables[dsSectionQuestionGroup.ClinicalSectionQuestionGroup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SectionQuestionGroupCount = dsSectionQuestionGroup.Tables[dsSectionQuestionGroup.ClinicalSectionQuestionGroup.TableName].Rows.Count,
                            SectionFill_JSON = MDVUtility.JSON_DataTable(dsSectionQuestionGroup.Tables[dsSectionQuestionGroup.ClinicalSectionQuestionGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            SectionQuestionGroupCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        SectionCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Change the HTML of Question Group as QuestionGroup is Modified
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="QuestoinGroupID"></param>
        /// <param name="HTMLTempalteQuestionGroup"></param>
        /// <returns></returns>
        private string UpdateQuestionGroupfromSection(string fieldsJSON, Int64 QuestoinGroupID, string HTMLTempalteQuestionGroup)
        {

            try
            {
                DSTemplateBuilder dsQuestionGroup = new DSTemplateBuilder();
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadQuestionGroup(QuestoinGroupID, 1, 3, null, null, 0, 0, null);
                dsQuestionGroup = obj.Data;
                if (dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);
                    foreach (DataRow dr in dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows)
                    {
                        //if (!string.IsNullOrEmpty(HTMLTempalteQuestionGroup))
                        //    dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn] = HTMLTempalteQuestionGroup;

                        if (!string.IsNullOrEmpty(HTMLTempalteQuestionGroup))
                        {
                            if (HTMLTempalteQuestionGroup != "undefined")
                            {
                                string[] ImageSources = HTMLTempalteQuestionGroup.Split(new string[] { "src=" }, StringSplitOptions.None);
                                for (int i = 0; i < ImageSources.Length; i++)
                                {
                                    if (ImageSources[i].Contains("data:image"))
                                    {
                                        string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                                        ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " alt=" + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                                    }
                                }

                                HTMLTempalteQuestionGroup = String.Join("", ImageSources);
                                dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn] = HTMLTempalteQuestionGroup;
                            }
                        }

                        dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedOnColumn] = DateTime.Now;
                    }

                    BLObject<DSTemplateBuilder> objQuestion = null;

                    objQuestion = BLLClinicalObj.UpdateQuestionGroup(ref dsQuestionGroup);

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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Change the HTML of QuestionGroup in Section as Questions Modified in Questiongroup of Section
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="QuestoinGroupID"></param>
        /// <param name="QuesGroupSectionID"></param>
        /// <returns></returns>
        private string UpdateQuestionGroupSectionfromSection(string fieldsJSON, Int64 QuestoinGroupID, Int64 QuesGroupSectionID)
        {

            try
            {
                DSTemplateBuilder dsQuestionGroupSection = new DSTemplateBuilder();
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadSectionQuestionGroup(QuesGroupSectionID, 0);
                dsQuestionGroupSection = obj.Data;
                if (dsQuestionGroupSection.Tables[dsQuestionGroupSection.ClinicalSectionQuestionGroup.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);
                    foreach (DataRow dr in dsQuestionGroupSection.Tables[dsQuestionGroupSection.ClinicalSectionQuestionGroup.TableName].Rows)
                    {
                        dr[dsQuestionGroupSection.ClinicalSectionQuestionGroup.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsQuestionGroupSection.ClinicalSectionQuestionGroup.ModifiedOnColumn] = DateTime.Now;
                    }

                    BLObject<DSTemplateBuilder> objQuestion = null;

                    objQuestion = BLLClinicalObj.UpdateSectionQuestionGroup(ref dsQuestionGroupSection);

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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Save Questions in QuestionGroup which were added from Section Screen
        /// </summary>
        /// <param name="QuestoinGroupID"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        private string SaveQuestionGroupQuestionsfromSection(Int64 QuestoinGroupID, Int64 QuestionID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();



                DSTemplateBuilder dsQuestionGroupQuestion = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalQuestionGroupQuestionRow dr = dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.NewClinicalQuestionGroupQuestionRow();
                dr.QuesGroupQuestionId = -1;
                dr.QuestionGroupID = QuestoinGroupID;
                dr.QuestionID = QuestionID;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.AddClinicalQuestionGroupQuestionRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertQuestionGroupQuestion(dsQuestionGroupQuestion);
                dsQuestionGroupQuestion = obj.Data;
                if (obj.Data != null)
                {
                    //String fieldsJson = "{TextSearched :''}";
                    //var ResponseSearch = SearchQuestion(fieldsJson, QuestionID, 1, 1);
                    //var searchedfieldsJson = ser.Deserialize<dynamic>(ResponseSearch);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        //QuestionLoad_JSON = searchedfieldsJson["QuestionLoad_JSON"],
                        QuesGroupQuestionId = dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName].Rows[0][dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.QuesGroupQuestionIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    string message = obj.Message;
                    if (message.Contains("UNIQUE KEY"))
                        message = "Cannot insert duplicate QuestionGroup.";

                    var response = new
                    {
                        status = false,
                        Message = message
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
                #region Question QuestionGroup Search

                case "SEARCH_QUESTION":
                    {
                        string fieldsJson = context.Request["QuestionData"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 page_no = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = SearchQuestion(fieldsJson, rpp, page_no);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SEARCH_QUESTION_GROUP":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        Int64 questionId = MDVUtility.ToInt64(context.Request["QuestionGroupId"]);

                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);

                        string strJsonData = LoadQuestionGroup(fieldsJSON, questionId, pageNo, rpp);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                #endregion

                #region Section Handle

                case "FILL_QUESTION":
                    {
                        string strQuestionId = context.Request["QuestionId"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 page_no = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = FillQuestion(MDVUtility.ToInt64(strQuestionId), rpp, page_no);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_SECTION":
                    {
                        string sectionId = context.Request["sectionId"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 page_no = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = FillSection(MDVUtility.ToInt64(sectionId), page_no, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_SECTION":
                    {
                        string fieldsJson = context.Request["SectionData"];
                        string strJsonData = SaveClinicalSection(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SEARCH_SECTION":
                    {
                        string fieldsJson = context.Request["SectionData"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        Int64 sectionID = MDVUtility.ToInt64(context.Request["sectionID"]);
                        string strJsonData = SearchSection(fieldsJson, rpp, pageNo, sectionID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_SECTION":
                    {
                        String sectionId = context.Request["sectionID"];
                        string strJsonData = DeleteSection(sectionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_SECTION_ACTIVE_INACTIVE":
                    {
                        Int64 sectionId = MDVUtility.ToInt64(context.Request["SectionID"]);
                        Int64 isActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJsonData = UpdateSectionIsActive(sectionId, isActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_SECTION":
                    {
                        string fieldsJson = context.Request["SectionData"];
                        string HTMLTempalteSection = context.Request["HTMLTempalteSection"];
                        Int64 sectionId = MDVUtility.ToInt64(context.Request["SectionID"]);
                        string strJsonData = UpdateSection(fieldsJson, sectionId, HTMLTempalteSection);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                #endregion

                #region Section QuestionGroup Handle

                case "INSERT_QUESTIONGROUP_SECTION":
                    {
                        string fieldsJson = context.Request["QuestionGroupData"];
                        Int64 questionGroupID = MDVUtility.ToInt64(context.Request["questionGroupID"]);
                        Int64 sectionId = MDVUtility.ToInt64(context.Request["SectionId"]);
                        string strJsonData = Save_Section_QuestionGroups(sectionId, questionGroupID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_QUESTION_GROUP_FROM_SECTION":
                    {
                        string SectionID = context.Request["SectionID"];

                        string strJSONData = Delete_QuestionGroup_From_Section(MDVUtility.ToInt64(context.Request["Section_QuestionGroupId"]), MDVUtility.ToInt64(context.Request["SectionID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "GETQUESTIONGROUP_SECTIONS":
                    {
                        string fieldsJSON = context.Request["fieldsJSON"];
                        Int64 SectionId = MDVUtility.ToInt64(context.Request["SectionId"]);
                        Int64 QuesGroupSectionId = MDVUtility.ToInt64(context.Request["QuesGroupSectionId"]);
                        string strJSONData = LoadSectionQuestionGroup(fieldsJSON, QuesGroupSectionId, SectionId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_QUESTION_GROUP_FROM_SECTION":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        string HTMLTempalteQuestionGroup = context.Request["HTMLSectionQuestionGroup"];
                        string UpdatedHTMLTempalteforQuestionGroup = context.Request["UpdatedQuestionGroupHTML"];
                        Int64 QuestionGroupId = MDVUtility.ToInt64(context.Request["QuestionGroupId"]);
                        if (HTMLTempalteQuestionGroup == "undefined")
                        {
                            HTMLTempalteQuestionGroup = UpdatedHTMLTempalteforQuestionGroup;
                        }
                        string strJSONData = UpdateQuestionGroupfromSection(fieldsJSON, QuestionGroupId, HTMLTempalteQuestionGroup);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_QUESTION_GROUP_SECTION_FROM_SECTION":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        Int64 QuesGroupSectionId = MDVUtility.ToInt64(context.Request["QuesGroupSectionId"]);
                        Int64 QuestionGroupId = MDVUtility.ToInt64(context.Request["QuestionGroupId"]);
                        string strJSONData = UpdateQuestionGroupSectionfromSection(fieldsJSON, QuestionGroupId, QuesGroupSectionId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "INSERT_QUESTION_IN_QUESTIONGROUP_FROM_SECTION":
                    {
                        Int64 questionId = MDVUtility.ToInt64(context.Request["questionID"]);
                        Int64 questionGroupId = MDVUtility.ToInt64(context.Request["questionGroupId"]);

                        string strJsonData = SaveQuestionGroupQuestionsfromSection(questionGroupId, questionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                    #endregion
            }
        }
        #endregion
    }
}