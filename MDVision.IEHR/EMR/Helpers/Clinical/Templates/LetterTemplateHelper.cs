/* Author:  Khaleel Ur Rehman
 * Created Date: 02/03/2016
 * OverView: Created to handel Letter Template
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using MDVision.IEHR.EMR.Model.Templates;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class LetterTemplateHelper
    {
        private BLLAdmin BLLAdminObj = null;
        public LetterTemplateHelper()
        {
            BLLAdminObj = new BLLAdmin();
        }

        public string loadLetterTemplates(Int64 letterId, bool isActive, string name, string description, int categoryId, Int32 pageNumber, Int32 rowsPerPage)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj;
                obj = BLLAdminObj.loadLetterTemplates(letterId, isActive, name, description, categoryId, pageNumber, rowsPerPage);
                dsLetter = obj.Data;
                if (obj.Data != null)
                {
                    if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            LetterTemplatesCount = dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count,
                            iTotalDisplayRecords = dsLetter.Letter.Rows[0][dsLetter.Letter.RecordCountColumn.ColumnName],
                            LetterTemplatesLoad_JSON = MDVUtility.JSON_DataTable(dsLetter.Tables[dsLetter.Letter.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            LetterTemplatesCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/03/2016
        //OverView: Methods "SaveTemplateLetter" for Call BLL for Save Template letter Data
        public string SaveTemplateLetter(LetterTemplateModel model)
        {
            try
            {
                DSLetter dsLetter = new DSLetter();
                DSLetter.LetterRow dr = dsLetter.Letter.NewLetterRow();
                dr.TemplateLetterId = -1;
                dr.Name = model.Name;
                dr.Description = model.Description;
                dr.CategoryId = model.CategoryId;
                dr.IsActive = model.IsActive;
                dr.TemplateContent = MDVUtility.ToStr(model.TemplateContent);
                dr.TagIds = model.TagIds;
                if (model.EntityId == -1)
                {

                }
                else
                {
                    dr.EntityId = model.EntityId;
                }

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
                dr.SpecialtyIds = model.SpecialtyIds;
                dr.ProviderIds = model.ProviderIds;
                //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
                dsLetter.Letter.AddLetterRow(dr);


                #region Database Insertion
                BLObject<DSLetter> obj = BLLAdminObj.InsertTemplateLetter(dsLetter);
                dsLetter = obj.Data;
                DataRow dr1 = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0];
                String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.Letter.TemplateContentColumn.ColumnName]);

                long templateLetterId = 0;
                if (obj.Data != null)
                    templateLetterId = MDVUtility.ToInt64(dsLetter.Tables[dsLetter.Letter.TableName].Rows[0][dsLetter.Letter.TemplateLetterIdColumn.ColumnName]);

                if (obj.Data != null && templateLetterId != -1)
                {
                    var response = new
                    {
                        TemplateLetterId = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0][dsLetter.Letter.TemplateLetterIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = templateLetterId == -1 ? "A Template with this name already exists. Try a different name." : obj.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/03/2016
        //OverView: Methods "UpdateTemplateLetter" for Call BLL for Update Template letter Data
        public string UpdateTemplateLetter(LetterTemplateModel model)
        {
            try
            {



                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-515
                obj = BLLAdminObj.loadLetterTemplates(model.TemplateLetterId, null, "", "", MDVUtility.ToInt32(""), model.PageNumber, model.RowsPerPage);
                // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-515
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0];

                    dr[dsLetter.Letter.TemplateLetterIdColumn.ColumnName] = model.TemplateLetterId;
                    dr[dsLetter.Letter.NameColumn.ColumnName] = model.Name;
                    dr[dsLetter.Letter.DescriptionColumn.ColumnName] = model.Description;
                    dr[dsLetter.Letter.CategoryIdColumn.ColumnName] = model.CategoryId;
                    dr[dsLetter.Letter.IsActiveColumn.ColumnName] = model.IsActive;
                    dr[dsLetter.Letter.TemplateContentColumn.ColumnName] = model.TemplateContent;
                    dr[dsLetter.Letter.TagIdsColumn.ColumnName] = model.TagIds;
                    if (model.EntityId == -1)
                    {

                    }
                    else
                    {
                        dr[dsLetter.Letter.EntityIdColumn.ColumnName] = model.EntityId;
                    }

                    dr[dsLetter.Letter.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsLetter.Letter.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
                    dr[dsLetter.Letter.SpecialtyIdsColumn.ColumnName] = model.SpecialtyIds;
                    dr[dsLetter.Letter.ProviderIdsColumn.ColumnName] = model.ProviderIds;
                    //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
                }

                #region Database Insertion
                obj = BLLAdminObj.UpdateTemplateLetter(dsLetter);

                long templateLetterId = 0;
                if (obj.Data != null)
                    templateLetterId = MDVUtility.ToInt64(obj.Data.Tables[dsLetter.Letter.TableName].Rows[0][dsLetter.Letter.TemplateLetterIdColumn.ColumnName]);

                if (obj.Data != null && templateLetterId != -1)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = templateLetterId == -1 ? "A Template with this name already exists. Try a different name." : obj.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/03/2016
        //OverView: Methods "ActiveInactiveTemplateLetter" for Call BLL for Active/Inactive Template letter Data
        public string ActiveInactiveTemplateLetter(LetterTemplateModel model)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                obj = BLLAdminObj.loadLetterTemplates(model.TemplateLetterId, (model.IsActive == false ? true : false), model.Name, model.Description, MDVUtility.ToInt32(""), model.PageNumber, model.RowsPerPage);
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0];
                    dr[dsLetter.Letter.IsActiveColumn.ColumnName] = model.IsActive;
                }
                #region Database Insertion
                obj = BLLAdminObj.UpdateTemplateLetter(dsLetter);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        public string deleteLetterTemplate(long letterId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(letterId)))
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
                    BLObject<string> obj = BLLAdminObj.deleteLetterTemplate(MDVUtility.ToStr(letterId));
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

        public string GetTemplateLetterData(long LetterId)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj;
                obj = BLLAdminObj.GetTemplateLetterData(LetterId);
                dsLetter = obj.Data;
                if (obj.Data != null)
                {
                    if (dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsLetter.Tables[dsLetter.Letter.TableName].Rows[0];
                        String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.Letter.TemplateContentColumn.ColumnName]);
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
                            { "Name", MDVUtility.ToStr(dr[dsLetter.Letter.NameColumn.ColumnName])},
                            { "Description", MDVUtility.ToStr(dr[dsLetter.Letter.DescriptionColumn.ColumnName])},
                            { "CategoryId", MDVUtility.ToStr(dr[dsLetter.Letter.CategoryIdColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsLetter.Letter.IsActiveColumn.ColumnName])},
                            { "EntityId", MDVUtility.ToStr(dr[dsLetter.Letter.EntityIdColumn.ColumnName])},
                            { "elm1",HtmlDocument},
                            { "ProviderIds", MDVUtility.ToStr(dr[dsLetter.Letter.ProviderIdsColumn.ColumnName])},
                            { "SpecialtyIds", MDVUtility.ToStr(dr[dsLetter.Letter.SpecialtyIdsColumn.ColumnName])},
                        };
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            LetterTemplatesCount = dsLetter.Tables[dsLetter.Letter.TableName].Rows.Count,
                            LetterTemplatesLoad_JSON = js.Serialize(keyValues)
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            LetterTemplatesCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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

        internal string getPatLettersForSoap(string patletterIds, long patientId)
        {
            try
            {
                DSLetter dsPatLetterSoap = null;
                BLObject<DSLetter> obj = BLLAdminObj.LoadPatLettersForSoap(patletterIds, patientId);
                dsPatLetterSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatLetterSoap.Tables[dsPatLetterSoap.PatientTemplateLetter.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatLetterSoapCount = dsPatLetterSoap.Tables[dsPatLetterSoap.PatientTemplateLetter.TableName].Rows.Count,
                            PatLetterSoap_JSON = MDVUtility.JSON_DataTable(dsPatLetterSoap.Tables[dsPatLetterSoap.PatientTemplateLetter.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatLetterSoapCount = 0,
                            PatLetterSoap_JSON = MDVUtility.JSON_DataTable(dsPatLetterSoap.Tables[dsPatLetterSoap.PatientTemplateLetter.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        internal string attachPatLetterWithNote(string patletterIds, long notesId)
        {
            try
            {
                DSLetter dsPatLetterSoap = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(patletterIds)) || MDVUtility.ToInt64(notesId) <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSLetter> obj = BLLAdminObj.AttachPatLettersWithNotes(patletterIds, notesId);
                    if (obj.Data != null)
                    {
                        dsPatLetterSoap = obj.Data;
                        var response = new
                        {
                            status = true,
                            PatLetterSoapCount = dsPatLetterSoap.Tables[dsPatLetterSoap.Letter.TableName].Rows.Count,
                            PatLetterSoap_JSON = MDVUtility.JSON_DataTable(dsPatLetterSoap.Tables[dsPatLetterSoap.Letter.TableName]),
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
        internal string detachLabOrderFromNotes(string patletterIds, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(patletterIds)) || MDVUtility.ToInt64(notesId) <= 0)
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
                    BLObject<string> obj = BLLAdminObj.DetachPatLettersWithNotes(patletterIds, notesId);
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
    }
}