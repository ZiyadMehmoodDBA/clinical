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
    public class DALQuestionGroup
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_QUESTIONGROUP_INSERT = "[Clinical].[sp_QuestionGroupInsert]";
        private const string PROC_QUESTIONGROUP_UPDATE = "[Clinical].[sp_QuestionGroupUpdate]";
        private const string PROC_QUESTIONGROUP_DELETE = "[Clinical].[sp_QuestionGroupDelete]";

        private const string PROC_QUESTIONGROUP_SELECT = "[Clinical].[sp_QuestionGroupSelect]";

        private const string  PROC_QUESTIONGROUPQUESTION_DELETE= "[Clinical].[sp_QuestionGroupQuestionDelete]";
        private const string  PROC_QUESTIONGROUPQUESTION_INSERT= "[Clinical].[sp_QuestionGroupQuestionInsert]";
        private const string  PROC_QUESTIONGROUPQUESTION_SELECT = "[Clinical].[sp_QuestionGroupQuestionSelect]";

        private const string PROC_QUESTIONGROUPQUESTION_UPDATE = "[Clinical].[sp_QuestionGroupQuestionUpdate]";

        private const string PROC_BODYSYSTEMSLOOKUP = "Patient.sp_BodySystemsLookup";

        #endregion

        #region "Parameters"
        private const string PARM_QUESGROUPQUESTION_ID = "@QuesGroupQuestionId";
        private const string PARM_QUESTION_ID = "@QuestionID";
        private const string PARM_QUESTIONGROUP_ID = "@QuestionGroupId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_STARTING_SENTENCE = "@StartingSentence";
        private const string PARM_ENDING_SENTENCE = "@EndingSentence";
        private const string PARM_COMMENT = "@Comment";
        private const string PARM_SORTORDER = "@SortOrder";
        private const string PARM_DEFALTTEXT = "@DefaultText";
        private const string PARM_SPECIALITYID = "@SpecialityID";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_BODYSYSTEMID = "@BodySystemId";
        private const string PARM_HTMLTEMPLATECOLUMN = "@HTMLTemplate";
        private const string PARM_CANVAS = "@Canvas";
        
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        //        private const string PARM_LETTERFIELD_ID = "@LetterFieldId";

        #endregion

        #region Constructors
        public DALQuestionGroup()
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
            dbManager.CreateParameters(17);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUESTIONGROUP_ID, ds.ClinicalQuestionGroup.QuestionGroupIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUESTIONGROUP_ID, ds.ClinicalQuestionGroup.QuestionGroupIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.ClinicalQuestionGroup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ClinicalQuestionGroup.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_STARTING_SENTENCE, ds.ClinicalQuestionGroup.StartingSentenceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ENDING_SENTENCE, ds.ClinicalQuestionGroup.EndingSentenceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_COMMENT, ds.ClinicalQuestionGroup.CommentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SORTORDER, ds.ClinicalQuestionGroup.SortOrderColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(7, PARM_DEFALTTEXT, ds.ClinicalQuestionGroup.DefaltTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SPECIALITYID, ds.ClinicalQuestionGroup.SpecialityIDColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ClinicalQuestionGroup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.ClinicalQuestionGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.ClinicalQuestionGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.ClinicalQuestionGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.ClinicalQuestionGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_BODYSYSTEMID, ds.ClinicalQuestionGroup.BodySystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_CANVAS, ds.ClinicalQuestionGroup.CanvasColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_HTMLTEMPLATECOLUMN, ds.ClinicalQuestionGroup.HTMLTemplateColumn.ColumnName, DbType.String);
        }

        private void CreateParametersQuestionGroupQuestion(IDBManager dbManager, DSTemplateBuilder ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUESGROUPQUESTION_ID, ds.ClinicalQuestionGroupQuestion.QuesGroupQuestionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUESGROUPQUESTION_ID, ds.ClinicalQuestionGroupQuestion.QuesGroupQuestionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_QUESTION_ID, ds.ClinicalQuestionGroupQuestion.QuestionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_QUESTIONGROUP_ID, ds.ClinicalQuestionGroupQuestion.QuestionGroupIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_CREATED_BY, ds.ClinicalQuestionGroupQuestion.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_ON, ds.ClinicalQuestionGroupQuestion.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.ClinicalQuestionGroupQuestion.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIED_ON, ds.ClinicalQuestionGroupQuestion.ModifiedOnColumn.ColumnName, DbType.DateTime);
            
        }
        
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the QuestionGroup.
        /// </summary>
        /// <param name="QuestionGroupId">The QuestionGroup identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadQuestionGroup(long QuestionGroupID, Int64 PageNumber, Int64 RowsPage, String ShortName, String Description, Int64 SpecialityID, Int64 BodySystemId, string IsActive)
        {

            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (QuestionGroupID <= 0)
                    dbManager.AddParameters(0, PARM_QUESTIONGROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUESTIONGROUP_ID, QuestionGroupID);

                dbManager.AddParameters(1, PARM_SHORT_NAME,ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION,Description);
                if (SpecialityID <= 0)
                {
                    dbManager.AddParameters(3, PARM_SPECIALITYID, null);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_SPECIALITYID, SpecialityID);
                }
                if (BodySystemId <= 0)
                {
                    dbManager.AddParameters(4, PARM_BODYSYSTEMID, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_BODYSYSTEMID, BodySystemId);
                }               
                dbManager.AddParameters(5, PARM_IS_ACTIVE,string.IsNullOrEmpty(IsActive)?null:IsActive);
                
                if (PageNumber <= 0) { dbManager.AddParameters(6, PARM_PAGE_NUMBER, null); } else { dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPage <= 0) { dbManager.AddParameters(7, PARM_ROWS_PAGE, null); } else { dbManager.AddParameters(7, PARM_ROWS_PAGE, RowsPage); }
             
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.ClinicalQuestionGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUP_SELECT, ds, ds.ClinicalQuestionGroup.TableName);
             
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::LoadQuestionGroup", PROC_QUESTIONGROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the QuestionGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertQuestionGroup(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUP_INSERT, ds, ds.ClinicalQuestionGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::InsertQuestionGroup", PROC_QUESTIONGROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the QuestionGroup
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateQuestionGroup(ref DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUP_UPDATE, ds, ds.ClinicalQuestionGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::UpdateQuestionGroup", PROC_QUESTIONGROUP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Deletes the QuestionGroup.
        /// </summary>
        /// <param name="QuestionGroupId">The QuestionGroup identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteQuestionGroup(string QuestionGroupID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESTIONGROUP_ID, QuestionGroupID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUESTIONGROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::DeleteQuestionGroup", PROC_QUESTIONGROUP_DELETE, ex);
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

        #region QuestionGroup Question CRUD

        /// <summary>
        /// Deletes the Question from QuestionGroup.
        /// </summary>
        /// <param name="QuestionGroupId">The QuestionGroup identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteQuestionGroupQuestion(Int64 QuesGroupQuestionId, Int64 QuestionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESGROUPQUESTION_ID, QuesGroupQuestionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUESTIONGROUPQUESTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::DeleteQuestionGroupQuestion", PROC_QUESTIONGROUPQUESTION_DELETE, ex);
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
        /// Loads the QuestionGroup Questions.
        /// </summary>
        /// <param name="QuestionGroupId">The QuestionGroup identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadQuestionGroupQuestion(long QuesGroupQuestionId, long QuestionGroupID)
        {

            DSTemplateBuilder ds = new DSTemplateBuilder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (QuesGroupQuestionId <= 0)
                    dbManager.AddParameters(0, PARM_QUESGROUPQUESTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUESGROUPQUESTION_ID, QuesGroupQuestionId);

                if (QuestionGroupID <= 0) { dbManager.AddParameters(1, PARM_QUESTIONGROUP_ID, null); } else { dbManager.AddParameters(1, PARM_QUESTIONGROUP_ID, QuestionGroupID); }
                

               // dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ClinicalQuestionGroupQuestion.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUPQUESTION_SELECT, ds, ds.ClinicalQuestionGroupQuestion.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::LoadQuestionGroupQuestion", PROC_QUESTIONGROUPQUESTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the QuestionGroup Question
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertQuestionGroupQuestion(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersQuestionGroupQuestion(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUPQUESTION_INSERT, ds, ds.ClinicalQuestionGroupQuestion.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::InsertQuestionGroupQuestion", PROC_QUESTIONGROUPQUESTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the QuestionGroup Question
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateQuestionGroupQuestion(ref DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersQuestionGroupQuestion(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUESTIONGROUPQUESTION_UPDATE, ds, ds.ClinicalQuestionGroupQuestion.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestionGroup::UpdateQuestionGroupQuestion", PROC_QUESTIONGROUPQUESTION_UPDATE, ex);
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

        public DSTemplateBuilderLookups LookupBodySystem()
        {
            DSTemplateBuilderLookups ds = new DSTemplateBuilderLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSTemplateBuilderLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BODYSYSTEMSLOOKUP, ds, ds.BodySystems.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::LookupQuestionType", PROC_BODYSYSTEMSLOOKUP, ex);
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
