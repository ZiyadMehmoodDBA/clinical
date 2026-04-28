using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
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
    public class DALFollowUpCodesMapping
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CODESMAPPING_INSERT = "Billing.sp_CodesMappingInsert";
        private const string PROC_CODESMAPPING_UPDATE = "Billing.sp_CodesMappingUpdate";
        private const string PROC_CODESMAPPING_DELETE = "Billing.sp_CodesMappingDelete";
        private const string PROC_CODESMAPPING_SELECT = "Billing.sp_CodesMappingSelect";
        #endregion

        #region "Parameters"

        private const string PARM_CODESMAPPING_ID = "@CodesMappingId";
        private const string PARM_CSCODE_ID = "@ClaimStatusCodeId";
        private const string PARM_CSCATEGORYCODE_ID = "@ClaimStatusCategoryCodeId";
        private const string PARM_ACTION_ID = "@ActionId";
        private const string PARM_REASON_ID = "@ReasonId";
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
        public DALFollowUpCodesMapping()
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
        /// Create The Parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CODESMAPPING_ID, ds.CodesMapping.CodesMappingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CODESMAPPING_ID, ds.CodesMapping.CodesMappingIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_CSCODE_ID, ds.CodesMapping.ClaimStatusCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_CSCATEGORYCODE_ID, ds.CodesMapping.ClaimStatusCategoryCodeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_ACTION_ID, ds.CodesMapping.ActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_REASON_ID, ds.CodesMapping.ReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.CodesMapping.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.CodesMapping.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.CodesMapping.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.CodesMapping.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.CodesMapping.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.CodesMapping.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadCodesMapping(long CodesMappingId, int ClaimStatusCodeId, int ClaimStatusCategoryCodeId, int ActionId, int ReasonId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (CodesMappingId == 0)
                    dbManager.AddParameters(0, PARM_CODESMAPPING_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CODESMAPPING_ID, CodesMappingId);

                if (ClaimStatusCodeId == 0)
                    dbManager.AddParameters(1, PARM_CSCODE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CSCODE_ID, ClaimStatusCodeId);

                if (ClaimStatusCategoryCodeId == 0)
                    dbManager.AddParameters(2, PARM_CSCATEGORYCODE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_CSCATEGORYCODE_ID, ClaimStatusCategoryCodeId);

                if (ActionId == 0)
                    dbManager.AddParameters(3, PARM_ACTION_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ACTION_ID, ActionId);

                if (ReasonId == 0)
                    dbManager.AddParameters(4, PARM_REASON_ID, null);
                else
                    dbManager.AddParameters(4, PARM_REASON_ID, ReasonId);

                //dbManager.AddParameters(5, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.CodesMapping.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CODESMAPPING_SELECT, ds, ds.CodesMapping.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpCodesMapping::LoadCodesMapping", PROC_CODESMAPPING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp InsertCodesMapping(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CodesMapping.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CODESMAPPING_INSERT, ds, ds.CodesMapping.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CodesMapping.Rows[0][ds.CodesMapping.CodesMappingIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpCodesMapping::InsertCodesMapping", PROC_CODESMAPPING_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateCodesMapping(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CodesMapping.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CODESMAPPING_UPDATE, ds, ds.CodesMapping.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CodesMapping.Rows[0][ds.CodesMapping.CodesMappingIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpCodesMapping::UpdateCodesMapping", PROC_CODESMAPPING_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteCodesMapping(string CodesMappingId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadCodesMapping(Convert.ToInt64(CodesMappingId), 0, 0, 0, 0, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CodesMapping;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CODESMAPPING_ID, CodesMappingId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CODESMAPPING_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.CodesMapping.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CodesMapping.Rows[0][ds.CodesMapping.CodesMappingIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpCodesMapping::DeleteCodesMapping", PROC_CODESMAPPING_DELETE, ex);
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

    }
}
