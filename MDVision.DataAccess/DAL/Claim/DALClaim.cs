using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Claim
{
    public class DALClaim
    {


        #region "Constructors"
        public DALClaim()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALClaim(SharedVariable SharedVariable)
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
        private const string PROC_PATIENT_CMAIL_SELECT = "Patient.sp_PatientClaimSelect";
        private const string PROC_BILLING_837_CMAIL_LOAD = "Billing.sp_837ClaimLoad";
        private const string PROC_BILLING_837_Header_LOAD = "Billing.sp_837HeaderLoad";
        private const string PROC_BILLING_837_SERVICE_LINE_LOAD = "Billing.837ServicesLineLoad";
        private const string PROC_CLAIM_SUBMIT_HISTORY = "System.sp_ClaimSubmitHistory";

        private const string PROC_CLAIM_SUBMISSION_INSERT = "Patient.sp_ClaimSubmissionErrorsInsert";
        private const string PROC_CLAIM_SUBMISSION_DELETE = "Patient.sp_ClaimSubmissionErrorsDelete";
        private const string PROC_CLAIM_SUBMISSION_SELECT = "Patient.sp_ClaimSubmissionErrorsSelect";
        #endregion

        #region "Parameters"
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_BATCH_NUMBER = "@BatchNumber";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_INSURANCE_ID = "@InsurancePlanId";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_BILLER_ID = "@BillerId";
        private const string PARM_SECONDARY_VISITS = "@SecondaryVisits";
        private const string PARM_RESUBMITTED_VISITS = "@ReSubmittedVisits";
        private const string PARM_SUBMITION_MOD = "@SubmitionMod";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_CLAIM_STATUS_ID = "@ClaimStatusId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SUBMIT_USER_ID = "@SubmitStatusUserId";
        private const string PARM_CLAIM_TYPE_ID = "@ClaimTypeId";
        private const string PARM_CLAIM_ERROR_ID = "@ClaimErroredId";
        private const string PARM_CLAIM_SUBMISSION_ERROR_ID = "@SubmissionErrorId";
        

        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_SUBMITTED_DATE = "@SubmitDate";
        private const string PARM_SUBMIT_TYPE = "@SubmitType";

        private const string PARM_ERROR_ID = "@ErrorId";
        private const string PARM_VISIT_IDs = "@VisitIds";
        private const string PARM_CLAIM_ERROR = "@ClaimError";
        private const string PARM_CREATED_BY = "@CreateBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_Submit_ON = "@CreatedOn";
        

        #endregion

        #region "Support Functions"
        private void CreateClaimSubmissionErrorParameters(IDBManager dbManager, DSVisits ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERROR_ID, ds.ClaimSubmissionError.ErrorIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERROR_ID, ds.ClaimSubmissionError.ErrorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_VISIT_ID, ds.ClaimSubmissionError.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CLAIM_ERROR, ds.ClaimSubmissionError.ClaimErrorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CREATED_BY, ds.ClaimSubmissionError.CreateByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ClaimSubmissionError.CreatedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Insert, delete, update and get Documents using dataset Functions"

        public DSVisits LoadPatientClaim(string AccountNumber, string FirstName, string LastName, long ProviderId, long BatchNumber, long FacilityId, long PracticeId, long PatientInsuranceId, DateTime? DOSForm, DateTime? DOSTo, long ClearingHouseId, long BillerId, string ClaimType, string BillType, int ClaimStatusId, string ClaimErroredId, string SubmitionMod, string ClaimNumber, int PageNumber, int RowspPage,string SubmissionErrorId,int submitUserId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(25);

                if (AccountNumber == "")
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, AccountNumber);

                if (FirstName == "")
                    dbManager.AddParameters(1, PARM_FIRST_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);

                if (LastName == "")
                    dbManager.AddParameters(2, PARM_LAST_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_LAST_NAME, LastName);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                if (BatchNumber == 0)
                    dbManager.AddParameters(4, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_BATCH_NUMBER, BatchNumber);

                if (FacilityId == 0)
                    dbManager.AddParameters(5, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_FACILITY_ID, FacilityId);

                if (PracticeId == 0)
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, PracticeId);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(7, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(7, PARM_INSURANCE_ID, PatientInsuranceId);


                dbManager.AddParameters(8, PARM_DOS_FROM, DOSForm);

                dbManager.AddParameters(9, PARM_DOS_TO, DOSTo);

                if (ClearingHouseId == 0)
                    dbManager.AddParameters(10, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(10, PARM_CLEARING_HOUSE_ID, ClearingHouseId);

                if (BillerId == 0)
                    dbManager.AddParameters(11, PARM_BILLER_ID, null);
                else
                    dbManager.AddParameters(11, PARM_BILLER_ID, BillerId);

                if (ClaimType == "")
                    dbManager.AddParameters(12, PARM_IS_PRIMARY, null);
                else
                    dbManager.AddParameters(12, PARM_IS_PRIMARY, ClaimType);

                //if (ReSubmittedVisits == "")
                //    dbManager.AddParameters(13, PARM_RESUBMITTED_VISITS, null);
                //else
                //    dbManager.AddParameters(13, PARM_RESUBMITTED_VISITS, ReSubmittedVisits);

                if (SubmitionMod == "")
                    dbManager.AddParameters(13, PARM_SUBMITION_MOD, null);
                else
                    dbManager.AddParameters(13, PARM_SUBMITION_MOD, SubmitionMod);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(14, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(14, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (ClaimStatusId == 0)
                    dbManager.AddParameters(15, PARM_CLAIM_STATUS_ID, null);
                else
                    dbManager.AddParameters(15, PARM_CLAIM_STATUS_ID, ClaimStatusId);


                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(16, PARM_CLAIM_NUMBER, ClaimNumber);


                if (PageNumber == 0)
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(19, PARM_RECORD_COUNT, ds.PatientClaims.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(20, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (string.IsNullOrEmpty(BillType))
                    dbManager.AddParameters(21, PARM_CLAIM_TYPE_ID, null);
                else
                    dbManager.AddParameters(21, PARM_CLAIM_TYPE_ID, BillType);

                if (string.IsNullOrEmpty(ClaimErroredId))
                    dbManager.AddParameters(22, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(22, PARM_CLAIM_ERROR_ID, ClaimErroredId);

                if (string.IsNullOrEmpty(SubmissionErrorId))
                    dbManager.AddParameters(23, PARM_CLAIM_SUBMISSION_ERROR_ID, null);
                else
                    dbManager.AddParameters(23, PARM_CLAIM_SUBMISSION_ERROR_ID, SubmissionErrorId);

                if (submitUserId == 0)
                    dbManager.AddParameters(24, PARM_SUBMIT_USER_ID, null);
                else
                    dbManager.AddParameters(24, PARM_SUBMIT_USER_ID, submitUserId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_CMAIL_SELECT, ds, ds.PatientClaims.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClaim::LoadPatientClaim", PROC_PATIENT_CMAIL_SELECT, ex);
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
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="AccountNumber"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="ProviderId"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="FacilityId"></param>
        /// <param name="PracticeId"></param>
        /// <param name="PatientInsuranceId"></param>
        /// <param name="DOSForm"></param>
        /// <param name="DOSTo"></param>
        /// <param name="ClearingHouseId"></param>
        /// <param name="BillerId"></param>
        /// <param name="ClaimType"></param>
        /// <param name="BillType"></param>
        /// <param name="ClaimStatusId"></param>
        /// <param name="ClaimErroredId"></param>
        /// <param name="SubmitionMod"></param>
        /// <param name="ClaimNumber"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowspPage"></param>
        /// <returns></returns>
        public DSVisits LoadPatientClaim(SharedVariable SharedVariable, string AccountNumber, string FirstName, string LastName, long ProviderId, long BatchNumber, long FacilityId, long PracticeId, long PatientInsuranceId, DateTime? DOSForm, DateTime? DOSTo, long ClearingHouseId, long BillerId, string ClaimType, string BillType, int ClaimStatusId, string ClaimErroredId, string SubmitionMod, string ClaimNumber, int PageNumber, int RowspPage)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(23);

                if (AccountNumber == "")
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, AccountNumber);

                if (FirstName == "")
                    dbManager.AddParameters(1, PARM_FIRST_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);

                if (LastName == "")
                    dbManager.AddParameters(2, PARM_LAST_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_LAST_NAME, LastName);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                if (BatchNumber == 0)
                    dbManager.AddParameters(4, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_BATCH_NUMBER, BatchNumber);

                if (FacilityId == 0)
                    dbManager.AddParameters(5, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_FACILITY_ID, FacilityId);

                if (PracticeId == 0)
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PRACTICE_ID, PracticeId);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(7, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(7, PARM_INSURANCE_ID, PatientInsuranceId);


                dbManager.AddParameters(8, PARM_DOS_FROM, DOSForm);

                dbManager.AddParameters(9, PARM_DOS_TO, DOSTo);

                if (ClearingHouseId == 0)
                    dbManager.AddParameters(10, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(10, PARM_CLEARING_HOUSE_ID, ClearingHouseId);

                if (BillerId == 0)
                    dbManager.AddParameters(11, PARM_BILLER_ID, null);
                else
                    dbManager.AddParameters(11, PARM_BILLER_ID, BillerId);

                if (ClaimType == "")
                    dbManager.AddParameters(12, PARM_IS_PRIMARY, null);
                else
                    dbManager.AddParameters(12, PARM_IS_PRIMARY, ClaimType);

                //if (ReSubmittedVisits == "")
                //    dbManager.AddParameters(13, PARM_RESUBMITTED_VISITS, null);
                //else
                //    dbManager.AddParameters(13, PARM_RESUBMITTED_VISITS, ReSubmittedVisits);

                if (SubmitionMod == "")
                    dbManager.AddParameters(13, PARM_SUBMITION_MOD, null);
                else
                    dbManager.AddParameters(13, PARM_SUBMITION_MOD, SubmitionMod);

                if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(14, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(14, PARM_ENTITY_ID, SharedVariable.EntityId);

                if (ClaimStatusId == 0)
                    dbManager.AddParameters(15, PARM_CLAIM_STATUS_ID, null);
                else
                    dbManager.AddParameters(15, PARM_CLAIM_STATUS_ID, ClaimStatusId);


                if (ClaimNumber == "")
                    ClaimNumber = null;
                dbManager.AddParameters(16, PARM_CLAIM_NUMBER, ClaimNumber);


                if (PageNumber == 0)
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(17, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(18, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(19, PARM_RECORD_COUNT, ds.PatientClaims.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(20, PARM_USER_ID, SharedVariable.AppUserId);

                if (string.IsNullOrEmpty(BillType))
                    dbManager.AddParameters(21, PARM_CLAIM_TYPE_ID, null);
                else
                    dbManager.AddParameters(21, PARM_CLAIM_TYPE_ID, BillType);

                if (string.IsNullOrEmpty(ClaimErroredId))
                    dbManager.AddParameters(22, PARM_CLAIM_ERROR_ID, null);
                else
                    dbManager.AddParameters(22, PARM_CLAIM_ERROR_ID, ClaimErroredId);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_CMAIL_SELECT, ds, ds.PatientClaims.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALClaim::LoadPatientClaim", PROC_PATIENT_CMAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVisits LoadClaimSubmitHistory(string AccountNumber, long ClearingHouseId, string BatchNumber, string ClaimNumber, string SubmitionMod, string SubmittedDate, int PageNumber, int RowspPage)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(11);

                if (string.IsNullOrEmpty(AccountNumber))
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_ACCOUNT_NUMBER, AccountNumber);

                if (ClearingHouseId == 0)
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, ClearingHouseId);

                if (string.IsNullOrEmpty(ClaimNumber))
                    dbManager.AddParameters(2, PARM_CLAIM_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_CLAIM_NUMBER, ClaimNumber);

                if (string.IsNullOrEmpty(SubmitionMod))
                    dbManager.AddParameters(3, PARM_SUBMIT_TYPE, null);
                else
                    dbManager.AddParameters(3, PARM_SUBMIT_TYPE, SubmitionMod);

                if (string.IsNullOrEmpty(SubmittedDate))
                    dbManager.AddParameters(4, PARM_SUBMITTED_DATE, null);
                else
                    dbManager.AddParameters(4, PARM_SUBMITTED_DATE, SubmittedDate);

                if (string.IsNullOrEmpty(BatchNumber))
                    dbManager.AddParameters(5, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_BATCH_NUMBER, BatchNumber);

                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.ClaimSubmitHistory.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMIT_HISTORY, ds, ds.ClaimSubmitHistory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClaim::LoadClaimSubmitHistory", PROC_CLAIM_SUBMIT_HISTORY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region "Lookups"
        #endregion

        #region "Insert, delete and get Functions For Claim Submission Error"
        
        public DSVisits LoadClaimSubmissionError(long ErrorId)
        {
            DSVisits ds = new DSVisits();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ErrorId == 0)
                    dbManager.AddParameters(0, PARM_ERROR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERROR_ID, ErrorId);
                ds = (DSVisits)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMISSION_SELECT, ds, ds.ClaimSubmissionError.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClaim::LoadClaimSubmissionError", PROC_CLAIM_SUBMISSION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteClaimSubmissionError(string ErrorIds, string VisitIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ErrorIds == "")
                    dbManager.AddParameters(0, PARM_ERROR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERROR_ID, ErrorIds);

                if (VisitIds == "")
                    dbManager.AddParameters(1, PARM_VISIT_IDs, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_IDs, VisitIds);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLAIM_SUBMISSION_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClaim::DeleteClaimSubmissionError", PROC_CLAIM_SUBMISSION_DELETE, ex);
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

        public string DeleteClaimSubmissionError(SharedVariable SharedVariable, string ErrorIds, string VisitIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ErrorIds == "")
                    dbManager.AddParameters(0, PARM_ERROR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERROR_ID, ErrorIds);

                if (VisitIds == "")
                    dbManager.AddParameters(1, PARM_VISIT_IDs, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_IDs, VisitIds);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLAIM_SUBMISSION_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALClaim::DeleteClaimSubmissionError", PROC_CLAIM_SUBMISSION_DELETE, ex);
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

        public DSVisits InsertClaimSubmissionError(SharedVariable SharedVariable, DSVisits ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                CreateClaimSubmissionErrorParameters(dbManager, ds, true);
                ds = (DSVisits)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLAIM_SUBMISSION_INSERT, ds, ds.ClaimSubmissionError.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALClaim::InsertClaimSubmissionError", PROC_CLAIM_SUBMISSION_INSERT, ex);

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
        #endregion
    }
}
