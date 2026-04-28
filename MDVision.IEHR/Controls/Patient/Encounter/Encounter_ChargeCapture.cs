using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Schedule;

namespace MDVision.IEHR.Controls.Patient.Encounter
{
    public class Encounter_ChargeCapture
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLVisits BLLVisitsObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLSchedule BLLScheduleObj = null;
        public Encounter_ChargeCapture()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLBillingObj = new BLLBilling();
            BLLVisitsObj = new BLLVisits();
            BLLPatientObj = new BLLPatient();
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Encounter_ChargeCapture _obj = null;
        public static Encounter_ChargeCapture Instance()
        {
            if (_obj == null)
                _obj = new Encounter_ChargeCapture();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Load all the Load Visit Charges for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The VisitID identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchCharge(string fieldsJSON, Int64 VisitID)
        {
            try
            {



                string ClaimNumber;
                DSCharge dsCharge = null;
                BLObject<DSCharge> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON == null)
                    obj = null; //BusinessWrapper.Visits.BusinessObj.LoadPatientsVisits(VisitID, PatientID, 0, 0, null, null, 0, 0, "");
                else
                {
                    Int64 ProviderId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtProvider"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfProvider"]) : 0;
                    Int64 FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtFacility"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfFacility"]) : 0;
                    DateTime? FromAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateFrom"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateFrom"]) : null;

                    DateTime? ToAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateTo"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateTo"]) : null;
                    ClaimNumber = SearchedfieldsJSON["txtClaimNumber"];
                    Int64 visitStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                    string IsActive = SearchedfieldsJSON["ddlActive"];
                    obj = null;//BusinessWrapper.Visits.BusinessObj.LoadPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, visitStatusId, IsActive);
                    //obj = BLLPatientObj.LoadPatientDocument("", PatientID, SearchedfieldsJSON["txtAccountNumber"], SearchedfieldsJSON["txtPatientLastName"], SearchedfieldsJSON["txtPatientFirstName"], FromDOS, ToDOS, FromEntryDate, ToEntryDate, SearchedfieldsJSON["ddlEnteredBy"], MDVUtility.ToInt64(SearchedfieldsJSON["ddlEnteredBy"]), MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignedtoReview"]), "", MDVUtility.ToInt32(SearchedfieldsJSON["hfDocumentId"]), SearchedfieldsJSON["ddlActive"]);
                }
                dsCharge = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0 ? dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count : 0,
                        ChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ChargeCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }



                //else
                //{
                //    var response = new
                //    {
                //        status = false,
                //        Message = "No Record Found"
                //    };
                //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //}

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
        /// Saves the Charge.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveCharge(string fieldsJSON, Int64 VisitId, string ChargeId)
        {
            try
            {
                DSCharge dsCharge = new DSCharge();

                DSCharge.PatientChargesRow dr = BuildChargeRow(fieldsJSON, dsCharge, VisitId, ChargeId);

                #region Database Insertion
                dsCharge.PatientCharges.AddPatientChargesRow(dr);
                BLObject<DSCharge> obj = BLLBillingObj.InsertPatientCharges(dsCharge);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ChargeId = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0][dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]
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

        public DSCharge.PatientChargesRow BuildChargeRow(string fieldsJSON, DSCharge dsCharge, Int64 VisitId, string ChargeId, DSCharge.PatientChargesRow dr = null)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            if (dr == null)
            {
                dr = dsCharge.PatientCharges.NewPatientChargesRow();

            }
            //else
            //{

                if (SearchedfieldsJSON.ContainsKey("txtChargeOrder" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtChargeOrder" + ChargeId]))
                    dr.ChargeOrder = MDVUtility.ToInt32(SearchedfieldsJSON["txtChargeOrder" + ChargeId]);
            //}


            //if (!string.IsNullOrEmpty(ChargeId))
            //    dr.ChargeCapId = MDVUtility.ToInt64(ChargeId);

            dr.VisitId = MDVUtility.ToInt64(VisitId);
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom" + ChargeId]))
                dr.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom" + ChargeId]);
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo" + ChargeId]))
                dr.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo" + ChargeId]);
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPT" + ChargeId]))
            {
                dr.CPTCode = MDVUtility.ToStr(SearchedfieldsJSON["txtCPT" + ChargeId]);
                dr.CPTDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfCPTDescription" + ChargeId]);
            }
            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtUnits" + ChargeId]))
                dr.Units = MDVUtility.ToDouble(SearchedfieldsJSON["txtUnits" + ChargeId]);

            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier1" + ChargeId]))
                dr.Modifier1 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier1" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.Modifier1Column] = DBNull.Value;

            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier2" + ChargeId]))
                dr.Modifier2 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier2" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.Modifier2Column] = DBNull.Value;

            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier3" + ChargeId]))
                dr.Modifier3 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier3" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.Modifier3Column] = DBNull.Value;

            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier4" + ChargeId]))
                dr.Modifier4 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier4" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.Modifier4Column] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtICDPointer1" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer1" + ChargeId]))
                dr.ICDPointer1 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer1" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.ICDPointer1Column] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtICDPointer2" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer2" + ChargeId]))
                dr.ICDPointer2 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer2" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.ICDPointer2Column] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtICDPointer3" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer3" + ChargeId]))
                dr.ICDPointer3 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer3" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.ICDPointer3Column] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtICDPointer4" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer4" + ChargeId]))
                dr.ICDPointer4 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer4" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.ICDPointer4Column] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtPOS" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPOS" + ChargeId]))
                dr.POSCode = MDVUtility.ToStr(SearchedfieldsJSON["txtPOS" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.POSCodeColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("txtTOS" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTOS" + ChargeId]))
                dr.TOSCode = MDVUtility.ToStr(SearchedfieldsJSON["txtTOS" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.TOSCodeColumn] = DBNull.Value;
            dr.EMG = MDVUtility.ToStr(SearchedfieldsJSON["chkEMG" + ChargeId]) == "True" ? true : false;
            if (SearchedfieldsJSON.ContainsKey("txtFEE" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtFEE" + ChargeId]))
                dr.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFEE" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.FeeColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtINSCharges" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtINSCharges" + ChargeId]))
                dr.InsCharges = MDVUtility.ToDouble(SearchedfieldsJSON["txtINSCharges" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.InsChargesColumn] = 0.0;
            if (SearchedfieldsJSON.ContainsKey("txtPATCharges" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPATCharges" + ChargeId]))
                dr.PatCharges = MDVUtility.ToDouble(SearchedfieldsJSON["txtPATCharges" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.PatChargesColumn] = 0.0;
            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["AllowedAmt"]))
            //    dr.AllowedAmt = MDVUtility.ToDouble(SearchedfieldsJSON["AllowedAmt"]);
            if (SearchedfieldsJSON.ContainsKey("txtCOPAY" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtCOPAY" + ChargeId]))
                dr.Copay = MDVUtility.ToDouble(SearchedfieldsJSON["txtCOPAY" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.CopayColumn] = 0.0;
            if (SearchedfieldsJSON.ContainsKey("txtPriorAuthorization" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPriorAuthorization" + ChargeId]))
                dr.PAN = MDVUtility.ToStr(SearchedfieldsJSON["txtPriorAuthorization" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.PANColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("hfAuthorizationId" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["hfAuthorizationId" + ChargeId]))
                dr.AuthorizeId = MDVUtility.ToStr(SearchedfieldsJSON["hfAuthorizationId" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.AuthorizeIdColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("txtExpectedFee" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtExpectedFee" + ChargeId]))
                dr.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee" + ChargeId]);

            if (SearchedfieldsJSON.ContainsKey("txtCLIA" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtCLIA" + ChargeId]))
                dr.CLIA = MDVUtility.ToStr(SearchedfieldsJSON["txtCLIA" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.CLIAColumn] = DBNull.Value;

            // imp-621
            if (SearchedfieldsJSON.ContainsKey("txtdrugCodeCost" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtdrugCodeCost" + ChargeId]))
                dr.DrugCodeCost = MDVUtility.ToDouble(SearchedfieldsJSON["txtdrugCodeCost" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.DrugCodeCostColumn] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtNDC" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDC" + ChargeId]))
                dr.NDC = MDVUtility.ToStr(SearchedfieldsJSON["txtNDC" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.NDCColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("txtNDCDescription" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCDescription" + ChargeId]))
                dr.NDCDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtNDCDescription" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.NDCDescriptionColumn] = DBNull.Value;
            
            if (SearchedfieldsJSON.ContainsKey("txtNDCUnit" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCUnit" + ChargeId]))
                dr.NDCUnit = MDVUtility.ToDouble(SearchedfieldsJSON["txtNDCUnit" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.NDCUnitColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("txtNDCUnitPrice" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCUnitPrice" + ChargeId]))
                dr.NDCUnitPrice = MDVUtility.ToDouble(SearchedfieldsJSON["txtNDCUnitPrice" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.NDCUnitPriceColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("ddlNDCMeasurement" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlNDCMeasurement" + ChargeId]))
                dr.NDCMeasurCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNDCMeasurement" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.NDCMeasurCodeIdColumn] = DBNull.Value;
            //dr.AssgBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;
            if (SearchedfieldsJSON.ContainsKey("txtComments" + ChargeId) )
                dr.LineNotes = MDVUtility.ToStr(SearchedfieldsJSON["txtComments" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.LineNotesColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("dtpEOD" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpEOD" + ChargeId]))
                dr.EOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEOD" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.EODColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("txtStatus" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtStatus" + ChargeId]))
                dr.Status = MDVUtility.ToStr(SearchedfieldsJSON["txtStatus" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.StatusColumn] = DBNull.Value;
            if (SearchedfieldsJSON.ContainsKey("chkReportCPTDesc" + ChargeId))
                dr.IsReportCPTDesc = MDVUtility.ToBool(SearchedfieldsJSON["chkReportCPTDesc" + ChargeId]);
            else
                dr[dsCharge.PatientCharges.IsReportCPTDescColumn] = DBNull.Value;

            if (SearchedfieldsJSON.ContainsKey("txtStartTime" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtStartTime" + ChargeId]))
                dr.StartTime = SearchedfieldsJSON["txtStartTime" + ChargeId];

            if (SearchedfieldsJSON.ContainsKey("txtEndTime" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtEndTime" + ChargeId]))
                dr.EndTime = SearchedfieldsJSON["txtEndTime" + ChargeId];

            if (SearchedfieldsJSON.ContainsKey("txtTimeUnits" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTimeUnits" + ChargeId]))
                dr.TimeUnits = SearchedfieldsJSON["txtTimeUnits" + ChargeId];
            else
                dr[dsCharge.PatientCharges.TimeUnitsColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtBaseUnits" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtBaseUnits" + ChargeId]))
                dr.BaseUnits = SearchedfieldsJSON["txtBaseUnits" + ChargeId];
            else
                dr[dsCharge.PatientCharges.BaseUnitsColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtRiskUnits" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtRiskUnits" + ChargeId]))
                dr.RiskUnits = SearchedfieldsJSON["txtRiskUnits" + ChargeId];
            else
                dr[dsCharge.PatientCharges.RiskUnitsColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtTotalMinutes" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTotalMinutes" + ChargeId]))
                dr.TotalMinutes = SearchedfieldsJSON["txtTotalMinutes" + ChargeId];
            else
                dr[dsCharge.PatientCharges.TotalMinutesColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtTotalUnits" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTotalUnits" + ChargeId]))
                dr.TotalUnits = SearchedfieldsJSON["txtTotalUnits" + ChargeId];
            else
                dr[dsCharge.PatientCharges.TotalUnitsColumn] = 0.0;

            if (SearchedfieldsJSON.ContainsKey("txtRoundBiuldUnits" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtRoundBiuldUnits" + ChargeId]))
                dr.RoundBilledUnitsId = SearchedfieldsJSON["txtRoundBiuldUnits" + ChargeId];
            else
                dr[dsCharge.PatientCharges.RoundBilledUnitsIdColumn] = 0.0;

            //if (SearchedfieldsJSON.ContainsKey("txtMasterChargeId" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtMasterChargeId" + ChargeId]))
            //    dr.MasterChargeId = MDVUtility.ToInt64(SearchedfieldsJSON["txtMasterChargeId" + ChargeId]);

            //if (SearchedfieldsJSON.ContainsKey("dtpHoldFrom" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpHoldFrom" + ChargeId]))
            //    dr.HoldFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHoldFrom" + ChargeId]);
            //else
            //    dr[dsCharge.PatientCharges.HoldFromColumn] = DBNull.Value;


            //if (SearchedfieldsJSON.ContainsKey("chkHold" + ChargeId))
            //{
            //    dr.IsHold = MDVUtility.ToStr(SearchedfieldsJSON["chkHold" + ChargeId]) == "True" ? true : false;

            //    if (dr.IsHold)
            //    {
            //        if (SearchedfieldsJSON.ContainsKey("txtHoldDays" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtHoldDays" + ChargeId]))
            //            dr.HoldDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtHoldDays" + ChargeId]);
            //        else
            //            dr[dsCharge.PatientCharges.HoldDaysColumn.ColumnName] = DBNull.Value;
            //    }
            //    else
            //    {

            //        dr.HoldDays = 0;
            //    }
            //}


            if (SearchedfieldsJSON.ContainsKey("chkPrimary" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["chkPrimary" + ChargeId]))
                dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["chkPrimary" + ChargeId]) == "True" ? true : false;

            if (SearchedfieldsJSON.ContainsKey("hfPatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                dr.PatientId = MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]);

            //if (SearchedfieldsJSON.ContainsKey("txtChargeOrder" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtChargeOrder" + ChargeId]))
            //       drcharge.ChargeOrder = MDVUtility.ToInt32(SearchedfieldsJSON["txtChargeOrder" + ChargeId]);

            Controls.Patient.Encounter.Encounter_Visits.Instance().chargesRowCount += 1;
            //dr.ChargeOrder = Controls.Patient.Encounter.Encounter_Visits.Instance().chargesRowCount;

            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive" + ChargeId]) == "True" ? true : false;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;

            return dr;
        }

        /// <summary>
        /// Updates the Charge.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseId">The Charge identifier.</param>
        /// <returns></returns>
        private string UpdateCharge(string fieldsJSON, Int64 ChargeId, string RefCtrl = null)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                string selectorID = MDVUtility.ToStr(ChargeId); ;
                DSCharge dsCharge = null;
                BLObject<DSCharge> obj = BLLBillingObj.LoadPatientCharges(ChargeId, "", "", 0, 0, "", 0, "", "", null, null, 0);
                if (RefCtrl != null && RefCtrl.ToLower() == "charge detail")
                {
                    selectorID = "";
                }

                //obj =BLLVisitsObj.LoadPatientsVisits(0, ChargeId, 0, 0, null, null, 0, 0, "");
                dsCharge = obj.Data;

                //int totalRows = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count;
                foreach (DSCharge.PatientChargesRow drcharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom" + selectorID]))
                        drcharge.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom" + selectorID]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo" + selectorID]))
                        drcharge.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo" + selectorID]);
                    if (RefCtrl != null && RefCtrl.ToLower() == "charge detail")
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPT" + selectorID]))
                        {
                            drcharge.CPTCode = MDVUtility.ToStr(SearchedfieldsJSON["txtCPT" + selectorID]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("hfCPTDescription" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["hfCPTDescription" + selectorID]))
                        {
                            drcharge.CPTDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfCPTDescription" + selectorID]);
                        }
                    }
                    if (SearchedfieldsJSON.ContainsKey("txtICDPointer1" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer1" + ChargeId]))
                        drcharge.ICDPointer1 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer1" + ChargeId]);
                    else
                        drcharge[dsCharge.PatientCharges.ICDPointer1Column] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("txtICDPointer2" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer2" + ChargeId]))
                        drcharge.ICDPointer2 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer2" + ChargeId]);
                    else
                        drcharge[dsCharge.PatientCharges.ICDPointer2Column] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("txtICDPointer3" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer3" + ChargeId]))
                        drcharge.ICDPointer3 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer3" + ChargeId]);
                    else
                        drcharge[dsCharge.PatientCharges.ICDPointer3Column] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("txtICDPointer4" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICDPointer4" + ChargeId]))
                        drcharge.ICDPointer4 = MDVUtility.ToStr(SearchedfieldsJSON["txtICDPointer4" + ChargeId]);
                    else
                        drcharge[dsCharge.PatientCharges.ICDPointer4Column] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier1" + selectorID]))
                        drcharge.Modifier1 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier1" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.Modifier1Column] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier2" + selectorID]))
                        drcharge.Modifier2 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier2" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.Modifier2Column] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier3" + selectorID]))
                        drcharge.Modifier3 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier3" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.Modifier3Column] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtModifier4" + selectorID]))
                        drcharge.Modifier4 = MDVUtility.ToStr(SearchedfieldsJSON["txtModifier4" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.Modifier4Column] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtUnits" + selectorID]))
                        drcharge.Units = MDVUtility.ToDouble(SearchedfieldsJSON["txtUnits" + selectorID]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPOS" + selectorID]))
                        drcharge.POSCode = MDVUtility.ToStr(SearchedfieldsJSON["txtPOS" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.POSCodeColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTOS" + selectorID]))
                        drcharge.TOSCode = MDVUtility.ToStr(SearchedfieldsJSON["txtTOS" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.TOSCodeColumn] = DBNull.Value;
                    drcharge.EMG = MDVUtility.ToStr(SearchedfieldsJSON["chkEMG" + selectorID]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFEE" + selectorID]))
                        drcharge.Fee = MDVUtility.ToDouble(SearchedfieldsJSON["txtFEE" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.FeeColumn] = 0.0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtINSCharges" + selectorID]))
                        drcharge.InsCharges = MDVUtility.ToDouble(SearchedfieldsJSON["txtINSCharges" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.InsChargesColumn] = 0.0;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPATCharges" + selectorID]))
                        drcharge.PatCharges = MDVUtility.ToDouble(SearchedfieldsJSON["txtPATCharges" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.PatChargesColumn] = 0.0;
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["AllowedAmt"]))
                    //    drcharge.AllowedAmt = MDVUtility.ToDouble(SearchedfieldsJSON["AllowedAmt"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCOPAY" + selectorID]))
                        drcharge.Copay = MDVUtility.ToDouble(SearchedfieldsJSON["txtCOPAY" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.CopayColumn] = 0.0;
                    if (SearchedfieldsJSON.ContainsKey("txtPriorAuthorization" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPriorAuthorization" + selectorID]))
                        drcharge.PAN = MDVUtility.ToStr(SearchedfieldsJSON["txtPriorAuthorization" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.PANColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("hfAuthorizationId" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["hfAuthorizationId" + ChargeId]))
                        drcharge.AuthorizeId = MDVUtility.ToStr(SearchedfieldsJSON["hfAuthorizationId" + ChargeId]);
                    else
                        drcharge[dsCharge.PatientCharges.AuthorizeIdColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtExpectedFee" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtExpectedFee" + selectorID]))
                        drcharge.ExpectedFee = MDVUtility.ToDouble(SearchedfieldsJSON["txtExpectedFee" + selectorID]);

                    if (SearchedfieldsJSON.ContainsKey("txtCLIA" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtCLIA" + selectorID]))
                        drcharge.CLIA = MDVUtility.ToStr(SearchedfieldsJSON["txtCLIA" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.CLIAColumn] = DBNull.Value;
                    // imp-621
                    if (SearchedfieldsJSON.ContainsKey("txtdrugCodeCost" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtdrugCodeCost" + selectorID]))
                        drcharge.DrugCodeCost = MDVUtility.ToDouble(SearchedfieldsJSON["txtdrugCodeCost" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.DrugCodeCostColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("chkReportCPTDesc" + selectorID))
                        drcharge.IsReportCPTDesc = MDVUtility.ToBool(SearchedfieldsJSON["chkReportCPTDesc" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.IsReportCPTDescColumn] = DBNull.Value;


                    if (SearchedfieldsJSON.ContainsKey("txtNDC" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDC" + selectorID]))
                        drcharge.NDC = MDVUtility.ToStr(SearchedfieldsJSON["txtNDC" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.NDCColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtNDCDescription" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCDescription" + selectorID]))
                        drcharge.NDCDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtNDCDescription" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.NDCDescriptionColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtNDCUnit" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCUnit" + selectorID]))
                        drcharge.NDCUnit = MDVUtility.ToDouble(SearchedfieldsJSON["txtNDCUnit" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.NDCUnitColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtNDCUnitPrice" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtNDCUnitPrice" + selectorID]))
                        drcharge.NDCUnitPrice = MDVUtility.ToDouble(SearchedfieldsJSON["txtNDCUnitPrice" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.NDCUnitPriceColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("ddlNDCMeasurement" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlNDCMeasurement" + selectorID]))
                        drcharge.NDCMeasurCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlNDCMeasurement" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.NDCMeasurCodeIdColumn] = DBNull.Value;
                    //drcharge.AssgBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;
                    if (SearchedfieldsJSON.ContainsKey("txtComments" + selectorID))
                        drcharge.LineNotes = MDVUtility.ToStr(SearchedfieldsJSON["txtComments" + selectorID]);
                    //else
                    //    drcharge[dsCharge.PatientCharges.LineNotesColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("dtpEOD" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpEOD" + selectorID]))
                        drcharge.EOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEOD" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.EODColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtStatus" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtStatus" + selectorID]))
                        drcharge.Status = MDVUtility.ToStr(SearchedfieldsJSON["txtStatus" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.StatusColumn] = DBNull.Value;
                    if (SearchedfieldsJSON.ContainsKey("txtMasterChargeId" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtMasterChargeId" + selectorID]))
                        drcharge.MasterChargeId = MDVUtility.ToInt64(SearchedfieldsJSON["txtMasterChargeId" + selectorID]);
                    else
                        drcharge[dsCharge.PatientCharges.MasterChargeIdColumn] = DBNull.Value;

                    //if (SearchedfieldsJSON.ContainsKey("dtpHoldFrom" + ChargeId) && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpHoldFrom" + ChargeId]))
                    //    drcharge.HoldFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHoldFrom" + ChargeId]);
                    //else
                    //    drcharge[dsCharge.PatientCharges.HoldFromColumn] = DBNull.Value;

                    //if (SearchedfieldsJSON.ContainsKey("chkHold" + selectorID))
                    //{
                    //    drcharge.IsHold = MDVUtility.ToStr(SearchedfieldsJSON["chkHold" + selectorID]) == "True" ? true : false;

                    //    if (drcharge.IsHold)
                    //    {
                    //        if (SearchedfieldsJSON.ContainsKey("txtHoldDays" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtHoldDays" + selectorID]))
                    //            drcharge.HoldDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtHoldDays" + selectorID]);
                    //        else
                    //            drcharge[dsCharge.PatientCharges.HoldDaysColumn.ColumnName] = DBNull.Value;
                    //    }
                    //    else
                    //    {
                    //        drcharge.HoldDays = 0;
                    //    }
                    //}


                    if (SearchedfieldsJSON.ContainsKey("chkPrimary" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["chkPrimary" + selectorID]))
                        drcharge.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["chkPrimary" + selectorID]) == "True" ? true : false;

                    if (SearchedfieldsJSON.ContainsKey("txtChargeOrder" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtChargeOrder" + selectorID]))
                        drcharge.ChargeOrder = MDVUtility.ToInt32(SearchedfieldsJSON["txtChargeOrder" + selectorID]);

                    if (SearchedfieldsJSON.ContainsKey("chkActive" + selectorID))
                        drcharge.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive" + ChargeId]) == "True" ? true : false;
                    if (SearchedfieldsJSON.ContainsKey("txtStartTime" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtStartTime" + selectorID]))
                        drcharge.StartTime = SearchedfieldsJSON["txtStartTime" + selectorID];

                    if (SearchedfieldsJSON.ContainsKey("txtEndTime" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtEndTime" + selectorID]))
                        drcharge.EndTime = SearchedfieldsJSON["txtEndTime" + selectorID];

                    if (SearchedfieldsJSON.ContainsKey("txtTimeUnits" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTimeUnits" + selectorID]))
                        drcharge.TimeUnits = SearchedfieldsJSON["txtTimeUnits" + selectorID];
                    else
                        drcharge[dsCharge.PatientCharges.TimeUnitsColumn] = 0.0;
                    if (SearchedfieldsJSON.ContainsKey("txtBaseUnits" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtBaseUnits" + selectorID]))
                        drcharge.BaseUnits = SearchedfieldsJSON["txtBaseUnits" + selectorID];
                    else
                        drcharge[dsCharge.PatientCharges.BaseUnitsColumn] = 0.0;
                    if (SearchedfieldsJSON.ContainsKey("txtRiskUnits" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtRiskUnits" + selectorID]))
                        drcharge.RiskUnits = SearchedfieldsJSON["txtRiskUnits" + selectorID];
                    else
                        drcharge[dsCharge.PatientCharges.RiskUnitsColumn] = 0.0;
                    if (SearchedfieldsJSON.ContainsKey("txtTotalMinutes" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTotalMinutes" + selectorID]))
                        drcharge.TotalMinutes = SearchedfieldsJSON["txtTotalMinutes" + selectorID];

                    if (SearchedfieldsJSON.ContainsKey("txtTotalUnits" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtTotalUnits" + selectorID]))
                        drcharge.TotalUnits = SearchedfieldsJSON["txtTotalUnits" + selectorID];

                    if (SearchedfieldsJSON.ContainsKey("txtRoundBiuldUnits" + selectorID) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtRoundBiuldUnits" + selectorID]))
                        drcharge.RoundBilledUnitsId = SearchedfieldsJSON["txtRoundBiuldUnits" + selectorID];
                    else
                        drcharge[dsCharge.PatientCharges.RoundBilledUnitsIdColumn] = 0.0;

                    drcharge.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drcharge.ModifiedOn = DateTime.Now;
                }

                #region Database Updation

                if (dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count > 0)
                {
                    BLObject<DSCharge> objCharge = BLLBillingObj.UpdatePatientCharges(dsCharge);
                    if (objCharge.Data != null)
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
                            Message = objCharge.Message
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            return null;
        }


        private string DeleteCharge(long ChargeCapId)
        {

            try
            {
                if (ChargeCapId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePatientCharges(ChargeCapId);
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

        private string SortPatientCharges(long fromId, long toId)
        {
            BLObject<DSCharge> objPatientChargeFrom;
            BLObject<DSCharge> objPatientChargeTo;

            DSCharge dsBillFrom = new DSCharge();
            DSCharge dsBillTo = new DSCharge();

            objPatientChargeFrom = BLLBillingObj.LoadPatientCharges(fromId, null, null, 0, 0, null, 0, "", null, null, null, 0);
            objPatientChargeTo = BLLBillingObj.LoadPatientCharges(toId, null, null, 0, 0, null, 0, "", null, null, null, 0);

            if (objPatientChargeTo.Data != null && objPatientChargeFrom.Data != null)
            {

                DSCharge mergedDataSet = new DSCharge();
                mergedDataSet.Merge(objPatientChargeFrom.Data);
                mergedDataSet.Merge(objPatientChargeTo.Data);

                int from_sortId = MDVUtility.ToInt(mergedDataSet.Tables[mergedDataSet.PatientCharges.TableName].Rows[0][mergedDataSet.PatientCharges.ChargeOrderColumn.ColumnName]);
                int to_sortId = MDVUtility.ToInt(mergedDataSet.Tables[mergedDataSet.PatientCharges.TableName].Rows[1][mergedDataSet.PatientCharges.ChargeOrderColumn.ColumnName]);
                mergedDataSet.Tables[mergedDataSet.PatientCharges.TableName].Rows[0][mergedDataSet.PatientCharges.ChargeOrderColumn.ColumnName] = to_sortId;
                mergedDataSet.Tables[mergedDataSet.PatientCharges.TableName].Rows[1][mergedDataSet.PatientCharges.ChargeOrderColumn.ColumnName] = from_sortId;

                //PMS-1640
                foreach (DSCharge.PatientChargesRow drcharge in mergedDataSet.Tables[mergedDataSet.PatientCharges.TableName].Rows)
                {
                    drcharge.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drcharge.ModifiedOn = DateTime.Now;
                }

                #region Database Updation

                BLObject<DSCharge> objCharge = BLLBillingObj.UpdatePatientCharges(mergedDataSet);

                if (objCharge.Data != null)
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
                        Message = objCharge.Message
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
                    Message = ""
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }

        private string FillCPTFee(Int64 VisitId, string CPTCode, Int64 ProviderId, Int64 FacilityId, Int64 PracticeId, DateTime ChargeDOS, Int64 PatientInsuranceId = 0
                                , string POSCode = "", string Modifier1 = "", string Modifier2 = "", string Modifier3 = "", string Modifier4 = "")
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj = null;
                obj = BLLBillingObj.LoadCPTFee(VisitId, CPTCode, ProviderId, FacilityId, PracticeId, ChargeDOS, PatientInsuranceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4);
                dsCodes = obj.Data;
                var response = new
                {
                    status = true,
                    CPTCount = dsCodes.Tables[dsCodes.CPTFee.TableName].Rows.Count,
                    CPTLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.CPTFee.TableName]),
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

        private string FillAllCPTFee(string fieldsJSON, long VisitId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON != null)
                {
                    List<string> reslist = new List<string>();
                    foreach (var item in SearchedfieldsJSON)
                    {
                        long FacilityId = MDVUtility.ToLong(MDVUtility.ToStr(item["Facility"]));
                        long ProviderId = MDVUtility.ToLong(MDVUtility.ToStr(item["Provider"]));
                        long PracticeId = MDVUtility.ToLong(MDVUtility.ToStr(item["Practice"]));
                        long PatientInsuranceId = MDVUtility.ToLong(MDVUtility.ToStr(item["PatientInsuraceId"]));
                        long ChargeId = MDVUtility.ToLong(MDVUtility.ToStr(item["ChargeId"]));

                        string POSCode = MDVUtility.ToStr(item["POSCode"]);
                        string CPTCode = MDVUtility.ToStr(item["CPT"]);
                        string Modifier1 = MDVUtility.ToStr(item["Modifier1"]);
                        string Modifier2 = MDVUtility.ToStr(item["Modifier2"]);
                        string Modifier3 = MDVUtility.ToStr(item["Modifier3"]);
                        string Modifier4 = MDVUtility.ToStr(item["Modifier4"]);
                        DateTime ChargeDOS = MDVUtility.ToDateTime(item["ChargeDOS"]);

                        DSCodes dsCodes = null;
                        BLObject<DSCodes> obj = null;
                        obj = BLLBillingObj.LoadCPTFee(VisitId, CPTCode, ProviderId, FacilityId, PracticeId, ChargeDOS, PatientInsuranceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4);
                        if (obj.Data != null)
                        {
                            dsCodes = obj.Data;
                            var response = new
                            {
                                status = true,
                                CPTCount = dsCodes.Tables[dsCodes.CPTFee.TableName].Rows.Count,
                                ChargeId = ChargeId,
                                CPTLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.CPTFee.TableName]),
                            };
                            reslist.Add((Newtonsoft.Json.JsonConvert.SerializeObject(response)));
                        }

                    }

                    if (reslist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AllCPTJsonList = Newtonsoft.Json.JsonConvert.SerializeObject(reslist),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
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


        private string ResubmitCharge(string VisitId, string ChargeId)
        {

            try
            {
                if (string.IsNullOrEmpty(VisitId) || string.IsNullOrEmpty(ChargeId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Resubmit_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    long visitId = MDVUtility.ToLong(VisitId);
                    long chargeId = MDVUtility.ToLong(ChargeId);

                    BLObject<bool> obj = BLLBillingClaimObj.UpdateVisitChargeStatus(3, visitId, chargeId);

                    if (obj.Data.Equals(true))
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Resubmit_Visit_Charge_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.Resubmit_Error_Message
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
        private string BillToPatientCharges(string ChargesIds, bool BillToPatient)
        {
            try
            {
                DSCharge dsCharge = new DSCharge();
                BLObject<DSCharge> obj = BLLBillingObj.BillToPatientCharges(ChargesIds, BillToPatient);
                string MsgToDisplay = "";
                if (BillToPatient)
                    MsgToDisplay = "Bill has been charged to patient";
                else
                    MsgToDisplay = "Bill has been charged to insurance";

                if (obj.Data != null)
                {
                    dsCharge = obj.Data;
                    if (dsCharge.PatientAndInsuranceCharges.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ChargesInsBalLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientAndInsuranceCharges.TableName]),
                            Message = MsgToDisplay
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false/*,
                            Message = AppPrivileges.No_Record_Message*/
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


        private string VoidAndReCreateClaim(string VisitId)
        {
            try
            {
                if (string.IsNullOrEmpty(VisitId))
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

                    BLObject<string> obj = BLLVisitsObj.VoidAndReCreateVisit(VisitId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            visitId = VisitId,
                            Message = Common.AppPrivileges.Void_And_ReCreate_Success_Message
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

        private string LoadClaimErrors(string VisitId)
        {
            try
            {
                if (string.IsNullOrEmpty(VisitId))
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
                    BLObject<DSVisits> obj = BLLVisitsObj.LoadClaimErrors(0, MDVUtility.ToLong(VisitId));
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimErrorsCount = obj.Data.Tables[obj.Data.ClaimErrors.TableName].Rows.Count,
                            ClaimErrorsJSON = MDVUtility.JSON_DataTable(obj.Data.Tables[obj.Data.ClaimErrors.TableName]),
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

        private string SplitClaim(long VisitId)
        {
            try
            {
                if (VisitId != 0)
                {

                    BLObject<DSVisits> obj = BLLVisitsObj.SplitClaim(VisitId);
                    if (obj.Data != null)
                    {
                        DSVisits dsVisit = obj.Data;

                        var response = new
                        {
                            status = true,
                            VisitId = VisitId,
                            Message = Common.AppPrivileges.Split_Claim_Success_Message
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


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
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
        private string FillCaseInfo(string CaseNumber)
        {
            try
            {
                DSWCNF dsCase = null;
                BLObject<DSWCNF> obj = null;
                obj = BLLPatientObj.LoadVisitRecord(CaseNumber);
                if (obj.Data != null)
                {
                    dsCase = obj.Data;
                    var response = new
                    {
                        status = true,
                        CASELoad_JSON = MDVUtility.JSON_DataTable(dsCase.Tables[dsCase.WCNFDetail.TableName]),
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        #endregion




        private string LoadPatientVisitInsurances(Int64 visitId, Int64 patientId)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                //string insuranceFill = null;
                obj = BLLPatientObj.LoadPatientVisitInsurance(visitId, patientId);
                if (obj.Data != null)
                {
                    dsPatient = obj.Data;
                    //if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                    //{
                    //    long insuranceId = 0;
                    //    //if (PatientInsuranceId > 0)
                    //    //{
                    //    //    insuranceId = PatientInsuranceId;//Utility.ToLong(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Select([dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]==PatientInsuranceId));
                    //    //}
                    //    //else
                    //    //{
                    //    insuranceId = MDVUtility.ToLong(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]);
                    //    //}

                    //    //insuranceFill = FillPatientInsurances(insuranceId);
                    //}
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count,
                        PatientInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                        //  PatientInsuranceFill_JSON = insuranceFill
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = 0,
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
        private string UpdatePatientVisitInsurance(string PatientId, string VisitId)
        {
            try
            {

                if (string.IsNullOrEmpty(PatientId))
                {
                    PatientId = "0";
                }
                if (string.IsNullOrEmpty(VisitId))
                {
                    VisitId = "0";
                }
                string result = BLLPatientObj.UpdatePatientVisitInsurance(MDVUtility.ToLong(PatientId), MDVUtility.ToLong(VisitId));
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Update_Message
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


        private string CheckPatientVisitInsurance(string PatientId, string VisitId)
        {
            try
            {

                if (string.IsNullOrEmpty(PatientId))
                {
                    PatientId = "0";
                }
                if (string.IsNullOrEmpty(VisitId))
                {
                    VisitId = "0";
                }
                int result = BLLPatientObj.CheckPatientVisitInsurance(MDVUtility.ToLong(PatientId), MDVUtility.ToLong(VisitId));
                var response = new
                {
                    check = result,
                    result = true ,
                    Messages=""
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    check = 0,
                    result = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }



        private string LoadAppointmentCopayAlert(string VisitId)
        {
            try
            {
                List<AppointmentCopayAlert> AlertCopayList=null;
                BLObject<List<AppointmentCopayAlert>>objAppointmentAlertCopay;
                objAppointmentAlertCopay = BLLScheduleObj.LoadAppointmentAlert(VisitId);
                AlertCopayList = objAppointmentAlertCopay.Data;
                var response = new
                {
                    status = true,
                    AlertCopayListCount = AlertCopayList.Count,
                    AlertCopayListInfo_JSON = AlertCopayList,
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
        #region Service Command Handler
        /// <summary>
        /// Handle the Employer Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "SAVE_CHARGE_CAPTURE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["ChargeData"];
                            Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                            string ChargeId = MDVUtility.ToStr(context.Request["ChargeId"]);
                            strJSONData = SaveCharge(fieldsJSON, VisitId, ChargeId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "VOID_AND_RECREATE_CLAIM":
                    {
                        string VisitId = MDVUtility.ToStr(context.Request["VisitId"]);
                        string strJSONData = VoidAndReCreateClaim(VisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SPLIT_CLAIM":
                    {
                        long VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                        string strJSONData = SplitClaim(VisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_VISITS":
                    {
                        //string fieldsJSON = context.Request["VisitData"];
                        //Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        //Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        //string strJSONData = SearchVisits(fieldsJSON, PatientID, VisitID);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_VISIT":
                    {
                        //Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        //Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        //string strJSONData = FillVisit(PatientID, VisitID);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_VISIT_INSURANCE":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = LoadPatientVisitInsurances(VisitID, PatientId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
               
                case "UPDATE_CHARGE_CAPTURE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["ChargeData"];
                            Int64 ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                            string RefCtrl = null;

                            if (context.Request["RefCtrl"] != null && context.Request["RefCtrl"].ToString().ToLower() == "charge detail")
                            {
                                RefCtrl = context.Request["RefCtrl"].ToString();
                                strJSONData = UpdateCharge(fieldsJSON, ChargeId, RefCtrl);
                            }
                            else
                            {
                                strJSONData = UpdateCharge(fieldsJSON, ChargeId);
                            }
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CHARGE_CAPTURE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 ChargeId = 0;
                            if (context.Request["ChargeId"] != null)
                            {
                                ChargeId = MDVUtility.ToInt64(context.Request["ChargeId"]);
                            }
                            strJSONData = DeleteCharge(ChargeId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }

                case "UPDATE_VISIT_ACTIVE_INACTIVE":
                    {
                        //string fieldsJSON = context.Request["VisitData"];
                        //Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        //Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        //Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        //string strJSONData = UpdateVisitIsActive(PatientID, VisitID, IsActive);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_VISITS":
                    {
                        //string fieldsJSON = context.Request["VisitData"];
                        //String VisitID = context.Request["VisitID"];
                        //string strJSONData = DeleteVisit(VisitID);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CHARGE_ORDER":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long fromId = MDVUtility.ToLong(context.Request["FromElementId"]);
                            long toId = MDVUtility.ToLong(context.Request["ToElementId"]);
                            strJSONData = SortPatientCharges(fromId, toId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_CPT_FEE":
                    {
                        string CPTCode = MDVUtility.ToStr(context.Request["CPTCode"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 ProviderId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        Int64 FacilityId = MDVUtility.ToInt64(context.Request["FacilityId"]);
                        Int64 PracticeId = MDVUtility.ToInt64(context.Request["PracticeId"]);
                        Int64 patientInsuraceId = MDVUtility.ToInt64(context.Request["patientInsuraceId"]);
                        string POSCode = MDVUtility.ToStr(context.Request["POSCode"]);
                        string Modifier1 = MDVUtility.ToStr(context.Request["Modifier1"]);
                        string Modifier2 = MDVUtility.ToStr(context.Request["Modifier2"]);
                        string Modifier3 = MDVUtility.ToStr(context.Request["Modifier3"]);
                        string Modifier4 = MDVUtility.ToStr(context.Request["Modifier4"]);
                        DateTime ChargeDOS = MDVUtility.ToDateTime(context.Request["ChargeDOS"]);
                        string strJSONData = FillCPTFee(VisitId, CPTCode, ProviderId, FacilityId, PracticeId, ChargeDOS, patientInsuraceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_ALL_CPT_FEE":
                    {
                        string CPTData = MDVUtility.ToStr(context.Request["CPTData"]);
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        string strJSONData = FillAllCPTFee(CPTData, VisitId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "RESUBMIT_CHARGE":
                    {
                        string VisitID = MDVUtility.ToStr(context.Request["VisitID"]);
                        string ChargeId = MDVUtility.ToStr(context.Request["ChargeId"]);
                        string strJSONData = ResubmitCharge(VisitID, ChargeId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "BILL_TO_PATIENT_CHARGES":
                    {
                        string ChargesIds = context.Request["ChargesIds"];
                        bool BillToPatient = Convert.ToBoolean(context.Request["BillToPatient"]);
                        string strJSONData = BillToPatientCharges(ChargesIds, BillToPatient);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_CLAIM_ERRORS":
                    {
                        string VisitId = context.Request["VisitId"];
                        string strJSONData = LoadClaimErrors(VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CASE_DETAIL":
                    {
                        string CaseNumber = context.Request["CaseNumber"];
                        string strJSONData = FillCaseInfo(CaseNumber);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_VISIT_INSURANCE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "Edit")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string PatientId = context.Request["PatientId"];
                            string VisitId = context.Request["VisitId"];
                            strJSONData = UpdatePatientVisitInsurance(PatientId, VisitId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                     case "CHECK_PATIENT_VISIT_INSURANCE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charges", "Edit")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string PatientId = context.Request["PatientId"];
                            string VisitId = context.Request["VisitId"];
                            strJSONData = CheckPatientVisitInsurance(PatientId, VisitId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "VISIT_APPOINTMENT_COPAY_ALERT":
                    {
                        string VisitId = context.Request["VisitId"];
                        string strJSONData = LoadAppointmentCopayAlert(VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

    }

        #endregion
}