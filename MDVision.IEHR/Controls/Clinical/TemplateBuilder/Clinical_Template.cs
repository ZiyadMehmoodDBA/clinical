using System;
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
    public class Clinical_Template
    {
         private BLLClinical BLLClinicalObj = null;
         public Clinical_Template()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton

        private static Clinical_Template _instance;
        public static Clinical_Template Instance()
        {
            return _instance ?? (_instance = new Clinical_Template());
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads the Clinical Template
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="templateId"></param>
        /// <param name="rpp"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private string LoadTempalate(string fieldsJson, Int64 templateId, Int64 rpp, Int64 pageNo)
        {


            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                BLObject<DSTemplateBuilder> objTemplate = searchedfieldsJson == null ? BLLClinicalObj.LoadClinicalTemplate(templateId, null, null, 0, 0, 0, null, rpp, pageNo) : BLLClinicalObj.LoadClinicalTemplate(templateId, searchedfieldsJson["txtShortName"], searchedfieldsJson["txtDescription"], string.IsNullOrEmpty(searchedfieldsJson["ddlTemplateTypeId"]) ? 0 : MDVUtility.ToInt64(searchedfieldsJson["ddlTemplateTypeId"]), string.IsNullOrEmpty(searchedfieldsJson["ddlSpecialityID"]) ? 0 : MDVUtility.ToInt64(searchedfieldsJson["ddlSpecialityID"]), string.IsNullOrEmpty(searchedfieldsJson["ddlProviderId"]) ? 0 : MDVUtility.ToInt64(searchedfieldsJson["ddlProviderId"]), searchedfieldsJson["ddlActive"], rpp, pageNo);

                var dsTemplate = objTemplate.Data;
                if (dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        TemplateCount = dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows.Count,
                        iTotalDisplayRecords = dsTemplate.ClinicalTemplate.Rows[0][dsTemplate.ClinicalTemplate.RecordCountColumn],
                        TemplateLoad_JSON = MDVUtility.JSON_DataTable(dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName]),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        TemplateCount = dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows.Count,
                        iTotalDisplayRecords = 0,
                        TemplateLoad_JSON = MDVUtility.JSON_DataTable(dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName]),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Save the Clinical Template
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <returns></returns>
        private string SaveTemplate(string fieldsJson)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                DSTemplateBuilder dsTemplate = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalTemplateRow dr = dsTemplate.ClinicalTemplate.NewClinicalTemplateRow();

                dr.ShortName = searchedfieldsJson["ShortName"];
                dr.Description = searchedfieldsJson["Description"];
                if (!string.IsNullOrEmpty(searchedfieldsJson["TemplateTypeId"]))
                    dr.TemplateTypeId = MDVUtility.ToInt64(searchedfieldsJson["TemplateTypeId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["LetterId"]))
                    dr.LetterId = MDVUtility.ToInt64(searchedfieldsJson["LetterId"]);

                if (!string.IsNullOrEmpty(searchedfieldsJson["ProviderId"]))
                    dr.ProviderId = MDVUtility.ToInt64(searchedfieldsJson["ProviderId"]);


                if (!string.IsNullOrEmpty(searchedfieldsJson["SpecialityId"]))
                    dr.SpecialityId = MDVUtility.ToInt64(searchedfieldsJson["SpecialityId"]);


                if (!string.IsNullOrEmpty(searchedfieldsJson["ClinicalTemplateId"]))
                    dr.TemplateId = MDVUtility.ToInt64(!string.IsNullOrEmpty(searchedfieldsJson["ClinicalTemplateId"]));

                if (!string.IsNullOrEmpty(searchedfieldsJson["FolderId"]))
                    dr.FolderId = MDVUtility.ToInt64(searchedfieldsJson["FolderId"]);


                dr.IsActive = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True";

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;


                #region Database Insertion
                dsTemplate.ClinicalTemplate.AddClinicalTemplateRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertTemplate(dsTemplate);
                dsTemplate = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        ClinicalTemplateId = dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows[0][dsTemplate.ClinicalTemplate.TemplateIdColumn.ColumnName],
                        TemplateDescription = dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows[0][dsTemplate.ClinicalTemplate.DescriptionColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    string message = obj.Message;
                    if (message.Contains("UNIQUE KEY"))
                        message = "Cannot insert duplicate Template.";

                    var response = new
                    {
                        status = false,
                        Message = message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Update the Clincial Template
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="templateId"></param>
        /// <param name="htmlTemplteTemplate"></param>
        /// <returns></returns>
        private string UpdateTemplate(string fieldsJson, Int64 templateId, string htmlTemplteTemplate)
        {

            try
            {
                var obj = BLLClinicalObj.LoadClinicalTemplate(templateId, null, null, 0, 0, 0, null, 1, 1);
                var dsTemplate = obj.Data;
                if (dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(fieldsJson) && !fieldsJson.Equals("null"))
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                        foreach (DataRow dr in dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows)
                        {
                            if (searchedfieldsJson.ContainsKey("ShortName") && !string.IsNullOrEmpty(searchedfieldsJson["ShortName"]))
                                dr[dsTemplate.ClinicalTemplate.ShortNameColumn] = MDVUtility.ToStr(searchedfieldsJson["ShortName"]);

                            if (searchedfieldsJson.ContainsKey("Description") && !string.IsNullOrEmpty(searchedfieldsJson["Description"]))
                                dr[dsTemplate.ClinicalTemplate.DescriptionColumn] = MDVUtility.ToStr(searchedfieldsJson["Description"]);

                            if (searchedfieldsJson.ContainsKey("SpecialityId") && !string.IsNullOrEmpty(searchedfieldsJson["SpecialityId"]))
                                dr[dsTemplate.ClinicalTemplate.SpecialityIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["SpecialityId"]);

                            if (searchedfieldsJson.ContainsKey("TemplateTypeId") && !string.IsNullOrEmpty(searchedfieldsJson["TemplateTypeId"]))
                                dr[dsTemplate.ClinicalTemplate.TemplateTypeIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["TemplateTypeId"]);

                            if (searchedfieldsJson.ContainsKey("Active"))
                                dr[dsTemplate.ClinicalTemplate.IsActiveColumn] = MDVUtility.ToStr(searchedfieldsJson["Active"]) == "True";

                            if (searchedfieldsJson.ContainsKey("LetterId") && !string.IsNullOrEmpty(searchedfieldsJson["LetterId"]))
                                dr[dsTemplate.ClinicalTemplate.LetterIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["LetterId"]);

                            if (searchedfieldsJson.ContainsKey("ProviderId") && !string.IsNullOrEmpty(searchedfieldsJson["ProviderId"]))
                                dr[dsTemplate.ClinicalTemplate.ProviderIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["ProviderId"]);

                            //if (searchedfieldsJson.ContainsKey("SpecialityId") && !string.IsNullOrEmpty(searchedfieldsJson["SpecialityId"]))
                            //    dr[dsTemplate.ClinicalTemplate.SpecialityIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["SpecialityId"]);

                            if (searchedfieldsJson.ContainsKey("FolderId") && !string.IsNullOrEmpty(searchedfieldsJson["FolderId"]))
                                dr[dsTemplate.ClinicalTemplate.FolderIdColumn] = MDVUtility.ToInt64(searchedfieldsJson["FolderId"]);

                            dr[dsTemplate.ClinicalTemplate.TemplateIdColumn] = templateId;//Utility.ToInt64(searchedfieldsJson["ClinicalTemplateId"]);

                            if (!string.IsNullOrEmpty(htmlTemplteTemplate))
                                dr[dsTemplate.ClinicalTemplate.HTMLTemplateColumn] = htmlTemplteTemplate;

                            dr[dsTemplate.ClinicalTemplate.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsTemplate.ClinicalTemplate.ModifiedOnColumn] = DateTime.Now;

                        }
                    }
                    else
                    {

                        foreach (DataRow dr in dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName].Rows)
                        {
                            if (!string.IsNullOrEmpty(htmlTemplteTemplate))
                                dr[dsTemplate.ClinicalTemplate.HTMLTemplateColumn] = htmlTemplteTemplate;

                            dr[dsTemplate.ClinicalTemplate.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsTemplate.ClinicalTemplate.ModifiedOnColumn] = DateTime.Now;
                        }

                    }

                    var objTemplate = BLLClinicalObj.UpdateTemplate(ref dsTemplate);

                    if (objTemplate.Data != null)
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
                            Message = objTemplate.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fill Template
        /// </summary>
        /// <param name="sectionsDetailsJson"></param>
        /// <param name="clinicalTemplateId"></param>
        /// <param name="rpp"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private string FillTempalate(string sectionsDetailsJson, Int64 clinicalTemplateId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                var objTemplate = BLLClinicalObj.LoadClinicalTemplate(clinicalTemplateId, null, null, 0, 0, 0, null, rpp, pageNo);

                var dsTemplate = objTemplate.Data;
                BLObject<DSTemplateBuilder> objSectionDetails = new BLObject<DSTemplateBuilder>();
                BLObject<DSTemplateBuilder> objTemplateSectionDetails = new BLObject<DSTemplateBuilder>();
                if (!string.IsNullOrEmpty(sectionsDetailsJson))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();

                    var searchedfieldsJson = ser.Deserialize<dynamic>(sectionsDetailsJson);
                    if (searchedfieldsJson != null)
                    {
                        objSectionDetails = BLLClinicalObj.LoadClinicalSection(-1, searchedfieldsJson["SearchSectiontxt"], searchedfieldsJson["SearchSectiontxt"], searchedfieldsJson["SearchSectiontxt"], null, null, null, string.IsNullOrEmpty(searchedfieldsJson["PageNoSection"]) ? 0 : MDVUtility.ToInt64(searchedfieldsJson["PageNoSection"]), string.IsNullOrEmpty(searchedfieldsJson["rppSection"]) ? 0 : MDVUtility.ToInt64(searchedfieldsJson["rppSection"]));
                    }
                    if (clinicalTemplateId > 0)
                    {
                        objTemplateSectionDetails = BLLClinicalObj.LoadTemplateSection(0, clinicalTemplateId);

                    }
                }

                var dsSection = objSectionDetails.Data;
                var dsTemplateSection = objTemplateSectionDetails.Data;

                var response = new
                {
                    status = true,
                    TemplateCount = dsTemplate.ClinicalTemplate.Rows[0][dsTemplate.ClinicalTemplate.RecordCountColumn],
                    iTotalDisplayRecords = dsTemplate.ClinicalTemplate.Rows[0][dsTemplate.ClinicalTemplate.RecordCountColumn],
                    TemplateLoad_JSON = MDVUtility.JSON_DataTable(dsTemplate.Tables[dsTemplate.ClinicalTemplate.TableName]),
                    //Sections List Load
                    SectionCount = dsSection != null ? dsSection.Tables[dsSection.ClinicalSection.TableName].Rows.Count : 0,
                    iTotalDisplayRecords_Sections = (dsSection != null && dsSection.ClinicalSection.Rows.Count > 0) ? dsSection.ClinicalSection.Rows[0][dsSection.ClinicalSection.RecordCountColumn] : 0,
                    SectionLoad_JSON = dsSection != null ? MDVUtility.JSON_DataTable(dsSection.Tables[dsSection.ClinicalSection.TableName]) : "",


                    TemplateSectionCounTemplateSectionLoad_JSONt = dsTemplateSection != null ? dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName].Rows.Count : 0,
                    TemplateSectionLoad_JSON = dsTemplateSection != null ? MDVUtility.JSON_DataTable(dsTemplateSection.Tables[dsSection.ClinicalTemplateSection.TableName]) : "",
                    //TemplateSectionId = dsTemplateSection != null ? dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName].Rows[0][dsTemplateSection.ClinicalTemplateSection.TemplateSectionIdColumn.ColumnName] : -1
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }

        /// <summary>
        /// Delete the Template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        private string DeleteTemplate(Int64 templateId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(templateId)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteTemplate(MDVUtility.ToStr(templateId));
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Update Template Active Inactive Status
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="isActive"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        private string UpdateTemplateIsActive(Int64 templateId, Int64 isActive, Int64 pageNo, Int64 rpp)
        {
            try
            {
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalTemplate(templateId, null, null, 0, 0, 0, null, 1, 1);
                var dsTEmplate = obj.Data;
                if (dsTEmplate.Tables[dsTEmplate.ClinicalTemplate.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsTEmplate.Tables[dsTEmplate.ClinicalTemplate.TableName].Rows[0];
                    dr[dsTEmplate.ClinicalTemplate.IsActiveColumn.ColumnName] = isActive;

                    BLObject<DSTemplateBuilder> objUser = BLLClinicalObj.UpdateTemplate(ref dsTEmplate);
                    if (objUser.Data != null)
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
                            Message = objUser.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }

        /// <summary>
        /// Put All Sections in TemplateSection as it is Dropped on Canvas
        /// </summary>
        /// <param name="clinicalTemplateId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        private string SaveSectionsInTemplate(Int64 clinicalTemplateId, Int64 sectionId)
        {
            try
            {
                //JavaScriptSerializer ser = new JavaScriptSerializer();
                DSTemplateBuilder dsTemplateSection = new DSTemplateBuilder();
                DSTemplateBuilder.ClinicalTemplateSectionRow dr = dsTemplateSection.ClinicalTemplateSection.NewClinicalTemplateSectionRow();
                dr.TemplateSectionId = -1;
                dr.TemplateID = clinicalTemplateId;
                dr.SectionID = sectionId;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsTemplateSection.ClinicalTemplateSection.AddClinicalTemplateSectionRow(dr);
                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.InsertTemplateSection(dsTemplateSection);
                dsTemplateSection = obj.Data;
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        //QuestionLoad_JSON = searchedfieldsJson["QuestionLoad_JSON"],
                        TemplateSectionId = dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName].Rows[0][dsTemplateSection.ClinicalTemplateSection.TemplateSectionIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    string message = obj.Message;
                    if (message.Contains("UNIQUE KEY"))
                        message = "Cannot insert duplicate TEmplate.";

                    var response = new
                    {
                        status = false,
                        Message = message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Delete Section From TemplateSection
        /// </summary>
        /// <param name="templateSectionId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        private string Delete_Section_From_Template(Int64 templateSectionId, Int64 sectionId)
        {
            try
            {

                BLObject<string> obj = BLLClinicalObj.DeleteTemplateSection(templateSectionId, sectionId);
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Load the Sections
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="sectionId"></param>
        /// <param name="rpp"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private string SearchSeaction(string fieldsJson, Int64 sectionId, Int64 rpp, Int64 pageNo)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                var shortName = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];
                var description = searchedfieldsJson["TextSearched"] == "" ? null : searchedfieldsJson["TextSearched"];

                BLObject<DSTemplateBuilder> obj = BLLClinicalObj.LoadClinicalSection(sectionId, shortName, description, null, null, null, null, pageNo, rpp);

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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string LoadTemplateSection(string fieldsJSON, Int64 templateSectionId, Int64 templateId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSTemplateBuilder dsTemplateSection = null;
                BLObject<DSTemplateBuilder> obj;
                obj = BLLClinicalObj.LoadTemplateSection(templateSectionId, templateId);

                dsTemplateSection = obj.Data;
                if (obj.Data != null)
                {
                    if (dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TemplateSectionCount = dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName].Rows.Count,
                            TemplateFill_JSON = MDVUtility.JSON_DataTable(dsTemplateSection.Tables[dsTemplateSection.ClinicalTemplateSection.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            TemplateSectionCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
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
                #region Template

                case "SEARCH_TEMPLATE":
                    {
                        string fieldsJson = context.Request["TemplateFormData"];
                        Int64 templateId = MDVUtility.ToInt64(context.Request["TemplateID"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = LoadTempalate(fieldsJson, templateId, rpp, pageNo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_TEMPLATE":
                    {

                        string sectionsDetailsJson = context.Request["SectionsDetailsJson"];
                        Int64 clinicalTemplateId = MDVUtility.ToInt64(context.Request["ClinicalTemplateId"]);

                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = FillTempalate(sectionsDetailsJson, clinicalTemplateId, rpp, pageNo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_TEMPLATE_GROUP":
                    {
                        string fieldsJson = context.Request["ClinicalTemplateData"];
                        string strJsonData = SaveTemplate(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_TEMPLATE_GROUP":
                    {
                        string clinicalTemplateData = context.Request["ClinicalTemplateData"];
                        string htmlTempalteTemplate = context.Request["HTMLTempalteTemplate"];
                        string updatedHtmlTempalteforTemplate = context.Request["UpdatedHTMLforTemplate"];
                        Int64 clinicalTemplateId = MDVUtility.ToInt64(context.Request["ClinicalTemplateId"]);
                        if (htmlTempalteTemplate == "undefined")
                        {
                            htmlTempalteTemplate = updatedHtmlTempalteforTemplate;
                        }
                        string strJsonData = UpdateTemplate(clinicalTemplateData, clinicalTemplateId, htmlTempalteTemplate);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_TEMPLATE":
                    {
                        string templateId = context.Request["TemplateID"];
                        string strJsonData = DeleteTemplate(MDVUtility.ToInt64(templateId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_TEMPLATE_ACTIVE_INACTIVE":
                    {
                        Int64 templateId = MDVUtility.ToInt64(context.Request["TemplateID"]);
                        Int64 isActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = UpdateTemplateIsActive(templateId, isActive, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                #endregion

                #region Template Sections


                case "INSERT_SECTION_IN_TEMPLATE":
                    {
                        Int64 sectionId = MDVUtility.ToInt64(context.Request["SectionId"]);
                        Int64 clinicalTemplateId = MDVUtility.ToInt64(context.Request["ClinicalTemplateId"]);

                        string strJsonData = SaveSectionsInTemplate(clinicalTemplateId, sectionId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "DELETE_SECTIONFRMTEMPLATE":
                    {
                        string strJsonData = Delete_Section_From_Template(MDVUtility.ToInt64(context.Request["TemplateSectionId"]), MDVUtility.ToInt64(context.Request["TemplateID"]));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "GET_SECTIONS_OF_TEMPLATE":
                    {
                        string fieldsJSON = context.Request["fieldsJSON"];
                        Int64 TemplateId = MDVUtility.ToInt64(context.Request["TemplateId"]);
                        Int64 TemplateSectionId = MDVUtility.ToInt64(context.Request["TemplateSectionId"]);
                        string strJSONData = LoadTemplateSection(fieldsJSON, TemplateSectionId, TemplateId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                #endregion

                #region Section

                case "SEARCH_SECTION":
                    {
                        string fieldsJson = context.Request["SectionData"];
                        Int64 sectionId = MDVUtility.ToInt64(context.Request["sectionID"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJsonData = SearchSeaction(fieldsJson, sectionId, rpp, pageNo);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                #endregion
            }
        }
        #endregion
    }
}