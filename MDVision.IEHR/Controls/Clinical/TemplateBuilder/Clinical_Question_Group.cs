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

namespace MDVision.IEHR.Controls.Clinical.TemplateBuilder
{
    public class Clinical_Question_Group
    {
         private BLLClinical BLLClinicalObj = null;
         public Clinical_Question_Group()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton

        private static Clinical_Question_Group _instance = null;
        public static Clinical_Question_Group Instance()
        {
            if (_instance == null)
                _instance = new Clinical_Question_Group();
            return _instance;
        }

        #endregion

        #region Private Functions


        #region QuestionGroup
        #region Crud QuestionGroup
        /// <summary>
        /// Saves the Question Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveQuestionGroup(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSTemplateBuilder dsQuestionGroup = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalQuestionGroupRow dr = dsQuestionGroup.ClinicalQuestionGroup.NewClinicalQuestionGroupRow();

                dr.ShortName = SearchedfieldsJSON["ShortName"];
                dr.Description = SearchedfieldsJSON["Description"];
                dr.DefaltText = null;//not implemented yet

                dr.EndingSentence = null;//not implemented yet
                dr.StartingSentence = null;//not implemented yet
                dr.Comment = null;//not implemented yet
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["Canvas"]))
                    dr.Canvas = MDVUtility.ToInt32(SearchedfieldsJSON["Canvas"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["QuestionGroupID"]))
                    dr.QuestionGroupID = MDVUtility.ToInt64(SearchedfieldsJSON["QuestionGroupID"]);

                // if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstSpecialityID"]))
                dr.SortOrder = 0;//not Implemented yet

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["SpecialityID"]))
                    dr.SpecialityID = MDVUtility.ToInt64(SearchedfieldsJSON["SpecialityID"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["BodySystemId"]))
                    dr.BodySystemId = MDVUtility.ToInt64(SearchedfieldsJSON["BodySystemId"]);

                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["Active"]) == "True" ? true : false;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;


                #region Database Insertion
                dsQuestionGroup.ClinicalQuestionGroup.AddClinicalQuestionGroupRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertQuestionGroup(dsQuestionGroup);
                dsQuestionGroup = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        QuestionGroupId = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows[0][dsQuestionGroup.ClinicalQuestionGroup.QuestionGroupIDColumn.ColumnName],
                        QuestionGroupDescription = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows[0][dsQuestionGroup.ClinicalQuestionGroup.DescriptionColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the Question Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string UpdateQuestionGroup(string fieldsJSON, Int64 QuestoinGroupID, string HTMLTempalteQuestionGroup)
        {

            try
            {
                DSTemplateBuilder dsQuestionGroup = new DSTemplateBuilder();
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadQuestionGroup(QuestoinGroupID, 1, 3,null,null,0,0,null);
                dsQuestionGroup = obj.Data;
                if (dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(fieldsJSON) && !fieldsJSON.Equals("null"))
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);
                        foreach (DataRow dr in dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows)
                        {
                            if (searchedfieldsJson.ContainsKey("ShortName") && !string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                                dr[dsQuestionGroup.ClinicalQuestionGroup.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);
                            if (searchedfieldsJson.ContainsKey("Description") && !string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                                dr[dsQuestionGroup.ClinicalQuestionGroup.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["Description"]);
                            if (searchedfieldsJson.ContainsKey("SpecialityID") && !string.IsNullOrEmpty(searchedfieldsJson["SpecialityID"]))
                                dr[dsQuestionGroup.ClinicalQuestionGroup.SpecialityIDColumn] = MDVUtility.ToInt64(searchedfieldsJson["SpecialityID"]);

                            if (searchedfieldsJson.ContainsKey("BodySystemId") && !string.IsNullOrEmpty(searchedfieldsJson["BodySystemId"]))
                                dr[dsQuestionGroup.ClinicalQuestionGroup.BodySystemIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["BodySystemId"]);
                            if (searchedfieldsJson.ContainsKey("Active"))
                                dr[dsQuestionGroup.ClinicalQuestionGroup.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True" ? true : false;

                            //if (!string.IsNullOrEmpty(HTMLTempalteQuestionGroup))
                            //    dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn] = HTMLTempalteQuestionGroup;

                            dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedOnColumn] = DateTime.Now;
                        }
                    }
                    else
                    {
                        //if (HTMLTempalteQuestionGroup != "undefined")
                        //{
                            foreach (DataRow dr in dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows)
                            {
                                if (!string.IsNullOrEmpty(HTMLTempalteQuestionGroup))
                                {
                                    if (HTMLTempalteQuestionGroup != "undefined")
                                    {
                                        //String HtmlDocument = MDVUtility.ToStr(dr[dsSection.ClinicalSection.HTMLTemplateColumn.ColumnName]);
                                        string[] ImageSources = HTMLTempalteQuestionGroup.Split(new string[] { "src=" }, StringSplitOptions.None);
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
                                        HTMLTempalteQuestionGroup = String.Join("", ImageSources);
                                        dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn] = HTMLTempalteQuestionGroup;
                                    }
                                } 

                                dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                dr[dsQuestionGroup.ClinicalQuestionGroup.ModifiedOnColumn] = DateTime.Now;
                            }
                        //}
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns></returns>
        private string LoadQuestionGroup(string fieldsJSON, Int64 questionGroupId, Int64 pageNo, Int64 rpp)
        {
            string chkDiscontinued = null;

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSTemplateBuilder dsQuestionGroup = null;
                BLObject<DSTemplateBuilder> obj;
                if (SearchedfieldsJSON == null || questionGroupId != 0)
                    obj = BLLClinicalObj.LoadQuestionGroup(questionGroupId, pageNo, rpp, null, null, 0, 0, null);
                else
                {
                    obj = BLLClinicalObj.LoadQuestionGroup(questionGroupId, pageNo, rpp, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON.ContainsKey("ddlSpecialityID") ? string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialityID"]) ? 0 : MDVUtility.ToLong(SearchedfieldsJSON["ddlSpecialityID"]) : 0, SearchedfieldsJSON.ContainsKey("ddlBodySystemId") ? string.IsNullOrEmpty(SearchedfieldsJSON["ddlBodySystemId"]) ? 0 : MDVUtility.ToLong(SearchedfieldsJSON["ddlBodySystemId"]) : 0, SearchedfieldsJSON["ddlActive"]);
                    // SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], , SearchedfieldsJSON["ddlActive"], , pageNo, rpp);

                }
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
                            //HTMLTemplate = HtmlDocument,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            QuestionGroupCount = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = 0,
                            QuestionGroupFill_JSON = MDVUtility.JSON_DataTable(dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName]),
                            //HTMLTemplate = HtmlDocument,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            
        }

        /// <summary>
        /// Deletes the Question Group.
        /// </summary>
        /// <param name="QuestionGroupId">The Question Group identifier.</param>
        /// <returns></returns>
        private string DeleteQuestionGroup(Int64 QuestionGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(QuestionGroupId)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteQuestionGroup(MDVUtility.ToStr(QuestionGroupId));
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

        private string UpdateQuestionGroupIsActive(Int64 QuestionGroupId, Int64 IsActive, Int64 pageNo, Int64 rpp)
        {
            try
            {
                DSTemplateBuilder dsQuestionGroup = null;
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadQuestionGroup(QuestionGroupId, pageNo, rpp, null, null, 0, 0, null); 
                dsQuestionGroup = obj.Data;
                if (dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows[0];
                    dr[dsQuestionGroup.ClinicalQuestionGroup.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSTemplateBuilder> objUser = BLLClinicalObj.UpdateQuestionGroup(ref dsQuestionGroup);
                    string successMsg;
                    if (objUser.Data != null)
                    {
                        if (IsActive == 0)
                            successMsg = Common.AppPrivileges.Inactive_Message;
                        else
                            successMsg = Common.AppPrivileges.Active_Message;
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
                            Message = objUser.Message
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
        #endregion

        #region CRUD QuestionGroup Question

        /// <summary>
        /// Deletes the Question From Question Group.
        /// </summary>
        /// <param name="QuestionGroupId">The Question Group identifier.</param>
        /// <returns></returns>
        private string Delete_QuestionFrom_QuestionGroup(Int64 QuestionGroupId, Int64 QuestionId)
        {
            try
            {

                BLObject<string> obj = BLLClinicalObj.DeleteQuestionGroupQuestion(QuestionGroupId, QuestionId);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Load all the QuestoinGroup Question for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns></returns>
        private string LoadQuestionGroupQuestion(string fieldsJSON, Int64 QuesGroupQuestionId, Int64 QuestionGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            //    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSTemplateBuilder dsQuestionGroupQuestion = null;
                BLObject<DSTemplateBuilder> obj;
                //   if (SearchedfieldsJSON == null)
                obj = BLLClinicalObj.LoadQuestionGroupQuestion(QuesGroupQuestionId, QuestionGroupId);
              
                dsQuestionGroupQuestion = obj.Data;
                if (obj.Data != null)
                {
                    if (dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroup.TableName].Rows)
                        //{

                        //    if (!string.IsNullOrEmpty(dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn].ToString()))
                        //        dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn] = MDVUtility.DecryptFrom64(dr[dsQuestionGroup.ClinicalQuestionGroup.HTMLTemplateColumn].ToString());

                        //}
                        var response = new
                        {
                            status = true,
                            QuestionGroupQuestionCount = dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName].Rows.Count,
                          //  iTotalDisplayRecords = dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.Rows[0][dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.RecordCountColumn],
                            QuestionGroupFill_JSON = MDVUtility.JSON_DataTable(dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            QuestionGroupQuestionCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        /// <summary>
        /// Update the Question Group Questions.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string UpdateQuestionGroup(Int64 QuestoinGroupID, Int64 QuestionID)
        {

            try
            {
                DSTemplateBuilder dsQuestionGroupQuestion = new DSTemplateBuilder();
                BLObject<DSTemplateBuilder> obj = null;
                obj = BLLClinicalObj.LoadQuestionGroupQuestion(QuestoinGroupID, QuestionID);
                dsQuestionGroupQuestion = obj.Data;
                if (dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                
                    foreach (DataRow dr in dsQuestionGroupQuestion.Tables[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.TableName].Rows)
                    {

                        dr[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.QuestionIDColumn] = QuestionID;

                        dr[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.QuestionGroupIDColumn] = QuestoinGroupID;
                        dr[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsQuestionGroupQuestion.ClinicalQuestionGroupQuestion.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSTemplateBuilder> objQuestion = null;

                    objQuestion = BLLClinicalObj.UpdateQuestionGroupQuestion(ref dsQuestionGroupQuestion);

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
        /// Saves the Question Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveQuestionGroupQuestions(Int64 QuestoinGroupID, Int64 QuestionID)
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
                    String fieldsJson = "{TextSearched :''}";
                   var ResponseSearch= SearchQuestion(fieldsJson, QuestionID, 1, 1);
                   var searchedfieldsJson = ser.Deserialize<dynamic>(ResponseSearch);                    
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        QuestionLoad_JSON =searchedfieldsJson["QuestionLoad_JSON"],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

          /// <summary>
        /// Saves the Question Group.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string UpdateQuestionGroupQuestions(Int64 QuesGroupQuestionId, Int64 QuestionID)
        {
            try
            {
                DSTemplateBuilder dsQuestionGroup = null;
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadQuestionGroupQuestion(QuesGroupQuestionId, 0);
                dsQuestionGroup = obj.Data;
                if (dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroupQuestion.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsQuestionGroup.Tables[dsQuestionGroup.ClinicalQuestionGroupQuestion.TableName].Rows[0];
                    dr[dsQuestionGroup.ClinicalQuestionGroupQuestion.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    dr[dsQuestionGroup.ClinicalQuestionGroupQuestion.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                   

                    BLObject<DSTemplateBuilder> objUser = BLLClinicalObj.UpdateQuestionGroupQuestion(ref dsQuestionGroup);
                    
                    if (objUser.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = "Updated"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objUser.Message
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

        
        #endregion
        /// <summary>
        /// Updates the Question Group is active.
        /// </summary>
        /// <param name="QuestionGroupId">The Question Group identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
      
        #endregion
        /// <summary>
        /// Searches the Clinical Question.
        /// </summary>
        /// <returns></returns>
        private string SearchQuestion(string fieldsJson, Int64 questionId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj;

               
                var shortName = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];
                var questionName = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];
              
                obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, questionName, null, null, rpp, pageNo);

                

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
                            QuestionCount = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                            iTotalDisplayRecords = 0,
                            QuestionLoad_JSON = MDVUtility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        QuestionCount = dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName].Rows.Count,
                        iTotalDisplayRecords = 0,
                        QuestionLoad_JSON = MDVUtility.JSON_DataTable(dsQuestion.Tables[dsQuestion.ClinicalQuestions.TableName]),
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
        /// Searches the Clinical Question.
        /// </summary>
        /// <returns></returns>
        private string InsertQuestionInQuestionGroup(string fieldsJson, Int64 questionId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                DSTemplateBuilder dsQuestion = null;
                BLObject<DSTemplateBuilder> obj;


                obj = BLLClinicalObj.LoadClinicalQuestion(questionId, null, null, null, null, rpp, pageNo);

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
                            Message = obj.Message
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
                        string fieldsJson = context.Request["QuestionData"];
                        // Int64 questionId = MDVUtility.ToInt64(context.Request["hfQuestionId"]);

                        Int64 questionId = MDVUtility.ToInt64(context.Request["questionID"]);

                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = SearchQuestion(fieldsJson, questionId, rpp, pageNo);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;
                case "SAVE_QUESTION_GROUP":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        string strJSONData = SaveQuestionGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_QUESTION_GROUP":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        string HTMLTempalteQuestionGroup = context.Request["HTMLTempalteQuestionGroup"];
                        string UpdatedHTMLTempalteforQuestionGroup = context.Request["UpdatedHTMLforQuestionGroup"];
                        Int64 questionId = MDVUtility.ToInt64(context.Request["QuestionGroupId"]);
                        if (HTMLTempalteQuestionGroup == "undefined")
                        {
                            HTMLTempalteQuestionGroup = UpdatedHTMLTempalteforQuestionGroup;
                        }
                        string strJSONData = UpdateQuestionGroup(fieldsJSON, questionId, HTMLTempalteQuestionGroup);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_QUESTION_GROUP":
                    {
                        string fieldsJSON = context.Request["QuestionGroupData"];
                        Int64 QuestionGroupId = MDVUtility.ToInt64(context.Request["QuestionGroupId"]);

                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);

                        string strJsonData = LoadQuestionGroup(fieldsJSON, QuestionGroupId, pageNo, rpp);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;


                case "DELETE_QUESTIONFROM_QUESTION_GROUP":
                    {
                        string strQUESTIONGROUPId = context.Request["QUESTIONGROUPID"];

                        string strJSONData = Delete_QuestionFrom_QuestionGroup(MDVUtility.ToInt64(context.Request["QuestionGroupId"]), MDVUtility.ToInt64(context.Request["QuestionID"]));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_QUESTION_GROUP":
                    {
                        string strQUESTIONGROUPId = context.Request["QUESTIONGROUPID"];
                        string strJSONData = DeleteQuestionGroup(MDVUtility.ToInt64(strQUESTIONGROUPId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_QUESTIONS_OF_QUESTIONGROUP":
                    {
                        string fieldsJSON = context.Request["QUESTIONGROUPData"];
                        Int64 QUESTIONGROUPID = MDVUtility.ToInt64(context.Request["QUESTIONGROUPID"]);
                        Int64 QuesGroupQuestionId = MDVUtility.ToInt64(context.Request["QuesGroupQuestionId"]);
                        string strJSONData = LoadQuestionGroupQuestion(fieldsJSON,QuesGroupQuestionId, QUESTIONGROUPID);
                        
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_QUESTION_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 QUESTIONGROUPID = MDVUtility.ToInt64(context.Request["QUESTIONGROUPID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = UpdateQuestionGroupIsActive(QUESTIONGROUPID, IsActive, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "INSERT_QUESTION_IN_QUESTIONGROUP":
                    {
                        string fieldsJson = context.Request["QuestionData"];
                        // Int64 questionId = MDVUtility.ToInt64(context.Request["hfQuestionId"]);

                        Int64 questionId = MDVUtility.ToInt64(context.Request["questionID"]);

                        Int64 QUESTIONGROUPID = MDVUtility.ToInt64(context.Request["QUESTIONGROUPID"]);
                        
                        string strJsonData = SaveQuestionGroupQuestions(QUESTIONGROUPID, questionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;
                case "UPDATE_QUESTION_IN_QUESTIONGROUP":
                    {
                        
                        Int64 QuestionId = MDVUtility.ToInt64(context.Request["QuestionId"]);
                        Int64 QuesGroupQuestionId = MDVUtility.ToInt64(context.Request["QuesGroupQuestionId"]);
                        string strJSONData = UpdateQuestionGroupQuestions(QuesGroupQuestionId, QuestionId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}