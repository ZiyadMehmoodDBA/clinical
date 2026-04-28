using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALFields
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_FIELDS_INSERT = "System.sp_FieldsInsert";

        #endregion

        #region "Parameters"

        private const string PARM_FIELDS_ID = "@FieldId";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_CATEGORY_ID = "@CategoryId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_MANUAL_INPUT = "@ManualInput";
        private const string PARM_NULL_ALLOWED = "@NullAllowed";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors
        public DALFields()
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
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSLetter ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FIELDS_ID, ds.Fields.FieldIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FIELDS_ID, ds.Fields.FieldIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.Fields.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Fields.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CATEGORY_ID, ds.Fields.CategoryIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_ENTITY_ID, ds.Fields.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_MANUAL_INPUT, ds.Fields.ManualInputColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_NULL_ALLOWED, ds.Fields.NullAllowedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Fields.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.Fields.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.Fields.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Fields.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.Fields.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Inserts the Field.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSLetter InsertField(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSLetter)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FIELDS_INSERT, ds, ds.Fields.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::InsertField", PROC_FIELDS_INSERT, ex);
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
