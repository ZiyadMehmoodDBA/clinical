/* Author:  Muhammad Arshad
 * Created Date: 07/01/2016
 * OverView: Created for MedicalHx in Clinical Module
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
using MDVision.Model;
using MDVision.Model.Clinical.History;
using System.Data.SqlClient;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Common.Utilities;
using System.Reflection;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALMedicalHx
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_MEDICALHX__TESTRESULT_LOOKUP = "Clinical.sp_MedicalHx_TestResultLookup";
        private const string PROC_MEDICALHX__DURATIONPERIOD_LOOKUP = "Clinical.sp_MedicalHx_DurationPeriodLookup";
        private const string PROC_MEDICALHX__PATTERN_LOOKUP = "Clinical.sp_MedicalHx_PatternLookup";
        private const string PROC_MEDICALHX_SEVERITY_LOOKUP = "Clinical.sp_MedicalHx_SeverityLookup";
        private const string PROC_MEDICALHX_AGGRAVATED_LOOKUP = "Clinical.sp_MedicalHx_AggravatedByLookup";
        private const string PROC_MEDICALHX_STATUS_LOOKUP = "Clinical.sp_MedicalHx_StatusLookup";
        private const string PROC_HXLOG_SELECT = "Clinical.sp_HXLogSelect";

        #endregion

        #region "Parameters"
        #endregion

        #region Constructors
        public DALMedicalHx()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALMedicalHx(bool IsNative)
        {

        }

        public DALMedicalHx(SharedVariable SharedVariable)
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

        #region "Stored Procedure Names"

        /* 08/01/2016 Muhammad Arshad MedicalHx Parent Table */
        private const string PROC_MEDICALHX_INSERT = "Clinical.sp_MedicalHxInsert";
        private const string PROC_MEDICALHX_UPDATE = "Clinical.sp_MedicalHxUpdate";
        private const string PROC_MEDICALHX_DELETE = "Clinical.sp_MedicalHxDelete";
        private const string PROC_MEDICALHX_SELECT = "Clinical.sp_MedicalHxSelect";
        private const string PROC_MEDICALHX_SELECT_NATIVE = "Clinical.sp_MedicalHx_Disease_SelectNative";
        private const string PROC_MEDICALHX_SELECT_ForSoapText = "Clinical.sp_MedicalHxSelectForSoapText";
        /* End 08/01/2016 Muhammad Arshad MedicalHx Parent Table */

        /* 11/01/2016 Muhammad Arshad MedicalHx_Disease Parent Table */
        private const string PROC_MEDICALHX_DISEASE_SELECT = "Clinical.sp_MedicalHx_DiseaseSelect";
        /* Start 12/01/2016 Muhammad Irfan Disease table procedures */
        private const string PROC_MEDICALHX_DISEASE_DELETE = "Clinical.sp_MedicalHx_DiseaseDelete";
        private const string PROC_MEDICALHX_DISEASE_INSERT = "Clinical.sp_MedicalHx_DiseaseInsert";
        private const string PROC_MEDICALHX_DISEASE_UPDATE = "Clinical.sp_MedicalHx_DiseaseUpdate";
        /* End 12/01/2016 Muhammad Irfan Disease table procedures */
        /* End 11/01/2016 Muhammad Arshad MedicalHx_Disease Parent Table */

        /*
            Change Implement BY: Muhammad Azhar Shahzad
            Reason: Stored Procedure For Soap Text and attachement detachment of social history with progress note
            Created Date: Dec 15, 2015
        */
        private const string PROC_DETACH_SOCIALHX_FROM_NOTES = "Clinical.sp_DetachSocialHxFromNotes";
        private const string PROC_ATTACH_SOCIALHX_FROM_NOTES = "Clinical.sp_AttachSocialHxWithNotes";
        private const string PROC_UPDATE_SOAPTEXT_FOR_SOCIALHX = "Clinical.sp_UpdateSoapTextForSocialHX";
        // end azhar changed

        // Start 08/01/2016 Muhammad Arshad MedicalHx association with note SPs
        private const string PROC_DETACH_MEDICALHX_FROM_NOTES = "Clinical.sp_DetachMedicalHxFromNotes";
        private const string PROC_ATTACH_MEDICALHX_FROM_NOTES = "Clinical.sp_AttachMedicalHxWithNotes";
        private const string PROC_UPDATE_SOAPTEXT_FOR_MEDICALHX = "Clinical.sp_UpdateSoapTextForMedicalHx";

        private const string PROC_NOTES_MEDICALHX_SELECT = "Clinical.sp_NotesMedicalHxSelect";

        // Start 08/01/2016 Muhammad Arshad MedicalHx association with note SPs

        #endregion

        #region "Parameters"
        // Start 08/01/2016 Muhammad Arshad for MedicalHx in Clinical
        private const string PARM_MEDICAL_HX_DISEASE_ID = "@DiseaseId";
        private const string PARM_MEDICAL_HX_ID = "@MedicalHxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_MEDICAL_HX_DATE = "@MedicalHxDate";
        private const string PARM_B_UNREMARKABLE = "@bUnremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_SOAPTEXT_BY = "@SoapText";
        private const string PARM_DISEASE_ID = "@DiseaseId";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD9_CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10CodeDescription";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_LEXI_CODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_FROM_DATE = "@FromDate";
        private const string PARM_TO_DATE = "@ToDate";
        private const string PARM_ONSET = "@Onset";
        private const string PARM_DURATION_LENGTH = "@DurationLength";
        private const string PARM_DURATION_PERIOD_ID = "@DurationPeriodId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODE_DESCRIPTION = "@CPTCodeDescription";
        private const string PARM_TEST_RESULT_ID = "@TestResultId";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_LOCATION = "@Location";
        private const string PARM_SEVERITY_ID = "@SeverityId";
        private const string PARM_PATTERN_ID = "@PatternId";
        private const string PARM_AGGRAVATED_BY_ID = "@AggravatedById";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_IS_PARENT = "@IsParent";

        private const string PARM_CPT_SNOMED_ID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMED_DESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_FREE_TEXT_ICD = "@FreeTextICD";
        private const string PARM_MEDICAL_HX_TEMP_DISEASE_ID = "@tempDiseaseId";
        private const string PARM_ICD_ID = "@tempIcdId";
        private const string PARM_ADDED_FROM_MOBILE = "@AddedFromMobileApp";
        // End 08/01/2016 Muhammad Arshad for MedicalHx in Clinical
        private const string PARM_HX_ID = "@HxId";
        private const string PARM_HX_TYPE = "@HxType";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";


        private const string PARM_IS_RC_PNEUMOCOCCAL = "@IsRCPneumococcal";
        private const string PARM_IS_RC_INFLUENZA = "@IsRCInfluenza";
        private const string PARM_RC_PNEUMOCOCCAL_DATE = "@RCPneumococcalDate";
        private const string PARM_RC_INFLUENZA_DATE = "@RCInfluenzaDate";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion

        #region "Support Functions MedicalHx"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersMedicalHx(IDBManager dbManager, DSMedicalHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(16);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, ds.MedicalHx.MedicalHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, ds.MedicalHx.MedicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.MedicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MEDICAL_HX_DATE, ds.MedicalHx.MedicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_B_UNREMARKABLE, ds.MedicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.MedicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.MedicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.MedicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.MedicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.MedicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.MedicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAPTEXT_BY, ds.MedicalHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NOTE_ID, ds.MedicalHx.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_IS_RC_INFLUENZA, ds.MedicalHx.IsRCInfluenzaColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_IS_RC_PNEUMOCOCCAL, ds.MedicalHx.IsRCPneumococcalColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(14, PARM_RC_INFLUENZA_DATE, ds.MedicalHx.RCInfluenzaDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_RC_PNEUMOCOCCAL_DATE, ds.MedicalHx.RCPneumococcalDateColumn.ColumnName, DbType.DateTime);

        }

        private void CreateMedicalHxInsertParameters(IDBManager dbManager, DSMedicalHx ds)
        {
            dbManager.CreateInsertParameters(15);

            dbManager.AddInsertUpdateParameters(0, PARM_MEDICAL_HX_ID, ds.MedicalHx.MedicalHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.MedicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_MEDICAL_HX_DATE, ds.MedicalHx.MedicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.MedicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.MedicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.MedicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.MedicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.MedicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.MedicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.MedicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.MedicalHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_IS_RC_PNEUMOCOCCAL, ds.MedicalHx.IsRCPneumococcalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_RC_INFLUENZA, ds.MedicalHx.IsRCInfluenzaColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_RC_PNEUMOCOCCAL_DATE, ds.MedicalHx.RCPneumococcalDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_RC_INFLUENZA_DATE, ds.MedicalHx.RCInfluenzaDateColumn.ColumnName, DbType.DateTime);
        }
        private void CreateMedicalHxUpdateParameters(IDBManager dbManager, DSMedicalHx ds)
        {
            dbManager.CreateUpdateParameters(15);

            dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, ds.MedicalHx.MedicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.MedicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_MEDICAL_HX_DATE, ds.MedicalHx.MedicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.MedicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.MedicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.MedicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.MedicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.MedicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.MedicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.MedicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.MedicalHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_IS_RC_PNEUMOCOCCAL, ds.MedicalHx.IsRCPneumococcalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_RC_INFLUENZA, ds.MedicalHx.IsRCInfluenzaColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_RC_PNEUMOCOCCAL_DATE, ds.MedicalHx.RCPneumococcalDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_RC_INFLUENZA_DATE, ds.MedicalHx.RCInfluenzaDateColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region"Supporting Functions MedicalHx Disease"
        /* Start 12/01/2016 Muhammad Irfan create parameters for disease*/
        private void CreateParametersMedicalHxDisease(IDBManager dbManager, DSMedicalHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(30);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, ds.MedicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, ds.MedicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, ds.MedicalHx_Disease.MedicalHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD9_CODE, ds.MedicalHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10_CODE, ds.MedicalHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMED_ID, ds.MedicalHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SNOMED_DESCRIPTION, ds.MedicalHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_LEXI_CODE, ds.MedicalHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.MedicalHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_FROM_DATE, ds.MedicalHx_Disease.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_TO_DATE, ds.MedicalHx_Disease.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_ONSET, ds.MedicalHx_Disease.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DURATION_LENGTH, ds.MedicalHx_Disease.DurationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_DURATION_PERIOD_ID, ds.MedicalHx_Disease.DurationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_CPT_CODE, ds.MedicalHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CPT_CODE_DESCRIPTION, ds.MedicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_TEST_RESULT_ID, ds.MedicalHx_Disease.TestResultIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(18, PARM_STATUS_ID, ds.MedicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(19, PARM_LOCATION, ds.MedicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_SEVERITY_ID, ds.MedicalHx_Disease.SeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(21, PARM_PATTERN_ID, ds.MedicalHx_Disease.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(22, PARM_AGGRAVATED_BY_ID, ds.MedicalHx_Disease.AggravatedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(23, PARM_COMMENTS, ds.MedicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_IS_ACTIVE, ds.MedicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(25, PARM_CREATED_BY, ds.MedicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_CREATED_ON, ds.MedicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(27, PARM_MODIFIED_BY, ds.MedicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_MODIFIED_ON, ds.MedicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_SOAP_TEXT, ds.MedicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);

        }
        private void CreateMedicalHxDiseaseInsertParameters(IDBManager dbManager, DSMedicalHx ds)
        {
            dbManager.CreateInsertParameters(36);

            dbManager.AddInsertUpdateParameters(0, PARM_MEDICAL_HX_DISEASE_ID, ds.MedicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_MEDICAL_HX_ID, ds.MedicalHx_Disease.MedicalHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.MedicalHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.MedicalHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.MedicalHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.MedicalHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.MedicalHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.MedicalHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_FROM_DATE, ds.MedicalHx_Disease.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_TO_DATE, ds.MedicalHx_Disease.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_ONSET, ds.MedicalHx_Disease.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_DURATION_LENGTH, ds.MedicalHx_Disease.DurationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_DURATION_PERIOD_ID, ds.MedicalHx_Disease.DurationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_CODE, ds.MedicalHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CPT_CODE_DESCRIPTION, ds.MedicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_TEST_RESULT_ID, ds.MedicalHx_Disease.TestResultIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_STATUS_ID, ds.MedicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(19, PARM_LOCATION, ds.MedicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_SEVERITY_ID, ds.MedicalHx_Disease.SeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(21, PARM_PATTERN_ID, ds.MedicalHx_Disease.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(22, PARM_AGGRAVATED_BY_ID, ds.MedicalHx_Disease.AggravatedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(23, PARM_COMMENTS, ds.MedicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_IS_ACTIVE, ds.MedicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(25, PARM_CREATED_BY, ds.MedicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_CREATED_ON, ds.MedicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_MODIFIED_BY, ds.MedicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_MODIFIED_ON, ds.MedicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_SOAP_TEXT, ds.MedicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(30, PARM_CPT_SNOMED_ID, ds.MedicalHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(31, PARM_CPT_SNOMED_DESCRIPTION, ds.MedicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(32, PARM_FREE_TEXT_ICD, ds.MedicalHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(33, PARM_MEDICAL_HX_TEMP_DISEASE_ID, ds.MedicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(34, PARM_ADDED_FROM_MOBILE, ds.MedicalHx_Disease.AddedFromMobileAppColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(35, PARM_ICD_ID, ds.MedicalHx_Disease.ICDIDColumn.ColumnName, DbType.Int64);

        }
        private void CreateMedicalHxDiseaseUpdateParameters(IDBManager dbManager, DSMedicalHx ds)
        {
            dbManager.CreateUpdateParameters(33);

            dbManager.AddInsertUpdateParameters(0, PARM_MEDICAL_HX_DISEASE_ID, ds.MedicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_MEDICAL_HX_ID, ds.MedicalHx_Disease.MedicalHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.MedicalHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.MedicalHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.MedicalHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.MedicalHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.MedicalHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.MedicalHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.MedicalHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_FROM_DATE, ds.MedicalHx_Disease.FromDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_TO_DATE, ds.MedicalHx_Disease.ToDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_ONSET, ds.MedicalHx_Disease.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_DURATION_LENGTH, ds.MedicalHx_Disease.DurationLengthColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_DURATION_PERIOD_ID, ds.MedicalHx_Disease.DurationPeriodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_CODE, ds.MedicalHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_CPT_CODE_DESCRIPTION, ds.MedicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_TEST_RESULT_ID, ds.MedicalHx_Disease.TestResultIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_STATUS_ID, ds.MedicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(19, PARM_LOCATION, ds.MedicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_SEVERITY_ID, ds.MedicalHx_Disease.SeverityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(21, PARM_PATTERN_ID, ds.MedicalHx_Disease.PatternIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(22, PARM_AGGRAVATED_BY_ID, ds.MedicalHx_Disease.AggravatedByIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(23, PARM_COMMENTS, ds.MedicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_IS_ACTIVE, ds.MedicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(25, PARM_CREATED_BY, ds.MedicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_CREATED_ON, ds.MedicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_MODIFIED_BY, ds.MedicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_MODIFIED_ON, ds.MedicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(29, PARM_SOAP_TEXT, ds.MedicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(30, PARM_CPT_SNOMED_ID, ds.MedicalHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(31, PARM_CPT_SNOMED_DESCRIPTION, ds.MedicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(32, PARM_FREE_TEXT_ICD, ds.MedicalHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);
        }
        /* End 12/01/2016 Muhammad Irfan create parameters for disease*/

        #endregion

        #region "MedicalHx Insert/Update/Delete/Select"
        // Start 08/01/2016 Muhammad Arshad MedicalHx Parent Table
        public DSMedicalHx InsertMedicalHx(DSMedicalHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.MedicalHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                CreateParametersMedicalHx(dbManager, ds, true);

                ds = (DSMedicalHx)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_INSERT, ds, ds.MedicalHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), null, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALMedicalHx::InsertMedicalHx", PROC_MEDICALHX_INSERT, ex);
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
        public DSMedicalHx UpdateMedicalHx(DSMedicalHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.MedicalHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParametersMedicalHx(dbManager, ds, false);
                ds = (DSMedicalHx)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_UPDATE, ds, ds.MedicalHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), null, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALMedicalHx::UpdateMedicalHx", PROC_MEDICALHX_UPDATE, ex);
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
        public string DeleteMedicalHx(string MedicalHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSMedicalHx dsCurrentMedicalHx = LoadMedicalHx(0, Convert.ToInt64(MedicalHxId), "", "");
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, MedicalHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEDICALHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                else
                {

                    DataTable dtTemp = dsCurrentMedicalHx.MedicalHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, MedicalHxId.ToString(), null, MedicalHxId.ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALMedicalHx::DeleteMedicalHx", PROC_MEDICALHX_DELETE, ex);
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

        public MedicalHxModel LoadNoteMedicalHx(long PatientId, long Userid, long EntityId, long NoteId)
        {
            MedicalHxModel model = new MedicalHxModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                parameters.Add(new SqlParameter(PARM_USER_ID, Userid));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_MEDICALHX_SELECT_ForSoapText, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(MedicalHxModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALMedicalHx::LoadNoteMedicalHx", PROC_MEDICALHX_SELECT_ForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSMedicalHx LoadMedicalHx(long PatientId, long MedicalHxId, string isViewMedicalHx = "", string isPrintMedicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSMedicalHx ds = new DSMedicalHx();
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

                if (MedicalHxId == 0)
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, MedicalHxId);
                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_SELECT, ds, ds.MedicalHx.TableName);
                if (ds.MedicalHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.MedicalHx.Rows[0]["MedicalHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.MedicalHx;
                        if (dtTemp != null)
                        {
                            if (isViewMedicalHx == "1" || isPrintMedicalHx == "1")
                            {
                                bool isViewAction = isViewMedicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintMedicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), null, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHx", PROC_MEDICALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHx LoadMedicalHxNative(long PatientId, string RequestStatus, long DiseaseId = 0)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSMedicalHx ds = new DSMedicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();

                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, "RequestStatus", RequestStatus);


                if (DiseaseId == 0)
                    dbManager.AddParameters(2, PARM_DISEASE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_DISEASE_ID, DiseaseId);
                var tableNames = new List<string>
                {
                    ds.MedicalHx.TableName,
                    ds.MedicalHx_Disease.TableName,
                    ds.MedicalHx_NativeChangeset.TableName,
                };


                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_SELECT_NATIVE, ds, tableNames);



                if (ds.MedicalHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.MedicalHx.Rows[0]["MedicalHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.MedicalHx;
                        if (dtTemp != null)
                        {


                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString(), null, ds.MedicalHx.Rows[0][ds.MedicalHx.MedicalHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHx", PROC_MEDICALHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // End 08/01/2016 Muhammad Arshad MedicalHx Parent Table
        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason: Function to get updated Soap text of  Medical history for progress note
           Created Date: Dec 15, 2015
       */
        public string updateSoapTextForMedicalHX(long MedicalHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, MedicalHxId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_MEDICALHX).ToString();

                if (returnVal == "" || returnVal == "-1")
                    return "";
                else
                    throw new Exception(returnVal);


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::updateSoapTextForMedicalHX", PROC_UPDATE_SOAPTEXT_FOR_MEDICALHX, ex);
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

        #region MedicalHx_Disease Insert/Update/Delete/Select

        // Start 11/01/2016 Muhammad Arshad for MedicalHx_Disease in Clinical
        public DSMedicalHx LoadMedicalHx_Disease(long MedicalHxId, long DiseaseId = 0, string isView = "", string isPrint = "")
        {
            DSMedicalHx ds = new DSMedicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (MedicalHxId == 0)
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, MedicalHxId);

                if (DiseaseId == 0)
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, DiseaseId);

                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_DISEASE_SELECT, ds, ds.MedicalHx_Disease.TableName);
                //Start 31-10-2016 Humaira Yousaf for dbaudit
                if (ds.MedicalHx_Disease.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.MedicalHx_Disease.Rows[0]["MedicalHxId"]) > 0 && DiseaseId > 0)
                    {

                        DataTable dtTemp = ds.MedicalHx_Disease;
                        if (dtTemp != null)
                        {
                            if (isView == "1" || isPrint == "1")
                            {
                                bool isViewAction = isView == "1" ? true : false;
                                bool isPrintAcion = isPrint == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx_Disease.Rows[0][ds.MedicalHx_Disease.DiseaseIdColumn].ToString(), null, ds.MedicalHx_Disease.Rows[0][ds.MedicalHx_Disease.MedicalHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                //End 31-10-2016 Humaira Yousaf for dbaudit
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHx_Disease", PROC_MEDICALHX_DISEASE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // End 11/01/2016 Muhammad Arshad for MedicalHx_Disease in Clinical

        /* Start 12/01/2016 Muhammad Irfan insert update disease */
        public DSMedicalHx insertUpdateMedicalHxDisease(DSMedicalHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.MedicalHx_Disease.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateMedicalHxDiseaseInsertParameters(dbManager, ds);
                CreateMedicalHxDiseaseUpdateParameters(dbManager, ds);
                ds = (DSMedicalHx)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_DISEASE_INSERT, PROC_MEDICALHX_DISEASE_UPDATE, ds, ds.MedicalHx_Disease.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");

                    DataTable dtTempNew = ds.MedicalHx_Disease.GetChanges();

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.MedicalHx_Disease.Where(
                    //            n => (string.IsNullOrWhiteSpace(n.ICD10Code) ? n.FreeTextICD : n.ICD10Code
                    //                0
                    //                == string.IsNullOrWhiteSpace(dtTemp.Rows[i]["ICD10Code"]) ? dtTemp.Rows[i]["FreeTextICD"] : dtTemp.Rows[i]["ICD10Code"])

                    //        ).Select(n => n.DiseaseId).FirstOrDefault(); //.Rows[i][ds.MedicalHx_Disease.DiseaseIdColumn];
                    //}
                    int k = 0;
                    for (int i = 0; i < ds.MedicalHx_Disease.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > k)
                        {
                            bool isExists = false;
                            if (!ds.MedicalHx_Disease.Rows[i].IsNull("FreeTextICD") &&
                                ds.MedicalHx_Disease.Rows[i][ds.MedicalHx_Disease.FreeTextICDColumn] == dtTemp.Rows[k]["FreeTextICD"])
                            {
                                isExists = true;
                            }
                            if (!ds.MedicalHx_Disease.Rows[i].IsNull("ICD10Code") &&
                                ds.MedicalHx_Disease.Rows[i][ds.MedicalHx_Disease.ICD10CodeColumn] == dtTemp.Rows[k]["ICD10Code"])
                            {
                                isExists = true;
                            }
                            if (isExists)
                            {
                                dtTemp.Rows[k]["PrimaryKey"] = ds.MedicalHx_Disease.Rows[i][ds.MedicalHx_Disease.DiseaseIdColumn];
                                k++;
                            }

                        }

                    }
                    //  foreach (var item in dtTemp.Rows)
                    //{
                    //                         for (int i = 0; i < ds.MedicalHx_Disease.Rows.Count; i++)
                    //                {
                    //                             if ( dtTemp.Rows[i]["PrimaryKey"] = ds.MedicalHx_Disease.Rows[i][ds.MedicalHx_Disease.DiseaseIdColumn])
                    //{

                    //}
                    //                    dtTemp.Rows[i]["PrimaryKey"] = ds.MedicalHx_Disease.Rows[i][ds.MedicalHx_Disease.DiseaseIdColumn];
                    //                }
                    //}
                    //                DataRow dr = ds.MedicalHx_Disease.Select(n=>n.);


                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.MedicalHx_Disease.Rows[0][ds.MedicalHx_Disease.DiseaseIdColumn].ToString(), null, ds.MedicalHx_Disease.Rows[0][ds.MedicalHx_Disease.MedicalHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALMedicalHx::insertUpdateMedicalHxDisease", PROC_MEDICALHX_DISEASE_INSERT + " " + PROC_MEDICALHX_DISEASE_UPDATE, ex);
                throw ex;
            }
        }
        /* End 12/01/2016 Muhammad Irfan insert update disease */
        /* Start 12/01/2016 Muhammad Irfan delete disease */
        public string deleteMedicalHxDisease(string DiseaseId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();

                dbManager.BeginTransaction();

                DSMedicalHx dsMedicalHx = LoadMedicalHx_Disease(0, Convert.ToInt64(DiseaseId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, DiseaseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEDICALHX_DISEASE_DELETE).ToString();

                if (returnVal != "" && returnVal != "-1" && returnVal != "Successfully Deleted")
                    throw new Exception(returnVal);
                else
                {
                    if (dsMedicalHx.MedicalHx_Disease.Rows.Count > 0)
                    {
                        DataTable dtTemp = dsMedicalHx.MedicalHx_Disease;
                        if (dtTemp != null)
                        {
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, DiseaseId, null, dsMedicalHx.MedicalHx_Disease.Rows[0][dsMedicalHx.MedicalHx_Disease.MedicalHxIdColumn].ToString(), false, false, true);
                            dsDBAudit.AcceptChanges();
                        }
                        dbManager.CommitTransaction();
                    }
                    else
                    {
                        throw new Exception("Disease Data Not Found");
                    }

                }
                return "";
            }
            catch (Exception ex)
            {

                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALMedicalHx::deleteMedicalHxDisease", PROC_MEDICALHX_DISEASE_DELETE, ex);
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
        /* End 12/01/2016 Muhammad Irfan delete disease */

        #endregion

        #region "MEDICAL History Lookups"
        public DSMedicalHxLookup LookupMedicalHxTestResults()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX__TESTRESULT_LOOKUP, ds, ds.MedicalHx_TestResult.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxTestResults", PROC_MEDICALHX__TESTRESULT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHxLookup LookupMedicalHxDurationPeriod()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX__DURATIONPERIOD_LOOKUP, ds, ds.MedicalHx_DurationPeriod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxDurationPeriod", PROC_MEDICALHX__DURATIONPERIOD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHxLookup LookupMedicalHxPattern()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX__PATTERN_LOOKUP, ds, ds.MedicalHx_Pattern.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxPatternPeriod", PROC_MEDICALHX__PATTERN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHxLookup LookupMedicalHxSeverity()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_SEVERITY_LOOKUP, ds, ds.MedicalHx_Severity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxPatternPeriod", PROC_MEDICALHX_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHxLookup LookupMedicalHxAggravatedBy()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_AGGRAVATED_LOOKUP, ds, ds.MedicalHx_AggravatedBy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxPatternPeriod", PROC_MEDICALHX_AGGRAVATED_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSMedicalHxLookup LookupMedicalHxStatus()
        {
            DSMedicalHxLookup ds = new DSMedicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSMedicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_STATUS_LOOKUP, ds, ds.MedicalHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMEDICALHistory::LookupMedicalHxPatternPeriod", PROC_MEDICALHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion "MEDICAL History Lookups"
        public DSMedicalHx attachMedicalHxWithNotes(string medicalHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSMedicalHx ds = new DSMedicalHx();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(medicalHxId))
                {
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, medicalHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_MEDICALHX_FROM_NOTES, ds, ds.MedicalHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::attachMedicalHxWithNotes", PROC_ATTACH_MEDICALHX_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //Start//11/01/2016//Ahmad Raza//Logic implimented to detach MedicalHx from Notes
        public string detachMedicalHxFromNotes(long medicalHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (medicalHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_MEDICAL_HX_ID, medicalHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_MEDICALHX_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::detachMedicalHxFromNotes", PROC_DETACH_MEDICALHX_FROM_NOTES, ex);
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
        //End//11/01/2016//Ahmad Raza//Logic implimented to detach MedicalHx from Notes


        #region "Native Functions"
        public List<HistoryLookupModel> LookupMedicalHxStatusNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MEDICALHX_STATUS_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.MedicalHxStatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                    model.MedicalHxStatusDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::LookupMedicalHxStatusNative", PROC_MEDICALHX_STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<MedicalHxDiseaseModel> LoadMedicalHxDiseaseNative(long PatientId)
        {
            DSMedicalHx ds = new DSMedicalHx();
            List<MedicalHxDiseaseModel> HistoryList = new List<MedicalHxDiseaseModel>();
            string ConnectionString = "";
            string ConnectionString2 = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            IDBManager dbManager_2 = ClientConfiguration.GetCustomerDBManager(ref ConnectionString2, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_MEDICAL_HX_ID, null);

                ds = (DSMedicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICALHX_SELECT, ds, ds.MedicalHx.TableName);
                if (ds.MedicalHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.MedicalHx.Rows[0]["MedicalHxId"]) > 0)
                    {
                        Int64 MedHxId = Convert.ToInt64(ds.MedicalHx.Rows[0]["MedicalHxId"]);

                        dbManager_2.Open();
                        dbManager_2.CreateParameters(2);

                        dbManager_2.AddParameters(1, PARM_MEDICAL_HX_ID, MedHxId);

                        dbManager_2.AddParameters(0, PARM_MEDICAL_HX_DISEASE_ID, null);

                        reader = (SqlDataReader)dbManager_2.ExecuteReader(CommandType.StoredProcedure, PROC_MEDICALHX_DISEASE_SELECT);
                        MedicalHxDiseaseModel model = null;
                        while (reader.Read())
                        {
                            model = new MedicalHxDiseaseModel();
                            model.DiseaseId = !String.IsNullOrEmpty(reader["DiseaseId"].ToString()) ? reader["DiseaseId"].ToString() : "";
                            model.MedicalHxId = !String.IsNullOrEmpty(reader["MedicalHxId"].ToString()) ? reader["MedicalHxId"].ToString() : "";
                            model.ICD9Code = !String.IsNullOrEmpty(reader["ICD9Code"].ToString()) ? reader["ICD9Code"].ToString() : "";
                            model.ICD9CodeDescription = !String.IsNullOrEmpty(reader["ICD9CodeDescription"].ToString()) ? reader["ICD9CodeDescription"].ToString() : "";
                            model.ICD10Code = !String.IsNullOrEmpty(reader["ICD10Code"].ToString()) ? reader["ICD10Code"].ToString() : "";
                            model.ICD10CodeDescription = !String.IsNullOrEmpty(reader["ICD10CodeDescription"].ToString()) ? reader["ICD10CodeDescription"].ToString() : "";
                            model.SNOMEDID = !String.IsNullOrEmpty(reader["SNOMEDID"].ToString()) ? reader["SNOMEDID"].ToString() : "";
                            model.SNOMEDDescription = !String.IsNullOrEmpty(reader["SNOMEDDescription"].ToString()) ? reader["SNOMEDDescription"].ToString() : "";
                            model.LexiCode = !String.IsNullOrEmpty(reader["LexiCode"].ToString()) ? reader["LexiCode"].ToString() : "";
                            model.LexiCodeDescription = !String.IsNullOrEmpty(reader["LexiCodeDescription"].ToString()) ? reader["LexiCodeDescription"].ToString() : "";
                            model.StatusId = !String.IsNullOrEmpty(reader["StatusId"].ToString()) ? reader["StatusId"].ToString() : "";
                            //model.CPTSNOMEDID = !String.IsNullOrEmpty(reader["CPTSNOMEDID"].ToString()) ? reader["CPTSNOMEDID"].ToString() : "";
                            //model.CPTSNOMEDDescription = !String.IsNullOrEmpty(reader["CPTSNOMEDDescription"].ToString()) ? reader["CPTSNOMEDDescription"].ToString() : "";

                            HistoryList.Add(model);
                        }
                        return HistoryList;
                    }
                }

                return HistoryList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHxDiseaseNative", PROC_MEDICALHX_SELECT, ex);
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

        public List<MedicalHxDiseaseModel> LoadMedicalHxLogNative(long medicalHxId)
        {
            List<MedicalHxDiseaseModel> HistoryLookupList = new List<MedicalHxDiseaseModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                //--------------
                dbManager.CreateParameters(6);

                if (medicalHxId == 0)
                    dbManager.AddParameters(0, PARM_HX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HX_ID, medicalHxId);

                dbManager.AddParameters(1, PARM_HX_TYPE, "MedicalHx");

                dbManager.AddParameters(2, PARM_LOG_TYPE, "Current");

                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);


                dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, 500);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ParamDirection.Output);

                //--------------
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HXLOG_SELECT);
                MedicalHxDiseaseModel model = null;
                while (reader.Read())
                {
                    model = new MedicalHxDiseaseModel();
                    model.SoapText = !String.IsNullOrEmpty(reader["SoapText"].ToString()) ? reader["SoapText"].ToString() : "";
                    model.Action = !String.IsNullOrEmpty(reader["Action"].ToString()) ? reader["Action"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::LoadMedicalHxLogNative", PROC_HXLOG_SELECT, ex);
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

        public List<MedicalHx> NotesMedicalHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MedicalHx> objList_MedicalHx = new List<MedicalHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_MEDICALHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        MedicalHx model = new MedicalHx();
                        var properties = typeof(MedicalHx).GetProperties();
                        Type type = model.GetType();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                //PRD-269 by:MAHmAD
                                PropertyInfo propertyInfo = type.GetProperty(prop.Name);
                                var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
                                var propertyVal = Convert.ChangeType(reader[prop.Name], targetType);
                                propertyInfo.SetValue(model, propertyVal, null);
                                //PRD-269 by:MAHmAD
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_MedicalHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedicalHx::NotesMedicalHxSelect", PROC_NOTES_MEDICALHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_MedicalHx;
        }
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
        #endregion Legacy Notes

    }
}
