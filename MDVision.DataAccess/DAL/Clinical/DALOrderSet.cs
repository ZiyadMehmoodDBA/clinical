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
using MDVision.Model;
using System.Data.SqlClient;
using MDVision.Common.Utilities;
using MDVision.Model.Lookups;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Patient;
using MDVision.Model.Clinical.FollowUp;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Clinical.Notes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALOrderSet
    {

        #region "Stored Procedure Names"
        private const string PROC_ORDER_SETS_SELECT = "Clinical.sp_OrderSetSelect";
        private const string PROC_ORDER_SET_FILL = "Clinical.sp_OrderSetFill";
        private const string PROC_ORDER_SETS_INSERT = "Clinical.sp_OrderSetInsert";
        private const string PROC_ORDER_SETS_UPDATE = "Clinical.sp_OrderSetUpdate";
        private const string PROC_ORDER_SETS_DELETE = "Clinical.sp_OrderSetDelete";
        private const string PROC_ORDER_SETS_UPDATE_STATUS = "Clinical.sp_OrderSetUpdateStatus";


        private const string PROC_ORDER_SETS_PATIENT_EDUCATION_SELECT = "Clinical.sp_OrderSetPatientEducation_Select";
        private const string PROC_ORDER_SETS_PATIENT_EDUCATION_DELETE = "Clinical.sp_OrderSetPatientEducation_Delete";
        private const string PROC_ORDER_SETS_PATIENT_EDUCATION_INSERT = "Clinical.sp_OrderSetPatientEducation_Insert";
        private const string PROC_ORDER_SETS_PATIENT_EDUCATION_LOOKUP = "Clinical.sp_OrderSetPatientEducation_Lookup";

        private const string PROC_ORDER_SETS_PATIENT_REFERRALS_SELECT = "Clinical.sp_OrderSetReferralsSelect";
        private const string PROC_ORDER_SETS_PATIENT_REFERRALS_UPDATE = "Clinical.sp_OrderSetReferralsUpdate";
        private const string PROC_ORDER_SETS_PATIENT_REFERRALS_DELETE = "Clinical.sp_OrderSetReferralsDelete";
        private const string PROC_ORDER_SETS_PATIENT_REFERRALS_INSERT = "Clinical.sp_OrderSetReferralsInsert";

        private const string PROC_ORDER_SETS_FOLLOW_UP_INSERT = "Clinical.sp_OS_FollowUpInsert";
        private const string PROC_ORDER_SETS_FOLLOW_UP_UPDATE = "Clinical.sp_OS_FollowUpUpdate";
        private const string PROC_ORDER_SETS_FOLLOW_UP_DELETE = "Clinical.sp_OS_FollowUpDelete";
        private const string PROC_ORDER_SETS_FOLLOW_UP_SELECT = "Clinical.sp_OS_FollowUpSelect";

        private const string PROC_ORDER_SETS_INSERT_TO_NOTES = "Clinical.sp_OrderSetInsertToNoteNew";
        private const string PROC_ORDER_SETS_DELETE_FROM_NOTES = "Clinical.sp_OrderSetDeleteFromNote";
        private const string PROC_ORDER_SETS_SAVE_AS = "Clinical.sp_DefaultOrdersetSaveAs";
        private const string PROC_ORDER_SET_LOOKUP = "Clinical.sp_OrderSetLookup";
        private const string PROC_NOTE_ORDERSET_INSERT = "Clinical.sp_NoteOrderSetInsert";
        private const string PROC_NOTE_ORDERSET_DELETE = "Clinical.sp_NoteOrderSetDelete";
        private const string PROC_NOTE_ORDERSET_SELECT = "Clinical.sp_NoteOrderSetSelect";
        private const string PROC_NOTE_ORDERSET_UPDATE = "Clinical.sp_NoteOrderSetUpdate";
        private const string PROC_ORDERSET_COMPONENTS_DELETE = "Clinical.sp_OrderSetDeleteToNote";
        private const string PROC_ORDER_SETS_NAME_SELECT = "Clinical.sp_GetOrderSetName";
        private const string PROC_ORDERSET_PROBLEM_SELECT = "Clinical.sp_OrderSetProblemSelect";
        private const string PROC_ORDERSET_PROBLEM_DELETE = "Clinical.sp_OrderSetProblemDelete";
        private const string PROC_GET_PATIENT_AND_ORDSET_ASSPROB = "Clinical.sp_GetPatientAndOrdSetAssProb";
        private const string PROC_ORDER_SETS_PATIENT_REFERRAL_PROCEDURE_SELECT = "Clinical.sp_OrderSetReferralProcedureSelect";
        private const string PROC_ORDER_SETS_REFERRALS_PROCEDURE_DELETE = "Clinical.sp_OrderSetReferralProcedureDelete";
        private const string PROC_ORDER_SET_PROVIDERS_LOOKUP = "Clinical.sp_OrderSetProviderLookup";
        private const string PROC_ORDER_SET_TEMPLATE_LOOKUP = "Clinical.OrderSetTemplateLookup";      
        #endregion

        #region "Parameters"
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ORDER_SET_ID = "@OrderSetId";
        private const string PARM_ORDER_SET_IDS = "@OrderSetIds";
        private const string PARM_ORDER_SET_NAME = "@OrderSetName";
        private const string PARM_ORDER_SET_HEADING = "@FormHeading";
        private const string PARM_ORDER_SET_HTML = "@OrderSetHTML";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_IS_SPECIALTY_ALL = "@IsSpecialtyAll";
        private const string PARM_IS_PROVIDER_ALL = "@IsProviderAll";
        private const string PARM_PROVIDER_NAMES = "@ProviderNames";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_SPECIALTY_NAMES = "@SpecialtyNames";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_ORDER_SET_PAT_EDUCATION_ID = "@OrderSetPatEducationId";
        private const string PARM_DOC_ID = "@DocId";
        private const string PARM_DOC_TYPE = "@DocType";
        private const string PARM_FILE_TYPE = "@FileType";
        private const string PARM_DOCUMENT_NAME = "@DocumentName";
        private const string PARM_FILE_STREAM = "@FileStream";
        private const string PARM_PAGES = "@Pages";

        private const string PARM_ORDER_SET_REFERRAL_ID = "@OrderSetReferralId";
        private const string PARM_REF_PROVIDER_ID = "@RefProviderId";
        private const string PARM_CPT_CODE_DESCRIPTION = "@CPTCodeDescription";
        private const string PARM_PAN = "@PAN";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FROM_DATE = "@DateFrom";
        private const string PARM_TO_DATE = "@DateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_TYPE = "@Type";
        private const string PARM_DATE = "@Date";
        private const string PARM_TIME = "@Time";
        private const string PARM_ASSIGNEE_ID = "@AssigneeId";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_REFERRAL_PROBLEM_ID = "@ReferralProblemId";
        private const string PARM_REFERRAL_PROCEDURE_ID = "@ReferralProcedureId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODEDESCRIPTION = "@CPTCodeDescription";
        private const string PARM_URGENCY_ID = "@UrgencyId";
        private const string PARM_REASON = "@Reason";
        private const string PARM_FOLLOWUP_TEXT = "@FollowUpText";
        private const string PARM_CREATE_APPOINTMENT = "@CreateAppointment";
        private const string PARM_APPOINTMENT_DATE = "@AppointmentDate";
        private const string PARM_FROM_TIME = "@FromTime";
        private const string PARM_TO_TIME = "@ToTime";
        private const string PARM_VISIT = "@Visits";
        private const string PARM_DATE_FROM = "@DateFrom";
        private const string PARM_DATE_TO = "@DateTo";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsurance";
        private const string PARM_FACILITY_FROM_ID = "@FacilityFrom";
        private const string PARM_FACILITY_TO_ID = "@FacilityTo";
        private const string PARM_VISITS_USED_ID = "@VisitsUsed";
        private const string PARM_TOSPECIALTYID = "@ToSpecialtyId";

        private const string PARM_FOLLOWUP_ID = "@FollowUpId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_SCHEDULE_COUNT = "@ScheduleCount";
        private const string PARM_SCHEDULE_TYPE = "@ScheduleType";
        public const string PRAM_DURATION = "@Duration";
        public const string PARM_NEW_ORDER_SET_ID = "@newOrderSetId";
        private const string PARM_NOTE_OS_ID = "@NoteOSId";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_CDS_ID = "@CDSId";
        private const string PARM_ORDERSET_COMMONENTS = "@OrderSetComponents";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_IS_DEFAULT_ORDER_SET = "@IsDefaultOrderSet";
        

        private const string PARM_PROBLEMIDS = "@ProblemListIDs";
        private const string PARM_PROCEDUREIDS = "@ProcedureIDs";
        private const string PARM_LABORDERIDS = "@LabOrderIDs";
        private const string PARM_RADIOLOGYORDERIDS = "@RadiologyOrderIDs";
        private const string PARM_FOLLOWUPIDS = "@FollowUpIDs";
        private const string PARM_PATIENTEDUCATIONIDS = "@PatientEducationIDs";
        private const string PARM_REFERRALSIDS = "@ReferralsIDs";
        private const string PARM_IMMUNIZATIONIDS = "@ImmunizationIDs";
        private const string PARAM_ADD_IN_VALID_AGE_RECORDS_IN_HXTAB = "@AddInValidAgeRecordsInHxTab";
        private const string PARM_THERAPEUTICIDS = "@TherapeuticIDs";
        private const string PARM_PROCEDUREORDERIDS = "@ProcedureOrderIDs";
        private const string PARM_ORDERSET_PROBLEM_XML = "@OrderSetProblemXML";
        private const string PARM_DEFAULT_FOLLOWUP_ID = "@IsDefaultFollowUpId";
        private const string PARM_ORDERSET_PROBLEM_ID = "@OrderSetProblemId";
        private const string PARM_PATIENTPROBLEM_IDS = "@PatientProblemIds";
        private const string PARM_ORDERSETASSOCIATEDPROBLEM_IDS = "@OrderSetAssocProblemIds";
        private const string PARM_PROVIDER_ID_NOTE = "@ProviderIdNote";
        private const string PARM_XML = "@Xml";
        private const string PARM_ORDER_SET_REFERRAL_PROCEDURE_ID = "@OrderSetReferralProcedureId";
        private const string PARM_ORDER_SET_BY_TEMPLATE_ID = "@TemplateID";
        private const string PARM_FOLLOWUP_DAYS = "@FollowUpDays";
        private const string PARM_FOLLOWUP_MONTHS = "@FollowUpMonths";
        private const string PARM_FOLLOWUP_YEARS = "@FollowUpYears";
        #endregion

        #region "Constructors"
        public DALOrderSet()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALOrderSet(SharedVariable SharedVariable)
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

        #region "Support Functions"

        private void createParameters(IDBManager dbManager, OrderSetModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, model.OrderSetId);
            else
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, model.OrderSetId);
            dbManager.AddParameters(1, PARM_ORDER_SET_NAME, model.OrderSetName);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(3, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(4, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(7, PARM_PROVIDER_IDS, String.IsNullOrEmpty(model.ProviderIds) ? null : model.ProviderIds);
            dbManager.AddParameters(8, PARM_SPECIALTY_IDS, String.IsNullOrEmpty(model.SpecialtyIds) ? null : model.SpecialtyIds);
            dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            dbManager.AddParameters(10, PARM_COMMENTS, String.IsNullOrEmpty(model.Comments) ? null : model.Comments);
            dbManager.AddParameters(11, PARM_ORDERSET_PROBLEM_XML, String.IsNullOrEmpty(model.OrderSetProblemXML) ? null : model.OrderSetProblemXML);
            dbManager.AddParameters(12, PARM_DEFAULT_FOLLOWUP_ID, model.DefaultFollowUpId);
        }
        #endregion

        #region "CRUD Order Set"

        /// Author: Azeem Raza Tayyab
        /// Purpose: To Insert Order Set
        /// Date : 10-Jan- 2017
        public string insertOrderSet(OrderSetModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createParameters(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_INSERT));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::InsertOrderSet", PROC_ORDER_SETS_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOrderSet detachOrderSetFromNotes(long PatientId, string OrderSetId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSOrderSet ds = new DSOrderSet();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_ORDER_SET_ID, OrderSetId);
                dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                ds = (DSOrderSet)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDERSET_COMPONENTS_DELETE, ds, ds.DeletedList.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::detachOrderSetFromNotes", PROC_ORDERSET_COMPONENTS_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Author: Azeem Raza Tayyab
        /// Purpose: To load Order Set
        /// Date : 10-Jan- 2017
        public List<OrderSetModel> loadOrderSet(string OrderSetName, int? isActive, string providerIds, string specialityIds, long pageNumber, long rowsPerPage)
        {
            List<OrderSetModel> listOrderSets = new List<OrderSetModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (isActive == null)
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, isActive);
                }

                if (pageNumber <= 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, rowsPerPage);

                if (String.IsNullOrEmpty(providerIds))
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, providerIds);

                if (String.IsNullOrEmpty(specialityIds))
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, null);
                else
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, specialityIds);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
                if (String.IsNullOrEmpty(OrderSetName))
                    dbManager.AddParameters(6, PARM_ORDER_SET_NAME, null);
                else
                    dbManager.AddParameters(6, PARM_ORDER_SET_NAME, OrderSetName);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(7, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_SELECT);
                OrderSetModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetName = !String.IsNullOrEmpty(reader["OrderSetName"].ToString()) ? reader["OrderSetName"].ToString() : "";
                    model.SpecialtyNames = !String.IsNullOrEmpty(reader["SpecialtyNames"].ToString()) ? reader["SpecialtyNames"].ToString() : "";
                    model.ProviderNames = !String.IsNullOrEmpty(reader["ProviderNames"].ToString()) ? reader["ProviderNames"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.ModifiedByName = !String.IsNullOrEmpty(reader["ModifiedByName"].ToString()) ? reader["ModifiedByName"].ToString() : "";

                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSets", PROC_ORDER_SETS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<OrderSetModel> loadOrderSetName(String OrderSetIds)
        {
            List<OrderSetModel> listOrderSets = new List<OrderSetModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ORDER_SET_IDS, OrderSetIds);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_NAME_SELECT);
                OrderSetModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetName = !String.IsNullOrEmpty(reader["OrderSetName"].ToString()) ? reader["OrderSetName"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSetName", PROC_ORDER_SETS_NAME_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        

        /// Author: Azeem Raza Tayyab
        /// Purpose: To Delete Order Set
        /// Date : 10-Jan- 2017
        public string deleteOrderSet(string formID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, formID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::DeleteOrderSet", PROC_ORDER_SETS_DELETE, ex);
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

        public List<OrderSetProblemModel> LoadOrderSetProblem(string orderSetId, string OrderSetProblemId)
        {
            List<OrderSetProblemModel> listOrderSetProblems = new List<OrderSetProblemModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (String.IsNullOrEmpty(orderSetId))
                    dbManager.AddParameters(PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(PARM_ORDER_SET_ID, orderSetId);
                if (String.IsNullOrEmpty(OrderSetProblemId))
                    dbManager.AddParameters(PARM_ORDERSET_PROBLEM_ID, null);
                else
                    dbManager.AddParameters(PARM_ORDERSET_PROBLEM_ID, OrderSetProblemId);
                listOrderSetProblems = dbManager.ExecuteReaders<OrderSetProblemModel>(PROC_ORDERSET_PROBLEM_SELECT);
                return listOrderSetProblems;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::LoadOrderSetProblem", PROC_ORDERSET_PROBLEM_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }


        public List<OrderSetProblemModel> LoadPatientAndOrdProblems(string orderSetId, string PatientId)
        {
            List<OrderSetProblemModel> listOrderSetProblems = new List<OrderSetProblemModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (String.IsNullOrEmpty(orderSetId))
                    dbManager.AddParameters(PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(PARM_ORDER_SET_ID, orderSetId);
                if (String.IsNullOrEmpty(PatientId))
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                listOrderSetProblems = dbManager.ExecuteReaders<OrderSetProblemModel>(PROC_GET_PATIENT_AND_ORDSET_ASSPROB);
                return listOrderSetProblems;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::LoadPatientAndOrdProblems", PROC_GET_PATIENT_AND_ORDSET_ASSPROB, ex);
                throw ex;
            }
            finally
            {
            }
        }
        
        /// Author: Azeem Raza Tayyab
        /// Purpose: To Fill Order Set
        /// Date : 10-Jan- 2017
        /// 
        public List<OrderSetModel> fillOrderSet(string orderSetId, string NoteId, string CDSId)
        {
            List<OrderSetModel> listOrderSets = new List<OrderSetModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);

                if (String.IsNullOrEmpty(orderSetId))
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, orderSetId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (String.IsNullOrEmpty(NoteId))
                    dbManager.AddParameters(2, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTES_ID, NoteId);

                if (String.IsNullOrEmpty(CDSId))
                    dbManager.AddParameters(3, PARM_CDS_ID, null);
                else
                    dbManager.AddParameters(3, PARM_CDS_ID, CDSId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SET_FILL);
                OrderSetModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetName = !String.IsNullOrEmpty(reader["OrderSetName"].ToString()) ? reader["OrderSetName"].ToString() : "";
                    model.SpecialtyIds = !String.IsNullOrEmpty(reader["SpecialtyIds"].ToString()) ? reader["SpecialtyIds"].ToString() : "";
                    model.ProviderIds = !String.IsNullOrEmpty(reader["ProviderIds"].ToString()) ? reader["ProviderIds"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.OrderSetComponents = !String.IsNullOrEmpty(reader["OrderSetComponents"].ToString()) ? reader["OrderSetComponents"].ToString() : "";
                    model.EntityId = !String.IsNullOrEmpty(reader["EntityId"].ToString()) ? MDVUtility.ToLong(reader["EntityId"].ToString()) : 0;
                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSets", PROC_ORDER_SETS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: Azeem Raza Tayyab
        /// Purpose: To Update Order Set
        /// Date : 10-Jan- 2017
        public string updateOrderSet(OrderSetModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = "";
                dbManager.Open();
                this.createParameters(dbManager, model, false);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_UPDATE));
                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                    return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::UpdateOrderSet", PROC_ORDER_SETS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: Azeem Raza Tayyab
        /// Purpose: To Update Order Set Active Inactive
        /// Date :10-Jan- 2017
        public string updateOrderSetStatus(string formId, string isActive)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (String.IsNullOrEmpty(formId))
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, formId);
                if (String.IsNullOrEmpty(isActive))
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, isActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_ORDER_SETS_UPDATE_STATUS);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::activeInactiveOrderSet", PROC_ORDER_SETS_UPDATE_STATUS, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string saveAsOrderSet(string OrderSetId, string OrderSetName, string DefaultFollowUpId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                string NewOrderSetId = "-1";
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (String.IsNullOrEmpty(OrderSetId))
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_ID, OrderSetId);
                if (String.IsNullOrEmpty(OrderSetName))
                    dbManager.AddParameters(1, PARM_ORDER_SET_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_ORDER_SET_NAME, OrderSetName);
                dbManager.AddParameters(2, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(3, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_NEW_ORDER_SET_ID, NewOrderSetId, DbType.Int64, ParamDirection.Output);
                if (String.IsNullOrEmpty(DefaultFollowUpId))
                    dbManager.AddParameters(5, PARM_DEFAULT_FOLLOWUP_ID, null);
                else
                    dbManager.AddParameters(5, PARM_DEFAULT_FOLLOWUP_ID, MDVUtility.ToInt64(DefaultFollowUpId));
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_SAVE_AS));
                if (retunVal.Contains("Violation of UNIQUE KEY"))
                    retunVal = "-1";
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::saveAsOrderSet", PROC_ORDER_SETS_SAVE_AS, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string deleteOrderSetProblemList(long problemListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ORDERSET_PROBLEM_ID, problemListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_ORDERSET_PROBLEM_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetProblemList", PROC_ORDERSET_PROBLEM_DELETE, ex);
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

        #region " Order Set Patient Education "

        private void createOrderSetPatientEducationParameters(IDBManager dbManager, OrderSetPatientEducationModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, model.OrderSetPatEducationId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, model.OrderSetPatEducationId, DbType.Int64);

            dbManager.AddParameters(1, PARM_ORDER_SET_ID, model.OrderSetId);
            dbManager.AddParameters(2, PARM_DOC_ID, model.DocId);
            dbManager.AddParameters(3, PARM_DOC_TYPE, model.DocType);

            dbManager.AddParameters(4, PARM_FILE_TYPE, model.FileType);
            dbManager.AddParameters(5, PARM_DOCUMENT_NAME, model.DocumentName);
            dbManager.AddParameters(6, PARM_FILE_STREAM, model.FileStream, DbType.Binary);
            dbManager.AddParameters(7, PARM_PAGES, model.Pages);
            dbManager.AddParameters(8, PARM_COMMENTS, model.Comments);
            dbManager.AddParameters(9, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(10, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(11, PARM_ENTITY_ID, null);
            else
                dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }

        public string insertOrderSetPatientEducation(OrderSetPatientEducationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createOrderSetPatientEducationParameters(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_EDUCATION_INSERT));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::insertOrderSetPatientEducation", PROC_ORDER_SETS_PATIENT_EDUCATION_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteOrderSetPatientEducation(string PatientEducationId, string DocId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, PatientEducationId);
                dbManager.AddParameters(1, PARM_DOC_ID, DocId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_EDUCATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetPatientEducation", PROC_ORDER_SETS_PATIENT_EDUCATION_DELETE, ex);
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

        public List<OrderSetPatientEducationModel> loadOrderSetPatientEducation(long OrderSetPatEducationId, long OrderSetId, string DocType, long pageNumber, long rowsPerPage)
        {
            List<OrderSetPatientEducationModel> listOrderSets = new List<OrderSetPatientEducationModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (OrderSetPatEducationId == 0)
                    dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, OrderSetPatEducationId);


                if (OrderSetId == 0)
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, OrderSetId);

                if (String.IsNullOrEmpty(MDVUtility.ToStr(DocType)))
                    dbManager.AddParameters(2, PARM_DOC_TYPE, null);
                else
                    dbManager.AddParameters(2, PARM_DOC_TYPE, DocType);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);


                if (pageNumber <= 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage <= 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_EDUCATION_SELECT);
                OrderSetPatientEducationModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetPatientEducationModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetPatEducationId = !String.IsNullOrEmpty(reader["OrderSetPatEducationId"].ToString()) ? reader["OrderSetPatEducationId"].ToString() : "";

                    model.DocId = !String.IsNullOrEmpty(reader["DocId"].ToString()) ? reader["DocId"].ToString() : "";
                    model.DocType = !String.IsNullOrEmpty(reader["DocType"].ToString()) ? reader["DocType"].ToString() : "";
                    model.FilePath = !String.IsNullOrEmpty(reader["FilePath"].ToString()) ? reader["FilePath"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.InfoDoc = !String.IsNullOrEmpty(reader["InfoDoc"].ToString()) ? reader["InfoDoc"].ToString() : "";
                    model.NonInfoDoc = !String.IsNullOrEmpty(reader["NonInfoDoc"].ToString()) ? reader["NonInfoDoc"].ToString() : "";
                    model.CreatedByName = !String.IsNullOrEmpty(reader["CreatedByName"].ToString()) ? reader["CreatedByName"].ToString() : "";               
                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSetPatientEducation", PROC_ORDER_SETS_PATIENT_EDUCATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void CreatePatDocInsertParameters(IDBManager dbManager, DSOrderSet ds)
        {
            dbManager.CreateParameters(12);

            dbManager.AddParameters(0, PARM_ORDER_SET_PAT_EDUCATION_ID, ds.PatientEducation.OrderSetPatEducationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_DOC_ID, ds.PatientEducation.DocIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_ORDER_SET_ID, ds.PatientEducation.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_DOC_TYPE, ds.PatientEducation.DocTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FILE_TYPE, ds.PatientEducation.FileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DOCUMENT_NAME, ds.PatientEducation.DocumentNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_FILE_STREAM, ds.PatientEducation.FileStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(7, PARM_PAGES, ds.PatientEducation.PagesColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.PatientEducation.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.PatientEducation.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.PatientEducation.ModifiedByColumn.ColumnName, DbType.String);
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(11, PARM_ENTITY_ID, null);
            else
                dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        public DSOrderSet InsertAdmin_OSPatientEducation(DSOrderSet ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePatDocInsertParameters(dbManager, ds);
                ds = (DSOrderSet)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_EDUCATION_INSERT, ds, ds.PatientEducation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::InsertClinical_OrderSetPatientEducation", PROC_ORDER_SETS_PATIENT_EDUCATION_INSERT, ex);
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

        #region " Order Set Patient Referrals "

        private void createOrderSetPatientReferralsOutgoingDetail(IDBManager dbManager, OrderSetPatientReferralModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(28);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, model.OrderSetReferralId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, model.OrderSetReferralId);

            dbManager.AddParameters(1, PARM_ORDER_SET_ID, model.OrderSetId);
            dbManager.AddParameters(2, PARM_TYPE, model.Type);
            dbManager.AddParameters(3, PARM_DATE, model.Date);
            dbManager.AddParameters(4, PARM_TIME, model.Time);
            dbManager.AddParameters(5, PARM_PROVIDER_ID, model.ProviderId);
            dbManager.AddParameters(6, PARM_REF_PROVIDER_ID, model.RefProviderId);
            dbManager.AddParameters(7, PARM_ASSIGNEE_ID, model.AssigneeId);
            dbManager.AddParameters(8, PARM_STATUS, model.Status);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(10, PARM_CREATED_BY, model.CreatedBy);
            dbManager.AddParameters(11, PARM_CREATED_ON, model.CreatedOn);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(14, PARM_SOAP_TEXT, model.SoapText);
            dbManager.AddParameters(15, PARM_PAN, model.PAN);
            dbManager.AddParameters(16, PARM_REASON, model.Reason);
            dbManager.AddParameters(17, PARM_VISIT, model.Visits);
            dbManager.AddParameters(18, PARM_DATE_FROM, model.DateFrom);
            dbManager.AddParameters(19, PARM_DATE_TO, model.DateTo);
            dbManager.AddParameters(20, PARM_PATIENT_INSURANCE_ID, model.PatientInsurance);
            dbManager.AddParameters(21, PARM_FACILITY_FROM_ID, model.FacilityFrom);
            dbManager.AddParameters(22, PARM_FACILITY_TO_ID, model.FacilityTo);
            dbManager.AddParameters(23, PARM_VISITS_USED_ID, model.VisitsUsed);
            dbManager.AddParameters(24, PARM_COMMENTS, model.Comments);
            dbManager.AddParameters(25, PARM_TOSPECIALTYID, model.SpecialtyFrom);
            dbManager.AddParameters(26, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(27, PARM_XML, model.XML);
        }

        public string insertOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createOrderSetPatientReferralsOutgoingDetail(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_REFERRALS_INSERT));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::createOrderSetPatientReferralsOutgoingDetail", PROC_ORDER_SETS_PATIENT_REFERRALS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createOrderSetPatientReferralsOutgoingDetail(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_REFERRALS_UPDATE));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::updateOrderSetPatientReferralsOutgoingDetail", PROC_ORDER_SETS_PATIENT_REFERRALS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteOrderSetPatientReferralsOutgoingDetail(string OrderSetReferralId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, OrderSetReferralId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_REFERRALS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetPatientReferralsOutgoingDetail", PROC_ORDER_SETS_PATIENT_REFERRALS_DELETE, ex);
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
        public string deleteOrderSetReferralsProcedure(string OrderSetReferralProcedureId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_PROCEDURE_ID, OrderSetReferralProcedureId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_REFERRALS_PROCEDURE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetReferralsProcedure", PROC_ORDER_SETS_REFERRALS_PROCEDURE_DELETE, ex);
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
        
        public List<OrderSetPatientReferralModel> loadOrderSetPatientReferralsOutgoingDetail(long orderSetReferralId, long orderSetId, string IsActive, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "")
        {
            List<OrderSetPatientReferralModel> listOrderSets = new List<OrderSetPatientReferralModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(14);

                if (orderSetReferralId == 0)
                    dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, orderSetReferralId);

                if (orderSetId == 0)
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, orderSetId);

                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                if (string.IsNullOrEmpty(procedureName))
                    dbManager.AddParameters(5, PARM_CPT_CODE_DESCRIPTION, null);
                else
                    dbManager.AddParameters(5, PARM_CPT_CODE_DESCRIPTION, procedureName);

                if (providerId == 0)
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(6, PARM_PROVIDER_ID, providerId);

                if (string.IsNullOrEmpty(Pan))
                    dbManager.AddParameters(7, PARM_PAN, null);
                else
                    dbManager.AddParameters(7, PARM_PAN, Pan);

                if (string.IsNullOrEmpty(DateFrom))
                    dbManager.AddParameters(8, PARM_FROM_DATE, null);
                else
                    dbManager.AddParameters(8, PARM_FROM_DATE, DateFrom);

                if (string.IsNullOrEmpty(DateTo))
                    dbManager.AddParameters(9, PARM_TO_DATE, null);
                else
                    dbManager.AddParameters(9, PARM_TO_DATE, DateTo);

                if (status == 0)
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);

                if (string.IsNullOrEmpty(Type))
                    dbManager.AddParameters(11, PARM_TYPE, null);
                else
                    dbManager.AddParameters(11, PARM_TYPE, Type);

                if (string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(12, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(12, PARM_IS_ACTIVE, (IsActive == "1" ? true : false));

                if (RefproviderId == 0)
                    dbManager.AddParameters(13, PARM_REF_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(13, PARM_REF_PROVIDER_ID, RefproviderId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_REFERRALS_SELECT);
                OrderSetPatientReferralModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetPatientReferralModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetReferralId = !String.IsNullOrEmpty(reader["OrderSetReferralId"].ToString()) ? reader["OrderSetReferralId"].ToString() : "";
                    model.Date = !String.IsNullOrEmpty(reader["Date"].ToString()) ? reader["Date"].ToString() : "";
                    model.Time = !String.IsNullOrEmpty(reader["Time"].ToString()) ? reader["Time"].ToString() : "";
                    model.PAN = !String.IsNullOrEmpty(reader["PAN"].ToString()) ? reader["PAN"].ToString() : "";
                    model.RefProvider = !String.IsNullOrEmpty(reader["RefProviderName"].ToString()) ? reader["RefProviderName"].ToString() : "";
                    model.Status = !String.IsNullOrEmpty(reader["StatusName"].ToString()) ? reader["StatusName"].ToString() : "";
                    model.Assignee = !String.IsNullOrEmpty(reader["AssigneeName"].ToString()) ? reader["AssigneeName"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.Provider = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.FacilityToName = !String.IsNullOrEmpty(reader["FacilityToName"].ToString()) ? reader["FacilityToName"].ToString() : "";
                    model.FacilityFromName = !String.IsNullOrEmpty(reader["FacilityFromName"].ToString()) ? reader["FacilityFromName"].ToString() : "";
                    model.FacilityTo = !String.IsNullOrEmpty(reader["FacilityTo"].ToString()) ? reader["FacilityTo"].ToString() : "";
                    model.FacilityFrom = !String.IsNullOrEmpty(reader["FacilityFrom"].ToString()) ? reader["FacilityFrom"].ToString() : "";
                    model.VisitType = !String.IsNullOrEmpty(reader["VisitType"].ToString()) ? reader["VisitType"].ToString() : "";
                    model.Visits = !String.IsNullOrEmpty(reader["Visits"].ToString()) ? reader["Visits"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.RefProviderId = !String.IsNullOrEmpty(reader["RefProviderId"].ToString()) ? reader["RefProviderId"].ToString() : "";
                    model.Reason = !String.IsNullOrEmpty(reader["Reason"].ToString()) ? reader["Reason"].ToString() : "";
                    model.AssigneeId = !String.IsNullOrEmpty(reader["AssigneeId"].ToString()) ? reader["AssigneeId"].ToString() : "";
                    model.SpecialtyFromName = !String.IsNullOrEmpty(reader["SpecialtyFromName"].ToString()) ? reader["SpecialtyFromName"].ToString() : "";
                    model.SpecialtyFrom = !String.IsNullOrEmpty(reader["ToSpecialtyId"].ToString()) ? reader["ToSpecialtyId"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.Procedures = !String.IsNullOrEmpty(reader["Procedures"].ToString()) ? reader["Procedures"].ToString() : "";
                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSetPatientReferralsOutgoingDetail", PROC_ORDER_SETS_PATIENT_REFERRALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<ReferralProcedureModel> loadOrderSetReferralsProcedure(long orderSetReferralId)
        {
            List<ReferralProcedureModel> listOrderSets = new List<ReferralProcedureModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (orderSetReferralId == 0)
                    dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ORDER_SET_REFERRAL_ID, orderSetReferralId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_PATIENT_REFERRAL_PROCEDURE_SELECT);
                ReferralProcedureModel model = null;
                while (reader.Read())
                {
                    model = new ReferralProcedureModel();
                    model.OrderSetReferralProcedureId = !String.IsNullOrEmpty(reader["OrderSetReferralProcedureId"].ToString()) ? reader["OrderSetReferralProcedureId"].ToString() : "";
                    model.OrderSetReferralId = !String.IsNullOrEmpty(reader["OrderSetReferralId"].ToString()) ? reader["OrderSetReferralId"].ToString() : "";
                    model.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    model.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPTCodeDescription"].ToString()) ? reader["CPTCodeDescription"].ToString() : "";
                    model.Urgency = !String.IsNullOrEmpty(reader["UrgencyId"].ToString()) ? reader["UrgencyId"].ToString() : "";
                    model.Procedure = !String.IsNullOrEmpty(reader["Procedure"].ToString()) ? reader["Procedure"].ToString() : "";
                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSetReferralsProcedure", PROC_ORDER_SETS_PATIENT_REFERRAL_PROCEDURE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        #endregion

        #region " Order Set FollowUp "

        private void createOrderSetFollowUpParameters(IDBManager dbManager, OrdertSetFollowUpModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(23);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FOLLOWUP_ID, model.FollowUpId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FOLLOWUP_ID, model.FollowUpId);

            dbManager.AddParameters(1, PARM_ORDER_SET_ID, model.OrderSetId);

            if (string.IsNullOrEmpty(model.FacilityId))
                dbManager.AddParameters(2, PARM_FACILITY_ID, null);
            else
            dbManager.AddParameters(2, PARM_FACILITY_ID, model.FacilityId);

            if (string.IsNullOrEmpty(model.ProviderId))
                dbManager.AddParameters(3, PARM_PROVIDER_ID, null);
            else
            dbManager.AddParameters(3, PARM_PROVIDER_ID, model.ProviderId);

            dbManager.AddParameters(4, PARM_REASON, model.Reason);
            dbManager.AddParameters(5, PARM_COMMENTS, model.Comments);
            dbManager.AddParameters(6, PRAM_DURATION, model.Duration);
            dbManager.AddParameters(7, PARM_TIME, model.Time);
            dbManager.AddParameters(8, PARM_SCHEDULE_COUNT, model.ScheduleCount);
            dbManager.AddParameters(9, PARM_SCHEDULE_TYPE, model.ScheduleType);
            dbManager.AddParameters(10, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(11, PARM_CREATED_ON, model.CreatedOn);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(13, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(14, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(15, PARM_FOLLOWUP_TEXT, model.FollowUpText);
            dbManager.AddParameters(16, PARM_CREATE_APPOINTMENT, model.CreateAppointment);
            dbManager.AddParameters(17, PARM_APPOINTMENT_DATE, model.Date);
            dbManager.AddParameters(18, PARM_FROM_TIME, model.FromTime);
            dbManager.AddParameters(19, PARM_TO_TIME, model.ToTime);
            dbManager.AddParameters(20, PARM_FOLLOWUP_DAYS, model.FollowUpDays);
            dbManager.AddParameters(21, PARM_FOLLOWUP_MONTHS, model.FollowUpMonths);
            dbManager.AddParameters(22, PARM_FOLLOWUP_YEARS, model.FollowUpYears);
        }

        public string insertOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createOrderSetFollowUpParameters(dbManager, model, true);
                retunVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_FOLLOW_UP_INSERT));
                if (!string.IsNullOrEmpty(retunVal) && MDVUtility.ToLong(retunVal) <= 0)
                    throw new Exception(retunVal);

                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::insertOrderSetFollowUp", PROC_ORDER_SETS_FOLLOW_UP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createOrderSetFollowUpParameters(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_FOLLOW_UP_UPDATE));
                if (!string.IsNullOrEmpty(retunVal))
                    throw new Exception(retunVal);

                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::updateOrderSetFollowUp", PROC_ORDER_SETS_FOLLOW_UP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteOrderSetFollowUp(string FollowUpId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FOLLOWUP_ID, FollowUpId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORDER_SETS_FOLLOW_UP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetFollowUp", PROC_ORDER_SETS_FOLLOW_UP_DELETE, ex);
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

        public List<OrdertSetFollowUpModel> loadOrderSetFollowUp(long FollowUpId, long orderSetId, long pageNumber, long rowspPage)
        {
            List<OrdertSetFollowUpModel> listOrderSets = new List<OrdertSetFollowUpModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (FollowUpId == 0)
                    dbManager.AddParameters(0, PARM_FOLLOWUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FOLLOWUP_ID, FollowUpId);

                if (orderSetId == 0)
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ORDER_SET_ID, orderSetId);
                if (pageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowspPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowspPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SETS_FOLLOW_UP_SELECT);
                OrdertSetFollowUpModel model = null;
                while (reader.Read())
                {
                    model = new OrdertSetFollowUpModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.FollowUpId = !String.IsNullOrEmpty(reader["FollowUpId"].ToString()) ? reader["FollowUpId"].ToString() : "";
                    model.FacilityId = !String.IsNullOrEmpty(reader["FacilityId"].ToString()) ? reader["FacilityId"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.FacilityName = !String.IsNullOrEmpty(reader["FacilityName"].ToString()) ? reader["FacilityName"].ToString() : "";
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? reader["ProviderId"].ToString() : "";
                    model.Reason = !String.IsNullOrEmpty(reader["Reason"].ToString()) ? reader["Reason"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.Duration = !String.IsNullOrEmpty(reader["Duration"].ToString()) ? reader["Duration"].ToString() : "";
                    model.Time = !String.IsNullOrEmpty(reader["Time"].ToString()) ? MDVUtility.To12HrTime(reader["Time"].ToString()) : "";
                    model.ScheduleCount = !String.IsNullOrEmpty(reader["ScheduleCount"].ToString()) ? reader["ScheduleCount"].ToString() : "";
                    model.ScheduleType = !String.IsNullOrEmpty(reader["ScheduleType"].ToString()) ? reader["ScheduleType"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    model.FollowUpText = !String.IsNullOrEmpty(reader["FollowUpText"].ToString()) ? reader["FollowUpText"].ToString() : "";
                    model.ModifiedByName = !String.IsNullOrEmpty(reader["ModifiedByName"].ToString()) ? reader["ModifiedByName"].ToString() : "";

                    model.Date = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["AppointmentDate"])) ? reader["AppointmentDate"].ToString() : "";
                    model.CreateAppointment = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["CreateAppointment"])) ? reader["CreateAppointment"].ToString() : "";
                    model.FromTime = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["FromTime"])) ? reader["FromTime"].ToString() : "";
                    model.ToTime = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["ToTime"])) ? reader["ToTime"].ToString() : "";
                    model.IsDefault = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["IsDefault"])) ? reader["IsDefault"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(MDVUtility.ToStr(reader["RecordCount"])) ? reader["RecordCount"].ToString() : "";

                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadOrderSetFollowUp", PROC_ORDER_SETS_FOLLOW_UP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public FollowUpModel GetFollowUpByPatient(long FollowUpId, long orderSetId, long noteId, long pageNumber, long rowspPage)
        {
            FollowUpModel model = new FollowUpModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (FollowUpId == 0)
                    parameters.Add(new SqlParameter(PARM_FOLLOWUP_ID, DBNull.Value));               
                else
                    parameters.Add(new SqlParameter(PARM_FOLLOWUP_ID, FollowUpId));              

                if (orderSetId == 0)
                    parameters.Add(new SqlParameter(PARM_ORDER_SET_ID, DBNull.Value));                
                else
                    parameters.Add(new SqlParameter(PARM_ORDER_SET_ID, orderSetId));
                if (noteId == 0)
                    parameters.Add(new SqlParameter(PARM_NOTE_ID, DBNull.Value));
                else
                    parameters.Add(new SqlParameter(PARM_NOTE_ID, noteId));

                if (pageNumber <= 0)
                parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, DBNull.Value));
                else
                    parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, pageNumber));
                if (rowspPage <= 0)
                    parameters.Add(new SqlParameter(PARM_ROWSP_PAGE, DBNull.Value));
                else
                    parameters.Add(new SqlParameter(PARM_ROWSP_PAGE, rowspPage));

                parameters.Add(new SqlParameter(PARM_RECORD_COUNT,1));

                using (var reader = dbManager.ExecuteReader(PROC_ORDER_SETS_FOLLOW_UP_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(FollowUpModel).GetProperties();

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
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::GetFollowUpByPatient", PROC_ORDER_SETS_FOLLOW_UP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOrderSet attachOrderSetWithNote(long notesId, long patientId, string orderSetId, string OrderSetComponents, string ProblemIDs, string ProcedureIDs, string LabOrderIDs, string RadiologyOrderIDs, string FollowUpIDs, string PatientEducationIDs, string ReferralsIDs, string ImmunizationIDs, string TherapeuticIDs, string ProcedureOrderIDs, long ProviderId, bool AddInValidAgeRecordsInHxTab = false, string PatientProblemIds = "", string OrderSetAssociatedProblemIds = "")
        {
            DSOrderSet ds = new DSOrderSet();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(19);

                dbManager.AddParameters(0, PARM_NOTES_ID, notesId);
                dbManager.AddParameters(1, PARM_ORDER_SET_ID, orderSetId);
                dbManager.AddParameters(2, PARM_PATIENT_ID, patientId);

                if (string.IsNullOrEmpty(OrderSetComponents))
                    dbManager.AddParameters(3, PARM_ORDERSET_COMMONENTS, null);
                else
                    dbManager.AddParameters(3, PARM_ORDERSET_COMMONENTS, OrderSetComponents);
                dbManager.AddParameters(4, PARAM_USERID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(5, PARM_PROBLEMIDS, ProblemIDs);
                dbManager.AddParameters(6, PARM_PROCEDUREIDS, ProcedureIDs);
                dbManager.AddParameters(7, PARM_LABORDERIDS, LabOrderIDs);
                dbManager.AddParameters(8, PARM_RADIOLOGYORDERIDS, RadiologyOrderIDs);
                dbManager.AddParameters(9, PARM_IMMUNIZATIONIDS, ImmunizationIDs);
                dbManager.AddParameters(10, PARM_FOLLOWUPIDS, FollowUpIDs);
                dbManager.AddParameters(11, PARM_PATIENTEDUCATIONIDS, PatientEducationIDs);
                dbManager.AddParameters(12, PARM_REFERRALSIDS, ReferralsIDs);
                dbManager.AddParameters(13, PARAM_ADD_IN_VALID_AGE_RECORDS_IN_HXTAB, AddInValidAgeRecordsInHxTab);
                dbManager.AddParameters(14, PARM_THERAPEUTICIDS, TherapeuticIDs);
                dbManager.AddParameters(15, PARM_PROCEDUREORDERIDS, ProcedureOrderIDs);
                dbManager.AddParameters(16, PARM_PATIENTPROBLEM_IDS, PatientProblemIds);
                dbManager.AddParameters(17, PARM_ORDERSETASSOCIATEDPROBLEM_IDS, OrderSetAssociatedProblemIds);
                if (ProviderId == 0)
                    dbManager.AddParameters(18, PARM_PROVIDER_ID_NOTE, DBNull.Value);
                else
                    dbManager.AddParameters(18, PARM_PROVIDER_ID_NOTE, ProviderId);


                ds = (DSOrderSet)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDER_SETS_INSERT_TO_NOTES, ds, new List<string> {
                    ds.PatientDocumentSoap.TableName, ds.Referrals.TableName, ds.LabOrder.TableName, ds.LabOrderTest.TableName, ds.LabOrderProblem.TableName,
                    ds.OS_RadiologyOrder.TableName, ds.RadiologyOrderTest.TableName, ds.RadiologyOrderProblem.TableName, ds.ProcedureOrder.TableName,
                    ds.OS_VaccineHx.TableName, ds.OS_TherapeuticInjection.TableName, ds.ProcedureSoap.TableName, ds.OS_ProblemListSoap.TableName,
                    ds.FollowUp.TableName, ds.Appointment.TableName});
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::attachOrderSetWithNote", PROC_ORDER_SETS_INSERT_TO_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOrderSet deleteOrderSetfFromNote(long notesId, string orderSetId)
        {
            DSOrderSet ds = new DSOrderSet();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ORDER_SET_ID, orderSetId);
                dbManager.AddParameters(1, PARM_NOTES_ID, notesId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                ds = (DSOrderSet)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDER_SETS_DELETE_FROM_NOTES, ds, ds.OrderSetDelete.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteOrderSetfFromNote", PROC_ORDER_SETS_DELETE_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region " Note Order Set "

        private void createNoteOrderSetParameters(IDBManager dbManager, OS_NoteModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_NOTE_OS_ID, model.NoteOSId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_NOTE_OS_ID, model.NoteOSId);

            dbManager.AddParameters(1, PARM_NOTE_ID, model.NoteId);
            dbManager.AddParameters(2, PARM_CDS_ID, model.CDSId);

            if (IsInsert == true)
                dbManager.AddParameters(3, PARM_ORDER_SET_IDS, model.OrderSetId);
            else
                dbManager.AddParameters(3, PARM_ORDER_SET_ID, model.OrderSetId);

            dbManager.AddParameters(4, PARM_IS_DELETED, model.IsDeleted);
            dbManager.AddParameters(5, PARM_ORDERSET_COMMONENTS, model.OrderSetComponents);
            dbManager.AddParameters(6, PARAM_IS_DEFAULT_ORDER_SET, model.IsDefaultOrderSet);
        }

        public string insertNoteOrderSet(OS_NoteModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createNoteOrderSetParameters(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_ORDERSET_INSERT));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::insertNoteOrderSet", PROC_NOTE_ORDERSET_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateNoteOrderSet(OS_NoteModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createNoteOrderSetParameters(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_ORDERSET_UPDATE));
                if (retunVal == "FormName Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::updateNoteOrderSet", PROC_NOTE_ORDERSET_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteNoteOrderSet(string NoteOSId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTE_OS_ID, NoteOSId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTE_ORDERSET_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::deleteNoteOrderSet", PROC_NOTE_ORDERSET_DELETE, ex);
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

        public List<OS_NoteModel> loadNoteOrderSet(long NoteOSId)
        {
            List<OS_NoteModel> listOrderSets = new List<OS_NoteModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (NoteOSId == 0)
                    dbManager.AddParameters(0, PARM_NOTE_OS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_OS_ID, NoteOSId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_NOTE_ORDERSET_SELECT);
                OS_NoteModel model = null;
                while (reader.Read())
                {
                    model = new OS_NoteModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.NoteOSId = !String.IsNullOrEmpty(reader["NoteOSId"].ToString()) ? reader["NoteOSId"].ToString() : "";
                    model.NoteId = !String.IsNullOrEmpty(reader["NoteId"].ToString()) ? reader["NoteId"].ToString() : "";
                    model.CDSId = !String.IsNullOrEmpty(reader["CDSId"].ToString()) ? reader["CDSId"].ToString() : "";
                    model.IsDeleted = !String.IsNullOrEmpty(reader["IsDeleted"].ToString()) ? reader["IsDeleted"].ToString() : "";
                    model.OrderSetComponents = !String.IsNullOrEmpty(reader["OrderSetComponents"].ToString()) ? reader["OrderSetComponents"].ToString() : "";

                    listOrderSets.Add(model);
                }
                return listOrderSets;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOrderSet::loadNoteOrderSet", PROC_NOTE_ORDERSET_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public DSOrderSet lookupOrderSet()
        {
            DSOrderSet ds = new DSOrderSet();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSOrderSet)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ORDER_SET_LOOKUP, ds, ds.OrderSetLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::lookupOrderSet", PROC_ORDER_SET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<OrderSetModel> LookupOrderSetTemplate( DataTable dtProvider, string TemplateID)
        {
            List<OrderSetModel> listOrderSetTemplate = new List<OrderSetModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROVIDER_IDS, dtProvider);
                dbManager.AddParameters(1, PARM_ORDER_SET_BY_TEMPLATE_ID, TemplateID);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SET_PROVIDERS_LOOKUP);
                OrderSetModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetName = !String.IsNullOrEmpty(reader["OrderSetName"].ToString()) ? reader["OrderSetName"].ToString() : "";
                    listOrderSetTemplate.Add(model);
                }
                return listOrderSetTemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupOrderSetTemplate", PROC_ORDER_SET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<OrderSetModel> LookupOrderSetByTemplateID(string TemplateID)
        {

            List<OrderSetModel> listOrderSetTemplate = new List<OrderSetModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ORDER_SET_BY_TEMPLATE_ID, TemplateID);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ORDER_SET_TEMPLATE_LOOKUP);
                OrderSetModel model = null;
                while (reader.Read())
                {
                    model = new OrderSetModel();
                    model.OrderSetId = !String.IsNullOrEmpty(reader["OrderSetId"].ToString()) ? reader["OrderSetId"].ToString() : "";
                    model.OrderSetName = !String.IsNullOrEmpty(reader["OrderSetName"].ToString()) ? reader["OrderSetName"].ToString() : "";
                    listOrderSetTemplate.Add(model);
                }
                return listOrderSetTemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::LookupOrderSetByTemplateID", PROC_ORDER_SET_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
    }
}


