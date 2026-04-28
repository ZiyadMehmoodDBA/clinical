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
    public class DALFollowUpClaimStatusCategoryCode
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CSCATEGORYCODE_INSERT = "Billing.sp_ClaimStatusCategoryCodeInsert";
        private const string PROC_CSCATEGORYCODE_UPDATE = "Billing.sp_ClaimStatusCategoryCodeUpdate";
        private const string PROC_CSCATEGORYCODE_DELETE = "Billing.sp_ClaimStatusCategoryCodeDelete";
        private const string PROC_CSCATEGORYCODE_SELECT = "Billing.sp_ClaimStatusCategoryCodeSelect";
        private const string PROC_CSCATEGORYCODE_LOOKUP = "Billing.sp_ClaimStatusCategoryCodeLookup";
        #endregion

        #region "Parameters"

        private const string PARM_CSCATEGORYCODE_ID = "@CSCatCodeId";
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
        public DALFollowUpClaimStatusCategoryCode()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALFollowUpClaimStatusCategoryCode(SharedVariable sharedVariable)
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
                dbManager.AddParameters(0, PARM_CSCATEGORYCODE_ID, ds.ClaimStatusCategoryCode.CSCatCodeIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CSCATEGORYCODE_ID, ds.ClaimStatusCategoryCode.CSCatCodeIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_CODE, ds.ClaimStatusCategoryCode.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ClaimStatusCategoryCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ClaimStatusCategoryCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ClaimStatusCategoryCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ClaimStatusCategoryCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ClaimStatusCategoryCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ClaimStatusCategoryCode.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadClaimStatusCategoryCode(int CSCatCodeId, string Code, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
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
                if (CSCatCodeId <= 0)
                    dbManager.AddParameters(0, PARM_CSCATEGORYCODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CSCATEGORYCODE_ID, CSCatCodeId);

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
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ClaimStatusCategoryCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CSCATEGORYCODE_SELECT, ds, ds.ClaimStatusCategoryCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCategoryCode::LoadClaimStatusCategoryCode", PROC_CSCATEGORYCODE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp InsertClaimStatusCategoryCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCategoryCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CSCATEGORYCODE_INSERT, ds, ds.ClaimStatusCategoryCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCategoryCode.Rows[0][ds.ClaimStatusCategoryCode.CSCatCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCategoryCode::InsertClaimStatusCategoryCode", PROC_CSCATEGORYCODE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateClaimStatusCategoryCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCategoryCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CSCATEGORYCODE_UPDATE, ds, ds.ClaimStatusCategoryCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCategoryCode.Rows[0][ds.ClaimStatusCategoryCode.CSCatCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCategoryCode::UpdateClaimStatusCategoryCode", PROC_CSCATEGORYCODE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteClaimStatusCategoryCode(string CSCatCodeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadClaimStatusCategoryCode(Convert.ToInt16(CSCatCodeId), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ClaimStatusCategoryCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CSCATEGORYCODE_ID, CSCatCodeId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CSCATEGORYCODE_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ClaimStatusCategoryCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ClaimStatusCategoryCode.Rows[0][ds.ClaimStatusCategoryCode.CSCatCodeIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCategoryCode::DeleteClaimStatusCategoryCode", PROC_CSCATEGORYCODE_DELETE, ex);
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
        public DSFollowUp LookupClaimStatusCategoryCode()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CSCATEGORYCODE_LOOKUP, ds, ds.ClaimStatusCategoryCodeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpClaimStatusCategoryCode::LookupClaimStatusCategoryCode", PROC_CSCATEGORYCODE_LOOKUP, ex);
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
