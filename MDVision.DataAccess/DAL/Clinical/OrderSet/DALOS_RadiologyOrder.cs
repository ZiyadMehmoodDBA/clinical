using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
    public class DALOS_RadiologyOrder
    {
        #region Variable

        #endregion


        #region Stored Procedure Names
        private const string PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_RadiologyOrderSelectForSoapText";
        //Start//17-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_RADIOLOGY_ORDER_INSERT = "Clinical.sp_OS_RadiologyOrderInsert";
        private const string PROC_RADIOLOGY_ORDER_UPDATE = "Clinical.sp_OS_RadiologyOrderUpdate";
        private const string PROC_RADIOLOGY_ORDER_DELETE = "Clinical.sp_OS_RadiologyOrderDelete";
        private const string PROC_RADIOLOGY_ORDER_SELECT = "Clinical.sp_OS_RadiologyOrderSelect";
        private const string PROC_RADIOLOGY_ORDER_FILL = "Clinical.sp_OS_RadiologyOrderSelectFill";
        //End//17-03-2016//Abid Ali// Store Procedures Names

        //Start//18-03-2016//Farooq Ahmad//Initializing Stored Procedures
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_INSERT = "Clinical.sp_OS_RadiologyOrderProblemInsert";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_UPDATE = "Clinical.sp_OS_RadiologyOrderProblemUpdate";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_DELETE = "Clinical.sp_OS_RadiologyOrderProblemDelete";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_SELECT = "Clinical.sp_OS_RadiologyOrderProblemSelect";
        //End//18-03-2016//Farooq Ahmad//Initializing Stored Procedures

        private const string PROC_COLLECTEDAT_LOOKUP = "Clinical.sp_CollectedAtLookup";
        private const string PROC_URGENCY_LOOKUP = "Clinical.sp_UrgencyLookup";
        private const string PROC_SPECIMEN_LOOKUP = "Clinical.sp_SpecimenLookup";
        private const string PROC_VOLUME_LOOKUP = "Clinical.sp_VolumeLookup";
        private const string PROC_DIET_LOOKUP = "Clinical.sp_DietLookup";
        private const string PROC_RADIOLOGYTEST_LOOKUP = "Clinical.sp_RadiologyTestLookup";

        private const string PROC_RADIOLOGY_ORDER_TEST_INSERT = "Clinical.sp_OS_RadiologyOrderTestInsert";
        private const string PROC_RADIOLOGY_ORDER_TEST_UPDATE = "Clinical.sp_OS_RadiologyOrderTestUpdate";
        private const string PROC_RADIOLOGY_ORDER_TEST_DELETE = "Clinical.sp_OS_RadiologyOrderTestDelete";
        private const string PROC_RADIOLOGY_ORDER_TEST_SELECT = "Clinical.sp_OS_RadiologyOrderTestSelect";
        private const string PROC_RADIOLOGY_ORDER_TEST_FILL = "Clinical.sp_OS_RadiologyOrderTestFill";

        private const string PROC_ATTACH_RADIOLOGY_ORDER = "Clinical.sp_AttachRadiologyOrderWithNotes";
        private const string PROC_DETACH_RADIOLOGY_ORDER = "Clinical.sp_DetachRadiologyOrderFromNotes";
        #endregion



        #region Parameters
        //Start//17-03-2016//Abid Ali// Paramenters Names
        private const string PARM_RADIOLOGY_ORDER_ID = "@RadiologyOrderId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_RADIOLOGY_ORDER_DATE = "@OrderDate";
        private const string PARM_RADIOLOGY_ORDER_TIME = "@OrderTime";
        private const string PARM_BILLING_TYPE_ID = "@BillingTypeId";
        private const string PARM_PRIMARY_INSURANCE_ID = "@PrimaryInsuraceId";
        private const string PARM_SECONDARY_INSURANCE_ID = "@SecondaryInsuraceId";
        private const string PARM_TERTIARY_INSURANCE_ID = "@TertiaryInsuraceId";
        private const string PARM_GUARANTOR_ID = "@GuarantorId";
        private const string PARM_RELATIONSHIP_ID = "@RelationShipId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        //End//17-03-2016//Abid Ali// Paramenters Names

        //Start//18-03-2016//Farooq Ahmad//Initializing Parameters
        private const string PARM_RADIOLOGY_ORDER_PROBLEM_ID = "@RadiologyOrderProblemId";
        private const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_COMMENTS = "@Comments";
        //End//18-03-2016//Farooq Ahmad//Initializing Parameters
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_TESTCODE = "@TestCode";
        private const string PARM_SPECIMENTYPE = "@SpecimenType";
        //Start 21-03-2016 Humaira Yousaf

        private const string PARM_RADIOLOGY_ORDER_TEST_ID = "@RadiologyOrderTestId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_COLLECTEDAT_ID = "@CollectedAtId";
        private const string PARM_SPECIMEN_ID = "@SpecimenId";
        private const string PARM_VOLUME_LENGTH = "@VolumeLength";
        private const string PARM_VOLUME_ID = "@VolumeId";
        private const string PARM_PATIENT_INSTRUCTION = "@PatientInstruction";
        private const string PARM_FILLER_INSTRUCTION = "@FillerInstruction";
        private const string PARM_TEST = "@Test";
        private const string PARM_ORDERNO = "@OrderNo";
        private const string PARM_ORDERFROM = "@OrderDateFrom";
        private const string PARM_ORDERTO = "@OrderDateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        private const string PARM_REASON = "@Reason";
        private const string PARM_BODY_SITE = "@BodySite";

        #endregion


        #region Constructors

        public DALOS_RadiologyOrder()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion


        #region Support Functions for Radiology
        //Start//17-03-2016//Abid Ali// Support functions for Radiology Oder
        /// <summary>
        /// Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateRadiologyOderInsertUpdateParameters(IDBManager dbManager, DSOS_RadiologyOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(22);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_ID, ds.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(22);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_ID, ds.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_ORDER_SET_ID, ds.OS_RadiologyOrder.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_LAB_ID, ds.OS_RadiologyOrder.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FACILITY_ID, ds.OS_RadiologyOrder.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_ID, ds.OS_RadiologyOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_ASSIGNEE_ID, ds.OS_RadiologyOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_RADIOLOGY_ORDER_DATE, ds.OS_RadiologyOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_RADIOLOGY_ORDER_TIME, ds.OS_RadiologyOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_BILLING_TYPE_ID, ds.OS_RadiologyOrder.BillingTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PRIMARY_INSURANCE_ID, ds.OS_RadiologyOrder.PrimaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_SECONDARY_INSURANCE_ID, ds.OS_RadiologyOrder.SecondaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_TERTIARY_INSURANCE_ID, ds.OS_RadiologyOrder.TertiaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_GUARANTOR_ID, ds.OS_RadiologyOrder.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_RELATIONSHIP_ID, ds.OS_RadiologyOrder.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.OS_RadiologyOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.OS_RadiologyOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.OS_RadiologyOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.OS_RadiologyOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.OS_RadiologyOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.OS_RadiologyOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Humaira Yousaf for status
            dbManager.AddInsertUpdateParameters(20, PARM_STATUS, ds.OS_RadiologyOrder.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_COMMENTS, ds.OS_RadiologyOrder.CommentsColumn.ColumnName, DbType.String);
            //End 22-03-2016 Humaira Yousaf for status

        }
        //End//17-03-2016//Abid Ali// Support functions for Radiology


        /// <summary>
        ///  Method Name: createRadiologyOrderProblemParameters
        ///  Author: Farooq Ahmad
        ///  Created Date: 18-03-2016
        ///  Description: insert/update  Radiology Order Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createRadiologyOrderProblemParameters(IDBManager dbManager, DSOS_RadiologyOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_PROBLEM_ID, ds.OS_RadiologyOrderProblem.RadiologyOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_PROBLEM_ID, ds.OS_RadiologyOrderProblem.RadiologyOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_RADIOLOGY_ORDER_ID, ds.OS_RadiologyOrderProblem.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.OS_RadiologyOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.OS_RadiologyOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.OS_RadiologyOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.OS_RadiologyOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.OS_RadiologyOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.OS_RadiologyOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.OS_RadiologyOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.OS_RadiologyOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion


        //Start//17-03-2016//Abid Ali// Radiology Oder CRUD Operations
        #region Radiology (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will insert/update Radiology Oder
        public DSOS_RadiologyOrder InsertUpdateRadiologyOrder(DSOS_RadiologyOrder ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.OS_RadiologyOrder.GetChanges();
                dbManager.BeginTransaction();
                this.CreateRadiologyOderInsertUpdateParameters(dbManager, ds, true);
                this.CreateRadiologyOderInsertUpdateParameters(dbManager, ds, false);

                ds = (DSOS_RadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_INSERT, PROC_RADIOLOGY_ORDER_UPDATE, ds, ds.OS_RadiologyOrder.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.OS_RadiologyOrder.Rows[0][ds.OS_RadiologyOrder.RadiologyOrderIdColumn].ToString(), null, ds.OS_RadiologyOrder.Rows[0][ds.OS_RadiologyOrder.RadiologyOrderIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                string Params_Insert = " INSERT PARAMETERS";
                string Params_Update = " UPDATE PARAMETERS";
                for (int i = 0; i < dbManager.InsertParameters.Length; i++)
                {
                    Params_Insert = Params_Insert + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                for (int i = 0; i < dbManager.UpdateParameters.Length; i++)
                {
                    Params_Update = Params_Update + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }

                MDVLogger.DALErrorLog("DALRdiologyOrder::PROC_RdiologyOrder_INSERT", PROC_RADIOLOGY_ORDER_INSERT + " " + PROC_RADIOLOGY_ORDER_UPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will load Radiology Order
        public DSOS_RadiologyOrder LoadRadiologyOrder(long orderSetId, string pageNumber, string rowsPerPage)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
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
                dbManager.CreateParameters(4);

                if (orderSetId <= 0)
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, orderSetId);
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, page);
                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rpp);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.OS_RadiologyOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT, ds, ds.OS_RadiologyOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::LoadRadiologyOrder", PROC_RADIOLOGY_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will delete Radiology Order
        public string DeleteRadiologyOrder(long radiologyOrderId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DSOS_RadiologyOrder dsCurrentOrder = FillRadiologyOrder(radiologyOrderId);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_DELETE).ToString();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::DeleteRadiologyOrder", PROC_RADIOLOGY_ORDER_DELETE, ex);
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
        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will load Radiology Order
        public DSOS_RadiologyOrder FillRadiologyOrder(long radiologyOrderId)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_FILL, ds, ds.OS_RadiologyOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::FillRadiologyOrder", PROC_RADIOLOGY_ORDER_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        //End//17-03-2016//Abid Ali// Radiology Oder CRUD Operations


        #region INSERT/DELETE/SELECT Functions RadiologyOrder Problems

        /// <summary>
        /// Method Name: insertUpdateRadiologyOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: insert/update  Radiology Order Problems
        /// </summary>
        /// <param name="DSOS_RadiologyOrder" type="DATASET"></param>
        public DSOS_RadiologyOrder insertUpdateRadiologyOrderProblems(DSOS_RadiologyOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.OS_RadiologyOrderProblem.GetChanges();
                dbManager.BeginTransaction();

                createRadiologyOrderProblemParameters(dbManager, ds, true);
                createRadiologyOrderProblemParameters(dbManager, ds, false);
                ds = (DSOS_RadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, ds, ds.OS_RadiologyOrderProblem.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.OS_RadiologyOrderProblem.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.OS_RadiologyOrderProblem.Rows[i][ds.OS_RadiologyOrderProblem.RadiologyOrderProblemIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.OS_RadiologyOrderProblem.Rows[0][ds.OS_RadiologyOrderProblem.RadiologyOrderProblemIdColumn].ToString(), null, ds.OS_RadiologyOrderProblem.Rows[0][ds.OS_RadiologyOrderProblem.RadiologyOrderIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::insertUpdateRadiologyProblemsOrder", PROC_RADIOLOGY_ORDER_PROBLEM_INSERT + " " + PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Method Name: loadRadiologyOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: loading Radiology Order Problems
        /// </summary>
        /// <param name="radiologyOrderId" type="long">radiologyOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSOS_RadiologyOrder loadRadiologyOrderProblems(long radiologyOrderId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int page;
                int rpp;
                if (pageNumber <= 0)
                {
                    page = 1;
                }
                else
                {
                    page = pageNumber;
                }

                if (rowsPerPage <= 0)
                {
                    rpp = 2000;
                }
                else
                {
                    rpp = rowsPerPage;
                }

                dbManager.Open();
                dbManager.CreateParameters(4);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, page);
                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.OS_RadiologyOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_SELECT, ds, ds.OS_RadiologyOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::loadRadiologyOrder", PROC_RADIOLOGY_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteRadiologyOrderProblem
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: deleting Radiology Order Problem
        /// </summary>
        /// <param name="radiologyOrderId" type="long">radiologyOrderId to be deleted</param>
        ///

        public string deleteRadiologyOrderProblems(long radiologyOrderId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSOS_RadiologyOrder dsCurrentOrderProblems = loadRadiologyOrderProblems(0, Convert.ToInt32(radiologyOrderId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderProblems.OS_RadiologyOrderProblem;
                    if (dtTemp != null)
                    {
                        if (dsCurrentOrderProblems.OS_RadiologyOrderProblem.Rows.Count > 1)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.OS_RadiologyOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.OS_RadiologyOrderProblem.Rows[0][dsCurrentOrderProblems.OS_RadiologyOrderProblem.RadiologyOrderIdColumn].ToString(), false, false, true);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::deleteRadiologyOrderProblem", PROC_RADIOLOGY_ORDER_PROBLEM_DELETE, ex);
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

        #region Association with Notes

        /// <summary>
        /// Method Name:  attachRadiologyrderWithNotes
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle attach of Radiology Order with Note
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public DSOS_RadiologyOrder attachRadiologyOrderWithNotes(string radiologyOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(radiologyOrderId))
                {
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_RADIOLOGY_ORDER, ds, ds.OS_RadiologyOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::attachRadiologyOrderWithNotes", PROC_ATTACH_RADIOLOGY_ORDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: detachRadiologyOrderFromNotes
        /// Author:  Ahmad Raza
        /// Date:    17-03-2016
        /// Reason:  This function will handle detach of Radiology Order from Note
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public string detachRadiologyOrderFromNotes(string radiologyOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(radiologyOrderId))
                {
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_RADIOLOGY_ORDER);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::detachRadiologyOrderFromNotes", PROC_DETACH_RADIOLOGY_ORDER, ex);
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

        //Start //18/03/2016 Muhammad Irfan Lookups Radiology
        #region"Lookups Radiology"
        public DSRadiologyOrderLookup lookupCollectedAt()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COLLECTEDAT_LOOKUP, ds, ds.CollectedAt.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupAllergyType", PROC_COLLECTEDAT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup lookupUrgency()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_URGENCY_LOOKUP, ds, ds.Urgency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupUrgency", PROC_URGENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRadiologyOrderLookup lookupSpecimen()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIMEN_LOOKUP, ds, ds.Specimen.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupSpecimen", PROC_SPECIMEN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRadiologyOrderLookup lookupSpecimen(string TestCode, string SpecimenType, long LabId)
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(TestCode))
                {
                    dbManager.AddParameters(0, PARM_TESTCODE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_TESTCODE, TestCode);
                }

                if (string.IsNullOrEmpty(SpecimenType))
                {
                    dbManager.AddParameters(1, PARM_SPECIMENTYPE, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_SPECIMENTYPE, SpecimenType);
                }

                if (LabId <= 0)
                {
                    dbManager.AddParameters(2, PARM_LAB_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_LAB_ID, LabId);
                }


                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIMEN_LOOKUP, ds, ds.Specimen.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupSpecimen", PROC_SPECIMEN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup lookupVolume()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VOLUME_LOOKUP, ds, ds.Volume.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupVolume", PROC_VOLUME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup lookupDiet()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DIET_LOOKUP, ds, ds.Diet.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::lookupDiet", PROC_DIET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        //End //18/03/2016 Muhammad Irfan Lookups Radiology

        #region "Radiology Order Test"
        private void CreateRadiologyOderTestInsertUpdateParameters(IDBManager dbManager, DSOS_RadiologyOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, ds.OS_RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, ds.OS_RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_RADIOLOGY_ORDER_ID, ds.OS_RadiologyOrderTest.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.OS_RadiologyOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.OS_RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_TEST_DATE, ds.OS_RadiologyOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TEST_TIME, ds.OS_RadiologyOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_URGENCY_ID, ds.OS_RadiologyOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COLLECTEDAT_ID, ds.OS_RadiologyOrderTest.CollectedAtIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_SPECIMEN_ID, ds.OS_RadiologyOrderTest.SpecimenIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_VOLUME_LENGTH, ds.OS_RadiologyOrderTest.VolumeLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_VOLUME_ID, ds.OS_RadiologyOrderTest.VolumeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(11, PARM_PATIENT_INSTRUCTION, ds.OS_RadiologyOrderTest.PatientInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_FILLER_INSTRUCTION, ds.OS_RadiologyOrderTest.FillerInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_COMMENTS, ds.OS_RadiologyOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.OS_RadiologyOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.OS_RadiologyOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.OS_RadiologyOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.OS_RadiologyOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.OS_RadiologyOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.OS_RadiologyOrderTest.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(20, PARM_CPT_SNOMEDID, ds.OS_RadiologyOrderTest.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_CPT_SNOMEDDESCRIPTION, ds.OS_RadiologyOrderTest.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_REASON, ds.OS_RadiologyOrderTest.ReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_BODY_SITE, ds.OS_RadiologyOrderTest.BodySiteColumn.ColumnName, DbType.String);

        }
        public DSOS_RadiologyOrder insertUpdateRadiologyOrderTest(DSOS_RadiologyOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.Open();
                CreateRadiologyOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateRadiologyOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSOS_RadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_INSERT, PROC_RADIOLOGY_ORDER_TEST_UPDATE, ds, ds.OS_RadiologyOrderTest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::insertUpdateRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_INSERT + " " + PROC_RADIOLOGY_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSOS_RadiologyOrder FillRadiologyOrderTest(Int32 radiologyOrderTestId)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (radiologyOrderTestId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, radiologyOrderTestId);


                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_FILL, ds, ds.OS_RadiologyOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::FillRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSOS_RadiologyOrder LoadRadiologyOrderTest(long radiologyOrderId, string pageNumber, string rowsPerPage)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
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
                dbManager.CreateParameters(4);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                if (page <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.OS_RadiologyOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_SELECT, ds, ds.OS_RadiologyOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::LoadRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteRadiologyOrderTest(long RadiologyOrderTestId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSOS_RadiologyOrder dsCurrentOrderTest = FillRadiologyOrderTest(Convert.ToInt32(RadiologyOrderTestId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, RadiologyOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.OS_RadiologyOrderTest;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, RadiologyOrderTestId.ToString(), null, dsCurrentOrderTest.OS_RadiologyOrderTest.Rows[0][dsCurrentOrderTest.OS_RadiologyOrderTest.RadiologyOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::deleteRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_DELETE, ex);
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


        public List<RadiologyTestLookup> LookupRadiologyTestReport(string Test)
        {
            List<RadiologyTestLookup> listModel = new List<RadiologyTestLookup>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(Test))
                    dbManager.AddParameters(0, PARM_TEST, null);
                else
                    dbManager.AddParameters(0, PARM_TEST, Test);
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RADIOLOGYTEST_LOOKUP);
                RadiologyTestLookup modelFill = null;
                while (reader.Read())
                {
                    modelFill = new RadiologyTestLookup();
                    modelFill.LOINC = MDVUtility.CheckStringNull(reader["LOINC"]);
                    modelFill.LOINCDescription = MDVUtility.CheckStringNull(reader["LOINCDescription"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::LookupRadiologyTestReport", PROC_RADIOLOGYTEST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public DSOS_RadiologyOrder loadRadiologyOrdersForSoap(string radiologyOrderId, long patientId)
        {
            DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (string.IsNullOrEmpty(radiologyOrderId))
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, patientId);



                ds = (DSOS_RadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT, ds, ds.OS_RadiologyOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_RadiologyOrder::loadRadiologyOrdersForSoap", PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




    }
}
