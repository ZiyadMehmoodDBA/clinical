/* Author:  Muhammad Arshad
 * Created Date: 25/02/2016
 * OverView: Created for CDS in Clinical Module
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
using System.Reflection.Emit;
using System.Reflection;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Model.Clinical.Medical.CDS;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALCDS
    {
        //Start 02-03-2016 Humaira Yousaf


        #region Stored Procedure Names
        private const string PROC_CDS_PATIENT_SELECT = "Clinical.sp_CDSPatientSelect";
        private const string PROC_CDS_DELETE = "Clinical.sp_CDSDelete";
        private const string PROC_CDS_AgeCondition_Lookup = "Clinical.sp_CDS_AgeConditionLookup";
        private const string PROC_CDS_ReminderPeriod_Lookup = "Clinical.sp_CDS_ReminderPeriodLookup";
        private const string PROC_CDS_RuleType_Lookup = "Clinical.sp_CDS_RuleTypeLookup";
        private const string PROC_CDS_TriggerLocation_Lookup = "Clinical.sp_CDS_TriggerLocationLookup";
        private const string PROC_CDS_INSERT = "Clinical.sp_CDSInsert";
        private const string PROC_CDS_ORDER_SET_SELECT = "Clinical.sp_CDSOrderSetSelect";

        private const string PROC_CDS_SELECT = "Clinical.sp_CDSSelect";
        private const string PROC_CDS_SELECT_WITH_PATIENTSTATUS = "Clinical.sp_CDSWithPatientStatus";

        private const string PROC_CDS_PATIENT_STATUS_SELECT = "Clinical.sp_CDSPatientStatusSelect";
        private const string PROC_CDS_UPDATE = "Clinical.sp_CDSUpdate";

        private const string PROC_CDS_PATIENT_STATUSINSERT = "Clinical.sp_CDSPatientStatusInsert";
        private const string PROC_CDS_PATIENT_STATUSUPDATE = "Clinical.sp_CDSPatientStatusUpdate";
        private const string PROC_CDS_PATIENT_STATUS_DELETE = "Clinical.sp_CDSPatientStatusDelete";       

        //Start 08-03-2016 Humaira Yousaf for CDS Vitals
        private const string PROC_CDSVITALS_INSERT = "Clinical.sp_CDSVitalsInsert";
        private const string PROC_CDSVITALS_UPDATE = "Clinical.sp_CDSVitalsUpdate";
        private const string PROC_CDSVITALS_SELECT = "Clinical.sp_CDSVitalsSelect";
        private const string PROC_CDSVITALS_DELETE = "Clinical.sp_CDSVitalsDelete";
        //End 08-03-2016 Humaira Yousaf for CDS Vitals

        //Start 09-03-2016 Humaira Yousaf for CDS Medication
        private const string PROC_CDSMEDICATION_INSERT = "Clinical.sp_CDSMedicationInsert";
        private const string PROC_CDSMEDICATION_UPDATE = "Clinical.sp_CDSMedicationUpdate";
        private const string PROC_CDSMEDICATION_SELECT = "Clinical.sp_CDSMedicationSelect";
        private const string PROC_CDSMEDICATION_DELETE = "Clinical.sp_CDSMedicationDelete";
        //End 09-03-2016 Humaira Yousaf for CDS Medication

        private const string PROC_CDSALLERGY_INSERT = "Clinical.sp_CDSAllergyInsert";
        private const string PROC_CDSALLERGY_SELECT = "Clinical.sp_CDSAllergySelect";
        private const string PROC_CDSALLERGY_UPDATE = "Clinical.sp_CDSAllergyUpdate";
        private const string PROC_CDSALLERGY_DELETE = "Clinical.sp_CDSAllergyDelete";

        private const string PROC_CDSPROBLEM_INSERT = "Clinical.sp_CDSProblemInsert";
        private const string PROC_CDSPROBLEM_SELECT = "Clinical.sp_CDSProblemSelect";
        private const string PROC_CDSPROBLEM_UPDATE = "Clinical.sp_CDSProblemUpdate";
        private const string PROC_CDSPROBLEM_DELETE = "Clinical.sp_CDSProblemDelete";
        private const string PROC_CDSLABRESULT_SELECT = "Clinical.sp_CDSLabResultSelect";
        private const string PROC_CDSLABRESULT_INSERT = "Clinical.sp_CDSLabResultInsert";
        private const string PROC_CDSLABRESULT_UPDATE = "Clinical.sp_CDSLabResultUpdate";
        private const string PROC_CDSLABRESULT_DELETE = "Clinical.sp_CDSLabResultDelete";
        private const string PROC_CDS_RECURSIVE_PERIOD_LOOKUP = "Clinical.sp_CDS_RecursivePeriodLookup";
        private const string PROC_CDS_QUESTIONNAIRE_CONTROL_TYPE_LOOKUP = "Clinical.sp_CDS_QuestionnaireControlTypeLookup";
        private const string PROC_CDQUESTIONNAIRE_SELECT = "Clinical.sp_CDSQuestionnaireSelect";
        private const string PROC_CDQUESTIONNAIRE_INSERT = "Clinical.sp_CDSQuestionnaireInsert";
        private const string PROC_CDQUESTIONNAIRE_UPDATE = "Clinical.sp_CDSQuestionnaireUpdate";
        private const string PROC_CDQUESTIONNAIRE_DELETE = "Clinical.sp_CDSQuestionnaireDelete";
        private const string PROC_CDS_SELECT_WITH_PATIENT_STATUS = "Clinical.sp_CDSSelectWithPatientStatus";
        private const string PROC_CDS_SEARCH = "Clinical.sp_CDSSearch";

        #region CDS INSURANCE SP NAMES

        private const string PROC_CDS_INSURANCE_INSERT = "Clinical.sp_CDSInsuranceInsert";
        private const string PROC_CDS_INSURANCE_UPDATE = "Clinical.sp_CDSInsuranceUpdate";
        private const string PROC_CDS_INSURANCE_SELECT = "Clinical.sp_CDSInsuranceSelect";
        private const string PROC_CDS_INSURANCE_DELETE = "Clinical.sp_CDSInsuranceDelete";
        #endregion

        #endregion

        #region Parameters
        private const string PARM_CDS_ID = "@CDSId";
        private const string PARM_CDS_IDs = "@CDSIds";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_CDS_PATIENT_STATUS_ID = "@CDSPatientStatusId";
        private const string PARM_CDS_PATIENT_STATUS_IDS = "@CDSPatientStatusIds";
        private const string PARM_CDS_TITLE = "@Title";
        private const string PARM_CDS_DEVELOPER = "@Developer";
        private const string PARM_CDS_FUNDINGSOURCE = "@FundingSource";
        private const string PARM_CDS_REFERENCEURL = "@ReferenceURL";
        private const string PARM_CDS_RELEASE = "@Release";
        private const string PARM_CDS_REVISIONDATE = "@RevisionDate";
        private const string PARM_CDS_TRIGGERLOCATION = "@TriggerLocation";
        private const string PARM_CDS_ROLE = "@Role";
        private const string PARM_CDS_RULETYPE = "@RuleType";
        private const string PARM_CDS_GENDER = "@Gender";
        private const string PARM_CDS_ETHNICITY = "@Ethnicity";
        private const string PARM_CDS_RACE = "@Race";
        private const string PARM_CDS_LANGUAGE = "@Language";
        private const string PARM_CDS_REMINDERLENGTH = "@ReminderLength";
        private const string PARM_CDS_REMINDERPERIODID = "@ReminderPeriodId";
        private const string PARM_CDS_ISRECURSIVE = "@IsRecursive";
        private const string PARM_CDS_PROBLEMLISTOPERATOR = "@ProblemListOperator";
        private const string PARM_CDS_CDSPROBLEMLISTQUERY = "@CDSProblemListQuery";
        private const string PARM_CDS_ALLERGIESOPERATOR = "@AllergiesOperator";
        private const string PARM_CDS_ALLERGIESQUERY = "@AllergiesQuery";
        private const string PARM_CDS_MEDICATIONSOPERATOR = "@MedicationsOperator";
        private const string PARM_CDS_MEDICATIONSQUERY = "@MedicationsQuery";
        private const string PARM_CDS_LABSOPERATOR = "@LabsOperator";
        private const string PARM_CDS_LABSQUERY = "@LabsQuery";
        private const string PARM_CDS_VITALSOPERATOR = "@VitalsOperator";
        private const string PARM_CDS_VITALSQUERY = "@VitalsQuery";
        private const string PARM_CDS_COMMENTS = "@Comments";
        private const string PARM_CDS_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_CDS_ISACTIVE = "@IsActive";
        private const string PARM_CDS_CREATED_BY = "@CreatedBy";
        private const string PARM_CDS_CREATED_ON = "@CreatedOn";
        private const string PARM_CDS_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CDS_CDSQUERY = "@CDSQuery";
        //Start 03-03-2016 Humaira Yousaf for paging
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        //End 03-03-2016 Humaira Yousaf for paging

        //Start 08-03-2016 Humaira Yousaf for CDS Vitals
        private const string PARM_CDSVITALS_ID = "@CDSVitalsId";
        private const string PARM_CDSVITALS_HEIGHT = "@Height";
        private const string PARM_CDSVITALS_WEIGHT = "@Weight";
        private const string PARM_CDSVITALS_BMI = "@BMI";
        private const string PARM_CDSVITALS_SYSTOLIC = "@Systolic";
        private const string PARM_CDSVITALS_DIASTOLIC = "@Diastolic";
        private const string PARM_CDSVITALS_VITALSQUERY = "@VitalsQuery";

        private const string PARM_CDSVITALS_LOGIC = "@VitalsLogic";
        private const string PARM_CDSVITALS_TYPE = "@VitalsType";
        private const string PARM_CDSVITALS_LOGICAL_OPERATOR = "@VitalsLogicalOperator";
        private const string PARM_CDSVITALS_VALUE = "@VitalsValue";
        private const string PARM_CDSVITALS_VALUE_FROM = "@VitalsValueFrom";
        private const string PARM_CDSVITALS_VALUE_TO = "@VitalsValueTo";
        private const string PARM_CDSVITALS_UNIT = "@VitalsUnit";
        private const string PARM_CDS_PROVIDER_IDs = "@ProviderIds";
        

        //End 08-03-2016 Humaira Yousaf for CDS Vitals

        //Start 09-03-2016 Humaira Yousaf for CDS Medication
        private const string PARM_CDSMEDICATION_ID = "@CDSMedicationId";
        private const string PARM_CDSMEDICATION_DRUGID = "@DrugId";
        private const string PARM_CDSMEDICATION_MEDICATIONQUERY = "@CDSMedicationQuery";
        private const string PARM_CDSMEDICATION_MEDICATIONOPERATOR = "@MedicationOperator";
        //End 09-03-2016 Humaira Yousaf for CDS Medication

        //Start 10-03-2016 Humaira Yousaf for CDS Age Condition
        private const string PARM_CDS_STATUS = "@Status";
        private const string PARM_CDS_END_DATE = "@EndDate";
        private const string PARM_CDS_AGECONDITION = "@AgeConditionId";
        private const string PARM_CDS_FROMAGE = "@FromAge";
        private const string PARM_CDS_TOAGE = "@ToAge";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        //End 10-03-2016 Humaira Yousaf for CDS Age Condition

        private const string PARM_CDSALLERGY_ID = "@CDSAllergyId";
        private const string PARM_CDSALLERGY_ALLERGEN = "@Allergen";
        private const string PARM_CDSALLERGY_ALLERGYQUERY = "@CDSAllergyQuery";
        private const string PARM_CDSALLERGY_ALLERGYOPERATOR = "@AllergyOperator";

        private const string PARM_CDSPROBLEM_ID = "@CDSProblemId";
        private const string PARM_CDSPROBLEM_PROBLEM = "@Problem";
        private const string PARM_CDSPROBLEM_PROBLEMQUERY = "@CDSProblemQuery";
        private const string PARM_CDSPROBLEM_PROBLEMOPERATOR = "@ProblemOperator";
        private const string PARM_CDSLABRESULT_ID = "@CDSLabResultId";
        private const string PARM_CDSLABRESULT_LOINC = "@LOINC";
        private const string PARM_CDSLABRESULT_LOINCDescription = "@LOINCDescription";
        private const string PARM_CDSLABRESULT_Name = "@LabResultName";
        private const string PARM_CDSLABRESULT_Query = "@CDSLabResultQuery";
        private const string PARM_CDSLABRESULT_OPERATOR = "@LabResultOperator";

        private const string PARM_CDSLABRESULT_LOGICAL_OPERATOR = "@LabResultLogicalOperator";
        private const string PARM_CDSLABRESULT_VALUE = "@LabResultValue";
        private const string PARM_CDSLABRESULT_VALUE_FROM = "@LabResultValueFrom";
        private const string PARM_CDSLABRESULT_VALUE_TO = "@LabResultValueTo";

        private const string PARM_CDSMEDICATION_RXNORMID = "@MedicationRxNormId";
        private const string PARM_CDSMEDICATION_MEDICATIONCODE = "@MedicationCode";
        private const string PARM_CDSPROBLEM_PROBLEMFORQUERY = "@ProblemForQuery";
        private const string PARM_GENDER_ID = "@GenderIds";
        private const string PARM_RACE_ID = "@RaceIds";
        private const string PARM_ETHNICITY_ID = "@EthnicityIds";
        private const string PARM_PREF_LANGUAGE_ID = "@PrefLanguageIds";
        private const string PARM_AGE_OPERATOR = "@AgeOperator";
        private const string PARM_FROM_AGE = "@FromAge";

        private const string PARM_TO_AGE = "@ToAge";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_CDSALLERGY_ALLERGYFORQUERY = "@AllergyForQuery";
        private const string PARM_CDQUESTIONNAIRE_ID = "@CDSQuestionnaireId";
        private const string PARM_CDORDER_SET_IDS = "@OrderSetIds";
        private const string PARM_QUESTIONNAIRE_CONTROL_ID = "@QuestionnaireControlTypeId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_DROP_DOWN_VALUES = "@dropdownvalues";
        private const string PARM_CDS_RECURSIVELENGTH = "@RecursiveLength";
        private const string PARM_CDS_RECURSIVEPERIODID = "@RecursivePeriodId";
        private const string PARM_CDS_QUESTIONNAIRE_HTML = "@QuestionnaireHTML";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_TEST_ID = "@TestId";
        private const string PARM_ATTRIBUTE_ID = "@AttributeId";


        #region Insurance Parameters

        private const string PARM_CDS_INSURANCE_ID = "@CDSInsuranceId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_CDS_INSURANCE_QUERY = "@CDSInsuranceQuery";
        private const string PARM_INSURANCE_OPERATOR = "@InsuranceOperator";


        #endregion



        #endregion

        #region Constructors
        public DALCDS()
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

        //End 02-03-2016 Humaira Yousaf
        #region CDS
        /// <summary>
        /// Module Name: lookupCDSAgeCondition
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Age Conditions
        /// </summary>
        public DSCDSLookup lookupCDSAgeCondition()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_AgeCondition_Lookup, ds, ds.CDS_AgeCondition.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupCDSAgeCondition", PROC_CDS_AgeCondition_Lookup, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Module Name: lookupCDSReminderPeriod
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Reminder Period
        /// </summary>
        public DSCDSLookup lookupCDSReminderPeriod()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_ReminderPeriod_Lookup, ds, ds.CDS_ReminderPeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupCDSReminderPeriod", PROC_CDS_ReminderPeriod_Lookup, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Module Name: lookupCDSRuleType
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Rule Type
        /// </summary>
        public DSCDSLookup lookupCDSRuleType()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_RuleType_Lookup, ds, ds.CDS_RuleType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupCDSRuleType", PROC_CDS_RuleType_Lookup, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Module Name: lookupCDSTriggerLocation
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Trigger Location
        /// </summary>
        public DSCDSLookup lookupCDSTriggerLocation()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_TriggerLocation_Lookup, ds, ds.CDS_TriggerLocation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupCDSTriggerLocation", PROC_CDS_TriggerLocation_Lookup, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Module Name: insertUpdateCDS
        /// Author: Humaira Yousaf
        /// Created Date: 07-03-2016
        /// Description: Inserts/Updates CDS
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset contains data to insert</param>
        public DSCDS insertUpdateCDS(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDS.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                createCDSParameters(dbManager, ds, true);
                createCDSParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDS_INSERT, PROC_CDS_UPDATE, ds, ds.CDS.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), null, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDS", PROC_CDS_INSERT + " " + PROC_CDS_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Module Name: createCDSParameters
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Creates CDS Parameters
        /// </summary>
        /// <param name="dbManager" type="IDBManager">datbase manager</param>
        /// <param name="ds" type="DSCDS">dataset contains data to update</param>
        /// <param name="isInsert" type="bool">parameters type</param>
        private void createCDSParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            //Start 07-03-2016 Humaira Yousaf to merge insert and update methods
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(44);
                dbManager.AddInsertUpdateParameters(0, PARM_CDS_ID, ds.CDS.CDSIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(44);
                dbManager.AddInsertUpdateParameters(0, PARM_CDS_ID, ds.CDS.CDSIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CDS_TITLE, ds.CDS.TitleColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(2, PARM_CDS_DEVELOPER, ds.CDS.DeveloperColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CDS_FUNDINGSOURCE, ds.CDS.FundingSourceColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CDS_REFERENCEURL, ds.CDS.ReferenceURLColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_RELEASE, ds.CDS.ReleaseColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_REVISIONDATE, ds.CDS.RevisionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_TRIGGERLOCATION, ds.CDS.TriggerLocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CDS_ROLE, ds.CDS.RoleColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CDS_RULETYPE, ds.CDS.RuleTypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CDS_GENDER, ds.CDS.GenderColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CDS_ETHNICITY, ds.CDS.EthnicityColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_CDS_RACE, ds.CDS.RaceColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_CDS_LANGUAGE, ds.CDS.LanguageColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CDS_REMINDERLENGTH, ds.CDS.ReminderLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(15, PARM_CDS_REMINDERPERIODID, ds.CDS.ReminderPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(16, PARM_CDS_RECURSIVELENGTH, ds.CDS.RecursiveLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(17, PARM_CDS_PROBLEMLISTOPERATOR, ds.CDS.ProblemListOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_CDS_CDSPROBLEMLISTQUERY, ds.CDS.CDSProblemListQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_CDS_ALLERGIESOPERATOR, ds.CDS.AllergiesOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_CDS_ALLERGIESQUERY, ds.CDS.AllergiesQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_CDS_MEDICATIONSOPERATOR, ds.CDS.MedicationsOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_CDS_MEDICATIONSQUERY, ds.CDS.MedicationsQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_CDS_LABSOPERATOR, ds.CDS.LabsOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_CDS_LABSQUERY, ds.CDS.LabsQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(25, PARM_CDS_VITALSOPERATOR, ds.CDS.VitalsOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_CDS_VITALSQUERY, ds.CDS.VitalsQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(27, PARM_CDS_COMMENTS, ds.CDS.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_CDS_MODIFIED_ON, ds.CDS.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_CDS_ISACTIVE, ds.CDS.IsActiveColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(30, PARM_CDS_CREATED_BY, ds.CDS.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(31, PARM_CDS_CREATED_ON, ds.CDS.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(32, PARM_CDS_MODIFIED_BY, ds.CDS.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(33, PARM_CDS_CDSQUERY, ds.CDS.CDSQueryColumn.ColumnName, DbType.String);

            //End 07-03-2016 Humaira Yousaf to merge insert and update methods

            //Start 10-03-2016 Humaira Yousaf for CDS Age Condition
            dbManager.AddInsertUpdateParameters(34, PARM_CDS_STATUS, ds.CDS.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(35, PARM_CDS_AGECONDITION, ds.CDS.AgeConditionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(36, PARM_CDS_FROMAGE, ds.CDS.FromAgeColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(37, PARM_CDS_TOAGE, ds.CDS.ToAgeColumn.ColumnName, DbType.Int32);
            //End 10-03-2016 Humaira Yousaf for CDS Age Condition
            dbManager.AddInsertUpdateParameters(38, PARM_CDORDER_SET_IDS, ds.CDS.OrderSetIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(39, PARM_CDS_RECURSIVEPERIODID, ds.CDS.RecursivePeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(40, PARM_CDS_QUESTIONNAIRE_HTML, ds.CDS.QuestionnaireHTMLColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(41, PARM_INSURANCE_OPERATOR, ds.CDS.InsuranceOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(42, PARM_CDS_INSURANCE_QUERY, ds.CDS.InsuranceQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(43, PARM_CDS_PROVIDER_IDs, ds.CDS.ProviderIdsColumn.ColumnName, DbType.String);
        }


        private void createCDSPatientStatusParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            int i = 0;
            //Start 07-03-2016 Humaira Yousaf to merge insert and update methods
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDS_PATIENT_STATUS_ID, ds.CDSPatientStatus.CDSPatientStatusIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDS_PATIENT_STATUS_ID, ds.CDSPatientStatus.CDSPatientStatusIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_ID, ds.CDSPatientStatus.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_PATIENT_ID, ds.CDSPatientStatus.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_STATUS, ds.CDSPatientStatus.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_BY, ds.CDSPatientStatus.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CREATED_ON, ds.CDSPatientStatus.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_BY, ds.CDSPatientStatus.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_MODIFIED_ON, ds.CDSPatientStatus.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_QUESTIONNAIRE_HTML, ds.CDSPatientStatus.QuestionnaireHTMLColumn.ColumnName, DbType.String);
            //End 10-03-2016 Humaira Yousaf for CDS Age Condition
        }

        /// <summary>
        /// Module Name: loadCDSList
        /// Author: Humaira Yousaf
        /// Created Date: 03-03-2016
        /// Description: Loads CDS
        /// </summary>
        /// <param name="CDSId" type="long">CDS Id to load</param>
        /// <param name="pageNumber" type="int">pageNumber</param>
        /// <param name="rowsPerPage" type="int">rowsPerPage</param>
        public DSCDS loadCDS(long CDSId, byte IsActive, int pageNumber = 1, int rowsPerPage = 1000, string isViewCDS = "", string isPrintCDS = "", long PatientId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(6);

                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (IsActive != 1)
                    dbManager.AddParameters(1, PARM_CDS_ISACTIVE, 0);
                else
                    dbManager.AddParameters(1, PARM_CDS_ISACTIVE, IsActive);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.CDS.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (PatientId == 0)
                    dbManager.AddParameters(5, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_ID, PatientId);
                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_SELECT, ds, ds.CDS.TableName);
                if (ds.CDS.Rows.Count > 0)
                {
                    if (CDSId > 0)
                    {

                        DataTable dtTemp = ds.CDS;
                        if (dtTemp != null)
                        {
                            if (isViewCDS == "1" || isPrintCDS == "1")
                            {
                                bool isViewAction = isViewCDS == "1" ? true : false;
                                bool isPrintAcion = isPrintCDS == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), null, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
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

                MDVLogger.DALErrorLog("DALCDS::loadCDS", PROC_CDS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCDS loadCDSOrderSet(long PatientId, string CDSIds, string NoteId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (string.IsNullOrEmpty(CDSIds))
                    dbManager.AddParameters(1, PARM_CDS_IDs, null);
                else
                    dbManager.AddParameters(1, PARM_CDS_IDs, CDSIds);

                if (string.IsNullOrEmpty(NoteId))
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);

                List<string> tableNames = new List<string>();
                tableNames.Add(ds.CDSOrderSet.TableName);
                tableNames.Add(ds.CDSNoteOrderSet.TableName);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_ORDER_SET_SELECT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSOrderSet", PROC_CDS_ORDER_SET_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Module Name: loadCDSList
        /// Author: Humaira Yousaf
        /// Created Date: 03-03-2016
        /// Description: Loads CDS
        /// </summary>
        /// <param name="CDSId" type="long">CDS Id to load</param>
        /// <param name="pageNumber" type="int">pageNumber</param>
        /// <param name="rowsPerPage" type="int">rowsPerPage</param>
        public DSCDS loadCDSWithPatientStatus(long PatientId, long CDPatientStatusId = 0)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDS_PATIENT_STATUS_ID, CDPatientStatusId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_SELECT_WITH_PATIENTSTATUS, ds, ds.CDSwithPatientStatus.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSWithPatientStatus", PROC_CDS_SELECT_WITH_PATIENTSTATUS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSCDS loadCDSPatientStatus(long PatientId, int CDSPatientStatusId = 0)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (CDSPatientStatusId == 0)
                    dbManager.AddParameters(1, PARM_CDS_PATIENT_STATUS_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDS_PATIENT_STATUS_ID, CDSPatientStatusId);
                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_PATIENT_STATUS_SELECT, ds, ds.CDSPatientStatus.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALCDS::loadCDSPatientStatus", PROC_CDS_PATIENT_STATUS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DataTable loadCDSPatient(string genderIds, string RaceIds, string EthnicityIds, string PrefLanguageIds, string AgeOperator, int FromAge, int ToAge, long patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            //DSCDS ds = new DSCDS();
            DSPatient ds = new DSPatient();
            DataTable dtResult = new DataTable();
            DataSet dsResult = new DataSet();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(8);

                if (string.IsNullOrEmpty(genderIds))
                    dbManager.AddParameters(0, PARM_GENDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_GENDER_ID, genderIds);
                if (string.IsNullOrEmpty(RaceIds))
                    dbManager.AddParameters(1, PARM_RACE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_RACE_ID, RaceIds);
                if (string.IsNullOrEmpty(EthnicityIds))
                    dbManager.AddParameters(2, PARM_ETHNICITY_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_ETHNICITY_ID, EthnicityIds);
                if (string.IsNullOrEmpty(PrefLanguageIds))
                    dbManager.AddParameters(3, PARM_PREF_LANGUAGE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PREF_LANGUAGE_ID, PrefLanguageIds);

                if (string.IsNullOrEmpty(AgeOperator))
                    dbManager.AddParameters(4, PARM_AGE_OPERATOR, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_AGE_OPERATOR, AgeOperator);
                if (FromAge == 0)
                    dbManager.AddParameters(5, PARM_FROM_AGE, null);
                else
                    dbManager.AddParameters(5, PARM_FROM_AGE, FromAge);
                if (ToAge == 0)
                    dbManager.AddParameters(6, PARM_TO_AGE, null);
                else
                    dbManager.AddParameters(6, PARM_TO_AGE, ToAge);
                if (patientId == 0)
                    dbManager.AddParameters(7, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PATIENT_ID, patientId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_PATIENT_SELECT, ds, ds.Patients.TableName);

                dbManager.CommitTransaction();
                return ds.Tables[ds.Patients.TableName];
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALCDS::loadCDSPatient", PROC_CDS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCDS insertUpdateCDSPatientStatus(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDSPatientStatus.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                createCDSPatientStatusParameters(dbManager, ds, true);
                createCDSPatientStatusParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDS_PATIENT_STATUSINSERT, PROC_CDS_PATIENT_STATUSUPDATE, ds, ds.CDSPatientStatus.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSPatientStatus.Rows[0][ds.CDSPatientStatus.CDSPatientStatusIdColumn].ToString(), null, ds.CDSPatientStatus.Rows[0][ds.CDSPatientStatus.CDSIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSPatientStatus", PROC_CDS_PATIENT_STATUSINSERT + " " + PROC_CDS_PATIENT_STATUSUPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteCDSPatientStatusId(string CDSPatientStatusIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CDS_PATIENT_STATUS_IDS, CDSPatientStatusIds);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CDS_PATIENT_STATUS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::DeleteCDSPatientStatusId", PROC_CDS_PATIENT_STATUS_DELETE, ex);
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
        public string deleteCDS(string cdsId, byte isActive)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSCDS ds = loadCDS(Convert.ToInt64(cdsId), isActive, 1, 1000, "", "");
                DataTable dtTemp = ds.CDS;

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDS_ID, cdsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CDS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), null, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALCDS::deleteCDS", PROC_CDS_DELETE, ex);
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

        public DSCDSLookup lookupCDSRecursivePeriod()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_RECURSIVE_PERIOD_LOOKUP, ds, ds.CDS_RecursivePeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::lookupCDSRecursivePeriod", PROC_CDS_RECURSIVE_PERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCDSLookup lookupQuestionnaireControlType()
        {
            DSCDSLookup ds = new DSCDSLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCDSLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_QUESTIONNAIRE_CONTROL_TYPE_LOOKUP, ds, ds.QuestionnaireControlType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::lookupQuestionnaireControlType", PROC_CDS_QUESTIONNAIRE_CONTROL_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCDS loadCDSAgainstPatient_Obsolete(long CDSId, int pageNumber = 1, int rowsPerPage = 1000, long PatientId = 0)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                dbManager.AddParameters(1, PARM_CDS_ISACTIVE, null);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.CDS.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (PatientId == 0)
                    dbManager.AddParameters(5, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_ID, PatientId);
                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_SELECT_WITH_PATIENT_STATUS, ds, ds.CDSForAlerts.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSAgainstPatient", PROC_CDS_SELECT_WITH_PATIENT_STATUS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<CDSForAlerts> loadCDSAgainstPatient(long CDSId, int pageNumber = 1, int rowsPerPage = 1000, long PatientId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                if (CDSId == 0)
                    dbManager.AddParameters(PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(PARM_CDS_ID, CDSId);
                dbManager.AddParameters(PARM_CDS_ISACTIVE, null);
                if (pageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, 0, DbType.Int64, ParamDirection.Output);
                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                List<CDSForAlerts> cdsForAlertsList = dbManager.ExecuteReaderMapper<CDSForAlerts>(PROC_CDS_SELECT_WITH_PATIENT_STATUS);

                return cdsForAlertsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSAgainstPatient", PROC_CDS_SELECT_WITH_PATIENT_STATUS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCDS searchCDS(long CDSId, byte IsActive, int pageNumber = 1, int rowsPerPage = 1000, string isViewCDS = "", string isPrintCDS = "", long PatientId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(5);

                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (IsActive != 1)
                    dbManager.AddParameters(1, PARM_CDS_ISACTIVE, 0);
                else
                    dbManager.AddParameters(1, PARM_CDS_ISACTIVE, IsActive);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.CDS.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_SEARCH, ds, ds.CDS.TableName);
                if (ds.CDS.Rows.Count > 0)
                {
                    if (CDSId > 0)
                    {

                        DataTable dtTemp = ds.CDS;
                        if (dtTemp != null)
                        {
                            if (isViewCDS == "1" || isPrintCDS == "1")
                            {
                                bool isViewAction = isViewCDS == "1" ? true : false;
                                bool isPrintAcion = isPrintCDS == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), null, ds.CDS.Rows[0][ds.CDS.CDSIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
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

                MDVLogger.DALErrorLog("DALCDS::searchCDS", PROC_CDS_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region CDS Vitals

        /// <summary>
        /// Module Name: insertUpdateCDSVitals
        /// Author: Humaira Yousaf
        /// Created Date: 08-03-2016
        /// Description: Inserts/updates CDSVitals
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset containing data</param>
        public DSCDS insertUpdateCDSVitals(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDSVitals.GetChanges();
                dbManager.BeginTransaction();
                createCDSVitalsParameters(dbManager, ds, true);
                createCDSVitalsParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDSVITALS_INSERT, PROC_CDSVITALS_UPDATE, ds, ds.CDSVitals.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSVitals.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.CDSVitals.Rows[i][ds.CDSVitals.CDSVitalsIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSVitals.Rows[0][ds.CDSVitals.CDSVitalsIdColumn].ToString(), null, ds.CDSVitals.Rows[0][ds.CDSVitals.CDSIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSVitals", PROC_CDSVITALS_INSERT + " " + PROC_CDSVITALS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Module Name: createCDSVitalsParameters
        /// Author: Humaira Yousaf
        /// Created Date: 08-03-2016
        /// Description: Creates CDSVitals Parameters
        /// </summary>
        /// <param name="dbManager" type="IDBManager">datbase manager</param>
        /// <param name="ds" type="DSCDS">dataset contains data to update</param>
        /// <param name="isInsert" type="bool">parameters type</param>
        private void createCDSVitalsParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(16);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_ID, ds.CDSVitals.CDSVitalsIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(16);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_ID, ds.CDSVitals.CDSVitalsIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_ID, ds.CDSVitals.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_LOGIC, ds.CDSVitals.VitalsLogicColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_TYPE, ds.CDSVitals.VitalTypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_LOGICAL_OPERATOR, ds.CDSVitals.VitalLogicalOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_VALUE, ds.CDSVitals.VitalValueColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_VALUE_FROM, ds.CDSVitals.VitalValueFromColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_VALUE_TO, ds.CDSVitals.VitalValueToColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_UNIT, ds.CDSVitals.VitalUnitColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDSVITALS_VITALSQUERY, ds.CDSVitals.VitalsQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_COMMENTS, ds.CDSVitals.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_MODIFIED_ON, ds.CDSVitals.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_ISACTIVE, ds.CDSVitals.IsActiveColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_CREATED_BY, ds.CDSVitals.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_CREATED_ON, ds.CDSVitals.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_MODIFIED_BY, ds.CDSVitals.ModifiedByColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Module Name: loadCDSVitals
        /// Author: Humaira Yousaf
        /// Created Date: 08-03-2016
        /// Description: Loads CDSVitals
        /// </summary>
        /// <param name="CDSId" type="long">CDSId</param>
        /// <param name="CDSVitalsId" type="long">CDSVitalsId</param>
        public DSCDS loadCDSVitals(long CDSId, long CDSVitalsId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (CDSVitalsId == 0)
                    dbManager.AddParameters(1, PARM_CDSVITALS_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDSVITALS_ID, CDSVitalsId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDSVITALS_SELECT, ds, ds.CDSVitals.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSVitals", PROC_CDSVITALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string deleteCDSVital(long vitalId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDSVITALS_ID, vitalId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDSVITALS_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSProblemList", PROC_CDSPROBLEM_DELETE, ex);
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

        #region CDS Medication
        /// <summary>
        /// Module Name: insertUpdateCDSMedication
        /// Author: Humaira Yousaf
        /// Created Date: 09-03-2016
        /// Description: Inserts/updates CDS Medication
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset containing data</param>
        public DSCDS insertUpdateCDSMedication(DSCDS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.CDSMedication.GetChanges();
                dbManager.BeginTransaction();

                createCDSMedicationParameters(dbManager, ds, true);
                createCDSMedicationParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDSMEDICATION_INSERT, PROC_CDSMEDICATION_UPDATE, ds, ds.CDSMedication.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSMedication.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.CDSMedication.Rows[i][ds.CDSMedication.CDSMedicationIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSMedication.Rows[0][ds.CDSMedication.CDSMedicationIdColumn].ToString(), null, ds.CDSMedication.Rows[0][ds.CDSMedication.CDSIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSMedication", PROC_CDSMEDICATION_INSERT + " " + PROC_CDSMEDICATION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Module Name: createCDSMedicationParameters
        /// Author: Humaira Yousaf
        /// Created Date: 09-03-2016
        /// Description: Creates CDS Medication Parameters
        /// </summary>
        /// <param name="dbManager" type="IDBManager">datbase manager</param>
        /// <param name="ds" type="DSCDS">dataset contains data to update</param>
        /// <param name="isInsert" type="bool">parameters type</param>
        private void createCDSMedicationParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(13);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSMEDICATION_ID, ds.CDSMedication.CDSMedicationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(13);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSMEDICATION_ID, ds.CDSMedication.CDSMedicationIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CDS_ID, ds.CDSMedication.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CDSMEDICATION_DRUGID, ds.CDSMedication.DrugIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_CDS_COMMENTS, ds.CDSMedication.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CDS_MODIFIED_ON, ds.CDSMedication.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_ISACTIVE, ds.CDSMedication.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_CREATED_BY, ds.CDSMedication.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_CREATED_ON, ds.CDSMedication.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CDS_MODIFIED_BY, ds.CDSMedication.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CDSMEDICATION_MEDICATIONQUERY, ds.CDSMedication.CDSMedicationQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CDSMEDICATION_MEDICATIONOPERATOR, ds.CDSMedication.MedicationOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CDSMEDICATION_MEDICATIONCODE, ds.CDSMedication.MedicationCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_CDSMEDICATION_RXNORMID, ds.CDSMedication.MedicationRxNormIdColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Module Name: loadCDSMedication
        /// Author: Humaira Yousaf
        /// Created Date: 09-03-2016
        /// Description: Loads CDS Medication
        /// </summary>
        /// <param name="CDSId" type="long">CDSId</param>
        /// <param name="medicationId" type="long">medicationId</param>
        public DSCDS loadCDSMedication(long CDSId, long medicationId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (medicationId == 0)
                    dbManager.AddParameters(1, PARM_CDSMEDICATION_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDSMEDICATION_ID, medicationId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDSMEDICATION_SELECT, ds, ds.CDSMedication.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSMedication", PROC_CDSMEDICATION_SELECT, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method Name: deleteCDSMedication
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: deleting CDS Medication
        /// </summary>
        /// <param name="medicationId" type="string">medicationId to be deleted</param>
        public string deleteCDSMedication(string medicationId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDSMEDICATION_ID, medicationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDSMEDICATION_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSMedication", PROC_CDSMEDICATION_DELETE, ex);
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


        #region CDS Insurance

        public DSCDS insertUpdateCDSInsurance(DSCDS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.CDSInsurance.GetChanges();
                dbManager.BeginTransaction();

                createCDSInsuranceParameters(dbManager, ds, true);
                createCDSInsuranceParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDS_INSURANCE_INSERT, PROC_CDS_INSURANCE_UPDATE, ds, ds.CDSInsurance.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSInsurance.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.CDSInsurance.Rows[i][ds.CDSInsurance.CDSInsuranceIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSInsurance.Rows[0][ds.CDSInsurance.CDSInsuranceIdColumn].ToString(), null, ds.CDSInsurance.Rows[0][ds.CDSInsurance.CDSInsuranceIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSInsurance", PROC_CDS_INSURANCE_INSERT + " " + PROC_CDS_INSURANCE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void createCDSInsuranceParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDS_INSURANCE_ID, ds.CDSInsurance.CDSInsuranceIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(i++, PARM_CDS_INSURANCE_ID, ds.CDSInsurance.CDSInsuranceIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_ID, ds.CDSInsurance.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_INSURANCE_PLAN_ID, ds.CDSInsurance.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_COMMENTS, ds.CDSInsurance.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_MODIFIED_ON, ds.CDSInsurance.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_ISACTIVE, ds.CDSInsurance.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_CREATED_BY, ds.CDSInsurance.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_CREATED_ON, ds.CDSInsurance.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_MODIFIED_BY, ds.CDSInsurance.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_CDS_INSURANCE_QUERY, ds.CDSInsurance.CDSInsuranceQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(i++, PARM_INSURANCE_OPERATOR, ds.CDSInsurance.InsuranceOperatorColumn.ColumnName, DbType.String);
        }


        public DSCDS loadCDSInsurance(long CDSId, long insuranceId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (insuranceId == 0)
                    dbManager.AddParameters(1, PARM_CDS_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDS_INSURANCE_ID, insuranceId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDS_INSURANCE_SELECT, ds, ds.CDSInsurance.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSInsurance", PROC_CDS_INSURANCE_SELECT, ex);
                throw ex;
            }
        }


        public string deleteCDSInsurance(string insuranceId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDS_INSURANCE_ID, insuranceId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDS_INSURANCE_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSInsurance", PROC_CDS_INSURANCE_DELETE, ex);
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

        #region CDS Allergy

        /// <summary>
        /// Method Name: insertUpdateCDSAllergy
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: Inserts/updates CDS Allergy
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset containing data</param>
        public DSCDS insertUpdateCDSAllergy(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDSAllergy.GetChanges();
                dbManager.BeginTransaction();

                createCDSAllergyParameters(dbManager, ds, true);
                createCDSAllergyParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDSALLERGY_INSERT, PROC_CDSALLERGY_UPDATE, ds, ds.CDSAllergy.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSAllergy.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.CDSAllergy.Rows[i][ds.CDSAllergy.CDSAllergyIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSAllergy.Rows[0][ds.CDSAllergy.CDSAllergyIdColumn].ToString(), null, ds.CDSAllergy.Rows[0][ds.CDSAllergy.CDSIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSAllergy", PROC_CDSALLERGY_INSERT + " " + PROC_CDSALLERGY_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: insertUpdateCDSLabResult
        /// Author: Ahmad Raza
        /// Created Date: 16-05-2016
        /// Description: Inserts/updates CDS LabResult
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset containing data</param>
        public DSCDS insertUpdateCDSLabResult(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDSLabResult.GetChanges();
                dbManager.BeginTransaction();

                createCDSLabResultParameters(dbManager, ds, true);
                createCDSLabResultParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDSLABRESULT_INSERT, PROC_CDSLABRESULT_UPDATE, ds, ds.CDSLabResult.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSLabResult.Rows.Count; i++)
                    {
                        //  dtTemp.Rows[i]["PrimaryKey"] = ds.CDSLabResult.Rows[i][ds.CDSLabResult.CDSLabResultIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSLabResult.Rows[0][ds.CDSLabResult.CDSLabResultIdColumn].ToString(), null, ds.CDSLabResult.Rows[0][ds.CDSLabResult.CDSIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSLabResult", PROC_CDSLABRESULT_INSERT + " " + PROC_CDSLABRESULT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: createCDSAllergyParameters
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: creates parameters for CDS Allergy
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createCDSAllergyParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSALLERGY_ID, ds.CDSAllergy.CDSAllergyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSALLERGY_ID, ds.CDSAllergy.CDSAllergyIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CDS_ID, ds.CDSAllergy.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CDSALLERGY_ALLERGEN, ds.CDSAllergy.AllergenColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CDS_COMMENTS, ds.CDSAllergy.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CDS_MODIFIED_ON, ds.CDSAllergy.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_ISACTIVE, ds.CDSAllergy.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_CREATED_BY, ds.CDSAllergy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_CREATED_ON, ds.CDSAllergy.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CDS_MODIFIED_BY, ds.CDSAllergy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CDSALLERGY_ALLERGYQUERY, ds.CDSAllergy.CDSAllergyQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CDSALLERGY_ALLERGYOPERATOR, ds.CDSAllergy.AllergyOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CDSALLERGY_ALLERGYFORQUERY, ds.CDSAllergy.AllergyForQueryColumn.ColumnName, DbType.String);
        }


        /// <summary>
        /// Method Name: createCDSLabResultParameters
        /// Author: Ahmad Raza
        /// Created Date: 16-05-2016
        /// Description: creates parameters for CDS LabResult
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createCDSLabResultParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            int i = 0;
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSLABRESULT_ID, ds.CDSLabResult.CDSLabResultIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSLABRESULT_ID, ds.CDSLabResult.CDSLabResultIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CDS_ID, ds.CDSLabResult.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CDSLABRESULT_LOINC, ds.CDSLabResult.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CDSLABRESULT_LOINCDescription, ds.CDSLabResult.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CDSLABRESULT_Name, ds.CDSLabResult.LabResultNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_ISACTIVE, ds.CDSLabResult.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_CREATED_BY, ds.CDSLabResult.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_CREATED_ON, ds.CDSLabResult.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CDS_MODIFIED_BY, ds.CDSLabResult.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CDS_MODIFIED_ON, ds.CDSLabResult.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_CDSLABRESULT_Query, ds.CDSLabResult.CDSLabResultQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CDSLABRESULT_OPERATOR, ds.CDSLabResult.LabResultOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_CDSLABRESULT_LOGICAL_OPERATOR, ds.CDSLabResult.LabResultLogicalOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_CDSLABRESULT_VALUE, ds.CDSLabResult.LabResultValueColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CDSLABRESULT_VALUE_FROM, ds.CDSLabResult.LabResultValueFromColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CDSLABRESULT_VALUE_TO, ds.CDSLabResult.LabResultValueToColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_LAB_ID, ds.CDSLabResult.LabIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_TEST_ID, ds.CDSLabResult.TestIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_ATTRIBUTE_ID, ds.CDSLabResult.AttributeIdColumn.ColumnName, DbType.String);

        }
        private void createCDSQuestionnaireParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_CDQUESTIONNAIRE_ID, ds.CDSQuestionnaire.CDSQuestionnaireIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_CDQUESTIONNAIRE_ID, ds.CDSQuestionnaire.CDSQuestionnaireIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_CDS_ID, ds.CDSQuestionnaire.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_DESCRIPTION, ds.CDSQuestionnaire.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_QUESTIONNAIRE_CONTROL_ID, ds.CDSQuestionnaire.QuestionnaireControlTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_CDS_CREATED_BY, ds.CDSLabResult.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_CREATED_ON, ds.CDSLabResult.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_MODIFIED_BY, ds.CDSLabResult.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_MODIFIED_ON, ds.CDSLabResult.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_DROP_DOWN_VALUES, ds.CDSQuestionnaire.dropDownValuesColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Method Name: loadCDSAllergy
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: loads CDS Allergy
        /// </summary>
        /// <param name="CDSId"></param>
        /// <param name="allergyId"></param>
        /// <returns></returns>
        public DSCDS loadCDSAllergy(long CDSId, long allergyId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (allergyId == 0)
                    dbManager.AddParameters(1, PARM_CDSALLERGY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDSALLERGY_ID, allergyId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDSALLERGY_SELECT, ds, ds.CDSAllergy.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSAllergy", PROC_CDSALLERGY_SELECT, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method Name: loadCDSLabResult
        /// Author: Ahmad Raza
        /// Created Date: 16-05-2016
        /// Description: loads CDS LabResult
        /// </summary>
        /// <param name="CDSId"></param>
        /// <param name="labResultId"></param>
        /// <returns></returns>
        public DSCDS loadCDSLabResult(long CDSId, long labResultId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (labResultId == 0)
                    dbManager.AddParameters(1, PARM_CDSLABRESULT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDSLABRESULT_ID, labResultId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDSLABRESULT_SELECT, ds, ds.CDSLabResult.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSLabResult", PROC_CDSLABRESULT_SELECT, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method Name: loadCDSProblemList
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: loads CDS Problem
        /// </summary>
        /// <param name="CDSId"></param>
        /// <param name="problemListId"></param>
        /// <returns></returns>
        public DSCDS loadCDSProblemList(long CDSId, long problemListId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (problemListId == 0)
                    dbManager.AddParameters(1, PARM_CDSPROBLEM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDSPROBLEM_ID, problemListId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDSPROBLEM_SELECT, ds, ds.CDSProblem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSProblemList", PROC_CDSPROBLEM_SELECT, ex);
                throw ex;
            }
        }
        public DSCDS loadCDSQuestionnaire(long CDSId, long QuestionnaireId)
        {
            DSCDS ds = new DSCDS();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (CDSId == 0)
                    dbManager.AddParameters(0, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CDS_ID, CDSId);
                if (QuestionnaireId == 0)
                    dbManager.AddParameters(1, PARM_CDQUESTIONNAIRE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CDQUESTIONNAIRE_ID, QuestionnaireId);

                ds = (DSCDS)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CDQUESTIONNAIRE_SELECT, ds, ds.CDSQuestionnaire.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::loadCDSQuestionnaire", PROC_CDQUESTIONNAIRE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCDS insertUpdateCDSQuestionnaire(DSCDS ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createCDSQuestionnaireParameters(dbManager, ds, true);
                createCDSQuestionnaireParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDQUESTIONNAIRE_INSERT, PROC_CDQUESTIONNAIRE_UPDATE, ds, ds.CDSQuestionnaire.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSQuestionnaire", PROC_CDQUESTIONNAIRE_INSERT + " " + PROC_CDQUESTIONNAIRE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region CDS Problem

        /// <summary>
        /// Method Name: insertUpdateCDSProblem
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: Inserts/updates CDS Problem
        /// </summary>
        /// <param name="ds" type="DSCDS">dataset containing data</param>
        public DSCDS insertUpdateCDSProblem(DSCDS ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CDSProblem.GetChanges();
                dbManager.BeginTransaction();

                createCDSProblemParameters(dbManager, ds, true);
                createCDSProblemParameters(dbManager, ds, false);
                ds = (DSCDS)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CDSPROBLEM_INSERT, PROC_CDSPROBLEM_UPDATE, ds, ds.CDSProblem.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.CDSProblem.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.CDSProblem.Rows[i][ds.CDSProblem.CDSProblemIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.CDSProblem.Rows[0][ds.CDSProblem.CDSProblemIdColumn].ToString(), null, ds.CDSProblem.Rows[0][ds.CDSProblem.CDSIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::insertUpdateCDSProblem", PROC_CDSPROBLEM_INSERT + " " + PROC_CDSPROBLEM_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Method Name: createCDSProblemParameters
        /// Author: Ahmad Raza
        /// Created Date: 14-03-2016
        /// Description: creates parameters for CDS Problem
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createCDSProblemParameters(IDBManager dbManager, DSCDS ds, bool isInsert)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSPROBLEM_ID, ds.CDSProblem.CDSProblemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_CDSPROBLEM_ID, ds.CDSProblem.CDSProblemIdColumn.ColumnName, DbType.Int64);

            }

            //change problem list parameters
            //dbManager.AddInsertUpdateParameters(1, PARM_CDSPROBLEM_ID, ds.CDSProblem.CDSProblemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_CDS_ID, ds.CDSProblem.CDSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CDSPROBLEM_PROBLEM, ds.CDSProblem.ProblemColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CDS_COMMENTS, ds.CDSProblem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CDS_MODIFIED_ON, ds.CDSProblem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_CDS_ISACTIVE, ds.CDSProblem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CDS_CREATED_BY, ds.CDSProblem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CDS_CREATED_ON, ds.CDSProblem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_CDS_MODIFIED_BY, ds.CDSProblem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CDSPROBLEM_PROBLEMQUERY, ds.CDSProblem.CDSProblemQueryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_CDSPROBLEM_PROBLEMOPERATOR, ds.CDSProblem.ProblemOperatorColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CDSPROBLEM_PROBLEMFORQUERY, ds.CDSProblem.ProblemForQueryColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Method Name: deleteCDSProblemList
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: deleting CDS Problem List
        /// </summary>
        /// <param name="problemListId" type="long">problemListId to be deleted</param>
        public string deleteCDSProblemList(long problemListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CDSPROBLEM_ID, problemListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDSPROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSProblemList", PROC_CDSPROBLEM_DELETE, ex);
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

        public string deleteCDSQuestionnaire(long questionnaireId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //dbManager.CreateParameters(2);
                dbManager.AddParameters(PARM_CDQUESTIONNAIRE_ID, questionnaireId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, DBNull.Value, DbType.String, ParamDirection.Output, 255);
                dbManager.ExecuteScalar(PROC_CDQUESTIONNAIRE_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSQuestionnaire", PROC_CDQUESTIONNAIRE_DELETE, ex);
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
        /// Method Name: deleteCDSAllergy
        /// Author: Ahmad Raza
        /// Created Date: 16-03-2016
        /// Description: deleting CDS Allergy
        /// </summary>
        /// <param name="allergyId" type="string">allergyId to be deleted</param>
        public string deleteCDSAllergy(string allergyId, string cdsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_CDSALLERGY_ID, allergyId);
                dbManager.AddParameters(1, PARM_CDS_ID, cdsId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDSALLERGY_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSAllergy", PROC_CDSALLERGY_DELETE, ex);
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
        /// Method Name: deleteCDSLabResult
        /// Author: Ahmad Raza
        /// Created Date: 16-05-2016
        /// Description: deleting CDS LabResult
        /// </summary>
        /// <param name="labResultId" type="string">labResultId to be deleted</param>
        public string deleteCDSLabResult(string cdsId, string testId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int i = 0;
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(i++, PARM_CDS_ID, cdsId);
                dbManager.AddParameters(i++, PARM_TEST_ID, testId);
                dbManager.AddParameters(i++, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CDSLABRESULT_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteCDSLabResult", PROC_CDSLABRESULT_DELETE, ex);
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
