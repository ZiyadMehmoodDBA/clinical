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
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALVitals
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_VITALS_INSERT = "Clinical.sp_VitalSignsInsert";
        private const string PROC_ALL_VITALS_SELECT = "Clinical.sp_VitalSignsAllSelect";
        private const string PROC_VITALS_UPDATE = "Clinical.sp_VitalSignsUpdate";
        private const string PROC_VITALS_DELETE = "Clinical.sp_VitalSignsDelete";
        private const string PROC_COPY_VITALS_INSERT = "Clinical.sp_CopyVitalSignsInsert";
        private const string PROC_VITALS_SELECT = "Clinical.sp_VitalSignsSelect";
        private const string PROC_DETACH_VITAL_SIGN_FROM_NOTES = "Clinical.sp_DetachVitalSignFromNotes";
        private const string PROC_ATTACH_VITAL_SIGN_FROM_NOTES = "Clinical.sp_AttachVitalSignWithNotes_New";//"Clinical.sp_AttachVitalSignWithNotes";
        private const string PROC_GET_LATEST_VITAL_SIGN_BY_PATIENTID = "Clinical.sp_GetLatestVitalSignByPatientId";

        private const string PROC_GET_LATEST_VITAL_SIGN_FORSOAPNOTE = "Clinical.sp_GetLatestVitalSignForSoapNote";

        private const string PROC_VITAL_SIGNS_LOOKUP = "Clinical.sp_VitalSignsLookup";

        private const string PROC_VITALS_PULSE_INSERT = "Clinical.sp_VitalSignsPulseInsert";
        private const string PROC_VITALS_PULSE_UPDATE = "Clinical.sp_VitalSignsPulseUpdate";
        private const string PROC_VITALS_PULSE_DELETE = "Clinical.sp_VitalSignsPulseDelete";
        private const string PROC_VITALS_PULSE_SELECT = "Clinical.sp_VitalSignsPulseSelect";

        private const string PROC_VITALS_PULSE_SELECT_FORNOTE_SOAPTEXT = "Clinical.sp_VitalSignsSelectForNoteSoapText";

        private const string PROC_VITALS_TEMPERATURE_INSERT = "Clinical.sp_VitalSignsTempratureInsert";
        private const string PROC_VITALS_TEMPERATURE_UPDATE = "Clinical.sp_VitalSignsTempratureUpdate";
        private const string PROC_VITALS_TEMPERATURE_DELETE = "Clinical.sp_VitalSignsTempratureDelete";
        private const string PROC_VITALS_TEMPERATURE_SELECT = "Clinical.sp_VitalSignsTempratureSelect";

        private const string PROC_VITALS_RESPIRATION_INSERT = "Clinical.sp_VitalSignsRespirationInsert";
        private const string PROC_VITALS_RESPIRATION_UPDATE = "Clinical.sp_VitalSignsRespirationUpdate";
        private const string PROC_VITALS_RESPIRATION_DELETE = "Clinical.sp_VitalSignsRespirationDelete";
        private const string PROC_VITALS_RESPIRATION_SELECT = "Clinical.sp_VitalSignsRespirationSelect";

        private const string PROC_VITALS_BLOODPRESSURE_INSERT = "Clinical.sp_VitalSignsBloodPressureInsert";
        private const string PROC_VITALS_BLOODPRESSURE_UPDATE = "Clinical.sp_VitalSignsBloodPressureUpdate";
        private const string PROC_VITALS_BLOODPRESSURE_DELETE = "Clinical.sp_VitalSignsBloodPressureDelete";
        private const string PROC_VITALS_BLOODPRESSURE_SELECT = "Clinical.sp_VitalSignsBloodPressureSelect";

        private const string PROC_NOTES_VITALSIGNS_ELECT = "[Clinical].[sp_NotesVitalSignSelect]";

        #endregion

        #region "Parameters"

        private const string PARM_VITAL_ID = "@VitalSignId";
        private const string PARM_COPY_VITAL_ID = "@CopyVitalId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_HEIGHT = "@Height";
        private const string PARM_WEIGHT = "@Weight";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_COPY_PARENT_ID = "@CopyParentId";
        private const string PARM_SPO2 = "@SPO2";
        private const string PARM_INHALED_O2_CONCENTRATION = "@InhaledO2Concentration";
        private const string PARM_OXYGEN_SOURCE = "@OxygenSource";
        private const string PARM_PEAK_FLOW = "@PeakFlow";
        private const string PARM_PAIN_ID = "@PainId";
        private const string PARM_SMOKE_STATUS_ID = "@SmokeStatusId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_DROPDOWN = "@DropDown";
        private const string PARM_DELETE_COMMENTS = "@DeleteComments";
        private const string PARM_RISK_ASSESSMENT_ID = "@RiskAssessmentId";
        //PULSE PARAMETER 
        private const string PARM_PULSE_ID = "@PulseId";
        private const string PARM_RESULT = "@Result";
        private const string PARM_RYTHM_ID = "@RhythmId";
        private const string PARM_TIME = "@Time";
        //TEMPERATURE PARAMETERS
        private const string PARM_TEMPERATURE_ID = "@TempratureId";
        private const string PARM_METHOD_ID = "@MethodId";
        //RESPIRATION PARAMETERS
        private const string PARM_RESPIRATION_ID = "@RespirationId";
        private const string PARM_PATTERN_ID = "@PatternId";
        //BLOOD PRESSURE PARAMETERS
        private const string PARM_BP_ID = "@BPId";
        private const string PARM_SYSTOLIC = "@Systolic";
        private const string PARM_DIASTOLIC = "@Diastolic";
        private const string PARM_POSITION_ID = "@PositionId";
        private const string PARM_CUFF_LOCATION_ID = "@CuffLocationId";
        private const string PARM_CUFF_SIZE_ID = "@CuffSizeId";
        private const string PARM_NEGATION_REASON = "@NegationReasonId";

        //-----------
        private const string PARM_VITALSIGN_DATE = "@VitalSignDate";
        private const string PARM_VITALSIGN_TIME = "@VitalSignTime";
        private const string PARM_BMI = "@BMI";
        private const string PARM_BSA = "@BSA";
        private const string PARM_HEAD_CR = "@HeadCr";
        private const string PARM_BLOOD_TYPE = "@BloodType";

        private const string PARM_IS_PARENT = "@IsParent";
        private const string PARM_IS_FROMNOTE = "@IsFromNote";
       
        
        #endregion

        #region Constructors
        public DALVitals()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALVitals(SharedVariable SharedVariable)
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

        #region "Support Functions Vital Signs"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSVitals ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(28);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_VITAL_ID, ds.VitalSigns.VitalSignIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_VITAL_ID, ds.VitalSigns.VitalSignIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.VitalSigns.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_VISIT_ID, ds.VitalSigns.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_HEIGHT, ds.VitalSigns.HeightColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_WEIGHT, ds.VitalSigns.WeightColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.VitalSigns.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.VitalSigns.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.VitalSigns.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.VitalSigns.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.VitalSigns.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_NOTES_ID, ds.VitalSigns.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_COPY_PARENT_ID, ds.VitalSigns.CopyParentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_SPO2, ds.VitalSigns.SPO2Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_OXYGEN_SOURCE, ds.VitalSigns.OxygenSourceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PEAK_FLOW, ds.VitalSigns.PeakFlowColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PAIN_ID, ds.VitalSigns.PainIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(16, PARM_SMOKE_STATUS_ID, ds.VitalSigns.SmokeStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(17, PARM_COMMENTS, ds.VitalSigns.CommentsColumn.ColumnName, DbType.String);


            dbManager.AddParameters(18, PARM_VITALSIGN_DATE, ds.VitalSigns.VitalSignDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_VITALSIGN_TIME, ds.VitalSigns.VitalSignTimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_BMI, ds.VitalSigns.BMIColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(21, PARM_BSA, ds.VitalSigns.BSAColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(22, PARM_HEAD_CR, ds.VitalSigns.HeadCrColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(23, PARM_BLOOD_TYPE, ds.VitalSigns.BloodTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(24, PARM_DELETE_COMMENTS, ds.VitalSigns.DeleteCommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_RISK_ASSESSMENT_ID, ds.VitalSigns.RiskAssessmentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_IS_FROMNOTE, ds.VitalSigns.IsFromNoteColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARM_INHALED_O2_CONCENTRATION, ds.VitalSigns.InhaledO2ConcentrationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_NEGATION_REASON, ds.VitalSigns.NegationReasonIdColumn.ColumnName, DbType.Int64);            
        }

        #endregion

        #region "Support Functions Vital Signs Pulse"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateVitalSignsPulseParameters(IDBManager dbManager, DSVitals ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PULSE_ID, ds.VitalSignsPulse.PulseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PULSE_ID, ds.VitalSignsPulse.PulseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VITAL_ID, ds.VitalSignsPulse.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RESULT, ds.VitalSignsPulse.ResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_RYTHM_ID, ds.VitalSignsPulse.RhythmIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_TIME, ds.VitalSignsPulse.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.VitalSignsPulse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.VitalSignsPulse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.VitalSignsPulse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.VitalSignsPulse.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateVitalSignsPulseInsertParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateInsertParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_PULSE_ID, ds.VitalSignsPulse.PulseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsPulse.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsPulse.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_RYTHM_ID, ds.VitalSignsPulse.RhythmIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsPulse.TimeColumn.ColumnName, DbType.String);


            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsPulse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsPulse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsPulse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsPulse.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        private void CreateVitalSignsPulseUpdateParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateUpdateParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_PULSE_ID, ds.VitalSignsPulse.PulseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsPulse.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsPulse.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_RYTHM_ID, ds.VitalSignsPulse.RhythmIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsPulse.TimeColumn.ColumnName, DbType.String);


            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsPulse.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsPulse.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsPulse.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsPulse.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Support Functions Vital Signs Temperature"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateVitalSignsTemperatureParameters(IDBManager dbManager, DSVitals ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_TEMPERATURE_ID, ds.VitalSignsTemperature.TempratureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_TEMPERATURE_ID, ds.VitalSignsTemperature.TempratureIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VITAL_ID, ds.VitalSignsTemperature.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RESULT, ds.VitalSignsTemperature.ResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_METHOD_ID, ds.VitalSignsTemperature.MethodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_TIME, ds.VitalSignsTemperature.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.VitalSignsTemperature.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.VitalSignsTemperature.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.VitalSignsTemperature.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.VitalSignsTemperature.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateVitalSignsTemperatureInsertParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateInsertParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_TEMPERATURE_ID, ds.VitalSignsTemperature.TempratureIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsTemperature.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsTemperature.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_METHOD_ID, ds.VitalSignsTemperature.MethodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsTemperature.TimeColumn.ColumnName, DbType.String);


            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsTemperature.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsTemperature.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsTemperature.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsTemperature.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        private void CreateVitalSignsTemperatureUpdateParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateUpdateParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_TEMPERATURE_ID, ds.VitalSignsTemperature.TempratureIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsTemperature.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsTemperature.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_METHOD_ID, ds.VitalSignsTemperature.MethodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsTemperature.TimeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsTemperature.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsTemperature.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsTemperature.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsTemperature.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Support Functions Vital Signs Respiration"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateVitalSignsRespirationParameters(IDBManager dbManager, DSVitals ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_RESPIRATION_ID, ds.VitalSignsRespiration.RespirationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_RESPIRATION_ID, ds.VitalSignsRespiration.RespirationIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VITAL_ID, ds.VitalSignsRespiration.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RESULT, ds.VitalSignsRespiration.ResultColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PATTERN_ID, ds.VitalSignsRespiration.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_TIME, ds.VitalSignsRespiration.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.VitalSignsRespiration.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.VitalSignsRespiration.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.VitalSignsRespiration.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.VitalSignsRespiration.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateVitalSignsRespirationInsertParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateInsertParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_RESPIRATION_ID, ds.VitalSignsRespiration.RespirationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsRespiration.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsRespiration.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_PATTERN_ID, ds.VitalSignsRespiration.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsRespiration.TimeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsRespiration.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsRespiration.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsRespiration.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsRespiration.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        private void CreateVitalSignsRespirationUpdateParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateUpdateParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_RESPIRATION_ID, ds.VitalSignsRespiration.RespirationIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsRespiration.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_RESULT, ds.VitalSignsRespiration.ResultColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_PATTERN_ID, ds.VitalSignsRespiration.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsRespiration.TimeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.VitalSignsRespiration.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.VitalSignsRespiration.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.VitalSignsRespiration.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.VitalSignsRespiration.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        #endregion

        #region "Support Functions Vital Signs BloodPressure"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateVitalSignsBloodPressureParameters(IDBManager dbManager, DSVitals ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BP_ID, ds.VitalSignsBloodPressure.BPIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BP_ID, ds.VitalSignsBloodPressure.BPIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_VITAL_ID, ds.VitalSignsBloodPressure.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SYSTOLIC, ds.VitalSignsBloodPressure.SystolicColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(3, PARM_DIASTOLIC, ds.VitalSignsBloodPressure.DiastolicColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(4, PARM_TIME, ds.VitalSignsBloodPressure.TimeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_POSITION_ID, ds.VitalSignsBloodPressure.PositionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_CUFF_LOCATION_ID, ds.VitalSignsBloodPressure.CuffLocationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(7, PARM_CUFF_SIZE_ID, ds.VitalSignsBloodPressure.CuffSizeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.VitalSigns.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.VitalSigns.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.VitalSigns.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.VitalSigns.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        private void CreateVitalSignsBloodPressureInsertParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateInsertParameters(13);

            dbManager.AddInsertUpdateParameters(0, PARM_BP_ID, ds.VitalSignsBloodPressure.BPIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsBloodPressure.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSTOLIC, ds.VitalSignsBloodPressure.SystolicColumn.ColumnName, DbType.Int16);
            dbManager.AddInsertUpdateParameters(3, PARM_DIASTOLIC, ds.VitalSignsBloodPressure.DiastolicColumn.ColumnName, DbType.Int16);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsBloodPressure.TimeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_POSITION_ID, ds.VitalSignsBloodPressure.PositionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_CUFF_LOCATION_ID, ds.VitalSignsBloodPressure.CuffLocationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_CUFF_SIZE_ID, ds.VitalSignsBloodPressure.CuffSizeIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.VitalSignsBloodPressure.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.VitalSignsBloodPressure.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.VitalSignsBloodPressure.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.VitalSignsBloodPressure.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_NEGATION_REASON, ds.VitalSignsBloodPressure.BPNegationReasonIdColumn.ColumnName, DbType.Int64);

            
        }
        private void CreateVitalSignsBloodPressureUpdateParameters(IDBManager dbManager, DSVitals ds)
        {
            dbManager.CreateUpdateParameters(13);

            dbManager.AddInsertUpdateParameters(0, PARM_BP_ID, ds.VitalSignsBloodPressure.BPIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_VITAL_ID, ds.VitalSignsBloodPressure.VitalSignIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSTOLIC, ds.VitalSignsBloodPressure.SystolicColumn.ColumnName, DbType.Int16);
            dbManager.AddInsertUpdateParameters(3, PARM_DIASTOLIC, ds.VitalSignsBloodPressure.DiastolicColumn.ColumnName, DbType.Int16);
            dbManager.AddInsertUpdateParameters(4, PARM_TIME, ds.VitalSignsBloodPressure.TimeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_POSITION_ID, ds.VitalSignsBloodPressure.PositionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(6, PARM_CUFF_LOCATION_ID, ds.VitalSignsBloodPressure.CuffLocationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(7, PARM_CUFF_SIZE_ID, ds.VitalSignsBloodPressure.CuffSizeIdColumn.ColumnName, DbType.Int32);

            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.VitalSignsBloodPressure.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.VitalSignsBloodPressure.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.VitalSignsBloodPressure.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.VitalSignsBloodPressure.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_NEGATION_REASON, ds.VitalSignsBloodPressure.BPNegationReasonIdColumn.ColumnName, DbType.Int64);

        }
        #endregion

        #region "Vital Signs"
        public DSVitals InsertVitals(ref DSVitals ds, IDBManager dbManager)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.VitalSigns.GetChanges();
                //dbManager.Open();
                // dbManager.BeginTransaction();

                CreateParameters(dbManager, ds, true);
                ds = (DSVitals)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VITALS_INSERT, ds, ds.VitalSigns.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), null, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), patientId: ds.VitalSigns.Rows[0][ds.VitalSigns.PatientIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}

                // dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //   dsDBAudit.RejectChanges();
                //  dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::InsertVitals", PROC_VITALS_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                // dbManager.Dispose();
            }
        }
        public DSVitals UpdateVitals(DSVitals ds, IDBManager dbManager = null)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();

            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
               
            }
            try
            {

                // DataTable dtTemp = ds.VitalSigns.GetChanges();
                //dbManager.Open();
                //  dbManager.BeginTransaction();

                this.CreateParameters(dbManager, ds, false);
                ds = (DSVitals)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_UPDATE, ds, ds.VitalSigns.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), null, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), patientId: ds.VitalSigns.Rows[0][ds.VitalSigns.PatientIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}

                //dbManager.CommitTransaction();
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //  dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::UpdateVitals", PROC_VITALS_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                // dbManager.Dispose();
            }
        }
        public string DeleteVitals(string VitalSignId, string patientId = "")
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VITALS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::DeleteVitals", PROC_VITALS_DELETE, ex);
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
        public DSVitals LoadVitals(long PatientId, long VitalSignId, int? PageNumber = 1, int? RowsPerPage = 1000, string isViewVitals = "", string isPrintVitals = "", long NotesId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (VitalSignId == 0)
                    dbManager.AddParameters(0, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);
                if (PageNumber == 0)
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(1, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(2, PARM_ROWS_PPAGE, null);
                else
                    dbManager.AddParameters(2, PARM_ROWS_PPAGE, RowsPerPage);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.VitalSigns.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (PatientId == 0)
                    dbManager.AddParameters(4, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PATIENT_ID, PatientId);

                if (NotesId <= 0)
                    dbManager.AddParameters(5, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(5, PARM_NOTES_ID, NotesId);

                //if (IsParent == "")
                //    IsParent = "1";
                //dbManager.AddParameters(5, PARM_IS_PARENT, IsParent);

                if (VitalSignId == 0)
                {
                    // For Child Records IsParent=0
                    dbManager.AddParameters(6, PARM_IS_PARENT, "0");
                    DSVitals dsChildVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT, ds, ds.VitalSignsChild.TableName);
                    if (dsChildVitals != null)
                    {
                        ds.Merge(dsChildVitals);
                    }

                    // For Parent Records IsParent=1
                    dbManager.AddParameters(6, PARM_IS_PARENT, "1");
                    DSVitals dsParentVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT, ds, ds.VitalSignSoap.TableName);
                    if (dsParentVitals != null)
                    {
                        ds.Merge(dsParentVitals);
                    }

                }
                else
                {
                    dbManager.AddParameters(6, PARM_IS_PARENT, "1");
                    ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_SELECT, ds, ds.VitalSigns.TableName);

                    DataTable dtTemp = ds.VitalSigns;
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        if (isViewVitals == "1" || isPrintVitals == "1")
                        {
                            bool isViewAction = isViewVitals == "1" ? true : false;
                            bool isPrintAcion = isPrintVitals == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), null, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), isViewAction, isPrintAcion, patientId: PatientId.ToString());
                            dsDBAudit.AcceptChanges();
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

                MDVLogger.DALErrorLog("DALVitals::LoadVitals", PROC_VITALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVitals LoadAllVitals(long PatientId, long VitalSignId, string isViewVitals = "", string isPrintVitals = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (VitalSignId == 0)
                    dbManager.AddParameters(0, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);


                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                //if (IsParent == "")
                //    IsParent = "1";
                //dbManager.AddParameters(5, PARM_IS_PARENT, IsParent);

                if (VitalSignId == 0)
                {
                    // For Child Records IsParent=0
                    dbManager.AddParameters(2, PARM_IS_PARENT, "0");
                    DSVitals dsChildVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALL_VITALS_SELECT, ds, ds.VitalSignsChild.TableName);
                    if (dsChildVitals != null)
                    {
                        ds.Merge(dsChildVitals);
                    }

                    // For Parent Records IsParent=1
                    dbManager.AddParameters(2, PARM_IS_PARENT, "1");
                    DSVitals dsParentVitals = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALL_VITALS_SELECT, ds, ds.VitalSignSoap.TableName);
                    if (dsParentVitals != null)
                    {
                        ds.Merge(dsParentVitals);
                    }

                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_PARENT, "1");
                    ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALL_VITALS_SELECT, ds, ds.VitalSigns.TableName);

                    DataTable dtTemp = ds.VitalSigns;
                    if (dtTemp != null)
                    {
                        if (isViewVitals == "1" || isPrintVitals == "1")
                        {
                            bool isViewAction = isViewVitals == "1" ? true : false;
                            bool isPrintAcion = isPrintVitals == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), null, ds.VitalSigns.Rows[0][ds.VitalSigns.VitalSignIdColumn].ToString(), isViewAction, isPrintAcion, patientId: PatientId.ToString());
                            dsDBAudit.AcceptChanges();
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

                MDVLogger.DALErrorLog("DALVitals::LoadAllVitals", PROC_ALL_VITALS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSVitals getLatestVitalByPatientId(long PatientId, long userId, long entityId)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (userId == 0)
                    dbManager.AddParameters(1, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_USER_ID, userId);
                if (entityId == 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_VITAL_SIGN_BY_PATIENTID, ds, ds.VitalSignSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::GetLatestVitalByPatientId", PROC_GET_LATEST_VITAL_SIGN_BY_PATIENTID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public VitalsModel getNoteVitalByPatientId(long PatientId, long userId, long entityId)
        {
            VitalsModel model = new VitalsModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, userId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, entityId));

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_VITAL_SIGN_FORSOAPNOTE, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(VitalsModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALVitals::GetLatestVitalByPatientId", PROC_GET_LATEST_VITAL_SIGN_FORSOAPNOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string copyVitalSignsInsert(long VitalSignId, long NotesId, String CreatedBy)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_VITAL_ID, "", DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_COPY_VITAL_ID, VitalSignId);
                dbManager.AddParameters(2, PARM_CREATED_BY, CreatedBy);
                if (NotesId == 0)
                {
                    dbManager.AddParameters(3, PARM_NOTES_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_NOTES_ID, NotesId);
                }

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_COPY_VITALS_INSERT).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::CopyVitalSignsInsert", PROC_COPY_VITALS_INSERT, ex);
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

        #region "Vital Sign For Notes Soap
        public DSVitals loadVitalSignsForSoap(string VitalSignId)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);



                if (string.IsNullOrEmpty(VitalSignId))
                    dbManager.AddParameters(0, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_PULSE_SELECT_FORNOTE_SOAPTEXT, ds, ds.VitalSignSoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::LoadVitalSignsForSoap", PROC_VITALS_PULSE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region"Vital Signs Pulse"
        public DSVitals InsertVitalSignsPulse(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsPulse.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();

                CreateVitalSignsPulseParameters(dbManager, ds, true);
                ds = (DSVitals)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VITALS_PULSE_INSERT, ds, ds.VitalSignsPulse.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.PulseIdColumn].ToString(), null, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::InsertVitalSignsPulse", PROC_VITALS_PULSE_INSERT, ex);
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
        public DSVitals UpdateVitalSignsPulse(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsPulse.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateVitalSignsPulseParameters(dbManager, ds, false);
                ds = (DSVitals)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_PULSE_UPDATE, ds, ds.VitalSignsPulse.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.PulseIdColumn].ToString(), null, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::UpdateVitalSignsPulse", PROC_VITALS_PULSE_UPDATE, ex);
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

        public DSVitals InsertUpdateVitalSignsPulse(ref DSVitals ds, string PatientId = "", IDBManager dbManager = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // DataTable dtTemp = ds.VitalSignsPulse.GetChanges();
                // dbManager.Open();
                // dbManager.BeginTransaction();

                CreateVitalSignsPulseInsertParameters(dbManager, ds);
                CreateVitalSignsPulseUpdateParameters(dbManager, ds);
                ds = (DSVitals)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_PULSE_INSERT, PROC_VITALS_PULSE_UPDATE, ds, ds.VitalSignsPulse.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.PulseIdColumn].ToString(), null, ds.VitalSignsPulse.Rows[0][ds.VitalSignsPulse.VitalSignIdColumn].ToString(), patientId: PatientId);
                //    dsDBAudit.AcceptChanges();
                //}

                //   dbManager.CommitTransaction();
                //  ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {

                ds.RejectChanges();
                // dsDBAudit.RejectChanges();
                // dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateVitalSignsPulse", PROC_VITALS_PULSE_INSERT + " " + PROC_VITALS_PULSE_UPDATE, ex);
                throw ex;
            }
        }

        public DSVitals LoadVitalSignsPulse(Int32 PulseId, long VitalSignId, string isViewVitalSignPulse = "")
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                //dbManager.BeginTransaction();

                dbManager.CreateParameters(2);

                if (PulseId == 0)
                    dbManager.AddParameters(0, PARM_PULSE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PULSE_ID, PulseId);
                if (VitalSignId == 0)
                    dbManager.AddParameters(1, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VITAL_ID, VitalSignId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_PULSE_SELECT, ds, ds.VitalSignsPulse.TableName);
                return ds;
            }
            catch (Exception ex)
            {


                MDVLogger.DALErrorLog("DALVitals::LoadVitalSignsPulse", PROC_VITALS_PULSE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteVitalSignsPulse(string PulseId, string patientId = "")
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSVitals dsVitals = LoadVitalSignsPulse(Convert.ToInt32(PulseId), 0);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PULSE_ID, PulseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VITALS_PULSE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsVitals.VitalSignsPulse;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, PulseId, null, dsVitals.VitalSignsPulse.Rows[0][dsVitals.VitalSignsPulse.VitalSignIdColumn].ToString(), false, false, true, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::DeleteVitalSignsPulse", PROC_VITALS_PULSE_DELETE, ex);
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

        #region"Vital Signs Temperature"
        public DSVitals InsertVitalSignsTemperature(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsTemperature.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateVitalSignsTemperatureParameters(dbManager, ds, true);
                ds = (DSVitals)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VITALS_TEMPERATURE_INSERT, ds, ds.VitalSignsTemperature.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsTemperature.Rows[0][ds.VitalSignsTemperature.TempratureIdColumn].ToString(), null, ds.VitalSignsTemperature.Rows[0][ds.VitalSignsTemperature.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::InsertVitalSignsTemperature", PROC_VITALS_TEMPERATURE_INSERT, ex);
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
        public DSVitals UpdateVitalSignsTemperature(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsTemperature.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateVitalSignsTemperatureParameters(dbManager, ds, false);
                ds = (DSVitals)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_TEMPERATURE_UPDATE, ds, ds.VitalSignsTemperature.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsTemperature.Rows[0][ds.VitalSignsTemperature.TempratureIdColumn].ToString(), null, ds.VitalSignsTemperature.Rows[0][ds.VitalSignsTemperature.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::UpdateVitalSignsTemperature", PROC_VITALS_TEMPERATURE_UPDATE, ex);
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
        public DSVitals InsertUpdateVitalSignsTemperature(ref DSVitals ds, string PatientId = "", IDBManager dbManager = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // DataTable dtTemp = ds.VitalSignsTemprature.GetChanges();
                //dbManager.Open();
                //  dbManager.BeginTransaction();

                CreateVitalSignsTemperatureInsertParameters(dbManager, ds);
                CreateVitalSignsTemperatureUpdateParameters(dbManager, ds);
                ds = (DSVitals)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_TEMPERATURE_INSERT, PROC_VITALS_TEMPERATURE_UPDATE, ds, ds.VitalSignsTemperature.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsTemprature.Rows[0][ds.VitalSignsTemprature.TempratureIdColumn].ToString(), null, ds.VitalSignsTemprature.Rows[0][ds.VitalSignsTemprature.VitalSignIdColumn].ToString(), patientId: PatientId);
                //    dsDBAudit.AcceptChanges();
                //}

                // dbManager.CommitTransaction();
                //   ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                // dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateVitalSignsTemperature", PROC_VITALS_TEMPERATURE_INSERT + " " + PROC_VITALS_TEMPERATURE_UPDATE, ex);
                throw ex;
            }
        }
        public DSVitals LoadVitalSignsTemperature(Int32 TempratureId, long VitalSignId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSVitals ds = new DSVitals();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (TempratureId == 0)
                    dbManager.AddParameters(0, PARM_TEMPERATURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPERATURE_ID, TempratureId);
                if (VitalSignId == 0)
                    dbManager.AddParameters(1, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VITAL_ID, VitalSignId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_TEMPERATURE_SELECT, ds, ds.VitalSignsTemperature.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::LoadVitalSignsTemperature", PROC_VITALS_TEMPERATURE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteVitalSignsTemperature(string TempratureId, string patientId = "")
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSVitals dsVitals = LoadVitalSignsTemperature(Convert.ToInt32(TempratureId), 0);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPERATURE_ID, TempratureId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VITALS_TEMPERATURE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsVitals.VitalSignsTemperature;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, TempratureId, null, dsVitals.VitalSignsTemperature.Rows[0][dsVitals.VitalSignsTemperature.VitalSignIdColumn].ToString(), false, false, true, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::DeleteVitalSignsTemperature", PROC_VITALS_TEMPERATURE_DELETE, ex);
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

        #region"Vital Signs Respiration"
        public DSVitals InsertVitalSignsRespiration(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsRespiration.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateVitalSignsRespirationParameters(dbManager, ds, true);
                ds = (DSVitals)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VITALS_RESPIRATION_INSERT, ds, ds.VitalSignsRespiration.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.RespirationIdColumn].ToString(), null, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::InsertVitalSignsRespiration", PROC_VITALS_RESPIRATION_INSERT, ex);
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
        public DSVitals UpdateVitalSignsRespiration(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsRespiration.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateVitalSignsRespirationParameters(dbManager, ds, false);
                ds = (DSVitals)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_RESPIRATION_UPDATE, ds, ds.VitalSignsRespiration.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.RespirationIdColumn].ToString(), null, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::UpdateVitalSignsRespiration", PROC_VITALS_RESPIRATION_UPDATE, ex);
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
        public DSVitals InsertUpdateVitalSignsRespiration(ref DSVitals ds, string PatientId = "", IDBManager dbManager = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // DataTable dtTemp = ds.VitalSignsRespiration.GetChanges();
                //dbManager.Open();
                //  dbManager.BeginTransaction();

                CreateVitalSignsRespirationInsertParameters(dbManager, ds);
                CreateVitalSignsRespirationUpdateParameters(dbManager, ds);
                ds = (DSVitals)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_RESPIRATION_INSERT, PROC_VITALS_RESPIRATION_UPDATE, ds, ds.VitalSignsRespiration.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.RespirationIdColumn].ToString(), null, ds.VitalSignsRespiration.Rows[0][ds.VitalSignsRespiration.VitalSignIdColumn].ToString(), patientId: PatientId);
                //    dsDBAudit.AcceptChanges();
                //}
                // dbManager.CommitTransaction();
                //   ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //  dsDBAudit.RejectChanges();
                //  dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateVitalSignsRespiration", PROC_VITALS_RESPIRATION_INSERT + " " + PROC_VITALS_RESPIRATION_UPDATE, ex);
                throw ex;
            }
        }
        public DSVitals LoadVitalSignsRespiration(Int32 RespirationId, long VitalSignId)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (RespirationId == 0)
                    dbManager.AddParameters(0, PARM_RESPIRATION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RESPIRATION_ID, RespirationId);
                if (VitalSignId == 0)
                    dbManager.AddParameters(1, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VITAL_ID, VitalSignId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_RESPIRATION_SELECT, ds, ds.VitalSignsRespiration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::LoadVitalSignsRespiration", PROC_VITALS_RESPIRATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteVitalSignsRespiration(string RespirationId, string patientId = "")
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSVitals dsVitals = LoadVitalSignsRespiration(Convert.ToInt32(RespirationId), 0);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_RESPIRATION_ID, RespirationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VITALS_RESPIRATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsVitals.VitalSignsRespiration;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, RespirationId, null, dsVitals.VitalSignsRespiration.Rows[0][dsVitals.VitalSignsRespiration.VitalSignIdColumn].ToString(), false, false, true, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::DeleteVitalSignsRespiration", PROC_VITALS_RESPIRATION_DELETE, ex);
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

        #region"Vital Signs BloodPressure"
        public DSVitals InsertVitalSignsBloodPressure(DSVitals ds, string PatientId = "", IDBManager dbManager = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsBloodPressure.GetChanges();
                //dbManager.Open();
                //  dbManager.BeginTransaction();

                CreateVitalSignsBloodPressureParameters(dbManager, ds, true);
                ds = (DSVitals)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VITALS_BLOODPRESSURE_INSERT, ds, ds.VitalSignsBloodPressure.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.BPIdColumn].ToString(), null, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::InsertVitalSignsBloodPressure", PROC_VITALS_BLOODPRESSURE_INSERT, ex);
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
        public DSVitals UpdateVitalSignsBloodPressure(DSVitals ds, string PatientId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.VitalSignsBloodPressure.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateVitalSignsBloodPressureParameters(dbManager, ds, false);
                ds = (DSVitals)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_BLOODPRESSURE_UPDATE, ds, ds.VitalSignsBloodPressure.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.BPIdColumn].ToString(), null, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.VitalSignIdColumn].ToString(), patientId: PatientId);
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

                MDVLogger.DALErrorLog("DALVitals::UpdateVitalSignsBloodPressure", PROC_VITALS_BLOODPRESSURE_UPDATE, ex);
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
        public DSVitals InsertUpdateVitalSignsBloodPressure(ref DSVitals ds, string PatientId = "", IDBManager dbManager = null)
        {
            //   DSDBAudit dsDBAudit = new DSDBAudit();
            if (dbManager == null)
                dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //    DataTable dtTemp = ds.VitalSignsBloodPressure.GetChanges();
                // dbManager.Open();
                // dbManager.BeginTransaction();

                CreateVitalSignsBloodPressureInsertParameters(dbManager, ds);
                CreateVitalSignsBloodPressureUpdateParameters(dbManager, ds);
                ds = (DSVitals)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_VITALS_BLOODPRESSURE_INSERT, PROC_VITALS_BLOODPRESSURE_UPDATE, ds, ds.VitalSignsBloodPressure.TableName);

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.BPIdColumn].ToString(), null, ds.VitalSignsBloodPressure.Rows[0][ds.VitalSignsBloodPressure.VitalSignIdColumn].ToString(), patientId: PatientId);
                //    dsDBAudit.AcceptChanges();
                //}
                //   dbManager.CommitTransaction();
                //  ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //   dsDBAudit.RejectChanges();
                // dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALVitals::InsertUpdateVitalSignsBloodPressure", PROC_VITALS_BLOODPRESSURE_INSERT + " " + PROC_VITALS_BLOODPRESSURE_UPDATE, ex);
                throw ex;
            }
        }
        public DSVitals LoadVitalSignsBloodPressure(Int32 BPId, long VitalSignId)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (BPId == 0)
                    dbManager.AddParameters(0, PARM_BP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BP_ID, BPId);
                if (VitalSignId == 0)
                    dbManager.AddParameters(1, PARM_VITAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VITAL_ID, VitalSignId);

                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITALS_BLOODPRESSURE_SELECT, ds, ds.VitalSignsBloodPressure.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::LoadVitalSignsBloodPressure", PROC_VITALS_BLOODPRESSURE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteVitalSignsBloodPressure(string BPId, string patientId = "")
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();

                DSVitals dsVitals = LoadVitalSignsBloodPressure(Convert.ToInt32(BPId), 0);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BP_ID, BPId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VITALS_BLOODPRESSURE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsVitals.VitalSignsBloodPressure;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, BPId, null, dsVitals.VitalSignsBloodPressure.Rows[0][dsVitals.VitalSignsBloodPressure.VitalSignIdColumn].ToString(), false, false, true, patientId);
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

                MDVLogger.DALErrorLog("DALVitals::DeleteVitalSignsBloodPressure", PROC_VITALS_BLOODPRESSURE_DELETE, ex);
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

        #region Notes and Vitals
        /// <summary>
        /// Detaching Vital Signs With Progress notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientId"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachVitalSignFromNotes(string VitalSignId, long PatientId, long VisitId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                foreach (var VitalId in VitalSignId.Split(','))
                {
                    dbManager.CreateParameters(4);

                    if (string.IsNullOrEmpty(VitalId))
                    {
                        dbManager.AddParameters(0, PARM_VITAL_ID, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(0, PARM_VITAL_ID, VitalId);
                    }
                    if (PatientId == 0)
                    {
                        dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                    }
                    if (VisitId == 0)
                    {
                        dbManager.AddParameters(2, PARM_VISIT_ID, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(2, PARM_VISIT_ID, VisitId);
                    }
                    if (NotesId == 0)
                    {
                        dbManager.AddParameters(3, PARM_NOTES_ID, DBNull.Value);
                    }
                    else
                    {
                        dbManager.AddParameters(3, PARM_NOTES_ID, NotesId);
                    }

                    //dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_VITAL_SIGN_FROM_NOTES);


                }
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::DetachVitalSignFromNotes", PROC_DETACH_VITAL_SIGN_FROM_NOTES, ex);
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
        /// Attaching Vital Signs With Progress notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientId"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DSVitals attachVitalSignFromNotes(string VitalSignId, long PatientId, long VisitId, long NotesId, String UserName)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSVitals ds = new DSVitals();

                dbManager.Open();
                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(VitalSignId))
                {
                    dbManager.AddParameters(0, PARM_VITAL_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_VITAL_ID, VitalSignId);
                }
                //if (PatientId == 0)
                //{
                //    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                //}
                //else
                //{
                //    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                //}
                //if (VisitId == 0)
                //{
                //    dbManager.AddParameters(2, PARM_VISIT_ID, DBNull.Value);
                //}
                //else
                //{
                //    dbManager.AddParameters(2, PARM_VISIT_ID, VisitId);
                //}
                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }
                //dbManager.AddParameters(4, PARM_CREATED_BY, UserName);
                //dbManager.AddParameters(5, PARM_MODIFIED_BY, UserName);
                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_VITAL_SIGN_FROM_NOTES, ds, ds.VitalSigns.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::AttachVitalSignFromNotes", PROC_ATTACH_VITAL_SIGN_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Lookups"
        public DSVitals LookupAllVitalSigns(string DropDownValue)
        {
            DSVitals ds = new DSVitals();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (DropDownValue == "")
                    DropDownValue = null;

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_DROPDOWN, DropDownValue);
                ds = (DSVitals)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VITAL_SIGNS_LOOKUP, ds, ds.VitalSignsLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::LookupAllVitalSigns", PROC_VITAL_SIGNS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Legacy Notes

        public List<VitalSigns> NotesVitalSignsSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<VitalSigns> objList_VitalSigns = new List<VitalSigns>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTES_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_VITALSIGNS_ELECT, parameters))
                {
                    while (reader.Read())
                    {
                        VitalSigns model = new VitalSigns();
                        var properties = typeof(VitalSigns).GetProperties();
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
                        objList_VitalSigns.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVitals::NotesVitalSignsSelect", PROC_NOTES_VITALSIGNS_ELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_VitalSigns;
        }

        #endregion Legacy Notes

    }
}
