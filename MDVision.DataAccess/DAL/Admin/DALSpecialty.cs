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

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALSpecialty
    {
        #region Variable



        #endregion

        #region " Stored Procedure Names"
        private const string PROC_SPECIALTY_INSERT = "Provider.sp_SpecialtyInsert";
        private const string PROC_SPECIALTY_UPDATE = "Provider.sp_SpecialtyUpdate";
        private const string PROC_SPECIALTY_DELETE = "Provider.sp_SpecialtyDelete";
        private const string PROC_SPECIALTY_SELECT = "Provider.sp_SpecialtySelect";
        private const string PROC_SPECIALTY_SELECT_ALL = "Provider.sp_AllSpecialtiesSelect";
        private const string PROC_SPECIALTY_LOOKUP_BY_NAME = "Provider.sp_SpecialtySelectLookup";

        private const string PROC_SPECIALTY_LOOKUP = "Provider.sp_SpecialtyLookup";
        private const string PROC_SPECIALTY_LOOKUP_ALL = "Provider.sp_AllSpecialities";

        private const string PROC_SPECIALTY_ENTITY_LOOKUP = "Provider.sp_SpecialtyEntityLookup";
        #endregion

        #region "Parameters"
        private const string PARM_SPECIALTY_ID = "@SpecialtyId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@errormessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSpecialty"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALSpecialty()
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
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSProfile ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SPECIALTY_ID, ds.Specialty.SpecialtyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SPECIALTY_ID, ds.Specialty.SpecialtyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_NAME, ds.Specialty.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.Specialty.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.Specialty.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.Practice.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Specialty.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Specialty.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.Specialty.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.Specialty.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_ENTITY_ID, ds.Specialty.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the specialty.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSProfile LoadSpecialty(long SpecialtyId, string ShortName, string Description, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (SpecialtyId <= 0)
                    dbManager.AddParameters(0, PARM_SPECIALTY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SPECIALTY_ID, SpecialtyId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                bool isMultipleEntities = false;
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                {
                    if (EntityId.IndexOf(',') > -1)
                    {
                        isMultipleEntities = true;
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    }
                    else
                    {
                        dbManager.AddParameters(3, PARM_ENTITY_ID, Convert.ToInt32(EntityId) < 1 ? null : EntityId);
                    }

                }
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.Specialty.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (isMultipleEntities)
                {
                    ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_ENTITY_LOOKUP, ds, ds.Specialty.TableName);
                }
                else
                {
                    ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_SELECT, ds, ds.Specialty.TableName);
                }

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LoadSpecialty", PROC_SPECIALTY_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupSpecialtyByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Searchstring == "")
                    Searchstring = null;
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_SHORT_NAME, Searchstring);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_LOOKUP_BY_NAME, ds, ds.Specialty.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialtyByName", PROC_SPECIALTY_LOOKUP_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile LoadSpecialtyAll(long SpecialtyId, string ShortName, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (SpecialtyId <= 0)
                    dbManager.AddParameters(0, PARM_SPECIALTY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SPECIALTY_ID, SpecialtyId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
               
                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.Specialty.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_SELECT_ALL, ds, ds.Specialty.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LoadSpecialty", PROC_SPECIALTY_SELECT_ALL, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Lookups the specialty.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupSpecialty(string Active)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_LOOKUP, ds, ds.Specialty.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialty", PROC_SPECIALTY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProfileLookup LookupSpecialtiesAllEntities(string Active)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_LOOKUP_ALL, ds, ds.Specialty.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialtyAll", PROC_SPECIALTY_LOOKUP_ALL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProfileLookup LookupSpecialty(string Active, string EntityId)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (string.IsNullOrEmpty(EntityId))
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                {
                    if (EntityId.IndexOf(',') > -1)
                    {
                        dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt32(EntityId) > 0 ? EntityId : null);
                    }

                }

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_ENTITY_LOOKUP, ds, ds.Specialty.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialty", PROC_SPECIALTY_ENTITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateSpecialty(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Specialty.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_UPDATE, ds, ds.Specialty.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Specialty.Rows[0][ds.Specialty.SpecialtyIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::UpdateSpecialty", PROC_SPECIALTY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the specialty.
        /// </summary>
        /// <param name="SpecialtyIds">The specialty ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteSpecialty(string SpecialtyIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadSpecialty(Convert.ToInt64(SpecialtyIds), null, null,null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Specialty;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SPECIALTY_ID, SpecialtyIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SPECIALTY_DELETE);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SPECIALTY_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Specialty.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Specialty.Rows[0][ds.Specialty.SpecialtyIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::DeleteSpecialty", PROC_SPECIALTY_DELETE, ex);
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
        /// Inserts the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertSpecialty(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Specialty.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_INSERT, ds, ds.Specialty.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Specialty.Rows[0][ds.Specialty.SpecialtyIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Specialty.TableName].Rows[0][ds.Specialty.SpecialtyIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::InsertSpecialty", PROC_SPECIALTY_INSERT, ex);
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
