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
   public class DALFollowUpARProviderGroup
    {
       #region Variable
        
        #endregion
        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_AR_PROVIDER_GROUP_INSERT = "Billing.sp_FollowupARProviderGroupInsert";
        private const string PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_UPDATE = "Billing.sp_FollowupARProviderGroupUpdate";
        private const string PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_DELETE = "Billing.sp_FollowupARProviderGroupDelete";
        private const string PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_SELECT = "Billing.sp_FollowupARProviderGroupSelect";
        
        #endregion
        #region "Parameters"

        private const string PARM_PROVIDER_GROUP_ID = "@ProviderGrpId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_AR_GROUP_ID = "@ARGroupId";
        
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
            dbManager.CreateParameters(3);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ds.FollowupARProviderGroup.ProviderGrpIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ds.FollowupARProviderGroup.ProviderGrpIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.FollowupARProviderGroup.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_AR_GROUP_ID, ds.FollowupARProviderGroup.ARGroupIdColumn.ColumnName, DbType.Int64);
        }
        #endregion
        #region Constructors
        public DALFollowUpARProviderGroup()
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

        

        #endregion
        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp InsertARProviderGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_PROVIDER_GROUP_INSERT, ds, ds.FollowupARProviderGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARProviderGroup::InsertARProviderGroup", PROC_FOLLOW_UP_AR_PROVIDER_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateARProviderGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_UPDATE, ds, ds.FollowupARProviderGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARProviderGroup::UpdateARProviderGroup", PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp LoadARProviderGroup(Int64 ARProviderGroupId, Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (ARProviderGroupId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ARProviderGroupId);

                if (ARGroupId == 0)
                    dbManager.AddParameters(1, PARM_AR_GROUP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_AR_GROUP_ID, ARGroupId);

                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);

                if (RecordPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PAGE_, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PAGE_, RecordPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.FollowupARProviderGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_SELECT, ds, ds.FollowupARProviderGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARProviderGroup::LoadARProviderGroup", PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteARProviderGroup(Int64 ARProviderGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROVIDER_GROUP_ID, ARProviderGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARProviderGroup::DeleteARProviderGroup", PROC_FOLLOW_UP_AR_FACILITY_PROVIDER_DELETE, ex);
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
