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
    public class DALSchool
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SCHOOL_INSERT = "Patient.sp_SchoolInsert";
        private const string PROC_SCHOOL_UPDATE = "Patient.sp_SchoolUpdate";
        private const string PROC_SCHOOL_DELETE = "Patient.sp_SchoolDelete";
        private const string PROC_SCHOOL_SELECT = "Patient.sp_SchoolSelect";
        #endregion

        #region "Parameters"
        private const string PARM_SCHOOL_ID = "@SchoolId";
        private const string PARM_SCHOOL_NAME = "@SchoolName";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZipCode";
        private const string PARM_ZIP_EXT = "@ZipExt";
        private const string PARM_CONTACT_NAME = "@ContactName";
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
        /// Initializes a new instance of the <see cref="DALSchool"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALSchool()
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
            dbManager.CreateParameters(ds.Tables[ds.School.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SCHOOL_ID, ds.School.SchoolIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SCHOOL_ID, ds.School.SchoolIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SCHOOL_NAME, ds.School.SchoolNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ADDRESS1, ds.School.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CITY, ds.School.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_STATE, ds.School.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ZIP_CODE, ds.School.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ZIP_EXT, ds.School.ZipExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CONTACT_NAME, ds.School.ContactNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PHONE_NO, ds.School.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_EMAIL_ADDRESS, ds.School.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.School.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.School.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.School.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.School.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.School.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_ENTITY_ID, ds.School.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <param name="SchoolName">Name of the school.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="State">The state.</param>
        /// <param name="Zip">The zip.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSPatientProfile LoadSchool(long SchoolId, string SchoolName, string Address, string City, string State, string Zip, string IsActive)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (SchoolName == "")
                    SchoolName = null;

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

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (SchoolId <= 0)
                    dbManager.AddParameters(0, PARM_SCHOOL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SCHOOL_ID, SchoolId);
                dbManager.AddParameters(1, PARM_SCHOOL_NAME, SchoolName);
                dbManager.AddParameters(2, PARM_ADDRESS1, Address);
                dbManager.AddParameters(3, PARM_CITY, City);
                dbManager.AddParameters(4, PARM_STATE, State);
                dbManager.AddParameters(5, PARM_ZIP_CODE, Zip);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);
               
                //if (SharedObj.IsAdmin)
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPatientProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHOOL_SELECT, ds, ds.School.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSchool::LoadSchool", PROC_SCHOOL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the school.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile UpdateSchool(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHOOL_UPDATE, ds, ds.School.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSchool::UpdateSchool", PROC_SCHOOL_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteSchool(string SchoolId)
        {
            string returnVal = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SCHOOL_ID, SchoolId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHOOL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSchool::DeleteSchool", PROC_SCHOOL_DELETE, ex);
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
        /// Inserts the school.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile InsertSchool(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHOOL_INSERT, ds, ds.School.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSchool::InsertSchool", PROC_SCHOOL_INSERT, ex);
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
