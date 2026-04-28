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
    public class DALQuestions
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_QUESTION_INSERT = "Clinical.sp_QuestionInsert";
        private const string PROC_QUESTION_UPDATE = "Clinical.sp_QuestionUpdate";
        private const string PROC_QUESTION_DELETE = "Clinical.sp_QuestionDelete";
        private const string PROC_QUESTION_SELECT = "Clinical.sp_QuestionSelect";
        private const string PROC_QUESTION_TYPE_LOOKUP = "Clinical.sp_QuestionTypelookup";

        #endregion

        #region "Parameters"

        private const string PARM_QUESTION_ID = "@QuestionId";
        private const string PARM_QUESTION_TYPE_ID = "@QuestionTypeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_MANDATORY = "@IsMandatory";
        private const string PARM_DISPLAY_TEXT = "@DisplayText";
        private const string PARM_SENTENCE = "@Sentence";
        private const string PARM_SENTENCE_TRUE = "@SentenceTrue";
        private const string PARM_SENTENCE_FALSE = "@SentenceFalse";
        private const string PARM_BOOL_TRUE_DISPLAY_TEXT = "@BoolTrueDisplay";
        private const string PARM_BOOL_FALSE_DISPLAY_TEXT = "@BoolFalseDisplay";
        private const string PARM_MINIMUM_VALUE = "@MinValue";
        private const string PARM_MAXIMUM_VALUE = "@MaxValue";
        private const string PARM_DEFAULT_VALUE = "@DefaultValue";
        private const string PARM_NUMBER_PRECISION = "@NumberPrecesion";
        private const string PARM_TEXT_LENGTH = "@TextLength";
        private const string PARM_TEXT_CASE = "@TextCase";
        private const string PARM_IS_NEW_LINE = "@IsNewLine";
        private const string PARM_DATE_TIME_FORMAT = "@DateTimeFormat";
        private const string PARM_SENTENCE_TAG = "@SentenceTag";
        private const string PARM_IMAGE_STREAM = "@ImageStream";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_FIELD_TYPE = "@FieldType";
        private const string PARM_IS_AUTO_COMPLETE = "@IsAutoComplete";
        private const string PARM_QUESTION_TYPE = "@QuestionType";
        private const string PARM_FRACTION_FIELD_LABEL1 = "@FractionFieldLabel1";
        private const string PARM_FRACTION_FIELD_LABEL2 = "@FractionFieldLabel2";
        private const string PARM_SENTENCETAG_TRUE = "@SentenceTagTrue";
        private const string PARM_SENTENCETAG_FALSE = "@SentenceTagFalse";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_FILE_PATH = "@FilePath";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors
        public DALQuestions()
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
            dbManager.CreateParameters(34);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_QUESTION_ID, ds.ClinicalQuestions.QuestionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_QUESTION_ID, ds.ClinicalQuestions.QuestionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_QUESTION_TYPE_ID, ds.ClinicalQuestions.QuestionTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.ClinicalQuestions.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.ClinicalQuestions.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_MANDATORY, ds.ClinicalQuestions.IsMandatoryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_DISPLAY_TEXT, ds.ClinicalQuestions.DisplayTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SENTENCE, ds.ClinicalQuestions.SentenceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SENTENCE_TRUE, ds.ClinicalQuestions.SentenceTrueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_SENTENCE_FALSE, ds.ClinicalQuestions.SentenceFalseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_BOOL_TRUE_DISPLAY_TEXT, ds.ClinicalQuestions.BoolTrueDisplayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_BOOL_FALSE_DISPLAY_TEXT, ds.ClinicalQuestions.BoolFalseDisplayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MINIMUM_VALUE, ds.ClinicalQuestions.MinValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MAXIMUM_VALUE, ds.ClinicalQuestions.MaxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DEFAULT_VALUE, ds.ClinicalQuestions.DefaultValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_NUMBER_PRECISION, ds.ClinicalQuestions.NumberPrecesionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_TEXT_LENGTH, ds.ClinicalQuestions.TextLengthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_TEXT_CASE, ds.ClinicalQuestions.TextCaseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_IS_NEW_LINE, ds.ClinicalQuestions.IsNewLineColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(18, PARM_DATE_TIME_FORMAT, ds.ClinicalQuestions.DateTimeFormatColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_SENTENCE_TAG, ds.ClinicalQuestions.SentenceTagColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_IMAGE_STREAM, ds.ClinicalQuestions.ImageStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(21, PARM_IS_ACTIVE, ds.ClinicalQuestions.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(22, PARM_CREATED_BY, ds.ClinicalQuestions.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CREATED_ON, ds.ClinicalQuestions.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_MODIFIED_BY, ds.ClinicalQuestions.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_MODIFIED_ON, ds.ClinicalQuestions.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_FIELD_TYPE, ds.ClinicalQuestions.FieldTypeColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARM_IS_AUTO_COMPLETE, ds.ClinicalQuestions.IsAutoCompleteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_FRACTION_FIELD_LABEL1, ds.ClinicalQuestions.FractionFieldLabel1Column.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_FRACTION_FIELD_LABEL2, ds.ClinicalQuestions.FractionFieldLabel2Column.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_SENTENCETAG_TRUE, ds.ClinicalQuestions.SentenceTagTrueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_SENTENCETAG_FALSE, ds.ClinicalQuestions.SentenceTagFalseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_FILE_TYPE, ds.ClinicalQuestions.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_FILE_PATH, ds.ClinicalQuestions.FilePathColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(28, PARM_QUESTION_TYPE, ds.ClinicalQuestions.QuestionTypeColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the Clinical Questions.
        /// </summary>
        /// <param name="QuestionId">The Question identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSTemplateBuilder LoadClinicalQuestion(long QuestionID, string ShortName, string Description, string IsActive, string questionTypeID, Int64 rpp, Int64 pageNo)
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
                if (questionTypeID == "")
                    questionTypeID = null;

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (QuestionID <= 0)
                    dbManager.AddParameters(0, PARM_QUESTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionID);

                dbManager.AddParameters(1, PARM_SHORT_NAME,ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE,IsActive);
                dbManager.AddParameters(4, PARM_QUESTION_TYPE_ID, questionTypeID);


                if (pageNo <= 0) { dbManager.AddParameters(5, PARM_PAGE_NUMBER, null); } else { dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNo); }
                if (rpp <= 0) { dbManager.AddParameters(6, PARM_ROWS_PAGE, null); } else { dbManager.AddParameters(6, PARM_ROWS_PAGE, rpp); }

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.ClinicalQuestions.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSTemplateBuilder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUESTION_SELECT, ds, ds.ClinicalQuestions.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::LoadClinicalQuestion", PROC_QUESTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the Clinical Question.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder InsertQuestions(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSTemplateBuilder)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_QUESTION_INSERT, ds, ds.ClinicalQuestions.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::InsertQuestion", PROC_QUESTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Clinical Question
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSTemplateBuilder UpdateQuestions(DSTemplateBuilder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSTemplateBuilder)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_QUESTION_UPDATE, ds, ds.ClinicalQuestions.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::UpdateClinicalQuestoins", PROC_QUESTION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Question.
        /// </summary>
        /// <param name="QuestionId">The Question identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteQuestion(string QuestionID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_QUESTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::DeleteQuestion", PROC_QUESTION_DELETE, ex);
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

        public DSTemplateBuilderLookups LookupQuestionType()
        {
            DSTemplateBuilderLookups ds = new DSTemplateBuilderLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
               ds = (DSTemplateBuilderLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_QUESTION_TYPE_LOOKUP, ds, ds.QuestionType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::LookupQuestionType", PROC_QUESTION_TYPE_LOOKUP, ex);
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
