using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin.FollowUp
{
    public class DALFollowUpClaimStatusCode
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CSCODE_INSERT = "Billing.sp_ClaimStatusCodeInsert";
        private const string PROC_CSCODE_UPDATE = "Billing.sp_ClaimStatusCodeUpdate";
        private const string PROC_CSCODE_DELETE = "Billing.sp_ClaimStatusCodeDelete";
        private const string PROC_CSCODE_SELECT = "Billing.sp_ClaimStatusCodeSelect";
        private const string PROC_CSCODE_LOOKUP = "Billing.sp_ClaimStatusCodeLookup";
        #endregion

        #region "Parameters"

        private const string PARM_CSCODE_ID = "@CSCodeId";
        private const string PARM_CODE = "@Code";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        public DALFollowUpClaimStatusCode()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        public DALFollowUpClaimStatusCode(SharedVariable sharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(sharedVariable);

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
        /// Create The Parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CSCODE_ID, ds.ClaimStatusCode.CSCodeIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CSCODE_ID, ds.ClaimStatusCode.CSCodeIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_CODE, ds.ClaimStatusCode.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ClaimStatusCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ClaimStatusCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ClaimStatusCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ClaimStatusCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ClaimStatusCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ClaimStatusCode.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadClaimStatusCode(int CSCodeId, string Code, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Code == "")
                    Code = null;
                if (Description == "")
                    Description = null;
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(7);
                if (CSCodeId <= 0)
                    dbManager.AddParameters(0, PARM_CSCODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CSCODE_ID, CSCodeId);

                dbManager.AddParameters(1, PARM_CODE, Code);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ClaimStatusCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CSCODE_SELECT, ds, ds.ClaimStatusCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCode::LoadClaimStatusCode", PROC_CSCODE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp InsertClaimStatusCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CSCODE_INSERT, ds, ds.ClaimStatusCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCode.Rows[0][ds.ClaimStatusCode.CSCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCode::InsertClaimStatusCode", PROC_CSCODE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateClaimStatusCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CSCODE_UPDATE, ds, ds.ClaimStatusCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCode.Rows[0][ds.ClaimStatusCode.CSCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCode::UpdateClaimStatusCode", PROC_CSCODE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteClaimStatusCode(string CSCodeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadClaimStatusCode(Convert.ToInt16(CSCodeId), null, null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CSCODE_ID, CSCodeId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CSCODE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ClaimStatusCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCode.Rows[0][ds.ClaimStatusCode.CSCodeIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCode::DeleteClaimStatusCode", PROC_CSCODE_DELETE, ex);
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

        #endregion

        #region "Lookups"
        public DSFollowUp LookupClaimStatusCode()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CSCODE_LOOKUP, ds, ds.ClaimStatusCodeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCode::LookupClaimStatusCode", PROC_CSCODE_LOOKUP, ex);
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
