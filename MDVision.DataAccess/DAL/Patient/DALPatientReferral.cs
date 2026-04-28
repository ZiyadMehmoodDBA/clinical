using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientReferral
    {
        #region Variable
        
        #endregion

        #region Constructors

        public DALPatientReferral()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        public DALPatientReferral(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Stored Procedure Names

        private const string PROC_REFERRAL_SELECT = "Patient.sp_ReferralsSelect";
        private const string PROC_REFERRAL_SEARCH = "Patient.sp_ReferralsSearch";
        private const string PROC_REFERRAL_INSERT = "Patient.sp_ReferralsInsert";
        private const string PROC_REFERRAL_UPDATE = "Patient.sp_ReferralsUpdate";
        private const string PROC_REFERRAL_PROBLEM_SELECT = "Patient.sp_ReferralProblemsSelect";
        private const string PROC_REFERRAL_PROBLEM_DELETE = "Patient.sp_ReferralProblemsDelete";
        private const string PROC_REFERRAL_PROBLEM_INSERT = "Patient.sp_ReferralProblemsInsert";
        private const string PROC_REFERRAL_PROCEDURE_SELECT = "Patient.sp_ReferralProcedureSelect";
        private const string PROC_REFERRAL_PROCEDURE_INSERT = "Patient.sp_ReferralProcedureInsert";
        private const string PROC_REFERRAL_PROCEDURE_UPDATE = "Patient.sp_ReferralProcedureUpdate";
        private const string PROC_REFERRAL_PROCEDURE_DELETE = "Patient.sp_ReferralProcedureDelete";
        private const string PROC_REFERRAL_DELETE = "Patient.sp_ReferralsDelete";
        private const string PROC_DETACH_REFERRAL_FROM_NOTES = "Clinical.sp_DetachReferralFromNotes";
        private const string PROC_ATTACH_REFERRAL_FROM_NOTES = "Clinical.sp_AttachReferralWithNotes";
        private const string PROC_GET_LATEST_REFERRAL_BY_PATIENTI = "Clinical.sp_GetLatestReferralByPatientId";
        private const string PROC_REFERRAL_SELECT_FORSOAPTEXT = "Clinical.sp_ReferralSelectForSoapText";
        private const string PROC_PROBLEMLIST_SELECT_FOR_REFERRALS = "Clinical.sp_ProblemListSelectForReferrals";
        private const string PROC_NOTES_REFERRAL_SELECT = "[Clinical].[sp_NotesReferralSelect]";

        #endregion

        #region Parameter Names

        private const string PARM_REFERRAL_ID = "@ReferralId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_REF_PROVIDER_ID = "@RefProviderId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_PROCEDURENAME = "@CPTCodeDescription";
        private const string PARM_PAN = "@PAN";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FROM_DATE = "@DateFrom";
        private const string PARM_TO_DATE = "@DateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_TYPE = "@Type";
        private const string PARM_DATE= "@Date";
        private const string PARM_TIME = "@Time";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_REFERRAL_PROBLEM_ID = "@ReferralProblemId";
        private const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_REFERRAL_PROCEDURE_ID = "@ReferralProcedureId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_REASON = "@Reason";
        private const string PARM_VISIT = "@Visits";
        private const string PARM_DATE_FROM = "@DateFrom";
        private const string PARM_DATE_TO = "@DateTo";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsurance";
        private const string PARM_FACILITY_FROM_ID = "@FacilityFrom";
        private const string PARM_FACILITY_TO_ID = "@FacilityTo";
        private const string PARM_VISITS_USED_ID = "@VisitsUsed";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_TOSPECIALTYID = "@ToSpecialtyId";
        private const string PARM_IS_DRAFT = "@IsDraft";
        private const string PARM_SOURCE = "@Source";
        private const string PARM_STATUS_REASON_IDS = "@StatusReasonIds";
        #endregion

        #region Supporting Functions

        /// <summary>
        /// Method Name: createConsultationOrderParameters
        /// Author: Ahmad Raza
        /// Date: 17-03-2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateReferralInsertUpdateParameters(IDBManager dbManager, DSPatientReferral ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(29);
                dbManager.AddInsertUpdateParameters(0, PARM_REFERRAL_ID, ds.Referrals.ReferralIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(28);
                dbManager.AddInsertUpdateParameters(0, PARM_REFERRAL_ID, ds.Referrals.ReferralIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.Referrals.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_TYPE, ds.Referrals.TypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_DATE, ds.Referrals.DateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.Referrals.TimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_PROVIDER_ID, ds.Referrals.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_REF_PROVIDER_ID, ds.Referrals.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(7, PARM_ASSIGNEE_ID, ds.Referrals.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(8, PARM_STATUS, ds.Referrals.StatusColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_IS_ACTIVE, ds.Referrals.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(10, PARM_CREATED_BY, ds.Referrals.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CREATED_ON, ds.Referrals.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIED_BY, ds.Referrals.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_MODIFIED_ON, ds.Referrals.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_SOAP_TEXT, ds.Referrals.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_PAN, ds.Referrals.PANColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_REASON, ds.Referrals.ReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_VISIT, ds.Referrals.VisitsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_DATE_FROM, ds.Referrals.DateFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_DATE_TO, ds.Referrals.DateToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(20, PARM_PATIENT_INSURANCE_ID, ds.Referrals.PatientInsuranceColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(21, PARM_FACILITY_FROM_ID, ds.Referrals.FacilityFromColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_FACILITY_TO_ID, ds.Referrals.FacilityToColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(23, PARM_VISITS_USED_ID, ds.Referrals.VisitsUsedColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(24, PARM_COMMENTS, ds.Referrals.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(25, PARM_TOSPECIALTYID, ds.Referrals.ToSpecialtyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(26, PARM_IS_DRAFT, ds.Referrals.IsDraftColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(27, PARM_STATUS_REASON_IDS, ds.Referrals.StatusReasonsIdsColumn.ColumnName, DbType.String);

            if (isInsert == true)
            {
                dbManager.AddInsertUpdateParameters(28, PARM_NOTE_ID, ds.Referrals.NotesIdColumn.ColumnName, DbType.Int64);
            }
        }

        /// <summary>
        ///  Method Name: createReferralProblemParameters
        ///  Author: M Ahmad Imran
        ///  Created Date: 12-05-2016
        ///  Description: insert/update  Referral Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createReferralProblemParameters(IDBManager dbManager, DSPatientReferral ds, bool isInsert)
        {
            dbManager.CreateParameters(8);
            if (isInsert == true)
            {
                dbManager.AddParameters(0, PARM_REFERRAL_PROBLEM_ID, ds.ReferralProblems.ReferralProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(0, PARM_REFERRAL_PROBLEM_ID, ds.ReferralProblems.ReferralProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_REFERRAL_ID, ds.ReferralProblems.ReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PROBLEM_ID, ds.ReferralProblems.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ReferralProblems.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ReferralProblems.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ReferralProblems.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ReferralProblems.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ReferralProblems.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateReferralProcedureInsertUpdateParameters(IDBManager dbManager, DSPatientReferral ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_REFERRAL_PROCEDURE_ID, ds.ReferralProcedure.ReferralProcedureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_REFERRAL_PROCEDURE_ID, ds.ReferralProcedure.ReferralProcedureIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_REFERRAL_ID, ds.ReferralProcedure.ReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.ReferralProcedure.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_URGENCY_ID, ds.ReferralProcedure.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.ReferralProcedure.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.ReferralProcedure.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.ReferralProcedure.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.ReferralProcedure.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.ReferralProcedure.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region CRUD
        /// <summary>
        /// Method Name: loadConsultationOrderProblems
        /// Author: M Ahmad Imran Sheikh
        /// Created Date: 18-03-2016
        /// Description: loading Consultation Order Problems
        /// </summary> 
        /// <param name="consultationOrderId" type="long">consultationOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSPatientReferral loadReferral(long NoteId,string IsActive, long ReferralId, long patientId, string procedureName, long providerId,long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type,int visits, long patientInsurance, long facilityFrom, long facilityTo, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "",string IsDraft="",string Source = "", long AssigneeId = 0, string StatusReasonIds = "", string Date = "", string Time = "", long ToSpecialityId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(27);
               
                if (ReferralId == 0)
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rowsPerPage);

                ////Start 18-03-2016 Humaira Yousaf for Record Count
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Referrals.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ////End 18-03-2016 Humaira Yousaf for Record Count

                //Start 21-03-2016 Humaira Yousaf 

                if (procedureName == "" || procedureName == null)
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, null);
                else
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, procedureName);

                if (providerId == 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, providerId);
                if (RefproviderId == 0)
                    dbManager.AddParameters(14, PARM_REF_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(14, PARM_REF_PROVIDER_ID, RefproviderId);
                
                if (DateFrom == "" || DateFrom == null)
                    dbManager.AddParameters(7, PARM_FROM_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_FROM_DATE, Convert.ToDateTime(DateFrom));
                
                if (DateTo == "" || DateTo == null)
                    dbManager.AddParameters(8, PARM_TO_DATE, null);
                else
                    dbManager.AddParameters(8, PARM_TO_DATE, Convert.ToDateTime(DateTo));

                if (status == 0)
                    dbManager.AddParameters(9, PARM_STATUS, null);
                else
                    dbManager.AddParameters(9, PARM_STATUS, status);
                //End 21-03-2016 Humaira Yousaf 

                //Start 21-04-2016 Farooq Ahmad
                if (NoteId == 0)
                    dbManager.AddParameters(10, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(10, PARM_NOTE_ID, NoteId);

                if (Pan == "" || Pan == null)
                    dbManager.AddParameters(11, PARM_PAN, null);
                else
                    dbManager.AddParameters(11, PARM_PAN, Pan);
                if (Type == "")
                    dbManager.AddParameters(12, PARM_TYPE, null);
                else
                    dbManager.AddParameters(12, PARM_TYPE, Type);
                if (IsActive == "")
                    dbManager.AddParameters(13, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(13, PARM_IS_ACTIVE, (IsActive == "1" ? true : false));
                //Start 21-04-2016 Farooq Ahmad
				//Start 15-08-2016 Humaira Yousaf for new controls
                if (visits == 0)
                    dbManager.AddParameters(15, PARM_VISIT, null);
                else
                    dbManager.AddParameters(15, PARM_VISIT, visits);

                if (patientInsurance == 0)
                    dbManager.AddParameters(16, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(16, PARM_PATIENT_INSURANCE_ID, patientInsurance);

                if (facilityFrom == 0)
                    dbManager.AddParameters(17, PARM_FACILITY_FROM_ID, null);
                else
                    dbManager.AddParameters(17, PARM_FACILITY_FROM_ID, facilityFrom);

                if (facilityTo == 0)
                    dbManager.AddParameters(18, PARM_FACILITY_TO_ID, null);
                else
                    dbManager.AddParameters(18, PARM_FACILITY_TO_ID, facilityTo);

                dbManager.AddParameters(19, PARM_User_ID, MDVSession.Current.AppUserId);

                if (string.IsNullOrEmpty(IsDraft))
                    dbManager.AddParameters(20, PARM_IS_DRAFT, null);
                else
                    dbManager.AddParameters(20, PARM_IS_DRAFT, IsDraft);

                if (string.IsNullOrEmpty(Source))
                    dbManager.AddParameters(21, PARM_SOURCE, null);
                else
                    dbManager.AddParameters(21, PARM_SOURCE, Source);
                //End 15-08-2016 Humaira Yousaf for new controls
                if (AssigneeId == 0)
                    dbManager.AddParameters(22, PARM_ASSIGNEE_ID, null);
                else
                    dbManager.AddParameters(22, PARM_ASSIGNEE_ID, AssigneeId);

                if (string.IsNullOrEmpty(StatusReasonIds))
                    dbManager.AddParameters(23, PARM_STATUS_REASON_IDS, null);
                else
                    dbManager.AddParameters(23, PARM_STATUS_REASON_IDS, StatusReasonIds);

                if (string.IsNullOrEmpty(Date))
                    dbManager.AddParameters(24, PARM_DATE, null);
                else
                    dbManager.AddParameters(24, PARM_DATE, Convert.ToDateTime(Date));

                if (string.IsNullOrEmpty(Time))
                    dbManager.AddParameters(25, PARM_TIME, null);
                else
                    dbManager.AddParameters(25, PARM_TIME, Time);

                if (ToSpecialityId == 0)
                    dbManager.AddParameters(26, PARM_TOSPECIALTYID, null);
                else
                    dbManager.AddParameters(26, PARM_TOSPECIALTYID, ToSpecialityId);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_SELECT, ds, ds.Referrals.TableName);
                //if (ReferralId > 0)
                //{

                //    DataTable dtTemp = ds.Referrals;
                //    if (dtTemp != null)
                //    {
                //        if (isViewOrder == "1" || isPrintOrder == "1")
                //        {
                //            bool isViewAction = isViewOrder == "1" ? true : false;
                //            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Referrals.Rows[0][ds.Referrals.ReferralIdColumn].ToString(), null, ds.Referrals.Rows[0][ds.Referrals.ReferralIdColumn].ToString(), isViewAction, isPrintAcion);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }
                //}
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::loadConsultationOrder", PROC_REFERRAL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientReferral searchReferral(long NoteId, string IsActive,  long patientId, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
               
                dbManager.BeginTransaction();

                dbManager.CreateParameters(14);
                
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ReferralsSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (procedureName == "" || procedureName == null)
                    dbManager.AddParameters(4, PARM_PROCEDURENAME, null);
                else
                    dbManager.AddParameters(4, PARM_PROCEDURENAME, procedureName);

                if (providerId == 0)
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PROVIDER_ID, providerId);
                if (Pan == "" || Pan == null)
                    dbManager.AddParameters(6, PARM_PAN, null);
                else
                    dbManager.AddParameters(6, PARM_PAN, Pan);
              
                if (DateFrom == "" || DateFrom == null)
                    dbManager.AddParameters(7, PARM_FROM_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_FROM_DATE, Convert.ToDateTime(DateFrom));

                if (DateTo == "" || DateTo == null)
                    dbManager.AddParameters(8, PARM_TO_DATE, null);
                else
                    dbManager.AddParameters(8, PARM_TO_DATE, Convert.ToDateTime(DateTo));

                if (status == 0)
                    dbManager.AddParameters(9, PARM_STATUS, null);
                else
                    dbManager.AddParameters(9, PARM_STATUS, status);

                if (Type == "")
                    dbManager.AddParameters(10, PARM_TYPE, null);
                else
                    dbManager.AddParameters(10, PARM_TYPE, Type);

                if (IsActive == "")
                    dbManager.AddParameters(11, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(11, PARM_IS_ACTIVE, (IsActive == "1" ? true : false));

                if (RefproviderId == 0)
                    dbManager.AddParameters(12, PARM_REF_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(12, PARM_REF_PROVIDER_ID, RefproviderId);

                if (NoteId == 0)
                    dbManager.AddParameters(13, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(13, PARM_NOTE_ID, NoteId);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_SEARCH, ds, ds.ReferralsSearch.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatientReferral::searchReferral", PROC_REFERRAL_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: loadConsultationOrderProblems
        /// Author: M Ahmad Imran
        /// Created Date: 18-03-2016
        /// Description: loading Consultation Order Problems
        /// </summary> 
        public DSPatientReferral InsertUpdateReferral(DSPatientReferral ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DataTable dtTemp = ds.Referrals.GetChanges();
                dbManager.BeginTransaction();

                CreateReferralInsertUpdateParameters(dbManager, ds, true);
                CreateReferralInsertUpdateParameters(dbManager, ds, false);
                ds = (DSPatientReferral)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_REFERRAL_INSERT, PROC_REFERRAL_UPDATE, ds, ds.Referrals.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.Referrals.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.Referrals.Rows[i][ds.Referrals.ReferralIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Referrals.Rows[0][ds.Referrals.ReferralIdColumn].ToString(), null, ds.Referrals.Rows[0][ds.Referrals.ReferralIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::InsertUpdateReferral", PROC_REFERRAL_INSERT + " " + PROC_REFERRAL_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteReferralProblems
        /// Author: M Ahmad Imran
        /// Created Date: 12-05-2016
        /// Description: deleting Referral Problem
        /// </summary> 
        /// <param name="ReferralId" type="long">ReferralId to be deleted</param>
        /// 

        public string deleteReferralProblems(long ReferralId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //  dbManager.Open();
                dbManager.BeginTransaction();
                DSPatientReferral dsReferralProblems = loadReferralProblems(0, Convert.ToInt64(ReferralId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REFERRAL_PROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    //DataTable dtTemp = dsReferralProblems.ReferralProblems;
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsReferralProblems.ReferralProblems.Rows[0].ToString(), null, dsReferralProblems.ReferralProblems.Rows[0][dsReferralProblems.ReferralProblems.ReferralIdColumn].ToString(), false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::deleteReferralProblems", PROC_REFERRAL_PROBLEM_DELETE, ex);
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

        /// <summary>
        /// Method Name: loadReferralProblems
        /// Author: M Ahmad Imran
        /// Created Date: 13-05-2016
        /// Description: loading Referral Problems
        /// </summary> 
        /// <param name="consultationOrderId" type="long">ReferralId to get Problems</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSPatientReferral loadReferralProblems(long ReferralProblemId, long ReferralId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (ReferralId == 0)
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);

                if (ReferralProblemId == 0)
                    dbManager.AddParameters(1, PARM_REFERRAL_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_REFERRAL_PROBLEM_ID, ReferralProblemId);

                if (patientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ReferralProblems.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_PROBLEM_SELECT, ds, ds.ReferralProblems.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::loadReferralProblems", PROC_REFERRAL_PROBLEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: insertUpdateReferralProblems
        /// Author: M Ahmad Imran
        /// Created Date: 13-05-2016
        /// Description: insert/update  Referral Problems
        /// </summary> 
        /// <param name="DSConsultationOrder" type="DATASET"></param>
        public DSPatientReferral insertReferralProblems(DSPatientReferral ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.ReferralProblems.GetChanges();
                dbManager.BeginTransaction();
                createReferralProblemParameters(dbManager, ds, true);
                //createReferralProblemParameters(dbManager, ds, false);
                ds = (DSPatientReferral)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REFERRAL_PROBLEM_INSERT, ds, ds.ReferralProblems.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ReferralProblems.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ReferralProblems.Rows[i][ds.ReferralProblems.ReferralProblemIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ReferralProblems.Rows[0][ds.ReferralProblems.ReferralProblemIdColumn].ToString(), null, ds.ReferralProblems.Rows[0][ds.ReferralProblems.ReferralIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::insertUpdateReferralProblems", PROC_REFERRAL_PROBLEM_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: LoadReferralProcedure
        /// Author: M Ahmad Imran
        /// Created Date: 13-05-2016
        /// Description: Load  Referral Procedure
        /// </summary> 
        /// <param name="DSConsultationOrder" type="DATASET"></param>
        public DSPatientReferral LoadReferralProcedure(long RefrralId, long RefrralProcedureId, long patientId, long ProviderId, string pageNumber, string rowsPerPage)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                int page;
                int rpp;
                if (string.IsNullOrEmpty(pageNumber))
                {
                    page = 1;
                }
                else
                {
                    page = Convert.ToInt32(pageNumber);
                }

                if (string.IsNullOrEmpty(rowsPerPage))
                {
                    rpp = 2000;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (RefrralId == 0)
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, RefrralId);

                if (RefrralProcedureId == 0)
                    dbManager.AddParameters(1, PARM_REFERRAL_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_REFERRAL_PROCEDURE_ID, RefrralProcedureId);

                if (patientId <= 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                if (page <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ReferralProcedure.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ProviderId == 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, ProviderId);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_PROCEDURE_SELECT, ds, ds.ReferralProcedure.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::LoadReferralProcedure", PROC_REFERRAL_PROCEDURE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientReferral insertUpdateReferralProcedure(DSPatientReferral ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.ReferralProcedure.GetChanges();
                dbManager.BeginTransaction();

                CreateReferralProcedureInsertUpdateParameters(dbManager, ds, true);
                CreateReferralProcedureInsertUpdateParameters(dbManager, ds, false);
                ds = (DSPatientReferral)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_REFERRAL_PROCEDURE_INSERT, PROC_REFERRAL_PROCEDURE_UPDATE, ds, ds.ReferralProcedure.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ReferralProcedure.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ReferralProcedure.Rows[i][ds.ReferralProcedure.ReferralProcedureIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ReferralProcedure.Rows[0][ds.ReferralProcedure.ReferralProcedureIdColumn].ToString(), null, ds.ReferralProcedure.Rows[0][ds.ReferralProcedure.ReferralIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::insertUpdateReferralProcedure", PROC_REFERRAL_PROCEDURE_INSERT + " " + PROC_REFERRAL_PROCEDURE_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteReferralProcedure(long ReferralProcedureId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSPatientReferral dsCurrentReferralProcedure = LoadReferralProcedure(0, Convert.ToInt32(ReferralProcedureId), 0, 0, "1", "100");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REFERRAL_PROCEDURE_ID, ReferralProcedureId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REFERRAL_PROCEDURE_DELETE);
                

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    //DataTable dtTemp = dsCurrentReferralProcedure.ReferralProcedure;
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ReferralProcedureId.ToString(), null, dsCurrentReferralProcedure.ReferralProcedure.Rows[0][dsCurrentReferralProcedure.ReferralProcedure.ReferralIdColumn].ToString(), false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALConsultationOrder::deleteReferralProcedure", PROC_REFERRAL_PROCEDURE_DELETE, ex);
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



        public DSProblemLists LoadProblemLists(long PatientId , int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
               

                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(4);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
             
                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);



                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_FOR_REFERRALS, ds, ds.ProblemList.TableName);
                
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT_FOR_REFERRALS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteReferral(string ReferralIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REFERRAL_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    //DataTable dtTemp = dsCurrentReferralProcedure.ReferralProcedure;
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ReferralProcedureId.ToString(), null, dsCurrentReferralProcedure.ReferralProcedure.Rows[0][dsCurrentReferralProcedure.ReferralProcedure.ReferralIdColumn].ToString(), false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                  //  dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatientReferral::deleteReferralProcedure", PROC_REFERRAL_DELETE, ex);
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


        public DSPatientReferral loadReferral(long NoteId, string IsActive, long ReferralId, long patientId, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int visits, long patientInsurance, long facilityFrom, long facilityTo, long userId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();              
                dbManager.CreateParameters(20);

                if (ReferralId == 0)
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rowsPerPage);
            
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Referrals.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                
                if (procedureName == "" || procedureName == null)
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, null);
                else
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, procedureName);

                if (providerId == 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, providerId);
                if (RefproviderId == 0)
                    dbManager.AddParameters(14, PARM_REF_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(14, PARM_REF_PROVIDER_ID, RefproviderId);

                if (DateFrom == "" || DateFrom == null)
                    dbManager.AddParameters(7, PARM_FROM_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_FROM_DATE, Convert.ToDateTime(DateFrom));

                if (DateTo == "" || DateTo == null)
                    dbManager.AddParameters(8, PARM_TO_DATE, null);
                else
                    dbManager.AddParameters(8, PARM_TO_DATE, Convert.ToDateTime(DateTo));

                if (status == 0)
                    dbManager.AddParameters(9, PARM_STATUS, null);
                else
                    dbManager.AddParameters(9, PARM_STATUS, status);

                if (NoteId == 0)
                    dbManager.AddParameters(10, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(10, PARM_NOTE_ID, NoteId);

                if (Pan == "" || Pan == null)
                    dbManager.AddParameters(11, PARM_PAN, null);
                else
                    dbManager.AddParameters(11, PARM_PAN, Pan);
                if (Type == "")
                    dbManager.AddParameters(12, PARM_TYPE, null);
                else
                    dbManager.AddParameters(12, PARM_TYPE, Type);
                if (IsActive == "")
                    dbManager.AddParameters(13, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(13, PARM_IS_ACTIVE, (IsActive == "1" ? true : false));
                
                if (visits == 0)
                    dbManager.AddParameters(15, PARM_VISIT, null);
                else
                    dbManager.AddParameters(15, PARM_VISIT, visits);

                if (patientInsurance == 0)
                    dbManager.AddParameters(16, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(16, PARM_PATIENT_INSURANCE_ID, patientInsurance);

                if (facilityFrom == 0)
                    dbManager.AddParameters(17, PARM_FACILITY_FROM_ID, null);
                else
                    dbManager.AddParameters(17, PARM_FACILITY_FROM_ID, facilityFrom);

                if (facilityTo == 0)
                    dbManager.AddParameters(18, PARM_FACILITY_TO_ID, null);
                else
                    dbManager.AddParameters(18, PARM_FACILITY_TO_ID, facilityTo);

                dbManager.AddParameters(19, PARM_User_ID, userId);
            
                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_SELECT, ds, ds.Referrals.TableName);
                
                return ds;
            }
            catch (Exception ex)
            {                              
                MDVLogger.DALErrorLog("DALPatientReferrals::loadReferral", PROC_REFERRAL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Notes and Referrals
        /// <summary>
        /// Detach Referral With Progress notes
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachReferralsFromNotes(string ReferralId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(ReferralId))
                {
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_REFERRAL_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::detachReferralsFromNotes", PROC_DETACH_REFERRAL_FROM_NOTES, ex);
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
        /// <summary>
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSPatientReferral attachReferralWithNotes(string ReferralId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSPatientReferral ds = new DSPatientReferral();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(ReferralId))
                {
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_REFERRAL_FROM_NOTES, ds, ds.Referrals.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachReferralWithNotes", PROC_ATTACH_REFERRAL_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientReferral getLatestReferralByPatientId(long PatientId, long ProviderId)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderId);

                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_REFERRAL_BY_PATIENTI, ds, ds.Referrals.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemList::getLatestReferralByPatientId", PROC_GET_LATEST_REFERRAL_BY_PATIENTI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Referral For Notes Soap"
        /// <summary>
        /// Get Referral Soap Text DataSet
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public DSPatientReferral loadReferralForSoap(string ReferralId, long PatientId, long ProviderId)
        {
            DSPatientReferral ds = new DSPatientReferral();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (string.IsNullOrEmpty(ReferralId))
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, ReferralId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);


                ds = (DSPatientReferral)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRAL_SELECT_FORSOAPTEXT, ds, ds.Referrals.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::loadReferralForSoap", PROC_REFERRAL_SELECT_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Legacy Notes

        public List<Referrals> NotesReferralsSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<Referrals> objList_Referrals = new List<Referrals>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_REFERRAL_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        Referrals model = new Referrals();
                        var properties = typeof(Referrals).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_Referrals.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferral::NotesReferralsSelect", PROC_NOTES_REFERRAL_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_Referrals;
        }

        #endregion Legacy Notes

    }
}
