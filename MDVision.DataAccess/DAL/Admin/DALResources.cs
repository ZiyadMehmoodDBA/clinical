using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALResources
    {
        #region Variable

        #endregion
        #region " Stored Procedure Names"
        private const string PROC_RESOURCES_INSERT = "Provider.sp_ResourcesInsert";
        private const string PROC_RESOURCES_UPDATE = "Provider.sp_ResourcesUpdate";
        private const string PROC_RESOURCES_DELETE = "Provider.sp_ResourcesDelete";
        private const string PROC_RESOURCES_SELECT = "Provider.sp_ResourcesSelect";
        private const string PROC_RESOURCES_LOOKUP = "Provider.sp_ResourcesLookup";
        #endregion

        #region "Parameters"
        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_NAME = "@Name";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_DESCCRIPTION = "@Description";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_PROVIDER_REQUIRED = "@ProviderRequired";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_RESPROVIDER_ID = "@ResProviderId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALResources"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALResources()
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
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_RESOURCE_ID, ds.Resources.ResourceIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_RESOURCE_ID, ds.Resources.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_NAME, ds.Resources.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.Resources.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_FACILITY_ID, ds.Resources.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_DESCCRIPTION, ds.Resources.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DURATION, ds.Resources.DurationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_PROVIDER_REQUIRED, ds.Resources.ProviderRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Resources.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.Resources.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.Resources.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Resources.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.Resources.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_RESPROVIDER_ID, ds.Resources.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <returns></returns>
        public DSProfile LoadResources(long ResourceId, string ShortName, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (ResourceId <= 0)
                    dbManager.AddParameters(0, PARM_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RESOURCE_ID, ResourceId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RESOURCES_SELECT, ds, ds.Resources.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::LoadResources", PROC_RESOURCES_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the resources.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupResources(string Active)
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

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RESOURCES_LOOKUP, ds, ds.Resources.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::LookupResources", PROC_RESOURCES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the resources.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateResources(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Resources.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_RESOURCES_UPDATE, ds, ds.Resources.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Resources.Rows[0][ds.Resources.ResourceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::UpdateResources", PROC_RESOURCES_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the resources.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile DeleteResources(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Resources;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_RESOURCE_ID, ds.Resources.ResourceIdColumn.ColumnName, DbType.Int64);
                ds = (DSProfile)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_RESOURCES_DELETE, ds, ds.Resources.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null && ds.Resources.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Resources.Rows[0][ds.Resources.ResourceIdColumn].ToString(), "", false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::Deleteresources", PROC_RESOURCES_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the resources.
        /// </summary>
        /// <param name="ResourceIds">The resource ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteResources(string ResourceIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadResources(Convert.ToInt64(ResourceIds), null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Resources;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RESOURCE_ID, ResourceIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_RESOURCES_DELETE);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_RESOURCES_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Resources.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Resources.Rows[0][ds.Resources.ResourceIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::DeleteResources", PROC_RESOURCES_DELETE, ex);
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
        /// Inserts the resources.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertResources(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Resources.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_RESOURCES_INSERT, ds, ds.Resources.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Resources.Rows[0][ds.Resources.ResourceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Resources.TableName].Rows[0][ds.Resources.ResourceIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALResources::InsertResources", PROC_RESOURCES_INSERT, ex);
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
