/* Author:  Muhammad Irfan
 * Created Date: 02/12/2015
 * OverView: Created for Social History in Clinical Module
 */

using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Model;
using MDVision.Model.Clinical.History;
using System.Data.SqlClient;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALSocialHistory
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        // Start 02/12/2015 Muhammad Irfan for SocialHx in Clinical
        private const string PROC_SOCIALHX_SEXUALHX_INSERT = "Clinical.sp_SocialHx_SexualHxInsert";
        private const string PROC_SOCIALHX_SEXUALHX_UPDATE = "Clinical.sp_SocialHx_SexualHxUpdate";
        private const string PROC_SOCIALHX_SEXUALHX_DELETE = "Clinical.sp_SocialHx_SexualHxDelete";
        private const string PROC_SOCIALHX_SEXUALHX_SELECT = "Clinical.sp_SocialHx_SexualHxSelect";
        private const string PROC_SOCIALHX_SEXUALHX_COMPLAINTS_LOOKUP = "Clinical.sp_SocialHx_SexualHx_ComplaintsLookup";
        private const string PROC_SOCIALHX_SEXUALHX_PREFERENCES_LOOKUP = "Clinical.sp_SocialHx_SexualHx_PreferencesLookup";
        private const string PROC_SOCIALHX_SEXUALHX_PROTECTIONMETHOD_LOOKUP = "Clinical.sp_SocialHx_SexualHx_ProtectionMethodLookup";
        private const string PROC_SOCIALHX_SEXUALHX_PROTECTIONPERIOD_LOOKUP = "Clinical.sp_SocialHx_SexualHx_ProtectionPeriodLookup";
        private const string PROC_SOCIALHX_SEXUALHX_STATUS_LOOKUP = "Clinical.sp_SocialHx_SexualHx_StatusLookup";
        private const string PROC_SOCIALHX_SEXUALHX_STD_LOOKUP = "Clinical.sp_SocialHx_SexualHx_STDLookup";

        private const string PROC_SOCIALHX_TOBACCO_INSERT = "Clinical.sp_SocialHx_TobaccoInsert";
        private const string PROC_SOCIALHX_TOBACCO_UPDATE = "Clinical.sp_SocialHx_TobaccoUpdate";
        private const string PROC_SOCIALHX_TOBACCO_DELETE = "Clinical.sp_SocialHx_TobaccoDelete";
        private const string PROC_SOCIALHX_TOBACCO_SELECT = "Clinical.sp_SocialHx_TobaccoSelect";
        private const string PROC_TOBACCO_COUNSELLINGTOPIC_LOOKUP = "Clinical.sp_SocialHx_Tobacco_CounsellingTopicLookup";
        private const string PROC_TOBACCO_FREQUENCY_LOOKUP = "Clinical.sp_SocialHx_Tobacco_FrequencyLookup";
        private const string PROC_TOBACCO_SMOKINGSTATUS_LOOKUP = "Clinical.sp_SocialHx_Tobacco_SmokingStatusLookup";
        private const string PROC_TOBACCO_TYPE_LOOKUP = "Clinical.sp_SocialHx_Tobacco_TypeLookup";
        private const string PROC_SOCIALHX_USAGEPERIOD_LOOKUP = "Clinical.sp_SocialHx_UsagePeriodLookup";

        private const string PROC_SOCIALHX_ALCOHOL_INSERT = "Clinical.sp_SocialHx_AlcoholInsert";
        private const string PROC_SOCIALHX_ALCOHOL_UPDATE = "Clinical.sp_SocialHx_AlcoholUpdate";
        private const string PROC_SOCIALHX_ALCOHOL_DELETE = "Clinical.sp_SocialHx_AlcoholDelete";
        private const string PROC_SOCIALHX_ALCOHOL_SELECT = "Clinical.sp_SocialHx_AlcoholSelect";
        private const string PROC_SOCIALHX_ALCOHOL_COUNSELLINGTOPICS_LOOKUP = "Clinical.sp_SocialHx_Alcohol_CounsellingTopicsSelectLookup";
        private const string PROC_SOCIALHX_ALCOHOL_FREQUENCY_LOOKUP = "Clinical.sp_SocialHx_Alcohol_FrequencySelectLookup";
        private const string PROC_SOCIALHX_ALCOHOL_STATUS_LOOKUP = "Clinical.sp_SocialHx_Alcohol_StatusSelectLookup";
        private const string PROC_SOCIALHX_ALCOHOL_TYPE_LOOKUP = "Clinical.sp_SocialHx_Alcohol_TypeSelectLookup";
        private const string PROC_SOCIALHX_CESSATION_PERIOD_LOOKUP = "Clinical.sp_SocialHx_CessationPeriodLookup";

        private const string PROC_SOCIALHX_DRUGABUSE_INSERT = "Clinical.sp_SocialHx_DrugAbuseInsert";
        private const string PROC_SOCIALHX_DRUGABUSE_UPDATE = "Clinical.sp_SocialHx_DrugAbuseUpdate";
        private const string PROC_SOCIALHX_DRUGABUSE_DELETE = "Clinical.sp_SocialHx_DrugAbuseDelete";
        private const string PROC_SOCIALHX_DRUGABUSE_SELECT = "Clinical.sp_SocialHx_DrugAbuseSelect";

        private const string PROC_SOCIALHX_DRUGABUSE_DRUG_LOOKUP = "Clinical.sp_SocialHx_DrugAbuse_DrugLookup";
        private const string PROC_SOCIALHX_DRUGABUSE_FREQUENCYDAILY_LOOKUP = "Clinical.sp_SocialHx_DrugAbuse_FrequencyDailyLookup";
        private const string PROC_SOCIALHX_DRUGABUSE_FREQUENCYMONTHLY_LOOKUP = "Clinical.sp_SocialHx_DrugAbuse_FrequencyMonthlyLookup";
        private const string PROC_SOCIALHX_DRUGABUSE_ROUTE_LOOKUP = "Clinical.sp_SocialHx_DrugAbuse_RouteLookup";
        private const string PROC_SOCIALHX_DRUGABUSE_STATUS_LOOKUP = "Clinical.sp_SocialHx_DrugAbuse_StatusLookup";

        /*Start 06/01/2016 Farooq Ahmad Lookup for SOCIALHX_MISCHX_EXERCISEHX_LOOKUP*/
        private const string PROC_SOCIALHX_MISCHX_EXERCISEHX_DIET_LOOKUP = "Clinical.sp_SocialHx_MiscHx_ExercisesHx_DietLookup";

        private const string PROC_SOCIALHX_MISCHX_EXERCISEHX_TYPE_LOOKUP = "Clinical.sp_SocialHx_MiscHx_ExercisesHx_TypeLookup";

        private const string PROC_SOCIALHX_MISCHX_EXERCISEHX_STATUS_LOOKUP = "Clinical.sp_SocialHx_MiscHx_ExercisesHx_StatusLookup";

        private const string PROC_SOCIALHX_MISCHX_SLEEPHX_STATUS_LOOKUP = "Clinical.sp_SocialHx_MiscHx_SleepHx_StatusLookup";
        /*End 06/01/2016 Farooq Ahmad Lookup for SOCIALHX_MISCHX_EXERCISEHX_LOOKUP*/

        /* 04/12/2015 Muhammad Irfan Social History Parent Table */
        private const string PROC_SOCIALHX_INSERT = "Clinical.sp_SocialHxInsert";
        private const string PROC_SOCIALHX_UPDATE = "Clinical.sp_SocialHxUpdate";
        private const string PROC_SOCIALHX_DELETE = "Clinical.sp_SocialHxDelete";
        private const string PROC_SOCIALHX_SELECT = "Clinical.sp_SocialHxSelect";
        private const string PROC_SOCIALHX_SELECT_ForSoapText = "Clinical.sp_SocialHxSelectForSoapText";
        /* End 04/12/2015 Muhammad Irfan Social History Parent Table */

        /* Start 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */

        private const string PROC_SOCIALHX_COUNSELLING_PERIOD_LOOKUP = "Clinical.sp_SocialHx_CounsellingPeriodLookup";

        /* End 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */

        // End 02/12/2015 Muhammad Irfan for SocialHx in Clinical

        /*
            Change Implement BY: Muhammad Azhar Shahzad
            Reason: Stored Procedure For Soap Text and attachement detachment of social history with progress note
            Created Date: Dec 15, 2015
        */
        private const string PROC_DETACH_SOCIALHX_FROM_NOTES = "Clinical.sp_DetachSocialHxFromNotes";
        private const string PROC_ATTACH_SOCIALHX_FROM_NOTES = "Clinical.sp_AttachSocialHxWithNotes";
        private const string PROC_UPDATE_SOAPTEXT_FOR_SOCIALHX = "Clinical.sp_UpdateSoapTextForSocialHX";
        // end azhar changed

        /* Start 06/01/2016 Farooq Ahmad MiscHx_SleepHx SPs*/
        private const string PROC_MISCHX_SLEEPHX_INSERT = "Clinical.sp_SocialHx_MiscHx_SleepHxInsert";
        private const string PROC_MISCHX_SLEEPHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_SleepHxUpdate";
        private const string PROC_MISCHX_SLEEPHX_DELETE = "Clinical.sp_SocialHx_MiscHx_SleepHxDelete";
        private const string PROC_MISCHX_SLEEPHX_SELECT = "Clinical.sp_SocialHx_MiscHx_SleepHxSelect";
        /* End 06/01/2016 Farooq Ahmad MiscHx_SleepHx SPs*/

        /* Start 07/01/2016 Farooq Ahmad MiscHx_ExercisesHx SPs*/
        private const string PROC_MISCHX_EXERCISESHX_INSERT = "Clinical.sp_SocialHx_MiscHx_ExercisesHxInsert";
        private const string PROC_MISCHX_EXERCISESHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_ExercisesHxUpdate";
        private const string PROC_MISCHX_EXERCISESHX_DELETE = "Clinical.sp_SocialHx_MiscHx_ExercisesHxDelete";
        private const string PROC_MISCHX_EXERCISESHX_SELECT = "Clinical.sp_SocialHx_MiscHx_ExercisesHxSelect";
        /* End 07/01/2016 Farooq Ahmad MiscHx_ExercisesHx SPs*/


        /* Start 11/01/2016 Farooq Ahmad MiscHx_HousingHx SPs*/
        private const string PROC_MISCHX_HOUSINGHX_INSERT = "Clinical.sp_SocialHx_MiscHx_HousingHxInsert";
        private const string PROC_MISCHX_HOUSINGHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_HousingHxUpdate";
        private const string PROC_MISCHX_HOUSINGHX_DELETE = "Clinical.sp_SocialHx_MiscHx_HousingHxDelete";
        private const string PROC_MISCHX_HOUSINGHX_SELECT = "Clinical.sp_SocialHx_MiscHx_HousingHxSelect";
        /* End 11/01/2016 Farooq Ahmad MiscHx_HousingHx SPs*/

        /* Start 06/01/2016 Muhammad Arshad MiscHx SPs*/
        private const string PROC_MISCHX_INSERT = "Clinical.sp_SocialHx_MiscHxInsert";
        private const string PROC_MISCHX_UPDATE = "Clinical.sp_SocialHx_MiscHxUpdate";
        private const string PROC_MISCHX_DELETE = "Clinical.sp_SocialHx_MiscHxDelete";
        private const string PROC_MISCHX_SELECT = "Clinical.sp_SocialHx_MiscHxSelect";
        /* End 06/01/2016 Muhammad Arshad MiscHx SPs*/

        /* Start 06/01/2016 Zia Lookup MiscHx SPs */

        private const string PROC_MISCHX_OCCUPATIONHX_INSERT = "Clinical.sp_SocialHx_MiscHx_OccupationHxInsert";
        private const string PROC_MISCHX_OCCUPATIONHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_OccupationHxUpdate";
        private const string PROC_MISCHX_OCCUPATIONHX_DELETE = "Clinical.sp_SocialHx_MiscHx_OccupationHxDelete";
        private const string PROC_MISCHX_OCCUPATIONHX_SELECT = "Clinical.sp_SocialHx_MiscHx_OccupationHxSelect";
        private const string PROC_MISCHX_OCCUPATIONHX_FILL = "Clinical.sp_SocialHx_MiscHx_OccupationHx_Fill";

        private const string PROC_MISCHX_CAFFEINEINTAKEHX_INSERT = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakHxInsert";
        private const string PROC_MISCHX_CAFFEINEINTAKEHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakHxUpdate";
        private const string PROC_MISCHX_CAFFEINEINTAKEHX_DELETE = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakHxDelete";
        private const string PROC_MISCHX_CAFFEINEINTAKEHX_SELECT = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakHxSelect";

        private const string PROC_MISCHX_TRAVELHX_SELECT = "Clinical.sp_SocialHx_MiscHx_TravelHx_Select";
        private const string PROC_MISCHX_TRAVELHX_FILL = "Clinical.sp_SocialHx_MiscHx_TravelHx_Fill";
        private const string PROC_MISCHX_TRAVELHX_UPDATE = "Clinical.sp_SocialHx_MiscHx_TravelHx_Update";
        private const string PROC_MISCHX_TRAVELHX_INSERT = "Clinical.sp_SocialHx_MiscHx_TravelHx_Insert";
        private const string PROC_MISCHX_TRAVELHX_DELETE = "Clinical.sp_SocialHx_MiscHx_TravelHxDelete";

        private const string PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT_LOOKUP = "Clinical.sp_SocialHx_MiscHx_OccupationHx_StatusSelectLookup";
        private const string PROC_SOCIALHX_MISCHX_CAFFEINEINTAKEHX_FREQUENCY_LOOKUP = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakeHx_FrequencyLookup";
        private const string PROC_SOCIALHX_MISCHX_HOUSINGHX_STATUS_LOOKUP = "Clinical.sp_SocialHx_MiscHx_HousingHx_StatusLookup";
        private const string PROC_SOCIALHX_MISCHX_CAFFEINE_INTAKEHX_STATUS_LOOKUP = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakeHx_StatusLookup";
        private const string PROC_SOCIALHX_MISCHX_TRAVEL_HX_STATUS_LOOKUP = "Clinical.sp_SocialHx_MiscHx_TravelHx_Status";

        /* End 07/01/2016 Zia Lookup for MiscHx SPs */

        // Start 13/01/2016 Muhammad Arshad MiscHx_Component table SPs
        private const string PROC_MISCHX_COMPONENT_INSERT = "Clinical.sp_SocialHx_MiscHx_ComponentInsert";
        private const string PROC_MISCHX_COMPONENT_UPDATE = "Clinical.sp_SocialHx_MiscHx_ComponentUpdate";
        private const string PROC_MISCHX_COMPONENT_DELETE = "Clinical.sp_SocialHx_MiscHx_ComponentDelete";
        private const string PROC_MISCHX_COMPONENT_SELECT = "Clinical.sp_SocialHx_MiscHx_ComponentSelect";
        // End 13/01/2016 Muhammad Arshad MiscHx_Component table SPs
        private const string PROC_HXLOG_SELECT = "Clinical.sp_HXLogSelect";

        private const string PROC_NOTES_SOCIALHX_SELECT = "Clinical.sp_NotesSocialHxSelect";

        #region SocialHx Mobile App Native Procedure
        private const string PROC_SOCIALHX_TABACCO_MOBILEAPP = "Clinical.sp_SocialHx_TabaccoNativeSelect";
        private const string PROC_SOCIALHX_ALCOHOL_MOBILEAPP = "Clinical.sp_SocialHx_AlcoholNativeSelect";
        private const string PROC_SOCIALHX_DRUGABUSE_MOBILEAPP = "Clinical.sp_SocialHx_DrugAbuseNativeSelect";
        private const string PROC_SOCIALHX_SEXUAL_MOBILEAPP = "Clinical.sp_SocialHx_SexualHxNativeSelect";
        private const string PROC_SOCIALHX_OCCUPATION_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_OccupationHxNativeSelect";
        private const string PROC_SOCIALHX_SLEEP_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_SleepHxNativeSelect";
        private const string PROC_SOCIALHX_HOUSING_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_HousingHxNativeSelect";
        private const string PROC_SOCIALHX_EXERCISE_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_ExercisesHxNativeSelect";
        private const string PROC_SOCIALHX_CAFEINE_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_CaffeineIntakHxNativeSelect";
        private const string PROC_SOCIALHX_TRAVEL_MOBILEAPP = "Clinical.sp_SocialHx_MiscHx_TravelHxNativeSelect";
        private const string PROC_CHECK_SOCIALHX_EXISTS = "Clinical.sp_SocialHxExistsCheck";

        private const string PROC_SOCIALHX_COMPONENTS_NATIVE = "Clinical.sp_SocialHxOriginalTableInsert";
        #endregion
        #endregion

        #region "Parameters"
        // Start 02/12/2015 Muhammad Irfan for SocialHx in Clinical

        private const string PARM_SEXUALHX_ID = "@SexualHxId";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_PREFERENCE_ID = "@PreferenceId";
        private const string PARM_USING_PROTECTION = "@bUSingProtection";
        private const string PARM_PROTECTION_METHOD_ID = "@ProtectionMethodId";
        private const string PARM_PROTECTION_PERIOD_ID = "@ProtectionPeriodId";
        private const string PARM_COMPLAINT_ID = "@ComplaintId";
        private const string PARM_EXPOSED_TO_STD = "@bExposedToSTD";
        private const string PARM_STD_IDS = "@STDIds";
        private const string PARM_PAIN_INTERCOURSE = "@bPainWithIntercourse";
        private const string PARM_SCXUALLY_ABUSED = "@bSexuallyAbused";
        private const string PARM_PREGNANCY_STATUS = "@PregnancyStatus";
        private const string PARM_PREGNANCY_DURATION = "@PregnancyDuration";
        private const string PARM_LMP = "@LMP";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        //---------------- 
        private const string PARM_TOBACCO_ID = "@TobaccoId";
        private const string PARM_SOCIALHX_ID = "@SocialHxId";
        private const string PARM_TYPE_ID = "@TypeId";
        private const string PARM_USAGE_PERIOD_ID = "@UsagePeriodId";
        private const string PARM_FREQUENCY_ID = "@FrequencyId";
        private const string PARM_COUNSELLING_PERIOD_ID = "@CounsellingPeriodId";
        private const string PARM_COUNSELLING_TOPIC_ID = "@CounsellingTopicId";
        private const string PARM_CESSATION_LENGTH = "@CessationLength";
        private const string PARM_CESSATION_PERIOD_ID = "@CessationPeriodId";
        private const string PARM_RECENTLY_QUIT = "@bRecentlyQuit";
        private const string PARM_WOULD_QUIT = "@bWouldQuit";
        private const string PARM_MU_ALERTS_COUNT = "@MUAlertsCount";

        private const string PARM_TOBACCO_CounsellingPeriod = "@TobaccoCounsellingPeriod";
        private const string PARM_TOBACCO_CounsellingTopic = "@TobaccoCounsellingTopic";
        private const string PARM_TOBACCO_CessationLength = "@TobaccoCessationLength";
        private const string PARM_TOBACCO_CessationPeriod = "@TobaccoCessationPeriod";
        private const string PARM_TOBACCO_RecentlyQuit = "@TobaccoRecentlyQuit";
        private const string PARM_TOBACCO_WouldQuit = "@TobaccoWouldQuit";
        private const string PARM_TOBACCO_Comments = "@TobaccoComments";
        //-------------------


        private const string PARM_ALCOHOL_ID = "@AlcoholId";
        private const string PARM_NOT_READY_TO_QUIT = "@bNotReadyToQuit";
        private const string PARM_ALCOHOL_CounsellingPeriod = "@AlcoholCounsellingPeriod";
        private const string PARM_ALCOHOL_CounsellingTopic = "@AlcoholCounsellingTopic";
        private const string PARM_ALCOHOL_CessationLength = "@AlcoholCessationLength";
        private const string PARM_ALCOHOL_CessationPeriod = "@AlcoholCessationPeriod";
        private const string PARM_ALCOHOL_RecentlyQuit = "@AlcoholRecentlyQuit";
        private const string PARM_ALCOHOL_WouldQuit = "@AlcoholWouldQuit";
        private const string PARM_ALCOHOL_Comments = "@AlcoholComments";
        private const string PARM_ALCOHOL_NotReadyToQuit = "@AlcoholbNotReadyToQuit";
        //---------------------
        private const string PARM_DRUG_ABUSE_ID = "@DrugAbuseId";
        private const string PARM_DRUG_ID = "@DrugId";
        private const string PARM_ROUTE_ID = "@RouteId";
        private const string PARM_FREQUENCY_DAILY_ID = "@FrequencyDailyId";
        private const string PARM_FREQUENCY_MONTHLY_ID = "@FrequencyMonthlyId";

        private const string PARM_DRUG_CessationLength = "@DRUGCessationLength";
        private const string PARM_DRUG_CessationPeriod = "@DRUGCessationPeriod";
        private const string PARM_DRUG_RecentlyQuit = "@DRUGRecentlyQuit";
        private const string PARM_DRUG_WouldQuit = "@DRUGWouldQuit";
        private const string PARM_DRUG_Comments = "@DRUGComments";

        //------------- Travel
        private const string PARM_Travel_Status = "@TravelChildStatus";
        private const string PARM_Travel_FromDate = "@TravelHxFromDate";
        private const string PARM_Travel_ToDate = "@TravelHxToDate";

        private const string PARM_Travel_Location = "@TravelHxLocation";
        private const string PARM_Travel_Comments = "@TravelHxComments";

        private const string PARM_Travel_CreatedBy = "@TravelHxCreatedBy";
        private const string PARM_Travel_CreatedOn = "@TravelHxCreatedOn";
        private const string PARM_Travel_ModifiedBy = "@TravelHxModifiedBy";
        private const string PARM_Travel_ModifiedOn = "@TravelHxModifiedOn";
        private const string PARM_Travel_SoapText = "@TravelHxSoapText";
        /* Start 04/12/2015 Muhammad Irfan Social History Parent Table */
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_IS_SYNCED = "@IsSynced";
        private const string PARM_SOCIALHX_DATE = "@SocialHxDate";
        private const string PARM_UNREMARKABLE = "@bUnremarkable";
        /* End 04/12/2015 Muhammad Irfan Social History Parent Table */

        // End 02/12/2015 Muhammad Irfan for SocialHx in Clinical
        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason:Paramters For Soap Text and attachement detachment of social history with progress note
           Created Date: Dec 15, 2015
       */
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_SOAPTEXT_BY = "@SoapText";
        //end azhar changed

        /* Start 06/01/2016 Muhammad Arshad MiscHx Parameters*/
        private const string PARM_MISCHX_ID = "@MiscHxId";
        /* Start 06/01/2016 Muhammad Arshad MiscHx Parameters*/

        /* Start 06/01/2016 Syed Zia, MiscHx Occupation Parameter*/
        private const string PARM_OCCUPATIONHX_ID = "@OccupationHxId";
        private const string PARM_PRESENT = "@Present";
        private const string PARM_PAST = "@Past";

        private const string PARM_OCCUPATION_START_DATE_NATIVE = "@OccupationStartDate";
        private const string PARM_OCCUPATION_END_DATE_NATIVE = "@OccupationEndDate";
        private const string PARM_OCCUPATION_DETAIL = "@OccupationDetail";
        private const string PARM_OCCUPATION_PAST_OR_PRESENT = "@IsPast";
        private const string PARM_OCCUPATION_START_DATE = "@StartDate";
        private const string PARM_OCCUPATION_END_DATE = "@EndDate";

        /* END 06/01/2016 Syed Zia, MiscHx Occupation Parameter*/

        /* Start 07/01/2016 Syed Zia, MiscHx CaffeineIntake Hx Parameter*/
        private const string PARM_CAFFEINEINTAKEHX_ID = "@CaffeineIntakHxId";
        private const string PARM_IS_HARMFUL = "@IsHarmful";
        /* End 07/01/2016 Syed Zia,MiscHx CaffeineIntake Hx Parameter*/

        /* Start 06/01/2016 Farooq Ahmad SleepHx Parameters*/
        private const string PARM_SLEEPHX_ID = "@SleepHxId";
        private const string PARM_SLEEP_HOURS = "@SleepHours";

        /* End 06/01/2016 Farooq Ahmad SleepHx Parameters*/
        private const string PARM_TRAVEL_ID = "@TravelHxId";
        private const string PARM_TRAVEL_FROM_DATE = "@FromDate";
        private const string PARM_TRAVEL_TO_DATE = "@Todate";
        private const string PARM_TRAVEL_COMMENTS = "@Comments";
        private const string PARM_TRAVEL_LOCATION = "@Location";
       

        /* Start 07/01/2016 Farooq Ahmad ExercisesHx Parameters*/
        private const string PARM_EXERCISESHX_ID = "@ExercisesHxId";
        private const string PARM_DIET_ID = "@DietId";

        /* End 07/01/2016 Farooq Ahmad ExercisesHx Parameters*/

        /* Start 07/01/2016 Farooq Ahmad HousingHx Parameters*/
        private const string PARM_HOUSINGHX_ID = "@HousingHxId";

        /* End 07/01/2016 Farooq Ahmad HousingHx Parameters*/
        // Start 13/01/2016 Muhammad Arshad MiscHx_Component table SPs Parameters
        private const string PARM_COMPONENT_ID = "@ComponentId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_COMPONENT_NAME = "@ComponentName";
        private const string PARM_COMPONENT_ORDER = "@ComponentOrder";
        private const string PARM_SORT_ORDER = "@SortOrder";
        // Start 13/01/2016 Muhammad Arshad MiscHx_Component table SPs Parameters


        private const string PARM_HX_ID = "@HxId";
        private const string PARM_HX_TYPE = "@HxType";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_REQUEST_STATUS = "@RequestStatus";
        private const string PARM_Sexual_Comments = "@SexualComments";
        private const string PARM_Sexual_Pregnacy_Duration = "@PregnacyDuration";

        #region SocialHx Mobile App Parameters

        private const string PARM_TABBACCO_STATUS = "@TabbacoStatus";
        private const string PARM_TABBACCO_TYPE = "@TabbaccoType";
        private const string PARM_TABBACO_USAGE = "@TabbaccoUsage";
        private const string PARM_TABBACCO_FREQUENCY = "@TabbaccoFrequency";
        private const string PARM_ALCOHOL_STATUS = "@AlcoholStatus";
        private const string PARM_ALCOHOL_TYPE = "@AlcoholType";
        private const string PARM_ALCOHOL_USAGE = "@AlcoholUsage";
        private const string PARM_ALCOHOL_FREQUENCY = "@AlcoholFrequency";
        private const string PARM_DRUG_STATUS = "@DrugStatus";
        private const string PARM_DRUG_TYPE = "@Drugtype";
        private const string PARM_DRUG_USAGE = "@DrugUsage";
        private const string PARM_DRUG_FREQUENCY = "@DrugFrequency";
        private const string PARM_SEXUAL_STATUS = "@SexualStatus";
        private const string PARM_SEXUAL_PREFERENCES = "@SexualPrefernces";
        private const string PARM_SEXUAL_PROTECTION = "@SexualProtection";
        private const string PARM_SEXUAL_PROTECTION_PERIOD = "@SexualProtectionPeriod";
        private const string PARM_SEXUAL_USING_PROTECTION = "@SexualbUsingProtection";
        private const string PARM_SEXUAL_PAIN_INTERCOURSE = "@SexualPainIntercourse";
        private const string PARM_SEXUAL_COMPLAINTS = "@SexualComplaints";
        private const string PARM_SEXUAL_EXPOSEDTOSTD = "@SexualsExposedtoSTD";
        private const string PARM_SEXUAL_ABUSED = "@SexuallyAbused";
        private const string PARM_MISCHX_OCCUPATION_STATUS = "@OccupationStatus";
        private const string PARM_MISCHX_OCCUPATION_PRESENT = "@OccupationPresent";
        private const string PARM_MISCHX_OCCUPATION_PAST = "@OccupationPast";
        private const string PARM_MISCHX_OCCUPATION_COMMENTS = "@OccupationComments";
        private const string PARM_MISCHX_OCCUPATION_DETAIL = "@OccupationDetail";
        private const string PARM_MISCHX_OCCUPATION_FROM_DATE = "@OccupationStartDate";
        private const string PARM_MISCHX_OCCUPATION_TO_DATE = "@OccupationEndDate";
        private const string PARM_MISCHX_SLEEP_STATUS = "@SleepStatus";
        private const string PARM_MISCHX_SLEEP_HOURS = "@SleepHours";
        private const string PARM_MISCHX_SLEEP_COMMENTS = "@SleepComments";
        private const string PARM_MISCHX_EXERCISE_STATUS = "@ExerciseStatus";
        private const string PARM_MISCHX_EXERCISE_TYPE = "@ExerciseType";
        private const string PARM_MISCHX_EXERCISE_DIET = "@ExerciseDiet";
        private const string PARM_MISCHX_EXERCISE_COMMENTS = "@ExerciseComments";
        private const string PARM_MISCHX_HOUSING_STATUS = "@HousingStatus";
        private const string PARM_MISCHX_HOUSING_PRESENT = "@HousingPresent";
        private const string PARM_MISCHX_HOUSING_PAST = "@HousingPast";
        private const string PARM_MISCHX_HOUSING_COMMENTS = "@HousingComments";
        private const string PARM_MISCHX_CAFFEINE_STATUS = "@CaffeineStatus";
        private const string PARM_MISCHX_CAFFEINE_ISHARMFUL = "@CaffeineIsHarmful";
        private const string PARM_MISCHX_CAFFEINE_FREQUENCY = "@CaffeineFrequency";
        private const string PARM_MISCHX_CAFFEINE_COMMENTS = "@CaffeineComments";
        private const string PARM_SOAPTEXT_TABBACCO = "@SoapTextTabbacco";
        private const string PARM_SOAPTEXT_ALCOHOL = "@SoapTextAlcohol";
        private const string PARM_SOAPTEXT_TRAVEL = "@SoapTextTravel";
        private const string PARM_SOAPTEXT_DRUGABUSE = "@SoapTextDrugabuse";
        private const string PARM_SOAPTEXT_SEXUAL = "@SoapTextSexual";
        private const string PARM_SOAPTEXT_OCCUPATION = "@SoapTextOccupation";
        private const string PARM_SOAPTEXT_SLEEP = "@SoapTextSleep";
        private const string PARM_SOAPTEXT_EXERCISE = "@SoapTextExercise";
        private const string PARM_SOAPTEXT_HOUSING = "@SoapTextHousing";
        private const string PARM_SOAPTEXT_CAFFEINE = "@SoapTextCaffeine";

        private const string PARM_CREATEDBY_ALCOHOL = "@CreatedByAlcohol";
        private const string PARM_CREATEDON_ALCOHOL = "@CreatedOnAlcohol";
        private const string PARM_MODIFIEDBY_ALCOHOL = "@ModifiedByAlcohol";
        private const string PARM_MODIFIEDON_ALCOHOL = "@ModifiedOnAlcohol";
        private const string PARM_CREATEDBY_DRUGABUSE = "@CreatedByDrugabuse";
        private const string PARM_CREATEDON_DRUGABUSE = "@CreatedOnDrugabuse";
        private const string PARM_MODIFIEDBY_DRUGABUSE = "@ModifiedByDrugabuse";
        private const string PARM_MODIFIEDON_DRUGABUSE = "@ModifiedOnDrugabuse";
        private const string PARM_CREATEDBY_SEXUAL = "@CreatedBySexual";
        private const string PARM_CREATEDON_SEXUAL = "@CreatedOnSexual";
        private const string PARM_MODIFIEDBY_SEXUAL = "@ModifiedBySexual";
        private const string PARM_MODIFIEDON_SEXUAL = "@ModifiedOnSexual";
        private const string PARM_CREATEDBY_OCCUPATION = "@CreatedByOccupation";
        private const string PARM_CREATEDON_OCCUPATION = "@CreatedOnOccupation";
        private const string PARM_MODIFIEDBY_OCCUPATION = "@ModifiedByOccupation";
        private const string PARM_MODIFIEDON_OCCUPATION = "@ModifiedOnOccupation";
        private const string PARM_CREATEDBY_SLEEP = "@CreatedBySleep";
        private const string PARM_CREATEDON_SLEEP = "@CreatedOnSleep";
        private const string PARM_MODIFIEDBY_SLEEP = "@ModifiedBySleep";
        private const string PARM_MODIFIEDON_SLEEP = "@ModifiedOnSleep";
        private const string PARM_CREATEDBY_EXERCISE = "@CreatedByExercise";
        private const string PARM_CREATEDON_EXERCISE = "@CreatedOnExercise";
        private const string PARM_MODIFIEDBY_EXERCISE = "@ModifiedByExercise";
        private const string PARM_MODIFIEDON_EXERCISE = "@ModifiedOnExercise";
        private const string PARM_CREATEDBY_HOUSING = "@CreatedByHousing";
        private const string PARM_CREATEDON_HOUSING = "@CreatedOnHousing";
        private const string PARM_MODIFIEDBY_HOUSING = "@ModifiedByHousing";
        private const string PARM_MODIFIEDON_HOUSING = "@ModifiedOnHousing";
        private const string PARM_CREATEDBY_CAFFEINE = "@CreatedByCaffeine";
        private const string PARM_CREATEDON_CAFFEINE = "@CreatedOnCaffeine";
        private const string PARM_MODIFIEDBY_CAFFEINE = "@ModifiedByCaffeine";
        private const string PARM_MODIFIEDON_CAFFEINE = "@ModifiedOnCaffeine";

        #endregion
        #endregion

        #region Constructors

        public DALSocialHistory(bool isNative)
        {
            //
        }

        public DALSocialHistory(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        public DALSocialHistory()
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

        #region "Support Functions Sexual History"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersSexualHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SEXUALHX_ID, ds.SocialHx_SexualHx.SexualHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SEXUALHX_ID, ds.SocialHx_SexualHx.SexualHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_STATUS_ID, ds.SocialHx_SexualHx.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_PREFERENCE_ID, ds.SocialHx_SexualHx.PreferenceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_USING_PROTECTION, ds.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_PROTECTION_METHOD_ID, ds.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(5, PARM_PROTECTION_PERIOD_ID, ds.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_COMPLAINT_ID, ds.SocialHx_SexualHx.ComplaintIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_EXPOSED_TO_STD, ds.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_STD_IDS, ds.SocialHx_SexualHx.STDIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PAIN_INTERCOURSE, ds.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_SCXUALLY_ABUSED, ds.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_LMP, ds.SocialHx_SexualHx.LMPColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_COMMENTS, ds.SocialHx_SexualHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.SocialHx_SexualHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.SocialHx_SexualHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CREATED_ON, ds.SocialHx_SexualHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.SocialHx_SexualHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.SocialHx_SexualHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_SOCIALHX_ID, ds.SocialHx_SexualHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_SexualHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateSexualHxInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(22);

            dbManager.AddInsertUpdateParameters(0, PARM_SEXUALHX_ID, ds.SocialHx_SexualHx.SexualHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_STATUS_ID, ds.SocialHx_SexualHx.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_PREFERENCE_ID, ds.SocialHx_SexualHx.PreferenceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_USING_PROTECTION, ds.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_PROTECTION_METHOD_ID, ds.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_PROTECTION_PERIOD_ID, ds.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COMPLAINT_ID, ds.SocialHx_SexualHx.ComplaintIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_EXPOSED_TO_STD, ds.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_STD_IDS, ds.SocialHx_SexualHx.STDIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_PAIN_INTERCOURSE, ds.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_SCXUALLY_ABUSED, ds.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_LMP, ds.SocialHx_SexualHx.LMPColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_COMMENTS, ds.SocialHx_SexualHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IS_ACTIVE, ds.SocialHx_SexualHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_BY, ds.SocialHx_SexualHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_ON, ds.SocialHx_SexualHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_BY, ds.SocialHx_SexualHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_ON, ds.SocialHx_SexualHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SOCIALHX_ID, ds.SocialHx_SexualHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_SexualHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_PREGNANCY_STATUS, ds.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(21, PARM_PREGNANCY_DURATION, ds.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName, DbType.String);
        }
        private void CreateSexualHxUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(22);

            dbManager.AddInsertUpdateParameters(0, PARM_SEXUALHX_ID, ds.SocialHx_SexualHx.SexualHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_STATUS_ID, ds.SocialHx_SexualHx.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_PREFERENCE_ID, ds.SocialHx_SexualHx.PreferenceIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_USING_PROTECTION, ds.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_PROTECTION_METHOD_ID, ds.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_PROTECTION_PERIOD_ID, ds.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COMPLAINT_ID, ds.SocialHx_SexualHx.ComplaintIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_EXPOSED_TO_STD, ds.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_STD_IDS, ds.SocialHx_SexualHx.STDIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_PAIN_INTERCOURSE, ds.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_SCXUALLY_ABUSED, ds.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_LMP, ds.SocialHx_SexualHx.LMPColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_COMMENTS, ds.SocialHx_SexualHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IS_ACTIVE, ds.SocialHx_SexualHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_BY, ds.SocialHx_SexualHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_ON, ds.SocialHx_SexualHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_BY, ds.SocialHx_SexualHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_ON, ds.SocialHx_SexualHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SOCIALHX_ID, ds.SocialHx_SexualHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_SexualHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_PREGNANCY_STATUS, ds.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(21, PARM_PREGNANCY_DURATION, ds.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Support Functions Tobacco"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersTobacco(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TOBACCO_ID, ds.SocialHx_Tobacco.TobaccoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TOBACCO_ID, ds.SocialHx_Tobacco.TobaccoIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Tobacco.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_Tobacco.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_TYPE_ID, ds.SocialHx_Tobacco.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Tobacco.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Tobacco.CessationLengthColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_WOULD_QUIT, ds.SocialHx_Tobacco.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_COMMENTS, ds.SocialHx_Tobacco.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.SocialHx_Tobacco.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.SocialHx_Tobacco.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CREATED_ON, ds.SocialHx_Tobacco.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_MODIFIED_BY, ds.SocialHx_Tobacco.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_MODIFIED_ON, ds.SocialHx_Tobacco.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_Tobacco.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateTobaccoInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(19);

            dbManager.AddInsertUpdateParameters(0, PARM_TOBACCO_ID, ds.SocialHx_Tobacco.TobaccoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Tobacco.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_Tobacco.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_Tobacco.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Tobacco.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Tobacco.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_WOULD_QUIT, ds.SocialHx_Tobacco.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_COMMENTS, ds.SocialHx_Tobacco.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IS_ACTIVE, ds.SocialHx_Tobacco.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_BY, ds.SocialHx_Tobacco.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_ON, ds.SocialHx_Tobacco.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_BY, ds.SocialHx_Tobacco.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_ON, ds.SocialHx_Tobacco.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_Tobacco.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateTobaccoUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(19);

            dbManager.AddInsertUpdateParameters(0, PARM_TOBACCO_ID, ds.SocialHx_Tobacco.TobaccoIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Tobacco.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_Tobacco.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_Tobacco.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Tobacco.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Tobacco.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_WOULD_QUIT, ds.SocialHx_Tobacco.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_COMMENTS, ds.SocialHx_Tobacco.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IS_ACTIVE, ds.SocialHx_Tobacco.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_BY, ds.SocialHx_Tobacco.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_ON, ds.SocialHx_Tobacco.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_BY, ds.SocialHx_Tobacco.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_ON, ds.SocialHx_Tobacco.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_Tobacco.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Support Functions Alcohol"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersAlcohol(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ALCOHOL_ID, ds.SocialHx_Alcohol.AlcoholIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ALCOHOL_ID, ds.SocialHx_Alcohol.AlcoholIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Alcohol.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_Alcohol.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_TYPE_ID, ds.SocialHx_Alcohol.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Alcohol.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Alcohol.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_NOT_READY_TO_QUIT, ds.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_WOULD_QUIT, ds.SocialHx_Alcohol.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_COMMENTS, ds.SocialHx_Alcohol.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.SocialHx_Alcohol.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.SocialHx_Alcohol.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.SocialHx_Alcohol.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.SocialHx_Alcohol.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.SocialHx_Alcohol.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_Alcohol.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateAlcoholInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(20);

            dbManager.AddInsertUpdateParameters(0, PARM_ALCOHOL_ID, ds.SocialHx_Alcohol.AlcoholIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Alcohol.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_Alcohol.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_Alcohol.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Alcohol.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Alcohol.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_NOT_READY_TO_QUIT, ds.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_WOULD_QUIT, ds.SocialHx_Alcohol.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_COMMENTS, ds.SocialHx_Alcohol.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.SocialHx_Alcohol.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.SocialHx_Alcohol.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.SocialHx_Alcohol.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.SocialHx_Alcohol.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.SocialHx_Alcohol.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_Alcohol.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateAlcoholUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(20);

            dbManager.AddInsertUpdateParameters(0, PARM_ALCOHOL_ID, ds.SocialHx_Alcohol.AlcoholIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_Alcohol.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_Alcohol.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_Alcohol.TypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_USAGE_PERIOD_ID, ds.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_ID, ds.SocialHx_Alcohol.FrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_COUNSELLING_PERIOD_ID, ds.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_COUNSELLING_TOPIC_ID, ds.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_Alcohol.CessationLengthColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CESSATION_PERIOD_ID, ds.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(10, PARM_RECENTLY_QUIT, ds.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_NOT_READY_TO_QUIT, ds.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_WOULD_QUIT, ds.SocialHx_Alcohol.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_COMMENTS, ds.SocialHx_Alcohol.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_IS_ACTIVE, ds.SocialHx_Alcohol.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_BY, ds.SocialHx_Alcohol.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CREATED_ON, ds.SocialHx_Alcohol.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_BY, ds.SocialHx_Alcohol.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_MODIFIED_ON, ds.SocialHx_Alcohol.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(19, PARM_SOAPTEXT_BY, ds.SocialHx_Alcohol.SoapTextColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Support Functions Drug"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersDrug(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_DRUG_ABUSE_ID, ds.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DRUG_ABUSE_ID, ds.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_DrugAbuse.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_DrugAbuse.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_DRUG_ID, ds.SocialHx_DrugAbuse.DrugIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ROUTE_ID, ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(5, PARM_FREQUENCY_DAILY_ID, ds.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(6, PARM_FREQUENCY_MONTHLY_ID, ds.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_USAGE_PERIOD_ID, ds.SocialHx_DrugAbuse.UsagePeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_RECENTLY_QUIT, ds.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_WOULD_QUIT, ds.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(11, PARM_COMMENTS, ds.SocialHx_DrugAbuse.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.SocialHx_DrugAbuse.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_CREATED_BY, ds.SocialHx_DrugAbuse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_ON, ds.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_CESSATION_PERIOD_ID, ds.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_DrugAbuse.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateDrugInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(19);

            dbManager.AddInsertUpdateParameters(0, PARM_DRUG_ABUSE_ID, ds.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_DrugAbuse.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_DrugAbuse.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_DRUG_ID, ds.SocialHx_DrugAbuse.DrugIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ROUTE_ID, ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_DAILY_ID, ds.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_FREQUENCY_MONTHLY_ID, ds.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_USAGE_PERIOD_ID, ds.SocialHx_DrugAbuse.UsagePeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_RECENTLY_QUIT, ds.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_WOULD_QUIT, ds.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_COMMENTS, ds.SocialHx_DrugAbuse.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_ACTIVE, ds.SocialHx_DrugAbuse.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_BY, ds.SocialHx_DrugAbuse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_ON, ds.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_BY, ds.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_ON, ds.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_CESSATION_PERIOD_ID, ds.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_DrugAbuse.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateDrugUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(19);

            dbManager.AddInsertUpdateParameters(0, PARM_DRUG_ABUSE_ID, ds.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_DrugAbuse.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_DrugAbuse.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_DRUG_ID, ds.SocialHx_DrugAbuse.DrugIdColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ROUTE_ID, ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(5, PARM_FREQUENCY_DAILY_ID, ds.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_FREQUENCY_MONTHLY_ID, ds.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_USAGE_PERIOD_ID, ds.SocialHx_DrugAbuse.UsagePeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(8, PARM_CESSATION_LENGTH, ds.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(9, PARM_RECENTLY_QUIT, ds.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_WOULD_QUIT, ds.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(11, PARM_COMMENTS, ds.SocialHx_DrugAbuse.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_ACTIVE, ds.SocialHx_DrugAbuse.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_BY, ds.SocialHx_DrugAbuse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_ON, ds.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_BY, ds.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_ON, ds.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_CESSATION_PERIOD_ID, ds.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_SOAPTEXT_BY, ds.SocialHx_DrugAbuse.SoapTextColumn.ColumnName, DbType.String);

        }

        #endregion

        #region "Support Functions Misc History"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersMiscHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MISCHX_ID, ds.SocialHx_MiscHx.MiscHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MISCHX_ID, ds.SocialHx_MiscHx.MiscHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_MiscHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CREATED_BY, ds.SocialHx_MiscHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CREATED_ON, ds.SocialHx_MiscHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.SocialHx_MiscHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MODIFIED_ON, ds.SocialHx_MiscHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateMiscHxInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(7);

            dbManager.AddInsertUpdateParameters(0, PARM_MISCHX_ID, ds.SocialHx_MiscHx.MiscHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_MiscHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CREATED_BY, ds.SocialHx_MiscHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CREATED_ON, ds.SocialHx_MiscHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_MODIFIED_BY, ds.SocialHx_MiscHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_MODIFIED_ON, ds.SocialHx_MiscHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(6, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx.SoapTextColumn.ColumnName, DbType.String);

        }
        private void CreateMiscHxUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(7);

            dbManager.AddInsertUpdateParameters(0, PARM_MISCHX_ID, ds.SocialHx_MiscHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_SOCIALHX_ID, ds.SocialHx_MiscHx.SocialHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CREATED_BY, ds.SocialHx_MiscHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CREATED_ON, ds.SocialHx_MiscHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_MODIFIED_BY, ds.SocialHx_MiscHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_MODIFIED_ON, ds.SocialHx_MiscHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(6, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx.SoapTextColumn.ColumnName, DbType.String);

        }

        /* Start 06/01/2016 Syed Zia, MiscHx Occupation Table*/
        private void CreateMiscHxOccupationHxInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(16);
            dbManager.AddInsertUpdateParameters(0, PARM_OCCUPATIONHX_ID, ds.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PRESENT, ds.SocialHx_MiscHx_OccupationHx.PresentColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PAST, ds.SocialHx_MiscHx_OccupationHx.PastColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_OccupationHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_OccupationHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_OccupationHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_OccupationHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_OccupationHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_OccupationHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_OCCUPATION_START_DATE, ds.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_OCCUPATION_END_DATE, ds.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_OCCUPATION_DETAIL, ds.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_OCCUPATION_PAST_OR_PRESENT, ds.SocialHx_MiscHx_OccupationHx.IsPastColumn.ColumnName, DbType.Boolean);
        }

        private void CreateMiscHxOccupationHxUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(16);
            dbManager.AddInsertUpdateParameters(0, PARM_OCCUPATIONHX_ID, ds.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PRESENT, ds.SocialHx_MiscHx_OccupationHx.PresentColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PAST, ds.SocialHx_MiscHx_OccupationHx.PastColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_OccupationHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_OccupationHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_OccupationHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_OccupationHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_OccupationHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_OccupationHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_OCCUPATION_START_DATE, ds.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(13, PARM_OCCUPATION_END_DATE, ds.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_OCCUPATION_DETAIL, ds.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_OCCUPATION_PAST_OR_PRESENT, ds.SocialHx_MiscHx_OccupationHx.IsPastColumn.ColumnName, DbType.Boolean);
        }
        /* End 06/01/2016 Syed Zia, MiscHx Occupation Table*/

        /* Start 07/01/2016 Syed Zia, MiscHx CaffeineIntake Hx Parameter*/
        private void CreateMiscHxCaffeineIntakeHxInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(12);
            dbManager.AddInsertUpdateParameters(0, PARM_CAFFEINEINTAKEHX_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FREQUENCY_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_HARMFUL, ds.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_CaffeineIntakHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateMiscHxCaffeineIntakeHxUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(12);
            dbManager.AddInsertUpdateParameters(0, PARM_CAFFEINEINTAKEHX_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_FREQUENCY_ID, ds.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_HARMFUL, ds.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_CaffeineIntakHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateMiscHxTraveHxInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(13);
            dbManager.AddInsertUpdateParameters(0, PARM_TRAVEL_ID, ds.SocialHx_MiscHx_TravelHx.TravelHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_TravelHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_TRAVEL_FROM_DATE, ds.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_TRAVEL_TO_DATE, ds.SocialHx_MiscHx_TravelHx.TodateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TRAVEL_COMMENTS, ds.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_TravelHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_TravelHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_TravelHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_TravelHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_TravelHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_TRAVEL_LOCATION, ds.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_SOAPTEXT_TRAVEL, ds.SocialHx_MiscHx_TravelHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateMiscHxTravelHxUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(13);
            dbManager.AddInsertUpdateParameters(0, PARM_TRAVEL_ID, ds.SocialHx_MiscHx_TravelHx.TravelHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_TravelHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_TRAVEL_FROM_DATE, ds.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_TRAVEL_TO_DATE, ds.SocialHx_MiscHx_TravelHx.TodateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_TRAVEL_COMMENTS, ds.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_TravelHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_TravelHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_TravelHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_TravelHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_TravelHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_TRAVEL_LOCATION, ds.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_SOAPTEXT_TRAVEL, ds.SocialHx_MiscHx_TravelHx.SoapTextColumn.ColumnName, DbType.String);
        }


        /* End 08/01/2016 Syed Zia, MiscHx CaffeineIntake Hx Parameter*/

        #endregion

        #region "Support Functions  Sleep History"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersSleepHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SLEEPHX_ID, ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SLEEPHX_ID, ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_SleepHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_SleepHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateUpdateParametersSleepHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(11);


            dbManager.AddInsertUpdateParameters(0, PARM_SLEEPHX_ID, ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_SleepHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_SleepHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateInsertParametersSleepHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(11);


            dbManager.AddInsertUpdateParameters(0, PARM_SLEEPHX_ID, ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);


            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_SleepHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(3, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName, DbType.Double);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_SleepHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName, DbType.String);
        }



        #endregion

        #region "Support Functions  Exercises History"
        private void CreateParametersExercisesHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EXERCISESHX_ID, ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EXERCISESHX_ID, ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_TYPE_ID, ds.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_DIET_ID, ds.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_ExercisesHx.SleepHoursColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_COMMENTS, ds.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_ExercisesHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName, DbType.String);
        }


        private void CreateInsertParametersExercisesHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(13);

            dbManager.AddInsertUpdateParameters(0, PARM_EXERCISESHX_ID, ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);


            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_DIET_ID, ds.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_ExercisesHx.SleepHoursColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_COMMENTS, ds.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(7, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_ExercisesHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateUpdateParametersExercisesHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(13);

            dbManager.AddInsertUpdateParameters(0, PARM_EXERCISESHX_ID, ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn.ColumnName, DbType.Int64);


            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_TYPE_ID, ds.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_DIET_ID, ds.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(5, PARM_SLEEP_HOURS, ds.SocialHx_MiscHx_ExercisesHx.SleepHoursColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_COMMENTS, ds.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(7, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_ExercisesHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Support Functions  Housing History"
        private void CreateParametersHousingHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_HOUSINGHX_ID, ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_HOUSINGHX_ID, ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_HousingHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PRESENT, ds.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PAST, ds.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_HousingHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateInsertParametersHousingHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(12);


            dbManager.AddInsertUpdateParameters(0, PARM_HOUSINGHX_ID, ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);


            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_HousingHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PRESENT, ds.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PAST, ds.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_HousingHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName, DbType.String);
        }

        private void CreateUpdateParametersHousingHx(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(12);


            dbManager.AddInsertUpdateParameters(0, PARM_HOUSINGHX_ID, ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_MISCHX_ID, ds.SocialHx_MiscHx_HousingHx.MiscHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_STATUS_ID, ds.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_PRESENT, ds.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PAST, ds.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_HousingHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAPTEXT_BY, ds.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Support Functions Social History"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersSocialHx(IDBManager dbManager, DSSocialHistory ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, ds.SocialHx.SocialHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, ds.SocialHx.SocialHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.SocialHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SOCIALHX_DATE, ds.SocialHx.SocialHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_UNREMARKABLE, ds.SocialHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.SocialHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.SocialHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.SocialHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.SocialHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.SocialHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.SocialHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_NOTE_ID, ds.SocialHx.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_USER_ID, MDVSession.Current.AppUserId.ToString());
            dbManager.AddParameters(12, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            dbManager.AddParameters(13, PARM_MU_ALERTS_COUNT, ds.SocialHx.MUAlertsCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

        }

        #endregion

        #region Support Functions MiscHx_Component

        private void CreateMiscHx_ComponentInsertParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateInsertParameters(10);

            dbManager.AddInsertUpdateParameters(0, PARM_COMPONENT_ID, ds.SocialHx_MiscHx_Component.ComponentIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_USER_ID, ds.SocialHx_MiscHx_Component.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_COMPONENT_NAME, ds.SocialHx_MiscHx_Component.ComponentNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_COMPONENT_ORDER, ds.SocialHx_MiscHx_Component.ComponentOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_SORT_ORDER, ds.SocialHx_MiscHx_Component.SortOrderColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_Component.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SocialHx_MiscHx_Component.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SocialHx_MiscHx_Component.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_Component.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_Component.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }
        private void CreateMiscHx_ComponentUpdateParameters(IDBManager dbManager, DSSocialHistory ds)
        {
            dbManager.CreateUpdateParameters(10);

            dbManager.AddInsertUpdateParameters(0, PARM_COMPONENT_ID, ds.SocialHx_MiscHx_Component.ComponentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_USER_ID, ds.SocialHx_MiscHx_Component.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_COMPONENT_NAME, ds.SocialHx_MiscHx_Component.ComponentNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_COMPONENT_ORDER, ds.SocialHx_MiscHx_Component.ComponentOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_SORT_ORDER, ds.SocialHx_MiscHx_Component.SortOrderColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SocialHx_MiscHx_Component.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SocialHx_MiscHx_Component.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SocialHx_MiscHx_Component.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SocialHx_MiscHx_Component.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SocialHx_MiscHx_Component.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region "Sexual History Insert/Update/Delete/Select"
        public DSSocialHistory InsertSexualHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersSexualHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_INSERT, ds, ds.SocialHx_SexualHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::InsertSexualHistory", PROC_SOCIALHX_SEXUALHX_INSERT, ex);
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
        public DSSocialHistory UpdateSexualHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersSexualHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_UPDATE, ds, ds.SocialHx_SexualHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::UpdateSexualHistory", PROC_SOCIALHX_SEXUALHX_UPDATE, ex);
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
        public DSSocialHistory InsertUpdateSexualHistory(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_SexualHx.GetChanges();
                dbManager.BeginTransaction();

                CreateSexualHxInsertParameters(dbManager, ds);
                CreateSexualHxUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_INSERT, PROC_SOCIALHX_SEXUALHX_UPDATE, ds, ds.SocialHx_SexualHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_SexualHx.Rows[0][ds.SocialHx_SexualHx.SexualHxIdColumn].ToString(), null, ds.SocialHx_SexualHx.Rows[0][ds.SocialHx_SexualHx.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateSexualHistory", PROC_SOCIALHX_TOBACCO_INSERT + " " + PROC_SOCIALHX_SEXUALHX_UPDATE, ex);
                throw ex;
            }
        }
        public string DeleteSexualHistory(string SexualHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SEXUALHX_ID, SexualHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::DeleteSexualHistory", PROC_SOCIALHX_SEXUALHX_DELETE, ex);
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
        public DSSocialHistory LoadSexualHistory(long SexualHxId, long SocialHxId, string isViewSocialHx = "", string isPrintSocialHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (SexualHxId <= 0)
                    dbManager.AddParameters(0, PARM_SEXUALHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEXUALHX_ID, SexualHxId);
                if (SocialHxId <= 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_SELECT, ds, ds.SocialHx_SexualHx.TableName);
                if (ds.SocialHx_SexualHx.Rows.Count > 0)
                {
                    if (SocialHxId > 0)
                    {
                        DataTable dtTemp = ds.SocialHx_SexualHx;
                        if (dtTemp != null)
                        {
                            if (isViewSocialHx == "1" || isPrintSocialHx == "1")
                            {
                                bool isViewAction = isViewSocialHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSocialHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_SexualHx.Rows[0][ds.SocialHx_SexualHx.SexualHxIdColumn].ToString(), null, ds.SocialHx_SexualHx.Rows[0][ds.SocialHx_SexualHx.SocialHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALSocialHistory::LoadSexualHistory", PROC_SOCIALHX_SEXUALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Tobacco Insert/Update/Delete/Select"

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle insert of Tobacco

        public DSSocialHistory InsertTobaccoHistory(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_Tobacco.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersTobacco(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_INSERT, ds, ds.SocialHx_Tobacco.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.TobaccoIdColumn].ToString(), null, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSocialHistory::InsertTobaccoHistory", PROC_SOCIALHX_TOBACCO_INSERT, ex);
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
        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle Update of Tobacco
        public DSSocialHistory UpdateTobaccoHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersTobacco(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_UPDATE, ds, ds.SocialHx_Tobacco.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::UpdateTobaccoHistory", PROC_SOCIALHX_TOBACCO_UPDATE, ex);
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

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle insert/update of Tobacco
        public DSSocialHistory InsertUpdateTobaccoHistory(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_Tobacco.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateTobaccoInsertParameters(dbManager, ds);
                CreateTobaccoUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_INSERT, PROC_SOCIALHX_TOBACCO_UPDATE, ds, ds.SocialHx_Tobacco.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.TobaccoIdColumn].ToString(), null, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateTobaccoHistory", PROC_SOCIALHX_TOBACCO_INSERT + " " + PROC_SOCIALHX_TOBACCO_UPDATE, ex);
                throw ex;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle delete of Tobacco

        public string DeleteTobaccoHistory(string TobaccoId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TOBACCO_ID, TobaccoId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::DeleteTobaccoHistory", PROC_SOCIALHX_TOBACCO_DELETE, ex);
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

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle Load of Tobacco
        public DSSocialHistory LoadTobaccoHistory(long TobaccoId, long SocialHxId, string isViewSocialHx = "", string isPrintSocialHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (TobaccoId <= 0)
                    dbManager.AddParameters(0, PARM_TOBACCO_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TOBACCO_ID, TobaccoId);
                if (SocialHxId <= 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_SELECT, ds, ds.SocialHx_Tobacco.TableName);
                if (ds.SocialHx_Tobacco.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.SocialHx_Tobacco.Rows[0]["TobaccoId"]) > 0)
                    {

                        DataTable dtTemp = ds.SocialHx_Tobacco;
                        if (dtTemp != null)
                        {
                            if (isViewSocialHx == "1" || isPrintSocialHx == "1")
                            {
                                bool isViewAction = isViewSocialHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSocialHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.TobaccoIdColumn].ToString(), null, ds.SocialHx_Tobacco.Rows[0][ds.SocialHx_Tobacco.SocialHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALSocialHistory::LoadTobaccoHistory", PROC_SOCIALHX_TOBACCO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Alcohol Insert/Update/Delete/Select"

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle insert of Alcohol
        public DSSocialHistory InsertAlcoholHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersAlcohol(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_INSERT, ds, ds.SocialHx_Alcohol.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::InsertAlcoholHistory", PROC_SOCIALHX_ALCOHOL_INSERT, ex);
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

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle update of Alcohol

        public DSSocialHistory UpdateAlcoholHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersAlcohol(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_UPDATE, ds, ds.SocialHx_Alcohol.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::UpdateAlcoholHistory", PROC_SOCIALHX_ALCOHOL_UPDATE, ex);
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
        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle insert/update of Alcohol

        public DSSocialHistory InsertUpdateAlcoholHistory(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_Alcohol.GetChanges();
                dbManager.BeginTransaction();

                CreateAlcoholInsertParameters(dbManager, ds);
                CreateAlcoholUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_INSERT, PROC_SOCIALHX_ALCOHOL_UPDATE, ds, ds.SocialHx_Alcohol.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_Alcohol.Rows[0][ds.SocialHx_Alcohol.AlcoholIdColumn].ToString(), null, ds.SocialHx_Alcohol.Rows[0][ds.SocialHx_Alcohol.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateAlcoholHistory", PROC_SOCIALHX_ALCOHOL_INSERT + " " + PROC_SOCIALHX_ALCOHOL_UPDATE, ex);
                throw ex;
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle delete of Alcohol
        public string DeleteAlcoholHistory(string AlcoholId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ALCOHOL_ID, AlcoholId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::DeleteAlcoholHistory", PROC_SOCIALHX_ALCOHOL_DELETE, ex);
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

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle load of Alcohol
        public DSSocialHistory LoadAlcoholHistory(long AlcoholId, long SocialHxId, string isViewSocialHx = "", string isPrintSocialHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (AlcoholId <= 0)
                    dbManager.AddParameters(0, PARM_ALCOHOL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ALCOHOL_ID, AlcoholId);
                if (SocialHxId <= 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_SELECT, ds, ds.SocialHx_Alcohol.TableName);
                if (ds.SocialHx_Alcohol.Rows.Count > 0)
                {
                    if (SocialHxId > 0)
                    {
                        DataTable dtTemp = ds.SocialHx_Alcohol;
                        if (dtTemp != null)
                        {
                            if (isViewSocialHx == "1" || isPrintSocialHx == "1")
                            {
                                bool isViewAction = isViewSocialHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSocialHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_Alcohol.Rows[0][ds.SocialHx_Alcohol.AlcoholIdColumn].ToString(), null, ds.SocialHx_Alcohol.Rows[0][ds.SocialHx_Alcohol.SocialHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALSocialHistory::LoadAlcoholHistory", PROC_SOCIALHX_ALCOHOL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Drug Insert/Update/Delete/Select"
        public DSSocialHistory InsertDrugHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersDrug(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_INSERT, ds, ds.SocialHx_DrugAbuse.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::InsertDrugHistory", PROC_SOCIALHX_DRUGABUSE_INSERT, ex);
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
        public DSSocialHistory UpdateDrugHistory(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersDrug(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_UPDATE, ds, ds.SocialHx_DrugAbuse.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::UpdateDrugHistory", PROC_SOCIALHX_DRUGABUSE_UPDATE, ex);
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

        public DSSocialHistory InsertUpdateDrugHistory(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_DrugAbuse.GetChanges();
                dbManager.BeginTransaction();

                CreateDrugInsertParameters(dbManager, ds);
                CreateDrugUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_INSERT, PROC_SOCIALHX_DRUGABUSE_UPDATE, ds, ds.SocialHx_DrugAbuse.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_DrugAbuse.Rows[0][ds.SocialHx_DrugAbuse.DrugAbuseIdColumn].ToString(), null, ds.SocialHx_DrugAbuse.Rows[0][ds.SocialHx_DrugAbuse.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateDrugHistory", PROC_SOCIALHX_DRUGABUSE_INSERT + " " + PROC_SOCIALHX_DRUGABUSE_UPDATE, ex);
                throw ex;
            }
        }

        public string DeleteDrugHistory(string DrugAbuseId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DRUG_ABUSE_ID, DrugAbuseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::DeleteDrugHistory", PROC_SOCIALHX_DRUGABUSE_DELETE, ex);
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
        public DSSocialHistory LoadDrugHistory(long DrugAbuseId, long SocialHxId, string isViewSocialHx = "", string isPrintSocialHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (DrugAbuseId <= 0)
                    dbManager.AddParameters(0, PARM_DRUG_ABUSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DRUG_ABUSE_ID, DrugAbuseId);
                if (SocialHxId <= 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_SELECT, ds, ds.SocialHx_DrugAbuse.TableName);
                if (ds.SocialHx_DrugAbuse.Rows.Count > 0)
                {
                    if (SocialHxId > 0)
                    {
                        DataTable dtTemp = ds.SocialHx_DrugAbuse;
                        if (dtTemp != null)
                        {
                            if (isViewSocialHx == "1" || isPrintSocialHx == "1")
                            {
                                bool isViewAction = isViewSocialHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSocialHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_DrugAbuse.Rows[0][ds.SocialHx_DrugAbuse.DrugAbuseIdColumn].ToString(), null, ds.SocialHx_DrugAbuse.Rows[0][ds.SocialHx_DrugAbuse.SocialHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALSocialHistory::LoadDrugHistory", PROC_SOCIALHX_DRUGABUSE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region MiscHx Insert/Update/Delete/Select

        /* Start 06/01/2016 Muhammad Arshad MiscHx Parent Table*/
        public DSSocialHistory insertMiscHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersMiscHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MISCHX_INSERT, ds, ds.SocialHx_MiscHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::insertMiscHx", PROC_MISCHX_INSERT, ex);
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

        public DSSocialHistory updateMiscHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersMiscHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_UPDATE, ds, ds.SocialHx_MiscHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::updateMiscHx", PROC_MISCHX_UPDATE, ex);
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

        public DSSocialHistory InsertUpdateMiscHx(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx.GetChanges();
                dbManager.BeginTransaction();

                CreateMiscHxInsertParameters(dbManager, ds);
                CreateMiscHxUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_INSERT, PROC_MISCHX_UPDATE, ds, ds.SocialHx_MiscHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx.Rows[0][ds.SocialHx_MiscHx.MiscHxIdColumn].ToString(), null, ds.SocialHx_MiscHx.Rows[0][ds.SocialHx_MiscHx.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateMiscHx", PROC_MISCHX_INSERT + " " + PROC_MISCHX_UPDATE, ex);
                throw ex;
            }
        }

        public string deleteMiscHx(string MiscHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, MiscHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHx", PROC_MISCHX_DELETE, ex);
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

        public DSSocialHistory LoadMiscHx(long MiscHxId, long SocialHxId)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (MiscHxId <= 0)
                    dbManager.AddParameters(0, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MISCHX_ID, MiscHxId);
                if (SocialHxId <= 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_SELECT, ds, ds.SocialHx_MiscHx.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LoadMiscHx", PROC_MISCHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 06/01/2016 Muhammad Arshad MiscHx Parent Table*/
        #endregion

        /* Start 06/01/2016 Syed Zia, MiscHx Occupation Table*/
        #region MiscHx Occupation Insert/Update/Delete/Select

        public DSSocialHistory insertUpdateMiscHxOccupationHx(DSSocialHistory ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_OccupationHx.GetChanges();
                dbManager.BeginTransaction();

                CreateMiscHxOccupationHxInsertParameters(dbManager, ds);
                CreateMiscHxOccupationHxUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_OCCUPATIONHX_INSERT, PROC_MISCHX_OCCUPATIONHX_UPDATE, ds, ds.SocialHx_MiscHx_OccupationHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_OccupationHx.Rows[0][ds.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_OccupationHx.Rows[0][ds.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALSocialHistory::insertMiscHxOccupation", PROC_MISCHX_OCCUPATIONHX_INSERT + " " + PROC_MISCHX_OCCUPATIONHX_UPDATE, ex);
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

        //public DSSocialHistory updateMiscHxOccupationHx(DSSocialHistory ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        CreateParametersMiscHxOccupationHx(dbManager, ds, false);
        //        ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_OCCUPATIONHX_UPDATE, ds, ds.SocialHx_MiscHx_OccupationHx.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALSocialHistory::updateMiscHxOccupation", PROC_MISCHX_OCCUPATIONHX_UPDATE, ex);
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

        public string deleteMiscHxOccupationHx(string occupationHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_OCCUPATIONHX_ID, occupationHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_OCCUPATIONHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxOccupationHx", PROC_MISCHX_OCCUPATIONHX_DELETE, ex);
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
        public string deleteMiscHxTravelHx(string TravelHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TRAVEL_ID, TravelHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_TRAVELHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxTravelHx", PROC_MISCHX_TRAVELHX_DELETE, ex);
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

        public DSSocialHistory loadMiscHxOccupationHx(long occupationHxId, long miscHxId, string isView = "", string isPrint = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (occupationHxId <= 0)
                    dbManager.AddParameters(0, PARM_OCCUPATIONHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_OCCUPATIONHX_ID, occupationHxId);
                if (miscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, miscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_OCCUPATIONHX_SELECT, ds, ds.SocialHx_MiscHx_OccupationHx.TableName);
                //Start 28-10-2016 Humaira Yousaf for dbaudit log 
                if (miscHxId > 0)
                {
                    DataTable dtTemp = ds.SocialHx_MiscHx_OccupationHx;
                    if (dtTemp != null)
                    {
                        if (isView == "1" || isPrint == "1")
                        {
                            bool isViewAction = isView == "1" ? true : false;
                            bool isPrintAcion = isPrint == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_OccupationHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_OccupationHx.Rows[0][ds.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_OccupationHx.Rows[0][ds.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }

                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 28-10-2016 Humaira Yousaf for dbaudit log              
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxOccupationHx", PROC_MISCHX_OCCUPATIONHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion


        /* End 06/01/2016 Syed Zia, MiscHx Occupation Table*/

        /* End 07/01/2016 Syed Zia, MiscHx Caffeine IntakeHx Table*/
        #region MiscHx Caffeine IntakHx Insert/Update/Delete/Select
        public DSSocialHistory insertUpdateMiscHxCaffeineIntakeHx(DSSocialHistory ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_CaffeineIntakHx.GetChanges();
                dbManager.BeginTransaction();

                CreateMiscHxCaffeineIntakeHxInsertParameters(dbManager, ds);
                CreateMiscHxCaffeineIntakeHxUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_CAFFEINEINTAKEHX_INSERT, PROC_MISCHX_CAFFEINEINTAKEHX_UPDATE, ds, ds.SocialHx_MiscHx_CaffeineIntakHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_CaffeineIntakHx.Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_CaffeineIntakHx.Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALSocialHistory::insertUpdateMiscHxCaffeineIntakeHx", PROC_MISCHX_CAFFEINEINTAKEHX_INSERT + " " + PROC_MISCHX_CAFFEINEINTAKEHX_UPDATE, ex);
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

        //public DSSocialHistory updateMiscHxCaffeineIntakeHx(DSSocialHistory ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        CreateParametersMiscHxCaffeineIntakeHx(dbManager, ds, false);
        //        ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_CAFFEINEINTAKEHX_UPDATE, ds, ds.SocialHx_MiscHx_CaffeineIntakHx.TableName);
        //        ds.AcceptChanges();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALSocialHistory::updateMiscHxCaffeineIntakeHx", PROC_MISCHX_CAFFEINEINTAKEHX_UPDATE, ex);
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

        public string deleteMiscHxCaffeineIntakeHx(string caffeineIntakHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CAFFEINEINTAKEHX_ID, caffeineIntakHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_CAFFEINEINTAKEHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxCaffeineIntakeHx", PROC_MISCHX_CAFFEINEINTAKEHX_DELETE, ex);
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

        public DSSocialHistory loadMiscHxCaffeineIntakeHx(long caffeineIntakHxId, long miscHxId, string isViewAction = "", string isPrintAction = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (caffeineIntakHxId <= 0)
                    dbManager.AddParameters(0, PARM_CAFFEINEINTAKEHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CAFFEINEINTAKEHX_ID, caffeineIntakHxId);
                if (miscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, miscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_CAFFEINEINTAKEHX_SELECT, ds, ds.SocialHx_MiscHx_CaffeineIntakHx.TableName);
                //Start 28-10-2016 Humaira Yousaf for dbaudit log 
                if (miscHxId > 0)
                {

                    DataTable dtTemp = ds.SocialHx_MiscHx_CaffeineIntakHx;
                    if (dtTemp != null)
                    {
                        if (isViewAction == "1" || isPrintAction == "1")
                        {
                            bool isView = isViewAction == "1" ? true : false;
                            bool isPrint = isPrintAction == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_CaffeineIntakHx.Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_CaffeineIntakHx.Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn].ToString(), isView, isPrint);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 28-10-2016 Humaira Yousaf for dbaudit log 
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxCaffeineIntakeHx", PROC_MISCHX_CAFFEINEINTAKEHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region MiscHx Travel Hx Insert/Update/Delete/Select
        public DSSocialHistory insertUpdateMiscHxTravelHx(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_TravelHx.GetChanges();
                dbManager.BeginTransaction();

                CreateMiscHxTraveHxInsertParameters(dbManager, ds);
                CreateMiscHxTravelHxUpdateParameters(dbManager, ds);
                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_TRAVELHX_INSERT, PROC_MISCHX_TRAVELHX_UPDATE, ds, ds.SocialHx_MiscHx_TravelHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.TravelHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.MiscHxIdColumn].ToString(), false, false, false, ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.PatientIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSocialHistory::insertUpdateMiscHxTravelHx", PROC_MISCHX_TRAVELHX_INSERT + " " + PROC_MISCHX_TRAVELHX_UPDATE, ex);
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
        public DSSocialHistory loadMiscHxTravelHx(long TravelHxId, long miscHxId, string isViewAction = "", string isPrintAction = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (TravelHxId <= 0)
                    dbManager.AddParameters(0, PARM_TRAVEL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TRAVEL_ID, TravelHxId);
                if (miscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, miscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_TRAVELHX_SELECT, ds, ds.SocialHx_MiscHx_TravelHx.TableName);

                if (miscHxId > 0)
                {
                    DataTable dtTemp = ds.SocialHx_MiscHx_TravelHx;
                    if (dtTemp != null)
                    {
                        if (isViewAction == "1" || isPrintAction == "1")
                        {
                            bool isView = isViewAction == "1" ? true : false;
                            bool isPrint = isPrintAction == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_TravelHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.TravelHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.MiscHxIdColumn].ToString(), isView, isPrint);
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
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxTravelHx", PROC_MISCHX_TRAVELHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory FillMiscHxOccupationHx(long OccupationHxId, long PatientId, long RowsPerPage = 15, long Page = 1)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                if (OccupationHxId == 0)
                    dbManager.AddParameters(0, PARM_OCCUPATIONHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_OCCUPATIONHX_ID, OccupationHxId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, Page);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ParameterDirection.Output);

                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_OCCUPATIONHX_FILL, ds, ds.SocialHx_MiscHx_OccupationHx.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::FillMiscHxOccupationHx", PROC_MISCHX_OCCUPATIONHX_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory FillMiscHxTravelHx(long TravelHxId, long PatientId, long RowsPerPage = 15, long Page = 1)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (TravelHxId == 0)
                    dbManager.AddParameters(0, PARM_TRAVEL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TRAVEL_ID, TravelHxId);
                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                dbManager.AddParameters(2, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_PAGE_NUMBER, Page);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ParameterDirection.Output);

                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_TRAVELHX_FILL, ds, ds.SocialHx_MiscHx_TravelHx.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::FillMiscHxTravelHx", PROC_MISCHX_TRAVELHX_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /* End 08/01/2016 Syed Zia, MiscHx Occupation Table*/

        #region MiscHx Sleep Insert/Update/Delete/Select

        /* Start 06/01/2016 Farooq Ahmad MiscHx Sleep Table*/
        public DSSocialHistory insertMiscHxSleepHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersSleepHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MISCHX_SLEEPHX_INSERT, ds, ds.SocialHx_MiscHx_SleepHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::insertMiscHxSleepHx", PROC_MISCHX_SLEEPHX_INSERT, ex);
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

        public DSSocialHistory updateMiscHxSleepHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersSleepHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_SLEEPHX_UPDATE, ds, ds.SocialHx_MiscHx_SleepHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::updateMiscHxSleepHx", PROC_MISCHX_SLEEPHX_UPDATE, ex);
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

        public DSSocialHistory insertUpdateMiscHxSleepHx(DSSocialHistory ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_SleepHx.GetChanges();
                dbManager.BeginTransaction();

                this.CreateInsertParametersSleepHx(dbManager, ds);
                this.CreateUpdateParametersSleepHx(dbManager, ds);

                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_SLEEPHX_INSERT, PROC_MISCHX_SLEEPHX_UPDATE, ds, ds.SocialHx_MiscHx_SleepHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_SleepHx.Rows[0][ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_SleepHx.Rows[0][ds.SocialHx_MiscHx_SleepHx.MiscHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateMiscHx", PROC_MISCHX_SLEEPHX_INSERT + " " + PROC_MISCHX_SLEEPHX_UPDATE, ex);
                throw ex;
            }
        }





        public string deleteMiscHxSleepHx(string SleepHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SLEEPHX_ID, SleepHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_SLEEPHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxSleepHx", PROC_MISCHX_SLEEPHX_DELETE, ex);
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

        public DSSocialHistory loadMiscHxSleepHx(long SleepHxId, long MiscHxId, string isView = "", string isPrint = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //                dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (SleepHxId <= 0)
                    dbManager.AddParameters(0, PARM_SLEEPHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SLEEPHX_ID, SleepHxId);

                if (MiscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, MiscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_SLEEPHX_SELECT, ds, ds.SocialHx_MiscHx_SleepHx.TableName);
                //Start 28-10-2016 Humaira Yousaf for dbaudit log 
                if (MiscHxId > 0)
                {

                    DataTable dtTemp = ds.SocialHx_MiscHx_SleepHx;
                    if (dtTemp != null)
                    {
                        if (isView == "1" || isPrint == "1")
                        {
                            bool isViewAction = isView == "1" ? true : false;
                            bool isPrintAcion = isPrint == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_SleepHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_SleepHx.Rows[0][ds.SocialHx_MiscHx_SleepHx.SleepHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_SleepHx.Rows[0][ds.SocialHx_MiscHx_SleepHx.MiscHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }

                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 28-10-2016 Humaira Yousaf for dbaudit log 
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxSleepHx", PROC_MISCHX_SLEEPHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 06/01/2016 Farooq Ahmad MiscHx Sleep Table*/
        #endregion

        #region MiscHx Exercises Insert/Update/Delete/Select

        /* Start 07/01/2016 Farooq Ahmad MiscHx Exercises Table*/
        public DSSocialHistory insertMiscHxExercisesHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersExercisesHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MISCHX_EXERCISESHX_INSERT, ds, ds.SocialHx_MiscHx_ExercisesHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::insertMiscHxExercisesHx", PROC_MISCHX_EXERCISESHX_INSERT, ex);
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

        public DSSocialHistory updateMiscHxExercisesHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersExercisesHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_EXERCISESHX_UPDATE, ds, ds.SocialHx_MiscHx_ExercisesHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::updateMiscHxExercisesHx", PROC_MISCHX_EXERCISESHX_UPDATE, ex);
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

        public string deleteMiscHxExercisesHx(string ExercisesHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_EXERCISESHX_ID, ExercisesHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_EXERCISESHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxExercisesHx", PROC_MISCHX_EXERCISESHX_DELETE, ex);
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

        public DSSocialHistory insertUpdateMiscHxExercisesHx(DSSocialHistory ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_ExercisesHx.GetChanges();
                dbManager.BeginTransaction();

                this.CreateInsertParametersExercisesHx(dbManager, ds);
                this.CreateUpdateParametersExercisesHx(dbManager, ds);

                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_EXERCISESHX_INSERT, PROC_MISCHX_EXERCISESHX_UPDATE, ds, ds.SocialHx_MiscHx_ExercisesHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_ExercisesHx.Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_ExercisesHx.Rows[0][ds.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::insertUpdateMiscHxExercisesHx", PROC_MISCHX_EXERCISESHX_INSERT + " " + PROC_MISCHX_EXERCISESHX_UPDATE, ex);
                throw ex;
            }
        }



        public DSSocialHistory loadMiscHxExercisesHx(long ExercisesHxId, long MiscHxId, string isView = "", string isPrint = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (ExercisesHxId <= 0)
                    dbManager.AddParameters(0, PARM_EXERCISESHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EXERCISESHX_ID, ExercisesHxId);

                if (MiscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, MiscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_EXERCISESHX_SELECT, ds, ds.SocialHx_MiscHx_ExercisesHx.TableName);
                //Start 28-10-2016 Humaira Yousaf for dbaudit log 
                if (MiscHxId > 0)
                {
                    DataTable dtTemp = ds.SocialHx_MiscHx_ExercisesHx;
                    if (dtTemp != null)
                    {
                        if (isView == "1" || isPrint == "1")
                        {
                            bool isViewAction = isView == "1" ? true : false;
                            bool isPrintAcion = isPrint == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_ExercisesHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_ExercisesHx.Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_ExercisesHx.Rows[0][ds.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }

                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 28-10-2016 Humaira Yousaf for dbaudit log 
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxExercisesHx", PROC_MISCHX_EXERCISESHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 07/01/2016 Farooq Ahmad MiscHx Sleep Table*/
        #endregion

        #region MiscHx Housing Insert/Update/Delete/Select

        /* Start 07/01/2016 Farooq Ahmad MiscHx Housing Table*/
        public DSSocialHistory insertMiscHxHousingHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersHousingHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MISCHX_HOUSINGHX_INSERT, ds, ds.SocialHx_MiscHx_HousingHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::insertMiscHxHousingHx", PROC_MISCHX_HOUSINGHX_INSERT, ex);
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

        public DSSocialHistory updateMiscHxHousingHx(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersExercisesHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_HOUSINGHX_UPDATE, ds, ds.SocialHx_MiscHx_HousingHx.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::updateMiscHxHousingHx", PROC_MISCHX_HOUSINGHX_UPDATE, ex);
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

        public string deleteMiscHxHousingHx(string HousingHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HOUSINGHX_ID, HousingHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_HOUSINGHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHxHousingHx", PROC_MISCHX_HOUSINGHX_DELETE, ex);
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

        public DSSocialHistory insertUpdateMiscHxHousingHx(DSSocialHistory ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx_MiscHx_HousingHx.GetChanges();
                dbManager.BeginTransaction();

                this.CreateInsertParametersHousingHx(dbManager, ds);
                this.CreateUpdateParametersHousingHx(dbManager, ds);

                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_HOUSINGHX_INSERT, PROC_MISCHX_HOUSINGHX_UPDATE, ds, ds.SocialHx_MiscHx_HousingHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_HousingHx.Rows[0][ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_HousingHx.Rows[0][ds.SocialHx_MiscHx_HousingHx.MiscHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::insertUpdateMiscHxHousingHx", PROC_MISCHX_HOUSINGHX_INSERT + " " + PROC_MISCHX_HOUSINGHX_UPDATE, ex);
                throw ex;
            }
        }


        public DSSocialHistory loadMiscHxHousingHx(long HousingHxId, long MiscHxId, string isView = "", string isPrint = "")
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (HousingHxId <= 0)
                    dbManager.AddParameters(0, PARM_HOUSINGHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HOUSINGHX_ID, HousingHxId);

                if (MiscHxId <= 0)
                    dbManager.AddParameters(1, PARM_MISCHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MISCHX_ID, MiscHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_HOUSINGHX_SELECT, ds, ds.SocialHx_MiscHx_HousingHx.TableName);
                //Start 28-10-2016 Humaira Yousaf for dbaudit log 
                if (MiscHxId > 0)
                {
                    DataTable dtTemp = ds.SocialHx_MiscHx_HousingHx;
                    if (dtTemp != null)
                    {
                        if (isView == "1" || isPrint == "1")
                        {
                            bool isViewAction = isView == "1" ? true : false;
                            bool isPrintAcion = isPrint == "1" ? true : false;
                            if (ds.SocialHx_MiscHx_HousingHx.Rows.Count > 0)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx_MiscHx_HousingHx.Rows[0][ds.SocialHx_MiscHx_HousingHx.HousingHxIdColumn].ToString(), null, ds.SocialHx_MiscHx_HousingHx.Rows[0][ds.SocialHx_MiscHx_HousingHx.MiscHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }

                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 28-10-2016 Humaira Yousaf for dbaudit log               
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxExercisesHx", PROC_MISCHX_HOUSINGHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 07/01/2016 Farooq Ahmad MiscHx Housing Table*/
        #endregion

        #region "SocialHx Insert/Update/Delete/Select"
        /* Start 04/12/2015 Muhammad Irfan Social History Parent Table */
        public DSSocialHistory InsertSocialHx(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersSocialHx(dbManager, ds, true);
                ds = (DSSocialHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_INSERT, ds, ds.SocialHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString(), null, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALSocialHistory::InsertSocialHx", PROC_SOCIALHX_INSERT, ex);
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
        public DSHxMobileApp InsertSocialHxComponentsNative(DSHxMobileApp ds, string PatientId, string IsSynced)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.BeginTransaction();

                dbManager.CreateParameters(127);
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.SocialHxIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.SocialHxIdColumn.ColumnName]);
                dbManager.AddParameters(1, PARM_CREATED_BY, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(2, PARM_CREATED_ON, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.ModifiedOnColumn.ColumnName]);
                dbManager.AddParameters(5, PARM_TABBACCO_STATUS, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(6, PARM_TABBACCO_TYPE, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.TypeIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.TypeIdColumn.ColumnName]);
                dbManager.AddParameters(7, PARM_TABBACO_USAGE, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName]);
                dbManager.AddParameters(8, PARM_TABBACCO_FREQUENCY, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.FrequencyIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.FrequencyIdColumn.ColumnName]);
                dbManager.AddParameters(9, PARM_SOAPTEXT_TABBACCO, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.SoapTextColumn.ColumnName]);

                dbManager.AddParameters(10, PARM_ALCOHOL_STATUS, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(11, PARM_ALCOHOL_TYPE, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.TypeIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.TypeIdColumn.ColumnName]);
                dbManager.AddParameters(12, PARM_ALCOHOL_USAGE, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName]);
                dbManager.AddParameters(13, PARM_ALCOHOL_FREQUENCY, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.FrequencyIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.FrequencyIdColumn.ColumnName]);
                dbManager.AddParameters(14, PARM_SOAPTEXT_ALCOHOL, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(15, PARM_CREATEDBY_ALCOHOL, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(16, PARM_CREATEDON_ALCOHOL, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(17, PARM_MODIFIEDBY_ALCOHOL, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(18, PARM_MODIFIEDON_ALCOHOL, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(19, PARM_DRUG_STATUS, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(20, PARM_DRUG_TYPE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.DrugIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.DrugIdColumn.ColumnName]);
                dbManager.AddParameters(21, PARM_DRUG_USAGE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName]);
                dbManager.AddParameters(22, PARM_DRUG_FREQUENCY, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName]);
                dbManager.AddParameters(23, PARM_SOAPTEXT_DRUGABUSE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(24, PARM_CREATEDBY_DRUGABUSE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(25, PARM_CREATEDON_DRUGABUSE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(26, PARM_MODIFIEDBY_DRUGABUSE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(27, PARM_MODIFIEDON_DRUGABUSE, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(28, PARM_SEXUAL_STATUS, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(29, PARM_SEXUAL_PREFERENCES, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.PreferenceIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.PreferenceIdColumn.ColumnName]);
                dbManager.AddParameters(30, PARM_SEXUAL_PROTECTION, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName]);
                dbManager.AddParameters(31, PARM_SEXUAL_PAIN_INTERCOURSE, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName]);
                dbManager.AddParameters(32, PARM_SEXUAL_COMPLAINTS, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ComplaintIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ComplaintIdColumn.ColumnName]);
                dbManager.AddParameters(33, PARM_SEXUAL_EXPOSEDTOSTD, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName]);
                dbManager.AddParameters(34, PARM_SEXUAL_ABUSED, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName]);
                dbManager.AddParameters(35, PARM_SOAPTEXT_SEXUAL, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(36, PARM_CREATEDBY_SEXUAL, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(37, PARM_CREATEDON_SEXUAL, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(38, PARM_MODIFIEDBY_SEXUAL, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(39, PARM_MODIFIEDON_SEXUAL, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(40, PARM_MISCHX_OCCUPATION_STATUS, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(41, PARM_MISCHX_OCCUPATION_PRESENT, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.PresentColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.PresentColumn.ColumnName]);
                dbManager.AddParameters(42, PARM_MISCHX_OCCUPATION_PAST, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.PastColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.PastColumn.ColumnName]);
                dbManager.AddParameters(43, PARM_MISCHX_OCCUPATION_COMMENTS, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(44, PARM_SOAPTEXT_OCCUPATION, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(45, PARM_CREATEDBY_OCCUPATION, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(46, PARM_CREATEDON_OCCUPATION, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(47, PARM_MODIFIEDBY_OCCUPATION, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(48, PARM_MODIFIEDON_OCCUPATION, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(49, PARM_MISCHX_SLEEP_STATUS, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(50, PARM_MISCHX_SLEEP_HOURS, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName]);
                dbManager.AddParameters(51, PARM_MISCHX_SLEEP_COMMENTS, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(52, PARM_SOAPTEXT_SLEEP, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(53, PARM_CREATEDBY_SLEEP, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(54, PARM_CREATEDON_SLEEP, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(55, PARM_MODIFIEDBY_SLEEP, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(56, PARM_MODIFIEDON_SLEEP, ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_SleepHx.TableName].Rows[0][ds.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(57, PARM_MISCHX_EXERCISE_STATUS, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(58, PARM_MISCHX_EXERCISE_TYPE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName]);
                dbManager.AddParameters(59, PARM_MISCHX_EXERCISE_DIET, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName]);
                dbManager.AddParameters(60, PARM_MISCHX_EXERCISE_COMMENTS, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(61, PARM_SOAPTEXT_EXERCISE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(62, PARM_CREATEDBY_EXERCISE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(63, PARM_CREATEDON_EXERCISE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(64, PARM_MODIFIEDBY_EXERCISE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(65, PARM_MODIFIEDON_EXERCISE, ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_ExercisesHx.TableName].Rows[0][ds.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(66, PARM_MISCHX_HOUSING_STATUS, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(67, PARM_MISCHX_HOUSING_PRESENT, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName]);
                dbManager.AddParameters(68, PARM_MISCHX_HOUSING_PAST, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName]);
                dbManager.AddParameters(69, PARM_MISCHX_HOUSING_COMMENTS, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(70, PARM_SOAPTEXT_HOUSING, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(71, PARM_CREATEDBY_HOUSING, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(72, PARM_CREATEDON_HOUSING, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(73, PARM_MODIFIEDBY_HOUSING, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(74, PARM_MODIFIEDON_HOUSING, ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_HousingHx.TableName].Rows[0][ds.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(75, PARM_MISCHX_CAFFEINE_STATUS, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName]);
                dbManager.AddParameters(76, PARM_MISCHX_CAFFEINE_ISHARMFUL, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName]);
                dbManager.AddParameters(77, PARM_MISCHX_CAFFEINE_FREQUENCY, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName]);
                dbManager.AddParameters(78, PARM_MISCHX_CAFFEINE_COMMENTS, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(79, PARM_SOAPTEXT_CAFFEINE, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(80, PARM_CREATEDBY_CAFFEINE, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(81, PARM_CREATEDON_CAFFEINE, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(82, PARM_MODIFIEDBY_CAFFEINE, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(83, PARM_MODIFIEDON_CAFFEINE, ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows[0][ds.SocialHx_MiscHx_CaffeineIntakHx.ModifiedOnColumn.ColumnName]);

                dbManager.AddParameters(84, PARM_PATIENT_ID, PatientId == "" ? null : PatientId);

                dbManager.AddParameters(85, PARM_SEXUAL_PROTECTION_PERIOD, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(86, PARM_SEXUAL_USING_PROTECTION, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName]);

                dbManager.AddParameters(87, PARM_IS_SYNCED, IsSynced == "" ? null : IsSynced);

                dbManager.AddParameters(88, PARM_TOBACCO_Comments, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CommentsColumn.ColumnName]);
                dbManager.AddParameters(89, PARM_TOBACCO_CessationLength, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CessationLengthColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CessationLengthColumn.ColumnName]);
                dbManager.AddParameters(90, PARM_TOBACCO_CessationPeriod, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(91, PARM_TOBACCO_CounsellingPeriod, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(92, PARM_TOBACCO_CounsellingTopic, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName]);
                dbManager.AddParameters(93, PARM_TOBACCO_RecentlyQuit, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName]);
                dbManager.AddParameters(94, PARM_TOBACCO_WouldQuit, ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.bWouldQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Tobacco.TableName].Rows[0][ds.SocialHx_Tobacco.bWouldQuitColumn.ColumnName]);

                dbManager.AddParameters(95, PARM_ALCOHOL_Comments, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CommentsColumn.ColumnName]);
                dbManager.AddParameters(96, PARM_ALCOHOL_CessationLength, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CessationLengthColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CessationLengthColumn.ColumnName]);
                dbManager.AddParameters(97, PARM_ALCOHOL_CessationPeriod, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(98, PARM_ALCOHOL_CounsellingPeriod, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(99, PARM_ALCOHOL_CounsellingTopic, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName]);
                dbManager.AddParameters(100, PARM_ALCOHOL_RecentlyQuit, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName]);
                dbManager.AddParameters(101, PARM_ALCOHOL_WouldQuit, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bWouldQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bWouldQuitColumn.ColumnName]);
                dbManager.AddParameters(102, PARM_ALCOHOL_NotReadyToQuit, ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_Alcohol.TableName].Rows[0][ds.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName]);

                dbManager.AddParameters(103, PARM_DRUG_CessationLength, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName]);
                dbManager.AddParameters(104, PARM_DRUG_CessationPeriod, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName]);
                dbManager.AddParameters(105, PARM_DRUG_RecentlyQuit, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName]);
                dbManager.AddParameters(106, PARM_DRUG_WouldQuit, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName]);
                dbManager.AddParameters(107, PARM_DRUG_Comments, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.CommentsColumn.ColumnName]);

                dbManager.AddParameters(108, PARM_Travel_Comments, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(109, PARM_Travel_FromDate, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName]);
                dbManager.AddParameters(110, PARM_Travel_ToDate, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ToDateColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ToDateColumn.ColumnName]);
                dbManager.AddParameters(111, PARM_Travel_Location, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName]);
                dbManager.AddParameters(112, PARM_Travel_Status, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName]);

                dbManager.AddParameters(113, PARM_Travel_CreatedOn, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CreatedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CreatedOnColumn.ColumnName]);
                dbManager.AddParameters(114, PARM_Travel_CreatedBy, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CreatedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.CreatedByColumn.ColumnName]);
                dbManager.AddParameters(115, PARM_Travel_ModifiedBy, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ModifiedByColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ModifiedByColumn.ColumnName]);
                dbManager.AddParameters(116, PARM_Travel_ModifiedOn, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ModifiedOnColumn.ColumnName].ToString() == System.Data.SqlTypes.SqlDateTime.MinValue.ToString() ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.ModifiedOnColumn.ColumnName]);
                dbManager.AddParameters(117, PARM_Travel_SoapText, ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.SoapTextColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows[0][ds.SocialHx_MiscHx_TravelHx.SoapTextColumn.ColumnName]);
                dbManager.AddParameters(118, PARM_ROUTE_ID, ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_DrugAbuse.TableName].Rows[0][ds.SocialHx_DrugAbuse.RouteIdColumn.ColumnName]);
                dbManager.AddParameters(119, PARM_Sexual_Comments, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CommentsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.CommentsColumn.ColumnName]);
                dbManager.AddParameters(120, PARM_Sexual_Pregnacy_Duration, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName]);
                dbManager.AddParameters(121, PARM_LMP, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.LMPColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.LMPColumn.ColumnName]);

                dbManager.AddParameters(122, PARM_MISCHX_OCCUPATION_DETAIL, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName]);
                dbManager.AddParameters(123, PARM_OCCUPATION_START_DATE_NATIVE, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName]);
                dbManager.AddParameters(124, PARM_OCCUPATION_END_DATE_NATIVE, ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][ds.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName]);
                dbManager.AddParameters(125, PARM_STD_IDS, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.STDIdsColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.STDIdsColumn.ColumnName]);
                dbManager.AddParameters(126, PARM_PREGNANCY_STATUS, ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName] == "" ? null : ds.Tables[ds.SocialHx_SexualHx.TableName].Rows[0][ds.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName]);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_Tobacco.TableName);
                TableNames.Add(ds.SocialHx_Alcohol.TableName);
                TableNames.Add(ds.SocialHx_DrugAbuse.TableName);
                TableNames.Add(ds.SocialHx_SexualHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_OccupationHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_SleepHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_ExercisesHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_HousingHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_CaffeineIntakHx.TableName);
                TableNames.Add(ds.SocialHx_MiscHx_TravelHx.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_COMPONENTS_NATIVE, ds, TableNames);
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::InsertSocialHxComponentsNative", PROC_SOCIALHX_COMPONENTS_NATIVE, ex);
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

        public DSSocialHistory UpdateSocialHx(DSSocialHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SocialHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateParametersSocialHx(dbManager, ds, false);
                ds = (DSSocialHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_UPDATE, ds, ds.SocialHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString(), null, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSocialHistory::UpdateSocialHx", PROC_SOCIALHX_UPDATE, ex);
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

        public string DeleteSocialHx(string SocialHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSSocialHistory dsCurrentSocialHx = LoadSocialHx(0, Convert.ToInt64(SocialHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, SocialHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SOCIALHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentSocialHx.SocialHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, SocialHxId.ToString(), null, SocialHxId.ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALSocialHistory::DeleteSocialHx", PROC_SOCIALHX_DELETE, ex);
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

        public SocialHxModel LoadNoteSocialHx(long PatientId, long Userid, long EntityId, long NoteId)
        {
            SocialHxModel model = new SocialHxModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, Userid));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_SOCIALHX_SELECT_ForSoapText, parameters))
                {
                    while (reader.Read())
                    {


                        var properties = typeof(SocialHxModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALSocialHistory::LoadNoteSocialHx", PROC_SOCIALHX_SELECT_ForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSocialHistory LoadSocialHx(long PatientId, long SocialHxId, string isViewSocialHx = "", string isPrintSocialHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSocialHistory ds = new DSSocialHistory();
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

                if (SocialHxId == 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SELECT, ds, ds.SocialHx.TableName);
                if (ds.SocialHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.SocialHx.Rows[0]["SocialHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.SocialHx;
                        if (dtTemp != null)
                        {
                            if (isViewSocialHx == "1" || isPrintSocialHx == "1")
                            {
                                bool isViewAction = isViewSocialHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSocialHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString(), null, ds.SocialHx.Rows[0][ds.SocialHx.SocialHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALSocialHistory::LoadSocialHx", PROC_SOCIALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /* End 04/12/2015 Muhammad Irfan Social History Parent Table */
        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason: Function to get updated Soap text of  social history for progress note
           Created Date: Dec 15, 2015
       */
        public string updateSoapTextForSocialHX(string SocialHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SOCIALHX_ID, SocialHxId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_SOCIALHX).ToString();

                if (returnVal == "" || returnVal == "-1")
                    return "";
                else
                    throw new Exception(returnVal);


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::updateSoapTextForSocialHX", PROC_UPDATE_SOAPTEXT_FOR_SOCIALHX, ex);
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
        //end change azhar dec 15 ,2015
        #endregion

        #region "MiscHx_Component"
        /*
         * Author: Muhammad Arshad
         * Date: 13/01/2016
         * Overview: Insert/Update function for MiscHx_Component
         */

        public DSSocialHistory insertUpdateMiscHx_Component(DSSocialHistory ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateMiscHx_ComponentInsertParameters(dbManager, ds);
                this.CreateMiscHx_ComponentUpdateParameters(dbManager, ds);



                ds = (DSSocialHistory)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MISCHX_COMPONENT_INSERT, PROC_MISCHX_COMPONENT_UPDATE, ds, ds.SocialHx_MiscHx_Component.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::insertUpdateMiscHx_Component", PROC_MISCHX_COMPONENT_INSERT + " " + PROC_MISCHX_COMPONENT_UPDATE, ex);
                throw ex;
            }
        }

        /*
       * Author: Muhammad Arshad
       * Date: 13/01/2016
       * Overview: Delete function for MiscHx_Component
       */
        public string deleteMiscHx_Component(string ComponentId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_COMPONENT_ID, ComponentId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MISCHX_COMPONENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::deleteMiscHx_Component", PROC_MISCHX_COMPONENT_DELETE, ex);
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
        /*
       * Author: Muhammad Arshad
       * Date: 13/01/2016
       * Overview: Load function for MiscHx_Component
       */
        public DSSocialHistory loadMiscHx_Component(long componentId)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (componentId == 0)
                    dbManager.AddParameters(0, PARM_COMPONENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_COMPONENT_ID, componentId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MISCHX_COMPONENT_SELECT, ds, ds.SocialHx_MiscHx_Component.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHx_Component", PROC_MISCHX_COMPONENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Social History Lookups"
        public DSSocialHistory LookupSexualHxComplaints()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_COMPLAINTS_LOOKUP, ds, ds.SocialHx_SexualHx_Complaints.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxComplaints", PROC_SOCIALHX_SEXUALHX_COMPLAINTS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSexualHxPreferences()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_PREFERENCES_LOOKUP, ds, ds.SocialHx_SexualHx_Preferences.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxPreferences", PROC_SOCIALHX_SEXUALHX_PREFERENCES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSexualHxProtectionMethod()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_PROTECTIONMETHOD_LOOKUP, ds, ds.SocialHx_SexualHx_ProtectionMethod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxProtectionMethod", PROC_SOCIALHX_SEXUALHX_PROTECTIONMETHOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSexualHxProtectionPeriod()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_PROTECTIONPERIOD_LOOKUP, ds, ds.SocialHx_SexualHx_ProtectionPeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxProtectionPeriod", PROC_SOCIALHX_SEXUALHX_PROTECTIONPERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSexualHxStatus()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_STATUS_LOOKUP, ds, ds.SocialHx_SexualHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxStatus", PROC_SOCIALHX_SEXUALHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSexualHxSTD()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUALHX_STD_LOOKUP, ds, ds.SocialHx_SexualHx_STD.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSexualHxSTD", PROC_SOCIALHX_SEXUALHX_STD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //--------------------------------------------
        public DSSocialHistory LookupTobaccoCounsellingTopic()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TOBACCO_COUNSELLINGTOPIC_LOOKUP, ds, ds.SocialHx_Tobacco_CounsellingTopic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoCounsellingTopic", PROC_TOBACCO_COUNSELLINGTOPIC_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupTobaccoFrequency()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TOBACCO_FREQUENCY_LOOKUP, ds, ds.SocialHx_Tobacco_Frequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoFrequency", PROC_TOBACCO_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupTobaccoSmokingStatus()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TOBACCO_SMOKINGSTATUS_LOOKUP, ds, ds.SocialHx_Tobacco_SmokingStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoSmokingStatus", PROC_TOBACCO_SMOKINGSTATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupTobaccoType()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TOBACCO_TYPE_LOOKUP, ds, ds.SocialHx_Tobacco_Type.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoType", PROC_TOBACCO_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupSocialHxUsagePeriod()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_USAGEPERIOD_LOOKUP, ds, ds.SocialHx_UsagePeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSocialHxUsagePeriod", PROC_SOCIALHX_USAGEPERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //--------------------------------------------
        public DSSocialHistory LookupAlcoholCounsellingTopics()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_COUNSELLINGTOPICS_LOOKUP, ds, ds.SocialHx_Alcohol_CounsellingTopics.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupAlcoholCounsellingTopics", PROC_SOCIALHX_ALCOHOL_COUNSELLINGTOPICS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupAlcoholFrequency()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_FREQUENCY_LOOKUP, ds, ds.SocialHx_Alcohol_Frequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupAlcoholFrequency", PROC_SOCIALHX_ALCOHOL_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupAlcoholStatus()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_STATUS_LOOKUP, ds, ds.SocialHx_Alcohol_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupAlcoholStatus", PROC_SOCIALHX_ALCOHOL_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupAlcoholType()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_TYPE_LOOKUP, ds, ds.SocialHx_Alcohol_Type.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupAlcoholType", PROC_SOCIALHX_ALCOHOL_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //-------------------------------------------
        public DSSocialHistory LookupSocialHxCessationPeriod()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_CESSATION_PERIOD_LOOKUP, ds, ds.SocialHx_CessationPeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSocialHxCessationPeriod", PROC_SOCIALHX_CESSATION_PERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupDrugAbuseDrug()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_DRUG_LOOKUP, ds, ds.SocialHx_DrugAbuse_Drug.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupDrugAbuseDrug", PROC_SOCIALHX_DRUGABUSE_DRUG_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupDrugAbuseFrequencyDaily()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_FREQUENCYDAILY_LOOKUP, ds, ds.SocialHx_DrugAbuse_FrequencyDaily.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupDrugAbuseFrequencyDaily", PROC_SOCIALHX_DRUGABUSE_FREQUENCYDAILY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupDrugAbuseFrequencyMonthly()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_FREQUENCYMONTHLY_LOOKUP, ds, ds.SocialHx_DrugAbuse_FrequencyMonthly.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupDrugAbuseFrequencyMonthly", PROC_SOCIALHX_DRUGABUSE_FREQUENCYMONTHLY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupDrugAbuseRoute()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_ROUTE_LOOKUP, ds, ds.SocialHx_DrugAbuse_Route.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupDrugAbuseRoute", PROC_SOCIALHX_DRUGABUSE_ROUTE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory LookupDrugAbuseStatus()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_STATUS_LOOKUP, ds, ds.SocialHx_DrugAbuse_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupDrugAbuseStatus", PROC_SOCIALHX_DRUGABUSE_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* Start 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */

        public DSSocialHistory LookupSocialHxCounsellingPeriod()
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_COUNSELLING_PERIOD_LOOKUP, ds, ds.SocialHx_CounsellingPeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSocialHxCounsellingPeriod", PROC_SOCIALHX_COUNSELLING_PERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* Start 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_DietLookup */
        public DSSocialHistoryLookup lookupSocialHxMiscHxExercisesHxDiet()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_EXERCISEHX_DIET_LOOKUP, ds, ds.SocialHx_MiscHx_ExercisesHx_Diet.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSSocialHistoryLookup::lookupSocialHxMiscHxExercisesHxDiet", PROC_SOCIALHX_MISCHX_EXERCISEHX_DIET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_DietLookup */

        /* Start 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_TypeLookup */
        public DSSocialHistoryLookup lookupSocialHxMiscHxExercisesHxType()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_EXERCISEHX_TYPE_LOOKUP, ds, ds.SocialHx_MiscHx_ExercisesHx_Type.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSSocialHistoryLookup::lookupSocialHxMiscHxExercisesHxType", PROC_SOCIALHX_MISCHX_EXERCISEHX_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /* End 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_TypeLookup */

        /* Start 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_StatusLookup */
        public DSSocialHistoryLookup lookupSocialHxMiscHxExercisesHxStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_EXERCISEHX_STATUS_LOOKUP, ds, ds.SocialHx_MiscHx_ExercisesHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSSocialHistoryLookup::lookupSocialHxMiscHxExercisesHxStatus", PROC_SOCIALHX_MISCHX_EXERCISEHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /* End 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_ExercisesHx_StatusLookup */

        /* Start 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_SleepHx_StatusLookup */
        public DSSocialHistoryLookup lookupSocialHxMiscHxSleepHxStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_SLEEPHX_STATUS_LOOKUP, ds, ds.SocialHx_MiscHx_SleepHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSSocialHistoryLookup::lookupSocialHxMiscHxSleepHxStatus", PROC_SOCIALHX_MISCHX_SLEEPHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /* End 06/01/2016 Farooq Ahmad Lookup for sp_SocialHx_MiscHx_SleepHx_StatusLookup */

        /* End 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */

        /* Start 06/01/2016 Zia Lookup functions for MiscHx */
        public DSSocialHistoryLookup lookupSocialHxOccupationStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT_LOOKUP, ds, ds.SocialHx_MiscHx_OccupationHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::lookupSocialHxOccupationStatus", PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSocialHistoryLookup lookupSocialHxMiscHxHousingHxStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_HOUSINGHX_STATUS_LOOKUP, ds, ds.SocialHx_MiscHx_HousingHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::lookupSocialHxMiscHxHousingHxStatus", PROC_SOCIALHX_MISCHX_HOUSINGHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSocialHistoryLookup lookupSocialHxMiscHxCaffeineIntakeHxStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_CAFFEINE_INTAKEHX_STATUS_LOOKUP, ds, ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::lookupSocialHxMiscHxCaffeineIntakeHxStatus", PROC_SOCIALHX_MISCHX_CAFFEINE_INTAKEHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistoryLookup lookupSocialHxMiscHxTravelHxStatus()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_TRAVEL_HX_STATUS_LOOKUP, ds, ds.SocialHx_MiscHx_TravelHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::lookupSocialHxMiscHxTravelHxStatus", PROC_SOCIALHX_MISCHX_TRAVEL_HX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSSocialHistoryLookup lookupSocialHxMiscHxCaffeineIntakeHxFrequency()
        {
            DSSocialHistoryLookup ds = new DSSocialHistoryLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSocialHistoryLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_CAFFEINEINTAKEHX_FREQUENCY_LOOKUP, ds, ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::lookupSocialHxMiscHxCaffeineIntakeHxFrequency", PROC_SOCIALHX_MISCHX_CAFFEINEINTAKEHX_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /* End 06/01/2016 Zia Lookup functions for MiscHx */
        #endregion

        /*
            Change Implement BY: Muhammad Azhar Shahzad
            Reason: Functions For Soap Text and attachement detachment of social history with progress note
            Created Date: Dec 15, 2015
        */
        #region Notes and Social History
        /// <summary>
        /// Detaching Social History From Progress notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachSocialHxFromNotes(string socialHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(socialHxId))
                {
                    dbManager.AddParameters(0, PARM_SOCIALHX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SOCIALHX_ID, socialHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_SOCIALHX_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::detachSocialHxFromNotes", PROC_DETACH_SOCIALHX_FROM_NOTES, ex);
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
        /// Attaching Social History With Progress notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSSocialHistory attachSocialHxWithNotes(string socialHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSSocialHistory ds = new DSSocialHistory();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(socialHxId))
                {
                    dbManager.AddParameters(0, PARM_SOCIALHX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SOCIALHX_ID, socialHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_SOCIALHX_FROM_NOTES, ds, ds.SocialHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::attachSocialHxWithNotes", PROC_ATTACH_SOCIALHX_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // end azhar changed dec 15,2015
        #endregion

        #region "Native Functions"
        public List<HistoryLookupModel> LookupTobaccoTypeNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TOBACCO_TYPE_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.TobaccoTypeId = !String.IsNullOrEmpty(reader["TypeId"].ToString()) ? reader["TypeId"].ToString() : "";
                    model.TobaccoTypeDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoTypeNative", PROC_TOBACCO_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<HistoryLookupModel> LookupUsagePeriodNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SOCIALHX_USAGEPERIOD_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.TobaccoUsagePeriodId = !String.IsNullOrEmpty(reader["UsagePeriodId"].ToString()) ? reader["UsagePeriodId"].ToString() : "";
                    model.TobaccoUsagePeriodDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupUsagePeriodNative", PROC_SOCIALHX_USAGEPERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<HistoryLookupModel> LookupTobaccoFrequencyNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TOBACCO_FREQUENCY_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.TobaccoFrequencyId = !String.IsNullOrEmpty(reader["FrequencyId"].ToString()) ? reader["FrequencyId"].ToString() : "";
                    model.TobaccoFrequencyDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupTobaccoFrequencyNative", PROC_TOBACCO_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<HistoryLookupModel> LookupSocialHxStatusNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TOBACCO_SMOKINGSTATUS_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.SocialHxStatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                    model.SocialHxStatusDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    model.SocialHxSnomedCtCode = !String.IsNullOrEmpty(reader["SNOMEDCTCode"].ToString()) ? reader["SNOMEDCTCode"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSocialHxStatusNative", PROC_TOBACCO_SMOKINGSTATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<HistoryModel> LoadSocialHxNative(long PatientId, long SocialHxId = 0)
        {
            DSSocialHistory ds = new DSSocialHistory();
            List<HistoryModel> HistoryLookupList = new List<HistoryModel>();
            string ConnectionString = "";
            string ConnectionString2 = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            IDBManager dbManager_2 = ClientConfiguration.GetCustomerDBManager(ref ConnectionString2, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (SocialHxId == 0)
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SOCIALHX_ID, SocialHxId);

                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SELECT, ds, ds.SocialHx.TableName);
                if (ds.SocialHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.SocialHx.Rows[0]["SocialHxId"]) > 0)
                    {
                        Int64 SocHxId = Convert.ToInt64(ds.SocialHx.Rows[0]["SocialHxId"]);

                        dbManager_2.Open();
                        dbManager_2.CreateParameters(2);
                        dbManager_2.AddParameters(0, PARM_TOBACCO_ID, null);

                        dbManager_2.AddParameters(1, PARM_SOCIALHX_ID, SocHxId);

                        reader = (SqlDataReader)dbManager_2.ExecuteReader(CommandType.StoredProcedure, PROC_SOCIALHX_TOBACCO_SELECT);
                        HistoryModel model = null;
                        while (reader.Read())
                        {
                            model = new HistoryModel();
                            model.SocialHxId = !String.IsNullOrEmpty(reader["SocialHxId"].ToString()) ? reader["SocialHxId"].ToString() : "";
                            model.TobaccoId = !String.IsNullOrEmpty(reader["TobaccoId"].ToString()) ? reader["TobaccoId"].ToString() : "";
                            model.SocialHxStatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                            model.SocialHxTypeId = !String.IsNullOrEmpty(reader["TypeId"].ToString()) ? reader["TypeId"].ToString() : "";
                            model.SocialHxUsagePeriodId = !String.IsNullOrEmpty(reader["UsagePeriodId"].ToString()) ? reader["UsagePeriodId"].ToString() : "";
                            model.SocialHxFrequencyId = !String.IsNullOrEmpty(reader["FrequencyId"].ToString()) ? reader["FrequencyId"].ToString() : "";

                            HistoryLookupList.Add(model);
                        }
                        return HistoryLookupList;
                    }
                }

                return HistoryLookupList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LoadSocialHxNative", PROC_SOCIALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
                dbManager_2.Dispose();
            }
        }

        public List<HistoryModel> LookupSocialHxLogNative(long socialHxId)
        {
            List<HistoryModel> HistoryLookupList = new List<HistoryModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                //--------------
                dbManager.CreateParameters(6);

                if (socialHxId == 0)
                    dbManager.AddParameters(0, PARM_HX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HX_ID, socialHxId);

                dbManager.AddParameters(1, PARM_HX_TYPE, "SocialHx");

                dbManager.AddParameters(2, PARM_LOG_TYPE, "Current");

                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);


                dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, 500);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ParamDirection.Output);

                //--------------
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HXLOG_SELECT);
                HistoryModel model = null;
                while (reader.Read())
                {
                    model = new HistoryModel();
                    model.SocialHxSoapText = !String.IsNullOrEmpty(reader["SoapText"].ToString()) ? reader["SoapText"].ToString() : "";
                    model.SocialHxAction = !String.IsNullOrEmpty(reader["Action"].ToString()) ? reader["Action"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LookupSocialHxLogNative", PROC_HXLOG_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        #endregion

        #region Legacy Notes

        public List<SocialHx> NotesSocialHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<SocialHx> objList_SocialHx = new List<SocialHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_SOCIALHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        SocialHx model = new SocialHx();
                        var properties = typeof(SocialHx).GetProperties();
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
                        objList_SocialHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::NotesSocialHxSelect", PROC_NOTES_SOCIALHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_SocialHx;
        }

        #endregion Legacy Notes

        #region Fill SocialHx Mobile App

        public DSHxMobileApp LoadSocialHxMobileApp(long PatientId, string Status)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TABACCO_MOBILEAPP, ds, ds.SocialHx_Alcohol.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALSocialHistory::LoadSocialHx", PROC_SOCIALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp LoadTobaccoHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TABACCO_MOBILEAPP, ds, ds.SocialHx_Tobacco.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALSocialHistory::LoadTobaccoHistory", PROC_SOCIALHX_TABACCO_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHxMobileApp LoadAlcoholHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_Alcohol.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_ALCOHOL_MOBILEAPP, ds, TableNames);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::LoadAlcoholHistory", PROC_SOCIALHX_ALCOHOL_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHxMobileApp LoadDrugHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_DrugAbuse.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_DRUGABUSE_MOBILEAPP, ds, TableNames);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::LoadDrugHistory", PROC_SOCIALHX_DRUGABUSE_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHxMobileApp LoadSexualHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_SexualHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SEXUAL_MOBILEAPP, ds, TableNames);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSocialHistory::LoadSexualHistory", PROC_SOCIALHX_SEXUAL_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHxMobileApp loadMiscHxOccupationHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_OccupationHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_OCCUPATION_MOBILEAPP, ds, TableNames);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxOccupationHx", PROC_SOCIALHX_OCCUPATION_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp loadMiscHxSleepHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //                dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_SleepHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_SLEEP_MOBILEAPP, ds, TableNames);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxSleepHx", PROC_SOCIALHX_SLEEP_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp loadMiscHxExercisesHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_ExercisesHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_EXERCISE_MOBILEAPP, ds, TableNames);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxExercisesHx", PROC_SOCIALHX_EXERCISE_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp loadMiscHxHousingHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_HousingHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_HOUSING_MOBILEAPP, ds, TableNames);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxExercisesHx", PROC_SOCIALHX_HOUSING_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp loadMiscHxCaffeineIntakeHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_CaffeineIntakHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_CAFEINE_MOBILEAPP, ds, TableNames);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxCaffeineIntakeHx", PROC_SOCIALHX_CAFEINE_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHxMobileApp loadMiscHxTravelHxMobileApp(long PatientId, string Status)
        {
            DSHxMobileApp ds = new DSHxMobileApp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (Status == null)
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, null);
                else
                    dbManager.AddParameters(1, PARM_REQUEST_STATUS, Status);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                List<string> TableNames = new List<string>();
                TableNames.Add(ds.SocialHx_MiscHx_TravelHx.TableName);
                TableNames.Add(ds.SocialHx_ChangedList.TableName);

                ds = (DSHxMobileApp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_TRAVEL_MOBILEAPP, ds, TableNames);

                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALSocialHistory::loadMiscHxTravelHxMobileApp", PROC_SOCIALHX_CAFEINE_MOBILEAPP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSSocialHistory CheckSocialHxExists(long PatientId)
        {
            DSSocialHistory ds = new DSSocialHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                ds = (DSSocialHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHECK_SOCIALHX_EXISTS, ds, ds.SocialHx.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LoadSpecialty", PROC_CHECK_SOCIALHX_EXISTS, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

    }
}
