using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical.Batch
{
    public class DALBatchPatientList
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALBatchPatientList"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALBatchPatientList()
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

        #region "Stored Procedure Names"

        private const string PROC_PATIENT_LIST = "Clinical.sp_PatientList";
        #endregion
        #region Parameters
        private const string PARM_AGEFROM = "@AgeFrom";
        private const string PARM_AGETO = "@AgeTo";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_SMOKING_STATUS_ID = "@SmokingStatusId";
        private const string PARM_RACE_ID = "@RaceId";
        private const string PARM_ETHNICITY_ID = "@EthnicityId";
        private const string PARM_PREF_LANGUAGE_ID = "@PrefLanguageId";
        private const string PARM_PREF_COMMUNICATION_ID = "@PrefCommunicationId";
        private const string PARM_PT_CREATION_FROM = "@Pt_CreationFrom";
        private const string PARM_PT_CREATION_TO = "@Pt_CreationTo";
        private const string PARM_PROBLEMS = "@Problems";
        private const string PARM_PROBLEMS_FROM = "@ProblemsFrom";
        private const string PARM_PROBLEMS_TO = "@ProblemsTo";
        private const string PARM_MEDICATIONS = "@Medications";
        private const string PARM_MEDICATIONS_FROM = "@MedicationsFrom";
        private const string PARM_MEDICATIONS_TO = "@MedicationsTo";
        private const string PARM_ALLERGIES = "@Allergies";
        private const string PARM_ALLERGIES_FROM = "@AllergiesFrom";
        private const string PARM_ALLERGIES_TO = "@AllergiesTo";
        private const string PARM_LABRESULTS = "@LabResults";
        private const string PARM_LABRESULTS_FROM = "@LabResultsFrom";
        private const string PARM_LABRESULTS_TO = "@LabResultsTo";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void createParameters(IDBManager dbManager, DSBatchPatientList ds, Boolean IsInsert, string Type = "BatchPatientList")
        {
            

        }
        #endregion
        /// <summary>
        /// this function is used to get data set against filter  criteria 
        /// </summary>
        /// <param name="ageFrom"></param>
        /// <param name="ageTo"></param>
        /// <param name="gender"></param>
        /// <param name="SmokingStatusId"></param>
        /// <param name="RaceId"></param>
        /// <param name="EthnicityId"></param>
        /// <param name="PrefLanguageId"></param>
        /// <param name="PrefCommunicationId"></param>
        /// <param name="Pt_CreationFrom"></param>
        /// <param name="Pt_CreationTo"></param>
        /// <param name="Problems"></param>
        /// <param name="ProblemsFrom"></param>
        /// <param name="ProblemsTo"></param>
        /// <param name="Medications"></param>
        /// <param name="MedicationsFrom"></param>
        /// <param name="MedicationsTo"></param>
        /// <param name="Allergies"></param>
        /// <param name="AllergiesFrom"></param>
        /// <param name="AllergiesTo"></param>
        /// <param name="LabResults"></param>
        /// <param name="LabResultsFrom"></param>
        /// <param name="LabResultsTo"></param>
        /// <param name="EntityId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSBatchPatientList loadClinical_BatchPatientList(string ageFrom, string ageTo, string gender, string SmokingStatusId, string RaceId,
             string EthnicityId, string PrefLanguageId, string PrefCommunicationId, string Pt_CreationFrom, string Pt_CreationTo, string Problems,
            string ProblemsFrom, string ProblemsTo, string Medications, string MedicationsFrom, string MedicationsTo, string Allergies, string AllergiesFrom,
            string AllergiesTo, string LabResults, string LabResultsFrom, string LabResultsTo,Int64 EntityId, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSBatchPatientList ds = new DSBatchPatientList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();                
                dbManager.CreateParameters(26);
                if (string.IsNullOrEmpty(ageFrom))
                {
                    dbManager.AddParameters(0, PARM_AGEFROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_AGEFROM, ageFrom);
                }
                if (string.IsNullOrEmpty(ageTo))
                {
                    dbManager.AddParameters(1, PARM_AGETO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_AGETO, ageTo);
                }

                if (string.IsNullOrEmpty(gender))
                {
                    dbManager.AddParameters(2, PARM_GENDER, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_GENDER, gender);
                }
                
                if (string.IsNullOrEmpty(SmokingStatusId))//<=0)
                {
                    dbManager.AddParameters(3, PARM_SMOKING_STATUS_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_SMOKING_STATUS_ID, SmokingStatusId);
                }
                if (string.IsNullOrEmpty(RaceId))// <= 0)
                {
                    dbManager.AddParameters(4, PARM_RACE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_RACE_ID, RaceId);
                }
                if (string.IsNullOrEmpty(EthnicityId))// <= 0)
                {
                    dbManager.AddParameters(5, PARM_ETHNICITY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_ETHNICITY_ID, EthnicityId);
                }
                if (string.IsNullOrEmpty(PrefLanguageId))// <= 0)
                {
                    dbManager.AddParameters(6, PARM_PREF_LANGUAGE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_PREF_LANGUAGE_ID, PrefLanguageId);
                }
                if (string.IsNullOrEmpty(PrefCommunicationId))// <= 0)
                {
                    dbManager.AddParameters(7, PARM_PREF_COMMUNICATION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_PREF_COMMUNICATION_ID, PrefCommunicationId);
                }
                if (string.IsNullOrEmpty(Pt_CreationFrom))
                {
                    dbManager.AddParameters(8, PARM_PT_CREATION_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_PT_CREATION_FROM, Convert.ToDateTime(Pt_CreationFrom));
                }
                if (string.IsNullOrEmpty(Pt_CreationTo))
                {
                    dbManager.AddParameters(9, PARM_PT_CREATION_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_PT_CREATION_TO, Convert.ToDateTime(Pt_CreationTo));
                }
                if (string.IsNullOrEmpty(Problems))
                {

                    dbManager.AddParameters(10, PARM_PROBLEMS, DBNull.Value);
                }
                else
                {

                    dbManager.AddParameters(10, PARM_PROBLEMS, Problems);
                }
              //  dbManager.AddParameters(10, PARM_PROBLEMS, Problems);
                if (string.IsNullOrEmpty(ProblemsFrom))
                {
                    dbManager.AddParameters(11, PARM_PROBLEMS_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(11, PARM_PROBLEMS_FROM, Convert.ToDateTime(ProblemsFrom));
                }
                if (string.IsNullOrEmpty(ProblemsTo))
                {
                    dbManager.AddParameters(12, PARM_PROBLEMS_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(12, PARM_PROBLEMS_TO, Convert.ToDateTime(ProblemsTo));
                }

                if (string.IsNullOrEmpty(Medications))
                {

                    dbManager.AddParameters(13, PARM_MEDICATIONS, DBNull.Value);
                }
                else
                {

                    dbManager.AddParameters(13, PARM_MEDICATIONS, Medications);
                }
                
                if (string.IsNullOrEmpty(MedicationsFrom))
                {
                    dbManager.AddParameters(14, PARM_MEDICATIONS_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(14, PARM_MEDICATIONS_FROM, Convert.ToDateTime(MedicationsFrom));
                }
                if (string.IsNullOrEmpty(MedicationsTo))
                {
                    dbManager.AddParameters(15, PARM_MEDICATIONS_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(15, PARM_MEDICATIONS_TO, Convert.ToDateTime(MedicationsTo));
                }
                if (string.IsNullOrEmpty(Allergies))
                {

                    dbManager.AddParameters(16, PARM_ALLERGIES, DBNull.Value);
                }
                else
                {

                    dbManager.AddParameters(16, PARM_ALLERGIES, Allergies);
                }
                
            
                if (string.IsNullOrEmpty(AllergiesFrom))
                {
                    dbManager.AddParameters(17, PARM_ALLERGIES_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(17, PARM_ALLERGIES_FROM, Convert.ToDateTime(AllergiesFrom));
                }
                if (string.IsNullOrEmpty(AllergiesTo))
                {
                    dbManager.AddParameters(18, PARM_ALLERGIES_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(18, PARM_ALLERGIES_TO, Convert.ToDateTime(AllergiesTo));
                }
          
 
                dbManager.AddParameters(19, PARM_LABRESULTS, LabResults);
                if (string.IsNullOrEmpty(LabResultsFrom))
                {
                    dbManager.AddParameters(20, PARM_LABRESULTS_FROM, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(20, PARM_LABRESULTS_FROM, Convert.ToDateTime(LabResultsFrom));
                }
                if (string.IsNullOrEmpty(LabResultsTo))
                {
                    dbManager.AddParameters(21, PARM_LABRESULTS_TO, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(21, PARM_LABRESULTS_TO, Convert.ToDateTime(LabResultsTo));
                }

                dbManager.AddParameters(22, PARM_ENTITY_ID, EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(23, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(23, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(24, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(24, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(25, PARM_RECORD_COUNT, ds.BatchPatientList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);                

                ds = (DSBatchPatientList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_LIST, ds, ds.BatchPatientList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatchPatientList::LoadClinical_BatchPatientList", PROC_PATIENT_LIST, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
