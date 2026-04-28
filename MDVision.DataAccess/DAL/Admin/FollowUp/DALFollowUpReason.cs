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
    public class DALFollowUpReason
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_REASON_INSERT = "Billing.sp_FollowupReasonsInsert";
        private const string PROC_FOLLOW_UP_REASON_UPDATE = "Billing.sp_FollowupReasonsUpdate";
        private const string PROC_FOLLOW_UP_REASON_DELETE = "Billing.sp_FollowupReasonsDelete";
        private const string PROC_FOLLOW_UP_REASON_SELECT = "Billing.sp_FollowupReasonsSelect";
        private const string PROC_FOLLOW_UP_AR_TYPE_LOOKUP = "[Billing].[sp_ARTypeLookup]";

        private const string PROC_FOLLOW_UP_REASON_LOOKUP = "Billing.sp_FollowupReasonsLookup";
        #endregion

        #region "Parameters"

        private const string PARM_REASON_ID = "@ReasonId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ARTYPE_ID = "@ARTypeId";
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
        public DALFollowUpReason()
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
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REASON_ID, ds.FollowupReasons.ReasonIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REASON_ID, ds.FollowupReasons.ReasonIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.FollowupReasons.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.FollowupReasons.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ARTYPE_ID, ds.FollowupReasons.ARTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.FollowupReasons.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.FollowupReasons.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.FollowupReasons.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.FollowupReasons.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.FollowupReasons.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Load the followup reasons
        /// </summary>
        /// <param name="ReasonId"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="ARTypeID"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSFollowUp LoadFollowUpReasons(long ReasonId, string ShortName, string Description, string ARTypeID, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (ARTypeID == "")
                    ARTypeID = null;
                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (ReasonId <= 0)
                    dbManager.AddParameters(0, PARM_REASON_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REASON_ID, ReasonId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_ARTYPE_ID, ARTypeID);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.FollowupReasons.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_REASON_SELECT, ds, ds.FollowupReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::LoadFollowUpReasons", PROC_FOLLOW_UP_REASON_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Insert the Follow Up Reason
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFollowUp InsertFollowupReason(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupReasons.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_REASON_INSERT, ds, ds.FollowupReasons.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupReasons.Rows[0][ds.FollowupReasons.ReasonIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::InsertFollowupReason", PROC_FOLLOW_UP_REASON_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Update the FollowUp Reason
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFollowUp UpdateFollowUpReason(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupReasons.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_REASON_UPDATE, ds, ds.FollowupReasons.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupReasons.Rows[0][ds.FollowupReasons.ReasonIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::UpdateFollowUpReason", PROC_FOLLOW_UP_REASON_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Delete the FollowUp Reason's
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public string DeleteFollowUpReason(string reasonId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadFollowUpReasons(Convert.ToInt64(reasonId), null, null, null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupReasons;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REASON_ID, reasonId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_REASON_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.FollowupReasons.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupReasons.Rows[0][ds.FollowupReasons.ReasonIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::DeleteFollowUpReason", PROC_FOLLOW_UP_REASON_DELETE, ex);
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

        public DSFollowUp LookupActionReasonType()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_TYPE_LOOKUP, ds, ds.FollowupActionReasonTypeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::LookupActionReasonType", PROC_FOLLOW_UP_AR_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFollowUp LookupFollowUpReasons()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_REASON_LOOKUP, ds, ds.FollowupReasonsLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::LookupFollowUpReasons", PROC_FOLLOW_UP_REASON_LOOKUP, ex);
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
