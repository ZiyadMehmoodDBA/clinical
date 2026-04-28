using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALAdvanceDirective
    {
        
        #region " Stored Procedure Names"
        private const string PROC_ADVANCE_DIRECTIVE_INSERT = "Provider.sp_AdvanceDirectiveInsert";
        private const string PROC_ADVANCE_DIRECTIVE_UPDATE = "Provider.sp_AdvanceDirectiveUpdate";
        private const string PROC_ADVANCE_DIRECTIVE_DELETE = "Provider.sp_AdvanceDirectiveDelete";
        private const string PROC_ADVANCE_DIRECTIVE_SELECT = "Provider.sp_AdvanceDirectiveSelect";
        #endregion

        #region "Parameters"
        private const string PARM_ADVANCE_DIRECTIVE_ID = "@AdvanceDirectiveId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DATE_ENTERED = "@DateEntered";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALAdvanceDirective"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALAdvanceDirective()
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
        private void CreateParameters(IDBManager dbManager, DSPatientProfile ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.AdvanceDirectives.TableName].Columns.Count);

            //if (IsInsert == true)
            //    dbManager.AddParameters(0, PARM_ADVANCE_DIRECTIVE_ID, ds.AdvanceDirective.AdvanceDirectiveIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            //else
            //    dbManager.AddParameters(0, PARM_ADVANCE_DIRECTIVE_ID, ds.AdvanceDirective.AdvanceDirectiveIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(1, PARM_ADVANCE_DIRECTIVE_CODE, ds.AdvanceDirective.AdvanceDirectiveCodeColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(2, PARM_DESCRIPTION, ds.AdvanceDirective.DescriptionColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(3, PARM_DATE_ENTERED, ds.AdvanceDirective.DescriptionColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(4, PARM_IS_PRIMARY, ds.AdvanceDirective..ColumnName, DbType.Byte);
            //dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.AdvanceDirective.IsActiveColumn.ColumnName, DbType.Byte);
            //dbManager.AddParameters(6, PARM_CREATED_BY, ds.AdvanceDirective.CreatedByColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(7, PARM_CREATED_ON, ds.AdvanceDirective.CreatedOnColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.AdvanceDirective.ModifiedByColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.AdvanceDirective.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the advance directive.
        /// </summary>
        /// <param name="AdvanceDirectiveId">The advance directive identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSPatientProfile LoadAdvanceDirective(long AdvanceDirectiveId, string ShortName, string Description)
        {
            DSPatientProfile ds = new DSPatientProfile();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (AdvanceDirectiveId <= 0)
                    dbManager.AddParameters(0, PARM_ADVANCE_DIRECTIVE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ADVANCE_DIRECTIVE_ID, AdvanceDirectiveId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                ds = (DSPatientProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ADVANCE_DIRECTIVE_SELECT, ds, ds.AdvanceDirectives.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvanceDirective::LoadAdvanceDirective", PROC_ADVANCE_DIRECTIVE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the advance directive.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatientProfile UpdateAdvanceDirective(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatientProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ADVANCE_DIRECTIVE_UPDATE, ds, ds.AdvanceDirectives.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvanceDirective::UpdateAdvanceDirective", PROC_ADVANCE_DIRECTIVE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the advance directive.
        /// </summary>
        /// <param name="AdvanceDirectiveIds">The advance directive ids.</param>
        /// <returns></returns>
        public string DeleteAdvanceDirective(string AdvanceDirectiveIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ADVANCE_DIRECTIVE_ID, AdvanceDirectiveIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ADVANCE_DIRECTIVE_DELETE).ToString();
                if (returnValue != null)
                    throw new Exception(returnValue.ToString());

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvanceDirective::DeleteAdvanceDirective", PROC_ADVANCE_DIRECTIVE_DELETE, ex);
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
        /// Inserts the advance directive.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatientProfile InsertAdvanceDirective(DSPatientProfile ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPatientProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ADVANCE_DIRECTIVE_INSERT, ds, ds.AdvanceDirectives.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdvanceDirective::InsertAdvanceDirective", PROC_ADVANCE_DIRECTIVE_INSERT, ex);
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
