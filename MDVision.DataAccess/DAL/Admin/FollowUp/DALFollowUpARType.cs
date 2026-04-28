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
    public class DALFollowUpARType
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_AR_TYPE_INSERT = "Billing.sp_ARTypeInsert";
        private const string PROC_FOLLOW_UP_AR_TYPE_UPDATE = "Billing.sp_ARTypeUpdate";
        private const string PROC_FOLLOW_UP_AR_TYPE_DELETE = "Billing.sp_ARTypeDelete";
        private const string PROC_FOLLOW_UP_AR_TYPE_SELECT = "Billing.sp_ARTypeSelect";
        //private const string PROC_FOLLOW_UP_AR_TYPE_LOOKUP = "Billing.sp_ActionReasonTypeLookup";

        #endregion

        #region "Parameters"

        private const string PARM_ARTYPE_ID = "@ARTypeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        public DALFollowUpARType()
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
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ARTYPE_ID, ds.ARType.ARTypeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ARTYPE_ID, ds.ARType.ARTypeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.ARType.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ARType.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ARType.IsActiveColumn.ColumnName, DbType.Byte);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Load the followup ARType
        /// </summary>
        /// <param name="ReasonId"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="ARTypeID"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DSFollowUp LoadFollowUpARType(long ARTypeId, string ShortName, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(7);
                if (ARTypeId <= 0)
                    dbManager.AddParameters(0, PARM_ARTYPE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ARTYPE_ID, ARTypeId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ARType.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_TYPE_SELECT, ds, ds.ARType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARType::LoadARType", PROC_FOLLOW_UP_AR_TYPE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Insert the Follow Up ARType
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFollowUp InsertFollowUpARType(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ARType.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_TYPE_INSERT, ds, ds.ARType.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ARType.Rows[0][ds.ARType.ARTypeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARType::InsertFollowUpARType", PROC_FOLLOW_UP_AR_TYPE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Update the FollowUp ARType
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFollowUp UpdateFollowUpARType(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ARType.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_TYPE_UPDATE, ds, ds.ARType.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ARType.Rows[0][ds.ARType.ARTypeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARType::UpdateFollowUpARType", PROC_FOLLOW_UP_AR_TYPE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Delete the FollowUp ARType
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public string DeleteFollowUpARType(string reasonId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadFollowUpARType(Convert.ToInt64(reasonId), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ARType;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ARTYPE_ID, reasonId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_TYPE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ARType.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ARType.Rows[0][ds.ARType.ARTypeIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARType::DeleteFollowUpARType", PROC_FOLLOW_UP_AR_TYPE_DELETE, ex);
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
