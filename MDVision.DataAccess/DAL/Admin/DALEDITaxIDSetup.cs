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
    public class DALEDITaxIDSetup
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_EDI_TAX_ID_SETUP_INSERT = "Provider.sp_EDITaxIDSetupInsert";
        private const string PROC_EDI_TAX_ID_SETUP_UPDATE = "Provider.sp_EDITaxIDSetupUpdate";
        private const string PROC_EDI_TAX_ID_SETUP_DELETE = "Provider.sp_EDITaxIDSetupDelete";
        private const string PROC_EDI_TAX_ID_SETUP_SELECT = "Provider.sp_EDITaxIDSetupSelect";
        #endregion

        #region "Parameters"
        private const string PARM_EDI_TAX_ID_SETUP_ID = "@EDITaxIDSetupId";
        private const string PARM_TAX_ID = "@TaxID";
        private const string PARM_CLEARING_HOUSER_ID = "@ClearinghouseId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SUBMITTER_SETUP_ID = "@SubmitterSetupId";
        private const string PARM_EDI_RECEIVER_ID = "@ReceiverId";
        private const string PARM_IS_ACTIVE = "@IsActive";
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
        /// Initializes a new instance of the <see cref="DALEDITaxIDSetup"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALEDITaxIDSetup()
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
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EDI_TAX_ID_SETUP_ID, ds.EDITaxIDSetup.EDITaxIDSetupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EDI_TAX_ID_SETUP_ID, ds.EDITaxIDSetup.EDITaxIDSetupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_TAX_ID, ds.EDITaxIDSetup.TaxIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_CLEARING_HOUSER_ID, ds.EDITaxIDSetup.ClearinghouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ENTITY_ID, ds.EDITaxIDSetup.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_EDI_RECEIVER_ID, ds.EDITaxIDSetup.ReceiverIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_SUBMITTER_SETUP_ID, ds.EDITaxIDSetup.SubmitterSetupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.EDITaxIDSetup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.EDITaxIDSetup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.EDITaxIDSetup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.EDITaxIDSetup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.EDITaxIDSetup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the edi tax identifier setup.
        /// </summary>
        /// <param name="EDITaxIDSetupId">The edi tax identifier setup identifier.</param>
        /// <param name="TaxID">The tax identifier.</param>
        /// <param name="Clearinghouse">The clearinghouse.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public DSEDI LoadEDITaxIDSetup(long EDITaxIDSetupId, string TaxID, string Clearinghouse, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (TaxID == "")
                    TaxID = null;

                if (Clearinghouse == "")
                    Clearinghouse = null;

                if (EntityId == "")
                    EntityId = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (EDITaxIDSetupId == 0)
                    dbManager.AddParameters(0, PARM_EDI_TAX_ID_SETUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_TAX_ID_SETUP_ID, EDITaxIDSetupId);
                dbManager.AddParameters(1, PARM_TAX_ID, TaxID);
                dbManager.AddParameters(2, PARM_CLEARING_HOUSER_ID, Clearinghouse);
               // dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);

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
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.EDITaxIDSetup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               

                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_TAX_ID_SETUP_SELECT, ds, ds.EDITaxIDSetup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDITaxIDSetup::LoadEDITaxIDSetup", PROC_EDI_TAX_ID_SETUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the EDITaxIDSetup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateEDITaxIDSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDITaxIDSetup.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EDI_TAX_ID_SETUP_UPDATE, ds, ds.EDITaxIDSetup.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDITaxIDSetup.Rows[0][ds.EDITaxIDSetup.EDITaxIDSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDITaxIDSetup::UpdateEDITaxIDSetup", PROC_EDI_TAX_ID_SETUP_UPDATE, ex);
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
        /// Deletes the edi tax identifier setup.
        /// </summary>
        /// <param name="EDITaxIDSetupId">The edi tax identifier setup identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteEDITaxIDSetup(string EDITaxIDSetupId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSEDI ds = LoadEDITaxIDSetup(Convert.ToInt64(EDITaxIDSetupId), null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDITaxIDSetup;
                dbManager.AddParameters(0, PARM_EDI_TAX_ID_SETUP_ID, EDITaxIDSetupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDI_TAX_ID_SETUP_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.EDITaxIDSetup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDITaxIDSetup.Rows[0][ds.EDITaxIDSetup.EDITaxIDSetupIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDITaxIDSetup::DeleteEDITaxIDSetup", PROC_EDI_TAX_ID_SETUP_DELETE, ex);
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
        /// Inserts the edi tax identifier setup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertEDITaxIDSetup(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDITaxIDSetup.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EDI_TAX_ID_SETUP_INSERT, ds, ds.EDITaxIDSetup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDITaxIDSetup.Rows[0][ds.EDITaxIDSetup.EDITaxIDSetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDITaxIDSetup::InsertEDITaxIDSetup", PROC_EDI_TAX_ID_SETUP_INSERT, ex);
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


