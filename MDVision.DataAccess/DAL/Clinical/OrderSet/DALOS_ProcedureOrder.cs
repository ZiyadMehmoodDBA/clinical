using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
    public class DALOS_ProcedureOrder
    {
        #region "Stored Procedure Names"
        private const string PROC_PROCEDURE_ORDER_INSERT = "Clinical.sp_OSProcedureOrderInsert";
        private const string PROC_PROCEDURE_ORDER_UPDATE = "Clinical.sp_OSProcedureOrderUpdate";
        private const string PROC_PROCEDURE_ORDER_SELECT = "Clinical.sp_OSProcedureOrderSelect";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_DELETE = "Clinical.sp_OSProcedureOrderProblemDelete";
        private const string PROC_PROCEDURE_ORDER_TEST_SELECT = "Clinical.sp_OSProcedureOrderTestSelect";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_INSERT = "Clinical.sp_OSProcedureOrderProblemInsert";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_UPDATE = "Clinical.sp_OSProcedureOrderProblemUpdate";
        private const string PROC_PROCEDURE_ORDER_TEST_INSERT = "Clinical.sp_OSProcedureOrderTestInsert";
        private const string PROC_PROCEDURE_ORDER_TEST_UPDATE = "Clinical.sp_OSProcedureOrderTestUpdate";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_SELECT = "Clinical.sp_OSProcedureOrderProblemSelect";
        private const string PROC_PROCEDURE_ORDER_DELETE = "Clinical.sp_OSProcedureOrderDelete";
        private const string PROC_PROCEDURE_ORDER_TEST_DELETE = "Clinical.sp_OSProcedureOrderTestDelete";

        #endregion

        #region "Parameters"
        private const string PARM_PROCEDURE_ORDER_ID = "@ProcedureOrderId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_STATUS = "@Status";
        private const string PARM_PROCEDURE_ORDER_PROBLEM_ID = "@ProcedureOrderProblemId";
        private const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_PROCEDUREORDER_TEST_ID = "@ProcedureOrderTestId";

        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";

        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_ORDERSET_ID = "@OrderSetId";
        
        #endregion

        #region Constructors
        public DALOS_ProcedureOrder()
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
        private void createProcedureOrderParameters(IDBManager dbManager, DSProcedureOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrder.ProcedureOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrder.ProcedureOrderIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_ASSIGNEE_ID, ds.ProcedureOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_IS_ACTIVE, ds.ProcedureOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(3, PARM_CREATED_BY, ds.ProcedureOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CREATED_ON, ds.ProcedureOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_MODIFIED_BY, ds.ProcedureOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_MODIFIED_ON, ds.ProcedureOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_SOAP_TEXT, ds.ProcedureOrder.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_STATUS, ds.ProcedureOrder.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_ORDERSET_ID, ds.ProcedureOrder.OrderSetIdColumn.ColumnName, DbType.Int64);
            
        }
        private void createProcedureOrderProblemParameters(IDBManager dbManager, DSProcedureOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_PROBLEM_ID, ds.ProcedureOrderProblem.ProcedureOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_PROBLEM_ID, ds.ProcedureOrderProblem.ProcedureOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrderProblem.ProcedureOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.ProcedureOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.ProcedureOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.ProcedureOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.ProcedureOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.ProcedureOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.ProcedureOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.ProcedureOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.ProcedureOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateProcedureOderTestInsertUpdateParameters(IDBManager dbManager, DSProcedureOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(16);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDUREORDER_TEST_ID, ds.ProcedureOrderTest.ProcedureOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(16);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDUREORDER_TEST_ID, ds.ProcedureOrderTest.ProcedureOrderTestIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrderTest.ProcedureOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.ProcedureOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.ProcedureOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_TEST_DATE, ds.ProcedureOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TEST_TIME, ds.ProcedureOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_URGENCY_ID, ds.ProcedureOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COMMENTS, ds.ProcedureOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_IS_ACTIVE, ds.ProcedureOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_BY, ds.ProcedureOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CREATED_ON, ds.ProcedureOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_BY, ds.ProcedureOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIED_ON, ds.ProcedureOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_SOAP_TEXT, ds.ProcedureOrderTest.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(14, PARM_CPT_SNOMEDID, ds.ProcedureOrderTest.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_SNOMEDDESCRIPTION, ds.ProcedureOrderTest.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
        }
        #endregion

        #region CRUD Functions
        public DSProcedureOrder loadProcedureOrder(long procedureOrderId, Int64 OrderSetId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.Open();
                // dbManager.BeginTransaction();
                dbManager.CreateParameters(5);

                if (procedureOrderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                if (pageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rowsPerPage);
                if (OrderSetId == 0)
                    dbManager.AddParameters(4, PARM_ORDERSET_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ORDERSET_ID, OrderSetId);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ProcedureOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_SELECT, ds, ds.ProcedureOrder.TableName);

                //dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::loadProcedureOrder", PROC_PROCEDURE_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureOrder insertUpdateProcedureOrder(DSProcedureOrder ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                createProcedureOrderParameters(dbManager, ds, true);
                createProcedureOrderParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_INSERT, PROC_PROCEDURE_ORDER_UPDATE, ds, ds.ProcedureOrder.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::insertUpdateProcedureOrder", PROC_PROCEDURE_ORDER_INSERT + " " + PROC_PROCEDURE_ORDER_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string deleteProcedureOrderProblems(long procedureOrderId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_DELETE);
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::deleteProcedureOrderProblem", PROC_PROCEDURE_ORDER_PROBLEM_DELETE, ex);
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

        public DSProcedureOrder insertUpdateProcedureOrderProblems(DSProcedureOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.Open();
                createProcedureOrderProblemParameters(dbManager, ds, true);
                createProcedureOrderProblemParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, ds, ds.ProcedureOrderProblem.TableName);
                dsDBAudit.AcceptChanges();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::insertUpdateProcedureProblemsOrder", PROC_PROCEDURE_ORDER_PROBLEM_INSERT + " " + PROC_PROCEDURE_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedureOrder LoadProcedureOrderTest(long procedureOderId, Int32 procedureOderTestId, string pageNumber, string rowsPerPage)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                int page;
                int rpp;
                if (string.IsNullOrEmpty(pageNumber))
                {
                    page = 1;
                }
                else
                {
                    page = Convert.ToInt32(pageNumber);
                }

                if (string.IsNullOrEmpty(rowsPerPage))
                {
                    rpp = 2000;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                dbManager.Open();
                dbManager.CreateParameters(5);

                if (procedureOderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOderId);

                if (procedureOderTestId == 0)
                    dbManager.AddParameters(1, PARM_PROCEDUREORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROCEDUREORDER_TEST_ID, procedureOderTestId);
                if (page <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ProcedureOrderTest.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_SELECT, ds, ds.ProcedureOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::LoadProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureOrder insertUpdateProcedureOrderTest(DSProcedureOrder ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateProcedureOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateProcedureOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_INSERT, PROC_PROCEDURE_ORDER_TEST_UPDATE, ds, ds.ProcedureOrderTest.TableName);
                dsDBAudit.AcceptChanges();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::insertUpdateProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_INSERT + " " + PROC_PROCEDURE_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedureOrder loadProcedureOrderProblems(long procedureOrderProblemId, long procedureOrderId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (procedureOrderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);

                if (procedureOrderProblemId == 0)
                    dbManager.AddParameters(1, PARM_PROCEDURE_ORDER_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROCEDURE_ORDER_PROBLEM_ID, procedureOrderProblemId);

                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ProcedureOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_SELECT, ds, ds.ProcedureOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::loadProcedureOrder", PROC_PROCEDURE_ORDER_PROBLEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string deleteProcedureOrder(long procedureOrderId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_DELETE).ToString();
                returnVal = Convert.ToString(((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).Value);
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::deleteProcedureOrder", PROC_PROCEDURE_ORDER_DELETE, ex);
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
        public string deleteProcedureOrderTest(long procedureOrderTestId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDUREORDER_TEST_ID, procedureOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_ProcedureOrder::deleteProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_DELETE, ex);
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
    }
}
