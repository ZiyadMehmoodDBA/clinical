using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.PQRS
{
    public class DALMeasureIndividual
    {
        #region Variable
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMeasureIndividual()
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

        private const string PROC_MEASURE_INDIVIDUAL_SEARCH = "Billing.MeasureIndividualSearch";
        private const string PROC_MEASURE_INDIVIDUAL_INSERT = "Billing.sp_MeasureIndividualInsert";
        private const string PROC_MEASURE_Group_INSERT = "Billing.sp_MeasureGroupsInsert";
        private const string PROC_MEASURE_INDIVIDUAL_DELETE = "Billing.sp_MeasureIndividualDelete";
        private const string PROC_PATIENT_LIST_INSERT = "Clinical.sp_PQRSReasoningInsert";
        private const string PROC_MEASURE_INDIVIDUAL_SELECT = "Billing.sp_MeasureIndividualSelect";
        private const string PROC_MEASURE_GROUP_SELECT_DEATAILS = "Billing.sp_MeasureGroupsSelect";
        private const string PROC_Measure_Groups_SELECT = "Billing.sp_MeasureGroupsLookup";
        private const string PROC_MEASURE_INDIVIDUAL_UPDATE = "Billing.sp_MeasureIndividualUpdate";
        private const string PROC_MEASURE_Group_UPDATE = "Billing.sp_MeasureGroupsUpdate";
        private const string PROC_MEASURE_Group_Delete = "Billing.sp_MeasureGroupsDelete";

        #endregion

        #region Parameters
        private const string PARM_MEASURE_INDIVIDUAl_ID = "@MeasureIndividualId";
        private const string PARM_MEASURE_Group_ID = "@MeasureGroupId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_MEASURE_GROUP_NAME = "@MeasureGroupName";
        private const string PARM_SUBMISSION_YEAR = "@SubmissionYear";
        private const string PARM_PERFORMANCE_YEAR = "@SubmissionYear";
        private const string PARM_IS_REPORTED = "@IsReported";

        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_MEASURE_ID = "@MeasureId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_SPECIALITY_ID = "@SpecialityId";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";


        private const string PARM_ERROR_MeasureGroup = "@ErrorMeasureGroup";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ID = "@Id";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_TREATMENT_TYPE_ID = "@TreatmentTypeId";
        private const string PARM_REASON_TYPE = "@ReasonType";


        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_REASON_COMMENT = "@ReasonComment";

        //private const string PARM_MEASURE_ID = "@MeasureId";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPQRS ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, ds.IndividualReporting.MeasureIndividualIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, ds.IndividualReporting.MeasureIndividualIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.IndividualReporting.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SPECIALITY_ID, ds.IndividualReporting.SpecialityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_SUBMISSION_YEAR, ds.IndividualReporting.SubmissionYearColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ENTITY_ID, ds.IndividualReporting.EntityIdColumn.ColumnName, DbType.Int64);
            
            dbManager.AddParameters(5, PARM_IS_REPORTED, ds.IndividualReporting.IsReportedColumn.ColumnName, DbType.Byte);
            
            dbManager.AddParameters(6, PARM_MEASURE_ID, ds.IndividualReporting.MeasureIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PRACTICE_ID, ds.IndividualReporting.PracticeIdsColumn.ColumnName, DbType.String);
            

            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.IndividualReporting.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.IndividualReporting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.IndividualReporting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.IndividualReporting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.IndividualReporting.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_ERROR_MESSAGE, ds.IndividualReporting.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);

        }

        private void CreateMeasureGroupParameters(IDBManager dbManager, DSPQRS ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(14);

         
              dbManager.AddParameters(0, PARM_MEASURE_Group_ID, ds.IndividualReporting.MeasureGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.IndividualReporting.ProviderMembersColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.IndividualReporting.MeasureGroupNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_PERFORMANCE_YEAR, ds.IndividualReporting.PerformanceYearColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ENTITY_ID, ds.IndividualReporting.EntityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_IS_REPORTED, ds.IndividualReporting.IsReportedColumn.ColumnName, DbType.Byte);

            dbManager.AddParameters(6, PARM_MEASURE_ID, ds.IndividualReporting.MeasureIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PRACTICE_ID, ds.IndividualReporting.PracticeIdsColumn.ColumnName, DbType.String);


            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.IndividualReporting.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.IndividualReporting.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.IndividualReporting.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.IndividualReporting.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.IndividualReporting.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_ERROR_MESSAGE, ds.IndividualReporting.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);

        }

        private void CreateReasoningParameters(IDBManager dbManager, DSPQRS ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ID, ds.PatientList.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ID, ds.PatientList.IdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientList.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_VISIT_ID, ds.PatientList.VisitIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_TREATMENT_TYPE_ID, ds.PatientList.TreatmentTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_MEASURE_ID, ds.PatientList.MeasureColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_REASON_COMMENT, ds.PatientList.ReasonCommentsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_REASON_TYPE, ds.PatientList.ReasonTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PatientList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.PatientList.CreatedByColumn.ColumnName, DbType.String);


        }
        #endregion


        #region "Insert, delete, update and get MeasureGroup using dataset Functions"
        /// <summary>
        /// Loads the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSPQRS LoadMeasureIndividual(Int64 IndividualReportingId, Int64 ProviderId, Int64 SpecialtyId, Int32 PageNumber , Int32 RowsPerPage, string IsActive )
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                //if (IndividualReportingId<=0)
                //    dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, DBNull.Value);
                //else
                //    dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, IndividualReportingId);
                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (SpecialtyId <= 0)
                    dbManager.AddParameters(1, PARM_SPECIALITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_SPECIALITY_ID, SpecialtyId);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, string.IsNullOrEmpty(IsActive) ? null : IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.MeasureGroups.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASURE_INDIVIDUAL_SEARCH, ds, ds.IndividualReportingList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::LoadMeasureIndividual", PROC_MEASURE_INDIVIDUAL_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS FillMeasureIndividual(long MeasureIndividualId)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (MeasureIndividualId <= 0)
                    dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, MeasureIndividualId);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASURE_INDIVIDUAL_SELECT, ds, ds.IndividualReporting.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::FillMeasureIndividual", PROC_MEASURE_INDIVIDUAL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS FillMeasureGroup(long MeasureGroupId)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (MeasureGroupId <= 0)
                    dbManager.AddParameters(0, PARM_MEASURE_Group_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_Group_ID, MeasureGroupId);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASURE_GROUP_SELECT_DEATAILS, ds, ds.IndividualReporting.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::FillMeasureIndividual", PROC_MEASURE_GROUP_SELECT_DEATAILS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS FillMeasureGroupData(string MeasureGroupId ,string Providers,string IsActive ,string PageNumber, string RowspPage)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(6);

                if (string.IsNullOrEmpty(MeasureGroupId))
                    dbManager.AddParameters(0, PARM_MEASURE_Group_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_Group_ID, MeasureGroupId);

                if(string.IsNullOrEmpty(Providers))
                dbManager.AddParameters(1, PARM_PROVIDER_IDS, null);
                else
                dbManager.AddParameters(1, PARM_PROVIDER_IDS, Providers);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                if(string.IsNullOrEmpty(PageNumber))
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);
                else dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);

                if (string.IsNullOrEmpty(RowspPage))
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, 15);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, RowspPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.MeasureGroups.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Measure_Groups_SELECT, ds, ds.MeasureGroups.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::FillMeasureGroupData", PROC_Measure_Groups_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the MeasureIndividual.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPQRS UpdateMeasureIndividual(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPQRS)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MEASURE_INDIVIDUAL_UPDATE, ds, ds.IndividualReporting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::UpdateMeasureIndividual", PROC_MEASURE_INDIVIDUAL_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPQRS UpdateMeasureGroupData(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateMeasureGroupParameters(dbManager, ds, false);
                ds = (DSPQRS)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MEASURE_Group_UPDATE, ds, ds.IndividualReporting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::UpdateMeasureGroupData", PROC_MEASURE_Group_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the MeasureIndividual.
        /// </summary>
        /// <param name="PatMsgId">The MeasureIndividual identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteMeasureIndividual(Int64 measureIndividualId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEASURE_INDIVIDUAl_ID, measureIndividualId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEASURE_INDIVIDUAL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::DeleteMeasureIndividual", PROC_MEASURE_INDIVIDUAL_DELETE, ex);
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


        public string DeleteMeasureGroupData(string measureGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEASURE_Group_ID, measureGroupId);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEASURE_Group_Delete);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::DeleteMeasureGroupData", PROC_MEASURE_INDIVIDUAL_DELETE, ex);
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
        /// Inserts the MeasureIndividual.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPQRS InsertMeasureIndividual(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSPQRS)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEASURE_INDIVIDUAL_INSERT, ds, ds.IndividualReporting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::InsertMeasureIndividual", PROC_MEASURE_INDIVIDUAL_DELETE, ex);
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

        public DSPQRS InsertMeasureGroup(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateMeasureGroupParameters(dbManager, ds, true);
                ds = (DSPQRS)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEASURE_Group_INSERT, ds, ds.IndividualReporting.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::InsertMeasureGroup", PROC_MEASURE_Group_INSERT, ex);
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
        /// Author:Ahmad Raza
        /// Date:19-08-2016
        /// Function Name: InsertPatientList
        /// Description: Inserts PatientList's Missing Data
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSPQRS InsertPatientList(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateReasoningParameters(dbManager, ds, true);
                ds = (DSPQRS)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_LIST_INSERT, ds, ds.PatientList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureIndividual::InsertPatientList", PROC_PATIENT_LIST_INSERT, ex);
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
