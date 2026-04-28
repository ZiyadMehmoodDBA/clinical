using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Immunization;
using MDVision.Common.Utilities;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Orderset;
using MDVision.Model.Clinical.History;
using MDVision.Model.Clinical.History.HistorySummary;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALSocPsyandBehaviorHx
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_GET_QUESTIONS = "Clinical.sp_GetQuestions";
        private const string PROC_GET_ANSWERS = "Clinical.sp_GetAnswers";
        private const string PROC_GET_TOTAL_AND_PHQSCORE = "Clinical.sp_GetTotalAndPHQScore";
        private const string PROC_SOC_PSY_AND_BEHAVIOR_HX_INSERT = "Clinical.sp_SocialandBehaviorHxInsert";
        private const string PROC_SOCIAL_AND_BEHAVIORHX_SELECT = "Clinical.sp_SocialandBehaviorHxSelect";
        private const string PROC_SOCIAL_AND_BEHAVIOR_QA_SELECT = "Clinical.sp_SocialandBehaviorQASelect";
        private const string PROC_SOC_PSY_AND_BEHAVIOR_HX_UPDATE = "Clinical.sp_SocialandBehaviorHxUpdate";
        private const string PROC_SOCIAL_AND_BEHAVIORHX_FOR_GRID = "Clinical.sp_SocialandBehaviorHxForGrid";
        private const string PROC_DETACH_SOCPSYANDBEHAVIORHX_FROM_NOTE = "Clinical.sp_DetachSocialandBehaviorHxFromNotes";
        private const string PROC_SOCPSYANDBEHAVIORHX_SELECT_ForSoapText = "Clinical.sp_SocialandBehaviorHxFromNotes";

        #endregion "Stored Procedure Names"

        #region "Parameters"
        private const string PARM_QUESTION_ID = "@QuestionnaireID";
        private const string PARM_ALL_ANSWER_IDS = "@AllAnswersIds";
        private const string PARM_PHQ_ANSWER_IDS = "@PHQAnswerIds";
        private const string PARM_SOCIALANDBEHAVIORHX_ID = "@SocialandBehaviorHxId";
        private const string PARM_PATIENTID = "@PatientId";
        private const string PARM_SOCIAL_BEHAVIOR_DATE = "@SocialBehaviorDate";
        private const string PARM_UNREMARKABLE = "@Unremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_TOTALSCORE = "@TotalScore";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_ISDELETED = "@IsDeleted";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARM_PHQSCORE = "@PHQscore";
        private const string PARM_XML_QUESTION_ANSWER = "@XMLQuestionAnswer";
        private const string PARM_CURRENT = "@Current";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_NOTESID = "@NoteId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ALCOHOL_ANSWER_IDS = "@AlcoholAnswerIds";
        private const string PARM_SOC_CONN_AND_ISOLANSWER_IDS = "@SocConnAndIsolAnswerIds";
        private const string PARM_EXPOS_TOVIOL_IDS = "@ExposToViolIds";
        #endregion


        #region Constructors

        public DALSocPsyandBehaviorHx()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALSocPsyandBehaviorHx(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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


        #region "Support Functions For Immunization"

        /// <summary>
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function for Create Parameters For Category
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        /// 
        private void createSocPsyandBehaviorHxParameters(IDBManager dbManager, SocPsyandBehaviorHxModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, model.SocialandBehaviorHxId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, model.SocialandBehaviorHxId, DbType.Int64);
            dbManager.AddParameters(PARM_PATIENTID, model.PatientId);
            if (!string.IsNullOrEmpty(model.SocialBehaviorDate))
            {
                dbManager.AddParameters(PARM_SOCIAL_BEHAVIOR_DATE, model.SocialBehaviorDate);
            }
            else
            {
                dbManager.AddParameters(PARM_SOCIAL_BEHAVIOR_DATE, null);
            }

            if (!string.IsNullOrEmpty(model.Unremarkable))
            {
                dbManager.AddParameters(PARM_UNREMARKABLE, model.Unremarkable);
            }
            else
            {
                dbManager.AddParameters(PARM_UNREMARKABLE, null);
            }
            if (!string.IsNullOrEmpty(model.Comments))
            {
                dbManager.AddParameters(PARM_COMMENTS, model.Comments);
            }
            else
            {
                dbManager.AddParameters(PARM_COMMENTS, null);
            }
            if (!string.IsNullOrEmpty(model.TotalScore))
            {
                dbManager.AddParameters(PARM_TOTALSCORE, model.TotalScore);
            }
            else
            {
                dbManager.AddParameters(PARM_TOTALSCORE, null);
            }
            if (!string.IsNullOrEmpty(model.PHQScore))
            {
                dbManager.AddParameters(PARM_PHQSCORE, model.PHQScore);
            }
            else
            {
                dbManager.AddParameters(PARM_PHQSCORE, null);
            }

            if (!string.IsNullOrEmpty(model.IsActive))
            {
                dbManager.AddParameters(PARM_ISACTIVE, model.IsActive);
            }
            else
            {
                dbManager.AddParameters(PARM_ISACTIVE, null);
            }

            if (!string.IsNullOrEmpty(model.IsDeleted))
            {
                dbManager.AddParameters(PARM_ISDELETED, model.IsDeleted);
            }
            else
            {
                dbManager.AddParameters(PARM_ISDELETED, null);
            }

            if (!string.IsNullOrEmpty(model.CreatedBy))
            {
                dbManager.AddParameters(PARM_CREATEDBY, model.CreatedBy);
            }
            else
            {
                dbManager.AddParameters(PARM_CREATEDBY, null);
            }

            if (!string.IsNullOrEmpty(model.CreatedOn))
            {
                dbManager.AddParameters(PARM_CREATEDON, model.CreatedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_CREATEDON, null);
            }

            if (!string.IsNullOrEmpty(model.ModifiedBy))
            {
                dbManager.AddParameters(PARM_MODIFIEDBY, model.ModifiedBy);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIEDBY, null);
            }

            if (!string.IsNullOrEmpty(model.ModifiedOn))
            {
                dbManager.AddParameters(PARM_MODIFIEDON, model.ModifiedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIEDON, null);
            }
            if (!string.IsNullOrEmpty(model.XMLQuestionAnswer))
            {
                dbManager.AddParameters(PARM_XML_QUESTION_ANSWER, model.XMLQuestionAnswer);
            }
            else
            {
                dbManager.AddParameters(PARM_XML_QUESTION_ANSWER, null);
            }
            if (!string.IsNullOrEmpty(model.NotesId))
            {
                dbManager.AddParameters(PARM_NOTESID, model.NotesId);
            }
            else
            {
                dbManager.AddParameters(PARM_NOTESID, null);
            }
        }
        #endregion


        public List<SocPsyandBehaviorHxModel> GetQuestions()
        {
            List<SocPsyandBehaviorHxModel> QuestionsList = new List<SocPsyandBehaviorHxModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                QuestionsList = dbManager.ExecuteReaders<SocPsyandBehaviorHxModel>(PROC_GET_QUESTIONS);
                return QuestionsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::GetQuestions", PROC_GET_QUESTIONS, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public DSImmunization GetAnswers(long QuestionId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_QUESTION_ID, QuestionId);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_ANSWERS, ds, ds.VaccineReactionLookups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetAnswers", PROC_GET_ANSWERS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<SocPsyandBehaviorHxModel> GetTotalAndPHQScore(string AllAnswerIds, string PHQAnswerIds, string AlcoholAnswerIds, string SocConnAndIsolAnswerIds, string ExposToViolIds)
        {
            List<SocPsyandBehaviorHxModel> ScoreList = new List<SocPsyandBehaviorHxModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(AllAnswerIds))
                {
                    dbManager.AddParameters(PARM_ALL_ANSWER_IDS, AllAnswerIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_ALL_ANSWER_IDS, null);
                }
                if (!string.IsNullOrEmpty(PHQAnswerIds))
                {
                    dbManager.AddParameters(PARM_PHQ_ANSWER_IDS, PHQAnswerIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_PHQ_ANSWER_IDS, null);
                }

                if (!string.IsNullOrEmpty(AlcoholAnswerIds))
                {
                    dbManager.AddParameters(PARM_ALCOHOL_ANSWER_IDS, AlcoholAnswerIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_ALCOHOL_ANSWER_IDS, null);
                }
                if (!string.IsNullOrEmpty(SocConnAndIsolAnswerIds))
                {
                    dbManager.AddParameters(PARM_SOC_CONN_AND_ISOLANSWER_IDS, SocConnAndIsolAnswerIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_SOC_CONN_AND_ISOLANSWER_IDS, null);
                }
                if (!string.IsNullOrEmpty(ExposToViolIds))
                {
                    dbManager.AddParameters(PARM_EXPOS_TOVIOL_IDS, ExposToViolIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_EXPOS_TOVIOL_IDS, null);
                }
                ScoreList = dbManager.ExecuteReaders<SocPsyandBehaviorHxModel>(PROC_GET_TOTAL_AND_PHQSCORE);
                return ScoreList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::GetTotalAndPHQScore", PROC_GET_TOTAL_AND_PHQSCORE, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public string SocPsyandBehaviorHxSaveUpdate(SocPsyandBehaviorHxModel model)
        {
            dynamic SocPsyandBehaviorHxId;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                if (MDVUtility.ToInt64(model.SocialandBehaviorHxId) > 0)
                {
                    createSocPsyandBehaviorHxParameters(dbManager, model, false);
                    SocPsyandBehaviorHxId = dbManager.ExecuteScalar(PROC_SOC_PSY_AND_BEHAVIOR_HX_UPDATE);
                }
                else
                {
                    createSocPsyandBehaviorHxParameters(dbManager, model, true);
                    SocPsyandBehaviorHxId = dbManager.ExecuteScalar(PROC_SOC_PSY_AND_BEHAVIOR_HX_INSERT);
                }
                return MDVUtility.ToStr(SocPsyandBehaviorHxId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::SocPsyandBehaviorHxSaveUpdate", PROC_SOC_PSY_AND_BEHAVIOR_HX_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<SocPsyandBehaviorHxModel> LoadSocPsyandBehaviorHx(string SocialandBehaviorHxId, string PatientId, string Current)
        {
            List<SocPsyandBehaviorHxModel> SocPsyandBehavior = new List<SocPsyandBehaviorHxModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(SocialandBehaviorHxId))
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, SocialandBehaviorHxId);
                }
                else
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, null);
                }
                if (!string.IsNullOrEmpty(PatientId))
                {
                    dbManager.AddParameters(PARM_PATIENTID, PatientId);
                }
                else
                {
                    dbManager.AddParameters(PARM_PATIENTID, null);
                }
                if (!string.IsNullOrEmpty(Current))
                {
                    dbManager.AddParameters(PARM_CURRENT, Current);
                }
                else
                {
                    dbManager.AddParameters(PARM_CURRENT, 1);
                }

                SocPsyandBehavior = dbManager.ExecuteReaders<SocPsyandBehaviorHxModel>(PROC_SOCIAL_AND_BEHAVIORHX_SELECT);
                return SocPsyandBehavior;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::LoadSocPsyandBehaviorHx", PROC_SOCIAL_AND_BEHAVIORHX_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<SocPsyandBehaviorHxModel> SearchSocPsyandBehaviorHx(string SocialandBehaviorHxId, string PatientId, string Current, string PageNumber, string RowspPage)
        {
            List<SocPsyandBehaviorHxModel> SocPsyandBehavior = new List<SocPsyandBehaviorHxModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(SocialandBehaviorHxId))
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, SocialandBehaviorHxId);
                }
                else
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, null);
                }
                if (!string.IsNullOrEmpty(PatientId))
                {
                    dbManager.AddParameters(PARM_PATIENTID, PatientId);
                }
                else
                {
                    dbManager.AddParameters(PARM_PATIENTID, null);
                }

                if (!string.IsNullOrEmpty(Current))
                {
                    dbManager.AddParameters(PARM_CURRENT, Current);
                }
                else
                {
                    dbManager.AddParameters(PARM_CURRENT, 1);
                }

                if (!string.IsNullOrEmpty(PageNumber))
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                }
                else
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                }

                if (!string.IsNullOrEmpty(RowspPage))
                {
                    dbManager.AddParameters(PARM_ROWSP_PAGE, RowspPage);
                }
                else
                {
                    dbManager.AddParameters(PARM_ROWSP_PAGE, null);
                }

                SocPsyandBehavior = dbManager.ExecuteReaders<SocPsyandBehaviorHxModel>(PROC_SOCIAL_AND_BEHAVIORHX_FOR_GRID);
                return SocPsyandBehavior;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::SearchSocPsyandBehaviorHx", PROC_SOCIAL_AND_BEHAVIORHX_FOR_GRID, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<SocPsyQuestionAnswerModel> LoadSocialandBehaviorQA(string SocialandBehaviorHxId, string PatientId, string Current)
        {
            List<SocPsyQuestionAnswerModel> SocPsyandBehavior = new List<SocPsyQuestionAnswerModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(SocialandBehaviorHxId))
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, SocialandBehaviorHxId);
                }
                else
                {
                    dbManager.AddParameters(PARM_SOCIALANDBEHAVIORHX_ID, null);
                }
                if (!string.IsNullOrEmpty(PatientId))
                {
                    dbManager.AddParameters(PARM_PATIENTID, PatientId);
                }
                else
                {
                    dbManager.AddParameters(PARM_PATIENTID, null);
                }
                if (!string.IsNullOrEmpty(Current))
                {
                    dbManager.AddParameters(PARM_CURRENT, Current);
                }
                else
                {
                    dbManager.AddParameters(PARM_CURRENT, 1);
                }
                SocPsyandBehavior = dbManager.ExecuteReaders<SocPsyQuestionAnswerModel>(PROC_SOCIAL_AND_BEHAVIOR_QA_SELECT);
                return SocPsyandBehavior;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::LoadSocialandBehaviorQA", PROC_SOCIAL_AND_BEHAVIOR_QA_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public string detachSocPsyandBehaviorHxFromNotes(long SocialandBehaviorHxId, long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (SocialandBehaviorHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_SOCIALANDBEHAVIORHX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SOCIALANDBEHAVIORHX_ID, SocialandBehaviorHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTESID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTESID, NotesId);
                }
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_SOCPSYANDBEHAVIORHX_FROM_NOTE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHx::detachSocPsyandBehaviorHxFromNotes", PROC_DETACH_SOCPSYANDBEHAVIORHX_FROM_NOTE, ex);
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

        public SocPsyandBehaviorHxMod LoadNoteSocPsyandBehaviorHx(long PatientId, long UserId, long EntityId, long NoteId)
        {
            SocPsyandBehaviorHxMod model = new SocPsyandBehaviorHxMod();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENTID, PatientId));

                parameters.Add(new SqlParameter(PARM_USER_ID, UserId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTESID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_SOCPSYANDBEHAVIORHX_SELECT_ForSoapText, parameters))
                {
                    while (reader.Read())
                    {


                        var properties = typeof(SocPsyandBehaviorHxMod).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocPsyandBehaviorHx::LoadNoteSocPsyandBehaviorHx", PROC_SOCPSYANDBEHAVIORHX_SELECT_ForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
