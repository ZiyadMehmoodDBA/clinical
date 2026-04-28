using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.CCM.CCMHub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.CCM
{
    public class DALHealthRiskAssessment
    {
        #region Constructors
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DALHealthRiskAssessment()
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

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region "Stored Procedure Names"
        //-----------------------------------------------------------------------------------------------------

        private const string PROC_CCM_HR_ASSESSMENT_SELECT = "CCM.sp_HRAssessmentSelect";
        private const string PROC_CCM_HR_ASSESSMENT_UPDATE = "CCM.sp_HRAssessmentUpdate";
        private const string PROC_CCM_HR_ASSESSMENT_INSERT = "CCM.sp_HRAssessmentInsert";
        private const string PROC_CCM_HR_ASSESSMENT_DELETE = "CCM.sp_HRAssessmentDelete";
        private const string PROC_CCM_HR_ASSESSMENT_FILL = "CCM.sp_HRAssessmentFill";
        private const string PROC_CCM_HR_ASSESSMENT_UPDATE_STATUS = "CCM.sp_HRAssessmentUpdateStatus";

        private const string PROC_CCM_HR_ASSESSMENT_TEMPT_SELECT = "CCM.sp_HRAssessmentTemptSelect";
        #endregion
        #region "Parameters"
        //-----------------------------------------------------------------------------------------------------

        private const string PARM_HR_ASSESSMENT_ID = "@HRAssessmentId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_RISK_SCORE = "@RiskScore";
        private const string PARM_ENROLLMENT_INFO_ID = "@EnrollmentInfoId";
        private const string PARM_HR_ASSESSMENT_TEMPLATE_ID = "@HRAssessmentTemptId";
        private const string PARM_HR_ASSESSMENT_HTML = "@HRAssessmentHTML";
        #endregion
        #region CRUD Methods
        public List<HRAssessmentFillModel> loadHRAssessmentList(HRAssessmentSearchModel model)
        {
            // List<HRAssessmentFillModel> listModel = new List<HRAssessmentFillModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //  SqlDataReader reader = null;
            try
            {
                //   List<SqlParameter> parameters = new List<SqlParameter>();
                //  dbManager.Open();
                //  dbManager.CreateParameters(4);
                #region paramaters
                if (model.EnrollmentInfoId <= 0)
                {

                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, DBNull.Value);
                    //  parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                    // parameters.Add(new SqlParameter(PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId));
                }

                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                // parameters.Add(new SqlParameter(PARM_IS_ACTIVE, model.IsActive));

                if (model.PageNumber <= 0)
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, DBNull.Value);
                    // parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_PAGE_NUMBER, model.PageNumber);
                    //parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, model.PageNumber));
                }
                if (model.RowsPerPage <= 0)
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, DBNull.Value);
                    //parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, DBNull.Value));
                }
                else
                {
                    dbManager.AddParameters(PARM_ROWS_PER_PAGE, model.RowsPerPage);
                    // parameters.Add(new SqlParameter(PARM_ROWS_PER_PAGE, model.RowsPerPage));
                }

                return dbManager.ExecuteReaders<HRAssessmentFillModel>(PROC_CCM_HR_ASSESSMENT_SELECT);

                #endregion
                // reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_SELECT);
                //HRAssessmentFillModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new HRAssessmentFillModel();
                //    modelFill.PatientId = Convert.ToInt64(reader["PatientId"]);
                //    modelFill.HRAssessmentId = Convert.ToInt64(reader["RiskAssessmentId"]);
                //    modelFill.RecordCount = Convert.ToInt64(reader["RecordCount"]);
                //    modelFill.Name = MDVUtility.CheckStringNull(reader["Name"]);
                //    modelFill.RiskScore = MDVUtility.CheckStringNull(reader["RiskScore"]);
                //    modelFill.Description = MDVUtility.CheckStringNull(reader["Description"]);
                //    modelFill.ModifiedOn = MDVUtility.CheckDateTimeNull(reader["ModifiedOn"]);

                //    modelFill.Createdby = MDVUtility.CheckStringNull(reader["Createdby"]);
                //    modelFill.ModifiedBy = MDVUtility.CheckStringNull(reader["ModifiedBy"]);
                //    modelFill.IsActive = MDVUtility.CheckBooleanNull(reader["IsActive"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::loadHRAssessmentList", PROC_CCM_HR_ASSESSMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                // dbManager.Dispose();
            }
        }

        public List<HRAssessmentModel> fillHRAssessmentList(long hRAssessmentId)
        {
            // List<HRAssessmentModel> listModel = new List<HRAssessmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //  SqlDataReader reader = null;
            try
            {
                // List<SqlParameter> parameters = new List<SqlParameter>();

                // dbManager.Open();
                //  dbManager.CreateParameters(1);
                #region paramaters

                dbManager.AddParameters(PARM_HR_ASSESSMENT_ID, hRAssessmentId);
                //  parameters.Add(new SqlParameter(PARM_HR_ASSESSMENT_ID, hRAssessmentId));

                return dbManager.ExecuteReaders<HRAssessmentModel>(PROC_CCM_HR_ASSESSMENT_FILL);

                #endregion
                // reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_FILL);
                //HRAssessmentModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new HRAssessmentModel();
                //    modelFill.HRAssessmentTemplateId = MDVUtility.ToInt64(reader["HRAssessmentTemplateId"]);
                //    modelFill.HRAssessmentId = MDVUtility.ToInt64(reader["RiskAssessmentId"]);
                //    modelFill.EnrollmentInfoId = MDVUtility.ToInt64(reader["EnrollmentInfoId"]);
                //    modelFill.VitalsId = MDVUtility.ToInt64(reader["VitalsId"]);
                //    modelFill.HRAssessmentHTML = MDVUtility.CheckStringNull(reader["HRAssessmentHTML"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::fillHRAssessmentList", PROC_CCM_HR_ASSESSMENT_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public long saveHRAssessmentList(HRAssessmentModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                long returnValue = -1;
                dbManager.Open();
                dbManager.CreateParameters(9);

                dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_ID, model.HRAssessmentId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_TEMPLATE_ID, model.HRAssessmentTemplateId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_HTML, model.HRAssessmentHTML);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(i++, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                object carePanId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_INSERT);
                if (carePanId != null)
                {
                    returnValue = MDVUtility.ToInt64(carePanId.ToString());
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::saveHRAssessmentList", PROC_CCM_HR_ASSESSMENT_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public long updateHRAssessmentList(HRAssessmentModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                long returnValue = -1;
                dbManager.Open();
                dbManager.CreateParameters(5);

                //  dbManager.AddParameters(i++, PARM_ENROLLMENT_INFO_ID, model.EnrollmentInfoId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_ID, model.HRAssessmentId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_TEMPLATE_ID, model.HRAssessmentTemplateId);
                dbManager.AddParameters(i++, PARM_HR_ASSESSMENT_HTML, model.HRAssessmentHTML);
                dbManager.AddParameters(i++, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(i++, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                object carePanId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_UPDATE);
                if (carePanId != null)
                {
                    returnValue = MDVUtility.ToInt64(carePanId.ToString());
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::updateHRAssessmentList", PROC_CCM_HR_ASSESSMENT_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateStatusHRAssessmentList(long hRAssessmentId, string isActive)
        {
            int returnVal = 0;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_HR_ASSESSMENT_ID, hRAssessmentId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_UPDATE_STATUS);

                if (returnVal < 0)
                    throw new Exception("Cannot Update Status");

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::updateStatusHRAssessmentList", PROC_CCM_HR_ASSESSMENT_UPDATE_STATUS, ex);
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

        public string deleteHRAssessment(long hRAssessmentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HR_ASSESSMENT_ID, hRAssessmentId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::deleteHRAssessment", PROC_CCM_HR_ASSESSMENT_DELETE, ex);
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

        #region Lookup
        public List<HRAssessmentTemptModel> loadHRAssessmentTemplates()
        {
            // List<HRAssessmentTemptModel> listModel = new List<HRAssessmentTemptModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            // SqlDataReader reader = null;
            try
            {
                // dbManager.Open();
               // List<SqlParameter> parameters = new List<SqlParameter>();
                // dbManager.CreateParameters(1);
                #region paramaters

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters( PARM_ENTITY_ID, DBNull.Value);
                //    parameters.Add(new SqlParameter(PARM_ENTITY_ID, DBNull.Value));
                else
                    dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                //parameters.Add(new SqlParameter(PARM_ENTITY_ID, MDVSession.Current.EntityId));

                return dbManager.ExecuteReaders<HRAssessmentTemptModel>(PROC_CCM_HR_ASSESSMENT_TEMPT_SELECT);
                #endregion
                //reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CCM_HR_ASSESSMENT_TEMPT_SELECT);
                //HRAssessmentTemptModel modelFill = null;
                //while (reader.Read())
                //{
                //    modelFill = new HRAssessmentTemptModel();
                //    modelFill.TemplateId = Convert.ToInt64(reader["TemplateId"]);
                //    modelFill.TemplateName = MDVUtility.CheckStringNull(reader["TemplateName"]);
                //    listModel.Add(modelFill);
                //}
                //return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHealthRiskAssessment::loadHRAssessmentList", PROC_CCM_HR_ASSESSMENT_TEMPT_SELECT, ex);
                throw ex;
            }
            finally
            {
                //  dbManager.Dispose();
            }
        }
        #endregion
    }
}
