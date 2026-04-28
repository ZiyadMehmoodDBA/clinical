using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Datasets;

using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.PQRS;
using MDVision.IEHR.EMR.Model.QRDA;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MDVision.IEHR.EMR.Helpers.Clinical.PQRSAdmin
{
    public class PQRSHelper
    {
        private BLLPQRS PQRSHelperObj = null;
        public PQRSHelper() {
            PQRSHelperObj = new BLLPQRS();
        }

        private static PQRSHelper _instance = null;
        public static PQRSHelper Instance()
        {
            if (_instance == null)
                _instance = new PQRSHelper();
            return _instance;
        }
        #region MeasureGroup
        public string searchMeasureGroup(PQRS_MeasureGroupSearchModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.loadMeasureGroup(model.MeasureGroupName, model.ProviderIds, model.PageNumber, model.RowsPerPage, model.IsActive);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.MeasureGroupsList.TableName].Rows.Count > 0)
                {
                    List<PQRS_MeasureGroupSelectModel> modelList = new List<PQRS_MeasureGroupSelectModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.MeasureGroupsList.TableName].Rows

                                 select new PQRS_MeasureGroupSelectModel
                                 {
                                     measureGroupName = row["MeasureGroupName"].ToString(),
                                     providersName = row["ProviderList"].ToString(),
                                     createdOn = MDVUtility.GetDateMMDDYYY(row["CreatedOn"].ToString()),
                                     measureGroupId = MDVUtility.ToInt64(row["MeasureGroupId"]),
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                     IsReported = Convert.ToBoolean(row["IsReported"])
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        measureGroupCount = DSPQRS.Tables[DSPQRS.MeasureGroupsList.TableName].Rows.Count,
                        iTotalDisplayRecords = DSPQRS.MeasureGroupsList.Rows[0][DSPQRS.MeasureGroupsList.RecordCountColumn.ColumnName],
                        measureGroupList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        measureGroupCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string updatePQRS_MeasureGroupsIsActive(Int64 measureGroupId, string IsActive)
        {
            try
            {
                if (measureGroupId > 0)
                {

                    DSPQRS dsMeasureGroup = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureGroup(measureGroupId);
                    dsMeasureGroup = obj.Data;
                    if (dsMeasureGroup.Tables[dsMeasureGroup.MeasureGroups.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureGroup.Tables[dsMeasureGroup.MeasureGroups.TableName].Rows[0];
                        dr[dsMeasureGroup.MeasureGroups.IsActiveColumn.ColumnName] = IsActive.Equals("1") ? true : false;

                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureGroup(dsMeasureGroup);
                        string successMsg;
                        if (objMG.Data != null)
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string deletePQRS_MeasureGroups(long measureGroupId)
        {
            try
            {
                if (measureGroupId < 0)
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
                    BLObject<string> obj = PQRSHelperObj.deleteMeasureGroup(measureGroupId);
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

        public string searchMeasures(PQRS_MeasureSearchModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.fillMeasures(model.MeasureId);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.Measures.TableName].Rows.Count > 0)
                {
                    List<PQRS_MeasureSelectModel> modelList = new List<PQRS_MeasureSelectModel>(); 
                    List<PQRS_MeasureSelectModel> CQMMeasurelList = new List<PQRS_MeasureSelectModel>();
                    List<PQRS_MeasureSelectModel> VBPMeasurelList = new List<PQRS_MeasureSelectModel>();
                    List<PQRS_MeasureSelectModel> IAMeasureList = new List<PQRS_MeasureSelectModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.Measures.TableName].Select("MeasureType='PQRS'")

                                 select new PQRS_MeasureSelectModel
                                 {
                                     MeasureId = MDVUtility.ToInt64(row["MeasureId"]),
                                     MeasureTitle = row["ShortName"].ToString(),
                                     MeasureNumber = row["MeasureNumber"].ToString(),
                                     NQSDomain = row["NQSDomain"].ToString(),
                                     DocumentName = row["DocumentName"].ToString(),
                                     DocumentPath = row["DocumentPath"].ToString(),
                                     MeasureType = row["MeasureType"].ToString()
                                    
                                 }).ToList();

                    CQMMeasurelList = (from DataRow row in DSPQRS.Tables[DSPQRS.Measures.TableName].Select("MeasureType='CQM'")

                                 select new PQRS_MeasureSelectModel
                                 {
                                     MeasureId = MDVUtility.ToInt64(row["MeasureId"]),
                                     MeasureTitle = row["ShortName"].ToString(),
                                     MeasureNumber = row["MeasureNumber"].ToString(),
                                     NQSDomain = row["NQSDomain"].ToString(),
                                     DocumentName = row["DocumentName"].ToString(),
                                     DocumentPath = row["DocumentPath"].ToString(),
                                     MeasureType = row["MeasureType"].ToString(),
                                     HighPriority = row["ActivityWeighting"].ToString()
                                 }).ToList();

                    IAMeasureList = (from DataRow row in DSPQRS.Tables[DSPQRS.Measures.TableName].Select("MeasureType='IA'")

                                       select new PQRS_MeasureSelectModel
                                       {
                                           MeasureId = MDVUtility.ToInt64(row["MeasureId"]),
                                           MeasureTitle = row["ShortName"].ToString(),
                                           MeasureNumber = row["MeasureNumber"].ToString(),
                                           NQSDomain = row["ActivityWeighting"].ToString(),
                                           MeasureType = row["MeasureType"].ToString()
                                       }).ToList();

                    VBPMeasurelList = (from DataRow row in DSPQRS.Tables[DSPQRS.Measures.TableName].Select("MeasureType='VBP'")

                                       select new PQRS_MeasureSelectModel
                                       {
                                           MeasureId = MDVUtility.ToInt64(row["MeasureId"]),
                                           MeasureTitle = row["ShortName"].ToString(),
                                           MeasureNumber = row["MeasureNumber"].ToString(),
                                           NQSDomain = row["NQSDomain"].ToString(),
                                           DocumentName = row["DocumentName"].ToString(),
                                           DocumentPath = row["DocumentPath"].ToString(),
                                           MeasureType = row["MeasureType"].ToString()
                                       }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        measureCount = DSPQRS.Tables[DSPQRS.Measures.TableName].Rows.Count,
                        PQRSmeasureCount = modelList.Count,
                        PQRSmeasureList_JSON = js.Serialize(modelList),
                        CQMmeasureCount = CQMMeasurelList.Count,
                        CQMmeasureList_JSON=js.Serialize(CQMMeasurelList),
                        VBPmeasureCount = VBPMeasurelList.Count,
                        VBPmeasureList_JSON = js.Serialize(VBPMeasurelList),
                        IAmeasureList_JSON = js.Serialize(IAMeasureList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        measureCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string viewPDFMeasures(long? MeasuresId)
        {
            try
            {
                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                if (MeasuresId != null && MeasuresId > 0)
                {


                    obj = PQRSHelperObj.fillMeasures(MeasuresId);
                    DSPQRS = obj.Data;

                    if (obj.Data != null && DSPQRS.Tables[DSPQRS.Measures.TableName].Rows.Count > 0)
                    {
                        string DocumentPath = MDVUtility.ToStr(DSPQRS.Measures.Rows[0][DSPQRS.Measures.DocumentPathColumn.ColumnName]);
                        string DocumentName = MDVUtility.ToStr(DSPQRS.Measures.Rows[0][DSPQRS.Measures.DocumentNameColumn.ColumnName]);
                        string FilePath = string.Empty;
                        if (!string.IsNullOrEmpty(DocumentPath) && !string.IsNullOrEmpty(DocumentName))
                        {
                            FilePath = HttpContext.Current.Server.MapPath(@DocumentPath + DocumentName);
                        }

                        if (!string.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath))
                        {
                            byte[] fileinBytes = System.IO.File.ReadAllBytes(FilePath);
                            var response = new
                            {
                                status = true,
                                CMS_ViewHTML = Convert.ToBase64String(fileinBytes),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                }
                var responseResult = new
                    {
                        status = false,
                        Message = "File Not Found"
                    };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseResult));


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

        internal string viewPDFMeasuresByMeasureNumber(string MeasureNumber)
        {
            try
            {
                string strFilePath = string.Empty;
                byte[] byteArr = null;
                strFilePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Measure\" + MeasureNumber;
                if (File.Exists(strFilePath))
                {
                    using (var stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            byteArr = reader.ReadBytes((int)stream.Length);
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    CMS_ViewHTML = Convert.ToBase64String(byteArr),
                };
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                js.MaxJsonLength = Int32.MaxValue;
                return js.Serialize(response);
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

        internal string savePQRS_MeasureGroups(PQRS_MeasureGroupFillModel modelfill)
        {
            try
            {
                DSPQRS dsMeasureGroup = new DSPQRS();
                DSPQRS.MeasureGroupsRow dr = dsMeasureGroup.MeasureGroups.NewMeasureGroupsRow();
                dr[dsMeasureGroup.MeasureGroups.MeasureGroupIdColumn.ColumnName] = -1;
                dr[dsMeasureGroup.MeasureGroups.IsActiveColumn.ColumnName] = modelfill.IsActive;
                dr[dsMeasureGroup.MeasureGroups.SpecialtyIdsColumn.ColumnName] = modelfill.SpecialtyIds;
                dr[dsMeasureGroup.MeasureGroups.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;
                dr[dsMeasureGroup.MeasureGroups.ProviderIdsColumn.ColumnName] = modelfill.ProviderIds;
                dr[dsMeasureGroup.MeasureGroups.ShortNameColumn.ColumnName] = modelfill.MeasureGroupsName;
                dr[dsMeasureGroup.MeasureGroups.MeasureIdsColumn.ColumnName] = modelfill.MeasureIds;
                dr[dsMeasureGroup.MeasureGroups.SubmissionYearColumn.ColumnName] = modelfill.SubmissionYear;


                dr.EntityId = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsMeasureGroup.MeasureGroups.AddMeasureGroupsRow(dr);


                #region Database Insertion
                BLObject<DSPQRS> obj = PQRSHelperObj.insertMeasureGroup(dsMeasureGroup);
                dsMeasureGroup = obj.Data;
               
                if (obj.Data != null)
                {
                    DataRow dr1 = dsMeasureGroup.Tables[dsMeasureGroup.MeasureGroups.TableName].Rows[0];
                    var response = new
                    {
                        MeasureGroupId = dr1[dsMeasureGroup.MeasureGroups.MeasureGroupIdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string PQRS_GetMeasureReasons(int measureId)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.getMeasureReasons(measureId);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.PQRS_MeasureReasons.TableName].Rows.Count > 0)
                {
                    List<PQRS_GetMeasureReasons> modelList = new List<PQRS_GetMeasureReasons>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.PQRS_MeasureReasons.TableName].Rows

                                 select new PQRS_GetMeasureReasons
                                 {
                                     ReasonId = MDVUtility.ToInt64(row["ReasonId"]),
                                     ReasonCode = MDVUtility.ToStr(row["ReasonCode"]),
                                     CodeType = MDVUtility.ToStr(row["CodeType"]),
                                     Reason = MDVUtility.ToStr(row["Reason"])
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        MeasureReasonsCount = modelList.Count,
                        MeasureReasonsList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        MeasureReasonsCount = 0,
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

        internal string PQRS_GetPatientsFromVisits(string visitIds)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.getPatientsFromVisits(visitIds);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.PQRS_PatientFromVisits.TableName].Rows.Count > 0)
                {
                    List<PQRS_GetPatientsFromVisits> modelList = new List<PQRS_GetPatientsFromVisits>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.PQRS_PatientFromVisits.TableName].Rows

                                 select new PQRS_GetPatientsFromVisits
                                 {
                                     DOB = MDVUtility.GetDateDDMMYYY(MDVUtility.ToStr(row["DOB"])),
                                     PatientName = MDVUtility.ToStr(row["PatientName"]),
                                     AccountNumber = MDVUtility.ToStr(row["AccountNumber"]),
                                     VisitId = MDVUtility.ToInt64(row["VisitId"]),
                                     PatientId = MDVUtility.ToInt64(row["PatientId"])
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        patientCount = DSPQRS.Tables[DSPQRS.PQRS_PatientFromVisits.TableName].Rows.Count,
                        patientList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        patientCount = 0,
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

        internal string updatePQRS_MeasureGroups(PQRS_MeasureGroupFillModel modelfill)
        {
            try
            {
                if (MDVUtility.ToInt32(modelfill.MeasureGroupId) > 0)
                {

                    DSPQRS dsMeasureGroup = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureGroup(MDVUtility.ToInt32(modelfill.MeasureGroupId));
                    dsMeasureGroup = obj.Data;
                    if (dsMeasureGroup.Tables[dsMeasureGroup.MeasureGroups.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureGroup.Tables[dsMeasureGroup.MeasureGroups.TableName].Rows[0];
                        dr[dsMeasureGroup.MeasureGroups.IsActiveColumn.ColumnName] = modelfill.IsActive;
                        dr[dsMeasureGroup.MeasureGroups.SpecialtyIdsColumn.ColumnName] = modelfill.SpecialtyIds;
                        dr[dsMeasureGroup.MeasureGroups.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;
                        dr[dsMeasureGroup.MeasureGroups.ProviderIdsColumn.ColumnName] = modelfill.ProviderIds;
                        dr[dsMeasureGroup.MeasureGroups.ShortNameColumn.ColumnName] = modelfill.MeasureGroupsName;
                        dr[dsMeasureGroup.MeasureGroups.MeasureIdsColumn.ColumnName] = modelfill.MeasureIds;
                        dr[dsMeasureGroup.MeasureGroups.SubmissionYearColumn.ColumnName] = modelfill.SubmissionYear;
                        dr[dsMeasureGroup.MeasureGroups.EntityIdColumn.ColumnName] = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));
                        dr[dsMeasureGroup.MeasureGroups.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsMeasureGroup.MeasureGroups.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureGroup(dsMeasureGroup);

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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string fillPQRS_MeasureGroups(long MeasureGroupId)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.fillMeasureGroup(MeasureGroupId);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.MeasureGroups.TableName].Rows.Count > 0)
                {
                    List<PQRS_MeasureGroupFillModel> modelList = new List<PQRS_MeasureGroupFillModel>();
                    List<PQRS_MeasureGroupFillModel> CQMMeasureList = new List<PQRS_MeasureGroupFillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.MeasureGroups.TableName].Rows

                                 select new PQRS_MeasureGroupFillModel
                                 {
                                     MeasureGroupId = row["MeasureGroupId"].ToString(),
                                     MeasureGroupsName = row["ShortName"].ToString(),
                                     SubmissionYear = row["SubmissionYear"].ToString(),
                                     ProviderIds = row["ProviderIds"].ToString(),
                                     MeasureIds = row["MeasureIds"].ToString(),
                                     SpecialtyIds = row["SpecialtyIds"].ToString(),
                                     PracticeIds = row["PracticeIds"].ToString(),
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                     IsReported = Convert.ToBoolean(row["IsReported"]),
                                     CQMMeasureIds = row["CQMMeasureIds"].ToString()
                                     //MeasureType = row["MeasureType"].ToString()
                                 }).ToList();

                    //CQMMeasureList = (from DataRow row in DSPQRS.Tables[DSPQRS.MeasureGroups.TableName].Select("MeasureType='CQM'")

                    //             select new PQRS_MeasureGroupFillModel
                    //             {
                    //                 MeasureGroupId = row["MeasureGroupId"].ToString(),
                    //                 MeasureGroupsName = row["ShortName"].ToString(),
                    //                 SubmissionYear = row["SubmissionYear"].ToString(),
                    //                 ProviderIds = row["ProviderIds"].ToString(),
                    //                 MeasureIds = row["MeasureIds"].ToString(),
                    //                 SpecialtyIds = row["SpecialtyIds"].ToString(),
                    //                 PracticeIds = row["PracticeIds"].ToString(),
                    //                 IsActive = Convert.ToBoolean(row["IsActive"]),
                    //                 IsReported = Convert.ToBoolean(row["IsReported"]),
                    //                 CQMMeasureIds = row["CQMMeasureIds"].ToString()
                    //             }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        measureGroupList_JSON = js.Serialize(modelList),
                        //PQRSmeasureCount = modelList.Count,
                        //PQRSmeasureList_JSON = js.Serialize(modelList),
                        //CQMmeasureCount = CQMMeasureList.Count,
                        //CQMmeasureList_JSON = js.Serialize(CQMMeasureList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Measure Individual
        internal string searchMeasureIndividual(Model.QRDA.PQRS_IndividualReportingSearchModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                long ProviderId = string.IsNullOrEmpty(model.ProviderId) ? -1 : MDVUtility.ToInt64(model.ProviderId);
                long SpecialityId = string.IsNullOrEmpty(model.SpecialityId) ? -1 : MDVUtility.ToInt64(model.SpecialityId);
                obj = PQRSHelperObj.loadMeasureIndividual(model.MeasureIndividualId, ProviderId, SpecialityId, model.PageNumber, model.RowsPerPage, model.IsActive);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.IndividualReportingList.TableName].Rows.Count > 0)
                {
                    List<Model.QRDA.PQRS_MeasureIndividualSelectModel> modelList = new List<Model.QRDA.PQRS_MeasureIndividualSelectModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.IndividualReportingList.TableName].Rows

                                 select new Model.QRDA.PQRS_MeasureIndividualSelectModel
                                 {
                                     measureIndividualId = MDVUtility.ToInt64(row["MeasureIndividualId"]),
                                     providerName = MDVUtility.ToStr(row["ProviderName"].ToString()),
                                     SubmissionYear = MDVUtility.ToStr(row["SubmissionYear"].ToString()),
                                     specialityName = MDVUtility.ToStr(row["SpecialityName"].ToString()),
                                     createdOn = MDVUtility.GetDateMMDDYYY(row["CreatedOn"].ToString()),
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                     IsReported = Convert.ToBoolean(row["IsReported"])
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        individualReportingCount = DSPQRS.Tables[DSPQRS.IndividualReportingList.TableName].Rows.Count,
                        iTotalDisplayRecords = DSPQRS.IndividualReportingList.Rows[0][DSPQRS.IndividualReportingList.RecordCountColumn.ColumnName],
                        IndividualReportingLoad_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        measureGroupCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string updatePQRS_MeasureIndividualIsActive(long MeasureIndividualId, string IsActive)
        {
            try
            {
                if (MeasureIndividualId > 0)
                {

                    DSPQRS dsMeasureIndi = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureIndividual(MeasureIndividualId);
                    dsMeasureIndi = obj.Data;
                    if (dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];
                        dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = IsActive.Equals("1") ? true : false;

                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureIndividual(dsMeasureIndi);
                        string successMsg;
                        if (objMG.Data != null)
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string updatePQRS_MeasureGroupIsActive(string MeasureGroupId, string IsActive)
        {
            try
            {
                if (Int64.Parse( MeasureGroupId) > 0)
                {

                    DSPQRS dsMeasureIndi = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureGroupDetails(Int64.Parse(MeasureGroupId));
                    dsMeasureIndi = obj.Data;
                    if (dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];
                        dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = IsActive.Equals("1") ? true : false;
                        dr[dsMeasureIndi.IndividualReporting.PerformanceYearColumn.ColumnName] = dr[dsMeasureIndi.IndividualReporting.SubmissionYearColumn.ColumnName];
                        if (dr[dsMeasureIndi.IndividualReporting.CQMMeasureIdsColumn.ColumnName].ToString() != "" && dr[dsMeasureIndi.IndividualReporting.IAMeasureIdsColumn.ColumnName].ToString() != "")
                            dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = dr[dsMeasureIndi.IndividualReporting.CQMMeasureIdsColumn.ColumnName].ToString() +","+ dr[dsMeasureIndi.IndividualReporting.IAMeasureIdsColumn.ColumnName];
                        else if (dr[dsMeasureIndi.IndividualReporting.CQMMeasureIdsColumn.ColumnName].ToString() != "")
                            dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = dr[dsMeasureIndi.IndividualReporting.CQMMeasureIdsColumn.ColumnName].ToString();
                        else
                            dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName]= dr[dsMeasureIndi.IndividualReporting.IAMeasureIdsColumn.ColumnName];
                        dr[dsMeasureIndi.IndividualReporting.ProviderMembersColumn.ColumnName] = dr[dsMeasureIndi.IndividualReporting.ProviderIdsColumn .ColumnName];

                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureGroupData(dsMeasureIndi);
                        string successMsg;
                        if (objMG.Data != null)
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

        internal string deletePQRS_MeasureIndividual(long MeasureIndividualId)
        {
            try
            {
                if (MeasureIndividualId < 0)
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
                    BLObject<string> obj = PQRSHelperObj.deleteMeasureIndividual(MeasureIndividualId);
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

        internal string deletePQRS_MeasureGroup(string MeasureGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty( MeasureGroupId))
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
                    BLObject<string> obj = PQRSHelperObj.deleteMeasureGroupData(MeasureGroupId);
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

        internal string fillPQRS_MeasureIndividual(long MeasureIndividualId)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.fillMeasureIndividual(MeasureIndividualId);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.IndividualReporting.TableName].Rows.Count > 0)
                {
                    List<Model.QRDA.PQRS_IndividualReportingFillModel> modelList = new List<Model.QRDA.PQRS_IndividualReportingFillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.IndividualReporting.TableName].Rows

                                 select new Model.QRDA.PQRS_IndividualReportingFillModel
                                 {
                                     MeasureIndividualId = row["MeasureIndividualId"].ToString(),
                                     SubmissionYear = row["SubmissionYear"].ToString(),
                                     ProviderId = row["ProviderId"].ToString(),
                                     MeasureIds = row["MeasureIds"].ToString(),
                                     SpecialityId = row["SpecialityId"].ToString(),
                                     PracticeIds = row["PracticeIds"].ToString(),
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                     IsReported = Convert.ToBoolean(row["IsReported"]),
                                     CQMMeasureIds = row["CQMMeasureIds"].ToString(),
                                     VBPMeasureIds = row["VBPMeasureIds"].ToString(),
                                     IAMeasureIds = row["IAMeasureIds"].ToString()
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        measureIndividualList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string fillPQRS_MeasureGroupdetails(long MeasureGroupId)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.fillMeasureGroupDetails(MeasureGroupId);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.IndividualReporting.TableName].Rows.Count > 0)
                {
                    List<Model.QRDA.PQRS_IndividualReportingFillModel> modelList = new List<Model.QRDA.PQRS_IndividualReportingFillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.IndividualReporting.TableName].Rows

                                 select new Model.QRDA.PQRS_IndividualReportingFillModel
                                 {
                                     MeasureGroupId = row["MeasureGroupId"].ToString(),
                                     PerformanceYear = row["SubmissionYear"].ToString(),
                                     ProviderId = row["ProviderIds"].ToString(),
                                    
                                     MeasureIds = row["MeasureIds"].ToString(),
                                     PracticeIds = row["PracticeIds"].ToString(),
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                     IsReported = Convert.ToBoolean(row["IsReported"]),
                                     CQMMeasureIds = row["CQMMeasureIds"].ToString(),
                                     VBPMeasureIds = row["VBPMeasureIds"].ToString(),
                                     IAMeasureIds = row["IAMeasureIds"].ToString()
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        measureIndividualList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        internal string fillPQRS_MeasureGroupData(PQRS_IndividualReportingFillModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.fillMeasureGroupData(model.MeasureGroupId, model.ProviderId , model.is_Active, model.PageNumber, model.RowspPage);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.MeasureGroups.TableName].Rows.Count > 0)
                {
                    List<Model.QRDA.PQRS_IndividualReportingFillModel> modelList = new List<Model.QRDA.PQRS_IndividualReportingFillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.MeasureGroups.TableName].Rows
                                     //MeasureGroupId	GroupName	Providers	CreatedOn	TotalProviders	RecordCount
                                 select new Model.QRDA.PQRS_IndividualReportingFillModel
                                 {
                                     MeasureGroupId = row["MeasureGroupId"].ToString(),
                                     GroupName = row["GroupName"].ToString(),
                                     MemberProvider = row["Providers"].ToString(),
                                      CreatedOn= row["CreatedOn"].ToString(),
                                     TotalProviders = row["TotalProviders"].ToString(),                                  
                                     IsActive = Convert.ToBoolean(row["IsActive"]),
                                 }).ToList();

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RecordCount = DSPQRS.MeasureGroups.Rows.Count.ToString(),
                        iTotalDisplayRecords = DSPQRS.MeasureGroups.Rows[0][DSPQRS.MeasureGroups.RecordCountColumn.ColumnName],
                        measureGroupList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        RecordCount=0,
                        iTotalDisplayRecords=0,
                        measureGroupList_JSON="",
                        status = true,
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

        internal string savePQRS_MeasureIndividual(Model.QRDA.PQRS_IndividualReportingFillModel modelfill)
        {
            try
            {
                DSPQRS dsMeasureIndi = new DSPQRS();
                DSPQRS.IndividualReportingRow dr = dsMeasureIndi.IndividualReporting.NewIndividualReportingRow();
                dr[dsMeasureIndi.IndividualReporting.MeasureIndividualIdColumn.ColumnName] = -1;
                dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = modelfill.IsActive;
                dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = modelfill.SpecialityId;
                dr[dsMeasureIndi.IndividualReporting.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;
                dr[dsMeasureIndi.IndividualReporting.ProviderIdColumn.ColumnName] = modelfill.ProviderId;
                dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = modelfill.MeasureIds;
                dr[dsMeasureIndi.IndividualReporting.SubmissionYearColumn.ColumnName] = modelfill.SubmissionYear;


                dr.EntityId = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsMeasureIndi.IndividualReporting.AddIndividualReportingRow(dr);


                #region Database Insertion
                BLObject<DSPQRS> obj = PQRSHelperObj.insertMeasureIndividual(dsMeasureIndi);
                dsMeasureIndi = obj.Data;
                
                if (obj.Data != null)
                {
                    DataRow dr1 = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];
                    var response = new
                    {
                        MeasureIndividualId = dr1[dsMeasureIndi.IndividualReporting.MeasureIndividualIdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string savePQRS_MeasureGroup(Model.QRDA.PQRS_IndividualReportingFillModel modelfill)
        {
            try
            {
                DSPQRS dsMeasureIndi = new DSPQRS();
                DSPQRS.IndividualReportingRow dr = dsMeasureIndi.IndividualReporting.NewIndividualReportingRow();
                dr[dsMeasureIndi.IndividualReporting.MeasureGroupIdColumn.ColumnName] = modelfill.MeasureGroupId;
                dr[dsMeasureIndi.IndividualReporting.MeasureIndividualIdColumn.ColumnName] = -1;
                dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = modelfill.IsActive;
                if(modelfill.SpecialityId !=null)
                dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = modelfill.SpecialityId;
                else
                    dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = DBNull.Value;
                    dr[dsMeasureIndi.IndividualReporting.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;

                //dr[dsMeasureIndi.IndividualReporting.ProviderIdColumn.ColumnName] = modelfill.ProviderId;
                dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = modelfill.MeasureIds;
                dr[dsMeasureIndi.IndividualReporting.MeasureGroupNameColumn .ColumnName] = modelfill.MeasureGroupId_text;
                dr[dsMeasureIndi.IndividualReporting.PerformanceYearColumn.ColumnName] = modelfill.PerformanceYear;
                dr[dsMeasureIndi.IndividualReporting.ProviderMembersColumn .ColumnName] = modelfill.MemberProvider;

                dr.EntityId = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsMeasureIndi.IndividualReporting.AddIndividualReportingRow(dr);


                #region Database Insertion
                BLObject<DSPQRS> obj = PQRSHelperObj.insertMeasureGroupData(dsMeasureIndi);
                dsMeasureIndi = obj.Data;

                if (obj.Data != null)
                {
                    DataRow dr1 = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];
                    var response = new
                    {
                        MeasureGroupId = dr1[dsMeasureIndi.IndividualReporting.MeasureGroupIdColumn.ColumnName],
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

        internal string updatePQRS_MeasureIndividual(Model.QRDA.PQRS_IndividualReportingFillModel modelfill)
        {
            try
            {
                if (MDVUtility.ToInt32(modelfill.MeasureIndividualId) > 0)
                {

                    DSPQRS dsMeasureIndi = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureIndividual(MDVUtility.ToInt32(modelfill.MeasureIndividualId));
                    dsMeasureIndi = obj.Data;
                    if (dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];
                        dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = modelfill.IsActive;
                        dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = modelfill.SpecialityId;
                        dr[dsMeasureIndi.IndividualReporting.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;
                        dr[dsMeasureIndi.IndividualReporting.ProviderIdColumn.ColumnName] = modelfill.ProviderId;
                        dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = string.IsNullOrEmpty(modelfill.MeasureIds) ? null: modelfill.MeasureIds;
                        dr[dsMeasureIndi.IndividualReporting.SubmissionYearColumn.ColumnName] = modelfill.SubmissionYear;
                        dr[dsMeasureIndi.IndividualReporting.EntityIdColumn.ColumnName] = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));
                        dr[dsMeasureIndi.IndividualReporting.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsMeasureIndi.IndividualReporting.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureIndividual(dsMeasureIndi);

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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string updatePQRS_MeasureGroup(Model.QRDA.PQRS_IndividualReportingFillModel modelfill)
        {
            try
            {
                if (MDVUtility.ToInt32(modelfill.MeasureGroupId) > 0)
                {

                    DSPQRS dsMeasureIndi = null;
                    BLObject<DSPQRS> obj = PQRSHelperObj.fillMeasureGroupDetails(MDVUtility.ToInt32(modelfill.MeasureGroupId));
                    dsMeasureIndi = obj.Data;
                    if (dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMeasureIndi.Tables[dsMeasureIndi.IndividualReporting.TableName].Rows[0];

                       // dr[dsMeasureIndi.IndividualReporting.MeasureGroupIdColumn.ColumnName] =Int64.Parse( modelfill.MeasureGroupId_text);
                        dr[dsMeasureIndi.IndividualReporting.MeasureIndividualIdColumn.ColumnName] = -1;
                        dr[dsMeasureIndi.IndividualReporting.IsActiveColumn.ColumnName] = modelfill.IsActive;
                        if(modelfill.SpecialityId != null)
                dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = modelfill.SpecialityId;
                else
                    dr[dsMeasureIndi.IndividualReporting.SpecialityIdColumn.ColumnName] = DBNull.Value;

                        dr[dsMeasureIndi.IndividualReporting.PracticeIdsColumn.ColumnName] = modelfill.PracticeIds;
                        dr[dsMeasureIndi.IndividualReporting.ProviderMembersColumn.ColumnName] = modelfill.MemberProvider;
                        dr[dsMeasureIndi.IndividualReporting.MeasureIdsColumn.ColumnName] = string.IsNullOrEmpty(modelfill.MeasureIds) ? null : modelfill.MeasureIds;
                        dr[dsMeasureIndi.IndividualReporting.PerformanceYearColumn.ColumnName] = modelfill.PerformanceYear;
                        dr[dsMeasureIndi.IndividualReporting.EntityIdColumn.ColumnName] = MDVUtility.ToInt32(modelfill.EntityId > 0 ? modelfill.EntityId : MDVUtility.ToInt32(MDVSession.Current.EntityId));
                        dr[dsMeasureIndi.IndividualReporting.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsMeasureIndi.IndividualReporting.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        BLObject<DSPQRS> objMG = PQRSHelperObj.UpdateMeasureGroupData(dsMeasureIndi);

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
        #endregion

        #region Reports PQRS


        internal string generateIndividualReport(PQRSReportsModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.generateIndividualReport(MDVUtility.ToInt64(model.ProviderId), model.ReportFromDate, model.ReportToDate);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count > 0)
                {
                    List<PQRSReports_FillModel> modelList = new List<PQRSReports_FillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows

                                 select new PQRSReports_FillModel
                                 {
                                     measureId = row["MeasureId"].ToString(),
                                     measureNumber = row["MeasureNumber"].ToString(),
                                     measureName = row["MeasureName"].ToString(),
                                     totalPatients = row["TotalPatients"].ToString(),
                                     numerator = row["Numerator"].ToString(),
                                     denuminator = row["Denominator"].ToString(),
                                     performanceMet = row["PerformanceMet"].ToString(),
                                     performanceRate = MDVUtility.ToLong(row["PerformanceRate"]),
                                     performanceNotMet = row["PerformanceNotMet"].ToString(),
                                     exclusion = row["Exclusion"].ToString(),
                                     reportingRate = MDVUtility.ToLong(row["ReportingRate"]),
                                     nonCompliantPatients = row["NonCompliantPatients"].ToString(),
                                     Patients = MDVUtility.ToStr(row["Patients"]),
                                     PMPatients = MDVUtility.ToStr(row["PMPatients"]),
                                     NonCompliantVisitsList = MDVUtility.ToStr(row["NonCompliantVisitsList"])
                                 }).ToList();


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        pqrsReportCount = DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count,
                        ProviderName = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.ProviderNameColumn.ColumnName],
                        TIN = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.TINColumn.ColumnName],
                        NPI = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.NPIColumn.ColumnName],
                        pqrsReportList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        pqrsReportCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        internal string generateGPROReport(PQRSReportsModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.generateGPROReport(MDVUtility.ToInt64(model.ProviderGroupId), model.ReportFromDate, model.ReportToDate);
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count > 0)
                {
                    List<PQRSReports_FillModel> modelList = new List<PQRSReports_FillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows

                                 select new PQRSReports_FillModel
                                 {
                                     measureId = row["MeasureNumber"].ToString(),
                                     measureName = row["MeasureName"].ToString(),
                                     totalPatients = row["TotalPatients"].ToString(),
                                     numerator = row["Numerator"].ToString(),
                                     denuminator = row["Denominator"].ToString(),
                                     performanceMet = row["PerformanceMet"].ToString(),
                                     performanceRate = MDVUtility.ToLong(row["PerformanceRate"]),
                                     performanceNotMet = row["PerformanceNotMet"].ToString(),
                                     exclusion = row["Exclusion"].ToString(),
                                     reportingRate = MDVUtility.ToLong(row["ReportingRate"]),
                                     nonCompliantPatients = row["NonCompliantPatients"].ToString()
                                 }).ToList();


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        gproReportCount = DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count,
                        ProviderName = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.ProviderNameColumn.ColumnName],
                        TIN = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.TINColumn.ColumnName],
                        NPI = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.NPIColumn.ColumnName],
                        gproReportList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        pqrsReportCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        internal string generateSummaryReport(PQRSReportsModel model)
        {
            try
            {

                DSPQRS DSPQRS = null;
                BLObject<DSPQRS> obj;
                obj = PQRSHelperObj.generateSummaryReport(MDVUtility.ToInt64(model.SummaryProviderId), MDVUtility.ToInt64(model.SummaryProviderGroupId));
                DSPQRS = obj.Data;

                if (obj.Data != null && DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count > 0)
                {
                    List<PQRSReports_FillModel> modelList = new List<PQRSReports_FillModel>();
                    modelList = (from DataRow row in DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows

                                 select new PQRSReports_FillModel
                                 {
                                     measureId = row["MeasureNumber"].ToString(),
                                     measureName = row["MeasureName"].ToString(),
                                     totalPatients = row["TotalPatients"].ToString(),
                                     numerator = row["Numerator"].ToString(),
                                     denuminator = row["Denominator"].ToString(),
                                     performanceMet = row["PerformanceMet"].ToString(),
                                     performanceRate = MDVUtility.ToLong(row["PerformanceRate"]),
                                     performanceNotMet = row["PerformanceNotMet"].ToString(),
                                     exclusion = row["Exclusion"].ToString(),
                                     reportingRate = MDVUtility.ToLong(row["ReportingRate"]),
                                     nonCompliantPatients = row["NonCompliantPatients"].ToString()
                                 }).ToList();


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        summaryReportCount = DSPQRS.Tables[DSPQRS.PQRSReports.TableName].Rows.Count,
                        ProviderName = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.ProviderNameColumn.ColumnName],
                        TIN = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.TINColumn.ColumnName],
                        NPI = DSPQRS.PQRSReports.Rows[0][DSPQRS.PQRSReports.NPIColumn.ColumnName],
                        summaryReportList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        pqrsReportCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        /// <summary>
        /// Author: Ahmad Raza
        /// Date: 19-08-2016
        /// FunctionName: saveMissingData
        /// Description: Saves Missing Data of PatientList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal string saveMissingData(PQRSPatientListModel model)
        {
            try
            {
                DSPQRS dsPatientList = new DSPQRS();

                foreach (var item in model.missingDataList)
                {
                    DSPQRS.PatientListRow dr = dsPatientList.PatientList.NewPatientListRow();
                    dr[dsPatientList.PatientList.IdColumn.ColumnName] = -1;
                    dr[dsPatientList.PatientList.PatientIdColumn.ColumnName] = model.PatientId;
                    if (!string.IsNullOrEmpty(item.TreatmentType))
                        dr[dsPatientList.PatientList.TreatmentTypeIdColumn.ColumnName] = item.TreatmentType;
                    else
                        dr[dsPatientList.PatientList.TreatmentTypeIdColumn.ColumnName] = DBNull.Value;
                    if (!string.IsNullOrEmpty(item.ReasonType))
                        dr[dsPatientList.PatientList.ReasonTypeIdColumn.ColumnName] = item.ReasonType;
                    else
                        dr[dsPatientList.PatientList.ReasonTypeIdColumn.ColumnName] = DBNull.Value;
                    if (!string.IsNullOrEmpty(item.ReasonComments))
                        dr[dsPatientList.PatientList.ReasonCommentsColumn.ColumnName] = item.ReasonComments;
                    else
                        dr[dsPatientList.PatientList.ReasonCommentsColumn.ColumnName] = DBNull.Value;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dsPatientList.PatientList.AddPatientListRow(dr);
                }

                #region Database Insertion
                BLObject<DSPQRS> obj = PQRSHelperObj.insertPatientList(dsPatientList);
                dsPatientList = obj.Data;
                DataRow dr1 = dsPatientList.Tables[dsPatientList.PatientList.TableName].Rows[0];
                if (obj.Data != null)
                {
                    var response = new
                    {
                        PatientListId = dr1[dsPatientList.PatientList.IdColumn.ColumnName],
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }



    }
}