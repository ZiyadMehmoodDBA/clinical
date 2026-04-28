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
    public class DALBlockHours
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SCHEDULE_BLOCKHOURS_INSERT = "Provider.sp_SchBlockHoursInsert";
        private const string PROC_SCHEDULE_BLOCKHOURS_UPDATE = "Provider.sp_SchBlockHoursUpdate";
        private const string PROC_SCHEDULE_BLOCKHOURS_DELETE = "Provider.sp_SchBlockHoursDelete";
        private const string PROC_SCHEDULE_BLOCKHOURS_SELECT = "Provider.sp_SchBlockHoursSelect";
        private const string PROC_SCHEDULE_BLOCKHOURS_LOOKUP = "Provider.sp_ScheduleReasonsLookupAutoComplete";
        #endregion

        #region "Parameters"
        private const string PARM_BLOCKHOURS_ID = "@BlockHoursId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_BLOCKREASON_ID = "@BlockReasonId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_FROM_DATE = "@FromDate";
        private const string PARM_TO_DATE = "@ToDate";
        private const string PARM_FROM_TIME = "@FromTime";
        private const string PARM_TO_TIME = "@ToTime";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_NAME = "@Name";
        private const string PARM_COLOR = "@Color";
        private const string PARM_OVERLAPPING_ALLOWED = "@OverLappingAllowed";
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALBlockHours"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALBlockHours()
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
            dbManager.CreateParameters(16);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BLOCKHOURS_ID, ds.SchBlockHours.BlockHoursIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BLOCKHOURS_ID, ds.SchBlockHours.BlockHoursIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.SchBlockHours.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RESOURCE_ID, ds.SchBlockHours.ResourceIdColumn.ColumnName, DbType.Int64);
           if( ds.SchBlockHours[0].FacilityId!=0)
            dbManager.AddParameters(3, PARM_FACILITY_ID, ds.SchBlockHours.FacilityIdColumn.ColumnName, DbType.Int64);
           else
            dbManager.AddParameters(3, PARM_FACILITY_ID, null);
            dbManager.AddParameters(4, PARM_BLOCKREASON_ID, ds.SchBlockHours.BlockReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.SchBlockHours.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_FROM_DATE, ds.SchBlockHours.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_TO_DATE, ds.SchBlockHours.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_FROM_TIME, ds.SchBlockHours.FromTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_TO_TIME, ds.SchBlockHours.ToTimeColumn.ColumnName, DbType.String);
           // dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.SchBlockHours.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.SchBlockHours.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.SchBlockHours.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.SchBlockHours.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.SchBlockHours.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_COLOR, ds.SchBlockHours.ColorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_OVERLAPPING_ALLOWED, ds.SchBlockHours.OverLappingAllowedColumn.ColumnName, DbType.Byte);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <param name="Provider">The provider.</param>
        /// <param name="Resource">The resource.</param>
        /// <param name="Facility">The facility.</param>
        /// <param name="FromDate">From date.</param>
        /// <param name="ToDate">To date.</param>
        /// <param name="FromTime">From time.</param>
        /// <param name="ToTime">To time.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup LoadBlockHours(long BlockHoursId, string Provider, string Resource, string Facility, string FromDate, string ToDate, string FromTime, string ToTime,  int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                if (Provider == ""||Provider=="-1")
                    Provider = null;

                if (Resource == "" || Resource == "-1")
                    Resource = null;

                if (Facility == "" || Facility == "-1")
                    Facility = null;

                if (FromDate == "" )
                    FromDate = null;

                if (ToDate == "" )
                    ToDate = null;

                if (FromTime == "" )
                    FromTime = null;

                if (ToTime == "" )
                    ToTime = null;

                

                dbManager.Open();
                dbManager.CreateParameters(12);

                if (BlockHoursId <= 0)
                    dbManager.AddParameters(0, PARM_BLOCKHOURS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BLOCKHOURS_ID, BlockHoursId);
                dbManager.AddParameters(1, PARM_PROVIDER_ID, Provider);
                dbManager.AddParameters(2, PARM_RESOURCE_ID, Resource);
                dbManager.AddParameters(3, PARM_FACILITY_ID, Facility);
                dbManager.AddParameters(4, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(5, PARM_TO_DATE, ToDate);
                dbManager.AddParameters(6, PARM_FROM_TIME, FromTime);
                dbManager.AddParameters(7, PARM_TO_TIME, ToTime);
             //   dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.SchBlockHours.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(11, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_SELECT, ds, ds.SchBlockHours.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBlockHours::LoadBlockHours", PROC_SCHEDULE_BLOCKHOURS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the block hours.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup UpdateBlockHours(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SchBlockHours.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_UPDATE, ds, ds.SchBlockHours.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SchBlockHours.Rows[0][ds.SchBlockHours.BlockHoursIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBlockHours::UpdateBlockHours", PROC_SCHEDULE_BLOCKHOURS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteBlockHours(string BlockHoursId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                //DSScheduleSetup ds = LoadBlockHours(Convert.ToInt64(BlockHoursId), null, null, null,null,null,null,null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SchBlockHours;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BLOCKHOURS_ID, BlockHoursId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.SchBlockHours.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SchBlockHours.Rows[0][ds.SchBlockHours.BlockHoursIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBlockHours::DeleteBlockHours", PROC_SCHEDULE_BLOCKHOURS_DELETE, ex);
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
        /// Inserts the block hours.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup InsertBlockHours(DSScheduleSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();;
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SchBlockHours.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_INSERT, ds, ds.SchBlockHours.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SchBlockHours.Rows[0][ds.SchBlockHours.BlockHoursIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBlockHours::InsertBlockHours", PROC_SCHEDULE_BLOCKHOURS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSScheduleLookups LookupReasonsAutoComplete(string Active, string Name)
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(3, PARM_NAME, Name);

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_LOOKUP, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReasons::LookupReasons", PROC_SCHEDULE_BLOCKHOURS_LOOKUP, ex);
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
