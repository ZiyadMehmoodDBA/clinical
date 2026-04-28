using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.FollowUp
{
    public class DALInsuranceAR
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_INSURANCE_AR_SELECT = "Billing.sp_InsuranceARDetailSelect";
        private const string PROC_INSURANCE_AR_DETAIL_SELECT = "Billing.sp_new_InsuranceARDetailSelect";
        private const string PROC_INSURANCE_AR_INSERT = "Billing.sp_InsuranceARDetailInsert";
        private const string PROC_INSURANCE_AR_UPDATE = "Billing.sp_InsuranceARDetailUpdate";
        private const string PROC_INSURANCE_AR_DELETE = "Billing.sp_InsuranceARDetailDelete";

        #endregion

        #region "Parameters"
        private const string PARM_FOLLOWUP_INS_AR_DETAIL_ID = "@InsARDetailId";
        private const string PARM_AR_GROUP_ID = "@ARGroupId";
        private const string PARM_ACTION_ID = "@FollowupActionId";
        private const string PARM_REASON_ID = "@FollowupReasonId";
        private const string PARM_REMIT_CODE_ID = "@RemittanceId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_SUSPEND_DATE = "@SuspendDate";
        private const string PARM_COMMENTS = "@Comments";

        private const string PARM_SUSPENDED_TILL = "@SuspendedTill";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_PAT_SSN = "@PatSSN";
        private const string PARM_PAT_LAST_NAME = "@LastName";
        private const string PARM_PAT_FIRST_NAME = "@FirstName";
        private const string PARM_PAT_MI = "@PatMI";
        private const string PARM_PAT_DOB = "@PatDOB";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_TAX_ID = "@TaxId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_INSURANCE_PHONE = "@InsurancePhone";
        private const string PARM_INS_SSN = "@InsSSN";
        //private const string PARM_INS_LAST_NAME = "@InsLastName";
        //private const string PARM_INS_FIRST_NAME = "@InsFirstName";
        //  private const string PARM_INS_MI = "@InsMI";
        private const string PARM_SUBSCRIBER_ID = "@SubscriberId";
        private const string PARM_GROUP_ID = "@GroupId";
        private const string PARM_ELIGIBILITY_DATE = "@EligibilityDate";
        private const string PARM_ELIGIBILITY_STATUS = "@EligibilityStatus";
        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_ICN_DCN = "@ICNDCN";
        private const string PARM_FIRST_STATEMENT_DATE = "@FirstStatementDate";
        private const string PARM_STATEMENT_DATE = "@StatementDate";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_UDATE_COMMENTS_DATE = "@UpdateCommentsDate";


        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_TYPE = "@Type";
        private const string PARM_SUSPENDED = "@Suspended";
        private const string PARM_AGE = "@Age";
        private const string PARM_CLAIM_TYPE = "@ClaimType";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_NAME_INITIAL_FROM = "@NameInitialFrom";
        private const string PARM_NAME_INITIAL_TO = "@NameInitialTo";
        private const string PARM_BAL_GREATER = "@InsBalFrom";
        private const string PARM_BAL_LESS = "@InsBalTo";
        private const string PARM_PLAN_CATEGORY = "@PlanCategory";

        private const string PARM_MODULE = "@Module";

        #endregion


        #region Constructors
        public DALInsuranceAR()
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
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, ds.FollowUpARDetail.FolUpARDtlIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, ds.FollowUpARDetail.FolUpARDtlIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_AR_GROUP_ID, ds.FollowUpARDetail.ARGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ACTION_ID, ds.FollowUpARDetail.FollowupActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_REASON_ID, ds.FollowUpARDetail.FollowupReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_REMIT_CODE_ID, ds.FollowUpARDetail.RemittanceIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_VISIT_ID, ds.FollowUpARDetail.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_SUSPEND_DATE, ds.FollowUpARDetail.SuspendDateColumn.ColumnName, DbType.DateTime);


            //dbManager.AddParameters(7, PARM_SUSPENDED_TILL, ds.FollowUpARDetail.SuspendedTillColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.FollowUpARDetail.CommentsColumn.ColumnName, DbType.String);

            //dbManager.AddParameters(9, PARM_ACCOUNT_NUMBER, ds.FollowUpARDetail.AccountNumberColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(10, PARM_PAT_SSN, ds.FollowUpARDetail.PatSSNColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(11, PARM_PAT_LAST_NAME, ds.FollowUpARDetail.PatLastNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(12, PARM_PAT_FIRST_NAME, ds.FollowUpARDetail.PatFirstNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(13, PARM_PAT_MI, ds.FollowUpARDetail.PatMIColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(14, PARM_PAT_DOB, ds.FollowUpARDetail.PatDOBColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(15, PARM_PROVIDER_ID, ds.FollowUpARDetail.ProviderIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(16, PARM_FACILITY_ID, ds.FollowUpARDetail.FacilityIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(17, PARM_PRACTICE_ID, ds.FollowUpARDetail.PracticeIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(18, PARM_TAX_ID, ds.FollowUpARDetail.TaxIdColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(19, PARM_INSURANCE_PLAN_ID, ds.FollowUpARDetail.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(20, PARM_INSURANCE_PHONE, ds.FollowUpARDetail.InsurancePhoneColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(21, PARM_INS_SSN, ds.FollowUpARDetail.InsSSNColumn.ColumnName, DbType.String);

            ////dbManager.AddParameters(22, PARM_PAT_LAST_NAME, ds.FollowUpARDetail.InsLastNameColumn.ColumnName, DbType.String);
            ////dbManager.AddParameters(23, PARM_PAT_FIRST_NAME, ds.FollowUpARDetail.InsFirstNameColumn.ColumnName, DbType.String);

            //dbManager.AddParameters(22, PARM_INS_MI, ds.FollowUpARDetail.InsMIColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(23, PARM_SUBSCRIBER_ID, ds.FollowUpARDetail.SubscriberIdColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(24, PARM_GROUP_ID, ds.FollowUpARDetail.GroupIdColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(25, PARM_ELIGIBILITY_DATE, ds.FollowUpARDetail.EligibilityDateColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(26, PARM_ELIGIBILITY_STATUS, ds.FollowUpARDetail.EligibilityStatusColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(27, PARM_CLAIM_NUMBER, ds.FollowUpARDetail.ClaimNumberColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(28, PARM_ICN_DCN, ds.FollowUpARDetail.ICNDCNColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(29, PARM_FIRST_STATEMENT_DATE, ds.FollowUpARDetail.FirstStatementDateColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(30, PARM_STATEMENT_DATE, ds.FollowUpARDetail.StatementDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.FollowUpARDetail.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.FollowUpARDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.FollowUpARDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.FollowUpARDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.FollowUpARDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            
        
    }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadFollowUpARDetail(Int64 FollowUpARDetailID, Int64 VisitId, string PatientAccount, long ProviderId, long FacilityId, string ClaimNumber, DateTime? DOSFrom, DateTime? DOSTo, long groupId, long ActionId, long ReasonId, long InsurancePlanId, string LastName, string FirstName, string suspended, long Age, long ClaimType, string ARType, int PageNumber, int RowspPage, string Module = "")
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;

                dbManager.Open();
                dbManager.CreateParameters(23);


                if (FollowUpARDetailID == 0)
                    dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, FollowUpARDetailID);

                if (VisitId == 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);



                if (PatientAccount == "")
                    dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, PatientAccount);


                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (ClaimNumber == "")
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, ClaimNumber);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (groupId == 0)
                    dbManager.AddParameters(8, PARM_GROUP_ID, null);
                else
                    dbManager.AddParameters(8, PARM_GROUP_ID, groupId);

                if (ActionId == 0)
                    dbManager.AddParameters(9, PARM_ACTION_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ACTION_ID, ActionId);

                if (ReasonId == 0)
                    dbManager.AddParameters(10, PARM_REASON_ID, null);
                else
                    dbManager.AddParameters(10, PARM_REASON_ID, ReasonId);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(11, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(11, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                if (LastName == "")
                    dbManager.AddParameters(12, PARM_PAT_LAST_NAME, null);
                else
                    dbManager.AddParameters(12, PARM_PAT_LAST_NAME, LastName);

                if (FirstName == "")
                    dbManager.AddParameters(13, PARM_PAT_FIRST_NAME, null);
                else
                    dbManager.AddParameters(13, PARM_PAT_FIRST_NAME, FirstName);

                if (suspended == "")
                    dbManager.AddParameters(14, PARM_SUSPENDED, null);
                else
                    dbManager.AddParameters(14, PARM_SUSPENDED, suspended);

                if (Age == 0)
                    dbManager.AddParameters(15, PARM_AGE, null);
                else
                    dbManager.AddParameters(15, PARM_AGE, Age);


                //if (ARType == "")
                //    dbManager.AddParameters(13, PARM_TYPE, null);
                //else
                //    dbManager.AddParameters(13, PARM_TYPE, ARType);

                if (PageNumber == 0)
                    dbManager.AddParameters(16, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(16, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(17, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(17, PARM_ROWS_PERPAGE, RowspPage);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(18, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(18, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(19, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClaimType == 0)
                    dbManager.AddParameters(20, PARM_CLAIM_TYPE, null);
                else
                    dbManager.AddParameters(20, PARM_CLAIM_TYPE, ClaimType);

                dbManager.AddParameters(21, PARM_RECORD_COUNT, ds.FollowUpARDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(22, PARM_MODULE, Module);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_AR_SELECT, ds, ds.FollowUpARDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::LoadFollowUpAR", PROC_INSURANCE_AR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFollowUp LoadFollowUpARDetailSelect(Int64 FollowUpARDetailID, Int64 VisitId, string PatientAccount, long ProviderId, long FacilityId, string ClaimNumber, DateTime? DOSFrom, DateTime? DOSTo, long groupId, long ActionId, long ReasonId, long InsurancePlanId, string LastName, string FirstName, string suspended, long Age, long ClaimType,string NameInitialTo, string NameInitialFrom, double InsBalGreater, double InsBalLess, long PlanCategory, string ARType, int PageNumber, int RowspPage, string Module = "",string LastModified="",string LastComment="")
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Module == "")
                    Module = null;

                dbManager.Open();
                dbManager.CreateParameters(30);


                if (FollowUpARDetailID == 0)
                    dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, FollowUpARDetailID);

                if (VisitId == 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);



                if (PatientAccount == "")
                    dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_ACCOUNT_NUMBER, PatientAccount);


                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);

                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);

                if (ClaimNumber == "")
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_CLAIM_NUMBER, ClaimNumber);

                dbManager.AddParameters(6, PARM_DOS_FROM, DOSFrom);
                dbManager.AddParameters(7, PARM_DOS_TO, DOSTo);

                if (groupId == 0)
                    dbManager.AddParameters(8, PARM_GROUP_ID, null);
                else
                    dbManager.AddParameters(8, PARM_GROUP_ID, groupId);

                if (ActionId == 0)
                    dbManager.AddParameters(9, PARM_ACTION_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ACTION_ID, ActionId);

                if (ReasonId == 0)
                    dbManager.AddParameters(10, PARM_REASON_ID, null);
                else
                    dbManager.AddParameters(10, PARM_REASON_ID, ReasonId);

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(11, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(11, PARM_INSURANCE_PLAN_ID, InsurancePlanId);

                if (LastName == "")
                    dbManager.AddParameters(12, PARM_PAT_LAST_NAME, null);
                else
                    dbManager.AddParameters(12, PARM_PAT_LAST_NAME, LastName);

                if (FirstName == "")
                    dbManager.AddParameters(13, PARM_PAT_FIRST_NAME, null);
                else
                    dbManager.AddParameters(13, PARM_PAT_FIRST_NAME, FirstName);

                if (suspended == "")
                    dbManager.AddParameters(14, PARM_SUSPENDED, null);
                else
                    dbManager.AddParameters(14, PARM_SUSPENDED, suspended);

                if (Age == 0)
                    dbManager.AddParameters(15, PARM_AGE, null);
                else
                    dbManager.AddParameters(15, PARM_AGE, Age);


                //if (ARType == "")
                //    dbManager.AddParameters(13, PARM_TYPE, null);
                //else
                //    dbManager.AddParameters(13, PARM_TYPE, ARType);

                if (PageNumber == 0)
                    dbManager.AddParameters(16, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(16, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(17, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(17, PARM_ROWS_PERPAGE, RowspPage);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(18, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(18, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(19, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClaimType == 0)
                    dbManager.AddParameters(20, PARM_CLAIM_TYPE, null);
                else
                    dbManager.AddParameters(20, PARM_CLAIM_TYPE, ClaimType);

                dbManager.AddParameters(21, PARM_RECORD_COUNT, ds.FollowUpARDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(22, PARM_MODULE, Module);

                if (NameInitialFrom == "")
                    dbManager.AddParameters(23, PARM_NAME_INITIAL_FROM, null);
                else
                    dbManager.AddParameters(23, PARM_NAME_INITIAL_FROM, NameInitialFrom);

                if (NameInitialTo ==  "")
                    dbManager.AddParameters(24, PARM_NAME_INITIAL_TO, null);
                else
                    dbManager.AddParameters(24, PARM_NAME_INITIAL_TO, NameInitialTo);

                if (InsBalGreater == 0)
                    dbManager.AddParameters(25, PARM_BAL_GREATER, null);
                else
                    dbManager.AddParameters(25, PARM_BAL_GREATER, InsBalGreater);

                if (InsBalLess == 0)
                    dbManager.AddParameters(26, PARM_BAL_LESS, null);
                else
                    dbManager.AddParameters(26, PARM_BAL_LESS, InsBalLess);

                if (PlanCategory == 0)
                    dbManager.AddParameters(27, PARM_PLAN_CATEGORY, null);
                else
                    dbManager.AddParameters(27, PARM_PLAN_CATEGORY, PlanCategory);
                if(!string.IsNullOrEmpty(LastModified))
                    dbManager.AddParameters(28, PARM_MODIFIED_ON, LastModified);
                else
                    dbManager.AddParameters(28, PARM_MODIFIED_ON, null);
                if (!string.IsNullOrEmpty(LastComment))
                    dbManager.AddParameters(29, PARM_COMMENTS, LastComment);
                else
                    dbManager.AddParameters(29, PARM_COMMENTS, null);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_AR_DETAIL_SELECT, ds, ds.FollowUpARDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::LoadFollowUpARDetailSelect", PROC_INSURANCE_AR_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFollowUp InsertFollowUpARDetail(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSURANCE_AR_INSERT, ds, ds.FollowUpARDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::InsertFollowUpARDetail", PROC_INSURANCE_AR_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateFollowUpARDetail(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.FollowUpARDetail.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_INSURANCE_AR_UPDATE, ds, ds.FollowUpARDetail.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowUpARDetail.Rows[0][ds.FollowUpARDetail.FolUpARDtlIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::UpdateFollowUpARDetail", PROC_INSURANCE_AR_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteFollowUpARDetail(Int64 followUpARDetailID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FOLLOWUP_INS_AR_DETAIL_ID, followUpARDetailID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSURANCE_AR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::DeleteFollowUpARDetail", PROC_INSURANCE_AR_DELETE, ex);
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
