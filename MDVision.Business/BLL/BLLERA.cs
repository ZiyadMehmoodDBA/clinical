using MDVision.Business.BCommon;
using MDVision.DataAccess.DAL.Admin.ERA;
using MDVision.DataAccess.DAL.ERA;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EDIParser;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.DataAccess.DAL.Claim;
using System.Data;
using MDVision.DataAccess.DAL.Payment;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Billing.ERA;

namespace MDVision.Business.BLL
{

    public class BLLERA
    {

        #region Constructors
        public BLLERA()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            //this. = ;
            //Add your own initialization code after the InitializeComponent() call

        }
        //public BLLERA()
        //{
        //    //SharedVariable 
        //    //This call is required by the Web Services Designer.
        //    InitializeComponent();

        //    //Add your own initialization code after the InitializeComponent() call

        //}
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

        #region ERAAdjustmentCode

        public BLObject<DSERA> LoadERAAdjustmentCode(long ERAAdjustmentCodeId, long ClaimAdjGroupCodeId, long ClaimAdjReasonCodesId, long ClearinghouseId, long ERAActionId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERAAdjustmentCode().LoadERAAdjustmentCode(ERAAdjustmentCodeId, ClaimAdjGroupCodeId, ClaimAdjReasonCodesId, ClearinghouseId, ERAActionId, IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LoadERAAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSERA> InsertERAAdjustmentCode(ref DSERA ds)
        {
            try
            {

                ds = new DALERAAdjustmentCode().InsertERAAdjustmentCode(ref ds);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::InsertERAAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        public BLObject<List<ERAVoidedClaims>> IsVoidedClaimExist(Int64 ERAID,Int64 VisitId)
        {
            try
            {
                List<ERAVoidedClaims> lstERAVoidedClaims = new List<ERAVoidedClaims>();
                lstERAVoidedClaims = new DALERA().IsVoidedClaimExist(ERAID, VisitId);
                return new BLObject<List<ERAVoidedClaims>>(lstERAVoidedClaims);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::IsVoidedClaimExist", ex);
                return new BLObject<List<ERAVoidedClaims>>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSERA> UpdateERAAdjustmentCode(DSERA ds)
        {
            try
            {

                ds = new DALERAAdjustmentCode().UpdateERAAdjustmentCode(ds);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::UpdateERAAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the LedgerAccount.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteERAAdjustmentCode(string ERAAdjCodeId)
        {
            try
            {

                ERAAdjCodeId = new DALERAAdjustmentCode().DeleteERAAdjustmentCode(ERAAdjCodeId);

                return new BLObject<string>(ERAAdjCodeId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::DeleteERAAdjustmentCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #region "Lookups"

        public BLObject<DSERALookup> LookupAdjustmentGroupCode()
        {
            try
            {
                DSERALookup ds = new DSERALookup();
                ds = new DALERAAdjustmentCode().LookupAdjustmentGroupCode();

                return new BLObject<DSERALookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LookupAdjustmentGroupCode", ex);
                return new BLObject<DSERALookup>(null, ex.Message);
            }

        }



        public BLObject<DSERALookup> LookupAdjustmentReasonCode()
        {
            try
            {
                DSERALookup ds = new DSERALookup();
                ds = new DALERAAdjustmentCode().LookupAdjustmentReasonCode();
                return new BLObject<DSERALookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LookupAdjustmentReasonCode", ex);
                return new BLObject<DSERALookup>(null, ex.Message);
            }
        }

        #endregion
        #endregion

        #region ERAActions

        public BLObject<DSERA> LoadERAAction(long ERAActionId)
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERAActions().LoadERAAction(ERAActionId);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LoadERAAction", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSERA> InsertERAAction(ref DSERA ds)
        {
            try
            {

                ds = new DALERAActions().InsertERAAction(ref ds);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::InsertERAAction", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the LedgerAccount.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSERA> UpdateERAAction(ref DSERA ds)
        {
            try
            {

                ds = new DALERAActions().UpdateERAAction(ref ds);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::UpdateERAAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the LedgerAccount.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteERAAction(string ERAActionIds)
        {
            try
            {

                ERAActionIds = new DALERAActions().DeleteERAAction(ERAActionIds);

                return new BLObject<string>(ERAActionIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::DeleteERAAction", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #region "Lookups"


        #endregion
        #endregion

        #region ERA Detail


        public BLObject<DSERA> FillERALinkedHistory(long ERAID)
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().FillERALinkedHistory(ERAID);
                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::FillERALinkedHistory", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        public BLObject<DSERA> LoadERADetail(long ERADtID, long ERAID, string Module = "")
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().LoadERADetail(ERADtID, ERAID, Module);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LoadERADetail", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }
        public BLObject<DSERA> LoadERAClaimAdjustmentCode(long ERAClaimAdjId, long ERADtID)
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().LoadERAClaimAdjustmentCode(ERAClaimAdjId, ERADtID);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LoadERAClaimAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }
        public BLObject<DSERA> UpdateERADetail(DSERA ds)
        {
            try
            {

                ds = new DALERA().UpdateERADetail(ds);
                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::UpdateERADetail", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }
        public BLObject<string> LinkERADetail(long ERADtID, long ChargeId, bool IsLink, long PaymentInsuranceId)
        {
            try
            {
                string response = "";
                response = new DALERA().LinkERADetail(ERADtID, ChargeId, IsLink, PaymentInsuranceId);
                return new BLObject<string>(response);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LinkERADetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteERADetail(string ERADtID)
        {
            try
            {
                ERADtID = new DALERA().DeleteERADetail(ERADtID);
                return new BLObject<string>(ERADtID);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteERADetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSERA> LoadERAProviderAdjustmentCode(long ERAProviderAdjId, long ERAId)
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().LoadERAProviderAdjustments(ERAProviderAdjId, ERAId);

                return new BLObject<DSERA>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLERA::LoadERAProviderAdjustmentCode", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }

        }

        #endregion

        #region " ERA "

        public BLObject<bool> DownloadERA(long EDIReportId = 0)
        {
            List<ERAActivitySteps> ActivityStepList = new List<ERAActivitySteps>();
            try
            {

                #region Step #:1 LoadReportStep
                ERAActivitySteps LoadReportStep = new ERAActivitySteps("Step #:1", "Function call to Load EDI Reports", "Start Loading EDI report with Perameter 'EDIReportId:" + EDIReportId + "'", 0, 0, EDIReportId);
                ActivityStepList.Add(LoadReportStep);
                #endregion

                DSEDIReports dsReoprt = new DSEDIReports();
                if (EDIReportId == 0)
                    dsReoprt = new DALEDIReports().LoadEDIReports(0, 0, "", "", "835", "", "", null, "");//Step #:2
                else
                    dsReoprt = new DALEDIReports().LoadEDIReports(EDIReportId, 0, "", "", "835", "", "", null, null);//Step #:2

                if (dsReoprt.EDIReports.Rows.Count > 0)
                {

                    LoadReportStep.EndTime = DateTime.Now;
                    LoadReportStep.Details += "Sucessfully Loaded EDI Reports with EDIReports Rows:" + dsReoprt.EDIReports.Rows.Count;
                    LoadReportStep.Status = true;

                    foreach (DSEDIReports.EDIReportsRow drReport in dsReoprt.EDIReports.Rows)
                    {
                        try
                        {
                            dsCache ds835Parser = new dsCache();
                            LoadReportStep.EDIReportId = drReport.EDIReportId;
                            #region Step #:3 LoadReportStep
                            ERAActivitySteps ParsingLoadedReportStep = new ERAActivitySteps("Step #:3", "Function call to Parse the loaded EDI File", "Start of parsing with the perameters:'EDIText:" + drReport.EDIText + "EDIReportId:" + drReport.EDIReportId + "FileName:" + drReport.FileName + "UserName:" + MDVSession.Current.AppUserName + "EDIReportId:" + drReport.EDIReportId + "'", 0, 0, drReport.EDIReportId);
                            ActivityStepList.Add(ParsingLoadedReportStep);
                            #endregion
                            string str = EDICommon.ParseEDI(drReport.EDIText, drReport.EDIReportId, drReport.FileName, ref ds835Parser, MDVSession.Current.AppUserName);

                            if (ds835Parser.Table_BL_ERA_835.Rows.Count > 0)
                            {
                                ParsingLoadedReportStep.Details += "EDI file has been parsed in '" + ds835Parser.Table_BL_ERA_835.Rows.Count + "' rows";
                                ParsingLoadedReportStep.EndTime = DateTime.Now;
                                ParsingLoadedReportStep.Status = true;
                                IDBManager dbManager = ClientConfiguration.GetDBManager();

                                //Begin Transaction
                                dbManager.BeginTransaction();
                                try
                                {
                                    #region Step #:4 DBInsertionParsedData
                                    ERAActivitySteps DBInsertionParsedData = new ERAActivitySteps("Step #:4", "Function call to Insert Parsed Data in ERA, ERADetail, ClaimAdjustmentCode Tables", "Start inserting Parsed data in ERA, ERADetail, ClaimAdjustmentCode Tables", 0, 0, drReport.EDIReportId);
                                    ActivityStepList.Add(DBInsertionParsedData);
                                    #endregion

                                    #region Insert Parsed Data in DB
                                    foreach (dsCache.Table_BL_ERA_835Row dr835Parse in ds835Parser.Table_BL_ERA_835.Rows)
                                    {
                                        #region Step #:5 FillERAStep
                                        ERAActivitySteps FillERAStep = new ERAActivitySteps("Step #:5", "Fill ERA object", "Start Filling ERA object and Values are: ", 0, 0, drReport.EDIReportId);
                                        ActivityStepList.Add(FillERAStep);
                                        #endregion

                                        #region FillERA
                                        DSERA dsERA = new DSERA();

                                        DSERA.ERARow drEra = dsERA.ERA.NewERARow();
                                        drEra.ClearingHouseId = drReport.ClearingHouseId;
                                        FillERAStep.Details += "ClearingHouseId: " + drEra.ClearingHouseId + ", " + Environment.NewLine;
                                        drEra.EDIReportId = drReport.EDIReportId;
                                        FillERAStep.Details += "EDIReportId: " + drEra.EDIReportId + ", " + Environment.NewLine;
                                        drEra.Status = "UnPosted";
                                        FillERAStep.Details += "Status: " + drEra.Status + ", " + Environment.NewLine;
                                        drEra.CheckNo = dr835Parse.FIELD_ERA835_CHEQUE_NO;
                                        FillERAStep.Details += "CheckNo: " + drEra.CheckNo + ", " + Environment.NewLine;
                                        drEra.CheckAmount = MDVUtility.ToDecimal(dr835Parse.FIELD_ERA835_CHECK_EFT_AMOUNT);
                                        FillERAStep.Details += "CheckAmount: " + drEra.CheckAmount + ", " + Environment.NewLine;
                                        if (MDVUtility.IsDateTime(dr835Parse.FIELD_ERA835_CHECK_DATE))
                                        {
                                            drEra.CheckDate = Convert.ToDateTime(dr835Parse.FIELD_ERA835_CHECK_DATE);
                                            FillERAStep.Details += "CheckDate: " + drEra.CheckDate + ", " + Environment.NewLine;
                                        }
                                        //drEra.CheckDepositDate = dr835Parse;
                                        drEra.PayerName = dr835Parse.FIELD_ERA835_PAYER_NAME;
                                        FillERAStep.Details += "PayerName: " + drEra.PayerName + ", " + Environment.NewLine;
                                        drEra.ClaimPayerId = dr835Parse.FIELD_ERA835_PAYER_ID;
                                        FillERAStep.Details += "ClaimPayerId: " + drEra.ClaimPayerId + ", " + Environment.NewLine;
                                        drEra.PayerAddress = dr835Parse.FIELD_ERA835_PAYER_ADDRESS;
                                        FillERAStep.Details += "PayerAddress: " + drEra.PayerAddress + ", " + Environment.NewLine;
                                        drEra.PayerCity = dr835Parse.FIELD_ERA835_PAYER_CITY;
                                        FillERAStep.Details += "PayerCity: " + drEra.PayerCity + ", " + Environment.NewLine;
                                        drEra.PayerState = dr835Parse.FIELD_ERA835_PAYER_STATE;
                                        FillERAStep.Details += "PayerState: " + drEra.PayerState + ", " + Environment.NewLine;
                                        drEra.PayerZipCode = dr835Parse.FIELD_ERA835_PAYER_POSTAL_CODE;
                                        FillERAStep.Details += "PayerZipCode: " + drEra.PayerZipCode + ", " + Environment.NewLine;
                                        drEra.PayeeName = dr835Parse.FIELD_ERA835_PAYEE_NAME;
                                        FillERAStep.Details += "PayeeName: " + drEra.PayeeName + ", " + Environment.NewLine;
                                        drEra.PayeeAddress = dr835Parse.FIELD_ERA835_PAYEE_ADDRESS;
                                        FillERAStep.Details += "PayeeAddress: " + drEra.PayeeAddress + ", " + Environment.NewLine;
                                        drEra.PayeeCity = dr835Parse.FIELD_ERA835_PAYEE_CITY;
                                        FillERAStep.Details += "PayeeCity: " + drEra.PayeeCity + ", " + Environment.NewLine;
                                        drEra.PayeeState = dr835Parse.FIELD_ERA835_PAYEE_STATE;
                                        FillERAStep.Details += "PayeeState: " + drEra.PayeeState + ", " + Environment.NewLine;
                                        drEra.PayeeZipCode = dr835Parse.FIELD_ERA835_PAYEE_POSTAL_CODE;
                                        FillERAStep.Details += "PayeeZipCode: " + drEra.PayeeZipCode + ", " + Environment.NewLine;
                                        drEra.IsActive = true;
                                        FillERAStep.Details += "IsActive: " + drEra.IsActive + ",";
                                        drEra.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drEra.CreatedOn = DateTime.Now;
                                        drEra.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drEra.ModifiedOn = DateTime.Now;
                                        dsERA.ERA.AddERARow(drEra);
                                        FillERAStep.EndTime = DateTime.Now;
                                        #endregion

                                        FillERAStep.Details += Environment.NewLine;
                                        FillERAStep.Details += "----------------Result--------------" + Environment.NewLine;
                                        FillERAStep.Details += "ERA has been Filled Successfully";
                                        FillERAStep.Status = true;
                                        FillERAStep.EndTime = DateTime.Now;
                                        //Insert ERA
                                        #region Step #:6 InsertERAStep
                                        ERAActivitySteps InsertERAStep = new ERAActivitySteps("Step #:6", "Function call for Inserting ERA in DB", "Start Inserting ERA in DB", 0, 0, drReport.EDIReportId);
                                        ActivityStepList.Add(InsertERAStep);
                                        #endregion

                                        dsERA = new DALERA().InsertERA(dsERA, dbManager);//"Step #:7"
                                        FillERAStep.ERAId = drEra.ERAId;
                                        InsertERAStep.Details += Environment.NewLine;
                                        InsertERAStep.Details += "----------------Result--------------" + Environment.NewLine;
                                        InsertERAStep.Details += "ERA has been Inserted Successfully. Inserted ERAId:" + drEra.ERAId;
                                        InsertERAStep.ERAId = drEra.ERAId;
                                        InsertERAStep.Status = true;
                                        InsertERAStep.EndTime = DateTime.Now;
                                        if (dsERA.ERA.Rows.Count > 0)
                                        {
                                            string ERA835Id = dr835Parse.FIELD_ERA835_ID_PK.ToString();
                                            #region Provider_Adjustment

                                            #region Step #:8 ProviderAdjustmentStepLoad
                                            ERAActivitySteps ProviderAdjustmentStepLoad = new ERAActivitySteps("Step #:8", "Loading ProviderAdjustment from ERA", "Start Loading ProviderAdjustment from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(ProviderAdjustmentStepLoad);
                                            #endregion
                                            dsCache.Table_BL_Provider_AdjustmentRow[] dsRowsProviderAdjustmentsParser = (dsCache.Table_BL_Provider_AdjustmentRow[])ds835Parser.Table_BL_Provider_Adjustment.Select(ds835Parser.Table_BL_Provider_Adjustment.FIELD_REMIT_BATCH_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(ERA835Id));
                                            ProviderAdjustmentStepLoad.Details += Environment.NewLine;
                                            ProviderAdjustmentStepLoad.Details += "----------------Result--------------" + Environment.NewLine;
                                            ProviderAdjustmentStepLoad.Details += "ProviderAdjustment has been loaded Successfully";
                                            ProviderAdjustmentStepLoad.Status = true;
                                            ProviderAdjustmentStepLoad.EndTime = DateTime.Now;
                                            foreach (dsCache.Table_BL_Provider_AdjustmentRow drProviderAdjParser in dsRowsProviderAdjustmentsParser)
                                            {
                                                #region Step #:9 ProviderAdjustmentStepFill
                                                ERAActivitySteps ProviderAdjustmentStepFill = new ERAActivitySteps("Step #:9", "Filling Provider Adjustment", "Start Filling Provider Adjustment", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(ProviderAdjustmentStepFill);
                                                #endregion

                                                //Fill Provider Adjustment
                                                DSERA.ERAProviderAdjustmentsRow drEraProviderAdjustment = dsERA.ERAProviderAdjustments.NewERAProviderAdjustmentsRow();
                                                drEraProviderAdjustment.ReasonCode = drProviderAdjParser.FIELD_PLB_REASON_CODE;
                                                ProviderAdjustmentStepFill.Details += " Details of Provider Adjustment is as. ReasonCode:" + drEraProviderAdjustment.ReasonCode + ",";
                                                drEraProviderAdjustment.Description = drProviderAdjParser.FIELD_PLB_DESCRIPTION;
                                                ProviderAdjustmentStepFill.Details += " Description:" + drEraProviderAdjustment.Description + ",";
                                                drEraProviderAdjustment.Amount = drProviderAdjParser.FIELD_PLB_AMOUNT;
                                                ProviderAdjustmentStepFill.Details += " Amount:" + drEraProviderAdjustment.Amount + ",";
                                                drEraProviderAdjustment.Identifier = drProviderAdjParser.FIELD_PLB_REFRENCE_IDENTIFIER;
                                                ProviderAdjustmentStepFill.Details += " Identifier:" + drEraProviderAdjustment.Identifier + ",";
                                                drEraProviderAdjustment.ERAId = MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);
                                                ProviderAdjustmentStepFill.ERAId += MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);

                                                dsERA.ERAProviderAdjustments.AddERAProviderAdjustmentsRow(drEraProviderAdjustment);

                                                ProviderAdjustmentStepFill.Details += Environment.NewLine;
                                                ProviderAdjustmentStepFill.Details += "ProviderAdjustment has been Filled Successfully";
                                                ProviderAdjustmentStepFill.Status = true;
                                                ProviderAdjustmentStepFill.EndTime = DateTime.Now;
                                                #region Step #:10 ProviderAdjustmentStepInsert
                                                ERAActivitySteps ProviderAdjustmentStepInsert = new ERAActivitySteps("Step #:10", "Function call for Inserting Provider Adjustment", "Start Inserting Provider Adjustment", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(ProviderAdjustmentStepInsert);
                                                #endregion

                                                //Insert Provider Adjustment
                                                dsERA = new DALERA().InsertERAProviderAdjustments(dsERA, dbManager);//Step #:11
                                                ProviderAdjustmentStepInsert.Status = true;
                                                ProviderAdjustmentStepInsert.EndTime = DateTime.Now;
                                                ProviderAdjustmentStepInsert.Details += Environment.NewLine;
                                                ProviderAdjustmentStepInsert.Details += "ProviderAdjustment has been Inserted Successfully";
                                            }
                                            #endregion

                                            //dsCache.Table_BL_ERA_835_ClaimRow[] dsRowsClaimParser = (dsCache.Table_BL_ERA_835_ClaimRow[])ds835Parser.Table_BL_ERA_835_Claim.Select(ds835Parser.Table_BL_ERA_835_Claim.FIELD_CLAIM_ERA835_ID_FKColumn.ColumnName + "='" + ERA835Id + "'");
                                            //foreach (dsCache.Table_BL_ERA_835_ClaimRow drClaimParse in dsRowsClaimParser)
                                            //{
                                            #region Remittance Parser
                                            #region Step #:12 remitanceParserLoad
                                            ERAActivitySteps remitanceParserLoad = new ERAActivitySteps("Step #:12", "Loading RemitParser from ERA", "Start Loading RemitParser from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(remitanceParserLoad);
                                            #endregion
                                            dsCache.Table_BL_Remittance_CacheRow[] dsRowsRemitParser = (dsCache.Table_BL_Remittance_CacheRow[])ds835Parser.Table_BL_Remittance_Cache.Select(ds835Parser.Table_BL_Remittance_Cache.FIELD_REMIT_BATCH_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(ERA835Id));
                                            remitanceParserLoad.Details += Environment.NewLine;
                                            remitanceParserLoad.Details += "----------------Result--------------" + Environment.NewLine;
                                            remitanceParserLoad.Details += "RemitParser has been loaded Successfully";
                                            remitanceParserLoad.EndTime = DateTime.Now;
                                            remitanceParserLoad.Status = true;
                                            // Reversal of Payments 
                                            // 1- If there any Reversal payments in this check
                                            // 2- If Yes then adjust those charges relatively and then enter into system.
                                            // 3- If No there will be no change.
                                            #region Step #:13 getReversalPayments
                                            ERAActivitySteps getReversalPayments = new ERAActivitySteps("Step #:13", "Function call for Getting ReversalPayments from ERA", "Start getting ReversalPayments from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(getReversalPayments);
                                            #endregion
                                            dsRowsRemitParser = this.GetReversalPayments(dsRowsRemitParser, ref ds835Parser).ToArray();
                                            getReversalPayments.Details += Environment.NewLine;
                                            getReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                            getReversalPayments.Details += "ReversalPayments has been loaded Successfully";
                                            getReversalPayments.EndTime = DateTime.Now;
                                            getReversalPayments.Status = true;
                                            foreach (dsCache.Table_BL_Remittance_CacheRow drRemitParser in dsRowsRemitParser)
                                            {
                                                #region Step #:14 FillReversalPayments
                                                ERAActivitySteps FillReversalPayments = new ERAActivitySteps("Step #:14", "Filling ReversalPayments", "Filling ReversalPayments details and Values are: ", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(FillReversalPayments);
                                                #endregion

                                                //Fill ERA Detail
                                                #region Fill ERA Details
                                                DSERA dsERADetail = new DSERA();
                                                DSERA.ERADetailRow drEraDetail = dsERADetail.ERADetail.NewERADetailRow();

                                                drEraDetail.ERAId = MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);
                                                FillReversalPayments.Details += "ERAId:" + drEraDetail.ERAId + ", " + Environment.NewLine;
                                                drEraDetail.PostStatus = "UnPosted";
                                                FillReversalPayments.Details += "PostStatus:" + drEraDetail.PostStatus + ", " + Environment.NewLine;
                                                drEraDetail.PatFirstName = drRemitParser.FIELD_REMIT_PATIENT_FIRST_NAME;
                                                FillReversalPayments.Details += "PatFirstName:" + drEraDetail.PatFirstName + ", " + Environment.NewLine;
                                                drEraDetail.PatLastName = drRemitParser.FIELD_REMIT_PATIENT_LAST_NAME;
                                                FillReversalPayments.Details += "PatLastName:" + drEraDetail.PatLastName + ", " + Environment.NewLine;
                                                drEraDetail.SubFirstName = drRemitParser.FIELD_REMIT_SUBSCRIBER_FIRST_NAME;
                                                FillReversalPayments.Details += "SubFirstName:" + drEraDetail.SubFirstName + ", " + Environment.NewLine;
                                                drEraDetail.SubLastName = drRemitParser.FIELD_REMIT_SUBSCRIBER_LAST_NAME;
                                                FillReversalPayments.Details += "SubLastName:" + drEraDetail.SubLastName + ", " + Environment.NewLine;
                                                drEraDetail.MI = drRemitParser.FIELD_REMIT_PATIENT_MIDDLE_INITIAL;
                                                FillReversalPayments.Details += "MI:" + drEraDetail.MI + ", " + Environment.NewLine;
                                                //drEraDetail.PatDOB = drRemitParser;
                                                drEraDetail.ERAClaimNumber = drRemitParser.FIELD_REMIT_PATIENT_ACCOUNT_NO;
                                                FillReversalPayments.Details += "ERAClaimNumber:" + drEraDetail.ERAClaimNumber + ", " + Environment.NewLine;
                                                drEraDetail.ERAChargeNumber = drRemitParser.FIELD_REMIT_CHARGE_NUMBER;
                                                FillReversalPayments.Details += "ERAChargeNumber:" + drEraDetail.ERAChargeNumber + ", " + Environment.NewLine;
                                                drEraDetail.ICN = drRemitParser.FIELD_REMIT_ICN;
                                                FillReversalPayments.Details += "ICN:" + drEraDetail.ICN + ", " + Environment.NewLine;

                                                FillReversalPayments.Details += "SUBSCRIBER:" + drRemitParser.FIELD_REMIT_SUBSCRIBER_ID + ", " + Environment.NewLine;
                                                if (drRemitParser.FIELD_REMIT_SUBSCRIBER_ID == "SELF")
                                                {
                                                    drEraDetail.SubscriberId = drRemitParser.FIELD_REMIT_HICN;
                                                    FillReversalPayments.Details += "SubscriberId:" + drEraDetail.SubscriberId + ", " + Environment.NewLine;
                                                    drEraDetail.SubFirstName = drEraDetail.PatFirstName;
                                                    FillReversalPayments.Details += "SubFirstName:" + drEraDetail.SubFirstName + ", " + Environment.NewLine;
                                                    drEraDetail.SubLastName = drEraDetail.PatLastName;
                                                    FillReversalPayments.Details += "SubLastName:" + drEraDetail.SubLastName + ", " + Environment.NewLine;
                                                }
                                                else
                                                {
                                                    drEraDetail.SubscriberId = drRemitParser.FIELD_REMIT_SUBSCRIBER_ID;
                                                    FillReversalPayments.Details += "SubscriberId:" + drEraDetail.SubscriberId + ", " + Environment.NewLine;
                                                }


                                                if (!string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_SERVICE_FROM_DATE))
                                                {
                                                    drEraDetail.DOSFrom = MDVUtility.StringToDate(drRemitParser.FIELD_REMIT_SERVICE_FROM_DATE);
                                                    FillReversalPayments.Details += "DOSFrom:" + drEraDetail.DOSFrom + ", " + Environment.NewLine;
                                                }
                                                if (!string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_SERVICE_TO_DATE))
                                                {
                                                    drEraDetail.DOSTo = MDVUtility.StringToDate(drRemitParser.FIELD_REMIT_SERVICE_TO_DATE);
                                                    FillReversalPayments.Details += "DOSTo:" + drEraDetail.DOSTo + ", " + Environment.NewLine;
                                                }
                                                drEraDetail.CPTCode = drRemitParser.FIELD_REMIT_HCPCS;
                                                FillReversalPayments.Details += "CPTCode:" + drEraDetail.CPTCode + ", " + Environment.NewLine;
                                                drEraDetail.ModifierCode = drRemitParser.FIELD_REMIT_MODS;
                                                FillReversalPayments.Details += "ModifierCode:" + drEraDetail.ModifierCode + ", " + Environment.NewLine;
                                                drEraDetail.UnitsBilled = MDVUtility.ToDecimal(drRemitParser.FIELD_REMIT_SERVICE_QTY);
                                                FillReversalPayments.Details += "UnitsBilled:" + drEraDetail.UnitsBilled + ", " + Environment.NewLine;
                                                drEraDetail.POS = drRemitParser.FIELD_REMIT_SERVICE_POS;
                                                FillReversalPayments.Details += "POS:" + drEraDetail.POS + ", " + Environment.NewLine;
                                                drEraDetail.ProcessAs = drRemitParser.FIELD_REMIT_CLAIM_STATUS;
                                                FillReversalPayments.Details += "ProcessAs:" + drEraDetail.ProcessAs + ", " + Environment.NewLine;
                                                //drEraDetail.ClaimStatusCode = drRemitParser.FIELD_REMIT_CLAIM_STATUS_CODE;
                                                drEraDetail.SecondaryInsurance = drRemitParser.FIELD_REMIT_FORWARD_PAYER;
                                                FillReversalPayments.Details += "SecondaryInsurance:" + drEraDetail.SecondaryInsurance + ", " + Environment.NewLine;
                                                drEraDetail.SecondarySubscriberId = drRemitParser.FIELD_REMIT_SEC_SUBSCRIBER_ID;
                                                FillReversalPayments.Details += "SecondarySubscriberId:" + drEraDetail.SecondarySubscriberId + ", " + Environment.NewLine;
                                                drEraDetail.RemitCode = drRemitParser.FIELD_REMIT_HC_CODE;
                                                FillReversalPayments.Details += "RemitCode:" + drEraDetail.RemitCode + ", " + Environment.NewLine;

                                                if (string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_FORWARD_PAYER))
                                                    drEraDetail.IsCrossedOver = false;
                                                else
                                                    drEraDetail.IsCrossedOver = true;
                                                FillReversalPayments.Details += "IsCrossedOver:" + drEraDetail.IsCrossedOver + ", " + Environment.NewLine;
                                                drEraDetail.ChargedAmount = drRemitParser.IsFIELD_REMIT_CHARGE_AMOUNTNull() == false ? drRemitParser.FIELD_REMIT_CHARGE_AMOUNT : 0;
                                                FillReversalPayments.Details += "ChargedAmount:" + drEraDetail.ChargedAmount + ", " + Environment.NewLine;
                                                drEraDetail.AllowedAmount = drRemitParser.IsFIELD_REMIT_ALLOWEDNull() == false ? drRemitParser.FIELD_REMIT_ALLOWED : 0;
                                                FillReversalPayments.Details += "AllowedAmount:" + drEraDetail.AllowedAmount + ", " + Environment.NewLine;
                                                drEraDetail.PaidAmount = drRemitParser.IsFIELD_REMIT_PAID_AMOUNTNull() == false ? drRemitParser.FIELD_REMIT_PAID_AMOUNT : 0;
                                                FillReversalPayments.Details += "PaidAmount:" + drEraDetail.PaidAmount + ", " + Environment.NewLine;
                                                drEraDetail.CoInsuranceAmount = drRemitParser.IsFIELD_REMIT_COINSURANCENull() == false ? drRemitParser.FIELD_REMIT_COINSURANCE : 0;
                                                FillReversalPayments.Details += "CoInsuranceAmount:" + drEraDetail.CoInsuranceAmount + ", " + Environment.NewLine;
                                                drEraDetail.DeductableAmount = drRemitParser.IsFIELD_REMIT_DEDUCTIBLENull() == false ? drRemitParser.FIELD_REMIT_DEDUCTIBLE : 0;
                                                FillReversalPayments.Details += "DeductableAmount:" + drEraDetail.DeductableAmount + ", " + Environment.NewLine;
                                                drEraDetail.Copayment = drRemitParser.IsFIELD_REMIT_COPAYMENTNull() == false ? drRemitParser.FIELD_REMIT_COPAYMENT : 0;
                                                FillReversalPayments.Details += "Copayment:" + drEraDetail.Copayment + ", " + Environment.NewLine;
                                                drEraDetail.LateFilingCharges = drRemitParser.IsFIELD_REMIT_LATE_FILING_CHARGENull() == false ? drRemitParser.FIELD_REMIT_LATE_FILING_CHARGE : 0;
                                                FillReversalPayments.Details += "LateFilingCharges:" + drEraDetail.LateFilingCharges + ", " + Environment.NewLine;
                                                if (MDVUtility.IsDateTime(drRemitParser.FIELD_REMIT_CLAIM_FROM_DATE))
                                                {
                                                    drEraDetail.VisitDate = Convert.ToDateTime(drRemitParser.FIELD_REMIT_CLAIM_FROM_DATE);
                                                    FillReversalPayments.Details += "VisitDate:" + drEraDetail.VisitDate + ", " + Environment.NewLine;
                                                }

                                                drEraDetail.IsActive = true;
                                                FillReversalPayments.Details += "DeductableAmount:" + drEraDetail.DeductableAmount + ", " + Environment.NewLine;
                                                drEraDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                                drEraDetail.CreatedOn = DateTime.Now;
                                                drEraDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                                drEraDetail.ModifiedOn = DateTime.Now;
                                                //WriteOff and PatientResponsibility
                                                #region Step #:15 calculateWriteOfPatientResponsibility
                                                ERAActivitySteps calculateWriteOfPatientResponsibility = new ERAActivitySteps("Step #:15", "Calculating WriteOff and PatientResponsibility", "Start Calculating WriteOff and PatientResponsibility", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(calculateWriteOfPatientResponsibility);
                                                #endregion
                                                #region WriteOff and PatientResponsibility
                                                dsCache.Table_BL_Remittance_WriteOffRow[] dsWriteOff = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(drRemitParser.FIELD_REMIT_BL_REMITTANCE_ID_PK));
                                                decimal WriteOff = 0;
                                                decimal PatientResponsiblity = 0;
                                                foreach (dsCache.Table_BL_Remittance_WriteOffRow drWriteOff in dsWriteOff)
                                                {
                                                    decimal tempWriteOff = MDVUtility.ToDecimal(drWriteOff.FIELD_WRITE_OFF);
                                                    string tempGroupCode = MDVUtility.ToStr(drWriteOff.FIELD_WRITE_OFF_GROUP_CODE);
                                                    int tempResionCode = MDVUtility.ToInt(drWriteOff.FIELD_WRITE_OFF_REASON_CODE);

                                                    // ignore OA 23 write off amount.
                                                    if (tempGroupCode == "OA" && tempResionCode == 23 /*&& drEraDetail.ProcessAs == "Processed as Secondary"*/)
                                                        tempWriteOff = 0;

                                                    if ((tempGroupCode == "PR" || tempGroupCode == "OA") && tempResionCode >= 4)
                                                    {
                                                        PatientResponsiblity += tempWriteOff;
                                                    }
                                                    else if (tempGroupCode != "PR" && tempResionCode >= 4)
                                                    {
                                                        WriteOff += tempWriteOff;
                                                    }

                                                }
                                                #endregion
                                                calculateWriteOfPatientResponsibility.Details += Environment.NewLine;
                                                calculateWriteOfPatientResponsibility.Details += "----------------Result--------------" + Environment.NewLine;
                                                calculateWriteOfPatientResponsibility.Details += "WriteOff and PatientResponsibility has been calculated Successfully and WriteOff:" + WriteOff;
                                                calculateWriteOfPatientResponsibility.EndTime = DateTime.Now;
                                                calculateWriteOfPatientResponsibility.Status = true;
                                                drEraDetail.WriteOff = WriteOff;
                                                FillReversalPayments.Details += "WriteOff:" + drEraDetail.WriteOff + ", " + Environment.NewLine;
                                                drEraDetail.PatientResponsibility = PatientResponsiblity;
                                                FillReversalPayments.Details += "PatientResponsibility:" + drEraDetail.PatientResponsibility + ", " + Environment.NewLine;
                                                dsERADetail.ERADetail.AddERADetailRow(drEraDetail);
                                                #endregion
                                                FillReversalPayments.Details += Environment.NewLine;
                                                FillReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                                FillReversalPayments.Details += "ReversalPayments has been filled Successfully";
                                                FillReversalPayments.EndTime = DateTime.Now;
                                                FillReversalPayments.Status = true;
                                                //Insert ERA Detail
                                                #region Step #:16 insertReversalPayments
                                                ERAActivitySteps insertReversalPayments = new ERAActivitySteps("Step #:16", "Function call for Inserting ReversalPayments", "Start Inserting ReversalPayments", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(insertReversalPayments);
                                                #endregion
                                                dsERADetail = new DALERA().InsertERADetail(dsERADetail, dbManager);

                                                insertReversalPayments.Details += Environment.NewLine;
                                                insertReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                                insertReversalPayments.Details += "ReversalPayments has been Inserted Successfully and EraDetailId=" + drEraDetail.ERADtlId;
                                                insertReversalPayments.ERADetailsId = drEraDetail.ERADtlId;
                                                insertReversalPayments.EndTime = DateTime.Now;
                                                insertReversalPayments.Status = true;
                                                if (dsERADetail.ERADetail.Rows.Count > 0)
                                                {
                                                    #region Remittance_WriteOff
                                                    dsCache.Table_BL_Remittance_WriteOffRow[] dsWriteOffParse = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(drRemitParser.FIELD_REMIT_BL_REMITTANCE_ID_PK));
                                                    foreach (dsCache.Table_BL_Remittance_WriteOffRow drWriteOffParse in dsWriteOffParse)
                                                    {
                                                        #region Step #:18 fillERAClaimAdjustmentCodeStep
                                                        ERAActivitySteps fillERAClaimAdjustmentCodeStep = new ERAActivitySteps("Step #:18", "Filling ERA Claim Adjustment Code", "Start Filling ERA Claim Adjustment Code and values are:", drEra.ERAId, drEraDetail.ERADtlId, drReport.EDIReportId);
                                                        ActivityStepList.Add(fillERAClaimAdjustmentCodeStep);
                                                        #endregion
                                                        //Fill ERA ClaimAdjustmentCode
                                                        DSERA.ERAClaimAdjustmentCodeRow drERAClaimAdjustmentCode = dsERADetail.ERAClaimAdjustmentCode.NewERAClaimAdjustmentCodeRow();

                                                        drERAClaimAdjustmentCode.ERADtlId = MDVUtility.ToLong(dsERADetail.ERADetail.Rows[0][dsERADetail.ERADetail.ERADtlIdColumn]);
                                                        fillERAClaimAdjustmentCodeStep.Details += " ERADtlId:" + drERAClaimAdjustmentCode.ERADtlId + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.AdjGroupCode = drWriteOffParse.FIELD_WRITE_OFF_GROUP_CODE;
                                                        fillERAClaimAdjustmentCodeStep.Details += "AdjGroupCode:" + drERAClaimAdjustmentCode.AdjGroupCode + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.AdjReasonCode = drWriteOffParse.FIELD_WRITE_OFF_REASON_CODE;
                                                        fillERAClaimAdjustmentCodeStep.Details += "AdjReasonCode:" + drERAClaimAdjustmentCode.AdjReasonCode + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.Amount = MDVUtility.ToDecimal(drWriteOffParse.FIELD_WRITE_OFF);
                                                        fillERAClaimAdjustmentCodeStep.Details += "Amount:" + drERAClaimAdjustmentCode.Amount + ", " + Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += "----------------Result--------------" + Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += "ERA Claim Adjustment Code has been Filled Successfully";
                                                        fillERAClaimAdjustmentCodeStep.EndTime = DateTime.Now;
                                                        fillERAClaimAdjustmentCodeStep.Status = true;
                                                        //Insert ERA Claim Adjustment Code
                                                        #region Step #:19 insertERAClaimAdjustmentCodeStep
                                                        ERAActivitySteps insertERAClaimAdjustmentCodeStep = new ERAActivitySteps("Step #:19", "Function call for Inserting ERA Claim Adjustment Code", "Start Inserting ERA Claim Adjustment Code", drEra.ERAId, drEraDetail.ERADtlId, drReport.EDIReportId);
                                                        ActivityStepList.Add(insertERAClaimAdjustmentCodeStep);
                                                        #endregion

                                                        dsERADetail.ERAClaimAdjustmentCode.AddERAClaimAdjustmentCodeRow(drERAClaimAdjustmentCode);
                                                        dsERADetail = new DALERA().InsertERAClaimAdjustmentCode(dsERADetail, dbManager);
                                                        insertERAClaimAdjustmentCodeStep.Details += Environment.NewLine;
                                                        insertERAClaimAdjustmentCodeStep.Details += "----------------Result--------------" + Environment.NewLine;
                                                        insertERAClaimAdjustmentCodeStep.Details += "ERA Claim Adjustment Code has been Inserted Successfully";
                                                        insertERAClaimAdjustmentCodeStep.Status = true;
                                                        insertERAClaimAdjustmentCodeStep.EndTime = DateTime.Now;
                                                    }
                                                    #endregion
                                                }
                                            }
                                            #endregion
                                            //}
                                        }
                                    }
                                    #endregion
                                    DBInsertionParsedData.Details += Environment.NewLine;
                                    DBInsertionParsedData.Details += "----------------Result--------------" + Environment.NewLine;
                                    DBInsertionParsedData.Details += "Parsed data has been successfully Inserted in DB";
                                    drReport.IsParse = "P";
                                    DBInsertionParsedData.EndTime = DateTime.Now;
                                    DBInsertionParsedData.Status = true;
                                    //Comment Transaction
                                    dbManager.CommitTransaction();
                                }
                                catch (Exception ex)
                                {
                                    drReport.IsParse = "F";
                                    drReport.Comments = ex.Message;
                                    //Set the details of last ERAStep on which the exception has been occurred
                                    int lineNumber = 0;
                                    const string lineSearch = ":line ";
                                    var index = ex.StackTrace.LastIndexOf(lineSearch);
                                    if (index != -1)
                                    {
                                        var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                                        int.TryParse(lineNumberText, out lineNumber);
                                    }
                                    if (ActivityStepList.Count > 0)
                                    {
                                        ERAActivitySteps step = ActivityStepList.Last();
                                        step.Status = false;
                                        step.EndTime = DateTime.Now;
                                        step.Details += "----------------Result--------------" + Environment.NewLine;
                                        step.Details += ex.Message + " on Line #:" + lineNumber;
                                    }
                                    //RollBack Transaction
                                    dbManager.RollBackTransaction();
                                }
                                finally
                                {
                                    //Dispose dbManager
                                    dbManager.Dispose();
                                }

                            }
                            else
                            {
                                drReport.IsParse = "F";
                                drReport.Comments = "Could not Parse this file.";
                                ParsingLoadedReportStep.Details += Environment.NewLine;
                                ParsingLoadedReportStep.Details += "----------------Result--------------" + Environment.NewLine;
                                ParsingLoadedReportStep.Details += "Could not Parse this file.";
                                ParsingLoadedReportStep.Status = false;
                                ParsingLoadedReportStep.EndTime = DateTime.Now;
                            }

                        }
                        catch (Exception ex)
                        {
                            drReport.IsParse = "F";
                            drReport.Comments = ex.Message;
                            //Set the details of last ERAStep on which the exception has been occurred
                            int lineNumber = 0;
                            const string lineSearch = ":line ";
                            var index = ex.StackTrace.LastIndexOf(lineSearch);
                            if (index != -1)
                            {
                                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                                int.TryParse(lineNumberText, out lineNumber);
                            }
                            if (ActivityStepList.Count > 0)
                            {
                                ERAActivitySteps step = ActivityStepList.Last();
                                step.Status = false;
                                step.EndTime = DateTime.Now;
                                step.Details += "----------------Result--------------" + Environment.NewLine;
                                step.Details += ex.Message + " on Line #:" + lineNumber;
                            }
                            //return new BLObject<bool>(false, ex.Message);
                        }
                    }

                    //Update Report Status that which EDI File is Parsed and UnParsed
                    new DALEDIReports().UpdateEDIReports(dsReoprt);
                    dsReoprt.AcceptChanges();
                    return new BLObject<bool>(true);

                }
                else
                {
                    LoadReportStep.Details += Environment.NewLine;
                    LoadReportStep.Details += "----------------Result--------------" + Environment.NewLine;
                    LoadReportStep.Details += "Could not find any new report to download.";
                    LoadReportStep.EndTime = DateTime.Now;
                    LoadReportStep.Status = false;
                    return new BLObject<bool>(false, "Could not find any new report to download.");
                }


            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLERA::DownloadERA", ex);
                int lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    int.TryParse(lineNumberText, out lineNumber);
                }
                if (ActivityStepList.Count > 0)
                {
                    ERAActivitySteps step = ActivityStepList.Last();
                    step.Status = false;
                    step.EndTime = DateTime.Now;
                    step.Details += "----------------Result--------------" + Environment.NewLine;
                    step.Details += ex.Message + " on Line #:" + lineNumber;
                }
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                string xml = ERAActivityStepsLogger.MaintainERALogging(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ActivityStepList, 1);
                if (!string.IsNullOrEmpty(xml))
                {
                    string returnVal = new DALERA().InsertERALog(xml);
                }
            }
        }
        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="EDIReportId"></param>
        /// <returns></returns>
        public BLObject<bool> DownloadERA(SharedVariable SharedVariable, long EDIReportId = 0)
        {
            List<ERAActivitySteps> ActivityStepList = new List<ERAActivitySteps>();
            try
            {
                #region Step #:1 LoadReportStep
                ERAActivitySteps LoadReportStep = new ERAActivitySteps("Step #:1", "Function call to Load EDI Reports", "Start Loading EDI report with Perameter 'EDIReportId:" + EDIReportId + "'", 0, 0, EDIReportId);
                ActivityStepList.Add(LoadReportStep);
                #endregion
                DSEDIReports dsReoprt = new DSEDIReports();
                if (EDIReportId == 0)
                    dsReoprt = new DALEDIReports(SharedVariable).LoadEDIReports(SharedVariable, 0, 0, "", "", "835", "", "", null, "");
                else
                    dsReoprt = new DALEDIReports(SharedVariable).LoadEDIReports(SharedVariable, EDIReportId, 0, "", "", "835", "", "", null, null);

                if (dsReoprt.EDIReports.Rows.Count > 0)
                {
                    LoadReportStep.EndTime = DateTime.Now;
                    LoadReportStep.Details += "Sucessfully Loaded EDI Reports with EDIReports Rows:" + dsReoprt.EDIReports.Rows.Count;
                    LoadReportStep.Status = true;
                    foreach (DSEDIReports.EDIReportsRow drReport in dsReoprt.EDIReports.Rows)
                    {
                        try
                        {
                            LoadReportStep.EDIReportId = drReport.EDIReportId;
                            dsCache ds835Parser = new dsCache();
                            #region Step #:3 ParsingLoadedReportStep
                            ERAActivitySteps ParsingLoadedReportStep = new ERAActivitySteps("Step #:3", "Parse the loaded EDI File", "Start of parsing with the perameters:'EDIText:" + drReport.EDIText + "EDIReportId:" + drReport.EDIReportId + "FileName:" + drReport.FileName + "UserName:" + MDVUtility.DecryptFrom64(SharedVariable.UserName) + "EDIReportId:" + drReport.EDIReportId + "'", 0, 0, drReport.EDIReportId);
                            ActivityStepList.Add(ParsingLoadedReportStep);
                            #endregion
                            string str = EDICommon.ParseEDI(drReport.EDIText, drReport.EDIReportId, drReport.FileName, ref ds835Parser, MDVUtility.DecryptFrom64(SharedVariable.UserName));

                            if (ds835Parser.Table_BL_ERA_835.Rows.Count > 0)
                            {
                                ParsingLoadedReportStep.Details += "EDI file has been parsed in '" + ds835Parser.Table_BL_ERA_835.Rows.Count + "' rows";
                                ParsingLoadedReportStep.EndTime = DateTime.Now;
                                ParsingLoadedReportStep.Status = true;
                                IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

                                //Begin Transaction
                                dbManager.BeginTransaction();
                                try
                                {
                                    #region Step #:4 DBInsertionParsedData
                                    ERAActivitySteps DBInsertionParsedData = new ERAActivitySteps("Step #:4", "Insert Parsed Data in ERA, ERADetail, ClaimAdjustmentCode Tables", "Start inserting Parsed data in ERA, ERADetail, ClaimAdjustmentCode Tables", 0, 0, drReport.EDIReportId);
                                    ActivityStepList.Add(DBInsertionParsedData);
                                    #endregion
                                    #region Insert Parsed Data in DB
                                    foreach (dsCache.Table_BL_ERA_835Row dr835Parse in ds835Parser.Table_BL_ERA_835.Rows)
                                    {
                                        #region Step #:5 FillERAStep
                                        ERAActivitySteps FillERAStep = new ERAActivitySteps("Step #:5", "Fill ERA object", "Start Filling ERA object and Values are: ", 0, 0, drReport.EDIReportId);
                                        ActivityStepList.Add(FillERAStep);
                                        #endregion
                                        #region FillERA
                                        DSERA dsERA = new DSERA();
                                        DSERA.ERARow drEra = dsERA.ERA.NewERARow();

                                        drEra.ClearingHouseId = drReport.ClearingHouseId;
                                        FillERAStep.Details += "ClearingHouseId: " + drEra.ClearingHouseId + ", " + Environment.NewLine;
                                        drEra.EDIReportId = drReport.EDIReportId;
                                        FillERAStep.Details += "EDIReportId: " + drEra.EDIReportId + ", " + Environment.NewLine;
                                        drEra.Status = "UnPosted";
                                        FillERAStep.Details += "Status: " + drEra.Status + ", " + Environment.NewLine;
                                        drEra.CheckNo = dr835Parse.FIELD_ERA835_CHEQUE_NO;
                                        FillERAStep.Details += "CheckNo: " + drEra.CheckNo + ", " + Environment.NewLine;
                                        drEra.CheckAmount = MDVUtility.ToDecimal(dr835Parse.FIELD_ERA835_CHECK_EFT_AMOUNT);
                                        FillERAStep.Details += "CheckAmount: " + drEra.CheckAmount + ", " + Environment.NewLine;
                                        if (MDVUtility.IsDateTime(dr835Parse.FIELD_ERA835_CHECK_DATE))
                                        {
                                            drEra.CheckDate = Convert.ToDateTime(dr835Parse.FIELD_ERA835_CHECK_DATE);
                                            FillERAStep.Details += "CheckDate: " + drEra.CheckDate + ", " + Environment.NewLine;
                                        }
                                        //drEra.CheckDepositDate = dr835Parse;
                                        drEra.PayerName = dr835Parse.FIELD_ERA835_PAYER_NAME;
                                        FillERAStep.Details += "PayerName: " + drEra.PayerName + ", " + Environment.NewLine;
                                        drEra.ClaimPayerId = dr835Parse.FIELD_ERA835_PAYER_ID;
                                        FillERAStep.Details += "ClaimPayerId: " + drEra.ClaimPayerId + ", " + Environment.NewLine;
                                        drEra.PayerAddress = dr835Parse.FIELD_ERA835_PAYER_ADDRESS;
                                        FillERAStep.Details += "PayerAddress: " + drEra.PayerAddress + ", " + Environment.NewLine;
                                        drEra.PayerCity = dr835Parse.FIELD_ERA835_PAYER_CITY;
                                        FillERAStep.Details += "PayerCity: " + drEra.PayerCity + ", " + Environment.NewLine;
                                        drEra.PayerState = dr835Parse.FIELD_ERA835_PAYER_STATE;
                                        FillERAStep.Details += "PayerState: " + drEra.PayerState + ", " + Environment.NewLine;
                                        drEra.PayerZipCode = dr835Parse.FIELD_ERA835_PAYER_POSTAL_CODE;
                                        FillERAStep.Details += "PayerZipCode: " + drEra.PayerZipCode + ", " + Environment.NewLine;
                                        drEra.PayeeName = dr835Parse.FIELD_ERA835_PAYEE_NAME;
                                        FillERAStep.Details += "PayeeName: " + drEra.PayeeName + ", " + Environment.NewLine;
                                        drEra.PayeeAddress = dr835Parse.FIELD_ERA835_PAYEE_ADDRESS;
                                        FillERAStep.Details += "PayeeAddress: " + drEra.PayeeAddress + ", " + Environment.NewLine;
                                        drEra.PayeeCity = dr835Parse.FIELD_ERA835_PAYEE_CITY;
                                        FillERAStep.Details += "PayeeCity: " + drEra.PayeeCity + ", " + Environment.NewLine;
                                        drEra.PayeeState = dr835Parse.FIELD_ERA835_PAYEE_STATE;
                                        FillERAStep.Details += "PayeeState: " + drEra.PayeeState + ", " + Environment.NewLine;
                                        drEra.PayeeZipCode = dr835Parse.FIELD_ERA835_PAYEE_POSTAL_CODE;
                                        FillERAStep.Details += "PayeeZipCode: " + drEra.PayeeZipCode + ", " + Environment.NewLine;
                                        drEra.IsActive = true;
                                        FillERAStep.Details += "IsActive: " + drEra.IsActive + ",";
                                        drEra.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        drEra.CreatedOn = DateTime.Now;
                                        drEra.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                        drEra.ModifiedOn = DateTime.Now;
                                        dsERA.ERA.AddERARow(drEra);
                                        FillERAStep.EndTime = DateTime.Now;
                                        #endregion

                                        FillERAStep.Details += Environment.NewLine;
                                        FillERAStep.Details += "----------------Result--------------" + Environment.NewLine;
                                        FillERAStep.Details += "ERA has been Filled Successfully";
                                        FillERAStep.Status = true;
                                        //Insert ERA
                                        #region Step #:6 InsertERAStep
                                        ERAActivitySteps InsertERAStep = new ERAActivitySteps("Step #:6", "Inserting ERA in DB", "Start Inserting ERA in DB", drEra.ERAId, 0, drReport.EDIReportId);
                                        ActivityStepList.Add(InsertERAStep);
                                        #endregion
                                        dsERA = new DALERA(SharedVariable).InsertERA(SharedVariable, dsERA, dbManager);
                                        InsertERAStep.Details += Environment.NewLine;
                                        InsertERAStep.Details += "----------------Result--------------" + Environment.NewLine;
                                        InsertERAStep.Details += "ERA has been Inserted Successfully. Inserted ERAId:" + drEra.ERAId;
                                        InsertERAStep.ERAId = drEra.ERAId;
                                        InsertERAStep.Status = true;
                                        InsertERAStep.EndTime = DateTime.Now;
                                        if (dsERA.ERA.Rows.Count > 0)
                                        {

                                            string ERA835Id = dr835Parse.FIELD_ERA835_ID_PK.ToString();
                                            #region Provider_Adjustment
                                            #region Step #:8 ProviderAdjustmentStepLoad
                                            ERAActivitySteps ProviderAdjustmentStepLoad = new ERAActivitySteps("Step #:8", "Loading ProviderAdjustment from ERA", "Start Loading ProviderAdjustment from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(ProviderAdjustmentStepLoad);
                                            #endregion
                                            dsCache.Table_BL_Provider_AdjustmentRow[] dsRowsProviderAdjustmentsParser = (dsCache.Table_BL_Provider_AdjustmentRow[])ds835Parser.Table_BL_Provider_Adjustment.Select(ds835Parser.Table_BL_Provider_Adjustment.FIELD_REMIT_BATCH_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(ERA835Id));
                                            ProviderAdjustmentStepLoad.Details += Environment.NewLine;
                                            ProviderAdjustmentStepLoad.Details += "----------------Result--------------" + Environment.NewLine;
                                            ProviderAdjustmentStepLoad.Details += "ProviderAdjustment has been loaded Successfully";
                                            ProviderAdjustmentStepLoad.Status = true;
                                            ProviderAdjustmentStepLoad.EndTime = DateTime.Now;
                                            foreach (dsCache.Table_BL_Provider_AdjustmentRow drProviderAdjParser in dsRowsProviderAdjustmentsParser)
                                            {
                                                #region Step #:9 ProviderAdjustmentStepFill
                                                ERAActivitySteps ProviderAdjustmentStepFill = new ERAActivitySteps("Step #:9", "Filling Provider Adjustment", "Start Filling Provider Adjustment", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(ProviderAdjustmentStepFill);
                                                #endregion
                                                //Fill Provider Adjustment
                                                DSERA.ERAProviderAdjustmentsRow drEraProviderAdjustment = dsERA.ERAProviderAdjustments.NewERAProviderAdjustmentsRow();
                                                drEraProviderAdjustment.ReasonCode = drProviderAdjParser.FIELD_PLB_REASON_CODE;
                                                ProviderAdjustmentStepFill.Details += " Details of Provider Adjustment is as. ReasonCode:" + drEraProviderAdjustment.ReasonCode + ",";
                                                drEraProviderAdjustment.Description = drProviderAdjParser.FIELD_PLB_DESCRIPTION;
                                                ProviderAdjustmentStepFill.Details += " Description:" + drEraProviderAdjustment.Description + ",";
                                                drEraProviderAdjustment.Amount = drProviderAdjParser.FIELD_PLB_AMOUNT;
                                                ProviderAdjustmentStepFill.Details += " Amount:" + drEraProviderAdjustment.Amount + ",";
                                                drEraProviderAdjustment.Identifier = drProviderAdjParser.FIELD_PLB_REFRENCE_IDENTIFIER;
                                                ProviderAdjustmentStepFill.Details += " Identifier:" + drEraProviderAdjustment.Identifier + ",";
                                                drEraProviderAdjustment.ERAId = MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);
                                                ProviderAdjustmentStepFill.ERAId += MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);

                                                dsERA.ERAProviderAdjustments.AddERAProviderAdjustmentsRow(drEraProviderAdjustment);

                                                ProviderAdjustmentStepFill.Details += Environment.NewLine;
                                                ProviderAdjustmentStepFill.Details += "ProviderAdjustment has been Filled Successfully";
                                                ProviderAdjustmentStepFill.Status = true;
                                                ProviderAdjustmentStepFill.EndTime = DateTime.Now;
                                                #region Step #:10 ProviderAdjustmentStepInsert
                                                ERAActivitySteps ProviderAdjustmentStepInsert = new ERAActivitySteps("Step #:10", "Inserting Provider Adjustment", "Start Inserting Provider Adjustment", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(ProviderAdjustmentStepInsert);
                                                #endregion
                                                //Insert Provider Adjustment
                                                dsERA = new DALERA(SharedVariable).InsertERAProviderAdjustments(SharedVariable, dsERA, dbManager);
                                                ProviderAdjustmentStepInsert.Status = true;
                                                ProviderAdjustmentStepInsert.EndTime = DateTime.Now;
                                                ProviderAdjustmentStepInsert.Details += Environment.NewLine;
                                                ProviderAdjustmentStepInsert.Details += "ProviderAdjustment has been Inserted Successfully";
                                            }
                                            #endregion
                                            //dsCache.Table_BL_ERA_835_ClaimRow[] dsRowsClaimParser = (dsCache.Table_BL_ERA_835_ClaimRow[])ds835Parser.Table_BL_ERA_835_Claim.Select(ds835Parser.Table_BL_ERA_835_Claim.FIELD_CLAIM_ERA835_ID_FKColumn.ColumnName + "='" + ERA835Id + "'");
                                            //foreach (dsCache.Table_BL_ERA_835_ClaimRow drClaimParse in dsRowsClaimParser)
                                            //{
                                            #region Remittance Parser
                                            #region Step #:12 remitanceParserLoad
                                            ERAActivitySteps remitanceParserLoad = new ERAActivitySteps("Step #:12", "Loading RemitParser from ERA", "Start Loading RemitParser from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(remitanceParserLoad);
                                            #endregion
                                            dsCache.Table_BL_Remittance_CacheRow[] dsRowsRemitParser = (dsCache.Table_BL_Remittance_CacheRow[])ds835Parser.Table_BL_Remittance_Cache.Select(ds835Parser.Table_BL_Remittance_Cache.FIELD_REMIT_BATCH_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(ERA835Id));
                                            remitanceParserLoad.Details += Environment.NewLine;
                                            remitanceParserLoad.Details += "----------------Result--------------" + Environment.NewLine;
                                            remitanceParserLoad.Details += "RemitParser has been loaded Successfully";
                                            remitanceParserLoad.EndTime = DateTime.Now;
                                            remitanceParserLoad.Status = true;
                                            // Reversal of Payments 
                                            // 1- If there any Reversal payments in this check
                                            // 2- If Yes then adjust those charges relatively and then enter into system.
                                            // 3- If No there will be no change.
                                            #region Step #:13 getReversalPayments
                                            ERAActivitySteps getReversalPayments = new ERAActivitySteps("Step #:13", "Getting ReversalPayments from ERA", "Start getting ReversalPayments from ERA", drEra.ERAId, 0, drReport.EDIReportId);
                                            ActivityStepList.Add(getReversalPayments);
                                            #endregion
                                            dsRowsRemitParser = this.GetReversalPayments(dsRowsRemitParser, ref ds835Parser).ToArray();
                                            getReversalPayments.Details += Environment.NewLine;
                                            getReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                            getReversalPayments.Details += "ReversalPayments has been loaded Successfully";
                                            getReversalPayments.EndTime = DateTime.Now;
                                            getReversalPayments.Status = true;
                                            foreach (dsCache.Table_BL_Remittance_CacheRow drRemitParser in dsRowsRemitParser)
                                            {
                                                #region Step #:14 FillReversalPayments
                                                ERAActivitySteps FillReversalPayments = new ERAActivitySteps("Step #:14", "Filling ReversalPayments", "Filling ReversalPayments details and Values are: ", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(FillReversalPayments);
                                                #endregion
                                                //Fill ERA Detail
                                                #region Fill ERA Details
                                                DSERA dsERADetail = new DSERA();
                                                DSERA.ERADetailRow drEraDetail = dsERADetail.ERADetail.NewERADetailRow();

                                                drEraDetail.ERAId = MDVUtility.ToLong(dsERA.ERA.Rows[0][dsERA.ERA.ERAIdColumn]);
                                                FillReversalPayments.Details += "ERAId:" + drEraDetail.ERAId + ", " + Environment.NewLine;
                                                drEraDetail.PostStatus = "UnPosted";
                                                FillReversalPayments.Details += "PostStatus:" + drEraDetail.PostStatus + ", " + Environment.NewLine;
                                                drEraDetail.PatFirstName = drRemitParser.FIELD_REMIT_PATIENT_FIRST_NAME;
                                                FillReversalPayments.Details += "PatFirstName:" + drEraDetail.PatFirstName + ", " + Environment.NewLine;
                                                drEraDetail.PatLastName = drRemitParser.FIELD_REMIT_PATIENT_LAST_NAME;
                                                FillReversalPayments.Details += "PatLastName:" + drEraDetail.PatLastName + ", " + Environment.NewLine;
                                                drEraDetail.SubFirstName = drRemitParser.FIELD_REMIT_SUBSCRIBER_FIRST_NAME;
                                                FillReversalPayments.Details += "SubFirstName:" + drEraDetail.SubFirstName + ", " + Environment.NewLine;
                                                drEraDetail.SubLastName = drRemitParser.FIELD_REMIT_SUBSCRIBER_LAST_NAME;
                                                FillReversalPayments.Details += "SubLastName:" + drEraDetail.SubLastName + ", " + Environment.NewLine;
                                                drEraDetail.MI = drRemitParser.FIELD_REMIT_PATIENT_MIDDLE_INITIAL;
                                                FillReversalPayments.Details += "MI:" + drEraDetail.MI + ", " + Environment.NewLine;
                                                //drEraDetail.PatDOB = drRemitParser;
                                                drEraDetail.ERAClaimNumber = drRemitParser.FIELD_REMIT_PATIENT_ACCOUNT_NO;
                                                FillReversalPayments.Details += "ERAClaimNumber:" + drEraDetail.ERAClaimNumber + ", " + Environment.NewLine;
                                                drEraDetail.ERAChargeNumber = drRemitParser.FIELD_REMIT_CHARGE_NUMBER;
                                                FillReversalPayments.Details += "ERAChargeNumber:" + drEraDetail.ERAChargeNumber + ", " + Environment.NewLine;
                                                drEraDetail.ICN = drRemitParser.FIELD_REMIT_ICN;
                                                FillReversalPayments.Details += "ICN:" + drEraDetail.ICN + ", " + Environment.NewLine;

                                                FillReversalPayments.Details += "SUBSCRIBER:" + drRemitParser.FIELD_REMIT_SUBSCRIBER_ID + ", " + Environment.NewLine;
                                                if (drRemitParser.FIELD_REMIT_SUBSCRIBER_ID == "SELF")
                                                {
                                                    drEraDetail.SubscriberId = drRemitParser.FIELD_REMIT_HICN;
                                                    FillReversalPayments.Details += "SubscriberId:" + drEraDetail.SubscriberId + ", " + Environment.NewLine;
                                                    drEraDetail.SubFirstName = drEraDetail.PatFirstName;
                                                    FillReversalPayments.Details += "SubFirstName:" + drEraDetail.SubFirstName + ", " + Environment.NewLine;
                                                    drEraDetail.SubLastName = drEraDetail.PatLastName;
                                                    FillReversalPayments.Details += "SubLastName:" + drEraDetail.SubLastName + ", " + Environment.NewLine;
                                                }
                                                else
                                                {
                                                    drEraDetail.SubscriberId = drRemitParser.FIELD_REMIT_SUBSCRIBER_ID;
                                                    FillReversalPayments.Details += "SubscriberId:" + drEraDetail.SubscriberId + ", " + Environment.NewLine;
                                                }


                                                if (!string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_SERVICE_FROM_DATE))
                                                {
                                                    drEraDetail.DOSFrom = MDVUtility.StringToDate(drRemitParser.FIELD_REMIT_SERVICE_FROM_DATE);
                                                    FillReversalPayments.Details += "DOSFrom:" + drEraDetail.DOSFrom + ", " + Environment.NewLine;
                                                }
                                                if (!string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_SERVICE_TO_DATE))
                                                {
                                                    drEraDetail.DOSTo = MDVUtility.StringToDate(drRemitParser.FIELD_REMIT_SERVICE_TO_DATE);
                                                    FillReversalPayments.Details += "DOSTo:" + drEraDetail.DOSTo + ", " + Environment.NewLine;
                                                }
                                                drEraDetail.CPTCode = drRemitParser.FIELD_REMIT_HCPCS;
                                                FillReversalPayments.Details += "CPTCode:" + drEraDetail.CPTCode + ", " + Environment.NewLine;
                                                drEraDetail.ModifierCode = drRemitParser.FIELD_REMIT_MODS;
                                                FillReversalPayments.Details += "ModifierCode:" + drEraDetail.ModifierCode + ", " + Environment.NewLine;
                                                drEraDetail.UnitsBilled = MDVUtility.ToDecimal(drRemitParser.FIELD_REMIT_SERVICE_QTY);
                                                FillReversalPayments.Details += "UnitsBilled:" + drEraDetail.UnitsBilled + ", " + Environment.NewLine;
                                                drEraDetail.POS = drRemitParser.FIELD_REMIT_SERVICE_POS;
                                                FillReversalPayments.Details += "POS:" + drEraDetail.POS + ", " + Environment.NewLine;
                                                drEraDetail.ProcessAs = drRemitParser.FIELD_REMIT_CLAIM_STATUS;
                                                FillReversalPayments.Details += "ProcessAs:" + drEraDetail.ProcessAs + ", " + Environment.NewLine;
                                                //drEraDetail.ClaimStatusCode = drRemitParser.FIELD_REMIT_CLAIM_STATUS_CODE;
                                                drEraDetail.SecondaryInsurance = drRemitParser.FIELD_REMIT_FORWARD_PAYER;
                                                FillReversalPayments.Details += "SecondaryInsurance:" + drEraDetail.SecondaryInsurance + ", " + Environment.NewLine;
                                                drEraDetail.SecondarySubscriberId = drRemitParser.FIELD_REMIT_SEC_SUBSCRIBER_ID;
                                                FillReversalPayments.Details += "SecondarySubscriberId:" + drEraDetail.SecondarySubscriberId + ", " + Environment.NewLine;
                                                drEraDetail.RemitCode = drRemitParser.FIELD_REMIT_HC_CODE;
                                                FillReversalPayments.Details += "RemitCode:" + drEraDetail.RemitCode + ", " + Environment.NewLine;

                                                if (string.IsNullOrEmpty(drRemitParser.FIELD_REMIT_FORWARD_PAYER))
                                                    drEraDetail.IsCrossedOver = false;
                                                else
                                                    drEraDetail.IsCrossedOver = true;
                                                FillReversalPayments.Details += "IsCrossedOver:" + drEraDetail.IsCrossedOver + ", " + Environment.NewLine;
                                                drEraDetail.ChargedAmount = drRemitParser.IsFIELD_REMIT_CHARGE_AMOUNTNull() == false ? drRemitParser.FIELD_REMIT_CHARGE_AMOUNT : 0;
                                                FillReversalPayments.Details += "ChargedAmount:" + drEraDetail.ChargedAmount + ", " + Environment.NewLine;
                                                drEraDetail.AllowedAmount = drRemitParser.IsFIELD_REMIT_ALLOWEDNull() == false ? drRemitParser.FIELD_REMIT_ALLOWED : 0;
                                                FillReversalPayments.Details += "AllowedAmount:" + drEraDetail.AllowedAmount + ", " + Environment.NewLine;
                                                drEraDetail.PaidAmount = drRemitParser.IsFIELD_REMIT_PAID_AMOUNTNull() == false ? drRemitParser.FIELD_REMIT_PAID_AMOUNT : 0;
                                                FillReversalPayments.Details += "PaidAmount:" + drEraDetail.PaidAmount + ", " + Environment.NewLine;
                                                drEraDetail.CoInsuranceAmount = drRemitParser.IsFIELD_REMIT_COINSURANCENull() == false ? drRemitParser.FIELD_REMIT_COINSURANCE : 0;
                                                FillReversalPayments.Details += "CoInsuranceAmount:" + drEraDetail.CoInsuranceAmount + ", " + Environment.NewLine;
                                                drEraDetail.DeductableAmount = drRemitParser.IsFIELD_REMIT_DEDUCTIBLENull() == false ? drRemitParser.FIELD_REMIT_DEDUCTIBLE : 0;
                                                FillReversalPayments.Details += "DeductableAmount:" + drEraDetail.DeductableAmount + ", " + Environment.NewLine;
                                                drEraDetail.Copayment = drRemitParser.IsFIELD_REMIT_COPAYMENTNull() == false ? drRemitParser.FIELD_REMIT_COPAYMENT : 0;
                                                FillReversalPayments.Details += "Copayment:" + drEraDetail.Copayment + ", " + Environment.NewLine;
                                                drEraDetail.LateFilingCharges = drRemitParser.IsFIELD_REMIT_LATE_FILING_CHARGENull() == false ? drRemitParser.FIELD_REMIT_LATE_FILING_CHARGE : 0;
                                                FillReversalPayments.Details += "LateFilingCharges:" + drEraDetail.LateFilingCharges + ", " + Environment.NewLine;
                                                if (MDVUtility.IsDateTime(drRemitParser.FIELD_REMIT_CLAIM_FROM_DATE))
                                                {
                                                    drEraDetail.VisitDate = Convert.ToDateTime(drRemitParser.FIELD_REMIT_CLAIM_FROM_DATE);
                                                    FillReversalPayments.Details += "VisitDate:" + drEraDetail.VisitDate + ", " + Environment.NewLine;
                                                }

                                                drEraDetail.IsActive = true;
                                                FillReversalPayments.Details += "DeductableAmount:" + drEraDetail.DeductableAmount + ", " + Environment.NewLine;
                                                drEraDetail.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                                drEraDetail.CreatedOn = DateTime.Now;
                                                drEraDetail.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                                                drEraDetail.ModifiedOn = DateTime.Now;

                                                //WriteOff and PatientResponsibility
                                                #region Step #:15 calculateWriteOfPatientResponsibility
                                                ERAActivitySteps calculateWriteOfPatientResponsibility = new ERAActivitySteps("Step #:15", "Calculating WriteOff and PatientResponsibility", "Start Calculating WriteOff and PatientResponsibility", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(calculateWriteOfPatientResponsibility);
                                                #endregion
                                                #region WriteOff and PatientResponsibility
                                                dsCache.Table_BL_Remittance_WriteOffRow[] dsWriteOff = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(drRemitParser.FIELD_REMIT_BL_REMITTANCE_ID_PK));
                                                decimal WriteOff = 0;
                                                decimal PatientResponsiblity = 0;
                                                foreach (dsCache.Table_BL_Remittance_WriteOffRow drWriteOff in dsWriteOff)
                                                {
                                                    decimal tempWriteOff = MDVUtility.ToDecimal(drWriteOff.FIELD_WRITE_OFF);
                                                    string tempGroupCode = MDVUtility.ToStr(drWriteOff.FIELD_WRITE_OFF_GROUP_CODE);
                                                    int tempResionCode = MDVUtility.ToInt(drWriteOff.FIELD_WRITE_OFF_REASON_CODE);

                                                    // ignore OA 23 write off amount.
                                                    if (tempGroupCode == "OA" && tempResionCode == 23 /*&& drEraDetail.ProcessAs == "Processed as Secondary"*/)
                                                        tempWriteOff = 0;

                                                    if (tempGroupCode == "PR" && tempResionCode >= 4)
                                                    {
                                                        PatientResponsiblity += tempWriteOff;
                                                    }
                                                    else if (tempGroupCode != "PR" && tempResionCode >= 4)
                                                    {
                                                        WriteOff += tempWriteOff;
                                                    }

                                                }
                                                #endregion
                                                calculateWriteOfPatientResponsibility.Details += Environment.NewLine;
                                                calculateWriteOfPatientResponsibility.Details += "----------------Result--------------" + Environment.NewLine;
                                                calculateWriteOfPatientResponsibility.Details += "WriteOff and PatientResponsibility has been calculated Successfully and WriteOff:" + WriteOff;
                                                calculateWriteOfPatientResponsibility.EndTime = DateTime.Now;
                                                calculateWriteOfPatientResponsibility.Status = true;
                                                drEraDetail.WriteOff = WriteOff;
                                                FillReversalPayments.Details += "WriteOff:" + drEraDetail.WriteOff + ", " + Environment.NewLine;
                                                drEraDetail.PatientResponsibility = PatientResponsiblity;
                                                FillReversalPayments.Details += "PatientResponsibility:" + drEraDetail.PatientResponsibility + ", " + Environment.NewLine;
                                                dsERADetail.ERADetail.AddERADetailRow(drEraDetail);
                                                #endregion
                                                FillReversalPayments.Details += Environment.NewLine;
                                                FillReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                                FillReversalPayments.Details += "ReversalPayments has been filled Successfully";
                                                FillReversalPayments.EndTime = DateTime.Now;
                                                FillReversalPayments.Status = true;
                                                //Insert ERA Detail
                                                #region Step #:16 insertReversalPayments
                                                ERAActivitySteps insertReversalPayments = new ERAActivitySteps("Step #:16", "Inserting ReversalPayments", "Start Inserting ReversalPayments", drEra.ERAId, 0, drReport.EDIReportId);
                                                ActivityStepList.Add(insertReversalPayments);
                                                #endregion
                                                dsERADetail = new DALERA(SharedVariable).InsertERADetail(SharedVariable, dsERADetail, dbManager);
                                                insertReversalPayments.Details += Environment.NewLine;
                                                insertReversalPayments.Details += "----------------Result--------------" + Environment.NewLine;
                                                insertReversalPayments.Details += "ReversalPayments has been Inserted Successfully and EraDetailId=" + drEraDetail.ERADtlId;
                                                insertReversalPayments.ERADetailsId = drEraDetail.ERADtlId;
                                                insertReversalPayments.EndTime = DateTime.Now;
                                                insertReversalPayments.Status = true;


                                                if (dsERADetail.ERADetail.Rows.Count > 0)
                                                {
                                                    #region Remittance_WriteOff
                                                    dsCache.Table_BL_Remittance_WriteOffRow[] dsWriteOffParse = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(drRemitParser.FIELD_REMIT_BL_REMITTANCE_ID_PK));
                                                    foreach (dsCache.Table_BL_Remittance_WriteOffRow drWriteOffParse in dsWriteOffParse)
                                                    {
                                                        #region Step #:18 fillERAClaimAdjustmentCodeStep
                                                        ERAActivitySteps fillERAClaimAdjustmentCodeStep = new ERAActivitySteps("Step #:18", "Filling ERA Claim Adjustment Code", "Start Filling ERA Claim Adjustment Code and values are:", drEra.ERAId, drEraDetail.ERADtlId, drReport.EDIReportId);
                                                        ActivityStepList.Add(fillERAClaimAdjustmentCodeStep);
                                                        #endregion
                                                        //Fill ERA ClaimAdjustmentCode
                                                        DSERA.ERAClaimAdjustmentCodeRow drERAClaimAdjustmentCode = dsERADetail.ERAClaimAdjustmentCode.NewERAClaimAdjustmentCodeRow();

                                                        drERAClaimAdjustmentCode.ERADtlId = MDVUtility.ToLong(dsERADetail.ERADetail.Rows[0][dsERADetail.ERADetail.ERADtlIdColumn]);
                                                        fillERAClaimAdjustmentCodeStep.Details += " ERADtlId:" + drERAClaimAdjustmentCode.ERADtlId + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.AdjGroupCode = drWriteOffParse.FIELD_WRITE_OFF_GROUP_CODE;
                                                        fillERAClaimAdjustmentCodeStep.Details += "AdjGroupCode:" + drERAClaimAdjustmentCode.AdjGroupCode + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.AdjReasonCode = drWriteOffParse.FIELD_WRITE_OFF_REASON_CODE;
                                                        fillERAClaimAdjustmentCodeStep.Details += "AdjReasonCode:" + drERAClaimAdjustmentCode.AdjReasonCode + ", " + Environment.NewLine;
                                                        drERAClaimAdjustmentCode.Amount = MDVUtility.ToDecimal(drWriteOffParse.FIELD_WRITE_OFF);
                                                        fillERAClaimAdjustmentCodeStep.Details += "Amount:" + drERAClaimAdjustmentCode.Amount + ", " + Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += "----------------Result--------------" + Environment.NewLine;
                                                        fillERAClaimAdjustmentCodeStep.Details += "ERA Claim Adjustment Code has been Filled Successfully";
                                                        fillERAClaimAdjustmentCodeStep.EndTime = DateTime.Now;
                                                        fillERAClaimAdjustmentCodeStep.Status = true;
                                                        //Insert ERA Claim Adjustment Code
                                                        #region Step #:19 insertERAClaimAdjustmentCodeStep
                                                        ERAActivitySteps insertERAClaimAdjustmentCodeStep = new ERAActivitySteps("Step #:19", "Inserting ERA Claim Adjustment Code", "Start Inserting ERA Claim Adjustment Code", drEra.ERAId, drEraDetail.ERADtlId, drReport.EDIReportId);
                                                        ActivityStepList.Add(insertERAClaimAdjustmentCodeStep);
                                                        #endregion

                                                        dsERADetail.ERAClaimAdjustmentCode.AddERAClaimAdjustmentCodeRow(drERAClaimAdjustmentCode);
                                                        dsERADetail = new DALERA(SharedVariable).InsertERAClaimAdjustmentCode(SharedVariable, dsERADetail, dbManager);
                                                        insertERAClaimAdjustmentCodeStep.Details += Environment.NewLine;
                                                        insertERAClaimAdjustmentCodeStep.Details += "----------------Result--------------" + Environment.NewLine;
                                                        insertERAClaimAdjustmentCodeStep.Details += "ERA Claim Adjustment Code has been Inserted Successfully";
                                                        insertERAClaimAdjustmentCodeStep.Status = true;
                                                        insertERAClaimAdjustmentCodeStep.EndTime = DateTime.Now;
                                                    }
                                                    #endregion
                                                }
                                            }
                                            #endregion
                                            //}
                                        }
                                    }
                                    #endregion
                                    DBInsertionParsedData.Details += Environment.NewLine;
                                    DBInsertionParsedData.Details += "----------------Result--------------" + Environment.NewLine;
                                    DBInsertionParsedData.Details += "Parsed data has been successfully Inserted in DB";
                                    drReport.IsParse = "P";
                                    DBInsertionParsedData.EndTime = DateTime.Now;
                                    DBInsertionParsedData.Status = true;
                                    //Comment Transaction
                                    dbManager.CommitTransaction();
                                }
                                catch (Exception ex)
                                {
                                    drReport.IsParse = "F";
                                    drReport.Comments = ex.Message;
                                    //Set the details of last ERAStep on which the exception has been occurred
                                    int lineNumber = 0;
                                    const string lineSearch = ":line ";
                                    var index = ex.StackTrace.LastIndexOf(lineSearch);
                                    if (index != -1)
                                    {
                                        var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                                        int.TryParse(lineNumberText, out lineNumber);
                                    }
                                    if (ActivityStepList.Count > 0)
                                    {
                                        ERAActivitySteps step = ActivityStepList.Last();
                                        step.Status = false;
                                        step.EndTime = DateTime.Now;
                                        step.Details += "----------------Result--------------" + Environment.NewLine;
                                        step.Details += ex.Message + " on Line #:" + lineNumber;
                                    }
                                    //RollBack Transaction
                                    dbManager.RollBackTransaction();
                                }
                                finally
                                {
                                    //Dispose dbManager
                                    dbManager.Dispose();
                                }

                            }
                            else
                            {
                                drReport.IsParse = "F";
                                drReport.Comments = "Could not Parse this file.";
                                ParsingLoadedReportStep.Details += Environment.NewLine;
                                ParsingLoadedReportStep.Details += "----------------Result--------------" + Environment.NewLine;
                                ParsingLoadedReportStep.Details += "Could not Parse this file.";
                                ParsingLoadedReportStep.Status = false;
                                ParsingLoadedReportStep.EndTime = DateTime.Now;
                            }

                        }
                        catch (Exception ex)
                        {
                            drReport.IsParse = "F";
                            drReport.Comments = ex.Message;
                            //Set the details of last ERAStep on which the exception has been occurred
                            int lineNumber = 0;
                            const string lineSearch = ":line ";
                            var index = ex.StackTrace.LastIndexOf(lineSearch);
                            if (index != -1)
                            {
                                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                                int.TryParse(lineNumberText, out lineNumber);
                            }
                            if (ActivityStepList.Count > 0)
                            {
                                ERAActivitySteps step = ActivityStepList.Last();
                                step.Status = false;
                                step.EndTime = DateTime.Now;
                                step.Details += "----------------Result--------------" + Environment.NewLine;
                                step.Details += ex.Message + " on Line #:" + lineNumber;
                            }
                            //return new BLObject<bool>(false, ex.Message);
                        }
                    }

                    //Update Report Status that which EDI File is Parsed and UnParsed
                    new DALEDIReports(SharedVariable).UpdateEDIReports(SharedVariable, dsReoprt);
                    dsReoprt.AcceptChanges();
                    return new BLObject<bool>(true);

                }
                else
                {
                    LoadReportStep.Details += Environment.NewLine;
                    LoadReportStep.Details += "----------------Result--------------" + Environment.NewLine;
                    LoadReportStep.Details += "Could not find any new report to download.";
                    LoadReportStep.EndTime = DateTime.Now;
                    LoadReportStep.Status = false;
                    return new BLObject<bool>(false, "Could not find any new report to download.");
                }


            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog(SharedVariable, "BLLERA::DownloadERA", ex);
                int lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    int.TryParse(lineNumberText, out lineNumber);
                }
                if (ActivityStepList.Count > 0)
                {
                    ERAActivitySteps step = ActivityStepList.Last();
                    step.Status = false;
                    step.EndTime = DateTime.Now;
                    step.Details += "----------------Result--------------" + Environment.NewLine;
                    step.Details += ex.Message + " on Line #:" + lineNumber;
                }
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                string xml = ERAActivityStepsLogger.MaintainERALogging(MDVUtility.DecryptFrom64(SharedVariable.UserName), ActivityStepList, 1);
                if (!string.IsNullOrEmpty(xml))
                {
                    string returnVal = new DALERA(SharedVariable).InsertERALog(SharedVariable, xml);
                }
            }
        }

        public BLObject<Dictionary<string, DSERA>> PostPayment(long ERAId, long ERADetailId, string Module = "")
        {
            List<ERAActivitySteps> ActivityStepList = new List<ERAActivitySteps>();
            try
            {
                DataTable dtPostedERAIDs = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "ID";
                COLUMN.DataType = typeof(long);
                dtPostedERAIDs.Columns.Add(COLUMN);
                string ERADtlIds = string.Empty;
                #region Step #:1 LoadERADetailStep
                ERAActivitySteps LoadERADetailStep = new ERAActivitySteps("Step #:1", "Function call to Load ERA Detail", "Start Loading ERA Detail with Perameter 'ERAId:" + ERAId + "', 'ERADetailId:" + ERADetailId + "', 'Module:" + Module + "'", ERAId, ERADetailId, 0);
                ActivityStepList.Add(LoadERADetailStep);
                #endregion
                Dictionary<string, DSERA> ResponseList = new Dictionary<string, DSERA>();
                DSERA dsERA = new DSERA();
                dsERA = new DALERA().LoadERADetail(ERADetailId, ERAId, Module);

                if (dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count > 0)
                {
                    LoadERADetailStep.EndTime = DateTime.Now;
                    LoadERADetailStep.Details += "Sucessfully Loaded ERA Detail with Rows:" + dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count;
                    LoadERADetailStep.Status = true;
                    IEnumerable<DataRow> Charges = dsERA.ERADetail.Select("" + dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND " + dsERA.ERADetail.PostStatusColumn.ColumnName + "='UnPosted'");// AND " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "<>'Reversal of Previous Payment'");

                    DSERA dsPostedCharges = new DSERA();
                    DSERA dsUnPostedCharges = new DSERA();
                    foreach (DSERA.ERADetailRow drDetail in Charges)
                    {
                        // those charges that have less then 0 ins balance and linked paid will be consider for future balance as negative.
                        if (decimal.Round(drDetail.InsBalance, 2) < 0 && (drDetail.ProcessAs != "Denied" && (drDetail.PaidAmount > 0 || drDetail.TotalAmount > 0)))
                        {
                            #region Step #:3 CheckPaymentIsPostedStep
                            ERAActivitySteps CheckPaymentIsPostedStep = new ERAActivitySteps("Step #:3", "Check either Payment Is already Posted", "Start Checking either Payment Is already Posted", ERAId, ERADetailId, 0);
                            ActivityStepList.Add(CheckPaymentIsPostedStep);
                            #endregion
                            if (drDetail.IsAlreadyPosted)
                                drDetail.Comments = "Payment is already posted.";
                            else
                                drDetail.Comments = "Future Balance will be negative.";

                            CheckPaymentIsPostedStep.Details += "----------------Result--------------" + Environment.NewLine;
                            CheckPaymentIsPostedStep.Details = drDetail.Comments;
                            CheckPaymentIsPostedStep.EndTime = DateTime.Now;
                            CheckPaymentIsPostedStep.Status = true;

                            dsUnPostedCharges.ERADetail.ImportRow(drDetail);
                        }
                        else if (drDetail.IsAlreadyPosted)
                        {
                            #region Step #:3 CheckPaymentIsPostedStep
                            ERAActivitySteps CheckPaymentIsPostedStep = new ERAActivitySteps("Step #:3", "Check either Payment Is already Posted", "Start Checking either Payment Is already Posted", ERAId, ERADetailId, 0);
                            ActivityStepList.Add(CheckPaymentIsPostedStep);
                            #endregion
                            drDetail.Comments = "Payment is already posted.";

                            CheckPaymentIsPostedStep.Details += "----------------Result--------------" + Environment.NewLine;
                            CheckPaymentIsPostedStep.Details = drDetail.Comments;
                            CheckPaymentIsPostedStep.EndTime = DateTime.Now;
                            CheckPaymentIsPostedStep.Status = true;

                            dsUnPostedCharges.ERADetail.ImportRow(drDetail);
                        }
                        else
                        {


                            DataRow Dr = dtPostedERAIDs.NewRow();
                            Dr[0] = drDetail.ERADtlId;
                            dtPostedERAIDs.Rows.Add(Dr);
                            ERADtlIds = ERADtlIds + drDetail.ERADtlId.ToString() + ",";


                        }
                    }
                    #region Update ERA's
                    DSERA dsPosted = new DSERA();
                    if (Charges.Count() > 0 && dtPostedERAIDs.Rows.Count > 0)
                    {
                        IDBManager dbManager = ClientConfiguration.GetDBManager();
                        try
                        {
                            #region Step #:3 AutoPaymentPostStep
                            ERAActivitySteps AutoPaymentPostStep = new ERAActivitySteps("Step #:3", "Auto Post Payment", "Start Auto Payment Posting and values are: ", ERAId, ERADetailId, 0);

                            AutoPaymentPostStep.Details += "ERADtlId: " + ERADtlIds + ", " + Environment.NewLine;
                            AutoPaymentPostStep.Details += "CreatedBy: " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName) + ", " + Environment.NewLine;
                            AutoPaymentPostStep.Details += "CreatedOn: " + DateTime.Now + ", " + Environment.NewLine;
                            AutoPaymentPostStep.Details += "ModifiedBy: " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName) + ", " + Environment.NewLine;
                            AutoPaymentPostStep.Details += "ModifiedOn: " + DateTime.Now + ", " + Environment.NewLine;
                            ActivityStepList.Add(AutoPaymentPostStep);
                            #endregion
                            //Begin Transaction
                            dbManager.BeginTransaction();
                            dsPosted = new DALPayment().AutoPaymentPostNew(dbManager, ERAId, dtPostedERAIDs, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now);//"Step #:4"
                            AutoPaymentPostStep.Details += "----------------Result--------------" + Environment.NewLine;
                            AutoPaymentPostStep.Details = "Successfully posted payments.";
                            AutoPaymentPostStep.EndTime = DateTime.Now;
                            AutoPaymentPostStep.Status = true;
                            //Comment Transaction
                            dbManager.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            //RollBack Transaction
                            dbManager.RollBackTransaction();
                            int lineNumber = 0;
                            const string lineSearch = ":line ";
                            var index = ex.StackTrace.LastIndexOf(lineSearch);
                            if (index != -1)
                            {
                                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                                int.TryParse(lineNumberText, out lineNumber);
                            }
                            if (ActivityStepList.Count > 0)
                            {
                                ERAActivitySteps step = ActivityStepList.Last();
                                step.Status = false;
                                step.EndTime = DateTime.Now;
                                step.Details += "----------------Result--------------" + Environment.NewLine;
                                step.Details += ex.Message + " on Line #:" + lineNumber;
                            }
                            throw ex;
                        }
                        finally
                        {
                            //Dispose dbManager
                            dbManager.Dispose();
                        }
                    }
                    //foreach (DataRow item in dsPosted.ERADetail.Rows)
                    //{
                    //    dsPostedCharges.ERADetail.ImportRow((DSERA.ERADetailRow)item);
                    //}
                    //if (dsPosted.ERADetail.Rows.Count > 0)
                    //dsPostedCharges.ERADetail.ImportRow((DSERA.ERADetailRow)dsPosted.ERADetail.Rows[0]);
                    //else
                    //    dsUnPostedCharges.ERADetail.ImportRow(drDetail);
                    #endregion
                    ResponseList.Add("UnPostedCharges", dsUnPostedCharges);
                    ResponseList.Add("PostedCharges", dsPosted);

                    return new BLObject<Dictionary<string, DSERA>>(ResponseList);
                }
                else
                {
                    LoadERADetailStep.Details += Environment.NewLine;
                    LoadERADetailStep.Details += "----------------Result--------------" + Environment.NewLine;
                    LoadERADetailStep.Details += "No ERA Detail rows found for Perameter 'ERAId:" + ERAId + "', 'ERADetailId:" + ERADetailId + "', 'Module:" + Module + "'";
                    LoadERADetailStep.EndTime = DateTime.Now;
                    LoadERADetailStep.Status = false;
                    return new BLObject<Dictionary<string, DSERA>>(null);
                }

            }
            catch (Exception ex)
            {
                int lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    int.TryParse(lineNumberText, out lineNumber);
                }
                if (ActivityStepList.Count > 0)
                {
                    ERAActivitySteps step = ActivityStepList.Last();
                    step.Status = false;
                    step.EndTime = DateTime.Now;
                    step.Details += "----------------Result--------------" + Environment.NewLine;
                    step.Details += ex.Message + " on Line #:" + lineNumber;
                }
                MDVLogger.BLLErrorLog("BLLERA::PostPayment", ex);
                return new BLObject<Dictionary<string, DSERA>>(null, ex.Message);
            }
            finally
            {
                string xml = ERAActivityStepsLogger.MaintainERALogging(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ActivityStepList, 2);
                if (!string.IsNullOrEmpty(xml))
                {
                    string returnVal = new DALERA().InsertERALog(xml);
                }
            }
        }

        public BLObject<bool> PostChargesPayment(long ERAId, string ERADetailIds, string Module = "")
        {
            List<ERAActivitySteps> ActivityStepList = new List<ERAActivitySteps>();
            try
            {
                #region Step #:1 LoadERADetailStep
                ERAActivitySteps LoadERADetailStep = new ERAActivitySteps("Step #:1", "Function call to Load ERA Detail", "Start Loading ERA Detail for multiple Charges with Perameter 'ERAId:" + ERAId + "', 'ERADetailIds:" + ERADetailIds + "', 'Module:" + Module + "'", ERAId, 0, 0);
                // LoadERADetailStep.ERADetailsId = ERADetailId;
                ActivityStepList.Add(LoadERADetailStep);
                #endregion
                DSERA dsERA = new DSERA();
                dsERA = new DALERA().LoadERADetail(0, ERAId, Module);

                if (dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count > 0)
                {
                    LoadERADetailStep.EndTime = DateTime.Now;
                    LoadERADetailStep.Details += "Sucessfully Loaded ERA Detail with Rows:" + dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count;
                    LoadERADetailStep.Status = true;
                    //IEnumerable<DataRow> Charges = dsERA.ERADetail.Select("" + dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND " + dsERA.ERADetail.PostStatusColumn.ColumnName + "='UnPosted' AND " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "<>'Reversal of Previous Payment' AND " + dsERA.ERADetail.ERADtlIdColumn.ColumnName + " IN (" + ERADetailId + ")");
                    IEnumerable<DataRow> Charges = dsERA.ERADetail.Select("" + dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND " + dsERA.ERADetail.PostStatusColumn.ColumnName + "='UnPosted' AND " + dsERA.ERADetail.ERADtlIdColumn.ColumnName + " IN (" + ERADetailIds + ")");
                    // dsERA.ERADetail Charges = dsERA.ERADetail.Select("" + dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND " + dsERA.ERADetail.PostStatusColumn.ColumnName + "='UnPosted' AND " + dsERA.ERADetail.ERADtlIdColumn.ColumnName + " IN (" + ERADetailIds + ")");
                    DataTable dtPostedERAIDs = new DataTable();
                    DataColumn COLUMN = new DataColumn();
                    COLUMN.ColumnName = "ID";
                    COLUMN.DataType = typeof(long);
                    dtPostedERAIDs.Columns.Add(COLUMN);
                    string output = string.Empty;
                    foreach (DSERA.ERADetailRow drDetail in Charges)
                    {
                        DataRow Dr = dtPostedERAIDs.NewRow();
                        Dr[0] = drDetail.ERADtlId;
                        dtPostedERAIDs.Rows.Add(Dr);
                        output = output + drDetail.ERADtlId.ToString() + ",";
                    }

                    // foreach (DSERA.ERADetailRow drDetail in Charges)
                    // {
                    DSERA dsPosted = new DSERA();
                    IDBManager dbManager = ClientConfiguration.GetDBManager();
                    try
                    {
                        #region Step #:3 AutoPaymentPostStep
                        string Details = "Start Auto Payment Posting and values are: ";
                        Details += "ERADtlId: " + output + ", " + Environment.NewLine;
                        Details += "CreatedBy: " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName) + ", " + Environment.NewLine;
                        Details += "CreatedOn: " + DateTime.Now + ", " + Environment.NewLine;
                        Details += "ModifiedBy: " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName) + ", " + Environment.NewLine;
                        Details += "ModifiedOn: " + DateTime.Now + ", " + Environment.NewLine;
                        ERAActivitySteps AutoPaymentPostStep = new ERAActivitySteps("Step #:3", "Auto Post Payment", Details, ERAId, 0, 0);
                        ActivityStepList.Add(AutoPaymentPostStep);
                        #endregion
                        //Begin Transaction
                        dbManager.BeginTransaction();
                        dsPosted = new DALPayment().AutoPaymentPostNew(dbManager, ERAId, dtPostedERAIDs, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now);
                        AutoPaymentPostStep.Details += "----------------Result--------------" + Environment.NewLine;
                        AutoPaymentPostStep.Details = "Successfully posted payments.";
                        AutoPaymentPostStep.EndTime = DateTime.Now;
                        AutoPaymentPostStep.Status = true;
                        //Comment Transaction
                        dbManager.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        //RollBack Transaction
                        dbManager.RollBackTransaction();
                        int lineNumber = 0;
                        const string lineSearch = ":line ";
                        var index = ex.StackTrace.LastIndexOf(lineSearch);
                        if (index != -1)
                        {
                            var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                            int.TryParse(lineNumberText, out lineNumber);
                        }
                        if (ActivityStepList.Count > 0)
                        {
                            ERAActivitySteps step = ActivityStepList.Last();
                            step.Status = false;
                            step.EndTime = DateTime.Now;
                            step.Details += "----------------Result--------------" + Environment.NewLine;
                            step.Details += ex.Message + " on Line #:" + lineNumber;
                        }
                        throw ex;
                    }
                    finally
                    {
                        //Dispose dbManager
                        dbManager.Dispose();
                    }
                    //}
                    return new BLObject<bool>(true);
                }
                else
                {
                    LoadERADetailStep.Details += Environment.NewLine;
                    LoadERADetailStep.Details += "----------------Result--------------" + Environment.NewLine;
                    LoadERADetailStep.Details += "No ERA Detail rows found for Perameter 'ERAId:" + ERAId + "', 'ERADetailIds:" + ERADetailIds + "', 'Module:" + Module + "'";
                    LoadERADetailStep.EndTime = DateTime.Now;
                    LoadERADetailStep.Status = false;
                    return new BLObject<bool>(false);
                }

            }
            catch (Exception ex)
            {
                int lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    int.TryParse(lineNumberText, out lineNumber);
                }
                if (ActivityStepList.Count > 0)
                {
                    ERAActivitySteps step = ActivityStepList.Last();
                    step.Status = false;
                    step.EndTime = DateTime.Now;
                    step.Details += "----------------Result--------------" + Environment.NewLine;
                    step.Details += ex.Message + " on Line #:" + lineNumber;
                }
                MDVLogger.BLLErrorLog("BLLERA::PostChargesPayment", ex);
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                string xml = ERAActivityStepsLogger.MaintainERALogging(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ActivityStepList, 2);
                if (!string.IsNullOrEmpty(xml))
                {
                    string returnVal = new DALERA().InsertERALog(xml);
                }
            }
        }

        public BLObject<DSERA> LoadERA(long ERAId, long ClearingHouseId, string CheckNo, long FacilityId, long PracticeId, string PayerName, int PayeeName, string Status, string FromEntryDate, string ToEntryDate, Int32 PageNumber = 1, Int32 RowsPerPage = 1000, string Module = "")
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().LoadERA(ERAId, ClearingHouseId, CheckNo, FacilityId, PracticeId, PayerName, PayeeName, Status, FromEntryDate, ToEntryDate, PageNumber, RowsPerPage, Module);
                return new BLObject<DSERA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadERA", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }
        }

        public BLObject<DSERA> ERASearch(long ERAId, long ClearingHouseId, string CheckNo, long FacilityId, long PracticeId, string PayerName, int PayeeName, string Status, string FromEntryDate, string ToEntryDate, Int32 PageNumber = 1, Int32 RowsPerPage = 1000, string Module = "",string CheckAmount="")
        {
            try
            {
                DSERA ds = new DSERA();
                ds = new DALERA().ERASearch(ERAId, ClearingHouseId, CheckNo, FacilityId, PracticeId, PayerName, PayeeName, Status, FromEntryDate, ToEntryDate, PageNumber, RowsPerPage, Module, CheckAmount);
                return new BLObject<DSERA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::ERASearch", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }
        }

        public BLObject<DSERA> InsertERA(DSERA ds)
        {
            try
            {
                ds = new DALERA().InsertERA(ds);
                return new BLObject<DSERA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::InsertERA", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }
        }
        public BLObject<DSERA> UpdateERA(DSERA ds)
        {
            try
            {
                ds = new DALERA().UpdateERA(ds);
                return new BLObject<DSERA>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::UpdateERA", ex);
                return new BLObject<DSERA>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteERA(string ERAId)
        {
            try
            {
                ERAId = new DALERA().DeleteERA(ERAId);
                return new BLObject<string>(ERAId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteERA", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSERALookup> LookupERAStatus()
        {
            try
            {
                DSERALookup ds = new DSERALookup();
                ds = new DALERA().LookupERAStatus();

                return new BLObject<DSERALookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupERAStatus", ex);
                return new BLObject<DSERALookup>(null, ex.Message);
            }

        }

        public BLObject<DSERALookup> LookupERAPayee()
        {
            try
            {
                DSERALookup ds = new DSERALookup();
                ds = new DALERA().LookupERAPayee();

                return new BLObject<DSERALookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LookupERAPayee", ex);
                return new BLObject<DSERALookup>(null, ex.Message);
            }

        }

        #region " ERA Reversal Payments "

        public List<dsCache.Table_BL_Remittance_CacheRow> GetReversalPayments(dsCache.Table_BL_Remittance_CacheRow[] dsRowsRemitParserArray, ref dsCache ds835Parser)
        {
            try
            {
                dsCache dstemp = new dsCache();
                List<dsCache.Table_BL_Remittance_CacheRow> dsRowsRemitParserList = new List<dsCache.Table_BL_Remittance_CacheRow>();
                List<dsCache.Table_BL_Remittance_CacheRow> dsReversalRowsList = new List<dsCache.Table_BL_Remittance_CacheRow>();
                // collect Reversal Rows
                dsReversalRowsList = dsRowsRemitParserArray.Where(p => p.FIELD_REMIT_CLAIM_STATUS == "Reversal of Previous Payment").ToList();
                // import all remaining rows.
                dsRowsRemitParserList = dsRowsRemitParserArray.Where(p => p.FIELD_REMIT_CLAIM_STATUS != "Reversal of Previous Payment").ToList();


                foreach (dsCache.Table_BL_Remittance_CacheRow item in dsReversalRowsList)
                {
                    dsCache.Table_BL_Remittance_CacheRow drPositiveReversalRow = null;
                    drPositiveReversalRow = (dsCache.Table_BL_Remittance_CacheRow)dsRowsRemitParserList.FirstOrDefault(
                        p =>
                           p.FIELD_REMIT_CHARGE_NUMBER == item.FIELD_REMIT_CHARGE_NUMBER // ERA Charge ID
                        && p.FIELD_REMIT_HCPCS == item.FIELD_REMIT_HCPCS   // CPT
                        && p.FIELD_REMIT_PATIENT_ACCOUNT_NO == item.FIELD_REMIT_PATIENT_ACCOUNT_NO   // Claim Number
                        && getSubscriberId(p) == getSubscriberId(item)); // Subscriber ID

                    if (drPositiveReversalRow != null)
                    {
                        // Adjust ReversalRow and its Positive Reversal Row negative
                        // remove this Positive Reversal Row because we have to adjust this row.
                        dsRowsRemitParserList.Remove(drPositiveReversalRow);

                        // Made Adjustments.
                        dsRowsRemitParserList.Add(AdjustReversalPayment(drPositiveReversalRow, item, ref ds835Parser));

                    }
                    else
                    {
                        // Add this Reversal Row only because there is no Positive Reversal Row for this.
                        dsRowsRemitParserList.Add(item);
                    }

                }

                return dsRowsRemitParserList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::GetReversalPayments", ex);
                throw ex;
            }


        }

        public dsCache.Table_BL_Remittance_CacheRow AdjustReversalPayment(dsCache.Table_BL_Remittance_CacheRow positiveRow, dsCache.Table_BL_Remittance_CacheRow negativeRow, ref dsCache ds835Parser)
        {
            //Positive
            decimal PositiveChargeAmount = positiveRow.IsFIELD_REMIT_CHARGE_AMOUNTNull() == false ? positiveRow.FIELD_REMIT_CHARGE_AMOUNT : 0;
            decimal PositiveAllowedAmount = positiveRow.IsFIELD_REMIT_ALLOWEDNull() == false ? positiveRow.FIELD_REMIT_ALLOWED : 0;
            decimal PositivePaidAmount = positiveRow.IsFIELD_REMIT_PAID_AMOUNTNull() == false ? positiveRow.FIELD_REMIT_PAID_AMOUNT : 0;
            decimal PositiveCopayAmount = positiveRow.IsFIELD_REMIT_COPAYMENTNull() == false ? positiveRow.FIELD_REMIT_COPAYMENT : 0;
            decimal PositiveCoInsAmount = positiveRow.IsFIELD_REMIT_COINSURANCENull() == false ? positiveRow.FIELD_REMIT_COINSURANCE : 0;
            decimal PositiveDeductiblesAmount = positiveRow.IsFIELD_REMIT_DEDUCTIBLENull() == false ? positiveRow.FIELD_REMIT_DEDUCTIBLE : 0;
            //decimal PositivePatientResponsibilityAmount = 0;
            //decimal PositiveWriteOffAmount = 0;
            //this.SetWriteOffAndPatientResponsiblity(ref PositiveWriteOffAmount, ref PositivePatientResponsibilityAmount, ds835Parser, positiveRow.FIELD_REMIT_BL_REMITTANCE_ID_PK.ToString());

            //Negative
            decimal NegativeChargeAmount = negativeRow.IsFIELD_REMIT_CHARGE_AMOUNTNull() == false ? negativeRow.FIELD_REMIT_CHARGE_AMOUNT : 0;
            decimal NegativeAllowedAmount = negativeRow.IsFIELD_REMIT_ALLOWEDNull() == false ? negativeRow.FIELD_REMIT_ALLOWED : 0;
            decimal NegativePaidAmount = negativeRow.IsFIELD_REMIT_PAID_AMOUNTNull() == false ? negativeRow.FIELD_REMIT_PAID_AMOUNT : 0;
            decimal NegativeCopayAmount = negativeRow.IsFIELD_REMIT_COPAYMENTNull() == false ? negativeRow.FIELD_REMIT_COPAYMENT : 0;
            decimal NegativeCoInsAmount = negativeRow.IsFIELD_REMIT_COINSURANCENull() == false ? negativeRow.FIELD_REMIT_COINSURANCE : 0;
            decimal NegativeDeductiblesAmount = negativeRow.IsFIELD_REMIT_DEDUCTIBLENull() == false ? negativeRow.FIELD_REMIT_DEDUCTIBLE : 0;
            //decimal NegativePatientResponsibilityAmount = 0;
            //decimal NegativeWriteOffAmount = 0;
            //this.SetWriteOffAndPatientResponsiblity(ref NegativeWriteOffAmount, ref NegativePatientResponsibilityAmount, ds835Parser, negativeRow.FIELD_REMIT_BL_REMITTANCE_ID_PK.ToString());

            //positiveRow.FIELD_REMIT_CHARGE_AMOUNT = PositiveChargeAmount + (NegativeChargeAmount);
            positiveRow.FIELD_REMIT_ALLOWED = PositiveAllowedAmount + (NegativeAllowedAmount);
            positiveRow.FIELD_REMIT_PAID_AMOUNT = PositivePaidAmount + (NegativePaidAmount);
            positiveRow.FIELD_REMIT_COPAYMENT = PositiveCopayAmount + (NegativeCopayAmount);
            positiveRow.FIELD_REMIT_COINSURANCE = PositiveCoInsAmount + (NegativeCoInsAmount);
            positiveRow.FIELD_REMIT_DEDUCTIBLE = PositiveDeductiblesAmount + (NegativeDeductiblesAmount);

            // if paid amount is negative then it's Process as will be 'Reversal of Previous Payment'
            if (MDVUtility.ToStr(positiveRow.FIELD_REMIT_PAID_AMOUNT).Contains('-') == true)
                positiveRow.FIELD_REMIT_CLAIM_STATUS = "Reversal of Previous Payment";

            //Adjust WriteOff And PatientResponsiblity
            AdjustWriteOffAndPatientResponsiblity(positiveRow.FIELD_REMIT_BL_REMITTANCE_ID_PK.ToString(), negativeRow.FIELD_REMIT_BL_REMITTANCE_ID_PK.ToString(), ref ds835Parser);

            return positiveRow;
        }

        public void AdjustWriteOffAndPatientResponsiblity(string PositiveRemittanceId, string NegativeRemittanceId, ref dsCache ds835Parser)
        {
            List<dsCache.Table_BL_Remittance_WriteOffRow> dsNegWriteOff = ((dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(NegativeRemittanceId))).ToList();

            List<long> matched_rows = new List<long>();

            foreach (dsCache.Table_BL_Remittance_WriteOffRow drPstWriteOff in ds835Parser.Table_BL_Remittance_WriteOff.Rows)
            {
                dsCache.Table_BL_Remittance_WriteOffRow matched_negrow = null;
                if (drPstWriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FK == MDVUtility.ToLong(PositiveRemittanceId))
                {
                    matched_negrow = dsNegWriteOff.ToList().FirstOrDefault(
                                     p => p.FIELD_WRITE_OFF_GROUP_CODE == drPstWriteOff.FIELD_WRITE_OFF_GROUP_CODE &&
                                          p.FIELD_WRITE_OFF_REASON == drPstWriteOff.FIELD_WRITE_OFF_REASON);

                    if (matched_negrow != null)
                    {
                        decimal pstWriteOff = MDVUtility.ToDecimal(drPstWriteOff.FIELD_WRITE_OFF);
                        decimal negWriteOff = MDVUtility.ToDecimal(matched_negrow.FIELD_WRITE_OFF);

                        // adjust matched row with positive row
                        drPstWriteOff.FIELD_WRITE_OFF = MDVUtility.ToStr(pstWriteOff + (negWriteOff));

                        //note matched rows Id's
                        matched_rows.Add(matched_negrow.FIELD_WRITE_OFF_ID_PK);
                    }
                }
            }

            //add unmatched renaming Negative  write off rows into positive write off
            foreach (dsCache.Table_BL_Remittance_WriteOffRow drNegWriteOff in dsNegWriteOff)
            {
                if (!matched_rows.Contains(drNegWriteOff.FIELD_WRITE_OFF_ID_PK))
                {
                    dsCache.Table_BL_Remittance_WriteOffRow new_row = ds835Parser.Table_BL_Remittance_WriteOff.NewTable_BL_Remittance_WriteOffRow();
                    new_row.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FK = MDVUtility.ToLong(PositiveRemittanceId);
                    new_row.FIELD_WRITE_OFF = drNegWriteOff.FIELD_WRITE_OFF;
                    new_row.FIELD_WRITE_OFF_REASON_CODE = drNegWriteOff.FIELD_WRITE_OFF_REASON_CODE;
                    new_row.FIELD_WRITE_OFF_GROUP_CODE = drNegWriteOff.FIELD_WRITE_OFF_GROUP_CODE;

                    ds835Parser.Table_BL_Remittance_WriteOff.AddTable_BL_Remittance_WriteOffRow(new_row);
                }
            }
        }

        public void SetWriteOffAndPatientResponsiblity(ref decimal WriteOff, ref decimal PatientResponsiblity, dsCache ds835Parser, string RemittanceId)
        {
            dsCache.Table_BL_Remittance_WriteOffRow[] dsWriteOff = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(RemittanceId));
            foreach (dsCache.Table_BL_Remittance_WriteOffRow drWriteOff in dsWriteOff)
            {
                decimal tempWriteOff = MDVUtility.ToDecimal(drWriteOff.FIELD_WRITE_OFF);
                string tempGroupCode = MDVUtility.ToStr(drWriteOff.FIELD_WRITE_OFF_GROUP_CODE);
                int tempResionCode = MDVUtility.ToInt(drWriteOff.FIELD_WRITE_OFF_REASON_CODE);

                // ignore OA 23 write off amount.
                if (tempGroupCode == "OA" && tempResionCode == 23 /*&& drEraDetail.ProcessAs == "Processed as Secondary"*/)
                    tempWriteOff = 0;

                if (tempGroupCode == "PR" && tempResionCode >= 4)
                {
                    PatientResponsiblity += tempWriteOff;
                }
                else if (tempGroupCode != "PR" && tempResionCode >= 4)
                {
                    WriteOff += tempWriteOff;
                }

            }
        }

        public string getSubscriberId(dsCache.Table_BL_Remittance_CacheRow row)
        {
            if (row.FIELD_REMIT_SUBSCRIBER_ID == "SELF")
                return row.FIELD_REMIT_HICN;
            else
                return row.FIELD_REMIT_SUBSCRIBER_ID;
        }

        #endregion

        #endregion


    }


}
