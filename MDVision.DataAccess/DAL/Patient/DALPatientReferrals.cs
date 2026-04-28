using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientReferrals
    {
        #region Variable
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientReferrals"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientReferrals()
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
        private const string PROC_PATIENT_REFERRALS_DELETE = "Patient.sp_PatientReferralsDelete";
        private const string PROC_PATIENT_REFERRALS_INSERT = "Patient.sp_PatientReferralsInsert";
        private const string PROC_PATIENT_REFERRALS_SELECT = "Patient.sp_PatientReferralsSelect";
        private const string PROC_PATIENT_REFERRALS_SEARCH = "Patient.sp_PatientReferralsSearch_New";
        private const string PROC_PATIENT_REFERRALS_UPDATE = "Patient.sp_PatientReferralsUpdate";
        #endregion

        #region Parameters
        private const string PARM_REFERRAL_ID = "@PatientReferralId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_REFERRING_FROM_ID = "@ReferringFromId";
        private const string PARM_REFERRING_TO_ID = "@ReferringToId";
        private const string PARM_REFERRAL_TYPE = "@ReferralType";
        private const string PARM_FACILITY_FROM_ID = "@FacilityFromId";
        private const string PARM_FACILITY_TO_ID = "@FacilityToId";
        private const string PARM_FROM_DATE = "@FromDate";
        private const string PARM_TO_DATE = "@ToDate";
        private const string PARM_VISITS_ALLOWED = "@VisitsAllowed";
        private const string PARM_VISITS_USED = "@VisitsUsed";
        private const string PARM_REFERRAL_AUTH_NO = "@ReferralAuthNo";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_REFERRING_DATE = "@ReferringDate";
        private const string PARM_IS_FROM_SCHEDULAR = "@isFromScheduler";

        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(17);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REFERRAL_ID, ds.PatientReferrals.PatientReferralIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REFERRAL_ID, ds.PatientReferrals.PatientReferralIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_INSURANCE_ID, ds.PatientReferrals.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_REFERRING_FROM_ID, ds.PatientReferrals.ReferringFromIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_REFERRING_TO_ID, ds.PatientReferrals.ReferringToIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_REFERRAL_TYPE, ds.PatientReferrals.ReferralTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_FACILITY_FROM_ID, ds.PatientReferrals.FacilityFromIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_FACILITY_TO_ID, ds.PatientReferrals.FacilityToIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_FROM_DATE, ds.PatientReferrals.FromDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_TO_DATE, ds.PatientReferrals.ToDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_VISITS_ALLOWED, ds.PatientReferrals.VisitsAllowedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_VISITS_USED, ds.PatientReferrals.VisitsUsedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_REFERRAL_AUTH_NO, ds.PatientReferrals.ReferralAuthNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.PatientReferrals.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.PatientReferrals.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.PatientReferrals.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.PatientReferrals.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.PatientReferrals.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the patient referrals.
        /// </summary>
        /// <param name="PatientInsuranceId">The patient insurance identifier.</param>
        /// <param name="PatientReferralId">The patient referral identifier.</param>
        /// <returns></returns>
        public DSPatient LoadPatientReferrals(long PatientInsuranceId, long PatientReferralId, string ReferralType = "", long FacilityToId = 0, long ReferringToId = 0, DateTime? ReferringDate = null, string IsActive = "")
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (PatientReferralId == 0)
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRAL_ID, PatientReferralId);
                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                if (ReferralType == "")
                    dbManager.AddParameters(3, PARM_REFERRAL_TYPE, null);
                else
                    dbManager.AddParameters(3, PARM_REFERRAL_TYPE, ReferralType);

                if (FacilityToId == 0)
                    dbManager.AddParameters(4, PARM_FACILITY_TO_ID, null);
                else
                    dbManager.AddParameters(4, PARM_FACILITY_TO_ID, FacilityToId);

                if (ReferringToId == 0)
                    dbManager.AddParameters(5, PARM_REFERRING_TO_ID, null);
                else
                    dbManager.AddParameters(5, PARM_REFERRING_TO_ID, ReferringToId);

                dbManager.AddParameters(6, PARM_REFERRING_DATE, ReferringDate);

                if (IsActive == "")
                    dbManager.AddParameters(7, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_REFERRALS_SELECT, ds, ds.PatientReferrals.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferrals::LoadPatientReferrals", PROC_PATIENT_REFERRALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient searchPatientReferrals(long PatientInsuranceId,string IsFromAppointment, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
               
                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, 15);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);

                if (IsFromAppointment == "true")
                {
                    dbManager.AddParameters(4, PARM_IS_FROM_SCHEDULAR,1);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_IS_FROM_SCHEDULAR,null);

                }
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PatientReferralsSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_REFERRALS_SEARCH, ds, ds.PatientReferralsSearch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferrals::searchPatientReferrals", PROC_PATIENT_REFERRALS_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the patient referrals.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdatePatientReferrals(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientReferrals.GetChanges();
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_REFERRALS_UPDATE, ds, ds.PatientReferrals.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientReferrals.Rows[0][ds.PatientReferrals.PatientReferralIdColumn].ToString(), "");
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferrals::UpdatePatientReferrals", PROC_PATIENT_REFERRALS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the patient referrals.
        /// </summary>
        /// <param name="PatientReferralId">The patient referral identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePatientReferrals(string PatientReferralId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                DSPatient ds = LoadPatientReferrals(0,Convert.ToInt64(PatientReferralId), "", 0, 0,null,"");
                DSDBAudit dsDBAudit = new DSDBAudit();               
                DataTable dtTemp = ds.PatientReferrals;                                  
                dbManager.AddParameters(0, PARM_REFERRAL_ID, PatientReferralId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_REFERRALS_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                else {
                    if (dtTemp != null && ds.PatientReferrals.Rows.Count > 0)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientReferrals.Rows[0][ds.PatientReferrals.PatientReferralIdColumn].ToString(), null, "", false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferrals::DeletePatientReferrals", PROC_PATIENT_REFERRALS_DELETE, ex);
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
        /// Inserts the patient referrals.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient InsertPatientReferrals(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.PatientReferrals.GetChanges();
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_REFERRALS_INSERT, ds, ds.PatientReferrals.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientReferrals.Rows[0][ds.PatientReferrals.PatientReferralIdColumn].ToString(), "");
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientReferrals::InsertPatientReferrals", PROC_PATIENT_REFERRALS_INSERT, ex);
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
