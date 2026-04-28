using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Clinical.Medical.ProblemLists;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDVision.Common.Utilities;


namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALProblemLists
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_DIAGNOSIS_CONFIRMATION_LOOKUP = "[Clinical].[TNMSystemCodes]";
        private const string PROC_LOAD_CANCER_CODES = "Clinical.sp_CancerCodesSelect";
        private const string PROC_LOAD_CANCER_DISEASE_DETAILS = "Clinical.sp_ProblemDetailsSelect";
        private const string PROC_PREVIOUS_PROBLEMLISTS = "Clinical.sp_PreviousVisitProblemListSelect";
        private const string PROC_PROBLEMLIST_INSERT = "Clinical.sp_ProblemListInsert";
        private const string PROC_PROBLEMDETAIL_INSERT_UPDATE = "Clinical.sp_ProblemDetailsInsertUpdate";
        private const string PROC_PROBLEMLIST_INSERT_Unique = "Clinical.sp_ProblemListInsertUnique";
        private const string PROC_PROBLEMLIST_UPDATE = "Clinical.sp_ProblemListUpdate";

        private const string PROC_ALL_PROBLEMLIST_SELECT = "Clinical.sp_ProblemListAllSelect";


        private const string PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE = "Clinical.sp_CProb_ProblemListUpdate_ForInActive";
        private const string PROC_PROBLEMLIST_UPDATE_FOR_GRID = "Clinical.sp_CProb_ProblemListUpdate_ForGrid";


        private const string PROC_PROBLEMLIST_DELETE = "Clinical.sp_ProblemListDelete";
        private const string PROC_PROBLEMLIST_SELECT = "Clinical.sp_ProblemListSelect";
        private const string PROC_PROBLEMLIST_LAB_BASED_SELECT = "[Clinical].[sp_NotesLabOrderICDSelect]";
        private const string PROC_PROBLEMS_SELECT = "Clinical.sp_ProblemsByNoteAndPatientId";

        private const string PROC_PROBLEMLIST_SELECT_OP = "Clinical.sp_CProb_ProblemListSelect";

        private const string PROC_PROBLEMLIST_SELECT_FOR_INACTIVE = "Clinical.sp_CProb_ProblemListSelect_For_Inactive";

        private const string PROC_PROBLEMLIST_SELECT_FOR_FILL = "Clinical.sp_CProb_ProblemListSelect_Fill";


        private const string PROC_PROBLEM_LIST_LOOKUP = "Clinical.sp_ProblemListLookUp";


        private const string PROC_PROBLEMLIST_HISTORY_SELECT = "Clinical.sp_ProblemListHistorySelect";
        private const string PROC_PROBLEM_LIST_SELECT_FORSOAPTEXT = "Clinical.sp_ProblemListsSelectForSoapText";

        private const string PROC_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP = "Clinical.sp_CProb_ProblemListsSelectForSoapText";
        private const string PROC_PREVIOUS_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP = "Clinical.sp_CProb_PreviousProblemListsSelectForSoapText";
        private const string PROC_GET_LATEST_PROBLEMLIST_BY_PATIENTI = "Clinical.sp_GetLatestProblemListByPatientId";

        private const string PROC_GET_LATEST_PROBLEMLIST_FORSOAPTEXT = "Clinical.sp_GetLatestProblemListForSoapText";

        private const string PROC_GET_LATEST_CHRONICPROBLEMLIST_BY_PATIENTI = "Clinical.sp_GetLatestChronicProblemListByPatientId";
        private const string PROC_DETACH_PROBLEM_LIST_FROM_NOTES = "Clinical.sp_DetachProblemListFromNotes";
        private const string PROC_ATTACH_PROBLEM_LIST_FROM_NOTES = "Clinical.sp_AttachProblemListWithNotes";

        // Start 27/11/2015 Muhammad Irfan Created to use dropdown values from DB end
        private const string PROC_CHRONICITY_LEVEL_LOOKUP = "Clinical.sp_ChronicityLevelLookup";
        private const string PROC_SEVERITY_TYPE_LOOKUP = "Clinical.sp_SeverityTypeLookup";

        private const string PROC_NOTE_STATUS_LOOKUP = "Clinical.sp_NoteStatusLookup";
        private const string PROC_NOTE_ACTION_LOOKUP = "Clinical.sp_NoteActionLookup";
        //Start 7/1/2016 Muhammad Wasim Abbas created to add Problem RcopiaID
        private const string PROC_PROBLEM_RCOPIAID = "Clinical.InsertProblemsRcopialID";
        private const string PROC_PROBLEMLIST_EXISTS = "Clinical.sp_CProb_ProblemListExists";

        private const string PROC_CLINICAL_CHRONICITY_LOOKUP = "Clinical.sp_ProblemChronicityLookup";
        private const string PROC_CLINICAL_SEVERITY_LOOKUP = "Clinical.sp_ProblemSeverityLookup";
        private const string PROC_PROBLEMLIST_UPDATE_ORDER = "Clinical.sp_ProblemListUpdateOrder";

        private const string PROC_NOTES_PROBLEMHX_SELECT = "[Clinical].[sp_NotesProblemHxSelect]";
        private const string PROC_ATTACH_LO_PROBLEMS_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachLabOrdProblemsWithNoteForSoap";
        private const string PROC_ATTACH_RO_PROBS_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachRadiologyOrdProblemsWithNoteForSoap";
        private const string PROC_ATTACH_CO_PROBS_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachConsultationOrdProblemsWithNoteForSoap";
        private const string PROC_ATTACH_PO_PROBS_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachProcedureOrdProblemsWithNoteForSoap";
        private const string PROC_ATTACH_REF_PROBS_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachReferralProblemsWithNoteForSoap";
        private const string PROC_ATTACH_PROBLEM_WITH_NOTE_LOAD_FORSOAP = "Clinical.sp_AttachProblemsWithNoteAndGetSoap";
        private const string PROC_TREATMENT_PROBLEMLIST = "Clinical.sp_TreatmentProblemsForESuperBill";
        private const string PROC_TREATMENT_PROBLEMLIST_LAB_BASED_SELECT = "Clinical.sp_TreatmentLabOrderProblemsForESuperBill";
        private const string PROC_GET_LATEST_PROBLEMLIST_BYPROVIDER = "Clinical.sp_PreviousNoteProblemSelect";

        #endregion

        #region "Parameters"


        private const string PARM_Problem_Detail_XML = "@ProblemDetailXML";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        private const string PARM_PROBLEM_NAME = "@ProblemName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_CHRONICITY_LEVEL = "@ChronicityLevel";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PROBLEM_EXISTS = "@Exists";
        private const string PARM_INACTIVE_VALUE = "@InActiveChkBoxValue";
        private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_IS_HISTORY = "@IsHistory";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT_ACTIVE_OR_INACTIVE = "@RecordCountActiveInactive";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        //M.Wasim Added new parameter @RcopiaID for [Clinical].[InsertProblemsRcopialID] stored procedure on 1/7/2016.
        private const string PARM_RCOPIAID = "@RcopiaID";

        //------------------------------
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_ISCHIEF_COMPLAINT = "@IsChiefComplaint";
        private const string PARM_PROBLEM_ORDER = "@ProblemOrder";
        //------------------------------
        private const string PARM_CUSTOMFORM_ID = "@CustomFormId";
        private const string PARM_FROMSUPERBILL = "@IsForSuperBill";
        private const string PARM_CF_PROBLEMS = "@IsShowCustomFormProblems";
        private const string PARM_CHECK_PROBLEM_EXISTS = "@CheckProblemExists";

        private const string PARM_PROB_COMPLAINT_ID = "@ProbComplaintId";
        private const string PARM_PROB_COMPLAINT_DETAIL_ID = "@ComplaintDetailId";
        private const string PARM_LABORDER_IDS = "@LabOrderIds";

        private const string PROC_CHRONICITY_SEVERITY_LOOKUP = "Clinical.sp_ChronicityAndSeverityLookup";

        private const string PARM_RADIOLOGYORDER_IDS = "@RadiologyOrderIds";
        private const string PARM_CONSULTATIONORDER_IDS = "@ConsultationOrderIds";

        private const string PARM_PROCEDUREORDER_IDS = "@ProcedureOrderIds";
        private const string PARM_REFERRAL_IDS = "@ReferralIds";



        private const string PARM_PROBLEM_DETAIL_ID = "@ProblemDetailId";
        private const string PARM_DIAGNOSIS = "@Diagnosis";
        private const string PARM_DIAGNOSIS_CONFIRMATION = "@DiagnosisConfirmation";
        private const string PARM_DIAGNOSIS_DATE = "@DiagnosisDate";
        private const string PARM_PRIMARY_SITE_ID = "@PrimarySiteId";
        private const string PARM_PRIMARY_SITE = "@PrimarySite";
        private const string PARM_LATERALITY = "@Laterality";
        private const string PARM_HISTOLOGIC_TYPE = "@HistologicType";
        private const string PARM_HISTOLOGIC_TYPE_ID = "@HistologicTypeId";
        private const string PARM_BEHAVIOR = "@Behavior";
        private const string PARM_GRADE = "@Grade";
        private const string PARM_NKO_CLINICAL = "@NKOClinical";
        private const string PARM_CLINICAL_DIAGNOSIS_DATE = "@ClinicalDiagnosisDate";
        private const string PARM_CLINICAL_STAGE_GROUP = "@ClinicalStageGroup";
        private const string PARM_CLINICAL_STAGE_DESCRIPTOR = "@ClinicalStageDescriptor";
        private const string PARM_PRIMARY_CLINICAL_TUMOR = "@PrimaryClinicalTumor";
        private const string PARM_RLNC = "@RLNC";
        private const string PARM_DISTANCE_MESTASTATASES = "@DistanceMestastatases";
        private const string PARM_STAGERCLINICALCANCER = "@StagerClinicalCancer";
        private const string PARM_NKOPATHOLOGIC = "@NKOPathologic";
        private const string PARM_EFFECTIVEDATE = "@EffectiveDate";
        private const string PARM_PATHOLOGICSTAGEGROUP = "@PathologicStageGroup";
        private const string PARM_PATHOLOGICSTAGEDESCRIPTOR = "@PathologicStageDescriptor";
        private const string PARM_PRIMARYTUMORPATHOLOGIC = "@PrimaryTumorPathologic";
        private const string PARM_RLNP = "@RLNP";
        private const string PARM_DISTANCEMESTASTATASESPATHOLOGIC = "@DistanceMestastatasesPathologic";
        private const string PARM_STAGERPATHOLOGICCANCER = "@StagerPathologicCancer";
        private const string PARM_ORDERSET_ID = "@OrderSetId";
        private const string PARM_NON_DIABETIC = "@IsNonDiabetic";
        private const string PARM_IS_DIABETIC_SCREENING = "@IsDiabeticScreening";
        #endregion

        #region Constructors
        public DALProblemLists()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALProblemLists(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(32);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.ProblemList.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_NOTE_ID, ds.ProblemList.NoteIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.ProblemList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.ProblemList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);



            dbManager.AddParameters(15, PARM_INACTIVE_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_SNOMEDID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_SNOMED_DESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_ISCHIEF_COMPLAINT, ds.ProblemList.IsChiefComplaintColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_PROBLEM_ORDER, ds.ProblemList.ProblemOrderColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CUSTOMFORM_ID, ds.ProblemList.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_CHECK_PROBLEM_EXISTS, ds.ProblemList.CheckProblemExistsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARM_PROB_COMPLAINT_ID, ds.ProblemList.ComplaintIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_PROB_COMPLAINT_DETAIL_ID, ds.ProblemList.ComplaintDetailIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ERROR_MESSAGE, ds.ProblemList.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(30, PARM_NON_DIABETIC, ds.ProblemList.IsNonDiabeticColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(31, PARM_IS_DIABETIC_SCREENING, ds.ProblemList.IsNonDiabeticColumn.ColumnName, DbType.Boolean);
        }

        private void CreateDetailInsertParameters(IDBManager dbManager, DSProblemLists ds)
        {
            dbManager.CreateInsertParameters(33);


            dbManager.AddInsertUpdateParameters(27, PARM_CREATED_BY, ds.ProblemDetails.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_CREATED_ON, ds.ProblemDetails.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_MODIFIED_BY, ds.ProblemDetails.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(30, PARM_MODIFIED_ON, ds.ProblemDetails.ModifiedOnColumn.ColumnName, DbType.DateTime);


        }

        private void CreateDetailUpdateParameters(IDBManager dbManager, DSProblemLists ds)
        {
            dbManager.CreateUpdateParameters(33);

            dbManager.AddInsertUpdateParameters(0, PARM_PROBLEM_DETAIL_ID, ds.ProblemDetails.ProblemDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_PROBLEMLIST_ID, ds.ProblemDetails.ProblemListsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_DIAGNOSIS, ds.ProblemDetails.DiagnosisColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_DIAGNOSIS_DATE, ds.ProblemDetails.DiagnosisDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_DIAGNOSIS_CONFIRMATION, ds.ProblemDetails.DiagnosisConfirmationColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_PRIMARY_SITE_ID, ds.ProblemDetails.PrimarySiteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_LATERALITY, ds.ProblemDetails.LateralityColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(7, PARM_HISTOLOGIC_TYPE_ID, ds.ProblemDetails.HistologicTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(8, PARM_BEHAVIOR, ds.ProblemDetails.BehaviorColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_GRADE, ds.ProblemDetails.GradeColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_NKO_CLINICAL, ds.ProblemDetails.NKOClinicalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(11, PARM_CLINICAL_DIAGNOSIS_DATE, ds.ProblemDetails.ClinicalDiagnosisDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_CLINICAL_STAGE_GROUP, ds.ProblemDetails.ClinicalStageGroupColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_CLINICAL_STAGE_DESCRIPTOR, ds.ProblemDetails.ClinicalStageDescriptorColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(14, PARM_PRIMARY_CLINICAL_TUMOR, ds.ProblemDetails.PrimaryClinicalTumorColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(15, PARM_RLNC, ds.ProblemDetails.RLNCColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(16, PARM_DISTANCE_MESTASTATASES, ds.ProblemDetails.DistanceMestastatasesColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(17, PARM_STAGERCLINICALCANCER, ds.ProblemDetails.StagerClinicalCancerColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(18, PARM_NKOPATHOLOGIC, ds.ProblemDetails.NKOPathologicColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(19, PARM_EFFECTIVEDATE, ds.ProblemDetails.EffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(20, PARM_PATHOLOGICSTAGEGROUP, ds.ProblemDetails.PathologicStageGroupColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(21, PARM_PATHOLOGICSTAGEDESCRIPTOR, ds.ProblemDetails.PathologicStageDescriptorColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_PRIMARYTUMORPATHOLOGIC, ds.ProblemDetails.PrimaryTumorPathologicColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(23, PARM_RLNP, ds.ProblemDetails.RLNPColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(24, PARM_DISTANCEMESTASTATASESPATHOLOGIC, ds.ProblemDetails.DistanceMestastatasesPathologicColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(25, PARM_STAGERPATHOLOGICCANCER, ds.ProblemDetails.StagerPathologicCancerColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(26, PARM_IS_ACTIVE, ds.ProblemDetails.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(27, PARM_CREATED_BY, ds.ProblemDetails.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_CREATED_ON, ds.ProblemDetails.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_MODIFIED_BY, ds.ProblemDetails.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(30, PARM_MODIFIED_ON, ds.ProblemDetails.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(31, PARM_PRIMARY_SITE, ds.ProblemDetails.PrimarySiteColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(32, PARM_HISTOLOGIC_TYPE, ds.ProblemDetails.HistologicTypeColumn.ColumnName, DbType.String);

        }

        private void CreateParametersForInactive(IDBManager dbManager, DSProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARM_INACTIVE_VALUE, ds.ProblemList.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_INACTIVE_REASON, ds.ProblemList.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_NOTE_ID, ds.ProblemList.NoteIdColumn.ColumnName, DbType.Int64);
        }



        private void CreateParametersForUpdateRow(IDBManager dbManager, DSProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);
            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROBLEM_NAME, ds.ProblemList.ProblemNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ProblemList.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CHRONICITY_LEVEL, ds.ProblemList.ChronicityLevelColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.ProblemList.SeverityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_START_DATE, ds.ProblemList.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_END_DATE, ds.ProblemList.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.ProblemList.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PATIENT_ID, ds.ProblemList.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.ProblemList.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_ICD9, ds.ProblemList.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ICD10, ds.ProblemList.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ICD10_DESCRIPTION, ds.ProblemList.ICD10_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ICD9_DESCRIPTION, ds.ProblemList.ICD9_DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SNOMEDID, ds.ProblemList.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_SNOMED_DESCRIPTION, ds.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_NOTE_ID, ds.ProblemList.NoteIdColumn.ColumnName, DbType.Int64);



        }
        // Start 7/1/2016 Muhammad Wasim Created to use parameter for Stored Procedure [Clinical].[InsertProblemsRcopialID] end
        private void CreateParametersProblemRcopiaID(IDBManager dbManager, DSProblemLists ds)
        {
            dbManager.CreateParameters(3);


            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int32);



            dbManager.AddParameters(1, PARM_RCOPIAID, ds.ProblemList.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.ProblemList.PatientIdColumn.ColumnName, DbType.Int32);

        }
        private void CreateParametersForOrderUpdate(IDBManager dbManager, DSProblemLists ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MODIFIED_BY, ds.ProblemList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_MODIFIED_ON, ds.ProblemList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_PROBLEM_ORDER, ds.ProblemList.ProblemOrderColumn.ColumnName, DbType.String);
        }

        #endregion


        #region "Problem Lists"
        public DSProblemLists InsertProblemLists(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_INSERT, ds, ds.ProblemList.TableName);
                //if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn]) == "-1")
                //{
                //    if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.IsActiveGridColumn]) == "1")
                //        throw new Exception("This Problem already exists in Inactive Problems. Please reactivate it to add it as an Active Problem. DB");
                //    else
                //        throw new Exception("This Problem already exists in Aactive Problems. DB");
                //}
                //else if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn]) == "-2")
                //{
                //    throw new Exception("Please Enter Valid Problem.");
                //}
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEMLIST_INSERT, ex);
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
        public DSProblemLists InsertProblemLists(IDBManager dbManager, DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_INSERT, ds, ds.ProblemList.TableName);
                //if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn]) == "-1")
                //{
                //    if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.IsActiveGridColumn]) == "1")
                //        throw new Exception("This Problem already exists in Inactive Problems. Please reactivate it to add it as an Active Problem. DB");
                //    else
                //        throw new Exception("This Problem already exists in Aactive Problems. DB");
                //}
                //else if (Convert.ToString(ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn]) == "-2")
                //{
                //    throw new Exception("Please Enter Valid Problem.");
                //}
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEMLIST_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }

        public string InsertProblemDetails(string xml)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string Result = string.Empty;
                dbManager.Open();

                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_Problem_Detail_XML, xml);

                dbManager.AddParameters(1, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(2, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
                Result = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROBLEMDETAIL_INSERT_UPDATE).ToString();

                if (Result == "-1")
                    Result = "";
                return Result;
            }
            catch (Exception ex)
            {


                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemDetails", PROC_PROBLEMDETAIL_INSERT_UPDATE, ex);
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
        public DSProblemLists InsertProblemListsUnique(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_INSERT_Unique, ds, ds.ProblemList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEMLIST_INSERT, ex);
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
        //M.Wasim Added new method InsertProblemRcopialID for inserting Rcopia in Problem List on 1/7/2016.
        public DSProblemLists InsertProblemRcopialID(DSProblemLists ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersProblemRcopiaID(dbManager, ds);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEM_RCOPIAID, ds, ds.ProblemList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEM_RCOPIAID, ex);
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
        public DSProblemLists InsertProblemRcopialID(IDBManager dbManager, DSProblemLists ds)
        {
            try
            {
                CreateParametersProblemRcopiaID(dbManager, ds);
                ds = (DSProblemLists)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROBLEM_RCOPIAID, ds, ds.ProblemList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::InsertProblemLists", PROC_PROBLEM_RCOPIAID, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }
        public DSProblemLists UpdateProblemLists(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE, ds, ds.ProblemList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProblemLists::UpdateProblemLists", PROC_PROBLEMLIST_UPDATE, ex);
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

        public DSProblemLists UpdateProblemListsForInActive(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParametersForInactive(dbManager, ds, false);
                ds = (DSProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE, ds, ds.ProblemList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProblemLists::UpdateProblemListsForInActive", PROC_PROBLEMLIST_UPDATE_FOR_INACTIVE, ex);
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

        public DSProblemLists UpdateProblemListsOp(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParametersForUpdateRow(dbManager, ds, false);
                ds = (DSProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE_FOR_GRID, ds, ds.ProblemList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProblemLists::UpdateProblemListsOp", PROC_PROBLEMLIST_UPDATE_FOR_GRID, ex);
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


        public string DeleteProblemLists(string ProblemListId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSProblemLists ds = LoadProblemLists(Convert.ToInt64(ProblemListId), 0, 0, "0", "", 1, 1000);
                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROBLEMLIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    if (dtTemp != null && ds.ProblemList.Rows.Count > 0)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", false, false, true);
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
                MDVLogger.DALErrorLog("DALProblemLists::DeleteProblemLists", PROC_PROBLEMLIST_DELETE, ex);
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

        public string DeleteProblemLists(IDBManager dbManager, string ProblemListId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DSProblemLists ds = LoadProblemLists(Convert.ToInt64(ProblemListId), 0, 0, "0", "", 1, 1000);
                DataTable dtTemp = ds.ProblemList;
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROBLEMLIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    if (dtTemp != null && ds.ProblemList.Rows.Count > 0)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProblemLists::DeleteProblemLists", PROC_PROBLEMLIST_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public string CheckProblemListExists(long PatientId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_PROBLEM_EXISTS, "", DbType.String, ParamDirection.Output, null, 1);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROBLEMLIST_EXISTS).ToString();

                if (returnVal == "1" || returnVal == "0")
                {
                    return returnVal;
                }
                else
                {
                    throw new Exception(returnVal);
                }

            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::CheckProblemListExists", PROC_PROBLEMLIST_EXISTS, ex);
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




        public DSProblemLists LoadProblemListsInLabOrder(long NoteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProblemLists tempDS = new DSProblemLists();
            try
            {
                // Lab based ICDs
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NotesId", NoteId);

                tempDS = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_LAB_BASED_SELECT, tempDS, tempDS.ProblemList.TableName);
                return tempDS;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LoadTreatmentProblemListsInLabOrder(long NoteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProblemLists tempDS = new DSProblemLists();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NoteId", NoteId);

                tempDS = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TREATMENT_PROBLEMLIST_LAB_BASED_SELECT, tempDS, tempDS.ProblemList.TableName);
                return tempDS;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadTreatmentProblemListsInLabOrder", PROC_TREATMENT_PROBLEMLIST_LAB_BASED_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LoadTreatmentProblems(long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProblemLists tempDS = new DSProblemLists();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NoteId", NoteId);

                tempDS = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TREATMENT_PROBLEMLIST, tempDS, tempDS.ProblemList.TableName);
                return tempDS;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadTreatmentProblems", PROC_TREATMENT_PROBLEMLIST, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LoadProblemLists(long ProblemListId, long PatientId, long NoteId, string isHistory = "0", string active = "", int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "", string isFromSuperBill = "", string showCustomFormProblems = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            DSProblemLists tempDS = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                dbManager.CreateParameters(10);

                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (!string.IsNullOrEmpty(isFromSuperBill))
                {
                    dbManager.AddParameters(8, PARM_FROMSUPERBILL, 1);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_FROMSUPERBILL, 0);
                }

                if (showCustomFormProblems == "1")
                {
                    dbManager.AddParameters(9, PARM_CF_PROBLEMS, 1);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_CF_PROBLEMS, 0);
                }

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);


                if (isHistory == "1")
                {
                    dbManager.AddParameters(3, PARM_IS_HISTORY, isHistory);
                    DSProblemLists dsProblemListHistory = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT, ds, ds.ProblemHistory.TableName);
                    if (dsProblemListHistory != null)
                    {
                        ds.Merge(dsProblemListHistory);
                    }
                }

                if (dtTemp != null)
                {
                    if (isViewProblemList == "1" || isPrintProblemList == "1")
                    {
                        bool isViewAction = isViewProblemList == "1" ? true : false;
                        bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemsByNoteAndProcedureId(long NoteId, long PatientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);


                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMS_SELECT, ds, ds.ProblemList.TableName);


                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemsByNoteAndProcedureId", PROC_PROBLEMS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProblemLists LoadAllProblemLists(long ProblemListId, long PatientId, long NoteId, string isHistory = "0", string active = "", string isViewProblemList = "", string isPrintProblemList = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(5);

                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);


                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALL_PROBLEMLIST_SELECT, ds, ds.ProblemList.TableName);

                if (isHistory == "1")
                {
                    dbManager.AddParameters(3, PARM_IS_HISTORY, isHistory);
                    DSProblemLists dsProblemListHistory = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALL_PROBLEMLIST_SELECT, ds, ds.ProblemHistory.TableName);
                    if (dsProblemListHistory != null)
                    {
                        ds.Merge(dsProblemListHistory);
                    }
                }

                if (dtTemp != null)
                {
                    if (isViewProblemList == "1" || isPrintProblemList == "1")
                    {
                        bool isViewAction = isViewProblemList == "1" ? true : false;
                        bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadAllProblemLists", PROC_ALL_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemListsOp_Obsolete(long PatientId, long NoteId, string isHistory = "0", string active = "", int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(3, PARM_IS_ACTIVE, active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ProblemList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);



                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_OP, ds, ds.ProblemList.TableName);
                if (ds.ProblemList.Rows.Count > 0)
                {
                    if (isHistory == "1")
                    {
                        dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                        DSProblemLists dsProblemListHistory = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_OP, ds, ds.ProblemHistory.TableName);
                        if (dsProblemListHistory != null)
                        {
                            ds.Merge(dsProblemListHistory);
                        }
                    }
                }
                //Start-- Commented by Humaira Yousaf to restrict view logging on problem lists grid load
                //if (ds.ProblemList.Rows.Count > 0)
                //{
                //    if (dtTemp != null)
                //    {
                //        if (isViewProblemList == "1" || isPrintProblemList == "1")
                //        {
                //            bool isViewAction = isViewProblemList == "1" ? true : false;
                //            bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }
                //}
                //End-- Commented by Humaira Yousaf to restrict view logging on problem lists grid load
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public Tuple<List<ProblemList>, List<ProblemHistory>> LoadProblemListsOp(long PatientId, long NoteId, string isHistory = "0", string active = "", int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {

            //        DSDBAudit dsDBAudit = new DSDBAudit();
            //    DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ProblemList> problemList = new List<ProblemList>();
            List<ProblemHistory> problemHistoryList = new List<ProblemHistory>();
            try
            {
                if (active == "")
                    active = null;

                //  DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                dbManager.BeginTransaction();


                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NoteId == 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(PARM_IS_HISTORY, "0");

                dbManager.AddParameters(PARM_IS_ACTIVE, active);
                if (PageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                problemList = dbManager.ExecuteReaderMapper<ProblemList>(PROC_PROBLEMLIST_SELECT_OP);

                if (problemList.Count > 0)
                {

                    if (isHistory == "1")
                    {
                        dbManager.AddUpdateParameterValue(PARM_IS_HISTORY, isHistory);
                        problemHistoryList = dbManager.ExecuteReaderMapper<ProblemHistory>(PROC_PROBLEMLIST_SELECT_OP);
                        //  DSProblemLists dsProblemListHistory = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_OP, ds, ds.ProblemHistory.TableName);
                    }
                }
                //Start-- Commented by Humaira Yousaf to restrict view logging on problem lists grid load
                //if (ds.ProblemList.Rows.Count > 0)
                //{
                //    if (dtTemp != null)
                //    {
                //        if (isViewProblemList == "1" || isPrintProblemList == "1")
                //        {
                //            bool isViewAction = isViewProblemList == "1" ? true : false;
                //            bool isPrintAcion = isPrintProblemList == "1" ? true : false;
                //            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                //            dsDBAudit.AcceptChanges();
                //        }
                //    }
                //}
                //End-- Commented by Humaira Yousaf to restrict view logging on problem lists grid load

                Tuple<List<ProblemList>, List<ProblemHistory>> tuple = new Tuple<List<ProblemList>, List<ProblemHistory>>(problemList, problemHistoryList);
                dbManager.CommitTransaction();
                return tuple;
                //  return ds;
            }
            catch (Exception ex)
            {
                //  dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemLists", PROC_PROBLEMLIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ProblemsListModel> LoadPreviousProblemLists(long PatientId, long NoteId, string providerId)
        {
            List<ProblemsListModel> PreviousProblemsList = new List<ProblemsListModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (string.IsNullOrEmpty(providerId))
                    dbManager.AddParameters(PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROVIDER_ID, providerId);
                if (NoteId == 0)
                    dbManager.AddParameters(PARM_NOTE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, NoteId);

                return dbManager.ExecuteReaders<ProblemsListModel>(PROC_PREVIOUS_PROBLEMLISTS);
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALProblemLists::LoadPreviousProblemLists", PROC_PREVIOUS_PROBLEMLISTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemListsForInActive(long ProblemListId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_FOR_INACTIVE, ds, ds.ProblemList.TableName);
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemListsForInActive", PROC_PROBLEMLIST_SELECT_FOR_INACTIVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadProblemListsForFillData(long ProblemListId, string isViewAction = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                if (ProblemListId == 0)
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_SELECT_FOR_FILL, ds, ds.ProblemList.TableName);
                //Start 27-10-2016 Humaira Yousaf to log view action for problem lists
                if (dtTemp != null)
                {
                    if (isViewAction == "1")
                    {
                        bool isView = isViewAction == "1" ? true : false;
                        if (ds.ProblemList.Rows.Count > 0)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", isView, false, false);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 27-10-2016 Humaira Yousaf to log view action for problem lists
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemListsForInActive", PROC_PROBLEMLIST_SELECT_FOR_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<ProblemsListModel> getNoteProblemListByPatientId(long PatientId, long userId, long entityId, long NoteId, long OrderSetId)
        {
            List<ProblemsListModel> modelList = new List<ProblemsListModel>();
            ProblemsListModel model = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, userId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, entityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));
                if (OrderSetId > 0)
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, OrderSetId));
                }
                else
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, DBNull.Value));
                }

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_PROBLEMLIST_FORSOAPTEXT, parameters))
                {
                    while (reader.Read())
                    {
                        model = new ProblemsListModel();
                        var properties = typeof(ProblemsListModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        modelList.Add(model);
                    }


                }

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemList::getLatestProblemListByPatientId", PROC_GET_LATEST_PROBLEMLIST_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ProblemsListModel> getPatientProblemListByProvider(long PatientId, long ProviderId)
        {
            List<ProblemsListModel> modelList = new List<ProblemsListModel>();
            ProblemsListModel model = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));               

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_PROBLEMLIST_BYPROVIDER, parameters))
                {
                    while (reader.Read())
                    {
                        model = new ProblemsListModel();
                        var properties = typeof(ProblemsListModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        modelList.Add(model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemList::getPatientProblemListByProvider", PROC_GET_LATEST_PROBLEMLIST_BYPROVIDER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists getLatestProblemListByPatientId(long PatientId, long userId, long entityId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (userId <= 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, userId);
                if (entityId <= 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_PROBLEMLIST_BY_PATIENTI, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemList::getLatestProblemListByPatientId", PROC_GET_LATEST_PROBLEMLIST_BY_PATIENTI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists getLatestChronicProblemListByPatientId(long PatientId, long userId, long entityId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (userId <= 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, userId);
                if (entityId <= 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_CHRONICPROBLEMLIST_BY_PATIENTI, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemList::getLatestChronicProblemListByPatientId", PROC_GET_LATEST_CHRONICPROBLEMLIST_BY_PATIENTI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Problem Lists For Notes Soap"
        /// <summary>
        /// Get Problem lists Soap Text DataSet
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public DSProblemLists loadProblemListsForSoap(string ProblemListId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(ProblemListId))
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemListsForSoap", PROC_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ProblemsListModel> loadPreviousProblemListsForSoap(string ProblemListId)
        {
            List<ProblemsListModel> PreviousProblemsListForSoap = new List<ProblemsListModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (string.IsNullOrEmpty(ProblemListId))
                    dbManager.AddParameters(PARM_PROBLEMLIST_ID, DBNull.Value);
                else
                    dbManager.AddParameters(PARM_PROBLEMLIST_ID, ProblemListId);

                return dbManager.ExecuteReaders<ProblemsListModel>(PROC_PREVIOUS_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LoadProblemListsForSoap", PROC_PREVIOUS_PROBLEM_LIST_SELECT_FORSOAPTEXT_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists attachLabOrdProblemsWithNoteForSoap(string LabOrderIds, long NotesId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(LabOrderIds))
                    dbManager.AddParameters(0, PARM_LABORDER_IDS, null);
                else
                    dbManager.AddParameters(0, PARM_LABORDER_IDS, LabOrderIds);
                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_LO_PROBLEMS_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachLabOrdProblemsWithNoteForSoap", PROC_ATTACH_LO_PROBLEMS_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists attachRadiologyProblemsWithNoteForSoap(string orderIds, long NotesId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_RADIOLOGYORDER_IDS, orderIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_RO_PROBS_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachRadiologyProblemsWithNoteForSoap", PROC_ATTACH_RO_PROBS_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists attachConsultationProblemsWithNoteForSoap(string orderIds, long NotesId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_CONSULTATIONORDER_IDS, orderIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_CO_PROBS_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachConsultationProblemsWithNoteForSoap", PROC_ATTACH_CO_PROBS_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists attachProcedureProblemsWithNoteForSoap(string orderIds, long NotesId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROCEDUREORDER_IDS, orderIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PO_PROBS_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachProcedureProblemsWithNoteForSoap", PROC_ATTACH_PO_PROBS_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists attachReferralProblemsWithNoteForSoap(string referralIds, long NotesId)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REFERRAL_IDS, referralIds);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_REF_PROBS_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::attachReferralProblemsWithNoteForSoap", PROC_ATTACH_REF_PROBS_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region Notes and ProblemLists
        /// <summary>
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachProblemListsFromNotes(string ProblemListId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PROBLEM_LIST_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::DetachProblemListsFromNotes", PROC_DETACH_PROBLEM_LIST_FROM_NOTES, ex);
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
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// /// <param name="dbManager"></param>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachProblemListsFromNotes(IDBManager dbManager, string ProblemListId, long NotesId)
        {
            try
            {
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PROBLEM_LIST_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::DetachProblemListsFromNotes", PROC_DETACH_PROBLEM_LIST_FROM_NOTES, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }
        /// <summary>
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProblemLists attachProblemListWithNotes(string ProblemListId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSProblemLists ds = new DSProblemLists();

                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);



                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROBLEM_LIST_FROM_NOTES, ds, ds.ProblemList.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::AttachProblemListsFromNotes", PROC_ATTACH_PROBLEM_LIST_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// /// <param name="dbManager"></param>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProblemLists attachProblemListWithNotes(IDBManager dbManager, string ProblemListId, long NotesId)
        {
            try
            {

                DSProblemLists ds = new DSProblemLists();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROBLEM_LIST_FROM_NOTES, ds, ds.ProblemList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::AttachProblemListsFromNotes", PROC_ATTACH_PROBLEM_LIST_FROM_NOTES, ex);
                throw ex;
            }
        }
        public DSProblemLists UpdateProblemListsOrder(DSProblemLists ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.ProblemList.GetChanges();
                dbManager.Open();
                // dbManager.BeginTransaction();
                this.CreateParametersForOrderUpdate(dbManager, ds, false);
                ds = (DSProblemLists)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROBLEMLIST_UPDATE_ORDER, ds, ds.ProblemList.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALProblemLists::UpdateProblemListsOrder", PROC_PROBLEMLIST_UPDATE_ORDER, ex);
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
        /// <summary>
        /// Attach problem with note and load soap
        /// </summary>
        /// <param name="ProblemListId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProblemLists attachProblemWithNotesAndLoadSOAP(string ProblemListId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSProblemLists ds = new DSProblemLists();

                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, ProblemListId);

                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);


                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROBLEM_WITH_NOTE_LOAD_FORSOAP, ds, ds.ProblemListSoap.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::AttachProblemListsFromNotes", PROC_ATTACH_PROBLEM_WITH_NOTE_LOAD_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        // Start 27/11/2015 Muhammad Irfan Created to use dropdown values from DB end
        #region Lookups
        public DSProblemLists LookupChronicityLevel()
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHRONICITY_LEVEL_LOOKUP, ds, ds.ChronicityLevel.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LookupChronicityLevel", PROC_CHRONICITY_LEVEL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LookupSeverityType()
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SEVERITY_TYPE_LOOKUP, ds, ds.SeverityType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LookupSeverityType", PROC_SEVERITY_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LookupNoteStatus()
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_STATUS_LOOKUP, ds, ds.NoteStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LookupNoteStatus", PROC_NOTE_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LookupNoteAction()
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NOTE_ACTION_LOOKUP, ds, ds.NoteAction.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LookupNoteAction", PROC_NOTE_ACTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProblemLists LookupProblemLists(int patientId, int problemListId = -1, string ProblemName = null)
        {
            DSProblemLists ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                //Start || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list
                if (patientId <= 0)
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                }
                if (problemListId <= 0)
                {
                    dbManager.AddParameters(1, PARM_PROBLEMLIST_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PROBLEMLIST_ID, problemListId);
                }

                dbManager.AddParameters(2, PARM_PROBLEM_NAME, ProblemName);
                //End   || 08 April, 2016 || ZeeshanAK || Changes made for Batch > Patient list

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROBLEM_LIST_LOOKUP, ds, ds.ProblemListForPt.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::LookupProblemLists", PROC_PROBLEM_LIST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        // End 27/11/2015 Muhammad Irfan Created to use dropdown values from DB end


        #region Clinical Report

        public List<ProblemLookUpModel> getAllProblemsforReports()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<ProblemLookUpModel> listModel = new List<ProblemLookUpModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CHRONICITY_SEVERITY_LOOKUP);
                ProblemLookUpModel model = null;
                while (reader.Read())
                {

                    model = new ProblemLookUpModel();
                    model.ProblemListId = Convert.ToInt64(reader["ProblemListId"]);
                    model.ChronicityLevel = Convert.ToString(reader["ChronicityLevel"]);
                    model.Severity = Convert.ToString(reader["Severity"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::getAllProblemsforReports", PROC_CHRONICITY_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public List<ChronicityLookupModel> getProblemChronicityLookup()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<ChronicityLookupModel> listModel = new List<ChronicityLookupModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_CHRONICITY_LOOKUP);
                ChronicityLookupModel model = null;
                while (reader.Read())
                {
                    model = new ChronicityLookupModel();
                    model.ChronicityLevelId = Convert.ToInt64(reader["ChronicityLevelId"]);
                    model.ShortName = Convert.ToString(reader["ShortName"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::getProblemChronicityLookup", PROC_CLINICAL_CHRONICITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProblemLists LookupDiagnosisConfirmation()
        {
            var ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                var tableNames = new List<string>
                {
                    ds.DiagnosisConfirmation.TableName,
                    ds.Laterality.TableName,
                    ds.BehaviorCode.TableName,
                    ds.Grade.TableName,
                    ds.ClinicalStageGroup.TableName,
                    ds.ClinicalStageDescriptor.TableName,
                    ds.ClinicalTumor.TableName,
                    ds.ClinicalNode.TableName,
                    ds.ClinicalMetastasis.TableName,
                    ds.PathologicMetastasis.TableName,
                    ds.PathologicNode.TableName,
                    ds.PathologicStageDescriptor.TableName,
                    ds.PathologicTumor.TableName,
                    ds.PathologicStageGroup.TableName,
                    ds.pathologicStagerCancer.TableName,
                    ds.StagerCancer.TableName,
                    ds.PrimarySite.TableName,
                    ds.HistologicType.TableName,

                };
                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DIAGNOSIS_CONFIRMATION_LOOKUP, ds, tableNames);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LookupDiagnosisConfirmation", PROC_DIAGNOSIS_CONFIRMATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public DSProblemLists LoadAllCancerCodes()
        {
            var ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                ds = (DSProblemLists)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_CANCER_CODES, ds, ds.CancerCodes.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadAllCancerCodes", PROC_LOAD_CANCER_CODES, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
        public ProblemDetailXML LoadCancerDiseaseDetails(Int64 problemListId)
        {
            var ds = new DSProblemLists();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;



            ProblemDetailModel ProblemDetail = null;
            List<ProblemDetailModel> lstProblemDetail = new List<ProblemDetailModel>();
            ProblemDetailXML model = new ProblemDetailXML();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROBLEMLIST_ID, problemListId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LOAD_CANCER_DISEASE_DETAILS);
                while (reader.Read())
                {
                    ProblemDetail = new ProblemDetailModel();
                    ProblemDetail.ProblemListId = reader.IsDBNull(reader.GetOrdinal("ProblemListId")) ? "" : Convert.ToString(reader["ProblemListId"]);
                    ProblemDetail.TNMSystemCodeId = reader.IsDBNull(reader.GetOrdinal("TNMSystemCodeId")) ? "" : Convert.ToString(reader["TNMSystemCodeId"]);

                    ProblemDetail.ValueSetName = reader.IsDBNull(reader.GetOrdinal("ValueSetName")) ? "" : Convert.ToString(reader["ValueSetName"]); ;
                    model.ProblemDetails.Add(ProblemDetail);
                }
                reader.NextResult();
                while (reader.Read())
                {

                    model.CancerDiagnosisDate = reader.IsDBNull(reader.GetOrdinal("CancerDiagnosisDate")) ? "" : Convert.ToDateTime((reader["CancerDiagnosisDate"])).ToShortDateString();
                    model.CancerClinicalDiagnosisDate = reader.IsDBNull(reader.GetOrdinal("CancerClinicalDiagnosisDate")) ? "" : Convert.ToDateTime((reader["CancerClinicalDiagnosisDate"])).ToShortDateString();

                    model.CancerEffectiveDate = reader.IsDBNull(reader.GetOrdinal("CancerEffectiveDate")) ? "" : Convert.ToDateTime((reader["CancerEffectiveDate"])).ToShortDateString();
                    model.CancerIsActive = reader.IsDBNull(reader.GetOrdinal("CancerIsActive")) ? "" : Convert.ToString(reader["CancerIsActive"]);
                    model.PrimarySiteId = reader.IsDBNull(reader.GetOrdinal("PrimarySiteId")) ? "" : Convert.ToString(reader["PrimarySiteId"]);
                    model.PrimarySite = reader.IsDBNull(reader.GetOrdinal("PrimarySite")) ? null : Convert.ToString(reader["PrimarySite"]);
                    model.HistologicType = reader.IsDBNull(reader.GetOrdinal("HistologicType")) ? null : Convert.ToString(reader["HistologicType"]);
                    model.HistologicTypeId = reader.IsDBNull(reader.GetOrdinal("HistologicTypeId")) ? null : Convert.ToString(reader["HistologicTypeId"]);
                    model.NKOClinical = reader.IsDBNull(reader.GetOrdinal("NKOClinical")) ? null : Convert.ToString(reader["NKOClinical"]);
                    model.NKOPathologic = reader.IsDBNull(reader.GetOrdinal("NKOPathologic")) ? null : Convert.ToString(reader["NKOPathologic"]);

                    model.DiseaseDiscription = reader.IsDBNull(reader.GetOrdinal("DiseaseDiscription")) ? null : Convert.ToString(reader["DiseaseDiscription"]);
                }

                return model;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadCancerDiseaseDetails", PROC_LOAD_CANCER_DISEASE_DETAILS, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
        public List<SeverityLookupModel> getProblemSeverityLookup()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<SeverityLookupModel> listModel = new List<SeverityLookupModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CLINICAL_SEVERITY_LOOKUP);
                SeverityLookupModel model = null;
                while (reader.Read())
                {
                    model = new SeverityLookupModel();
                    model.SeverityTypeId = Convert.ToInt64(reader["SeverityTypeId"]);
                    model.ShortName = Convert.ToString(reader["ShortName"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::getProblemSeverityLookup", PROC_CLINICAL_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #region Legacy Notes

        public List<ProblemHx> NotesProblemHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ProblemHx> objList_ProblemHx = new List<ProblemHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_PROBLEMHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        ProblemHx model = new ProblemHx();
                        var properties = typeof(ProblemHx).GetProperties();
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
                        objList_ProblemHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::NotesProblemHxSelect", PROC_NOTES_PROBLEMHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ProblemHx;
        }

        #endregion Legacy Notes

    }
}
