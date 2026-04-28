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
    public class DALLetter
    {
        
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_LETTER_INSERT = "System.sp_LettersInsert";
        private const string PROC_LETTER_UPDATE = "System.sp_LettersUpdate";
        private const string PROC_LETTER_DELETE = "System.sp_LettersDelete";
        private const string PROC_LETTER_SELECT = "System.sp_LettersSelect";
        private const string PROC_LETTER_CATEGORY_LOOKUP = "System.sp_LtrCategoryLookup";
        private const string PROC_FIELD_FORMATS_LOOKUP = "System.sp_FieldFormatsLookup";
        private const string PROC_FIELDS_LOOKUP = "System.sp_FieldsLookup";

        private const string PROC_FIELDLETTER_DELETE = "System.sp_FIELDLettersIDDelete";
        private const string PROC_LETTERS_LOOKUP = "System.sp_LettersLookup";

        #endregion

        #region "Parameters"

        private const string PARM_LETTER_ID = "@LetterId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_HTML_DOCUMENT = "@HtmlDocument";
        private const string PARM_CATEGORY_ID = "@CategoryId";
        private const string PARM_DOCUMENT_TYPE_ID = "@DocumentTypeId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_LABELED = "@IsLabeled";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_IS_SHOW = "@IsShow";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_LETTERFIELD_ID = "@LetterFieldId";
        
        #endregion

        #region Constructors
        public DALLetter()
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
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LETTER_ID, ds.Letter.TemplateLetterIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LETTER_ID, ds.Letter.TemplateLetterIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Letter.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Letter.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CATEGORY_ID, ds.Letter.CategoryIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_DOCUMENT_TYPE_ID, ds.Letter.DocumentTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_ENTITY_ID, ds.Letter.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_IS_LABELED, ds.Letter.IsLabeledColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Letter.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.Letter.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.Letter.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Letter.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.Letter.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_HTML_DOCUMENT, ds.Letter.HtmlDocumentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_IS_SHOW, ds.Letter.IsShowColumn.ColumnName, DbType.Byte);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the Letter.
        /// </summary>
        /// <param name="LetterId">The Letter identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSLetter LoadLetter(long letterID, string shortName, string description, string categoryID, string documentTypeID, string entityID, bool? labeleb, string active)
        {

            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (shortName == "")
                    shortName = null;
                if (description == "")
                    description = null;
                if (categoryID == "")
                    categoryID = null;
                if (documentTypeID == "")
                    documentTypeID = null;
                if (categoryID == "")
                    categoryID = null;
                if (entityID == "")
                    entityID = null;
                //if (labeleb == "")
                //    labeleb = null;
                if (active == "")
                    active = null;


                dbManager.Open();
                dbManager.CreateParameters(9);
                if (letterID <= 0)
                    dbManager.AddParameters(0, PARM_LETTER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LETTER_ID, letterID);
                dbManager.AddParameters(1, PARM_ENTITY_ID, entityID);
                dbManager.AddParameters(2, PARM_SHORT_NAME, shortName);
                dbManager.AddParameters(3, PARM_DESCRIPTION, description);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
                dbManager.AddParameters(5, PARM_DOCUMENT_TYPE_ID, documentTypeID);
                dbManager.AddParameters(6, PARM_CATEGORY_ID, categoryID);                
                dbManager.AddParameters(7, PARM_IS_LABELED, labeleb);
                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);
                

                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LETTER_SELECT, ds, ds.Letter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::LoadLetter", PROC_LETTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Letter.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSLetter InsertLetter(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSLetter)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_LETTER_INSERT, ds, ds.Letter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::InsertLetter", PROC_LETTER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Letter
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSLetter UpdateLetter(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSLetter)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_LETTER_UPDATE, ds, ds.Letter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::UpdateLetter", PROC_LETTER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Letter.
        /// </summary>
        /// <param name="LetterId">The Letter identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteLetter(string LetterID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LETTER_ID, LetterID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_LETTER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::DeleteLetter", PROC_LETTER_DELETE, ex);
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
        /// Deletes the Fields Inserted against Letters Created By User.
        /// </summary>
        /// <param name="LetterId">The Field  identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DELETED_FieldsIDsFrom_USER(string deletedFieldsIDsFromLetter, Int64 LetterID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_LETTERFIELD_ID, deletedFieldsIDsFromLetter);
                dbManager.AddParameters(1, PARM_LETTER_ID, LetterID);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
               
                
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FIELDLETTER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::DeleteLetter", PROC_FIELDLETTER_DELETE, ex);
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

        public DSLetterLookup LookupCategory()
        {
            DSLetterLookup ds = new DSLetterLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSLetterLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LETTER_CATEGORY_LOOKUP, ds, ds.Category.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::LookupCategory", PROC_LETTER_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLetterLookup LookupLetterFieldFormat()
        {
            DSLetterLookup ds = new DSLetterLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSLetterLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FIELD_FORMATS_LOOKUP, ds, ds.FieldFromat.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::LookupLetterFieldFormat", PROC_FIELD_FORMATS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLetterLookup LookupLetterFields()
        {
            DSLetterLookup ds = new DSLetterLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSLetterLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FIELDS_LOOKUP, ds, ds.Fields.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::LookupLetterFields", PROC_FIELDS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLetterLookup LookupLetters(long CategoryId)
        {
            DSLetterLookup ds = new DSLetterLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                //if (SharedObj.IsAdmin)
                //    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (CategoryId <= 0)
                    dbManager.AddParameters(1, PARM_CATEGORY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CATEGORY_ID, CategoryId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(2, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSLetterLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LETTERS_LOOKUP, ds, ds.Letter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLetter::LookupLetters", PROC_LETTERS_LOOKUP, ex);
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
