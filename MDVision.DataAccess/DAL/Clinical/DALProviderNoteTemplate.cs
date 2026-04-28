// Created By:  Muhammad Ahmad Imran
// Created Date: 03/16/2016
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALProviderNoteTemplate
    {
        #region Variable

        #endregion
        #region "Stored Procedure Names"
        private const string PROC_NOTES_TEMPLATE_SELECT = "Clinical.sp_NotesTemplateSelect";
        private const string PROC_NOTES_TEMPLATE_UPDATE = "Clinical.sp_NoteTemplateUpdate";
        private const string PROC_NOTES_TEMPLATE_INSERT = "Clinical.sp_NotesTemplateInsert";
        private const string PROC_NOTES_TEMPLATE_DELETE = "Clinical.sp_NotesTemplateDelete";

        private const string PROC_NOTE_TEMPLATE_TYPE_LOOKUP = "Clinical.sp_NotesTemplateTypeLookup";
        private const string PROC_NOTE_PH_ENCOUNTER_TEMPLATE_TYPE_LOOKUP = "Clinical.sp_PhoneEncounterTemplateTypeLookup";

        private const string PROC_NOTE_TAG_CATEGORY_LOOKUP = "Clinical.sp_NotesTagCategoryLookup";
        private const string PROC_NOTE_TAG_NAME_LOOKUP = "Clinical.sp_NotesTagNameLookup";
        private const string PROC_NOTES_TEMPLATE_TYPE_INSERT = "Clinical.sp_NotesTemplateTypeInsert";

        private const string PROC_NOTE_TEMPLATE_LOOKUP = "Clinical.sp_NotesTemplateLookup";

        private const string PROC_ROS_DATA_TEMPLATE_LOOKUP = "Clinical.sp_ROSDataTemplateLookup";
        private const string PROC_PE_DATA_TEMPLATE_LOOKUP = "Clinical.sp_PhysExamDataTemplateLookup";



        #endregion

        #region "Parameters"
        private const string PARM_NOTES_TEMPLATE_ID = "@NotesTemplateID";

        private const string PARM_NOTES_TAG_CATEGORY_ID = "@NotesTagCategoryId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_NOTE_TEMPLATE_NAME = "@NotesTemplateName";
        private const string PARM_PROVIDER_ID = "@ProviderId";

        private const string PARM_PROVIDER_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_NOTES_TAGNAME_IDS = "@NotesTagNameIds";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ROS_DATATEMPT_ID = "@ROSDataTemptId";
        private const string PARM_HTML_TEMPLATE = "@HTMLTemplate";
        private const string PARM_PE_DATATEMPT_ID = "@PEDataTemptId";
        private const string PARM_HPI_TEMPLATE_ID = "@HPITemplateId";

        private const string PARM_TEMPLATE_TYPE_ID = "@TemplateTypeId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_NOTES_TEMPLATE_TYPE_ID = "@NotesTemplateTypeId";
        private const string PARM_SHORTNAME = "@ShortName";
        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        #endregion

        #region Constructors
        public DALProviderNoteTemplate()
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
        private void createParameters(IDBManager dbManager, DSClinicalNoteTemplate ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(17);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_ID, ds.NotesTemplate.NotesTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_ID, ds.NotesTemplate.NotesTemplateIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NOTE_TEMPLATE_NAME, ds.NotesTemplate.NoteTemplateNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(2, PARM_TEMPLATE_TYPE_ID, ds.NotesTemplate.TemplateTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_ROS_DATATEMPT_ID, ds.NotesTemplate.ROSDataTemptIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PE_DATATEMPT_ID, ds.NotesTemplate.PEDataTemptIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_HTML_TEMPLATE, ds.NotesTemplate.HTMLTemplateColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.NotesTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.NotesTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.NotesTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.NotesTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.NotesTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);


            dbManager.AddParameters(11, PARM_PROVIDER_IDS, ds.NotesTemplate.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_SPECIALTY_IDS, ds.NotesTemplate.SpecialtyIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_NOTES_TAGNAME_IDS, ds.NotesTemplate.NotesTagNameIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ENTITY_ID, ds.NotesTemplate.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_HPI_TEMPLATE_ID, ds.NotesTemplate.HPITemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_ORDER_SET_ID, ds.NotesTemplate.OrderSetIdColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Creates the parameters.
        /// created by:M Ahmad Imran
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        public void createParametersForNoteType(IDBManager dbManager, DSClinicalNoteTemplateLookup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_TYPE_ID, ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_TYPE_ID, ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORTNAME, ds.NotesTemplateType.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.NotesTemplateType.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_CREATED_BY, ds.NotesTemplateType.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.NotesTemplateType.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.NotesTemplateType.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.NotesTemplateType.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion
        #region "Provider Note Template lookUp"
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 03/16/2016
        public DSClinicalNoteTemplateLookup GetNoteTemplateType(bool? isActive, Int64 TemplateId)
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);
                if (TemplateId <= 0)
                {
                    dbManager.AddParameters(1, PARM_NOTES_TEMPLATE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTES_TEMPLATE_ID, TemplateId);
                }
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_TEMPLATE_TYPE_LOOKUP, ds, ds.NotesTemplateType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetNoteTemplateType", PROC_NOTE_TEMPLATE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup GetPhoneEncounterTemplateType(bool? isActive, Int64 TemplateId)
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);
                //if (TemplateId <= 0)
                //{
                //    dbManager.AddParameters(1, PARM_NOTES_TEMPLATE_ID, null);
                //}
                //else
                //{
                //    dbManager.AddParameters(1, PARM_NOTES_TEMPLATE_ID, TemplateId);
                //}
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_PH_ENCOUNTER_TEMPLATE_TYPE_LOOKUP, ds, ds.NotesTemplateType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetPhoneEncounterTemplateType", PROC_NOTE_PH_ENCOUNTER_TEMPLATE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup GetNoteTemplatTagCategory()
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_TAG_CATEGORY_LOOKUP, ds, ds.NotesTagCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetNoteTemplatTagCategory", PROC_NOTE_TAG_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup GetNoteTemplateTagName(Int32 NoteTagCategory)
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (NoteTagCategory <= 0)
                {
                    dbManager.AddParameters(0, PARM_NOTES_TAG_CATEGORY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_NOTES_TAG_CATEGORY_ID, NoteTagCategory);
                }

                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_TAG_NAME_LOOKUP, ds, ds.NotesTagName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetNoteTemplatTagName", PROC_NOTE_TAG_NAME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup LookupNotesTemplate(Int64 TemplateTypeID = 0, Int64 ProviderID = 0)
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                //if (TemplateTypeID <= 0)
                //{
                //    dbManager.AddParameters(0, PARM_TEMPLATE_TYPE_ID, null);
                //}
                //else
                //{
                //    dbManager.AddParameters(0, PARM_TEMPLATE_TYPE_ID, TemplateTypeID);
                //}
                dbManager.AddParameters(0, PARM_TEMPLATE_TYPE_ID, null);

                if (ProviderID <= 0)
                {
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PROVIDER_ID, ProviderID);
                }

                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_TEMPLATE_LOOKUP, ds, ds.NotesTemplateLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::LookupNotesTemplate", PROC_NOTE_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalNoteTemplateLookup GetROSDataTemplate()
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DATA_TEMPLATE_LOOKUP, ds, ds.ROSDataTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetROSDataTemplate", PROC_ROS_DATA_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalNoteTemplateLookup GetPEDataTemplate()
        {
            DSClinicalNoteTemplateLookup ds = new DSClinicalNoteTemplateLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalNoteTemplateLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PE_DATA_TEMPLATE_LOOKUP, ds, ds.PEDataTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::GetPEDataTemplate", PROC_PE_DATA_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "CRUD Provider Note Template"
        public DSClinicalNoteTemplate loadNoteTemplate(long notesTemplateID, string noteTemplateName, string providerIds, string specialtyIds, int? entityId, long templateTypeId, int? isActive, int pageNumber, int rowsPerPage)
        {
            DSClinicalNoteTemplate ds = new DSClinicalNoteTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(10);
                if (notesTemplateID <= 0)
                    dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_ID, notesTemplateID);

                if (string.IsNullOrEmpty(noteTemplateName))
                {
                    dbManager.AddParameters(1, PARM_NOTE_TEMPLATE_NAME, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_TEMPLATE_NAME, noteTemplateName);
                }

                if (string.IsNullOrEmpty(providerIds))
                {
                    dbManager.AddParameters(2, PARM_PROVIDER_IDS, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_PROVIDER_IDS, providerIds);
                }
                if (string.IsNullOrEmpty(specialtyIds))
                {
                    dbManager.AddParameters(3, PARM_SPECIALTY_IDS, null);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_SPECIALTY_IDS, specialtyIds);
                }

                if (entityId <= 0)
                    dbManager.AddParameters(4, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ENTITY_ID, entityId);
                if (templateTypeId <= 0)
                    dbManager.AddParameters(5, PARM_TEMPLATE_TYPE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_TEMPLATE_TYPE_ID, templateTypeId);


                if (isActive == null)
                {
                    dbManager.AddParameters(6, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_IS_ACTIVE, isActive);
                }

                if (pageNumber <= 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage <= 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.NotesTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSClinicalNoteTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTES_TEMPLATE_SELECT, ds, ds.NotesTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::loadNoteTemplate", PROC_NOTES_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Updates the NoteTemplate.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalNoteTemplate updateNoteTemplate(DSClinicalNoteTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, false);
                ds = (DSClinicalNoteTemplate)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_NOTES_TEMPLATE_UPDATE, ds, ds.NotesTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::UpdateNoteTemplate", PROC_NOTES_TEMPLATE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the NoteTemplate.
        /// </summary>
        /// <param name="MsgId">The NoteTemplate identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string deleteNoteTemplate(long templateID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTES_TEMPLATE_ID, templateID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTES_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::DeleteNoteTemplate", PROC_NOTES_TEMPLATE_DELETE, ex);
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
        /// Inserts the NoteTemplate.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalNoteTemplate insertNoteTemplate(DSClinicalNoteTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true);
                ds = (DSClinicalNoteTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_NOTES_TEMPLATE_INSERT, ds, ds.NotesTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::InsertNoteTemplate", PROC_NOTES_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Created by: M Ahmad Imran
        /// Inserts the NoteTemplateType.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSClinicalNoteTemplateLookup InsertNewNoteType(DSClinicalNoteTemplateLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForNoteType(dbManager, ds, true);
                ds = (DSClinicalNoteTemplateLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_NOTES_TEMPLATE_TYPE_INSERT, ds, ds.NotesTemplateType.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProviderNoteTemplate::InsertNewNoteType", PROC_NOTES_TEMPLATE_TYPE_INSERT, ex);
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
