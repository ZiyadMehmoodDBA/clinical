using MDVision.Common.Shared;
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
    public class DALTemplate
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_TEMPLATE_INSERT = "Clinical.sp_TemplateInsert";
        private const string PROC_TEMPLATE_UPDATE = "Clinical.sp_TemplateUpdate";
        private const string PROC_TEMPLATE_DELETE = "Clinical.sp_TemplateDelete";
        private const string PROC_TEMPLATE_SELECT = "Clinical.sp_TemplateSelect";
        private const string PROC_TEMPLATE_TYPE_LOOKUP = "[Clinical].[sp_TemplateTypeLookup]";
        private const string PROC_TEMPLATE_LOOKUP = "[Clinical].[sp_TemplateLookup]";
        

        private const string PROC_TEMPLATESECTION_UPDATE = "[Clinical].[sp_TemplateSectionUpdate]";
        private const string PROC_TEMPLATESECTION_DELETE = "[Clinical].[sp_TemplateSectionDelete]";
        private const string PROC_TEMPLATESECTION_SELECT = "[Clinical].[sp_TemplateSectionSelect]";
        private const string PROC_TEMPLATESECTION_INSERT = "[Clinical].[sp_TemplateSectionInsert]";
        #endregion

        #region "Parameters"

        private const string PARM_TEMPLATE_ID = "@TemplateId";
        private const string PARM_TEMPLATE_TYPE_ID = "@TemplateTypeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        //  private const string PARM_IS_MANDATORY = "@IsMandatory";
        private const string PARM_SPECIALITY_ID = "@SpecialityId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_HTMLTEMPLATE = "@HTMLTemplate";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_LETTER_ID = "@LetterId";
        private const string PARM_FOLDER_ID = "@FolderId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_TEMPLATESECTION_ID = "@TemplateSectionId";
        private const string PARM_SECTION_ID = "@SectionID";

        #endregion

        #region Constructors
        public DALTemplate()
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
        private void CreateParameters(IDBManager dbManager, DSTemplateBuilder ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, ds.ClinicalTemplate.TemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, ds.ClinicalTemplate.TemplateIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.ClinicalTemplate.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ClinicalTemplate.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_TEMPLATE_TYPE_ID, ds.ClinicalTemplate.TemplateTypeIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(4, PARM_SPECIALITY_ID, ds.ClinicalTemplate.SpecialityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ClinicalTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ClinicalTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.ClinicalTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.ClinicalTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.ClinicalTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_HTMLTEMPLATE, ds.ClinicalTemplate.HTMLTemplateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_PROVIDER_ID, ds.ClinicalTemplate.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_LETTER_ID, ds.ClinicalTemplate.LetterIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_FOLDER_ID, ds.ClinicalTemplate.FolderIdColumn.ColumnName, DbType.Int32);

        }

        private void CreateParametersTemplateSection(IDBManager dbManager, DSTemplateBuilder ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TEMPLATESECTION_ID, ds.ClinicalTemplateSection.TemplateSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TEMPLATESECTION_ID, ds.ClinicalTemplateSection.TemplateSectionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SECTION_ID, ds.ClinicalTemplateSection.SectionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_TEMPLATE_ID, ds.ClinicalTemplateSection.TemplateIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_CREATED_BY, ds.ClinicalTemplateSection.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ClinicalTemplateSection.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.ClinicalTemplateSection.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.ClinicalTemplateSection.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the Clinical Templates.
        /// </summary>
        /// <param name="TemplateId">The Template identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadClinicalTemplate(long TemplateID, string ShortName, string Description, long TemplateTypeId, long SpecialityId, long ProviderId, string IsActive, Int64 rpp, Int64 pageNo)
        {
            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;


                dbManager.Open();
                dbManager.CreateParameters(10);
                if (TemplateID <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateID);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (TemplateTypeId <= 0)
                    dbManager.AddParameters(3, PARM_TEMPLATE_TYPE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_TEMPLATE_TYPE_ID, TemplateTypeId);
                if (SpecialityId <= 0)
                    dbManager.AddParameters(4, PARM_SPECIALITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_SPECIALITY_ID, SpecialityId);


                dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);
                if (ProviderId <= 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, ProviderId);



                if (pageNo <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); } else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, pageNo); }
                if (rpp <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, null); } else { dbManager.AddParameters(8, PARM_ROWS_PAGE, rpp); }

                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.ClinicalTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_SELECT, ds, ds.ClinicalTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplates::LoadClinicalTemplate", PROC_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Clinical Template.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertTemplate(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_INSERT, ds, ds.ClinicalTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::InsertTemplate", PROC_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Clinical Template
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateTemplate(ref DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_UPDATE, ds, ds.ClinicalTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::UpdateClinicalTemplate", PROC_TEMPLATE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Template.
        /// </summary>
        /// <param name="TemplateId">The Template identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteTemplate(string TemplateID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::DeleteTemplate", PROC_TEMPLATE_DELETE, ex);
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

        #region Template Section CRUD

        /// <summary>
        /// Deletes the Section from Template.
        /// </summary>
        /// <param name="TemplateId">The Template identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteTemplateSection(Int64 TemplateSectionId, Int64 SectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATESECTION_ID, TemplateSectionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_TEMPLATESECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::DeleteTemplateSection", PROC_TEMPLATESECTION_DELETE, ex);
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
        /// Loads the Template Sections.
        /// </summary>
        /// <param name="TemplateId">The Template identifier.</param> 
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadTemplateSection(long TemplateSectionId, long TemplateID)
        {

            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (TemplateSectionId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATESECTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATESECTION_ID, TemplateSectionId);

                if (TemplateID <= 0) { dbManager.AddParameters(1, PARM_TEMPLATE_ID, null); } else { dbManager.AddParameters(1, PARM_TEMPLATE_ID, TemplateID); }


                // dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ClinicalTemplateSection.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATESECTION_SELECT, ds, ds.ClinicalTemplateSection.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::LoadTemplateSection", PROC_TEMPLATESECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Template Section
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertTemplateSection(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersTemplateSection(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_TEMPLATESECTION_INSERT, ds, ds.ClinicalTemplateSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::InsertTemplateSection", PROC_TEMPLATESECTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Template Section
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateTemplateSection(ref DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersTemplateSection(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_TEMPLATESECTION_UPDATE, ds, ds.ClinicalTemplateSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::UpdateTemplateSection", PROC_TEMPLATESECTION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion
        #endregion

        #region "Lookups"

        public DSTemplateBuilderLookups LookupTemplateType(Int64 isNote = 0, Int64 TemplateID = 0)
        {
            DSTemplateBuilderLookups ds = new DSTemplateBuilderLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (isNote==0)
                {
                    dbManager.AddParameters(0, "@IsNote", DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, "@IsNote", isNote);
                }
                if (TemplateID == 0)
                {
                    dbManager.AddParameters(1, "@TemplateID", DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, "@TemplateID", TemplateID);
                }
                
                
                ds = (DSTemplateBuilderLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_TYPE_LOOKUP, ds, ds.TemplateType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::LookupTemplateType", PROC_TEMPLATE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSTemplateBuilderLookups LookupTemplate(Int64 TemplateTypeID=0)
        {
            DSTemplateBuilderLookups ds = new DSTemplateBuilderLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (TemplateTypeID==0)
	            {
		              dbManager.AddParameters(0,"@TemplateTypeID", DBNull.Value);
	            }else
	            {
                    dbManager.AddParameters(0, "@TemplateTypeID", TemplateTypeID);
	            }
                
                ds = (DSTemplateBuilderLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TEMPLATE_LOOKUP, ds, ds.Template.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTemplate::LookupTemplate", PROC_TEMPLATE_LOOKUP, ex);
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
