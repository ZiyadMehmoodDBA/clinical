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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPogressNote
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PROGRESSNOTE_INSERT = "[Clinical].[sp_ProgressNoteInsert]";
        private const string PROC_PROGRESSNOTE_UPDATE = "[Clinical].[sp_ProgressNoteUpdate]";
        private const string PROC_PROGRESSNOTE_DELETE = "[Clinical].[sp_ProgressNoteDelete]";
        private const string PROC_PROGRESSNOTE_SELECT = "[Clinical].[sp_ProgressNoteSelect]";
        private const string PROC_PROGRESSNOTE_HTML_UPDATE = "[Clinical].[sp_ProgressNoteHTMLUpdate]";
        //private const string PROC_TEMPLATE_TYPE_LOOKUP = "[Clinical].[sp_TemplateTypeLookup]";

        private const string PROC_ANSWERSHEET_INSERT = "[Clinical].[sp_AnswersSheetInsert]";
        private const string PROC_ANSWERSHEET_UPDATE = "[Clinical].[sp_AnswersSheetUpdate]";
        private const string PROC_ANSWERSHEET_DELETE = "[Clinical].[sp_AnswersSheetDelete]";
        private const string PROC_SCHEDULE_BLOCKHOURS_LOOKUP = "Clinical.sp_ScheduleReasonsNotesLookupAutoComplete";
        //private const string PROC_PROGRESSNOTE_DELETE #endregion
        //private const string PROC_PROGRESSNOTE_SELECT 

        #endregion

        #region "Parameters"

        private const string PARM_PROGRESSNOTE_ID = "@ProgressNoteID";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_TEMPLATE_ID = "@TemplateId";

        private const string PARM_ANSWERSHEET_ID = "@AnswerSheetID";
        private const string PARM_SECTION_ID = "@SectionID";
        private const string PARM_QUESTIONGROUP_ID = "@QuestionGroupID";
        private const string PARM_QUESTION_ID = "@QuestionID";

        private const string PARM_ANSWER_VALUE = "@AnswerValue";
        private const string PARM_ANSWER_TEXT = "@AnswerText";
        private const string PARM_ANSWER_COMMENTS = "@AnswerComments";
        private const string PARM_ROW_ID = "@RowID";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_HTML_TEMPLATE = "@HTMLTemplate";

        private const string PARM_PREVIOUS_HTML = "@PreviousHTML";
        private const string PARM_CURRENT_HTML = "@CurrentHTML";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_NAME = "@Name";
        private const string PARM_IS_ACTIVE = "@IsActive";

        #endregion

        #region Constructors
        public DALPogressNote()
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
        private void createParameters(IDBManager dbManager, DSProgressNote ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, ds.ProgressNote.ProgressNoteIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, ds.ProgressNote.ProgressNoteIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VISIT_ID, ds.ProgressNote.VisitID_Column.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.ProgressNote.PatientIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_TEMPLATE_ID, ds.ProgressNote.TemplateIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ProgressNote.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ProgressNote.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ProgressNote.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ProgressNote.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(8, PARM_HTML_TEMPLATE, ds.ProgressNote.HTMLTemplateColumn.ColumnName, DbType.String);

            //dbManager.AddParameters(9, PARM_PREVIOUS_HTML, ds.ProgressNote.PreviousHTMLColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(10, PARM_CURRENT_HTML, ds.ProgressNote.CurrentHTMLColumn.ColumnName, DbType.String);

        }

        private void createParameters_AnswerSheet(IDBManager dbManager, DSProgressNote ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(16);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ANSWERSHEET_ID, ds.AnswerSheet.AnswerSheetIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ANSWERSHEET_ID, ds.AnswerSheet.AnswerSheetIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_TEMPLATE_ID, ds.AnswerSheet.TemplateIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SECTION_ID, ds.AnswerSheet.SectionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_QUESTIONGROUP_ID, ds.AnswerSheet.QuestionGroupIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_QUESTION_ID, ds.AnswerSheet.QuestionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_ANSWER_VALUE, ds.AnswerSheet.AnswerValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ANSWER_TEXT, ds.AnswerSheet.AnswerTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ANSWER_COMMENTS, ds.AnswerSheet.AnswerCommentsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(8, PARM_ROW_ID, ds.AnswerSheet.RowIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PROGRESSNOTE_ID, ds.AnswerSheet.ProgressNoteIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(10, PARM_CREATED_BY, ds.AnswerSheet.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.AnswerSheet.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.AnswerSheet.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.AnswerSheet.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(14, PARM_PATIENT_ID, ds.AnswerSheet.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_VISIT_ID, ds.AnswerSheet.VisitIdColumn.ColumnName, DbType.Int64);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        #region ProgressNote

        public DSProgressNote loadProgressNote(long ProgressNoteId, long visitId, long patientId, long templateId)
        {
            DSProgressNote ds = new DSProgressNote();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ProgressNoteId <= 0)
                    dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, ProgressNoteId);

                if (visitId <= 0)
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, visitId);

                if (patientId <= 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);

                if (templateId <= 0)
                    dbManager.AddParameters(3, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_TEMPLATE_ID, templateId);

                ds = (DSProgressNote)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROGRESSNOTE_SELECT, ds, ds.ProgressNote.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgressNote::LoadProgressNote", PROC_PROGRESSNOTE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Update the ProgressNote 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSProgressNote updateProgressNote(ref DSProgressNote ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, false);
                ds = (DSProgressNote)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROGRESSNOTE_UPDATE, ds, ds.ProgressNote.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgressNote::UpdateProgressNote", PROC_PROGRESSNOTE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProgressNote updateProgressNoteHTML(long ProgressNoteId, string modifiedBy, DateTime modifiedOn, string previousHTML, string currentHTML, ref DSProgressNote ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //this.CreateParameters(dbManager, ds, false);
                dbManager.CreateParameters(5);
                if (ProgressNoteId <= 0)
                    dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, ProgressNoteId);

                dbManager.AddParameters(1, PARM_PREVIOUS_HTML, previousHTML);
                dbManager.AddParameters(2, PARM_CURRENT_HTML, currentHTML);

                dbManager.AddParameters(3, PARM_MODIFIED_BY, modifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, modifiedOn);

                ds = (DSProgressNote)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROGRESSNOTE_HTML_UPDATE, ds, ds.ProgressNote.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgressNote::UpdateProgresNoteHTML", PROC_PROGRESSNOTE_HTML_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string deleteAnswerSheet(string progressNoteId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROGRESSNOTE_ID, progressNoteId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ANSWERSHEET_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgressNote::DeleteAnswerSheet", PROC_ANSWERSHEET_DELETE, ex);
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

        public DSScheduleLookups LookupReasonsAutoComplete(string Active, string ShortName)
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(3, PARM_NAME, ShortName);

                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_BLOCKHOURS_LOOKUP, ds, ds.ScheduleReasons.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPogressNote::LookupReasons", PROC_SCHEDULE_BLOCKHOURS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region AnswerSheet

        /// <summary>
        /// Inserts the Patient-Visit Data
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProgressNote insertPatientVisit(DSProgressNote ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createParameters_AnswerSheet(dbManager, ds, true);
                ds = (DSProgressNote)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ANSWERSHEET_INSERT, ds, ds.AnswerSheet.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProgresNote::InsertPatientVisit", PROC_ANSWERSHEET_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #endregion
        
    }
}
