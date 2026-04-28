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
    public class DALLetterFields
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_LETTER_FIELDS_INSERT = "System.sp_LetterFieldsInsert";
        private const string PROC_LETTER_FIELDS_SELECT = "System.sp_LetterFieldsSelect";
        

        #endregion

        #region "Parameters"

        private const string PARM_LETTER_FIELDS_ID = "@LtrFieldId";
        private const string PARM_LETTER_ID = "@LetterId";
        private const string PARM_FIELD_ID = "@FieldId";
        private const string PARM_FORMAT_ID = "@FormatId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "CreatedBy";
        private const string PARM_CREATED_ON = "CreatedOn";
        private const string PARM_MODIFIED_BY = "ModifiedBy";
        private const string PARM_MODIFIED_ON = "ModifiedOn";

        private const string PARM_ERROR_MESSAGE = "ErrorMessage";

        #endregion

        #region Constructors
        public DALLetterFields()
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
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LETTER_FIELDS_ID, ds.LetterFields.LtrFieldIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LETTER_FIELDS_ID, ds.LetterFields.LtrFieldIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_LETTER_ID, ds.LetterFields.LetterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FIELD_ID, ds.LetterFields.FieldIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_FORMAT_ID, ds.LetterFields.FormatIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.LetterFields.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.LetterFields.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.LetterFields.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.LetterFields.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.LetterFields.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Inserts the LetterFields.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSLetter InsertLetterFields(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSLetter)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_LETTER_FIELDS_INSERT, ds, ds.LetterFields.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetterFields::InsertLetterFields", PROC_LETTER_FIELDS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLetter LoadLetterFields(Int32 LtrFieldId, Int32 LetterId)
        {
            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
               
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (LtrFieldId <= 0)
                    dbManager.AddParameters(0, PARM_LETTER_FIELDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LETTER_FIELDS_ID, LtrFieldId);
                if (LetterId <= 0)
                    dbManager.AddParameters(1, PARM_LETTER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LETTER_ID, LetterId);

                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LETTER_FIELDS_SELECT, ds, ds.LetterFields.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetterFields::LoadLetterFields", PROC_LETTER_FIELDS_SELECT, ex);
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
