using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.PatientStatement
{
    public class DALPatientStatement
    {
        #region Variable
        public SharedVariable SharedVariable { get; set; }
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_STATEMENT_SELECT = "System.sp_StatementSelect";
        private const string PROC_STATEMENT_SEARCH = "System.sp_StatementSearch";
        private const string PROC_PATIENT_STATEMENT_PRINT = "System.PatientStatementPrint";
        private const string PROC_PATIENT_SUBMITTED_STATEMENT_PRINT = "System.SubmittedStatementPrint ";
        private const string PROC_PATIENT_STATEMENT_INSERT = "Billing.sp_PatientStatementInsert";
        private const string PROC_PATIENT_STATEMENT_SELECT = "Billing.sp_PatientStatementSelect";
        private const string PROC_PATIENT_SUBMITTED_STATEMENT_SELECT = "Billing.sp_PatientSubmittedStatementSelect";
        private const string PROC_PATIENT_STATEMENT_UPDATE = "Billing.sp_PatientStatementUpdate";
        private const string PROC_PATIENT_STATEMENT_DELETE = "Billing.sp_PatientStatementDelete";
        private const string PROC_STATEMENT_SUBMITTED_STATUS = "System.sp_StatementSubmittedStatus";
        private const string PROC_PATIENT_STATEMENT_BATCH_INSERT = "Billing.sp_SubmittedStatementBatchInsert";
        private const string PROC_PATIENT_STATEMENT_BATCH_SELECT = "Billing.sp_SubmittedStatementBatchSelect";
        private const string PROC_STATEMENT_BATCH_DETAIL_SELECT = "Billing.sp_StatementBatchDetailSelect";
        private const string PROC_PATIENT_STATEMENT_BATCH_RESUBMIT = "Billing.sp_SubmittedStatementBatchResubmit";
        private const string PROC_GET_SUBMITTEDSTATEMENT_HTML = "Billing.sp_GetSubmittedStatementHTML";



        #endregion


        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_GURANTOR_ID = "@GuarantorId";

        private const string PARM_PATIENT_LAST_NAME = "@LastName";
        private const string PARM_PATIENT_FIRST_NAME = "@FirstName";
        private const string PARM_IS_SUBMIT = "@IsSubmit";

        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        
        private const string PARM_STATEMENT_FORMAT = "@StatementFormat";

        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";

        private const string PARM_LASTSTAEMENT_DOS_FROM = "@LastStatementDOSFrom";
        private const string PARM_LASTSTAEMENT_DOS_TO = "@LastStatementDOSTo";
        private const string PARM_PAT_BAL_GREATER_THAN = "@PatBalanceGreaterThan";
        private const string PARM_PAT_BAL_LESS_THAN = "@PatBalanceLessThan";

        private const string PARM_COMMENT = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SUBMITTED_STATEMENT = "@SubmittedStatementId";
        private const string PARM_IS_IGNORE_CYCLE_DAYES_CHECKED = "@isIgnoreCycleDaysChecked";
        private const string PARM_AGE = "@Age";
        

        private const string PARM_PATIENT_STATEMENT_ID = "@PatStatementId";
        private const string PARM_ADV_PAYMENT_ID = "@AdvPaymentId";
        private const string PARM_PATIENT_BALANCE = "@PatientBalance";
        private const string PARM_LAST_STATEMENT_DATE = "@LastStatementDate";
        private const string PARM_STATEMENT = "@Statement";
        private const string PARM_VISIT_IDs = "@VisitIDs";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_BATCH_ID = "@BatchId";
        private const string PARM_SUBMITTED_CHARGE_IDs = "@SubmittedChargeIds";
        private const string PARM_BATCH_NUMBER = "@BatchNumber";
        private const string PARM_BATCH_SUBMITTED_DATE = "@SubmittedDate";
        private const string PARM_BATCH_SUBMITTED_BY = "@SubmittedBy";
        private const string PARM_BATCH_SUBMIT_TYPE = "@SubmitType";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_BATCH_STATUS = "@BatchStatus";
        private const string PARM_BATCH_TOTAL_PATIENTS = "@TotalPatients";
        private const string PARM_BATCH_XML = "@BatchXML";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_BATCH_SUBMITTED_BY_Id = "@SubmittedById";



        #endregion

        #region Constructors
        public DALPatientStatement()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
       
        public DALPatientStatement(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

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
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager object</param>
        /// <param name="ds">dataset of Patient Statement</param>
        /// <param name="IsInsert">whether the parameters are being created for insert operation or not </param>
        /// 
        private void CreateParameters(IDBManager dbManager, DSPatientStatement ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(22);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, ds.PatientStatement.PatientStmtIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, ds.PatientStatement.PatientStmtIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientStatement.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, ds.PatientStatement.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PATIENT_FIRST_NAME, ds.PatientStatement.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FACILITY_ID, ds.PatientStatement.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_AGE, ds.PatientStatement.AgeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_DOS_FROM, ds.PatientStatement.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_DOS_TO, ds.PatientStatement.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_PROVIDER_ID, ds.PatientStatement.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_ADV_PAYMENT_ID, ds.PatientStatement.AdvancePaymentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_PATIENT_BALANCE, ds.PatientStatement.PatBalanceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(11, PARM_LAST_STATEMENT_DATE, ds.PatientStatement.LastStatementDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_STATEMENT, ds.PatientStatement.StatementColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_VISIT_IDs, ds.PatientStatement.VisitIDsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.PatientStatement.isActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.PatientStatement.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.PatientStatement.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.PatientStatement.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.PatientStatement.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_BATCH_ID, ds.PatientStatement.BatchIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(20, PARM_SUBMITTED_CHARGE_IDs, ds.PatientStatement.SubmittedChargeIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_GURANTOR_ID, ds.PatientStatement.GuarantorIdColumn.ColumnName, DbType.String);
          
        }

        #endregion


        #region "Functions Patient Statements"
        public DSPatientStatement InsertPatientStatement(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSPatientStatement)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_INSERT, ds, ds.PatientStatement.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::InsertPatientStatement", PROC_PATIENT_STATEMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeletePatientStatement(long PatientStatementID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, PatientStatementID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::DeletePatientStatement", PROC_PATIENT_STATEMENT_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement LoadPrintedPatientStatement(Int64 PatientStatementID, long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(10);

                if (patientLastName == "")
                    patientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                //if (statementFormat == "")
                //    statementFormat = null;

                if (PatientStatementID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, PatientStatementID);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, patientLastName);
                dbManager.AddParameters(3, PARM_PATIENT_FIRST_NAME, PatientFirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);

                if (Age == 0)
                    dbManager.AddParameters(5, PARM_AGE, null);
                else
                    dbManager.AddParameters(5, PARM_AGE, Age);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(9, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_SELECT, ds, ds.PatientStatement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPrintedPatientStatement", PROC_PATIENT_STATEMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientStatement LoadPatientSubmittedStatement(Int64 PatientStatementID, long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? LastStatementDateFrom, DateTime? LastStatementDateTo, Int64 PatBalanceGreaterThan, Int64 PatBalanceLessThan, int ClearingHouseId , int pageNumber, int RecordPerPage = 15)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(16);

                if (patientLastName == "")
                    patientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                //if (statementFormat == "")
                //    statementFormat = null;

                if (PatientStatementID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_STATEMENT_ID, PatientStatementID);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_PATIENT_LAST_NAME, patientLastName);
                dbManager.AddParameters(3, PARM_PATIENT_FIRST_NAME, PatientFirstName);

                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);

                if (Age == 0)
                    dbManager.AddParameters(5, PARM_AGE, null);
                else
                    dbManager.AddParameters(5, PARM_AGE, Age);

                //dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                //dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(9, PARM_ROWS_PER_PAGE, RecordPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, 0);
                dbManager.AddParameters(11, PARM_LASTSTAEMENT_DOS_FROM, LastStatementDateFrom);
                dbManager.AddParameters(12, PARM_LASTSTAEMENT_DOS_TO, LastStatementDateTo);
                if (PatBalanceGreaterThan == 0)
                {
                    dbManager.AddParameters(13, PARM_PAT_BAL_GREATER_THAN, null);
                }
                else
                {
                    dbManager.AddParameters(13, PARM_PAT_BAL_GREATER_THAN, PatBalanceGreaterThan);

                }

                if (PatBalanceLessThan == 0)
                {
                    dbManager.AddParameters(14, PARM_PAT_BAL_LESS_THAN, null);
                }
                else
                {
                    dbManager.AddParameters(14, PARM_PAT_BAL_LESS_THAN, PatBalanceLessThan);
                }
                if (ClearingHouseId == 0)
                    dbManager.AddParameters(15, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(15, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                //dbManager.AddParameters(10, PARM_PAGE_NUMBER, pageNumber);
                //dbManager.AddParameters(11, PARM_ROWS_PER_PAGE, RecordPerPage);

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_SUBMITTED_STATEMENT_SELECT, ds, ds.PatientStatement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPatientSubmittedStatement", PROC_PATIENT_SUBMITTED_STATEMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement LoadPatientStatement(long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo, string statementFormat = "")
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (patientLastName == "")
                    patientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                //if (statementFormat == "")
                //    statementFormat = null;


                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_PATIENT_LAST_NAME, patientLastName);
                dbManager.AddParameters(2, PARM_PATIENT_FIRST_NAME, PatientFirstName);

                //if (ProvideId == 0)
                //    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                //else
                //    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProvideId);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                //if (PracticeId == 0)
                //    dbManager.AddParameters(5, PARM_PRACTICE_ID, null);
                //else
                //    dbManager.AddParameters(5, PARM_PRACTICE_ID, PracticeId);

                if (Age == 0)
                    dbManager.AddParameters(4, PARM_AGE, null);
                else
                    dbManager.AddParameters(4, PARM_AGE, Age);

                dbManager.AddParameters(5, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(6, PARM_DOS_TO, DOSTo);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);
               
                //dbManager.AddParameters(9, PARM_STATEMENT_FORMAT, statementFormat);

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_SELECT, ds, ds.PatientStatement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPatientStatement", PROC_STATEMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement PatientStatementSearch(long PatientId, string patientLastName, string PatientFirstName, long FacilityId, long Age, DateTime? DOSFrom, DateTime? DOSTo, DateTime? LastStatementDateFrom, DateTime? LastStatementDateTo, Double PatBalanceGreaterThan, Double PatBalanceLessThan, bool isIgnoreCycleDaysChecked, string statementFormat, int pageNumber, int RecordPerPage)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (patientLastName == "")
                    patientLastName = null;

                if (PatientFirstName == "")
                    PatientFirstName = null;

                //if (statementFormat == "")
                //    statementFormat = null;


                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(1, PARM_PATIENT_LAST_NAME, patientLastName);
                dbManager.AddParameters(2, PARM_PATIENT_FIRST_NAME, PatientFirstName);

                //if (ProvideId == 0)
                //    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                //else
                //    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProvideId);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                //if (PracticeId == 0)
                //    dbManager.AddParameters(5, PARM_PRACTICE_ID, null);
                //else
                //    dbManager.AddParameters(5, PARM_PRACTICE_ID, PracticeId);

                if (Age == 0)
                    dbManager.AddParameters(4, PARM_AGE, null);
                else
                    dbManager.AddParameters(4, PARM_AGE, Age);

                dbManager.AddParameters(5, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(6, PARM_DOS_TO, DOSTo);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(9, PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(10, PARM_ROWS_PER_PAGE, RecordPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, 0);
                dbManager.AddParameters(12, PARM_LASTSTAEMENT_DOS_FROM, LastStatementDateFrom);
                dbManager.AddParameters(13, PARM_LASTSTAEMENT_DOS_TO, LastStatementDateTo);
                if (PatBalanceGreaterThan == 0)
                {
                    dbManager.AddParameters(14, PARM_PAT_BAL_GREATER_THAN, null);
                }
                else
                {
                    dbManager.AddParameters(14, PARM_PAT_BAL_GREATER_THAN, PatBalanceGreaterThan);

                }

                if (PatBalanceLessThan == 0)
                {
                    dbManager.AddParameters(15, PARM_PAT_BAL_LESS_THAN, null);
                }
                else
                {
                    dbManager.AddParameters(15, PARM_PAT_BAL_LESS_THAN, PatBalanceLessThan);
                }

                dbManager.AddParameters(16, PARM_IS_IGNORE_CYCLE_DAYES_CHECKED, isIgnoreCycleDaysChecked);


                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_SEARCH, ds, ds.PatientStatement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::PatientStatementSearch", PROC_STATEMENT_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        public DSPatientStatement LoadPrintSubmitPatientStatement(long PatientId, string lastName, string FirstName, long facilityId, long submittedstatementId, DateTime? DOSFrom, DateTime? DOSTo)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FirstName == "")
                    FirstName = null;
                if (lastName == "")
                    lastName = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_PATIENT_LAST_NAME, lastName);
                dbManager.AddParameters(2, PARM_PATIENT_FIRST_NAME, FirstName);

                if (facilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, facilityId);
                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(5, PARM_DOS_TO, DOSTo);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_SUBMITTED_STATEMENT, submittedstatementId);
                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_SUBMITTED_STATEMENT_PRINT, ds, ds.PatientStatementPrint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPrintSubmitPatientStatement", PROC_PATIENT_SUBMITTED_STATEMENT_PRINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientStatement LoadPrintPatientStatement(long PatientId, string lastName, string FirstName, long facilityId,long submittedstatementId,int Age, DateTime? DOSFrom, DateTime? DOSTo)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FirstName == "")
                    FirstName = null;
                if (lastName == "")
                    lastName = null;
                dbManager.Open();
                dbManager.CreateParameters(10);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_PATIENT_LAST_NAME, lastName);
                dbManager.AddParameters(2, PARM_PATIENT_FIRST_NAME, FirstName);

                if (facilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, facilityId);
                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(5, PARM_DOS_TO, DOSTo);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(8, PARM_SUBMITTED_STATEMENT, submittedstatementId);
                if (Age != 0)
                {
                    dbManager.AddParameters(9, PARM_AGE, Age);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_AGE, null);
                }
                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_PRINT, ds, ds.PatientStatementPrint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPrintPatientStatement", PROC_PATIENT_STATEMENT_PRINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// overload Method for multithreading 
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="lastName"></param>
        /// <param name="FirstName"></param>
        /// <param name="facilityId"></param>
        /// <param name="DOSFrom"></param>
        /// <param name="DOSTo"></param>
        /// <returns></returns>
        public DSPatientStatement LoadPrintPatientStatement(SharedVariable sharedVariable, long PatientId, string lastName, string FirstName, long facilityId,long submittedstatementId, DateTime? DOSFrom, DateTime? DOSTo)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FirstName == "")
                    FirstName = null;
                if (lastName == "")
                    lastName = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_PATIENT_LAST_NAME, lastName);
                dbManager.AddParameters(2, PARM_PATIENT_FIRST_NAME, FirstName);

                if (facilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, facilityId);
                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(5, PARM_DOS_TO, DOSTo);

                if (ClientConfiguration.DecryptFrom64(sharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, sharedVariable.EntityId);

                dbManager.AddParameters(7, PARM_USER_ID, sharedVariable.AppUserId);
                dbManager.AddParameters(8, PARM_SUBMITTED_STATEMENT, submittedstatementId);
                
                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_PRINT, ds, ds.PatientStatementPrint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::LoadPrintPatientStatement", PROC_PATIENT_STATEMENT_PRINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement GetSubmittedStatementHTML(Int64 SubmittedStatementId)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SUBMITTED_STATEMENT, SubmittedStatementId);
                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_SUBMITTEDSTATEMENT_HTML, ds, ds.SubmittedStatement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::GetSubmittedStatementHTML", PROC_GET_SUBMITTEDSTATEMENT_HTML, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string StatementSubmittedStatus(long PatientId)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                return Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_STATEMENT_SUBMITTED_STATUS));
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::StatementSubmittedStatus", PROC_STATEMENT_SUBMITTED_STATUS, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement SearchPatientStatementsBatch(long BatchId, string BatchNumber, DateTime? SubmittedDate, string SubmittedBy, long ?SubmittedById,string SubmitType, int ClearingHouseId, string BatchStatus, int PageNumber, int RowspPage)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);

                if (BatchId <= 0)
                    dbManager.AddParameters(0, PARM_BATCH_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

                if (BatchNumber == "")
                    dbManager.AddParameters(1, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_BATCH_NUMBER, BatchNumber);

                dbManager.AddParameters(2, PARM_BATCH_SUBMITTED_DATE, SubmittedDate);

                
                 dbManager.AddParameters(3, PARM_BATCH_SUBMITTED_BY, null);
               

                if (SubmitType == "")
                    dbManager.AddParameters(4, PARM_BATCH_SUBMIT_TYPE, null);
                else
                    dbManager.AddParameters(4, PARM_BATCH_SUBMIT_TYPE, MDVUtility.ToInt32(SubmitType));

                if (ClearingHouseId == 0)
                    dbManager.AddParameters(5, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_CLEARING_HOUSE_ID, ClearingHouseId);


                if (BatchStatus == "")
                    dbManager.AddParameters(6, PARM_BATCH_STATUS, null);
                else
                    dbManager.AddParameters(6, PARM_BATCH_STATUS, BatchStatus);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PER_PAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.SubmittedStatementBatch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (SubmittedById <= 0 || SubmittedById == null)
                    dbManager.AddParameters(11, PARM_BATCH_SUBMITTED_BY_Id, null);
                else
                    dbManager.AddParameters(11, PARM_BATCH_SUBMITTED_BY_Id, SubmittedById);

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_BATCH_SELECT, ds, ds.SubmittedStatementBatch.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::SearchPatientStatementsBatch", PROC_PATIENT_STATEMENT_BATCH_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientStatement InsertPatientStatementsBatch(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);

                dbManager.AddParameters(0, PARM_BATCH_ID, ds.SubmittedStatementBatch.BatchIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_BATCH_NUMBER, ds.SubmittedStatementBatch.BatchNumberColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_BATCH_SUBMITTED_DATE, ds.SubmittedStatementBatch.SubmittedDateColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(3, PARM_BATCH_SUBMITTED_BY, ds.SubmittedStatementBatch.SubmittedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_BATCH_SUBMIT_TYPE, ds.SubmittedStatementBatch.SubmitTypeColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(5, PARM_CLEARING_HOUSE_ID, ds.SubmittedStatementBatch.ClearingHouseIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, PARM_BATCH_TOTAL_PATIENTS, ds.SubmittedStatementBatch.TotalPatientsColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(7, PARM_BATCH_STATUS, ds.SubmittedStatementBatch.BatchStatusColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_BATCH_XML, ds.SubmittedStatementBatch.BatchXMLColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, PARM_ENTITY_ID, ds.SubmittedStatementBatch.EntityIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.SubmittedStatementBatch.IsActiveColumn.ColumnName, DbType.Byte);
                dbManager.AddParameters(11, PARM_CREATED_BY, ds.SubmittedStatementBatch.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, PARM_CREATED_ON, ds.SubmittedStatementBatch.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.SubmittedStatementBatch.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.SubmittedStatementBatch.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, PARM_ERROR_MESSAGE, ds.SubmittedStatementBatch.ErrorMessageColumn.ColumnName, DbType.String);

                ds = (DSPatientStatement)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_BATCH_INSERT, ds, ds.SubmittedStatementBatch.TableName);
                //  ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::InsertPatientStatementsBatch", PROC_PATIENT_STATEMENT_BATCH_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientStatement SearchPatientStatementsBatchDetail(long BatchId)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (BatchId <= 0)
                {
                    throw new Exception("Please Select a Batch");
                }
                else
                {
                    dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);
                }
                dbManager.AddParameters(1, PARM_RECORD_COUNT, 0);

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_BATCH_DETAIL_SELECT, ds, ds.PatientStatement.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::SearchPatientStatementsBatchDetail", PROC_STATEMENT_BATCH_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string ResubmitPatientStatementsBatch(SharedVariable sharedVariable, int BatchId) 
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);
                dbManager.AddParameters(1, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(sharedVariable.UserName));
                dbManager.AddParameters(2, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
              
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_STATEMENT_BATCH_RESUBMIT).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::ResubmitPatientStatementsBatch", PROC_PATIENT_STATEMENT_BATCH_RESUBMIT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

    }
}
