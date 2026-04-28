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
using MDVision.Common.Utilities;
using System.Data.SqlClient;
using MDVision.Model.Clinical.ROS;
using MDVision.Model.Clinical.Notes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALReviewofSystem
    {
        #region "Variables"

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_ROS_LOOKUP = "Clinical.sp_ROSSystemLookup";


        //Start By Khaleel Ur Rehman for look ups
        private const string PROC_ROS_AGGRAVEDBY_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailAggravedByLookup";
        private const string PROC_ROS_CHARACTERCSZ_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailCharacterCSZLookup";
        private const string PROC_ROS_CONTEXT_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailContextLookup";
        private const string PROC_ROS_FREQUENCY_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailFrequencyLookup";
        private const string PROC_ROS_RADIATION_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailRadiationLookup";
        private const string PROC_ROS_COURSE_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailCourseLookup";
        private const string PROC_ROS_SEVERITY_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailSeverityLookup";
        private const string PROC_ROS_PATTERN_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailPatternLookup";
        private const string PROC_ROS_DURATION_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailDurationLookup";
        private const string PROC_ROS_STATUS_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailStatusLookup";
        private const string PROC_ROS_RELIEVEDBY_LOOKUP = "Clinical.sp_ROSCharacteristicsDetailRelievedByLookup";
        //End By Khaleel Ur Rehman for look ups
        private const string PROC_ROSSYSTEM_RESET_PATIENTINFO = "Clinical.sp_ResetRosSystemPatientInfo";
        private const string PROC_ROSSYSTEM_CHARACTERISTICS_SELECT = "Clinical.sp_GetROSSystemCharacteristics";

        private const string PROC_ROSSYSTEM_INFO_SELECT = "Clinical.sp_ROSSystemInfoSelect";
        private const string PROC_ROSSYSTEM_SaveAs = "Clinical.sp_DefaultROSDataTemplateSaveAs";
        private const string PROC_ROSSYSTEM_PATIENT_SELECT = "Clinical.sp_ROSSystemPatientSelect";
        private const string PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_SELECT = "Clinical.sp_ROSSystemPatientCharacteristicsSelect";
        private const string PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_SELECT = "Clinical.sp_ROSCharacteristicsDetailsSelect";

        private const string PROC_ROSSYSTEM_INFO_INSERT = "Clinical.sp_ROSSystemInfoInsert";
        private const string PROC_ROSSYSTEM_PATIENT_INSERT = "Clinical.sp_ROSSystemPatientInsert";
        private const string PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_INSERT = "Clinical.sp_ROSSystemPatientCharacteristicsInsert";
        private const string PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_INSERT = "Clinical.sp_ROSCharacteristicsDetailsInsert";
        private const string PROC_ROSSYSTEM_PATIENT_INFO_INSERT = "Clinical.sp_ROSSystemUserInfoInsert";

        private const string PROC_ROSSYSTEM_INFO_UPDATE = "Clinical.sp_ROSSystemInfoUpdate";
        private const string PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_UPDATE = "Clinical.sp_ROSSystemPatientCharacteristicsUpdate";
        private const string PROC_ROSSYSTEM_PATIENT_UPDATE = "Clinical.sp_ROSSystemPatientUpdate";
        private const string PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_UPDATE = "Clinical.sp_ROSCharacteristicsDetailsUpdate";
        private const string PROC_ROSSYSTEM_PATIENT_INFO_UPDATE = "Clinical.sp_ROSSystemUserInfoUpdate";

        private const string PROC_ROSSYSTEM_INFO_DELETE = "Clinical.sp_ROSSystemInfoDelete";
        private const string PROC_ROSSYSTEM_PATIENT_DELETE = "Clinical.sp_ROSSystemPatientDelete";
        private const string PROC_ROSSYSTEM_PATIENT_BULK_DELETE = "Clinical.sp_ROSSystemPatientBulkDelete";
        private const string PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_DELETE = "Clinical.sp_ROSSystemPatientCharacteristicsDelete";
        private const string PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_DELETE = "Clinical.sp_ROSCharacteristicsDetailsDelete";

        private const string PROC_ROSSYSTEM_PATIENT_CHARC_DELETE = "Clinical.sp_ROSSysPatCharacReset";
        private const string PROC_ROS_SYSTEM_PATIENT_RESET = "Clinical.sp_ROSSystemPatientReset";

        private const string PROC_GET_ROSDATA_FOR_NOTES = "Clinical.sp_GetROSDataForNotes";

        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_ROSSYSTEM_INFO_ID = "@ROSSystemInfoId";
        private const string PARM_SYSTEM_ID = "@ROSSystemId";
        private const string PARM_ROSSYSTEM_PATIENT_ID = "@ROSSystemPatientID";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_ENTITY_ID = "@EntityId";

        private const string PARM_ROS_SYSTEM_USER_INFOID = "@ROSSystemUserInfoID";

        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAPTEXT = "@SoapText";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_ROS_TEMPLATE_ID = "@ROSTemplateID";
        private const string PARM_ROS_DATA_TEMPLATE_ID = "@ROSDataTemplateID";

        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ISNORMAL = "@IsNormal";
        private const string PARM_ROSSYSTEM_DATE = "@ROSSystemDate";

        private const string PARM_SORTING_ORDER = "@SortingOrder";

        private const string PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID = "@ROSSystemPatientCharacteristicsId";
        private const string PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID = "@ROSCharacteristicsDetailsId";
        private const string PARM_ROSSYSTEM_CHARACTERISTICSID = "@ROSSystemCharacteristicsId";
        private const string PARM_ISPOSITIVE = "@IsPositive";
        private const string PARM_PREVIOUSHISTORY = "@PreviousHistory";
        private const string PARM_ROSCHARACTERISTICSDETAIL_STATUSID = "@ROSCharacteristicsDetailStatusId";
        private const string PARM_ONSET = "@Onset";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_ROSCHARACTERISTICSDETAIL_DURATIONID = "@ROSCharacteristicsDetailDurationId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_PATTERNID = "@ROSCharacteristicsDetailPatternId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_SEVERITYID = "@ROSCharacteristicsDetailSeverityId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_COURSEID = "@ROSCharacteristicsDetailCourseId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_RADIATIONID = "@ROSCharacteristicsDetailRadiationId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_FREQUENCYID = "@ROSCharacteristicsDetailFrequencyId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_CONTEXTID = "@ROSCharacteristicsDetailContextId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_CHARACTERCSZID = "@ROSCharacteristicsDetailCharacterCSZId";
        private const string PARM_ROSCHARACTERISTICSDETAIL_AGGRAVEDBYID = "@ROSCharacteristicsDetailAggravedById";
        private const string PARM_ROSCHARACTERISTICSDETAIL_RELIEVEDBYID = "@ROSCharacteristicsDetailRelievedById";
        private const string PARM_LOCATION = "@Location";
        private const string PARM_PRECIPITATEDBY = "@PrecipitatedBY";
        private const string PARM_ASSOCIATEDWITH = "@AssociatedWith";
        private const string PARM_REMOVE_SYSCHARC_DETAILS = "@RemoveSystemCharcDetails";
        private const string PARM_REMOVE_SYSTEMDETAILS = "@removeSystemDetails";
        #endregion

        #region "Constructors"
        public DALReviewofSystem()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALReviewofSystem(SharedVariable SharedVariable)
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
        #region "Support Functions Review of System"
        private void createParametersROSSystemInfo(IDBManager dbManager, DSClinicalReviewofSystem ds, Boolean isInsert)
        {
            dbManager.CreateParameters(13);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_ROSSYSTEM_INFO_ID, ds.ROSSystemInfo.ROSSystemInfoIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROSSYSTEM_INFO_ID, ds.ROSSystemInfo.ROSSystemInfoIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_DESCRIPTION, ds.ROSSystemInfo.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ROSSYSTEM_DATE, ds.ROSSystemInfo.ROSSystemDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_ISNORMAL, ds.ROSSystemInfo.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.ROSSystemInfo.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ROSSystemInfo.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ROSSystemInfo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.ROSSystemInfo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.ROSSystemInfo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.ROSSystemInfo.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(10, PARM_NOTE_ID, ds.ROSSystemInfo.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_ROS_TEMPLATE_ID, ds.ROSSystemInfo.ROSTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_ROS_DATA_TEMPLATE_ID, ds.ROSSystemInfo.ROSDataTemplateIdColumn.ColumnName, DbType.Int64);
        }


        private void createParametersROSSystemPatient(IDBManager dbManager, DSClinicalReviewofSystem ds, Boolean isInsert)
        {
            dbManager.CreateParameters(5);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, ds.ROSSystemPatient.ROSSystemPatientIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, ds.ROSSystemPatient.ROSSystemPatientIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SYSTEM_ID, ds.ROSSystemPatient.ROSSystemIdColumn.ColumnName, DbType.Int32);
            //  dbManager.AddParameters(2, PARM_PATIENT_ID, ds.ROSSystemPatient.PatientIDColumn.ColumnName, DbType.Int64);
            // dbManager.AddParameters(3, PARM_SORTING_ORDER, ds.ROSSystemPatient.SortingOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ROSSystemPatient.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ISNORMAL, ds.ROSSystemPatient.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_ROSSYSTEM_INFO_ID, ds.ROSSystemPatient.ROSSystemInfoIDColumn.ColumnName, DbType.Int64);
            // dbManager.AddParameters(7, PARM_SOAPTEXT, ds.ROSSystemPatient.SoapTextColumn.ColumnName, DbType.String);

        }
        //Srart By Khaleel Ur Rehman on 29 Jan 2016 to create Params.
        private void createParametersForROSSystemPatientCharacteristics(IDBManager dbManager, DSClinicalReviewofSystem ds, Boolean isInsert)
        {
            dbManager.CreateParameters(5);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, ds.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, ds.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ROSSYSTEM_PATIENT_ID, ds.ROSSystemCharacteristics.ROSSystemPatientIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ROSSYSTEM_CHARACTERISTICSID, ds.ROSSystemCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.ROSSystemCharacteristics.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ISPOSITIVE, ds.ROSSystemCharacteristics.IsPositiveColumn.ColumnName, DbType.Boolean);
            //  dbManager.AddParameters(5, PARM_SOAPTEXT, ds.ROSSystemCharacteristics.SoapTextColumn.ColumnName, DbType.String);
        }
        //By Khaleel Ur Rehman on 29 Jan 2016 to create Params.
        private void createParametersForROSCharacteristicsDetails(IDBManager dbManager, DSClinicalReviewofSystem ds, Boolean isInsert)
        {
            dbManager.CreateParameters(19);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailsIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailsIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, ds.ROSCharacteristicsDetails.ROSSystemPatientCharacteristicsIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PREVIOUSHISTORY, ds.ROSCharacteristicsDetails.PreviousHistoryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ROSCHARACTERISTICSDETAIL_STATUSID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_ONSET, ds.ROSCharacteristicsDetails.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DURATION, ds.ROSCharacteristicsDetails.DurationColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_ROSCHARACTERISTICSDETAIL_DURATIONID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailDurationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_ROSCHARACTERISTICSDETAIL_PATTERNID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailPatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_ROSCHARACTERISTICSDETAIL_SEVERITYID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailSeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_ROSCHARACTERISTICSDETAIL_COURSEID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailCourseIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_ROSCHARACTERISTICSDETAIL_RADIATIONID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailRadiationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_ROSCHARACTERISTICSDETAIL_FREQUENCYID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailFrequencyIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_ROSCHARACTERISTICSDETAIL_CONTEXTID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailContextIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_ROSCHARACTERISTICSDETAIL_CHARACTERCSZID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_ROSCHARACTERISTICSDETAIL_AGGRAVEDBYID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailAggravedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_ROSCHARACTERISTICSDETAIL_RELIEVEDBYID, ds.ROSCharacteristicsDetails.ROSCharacteristicsDetailRelievedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARM_LOCATION, ds.ROSCharacteristicsDetails.LocationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_PRECIPITATEDBY, ds.ROSCharacteristicsDetails.PrecipitatedBYColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ASSOCIATEDWITH, ds.ROSCharacteristicsDetails.AssociatedWithColumn.ColumnName, DbType.String);
            // dbManager.AddParameters(24, PARM_SOAPTEXT, "", DbType.String);
        }


        //End By Khaleel Ur Rehman on 29 Jan 2016 to create Params.
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: To load ROS Systems
        /// Date : January 26, 2016
        #region "Load ROS Systems"
        public DSClinicalReviewofSystem lookupROS(long rosSystemInfoId, long userId, long noteId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);
                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);

                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);



                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_SELECT, ds, ds.ROSSystemInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::loadROSSystems", PROC_ROSSYSTEM_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: Lookup for ROS Systems
        /// Date : January 26, 2016
        #region "Lookup for ROS Systems"
        public DSClinicalReviewofSystem loadROSSystems(long rosSystemInfoId, long userId, long noteId, long templateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);

                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                if (templateId <= 0)
                    dbManager.AddParameters(3, PARM_ROS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ROS_TEMPLATE_ID, templateId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_LOOKUP, ds, ds.SystemLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROS", PROC_ROS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystem loadROSSystemsAgainstDT(long rosSystemInfoId, long userId, long noteId, long templateId, long ROSDataTemplateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);

                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                if (templateId <= 0)
                    dbManager.AddParameters(3, PARM_ROS_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ROS_TEMPLATE_ID, templateId);

                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(4, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_LOOKUP, ds, ds.SystemLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROS", PROC_ROS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailAggravedBy.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailAggravedBy()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_AGGRAVEDBY_LOOKUP, ds, ds.ROSCharacteristicsDetailAggravedBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailAggravedBy", PROC_ROS_AGGRAVEDBY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailCharacterCSZ.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailCharacterCSZ()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_CHARACTERCSZ_LOOKUP, ds, ds.ROSCharacteristicsDetailCharacterCSZ.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailCharacterCSZ", PROC_ROS_CHARACTERCSZ_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailContext.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailContext()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_CONTEXT_LOOKUP, ds, ds.ROSCharacteristicsDetailContext.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailContext", PROC_ROS_CONTEXT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailFrequency.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailFrequency()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_FREQUENCY_LOOKUP, ds, ds.ROSCharacteristicsDetailFrequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailFrequency", PROC_ROS_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailRadiation.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailRadiation()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_RADIATION_LOOKUP, ds, ds.ROSCharacteristicsDetailRadiation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailRadiation", PROC_ROS_RADIATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailCourse.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailCourse()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_COURSE_LOOKUP, ds, ds.ROSCharacteristicsDetailCourse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailCourse", PROC_ROS_COURSE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailSeverity.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailSeverity()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_SEVERITY_LOOKUP, ds, ds.ROSCharacteristicsDetailSeverity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailSeverity", PROC_ROS_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailPattern.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailPattern()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_PATTERN_LOOKUP, ds, ds.ROSCharacteristicsDetailPattern.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailPattern", PROC_ROS_PATTERN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailDuration.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailDuration()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_DURATION_LOOKUP, ds, ds.ROSCharacteristicsDetailDuration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailDuration", PROC_ROS_DURATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailStatus.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailStatus()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_STATUS_LOOKUP, ds, ds.ROSCharacteristicsDetailStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailStatus", PROC_ROS_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /*
         Author: Khaleel Ur Rehman.
         Purpose : Look up for ROSCharacteristicsDetailRelievedBy.
         Date : 27 January 2016.
         */
        public DSClinicalReviewofSystem lookupROSCharacteristicsDetailRelievedBy()
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROS_RELIEVEDBY_LOOKUP, ds, ds.ROSCharacteristicsDetailRelievedBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::lookupROSCharacteristicsDetailRelievedBy", PROC_ROS_RELIEVEDBY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        /// Author: ZeeshanAK
        /// Purpose: To load System Characteristics
        /// Date : January 27, 2016
        #region "Load System Characteristics"
        public DSClinicalReviewofSystem loadSystemCharacteristics(long systemId, long ROSSystemPatientID, long ROSDataTemplateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (systemId <= 0)
                    dbManager.AddParameters(0, PARM_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SYSTEM_ID, systemId);

                if (ROSSystemPatientID <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientID);
                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);

                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_CHARACTERISTICS_SELECT, ds, ds.SystemLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystem::loadSystemCharacteristics", PROC_ROSSYSTEM_CHARACTERISTICS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: Insert ROS System User Info 
        /// Date : February 17, 2016
        #region "Insert ROS System User Info"
        public DSClinicalReviewofSystem insertROSSystemUserInfo(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSSystemPatient(dbManager, ds, true);
                ds = (DSClinicalReviewofSystem)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_INFO_INSERT, ds, ds.ROSSystemUserInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insertROSSystemUserInfo", PROC_ROSSYSTEM_PATIENT_INFO_INSERT, ex);
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

        /// Author: ZeeshanAK
        /// Purpose: Update ROS System User Info 
        /// Date : February 17, 2016
        #region "Update ROS System User Info"
        public string updateROSSystemUserInfo(long ROSSystemUserInfoID, long UserID, string ROSSystemID, string ROSSystemPatientID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(4);

                if (ROSSystemUserInfoID > 0)
                {
                    dbManager.AddParameters(0, PARM_ROS_SYSTEM_USER_INFOID, ROSSystemUserInfoID);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ROS_SYSTEM_USER_INFOID, DBNull.Value);
                }
                if (UserID > 0)
                {
                    dbManager.AddParameters(1, PARM_USER_ID, UserID);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_USER_ID, DBNull.Value);

                }

                if (string.IsNullOrEmpty(ROSSystemPatientID))
                {
                    dbManager.AddParameters(2, PARM_ROSSYSTEM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientID);
                }
                if (string.IsNullOrEmpty(ROSSystemID))
                {
                    dbManager.AddParameters(3, PARM_SYSTEM_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_SYSTEM_ID, ROSSystemID);
                }

                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_INFO_UPDATE);
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSSystemUserInfo", PROC_ROSSYSTEM_PATIENT_INFO_UPDATE, ex);
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

        /// Author: ZeeshanAK
        /// Purpose: Delete Characteristics Details Info 
        /// Date : February 19, 2016
        #region "Delete Characteristics Details Info"
        public string deleteCharacteristicsDetails(long ROSSystemPatientCharacteristicsID, bool removeSystemCharcDetails)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, ROSSystemPatientCharacteristicsID);
                dbManager.AddParameters(1, PARM_REMOVE_SYSCHARC_DETAILS, removeSystemCharcDetails);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_CHARC_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteCharacteristicsDetails", PROC_ROSSYSTEM_PATIENT_CHARC_DELETE, ex);
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

        #region "ROS Patient Info Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for Insert ROs SystemInfo.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem insertROSPatientInfo(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSSystemInfo(dbManager, ds, true);
                ds = (DSClinicalReviewofSystem)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_INSERT, ds, ds.ROSSystemInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insertROSPatientInfo", PROC_ROSSYSTEM_INFO_INSERT, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for update Ros Sytem Info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem updateROSPatientInfo(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSSystemInfo(dbManager, ds, false);
                ds = (DSClinicalReviewofSystem)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_UPDATE, ds, ds.ROSSystemInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSPatientInfo", PROC_ROSSYSTEM_INFO_UPDATE, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for delete ROS System info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public string deleteROSPatientInfo(long ROSSystemInfoId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSSYSTEM_INFO_ID, ROSSystemInfoId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteROSPatientInfo", PROC_ROSSYSTEM_INFO_DELETE, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for load ROS system Info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem loadROSSystemInfo(long userId, long rosSystemInfoId, long noteId, long ROSDataTemplateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(4);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);

                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_SELECT, ds, ds.ROSSystemInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::loadROSSystemInfo", PROC_ROSSYSTEM_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalReviewofSystem loadROSSystemInfoForNote(long userId, long rosSystemInfoId, long noteId, long ROSDataTemplateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(4);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);

                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_INFO_SELECT, ds, ds.ROSSystemInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::loadROSSystemInfo", PROC_ROSSYSTEM_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public String getROSSystemInfo(long NotesID, long ROSDataTemplateId)
        {
            object returnValue;
            String infoID = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_NOTE_ID, NotesID);
                dbManager.AddParameters(1, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                dbManager.AddParameters(2, PARM_ROSSYSTEM_INFO_ID, "", DbType.Int64, ParamDirection.Output);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_SaveAs);
                if (returnValue != null)
                {
                    infoID = Convert.ToString(returnValue);
                }
                return infoID;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::getROSSystemInfo", PROC_ROSSYSTEM_SaveAs, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        #region "ROS Patient System  Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for Insert ROs SystemInfo.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem insertROSSystemPatient(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSSystemPatient(dbManager, ds, true);
                ds = (DSClinicalReviewofSystem)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_INSERT, ds, ds.ROSSystemPatient.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insertROSSystemPatient", PROC_ROSSYSTEM_PATIENT_INSERT, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for update Ros Sytem Info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem updateROSSystemPatient(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersROSSystemPatient(dbManager, ds, false);
                ds = (DSClinicalReviewofSystem)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_UPDATE, ds, ds.ROSSystemPatient.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSSystemPatient", PROC_ROSSYSTEM_PATIENT_UPDATE, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for delete ROS System info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public string deleteROSSystemPatient(long ROSSystemPatientID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientID);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteROSSystemPatient", PROC_ROSSYSTEM_PATIENT_DELETE, ex);
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

        public string deleteROSSystemPatients(string ROSSystemPatientIDs)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientIDs);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_BULK_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteROSSystemPatients", PROC_ROSSYSTEM_PATIENT_BULK_DELETE, ex);
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
        /// Author : Muhammad Azhar Shahzad
        /// Purpose : function for load ROS system Info.
        /// Date : January 29, 2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem loadROSSystemPatient(long userId, long ROSSystemPatientID, long noteId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (userId <= 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, userId);
                if (ROSSystemPatientID <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientID);
                if (noteId <= 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_SELECT, ds, ds.ROSSystemPatient.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::loadROSSystemPatient", PROC_ROSSYSTEM_PATIENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<RosSoapModel> GetROSSystemPatientInfoFromNotes(long userId, long ROSSystemPatientID, long noteId, long ROSDataTemptId, long ROSSystemInfoid = -1)
        {
            List<RosSoapModel> modelList = new List<RosSoapModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemptId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, noteId));
                parameters.Add(new SqlParameter(PARM_ROSSYSTEM_INFO_ID, ROSSystemInfoid));
                parameters.Add(new SqlParameter(PARM_USER_ID, userId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, null));

                using (var reader = dbManager.ExecuteReader(PROC_GET_ROSDATA_FOR_NOTES, parameters))
                {
                    while (reader.Read())
                    {
                        //if (RosSystemInfoId == 0)
                        //{
                        //    model.ROSSystemInfoID = MDVUtility.CheckStringNull(reader["ROSSystemInfoID"]);
                        //    model.RosSystemInfoIsNormal = MDVUtility.CheckStringNull(reader["RosSystemInfoIsNormal"]);
                        //    model.RosSystemDate = MDVUtility.CheckStringNull(reader["RosSystemDate"]);
                        //    model.RsoSystemInfoComments = MDVUtility.CheckStringNull(reader["RsoSystemInfoComments"]);
                        //    model.RosSystemInfoDescription = MDVUtility.CheckStringNull(reader["RosSystemInfoDescription"]);
                        //    model.RosSystemInfoIsNormal = MDVUtility.CheckStringNull(reader["ROSSystemInfoID"]);
                        //}

                        RosSoapModel model = new RosSoapModel();
                        var properties = typeof(RosSoapModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            object value = reader[prop.Name];
                            if (value == DBNull.Value)
                                value = null;
                            else
                                value = prop.PropertyType.FullName.Contains("Nullable") ? value : Convert.ChangeType(value, prop.PropertyType);
                            prop.SetValue(model, value, null);
                        }

                        modelList.Add(model);
                    }
                }


                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::GetROSSystemPatientInfoFromNotes", PROC_GET_ROSDATA_FOR_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion


        #region ROS SystemPatient Characteristics AND Detail (Select, Insert, Update, Ddelete)
        /// <summary>
        ///  Author: Khaleel Ur Rehman.
        // Purpose : function to load ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="rOSSystemPatientCharacteristicsId"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem loadROSSystemPatientCharacteristics(long notesId, long rOSSystemPatientCharacteristicsId, long ROSSystemPatientID)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (notesId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, notesId);
                if (rOSSystemPatientCharacteristicsId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, rOSSystemPatientCharacteristicsId);
                if (ROSSystemPatientID <= 0)
                    dbManager.AddParameters(2, PARM_ROSSYSTEM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ROSSYSTEM_PATIENT_ID, ROSSystemPatientID);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_SELECT, ds, ds.ROSSystemPatientCharacteristics.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::loadROSSystemPatientCharacteristics", PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to load ROSSystem Patient Characteristics Details.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="rOSCharacteristicsDetailsId"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem loadROSCharacteristicsDetails(long notesId, long rOSSystemPatientCharacteristicsID, long rOSCharacteristicsDetailsId, long ROSDataTemplateId)
        {
            DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (notesId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, notesId);

                if (rOSSystemPatientCharacteristicsID <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, rOSSystemPatientCharacteristicsID);

                if (rOSCharacteristicsDetailsId <= 0)
                    dbManager.AddParameters(2, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID, null);
                else
                    dbManager.AddParameters(2, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID, rOSCharacteristicsDetailsId);

                if (ROSDataTemplateId <= 0)
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ROS_DATA_TEMPLATE_ID, ROSDataTemplateId);
                ds = (DSClinicalReviewofSystem)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_SELECT, ds, ds.ROSCharacteristicsDetails.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::loadROSCharacteristicsDetails", PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman.
        /// Purpose : disAssociate Systems AgainstPatient
        /// Date : 15-Feb-2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="rosSystemInfoId"></param>
        /// <returns></returns>
        public string disAssociateSystemsAgainstNoteId(long notesId, long rosSystemInfoId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (notesId <= 0)
                    dbManager.AddParameters(0, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_NOTE_ID, notesId);

                if (rosSystemInfoId <= 0)
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_RESET_PATIENTINFO).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::disAssociateSystemsAgainstNoteId", PROC_ROSSYSTEM_RESET_PATIENTINFO, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to insert ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem insertROSSystemPatientCharacteristics(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSSystemPatientCharacteristics(dbManager, ds, true);
                ds = (DSClinicalReviewofSystem)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_INSERT, ds, ds.ROSSystemPatientCharacteristics.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insertROSSystemPatientCharacteristics", PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_INSERT, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to insert ROSSystem Patient Characteristics Details.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem insertROSCharacteristicsDetails(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSCharacteristicsDetails(dbManager, ds, true);
                ds = (DSClinicalReviewofSystem)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_INSERT, ds, ds.ROSCharacteristicsDetails.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insertROSCharacteristicsDetails", PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_INSERT, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to update ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem updateROSSystemPatientCharacteristics(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSSystemPatientCharacteristics(dbManager, ds, false);
                ds = (DSClinicalReviewofSystem)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_UPDATE, ds, ds.ROSSystemPatientCharacteristics.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSSystemPatientCharacteristics", PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_UPDATE, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to update ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalReviewofSystem updateROSCharacteristicsDetails(DSClinicalReviewofSystem ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersForROSCharacteristicsDetails(dbManager, ds, false);
                ds = (DSClinicalReviewofSystem)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_UPDATE, ds, ds.ROSCharacteristicsDetails.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSCharacteristicsDetails", PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_UPDATE, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to delete ROSSystem Patient Characteristics.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="rOSSystemPatientCharacteristicsId"></param>
        /// <returns></returns>
        public string deleteROSSystemPatientCharacteristics(long rOSSystemPatientCharacteristicsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSID, rOSSystemPatientCharacteristicsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteROSSystemPatientCharacteristics", PROC_ROSSYSTEM_PATIENT_CHARACTERISTICS_DELETE, ex);
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
        /// Author: Khaleel Ur Rehman.
        // Purpose : function to delete ROSSystem Patient Characteristics details.
        // Date : 29 January 2016.
        /// </summary>
        /// <param name="rOSCharacteristicsDetailsId"></param>
        /// <returns></returns>
        public string deleteROSCharacteristicsDetails(long rOSCharacteristicsDetailsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSSYSTEMPATIENT_CHARACTERISTICSDETAILSID, rOSCharacteristicsDetailsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::deleteROSCharacteristicsDetails", PROC_ROSSYSTEM_CHARACTERISTICS_DETAILS_DELETE, ex);
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


        /*
            Change Implement BY: Muhammad Azhar Shahzad
            Reason: Functions For Soap Text and attachement detachment of ROS with progress note
            Created Date: Feb 11, 2016
        */
        #region Notes and ROS

        /*
          Change Implement BY: Muhammad Azhar Shahzad
          Reason: Function to get updated Soap text of  social history for progress note
          Created Date: Dec 07, 2016
      */
        public string updateSoapTextForROS(long rosSystemInfoID)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ROSSYSTEM_INFO_ID, rosSystemInfoID);

                //  returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_BIRTHHX).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                //  MDVLogger.LogErrorMessage("DALReviewofSystem::updateSoapTextForBirthHX", PROC_UPDATE_SOAPTEXT_FOR_BIRTHHX, ex);
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

        // end azhar changed jan 07,2016
        #endregion
        /// <summary>
        /// Author : Khaleel Ur Rehman.
        /// Purpose : ROS System Patient Reset.
        /// Date : 19-Feb-2016.
        /// </summary>
        /// <param name="rOSSystemPatientID"></param>
        /// <param name="rOSSystemId"></param>
        /// <param name="rOSSystemInfoID"></param>
        /// <param name="removeSystemDetails"></param>
        /// <returns></returns>
        public string rOSSystemPatientReset(long rOSSystemPatientID, bool removeSystemDetails)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = rOSSystemPatientID.ToString();
                //DSClinicalReviewofSystem ds = new DSClinicalReviewofSystem();
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (rOSSystemPatientID <= 0)
                {
                    dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ROSSYSTEM_PATIENT_ID, rOSSystemPatientID);
                }
                //

                dbManager.AddParameters(1, PARM_REMOVE_SYSTEMDETAILS, removeSystemDetails);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROS_SYSTEM_PATIENT_RESET).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::ROSSystemPatientReset", PROC_ROS_SYSTEM_PATIENT_RESET, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


    }
}
