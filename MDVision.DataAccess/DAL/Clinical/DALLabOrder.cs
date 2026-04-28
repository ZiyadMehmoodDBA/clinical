/* Author:  Muhammad Arshad
 * Created Date: 31/03/2016
 * OverView: Created for LabOrder in Clinical Module
 */

using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.Model.Admin.Provider;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Common;
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
    public class DALLabOrder
    {
        #region Variable

        #endregion


        #region Stored Procedure Names
        //Start//31-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_Lab_ORDER_INSERT = "Clinical.sp_LabOrderInsert";
        private const string PROC_Lab_ORDER_UPDATE = "Clinical.sp_LabOrderUpdate";
        private const string PROC_Lab_ORDER_DELETE = "Clinical.sp_LabOrderDelete";
        private const string PROC_Lab_ORDER_SELECT = "Clinical.sp_LabOrderSelect";
        //End//31-03-2016//Abid Ali// Store Procedures Names
        private const string PROC_Lab_ORDER_SELECTPDF = "Clinical.sp_LabOrderSelectPDF";
        //Start//31-03-2016//Abid Ali//Initializing Stored Procedures
        private const string PROC_Lab_ORDER_PROBLEM_INSERT = "Clinical.sp_LabOrderProblemInsert";
        private const string PROC_Lab_ORDER_PROBLEM_UPDATE = "Clinical.sp_LabOrderProblemUpdate";
        private const string PROC_Lab_ORDER_PROBLEM_DELETE = "Clinical.sp_LabOrderProblemDelete";
        private const string PROC_Lab_ORDER_PROBLEM_SELECT = "Clinical.sp_LabOrderProblemSelect";
        //End//31-03-2016//Abid Ali//Initializing Stored Procedures

        private const string PROC_COLLECTEDAT_LOOKUP = "Clinical.sp_CollectedAtLookup";
        private const string PROC_URGENCY_LOOKUP = "Clinical.sp_UrgencyLookup";
        private const string PROC_SPECIMEN_LOOKUP = "Clinical.sp_SpecimenLookup";
        private const string PROC_VOLUME_LOOKUP = "Clinical.sp_VolumeLookup";
        private const string PROC_DIET_LOOKUP = "Clinical.sp_DietLookup";

        private const string PROC_Lab_ORDER_TEST_INSERT = "Clinical.sp_LabOrderTestInsert";
        private const string PROC_Lab_ORDER_TEST_UPDATE = "Clinical.sp_LabOrderTestUpdate";
        private const string PROC_Lab_ORDER_TEST_DELETE = "Clinical.sp_LabOrderTestDelete";
        private const string PROC_Lab_ORDER_TEST_SELECT = "Clinical.sp_LabOrderTestSelect";

        private const string PROC_Lab_ORDER_LATEST_NOTEID_SELECT = "Clinical.sp_LabOrdersAndResultsLatestNoteSelect";
        private const string PROC_GET_PROVIDER_CPT = "Clinical.sp_GetProviderCptsForLabOrder";

        private const string PROC_ATTACH_Lab_ORDER = "Clinical.sp_AttachLabOrderWithNotes";
        private const string PROC_DETACH_Lab_ORDER = "Clinical.sp_DetachLabOrderFromNotes";
        private const string PROC_LABORDER_SELECT_FOR_SOAPTEXT = "Clinical.sp_LabOrderSelectForSoapText";
        private const string PROC_LAB_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT = "Clinical.sp_LabOrderSelectForSoapTextWithOrderSetId";
        private const string PROC_Lab_ORDER_SELECT_PDF = "Clinical.sp_LabOrderSelectPDF";

        private const string PROC_LAB_TEST_LOOKUP = "Clinical.sp_LabTestLookup";
        private const string PROC_Lab_ORDER_AOE_SELECT = "Clinical.sp_LabOrderAOESelect";
        private const string PROC_LAB_TEST_ABN = "Clinical.sp_LabOrderABNInsert";
        private const string PROC_LAB_ORDER_ABN_TEST_SELECT = "Clinical.sp_LabOrderABNSelect";


        private const string PROC_Lab_ORDER_AOE_ANSWERS_SELECT = "Clinical.sp_LabOrderAOEAnswersSelect";
        private const string PROC_Lab_ORDER_AOE_ANSERWS_INSERT = "Clinical.sp_LabOrderAOEAnswersInsert";
        private const string PROC_Lab_ORDER_AOE_ANSERWS_UPDATE = "Clinical.sp_LabOrderAOEAnswersUpdate";
        private const string PROC_LAB_ORDER_CHANGE_PATIENT = "Clinical.sp_LabOrderChangePatient";

        private const string PROC_NOTES_LABORDER_SELECT = "[Clinical].[sp_NotesLabOrderSelect]";

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
        private const string PARM_PROVIDER_ID_NOTE = "@ProviderIdNote";
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

        private const string PARM_SPECIMEN_SOURCE_ID = "@SpecimenSourceId";
        private const string PARM_ALT_SPECIMEN_SOURCE_ID = "@AltSpecimenSourceId";
        private const string PARM_ANTIMICROBIAL_ID = "@AntimicrobialIds";
        private const string PARM_TESTTYPE_ID = "@TestTypeId";
        private const string PARM_ORGANISM_ID = "@OrganismId";
        private const string PARM_NEGATION_REASON_ID = "@NegationReasonId";
        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        #region MK, Insurance ExternalInformation

        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";

        #endregion

        #endregion


        #region Constructors

        public DALLabOrder()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALLabOrder(SharedVariable SharedVariable)
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
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ABN_Select", PROC_LAB_TEST_ABN, e);
                return e.Message;
                throw e;

            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public DSLabOrder GetLabOrderABN(long LabOrderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSLabOrder ds = new DSLabOrder();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@labOrderId", LabOrderId);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAB_ORDER_ABN_TEST_SELECT, ds, ds.LabOrderTest.TableName);
            }
            catch (Exception e)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_SELECT", PROC_LAB_TEST_ABN, e);
                throw e;
                return ds;
            }
            return ds;

        }

        /// <summary>
        /// Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateLabOderInsertUpdateParameters(IDBManager dbManager, DSLabOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(23);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_ID, ds.LabOrder.LabOrderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(23);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_ID, ds.LabOrder.LabOrderIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.LabOrder.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_LAB_ID, ds.LabOrder.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FACILITY_ID, ds.LabOrder.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_ID, ds.LabOrder.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_ASSIGNEE_ID, ds.LabOrder.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_Lab_ORDER_DATE, ds.LabOrder.OrderDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_Lab_ORDER_TIME, ds.LabOrder.OrderTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_BILLING_TYPE_ID, ds.LabOrder.BillingTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PRIMARY_INSURANCE_ID, ds.LabOrder.PrimaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_SECONDARY_INSURANCE_ID, ds.LabOrder.SecondaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_TERTIARY_INSURANCE_ID, ds.LabOrder.TertiaryInsuraceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_GUARANTOR_ID, ds.LabOrder.GuarantorIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_RELATIONSHIP_ID, ds.LabOrder.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.LabOrder.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.LabOrder.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.LabOrder.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.LabOrder.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.LabOrder.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAP_TEXT, ds.LabOrder.SoapTextColumn.ColumnName, DbType.String);
            //Start 22-03-2016 Abid Ali for status
            dbManager.AddInsertUpdateParameters(20, PARM_STATUS, ds.LabOrder.StatusColumn.ColumnName, DbType.String);
            //End 22-03-2016 Abid Ali for status

            dbManager.AddInsertUpdateParameters(21, PARM_NOTE_ID, ds.LabOrder.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_NEGATION_REASON_ID, ds.LabOrder.NegationReasonIdColumn.ColumnName, DbType.Int32);

        }
        //End//31-03-2016//Abid Ali// Support functions for Lab

        /// <summary>
        /// LabOrder Test Insert Update Support Function
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateLabOderTestInsertUpdateParameters(IDBManager dbManager, DSLabOrder ds, bool isInsert = true)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(29);
                dbManager.AddInsertUpdateParameters(i++, PARM_Lab_ORDER_TEST_ID, ds.LabOrderTest.LabOrderTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(29);
                dbManager.AddInsertUpdateParameters(i++, PARM_Lab_ORDER_TEST_ID, ds.LabOrderTest.LabOrderTestIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_Lab_ORDER_ID, ds.LabOrderTest.LabOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CPT_CODE, ds.LabOrderTest.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CPT_CODEDESCRIPTION, ds.LabOrderTest.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_TEST_DATE, ds.LabOrderTest.TestDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_TEST_TIME, ds.LabOrderTest.TestTimeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_URGENCY_ID, ds.LabOrderTest.UrgencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_COLLECTEDAT_ID, ds.LabOrderTest.CollectedAtIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_SPECIMEN_ID, ds.LabOrderTest.SpecimenIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_VOLUME_LENGTH, ds.LabOrderTest.VolumeLengthColumn.ColumnName, DbType.String);
         //   dbManager.AddInsertUpdateParameters(i++, PARM_VOLUME_ID, ds.LabOrderTest.VolumeIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_PATIENT_INSTRUCTION, ds.LabOrderTest.PatientInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_FILLER_INSTRUCTION, ds.LabOrderTest.FillerInstructionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_COMMENTS, ds.LabOrderTest.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_ACTIVE, ds.LabOrderTest.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_BY, ds.LabOrderTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_ON, ds.LabOrderTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_BY, ds.LabOrderTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_ON, ds.LabOrderTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_SOAP_TEXT, ds.LabOrderTest.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_DIET_ID, ds.LabOrderTest.DietIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_SAMPLESTORAGE, ds.LabOrderTest.SampleStorageColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_ALTSPECIMENID, ds.LabOrderTest.AltSpecimenIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(i++, PARM_SPECIMEN_SOURCE_ID, ds.LabOrderTest.SpecimenSourceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_ALT_SPECIMEN_SOURCE_ID, ds.LabOrderTest.AltSpecimenSourceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_ORGANISM_ID, ds.LabOrderTest.OrganismIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(i++, PARM_ANTIMICROBIAL_ID, ds.LabOrderTest.AntimicrobialIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_TESTTYPE_ID, ds.LabOrderTest.TestTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, "@Unit", ds.LabOrderTest.UnitColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, "@Modifier", ds.LabOrderTest.ModifierColumn.ColumnName, DbType.String);
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
        private void createLabOrderProblemParameters(IDBManager dbManager, DSLabOrder ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_PROBLEM_ID, ds.LabOrderProblem.LabOrderProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_PROBLEM_ID, ds.LabOrderProblem.LabOrderProblemIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Lab_ORDER_ID, ds.LabOrderProblem.LabOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PROBLEM_ID, ds.LabOrderProblem.ProblemIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.LabOrderProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.LabOrderProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.LabOrderProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.LabOrderProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.LabOrderProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.LabOrderProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.LabOrderProblem.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion

        #region Support functions for lab order AOE

        private void CreateLabOderAOEAnswersInsertUpdateParameters(IDBManager dbManager, DSLabOrder ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(5);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_AOE_ANSWER_ID, ds.LabOrderAOEAnswers.LabOrderAOEAnswersIDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(5);
                dbManager.AddInsertUpdateParameters(0, PARM_Lab_ORDER_AOE_ANSWER_ID, ds.LabOrderAOEAnswers.LabOrderAOEAnswersIDColumn.ColumnName, DbType.Int32);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Lab_ORDER_AOE_LAB_ORDER_TEST_ID, ds.LabOrderAOEAnswers.LabOrderTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_Lab_ORDER_AOE_TEST_CODE, ds.LabOrderAOEAnswers.TestCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_Lab_ORDER_AOE_QUESTION, ds.LabOrderAOEAnswers.QuestionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_Lab_ORDER_AOE_ANSWER, ds.LabOrderAOEAnswers.AnswerColumn.ColumnName, DbType.String);

        }

        #endregion



        #region Lab (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will insert/update Lab Oder
        public DSLabOrder InsertUpdateLabOrder(DSLabOrder ds, IDBManager dbManager = null)
        {
            bool isdbManagerNull = dbManager == null ? true : false;

            if (isdbManagerNull)
            {
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.BeginTransaction();
            }
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //System.Diagnostics.Debug.Write("Start Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                DataTable dtTemp = ds.LabOrder.GetChanges();

                this.CreateLabOderInsertUpdateParameters(dbManager, ds, true);
                this.CreateLabOderInsertUpdateParameters(dbManager, ds, false);

                ds = (DSLabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_INSERT, PROC_Lab_ORDER_UPDATE, ds, ds.LabOrder.TableName);
                if (dtTemp != null && ds.LabOrder.Rows.Count > 0)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString(), null, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                if (isdbManagerNull)
                {
                    dbManager.CommitTransaction();
                }
                ds.AcceptChanges();
                // System.Diagnostics.Debug.Write("End Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                if (isdbManagerNull)
                {
                    dbManager.RollBackTransaction();
                }
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


                MDVLogger.DALErrorLog("DALLabOrder::PROC_LabOrder_INSERT", PROC_Lab_ORDER_INSERT + " " + PROC_Lab_ORDER_UPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will load Lab Order
        public DSLabOrder LoadLabOrder(long LabOrderId, long patientId, long noteId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, long labId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

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

                dbManager.BeginTransaction();
                dbManager.CreateParameters(15);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.LabOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //Start 31-03-2016 Abid Ali 

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
                //Start 31-03-2016 Abid Ali 

                dbManager.AddParameters(13, PARM_Entity_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(14, PARM_User_ID, MDVSession.Current.AppUserId);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECT, ds, ds.LabOrder.TableName);
                if (LabOrderId > 0)
                {

                    DataTable dtTemp = ds.LabOrder;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            if (ds.LabOrder.Rows.Count > 0)
                            {
                                string labOrderId_audit = ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString();
                                new DBActivityAudit().InsertDBAuditAsync(dtTemp, labOrderId_audit, null, labOrderId_audit, isViewAction, isPrintAcion, false, "", "0");
                                //dsDBAudit.AcceptChanges();
                            }

                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_SELECT", PROC_Lab_ORDER_SELECT, ex);
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
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_Entity_ID, MDVSession.Current.EntityId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Lab_ORDER_DELETE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::DeleteLabOrder", PROC_Lab_ORDER_DELETE, ex);
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
        public DSLabOrder LoadLabOrderPDF(long LabOrderId, long patientId)
        {
            DSLabOrder ds = new DSLabOrder();
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

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECTPDF, ds, ds.LabOrderPdf.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::LoadLabOrderPDF", PROC_Lab_ORDER_SELECTPDF, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabOrder LoadLabOrder(long LabOrderId, long patientId, long noteId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, long labId, long userId, long entityId)
        {
            DSLabOrder ds = new DSLabOrder();
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
                    rpp = 15;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }
                dbManager.Open();
                dbManager.CreateParameters(15);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.LabOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

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
                
                dbManager.AddParameters(13, PARM_Entity_ID, entityId);
                dbManager.AddParameters(14, PARM_User_ID, userId);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECT, ds, ds.LabOrder.TableName);              
                return ds;
            }
            catch (Exception ex)
            {                                       
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_SELECT", PROC_Lab_ORDER_SELECT, ex);
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
        /// <param name="DSLabOrder" type="DATASET"></param>
        public DSLabOrder insertUpdateLabOrderProblems(DSLabOrder ds, string patientId, IDBManager dbManager = null)
        {
            bool isdbManagerNull = dbManager == null ? true : false;

            if (isdbManagerNull)
            {
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.BeginTransaction();
            }
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.LabOrderProblem.GetChanges();

                createLabOrderProblemParameters(dbManager, ds, true);
                createLabOrderProblemParameters(dbManager, ds, false);
                ds = (DSLabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_INSERT, PROC_Lab_ORDER_PROBLEM_INSERT, ds, ds.LabOrderProblem.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.LabOrderProblem.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.LabOrderProblem.Rows[i][ds.LabOrderProblem.LabOrderProblemIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrderProblem.Rows[0][ds.LabOrderProblem.LabOrderProblemIdColumn].ToString(), null, ds.LabOrderProblem.Rows[0][ds.LabOrderProblem.LabOrderIdColumn].ToString(), false, false, false, patientId);
                    dsDBAudit.AcceptChanges();
                }
                if (isdbManagerNull)
                {
                    dbManager.CommitTransaction();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                if (isdbManagerNull)
                {
                    dbManager.RollBackTransaction();
                }
                MDVLogger.DALErrorLog("DALLabOrder::insertUpdateLabProblemsOrder", PROC_Lab_ORDER_PROBLEM_INSERT + " " + PROC_Lab_ORDER_PROBLEM_INSERT, ex);
                throw ex;

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
        public DSLabOrder loadLabOrderProblems(long LabOrderProblemId, long LabOrderId, long patientId, int pageNumber = 1, int rowsPerPage = 2000)
        {
            DSLabOrder ds = new DSLabOrder();
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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.LabOrderProblem.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_SELECT, ds, ds.LabOrderProblem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::loadLabOrder", PROC_Lab_ORDER_SELECT, ex);
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

        public string deleteLabOrderProblems(long LabOrderId, string patientId, IDBManager dbManager = null)
        {
            bool isdbManagerNull = dbManager == null ? true : false;

            if (isdbManagerNull)
            {
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.BeginTransaction();
            }
            string returnVal = "";

            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();

                DSLabOrder dsCurrentOrderProblems = loadLabOrderProblems(0, Convert.ToInt32(LabOrderId), 0);
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_ORDER_PROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderProblems.LabOrderProblem;
                    if (dtTemp != null)
                    {
                        if (dsCurrentOrderProblems.LabOrderProblem.Rows.Count > 0)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderProblems.LabOrderProblem.Rows[0].ToString(), null, dsCurrentOrderProblems.LabOrderProblem.Rows[0][dsCurrentOrderProblems.LabOrderProblem.LabOrderIdColumn].ToString(), false, false, true, patientId);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                    if (isdbManagerNull)
                    {
                        dbManager.CommitTransaction();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                if (isdbManagerNull)
                {
                    dbManager.RollBackTransaction();
                }
                MDVLogger.DALErrorLog("DALLabOrder::deleteLabOrderProblem", PROC_Lab_ORDER_PROBLEM_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
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
                MDVLogger.DALErrorLog("DALLabOrder::labOrderChangePatient", PROC_LAB_ORDER_CHANGE_PATIENT, ex);
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
        public DSLabOrder attachLabOrderWithNotes(string LabOrderId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSLabOrder ds = new DSLabOrder();

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

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_Lab_ORDER, ds, ds.LabOrder.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::attachLabOrderWithNotes", PROC_ATTACH_Lab_ORDER, ex);
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
                MDVLogger.DALErrorLog("DALLabOrder::detachLabOrderFromNotes", PROC_DETACH_Lab_ORDER, ex);
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

        #region "Lab Order Test"

        public DSLabOrder insertUpdateLabOrderTest(DSLabOrder ds, string patientId, IDBManager dbManager = null)
        {
            bool isdbManagerNull = dbManager == null ? true : false;
            if (isdbManagerNull)
            {
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.BeginTransaction();
            }
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.LabOrderTest.GetChanges();

                CreateLabOderTestInsertUpdateParameters(dbManager, ds, true);
                CreateLabOderTestInsertUpdateParameters(dbManager, ds, false);
                ds = (DSLabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_INSERT, PROC_Lab_ORDER_TEST_UPDATE, ds, ds.LabOrderTest.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.LabOrderTest.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > i)
                        {
                            dtTemp.Rows[i]["PrimaryKey"] = ds.LabOrderTest.Rows[i][ds.LabOrderTest.LabOrderTestIdColumn];
                        }

                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrderTest.Rows[0][ds.LabOrderTest.LabOrderTestIdColumn].ToString(), null, ds.LabOrderTest.Rows[0][ds.LabOrderTest.LabOrderIdColumn].ToString(), false, false, false, patientId);
                    dsDBAudit.AcceptChanges();
                }
                if (isdbManagerNull)
                {
                    dbManager.CommitTransaction();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                if (isdbManagerNull)
                {
                    dbManager.RollBackTransaction();
                }
                MDVLogger.DALErrorLog("DALLabOrder::insertUpdateLabOrderTest", PROC_Lab_ORDER_TEST_INSERT + " " + PROC_Lab_ORDER_TEST_UPDATE, ex);
                throw ex;

            }
        }
        public List<ProviderCPTs> GetProviderCPTs(long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            List<ProviderCPTs> ProviderCPTList = new List<ProviderCPTs>();
            ProviderCPTs providerCPTs = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_PROVIDER_CPT);

                while (reader.Read())
                {
                    providerCPTs = new ProviderCPTs();
                    providerCPTs.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    providerCPTs.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPT_Description"].ToString()) ? reader["CPT_Description"].ToString() : "";
                    ProviderCPTList.Add(providerCPTs);
                }

                return ProviderCPTList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::GetProviderCPTs", PROC_GET_PROVIDER_CPT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string GetLabOrderLatestNoteId(long LabOrderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            string data = "";
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);
                dbManager.AddParameters(1, "@LabOrderResultId", null);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_Lab_ORDER_LATEST_NOTEID_SELECT);
                while (reader.Read())
                {
                    data += Convert.ToString(reader["NoteId"]) + ";" + Convert.ToString(reader["NoteStatus"]) + ";" + Convert.ToInt64(reader["ProviderId"]);
                }
                return data;


            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string GetLabOrderResultLatestNoteId(long LabOrderResultId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            string data = "";
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                dbManager.AddParameters(1, "@LabOrderResultId", LabOrderResultId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_Lab_ORDER_LATEST_NOTEID_SELECT);
                while (reader.Read())
                {
                    data += Convert.ToString(reader["NoteId"]) + ";" + Convert.ToString(reader["NoteStatus"]) + ";" + Convert.ToInt64(reader["ProviderId"]);
                }
                return data;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public DSLabOrder LoadLabOrderTest(long LabOrderId, Int32 LabOrderTestId, long patientId, string pageNumber, string rowsPerPage, Int64 noteId = 0, SharedVariable sharedVariable = null)
        {
            DSLabOrder ds = new DSLabOrder();
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
                dbManager.CreateParameters(8);

                if (LabOrderId == 0)
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, LabOrderId);

                if (LabOrderTestId == 0)
                    dbManager.AddParameters(1, PARM_Lab_ORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_Lab_ORDER_TEST_ID, LabOrderTestId);

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

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.LabOrder.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (sharedVariable == null)
                {
                    dbManager.AddParameters(6, PARM_User_ID, MDVSession.Current.AppUserId);
                }
                else
                {
                    dbManager.AddParameters(6, PARM_User_ID, sharedVariable.AppUserId);
                }

                if (noteId <= 0)
                    dbManager.AddParameters(7, PARM_NOTE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(7, PARM_NOTE_ID, noteId);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_SELECT, ds, ds.LabOrderTest.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALLabOrder::LoadLabOrderTest", PROC_Lab_ORDER_TEST_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteLabOrderTest(long LabOrderTestId, string patientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSLabOrder dsCurrentOrderTest = LoadLabOrderTest(0, Convert.ToInt32(LabOrderTestId), 0, "1", "100");
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Lab_ORDER_TEST_ID, LabOrderTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_ORDER_TEST_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderTest.LabOrderTest;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, LabOrderTestId.ToString(), null, dsCurrentOrderTest.LabOrderTest.Rows[0][dsCurrentOrderTest.LabOrderTest.LabOrderIdColumn].ToString(), false, false, true, patientId);
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
                MDVLogger.DALErrorLog("DALLabOrder::deleteLabOrderTest", PROC_Lab_ORDER_TEST_DELETE, ex);
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



        public DSLabOrder loadOrdersForSoap(string medicationID, long patientId, long ProviderId)
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (string.IsNullOrEmpty(medicationID))
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Lab_ORDER_ID, medicationID);
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
                        ds.LabOrder.TableName,
                        ds.LabOrderTest.TableName,
                        ds.LabOrderProblem.TableName
                };

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LABORDER_SELECT_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::loadOrdersForSoap", PROC_LABORDER_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabOrder GetLabOrdersForSoapTextByOrderSetId(long OrderSetID, long notesId, long patientid, long providerId)
        {
            DSLabOrder ds = new DSLabOrder();
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
                        ds.LabOrder.TableName,
                        ds.LabOrderTest.TableName,
                        ds.LabOrderProblem.TableName
                };
                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAB_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::GetLabOrdersForSoapTextByOrderSetId", PROC_LAB_ORDER_SELECT_BY_ORDERSET_ID_FOR_SOAPTEXT, ex);
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
                MDVLogger.DALErrorLog("DALLabOrder::LookupLabTestReport", PROC_LAB_TEST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Lab Order PDF"

        public DSLabOrder loadLabOrderforPDF(long LabOrderId, long patientId)
        {
            DSLabOrder ds = new DSLabOrder();
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

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_SELECT_PDF, ds, ds.LabOrderPdf.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::LoadLabOrderforPDF", PROC_Lab_ORDER_SELECT_PDF, ex);
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
        public DSLabOrder LoadLabOrderAOE(string testCode)
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_Lab_ORDER_AOE_TEST_CODE, testCode);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_SELECT, ds, ds.LabOrderAOE.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_AOE_SELECT", PROC_Lab_ORDER_AOE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLabOrder insertUpdateLabOrderAOEAnswers(DSLabOrder ds, IDBManager dbManager = null)
        {
            bool isdbManagerNull = dbManager == null ? true : false;
            if (isdbManagerNull)
            {
                dbManager = ClientConfiguration.GetDBManager();
                dbManager.BeginTransaction();
            }
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //System.Diagnostics.Debug.Write("Start Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                // DataTable dtTemp = ds.LabOrder.GetChanges();

                this.CreateLabOderAOEAnswersInsertUpdateParameters(dbManager, ds, true);
                this.CreateLabOderAOEAnswersInsertUpdateParameters(dbManager, ds, false);

                ds = (DSLabOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_ANSERWS_INSERT, PROC_Lab_ORDER_AOE_ANSERWS_UPDATE, ds, ds.LabOrderAOEAnswers.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString(), null, ds.LabOrder.Rows[0][ds.LabOrder.LabOrderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                if (isdbManagerNull)
                {
                    dbManager.CommitTransaction();
                }
                ds.AcceptChanges();
                // System.Diagnostics.Debug.Write("End Time of DAL InsertUpdateLabOrder = " + DateTime.Now);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                if (isdbManagerNull)
                {
                    dbManager.RollBackTransaction();
                }
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
                MDVLogger.DALErrorLog("DALLabOrder::PROC_LabOrder_AOE_ANSWERS_INSERT", PROC_Lab_ORDER_AOE_ANSERWS_INSERT + " " + PROC_Lab_ORDER_AOE_ANSERWS_UPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }
        public DSLabOrder LoadLabOrderAOEAnswers(string testCode, string testId)
        {
            DSLabOrder ds = new DSLabOrder();
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
                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_AOE_ANSWERS_SELECT, ds, ds.LabOrderAOEAnswers.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_AOE_Answer", PROC_Lab_ORDER_AOE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Lab Order Patient, Insurance, Plan, Address and External InsurancePlanCode
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testCode">LOINC Code</param>
        /// <returns></returns>
        public DSLabOrder LoadLabOrderExternalBillingInformation(string patientInsuranceId)
        {
            DSLabOrder ds = new DSLabOrder();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_INSURANCE_ID, patientInsuranceId);

                ds = (DSLabOrder)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAB_ORDER_EXTERNAL_PATIENT_INSURANCE_PLAN_ADDRESS, ds, ds.External_PatientInsurancePlanAddress.TableName);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::PROC_LAB_ORDER_EXTERNAL_PATIENT_INSURANCE_PLAN_ADDRESS", PROC_LAB_ORDER_EXTERNAL_PATIENT_INSURANCE_PLAN_ADDRESS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Legacy Notes

        public List<LabOrder> NotesLabOrderSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<LabOrder> objList_LabOrder = new List<LabOrder>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_LABORDER_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        LabOrder model = new LabOrder();
                        var properties = typeof(LabOrder).GetProperties();
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
                        objList_LabOrder.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrder::NotesLabOrderSelect", PROC_NOTES_LABORDER_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_LabOrder;
        }

        #endregion Legacy Notes

    }
}
