using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using MDVision.Model;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALGuarantor
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_GUARANTOR_INSERT = "Patient.sp_GuarantorInsert";
        private const string PROC_GUARANTOR_UPDATE = "Patient.sp_GuarantorUpdate";
        private const string PROC_GUARANTOR_DELETE = "Patient.sp_GuarantorDelete";
        private const string PROC_GUARANTOR_SELECT = "Patient.sp_GuarantorSelect";
        private const string PROC_RELATIONS_LOOKUP = "Patient.sp_RelationShipLookup";
        private const string PROC_GUARANTOR_LOOKUP = "Patient.sp_GuarantorLookup";
        // MK, Guarantor Laod WRT Patient // To not modify the current Implementation
        private const string PROC_PATIENTGUARANTOR_SELECT = "Patient.sp_PatientGuarantorSelect";
        //

        #endregion

        #region "Parameters"
        private const string PARM_GUARANTOR_ID = "@GuarantorId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_DOB = "@DOB";
        private const string PARM_RELATION_ID = "@RelationId";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZipCode";
        private const string PARM_ZIP_EXT = "@ZipExt";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";

        private const string PARM_NAME = "@Name";
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALGuarantor"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALGuarantor()
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
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_GUARANTOR_ID, ds.Guarantor.GuarantorIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_GUARANTOR_ID, ds.Guarantor.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_FIRST_NAME, ds.Guarantor.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_LAST_NAME, ds.Guarantor.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DOB, ds.Guarantor.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_RELATION_ID, ds.Guarantor.RelationIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_PHONE_NO, ds.Guarantor.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ADDRESS1, ds.Guarantor.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CITY, ds.Guarantor.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_STATE, ds.Guarantor.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ZIP_CODE, ds.Guarantor.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ZIP_EXT, ds.Guarantor.ZipExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_EMAIL_ADDRESS, ds.Guarantor.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.Guarantor.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.Guarantor.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.Guarantor.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.Guarantor.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.Guarantor.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.Guarantor.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_PATIENT_ID, ds.Guarantor.PatientIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="RelationId">The relation identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSPatientProfile LoadGuarantor(long GuarantorId, string FirstName, string LastName, string RelationId, string IsActive, Int64 PatientID = 0, int PageNumber=1, int RowspPage=15)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (RelationId == "")
                    RelationId = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(11);
                if (GuarantorId <= 0)
                    dbManager.AddParameters(0, PARM_GUARANTOR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_GUARANTOR_ID, GuarantorId);
                dbManager.AddParameters(1, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(3, PARM_RELATION_ID, RelationId);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, IsActive);
                //if (SharedObj.IsAdmin)
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(5, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(8, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.Guarantor.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (PatientID == 0)
                    dbManager.AddParameters(10, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PATIENT_ID, PatientID);
                ds = (DSPatientProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_SELECT, ds, ds.Guarantor.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::LoadGuarantor", PROC_GUARANTOR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientProfile LoadPatientGuarantor(long GuarantorId, long PatientId, string IsActive, int PageNumber,
            int RowspPage)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (GuarantorId <= 0)
                    dbManager.AddParameters(0, PARM_GUARANTOR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_GUARANTOR_ID, GuarantorId);

                if (PatientId <= 1)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);


                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Guarantor.RecordCountColumn.ColumnName, DbType.Int64,
                    ParamDirection.Output);

                ds =
                    (DSPatientProfile)
                        dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENTGUARANTOR_SELECT, ds,
                            ds.Guarantor.TableName);
                return ds;
            }
            catch (Exception ex)
            {
            MDVLogger.DALErrorLog("DALGuarantor::LoadPatientGuarantor", PROC_PATIENTGUARANTOR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the guarantor.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile UpdateGuarantor(DSPatientProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_UPDATE, ds, ds.Guarantor.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::UpdateGuarantor", PROC_GUARANTOR_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteGuarantor(string GuarantorId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_GUARANTOR_ID, GuarantorId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GUARANTOR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::DeleteGuarantor", PROC_GUARANTOR_DELETE, ex);
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
        /// Inserts the guarantor.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatientProfile.</returns>
        public DSPatientProfile InsertGuarantor(DSPatientProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_INSERT, ds, ds.Guarantor.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::InsertGuarantor", PROC_GUARANTOR_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the relation.
        /// </summary>
        /// <returns>DSRelationLookup.</returns>
        public DSPatientLookups LookupRelation()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RELATIONS_LOOKUP, ds, ds.RelationShip.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::LookupRelation", PROC_RELATIONS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<RelationModel> LookupRelationDemographic()
        {
            List<RelationModel> listAllergies = new List<RelationModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RELATIONS_LOOKUP);

                RelationModel model = null;
                while (reader.Read())
                {
                    model = new RelationModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::LookupRelationDemographic", PROC_RELATIONS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the guarantor.
        /// </summary>
        /// <returns></returns>
        public DSPatientLookups LookupGuarantor()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_LOOKUP, ds, ds.Guarantor.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::LookupGuarantor", PROC_GUARANTOR_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientLookups LookupGuarantor(string name, string IsActive = "1")
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (name == "")
                    name = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();

                dbManager.CreateParameters(4);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(1, PARM_NAME, name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GUARANTOR_LOOKUP, ds, ds.Guarantor.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALGuarantor::LookupGuarantor", PROC_GUARANTOR_LOOKUP, ex);
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
