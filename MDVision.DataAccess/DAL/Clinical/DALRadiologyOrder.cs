/* Author:  Muhammad Arshad
 * Created Date: 17/03/2016
 * OverView: Created for RadiologyOrder in Clinical Module
 */

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
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Orders;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALRadiologyOrder
    {
        #region Variable

        #endregion


        #region Stored Procedure Names
        private const string PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_RadiologyOrderSelectForSoapText";
        private const string PROC_RADIOLOGY_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT = "Clinical.sp_RadiologyOrderSelectForSoapTextWithOrderSetId";
        private const string PROC_GET_USER = "Patient.getUsers";
        //Start//17-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_RADIOLOGY_ORDER_INSERT = "Clinical.sp_RadiologyOrderInsert";
        private const string PROC_RADIOLOGY_ORDER_UPDATE = "Clinical.sp_RadiologyOrderUpdate";
        private const string PROC_RADIOLOGY_ORDER_DELETE = "Clinical.sp_RadiologyOrderDelete";
        private const string PROC_RADIOLOGY_ORDER_SELECT = "Clinical.sp_RadiologyOrderSelect";
        //End//17-03-2016//Abid Ali// Store Procedures Names

        //Start//18-03-2016//Farooq Ahmad//Initializing Stored Procedures
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_INSERT = "Clinical.sp_RadiologyOrderProblemInsert";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_UPDATE = "Clinical.sp_RadiologyOrderProblemUpdate";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_DELETE = "Clinical.sp_RadiologyOrderProblemDelete";
        private const string PROC_RADIOLOGY_ORDER_PROBLEM_SELECT = "Clinical.sp_RadiologyOrderProblemSelect";
        //End//18-03-2016//Farooq Ahmad//Initializing Stored Procedures

        private const string PROC_COLLECTEDAT_LOOKUP = "Clinical.sp_CollectedAtLookup";
        private const string PROC_URGENCY_LOOKUP = "Clinical.sp_UrgencyLookup";
        private const string PROC_SPECIMEN_LOOKUP = "Clinical.sp_SpecimenLookup";
        private const string PROC_SPECIMEN_SOURCE_LOOKUP = "Clinical.sp_SpecimenSourceLookup";
        private const string PROC_ORGANISM_LOOKUP = "Clinical.sp_OrganismLookup";
        private const string PROC_ANTIMICROBIAL_BY_SPECIMEN_AND_ORGANISM_LOOKUP = "Clinical.sp_AntimicrobialBySpecimenAndOrganismLookup";
        private const string PROC_VOLUME_LOOKUP = "Clinical.sp_VolumeLookup";
        private const string PROC_DIET_LOOKUP = "Clinical.sp_DietLookup";
        private const string PROC_RADIOLOGYTEST_LOOKUP = "Clinical.sp_RadiologyTestLookup";

        private const string PROC_RADIOLOGY_ORDER_TEST_INSERT = "Clinical.sp_RadiologyOrderTestInsert";
        private const string PROC_RADIOLOGY_ORDER_TEST_UPDATE = "Clinical.sp_RadiologyOrderTestUpdate";
        private const string PROC_RADIOLOGY_ORDER_TEST_DELETE = "Clinical.sp_RadiologyOrderTestDelete";
        private const string PROC_RADIOLOGY_ORDER_TEST_SELECT = "Clinical.sp_RadiologyOrderTestSelect";

        private const string PROC_ATTACH_RADIOLOGY_ORDER = "Clinical.sp_AttachRadiologyOrderWithNotes";
        private const string PROC_DETACH_RADIOLOGY_ORDER = "Clinical.sp_DetachRadiologyOrderFromNotes";

        private const string PROC_NOTES_RADORDER_SELECT = "[Clinical].[sp_NotesRadOrderSelect]";

        private const string PROC_RADORDERTEST_FINDINGS_SELECT = "[Clinical].[sp_RadiologyOrderTestFindingSelect]";

        #endregion



        #region Parameters
        //Start//17-03-2016//Abid Ali// Paramenters Names
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ENTITY_ID = "@EntityId";
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
        private const string PARM_PROVIDER_ID_NOTE = "@ProviderIdNote";
        private const string PARM_RADIOLOGY_ORDER_TEST_ID = "@RadiologyOrderTestId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_COLLECTEDAT_ID = "@CollectedAtId";
        private const string PARM_SPECIMEN_ID = "@SpecimenId";
        private const string PARM_ORGANISM_ID = "@OrganismId";
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
        private const string PARM_FACILITY_TO_ID = "@FacilityTo";
        private const string PARM_REASON = "@Reason";
        private const string PARM_BODY_SITE = "@BodySite";
        private const string PARM_IS_FINDING_UPDATED = "@IsFindingUpdate";
        private const string PARM_NEGATION_REASON_ID = "@NegationReasonId";
        private const string PARM_IS_INCLUDE_COMMENTS = "@IncludeComments";

        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        #endregion


        #region Constructors

        public DALRadiologyOrder()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALRadiologyOrder(SharedVariable SharedVariable)
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


        #region Support Functions for Radiology
        //Start//17-03-2016//Abid Ali// Support functions for Radiology Oder
        /// <summary>
        /// Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateRadiologyOderInsertUpdateParameters(IDBManager dbManager, DSRadiologyOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(26);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_ID, ds.RadiologyOrder.RadiologyOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(26);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_ID, ds.RadiologyOrder.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.RadiologyOrder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_LAB_ID, ds.RadiologyOrder.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FACILITY_ID, ds.RadiologyOrder.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_ID, ds.RadiologyOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_ASSIGNEE_ID, ds.RadiologyOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_RADIOLOGY_ORDER_DATE, ds.RadiologyOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_RADIOLOGY_ORDER_TIME, ds.RadiologyOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_BILLING_TYPE_ID, ds.RadiologyOrder.BillingTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PRIMARY_INSURANCE_ID, ds.RadiologyOrder.PrimaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_SECONDARY_INSURANCE_ID, ds.RadiologyOrder.SecondaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_TERTIARY_INSURANCE_ID, ds.RadiologyOrder.TertiaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_GUARANTOR_ID, ds.RadiologyOrder.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_RELATIONSHIP_ID, ds.RadiologyOrder.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.RadiologyOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.RadiologyOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.RadiologyOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.RadiologyOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.RadiologyOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.RadiologyOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Humaira Yousaf for status
            dbManager.AddInsertUpdateParameters(20, PARM_STATUS, ds.RadiologyOrder.StatusColumn.ColumnName, DbType.String);
            //End 22-03-2016 Humaira Yousaf for status

            dbManager.AddInsertUpdateParameters(21, PARM_NOTE_ID, ds.RadiologyOrder.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_FACILITY_TO_ID, ds.RadiologyOrder.FacilityToColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(23, PARM_COMMENTS, ds.RadiologyOrder.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(24, PARM_NEGATION_REASON_ID, ds.RadiologyOrder.NegationReasonIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(25, PARM_IS_INCLUDE_COMMENTS, ds.RadiologyOrder.IncludeCommentsColumn.ColumnName, DbType.Boolean);
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
        private void createRadiologyOrderProblemParameters(IDBManager dbManager, DSRadiologyOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_PROBLEM_ID, ds.RadiologyOrderProblem.RadiologyOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_PROBLEM_ID, ds.RadiologyOrderProblem.RadiologyOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_RADIOLOGY_ORDER_ID, ds.RadiologyOrderProblem.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.RadiologyOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.RadiologyOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.RadiologyOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.RadiologyOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.RadiologyOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.RadiologyOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.RadiologyOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.RadiologyOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion


        //Start//17-03-2016//Abid Ali// Radiology Oder CRUD Operations
        #region Radiology (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will insert/update Radiology Oder
        public DSRadiologyOrder InsertUpdateRadiologyOrder(DSRadiologyOrder ds, IDBManager dbManager = null)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.RadiologyOrder.GetChanges();
                //dbManager.BeginTransaction();
                this.CreateRadiologyOderInsertUpdateParameters(dbManager, ds, true);
                this.CreateRadiologyOderInsertUpdateParameters(dbManager, ds, false);

                ds = (DSRadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_INSERT, PROC_RADIOLOGY_ORDER_UPDATE, ds, ds.RadiologyOrder.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString(), null, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString());
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
        public DSRadiologyOrder LoadRadiologyOrder(long radiologyOrderId, long patientId, long noteId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, long labId, string isViewOrder = "", string isPrintOrder = "", SharedVariable sharedVariable = null)
        {
            DSRadiologyOrder ds = new DSRadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
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
                    rpp = 15;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(13);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                if (patientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);
                if (page <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.RadiologyOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //Start 21-03-2016 Humaira Yousaf 

                if (test == "")
                    dbManager.AddParameters(6, PARM_TEST, null);
                else
                    dbManager.AddParameters(6, PARM_TEST, test);

                if (orderNo == "")
                    dbManager.AddParameters(7, PARM_ORDERNO, null);
                else
                    dbManager.AddParameters(7, PARM_ORDERNO, orderNo);

                if (providerId == 0)
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, providerId);

                if (orderDateFrom == "")
                    dbManager.AddParameters(9, PARM_ORDERFROM, null);
                else
                    dbManager.AddParameters(9, PARM_ORDERFROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(10, PARM_ORDERTO, null);
                else
                    dbManager.AddParameters(10, PARM_ORDERTO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(11, PARM_STATUS, null);
                else
                    dbManager.AddParameters(11, PARM_STATUS, status);

                if (labId == 0)
                    dbManager.AddParameters(12, PARM_LAB_ID, null);
                else
                    dbManager.AddParameters(12, PARM_LAB_ID, labId);
                //Start 21-03-2016 Humaira Yousaf 

                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT, ds, ds.RadiologyOrder.TableName);
                if (radiologyOrderId > 0)
                {

                    DataTable dtTemp = ds.RadiologyOrder;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString(), null, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString(), isViewAction, isPrintAcion);
                            //dsDBAudit.AcceptChanges();
                            new DBActivityAudit().InsertDBAuditAsync(dtTemp, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString(), null, ds.RadiologyOrder.Rows[0][ds.RadiologyOrder.RadiologyOrderIdColumn].ToString(), isViewAction, isPrintAcion, false, "", "0", sharedVariable);
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //MDVLogger.DALErrorLog("DALRadiologyOrder::LoadRadiologyOrder", PROC_RADIOLOGY_ORDER_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALRadiologyOrder::LoadRadiologyOrder", PROC_RADIOLOGY_ORDER_SELECT);
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
                // dbManager.Open();
                dbManager.BeginTransaction();
                DSRadiologyOrder dsCurrentOrder = LoadRadiologyOrder(radiologyOrderId, 0, 0, "1", "1000", "", "", 0, "", "", "", 0, "", "");
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.RadiologyOrder;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, radiologyOrderId.ToString(), null, radiologyOrderId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALRadiologyOrder::DeleteRadiologyOrder", PROC_RADIOLOGY_ORDER_DELETE, ex);
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
        //End//17-03-2016//Abid Ali// Radiology Oder CRUD Operations


        #region INSERT/DELETE/SELECT Functions RadiologyOrder Problems

        /// <summary>
        /// Method Name: insertUpdateRadiologyOrderProblems
        /// Author: Farooq Ahmad
        /// Created Date: 18-03-2016
        /// Description: insert/update  Radiology Order Problems
        /// </summary> 
        /// <param name="DSRadiologyOrder" type="DATASET"></param>
        public DSRadiologyOrder insertUpdateRadiologyOrderProblems(DSRadiologyOrder ds, IDBManager dbManager = null)
        {
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                //DataTable dtTemp = ds.RadiologyOrderProblem.GetChanges();
                //dbManager.BeginTransaction();

                createRadiologyOrderProblemParameters(dbManager, ds, true);
                createRadiologyOrderProblemParameters(dbManager, ds, false);
                ds = (DSRadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, ds, ds.RadiologyOrderProblem.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.RadiologyOrderProblem.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.RadiologyOrderProblem.Rows[i][ds.RadiologyOrderProblem.RadiologyOrderProblemIdColumn];
                //    }

                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrderProblem.Rows[0][ds.RadiologyOrderProblem.RadiologyOrderProblemIdColumn].ToString(), null, ds.RadiologyOrderProblem.Rows[0][ds.RadiologyOrderProblem.RadiologyOrderIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::insertUpdateRadiologyProblemsOrder", PROC_RADIOLOGY_ORDER_PROBLEM_INSERT + " " + PROC_RADIOLOGY_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
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
        public DSRadiologyOrder loadRadiologyOrderProblems(long radiologyOrderProblemId, long radiologyOrderId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSRadiologyOrder ds = new DSRadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);

                if (radiologyOrderProblemId == 0)
                    dbManager.AddParameters(1, PARM_RADIOLOGY_ORDER_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_RADIOLOGY_ORDER_PROBLEM_ID, radiologyOrderProblemId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.RadiologyOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_SELECT, ds, ds.RadiologyOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::loadRadiologyOrder", PROC_RADIOLOGY_ORDER_PROBLEM_SELECT, ex);
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

        public string deleteRadiologyOrderProblems(long radiologyOrderId, IDBManager dbManager = null)
        {
            //string returnVal = "";
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                //dbManager.BeginTransaction();
                //DSRadiologyOrder dsCurrentOrderProblems = loadRadiologyOrderProblems(0, Convert.ToInt32(radiologyOrderId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_PROBLEM_DELETE);

                //if (returnVal != "")
                //    throw new Exception(returnVal);
                //else
                //{
                //    DataTable dtTemp = dsCurrentOrderProblems.RadiologyOrderProblem;
                //    if (dtTemp != null)
                //    {
                //        if (dsCurrentOrderProblems.RadiologyOrderProblem.Rows.Count > 1)
                //        {
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.RadiologyOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.RadiologyOrderProblem.Rows[0][dsCurrentOrderProblems.RadiologyOrderProblem.RadiologyOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::deleteRadiologyOrderProblem", PROC_RADIOLOGY_ORDER_PROBLEM_DELETE, ex);
                throw ex;
                //string[] str = ex.Message.Split('|');
                //if (str.Length > 1)
                //    return str[1].ToString();
                //else
                //    return ex.Message;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
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
        public DSRadiologyOrder attachRadiologyOrderWithNotes(string radiologyOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSRadiologyOrder ds = new DSRadiologyOrder();

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


                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_RADIOLOGY_ORDER, ds, ds.RadiologyOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::attachRadiologyOrderWithNotes", PROC_ATTACH_RADIOLOGY_ORDER, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::detachRadiologyOrderFromNotes", PROC_DETACH_RADIOLOGY_ORDER, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupAllergyType", PROC_COLLECTEDAT_LOOKUP, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupUrgency", PROC_URGENCY_LOOKUP, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupSpecimen", PROC_SPECIMEN_LOOKUP, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupSpecimen", PROC_SPECIMEN_LOOKUP, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupVolume", PROC_VOLUME_LOOKUP, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupDiet", PROC_DIET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRadiologyOrderLookup lookupSpecimenSource(long SpecimenId)
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (SpecimenId <= 0)
                {
                    dbManager.AddParameters(0, PARM_SPECIMEN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SPECIMEN_ID, SpecimenId);
                }


                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIMEN_SOURCE_LOOKUP, ds, ds.SpecimenSource.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupSpecimenSource", PROC_SPECIMEN_SOURCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup lookupOrganism()
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORGANISM_LOOKUP, ds, ds.Organism.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupOrganism", PROC_ORGANISM_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrderLookup lookupAntimicrobialBySpecimentAndOrganism(long SpecimenId, long OrganismId)
        {
            DSRadiologyOrderLookup ds = new DSRadiologyOrderLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (SpecimenId <= 0)
                {
                    dbManager.AddParameters(0, PARM_SPECIMEN_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SPECIMEN_ID, SpecimenId);
                }
                if (OrganismId <= 0)
                {
                    dbManager.AddParameters(1, PARM_ORGANISM_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ORGANISM_ID, OrganismId);
                }

                ds = (DSRadiologyOrderLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANTIMICROBIAL_BY_SPECIMEN_AND_ORGANISM_LOOKUP, ds, ds.Antimicrobial.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::lookupAntimicrobialBySpecimentAndOrganism", PROC_ANTIMICROBIAL_BY_SPECIMEN_AND_ORGANISM_LOOKUP, ex);
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
        private void CreateRadiologyOderTestInsertUpdateParameters(IDBManager dbManager, DSRadiologyOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, ds.RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, ds.RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_RADIOLOGY_ORDER_ID, ds.RadiologyOrderTest.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.RadiologyOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_TEST_DATE, ds.RadiologyOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TEST_TIME, ds.RadiologyOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_URGENCY_ID, ds.RadiologyOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COLLECTEDAT_ID, ds.RadiologyOrderTest.CollectedAtIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_SPECIMEN_ID, ds.RadiologyOrderTest.SpecimenIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_VOLUME_LENGTH, ds.RadiologyOrderTest.VolumeLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_VOLUME_ID, ds.RadiologyOrderTest.VolumeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(11, PARM_PATIENT_INSTRUCTION, ds.RadiologyOrderTest.PatientInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_FILLER_INSTRUCTION, ds.RadiologyOrderTest.FillerInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_COMMENTS, ds.RadiologyOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.RadiologyOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.RadiologyOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.RadiologyOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.RadiologyOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.RadiologyOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.RadiologyOrderTest.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(20, PARM_CPT_SNOMEDID, ds.RadiologyOrderTest.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_CPT_SNOMEDDESCRIPTION, ds.RadiologyOrderTest.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_REASON, ds.RadiologyOrderTest.ReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_BODY_SITE, ds.RadiologyOrderTest.BodySiteColumn.ColumnName, DbType.String);

        }
        public DSRadiologyOrder insertUpdateRadiologyOrderTest(DSRadiologyOrder ds, IDBManager dbManager = null)
        {
            bool IsdbManagerNull = false;
            DataTable dtTemp = null;
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                IsdbManagerNull = true;
            }
            try
            {
                if (IsdbManagerNull)
                {
                    dbManager.Open();
                    dtTemp = ds.RadiologyOrderTest.GetChanges();
                    dbManager.BeginTransaction();
                }
                CreateRadiologyOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateRadiologyOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSRadiologyOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_INSERT, PROC_RADIOLOGY_ORDER_TEST_UPDATE, ds, ds.RadiologyOrderTest.TableName);
                if (IsdbManagerNull)
                {
                    if (dtTemp != null)
                    {
                        dtTemp.Columns.Add("PrimaryKey");
                        for (int i = 0; i < ds.RadiologyOrderTest.Rows.Count; i++)
                        {
                            dtTemp.Rows[i]["PrimaryKey"] = ds.RadiologyOrderTest.Rows[i][ds.RadiologyOrderTest.RadiologyOrderTestIdColumn];
                        }
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrderTest.Rows[0][ds.RadiologyOrderTest.RadiologyOrderTestIdColumn].ToString(), null, ds.RadiologyOrderTest.Rows[0][ds.RadiologyOrderTest.RadiologyOrderIdColumn].ToString());
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                if (IsdbManagerNull)
                {
                    dsDBAudit.RejectChanges();
                    dbManager.RollBackTransaction();
                }
                MDVLogger.DALErrorLog("DALRadiologyOrder::insertUpdateRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_INSERT + " " + PROC_RADIOLOGY_ORDER_TEST_UPDATE, ex);
                throw ex;
            }
            finally
            {
                if (IsdbManagerNull)
                {
                    dbManager.Dispose();
                }
            }
        }

        public DSRadiologyOrder LoadRadiologyOrderTest(long radiologyOrderId, Int32 radiologyOrderTestId, long patientId, string pageNumber, string rowsPerPage, Int64 noteId = 0, SharedVariable sharedVariable = null)
        {
            DSRadiologyOrder ds = new DSRadiologyOrder();
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
                dbManager.CreateParameters(7);

                if (radiologyOrderId == 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);

                if (radiologyOrderTestId == 0)
                    dbManager.AddParameters(1, PARM_RADIOLOGY_ORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_RADIOLOGY_ORDER_TEST_ID, radiologyOrderTestId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.RadiologyOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (noteId <= 0)
                    dbManager.AddParameters(6, PARM_NOTE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_NOTE_ID, noteId);

                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_SELECT, ds, ds.RadiologyOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALRadiologyOrder::LoadRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALRadiologyOrder::LoadRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_SELECT);
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
                DSRadiologyOrder dsCurrentOrderTest = LoadRadiologyOrderTest(0, Convert.ToInt32(RadiologyOrderTestId), 0, "1", "100");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_TEST_ID, RadiologyOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.RadiologyOrderTest;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, RadiologyOrderTestId.ToString(), null, dsCurrentOrderTest.RadiologyOrderTest.Rows[0][dsCurrentOrderTest.RadiologyOrderTest.RadiologyOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::deleteRadiologyOrderTest", PROC_RADIOLOGY_ORDER_TEST_DELETE, ex);
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
                MDVLogger.DALErrorLog("DALRadiologyOrder::LookupRadiologyTestReport", PROC_RADIOLOGYTEST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<OrderFindingsModel> LoadRadiologyOrderTestFindings(long radiologyOderId, bool isFindingUpdate = false, long notesId = 0)
        {
            List<OrderFindingsModel> listModel = new List<OrderFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (radiologyOderId <= 0)
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOderId);

                dbManager.AddParameters(1, PARM_IS_FINDING_UPDATED, isFindingUpdate);

                if (notesId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, notesId);

                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_RADORDERTEST_FINDINGS_SELECT);
                OrderFindingsModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new OrderFindingsModel();
                    modelFill.TestName = MDVUtility.CheckStringNull(reader["TestName"]);
                    modelFill.SystemName = MDVUtility.CheckStringNull(reader["SystemName"]);
                    modelFill.Findings = MDVUtility.CheckStringNull(reader["Findings"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::LoadRadiologyOrderTestFindings", PROC_RADORDERTEST_FINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public DSRadiologyOrder loadRadiologyOrdersForSoap(string radiologyOrderId, long patientId, long ProviderId)
        {
            DSRadiologyOrder ds = new DSRadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (string.IsNullOrEmpty(radiologyOrderId))
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RADIOLOGY_ORDER_ID, radiologyOrderId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID_NOTE, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID_NOTE, ProviderId);


                List<string> tableNames = new List<string>
                {
                        ds.RadiologyOrder.TableName,
                        ds.RadiologyOrderTest.TableName,
                        ds.RadiologyOrderProblem.TableName
                };

                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::loadRadiologyOrdersForSoap", PROC_RADIOLOGY_ORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRadiologyOrder GetDiagnosticImagingOrdersForSoapTextByOrderSetId(long OrderSetID, long notesId, long patientid, long providerId)
        {
            DSRadiologyOrder ds = new DSRadiologyOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, OrderSetID);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientid);
                dbManager.AddParameters(2, PARM_PROVIDER_ID, providerId);
                dbManager.AddParameters(3, PARM_NOTE_ID, notesId);
                List<string> tableNames = new List<string>
                {
                        ds.RadiologyOrder.TableName,
                        ds.RadiologyOrderTest.TableName,
                        ds.RadiologyOrderProblem.TableName
                };
                ds = (DSRadiologyOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGY_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::GetDiagnosticImagingOrdersForSoapTextByOrderSetId", PROC_RADIOLOGY_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSUsers LookupUsers(string UserName)
        {
            DSUsers ds = new DSUsers();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_USER_NAME, UserName);

                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_USER, ds, ds.UserLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::LookupUsers", PROC_GET_USER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region Legacy Notes

        public List<RadOrder> NotesRadOrderSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<RadOrder> objList_RadOrder = new List<RadOrder>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_RADORDER_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        RadOrder model = new RadOrder();
                        var properties = typeof(RadOrder).GetProperties();
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
                        objList_RadOrder.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyOrder::NotesRadOrderSelect", PROC_NOTES_RADORDER_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_RadOrder;
        }

        #endregion Legacy Notes

    }
}
