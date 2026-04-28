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
    public class DALPlanFeeLink
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PLAN_FEE_LINK_INSERT = "Provider.sp_PlanFeeLinkInsert";
        private const string PROC_PLAN_FEE_LINK_UPDATE = "Provider.sp_PlanFeeLinkUpdate";
        private const string PROC_PLAN_FEE_LINK_DELETE = "Provider.sp_PlanFeeLinkDelete";
        private const string PROC_PLAN_FEE_LINK_SELECT = "Provider.sp_PlanFeeLinkSelect";
        private const string PROC_PLAN_FEE_LINK_LOOKUP = "Provider.sp_PlanFeeLinkLookup";
        #endregion

        #region "Parameters"
        private const string PARM_PLAN_FEE_LINK_ID = "@PlanFeeLinkId";
        private const string PARM_SHORT_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
      
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPlanFeeLink()
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
        private void CreateParameters(IDBManager dbManager, DSFeeSchedule ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PLAN_FEE_LINK_ID, ds.PlanFeeLink.PlanFeeLinkIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PLAN_FEE_LINK_ID, ds.PlanFeeLink.PlanFeeLinkIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.PlanFeeLink.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.PlanFeeLink.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.PlanFeeLink.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PlanFeeLink.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.PlanFeeLink.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PlanFeeLink.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PlanFeeLink.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ENTITY_ID, ds.PlanFeeLink.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the plan fee link.
        /// </summary>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSFeeSchedule LoadPlanFeeLink(long PlanFeeLinkId, string ShortName, string Description, string EntityId,string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSFeeSchedule ds = new DSFeeSchedule();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (PlanFeeLinkId <= 0)
                    dbManager.AddParameters(0, PARM_PLAN_FEE_LINK_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PLAN_FEE_LINK_ID, PlanFeeLinkId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                if (IsActive == "")
                    dbManager.AddParameters(7, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);

                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.PlanFeeLink.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSFeeSchedule)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_SELECT, ds, ds.PlanFeeLink.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlanFeeLink::LoadPlanFeeLink", PROC_PLAN_FEE_LINK_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the plan fee link.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule UpdatePlanFeeLink(ref DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanFeeLink.GetChanges();
                ds = (DSFeeSchedule)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_UPDATE, ds, ds.PlanFeeLink.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanFeeLink.Rows[0][ds.PlanFeeLink.PlanFeeLinkIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlanFeeLink::UpdatePlanFeeLink", PROC_PLAN_FEE_LINK_UPDATE, ex);
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
        /// Deletes the plan fee link.
        /// </summary>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePlanFeeLink(string PlanFeeLinkId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSFeeSchedule ds = LoadPlanFeeLink(Convert.ToInt64(PlanFeeLinkId), null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanFeeLink;
                dbManager.AddParameters(0, PARM_PLAN_FEE_LINK_ID, PlanFeeLinkId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_DELETE);

                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.PlanFeeLink.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanFeeLink.Rows[0][ds.PlanFeeLink.PlanFeeLinkIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlanFeeLink::DeletePlanFeeLink", PROC_PLAN_FEE_LINK_DELETE, ex);
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
        /// Inserts the plan fee link.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSFeeSchedule InsertPlanFeeLink(ref DSFeeSchedule ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PlanFeeLink.GetChanges();
                ds = (DSFeeSchedule)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_INSERT, ds, ds.PlanFeeLink.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.PlanFeeLink.Rows[0][ds.PlanFeeLink.PlanFeeLinkIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPlanFeeLink::InsertPlanFeeLink", PROC_PLAN_FEE_LINK_INSERT, ex);
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
    }
}
