using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALEDIReceiverSetup
    {
        #region Variable
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_EDI_RECEIVER_SETUP_INSERT = "Provider.sp_EDIReceiverSetupInsert";
        private const string PROC_EDI_RECEIVER_SETUP_UPDATE = "Provider.sp_EDIReceiverSetupUpdate";
        private const string PROC_EDI_RECEIVER_SETUP_DELETE = "Provider.sp_EDIReceiverSetupDelete";
        private const string PROC_EDI_RECEIVER_SETUP_SELECT = "Provider.sp_EDIReceiverSetupSelect";
        private const string PROC_EDI_RECEIVER_SETUP_LOOKUP = "Provider.sp_EDIReceiverSetupLookup";
        #endregion

        #region "Parameters"
        private const string PARM_EDI_RECEIVER_SETUP_ID = "@EDIReceiverSetupId";
        private const string PARM_RECEIVER_ID = "@ReceiverId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_RECEIVER_CODE = "@ReceiverCode";
        private const string PARM_RECEIVER_TYPE_CODE = "@ReceiverTypecode";
        private const string PARM_SUBMITTER_ID = "@SubmitterID";
        private const string PARM_ENTITY_ID = "@EnitityId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_BATCH_TYPE = "@BatchType";
        private const string PARM_RECEIVER_NSF_NAME = "@ReceiverNSFName";
        private const string PARM_VERSION_CODE_NATIONAL = "@VersioncodeNational";
        private const string PARM_SOFTWARE_ISSUER_ID = "@SoftwareIssuerId";
        private const string PARM_ORG_SUB_ID = "@OrgSubId";
        private const string PARM_VENDOR_SOFT_VERSION = "@VendorSoftVersion";
        private const string PARM_VALIDATION_XML = "@ValidationXML";
        private const string PARM_2000A_PRV01_BILLING_PAY_TO_PROVIDER_TYPE = "@RS2000APRV01";
        private const string PARM_BATCH_BY_BILLING_NPI = "@BatchbyBillingNPI";
        private const string PARM_ANSI5010 = "@ANSI5010";
        private const string PARM_2010AAREF_G5SEND_SITE_ID = "@RS2010AAREFG5";
        private const string PARM_TEST_PRODUCTION_INDICATOR = "@TestProductionIndicator";
        private const string PARM_VERSION_CODE_LOCAL = "@VersionCodeLocal";
        private const string PARM_BHT02_TRANSACTION_SET_PURPOSE_CODE = "@BHT02Transaction";
        private const string PARM_VENDOR_APP_CATEGORY = "@VendorAppCategory";
        private const string PARM_VENDOR_SOFT_UPDATE = "@VendorSoftUpdate";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_PATH = "@Path";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID1 = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALEDIReceiverSetup"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALEDIReceiverSetup()
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
        private void CreateParameters(IDBManager dbManager, DSEDI ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(31);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EDI_RECEIVER_SETUP_ID, ds.EDIReceiverSetup.EDIReceiverSetupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EDI_RECEIVER_SETUP_ID, ds.EDIReceiverSetup.EDIReceiverSetupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_RECEIVER_ID, ds.EDIReceiverSetup.ReceiverIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.EDIReceiverSetup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_RECEIVER_CODE, ds.EDIReceiverSetup.ReceiverCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_RECEIVER_TYPE_CODE, ds.EDIReceiverSetup.ReceiverTypecodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SUBMITTER_ID, ds.EDIReceiverSetup.SubmitterIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ENTITY_ID, ds.EDIReceiverSetup.EnitityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_CLEARING_HOUSE_ID, ds.EDIReceiverSetup.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_BATCH_TYPE, ds.EDIReceiverSetup.BatchTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_RECEIVER_NSF_NAME, ds.EDIReceiverSetup.ReceiverNSFNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_VERSION_CODE_NATIONAL, ds.EDIReceiverSetup.VersioncodeNationalColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SOFTWARE_ISSUER_ID, ds.EDIReceiverSetup.SoftwareIssuerIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ORG_SUB_ID, ds.EDIReceiverSetup.OrgSubIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_VENDOR_SOFT_VERSION, ds.EDIReceiverSetup.VendorSoftVersionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_VALIDATION_XML, ds.EDIReceiverSetup.ValidationXMLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_2000A_PRV01_BILLING_PAY_TO_PROVIDER_TYPE, ds.EDIReceiverSetup.RS2000APRV01Column.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_BATCH_BY_BILLING_NPI, ds.EDIReceiverSetup.BatchbyBillingNPIColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_ANSI5010, ds.EDIReceiverSetup.ANSI5010Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(18, PARM_2010AAREF_G5SEND_SITE_ID, ds.EDIReceiverSetup.RS2010AAREFG5Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(19, PARM_TEST_PRODUCTION_INDICATOR, ds.EDIReceiverSetup.TestProductionIndicatorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_VERSION_CODE_LOCAL, ds.EDIReceiverSetup.VersionCodeLocalColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_BHT02_TRANSACTION_SET_PURPOSE_CODE, ds.EDIReceiverSetup.BHT02TransactionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_VENDOR_APP_CATEGORY, ds.EDIReceiverSetup.VendorAppCategoryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_VENDOR_SOFT_UPDATE, ds.EDIReceiverSetup.VendorSoftUpdateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_IS_PRIMARY, ds.EDIReceiverSetup.IsPrimaryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(25, PARM_PATH, ds.EDIReceiverSetup.PathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_IS_ACTIVE, ds.EDIReceiverSetup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARM_CREATED_BY, ds.EDIReceiverSetup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_CREATED_ON, ds.EDIReceiverSetup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_MODIFIED_BY, ds.EDIReceiverSetup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_MODIFIED_ON, ds.EDIReceiverSetup.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the edi receiver setup.
        /// </summary>
        /// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        /// <param name="ReceiverCode">The receiver code.</param>
        /// <param name="ShortName">The short name.</param>
        /// <returns></returns>
        public DSEDI LoadEDIReceiverSetup(long EDIReceiverSetupId, string SubmitterID, string ShortName, string ClearingHouseId, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (SubmitterID == "")
                    SubmitterID = null;

                if (ShortName == "")
                    ShortName = null;

                if (ClearingHouseId == "")
                    ClearingHouseId = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(10);

                if (EDIReceiverSetupId <= 0)
                    dbManager.AddParameters(0, PARM_EDI_RECEIVER_SETUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_RECEIVER_SETUP_ID, EDIReceiverSetupId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_SUBMITTER_ID, SubmitterID);
                dbManager.AddParameters(3, PARM_CLEARING_HOUSE_ID, ClearingHouseId);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(4, PARM_ENTITY_ID1, EntityId);
                    else
                        dbManager.AddParameters(4, PARM_ENTITY_ID1, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID1, EntityId);

                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.EDIReceiverSetup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_RECEIVER_SETUP_SELECT, ds, ds.EDIReceiverSetup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverSetup::LoadEDIReceiverSetup", PROC_EDI_RECEIVER_SETUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the edi receiver setup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateEDIReceiverSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIReceiverSetup.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EDI_RECEIVER_SETUP_UPDATE, ds, ds.EDIReceiverSetup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIReceiverSetup.Rows[0][ds.EDIReceiverSetup.EDIReceiverSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverSetup::UpdateEDIReceiverSetup", PROC_EDI_RECEIVER_SETUP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the edi receiver setup.
        /// </summary>
        /// <param name="EDIReceiverSetupIds">The edi receiver setup ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteEDIReceiverSetup(string EDIReceiverSetupIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSEDI ds = LoadEDIReceiverSetup(Convert.ToInt64(EDIReceiverSetupIds), null, null, null, null, null,1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIReceiverSetup;
                dbManager.AddParameters(0, PARM_EDI_RECEIVER_SETUP_ID, EDIReceiverSetupIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDI_RECEIVER_SETUP_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.EDIReceiverSetup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIReceiverSetup.Rows[0][ds.EDIReceiverSetup.EDIReceiverSetupIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverSetup::DeleteEDIReceiverSetup", PROC_EDI_RECEIVER_SETUP_DELETE, ex);
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
        /// Inserts the edi receiver setup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertEDIReceiverSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIReceiverSetup.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EDI_RECEIVER_SETUP_INSERT, ds, ds.EDIReceiverSetup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIReceiverSetup.Rows[0][ds.EDIReceiverSetup.EDIReceiverSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverSetup::InsertEDIReceiverSetup", PROC_EDI_RECEIVER_SETUP_INSERT, ex);
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
        /// Lookups the edi receiver setup.
        /// </summary>
        /// <returns></returns>
        public DSEDILookup LookupEDIReceiverSetup()
        {
            DSEDILookup ds = new DSEDILookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID1, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID1, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_RECEIVER_SETUP_LOOKUP, ds, ds.EDIReceiverSetup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverSetup::LookupEDIReceiverSetup", PROC_EDI_RECEIVER_SETUP_LOOKUP, ex);
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
