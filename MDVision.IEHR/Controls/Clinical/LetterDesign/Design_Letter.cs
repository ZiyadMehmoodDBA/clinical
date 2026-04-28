using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using MDVision.Business.BCommon;
using MDVision.Datasets;

using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Clinical
{
    public class Design_Letter
    {
        private BLLClinical BLLClinicalObj = null;
        public Design_Letter()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton

        private static Design_Letter _instance = null;
        public static Design_Letter Instance()
        {
            return _instance ?? (_instance = new Design_Letter());
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        /// <summary>
        /// Searches the Letter.
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="letterId"></param>
        /// <returns></returns>
        private string SearchLetter(string fieldsJson, Int64 letterId)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj;

                var letterID = MDVUtility.ToInt64(searchedfieldsJson["hfLetterId"]);
                if (letterID <= 0)
                {
                    searchedfieldsJson["hfQuestionId"] = 0;
                }
                var name = searchedfieldsJson["txtLetterName"] == "" ? null : searchedfieldsJson["txtLetterName"];
                var description = searchedfieldsJson["txtLetterDescription"] == "" ? null : searchedfieldsJson["txtLetterDescription"];
                var categoryId = string.Empty;
                var entityId = string.Empty;
                var documentId = string.Empty;
                if (searchedfieldsJson.ContainsKey("lstCategoryId"))
                {
                    categoryId = MDVUtility.ToInt64(searchedfieldsJson["lstCategoryId"]) <= 0 ? null : searchedfieldsJson["lstCategoryId"];
                }
                if (searchedfieldsJson.ContainsKey("lstFolderTypeId"))
                {
                    documentId = MDVUtility.ToInt64(searchedfieldsJson["lstFolderTypeId"]) <= 0 ? null : searchedfieldsJson["lstFolderTypeId"];

                }
                if (searchedfieldsJson.ContainsKey("lstEntityId"))
                {
                    entityId = MDVUtility.ToInt64(searchedfieldsJson["lstEntityId"]) <= 0 ? null : searchedfieldsJson["lstEntityId"];
                }
                var isLabeled = MDVUtility.ToStr(searchedfieldsJson["chkLabel"]) == "True" ? true : false;
                var isActive = MDVUtility.ToStr(searchedfieldsJson["ddlActive"]);//== "True" ? true : false;

                obj = BLLClinicalObj.LoadLetter(letterID, name, description, categoryId, documentId, entityId, isLabeled, isActive);

                dsLetter = obj.Data;
                if (obj.Data != null)
                {
                    if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            letterCount = dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count,
                            letterLoad_JSON = MDVUtility.JSON_DataTable(dsLetter.Tables[dsLetter.Letter.TableName])
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
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
        /// Inserts the Letter.
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveLetter(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSLetter dsLetter = new DSLetter();
                DSLetter.LetterRow dr = dsLetter.Letter.NewLetterRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfLetterId"]))
                    dr.TemplateLetterId = MDVUtility.ToInt64(searchedfieldsJson["hfLetterId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtLetterName"]))
                    dr.ShortName = MDVUtility.ToStr(searchedfieldsJson["txtLetterName"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtLetterDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtLetterDescription"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstCategoryId"]))
                    dr.CategoryId = MDVUtility.ToInt64(searchedfieldsJson["lstCategoryId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstFolderTypeId"]))
                    dr.DocumentTypeId = MDVUtility.ToInt64(searchedfieldsJson["lstFolderTypeId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstEntityId"]))
                    dr.EntityId = MDVUtility.ToInt64(searchedfieldsJson["lstEntityId"]);

                dr.IsLabeled = MDVUtility.ToStr(searchedfieldsJson["chkLabel"]) == "True";

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True";

                if (!string.IsNullOrEmpty(searchedfieldsJson["elm1"]))
                    dr.HtmlDocument = MDVUtility.ToStr(searchedfieldsJson["elm1"]);

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsLetter.Letter.AddLetterRow(dr);
                BLObject<DSLetter> obj = BLLClinicalObj.InsertLetter(dsLetter);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        LetterId = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0][dsLetter.Letter.TemplateLetterIdColumn.ColumnName]
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
        /// Fills the Letter.
        /// </summary>
        /// <param name="LetterId">The Letter identifier.</param>
        /// <returns></returns>
        private string FillLetter(Int64 LetterId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LetterId)))
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
                    DSLetter dsLetter = null;
                    BLObject<DSLetter> obj = BLLClinicalObj.LoadLetter(LetterId, null, null, null, null, null, null, null);

                    if (obj.Data != null)
                    {
                        dsLetter = obj.Data;
                        if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                        {


                            DataRow dr = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0];
                            String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.Letter.HtmlDocumentColumn.ColumnName]);
                            string[] ImageSources = HtmlDocument.Split(new string[] { "src=" }, StringSplitOptions.None);
                            for (int i = 0; i < ImageSources.Length; i++)
                            {
                                if (ImageSources[i].Contains("data:image"))
                                {
                                    string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                                    ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " " + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                                }
                            }

                            HtmlDocument = String.Join("", ImageSources);
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtLetterName", MDVUtility.ToStr(dr[dsLetter.Letter.ShortNameColumn.ColumnName])},
                                { "lstCategoryId", MDVUtility.ToStr(dr[dsLetter.Letter.CategoryIdColumn.ColumnName])},
                                { "chkActive", MDVUtility.ToStr(dr[dsLetter.Letter.IsActiveColumn.ColumnName])},
                                { "txtLetterDescription", MDVUtility.ToStr(dr[dsLetter.Letter.DescriptionColumn.ColumnName])},
                                { "chkLabel", MDVUtility.ToStr(dr[dsLetter.Letter.IsLabeledColumn.ColumnName])},
                                { "hfLetterId", MDVUtility.ToStr(dr[dsLetter.Letter.TemplateLetterIdColumn.ColumnName])},
                                { "lstFolderTypeId", MDVUtility.ToStr(dr[dsLetter.Letter.DocumentTypeIdColumn.ColumnName])},
                                { "lstEntityId", MDVUtility.ToStr(dr[dsLetter.Letter.EntityIdColumn.ColumnName])},
                                { "elm1",HtmlDocument}// MDVUtility.ToStr(dr[dsLetter.Letter.HtmlDocumentColumn.ColumnName])}
                           
                            };
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                LetterLoad_JSON = js.Serialize(keyValues)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = MDVUtility.ToStr(AppPrivileges.No_Record_Message)
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message)
                        };
                        return JsonConvert.SerializeObject(response);
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
        /// Updates the Letter.
        /// </summary>
        /// <returns></returns>
        private string UpdateLetter(string fieldsJson, Int64 letterId, string letterBody)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                obj = BLLClinicalObj.LoadLetter(letterId, null, null, null, null, null, null, null);
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    foreach (DataRow dr in dsLetter.Tables[dsLetter.Letter.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtLetterName") && !string.IsNullOrEmpty(searchedfieldsJson["txtLetterName"]))
                            dr[dsLetter.Letter.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["txtLetterName"]);
                        if (searchedfieldsJson.ContainsKey("txtLetterDescription") && !string.IsNullOrEmpty(searchedfieldsJson["txtLetterDescription"]))
                            dr[dsLetter.Letter.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["txtLetterDescription"]);
                        if (searchedfieldsJson.ContainsKey("lstCategoryId") && !string.IsNullOrEmpty(searchedfieldsJson["lstCategoryId"]))
                            dr[dsLetter.Letter.CategoryIdColumn] = MDVUtility.ToStr(searchedfieldsJson["lstCategoryId"]);
                        if (searchedfieldsJson.ContainsKey("lstFolderTypeId") && !string.IsNullOrEmpty(searchedfieldsJson["lstFolderTypeId"]))
                            dr[dsLetter.Letter.DocumentTypeIdColumn] = MDVUtility.ToStr(searchedfieldsJson["lstFolderTypeId"]);
                        if (searchedfieldsJson.ContainsKey("lstEntityId") && !string.IsNullOrEmpty(searchedfieldsJson["lstEntityId"]))
                            dr[dsLetter.Letter.EntityIdColumn] = MDVUtility.ToStr(searchedfieldsJson["lstEntityId"]);

                        if (searchedfieldsJson.ContainsKey("elm1") && !string.IsNullOrEmpty(letterBody))
                        {
                            String LetterDetail = MDVUtility.ToStr(searchedfieldsJson["elm1"]);
                            dr[dsLetter.Letter.HtmlDocumentColumn] = letterBody.Length > LetterDetail.Length ? letterBody : LetterDetail;

                        }


                        if (searchedfieldsJson.ContainsKey("chkLabel"))
                            dr[dsLetter.Letter.IsLabeledColumn] = MDVUtility.ToStr(searchedfieldsJson["chkLabel"]) == "True" ? true : false;

                        if (searchedfieldsJson.ContainsKey("chkActive"))
                            dr[dsLetter.Letter.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["chkActive"]) == "True" ? true : false;


                        dr[dsLetter.Letter.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsLetter.Letter.ModifiedOnColumn] = DateTime.Now;
                    }
                    BLObject<DSLetter> objLetter = null;
                    objLetter = BLLClinicalObj.UpdateLetter(dsLetter);

                    if (objLetter.Data != null)
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
                            Message = objLetter.Message
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
        /// Inserts the Fields.
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveFields(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSLetter dsFields = new DSLetter();
                DSLetter.FieldsRow dr = dsFields.Fields.NewFieldsRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfFieldsId"]))
                    dr.FieldId = MDVUtility.ToInt64(searchedfieldsJson["hfFieldsId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtDataFieldName"]))
                    dr.Name = MDVUtility.ToStr(searchedfieldsJson["txtDataFieldName"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["txtDataFieldDescription"]))
                    dr.Description = MDVUtility.ToStr(searchedfieldsJson["txtDataFieldDescription"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ddlCategory"]))
                    dr.CategoryId = MDVUtility.ToInt64(searchedfieldsJson["ddlCategory"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstEntityId"]))
                    dr.EntityId = MDVUtility.ToInt64(searchedfieldsJson["lstEntityId"]);

                dr.ManualInput = MDVUtility.ToStr(searchedfieldsJson["chkManualInput"]) == "True";

                dr.NullAllowed = MDVUtility.ToStr(searchedfieldsJson["chkNullAllowed"]) == "True";

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkIsActive"]) == "True";

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFields.Fields.AddFieldsRow(dr);
                BLObject<DSLetter> obj = BLLClinicalObj.InsertField(dsFields);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        MessageId = dsFields.Tables[dsFields.Fields.TableName].Rows[0][dsFields.Fields.FieldIdColumn.ColumnName]
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
        /// Inserts the Letter Fields.
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveLetterFields(string fieldsJson, Int64 letterId)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSLetter dsLetterFields = new DSLetter();
                DSLetter.LetterFieldsRow dr = dsLetterFields.LetterFields.NewLetterFieldsRow();

                if (!string.IsNullOrEmpty(searchedfieldsJson["hfLetterFieldsId"]))
                    dr.LtrFieldId = MDVUtility.ToInt64(searchedfieldsJson["hfLetterFieldsId"]);

                dr.LetterId = letterId;

                string lstfieldId = searchedfieldsJson["lstFieldId"];
                string[] strArray = lstfieldId.Split('~');

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstFieldId"]))
                    dr.FieldId = Int64.Parse(strArray[0]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["lstFormatId"]))
                    dr.FormatId = MDVUtility.ToInt64(searchedfieldsJson["lstFormatId"]);

                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["chkIsActive"]) == "True";


                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsLetterFields.LetterFields.AddLetterFieldsRow(dr);
                BLObject<DSLetter> obj = BLLClinicalObj.InsertLetterFields(dsLetterFields);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        MessageId = dsLetterFields.Tables[dsLetterFields.LetterFields.TableName].Rows[0][dsLetterFields.LetterFields.LtrFieldIdColumn.ColumnName]
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

        private string LoadLetterFields(Int32 LtrFieldId, Int32 LetterId)
        {
            try
            {
                //System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                //var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSLetter dsLetter = null;
                BLObject<DSLetter> obj;
                obj = BLLClinicalObj.LoadLetterFields(LtrFieldId, LetterId);
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.LetterFields.TableName].Rows.Count > 0)
                {

                    DataRow dr = dsLetter.Tables[dsLetter.LetterFields.TableName].Rows[0];
                    String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.LetterFields.HtmlDocumentColumn.ColumnName]);
                    string[] ImageSources = HtmlDocument.Split(new string[] { "src=" }, StringSplitOptions.None);
                    for (int i = 0; i < ImageSources.Length; i++)
                    {
                        if (ImageSources[i].Contains("data:image"))
                        {
                            string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                            ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " " + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                        }
                    }

                    HtmlDocument = String.Join("", ImageSources);

                    var response = new
                    {
                        status = true,
                        LetterFieldsCount = dsLetter.Tables[dsLetter.LetterFields.TableName].Rows.Count,
                        LetterFieldsLoad_JSON = MDVUtility.JSON_DataTable(dsLetter.Tables[dsLetter.LetterFields.TableName]),
                        Letters_JSON = MDVUtility.JSON_DataTable(dsLetter.Tables[dsLetter.Letter.TableName]),
                        HtmlDocument,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        LetterFieldsCount = 0,
                        Message = "Record not found."
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

        private string LetterUpdateActiveInactive(Int64 LetterID, Int64 pageNo, Int64 rpp, Int64 IsActive)
        {
            try
            {

                DSLetter dsLetterFields = new DSLetter();

                BLObject<DSLetter> obj = BLLClinicalObj.LoadLetter(LetterID, null, null, null, null, null, null, null);
                dsLetterFields = obj.Data;
                if (dsLetterFields.Tables[dsLetterFields.Letter.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLetterFields.Tables[dsLetterFields.Letter.TableName].Rows[0];
                    dr[dsLetterFields.Letter.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSLetter> objLetter = BLLClinicalObj.UpdateLetter(dsLetterFields);
                    string successMsg;
                    if (objLetter.Data != null)
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
                            Message = objLetter.Message
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
        /// Deletes the Letter.
        /// </summary>
        /// <param name="MessageID">The Message identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string DeleteLetter(long LetterID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LetterID)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteLetter(MDVUtility.ToStr(LetterID));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        private string DELETED_FieldsIDsFrom_USER(string deletedFieldsIDsFromLetter, Int64 LetterID)
        {
            try
            {
                #region Database Insertion

                BLObject<string> obj = BLLClinicalObj.DELETED_FieldsIDsFrom_USER(deletedFieldsIDsFromLetter, LetterID);
                if (obj.Data != null)
                {
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                case "SEARCH_LETTER":
                    {
                        string fieldsJson = context.Request["letterData"];
                        Int64 letterId = MDVUtility.ToInt64(context.Request["hfLetterId"]);
                        string strJsonData = SearchLetter(fieldsJson, letterId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_LETTER":
                    {
                        string fieldsJson = context.Request["letterData"];
                        string strJsonData = SaveLetter(fieldsJson);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_LETTER":
                    {
                        string strLetterId = context.Request["LetterId"];
                        string strJSONData = FillLetter(MDVUtility.ToInt64(strLetterId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_LETTER":
                    {

                        string fieldsJson = context.Request["letterData"];
                        Int64 letterId = MDVUtility.ToInt64(context.Request["letterID"]);
                        string letterBody = context.Request["letterBody"];

                        string strJsonData = UpdateLetter(fieldsJson.Replace(@"||", @"&"), letterId, letterBody.Replace(@"||", @"&"));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "UPDATE_LETTERS_ACTIVE_INACTIVE":
                    {
                        Int64 letterId = MDVUtility.ToInt64(context.Request["letterID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                        Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);
                        string strJSONData = LetterUpdateActiveInactive(letterId, pageNo, rpp, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_LETTER":
                    {
                        Int64 letterID = MDVUtility.ToInt64(context.Request["letterID"]);
                        string strJSONData = DeleteLetter(letterID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_FIELDS":
                    {
                        string fieldsJson = context.Request["fieldsData"];
                        string strJsonData = SaveFields(fieldsJson);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_LETTER_FIELDS":
                    {
                        string fieldsJson = context.Request["letterFieldsData"];
                        Int64 letterId = MDVUtility.ToInt64(context.Request["letterId"]);
                        string strJsonData = SaveLetterFields(fieldsJson, letterId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "DELETED_FIELDSIDSFROM_USER":
                    {
                        string deletedFieldsIDsFromLetter = context.Request["deletedFieldsIDsFromLetter"];
                        Int64 letterId = MDVUtility.ToInt64(context.Request["LetterID"]);
                        string strJsonData = DELETED_FieldsIDsFrom_USER(deletedFieldsIDsFromLetter, letterId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SEARCH_LETTER_FIELDS":
                    {
                        Int32 LtrFieldId = MDVUtility.ToInt32(context.Request["LtrFieldId"]);
                        Int32 LetterId = MDVUtility.ToInt32(context.Request["LetterId"]);
                        string strJSONData = LoadLetterFields(LtrFieldId, LetterId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}