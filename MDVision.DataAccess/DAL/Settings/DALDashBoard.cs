using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using MDVision.Model.Dashboard;
using MDVision.Common.Logging;


namespace MDVision.DataAccess.DAL.Settings
{
    public class DALDashBoard
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_DASHBOARD_SELECT = "System.sp_DashboardSettingsSelect";
        private const string PROC_DASHBOARD_WIDGET_SELECT = "System.sp_DBWidgetsSelect";
        private const string PROC_DASHBOARD_KPI_SELECT = "System.sp_DBKPISelect";
        private const string PROC_DASHBOARD_INSERT = "System.sp_DashboardSettingsInsert";
        private const string PROC_DASHBOARD_DELETE = "System.sp_DashboardSettingsDelete";
        private const string PROC_DASHBOARD_UPDATE = "System.sp_DashboardSettingsUpdate";
        private const string PROC_DASHBOARD_PATIENTS_VISITS_SELECT = "Reports.MthPatientVisits";

        private const string PROC_COLLECTED_COPAY_SELECT = "Reports.CollectedCopayment";
        private const string PROC_COLLECTED_REVENUE_SELECT = "Reports.sp_CollectedRevenue";
        private const string PROC_CHARGE_PAYMENT_SELECT = "Reports.sp_ChargesAndPayments";
        private const string PROC_DBUSER_KPIUPDATE = "System.sp_DBUserKPIUpdate";
        private const string PROC_DASHBOARD_PATIENT_CHANGES = "System.sp_DashBoardPatientChanges";
        private const string PROC_ACCOUNT_RECEIVABLE_SELECT = "Patient.sp_AccountReceivable";
        private const string PROC_DASHBOARD_WIDGET_COUNT = "System.sp_DashBoardWidgetsCount";
        private const string PROC_DASHBOARD_KPIS_LOAD = "System.sp_DashboardKPISLoad";
        private const string PROC_D_PATIENT_CHANGES = "System.sp_D_PatientChanges";
        private const string PROC_PATIENT_PORTAL_ACCOUNTS = "Patient.sp_PatientPortalAccounts";
        #endregion
        #region "Parameters"
        private const string PARM_DBSETTING_ID = "@DBSId ";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_DBSETTING_TYPE = "@DBSType";
        private const string PARM_WIDGET_ID = "@WidgetsId";
        private const string PARM_KPI_ID = "@KPIId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_NEWKPI_ID = "@NewKPIId";
        private const string PARM_OLDKPI_ID = "@OldKPIId";
        private const string PARM_GRAPH_ID = "@GraphId";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";


        private const string PARM_MONTH = "@Month";
        private const string PARM_YEAR = "@Year";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_CHECKED_IN = "@IsCheckedIn";
        private const string PARM_ASSIGNED_TO_ID = "@AssignedToId";
        private const string PARM_MSG_TYPE_NOT = "@MsgTypeNot";
        private const string PARM_NOTE_STATUS = "@NoteStatus";
        private const string PARM_MSG_STATUS_ID = "@MsgStatusId";
        private const string PARM_MSG_TYPE_SHORT_NAME = "@MsgTypeShortName";
        private const string PARM_FROM_DATE = "@FromEntryDate";
        private const string PARM_TO_DATE = "@ToEntryDate";
        private const string PARM_APPOINMENT_DATE = "@AppointmentDate";
        private const string PARM_PROFILE_NAME = "@ProfileName";

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_DOB = "@DOB";
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALAppointmentStatus"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALDashBoard()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        public DALDashBoard(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        private IContainer components;
        private SharedVariable sharedobj;

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
        private void CreateParameters(IDBManager dbManager, DSSettings ds, Boolean IsInsert)
        {
            //dbManager.CreateParameters(ds.Tables[ds.DashboardSettings.TableName].Columns.Count);
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_DBSETTING_ID, ds.DashboardSettings.DBSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DBSETTING_ID, ds.DashboardSettings.DBSIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_WIDGET_ID, ds.DashboardSettings.WidgetsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_KPI_ID, ds.DashboardSettings.KPIIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_GRAPH_ID, ds.DashboardSettings.GraphIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_USER_ID, ds.DashboardSettings.UserIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DBSETTING_TYPE, ds.DashboardSettings.DBSTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ENTITY, ds.DashboardSettings.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.DashboardSettings.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.DashboardSettings.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.DashboardSettings.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.DashboardSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.DashboardSettings.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_IS_DEFAULT, ds.DashboardSettings.IsDefaultColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_ERROR_MESSAGE, ds.DashboardSettings.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>DSScheduleSetup.</returns>
        public DSSettings LoadDashBoardSetting(long DBSId, long UserId, string DBSType)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(4);

                if (DBSId <= 0)
                    dbManager.AddParameters(0, PARM_DBSETTING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DBSETTING_ID, DBSId);

                dbManager.AddParameters(1, PARM_USER_ID, UserId);

                if (DBSType == "")
                    dbManager.AddParameters(2, PARM_DBSETTING_TYPE, null);
                else
                    dbManager.AddParameters(2, PARM_DBSETTING_TYPE, DBSType);

                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_SELECT, ds, ds.DashboardSettings.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadDashBoardSetting", PROC_DASHBOARD_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadDashBoardWidget(int? WidgetId)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(3);

                if (WidgetId <= 0)
                    dbManager.AddParameters(0, PARM_WIDGET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_WIDGET_ID, WidgetId);



                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_WIDGET_SELECT, ds, ds.DashBoardWidgets.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("LoadDashBoardWidget::LoadDashBoardWidget", PROC_DASHBOARD_WIDGET_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSettings LoadDashBoardKPI(int? KPIId)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(1);

                if (KPIId <= 0)
                    dbManager.AddParameters(0, PARM_KPI_ID, null);
                else
                    dbManager.AddParameters(0, PARM_KPI_ID, KPIId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_KPI_SELECT, ds, ds.DashBoardGraph.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadDashBoardSetting", PROC_DASHBOARD_KPI_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadPatientVisitsKPI(long Entity)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_PATIENTS_VISITS_SELECT, ds, ds.PatientVisitsKPI.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadPatientVisitsKPI", PROC_DASHBOARD_PATIENTS_VISITS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings UpdateDashboardSetting(DSSettings ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSSettings)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_UPDATE, ds, ds.DashboardSettings.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSetting::UpdateDashboardsetting", PROC_DASHBOARD_UPDATE, ex);
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


        public string UpdateUserKPI(int OldKPIId, int NewKPIId)
        {
            string returnValue = string.Empty;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (NewKPIId <= 0)
                    dbManager.AddParameters(0, PARM_NEWKPI_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NEWKPI_ID, NewKPIId);
                if (OldKPIId <= 0)
                    dbManager.AddParameters(1, PARM_OLDKPI_ID, null);
                else
                    dbManager.AddParameters(1, PARM_OLDKPI_ID, OldKPIId);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY, MDVSession.Current.EntityId);

                dbManager.AddParameters(4, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(7, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(8, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DBUSER_KPIUPDATE).ToString();

                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSetting::UpdateDashboardsetting", PROC_DASHBOARD_UPDATE, ex);
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

        public DSSettings LoadCollectedCopayKPI(long Entity)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COLLECTED_COPAY_SELECT, ds, ds.CollectedCopaymentKPI.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadCollectedCopayKPI", PROC_COLLECTED_COPAY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadCollectedRevenueKPI(long Entity)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COLLECTED_REVENUE_SELECT, ds, ds.CollectedRevenueKPI.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadCollectedRevenueKPI", PROC_COLLECTED_REVENUE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSSettings LoadDashBoardKPIS(long EntityId, long ProviderId, long FacilityId)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (EntityId != 0)
                    dbManager.AddParameters(0, PARM_ENTITY, EntityId);
                else
                    dbManager.AddParameters(0, PARM_ENTITY, null);

                if (FacilityId != 0)
                    dbManager.AddParameters(1, PARM_FACILITY_ID, FacilityId);
                else
                    dbManager.AddParameters(1, PARM_FACILITY_ID, null);

                if (ProviderId != 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);


                List<string> Tables = new List<string>();
                Tables.Add(ds.CollectedRevenueKPI.TableName);
                Tables.Add(ds.PatientVisitsKPI.TableName);
                Tables.Add(ds.CollectedCopaymentKPI.TableName);
                Tables.Add(ds.ChargesPaymentsKPI.TableName);
                Tables.Add(ds.AccountReceivable.TableName);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_KPIS_LOAD, ds, Tables);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadDashBoardKPIS", PROC_DASHBOARD_KPIS_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadChargesAndPaymentsKPI(long Entity)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHARGE_PAYMENT_SELECT, ds, ds.ChargesPaymentsKPI.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadChargesAndPaymentsKPI", PROC_CHARGE_PAYMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadDashBoardPatientChanges(int Month, int Year, string ProfileName, int PageNumber, int RowspPage)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);

                if (Month <= 0)
                    dbManager.AddParameters(0, PARM_MONTH, null);
                else
                    dbManager.AddParameters(0, PARM_MONTH, Month);

                if (Year <= 0)
                    dbManager.AddParameters(1, PARM_YEAR, null);
                else
                    dbManager.AddParameters(1, PARM_YEAR, Year);

                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowspPage);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.PatientChanges.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (string.IsNullOrEmpty(ProfileName))
                    dbManager.AddParameters(6, PARM_PROFILE_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_PROFILE_NAME, ProfileName);

                if (MDVSession.Current.AppUserId < 1)
                    dbManager.AddParameters(7, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_PATIENT_CHANGES, ds, ds.PatientChanges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoard::LoadDashBoardPatientChanges", PROC_DASHBOARD_PATIENT_CHANGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadDashBoardWidgetsCount(long Month, long Year, long CheckedId, DateTime AppointmentDate, long AssignedToId, string MsgtypeNot, string NoteStatus, long MsgStatusId, string MsgTypeShortName, DateTime FromDate, DateTime ToDate, long PageNumber, long RowspPage)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (CheckedId <= 0)
                    dbManager.AddParameters(2, PARM_CHECKED_IN, null);
                else
                    dbManager.AddParameters(2, PARM_CHECKED_IN, CheckedId);

                dbManager.AddParameters(3, PARM_APPOINMENT_DATE, AppointmentDate);


                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowspPage);


                dbManager.AddParameters(6, PARM_ASSIGNED_TO_ID, AssignedToId);
                dbManager.AddParameters(7, PARM_MSG_TYPE_NOT, MsgtypeNot);
                dbManager.AddParameters(8, PARM_NOTE_STATUS, NoteStatus);
                dbManager.AddParameters(9, PARM_MSG_STATUS_ID, MsgStatusId);
                dbManager.AddParameters(10, PARM_MSG_TYPE_SHORT_NAME, MsgTypeShortName);
                dbManager.AddParameters(11, PARM_FROM_DATE, FromDate);
                dbManager.AddParameters(12, PARM_TO_DATE, ToDate);

                if (Month <= 0)
                    dbManager.AddParameters(13, PARM_MONTH, null);
                else
                    dbManager.AddParameters(13, PARM_MONTH, Month);

                if (Year <= 0)
                    dbManager.AddParameters(14, PARM_YEAR, null);
                else
                    dbManager.AddParameters(14, PARM_YEAR, Year);


                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DASHBOARD_WIDGET_COUNT, ds, ds.DashboardSettings.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoard::LoadDashBoardWidgetsCount", PROC_DASHBOARD_WIDGET_COUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSettings LoadAccountReceivable(long Entity)
        {
            DSSettings ds = new DSSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_ENTITY, Entity);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ACCOUNT_RECEIVABLE_SELECT, ds, ds.AccountReceivable.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoardSetting::LoadAccountReceivable", PROC_ACCOUNT_RECEIVABLE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<DPatientChangeModel> LoadDashboardPatientChanges(int Month, int Year, string ProfileName,int ProviderId, int PageNumber, int RowspPage)
        {
            List<DPatientChangeModel> listModel = new List<DPatientChangeModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (Month <= 0)
                    dbManager.AddParameters(0, PARM_MONTH, null);
                else
                    dbManager.AddParameters(0, PARM_MONTH, Month);

                if (Year <= 0)
                    dbManager.AddParameters(1, PARM_YEAR, null);
                else
                    dbManager.AddParameters(1, PARM_YEAR, Year);

                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowspPage);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                if (string.IsNullOrEmpty(ProfileName))
                    dbManager.AddParameters(6, PARM_PROFILE_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_PROFILE_NAME, ProfileName);

                if (MDVSession.Current.AppUserId < 1)
                    dbManager.AddParameters(7, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ProviderId <= 0)
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, ProviderId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_D_PATIENT_CHANGES);
                DPatientChangeModel model = null;
                while (reader.Read())
                {
                    model = new DPatientChangeModel();
                    model.PatientId = Convert.ToString(reader["PatientId"]);
                    model.PatientName = Convert.ToString(reader["PatientName"]);
                    model.ProviderName = Convert.ToString(reader["ProviderName"]);
                    model.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                    model.ProfileName = Convert.ToString(reader["ProfileName"]);
                    model.RecordCount = Convert.ToString(reader["RecordCount"]);
                    model.DBAuditAction = Convert.ToString(reader["DBAuditAction"]);
                    model.ColumnName = Convert.ToString(reader["ColumnName"]);
                    model.OldValue = Convert.ToString(reader["OldValue"]);
                    model.NewValue = Convert.ToString(reader["NewValue"]);
                    model.UserName = Convert.ToString(reader["UserName"]);
                    model.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                    listModel.Add(model);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoard::LoadDashboardPatientChanges", PROC_D_PATIENT_CHANGES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Active Accounts

        public PatientPortalAccounts LoadPatientPortalAccounts(long EntityId, string UserId, DateTime? DOB = null, string PatientId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            PatientPortalAccounts patPortalAccountsModel = new PatientPortalAccounts();
            patPortalAccountsModel.listActiveAccounts = new List<ActiveAccountsModel>();
            ActiveAccountsModel model = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (PageNumber == 0)
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(1, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(1, PARM_ROWS_PER_PAGE, RowspPage);

                dbManager.AddParameters(2, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                if (EntityId == 0)
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);

                if (DOB != null)
                    dbManager.AddParameters(4, PARM_DOB, DOB);
                else
                    dbManager.AddParameters(4, PARM_DOB, null);

                dbManager.AddParameters(5, PARM_PATIENT_ID, PatientId);

                if (string.IsNullOrEmpty(UserId))
                    dbManager.AddParameters(6, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_USER_ID, UserId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_PORTAL_ACCOUNTS);

                while (reader.Read())
                {
                    model = new ActiveAccountsModel();
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.PatientName = !String.IsNullOrEmpty(reader["Patient"].ToString()) ? reader["Patient"].ToString() : "";
                    model.AccountNo = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.DOB = Convert.ToDateTime(reader["DOB"]);
                    model.Insurance = !String.IsNullOrEmpty(reader["Insurance"].ToString()) ? reader["Insurance"].ToString() : "";
                    model.Provider = !String.IsNullOrEmpty(reader["Provider"].ToString()) ? reader["Provider"].ToString() : "";
                    model.PatientPortalAccountsCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    patPortalAccountsModel.listActiveAccounts.Add(model);
                }
                return patPortalAccountsModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDashBoard::LoadPatientPortalAccounts", PROC_PATIENT_PORTAL_ACCOUNTS, ex);
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

