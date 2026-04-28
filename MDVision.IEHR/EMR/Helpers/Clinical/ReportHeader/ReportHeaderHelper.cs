using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.ReportHeader;
using MDVision.Model.Patient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader
{
    public class ReportHeaderHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminClinical BLLAdminClinicalObj = null;
        public ReportHeaderHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLAdminClinicalObj = new BLLAdminClinical();
        }
        private static ReportHeaderHelper _instance = null;
        public static ReportHeaderHelper Instance()
        {
            if (_instance == null)
                _instance = new ReportHeaderHelper();
            return _instance;
        }

        internal string seachReportHeader(ReportHeader_searchModel model)
        {
            try
            {
                DSReportHeader DSReportHeader = null;
                BLObject<DSReportHeader> obj;
                obj = BLLAdminClinicalObj.loadReportHeader(model.ReportHeaderName, model.SpecialtyIds, model.ProviderIds, model.FacilityIds, model.PageNumber, model.RowsPerPage, model.IsActive);
                DSReportHeader = obj.Data;

                if (obj.Data != null && DSReportHeader.Tables[DSReportHeader.ReportHeaderList.TableName].Rows.Count > 0)
                {
                    List<ReportHeader_selectModel> modelList = new List<ReportHeader_selectModel>();
                    modelList = (from DataRow row in DSReportHeader.Tables[DSReportHeader.ReportHeaderList.TableName].Rows

                                 select new ReportHeader_selectModel
                                 {
                                     ReportHeaderId = MDVUtility.ToInt64(row["ReportHeaderId"]),
                                     ReportHeaderName = MDVUtility.ToStr(row["Name"]),
                                     SpecialtyNames = MDVUtility.ToStr(row["SpecialtyNames"]),
                                     ProviderNames = MDVUtility.ToStr(row["ProviderNames"]),
                                     FacilityNames = MDVUtility.ToStr(row["FacilityNames"]),
                                     LastUpdated = MDVUtility.ToStr(row["ModifiedOn"]) + " by " + MDVUtility.ToStr(row["ModifiedByName"]),
                                     IsActive = Convert.ToBoolean(row["IsActive"])
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        reportHeaderCount = DSReportHeader.Tables[DSReportHeader.ReportHeaderList.TableName].Rows.Count,
                        iTotalDisplayRecords = DSReportHeader.ReportHeaderList.Rows[0][DSReportHeader.ReportHeaderList.RecordCountColumn.ColumnName],
                        reportHeaderList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        reportHeaderCount = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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

        internal string deleteClinical_ReportHeader(long ReportHeaderId)
        {
            try
            {
                if (ReportHeaderId < 0)
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
                    BLObject<string> obj = BLLAdminClinicalObj.deleteReportHeader(ReportHeaderId);
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

        internal string updateClinical_ReportHeaderIsActive(long ReportHeaderId, string IsActive)
        {
            try
            {
                string successMsg = string.Empty;
                if (ReportHeaderId > 0)
                {
                    BLObject<string> obj = BLLAdminClinicalObj.UpdateReportHeaderActiveInactive(ReportHeaderId, IsActive.Equals("1") ? true : false);


                    if (obj.Data == "")
                    {
                        if (IsActive.Equals("0"))
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
                        Message = "Record not found."
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
        #region report tags

        public string GetNoteTemplateTagName()
        {
            BLObject<DSClinicalNoteTemplateLookup> obCase = BLLClinicalObj.GetNoteTemplateTagName(-1);

            if (obCase.Data != null)
            {
                DSClinicalNoteTemplateLookup ds = obCase.Data;
                List<NotesTagName_selectModel> modelList = new List<NotesTagName_selectModel>();
                modelList = (from DataRow row in ds.Tables[ds.NotesTagName.TableName].Rows

                             select new NotesTagName_selectModel
                             {
                                 NotesTagNameId = MDVUtility.ToInt64(row["NotesTagNameId"]),
                                 ShortName = MDVUtility.ToStr(row["ShortName"]),
                                 CategoryName = MDVUtility.ToStr(row["CategoryName"])
                             }).ToList();

                List<NotesTagName_selectModel> modelListPatient = modelList.Where(n => n.CategoryName.Equals("Patient")).ToList<NotesTagName_selectModel>();// as List<NotesTagName_selectModel>;
                List<NotesTagName_selectModel> modelListPractice = modelList.Where(n => n.CategoryName.Equals("Practice")).ToList<NotesTagName_selectModel>();// as List<NotesTagName_selectModel>;
                List<NotesTagName_selectModel> modelListProvider = modelList.Where(n => n.CategoryName.Equals("Primary Care Provider")).ToList<NotesTagName_selectModel>();//as List<NotesTagName_selectModel>;

                if (ds.Tables[ds.NotesTagName.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        reportHeaderCount = ds.Tables[ds.NotesTagName.TableName].Rows.Count,
                        PatientList_JSON = js.Serialize(modelListPatient),
                        PracticeList_JSON = js.Serialize(modelListPractice),
                        ProviderList_JSON = js.Serialize(modelListProvider),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            var responseResult = new
            {
                status = false,

            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseResult));
        }
        #endregion

        internal string SaveReportHeader(ReportHeader_FillModel fillmodel)
        {
            try
            {
                DSReportHeader dsReportHeader = new DSReportHeader();
                DSReportHeader.ReportHeaderRow dr = dsReportHeader.ReportHeader.NewReportHeaderRow();
                dr[dsReportHeader.ReportHeader.ReportHeaderIdColumn.ColumnName] = -1;
                dr[dsReportHeader.ReportHeader.NameColumn.ColumnName] = fillmodel.ReportHeaderName;
                dr[dsReportHeader.ReportHeader.SpecialtyIdsColumn.ColumnName] = fillmodel.SpecialtyIds;
                dr[dsReportHeader.ReportHeader.ProviderIdsColumn.ColumnName] = fillmodel.ProviderIds;
                dr[dsReportHeader.ReportHeader.FacilityIdsColumn.ColumnName] = fillmodel.FacilityIds;
                dr[dsReportHeader.ReportHeader.EntityIdsColumn.ColumnName] = fillmodel.EntityId;
                dr[dsReportHeader.ReportHeader.GeneratedByColumn.ColumnName] = fillmodel.FooterText;

                dr[dsReportHeader.ReportHeader.IsActiveColumn.ColumnName] = fillmodel.IsActive;

                dr[dsReportHeader.ReportHeader.PracticeTagColumn.ColumnName] = fillmodel.PracticeTags;

                dr[dsReportHeader.ReportHeader.PatientTagColumn.ColumnName] = fillmodel.PatientTags;
                dr[dsReportHeader.ReportHeader.ProviderTagColumn.ColumnName] = fillmodel.ProviderTags;
                dr[dsReportHeader.ReportHeader.HeaderLogoNameColumn.ColumnName] = fillmodel.HeaderLogoName;
                dr[dsReportHeader.ReportHeader.HeaderLogoColumn.ColumnName] = fillmodel.HeaderLogo;
                if (!string.IsNullOrEmpty(fillmodel.HeaderLogoName))
                {
                    dr[dsReportHeader.ReportHeader.HeaderLogoUpldDateColumn.ColumnName] = DateTime.Now;
                }
                else
                {
                    dr[dsReportHeader.ReportHeader.HeaderLogoUpldDateColumn.ColumnName] = DBNull.Value;
                }



                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsReportHeader.ReportHeader.AddReportHeaderRow(dr);


                #region Database Insertion
                BLObject<DSReportHeader> obj = BLLAdminClinicalObj.insertReportHeader(dsReportHeader);
                dsReportHeader = obj.Data;
                DataRow dr1 = dsReportHeader.Tables[dsReportHeader.ReportHeader.TableName].Rows[0];
                if (obj.Data != null)
                {
                    var response = new
                    {
                        ReportHeaderId = dr1[dsReportHeader.ReportHeader.ReportHeaderIdColumn.ColumnName],
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

        internal string updateReportHeader(ReportHeader_FillModel fillmodel)
        {
            try
            {
                if (MDVUtility.ToInt32(fillmodel.ReportHeaderId) > 0)
                {

                    DSReportHeader dsReportHeader = null;
                    BLObject<DSReportHeader> obj = BLLAdminClinicalObj.fillReportHeader(MDVUtility.ToInt32(fillmodel.ReportHeaderId));
                    dsReportHeader = obj.Data;
                    if (dsReportHeader.Tables[dsReportHeader.ReportHeader.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsReportHeader.Tables[dsReportHeader.ReportHeader.TableName].Rows[0];

                        dr[dsReportHeader.ReportHeader.NameColumn.ColumnName] = fillmodel.ReportHeaderName;
                        dr[dsReportHeader.ReportHeader.SpecialtyIdsColumn.ColumnName] = fillmodel.SpecialtyIds;
                        dr[dsReportHeader.ReportHeader.ProviderIdsColumn.ColumnName] = fillmodel.ProviderIds;
                        dr[dsReportHeader.ReportHeader.FacilityIdsColumn.ColumnName] = fillmodel.FacilityIds;
                        dr[dsReportHeader.ReportHeader.EntityIdsColumn.ColumnName] = fillmodel.EntityId;
                        dr[dsReportHeader.ReportHeader.GeneratedByColumn.ColumnName] = fillmodel.FooterText;

                        dr[dsReportHeader.ReportHeader.IsActiveColumn.ColumnName] = fillmodel.IsActive;

                        dr[dsReportHeader.ReportHeader.PracticeTagColumn.ColumnName] = fillmodel.PracticeTags;

                        dr[dsReportHeader.ReportHeader.PatientTagColumn.ColumnName] = fillmodel.PatientTags;
                        dr[dsReportHeader.ReportHeader.ProviderTagColumn.ColumnName] = fillmodel.ProviderTags;

                        if (!string.IsNullOrEmpty(fillmodel.HeaderLogoName) && fillmodel.HeaderLogo != MDVUtility.ToStr(dr[dsReportHeader.ReportHeader.HeaderLogoColumn.ColumnName]))
                        {
                            dr[dsReportHeader.ReportHeader.HeaderLogoUpldDateColumn.ColumnName] = DateTime.Now;
                        }
                        dr[dsReportHeader.ReportHeader.HeaderLogoColumn.ColumnName] = fillmodel.HeaderLogo;
                        dr[dsReportHeader.ReportHeader.HeaderLogoNameColumn.ColumnName] = fillmodel.HeaderLogoName;
                        dr[dsReportHeader.ReportHeader.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsReportHeader.ReportHeader.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        BLObject<DSReportHeader> objMG = BLLAdminClinicalObj.UpdateReportHeader(dsReportHeader);

                        if (objMG.Data != null)
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
                                Message = objMG.Message
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
                        Message = "Record not found."
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



        internal string LoadHeaderSettings(ReportHeader_searchModel model)
        {
            try
            {

                DSReportHeader ds = null;
                BLObject<DSReportHeader> obj;

                obj = BLLAdminClinicalObj.LoadReportHeaderSettings(model.ReportHeaderId);
                ds = obj.Data;

                if (ds.Tables[ds.RptHeaderSettings.TableName].Rows.Count > 0)
                {
                    List<Dictionary<string, string>> headerSettings = new List<Dictionary<string, string>>();
                    foreach (DataRow dr in ds.Tables[ds.RptHeaderSettings.TableName].Rows)
                    {
                        var ConfigurationkeyValues = new Dictionary<string, string>
                        {
                            { "RptHeaderSettingId",  MDVUtility.ToStr(dr[ds.RptHeaderSettings.RptHeaderSettingsIdColumn.ColumnName])},
                            { "RptHeaderConfigurationId", MDVUtility.ToStr(dr[ds.RptHeaderSettings.RptHeaderConfigurationIdColumn.ColumnName])},
                            { "Name", MDVUtility.ToStr(dr[ds.RptHeaderSettings.NameColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[ds.RptHeaderSettings.IsActiveColumn.ColumnName])}
                        };
                        headerSettings.Add(ConfigurationkeyValues);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        HeaderConfigurationFill_JSON = js.Serialize(headerSettings),
                        HeaderConfigurationCount = ds.Tables[ds.RptHeaderSettings.TableName].Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        HeaderConfigurationFill_JSON = "[]",
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

        public string UpdateHeaderSettings(ReportHeader_searchModel model)
        {
            try
            {
                DSReportHeader dsReportHeader = new DSReportHeader();

                BLObject<DSReportHeader> objHeaderSettings = BLLAdminClinicalObj.LoadReportHeaderSettings(model.ReportHeaderId);
                dsReportHeader = objHeaderSettings.Data;

                //  List<ReportHeader_searchModel> lstHeaderSettings = lstObjects.OfType<ReportHeader_searchModel>().ToList();
                DSReportHeader.RptHeaderSettingsRow RowHeaderSettings = null;
                DSReportHeader.RptHeaderSettingsRow[] arrHeaderSettingsRows = (DSReportHeader.RptHeaderSettingsRow[])dsReportHeader.RptHeaderSettings.Select(dsReportHeader.RptHeaderSettings.RptHeaderSettingsIdColumn.ColumnName + "=" + model.RptHeaderSettingId);
                if (arrHeaderSettingsRows.Length > 0)
                {
                    RowHeaderSettings = arrHeaderSettingsRows[0];
                }
                else
                {
                    RowHeaderSettings = dsReportHeader.RptHeaderSettings.NewRptHeaderSettingsRow();
                }

                if (RowHeaderSettings != null)
                {
                    RowHeaderSettings.RptHeaderSettingsId = Convert.ToString(model.RptHeaderSettingId);
                    RowHeaderSettings.ReportHeaderId = Convert.ToString(model.ReportHeaderId);
                    RowHeaderSettings.RptHeaderConfigurationId = model.RptHeaderConfigurationId;
                    RowHeaderSettings.IsActive = model.IsActive;
                    if (arrHeaderSettingsRows.Length < 1)
                    {
                        dsReportHeader.RptHeaderSettings.AddRptHeaderSettingsRow(RowHeaderSettings);
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSReportHeader> objInsertedHeaderSetting = BLLAdminClinicalObj.UpdateReportHeaderSettings(dsReportHeader);
                if (objInsertedHeaderSetting.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedHeaderSetting.Message
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

        internal string fillReportHeader(long reportHeaderId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSReportHeader DSReportHeader = null;
                    BLObject<DSReportHeader> obj;
                    obj = BLLAdminClinicalObj.fillReportHeader(reportHeaderId);
                    DSReportHeader = obj.Data;

                    if (obj.Data != null && DSReportHeader.Tables[DSReportHeader.ReportHeader.TableName].Rows.Count > 0)
                    {
                        List<ReportHeader_FillModel> modelList = new List<ReportHeader_FillModel>();
                        modelList = (from DataRow row in DSReportHeader.Tables[DSReportHeader.ReportHeader.TableName].Rows

                                     select new ReportHeader_FillModel
                                     {
                                         ReportHeaderId = MDVUtility.ToInt64(row["ReportHeaderId"]),
                                         ReportHeaderName = MDVUtility.ToStr(row["Name"]),
                                         SpecialtyIds = MDVUtility.ToStr(row["SpecialtyIds"]),
                                         ProviderIds = MDVUtility.ToStr(row["ProviderIds"]),
                                         FacilityIds = MDVUtility.ToStr(row["FacilityIds"]),
                                         EntityIds = MDVUtility.ToStr(row["EntityIds"]),
                                         FooterText = MDVUtility.ToStr(row["GeneratedBy"]),
                                         PracticeTags = MDVUtility.ToStr(row["PracticeTag"]),
                                         PatientTags = MDVUtility.ToStr(row["PatientTag"]),
                                         ProviderTags = MDVUtility.ToStr(row["ProviderTag"]),
                                         HeaderLogo = MDVUtility.ToStr(row["HeaderLogo"]),
                                         HeaderLogoUpldDate = MDVUtility.ToStr(row["HeaderLogoUpldDate"]),
                                         HeaderLogoName = MDVUtility.ToStr(row["HeaderLogoName"]),
                                         ModifiedBy = MDVUtility.ToStr(row["ModifiedBy"]),
                                         ModifiedOn = MDVUtility.ToStr(row["ModifiedOn"]),
                                         IsActive = Convert.ToBoolean(row["IsActive"])
                                     }).ToList();

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            reportHeaderCount = DSReportHeader.Tables[DSReportHeader.ReportHeader.TableName].Rows.Count,
                            reportHeaderList_JSON = js.Serialize(modelList),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            reportHeaderCount = 0,
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
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private ReportHeader_TagsSelectModel generateHeaderFooterHTML(List<ReportHeader_TagsModel> modelList)
        {
            ReportHeader_TagsSelectModel modelfill = new ReportHeader_TagsSelectModel();
            #region Header
            StringBuilder HeaderSb = new StringBuilder();
            StringBuilder FooterSb = new StringBuilder();
            foreach (var model in modelList)
            {
                bool isHeaderLogoNull = true;
                bool isPracticeNull = true;
                bool isPatientNull = true;
                bool isProviderNull = true;
                if (!string.IsNullOrEmpty(model.HeaderLogo))
                {
                    isHeaderLogoNull = false;
                }
                if (!string.IsNullOrEmpty(model.PracticeText))
                {
                    isPracticeNull = false;
                }
                if (!string.IsNullOrEmpty(model.PatientText))
                {
                    isPatientNull = false;
                }
                if (!string.IsNullOrEmpty(model.ProviderText))
                {
                    isProviderNull = false;
                }
                if (!string.IsNullOrEmpty(model.PatientName))
                {
                    modelfill.PatientName = model.PatientName;
                }
                if (model.PatientDOB != null )
                {
                    modelfill.PatientDOB = model.PatientDOB;
                }
                if (!string.IsNullOrEmpty(model.ProviderName))
                {
                    modelfill.ProviderName = model.ProviderName;
                }
                if (model.DOS != null)
                {
                    modelfill.DOS = model.DOS;
                }

                if (!(isHeaderLogoNull && isPracticeNull && isPatientNull && isProviderNull))
                {
                    HeaderSb.Append("<header><div class='form-group'><hr class='blue' style='color: darkblue;border: 3px;background: navy;height: 5px;margin:2px;'>");
                    HeaderSb.Append("<div class='col-sm-12 col-lg-12  mt-xs'>");
                    if (!isHeaderLogoNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-left'><img style='max-width:350px;max-height:140px;' src='" + model.HeaderLogo + "' class='img-responsive'></div>");
                    }

                    if (!isPracticeNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-right mr-sm'><ul class='list-unstyled line-height-fix'  id='PracticeList'>");
                        string[] PracticeArray = model.PracticeText.Split(new string[] { "<br/>" },
                                                    StringSplitOptions.None);
                        foreach (var item in PracticeArray)
                        {
                            HeaderSb.Append("<li class='text-right'>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");
                    }
                    HeaderSb.Append("</div>");
                    HeaderSb.Append("<div class='clearfix'></div><div class='splitter m-none mt-xs'><div class='spacer3'></div></div>");

                    HeaderSb.Append(" <div class='spacer3'></div><div class='col-sm-12 col-lg-12  mt-xs'>");
                    if (!isPatientNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-left mr-sm'><ul class='list-unstyled line-height-fix' id='PatientList'>");
                        string[] PatientText = model.PatientText.Split(new string[] { "<br/>" },
                                                   StringSplitOptions.None);
                        foreach (var item in PatientText)
                        {
                            HeaderSb.Append("<li>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");
                    }
                    if (!isProviderNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-right mr-sm'><ul class='list-unstyled line-height-fix'  id='ProviderList'>");
                        string[] ProviderText = model.ProviderText.Split(new string[] { "<br/>" },
                                                  StringSplitOptions.None);
                        foreach (var item in ProviderText)
                        {
                            HeaderSb.Append("<li class='text-right'>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");

                    }
                    HeaderSb.Append("</div>");
                    HeaderSb.Append("<div class='clearfix'></div><div class='splitter m-none mt-xs'><div class='spacer3'></div></div></header>");
                }


            #endregion
                #region Footer
                if (!string.IsNullOrEmpty(model.FooterText))
                {
                    FooterSb.Append("<div class='clearfix'></div><footer><div  style='position:fixed;bottom:0px;float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:5px 25px'><label class='white pl-sm pull-left' style='color:white'> Generated by: " + model.FooterText + "</label></div></footer>");
                }
                #endregion
            }
            modelfill.reportHeaderCount = modelList.Count;
            modelfill.Header = HeaderSb.ToString();
            modelfill.Footer = FooterSb.ToString();
            return modelfill;
        }

        // set multiple notes headers for bulk sign note
        internal List<RptHdrTags> generateHeaderFooterHTMLBulk(List<RptHdrTags> modelList)
        {
            foreach (var model in modelList)
            {
                #region Header
                StringBuilder HeaderSb = new StringBuilder();
                StringBuilder FooterSb = new StringBuilder();
                bool isHeaderLogoNull = true;
                bool isPracticeNull = true;
                bool isPatientNull = true;
                bool isProviderNull = true;
                if (!string.IsNullOrEmpty(model.HeaderLogo))
                {
                    isHeaderLogoNull = false;
                }
                if (!string.IsNullOrEmpty(model.PracticeText))
                {
                    isPracticeNull = false;
                }
                if (!string.IsNullOrEmpty(model.PatientText))
                {
                    isPatientNull = false;
                }
                if (!string.IsNullOrEmpty(model.ProviderText))
                {
                    isProviderNull = false;
                }
                if (model.PatientDOB != null)
                {
                    model.PatientDOB = MDVUtility.ToDateTime(model.PatientDOB).ToString("MM/dd/yyyy");
                }
                if (model.DOS != null)
                {
                    model.DOS = MDVUtility.ToDateTime(model.DOS).ToString("MM/dd/yyyy");
                }

                if (!(isHeaderLogoNull && isPracticeNull && isPatientNull && isProviderNull))
                {
                    HeaderSb.Append("<header><div class='form-group'><hr class='blue' style='color: darkblue;border: 3px;background: navy;height: 5px;margin:2px;'>");
                    HeaderSb.Append("<div class='col-sm-12 col-lg-12  mt-xs'>");
                    if (!isHeaderLogoNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-left'><img style='max-width:350px;max-height:140px;' src='" + model.HeaderLogo + "' class='img-responsive'></div>");
                    }

                    if (!isPracticeNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-right mr-sm'><ul class='list-unstyled line-height-fix'  id='PracticeList'>");
                        string[] PracticeArray = model.PracticeText.Split(new string[] { "<br/>" },
                                                    StringSplitOptions.None);
                        foreach (var item in PracticeArray)
                        {
                            HeaderSb.Append("<li class='text-right'>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");
                    }
                    HeaderSb.Append("</div>");
                    HeaderSb.Append("<div class='clearfix'></div><div class='splitter m-none mt-xs'><div class='spacer3'></div></div>");

                    HeaderSb.Append(" <div class='spacer3'></div><div class='col-sm-12 col-lg-12  mt-xs'>");
                    if (!isPatientNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-left mr-sm'><ul class='list-unstyled line-height-fix' id='PatientList'>");
                        string[] PatientText = model.PatientText.Split(new string[] { "<br/>" },
                                                   StringSplitOptions.None);
                        foreach (var item in PatientText)
                        {
                            HeaderSb.Append("<li>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");
                    }
                    if (!isProviderNull)
                    {
                        HeaderSb.Append("<div class='col-sm-5 col-lg-5 pull-right mr-sm'><ul class='list-unstyled line-height-fix'  id='ProviderList'>");
                        string[] ProviderText = model.ProviderText.Split(new string[] { "<br/>" },
                                                  StringSplitOptions.None);
                        foreach (var item in ProviderText)
                        {
                            HeaderSb.Append("<li class='text-right'>" + item + "</li>");
                        }
                        HeaderSb.Append("</ul></div>");

                    }
                    HeaderSb.Append("</div>");
                    HeaderSb.Append("<div class='clearfix'></div><div class='splitter m-none mt-xs'><div class='spacer3'></div></div></header>");
                }


                #endregion
                #region Footer
                if (!string.IsNullOrEmpty(model.FooterText))
                {
                    FooterSb.Append("<div class='clearfix'></div><footer><div  style='position:fixed;bottom:0px;float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:5px 25px'><label class='white pl-sm pull-left' style='color:white'> Generated by: " + model.FooterText + "</label></div></footer>");
                }
                model.Header = HttpUtility.HtmlDecode(HeaderSb.ToString());
                model.Footer = HttpUtility.HtmlDecode(FooterSb.ToString());
                #endregion
            }
            return modelList;
        }

        internal ReportHeader_TagsSelectModel getReportHeaderTagsHTML(long PatientId, long ProviderId, long NotesID, string formName, string PreviewStyle = null)
        {
            ReportHeader_TagsSelectModel modelfill = new ReportHeader_TagsSelectModel();
            try
            {

                DSReportHeader DSReportHeader = null;
                BLObject<DSReportHeader> obj;
                obj = BLLAdminClinicalObj.getReportHeaderTagsValue(PatientId, ProviderId, NotesID, formName, PreviewStyle);
                DSReportHeader = obj.Data;

                if (obj.Data != null && DSReportHeader.Tables[DSReportHeader.ReportHeaderTags.TableName].Rows.Count > 0)
                {
                    List<ReportHeader_TagsModel> modelList = new List<ReportHeader_TagsModel>();
                    modelList = (from DataRow row in DSReportHeader.Tables[DSReportHeader.ReportHeaderTags.TableName].Rows

                                 select new ReportHeader_TagsModel
                                 {
                                     PracticeText = MDVUtility.ToStr(row["PracticeText"]),
                                     PatientText = MDVUtility.ToStr(row["PatientText"]),
                                     ProviderText = MDVUtility.ToStr(row["ProviderText"]),
                                     HeaderLogo = MDVUtility.ToStr(row["HeaderLogo"]),
                                     FooterText = MDVUtility.ToStr(row["FooterText"]),
                                     PatientName = MDVUtility.ToStr(row["PatientName"]),
                                     PatientDOB = MDVUtility.ToDateTime(row["PatientDOB"]),
                                     ProviderName = MDVUtility.ToStr(row["ProviderName"]),
                                     DOS = MDVUtility.ToDateTime(row["DOS"])
                                 }).ToList();

                    if (modelList != null && modelList.Count > 0)
                    {
                        modelfill = generateHeaderFooterHTML(modelList);
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return modelfill;
        }

        internal string getReportHeaderTags(long PatientId, long ProviderId, long NotesID, string formName)
        {
            try
            {
                DSReportHeader DSReportHeader = null;
                BLObject<DSReportHeader> obj;
                obj = BLLAdminClinicalObj.getReportHeaderTagsValue(PatientId, ProviderId, NotesID, formName);
                DSReportHeader = obj.Data;

                DSNotes dsHeaderNotes = new DSNotes();
                BLObject<DSNotes> objHeaderLoad = BLLClinicalObj.loadClinicalNoteHeaderData(PatientId, ProviderId, NotesID);
                dsHeaderNotes = objHeaderLoad.Data;
                DSPatient dsPatientInfo = new DSPatient();
                DSProfile dsProvider = new DSProfile();
                if (obj.Data != null && DSReportHeader.Tables[DSReportHeader.ReportHeaderTags.TableName].Rows.Count > 0)
                {
                    List<ReportHeader_TagsModel> modelList = new List<ReportHeader_TagsModel>();
                    modelList = (from DataRow row in DSReportHeader.Tables[DSReportHeader.ReportHeaderTags.TableName].Rows

                                 select new ReportHeader_TagsModel
                                 {
                                     PracticeText = MDVUtility.ToStr(row["PracticeText"]),
                                     PatientText = MDVUtility.ToStr(row["PatientText"]),
                                     ProviderText = MDVUtility.ToStr(row["ProviderText"]),
                                     HeaderLogo = MDVUtility.ToStr(row["HeaderLogo"]),
                                     FooterText = MDVUtility.ToStr(row["FooterText"])
                                 }).ToList();

                    if (modelList != null && modelList.Count > 0)
                    {
                        ReportHeader_TagsSelectModel modelfill = generateHeaderFooterHTML(modelList);
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            reportHeaderCount = modelfill.reportHeaderCount,
                            Footer = modelfill.Footer,
                            Header = modelfill.Header,
                            HeaderPatientData = MDVUtility.JSON_DataTable(dsHeaderNotes.Tables[dsPatientInfo.Patients.TableName]),
                            HeaderProviderData = MDVUtility.JSON_DataTable(dsHeaderNotes.Tables[dsProvider.Provider.TableName]),
                            HeaderPracticeData = MDVUtility.JSON_DataTable(dsHeaderNotes.Tables[dsProvider.Practice.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            reportHeaderCount = 0,
                            Footer = string.Empty,
                            Header = string.Empty
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        reportHeaderCount = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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

    public class NotesTagName_selectModel
    {
        public long ReportHeaderId { get; set; }
        public string ReportHeaderName { get; set; }

        public string SpecialtyNames { get; set; }

        public string CategoryName { get; set; }

        public string ShortName { get; set; }

        public long NotesTagNameId { get; set; }
    }
}