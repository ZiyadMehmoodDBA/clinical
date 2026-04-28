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
    public class DALModifier
    {
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_MODIFIER_INSERT = "Provider.sp_ModifierInsert";
        private const string PROC_MODIFIER_UPDATE = "Provider.sp_ModifierUpdate";
        private const string PROC_MODIFIER_DELETE = "Provider.sp_ModifierDelete";
        private const string PROC_MODIFIER_SELECT = "Provider.sp_ModifierSelect";
        private const string PROC_MODIFIER_LOOKUP = "Provider.sp_ModifierLookup";
        #endregion

        #region "Parameters"
        private const string PARM_MODIFIER_ID = "@ModifierId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_MODIFIER_CODE = "@ModifierCode";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_BIT = "@Bit";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        
        #endregion

        #region Constructors
        public DALModifier()
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
        private void CreateParameters(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MODIFIER_ID, ds.Modifier.ModifierIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MODIFIER_ID, ds.Modifier.ModifierIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MODIFIER_CODE, ds.Modifier.ModifierCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Modifier.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Modifier.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.Modifier.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.Modifier.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.Modifier.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.Modifier.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <param name="ModifierCode">The modifier code.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSCodes LoadModifier(long ModifierId, string ModifierCode, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ModifierCode == "")
                    ModifierCode = null;

                if (Description == "")
                    Description = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ModifierId <= 0)
                    dbManager.AddParameters(0, PARM_MODIFIER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODIFIER_ID, ModifierId);
                dbManager.AddParameters(1, PARM_MODIFIER_CODE, ModifierCode);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.Modifier.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, IsActive);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODIFIER_SELECT, ds, ds.Modifier.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModifier::LoadModifier", PROC_MODIFIER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateModifier(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Modifier.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MODIFIER_UPDATE, ds, ds.Modifier.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Modifier.Rows[0][ds.Modifier.ModifierIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModifier::UpdateModifier", PROC_MODIFIER_UPDATE, ex);
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
        /// Deletes the modifier.
        /// </summary>
        /// <param name="ModifierIds">The modifier ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteModifier(string ModifierIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadModifier(Convert.ToInt64(ModifierIds), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Modifier;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MODIFIER_ID, ModifierIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MODIFIER_DELETE).ToString();
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.Modifier.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Modifier.Rows[0][ds.Modifier.ModifierIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModifier::DeleteModifier", PROC_MODIFIER_DELETE, ex);
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
        /// Inserts the modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertModifier(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Modifier.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODIFIER_INSERT, ds, ds.Modifier.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Modifier.Rows[0][ds.Modifier.ModifierIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModifier::InsertModifier", PROC_MODIFIER_INSERT, ex);
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

        #region Lookups
        /// <summary>
        /// Lookups the modifier.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupModifier(string ModifierCode, int IsEqule, string IsActive)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_MODIFIER_CODE, ModifierCode);
                dbManager.AddParameters(1, PARM_BIT, IsEqule);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                dbManager.Open();
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODIFIER_LOOKUP, ds, ds.Modifier.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALModifier::LookupModifier", PROC_MODIFIER_LOOKUP, ex);
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
