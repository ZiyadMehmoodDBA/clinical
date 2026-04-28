using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.FollowUp
{
    public class DALFollowUpARCall
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_FOLLOW_UP_AR_CALL_SELECT = "Billing.sp_FollowupARCallSelect";
        private const string PROC_FOLLOW_UP_AR_CALL_INSERT = "Billing.sp_FollowupARCallInsert";
        private const string PROC_FOLLOW_UP_AR_CALL_UPDATE = "Billing.sp_FollowupARCallUpdate";
        private const string PROC_FOLLOW_UP_AR_CALL_DELETE = "Billing.sp_FollowupARCallDelete";
        private const string PROC_FOLLOW_UP_CALL_STAUS_LOOKUP = "[Billing].[sp_CallStatusLookup]";

        #endregion

        #region "Parameters"

        private const string PARM_CALL_ID = "@ARCallId";
        private const string PARM_TYPE_ID = "@ARTypeId";
        private const string PARM_ACTION_ID = "@FollowupActionId";
        private const string PARM_REASON_ID = "@FollowupReasonId";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_DURATION = "@Duration";
        
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_INSURANCE_AR_ID = "@InsuranceARDtId";
        private const string PARM_PATIENT_AR_ID = "@PatientARDtId";



        #endregion


        #region Constructors
        public DALFollowUpARCall()
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
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CALL_ID, ds.FollowupARCall.ARCallIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CALL_ID, ds.FollowupARCall.ARCallIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_TYPE_ID, ds.FollowupARCall.ARTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ACTION_ID, ds.FollowupARCall.FollowupActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_REASON_ID, ds.FollowupARCall.FollowupReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_STATUS_ID, ds.FollowupARCall.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_DURATION, ds.FollowupARCall.DurationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_COMMENTS, ds.FollowupARCall.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.FollowupARCall.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.FollowupARCall.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.FollowupARCall.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.FollowupARCall.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.FollowupARCall.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(12, PARM_INSURANCE_AR_ID, ds.FollowupARCall.InsuranceARDtIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_PATIENT_AR_ID, ds.FollowupARCall.PatientARDtIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp LoadFollowUpARCall(Int64 FollowUpARCallId, Int64 ARTypeId, Int64 ActionId, Int64 ReasonId, Int64 InsuranceARDtId,Int64 PatientARDtId, int PageNumber, int RowspPage)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); 
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(9);
                if (FollowUpARCallId == 0)
                    dbManager.AddParameters(0, PARM_CALL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CALL_ID, FollowUpARCallId);

                if (ARTypeId == 0)
                    dbManager.AddParameters(1, PARM_TYPE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TYPE_ID, ARTypeId);

                if (ActionId == 0)
                    dbManager.AddParameters(2, PARM_ACTION_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ACTION_ID, ActionId);

                if (ReasonId == 0)
                    dbManager.AddParameters(3, PARM_REASON_ID, null);
                else
                    dbManager.AddParameters(3, PARM_REASON_ID, ReasonId);

                if (InsuranceARDtId == 0)
                    dbManager.AddParameters(4, PARM_INSURANCE_AR_ID, null);
                else
                    dbManager.AddParameters(4, PARM_INSURANCE_AR_ID, InsuranceARDtId);

                if (PatientARDtId == 0)
                    dbManager.AddParameters(5, PARM_PATIENT_AR_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_AR_ID, PatientARDtId);
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(7, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWS_PERPAGE, RowspPage);


                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.FollowupARCall.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_CALL_SELECT, ds, ds.FollowupARCall.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::LoadFollowUpARCall", PROC_FOLLOW_UP_AR_CALL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFollowUp InsertFollowUpARCall(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_CALL_INSERT, ds, ds.FollowupARCall.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::InsertFollowUpARCall", PROC_FOLLOW_UP_AR_CALL_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateFollowUpARCall(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_CALL_UPDATE, ds, ds.FollowupARCall.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::UpdateFollowUpARCall", PROC_FOLLOW_UP_AR_CALL_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteFollowUpARCall(Int64 followUpARCallID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CALL_ID, followUpARCallID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_AR_CALL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsuranceAR::DeleteFollowUpARCall", PROC_FOLLOW_UP_AR_CALL_DELETE, ex);
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
        public DSFollowUp LookupCallStatus()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_CALL_STAUS_LOOKUP, ds, ds.CallStatusLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpReason::LookupCallStatus", PROC_FOLLOW_UP_CALL_STAUS_LOOKUP, ex);
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
