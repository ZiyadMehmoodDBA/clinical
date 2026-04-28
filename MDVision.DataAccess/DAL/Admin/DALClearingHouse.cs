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

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALClearingHouse
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_CLEARING_HOUSE_INSERT = "Provider.sp_ClearingHouseInsert";
        private const string PROC_CLEARING_HOUSE_UPDATE = "Provider.sp_ClearingHouseUpdate";
        private const string PROC_CLEARING_HOUSE_DELETE = "Provider.sp_ClearingHouseDelete";
        private const string PROC_CLEARING_HOUSE_SELECT = "Provider.sp_ClearingHouseSelect";
        private const string PROC_CLEARING_HOUSE_LOOKUP = "Provider.sp_ClearingHouseLookup";
        private const string PROC_GET_ClEARING_HOUSE_ID = "Provider.sp_GetClearingHouseId";
        private const string PROC_CLEARING_HOUSE_TYPE_LOOKUP = "Provider.sp_ClearingHouseTypeLookup";
        #endregion

        #region "Parameters"
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_TYPE_ID = "@TypeId";
        private const string PARM_CLAIM_SUBMIT_ALLOWED = "@ClaimSubmitAllowed";
        private const string PARM_CLAIM_STATUS_ALLOWED = "@ClaimStatusAllowed";
        private const string PARM_ELIGIBILITY_ALLOWED = "@EligibilityAllowed";
        private const string PARM_ELECTRONIC_EOB_ALLOWED = "@ElectronicEOBAllowed";
        private const string PARM_SECONDARY_ALLOWED = "@SecondaryAllowed";
        private const string PARM_FTP = "@FTP";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_USER_PASSWORD = "@UserPassword";
        private const string PARM_FTP_PORTNO = "@FTP_PORTNO";
        private const string PARM_URL = "@URL";
        private const string PARM_IN_UPLOADED = "@IN_UPLOADED";
        private const string PARM_IN_STATEMENTS = "@IN_STATEMENTS";
        private const string PARM_OUT_REPORTS = "@OUT_REPORTS";
        private const string PARM_OUT_277 = "@OUT_277";
        private const string PARM_OUT_271 = "@OUT_271";
        private const string PARM_OUT_997 = "@OUT_997";
        private const string PARM_OUT_835 = "@OUT_835";
        private const string PARM_FTP_HOSTKEY = "@FTP_HOSTKEY";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_ELIGIBILITY_USERNAME = "@PatientEligibilityUserName";
        private const string PARM_PATIENT_ELIGIBILITY_PASSWORD = "@PatientEligibilityUserPassword";
        private const string PARM_PROFESSIONAL_CLAIM_EXTENSION = "@ProfessionalClaimExtension";
        private const string PARM_INSTITUTIONAL_CLAIM_EXTENSION = "@InstitutionalClaimExtension";
        private const string PARM_PATIENT_STATEMENT_CLAIM_EXTENSION = "@PatientStatementExtension";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALClearingHouse"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALClearingHouse()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALClearingHouse(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

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
        private void CreateParameters(IDBManager dbManager, DSEDI ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(33);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ds.ClearingHouse.ClearingHouseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ds.ClearingHouse.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.ClearingHouse.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_TYPE_ID, ds.ClearingHouse.TypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_CLAIM_SUBMIT_ALLOWED, ds.ClearingHouse.ClaimSubmitAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CLAIM_STATUS_ALLOWED, ds.ClearingHouse.ClaimStatusAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_ELIGIBILITY_ALLOWED, ds.ClearingHouse.EligibilityAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_ELECTRONIC_EOB_ALLOWED, ds.ClearingHouse.ElectronicEOBAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_SECONDARY_ALLOWED, ds.ClearingHouse.SecondaryAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_FTP, ds.ClearingHouse.FTPColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ClearingHouse.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.ClearingHouse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.ClearingHouse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.ClearingHouse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.ClearingHouse.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_ENTITY_ID, ds.ClearingHouse.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_USER_NAME, ds.ClearingHouse.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_USER_PASSWORD, ds.ClearingHouse.UserPasswordColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(17, PARM_FTP_PORTNO, ds.ClearingHouse.FTP_PORTNOColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

            dbManager.AddParameters(18, PARM_URL, ds.ClearingHouse.URLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_IN_UPLOADED, ds.ClearingHouse.IN_UPLOADEDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_IN_STATEMENTS, ds.ClearingHouse.IN_STATEMENTSColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_OUT_REPORTS, ds.ClearingHouse.OUT_REPORTSColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_OUT_277, ds.ClearingHouse.OUT_277Column.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_OUT_271, ds.ClearingHouse.OUT_271Column.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_OUT_997, ds.ClearingHouse.OUT_997Column.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_OUT_835, ds.ClearingHouse.OUT_835Column.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_FTP_PORTNO, ds.ClearingHouse.FTP_PORTNOColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_FTP_HOSTKEY, ds.ClearingHouse.FTP_HOSTKEYColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_PATIENT_ELIGIBILITY_USERNAME, ds.ClearingHouse.PatientEligibilityUserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_PATIENT_ELIGIBILITY_PASSWORD, ds.ClearingHouse.PatientEligibilityUserPasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_PROFESSIONAL_CLAIM_EXTENSION, ds.ClearingHouse.ProfessionalClaimExtensionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_INSTITUTIONAL_CLAIM_EXTENSION, ds.ClearingHouse.InstitutionalClaimExtensionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_PATIENT_STATEMENT_CLAIM_EXTENSION, ds.ClearingHouse.PatientStatementExtensionColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the clearing house.
        /// </summary>
        /// <param name="ClearingHouseId">The clearing house identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Type">The type.</param>
        /// <returns></returns>
        public DSEDI LoadClearingHouse(long ClearingHouseId, string ShortName, string TypeId, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (TypeId == "")
                    TypeId = null;

                if (EntityId == "")
                    EntityId = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (ClearingHouseId <= 0)
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_TYPE_ID, TypeId);
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
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.ClearingHouse.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_SELECT, ds, ds.ClearingHouse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::LoadClearingHouse", PROC_CLEARING_HOUSE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ClearingHouseId"></param>
        /// <param name="ShortName"></param>
        /// <param name="TypeId"></param>
        /// <param name="EntityId"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSEDI LoadClearingHouse(SharedVariable SharedVariable, long ClearingHouseId, string ShortName, string TypeId, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (TypeId == "")
                    TypeId = null;

                if (EntityId == "")
                    EntityId = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (ClearingHouseId <= 0)
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_TYPE_ID, TypeId);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, SharedVariable.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, SharedVariable.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.ClearingHouse.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_SELECT, ds, ds.ClearingHouse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALClearingHouse::LoadClearingHouse", PROC_CLEARING_HOUSE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the clearing house.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateClearingHouse(DSEDI ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClearingHouse.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_UPDATE, ds, ds.ClearingHouse.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ClearingHouse.Rows[0][ds.ClearingHouse.ClearingHouseIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::UpdateClearingHouse", PROC_CLEARING_HOUSE_UPDATE, ex);
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
        /// Deletes the clearing house.
        /// </summary>
        /// <param name="ClearingHouseId">The clearing house identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteClearingHouse(string ClearingHouseId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSEDI ds = LoadClearingHouse(Convert.ToInt64(ClearingHouseId),null,null,null,null,1,1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClearingHouse;
                dbManager.AddParameters(0, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_DELETE);
                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.ClearingHouse.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ClearingHouse.Rows[0][ds.ClearingHouse.ClearingHouseIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::DeleteClearingHouse", PROC_CLEARING_HOUSE_DELETE, ex);
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
        /// Inserts the clearing house.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertClearingHouse(DSEDI ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClearingHouse.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_INSERT, ds, ds.ClearingHouse.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ClearingHouse.Rows[0][ds.ClearingHouse.ClearingHouseIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::InsertClearingHouse", PROC_CLEARING_HOUSE_INSERT, ex);
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

        #region "Lookups"
        /// <summary>
        /// Lookups the clearing house.
        /// </summary>
        /// <returns></returns>
        public DSEDILookup LookupClearingHouse()
        {
            DSEDILookup ds = new DSEDILookup();
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
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_LOOKUP, ds, ds.ClearingHouse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::LookupClearingHouse", PROC_CLEARING_HOUSE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSEDILookup GetClearingHouseId()
        {
            DSEDILookup ds = new DSEDILookup();
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
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_ClEARING_HOUSE_ID, ds, ds.ClearingHouse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::GetClearingHouseId", PROC_GET_ClEARING_HOUSE_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the type of the clearing house.
        /// </summary>
        /// <returns></returns>
        public DSEDILookup LookupClearingHouseType()
        {
            DSEDILookup ds = new DSEDILookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLEARING_HOUSE_TYPE_LOOKUP, ds, ds.ClearingHouseType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClearingHouse::LookupClearingHouseType", PROC_CLEARING_HOUSE_TYPE_LOOKUP, ex);
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
