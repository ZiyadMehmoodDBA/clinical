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
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientFamily
    {
        #region Variable
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientFamily"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientFamily()
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
        private const string PROC_PATIENT_Family_DELETE = "Patient.sp_PatientFamilyDelete";
        private const string PROC_PATIENT_Family_INSERT = "Patient.sp_PatientFamilyInsert";
        private const string PROC_PATIENT_Family_SELECT = "Patient.sp_PatientFamilySelect";
        private const string PROC_PATIENT_Family_SEARCH = "Patient.sp_PatientFamilySearch_New";
        private const string PROC_PATIENT_Family_UPDATE = "Patient.sp_PatientFamilyUpdate";
        private const string PROC_PATIENT_FAMILY_LOOKUP = "Patient.sp_PatientFamilyLookup";
        #endregion

        #region Parameters
        private const string PARM_FAMILY_ID = "@FamilyId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_Last_Name = "@LastName";
        private const string PARM_First_Name = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_RELATIONSHIP_ID = "@RelationShipId";
        private const string PARM_DOB = "@DOB";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@Zipcode";
        private const string PARM_ZIP_EXT = "@ZipExt";
        private const string PARM_HOME_PHONE_NO = "@HomePhoneNo";
        private const string PARM_WORK_PHONE_NO = "@WorkPhoneNo";
        private const string PARM_WORK_PHONE_EXT = "@WorkPhExt";
        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_DOD = "@DOD";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_OTHER_RELATION = "@OtherRelation";
        private const string PARM_LINKED_PATIENT_ID = "@LinkedPatientId";
        private const string PARM_ACCOUNT_NUMBER = "@AccountNumber";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_IS_ADD_AS_PATIENT = "@IsAddAsPatient";
        #endregion

        #region "Support Functions"

        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(32);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FAMILY_ID, ds.PatientFamily.FamilyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FAMILY_ID, ds.PatientFamily.FamilyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientFamily.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_First_Name, ds.PatientFamily.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_Last_Name, ds.PatientFamily.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MI, ds.PatientFamily.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_RELATIONSHIP_ID, ds.PatientFamily.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_DOB, ds.PatientFamily.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_ADDRESS1, ds.PatientFamily.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ADDRESS2, ds.PatientFamily.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CITY, ds.PatientFamily.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_STATE, ds.PatientFamily.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ZIP_CODE, ds.PatientFamily.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ZIP_EXT, ds.PatientFamily.ZipExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_HOME_PHONE_NO, ds.PatientFamily.HomePhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_WORK_PHONE_NO, ds.PatientFamily.WorkPhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_WORK_PHONE_EXT, ds.PatientFamily.WorkPhExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CELL_NO, ds.PatientFamily.CellNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_FAX_NO, ds.PatientFamily.FaxNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_EMAIL_ADDRESS, ds.PatientFamily.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_DOD, ds.PatientFamily.DODColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_IS_ACTIVE, ds.PatientFamily.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(21, PARM_CREATED_BY, ds.PatientFamily.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_ON, ds.PatientFamily.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_MODIFIED_BY, ds.PatientFamily.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_MODIFIED_ON, ds.PatientFamily.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_OTHER_RELATION, ds.PatientFamily.OtherRelationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_LINKED_PATIENT_ID, ds.PatientFamily.LinkedPatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(27, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
            if (MDVUtility.ToInt64(ds.PatientFamily.Rows[0][ds.PatientFamily.ProviderIdColumn]) > 0)
                dbManager.AddParameters(28, PARM_PROVIDER_ID, ds.PatientFamily.ProviderIdColumn.ColumnName, DbType.Int64);
            else
                dbManager.AddParameters(28, PARM_PROVIDER_ID, DBNull.Value);
            if (MDVUtility.ToInt64(ds.PatientFamily.Rows[0][ds.PatientFamily.FacilityIdColumn]) > 0)
                dbManager.AddParameters(29, PARM_FACILITY_ID, ds.PatientFamily.FacilityIdColumn.ColumnName, DbType.Int64);
            else
                dbManager.AddParameters(29, PARM_FACILITY_ID, DBNull.Value);
            if (MDVUtility.ToInt64(ds.PatientFamily.Rows[0][ds.PatientFamily.PracticeIdColumn]) > 0)
                dbManager.AddParameters(30, PARM_PRACTICE_ID, ds.PatientFamily.PracticeIdColumn.ColumnName, DbType.Int64);
            else
                dbManager.AddParameters(30, PARM_PRACTICE_ID, DBNull.Value);
            dbManager.AddParameters(31, PARM_IS_ADD_AS_PATIENT, ds.PatientFamily.IsAddAsPatientColumn.ColumnName, DbType.Boolean);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the patient family.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="FamilyId">The family identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient LoadPatientFamily(long PatientId, long FamilyId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (FamilyId <= 0)
                    dbManager.AddParameters(0, PARM_FAMILY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAMILY_ID, FamilyId);

                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_Family_SELECT, ds, ds.PatientFamily.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::LoadPatientFamily", PROC_PATIENT_Family_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient searchPatientFamily(long PatientId, long PatientRepresentativeId = 0, int pageNumber = 1, int rowsPerPage = 1000, string FirstName = "", string LastName = "", string AccountNo = "", string PhoneNo = "")
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(10);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.PatientFamilySearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (PatientRepresentativeId <= 0)
                    dbManager.AddParameters(5, PARM_FAMILY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_FAMILY_ID, PatientRepresentativeId);

                if(string.IsNullOrEmpty(FirstName))
                    dbManager.AddParameters(6, PARM_First_Name , null);
                else
                    dbManager.AddParameters(6, PARM_First_Name, FirstName);

                if(string.IsNullOrEmpty(LastName))
                    dbManager.AddParameters(7, PARM_Last_Name, null);
                else
                    dbManager.AddParameters(7, PARM_Last_Name, LastName);

                if(string.IsNullOrEmpty(AccountNo))
                    dbManager.AddParameters(8, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_ACCOUNT_NUMBER, AccountNo);

                if(string.IsNullOrEmpty(PhoneNo))
                    dbManager.AddParameters(9, PARM_PHONE_NO, null);
                else
                    dbManager.AddParameters(9, PARM_PHONE_NO, PhoneNo);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_Family_SEARCH, ds, ds.PatientFamilySearch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::searchPatientFamily", PROC_PATIENT_Family_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the patient family.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdatePatientFamily(DSPatient ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.PatientFamily.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_Family_UPDATE, ds, ds.PatientFamily.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientFamily.Rows[0][ds.PatientFamily.FamilyIdColumn].ToString());

                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::UpdatePatientFamily", PROC_PATIENT_Family_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the patient family.
        /// </summary>
        /// <param name="FamilyId">The family identifier.</param>
        /// <returns>System.String.</returns>
        public string DeletePatientFamily(string FamilyId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAMILY_ID, FamilyId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_Family_DELETE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::DeletePatientFamily", PROC_PATIENT_Family_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the patient family.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient InsertPatientFamily(DSPatient ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.PatientFamily.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);

                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_Family_INSERT, ds, ds.PatientFamily.TableName);
                ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientFamily.Rows[0][ds.PatientFamily.FamilyIdColumn].ToString());

                    dsDBAudit.AcceptChanges();
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::InsertPatientFamily", PROC_PATIENT_Family_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient LookupPatientFamily(string PatientId, string AccountNo, string FirstName, string LastName)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(4);
                if (string.IsNullOrEmpty(PatientId))
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, MDVUtility.ToLong(PatientId));
                if (string.IsNullOrEmpty(AccountNo))
                    dbManager.AddParameters(1, PARM_ACCOUNT_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_ACCOUNT_NUMBER, AccountNo);

                if (string.IsNullOrEmpty(FirstName))
                    dbManager.AddParameters(2, PARM_First_Name, null);
                else
                    dbManager.AddParameters(2, PARM_First_Name, FirstName);

                if (string.IsNullOrEmpty(LastName))
                    dbManager.AddParameters(3, PARM_Last_Name, null);
                else
                    dbManager.AddParameters(3, PARM_Last_Name, LastName);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_FAMILY_LOOKUP, ds, ds.Patients.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientFamily::LookupPatientFamily", PROC_PATIENT_FAMILY_LOOKUP, ex);
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
