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
    public class DALEDIClaimStatusInsurance
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_EDI_CLAIM_STATUS_INSURANCE_INSERT = "Provider.sp_EDIClaimStatusInsuranceInsert";
        private const string PROC_EDI_CLAIM_STATUS_INSURANCE_UPDATE = "Provider.sp_EDIClaimStatusInsuranceUpdate";
        private const string PROC_EDI_CLAIM_STATUS_INSURANCE_DELETE = "Provider.sp_EDIClaimStatusInsuranceDelete";
        private const string PROC_EDI_CLAIM_STATUS_INSURANCE_SELECT = "Provider.sp_EDIClaimStatusInsuranceSelect";
        private const string PROC_EDI_CLAIM_STATUS_INSURANCE_LOOKUP = "Provider.sp_EDIClaimStatusInsuranceLookup";
        #endregion

        #region "Parameters"
        private const string PARM_EDI_CLAIM_STATUS_ID = "@EDIClaimStatusID";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_EDI_STATUS_INSURANCE = "@EDIStatusInsurance";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_PAYOR_ID = "@PayorId";
        private const string PARM_TAX_ID = "@TaxId";
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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALEDIClaimStatusInsurance"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALEDIClaimStatusInsurance()
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
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EDI_CLAIM_STATUS_ID, ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EDI_CLAIM_STATUS_ID, ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, ds.EDIClaimStatusInsurance.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_EDI_STATUS_INSURANCE, ds.EDIClaimStatusInsurance.EDIStatusInsuranceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PHONE_NO, ds.EDIClaimStatusInsurance.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PHONE_EXT, ds.EDIClaimStatusInsurance.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_PAYOR_ID, ds.EDIClaimStatusInsurance.PayorIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_TAX_ID, ds.EDIClaimStatusInsurance.TaxIdColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.EDIClaimStatusInsurance.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.EDIClaimStatusInsurance.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.EDIClaimStatusInsurance.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.EDIClaimStatusInsurance.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.EDIClaimStatusInsurance.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the edi claim status insurance.
        /// </summary>
        /// <param name="EDIStatusId">The edi status identifier.</param>
        /// <param name="CHouseId">The c house identifier.</param>
        /// <param name="EDIStatusInsName">Name of the edi status ins.</param>
        /// <param name="PayorId">The payor identifier.</param>
        /// <returns></returns>
        public DSEDI LoadEDIClaimStatusInsurance(long EDIStatusId, string CHouseId, string EDIStatusInsName, string PayorId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (CHouseId == "")
                    CHouseId = null;

                if (EDIStatusInsName == "")
                    EDIStatusInsName = null;

                if (PayorId == "")
                    PayorId = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (EDIStatusId <= 0)
                    dbManager.AddParameters(0, PARM_EDI_CLAIM_STATUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_CLAIM_STATUS_ID, EDIStatusId);
                dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, CHouseId);
                dbManager.AddParameters(2, PARM_EDI_STATUS_INSURANCE, EDIStatusInsName);
                dbManager.AddParameters(3, PARM_PAYOR_ID, PayorId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.EDIClaimStatusInsurance.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_CLAIM_STATUS_INSURANCE_SELECT, ds, ds.EDIClaimStatusInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIClaimStatusInsurance::LoadEDIClaimStatusInsurance", PROC_EDI_CLAIM_STATUS_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the edi claim status insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateEDIClaimStatusInsurance(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIClaimStatusInsurance.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_EDI_CLAIM_STATUS_INSURANCE_UPDATE, ds, ds.EDIClaimStatusInsurance.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIClaimStatusInsurance.Rows[0][ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIClaimStatusInsurance::UpdateEDIClaimStatusInsurance", PROC_EDI_CLAIM_STATUS_INSURANCE_UPDATE, ex);
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
        /// Deletes the edi claim status insurance.
        /// </summary>
        /// <param name="EDIStatusId">The edi status identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteEDIClaimStatusInsurance(string EDIStatusId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSEDI ds = LoadEDIClaimStatusInsurance(Convert.ToInt64(EDIStatusId), null, null, null, null,1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIClaimStatusInsurance;
                dbManager.AddParameters(0, PARM_EDI_CLAIM_STATUS_ID, EDIStatusId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_EDI_CLAIM_STATUS_INSURANCE_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.EDIClaimStatusInsurance.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIClaimStatusInsurance.Rows[0][ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIClaimStatusInsurance::DeleteEDIClaimStatusInsurance", PROC_EDI_CLAIM_STATUS_INSURANCE_DELETE, ex);
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
        /// Inserts the edi claim status insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertEDIClaimStatusInsurance(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIClaimStatusInsurance.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_EDI_CLAIM_STATUS_INSURANCE_INSERT, ds, ds.EDIClaimStatusInsurance.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIClaimStatusInsurance.Rows[0][ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIClaimStatusInsurance::InsertEDIClaimStatusInsurance", PROC_EDI_CLAIM_STATUS_INSURANCE_INSERT, ex);
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
        /// Lookups the edi claim status insurance.
        /// </summary>
        /// <returns></returns>
        public DSEDILookup LookupEDIClaimStatusInsurance()
        {
            DSEDILookup ds = new DSEDILookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSEDILookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_EDI_CLAIM_STATUS_INSURANCE_LOOKUP, ds, ds.EDIClaimStatusInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIClaimStatusInsurance::LookupEDIClaimStatusInsurance", PROC_EDI_CLAIM_STATUS_INSURANCE_LOOKUP, ex);
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
