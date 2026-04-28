using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.Case
{
    public class Patient_Case
    {
        private BLLPatient BLLPatientObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Patient_Case()
        {
            BLLPatientObj = new BLLPatient();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Patient_Case _obj = null;
        public static Patient_Case Instance()
        {
            if (_obj == null)
                _obj = new Patient_Case();
            return _obj;
        }
        #endregion


        #region Private Functions

        /// <summary>
        /// Saves the Case.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveCase(string fieldsJSON, Int64 PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSCase dsCase = new DSCase();
                    DSCase.CaseManagementRow dr = dsCase.CaseManagement.NewCaseManagementRow();

                    dr.PatientId = MDVUtility.ToInt64(PatientId);
                    dr.CaseTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCaseType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientInsuranceId"]))
                        dr.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPatientInsuranceId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                        dr.ReferringProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        dr.OperatingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtHospitalCaseNo"]))
                        dr.HospitalCaseNo = MDVUtility.ToStr(SearchedfieldsJSON["txtHospitalCaseNo"]);
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsCase.CaseManagement.AddCaseManagementRow(dr);
                    BLObject<DSCase> obj = BLLPatientObj.InsertCaseManagement(dsCase);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            CaseId = dsCase.Tables[dsCase.CaseManagement.TableName].Rows[0][dsCase.CaseManagement.CaseMgmtIdColumn.ColumnName],
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
                    #endregion
                }
                else
                {
                    throw new Exception("Patient not found.");
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

        /// Load all the Case for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseId">The Case identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchCase(string fieldsJSON, Int64 PatientId, Int64 CaseId, int PageNumber, int RowsPerPage)
        {
            try
            {
                Int32 caseType = 0;
                string AccountNumber = "";
                string ClaimNumber = "";
                Int64 PatientInsurance = 0;
                Int64 FacilityId = 0;
                Int64 ProviderId = 0;
                string IsActive = "";
                string CaseNumber = "";
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (SearchedfieldsJSON.ContainsKey("txtPatientName"))
                    AccountNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientName"]) : "";

                if (SearchedfieldsJSON.ContainsKey("txtClaimNumber"))
                    ClaimNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtClaimNumber"]) : "";

                if (SearchedfieldsJSON.ContainsKey("ddlPatientInsuranceId"))
                    PatientInsurance = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientInsuranceId"]) ? MDVUtility.ToInt64(SearchedfieldsJSON["ddlPatientInsuranceId"]) : 0;

                if (SearchedfieldsJSON.ContainsKey("hfFacility"))
                    FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]) ? MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]) : 0;

                if (SearchedfieldsJSON.ContainsKey("hfProvider"))
                    ProviderId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]) ? MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]) : 0;

                if (SearchedfieldsJSON.ContainsKey("ddlActive"))
                    IsActive = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]) : "";

                if (SearchedfieldsJSON.ContainsKey("txtCaseNumber"))
                    CaseNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["txtCaseNumber"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtCaseNumber"]) : "";

                DSCase dsCase = null;
                BLObject<DSCase> obj = null;
                if (SearchedfieldsJSON == null)
                    obj = BLLPatientObj.LoadCaseManagement(AccountNumber, ClaimNumber, PatientId, CaseId, 0, 0, 0, 0, null, null);
                else
                {

                    if (SearchedfieldsJSON.ContainsKey("ddlType") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlType"]))
                        caseType = MDVUtility.ToInt32(SearchedfieldsJSON["ddlType"]);


                    obj = BLLPatientObj.LoadCaseManagement(AccountNumber, ClaimNumber, PatientId, CaseId, PatientInsurance, caseType, FacilityId, ProviderId, IsActive, CaseNumber, PageNumber, RowsPerPage);
                }

                if (obj.Data != null)
                {
                    dsCase = obj.Data;

                    if (dsCase.Tables[dsCase.CaseManagement.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CaseCount = dsCase.Tables[dsCase.CaseManagement.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCase.CaseManagement.Rows[0][dsCase.CaseManagement.RecordCountColumn.ColumnName],
                            CaseLoad_JSON = MDVUtility.JSON_DataTable(dsCase.Tables[dsCase.CaseManagement.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CaseCount = 0,
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
        private string SearchCaseDocuments(string CaseId)
        {
            try
            {
                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj;

                obj = BLLPatientObj.SearchClaimDocuments(CaseId);

                if (obj.Data != null)
                {
                    dsBatchCharge = obj.Data;

                    if (dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        var response = new
                        {
                            status = true,
                            ClaimChargeDocumentCount = dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count,
                            ClaimChargeDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
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
        /// <summary>
        /// Updates the Case.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseId">The Case identifier.</param>
        /// <returns></returns>
        private string UpdateCase(string fieldsJSON, Int64 PatientId, Int64 CaseId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (CaseId > 0)
                {
                    DSCase dsCase = new DSCase();
                    DSWCNF dsWCNF = new DSWCNF();
                    BLObject<DSCase> objLoad = BLLPatientObj.LoadCaseManagement(null, null, PatientId, CaseId, 0, 0, 0, 0, "", null);
                    dsCase = objLoad.Data;
                    // dsCase.Tables[0] = objLoad.Data.Tables[0];
                    foreach (DSCase.CaseManagementRow drCase in dsCase.Tables[dsCase.CaseManagement.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCaseType"]))
                            drCase.CaseTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlCaseType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientInsuranceId"]))
                            drCase.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPatientInsuranceId"]);
                        else
                            drCase[dsCase.CaseManagement.PatientInsuranceIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                            drCase.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        else
                            drCase[dsCase.CaseManagement.FacilityIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                            drCase.ReferringProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                        else
                            drCase[dsCase.CaseManagement.ReferringProviderIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                            drCase.OperatingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        else
                            drCase[dsCase.CaseManagement.OperatingProviderIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtHospitalCaseNo"]))
                            drCase.HospitalCaseNo = MDVUtility.ToStr(SearchedfieldsJSON["txtHospitalCaseNo"]);
                        else
                            drCase[dsCase.CaseManagement.HospitalCaseNoColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFrequencyCode"]))
                            drCase.FrequencyCode = MDVUtility.ToStr(SearchedfieldsJSON["txtFrequencyCode"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtSourceofAdmit"]))
                            drCase.SourceOfAdmit = MDVUtility.ToStr(SearchedfieldsJSON["txtSourceofAdmit"]);
                        else
                            drCase[dsCase.CaseManagement.SourceOfAdmitColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientStatus"]))
                            drCase.PatientStatus = MDVUtility.ToStr(SearchedfieldsJSON["txtPatientStatus"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTypeofAdmission"]))
                            drCase.TypeOfAdmission = MDVUtility.ToStr(SearchedfieldsJSON["txtTypeofAdmission"]);
                        else
                            drCase[dsCase.CaseManagement.TypeOfAdmissionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAdmitDxCode"]))
                            drCase.AdmitDXCode = MDVUtility.ToStr(SearchedfieldsJSON["txtAdmitDxCode"]);
                        else
                            drCase[dsCase.CaseManagement.AdmitDXCodeColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlConditionCode1"]))
                            drCase.ConditionCode1 = MDVUtility.ToInt32(SearchedfieldsJSON["ddlConditionCode1"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlConditionCode2"]))
                            drCase.ConditionCode2 = MDVUtility.ToInt32(SearchedfieldsJSON["ddlConditionCode2"]);
                        else
                            drCase[dsCase.CaseManagement.ConditionCode2Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlConditionCode3"]))
                            drCase.ConditionCode3 = MDVUtility.ToInt32(SearchedfieldsJSON["ddlConditionCode3"]);
                        else
                            drCase[dsCase.CaseManagement.ConditionCode3Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlConditionCode4"]))
                            drCase.ConditionCode4 = MDVUtility.ToInt32(SearchedfieldsJSON["ddlConditionCode4"]);
                        else
                            drCase[dsCase.CaseManagement.ConditionCode4Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOccurrenceCode1"]))
                            drCase.OccurrenceCode1 = MDVUtility.ToStr(SearchedfieldsJSON["txtOccurrenceCode1"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceCode1Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOccurrenceCode2"]))
                            drCase.OccurrenceCode2 = MDVUtility.ToStr(SearchedfieldsJSON["txtOccurrenceCode2"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceCode2Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOccurrenceCode3"]))
                            drCase.OccurrenceCode3 = MDVUtility.ToStr(SearchedfieldsJSON["txtOccurrenceCode3"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceCode3Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOccurrenceCode4"]))
                            drCase.OccurrenceCode4 = MDVUtility.ToStr(SearchedfieldsJSON["txtOccurrenceCode4"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceCode4Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpOccurrenceDate1"]))
                            drCase.OccurrenceDate1 = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpOccurrenceDate1"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceDate1Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpOccurrenceDate2"]))
                            drCase.OccurrenceDate2 = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpOccurrenceDate2"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceDate2Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpOccurrenceDate3"]))
                            drCase.OccurrenceDate3 = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpOccurrenceDate3"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceDate3Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpOccurrenceDate4"]))
                            drCase.OccurrenceDate4 = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpOccurrenceDate4"]);
                        else
                            drCase[dsCase.CaseManagement.OccurrenceDate4Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode1"]))
                            drCase.ValueCode1 = MDVUtility.ToStr(SearchedfieldsJSON["txtValueCode1"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode1Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode2"]))
                            drCase.ValueCode2 = MDVUtility.ToStr(SearchedfieldsJSON["txtValueCode2"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode2Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode3"]))
                            drCase.ValueCode3 = MDVUtility.ToStr(SearchedfieldsJSON["txtValueCode3"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode3Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode4"]))
                            drCase.ValueCode4 = MDVUtility.ToStr(SearchedfieldsJSON["txtValueCode4"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode4Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode1Amount"]))
                            drCase.ValueCode1Amount = MDVUtility.ToDouble(SearchedfieldsJSON["txtValueCode1Amount"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode1AmountColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode2Amount"]))
                            drCase.ValueCode2Amount = MDVUtility.ToDouble(SearchedfieldsJSON["txtValueCode2Amount"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode2AmountColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode3Amount"]))
                            drCase.ValueCode3Amount = MDVUtility.ToDouble(SearchedfieldsJSON["txtValueCode3Amount"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode3AmountColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtValueCode4Amount"]))
                            drCase.ValueCode4Amount = MDVUtility.ToDouble(SearchedfieldsJSON["txtValueCode4Amount"]);
                        else
                            drCase[dsCase.CaseManagement.ValueCode4AmountColumn] = DBNull.Value;

                        drCase.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;

                        drCase.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drCase.ModifiedOn = DateTime.Now;
                    }
                    BLObject<DSWCNF> LoadWCNFDetail = BLLPatientObj.LoadWCNFCaseManagement(CaseId);
                    dsWCNF = LoadWCNFDetail.Data;
                    DSWCNF.WCNFDetailRow dr = dsWCNF.WCNFDetail.NewWCNFDetailRow();

                    if (dsWCNF.WCNFDetail.Rows.Count == 0)
                    {
                        dr[dsWCNF.WCNFDetail.CaseMgmtIdColumn] = CaseId;
                        dr[dsWCNF.WCNFDetail.PatientIdColumn] = PatientId;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseAdjusterId"]))
                            dr.CaseAdjusterId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseAdjusterId"]);
                        else
                            dr[dsWCNF.WCNFDetail.CaseAdjusterIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTelephone"]))
                            dr.PhoneNo = MDVUtility.ToStr(SearchedfieldsJSON["txtTelephone"]);
                        else
                            dr[dsWCNF.WCNFDetail.PhoneNoColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFax"]))
                            dr.Fax = MDVUtility.ToStr(SearchedfieldsJSON["txtFax"]);
                        else
                            dr[dsWCNF.WCNFDetail.FaxColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEmail"]))
                            dr.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtEmail"]);
                        else
                            dr[dsWCNF.WCNFDetail.EmailColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPreAuth"]))
                            dr.PreAuth = MDVUtility.ToStr(SearchedfieldsJSON["txtPreAuth"]);
                        else
                            dr[dsWCNF.WCNFDetail.PreAuthColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpInjuryDate"]))
                            dr.InjuryDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpInjuryDate"]);
                        else
                            dr[dsWCNF.WCNFDetail.InjuryDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferral"]))
                            dr.Referral = MDVUtility.ToStr(SearchedfieldsJSON["txtReferral"]);
                        else
                            dr[dsWCNF.WCNFDetail.ReferralColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAccidentCause"]))
                            dr.CauseOfAccident = MDVUtility.ToStr(SearchedfieldsJSON["txtAccidentCause"]);
                        else
                            dr[dsWCNF.WCNFDetail.CauseOfAccidentColumn] = DBNull.Value;

                        if (Convert.ToBoolean(SearchedfieldsJSON["chkYes"]))
                            dr.EmploymentRelate = true;
                        else if (Convert.ToBoolean(SearchedfieldsJSON["chkNo"]))
                            dr.EmploymentRelate = false;
                        else
                            dr[dsWCNF.WCNFDetail.EmploymentRelateColumn] = DBNull.Value;
                        if (Convert.ToBoolean(SearchedfieldsJSON["chkAuto"]))
                            dr.Accident = "Auto";
                        else if (Convert.ToBoolean(SearchedfieldsJSON["chkNonAuto"]))
                            dr.Accident = "Non Auto";
                        else if (Convert.ToBoolean(SearchedfieldsJSON["chkAccidentNo"]))
                            dr.Accident = "No";
                        else
                            dr[dsWCNF.WCNFDetail.AccidentColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtState"]))
                            dr.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);
                        else
                            dr[dsWCNF.WCNFDetail.StateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtZip"]))
                            dr.Zip = MDVUtility.ToStr(SearchedfieldsJSON["txtZip"]);
                        else
                            dr[dsWCNF.WCNFDetail.ZipColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txthour"]))
                            dr.Hour = MDVUtility.ToStr(SearchedfieldsJSON["txthour"]);
                        else
                            dr[dsWCNF.WCNFDetail.HourColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField16DateFrom"]))
                            dr.HCFAField16DateFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField16DateFrom"]);
                        else
                            dr[dsWCNF.WCNFDetail.HCFAField16DateFromColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField16DateTo"]))
                            dr.HCFAField16DateTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField16DateTo"]);
                        else
                            dr[dsWCNF.WCNFDetail.HCFAField16DateToColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField18DateFrom"]))
                            dr.HCFAField18DateFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField18DateFrom"]);
                        else
                            dr[dsWCNF.WCNFDetail.HCFAField18DateFromColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField18DateTo"]))
                            dr.HCFAField18DateTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField18DateTo"]);
                        else
                            dr[dsWCNF.WCNFDetail.HCFAField18DateToColumn] = DBNull.Value;
                        dr[dsWCNF.WCNFDetail.CreatedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsWCNF.WCNFDetail.CreatedOnColumn] = DateTime.Now;
                        dr[dsWCNF.WCNFDetail.IsActiveColumn] = true;
                        dsWCNF.WCNFDetail.Rows.Add(dr);

                    }
                    else
                    {
                        foreach (DSWCNF.WCNFDetailRow drCase in dsWCNF.Tables[dsWCNF.WCNFDetail.TableName].Rows)
                        {

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseAdjusterId"]))
                                drCase.CaseAdjusterId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseAdjusterId"]);
                            else
                                drCase[dsWCNF.WCNFDetail.CaseAdjusterIdColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTelephone"]))
                                drCase.PhoneNo = MDVUtility.ToStr(SearchedfieldsJSON["txtTelephone"]);
                            else
                                drCase[dsWCNF.WCNFDetail.PhoneNoColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFax"]))
                                drCase.Fax = MDVUtility.ToStr(SearchedfieldsJSON["txtFax"]);
                            else
                                drCase[dsWCNF.WCNFDetail.FaxColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEmail"]))
                                drCase.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtEmail"]);
                            else
                                drCase[dsWCNF.WCNFDetail.EmailColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPreAuth"]))
                                drCase.PreAuth = MDVUtility.ToStr(SearchedfieldsJSON["txtPreAuth"]);
                            else
                                drCase[dsWCNF.WCNFDetail.PreAuthColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpInjuryDate"]))
                                drCase.InjuryDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpInjuryDate"]);
                            else
                                drCase[dsWCNF.WCNFDetail.InjuryDateColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferral"]))
                                drCase.Referral = MDVUtility.ToStr(SearchedfieldsJSON["txtReferral"]);
                            else
                                drCase[dsWCNF.WCNFDetail.ReferralColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAccidentCause"]))
                                drCase.CauseOfAccident = MDVUtility.ToStr(SearchedfieldsJSON["txtAccidentCause"]);
                            else
                                drCase[dsWCNF.WCNFDetail.CauseOfAccidentColumn] = DBNull.Value;

                            if (Convert.ToBoolean(SearchedfieldsJSON["chkYes"]))
                                drCase.EmploymentRelate = true;
                            else if (Convert.ToBoolean(SearchedfieldsJSON["chkNo"]))
                                drCase.EmploymentRelate = false;
                            else
                                drCase[dsWCNF.WCNFDetail.EmploymentRelateColumn] = DBNull.Value;
                            if (Convert.ToBoolean(SearchedfieldsJSON["chkAuto"]))
                                drCase.Accident = "Auto";
                            else if (Convert.ToBoolean(SearchedfieldsJSON["chkNonAuto"]))
                                drCase.Accident = "Non Auto";
                            else if (Convert.ToBoolean(SearchedfieldsJSON["chkAccidentNo"]))
                                drCase.Accident = "No";
                            else
                                drCase[dsWCNF.WCNFDetail.AccidentColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtState"]))
                                drCase.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);
                            else
                                drCase[dsWCNF.WCNFDetail.StateColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtZip"]))
                                drCase.Zip = MDVUtility.ToStr(SearchedfieldsJSON["txtZip"]);
                            else
                                dr[dsWCNF.WCNFDetail.ZipColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txthour"]))
                                drCase.Hour = MDVUtility.ToStr(SearchedfieldsJSON["txthour"]);
                            else
                                drCase[dsWCNF.WCNFDetail.HourColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField16DateFrom"]))
                                drCase.HCFAField16DateFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField16DateFrom"]);
                            else
                                drCase[dsWCNF.WCNFDetail.HCFAField16DateFromColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField16DateTo"]))
                                drCase.HCFAField16DateTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField16DateTo"]);
                            else
                                drCase[dsWCNF.WCNFDetail.HCFAField16DateToColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField18DateFrom"]))
                                drCase.HCFAField18DateFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField18DateFrom"]);
                            else
                                drCase[dsWCNF.WCNFDetail.HCFAField18DateFromColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpHCFAField18DateTo"]))
                                drCase.HCFAField18DateTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHCFAField18DateTo"]);
                            else
                                drCase[dsWCNF.WCNFDetail.HCFAField18DateToColumn] = DBNull.Value;
                            drCase[dsWCNF.WCNFDetail.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            drCase[dsWCNF.WCNFDetail.ModifiedOnColumn] = DateTime.Now;
                            // drCase.AcceptChanges();
                        }
                    }
                    //  dsCase.AcceptChanges();
                    #region Database Updation

                    if (dsCase.Tables[dsCase.CaseManagement.TableName].Rows.Count > 0)
                    {
                        //dsCase.CaseManagement.Rows[0].SetModified();
                        BLObject<DSCase> obj = null;


                        obj = BLLPatientObj.UpdateCaseManagement(dsCase);

                        if (dsWCNF.Tables[dsWCNF.WCNFDetail.TableName].Rows[0].Field<Int64>(0) > 0)
                            BLLPatientObj.UpdateWCNFCaseManagement(dsWCNF);
                        else
                        {
                            BLLPatientObj.InsertWCNFCaseManagement(dsWCNF);
                        }
                        if (obj.Data != null)
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
                            Message = ""
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
                        Message = "Case not found."
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
        /// Updates the Case IsActive.
        /// </summary>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="CaseId">The Case identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateCaseIsActive(Int64 PatientId, Int64 CaseId, Int64 IsActive)
        {
            try
            {
                DSCase dsCase = null;
                BLObject<DSCase> obj = BLLPatientObj.LoadCaseManagement(null, null, PatientId, CaseId, 0, 0, 0, 0, null, null);
                dsCase = obj.Data;
                if (dsCase.Tables[dsCase.CaseManagement.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsCase.Tables[dsCase.CaseManagement.TableName].Rows[0];
                    dr[dsCase.CaseManagement.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSCase> objCase = BLLPatientObj.UpdateCaseManagement(dsCase);
                    string successMsg;
                    if (objCase.Data != null)
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
                            Message = objCase.Message
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
        /// Deletes the Case.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>

        private string DeleteCase(long CaseId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CaseId)))
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
                    BLObject<string> obj = BLLPatientObj.DeleteCaseManagement(MDVUtility.ToStr(CaseId));
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
        /// <summary>
        /// Fills the PreAuthorization.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillCase(long PatientID, long CaseId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CaseId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSCase dsCase = null;
                    BLObject<DSCase> obj = BLLPatientObj.LoadCaseManagement(null, null, PatientID, CaseId, 0, 0, 0, 0, null, null);
                    if (obj.Data != null)
                    {
                        dsCase = obj.Data;
                        if (dsCase.Tables[dsCase.CaseManagement.TableName].Rows.Count > 0)
                        {

                            DataRow dr = dsCase.Tables[dsCase.CaseManagement.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                            //[OperatingProviderId],pro.ShortName as ProviderName,cm.PatientStatus,
                           //cm.FrequencyCode,cm.ConditionCode1,cm.IsActive,cm.CreatedBy,cm.CreatedOn,cm.ModifiedBy,cm.ModifiedOn
                            {"ddlPatientInsuranceId", MDVUtility.ToStr(dr[dsCase.CaseManagement.PatientInsuranceIdColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsCase.CaseManagement.FacilityIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsCase.CaseManagement.FacilityNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsCase.CaseManagement.OperatingProviderIdColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsCase.CaseManagement.ProviderNameColumn.ColumnName])},
                            { "hfRefProvider", MDVUtility.ToStr(dr[dsCase.CaseManagement.ReferringProviderIdColumn.ColumnName])},
                            { "txtRefProvider", MDVUtility.ToStr(dr[dsCase.CaseManagement.RefProviderNameColumn.ColumnName])},
                            { "ddlCaseType", MDVUtility.ToStr(dr[dsCase.CaseManagement.CaseTypeIdColumn.ColumnName])},
                            { "hfCaseNumber", MDVUtility.ToStr(dr[dsCase.CaseManagement.CaseNumberColumn.ColumnName])},
                            //SearchedfieldsJSON["ddlConditionCode1"]
                            { "txtPatientStatus", MDVUtility.ToStr(dr[dsCase.CaseManagement.PatientStatusColumn.ColumnName])},
                            { "txtFrequencyCode", MDVUtility.ToStr(dr[dsCase.CaseManagement.FrequencyCodeColumn.ColumnName])},
                            { "ddlConditionCode1", MDVUtility.ToStr(dr[dsCase.CaseManagement.ConditionCode1Column.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsCase.CaseManagement.IsActiveColumn.ColumnName])},
                            { "txtSourceofAdmit", MDVUtility.ToStr(dr[dsCase.CaseManagement.SourceOfAdmitColumn.ColumnName])},
                            { "txtTypeofAdmission", MDVUtility.ToStr(dr[dsCase.CaseManagement.TypeOfAdmissionColumn.ColumnName])},
                            { "txtAdmitDxCode", MDVUtility.ToStr(dr[dsCase.CaseManagement.IsActiveColumn.ColumnName])},
                            { "ddlConditionCode2", MDVUtility.ToStr(dr[dsCase.CaseManagement.ConditionCode2Column.ColumnName])},
                            { "ddlConditionCode3", MDVUtility.ToStr(dr[dsCase.CaseManagement.ConditionCode3Column.ColumnName])},
                            { "ddlConditionCode4", MDVUtility.ToStr(dr[dsCase.CaseManagement.ConditionCode4Column.ColumnName])},
                            { "txtOccurrenceCode1", MDVUtility.ToStr(dr[dsCase.CaseManagement.OccurrenceCode1Column.ColumnName])},
                            { "dtpOccurrenceDate1",  dr[dsCase.CaseManagement.OccurrenceDate1Column.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.CaseManagement.OccurrenceDate1Column.ColumnName]).ToShortDateString():""},
                            { "txtOccurrenceCode2", MDVUtility.ToStr(dr[dsCase.CaseManagement.OccurrenceCode2Column.ColumnName])},
                            { "dtpOccurrenceDate2", dr[dsCase.CaseManagement.OccurrenceDate2Column.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.CaseManagement.OccurrenceDate2Column.ColumnName]).ToShortDateString():""},
                            { "txtOccurrenceCode3", MDVUtility.ToStr(dr[dsCase.CaseManagement.OccurrenceCode3Column.ColumnName])},
                            { "dtpOccurrenceDate3",dr[dsCase.CaseManagement.OccurrenceDate3Column.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.CaseManagement.OccurrenceDate3Column.ColumnName]).ToShortDateString():""},
                            { "txtOccurrenceCode4", MDVUtility.ToStr(dr[dsCase.CaseManagement.OccurrenceCode4Column.ColumnName])},
                            { "dtpOccurrenceDate4",dr[dsCase.CaseManagement.OccurrenceDate4Column.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.CaseManagement.OccurrenceDate4Column.ColumnName]).ToShortDateString():""},
                            { "txtValueCode1", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode1Column.ColumnName])},
                            { "txtValueCode1Amount", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode1AmountColumn.ColumnName])},
                            { "txtValueCode2", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode2Column.ColumnName])},
                            { "txtValueCode2Amount", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode2AmountColumn.ColumnName])},
                            { "txtValueCode3", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode3Column.ColumnName])},
                            { "txtValueCode3Amount", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode3AmountColumn.ColumnName])},
                            { "txtValueCode4", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode4Column.ColumnName])},
                            { "txtValueCode4Amount", MDVUtility.ToStr(dr[dsCase.CaseManagement.ValueCode4AmountColumn.ColumnName])},
                            { "txtHospitalCaseNo", MDVUtility.ToStr(dr[dsCase.CaseManagement.HospitalCaseNoColumn.ColumnName])},
                            { "txtCaseId",MDVUtility.ToStr(dr[dsCase.CaseManagement.CaseNumberColumn.ColumnName])},


                        };
                            Dictionary<string, string> keyValue = null;
                            // WCNF Detail.
                            DSWCNF dswcnf = null;
                            BLObject<DSWCNF> objwcnf = BLLPatientObj.LoadWCNFCaseManagement(CaseId);
                            if (objwcnf.Data != null)
                            {
                                dswcnf = objwcnf.Data;
                                if (dswcnf.Tables[dswcnf.WCNFDetail.TableName].Rows.Count > 0)
                                {

                                    DataRow drwcnf = dswcnf.Tables[dswcnf.WCNFDetail.TableName].Rows[0];
                                    string IsYes = "false";
                                    string IsNo = "true";
                                    string Auto = "false";
                                    string NonAuto = "false";
                                    string No = "true";
                                    if (drwcnf[dswcnf.WCNFDetail.EmploymentRelateColumn] != DBNull.Value)
                                    {
                                        if (Convert.ToBoolean(drwcnf[dswcnf.WCNFDetail.EmploymentRelateColumn]))
                                        {
                                            IsYes = "true";
                                            IsNo = "false";
                                        }

                                    }
                                    if (drwcnf[dswcnf.WCNFDetail.AccidentColumn] != DBNull.Value)
                                    {
                                        if (drwcnf[dswcnf.WCNFDetail.AccidentColumn].ToString() == "Auto")
                                        {
                                            Auto = "true";
                                            No = "false";
                                        }
                                        else if (drwcnf[dswcnf.WCNFDetail.AccidentColumn].ToString() == "Non Auto")
                                        {
                                            NonAuto = "true";
                                            No = "false";
                                        }
                                    }
                                    keyValue = new Dictionary<string, string>
                            {
                            {"txtCaseAdjuster", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.CaseAdjusterNameColumn.ColumnName])},
                            {"hfCaseAdjusterId", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.CaseAdjusterIdColumn.ColumnName]) },
                            { "txtTelephone", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.PhoneNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.FaxColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.EmailColumn.ColumnName])},
                            { "txtPreAuth", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.PreAuthColumn.ColumnName])},
                            { "dtpInjuryDate",  drwcnf[dswcnf.WCNFDetail.InjuryDateColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(drwcnf[dswcnf.WCNFDetail.InjuryDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtReferral", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.ReferralColumn.ColumnName])},
                            { "chkYes", IsYes},
                            { "chkNo", IsNo},
                            { "txtAccidentCause", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.CauseOfAccidentColumn.ColumnName])},
                            { "chkAuto", Auto},
                            { "chkNonAuto", NonAuto},
                            { "chkAccidentNo",No},
                            { "txtState", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.ZipColumn.ColumnName])},
                            { "txthour", MDVUtility.ToStr(drwcnf[dswcnf.WCNFDetail.HourColumn.ColumnName])},
                            { "dtpHCFAField16DateFrom",  drwcnf[dswcnf.WCNFDetail.HCFAField16DateFromColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(drwcnf[dswcnf.WCNFDetail.HCFAField16DateFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField16DateTo", drwcnf[dswcnf.WCNFDetail.HCFAField16DateToColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(drwcnf[dswcnf.WCNFDetail.HCFAField16DateToColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField18DateFrom",  drwcnf[dswcnf.WCNFDetail.HCFAField18DateFromColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(drwcnf[dswcnf.WCNFDetail.HCFAField18DateFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField18DateTo",  drwcnf[dswcnf.WCNFDetail.HCFAField18DateToColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(drwcnf[dswcnf.WCNFDetail.HCFAField18DateToColumn.ColumnName]).ToShortDateString():""},
                        };

                                }
                            }
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CaseFill_JSON = js.Serialize(keyValues),
                                CaseFill_WCNF=js.Serialize(keyValue),
                                DocumentCount = dsCase.CaseManagement.Rows[0][dsCase.CaseManagement.DocumentCountColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
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
        private string FillWCNFDetail(long CaseId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CaseId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSWCNF dsCase = null;
                    BLObject<DSWCNF> obj = BLLPatientObj.LoadWCNFCaseManagement(CaseId);
                    if (obj.Data != null)
                    {
                        dsCase = obj.Data;
                        if (dsCase.Tables[dsCase.WCNFDetail.TableName].Rows.Count > 0)
                        {

                            DataRow dr = dsCase.Tables[dsCase.WCNFDetail.TableName].Rows[0];
                            string IsYes = "false";
                            string IsNo = "true";
                            string Auto = "false";
                            string NonAuto = "false";
                            string No = "true";
                            if (dr[dsCase.WCNFDetail.EmploymentRelateColumn] != DBNull.Value)
                            {
                                if (Convert.ToBoolean(dr[dsCase.WCNFDetail.EmploymentRelateColumn]))
                                {
                                    IsYes = "true";
                                    IsNo = "false";
                                }

                            }
                            if (dr[dsCase.WCNFDetail.AccidentColumn] != DBNull.Value)
                            {
                                if (dr[dsCase.WCNFDetail.AccidentColumn].ToString() == "Auto")
                                {
                                    Auto = "true";
                                    No = "false";
                                }
                                else if (dr[dsCase.WCNFDetail.AccidentColumn].ToString() == "Non Auto")
                                {
                                    NonAuto = "true";
                                    No = "false";
                                }
                            }
                            var keyValues = new Dictionary<string, string>
                            {
                            {"txtCaseAdjuster", MDVUtility.ToStr(dr[dsCase.WCNFDetail.CaseAdjusterNameColumn.ColumnName])},
                            {"hfCaseAdjusterId", MDVUtility.ToStr(dr[dsCase.WCNFDetail.CaseAdjusterIdColumn.ColumnName]) },
                            { "txtTelephone", MDVUtility.ToStr(dr[dsCase.WCNFDetail.PhoneNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsCase.WCNFDetail.FaxColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsCase.WCNFDetail.EmailColumn.ColumnName])},
                            { "txtPreAuth", MDVUtility.ToStr(dr[dsCase.WCNFDetail.PreAuthColumn.ColumnName])},
                            { "dtpInjuryDate",  dr[dsCase.WCNFDetail.InjuryDateColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.WCNFDetail.InjuryDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtReferral", MDVUtility.ToStr(dr[dsCase.WCNFDetail.ReferralColumn.ColumnName])},
                            { "chkYes", IsYes},
                            { "chkNo", IsNo},
                            { "txtAccidentCause", MDVUtility.ToStr(dr[dsCase.WCNFDetail.CauseOfAccidentColumn.ColumnName])},
                            { "chkAuto", Auto},
                            { "chkNonAuto", NonAuto},
                            { "chkAccidentNo",No},
                            { "txtState", MDVUtility.ToStr(dr[dsCase.WCNFDetail.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsCase.WCNFDetail.ZipColumn.ColumnName])},
                            { "txthour", MDVUtility.ToStr(dr[dsCase.WCNFDetail.HourColumn.ColumnName])},
                            { "dtpHCFAField16DateFrom",  dr[dsCase.WCNFDetail.HCFAField16DateFromColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.WCNFDetail.HCFAField16DateFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField16DateTo", dr[dsCase.WCNFDetail.HCFAField16DateToColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.WCNFDetail.HCFAField16DateToColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField18DateFrom",  dr[dsCase.WCNFDetail.HCFAField18DateFromColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.WCNFDetail.HCFAField18DateFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpHCFAField18DateTo",  dr[dsCase.WCNFDetail.HCFAField18DateToColumn.ColumnName].ToString()!=""? MDVUtility.ToDateTime(dr[dsCase.WCNFDetail.HCFAField18DateToColumn.ColumnName]).ToShortDateString():""},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CaseWCNF_Fill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
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
        private string LinkVisitCase(Int64 VisitId, Int64 CaseId)
        {

            try
            {
                DSVisits dsVisits = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientsVisits(VisitId, 0, 0, 0, null, null, "", "", "");
                if (obj.Data != null)
                {
                    dsVisits = obj.Data;
                    if (dsVisits.PatientVisits.Rows.Count > 0)
                    {
                        DSVisits.PatientVisitsRow dr = (DSVisits.PatientVisitsRow)dsVisits.Tables[dsVisits.PatientVisits.TableName].Rows[0];
                        dr.CaseMgmtId = CaseId;

                        BLObject<DSVisits> objVisit = BLLVisitsObj.UpdatePatientsVisit(dsVisits);

                        if (objVisit.Data != null)
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
                                Message = objVisit.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

        }

        #endregion


        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_CASE":
                    {
                        string fieldsJSON = context.Request["CaseData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SaveCase(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_CASE":
                    {
                        string fieldsJSON = context.Request["CaseData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);

                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);

                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = SearchCase(fieldsJSON, PatientID, CaseID, PageNumber, RowsPerPage);


                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CASE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = FillCase(PatientID, CaseID);


                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_WCNF_DETAIL":
                    {
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = FillWCNFDetail(CaseID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_CASE":
                    {
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = DeleteCase(CaseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CASE":
                    {
                        string fieldsJSON = context.Request["CaseData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = UpdateCase(fieldsJSON, PatientID, CaseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CASE_ACTIVE_INACTIVE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateCaseIsActive(PatientID, CaseID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_VISIT":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = LinkVisitCase(VisitID, CaseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DOCUMENT_DETAIL":
                    {
                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                        string strJSONData = SearchCaseDocuments(MDVUtility.ToStr(CaseID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}