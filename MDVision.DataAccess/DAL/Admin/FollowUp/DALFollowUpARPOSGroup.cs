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
   public class DALFollowUpARPOSGroup
    {
       #region Variable
        
        #endregion
        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_AR_POS_GROUP_INSERT = "Billing.sp_FollowupARPOSGroupInsert";
        private const string PROC_FOLLOW_UP_AR_POS_GROUP_UPDATE = "Billing.sp_FollowupARPOSGroupUpdate";
        private const string PROC_FOLLOW_UP_AR_POS_GROUP_DELETE = "Billing.sp_FollowupARPOSGroupDelete";
        private const string PROC_FOLLOW_UP_AR_POS_GROUP_SELECT = "Billing.sp_FollowupARPOSGroupSelect";
        
        #endregion
        #region "Parameters"

        private const string PARM_POS_GROUP_ID = "@POSGrpId";
        private const string PARM_POS_ID = "@PlaceOfServiceId";
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
                dbManager.AddParameters(0, PARM_POS_GROUP_ID, ds.FollowupARPOSGroup.POSGrpIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_POS_GROUP_ID, ds.FollowupARPOSGroup.POSGrpIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_POS_ID, ds.FollowupARPOSGroup.PlaceOfServiceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_AR_GROUP_ID, ds.FollowupARPOSGroup.ARGroupIdColumn.ColumnName, DbType.Int64);
        }
        #endregion
        #region Constructors
        public DALFollowUpARPOSGroup()
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
        public DSFollowUp InsertARPOSGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_POS_GROUP_INSERT, ds, ds.FollowupARPOSGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARPOSGroup::InsertARPOSGroup", PROC_FOLLOW_UP_AR_POS_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateARPOSGroup(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_POS_GROUP_UPDATE, ds, ds.FollowupARPOSGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARPOSGroup::UpdateARPOSGroup", PROC_FOLLOW_UP_AR_POS_GROUP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp LoadARPOSGroup(Int64 ARPOSGroup , Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (ARPOSGroup == 0)
                    dbManager.AddParameters(0, PARM_POS_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_POS_GROUP_ID, ARPOSGroup);

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

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.FollowupARPOSGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_POS_GROUP_SELECT, ds, ds.FollowupARPOSGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARPOSGroup::LoadARPOSGroup", PROC_FOLLOW_UP_AR_POS_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteARPOSGroup(Int64 ARPOSGroup)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_POS_GROUP_ID, ARPOSGroup);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_POS_GROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpARPOSGroup::DeleteARPOSGroup", PROC_FOLLOW_UP_AR_POS_GROUP_DELETE, ex);
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
