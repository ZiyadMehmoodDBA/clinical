/* Author:  Muhammad Arshad
 * Created Date: 17/03/2016
 * OverView: Created for ProcedureOrder in Clinical Module
 */

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
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;
using MDVision.Model.Clinical.Orders;
using MDVision.Common.Utilities;


namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALProcedureOrder
    {
        #region Variable

        #endregion

        #region Constructors

        public DALProcedureOrder()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALProcedureOrder(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Stored Procedure Names
        //Start//17-03-2016//Ahmad Raza//Initializing Stored Procedures
        private const string PROC_PROCEDURE_ORDER_INSERT = "Clinical.sp_ProcedureOrderInsert";
        private const string PROC_PROCEDURE_ORDER_UPDATE = "Clinical.sp_ProcedureOrderUpdate";
        private const string PROC_PROCEDURE_ORDER_DELETE = "Clinical.sp_ProcedureOrderDelete";
        private const string PROC_PROCEDURE_ORDER_SELECT = "Clinical.sp_ProcedureOrderSelect";
        private const string PROC_ATTACH_PROCEDURE_ORDER = "Clinical.sp_AttachProcedureOrderWithNotes";
        private const string PROC_DETACH_PROCEDURE_ORDER = "Clinical.sp_DetachProcedureOrderFromNotes";
        //End//17-03-2016//Ahmad Raza//Initializing Stored Procedures
        //Start//18-03-2016//Farooq Ahmad//Initializing Stored Procedures
        private const string PROC_PROCEDURE_ORDER_PROBLEM_INSERT = "Clinical.sp_ProcedureOrderProblemInsert";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_UPDATE = "Clinical.sp_ProcedureOrderProblemUpdate";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_DELETE = "Clinical.sp_ProcedureOrderProblemDelete";
        private const string PROC_PROCEDURE_ORDER_PROBLEM_SELECT = "Clinical.sp_ProcedureOrderProblemSelect";
        //End//18-03-2016//Farooq Ahmad//Initializing Stored Procedures

        private const string PROC_PROCEDURE_ORDER_TEST_INSERT = "Clinical.sp_ProcedureOrderTestInsert";
        private const string PROC_PROCEDURE_ORDER_TEST_UPDATE = "Clinical.sp_ProcedureOrderTestUpdate";
        private const string PROC_PROCEDURE_ORDER_TEST_DELETE = "Clinical.sp_ProcedureOrderTestDelete";
        private const string PROC_PROCEDURE_ORDER_TEST_SELECT = "Clinical.sp_ProcedureOrderTestSelect";
        private const string PROC_PROCEDUREORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_ProcedureOrderSelectForSoapText";
        private const string PROC_PROCEDUREORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT = "Clinical.sp_ProcedureOrderSelectForSoapTextWithOrderSetId";

        private const string PROC_NOTES_PROCEDUREORDER_SELECT = "[Clinical].[sp_NotesProcedureOrderSelect]";
        private const string PROC_GET_FIND_PROCEDUREORDER_SELECT = "[Clinical].[sp_ProcedureOrderTestFindingSelect]";

        #endregion

        #region Parameter Names
        //Start//17-03-2016//Ahmad Raza//Initializing Parameters
        private const string PARM_PROCEDURE_ORDER_ID = "@ProcedureOrderId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_ORDER_DATE = "@OrderDate";
        private const string PARM_ORDER_TIME = "@OrderTime";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_NOTE_ID = "@NoteId";
        //End//17-03-2016//Ahmad Raza//Initializing Parameters


        //Start//18-03-2016//Farooq Ahmad//Initializing Parameters
        private const string PARM_PROCEDURE_ORDER_PROBLEM_ID = "@ProcedureOrderProblemId";
        private const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_COMMENTS = "@Comments";
        //End//18-03-2016//Farooq Ahmad//Initializing Parameters
        //Start 21-03-2016 Humaira Yousaf 
        private const string PARM_PROCEDURENAME = "@ProcedureName";
        private const string PARM_ORDERNO = "@OrderNo";
        private const string PARM_ORDERFROM = "@OrderDateFrom";
        private const string PARM_ORDERTO = "@OrderDateTo";
        private const string PARM_STATUS = "@Status";
        //End 21-03-2016 Humaira Yousaf 

        private const string PARM_PROCEDUREORDER_TEST_ID = "@ProcedureOrderTestId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";

        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_IS_FINDING_UPDATED = "@IsFindingUpdate";

        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        #endregion

        #region Supporting Functions

        /// <summary>
        /// Method Name: createProcedureOrderParameters
        /// Author: Ahmad Raza
        /// Date: 17-03-2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createProcedureOrderParameters(IDBManager dbManager, DSProcedureOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrder.ProcedureOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_PROCEDURE_ORDER_ID, ds.ProcedureOrder.ProcedureOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.ProcedureOrder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROVIDER_ID, ds.ProcedureOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ASSIGNEE_ID, ds.ProcedureOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_ORDER_DATE, ds.ProcedureOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_ORDER_TIME, ds.ProcedureOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.ProcedureOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.ProcedureOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.ProcedureOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.ProcedureOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.ProcedureOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAP_TEXT, ds.ProcedureOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Humaira Yousaf for status
            dbManager.AddInsertUpdateParameters(12, PARM_STATUS, ds.ProcedureOrder.StatusColumn.ColumnName, DbType.String);
            //End 22-03-2016 Humaira Yousaf for status

            dbManager.AddInsertUpdateParameters(13, PARM_NOTE_ID, ds.ProcedureOrder.NoteIdColumn.ColumnName, DbType.Int64);

        }

        /// <summary>
        ///  Method Name: createProcedureOrderProblemParameters
        ///  Author: Farooq Ahmad
        ///  Created Date: 18-03-2016
        ///  Description: insert/update  Procedure Order Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
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

        #endregion



        #region INSERT/UPDATE/DELETE/SELECT Functions

        /// <summary>
        /// Method Name: insertUpdateProcedureOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: insert/update  Procedure Order
        /// </summary> 
        /// <param name="DSProcedureOrder" type="DATASET"></param>
        /// 


        public DSProcedureOrder ProcedureOrderInsertUpdate(DSProcedureOrder ds, IDBManager dbManager = null)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //bool IsdbManagerExists = true;
            //if (dbManager == null)
            //{
            //    dbManager = ClientConfiguration.GetDBManager();
            //    dbManager.Open();
            //    IsdbManagerExists = false;
            //}
            try
            {
                //DataTable dtTemp = ds.ProcedureOrder.GetChanges();
                // dbManager.Open();
                //dbManager.BeginTransaction();
                createProcedureOrderParameters(dbManager, ds, true);
                createProcedureOrderParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_INSERT, PROC_PROCEDURE_ORDER_UPDATE, ds, ds.ProcedureOrder.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString(), null, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //ds.RejectChanges();
                ////dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProcedureOrder::ProcedureOrderInsertUpdate", PROC_PROCEDURE_ORDER_INSERT + " " + PROC_PROCEDURE_ORDER_UPDATE, ex);
                throw ex;

            }
            //finally
            //{
            //    if (!IsdbManagerExists)
            //        dbManager.Dispose();
            //}
        }



        //public DSProcedureOrder insertUpdateProcedureOrder(DSProcedureOrder ds)
        //{
        //    DSDBAudit dsDBAudit = new DSDBAudit();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        DataTable dtTemp = ds.ProcedureOrder.GetChanges();
        //        // dbManager.Open();
        //        dbManager.BeginTransaction();
        //        createProcedureOrderParameters(dbManager, ds, true);
        //        createProcedureOrderParameters(dbManager, ds, false);
        //        ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_INSERT, PROC_PROCEDURE_ORDER_UPDATE, ds, ds.ProcedureOrder.TableName);
        //        if (dtTemp != null)
        //        {
        //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString(), null, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString());
        //            dsDBAudit.AcceptChanges();
        //        }
        //        dbManager.CommitTransaction();
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        ds.RejectChanges();
        //        dsDBAudit.RejectChanges();
        //        dbManager.RollBackTransaction();
        //        MDVLogger.DALErrorLog("DALProcedureOrder::insertUpdateProcedureOrder", PROC_PROCEDURE_ORDER_INSERT + " " + PROC_PROCEDURE_ORDER_UPDATE, ex);
        //        throw ex;

        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        /// <summary>
        /// Method Name: loadProcedureOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: loading Procedure Order
        /// </summary> 
        /// <param name="procedureOrderId" type="long">procedureOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSProcedureOrder loadProcedureOrder(long procedureOrderId, long patientId, string procedureName, string orderNumber, long providerId, string orderDateFrom, string orderDateTo, string status, int pageNumber = 1, int rowsPerPage = 1000, string isViewOrder = "", string isPrintOrder = "", long notesId = 0)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(12);

                if (procedureOrderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ProcedureOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //Start 21-03-2016 Humaira Yousaf 
                if (procedureName == "")
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, null);
                else
                    dbManager.AddParameters(5, PARM_PROCEDURENAME, procedureName);

                if (orderNumber == "")
                    dbManager.AddParameters(6, PARM_ORDERNO, null);
                else
                    dbManager.AddParameters(6, PARM_ORDERNO, orderNumber);

                if (providerId == 0)
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, providerId);

                if (orderDateFrom == "")
                    dbManager.AddParameters(8, PARM_ORDERFROM, null);
                else
                    dbManager.AddParameters(8, PARM_ORDERFROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(9, PARM_ORDERTO, null);
                else
                    dbManager.AddParameters(9, PARM_ORDERTO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);
                //End 21-03-2016 Humaira Yousaf 
                if (notesId == 0)
                    dbManager.AddParameters(11, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(11, PARM_NOTE_ID, notesId);


                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_SELECT, ds, ds.ProcedureOrder.TableName);
                if (procedureOrderId > 0)
                {

                    DataTable dtTemp = ds.ProcedureOrder;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString(), null, ds.ProcedureOrder.Rows[0][ds.ProcedureOrder.ProcedureOrderIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProcedureOrder::loadProcedureOrder", PROC_PROCEDURE_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteProcedureOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: deleting Procedure Order
        /// </summary> 
        /// <param name="procedureOrderId" type="long">procedureOrderId to be deleted</param>
        public string deleteProcedureOrder(long procedureOrderId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                // dbManager.Open();
                dbManager.BeginTransaction();
                DSProcedureOrder dsCurrentOrder = loadProcedureOrder(procedureOrderId, 0, "", "", 0, "", "", "", 1, 1000, "", "");
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_DELETE).ToString();
                returnVal = Convert.ToString(((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).Value);
                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    //dsCurrentOrder.ProcedureOrder.Select(dsCurrentOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName + '=' + procedureOrderId).FirstOrDefault().Delete();
                    //DataRow[] drDeleted = dsCurrentOrder.ProcedureOrder.Select("", "", DataViewRowState.Deleted);
                    //DataView dvDeletedProcedureOrder = new DataView(dsCurrentOrder.ProcedureOrder, null, null, DataViewRowState.Deleted);
                    ////dsCurrentOrder.AcceptChanges(); 
                    ////DataTable dtTemp2 = drDeleted.CopyToDataTable();
                    //DataTable dttemp3 = new DataTable(dsCurrentOrder.ProcedureOrder.TableName);
                    //foreach (DataRow drCurrent in drDeleted)
                    //{
                    //    dttemp3.ImportRow(drCurrent);
                    //}
                    //DataTable dtTemp = dvDeletedProcedureOrder.ToTable(); //dsCurrentOrder.ProcedureOrder.GetChanges(DataRowState.Deleted);
                    DataTable dtTemp = dsCurrentOrder.ProcedureOrder;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, procedureOrderId.ToString(), null, procedureOrderId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProcedureOrder::deleteProcedureOrder", PROC_PROCEDURE_ORDER_DELETE, ex);
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


        #region INSERT/DELETE/SELECT Functions ProcedureOrder Problems

        /// <summary>
        /// Method Name: insertUpdateProcedureOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: insert/update  Procedure Order Problems
        /// </summary> 
        /// <param name="DSProcedureOrder" type="DATASET"></param>
        public DSProcedureOrder insertUpdateProcedureOrderProblems(DSProcedureOrder ds, IDBManager dbManager = null)
        {
            //bool IsdbManagerExists = true;
            //DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //if (dbManager == null)
                //{
                //    dbManager = ClientConfiguration.GetDBManager();
                //    dbManager.BeginTransaction();
                //    IsdbManagerExists = false;
                //}
                //dbManager.Open();
                //DataTable dtTemp = ds.ProcedureOrderProblem.GetChanges();
                //dbManager.BeginTransaction();

                createProcedureOrderProblemParameters(dbManager, ds, true);
                createProcedureOrderProblemParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, ds, ds.ProcedureOrderProblem.TableName);
                //if (!IsdbManagerExists)
                //{
                //    if (dtTemp != null)
                //    {
                //        dtTemp.Columns.Add("PrimaryKey");
                //        for (int i = 0; i < ds.ProcedureOrderProblem.Rows.Count; i++)
                //        {
                //            dtTemp.Rows[i]["PrimaryKey"] = ds.ProcedureOrderProblem.Rows[i][ds.ProcedureOrderProblem.ProcedureOrderProblemIdColumn];
                //        }

                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProcedureOrderProblem.Rows[0][ds.ProcedureOrderProblem.ProcedureOrderProblemIdColumn].ToString(), null, ds.ProcedureOrderProblem.Rows[0][ds.ProcedureOrderProblem.ProcedureOrderIdColumn].ToString());
                //        dsDBAudit.AcceptChanges();
                //    }
                //    dbManager.CommitTransaction();
                //}
                    ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //if (!IsdbManagerExists)
                //{
                //    ds.RejectChanges();
                //    dsDBAudit.RejectChanges();
                //    dbManager.RollBackTransaction();
                //}
                MDVLogger.DALErrorLog("DALProcedureOrder::insertUpdateProcedureProblemsOrder", PROC_PROCEDURE_ORDER_PROBLEM_INSERT + " " + PROC_PROCEDURE_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            //finally
            //{
            //    if (!IsdbManagerExists)
            //        dbManager.Dispose();
            //}
        }



        /// <summary>
        /// Method Name: loadProcedureOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: loading Procedure Order Problems
        /// </summary> 
        /// <param name="procedureOrderId" type="long">procedureOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSProcedureOrder loadProcedureOrderProblems(long procedureOrderProblemId, long procedureOrderId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (procedureOrderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);

                if (procedureOrderProblemId == 0)
                    dbManager.AddParameters(1, PARM_PROCEDURE_ORDER_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROCEDURE_ORDER_PROBLEM_ID, procedureOrderProblemId);

                if (patientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ProcedureOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_SELECT, ds, ds.ProcedureOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::loadProcedureOrder", PROC_PROCEDURE_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteProcedureOrderProblem
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: deleting Procedure Order Problem
        /// </summary> 
        /// <param name="procedureOrderId" type="long">procedureOrderId to be deleted</param>
        /// 

        public string deleteProcedureOrderProblems(long procedureOrderId, IDBManager dbManager = null)
        {
            //string returnVal = "";
            //if (dbManager == null)
            //{
            //    dbManager = ClientConfiguration.GetDBManager();
            //}
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                // dbManager.Open();
                //dbManager.BeginTransaction();
                //DSProcedureOrder dsCurrentOrderProblems = loadProcedureOrderProblems(0, Convert.ToInt32(procedureOrderId), 0);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_DELETE);

                //if (returnVal != "")
                //    throw new Exception(returnVal);
                //else
                //{
                //    DataTable dtTemp = dsCurrentOrderProblems.ProcedureOrderProblem;
                //    if (dtTemp != null)
                //    {
                //        if (dsCurrentOrderProblems.ProcedureOrderProblem.Rows.Count > 0)
                //        {
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.ProcedureOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.ProcedureOrderProblem.Rows[0][dsCurrentOrderProblems.ProcedureOrderProblem.ProcedureOrderIdColumn].ToString(), false, false, true);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }
                //    dbManager.CommitTransaction();
                //}

                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProcedureOrder::deleteProcedureOrderProblem", PROC_PROCEDURE_ORDER_PROBLEM_DELETE, ex);
                //string[] str = ex.Message.Split('|');
                //if (str.Length > 1)
                //    return str[1].ToString();
                //else
                //    return ex.Message;
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        #endregion


        #region Association with Notes

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle attach of ProcedureOrder with Note 
        /// </summary>
        /// <param name="procedureOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProcedureOrder attachProcedureOrderWithNotes(string procedureOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSProcedureOrder ds = new DSProcedureOrder();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(procedureOrderId))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROCEDURE_ORDER, ds, ds.ProcedureOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::attachProcedureOrderWithNotes", PROC_ATTACH_PROCEDURE_ORDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle detach of ProcedureOrder from Note 
        /// </summary>
        /// <param name="procedureOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachProcedureOrderFromNotes(string procedureOrderIDs, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(procedureOrderIDs))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderIDs);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PROCEDURE_ORDER);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::detachProcedureOrderFromNotes", PROC_DETACH_PROCEDURE_ORDER, ex);
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
        /// Author: Ahmad Raza
        /// Method Name: loadProcedureOrdersForSoap
        /// Date: 02-07-2016
        /// Desription: This function will load Procedure Orders for soap text
        /// </summary>
        /// <param name="consultationOrderID"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public DSProcedureOrder loadProcedureOrdersForSoap(string procedureOrderID, long patientId)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (string.IsNullOrEmpty(procedureOrderID))
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOrderID);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);



                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDUREORDER_SELECT_FOR_SOAPTEXT, ds, ds.ProcedureOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::loadProcedureOrdersForSoap", PROC_PROCEDUREORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureOrder GetProceduresOrdersForSoapTextByOrderSetId(long OrderSetID, long notesId, long patientid, long providerId)
        {
            DSProcedureOrder ds = new DSProcedureOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, OrderSetID);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientid);
                dbManager.AddParameters(2, PARM_NOTE_ID, notesId);
                dbManager.AddParameters(3, PARM_PROVIDER_ID, providerId);
                List<string> tableNames = new List<string>
                {
                        ds.ProcedureOrder.TableName,
                        ds.ProcedureOrderTest.TableName,
                        ds.ProcedureOrderProblem.TableName
                };
                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDUREORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::GetProceduresOrdersForSoapTextByOrderSetId", PROC_PROCEDUREORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        #region"Procedure Order Test"

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
        public DSProcedureOrder insertUpdateProcedureOrderTest(DSProcedureOrder ds, IDBManager dbManager = null)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            //if (dbManager == null)
            //{
            //    dbManager = ClientConfiguration.GetDBManager();
            //}
            try
            {
                //DataTable dtTemp = ds.ProcedureOrderTest.GetChanges();
                // dbManager.Open();
                //dbManager.BeginTransaction();
                CreateProcedureOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateProcedureOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_INSERT, PROC_PROCEDURE_ORDER_TEST_UPDATE, ds, ds.ProcedureOrderTest.TableName);

                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ProcedureOrderTest.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ProcedureOrderTest.Rows[i][ds.ProcedureOrderTest.ProcedureOrderTestIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProcedureOrderTest.Rows[0][ds.ProcedureOrderTest.ProcedureOrderTestIdColumn].ToString(), null, ds.ProcedureOrderTest.Rows[0][ds.ProcedureOrderTest.ProcedureOrderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProcedureOrder::insertUpdateProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_INSERT + " " + PROC_PROCEDURE_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }
        public DSProcedureOrder LoadProcedureOrderTest(long procedureOderId, Int32 procedureOderTestId, long patientId, string pageNumber, string rowsPerPage)
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
                dbManager.CreateParameters(6);

                if (procedureOderId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ORDER_ID, procedureOderId);

                if (procedureOderTestId == 0)
                    dbManager.AddParameters(1, PARM_PROCEDUREORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROCEDUREORDER_TEST_ID, procedureOderTestId);

                if (patientId <= 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                if (page <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ProcedureOrderTest.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProcedureOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_SELECT, ds, ds.ProcedureOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::LoadProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteProcedureOrderTest(long procedureOrderTestId, string patientid)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                DSProcedureOrder dsCurrentOrderTest = LoadProcedureOrderTest(0, Convert.ToInt32(procedureOrderTestId), 0, "1", "100");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDUREORDER_TEST_ID, procedureOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.ProcedureOrderTest;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, procedureOrderTestId.ToString(), null, dsCurrentOrderTest.ProcedureOrderTest.Rows[0][dsCurrentOrderTest.ProcedureOrderTest.ProcedureOrderIdColumn].ToString(), false, false, true, patientid);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::deleteProcedureOrderTest", PROC_PROCEDURE_ORDER_TEST_DELETE, ex);
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


        public List<OrderFindingsModel> LoadProcedureOrderTestFindings(long ProcedureOrderId, bool isFindingUpdated, long notesId)
        {
            List<OrderFindingsModel> ProcedureOrderFindingList = new List<OrderFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ProcedureOrderId <= 0)
                    dbManager.AddParameters(PARM_PROCEDURE_ORDER_ID, null);
                else
                    dbManager.AddParameters(PARM_PROCEDURE_ORDER_ID, ProcedureOrderId);

                dbManager.AddParameters(PARM_IS_FINDING_UPDATED, isFindingUpdated);

                if (notesId <= 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, notesId);

                ProcedureOrderFindingList = dbManager.ExecuteReaders<OrderFindingsModel>(PROC_GET_FIND_PROCEDUREORDER_SELECT);
                return ProcedureOrderFindingList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::LoadProcedureOrderTestFindings", PROC_GET_FIND_PROCEDUREORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        #region Legacy Notes

        public List<ProcedureOrder> NotesProcedureOrderSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ProcedureOrder> objList_ProcedureOrder = new List<ProcedureOrder>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_PROCEDUREORDER_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        ProcedureOrder model = new ProcedureOrder();
                        var properties = typeof(ProcedureOrder).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_ProcedureOrder.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureOrder::NotesProcedureOrderSelect", PROC_NOTES_PROCEDUREORDER_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ProcedureOrder;
        }

        #endregion Legacy Notes


    }
}

