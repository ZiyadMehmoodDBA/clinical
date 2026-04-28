/* Author:  Muhammad Arshad
 * Created Date: 17/03/2016
 * OverView: Created for ConsultationOrder in Clinical Module
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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALConsultationOrder
    {
        #region Variable

        #endregion

        #region Constructors

        public DALConsultationOrder()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALConsultationOrder(SharedVariable SharedVariable)
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
        private const string PROC_CONSULTATION_ORDER_INSERT = "Clinical.sp_ConsultationOrderInsert";
        private const string PROC_CONSULTATION_ORDER_UPDATE = "Clinical.sp_ConsultationOrderUpdate";
        private const string PROC_CONSULTATION_ORDER_DELETE = "Clinical.sp_ConsultationOrderDelete";
        private const string PROC_CONSULTATION_ORDER_SELECT = "Clinical.sp_ConsultationOrderSelect";
        private const string PROC_ATTACH_CONSULTATION_ORDER = "Clinical.sp_AttachConsultationOrderWithNotes";
        private const string PROC_DETACH_CONSULTATION_ORDER = "Clinical.sp_DetachConsultationOrderFromNotes";
        //End//17-03-2016//Ahmad Raza//Initializing Stored Procedures

        //Start//18-03-2016//Farooq Ahmad//Initializing Stored Procedures
        private const string PROC_CONSULTATION_ORDER_PROBLEM_INSERT = "Clinical.sp_ConsultationOrderProblemInsert";
        private const string PROC_CONSULTATION_ORDER_PROBLEM_UPDATE = "Clinical.sp_ConsultationOrderProblemUpdate";
        private const string PROC_CONSULTATION_ORDER_PROBLEM_DELETE = "Clinical.sp_ConsultationOrderProblemDelete";
        private const string PROC_CONSULTATION_ORDER_PROBLEM_SELECT = "Clinical.sp_ConsultationOrderProblemSelect";
        //End//18-03-2016//Farooq Ahmad//Initializing Stored Procedures

        private const string PROC_CONSULTATION_ORDER_TEST_INSERT = "Clinical.sp_ConsultationOrderTestInsert";
        private const string PROC_CONSULTATION_ORDER_TEST_UPDATE = "Clinical.sp_ConsultationOrderTestUpdate";
        private const string PROC_CONSULTATION_ORDER_TEST_DELETE = "Clinical.sp_ConsultationOrderTestDelete";
        private const string PROC_CONSULTATION_ORDER_TEST_SELECT = "Clinical.sp_ConsultationOrderTestSelect";
        private const string PROC_CONSULTATIONORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_ConsultationOrderSelectForSoapText";

        private const string PROC_NOTES_CONSULTATIONORDER_SELECT = "[Clinical].[sp_NotesConsultationOrderSelect]";

        #endregion

        #region Parameter Names
        //Start//17-03-2016//Ahmad Raza//Initializing Parameters
        private const string PARM_CONSULTATION_ORDER_ID = "@ConsultationOrderId";
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
        private const string PARM_CONSULTATION_ORDER_PROBLEM_ID = "@ConsultationOrderProblemId";
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


        private const string PARM_CONSULTATIONORDER_TEST_ID = "@ConsultationOrderTestId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";

        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";

        #endregion

        #region Supporting Functions

        /// <summary>
        /// Method Name: createConsultationOrderParameters
        /// Author: Ahmad Raza
        /// Date: 17-03-2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createConsultationOrderParameters(IDBManager dbManager, DSConsultationOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATION_ORDER_ID, ds.ConsultationOrder.ConsultationOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATION_ORDER_ID, ds.ConsultationOrder.ConsultationOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.ConsultationOrder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROVIDER_ID, ds.ConsultationOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ASSIGNEE_ID, ds.ConsultationOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_ORDER_DATE, ds.ConsultationOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_ORDER_TIME, ds.ConsultationOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.ConsultationOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.ConsultationOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.ConsultationOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.ConsultationOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.ConsultationOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAP_TEXT, ds.ConsultationOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Humaira Yousaf for status
            dbManager.AddInsertUpdateParameters(12, PARM_STATUS, ds.ConsultationOrder.StatusColumn.ColumnName, DbType.String);
            //End 22-03-2016 Humaira Yousaf for status
            dbManager.AddInsertUpdateParameters(13, PARM_NOTE_ID, ds.ConsultationOrder.NoteIdColumn.ColumnName, DbType.Int64);


        }

        /// <summary>
        ///  Method Name: createConsultationOrderProblemParameters
        ///  Author: Farooq Ahmad
        ///  Created Date: 18-03-2016
        ///  Description: insert/update  Consultation Order Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createConsultationOrderProblemParameters(IDBManager dbManager, DSConsultationOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATION_ORDER_PROBLEM_ID, ds.ConsultationOrderProblem.ConsultationOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATION_ORDER_PROBLEM_ID, ds.ConsultationOrderProblem.ConsultationOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CONSULTATION_ORDER_ID, ds.ConsultationOrderProblem.ConsultationOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.ConsultationOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.ConsultationOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.ConsultationOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.ConsultationOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.ConsultationOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.ConsultationOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.ConsultationOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.ConsultationOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion

        #region INSERT/UPDATE/DELETE/SELECT Functions

        /// <summary>
        /// Method Name: insertUpdateConsultationOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: insert/update  consultation Order
        /// </summary> 
        /// <param name="DSConsultationOrder" type="DATASET"></param>
        public DSConsultationOrder insertUpdateConsultationOrder(DSConsultationOrder ds, IDBManager dbManager = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                //DataTable dtTemp = ds.ConsultationOrder.GetChanges();
                //dbManager.BeginTransaction();
                createConsultationOrderParameters(dbManager, ds, true);
                createConsultationOrderParameters(dbManager, ds, false);
                ds = (DSConsultationOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_INSERT, PROC_CONSULTATION_ORDER_UPDATE, ds, ds.ConsultationOrder.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ConsultationOrder.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ConsultationOrder.Rows[i][ds.ConsultationOrder.ConsultationOrderIdColumn];
                //    }
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ConsultationOrder.Rows[0][ds.ConsultationOrder.ConsultationOrderIdColumn].ToString(), null, ds.ConsultationOrder.Rows[0][ds.ConsultationOrder.ConsultationOrderIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALConsultationOrder::insertUpdateConsultationOrder", PROC_CONSULTATION_ORDER_INSERT + " " + PROC_CONSULTATION_ORDER_UPDATE, ex);
                throw ex;

            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        /// <summary>
        /// Method Name: loadConsultationOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: loading Consultation Order 
        /// </summary> 
        /// <param name="consultationOrderId" type="long"></param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSConsultationOrder loadConsultationOrder(long consultationOrderId, long patientId, string procedureName, string orderNumber, long providerId, string orderDateFrom, string orderDateTo, string status, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "", long NoteId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSConsultationOrder ds = new DSConsultationOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(12);

                if (consultationOrderId == 0)
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
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

                //Start 18-03-2016 Humaira Yousaf for Record Count
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ConsultationOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //End 18-03-2016 Humaira Yousaf for Record Count

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

                //Start 21-04-2016 Farooq Ahmad
                if (NoteId == 0)
                    dbManager.AddParameters(11, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(11, PARM_NOTE_ID, NoteId);
                //Start 21-04-2016 Farooq Ahmad

                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_SELECT, ds, ds.ConsultationOrder.TableName);
                if (consultationOrderId > 0)
                {

                    DataTable dtTemp = ds.ConsultationOrder;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ConsultationOrder.Rows[0][ds.ConsultationOrder.ConsultationOrderIdColumn].ToString(), null, ds.ConsultationOrder.Rows[0][ds.ConsultationOrder.ConsultationOrderIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALConsultationOrder::loadConsultationOrder", PROC_CONSULTATION_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteConsultationOrder
        /// Author: Ahmad Raza
        /// Created Date: 17-03-2016
        /// Description: deleting Consultation Order
        /// </summary> 
        /// <param name="consultationOrderId" type="long">consultationOrderId to be deleted</param>
        public string deleteConsultationOrder(long consultationOrderId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                DSConsultationOrder dsCurrentOrder = loadConsultationOrder(consultationOrderId, 0, "", "", 0, "", "", "", 1, 2000, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal=dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.ConsultationOrder;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, consultationOrderId.ToString(), null, consultationOrderId.ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALConsultationOrder::deleteConsultationOrder", PROC_CONSULTATION_ORDER_DELETE, ex);
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

        #endregion

        #region INSERT/DELETE/SELECT Functions ConsultationOrder Problems

        /// <summary>
        /// Method Name: insertUpdateConsultationOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: insert/update  Consultation Order Problems
        /// </summary> 
        /// <param name="DSConsultationOrder" type="DATASET"></param>
        public DSConsultationOrder insertUpdateConsultationOrderProblems(DSConsultationOrder ds, IDBManager dbManager = null)
        {
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                //DataTable dtTemp = ds.ConsultationOrderProblem.GetChanges();
                //dbManager.BeginTransaction();
                createConsultationOrderProblemParameters(dbManager, ds, true);
                createConsultationOrderProblemParameters(dbManager, ds, false);
                ds = (DSConsultationOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_PROBLEM_INSERT, PROC_CONSULTATION_ORDER_PROBLEM_INSERT, ds, ds.ConsultationOrderProblem.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ConsultationOrderProblem.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ConsultationOrderProblem.Rows[i][ds.ConsultationOrderProblem.ConsultationOrderProblemIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ConsultationOrderProblem.Rows[0][ds.ConsultationOrderProblem.ConsultationOrderProblemIdColumn].ToString(), null, ds.ConsultationOrderProblem.Rows[0][ds.ConsultationOrderProblem.ConsultationOrderIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALConsultationOrder::insertUpdateConsultationProblemsOrder", PROC_CONSULTATION_ORDER_PROBLEM_INSERT + " " + PROC_CONSULTATION_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }



        /// <summary>
        /// Method Name: loadConsultationOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: loading Consultation Order Problems
        /// </summary> 
        /// <param name="consultationOrderId" type="long">consultationOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSConsultationOrder loadConsultationOrderProblems(long consultationOrderProblemId, long consultationOrderId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSConsultationOrder ds = new DSConsultationOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (consultationOrderId == 0)
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);

                if (consultationOrderProblemId == 0)
                    dbManager.AddParameters(1, PARM_CONSULTATION_ORDER_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CONSULTATION_ORDER_PROBLEM_ID, consultationOrderProblemId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ConsultationOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_PROBLEM_SELECT, ds, ds.ConsultationOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::loadConsultationOrder", PROC_CONSULTATION_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteConsultationOrderProblem
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: deleting Consultation Order Problem
        /// </summary> 
        /// <param name="consultationOrderId" type="long">consultationOrderId to be deleted</param>
        /// 

        public string deleteConsultationOrderProblems(long consultationOrderId, IDBManager dbManager = null)
        {
            //string returnVal = "";
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //  dbManager.Open();
                //dbManager.BeginTransaction();
                //DSConsultationOrder dsCurrentOrderProblems = loadConsultationOrderProblems(0, Convert.ToInt32(consultationOrderId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_PROBLEM_DELETE);

                //if (returnVal != "")
                //    throw new Exception(returnVal);
                //else
                //{
                //    DataTable dtTemp = dsCurrentOrderProblems.ConsultationOrderProblem;
                //    if (dtTemp != null)
                //    {
                //        if (dsCurrentOrderProblems.ConsultationOrderProblem.Rows.Count > 0)
                //        {
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.ConsultationOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.ConsultationOrderProblem.Rows[0][dsCurrentOrderProblems.ConsultationOrderProblem.ConsultationOrderIdColumn].ToString(), false, false, true);
                //            dsDBAudit.AcceptChanges();
                //        }

                //    }
                //    dbManager.CommitTransaction();
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::deleteConsultationOrderProblem", PROC_CONSULTATION_ORDER_PROBLEM_DELETE, ex);
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
        /// Method Name:  attachConsultationOrderWithNotes
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle attach of Consultation Order with Note 
        /// </summary>
        /// <param name="consultationOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSConsultationOrder attachConsultationOrderWithNotes(string consultationOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSConsultationOrder ds = new DSConsultationOrder();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(consultationOrderId))
                {
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_CONSULTATION_ORDER, ds, ds.ConsultationOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::attachConsultationOrderWithNotes", PROC_ATTACH_CONSULTATION_ORDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: detachConsultationOrderFromNotes
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle detach of Consultation Order from Note 
        /// </summary>
        /// <param name="consultationOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachConsultationOrderFromNotes(string  consultationOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty( consultationOrderId))
                {
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_CONSULTATION_ORDER);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::detachConsultationOrderFromNotes", PROC_DETACH_CONSULTATION_ORDER, ex);
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


        #region"Consultation Order Test"

        private void CreateConsultationOderTestInsertUpdateParameters(IDBManager dbManager, DSConsultationOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(16);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATIONORDER_TEST_ID, ds.ConsultationOrderTest.ConsultationOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(16);
                dbManager.AddInsertUpdateParameters(0, PARM_CONSULTATIONORDER_TEST_ID, ds.ConsultationOrderTest.ConsultationOrderTestIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_CONSULTATION_ORDER_ID, ds.ConsultationOrderTest.ConsultationOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.ConsultationOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.ConsultationOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_TEST_DATE, ds.ConsultationOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TEST_TIME, ds.ConsultationOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_URGENCY_ID, ds.ConsultationOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COMMENTS, ds.ConsultationOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_IS_ACTIVE, ds.ConsultationOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_BY, ds.ConsultationOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CREATED_ON, ds.ConsultationOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_BY, ds.ConsultationOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIED_ON, ds.ConsultationOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_SOAP_TEXT, ds.ConsultationOrderTest.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(14, PARM_CPT_SNOMEDID, ds.ConsultationOrderTest.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_SNOMEDDESCRIPTION, ds.ConsultationOrderTest.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);

        }
        public DSConsultationOrder insertUpdateConsultationOrderTest(DSConsultationOrder ds, IDBManager dbManager = null)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                //DataTable dtTemp = ds.ConsultationOrderTest.GetChanges();
                //dbManager.BeginTransaction();

                CreateConsultationOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateConsultationOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSConsultationOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_TEST_INSERT, PROC_CONSULTATION_ORDER_TEST_UPDATE, ds, ds.ConsultationOrderTest.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.ConsultationOrderTest.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.ConsultationOrderTest.Rows[i][ds.ConsultationOrderTest.ConsultationOrderTestIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ConsultationOrderTest.Rows[0][ds.ConsultationOrderTest.ConsultationOrderTestIdColumn].ToString(), null, ds.ConsultationOrderTest.Rows[0][ds.ConsultationOrderTest.ConsultationOrderIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALConsultationOrder::insertUpdateConsultationOrderTest", PROC_CONSULTATION_ORDER_TEST_INSERT + " " + PROC_CONSULTATION_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }
        public DSConsultationOrder LoadConsultationOrderTest(long consultationOderId, Int32 consultationOderTestId, long patientId, string pageNumber, string rowsPerPage)
        {
            DSConsultationOrder ds = new DSConsultationOrder();
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

                if (consultationOderId == 0)
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOderId);

                if (consultationOderTestId == 0)
                    dbManager.AddParameters(1, PARM_CONSULTATIONORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CONSULTATIONORDER_TEST_ID, consultationOderTestId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ConsultationOrderTest.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_TEST_SELECT, ds, ds.ConsultationOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::LoadConsultationOrderTest", PROC_CONSULTATION_ORDER_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteConsultationOrderTest(long ConsultationOrderTestId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSConsultationOrder dsCurrentOrderTest = LoadConsultationOrderTest(0, Convert.ToInt32(ConsultationOrderTestId), 0, "1", "100");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CONSULTATIONORDER_TEST_ID, ConsultationOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CONSULTATION_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.ConsultationOrderTest;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ConsultationOrderTestId.ToString(), null, dsCurrentOrderTest.ConsultationOrderTest.Rows[0][dsCurrentOrderTest.ConsultationOrderTest.ConsultationOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALConsultationOrder::deleteConsultationOrderTest", PROC_CONSULTATION_ORDER_TEST_DELETE, ex);
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


        /// <summary>
        /// Author: Ahmad Raza
        /// Method Name: loadConsultationOrdersForSoap
        /// Date: 28-06-2016
        /// Desription: This function will load Consultation Orders for soap text
        /// </summary>
        /// <param name="consultationOrderID"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public DSConsultationOrder loadConsultationOrdersForSoap(string consultationOrderID, long patientId)
        {
            DSConsultationOrder ds = new DSConsultationOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (string.IsNullOrEmpty(consultationOrderID))
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONSULTATION_ORDER_ID, consultationOrderID);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);



                ds = (DSConsultationOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONSULTATIONORDER_SELECT_FOR_SOAPTEXT, ds, ds.ConsultationOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::loadConsultationOrdersForSoap", PROC_CONSULTATIONORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region Legacy Notes

        public List<ConsultationOrder> NotesConsultationOrderSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ConsultationOrder> objList_ConsultationOrder = new List<ConsultationOrder>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_CONSULTATIONORDER_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        ConsultationOrder model = new ConsultationOrder();
                        var properties = typeof(ConsultationOrder).GetProperties();
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
                        objList_ConsultationOrder.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALConsultationOrder::NotesConsultationOrderSelect", PROC_NOTES_CONSULTATIONORDER_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ConsultationOrder;
        }

        #endregion Legacy Notes


    }
}