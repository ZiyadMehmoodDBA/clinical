/* Author:  Muhammad Arshad
 * Created Date: 26/02/2016
 * OverView: Created to handel Physical Exam Template
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.PhysicalExam;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.Templates;
using MDVision.Model.Clinical.BillingInformation;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Clinical.Medical.ProblemLists;
using System.Web.Configuration;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class BillingInformationHelper
    {
        private BLLClinical BLLClinicalObj = null;

        public BillingInformationHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static BillingInformationHelper _instance = null;
        public static BillingInformationHelper Instance()
        {
            if (_instance == null)
                _instance = new BillingInformationHelper();
            return _instance;
        }

        public long GetAppointmentIdByNoteId(long NoteId)
        {
            try
            {
                var AppointmentId = BLLClinicalObj.GetAppointmentIdByNoteId(NoteId);
                return AppointmentId;
            }
            catch (Exception ex) { throw ex; }

        }



        public DSBillingInformationLookup Get_LookupBillingInfoTime()
        {
            try
            {
                DSBillingInformationLookup ds = new DSBillingInformationLookup();
                BLObject<DSBillingInformationLookup> obj = BLLClinicalObj.LookupBillingInfoTime();
                if (obj.Data != null)
                {
                    ds = obj.Data;
                }
                return ds;
            }
            catch (Exception ex) { throw ex; }
        }


        public string BillingInfo_Save(BillingInformationModel model)
        {


            try
            {
                DSBillingInformation dsDSBillingInformation = new DSBillingInformation();
                if (string.IsNullOrEmpty(model.BillingInfoId))
                {
                    model.BillingInfoId = "-1";
                }
                BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                dsDSBillingInformation = obj.Data;
                if (obj.Data != null)
                {
                    DSBillingInformation.BillingInfoRow cdsRow = null;
                    DSBillingInformation.BillingInfoRow[] cdsRows = (DSBillingInformation.BillingInfoRow[])dsDSBillingInformation.BillingInfo.Select(dsDSBillingInformation.BillingInfo.BillingInfoIdColumn + "=" + model.BillingInfoId);
                    if (cdsRows.Length > 0)
                    {
                        cdsRow = cdsRows[0];
                    }
                    else
                    {
                        cdsRow = dsDSBillingInformation.BillingInfo.NewBillingInfoRow();
                    }
                    if (cdsRow != null)
                    {
                        if (!string.IsNullOrEmpty(model.ENMTypeId))
                            cdsRow.ENMTypeId = MDVUtility.ToInt32(model.ENMTypeId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ENMTimeId))
                            cdsRow.ENMTimeId = MDVUtility.ToInt32(model.ENMTimeId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ENMCPTCode))
                            cdsRow.ENMCPTCode = model.ENMCPTCode;
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ENMCPTDescription))
                            cdsRow.ENMCPTDescription = model.ENMCPTDescription;
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.ENMCPTUnit))
                            cdsRow.ENMCPTUnit = MDVUtility.ToDouble(model.ENMCPTUnit);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ENMCPTDOSFrom))
                            cdsRow.ENMCPTDOSFrom = MDVUtility.ToDateTime(model.ENMCPTDOSFrom);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ENMCPTDOSTo))
                            cdsRow.ENMCPTDOSTo = MDVUtility.ToDateTime(model.ENMCPTDOSTo);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.AdmissionDate))
                            cdsRow.AdmissionDate = MDVUtility.ToDateTime(model.AdmissionDate);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.AdmissionDateColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.DischargeDate))
                            cdsRow.DischargeDate = MDVUtility.ToDateTime(model.DischargeDate);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.DischargeDateColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.PatientId))
                            cdsRow.PatientId = MDVUtility.ToInt64(model.PatientId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.PatientIdColumn.ColumnName] = DBNull.Value;

                        if (model.NotesId != 0)
                        {
                            cdsRow[dsDSBillingInformation.BillingInfo.NotesIdColumn.ColumnName] = model.NotesId;
                        }
                        else
                        {
                            cdsRow[dsDSBillingInformation.BillingInfo.NotesIdColumn.ColumnName] = DBNull.Value;
                        }
                        // cdsRow.NotesId = model.NotesId;

                        if (!string.IsNullOrEmpty(model.VisitId))
                            cdsRow.VisitId = MDVUtility.ToInt64(model.VisitId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.VisitIdColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ProviderId))
                            cdsRow.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ProviderIdColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.Status))
                            cdsRow.Status = model.Status;
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.StatusColumn.ColumnName] = DBNull.Value;


                        if (!string.IsNullOrEmpty(model.BillingInfoType))
                            cdsRow.BillingInfoType = MDVUtility.ToStr(model.BillingInfoType);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.BillingInfoTypeColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.FacilityId))
                            cdsRow.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.FacilityIdColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ResourceProviderId))
                            cdsRow.ResourceProviderId = MDVUtility.ToInt64(model.ResourceProviderId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.RefProviderId))
                            cdsRow.RefProviderId = MDVUtility.ToInt64(model.RefProviderId);
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.RefProviderIdColumn.ColumnName] = DBNull.Value;

                        cdsRow.IsActive = true;



                        cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.ModifiedOn = DateTime.Now;

                        cdsRow.SoapText = model.SoapText;

                        if (cdsRows.Length == 0)
                        {
                            cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsRow.CreatedOn = DateTime.Now;
                        }
                        cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cdsRow.ModifiedOn = DateTime.Now;

                        if (cdsRows.Length < 1)
                        {
                            dsDSBillingInformation.BillingInfo.AddBillingInfoRow(cdsRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<BLLBillingInformationModel> objBillingInfo = BLLClinicalObj.BillingInformationSave(dsDSBillingInformation, model, Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]));
                    if (objBillingInfo.Data != null)
                    {
                        DSProblemLists dsProblemList = new DSProblemLists(); ;
                        try
                        {
                            var objProblem = BLLClinicalObj.LoadProblemListsOp_Obsolete(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId));
                            if (objProblem.Data != null)
                            {
                                dsProblemList = objProblem.Data;
                            }
                        }
                        catch (Exception ex) { }

                        DSProcedures dsProcedure = new DSProcedures();
                        try
                        {
                            var objProcedure = BLLClinicalObj.loadProcedures_Obsolete(0, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "0", "", 1, 1000, "", "", "0", "");
                            if (objProcedure.Data != null)
                            {
                                dsProcedure = objProcedure.Data;
                            }
                        }
                        catch (Exception ex) { }

                        List<BillingInfoCPTModel> cptlist = BLLClinicalObj.GetProviderCPTsForBillingInfo(MDVUtility.ToLong(model.BillingInfoId), MDVUtility.ToLong(model.ProviderId));                        

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();                       

                        var CPTResponse = new
                        {
                            status = !string.IsNullOrEmpty(objBillingInfo.Data.CPTS_JSON) ? true : false,
                            message = !string.IsNullOrEmpty(objBillingInfo.Data.CPTS_JSON) ? Common.AppPrivileges.Save_Message : Common.AppPrivileges.Select_Error_Message,
                            BillingInfoId = objBillingInfo.Data.BillingInfoId,
                            CPTSListFill_JSON = objBillingInfo.Data.CPTS_JSON
                        };

                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BillingInfoId = objBillingInfo.Data.BillingInfoId,
                            BillingInfoFillJson = BillingInfo_SELECT(0, 0, objBillingInfo.Data.BillingInfoId),
                            ProblemList = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemList.TableName]),
                            Procedure = MDVUtility.JSON_DataTable(dsProcedure.Tables[dsProcedure.Procedures.TableName]),
                            CPTICDResponse = Newtonsoft.Json.JsonConvert.SerializeObject(CPTResponse),
                            ProviderCPTs = js.Serialize(cptlist)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objBillingInfo.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
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



        public string Signed_BillingInfo(BillingInformationModel model)
        {


            try
            {
                DSBillingInformation dsDSBillingInformation = new DSBillingInformation();
                if (string.IsNullOrEmpty(model.BillingInfoId))
                {
                    model.BillingInfoId = "-1";
                }
                BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                dsDSBillingInformation = obj.Data;
                if (obj.Data != null)
                {
                    DSBillingInformation.BillingInfoRow cdsRow = null;
                    DSBillingInformation.BillingInfoRow[] cdsRows = (DSBillingInformation.BillingInfoRow[])dsDSBillingInformation.BillingInfo.Select(dsDSBillingInformation.BillingInfo.BillingInfoIdColumn + "=" + model.BillingInfoId);
                    if (cdsRows.Length > 0)
                    {
                        cdsRow = cdsRows[0];
                    }
                    if (cdsRow != null)
                    {
                        if (!string.IsNullOrEmpty(model.Status))
                            cdsRow.Status = model.Status;
                        else
                            cdsRow[dsDSBillingInformation.BillingInfo.StatusColumn.ColumnName] = DBNull.Value;

                    }

                    #region Database Insertion/Updation
                    BLObject<DSBillingInformation> objCDS = BLLClinicalObj.BillingInfo_Save(dsDSBillingInformation, model.PatientId);
                    dsDSBillingInformation = objCDS.Data;

                    if (objCDS.Data != null)
                    {
                        long insertedBillingInfoId = MDVUtility.ToInt32(model.BillingInfoId);
                        if (dsDSBillingInformation.Tables[dsDSBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                            insertedBillingInfoId = MDVUtility.ToInt64(dsDSBillingInformation.Tables[dsDSBillingInformation.BillingInfo.TableName].Rows[0][dsDSBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);

                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            BillingInfoId = insertedBillingInfoId,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCDS.Message.Contains("duplicate") ? "CDS Title already exists" : obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
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

        public string BillingInfo_DELETE(string BillingInfoId)
        {
            DSBillingInformationLookup ds = new DSBillingInformationLookup();
            try
            {
                BLObject<string> obj = BLLClinicalObj.BillingInfo_DELETE(BillingInfoId);
                if (obj.Data == "")
                {
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForCognitive(MDVUtility.ToInt64(cognitiveId));
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
                MDVLogger.BLLErrorLog("BLLClinical::BillingInfo_DELETE", ex);
                return ex.Message;
            }
        }

        public string IsCptExsistsInEsupperbill(long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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

                    BLObject<string> obj = BLLClinicalObj.IsCptExsistsInEsupperbill(NotesId);
                    if (obj.Data != "" && (obj.Data == "0" || obj.Data == "1"))
                    {
                        var response = new
                        {
                            status = true,
                            IsCptExsistsInEsupperbill = obj.Data
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

        public string IsBillingInfoCreated(long NoteId, long PatientId, long BillingInfoId)
        {
            try
            {
                if ((NoteId == 0 && PatientId == 0) && BillingInfoId == 0)
                {
                    var response = new
                    {
                        status = false,
                        IsBillingInfoCreated = false,
                        BillingInfoId = 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSBillingInformation dsBillingInformation = null;
                    BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT(NoteId, PatientId, BillingInfoId);
                    dsBillingInformation = obj.Data;
                    if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];
                        var response = new
                        {
                            status = true,
                            IsBillingInfoCreated = true,
                            BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            IsBillingInfoCreated = false,
                            BillingInfoId = 0
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

        public string BillingInfo_SELECT(long NoteId, long PatientId, long BillingInfoId)
        {
            try
            {
                if ((NoteId == 0 && PatientId == 0) && BillingInfoId == 0)
                {
                    var response = new
                    {
                        status = true,
                        BillingInfoFill_JSON = "[]",
                        BillingInfoICDFill_JSON = "[]",
                        BillingInfoCPTFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSBillingInformation dsBillingInformation = null;
                    BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT(NoteId, PatientId, BillingInfoId);
                    dsBillingInformation = obj.Data;
                    if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];
                        BillingInformationModel result = new BillingInformationModel();
                        result.BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName]);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.SoapTextColumn.ColumnName]);
                        result.POS = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.POSColumn.ColumnName]);
                        result.Facility = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityColumn.ColumnName]);
                        result.FacilityId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityIdColumn.ColumnName]);
                        result.Provider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderColumn.ColumnName]);

                        result.ResourceProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName]);
                        result.ResourceProvider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderColumn.ColumnName]);
                        result.RefProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.RefProviderIdColumn.ColumnName]);
                        result.RefProvider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.RefProviderColumn.ColumnName]);
                        result.AdmissionDate = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.AdmissionDateColumn.ColumnName]);
                        result.DischargeDate = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.DischargeDateColumn.ColumnName]);
                        BLObject<DSBillingInformation> objICD = BLLClinicalObj.BillingInfoICD_SELECT(BillingInfoId);
                        dsBillingInformation.Merge(objICD.Data);
                        BLObject<DSBillingInformation> objCPT = BLLClinicalObj.BillingInfoCPT_SELECT(BillingInfoId);
                        dsBillingInformation.Merge(objCPT.Data);

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            BillingInfoFill_JSON = js.Serialize(result),
                            BillingInfoICDFill_JSON = MDVUtility.JSON_DataTable(dsBillingInformation.Tables[dsBillingInformation.BillingInfoICD.TableName]),
                            BillingInfoCPTFill_JSON = MDVUtility.JSON_DataTable(dsBillingInformation.Tables[dsBillingInformation.BillingInfoCPT.TableName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            BillingInfoFill_JSON = "[]",
                            BillingInfoICDFill_JSON = "[]",
                            BillingInfoCPTFill_JSON = "[]"
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

        public string BillingInfo_SELECT_By_VisitId(long visitId)
        {
            try
            {
                if (visitId == 0)
                {
                    var response = new
                    {
                        status = true,
                        BillingInfoFill_JSON = "[]",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSBillingInformation dsBillingInformation = null;
                    BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT_By_VisitId(visitId);
                    dsBillingInformation = obj.Data;
                    if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];
                        BillingInformationModel result = new BillingInformationModel();
                        result.BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName]);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.SoapTextColumn.ColumnName]);

                        result.Facility = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityColumn.ColumnName]);
                        result.FacilityId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityIdColumn.ColumnName]);
                        result.Provider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderColumn.ColumnName]);

                        result.ResourceProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName]);
                        result.ResourceProvider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderColumn.ColumnName]);

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            BillingInfoFill_JSON = js.Serialize(result),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            BillingInfoFill_JSON = "[]",
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

        public string LoadAttachedProceduresAndProblems(long NoteId, long PatientId)
        {
            try
            {
                if ((NoteId == 0))
                {
                    var response = new
                    {
                        status = true,
                        ProblemListFill_JSON = "[]",
                        ProcedureListFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSProblemLists dSProblemLists = null;
                    DSProcedures dsProcedure = new DSProcedures();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemAndProcedureList(NoteId, PatientId);
                    dSProblemLists = obj.Data;
                    if (dSProblemLists != null)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dSProblemLists.ProblemList.TableName]),
                            ProcedureListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dsProcedure.Procedures.TableName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = "[]",
                            ProcedureListFill_JSON = "[]"
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

        public string LoadAttachedProceduresAndProblemsForSign(long NoteId, long PatientId, long BillingInfoId)
        {
            try
            {
                if ((NoteId == 0))
                {
                    var response = new
                    {
                        status = true,
                        ProblemListFill_JSON = "[]",
                        ProcedureListFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSProblemLists dSProblemLists = null;
                    DSProcedures dsProcedure = new DSProcedures();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemAndProcedureListForSign(NoteId, PatientId, BillingInfoId);
                    dSProblemLists = obj.Data;
                    if (dSProblemLists != null)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dSProblemLists.ProblemList.TableName]),
                            ProcedureListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dsProcedure.Procedures.TableName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = "[]",
                            ProcedureListFill_JSON = "[]"
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

        public string LoadProceduresAndProblems(long NoteId, long PatientId)
        {
            try
            {
                if ((NoteId == 0))
                {
                    var response = new
                    {
                        status = true,
                        ProblemListFill_JSON = "[]",
                        ProcedureListFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSProblemLists dSProblemLists = null;
                    DSProcedures dsProcedure = new DSProcedures();
                    BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemsAndProcedures(NoteId, PatientId);
                    dSProblemLists = obj.Data;
                    if (dSProblemLists != null)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dSProblemLists.ProblemList.TableName]),
                            ProcedureListFill_JSON = MDVUtility.JSON_DataTable(dSProblemLists.Tables[dsProcedure.Procedures.TableName]),
                            ProblemListCount = dSProblemLists.Tables[dSProblemLists.ProblemList.TableName].Rows.Count,
                            ProceduresCount = dSProblemLists.Tables[dsProcedure.Procedures.TableName].Rows.Count,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProblemListFill_JSON = "[]",
                            ProcedureListFill_JSON = "[]"
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
        public string InsertMissingBillingCpt(long NoteId, long billingId)
        {
            try
            {
                if ((NoteId == 0 || billingId == 0))
                {
                    var response = new
                    {
                        status = true,
                        ProcedureListFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    DSProcedures dsProcedure = null;
                    BLObject<DSProcedures> obj = BLLClinicalObj.InsertMissingBillingCpt(NoteId, billingId);
                    dsProcedure = obj.Data;
                    if (dsProcedure != null)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProcedureListFill_JSON = MDVUtility.JSON_DataTable(dsProcedure.Tables[dsProcedure.Procedures.TableName]),
                            ProceduresCount = dsProcedure.Tables[dsProcedure.Procedures.TableName].Rows.Count,
                            BillingInfoFillJson = BillingInfo_SELECT(0, 0, billingId),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ProcedureListFill_JSON = "[]"
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

        //public string BillingInfoICD_Save(List<BillingInfoICDModel> lstModels, string BillingInfoId, string PatientId, long NoteId, bool showCustomFormData)
        //{

        //    try
        //    {
        //        DSBillingInformation dsDSBillingInformation = new DSBillingInformation();
        //        BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfoICD_SELECT(MDVUtility.ToInt64(BillingInfoId));
        //        dsDSBillingInformation = obj.Data;
        //        if (obj.Data != null)
        //        {
        //            //DSBillingInformation.BillingInfoICDRow cdsRow = null;
        //            DSBillingInformation.BillingInfoICDRow[] cdsRows = (DSBillingInformation.BillingInfoICDRow[])dsDSBillingInformation.BillingInfoICD.Select(dsDSBillingInformation.BillingInfoICD.BillingInfoIdColumn + "=" + BillingInfoId);

        //            ProblemListHelper problemListHelper = new ProblemListHelper();
        //            DSProblemLists dsProblemLists = new DSProblemLists();
        //            BLObject<DSProblemLists> objProblemList = null;

        //            if (showCustomFormData)
        //            {
        //                objProblemList = BLLClinicalObj.LoadProblemLists(0, MDVUtility.ToInt64(PatientId), NoteId, "0", "1", 1, 1000, "", "", "1");
        //            }
        //            else
        //            {
        //                objProblemList = BLLClinicalObj.LoadProblemLists(0, MDVUtility.ToInt64(PatientId), NoteId, "0", "1");
        //            }

        //            if (objProblemList.Data != null)
        //            {
        //                dsProblemLists = objProblemList.Data;
        //            }

        //            foreach (var row in cdsRows)
        //            {
        //                var CurBillingInfoICDId = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoICD.BillingInfoICDIdColumn.ColumnName]);
        //                var icdCode = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoICD.ICDCodeColumn.ColumnName]);
        //                var result = lstModels.Where(p => p.ICDCode9 == icdCode || p.ICDCode10 == icdCode);
        //                //  var result2 = lstModels.Where(p => p.ICDType == "9" && p.ICDCode9 == icdCode);
        //                if ((result != null && result.ToList().Count() > 0))
        //                {
        //                    try
        //                    {
        //                        DataRow[] drRows = dsProblemLists.Tables[dsProblemLists.ProblemList.TableName].Select(string.Concat(dsProblemLists.ProblemList.ICD10Column, "=", MDVUtility.ToLINQFormatString(result.FirstOrDefault().ICDCode10), " AND ", dsProblemLists.ProblemList.SNOMEDIDColumn, "=", MDVUtility.ToLINQFormatString(result.FirstOrDefault().SNOMEDCode)));

        //                        if (drRows.Count() > 0)
        //                        {
        //                            if (NoteId > 0 && MDVUtility.ToInt64(drRows[0][dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]) > 0)
        //                            {
        //                                BLLClinicalObj.detachProblemListFromNotes(MDVUtility.ToStr(drRows[0][dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]), NoteId);
        //                                BLLClinicalObj.attachProblemListWithNotes(MDVUtility.ToStr(drRows[0][dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]), NoteId);
        //                            }
        //                        }

        //                    }
        //                    catch (Exception innerEx) { }
        //                }
        //                else
        //                {
        //                    BLLClinicalObj.BillingInfoICD_DELETE(CurBillingInfoICDId);
        //                    if (row.ICDType == 10)
        //                    {
        //                        var ProblemsRow = dsProblemLists.Tables[dsProblemLists.ProblemList.TableName].Select(string.Concat(dsProblemLists.ProblemList.ICD10Column.ColumnName, "=", MDVUtility.ToLINQFormatString(row.ICDCode)));
        //                        if (ProblemsRow.Count() > 0)
        //                        {
        //                            var Prob = ProblemsRow[0];
        //                            ProblemListModel model = new ProblemListModel();
        //                            model.ProblemListId = MDVUtility.ToStr(ProblemsRow[0][dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]);
        //                            model.PatientId = MDVUtility.ToStr(ProblemsRow[0][dsProblemLists.ProblemList.PatientIdColumn.ColumnName]);
        //                            model.Description = MDVUtility.ToStr(ProblemsRow[0][dsProblemLists.ProblemList.DescriptionColumn.ColumnName]);
        //                            model.StartDate = MDVUtility.ToStr(ProblemsRow[0][dsProblemLists.ProblemList.StartDateColumn.ColumnName]);
        //                            problemListHelper.DeleteProblemList(model);
        //                        }
        //                    }
        //                    else
        //                    {

        //                    }
        //                }
        //            }

        //            long BillingInfoICDId_Nagitive = -1;

        //            foreach (var model in lstModels)
        //            {
        //                bool isEdit = true;
        //                DSBillingInformation.BillingInfoICDRow cdsRow = null;

        //                ProblemListModel objProblemListModel = new ProblemListModel();

        //                objProblemListModel.PatientId = PatientId;
        //                objProblemListModel.ICD9 = model.ICDCode9;
        //                objProblemListModel.ICD10 = model.ICDCode10;
        //                objProblemListModel.ICD9_Description = model.ICDDescription9;
        //                objProblemListModel.ICD10_Description = model.ICDDescription10;
        //                objProblemListModel.SNOMEDID = model.SNOMEDCode;
        //                objProblemListModel.SNOMED_DESCRIPTION = model.SNOMEDDescription;
        //                if (!string.IsNullOrEmpty(model.ICDDescription10))
        //                {
        //                    objProblemListModel.ProblemName = model.ICDDescription10;
        //                }
        //                else if (!string.IsNullOrEmpty(model.ICDDescription9))
        //                {
        //                    objProblemListModel.ProblemName = model.ICDDescription9;
        //                }
        //                else if (!string.IsNullOrEmpty(model.SNOMEDDescription))
        //                {
        //                    objProblemListModel.ProblemName = model.SNOMEDDescription;
        //                }
        //                objProblemListModel.Description = string.Concat(MDVUtility.ToStr(model.ICDCode9), " - ", MDVUtility.ToStr(model.ICDCode10), " - ", MDVUtility.ToStr(model.ICDDescription10));

        //                if (cdsRows.Length > 0 && MDVUtility.ToInt64(BillingInfoId) > 0 && (!string.IsNullOrEmpty(model.ICDCode10) || !string.IsNullOrEmpty(model.ICDCode9)))
        //                {
        //                    var BillingId = MDVUtility.ToInt64(BillingInfoId);
        //                    if (!string.IsNullOrEmpty(model.ICDCode10))
        //                        cdsRow = cdsRows.Where(p => p.BillingInfoId == BillingId && p.ICDCode == model.ICDCode10).FirstOrDefault();
        //                    else
        //                        cdsRow = cdsRows.Where(p => p.BillingInfoId == BillingId && p.ICDCode == model.ICDCode9).FirstOrDefault();
        //                }
        //                if (cdsRow == null)
        //                {
        //                    isEdit = false;
        //                    cdsRow = dsDSBillingInformation.BillingInfoICD.NewBillingInfoICDRow();
        //                }
        //                if (cdsRow != null)
        //                {
        //                    if (!isEdit)
        //                        cdsRow.BillingInfoICDId = BillingInfoICDId_Nagitive--;


        //                    cdsRow.BillingInfoId = MDVUtility.ToInt64(BillingInfoId);

        //                    if (!string.IsNullOrEmpty(model.ICDCode10) && !string.IsNullOrEmpty(model.ICDDescription10))
        //                    {
        //                        cdsRow.ICDType = 10;
        //                        cdsRow.ICDCode = model.ICDCode10;
        //                        cdsRow.ICDCodeDescription = model.ICDDescription10;
        //                        try
        //                        {
        //                            var output = dsDSBillingInformation.BillingInfoICD.Where(p => p.ICDCode == model.ICDCode10);
        //                            if (output.ToList().Count > 0)
        //                                continue;

        //                        }
        //                        catch (Exception ex) { }

        //                    }
        //                    else if (!string.IsNullOrEmpty(model.ICDCode9) && !string.IsNullOrEmpty(model.ICDDescription9))
        //                    {
        //                        cdsRow.ICDType = 9;
        //                        cdsRow.ICDCode = model.ICDCode9;
        //                        cdsRow.ICDCodeDescription = model.ICDDescription9;

        //                        try
        //                        {
        //                            var output = dsDSBillingInformation.BillingInfoICD.Where(p => p.ICDCode == model.ICDCode9);
        //                            if (output.ToList().Count > 0)
        //                                continue;

        //                        }
        //                        catch (Exception ex) { }

        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }

        //                    cdsRow.SNOMEDID = model.SNOMEDCode;
        //                    cdsRow.SNOMEDDescription = model.SNOMEDDescription;


        //                    if (!isEdit)
        //                    {
        //                        dsDSBillingInformation.BillingInfoICD.AddBillingInfoICDRow(cdsRow);

        //                        try
        //                        {
        //                            DataRow[] drRows = dsProblemLists.Tables[dsProblemLists.ProblemList.TableName].Select(string.Concat(dsProblemLists.ProblemList.ICD10Column, "=", MDVUtility.ToLINQFormatString(objProblemListModel.ICD10)));

        //                            if (drRows.Count() == 0)
        //                            {
        //                                objProblemListModel.ProblemOrder = "0";
        //                                var problemResponse = problemListHelper.SaveProblemList(objProblemListModel);
        //                                var problemObject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(problemResponse);
        //                                if (!problemResponse.Contains("Problem already exists"))
        //                                {
        //                                    if (NoteId > 0 && MDVUtility.ToInt64(problemObject["ProblemListId"]) > 0)
        //                                    {
        //                                        problemListHelper.attach_ProblemList_With_Notes(MDVUtility.ToStr(problemObject["ProblemListId"]), NoteId);
        //                                    }
        //                                }
        //                            }
        //                            else if (drRows.Count() > 0)
        //                            {
        //                                /* Start 11-04-2017 Humaira Yousaf Bug# EMR-3484
        //                                 The dataset dsProblemLists has information of all problems attached to any note of selected patient.
        //                                 Previous code was attaching first problem with note irrespective of whether it was associated with current note or not 
        //                                 and resulting in attaching a problem with current note which user has not attached. Upon deleting problems from note, those problems were not deleting and were showing up in eSuperbill.
        //                                 Modified code to filter that problem among all notes which is attached to current note. */
        //                                DataRow drProblems = drRows.Where(row => MDVUtility.ToStr(row["NoteId"]).Split(',').Contains(MDVUtility.ToStr(NoteId))).FirstOrDefault();
        //                                if (drProblems != null)
        //                                {
        //                                    if (NoteId > 0 && MDVUtility.ToInt64(drProblems[dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]) > 0)
        //                                    {
        //                                        BLLClinicalObj.detachProblemListFromNotes(MDVUtility.ToStr(drProblems[dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]), NoteId);
        //                                        BLLClinicalObj.attachProblemListWithNotes(MDVUtility.ToStr(drProblems[dsProblemLists.ProblemList.ProblemListIdColumn.ColumnName]), NoteId);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    objProblemListModel.ProblemOrder = "0";
        //                                    var problemResponse = problemListHelper.SaveProblemList(objProblemListModel);
        //                                    var problemObject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(problemResponse);
        //                                    if (!problemResponse.Contains("Problem already exists"))
        //                                    {
        //                                        if (NoteId > 0 && MDVUtility.ToInt64(problemObject["ProblemListId"]) > 0)
        //                                        {
        //                                            problemListHelper.attach_ProblemList_With_Notes(MDVUtility.ToStr(problemObject["ProblemListId"]), NoteId);
        //                                        }
        //                                    }
        //                                }

        //                                // End 11-04-2017 Humaira Yousaf Bug# EMR-3484
        //                            }

        //                        }
        //                        catch (Exception innerEx) { }
        //                    }
        //                }
        //            }

        //            #region Database Insertion/Updation
        //            BLObject<DSBillingInformation> objCDS = BLLClinicalObj.BillingInfoICD_Save(dsDSBillingInformation, PatientId);
        //            dsDSBillingInformation = objCDS.Data;

        //            if (objCDS.Data != null)
        //            {
        //                long insertedBillingInfoId = MDVUtility.ToInt64(BillingInfoId);
        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Save_Message,
        //                    BillingInfoId = insertedBillingInfoId,

        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objCDS.Message.Contains("duplicate") ? "ICD Title already exists" : obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}


        //public string BillingInfoCPT_Save(List<BillingInfoCPTModel> lstModels, string BillingInfoId, string PatientId, long NoteId, bool showCustomFromData = false)
        //{

        //    try
        //    {
        //        DSBillingInformation dsDSBillingInformation = new DSBillingInformation();
        //        BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfoCPT_SELECT(MDVUtility.ToInt64(BillingInfoId));
        //        dsDSBillingInformation = obj.Data;
        //        if (obj.Data != null)
        //        {
        //            //DSBillingInformation.BillingInfoCPTRow cdsRow = null;
        //            DSBillingInformation.BillingInfoCPTRow[] cdsRows = (DSBillingInformation.BillingInfoCPTRow[])dsDSBillingInformation.BillingInfoCPT.Select(dsDSBillingInformation.BillingInfoCPT.BillingInfoIdColumn + "=" + BillingInfoId);
        //            ProceduresHelper objProceduresHelper = new ProceduresHelper();

        //            ProceduresModel proceduresModel = new ProceduresModel();

        //            proceduresModel.procedureDetailModel = new List<ProceduresDetailModel>();

        //            List<ProceduresDetailModel> lstUpdatedProcedureDetail = new List<ProceduresDetailModel>();

        //            DSProcedures dsProcedure = new DSProcedures();

        //            BLObject<DSProcedures> objProcedure = null;
        //            if (showCustomFromData)
        //            {
        //                objProcedure = BLLClinicalObj.loadProcedures_Obsolete(0, MDVUtility.ToInt64(PatientId), (NoteId > 0 ? NoteId : 0), "0", "", 1, 1000, "", "", "", "1", "1");
        //            }
        //            else
        //            {
        //                objProcedure = BLLClinicalObj.loadProcedures_Obsolete(0, MDVUtility.ToInt64(PatientId), (NoteId > 0 ? NoteId : 0), "0", "", 1, 1000, "", "", "", "", "1");
        //            }

        //            if (objProcedure.Data != null)
        //            {
        //                dsProcedure = objProcedure.Data;
        //            }

        //            foreach (var row in cdsRows)
        //            {
        //                //Start//21/10/2016//Ahmad Raza//Adding CPTSNOMEDID and CPTSNOMEDDescription in update case
        //                var CPTSNOMEDID = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoCPT.CPTSNOMEDIDColumn.ColumnName]);
        //                var CPTSNOMEDDescription = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoCPT.CPTSNOMEDDescriptionColumn.ColumnName]);
        //                var cptcode = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoCPT.CPTCodeColumn.ColumnName]);

        //                for (var i = 0; i < lstModels.Count; i++)
        //                {
        //                    if (lstModels[i].CPTSNOMEDCodeId == "" && lstModels[i].CPTCode == cptcode)
        //                    {
        //                        lstModels[i].CPTSNOMEDCodeId = CPTSNOMEDID;
        //                        lstModels[i].CPTSNOMEDDescription = CPTSNOMEDDescription;
        //                    }
        //                }

        //                //End//21/10/2016//Ahmad Raza//Adding CPTSNOMEDID and CPTSNOMEDDescription in update case
        //                var CurBillingInfoCPTId = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoCPT.BillingInfoCPTIdColumn.ColumnName]);
        //                var cptCode = MDVUtility.ToStr(row[dsDSBillingInformation.BillingInfoCPT.CPTCodeColumn.ColumnName]);
        //                var result = lstModels.Where(p => p.CPTCode == cptCode);
        //                if (result != null && result.ToList().Count() > 0)
        //                {

        //                }
        //                else
        //                {
        //                    BLLClinicalObj.BillingInfoCPT_DELETE(CurBillingInfoCPTId);
        //                    try
        //                    {
        //                        var drProcedures = dsProcedure.Tables[dsProcedure.Procedures.TableName].Select(dsProcedure.Procedures.CPTCodeColumn.ColumnName + '=' + MDVUtility.ToLINQFormatString(cptCode));
        //                        if (drProcedures.Count() > 0)
        //                        {
        //                            foreach (var dr in drProcedures)
        //                            {
        //                                ProceduresDetailModel proceduresDetailModel = new ProceduresDetailModel();
        //                                proceduresDetailModel.ProcedureId = MDVUtility.ToStr(dr[dsProcedure.Procedures.ProcedureIdColumn.ColumnName]);
        //                                objProceduresHelper.deleteProcedure(proceduresDetailModel);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception e) { }

        //                }
        //            }

        //            long BillingInfoCPTId_Nagitive = -1;


        //            int j = 0;

        //            foreach (var model in lstModels)
        //            {
        //                ProceduresDetailModel objProceduresDetailModel = new ProceduresDetailModel();
        //                objProceduresDetailModel.Modifier = model.Modifier1;
        //                objProceduresDetailModel.Unit = model.UnitsId;
        //                objProceduresDetailModel.StartDate = model.DOSFrom;
        //                objProceduresDetailModel.EndDate = model.DOSTo;
        //                objProceduresDetailModel.Comments = string.Empty;
        //                objProceduresDetailModel.PatientId = PatientId;
        //                objProceduresDetailModel.CPTCode = model.CPTCode;
        //                objProceduresDetailModel.CPT_DESCRIPTION = model.CPTDescription;
        //                objProceduresDetailModel.IsFromSupperBill = "1";
        //                objProceduresDetailModel.NotesId = MDVUtility.ToStr(NoteId);
        //                objProceduresDetailModel.CPTSNOMEDCodeId = model.CPTSNOMEDCodeId;
        //                objProceduresDetailModel.CPTSNOMEDDescription = model.CPTSNOMEDDescription;
        //                DSBillingInformation.BillingInfoCPTRow cdsRow = null;
        //                bool IsEdit = true;
        //                if (cdsRows.Length > 0 && !string.IsNullOrEmpty(model.BillingInfoCPTId))
        //                {
        //                    var BillingId = MDVUtility.ToInt64(BillingInfoId);

        //                    cdsRow = cdsRows.Where(p => p.BillingInfoId == BillingId && p.BillingInfoCPTId == MDVUtility.ToLong(model.BillingInfoCPTId)).FirstOrDefault();
        //                }
        //                if (cdsRow == null)
        //                {
        //                    IsEdit = false;
        //                    cdsRow = dsDSBillingInformation.BillingInfoCPT.NewBillingInfoCPTRow();
        //                }
        //                if (cdsRow != null)
        //                {
        //                    if (!IsEdit)
        //                        cdsRow.BillingInfoCPTId = BillingInfoCPTId_Nagitive--;


        //                    cdsRow.BillingInfoId = MDVUtility.ToInt64(BillingInfoId);
        //                    cdsRow.IsActive = true;
        //                    cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    cdsRow.ModifiedOn = DateTime.Now;


        //                    if (!IsEdit)
        //                    {
        //                        cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                        cdsRow.CreatedOn = DateTime.Now;
        //                    }
        //                    cdsRow.CPTCode = model.CPTCode;
        //                    cdsRow.CPTDescription = model.CPTDescription;
        //                    cdsRow.CPTSNOMEDID = model.CPTSNOMEDCodeId;
        //                    cdsRow.CPTSNOMEDDescription = model.CPTSNOMEDDescription;

        //                    if (string.IsNullOrEmpty(model.CPTCode) && string.IsNullOrEmpty(model.CPTDescription))
        //                        continue;

        //                    cdsRow.Units = MDVUtility.ToDouble(model.UnitsId);
        //                    cdsRow.POSCode = MDVUtility.ToStr(model.POS);
        //                    cdsRow.Modifier1 = model.Modifier1;

        //                    cdsRow.Modifier2 = model.Modifier2;
        //                    cdsRow.Modifier3 = model.Modifier3;
        //                    cdsRow.Modifier4 = model.Modifier4;
        //                    cdsRow.ICDPointer1 = model.DxPointer1;
        //                    cdsRow.ICDPointer2 = model.DxPointer2;
        //                    cdsRow.ICDPointer3 = model.DxPointer3;
        //                    cdsRow.ICDPointer4 = model.DxPointer4;
        //                    if (!string.IsNullOrEmpty(model.Type))
        //                    {
        //                        cdsRow.Type = model.Type;
        //                    }
        //                    else
        //                    {
        //                        cdsRow[dsDSBillingInformation.BillingInfoCPT.TypeColumn.ColumnName] = DBNull.Value;
        //                    }

        //                    if (!string.IsNullOrEmpty(model.BillingInfoTimeId))
        //                    {
        //                        cdsRow.BillingInfoTimeId = model.BillingInfoTimeId;
        //                    }
        //                    else
        //                    {
        //                        cdsRow[dsDSBillingInformation.BillingInfoCPT.BillingInfoTimeIdColumn.ColumnName] = DBNull.Value;
        //                    }



        //                    if (!string.IsNullOrEmpty(model.DOSFrom))
        //                        cdsRow.DOSFrom = Convert.ToDateTime(model.DOSFrom);
        //                    if (!string.IsNullOrEmpty(model.DOSTo))
        //                        cdsRow.DOSTo = Convert.ToDateTime(model.DOSTo);

        //                    /* Start 11-04-2017 Humaira Yousaf Bug# EMR-3484
        //                               The dataset dsProblemLists has information of all procedures attached to any note of selected patient.
        //                              Previous code was attaching first procedure with note irrespective of whether it was associated with current note or not 
        //                             and resulting in attaching a procedure with current note which user has not attached. Upon deleting procedures from note, those procedures were not deleting and were showing up in eSuperbill.
        //                              Modified code to filter that procedure among all notes which is attached to current note. */

        //                    if (!IsEdit)
        //                    {
        //                        dsDSBillingInformation.BillingInfoCPT.AddBillingInfoCPTRow(cdsRow);
        //                        try
        //                        {
        //                            DataRow[] drRows = dsProcedure.Tables[dsProcedure.Procedures.TableName].Select(dsProcedure.Procedures.CPTCodeColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(objProceduresDetailModel.CPTCode) + " AND " + dsProcedure.Procedures.CPT_DESCRIPTIONColumn.ColumnName + "='" + objProceduresDetailModel.CPT_DESCRIPTION + "'");

        //                            if (drRows.Count() == 0)
        //                                if (MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                    proceduresModel.procedureDetailModel.Add(objProceduresDetailModel);
        //                            if (drRows.Count() > 0)
        //                            {
        //                                if (j == 0)
        //                                {
        //                                    j++;
        //                                    objProceduresDetailModel.ProcedureId = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.ProcedureIdColumn.ColumnName]);
        //                                    objProceduresDetailModel.StartDate = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.StartDateColumn.ColumnName]);
        //                                    objProceduresDetailModel.EndDate = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.EndDateColumn.ColumnName]);
        //                                    objProceduresDetailModel.Comments = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.CommentsColumn.ColumnName]);
        //                                    objProceduresDetailModel.IsActive = "1";
        //                                    objProceduresDetailModel.ProblemListId_text = string.Empty;
        //                                    if (MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                        lstUpdatedProcedureDetail.Add(objProceduresDetailModel);
        //                                    if (NoteId > 0 && MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                    {
        //                                        BLLClinicalObj.detachProcedureFromNotes(MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.ProcedureIdColumn.ColumnName]), NoteId);
        //                                        BLLClinicalObj.attachProcedureWithNotes(MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.ProcedureIdColumn.ColumnName]), NoteId);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    DataRow drProcedure = drRows.Where(row => MDVUtility.ToStr(row["NotesId"]).Split(',').Contains(MDVUtility.ToStr(NoteId))).FirstOrDefault();
        //                                    if (drProcedure != null)
        //                                    {
        //                                        objProceduresDetailModel.ProcedureId = MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.ProcedureIdColumn.ColumnName]);
        //                                        objProceduresDetailModel.StartDate = MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.StartDateColumn.ColumnName]);
        //                                        objProceduresDetailModel.EndDate = MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.EndDateColumn.ColumnName]);
        //                                        objProceduresDetailModel.Comments = MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.CommentsColumn.ColumnName]);
        //                                        objProceduresDetailModel.IsActive = "1";
        //                                        objProceduresDetailModel.ProblemListId_text = string.Empty;
        //                                        if (MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                            lstUpdatedProcedureDetail.Add(objProceduresDetailModel);
        //                                        if (NoteId > 0 && MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                        {
        //                                            BLLClinicalObj.detachProcedureFromNotes(MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.ProcedureIdColumn.ColumnName]), NoteId);
        //                                            BLLClinicalObj.attachProcedureWithNotes(MDVUtility.ToStr(drProcedure[dsProcedure.Procedures.ProcedureIdColumn.ColumnName]), NoteId);
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }

        //                        //End 11-04-2017 Humaira Yousaf Bug# EMR-3484                                          
        //                    }
        //                    else
        //                    {
        //                        DataRow[] drRows = dsProcedure.Tables[dsProcedure.Procedures.TableName].Select(dsProcedure.Procedures.CPTCodeColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(objProceduresDetailModel.CPTCode) + " AND " + dsProcedure.Procedures.CPT_DESCRIPTIONColumn.ColumnName + "='" + objProceduresDetailModel.CPT_DESCRIPTION + "'");
        //                        if (drRows.Count() > 0)
        //                        {
        //                            objProceduresDetailModel.ProcedureId = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.ProcedureIdColumn.ColumnName]);
        //                            objProceduresDetailModel.StartDate = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.StartDateColumn.ColumnName]);
        //                            objProceduresDetailModel.EndDate = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.EndDateColumn.ColumnName]);
        //                            objProceduresDetailModel.Comments = MDVUtility.ToStr(drRows[0][dsProcedure.Procedures.CommentsColumn.ColumnName]);
        //                            objProceduresDetailModel.IsActive = "1";
        //                            objProceduresDetailModel.ProblemListId_text = string.Empty;
        //                            if (MDVUtility.ToBool(model.IsLabBasedCPT) != true)
        //                                lstUpdatedProcedureDetail.Add(objProceduresDetailModel);
        //                        }
        //                    }
        //                }
        //            }


        //            #region Database Insertion/Updation
        //            BLObject<DSBillingInformation> objCDS = BLLClinicalObj.BillingInfoCPT_Save(dsDSBillingInformation, PatientId);
        //            dsDSBillingInformation = objCDS.Data;

        //            if (objCDS.Data != null)
        //            {

        //                long insertedBillingInfoId = MDVUtility.ToInt64(BillingInfoId);
        //                var procedureResponse = objProceduresHelper.saveProcedure(proceduresModel);
        //                if (lstUpdatedProcedureDetail.Count > 0)
        //                {
        //                    for (int counter = 0; counter < lstUpdatedProcedureDetail.Count; counter++)
        //                    {
        //                        List<ProceduresDetailModel> UpdatedProcedureDetail = new List<ProceduresDetailModel>();
        //                        UpdatedProcedureDetail.Add(lstUpdatedProcedureDetail[counter]);
        //                        objProceduresHelper.updateProcedures(UpdatedProcedureDetail);
        //                    }

        //                }
        //                if (procedureResponse != null && procedureResponse != string.Empty)
        //                {
        //                    var procedureObject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(procedureResponse);
        //                    if (MDVUtility.ToStr(procedureObject["ProcedureLoad_JSON"]) != "")
        //                    {

        //                        var procedureList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProceduresDetailModel>>(MDVUtility.ToStr(procedureObject["ProcedureLoad_JSON"]));
        //                        foreach (var pro in procedureList)
        //                        {
        //                            objProceduresHelper.attach_Procedure_With_Notes(MDVUtility.ToStr(pro.ProcedureId), NoteId);
        //                        }
        //                    }
        //                }

        //                //to fetch cpt fee and charges in response
        //                DSProblemLists dSProblemListseAttached = null;
        //                BLObject<DSProblemLists> objaa = BLLClinicalObj.LoadProblemAndProcedureList(NoteId, MDVUtility.ToLong(PatientId));
        //                dSProblemListseAttached = objaa.Data;


        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Save_Message,
        //                    BillingInfoId = insertedBillingInfoId,
        //                    CPTSListFill_JSON = MDVUtility.JSON_DataTable(dSProblemListseAttached.Tables[dsProcedure.Procedures.TableName])
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objCDS.Message.Contains("duplicate") ? "CDS Title already exists" : obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string BillingInfoCPT_DELETE(string BillingInfoId, string CPTCode, long PatientId, long NotesId)
        {

            try
            {
                ProceduresHelper objProceduresHelper = new ProceduresHelper();
                DSProcedures dsProcedure = new DSProcedures();
                var objProcedure = BLLClinicalObj.loadProcedures_Obsolete(0, MDVUtility.ToInt64(PatientId), 0);

                if (objProcedure.Data != null)
                {
                    dsProcedure = objProcedure.Data;
                }

                var drProcedures = dsProcedure.Tables[dsProcedure.Procedures.TableName].Select(dsProcedure.Procedures.CPTCodeColumn.ColumnName + '=' + MDVUtility.ToLINQFormatString(CPTCode));
                try
                {
                    if (drProcedures.Count() > 0)
                    {
                        foreach (var dr in drProcedures)
                        {
                            ProceduresDetailModel proceduresModel = new ProceduresDetailModel();
                            proceduresModel.ProcedureId = MDVUtility.ToStr(dr[dsProcedure.Procedures.ProcedureIdColumn.ColumnName]);
                            BLLClinicalObj.detachProcedureFromNotes(proceduresModel.ProcedureId, NotesId);
                            objProceduresHelper.deleteProcedure(proceduresModel);

                        }
                    }
                }
                catch (Exception e) { }



                BLLClinicalObj.BillingInfoCPT_DELETE(BillingInfoId, CPTCode);

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Delete_Message,
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

        //public BLObject<DSBillingInformation> BillingInfoCPT_SELECT(long BillingInfoId)
        //{
        //    DSBillingInformation ds = new DSBillingInformation();
        //    try
        //    {
        //        ds = new DALBillingInformation(SharedObj).BillingInfoCPT_SELECT(BillingInfoId);
        //        return new BLObject<DSBillingInformation>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLClinical::BillingInfoCPT_Save", ex);
        //        return new BLObject<DSBillingInformation>(null, ex.Message);
        //    }
        //}

        #region For eSuperBill from Schedular and Appointments (Created by Abid Ali)
        public string BillingInfo_SELECT_Customized(long NoteId, long PatientId, long BillingInfoId)
        {
            try
            {
                DSBillingInformation dsBillingInformation = null;
                BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT(NoteId, PatientId, BillingInfoId);
                dsBillingInformation = obj.Data;
                if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                {
                    List<BillingInformationModel> billingInfoRows = new List<BillingInformationModel>();
                    foreach (DSBillingInformation.BillingInfoRow dr in dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows)
                    {

                        //DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];
                        BillingInformationModel result = new BillingInformationModel();
                        result.BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName]);
                        if (result.ENMCPTDescription.Length > 50)
                            result.ENMCPTDescription = result.ENMCPTDescription.Substring(0, 49);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.SoapTextColumn.ColumnName]);
                        if (result.SoapText.Length > 50)
                            result.SoapText = result.SoapText.Substring(0, 49);


                        billingInfoRows.Add(result);
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = 2147483644;
                    var response = new
                    {
                        addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                        editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                        viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                        status = true,
                        BillingInfoFill_JSON = js.Serialize(billingInfoRows)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                        editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                        viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                        status = true,
                        BillingInfoFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                    editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                    viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Out Of Office Visit
        public string BillingInfo_SELECT_OutOfOfficeVisit(string ICDCode, string CPTCode, long PatientId, long ProviderId, long FacilityId, string Status, string DOSFrom, string DOSTo, string pageNumber = "1", string rowsPerPage = "15")
        {
            try
            {
                DSBillingInformation dsBillingInformation = null;
                BLObject<DSBillingInformation> obj = BLLClinicalObj.loadOutOfOfficeVisit(ICDCode, CPTCode, PatientId, ProviderId, FacilityId, Status, DOSFrom, DOSTo, pageNumber, rowsPerPage);
                dsBillingInformation = obj.Data;
                if (dsBillingInformation.Tables[dsBillingInformation.OutOfOfficeVisit.TableName].Rows.Count > 0)
                {
                    List<BillingInformationModel> billingInfoRows = new List<BillingInformationModel>();
                    foreach (DSBillingInformation.OutOfOfficeVisitRow dr in dsBillingInformation.Tables[dsBillingInformation.OutOfOfficeVisit.TableName].Rows)
                    {

                        //DataRow dr = dsBillingInformation.Tables[dsBillingInformation.OutOfOfficeVisit.TableName].Rows[0];
                        BillingInformationModel result = new BillingInformationModel();
                        result.BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.BillingInfoIdColumn.ColumnName]);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMCPTDescriptionColumn.ColumnName]);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.OutOfOfficeVisit.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ProviderIdColumn.ColumnName]);
                        result.Provider = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ProviderColumn.ColumnName]);
                        result.Facility = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.FacilityColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.SoapTextColumn.ColumnName]);
                        result.ICDCodes = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.ICDCodesColumn.ColumnName]);
                        result.CPTCodes = MDVUtility.ToStr(dr[dsBillingInformation.OutOfOfficeVisit.CPTCodesColumn.ColumnName]);

                        billingInfoRows.Add(result);
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                        editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                        viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                        status = true,
                        BillingInfoFill_JSON = js.Serialize(billingInfoRows),
                        BillingInfoCount = dsBillingInformation.Tables[dsBillingInformation.OutOfOfficeVisit.TableName].Rows.Count,
                        //LabLoad_JSON = MDVUtility.JSON_DataTable(dsBillingInformation.Tables[dsBillingInformation.OutOfOfficeVisit.TableName]),
                        iTotalDisplayRecords = (dsBillingInformation.OutOfOfficeVisit.Rows.Count > 0) ? dsBillingInformation.OutOfOfficeVisit.Rows[0][dsBillingInformation.OutOfOfficeVisit.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                        editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                        viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                        status = true,
                        BillingInfoFill_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    addPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "ADD")).ToString(),
                    editPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "EDIT")).ToString(),
                    viewPermission = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "VIEW")).ToString(),
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}