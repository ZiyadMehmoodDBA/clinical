using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Admin;
using System.Threading.Tasks;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Controls.CommonControls;
using MDVision.Model.Billing.ERA;

namespace MDVision.IEHR.Controls.Patient.Encounter
{
    public class Encounter_Visits
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Encounter_Visits()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLBillingObj = new BLLBilling();
            BLLVisitsObj = new BLLVisits();
        }


        #region Singleton

        public int chargesRowCount = 0;

        private static Encounter_Visits _obj = null;
        public static Encounter_Visits Instance()
        {
            if (_obj == null)
                _obj = new Encounter_Visits();
            return _obj;
        }
        #endregion

        #region Private Functions

        //private void UpdateDataRow(DSCharge.PatientChargesRow NewRow, ref DSCharge.PatientChargesRow ExistingRow)
        //{
        //    DSCharge Myds = new DSCharge();
        //    for (int i = 2; i < Myds.PatientCharges.Columns.Count; i++)
        //    {
        //        if (NewRow[Myds.PatientCharges.Columns[i].ColumnName] != null)
        //            ExistingRow[Myds.PatientCharges.Columns[i].ColumnName] = NewRow[Myds.PatientCharges.Columns[i].ColumnName];
        //    }
        //    //return ExistingRow;
        //}

        /// <summary>
        /// Saves the Visit.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveVisit(string fieldsJSON, Int64 PatientId, Int64 VisitId, string SelectedPatientInsurance, string IsSelfPay)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);


                DSCharge dsCharge = new DSCharge();
                BLObject<DSCharge> objCharge = null;
                objCharge = BLLBillingObj.LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, VisitId);
                dsCharge = objCharge.Data;
                // Build Charges Dataset to be merged in Visit DataSet

                string ChargeRowIds = MDVUtility.ToStr(SearchedfieldsJSON["hfChargeRowId"]);
                string[] arrChargeRowId = ChargeRowIds.Split(',');

                foreach (string ChargeRowId in arrChargeRowId)
                {
                    if (!string.IsNullOrEmpty(ChargeRowId))
                    {


                        DSCharge.PatientChargesRow[] dRows = (DSCharge.PatientChargesRow[])dsCharge.PatientCharges.Select(dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(ChargeRowId));
                        if (dRows.Length > 0)
                        {
                            //// Here we'll update this row with new values..
                            // Row containing updated data of charges
                            dRows[0] = Controls.Patient.Encounter.Encounter_ChargeCapture.Instance().BuildChargeRow(fieldsJSON, dsCharge, VisitId, ChargeRowId, dRows[0]);
                        }
                        else
                        {
                            // Row containing updated data of charges
                            DSCharge.PatientChargesRow drCharges = Controls.Patient.Encounter.Encounter_ChargeCapture.Instance().BuildChargeRow(fieldsJSON, dsCharge, VisitId, ChargeRowId);
                            dsCharge.PatientCharges.AddPatientChargesRow(drCharges);
                        }
                    }
                }

                DSVisits dsVisit = null;
                BLObject<DSVisits> objVisits = null;
                objVisits = BLLVisitsObj.LoadPatientsVisits(VisitId, PatientId, 0, 0, null, null, "", "", "");
                dsVisit = objVisits.Data;

                DSVisits dsVisitICD = null;
                BLObject<DSVisits> objVisitICDs = null;
                objVisitICDs = BLLVisitsObj.LoadPatientVisitICDs(VisitId, 0);
                dsVisitICD = objVisitICDs.Data;
                int PVICDIdsLength = MDVUtility.ToInt(SearchedfieldsJSON["hfPVICDIdsLength"]);
                // dsVisit.Merge(dsVisitICD);
                DSVisits.PatientVisitICDRow[] dICD9Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.VisitIdColumn.ColumnName + " = " + VisitId + " and " + dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=9");
                if (dICD9Rows.Length > 0)
                {
                    if (dICD9Rows.Length > PVICDIdsLength)
                    {
                        for (int r = 0; r < dICD9Rows.Length; r++)
                        {
                            if (r < dICD9Rows.Length)
                            {
                                dICD9Rows[r] = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 9, r + 1, dICD9Rows[r]);
                            }
                        }
                    }
                    else
                    {
                        for (int r = 0; r < PVICDIdsLength; r++)
                        {
                            if (r < dICD9Rows.Length)
                            {
                                dICD9Rows[r] = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 9, r + 1, dICD9Rows[r]);
                            }
                            else
                            {
                                DSVisits.PatientVisitICDRow drVisitICDs = BuildVisitICDRow(fieldsJSON, dsVisitICD, 0, 9, r + 1);
                                dsVisitICD.PatientVisitICD.AddPatientVisitICDRow(drVisitICDs);
                            }
                        }
                    }
                }
                else
                {
                    for (int r = 0; r < PVICDIdsLength; r++)
                    {
                        DSVisits.PatientVisitICDRow drVisitICDs = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 9, r + 1);
                        dsVisitICD.PatientVisitICD.AddPatientVisitICDRow(drVisitICDs);
                    }

                }

                DSVisits.PatientVisitICDRow[] dICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.VisitIdColumn.ColumnName + " = " + VisitId + " and " + dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=10");
                if (dICD10Rows.Length > 0)
                {
                    if (dICD10Rows.Length > PVICDIdsLength)
                    {
                        for (int r = 0; r < dICD10Rows.Length; r++)
                        {
                            if (r < dICD10Rows.Length)
                            {

                                dICD10Rows[r] = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 10, r + 1, dICD10Rows[r]);
                            }
                        }
                    }
                    else
                    {
                        for (int r = 0; r < PVICDIdsLength; r++)
                        {
                            if (r < dICD10Rows.Length)
                            {
                                dICD10Rows[r] = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 10, r + 1, dICD10Rows[r]);
                            }
                            else
                            {
                                DSVisits.PatientVisitICDRow drVisitICDs = BuildVisitICDRow(fieldsJSON, dsVisitICD, VisitId, 10, r + 1);
                                dsVisitICD.PatientVisitICD.AddPatientVisitICDRow(drVisitICDs);
                            }
                        }
                    }
                }
                else
                {
                    for (int r = 0; r < PVICDIdsLength; r++)
                    {
                        DSVisits.PatientVisitICDRow drVisitICDs = BuildVisitICDRow(fieldsJSON, dsVisitICD, 0, 10, r + 1);
                        dsVisitICD.PatientVisitICD.AddPatientVisitICDRow(drVisitICDs);
                    }
                }
                //Merge Charges related to this Visit

                if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count < 1)
                {
                    DSVisits.PatientVisitsRow drvisit = dsVisit.PatientVisits.NewPatientVisitsRow();
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                        drvisit.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDate"]))
                        drvisit.AppointmentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpEncounterSignOffDate"]))
                        drvisit.EncounterSignOffDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEncounterSignOffDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtReferralNumber"]))
                        drvisit.ReferralNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralNumber"]);
                    if (SearchedfieldsJSON.ContainsKey("hfReferralNumerId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfReferralNumerId"]))
                    {
                        drvisit.PatientReferralId = MDVUtility.ToInt64(SearchedfieldsJSON["hfReferralNumerId"]);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.PatientReferralIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseId"]))
                        drvisit.CaseMgmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtBatchNumber"]))
                    {
                        drvisit.BatchNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtBatchNumber"]);
                        drvisit.BatchId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBatchId"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                    {
                        drvisit.ClaimNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtClaimNumber"]);
                    }
                    else
                    {
                        drvisit.ClaimNumber = null;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                    {
                        drvisit.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.ProviderIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                    {
                        drvisit.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.FacilityIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                        drvisit.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);

                    if (!string.IsNullOrEmpty(SelectedPatientInsurance))
                    {
                        if (MDVUtility.ToInt64(SelectedPatientInsurance) > 0)
                            drvisit.PatientInsuranceId = MDVUtility.ToInt64(SelectedPatientInsurance);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.PatientInsuranceIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]))
                        drvisit.ClaimTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimType"]);

                    if (SearchedfieldsJSON.ContainsKey("ddlSubmissionMode") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlSubmissionMode"]))
                        drvisit.IsElectronicSubmit = MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmissionMode"]) == "1" ? true : false;
                    else
                        drvisit[dsVisit.PatientVisits.IsElectronicSubmitColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBillingProvider"]))
                        drvisit.BillingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBillingProvider"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitCopayment"]))
                        drvisit.VisitCopayment = MDVUtility.ToDouble(SearchedfieldsJSON["txtVisitCopayment"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]))
                        drvisit.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]))
                        drvisit.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkFrom"]))
                        drvisit.UAWorkFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkFrom"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkTo"]))
                        drvisit.UAWorkto = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkTo"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpInjuryDate"]))
                        drvisit.InjuryDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpInjuryDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDelayReason"]))
                        drvisit.DelayReason = MDVUtility.ToInt(SearchedfieldsJSON["ddlDelayReason"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLMPDate"]))
                        drvisit.LMPDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLMPDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfOrderingProvider"]))
                        drvisit.OrderingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfOrderingProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCurrentIllnessDate"]))
                        drvisit.IllnessDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCurrentIllnessDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFrequency"]))
                        drvisit.ClaimFrequency = MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimFrequency"]);

                    drvisit.PrintHCFA = MDVUtility.ToStr(SearchedfieldsJSON["chkPrintonHCFAF19"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSupervisingProvider"]))
                        drvisit.SupervisingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfSupervisingProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceProvider"]))
                        drvisit.ResourceProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceProvider"]);
                    drvisit.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;

                    if (SearchedfieldsJSON.ContainsKey("ddlBox24BShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24BShaded"]))
                    {
                        drvisit.Box24BShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24BShaded"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlBox24IJShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24IJShaded"]))
                    {
                        drvisit.Box24IJShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24IJShaded"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAdmissionDate"]))
                        drvisit.AdmissionDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAdmissionDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDischargeDate"]))
                        drvisit.DischargeDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDischargeDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtICN_DCN"]))
                        drvisit.ICDDCN = MDVUtility.ToStr(SearchedfieldsJSON["txtICN_DCN"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                        drvisit.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    //prd-273
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hftxtClaimComments"])) {
                        drvisit.ClaimComments = MDVUtility.ToStr(SearchedfieldsJSON["hftxtClaimComments"]);

                    }
                        

                    if (SearchedfieldsJSON.ContainsKey("txtNoteComments"))
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNoteComments"]))
                        {
                            drvisit.NoteComments = MDVUtility.ToStr(SearchedfieldsJSON["txtNoteComments"]);
                            drvisit.NoteModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            drvisit.NoteModifiedOn = DateTime.Now;
                        }
                    }
                    drvisit.IsActive = 1;
                    drvisit.VisitStatus = "CheckIn";
                    drvisit.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drvisit.CreatedOn = DateTime.Now;
                    drvisit.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drvisit.ModifiedOn = DateTime.Now;

                    drvisit.IsEmployed = MDVUtility.ToStr(SearchedfieldsJSON["RadEmploymentYes"]) == "True" ? true : false;

                    drvisit.AutoAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadAutoYes"]) == "True" ? true : false;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtState"]))
                        drvisit.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);

                    drvisit.OtherAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadOtherYes"]) == "True" ? true : false;
                    if (SearchedfieldsJSON.ContainsKey("ddlSubmitStatus"))
                    {
                        drvisit.SubmitStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlSubmitStatus"]);
                        if (MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit electronic" || MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit paper")
                        {
                            drvisit.ReadyToSubmitOn = DateTime.Now;
                        }
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReportTypeCode"]))
                        drvisit.ReportTypeCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlReportTypeCode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlTransmissionCode"]))
                        drvisit.TransmissionCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlTransmissionCode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtControlNumber"]))
                        drvisit.ControlNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtControlNumber"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDocumentSentDate"]))
                        drvisit.DocumentSentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDocumentSentDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLastSeenDate"]))
                        drvisit.LastSeenDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLastSeenDate"]);
                    //Begin Edited by Azeem Raza Tayyab to fix bug#:  PMS-4115
                    if (SearchedfieldsJSON.ContainsKey("radPCP") && SearchedfieldsJSON["radPCP"] == true)
                    {
                        drvisit.IsSpecialist = false;
                    }
                    else if (SearchedfieldsJSON.ContainsKey("radSpecialist") && SearchedfieldsJSON["radSpecialist"] == true)
                    {
                        drvisit.IsSpecialist = true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkBillToPatient"))
                    {
                        drvisit.BillToPatient = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToPatient"]) == "True" ? true : false; ;
                    }

                    if (SearchedfieldsJSON.ContainsKey("dtpHoldTill") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpHoldTill"]))
                        drvisit.HoldTill = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHoldTill"]);
                    if (SearchedfieldsJSON.ContainsKey("hfAuthorizeId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfAuthorizeId"]))
                        drvisit.AuthorizeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAuthorizeId"]);
                    if (SearchedfieldsJSON.ContainsKey("txtPriorAuthNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPriorAuthNumber"]))
                        drvisit.PAN = MDVUtility.ToStr(SearchedfieldsJSON["txtPriorAuthNumber"]);
                    if (SearchedfieldsJSON.ContainsKey("chkIsReportNPI"))
                    {
                        drvisit.IsReportNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkIsReportNPI"]) == "True" ? true : false; ;
                    }

                    //PMS-8 adnan maqbool
                    if (SearchedfieldsJSON.ContainsKey("chkIsCleanclaim"))
                    {
                        drvisit.IsCleanClaim = MDVUtility.ToStr(SearchedfieldsJSON["chkIsCleanclaim"]) == "True" ? true : false; ;
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkInCollections"))
                    {
                        drvisit.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["chkInCollections"]) == "True" ? true : false; ;
                    }

                    //

                    //******** Start Anesthesia Code @@Irfan@@
                    if (SearchedfieldsJSON["hfIsAnesthesiaBilling"] == "true")
                    {
                        if (SearchedfieldsJSON.ContainsKey("radAnesthesiologist") && SearchedfieldsJSON["radAnesthesiologist"] == true)
                        {
                            drvisit.IsAnes = true;
                        }
                        else
                        {
                            drvisit.IsAnes = false;
                        }

                        if (SearchedfieldsJSON.ContainsKey("hfAnesthesiologist"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAnesthesiologist"]))
                                drvisit.AnesthesiologistId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAnesthesiologist"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtAnesthesiologistStartTime"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiologistStartTime"]))
                                drvisit.AnesStartTime = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiologistStartTime"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtAnesthesiologistEndTime"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiologistEndTime"]))
                                drvisit.AnesEndTime = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiologistEndTime"]);
                        }


                        if (SearchedfieldsJSON.ContainsKey("hfCRNA"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCRNA"]))
                                drvisit.CRNAId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCRNA"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtCRNAStartTime"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCRNAStartTime"]))
                                drvisit.CRNAStartTime = MDVUtility.ToStr(SearchedfieldsJSON["txtCRNAStartTime"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtCRNAEndTime"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCRNAEndTime"]))
                                drvisit.CRNAEndTime = MDVUtility.ToStr(SearchedfieldsJSON["txtCRNAEndTime"]);
                        }

                        if (SearchedfieldsJSON.ContainsKey("ddlAnesthesiaType"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAnesthesiaType"]))
                                drvisit.AnesTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAnesthesiaType"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("ddlASA"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlASA"]))
                                drvisit.ASAId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlASA"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("ddlServiceType"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlServiceType"]))
                                drvisit.AnesServiceTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlServiceType"]);
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtAnesthesiaComments"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiaComments"]))
                                drvisit.AnesComments = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiaComments"]);
                            else
                                drvisit[dsVisit.PatientVisits.AnesCommentsColumn] = DBNull.Value;
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtRiskUnits"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRiskUnits"]))
                                drvisit.RiskUnits = MDVUtility.ToStr(SearchedfieldsJSON["txtRiskUnits"]);
                        }

                    }

                    //******** End Anesthesia Code @@Irfan@@
                    drvisit.selfpay = string.IsNullOrEmpty(IsSelfPay) ? "False" : IsSelfPay;
                    dsVisit.PatientVisits.AddPatientVisitsRow(drvisit);
                }
                else // Update Visits
                {
                    string s = SearchedfieldsJSON["dtpAppointmentDate"];
                    foreach (DSVisits.PatientVisitsRow drvisit in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            drvisit.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDate"]))
                            drvisit.AppointmentDate = MDVUtility.ToDateTime(s);
                        else
                            drvisit[dsVisit.PatientVisits.AppointmentDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpEncounterSignOffDate"]))
                            drvisit.EncounterSignOffDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEncounterSignOffDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.EncounterSignOffDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpSubmittedDate"]))
                        {
                            //Begin Edited by Azeem Raza Tayyab on 11-Feb-2016 to fix bug#:PMS-3908
                            drvisit.SubmittedDate = MDVUtility.ToStr(MDVUtility.ToDateTime(SearchedfieldsJSON["dtpSubmittedDate"]));
                            //End Edited by Azeem Raza Tayyab on 11-Feb-2016 to fix bug#:PMS-3908
                        }
                        else
                        {

                            drvisit.SubmittedDate = null;
                        }

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtSubmittedBy"]))
                        //drvisit.SubmittedBy = SearchedfieldsJSON["txtSubmittedBy"];

                        drvisit.ReferralNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralNumber"]);
                        if (SearchedfieldsJSON.ContainsKey("hfReferralNumerId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfReferralNumerId"]))
                        {
                            drvisit.PatientReferralId = MDVUtility.ToInt64(SearchedfieldsJSON["hfReferralNumerId"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.PatientReferralIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseId"]))
                            drvisit.CaseMgmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                        else
                            drvisit[dsVisit.PatientVisits.CaseMgmtIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtBatchNumber"]))
                        {
                            drvisit.BatchNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtBatchNumber"]);
                            drvisit.BatchId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBatchId"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.BatchNumberColumn] = DBNull.Value;
                            drvisit[dsVisit.PatientVisits.BatchIdColumn] = DBNull.Value;
                        }
                        //start//16-02-2017//Ahmad Raza//fixed bug #EMR-2986
                        // if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                        drvisit.ClaimNumber = null;
                        //else
                        //     drvisit[dsVisit.PatientVisits.ClaimNumberColumn] = DBNull.Value;
                        //end//16-02-2017//Ahmad Raza//fixed bug #EMR-2986

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        {
                            drvisit.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.ProviderIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        {
                            drvisit.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.FacilityIdColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                            drvisit.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                        else
                            drvisit[dsVisit.PatientVisits.RefProviderIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SelectedPatientInsurance))
                        {
                            if (MDVUtility.ToInt64(SelectedPatientInsurance) > 0)
                                drvisit.PatientInsuranceId = MDVUtility.ToInt64(SelectedPatientInsurance);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.PatientInsuranceIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]))
                            drvisit.ClaimTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimType"]);
                        else
                            drvisit[dsVisit.PatientVisits.ClaimTypeIdColumn] = DBNull.Value;

                        if (SearchedfieldsJSON.ContainsKey("ddlSubmissionMode") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlSubmissionMode"]))
                            drvisit.IsElectronicSubmit = MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmissionMode"]) == "1" ? true : false;
                        else
                            drvisit[dsVisit.PatientVisits.IsElectronicSubmitColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBillingProvider"]))
                            drvisit.BillingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBillingProvider"]);
                        else
                            drvisit[dsVisit.PatientVisits.BillingProviderIdColumn] = DBNull.Value;



                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitCopayment"]))
                            drvisit.VisitCopayment = MDVUtility.ToDouble(SearchedfieldsJSON["txtVisitCopayment"]);
                        else
                            drvisit[dsVisit.PatientVisits.VisitCopaymentColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]))
                            drvisit.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]))
                            drvisit.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkFrom"]))
                            drvisit.UAWorkFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkFrom"]);
                        else
                            drvisit[dsVisit.PatientVisits.UAWorkFromColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkTo"]))
                            drvisit.UAWorkto = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkTo"]);
                        else
                            drvisit[dsVisit.PatientVisits.UAWorktoColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpInjuryDate"]))
                            drvisit.InjuryDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpInjuryDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.InjuryDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDelayReason"]))
                            drvisit.DelayReason = MDVUtility.ToInt(SearchedfieldsJSON["ddlDelayReason"]);
                        else
                            drvisit[dsVisit.PatientVisits.DelayReasonColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLMPDate"]))
                            drvisit.LMPDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLMPDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.LMPDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfOrderingProvider"]))
                            drvisit.OrderingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfOrderingProvider"]);
                        else
                            drvisit[dsVisit.PatientVisits.OrderingProviderColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCurrentIllnessDate"]))
                            drvisit.IllnessDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCurrentIllnessDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.IllnessDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFrequency"]))
                            drvisit.ClaimFrequency = MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimFrequency"]);
                        else
                            drvisit[dsVisit.PatientVisits.ClaimFrequencyColumn] = DBNull.Value;

                        drvisit.PrintHCFA = MDVUtility.ToStr(SearchedfieldsJSON["chkPrintonHCFAF19"]) == "True" ? true : false;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSupervisingProvider"]))
                            drvisit.SupervisingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfSupervisingProvider"]);
                        else
                            drvisit[dsVisit.PatientVisits.SupervisingProviderColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceProvider"]))
                            drvisit.ResourceProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceProvider"]);
                        else
                            drvisit[dsVisit.PatientVisits.ResourceProviderIdColumn] = DBNull.Value;
                        drvisit.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;

                        if (SearchedfieldsJSON.ContainsKey("ddlBox24BShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24BShaded"]))
                            drvisit.Box24BShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24BShaded"]);

                        if (SearchedfieldsJSON.ContainsKey("ddlBox24IJShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24IJShaded"]))
                            drvisit.Box24IJShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24IJShaded"]);


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAdmissionDate"]))
                            drvisit.AdmissionDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAdmissionDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.AdmissionDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDischargeDate"]))
                            drvisit.DischargeDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDischargeDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.DischargeDateColumn] = DBNull.Value;
                        drvisit.ICDDCN = MDVUtility.ToStr(SearchedfieldsJSON["txtICN_DCN"]);
                        drvisit.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                        if (SearchedfieldsJSON.ContainsKey("hfIsCommnetsChanged") && MDVUtility.ToStr(SearchedfieldsJSON["hfIsCommnetsChanged"]) == "true"){
                            drvisit.ClaimComments = MDVUtility.ToStr(SearchedfieldsJSON["hftxtClaimComments"]);
                            drvisit.ClaimCommentChanged = true;
                        }
                        else
                        {
                            drvisit.ClaimCommentChanged = false;
                        }
                       
                        if (SearchedfieldsJSON.ContainsKey("txtNoteComments"))
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNoteComments"]))
                            {
                                drvisit.NoteComments = MDVUtility.ToStr(SearchedfieldsJSON["txtNoteComments"]);
                                drvisit.NoteModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                drvisit.NoteModifiedOn = DateTime.Now;
                            }
                        }
                        drvisit.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drvisit.ModifiedOn = DateTime.Now;


                        drvisit.IsEmployed = MDVUtility.ToStr(SearchedfieldsJSON["RadEmploymentYes"]) == "True" ? true : false;

                        drvisit.AutoAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadAutoYes"]) == "True" ? true : false;

                        drvisit.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);


                        drvisit.OtherAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadOtherYes"]) == "True" ? true : false;
                        //drvisit.VisitStatus = "Seen";//drvisit.VisitStatus;
                        if (SearchedfieldsJSON.ContainsKey("ddlSubmitStatus"))
                        {
                            drvisit.SubmitStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlSubmitStatus"]);
                            if (MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit electronic" || MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit paper")
                            {
                                drvisit.ReadyToSubmitOn = DateTime.Now;
                            }
                        }


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReportTypeCode"]))
                            drvisit.ReportTypeCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlReportTypeCode"]);
                        else
                            drvisit[dsVisit.PatientVisits.ReportTypeCodeIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlTransmissionCode"]))
                            drvisit.TransmissionCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlTransmissionCode"]);
                        else
                            drvisit[dsVisit.PatientVisits.TransmissionCodeIdColumn] = DBNull.Value;
                        drvisit.ControlNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtControlNumber"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDocumentSentDate"]))
                            drvisit.DocumentSentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDocumentSentDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.DocumentSentDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLastSeenDate"]))
                            drvisit.LastSeenDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLastSeenDate"]);
                        else
                            drvisit[dsVisit.PatientVisits.LastSeenDateColumn] = DBNull.Value;
                        //Begin Edited by Azeem Raza Tayyab to fix bug#:  PMS-4115
                        if (SearchedfieldsJSON.ContainsKey("radPCP") && SearchedfieldsJSON["radPCP"] == true)
                        {
                            drvisit.IsSpecialist = false;
                        }
                        else if (SearchedfieldsJSON.ContainsKey("radSpecialist") && SearchedfieldsJSON["radSpecialist"] == true)
                        {
                            drvisit.IsSpecialist = true;
                        }
                        if (SearchedfieldsJSON.ContainsKey("chkBillToPatient"))
                        {
                            drvisit.BillToPatient = MDVUtility.ToStr(SearchedfieldsJSON["chkBillToPatient"]) == "True" ? true : false; ;
                        }
                        if (SearchedfieldsJSON.ContainsKey("dtpHoldTill") && !string.IsNullOrEmpty(SearchedfieldsJSON["dtpHoldTill"]))
                            drvisit.HoldTill = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpHoldTill"]);
                        else
                        {
                            drvisit[dsVisit.PatientVisits.HoldTillColumn] = DBNull.Value;
                        }
                        if (SearchedfieldsJSON.ContainsKey("hfAuthorizeId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfAuthorizeId"]))
                        {
                            drvisit.AuthorizeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAuthorizeId"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.AuthorizeIdColumn] = DBNull.Value;
                        }
                        if (SearchedfieldsJSON.ContainsKey("txtPriorAuthNumber") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtPriorAuthNumber"]))
                        {
                            drvisit.PAN = MDVUtility.ToStr(SearchedfieldsJSON["txtPriorAuthNumber"]);
                        }
                        else
                        {
                            drvisit[dsVisit.PatientVisits.PANColumn] = string.Empty;
                        }
                        //End Edited by Azeem Raza Tayyab to fix bug#:  PMS-4115
                        if (SearchedfieldsJSON.ContainsKey("chkIsReportNPI"))
                        {
                            drvisit.IsReportNPI = MDVUtility.ToStr(SearchedfieldsJSON["chkIsReportNPI"]) == "True" ? true : false; ;
                        }
                        //PMS-8 adnan maqbool
                        if (SearchedfieldsJSON.ContainsKey("chkIsCleanclaim"))
                        {
                            drvisit.IsCleanClaim = MDVUtility.ToStr(SearchedfieldsJSON["chkIsCleanclaim"]) == "True" ? true : false;
                        }
                        //
                        if (SearchedfieldsJSON.ContainsKey("chkInCollections"))
                        {
                            drvisit.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["chkInCollections"]) == "True" ? true : false; ;
                        }

                        //******** Start Anesthesia Code @@Irfan@@
                        if (SearchedfieldsJSON["hfIsAnesthesiaBilling"] == "true")
                        {
                            if (SearchedfieldsJSON.ContainsKey("radAnesthesiologist") && SearchedfieldsJSON["radAnesthesiologist"] == true)
                            {
                                drvisit.IsAnes = true;
                            }
                            else
                            {
                                drvisit.IsAnes = false;
                            }
                            if (SearchedfieldsJSON.ContainsKey("hfAnesthesiologist"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfAnesthesiologist"]))
                                    drvisit.AnesthesiologistId = MDVUtility.ToInt64(SearchedfieldsJSON["hfAnesthesiologist"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtAnesthesiologistStartTime"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiologistStartTime"]))
                                    drvisit.AnesStartTime = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiologistStartTime"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtAnesthesiologistEndTime"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiologistEndTime"]))
                                    drvisit.AnesEndTime = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiologistEndTime"]);
                            }


                            if (SearchedfieldsJSON.ContainsKey("hfCRNA"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCRNA"]))
                                    drvisit.CRNAId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCRNA"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtCRNAStartTime"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCRNAStartTime"]))
                                    drvisit.CRNAStartTime = MDVUtility.ToStr(SearchedfieldsJSON["txtCRNAStartTime"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtCRNAEndTime"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCRNAEndTime"]))
                                    drvisit.CRNAEndTime = MDVUtility.ToStr(SearchedfieldsJSON["txtCRNAEndTime"]);
                            }

                            if (SearchedfieldsJSON.ContainsKey("ddlAnesthesiaType"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAnesthesiaType"]))
                                    drvisit.AnesTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAnesthesiaType"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlASA"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlASA"]))
                                    drvisit.ASAId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlASA"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("ddlServiceType"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlServiceType"]))
                                    drvisit.AnesServiceTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlServiceType"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtAnesthesiaComments"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAnesthesiaComments"]))
                                    drvisit.AnesComments = MDVUtility.ToStr(SearchedfieldsJSON["txtAnesthesiaComments"]);
                            }
                            if (SearchedfieldsJSON.ContainsKey("txtRiskUnits"))
                            {
                                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRiskUnits"]))
                                    drvisit.RiskUnits = MDVUtility.ToStr(SearchedfieldsJSON["txtRiskUnits"]);
                            }
                        }
                        //******** End Anesthesia Code @@Irfan@@
                        
                    }
                }

                #region Database Insertion

                BLObject<DSVisits> obj = BLLVisitsObj.InsertUpdatePatientVisits(dsVisit, dsCharge, dsVisitICD);
                if (obj.Data != null)
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>
                    {
                    };
                    long visitId = Convert.ToInt64(dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0][dsVisit.PatientVisits.VisitIdColumn.ColumnName]);
                    DSVisits.PatientVisitICDRow[] PatientVisitICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisit.PatientVisitICD.Select(dsVisit.PatientVisitICD.VisitIdColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(visitId) + " and " + dsVisit.PatientVisitICD.ICDTypeColumn.ColumnName + "=10");
                    //  objVisitICDs = BLLVisitsObj.LoadPatientVisitICDs(visitId, 0);
                    //  DataRow[] PatientVisitICD10Rows = obj.Data.PatientVisitICD.Select(obj.Data.PatientVisitICD.ICDTypeColumn.ColumnName + "=10");
                    if (PatientVisitICD10Rows.Length > 0)
                    {
                        for (int r = 0; r < PatientVisitICD10Rows.Length; r++)
                        {
                            keyValues.Add("hfPVICDId" + (r + 1), MDVUtility.ToStr(PatientVisitICD10Rows[r][objVisitICDs.Data.PatientVisitICD.PVICDIdColumn.ColumnName]));

                        }
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {

                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        visitId = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0][dsVisit.PatientVisits.VisitIdColumn.ColumnName],
                        VisitFill_JSON = js.Serialize(keyValues),
                        ClaimNumber = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0][dsVisit.PatientVisits.ClaimNumberColumn.ColumnName],
                        NoteComments = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0][dsVisit.PatientVisits.NoteCommentsColumn.ColumnName]
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

        private string SaveMultipleNotesComments(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                string NoteComments = null, NoteModifiedBy = null, ModifiedBy = string.Empty, VisitIds = string.Empty;
                DateTime? NoteModifiedOn = null;
                DateTime ModifiedOn = DateTime.Now;

                if (SearchedfieldsJSON.ContainsKey("VisitIds"))
                {
                    VisitIds = MDVUtility.ToStr(SearchedfieldsJSON["VisitIds"]);
                }

                if (SearchedfieldsJSON.ContainsKey("NoteComments"))
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["NoteComments"]))
                    {
                        NoteComments = MDVUtility.ToStr(SearchedfieldsJSON["NoteComments"]);
                        NoteModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        NoteModifiedOn = DateTime.Now;
                    }
                }
                ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                BLObject<string> obj = BLLVisitsObj.SaveMultipleNoteComments(VisitIds, ModifiedBy, ModifiedOn, NoteComments, NoteModifiedBy, NoteModifiedOn);

                if (string.IsNullOrEmpty(obj.Data))
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message
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
        /// Load all the LoadPatientVisits for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The VisitID identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchVisits(string fieldsJSON, Int64 PatientID, Int64 VisitID, Int32 PageNumber, Int32 RowsPerPage, string VisitStatus)
        {
            try
            {
                string ClaimNumber;
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                DSVisits dsVisitClose = null;
                BLObject<DSVisits> obj1 = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON == null)
                    obj = BLLVisitsObj.searchPatientsVisits(VisitID, PatientID, 0, 0, null, null, "", "", "");
                else
                {
                    Int64 ProviderId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtProvider"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfProvider"]) : 0;
                    Int64 FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtFacility"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfFacility"]) : 0;
                    DateTime? FromAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateFrom"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateFrom"]) : null;

                    DateTime? ToAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateTo"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateTo"]) : null;
                    ClaimNumber = SearchedfieldsJSON["txtClaimNumber"];
                    string IsActive = SearchedfieldsJSON["ddlActive"];
                    if (VisitStatus == "")
                    {
                        obj = BLLVisitsObj.searchPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, "0", IsActive, PageNumber, RowsPerPage);
                        obj1 = BLLVisitsObj.searchPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, "1", IsActive, PageNumber, RowsPerPage);
                    }
                    else
                    {
                        if (VisitStatus == "0")
                            obj = BLLVisitsObj.searchPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, VisitStatus, IsActive, PageNumber, RowsPerPage);
                        else if (VisitStatus == "1")
                            obj1 = BLLVisitsObj.searchPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, VisitStatus, IsActive, PageNumber, RowsPerPage);
                    }

                }
                if (obj != null)
                {
                    dsVisit = obj.Data;
                }
                if (obj1 != null)
                {
                    dsVisitClose = obj1.Data;
                }

                if (dsVisitClose != null && dsVisit != null)
                {
                    if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count > 0 || dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count > 0)
                    {
                        if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count > dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count)
                        {
                            if (dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }

                        else if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count < dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count)
                        {
                            if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                    OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                    CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                    VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                    CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }

                        else
                        {
                            var response = new
                            {
                                status = true,
                                VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                                iTotalDisplayRecords = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.RecordCountColumn.ColumnName],
                                OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                                CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                                VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                                CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }


                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else if (dsVisitClose == null)
                {
                    if (dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.RecordCountColumn.ColumnName],
                            OpenVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.OpenVisitsColumn.ColumnName],
                            CloseVisitsCount = dsVisit.PatientVisitsSearch.Rows[0][dsVisit.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisitsSearch.TableName]),
                            CloseVisitsLoad_JSON = "",

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    if (dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.RecordCountColumn.ColumnName],
                            //OpenVisitsCount = dsVisit.PatientVisits.Rows[0][dsVisit.PatientVisits.OpenVisitsColumn.ColumnName],
                            CloseVisitsCount = dsVisitClose.PatientVisitsSearch.Rows[0][dsVisitClose.PatientVisitsSearch.CloseVisitsColumn.ColumnName],
                            VisitsLoad_JSON = "",
                            CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisitsSearch.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            Message = "Record not found."
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
        /// Load all the Visit Plans for List binding.
        /// </summary>
        /// <param name="ProviderID">The VisitID identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchVisitPlans(Int64 VisitID)
        {
            try
            {
                DSVisitLookup dsVisit = null;
                BLObject<DSVisitLookup> obj = null;
                obj = BLLVisitsObj.LoadVisitsPlans(VisitID);
                dsVisit = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        VisitPlansCount = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0 ? dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count : 0,
                        VisitPlansLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisits.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        VisitPlansCount = 0,
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
        /// Load all the Visits of Case.
        /// </summary>
        /// <param name="ProviderID">The CaseID identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchCaseVisits(Int64 PatientID, Int64 CaseID)
        {
            try
            {
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientsVisits(0, PatientID, 0, 0, null, null, "", "", "", 0, 1, 1000, CaseID);
                dsVisit = obj.Data;

                if (obj.Data != null)
                {
                    if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count,
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisits.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitPlansCount = 0,
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

        /// <summary>
        /// Fills the Visit.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillVisit(long PatientID, long VisitId)
        {
            string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitId)))
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
                    DSVisits dsVisit = null;
                    DSCharge dsCharge = new DSCharge();
                    BLObject<DSVisits> obj = null;
                    BLObject<DSVisits> obj1 = null;
                    BLObject<DSVisits> obj2 = null;
                    BLObject<DSVisits> obj3 = null;
                    BLObject<DSVisits> objPayments = null;
                    BLObject<List<STC>> objEDI = null;
                    BLObject<string> claimStatusObj = null;
                    List<STC> stcList = null;
                    string claimStatus = "-1";
                    obj = BLLVisitsObj.LoadPatientsVisits(VisitId, PatientID, 0, 0, null, null, "", "", "");
                    if (obj.Data != null)
                    {

                        dsVisit = obj.Data;
                        if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                        {


                            obj1 = BLLVisitsObj.LoadClaimVoidInfo(VisitId);
                            if (obj1.Data != null)
                                dsVisit.Merge(obj1.Data);

                            obj2 = BLLVisitsObj.LoadPatientVistsDetails(VisitId);
                            if (obj2.Data != null)
                                dsVisit.Merge(obj2.Data);

                            obj3 = BLLVisitsObj.LoadPatientVisitICDs(VisitId, 0);
                            if (obj3.Data != null)
                                dsVisit.Merge(obj3.Data);
                            objPayments = BLLVisitsObj.LoadClaimPayments(VisitId);
                            if (objPayments.Data != null)
                                dsVisit.Merge(objPayments.Data);

                            DataRow dr = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0];

                            var ClaimNumber = MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimNumberColumn.ColumnName]);
                            var SubmitStatus = MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmitStatusColumn.ColumnName]);
                            var editString = "";
                            //if (SubmitStatus == "Ready to Submit Electronic")
                            //{
                                objEDI = BLLVisitsObj.getEDIDetail(ClaimNumber);
                                if (objEDI.Data != null)
                                {
                                    stcList = new List<STC>();
                                    stcList=objEDI.Data;
                                    claimStatusObj = BLLVisitsObj.getClaimStatus(ClaimNumber);
                                    if (objEDI.Data != null)
                                    {
                                        claimStatus = claimStatusObj.Data;
                                    }
                                    else
                                    {
                                        var response1 = new
                                        {
                                            status = false,
                                            Message = claimStatusObj.Message
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                                    }
                            }
                                else
                                {
                                    var response1 = new
                                    {
                                        status = false,
                                        Message = objEDI.Message
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                                }
                            //}

                            string ddlSubmissionMode = "";
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsElectronicSubmitColumn.ColumnName])))
                                ddlSubmissionMode = MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsElectronicSubmitColumn.ColumnName]) == "False" ? "0" : "1";

                            string IsVNC_Visit = "";
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsVNCColumn.ColumnName])))
                                IsVNC_Visit = String.Format(Format, dr[dsVisit.PatientVisits.IsVNCColumn.ColumnName]) == "True" ? "True" : "False";

                            // string Age= calculateAge(MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOBColumn.ColumnName]));
                            string modifiedNoteDate = dr[dsVisit.PatientVisits.NoteModifiedOnColumn.ColumnName] != System.DBNull.Value ? MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteModifiedOnColumn.ColumnName]) : MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.NoteModifiedOnColumn.ColumnName]).ToString();
                            Dictionary<string, string> keyValues = new Dictionary<string, string>

                        {
                            { "txtPatientName", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AccountNumberColumn.ColumnName])},
                            { "txtSex", MDVUtility.ToStr(dr[dsVisit.PatientVisits.GenderColumn.ColumnName])},
                            { "hfSelfPay", MDVUtility.ToStr(dr[dsVisit.PatientVisits.selfpayColumn.ColumnName])},
                            { "hfNoteId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteIdColumn.ColumnName])},
                            { "txtPatientFullName", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientNameColumn.ColumnName])},
                            { "hfDOB", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOBColumn.ColumnName])},
                            { "txtDOB", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOBColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOBColumn.ColumnName]).ToShortDateString()},
                            { "NoteInsurancePlanName", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteInsurancePlanNameColumn.ColumnName]))?"": MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteInsurancePlanNameColumn.ColumnName])},
                            { "dtpAppointmentDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AppointmentDateColumn.ColumnName])},
                            { "dtpEncounterSignOffDate",MDVUtility.ToStr(dr[dsVisit.PatientVisits.EncounterSignOffDateColumn.ColumnName])},
                            { "ddlPatientInsurance", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientInsuranceIdColumn.ColumnName])},
                            { "ddlClaimType", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimTypeIdColumn.ColumnName])},
                            { "ddlSubmissionMode", ddlSubmissionMode },
                            { "ddlBillingProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BillingProviderIdColumn.ColumnName])},
                            { "hfInsurancePlan", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientInsuranceIdColumn.ColumnName])},
                            { "hfCaseId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseMgmtIdColumn.ColumnName]) !="" ? MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseMgmtIdColumn.ColumnName]):""},
                            { "txtCaseNumber",  MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseNumberColumn.ColumnName]) !="" ? MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseNumberColumn.ColumnName]):""},
                            { "dtpSubmittedDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmittedDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.SubmittedDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtBatchNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BatchNumberColumn.ColumnName])},
                            { "hfBatchId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BatchIdColumn.ColumnName])},
                            { "txtClaimNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimNumberColumn.ColumnName])},
                            { "hfPatientId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientIdColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsVisit.PatientVisits.FacilityIdColumn.ColumnName])},
                            { "hfPractice", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PracticeIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsVisit.PatientVisits.FacilityNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ProviderIdColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ProviderNameColumn.ColumnName])},
                            { "hfRefProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.RefProviderIdColumn.ColumnName])},
                            { "txtRefProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ReferrringProviderNameColumn.ColumnName])},
                            { "txtSubmittedBy", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmittedByFullNameColumn.ColumnName])},
                            { "txtVisitCopayment", MDVUtility.ToStr(dr[dsVisit.PatientVisits.VisitCopaymentColumn.ColumnName])},
                            { "txtReferralNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ReferralNumberColumn.ColumnName])},
                            { "dtpDOSFrom", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOSFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOSFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpUnableToWorkFrom", MDVUtility.ToStr(dr[dsVisit.PatientVisits.UAWorkFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.UAWorkFromColumn.ColumnName]).ToShortDateString():""},
                            { "dtpInjuryDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.InjuryDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.InjuryDateColumn.ColumnName]).ToShortDateString():""},
                            { "ddlDelayReason", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DelayReasonColumn.ColumnName])},
                            { "dtpLMPDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.LMPDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.LMPDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtOrderingProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.OrderingProviderNameColumn.ColumnName])},
                            { "hfOrderingProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.OrderingProviderColumn.ColumnName])},
                            { "dtpDOSTo", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOSToColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOSToColumn.ColumnName]).ToShortDateString():""},
                            { "dtpUnableToWorkTo", MDVUtility.ToStr(dr[dsVisit.PatientVisits.UAWorktoColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.UAWorktoColumn.ColumnName]).ToShortDateString():""},
                            { "dtpCurrentIllnessDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IllnessDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.IllnessDateColumn.ColumnName]).ToShortDateString():""},
                            { "ddlClaimFrequency", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimFrequencyColumn.ColumnName])},
                            { "chkPrintonHCFAF19", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PrintHCFAColumn.ColumnName])},
                            { "txtSupervisingProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SupervisingProviderNameColumn.ColumnName])},
                            { "hfSupervisingProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SupervisingProviderColumn.ColumnName])},
                            { "txtResourceProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ResourceProviderNameColumn.ColumnName])},
                            { "hfResourceProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ResourceProviderIdColumn.ColumnName])},
                            { "chkAssgBenefits", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AssignBenefitsColumn.ColumnName])},
                            { "ddlBox24IJShaded", MDVUtility.ToStr(dr[dsVisit.PatientVisits.Box24IJShadedColumn.ColumnName])},
                            { "ddlBox24BShaded", MDVUtility.ToStr(dr[dsVisit.PatientVisits.Box24BShadedColumn.ColumnName])},
                            { "dtpAdmissionDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AdmissionDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.AdmissionDateColumn.ColumnName]).ToShortDateString():""},
                            { "dtpDischargeDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DischargeDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DischargeDateColumn.ColumnName]).ToShortDateString():""},
                            { "txtICN_DCN", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ICDDCNColumn.ColumnName])},
                            { "AppointmentId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AppointmentIdColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CommentsColumn.ColumnName])},
                            { "RadEmploymentYes", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsEmployedColumn.ColumnName])},
                            { "RadAutoYes", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AutoAccidentColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsVisit.PatientVisits.StateColumn.ColumnName])},
                            { "RadOtherYes", MDVUtility.ToStr(dr[dsVisit.PatientVisits.OtherAccidentColumn.ColumnName])},
                            { "chkPaid", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsCopayPaidColumn.ColumnName])},
                            { "ddlSubmitStatus", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmitStatusIdColumn.ColumnName])},
                            { "txtSubmitStatus", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmitStatusColumn.ColumnName])},
                            { "hfVisitDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CreatedOnColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.CreatedOnColumn.ColumnName]).ToShortDateString():""},
                            { "ddlReportTypeCode", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ReportTypeCodeIdColumn.ColumnName])},
                            { "ddlTransmissionCode", MDVUtility.ToStr(dr[dsVisit.PatientVisits.TransmissionCodeIdColumn.ColumnName])},
                            { "txtControlNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ControlNumberColumn.ColumnName])},
                            { "dtpDocumentSentDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DocumentSentDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DocumentSentDateColumn.ColumnName]).ToShortDateString():""},
                            { "dtpLastSeenDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.LastSeenDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.LastSeenDateColumn.ColumnName]).ToShortDateString():""},
                            { "hfMasterVisitId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.MasterVisitIdColumn.ColumnName])},
                            { "radSpecialist", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsSpecialistColumn.ColumnName])=="True"?"True":"False"},
                            { "radPCP", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsSpecialistColumn.ColumnName])!="True"?"True":"False"},
                            { "txtNoteComments", ""},
                            { "txtNoteCommentsLoad", "Last Updated By: " + MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteModifiedByNameColumn.ColumnName]) + " " + 
                            //MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteModifiedOnColumn.ColumnName]) 
                             modifiedNoteDate + " Notes: " + MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteCommentsColumn.ColumnName])},
                            { "txtClaimComments", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimCommentsColumn.ColumnName])},
                            { "chkBillToPatient", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BillToPatientColumn.ColumnName])=="True"?"True":"False"},
                            { "chkBillToPatientForIns", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BillToPatientColumn.ColumnName])=="True"?"True":"False"},
                            //adnan maqbool ,PMS-1
                            { "chkIsCleanclaim", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsCleanClaimColumn.ColumnName])=="True"?"True":"False"},
                            { "chkInCollections", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsCollectionColumn.ColumnName])=="True"?"True":"False"},
                            { "chkIsDocAttach", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsDocAttachColumn.ColumnName])=="True"?"True":"False"},
                            { "chkIsERAttach", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsERAttachColumn.ColumnName])=="True"?"True":"False"},
                            { "LockedOn", MDVUtility.ToStr(dr[dsVisit.PatientVisits.LockedOnColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.LockedOnColumn.ColumnName]).ToShortDateString():""},
                            { "IsLocked", String.Format(Format,dr[dsVisit.PatientVisits.IsLockedColumn.ColumnName])=="True"?"True":"False"},
                            { "IsVNC", IsVNC_Visit},
                            { "VNCVisitId", String.Format(Format,dr[dsVisit.PatientVisits.VNCVisitIdColumn.ColumnName])},
                            { "ClaimStatusId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimStatusIdColumn.ColumnName])},
                            { "ClaimStatus", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimStatusColumn.ColumnName])},
                            { "hfReferralNumerId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientReferralIdColumn.ColumnName])},
                            { "dtpClaimDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CreatedOnColumn.ColumnName])},
                            { "dtpHoldTill" , MDVUtility.ToStr(dr[dsVisit.PatientVisits.HoldTillColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.HoldTillColumn.ColumnName]).ToShortDateString():""},
                            { "hfAuthorizeId" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.AuthorizeIdColumn.ColumnName])},
                            { "txtPriorAuthNumber" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.PANColumn.ColumnName])},
                            { "chkIsReportNPI" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.IsReportNPIColumn.ColumnName])},

                            //****** Start Anesthesia Code @@Irfan@@
                            { "hfAnesthesiologist", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AnesthesiologistIdColumn.ColumnName])},
                            { "hfCRNA", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CRNAIdColumn.ColumnName])},
                            { "txtAnesthesiologistStartTime", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AnesStartTimeColumn.ColumnName])},
                            { "txtAnesthesiologistEndTime", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AnesEndTimeColumn.ColumnName])},
                            { "txtCRNAStartTime", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CRNAStartTimeColumn.ColumnName])},
                            { "txtCRNAEndTime", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CRNAEndTimeColumn.ColumnName])},
                            { "ddlAnesthesiaType" , MDVUtility.ToStr(dr[dsVisit.PatientVisits.AnesTypeIdColumn.ColumnName])},
                            { "ddlASA" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.ASAIdColumn.ColumnName])},
                            { "ddlServiceType" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.AnesServiceTypeIdColumn.ColumnName])},
                            { "txtAnesthesiaComments" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.AnesCommentsColumn.ColumnName])},
                            { "txtAnesthesiologist" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.AnesthesiologistNameColumn.ColumnName])},
                            { "txtCRNA" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.CRNANameColumn.ColumnName])},
                            { "txtRiskUnits" , MDVUtility.ToStr( dr[dsVisit.PatientVisits.RiskUnitsColumn.ColumnName])},
                            { "radAnesthesiologist", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsAnesColumn.ColumnName])=="True"?"True":"False"},
                            { "radCRNA", MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsAnesColumn.ColumnName])!="True"?"True":"False"},
                            { "hfProviderCLIA", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PoviderCLIAColumn.ColumnName])},
                            //****** End Anesthesia Code @@Irfan@@
                        };

                            //PMS-1178
                            if (MDVUtility.ToStr(dr[dsVisit.PatientVisits.NoteCommentsColumn.ColumnName]) == "")
                            {
                                keyValues["txtNoteCommentsLoad"] = "";
                            }

                            if (dsVisit.Tables[dsVisit.ClaimVoidInfo.TableName].Rows.Count > 0)
                            {
                                DataRow drClaim = dsVisit.Tables[dsVisit.ClaimVoidInfo.TableName].Rows[0];
                                keyValues.Add("VoidedClaimNumber", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.VoidedClaimNumberColumn.ColumnName]));
                                keyValues.Add("NewClaimNumber", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.NewClaimNumberColumn.ColumnName]));
                                keyValues.Add("OriginalClaimNumber", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.OriginalClaimNumberColumn.ColumnName]));
                                keyValues.Add("VoidedVisitId", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.VoidedVisitIdColumn.ColumnName]));
                                keyValues.Add("NewVisitId", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.NewVisitIdColumn.ColumnName]));
                                keyValues.Add("OriginalVisitId", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.OriginalVisitIdColumn.ColumnName]));

                                keyValues.Add("IsSplitted", MDVUtility.ToStr(drClaim[dsVisit.ClaimVoidInfo.IsSplittedColumn.ColumnName]) == "True" ? "True" : "False");
                                keyValues.Add("SplittedFromClaimNumber", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.SplittedFromClaimNumberColumn.ColumnName]));
                                keyValues.Add("SplittedFromVisitId", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.SplittedFromVisitIdColumn.ColumnName]));
                                keyValues.Add("SplittedToClaimNumber", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.SplittedToClaimNumberColumn.ColumnName]));
                                keyValues.Add("SplittedToVisitId", String.Format(Format, drClaim[dsVisit.ClaimVoidInfo.SplittedToVisitIdColumn.ColumnName]));
                            }

                            if (dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows.Count > 0)
                            {
                                DataRow drClaim = dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows[0];
                                keyValues.Add("txtInsCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsChargesColumn.ColumnName]));
                                keyValues.Add("txtPatCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatChargesColumn.ColumnName]));
                                keyValues.Add("txtTotalCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalChargesColumn.ColumnName]));
                                keyValues.Add("txtInsBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsBalancesColumn.ColumnName]));
                                keyValues.Add("txtPatBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatBalancesColumn.ColumnName]));
                                keyValues.Add("txtTotalBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalBalancesColumn.ColumnName]));
                                keyValues.Add("txtInsPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsPaymentsColumn.ColumnName]));
                                keyValues.Add("txtPatPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatPaymentsColumn.ColumnName]));
                                keyValues.Add("txtTotalPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalPaymentsColumn.ColumnName]));
                                keyValues.Add("txtInsAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsAdjustColumn.ColumnName]));
                                keyValues.Add("txtPatAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatAdjustColumn.ColumnName]));
                                keyValues.Add("txtTotalAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalAdjustColumn.ColumnName]));
                            }

                            List<Dictionary<string, string>> lstChargeVisits = new List<Dictionary<string, string>>();
                            List<Dictionary<string, string>> lstPaymentsData = new List<Dictionary<string, string>>();
                            foreach (DataRow drChargeVisit in dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows)
                            {
                                string CPTDescription = null;
                                if (MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName]) == "")
                                {
                                    string cptCode = MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName]);
                                    //new Task(() => {
                                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string strCPT = IMO.GetAllIMOCPTCodes(null, cptCode, 1, 100, false);
                                    var CPTSerializer = ser.Deserialize<dynamic>(strCPT);
                                    // if record found then do this 
                                    if (CPTSerializer.ContainsKey("CPTLoad_JSON"))
                                    {
                                        var cptJSNData = CPTSerializer["CPTLoad_JSON"];
                                        var CPT = ser.Deserialize<dynamic>(cptJSNData);
                                        CPTDescription = CPT[0]["Description"];
                                        Admin_CPTCode_Detail objCptCode = new Admin_CPTCode_Detail();
                                        objCptCode.SaveCPTCodeFromFillVisit(cptCode, CPTDescription);
                                    }
                                    
                                   
                                              
                                    // }).Start();
                                }

                                string IsVNC = "";
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName])))
                                    IsVNC = String.Format(Format, drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName]) == "True" ? "True" : "False";
                                var ChargeVisitValues = new Dictionary<string, string>
                            {

                                {"hfChargeId", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName])},
                                {"dtpDOSFrom" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]).ToShortDateString():""},
                                {"dtpDOSTo" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]).ToShortDateString():""},
                                { "txtCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "hfCPTDescription" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName])},
                                { "hfCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "hfCurrentChargeCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "txtUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.UnitsColumn.ColumnName])},
                                { "hfUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.UnitsColumn.ColumnName])},
                                { "txtModifier1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier1Column.ColumnName])},
                                { "txtModifier2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier2Column.ColumnName])},
                                { "txtModifier3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier3Column.ColumnName])},
                                { "txtModifier4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier4Column.ColumnName])},

                                  { "txtICDPointer1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer1Column.ColumnName])},
                                { "txtICDPointer2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer2Column.ColumnName])},
                                { "txtICDPointer3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer3Column.ColumnName])},
                                { "txtICDPointer4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer4Column.ColumnName])},

                                { "txtPOS" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},
                                { "chkEMG" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EMGColumn.ColumnName])},
                                { "txtFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.FeeColumn.ColumnName])},
                                { "txtTotalFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,MDVUtility.ToDouble(drChargeVisit[dsCharge.PatientCharges.BilledColumn.ColumnName]))},
                                { "txtINSCharges" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.InsChargesColumn.ColumnName])},
                                { "txtPATCharges" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.PatChargesColumn.ColumnName])},
                                //SearchedfieldsJSON["ddlConditionCode1"]
                                { "txtCOPAY" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.CopayColumn.ColumnName])},
                                { "txtPriorAuthorization" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.PANColumn.ColumnName])},
                                { "txtExpectedFee" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                                 { "hfExpectedFee" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                                { "txtNDC" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCColumn.ColumnName])},
                                { "txtNDCDescription" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCDescriptionColumn.ColumnName])},
                                { "txtNDCUnit" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitColumn.ColumnName])},
                                { "txtNDCUnitPrice" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitPriceColumn.ColumnName])},
                                { "txtCLIA" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CLIAColumn.ColumnName])},
                                { "txtdrugCodeCost" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DrugCodeCostColumn.ColumnName])},
                                { "chkReportCPTDesc" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsReportCPTDescColumn.ColumnName]) == "True"?"True":"False"},

                                { "ddlNDCMeasurement" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCMeasurCodeIdColumn.ColumnName])},
                                { "txtComments" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.LineNotesColumn.ColumnName])},
                                { "dtpEOD" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EODColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.EODColumn.ColumnName]).ToShortDateString():""},
                                { "txtStatus" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.StatusColumn.ColumnName])},
                                { "txtMasterChargeId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.MasterChargeIdColumn.ColumnName])},
                                //{ "chkHold" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsHoldColumn.ColumnName])},
                                //{ "txtHoldDays" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToInt(drChargeVisit[dsCharge.PatientCharges.HoldDaysColumn.ColumnName]) != 0 ? MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.HoldDaysColumn.ColumnName]):""},
                                //{ "dtpHoldFrom" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.HoldFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.HoldFromColumn.ColumnName]).ToShortDateString():""},
                                { "chkPrimary" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsPrimaryColumn.ColumnName])},
                                { "txtChargeOrder" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeOrderColumn.ColumnName])},
                                { "hfTotalBalance" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalBalColumn.ColumnName])},
                                { "hfAssignBenefits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.AssignBenefitsColumn.ColumnName])},
                                { "txtPrimaryFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.PrimaryFeeColumn.ColumnName])},
                                { "chkActive" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.IsActiveColumn.ColumnName])=="True"?"True":"False"},

                                { "LockedOn" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.LockedOnColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.LockedOnColumn.ColumnName]).ToShortDateString():""},
                                { "IsLocked" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.IsLockedColumn.ColumnName])=="True"?"True":"False"},
                                { "IsVNC" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), IsVNC},
                                { "VNCVisitId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.VNCVisitIdColumn.ColumnName])},
                                { "VNCChargesId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.VNCChargesIdColumn.ColumnName])},
                                { "txtTOS" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TOSCodeColumn.ColumnName])},
                                { "txtICD1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode1Column.ColumnName])},
                                 { "txtICD2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode2Column.ColumnName])},
                                 { "txtICD3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode3Column.ColumnName])},
                                 { "txtICD4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode4Column.ColumnName])},
                                 { "txtStartTime" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.StartTimeColumn.ColumnName])},
                                 { "txtEndTime" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EndTimeColumn.ColumnName])},
                                 { "txtTimeUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TimeUnitsColumn.ColumnName])},
                                 { "txtBaseUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.BaseUnitsColumn.ColumnName])},
                                 { "txtRiskUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.RiskUnitsColumn.ColumnName])},
                                 { "txtTotalMinutes" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalMinutesColumn.ColumnName])},
                                 { "txtTotalUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalUnitsColumn.ColumnName])},
                                 { "txtRoundBiuldUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.RoundBilledUnitsIdColumn.ColumnName])},
                                //{ "hfPOS", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},


                            };

                                lstChargeVisits.Add(ChargeVisitValues);
                            }
                            int ClaimPaymentsCount = 0;
                            if (dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows.Count > 0)
                            {
                                ClaimPaymentsCount = dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows.Count;
                                foreach (DataRow drVisitPayments in dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows)
                                {
                                    var VisitPaymentValues = new Dictionary<string, string>
                                    {
                                        {"PaymentId", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaymentIdColumn.ColumnName])},
                                        {"PaymentDate", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaymentDateColumn.ColumnName])},
                                        {"ApplyTo", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.ApplyToColumn.ColumnName])},
                                        {"Allowed", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.AllowedColumn.ColumnName])},
                                        {"Deductables", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.DeductablesColumn.ColumnName])},
                                        {"Coinsurance", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CoinsuranceColumn.ColumnName])},
                                        {"Copay", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CopayColumn.ColumnName])},
                                        {"Paid", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaidColumn.ColumnName])},
                                        {"Adj", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.AdjColumn.ColumnName])},
                                        {"Code", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CodeColumn.ColumnName])},
                                        {"CheckNo", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CheckNoColumn.ColumnName])},
                                        {"CheckDate", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CheckDateColumn.ColumnName])},
                                        {"PatientResponsibility",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PatientResponsibilityColumn.ColumnName])},
                                        {"InsuranceName",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.InsuranceNameColumn.ColumnName])},
                                        {"RemittanceDes", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.RemittanceDesColumn.ColumnName])},
                                        {"Comments",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CommentsColumn.ColumnName])},
                                        {"PaidAmountCr",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaidAmountCrColumn.ColumnName])},

                                    };
                                    lstPaymentsData.Add(VisitPaymentValues);
                                }
                            }
                            //foreach (DataRow drVisitICD in dsVisit.Tables[dsVisit.PatientVisitICD.TableName].Rows)
                            //{
                            //string PVICD9Ids = "";
                            //string PVICD10Ids = "";
                            DSVisits.PatientVisitICDRow[] dICD9Rows = (DSVisits.PatientVisitICDRow[])dsVisit.PatientVisitICD.Select(dsVisit.PatientVisitICD.VisitIdColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(VisitId) + " and " + dsVisit.PatientVisitICD.ICDTypeColumn.ColumnName + "=9");
                            if (dICD9Rows.Length > 0)
                            {
                                for (int r = 0; r < dICD9Rows.Length; r++)
                                {
                                    //if (PVICD9Ids != "")
                                    //    PVICD9Ids = PVICD9Ids + ",";
                                    //PVICD9Ids = PVICD9Ids + MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.PVICDIdColumn.ColumnName]) + ",";
                                    keyValues.Add("hfICD" + (r + 1), MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.ICDCodeColumn.ColumnName]));
                                    keyValues.Add("hfICDDescription" + (r + 1), MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName]));
                                    keyValues.Add("hfSNOMED" + (r + 1), MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.SNOMEDIDColumn.ColumnName]));
                                    keyValues.Add("hfSNOMEDDescription" + (r + 1), MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.SNOMEDDescriptionColumn.ColumnName]));
                                }
                                //keyValues.Add("hfPVICD9Id", PVICD9Ids);
                            }
                            DSVisits.PatientVisitICDRow[] dICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisit.PatientVisitICD.Select(dsVisit.PatientVisitICD.VisitIdColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(VisitId) + " and " + dsVisit.PatientVisitICD.ICDTypeColumn.ColumnName + "=10");
                            if (dICD10Rows.Length > 0)
                            {
                                for (int r = 0; r < dICD10Rows.Length; r++)
                                {
                                    //if (PVICD10Ids != "")
                                    //    PVICD10Ids = PVICD10Ids + ",";
                                    //PVICD10Ids = PVICD10Ids + MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.PVICDIdColumn.ColumnName]);
                                    keyValues.Add("txtICD" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.ICDCodeColumn.ColumnName]));
                                    keyValues.Add("hfICD10" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.ICDCodeColumn.ColumnName]));
                                    keyValues.Add("hfPVICDId" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.PVICDIdColumn.ColumnName]));
                                    keyValues.Add("txtICD10Description" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName]));
                                    if (!keyValues.ContainsKey("hfSNOMED" + (r + 1)))
                                    {
                                        keyValues.Add("hfSNOMED" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.SNOMEDIDColumn.ColumnName]));
                                        keyValues.Add("hfSNOMEDDescription" + (r + 1), MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.SNOMEDDescriptionColumn.ColumnName]));
                                    }
                                }
                                //keyValues.Add("hfPVICD10Id", PVICD9Ids);
                            }
                            //}
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                VisitChargesCount = dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                                VisitsChargesLoad_JSON = js.Serialize(lstChargeVisits),//Utility.JSON_DataTable(dsVisit.Tables[dsCharge.PatientCharges.TableName]),
                                VisitFill_JSON = js.Serialize(keyValues),
                                ClaimPaymentsLoad_JSON = js.Serialize(lstPaymentsData),
                                ClaimPaymentsCount = ClaimPaymentsCount,
                                stcList= stcList,
                                claimStatus = claimStatus
                            };


                            chargesRowCount = dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Count;
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message
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


        private string LoadVisitCharges(long VisitId)
        {
            string Format = "{0:0.00}";
            DSCharge dsCharge = new DSCharge();
            BLObject<DSCharge> obj = BLLVisitsObj.LoadVisitCharges(VisitId);
            dsCharge = obj.Data;
            List<Dictionary<string, string>> lstChargeVisits = new List<Dictionary<string, string>>();

            foreach (DataRow drChargeVisit in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
            {
                //string CPTDescription = null;
                //if (MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName]) == "")
                //{
                //    string cptCode = MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName]);
                //    //new Task(() => {
                //    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    string strCPT = IMO.GetAllIMOCPTCodes(null, cptCode, 1, 100, false);
                //    var CPTSerializer = ser.Deserialize<dynamic>(strCPT);
                //    var cptJSNData = CPTSerializer["CPTLoad_JSON"];
                //    var CPT = ser.Deserialize<dynamic>(cptJSNData);
                //    CPTDescription = CPT[0]["Description"];
                //    Admin_CPTCode_Detail objCptCode = new Admin_CPTCode_Detail();
                //    objCptCode.SaveCPTCodeFromFillVisit(cptCode, CPTDescription);
                //    // }).Start();
                //}

                string IsVNC = "";
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName])))
                    IsVNC = String.Format(Format, drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName]) == "True" ? "True" : "False";
                var ChargeVisitValues = new Dictionary<string, string>
                            {

                                {"hfChargeId", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName])},
                                {"dtpDOSFrom" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]).ToShortDateString():""},
                                {"dtpDOSTo" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]).ToShortDateString():""},
                                { "txtCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "hfCPTDescription" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName])},
                                { "hfCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "hfCurrentChargeCPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "txtUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.UnitsColumn.ColumnName])},
                                { "hfUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.UnitsColumn.ColumnName])},
                                { "txtModifier1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier1Column.ColumnName])},
                                { "txtModifier2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier2Column.ColumnName])},
                                { "txtModifier3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier3Column.ColumnName])},
                                { "txtModifier4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier4Column.ColumnName])},

                                  { "txtICDPointer1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer1Column.ColumnName])},
                                { "txtICDPointer2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer2Column.ColumnName])},
                                { "txtICDPointer3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer3Column.ColumnName])},
                                { "txtICDPointer4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer4Column.ColumnName])},

                                { "txtPOS" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},
                                { "chkEMG" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EMGColumn.ColumnName])},
                                { "txtFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.FeeColumn.ColumnName])},
                                { "txtTotalFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,MDVUtility.ToDouble(drChargeVisit[dsCharge.PatientCharges.BilledColumn.ColumnName]))},
                                { "txtINSCharges" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.InsChargesColumn.ColumnName])},
                                { "txtPATCharges" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.PatChargesColumn.ColumnName])},
                                //SearchedfieldsJSON["ddlConditionCode1"]
                                { "txtCOPAY" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.CopayColumn.ColumnName])},
                                { "txtPriorAuthorization" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.PANColumn.ColumnName])},
                                { "txtExpectedFee" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                                 { "hfExpectedFee" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                                { "txtNDC" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCColumn.ColumnName])},
                                { "txtNDCUnit" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitColumn.ColumnName])},
                                { "txtNDCUnitPrice" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitPriceColumn.ColumnName])},
                                { "txtCLIA" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CLIAColumn.ColumnName])},
                                { "txtdrugCodeCost" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DrugCodeCostColumn.ColumnName])},
                                { "chkReportCPTDesc" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsReportCPTDescColumn.ColumnName]) == "True"?"True":"False"},

                                { "ddlNDCMeasurement" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCMeasurCodeIdColumn.ColumnName])},
                                { "txtComments" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.LineNotesColumn.ColumnName])},
                                { "dtpEOD" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EODColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.EODColumn.ColumnName]).ToShortDateString():""},
                                { "txtStatus" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.StatusColumn.ColumnName])},
                                { "txtMasterChargeId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.MasterChargeIdColumn.ColumnName])},
                                //{ "chkHold" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsHoldColumn.ColumnName])},
                                //{ "txtHoldDays" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToInt(drChargeVisit[dsCharge.PatientCharges.HoldDaysColumn.ColumnName]) != 0 ? MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.HoldDaysColumn.ColumnName]):""},
                                //{ "dtpHoldFrom" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.HoldFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.HoldFromColumn.ColumnName]).ToShortDateString():""},
                                { "chkPrimary" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsPrimaryColumn.ColumnName])},
                                { "txtChargeOrder" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeOrderColumn.ColumnName])},
                                { "hfTotalBalance" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalBalColumn.ColumnName])},
                                { "hfAssignBenefits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.AssignBenefitsColumn.ColumnName])},
                                { "txtPrimaryFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.PrimaryFeeColumn.ColumnName])},
                                { "chkActive" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.IsActiveColumn.ColumnName])=="True"?"True":"False"},

                                { "LockedOn" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.LockedOnColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.LockedOnColumn.ColumnName]).ToShortDateString():""},
                                { "IsLocked" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.IsLockedColumn.ColumnName])=="True"?"True":"False"},
                                { "IsVNC" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), IsVNC},
                                { "VNCVisitId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.VNCVisitIdColumn.ColumnName])},
                                { "VNCChargesId" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.VNCChargesIdColumn.ColumnName])},
                                { "txtTOS" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TOSCodeColumn.ColumnName])},
                                { "txtICD1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode1Column.ColumnName])},
                                 { "txtICD2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode2Column.ColumnName])},
                                 { "txtICD3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode3Column.ColumnName])},
                                 { "txtICD4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDCode4Column.ColumnName])},
                                 { "txtStartTime" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.StartTimeColumn.ColumnName])},
                                 { "txtEndTime" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EndTimeColumn.ColumnName])},
                                 { "txtTimeUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TimeUnitsColumn.ColumnName])},
                                 { "txtBaseUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.BaseUnitsColumn.ColumnName])},
                                 { "txtRiskUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.RiskUnitsColumn.ColumnName])},
                                 { "txtTotalMinutes" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalMinutesColumn.ColumnName])},
                                 { "txtTotalUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalUnitsColumn.ColumnName])},
                                 { "txtRoundBiuldUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.RoundBilledUnitsIdColumn.ColumnName])},
                                //{ "hfPOS", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},


                            };

                lstChargeVisits.Add(ChargeVisitValues);
            }

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var response = new
            {
                status = true,
                VisitChargesCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                VisitsChargesLoad_JSON = js.Serialize(lstChargeVisits),//Utility.JSON_DataTable(dsVisit.Tables[dsCharge.PatientCharges.TableName]),
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Updates the Visit.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseId">The Visit identifier.</param>
        /// <returns></returns>
        private string UpdateVisit(string fieldsJSON, Int64 PatientId, Int64 VisitId, string ChargeRowIds)
        {
            try
            {
                // Build Charges Dataset to be merged in Visit DataSet
                DSCharge dsCharge = new DSCharge();
                string[] arrChargeRowId = ChargeRowIds.Split(',');
                foreach (string ChargeRowId in arrChargeRowId)
                {
                    DSCharge.PatientChargesRow drCharges = Controls.Patient.Encounter.Encounter_ChargeCapture.Instance().BuildChargeRow(fieldsJSON, dsCharge, VisitId, ChargeRowId);
                    dsCharge.PatientCharges.AddPatientChargesRow(drCharges);
                }

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientsVisits(VisitId, PatientId, 0, 0, null, null, "", "", "");
                dsVisit = obj.Data;

                for (int i = 0; i < dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Count; i++)
                {
                    DataRow drVisitCharge = (DataRow)dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows[i];
                    dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Remove(drVisitCharge);
                }
                //dsVisit.Tables[dsCharge.PatientCharges.TableName].AcceptChanges();
                //foreach (DataRow dr in dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows)
                //{
                //    dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Remove(dr);
                //}

                // dsVisit.Tables[dsCharge.PatientCharges.TableName].row = dsCharge.Tables[dsCharge.PatientCharges.TableName];

                foreach (DataRow dr in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                {

                    dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Add(dr.ItemArray);
                }

                //Merge Charges related to this Visit
                //dsVisit.Merge(dsCharge);

                foreach (DSVisits.PatientVisitsRow drvisit in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                        drvisit.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDate"]))
                        drvisit.AppointmentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.AppointmentDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpEncounterSignOffDate"]))
                        drvisit.EncounterSignOffDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEncounterSignOffDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.EncounterSignOffDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpSubmittedDate"]))
                    {
                        drvisit.SubmittedDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpSubmittedDate"]);
                    }
                    else
                    {
                        drvisit.SubmittedDate = null;
                    }

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtSubmittedBy"]))
                    //drvisit.SubmittedBy = SearchedfieldsJSON["txtSubmittedBy"];
                    if (SearchedfieldsJSON.ContainsKey("hfReferralNumerId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfReferralNumerId"]))
                    {
                        drvisit.PatientReferralId = MDVUtility.ToInt64(SearchedfieldsJSON["hfReferralNumerId"]);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.PatientReferralIdColumn] = DBNull.Value;
                    }
                    drvisit.ReferralNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralNumber"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfCaseId"]))
                        drvisit.CaseMgmtId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                    else
                        drvisit[dsVisit.PatientVisits.CaseMgmtIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtBatchNumber"]))
                    {
                        drvisit.BatchNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtBatchNumber"]);
                        drvisit.BatchId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBatchId"]);
                    }
                    else
                    {
                        drvisit[dsVisit.PatientVisits.BatchNumberColumn] = DBNull.Value;
                        drvisit[dsVisit.PatientVisits.BatchIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtClaimNumber"]))
                        drvisit.ClaimNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtClaimNumber"]);
                    else
                        drvisit[dsVisit.PatientVisits.ClaimNumberColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        drvisit.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        drvisit.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                        drvisit.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                    else
                        drvisit[dsVisit.PatientVisits.RefProviderIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientInsurance"]))
                        drvisit.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPatientInsurance"]);
                    else
                        drvisit[dsVisit.PatientVisits.PatientInsuranceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimType"]))
                        drvisit.ClaimTypeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaimType"]);
                    else
                        drvisit[dsVisit.PatientVisits.ClaimTypeIdColumn] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("ddlSubmissionMode") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlSubmissionMode"]))
                        drvisit.IsElectronicSubmit = MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmissionMode"]) == "1" ? true : false;
                    else
                        drvisit[dsVisit.PatientVisits.IsElectronicSubmitColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBillingProvider"]))
                        drvisit.BillingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBillingProvider"]);
                    else
                        drvisit[dsVisit.PatientVisits.BillingProviderIdColumn] = DBNull.Value;



                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitCopayment"]))
                        drvisit.VisitCopayment = MDVUtility.ToDouble(SearchedfieldsJSON["txtVisitCopayment"]);
                    else
                        drvisit[dsVisit.PatientVisits.VisitCopaymentColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]))
                        drvisit.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]))
                        drvisit.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkFrom"]))
                        drvisit.UAWorkFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkFrom"]);
                    else
                        drvisit[dsVisit.PatientVisits.UAWorkFromColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpUnableToWorkTo"]))
                        drvisit.UAWorkto = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpUnableToWorkTo"]);
                    else
                        drvisit[dsVisit.PatientVisits.UAWorktoColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpInjuryDate"]))
                        drvisit.InjuryDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpInjuryDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.InjuryDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlDelayReason"]))
                        drvisit.DelayReason = MDVUtility.ToInt(SearchedfieldsJSON["ddlDelayReason"]);
                    else
                        drvisit[dsVisit.PatientVisits.DelayReasonColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLMPDate"]))
                        drvisit.LMPDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLMPDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.LMPDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfOrderingProvider"]))
                        drvisit.OrderingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfOrderingProvider"]);
                    else
                        drvisit[dsVisit.PatientVisits.OrderingProviderColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCurrentIllnessDate"]))
                        drvisit.IllnessDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCurrentIllnessDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.IllnessDateColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFrequency"]))
                        drvisit.ClaimFrequency = MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimFrequency"]);
                    else
                        drvisit[dsVisit.PatientVisits.ClaimFrequencyColumn] = DBNull.Value;


                    drvisit.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkPrintonHCFAF19"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSupervisingProvider"]))
                        drvisit.SupervisingProvider = MDVUtility.ToInt64(SearchedfieldsJSON["hfSupervisingProvider"]);
                    else
                        drvisit[dsVisit.PatientVisits.SupervisingProviderColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResourceProvider"]))
                        drvisit.ResourceProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResourceProvider"]);
                    else
                        drvisit[dsVisit.PatientVisits.ResourceProviderIdColumn] = DBNull.Value;
                    drvisit.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;

                    //   { "ddlBox24IJShaded", MDVUtility.ToStr(dr[dsVisit.PatientVisits.Box24IJShadedColumn.ColumnName])},
                    //       { "ddlBox24BShaded", MDVUtility.ToStr(dr[dsVisit.PatientVisits.Box24BShadedColumn.ColumnName])},


                    if (SearchedfieldsJSON.ContainsKey("ddlBox24BShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24BShaded"]))
                        drvisit.Box24BShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24BShaded"]);

                    if (SearchedfieldsJSON.ContainsKey("ddlBox24IJShaded") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlBox24IJShaded"]))
                        drvisit.Box24IJShaded = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBox24IJShaded"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaimFrequency"]))
                        drvisit.ClaimFrequency = MDVUtility.ToStr(SearchedfieldsJSON["ddlClaimFrequency"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpAdmissionDate"]))
                        drvisit.AdmissionDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAdmissionDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.AdmissionDateColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDischargeDate"]))
                        drvisit.DischargeDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDischargeDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.DischargeDateColumn] = DBNull.Value;

                    drvisit.ICDDCN = MDVUtility.ToStr(SearchedfieldsJSON["txtICN_DCN"]);
                    drvisit.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    drvisit.ClaimComments = MDVUtility.ToStr(SearchedfieldsJSON["hftxtClaimComments"]);

                    drvisit.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drvisit.ModifiedOn = DateTime.Now;


                    drvisit.IsEmployed = MDVUtility.ToStr(SearchedfieldsJSON["RadEmploymentYes"]) == "True" ? true : false;

                    drvisit.AutoAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadAutoYes"]) == "True" ? true : false;

                    drvisit.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);


                    drvisit.OtherAccident = MDVUtility.ToStr(SearchedfieldsJSON["RadOtherYes"]) == "True" ? true : false;
                    //drvisit.VisitStatus = "Seen";//drvisit.VisitStatus;
                    if (SearchedfieldsJSON.ContainsKey("ddlSubmitStatus"))
                    {
                        drvisit.SubmitStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlSubmitStatus"]);
                        if (MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit electronic" || MDVUtility.ToStr(SearchedfieldsJSON["ddlSubmitStatus_text"]).ToLower() == "ready to submit paper")
                        {
                            drvisit.ReadyToSubmitOn = DateTime.Now;
                        }
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReportTypeCode"]))
                        drvisit.ReportTypeCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlReportTypeCode"]);
                    else
                        drvisit[dsVisit.PatientVisits.ReportTypeCodeIdColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlTransmissionCode"]))
                        drvisit.TransmissionCodeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlTransmissionCode"]);
                    else
                        drvisit[dsVisit.PatientVisits.TransmissionCodeIdColumn] = DBNull.Value;
                    drvisit.ControlNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtControlNumber"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDocumentSentDate"]))
                        drvisit.DocumentSentDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDocumentSentDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.DocumentSentDateColumn] = DBNull.Value;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpLastSeenDate"]))
                        drvisit.LastSeenDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpLastSeenDate"]);
                    else
                        drvisit[dsVisit.PatientVisits.LastSeenDateColumn] = DBNull.Value;
                }

                #region Database Updation

                if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                {
                    BLObject<DSVisits> objVisit = BLLVisitsObj.UpdatePatientsVisit(dsVisit);
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

        /// <summary>
        /// Updates the Visit IsActive.
        /// </summary>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="CaseId">The Visit identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateVisitIsActive(Int64 PatientId, Int64 VisitId, Int64 IsActive)
        {
            try
            {
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientsVisits(VisitId, PatientId, 0, 0, null, null, "", "", "");
                dsVisit = obj.Data;
                if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0];
                    dr[dsVisit.PatientVisits.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSVisits> objVisit = BLLVisitsObj.UpdatePatientsVisit(dsVisit);
                    string successMsg;
                    if (objVisit.Data != null)
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

        private string Resubmit(string VisitId)
        {
            try
            {
                if (string.IsNullOrEmpty(VisitId))
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
                    BLObject<bool> obj = BLLBillingClaimObj.UpdateVisitChargeStatus(3, visitId);

                    if (obj.Data.Equals(true))
                    {
                        //RESUBMIT BATCH STATUS, check if all batch of that visit is resubmitted then update batch status to resubmitted.
                        BLLBillingClaimObj.Update837BatchStatusResubmit(0, MDVUtility.ToInt(visitId));
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

        /// <summary>
        /// Deletes the Patient Document.
        /// </summary>
        /// <param name="PatientDocId">The PatientDocument identifier.</param>
        /// <returns></returns>
        private string DeleteVisit(string VisitId)
        {
            try
            {
                if (string.IsNullOrEmpty(VisitId))
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
                    BLObject<string> obj = BLLVisitsObj.DeletePatientsVisit(VisitId);
                    if (obj.Data != null && obj.Data == "")
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
        private string SearchClaimDocuments(string VisitID)
        {
            try
            {
                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj;
                obj = BLLBillingObj.SearchClaimDocuments(VisitID);

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

        public DSVisits.PatientVisitICDRow BuildVisitICDRow(string fieldsJSON, DSVisits dsVisit, Int64 VisitId, Int32 ICDTypeId, int ICDIndex, DSVisits.PatientVisitICDRow dr = null)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            bool isNewRow = false;
            if (dr == null)
            {
                dr = dsVisit.PatientVisitICD.NewPatientVisitICDRow();
                isNewRow = true;

            }
            #region PatientVisitICD
            for (int index = 1; index <= 12; index++)
            {
                if (index == ICDIndex)
                {
                    if (SearchedfieldsJSON.ContainsKey("txtICD" + index) && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD" + index]))
                    {
                        if (ICDTypeId == 9)
                        {
                            dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD" + index]);
                            dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription" + index]);
                            dr.ICDType = ICDTypeId;
                            dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED" + index]);
                            dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription" + index]);
                        }
                        else
                        {
                            dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD10" + index]);
                            dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description" + index]);
                            dr.ICDType = ICDTypeId;
                            dr.VisitId = VisitId;
                            dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED" + index]);
                            dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription" + index]);
                        }
                    }
                    else
                    {
                        //Delete Patient Visits ICD in case of update
                        if (isNewRow == false)
                        {
                            BLObject<bool> obj;
                            obj = BLLVisitsObj.DeleteVisitICDByPVICDID(MDVUtility.ToLong(dr[dsVisit.PatientVisitICD.PVICDIdColumn]));
                        }
                        else
                        {
                            dr[dsVisit.PatientVisitICD.ICDCodeColumn] = DBNull.Value;
                            dr[dsVisit.PatientVisitICD.ICDCodeDescriptionColumn] = DBNull.Value;
                            dr[dsVisit.PatientVisitICD.ICDTypeColumn] = ICDTypeId;
                            dr[dsVisit.PatientVisitICD.VisitIdColumn] = VisitId;
                            dr[dsVisit.PatientVisitICD.SNOMEDIDColumn] = DBNull.Value;
                            dr[dsVisit.PatientVisitICD.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                    }
                }
            }
            #region Commented
            /* if (SearchedfieldsJSON.ContainsKey("txtICD2") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD2"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD2"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription2"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED2"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription2"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD102"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description2"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED2"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription2"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_2Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_2Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_2Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_2Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD3") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD3"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD3"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription3"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED3"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription3"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD103"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description3"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED3"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription3"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_3Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_3Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_3Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_3Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD4") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD4"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD4"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription4"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED4"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription4"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD104"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description4"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED4"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription4"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_4Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_4Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_4Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_4Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD5") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD5"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD5"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription5"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED5"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription5"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD105"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description5"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED5"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription5"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_5Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_5Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_5Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_5Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD6") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD6"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD6"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription6"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED6"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription6"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD106"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description6"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED6"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription6"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_6Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_6Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_6Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_6Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD7") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD7"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD7"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription7"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED7"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription7"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD107"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description7"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED7"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription7"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_7Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_7Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_7Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_7Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD8") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD8"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD8"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription8"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED8"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription8"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD108"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description8"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED8"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription8"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_8Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_8Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_8Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_8Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD9") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD9"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD9"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription9"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED9"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription9"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD109"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description9"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED9"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription9"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_9Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_9Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_9Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_9Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD10") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD10"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD10"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription10"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED10"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription10"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD1010"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description10"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED10"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription10"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_10Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_10Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_10Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_10Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD11") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD11"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD11"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription11"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED11"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription11"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD1011"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description11"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED11"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription11"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_11Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_11Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_11Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_11Column] = DBNull.Value;
            }
            if (SearchedfieldsJSON.ContainsKey("txtICD12") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtICD12"]))
            {
                if (ICDTypeId == 9)
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD12"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfICDDescription12"]);
                    dr.ICDType = ICDTypeId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED12"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription12"]);
                }
                else
                {
                    dr.ICDCode = MDVUtility.ToStr(SearchedfieldsJSON["hfICD1012"]);
                    dr.ICDCodeDescription = MDVUtility.ToStr(SearchedfieldsJSON["txtICD10Description12"]);
                    dr.ICDType = ICDTypeId;
                    dr.VisitId = VisitId;
                    dr.SNOMEDID = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMED12"]);
                    dr.SNOMEDDescription = MDVUtility.ToStr(SearchedfieldsJSON["hfSNOMEDDescription12"]);
                }
            }
            else
            {
                //dr[dsVisit.PatientVisitICD.ICDCode_12Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.ICDCodeDescription_12Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDID_12Column] = DBNull.Value;
                //dr[dsVisit.PatientVisitICD.SNOMEDDescription_12Column] = DBNull.Value;
            }*/
            #endregion
            #endregion
            dr.VisitId = MDVUtility.ToInt64(VisitId);
            if (SearchedfieldsJSON.ContainsKey("hfPatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                dr.PatientId = MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]);

            return dr;
        }

        private string DeleteVisitICD(Int64 VisitId, int ICDIndex)
        {
            try
            {
                if (VisitId == 0)
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
                    DSCharge dsCharge = null;
                    BLObject<DSCharge> obj = BLLVisitsObj.DeleteVisitICD(VisitId, ICDIndex);
                    if (obj.Data != null)
                    {
                        dsCharge = obj.Data;
                        List<Dictionary<string, string>> lstChargeVisits = new List<Dictionary<string, string>>();
                        foreach (DataRow drChargeVisit in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                        {
                            var ChargeVisitValues = new Dictionary<string, string>
                            {
                                {"hfChargeId", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName])},
                                  { "txtICDPointer1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer1Column.ColumnName])},
                                { "txtICDPointer2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer2Column.ColumnName])},
                                { "txtICDPointer3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer3Column.ColumnName])},
                                { "txtICDPointer4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer4Column.ColumnName])},
                            };

                            lstChargeVisits.Add(ChargeVisitValues);
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            VisitChargesCount = dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                            VisitsChargesLoad_JSON = js.Serialize(lstChargeVisits),
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
        private string LoadClaimPaymentsDetail(long VisitId)
        {
            string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitId)))
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
                    DSVisits dsVisit = null;
                    DSCharge dsCharge = new DSCharge();
                    BLObject<DSVisits> obj = null;
                    obj = BLLVisitsObj.LoadPatientVistsDetails(VisitId);
                    if (obj.Data != null)
                    {
                        dsVisit = obj.Data;
                        if (dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows.Count > 0)
                        {
                            DataRow drClaim = dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows[0];
                            Dictionary<string, string> keyValues = new Dictionary<string, string>
                            {
                                {"txtInsCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsChargesColumn.ColumnName])},
                                {"txtPatCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatChargesColumn.ColumnName])},
                                {"txtTotalCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalChargesColumn.ColumnName])},
                                {"txtInsBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsBalancesColumn.ColumnName])},
                                {"txtPatBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatBalancesColumn.ColumnName])},
                                {"txtTotalBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalBalancesColumn.ColumnName])},
                                {"txtInsPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsPaymentsColumn.ColumnName])},
                                {"txtPatPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatPaymentsColumn.ColumnName])},
                                {"txtTotalPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalPaymentsColumn.ColumnName])},
                                {"txtInsAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsAdjustColumn.ColumnName])},
                                {"txtPatAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatAdjustColumn.ColumnName])},
                                {"txtTotalAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalAdjustColumn.ColumnName])},
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ClaimPaymentsDetail_JSON = js.Serialize(keyValues),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message
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
        /// Claim Summary Preview.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string ClaimSummaryPreview(long PatientID, long VisitId)
        {
            string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VisitId)))
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
                    DSVisits dsVisit = null;
                    DSCharge dsCharge = new DSCharge();
                    BLObject<DSVisits> obj = null;
                    BLObject<DSVisits> obj1 = null;
                    BLObject<DSVisits> obj2 = null;
                    BLObject<DSVisits> obj3 = null;
                    BLObject<DSVisits> objPayments = null;
                    obj = BLLVisitsObj.LoadPatientsVisits(VisitId, PatientID, 0, 0, null, null, "", "", "");
                    if (obj.Data != null)
                    {
                        dsVisit = obj.Data;
                        if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                        {
                            obj1 = BLLVisitsObj.LoadClaimVoidInfo(VisitId);
                            if (obj1.Data != null)
                                dsVisit.Merge(obj1.Data);

                            obj2 = BLLVisitsObj.LoadPatientVistsDetails(VisitId);
                            if (obj2.Data != null)
                                dsVisit.Merge(obj2.Data);

                            obj3 = BLLVisitsObj.LoadPatientVisitICDs(VisitId, 0);
                            if (obj3.Data != null)
                                dsVisit.Merge(obj3.Data);
                            objPayments = BLLVisitsObj.LoadClaimPayments(VisitId);
                            if (objPayments.Data != null)
                                dsVisit.Merge(objPayments.Data);

                            DataRow dr = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0];
                            string ddlSubmissionMode = "";
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsElectronicSubmitColumn.ColumnName])))
                                ddlSubmissionMode = MDVUtility.ToStr(dr[dsVisit.PatientVisits.IsElectronicSubmitColumn.ColumnName]) == "False" ? "0" : "1";
                            #region Main Claim Info
                            Dictionary<string, string> keyValues = new Dictionary<string, string>
                        {
                            { "PatientFullName", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientNameColumn.ColumnName])},
                            { "AppointmentDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.AppointmentDateColumn.ColumnName])},
                            { "EncounterSignOffDate",MDVUtility.ToStr(dr[dsVisit.PatientVisits.EncounterSignOffDateColumn.ColumnName])},
                            { "PatientInsurance", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientInsuranceIdColumn.ColumnName])},
                            { "ClaimType", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimTypeIdColumn.ColumnName])},
                            { "SubmissionMode", ddlSubmissionMode },
                             { "CaseNumber",  MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseNumberColumn.ColumnName]) !="" ? MDVUtility.ToStr(dr[dsVisit.PatientVisits.CaseNumberColumn.ColumnName]):""},
                            { "SubmittedDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmittedDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.SubmittedDateColumn.ColumnName]).ToShortDateString():""},
                            { "BatchNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.BatchNumberColumn.ColumnName])},
                            { "ClaimNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimNumberColumn.ColumnName])},
                            { "Facility", MDVUtility.ToStr(dr[dsVisit.PatientVisits.FacilityNameColumn.ColumnName])},
                            { "Provider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ProviderNameColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ReferrringProviderNameColumn.ColumnName])},
                            { "SubmittedBy", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmittedByFullNameColumn.ColumnName])},
                            { "VisitCopayment", MDVUtility.ToStr(dr[dsVisit.PatientVisits.VisitCopaymentColumn.ColumnName])},
                            { "ReferralNumber", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ReferralNumberColumn.ColumnName])},
                            { "DOSFrom", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOSFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOSFromColumn.ColumnName]).ToShortDateString():""},
                            { "DOSTo", MDVUtility.ToStr(dr[dsVisit.PatientVisits.DOSToColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsVisit.PatientVisits.DOSToColumn.ColumnName]).ToShortDateString():""},
                            { "ResourceProvider", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ResourceProviderNameColumn.ColumnName])},
                            { "SubmitStatus", MDVUtility.ToStr(dr[dsVisit.PatientVisits.SubmitStatusColumn.ColumnName])},
                            { "ClaimStatus", MDVUtility.ToStr(dr[dsVisit.PatientVisits.ClaimStatusColumn.ColumnName])},
                            { "hfReferralNumerId", MDVUtility.ToStr(dr[dsVisit.PatientVisits.PatientReferralIdColumn.ColumnName])},
                            { "ClaimDate", MDVUtility.ToStr(dr[dsVisit.PatientVisits.CreatedOnColumn.ColumnName])},
                        };

                            #endregion

                            #region ClaimDetails
                            if (dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows.Count > 0)
                            {
                                DataRow drClaim = dsVisit.Tables[dsVisit.PatientVisitsDetail.TableName].Rows[0];
                                keyValues.Add("InsCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsChargesColumn.ColumnName]));
                                keyValues.Add("PatCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatChargesColumn.ColumnName]));
                                keyValues.Add("TotalCharges", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalChargesColumn.ColumnName]));
                                keyValues.Add("InsBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsBalancesColumn.ColumnName]));
                                keyValues.Add("PatBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatBalancesColumn.ColumnName]));
                                keyValues.Add("TotalBalances", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalBalancesColumn.ColumnName]));
                                keyValues.Add("InsPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsPaymentsColumn.ColumnName]));
                                keyValues.Add("PatPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatPaymentsColumn.ColumnName]));
                                keyValues.Add("TotalPayments", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalPaymentsColumn.ColumnName]));
                                keyValues.Add("InsAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.InsAdjustColumn.ColumnName]));
                                keyValues.Add("PatAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.PatAdjustColumn.ColumnName]));
                                keyValues.Add("TotalAdjust", String.Format(Format, drClaim[dsVisit.PatientVisitsDetail.TotalAdjustColumn.ColumnName]));
                            }
                            #endregion
                            List<Dictionary<string, string>> lstChargeVisits = new List<Dictionary<string, string>>();
                            List<Dictionary<string, string>> lstPaymentsData = new List<Dictionary<string, string>>();
                            #region FillingCharges
                            foreach (DataRow drChargeVisit in dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows)
                            {
                                string CPTDescription = null;
                                if (MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTDescriptionColumn.ColumnName]) == "")
                                {
                                    string cptCode = MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName]);
                                    //new Task(() => {
                                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string strCPT = IMO.GetAllIMOCPTCodes(null, cptCode, 1, 100, false);
                                    var CPTSerializer = ser.Deserialize<dynamic>(strCPT);
                                    var cptJSNData = CPTSerializer["CPTLoad_JSON"];
                                    var CPT = ser.Deserialize<dynamic>(cptJSNData);
                                    CPTDescription = CPT[0]["Description"];
                                    Admin_CPTCode_Detail objCptCode = new Admin_CPTCode_Detail();
                                    objCptCode.SaveCPTCodeFromFillVisit(cptCode, CPTDescription);
                                    // }).Start();
                                }

                                string IsVNC = "";
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName])))
                                    IsVNC = String.Format(Format, drChargeVisit[dsCharge.PatientCharges.IsVNCColumn.ColumnName]) == "True" ? "True" : "False";
                                var ChargeVisitValues = new Dictionary<string, string>
                            {

                                {"ChargeId", MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName])},
                                {"DOSFrom" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSFromColumn.ColumnName]).ToShortDateString():""},
                                {"DOSTo" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drChargeVisit[dsCharge.PatientCharges.DOSToColumn.ColumnName]).ToShortDateString():""},
                                { "CPT" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.CPTCodeColumn.ColumnName])},
                                { "Units" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.UnitsColumn.ColumnName])},
                                { "Modifier1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier1Column.ColumnName])},
                                { "Modifier2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier2Column.ColumnName])},
                                { "Modifier3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier3Column.ColumnName])},
                                { "Modifier4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.Modifier4Column.ColumnName])},

                                  { "ICDPointer1" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer1Column.ColumnName])},
                                { "ICDPointer2" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer2Column.ColumnName])},
                                { "ICDPointer3" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer3Column.ColumnName])},
                                { "ICDPointer4" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ICDPointer4Column.ColumnName])},

                                { "POS" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.POSCodeColumn.ColumnName])},
                                { "EMG" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.EMGColumn.ColumnName])},
                                { "FEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.FeeColumn.ColumnName])},
                                { "TotalFEE" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,MDVUtility.ToDouble(drChargeVisit[dsCharge.PatientCharges.BilledColumn.ColumnName]))},
                              { "ExpectedFee" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), String.Format(Format,drChargeVisit[dsCharge.PatientCharges.ExpectedFeeColumn.ColumnName])},
                                { "NDC" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCColumn.ColumnName])},
                                { "NDCUnit" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitColumn.ColumnName])},
                                { "NDCUnitPrice" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.NDCUnitPriceColumn.ColumnName])},
                               { "Status" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.StatusColumn.ColumnName])},
                                { "TotalUnits" + MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]), MDVUtility.ToStr(drChargeVisit[dsCharge.PatientCharges.TotalUnitsColumn.ColumnName])},
                            };

                                lstChargeVisits.Add(ChargeVisitValues);
                            }
                            #endregion
                            int ClaimPaymentsCount = 0;
                            if (dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows.Count > 0)
                            {
                                ClaimPaymentsCount = dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows.Count;
                                foreach (DataRow drVisitPayments in dsVisit.Tables[dsVisit.ClaimPayments.TableName].Rows)
                                {
                                    var VisitPaymentValues = new Dictionary<string, string>
                                    {
                                        {"PaymentId", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaymentIdColumn.ColumnName])},
                                        {"PaymentDate", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaymentDateColumn.ColumnName])},
                                        {"ApplyTo", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.ApplyToColumn.ColumnName])},
                                        {"Allowed", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.AllowedColumn.ColumnName])},
                                        {"Deductables", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.DeductablesColumn.ColumnName])},
                                        {"Coinsurance", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CoinsuranceColumn.ColumnName])},
                                        {"Copay", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CopayColumn.ColumnName])},
                                        {"Paid", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaidColumn.ColumnName])},
                                        {"Adj", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.AdjColumn.ColumnName])},
                                        {"Code", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CodeColumn.ColumnName])},
                                        {"CheckNo", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CheckNoColumn.ColumnName])},
                                        {"CheckDate", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CheckDateColumn.ColumnName])},
                                        {"PatientResponsibility",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PatientResponsibilityColumn.ColumnName])},
                                        {"InsuranceName",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.InsuranceNameColumn.ColumnName])},
                                        {"RemittanceDes", MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.RemittanceDesColumn.ColumnName])},
                                        {"Comments",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.CommentsColumn.ColumnName])},
                                        {"PaidAmountCr",MDVUtility.ToStr(drVisitPayments[dsVisit.ClaimPayments.PaidAmountCrColumn.ColumnName])},

                                    };
                                    lstPaymentsData.Add(VisitPaymentValues);
                                }
                            }
                            string ICD9IdAndDes = "";
                            string ICD10IdAndDes = "";

                            DSVisits.PatientVisitICDRow[] dICD9Rows = (DSVisits.PatientVisitICDRow[])dsVisit.PatientVisitICD.Select(dsVisit.PatientVisitICD.VisitIdColumn.ColumnName + " = '" + VisitId + "' and " + dsVisit.PatientVisitICD.ICDTypeColumn.ColumnName + "=9");
                            if (dICD9Rows.Length > 0)
                            {
                                for (int r = 0; r < dICD9Rows.Length; r++)
                                {
                                    if (ICD9IdAndDes != "")
                                        ICD9IdAndDes += " ";
                                    ICD9IdAndDes += MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.ICDCodeColumn.ColumnName]);
                                    ICD9IdAndDes += " " + MDVUtility.ToStr(dICD9Rows[r][dsVisit.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName]);
                                }
                            }
                            keyValues.Add("ICD9IdAndDes", ICD9IdAndDes);
                            DSVisits.PatientVisitICDRow[] dICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisit.PatientVisitICD.Select(dsVisit.PatientVisitICD.VisitIdColumn.ColumnName + " = '" + VisitId + "' and " + dsVisit.PatientVisitICD.ICDTypeColumn.ColumnName + "=10");
                            if (dICD10Rows.Length > 0)
                            {
                                for (int r = 0; r < dICD10Rows.Length; r++)
                                {
                                    if (ICD10IdAndDes != "")
                                        ICD10IdAndDes += " ";
                                    ICD10IdAndDes += MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.ICDCodeColumn.ColumnName]);
                                    ICD10IdAndDes += " " + MDVUtility.ToStr(dICD10Rows[r][dsVisit.PatientVisitICD.ICDCodeDescriptionColumn.ColumnName]);
                                }

                            }
                            keyValues.Add("ICD10IdAndDes", ICD10IdAndDes);
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                VisitChargesCount = dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Count,
                                VisitsChargesLoad_JSON = js.Serialize(lstChargeVisits),
                                VisitFill_JSON = js.Serialize(keyValues),
                                ClaimPaymentsLoad_JSON = js.Serialize(lstPaymentsData),
                                ClaimPaymentsCount = ClaimPaymentsCount
                            };


                            chargesRowCount = dsVisit.Tables[dsCharge.PatientCharges.TableName].Rows.Count;
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message
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
        //Start PRD-635 TahreeMalik         to verify claim exists in system with same Provider, Facility and DOS or not
        private string VerifyDuplicateClaim(string fieldsJSON)
        {
            try
            {
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                Int64 ProviderId = MDVUtility.ToInt32(SearchedfieldsJSON["providerId"]);
                Int64 FacilityId = MDVUtility.ToInt32(SearchedfieldsJSON["facilityId"]);
                Int64 PatientId = MDVUtility.ToInt32(SearchedfieldsJSON["PatientId"]);
                DateTime DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["DOSFrom"]);
                DateTime DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["DOSTo"]);
                string ClaimNumber = SearchedfieldsJSON.ContainsKey("ClaimNumber") ? MDVUtility.ToStr(SearchedfieldsJSON["ClaimNumber"]) : "";

                obj = BLLVisitsObj.VerifyDuplicateClaim(PatientId, ProviderId, FacilityId, DOSFrom, DOSTo, ClaimNumber);

                if (obj.Data != null)
                {
                    dsVisit = obj.Data;
                    if (dsVisit.Tables[dsVisit.DuplicateClaim.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.DuplicateClaim.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.No_Record_Message
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
        //End PRD-635 TahreeMalik         to verify claim exists in system with same Provider, Facility and DOS or not
        #endregion

        #region"Search function for Batch Encounter"

        private string SearchVisitsBatch(string fieldsJSON, Int64 PatientID, Int64 VisitID, Int32 PageNumber, Int32 RowsPerPage, string VisitStatus)
        {
            try
            {
                string ClaimNumber;
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON == null)
                    obj = BLLVisitsObj.LoadPatientsVisits(VisitID, PatientID, 0, 0, null, null, "", "", "");
                else
                {
                    Int64 ProviderId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtProvider"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfProvider"]) : 0;
                    Int64 FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["txtFacility"]) ? MDVUtility.ToInt32(SearchedfieldsJSON["hfFacility"]) : 0;
                    DateTime? FromAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateFrom"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateFrom"]) : null;

                    DateTime? ToAppointmentDate = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpAppointmentDateTo"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpAppointmentDateTo"]) : null;
                    ClaimNumber = SearchedfieldsJSON["txtClaimNumber"];
                    string IsActive = SearchedfieldsJSON["ddlActive"];
                    obj = BLLVisitsObj.LoadPatientsVisits(VisitID, PatientID, ProviderId, FacilityId, FromAppointmentDate, ToAppointmentDate, ClaimNumber, VisitStatus, IsActive, 0, PageNumber, RowsPerPage);


                }
                if (obj.Data != null)
                {
                    dsVisit = obj.Data;
                    if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsVisit.PatientVisits.Rows[0][dsVisit.PatientVisits.RecordCountColumn.ColumnName],
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisits.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            Message = AppPrivileges.No_Record_Message
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

        #endregion

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
                case "SAVE_VISIT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 VisitID = -1;
                            string SelectedPatientInsurance = MDVUtility.ToStr(context.Request["SelectedPatientInsurance"]);
                            string IsSelfPay = MDVUtility.ToStr(context.Request["IsSelfPay"]);
                            strJSONData = SaveVisit(fieldsJSON, PatientID, VisitID, SelectedPatientInsurance, IsSelfPay);
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
                case "SEARCH_VISITS":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            string VisitStatus = MDVUtility.ToStr(context.Request["VisitStatus"]);
                            if (VisitStatus == "undefined")
                                VisitStatus = "";
                            strJSONData = SearchVisits(fieldsJSON, PatientID, VisitID, PageNumber, RowsPerPage, VisitStatus);
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
                case "SEARCH_CASE_VISITS":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Demographic", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseID"]);
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            strJSONData = SearchCaseVisits(PatientID, CaseID);
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
                case "SEARCH_VISIT_PLANS":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string strJSONData = SearchVisitPlans(VisitID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_VISIT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string strJSONData = FillVisit(PatientID, VisitID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_VISIT_CHARGE":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string strJSONData = LoadVisitCharges(VisitID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_VISIT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                            string SelectedPatientInsurance = MDVUtility.ToStr(context.Request["SelectedPatientInsurance"]);
                            strJSONData = SaveVisit(fieldsJSON, PatientID, VisitID, SelectedPatientInsurance, "False");
                            //string strJSONData = UpdateVisit(fieldsJSON, PatientID, VisitID, ChargeRowIds);
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
                case "UPDATE_VISIT_ACTIVE_INACTIVE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                            Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                            strJSONData = UpdateVisitIsActive(PatientID, VisitID, IsActive);
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
                case "DELETE_VISITS":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            String VisitID = context.Request["VisitID"];
                            strJSONData = DeleteVisit(VisitID);
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

                case "RESUBMIT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Encounter", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            string VisitID = context.Request["VisitID"];
                            strJSONData = Resubmit(VisitID);
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
                case "SEARCH_VISITS_BATCH":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Batch Encounter", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["VisitData"];
                            Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            string VisitStatus = MDVUtility.ToStr(context.Request["VisitStatus"]);
                            if (VisitStatus == "undefined")
                                VisitStatus = "";
                            strJSONData = SearchVisitsBatch(fieldsJSON, PatientID, VisitID, PageNumber, RowsPerPage, VisitStatus);
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
                case "CLAIM_DOCUMENTS":
                    {
                        string VisitID = MDVUtility.ToStr(context.Request["VisitID"]);
                        string strJSONData = SearchClaimDocuments(VisitID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELATE_VISIT_ICD":
                    {
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        int ICDIndex = MDVUtility.ToInt(context.Request["ICDIndex"]);
                        string strJSONData = DeleteVisitICD(VisitId, ICDIndex);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CLAIM_PAYMENTS_DETAILS":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string strJSONData = LoadClaimPaymentsDetail(VisitID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CLAIM_SUMMARY":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        string strJSONData = ClaimSummaryPreview(PatientID, VisitID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_MULTIPLE_NOTE_COMMENTS":
                    {
                        string fieldsJSON = MDVUtility.ToStr(context.Request["VisitData"]);
                        string strJSONData = SaveMultipleNotesComments(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                 case "VERIFY_DUPLICATE_CLAIM":
                    {
                        string fieldsJSON = context.Request["ClaimData"];
                        string strJSONData = VerifyDuplicateClaim(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}