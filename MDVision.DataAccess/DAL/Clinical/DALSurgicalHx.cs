/* Author:  Muhammad Arshad
 * Created Date: 14/01/2016
 * OverView: Created for SurgicalHx in Clinical Module
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
using MDVision.Model.Clinical.Notes;
using System.Data.SqlClient;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALSurgicalHx
    {
        #region Variable

        #endregion

        //Start//20/01/2016//Ahmad Raza//Declaring Procedure names
        #region Stored Procedure Names
        private const string PROC_SURGICALHX__STATUS_LOOKUP = "Clinical.sp_SurgicalHx_StatusLookup";
        private const string PROC_SURGICALHX__LOCATION_LOOKUP = "Clinical.sp_SurgicalHx_LocationLookup";

        private const string PROC_SURGICALHX_INSERT = "Clinical.sp_SurgicalHxInsert";
        private const string PROC_SURGICALHX_UPDATE = "Clinical.sp_SurgicalHxUpdate";
        private const string PROC_SURGICALHX_DELETE = "Clinical.sp_SurgicalHxDelete";
        private const string PROC_SURGICALHX_SELECT = "Clinical.sp_SurgicalHxSelect";
        private const string PROC_SURGICALHX_SELECT_ForSoapText = "Clinical.sp_SurgicalHxSelectForSoapText";

        private const string PROC_SURGICALHX_DISEASE_SELECT = "Clinical.sp_SurgicalHx_DiseaseSelect";
        private const string PROC_SURGICALHX_DISEASE_DELETE = "Clinical.sp_SurgicalHx_DiseaseDelete";
        private const string PROC_SURGICALHX_DISEASE_INSERT = "Clinical.sp_SurgicalHx_DiseaseInsert";
        private const string PROC_SURGICALHX_DISEASE_UPDATE = "Clinical.sp_SurgicalHx_DiseaseUpdate";


        private const string PROC_DETACH_SURGICALHX_FROM_NOTES = "Clinical.sp_DetachSurgicalHxFromNotes";
        private const string PROC_ATTACH_SURGICALHX_FROM_NOTES = "Clinical.sp_AttachSurgicalHxWithNotes";
        private const string PROC_UPDATE_SOAPTEXT_FOR_SURGICALHX = "Clinical.sp_UpdateSoapTextForSurgicalHX";

        private const string PROC_NOTES_SURGICALHX_SELECT = "[Clinical].[sp_NotesSurgicalHxSelect]";
        #endregion
        //End//20/01/2016//Ahmad Raza//Declaring Procedure names

        //Start//20/01/2016//Ahmad Raza//Declaring Parameter names
        #region "Parameters"
        private const string PARM_SURGICAL_HX_DISEASE_ID = "@DiseaseId";
        private const string PARM_SURGICAL_HX_ID = "@SurgicalHxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_SURGICAL_HX_DATE = "@SurgicalHxDate";
        private const string PARM_B_UNREMARKABLE = "@bUnremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_SOAPTEXT = "@SoapText";
        private const string PARM_DISEASE_ID = "@DiseaseId";
        private const string PARM_ICD9_CODE = "@ICD9Code";
        private const string PARM_ICD9_CODE_DESCRIPTION = "@ICD9CodeDescription";
        private const string PARM_ICD10_CODE = "@ICD10Code";
        private const string PARM_ICD10_CODE_DESCRIPTION = "@ICD10CodeDescription";
        private const string PARM_SNOMED_ID = "@SNOMEDID";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_LEXI_CODE_DESCRIPTION = "@LexiCodeDescription";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_LOCATION = "@Location";
        private const string PARM_SURGERY_DATE = "@SurgeryDate";
        private const string PARM_AGE_AT_SURGERY = "@AgeAtSurgery";
        private const string PARM_SURGERY_REASON = "@SurgeryReason";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_CODE_DESCRIPTION = "@CPTCodeDescription";
        private const string PARM_ORDERING_PROVIDER_ID = "@OrderingProviderId";
        private const string PARM_PERFORMING_PROVIDER_ID = "@PerformingProviderId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_CPT_SNOMED_ID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMED_DESCRIPTION = "@CPTSNOMEDDescription";
        private const string PARM_FREETEXTCPT = "@FreeTextCPT";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";

        #endregion
        //End//20/01/2016//Ahmad Raza//Declaring Parameter names

        //Start//20/01/2016//Ahmad Raza//Class Constructor
        #region Constructors

        public DALSurgicalHx()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALSurgicalHx(SharedVariable SharedVariable)
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
        //End//20/01/2016//Ahmad Raza//Class Constructor

        //Start//20/01/2016//Ahmad Raza//Create Parameter methods for SurgicalHx insert,Update
        #region "Support Functions SurgicalHx"
        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create parameters for surgical Hx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateParametersSurgicalHx(IDBManager dbManager, DSSurgicalHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, ds.SurgicalHx.SurgicalHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, ds.SurgicalHx.SurgicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.SurgicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SURGICAL_HX_DATE, ds.SurgicalHx.SurgicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_B_UNREMARKABLE, ds.SurgicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.SurgicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.SurgicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.SurgicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.SurgicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.SurgicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.SurgicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAPTEXT, ds.SurgicalHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NOTE_ID, ds.SurgicalHx.NotesIdColumn.ColumnName, DbType.Int64);
        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create/Insert parameters for surgical Hx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateSurgicalHxInsertParameters(IDBManager dbManager, DSSurgicalHx ds)
        {
            dbManager.CreateInsertParameters(11);

            dbManager.AddInsertUpdateParameters(0, PARM_SURGICAL_HX_ID, ds.SurgicalHx.SurgicalHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.SurgicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SURGICAL_HX_DATE, ds.SurgicalHx.SurgicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.SurgicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.SurgicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SurgicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SurgicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SurgicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SurgicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SurgicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT, ds.SurgicalHx.SoapTextColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create/update parameters for surgical Hx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateSurgicalHxUpdateParameters(IDBManager dbManager, DSSurgicalHx ds)
        {
            dbManager.CreateUpdateParameters(11);

            dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, ds.SurgicalHx.SurgicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_ID, ds.SurgicalHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SURGICAL_HX_DATE, ds.SurgicalHx.SurgicalHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_B_UNREMARKABLE, ds.SurgicalHx.bUnremarkableColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.SurgicalHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.SurgicalHx.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.SurgicalHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.SurgicalHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.SurgicalHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.SurgicalHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAPTEXT, ds.SurgicalHx.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion
        //End//20/01/2016//Ahmad Raza//Create Parameter methods for SurgicalHx insert,Update

        //Start//20/01/2016//Ahmad Raza//Create Parameter methods for SurgicalHx Disease insert,Update
        #region"Supporting Functions SurgicalHx Disease"

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create parameters for surgical Hx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateParametersSurgicalHxDisease(IDBManager dbManager, DSSurgicalHx ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(26);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SURGICAL_HX_DISEASE_ID, ds.SurgicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SURGICAL_HX_DISEASE_ID, ds.SurgicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, ds.SurgicalHx_Disease.SurgicalHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD9_CODE, ds.SurgicalHx_Disease.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ICD9_CODE_DESCRIPTION, ds.SurgicalHx_Disease.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10_CODE, ds.SurgicalHx_Disease.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ICD10_CODE_DESCRIPTION, ds.SurgicalHx_Disease.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMED_ID, ds.SurgicalHx_Disease.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_SNOMED_DESCRIPTION, ds.SurgicalHx_Disease.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_LEXI_CODE, ds.SurgicalHx_Disease.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_LEXI_CODE_DESCRIPTION, ds.SurgicalHx_Disease.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CPT_CODE, ds.SurgicalHx_Disease.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CPT_CODE_DESCRIPTION, ds.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_STATUS_ID, ds.SurgicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(13, PARM_LOCATION, ds.SurgicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_SURGERY_DATE, ds.SurgicalHx_Disease.SurgeryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_AGE_AT_SURGERY, ds.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SURGERY_REASON, ds.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ORDERING_PROVIDER_ID, ds.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(18, PARM_PERFORMING_PROVIDER_ID, ds.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(19, PARM_COMMENTS, ds.SurgicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_IS_ACTIVE, ds.SurgicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(21, PARM_CREATED_BY, ds.SurgicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_ON, ds.SurgicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_MODIFIED_BY, ds.SurgicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_MODIFIED_ON, ds.SurgicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_SOAP_TEXT, ds.SurgicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create/Insert parameters for surgical Hx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateSurgicalHxDiseaseInsertParameters(IDBManager dbManager, DSSurgicalHx ds)
        {
            dbManager.CreateInsertParameters(24);
            dbManager.AddInsertUpdateParameters(0, PARM_SURGICAL_HX_DISEASE_ID, ds.SurgicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_SURGICAL_HX_ID, ds.SurgicalHx_Disease.SurgicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, null, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODE_DESCRIPTION, ds.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_STATUS_ID, ds.SurgicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(5, PARM_LOCATION, ds.SurgicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SURGERY_DATE, ds.SurgicalHx_Disease.SurgeryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_AGE_AT_SURGERY, ds.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_SURGERY_REASON, ds.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_ORDERING_PROVIDER_ID, ds.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_PERFORMING_PROVIDER_ID, ds.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_COMMENTS, ds.SurgicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_ACTIVE, ds.SurgicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_BY, ds.SurgicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_ON, ds.SurgicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_BY, ds.SurgicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_ON, ds.SurgicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_SOAP_TEXT, ds.SurgicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(18, PARM_CPT_SNOMED_ID, ds.SurgicalHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_CPT_SNOMED_DESCRIPTION, ds.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_FREETEXTCPT, ds.SurgicalHx_Disease.FreeTextProcedureColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, "@tempDiseaseId", ds.SurgicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, "@AddFromMobile", ds.SurgicalHx_Disease.AddFromMobileColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, "@tempCPTId", null, DbType.Int64);

        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will create/Update parameters for surgical Hx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        private void CreateSurgicalHxDiseaseUpdateParameters(IDBManager dbManager, DSSurgicalHx ds)
        {
            dbManager.CreateUpdateParameters(21);

            dbManager.AddInsertUpdateParameters(0, PARM_SURGICAL_HX_DISEASE_ID, ds.SurgicalHx_Disease.DiseaseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_SURGICAL_HX_ID, ds.SurgicalHx_Disease.SurgicalHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, null, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_CODE_DESCRIPTION, ds.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_STATUS_ID, ds.SurgicalHx_Disease.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(5, PARM_LOCATION, ds.SurgicalHx_Disease.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SURGERY_DATE, ds.SurgicalHx_Disease.SurgeryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_AGE_AT_SURGERY, ds.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_SURGERY_REASON, ds.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_ORDERING_PROVIDER_ID, ds.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_PERFORMING_PROVIDER_ID, ds.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_COMMENTS, ds.SurgicalHx_Disease.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IS_ACTIVE, ds.SurgicalHx_Disease.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(13, PARM_CREATED_BY, ds.SurgicalHx_Disease.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_ON, ds.SurgicalHx_Disease.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_MODIFIED_BY, ds.SurgicalHx_Disease.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_ON, ds.SurgicalHx_Disease.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_SOAP_TEXT, ds.SurgicalHx_Disease.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_CPT_SNOMED_ID, ds.SurgicalHx_Disease.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_CPT_SNOMED_DESCRIPTION, ds.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_FREETEXTCPT, ds.SurgicalHx_Disease.FreeTextProcedureColumn.ColumnName, DbType.String);



        }

        #endregion
        //End//20/01/2016//Ahmad Raza//Create Parameter methods for SurgicalHx Disease insert,Update

        //Start//20/01/2016//Ahmad Raza//methods for SurgicalHx  insert,Update,Delete
        #region "SurgicalHx Insert/Update/Delete/Select"

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will insert surgicalHx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx InsertSurgicalHx(DSSurgicalHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.SurgicalHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersSurgicalHx(dbManager, ds, true);
                ds = (DSSurgicalHx)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_INSERT, ds, ds.SurgicalHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString(), null, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSurgicalHx::InsertSurgicalHx", PROC_SURGICALHX_INSERT, ex);
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
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will update surgicalHx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx UpdateSurgicalHx(DSSurgicalHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.SurgicalHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                this.CreateParametersSurgicalHx(dbManager, ds, false);
                ds = (DSSurgicalHx)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_UPDATE, ds, ds.SurgicalHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString(), null, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSurgicalHx::UpdateSurgicalHx", PROC_SURGICALHX_UPDATE, ex);
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
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will delete surgicalHx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public string DeleteSurgicalHx(string SurgicalHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSSurgicalHx dsCurrentSurgicalHx = LoadSurgicalHx(0, Convert.ToInt64(SurgicalHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, SurgicalHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SURGICALHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                else
                {

                    DataTable dtTemp = dsCurrentSurgicalHx.SurgicalHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, SurgicalHxId.ToString(), null, SurgicalHxId.ToString(), false, false, true);
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

                MDVLogger.DALErrorLog("DALSurgicalHx::DeleteSurgicalHx", PROC_SURGICALHX_DELETE, ex);
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

        public SurgicalHxSoapModel LoadNoteSurgicalHx(long PatientId, long UserId, long EntityId, long NoteId)
        {
            SurgicalHxSoapModel model = new SurgicalHxSoapModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));

                parameters.Add(new SqlParameter(PARM_USER_ID, UserId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_SURGICALHX_SELECT_ForSoapText, parameters))
                {
                    while (reader.Read())
                    {


                        var properties = typeof(SurgicalHxSoapModel).GetProperties();

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
                MDVLogger.DALErrorLog("DALSurgicalHx::LoadNoteSurgicalHx", PROC_SURGICALHX_SELECT_ForSoapText, ex);
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
        /// Reason:  This function will load surgicalHx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx LoadSurgicalHx(long PatientId, long SurgicalHxId, string isViewSurgicalHx = "", string isPrintSurgicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSSurgicalHx ds = new DSSurgicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (SurgicalHxId == 0)
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, SurgicalHxId);
                ds = (DSSurgicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_SELECT, ds, ds.SurgicalHx.TableName);
                if (ds.SurgicalHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.SurgicalHx.Rows[0]["SurgicalHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.SurgicalHx;
                        if (dtTemp != null)
                        {
                            if (isViewSurgicalHx == "1" || isPrintSurgicalHx == "1")
                            {
                                bool isViewAction = isViewSurgicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintSurgicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString(), null, ds.SurgicalHx.Rows[0][ds.SurgicalHx.SurgicalHxIdColumn].ToString(), isViewAction, isPrintAcion);
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

                MDVLogger.DALErrorLog("DALSurgicalHx::LoadSurgicalHx", PROC_SURGICALHX_SELECT, ex);
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
        /// Reason:  This function will update soaptext for surgicalHx
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public string updateSoapTextForSurgicalHX(long SurgicalHxId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, SurgicalHxId);

                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_SURGICALHX).ToString();
                
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::updateSoapTextForSurgicalHX", PROC_UPDATE_SOAPTEXT_FOR_SURGICALHX, ex);
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
        //End//20/01/2016//Ahmad Raza//methods for SurgicalHx  insert,Update,Delete

        //Start//20/01/2016//Ahmad Raza//methods for SurgicalHx Disease insert,Update,Delete
        #region  SurgicalHx_Disease Insert/Update/Delete/Select

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will load surgicalHx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx LoadSurgicalHx_Disease(long SurgicalHxId, long DiseaseId, string isView = "", string isPrint = "")
        {
            DSSurgicalHx ds = new DSSurgicalHx();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (SurgicalHxId == 0)
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SURGICAL_HX_ID, SurgicalHxId);

                if (DiseaseId == 0)
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_DISEASE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_DISEASE_ID, DiseaseId);

                ds = (DSSurgicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_DISEASE_SELECT, ds, ds.SurgicalHx_Disease.TableName);
                //Start 31-10-2016 Humaira Yousaf for dbaudit
                if (ds.SurgicalHx_Disease.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.SurgicalHx_Disease.Rows[0]["SurgicalHxId"]) > 0 && DiseaseId > 0)
                    {
                        DataTable dtTemp = ds.SurgicalHx_Disease;
                        if (dtTemp != null)
                        {
                            if (isView == "1" || isPrint == "1")
                            {
                                bool isViewAction = isView == "1" ? true : false;
                                bool isPrintAcion = isPrint == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SurgicalHx_Disease.Rows[0][ds.SurgicalHx_Disease.DiseaseIdColumn].ToString(), null, ds.SurgicalHx_Disease.Rows[0][ds.SurgicalHx_Disease.SurgicalHxIdColumn].ToString(), isViewAction, isPrintAcion);
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
                MDVLogger.DALErrorLog("DALSurgicalHx::LoadSurgicalHx_Disease", PROC_SURGICALHX_DISEASE_SELECT, ex);
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
        /// Reason:  This function will insert update surgicalHx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx insertUpdateSurgicalHxDisease(DSSurgicalHx ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.SurgicalHx_Disease.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateSurgicalHxDiseaseInsertParameters(dbManager, ds);
                CreateSurgicalHxDiseaseUpdateParameters(dbManager, ds);
                ds = (DSSurgicalHx)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_SURGICALHX_DISEASE_INSERT, PROC_SURGICALHX_DISEASE_UPDATE, ds, ds.SurgicalHx_Disease.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");

                    int k = 0;
                    for (int i = 0; i < ds.SurgicalHx_Disease.Rows.Count; i++)
                    {
                        if (dtTemp.Rows.Count > k)
                        {
                            bool isExists = false;
                            if (!ds.SurgicalHx_Disease.Rows[i].IsNull("FreeTextProcedure") &&
                                ds.SurgicalHx_Disease.Rows[i][ds.SurgicalHx_Disease.FreeTextProcedureColumn] == dtTemp.Rows[k]["FreeTextProcedure"])
                            {
                                isExists = true;
                            }
                            if (!ds.SurgicalHx_Disease.Rows[i].IsNull("CPTCode") &&
                                ds.SurgicalHx_Disease.Rows[i][ds.SurgicalHx_Disease.CPTCodeColumn] == dtTemp.Rows[k]["CPTCode"])
                            {
                                isExists = true;
                            }
                            if (isExists)
                            {
                                dtTemp.Rows[k]["PrimaryKey"] = ds.SurgicalHx_Disease.Rows[i][ds.SurgicalHx_Disease.DiseaseIdColumn];
                                k++;
                            }
                        }
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.SurgicalHx_Disease.Rows[0][ds.SurgicalHx_Disease.DiseaseIdColumn].ToString(), null, ds.SurgicalHx_Disease.Rows[0][ds.SurgicalHx_Disease.SurgicalHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALSurgicalHx::insertUpdateSurgicalHxDisease", PROC_SURGICALHX_DISEASE_INSERT + " " + PROC_SURGICALHX_DISEASE_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will delete surgicalHx Disease
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public string deleteSurgicalHxDisease(string DiseaseId, string PatientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                //dbManager.Open();
                dbManager.BeginTransaction();

                DSSurgicalHx dsSurgicalHx = LoadSurgicalHx_Disease(0, Convert.ToInt64(DiseaseId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SURGICAL_HX_DISEASE_ID, DiseaseId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SURGICALHX_DISEASE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsSurgicalHx.SurgicalHx_Disease;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, DiseaseId, null, dsSurgicalHx.SurgicalHx_Disease.Rows[0][dsSurgicalHx.SurgicalHx_Disease.SurgicalHxIdColumn].ToString(), false, false, true, PatientId);
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

                MDVLogger.DALErrorLog("DALSurgicalHx::deleteSurgicalHxDisease", PROC_SURGICALHX_DISEASE_DELETE, ex);
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
        //End//20/01/2016//Ahmad Raza//methods for SurgicalHx Disease insert,Update,Delete

        //Start//20/01/2016//Ahmad Raza//methods for Attach/Detach SurgiclaHx from Note
        #region Association with Notes

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will attach surgicalHx with notes
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHx attachSurgicalHxWithNotes(string surgicalHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSSurgicalHx ds = new DSSurgicalHx();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(surgicalHxId))
                {
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, surgicalHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSSurgicalHx)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_SURGICALHX_FROM_NOTES, ds, ds.SurgicalHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::attachSurgicalHxWithNotes", PROC_ATTACH_SURGICALHX_FROM_NOTES, ex);
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
        /// Reason:  This function will detach surgicalHx from Notes
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public string detachSurgicalHxFromNotes(long surgicalHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (surgicalHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_SURGICAL_HX_ID, surgicalHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_SURGICALHX_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::detachSurgicalHxFromNotes", PROC_DETACH_SURGICALHX_FROM_NOTES, ex);
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
        //End//20/01/2016//Ahmad Raza//methods for Attach/Detach SurgiclaHx from Note

        //Start//20/01/2016//Ahmad Raza//methods for loading lookups
        #region Surgical History lookups

        /// <summary>
        /// Author:  Ahmad Raza
        /// Date:    28/01/2016
        /// Reason:  This function will load SurgicalHxStatus Lookup
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHxLookup LookupSurgicalHxStatus()
        {
            DSSurgicalHxLookup ds = new DSSurgicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSurgicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SURGICALHX__STATUS_LOOKUP, ds, ds.SurgicalHx_Status.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::LookupSurgicalHxStatus", PROC_SURGICALHX__STATUS_LOOKUP, ex);
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
        /// Reason:  This function will load SurgicalHxLocaion Lookup
        /// </summary>
        /// <param name="surgicalHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DSSurgicalHxLookup LookupSurgicalHxLocaion()
        {
            DSSurgicalHxLookup ds = new DSSurgicalHxLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSSurgicalHxLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SURGICALHX__LOCATION_LOOKUP, ds, ds.SurgicalHx_Location.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::LookupSurgicalHxLocaion", PROC_SURGICALHX__LOCATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        //End//20/01/2016//Ahmad Raza//methods for loading lookups

        #region Legacy Notes

        public List<SurgicalHx> NotesSurgicalHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<SurgicalHx> objList_SurgicalHx = new List<SurgicalHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_SURGICALHX_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        SurgicalHx model = new SurgicalHx();
                        var properties = typeof(SurgicalHx).GetProperties();
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
                        objList_SurgicalHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSurgicalHx::NotesSurgicalHxSelect", PROC_NOTES_SURGICALHX_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_SurgicalHx;
        }

        #endregion Legacy Notes

    }
}
