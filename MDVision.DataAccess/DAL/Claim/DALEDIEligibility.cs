using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using EDIParser;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Claim
{
    public class DALEDIEligibility
    {
        #region "Variable"

        #endregion

        #region "Constructors"
        public DALEDIEligibility()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALEDIEligibility(SharedVariable SharedVariable)
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

        #region "Stored Procedure Names"

        private const string PROC_BILLING_EDI_ELIGIBILITY_INSERT = "Billing.sp_EDIEligibilityInsert";
        private const string PROC_BILLING_EDI_ELIGIBILITY_DELETE = "Billing.sp_EDIEligibilityDelete";
        private const string PROC_BILLING_EDI_ELIGIBILITY_LOAD = "Billing.sp_EDIEligibilitySelect";
        private const string PROC_BILLING_EDI_ELIGIBILITY_LOAD_EXPORT = "Billing.sp_EDIEligibilitySelectForExport";
        private const string PROC_BILLING_EDI_ELIGIBILITY_UPDATE = "Billing.sp_EDIEligibilityUpdate";
        private const string PROC_BILLING_EDI_270_HEADER_LOAD = "Billing.sp_270HeaderLoad";
        private const string PROC_BILLING_EDI_270_NAMES_LOAD = "Billing.sp_270NamesLoad";
        private const string PROC_Provider_PatientEligibility_Service_Select = "Provider.sp_PatientEligibilityServiceSelect";

        #endregion

        #region "Parameters"

        private const string PARM_EDI_ELIGIBILITY_ID = "@EDIEligibilityId";
        private const string PARM_EDI_CONTROL_NUMBER = "@ControlNumber";
        private const string PARM_STR_270 = "@Str270";
        private const string PARM_STR_271 = "@Str271";
        private const string PARM_CLEARINGHOUSE_ID = "@ClearningHouseId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_SUBMITTER_ID = "@SubmitterId";
        private const string PARM_SUBMITTER_NAME = "@SubmitterName";
        private const string PARM_STATUS = "@Status";
        private const string PARM_PATIENT_INSURASNCE_ID = "@PatientInsuranceId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_EQ_SERVICE = "@EQSevice";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_DOS = "@DOS";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";

        private const string PARM_COPAY = "@Copay";
        private const string PARM_DEDUCTIBLE = "@Deductible";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ELIGIBILITY_FROM_DATE = "@EligibilityDateFrom";
        private const string PARM_ELIGIBILITY_TO_DATE = "@EligibilityDateTo";

        #endregion

        #region "Support Functions"

        private void CreateEDIEligibilityParameters(IDBManager dbManager, DSPatientEligibility ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(22);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, ds.EDIEligibility.EDIEligibilityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_EDI_CONTROL_NUMBER, ds.EDIEligibility.ControlNumberColumn.ColumnName, DbType.String, ParamDirection.Output, null, 20);
            }
            else
            {
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, ds.EDIEligibility.EDIEligibilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_EDI_CONTROL_NUMBER, ds.EDIEligibility.ControlNumberColumn.ColumnName, DbType.String);
            }

            dbManager.AddParameters(2, PARM_STR_270, ds.EDIEligibility.Str270Column.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_STR_271, ds.EDIEligibility.Str271Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CLEARINGHOUSE_ID, ds.EDIEligibility.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_PATIENT_ID, ds.EDIEligibility.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_SUBMITTER_ID, ds.EDIEligibility.SubmitterIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SUBMITTER_NAME, ds.EDIEligibility.SubmitterNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_STATUS, ds.EDIEligibility.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_COMMENTS, ds.EDIEligibility.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds.EDIEligibility.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.EDIEligibility.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.EDIEligibility.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.EDIEligibility.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.EDIEligibility.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_PATIENT_INSURASNCE_ID, ds.EDIEligibility.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_EQ_SERVICE, ds.EDIEligibility.EQSeviceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_PROVIDER_ID, ds.EDIEligibility.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

            dbManager.AddParameters(19, PARM_DOS, ds.EDIEligibility.DOSColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_COPAY, ds.EDIEligibility.CopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(21, PARM_DEDUCTIBLE, ds.EDIEligibility.DeductibleColumn.ColumnName, DbType.Double);

        }

        #endregion

        #region " EDI Eligibility "

        public DSPatientEligibility LoadEDIEligibility(long EDIEligibilityId, long PatientId, long InsurancePlanId, long ProviderId, DateTime? DOSFrom, DateTime? DOSTo, string EQSevice, string LastName, string FirstName, string Status, int PageNumber = 1, int RowspPage = 1000,DateTime? EligibiltyFrom=null, DateTime? EligibiltyTo=null)
        {

            DSPatientEligibility ds = new DSPatientEligibility();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);

                if (EDIEligibilityId == 0)
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);


                if (string.IsNullOrEmpty(EQSevice))
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, null);
                else
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, EQSevice);

                if (string.IsNullOrEmpty(LastName))
                    dbManager.AddParameters(6, PARM_LAST_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_LAST_NAME, LastName);

                if (string.IsNullOrEmpty(FirstName))
                    dbManager.AddParameters(7, PARM_FIRST_NAME, null);
                else
                    dbManager.AddParameters(7, PARM_FIRST_NAME, FirstName);

                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(8, PARM_STATUS, null);
                else
                    dbManager.AddParameters(8, PARM_STATUS, Status);

                dbManager.AddParameters(9, PARM_DOS_TO, DOSTo);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);
                if (EligibiltyFrom == null)
                    dbManager.AddParameters(12, PARM_ELIGIBILITY_FROM_DATE, DBNull.Value);
                else
                    dbManager.AddParameters(12, PARM_ELIGIBILITY_FROM_DATE, EligibiltyFrom);
                if (EligibiltyTo == null)
                    dbManager.AddParameters(13, PARM_ELIGIBILITY_TO_DATE, DBNull.Value);
                else
                    dbManager.AddParameters(13, PARM_ELIGIBILITY_TO_DATE, EligibiltyTo);

                dbManager.AddParameters(14, PARM_RECORD_COUNT, ds.EDIEligibility.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatientEligibility)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_LOAD, ds, ds.EDIEligibility.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::LoadEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatientEligibility LoadPatientEligibilityExport(long EDIEligibilityId, long PatientId, long InsurancePlanId, long ProviderId, DateTime? DOSFrom, DateTime? DOSTo, string EQSevice, string LastName, string FirstName, string Status)
        {

            DSPatientEligibility ds = new DSPatientEligibility();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);

                if (EDIEligibilityId == 0)
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);


                if (string.IsNullOrEmpty(EQSevice))
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, null);
                else
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, EQSevice);

                if (string.IsNullOrEmpty(LastName))
                    dbManager.AddParameters(6, PARM_LAST_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_LAST_NAME, LastName);

                if (string.IsNullOrEmpty(FirstName))
                    dbManager.AddParameters(7, PARM_FIRST_NAME, null);
                else
                    dbManager.AddParameters(7, PARM_FIRST_NAME, FirstName);

                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(8, PARM_STATUS, null);
                else
                    dbManager.AddParameters(8, PARM_STATUS, Status);

                dbManager.AddParameters(9, PARM_DOS_TO, DOSTo);

             

               

                ds = (DSPatientEligibility)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_LOAD_EXPORT, ds, ds.EDIEligibility.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("LoadPatientEligibilityExport::LoadPatientEligibilityExport", PROC_BILLING_EDI_ELIGIBILITY_LOAD_EXPORT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="EDIEligibilityId"></param>
        /// <param name="PatientId"></param>
        /// <param name="InsurancePlanId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="DOSFrom"></param>
        /// <param name="DOSTo"></param>
        /// <param name="EQSevice"></param>
        /// <param name="LastName"></param>
        /// <param name="FirstName"></param>
        /// <param name="Status"></param>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public DSPatientEligibility LoadEDIEligibility(long EDIEligibilityId, long PatientId, long InsurancePlanId, long ProviderId, DateTime? DOSFrom, DateTime? DOSTo, string EQSevice, string LastName, string FirstName, string Status, SharedVariable SharedVariable, int PageNumber = 1, int RowspPage = 1000)
        {

            DSPatientEligibility ds = new DSPatientEligibility();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);

                if (EDIEligibilityId == 0)
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                dbManager.AddParameters(4, PARM_DOS_FROM, DOSFrom);


                if (string.IsNullOrEmpty(EQSevice))
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, null);
                else
                    dbManager.AddParameters(5, PARM_EQ_SERVICE, EQSevice);

                if (string.IsNullOrEmpty(LastName))
                    dbManager.AddParameters(6, PARM_LAST_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_LAST_NAME, LastName);

                if (string.IsNullOrEmpty(FirstName))
                    dbManager.AddParameters(7, PARM_FIRST_NAME, null);
                else
                    dbManager.AddParameters(7, PARM_FIRST_NAME, FirstName);

                if (string.IsNullOrEmpty(Status))
                    dbManager.AddParameters(8, PARM_STATUS, null);
                else
                    dbManager.AddParameters(8, PARM_STATUS, Status);

                dbManager.AddParameters(9, PARM_DOS_TO, DOSTo);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.EDIEligibility.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSPatientEligibility)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_LOAD, ds, ds.EDIEligibility.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::LoadEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientEligibility InsertEDIEligibility(DSPatientEligibility ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                CreateEDIEligibilityParameters(dbManager, ds, true);
                ds = (DSPatientEligibility)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_INSERT, ds, ds.EDIEligibility.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::InsertEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSPatientEligibility InsertEDIEligibility(DSPatientEligibility ds, SharedVariable SharedVariable)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                CreateEDIEligibilityParameters(dbManager, ds, true);
                ds = (DSPatientEligibility)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_INSERT, ds, ds.EDIEligibility.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::InsertEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientEligibility UpdateEDIEligibility(DSPatientEligibility ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateEDIEligibilityParameters(dbManager, ds, false);
                ds = (DSPatientEligibility)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_UPDATE, ds, ds.EDIEligibility.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::UpdateEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <returns></returns>
        public DSPatientEligibility UpdateEDIEligibility(DSPatientEligibility ds, SharedVariable SharedVariable)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                CreateEDIEligibilityParameters(dbManager, ds, false);
                ds = (DSPatientEligibility)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_UPDATE, ds, ds.EDIEligibility.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::UpdateEDIEligibilityService", PROC_BILLING_EDI_ELIGIBILITY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteEDIEligibility(long EDIEligibilityId)
        {
            object returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_EDI_ELIGIBILITY_DELETE);
                if (returnValue != null)
                    throw new Exception(returnValue.ToString());

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::DeleteEDIEligibility", PROC_BILLING_EDI_ELIGIBILITY_DELETE, ex);
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

        public DS270 Load270Header(long EDIEligibilityId)
        {

            DS270 ds = new DS270();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);
                ds = (DS270)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_270_HEADER_LOAD, ds, ds.EDI270Header.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::Load270Header", PROC_BILLING_EDI_270_HEADER_LOAD, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="EDIEligibilityId"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <returns></returns>
        public DS270 Load270Header(long EDIEligibilityId, SharedVariable SharedVariable)
        {

            DS270 ds = new DS270();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);
                ds = (DS270)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_270_HEADER_LOAD, ds, ds.EDI270Header.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::Load270Header", PROC_BILLING_EDI_270_HEADER_LOAD, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="EDIEligibilityId"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <returns></returns>
        public DS270 Load270Names(long EDIEligibilityId, SharedVariable SharedVariable)
        {

            DS270 ds = new DS270();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);
                ds = (DS270)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_270_NAMES_LOAD, ds, ds.EDI270Names.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::Load270Names", PROC_BILLING_EDI_270_NAMES_LOAD, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DS270 Load270Names(long EDIEligibilityId)
        {

            DS270 ds = new DS270();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_EDI_ELIGIBILITY_ID, EDIEligibilityId);
                ds = (DS270)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_270_NAMES_LOAD, ds, ds.EDI270Names.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIEligibility::Load270Names", PROC_BILLING_EDI_270_NAMES_LOAD, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientEligibility LoadPatientEligibilityService(DateTime? DOS, SharedVariable SharedVariable)
        {

            DSPatientEligibility ds = new DSPatientEligibility();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_DOS_FROM, DOS);

                ds = (DSPatientEligibility)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Provider_PatientEligibility_Service_Select, ds, ds.PatientEligibilityService.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIEligibility::LoadPatientEligibilityService", PROC_Provider_PatientEligibility_Service_Select, ex);
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
