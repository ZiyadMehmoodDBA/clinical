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

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALEmployer
    {


        #region "Stored Procedure Names"
        private const string PROC_EMPLOYER_INSERT = "Patient.sp_EmployerInsert";
        private const string PROC_EMPLOYER_UPDATE = "Patient.sp_EmployerUpdate";
        private const string PROC_EMPLOYER_DELETE = "Patient.sp_EmployerDelete";
        private const string PROC_EMPLOYER_SELECT = "Patient.sp_EmployerSelect";
        private const string PROC_EMPLOYER_LOOKUP = "Patient.sp_EmployerLookup";
        #endregion

        #region "Parameters"
        private const string PARM_EMPLOYER_ID = "@EmployerId";
        private const string PARM_EMPLOYER_NAME = "@EmployerName";
        private const string PARM_CONTACT_NAME = "@ContactName";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZipCode";
        private const string PARM_ZIP_EXT = "@ZipExt";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region Constructors

        public DALEmployer()
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
        private void CreateParameters(IDBManager dbManager, DSPatientProfile ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.Employer.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EMPLOYER_ID, ds.Employer.EmployerIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EMPLOYER_ID, ds.Employer.EmployerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_EMPLOYER_NAME, ds.Employer.EmployerNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_CONTACT_NAME, ds.Employer.ContactNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ADDRESS1, ds.Employer.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ADDRESS2, ds.Employer.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CITY, ds.Employer.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_STATE, ds.Employer.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ZIP_CODE, ds.Employer.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ZIP_EXT, ds.Employer.ZipExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PHONE_NO, ds.Employer.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_FAX_NO, ds.Employer.FaxNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EMAIL_ADDRESS, ds.Employer.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.Employer.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.Employer.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.Employer.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.Employer.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.Employer.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.Employer.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the employer.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <param name="EmployerName">Name of the employer.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="State">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile LoadEmployer(long EmployerId, string EmployerName, string Address, string City, string State, string Zip, string IsActive, string zipExt)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (EmployerName == "")
                    EmployerName = null;

                if (Address == "")
                    Address = null;

                if (City == "")
                    City = null;

                if (State == "")
                    State = null;

                if (Zip == "")
                    Zip = null;

                if (IsActive == "")
                    IsActive = null;

                if (zipExt == "")
                    zipExt = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (EmployerId <= 0)
                    dbManager.AddParameters(0, PARM_EMPLOYER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EMPLOYER_ID, EmployerId);
                dbManager.AddParameters(1, PARM_EMPLOYER_NAME, EmployerName);
                dbManager.AddParameters(2, PARM_ADDRESS1, Address);
                dbManager.AddParameters(3, PARM_CITY, City);
                dbManager.AddParameters(4, PARM_STATE, State);
                dbManager.AddParameters(5, PARM_ZIP_CODE, Zip);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(7, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                dbManager.AddParameters(8, PARM_ZIP_EXT, zipExt);
                ds = (DSPatientProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EMPLOYER_SELECT, ds, ds.Employer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEmployer::LoadEmployer", PROC_EMPLOYER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the employer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile UpdateEmployer(DSPatientProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EMPLOYER_UPDATE, ds, ds.Employer.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEmployer::UpdateEmployer", PROC_EMPLOYER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the employer.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteEmployer(string EmployerId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_EMPLOYER_ID, EmployerId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EMPLOYER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEmployer::DeleteEmployer", PROC_EMPLOYER_DELETE, ex);
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
        /// Inserts the employer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile InsertEmployer(DSPatientProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EMPLOYER_INSERT, ds, ds.Employer.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEmployer::InsertEmployer", PROC_EMPLOYER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Lookups
        /// <summary>
        /// Lookups the employer.
        /// </summary>
        /// <returns></returns>
        public DSPatientLookups LookupEmployer(string EmployerName)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(EmployerName))
                    dbManager.AddParameters(0, PARM_EMPLOYER_NAME, null);
                else
                    dbManager.AddParameters(0, PARM_EMPLOYER_NAME, EmployerName);

                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EMPLOYER_LOOKUP, ds, ds.Employer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEmployer::LookupEmployer", PROC_EMPLOYER_LOOKUP, ex);
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
