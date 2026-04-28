// Created By:  Muhammad Ahmad Imran
// Created Date: 08/02/2016
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
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.Templates;
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALComplaint
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SELECT_BODY_PARTS = "Clinical.sp_BodyPartsSelect";
        private const string PROC_UPDATE_COMPLAINT_FROM_NOTE = "Clinical.sp_ComplaintUpdateFromNote";
        private const string PROC_INSERT_COMPLAINT = "Clinical.sp_ComplaintInsert";
        private const string PROC_INSERT_COMPLAINT_DETAIL = "Clinical.sp_ComplaintDetailInsert";
        private const string PROC_INSERT_NOTES_COMPLAINT = "Clinical.sp_NotesComplaintInsert";
        private const string PROC_COMPLAINT_CASE_LOOKUP = "Clinical.sp_Complaint_CaseLookup";
        private const string PROC_COMPLAINT_TEMPLATE_LETTER_CATEGORY_LOOKUP = "Clinical.sp_Template_LetterCategoryLookup";
        private const string PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORY_LOOKUP = "Clinical.Template_Letter_TagCategoryLookup";
        private const string PROC_COMPLAINT_TEMPLATE_LETTER_TAGCATEGORYNAME_LOOKUP = "sp_Letter_Template_TagCategoryNameLookup";
        private const string PROC_COMPLAINT_DETAILS_SELECT = "Sp_Complaint_DetailSelect";
        private const string PROC_GET_COMPLAINT_FORSOAP = "Clinical.Sp_Complaint_DetailSelectForSoapText";

        private const string PROC_COMPLAINT_DETAILS_UPDATE = "Clinical.sp_ComplaintDetailUpdate";
        private const string PROC_COMPLAINT_CHARACTER_LOOKUP = "Clinical.sp_Complaint_CharacterLookup";
        private const string PROC_COMPLAINT_CONTEXT_LOOKUP = "Clinical.sp_Complaint_ContextLookup";
        private const string PROC_COMPLAINT_DURATION_LOOKUP = "Clinical.sp_Complaint_DurationLookup";
        private const string PROC_COMPLAINT_FREQUENCY_LOOKUP = "Clinical.sp_Complaint_FrequencyLookup";
        private const string PROC_COMPLAINT_LOCATION_LOOKUP = "Clinical.sp_Complaint_LocationLookup";
        private const string PROC_COMPLAINT_QUALITY_LOOKUP = "Clinical.sp_Complaint_QualityLookup";
        private const string PROC_COMPLAINT_RADIATION_LOOKUP = "Clinical.sp_Complaint_RadiationLookup";
        private const string PROC_COMPLAINT_REVIELEDBY_LOOKUP = "Clinical.sp_Complaint_RelievedByLookup";
        private const string PROC_COMPLAINT_SEVERITY_LOOKUP = "Clinical.sp_Complaint_SeverityLookup";
        private const string PROC_COMPLAINT_AGGRAVATED_LOOKUP = "Clinical.sp_Complaint_AggravatedByLookup";
        private const string PROC_COMPLAINT_NOTE = "Clinical.Sp_GetNoteComplaint";
        private const string PROC_COMPLAINTNOTE_ID = "Clinical.Sp_GetNotesComplaintID";
        private const string PROC_COMPLAINT_DETAILS_ID = "Clinical.Sp_ComplaintDetailsID";
        private const string PROC_COMPLAINT_DETAIL_DELETE = "Clinical.Sp_ComplaintDelete";
        private const string PROC_COMPLAINT_RESET = "Clinical.Sp_ComplaintReset";
        private const string PROC_COMPLAINT_DELETE = "Clinical.Sp_ComplaintDelete";
        private const string PROC_DETACH_Complaint_FROM_NOTES = "Clinical.sp_DetachComplaintFromNotes";

        private const string PROC_ATTACH_Complaint_FROM_NOTES = "Clinical.sp_AttachComplaintWithNotes";
        private const string PROC_COMPLAINT_DETAILS_FOR_HL7 = "Clinical.Sp_Complaint_DetailSelectForHL7";
        private const string PROC_COMPLAINT_SELECT_For_FaceSheet = "Clinical.sp_FaceSheetComplaintSelect";
        private const string PROC_SELECT_COMPLAINT_FACESHEET = "Clinical.sp_SelectComplaintForFaceSheet";


        private const string PROC_NOTES_COMPLAINTS_SELECT = "Clinical.sp_NotesComplaintsSelect";
        private const string PROC_NOTES_COMPLAINTS_HPI_SELECT = "Clinical.sp_NotesComplaintsHPIDataSelect";
        private const string PROC_LATESTCOMPLAINTS_BYPATIENT = "Clinical.GetLatestComplaintsByPatientId";
        private const string PROC_IS_LATESTCOMPLAINTS_EXISTS = "Clinical.sp_IsPatientComplaintExists";
        
        #endregion

        #region "Parameters"


        private const string PARM_COMPLAINT_ID = "@ComplaintId";
        private const string PARM_COMPLAINT_ID_FOR_USE = "@ComplaintIdForUse";
        private const string PARM_COMPLAINT_DETAIL_ID = "@ComplaintDetailId";
        private const string PARM_COMPLAINT_DETAIL_ID_FOR_USE = "@ComplaintDetailIdForUse";
        private const string PARM_PREVIOUS_HISTORY = "@PreviousHistory";
        private const string PARM_IS_CHIEF_COMPLAINT = "@IsChiefComplaint";
        private const string PARM_CASE = "@Complaint_CaseId";
        private const string PARM_LOCATION = "@Complaint_LocationIds";
        private const string PARM_RADIATION = "@Complaint_RadiationId";
        private const string PARM_QUALITY = "@Complaint_QualityId";
        private const string PARM_SEVERITY = "@Complaint_SeverityId";
        private const string PARM_ONSET = "@Onset";
        private const string PARM_COMPLAINT_DURATIONId = "@Complaint_DurationId";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_FREQUENCY = "@Complaint_FrequencyId";
        private const string PARM_CONTEXT = "@Complaint_ContextId";
        private const string PARM_CHARACTER = "@Complaint_CharacterIds";
        private const string PARM_ASSOCIATED_WITH = "@AssociatedWith";
        private const string PARM_PRECIPITATED_BY = "@PrecipitatedBy";
        private const string PARM_AGGRAVATED_BY = "@Complaint_AggravatedById";
        private const string PARM_RELIEVED_BY = "@Complaint_RelievedById";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_COMPLAINT_DESCRIPTION = "@ComplaintDescription";
        private const string PARM_DATE_CAPTURED = "@DateCaptured";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_OVERALL_COMMENTS = "@OverallComments";
        private const string PARM_NOTEID = "@NotesId";
        private const string PARM_NOTES_COMPLAINT_Id = "@NotesComplaintId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CATEGORY_ID = "@CategoryId";
        private const string PARM_TAG_CATEGORY_ID = "@TagCategoryId";
        private const string PARM_TAG_CATEGORYNAME_ID = "@TagCategoryNameId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ICD_9 = "@ICD9";
        private const string PARM_ICD_10 = "@ICD10";
        private const string PARM_ICD_9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD_10_DESCRIPTION = "@ICD10_DESCRIPTION";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_USERID = "@UserId";
        private const string PARM_ENTITYID = "@EntityId";
        private const string PARM_IS_REPORTED = "@IsReported";
        private const string PARM_IS_BODYPART = "@IsBodyPart";


        #endregion

        #region Constructors
        public DALComplaint()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALComplaint(SharedVariable SharedVariable)
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

        #region "Support Functions For Rcopia"

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 12/02/2016
        //OverView: Methods "CreateParametersForInsertComplaintDetail" for create Parameters for insert complaint Detail
        private void CreateParametersForInsertComplaintDetail(IDBManager dbManager, DSClinicalComplaint ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(34);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_COMPLAINT_DETAIL_ID, ds.ComplaintDetail.ComplaintDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_COMPLAINT_DETAIL_ID, ds.ComplaintDetail.ComplaintDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_COMPLAINT_ID, ds.ComplaintDetail.ComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_COMPLAINT_DESCRIPTION, ds.ComplaintDetail.ComplaintDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_CHIEF_COMPLAINT, ds.ComplaintDetail.IsChiefComplaintColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CASE, ds.ComplaintDetail.Complaint_CaseIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(5, PARM_LOCATION, ds.ComplaintDetail.Complaint_LocationIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_RADIATION, ds.ComplaintDetail.Complaint_RadiationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_QUALITY, ds.ComplaintDetail.Complaint_QualityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_SEVERITY, ds.ComplaintDetail.Complaint_SeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_ONSET, ds.ComplaintDetail.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_DURATION, ds.ComplaintDetail.DurationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_COMPLAINT_DURATIONId, ds.ComplaintDetail.Complaint_DurationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_FREQUENCY, ds.ComplaintDetail.Complaint_FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_CONTEXT, ds.ComplaintDetail.Complaint_ContextIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_CHARACTER, ds.ComplaintDetail.Complaint_CharacterIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ASSOCIATED_WITH, ds.ComplaintDetail.AssociatedWithColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_PRECIPITATED_BY, ds.ComplaintDetail.PrecipitatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_AGGRAVATED_BY, ds.ComplaintDetail.Complaint_AggravatedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(18, PARM_RELIEVED_BY, ds.ComplaintDetail.Complaint_RelievedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(19, PARM_COMMENTS, ds.ComplaintDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_PREVIOUS_HISTORY, ds.ComplaintDetail.PreviousHistoryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_MODIFIED_BY, ds.ComplaintDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_BY, ds.ComplaintDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_CREATED_ON, ds.ComplaintDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_MODIFY_ON, ds.ComplaintDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_COMPLAINT_DETAIL_ID_FOR_USE, ds.ComplaintDetail.ComplaintDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_ICD_9, ds.ComplaintDetail.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_ICD_10, ds.ComplaintDetail.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_SNOMED_ID, ds.ComplaintDetail.SNOMEDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_SNOMED_DESCRIPTION, ds.ComplaintDetail.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_ICD_9_DESCRIPTION, ds.ComplaintDetail.ICD9DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_ICD_10_DESCRIPTION, ds.ComplaintDetail.ICD10DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_IS_REPORTED, ds.ComplaintDetail.IsReportedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARM_IS_BODYPART, ds.ComplaintDetail.IsBodyPartColumn.ColumnName, DbType.Byte);
        }

        private void ParamsForUpdateComplaintFromNote(IDBManager dbManager, DSClinicalComplaint ds)
        {
            dbManager.CreateParameters(4);
            dbManager.AddParameters(0, PARM_COMPLAINT_DETAIL_ID, ds.ComplaintDetail.ComplaintDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_COMPLAINT_DESCRIPTION, ds.ComplaintDetail.ComplaintDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_MODIFIED_BY, ds.Complaint.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_MODIFY_ON, ds.Complaint.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }


        // Created By:  Muhammad Ahmad Imran
        // Created Date: 12/02/2016
        //OverView: Methods "CreateParametersForInsertComplaintDetail" for create Parameters for insert complaint
        private void CreateParametersForInsertComplaint(IDBManager dbManager, DSClinicalComplaint ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ds.Complaint.ComplaintIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ds.Complaint.ComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Complaint.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DATE_CAPTURED, ds.Complaint.DateCapturedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_OVERALL_COMMENTS, ds.Complaint.OverallCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SOAP_TEXT, ds.Complaint.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, ds.Complaint.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.Complaint.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.Complaint.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFY_ON, ds.Complaint.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_COMPLAINT_ID_FOR_USE, ds.Complaint.ComplaintIdColumn.ColumnName, DbType.Int64);

        }
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 12/02/2016
        //OverView: Methods "CreateParametersForInsertNotesComplaint" for create Parameters for insert NotesComplaint
        private void CreateParametersForInsertNotesComplaint(IDBManager dbManager, DSClinicalComplaint ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(3);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_NOTES_COMPLAINT_Id, ds.NotesComplaint.NotesComplaintIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_NOTES_COMPLAINT_Id, ds.NotesComplaint.NotesComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_COMPLAINT_ID, ds.NotesComplaint.ComplaintIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_NOTEID, ds.NotesComplaint.NotesIdColumn.ColumnName, DbType.Int64);

        }
        #endregion


        // Created By:  Muhammad Ahmad Imran
        // Created Date: 09/02/2016
        //OverView: Methods "InsertComplaint" for save complaint Detail info
        public DSClinicalComplaint InsertComplaintDetail(DSClinicalComplaint ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertComplaintDetail(dbManager, ds, true);
                ds = (DSClinicalComplaint)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_COMPLAINT_DETAIL, ds, ds.ComplaintDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::InsertComplaintDetail", PROC_INSERT_COMPLAINT_DETAIL, ex);
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
        public List<BodyPartModel> GetBodyPartsLookUp(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_NOTEID, MDVUtility.ToLong(NotesId));
                return dbManager.ExecuteReaders<BodyPartModel>(PROC_SELECT_BODY_PARTS);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetBodyPartsLookUp", PROC_SELECT_BODY_PARTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        
        public DSClinicalComplaint UpdateComplaintFromNotes(DSClinicalComplaint ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ParamsForUpdateComplaintFromNote(dbManager, ds);
                ds = (DSClinicalComplaint)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_UPDATE_COMPLAINT_FROM_NOTE, ds, ds.ComplaintDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::UpdateComplaintFromNotes", PROC_UPDATE_COMPLAINT_FROM_NOTE, ex);
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 11/02/2016
        //OverView: Methods "InsertComplaint" for save complaint
        public DSClinicalComplaint InsertComplaint(DSClinicalComplaint ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertComplaint(dbManager, ds, true);
                ds = (DSClinicalComplaint)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_COMPLAINT, ds, ds.Complaint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::InsertComplaint", PROC_INSERT_COMPLAINT, ex);
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 11/02/2016
        //OverView: Methods "InsertNotesComplaint" for save Notes complaint
        public DSClinicalComplaint InsertNotesComplaint(DSClinicalComplaint ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertNotesComplaint(dbManager, ds, true);
                ds = (DSClinicalComplaint)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_NOTES_COMPLAINT, ds, ds.NotesComplaint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::InsertNotesComplaint", PROC_INSERT_NOTES_COMPLAINT, ex);
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
        #region Complaint lookUps

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Case.

        public DSClinicalComplaintLookup GetCase()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_CASE_LOOKUP, ds, ds.Complaint_Case.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetCase", PROC_COMPLAINT_CASE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Created By:  Wasim Malik
        // Created Date: 10/02/2016
        // Purpose : Get Complaints and complaint Details.
        public DSClinicalComplaint GetComplaints_ComplaintDetails(long ComplaintId, long NotesId)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ComplaintId);
                dbManager.AddParameters(1, PARM_NOTEID, NotesId);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_DETAILS_SELECT, ds, ds.NotesComplaint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaint_ComplaintDetails", PROC_COMPLAINT_DETAILS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSClinicalComplaint loadComplaintsForFaceSheet(long patientId, long pageNumber, long rowsPerPage)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            dbManager.CreateParameters(4);
            if (patientId > 0)
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
            else
                dbManager.AddParameters(0, PARM_PATIENT_ID, null);

            if (pageNumber > 0)
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
            else
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
            if (rowsPerPage > 0)
                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, rowsPerPage);
            else
                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);

            dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.ComplaintDetail.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_SELECT_For_FaceSheet, ds, ds.FaceSheetComplaints.TableName);

            dbManager.Dispose();
            return ds;
        }

        public List<ComplaintsModel> GetNoteSoapComplaintsWithDetails(long NoteId)
        {
            List<ComplaintsModel> listComplaints = new List<ComplaintsModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_COMPLAINT_ID, DBNull.Value));
                parameters.Add(new SqlParameter(PARM_NOTEID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_GET_COMPLAINT_FORSOAP, parameters))
                {
                    while (reader.Read())
                    {
                        ComplaintsModel model = new ComplaintsModel();

                        var properties = typeof(ComplaintsModel).GetProperties();

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

                        listComplaints.Add(model);
                    }
                }

                return listComplaints;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetNoteSoapComplaintsWithDetails", PROC_GET_COMPLAINT_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public ComplaintsModelWaper GetLatestComplaintsByPatientId(long PatientId, long NoteId)
        {
            ComplaintsModelWaper wraper = new ComplaintsModelWaper();          
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_NOTEID, NoteId));

                var reader = dbManager.ExecuteReader(PROC_LATESTCOMPLAINTS_BYPATIENT, parameters);

                wraper.Complaints = GetNoteSoapComplaintsWithDetails(NoteId);

                List<HPINotesFindings> listHPISymFindings = new List<HPINotesFindings>();

                HPINotesFindings model = null;

                while (reader.Read())
                {
                    model = new HPINotesFindings();

                    model.HPITemplateId = !String.IsNullOrEmpty(reader["HPITemplateId"].ToString()) ? reader["HPITemplateId"].ToString() : "";
                    model.TemplateName = !String.IsNullOrEmpty(reader["TemplateName"].ToString()) ? reader["TemplateName"].ToString() : "";
                    model.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    model.HPISymptomId = !String.IsNullOrEmpty(reader["HPISymptomsId"].ToString()) ? reader["HPISymptomsId"].ToString() : "";
                    model.SymptomName = !String.IsNullOrEmpty(reader["SymptomName"].ToString()) ? reader["SymptomName"].ToString() : "";
                    model.Desc = !String.IsNullOrEmpty(reader["SymptomDescription"].ToString()) ? reader["SymptomDescription"].ToString() : "";
                    model.HPITemplateSymptomId = !String.IsNullOrEmpty(reader["HPITemplateSymptomsId"].ToString()) ? reader["HPITemplateSymptomsId"].ToString() : "";
                    model.HPINotesFindingsId = !String.IsNullOrEmpty(reader["HPINotesFindingId"].ToString()) ? reader["HPINotesFindingId"].ToString() : "";
                    model.IsSelected = !String.IsNullOrEmpty(reader["IsSelected"].ToString()) ? reader["IsSelected"].ToString() : "";
                    model.SymptomDescription = !String.IsNullOrEmpty(reader["SymptomDescription"].ToString()) ? reader["SymptomDescription"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    listHPISymFindings.Add(model);

                }

                wraper.HPINotesFindings = listHPISymFindings;
                wraper.HPINotesFindings = wraper.HPINotesFindings.Where(a => a.NotesId == NoteId.ToString()).ToList();

                reader.NextResult();
                while (reader.Read())
                {   
                    wraper.PrevComplaintFreeText = !String.IsNullOrEmpty(reader["SoapText"].ToString()) ? reader["SoapText"].ToString() : "";
                    wraper.ComplaintId = MDVUtility.ToStr(reader["NewComplaintId"].ToString());
                }

                return wraper;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetLatestComplaintsByPatientId", PROC_LATESTCOMPLAINTS_BYPATIENT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string isPatientComplaintExists(long PatientId, long UserId, long EntityId)
        {            
            IDBManager dbManager = ClientConfiguration.GetDBManager();           
            try
            {
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_USERID, UserId);
                dbManager.AddParameters(2, PARM_ENTITYID, EntityId);


                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_LATESTCOMPLAINTS_EXISTS));
                return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHPI::isPatientComplaintExists", PROC_IS_LATESTCOMPLAINTS_EXISTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalComplaint GetComplaints_ComplaintDetailsForSoap(long ComplaintId, long NotesId)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ComplaintId);
                dbManager.AddParameters(1, PARM_NOTEID, NotesId);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_COMPLAINT_FORSOAP, ds, ds.ComplaintDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaint_ComplaintDetails", PROC_GET_COMPLAINT_FORSOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Wasim Malik
        // Created Date: 10/02/2016
        // Purpose : Update complaint Details.
        public DSClinicalComplaint UpdateComplaintDetails(DSClinicalComplaint ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForInsertComplaintDetail(dbManager, ds, false);
                ds = (DSClinicalComplaint)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_DETAILS_UPDATE, ds, ds.ComplaintDetail.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALQuestions::UpdateComplaintDetails", PROC_COMPLAINT_DETAILS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Location.

        public DSClinicalComplaintLookup GetLocation()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_LOCATION_LOOKUP, ds, ds.Complaint_Location.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetLocation", PROC_COMPLAINT_CASE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Radiation.

        public DSClinicalComplaintLookup GetRadiation()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_RADIATION_LOOKUP, ds, ds.Complaint_Radiation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetRadiation", PROC_COMPLAINT_RADIATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Quality.

        public DSClinicalComplaintLookup GetQuality()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_QUALITY_LOOKUP, ds, ds.Complaint_Quality.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetQuality", PROC_COMPLAINT_QUALITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Severity.

        public DSClinicalComplaintLookup GetSeverity()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_SEVERITY_LOOKUP, ds, ds.Complaint_Severity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetSeverity", PROC_COMPLAINT_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Duration.

        public DSClinicalComplaintLookup GetDuration()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_DURATION_LOOKUP, ds, ds.Complaint_Duration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetDuration", PROC_COMPLAINT_DURATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Frequency.
        public DSClinicalComplaintLookup GetFrequency()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_FREQUENCY_LOOKUP, ds, ds.Complaint_Frequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetFrequency", PROC_COMPLAINT_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Context.
        public DSClinicalComplaintLookup GetContext()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_CONTEXT_LOOKUP, ds, ds.Complaint_Context.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetContext", PROC_COMPLAINT_CONTEXT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint Character.

        public DSClinicalComplaintLookup GetCharacter()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_CHARACTER_LOOKUP, ds, ds.Complaint_Character.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetCharacter", PROC_COMPLAINT_CHARACTER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint AggravatedBy.

        public DSClinicalComplaintLookup GetAggravatedBy()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_AGGRAVATED_LOOKUP, ds, ds.Complaint_AggravatedBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetAggravatedBy", PROC_COMPLAINT_AGGRAVATED_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/02/2016
        // Purpose : Look up for Complaint RelievedBy.

        public DSClinicalComplaintLookup GetRevieledBy()
        {
            DSClinicalComplaintLookup ds = new DSClinicalComplaintLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalComplaintLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_REVIELEDBY_LOOKUP, ds, ds.Complaint_RelievedBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetRevieledBy", PROC_COMPLAINT_REVIELEDBY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        #region Complaints

        /// Author : Wasim Malik
        /// Purpose : function Get Notes Complaint.
        /// Date : 11 Feb 2016
        //public DSClinicalComplaint GetNotesComplaint(long NotesID)
        //{
        //    DSClinicalComplaint ds = new DSClinicalComplaint();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        dbManager.AddParameters(0, PARM_NOTEID, NotesID);
        //        ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_NOTE, ds, ds.NotesComplaint.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALComplaint::GetComplaint_Note", PROC_COMPLAINT_NOTE, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }

        //}

        //Author : Wasim Malik
        //Purpose : function Get Notes Complaint.
        //Date : 11 Feb 2016
        public DSClinicalComplaint GetNotesComplaint(long CompliantID)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, CompliantID);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_NOTE, ds, ds.Complaint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaint_Note", PROC_COMPLAINT_NOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// Author : Wasim Malik
        /// Purpose : function Get ComplaintID.
        /// Date : 11 Feb 2016
        public DSClinicalComplaint GetNotesComplaintID(long NotesID)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTEID, NotesID);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINTNOTE_ID, ds, ds.NotesComplaint.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaintNote_ID", PROC_COMPLAINTNOTE_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        /// Author : Wasim Malik
        /// Purpose : function Get ComplaintDetail ID.
        /// Date : 11 Feb 2016
        public DSClinicalComplaint GetComplaintDetailsID(long CompliantID)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, CompliantID);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_DETAILS_ID, ds, ds.ComplaintDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaint_Details_ID", PROC_COMPLAINT_DETAILS_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        /// Author : M Ahmad Imran
        /// Purpose : function Get Complaints info for HL7.
        /// Date :27 April 2016
        public DSClinicalComplaint GetComplaintDetailsForHL7(long NotesID)
        {
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_NOTEID, NotesID);
                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COMPLAINT_DETAILS_FOR_HL7, ds, ds.ComplaintDetail.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::GetComplaintDetailsForHL7", PROC_COMPLAINT_DETAILS_FOR_HL7, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        /// Author : Wasim Malik
        /// Purpose : function Delete Complaint.
        /// Date : 12 Feb 2016
        public string DeleteCompliantDetail(long CompliantDetailID,long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_COMPLAINT_DETAIL_ID, CompliantDetailID);
                dbManager.AddParameters(1, PARM_NOTEID, NotesId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COMPLAINT_DETAIL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::DeleteCompliantDetail", PROC_COMPLAINT_DETAIL_DELETE, ex);
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 16/02/2016
        //OverView: Methods "ResetComplaint" for Reset complaint
        public string ResetComplaint(long ComplaintID,long? NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ComplaintID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(2, PARM_NOTEID, NotesId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COMPLAINT_RESET).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::ResetComplaint", PROC_COMPLAINT_RESET, ex);
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


        public DSClinicalComplaint LoadAllComplaintsForFaceSheet(long PatientId, int PageNumber = 1, int RowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalComplaint ds = new DSClinicalComplaint();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                DataTable dtTemp = ds.FaceSheetComplaints;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(4);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.FaceSheetComplaints.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_COMPLAINT_FACESHEET, ds, ds.FaceSheetComplaints.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALComplaints::LoadAllComplaintsForFaceSheet", PROC_SELECT_COMPLAINT_FACESHEET, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<Complaints> NotesComplaintsSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<Complaints> objList_Complaints = new List<Complaints>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTEID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_COMPLAINTS_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        Complaints model = new Complaints();
                        var properties = typeof(Complaints).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_Complaints.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::NotesComplaintsSelect", PROC_NOTES_COMPLAINTS_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_Complaints;
        }

        public List<ComplaintsHPI> NotesComplaintsHPIDataSelect(Int64 NoteID)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ComplaintsHPI> objList_ComplaintsHPI = new List<ComplaintsHPI>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                //  parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTEID, Convert.ToInt32(NoteID)));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_COMPLAINTS_HPI_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        ComplaintsHPI model = new ComplaintsHPI();
                        var properties = typeof(ComplaintsHPI).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_ComplaintsHPI.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::sp_NotesComplaintsHPIDataSelect", PROC_NOTES_COMPLAINTS_HPI_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ComplaintsHPI;
        }

        #endregion
        #region Notes and Birth History
        /// <summary>
        /// Author : Azhar Shahzad.
        /// Purpose : Detaching Birth History From Progress notes
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachComplaintFromNotes(long ComplaintID, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (ComplaintID <= 0)
                {
                    dbManager.AddParameters(0, PARM_COMPLAINT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_COMPLAINT_ID, ComplaintID);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTEID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTEID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_Complaint_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::detachComplaintFromNotes", PROC_DETACH_Complaint_FROM_NOTES, ex);
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
        /// Author : Azhar Shahzad.
        /// Purpose : Attaching Birth History With Progress notes
        /// Date : 6 january 2016
        /// <param name="birthHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSClinicalComplaint attachComplaintWithNotes(long ComplaintID, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSClinicalComplaint ds = new DSClinicalComplaint();

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_COMPLAINT_ID, ComplaintID);
                dbManager.AddParameters(1, PARM_NOTEID, NotesId);

                ds = (DSClinicalComplaint)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_Complaint_FROM_NOTES, ds, ds.NotesComplaint.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALComplaint::AtachComplaintFromNotes", PROC_ATTACH_Complaint_FROM_NOTES, ex);
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
