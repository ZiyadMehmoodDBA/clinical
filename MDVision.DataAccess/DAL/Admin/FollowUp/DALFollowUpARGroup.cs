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
    public class DALFollowUpARGroup
    {
        #region Variable
        
        #endregion
        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_GROUP_INSERT = "Billing.sp_FollowupARGroupInsert";
        private const string PROC_FOLLOW_UP_GROUP_UPDATE = "Billing.sp_FollowupARGroupUpdate";
        private const string PROC_FOLLOW_UP_GROUP_DELETE = "Billing.sp_FollowupARGroupDelete";
        private const string PROC_FOLLOW_UP_GROUP_SELECT = "Billing.sp_FollowupARGroupSelect";
        private const string PROC_FOLLOW_AUTO_GROUP_LOOKUP = "Billing.sp_FollowupARGroupLookUp";

        #endregion
        #region "Parameters"

        private const string PARM_GROUP_ID = "@ARGroupId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE_ = "@RowspPage";
        

        #endregion
        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_GROUP_ID, ds.FollowupARGroup.ARGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_GROUP_ID, ds.FollowupARGroup.ARGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.FollowupARGroup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.FollowupARGroup.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.FollowupARGroup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.FollowupARGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.FollowupARGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.FollowupARGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.FollowupARGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion
        #region Constructors
        public DALFollowUpARGroup()
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
        #region "Lookups"

        public DSFollowUp LookupARGroup()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_AUTO_GROUP_LOOKUP, ds, ds.FollowupARGroupLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARGroup::LookupARGroup", PROC_FOLLOW_AUTO_GROUP_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp InsertARGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_GROUP_INSERT, ds, ds.FollowupARGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARGroup::InsertARGroup", PROC_FOLLOW_UP_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateARGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_GROUP_UPDATE, ds, ds.FollowupARGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARGroup::UpdateARGroup", PROC_FOLLOW_UP_GROUP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp LoadARGroup(Int64 ARGroupId, string shortName, string Description ,string isActive, int pageNumber, int RecordPerPage)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (shortName == "")
                    shortName = null;
                if (Description == "")
                    Description = null;
                if (isActive == "")
                    isActive = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ARGroupId == 0)
                    dbManager.AddParameters(0, PARM_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_GROUP_ID, ARGroupId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, shortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(5, PARM_ROWS_PAGE_, RecordPerPage);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.FollowupARGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_GROUP_SELECT, ds, ds.FollowupARGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARGroup::LoadARGroup", PROC_FOLLOW_UP_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteARGroup(Int64 ARGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_GROUP_ID, ARGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_GROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARGroup::DeleteARGroup", PROC_FOLLOW_UP_GROUP_DELETE, ex);
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
