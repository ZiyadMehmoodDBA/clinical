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
    public class DALSection
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_SECTION_INSERT = "Clinical.sp_SectionInsert";
        private const string PROC_SECTION_UPDATE = "Clinical.sp_SectionUpdate";
        private const string PROC_SECTION_DELETE = "Clinical.sp_SectionDelete";
        private const string PROC_SECTION_SELECT = "Clinical.sp_SectionSelect";

        private const string  PROC_SECTION_QUESTIONGROUP_DELETE = "[Clinical].[sp_QuestionGroupSectionDelete]";
        private const string  PROC_SECTION_QUESTIONGROUP_INSERT = "[Clinical].[sp_QuestionGroupSectionInsert]";
        private const string  PROC_SECTION_QUESTIONGROUP_SELECT = "[Clinical].[sp_QuestionGroupSectionSelect]";
        private const string  PROC_SECTION_QUESTIONGROUP_UPDATE = "[Clinical].[sp_QuestionGroupSectionUpdate]";

        private const string PROC_SECTION_TYPE_LOOKUP = "[Clinical].[sp_SectionTypeLookup]";

        #endregion

        #region "Parameters"

        private const string PARM_SECTION_ID = "@SectionId";
        private const string PARM_SECTION_TYPE_ID = "@SectionTypeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SECTION_TITLE = "@SectionTitle";
        private const string PARM_CANVAS_COLUMNS = "@CanvasColumns";
        private const string PARM_SPECIALITY_ID = "@SpecialityId";
        private const string PARM_IS_RECURRING = "@IsRecurring";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_HTMLTEMPLATECOLUMN = "@HTMLTemplate";

        private const string PARM_USER_ID = "@UserId";

        private const string PARM_SECTION_QUESTIONGROUP_ID = "@QuesGroupSectionId";
        private const string PARM_QUESTIONGROUP_ID = "@QuestionGroupId";



        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors
        public DALSection()
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
                dbManager.AddParameters(0, PARM_SECTION_ID, ds.ClinicalSection.SectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SECTION_ID, ds.ClinicalSection.SectionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SECTION_TYPE_ID, ds.ClinicalSection.SectionTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.ClinicalSection.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.ClinicalSection.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SECTION_TITLE, ds.ClinicalSection.SectionTitleColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CANVAS_COLUMNS, ds.ClinicalSection.CanvasColumnsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SPECIALITY_ID, ds.ClinicalSection.SpecialityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_IS_RECURRING, ds.ClinicalSection.IsRecurringColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.ClinicalSection.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.ClinicalSection.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.ClinicalSection.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.ClinicalSection.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.ClinicalSection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_HTMLTEMPLATECOLUMN, ds.ClinicalSection.HTMLTemplateColumn.ColumnName, DbType.String);
        }

        private void CreateParametersSectionQuestionGroup(IDBManager dbManager, DSTemplateBuilder ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);
           
            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_SECTION_QUESTIONGROUP_ID, ds.ClinicalSectionQuestionGroup.QuesGroupSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else { 
                dbManager.AddParameters(0, PARM_SECTION_QUESTIONGROUP_ID, ds.ClinicalSectionQuestionGroup.QuesGroupSectionIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_QUESTIONGROUP_ID, ds.ClinicalSectionQuestionGroup.QuestionGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SECTION_ID, ds.ClinicalSectionQuestionGroup.SectionIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_CREATED_BY, ds.ClinicalSectionQuestionGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ClinicalSectionQuestionGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.ClinicalSectionQuestionGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.ClinicalSectionQuestionGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        #region Section

        /// <summary>
        /// Loads the Clinical Section.
        /// </summary>
        /// <param name="SectionId">The Section identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadClinicalSection(long sectionID, string shortName, string description, string sectionTitle, string sectionTypeID, string specialityId, string isActive, Int64 PageNumber, Int64 RowsPage)
        {
            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (shortName == "")
                    shortName = null;
                if (description == "")
                    description = null;
                if (sectionTitle == "")
                    sectionTitle = null;
                if (sectionTypeID == "")
                    sectionTypeID = null;
                if (specialityId == "")
                    specialityId = null;
                if (isActive == "")
                    isActive = null;

                dbManager.Open();
                dbManager.CreateParameters(10);
                if (sectionID <= 0)
                    dbManager.AddParameters(0, PARM_SECTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SECTION_ID, sectionID);

                dbManager.AddParameters(1, PARM_SHORT_NAME, shortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, description);
                dbManager.AddParameters(3, PARM_SECTION_TITLE, sectionTitle);
                dbManager.AddParameters(4, PARM_SECTION_TYPE_ID, sectionTypeID);
                dbManager.AddParameters(5, PARM_SPECIALITY_ID, specialityId);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, isActive);

                if (PageNumber <= 0) { dbManager.AddParameters(7, PARM_PAGE_NUMBER, null); } else { dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPage <= 0) { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPage); } else { dbManager.AddParameters(8, PARM_ROWS_PAGE, RowsPage); }

                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.ClinicalSection.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECTION_SELECT, ds, ds.ClinicalSection.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::LoadClinicalSection", PROC_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Clinical Section.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertSections(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECTION_INSERT, ds, ds.ClinicalSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::InsertSection", PROC_SECTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Clinical Section
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateSection(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SECTION_UPDATE, ds, ds.ClinicalSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::UpdateClinicalSection", PROC_SECTION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Section.
        /// </summary>
        /// <param name="SectionId">The Section identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteSection(string SectionID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SECTION_ID, SectionID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::DeleteSection", PROC_SECTION_DELETE, ex);
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

        #region Section Question Group CRUD

        /// <summary>
        /// Delete the Question Group from the section
        /// </summary>
        /// <param name="QuesGroupQuestionId"></param>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public string DeleteSectionQuestionGroup(Int64 SectionQuestionGroupId, Int64 SectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SECTION_QUESTIONGROUP_ID, SectionQuestionGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SECTION_QUESTIONGROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::DeleteSectionQuestionGroup", PROC_SECTION_QUESTIONGROUP_DELETE, ex);
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
        /// Load the Section Question Group.
        /// </summary>
        /// <param name="QuesGroupQuestionId"></param>
        /// <param name="QuestionGroupID"></param>
        /// <returns></returns>
        public DSTemplateBuilder LoadSectionQuestionGroup(long SectionQuestionGroupId, long SectionId)
        {

            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (SectionQuestionGroupId <= 0)
                    dbManager.AddParameters(0, PARM_SECTION_QUESTIONGROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SECTION_QUESTIONGROUP_ID, SectionQuestionGroupId);

                if (SectionId <= 0) { dbManager.AddParameters(1, PARM_SECTION_ID, null); } else { dbManager.AddParameters(1, PARM_SECTION_ID, SectionId); }


                // dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ClinicalQuestionGroupQuestion.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECTION_QUESTIONGROUP_SELECT, ds, ds.ClinicalSectionQuestionGroup.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::LoadSectionQuestionGroup", PROC_SECTION_QUESTIONGROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Section QuestionGroup
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertSectionQuestionGroup(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersSectionQuestionGroup(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECTION_QUESTIONGROUP_INSERT, ds, ds.ClinicalSectionQuestionGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::InsertSectionQuestionGroup", PROC_SECTION_QUESTIONGROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Section QuestionGroup
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateSectionQuestionGroup(ref DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersSectionQuestionGroup(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SECTION_QUESTIONGROUP_UPDATE, ds, ds.ClinicalSectionQuestionGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::UpdateSectionQuestionGroup", PROC_SECTION_QUESTIONGROUP_UPDATE, ex);
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

        public DSTemplateBuilderLookups LookupSectionType()
        {
            DSTemplateBuilderLookups ds = new DSTemplateBuilderLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSTemplateBuilderLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECTION_TYPE_LOOKUP, ds, ds.SectionType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::LookupSectionType", PROC_SECTION_TYPE_LOOKUP, ex);
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
