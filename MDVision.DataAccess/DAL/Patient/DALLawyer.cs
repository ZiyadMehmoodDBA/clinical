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
    public class DALLawyer
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_LAWYER_INSERT = "Patient.sp_LawyerInsert";
        private const string PROC_LAWYER_UPDATE = "Patient.sp_LawyerUpdate";
        private const string PROC_LAWYER_DELETE = "Patient.sp_LawyerDelete";
        private const string PROC_LAWYER_SELECT = "Patient.sp_LawyerSelect";
        private const string PROC_LAWYER_LOOKUP = "Patient.sp_LawyerLookup";
        #endregion

        #region "Parameters"
        private const string PARM_LAWYER_ID = "@LawyerId";
        private const string PARM_LAWYER_NAME = "@LawyerName";
        private const string PARM_FIRM_NAME = "@FirmName";
        private const string PARM_LICENSE_NO = "@LicenseNo";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZipCode";
        private const string PARM_ZIP_EXT = "@ZipExt";
        private const string PARM_CONTACT_NO = "@ContactNo";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALLawyer"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALLawyer()
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
            dbManager.CreateParameters(ds.Tables[ds.Lawyer.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LAWYER_ID, ds.Lawyer.LawyerIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LAWYER_ID, ds.Lawyer.LawyerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_LAWYER_NAME, ds.Lawyer.LawyerNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_FIRM_NAME, ds.Lawyer.FirmNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_LICENSE_NO, ds.Lawyer.LicenseNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ADDRESS1, ds.Lawyer.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CITY, ds.Lawyer.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_STATE, ds.Lawyer.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ZIP_CODE, ds.Lawyer.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ZIP_EXT, ds.Lawyer.ZipExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CONTACT_NO, ds.Lawyer.ContactNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_PHONE_NO, ds.Lawyer.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EMAIL_ADDRESS, ds.Lawyer.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.Lawyer.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.Lawyer.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.Lawyer.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.Lawyer.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.Lawyer.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.Lawyer.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <param name="LawyerName">Name of the lawyer.</param>
        /// <param name="FirmName">Name of the firm.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="Active">The active.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile LoadLawyer(long LawyerId, string LawyerName, string FirmName, string Address, string City, string IsActive)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (LawyerName == "")
                    LawyerName = null;

                if (FirmName == "")
                    FirmName = null;

                if (Address == "")
                    Address = null;

                if (City == "")
                    City = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (LawyerId == 0)
                    dbManager.AddParameters(0, PARM_LAWYER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAWYER_ID, LawyerId);
                dbManager.AddParameters(1, PARM_LAWYER_NAME, LawyerName);
                dbManager.AddParameters(2, PARM_FIRM_NAME, FirmName);
                dbManager.AddParameters(3, PARM_ADDRESS1, Address);
                dbManager.AddParameters(4, PARM_CITY, City);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPatientProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAWYER_SELECT, ds, ds.Lawyer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLawyer::LoadLawyer", PROC_LAWYER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the lawyer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile UpdateLawyer(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_LAWYER_UPDATE, ds, ds.Lawyer.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLawyer::UpdateLawyer", PROC_LAWYER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteLawyer(string LawyerId)
        {
            string returnVal = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LAWYER_ID, LawyerId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_LAWYER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLawyer::DeleteLawyer", PROC_LAWYER_DELETE, ex);
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
        /// Inserts the lawyer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile InsertLawyer(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_LAWYER_INSERT, ds, ds.Lawyer.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLawyer::InsertLawyer", PROC_LAWYER_INSERT, ex);
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
        /// Lookups the lawyer.
        /// </summary>
        /// <returns></returns>
        public DSPatientLookups LookupLawyer(string LawyerName)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (string.IsNullOrEmpty(LawyerName))
                    dbManager.AddParameters(1, PARM_LAWYER_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_LAWYER_NAME, LawyerName);

                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAWYER_LOOKUP, ds, ds.Lawyer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLawyer::LookupLawyer", PROC_LAWYER_LOOKUP, ex);
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
