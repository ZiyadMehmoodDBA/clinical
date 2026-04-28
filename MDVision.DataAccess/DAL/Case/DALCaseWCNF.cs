using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Case
{
    public class DALCaseWCNF
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALDocument"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALCaseWCNF()
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

        #region "Stored Procedure Names"

        private const string PROC_CASE_MANAGEMENT_WCNF_DELETE = "Patient.sp_CaseManagementWCNFDelete";
        private const string PROC_CASE_MANAGEMENT_WCNF_INSERT = "Patient.sp_CaseManagementWCNFInsert";
        private const string PROC_CASE_MANAGEMENT_WCNF_SELECT = "Patient.sp_CaseManagementWCNFSelect";
        private const string PROC_CASE_MANAGEMENT_WCNF_UPDATE = "Patient.sp_CaseManagementWCNFUpdate";
        private const string PROC_CASE_MANAGEMENT_WCNF_SELECT_VISIT_RECORD = "Patient.sp_CaseManagementWCNF_SelectVisitRecord";
        private const string PROC_CASE_TYPE_LOOKUP = "Patient.sp_CaseTypeLookup";
        private const string PROC_CONDITION_CODES_LOOKUP = "Patient.sp_ConditionCodesLookup";
        private const string PROC_CASE_LOOKUP = "Patient.sp_CaseManagementLoopkup";

        #endregion

        #region Parameters
        private const string PARM_CASE_WCNF_ID = "@WCNFDetailId";
        private const string PARM_CASE_MGMT_ID = "@CaseMgmtId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_CASE_ADJUSTER_ID = "@CaseAdjusterId";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_FAX = "@Fax";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_PRE_AUTH = "@PreAuth";
        private const string PARM_INJURY_DATE = "@InjuryDate";
        private const string PARM_REFERRAL = "@Referral";
        private const string PARM_EMPLOYMENT_RELATE = "@EmploymentRelate";
        private const string PARM_CAUSE_OF_ACCIDENT = "@CauseOfAccident";
        private const string PARM_ACCIDENT = "@Accident";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP = "@Zip";
        private const string PARM_HOUR = "@Hour";
        private const string PARM_HCFA_FIELD16DATE_FROM = "@HCFAField16DateFrom";
        private const string PARM_HCFA_FIELD16DATE_TO = "@HCFAField16DateTo";
        private const string PARM_HCFA_FIELD18DATE_FROM = "@HCFAField18DateFrom";
        private const string PARM_HCFA_FIELD18DATE_TO = "@HCFAField18DateTo";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CASE_NUMBER = "@CaseNumber";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSWCNF ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(25);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CASE_WCNF_ID, ds.WCNFDetail.WCNFDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CASE_WCNF_ID, ds.WCNFDetail.WCNFDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CASE_MGMT_ID, ds.WCNFDetail.CaseMgmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.WCNFDetail.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CASE_ADJUSTER_ID, ds.WCNFDetail.CaseAdjusterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PHONE_NO, ds.WCNFDetail.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_FAX, ds.WCNFDetail.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_EMAIL, ds.WCNFDetail.EmailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PRE_AUTH, ds.WCNFDetail.PreAuthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_INJURY_DATE, ds.WCNFDetail.InjuryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_REFERRAL, ds.WCNFDetail.ReferralColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_EMPLOYMENT_RELATE, ds.WCNFDetail.EmploymentRelateColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARM_CAUSE_OF_ACCIDENT, ds.WCNFDetail.CauseOfAccidentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ACCIDENT, ds.WCNFDetail.AccidentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_STATE, ds.WCNFDetail.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ZIP, ds.WCNFDetail.ZipColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_HOUR, ds.WCNFDetail.HourColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_HCFA_FIELD16DATE_FROM, ds.WCNFDetail.HCFAField16DateFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_HCFA_FIELD16DATE_TO, ds.WCNFDetail.HCFAField16DateToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_HCFA_FIELD18DATE_FROM, ds.WCNFDetail.HCFAField18DateFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_HCFA_FIELD18DATE_TO, ds.WCNFDetail.HCFAField18DateToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_IS_ACTIVE, ds.WCNFDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(21, PARM_CREATED_BY, ds.WCNFDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_ON, ds.WCNFDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_MODIFIED_BY, ds.WCNFDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_MODIFIED_ON, ds.WCNFDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get Documents using dataset Functions"
        /// <summary>
        /// Loads the CaseManagement.
        /// </summary>
        /// <param name="CaseMgmtId">The Case identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSWCNF LoadCaseManagementWCNF(long CaseMgmtId)
        {
            //WCNFDetailModel model = new WCNFDetailModel();
            DSWCNF ds = new DSWCNF();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                //List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter(PARM_CASE_MGMT_ID, CaseMgmtId));
                //using (var reader = dbManager.ExecuteReader(PROC_CASE_MANAGEMENT_WCNF_SELECT, parameters))
                //{
                //    while (reader.Read())
                //    {
                //        var properties = typeof(WCNFDetailModel).GetProperties();

                //        foreach (var prop in properties)
                //        {
                //            prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                //        }
                //    }
                //}
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CASE_MGMT_ID, CaseMgmtId);
                ds = (DSWCNF)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_WCNF_SELECT, ds, ds.WCNFDetail.TableName);
                ds.AcceptChanges();
                return ds;
               
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LoadCaseManagementWcnf", PROC_CASE_MANAGEMENT_WCNF_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the CaseManagement
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSWCNF UpdateWCNFCaseManagement(DSWCNF ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSWCNF)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_WCNF_UPDATE, ds, ds.WCNFDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::UpdateCaseManagementWCNF", PROC_CASE_MANAGEMENT_WCNF_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the CaseManagement.
        /// </summary>
        /// <param name="CaseMgmtId">The case identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteWNCFCaseManagement(string CaseMgmtId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CASE_MGMT_ID, CaseMgmtId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_WCNF_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::DeleteCaseManagementWCNF", PROC_CASE_MANAGEMENT_WCNF_DELETE, ex);
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
        /// Inserts the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSWCNF InsertWCNFCaseManagement(DSWCNF ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSWCNF)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_WCNF_INSERT, ds, ds.WCNFDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::InsertCaseManagementWCNF", PROC_CASE_MANAGEMENT_WCNF_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        } 
        public DSWCNF LoadVisitRecord(string CaseNumber)
        {
            DSWCNF ds = new DSWCNF();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CASE_NUMBER, CaseNumber);

                ds = (DSWCNF)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_WCNF_SELECT_VISIT_RECORD, ds, ds.WCNFDetail.TableName);

                ds.AcceptChanges();
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::SelectVisitCaseManagementWcnf", PROC_CASE_MANAGEMENT_WCNF_SELECT_VISIT_RECORD, ex);
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
