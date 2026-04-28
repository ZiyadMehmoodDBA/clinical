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
    public class DALSubmitterSetup
    {
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_SUBMITTER_SETUP_INSERT = "Provider.sp_SubmitterSetupInsert";
        private const string PROC_SUBMITTER_SETUP_UPDATE = "Provider.sp_SubmitterSetupUpdate";
        private const string PROC_SUBMITTER_SETUP_DELETE = "Provider.sp_SubmitterSetupDelete";
        private const string PROC_SUBMITTER_SETUP_SELECT = "Provider.sp_SubmitterSetupSelect";
        private const string PROC_SUBMITTER_SETUP_LOOKUP = "Provider.sp_SubmitterSetupLookup";
        #endregion

        #region "Parameters"
        private const string PARM_SUBMITTER_SETUP_ID = "@SubmitterSetupId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ORGANIZATION_LAST_NAME = "@SS1000ANM103";
        private const string PARM_SUBMITTER_ADDRESS1 = "@AA07";
        private const string PARM_SUBMITTER_ADDRESS2 = "@AA08";
        private const string PARM_CITY = "@AA09";
        private const string PARM_STATE = "@AA010";
        private const string PARM_ZIPCODE = "@AA011";
        private const string PARM_REGION = "@AA012";
        private const string PARM_CONTACT_NAME = "@SS1000APer02";
        private const string PARM_CONTACT_TELEPHONE = "@SS1000APer04";
        private const string PARM_ENTITY_IDENTIFIER = "@SS1000ANM101";
        private const string PARM_ENTITY_TYPE_QUALIFIER = "@SS1000ANM102";
        private const string PARM_SUBMITTER_FIRST_NAME = "@SS1000ANM104";
        private const string PARM_SUBMITTER_MIDDLE_NAME = "@SS1000ANM105";
        private const string PARM_IDENTIFICATTION_CODE_QUALIFIER = "@SS1000ANM109";
        private const string PARM_IS_ACTIVE = "@isActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSubmitterSetup"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALSubmitterSetup()
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
            dbManager.CreateParameters(23);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SUBMITTER_SETUP_ID, ds.SubmitterSetup.SubmitterSetupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SUBMITTER_SETUP_ID, ds.SubmitterSetup.SubmitterSetupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_ENTITY_ID, ds.SubmitterSetup.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.SubmitterSetup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ORGANIZATION_LAST_NAME, ds.SubmitterSetup.SS1000ANM103Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SUBMITTER_ADDRESS1, ds.SubmitterSetup.AA07Column.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SUBMITTER_ADDRESS2, ds.SubmitterSetup.AA08Column.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CITY, ds.SubmitterSetup.AA09Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_STATE, ds.SubmitterSetup.AA010Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ZIPCODE, ds.SubmitterSetup.AA011Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_REGION, ds.SubmitterSetup.AA012Column.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CONTACT_NAME, ds.SubmitterSetup.SS1000APer02Column.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CONTACT_TELEPHONE, ds.SubmitterSetup.SS1000APer04Column.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ENTITY_IDENTIFIER, ds.SubmitterSetup.SS1000ANM101Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ENTITY_TYPE_QUALIFIER, ds.SubmitterSetup.SS1000ANM102Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_SUBMITTER_FIRST_NAME, ds.SubmitterSetup.SS1000ANM104Column.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_SUBMITTER_MIDDLE_NAME, ds.SubmitterSetup.SS1000ANM105Column.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_IDENTIFICATTION_CODE_QUALIFIER, ds.SubmitterSetup.SS1000ANM109Column.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_IS_ACTIVE, ds.SubmitterSetup.isActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(18, PARM_CREATED_BY, ds.SubmitterSetup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_CREATED_ON, ds.SubmitterSetup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_MODIFIED_BY, ds.SubmitterSetup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_MODIFIED_ON, ds.SubmitterSetup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the submitter setup.
        /// </summary>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <param name="OrganizationLastName">Last name of the organization.</param>
        /// <param name="SubmitterAddress1">The submitter address1.</param>
        /// <param name="SubmitterAddress2">The submitter address2.</param>
        /// <returns></returns>
        public DSEDI LoadSubmitterSetup(long SubmitterSetupId, string OrganizationLastName, string ShortName, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (OrganizationLastName == "")
                    OrganizationLastName = null;

                if (ShortName == "")
                    ShortName = null;

                if (IsActive == "")
                    IsActive = null;

                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (SubmitterSetupId == 0)
                    dbManager.AddParameters(0, PARM_SUBMITTER_SETUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SUBMITTER_SETUP_ID, SubmitterSetupId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_ORGANIZATION_LAST_NAME, OrganizationLastName);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.SubmitterSetup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUBMITTER_SETUP_SELECT, ds, ds.SubmitterSetup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSubmitterSetup::LoadSubmitterSetup", PROC_SUBMITTER_SETUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the submitter setup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateSubmitterSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SubmitterSetup.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUBMITTER_SETUP_UPDATE, ds, ds.SubmitterSetup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SubmitterSetup.Rows[0][ds.SubmitterSetup.SubmitterSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSubmitterSetup::UpdateSubmitterSetup", PROC_SUBMITTER_SETUP_UPDATE, ex);
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
        /// Deletes the submitter setup.
        /// </summary>
        /// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteSubmitterSetup(string SubmitterSetupId)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSEDI ds = LoadSubmitterSetup(Convert.ToInt64(SubmitterSetupId), null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SubmitterSetup;
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SUBMITTER_SETUP_ID, SubmitterSetupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SUBMITTER_SETUP_DELETE).ToString();
                if (returnValue != null && returnValue != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.SubmitterSetup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SubmitterSetup.Rows[0][ds.SubmitterSetup.SubmitterSetupIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSubmitterSetup::DeleteSubmitterSetup", PROC_SUBMITTER_SETUP_DELETE, ex);
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
        /// Inserts the submitter setup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertSubmitterSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SubmitterSetup.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUBMITTER_SETUP_INSERT, ds, ds.SubmitterSetup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SubmitterSetup.Rows[0][ds.SubmitterSetup.SubmitterSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSubmitterSetup::InsertSubmitterSetup", PROC_SUBMITTER_SETUP_INSERT, ex);
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

        #region Lookups
        /// <summary>
        /// Lookups the submitter setup.
        /// </summary>
        /// <returns></returns>
        public DSEDILookup LookupSubmitterSetup()
        {
            DSEDILookup ds = new DSEDILookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUBMITTER_SETUP_LOOKUP, ds, ds.SubmitterSetup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSubmitterSetup::LookupSubmitterSetup", PROC_SUBMITTER_SETUP_LOOKUP, ex);
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
