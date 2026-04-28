/* Author:  Muhammad Arshad
 * Created Date: 31/03/2016
 * OverView: Created for LabResult in Clinical Module
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
using System.Data.SqlClient;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALLabResult
    {
        #region Variable

        #endregion

        #region Stored Procedure Names
        //Start//18-04-2016//Abid Ali// Result Store Procedures Names
        private const string PROC_Lab_ORDER_RESULT_DELETE = "Clinical.sp_LabOrderResultDelete";
        private const string PROC_Lab_TEST_DELETE = "Clinical.Sp_LabOrderResultTestDelete";
        private const string PROC_Lab_ORDER_RESULT_INSERT = "Clinical.sp_LabOrderResultInsert";
        private const string PROC_Lab_ORDER_RESULT_UPDATE = "Clinical.sp_LabOrderResultUpdate";
        private const string PROC_Lab_ORDER_RESULT_SELECT = "Clinical.sp_LabOrderResultSelect";
        private const string PROC_Lab_ORDER_LATEST_NOTEID_SELECT = "Clinical.sp_LabOrdersAndResultsLatestNoteSelect";
        //End//18-04-2016//Abid Ali// Result Store Procedures Names
        private const string PROC_GET_ASSIGNED_RESULTS_COUNT = "Clinical.sp_GetAssignedLabResultsCount";

        //Start Farooq Ahmad 22-8-2016
        private const string PROC_Lab_ORDER_Unsolicited_RESULT_SELECT = "Clinical.sp_LabOrderUnsolicitedResultSelect";
        //End Farooq Ahmad 22-8-2016

        //Start//18-04-2016//Abid Ali//Result Detail Store Procedures Names
        private const string PROC_Lab_ORDER_RESULT_DETAIL_DELETE = "Clinical.sp_LabOrderResultDetailDelete";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_INSERT = "Clinical.sp_LabOrderResultDetailInsert";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_UPDATE = "Clinical.sp_LabOrderResultDetailUpdate";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_SELECT_FACESHEET = "Clinical.sp_FS_LabOrderResultDetailSelect";
        private const string PROC_Lab_ORDER_RESULT_DETAIL_SELECT = "Clinical.sp_LabOrderResultDetailSelect";
        private const string PROC_ATTACH_LABORDER_RESULT_WITH_NOTES = "Clinical.sp_AttachLabOrderResultWithNotes";
        private const string PROC_DETACH_LAB_ORDER_FROM_NOTES = "Clinical.sp_DetachLabOrderResultFromNotes";
        //End//18-04-2016//Abid Ali//Result Detail Store Procedures Names

        //Start 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PROC_ORDER_RESULT_LOINC_LOOKUP = "Clinical.sp_OrderResultLOINCLookup";
        //End 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PROC_ORDER_RESULT_ORGANISM_LOOKUP = "Clinical.sp_ResultOrganismsLookup";
        private const string PROC_LABORDER_RESULT_SELECT_FOR_SOAPTEXT = "Clinical.sp_GetSoapTextForLabOrderResult"; //"Clinical.sp_LabOrderResultSelectForSoapText";
        //Start 09-05-2016 Humaira Yousaf
        private const string PROC_LABRESULT_SPECIMEN_SELECT = "Clinical.sp_LabResultSpecimenSelect";
        //End 09-05-2016 Humaira Yousaf
        private const string PROC_LAB_RESULT_SPECIMEN_INSERT = "Clinical.sp_LabResultSpecimenInsert";
        private const string PROC_SPECIMEN_REJECT_REASON_INSERT = "Clinical.sp_SpecimenRejectReasonInsert";
        //Start 19-05-2016 Humaira Yousaf
        private const string PROC_SPECIMEN_REJECT_REASON_SELECT = "Clinical.sp_SpecimenRejectReasonSelect";
        //End 19-05-2016 Humaira Yousaf

        private const string PROC_NOTES_LABORDER_RESULT_SELECT = "[Clinical].[sp_NotesLabOrderResultSelect]";

        #endregion

        #region Parameters
        //Start//18-04-2016//Abid Ali// Paramenters Names

        private const string PARM_NOTE_ID = "NoteId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_Entity_ID = "@EntityId";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_LAB_ORDER_RESULT_ID = "@LabOrderResultId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_LAB_TEST_ID = "@LabOrderTestId";
        private const string PARM_LAB_ORDER_ID = "@LabOrderId";
        private const string PARM_TEST = "@Test";
        private const string PARM_ORDER_NO = "@OrderNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ORDER_DATE_FROM = "@OrderDateFrom";
        private const string PARM_ORDER_DATE_TO = "@OrderDateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_REFERENCE_RANGE_INTERPRATION = "@ReferenceRangeInterpration";
        private const string PARM_TEST_ANTIMICROBIAL = "@TestAntimicrobial";
        private const string PARM_REFERENCE_RANGE_DESCRIPTION = "@ReferenceRangeDescription";

        private const string PARM_OBSERVATION_DATE = "@ObservationDate";

        private const string PARM_LAB_ORDER_RESULT_DETAIL_ID = "@LabOrderResultDetailId";

        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_DESCRIPTION = "@CPTCodeDescription";

        private const string PARM_LOINC = "@LOINC";
        private const string PARM_LOINC_DECSRIPTION = "@LOINCDescription";

        private const string PARM_RESULT = "@Result";
        private const string PARM_CONDITION_STATEMENT = "@ConditionStatement";
        private const string PARM_UoM = "@UoM";
        private const string PARM_FLAG = "@Flag";
        private const string PARM_RANGE = "@Range";

        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_REMARKS = "@Remarks";
        private const string PARM_FINAL_INTERPRETATION = "@FinalInterpretation";


        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";

        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_MODIFIED_BY = "ModifiedBy";
        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ASSIGNEE_ID = "@AssigneeId";

        private const string PARM_REVIEWEDBY_ID = "@ReviewedById";

        //End//18-04-2016//Abid Ali// Paramenters Names


        //Start 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PARM_LOINC_CODE = "@LOINCCode";
        private const string PARM_LOINC_CODE_DESCRIPTION = "@LOINCCodeDescription";
        //End 18-04-2016 Muhammad Arshad Lookup LOINC


        private const string PARM_LABID = "@LabId";

        //Start 26-04-2016 Muhammad Azhar Shahzad HL7 data 
        private const string PARM_OBSERVATION_RESULT_STATUS = "@ObservationResultStatus";
        private const string PARM_OBSERVATION_VALUE = "@ObservationValue";
        //End 26-04-2016 Muhammad Azhar Shahzad HL7 data 

        private const string PARM_LABORDER_ID = "@LabOrderId";
        private const string PARM_SPECIMEN_ID = "@SpecimenId";
        private const string PARM_COLLECTION_DATETIME = "@CollectionDateTime";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_NAME_OF_CODINGSYSTEM = "@NameofCodingSystem";
        private const string PARM_ORIGINAL_TEXT = "@OriginalText";
        private const string PARM_TEXT = "@Text";
        private const string PARM_LABRESULT_SPECIMEN_ID = "@LabResultSpecimenId";
        private const string PARM_SPECIMEN_TYPE = "@SpecimenType";
        private const string PARM_SPECIMEN_REJECT_REASON_ID = "@SpecimenRejectReasonId";
        private const string PARM_LABORDER_TEST_ID = "@LabOrderTestId";
        private const string PARM_ALTERNATE_TEXT = "@AlternateText";
        private const string PARM_NAMEOF_ALTERNATE_CODING_SYSTEM = "@NameofAlternateCodingSystem";
        private const string PARM_ALTERNATE_IDENTIFIER = "@AlternateIdentifier";
        private const string PARM_LAB_RESULT_SPECIMEN_ID = "@LabResultSpecimenId";
        private const string PARM_IDENTIFIER = "@Identifier";

        private const string PARM_CONDITION_NOC_SYSTEM = "@ConditionNOCSystem";
        private const string PARM_CONDITION_TEXT = "@ConditionText";

        private const string PARM_NAME_OF_ALTERNATE_CODING_SYSTEM = "@NameofAlternateCodingSystem";
        private const string PARM_CONDITION_ORIGINAL_TEXT = "@ConditionOriginalText";

        private const string PARM_IS_SENT_TO_POTRTAL = "@IsSentToPortal";

        private const string PARM_IS_AKNOWLEDGED = "@IsAknowledged";
        private const string PARM_MARKASREVIEWED = "@MarkAsReviewed";

        private const string PARM_IS_Reviewed = "@IsReviewed";
        private const string PARM_IS_All_Result = "@IsAllResult";
        private const string PARM_IS_ATTRIBUTE = "@IsAttribute";

        private const string PARM_LABTEST_ID = "@LabTestId";
        private const string PARM_LABTESTATTRIBUTE_ID = "@LabTestAttributeId";
        private const string PARM_Url = "@Url";

        private const string PARM_IS_PATIENT_PORTAL_STATUS = "@IsPatientPortalStatus";
        private const string PARM_IS_DISABLE_ACCOUNT = "@IsDisabledAccount";
        private const string PARM_IS_UNLOCK_ACCOUNT = "@IsUnlockedAccount";
        #endregion

        #region Constructors

        public DALLabResult()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALLabResult(SharedVariable SharedVariable)
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

        #region Support Functions for Lab Result & Lab Result Detail


        /// <summary>
        /// Lab Result Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateLabOderResultInsertUpdateParameters(IDBManager dbManager, DSLabResult ds, bool isInsert = true)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(22);
                dbManager.AddInsertUpdateParameters(0, PARM_LAB_ORDER_RESULT_ID, ds.LabOrderResult.LabOrderResultIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(22);
                dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ORDER_RESULT_ID, ds.LabOrderResult.LabOrderResultIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ORDER_ID, ds.LabOrderResult.LabOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_ORDER_NO, ds.LabOrderResult.OrderNoColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_STATUS, ds.LabOrderResult.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_COMMENTS, ds.LabOrderResult.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_ACTIVE, ds.LabOrderResult.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_BY, ds.LabOrderResult.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_ON, ds.LabOrderResult.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_BY, ds.LabOrderResult.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_ON, ds.LabOrderResult.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_SOAP_TEXT, ds.LabOrderResult.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(i++, PARM_PROVIDER_ID, ds.LabOrderResult.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_PATIENT_ID, ds.LabOrderResult.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_REMARKS, ds.LabOrderResult.RemarksColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_ASSIGNEE_ID, ds.LabOrderResult.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_SENT_TO_POTRTAL, ds.LabOrderResult.IsSentToPortalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_AKNOWLEDGED, ds.LabOrderResult.IsAknowledgedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_REVIEWEDBY_ID, ds.LabOrderResult.ReviewedByIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_NOTE_ID, ds.LabOrderResult.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_MARKASREVIEWED, ds.LabOrderResult.MarkAsReviewedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_Url, ds.LabOrderResult.UrlColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_FINAL_INTERPRETATION, ds.LabOrderResult.FinalInterpretationColumn.ColumnName, DbType.String);
            //End 22-03-2016 Abid Ali for status
        }


        /// <summary>
        ///Lab Order Result Detail Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateLabOderResultDetailInsertUpdateParameters(IDBManager dbManager, DSLabResult ds, bool isInsert = true)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(34);
                dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ORDER_RESULT_DETAIL_ID, ds.LabOrderResultDetail.LabOrderResultDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(34);
                dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ORDER_RESULT_DETAIL_ID, ds.LabOrderResultDetail.LabOrderResultDetailIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ORDER_RESULT_ID, ds.LabOrderResultDetail.LabOrderResultIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CPT_CODE, ds.LabOrderResultDetail.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CPT_DESCRIPTION, ds.LabOrderResultDetail.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_OBSERVATION_DATE, ds.LabOrderResultDetail.ObservationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_LOINC, ds.LabOrderResultDetail.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_LOINC_DECSRIPTION, ds.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(i++, PARM_CONDITION_STATEMENT, ds.LabOrderResultDetail.ConditionStatementColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(i++, PARM_RESULT, ds.LabOrderResultDetail.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_UoM, ds.LabOrderResultDetail.UoMColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_FLAG, ds.LabOrderResultDetail.FlagColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_RANGE, ds.LabOrderResultDetail.RangeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_STATUS, ds.LabOrderResultDetail.StatusColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(i++, PARM_COMMENTS, ds.LabOrderResultDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_ACTIVE, ds.LabOrderResultDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_BY, ds.LabOrderResultDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_ON, ds.LabOrderResultDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_BY, ds.LabOrderResultDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_ON, ds.LabOrderResultDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_SOAP_TEXT, ds.LabOrderResultDetail.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(i++, PARM_PROVIDER_ID, ds.LabOrderResultDetail.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_PATIENT_ID, ds.LabOrderResultDetail.PatientIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(i++, PARM_OBSERVATION_VALUE, ds.LabOrderResultDetail.ObservationValueColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_IS_ATTRIBUTE, ds.LabOrderResultDetail.IsAttributeColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_OBSERVATION_RESULT_STATUS, ds.LabOrderResultDetail.ObservationResultStatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_LABTEST_ID, ds.LabOrderResultDetail.LabTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_LABTESTATTRIBUTE_ID, ds.LabOrderResultDetail.LabTestAttributeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_LAB_ID, ds.LabOrderResultDetail.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, "@IsOrganismAssociated", ds.LabOrderResultDetail.IsOrganismAssociatedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, "@OrganismCode", ds.LabOrderResultDetail.OrganismCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, "@OrganismCodeDescription", ds.LabOrderResultDetail.OrganismCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_REFERENCE_RANGE_INTERPRATION, ds.LabOrderResultDetail.ReferenceRangeInterprationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_TEST_ANTIMICROBIAL, ds.LabOrderResultDetail.TestAntimicrobialColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_REFERENCE_RANGE_DESCRIPTION, ds.LabOrderResultDetail.ReferenceRangeDescriptionColumn.ColumnName, DbType.String);

        }

        //End//31-03-2016//Abid Ali// Support functions for Lab Result

        #endregion

        #region Lab Result (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 18/044/2016
        //This function will insert/update Lab Result
        public DSLabResult InsertUpdateLabResult(DSLabResult ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.LabOrderResult.GetChanges();
                dbManager.BeginTransaction();
                this.CreateLabOderResultInsertUpdateParameters(dbManager, ds, true);
                this.CreateLabOderResultInsertUpdateParameters(dbManager, ds, false);

                ds = (DSLabResult)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_INSERT, PROC_Lab_ORDER_RESULT_UPDATE, ds, ds.LabOrderResult.TableName);
                if (dtTemp != null && ds.LabOrderResult.Rows.Count > 0)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrderResult.Rows[0][ds.LabOrderResult.LabOrderResultIdColumn].ToString(), null, ds.LabOrderResult.Rows[0][ds.LabOrderResult.LabOrderResultIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALLabResult::PROC_LabResult_INSERT", PROC_Lab_ORDER_RESULT_INSERT + " " + PROC_Lab_ORDER_RESULT_UPDATE, ex);
                throw ex;
            }
        }
        public string GetLabOrderResultDocumentFolderType(long LabOrderResultId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            string data = "";
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@LabOrderResultId", LabOrderResultId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, "[Patient].[sp_GetResultDocumentFolderType]");
                while (reader.Read())
                {
                    data += Convert.ToString(reader["LabOrderResultId"]) + ";" + Convert.ToString(reader["IsPSA"]) + ";" + Convert.ToString(reader["IsUrine"]);
                }
                return data;
            }
            catch (Exception e)
            {
                //dsDBAudit.RejectChanges();
                //   dbManager.RollBackTransaction();
                MDVLogger.SendExcepToDB(e, "DALLabResult::GetLabOrderResultDocumentFolderType", "[Patient].[sp_GetResultDocumentFolderType]");
                throw e;
            }
        }
        public bool CheckDocumentIsReviewed(long LabOrderResultId, long PatientId, string FolderName)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            bool data = false;
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@LabOrderResultId", LabOrderResultId);
                dbManager.AddParameters(1, "@PatientId", PatientId);
                dbManager.AddParameters(2, "@FolderName", FolderName);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, "[Patient].[sp_GetResultDocumentIsReviewed]");
                while (reader.Read())
                {
                    data = Convert.ToBoolean(reader["IsReviewed"]);
                }
                return data;
            }
            catch (Exception e)
            {
                //dsDBAudit.RejectChanges();
                //   dbManager.RollBackTransaction();
                MDVLogger.SendExcepToDB(e, "DALLabResult::CheckDocumentIsReviewed", "[Patient].[sp_GetResultDocumentIsReviewed]");
                throw e;
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
                dbManager.AddParameters(0, "@LabOrderId", null);
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
        // Author: Abid Ali
        // Date: 18/044/2016
        //This function will load Lab Result
        public DSLabResult LoadLabResult(long labResultId, long labOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, string labId, long noteId, long patientId, string isViewOrder = "", string isPrintOrder = "", bool isReviewed = false, bool isAllResult = true, bool isReviewedFromDashBoard = false, SharedVariable sharedVariable = null, string flag = null, bool IsPatientPortalStatus = true, bool IsDisabledAccount = false, bool IsUnlockAccount = true, string PatientPortalStatus = null, string AssigneeId = null)
        {
            DSLabResult ds = new DSLabResult();
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
                dbManager.CreateParameters(23);

                if (labResultId <= 0)
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, labResultId);
                if (labOrderId <= 0)
                    dbManager.AddParameters(1, PARM_LAB_ORDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LAB_ORDER_ID, labOrderId);
                if (page <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.LabOrderResult.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //Start 31-03-2016 Abid Ali 

                if (test == "")
                    dbManager.AddParameters(5, PARM_TEST, null);
                else
                    dbManager.AddParameters(5, PARM_TEST, test);

                if (orderNo == "")
                    dbManager.AddParameters(6, PARM_ORDER_NO, null);
                else
                    dbManager.AddParameters(6, PARM_ORDER_NO, orderNo);

                if (providerId == 0)
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, providerId);

                if (orderDateFrom == "")
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, null);
                else
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, null);
                else
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);

                if (labId == "")
                    dbManager.AddParameters(11, PARM_LAB_ID, null);
                else
                    dbManager.AddParameters(11, PARM_LAB_ID, labId);

                if (noteId <= 0)
                    dbManager.AddParameters(12, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(12, PARM_NOTE_ID, noteId);
                if (patientId <= 0)
                    dbManager.AddParameters(13, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(13, PARM_PATIENT_ID, patientId);

                if (sharedVariable == null)
                {
                    dbManager.AddParameters(14, PARM_Entity_ID, MDVSession.Current.EntityId);
                    dbManager.AddParameters(15, PARM_User_ID, MDVSession.Current.AppUserId);
                }
                else
                {
                    dbManager.AddParameters(14, PARM_Entity_ID, sharedVariable.EntityId);
                    dbManager.AddParameters(15, PARM_User_ID, sharedVariable.AppUserId);
                }

                if (isReviewedFromDashBoard)
                {
                    dbManager.AddParameters(16, PARM_IS_Reviewed, null);
                }
                else
                {
                    dbManager.AddParameters(16, PARM_IS_Reviewed, isReviewed);
                }

                dbManager.AddParameters(17, PARM_IS_All_Result, isAllResult);

                if (String.IsNullOrEmpty(flag))
                {
                    dbManager.AddParameters(18, PARM_FLAG, null);
                }
                else
                {
                    dbManager.AddParameters(18, PARM_FLAG, flag);
                }
                //------- PRD-423 Start
                if (String.IsNullOrEmpty(PatientPortalStatus))          //if PatientPortalStatus is empty str, gets all lab results
                {
                    dbManager.AddParameters(19, PARM_IS_PATIENT_PORTAL_STATUS, null);
                    dbManager.AddParameters(20, PARM_IS_DISABLE_ACCOUNT, null);
                    dbManager.AddParameters(21, PARM_IS_UNLOCK_ACCOUNT, null);
                }
                else if (IsDisabledAccount == true)
                {   //if PatientPortalStatus is disabled, gets all lab results with disabled status, either UnlockAccount = 0 or UnlockAccount = 1
                    dbManager.AddParameters(19, PARM_IS_PATIENT_PORTAL_STATUS, IsPatientPortalStatus);
                    dbManager.AddParameters(20, PARM_IS_DISABLE_ACCOUNT, IsDisabledAccount);
                    dbManager.AddParameters(21, PARM_IS_UNLOCK_ACCOUNT, null);
                }
                else if (IsPatientPortalStatus == false)
                {           //if PatientPortalStatus is not enabled, gets all lab results; patient's record not exists in Patient.PatientLogin table
                    dbManager.AddParameters(19, PARM_IS_PATIENT_PORTAL_STATUS, IsPatientPortalStatus);
                    dbManager.AddParameters(20, PARM_IS_DISABLE_ACCOUNT, null);
                    dbManager.AddParameters(21, PARM_IS_UNLOCK_ACCOUNT, null);
                }
                else           //if PatientPortalStatus is enabled/locked, gets all lab results. In case of locked, DisableAccount = 0.
                {
                    dbManager.AddParameters(19, PARM_IS_PATIENT_PORTAL_STATUS, IsPatientPortalStatus);
                    dbManager.AddParameters(20, PARM_IS_DISABLE_ACCOUNT, IsDisabledAccount);
                    dbManager.AddParameters(21, PARM_IS_UNLOCK_ACCOUNT, IsUnlockAccount);
                }
                //------- PRD-423 End

                if (string.IsNullOrEmpty(AssigneeId))
                    dbManager.AddParameters(22, PARM_ASSIGNEE_ID, null);
                else
                    dbManager.AddParameters(22, PARM_ASSIGNEE_ID, AssigneeId);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_SELECT, ds, ds.LabOrderResult.TableName);
                if (labOrderId > 0)
                {

                    DataTable dtTemp = ds.LabOrderResult;
                    if (dtTemp != null)
                    {

                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            if (ds.LabOrderResult.Rows.Count > 0)
                            {
                                string labOrderResultId_audit = ds.LabOrderResult.Rows[0][ds.LabOrderResult.LabOrderResultIdColumn].ToString();
                                new DBActivityAudit().InsertDBAuditAsync(dtTemp, labOrderResultId_audit, null, labOrderResultId_audit, isViewAction, isPrintAcion, false, "", "0", sharedVariable);
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
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.SendExcepToDB(ex, "DALLabResult::Lab_ORDER_SELECT", PROC_Lab_ORDER_RESULT_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string GetAssignedLabResultsCount(long AssigneeId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ASSIGNEE_ID, AssigneeId);

                var AssignedResultsCount = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_ASSIGNED_RESULTS_COUNT);

                return MDVUtility.ToStr(AssignedResultsCount);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::GetAssignedLabResultsCount", PROC_GET_ASSIGNED_RESULTS_COUNT, ex);
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

        // Author: Farooq Ahmad
        // Date: 22/08/2016
        //This function will load Lab Result
        public DSLabResult LoadLabUnsolicitedResult(long labResultId, long labOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, string labId, string isViewOrder = "", string isPrintOrder = "", bool isReviewed = false, bool isAllResult = true, bool isReviewedDashboard = false, SharedVariable sharedVariable = null)
        {
            DSLabResult ds = new DSLabResult();
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
                    rpp = 2000;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                dbManager.BeginTransaction();
                dbManager.CreateParameters(15);

                if (labResultId == 0)
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, labResultId);
                if (labOrderId <= 0)
                    dbManager.AddParameters(1, PARM_LAB_ORDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LAB_ORDER_ID, labOrderId);
                if (page <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.LabOrderResult.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //Start 31-03-2016 Abid Ali 

                if (test == "")
                    dbManager.AddParameters(5, PARM_TEST, null);
                else
                    dbManager.AddParameters(5, PARM_TEST, test);

                if (orderNo == "")
                    dbManager.AddParameters(6, PARM_ORDER_NO, null);
                else
                    dbManager.AddParameters(6, PARM_ORDER_NO, orderNo);

                if (providerId == 0)
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, providerId);

                if (orderDateFrom == "")
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, null);
                else
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, null);
                else
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);

                if (labId == "")
                    dbManager.AddParameters(11, PARM_LAB_ID, null);
                else
                    dbManager.AddParameters(11, PARM_LAB_ID, labId);

                if (sharedVariable == null)
                {
                    dbManager.AddParameters(12, PARM_User_ID, MDVSession.Current.AppUserId);
                }
                else
                {
                    dbManager.AddParameters(12, PARM_User_ID, sharedVariable.AppUserId);
                }

                if (isReviewedDashboard)
                {
                    dbManager.AddParameters(13, PARM_IS_Reviewed, null);
                }
                else
                {
                    dbManager.AddParameters(13, PARM_IS_Reviewed, isReviewed);
                }

                dbManager.AddParameters(14, PARM_IS_All_Result, isAllResult);


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_Unsolicited_RESULT_SELECT, ds, ds.LabOrderResult.TableName);
                if (labOrderId > 0)
                {

                    DataTable dtTemp = ds.LabOrderResult;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            if (ds.LabOrderResult.Rows.Count > 0)
                            {
                                string labOrderResultId_audit = ds.LabOrderResult.Rows[0][ds.LabOrderResult.LabOrderResultIdColumn].ToString();
                                new DBActivityAudit().InsertDBAuditAsync(dtTemp, labOrderResultId_audit, null, labOrderResultId_audit, isViewAction, isPrintAcion, false, "", "0", sharedVariable);
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
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.SendExcepToDB(ex, "DALLabResult::Lab_ORDER_SELECT", PROC_Lab_ORDER_RESULT_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will delete Lab Result
        public string DeleteLabResult(string labResultId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                //DSLabResult dsCurrentOrder = LoadLabResult(MDVUtility.ToLong(labResultId), 0, "", "", "", "", 0, "", "", "", "", 1, 0, "", "");
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, labResultId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_Entity_ID, MDVSession.Current.EntityId);
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DELETE));

                dbManager.CommitTransaction();
                if (returnVal.IndexOf('|') > -1)
                {
                    string[] str = returnVal.Split('|');
                    if (str.Length > 1)
                        returnVal = str[1].ToString();
                }

                return returnVal;

            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabResult::DeleteLabResult", PROC_Lab_ORDER_RESULT_DELETE, ex);
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
        public string DeleteLabTest(long labTestId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_LAB_TEST_ID, labTestId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_TEST_DELETE).ToString();
                dbManager.CommitTransaction();
                //if (returnVal != "")
                //    throw new Exception(returnVal);
                //else
                //{
                //    dbManager.CommitTransaction();
                //}
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabResult::DeleteLabTest", PROC_Lab_TEST_DELETE, ex);
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

        #region LabResult Detail (Insert, Update, Delete, Select)

        /// <summary>
        /// Method Name: insertUpdateLabResultDetails
        /// Author: Abid Ali
        /// Created Date: 18-04-2016
        /// Description: insert/update  Lab Result Details
        /// </summary> 
        /// <param name="DSLabResult" type="DATASET"></param>
        public DSLabResult insertUpdateLabResultDetail(DSLabResult ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.LabOrderResultDetail.GetChanges();
                dbManager.BeginTransaction();
                CreateLabOderResultDetailInsertUpdateParameters(dbManager, ds, true);
                CreateLabOderResultDetailInsertUpdateParameters(dbManager, ds, false);
                ds = (DSLabResult)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_INSERT, PROC_Lab_ORDER_RESULT_DETAIL_UPDATE, ds, ds.LabOrderResultDetail.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.LabOrderResultDetail.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > i)
                        {
                            dtTemp.Rows[i]["PrimaryKey"] = ds.LabOrderResultDetail.Rows[i][ds.LabOrderResultDetail.LabOrderResultDetailIdColumn];
                        }
                        else
                        {
                            break;
                        }

                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.LabOrderResultDetail.Rows[0][ds.LabOrderResultDetail.LabOrderResultDetailIdColumn].ToString(), null, ds.LabOrderResultDetail.Rows[0][ds.LabOrderResultDetail.LabOrderResultIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALLabResult::insertUpdateLabResultDetail", PROC_Lab_ORDER_RESULT_DETAIL_INSERT + " " + PROC_Lab_ORDER_RESULT_DETAIL_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Method Name: loadLabResultDetaii
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: loading Lab Result Detail
        /// </summary> 
        /// <param name="LabResultId" type="long">LabOrderResultId</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>  
        public DSLabResult loadLabResultDetail(long LabOrderResultDetailId, long LabOrderResultId, long PatientId = 0, int pageNumber = 1, int rowsPerPage = 1000, SharedVariable sharedVariable = null)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (LabOrderResultDetailId == 0)
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, LabOrderResultDetailId);

                if (LabOrderResultId == 0)
                    dbManager.AddParameters(1, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LAB_ORDER_RESULT_ID, LabOrderResultId);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);
                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rowsPerPage);

                if (sharedVariable == null)
                {
                    dbManager.AddParameters(5, PARM_User_ID, MDVSession.Current.AppUserId);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_User_ID, sharedVariable.AppUserId);
                }

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.LabOrderResultDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_SELECT, ds, ds.LabOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.SendExcepToDB(ex, "DALLabOrderResultDetail::loadLabOrderResultDetail", PROC_Lab_ORDER_RESULT_DETAIL_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSLabResult loadLabResultDetailForFaceSheet(long PatientId = 0)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_SELECT_FACESHEET, ds, ds.LabOrderResultPDF.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrderResultDetail::loadLabResultDetailForFaceSheet", PROC_Lab_ORDER_RESULT_DETAIL_SELECT_FACESHEET, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSLabResult LoadLabResultForCDS(long patientId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, null);
                dbManager.AddParameters(1, PARM_LAB_ORDER_RESULT_ID, null);
                if (patientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);
                dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, 2000);
                dbManager.AddParameters(5, PARM_User_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.LabOrderResultDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_SELECT, ds, ds.LabOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabOrderResultDetail::LoadLabResultForCDS", PROC_Lab_ORDER_RESULT_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Method Name: deleteLabResultDetail
        /// Author: Abid Ali
        /// Created Date: 18-04-2016
        /// Description: deleting Lab Result Detail
        /// </summary> 
        /// <param name="LabResultId" type="long">LabResultId to be deleted</param>
        /// 
        public string deleteLabResultDetail(long labResultDetailId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSLabResult dsCurrentOrderDetails = loadLabResultDetail(labResultDetailId, 0, 0);
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_DETAIL_ID, labResultDetailId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Lab_ORDER_RESULT_DETAIL_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderDetails.LabOrderResultDetail;
                    if (dtTemp != null)
                    {

                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderDetails.LabOrderResultDetail.Rows[0].ToString(), null, dsCurrentOrderDetails.LabOrderResultDetail.Rows[0][dsCurrentOrderDetails.LabOrderResultDetail.LabOrderResultIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALLabResult::deleteLabResultDetail", PROC_Lab_ORDER_RESULT_DETAIL_DELETE, ex);
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
        /// Reason:  This function will handle attach of Lab Result with Note 
        /// </summary>
        /// <param name="LabResultId"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public DSLabResult attachLabResultWithNotes(string LabResultId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSLabResult ds = new DSLabResult();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(LabResultId))
                {
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, LabResultId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_LABORDER_RESULT_WITH_NOTES, ds, ds.LabOrderResult.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::attachLabResultWithNotes", PROC_ATTACH_LABORDER_RESULT_WITH_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string attachLabOrderTestWithNotes(long LabOrderTestId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSLabResult ds = new DSLabResult();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (LabOrderTestId == null)
                {
                    dbManager.AddParameters(0, "@LabOrderTestId", DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, "@LabOrderTestId", LabOrderTestId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[Clinical].[sp_AttachLabOrderTestWithNotes]", ds, ds.LabOrderResult.TableName);


                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::attachLabResultWithNotes", "[Clinical].[sp_AttachLabOrderTestWithNotes]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string detachLabOrderTestWithNotes(long LabOrderTestId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSLabResult ds = new DSLabResult();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (LabOrderTestId == null)
                {
                    dbManager.AddParameters(0, "@LabOrderTestId", DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, "@LabOrderTestId", LabOrderTestId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[Clinical].[sp_DetachLabOrderTestWithNotes]", ds, ds.LabOrderResult.TableName);


                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::attachLabResultWithNotes", "[Clinical].[sp_DetachLabOrderTestWithNotes]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        ///// <summary>
        ///// Method Name: detachLabResultFromNotes
        ///// Author:  Abid Ali
        ///// Date:    31-03-2016
        ///// Reason:  This function will handle detach of Lab Result from Note 
        ///// </summary>
        ///// <param name="LabResultId"></param>
        ///// <param name="NoteId"></param>
        ///// <returns></returns>
        public string detachLabResultFromNotes(string LabResultId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(LabResultId))
                {
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, LabResultId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_LAB_ORDER_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::detachLabResultFromNotes", PROC_DETACH_LAB_ORDER_FROM_NOTES, ex);
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

        #region Lookups

        public DSLabResult LookupLabResultLOINC(string LOINCCode = "", string LOINCCOdeDescription = "", string LabId = "")
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LOINCCode == "")
                    LOINCCode = null;

                if (LOINCCOdeDescription == "")
                    LOINCCOdeDescription = null;

                if (LabId == "")
                    LabId = null;

                dbManager.Open();

                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_LOINC_CODE, LOINCCode);
                dbManager.AddParameters(1, PARM_LOINC_CODE_DESCRIPTION, LOINCCOdeDescription);
                dbManager.AddParameters(2, PARM_LABID, LabId);



                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDER_RESULT_LOINC_LOOKUP, ds, ds.OrderResultLOINC.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::LookupLabResult", PROC_ORDER_RESULT_LOINC_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabResult LookupLabResultOrganism(string SearchQuery = "")
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();

                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, "@SearchQuery", SearchQuery);




                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDER_RESULT_ORGANISM_LOOKUP, ds, ds.OrderResultOrganisms.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::LookupLabResult", PROC_ORDER_RESULT_ORGANISM_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public DSLabResult loadLabResultExternalPDF(long labOrderResultExternalPDFId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@LabOrderResultExternalPDFId", labOrderResultExternalPDFId);
                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_LabOrderResultExternalPDFSelect", ds, ds.LabOrderResultExternalPDF.TableName);
                return ds;
            }
            catch (Exception e)
            {
                MDVLogger.DALErrorLog("DALLabResult::loadResultExternalPDF", PROC_LABORDER_RESULT_SELECT_FOR_SOAPTEXT, e);
                throw e;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string acknowledgeLabOrderResult(long labOrderResultId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@LabOrderResultId", labOrderResultId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "Clinical.sp_AcknowledgeLabOrderResult").ToString();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALLabResult::AcknowledgeLabOrderResult", "Clinical.sp_AcknowledgeLabOrderResult", ex);
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
        public DSLabResult loadResultsForSoap(string resultID, long patientId, long NotesId, long ProviderId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);


                if (string.IsNullOrEmpty(resultID))
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_ORDER_RESULT_ID, resultID);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                if (NotesId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NotesId);
                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);

                List<string> tableNames = new List<string>
                {
                    ds.LabOrderResult.TableName,
                    ds.LabOrderResultDetail.TableName,
                    ds.LabOrderResultSoapText.TableName
                };

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LABORDER_RESULT_SELECT_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::loadResultsForSoap", PROC_LABORDER_RESULT_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region specimen
        public DSLabResult insertLabResultSpecimen(DSLabResult ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);
                dbManager.AddParameters(0, PARM_LABRESULT_SPECIMEN_ID, ds.LabResultSpecimen.LabResultSpecimenIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);


                dbManager.AddParameters(1, PARM_SPECIMEN_TYPE, ds.LabResultSpecimen.SpecimenTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_TEXT, ds.LabResultSpecimen.TextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_ORIGINAL_TEXT, ds.LabResultSpecimen.OriginalTextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_NAME_OF_CODINGSYSTEM, ds.LabResultSpecimen.NameofCodingSystemColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_QUANTITY, ds.LabResultSpecimen.QuantityColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_COLLECTION_DATETIME, ds.LabResultSpecimen.CollectionDateTimeColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(7, PARM_SPECIMEN_ID, ds.LabResultSpecimen.SpecimenIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(8, PARM_LABORDER_TEST_ID, ds.LabResultSpecimen.LabOrderTestIdColumn.ColumnName, DbType.Int64);

                dbManager.AddParameters(9, PARM_IDENTIFIER, ds.LabResultSpecimen.IdentifierColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_CONDITION_TEXT, ds.LabResultSpecimen.ConditionTextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_CONDITION_NOC_SYSTEM, ds.LabResultSpecimen.ConditionNOCSystemColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, PARM_ALTERNATE_IDENTIFIER, ds.LabResultSpecimen.AlternateIdentifierColumn.ColumnName, DbType.String);
                dbManager.AddParameters(13, PARM_ALTERNATE_TEXT, ds.LabResultSpecimen.AlternateTextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_NAME_OF_ALTERNATE_CODING_SYSTEM, ds.LabResultSpecimen.NameofAlternateCodingSystemColumn.ColumnName, DbType.String);
                dbManager.AddParameters(15, PARM_CONDITION_ORIGINAL_TEXT, ds.LabResultSpecimen.ConditionOriginalTextColumn.ColumnName, DbType.String);


                ds = (DSLabResult)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_LAB_RESULT_SPECIMEN_INSERT, ds, ds.LabResultSpecimen.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();

                MDVLogger.DALErrorLog("DALLabResult::insertLabResultSpecimen", PROC_LAB_RESULT_SPECIMEN_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSLabResult insertLabResultSpecimenRejectReason(DSLabResult ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);
                dbManager.AddParameters(0, PARM_SPECIMEN_REJECT_REASON_ID, ds.SpecimenRejectReason.SpecimenRejectReasonIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);


                dbManager.AddParameters(1, PARM_IDENTIFIER, ds.SpecimenRejectReason.IdentifierColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_TEXT, ds.SpecimenRejectReason.TextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_NAME_OF_CODINGSYSTEM, ds.SpecimenRejectReason.NameofCodingSystemColumn.ColumnName, DbType.String);


                dbManager.AddParameters(4, PARM_ALTERNATE_IDENTIFIER, ds.SpecimenRejectReason.AlternateIdentifierColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_ALTERNATE_TEXT, ds.SpecimenRejectReason.AlternateTextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_NAMEOF_ALTERNATE_CODING_SYSTEM, ds.SpecimenRejectReason.NameofAlternateCodingSystemColumn.ColumnName, DbType.String);

                dbManager.AddParameters(7, PARM_ORIGINAL_TEXT, ds.SpecimenRejectReason.OriginalTextColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_LAB_RESULT_SPECIMEN_ID, ds.SpecimenRejectReason.LabResultSpecimenIdColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(9, PARM_LABORDER_TEST_ID, ds.SpecimenRejectReason.LabOrderTestIdColumn.ColumnName, DbType.Int64);


                ds = (DSLabResult)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SPECIMEN_REJECT_REASON_INSERT, ds, ds.SpecimenRejectReason.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();

                MDVLogger.DALErrorLog("DALLabResult::insertLabResultSpecimenRejectReason", PROC_SPECIMEN_REJECT_REASON_INSERT, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        ///// <summary>
        ///// Method Name: loadLabResultSpecimen
        ///// Author:  Humaira Yousaf
        ///// Date:    09-05-2016
        ///// Reason:  Loads Specimen for lab result 
        ///// </summary>
        ///// <param name="specimenId"></param>
        ///// <param name="laborderId"></param>
        ///// <returns></returns>
        public DSLabResult loadLabResultSpecimen(long specimenId, long labordertestId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (specimenId == 0)
                    dbManager.AddParameters(0, PARM_LABRESULT_SPECIMEN_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LABRESULT_SPECIMEN_ID, specimenId);
                if (labordertestId == 0)
                    dbManager.AddParameters(1, PARM_LABORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LABORDER_TEST_ID, labordertestId);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LABRESULT_SPECIMEN_SELECT, ds, ds.LabResultSpecimen.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::loadLabResultSpecimen", PROC_LABRESULT_SPECIMEN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSLabResult loadLabResultSpecimenRejectReason(long specimenRejectReasonId, long laborderTestId)
        {
            DSLabResult ds = new DSLabResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (specimenRejectReasonId == 0)
                    dbManager.AddParameters(0, PARM_SPECIMEN_REJECT_REASON_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SPECIMEN_REJECT_REASON_ID, specimenRejectReasonId);
                if (laborderTestId == 0)
                    dbManager.AddParameters(1, PARM_LABORDER_TEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LABORDER_TEST_ID, laborderTestId);

                ds = (DSLabResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIMEN_REJECT_REASON_SELECT, ds, ds.SpecimenRejectReason.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::loadLabResultSpecimenRejectReason", PROC_SPECIMEN_REJECT_REASON_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Legacy Notes

        public List<LabOrderResult> NotesLabOrderResultSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<LabOrderResult> objList_LabOrderResult = new List<LabOrderResult>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_LABORDER_RESULT_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        LabOrderResult model = new LabOrderResult();
                        var properties = typeof(LabOrderResult).GetProperties();
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
                        objList_LabOrderResult.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::NotesLabOrderResultSelect", PROC_NOTES_LABORDER_RESULT_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_LabOrderResult;
        }

        #endregion Legacy Notes

    }
}
