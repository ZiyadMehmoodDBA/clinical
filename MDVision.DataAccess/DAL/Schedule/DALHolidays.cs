using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Schedule
{
    public class DALHolidays
    {
        #region Variable
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SCHEDULE_HOLIDAYS_INSERT = "Provider.sp_ScheduleHolidaysInsert";
        private const string PROC_SCHEDULE_HOLIDAYS_UPDATE = "Provider.sp_ScheduleHolidaysUpdate";
        private const string PROC_SCHEDULE_HOLIDAYS_DELETE = "Provider.sp_ScheduleHolidaysDelete";
        private const string PROC_SCHEDULE_HOLIDAYS_SELECT = "Provider.sp_ScheduleHolidaysSelect";
        #endregion

        #region "Parameters"
        private const string PARM_SCHEDULE_HOLIDAYS_ID = "@ScheduleHolidayId";
        private const string PARM_SCHEDULE_HOLIDAYS_DATE = "@HolidayOn";
        private const string PARM_SCHEDULE_HOLIDAYS_DESC = "@HolidayDescription";
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
        /// Initializes a new instance of the <see cref="DALHolidays"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALHolidays()
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
                dbManager.AddParameters(0, PARM_SCHEDULE_HOLIDAYS_ID, ds.ScheduleHolidays.ScheduleHolidayIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SCHEDULE_HOLIDAYS_ID, ds.ScheduleHolidays.ScheduleHolidayIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SCHEDULE_HOLIDAYS_DATE, ds.ScheduleHolidays.HolidayOnColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SCHEDULE_HOLIDAYS_DESC, ds.ScheduleHolidays.HolidayDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ScheduleHolidays.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ScheduleHolidays.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ScheduleHolidays.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ScheduleHolidays.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ScheduleHolidays.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ENTITY_ID, ds.ScheduleHolidays.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the holidays.
        /// </summary>
        /// <param name="HolidayId">The holiday identifier.</param>
        /// <param name="Date">The date.</param>
        /// <param name="Holiday">The holiday.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup LoadHolidays(long HolidayId, string Date, string Holiday, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Date == "")
                    Date = null;

                if (Holiday == "")
                    Holiday = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (HolidayId <= 0)
                    dbManager.AddParameters(0, PARM_SCHEDULE_HOLIDAYS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SCHEDULE_HOLIDAYS_ID, HolidayId);
                dbManager.AddParameters(1, PARM_SCHEDULE_HOLIDAYS_DATE, Date);
                dbManager.AddParameters(2, PARM_SCHEDULE_HOLIDAYS_DESC, Holiday);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.ScheduleHolidays.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_HOLIDAYS_SELECT, ds, ds.ScheduleHolidays.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHolidays::LoadHolidays", PROC_SCHEDULE_HOLIDAYS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the holidays.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup UpdateHolidays(DSScheduleSetup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleHolidays.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSScheduleSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_HOLIDAYS_UPDATE, ds, ds.ScheduleHolidays.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleHolidays.Rows[0][ds.ScheduleHolidays.ScheduleHolidayIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHolidays::UpdateHolidays", PROC_SCHEDULE_HOLIDAYS_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Deletes the holidays.
        /// </summary>
        /// <param name="HolidayId">The holiday identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteHolidays(string HolidaysId)
        {
            string returnVal = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSScheduleSetup ds = LoadHolidays(Convert.ToInt64(HolidaysId), null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleHolidays;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SCHEDULE_HOLIDAYS_ID, HolidaysId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHEDULE_HOLIDAYS_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ScheduleHolidays.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleHolidays.Rows[0][ds.ScheduleHolidays.ScheduleHolidayIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHolidays::DeleteHolidays", PROC_SCHEDULE_HOLIDAYS_DELETE, ex);
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
        /// Inserts the holidays.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSScheduleSetup InsertHolidays(DSScheduleSetup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ScheduleHolidays.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_HOLIDAYS_INSERT, ds, ds.ScheduleHolidays.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ScheduleHolidays.Rows[0][ds.ScheduleHolidays.ScheduleHolidayIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHolidays::InsertHolidays", PROC_SCHEDULE_HOLIDAYS_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);  
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

    }
}
