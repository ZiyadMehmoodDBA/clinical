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
using MDVision.Model.Clinical.Notes;
using System.Data.SqlClient;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.Procedures;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Threading;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALProcedures
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PROCEDURES_SELECT = "Clinical.sp_ProceduresSelect";
        private const string PROC_GET_LATEST_PROCEDURES_FORSOAPTEXT = "Clinical.sp_GetLatestProceduresForSoapText";
        private const string PROC_PROCEDURES_LABBASED_SELECT = "Clinical.sp_NotesLabOrderCPTSelect";
        private const string PROC_PROCEDURES_SELECT_BYNOTEANDPATIENTID = "Clinical.sp_ProceduresByNoteAndPatientId";
        private const string PROC_PROCEDURE_INSERT = "Clinical.sp_ProceduresInsert";
        private const string PROC_PROCEDURE_UPDATE = "Clinical.sp_ProceduresUpdate";
        private const string PROC_PROCEDURE_DELETE = "Clinical.sp_ProceduresDelete";
        private const string PROC_GET_PROCEDURE_IDS = "Clinical.sp_GetProcedureIdsOfVaccineHxId";

        private const string PROC_PROCEDURE_LOOKUP = "Clinical.sp_ProcedureLookUp";
        //sp_ProcedureLookUp

        private const string PROC_Procedure_HISTORY_SELECT = "Clinical.sp_ProcedureHistorySelect";
        private const string PROC_PROCEDURE_SELECT_FORSOAPTEXT = "Clinical.sp_ProceduresSelectForSoapText";
        private const string PROC_GET_LATEST_PROCEDURES_BY_PATIENT = "Clinical.sp_GetLatestProceduresByPatientId";
        private const string PROC_INSERT_MISSING_CPT_ON_VALIDATE_VBP = "Clinical.InsertMissingCptonValidateVBP";
        private const string PROC_DETACH_PROCEDURES_FROM_NOTES = "Clinical.sp_DetachProceduresFromNotes";
        private const string PROC_ATTACH_PROCEDURES_FROM_NOTES = "Clinical.sp_AttachProceduresWithNotes";
        private const string PROC_IS_PHQ_PROCEDURE = "Clinical.sp_IsPHQProcedure";
        private const string PROC_IS_PHQ_Calculate_VBP_Socre = "Clinical.CalculateVBPSocre";

        private const string PROC_PROCEDURE_CPTLOOKUP_INSERT = "System.sp_CPTLookupInsert";
        private const string PROC_PROCEDURE_CPTLOOKUP_SELECT = "System.sp_CPTLookupSelect";
        private const string PROC_PROCEDURES_SELECT_FORSIGN = "Clinical.sp_ProceduresSelect_ForSign";

        private const string PROC_NOTES_PROCEDURES_SELECT = "[Clinical].[sp_NotesProceduresSelect]";
        private const string PROC_NOTES_PROCEDURES_AOE_FINDINGS_SELECT = "Clinical.sp_ProcedureFindingSelect";
        private const string PROC_PROCEDURES_TREATMENT_SELECT = "Clinical.sp_TreatmentProceduresForESuperBill";
        private const string PROC_LAB_PROCEDURES_TREATMENT_SELECT = "Clinical.sp_TreatmentLabOrderProceduresForESuperBill";
        
        /*
         * private const string PROC_Procedure_SELECT = "Clinical.sp_ProcedureSelect";  
        private const string PROC_CHRONICITY_LEVEL_LOOKUP = "Clinical.sp_ChronicityLevelLookup";
        private const string PROC_SEVERITY_TYPE_LOOKUP = "Clinical.sp_SeverityTypeLookup";

        private const string PROC_NOTE_STATUS_LOOKUP = "Clinical.sp_NoteStatusLookup";
        private const string PROC_NOTE_ACTION_LOOKUP = "Clinical.sp_NoteActionLookup";
        private const string PROC_PROBLEM_RCOPIAID = "Clinical.InsertProblemsRcopialID";*/
        #endregion

        #region "Parameters"

        private const string PARM_PROCEDURE_ID = "@ProcedureId";//kr
        private const string PARM_PROCEDURE_IDS = "@ProcedureIds";
        private const string PARM_START_DATE = "@StartDate";//
        private const string PARM_END_DATE = "@EndDate";//
        private const string PARM_COMMENTS = "@Comments";//
        private const string PARM_PATIENT_ID = "@PatientId";//
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_NOTEID = "@NoteId";//
        private const string PARM_PHQ_TEXT_NEEDED = "@PHQTextNeeded";//
        private const string PARM_NOTE_ID = "@NotesId";//
        private const string PARM_ForVBP = "@ForVBP";//

        private const string PARM_IS_ACTIVE = "@IsActive";//
        private const string PARM_CREATED_BY = "@CreatedBy";//
        private const string PARM_CREATED_ON = "@CreatedOn";//
        private const string PARM_MODIFIED_BY = "@ModifiedBy";//
        private const string PARM_MODIFIED_ON = "@ModifiedOn";//
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_INACTIVE_VALUE = "@InActiveChkBoxValue";//
        private const string PARM_INACTIVE_REASON = "@InActiveReason";//
        //------------------------------
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_CPTCODE = "@CPTCode";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_CPT_DESCRIPTION = "@CPT_DESCRIPTION";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";


        private const string PARM_CPT_ID = "@CPTId";
        private const string PARM_VACCINE_HX_ID = "@VaccineHxId";
        private const string PARM_IMM_THER_INJECTION_ID = "@ImmTherInjectionId";
        private const string PARM_IS_FROM_SUPPERBILL = "@IsFromSupperbill";


        private const string PARM_PROVIDER_ID = "@ProviderID";

        //------------------------------
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_IS_HISTORY = "@IsHistory";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_SHOW_EM_CODES = "@ShowEmCodes";
        private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_UNIT = "@Unit";
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        private const string PARM_BillingInfo_ID = "@BillingInfoId";
        private const string PARM_CUSTOMFORM_ID = "@CustomFormId";
        private const string PARM_SHOW_CFPROCEDURES = "@showCustomFormProcedure";
        private const string PARM_SHOW_VPROCEDURES = "@showVaccineProcedure";
        private const string PARM_IS_FINDING_UPDATE = "@IsFindingUpdate";
        private const string PARM_SHOW_IDPROCEDURES = "@showImplantableDeviceProcedure";
        private const string PARM_SURGICAL = "@Surgical";
        private const string PARM_REASON_ID = "@ReasonId";
        private const string PARM_CQM_ENCOUNTER_TYPE_ID = "@CQMEncounterTypeId";
        private const string PARM_ORDERSET_ID = "@OrderSetId";

        #endregion

        #region Constructors
        public DALProcedures()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALProcedures(SharedVariable SharedVariable)
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

        #region "Support Functions For Procedures"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSProcedures ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(25);
                dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.Procedures.ProcedureIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(22);
                dbManager.AddParameters(0, PARM_PROCEDURE_ID, ds.Procedures.ProcedureIdColumn.ColumnName, DbType.Int32);
            }

            dbManager.AddParameters(1, PARM_START_DATE, ds.Procedures.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(2, PARM_END_DATE, ds.Procedures.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_COMMENTS, ds.Procedures.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PATIENT_ID, ds.Procedures.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_NOTE_ID, ds.Procedures.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.Procedures.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.Procedures.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.Procedures.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.Procedures.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.Procedures.ModifiedOnColumn.ColumnName, DbType.DateTime);
            // dbManager.AddParameters(11, PARM_INACTIVE_VALUE, ds.Procedures.InActiveChkBoxValueColumn.ColumnName, DbType.String);
            //  dbManager.AddParameters(12, PARM_INACTIVE_REASON, ds.Procedures.InActiveReasonColumn.ColumnName, DbType.String);

            dbManager.AddParameters(11, PARM_CPTCODE, ds.Procedures.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CPT_DESCRIPTION, ds.Procedures.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_UNIT, ds.Procedures.UnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIER, ds.Procedures.ModifierColumn.ColumnName, DbType.String);
            //  dbManager.AddParameters(17, PARM_ICD9, ds.Procedures.ICD9Column.ColumnName, DbType.String);
            //  dbManager.AddParameters(18, PARM_ICD10, ds.Procedures.ICD10Column.ColumnName, DbType.String);
            //  dbManager.AddParameters(19, PARM_ICD10_DESCRIPTION, ds.Procedures.ICD10_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PROBLEMLIST_ID, ds.Procedures.ProblemListIdColumn.ColumnName, DbType.Int32);
            // if (IsInsert == true)
            {
                dbManager.AddParameters(16, PARM_SNOMEDID, ds.Procedures.SNOMEDIDColumn.ColumnName, DbType.String);
                dbManager.AddParameters(17, PARM_SNOMED_DESCRIPTION, ds.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
                dbManager.AddParameters(18, PARM_CUSTOMFORM_ID, ds.Procedures.CustomFormIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(19, PARM_SURGICAL, ds.Procedures.SurgicalColumn.ColumnName, DbType.String);
            }
            if (IsInsert == true)
            {
                dbManager.AddParameters(18, PARM_CPT_ID, ds.Procedures.CPTIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(19, PARM_VACCINE_HX_ID, ds.Procedures.VaccineHxIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(20, PARM_IMM_THER_INJECTION_ID, ds.Procedures.ImmTherInjectionIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(21, PARM_IS_FROM_SUPPERBILL, ds.Procedures.IsFromSupperBillColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(21, PARM_IS_FROM_SUPPERBILL, ds.Procedures.IsFromSupperBillColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(22, PARM_CUSTOMFORM_ID, ds.Procedures.CustomFormIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(23, PARM_SURGICAL, ds.Procedures.SurgicalColumn.ColumnName, DbType.String);
                dbManager.AddParameters(24, PARM_REASON_ID, ds.Procedures.ReasonIdColumn.ColumnName, DbType.String);
            }
            else
            {
                dbManager.AddParameters(20, PARM_REASON_ID, ds.Procedures.ReasonIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(21, PARM_CQM_ENCOUNTER_TYPE_ID, ds.Procedures.CQMEncounterTypeIdColumn.ColumnName, DbType.Int64);
                
            }
        }

        #endregion

        #region Procedures (CRUD)

        public DSProcedures loadLabBasedProcedures(long noteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProcedures ds = new DSProcedures();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NotesId", noteId);

                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_LABBASED_SELECT, ds, ds.Procedures.TableName);
                return ds;
            }
            // Lab based ICDs
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadLabBasedProcedures", PROC_PROCEDURES_LABBASED_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSProcedures loadTreatmentProcedures(long noteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProcedures ds = new DSProcedures();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NoteId", noteId);

                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_TREATMENT_SELECT, ds, ds.Procedures.TableName);
                return ds;
            }
            // Lab based ICDs
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadTreatmentProcedures", PROC_PROCEDURES_TREATMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSProcedures loadLabBasedTreatmentProcedures(long noteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProcedures ds = new DSProcedures();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@NoteId", noteId);

                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LAB_PROCEDURES_TREATMENT_SELECT,ds, ds.Procedures.TableName);
                return ds;
            }
            // Lab based ICDs
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadLabBasedTreatmentProcedures", PROC_LAB_PROCEDURES_TREATMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedures loadProcedures_Obsolete(long procedureId, long patientId, long noteId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProcedure = "", string isPrintAllergy = "", string ShowEMCodes = "", string showCustomFormProcedures = "", string showVaccineProcedures = "", string showImplantableDeviceProcedures = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProcedures ds = new DSProcedures();
            DSProcedures dsProcedures = new DSProcedures();
            DSProcedures tempDS = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.Procedures;
                dbManager.CreateParameters(12);

                if (procedureId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Procedures.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ShowEMCodes == "0")
                {
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, false);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, null);
                }
                if (showCustomFormProcedures == "")
                {
                    dbManager.AddParameters(9, PARM_SHOW_CFPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(9, PARM_SHOW_CFPROCEDURES, true);
                }
                if (showVaccineProcedures == "")
                {
                    dbManager.AddParameters(10, PARM_SHOW_VPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(10, PARM_SHOW_VPROCEDURES, true);
                }
                if(showImplantableDeviceProcedures == "" || showImplantableDeviceProcedures == "false")
                {
                    dbManager.AddParameters(11, PARM_SHOW_IDPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(11, PARM_SHOW_IDPROCEDURES, true);
                }
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.Procedures.TableName);



                if (isHistory == "1")
                {
                    dbManager.AddParameters(3, PARM_IS_HISTORY, isHistory);
                    DSProcedures dsProcedureHistory = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.ProcedureHistory.TableName);
                    if (dsProcedureHistory != null)
                    {
                        ds.Merge(dsProcedureHistory);
                    }
                }
                if (dtTemp != null)
                {
                    if (isViewProcedure == "1")
                    {
                        bool isView = isViewProcedure == "1" ? true : false;
                        if (ds.Procedures.Rows.Count > 0)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString(), null, "", isView, false, false);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadProcedures", PROC_PROCEDURES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public Tuple<List<ProcedureModel>, List<ProcedureHistoryModel>> loadProcedures(long procedureId, long patientId, long noteId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProcedure = "", string isPrintAllergy = "", string ShowEMCodes = "", string showCustomFormProcedures = "", string showVaccineProcedures = "", string showImplantableDeviceProcedures = "")
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            //DSProcedures ds = new DSProcedures();
            //IDBManager tempDBManager = ClientConfiguration.GetDBManager();
            //DSProcedures dsProcedures = new DSProcedures();
            //DSProcedures tempDS = new DSProcedures();

            List<ProcedureModel> clinicalProcedureList = new List<ProcedureModel>();
            List<ProcedureHistoryModel> clinicalProcedureHistoryList = new List<ProcedureHistoryModel>();

            Tuple<List<ProcedureModel>, List<ProcedureHistoryModel>> tuple;

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                // DataTable dtTemp = ds.Procedures;
                //dbManager.Open();
                dbManager.BeginTransaction();
                //dbManager.CreateParameters(11);

                if (procedureId == 0)
                    dbManager.AddParameters(PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(PARM_PROCEDURE_ID, procedureId);
                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, noteId);

                dbManager.AddParameters(PARM_IS_HISTORY, "0");

                dbManager.AddParameters(PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                if (ShowEMCodes == "0")
                {
                    dbManager.AddParameters(PARM_SHOW_EM_CODES, false);
                }
                else
                {
                    dbManager.AddParameters(PARM_SHOW_EM_CODES, null);
                }
                if (showCustomFormProcedures == "")
                {
                    dbManager.AddParameters(PARM_SHOW_CFPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(PARM_SHOW_CFPROCEDURES, true);
                }
                if (showVaccineProcedures == "")
                {
                    dbManager.AddParameters(PARM_SHOW_VPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(PARM_SHOW_VPROCEDURES, true);
                }
                if (showImplantableDeviceProcedures == "")
                {
                    dbManager.AddParameters(PARM_SHOW_IDPROCEDURES, false);
                }
                else
                {
                    dbManager.AddParameters(PARM_SHOW_IDPROCEDURES, true);
                }
                // ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.Procedures.TableName);
                clinicalProcedureList = dbManager.ExecuteReaderMapper<ProcedureModel>(PROC_PROCEDURES_SELECT);


                if (isHistory == "1")
                {
                    dbManager.AddUpdateParameterValue(PARM_IS_HISTORY, isHistory);

                    // dbManager.AddUpdateParameterValue(PARM_IS_HISTORY, isHistory);
                    clinicalProcedureHistoryList = dbManager.ExecuteReaderMapper<ProcedureHistoryModel>(PROC_PROCEDURES_SELECT);
                    //  DSProcedures dsProcedureHistory = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.ProcedureHistory.TableName);
                    //if (dsProcedureHistory != null)
                    //{
                    //   // ds.Merge(dsProcedureHistory);
                    //}
                }
                if (clinicalProcedureList.Count > 0)
                {
                    if (isViewProcedure == "1")
                    {
                        bool isView = isViewProcedure == "1" ? true : false;

                        new DBActivityAudit().InsertDBAuditAsync<ProcedureModel>("procedures", clinicalProcedureList, clinicalProcedureList[0].ProcedureId, null, "", isView);

                    }
                }

                dbManager.CommitTransaction();


                tuple = new Tuple<List<ProcedureModel>, List<ProcedureHistoryModel>>(clinicalProcedureList, clinicalProcedureHistoryList);
                return tuple;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadProcedures", PROC_PROCEDURES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProcedures loadProceduresForSign(long procedureId, long patientId, long noteId, long BillingInfoId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProcedure = "", string isPrintAllergy = "", string ShowEMCodes = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProcedures ds = new DSProcedures();
            IDBManager tempDBManager = ClientConfiguration.GetDBManager();
            DSProcedures dsProcedures = new DSProcedures();
            DSProcedures tempDS = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.Procedures;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(10);

                if (procedureId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                dbManager.AddParameters(3, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Procedures.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (ShowEMCodes == "0")
                {
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, false);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_SHOW_EM_CODES, null);
                }
                dbManager.AddParameters(9, PARM_BillingInfo_ID, BillingInfoId);
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT_FORSIGN, ds, ds.Procedures.TableName);



                if (isHistory == "1")
                {
                    dbManager.AddParameters(3, PARM_IS_HISTORY, isHistory);
                    DSProcedures dsProcedureHistory = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT_FORSIGN, ds, ds.ProcedureHistory.TableName);
                    if (dsProcedureHistory != null)
                    {
                        ds.Merge(dsProcedureHistory);
                    }
                }
                if (dtTemp != null)
                {
                    if (isViewProcedure == "1")
                    {
                        bool isView = isViewProcedure == "1" ? true : false;
                        if (ds.Procedures.Rows.Count > 0)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString(), null, "", isView, false, false);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }


                dbManager.CommitTransaction();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadProceduresForSign", PROC_PROCEDURES_SELECT_FORSIGN, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedures loadProceduresByNoteAndProcedureId(long noteId, long patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSProcedures ds = new DSProcedures();
            DSProcedures dsProcedures = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Procedures;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTEID, noteId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);


                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT_BYNOTEANDPATIENTID, ds, ds.Procedures.TableName);



                dbManager.CommitTransaction();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadProceduresByNoteAndProcedureId", PROC_PROCEDURES_SELECT_BYNOTEANDPATIENTID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedures InsertMissingBillingCpt(long noteId, long billingId)
        {
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_BillingInfo_ID, billingId);
                dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSERT_MISSING_CPT_ON_VALIDATE_VBP, ds, ds.Procedures.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedure::InsertMissingBillingCpt", PROC_INSERT_MISSING_CPT_ON_VALIDATE_VBP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedures insertProcedure(DSProcedures ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Procedures.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);

                ds = (DSProcedures)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT, ds, ds.Procedures.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.Procedures.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.Procedures.Rows[i][ds.Procedures.ProcedureIdColumn];
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProcedures::insertProcedure", PROC_PROCEDURE_INSERT, ex);
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

        public DSProcedures insertProcedure(IDBManager dbManager, DSProcedures ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = ds.Procedures.GetChanges();
                CreateParameters(dbManager, ds, true);

                ds = (DSProcedures)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT, ds, ds.Procedures.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.Procedures.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.Procedures.Rows[i][ds.Procedures.ProcedureIdColumn];
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProcedures::insertProcedure", PROC_PROCEDURE_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }

        public DSProcedures updateProcedure(DSProcedures ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Procedures.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);

                ds = (DSProcedures)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_UPDATE, ds, ds.Procedures.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALProcedures::updateProcedure", PROC_PROCEDURE_UPDATE, ex);
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

        public DSProcedures updateProcedure(IDBManager dbManager, DSProcedures ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = ds.Procedures.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProcedures)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_UPDATE, ds, ds.Procedures.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProcedures::updateProcedure", PROC_PROCEDURE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
        }

        public string deleteProcedure(string procedureId, string VaccineHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (VaccineHxId != "")//for a time no db aduit
                {
                    var ReturnProcedureIds = GetProcedureIdsOfVaccineHxId(VaccineHxId);
                    var ReturnProcedureIdsList = ReturnProcedureIds.Split(',');
                    DSProcedures.ProceduresDataTable dtTemp = new DSProcedures.ProceduresDataTable();
                    DSProcedures ds = new DSProcedures();
                    if (ReturnProcedureIds == "")
                    {
                        dtTemp = null;
                        returnVal = "";
                    }
                    else
                    {
                        foreach (string a in ReturnProcedureIds.Split(','))
                        {
                            ds = loadProcedures_Obsolete(Convert.ToInt64(a), 0, 0, "0", "", 1, 1000);
                            dtTemp.Merge(ds.Procedures);
                        }

                        dbManager.BeginTransaction();
                        dbManager.CreateParameters(3);
                        dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                        if (!string.IsNullOrEmpty(VaccineHxId))
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                        }
                        else
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                        }
                        dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                        returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                        if (returnVal != "")
                        {
                            throw new Exception(returnVal);
                        }
                        else
                        {
                            if (dtTemp != null)
                            {
                                foreach (DSProcedures.ProceduresRow Pr in ds.Procedures)
                                {
                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, Pr[ds.Procedures.ProcedureIdColumn].ToString(), null, "", false, false, true);
                                }
                                dsDBAudit.AcceptChanges();
                            }
                        }
                        dbManager.CommitTransaction();
                    }


                }
                else
                {
                    DSProcedures ds = loadProcedures_Obsolete(Convert.ToInt64(procedureId), 0, 0, "0", "", 1, 1000);
                    DataTable dtTemp = ds.Procedures;
                    //dbManager.Open();
                    dbManager.BeginTransaction();
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                    if (!string.IsNullOrEmpty(VaccineHxId))
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                    }
                    dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                    if (returnVal != "")
                        throw new Exception(returnVal);
                    else
                    {
                        if (dtTemp != null)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString(), null, "", false, false, true);
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
                MDVLogger.DALErrorLog("DALProcedures::deleteProcedure", PROC_PROCEDURE_DELETE, ex);
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

        private string GetProcedureIdsOfVaccineHxId(string VaccineHxId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VACCINE_HX_ID, VaccineHxId);
                dbManager.AddParameters(1, PARM_PROCEDURE_IDS, "", DbType.String, ParamDirection.Output, null, 255);
                var ReturnProcedureIds = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROCEDURE_IDS).ToString();
                return ReturnProcedureIds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteProcedure(IDBManager dbManager, string procedureId, string VaccineHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                if (VaccineHxId != "")//for a time no db aduit
                {

                    var ReturnProcedureIds = GetProcedureIdsOfVaccineHxId(VaccineHxId);
                    var ReturnProcedureIdsList = ReturnProcedureIds.Split(',');
                    DSProcedures.ProceduresDataTable dtTemp = new DSProcedures.ProceduresDataTable();
                    DSProcedures ds = new DSProcedures();
                    if (ReturnProcedureIds == "")
                    {
                        dtTemp = null;
                        returnVal = "";
                    }
                    else
                    {
                        foreach (string a in ReturnProcedureIds.Split(','))
                        {
                            ds = loadProcedures_Obsolete(Convert.ToInt64(a), 0, 0, "0", "", 1, 1000);
                            dtTemp.Merge(ds.Procedures);
                        }

                        dbManager.CreateParameters(3);
                        dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                        if (!string.IsNullOrEmpty(VaccineHxId))
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                        }
                        else
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                        }
                        dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                        returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                        if (returnVal != "")
                        {
                            throw new Exception(returnVal);
                        }
                        else
                        {
                            if (dtTemp != null)
                            {
                                foreach (DSProcedures.ProceduresRow Pr in ds.Procedures)
                                {
                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, Pr[ds.Procedures.ProcedureIdColumn].ToString(), null, "", false, false, true);
                                }
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                else
                {
                    DSProcedures ds = loadProcedures_Obsolete(Convert.ToInt64(procedureId), 0, 0, "0", "", 1, 1000);
                    DataTable dtTemp = ds.Procedures;
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                    if (!string.IsNullOrEmpty(VaccineHxId))
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                    }
                    dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                    if (returnVal != "")
                        throw new Exception(returnVal);
                    else
                    {
                        if (dtTemp != null)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Procedures.Rows[0][ds.Procedures.ProcedureIdColumn].ToString(), null, "", false, false, true);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALProcedures::deleteProcedure", PROC_PROCEDURE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public string ProcedureDeleteForImmuniztion(string VaccineHxId, IDBManager dbManager = null)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
            }
            try
            {
                if (!string.IsNullOrEmpty(VaccineHxId))//for a time no db aduit
                {
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                    if (!string.IsNullOrEmpty(VaccineHxId))
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                    }
                    dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                    if (returnVal != "")
                    {
                        throw new Exception(returnVal);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                MDVLogger
                    .DALErrorLog("DALProcedures::ProcedureDeleteForImmuniztion", PROC_PROCEDURE_DELETE, ex);
                //string[] str = ex.Message.Split('|');
                //if (str.Length > 1)
                //    return str[1].ToString();
                //else
                //    return ex.Message;
                throw ex;
            }
        }

        public DSProcedures.ProceduresDataTable GetProcedureData(string VaccineHxId, ref string ProcedureIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProcedures ds = new DSProcedures();
            DSProcedures.ProceduresDataTable dtTemp = new DSProcedures.ProceduresDataTable();
            int counter = 0;
            try
            {
                dbManager.Open();
                if (!string.IsNullOrWhiteSpace(VaccineHxId))//for a time no db aduit
                {
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, PARM_VACCINE_HX_ID, VaccineHxId);
                    dbManager.AddParameters(1, PARM_PROCEDURE_IDS, "", DbType.String, ParamDirection.Output, null, 255);
                    var ReturnProcedureIds = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROCEDURE_IDS).ToString();
                    var ReturnProcedureIdsList = ReturnProcedureIds.Split(',');
                    if (ReturnProcedureIds == "")
                    {
                        dtTemp = null;
                    }
                    else
                    {
                        foreach (string a in ReturnProcedureIds.Split(','))
                        {
                            ds = loadProcedures_Obsolete(Convert.ToInt64(a), 0, 0, "0", "", 1, 1000);
                            dtTemp.Merge(ds.Procedures);
                        }
                    }
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        foreach (DSProcedures.ProceduresRow Pr in dtTemp)
                        {
                            if (counter == 0)
                            {
                                ProcedureIds = Pr[ds.Procedures.ProcedureIdColumn].ToString();
                            }
                            else
                            {
                                ProcedureIds = "," + Pr[ds.Procedures.ProcedureIdColumn].ToString();
                            }
                            counter++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::deleteProcedure", PROC_PROCEDURE_DELETE, ex);
                string[] str = ex.Message.Split('|');
            }
            finally
            {
                dbManager.Dispose();
            }
            return dtTemp;
        }

        #endregion

        #region Notes and Procedures
        public DSProcedures getLatestProcedureByPatientId(long PatientId, long UserId, long EntityId, long ProviderId)
        {
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (UserId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, UserId);
                if (EntityId == 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(3, PARM_SHOW_VPROCEDURES, false);
                if (ProviderId == 0)
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PROVIDER_ID, ProviderId);


                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_PROCEDURES_BY_PATIENT, ds, ds.ProcedureSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedure::getLatestProcedureByPatientId", PROC_GET_LATEST_PROCEDURES_BY_PATIENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Get Problem lists Soap Text DataSet
        /// </summary>
        /// <param name="ProcedureId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public DSProcedures loadProceduresForSoap(string ProcedureId, long PatientId, string ProviderId = null)
        {
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (string.IsNullOrEmpty(ProcedureId))
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, ProcedureId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (ProviderId != null || ProviderId != "")
                {
                    long ProvId = Convert.ToInt64(ProviderId);
                    dbManager.AddParameters(2, "@NoteProviderId", ProvId);
                }
                else
                {
                    dbManager.AddParameters(2, "@NoteProviderId", DBNull.Value);
                }
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_SELECT_FORSOAPTEXT, ds, ds.ProcedureSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::LoadProceduresForSoap", PROC_PROCEDURE_SELECT_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Attaching Procedures Lists With Progress notes
        /// </summary>
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachProceduresFromNotes(string ProcedureId, long NotesId, bool ForVBP = false)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(ProcedureId))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.AddParameters(2, PARM_ForVBP, ForVBP);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PROCEDURES_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::DetachProceduresFromNotes", PROC_DETACH_PROCEDURES_FROM_NOTES, ex);
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
        /// Attaching Procedures Lists With Progress notes
        /// </summary>
        /// /// <param name="dbManager"></param>
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachProceduresFromNotes(IDBManager dbManager, string ProcedureId, long NotesId, bool ForVBP = false)
        {
            try
            {

                dbManager.CreateParameters(3);

                if (string.IsNullOrEmpty(ProcedureId))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.AddParameters(2, PARM_ForVBP, ForVBP);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PROCEDURES_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::DetachProceduresFromNotes", PROC_DETACH_PROCEDURES_FROM_NOTES, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        /// <summary>
        /// Attaching Procedures Lists With Progress notes
        /// </summary>
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProcedures attachProcedureWithNotes(string ProcedureId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSProcedures ds = new DSProcedures();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(ProcedureId))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROCEDURES_FROM_NOTES, ds, ds.Procedures.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::AttachProceduresFromNotes", PROC_ATTACH_PROCEDURES_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Attaching Procedures Lists With Progress notes
        /// </summary>
        /// /// <param name="dbManager"></param>
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSProcedures attachProcedureWithNotes(IDBManager dbManager, string ProcedureId, long NotesId)
        {
            try
            {

                DSProcedures ds = new DSProcedures();
                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(ProcedureId))
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PROCEDURES_FROM_NOTES, ds, ds.Procedures.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::AttachProceduresFromNotes", PROC_ATTACH_PROCEDURES_FROM_NOTES, ex);
                throw ex;
            }
        }

        public string isPHQProcedure(string ProcedureId, long PatientID, long providerid, long notesid)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string isPHQProcedure = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(ProcedureId))
                {
                    parameters.Add(new SqlParameter(PARM_PROCEDURE_ID, ProcedureId));
                    parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientID));
                    parameters.Add(new SqlParameter(PARM_PROVIDER_ID, providerid));
                    parameters.Add(new SqlParameter(PARM_NOTEID, notesid));
                    parameters.Add(new SqlParameter(PARM_ENTITY_ID, MDVSession.Current.EntityId));
                    using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(PROC_IS_PHQ_PROCEDURE, parameters))
                    {
                        while (reader.Read())
                        {
                            isPHQProcedure = MDVUtility.CheckStringNull(reader["isPHQ"]);
                        }
                    }
                }
                return isPHQProcedure;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::isPHQProcedure", PROC_IS_PHQ_PROCEDURE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public Tuple<String, String> CalculateVBPSocreForSoapText(long NoteId, bool PHQTextNeeded)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                string PHQSoapText = "";
                string ProceudreID = "";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));
                parameters.Add(new SqlParameter(PARM_PHQ_TEXT_NEEDED, PHQTextNeeded));
                using (SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(PROC_IS_PHQ_Calculate_VBP_Socre, parameters))
                {

                    if (reader.Read())
                    {
                        PHQSoapText = reader.GetSqlValue(0).ToString();
                        ProceudreID = reader.GetSqlValue(1).ToString();
                    }
                }
                return Tuple.Create(PHQSoapText, ProceudreID);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::CalculateVBPSocreForSoapText", PROC_IS_PHQ_Calculate_VBP_Socre, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ProcedureFindingsModel> loadProcedureFindings(String ProcedureIds, long noteid, bool isFindingUpdate = false)
        {
            List<ProcedureFindingsModel> modelList = new List<ProcedureFindingsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_PROCEDURE_IDS, ProcedureIds);
                dbManager.AddParameters(PARM_NOTEID, noteid);
                dbManager.AddParameters(PARM_IS_FINDING_UPDATE, isFindingUpdate);

                return dbManager.ExecuteReaders<ProcedureFindingsModel>(PROC_NOTES_PROCEDURES_AOE_FINDINGS_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::loadProcedureFindings", PROC_NOTES_PROCEDURES_AOE_FINDINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Lookups
        public DSProcedures LookupProcedures()
        {
            DSProcedures ds = new DSProcedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_LOOKUP, ds, ds.Procedures.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::LookupProcedures", PROC_PROCEDURE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion



        public List<ProceduresModel> getNoteProceduresByPatientId(long PatientId, long userId, long entityId, long NoteId, long ProviderId, long OrderSetId)
        {
            List<ProceduresModel> modelList = new List<ProceduresModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, userId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, entityId));
                parameters.Add(new SqlParameter(PARM_NOTEID, NoteId));
                parameters.Add(new SqlParameter(PARM_SHOW_VPROCEDURES, false));
                parameters.Add(new SqlParameter(PARM_PROVIDER_ID, ProviderId));
                if (OrderSetId > 0)
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, OrderSetId));
                }
                else
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, DBNull.Value));
                }

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_PROCEDURES_FORSOAPTEXT, parameters))
                {
                    while (reader.Read())
                    {
                        ProceduresModel model = new ProceduresModel();
                        var properties = typeof(ProceduresModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALProcedures::getNoteProceduresByPatientId", PROC_GET_LATEST_PROCEDURES_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region CPTLookupInsert
        public DSProcedures InsertCPTLookup(ref DSProcedures ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, "@CPTLookupId", ds.CPTLookup.CPTLookupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, "@CPTCode", ds.CPTLookup.CPTCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CPT_Description", ds.CPTLookup.CPT_DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@SNOMEDId", ds.CPTLookup.SNOMEDIdColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@SNOMED_Description", ds.CPTLookup.SNOMED_DescriptionColumn.ColumnName, DbType.String);
                ds = (DSProcedures)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CPTLOOKUP_INSERT, ds, ds.CPTLookup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::InsertCPTLookup", PROC_PROCEDURE_CPTLOOKUP_INSERT, ex);
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

        public DSProcedures SelectCPTLookup(string SNOMEDId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSProcedures ds = new DSProcedures();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@SNOMEDId", SNOMEDId);
                ds = (DSProcedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CPTLOOKUP_SELECT, ds, ds.CPTLookup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCCM::SelectCPTLookup", PROC_PROCEDURE_CPTLOOKUP_SELECT, ex);
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

        #region Legacy Notes

        public List<Procedure> NotesProcedureSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<Procedure> objList_Procedure = new List<Procedure>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_PROCEDURES_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        Procedure model = new Procedure();
                        var properties = typeof(Procedure).GetProperties();
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
                        objList_Procedure.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedures::NotesProcedureSelect", PROC_NOTES_PROCEDURES_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_Procedure;
        }

        #endregion Legacy Notes


    }
}
