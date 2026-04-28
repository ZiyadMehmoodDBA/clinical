using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.Templates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class ProviderNoteTemplateHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public ProviderNoteTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ProviderNoteTemplateHelper _instance = null;
        public static ProviderNoteTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new ProviderNoteTemplateHelper();
            return _instance;
        }

        #region Load ROS Templates
        public string loadNoteTemplate(ProviderNoteTemplateModel model)
        {
            try
            {
                DSClinicalNoteTemplate dsNotesTemplate = null;
                BLObject<DSClinicalNoteTemplate> obj;
                if (model.EntityId == null)
                {
                    model.EntityId = MDVUtility.ToInt32(MDVSession.Current.EntityId);
                }
                obj = BLLClinicalObj.loadNoteTemplate(model.NotesTemplateId, model.NoteTemplateName, model.ProviderIds, model.SpecialtyIds, model.EntityId, model.TemplateTypeId, model.IsActive, model.PageNumber, model.RowsPerPage);

                dsNotesTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows.Count > 0)
                    {
                        List<ProviderNoteTemplateModel> mdlist = new List<ProviderNoteTemplateModel>();
                        foreach (DataRow dr in dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows)
                        {
                            model = new ProviderNoteTemplateModel();
                            model.NotesTemplateId = MDVUtility.ToLong(dr[dsNotesTemplate.NotesTemplate.NotesTemplateIdColumn.ColumnName]);
                            model.NoteTemplateName = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.NoteTemplateNameColumn.ColumnName]);
                            model.SpecialtyNames = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.SpecialtyNamesColumn.ColumnName]);
                            model.TemplateTypeId = MDVUtility.ToInt32(MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.TemplateTypeIdColumn.ColumnName]));
                            model.TemplateType = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.TemplateTypeNameColumn.ColumnName]);
                            model.ModifiedBy = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.ModifiedByColumn.ColumnName]);
                            model.ModifiedOn = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.ModifiedOnColumn.ColumnName]);
                            model.SpecialtyIds = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.SpecialtyIdsColumn.ColumnName]);
                            model.ProviderIds = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.ProviderIdsColumn.ColumnName]);
                            model.EntityId = MDVUtility.ToInt32(dr[dsNotesTemplate.NotesTemplate.EntityIdColumn.ColumnName]);
                            model.IsActive = Convert.ToBoolean(MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.IsActiveColumn.ColumnName])) ? 1 : 0;
                            //Start || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template
                            string ROSDataTemptId = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.ROSDataTemptIdColumn.ColumnName]);
                            string PEDataTemptId = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.PEDataTemptIdColumn.ColumnName]);
                            model.PEDataTemptId = string.IsNullOrEmpty(PEDataTemptId) ? -1 : MDVUtility.ToInt64(PEDataTemptId);
                            model.ROSDataTemptId = string.IsNullOrEmpty(ROSDataTemptId) ? -1 : MDVUtility.ToInt64(ROSDataTemptId);
                            string HPITemplateId = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.HPITemplateIdColumn.ColumnName]);
                            model.HPITemplateId = string.IsNullOrEmpty(HPITemplateId) ? -1 : MDVUtility.ToInt64(HPITemplateId);
                            model.CreatedByName = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.CreatedByNameColumn.ColumnName]);
                            model.ModifiedByName = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.ModifiedByNameColumn.ColumnName]);
                            model.OrderSetId = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.OrderSetIdColumn.ColumnName]);
                            //End   || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template
                            String HtmlDocument = MDVUtility.ToStr(dr[dsNotesTemplate.NotesTemplate.HTMLTemplateColumn.ColumnName]);
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

                            model.HTMLTemplate = HtmlDocument;
                            mdlist.Add(model);
                        }




                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            NotesTemplateCount = dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows.Count,
                            iTotalDisplayRecords = dsNotesTemplate.NotesTemplate.Rows[0][dsNotesTemplate.NotesTemplate.RecordCountColumn.ColumnName],
                            NotesTemplateLoad_JSON = js.Serialize(mdlist),// MDVUtility.JSON_DataTable(dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            NotesTemplateCount = 0,
                            iTotalDisplayRecords = 0,
                            NotesTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        NotesTemplateCount = 0,
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
        #endregion

        #region insert/delete/update
        public string updateClinical_NotesTemplateIsActive(Int64 templateID, int? IsActive, int? EntityId)
        {
            try
            {
                if (templateID > 0)
                {

                    DSClinicalNoteTemplate dsNotesTemplate = null;
                    BLObject<DSClinicalNoteTemplate> obj = BLLClinicalObj.loadNoteTemplate(templateID, null, null, null, EntityId, 0, null, 1, 1);
                    dsNotesTemplate = obj.Data;
                    if (dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows[0];
                        dr[dsNotesTemplate.NotesTemplate.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSClinicalNoteTemplate> objNotes = BLLClinicalObj.updateNotesTemplate(dsNotesTemplate);
                        string successMsg;
                        if (objNotes.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objNotes.Message
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "ROS Template not found."
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
        /// Deletes the Clinical Notes.
        /// </summary>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string deleteClinical_NotesTemplate(long templateID)
        {
            try
            {
                if (templateID < 0)
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
                    BLObject<string> obj = BLLClinicalObj.deleteNotesTemplate(templateID);
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

        public string updateNotesTemplateInfo(ProviderNoteTemplateModel model)
        {

            try
            {
                if (model.NotesTemplateId > 0)
                {
                    int? EntityId = model.EntityId;
                    if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToLower().Equals("mdvision"))
                    {
                        EntityId = null;
                    }
                    DSClinicalNoteTemplate dsNotesTemplate = null;
                    BLObject<DSClinicalNoteTemplate> obj = BLLClinicalObj.loadNoteTemplate(model.NotesTemplateId, null, null, null, EntityId, 0, null, 1, 1);
                    dsNotesTemplate = obj.Data;
                    if (dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsNotesTemplate.Tables[dsNotesTemplate.NotesTemplate.TableName].Rows[0];
                        dr[dsNotesTemplate.NotesTemplate.NotesTemplateIdColumn.ColumnName] = model.NotesTemplateId;
                        dr[dsNotesTemplate.NotesTemplate.NotesTagNameIdsColumn.ColumnName] = model.NotesTagNameIds;
                        //dr[dsNotesTemplate.NotesTemplate.PEDataTemptIdColumn.ColumnName] = model.PEDataTemptId;
                        //Start || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template
                        if (model.ROSDataTemptId <= 0)
                        {
                            dr[dsNotesTemplate.NotesTemplate.ROSDataTemptIdColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsNotesTemplate.NotesTemplate.ROSDataTemptIdColumn.ColumnName] = model.ROSDataTemptId;
                        }
                        if (model.PEDataTemptId <= 0)
                        {
                            dr[dsNotesTemplate.NotesTemplate.PEDataTemptIdColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsNotesTemplate.NotesTemplate.PEDataTemptIdColumn.ColumnName] = model.PEDataTemptId;
                        }
                        if (model.HPITemplateId <= 0)
                        {
                            dr[dsNotesTemplate.NotesTemplate.HPITemplateIdColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsNotesTemplate.NotesTemplate.HPITemplateIdColumn.ColumnName] = model.HPITemplateId;
                        }
                        //End   || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template

                        //Start || 14 July, 2016 || ZeeshanAK || Fix for EMR-1525
                        //dr[dsNotesTemplate.NotesTemplate.ROSDataTemptIdColumn.ColumnName] = model.ROSDataTemptId;
                        //End   || 14 July, 2016 || ZeeshanAK || Fix for EMR-1525

                        dr[dsNotesTemplate.NotesTemplate.TemplateTypeIdColumn.ColumnName] = model.TemplateTypeId;
                        dr[dsNotesTemplate.NotesTemplate.HTMLTemplateColumn.ColumnName] = model.HTMLTemplate;
                        dr[dsNotesTemplate.NotesTemplate.ProviderIdsColumn.ColumnName] = model.ProviderIds;
                        dr[dsNotesTemplate.NotesTemplate.SpecialtyIdsColumn.ColumnName] = model.SpecialtyIds;
                        dr[dsNotesTemplate.NotesTemplate.NoteTemplateNameColumn.ColumnName] = model.NoteTemplateName;
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToLower().Equals("mdvision"))
                        {
                            dr[dsNotesTemplate.NotesTemplate.EntityIdColumn.ColumnName] = model.EntityId;
                        }
                        else
                        {
                            dr[dsNotesTemplate.NotesTemplate.EntityIdColumn.ColumnName] = MDVUtility.ToLong(MDVSession.Current.EntityId);
                        }
                        dr[dsNotesTemplate.NotesTemplate.IsActiveColumn.ColumnName] = model.IsActive;
                        dr[dsNotesTemplate.NotesTemplate.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsNotesTemplate.NotesTemplate.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        //Start 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289
                        if (MDVUtility.ToInt64(model.OrderSetId) <= 0)
                        {
                            dr[dsNotesTemplate.NotesTemplate.OrderSetIdColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsNotesTemplate.NotesTemplate.OrderSetIdColumn.ColumnName] = model.OrderSetId;
                        }
                        //End 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289                    
                        BLObject<DSClinicalNoteTemplate> objNotes = BLLClinicalObj.updateNotesTemplate(dsNotesTemplate);

                        long notesTemplateId = 0;
                        if (objNotes.Data != null)
                            notesTemplateId = MDVUtility.ToInt64(objNotes.Data.Tables[objNotes.Data.NotesTemplate.TableName].Rows[0][objNotes.Data.NotesTemplate.NotesTemplateIdColumn.ColumnName]);

                        if (objNotes.Data != null && notesTemplateId != -1)
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
                                Message = notesTemplateId == -1 ? "A Template with this name already exists. Try a different name." : obj.Message
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "ROS Template not found."
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


        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/16/2016
        //OverView: Methods "SaveProviderNoteTemplate" for Call BLL for Save Provider Note Template
        public string SaveProviderNoteTemplate(ProviderNoteTemplateModel model)
        {
            try
            {
                DSClinicalNoteTemplate dsLetter = new DSClinicalNoteTemplate();
                DSClinicalNoteTemplate.NotesTemplateRow dr = dsLetter.NotesTemplate.NewNotesTemplateRow();
                dr.NotesTemplateId = -1;
                dr.NoteTemplateName = model.NoteTemplateName;
                dr.TemplateTypeId = model.TemplateTypeId;
                dr.SpecialtyIds = model.SpecialtyIds;
                dr.IsActive = model.IsActive == 1 ? true : false;
                dr.ProviderIds = model.ProviderIds;
                //Start 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289            
                if (MDVUtility.ToInt64(model.OrderSetId) <= 0)
                {
                    dr[dsLetter.NotesTemplate.OrderSetIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr[dsLetter.NotesTemplate.OrderSetIdColumn.ColumnName] = model.OrderSetId;
                }
                //End 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289
                //Start || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template
                if (model.ROSDataTemptId <= 0)
                {
                    dr[dsLetter.NotesTemplate.ROSDataTemptIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr.ROSDataTemptId = model.ROSDataTemptId;
                }
                //End   || 20 June, 2016 || ZeeshanAK || Changes for ROS Data Template
                if (model.PEDataTemptId <= 0)
                {
                    dr[dsLetter.NotesTemplate.PEDataTemptIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr.PEDataTemptId = model.PEDataTemptId;
                }

                if (MDVUtility.ToInt64(model.OrderSetId) <= 0)
                {
                    dr[dsLetter.NotesTemplate.OrderSetIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr[dsLetter.NotesTemplate.OrderSetIdColumn.ColumnName] = model.OrderSetId;
                }

                if (model.HPITemplateId <= 0)
                {
                    dr[dsLetter.NotesTemplate.HPITemplateIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr.HPITemplateId = model.HPITemplateId;
                }
                dr.EntityId = MDVUtility.ToInt32(model.EntityId != null ? model.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));
                dr.NotesTagNameIds = model.NotesTagNameIds;
                dr.HTMLTemplate = model.HTMLTemplate;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsLetter.NotesTemplate.AddNotesTemplateRow(dr);


                #region Database Insertion
                BLObject<DSClinicalNoteTemplate> obj = BLLClinicalObj.insertNotesTemplate(dsLetter);
                dsLetter = obj.Data;
                DataRow dr1 = dsLetter.Tables[dsLetter.NotesTemplate.TableName].Rows[0];
                String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.NotesTemplate.HTMLTemplateColumn.ColumnName]);
                long notesTemplateId = 0;
                if (obj.Data != null)
                    notesTemplateId = MDVUtility.ToInt64(dsLetter.Tables[dsLetter.NotesTemplate.TableName].Rows[0][dsLetter.NotesTemplate.NotesTemplateIdColumn.ColumnName]);

                if (obj.Data != null && notesTemplateId != -1)
                {
                    var response = new
                    {
                        NotesTemplateId = dsLetter.Tables[dsLetter.NotesTemplate.TableName].Rows[0][dsLetter.NotesTemplate.NotesTemplateIdColumn.ColumnName],
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
                        Message = notesTemplateId == -1 ? "A Template with this name already exists. Try a different name." : obj.Message
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

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/16/2016
        //OverView: Methods "InsertNewNoteType" for Call BLL for Save New Note Type
        public string InsertNewNoteType(string NewNoteType)
        {
            try
            {
                DSClinicalNoteTemplateLookup dsNoteType = new DSClinicalNoteTemplateLookup();
                DSClinicalNoteTemplateLookup.NotesTemplateTypeRow dr = dsNoteType.NotesTemplateType.NewNotesTemplateTypeRow();
                dr.NotesTemplateTypeId = -1;
                dr.ShortName = NewNoteType;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsNoteType.NotesTemplateType.AddNotesTemplateTypeRow(dr);


                #region Database Insertion
                BLObject<DSClinicalNoteTemplateLookup> obj = BLLClinicalObj.InsertNewNoteType(dsNoteType);
                dsNoteType = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        NotesTemplateId = dsNoteType.Tables[dsNoteType.NotesTemplateType.TableName].Rows[0][dsNoteType.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}