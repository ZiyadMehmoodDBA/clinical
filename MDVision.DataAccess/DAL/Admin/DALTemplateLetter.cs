// Created By:  Muhammad Ahmad Imran
// Created Date: 1/02/2016
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALTemplateLetter
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_COMPLAINT_TEMPLATE_LETTER_CATEGORY_LOOKUP = "Clinical.sp_Template_LetterCategoryLookup";
        private const string PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORY_LOOKUP = "Clinical.sp_Template_LetterTagCategoryLookup";
        private const string PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORYNAME_LOOKUP = "Clinical.sp_Template_LetterTagNameLookup";
        private const string PROC_TEMPLATE_LETTER_SELECT = "Clinical.sp_Template_LetterSelect";
        private const string PROC_TEMPLATE_LETTER_INSERT = "Clinical.sp_Template_LetterInsert";
        private const string PROC_TEMPLATE_LETTER_DELETE = "Clinical.sp_Template_LetterDelete";
        private const string PROC_TEMPLATE_LETTER_UPDATE = "Clinical.sp_Template_LetterUpdate";
        private const string PROC_LETTER_SELECT_FOR_SOAPTEXT = "Clinical.sp_PatLetterSelectForSoapText";
        private const string PROC_ATTACH_PAT_LETTER = "Clinical.sp_AttachLetterWithNotes";
        private const string PROC_DETACH_PAT_LETTER = "Clinical.sp_DetachLetterWithNotes";

        #endregion

        #region "Parameters"

        private const string PARM_CATEGORY_ID = "@CategoryId";
        private const string PARM_TAG_CATEGORY_ID = "@TagCategoryId";
        private const string PARM_TAG_CATEGORYNAME_ID = "@TagCategoryNameId";
        private const string PARM_TEMPLATE_LETTER_ID = "@TemplateLetterId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_pPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_TEMPLATE_CONTENT = "@TemplateContent";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_TAG_IDS = "@TagIds";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_PAT_LETTER_IDs = "@PatLetterIds ";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_NOTE_ID = "@NoteId";
        #endregion

        #region Constructors
        public DALTemplateLetter()
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

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 2/03/2016
        //OverView: Methods "CreateParametersForInsertLetter" for create Parameters for insert Template Letter
        private void CreateParametersForInsertLetter(IDBManager dbManager, DSLetter ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, ds.Letter.TemplateLetterIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, ds.Letter.TemplateLetterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_NAME, ds.Letter.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Letter.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CATEGORY_ID, ds.Letter.CategoryIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.Letter.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_TEMPLATE_CONTENT, ds.Letter.TemplateContentColumn.ColumnName, DbType.String);

            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(6, PARM_ENTITY_ID, ds.Letter.EntityIdColumn.ColumnName, DbType.Int64);

            else
                dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            

            dbManager.AddParameters(7, PARM_TAG_IDS, ds.Letter.TagIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.Letter.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.Letter.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.Letter.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFY_ON, ds.Letter.ModifiedOnColumn.ColumnName, DbType.DateTime);
            //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
            dbManager.AddParameters(12, PARM_PROVIDER_IDS, ds.Letter.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SPECIALTY_IDS, ds.Letter.SpecialtyIdsColumn.ColumnName, DbType.String);
            //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
        }





        #endregion

        #region "Template Letter LookUps"
        public DSClinicalLetterTemplateLookup GetClinicalTemplateLetterCategory()
        {
            DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLetterTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_TEMPLATE_LETTER_CATEGORY_LOOKUP, ds, ds.Template_LetterCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetClinicalTemplateLetterCategory", PROC_COMPLAINT_TEMPLATE_LETTER_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //GetClinicalTemplateLetterTagCategory
        public DSClinicalLetterTemplateLookup GetClinicalTemplateLetterTagCategory()
        {
            DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLetterTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORY_LOOKUP, ds, ds.Template_Letter_TagCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetClinicalTemplateLetterTagCategory", PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalLetterTemplateLookup GetClinicalTemplateLetterTagCategoryName(int TagCategoryNameId)
        {
            DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_TAG_CATEGORY_ID, TagCategoryNameId);
                ds = (DSClinicalLetterTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORYNAME_LOOKUP, ds, ds.Template_Letter_TagCategoryName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetClinicalTemplateLetterTagCategoryName", PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORYNAME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

       

        #region CRUD for Letter Template.
        /// <summary>
        /// Author : Khaleel Ur Rehman.
        /// Purpose : To load Letter Templates.
        /// Date : 05-Feb-2016
        /// </summary>
        /// <param name="TemplateLetterId"></param>
        /// <param name="IsActive"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        /// // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-515
        public DSLetter loadLetterTemplates(long TemplateLetterId, bool? IsActive, string name, string description, int categoryId, int pageNo, int rpp)
        {
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-515
            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                /*if (IsActive == "")
                    IsActive = null;*/
                dbManager.Open();
                dbManager.CreateParameters(9);
                if (TemplateLetterId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, TemplateLetterId);
                
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);//kkkrrrrrrr
                //----------------
                if (string.IsNullOrEmpty(name))
                    dbManager.AddParameters(2, PARM_NAME, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_NAME, name);

                if (string.IsNullOrEmpty(description))
                    dbManager.AddParameters(3, PARM_DESCRIPTION, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_DESCRIPTION, description);

                if (categoryId <= 0)
                    dbManager.AddParameters(4, PARM_CATEGORY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_CATEGORY_ID, categoryId);
                //----------------
                if (pageNo <= 0) { dbManager.AddParameters(5, PARM_PAGE_NUMBER, DBNull.Value); } else { dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNo); }
                if (rpp <= 0) { dbManager.AddParameters(6, PARM_ROWS_pPAGE, DBNull.Value); } else { dbManager.AddParameters(6, PARM_ROWS_pPAGE, rpp); }

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Letter.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);

                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_SELECT, ds, ds.Letter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::loadLetterTemplates", PROC_TEMPLATE_LETTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 02/03/2016
        //OverView: Methods "GetTemplateLetterData" for Get Template letter Data
        public DSLetter GetTemplateLetterData(long TemplateLetterId)
        {
            DSLetter ds = new DSLetter();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (TemplateLetterId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, TemplateLetterId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, DBNull.Value);
                dbManager.AddParameters(2, PARM_NAME, DBNull.Value);
                dbManager.AddParameters(3, PARM_DESCRIPTION, DBNull.Value);
                dbManager.AddParameters(4, PARM_CATEGORY_ID, DBNull.Value);
                dbManager.AddParameters(5, PARM_PAGE_NUMBER, DBNull.Value);
                dbManager.AddParameters(6, PARM_ROWS_pPAGE, DBNull.Value);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, DBNull.Value);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);

                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_SELECT, ds, ds.Letter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::GetTemplateLetterData", PROC_TEMPLATE_LETTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 02/03/2016
        //OverView: Methods "InsertTemplateLetter" for save Template letter 
        public DSLetter InsertTemplateLetter(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertLetter(dbManager, ds, true);
                ds = (DSLetter)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_INSERT, ds, ds.Letter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::InsertTemplateLetter", PROC_TEMPLATE_LETTER_INSERT, ex);
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


        // Created By:  Muhammad Ahmad Imran
        // Created Date: 02/03/2016
        //OverView: Methods "UpdateTemplateLetter" for Update Template letter Data
        public DSLetter UpdateTemplateLetter(DSLetter ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertLetter(dbManager, ds, false);
                ds = (DSLetter)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_UPDATE, ds, ds.Letter.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::UpdateTemplateLetter", PROC_TEMPLATE_LETTER_UPDATE, ex);
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
        /// Author : Khaleel Ur Rehman.
        /// Purpose : To delete Letter Templates.
        /// Date : 05-Feb-2016
        /// </summary>
        /// <param name="TemplateLetterId"></param>
        /// <returns></returns>
        public string deleteLetterTemplate(string TemplateLetterId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_LETTER_ID, TemplateLetterId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_TEMPLATE_LETTER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::deleteLetterTemplate", PROC_TEMPLATE_LETTER_DELETE, ex);
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
        public DSLetter LoadPatLettersForSoap(string patletterIds, long patientId)
        {
            IDBManager dbManager = null;
            try
            {
                DSLetter ds = new DSLetter();
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(patletterIds))
                    dbManager.AddParameters(0, PARM_PAT_LETTER_IDs, null);
                else
                    dbManager.AddParameters(0, PARM_PAT_LETTER_IDs, patletterIds);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LETTER_SELECT_FOR_SOAPTEXT, ds, ds.PatientTemplateLetter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::LoadPatLettersForSoap", PROC_LETTER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager?.Dispose();
            }
        }
        public DSLetter AttachPatLettersWithNotes(string patletterIds, long noteId)
        {
            IDBManager dbManager = null;
            try
            {
                dbManager = ClientConfiguration.GetDBManager();
                DSLetter ds = new DSLetter();
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_PAT_LETTER_IDs, patletterIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(2, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_MODIFY_ON, DateTime.Now);
                ds = (DSLetter)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PAT_LETTER, ds, ds.Letter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::AttachPatLettersWithNotes", PROC_ATTACH_PAT_LETTER, ex);
                throw ex;
            }
            finally
            {
                dbManager?.Dispose();
            }
        }
        public string DetachPatLettersWithNotes(string patletterIds, long noteId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PAT_LETTER_IDs, patletterIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DETACH_PAT_LETTER));
                if (!string.IsNullOrEmpty(returnVal))
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplateLetter::DetachPatLettersWithNotes", PROC_DETACH_PAT_LETTER, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager?.Dispose();
            }
        }
        #endregion
    }
}
