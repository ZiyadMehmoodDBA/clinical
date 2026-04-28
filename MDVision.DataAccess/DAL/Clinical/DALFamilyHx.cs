/* Author:  Muhammad Arshad
 * Created Date: 14/01/2016
 * OverView: Created for FamilyHx in Clinical Module
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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALFamilyHx
    {
        #region Variable

        #endregion

        #region Constructors
        public DALFamilyHx()
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

        public DALFamilyHx(bool IsNative)
        {

        }

        public DALFamilyHx(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        #endregion

        #region "Stored Procedure Names"
        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Initializing Stored Procedures for FamilyHx  
        /// </summary>
        private const string PROC_FAMILYHX_INSERT = "Clinical.sp_FamilyHxInsert";
        private const string PROC_FAMILYHX_UPDATE = "Clinical.sp_FamilyHxUpdate";
        private const string PROC_FAMILYHX_DELETE = "Clinical.sp_FamilyHxDelete";
        private const string PROC_FAMILYHX_SELECT = "Clinical.sp_FamilyHxSelect";
        private const string PROC_FAMILYHX_SELECTForSoapText = "Clinical.sp_FamilyHxSelectForSoapText";
        private const string PROC_FAMILYHX_DISEASE_INSERT = "Clinical.sp_FamilyHx_DiseaseInsert";
        private const string PROC_FAMILYHX_DISEASE_UPDATE = "Clinical.sp_FamilyHx_DiseaseUpdate";
        private const string PROC_FAMILYHX_DISEASE_DELETE = "Clinical.sp_FamilyHx_DiseaseDelete";
        private const string PROC_FAMILYHX_DISEASE_SELECT = "Clinical.sp_FamilyHx_DiseaseSelect";
        private const string PROC_FAMILYHX_FAMILYMEMBER_INSERT = "Clinical.sp_FamilyHx_FamilyMemberDetailInsert";
        private const string PROC_FAMILYHX_FAMILYMEMBER_UPDATE = "Clinical.sp_FamilyHx_FamilyMemberDetailUpdate";
        private const string PROC_FAMILYHX_FAMILYMEMBER_DELETE = "Clinical.sp_FamilyHx_FamilyMemberDetailDelete";
        private const string PROC_FAMILYHX_FAMILYMEMBER_SELECT = "Clinical.sp_FamilyHx_FamilyMemberDetailSelect";
        private const string PROC_FAMILYHX_FAMILYMEMBER_LOOKUP = "Clinical.sp_FamilyHx_FamilyMemberLookup";
        private const string PROC_FAMILYHX_HEALTHSTATUS_LOOKUP = "Clinical.sp_FamilyHx_HealthStatusLookup";
        private const string PROC_ATTACH_FAMILYHX_FROM_NOTES = "Clinical.sp_AttachFamilyHxWithNotes";
        private const string PROC_DETACH_FAMILYHX_FROM_NOTES = "Clinical.sp_DetachFamilyHxFromNotes";

        private const string PROC_UPDATE_SOAPTEXT_FOR_FamilyHX = "Clinical.sp_UpdateSoapTextForFamilyHX";
        private const string PROC_FAMILYHX_DISEASE_SOAP_INSERT = "Clinical.sp_UpdateSoapTextForFamilyHX_Disease";
        private const string PROC_HXLOG_SELECT = "Clinical.sp_HXLogSelect";
        private const string PROC_FAMILYHX_MEMBER_HASDISEASE = "Clinical.sp_FamilyHx_FamilyMemberHasDisease";


        private const string PROC_NOTES_FAMILYHX_SELECT = "[Clinical].[sp_NotesFamilyHxSelect]";

        #endregion

        #region "Parameters"
        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Initializing parameters for FamilyHx  
        /// </summary>
        private const string PARM_FAMILY_HX_ID = "@FamilyHxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_FAMILY_HX_DATE = "@FamilyHxDate";
        private const string PARM_B_UNREMARKABLE = "@bUnremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAPTEXT = "@SoapText";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_DISEASE_ID = "@DiseaseId";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD9_CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10CodeDescription";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_LEXI_CODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_MEMBERDETAIL_ID = "@MemberDetailId";
        private const string PARM_MEMBER_ID = "@MemberId";
        private const string PARM_HEALTHSTATUS_ID = "@HealthStatusId";
        private const string PARM_BIRTH_YEAR = "@BirthYear";
        private const string PARM_IS_RELATIVE_DIED = "@IsRelativeDied";
        private const string PARM_AGE_AT_DEATH = "@AgeAtDeath";
        private const string PARM_AGE_AT_DIAGNOSIS = "@AgeAtDiagnosis";

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_FAMILYMEMBER_ID = "@FamilyMemberId";
        private const string PARM_FREETEXT_ICD = "@FreeTextICD";

        private const string PARM_HX_ID = "@HxId";
        private const string PARM_HX_TYPE = "@HxType";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string NOTE_ID = "@NoteId";


        private const string PARAM_TEMP_DISEASES_ID = "@tempDiseaseId";
        private const string PARAM_TEMP_ICD_ID = "@TempICDID";
        private const string PARAM_ADD_FROM_MOBILE = "@AddFromMobile";

        #endregion

        /* Start 19/01/2016 Muhammad Irfan supporting functions for FamilyHx */
        #region "Support Functions FamilyHx"

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating parameters for FamilyHx  
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersFamilyHx(IDBManager dbManager, DSFamilyHx ds, Boolean IsInsert)
        {

            if (IsInsert == true)
            {
                dbManager.CreateParameters(12);
                dbManager.AddParameters(0, PARM_FAMILY_HX_ID, ds.FamilyHx.FamilyHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(11);
                dbManager.AddParameters(0, PARM_FAMILY_HX_ID, ds.FamilyHx.FamilyHxIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.FamilyHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FAMILY_HX_DATE, ds.FamilyHx.FamilyHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_B_UNREMARKABLE, ds.FamilyHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.FamilyHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.FamilyHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.FamilyHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.FamilyHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.FamilyHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.FamilyHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAPTEXT, ds.FamilyHx.SoapTextColumn.ColumnName, DbType.String);
            if (IsInsert == true)
            {
                dbManager.AddParameters(11, NOTE_ID, ds.FamilyHx.NoteIdColumn.ColumnName, DbType.Int64);                
            }
        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Insert parameters for FamilyHx  
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxInsertParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateInsertParameters(11);

            dbManager.AddInsertUpdateParameters(0, PARM_FAMILY_HX_ID, ds.FamilyHx.FamilyHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.FamilyHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_FAMILY_HX_DATE, ds.FamilyHx.FamilyHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.FamilyHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.FamilyHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.FamilyHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.FamilyHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.FamilyHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.FamilyHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.FamilyHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT, ds.FamilyHx.SoapTextColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Update  parameters for FamilyHx  
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxUpdateParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateUpdateParameters(11);

            dbManager.AddParameters(0, PARM_FAMILY_HX_ID, ds.FamilyHx.FamilyHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.FamilyHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_FAMILY_HX_DATE, ds.FamilyHx.FamilyHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.FamilyHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.FamilyHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.FamilyHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.FamilyHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.FamilyHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.FamilyHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.FamilyHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT, ds.FamilyHx.SoapTextColumn.ColumnName, DbType.String);
        }
        #endregion
        /* End 19/01/2016 Muhammad Irfan supporting functions for FamilyHx */


        /* Start 19/01/2016 Muhammad Irfan supporting functions for FamilyHx Disease */
        #region"Supporting Functions FamilyHx Disease"

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating parameters for FamilyHx Disease 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersFamilyHxDisease(IDBManager dbManager, DSFamilyHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(17);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_DISEASE_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DISEASE_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_FAMILY_HX_ID, ds.FamilyHx_Disease.FamilyHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD9_CODE, ds.FamilyHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10_CODE, ds.FamilyHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMED_ID, ds.FamilyHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SNOMED_DESCRIPTION, ds.FamilyHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_LEXI_CODE, ds.FamilyHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.FamilyHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_COMMENTS, ds.FamilyHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.FamilyHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.FamilyHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.FamilyHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.FamilyHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.FamilyHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_SOAPTEXT, ds.FamilyHx_Disease.SoapTextColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Insert parameters for FamilyHx Disease 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxDiseaseInsertParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateInsertParameters(22);

            dbManager.AddInsertUpdateParameters(0, PARM_DISEASE_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_FAMILY_HX_ID, ds.FamilyHx_Disease.FamilyHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.FamilyHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.FamilyHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.FamilyHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.FamilyHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.FamilyHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.FamilyHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_COMMENTS, ds.FamilyHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_IS_ACTIVE, ds.FamilyHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_CREATED_BY, ds.FamilyHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_ON, ds.FamilyHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_MODIFIED_BY, ds.FamilyHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_ON, ds.FamilyHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_SOAPTEXT, ds.FamilyHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_FAMILYMEMBER_ID, ds.FamilyHx_Disease.FamilyMemberIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_FREETEXT_ICD, ds.FamilyHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(19, PARAM_TEMP_ICD_ID, ds.FamilyHx_Disease.TempICDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARAM_ADD_FROM_MOBILE, ds.FamilyHx_Disease.AddedFromMobileAppColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(21, PARAM_TEMP_DISEASES_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.String);

          
    }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  Creating parameters for FamilyHx disease soap text Insert 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxDiseaseInsertSoapParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateInsertParameters(1);

            dbManager.AddInsertUpdateParameters(0, PARM_DISEASE_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Update parameters for FamilyHx Disease 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxDiseaseUpdateParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateUpdateParameters(19);

            dbManager.AddInsertUpdateParameters(0, PARM_DISEASE_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_FAMILY_HX_ID, ds.FamilyHx_Disease.FamilyHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9_CODE, ds.FamilyHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICD10_CODE, ds.FamilyHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.FamilyHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMED_ID, ds.FamilyHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_SNOMED_DESCRIPTION, ds.FamilyHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXI_CODE, ds.FamilyHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.FamilyHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_COMMENTS, ds.FamilyHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_IS_ACTIVE, ds.FamilyHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(12, PARM_CREATED_BY, ds.FamilyHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_ON, ds.FamilyHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_MODIFIED_BY, ds.FamilyHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_ON, ds.FamilyHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_SOAPTEXT, ds.FamilyHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_FAMILYMEMBER_ID, ds.FamilyHx_Disease.FamilyMemberIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(18, PARM_FREETEXT_ICD, ds.FamilyHx_Disease.FreeTextICDColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(19, PARAM_TEMP_ICD_ID, ds.FamilyHx_Disease.TempICDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARAM_ADD_FROM_MOBILE, ds.FamilyHx_Disease.AddedFromMobileAppColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(21, PARAM_TEMP_DISEASES_ID, ds.FamilyHx_Disease.DiseaseIdColumn.ColumnName, DbType.String);

        }
        #endregion
        /* End 19/01/2016 Muhammad Irfan supporting functions for FamilyHx Disease */


        /* Start 20/01/2016 Muhammad Irfan supporting functions for FamilyHx_FamilyMember Disease */
        #region"Supporting Functions FamilyHx FamilyMember"

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating parameters for FamilyHx Family Member 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersFamilyHxFamilyMember(IDBManager dbManager, DSFamilyHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(15);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEMBERDETAIL_ID, ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEMBERDETAIL_ID, ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_DISEASE_ID, ds.FamilyHx_FamilyMemberDetail.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MEMBER_ID, ds.FamilyHx_FamilyMemberDetail.MemberIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(3, PARM_HEALTHSTATUS_ID, ds.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_BIRTH_YEAR, ds.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_RELATIVE_DIED, ds.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_AGE_AT_DEATH, ds.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_AGE_AT_DIAGNOSIS, ds.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.FamilyHx_FamilyMemberDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.FamilyHx_FamilyMemberDetail.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.FamilyHx_FamilyMemberDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.FamilyHx_FamilyMemberDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.FamilyHx_FamilyMemberDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.FamilyHx_FamilyMemberDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_SOAPTEXT, ds.FamilyHx_FamilyMemberDetail.SoapTextColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Insert parameters for FamilyHx Family Member 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxFamilyMemberInsertParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateInsertParameters(15);

            dbManager.AddInsertUpdateParameters(0, PARM_MEMBERDETAIL_ID, ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_DISEASE_ID, ds.FamilyHx_FamilyMemberDetail.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_MEMBER_ID, ds.FamilyHx_FamilyMemberDetail.MemberIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_HEALTHSTATUS_ID, ds.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_BIRTH_YEAR, ds.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_RELATIVE_DIED, ds.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_AGE_AT_DEATH, ds.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_AGE_AT_DIAGNOSIS, ds.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_COMMENTS, ds.FamilyHx_FamilyMemberDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_IS_ACTIVE, ds.FamilyHx_FamilyMemberDetail.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_CREATED_BY, ds.FamilyHx_FamilyMemberDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CREATED_ON, ds.FamilyHx_FamilyMemberDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIED_BY, ds.FamilyHx_FamilyMemberDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_MODIFIED_ON, ds.FamilyHx_FamilyMemberDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_SOAPTEXT, ds.FamilyHx_FamilyMemberDetail.SoapTextColumn.ColumnName, DbType.String);
            //dbManager.AddInsertUpdateParameters(16, PARM_PATIENT_ID, ds.FamilyHx_FamilyMemberDetail.PatientIdColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  Creating Update parameters for FamilyHx Family Member 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        private void CreateFamilyHxFamilyMemberUpdateParameters(IDBManager dbManager, DSFamilyHx ds)
        {
            dbManager.CreateUpdateParameters(15);

            dbManager.AddInsertUpdateParameters(0, PARM_MEMBERDETAIL_ID, ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_DISEASE_ID, ds.FamilyHx_FamilyMemberDetail.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_MEMBER_ID, ds.FamilyHx_FamilyMemberDetail.MemberIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_HEALTHSTATUS_ID, ds.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(4, PARM_BIRTH_YEAR, ds.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_RELATIVE_DIED, ds.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_AGE_AT_DEATH, ds.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_AGE_AT_DIAGNOSIS, ds.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_COMMENTS, ds.FamilyHx_FamilyMemberDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_IS_ACTIVE, ds.FamilyHx_FamilyMemberDetail.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(10, PARM_CREATED_BY, ds.FamilyHx_FamilyMemberDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_CREATED_ON, ds.FamilyHx_FamilyMemberDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIED_BY, ds.FamilyHx_FamilyMemberDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_MODIFIED_ON, ds.FamilyHx_FamilyMemberDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(14, PARM_SOAPTEXT, ds.FamilyHx_FamilyMemberDetail.SoapTextColumn.ColumnName, DbType.String);
            //dbManager.AddInsertUpdateParameters(15, PARM_PATIENT_ID, ds.FamilyHx_FamilyMemberDetail.PatientIdColumn.ColumnName, DbType.String);

        }
        #endregion
        /* End 20/01/2016 Muhammad Irfan supporting functions for FamilyHx_FamilyMember Disease */


        /* Start 19/01/2016 Muhammad Irfan DAL functions for FamilyHx insert/update/delete/select */
        #region "FamilyHx Insert/Update/Delete/Select"
        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will insert Family hx 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFamilyHx InsertFamilyHx(DSFamilyHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.FamilyHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersFamilyHx(dbManager, ds, true);
                ds = (DSFamilyHx)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_INSERT, ds, ds.FamilyHx.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FamilyHx.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FamilyHx.Rows[i][ds.FamilyHx.FamilyHxIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), null, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALFamilyHx::InsertFamilyHx", PROC_FAMILYHX_INSERT, ex);
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
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will update Family hx 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFamilyHx UpdateFamilyHx(DSFamilyHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.FamilyHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateParametersFamilyHx(dbManager, ds, false);
                ds = (DSFamilyHx)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_UPDATE, ds, ds.FamilyHx.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FamilyHx.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FamilyHx.Rows[i][ds.FamilyHx.FamilyHxIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), null, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALFamilyHx::UpdateFamilyHx", PROC_FAMILYHX_UPDATE, ex);
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
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will delete Family hx 
        /// </summary>
        /// <param name="FamilyHxId"></param>
        /// <returns></returns>
        public string DeleteFamilyHx(string FamilyHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSFamilyHx dsCurrentFamilyHx = LoadFamilyHx(0, Convert.ToInt64(FamilyHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAMILY_HX_ID, FamilyHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAMILYHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    DataTable dtTemp = dsCurrentFamilyHx.FamilyHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, FamilyHxId.ToString(), null, FamilyHxId.ToString(), false, false, true);
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

                MDVLogger.DALErrorLog("DALFamilyHx::DeleteFamilyHx", PROC_FAMILYHX_DELETE, ex);
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

        public FamilyHxSoapModel LoadNoteFamilyHx(long PatientId, long UserId, long EntityId, long NoteId)
        {
            FamilyHxSoapModel model = new FamilyHxSoapModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, UserId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_FAMILYHX_SELECTForSoapText, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(FamilyHxSoapModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALFamilyHx::LoadNoteFamilyHx", PROC_FAMILYHX_SELECTForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will load Family hx 
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="FamilyHxId"></param>
        /// <returns></returns>
        public DSFamilyHx LoadFamilyHx(long PatientId, long FamilyHxId, string isViewFamilyHx = "", string isPrintFamilyHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (FamilyHxId == 0)
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, FamilyHxId);
                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_SELECT, ds, ds.FamilyHx.TableName);
                if (ds.FamilyHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.FamilyHx.Rows[0]["FamilyHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.FamilyHx;
                        if (dtTemp != null)
                        {
                            if (isViewFamilyHx == "1" || isPrintFamilyHx == "1")
                            {
                                bool isViewAction = isViewFamilyHx == "1" ? true : false;
                                bool isPrintAcion = isPrintFamilyHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), null, ds.FamilyHx.Rows[0][ds.FamilyHx.FamilyHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHx", PROC_FAMILYHX_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        /* End 19/01/2016 Muhammad Irfan DAL functions for FamilyHx insert/update/delete/select */


        #region FamilyHx_Disease Insert/Update/Delete/Select
        /* Start 19/01/2016 Muhammad Irfan DAL functions for FamilyHx Disease insert/update/delete/select */
        public DSFamilyHx LoadFamilyHx_Disease(long FamilyHxId, long DiseaseId, int FamilyMemberId, string isView = "", string isPrint = "")
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (FamilyHxId == 0)
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAMILY_HX_ID, FamilyHxId);

                if (DiseaseId == 0)
                    dbManager.AddParameters(0, PARM_DISEASE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DISEASE_ID, DiseaseId);

                if (FamilyMemberId == 0)
                    dbManager.AddParameters(2, PARM_FAMILYMEMBER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_FAMILYMEMBER_ID, FamilyMemberId);

                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_DISEASE_SELECT, ds, ds.FamilyHx_Disease.TableName);
                if (ds.FamilyHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.FamilyHx.Rows[0]["FamilyHxId"]) > 0 && DiseaseId > 0)
                    {

                        DataTable dtTemp = ds.FamilyHx_Disease;
                        if (dtTemp != null)
                        {
                            if (isView == "1" || isPrint == "1")
                            {
                                bool isViewAction = isView == "1" ? true : false;
                                bool isPrintAcion = isPrint == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx_Disease.Rows[0][ds.FamilyHx_Disease.DiseaseIdColumn].ToString(), null, ds.FamilyHx_Disease.Rows[0][ds.FamilyHx_Disease.FamilyHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHx_Disease", PROC_FAMILYHX_DISEASE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFamilyHx insertUpdateFamilyHxDisease(DSFamilyHx ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.FamilyHx_Disease.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateFamilyHxDiseaseInsertParameters(dbManager, ds);
                CreateFamilyHxDiseaseUpdateParameters(dbManager, ds);
                ds = (DSFamilyHx)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_DISEASE_INSERT, PROC_FAMILYHX_DISEASE_UPDATE, ds, ds.FamilyHx_Disease.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");

                    int k = 0;
                    for (int i = 0; i < ds.FamilyHx_Disease.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > k)
                        {
                            bool isExists = false;
                            if (!ds.FamilyHx_Disease.Rows[i].IsNull("FreeTextICD") &&
                                ds.FamilyHx_Disease.Rows[i][ds.FamilyHx_Disease.FreeTextICDColumn] == dtTemp.Rows[k]["FreeTextICD"])
                            {
                                isExists = true;
                            }
                            if (!ds.FamilyHx_Disease.Rows[i].IsNull("ICD10Code") &&
                                ds.FamilyHx_Disease.Rows[i][ds.FamilyHx_Disease.ICD10CodeColumn] == dtTemp.Rows[k]["ICD10Code"])
                            {
                                isExists = true;
                            }
                            if (isExists)
                            {
                                dtTemp.Rows[k]["PrimaryKey"] = ds.FamilyHx_Disease.Rows[i][ds.FamilyHx_Disease.DiseaseIdColumn];
                                k++;
                            }

                        }

                    }
                    //for (int i = 0; i < ds.FamilyHx_Disease.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.FamilyHx_Disease.Rows[i][ds.FamilyHx_Disease.DiseaseIdColumn];
                    //}
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx_Disease.Rows[0][ds.FamilyHx_Disease.DiseaseIdColumn].ToString(), null, ds.FamilyHx_Disease.Rows[0][ds.FamilyHx_Disease.FamilyHxIdColumn].ToString(), false, false, false, patientId);
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

                MDVLogger.DALErrorLog("DALFamilyHx::insertUpdateFamilyHxDisease", PROC_FAMILYHX_DISEASE_INSERT + " " + PROC_FAMILYHX_DISEASE_UPDATE, ex);
                throw ex;
            }
        }

        //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
        public DSFamilyHx insertUpdateSoapTextForFamilyHxDisease(long diseaseId)
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (diseaseId == 0)
                    dbManager.AddParameters(0, PARM_DISEASE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DISEASE_ID, diseaseId);

                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_DISEASE_SOAP_INSERT, ds, ds.FamilyHx_Disease.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::insertUpdateSoapTextForFamilyHxDisease", PROC_FAMILYHX_DISEASE_SOAP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
        public string deleteFamilyHxDisease(string DiseaseId, string familyId, int familyMemberId, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSFamilyHx dsFamilyHx = LoadFamilyHx_Disease(Convert.ToInt64(familyId), Convert.ToInt64(DiseaseId), familyMemberId);

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_DISEASE_ID, DiseaseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.AddParameters(2, PARM_FAMILYMEMBER_ID, familyMemberId);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAMILYHX_DISEASE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsFamilyHx.FamilyHx_Disease;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, DiseaseId, null, dsFamilyHx.FamilyHx_Disease.Rows[0][dsFamilyHx.FamilyHx_Disease.FamilyHxIdColumn].ToString(), false, false, true, patientId);
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
                MDVLogger.DALErrorLog("DALFamilyHx::deleteFamilyHxDisease", PROC_FAMILYHX_DISEASE_DELETE, ex);
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

        /* End 19/01/2016 Muhammad Irfan DAL functions for FamilyHx Disease insert/update/delete/select */
        #endregion

        #region FamilyHx_FamilyMember Insert/Update/Delete/Select
        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    27/01/2016
        /// Reason:  This function will load Family Members Detail 
        /// </summary>
        /// <param name="MemberDetailId"></param>
        /// <param name="MemberId"></param>
        /// <param name="DiseaseId"></param>
        /// <returns></returns>
        public DSFamilyHx LoadFamilyHx_FamilyMemberDetail(long MemberDetailId, long MemberId, long DiseaseId, string familyHxId = "", string isView = "", string isPrint = "", string patientId = "")
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);

                if (MemberDetailId == 0)
                    dbManager.AddParameters(0, PARM_MEMBERDETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MEMBERDETAIL_ID, MemberDetailId);

                if (MemberId == 0)
                    dbManager.AddParameters(1, PARM_MEMBER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEMBER_ID, MemberId);

                if (DiseaseId == 0)
                    dbManager.AddParameters(2, PARM_DISEASE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_DISEASE_ID, DiseaseId);
                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_SELECT, ds, ds.FamilyHx_FamilyMemberDetail.TableName);

                //Start 31-10-2016 Humaira Yousaf for dbaudit
                if (ds.FamilyHx_FamilyMemberDetail.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.FamilyHx_FamilyMemberDetail.Rows[0]["MemberId"]) > 0 && familyHxId != "")
                    {
                        DataTable dtTemp = ds.FamilyHx_FamilyMemberDetail;
                        if (dtTemp != null)
                        {
                            if (isView == "1" || isPrint == "1")
                            {
                                bool isViewAction = isView == "1" ? true : false;
                                bool isPrintAcion = isPrint == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx_FamilyMemberDetail.Rows[0][ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn].ToString(), null, familyHxId, isViewAction, isPrintAcion, false, patientId);
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
                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHx_FamilyMemberDetail", PROC_FAMILYHX_FAMILYMEMBER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will insert/update Family Members Detail 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFamilyHx insertUpdateFamilyHx_FamilyMemberDetail(DSFamilyHx ds, Int64 patientId, Int64 familyHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.FamilyHx_FamilyMemberDetail.GetChanges();
                dtTemp.Columns.Add("PatientId");
                if (dtTemp.Rows.Count > 0)
                {
                    dtTemp.Rows[0]["PatientId"] = patientId;
                }
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateFamilyHxFamilyMemberInsertParameters(dbManager, ds);
                CreateFamilyHxFamilyMemberUpdateParameters(dbManager, ds);
                ds = (DSFamilyHx)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_INSERT, PROC_FAMILYHX_FAMILYMEMBER_UPDATE, ds, ds.FamilyHx_FamilyMemberDetail.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FamilyHx_FamilyMemberDetail.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FamilyHx_FamilyMemberDetail.Rows[i][ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FamilyHx_FamilyMemberDetail.Rows[0][ds.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn].ToString(), null, familyHxId.ToString(), false, false, false, Convert.ToString(patientId));
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

                MDVLogger.DALErrorLog("DALFamilyHx::insertUpdateFamilyHx_FamilyMemberDetail", PROC_FAMILYHX_FAMILYMEMBER_INSERT + " " + PROC_FAMILYHX_FAMILYMEMBER_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    27/01/2016
        /// Reason:  This function will delete of  Family Members Detail 
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="DiseaseId"></param>
        /// <returns></returns>
        public string deleteFamilyHx_FamilyMemberDetail(string MemberId, string DiseaseId, long patientId, long familyhxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.BeginTransaction();

                DSFamilyHx dsFamilyHx = LoadFamilyHx_FamilyMemberDetail(Convert.ToInt64(MemberId), 0, Convert.ToInt64(DiseaseId));
                DataTable dtTemp = dsFamilyHx.FamilyHx_FamilyMemberDetail;

                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_MEMBER_ID, MemberId);
                dbManager.AddParameters(1, PARM_DISEASE_ID, DiseaseId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                else
                {
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, MemberId, null, familyhxId.ToString(), false, false, true, patientId.ToString());
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

                MDVLogger.DALErrorLog("DALFamilyHx::deleteFamilyHx_FamilyMemberDetail", PROC_FAMILYHX_FAMILYMEMBER_DELETE, ex);
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

        #region"FamilyHx Lookups"
        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will load Family Members 
        /// </summary>
        /// <returns></returns>
        public DSFamilyHxLookup LookupFamilyHx_FamilyMember()
        {
            DSFamilyHxLookup ds = new DSFamilyHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFamilyHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_LOOKUP, ds, ds.FamilyHx_FamilyMember.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LookupFamilyHx_FamilyMember", PROC_FAMILYHX_FAMILYMEMBER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Date:    20/01/2016
        /// Reason:  This function will load Health Status 
        /// </summary>
        /// <returns></returns>
        public DSFamilyHxLookup LookupFamilyHx_HealthStatus()
        {
            DSFamilyHxLookup ds = new DSFamilyHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFamilyHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_HEALTHSTATUS_LOOKUP, ds, ds.FamilyHx_HealthStatus.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LookupFamilyHx_HealthStatus", PROC_FAMILYHX_HEALTHSTATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region SoapText
        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will update soap text of  Familyhx  
        /// </summary>
        /// <param name="FamilyHxId"></param>
        /// <returns></returns>
        public string updateSoapTextForFamilyHX(long FamilyHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAMILY_HX_ID, FamilyHxId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_FamilyHX).ToString();

                if (returnVal != "" && returnVal != "-1")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::updateSoapTextForFamilyHX", PROC_UPDATE_SOAPTEXT_FOR_FamilyHX, ex);
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

        //Start//20/01/2016//Ahmad Raza//methods for Attach/Detach FamilyHx from Note
        #region Association with Notes

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will handle attach of  Familyhx with Note 
        /// </summary>
        /// <param name="familyHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSFamilyHx attachFamilyHxWithNotes(string familyHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSFamilyHx ds = new DSFamilyHx();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(familyHxId))
                {
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, familyHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_FAMILYHX_FROM_NOTES, ds, ds.FamilyHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::attachFamilyHxWithNotes", PROC_ATTACH_FAMILYHX_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will handle detach of Familyhx from Note 
        /// </summary>
        /// <param name="familyHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachFamilyHxFromNotes(long familyHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (familyHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, familyHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_FAMILYHX_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::detachFamilyHxFromNotes", PROC_DETACH_FAMILYHX_FROM_NOTES, ex);
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
        //End//20/01/2016//Ahmad Raza//methods for Attach/Detach FamilyHx from Note

        #region "Native Functions"
        public List<HistoryLookupModel> LookupFamilyHxFamilyMemberNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.FamilyHxFamilyMemberId = !String.IsNullOrEmpty(reader["FamilyMemberId"].ToString()) ? reader["FamilyMemberId"].ToString() : "";
                    model.FamilyHxFamilyMemberDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LookupFamilyHxFamilyMemberNative", PROC_FAMILYHX_FAMILYMEMBER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<HistoryLookupModel> LookupFamilyHxFamilyStatusNative()
        {
            List<HistoryLookupModel> HistoryLookupList = new List<HistoryLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAMILYHX_HEALTHSTATUS_LOOKUP);
                HistoryLookupModel model = null;
                while (reader.Read())
                {
                    model = new HistoryLookupModel();
                    model.FamilyHxHealthStatusId = !String.IsNullOrEmpty(reader["HealthStatusId"].ToString()) ? reader["HealthStatusId"].ToString() : "";
                    model.FamilyHxHealthStatusDescription = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LookupFamilyHxFamilyStatusNative", PROC_FAMILYHX_HEALTHSTATUS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<FamilyHxDiseaseModel> LoadFamilyHxDiseaseNative(long PatientId, long FamilyHxId = 0)
        {
            DSFamilyHx ds = new DSFamilyHx();
            List<FamilyHxDiseaseModel> HistoryList = new List<FamilyHxDiseaseModel>();
            string ConnectionString = "";
            string ConnectionString2 = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            IDBManager dbManager_2 = ClientConfiguration.GetCustomerDBManager(ref ConnectionString2, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_SELECT, ds, ds.FamilyHx.TableName);
                if (ds.FamilyHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.FamilyHx.Rows[0]["FamilyHxId"]) > 0)
                    {
                        Int64 FamHxId = Convert.ToInt64(ds.FamilyHx.Rows[0]["FamilyHxId"]);

                        dbManager_2.Open();
                        dbManager_2.CreateParameters(2);
                        dbManager_2.AddParameters(1, PARM_FAMILY_HX_ID, FamHxId);
                        dbManager_2.AddParameters(0, PARM_DISEASE_ID, null);

                        reader = (SqlDataReader)dbManager_2.ExecuteReader(CommandType.StoredProcedure, PROC_FAMILYHX_DISEASE_SELECT);
                        FamilyHxDiseaseModel model = null;
                        while (reader.Read())
                        {
                            model = new FamilyHxDiseaseModel();
                            model.DiseaseId = !String.IsNullOrEmpty(reader["DiseaseId"].ToString()) ? reader["DiseaseId"].ToString() : "";
                            model.FamilyHxId = !String.IsNullOrEmpty(reader["FamilyHxId"].ToString()) ? reader["FamilyHxId"].ToString() : "";
                            model.ICD9Code = !String.IsNullOrEmpty(reader["ICD9Code"].ToString()) ? reader["ICD9Code"].ToString() : "";
                            model.ICD9CodeDescription = !String.IsNullOrEmpty(reader["ICD9CodeDescription"].ToString()) ? reader["ICD9CodeDescription"].ToString() : "";
                            model.ICD10Code = !String.IsNullOrEmpty(reader["ICD10Code"].ToString()) ? reader["ICD10Code"].ToString() : "";
                            model.ICD10CodeDescription = !String.IsNullOrEmpty(reader["ICD10CodeDescription"].ToString()) ? reader["ICD10CodeDescription"].ToString() : "";
                            model.SNOMEDID = !String.IsNullOrEmpty(reader["SNOMEDID"].ToString()) ? reader["SNOMEDID"].ToString() : "";
                            model.SNOMEDDescription = !String.IsNullOrEmpty(reader["SNOMEDDescription"].ToString()) ? reader["SNOMEDDescription"].ToString() : "";
                            model.LexiCode = !String.IsNullOrEmpty(reader["LexiCode"].ToString()) ? reader["LexiCode"].ToString() : "";
                            model.LexiCodeDescription = !String.IsNullOrEmpty(reader["LexiCodeDescription"].ToString()) ? reader["LexiCodeDescription"].ToString() : "";

                            HistoryList.Add(model);
                        }
                        return HistoryList;
                    }
                }

                return HistoryList;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHistory::LoadFamilyHxDiseaseNative", PROC_FAMILYHX_SELECT, ex);
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

        public List<FamilyHxModel> LoadFamilyHxFamilyMemberNative(long DiseaseId)
        {
            List<FamilyHxModel> HistoryLookupList = new List<FamilyHxModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_MEMBERDETAIL_ID, null);
                dbManager.AddParameters(1, PARM_MEMBER_ID, null);
                dbManager.AddParameters(2, PARM_DISEASE_ID, DiseaseId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_FAMILYHX_FAMILYMEMBER_SELECT);
                FamilyHxModel model = null;
                while (reader.Read())
                {
                    model = new FamilyHxModel();
                    model.MemberId = !String.IsNullOrEmpty(reader["MemberId"].ToString()) ? reader["MemberId"].ToString() : "";
                    model.HealthStatusId = !String.IsNullOrEmpty(reader["HealthStatusId"].ToString()) ? reader["HealthStatusId"].ToString() : "";
                    model.DiseaseId = !String.IsNullOrEmpty(reader["DiseaseId"].ToString()) ? reader["DiseaseId"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHxFamilyMemberNative", PROC_FAMILYHX_FAMILYMEMBER_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<FamilyHxModel> LoadFamilyHxLogNative(long socialHxId)
        {
            List<FamilyHxModel> HistoryLookupList = new List<FamilyHxModel>();
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

                dbManager.AddParameters(1, PARM_HX_TYPE, "FamilyHx");

                dbManager.AddParameters(2, PARM_LOG_TYPE, "Current");

                dbManager.AddParameters(3, PARM_PAGE_NUMBER, 1);


                dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, 500);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ParamDirection.Output);

                //--------------
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_HXLOG_SELECT);
                FamilyHxModel model = null;
                while (reader.Read())
                {
                    model = new FamilyHxModel();
                    model.SoapText = !String.IsNullOrEmpty(reader["SoapText"].ToString()) ? reader["SoapText"].ToString() : "";
                    model.Action = !String.IsNullOrEmpty(reader["Action"].ToString()) ? reader["Action"].ToString() : "";
                    HistoryLookupList.Add(model);
                }
                return HistoryLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHxLogNative", PROC_HXLOG_SELECT, ex);
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

        /// <summary>
        /// Author:  Humaira Yousaf
        /// Date:   04-11-2016
        /// Reason:  Returns information if family member has disease or not
        /// </summary>
        /// <param name="familyHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSFamilyHx FamilyHxMemberHasDisease(long FamilyHxId)
        {
            DSFamilyHx ds = new DSFamilyHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (FamilyHxId == 0)
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAMILY_HX_ID, FamilyHxId);

                ds = (DSFamilyHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAMILYHX_MEMBER_HASDISEASE, ds, ds.FamilyHx_FamilyMemberHasDisease.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALFamilyHx::LoadFamilyHx_Disease", PROC_FAMILYHX_DISEASE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region Legacy Notes

        public List<FamilyHx> NotesFamilyHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<FamilyHx> objList_FamilyHx = new List<FamilyHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_FAMILYHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        FamilyHx model = new FamilyHx();
                        var properties = typeof(FamilyHx).GetProperties();
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
                        objList_FamilyHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFamilyHx::NotesFamilyHxSelect", PROC_NOTES_FAMILYHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_FamilyHx;
        }

        #endregion Legacy Notes

    }
}
