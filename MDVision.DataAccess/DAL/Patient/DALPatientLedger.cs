using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientLedger
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_LEDGER_SELECT = "Patient.sp_PatientLedgerSelect";
        private const string PROC_PATIENT_OUTSTANDING_SELECT = "Billing.sp_PatientLedgerOutstanding";
        private const string PROC_ACCOUNT_LEDGER_UPDATE = "Patient.sp_PatientPaymentUpdateComments";
        private const string PROC_ACCOUNT_LEDGER_SUMMARY = "Patient.sp_PatientLedger_Summery";
        private const string PROC_ACCOUNT_LEDGER_DETAIL = "Patient.sp_PatientLedger_Detail";
        private const string PROC_GET_PATIENT_PAYMENTS = "Patient.Sp_PatientPaymentSelect";
        private const string PROC_GET_PATIENT_RECEIVED_PAYMENTS = "Patient.sp_ReceivedPatientPayment";
        private const string PROC_LOAD_PRINT_PRACTICE= "Patient.sp_PatPaymentsPrintPractice";

        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PAT_PAYMENT_ID = "@PatPaymentId";
        private const string PARM_CHARGECAP_ID = "@ChargeCapId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_UNITS = "@Units";
        private const string PARM_MODIFIER_1 = "@Modifier1";
        private const string PARM_MODIFIER_2 = "@Modifier2";
        private const string PARM_MODIFIER_3 = "@Modifier3";
        private const string PARM_MODIFIER_4 = "@Modifier4";
        private const string PARM_ICDCODE_1 = "@ICDCode1";
        private const string PARM_ICDCODE_2 = "@ICDCode2";
        private const string PARM_ICDCODE_3 = "@ICDCode3";
        private const string PARM_ICDCODE_4 = "@ICDCode4";
        private const string PARM_POS_CODE = "@POSCode";
        private const string PARM_EMG = "@EMG";
        private const string PARM_FEE = "@Fee";
        private const string PARM_INS_CHARGES = "@InsCharges";
        private const string PARM_PAT_CHARGES = "@PatCharges";
        private const string PARM_ALLOWED_AMT = "@AllowedAmt";
        private const string PARM_COPAY = "@Copay";

        private const string PARM_PAN = "@PAN";
        private const string PARM_NDC = "@NDC";
        private const string PARM_NDC_UNIT = "@NDCUnit";
        private const string PARM_NDC_UNIT_PRICE = "@NDCUnitPrice";
        private const string PARM_NDC_MEASURE_CODEID = "@NDCMeasurCodeId";
        private const string PARM_LINE_NOTES = "@LineNotes";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";



        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";

        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PRACTICE_ID = "@PracticeId";

        private const string PARM_CLAIMED = "@Claimed";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_CLAIMED_NUMBER = "@ClaimNumber";

        private const string PARM_PAID = "@Paid";

        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_PATINSURANCE_ID = "@PatInsuranceId";

        private const string PARM_APPOINTMENT_STATUS = "@AppointmentStatus";

        private const string PARM_CHARGE_STATUS = "@ChargeStatus";

        private const string PARM_EOD = "@EOD";
        private const string PARM_STATUS = "@Status";
        private const string PARM_MASTER_CHARGE = "@MasterChargeId";
        private const string PARM_HOLD_DAYS = "@HoldDays";
        private const string PARM_IS_HOLD = "@IsHold";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_CHARGE_ORDER = "@ChargeOrder";
        private const string PARM_IS_PAYMENT = "@IsPayment";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_IS_VOIDED_CLAIM= "@isVoidedClaim";
        private const string PARM_CLAIM_BALANCE = "@ClaimBalance";

        private const string PARM_837_BATCH_ID = "@837BatchId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_READY_TO_SUBMIT = "@ReadyToSubmit";
        private const string PARM_CLAIM_STATUS = "@ClaimStatusId";


        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SERVICE_DESCRIPTION = "@ServiceDescription";
        private const string PARM_EXPECTED_FEE = "@ExpectedFee";//
        private const string PARM_PARENT_CHARGE_ID = "@ParentChargeId";

        private const string PARM_ICDCODE_1_DESCRIPTION = "@ICDCode1Description";
        private const string PARM_ICDCODE_2_DESCRIPTION = "@ICDCode2Description";
        private const string PARM_ICDCODE_3_DESCRIPTION = "@ICDCode3Description";
        private const string PARM_ICDCODE_4_DESCRIPTION = "@ICDCode4Description";

        private const string PARM_ICD10CODE_1 = "@ICD10Code1";
        private const string PARM_ICD10CODE_2 = "@ICD10Code2";
        private const string PARM_ICD10CODE_3 = "@ICD10Code3";
        private const string PARM_ICD10CODE_4 = "@ICD10Code4";

        private const string PARM_ICD10CODE_1_DESCRIPTION = "@ICD10Code1Description";
        private const string PARM_ICD10CODE_2_DESCRIPTION = "@ICD10Code2Description";
        private const string PARM_ICD10CODE_3_DESCRIPTION = "@ICD10Code3Description";
        private const string PARM_ICD10CODE_4_DESCRIPTION = "@ICD10Code4Description";

        private const string PARM_SNOMED_ID = "@SnomedId";
        private const string PARM_SNOMED_DESCRIPTION = "@SnomedDescription";
        private const string PARM_LEXICODE = "@LexiCode";
        private const string PARM_LEXICODE_DESCRIPTION = "@LexiCodeDescription";



        private const string PARM_CHARGES = "@Charges";
        private const string PARM_INS_PAYMENT = "@InsPayment";
        private const string PARM_PAT_PAYMENT = "@PatPayment";
        private const string PARM_STATEMENT = "@Statement";
        private const string PARM_SUBMIT = "@Submit";
        private const string PARM_PAYMENT_ID = "@Submit";
        private const string PARM_COMMENT = "@Submit";
        private const string PARM_IS_COLLECTION = "@IsCollection";
        private const string PARM_IS_OTHERCLAIMS = "@IsOtherClaim";


        #endregion

        #region Constructors
        public DALPatientLedger()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
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

        #region "Support Functions"


        private void CreateParameters(IDBManager dbManager, DSCharge ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(57);

            if (IsInsert == true)
            {
                //              dbManager.CreateParameters(38);
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                //                dbManager.CreateParameters(40);
                dbManager.AddParameters(0, PARM_CHARGECAP_ID, ds.PatientCharges.ChargeCapIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_VISIT_ID, ds.PatientCharges.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DOS_FROM, ds.PatientCharges.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_DOS_TO, ds.PatientCharges.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_CPT_CODE, ds.PatientCharges.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_UNITS, ds.PatientCharges.UnitsColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_MODIFIER_1, ds.PatientCharges.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIER_2, ds.PatientCharges.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIER_3, ds.PatientCharges.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIER_4, ds.PatientCharges.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ICDCODE_1, ds.PatientCharges.ICDCode1Column.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ICDCODE_2, ds.PatientCharges.ICDCode2Column.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ICDCODE_3, ds.PatientCharges.ICDCode3Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ICDCODE_4, ds.PatientCharges.ICDCode4Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_POS_CODE, ds.PatientCharges.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_EMG, ds.PatientCharges.EMGColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(16, PARM_FEE, ds.PatientCharges.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(17, PARM_INS_CHARGES, ds.PatientCharges.InsChargesColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(18, PARM_PAT_CHARGES, ds.PatientCharges.PatChargesColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(19, PARM_ALLOWED_AMT, ds.PatientCharges.AllowedAmtColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(20, PARM_COPAY, ds.PatientCharges.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(21, PARM_PAN, ds.PatientCharges.PANColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_NDC, ds.PatientCharges.NDCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_NDC_UNIT, ds.PatientCharges.NDCUnitColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(24, PARM_NDC_UNIT_PRICE, ds.PatientCharges.NDCUnitPriceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(25, PARM_NDC_MEASURE_CODEID, ds.PatientCharges.NDCMeasurCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_LINE_NOTES, ds.PatientCharges.LineNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_IS_ACTIVE, ds.PatientCharges.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(28, PARM_CREATED_BY, ds.PatientCharges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_CREATED_ON, ds.PatientCharges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(30, PARM_MODIFIED_BY, ds.PatientCharges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_MODIFIED_ON, ds.PatientCharges.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(32, PARM_EOD, ds.PatientCharges.EODColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(33, PARM_STATUS, ds.PatientCharges.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_MASTER_CHARGE, ds.PatientCharges.MasterChargeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(35, PARM_HOLD_DAYS, ds.PatientCharges.HoldDaysColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(36, PARM_IS_HOLD, ds.PatientCharges.IsHoldColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(37, PARM_CHARGE_ORDER, ds.PatientCharges.ChargeOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(38, PARM_EXPECTED_FEE, ds.PatientCharges.ExpectedFeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(39, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(40, PARM_PARENT_CHARGE_ID, ds.PatientCharges.ParentChargeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(41, PARM_ICDCODE_1_DESCRIPTION, ds.PatientCharges.ICDCode1DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(42, PARM_ICDCODE_2_DESCRIPTION, ds.PatientCharges.ICDCode2DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(43, PARM_ICDCODE_3_DESCRIPTION, ds.PatientCharges.ICDCode3DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(44, PARM_ICDCODE_4_DESCRIPTION, ds.PatientCharges.ICDCode4DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(45, PARM_ICD10CODE_1, ds.PatientCharges.ICD10Code1Column.ColumnName, DbType.String);
            dbManager.AddParameters(46, PARM_ICD10CODE_2, ds.PatientCharges.ICD10Code2Column.ColumnName, DbType.String);
            dbManager.AddParameters(47, PARM_ICD10CODE_3, ds.PatientCharges.ICD10Code3Column.ColumnName, DbType.String);
            dbManager.AddParameters(48, PARM_ICD10CODE_4, ds.PatientCharges.ICD10Code4Column.ColumnName, DbType.String);

            dbManager.AddParameters(49, PARM_ICD10CODE_1_DESCRIPTION, ds.PatientCharges.ICD10Code1DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(50, PARM_ICD10CODE_2_DESCRIPTION, ds.PatientCharges.ICD10Code2DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(51, PARM_ICD10CODE_3_DESCRIPTION, ds.PatientCharges.ICD10Code3DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(52, PARM_ICD10CODE_4_DESCRIPTION, ds.PatientCharges.ICD10Code4DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(53, PARM_SNOMED_ID, ds.PatientCharges.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(54, PARM_SNOMED_DESCRIPTION, ds.PatientCharges.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(55, PARM_LEXICODE, ds.PatientCharges.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(56, PARM_LEXICODE_DESCRIPTION, ds.PatientCharges.LexiCodeDescriptionColumn.ColumnName, DbType.String);

            //if (IsInsert == false)
            //{
            //    dbManager.AddParameters(37, PARM_CHARGE_ORDER, ds.PatientCharges.ChargeOrderColumn.ColumnName, DbType.Int32);
            //    dbManager.AddParameters(38, PARM_EXPECTED_FEE, ds.PatientCharges.ExpectedFeeColumn.ColumnName, DbType.Double);
            //    dbManager.AddParameters(39, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            //}
            //else
            //{
            //    dbManager.AddParameters(37, PARM_SERVICE_DESCRIPTION, ds.PatientCharges.ServiceDescriptionColumn.ColumnName, DbType.String);
            //}



            // dbManager.AddParameters(37, PARM_IS_PRIMARY, ds.PatientCharges.IsPrimaryColumn.ColumnName, DbType.Byte);

        }



        #endregion

        #region "Insert, delete, update and get using dataset Functions"




        public DSPatientLedger LoadPatientLedger(long patientId, long FacilityId, long ProviderId, DateTime? DOSFrom = null, DateTime? DOSTo = null, long InsurancePlanId = 0, long BilledToId = 0, long ClaimBalId = 0,bool isCollection = false,bool isOtherClaims = false,bool IsVoidedClaims = false,bool isShowDetails = false)
        {
            DSPatientLedger ds = new DSPatientLedger();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(10);

                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (FacilityId == 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);


                dbManager.AddParameters(3, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(4, PARM_DOS_TO, DOSTo);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(5, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                dbManager.AddParameters(6, PARM_IS_VOIDED_CLAIM, IsVoidedClaims);

                if (ClaimBalId == 0)
                {
                    dbManager.AddParameters(7, PARM_CLAIM_BALANCE, null);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_CLAIM_BALANCE, ClaimBalId);
                }
                dbManager.AddParameters(8, PARM_IS_COLLECTION, isCollection);
                dbManager.AddParameters(9, PARM_IS_OTHERCLAIMS, isOtherClaims);

                //dbManager.AddParameters(7, PARM_INS_PAYMENT, InsPayment);
                //dbManager.AddParameters(8, PARM_PAT_PAYMENT, PatPayment);
                //dbManager.AddParameters(9, PARM_STATEMENT, Statement);
                //dbManager.AddParameters(10, PARM_SUBMIT, Submit);


                //if (PageNumber == 0)
                //    dbManager.AddParameters(11, PARM_PAGE_NUMBER, null);
                //else
                //    dbManager.AddParameters(11, PARM_PAGE_NUMBER, PageNumber);

                //if (RowspPage == 0)
                //    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, null);
                //else
                //    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, RowspPage);

                //dbManager.AddParameters(13, PARM_RECORD_COUNT, ds.PatientCharges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                //    dbManager.AddParameters(14, PARM_ENTITY_ID, null);
                //else
                //    dbManager.AddParameters(14, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                //dbManager.AddParameters(15, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (isShowDetails == true)
                {
                    ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACCOUNT_LEDGER_DETAIL, ds, ds.PatientCharges_New.TableName);
                }
                else
                {
                    ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACCOUNT_LEDGER_SUMMARY, ds, ds.PatientCharges_New.TableName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientLedger::LoadPatientCharges", PROC_ACCOUNT_LEDGER_SUMMARY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientLedger SearchPatientPayments(long patientId, long PageNumber = 1, long RowsPerPage = 15)
        {
            DSPatientLedger ds = new DSPatientLedger();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PERPAGE, RowsPerPage);
                ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_PATIENT_PAYMENTS, ds, ds.PatientPayments.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientLedger::LoadPatientOutstandingBalance", PROC_PATIENT_OUTSTANDING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientLedger SearchReceivedPayments(long pmtId)
        {
            DSPatientLedger ds = new DSPatientLedger();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PAT_PAYMENT_ID, pmtId);
                ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_PATIENT_RECEIVED_PAYMENTS, ds, ds.ReceivedPatPayment.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientLedger::LoadPatientOutstandingBalance", PROC_PATIENT_OUTSTANDING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientLedger LoadPrintPractice(long PracticeId)
        {
            DSPatientLedger ds = new DSPatientLedger();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PRACTICE_ID, PracticeId);
                ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_PRINT_PRACTICE, ds, ds.DT_Print_Practice.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientLedger::LoadPrintPractice", PROC_LOAD_PRINT_PRACTICE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientLedger LoadPatientOutstandingBalance(long patientId)
        {
            DSPatientLedger ds = new DSPatientLedger();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSPatientLedger)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_OUTSTANDING_SELECT, ds, ds.PatientOutstandingBalance.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientLedger::LoadPatientOutstandingBalance", PROC_PATIENT_OUTSTANDING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion
    }
}

