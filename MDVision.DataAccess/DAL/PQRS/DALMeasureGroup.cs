using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.PQRS
{
    public class DALMeasureGroup
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMeasureGroup()
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

        private const string PROC_MEASUREGROUPS_SEARCH = "Billing.MeasureGroupSearch";
        private const string PROC_MEASUREGROUPS_INSERT = "Billing.sp_MeasureGroupsInsert";
        private const string PROC_MEASUREGROUPS_DELETE = "Billing.sp_MeasureGroupsDelete";
        private const string PROC_MEASUREGROUPS_SELECT = "Billing.sp_MeasureGroupsSelect";
        private const string PROC_MEASUREGROUPS_UPDATE = "Billing.sp_MeasureGroupsUpdate";
        private const string PROC_MEASUREGROUPS_LOOKUP = "[Billing].[sp_MeasureGroupsLookup]";


        private const string PROC_MEASURES_INSERT = "Billing.sp_MeasuresInsert";
        private const string PROC_MEASURES_DELETE = "Billing.sp_MeasuresDelete";
        private const string PROC_MEASURES_SELECT = "Billing.sp_MeasuresSelect";
        private const string PROC_MEASURES_UPDATE = "Billing.sp_MeasuresUpdate";
        
        #endregion

        #region Parameters
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_MEASURE_GROUP_ID = "@MeasureGroupId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_MEASURE_GROUP_NAME = "@MeasureGroupName";
        private const string PARM_SUBMISSION_YEAR = "@SubmissionYear";
        private const string PARM_IS_REPORTED = "@IsReported";

        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_MEASURE_ID = "@MeasureId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_SPECIALTY_ID = "@SpecialtyId";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";


        private const string PARM_ERROR_MeasureGroup = "@ErrorMeasureGroup";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";


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

            dbManager.CreateParameters(15);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEASURE_GROUP_ID, ds.MeasureGroups.MeasureGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEASURE_GROUP_ID, ds.MeasureGroups.MeasureGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.MeasureGroups.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SUBMISSION_YEAR, ds.MeasureGroups.SubmissionYearColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_REPORTED, ds.MeasureGroups.IsReportedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_PROVIDER_ID, ds.MeasureGroups.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MEASURE_ID, ds.MeasureGroups.MeasureIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_PRACTICE_ID, ds.MeasureGroups.PracticeIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SPECIALTY_ID, ds.MeasureGroups.SpecialtyIdsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.MeasureGroups.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.MeasureGroups.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.MeasureGroups.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.MeasureGroups.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.MeasureGroups.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_ENTITY_ID, ds.MeasureGroups.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

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
        public DSPQRS LoadMeasureGroup(string MeasureGroupName, string ProviderIds, Int32 PageNumber, Int32 RowsPerPage, string IsActive)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (string.IsNullOrEmpty(MeasureGroupName ))
                    dbManager.AddParameters(0, PARM_MEASURE_GROUP_NAME, null);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_GROUP_NAME, string.IsNullOrEmpty(MeasureGroupName) ? null : MeasureGroupName);
                dbManager.AddParameters(1, PARM_PROVIDER_ID, string.IsNullOrEmpty(ProviderIds)?null:ProviderIds);
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

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASUREGROUPS_SEARCH, ds, ds.MeasureGroupsList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::LoadMeasureGroup", PROC_MEASUREGROUPS_SEARCH, ex);
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
                    dbManager.AddParameters(0, PARM_MEASURE_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_GROUP_ID, MeasureGroupId);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASUREGROUPS_SELECT, ds, ds.MeasureGroups.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::FillMeasureGroup", PROC_MEASUREGROUPS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the MeasureGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPQRS UpdateMeasureGroup(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPQRS)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MEASUREGROUPS_UPDATE, ds, ds.MeasureGroups.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::UpdateMeasureGroup", PROC_MEASUREGROUPS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the MeasureGroup.
        /// </summary>
        /// <param name="PatMsgId">The MeasureGroup identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteMeasureGroup(Int64 measureGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEASURE_GROUP_ID, measureGroupId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEASUREGROUPS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::DeleteMeasureGroup", PROC_MEASUREGROUPS_DELETE, ex);
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
        /// Inserts the MeasureGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPQRS InsertMeasureGroup(DSPQRS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                try
                {

                    ds = (DSPQRS)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEASUREGROUPS_INSERT, ds, ds.MeasureGroups.TableName);

                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception e)
                {
                    MDVLogger.DALErrorLog("DALMeasureGroup::InsertMeasureGroup", PROC_MEASUREGROUPS_INSERT, e);
                    throw e;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::InsertMeasureGroup", PROC_MEASUREGROUPS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region "Insert, delete, update and get Measures using dataset Functions"
        public DSPQRS FillMeasures(long? MeasureId)
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (MeasureId == null || MeasureId <= 0)
                    dbManager.AddParameters(0, PARM_MEASURE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_MEASURE_ID, MeasureId);

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASURES_SELECT, ds, ds.Measures.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::FillMeasures", PROC_MEASURES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public DSPQRS LookupMeasureProviderGroup()
        {
            DSPQRS ds = new DSPQRS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSPQRS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEASUREGROUPS_LOOKUP, ds, ds.MeasureGroupLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMeasureGroup::LookupMeasureProviderGroup", PROC_MEASUREGROUPS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
