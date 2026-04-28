/*  
    Author: Muhammad Azhar Shahzad
    Creation Date: Dec 16,2015
    OverView:This File Is created for History Summary Controller
*/
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using System.Data;
using MDVision.Model.Clinical.History.HistorySummary;
using Newtonsoft.Json;

namespace MDVision.IEHR.EMR.Helpers.Clinical.History
{
    public class HistorySummaryHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public HistorySummaryHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static HistorySummaryHelper _instance = null;
        public static HistorySummaryHelper Instance()
        {
            if (_instance == null)
                _instance = new HistorySummaryHelper();
            return _instance;
        }

        /// <summary>
        /// This Functoin will get Soap text for History summary against Patient Id
        /// Created: Dec 16,2015
        /// Author: Muhammad Azhar Shahzad
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string getHistorySummarySoapByPatientid_Obsolete(HistorySummary model)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> obj;
                obj = BLLClinicalObj.loadHistorySummaryForSoap_Obsolete(model.PatientId, MDVUtility.ToInt64(model.NoteId));
                dsHistorySummarySoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsHistorySummarySoap.Tables[dsHistorySummarySoap.HistorySummary_Soap.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            HistorySummarySoapCount = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HistorySummary_Soap.TableName].Rows.Count,
                            HistorySummarySoap_JSON = MDVUtility.JSON_DataTable(dsHistorySummarySoap.Tables[dsHistorySummarySoap.HistorySummary_Soap.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HistorySummarySoapCount = 0,
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

        public string getHistorySummarySoapByPatientid(HistorySummary model)
        {
            try
            {
                List<HistorySummary_Soap> historySummary_SoapList = BLLClinicalObj.loadHistorySummaryForSoap(model.PatientId, MDVUtility.ToInt64(model.NoteId));

                if (historySummary_SoapList != null)
                {
                    if (historySummary_SoapList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            HistorySummarySoapCount = historySummary_SoapList.Count,
                            HistorySummarySoap_JSON = JsonConvert.SerializeObject(historySummary_SoapList),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HistorySummarySoapCount = 0,
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
                        Message = "No record found."
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

        public string getHxLog(HistorySummary model)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> obj;
                obj = BLLClinicalObj.loadHxLog(model.HxId, model.HxType, model.Status, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsHistorySummarySoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            HxLogSoapCount = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count,
                            HxLogSoap_JSON = MDVUtility.JSON_DataTable(dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName]),
                            iTotalDisplayRecords = (dsHistorySummarySoap.HxLog.Rows.Count > 0) ? dsHistorySummarySoap.HxLog.Rows[0][dsHistorySummarySoap.HxLog.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HxLogSoapCount = 0,
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

        public string getAutoPopulateSetting(HistorySummary model)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.LoadAutoPopulateOptions(MDVUtility.ToInt64(model.UserId), model.ComponentType, MDVUtility.ToInt64(model.EntityId));
                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.NoteAutoPopulateOptions.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsNotes.Tables[dsNotes.NoteAutoPopulateOptions.TableName].Rows[0];
                        var keyValues = new Dictionary<string, string>
                        {

                            { "chkFamilyHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsFamilyHistoryColumn.ColumnName])},
                            { "chkBirthHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsBirthHistoryColumn.ColumnName])},
                            { "chkSocialHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsSocialHistoryColumn.ColumnName])},
                            { "chkHospitalHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsHospitalizationHistoryColumn.ColumnName])},
                            { "chkMedicalHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsMedicalHistoryColumn.ColumnName])},
                            { "chkSurgicalHistory", MDVUtility.ToStr(dr[dsNotes.NoteAutoPopulateOptions.IsSurgicalHistoryColumn.ColumnName])},

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            AutoPopulateOptionsCount = dsNotes.Tables[dsNotes.NoteAutoPopulateOptions.TableName].Rows.Count,
                            AutoPopulateOptionsLoad_JSON = js.Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EntityGroupCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string UpdateHistoryTabFromNotes(HistorySummary model)
        {
            string response = string.Empty;
            if (model.MedicalHxList != null)
            {
                MedicalHxHelper helperMedicalHx = new MedicalHxHelper();
                long MedicalHxId = MDVUtility.ToInt64(model.MedicalHxList.MedicalHxId);
                long noteId = MDVUtility.ToInt64(model.NoteId);
                if (MedicalHxId > 0)
                {
                    response = helperMedicalHx.updateMedicalHx(model.MedicalHxList, noteId);                  
                }
                else
                {
                    response = helperMedicalHx.saveMedicalHx(model.MedicalHxList, noteId);                        
                }
            }

            if (model.FamilyHxList != null)
            {
                FamilyHxHelper helperFamilyHx = new FamilyHxHelper();
                long FamilyHxId = MDVUtility.ToInt64(model.FamilyHxList.FamilyHxId);
                long noteId = MDVUtility.ToInt64(model.NoteId);
                if (FamilyHxId > 0)
                {
                    response = helperFamilyHx.updateFamilyHx(model.FamilyHxList, noteId);
                }
                else
                {
                    response = helperFamilyHx.saveFamilyHx(model.FamilyHxList, noteId);
                }
            }
            if (model.SurgicalHxList != null)
            {
                SurgicalHxHelper helperSurgicalHx = new SurgicalHxHelper();
                SurgicalHxModel modelSurgical = helperSurgicalHx.saveSurgicalHx(model.SurgicalHxList, "", model.SurgicalHxList.SurgicalDiseaseList, MDVUtility.ToLong(model.NoteId));
            }
            if (model.BirthHxList != null)
            {
                BirthHxHelper helperBirthHx = new BirthHxHelper();
                string Surgical = helperBirthHx.saveBirthHx(model.BirthHxList);
            }
            if (model.HospitalizationHxList != null)
            {
                HospitalizationHxHelper helperHospitalizationHx = new HospitalizationHxHelper();
                long HospitalizationHxId = 0;

                HospitalizationHxId = MDVUtility.ToInt64(model.HospitalizationHxList.HospitalizationHxId);
                if (HospitalizationHxId > 0)
                {                  
                    string HospitalizationSave = helperHospitalizationHx.saveHospitalizationHx(model.HospitalizationHxList, MDVUtility.ToInt64(model.NoteId));
                }
                else
                {
                    string HospitalizationSave = helperHospitalizationHx.saveHospitalizationHx(model.HospitalizationHxList, MDVUtility.ToInt64(model.NoteId));
                }
            }
            if (model.SocialHxList != null)
            {
                SocialHxHelper helperSocialHx = new SocialHxHelper();
                response = helperSocialHx.saveSocialHx(model.SocialHxList, "", model.SocialHxList.lstTobaccoModel, model.SocialHxList.lstAlcoholModel, model.SocialHxList.lstDrugAbuseModel, model.SocialHxList.lstSexualHxModel, model.SocialHxList.lstOccupationHxModel, model.SocialHxList.lstCaffeineIntakHxModel, model.SocialHxList.lstSleepHxModel, model.SocialHxList.lstExercisesHxModel, model.SocialHxList.lstHousingHxModel, MDVUtility.ToInt64(model.NoteId));

            }
            if (model.SocPsyandBehaviorHx != null)
            {
                SocPsyandBehaviorHxHelper helperSocPsyandBehaviorHx = new SocPsyandBehaviorHxHelper();
                model.SocPsyandBehaviorHx.NotesId = model.NoteId;
                response = helperSocPsyandBehaviorHx.SocPsyandBehaviorHxSave(model.SocPsyandBehaviorHx);
            }
            return HistorySummaryHelper.Instance().getHistorySummarySoapByPatientid_Obsolete(model);
        }

        // Created By:  Farooq Ahmad
        // Created Date: 07/09/2016
        //OverView: Methods "UpdateNotesHxtabOrder"
        public string UpdateNotesHxtabOrder(long NoteId, string HxtabOrder)
        {
            try
            {
                int obj;
                obj = BLLClinicalObj.UpdateNotesHxtabOrder(NoteId, HxtabOrder);
                var response = new
                {
                    status = true
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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