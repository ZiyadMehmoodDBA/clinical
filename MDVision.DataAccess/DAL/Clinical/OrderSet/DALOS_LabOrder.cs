/* Author:  Muhammad Arshad
 * Created Date: 31/03/2016
 * OverView: Created for LabOrder in Clinical Module
 */

using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALOS_LabOrder
    {
        #region Variable

        #endregion


        #region Stored Procedure Names
        //Start//31-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_Lab_ORDER_INSERT = "Clinical.sp_OS_LabOrderInsert";
        private const string PROC_Lab_ORDER_UPDATE = "Clinical.sp_OS_LabOrderUpdate";
        private const string PROC_Lab_ORDER_DELETE = "Clinical.sp_OS_LabOrderDelete";
        private const string PROC_Lab_ORDER_SELECT = "Clinical.sp_OS_LabOrderSelect";
        private const string PROC_Lab_ORDER_FILL = "Clinical.sp_OS_LabOrderFILL";
        //End//31-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_Lab_ORDER_SELECTPDF = "Clinical.sp_LabOrderSelectPDF";
        //Start//31-03-2016//Abid Ali//Initializing Stored Procedures
        private const string PROC_Lab_ORDER_PROBLEM_INSERT = "Clinical.sp_OS_LabOrderProblemInsert";
        private const string PROC_Lab_ORDER_PROBLEM_UPDATE = "Clinical.sp_OS_LabOrderProblemUpdate";
        private const string PROC_Lab_ORDER_PROBLEM_DELETE = "Clinical.sp_OS_LabOrderProblemDelete";
        private const string PROC_Lab_ORDER_PROBLEM_SELECT = "Clinical.sp_OS_LabOrderProblemSelect";
        //End//31-03-2016//Abid Ali//Initializing Stored Procedures

        private const string PROC_COLLECTEDAT_LOOKUP = "Clinical.sp_CollectedAtLookup";
        private const string PROC_URGENCY_LOOKUP = "Clinical.sp_UrgencyLookup";
        private const string PROC_SPECIMEN_LOOKUP = "Clinical.sp_SpecimenLookup";
        private const string PROC_VOLUME_LOOKUP = "Clinical.sp_VolumeLookup";
        private const string PROC_DIET_LOOKUP = "Clinical.sp_DietLookup";

        private const string PROC_Lab_ORDER_TEST_INSERT = "Clinical.sp_OS_LabOrderTestInsert";
        private const string PROC_Lab_ORDER_TEST_UPDATE = "Clinical.sp_OS_LabOrderTestUpdate";
        private const string PROC_Lab_ORDER_TEST_DELETE = "Clinical.sp_OS_LabOrderTestDelete";
        private const string PROC_Lab_ORDER_TEST_SELECT = "Clinical.sp_OS_LabOrderTestSelect";

        private const string PROC_ATTACH_Lab_ORDER = "Clinical.sp_AttachLabOrderWithNotes";
        private const string PROC_DETACH_Lab_ORDER = "Clinical.sp_DetachLabOrderFromNotes";
        private const string PROC_LABORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_LabOrderSelectForSoapText";
        private const string PROC_Lab_ORDER_SELECT_PDF = "Clinical.sp_LabOrderSelectPDF";

        private const string PROC_LAB_TEST_LOOKUP = "Clinical.sp_LabTestLookup";
        private const string PROC_Lab_ORDER_AOE_SELECT = "Clinical.sp_LabOrderAOESelect";
        private const string PROC_LAB_TEST_ABN = "Clinical.sp_LabOrderABNInsert";
        private const string PROC_LAB_ORDER_ABN_TEST_SELECT = "Clinical.sp_LabOrderABNSelect";


        private const string PROC_Lab_ORDER_AOE_ANSWERS_SELECT = "Clinical.sp_OS_LabOrderAOEAnswersSelect";
        private const string PROC_Lab_ORDER_AOE_ANSERWS_INSERT = "Clinical.sp_OS_LabOrderAOEAnswersInsert";
        private const string PROC_Lab_ORDER_AOE_ANSERWS_UPDATE = "Clinical.sp_OS_LabOrderAOEAnswersUpdate";
        private const string PROC_LAB_ORDER_CHANGE_PATIENT = "Clinical.sp_LabOrderChangePatient";

        #region MK, Insurance ExternalInformation

        private const string PROC_LAB_ORDER_EXTERNAL_PATIENT_INSURANCE_PLAN_ADDRESS = "Patient.sp_ExternalPatientInsuarnce_Plan_Address";

        #endregion

        #endregion



        #region Parameters
        //Start//31-03-2016//Abid Ali// Paramenters Names

        private const string PARM_Lab_ORDER_ID = "@LabOrderId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_Entity_ID = "@EntityId";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_Lab_ORDER_DATE = "@OrderDate";
        private const string PARM_Lab_ORDER_TIME = "@OrderTime";
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
        private const string PARM_SAMPLESTORAGE = "@SampleStorage"; 
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_ALTSPECIMENID = "@AltSpecimenId";
        //End//31-03-2016//Abid Ali// Paramenters Names

        //Start//31-03-2016//Abid Ali//Initializing Parameters
        private const string PARM_Lab_ORDER_PROBLEM_ID = "@LabOrderProblemId";
        private const string PARM_PROBLEM_ID = "@ProblemId";
        private const string PARM_COMMENTS = "@Comments";
        //End//31-03-2016//Abid Ali//Initializing Parameters
        private const string PARM_NOTE_ID = "@NoteId";
        //Start 31-03-2016 Abid Ali

        private const string PARM_Lab_ORDER_AOE_TEST_CODE = "@TestCode";
        private const string PARM_Lab_ORDER_AOE_ANSWER_ID = "@LabOrderAOEAnswersID";
        private const string PARM_Lab_ORDER_AOE_LAB_ORDER_TEST_ID = "@LabOrderTestId";
        private const string PARM_Lab_ORDER_AOE_QUESTION = "@Question";
        private const string PARM_Lab_ORDER_AOE_ANSWER = "@Answer";



        private const string PARM_Lab_ORDER_TEST_ID = "@LabOrderTestId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_TEST_DATE = "@TestDate";
        private const string PARM_TEST_TIME = "@TestTime";
        private const string PARM_DIET_ID = "@DietId";
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

        private const string PARM_ORDER_SET_ID = "@OrderSetId";

        #region MK, Insurance ExternalInformation

        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";

        #endregion

        #endregion


        #region Constructors

        public DALOS_LabOrder()
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


        #region Support Functions for Lab


        public string SaveABNAgainstTest(long LabOrderTestId, bool IsABN)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@LabOrderTestId", LabOrderTestId);
                dbManager.AddParameters(1, "@IsABN", IsABN);

                dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAB_TEST_ABN);
                return "Successfully Inserted";
            }
            catch (Exception e)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::Lab_ABN_Select", PROC_LAB_TEST_ABN, e);
                return e.Message;
                throw e;

            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// <summary>
        /// Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateLabOderInsertUpdateParameters(IDBManager dbManager, DSOS_LabOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(21);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_ID, ds.OS_LabOrder.LabOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(21);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_ID, ds.OS_LabOrder.LabOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_ORDER_SET_ID, ds.OS_LabOrder.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_LAB_ID, ds.OS_LabOrder.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FACILITY_ID, ds.OS_LabOrder.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_ID, ds.OS_LabOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_ASSIGNEE_ID, ds.OS_LabOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_Lab_ORDER_DATE, ds.OS_LabOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_Lab_ORDER_TIME, ds.OS_LabOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_BILLING_TYPE_ID, ds.OS_LabOrder.BillingTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PRIMARY_INSURANCE_ID, ds.OS_LabOrder.PrimaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_SECONDARY_INSURANCE_ID, ds.OS_LabOrder.SecondaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_TERTIARY_INSURANCE_ID, ds.OS_LabOrder.TertiaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_GUARANTOR_ID, ds.OS_LabOrder.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_RELATIONSHIP_ID, ds.OS_LabOrder.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.OS_LabOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.OS_LabOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.OS_LabOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.OS_LabOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.OS_LabOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.OS_LabOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Abid Ali for status
            dbManager.AddInsertUpdateParameters(20, PARM_STATUS, ds.OS_LabOrder.StatusColumn.ColumnName, DbType.String);
            //End 22-03-2016 Abid Ali for status


        }
        //End//31-03-2016//Abid Ali// Support functions for Lab

        private void CreateLabOderTestInsertUpdateParameters(IDBManager dbManager, DSOS_LabOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_TEST_ID, ds.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_TEST_ID, ds.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_Lab_ORDER_ID, ds.OS_LabOrderTest.LabOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.OS_LabOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODEDESCRIPTION, ds.OS_LabOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_TEST_DATE, ds.OS_LabOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TEST_TIME, ds.OS_LabOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_URGENCY_ID, ds.OS_LabOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COLLECTEDAT_ID, ds.OS_LabOrderTest.CollectedAtIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_SPECIMEN_ID, ds.OS_LabOrderTest.SpecimenIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_VOLUME_LENGTH, ds.OS_LabOrderTest.VolumeLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_VOLUME_ID, ds.OS_LabOrderTest.VolumeIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_PATIENT_INSTRUCTION, ds.OS_LabOrderTest.PatientInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_FILLER_INSTRUCTION, ds.OS_LabOrderTest.FillerInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_COMMENTS, ds.OS_LabOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.OS_LabOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.OS_LabOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.OS_LabOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.OS_LabOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.OS_LabOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.OS_LabOrderTest.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_DIET_ID, ds.OS_LabOrderTest.DietIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(21, PARM_SAMPLESTORAGE, ds.OS_LabOrderTest.SampleStorageColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_ALTSPECIMENID, ds.OS_LabOrderTest.AltSpecimenIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(23, PARM_MODIFIER, ds.OS_LabOrderTest.ModifierColumn.ColumnName, DbType.String);
        }

        /// <summary>
        ///  Method Name: createLabOrderProblemParameters
        ///  Author: Abid Ali
        ///  Created Date: 31-03-2016
        ///  Description: insert/update  Lab Order Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createLabOrderProblemParameters(IDBManager dbManager, DSOS_LabOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_PROBLEM_ID, ds.OS_LabOrderProblem.LabOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_PROBLEM_ID, ds.OS_LabOrderProblem.LabOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Lab_ORDER_ID, ds.OS_LabOrderProblem.LabOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.OS_LabOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.OS_LabOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.OS_LabOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.OS_LabOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.OS_LabOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.OS_LabOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.OS_LabOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.OS_LabOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateLabOderAOEAnswersInsertUpdateParameters(IDBManager dbManager, DSOS_LabOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(5);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_AOE_ANSWER_ID, ds.OS_LabOrderAOEAnswers.LabOrderAOEAnswersIDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(5);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_AOE_ANSWER_ID, ds.OS_LabOrderAOEAnswers.LabOrderAOEAnswersIDColumn.ColumnName, DbType.Int32);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Lab_ORDER_AOE_LAB_ORDER_TEST_ID, ds.OS_LabOrderAOEAnswers.LabOrderTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_Lab_ORDER_AOE_TEST_CODE, ds.OS_LabOrderAOEAnswers.TestCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_Lab_ORDER_AOE_QUESTION, ds.OS_LabOrderAOEAnswers.QuestionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_Lab_ORDER_AOE_ANSWER, ds.OS_LabOrderAOEAnswers.AnswerColumn.ColumnName, DbType.String);

        }
        #endregion

        #region Lab (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will insert/update Lab Oder
        public DSOS_LabOrder InsertUpdateLabOrder(DSOS_LabOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //System.Diagnostics.Debug.Write("Start Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
               // DataTable dtTemp = ds.OS_LabOrder.GetChanges();
                dbManager.BeginTransaction();
                this.CreateLabOderInsertUpdateParameters(dbManager, ds, true);
                this.CreateLabOderInsertUpdateParameters(dbManager, ds, false);

                ds = (DSOS_LabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_INSERT, PROC_Lab_ORDER_UPDATE, ds, ds.OS_LabOrder.TableName);
                //if (dtTemp != null && ds.OS_LabOrder.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.OS_LabOrder.Rows[0][ds.OS_LabOrder.LabOrderIdColumn].ToString(), null, ds.OS_LabOrder.Rows[0][ds.OS_LabOrder.LabOrderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                // System.Diagnostics.Debug.Write("End Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::InsertUpdateLabOrder", PROC_Lab_ORDER_INSERT + " " + PROC_Lab_ORDER_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will load Lab Order
        public DSOS_LabOrder LoadLabOrder( long OrderSetId, string pageNumber, string rowsPerPage)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
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

                if (OrderSetId == 0)
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, OrderSetId);
                if (page <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.OS_LabOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECT, ds, ds.OS_LabOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::LoadLabOrder", PROC_Lab_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will delete Lab Order
        public string DeleteLabOrder(string LabOrderId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Lab_ORDER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
               
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::DeleteLabOrder", PROC_Lab_ORDER_DELETE, ex);
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
        public DSOS_LabOrder LoadLabOrderPDF(long LabOrderId, long patientId)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                if (patientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECTPDF, ds, ds.OS_LabOrderPdf.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::LoadLabOrderPDF", PROC_Lab_ORDER_SELECTPDF, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSOS_LabOrder FillLabOrder(long LabOrderId)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_FILL, ds, ds.OS_LabOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::FillLabOrder", PROC_Lab_ORDER_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        //End//31-03-2016//Abid Ali// Lab Oder CRUD Operations


        #region INSERT/DELETE/SELECT Functions LabOrder Problems

        /// <summary>
        /// Method Name: insertUpdateLabOrderProblems
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: insert/update  Lab Order Problems
        /// </summary>
        /// <param name="DSOS_LabOrder" type="DATASET"></param>
        public DSOS_LabOrder insertUpdateLabOrderProblems(DSOS_LabOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.OS_LabOrderProblem.GetChanges();
                dbManager.BeginTransaction();
                createLabOrderProblemParameters(dbManager, ds, true);
                createLabOrderProblemParameters(dbManager, ds, false);
                ds = (DSOS_LabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_INSERT, PROC_Lab_ORDER_PROBLEM_INSERT, ds, ds.OS_LabOrderProblem.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.OS_LabOrderProblem.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.OS_LabOrderProblem.Rows[i][ds.OS_LabOrderProblem.LabOrderProblemIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.OS_LabOrderProblem.Rows[0][ds.OS_LabOrderProblem.LabOrderProblemIdColumn].ToString(), null, ds.OS_LabOrderProblem.Rows[0][ds.OS_LabOrderProblem.LabOrderIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALOS_LabOrder::insertUpdateLabProblemsOrder", PROC_Lab_ORDER_PROBLEM_INSERT + " " + PROC_Lab_ORDER_PROBLEM_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Method Name: loadLabOrderProblems
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: loading Lab Order Problems
        /// </summary>
        /// <param name="LabOrderId" type="long">LabOrderId to be deleted</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSOS_LabOrder loadLabOrderProblems(long LabOrderProblemId, long LabOrderId, long orderSetId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);

                if (LabOrderProblemId == 0)
                    dbManager.AddParameters(1, PARM_Lab_ORDER_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_Lab_ORDER_PROBLEM_ID, LabOrderProblemId);

                if (orderSetId == 0)
                    dbManager.AddParameters(2, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ORDER_SET_ID, orderSetId);
                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.OS_LabOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_SELECT, ds, ds.OS_LabOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::loadLabOrder", PROC_Lab_ORDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: deleteLabOrderProblem
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: deleting Lab Order Problem
        /// </summary>
        /// <param name="LabOrderId" type="long">LabOrderId to be deleted</param>
        ///

        public string deleteLabOrderProblems(long LabOrderId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSOS_LabOrder dsCurrentOrderProblems = loadLabOrderProblems(0, Convert.ToInt32(LabOrderId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderProblems.OS_LabOrderProblem;
                    if (dtTemp != null)
                    {
                        if (dsCurrentOrderProblems.OS_LabOrderProblem.Rows.Count > 0)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.OS_LabOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.OS_LabOrderProblem.Rows[0][dsCurrentOrderProblems.OS_LabOrderProblem.LabOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALOS_LabOrder::deleteLabOrderProblem", PROC_Lab_ORDER_PROBLEM_DELETE, ex);
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
        /// Method Name: labOrderChangePatient
        /// Author: Farooq AHmad
        /// Created Date: 17-08-2016
        /// Description: LabOrder Change Patient
        /// </summary>
        /// <param name="LabOrderId" type="long">LabOrderId to be deleted</param>
        ///
        public string labOrderChangePatient(long labOrderId, long PatientId)
        {

            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, labOrderId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_LAB_ORDER_CHANGE_PATIENT);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::labOrderChangePatient", PROC_LAB_ORDER_CHANGE_PATIENT, ex);
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
        /// Method Name:  attachLabrderWithNotes
        /// Author:  Abid Ali
        /// Date:    31-03-2016
        /// Reason:  This function will handle attach of Lab Order with Note
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public DSOS_LabOrder attachLabOrderWithNotes(string LabOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSOS_LabOrder ds = new DSOS_LabOrder();

                dbManager.Open();

                dbManager.CreateParameters(6);
                if (string.IsNullOrEmpty(LabOrderId))
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }
                dbManager.AddParameters(2, PARM_CREATED_BY, MDVSession.Current.AppUserName);
                dbManager.AddParameters(3, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_MODIFIED_BY, MDVSession.Current.AppUserName);
                dbManager.AddParameters(5, PARM_MODIFIED_ON, DateTime.Now);


                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_Lab_ORDER, ds, ds.OS_LabOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::attachLabOrderWithNotes", PROC_ATTACH_Lab_ORDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: detachLabOrderFromNotes
        /// Author:  Abid Ali
        /// Date:    31-03-2016
        /// Reason:  This function will handle detach of Lab Order from Note
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public string detachLabOrderFromNotes(string LabOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(LabOrderId))
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_Lab_ORDER);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::detachLabOrderFromNotes", PROC_DETACH_Lab_ORDER, ex);
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

        #region "Lab Order PDF"

        public DSOS_LabOrder loadLabOrderforPDF(long LabOrderId, long patientId)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                if (patientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECT_PDF, ds, ds.OS_LabOrderPdf.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::LoadLabOrderforPDF", PROC_Lab_ORDER_SELECT_PDF, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Lab Order Test"

        public DSOS_LabOrder insertUpdateLabOrderTest(DSOS_LabOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                CreateLabOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateLabOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSOS_LabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_INSERT, PROC_Lab_ORDER_TEST_UPDATE, ds, ds.OS_LabOrderTest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::insertUpdateLabOrderTest", PROC_Lab_ORDER_TEST_INSERT + " " + PROC_Lab_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOS_LabOrder LoadLabOrderTest(long LabOrderId, Int32 LabOrderTestId, long orderSetId, string pageNumber, string rowsPerPage, Int64 noteId = 0)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
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

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);

                if (LabOrderTestId == 0)
                    dbManager.AddParameters(1, PARM_Lab_ORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_Lab_ORDER_TEST_ID, LabOrderTestId);

                if (orderSetId <= 0)
                    dbManager.AddParameters(2, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ORDER_SET_ID, orderSetId);
                if (page <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.OS_LabOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(6, PARM_User_ID, MDVSession.Current.AppUserId);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_SELECT, ds, ds.OS_LabOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::LoadLabOrderTest", PROC_Lab_ORDER_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteLabOrderTest(long LabOrderTestId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSOS_LabOrder dsCurrentOrderTest = LoadLabOrderTest(0, Convert.ToInt32(LabOrderTestId), 0, "1", "100");
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_TEST_ID, LabOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.OS_LabOrderTest;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, LabOrderTestId.ToString(), null, dsCurrentOrderTest.OS_LabOrderTest.Rows[0][dsCurrentOrderTest.OS_LabOrderTest.LabOrderIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALOS_LabOrder::deleteLabOrderTest", PROC_Lab_ORDER_TEST_DELETE, ex);
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



        public DSOS_LabOrder loadOrdersForSoap(string medicationID, long patientId)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (string.IsNullOrEmpty(medicationID))
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, medicationID);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);



                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LABORDER_SELECT_FOR_SOAPTEXT, ds, ds.OS_LabOrder.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::loadOrdersForSoap", PROC_LABORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<LabTestLookupModel> LookupLabTestReport(string Test)
        {
            List<LabTestLookupModel> listModel = new List<LabTestLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(Test))
                    dbManager.AddParameters(0, PARM_TEST, null);
                else
                    dbManager.AddParameters(0, PARM_TEST, Test);
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LAB_TEST_LOOKUP);
                LabTestLookupModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new LabTestLookupModel();
                    modelFill.LOINC = MDVUtility.CheckStringNull(reader["LOINC"]);
                    modelFill.LOINCDescription = MDVUtility.CheckStringNull(reader["LOINCDescription"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_LabOrder::LookupLabTestReport", PROC_LAB_TEST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Lab Order AOE
        /// <summary>
        ///
        /// </summary>
        /// <param name="testCode">LOINC Code</param>
        /// <returns></returns>
        public DSOS_LabOrder LoadLabOrderAOE(string testCode)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_Lab_ORDER_AOE_TEST_CODE, testCode);

                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_SELECT, ds, ds.OS_LabOrderAOE.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::Lab_ORDER_AOE_SELECT", PROC_Lab_ORDER_AOE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOS_LabOrder insertUpdateLabOrderAOEAnswers(DSOS_LabOrder ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //System.Diagnostics.Debug.Write("Start Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                // DataTable dtTemp = ds.LabOrder.GetChanges();
                dbManager.BeginTransaction();
                this.CreateLabOderAOEAnswersInsertUpdateParameters(dbManager, ds, true);
                this.CreateLabOderAOEAnswersInsertUpdateParameters(dbManager, ds, false);

                ds = (DSOS_LabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_ANSERWS_INSERT, PROC_Lab_ORDER_AOE_ANSERWS_UPDATE, ds, ds.OS_LabOrderAOEAnswers.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString(), null, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                // System.Diagnostics.Debug.Write("End Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::PROC_LabOrder_AOE_ANSWERS_INSERT", PROC_Lab_ORDER_AOE_ANSERWS_INSERT + " " + PROC_Lab_ORDER_AOE_ANSERWS_UPDATE, ex);
                throw ex;
            }
        }
        public DSOS_LabOrder LoadLabOrderAOEAnswers(string testCode, string testId)
        {
            DSOS_LabOrder ds = new DSOS_LabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {


                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (testId != "")
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_TEST_ID, testId);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_Lab_ORDER_TEST_ID, null);
                }

                if (testCode != "")
                {
                    dbManager.AddParameters(1, PARM_Lab_ORDER_AOE_TEST_CODE, testCode);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_Lab_ORDER_AOE_TEST_CODE, null);
                }
                //dbManager.AddParameters(1, PARM_Lab_ORDER_AOE_QUESTION, question);
                ds = (DSOS_LabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_ANSWERS_SELECT, ds, ds.OS_LabOrderAOEAnswers.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_LabOrder::Lab_ORDER_AOE_Answer", PROC_Lab_ORDER_AOE_SELECT, ex);
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
