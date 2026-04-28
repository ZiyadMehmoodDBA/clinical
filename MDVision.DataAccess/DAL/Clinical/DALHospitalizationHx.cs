/* Author:  Muhammad Arshad
 * Created Date: 14/01/2016
 * OverView: Created for HospitalizationHx in Clinical Module
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
using System.Data.SqlClient;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALHospitalizationHx
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_HOSPITALIZATIONHX__STAY_LOOKUP = "Clinical.sp_HospitalizationHx_StayLookup";
        private const string PROC_HOSPITALIZATIONHX__STATUS_LOOKUP = "Clinical.sp_HospitalizationHx_StatusLookup";
        private const string PROC_HOSPITALIZATIONHX__HOSPITAL_LOOKUP = "Clinical.sp_HospitalizationHx_HospitalLookup";

        private const string PROC_HOSPITALIZATIONHX_INSERT = "Clinical.sp_HospitalizationHxInsert";
        private const string PROC_HOSPITALIZATIONHX_UPDATE = "Clinical.sp_HospitalizationHxUpdate";
        private const string PROC_HOSPITALIZATIONHX_DELETE = "Clinical.sp_HospitalizationHxDelete";
        private const string PROC_HOSPITALIZATIONHX_SELECT = "Clinical.sp_HospitalizationHxSelect";
        private const string PROC_HOSPITALIZATIONHX_SELECTForSoapText = "Clinical.sp_HospitalizationHxSelectForSoapText";

        private const string PROC_UPDATE_SOAPTEXT_FOR_HOSPITALIZATIONHX = "Clinical.sp_UpdateSoapTextForHospitalizationHX";
        private const string PROC_ATTACH_HOSPITALIZATIONHX_WITH_NOTES = "Clinical.sp_AttachHospitalizationHxWithNotes";
        private const string PROC_DETACH_HOSPITALIZATIONHX_FORM_NOTES = "Clinical.sp_DetachHospitalizationHxFromNotes";

        //Start/ Abid Ali // 1-20-2016 //HospitalHx Disease
        private const string PROC_HOSPITALIZATIONHX_Disease_INSERT = "Clinical.sp_HospitalizationHx_DiseaseInsert";
        private const string PROC_HOSPITALIZATIONHX_Disease_UPDATE = "Clinical.sp_HospitalizationHx_DiseaseUpdate";
        private const string PROC_HOSPITALIZATIONHX_Disease_DELETE = "Clinical.sp_HospitalizationHx_DiseaseDelete";
        private const string PROC_HOSPITALIZATIONHX_Disease_SELECT = "Clinical.sp_HospitalizationHx_DiseaseSelect";
        //End/ Abid Ali // 1-20-2016 //HospitalHx Disease

        private const string PROC_ATTACH_HOSPITALIZATIONHX_FROM_NOTE = "Clinical.sp_AttachHospitalizationHxWithNotes";
        private const string PROC_DETACH_HOSPITALIZATIONHX_FROM_NOTE = "Clinical.sp_DetachHospitalizationHxFromNotes";

        private const string PROC_NOTES_HOSPITALIZATIONHX_SELECT = "[Clinical].[sp_NotesHospitalizationHxSelect]";

        #endregion

        #region "Parameters"

        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_HOSPITALIZATION_HX_DISEASE_ID = "@DiseaseId";
        private const string PARM_HOSPITALIZATION_HX_TEMP_DISEASE_ID = "@tempDiseaseId";
        private const string PARM_temp_ICD_ID = "@tempIcdId";
        private const string PARM_temp_CPT_ID = "@tempCptId";
        private const string PARM_ADDED_FROM_MOBILE = "@AddedFromMobileApp";
        private const string PARM_HOSPITALIZATION_HX_ID = "@HospitalizationHxId";
        private const string PARM_HOSPITALIZATION_HX_DATE = "@HospitalizationHxDate";

        private const string PARM_HOSPITAL = "@Hospital";

        private const string PARAM_DISCHARGE_DATE = "@DischargeDate";
        private const string PARM_ADMISSION_DATE = "@AdmissionDate";
        private const string PARM_STAY_ID = "@StayId";
        private const string PARM_STAY_DURATION = "@StayDuration";

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
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODE_DESCRIPTION = "@CPTCodeDescription";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_CPT_SNOMED_ID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMED_DESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_FREE_TEXT_ICD = "@FreeTextICD";


        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        #endregion
        #region Constructors

        public DALHospitalizationHx()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALHospitalizationHx(SharedVariable SharedVariable)
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

        #region "Support Functions HospitalizationHx"
        //Start/20-1-2016/Abid Ali, Hospitalization History support functions
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersHospitalizationHx(IDBManager dbManager, DSHospitalizationHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx.HospitalizationHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx.HospitalizationHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.HospitalizationHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_HOSPITALIZATION_HX_DATE, ds.HospitalizationHx.HospitalizationHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_B_UNREMARKABLE, ds.HospitalizationHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.HospitalizationHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.HospitalizationHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.HospitalizationHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.HospitalizationHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.HospitalizationHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.HospitalizationHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAPTEXT_BY, ds.HospitalizationHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NOTE_ID, ds.HospitalizationHx.NotesIdColumn.ColumnName, DbType.Int64);
        }

        private void CreateHospitalizationHxInsertParameters(IDBManager dbManager, DSHospitalizationHx ds)
        {
            dbManager.CreateInsertParameters(11);

            dbManager.AddInsertUpdateParameters(0, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx.HospitalizationHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.HospitalizationHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_HOSPITALIZATION_HX_DATE, ds.HospitalizationHx.HospitalizationHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.HospitalizationHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.HospitalizationHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.HospitalizationHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.HospitalizationHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.HospitalizationHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.HospitalizationHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.HospitalizationHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.HospitalizationHx.SoapTextColumn.ColumnName, DbType.String);
        }
        private void CreateHospitalizationHxUpdateParameters(IDBManager dbManager, DSHospitalizationHx ds)
        {
            dbManager.CreateUpdateParameters(11);

            dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx.HospitalizationHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.HospitalizationHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_HOSPITALIZATION_HX_DATE, ds.HospitalizationHx.HospitalizationHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.HospitalizationHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.HospitalizationHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.HospitalizationHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.HospitalizationHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.HospitalizationHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.HospitalizationHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.HospitalizationHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT_BY, ds.HospitalizationHx.SoapTextColumn.ColumnName, DbType.String);
        }

        //End/20-1-2016/Abid Ali, Hospitalization History support functions
        #endregion

        #region "Supporting Functions HospitalizationHx Disease"
        //Start/20-1-2016/Farooq Ahmad, Hospitalization History Disease support functions
        private void CreateParametersHospitalizationHxDisease(IDBManager dbManager, DSHospitalizationHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(25);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx_Disease.HospitalizationHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD9_CODE, ds.HospitalizationHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10_CODE, ds.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMED_ID, ds.HospitalizationHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SNOMED_DESCRIPTION, ds.HospitalizationHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_LEXI_CODE, ds.HospitalizationHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARAM_DISCHARGE_DATE, ds.HospitalizationHx_Disease.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_ADMISSION_DATE, ds.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_STAY_ID, ds.HospitalizationHx_Disease.StayIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_STAY_DURATION, ds.HospitalizationHx_Disease.StayDurationColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_CPT_CODE, ds.HospitalizationHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CPT_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_STATUS_ID, ds.HospitalizationHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(17, PARM_COMMENTS, ds.HospitalizationHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_IS_ACTIVE, ds.HospitalizationHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(19, PARM_CREATED_BY, ds.HospitalizationHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_CREATED_ON, ds.HospitalizationHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_MODIFIED_BY, ds.HospitalizationHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_MODIFIED_ON, ds.HospitalizationHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_SOAP_TEXT, ds.HospitalizationHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_HOSPITAL, ds.HospitalizationHx_Disease.HospitalColumn.ColumnName, DbType.String);

        }
        private void CreateHospitalizationHxDiseaseInsertParameters(IDBManager dbManager, DSHospitalizationHx ds)
        {
            dbManager.CreateInsertParameters(32);

            dbManager.AddInsertUpdateParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx_Disease.HospitalizationHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.HospitalizationHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.HospitalizationHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.HospitalizationHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.HospitalizationHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARAM_DISCHARGE_DATE, ds.HospitalizationHx_Disease.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_ADMISSION_DATE, ds.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_STAY_ID, ds.HospitalizationHx_Disease.StayIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(13, PARM_STAY_DURATION, ds.HospitalizationHx_Disease.StayDurationColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_CPT_CODE, ds.HospitalizationHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_STATUS_ID, ds.HospitalizationHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(17, PARM_COMMENTS, ds.HospitalizationHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_IS_ACTIVE, ds.HospitalizationHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(19, PARM_CREATED_BY, ds.HospitalizationHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_CREATED_ON, ds.HospitalizationHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(21, PARM_MODIFIED_BY, ds.HospitalizationHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_MODIFIED_ON, ds.HospitalizationHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(23, PARM_SOAP_TEXT, ds.HospitalizationHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_HOSPITAL, ds.HospitalizationHx_Disease.HospitalColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(25, PARM_CPT_SNOMED_ID, ds.HospitalizationHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_CPT_SNOMED_DESCRIPTION, ds.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(27, PARM_FREE_TEXT_ICD, ds.HospitalizationHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(28, PARM_HOSPITALIZATION_HX_TEMP_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(29, PARM_temp_CPT_ID, ds.HospitalizationHx_Disease.CPTIDColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(30, PARM_temp_ICD_ID, ds.HospitalizationHx_Disease.ICDIDColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(31, PARM_ADDED_FROM_MOBILE, ds.HospitalizationHx_Disease.AddedFromMobileAppColumn.ColumnName, DbType.String);

        }
        private void CreateHospitalizationHxDiseaseUpdateParameters(IDBManager dbManager, DSHospitalizationHx ds)
        {
            dbManager.CreateUpdateParameters(28);

            dbManager.AddInsertUpdateParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, ds.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_HOSPITALIZATION_HX_ID, ds.HospitalizationHx_Disease.HospitalizationHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.HospitalizationHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.HospitalizationHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.HospitalizationHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.HospitalizationHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARAM_DISCHARGE_DATE, ds.HospitalizationHx_Disease.DischargeDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_ADMISSION_DATE, ds.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_STAY_ID, ds.HospitalizationHx_Disease.StayIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(13, PARM_STAY_DURATION, ds.HospitalizationHx_Disease.StayDurationColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(14, PARM_CPT_CODE, ds.HospitalizationHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CPT_CODE_DESCRIPTION, ds.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_STATUS_ID, ds.HospitalizationHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(17, PARM_COMMENTS, ds.HospitalizationHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_IS_ACTIVE, ds.HospitalizationHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(19, PARM_CREATED_BY, ds.HospitalizationHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_CREATED_ON, ds.HospitalizationHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(21, PARM_MODIFIED_BY, ds.HospitalizationHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_MODIFIED_ON, ds.HospitalizationHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(23, PARM_SOAP_TEXT, ds.HospitalizationHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_HOSPITAL, ds.HospitalizationHx_Disease.HospitalColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(25, PARM_CPT_SNOMED_ID, ds.HospitalizationHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_CPT_SNOMED_DESCRIPTION, ds.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(27, PARM_FREE_TEXT_ICD, ds.HospitalizationHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);
        }

        //End/20-1-2016/Farooq Ahmad, Hospitalization History Disease support functions
        #endregion


        #region "Hospitalization History Lookups"
        //Start/20-1-2016/Abid Ali, Hospitalization History Lookups
        public DSHospitalizationHxLookup LookupHospitalizationHxStatus()
        {
            DSHospitalizationHxLookup ds = new DSHospitalizationHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSHospitalizationHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX__STATUS_LOOKUP, ds, ds.HospitalizationHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHistory::LookupHospitalizationHxStatus", PROC_HOSPITALIZATIONHX__STATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHospitalizationHxLookup LookupHospitalizationHxStay()
        {
            DSHospitalizationHxLookup ds = new DSHospitalizationHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSHospitalizationHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX__STAY_LOOKUP, ds, ds.HospitalizationHx_Stay.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHistory::LookupHospitalizationHxStay", PROC_HOSPITALIZATIONHX__STAY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSHospitalizationHxLookup LookupHospitalizationHxHospital()
        {
            DSHospitalizationHxLookup ds = new DSHospitalizationHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSHospitalizationHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX__HOSPITAL_LOOKUP, ds, ds.HospitalizationHx_Hospital.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHistory::LookupHospitalizationHxHospital", PROC_HOSPITALIZATIONHX__HOSPITAL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //End/20-1-2016/Abid Ali, Hospitalization History Lookups
        #endregion "Hospitalization History Lookups"

        #region "HospitalizationHx Insert/Update/Delete/Select"
        // Start 20/01/2016 Abid Ali HospitalizationHx Parent Table
        public DSHospitalizationHx InsertHospitalizationHx(DSHospitalizationHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.HospitalizationHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersHospitalizationHx(dbManager, ds, true);
                ds = (DSHospitalizationHx)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_INSERT, ds, ds.HospitalizationHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString(), null, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALInsertHospitalizationHx::InsertHospitalizationHx", PROC_HOSPITALIZATIONHX_INSERT, ex);
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
        public DSHospitalizationHx UpdateHospitalizationHx(DSHospitalizationHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.HospitalizationHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateParametersHospitalizationHx(dbManager, ds, false);
                ds = (DSHospitalizationHx)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_UPDATE, ds, ds.HospitalizationHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString(), null, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALHospitalizationHx::UpdateHospitalizationHx", PROC_HOSPITALIZATIONHX_UPDATE, ex);
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

        public string DeleteHospitalizationHx(string HospitalizationHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // dbManager.Open();
                dbManager.BeginTransaction();
                DSHospitalizationHx dsCurrentHospitalization = LoadHospitalizationHx(0, Convert.ToInt64(HospitalizationHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, HospitalizationHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentHospitalization.HospitalizationHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, HospitalizationHxId.ToString(), null, HospitalizationHxId.ToString(), false, false, true);
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

                MDVLogger.DALErrorLog("DALHospitalizationHx::DeleteHospitalizationHx", PROC_HOSPITALIZATIONHX_DELETE, ex);
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

        public HospitalizationHxModel LoadNoteHospitalizationHx(long PatientId, long UserId, long EntityId, long NoteId)
        {
            HospitalizationHxModel model = new HospitalizationHxModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, UserId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_HOSPITALIZATIONHX_SELECTForSoapText, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(HospitalizationHxModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALHospitalizationHx::LoadNoteHospitalizationHx", PROC_HOSPITALIZATIONHX_SELECTForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHospitalizationHx LoadHospitalizationHx(long PatientId, long HospitalizationHxId, string isViewHospitalizationHx = "", string isPrintHospitalizationHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSHospitalizationHx ds = new DSHospitalizationHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (HospitalizationHxId == 0)
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, HospitalizationHxId);
                ds = (DSHospitalizationHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_SELECT, ds, ds.HospitalizationHx.TableName);
                if (ds.HospitalizationHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.HospitalizationHx.Rows[0]["HospitalizationHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.HospitalizationHx;
                        if (dtTemp != null)
                        {
                            if (isViewHospitalizationHx == "1" || isPrintHospitalizationHx == "1")
                            {
                                bool isViewAction = isViewHospitalizationHx == "1" ? true : false;
                                bool isPrintAcion = isPrintHospitalizationHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString(), null, ds.HospitalizationHx.Rows[0][ds.HospitalizationHx.HospitalizationHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALHospitalizationHx::LoadHospitalizationHx", PROC_HOSPITALIZATIONHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string updateSoapTextForHospitalizationHX(long HospitalizationHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, HospitalizationHxId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_HOSPITALIZATIONHX).ToString();

                if (returnVal == "" || returnVal == "-1")
                    return "";
                else
                    throw new Exception(returnVal);


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHX::updateSoapTextForHospitalizationHX", PROC_UPDATE_SOAPTEXT_FOR_HOSPITALIZATIONHX, ex);
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



        // End 20/01/2016 Abid Ali HospitalizationHx Parent Table
        #endregion


        //Start//21/01/2016//Ahmad Raza//methods for Attach/Detach HospitalizationHx from Note
        #region Association with Notes


        public DSHospitalizationHx attachHospitalizationHxWithNotes(string hospitalizationHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSHospitalizationHx ds = new DSHospitalizationHx();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(hospitalizationHxId))
                {
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, hospitalizationHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSHospitalizationHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_HOSPITALIZATIONHX_FROM_NOTE, ds, ds.HospitalizationHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHx::attachHospitalizationHxWithNotes", PROC_ATTACH_HOSPITALIZATIONHX_FROM_NOTE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachHospitalizationHxFromNotes(long hospitalizationHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (hospitalizationHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_ID, hospitalizationHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_HOSPITALIZATIONHX_FROM_NOTE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHx::detachHospitalizationHxFromNotes", PROC_DETACH_HOSPITALIZATIONHX_FROM_NOTE, ex);
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
        //End//20/01/2016//Ahmad Raza//methods for Attach/Detach HospitalizationHx from Note


        #region Hospitalization_Disease Insert/Update/Delete/Select

        // Start 21/01/2016 Farooq Ahmad for HospitalizationHx_Disease in Clinical
        public DSHospitalizationHx LoadHospitalizationHx_Disease(long HospitalizationHxId, long DiseaseId, string isViewHospitalizationHx = "", string isPrintHospitalizationHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSHospitalizationHx ds = new DSHospitalizationHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (HospitalizationHxId == 0)
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_HOSPITALIZATION_HX_ID, HospitalizationHxId);

                if (DiseaseId == 0)
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, DiseaseId);

                ds = (DSHospitalizationHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_Disease_SELECT, ds, ds.HospitalizationHx_Disease.TableName);
                if (ds.HospitalizationHx_Disease.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.HospitalizationHx_Disease.Rows[0]["HospitalizationHxId"]) > 0 && DiseaseId > 0)
                    {
                        DataTable dtTemp = ds.HospitalizationHx_Disease;
                        if (dtTemp != null)
                        {
                            if (isViewHospitalizationHx == "1" || isPrintHospitalizationHx == "1")
                            {
                                bool isViewAction = isViewHospitalizationHx == "1" ? true : false;
                                bool isPrintAcion = isPrintHospitalizationHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.HospitalizationHx_Disease.Rows[0][ds.HospitalizationHx_Disease.DiseaseIdColumn].ToString(), null, ds.HospitalizationHx_Disease.Rows[0][ds.HospitalizationHx_Disease.HospitalizationHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALHospitalizationHx::LoadHospitalizationHx_Disease", PROC_HOSPITALIZATIONHX_Disease_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        // End 21/01/2016 Farooq Ahmad for HospitalizationHx_Disease in Clinical

        /* Start 21/01/2016 Farooq Ahmad insert update disease */
        public DSHospitalizationHx insertUpdateHospitalizationHxDisease(DSHospitalizationHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.HospitalizationHx_Disease.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateHospitalizationHxDiseaseInsertParameters(dbManager, ds);
                CreateHospitalizationHxDiseaseUpdateParameters(dbManager, ds);
                ds = (DSHospitalizationHx)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_Disease_INSERT, PROC_HOSPITALIZATIONHX_Disease_UPDATE, ds, ds.HospitalizationHx_Disease.TableName);
                //Start 05-10-2017 Humaira Yousaf EMR-4892
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");

                    int k = 0;
                    for (int i = 0; i < ds.HospitalizationHx_Disease.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > k)
                        {
                            bool isExists = false;
                            if (!ds.HospitalizationHx_Disease.Rows[i].IsNull(ds.HospitalizationHx_Disease.FreeTextICDColumn.ColumnName) &&
                                ds.HospitalizationHx_Disease.Rows[i][ds.HospitalizationHx_Disease.FreeTextICDColumn] == dtTemp.Rows[k][ds.HospitalizationHx_Disease.FreeTextICDColumn.ColumnName])
                            {
                                isExists = true;
                            }
                            if (!ds.HospitalizationHx_Disease.Rows[i].IsNull(ds.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName) &&
                                ds.HospitalizationHx_Disease.Rows[i][ds.HospitalizationHx_Disease.ICD10CodeColumn] == dtTemp.Rows[k][ds.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName])
                            {
                                isExists = true;
                            }
                            if (isExists)
                            {
                                dtTemp.Rows[k]["PrimaryKey"] = ds.HospitalizationHx_Disease.Rows[i][ds.HospitalizationHx_Disease.DiseaseIdColumn];
                                k++;
                            }
                        }
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.HospitalizationHx_Disease.Rows[0][ds.HospitalizationHx_Disease.DiseaseIdColumn].ToString(), null, ds.HospitalizationHx_Disease.Rows[0][ds.HospitalizationHx_Disease.HospitalizationHxIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                //End 05-10-2017 Humaira Yousaf EMR-4892            
                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALHospitalizationHx::insertUpdateHospitalizationHxDisease", PROC_HOSPITALIZATIONHX_Disease_INSERT + " " + PROC_HOSPITALIZATIONHX_Disease_UPDATE, ex);
                throw ex;
            }
        }
        /* End 21/01/2016 Farooq Ahmad insert update disease */


        /* Start 21/01/2016 Farooq Ahmad delete disease */
        public string deleteHospitalizationHxDisease(string DiseaseId, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSHospitalizationHx dsHospitalizationHx = LoadHospitalizationHx_Disease(0, Convert.ToInt64(DiseaseId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_HOSPITALIZATION_HX_DISEASE_ID, DiseaseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_HOSPITALIZATIONHX_Disease_DELETE).ToString();

                if (returnVal != "" && returnVal != "-1" && returnVal != "Successfully Deleted")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsHospitalizationHx.HospitalizationHx_Disease;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, DiseaseId, null, dsHospitalizationHx.HospitalizationHx_Disease.Rows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalizationHxIdColumn].ToString(), false, false, true, patientId);
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

                MDVLogger.DALErrorLog("DALHospitalizationHx::deleteHospitalizationHxDisease", PROC_HOSPITALIZATIONHX_Disease_DELETE, ex);
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
        /* End 21/01/2016 Farooq Ahmad delete disease */

        #endregion

        #region Legacy Notes

        public List<HospitalizationHx> NotesHospitalizationHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<HospitalizationHx> objList_HospitalizationHx = new List<HospitalizationHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_HOSPITALIZATIONHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        HospitalizationHx model = new HospitalizationHx();
                        var properties = typeof(HospitalizationHx).GetProperties();
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
                        objList_HospitalizationHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHospitalizationHx::NotesHospitalizationHxSelect", PROC_NOTES_HOSPITALIZATIONHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_HospitalizationHx;
        }

        #endregion Legacy Notes

    }
}
