using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Case
{
    public class DALCase
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALDocument"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALCase()
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

        private const string PROC_CASE_MANAGEMENT_DELETE = "Patient.sp_CaseManagementDelete";
        private const string PROC_CASE_MANAGEMENT_INSERT = "Patient.sp_CaseManagementInsert";
        private const string PROC_CASE_MANAGEMENT_SELECT = "Patient.sp_CaseManagementSelect";
        private const string PROC_CASE_MANAGEMENT_UPDATE = "Patient.sp_CaseManagementUpdate";
        private const string PROC_CASE_TYPE_LOOKUP = "Patient.sp_CaseTypeLookup";
        private const string PROC_CONDITION_CODES_LOOKUP = "Patient.sp_ConditionCodesLookup";
        private const string PROC_CASE_LOOKUP = "Patient.sp_CaseManagementLoopkup";
        private const string PROC_CASE_DOCUMENTSELECT = "Patient.sp_CaseManagement_DocumentSelect";
        #endregion

        #region Parameters

        private const string PARM_CASE_MGMT_ID = "@CaseMgmtId";
        private const string PARM_CASE_NUMBER = "@CaseNumber";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_CASE_TYPE_ID = "@CaseTypeId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_REFERRING_PROVIDER_ID = "@ReferringProviderId";
        private const string PARM_OPERATING_PROVIDER_ID = "@OperatingProviderId";
        private const string PARM_HOSPITAL_CASE_NO = "@HospitalCaseNo";
        private const string PARM_FREQUENCY_CODE = "@FrequencyCode";
        private const string PARM_SOURCE_OF_ADMIT = "@SourceOfAdmit";
        private const string PARM_PATIENT_STATUS = "@PatientStatus";
        private const string PARM_TYPE_OF_ADMISSION = "@TypeOfAdmission";
        private const string PARM_ADMIT_DX_CODE = "@AdmitDXCode";
        private const string PARM_CONDITION_CODE_1 = "@ConditionCode1";
        private const string PARM_CONDITION_CODE_2 = "@ConditionCode2";
        private const string PARM_CONDITION_CODE_3 = "@ConditionCode3";
        private const string PARM_CONDITION_CODE_4 = "@ConditionCode4";
        private const string PARM_OCCURRENCE_CODE_1 = "@OccurrenceCode1";
        private const string PARM_OCCURRENCE_CODE_2 = "@OccurrenceCode2";
        private const string PARM_OCCURRENCE_CODE_3 = "@OccurrenceCode3";
        private const string PARM_OCCURRENCE_CODE_4 = "@OccurrenceCode4";
        private const string PARM_OCCURRENCE_DATE_1 = "@OccurrenceDate1";
        private const string PARM_OCCURRENCE_DATE_2 = "@OccurrenceDate2";
        private const string PARM_OCCURRENCE_DATE_3 = "@OccurrenceDate3";
        private const string PARM_OCCURRENCE_DATE_4 = "@OccurrenceDate4";
        private const string PARM_VALUE_CODE_1 = "@ValueCode1";
        private const string PARM_VALUE_CODE_2 = "@ValueCode2";
        private const string PARM_VALUE_CODE_3 = "@ValueCode3";
        private const string PARM_VALUE_CODE_4 = "@ValueCode4";
        private const string PARM_VALUE_CODE1_AMOUNT = "@ValueCode1Amount";
        private const string PARM_VALUE_CODE2_AMOUNT = "@ValueCode2Amount";
        private const string PARM_VALUE_CODE3_AMOUNT = "@ValueCode3Amount";
        private const string PARM_VALUE_CODE4_AMOUNT = "@ValueCode4Amount";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_DOCUMENT_COUNT = "@DocumentCount";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSCase ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(39);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CASE_MGMT_ID, ds.CaseManagement.CaseMgmtIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CASE_MGMT_ID, ds.CaseManagement.CaseMgmtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CASE_NUMBER, ds.CaseManagement.CaseNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.CaseManagement.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CASE_TYPE_ID, ds.CaseManagement.CaseTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_PATIENT_INSURANCE_ID, ds.CaseManagement.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_FACILITY_ID, ds.CaseManagement.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_REFERRING_PROVIDER_ID, ds.CaseManagement.ReferringProviderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(7, PARM_OPERATING_PROVIDER_ID, ds.CaseManagement.OperatingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_HOSPITAL_CASE_NO, ds.CaseManagement.HospitalCaseNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_FREQUENCY_CODE, ds.CaseManagement.FrequencyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SOURCE_OF_ADMIT, ds.CaseManagement.SourceOfAdmitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_PATIENT_STATUS, ds.CaseManagement.PatientStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_TYPE_OF_ADMISSION, ds.CaseManagement.TypeOfAdmissionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ADMIT_DX_CODE, ds.CaseManagement.AdmitDXCodeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(14, PARM_CONDITION_CODE_1, ds.CaseManagement.ConditionCode1Column.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_CONDITION_CODE_2, ds.CaseManagement.ConditionCode2Column.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARM_CONDITION_CODE_3, ds.CaseManagement.ConditionCode3Column.ColumnName, DbType.Int32);
            dbManager.AddParameters(17, PARM_CONDITION_CODE_4, ds.CaseManagement.ConditionCode4Column.ColumnName, DbType.Int32);

            dbManager.AddParameters(18, PARM_OCCURRENCE_CODE_1, ds.CaseManagement.OccurrenceCode1Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_OCCURRENCE_CODE_2, ds.CaseManagement.OccurrenceCode2Column.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_OCCURRENCE_CODE_3, ds.CaseManagement.OccurrenceCode3Column.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_OCCURRENCE_CODE_4, ds.CaseManagement.OccurrenceCode4Column.ColumnName, DbType.String);

            dbManager.AddParameters(22, PARM_OCCURRENCE_DATE_1, ds.CaseManagement.OccurrenceDate1Column.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_OCCURRENCE_DATE_2, ds.CaseManagement.OccurrenceDate2Column.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_OCCURRENCE_DATE_3, ds.CaseManagement.OccurrenceDate3Column.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_OCCURRENCE_DATE_4, ds.CaseManagement.OccurrenceDate4Column.ColumnName, DbType.DateTime);

            dbManager.AddParameters(26, PARM_VALUE_CODE_1, ds.CaseManagement.ValueCode1Column.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_VALUE_CODE_2, ds.CaseManagement.ValueCode2Column.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_VALUE_CODE_3, ds.CaseManagement.ValueCode3Column.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_VALUE_CODE_4, ds.CaseManagement.ValueCode4Column.ColumnName, DbType.String);

            dbManager.AddParameters(30, PARM_VALUE_CODE1_AMOUNT, ds.CaseManagement.ValueCode1AmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(31, PARM_VALUE_CODE2_AMOUNT, ds.CaseManagement.ValueCode2AmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(32, PARM_VALUE_CODE3_AMOUNT, ds.CaseManagement.ValueCode3AmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(33, PARM_VALUE_CODE4_AMOUNT, ds.CaseManagement.ValueCode4AmountColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(34, PARM_IS_ACTIVE, ds.CaseManagement.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(35, PARM_CREATED_BY, ds.CaseManagement.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_CREATED_ON, ds.CaseManagement.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARM_MODIFIED_BY, ds.CaseManagement.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(38, PARM_MODIFIED_ON, ds.CaseManagement.ModifiedOnColumn.ColumnName, DbType.DateTime);



        }
        #endregion

        #region "Insert, delete, update and get Documents using dataset Functions"
        /// <summary>
        /// Loads the CaseManagement.
        /// </summary>
        /// <param name="CaseMgmtId">The Case identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSCase LoadCaseManagement(string AccountNumber, string ClaimNumber, long PatientId, long CaseId, long PatientInsuranceId, int CaseTypeId, long FacilityId, long ReffProviderId, string IsActive, string CaseNumber, int PageNumber = 0, int RowspPage = 0)
        {
            DSCase ds = new DSCase();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(15);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (CaseId == 0)
                    dbManager.AddParameters(1, PARM_CASE_MGMT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CASE_MGMT_ID, CaseId);
                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                if (CaseTypeId == 0)
                    dbManager.AddParameters(3, PARM_CASE_TYPE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_CASE_TYPE_ID, CaseTypeId);
                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);
                if (ReffProviderId == 0)
                    dbManager.AddParameters(5, PARM_REFERRING_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(5, PARM_REFERRING_PROVIDER_ID, ReffProviderId);

                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(7, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.CaseManagement.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(11, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (AccountNumber == "")
                    dbManager.AddParameters(12, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(12, PARM_ACCOUNT_NUMBER, AccountNumber);

                if (ClaimNumber == "")
                    dbManager.AddParameters(13, PARM_CLAIM_NUMBER, null);
                else
                    dbManager.AddParameters(13, PARM_CLAIM_NUMBER, ClaimNumber);
                if (!string.IsNullOrEmpty(CaseNumber))
                    dbManager.AddParameters(14, PARM_CASE_NUMBER, CaseNumber);
                else
                    dbManager.AddParameters(14, PARM_CASE_NUMBER, null);
                ds = (DSCase)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_SELECT, ds, ds.CaseManagement.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LoadCaseManagement", PROC_CASE_MANAGEMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSBatchCharge LoadBatchChargeDocument(string CaseId)
        {
            DSBatchCharge ds = new DSBatchCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_CASE_MGMT_ID, CaseId);

                ds = (DSBatchCharge)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_DOCUMENTSELECT, ds, ds.BatchDocuments.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchCharge::LoadBatchChargeDocument", PROC_CASE_DOCUMENTSELECT, ex);
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
        public DSCase UpdateCaseManagement(DSCase ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCase)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_UPDATE, ds, ds.CaseManagement.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::UpdateCaseManagement", PROC_CASE_MANAGEMENT_UPDATE, ex);
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
        public string DeleteCaseManagement(string CaseMgmtId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CASE_MGMT_ID, CaseMgmtId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::DeleteCaseManagement", PROC_CASE_MANAGEMENT_DELETE, ex);
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
        public DSCase InsertCaseManagement(DSCase ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSCase)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CASE_MANAGEMENT_INSERT, ds, ds.CaseManagement.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::InsertCaseManagement", PROC_CASE_MANAGEMENT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Lookups"
        public DSCaseLookup LookupCaseManagement(long PatientId)
        {
            DSCaseLookup ds = new DSCaseLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                ds = (DSCaseLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_LOOKUP, ds, ds.CaseManagement.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LookupCaseManagement", PROC_CASE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCaseLookup LookupCaseType()
        {
            DSCaseLookup ds = new DSCaseLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCaseLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_TYPE_LOOKUP, ds, ds.CaseType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LookupCaseType", PROC_CASE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCaseLookup LookupConditionCode()
        {
            DSCaseLookup ds = new DSCaseLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCaseLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONDITION_CODES_LOOKUP, ds, ds.ConditionCodes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LookupConditionCode", PROC_CONDITION_CODES_LOOKUP, ex);
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
