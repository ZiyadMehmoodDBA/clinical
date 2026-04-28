using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALParticipantProvider
    {

        #region Variable



        #endregion

        #region " Stored Procedure Names"
        private const string PROC_PARTICIPANT_INSERT = "Provider.sp_ProviderParticipantInsert";
        private const string PROC_PARTICIPANT_UPDATE = "Provider.sp_ProviderParticipantUpdate";
        private const string PROC_PARTICIPAN_DELETE = "Provider.sp_ProviderParticipantDelete";
        private const string PROC_PARTICIPANT_SELECT = "Provider.sp_ProviderParticipantSelect";
        #endregion

        #region "Parameters"
        private const string PARM_PARTICIPANTPROVIDER_ID = "@ProviderParticipantId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ASSIGNMENT = "@Assignment";
        private const string PARM_ASSIGNMENT_ID = "@AssingmentId";
        private const string PARM_PARTICIPANT_STATUS_ID = "@ProviderParticipantStatusId";
        private const string PARM_ASSIGNMENT_TYPE_RTK = "@AssignmentTypeRTK";
        private const string PARM_ASSIGNMENT_ADDITIONAL_ID = "@AssingnmentAdditionalId";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@errormessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
     

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALParticipantProvider"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALParticipantProvider()
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
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSProfile ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PARTICIPANTPROVIDER_ID, ds.ProviderParticipant.ProviderParticipantIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PARTICIPANTPROVIDER_ID, ds.ProviderParticipant.ProviderParticipantIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.ProviderParticipant.ProviderIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ASSIGNMENT, ds.ProviderParticipant.AssignmentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ASSIGNMENT_ID, ds.ProviderParticipant.AssingnmentIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PARTICIPANT_STATUS_ID, ds.ProviderParticipant.ProviderParticipantStatusIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ASSIGNMENT_TYPE_RTK, ds.ProviderParticipant.AssignmentTypeRTKColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ASSIGNMENT_ADDITIONAL_ID, ds.ProviderParticipant.AssingnmentAdditionalIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_START_DATE, ds.ProviderParticipant.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.ProviderParticipant.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ISACTIVE, ds.ProviderParticipant.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.ProviderParticipant.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.ProviderParticipant.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.ProviderParticipant.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.ProviderParticipant.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the specialty.
        /// </summary>
        /// <param name="ProviderParticipantId">The Provider Participant identifier.</param>
        /// <param name="Assignment">The Assignment.</param>
        /// <param name="ParticipantStatusId">The Participant Status.</param>
        /// <returns></returns>
        public DSProfile LoadParticipantProvider(long ProviderParticipantId, string Assignment, string Active, string ProviderId , int PageNumber = 1, int RowsPerPage = 1000)
        {

            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (string.IsNullOrEmpty(Assignment))
                    Assignment = null;
                if (string.IsNullOrEmpty(Active))
                    Active = null;

                if (string.IsNullOrEmpty(ProviderId))
                    ProviderId = null;
              
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ProviderParticipantId <= 0)
                    dbManager.AddParameters(0, PARM_PARTICIPANTPROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PARTICIPANTPROVIDER_ID, ProviderParticipantId);

                dbManager.AddParameters(1, PARM_ASSIGNMENT, Assignment);
                dbManager.AddParameters(2, PARM_ISACTIVE, Active);
                dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ProviderParticipant.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PARTICIPANT_SELECT, ds, ds.ProviderParticipant.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALParticipant::LoadParticipantProvider", PROC_PARTICIPANT_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }
       
        /// <summary>
        /// Updates the ParticipantProvider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateParticipantProvider(ref DSProfile ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //  DataTable dtTemp = ds.ProviderParticipant.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PARTICIPANT_UPDATE, ds, ds.ProviderParticipant.TableName);

                //if (dtTemp != null && ds.ProviderParticipant.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProviderParticipant.Rows[0][ds.ProviderParticipant.ProviderParticipantIdColumn].ToString(), null, ds.Specialty.Rows[0][ds.ProviderParticipant.ProviderParticipantIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALParticipantProvider::UpdateParticipantProvider", PROC_PARTICIPANT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the Participant.
        /// </summary>
        /// <param name="ProviderParticipantIds">The Participant Provider ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteParticipantProvider(string ProviderParticipantIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PARTICIPANTPROVIDER_ID, ProviderParticipantIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PARTICIPAN_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALParticipantProvider::DeleteParticipant", PROC_PARTICIPAN_DELETE, ex);
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
        /// Inserts the Participant Provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertParticipantProvider(ref DSProfile ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.ProviderParticipant.GetChanges();
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PARTICIPANT_INSERT, ds, ds.ProviderParticipant.TableName);
                //if (dtTemp != null && ds.ProviderParticipant.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProviderParticipant.Rows[0][ds.ProviderParticipant.ProviderParticipantIdColumn].ToString(), null, ds.ProviderParticipant.Rows[0][ds.ProviderParticipant.ProviderParticipantIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALParticipantProvider::InsertParticipantProvider", PROC_PARTICIPANT_INSERT, ex);
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
