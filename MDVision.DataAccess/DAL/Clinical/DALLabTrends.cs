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
using MDVision.Model.Clinical.LabTrends;



namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALLabTrends
    {
        #region Variable

        #endregion



        #region Parameters
        //Start//18-04-2016//Abid Ali// Paramenters Names

        private const string PARM_NOTE_ID = "NoteId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_Entity_ID = "@EntityId";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_LAB_ORDER_RESULT_ID = "@LabOrderResultId";
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


        #endregion

        #region Constructors

        public DALLabTrends()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALLabTrends(SharedVariable SharedVariable)
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
        public List<LabTrends> fetch_LabResultTrends(long LabOrderResultId, string FilterSearch, string DateFrom, string DateTo)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<LabTrends> objList_LabOrderResult = new List<LabTrends>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LabOrderResultId", LabOrderResultId));
                parameters.Add(new SqlParameter("@LabTestName", FilterSearch == "" ? null : FilterSearch));
                parameters.Add(new SqlParameter("@DateFrom", DateFrom == "" ? null : DateFrom));
                parameters.Add(new SqlParameter("@DateTo", DateTo == "" ? null : DateTo));
                using (var reader = dbManager.ExecuteReader("[Clinical].[sp_FetchLabResultTrends]", parameters))
                {
                    while (reader.Read())
                    {
                        LabTrends model = new LabTrends();
                        model.TestCode = Convert.ToString(reader["CPTCode"]);
                        model.TestDescription = Convert.ToString(reader["CPTCodeDescription"]);
                        model.ResultDatesXML = Convert.ToString(reader["ResultDates"]);
                        model.TestsXML = Convert.ToString(reader["Tests"]);


                        objList_LabOrderResult.Add(model);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        PatPracticeModel Patmodel = new PatPracticeModel();
                        Patmodel.PatientId = Convert.ToInt64(reader["PatientId"]);
                        Patmodel.FirstName = Convert.ToString(reader["FirstName"]);
                        Patmodel.LastName = Convert.ToString(reader["LastName"]);
                        Patmodel.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                        Patmodel.Gender = Convert.ToString(reader["Gender"]);
                        Patmodel.PatientAddress = Convert.ToString(reader["PatientAddress"]);
                        Patmodel.PatientCity = Convert.ToString(reader["PatientCity"]);
                        Patmodel.PatientState = Convert.ToString(reader["PatientState"]);
                        Patmodel.PatientZipCode = Convert.ToString(reader["PatientZipCode"]);
                        Patmodel.PatientDOB = Convert.ToString(reader["PatientDOB"]);
                        Patmodel.PracticeName = Convert.ToString(reader["PracticeName"]);
                        Patmodel.PracticeAddress = Convert.ToString(reader["PracticeAddress"]);
                        Patmodel.PracticeCity = Convert.ToString(reader["PracticeCity"]);
                        Patmodel.PracticeState = Convert.ToString(reader["PracticeState"]);
                        Patmodel.PracticeZIP = Convert.ToString(reader["PracticeZIP"]);

                        Patmodel.ProviderName = Convert.ToString(reader["ProviderName"]);
                        Patmodel.ProviderNPI = Convert.ToString(reader["NPI"]);
                        Patmodel.ProviderSpecialty = Convert.ToString(reader["SpecialtyName"]);

                        objList_LabOrderResult[0].PatPracticeModel = Patmodel;
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLabResult::NotesLabOrderResultSelect", "[Clinical].[sp_FetchLabResultTrends]", ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_LabOrderResult;
        }
        public List<Letter> get_LabTemps()
        {
            List<Letter> obj = new List<Letter>();
            
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<LabTrends> objList_LabOrderResult = new List<LabTrends>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserId", MDVSession.Current.AppUserId));
                using (var reader = dbManager.ExecuteReader("[Clinical].[sp_LabTemplateLetterLookUp]", parameters))
                {
                    while (reader.Read())
                    {
                        Letter let = new Letter();
                        let.TemplateId = Convert.ToInt16(reader["TemplateLetterId"]);
                        let.TemplateName = Convert.ToString(reader["TemplateLetter"]);



                        obj.Add(let);
                    }
                }
                return obj;
            }
            catch(Exception e)
            {
                return null;
            }            
        }
    }
}
