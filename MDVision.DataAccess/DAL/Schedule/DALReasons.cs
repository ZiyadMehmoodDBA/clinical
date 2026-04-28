using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Schedule
{
    public class DALReasons
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SCHEDULE_REASONS_INSERT = "Provider.sp_ScheduleReasonsInsert";
        private const string PROC_SCHEDULE_REASONS_UPDATE = "Provider.sp_ScheduleReasonsUpdate";
        private const string PROC_SCHEDULE_REASONS_DELETE = "Provider.sp_ScheduleReasonsDelete";
        private const string PROC_SCHEDULE_REASONS_SELECT = "Provider.sp_ScheduleReasonsSelect";
        private const string PROC_SCHEDULE_REASONS_LOOKUP = "Provider.sp_ScheduleReasonsLookup";
        #endregion

        #region "Parameters"
        private const string PARM_SCHEDULE_REASONS_ID = "@ScheduleReasonId";
        private const string PARM_SCHEDULE_REASONS_SHORT_NAME = "@ShortName";
        private const string PARM_SCHEDULE_REASONS_DESC = "@Description";
        private const string PARM_SCHEDULE_REASONS_DURATION = "@Duration";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALReasons"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALReasons()
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
        private void CreateParameters(IDBManager dbManager, DSScheduleSetup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SCHEDULE_REASONS_ID, ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SCHEDULE_REASONS_ID, ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SCHEDULE_REASONS_SHORT_NAME, ds.ScheduleReasons.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SCHEDULE_REASONS_DESC, ds.ScheduleReasons.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_SCHEDULE_REASONS_DURATION, ds.ScheduleReasons.DurationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.ScheduleReasons.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.ScheduleReasons.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.ScheduleReasons.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.ScheduleReasons.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.ScheduleReasons.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_ENTITY_ID, ds.ScheduleReasons.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the reasons.
        /// </summary>
        /// <param name="ReasonId">The reason identifier.</param>
        /// <param name="Reason">The reason.</param>
        /// <param name="Description">The description.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup LoadReasons(long ReasonId, string ShortName, string Description, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (IsActive == "")
                    IsActive = null;

                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (ReasonId <= 0)
                    dbManager.AddParameters(0, PARM_SCHEDULE_REASONS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SCHEDULE_REASONS_ID, ReasonId);
                dbManager.AddParameters(1, PARM_SCHEDULE_REASONS_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_SCHEDULE_REASONS_DESC, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(4, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.ScheduleReasons.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASONS_SELECT, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::LoadReasons", PROC_SCHEDULE_REASONS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the reasons.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup UpdateReasons(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleReasons.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASONS_UPDATE, ds, ds.ScheduleReasons.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleReasons.Rows[0][ds.ScheduleReasons.ScheduleReasonIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::UpdateReasons", PROC_SCHEDULE_REASONS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the reasons.
        /// </summary>
        /// <param name="ReasonId">The reason identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteReasons(string ReasonId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                //DSScheduleSetup ds = LoadReasons(Convert.ToInt64(ReasonId), null, null,null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleReasons;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SCHEDULE_REASONS_ID, ReasonId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHEDULE_REASONS_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ScheduleReasons.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleReasons.Rows[0][ds.ScheduleReasons.ScheduleReasonIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::DeleteReasons", PROC_SCHEDULE_REASONS_DELETE, ex);
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
        /// Inserts the reasons.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup InsertReasons(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleReasons.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASONS_INSERT, ds, ds.ScheduleReasons.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleReasons.Rows[0][ds.ScheduleReasons.ScheduleReasonIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::InsertReasons", PROC_SCHEDULE_REASONS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the reasons.
        /// </summary>
        /// <returns>DSScheduleLookups.</returns>
        public DSScheduleLookups LookupReasons(string Active, string EntityId)
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_REASONS_LOOKUP, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::LookupReasons", PROC_SCHEDULE_REASONS_LOOKUP, ex);
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
