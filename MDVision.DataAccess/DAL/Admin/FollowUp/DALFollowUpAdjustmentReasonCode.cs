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
    public class DALFollowUpAdjustmentReasonCode 
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_ADRCODE_INSERT = "Billing.sp_AdjustmentReasonCodeInsert";
        private const string PROC_ADRCODE_UPDATE = "Billing.sp_AdjustmentReasonCodeUpdate";
        private const string PROC_ADRCODE_DELETE = "Billing.sp_AdjustmentReasonCodeDelete";
        private const string PROC_ADRCODE_SELECT = "Billing.sp_AdjustmentReasonCodeSelect";
        private const string PROC_ADRCODE_LOOKUP = "Billing.sp_AdjustmentReasonCodeLookup";
        #endregion

        #region "Parameters"

        private const string PARM_ADJUSTMENT_ID = "@AdjustmentId";
        private const string PARM_CODE = "@Code";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors
        public DALFollowUpAdjustmentReasonCode()
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
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ADJUSTMENT_ID, ds.AdjustmentReasonCode.AdjustmentIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ADJUSTMENT_ID, ds.AdjustmentReasonCode.AdjustmentIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_CODE, ds.AdjustmentReasonCode.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.AdjustmentReasonCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.AdjustmentReasonCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.AdjustmentReasonCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.AdjustmentReasonCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.AdjustmentReasonCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.AdjustmentReasonCode.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadAdjustmentReasonCode(int AdjsutmentId, string Code, string Description, string Active, int PageNumber, int RowsPerPage)
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
                if (AdjsutmentId <= 0)
                    dbManager.AddParameters(0, PARM_ADJUSTMENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ADJUSTMENT_ID, AdjsutmentId);
                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.AdjustmentReasonCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(4, PARM_CODE, Code);
                dbManager.AddParameters(5, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, Active);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ADRCODE_SELECT, ds, ds.AdjustmentReasonCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAdjustmentReasonCode::LoadAdjustmentReasonCode", PROC_ADRCODE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp InsertAdjustmentReasonCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AdjustmentReasonCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ADRCODE_INSERT, ds, ds.AdjustmentReasonCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AdjustmentReasonCode.Rows[0][ds.AdjustmentReasonCode.AdjustmentIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAdjustmentReasonCode::InsertAdjustmentReasonCode", PROC_ADRCODE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateAdjustmentReasonCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AdjustmentReasonCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ADRCODE_UPDATE, ds, ds.AdjustmentReasonCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AdjustmentReasonCode.Rows[0][ds.AdjustmentReasonCode.AdjustmentIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAdjustmentReasonCode::UpdateAdjustmentReasonCode", PROC_ADRCODE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteAdjustmentReasonCode(string AdjustmentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadAdjustmentReasonCode(Convert.ToInt16(AdjustmentId), null, null,null,1,15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.AdjustmentReasonCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ADJUSTMENT_ID, AdjustmentId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ADRCODE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.AdjustmentReasonCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.AdjustmentReasonCode.Rows[0][ds.AdjustmentReasonCode.AdjustmentIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAdjustmentReasonCode::DeleteAdjustmentReasonCode", PROC_ADRCODE_DELETE, ex);
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
        public DSFollowUp LookupAdjustmentReasonCode()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ADRCODE_LOOKUP, ds, ds.AdjustmentReasonCodeLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAdjustmentReasonCode::LookupAdjustmentReasonCode", PROC_ADRCODE_LOOKUP, ex);
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
