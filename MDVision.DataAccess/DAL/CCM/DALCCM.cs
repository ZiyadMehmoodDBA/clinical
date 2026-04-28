using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using EDIParser;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Common.Logging;
using MDVision.Model.Patient;
using System.Data.SqlClient;
using MDVision.Model.CCM;
using MDVision.Common.Utilities;
using MDVision.Model.Lookups;

namespace MDVision.DataAccess.DAL.CCM
{
    // ReSharper disable once InconsistentNaming
    public class DALCCM
    {


        #region "Stored Procedure Names"
        private const string PROC_CCM_ENROLLMENT_INFO_SELECT = "CCM.sp_EnrollmentInfoSelect";
        private const string PROC_CCM_TEMPLATE_SELECT = "ccm.sp_TemplatesSelect";
        private const string PROC_CCM_TEMPLATE_INSERT = "ccm.sp_TemplatesInsert";
        private const string PROC_CCM_TEMPLATE_UPDATE = "ccm.sp_TemplatesUpdate";
        private const string PROC_CCM_TEMPLATE_DELETE = "ccm.sp_TemplatesDelete";
        private const string PROC_CCM_TEMPALTE_ACTIVE_INACTIVE = "CCM.TEMPLATEACTIVEINACTIVE";
        private const string PROC_CCMHealthRiskAssessment_LOOKUP = "ccm.sp_HealthRiskAssessmentLookup";

        private const string PROC_CCM_TEMPLATE_FILL = "ccm.sp_TemplatesFill";
        private const string PROC_CCM_QUESTION_INSERT = "ccm.sp_QuestionsInsert";
        private const string PROC_CCM_QUESTION_SELECT = "ccm.sp_QuestionsSelect";
        private const string PROC_CCM_QUESTION_UPDATE = "ccm.sp_QuestionsUpdate";
        private const string PROC_CCM_QUESTION_DELETE = "ccm.sp_QuestionsDelete";
        private const string PROC_CCM_SECTION_INSERT = "ccm.sp_SectionsInsert";
        private const string PROC_CCM_SECTION_SELECT = "ccm.sp_SectionsSelect";
        private const string PROC_CCM_SECTION_UPDATE = "ccm.sp_SectionsUpdate";
        private const string PROC_CCM_SECTION_DELETE = "ccm.sp_SectionsDelete";

        private const string PROC_ICD_GROUP_LOOKUP = "ccm.sp_ICDGroupsLookup";

        private const string PROC_CCMTEMPLATE_SELECT = "ccm.sp_TemplatesSelect";
        private const string PROC_CCMICDGroups_SELECT = "ccm.sp_ICDGroupsSelect";
        private const string PROC_CCMCareTeams_INSERT = "ccm.sp_CareTeamsInsert";
        private const string PROC_CCMCareTeams_SELECT = "ccm.sp_CareTeamsSelect";
        private const string PROC_CCMCareTeams_LOOKUP = "ccm.sp_CareTeamsLookup";
        private const string PROC_CCMCareTeams_UPDATE = "ccm.sp_CareTeamsUpdate";
        private const string PROC_CCMCareTeams_DELETE = "CCM.SP_CareTeamsDelete";
        private const string PROC_CCMCARETEAMS_ACTIVEINACTIVE = "CCM.CARETEAMSACTIVEINACTIVE";

        private const string PROC_CCMCareManager_INSERT = "ccm.sp_CareManagerInsert";
        private const string PROC_CCMCareCoordinator_INSERT = "ccm.sp_CareCoordinatorInsert";
        private const string PROC_CCMCareGiver_INSERT = "ccm.sp_CareGiverInsert";





        private const string PROC_ICDGROUPS_INSERT = "CCM.ICDGROUPSINSERT";
        private const string PROC_ICDGROUPS_UPDATE = "CCM.ICDGROUPSUPDATE";
        private const string PROC_ICDGROUPSMAP_INSERT = "CCM.ICDGROUPSMAPINSERT";
        private const string PROC_ICDGROUPSMAP_DELETE = "CCM.ICDGROUPSMAPDELETE";
        private const string PROC_ICDGROUPS_DELETE = "CCM.ICDGROUPSDELETE";
        private const string PROC_ICDGROUPS_ACTIVEINACTIVE = "CCM.ICDGroupsActiveInActive";

        private const string PROC_CCMICDGroupsDetail_SELECT = "CCM.SP_ICDGROUPSDETAILSELECT";


        private const string PROC_CCM_SECTION_QUESTIONS_SELECT = "ccm.sp_SectionQuestionsSelect";
        private const string PROC_CCM_SECTION_QUESTIONS_INSERT = "ccm.sp_SectionQuestionsInsert";
        private const string PROC_CCM_TEMPLATE_SECTIONS_SELECT = "ccm.sp_TemptSectionsSelect";
        private const string PROC_CCM_TEMPLATE_SECTIONS_INSERT = "ccm.sp_TemptSectionsInsert";
        private const string PROC_CCM_TEMPLATE_SECTIONS_UPDATE = "ccm.sp_TemptSectionsUpdate";
        private const string PROC_CCM_TEMPLATE_SECTIONS_DELETE = "ccm.ssp_TemptSectionsDelete";
        private const string PROC_CCM_SUB_QUESTION_SELECT = "ccm.sp_SubQuestionsSelect";
        private const string PROC_CCM_QUESTION_ORDER_UPDATE = "ccm.sp_QuestionsOrderUpdate";

        private const string PROC_CCM_TASKREASON_LOOKUP = "ccm.sp_ReasonLookup";

        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_STATUS = "@Status";
        private const string PARM_TEMPLATE_ID = "@TemplateId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_ICDGROUP_NAMES = "@ICDGroupNames";
        private const string PARM_ICDGROUP_IDS = "@ICDGroupIds";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_QUESTION_ID = "@QuestionId";
        private const string PARM_DESCREPTION = "@Description";
        private const string PARM_QUESTION_HTML = "@QuestionHTML";
        private const string PARM_PARENT_QUEST_ID = "@ParentQuestId";
        private const string PARM_SECTION_ID = "@SectionId";

        private const string PARM_ICDGroupMapId = "@ICDGroupMapId";
        private const string PARM_ICDCodeId = "@ICDCodeId";

        private const string PARM_ICDGroupId = "@ICDGroupId";
        private const string PARM_CareTeamId = "@CareTeamId";
        private const string PARM_ICDGroupName = "@ICDGroupName";
        private const string PARM_ICDGroupDescription = "@ICDGroupDescription";
        private const string PARM_SEC_QUES_ID = "@SecQuestId";
        private const string PARM_TEMP_SEC_ID = "@TemptSectionId";
        private const string PARM_TEM_LOOKUP_ID = "@TempLookupId";
        private const string PARM_TEM_LOOKUP_NAME = "@TempLookupName";
        private const string PARM_QUESTION_IDS = "@QuestionIds";

        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_REASON_ID = "@ReasonId";

        #endregion

        #region Constructors
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DALCCM()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        //-----------------------------------------------------------------------------------------------------
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new Container();
        }


        #endregion

        #region "Support Functions"
        private void CreateParametersForTemplate(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);
            if (IsInsert)
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, ds.Templates.TemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, ds.Templates.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Templates.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCREPTION, ds.Templates.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Templates.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.Templates.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.Templates.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.Templates.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.Templates.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ICDGROUP_IDS, ds.Templates.ICDGroupIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_TEM_LOOKUP_ID, ds.Templates.TempLookupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        private void CreateParametersForSections(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(2);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SECTION_ID, ds.Sections.SectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SECTION_ID, ds.Sections.SectionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Sections.ShortNameColumn.ColumnName, DbType.String);
        }
        private void CreateParametersForQuestions(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);
            if (IsInsert)
                dbManager.AddParameters(0, PARM_QUESTION_ID, ds.Questions.QuestionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUESTION_ID, ds.Questions.QuestionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_DESCREPTION, ds.Questions.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_QUESTION_HTML, ds.Questions.QuestionHTMLColumn.ColumnName, DbType.String);
            if (string.IsNullOrEmpty(ds.Questions.ParentQuestIdColumn.ColumnName))
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, null);
            else
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, ds.Questions.ParentQuestIdColumn.ColumnName, DbType.Int64);
        }
        private void CreateUpdateParametersForQuestions(IDBManager dbManager, DSCCM ds)
        {
            dbManager.CreateParameters(4);
            dbManager.AddParameters(0, PARM_QUESTION_ID, ds.Questions.QuestionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_DESCREPTION, ds.Questions.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_QUESTION_HTML, ds.Questions.QuestionHTMLColumn.ColumnName, DbType.String);
            if (string.IsNullOrEmpty(ds.Questions.ParentQuestIdColumn.ColumnName))
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, null);
            else
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, ds.Questions.ParentQuestIdColumn.ColumnName, DbType.Int64);
        }


        private void CreateInsertParametersForTemplateSections(IDBManager dbManager, DSCCM ds)
        {
            dbManager.CreateParameters(3);
            dbManager.AddParameters(0, PARM_TEMP_SEC_ID, ds.TemplateSections.TemptSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_SECTION_ID, ds.TemplateSections.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_TEMPLATE_ID, ds.TemplateSections.TemplateIdColumn.ColumnName, DbType.Int64);
        }
        private void CreateInsertParametersForSectionQuestions(IDBManager dbManager, DSCCM ds)
        {
            dbManager.CreateParameters(3);
            dbManager.AddParameters(0, PARM_SEC_QUES_ID, ds.SectionQuestions.SecQuestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_SECTION_ID, ds.SectionQuestions.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_QUESTION_ID, ds.SectionQuestions.QuestionIdColumn.ColumnName, DbType.Int64);
        }
        private void CreateInsertParametersForSubQuestions(IDBManager dbManager, DSCCM ds)
        {
            dbManager.CreateParameters(4);

            dbManager.AddParameters(0, PARM_QUESTION_ID, ds.SubQuestions.QuestionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_DESCREPTION, ds.SubQuestions.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_QUESTION_HTML, ds.Questions.QuestionHTMLColumn.ColumnName, DbType.String);
            if (string.IsNullOrEmpty(ds.SubQuestions.ParentQuestIdColumn.ColumnName))
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, null);
            else
                dbManager.AddParameters(3, PARM_PARENT_QUEST_ID, ds.SubQuestions.ParentQuestIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region CCM Admin Side

        #region "CRUD CCM Templates"

        /// <summary>
        /// LoadCCMTemplate
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <param name="TempLookupName"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMTemplate(int TemplateId, string TempLookupName, string ShortName, string Description, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (TemplateId == 0)
                {
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateId);
                }
                dbManager.AddParameters(1, PARM_TEM_LOOKUP_NAME, TempLookupName);
                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(2, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_SHORT_NAME, ShortName);
                if (string.IsNullOrEmpty(Description))
                    dbManager.AddParameters(3, PARM_DESCREPTION, null);
                else
                    dbManager.AddParameters(3, PARM_DESCREPTION, Description);
                dbManager.AddParameters(4, "@IsActive", IsActive);
                dbManager.AddParameters(5, "@PageNumber", PageNumber);
                dbManager.AddParameters(6, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(7, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMTEMPLATE_SELECT, ds, ds.Templates.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMTemplate", PROC_CCMTEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// DeleteTemplate
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        public string DeleteTemplate(string TemplateId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteTemplate", PROC_CCM_TEMPLATE_DELETE, ex);
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

        public DSCCM FillTemplate(long TemplateId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM ds = new DSCCM();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_TEMPLATE_ID, TemplateId);

                var tableNames = new List<string>
                {
                    ds.Templates.TableName,
                    ds.Sections.TableName,
                    ds.SectionQuestions.TableName,
                    ds.Questions.TableName,
                    ds.SubQuestions.TableName,
                };

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_TEMPLATE_FILL, ds, tableNames);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::FillTemplate", PROC_CCM_TEMPLATE_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// LookupICDGroup
        /// </summary>
        /// <returns></returns>
        public List<ICDGroupLookupModel> LookupICDGroup()
        {
            //  List<ICDGroupLookupModel> listModel = new List<ICDGroupLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //List<SqlParameter> parameters = new List<SqlParameter>();

                //parameters.Add(new SqlParameter())

                return dbManager.ExecuteReaders<ICDGroupLookupModel>(PROC_ICD_GROUP_LOOKUP);

                //dbManager.Open();
                //  SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ICD_GROUP_LOOKUP);
                //ICDGroupLookupModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new ICDGroupLookupModel();
                //    modelFill.ICDGroupId = Convert.ToInt64(reader["ICDGroupId"]);
                //    modelFill.ShortName = MDVUtility.CheckStringNull(reader["ShortName"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LookupICDGroup", PROC_ICD_GROUP_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                //  dbManager.Dispose();
            }
        }

        /// <summary>
        /// ActiveInActiveTemplate
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public string ActiveInActiveTemplate(string templateId, long isActive)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, (string.IsNullOrEmpty(templateId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(templateId)));
                dbManager.AddParameters(1, PARM_IS_ACTIVE, (isActive));
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_TEMPALTE_ACTIVE_INACTIVE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::ActiveInActiveTemplate", PROC_CCM_TEMPALTE_ACTIVE_INACTIVE, ex);
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

        public DSCCM InsertTemplate(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateParametersForTemplate(dbManager, dsCCM, true);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_TEMPLATE_INSERT, dsCCM, dsCCM.Templates.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM UpdateTemplate(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateParametersForTemplate(dbManager, dsCCM, false);

            dsCCM = (DSCCM)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CCM_TEMPLATE_UPDATE, dsCCM, dsCCM.Templates.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        #endregion

        #region "CRUD CCM Section"
        /// <summary>
        /// DeleteSection
        /// </summary>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        public string DeleteSection(string SectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SECTION_ID, SectionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_SECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteSection", PROC_CCM_SECTION_DELETE, ex);
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
        public DSCCM UpdateSections(DSCCM dsCCM, IDBManager dbManager)
        {

            this.CreateParametersForSections(dbManager, dsCCM, false);
            dsCCM = (DSCCM)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CCM_SECTION_UPDATE, dsCCM, dsCCM.Sections.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM InsertSections(DSCCM dsCCM, IDBManager dbManager)
        {

            this.CreateParametersForSections(dbManager, dsCCM, true);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_SECTION_INSERT, dsCCM, dsCCM.Sections.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        #endregion

        #region "CRUD CCM Question"

        /// <summary>
        /// DeleteQuestion
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public string DeleteQuestion(string QuestionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_QUESTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteTemplate", PROC_CCM_TEMPLATE_DELETE, ex);
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
        public DSCCM UpdateQuestion(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateParametersForQuestions(dbManager, dsCCM, false);
            dsCCM = (DSCCM)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CCM_QUESTION_UPDATE, dsCCM, dsCCM.Questions.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM InsertQuestion(DSCCM dsCCM, IDBManager dbManager)
        {

            this.CreateParametersForQuestions(dbManager, dsCCM, true);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_QUESTION_INSERT, dsCCM, dsCCM.Questions.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        #endregion

        #region "CRUD CCM ICD Groups"

        /// <summary>
        /// CreateParameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDGroupId, ds.ICDGroups.ICDGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDGroupId, ds.ICDGroups.ICDGroupIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICDGroupName, ds.ICDGroups.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ICDGroupDescription, ds.ICDGroups.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ICDGroups.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ICDGroups.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ICDGroups.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ICDGroups.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ICDGroups.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        /// <summary>
        /// CreateParametersICDGroupMap
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersICDGroupMap(IDBManager dbManager, DSCCM ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(3);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDGroupMapId, ds.ICDGroupMap.ICDGroupMapIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDGroupMapId, ds.ICDGroupMap.ICDGroupMapIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICDCodeId, ds.ICDGroupMap.ICDCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICDGroupId, ds.ICDGroupMap.ICDGroupIdColumn.ColumnName, DbType.Int64);
        }

        /// <summary>
        /// LoadCCMICDGroups
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="ShortName"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMICDGroups(long ICDGroupId, string ShortName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (ICDGroupId == 0)
                {
                    dbManager.AddParameters(0, "@ICDGroupId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@ICDGroupId", ICDGroupId);
                }
                dbManager.AddParameters(1, "@ShortName", ShortName);
                dbManager.AddParameters(2, "@IsActive", IsActive);
                dbManager.AddParameters(3, "@PageNumber", PageNumber);
                dbManager.AddParameters(4, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(5, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMICDGroups_SELECT, ds, ds.ICDGroups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMICDGroups", PROC_CCMICDGroups_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// LoadCCMICDGroupsDetail
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMICDGroupsDetail(long ICDGroupId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ICDGroupId == 0)
                    dbManager.AddParameters(0, "@ICDGroupId", null);
                else
                    dbManager.AddParameters(0, "@ICDGroupId", ICDGroupId);

                dbManager.AddParameters(1, "@PageNumber", PageNumber);
                dbManager.AddParameters(2, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(3, "@RecordCount", ds.ICDGroupsDetailSelect.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMICDGroupsDetail_SELECT, ds, ds.ICDGroupsDetailSelect.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMICDGroupsDetail", PROC_CCMICDGroupsDetail_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// InsertICDGroup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM InsertICDGroup(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPS_INSERT, ds, ds.ICDGroups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertICDGroup", PROC_ICDGROUPS_INSERT, ex);
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
        /// UpdateICDGroup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM UpdateICDGroup(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, false);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPS_UPDATE, ds, ds.ICDGroups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::UpdateICDGroup", PROC_ICDGROUPS_UPDATE, ex);
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
        /// ActiveInActiveICDGroupICDGroup
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="isactive"></param>
        /// <returns></returns>
        public string ActiveInActiveICDGroupICDGroup(string ICDGroupId, long isactive)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ICDGroupId, (string.IsNullOrEmpty(ICDGroupId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(ICDGroupId)));
                dbManager.AddParameters(1, PARM_IS_ACTIVE, (isactive));
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPS_ACTIVEINACTIVE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::ActiveInActiveICDGroupICDGroup", PROC_ICDGROUPS_ACTIVEINACTIVE, ex);
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
        /// DeleteICDGroup
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <returns></returns>
        public string DeleteICDGroup(string ICDGroupId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ICDGroupId, (string.IsNullOrEmpty(ICDGroupId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(ICDGroupId)));
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPS_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteICDGroup", PROC_ICDGROUPS_DELETE, ex);
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
        /// InsertICDGroupMap
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM InsertICDGroupMap(ref DSCCM ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersICDGroupMap(dbManager, ds, true);
                ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDGROUPSMAP_INSERT, ds, ds.ICDGroupMap.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertICDGroupMap", PROC_ICDGROUPSMAP_INSERT, ex);
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
        /// DeleteICDGroupMap
        /// </summary>
        /// <param name="ICDGroupMapId"></param>
        /// <returns></returns>
        public string DeleteICDGroupMap(string ICDGroupMapId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ICDGroupMapId, (string.IsNullOrEmpty(ICDGroupMapId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(ICDGroupMapId)));
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPSMAP_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteICDGroupMap", PROC_ICDGROUPSMAP_DELETE, ex);
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

        #region "CRUD CCM Care Teams"

        /// <summary>
        /// LoadCCMCareTeams
        /// </summary>
        /// <param name="CareTeamId"></param>
        /// <param name="ShortName"></param>
        /// <param name="ProviderName"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSCCM LoadCCMCareTeams(long CareTeamId, string ShortName, string ProviderName, bool IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCCM ds = new DSCCM();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (CareTeamId == 0)
                {
                    dbManager.AddParameters(0, "@CareTeamId", null);
                }
                else
                {
                    dbManager.AddParameters(0, "@CareTeamId", CareTeamId);
                }
                dbManager.AddParameters(1, "@ShortName", ShortName);
                dbManager.AddParameters(2, "@ProviderName", ProviderName);
                dbManager.AddParameters(3, "@IsActive", IsActive);
                dbManager.AddParameters(4, "@PageNumber", PageNumber);
                dbManager.AddParameters(5, "@RowspPage", RowsPerPage);
                dbManager.AddParameters(6, "@RecordCount", ds.Templates.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMCareTeams_SELECT, ds, ds.CareTeams.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMCareTeams", PROC_CCMCareTeams_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// SaveCCMCareTeam
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM SaveCCMCareTeam(ref DSCCM ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    SaveCCMCareTeamConcrete(ref ds,dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareTeamsInsert", PROC_CCMCareTeams_INSERT, ex);
                    //throw ex;
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
            else
            {
                try
                {
                    SaveCCMCareTeamConcrete(ref ds, dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareTeamsInsert", PROC_CCMCareTeams_INSERT, ex);
                    //throw ex;
                    string[] str = ex.Message.Split('|');
                    if (str.Length > 1)
                        throw new Exception(str[1].ToString());
                    else
                        throw new Exception(ex.Message);
                }
            }
        }


        private DSCCM SaveCCMCareTeamConcrete(ref DSCCM ds, IDBManager dbManager = null)
        {
            dbManager.CreateParameters(10);
            dbManager.AddParameters(0, "@CareTeamId", ds.CareTeams.CareTeamIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, "@ShortName", ds.CareTeams.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, "@Description", ds.CareTeams.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, "@PCPId", ds.CareTeams.PCPIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(4, "@CareManagerId", ds.CareTeams.CareManagerIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(5, "@CareCoordinatorId", ds.CareTeams.CareCoordinatorIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(6, "@CareGiverId", ds.CareTeams.CareGiverIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, "@ProviderId", ds.CareTeams.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, "@IsActive", ds.CareTeams.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, "@CreatedBy", ds.CareTeams.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, "@CreatedOn", ds.CareTeams.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, "@ModifiedBy", ds.CareTeams.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, "@ModifiedOn", ds.CareTeams.ModifiedOnColumn.ColumnName, DbType.DateTime);
            ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareTeams_INSERT, ds, ds.CareTeams.TableName);
            return ds;
        }
        /// <summary>
        /// UpdateCCMCareTeam
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSCCM UpdateCCMCareTeam(ref DSCCM ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                 dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    UpdateCCMCareTeamConcrete(ref ds,dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareTeamsUpdate", PROC_CCMCareTeams_UPDATE, ex);
                    //throw ex;
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
            else
            {
                try
                {
                    UpdateCCMCareTeamConcrete(ref ds, dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareTeamsUpdate", PROC_CCMCareTeams_UPDATE, ex);
                    //throw ex;
                    string[] str = ex.Message.Split('|');
                    if (str.Length > 1)
                        throw new Exception(str[1].ToString());
                    else
                        throw new Exception(ex.Message);
                }
            }
        }

        private DSCCM UpdateCCMCareTeamConcrete(ref DSCCM ds, IDBManager dbManager = null)
        {
            dbManager.CreateParameters(8);
            dbManager.AddParameters(0, "@CareTeamId", ds.CareTeams.CareTeamIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, "@ShortName", ds.CareTeams.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, "@Description", ds.CareTeams.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, "@PCPId", ds.CareTeams.PCPIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(4, "@CareManagerId", ds.CareTeams.CareManagerIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(5, "@CareCoordinatorId", ds.CareTeams.CareCoordinatorIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(6, "@CareGiverId", ds.CareTeams.CareGiverIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, "@ProviderId", ds.CareTeams.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, "@IsActive", ds.CareTeams.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, "@ModifiedBy", ds.CareTeams.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, "@ModifiedOn", ds.CareTeams.ModifiedOnColumn.ColumnName, DbType.DateTime);
            ds = (DSCCM)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CCMCareTeams_UPDATE, ds, ds.CareTeams.TableName);
            return ds;
        }

        /// <summary>
        /// DeleteCCMCareTeam
        /// </summary>
        /// <param name="CareTeamId"></param>
        /// <returns></returns>
        public string DeleteCCMCareTeam(string CareTeamId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@CareTeamId", (string.IsNullOrEmpty(CareTeamId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(CareTeamId)));
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                var returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCMCareTeams_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::DeleteCareTeam", PROC_CCMCareTeams_DELETE, ex);
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
        #region "CRUD CCM CareManagers"
        public DSCCM SaveCCMCareManagers(ref DSCCM ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareManagerId", ds.CareManagers.CareManagerIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareManagers.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareManager_INSERT, ds, ds.CareManagers.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareManagerInsert", PROC_CCMCareManager_INSERT, ex);
                    //throw ex;
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
            else
            {
                try
                {
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareManagerId", ds.CareManagers.CareManagerIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareManagers.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareManager_INSERT, ds, ds.CareManagers.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareManagerInsert", PROC_CCMCareManager_INSERT, ex);
                    //throw ex;
                    string[] str = ex.Message.Split('|');
                    if (str.Length > 1)
                        throw new Exception(str[1].ToString());
                    else
                        throw new Exception(ex.Message);
                }
            }
        }
       
        #endregion

        #endregion
        #region "CRUD CCM CareCoordinators"
        public DSCCM SaveCCMCareCoordinators(ref DSCCM ds,IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareCoordinatorId", ds.CareCoordinators.CareCoordinatorIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareCoordinators.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareCoordinator_INSERT, ds, ds.CareCoordinators.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareCoordinatorInsert", PROC_CCMCareCoordinator_INSERT, ex);
                    //throw ex;
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
            else
            {
                try
                {
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareCoordinatorId", ds.CareCoordinators.CareCoordinatorIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareCoordinators.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareCoordinator_INSERT, ds, ds.CareCoordinators.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareCoordinatorInsert", PROC_CCMCareCoordinator_INSERT, ex);
                    //throw ex;
                    string[] str = ex.Message.Split('|');
                    if (str.Length > 1)
                        throw new Exception(str[1].ToString());
                    else
                        throw new Exception(ex.Message);
                }
            }
        }
     
        #endregion
        #region "CRUD CCM CareGivers"
        public DSCCM SaveCCMCareGivers(ref DSCCM ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    dbManager.Open();
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareGiverId", ds.CareGivers.CareGiverIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareGivers.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareGiver_INSERT, ds, ds.CareGivers.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareGiverInsert", PROC_CCMCareGiver_INSERT, ex);
                    //throw ex;
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
            else
            {
                try
                {
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, "@CareGiverId", ds.CareGivers.CareGiverIdColumn.ColumnName, DbType.Int64);
                    dbManager.AddParameters(1, "@CareTeamId", ds.CareGivers.CareTeamIdColumn.ColumnName, DbType.Int64);

                    ds = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCMCareGiver_INSERT, ds, ds.CareGivers.TableName);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALCCM:CareGiverInsert", PROC_CCMCareGiver_INSERT, ex);
                    //throw ex;
                    string[] str = ex.Message.Split('|');
                    if (str.Length > 1)
                        throw new Exception(str[1].ToString());
                    else
                        throw new Exception(ex.Message);
                }
            }
        }
  
        #endregion
        /// <summary>
        /// ActiveInActiveCareTeam
        /// </summary>
        /// <param name="CareTeamId"></param>
        /// <param name="isactive"></param>
        /// <returns></returns>
        public string ActiveInActiveCareTeam(string CareTeamId, long isactive)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_CareTeamId, (string.IsNullOrEmpty(CareTeamId) ? MDVUtility.ToLong("0") : MDVUtility.ToLong(CareTeamId)));
                dbManager.AddParameters(1, PARM_IS_ACTIVE, (isactive));
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCMCARETEAMS_ACTIVEINACTIVE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::ActiveInActiveCareTeam", PROC_CCMCARETEAMS_ACTIVEINACTIVE, ex);
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

        public DSCCM CCMCareTeamLookup()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSCCM ds = new DSCCM();
                dbManager.Open();
                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMCareTeams_LOOKUP, ds, ds.CareTeams.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM:CareTeamsLookup", PROC_CCMCareTeams_LOOKUP, ex);
                //throw ex;
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

        #region "CRUD CCM Questions"
        public DSCCM LoadSubQuestion(string QuestionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM ds = new DSCCM();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionId);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_SUB_QUESTION_SELECT, ds, ds.Questions.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadSubQuestion", PROC_CCM_SUB_QUESTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCM LoadSectionQuestions(string SectionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM ds = new DSCCM();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SECTION_ID, SectionId);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_SECTION_QUESTIONS_SELECT, ds, ds.SectionQuestions.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadSectionQuestions", PROC_CCM_SECTION_QUESTIONS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCM LoadSection(Int64 SectionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM ds = new DSCCM();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SECTION_ID, SectionId);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_SECTION_SELECT, ds, ds.Sections.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadSection", PROC_CCM_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCCM LoadQuestion(Int64 QuestionId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSCCM ds = new DSCCM();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionId);

                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_QUESTION_SELECT, ds, ds.Questions.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadQuestion", PROC_CCM_QUESTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// LoadCCMEnrollmentInfo
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="ProviderId"></param>
        /// <param name="InsurancePlanId"></param>
        /// <param name="UserId"></param>
        /// <param name="Status"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public List<CCMEnrollmentInfoModel> LoadCCMEnrollmentInfo(Int32 PatientId, Int32 ProviderId, Int32 InsurancePlanId, long UserId, string Status, Int32 PageNumber = 1, Int32 RowsPerPage = 1500)
        {
            //List<CCMEnrollmentInfoModel> CCMEnrollmentInfoList = new List<CCMEnrollmentInfoModel>();

            //SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                //  List<SqlParameter> parameters = new List<SqlParameter>();
                //  dbManager.Open();
                //dbManager.CreateParameters(5);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                //  parameters.Add(new SqlParameter(PARM_PATIENT_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(PARM_PROVIDER_ID, null);
                //parameters.Add(new SqlParameter(PARM_PROVIDER_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                //parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(PARM_INSURANCE_PLAN_ID, null);
                //parameters.Add(new SqlParameter(PARM_INSURANCE_PLAN_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_INSURANCE_PLAN_ID, InsurancePlanId);
                //parameters.Add(new SqlParameter(PARM_INSURANCE_PLAN_ID, InsurancePlanId));

                if (Status == "")
                    dbManager.AddParameters(PARM_STATUS, null);
                //parameters.Add(new SqlParameter(PARM_STATUS, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_STATUS, Status);
                //parameters.Add(new SqlParameter(PARM_STATUS, Status));

                dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                //parameters.Add(new SqlParameter(PARM_ENTITY_ID, MDVSession.Current.EntityId));

                return dbManager.ExecuteReaders<CCMEnrollmentInfoModel>(PROC_CCM_ENROLLMENT_INFO_SELECT);
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_ENROLLMENT_INFO_SELECT);

                //CCMEnrollmentInfoModel model = null;
                //while (reader.Read())
                //{
                //    model = new CCMEnrollmentInfoModel();
                //    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                //    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                //    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                //    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                //    model.Problems = !String.IsNullOrEmpty(reader["Problems"].ToString()) ? reader["Problems"].ToString() : "";
                //    model.InsuranceName = !String.IsNullOrEmpty(reader["InsuranceName"].ToString()) ? reader["InsuranceName"].ToString() : "";
                //    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";

                //    CCMEnrollmentInfoList.Add(model);
                //}


                //return CCMEnrollmentInfoList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::LoadCCMEnrollmentInfo", PROC_CCM_ENROLLMENT_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                //dbManager.Dispose();
            }
        }
        public DSCCM InsertUpdateQuestions(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateParametersForQuestions(dbManager, dsCCM, true);
            //  this.CreateUpdateParametersForQuestions(dbManager, dsCCM);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_QUESTION_INSERT, dsCCM, dsCCM.Questions.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM InsertTemplateSections(DSCCM dsCCM, IDBManager dbManager)
        {

            this.CreateInsertParametersForTemplateSections(dbManager, dsCCM);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_TEMPLATE_SECTIONS_INSERT, dsCCM, dsCCM.TemplateSections.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM InsertSectionQuestions(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateInsertParametersForSectionQuestions(dbManager, dsCCM);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_SECTION_QUESTIONS_INSERT, dsCCM, dsCCM.SectionQuestions.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM InsertSubQuestions(DSCCM dsCCM, IDBManager dbManager)
        {
            this.CreateInsertParametersForSubQuestions(dbManager, dsCCM);
            dsCCM = (DSCCM)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CCM_QUESTION_INSERT, dsCCM, dsCCM.SubQuestions.TableName);
            dsCCM.AcceptChanges();

            return dsCCM;
        }
        public DSCCM CCMHealthRiskAssessmentLookup(Int64 EnrollmentInfoId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSCCM ds = new DSCCM();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ENROLLMENT_INFO_ID, EnrollmentInfoId);
                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCMHealthRiskAssessment_LOOKUP, ds, ds.HRATemplateLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM:HealthRiskAssessmentsLookup", PROC_CCMCareTeams_LOOKUP, ex);
                //throw ex;
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

        public string UpdateCCMQuestionOrder(string QuestionIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESTION_IDS, QuestionIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_QUESTION_ORDER_UPDATE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::ActiveInActiveTemplate", PROC_CCM_QUESTION_ORDER_UPDATE, ex);
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

        public DSCCM TaskReasonLookup()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSCCM ds = new DSCCM();
                dbManager.Open();
                ds = (DSCCM)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CCM_TASKREASON_LOOKUP, ds, ds.Reasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM:TaskReasonLookup", PROC_CCM_TASKREASON_LOOKUP, ex);
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
    }
}


