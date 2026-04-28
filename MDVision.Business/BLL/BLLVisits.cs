using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Visits;
using System;
using MDVision.DataAccess.DAL.Charges;
using MDVision.DataAccess.DCommon;
using System.Data;
using System.Collections.Generic;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Billing.ERA;

namespace MDVision.Business.BLL
{
    public class BLLVisits
    {
        #region Constructors
        public BLLVisits()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call

        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Variable

        #endregion

        #region Visits
        /// <summary>
        /// Loads Visits .
        /// </summary>
        public BLObject<DSVisits> LoadPatientsVisits(long VisitId, long PatientId, long ProviderId, long FacilityId, DateTime? fAppDate, DateTime? toAppDate, string ClaimNum, string IsCheckout, string IsActive, int _837Batchid = 0, int PageNumber = 1, int RowspPage = 15, Int64 CaseMgmtId = 0)

        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadPatientVisits(VisitId, PatientId, ProviderId, FacilityId, fAppDate, toAppDate, ClaimNum, IsCheckout, IsActive, _837Batchid, PageNumber, RowspPage, CaseMgmtId);

                // Load Visit Charges for specific Visit Only
                if (VisitId > 0)
                {
                    //change by mr.Azhar ( suggestions )
                    DSCharge dsc = new DSCharge();
                    dsc = new DALCharge().LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, VisitId, 1);
                    if (dsc != null)
                        ds.Merge(dsc);
                }

                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadPatientsVisits", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }


        public BLObject<DSCharge> LoadVisitCharges(long VisitId)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALCharge().LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, VisitId, 1);
                return new BLObject<DSCharge>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadPatientsVisits", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }
        }

        public BLObject<DSVisits> searchPatientsVisits(long VisitId, long PatientId, long ProviderId, long FacilityId, DateTime? fAppDate, DateTime? toAppDate, string ClaimNum, string IsCheckout, string IsActive, int PageNumber = 1, int RowspPage = 1000)

        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().searchPatientVisits(VisitId, PatientId, ProviderId, FacilityId, fAppDate, toAppDate, ClaimNum, IsCheckout, IsActive, PageNumber, RowspPage);


                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::searchPatientsVisits", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        public BLObject<string> VoidAndReCreateVisit(string VisitId)
        {
            try
            {
                VisitId = new DALVisits().VoidAndReCreateVisit(VisitId);
                return new BLObject<string>(VisitId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::VoidAndReCreateVisit", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSVisits> SplitClaim(long VisitId)
        {
            try
            {
                //RULES
                //a- Create New Claim.
                //b- Copy all claim information from the original claim.
                //c- Date Submitted and Submitted by will be empty.

                //IMPLEMENTATION
                //a- Load current claim
                //b- create new claim with copy information of selected current
                //c- new claim apply rules
                //d- update current claim as splitted.

                DSVisits dsVisit = new DSVisits();
                dsVisit = new DALVisits().LoadPatientVisits(VisitId, 0, 0, 0, null, null, null, null, null);

                DSCharge dsCharge = new DSCharge();
                dsCharge = new DALCharge().LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, VisitId, 0, null, 0, 1, 1000, null, null, 1);

                dsCharge.Merge(new DALCharge().LoadChargesPointers(0, 0, VisitId));
                dsVisit.Merge(new DALVisits().LoadPatientVisitICDs(VisitId, 0));

                DateTime date_now = DateTime.Now;
                string UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);

                if (dsVisit.PatientVisits.Rows.Count > 0)
                {
                    //1- copy claim and charges information
                    DSVisits NewClaimDS = new DSVisits();
                    DSCharge NewClaimChargesDS = new DSCharge();
                    DSVisits NewClaimICD = new DSVisits();
                    DSCharge NewChargePointers = new DSCharge();

                    List<string> VisitIgnoreList = new List<string>();
                    VisitIgnoreList.Add(NewClaimDS.PatientVisits.VisitIdColumn.ColumnName);
                    VisitIgnoreList.Add(NewClaimDS.PatientVisits.MasterVisitIdColumn.ColumnName);
                    VisitIgnoreList.Add(NewClaimDS.PatientVisits.ClaimNumberColumn.ColumnName);

                    List<string> ChargeIgnoreList = new List<string>();
                    VisitIgnoreList.Add(NewClaimChargesDS.PatientCharges.ChargeCapIdColumn.ColumnName);
                    VisitIgnoreList.Add(NewClaimChargesDS.PatientCharges.MasterChargeIdColumn.ColumnName);

                    DSVisits.PatientVisitsRow drNew = (DSVisits.PatientVisitsRow)MDVUtility.CopyRow(dsVisit.PatientVisits.Rows[0], NewClaimDS, NewClaimDS.PatientVisits.TableName, VisitIgnoreList);

                    drNew.SubmittedBy = null;
                    drNew.SubmittedDate = null;
                    drNew.IsSplitted = false;
                    drNew.SplittedVisitId = VisitId;
                    drNew.CreatedBy = UserName;
                    drNew.CreatedOn = date_now;
                    drNew.ModifiedBy = UserName;
                    drNew.ModifiedOn = date_now;
                    drNew.SubmitStatusId = 1;
                    drNew.SubmitStatus = "Pending";
                    drNew.ClaimStatusId = 1;
                    NewClaimDS.PatientVisits.AddPatientVisitsRow(drNew);

                    IDBManager dbManager = ClientConfiguration.GetDBManager();
                    //Begin Transaction
                    dbManager.BeginTransaction();
                    try
                    {
                        //2- Insert new copied claim
                        NewClaimDS = new DALVisits().InsertPatientVisits(NewClaimDS, dbManager);
                        DSVisits.PatientVisitsRow drNewSaved = (DSVisits.PatientVisitsRow)NewClaimDS.PatientVisits.Rows[0];

                        //2.5 Insert VisitICD's
                        foreach (DSVisits.PatientVisitICDRow item in dsVisit.PatientVisitICD.Rows)
                        {
                            DSVisits.PatientVisitICDRow drICD = NewClaimICD.PatientVisitICD.NewPatientVisitICDRow();
                            drICD.ICDType = item.ICDType;
                            drICD.ICDCode = item.ICDCode;
                            drICD.ICDCodeDescription = item.ICDCodeDescription;
                            drICD.SNOMEDID = item.SNOMEDID;
                            drICD.SNOMEDDescription = item.SNOMEDDescription;
                            drICD.LexiCode = item.LexiCode;
                            drICD.LexiCodeDescription = item.LexiCodeDescription;
                            drICD.VisitId = drNewSaved.VisitId;
                            drICD.OldPVICDId = item.PVICDId;
                            NewClaimICD.PatientVisitICD.AddPatientVisitICDRow(drICD);
                        }
                        NewClaimICD = new DALVisits().InsertUpdatePatientVisitsICD(NewClaimICD, dbManager);

                        //3- Insert new claims charges
                        foreach (DSCharge.PatientChargesRow item in dsCharge.PatientCharges.Rows)
                        {
                            DSCharge.PatientChargesRow drcharge = (DSCharge.PatientChargesRow)MDVUtility.CopyRow(item, NewClaimChargesDS, NewClaimChargesDS.PatientCharges.TableName, ChargeIgnoreList);

                            drcharge.VisitId = drNewSaved.VisitId;
                            drcharge.StatusId = 1;
                            drcharge.Status = "Regular";
                            drcharge.OldChargeCapId = item.ChargeCapId;
                            drcharge.CreatedBy = UserName;
                            drcharge.CreatedOn = date_now;
                            drcharge.ModifiedBy = UserName;
                            drcharge.ModifiedOn = date_now;
                            NewClaimChargesDS.PatientCharges.AddPatientChargesRow(drcharge);
                        }
                        NewClaimChargesDS = new DALCharge().InsertPatientCharges(NewClaimChargesDS, dbManager);

                        //3.5 Insert Charges Pointers
                        foreach (DSCharge.ChargesICDPointersRow item in dsCharge.ChargesICDPointers)
                        {
                            DSCharge.ChargesICDPointersRow drPointer = NewChargePointers.ChargesICDPointers.NewChargesICDPointersRow();

                            DSVisits.PatientVisitICDRow[] dr_icd = (DSVisits.PatientVisitICDRow[])NewClaimICD.PatientVisitICD.Select(NewClaimICD.PatientVisitICD.OldPVICDIdColumn + "=" + item.PVICDId);

                            DSCharge.PatientChargesRow[] dr_charge = (DSCharge.PatientChargesRow[])NewClaimChargesDS.PatientCharges.Select(NewClaimChargesDS.PatientCharges.OldChargeCapIdColumn + "=" + item.ChargeCapId);

                            if (dr_icd.Length > 0 && dr_charge.Length > 0)
                            {
                                drPointer.PVICDId = dr_icd[0].PVICDId;
                                drPointer.ChargeCapId = dr_charge[0].ChargeCapId;
                                drPointer.Order = item.Order;
                                NewChargePointers.ChargesICDPointers.AddChargesICDPointersRow(drPointer);
                            }
                        }
                        NewChargePointers = new DALCharge().InsertChargesPointers(NewChargePointers, dbManager);

                        //4- update current claim
                        DSVisits.PatientVisitsRow drCurrent = (DSVisits.PatientVisitsRow)dsVisit.PatientVisits.Rows[0];
                        drCurrent.IsSplitted = true;

                        dsVisit.Merge(new DALVisits().UpdatePatientVisits(dsVisit, dbManager));

                        //Comment Transaction
                        dbManager.CommitTransaction();
                        return new BLObject<DSVisits>(dsVisit);
                    }
                    catch (Exception ex)
                    {
                        //RollBack Transaction
                        dbManager.RollBackTransaction();
                        MDVLogger.BLLErrorLog("BLLBilling::SplitClaim", ex);
                        return new BLObject<DSVisits>(null, ex.Message);
                    }
                    finally
                    {
                        //Dispose dbManager
                        dbManager.Dispose();
                    }


                }
                else
                    throw new Exception("No Record found.");
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::SplitClaim", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        public BLObject<DSVisits> LoadClaimVoidInfo(long VisitId)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadClaimVoidInfo(VisitId);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadClaimVoidInfo", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads Patient Outstanding Visits .
        /// </summary>
        public BLObject<DSVisits> LoadPatientOutstandingVisits(long PatientId, string PatientOutstanding = "")
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadPatientOutstandingVisits(PatientId, PatientOutstanding);

                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadPatientOutstandingVisits", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads Visits Plans.
        /// </summary>
        public BLObject<DSVisitLookup> LoadVisitsPlans(long VisitId)
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LoadVisitPlan(VisitId);

                DSVisitLookup.PatientVisitsRow[] _rows = (DSVisitLookup.PatientVisitsRow[])ds.Tables[ds.PatientVisits.TableName].Select("1=1", ds.PatientVisits.VisitIdColumn.ColumnName + " ASC");

                DSVisitLookup ds_res = new DSVisitLookup();
                foreach (DSVisitLookup.PatientVisitsRow item in _rows)
                    ds_res.PatientVisits.ImportRow(item);

                return new BLObject<DSVisitLookup>(ds_res);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadVisitsPlans", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the PatientsVisit.
        /// </summary>
        /// <param name="CaseMgmtId">The case identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientsVisit(string VisitId)
        {
            try
            {

                VisitId = new DALVisits().DeleteVisit(VisitId);

                return new BLObject<string>(VisitId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteLedgerAccount", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the Patients Visit.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///
        public BLObject<DSVisits> InsertPatientsVisit(DSVisits ds)
        {
            try
            {
                // Mark this visit as Not ReadyToSubmit
                // No charge is attached to this visit yet
                //foreach (DataRow drVisit in ds.PatientVisits.Rows)
                //{
                //    drVisit[ds.PatientVisits.SubmitStatusIdColumn.ColumnName] = 1;
                //}

                ds = new DALVisits().InsertPatientVisits(ds);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::InsertPatientsVisit", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the Patients Visit.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///

        public BLObject<DSVisits> UpdatePatientsVisit(DSVisits ds)
        {
            try
            {
                ds = new DALVisits().UpdatePatientVisits(ds);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::UpdatePatientsVisit", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        public BLObject<DSVisits> UpdatePatientsVisitWithTran(DSVisits ds, IDBManager dbManager)
        {
            try
            {
                ds = new DALVisits().UpdatePatientVisits(ds, dbManager);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::UpdatePatientsVisit", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        public BLObject<DSVisits> InsertUpdatePatientVisits(DSVisits dsVisit, DSCharge dsCharge, DSVisits dsVisitICD = null)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCharge dsChargePointers = new DSCharge();
            DSCharge dsChargeAudit = new DSCharge();
            DSVisits dsVisitICDAudit = new DSVisits();
            DataTable dtTempCharge = null;
            DataTable dtTempVisitICD = null;
            DataTable dtTempVisit = null;
            try
            {
                dtTempVisit = dsVisit.PatientVisits.GetChanges();
                dbManager.BeginTransaction();

                dsVisit = new DALVisits().InsertUpdatePatientVisits(dsVisit, dbManager);

                foreach (DataRow drVisit in dsVisit.PatientVisits.Rows)
                {
                    foreach (DataRow drCharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                    {
                        if (drCharge.RowState == DataRowState.Added)
                        {
                            drCharge[dsCharge.PatientCharges.VisitIdColumn.ColumnName] = drVisit[dsVisit.PatientVisits.VisitIdColumn.ColumnName];
                        }

                    }

                    foreach (DataRow drVisitIcd in dsVisitICD.Tables[dsVisitICD.PatientVisitICD.TableName].Rows)
                    {
                        if (drVisitIcd.RowState == DataRowState.Added)
                        {
                            drVisitIcd[dsVisitICD.PatientVisitICD.VisitIdColumn.ColumnName] = drVisit[dsVisit.PatientVisits.VisitIdColumn.ColumnName];
                        }

                        ////DSCharge.PatientChargesRow drCurrentCharge = (DSCharge.PatientChargesRow)drCharge;
                        //var CurrentChargeRow = drCharge.ItemArray;
                        //CurrentChargeRow[0] = null;
                        //dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows.Add(CurrentChargeRow);
                    }
                }


                if (dsCharge.PatientCharges.Rows.Count > 0)
                {
                    //dsCharge.AcceptChanges();
                    dtTempCharge = dsCharge.PatientCharges.GetChanges();
                    dsCharge = new DALCharge().InsertUpdatePatientCharges(dsCharge, dbManager);
                    dsChargeAudit = dsCharge;
                }

                if (dsVisitICD != null && dsVisitICD.PatientVisitICD.Rows.Count > 0)
                {
                    dtTempVisitICD = dsVisitICD.PatientVisitICD.GetChanges();
                    dsVisitICD = new DALVisits().InsertUpdatePatientVisitsICDWithTran(dsVisitICD, dbManager);
                    dsVisitICDAudit = dsVisitICD;
                }

                // Charge Pointers
                foreach (DataRow drCharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                {
                    //1- Delete this charge Previous Pointers
                    //2- Add Its new Pointers
                    long chargeCapId = MDVUtility.ToLong(drCharge[dsCharge.PatientCharges.ChargeCapIdColumn.ColumnName]);
                    int pointer1Index = MDVUtility.ToInt(drCharge[dsCharge.PatientCharges.ICDPointer1Column.ColumnName]);
                    int pointer2Index = MDVUtility.ToInt(drCharge[dsCharge.PatientCharges.ICDPointer2Column.ColumnName]);
                    int pointer3Index = MDVUtility.ToInt(drCharge[dsCharge.PatientCharges.ICDPointer3Column.ColumnName]);
                    int pointer4Index = MDVUtility.ToInt(drCharge[dsCharge.PatientCharges.ICDPointer4Column.ColumnName]);

                    string PatientChargesPointers = new DALCharge().DeleteChargesPointers(0, chargeCapId, dbManager);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer1Index - 1, 1);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer2Index - 1, 2);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer3Index - 1, 3);
                    BuildICDPointers(ref dsChargePointers, dsVisitICD, chargeCapId, pointer4Index - 1, 4);
                }

                if (dsChargePointers.ChargesICDPointers.Rows.Count > 0)
                {
                    dsCharge = new DALCharge().InsertChargesPointers(dsChargePointers, dbManager);
                }

                dbManager.CommitTransaction();
                dsCharge.AcceptChanges();
                dsVisit.AcceptChanges();
                dsVisitICD.AcceptChanges();
                dsVisit.Merge(dsVisitICD);
            }
            catch (Exception ex)
            {
                dsVisit.RejectChanges();
                dsCharge.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLVisits::InsertUpdatePatientVisits", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

            try
            {
                //Audit log after transaction commit
                //Visit Audit log 
                new DALVisits().InsertUpdatePatientVisitsAuditLog(dtTempVisit, dsVisit);

                //charges Auditlog
                new DALCharge().InsertUpdatePatientChargesAuditlog(dtTempCharge, dsChargeAudit);

                //ICDPointers Auditlog
                new DALVisits().InsertUpdatePatientVisitsICDAuditLog(dtTempVisitICD, dsVisitICDAudit);
                return new BLObject<DSVisits>(dsVisit);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::InsertUpdatePatientVisits - Audit log insertion failed", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
         
        }

        public BLObject<string> SaveMultipleNoteComments(string VisitIds, string ModifiedBy, DateTime ModifiedOn, string NoteComments = null, string NoteModifiedBy = null, DateTime? NoteModifiedOn = null)
        {
            try
            {
                string result = new DALVisits().SaveMultipleNoteComments(VisitIds, ModifiedBy, ModifiedOn, NoteComments, NoteModifiedBy, NoteModifiedOn);
                if (!string.IsNullOrEmpty(result))
                    throw (new Exception(result));
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::SaveMultipleNoteComments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Load Visits' Charges, Payments, Adjustment and Balances
        /// </summary>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        public BLObject<DSVisits> LoadPatientVistsDetails(long VisitId)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadPatientVistsDetails(VisitId);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadPatientVistsDetails", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        /// <summary>
        /// Load Claim Payments
        /// </summary>
        /// <param name="VisitId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowspPage"></param>
        /// <returns></returns>
        public BLObject<DSVisits> LoadClaimPayments(long VisitId)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadClaimPayments(VisitId);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadClaimPayments", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        public void BuildICDPointers(ref DSCharge dsChargePointers, DSVisits dsVisitICD, long chargeCapId, int pointerIndex, int Order)
        {
            try
            {
                if (pointerIndex >= 0)
                {
                    // ICD 9 row

                    DSVisits.PatientVisitICDRow[] dICD9Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=9", dsVisitICD.PatientVisitICD.PVICDIdColumn.ColumnName);
                    DSCharge.ChargesICDPointersRow drPointer9 = dsChargePointers.ChargesICDPointers.NewChargesICDPointersRow();
                    drPointer9.ChargeCapId = chargeCapId;
                    if (dICD9Rows.Length > pointerIndex)
                    {
                        DSVisits.PatientVisitICDRow drr9 = dICD9Rows[pointerIndex];
                        drPointer9.Order = Order;
                        drPointer9.PVICDId = drr9.PVICDId;
                        dsChargePointers.ChargesICDPointers.AddChargesICDPointersRow(drPointer9);
                    }


                    // ICD 10 row

                    DSVisits.PatientVisitICDRow[] dICD10Rows = (DSVisits.PatientVisitICDRow[])dsVisitICD.PatientVisitICD.Select(dsVisitICD.PatientVisitICD.ICDTypeColumn.ColumnName + "=10", dsVisitICD.PatientVisitICD.PVICDIdColumn.ColumnName);
                    DSCharge.ChargesICDPointersRow drPointer10 = dsChargePointers.ChargesICDPointers.NewChargesICDPointersRow();
                    drPointer10.ChargeCapId = chargeCapId;
                    if (dICD10Rows.Length > pointerIndex)
                    {
                        DSVisits.PatientVisitICDRow drr10 = dICD10Rows[pointerIndex];
                        drPointer10.Order = Order;
                        drPointer10.PVICDId = drr10.PVICDId;
                        dsChargePointers.ChargesICDPointers.AddChargesICDPointersRow(drPointer10);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::BuildICDPointers", ex);
            }
        }
        //Start PRD-635 TahreeMalik         to verify claim exists in system with same Provider, Facility and DOS or not
        public BLObject<DSVisits> VerifyDuplicateClaim(Int64 PatientId, Int64 ProviderId, Int64 FacilityId, DateTime DOSFrom, DateTime DOSTo, string ClaimNumber = null)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().VerifyDuplicateClaim(PatientId, ProviderId, FacilityId, DOSFrom, DOSTo, ClaimNumber);

                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::VerifyDuplicateClaim", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }
        //End PRD-635 TahreeMalik

        #endregion

        #region Patient Visit ICD
        /// <summary>
        /// Load PatientVisit ICDs
        /// </summary>
        /// <param name="VisitId"></param>
        /// <param name="ICDType"></param>
        /// <returns></returns>
        public BLObject<DSVisits> LoadPatientVisitICDs(long VisitId, Int32 ICDType)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadPatientVisitICDs(VisitId, ICDType);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadPatientVisitICDs", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }
        public BLObject<DSCharge> DeleteVisitICD(long VisitId, int ICDIndex)
        {
            try
            {
                DSCharge ds = new DSCharge();
                ds = new DALVisits().DeleteVisitICD(VisitId, ICDIndex);
                return new BLObject<DSCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits:DeleteVisitICD", ex);
                return new BLObject<DSCharge>(null, ex.Message);
            }

        }
        public BLObject<bool> DeleteVisitICDByPVICDID(long PVICDId)
        {
            try
            {
                bool res = false;
                res = new DALVisits().DeleteVisitICDByPVICDID(PVICDId);
                return new BLObject<bool>(res);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits:DeleteVisitICDByPVICDID", ex);
                return new BLObject<bool>(false, ex.Message);
            }

        }


        #endregion

        #region " Claim Errors "

        public BLObject<DSVisits> LoadClaimErrors(long ErrorId, long VisitId)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALVisits().LoadClaimErrors(ErrorId, VisitId);

                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LoadClaimErrors", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }

        #endregion

        #region lookUp
        /// <summary>
        /// Lookups the Patient Visits.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSVisitLookup> LookupPatientVisits(Int64 PatientId = 0, string ClaimNumber = "", Boolean isVisitCharge = false)
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupPatientVisits(PatientId, ClaimNumber, isVisitCharge);
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupPatientVisit", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the Visit Delay Reason.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSVisitLookup> LookupVisitsDelayReason()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupVisitsDelayReason();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupVisitsDelayReason", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the Visit Claim Frequency.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSVisitLookup> LookupVisitsClaimFrequency()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupVisitsClaimFrequency();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupVisitsClaimFrequency", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupReportTypeCode()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupReportTypeCode();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupReportTypeCode", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupTransmissionCode()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupTransmissionCode();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupTransmissionCode", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }


        /// <summary>
        /// Lookups the Visit Status.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSVisitLookup> LookupVisitStatus()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupVisitStatus();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupVisitStatus", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the Submit Status.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSVisitLookup> LookupSubmitStatus(string IsActive)
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupSubmitStatus(IsActive);
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupSubmitStatus", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupVoidedStatus(string IsActive)
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupVoidedStatus(IsActive);
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupSubmitStatus", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupAnesthesiaType()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupAnesthesiaType();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupAnesthesiaType", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupAnesServiceType()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupAnesServiceType();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupAnesServiceType", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }

        public BLObject<DSVisitLookup> LookupAnesthesiaASA()
        {
            try
            {
                DSVisitLookup ds = new DSVisitLookup();
                ds = new DALVisits().LookupAnesthesiaASA();
                return new BLObject<DSVisitLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::LookupAnesthesiaASA", ex);
                return new BLObject<DSVisitLookup>(null, ex.Message);
            }
        }
        public BLObject<List<STC>> getEDIDetail(string claimNumber)
        {
            try
            {
                List<STC> EdiDetail = new List<STC>();
                EdiDetail = new DALVisits().getEDIDetail(claimNumber);
                return new BLObject<List<STC>>(EdiDetail);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::getEDIDetail", ex);
                return new BLObject<List<STC>>(null, ex.Message);
            }
        }
        
        public BLObject<string> getClaimStatus(string claimNumber)
        {
            try
            {
                
                string status = new DALVisits().getClaimStatus(claimNumber);
                return new BLObject<string>(status);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLVisits::getClaimStatus", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion
    }
}
