using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
   
    public class DALOS_ProblemLists
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PROBLEMLIST_INSERT = "Clinical.sp_OS_ProblemListInsert";
        private const string PROC_PROBLEMLIST_UPDATE = "Clinical.sp_OS_ProblemListUpdate";

        private const string PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE = "Clinical.sp_OS_ProblemListUpdateStatus";
        private const string PROC_PROBLEMLIST_UPDATE_FOR_GRID = "Clinical.sp_OS_ProblemListUpdate_ForGrid";


        private const string PROC_PROBLEMLIST_DELETE = "Clinical.sp_OS_ProblemListDelete";
        private const string PROC_PROBLEMLIST_SELECT = "Clinical.sp_OS_ProblemListSelect";

       // private const string PROC_PROBLEMLIST_SELECT_OP = "Clinical.sp_OS_ProblemListSelect";

        private const string PROC_PROBLEMLIST_SELECT_FOR_INACTIVE = "Clinical.sp_OS_ProblemListSelect_For_Inactive";

        private const string PROC_PROBLEMLIST_SELECT_FOR_FILL = "Clinical.sp_OS_ProblemListSelect_Fill";


        private const string PROC_PROBLEM_LIST_LOOKUP = "Clinical.sp_OS_ProblemListLookUp";


        private const string PROC_PROBLEMLIST_HISTORY_SELECT = "Clinical.sp_OS_ProblemListHistorySelect";

  

      

      
        private const string PROC_PROBLEMLIST_EXISTS = "Clinical.sp_OS_ProblemListExists";
        

        #endregion

        #region "Parameters"

        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        private const string PARM_PROBLEM_NAME = "@ProblemName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_CHRONICITY_LEVEL = "@ChronicityLevel";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_ORDERSET_ID = "@OrderSetId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PROBLEM_EXISTS = "@Exists";
        private const string PARM_INACTIVE_VALUE = "@InActiveChkBoxValue";
        private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_IS_HISTORY = "@IsHistory";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT_ACTIVE_OR_INACTIVE = "@RecordCountActiveInactive";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        //M.Wasim Added new parameter @RcopiaID for [Clinical].[InsertProblemsRcopialID] stored procedure on 1/7/2016.
        private const string PARM_RCOPIAID = "@RcopiaID";

        //------------------------------
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_ISCHIEF_COMPLAINT = "@IsChiefComplaint";
        //------------------------------

        private const string PROC_CHRONICITY_SEVERITY_LOOKUP = "Clinical.sp_ChronicityAndSeverityLookup";

        #endregion

        #region Constructors
        public DALOS_ProblemLists()
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

        #region "Support Functions Problem Lists"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSOS_ProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(23);
            int i = 0;
            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(i++, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(i++, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ORDERSET_ID, ds.ProblemList.OrderSetIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(i++, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(i++, PARM_CREATED_BY, ds.ProblemList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CREATED_ON, ds.ProblemList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(i++, PARM_INACTIVE_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.String);

            dbManager.AddParameters(i++, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_SNOMEDID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_SNOMED_DESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ISCHIEF_COMPLAINT, ds.ProblemList.IsChiefComplaintColumn.ColumnName, DbType.String);


        }

        private void CreateParametersForInactive(IDBManager dbManager, DSOS_ProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_INACTIVE_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.String);
           
        }



        private void CreateParametersForUpdateRow(IDBManager dbManager, DSOS_ProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(18);
            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ORDERSET_ID, ds.ProblemList.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SNOMEDID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_SNOMED_DESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            


        }
        // Start 7/1/2016 Muhammad Wasim Created to use parameter for Stored Procedure [Clinical].[InsertProblemsRcopialID] end
        private void CreateParametersProblemRcopiaID(IDBManager dbManager, DSOS_ProblemLists ds)
        {
            dbManager.CreateParameters(3);


            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int32);



            dbManager.AddParameters(1, PARM_RCOPIAID, ds.ProblemList.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ORDERSET_ID, ds.ProblemList.OrderSetIdColumn.ColumnName, DbType.Int32);

        }
        #endregion


        #region "Problem Lists"
        public DSOS_ProblemLists InsertProblemLists(DSOS_ProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);
                ds = (DSOS_ProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_INSERT, ds, ds.ProblemList.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::InsertProblemLists", PROC_PROBLEMLIST_INSERT, ex);
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
        //M.Wasim Added new method InsertProblemRcopialID for inserting Rcopia in Problem List on 1/7/2016.
        
        public DSOS_ProblemLists UpdateProblemLists(DSOS_ProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
               // DataTable dtTemp = ds.ProblemList.GetChanges();
                 dbManager.Open();
               // dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSOS_ProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE, ds, ds.ProblemList.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
              //  dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::UpdateProblemLists", PROC_PROBLEMLIST_UPDATE, ex);
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

        public DSOS_ProblemLists UpdateProblemListsForInActive(DSOS_ProblemLists ds)
        {
           // DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                this.CreateParametersForInactive(dbManager, ds, false);
                ds = (DSOS_ProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE, ds, ds.ProblemList.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::UpdateProblemListsForInActive", PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE, ex);
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

        public DSOS_ProblemLists UpdateProblemListsOp(DSOS_ProblemLists ds)
        {
           // DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                this.CreateParametersForUpdateRow(dbManager, ds, false);
                ds = (DSOS_ProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE_FOR_GRID, ds, ds.ProblemList.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::UpdateProblemListsOp", PROC_PROBLEMLIST_UPDATE_FOR_GRID, ex);
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


        public string DeleteProblemLists(string ProblemListId)
        {
            string returnVal = "";
          //  DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSOS_ProblemLists ds = LoadProblemLists(Convert.ToInt64(ProblemListId), 0, "0", "", 1, 1000);
                //DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROBLEMLIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                //else
                //{
                //    if (dtTemp != null)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
             //   dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::DeleteProblemLists", PROC_PROBLEMLIST_DELETE, ex);
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

        public string CheckProblemListExists(long OrderSetId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
              
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ORDERSET_ID, OrderSetId);
                dbManager.AddParameters(1, PARM_PROBLEM_EXISTS, "", DbType.String, ParamDirection.Output, null, 1);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROBLEMLIST_EXISTS).ToString();

                if (returnVal == "1" || returnVal == "0")
                {
                    return returnVal;
                }
                else
                {
                    throw new Exception(returnVal);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_ProblemLists::CheckProblemListExists", PROC_PROBLEMLIST_EXISTS, ex);
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






        public DSOS_ProblemLists LoadProblemLists(long ProblemListId, long OrderSetId,  string isHistory = "0", string active = "", int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
        //    DSDBAudit dsDBAudit = new DSDBAudit();
            DSOS_ProblemLists ds = new DSOS_ProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                //DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                if (OrderSetId == 0)
                    dbManager.AddParameters(1, PARM_ORDERSET_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ORDERSET_ID, OrderSetId);
               

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(3, PARM_IS_ACTIVE, active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);

                if (isHistory == "1")
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSOS_ProblemLists dsProblemListHistory = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemHistory.TableName);
                    if (dsProblemListHistory != null)
                    {
                        ds.Merge(dsProblemListHistory);
                    }
                }

                //if (dtTemp != null)
                //{
                //    if (isViewProblemList == "1" || isPrintProblemList == "1")
                //    {
                //        bool isViewAction = isViewProblemList == "1" ? true : false;
                //        bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                //dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
              //  dsDBAudit.RejectChanges();
              //  dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


       
        public DSOS_ProblemLists LoadProblemListsOp(long OrderSetId,  string isHistory = "0", string active = "", int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSOS_ProblemLists ds = new DSOS_ProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                //DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(6);


                if (OrderSetId == 0)
                    dbManager.AddParameters(0, PARM_ORDERSET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDERSET_ID, OrderSetId);
               

                dbManager.AddParameters(1, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(2, PARM_IS_ACTIVE, active);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);



                ds = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);
                if (ds.ProblemList.Rows.Count > 0)
                {
                    if (isHistory == "1")
                    {
                        dbManager.AddParameters(1, PARM_IS_HISTORY, isHistory);
                        DSOS_ProblemLists dsProblemListHistory = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemHistory.TableName);
                        if (dsProblemListHistory != null)
                        {
                            ds.Merge(dsProblemListHistory);
                        }
                    }
                }
                
               // dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
              //  dsDBAudit.RejectChanges();
            //    dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOS_ProblemLists LoadProblemListsForInActive(long ProblemListId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSOS_ProblemLists ds = new DSOS_ProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
               // DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                ds = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_FOR_INACTIVE, ds, ds.ProblemList.TableName);
               // ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
              //  dsDBAudit.RejectChanges();
              //  dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::LoadProblemListsForInActive", PROC_PROBLEMLIST_SELECT_FOR_INACTIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOS_ProblemLists LoadProblemListsForFillData(long ProblemListId, string isViewAction = "")
        {
           // DSDBAudit dsDBAudit = new DSDBAudit();
            DSOS_ProblemLists ds = new DSOS_ProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
               // dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                ds = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_FOR_FILL, ds, ds.ProblemList.TableName);
                //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
                //if (dtTemp != null)
                //{
                //    if (isViewAction == "1")
                //    {
                //        bool isView = isViewAction == "1" ? true : false;
                //        if (ds.ProblemList.Rows.Count > 0)
                //        {
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isView, false, false);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }
                //}
                //dbManager.CommitTransaction();
                //End 27-10-2016 Humaira Yousaf to log view action for problem lists                         
                return ds;
            }
            catch (Exception ex)
            {
              //  dsDBAudit.RejectChanges();
              //  dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProblemLists::LoadProblemListsForInActive", PROC_PROBLEMLIST_SELECT_FOR_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSOS_ProblemLists LookupProblemListsForOrderSet(int orderSetId, int problemListId = -1, string ProblemName = null)
        {
            DSOS_ProblemLists ds = new DSOS_ProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
                if (orderSetId <= 0)
                {
                    dbManager.AddParameters(0, PARM_ORDERSET_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ORDERSET_ID, orderSetId);
                }
                if (problemListId <= 0)
                {
                    dbManager.AddParameters(1, PARM_PROBLEMLIST_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PROBLEMLIST_ID, problemListId);
                }

                dbManager.AddParameters(2, PARM_PROBLEM_NAME, ProblemName);
                //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list

                ds = (DSOS_ProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEM_LIST_LOOKUP, ds, ds.ProblemListForOS.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALos_Procedures::LookupProblemListsForOrderSet", PROC_PROBLEM_LIST_LOOKUP, ex);
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
