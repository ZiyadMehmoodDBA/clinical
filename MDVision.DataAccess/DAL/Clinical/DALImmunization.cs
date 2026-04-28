using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Immunization;
using MDVision.Common.Utilities;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALImmunization
    {

        #region Variable

        #endregion

        #region "Stored Procedure Names"

        //private const string PROC_Immunization_INSERT = "Clinical.sp_ImmunizationInsert";
        //private const string PROC_Immunization_UPDATE = "Clinical.sp_ImmunizationUpdate";
        //private const string PROC_Immunization_DELETE = "Clinical.sp_ImmunizationDelete";
        //   private const string PROC_ImmunizationHx_SELECT = "Clinical.sp_ImmunizationSelect";
        private const string PROC_ImmunizationHx_SELECT = "Clinical.sp_VaccineHxSELECT"; //"dbo.sp_testImmunization";

        private const string PROC_Therapeutic_Injection_SELECT = "Clinical.sp_ImmunizationTherapeuticInjectionSelect";

        private const string PROC_VACCINE_GROUP_LOOKUP = "Clinical.sp_VaccineGroupLookup";//"Clinical.sp_GetVaccineGroup";
        private const string PROC_ALERT_TYPE_LOOKUP = "Clinical.sp_ImmunizationAlertLookup";

        private const string PROC_SYSTEM_VACCINE_GROUP = "Clinical.sp_SystemVaccineGroup";//"Clinical.sp_GetVaccineGroup";
        private const string PROC_VACCINE_LOOKUP = "Clinical.sp_VaccineLookup";
        private const string PROC_VACCINE_ROUTE_LOOKUP = "Clinical.sp_VaccineRouteLookup";
        private const string PROC_VACCINE_FUNDING_SOURCE_LOOKUP = "Clinical.sp_VaccineFundingSourceLookup";
        private const string PROC_THERAPEUTIC_INJECTION_LOOKUP = "Clinical.sp_TherapeuticInjectionLookup";
        private const string PROC_GET_CATEGORY_AGAINTS_SCH_AND_SCHTYPE = "Clinical.sp_GetCategoryAgaintsSchAndSchtype";


        private const string PROC_VACCINE_SITE_LOOKUP = "Clinical.sp_VaccineSiteLookup";
        private const string PROC_VACCINE_REACTION_LOOKUP = "Clinical.sp_VaccineReactionLookup";

        private const string PROC_VACCINE_MANUFACTURER_LOOKUP = "Clinical.sp_VaccineManufacturerLookup";
        private const string PROC_VACCINE_VFC_LOOKUP = "Clinical.sp_VaccineVFCLookup";
        private const string PROC_VACCINE_SOURCE_OF_HX_LOOKUP = "Clinical.sp_VaccineSourceOfHxLookup";
        private const string PROC_VACCINE_GIVEN_BY_LOOKUP = "Clinical.sp_VaccineGivenByLookup";

        private const string PROC_VACCINE_LOAD = "Clinical.sp_GetVaccine";

        private const string PROC_ImmunizationHx_INSERT = "Clinical.sp_VaccineHxInsert";
        private const string PROC_INSERT_VACCINEHX = "[Clinical].[sp_InsertVaccineHx]";
        private const string PROC_UPDATE_VACCINEHX = "[Clinical].[sp_UpdateVaccineHx]";
        private const string PROC_THERAPEUTIC_INJECTION_INSERT = "Clinical.sp_ImmunizationTherapeuticInjectionInsert";

        private const string PROC_THERAPEUTIC_INJECTION_UPDATE = "Clinical.sp_ImmunizationTherapeuticInjectionUpdate";



        private const string PROC_ImmunizationHx_LOAD = "Clinical.sp_VaccineHxLoad";
        private const string PROC_ImmunizationHx_UPDATE = "Clinical.sp_VaccineHxUpdate";

        private const string PROC_GET_VACCINEHX_VISDATE_AND_URL = "Clinical.sp_GetVaccineVISDateAndUrl";
        private const string PROC_GET_VACCINEHX_VISDATE = "Clinical.sp_GetVaccineVISDate";
        private const string PROC_GET_VACCINEHX_VIS_URL = "Clinical.sp_GetVaccineVISURL";
        private const string PROC_VACCINEHx_SELECT = "Clinical.sp_GetVaccineHx";
        private const string PROC_VACCINEHX_SELECT_FORSOAPTEXT = "Clinical.sp_GetVaccineHxForSoapText";
        private const string PROC_CPT_CODEANDADMINISTEREDCODE_SELECT = "Clinical.sp_GetCptCodeAndAdministeredCode";
        private const string PROC_GET_THERAPEUT_ICINJECTION_SELECT = "Clinical.sp_GetTherapeuticInjection";
        private const string PROC_GET_THERAPEUT_ICINJECTION_SELECT_FORSOAPTEXT = "Clinical.sp_GetTherapeuticInjectionForSoapText";

        private const string PROC_ImmunizationHx_SELECT_For_FaceSheet = "Clinical.sp_VaccineFaceSheetSelect";
        private const string PROC_ATTACH_VACCINE_FROM_NOTES = "Clinical.sp_AttachVaccineWithNotes";
        private const string PROC_DETACH_VACCINE_FROM_NOTES = "Clinical.sp_DetachVaccineFromNotes";


        private const string PROC_ATTACH_THER_INJECTION_FROM_NOTES = "Clinical.sp_AttachTherapeuticInjectionWithNotes";
        private const string PROC_DETACH_THER_INJECTION_FROM_NOTES = "Clinical.sp_DetachTherapeuticInjectionFromNotes";



        private const string PROC_GENERATE_HL7_IMMUNIZATION_MESSAGE = "Clinical.sp_Generate_HL7Immunization_Message";



        private const string PROC_SEARCH_ALL_VACCINE = "Clinical.sp_SearchAllVaccines";
        private const string PROC_SEARCH_ALL_MANUFACTURER = "Clinical.sp_SearchAllManufacturer";

        private const string PROC_CATEGORY_SELECT = "Clinical.sp_VaccineCategorySelect";
        private const string PROC_CATEGORY_INSERT = "Clinical.sp_VaccineCategoryInsert";
        private const string PROC_CATEGORY_UPDATE = "Clinical.sp_VaccineCategoryUpdate";
        private const string PROC_CATEGORY_DELETE = "Clinical.sp_VaccineCategoryDelete";
        private const string PROC_GET_PATIENT_PROVIDER_ID = "Clinical.sp_GetPatientProviderId";
        private const string PROC_IS_LAST_ADMINISTERED_DOES = "Clinical.sp_IsLastAdministeredDoes";
        private const string PROC_IS_ADMINISTERED_PERIOD_OVER = "Clinical.sp_IsAdministrationPeriodOver";


        private const string PROC_GET_CPT_OF_VACCINE = "Clinical.sp_GetCptOfVaccine";
        private const string PROC_VACCINE_SCHEDULE_ID = "Clinical.sp_GetVaccineScheduleId";
        private const string PROC_VACCINE_HX_IDS = "Clinical.sp_GetVaccineHxIds";
        private const string PROC_GET_PROCEDUREIDS_AGAINST_VACCANDIMM = "Clinical.sp_ProcedureIdsAgainstVaccAndImm";





        private const string PROC_CHECK_PATIENT_INSURANCE_IS_MEDICARE = "Clinical.sp_CheckPatientInsuranceIsMedicare";
        private const string PROC_GET_IMMUNIZATION_ALERT_COUNT = "Clinical.sp_GetPatientImmunizationAlertCount";
        private const string PROC_GET_PROCEDUREID_AGAINST_IMMTHERAPEUTICINJECTIONID = "Clinical.sp_GetProcedureIdAgainstImmTherapeuticInjecId";

        private const string PROC_LOAD_VACCINE_SCHEDULER = "Clinical.sp_VaccineScheduleChartSelect";
        private const string PROC_LOAD_VACCINE_SCHEDULER_PREVIEW = "Clinical.sp_VaccineScheduleChartForPreviewSelect";





        private const string PROC_VACCINE_CROSSWALK_SELECT = "Clinical.sp_VaccineCrosswalkSelect";
        private const string PROC_VACCINE_CROSSWALK_INSERT = "Clinical.sp_VaccineCrosswalkInsert";
        private const string PROC_VACCINE_CROSSWALK_UPDATE = "Clinical.sp_VaccineCrosswalkUpdate";
        private const string PROC_VACCINE_CROSSWALK_DELETE = "Clinical.sp_VaccineCrosswalkDelete";
        private const string PROC_VACCINE_LOT_NUMBER_LOOKUP = "Clinical.sp_VaccineLotNumberLookup";
        private const string PROC_THERAPUETIC_INJECTION_LOT_NUMBER_LOOKUP = "Clinical.sp_TherapueticInjecLotNumberLookup";

        private const string PROC_VACCINE_PUBLICITY_CODE_LOOKUP = "Clinical.sp_VaccinePublicityCodeLookup";
        private const string PROC_VACCINE_REGISTRY_STATUS_LOOKUP = "Clinical.sp_VaccineRegistryStatusLookup";
        private const string PROC_VACCINE_REFUSAL_REASON_LOOKUP = "Clinical.sp_VaccineRefusalReasonLookup";
        private const string PROC_VACCINE_SELECT = "Clinical.sp_VaccinesSelect";
        private const string PROC_SCHEDULE_LOOKUP = "Clinical.sp_ScheduleLookup";
        private const string PROC_SCHEDULE_TYPE_LOOKUP = "Clinical.sp_ScheduleTypeLookup";
        private const string PROC_VACCINE_SCHEDULE_SELECT = "Clinical.sp_VaccineScheduleSelect";
        private const string PROC_VACCINE_SCHEDULE_SELECT_FOR_SORT = "Clinical.sp_VaccineScheduleSelectForSort";

        private const string PROC_VACCINE_SCHEDULE_INSERT = "Clinical.sp_VaccineScheduleInsert";
        private const string PROC_VACCINE_SCHEDULE_UPDATE = "Clinical.sp_VaccineScheduleUpdate";
        private const string PROC_VACCINE_SCHEDULE_UPDATE_FOR_SORT = "Clinical.sp_VaccineScheduleUpdate_For_Sort";


        private const string PROC_VACCINE_SCHEDULE_DELETE = "Clinical.sp_VaccineScheduleDelete";

        private const string PROC_IMMUNIZATION_LOT_NUMBER_SELECT = "Clinical.sp_LotNumberSelect";
        private const string PROC_IMMUNIZATION_LOT_NUMBER_INSERT = "Clinical.sp_LotNumberInsert";
        private const string PROC_IMMUNIZATION_LOT_NUMBER_UPDATE = "Clinical.sp_LotNumberUpdate";
        private const string PROC_IMMUNIZATION_LOT_NUMBER_DELETE = "Clinical.sp_LotNumberDelete";
        private const string PROC_MANUFACTURER_LOOKUP = "Clinical.sp_ManufacturerLookup";

        private const string PROC_SELECT_IMMUNIZATION_FOR_PREVIEW = "Clinical.sp_ImmunizationForPrint";
        private const string PROC_IMMUNIZATION_ALERT_SELECT = "Clinical.sp_PatientImmunizationAlertSelect";
        private const string PROC_IMMUNIZATION_ALERT_SELECT_FOR_PRINT = "Clinical.sp_PatientImmunizationAlertSelectForPrint";
        private const string PROC_PATIENT_IMMUNIZATION_ALERT_INSERT = "Clinical.sp_PatientImmunizationAlertInsert";

        private const string PROC_IMMUNIZATION_LOOKUP_FOR_REPORTS = "Clinical.sp_ImmunizationLookupForReports";
        private const string PROC_VACCINE_LOOKUP_FOR_REPORTS = "Clinical.sp_VaccineLookupForReports";
        private const string PROC_GET_LOT_MANUFACTUREID = "Clinical.sp_GetLotManufactureId";
        private const string PROC_WHY_LOT_IS_NOT_AVAILABLE = "Clinical.Sp_WhyLotIsNotAvailable";
        //Add vaccine and therapeutic
        private const string PROC_GET_VACCINE_OR_THERA_NAME = "Clinical.sp_GetImmOrTherName";
        private const string PROC_LOAD_VACCINE_OR_THERA = "Clinical.sp_GetVaccineAndTherapeutic";
        private const string PROC_VACCINE_INSERT = "Clinical.sp_VaccineInsert";
        private const string PROC_VACCINE_UPDATE = "Clinical.sp_VaccineUpdate";
        private const string PROC_LOAD_VACCINE_DETAIL = "Clinical.sp_LoadVaccineDetail";
        private const string PROC_VACCINE_VIS_INSERT = "Clinical.sp_VaccineVisInsert";
        private const string PROC_VACCINE_VIS_AND_URL_UPDATE = "Clinical.sp_VaccineVisAnd_Url_Insert";
        private const string PROC_IMMUNIZATION_DELETE = "Clinical.sp_deleteVaccine";
        private const string PROC_THERAPEUTIC_INSERT = "Clinical.sp_TherapeuticInsert";
        private const string PROC_LOAD_THERAPEUTIC_DETAIL = "Clinical.sp_LoadTherapeuticDetail";
        private const string PROC_THERAPEUTIC_UPDATE = "Clinical.sp_TherapeuticUpdate";
        private const string PROC_THERAPEUTIC_DELETE = "Clinical.sp_deleteTherapeutic";
        private const string PROC_LOAD_VACCINE_INFO_FOR_AUTO_POPULATION = "Clinical.sp_GetVaccineInfoForAutoPopulation";
        private const string PROC_ACTIVEINACTIVE_IMMORTHERA = "Clinical.sp_ActiceInactiveImmOrThera";
        private const string PROC_GET_MANUFACTURER_NAME = "Clinical.sp_GetManufacturerName";
        private const string PROC_LOAD_MANUFACTURER = "Clinical.sp_LoadManufacturer";
        private const string PROC_MANUFACTURER_INSERT = "Clinical.sp_ManufacturerInsert";
        private const string PROC_LOAD_MANUFACTURER_DETAIL = "Clinical.sp_LoadManufacturerDetail";
        private const string PROC_MANUFACTURER_UPDATE = "Clinical.sp_ManufacturerUpdate";
        private const string PROC_MANUFACTURER_DELETE = "Clinical.sp_deleteManufacturer";
        private const string PROC_ACTIVEINACTIVE_MANUFACTURER = "Clinical.sp_ActiceInactiveManufacturer";
        private const string PROC_VACCINE_AMOUNT_LOOKUP = "Clinical.sp_VaccineAmountLookup";
        private const string PROC_VIS_INFORMATION_DELETE = "Clinical.sp_deleteVaccineVISInfo";
        private const string PROC_NOTES_IMMUNIZATION_SELECT = "[Clinical].[sp_NotesImmunizationSelect]";
        private const string PROC_RECEIVING_APPLICATION_LOOKUP = "Provider.ReceivingApplicationLookup";
        private const string PROC_REGISTERY_SUBMISSION_LOOKUP = "Provider.RegistrySubmissionLookup";
        private const string PROC_REGISTERY_CONFIGURATION_SELECT = "Provider.sp_RegistryConfigurationSelect";
        private const string PROC_REGISTERY_CONFIGURATION_INSERT = "Provider.sp_RegistryConfigurationInsert";
        private const string PROC_REGISTERY_CONFIGURATION_UPDATE = "Provider.sp_RegistryConfigurationUpdate";
        private const string PROC_REGISTERY_CONFIGURATION_DELETE = "Provider.sp_RegistryConfigurationDelete";
        private const string PROC_QUERY_IMMUNIZATION_INSERT = "Clinical.sp_QueryImmunizationInsert";
        private const string PROC_UPDATE_HL7_MESSAGE = "Clinical.sp_UpdateHL7Message";
        private const string PROC_IMM_QUERY_SELECT = "Clinical.sp_ImmunizationQuerySelect";
        private const string PROC_IMMUNIZATION_QUERY_RESPONSE_INSERT = "Clinical.ImmunizationQueryResponseinsert";
        private const string PROC_IMM_QUERY_RESPONSE_SELECT = "Clinical.ImmunizationQueryResponseSelect";
        private const string PROC_IMM_QUERY_RESPONSE_PATIENT_SELECT = "Clinical.ImmunizationQueryResponsePatientSelect";
        private const string PROC_IMM_QUERY_RESPONSE_DELETE = "Clinical.sp_ImmunizationQueryResponseDelete";
        private const string PROC_IMM_QUERY_RESPONSE_HX_SELECT = "Clinical.sp_ImmunizationQueryResponseHXSelect";
        private const string PROC_IMM_QUERY_RESPONSE_FORECAST_SELECT = "Clinical.sp_ImmunizationQueryResponseForecastSelect";
        private const string PROC_ADD_EVALIMMU_HX_TO_PATIENT_VACC = "Clinical.sp_AddEvalImmuHxToPatientVacc";
        private const string PROC_IMM_QUERY_DELETE = "Clinical.sp_ImmunizationQueryDelete";
        private const string PROC_IMM_ACKNOW_SAVE = "Clinical.ImmAcknowSave";
        private const string PROC_VACCINEHX_SELECT_BY_ID = "Clinical.sp_VaccineHxSelectById";
        private const string PROC_GET_ENCOUNTER_TYPE_ID = "Clinical.sp_EncounterLookup";
        private const string PROC_GET_VACCINE_SHEDULEID = "Clinical.sp_GetVaccineScheduleIdForTreatment";
        #endregion "Stored Procedure Names"

        #region "Parameters"


        private const string PARM_IS_VACCINE_INSERT = "@VaccineInsert";

        private const string PARM_ALERT_COUNT = "@AlertCount";
        private const string PARAM_IMMUNIZATION_IDS = "@ImmunizationIds";
        private const string PARM_VACCINE_GROUP_ID = "@vaccineGroupId";
        private const string PARM_VACCINE_FOR_MODULE = "@forModule";

        //private const string PARAM_ADMINISTER_VACCINE_ID = "@AdministerVaccineId";
        private const string PARAM_VACCINE_HX_ID = "@VaccineHxId";
        private const string PARAM_IMM_THER_INJECTION_ID = "@ImmTherInjectionId";
        private const string PARM_PROCDURE_ID = "@ProcedureId";
        private const string PARM_PROCDURE_IDS = "@ProcedureIds";



        private const string PARAM_THERAPEUTIC_INJECTION_ID = "@TherapeuticInjectionId";



        private const string PARAM_VACCINE_HX_IDS = "@VaccineHxIds";
        private const string PARAM_USERID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";


        private const string PARAM_VACCINE_GROUP_CATEGORY = "@VaccineGroupCategory";
        private const string PARAM_VACCINE = "@Vaccine";
        private const string PARAM_VACCINE_ID = "@VaccineId";
        private const string PARAM_LOT_NUMBER = "@LotNumber";
        private const string PARAM_ROUTE = "@Route";
        private const string PARAM_ROUTE_ID = "@RouteId";
        private const string PARAM_VFC = "@VFC";
        private const string PARAM_COMMENTS = "@Comments";
        private const string PARAM_VISIT_DATE = "@VisitDate";
        private const string PARAM_ADMINISTRATION_DATE = "@AdministrationDate";
        private const string PARAM_DOSE = "@Dose";
        private const string PARAM_AMOUNT = "@Amount";
        private const string PARAM_SITE = "@Site";
        private const string PARAM_SITE_ID = "@SiteId";
        private const string PARAM_VISDATE = "@VISDate";
        private const string PARAM_GIVEN_BY = "@GivenBy";
        private const string PARAM_MANUFACTURER = "@Manufacturer";
        private const string PARAM_EXPIRY_DATE = "@ExpiryDate";
        private const string PARM_VACCINE_CATEGORY_ID = "@CategoryId";
        private const string PARAM_SCHEDULE_SHORT_NAME = "@ScheduleShortName";

        private const string PARAM_TAB_ID = "@TabId";
        private const string PARAM_REACTION = "@ReactionId";
        private const string PARAM_VISIT_DATE_ID = "@VisitDateId";
        private const string PARAM_PATIENT_AGE = "@PatientAge";
        private const string PARAM_OVERRIDE_RULE = "@OverrideRule";

        //private const string PARAM_DOCUMENT_HX_ID = "@DocumentHxDoseId";
        private const string PARAM_SOURCE_OF_HX = "@SourceOfHx";
        private const string PARAM_SCHEDULER_ID = "@SchedulerId";
        private const string PARAM_REFUSAL_REASON_ID = "@RefusalReasonId";

        private const string PARAM_TYPE = "@Type";
        private const string PARAM_CVX_CODE = "@CVXCode";

        private const string PARAM_PATIENT_ID = "@PatientId";
        private const string PARAM_IS_ACITVE = "@ISActive";

        private const string PARAM_SEARCH_TEXT = "@SearchText";

        private const string PARAM_ENTITY_ID = "@EntityId";

        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
        private const string PARAM_CREATED_BY = "@CreatedBy";
        private const string PARAM_CREATED_ON = "@CreatedOn";
        private const string PARAM_MODIFIED_ON = "@ModifiedOn";
        private const string PARAM_MODIFIED_BY = "@ModifiedBy";
        private const string PARAM_PROVIDER_ID = "@ProviderId";
        private const string PARAM_COMPLETION_STATUS_CODE = "@CompletionStatusCode";
        private const string PARAM_REFUSAL_REASON_CODE = "@RefusalReasonCode";
        private const string PARAM_PUBLICITY_CODE = "@PublicityCode";
        private const string PARAM_PUBLICITY_CODE_EXPIRY_DATE = "@PublicityCodeExpiryDate";
        private const string PARAM_IMMUNIZATION_REGISTRY_STATUS_CODE = "@ImmunizationRegistryStatusCode";
        private const string PARAM_IRS_EFFECTIVE_DATE = "@IRSEffectiveDate";
        private const string PARAM_PROTECTION_INDICATOR = "@ProtectionIndicator";
        private const string PARAM_PI_EFFECTIVE_DATE = "@PIEffectiveDate";
        //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields


        //private const string PARM_Immunization_ID = "@ImmunizationId";
        //private const string PARM_ALLERGEN = "@Allergen";
        private const string PARM_TYPE = "@Type";
        //private const string PARM_REACTION = "@Reaction";
        //private const string PARM_SEVERITY = "@Severity";
        //private const string PARM_ONSET_DATE = "@OnSetDate";
        //private const string PARM_LAST_MODIFIED = "@LastModified";
        //private const string PARM_COMMENTS = "@Comments";
        private const string PARM_NOTE_ID = "@NotesId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        //private const string PARM_INACTIVE_CHECKBOXVALUE = "@InActiveCheckBoxValue";
        //private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        //private const string PARM_IS_HISTORY = "@IsHistory";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_VACCINE_HX_ID = "@vaccineHxId";
        private const string PARM_VACCINE_HX_IDS = "@vaccineHxIds";

        //private const string PARM_VISIT_ID = "@VisitId";
        //private const string PARM_MODIFIED_BY = "@ModifiedBy";
        //private const string PARM_CREATED_BY = "@CreatedBy";

        private const string PARM_PAGE_NUMBER = "@PageNo";
        private const string PARM_ROWSP_PAGE = "@RowsPerPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_VACCINE_ID = "@VaccineId";
        private const string PARM_CPT = "@CPT";



        //private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_ISDELETED = "@IsDeleted";
        //private const string PARM_ImmunizationLastUpdateDate = "@ImmunizationLastUpdateDate";
        //private const string PARM_R_PLASTUPDATEINFO = "@R_PLastUpdateInfo";

        //private const string PARM_MODIFY_ON = "@ModifiedOn";
        //private const string PARM_CREATED_ON = "@CreatedOn";

        //private const string PARM_Immunization_REVIEW_ID = "@ImmunizationReviewId";
        //private const string PARM_REVIEWED_BY = "@ReviewedBy";
        //private const string PARM_REVIEWED_ON = "@ReviewedOn";

        private const string PARM_SEARCH_TEXT = "@SearchText";

        private const string PARM_PAGE_NUM = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARAM_VACCINE_MANUFACTURER_ID = "@ManufacturerId";
        private const string PARAM_AGE_LIMIT = "@AgeLimit";

        private const string PARAM_SHORTNAME = "@ShortName";
        //private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARAM_VACCINE_SCHEDULE_Id = "@VaccineScheduleId";
        private const string PARM_IS_OVER = "@IsOver";

        private const string PARAM_VACCINE_PRIORITY = "@Priority";

        private const string PARAM_VACCINE_CROSSWALK_ID = "@VaccineCrosswalkId";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_VACCINE_STATUS = "@VaccineStatus";
        private const string PARM_CVX_SHORT_DESCRIPTION = "@CVXShortDescription";
        private const string PARAM_VOID_DOES = "@VoidDoes";
        private const string PARAM_SCHEDULE_TYPE_ID = "@ScheduleTypeId";

        private const string PARAM_START_DUE_DATE = "@StartDueDate";
        private const string PARAM_END_OVERDUE_DATE = "@EndOverdueDate";
        private const string PARAM_MALE_MAXAGE = "@MaleMaxage";
        private const string PARAM_FEMALE_MAXAGE = "@FemaleMaxage";

        private const string PARAM_CATEGORY = "@Category";
        private const string PARAM_SCHEDULE_ID = "@ScheduleId";




        private const string PARAM_LOT_NUMBER_ID = "@VaccineLotNoId";
        private const string PARAM_LOT_NO = "@LotNo";

        private const string PARAM_NDC_CODE = "@NdcCode";
        private const string PARAM_QUANTITY = "@Quantity";
        private const string PARAM_QUANTITY_LEFT = "@QuantityLeft";
        private const string PARAM_ACTIVE = "@Active";
        private const string PARM_NOTES_ID = "@NoteId";
        private const string PARAM_PROVIDER_IDS = "@ProviderIds";
        private const string PARAM_FUNDING_SOURCE_ID = "@VaccineFundingSourceId";
        private const string PARM_LOTISNOTAVAILABLE = "@LotIsNotAvailable";
        private const string PARM_ID = "@Id";
        private const string PARAM_ONLY_EXPIRED = "@OnlyExpired";
        private const string PARAM_ONLY_LOWQUANTITY = "@OnlyLowQuantity";
        private const string PARAM_ORDERSET_ID = "@OrderSetId";
        //Add vaccine and therpeutic
        private const string PARM_IMMUNIZATION_NAME = "@ImmunizationName";
        private const string PARM_STATUS = "@Status";
        private const string PARM_CVX = "@CVX";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_DESCRIPTION = "@CPTDescription";
        private const string PARM_ADMIN_CODE = "@AdminCode";
        private const string PARM_ADMIN_CODE_DESCRIPTION = "@AdminCodeDescription";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_AMOUNT = "@Amount";
        private const string PARM_MANUFACTUREIDS = "@ManufactureIds";
        private const string PARM_VIS_DATE = "@VISDate";
        private const string PARM_DOCUMENT_LINK = "@DocumentLink";
        private const string PARM_VIS_FULLY_ENCODED_TEXT = "@VIS_FullyEncodedtextstring";
        private const string PARM_DOCUMENT_Name = "@DocumentName";
        private const string PARM_VACCINE_VIS_ID = "@VaccineVisId";
        private const string PARM_VACCINE_VIS_URL_ID = "@VaccineVIS_URLId";
        private const string PARM_NDC_CODE = "@NDCCode";
        private const string PARM_THERAPEUTIC_ID = "@TherapeuticId";
        private const string PARM_THER_INJ_NAME = "@TherInjName";
        private const string PARM_MANUFACTURER_NAME = "@ManufacturerName";
        private const string PARM_MVX_CODE = "@MVXCode";
        private const string PARM_MANUFACTURER_ID = "@ManufacturerId";
        private const string PARM_CPT_BASE_SEARCH = "@CptBaseSearch";
        private const string PARM_IMMUNIZATION_Id = "@ImmunizationId";

        private const string PARAM_SENDING_APPLICATION = "@SendingApplication";
        private const string PARAM_PROVIDER_FACILITY_ID = "@PoviderFacilityId";
        private const string PARAM_SENDING_FACILITY = "@SendingFacility";
        private const string PARAM_FACILITY_ID = "@FacilityId";
        private const string PARAM_REGISTERY_SUBMISSION_ID = "@RegistrySubmissionId";
        private const string PARAM_TIME_SLOT = "@Timeslot";
        private const string PARAM_IS_ADMINISTERED = "@IsAdministered";
        private const string PARAM_IS_REFUSAL = "@IsRefusal";
        private const string PARAM_IS_HISTORY_DOSE = "@IsHistoryDose";
        private const string PARAM_FILES_PER_BATCH = "@FilesPerBatch";
        private const string PARAM_REGISTRY_CONFIGURATION_ID = "@RegistryConfigurationId";
        private const string PARAM_RECEIVING_APPLICATION_ID = "@ReceivingApplicationId";


        private const string PARM_PNOTESID = "@pNotesId";
        private const string PARM_PENTITYID = "@pEntityId";
        private const string PARM_PENTITYREGCODE = "@pEntityRegCode";
        private const string PARM_PFAVLISTNAMES = "@pFavListNames";
        private const string PARM_PFAVLISTVAL = "@pFavListVal";
        private const string PARM_PVACCINE_INSERT = "@pVaccineInsert";
        private const string PARM_PTABID = "@pTabId";
        private const string PARM_PPREVIOUS_VOID = "@pPreviousVoid";
        private const string PARM_PISADD_PROCEDURES = "@pIsAddProcedures";
        private const string PARM_USER_NAME = "@pUserName";
        private const string PARAM_VACCINE_IDS = "@LotVaccineIds";
        private const string PARAM_THERAPEUTIC_INJECTION_IDS = "@TherapeuticInjectionIds";
        private const string PARM_QUERY_ID = "@QueryId";
        private const string PARM_REQUEST_DATE_TIME = "@RequestDateTime";
        private const string PARM_GIVENBY = "@GivenBy";
        private const string PARM_HL7_MESSAGE = "@HL7Message";

        private const string PARM_IMMUNIZATION_QUERY_RESPONSE_ID = "@ImmunizationQueryResponseId";
        private const string PARM_FILE = "@File";
        private const string PARM_PATIENT_NAME = "@PatientName";
        private const string PARM_DOB = "@DOB";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_MOTHERS_MAIDEN_NAME = "@MothersMaidenName";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_XML = "@XML";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_EVALUATED_IMMUNIZATION_HISTORY_IDS = "@EvaluatedImmunizationHistoryIds";
        private const string PARM_RESPONSE_TYPE = "@ResponseType";
        private const string PARM_ACKNOWLEDGEMENT_CODE = "@AcknowledgementCode";
        private const string PARM_ACKNOWLEDGEMENT_FILE = "@AcknowledgementFile";
        private const string PARM_MAIN_AGE_GROUP = "@MainAge";
        private const string PARM_MAIN_SCHEDULE_GROUP = "@MainSchedule";
        private const string PARM_MAIN_CATEGORY_GROUP = "@MainCategory";        
        #endregion

        #region Constructors

        public DALImmunization()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALImmunization(SharedVariable SharedVariable)
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

        #region "Support Functions For Immunization"

        /// <summary>
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function for Create Parameters For Category
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        /// 

        private void createSendQueryParameters(IDBManager dbManager, ImmunizationQueryModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_QUERY_ID, model.QueryId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_QUERY_ID, model.QueryId, DbType.Int64);
            if (!string.IsNullOrEmpty(model.PatientId))
            {
                dbManager.AddParameters(PARM_PATIENT_ID, model.PatientId);
            }
            else
            {
                dbManager.AddParameters(PARM_PATIENT_ID, null);
            }
            if (!string.IsNullOrEmpty(model.RequestDateTime))
            {
                dbManager.AddParameters(PARM_REQUEST_DATE_TIME, model.RequestDateTime);
            }
            else
            {
                dbManager.AddParameters(PARM_REQUEST_DATE_TIME, null);
            }

            if (!string.IsNullOrEmpty(model.GivenBy))
            {
                dbManager.AddParameters(PARM_GIVENBY, model.GivenBy);
            }
            else
            {
                dbManager.AddParameters(PARM_GIVENBY, null);
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                dbManager.AddParameters(PARM_STATUS, model.Status);
            }
            else
            {
                dbManager.AddParameters(PARM_STATUS, null);
            }


            if (!string.IsNullOrEmpty(model.HL7Message))
            {
                dbManager.AddParameters(PARM_HL7_MESSAGE, model.HL7Message);
            }
            else
            {
                dbManager.AddParameters(PARM_HL7_MESSAGE, null);
            }
        }



        private void createSaveResponseParameters(IDBManager dbManager, ImmunizationQueryResponseModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, model.ImmunizationQueryResponseId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, model.ImmunizationQueryResponseId, DbType.Int64);
            if (!string.IsNullOrEmpty(model.PatientId))
            {
                dbManager.AddParameters(PARM_PATIENT_ID, model.PatientId);
            }
            else
            {
                dbManager.AddParameters(PARM_PATIENT_ID, null);
            }
            if (!string.IsNullOrEmpty(model.File))
            {
                dbManager.AddParameters(PARM_FILE, model.File);
            }
            else
            {
                dbManager.AddParameters(PARM_FILE, null);
            }
            if (!string.IsNullOrEmpty(model.PatientName))
            {
                dbManager.AddParameters(PARM_PATIENT_NAME, model.PatientName);
            }
            else
            {
                dbManager.AddParameters(PARM_PATIENT_NAME, null);
            }

            if (!string.IsNullOrEmpty(model.DOB))
            {
                dbManager.AddParameters(PARM_DOB, model.DOB);
            }
            else
            {
                dbManager.AddParameters(PARM_DOB, null);
            }

            if (!string.IsNullOrEmpty(model.Gender))
            {
                dbManager.AddParameters(PARM_GENDER, model.Gender);
            }
            else
            {
                dbManager.AddParameters(PARM_GENDER, null);
            }


            if (!string.IsNullOrEmpty(model.MothersMaidenName))
            {
                dbManager.AddParameters(PARM_MOTHERS_MAIDEN_NAME, model.MothersMaidenName);
            }
            else
            {
                dbManager.AddParameters(PARM_MOTHERS_MAIDEN_NAME, null);
            }

            if (!string.IsNullOrEmpty(model.Address))
            {
                dbManager.AddParameters(PARM_ADDRESS, model.Address);
            }
            else
            {
                dbManager.AddParameters(PARM_ADDRESS, null);
            }
            if (!string.IsNullOrEmpty(model.Comments))
            {
                dbManager.AddParameters(PARM_COMMENTS, model.Comments);
            }
            else
            {
                dbManager.AddParameters(PARM_COMMENTS, null);
            }
            if (!string.IsNullOrEmpty(model.CreatedOn))
            {
                dbManager.AddParameters(PARM_CREATEDON, model.CreatedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_CREATEDON, null);
            }
            if (!string.IsNullOrEmpty(model.CreatedBy))
            {
                dbManager.AddParameters(PARM_CREATEDBY, model.CreatedBy);
            }
            else
            {
                dbManager.AddParameters(PARM_CREATEDBY, null);
            }
            if (!string.IsNullOrEmpty(model.ModifiedOn))
            {
                dbManager.AddParameters(PARM_MODIFIEDON, model.ModifiedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIEDON, null);
            }
            if (!string.IsNullOrEmpty(model.ModifiedBy))
            {
                dbManager.AddParameters(PARM_MODIFIEDBY, model.ModifiedBy);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIEDBY, null);
            }
            if (!string.IsNullOrEmpty(model.XML))
            {
                dbManager.AddParameters(PARM_XML, model.XML);
            }
            else
            {
                dbManager.AddParameters(PARM_XML, null);
            }
            if (!string.IsNullOrEmpty(model.ResponseType))
            {
                dbManager.AddParameters(PARM_RESPONSE_TYPE, model.ResponseType);
            }
            else
            {
                dbManager.AddParameters(PARM_RESPONSE_TYPE, null);
            }
            if (!string.IsNullOrEmpty(model.QueryId))
            {
                dbManager.AddParameters(PARM_QUERY_ID, model.QueryId);
            }
            else
            {
                dbManager.AddParameters(PARM_QUERY_ID, null);
            }
            

        }
        private void createManufacturerParameters(IDBManager dbManager, Manufacturer model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_MANUFACTURER_ID, model.ManufacturerId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_MANUFACTURER_ID, model.ManufacturerId, DbType.Int64);
            dbManager.AddParameters(PARM_MANUFACTURER_NAME, model.ManufacturerName, DbType.String);
            dbManager.AddParameters(PARM_MVX_CODE, model.MVXCode, DbType.String);
            dbManager.AddParameters(PARM_STATUS, model.Status, DbType.String);
            dbManager.AddParameters(PARAM_CREATED_BY, model.CreatedBy, DbType.String);
            dbManager.AddParameters(PARAM_CREATED_ON, model.CreatedOn, DbType.DateTime);
            dbManager.AddParameters(PARAM_MODIFIED_BY, model.ModifiedBy, DbType.String);
            dbManager.AddParameters(PARAM_MODIFIED_ON, model.ModifiedOn, DbType.DateTime);
        }

        private void createVaccineParameters(IDBManager dbManager, VaccineAndTerapuetic model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_VACCINE_ID, model.ImmunizationId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_VACCINE_ID, model.ImmunizationId, DbType.Int64);
            dbManager.AddParameters(PARM_IMMUNIZATION_NAME, model.ImmunizationName, DbType.String);
            dbManager.AddParameters(PARM_CVX, model.CVX, DbType.String);
            if (model.CPTCode == "")
            {
                dbManager.AddParameters(PARM_CPT_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_CPT_CODE, model.CPTCode, DbType.String);
            }
            if (model.CPTDescription == "")
            {
                dbManager.AddParameters(PARM_CPT_DESCRIPTION, null);
            }
            else
            {
                dbManager.AddParameters(PARM_CPT_DESCRIPTION, model.CPTDescription, DbType.String);
            }
            if (model.AdminCode == "")
            {
                dbManager.AddParameters(PARM_ADMIN_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_ADMIN_CODE, model.AdminCode, DbType.String);
            }
            if (model.AdminCodeDescription == "")
            {
                dbManager.AddParameters(PARM_ADMIN_CODE_DESCRIPTION, null);
            }
            else
            {
                dbManager.AddParameters(PARM_ADMIN_CODE_DESCRIPTION, model.AdminCodeDescription, DbType.String);
            }
            if (model.NDCCode == "")
            {
                dbManager.AddParameters(PARM_NDC_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_NDC_CODE, model.NDCCode, DbType.String);
            }

            if (model.Dose == "")
            {
                dbManager.AddParameters(PARM_DOSE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE, model.Dose, DbType.Decimal);
            }
            if (model.Amount == "")
            {
                dbManager.AddParameters(PARM_AMOUNT, null);
            }
            else
            {
                dbManager.AddParameters(PARM_AMOUNT, model.Amount, DbType.Int32);
            }
            if (model.ManufactureIds == "")
            {
                dbManager.AddParameters(PARM_MANUFACTUREIDS, null);
            }
            else
            {
                dbManager.AddParameters(PARM_MANUFACTUREIDS, model.ManufactureIds, DbType.String);
            }
            dbManager.AddParameters(PARM_STATUS, model.Status, DbType.String);
            dbManager.AddParameters(PARAM_CREATED_BY, model.CreatedBy, DbType.String);
            dbManager.AddParameters(PARAM_CREATED_ON, model.CreatedOn, DbType.DateTime);
            dbManager.AddParameters(PARAM_MODIFIED_BY, model.ModifiedBy, DbType.String);
            dbManager.AddParameters(PARAM_MODIFIED_ON, model.ModifiedOn, DbType.DateTime);
        }


        private void createVaccineVISParameters(IDBManager dbManager, VaccineVIS model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_VACCINE_VIS_ID, model.VaccineVISId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_VACCINE_VIS_ID, model.VaccineVISId, DbType.Int64);
            dbManager.AddParameters(PARM_VIS_DATE, model.VISDate, DbType.DateTime);
            dbManager.AddParameters(PARM_DOCUMENT_Name, model.VISDocumentName, DbType.String);
            dbManager.AddParameters(PARM_DOCUMENT_LINK, model.VISDocumentLink, DbType.String);
            dbManager.AddParameters(PARM_VIS_FULLY_ENCODED_TEXT, model.VISFullyEncodedText, DbType.String);
            dbManager.AddParameters(PARM_VACCINE_ID, model.VaccineId, DbType.Int64);
            dbManager.AddParameters(PARM_CVX, model.CVX, DbType.String);

        }

        private void createUpdateVaccineVisParameters(IDBManager dbManager, VaccineVIS model, Boolean IsInsert)
        {

            dbManager.AddParameters(PARM_VACCINE_VIS_ID, model.VaccineVISId, DbType.Int64);
            dbManager.AddParameters(PARM_VACCINE_VIS_URL_ID, model.VaccineVIS_URLId, DbType.Int64);
            dbManager.AddParameters(PARM_VIS_DATE, model.VISDate, DbType.DateTime);
            dbManager.AddParameters(PARM_DOCUMENT_Name, model.VISDocumentName, DbType.String);
            dbManager.AddParameters(PARM_DOCUMENT_LINK, model.VISDocumentLink, DbType.String);
            dbManager.AddParameters(PARM_VIS_FULLY_ENCODED_TEXT, model.VISFullyEncodedText, DbType.String);
            dbManager.AddParameters(PARM_CVX, model.CVX, DbType.String);
        }

        private void CreateParametersForCategory(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, ds.Category.VaccineGroupIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, ds.Category.VaccineGroupIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARAM_SHORTNAME, ds.Category.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.Category.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(3, PARAM_CREATED_BY, ds.Category.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARAM_CREATED_ON, ds.Category.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARAM_MODIFIED_BY, ds.Category.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARAM_MODIFIED_ON, ds.Category.ModifyOnColumn.ColumnName, DbType.DateTime);
        }

        private void createTherapeuticeParameters(IDBManager dbManager, VaccineAndTerapuetic model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_THERAPEUTIC_ID, model.TherapeuticId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_THERAPEUTIC_ID, model.TherapeuticId, DbType.Int64);
            dbManager.AddParameters(PARM_THER_INJ_NAME, model.TherInjName, DbType.String);
            if (model.NDCCode == "")
            {
                dbManager.AddParameters(PARM_NDC_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_NDC_CODE, model.NDCCode, DbType.String);
            }
            if (model.CPTCode == "")
            {
                dbManager.AddParameters(PARM_CPT_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_CPT_CODE, model.CPTCode, DbType.String);
            }
            if (model.CPTDescription == "")
            {
                dbManager.AddParameters(PARM_CPT_DESCRIPTION, null);
            }
            else
            {
                dbManager.AddParameters(PARM_CPT_DESCRIPTION, model.CPTDescription, DbType.String);
            }
            if (model.AdminCode == "")
            {
                dbManager.AddParameters(PARM_ADMIN_CODE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_ADMIN_CODE, model.AdminCode, DbType.String);
            }

            if (model.AdminCodeDescription == "")
            {
                dbManager.AddParameters(PARM_ADMIN_CODE_DESCRIPTION, null);
            }
            else
            {
                dbManager.AddParameters(PARM_ADMIN_CODE_DESCRIPTION, model.AdminCodeDescription, DbType.String);
            }
            if (model.Dose == "")
            {
                dbManager.AddParameters(PARM_DOSE, null);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE, model.Dose, DbType.Decimal);
            }
            if (model.Amount == "")
            {
                dbManager.AddParameters(PARM_AMOUNT, null);
            }
            else
            {
                dbManager.AddParameters(PARM_AMOUNT, model.Amount, DbType.Int32);
            }
            if (model.ManufactureIds == "")
            {
                dbManager.AddParameters(PARM_MANUFACTUREIDS, null);
            }
            else
            {
                dbManager.AddParameters(PARM_MANUFACTUREIDS, model.ManufactureIds, DbType.String);
            }


            dbManager.AddParameters(PARM_STATUS, model.Status, DbType.Byte);
            dbManager.AddParameters(PARAM_CREATED_BY, model.CreatedBy, DbType.String);
            dbManager.AddParameters(PARAM_CREATED_ON, model.CreatedOn, DbType.DateTime);
            dbManager.AddParameters(PARAM_MODIFIED_BY, model.ModifiedBy, DbType.String);
            dbManager.AddParameters(PARAM_MODIFIED_ON, model.ModifiedOn, DbType.DateTime);
        }



        public DSImmunization InsertAministerVaccine(DSImmunization ds, bool PreviousVoid, bool IsAddProcedures, string NotesId, string FavListNames, string FavListVal)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VaccineHx.GetChanges();
                dbManager.Open();
                CreateParametersForVaccineHxInsetUpdate(dbManager, ds, true, PreviousVoid, IsAddProcedures, NotesId, FavListNames, FavListVal);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_VACCINEHX, ds, ds.VaccineHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALImmunization::InsertAministerVaccine", PROC_INSERT_VACCINEHX, ex);
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

        public DSImmunization InsertTherapeuticInjection(DSImmunization ds)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.TherapeuticInjection.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                CreateParametersForTherapeuticInjection(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_THERAPEUTIC_INJECTION_INSERT, ds, ds.TherapeuticInjection.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALImmunization::InsertTherapeuticInjection", PROC_THERAPEUTIC_INJECTION_INSERT, ex);
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

        public DSImmunization UpdateTherapeuticInjection(DSImmunization ds)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.TherapeuticInjection.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                CreateParametersForTherapeuticInjection(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_THERAPEUTIC_INJECTION_UPDATE, ds, ds.TherapeuticInjection.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALImmunization::UpdateTherapeuticInjection", PROC_THERAPEUTIC_INJECTION_UPDATE, ex);
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


        //Start//11/04/2016//Abid Ali//method to Load VaccineHx
        //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
        public DSImmunization LoadVaccineHx(long patientId, long vaccineHxId = 0, string isViewVaccineHx = "", string isPrintVaccineHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                DSImmunization ds = new DSImmunization();
                //dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, vaccineHxId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_LOAD, ds, ds.VaccineHx.TableName);
                //if (ds.VaccineHx.Rows.Count > 0)
                //{
                //    if (Convert.ToInt64(ds.VaccineHx.Rows[0]["VaccineHxId"]) > 0)
                //    {

                //        DataTable dtTemp = ds.VaccineHx;
                //        if (dtTemp != null)
                //        {
                //            if (isViewVaccineHx == "1" || isPrintVaccineHx == "1")
                //            {
                //                bool isViewAction = isViewVaccineHx == "1" ? true : false;
                //                bool isPrintAcion = isPrintVaccineHx == "1" ? true : false;
                //                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), isViewAction, isPrintAcion);
                //                dsDBAudit.AcceptChanges();
                //            }
                //        }
                //    }
                //}
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALImmunization::InsertVaccineHx", PROC_ImmunizationHx_INSERT, ex);
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
        //End//11/04/2016//Abid Ali//method to Load VaccineHx


        //Start//11/04/2016//Abid Ali//method to Update VaccineHx
        public DSImmunization UpdateVaccineHx(DSImmunization ds)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VaccineHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParametersForVaccineHx(dbManager, ds, false);

                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_UPDATE, ds, ds.VaccineHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString());
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
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit


                MDVLogger.DALErrorLog("DALImmunization::UpdateVaccineHx", PROC_ImmunizationHx_UPDATE, ex);
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

        public DSImmunization VaccineHxUpdate(DSImmunization ds, bool PreviousVoid, bool IsAddProcedures, string NotesId = null, IDBManager dbManager = null)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
            }
            try
            {
                this.CreateParametersForVaccineHxInsetUpdate(dbManager, ds, false, PreviousVoid, IsAddProcedures, NotesId);

                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_UPDATE_VACCINEHX, ds, ds.VaccineHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::VaccineHxUpdate", PROC_ImmunizationHx_UPDATE, ex);
                throw ex;
                //string[] str = ex.Message.Split('|');
                //if (str.Length > 1)
                //    throw new Exception(str[1].ToString());
                //else
                //    throw new Exception(ex.Message);
            }
        }

        //End//11/04/2016//Abid Ali//method to Update VaccineHx

        private void CreateParametersForVaccineHx(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

            //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields



            if (IsInsert)
            {
                dbManager.CreateParameters(38);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.VaccineHx.VaccineHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(37);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.VaccineHx.VaccineHxIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARAM_VACCINE_GROUP_CATEGORY, ds.VaccineHx.VaccineGroupCategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARAM_VISIT_DATE, ds.VaccineHx.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARAM_PROVIDER_ID, ds.VaccineHx.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARAM_VACCINE, ds.VaccineHx.VaccineColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARAM_ADMINISTRATION_DATE, ds.VaccineHx.AdministrationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARAM_DOSE, ds.VaccineHx.DoseColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(7, PARAM_AMOUNT, ds.VaccineHx.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARAM_LOT_NUMBER, ds.VaccineHx.LotNumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARAM_MANUFACTURER, ds.VaccineHx.ManufacturerColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARAM_ROUTE, ds.VaccineHx.RouteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARAM_SITE, ds.VaccineHx.SiteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARAM_EXPIRY_DATE, ds.VaccineHx.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARAM_VFC, ds.VaccineHx.VFCColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARAM_VISDATE, ds.VaccineHx.VISDateColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARAM_REACTION, ds.VaccineHx.ReactionColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARAM_VOID_DOES, ds.VaccineHx.VoidDoseColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARAM_COMMENTS, ds.VaccineHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARAM_PUBLICITY_CODE, ds.VaccineHx.PublicityCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARAM_PUBLICITY_CODE_EXPIRY_DATE, ds.VaccineHx.PublicityCodeExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARAM_IMMUNIZATION_REGISTRY_STATUS_CODE, ds.VaccineHx.ImmunizationRegistryStatusCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARAM_IRS_EFFECTIVE_DATE, ds.VaccineHx.IRSEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARAM_PROTECTION_INDICATOR, ds.VaccineHx.ProtectionIndicatorColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(23, PARAM_PI_EFFECTIVE_DATE, ds.VaccineHx.PIEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARAM_TYPE, ds.VaccineHx.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARAM_PATIENT_ID, ds.VaccineHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARAM_IS_ACITVE, ds.VaccineHx.ISActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARAM_SOURCE_OF_HX, ds.VaccineHx.SourceOfHxColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(28, PARAM_SCHEDULER_ID, ds.VaccineHx.VaccineScheduleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARAM_REFUSAL_REASON_ID, ds.VaccineHx.RefusalReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARAM_VISIT_DATE_ID, ds.VaccineHx.VisitDateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARAM_PATIENT_AGE, ds.VaccineHx.PatientAgeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARAM_OVERRIDE_RULE, ds.VaccineHx.OverrideRuleColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARAM_GIVEN_BY, ds.VaccineHx.GivenByColumn.ColumnName, DbType.Int64);

            if (IsInsert == true)
            {
                dbManager.AddParameters(34, PARAM_CREATED_BY, ds.VaccineHx.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARAM_CREATED_ON, ds.VaccineHx.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(36, PARAM_MODIFIED_BY, ds.VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(37, PARAM_MODIFIED_ON, ds.VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
            else
            {
                dbManager.AddParameters(34, PARAM_MODIFIED_BY, ds.VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARAM_MODIFIED_ON, ds.VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(36, PARAM_ORDERSET_ID, ds.VaccineHx.OrderSetIdColumn.ColumnName, DbType.Int64);

            }
        }

        private void CreateParametersForVaccineHxInsetUpdate(IDBManager dbManager, DSImmunization ds, Boolean IsInsert, bool PreviousVoid, bool IsAddProcedures, string NotesId, string FavListNames = null, string FavListVal = null)
        {
            //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

            //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields
            if (IsInsert)
            {
                dbManager.CreateParameters(49);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.VaccineHx.VaccineHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(44);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.VaccineHx.VaccineHxIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARAM_VACCINE_GROUP_CATEGORY, ds.VaccineHx.VaccineGroupCategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARAM_VISIT_DATE, ds.VaccineHx.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARAM_PROVIDER_ID, ds.VaccineHx.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARAM_VACCINE, ds.VaccineHx.VaccineColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARAM_ADMINISTRATION_DATE, ds.VaccineHx.AdministrationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARAM_DOSE, ds.VaccineHx.DoseColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(7, PARAM_AMOUNT, ds.VaccineHx.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARAM_LOT_NUMBER, ds.VaccineHx.LotNumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARAM_MANUFACTURER, ds.VaccineHx.ManufacturerColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARAM_ROUTE, ds.VaccineHx.RouteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARAM_SITE, ds.VaccineHx.SiteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARAM_EXPIRY_DATE, ds.VaccineHx.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARAM_VFC, ds.VaccineHx.VFCColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARAM_VISDATE, ds.VaccineHx.VISDateColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARAM_REACTION, ds.VaccineHx.ReactionColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARAM_VOID_DOES, ds.VaccineHx.VoidDoseColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARAM_COMMENTS, ds.VaccineHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARAM_PUBLICITY_CODE, ds.VaccineHx.PublicityCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARAM_PUBLICITY_CODE_EXPIRY_DATE, ds.VaccineHx.PublicityCodeExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARAM_IMMUNIZATION_REGISTRY_STATUS_CODE, ds.VaccineHx.ImmunizationRegistryStatusCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARAM_IRS_EFFECTIVE_DATE, ds.VaccineHx.IRSEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARAM_PROTECTION_INDICATOR, ds.VaccineHx.ProtectionIndicatorColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(23, PARAM_PI_EFFECTIVE_DATE, ds.VaccineHx.PIEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARAM_TYPE, ds.VaccineHx.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARAM_PATIENT_ID, ds.VaccineHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARAM_IS_ACITVE, ds.VaccineHx.ISActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARAM_SOURCE_OF_HX, ds.VaccineHx.SourceOfHxColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(28, PARAM_SCHEDULER_ID, ds.VaccineHx.VaccineScheduleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARAM_REFUSAL_REASON_ID, ds.VaccineHx.RefusalReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARAM_VISIT_DATE_ID, ds.VaccineHx.VisitDateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(31, PARAM_PATIENT_AGE, ds.VaccineHx.PatientAgeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARAM_OVERRIDE_RULE, ds.VaccineHx.OverrideRuleColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARAM_GIVEN_BY, ds.VaccineHx.GivenByColumn.ColumnName, DbType.Int64);
            if (IsInsert == true)
            {
                dbManager.AddParameters(34, PARAM_CREATED_BY, ds.VaccineHx.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARAM_CREATED_ON, ds.VaccineHx.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(36, PARAM_MODIFIED_BY, ds.VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(37, PARAM_MODIFIED_ON, ds.VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(38, PARM_PNOTESID, NotesId);
                dbManager.AddParameters(39, PARM_PENTITYID, MDVSession.Current.EntityId);
                dbManager.AddParameters(40, PARM_PENTITYREGCODE, MDVSession.Current.EntityRegCode);
                dbManager.AddParameters(41, PARM_PFAVLISTNAMES, FavListNames);
                dbManager.AddParameters(42, PARM_PFAVLISTVAL, FavListVal);
                dbManager.AddParameters(43, PARM_PISADD_PROCEDURES, IsAddProcedures);
                dbManager.AddParameters(44, PARM_USER_NAME, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(45, PARAM_FACILITY_ID, ds.VaccineHx.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(46, PARAM_RECEIVING_APPLICATION_ID, ds.VaccineHx.ReceivingApplicationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(47, PARAM_USERID, ds.VaccineHx.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(48, PARAM_IS_HISTORY_DOSE, ds.VaccineHx.IsHistoryDoseColumn.ColumnName, DbType.Byte);
            }
            else
            {
                dbManager.AddParameters(34, PARAM_MODIFIED_BY, ds.VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARAM_MODIFIED_ON, ds.VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(36, PARAM_ORDERSET_ID, ds.VaccineHx.OrderSetIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(37, PARM_PNOTESID, NotesId);
                dbManager.AddParameters(38, PARM_PPREVIOUS_VOID, PreviousVoid);
                dbManager.AddParameters(39, PARM_PISADD_PROCEDURES, IsAddProcedures);
                dbManager.AddParameters(40, PARM_USER_NAME, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(41, PARAM_FACILITY_ID, ds.VaccineHx.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(42, PARAM_RECEIVING_APPLICATION_ID, ds.VaccineHx.ReceivingApplicationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(43, PARAM_USERID, ds.VaccineHx.UserIdColumn.ColumnName, DbType.Int64);

            }
        }

        private void CreateParametersForTherapeuticInjection(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

            //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields



            if (IsInsert)
            {
                dbManager.CreateParameters(22);
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, ds.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(19);
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, ds.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARAM_THERAPEUTIC_INJECTION_ID, ds.TherapeuticInjection.TherapeuticInjectionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARAM_VISIT_DATE, ds.TherapeuticInjection.VisitDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARAM_PROVIDER_ID, ds.TherapeuticInjection.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARAM_ADMINISTRATION_DATE, ds.TherapeuticInjection.AdministrationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(5, PARAM_DOSE, ds.TherapeuticInjection.DoseColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(6, PARAM_AMOUNT, ds.TherapeuticInjection.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARAM_VACCINE_MANUFACTURER_ID, ds.TherapeuticInjection.ManufacturerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARAM_ROUTE_ID, ds.TherapeuticInjection.RouteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARAM_SITE_ID, ds.TherapeuticInjection.SiteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARAM_EXPIRY_DATE, ds.TherapeuticInjection.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARAM_VFC, ds.TherapeuticInjection.VFCColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARAM_REACTION, ds.TherapeuticInjection.ReactionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARAM_COMMENTS, ds.TherapeuticInjection.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARAM_SOURCE_OF_HX, ds.TherapeuticInjection.SourceOfHxColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARAM_TYPE, ds.TherapeuticInjection.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARAM_LOT_NUMBER, ds.TherapeuticInjection.LotNumberColumn.ColumnName, DbType.Int64);

            if (IsInsert == true)
            {
                dbManager.AddParameters(17, PARAM_PATIENT_ID, ds.TherapeuticInjection.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(18, PARAM_CREATED_BY, ds.TherapeuticInjection.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(19, PARAM_CREATED_ON, ds.TherapeuticInjection.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(20, PARAM_MODIFIED_BY, ds.TherapeuticInjection.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(21, PARAM_MODIFIED_ON, ds.TherapeuticInjection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
            else
            {
                dbManager.AddParameters(17, PARAM_MODIFIED_BY, ds.TherapeuticInjection.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(18, PARAM_MODIFIED_ON, ds.TherapeuticInjection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
        }

        //public DSImmunization InsertDocumentHxDose(DSImmunization ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        CreateParametersForDocumentHxDose(dbManager, ds, true);
        //        ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENTLOGIN_INSERT, ds, ds.DocumentHxDose.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::InsertDocumentHxDose", PROC_PATIENTLOGIN_INSERT, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            throw new Exception(str[1].ToString());
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}


        //public DSImmunization UpdateDocumentHxDose(DSImmunization ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        this.CreateParametersForDocumentHxDose(dbManager, ds, false);
        //        ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENTLOGIN_UPDATE, ds, ds.DocumentHxDose.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::UpdatePatientLogin", PROC_PATIENTLOGIN_UPDATE, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            throw new Exception(str[1].ToString());
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        //private void CreateParametersForDocumentHxDose(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        //{
        //    dbManager.CreateParameters(15);

        //    int x = 0;

        //    if (IsInsert == true)
        //        dbManager.AddParameters(x++, PARAM_DOCUMENT_HX_ID, ds.DocumentHxDose.DocumentHxDoseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        //    else
        //        dbManager.AddParameters(x++, PARAM_DOCUMENT_HX_ID, ds.DocumentHxDose.DocumentHxDoseIdColumn.ColumnName, DbType.Int64);

        //    dbManager.AddParameters(x++, PARAM_VACCINE_GROUP_CATEGORY, ds.DocumentHxDose.VaccineGroupCategoryColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_VACCINE, ds.DocumentHxDose.VaccineColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_LOT_NUMBER, ds.DocumentHxDose.LotNumberColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_ROUTE, ds.DocumentHxDose.RouteColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_VFC, ds.DocumentHxDose.VFCColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_COMMENTS, ds.DocumentHxDose.CommentsColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(x++, PARAM_SOURCE_OF_HX, ds.DocumentHxDose.SourceOfHxColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_ADMINISTRATION_DATE, ds.DocumentHxDose.AdministrationDateColumn.ColumnName, DbType.DateTime);
        //    dbManager.AddParameters(x++, PARAM_DOSE, ds.DocumentHxDose.DoseColumn.ColumnName, DbType.Double);
        //    dbManager.AddParameters(x++, PARAM_AMOUNT, ds.DocumentHxDose.AmountColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(x++, PARAM_SITE, ds.DocumentHxDose.SiteColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_GIVEN_BY, ds.DocumentHxDose.GivenByColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x++, PARAM_MANUFACTURER, ds.DocumentHxDose.ManufacturerColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(x, PARAM_EXPIRY_DATE, ds.DocumentHxDose.ExpiryDateColumn.ColumnName, DbType.DateTime);

        //}

        #endregion

        #region "LastUpdatePatientInfo"

        //public DSRcopia InsertLastUpdatePatientInfo(DSRcopia ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        CreateParametersForLastUpdatePatientInfo(dbManager, ds, true);
        //        ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_LASTUPDATEPATIENTINFO, ds, ds.Rcopia_PatientLastUpdateInfo.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::insertImmunization", PROC_Immunization_INSERT, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            throw new Exception(str[1].ToString());
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        #endregion

        #region "Immunization"

        //Start//01/12/2015//Ahmad Raza//method to Add Immunization
        //public DSImmunization insertImmunization(DSImmunization ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        CreateParameters(dbManager, ds, true);
        //        ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Immunization_INSERT, ds, ds.Immunization.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::insertImmunization", PROC_Immunization_INSERT, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            throw new Exception(str[1].ToString());
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //End//01/12/2015//Ahmad Raza//method to Add Immunization

        //Start//01/12/2015//Ahmad Raza//method to update Immunization
        //public DSImmunization updateImmunization(DSImmunization ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        this.CreateParameters(dbManager, ds, false);
        //        ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_Immunization_UPDATE, ds, ds.Immunization.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::updateImmunization", PROC_Immunization_UPDATE, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            throw new Exception(str[1].ToString());
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //End//01/12/2015//Ahmad Raza//method to update Immunization

        //Start//01/12/2015//Ahmad Raza//method to delete Immunization
        //public string deleteImmunization(string ImmunizationId)
        //{
        //    string returnVal = "";
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, PARM_Immunization_ID, ImmunizationId);
        //        dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        //        returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Immunization_DELETE).ToString();

        //        if (returnVal != "")
        //            throw new Exception(returnVal);

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALImmunization::deleteImmunization", PROC_Immunization_DELETE, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            return str[1].ToString();
        //        else
        //            return ex.Message;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //End//01/12/2015//Ahmad Raza//method to delete Immunization

        //Start//01/12/2015//Ahmad Raza//method to load Immunization for grid
        public DSImmunization loadImmunization()
        {
            DSImmunization ds = new DSImmunization();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(1);
            dbManager.AddParameters(0, PARAM_SEARCH_TEXT, "jjkjlkjkl");

            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_SELECT, ds, ds.ImmunizationHx.TableName);

            dbManager.Dispose();
            return ds;
        }

        public DSImmunization LoadImmunizationTherapeuticInjection(long immTherapeuticInjectionId, long patientId, long NoteId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSImmunization ds = new DSImmunization();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(6);
            if (immTherapeuticInjectionId > 0)
            {
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, immTherapeuticInjectionId);
            }
            else
            {
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, null);
            }
            if (patientId > 0)
            {
                dbManager.AddParameters(1, PARAM_PATIENT_ID, patientId);
            }
            else
            {
                dbManager.AddParameters(1, PARAM_PATIENT_ID, null);
            }
            if (NoteId > 0)
            {
                dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);
            }
            else
            {
                dbManager.AddParameters(2, PARM_NOTE_ID, null);
            }
            if (pageNumber == 0)
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
            else
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
            if (rowsPerPage == 0)
                dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
            else
                dbManager.AddParameters(4, PARM_ROWSP_PAGE, rowsPerPage);

            dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.TherapeuticInjection.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Therapeutic_Injection_SELECT, ds, ds.TherapeuticInjection.TableName);

            dbManager.Dispose();
            return ds;
        }
        //Start//16-06-2016//Ahmad Raza// Loading Immunization for FaceSheet View
        public DSImmunization loadImmunizationForFaceSheet(long patientId)
        {
            DSImmunization ds = new DSImmunization();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            dbManager.CreateParameters(1);
            if (patientId > 0)
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
            else
                dbManager.AddParameters(0, PARM_PATIENT_ID, null);

            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_SELECT_For_FaceSheet, ds, ds.VaccineFaceSheet.TableName);

            dbManager.Dispose();
            return ds;
        }
        //End//16-06-2016//Ahmad Raza// Loading Immunization for FaceSheet View

        //Start//08/04/2016//Abid Ali//method to Load Parent Child Immunization
        public DSImmunization loadParentChildImmunization(long patientId, long entityId, bool IsActive, int pageNo = 0, int rpp = 0, long NotesId = 0)
        {
            DSImmunization ds = new DSImmunization();

            //For multiple select in same store procedure.
            List<string> tablesList = new List<string>();
            tablesList.Add(ds.ParentVaccineHx.TableName);
            tablesList.Add(ds.ChildVaccineHx.TableName);

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(7);
            if (entityId <= 0)
                dbManager.AddParameters(0, PARAM_ENTITY_ID, null);
            else
                dbManager.AddParameters(0, PARAM_ENTITY_ID, entityId);
            if (patientId <= 0)
                dbManager.AddParameters(1, PARM_PATIENT_ID, null);
            else
                dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
            //dbManager.AddParameters(1, PARM_PATIENT_ID, 163);


            dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

            if (pageNo <= 0)
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);
            else
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNo);
            if (rpp <= 0)
                dbManager.AddParameters(4, PARM_ROWSP_PAGE, 2000);
            else
                dbManager.AddParameters(4, PARM_ROWSP_PAGE, rpp);

            dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.ParentVaccineHx.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            if (NotesId == 0)
                dbManager.AddParameters(6, PARM_NOTE_ID, null);
            else
                dbManager.AddParameters(6, PARM_NOTE_ID, NotesId);

            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ImmunizationHx_SELECT, ds, tablesList);

            dbManager.Dispose();
            return ds;

        }
        //End//08/04/2016//Abid Ali//method to Load Parent Child Immunization

        //Start//08/04/2016//Abid Ali//method to Load Parent Child Immunization

        public DSImmunization LoadImmunizationAlert(long PatientId, int PageNumber = 1, int RowsPerPage = 1000)
        {

            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
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
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.PatientImmunizationAlert.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_ALERT_SELECT, ds, ds.PatientImmunizationAlert.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationAlert", PROC_IMMUNIZATION_ALERT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LoadImmunizationAlertForPrint(long PatientId)
        {

            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_ALERT_SELECT_FOR_PRINT, ds, ds.PatientImmunizationAlert.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationAlertForPrint", PROC_IMMUNIZATION_ALERT_SELECT_FOR_PRINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LoadVaccine(string VaccineHxIds, long PatientId, long userId)
        {
            DSImmunization ds = new DSImmunization();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(4);

            if (VaccineHxIds == null)
                dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, null);
            else
                dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, VaccineHxIds);
            dbManager.AddParameters(1, PARAM_PATIENT_ID, PatientId);
            if (userId <= 0)
                dbManager.AddParameters(2, PARAM_USERID, null);
            else
                dbManager.AddParameters(2, PARAM_USERID, userId);
            dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);            

            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINEHx_SELECT, ds, ds.Vaccine.TableName);

            dbManager.Dispose();
            return ds;

        }



        public DSImmunization GetCptCodeAndAdministeredCode(Int16 TherapeuticInjectionId)
        {
            DSImmunization ds = new DSImmunization();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(1);

            if (TherapeuticInjectionId == 0)
                dbManager.AddParameters(0, PARAM_THERAPEUTIC_INJECTION_ID, null);
            else
                dbManager.AddParameters(0, PARAM_THERAPEUTIC_INJECTION_ID, TherapeuticInjectionId);


            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CPT_CODEANDADMINISTEREDCODE_SELECT, ds, ds.Cpts.TableName);

            dbManager.Dispose();
            return ds;

        }
        //End//08/04/2016//Abid Ali//method to Load Parent Child Immunization

        public DSImmunization LoadImmTherapeuticInjectionForSoapText(string TheraInjectionIds, long PatientId, long userId)
        {
            DSImmunization ds = new DSImmunization();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(4);

            if (TheraInjectionIds == null)
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, null);
            else
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, TheraInjectionIds);
            dbManager.AddParameters(1, PARAM_PATIENT_ID, PatientId);
            if (userId <= 0)
                dbManager.AddParameters(2, PARAM_USERID, null);
            else
                dbManager.AddParameters(2, PARAM_USERID, userId);
            dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_THERAPEUT_ICINJECTION_SELECT, ds, ds.TherapeuticInjection.TableName);

            dbManager.Dispose();
            return ds;

        }

        /// <summary>
        /// Attaching Problem Lists With Progress notes
        /// </summary>
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachVaccineFromNotes(string ProcedureId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (ProcedureId == "")
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_VACCINE_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::detachVaccineFromNotes", PROC_DETACH_VACCINE_FROM_NOTES, ex);
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
        /// <param name="ProcedureId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSImmunization attachVaccineWithNotes(string ProcedureId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSImmunization ds = new DSImmunization();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (ProcedureId == "")
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, ProcedureId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_VACCINE_FROM_NOTES, ds, ds.VaccineHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::attachVaccineWithNotes", PROC_ATTACH_VACCINE_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization attachTheraInjectionwithnotes(string ImmTherInjectionId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSImmunization ds = new DSImmunization();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (ImmTherInjectionId == "")
                {
                    dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, ImmTherInjectionId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_THER_INJECTION_FROM_NOTES, ds, ds.VaccineHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::attachTheraInjectionwithnotes", PROC_ATTACH_THER_INJECTION_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachTheraInjectionwithnotes(string ImmTherInjectionId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (ImmTherInjectionId == "")
                {
                    dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, ImmTherInjectionId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_THER_INJECTION_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::detachTheraInjectionwithnotes", PROC_DETACH_THER_INJECTION_FROM_NOTES, ex);
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




        public DSImmunization GetSystemCategory()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SYSTEM_VACCINE_GROUP, ds, ds.LookUpVaccineGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetSystemCategory", PROC_SYSTEM_VACCINE_GROUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion



        #region  " Lookups "

        public List<LookupModel> getAllImmunizationLookupforReports()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<LookupModel> listModel = new List<LookupModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMMUNIZATION_LOOKUP_FOR_REPORTS);
                LookupModel model = null;
                while (reader.Read())
                {
                    model = new LookupModel();
                    model.LookUpType = Convert.ToString(reader["LookUpType"]);
                    model.Id = Convert.ToInt64(reader["Id"]);
                    model.Name = Convert.ToString(reader["Name"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::getAllImmunizationLookupforReports", PROC_IMMUNIZATION_LOOKUP_FOR_REPORTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<LookupModel> GetVaccineDropdownForReports(long categoryId)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<LookupModel> listModel = new List<LookupModel>();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VACCINE_CATEGORY_ID, categoryId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_VACCINE_LOOKUP_FOR_REPORTS);
                LookupModel model = null;
                while (reader.Read())
                {
                    model = new LookupModel();
                    model.LookUpType = Convert.ToString(reader["LookUpType"]);
                    model.Id = Convert.ToInt64(reader["Id"]);
                    model.Name = Convert.ToString(reader["Name"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALIMMUNIZATION::GetVaccineDropdownForReports", PROC_VACCINE_LOOKUP_FOR_REPORTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineGroup(bool IsActive)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (IsActive)
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);
                }
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_GROUP_LOOKUP, ds, ds.LookUpVaccineGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineGroup", PROC_VACCINE_GROUP_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupImmunizationAlerts()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALERT_TYPE_LOOKUP, ds, ds.ImmunizationAlertType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupImmunizationAlerts", PROC_ALERT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSImmunization LookupVaccine(long vaccineGroupId, string forModule)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(2);

                if (vaccineGroupId > 0) { dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, vaccineGroupId); }
                else { dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, null); }

                dbManager.AddParameters(1, PARM_VACCINE_FOR_MODULE, forModule);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_LOOKUP, ds, ds.LookupVaccine.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccine", PROC_VACCINE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author : Farooq Ahmad
        //Date   : 19/04/2016
        /// <summary>
        /// GetVaccines
        /// </summary>
        /// <param name="vaccineGroupId"></param>
        /// <param name="forModule"></param>
        /// <returns></returns>
        public DSImmunization GetVaccines(long vaccineGroupId, string forModule)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (vaccineGroupId > 0) { dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, vaccineGroupId); }
                else { dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, null); }

                dbManager.AddParameters(1, PARM_VACCINE_FOR_MODULE, forModule);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_LOAD, ds, ds.VaccineInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccines", PROC_VACCINE_LOAD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineRoute()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_ROUTE_LOOKUP, ds, ds.LookupVaccineRoute.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineRoute", PROC_VACCINE_ROUTE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineFundingSource()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_FUNDING_SOURCE_LOOKUP, ds, ds.LookupVaccineFundingSource.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineFundingSource", PROC_VACCINE_FUNDING_SOURCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization GetCategoryAgaintsSchAndSchtype(Int64 ScheduleTypeId, Int64 ScheduleId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ScheduleTypeId <= 0)
                {
                    dbManager.AddParameters(0, PARAM_SCHEDULE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_SCHEDULE_TYPE_ID, ScheduleTypeId);
                }
                if (ScheduleId <= 0)
                {
                    dbManager.AddParameters(1, PARAM_SCHEDULE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARAM_SCHEDULE_ID, ScheduleId);
                }

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_CATEGORY_AGAINTS_SCH_AND_SCHTYPE, ds, ds.Category.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetCategoryAgaintsSchAndSchtype", PROC_GET_CATEGORY_AGAINTS_SCH_AND_SCHTYPE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSImmunization LookupTherapeuticInjection()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_THERAPEUTIC_INJECTION_LOOKUP, ds, ds.LookupTherapeuticInjection.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupTherapeuticInjection", PROC_THERAPEUTIC_INJECTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineVfc()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_VFC_LOOKUP, ds, ds.LookupVaccineVFC.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineVfc", PROC_VACCINE_VFC_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSImmunization LookupVaccineSourceOfHx()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SOURCE_OF_HX_LOOKUP, ds, ds.LookupVaccineSourceOfHx.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineSourceOfHx", PROC_VACCINE_SOURCE_OF_HX_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineSite()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SITE_LOOKUP, ds, ds.LookupVaccineSite.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineSite", PROC_VACCINE_SITE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization loadSchedulerData(string TabId, long PatientId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_TAB_ID, TabId);
                dbManager.AddParameters(1, PARAM_PATIENT_ID, PatientId);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_VACCINE_SCHEDULER, ds, ds.VaccineSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::loadSchedlerData", PROC_LOAD_VACCINE_SCHEDULER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSImmunization loadSchedulerDataForPreview(string TabId, long PatientId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_TAB_ID, TabId);
                dbManager.AddParameters(1, PARAM_PATIENT_ID, PatientId);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_VACCINE_SCHEDULER_PREVIEW, ds, ds.VaccineSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::loadSchedlerData", PROC_LOAD_VACCINE_SCHEDULER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSImmunization loadImmunizationForPrint(long PatientId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_PATIENT_ID, PatientId);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_IMMUNIZATION_FOR_PREVIEW, ds, ds.VaccineSchedulePreview.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::loadImmunizationForPrint", PROC_SELECT_IMMUNIZATION_FOR_PREVIEW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSImmunization LookupVaccineReaction()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_REACTION_LOOKUP, ds, ds.VaccineReactionLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineReaction", PROC_VACCINE_REACTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupVaccineLotNumbers(long VaccineId, long ProviderId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARAM_VACCINE_ID, VaccineId);
                if (ProviderId == 0)
                {
                    dbManager.AddParameters(1, PARAM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARAM_PROVIDER_ID, ProviderId);
                }
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARAM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                }
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_LOT_NUMBER_LOOKUP, ds, ds.VaccineLotNumberLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineLotNumbers", PROC_VACCINE_LOT_NUMBER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupTherapeuticInjectionLotNumber(int TherapueticInjectionId, Int64 ProviderId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARAM_THERAPEUTIC_INJECTION_ID, TherapueticInjectionId);
                dbManager.AddParameters(1, PARAM_PROVIDER_ID, ProviderId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARAM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                }
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_THERAPUETIC_INJECTION_LOT_NUMBER_LOOKUP, ds, ds.VaccineLotNumberLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupTherapeuticInjectionLotNumber", PROC_THERAPUETIC_INJECTION_LOT_NUMBER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public DSImmunization LookupVaccinePublicityCode()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_PUBLICITY_CODE_LOOKUP, ds, ds.VaccinePublicityCodeLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineLotNumbers", PROC_VACCINE_PUBLICITY_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupVaccineRegistryStatus()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_REGISTRY_STATUS_LOOKUP, ds, ds.VaccineRegistryStatusLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineLotNumbers", PROC_VACCINE_REGISTRY_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupVaccineRefusalReason()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_REFUSAL_REASON_LOOKUP, ds, ds.VaccineRefusalReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineRefusalReason", PROC_VACCINE_REFUSAL_REASON_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization GetCQMEncounterType()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_ENCOUNTER_TYPE_ID, ds, ds.VaccineRefusalReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetCQMEncounterType", PROC_GET_ENCOUNTER_TYPE_ID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public DSImmunization LookupVaccineManufacturer(string VaccineId = "", string TherpeuticId = "")
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(2);
                if ((!string.IsNullOrWhiteSpace(VaccineId)) && (VaccineId != "0"))
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_ID, MDVUtility.ToInt64(VaccineId));
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_VACCINE_ID, null);
                }

                if ((!string.IsNullOrWhiteSpace(TherpeuticId)) && (TherpeuticId != "0"))
                {
                    dbManager.AddParameters(1, PARAM_THERAPEUTIC_INJECTION_ID, MDVUtility.ToInt64(TherpeuticId));
                }
                else
                {
                    dbManager.AddParameters(1, PARAM_THERAPEUTIC_INJECTION_ID, null);
                }


                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_MANUFACTURER_LOOKUP, ds, ds.LookupVaccineManufacturer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineManufacturer", PROC_VACCINE_MANUFACTURER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupVaccineGivenBy()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_GIVEN_BY_LOOKUP, ds, ds.LookupVaccineGivenBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineGivenBy", PROC_VACCINE_GIVEN_BY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /*public string GetVISDateAndVisURL(string VaccineID)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_VACCINE_ID, !string.IsNullOrWhiteSpace(VaccineID) ? VaccineID : null);

                object obj = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_VACCINEHX_VISDATE_AND_URL);
                return obj.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVISDateAndVisURL", PROC_GET_VACCINEHX_VISDATE_AND_URL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }*/

        public List<VaccineVIS> GetVISDateAndVisURL(string VaccineID)
        {
            List<VaccineVIS> ImmList = new List<VaccineVIS>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARAM_VACCINE_ID, !string.IsNullOrWhiteSpace(VaccineID) ? VaccineID : null);
                ImmList = dbManager.ExecuteReaders<VaccineVIS>(PROC_GET_VACCINEHX_VISDATE_AND_URL);
                return ImmList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetVISDateAndVisURL", PROC_GET_VACCINEHX_VISDATE_AND_URL, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public string GetVaccineVISDate(string VaccineID)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_VACCINE_ID, !string.IsNullOrWhiteSpace(VaccineID) ? VaccineID : null);

                object obj = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_VACCINEHX_VISDATE);
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccineVISDate", PROC_GET_VACCINEHX_VISDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string GetVaccineVIS_URL(string VaccineID)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_VACCINE_ID, !string.IsNullOrWhiteSpace(VaccineID) ? VaccineID : null);

                object obj = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_VACCINEHX_VIS_URL);
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccineVIS_URL", PROC_GET_VACCINEHX_VIS_URL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupSchedule(string ScheduleTypeId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (!string.IsNullOrEmpty(ScheduleTypeId))
                {
                    dbManager.AddParameters(0, PARAM_SCHEDULE_TYPE_ID, ScheduleTypeId);
                }
                else
                {
                    dbManager.AddParameters(0, PARAM_SCHEDULE_TYPE_ID, null);
                }
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_LOOKUP, ds, ds.LookupSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupSchedule", PROC_SCHEDULE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunization LookupScheduleType()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_TYPE_LOOKUP, ds, ds.LookupScheduleType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupScheduleType", PROC_SCHEDULE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region " Registery "

        public DSImmunization LookupRegistery(string IsActive)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RECEIVING_APPLICATION_LOOKUP, ds, ds.LookupRegistery.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupRegistery", PROC_RECEIVING_APPLICATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSImmunization LookupRegistrySubmission(string IsActive)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(0, PARM_IS_ACTIVE, null);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REGISTERY_SUBMISSION_LOOKUP, ds, ds.LookupRegistrySubmission.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupRegistrySubmission", PROC_REGISTERY_SUBMISSION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region HL7 Immunization

        public DSImmunizationHL7 Generate_HL7Immunization_Message(string vaccinheHxIds)
        {
            DSImmunizationHL7 ds = new DSImmunizationHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();





            try
            {

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, (string.IsNullOrWhiteSpace(vaccinheHxIds)) ? null : vaccinheHxIds);
                //                if (!string.IsNullOrWhiteSpace(vaccinheHxIds) ) { dbManager.AddParameters(0, PARAM_VACCINE_HX_IDS, vaccinheHxIds); }
                //else { dbManager.AddParameters(0, vaccinheHxIds, null); }

                //List<string> tableNames = new List<string>
                //{
                //   "Vaccine"
                // , "VaccineGroup"
                // , "VaccineHx"
                // , "VaccineManufacturer"
                // , "VaccineRoute"
                // , "VaccineSite"
                // , "VaccineSourceOfHx"
                // , "VaccineVFC"
                // , "VaccineVIS"
                // , "VaccineVIS_URL"
                //};

                List<string> tableNames = new List<string>
                {
                   ds.Vaccine.TableName
                 , ds.VaccineGroup.TableName
                 , ds.VaccineHx.TableName
                 , ds.VaccineManufacturer.TableName
                 , ds.VaccineRoute.TableName
                 , ds.VaccineSite.TableName
                 , ds.VaccineSourceOfHx.TableName
                 , ds.VaccineVFC.TableName
                 , ds.VaccineVIS.TableName
                 , ds.VaccineVIS_URL.TableName
                };


                dbManager.Open();
                ds = (DSImmunizationHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GENERATE_HL7_IMMUNIZATION_MESSAGE, ds, tableNames);
                //   ds = (DSImmunizationHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Clinical.sp_Generate_HL7Immunization_Messagetest", ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::Generate_HL7Immunization_Message", PROC_GENERATE_HL7_IMMUNIZATION_MESSAGE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ImmunizationQueryModel> LoadVaccineHxById(string VaccineHxId)
        {
            List<ImmunizationQueryModel> QueryList = new List<ImmunizationQueryModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_VACCINE_HX_ID, VaccineHxId);
                QueryList = dbManager.ExecuteReaders<ImmunizationQueryModel>(PROC_VACCINEHX_SELECT_BY_ID);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::LoadVaccineHxById", PROC_VACCINEHX_SELECT_BY_ID, ex);
                throw ex;
            }
            finally
            {
            }
        }
        #endregion


        #region Lot Management

        public DSImmunization SearchVaccine(string searchText)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SEARCH_TEXT, !string.IsNullOrWhiteSpace(searchText) ? searchText : null);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SEARCH_ALL_VACCINE, ds, ds.SearchVaccine.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SearchVaccine", PROC_SEARCH_ALL_VACCINE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSImmunization SearchManufacturer(string searchText)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SEARCH_TEXT, !string.IsNullOrWhiteSpace(searchText) ? searchText : null);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SEARCH_ALL_MANUFACTURER, ds, ds.SearchManufacturer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SearchManufacturer", PROC_SEARCH_ALL_MANUFACTURER, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Category
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function Load Immunization Category
        public DSImmunization LoadImmunizationCategory(long vaccineGroupId, string shortName, string active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            bool isactive = true;
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "") active = null; else if (active == "1") isactive = true; else if (active == "0") isactive = false;

                //DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(6);

                if (vaccineGroupId == 0)
                    dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, vaccineGroupId);
                if (!string.IsNullOrEmpty(shortName))
                    dbManager.AddParameters(1, PARAM_SHORTNAME, shortName);
                else
                    dbManager.AddParameters(1, PARAM_SHORTNAME, DBNull.Value);

                if (string.IsNullOrEmpty(active))
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, isactive);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.Category.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CATEGORY_SELECT, ds, ds.Category.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationCategory", PROC_CATEGORY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function Insert Immunization Category
        public DSImmunization InsertImmunizationCategory(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForCategory(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CATEGORY_INSERT, ds, ds.Category.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertImmunizationCategory", PROC_CATEGORY_INSERT, ex);
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
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function Update Immunization Category
        public DSImmunization UpdateImmunizationCategory(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForCategory(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CATEGORY_UPDATE, ds, ds.Category.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateImmunizationCategory", PROC_CATEGORY_UPDATE, ex);
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
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function Delete Immunization Category
        public string DeleteImmunizationCategory(string vaccineGropId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VACCINE_GROUP_ID, vaccineGropId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteImmunizationCategory", PROC_CATEGORY_DELETE, ex);
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



        public string GetProviderId(long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PATIENT_PROVIDER_ID).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetProviderId", PROC_GET_PATIENT_PROVIDER_ID, ex);
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

        public string CheckPatientInsuranceIsMedicare(long PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CHECK_PATIENT_INSURANCE_IS_MEDICARE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::CheckPatientInsuranceIsMedicare", PROC_CHECK_PATIENT_INSURANCE_IS_MEDICARE, ex);
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

        public string IsLastAdministeredDoes(long vaccineHxId, long PatientId, bool VoidDose, long VaccineScheduleId, long VaccineID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, PARM_VACCINE_HX_ID, vaccineHxId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARAM_VOID_DOES, VoidDose);
                dbManager.AddParameters(3, PARAM_VACCINE_SCHEDULE_Id, VaccineScheduleId);
                dbManager.AddParameters(4, PARM_VACCINE_ID, VaccineID);
                dbManager.AddParameters(5, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_LAST_ADMINISTERED_DOES).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::IsLastAdministeredDoes", PROC_IS_LAST_ADMINISTERED_DOES, ex);
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

        public string IsAdministrationPeriodOver(long VaccineScheduleId, long PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARAM_VACCINE_SCHEDULE_Id, VaccineScheduleId);
                dbManager.AddParameters(2, PARM_IS_OVER, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_ADMINISTERED_PERIOD_OVER).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::IsAdministrationPeriodOver", PROC_IS_ADMINISTERED_PERIOD_OVER, ex);
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


        public string GetCptOfVaccine(long VaccineId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VACCINE_ID, VaccineId);
                dbManager.AddParameters(1, PARM_CPT, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_CPT_OF_VACCINE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetCptOfVaccine", PROC_GET_CPT_OF_VACCINE, ex);
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

        public string GetVaccineSchedulerId(long CategoryId, string ScheduleShortName)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_VACCINE_CATEGORY_ID, CategoryId);
                dbManager.AddParameters(1, PARAM_SCHEDULE_SHORT_NAME, ScheduleShortName);
                dbManager.AddParameters(2, PARAM_VACCINE_SCHEDULE_Id, "", DbType.Int64, ParamDirection.Output);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_ID).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccineSchedulerId", PROC_VACCINE_SCHEDULE_ID, ex);
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


        public string GetVaccineHxIds(long vaccineHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VACCINE_HX_ID, vaccineHxId);
                dbManager.AddParameters(1, PARM_VACCINE_HX_IDS, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VACCINE_HX_IDS).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccineHxIds", PROC_VACCINE_HX_IDS, ex);
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

        public string WhyLotIsNotAvailable(long vaccineId, long Provider, string Type)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_ID, vaccineId);
                dbManager.AddParameters(1, PARAM_PROVIDER_ID, Provider);
                dbManager.AddParameters(2, PARM_LOTISNOTAVAILABLE, "", DbType.String, ParamDirection.Output, null, 255);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(3, PARAM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                }
                dbManager.AddParameters(4, PARAM_TYPE, Type);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_WHY_LOT_IS_NOT_AVAILABLE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::WhyLotIsNotAvailable", PROC_WHY_LOT_IS_NOT_AVAILABLE, ex);
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



        public List<Manufacturer> GetLotManufanucture(long LotId)
        {
            //string returnVal = "";
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //try
            //{
            //    dbManager.Open();
            //    dbManager.CreateParameters(2);
            //    dbManager.AddParameters(0, PARAM_LOT_NO, LotId);
            //    dbManager.AddParameters(1, PARAM_VACCINE_MANUFACTURER_ID, "", DbType.String, ParamDirection.Output, null, 255);

            //    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_LOT_MANUFACTUREID).ToString();
            //    return returnVal;
            //}
            //catch (Exception ex)
            //{
            //    MDVLogger.DALErrorLog("DALImmunization::GetLotManufanucture", PROC_GET_LOT_MANUFACTUREID, ex);
            //    string[] str = ex.Message.Split('|');
            //    if (str.Length > 1)
            //        return str[1].ToString();
            //    else
            //        return ex.Message;
            //}
            //finally
            //{
            //    dbManager.Dispose();
            //}

            List<Manufacturer> ManufacturerList = new List<Manufacturer>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARAM_LOT_NO, LotId);
                ManufacturerList = dbManager.ExecuteReaders<Manufacturer>(PROC_GET_LOT_MANUFACTUREID);
                return ManufacturerList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetLotManufanucture", PROC_GET_LOT_MANUFACTUREID, ex);
                throw ex;
            }
            finally
            {
            }

        }

        public string GetProcedureIdsAgainstVaccAndImm(string VaccineHxIds, string ImmTherInjectionIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VaccineHxIds == "")
                {
                    dbManager.AddParameters(0, PARM_VACCINE_HX_ID, VaccineHxIds);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_VACCINE_HX_ID, VaccineHxIds);
                }
                if (ImmTherInjectionIds == "")
                {
                    dbManager.AddParameters(1, PARAM_IMM_THER_INJECTION_ID, ImmTherInjectionIds);
                }
                else
                {
                    dbManager.AddParameters(1, PARAM_IMM_THER_INJECTION_ID, ImmTherInjectionIds);
                }
                dbManager.AddParameters(2, PARM_PROCDURE_IDS, "", DbType.String, ParamDirection.Output, null, 1000);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROCEDUREIDS_AGAINST_VACCANDIMM).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetProcedureIdsAgainstVaccAndImm", PROC_GET_PROCEDUREIDS_AGAINST_VACCANDIMM, ex);
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


        /// Author: M Ahmad Imran.
        /// Date : 22-08-2016
        /// Purpose : Function Insert Or Update Patient Immunization Alert
        public string InsertOrUpdatePatientImmunizationAlert(long PatientId, bool IsVaccineInsert)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_IS_VACCINE_INSERT, IsVaccineInsert);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_IMMUNIZATION_ALERT_INSERT).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertOrUpdatePatientImmunizationAlert", PROC_PATIENT_IMMUNIZATION_ALERT_INSERT, ex);
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



        /// Author: M Ahmad Imran.
        /// Date : 19-08-2016
        /// Purpose : Function Get Immunization Alert Count
        public string Get_ImmunizationAlertCount(long patientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                dbManager.AddParameters(1, PARM_ALERT_COUNT, "", DbType.String, ParamDirection.Output, null, 255);

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_IMMUNIZATION_ALERT_COUNT).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::Get_ImmunizationAlertCount", PROC_GET_IMMUNIZATION_ALERT_COUNT, ex);
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

        public string GetProcedureIdAgainstImmTherapeuticInjectionId(long ImmTherInjectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_IMM_THER_INJECTION_ID, ImmTherInjectionId);
                dbManager.AddParameters(1, PARM_PROCDURE_ID, "", DbType.String, ParamDirection.Output, null, 255);

                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CATEGORY_DELETE).ToString();
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROCEDUREID_AGAINST_IMMTHERAPEUTICINJECTIONID).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetProcedureIdAgainstImmTherapeuticInjectionId", PROC_GET_PROCEDUREID_AGAINST_IMMTHERAPEUTICINJECTIONID, ex);
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
        //public DSImmunization LoadVaccineScheduleChart(long patientId, string ageLimit)
        //{
        //    DSImmunization ds = new DSImmunization();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();

        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, PARAM_AGE_LIMIT, string.IsNullOrWhiteSpace(ageLimit) ? null : ageLimit);
        //        dbManager.AddParameters(1, PARM_PATIENT_ID, (patientId == 0) ? null : (object)patientId);
        //        ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_CHART_SELECT, ds, ds.VaccineSchedule.TableName);


        #region Vaccine Schedule Charts




        #endregion

        #region Vaccine Crosswalk
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Load Immunization VaccineCrosswalk
        public DSImmunization LoadImmunizationVaccineCrosswalk(long VaccineCrosswalkId, long vaccineGroupId, long vaccineId, string active, string ISdefault, int PageNumber = 1, int RowsPerPage = 1000)
        {
            bool isactive = true;
            bool isdefault = true;
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "") active = null; else if (active == "True") isactive = true; else if (active == "False") isactive = false;
                if (ISdefault == "") ISdefault = null; else if (ISdefault == "1") isdefault = true; else if (ISdefault == "0") isdefault = false;
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (VaccineCrosswalkId == 0)
                    dbManager.AddParameters(0, PARAM_VACCINE_CROSSWALK_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARAM_VACCINE_CROSSWALK_ID, VaccineCrosswalkId);
                if (vaccineGroupId == 0)
                    dbManager.AddParameters(1, PARM_VACCINE_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_VACCINE_GROUP_ID, vaccineGroupId);

                if (vaccineId == 0)
                    dbManager.AddParameters(2, PARAM_VACCINE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARAM_VACCINE_ID, vaccineId);

                if (string.IsNullOrEmpty(active))
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, isactive);

                if (string.IsNullOrEmpty(ISdefault))
                    dbManager.AddParameters(4, PARM_IS_DEFAULT, null);
                else
                    dbManager.AddParameters(4, PARM_IS_DEFAULT, isdefault);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.VaccineCrosswalk.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_CROSSWALK_SELECT, ds, ds.VaccineCrosswalk.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationVaccineCrosswalk", PROC_VACCINE_CROSSWALK_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Insert Immunization VaccineCrosswalk
        public DSImmunization InsertImmunizationVaccineCrosswalk(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForVaccineCrosswalk(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VACCINE_CROSSWALK_INSERT, ds, ds.VaccineCrosswalk.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertImmunizationVaccineCrosswalk", PROC_VACCINE_CROSSWALK_INSERT, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Update Immunization VaccineCrosswalk
        public DSImmunization UpdateImmunizationVaccineCrosswalk(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForVaccineCrosswalk(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VACCINE_CROSSWALK_UPDATE, ds, ds.VaccineCrosswalk.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateImmunizationVaccineCrosswalk", PROC_VACCINE_CROSSWALK_UPDATE, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Delete Immunization VaccineCrosswalk
        public string DeleteImmunizationVaccineCrosswalk(string VaccineCrosswalkId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_VACCINE_CROSSWALK_ID, VaccineCrosswalkId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VACCINE_CROSSWALK_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteImmunizationVaccineCrosswalk", PROC_VACCINE_CROSSWALK_DELETE, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Parameters For VaccineCrosswalk
        private void CreateParametersForVaccineCrosswalk(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARAM_VACCINE_CROSSWALK_ID, ds.VaccineCrosswalk.VaccineCrosswalkIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARAM_VACCINE_CROSSWALK_ID, ds.VaccineCrosswalk.VaccineCrosswalkIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_VACCINE_GROUP_ID, ds.VaccineCrosswalk.VaccineGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARAM_VACCINE_ID, ds.VaccineCrosswalk.VaccineIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.VaccineCrosswalk.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_IS_DEFAULT, ds.VaccineCrosswalk.IsDefaultColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARAM_CREATED_BY, ds.VaccineCrosswalk.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARAM_CREATED_ON, ds.VaccineCrosswalk.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARAM_MODIFIED_BY, ds.VaccineCrosswalk.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARAM_MODIFIED_ON, ds.VaccineCrosswalk.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region Vaccine
        /// Author: Azeem Raza Tayyab
        /// Date : 22-07-2016
        /// Purpose : Function Load Vaccines
        public DSImmunization LoadVaccineList(long vaccineId, long vaccineGroupId, string VaccineStatus, string VCVXShortDes, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (vaccineId == 0)
                    dbManager.AddParameters(0, PARAM_VACCINE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARAM_VACCINE_ID, vaccineId);

                if (vaccineGroupId == 0)
                    dbManager.AddParameters(1, PARM_VACCINE_GROUP_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_VACCINE_GROUP_ID, vaccineGroupId);

                if (string.IsNullOrEmpty(VaccineStatus))
                    dbManager.AddParameters(2, PARM_VACCINE_STATUS, null);
                else
                    dbManager.AddParameters(2, PARM_VACCINE_STATUS, VaccineStatus);

                if (string.IsNullOrEmpty(VaccineStatus))
                    dbManager.AddParameters(3, PARM_CVX_SHORT_DESCRIPTION, null);
                else
                    dbManager.AddParameters(3, PARM_CVX_SHORT_DESCRIPTION, VCVXShortDes);

                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.VaccineCrosswalk.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SELECT, ds, ds.VaccineList.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadVaccineList", PROC_VACCINE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Schedule Setup
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Load Immunization ScheduleSetup
        public DSImmunization LoadImmunizationScheduleSetup(string ScheduleTypeId, long VaccineScheduleId, string ScheduleId, string CategoryId, string active)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                bool IsActive = false;
                if (active == "") active = null; else if (active == "True" || active == "1") IsActive = true; else if (active == "False" || active == "0") IsActive = false;
                dbManager.Open();
                dbManager.CreateParameters(6);
                if (string.IsNullOrEmpty(ScheduleTypeId))
                    dbManager.AddParameters(0, PARAM_SCHEDULE_TYPE_ID, null);
                else
                    dbManager.AddParameters(0, PARAM_SCHEDULE_TYPE_ID, ScheduleTypeId);
                if (VaccineScheduleId == 0)
                    dbManager.AddParameters(1, PARAM_VACCINE_SCHEDULE_Id, null);
                else
                    dbManager.AddParameters(1, PARAM_VACCINE_SCHEDULE_Id, VaccineScheduleId);
                if (string.IsNullOrEmpty(ScheduleId))
                    dbManager.AddParameters(2, PARAM_SCHEDULE_ID, null);
                else
                    dbManager.AddParameters(2, PARAM_SCHEDULE_ID, ScheduleId);
                if (string.IsNullOrEmpty(CategoryId))
                    dbManager.AddParameters(3, PARM_VACCINE_GROUP_ID, null);
                else
                    dbManager.AddParameters(3, PARM_VACCINE_GROUP_ID, CategoryId);
                if (string.IsNullOrEmpty(active))
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, IsActive);


                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.VaccineSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_SELECT, ds, ds.VaccineSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: M Ahmad Imran
        //Date : 26-08-2016
        //Purpose : Function Load Immunization ScheduleSetup For Sort
        public DSImmunization LoadImmunizationScheduleSetupForSort(long VaccineScheduleId)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARAM_VACCINE_SCHEDULE_Id, VaccineScheduleId);



                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_SELECT_FOR_SORT, ds, ds.VaccineSchedule.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Insert Immunization ScheduleSetup
        public DSImmunization InsertImmunizationScheduleSetup(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForScheduleSetup(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_INSERT, ds, ds.VaccineSchedule.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_INSERT, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Update Immunization ScheduleSetup
        public DSImmunization UpdateImmunizationScheduleSetup(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForScheduleSetup(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_UPDATE, ds, ds.VaccineSchedule.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_UPDATE, ex);
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

        /// Author: M Ahmad Imran
        /// Date : 26-08-2016
        /// Purpose : Function Update Immunization Schedule Setup For Sort
        public DSImmunization UpdateImmunizationScheduleSetupForSort(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARAM_VACCINE_SCHEDULE_Id, ds.VaccineSchedule.VaccineScheduleIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARAM_VACCINE_PRIORITY, ds.VaccineSchedule.PriorityColumn.ColumnName, DbType.Int32);

                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_UPDATE_FOR_SORT, ds, ds.VaccineSchedule.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_UPDATE_FOR_SORT, ex);
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

        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Delete Immunization ScheduleSetup
        public string DeleteImmunizationScheduleSetup(string ScheduleSetupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_VACCINE_SCHEDULE_Id, ScheduleSetupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VACCINE_SCHEDULE_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteImmunizationScheduleSetup", PROC_VACCINE_SCHEDULE_DELETE, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 21-07-2016
        /// Purpose : Function Parameters For ScheduleSetup
        private void CreateParametersForScheduleSetup(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARAM_VACCINE_SCHEDULE_Id, ds.VaccineSchedule.VaccineScheduleIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARAM_VACCINE_SCHEDULE_Id, ds.VaccineSchedule.VaccineScheduleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARAM_SCHEDULE_ID, ds.VaccineSchedule.ScheduleIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_VACCINE_GROUP_ID, ds.VaccineSchedule.VaccineGroupIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARAM_SCHEDULE_TYPE_ID, ds.VaccineSchedule.ScheduleTypeIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARAM_START_DUE_DATE, ds.VaccineSchedule.StartDueDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARAM_END_OVERDUE_DATE, ds.VaccineSchedule.EndOverDueDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARAM_MALE_MAXAGE, ds.VaccineSchedule.MaleMaxAgeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARAM_FEMALE_MAXAGE, ds.VaccineSchedule.FemaleMaxAgeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.VaccineSchedule.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(9, PARM_PATIENT_ID, ds.VaccineSchedule.PatientIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region Schedule Setup
        /// Author: Azeem Raza Tayyab
        /// Date : 28-07-2016
        /// Purpose : Function Load Immunization LotNumber
        public DSImmunization LoadImmunizationLotNumber(long VaccineLotNoId, string active, int PageNumber = 0, int RowsPerPage = 100, long VaccineId = 0, string Type = "", int TherapueticInjectionId = 0, Int64 ProviderId = 00, bool OnlyExpired = false, bool OnlyLowQuantity = false)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                bool IsActive = false;
                if (active == "") active = null; else if (active == "True" || active == "1") IsActive = true; else if (active == "False" || active == "0") IsActive = false;
                dbManager.Open();
                dbManager.CreateParameters(12);
                if (VaccineLotNoId == 0)
                    dbManager.AddParameters(0, PARAM_LOT_NUMBER_ID, null);
                else
                    dbManager.AddParameters(0, PARAM_LOT_NUMBER_ID, VaccineLotNoId);
                if (string.IsNullOrEmpty(active))
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.VaccineSchedule.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (VaccineId == 0)
                    dbManager.AddParameters(5, PARM_VACCINE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(5, PARM_VACCINE_ID, VaccineId);
                if (Type == "")
                    dbManager.AddParameters(6, PARM_TYPE, DBNull.Value);
                else
                    dbManager.AddParameters(6, PARM_TYPE, Type);
                if (TherapueticInjectionId == 0)
                    dbManager.AddParameters(7, PARAM_THERAPEUTIC_INJECTION_ID, DBNull.Value);
                else
                    dbManager.AddParameters(7, PARAM_THERAPEUTIC_INJECTION_ID, TherapueticInjectionId);

                if (ProviderId == 0)
                    dbManager.AddParameters(8, PARAM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(8, PARAM_PROVIDER_ID, ProviderId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(9, PARAM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                }
                dbManager.AddParameters(10, PARAM_ONLY_EXPIRED, OnlyExpired);
                dbManager.AddParameters(11, PARAM_ONLY_LOWQUANTITY, OnlyLowQuantity);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_LOT_NUMBER_SELECT, ds, ds.VaccineLotNo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImmunizationLotNumber", PROC_IMMUNIZATION_LOT_NUMBER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// Author: Azeem Raza Tayyab
        /// Date : 28-07-2016
        /// Purpose : Function Insert Immunization LotNumber
        public DSImmunization InsertImmunizationLotNumber(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForLotNumber(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_LOT_NUMBER_INSERT, ds, ds.VaccineLotNo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertImmunizationLotNumber", PROC_IMMUNIZATION_LOT_NUMBER_INSERT, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 28-07-2016
        /// Purpose : Function Update Immunization LotNumber
        public DSImmunization UpdateImmunizationLotNumber(DSImmunization ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersForLotNumber(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_IMMUNIZATION_LOT_NUMBER_UPDATE, ds, ds.VaccineLotNo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateImmunizationLotNumber", PROC_IMMUNIZATION_LOT_NUMBER_UPDATE, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 28-07-2016
        /// Purpose : Function Delete Immunization LotNumber
        public string DeleteImmunizationLotNumber(string LotNumberId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_LOT_NUMBER_ID, LotNumberId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMMUNIZATION_LOT_NUMBER_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteImmunizationLotNumber", PROC_IMMUNIZATION_LOT_NUMBER_DELETE, ex);
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
        /// Author: Azeem Raza Tayyab
        /// Date : 28-07-2016
        /// Purpose : Function Parameters For LotNumber
        private void CreateParametersForLotNumber(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARAM_LOT_NUMBER_ID, ds.VaccineLotNo.VaccineLotNoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARAM_LOT_NUMBER_ID, ds.VaccineLotNo.VaccineLotNoIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARAM_VACCINE_IDS, ds.VaccineLotNo.LotVaccineIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARAM_LOT_NO, ds.VaccineLotNo.LotNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARAM_VACCINE_MANUFACTURER_ID, ds.VaccineLotNo.ManufacturerIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARAM_ROUTE_ID, ds.VaccineLotNo.RouteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARAM_VISDATE, ds.VaccineLotNo.VISDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARAM_EXPIRY_DATE, ds.VaccineLotNo.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARAM_NDC_CODE, ds.VaccineLotNo.NDCCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARAM_QUANTITY, ds.VaccineLotNo.QuantityColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARAM_QUANTITY_LEFT, ds.VaccineLotNo.QuantityLeftColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARAM_ACTIVE, ds.VaccineLotNo.ActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARAM_CREATED_BY, ds.VaccineLotNo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARAM_CREATED_ON, ds.VaccineLotNo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARAM_MODIFIED_BY, ds.VaccineLotNo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARAM_MODIFIED_ON, ds.VaccineLotNo.ModifiedOnColumn.ColumnName, DbType.DateTime);
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
            {
                dbManager.AddParameters(15, PARAM_ENTITY_ID, ds.VaccineLotNo.EntityIdColumn.ColumnName, DbType.Int64);
            }
            else
            {
                dbManager.AddParameters(15, PARAM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
            }
            dbManager.AddParameters(16, PARAM_THERAPEUTIC_INJECTION_ID, ds.VaccineLotNo.TherapeuticInjectionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(17, PARAM_PROVIDER_IDS, ds.VaccineLotNo.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARAM_FUNDING_SOURCE_ID, ds.VaccineLotNo.VaccineFundingSourceIdColumn.ColumnName, DbType.Int64);
        }
        public DSImmunization LookupManufacturer()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MANUFACTURER_LOOKUP, ds, ds.LookupManufacturer.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupManufacturer", PROC_MANUFACTURER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Immunization With T Type

        public List<VaccineSoapModel> LoadVaccine_ForSoapTest(long PatientId, long UserId, long EntityId, long NoteId)
        {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                // List<SqlParameter> parameters = new List<SqlParameter>();

                //parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                // parameters.Add(new SqlParameter(PARAM_USERID, UserId));
                dbManager.AddParameters(PARAM_USERID, UserId);
                // parameters.Add(new SqlParameter(PARAM_ENTITY_ID, EntityId));
                dbManager.AddParameters(PARAM_ENTITY_ID, EntityId);

                List<VaccineSoapModel> modelList = dbManager.ExecuteReaders<VaccineSoapModel>(PROC_VACCINEHX_SELECT_FORSOAPTEXT);

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LoadVaccine_ForSoapTest", PROC_VACCINEHX_SELECT_FORSOAPTEXT, ex);
                throw ex;
            }
        }

        public List<TherapeuticInjectionSoapModel> LoadImmTherapeuticInjectionForSoapText(long PatientId, long UserId, long EntityId, long NoteId)
        {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                //  List<SqlParameter> parameters = new List<SqlParameter>();

                // parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //  parameters.Add(new SqlParameter(PARAM_USERID, UserId));
                dbManager.AddParameters(PARAM_USERID, UserId);
                //   parameters.Add(new SqlParameter(PARAM_ENTITY_ID, EntityId));
                dbManager.AddParameters(PARAM_ENTITY_ID, EntityId);

                List<TherapeuticInjectionSoapModel> modelList = dbManager.ExecuteReaders<TherapeuticInjectionSoapModel>(PROC_GET_THERAPEUT_ICINJECTION_SELECT_FORSOAPTEXT);

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LoadImmTherapeuticInjectionForSoapText", PROC_GET_THERAPEUT_ICINJECTION_SELECT_FORSOAPTEXT, ex);
                throw ex;
            }
        }

        public List<VaccineSoapModel> LoadVaccine(string VaccineHxIds, long PatientId, long UserId, long EntityId, long OrderSetId)
        {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                //    List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(VaccineHxIds))
                    //parameters.Add(new SqlParameter(PARAM_VACCINE_HX_IDS, VaccineHxIds));
                    dbManager.AddParameters(PARAM_VACCINE_HX_IDS, VaccineHxIds);

                else
                    //parameters.Add(new SqlParameter(PARAM_VACCINE_HX_IDS, DBNull.Value));
                    dbManager.AddParameters(PARAM_VACCINE_HX_IDS, DBNull.Value);

                //parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                if (UserId > 0)
                    //parameters.Add(new SqlParameter(PARAM_USERID, UserId));
                    dbManager.AddParameters(PARAM_USERID, UserId);
                else
                    //parameters.Add(new SqlParameter(PARAM_USERID, DBNull.Value));
                    dbManager.AddParameters(PARAM_USERID, DBNull.Value);

                //parameters.Add(new SqlParameter(PARAM_ENTITY_ID, EntityId));
                dbManager.AddParameters(PARAM_ENTITY_ID, EntityId);

                if (OrderSetId > 0)                    
                    dbManager.AddParameters(PARAM_ORDERSET_ID, OrderSetId);
                else                  
                    dbManager.AddParameters(PARAM_ORDERSET_ID, DBNull.Value);

                List<VaccineSoapModel> modelList = dbManager.ExecuteReaders<VaccineSoapModel>(PROC_VACCINEHx_SELECT);

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LoadVaccine_ForSoapTest", PROC_VACCINEHx_SELECT, ex);
                throw ex;
            }
        }

        public List<TherapeuticInjectionSoapModel> LoadImmTherapeuticInjectionForSoapText(string TheraInjectionIds, long PatientId, long UserId, long EntityId, long OrderSetId)
        {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                ///   List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(TheraInjectionIds))
                    //parameters.Add(new SqlParameter(PARAM_IMM_THER_INJECTION_ID, TheraInjectionIds));
                    dbManager.AddParameters(PARAM_IMM_THER_INJECTION_ID, TheraInjectionIds);
                else
                    //parameters.Add(new SqlParameter(PARAM_IMM_THER_INJECTION_ID, DBNull.Value));
                    dbManager.AddParameters(PARAM_IMM_THER_INJECTION_ID, null);


                //parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //parameters.Add(new SqlParameter(PARAM_USERID, UserId));
                dbManager.AddParameters(PARAM_USERID, UserId);
                //parameters.Add(new SqlParameter(PARAM_ENTITY_ID, EntityId));
                dbManager.AddParameters(PARAM_ENTITY_ID, EntityId);

                if (OrderSetId > 0)
                {
                    dbManager.AddParameters(PARAM_ORDERSET_ID, OrderSetId);
                }
                else
                {
                    dbManager.AddParameters(PARAM_ORDERSET_ID, DBNull.Value); 
                }

                List<TherapeuticInjectionSoapModel> modelList = dbManager.ExecuteReaders<TherapeuticInjectionSoapModel>(PROC_GET_THERAPEUT_ICINJECTION_SELECT);

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LoadImmTherapeuticInjectionForSoapText", PROC_GET_THERAPEUT_ICINJECTION_SELECT, ex);
                throw ex;
            }
        }

        #endregion Immunization With Data Model
        #region Add Vaccine and Therapeutic
        public DSImmunization LookupVaccineAmount()
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VACCINE_AMOUNT_LOOKUP, ds, ds.VaccineReactionLookups.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LookupVaccineAmount", PROC_VACCINE_AMOUNT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<VaccineAndTerapuetic> GetImmAndTheraArray(string ImmunizationName, int Type, bool CptBaseSearch = false)
        {
            List<VaccineAndTerapuetic> ImmAndTheralList = new List<VaccineAndTerapuetic>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Type == 0)
                {
                    dbManager.AddParameters(PARM_TYPE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_TYPE, Type);
                }
                dbManager.AddParameters(PARM_IMMUNIZATION_NAME, ImmunizationName);
                dbManager.AddParameters(PARM_CPT_BASE_SEARCH, CptBaseSearch);
                ImmAndTheralList = dbManager.ExecuteReaders<VaccineAndTerapuetic>(PROC_GET_VACCINE_OR_THERA_NAME);
                return ImmAndTheralList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetImmAndTheraArray", PROC_GET_VACCINE_OR_THERA_NAME, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<Manufacturer> GetManufacturerArray(string ManufacturerName, string VaccineId = "", string TherapeuticId = "")
        {
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_MANUFACTURER_NAME, ManufacturerName);
                if (VaccineId == "")
                {
                    dbManager.AddParameters(PARAM_VACCINE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARAM_VACCINE_ID, VaccineId, DbType.Int64);
                }
                if (TherapeuticId == "")
                {
                    dbManager.AddParameters(PARAM_THERAPEUTIC_INJECTION_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARAM_THERAPEUTIC_INJECTION_ID, TherapeuticId, DbType.Int64);
                }

                ManufacturerList = dbManager.ExecuteReaders<Manufacturer>(PROC_GET_MANUFACTURER_NAME);
                return ManufacturerList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetManufacturerArray", PROC_GET_MANUFACTURER_NAME, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<Manufacturer> LoadManufacturer(string ManufacturerName, string MVXCode, string Status, Int64 PageNumber, Int64 RowsPerPage)
        {
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (MVXCode == "")
                {
                    dbManager.AddParameters(PARM_MVX_CODE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_MVX_CODE, MVXCode);
                }
                if (Status == "")
                {
                    dbManager.AddParameters(PARM_STATUS, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_STATUS, Status);
                }
                if (ManufacturerName == "")
                {
                    dbManager.AddParameters(PARM_MANUFACTURER_NAME, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_MANUFACTURER_NAME, ManufacturerName);
                }
                dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.Int64, ParamDirection.Output);
                ManufacturerList = dbManager.ExecuteReaders<Manufacturer>(PROC_LOAD_MANUFACTURER);
                return ManufacturerList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadManufacturer", PROC_LOAD_MANUFACTURER, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public List<VaccineAndTerapuetic> loadVaccineAndTherapeutic(string ImmunizationId, int Type, string Status, Int64 PageNumber, Int64 RowsPerPage)
        {
            List<VaccineAndTerapuetic> ImmAndTheralList = new List<VaccineAndTerapuetic>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Type == 0)
                {
                    dbManager.AddParameters(PARM_TYPE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_TYPE, Type);
                }
                if (Status == "")
                {
                    dbManager.AddParameters(PARM_STATUS, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_STATUS, Status);
                }
                if (ImmunizationId == "")
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_Id, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_Id, MDVUtility.ToInt64(ImmunizationId));
                }
                dbManager.AddParameters(PARM_PAGE_NUMBER, PageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.Int64, ParamDirection.Output);
                ImmAndTheralList = dbManager.ExecuteReaders<VaccineAndTerapuetic>(PROC_LOAD_VACCINE_OR_THERA);
                return ImmAndTheralList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadVaccineAndTherapeutic", PROC_LOAD_VACCINE_OR_THERA, ex);
                throw ex;
            }
            finally
            {
            }
        }


        public List<VaccineAndTerapuetic> GetVaccineInformationForAutoPopu(string Id, string type)
        {
            List<VaccineAndTerapuetic> ImmAndTheralList = new List<VaccineAndTerapuetic>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_ID, Id);
                dbManager.AddParameters(PARM_TYPE, type);
                ImmAndTheralList = dbManager.ExecuteReaders<VaccineAndTerapuetic>(PROC_LOAD_VACCINE_INFO_FOR_AUTO_POPULATION);
                return ImmAndTheralList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetVaccineInformationForAutoPopu", PROC_LOAD_VACCINE_INFO_FOR_AUTO_POPULATION, ex);
                throw ex;
            }
            finally
            {
            }
        }



        public List<Manufacturer> LoadManufacturerDetail(Int64 ManufacturerId)
        {
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_MANUFACTURER_ID, ManufacturerId);
                ManufacturerList = dbManager.ExecuteReaders<Manufacturer>(PROC_LOAD_MANUFACTURER_DETAIL);
                return ManufacturerList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImunization::LoadManufacturerDetail", PROC_LOAD_MANUFACTURER_DETAIL, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public VaccineAndTerapuetic LoadVaccineDetail(Int64 ImmunizationId)
        {



            VaccineAndTerapuetic VaccineDetail = new VaccineAndTerapuetic();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VACCINE_ID, ImmunizationId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LOAD_VACCINE_DETAIL);


                while (reader.Read())
                {
                    VaccineDetail.ImmunizationName = Convert.ToString(reader["ImmunizationName"]);
                    VaccineDetail.CVX = Convert.ToString(reader["CVX"]);
                    VaccineDetail.NDCCode = Convert.ToString(reader["NDCCode"]);
                    VaccineDetail.Dose = Convert.ToString(reader["Dose"]);
                    VaccineDetail.Amount = Convert.ToString(reader["Amount"]);
                    VaccineDetail.Status = Convert.ToString(reader["Status"]);
                    VaccineDetail.ManufactureIds = Convert.ToString(reader["ManufactureIds"]);
                    VaccineDetail.CPTCode = Convert.ToString(reader["CPTCode"]);
                    VaccineDetail.CPTDescription = Convert.ToString(reader["CPTDescription"]);
                    VaccineDetail.AdminCode = Convert.ToString(reader["AdminCode"]);
                    VaccineDetail.AdminCodeDescription = Convert.ToString(reader["AdminCodeDescription"]);
                }
                reader.NextResult();
                VaccineDetail.VaccineVisInformation = new List<VaccineVIS>();
                while (reader.Read())
                {
                    VaccineVIS VaccineVis = new VaccineVIS();

                    VaccineVis.VaccineVIS_URLId = Convert.ToString(reader["VaccineVIS_URLId"]);
                    VaccineVis.VaccineVISId = Convert.ToString(reader["VaccineVISId"]);
                    VaccineVis.VISDate = Convert.ToString(reader["VISDate"]);
                    VaccineVis.VISDocumentName = Convert.ToString(reader["VISDocumentName"]);
                    VaccineVis.VISDocumentLink = Convert.ToString(reader["VISDocumentLink"]);
                    VaccineVis.VISFullyEncodedText = Convert.ToString(reader["VISFullyEncodedText"]);

                    VaccineDetail.VaccineVisInformation.Add(VaccineVis);
                }


                return VaccineDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadVaccineDetail", PROC_LOAD_VACCINE_DETAIL, ex);
                throw ex;
            }
        }


        public List<VaccineAndTerapuetic> LoadTherapeuticDetail(int TherapeuticId)
        {
            List<VaccineAndTerapuetic> ImmAndTheralList = new List<VaccineAndTerapuetic>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_THERAPEUTIC_ID, TherapeuticId);
                ImmAndTheralList = dbManager.ExecuteReaders<VaccineAndTerapuetic>(PROC_LOAD_THERAPEUTIC_DETAIL);
                return ImmAndTheralList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadTherapeuticDetail", PROC_LOAD_THERAPEUTIC_DETAIL, ex);
                throw ex;
            }
            finally
            {
            }
        }



        public string SaveManufacturer(Manufacturer model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createManufacturerParameters(dbManager, model, true);
                var ManufacturerId = dbManager.ExecuteScalar(PROC_MANUFACTURER_INSERT);
                return MDVUtility.ToStr(ManufacturerId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveManufacturer", PROC_MANUFACTURER_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveVaccine(VaccineAndTerapuetic model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createVaccineParameters(dbManager, model, true);
                var VaccineId = dbManager.ExecuteScalar(PROC_VACCINE_INSERT);
                return MDVUtility.ToStr(VaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveVaccine", PROC_VACCINE_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string SaveTherapeutic(VaccineAndTerapuetic model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createTherapeuticeParameters(dbManager, model, true);
                var TherpeuticId = dbManager.ExecuteScalar(PROC_THERAPEUTIC_INSERT);
                return MDVUtility.ToStr(TherpeuticId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveTherapeutic", PROC_THERAPEUTIC_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveVaccineVIS(VaccineVIS model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createVaccineVISParameters(dbManager, model, true);
                var FavVaccineId = dbManager.ExecuteScalar(PROC_VACCINE_VIS_INSERT);
                return MDVUtility.ToStr(FavVaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveVaccineVIS", PROC_VACCINE_VIS_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string ActiveOrInactiveManufacturer(Manufacturer model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_MANUFACTURER_ID, model.ManufacturerId, DbType.Int64);
                dbManager.AddParameters(PARM_STATUS, model.Status);

                var VaccineId = dbManager.ExecuteScalar(PROC_ACTIVEINACTIVE_MANUFACTURER);
                return MDVUtility.ToStr(VaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::ActiveOrInactiveManufacturer", PROC_ACTIVEINACTIVE_MANUFACTURER, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string ActiveOrInactiveImmOrThera(VaccineAndTerapuetic model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_ID, model.Id);
                dbManager.AddParameters(PARAM_TYPE, model.Type);
                dbManager.AddParameters(PARM_STATUS, model.Status, DbType.Byte);

                var VaccineId = dbManager.ExecuteScalar(PROC_ACTIVEINACTIVE_IMMORTHERA);
                return MDVUtility.ToStr(VaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::ActiveOrInactiveImmOrThera", PROC_ACTIVEINACTIVE_IMMORTHERA, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string UpdateManufacturer(Manufacturer model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createManufacturerParameters(dbManager, model, false);
                var VaccineId = dbManager.ExecuteScalar(PROC_MANUFACTURER_UPDATE);
                return MDVUtility.ToStr(VaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateManufacturer", PROC_MANUFACTURER_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateVaccine(VaccineAndTerapuetic model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createVaccineParameters(dbManager, model, false);
                var VaccineId = dbManager.ExecuteScalar(PROC_VACCINE_UPDATE);
                return MDVUtility.ToStr(VaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateVaccine", PROC_VACCINE_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdateTherapeutic(VaccineAndTerapuetic model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createTherapeuticeParameters(dbManager, model, false);
                var TherapeuticId = dbManager.ExecuteScalar(PROC_THERAPEUTIC_UPDATE);
                return MDVUtility.ToStr(TherapeuticId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateTherapeutic", PROC_THERAPEUTIC_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateVaccineVis(VaccineVIS model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createUpdateVaccineVisParameters(dbManager, model, false);
                var FavVaccineId = dbManager.ExecuteScalar(PROC_VACCINE_VIS_AND_URL_UPDATE);
                return MDVUtility.ToStr(FavVaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateVaccine", PROC_VACCINE_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteImmunization(long ImmunizationId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARAM_VACCINE_ID, ImmunizationId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var dbCallId = dbManager.ExecuteScalar(PROC_IMMUNIZATION_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::Deletemmunization", PROC_IMMUNIZATION_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteVISInformation(long VISInformationId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_VACCINE_VIS_ID, VISInformationId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var dbCallId = dbManager.ExecuteScalar(PROC_VIS_INFORMATION_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteVISInformation", PROC_VIS_INFORMATION_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string DeleteManufacturer(long ManufacturerId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_MANUFACTURER_ID, ManufacturerId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var dbCallId = dbManager.ExecuteScalar(PROC_MANUFACTURER_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteManufacturer", PROC_MANUFACTURER_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteTherapeutic(long TherapeuticId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_THERAPEUTIC_ID, TherapeuticId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var dbCallId = dbManager.ExecuteScalar(PROC_THERAPEUTIC_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteTherapeutic", PROC_THERAPEUTIC_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Legacy Notes

        public List<ImmunizationHx> NotesImmunizationHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<ImmunizationHx> objList_ImmunizationHx = new List<ImmunizationHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_IMMUNIZATION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        ImmunizationHx model = new ImmunizationHx();
                        var properties = typeof(ImmunizationHx).GetProperties();
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
                        objList_ImmunizationHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::NotesImmunizationHxSelect", PROC_NOTES_IMMUNIZATION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_ImmunizationHx;
        }

        #endregion Legacy Notes

        #region Register HL7
        public DSImmunizationHL7 LoadImunizationHL7Registery(string RegistryConfigurationId, string ProviderId = null, string RegisteryId = null, bool IsActive = true, int PageNumber = 0, int RowsPerPage = 100)
        {
            DSImmunizationHL7 ds = new DSImmunizationHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (String.IsNullOrEmpty(ProviderId))
                    dbManager.AddParameters(0, PARAM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARAM_PROVIDER_ID, MDVUtility.ToInt64(ProviderId));
                if (String.IsNullOrEmpty(RegisteryId))
                    dbManager.AddParameters(1, PARAM_RECEIVING_APPLICATION_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARAM_RECEIVING_APPLICATION_ID, MDVUtility.ToInt64(RegisteryId));
                if (String.IsNullOrEmpty(RegistryConfigurationId))
                    dbManager.AddParameters(2, PARAM_REGISTRY_CONFIGURATION_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARAM_REGISTRY_CONFIGURATION_ID, MDVUtility.ToInt64(RegistryConfigurationId));

                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_PAGE_NUM, PageNumber);
                dbManager.AddParameters(5, PARM_ROWS_PERPAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.RegistryConfiguration.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSImmunizationHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REGISTERY_CONFIGURATION_SELECT, ds, ds.RegistryConfiguration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::LoadImunizationHL7Registery", PROC_REGISTERY_CONFIGURATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string InsertHL7Registery(ImmunizationRegisteryWrapperModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARAM_REGISTRY_CONFIGURATION_ID, model.RegistryConfigurationId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(PARAM_PROVIDER_ID, model.ProviderId);
                dbManager.AddParameters(PARAM_SENDING_APPLICATION, model.SendingApplication);
                dbManager.AddParameters(PARAM_RECEIVING_APPLICATION_ID, model.ReceivingApplicationId);
                dbManager.AddParameters(PARAM_PROVIDER_FACILITY_ID, model.PoviderFacilityId);
                dbManager.AddParameters(PARAM_SENDING_FACILITY, model.SendingFacility);
                dbManager.AddParameters(PARAM_REGISTERY_SUBMISSION_ID, model.RegistrySubmissionId);
                dbManager.AddParameters(PARAM_TIME_SLOT, model.Timeslot);
                dbManager.AddParameters(PARAM_IS_ADMINISTERED, model.IsAdministered);
                dbManager.AddParameters(PARAM_IS_HISTORY_DOSE, model.IsHistoryDose);
                dbManager.AddParameters(PARAM_IS_REFUSAL, model.IsRefusal);
                dbManager.AddParameters(PARM_ISDELETED, model.IsDeleted);
                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(PARM_STATUS, model.Status);
                dbManager.AddParameters(PARAM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(PARAM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(PARAM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(PARAM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARAM_ENTITY_ID, model.EntityId);
                dbManager.AddParameters(PARAM_FILES_PER_BATCH, model.FilesPerBatch);

                var returnValue = dbManager.ExecuteScalar(PROC_REGISTERY_CONFIGURATION_INSERT);
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::InsertHL7Registery", PROC_REGISTERY_CONFIGURATION_INSERT, ex);
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
        public string UpdateHL7Registery(ImmunizationRegisteryWrapperModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.AddParameters(PARAM_REGISTRY_CONFIGURATION_ID, model.RegistryConfigurationId);
                dbManager.AddParameters(PARAM_PROVIDER_ID, model.ProviderId);
                dbManager.AddParameters(PARAM_SENDING_APPLICATION, model.SendingApplication);
                dbManager.AddParameters(PARAM_RECEIVING_APPLICATION_ID, model.ReceivingApplicationId);
                dbManager.AddParameters(PARAM_PROVIDER_FACILITY_ID, model.PoviderFacilityId);
                dbManager.AddParameters(PARAM_SENDING_FACILITY, model.SendingFacility);
                dbManager.AddParameters(PARAM_REGISTERY_SUBMISSION_ID, model.RegistrySubmissionId);
                dbManager.AddParameters(PARAM_TIME_SLOT, model.Timeslot);
                dbManager.AddParameters(PARAM_IS_ADMINISTERED, model.IsAdministered);
                dbManager.AddParameters(PARAM_IS_HISTORY_DOSE, model.IsHistoryDose);
                dbManager.AddParameters(PARAM_IS_REFUSAL, model.IsRefusal);
                dbManager.AddParameters(PARM_ISDELETED, model.IsDeleted);
                dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(PARM_STATUS, model.Status);
                dbManager.AddParameters(PARAM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(PARAM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(PARAM_ENTITY_ID, model.EntityId);
                dbManager.AddParameters(PARAM_FILES_PER_BATCH, model.FilesPerBatch);

                var returnValue = dbManager.ExecuteScalar(PROC_REGISTERY_CONFIGURATION_UPDATE);
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateHL7Registery", PROC_REGISTERY_CONFIGURATION_UPDATE, ex);
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
        public string DeleteHL7Registery(string RegistryConfigurationId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARAM_REGISTRY_CONFIGURATION_ID, MDVUtility.ToInt64(RegistryConfigurationId));
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var returnValue = dbManager.ExecuteScalar(PROC_REGISTERY_CONFIGURATION_DELETE);
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::DeleteHL7Registery", PROC_REGISTERY_CONFIGURATION_DELETE, ex);
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

        #region Immunization Query
        public string SendQuery(ImmunizationQueryModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createSendQueryParameters(dbManager, model, true);
                var QueryId = MDVUtility.ToStr(dbManager.ExecuteScalar(PROC_QUERY_IMMUNIZATION_INSERT));
                if (QueryId != null)
                {
                    return QueryId;
                }
                else
                {
                    throw new Exception("error found");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SendQuery", PROC_QUERY_IMMUNIZATION_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string AddToHxTab(ImmunizationQueryResponseModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_EVALUATED_IMMUNIZATION_HISTORY_IDS, model.EvaluatedImmunizationHistoryIds);
                dbManager.AddParameters(PARM_PATIENT_ID, model.PatientId);
                dbManager.AddParameters(PARM_CREATEDON, model.CreatedOn);
                dbManager.AddParameters(PARM_CREATEDBY, model.CreatedBy);
                dbManager.AddParameters(PARM_MODIFIEDBY, model.ModifiedBy);
                dbManager.AddParameters(PARM_MODIFIEDON, model.ModifiedOn);
                dbManager.AddParameters(PARAM_USERID, model.UserId);
                var VaccineHxId = MDVUtility.ToStr(dbManager.ExecuteScalar(PROC_ADD_EVALIMMU_HX_TO_PATIENT_VACC));
                if (VaccineHxId != null)
                {
                    return VaccineHxId;
                }
                else
                {
                    throw new Exception("error found");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::AddToHxTab", PROC_ADD_EVALIMMU_HX_TO_PATIENT_VACC, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string SaveResponse(ImmunizationQueryResponseModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createSaveResponseParameters(dbManager, model, true);
                var QueryResponseId = MDVUtility.ToStr(dbManager.ExecuteScalar(PROC_IMMUNIZATION_QUERY_RESPONSE_INSERT));
                if (QueryResponseId != null)
                {
                    return QueryResponseId;
                }
                else
                {
                    throw new Exception("error found");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveResponse", PROC_IMMUNIZATION_QUERY_RESPONSE_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveAcknowledgement(ImmunizationQueryResponseModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_VACCINE_HX_ID, model.VaccineHxId);
                dbManager.AddParameters(PARM_ACKNOWLEDGEMENT_CODE, model.AcknowledgementCode);
                dbManager.AddParameters(PARM_ACKNOWLEDGEMENT_FILE, model.File);
                var AcknowledgmentId = MDVUtility.ToStr(dbManager.ExecuteScalar(PROC_IMM_ACKNOW_SAVE));
                if (AcknowledgmentId != null)
                {
                    return AcknowledgmentId;
                }
                else
                {
                    throw new Exception("error found");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::SaveAcknowledgement", PROC_IMM_ACKNOW_SAVE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string UpdateHL7Message(Int64 QueryId, string HL7Message)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUERY_ID, QueryId);
                dbManager.AddParameters(1, PARM_HL7_MESSAGE, HL7Message);
                var EffectedRows = MDVUtility.ToStr(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_HL7_MESSAGE));
                if (EffectedRows != "")
                {
                    return EffectedRows;
                }
                else
                {
                    throw new Exception("error found DALImmunization::UpdateHL7Message");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::UpdateHL7Message", PROC_UPDATE_HL7_MESSAGE, ex);
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

        public List<ImmunizationQueryModel> LoadImmQuery(string QueryId, string PatientId, int pageNumber, int rowsPerPage)
        {
            List<ImmunizationQueryModel> QueryList = new List<ImmunizationQueryModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(QueryId) && MDVUtility.ToInt64(QueryId) > 0)
                {
                    dbManager.AddParameters(PARM_QUERY_ID, QueryId);
                }
                else
                {
                    dbManager.AddParameters(PARM_QUERY_ID, null);
                }
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.String, ParamDirection.Output, 500);
                QueryList = dbManager.ExecuteReaders<ImmunizationQueryModel>(PROC_IMM_QUERY_SELECT);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::LoadImmQuery", PROC_IMM_QUERY_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public List<ImmunizationQueryResponseModel> LoadImmQueryResponse(string ImmunizationQueryResponseId, string PatientId, int pageNumber, int rowsPerPage)
        {
            List<ImmunizationQueryResponseModel> QueryList = new List<ImmunizationQueryResponseModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(ImmunizationQueryResponseId) && MDVUtility.ToInt64(ImmunizationQueryResponseId) > 0)
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, ImmunizationQueryResponseId);
                }
                else
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, null);
                }
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.String, ParamDirection.Output, 500);
                QueryList = dbManager.ExecuteReaders<ImmunizationQueryResponseModel>(PROC_IMM_QUERY_RESPONSE_SELECT);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::LoadImmQueryResponse", PROC_IMM_QUERY_RESPONSE_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }




        public List<EvaluatedImmunizationHistoryModel> SearchQueryResponseHX(string ImmunizationQueryResponseId, string PatientId, int pageNumber, int rowsPerPage)
        {
            List<EvaluatedImmunizationHistoryModel> QueryList = new List<EvaluatedImmunizationHistoryModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(ImmunizationQueryResponseId) && MDVUtility.ToInt64(ImmunizationQueryResponseId) > 0)
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, ImmunizationQueryResponseId);
                }
                else
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, null);
                }
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                QueryList = dbManager.ExecuteReaders<EvaluatedImmunizationHistoryModel>(PROC_IMM_QUERY_RESPONSE_HX_SELECT);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::SearchQueryResponseHX", PROC_IMM_QUERY_RESPONSE_HX_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<ImmunizationForecastModel> SearchQueryResponseForecast(string ImmunizationQueryResponseId, int pageNumber, int rowsPerPage)
        {
            List<ImmunizationForecastModel> QueryList = new List<ImmunizationForecastModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(ImmunizationQueryResponseId) && MDVUtility.ToInt64(ImmunizationQueryResponseId) > 0)
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, ImmunizationQueryResponseId);
                }
                else
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, null);
                }
                dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                QueryList = dbManager.ExecuteReaders<ImmunizationForecastModel>(PROC_IMM_QUERY_RESPONSE_FORECAST_SELECT);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::SearchQueryResponseForecast", PROC_IMM_QUERY_RESPONSE_FORECAST_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }


        public List<ImmunizationQueryResponseModel> LoadImmQueryResponsePatientDetail(string ImmunizationQueryResponseId)
        {
            List<ImmunizationQueryResponseModel> QueryList = new List<ImmunizationQueryResponseModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(ImmunizationQueryResponseId) && MDVUtility.ToInt64(ImmunizationQueryResponseId) > 0)
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, ImmunizationQueryResponseId);
                }
                else
                {
                    dbManager.AddParameters(PARM_IMMUNIZATION_QUERY_RESPONSE_ID, null);
                }
                QueryList = dbManager.ExecuteReaders<ImmunizationQueryResponseModel>(PROC_IMM_QUERY_RESPONSE_PATIENT_SELECT);
                return QueryList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Immunization::LoadImmQueryResponsePatientDetail", PROC_IMM_QUERY_RESPONSE_PATIENT_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public string DeleteImmQueryResponse(string ImmunizationQueryResponseId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IMMUNIZATION_QUERY_RESPONSE_ID, ImmunizationQueryResponseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMM_QUERY_RESPONSE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::DeleteImmQueryResponse", PROC_IMM_QUERY_RESPONSE_DELETE, ex);
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
        public string DeleteImmQuery(string QueryId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_QUERY_ID, QueryId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMM_QUERY_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::DeleteImmQueryResponse", PROC_IMM_QUERY_DELETE, ex);
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
        public string GetVaccineScheduleId(string MainAgeGroup, string MainSchedule, string MainCategory)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_MAIN_AGE_GROUP, MainAgeGroup);
                dbManager.AddParameters(1, PARM_MAIN_SCHEDULE_GROUP, MainSchedule);
                dbManager.AddParameters(2, PARM_MAIN_CATEGORY_GROUP, MainCategory);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_VACCINE_SHEDULEID).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::GetVaccineScheduleId", PROC_GET_VACCINE_SHEDULEID, ex);
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
    }
}
