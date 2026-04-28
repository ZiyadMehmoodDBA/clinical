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
    public class DALPatientAuthorization
    {
        

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientFamily"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientAuthorization()
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
        private const string PROC_PATIENT_AUTHORIZATION_DELETE = "Patient.sp_AuthorizationsDelete";
        private const string PROC_PATIENT_AUTHORIZATION_INSERT = "Patient.sp_AuthorizationsInsert";
        private const string PROC_PATIENT_AUTHORIZATION_SELECT = "Patient.sp_AuthorizationsSelect";
        private const string PROC_PATIENT_AUTHORIZATION_UPDATE = "Patient.sp_AuthorizationsUpdate";
        #endregion

        #region Parameters
        private const string PARM_AUTHORIZE_ID = "@AuthorizeId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_FROM_DATE = "@FromDate";
        private const string PARM_TO_DATE = "@ToDate";
        private const string PARM_VISITS_ALLOWED = "@VisitsAllowed";
        private const string PARM_VISITS_USED = "@VisitsUsed";
        private const string PARM_PAN = "@PAN";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region "Support Functions"

        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(16);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_AUTHORIZE_ID, ds.Authorizations.AuthorizeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_AUTHORIZE_ID, ds.Authorizations.AuthorizeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ID, ds.Authorizations.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CPT_CODE, ds.Authorizations.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PATIENT_ID, ds.Authorizations.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_FROM_DATE, ds.Authorizations.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_TO_DATE, ds.Authorizations.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_VISITS_ALLOWED, ds.Authorizations.VisitsAllowedColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_VISITS_USED, ds.Authorizations.VisitsUsedColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_PAN, ds.Authorizations.PANColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_COMMENTS, ds.Authorizations.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.Authorizations.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.Authorizations.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.Authorizations.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.Authorizations.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.Authorizations.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_DESCRIPTION, ds.Authorizations.DescriptionColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the patient Authorization.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="AuthorizationId">The Authorization identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient LoadPatientAuthorization(long PatientId, long AuthorizationId, long patientInsuranceId = 0, string CPTCode = null, DateTime? DOSFrom = null, DateTime? DOSTo = null)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (AuthorizationId <= 0)
                    dbManager.AddParameters(0, PARM_AUTHORIZE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_AUTHORIZE_ID, AuthorizationId);

                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (patientInsuranceId <= 0)
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, patientInsuranceId);

                dbManager.AddParameters(3, PARM_CPT_CODE, CPTCode);
                dbManager.AddParameters(4, PARM_FROM_DATE, DOSFrom);
                dbManager.AddParameters(5, PARM_TO_DATE, DOSTo);
                if (MDVSession.Current.IsAdmin)
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_AUTHORIZATION_SELECT, ds, ds.Authorizations.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientAuthorization::LoadPatientAuthorization", PROC_PATIENT_AUTHORIZATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the patient Authorization.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdatePatientAuthorization(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager( );
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_AUTHORIZATION_UPDATE, ds, ds.Authorizations.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientAuthorization::UpdatePatientAuthorization", PROC_PATIENT_AUTHORIZATION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the patient Authorization
        /// </summary>
        /// <param name="AuthorizationId">The Authorization identifier.</param>
        /// <returns>System.String.</returns>
        public string DeletePatientAuthorization(string AuthorizationId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager( );
            try
            {
                string returnVal = "";
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_AUTHORIZE_ID, AuthorizationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_AUTHORIZATION_DELETE);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_AUTHORIZATION_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientAuthorization::DeletePatientAuthorization", PROC_PATIENT_AUTHORIZATION_DELETE, ex);
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
        /// Inserts the patient Authorization.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient InsertPatientAuthorization(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager( );
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_AUTHORIZATION_INSERT, ds, ds.Authorizations.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientAuthorization::InsertPatientAuthorization", PROC_PATIENT_AUTHORIZATION_INSERT, ex);
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
