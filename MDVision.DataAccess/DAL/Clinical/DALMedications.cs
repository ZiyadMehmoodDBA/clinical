using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Medication;
using MDVision.Model.Clinical.Medical.ProblemLists;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALMedications
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PRESCRIPTION_SELECT = "Clinical.sp_PrescriptionSelect";
        private const string PROC_PRESCRIPTION_SELECT_OP = "Clinical.sp_CPre_PrescriptionSelect";
        private const string PROC_PRESCRIPTION_SELECT_PRINT = "Clinical.sp_PrescriptionSelectForPrint";
        private const string PROC_MEDICATION_SELECT = "Clinical.sp_MedicationSelect";
        private const string PROC_MEDICATION_SELECT_OP = "Clinical.sp_CM_MedicationSelect";
        private const string PROC_MEDICATION_SELECT_ALL = "Clinical.sp_SelectAllMedication";
        private const string PROC_MEDICATION_SELECT_FOR_CDS = "Clinical.sp_DrugSelect";
        private const string PROC_MEDICATION_SELECT_FOR_CDSMedication = "Clinical.sp_DrugSelectMedication";
        private const string PROC_MEDICATION_REVIEW_SELECT = "Clinical.sp_MedicationReviewSelect";
        private const string PROC_GET_EXSISTING_PATIENTS = "Clinical.sp_GetExsistingPatients";

        private const string PROC_MEDICATION_ROUTEID_UPDATE = "Clinical.sp_MedicationRouteIdUpdate";
        private const string PROC_MEDICATION_NEGATIONREASONID_UPDATE = "Clinical.sp_MedicationNegationReasonIdUpdate";

        private const string PROC_MEDICATION_SELECT_LOOKUP = "Clinical.sp_MedicationLookup";
        private const string PROC_MEDICATION_REPORT_LOOKUP = "Clinical.sp_MedicationReportLookup";
        private const string PROC_MEDICATION_ANTIMICROBIAL_ROUTE_LOOKUP = "Clinical.sp_MedicationAntimicrobialRouteLookup";
        private const string PROC_All_MEDICATION_SELECT = "Clinical.sp_MedicationAllSelect";
        private const string PROC_MEDICATION_NEGATION_REASON_LOOKUP = "Clinical.sp_VaccineRefusalReasonLookup";
        private const string PROC_MEDICATION_INSERT = "Clinical.sp_MedicationInsert";
        private const string PROC_PRESCRIPTION_INSERT = "Clinical.sp_PrescriptionInsert";
        private const string PROC_DRUG_INSERT = "Clinical.sp_DrugInsert";

        private const string PROC_PHARAMACY_INSERT = "Clinical.sp_PharmacyInsert";
        private const string PROC_ATTACH_PRESCRIPTION_WITH_NOTES = "Clinical.sp_AttachPrescriptionWithNotes";
        private const string PROC_ATTACH_PRESCRIPTION_WITH_NOTES_OP = "Clinical.sp_CPre_AttachPrescriptionWithNotes";
        private const string PROC_DETACH_PRESCRIPTION_FROM_NOTES = "Clinical.sp_DetachPrescriptionFromNotes";

        private const string PROC_GET_LATEST_PRESCRIPTION_BY_PATIENTID = "Clinical.sp_GetLatestPrescriptionByPatientId";
        private const string PROC_GET_LATEST_PRESCRIPTION_FORSOAPTEXT = "Clinical.sp_GetLatestPrescriptionForSoapText";

        private const string PROC_PRESCRIPTION_SELECT_FORSOAPTEXT = "Clinical.sp_PrescriptionsSelectForSoapText";
        private const string PROC_PRESCRIPTION_SELECT_FORSOAPTEXT_OP = "Clinical.sp_CPre_PrescriptionsSelectForSoapText";

        private const string PROC_MEDICATION_SELECTT_FORSOAPTEXT = "Clinical.sp_MedicationsSelectForSoapText";
        private const string PROC_MEDICATION_SELECTT_FORSOAPTEXT_OP = "Clinical.sp_CM_MedicationsSelectForSoapText";
        private const string PROC_ATTACH_MEDICATION_WITH_NOTES = "Clinical.sp_AttachMedicationWithNotes";
        private const string PROC_ATTACH_MEDICATION_WITH_NOTES_OP = "Clinical.sp_CM_AttachMedicationWithNotes";
        private const string PROC_DETACH_MEDICATION_FROM_NOTES = "Clinical.sp_DetachMedicationFromNotes";

        private const string PROC_GET_LATEST_MED_BY_PATIENTID = "Clinical.sp_GetLatestMedicationByPatientId";
        private const string PROC_GET_LATEST_MED_FORSOAPTEXT = "Clinical.sp_GetLatestMedicationForSoapText";

        private const string PROC_INSERT_MEDICATION_REVIEW = "Clinical.sp_MedicationReviewInsert";
        private const string GET_MEDICATION_BY_RCOPIAID = "Clinical.sp_GetMedicationAgainstRcopiaID";
        private const string GET_PRESCRIPTION_BY_RCOPIAID = "Clinical.sp_GetPrescriptionAgainstRcopiaID";

        private const string PROC_PRESCRIPTION_DELETE = "Clinical.sp_PrescriptionDelete";
        private const string PROC_ATTACH_WITH_WHICH_NOTE = "Clinical.sp_PresAttachWithWhichNote";

        private const string PROC_NOTES_MEDICATION_SELECT = "[Clinical].[sp_NotesMedicationSelect]";





        #endregion
        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : Parameters for Medications.
        /// Date : 14 january 2016


        #region "Parameters"
        private const string PARM_PRESCRIPTION_ID = "@PrescriptionID";
        private const string PARM_MEDICATION_ID = "@MedicationID";
        private const string PARM_MEDICATION_NAME = "@MedicationName";

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_RECORDS_PERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_RCOPIA = "@RcopiaID";
        private const string PARM_PATIENTID = "@PatientID";
        private const string PARM_ROUTEID = "@RouteId";
        private const string PARM_USERID = "@UserId";
        private const string PARM_ENTITYID = "@EntityId";
        private const string PARM_PRESCRIPTIONID = "@PrescriptionID";
        private const string PARM_PROVIDERID = "@ProviderID";
        private const string PARM_PREPARER_USERID = "@Preparer_UserID";
        private const string PARM_DRUGID = "@DrugID";
        private const string PARM_ACTION = "@Action";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_DOSEUNIT = "@DoseUnit";
        private const string PARM_ROUTEBY = "@Routeby";
        private const string PARM_DOSE_TIMING = "@DoseTiming";
        private const string PARM_DOSE_OTHER = "@DoseOther";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_NPI = "@NPI";
        private const string PARM_QUANTITYUNIT = "@QuantityUnit";
        private const string PARM_SUBSTITUTION = "@Substitution";
        private const string PARM_OTHERNOTE = "@OtherNote";
        private const string PARM_PATIENTNOTES = "@PatientNotes";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_STOPEDATE = "@StopDate";
        private const string PARM_STOPREASON = "@StopReason";
        private const string PARM_SIGCHNAGEDATE = "@SigChangedDate";
        private const string PARM_LASTMODIFIEDBY = "@LastModifiedBy";
        private const string PARM_LASTMODIFIEDDATE = "@LastModifiedDate";
        private const string PARM_INTENDEDUSE = "@IntendedUse";
        private const string PARM_NUMBER = "@Number";
        private const string PARM_STATUS = "@Status";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARAM_ISCURRENT = "@isCurrent";
        private const string PARAM_PRESCRIPTION_RCOPIAID = "@PrescriptionRcopiaID";
        private const string PARAM_REFILL = "@Refill";
        private const string PARAM_FILLDATE = "@FillDate";
        private const string PARAM_ISDELETED = "@IsDeleted";
        private const string PARM_NEGATIONREASONID = "@NegationReasonId";

        private const string PARM_MEDREVIEW_ID = "@MedicationReviewId";

        private const string PARM_PATIENT_IDS = "@PatientIds";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_NOTES_ID = "@NotesId";



        private const string PARM_MEDICATION_REVIEW_ID = "@MedicationReviewId";

        private const string PARM_REVIEWED_BY = "@ReviewedBy";
        private const string PARM_REVIEWED_ON = "@ReviewedOn";
        private const string PARM_ISNEWROW = "@IsNewRow";

        private const string PARM_IS_HISTORY = "@IsHistory";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ORDERSET_ID = "@OrderSetId";
        private const string PARM_RCOPIA_USERNAME = "@RcopiaUserName";
        

        #endregion
        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : Parameters for Prescription
        /// Date : 14 january 2016

        #region "Prescription Parameters"
        private const string PARM_PRESCRIPTIONID_PRESCRIPTION = "@PrescriptionID";
        private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_PATIIENTID = "@PatientID";
        private const string PARM_PROVIDERID_PRESCRIPTION = "@ProviderID";
        private const string PARM_PREPARER_USERID_PRESCRIPTION = "@Preparer_UserID";
        private const string PARM_PHARMACYID = "@PharmacyID";
        private const string PARM_CREATEDATE = "@CreatedDate";
        private const string PARM_COMPLETEDDATE = "@CompletedDate";
        private const string PARM_SIGNEDDATE = "@SignedDate";
        private const string PARM_STOPDATE = "@StopDate";
        private const string PARM_LASTMODIFIEDBYMEDICATION = "@LastModifiedBy";
        private const string PARM_LASTMODIFIEDDATEMEDICATION = "@LastModifiedDate";
        private const string PARM_SENDMETHOD = "@SendMethod";
        private const string PARM_REFILL = "@Refill";
        private const string PARM_CANCELLEDDATE = "@Cancelleddate";
        private const string PARM_ISACTIVE_PRESCRIPTION = "@IsActive";
        private const string PARM_CREATEDBY_PRESCRIPTION = "@CreatedBy";
        private const string PARM_CREATEDON_PRESCRIPTION = "@CreatedOn";
        private const string PARM_MODIFIEDBY_PRESCRIPTION = "@ModifiedBy";
        private const string PARM_MODIFIEDON_PRESCRIPTION = "@ModifiedOn";

        private const string PARM_DRUG_RCOPIAID = "@DrugRcopiaID";
        private const string PARM_COMPLETION_ACTION = "@CompletionAction";
        private const string PARM_SEND_METHOD = "@SendMethod";

        #endregion
        #region "Drug Parameters"
        private const string PARM_DRUG_ID = "@DrugId";
        private const string PARM_DRUGRCOPIA = "@RcopiaID";
        private const string PARM_NDCID = "@NDCID";
        private const string PARM_FIRSTDATABANKMEDID = "@FirstDataBankMedID";
        private const string PARM_DRUGDESCRIPTION = "@DrugDescription";
        private const string PARM_BRANDNAME = "@BrandName";
        private const string PARM_GENERICNAME = "@GenericName";
        private const string PARM_SCHEDULE = "@Schedule";
        private const string PARM_BRANDTYPE = "@BrandType";
        private const string PARM_LEGENDSTATUS = "@LegendStatus";
        private const string PARM_ROUTE = "@Route";
        private const string PARM_FORM = "@Form";
        private const string PARM_STRENGTH = "@Strength";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_DRUGCREATEDBY = "@CreatedBy";
        private const string PARM_DRUGCREATEDON = "@CreatedOn";
        private const string PARM_DRUGMODIFIEDBY = "@ModifiedBy";
        private const string PARM_DRUGMODIFIEDON = "@ModifiedOn";
        private const string PARM_RXNORMID = "@RxnormID";
        private const string PARM_RXNORMIDTYPE = "@RxnormIDType";

        private const string PARM_PHARAMACYID = "@PharmacyId";
        private const string PARM_PHARAMACYRCOPIAID = "@RcopiaID";
        private const string PARM_RCOPIAMASTERID = "@RcopiaMasterID";
        private const string PARM_NCPDPID = "@NCPDPID";
        private const string PARM_PHARAMACYNAME = "@PharmacyName";
        private const string PARM_PHARAMACYADDRESS = "@Address";
        private const string PARM_PHARAMACYCITY = "@City";
        private const string PARM_PHARAMACYSTATE = "@State";
        private const string PARM_PHARAMACYZIP = "@Zip";
        private const string PARM_PHARAMACYPHONE = "@Phone";
        private const string PARM_PHARAMACYFAX = "@Fax";
        private const string PARM_IS24HOURS = "@Is24Hour";
        private const string PARM_LEVEL3 = "@Level3";
        private const string PARM_ELECTRONIC = "@Electronic";
        private const string PARM_MAILORDER = "@MailOrder";
        private const string PARM_RETAIL = "@Retail";
        private const string PARM_LONGTERMCARE = "@LongTermCare";
        private const string PARM_SPECIALITY = "@Specialty";
        private const string PARM_CANRECEIVEDCONTROLLEDSUBSTANCE = "@CanReceiveControlledSubstance";
        private const string PARM_PHARAMACYISACTIVE = "@IsActive";
        private const string PARM_PHARAMACYCREATEDBY = "@CreatedBy";
        private const string PARM_PHARAMACYCREATEDON = "@CreatedOn";
        private const string PARM_PHARAMACYMODIFIEDBY = "@ModifiedBy";
        private const string PARM_PHARAMACYMOFIEDON = "@ModifiedOn";


        #endregion

        #region Constructors

        public DALMedications()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALMedications(SharedVariable SharedVariable)
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

        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : function for Medication Parameters.
        /// Date : 14 january 2016
        /// </summary>
        /// <param name="medicationId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        #region "Drug Parameters"
        /// Author : Wasim Malik
        /// Purpose : function for Medication Parameters.
        /// Date : 14 january 2016
        private void CreateParametersDrug(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_DRUG_ID, ds.Drug.DrugIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_DRUG_ID, ds.Drug.DrugIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(1, PARM_DRUGRCOPIA, ds.Drug.RcopiaIDColumn.ColumnName, DbType.String);

            dbManager.AddParameters(2, PARM_NDCID, ds.Drug.NDCIDColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_FIRSTDATABANKMEDID, ds.Drug.FirstDataBankMedIDColumn.ColumnName, DbType.String);

            dbManager.AddParameters(4, PARM_DRUGDESCRIPTION, ds.Drug.DrugDescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_BRANDNAME, ds.Drug.BrandNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_GENERICNAME, ds.Drug.GenericNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_SCHEDULE, ds.Drug.ScheduleColumn.ColumnName, DbType.String);

            dbManager.AddParameters(8, PARM_BRANDTYPE, ds.Drug.BrandTypeColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_LEGENDSTATUS, ds.Drug.LegendStatusColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_ROUTE, ds.Drug.RouteColumn.ColumnName, DbType.String);

            dbManager.AddParameters(11, PARM_FORM, ds.Drug.FormColumn.ColumnName, DbType.String);

            dbManager.AddParameters(12, PARM_STRENGTH, ds.Drug.StrengthColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_IS_ACTIVE, ds.Drug.IsActiveColumn.ColumnName, DbType.String);

            dbManager.AddParameters(14, PARM_DRUGCREATEDBY, ds.Drug.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(15, PARM_DRUGCREATEDON, ds.Drug.CreatedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(16, PARM_DRUGMODIFIEDBY, ds.Drug.ModifiedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(17, PARM_DRUGMODIFIEDON, ds.Drug.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(18, PARM_RXNORMID, ds.Drug.RxnormIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_RXNORMIDTYPE, ds.Drug.RxnormIDTypeColumn.ColumnName, DbType.String);





        }
        private void CreateParametersForInsertReviews(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEDICATION_REVIEW_ID, ds.MedicationReview.MedicationReviewIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEDICATION_REVIEW_ID, ds.MedicationReview.MedicationReviewIDColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.MedicationReview.PatientIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(2, PARM_REVIEWED_BY, ds.MedicationReview.ReviewedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_REVIEWED_ON, ds.MedicationReview.ReviewedOnColumn.ColumnName, DbType.DateTime);



        }

        #endregion
        #region "Medication Parameters"
        private void CreatePharamacyParameter(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(23);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PHARAMACYID, ds.Pharmacy.PharmacyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PHARAMACYID, ds.Pharmacy.PharmacyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PHARAMACYRCOPIAID, ds.Pharmacy.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_RCOPIAMASTERID, ds.Pharmacy.RcopiaMasterIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_NCPDPID, ds.Pharmacy.NCPDPIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PHARAMACYNAME, ds.Pharmacy.PharmacyNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_PHARAMACYADDRESS, ds.Pharmacy.AddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_PHARAMACYCITY, ds.Pharmacy.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PHARAMACYSTATE, ds.Pharmacy.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PHARAMACYZIP, ds.Pharmacy.ZipColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PHARAMACYPHONE, ds.Pharmacy.PhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_PHARAMACYFAX, ds.Pharmacy.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS24HOURS, ds.Pharmacy.Is24HourColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ELECTRONIC, ds.Pharmacy.ElectronicColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MAILORDER, ds.Pharmacy.MailOrderColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_RETAIL, ds.Pharmacy.RetailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_LONGTERMCARE, ds.Pharmacy.LongTermCareColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SPECIALITY, ds.Pharmacy.SpecialtyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_CANRECEIVEDCONTROLLEDSUBSTANCE, ds.Pharmacy.CanReceiveControlledSubstanceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_ISACTIVE, ds.Pharmacy.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(19, PARM_CREATEDBY, ds.Pharmacy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_CREATEDON, ds.Pharmacy.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_MODIFIEDBY, ds.Pharmacy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_MODIFIEDON, ds.Pharmacy.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }
        private void CreateParameters(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(44);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MEDICATION_ID, ds.Medication.MedicationIDColumn.ColumnName, DbType.Int64);




            dbManager.AddParameters(1, PARM_RCOPIA, ds.Medication.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PATIENTID, ds.Medication.PatientIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PRESCRIPTIONID, ds.Medication.PrescriptionIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PROVIDERID, ds.Medication.ProviderIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_PREPARER_USERID, ds.Medication.Preparer_UserIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_DRUGID, ds.Medication.DrugIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_ACTION, ds.Medication.ActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_DOSE, ds.Medication.DoseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_DOSEUNIT, ds.Medication.DoseUnitColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_ROUTEBY, ds.Medication.RoutebyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_DOSE_TIMING, ds.Medication.DoseTimingColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_DOSE_OTHER, ds.Medication.DoseOtherColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_DURATION, ds.Medication.DurationColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_QUANTITY, ds.Medication.QuantityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_QUANTITYUNIT, ds.Medication.QuantityUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SUBSTITUTION, ds.Medication.SubstitutionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_OTHERNOTE, ds.Medication.OtherNoteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_PATIENTNOTES, ds.Medication.PatientNotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_COMMENTS, ds.Medication.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_STARTDATE, ds.Medication.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARM_STOPEDATE, ds.Medication.StopDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_STOPREASON, ds.Medication.StopReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_SIGCHNAGEDATE, ds.Medication.SigChangedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_LASTMODIFIEDBY, ds.Medication.LastModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_LASTMODIFIEDDATE, ds.Medication.LastModifiedDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(26, PARM_INTENDEDUSE, ds.Medication.IntendedUseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_NUMBER, ds.Medication.NumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(28, PARM_STATUS, ds.Medication.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ISACTIVE, ds.Medication.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(30, PARM_CREATEDBY, ds.Medication.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_CREATEDON, ds.Medication.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(32, PARM_MODIFIEDBY, ds.Medication.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_MODIFIEDON, ds.Medication.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(34, PARAM_PRESCRIPTION_RCOPIAID, ds.Medication.PrescriptionRcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARAM_REFILL, ds.Medication.RefillColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARAM_FILLDATE, ds.Medication.FillDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARAM_ISDELETED, ds.Medication.IsDeletedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(38, PARM_ISNEWROW, ds.Medication.IsNewRowColumn.ColumnName, DbType.Byte, ParamDirection.Output);
            dbManager.AddParameters(39, PARM_DRUGDESCRIPTION, ds.Medication.DrugDescriptionColumn.ColumnName, DbType.String);//kr
            dbManager.AddParameters(40, PARM_ORDERSET_ID, ds.Medication.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(41, PARM_RCOPIA_USERNAME, ds.Medication.RcopiaUserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(42, PARM_NPI, ds.Medication.NPIColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(43, PARM_ENTITYID, ds.Medication.EntityIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : function For Prescription Parameters.
        /// Date : 14 january 2016

        #region "Prescription Parameters"
        private void CreateParametersPrescription(IDBManager dbManager, DSClinicalMedication ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(35);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PRESCRIPTIONID_PRESCRIPTION, ds.Prescription.PrescriptionIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PRESCRIPTIONID_PRESCRIPTION, ds.Prescription.PrescriptionIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_RCOPIAID, ds.Prescription.RcopiaIDColumn.ColumnName, DbType.String);

            dbManager.AddParameters(2, PARM_PATIIENTID, ds.Prescription.PatientIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_PROVIDERID_PRESCRIPTION, ds.Prescription.ProviderIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(4, PARM_PREPARER_USERID, ds.Medication.Preparer_UserIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_PHARMACYID, ds.Prescription.PharmacyIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(6, PARM_CREATEDATE, ds.Prescription.CreatedDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(7, PARM_COMPLETEDDATE, ds.Prescription.CompletedDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(8, PARM_SIGNEDDATE, ds.Prescription.SignedDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(9, PARM_STOPDATE, ds.Prescription.StopDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(10, PARM_LASTMODIFIEDBYMEDICATION, ds.Prescription.LastModifiedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(11, PARM_LASTMODIFIEDDATEMEDICATION, ds.Prescription.LastModifiedDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(12, PARM_SENDMETHOD, ds.Prescription.SendMethodColumn.ColumnName, DbType.String);

            dbManager.AddParameters(13, PARM_REFILL, ds.Prescription.RefillColumn.ColumnName, DbType.String);

            dbManager.AddParameters(14, PARM_CANCELLEDDATE, ds.Prescription.CancelleddateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(15, PARM_ISACTIVE_PRESCRIPTION, ds.Prescription.IsActiveColumn.ColumnName, DbType.Byte);

            dbManager.AddParameters(16, PARM_CREATEDBY_PRESCRIPTION, ds.Prescription.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(17, PARM_CREATEDON_PRESCRIPTION, ds.Prescription.CreatedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(18, PARM_MODIFIEDBY_PRESCRIPTION, ds.Prescription.ModifiedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(19, PARM_MODIFIEDON_PRESCRIPTION, ds.Prescription.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(20, PARM_DRUG_RCOPIAID, ds.Prescription.DrugRcopiaIDColumn.ColumnName, DbType.String);

            dbManager.AddParameters(21, PARM_COMPLETION_ACTION, ds.Prescription.CompletionActionColumn.ColumnName, DbType.String);


            dbManager.AddParameters(22, PARM_INTENDEDUSE, ds.Prescription.IntendedUseColumn.ColumnName, DbType.String);

            dbManager.AddParameters(23, PARM_ACTION, ds.Prescription.ActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_DOSE, ds.Prescription.DoseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_DOSEUNIT, ds.Prescription.DoseUnitColumn.ColumnName, DbType.String);

            dbManager.AddParameters(26, PARM_ROUTEBY, ds.Prescription.RoutebyColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_DOSE_TIMING, ds.Prescription.DoseTimingColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_DOSE_OTHER, ds.Prescription.DoseOtherColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_DURATION, ds.Prescription.DurationColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARM_QUANTITY, ds.Prescription.QuantityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_QUANTITYUNIT, ds.Prescription.QuantityUnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_SUBSTITUTION, ds.Prescription.SubstitutionPermittedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_ISNEWROW, ds.Prescription.IsNewRowColumn.ColumnName, DbType.Byte, ParamDirection.Output);
            dbManager.AddParameters(34, PARM_DRUGDESCRIPTION, ds.Prescription.DrugDescriptionColumn.ColumnName, DbType.String);

        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose:  Getting Prescriptions
        /// Date : January 14, 2016
        #region "Load Prescriptions"
        public DSClinicalMedication loadPrescriptions(long prescriptionID, long patientId, long notesId, int pageNumber, int rowsPerPage, string isViewAllergy = "", string isPrintAllergy = "", string RcopiaId = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Prescription;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);

                if (prescriptionID == 0)
                    dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, prescriptionID);


                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENTID, patientId);
                if (notesId == 0)
                    dbManager.AddParameters(2, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTES_ID, notesId);
                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_RECORDS_PERPAGE, null);
                else
                    dbManager.AddParameters(4, PARM_RECORDS_PERPAGE, rowsPerPage);
                if (RcopiaId == "")
                    dbManager.AddParameters(6, PARM_RCOPIA, null);
                else
                    dbManager.AddParameters(6, PARM_RCOPIA, RcopiaId);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.Prescription.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRESCRIPTION_SELECT, ds, ds.Prescription.TableName);
                if (dtTemp != null)
                {
                    if (isViewAllergy == "1" || isPrintAllergy == "1")
                    {

                        if (ds.Prescription.Rows.Count > 0)
                        {
                            bool isViewAction = isViewAllergy == "1" ? true : false;
                            bool isPrintAcion = isPrintAllergy == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Prescription.Rows[0][ds.Prescription.PrescriptionIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                            dsDBAudit.AcceptChanges();
                        }

                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::loadPrescriptions", PROC_PRESCRIPTION_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::loadPrescriptions", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication loadPrescriptionsOp(long patientId, long notesId, int pageNumber, int rowsPerPage, string isViewPrescription = "", string isPrintPrescription = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Prescription;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(5);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);
                if (notesId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, notesId);
                if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_RECORDS_PERPAGE, null);
                else
                    dbManager.AddParameters(3, PARM_RECORDS_PERPAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.Prescription.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRESCRIPTION_SELECT_OP, ds, ds.Prescription.TableName);
                if (dtTemp != null)
                {
                    //Start 07-11-2016 Humaira Yousaf to add view action log against each item
                    if (isViewPrescription == "1" || isPrintPrescription == "1")
                    {

                        if (ds.Prescription.Rows.Count > 0)
                        {
                            bool isViewAction = isViewPrescription == "1" ? true : false;
                            bool isPrintAcion = isPrintPrescription == "1" ? true : false;


                            dtTemp.Columns.Add("PrimaryKey");
                            dtTemp.Columns.Add("ViewAction");

                            for (int i = 0; i < ds.Prescription.Rows.Count; i++)
                            {
                                dtTemp.Rows[i]["PrimaryKey"] = ds.Prescription.Rows[i][ds.Prescription.PrescriptionIDColumn];
                                dtTemp.Rows[i]["ViewAction"] = "View";
                            }
                            //End 07-11-2016 Humaira Yousaf to add view action log against each item

                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Prescription.Rows[0][ds.Prescription.PrescriptionIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                            dsDBAudit.AcceptChanges();
                        }

                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadPrescriptions", PROC_PRESCRIPTION_SELECT_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalMedication LoadPrescriptionsForPrint(long patientId, string isPrintPrescription = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Prescription;
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENTID, patientId);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRESCRIPTION_SELECT_PRINT, ds, ds.Prescription.TableName);
                if (dtTemp != null)
                {
                    if (isPrintPrescription == "1")
                    {
                        if (ds.Prescription.Rows.Count > 0)
                        {
                            bool isPrintAcion = isPrintPrescription == "1" ? true : false;
                            dtTemp.Columns.Add("PrimaryKey");
                            dtTemp.Columns.Add("ViewAction");
                            for (int i = 0; i < ds.Prescription.Rows.Count; i++)
                            {
                                dtTemp.Rows[i]["PrimaryKey"] = ds.Prescription.Rows[i][ds.Prescription.PrescriptionIDColumn];
                                dtTemp.Rows[i]["ViewAction"] = "View";
                            }
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Prescription.Rows[0][ds.Prescription.PrescriptionIDColumn].ToString(), null, "", false, isPrintAcion, false);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LoadPrescriptionsForPrint", PROC_PRESCRIPTION_SELECT_PRINT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to load Medications.
        /// Date : 14 january 2016
        /// </summary>
        /// <param name="medicationId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        public DSClinicalMedication loadMedications(long medicationId, long patientId, long noteId, bool isCurrent, int pageNo = 1, int rpp = 15, string isHistory = "0", string isViewAllergy = "", string isPrintAllergy = "", bool isFromCDS = false)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(8);
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);
                if (medicationId == 0)
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, medicationId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");

                if (isFromCDS == true)
                {
                    dbManager.AddParameters(3, PARAM_ISCURRENT, true);
                }
                else
                {
                    dbManager.AddParameters(3, PARAM_ISCURRENT, isCurrent);
                }


                if (noteId == 0)
                    dbManager.AddParameters(4, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_NOTE_ID, noteId);

                if (pageNo == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNo);
                if (rpp == 0)
                    dbManager.AddParameters(6, PARM_RECORDS_PERPAGE, null);
                else
                    dbManager.AddParameters(6, PARM_RECORDS_PERPAGE, rpp);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Medication.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT, ds, ds.Medication.TableName);

                if (isHistory.Equals("1"))
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSClinicalMedication dsMedicationHistory = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT, ds, ds.MedicationHistory.TableName);
                    if (dsMedicationHistory != null)
                    {
                        ds.Merge(dsMedicationHistory);
                    }
                }

                if (isFromCDS == true)
                {
                    dbManager.CommitTransaction();
                    return ds;
                }
                else
                {
                    if (ds.Medication.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(ds.Medication.Rows[0]["MedicationID"]) > 0)
                        {
                            if (dtTemp != null)
                            {
                                if (isViewAllergy == "1" || isPrintAllergy == "1")
                                {
                                    bool isViewAction = isViewAllergy == "1" ? true : false;
                                    bool isPrintAcion = isPrintAllergy == "1" ? true : false;
                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                    dsDBAudit.AcceptChanges();
                                }
                            }
                        }
                    }
                    dbManager.CommitTransaction();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedications", PROC_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication loadAllMedications(long medicationId, long patientId, long noteId, bool isCurrent, string isHistory = "0", string isViewAllergy = "", string isPrintAllergy = "", bool isFromCDS = false)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(5);
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);
                if (medicationId == 0)
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, null);
                else
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, medicationId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");

                if (isFromCDS == true)
                {
                    dbManager.AddParameters(3, PARAM_ISCURRENT, true);
                }
                else
                {
                    dbManager.AddParameters(3, PARAM_ISCURRENT, isCurrent);
                }


                if (noteId == 0)
                    dbManager.AddParameters(4, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_NOTE_ID, noteId);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_All_MEDICATION_SELECT, ds, ds.Medication.TableName);

                if (isHistory.Equals("1"))
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSClinicalMedication dsMedicationHistory = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_All_MEDICATION_SELECT, ds, ds.MedicationHistory.TableName);
                    if (dsMedicationHistory != null)
                    {
                        ds.Merge(dsMedicationHistory);
                    }
                }

                if (isFromCDS == true)
                {
                    dbManager.CommitTransaction();
                    return ds;
                }
                else
                {
                    if (ds.Medication.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(ds.Medication.Rows[0]["MedicationID"]) > 0)
                        {
                            if (dtTemp != null)
                            {
                                if (isViewAllergy == "1" || isPrintAllergy == "1")
                                {
                                    bool isViewAction = isViewAllergy == "1" ? true : false;
                                    bool isPrintAcion = isPrintAllergy == "1" ? true : false;
                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                    dsDBAudit.AcceptChanges();
                                }
                            }
                        }
                    }
                    dbManager.CommitTransaction();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadAllMedications", PROC_All_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        public DSClinicalMedication loadMedicationsOp_Obsolete(long patientId, long noteId, bool isCurrent, int pageNo = 1, int rpp = 15, string isHistory = "0", string isViewAllergy = "", string isPrintAllergy = "", bool isFromCDS = false)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);


                dbManager.AddParameters(1, PARM_IS_HISTORY, "0");

                if (isFromCDS == true)
                {
                    dbManager.AddParameters(2, PARAM_ISCURRENT, true);
                }
                else
                {
                    dbManager.AddParameters(2, PARAM_ISCURRENT, isCurrent);
                }


                if (noteId == 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, noteId);

                if (pageNo == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNo);
                if (rpp == 0)
                    dbManager.AddParameters(5, PARM_RECORDS_PERPAGE, null);
                else
                    dbManager.AddParameters(5, PARM_RECORDS_PERPAGE, rpp);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.Medication.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_OP, ds, ds.Medication.TableName);

                if (ds.Medication.Rows.Count > 0)


                {
                    if (isHistory.Equals("1"))
                    {
                        dbManager.AddParameters(1, PARM_IS_HISTORY, isHistory);
                        DSClinicalMedication dsMedicationHistory = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_OP, ds, ds.MedicationHistory.TableName);
                        if (dsMedicationHistory != null)
                        {
                            ds.Merge(dsMedicationHistory);
                        }
                    }
                }

                DSClinicalMedication dsMdr = (DSClinicalMedication)loadMedicationsReviewedBy_Obsolete(patientId);
                ds.Merge(dsMdr);

                if (isFromCDS == true)
                {
                    dbManager.CommitTransaction();
                    return ds;
                }
                else
                {
                    if (ds.Medication.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(ds.Medication.Rows[0]["MedicationID"]) > 0)
                        {
                            if (dtTemp != null)
                            {
                                if (isViewAllergy == "1" || isPrintAllergy == "1")
                                {
                                    bool isViewAction = isViewAllergy == "1" ? true : false;
                                    bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                                    //Start 26-10-2016 Humaira Yousaf to add view action log against each item
                                    dtTemp.Columns.Add("PrimaryKey");
                                    dtTemp.Columns.Add("ViewAction");

                                    for (int i = 0; i < ds.Medication.Rows.Count; i++)
                                    {
                                        dtTemp.Rows[i]["PrimaryKey"] = ds.Medication.Rows[i][ds.Medication.MedicationIDColumn];
                                        dtTemp.Rows[i]["ViewAction"] = "View";
                                    }
                                    //End 26-10-2016 Humaira Yousaf to add view action log against each item

                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                    dsDBAudit.AcceptChanges();
                                }
                            }
                        }
                    }
                    dbManager.CommitTransaction();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsOp", PROC_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public Tuple<List<ClinicalMedicationsModel>, List<ClinicalMedicationHistoryModel>, List<ClinicalMedicationReviewModel>> loadMedicationsOp(long patientId, long noteId, bool isCurrent, int pageNo = 1, int rpp = 15, string isHistory = "0", string isViewAllergy = "", string isPrintAllergy = "", bool isFromCDS = false)
        {
            List<ClinicalMedicationsModel> clinicalMedicationList = new List<ClinicalMedicationsModel>();
            List<ClinicalMedicationHistoryModel> clinicalMedicationHistoryList = new List<ClinicalMedicationHistoryModel>();
            List<ClinicalMedicationReviewModel> clinicalMedicationReviewList = new List<ClinicalMedicationReviewModel>();

            Tuple<List<ClinicalMedicationsModel>, List<ClinicalMedicationHistoryModel>, List<ClinicalMedicationReviewModel>> tuple;

            //fixme
            //  DSDBAudit dsDBAudit = new DSDBAudit();
            //  DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                //fixme
                DataTable dtTemp = new DataTable("Medication");// ds.Medication;
                dbManager.Open();
                dbManager.BeginTransaction();
                //dbManager.CreateParameters(7);
                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(PARM_PATIENTID, patientId);

                dbManager.AddParameters(PARM_IS_HISTORY, "0");

                if (isFromCDS == true)
                {
                    dbManager.AddParameters(PARAM_ISCURRENT, true);
                }
                else
                {
                    dbManager.AddParameters(PARAM_ISCURRENT, isCurrent);
                }

                if (noteId == 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, noteId);

                if (pageNo == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, pageNo);
                if (rpp == 0)
                    dbManager.AddParameters(PARM_RECORDS_PERPAGE, null);
                else
                    dbManager.AddParameters(PARM_RECORDS_PERPAGE, rpp);

                dbManager.AddParameters(PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                clinicalMedicationList = dbManager.ExecuteReaderMapper<ClinicalMedicationsModel>(PROC_MEDICATION_SELECT_OP);
                if (clinicalMedicationList.Count > 0)

                {
                    if (isHistory.Equals("1"))
                    {
                        dbManager.AddUpdateParameterValue(PARM_IS_HISTORY, isHistory);
                        clinicalMedicationHistoryList = dbManager.ExecuteReaderMapper<ClinicalMedicationHistoryModel>(PROC_MEDICATION_SELECT_OP);
                    }
                }

                clinicalMedicationReviewList = loadMedicationsReviewedBy(patientId);

                if (isFromCDS == true)
                {
                    dbManager.CommitTransaction();
                }
                else
                {

                    if (clinicalMedicationList.Count > 0)
                    {
                        if (isViewAllergy == "1" || isPrintAllergy == "1")
                        {
                            bool isViewAction = isViewAllergy == "1" ? true : false;
                            bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                            new DBActivityAudit().InsertDBAuditAsync<ClinicalMedicationsModel>("Medication", clinicalMedicationList, clinicalMedicationList[0].MedicationID, null, "", isViewAction, isPrintAcion);
                        }
                        dbManager.CommitTransaction();
                    }

                    /* DB AUDIT DISABLED WHILE READING RECORDS 
                    if (clinicalMedicationList.Count > 0)
                    {
                        if (Convert.ToInt64(clinicalMedicationList[0].MedicationID) > 0)
                        {
                            if (dtTemp != null)
                            {
                                if (isViewAllergy == "1" || isPrintAllergy == "1")
                                {
                                    bool isViewAction = isViewAllergy == "1" ? true : false;
                                    bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                                    //   dtTemp.Columns.Add("PrimaryKey");
                                    //   dtTemp.Columns.Add("ViewAction");
                                    //   for (int i = 0; i < clinicalMedicationList.Count; i++)
                                    //    {
                                    //   dtTemp.Rows[i]["PrimaryKey"] = clinicalMedicationList[i].MedicationID;
                                    //  dtTemp.Rows[i]["ViewAction"] = "View";
                                    //}
                                    // dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                    // // dsDBAudit.AcceptChanges();
                                    //                        }
                                }
                            }
                        }
                        */


                }
                tuple = new Tuple<List<ClinicalMedicationsModel>, List<ClinicalMedicationHistoryModel>, List<ClinicalMedicationReviewModel>>(clinicalMedicationList, clinicalMedicationHistoryList, clinicalMedicationReviewList);
                return tuple;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsOp", PROC_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();

            }
            // return tuple;
        }

        public DSClinicalMedication loadAllMedicationsOp(long patientId, long noteId, bool isCurrent, string isHistory = "0", string isViewAllergy = "", string isPrintAllergy = "", bool isFromCDS = false)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(4);
                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);


                dbManager.AddParameters(1, PARM_IS_HISTORY, "0");

                if (isFromCDS == true)
                {
                    dbManager.AddParameters(2, PARAM_ISCURRENT, true);
                }
                else
                {
                    dbManager.AddParameters(2, PARAM_ISCURRENT, isCurrent);
                }


                if (noteId == 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, noteId);


                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_ALL, ds, ds.Medication.TableName);

                if (ds.Medication.Rows.Count > 0)
                {
                    if (isHistory.Equals("1"))
                    {
                        dbManager.AddParameters(1, PARM_IS_HISTORY, isHistory);
                        DSClinicalMedication dsMedicationHistory = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_ALL, ds, ds.MedicationHistory.TableName);
                        if (dsMedicationHistory != null)
                        {
                            ds.Merge(dsMedicationHistory);
                        }
                    }
                }

                if (isFromCDS == true)
                {
                    dbManager.CommitTransaction();
                    return ds;
                }
                else
                {
                    if (ds.Medication.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(ds.Medication.Rows[0]["MedicationID"]) > 0)
                        {
                            if (dtTemp != null)
                            {
                                if (isViewAllergy == "1" || isPrintAllergy == "1")
                                {
                                    bool isViewAction = isViewAllergy == "1" ? true : false;
                                    bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                                    //Start 26-10-2016 Humaira Yousaf to add view action log against each item
                                    dtTemp.Columns.Add("PrimaryKey");
                                    dtTemp.Columns.Add("ViewAction");

                                    for (int i = 0; i < ds.Medication.Rows.Count; i++)
                                    {
                                        dtTemp.Rows[i]["PrimaryKey"] = ds.Medication.Rows[i][ds.Medication.MedicationIDColumn];
                                        dtTemp.Rows[i]["ViewAction"] = "View";
                                    }
                                    //End 26-10-2016 Humaira Yousaf to add view action log against each item

                                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                    dsDBAudit.AcceptChanges();
                                }
                            }
                        }
                    }
                    dbManager.CommitTransaction();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadAllMedicationsOp", PROC_MEDICATION_SELECT_ALL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication loadMedicationsForCDS(string MedLookupName)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                if (!string.IsNullOrEmpty(MedLookupName))
                {
                    dbManager.AddParameters(0, "@MedSearch", MedLookupName);

                    ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_FOR_CDSMedication, ds, ds.MedicationCDS.TableName);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_DRUG_ID, null);

                    ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_FOR_CDS, ds, ds.MedicationCDS.TableName);
                }
               

                dbManager.CommitTransaction();
                return ds;
            }

            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsForCDS", PROC_MEDICATION_SELECT_FOR_CDS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Author: ZeeshanAK
        /// Purpose:  Get reviewed info by Patient ID
        /// Date : January 25, 2016
        public DSClinicalMedication loadMedicationsReviewedBy_Obsolete(long patientId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);




                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_REVIEW_SELECT, ds, ds.MedicationReview.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsReviewedBy", PROC_MEDICATION_REVIEW_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public String UpdateNegationReasonIdByMedicationId(string MedicationId, string NegationReasonId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = "";
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (string.IsNullOrEmpty(NegationReasonId))
                    dbManager.AddParameters(0, PARM_NEGATIONREASONID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_NEGATIONREASONID, NegationReasonId);
                dbManager.AddParameters(1, PARM_MEDICATION_ID, MedicationId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEDICATION_NEGATIONREASONID_UPDATE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::UpdateNegationReasonIdByMedicationId", PROC_MEDICATION_NEGATIONREASONID_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public String UpdateRouteIdByMedicationId(string MedicationId, string RouteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = "";
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (string.IsNullOrEmpty(RouteId))
                    dbManager.AddParameters(0, PARM_ROUTEID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_ROUTEID, RouteId);
                dbManager.AddParameters(1, PARM_MEDICATION_ID, MedicationId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MEDICATION_ROUTEID_UPDATE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::UpdateRouteIdByMedicationId", PROC_MEDICATION_ROUTEID_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ClinicalMedicationReviewModel> loadMedicationsReviewedBy(long patientId)
        {
            List<ClinicalMedicationReviewModel> clinicalMedicationReviewModelList = new List<ClinicalMedicationReviewModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                // dbManager.CreateParameters(1);

                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(PARM_PATIENTID, patientId);

                clinicalMedicationReviewModelList = dbManager.ExecuteReaderMapper<ClinicalMedicationReviewModel>(PROC_MEDICATION_REVIEW_SELECT);
                return clinicalMedicationReviewModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsReviewedBy", PROC_MEDICATION_REVIEW_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalMedication getLatestMedicationsByPatientId(long patientId, long userId, long entityId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENTID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENTID, patientId);
                if (userId <= 0)
                    dbManager.AddParameters(1, PARM_USERID, null);
                else
                    dbManager.AddParameters(1, PARM_USERID, userId);
                if (entityId <= 0)
                    dbManager.AddParameters(2, PARM_ENTITYID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITYID, entityId);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_MED_BY_PATIENTID, ds, ds.Medication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getLatestMedicationsByPatientId", PROC_GET_LATEST_MED_BY_PATIENTID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MedicationModel> GetNoteMedicationsByPatientId(long patientId, long userId, long entityId, long noteId, long OrderSetId)
        {
            List<MedicationModel> modelList = new List<MedicationModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_PATIENT_ID, patientId));
                parameters.Add(new SqlParameter(PARM_USERID, userId));
                parameters.Add(new SqlParameter(PARM_ENTITYID, entityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, noteId));
                if (OrderSetId > 0)
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, OrderSetId));
                }
                else
                {
                    parameters.Add(new SqlParameter(PARM_ORDERSET_ID, DBNull.Value));
                }

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_MED_FORSOAPTEXT, parameters))
                {
                    while (reader.Read())
                    {
                        MedicationModel model = new MedicationModel();

                        var properties = typeof(MedicationModel).GetProperties();

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

                        modelList.Add(model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::GetNoteMedicationsByPatientId", PROC_GET_LATEST_MED_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : function to insert Medications.
        /// Date : 14 january 2016

        public DSClinicalMedication GetExsistingPatients(string PatientIds)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (PatientIds == "")
                    dbManager.AddParameters(0, PARM_PATIENT_IDS, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_IDS, PatientIds);




                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_EXSISTING_PATIENTS, ds, ds.Patients.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::GetExsistingPatients", PROC_GET_EXSISTING_PATIENTS, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::GetExsistingPatients", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "Insert Medication"
        public DSClinicalMedication insertMedication(DSClinicalMedication ds, SharedVariable sharedVariable = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Medication.GetChanges();
                // dbManager.Open();
                DSClinicalMedication dsOldDataMedication = new DSClinicalMedication();

                for (int i = 0; i < ds.Medication.Rows.Count; i++)
                {
                    string RcopiaID = ds.Medication[i]["RcopiaID"].ToString();
                    if (RcopiaID != "")
                    {
                        var asas = getMedicationByRcopiaID(RcopiaID);
                        dsOldDataMedication.Merge(asas);
                        //dsOldDataAllergy.Allergy.Merge(getAllergyByRcopiaID(Convert.ToInt64(RcopiaID)).Tables["Allergy"]);
                    }
                    string entityId = "";
                    entityId = sharedVariable != null ? sharedVariable.EntityId : MDVSession.Current.EntityId;
                    ds.Medication.Rows[i]["EntityId"] = entityId;
                }

                DataTable dt = dsOldDataMedication.Tables["Medication"];

                dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MEDICATION_INSERT, ds, ds.Medication.TableName);
                DataRow row;
                for (int i = 0; i < ds.Medication.Rows.Count; i++)
                {
                    if (Boolean.Parse((ds.Medication.Rows[i]["IsNewRow"]).ToString()) == true)
                    {
                        row = dt.NewRow();
                        row = ds.Medication.Rows[i];
                        dt.Rows.Add(row.ItemArray);
                    }
                    else if (Boolean.Parse((ds.Medication.Rows[i]["IsNewRow"]).ToString()) == false)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["RcopiaID"].ToString() == ds.Medication.Rows[i]["RcopiaID"].ToString())
                            {
                                dt.Rows[j]["RcopiaID"] = ds.Medication.Rows[i]["RcopiaID"];
                                dt.Rows[j]["StartDate"] = ds.Medication.Rows[i]["StartDate"];
                                dt.Rows[j]["Action"] = ds.Medication.Rows[i]["Action"];
                                dt.Rows[j]["Dose"] = ds.Medication.Rows[i]["Dose"];
                                dt.Rows[j]["DoseUnit"] = ds.Medication.Rows[i]["DoseUnit"];
                                dt.Rows[j]["Routeby"] = ds.Medication.Rows[i]["Routeby"];
                                dt.Rows[j]["DoseTiming"] = ds.Medication.Rows[i]["DoseTiming"];
                                dt.Rows[j]["DoseOther"] = ds.Medication.Rows[i]["DoseOther"];
                                dt.Rows[j]["Duration"] = ds.Medication.Rows[i]["Duration"];
                                dt.Rows[j]["Routeby"] = ds.Medication.Rows[i]["Routeby"];
                                dt.Rows[j]["Quantity"] = ds.Medication.Rows[i]["Quantity"];
                                dt.Rows[j]["QuantityUnit"] = ds.Medication.Rows[i]["QuantityUnit"];
                                dt.Rows[j]["OtherNote"] = ds.Medication.Rows[i]["OtherNote"];
                                dt.Rows[j]["PatientNotes"] = ds.Medication.Rows[i]["PatientNotes"];
                                dt.Rows[j]["Comments"] = ds.Medication.Rows[i]["Comments"];
                                dt.Rows[j]["IsActive"] = ds.Medication.Rows[i]["IsActive"];
                                dt.Rows[j]["IsDeleted"] = ds.Medication.Rows[i]["IsDeleted"];
                                dt.Rows[j]["Refill"] = ds.Medication.Rows[i]["Refill"];
                                //Start 17-10-2016 Humaira Yousaf to log stop date of medication
                                if (ds.Medication.Rows[i]["StopDate"].ToString() != "")
                                {
                                    dt.Rows[j]["StopDate"] = ds.Medication.Rows[i]["StopDate"];
                                }
                                //End 17-10-2016 Humaira Yousaf to log stop date of medication
                            }
                        }
                    }
                }
                dtTemp = ds.Medication.GetChanges();

                if (dt != null)
                {
                    //Start 21-10-2016 Edit By Humaira Yousaf Bug# QAC2-621
                    dt.Columns.Add("PrimaryKey");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["PrimaryKey"] = dt.Rows[i]["MedicationID"];
                    }
                    //End 21-10-2016 Edit By Humaira Yousaf Bug# QAC2-621
                    if (sharedVariable == null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dt, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString());
                    }
                    else
                    {
                        dsDBAudit = new DBActivityAudit(sharedVariable).InsertDBAudit(dt, dbManager, ds.Medication.Rows[0][ds.Medication.MedicationIDColumn].ToString(), null, "", false, false, false, "", "0", sharedVariable);
                    }
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
                //MDVLogger.DALErrorLog("DALMedications::insertMedication", PROC_MEDICATION_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::insertMedication", null);
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

        //Get Medication by RcopiaID
        public DSClinicalMedication getMedicationByRcopiaID(string RcopiaID)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (RcopiaID == "")
                    dbManager.AddParameters(0, PARM_RCOPIAID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RCOPIAID, RcopiaID);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, GET_MEDICATION_BY_RCOPIAID, ds, ds.Medication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALAllergies::getMedicationByRcopiaID", GET_MEDICATION_BY_RCOPIAID, ex);
                MDVLogger.SendExcepToDB(ex, "DALAllergies::getMedicationByRcopiaID", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalMedication InsertMedicationReviews(DSClinicalMedication ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertReviews(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_MEDICATION_REVIEW, ds, ds.MedicationReview.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::InsertMedicationReviews", PROC_INSERT_MEDICATION_REVIEW, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::InsertMedicationReviews", null);
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
        /// <summary>
        /// Author : Wasim Malik
        /// Purpose : function insert Prescription.
        /// Date : 14 january 2016

        #region "Insert Prescription"
        public DSClinicalMedication InsertPrescription(DSClinicalMedication ds, SharedVariable sharedVariable = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                DataTable dtTemp = ds.Prescription.GetChanges();
                // dbManager.Open();
                DSClinicalMedication dsOldDataPrescription = new DSClinicalMedication();

                for (int i = 0; i < ds.Prescription.Rows.Count; i++)
                {
                    string RcopiaID = ds.Prescription[i]["RcopiaID"].ToString();
                    if (RcopiaID != "")
                    {
                        var asas = getPrescriptionByRcopiaID(RcopiaID);
                        dsOldDataPrescription.Merge(asas);
                        //dsOldDataAllergy.Allergy.Merge(getAllergyByRcopiaID(Convert.ToInt64(RcopiaID)).Tables["Allergy"]);
                    }
                }

                DataTable dt = dsOldDataPrescription.Tables["Prescription"];

                dbManager.BeginTransaction();

                CreateParametersPrescription(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PRESCRIPTION_INSERT, ds, ds.Prescription.TableName);
                DataRow row;
                for (int i = 0; i < ds.Prescription.Rows.Count; i++)
                {
                    if (Boolean.Parse((ds.Prescription.Rows[i]["IsNewRow"]).ToString()) == true)
                    {
                        row = dt.NewRow();
                        row = ds.Prescription.Rows[i];
                        dt.Rows.Add(row.ItemArray);
                    }
                    else if (Boolean.Parse((ds.Prescription.Rows[i]["IsNewRow"]).ToString()) == false)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["RcopiaID"].ToString() == ds.Prescription.Rows[i]["RcopiaID"].ToString())
                            {
                                dt.Rows[j]["RcopiaID"] = ds.Prescription.Rows[i]["RcopiaID"];
                                dt.Rows[j]["CreatedDate"] = ds.Prescription.Rows[i]["CreatedDate"];
                                dt.Rows[j]["Action"] = ds.Prescription.Rows[i]["Action"];
                                dt.Rows[j]["Dose"] = ds.Prescription.Rows[i]["Dose"];
                                dt.Rows[j]["DoseUnit"] = ds.Prescription.Rows[i]["DoseUnit"];
                                dt.Rows[j]["Routeby"] = ds.Prescription.Rows[i]["Routeby"];
                                dt.Rows[j]["DoseTiming"] = ds.Prescription.Rows[i]["DoseTiming"];
                                dt.Rows[j]["DoseOther"] = ds.Prescription.Rows[i]["DoseOther"];
                                dt.Rows[j]["Duration"] = ds.Prescription.Rows[i]["Duration"];
                                dt.Rows[j]["Routeby"] = ds.Prescription.Rows[i]["Routeby"];
                                dt.Rows[j]["Quantity"] = ds.Prescription.Rows[i]["Quantity"];
                                dt.Rows[j]["QuantityUnit"] = ds.Prescription.Rows[i]["QuantityUnit"];
                                dt.Rows[j]["OtherNotes"] = ds.Prescription.Rows[i]["OtherNotes"];
                                dt.Rows[j]["PatientNotes"] = ds.Prescription.Rows[i]["PatientNotes"];
                                dt.Rows[j]["Comments"] = ds.Prescription.Rows[i]["Comments"];
                                dt.Rows[j]["IsActive"] = ds.Prescription.Rows[i]["IsActive"];
                                dt.Rows[j]["IsDeleted"] = ds.Prescription.Rows[i]["IsDeleted"];
                                dt.Rows[j]["Refill"] = ds.Prescription.Rows[i]["Refill"];
                                dt.Rows[j]["PharmacyID"] = ds.Prescription.Rows[i]["PharmacyID"];
                                dt.Rows[j]["CompletionAction"] = ds.Prescription.Rows[i]["CompletionAction"];
                                dt.Rows[j]["ProviderID"] = ds.Prescription.Rows[i]["ProviderID"];
                            }
                        }
                    }
                }
                dtTemp = ds.Prescription.GetChanges();

                if (dt != null)
                {
                    if (sharedVariable == null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dt, dbManager, ds.Prescription.Rows[0][ds.Prescription.PrescriptionIDColumn].ToString());
                    }
                    else
                    {
                        dsDBAudit = new DBActivityAudit(sharedVariable).InsertDBAudit(dt, dbManager, ds.Prescription.Rows[0][ds.Prescription.PrescriptionIDColumn].ToString(), null, "", false, false, false, "", "0", sharedVariable);
                    }
                    
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::PROC_PRESCRIPTION_INSERT", PROC_PRESCRIPTION_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::PROC_PRESCRIPTION_INSERT", null);
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


        public string DeletePrescription(string RcopiaID, SharedVariable sharedVariable=null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
              //  dbManager.BeginTransaction();
                DSClinicalMedication dsprescription = loadPrescriptions(0, 0, 0, 1, 15, "0", "0", RcopiaID);

                if (dsprescription.Prescription.Rows.Count > 0)
                {
                    dbManager.CreateParameters(2);
                    var prescriptionId = MDVUtility.ToLong(dsprescription.Prescription.Rows[0][dsprescription.Prescription.PrescriptionIDColumn.ColumnName]);
                    dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, prescriptionId);
                    dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PRESCRIPTION_DELETE).ToString();

                    if (returnVal != "")
                        throw new Exception(returnVal);
                    else
                    {

                        DataTable dtTemp = dsprescription.Prescription;//.GetChanges();
                        if (dtTemp != null)
                        {
                            if (sharedVariable == null)
                            {
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, prescriptionId.ToString(), null, prescriptionId.ToString(), false, false, true);
                            }
                            else
                            {
                                dsDBAudit = new DBActivityAudit(sharedVariable).InsertDBAudit(dtTemp, dbManager, prescriptionId.ToString(), null, prescriptionId.ToString(), false, false, true, "", "0", sharedVariable);
                            }
                            
                            dsDBAudit.AcceptChanges();
                        }
                        //dbManager.CommitTransaction();
                    }
                }
                else
                {

                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();

                //MDVLogger.DALErrorLog(sharedVariable,"DALMedication::DeletePrescription", PROC_PRESCRIPTION_DELETE, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedication::DeletePrescription", null);
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


        public DSNotes AttachWithWhichNote(string RcopiaID)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSNotes ds = new DSNotes();
                dbManager.Open();
                // dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_RCOPIA, RcopiaID);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_WITH_WHICH_NOTE, ds, ds.Notes.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::AttachWithWhichNote", PROC_ATTACH_WITH_WHICH_NOTE, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::AttachWithWhichNote", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Get Prescription by RcopiaID
        public DSClinicalMedication getPrescriptionByRcopiaID(string RcopiaID)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (RcopiaID == "")
                    dbManager.AddParameters(0, PARM_RCOPIAID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RCOPIAID, RcopiaID);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, GET_PRESCRIPTION_BY_RCOPIAID, ds, ds.Prescription.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALAllergies::getPrescriptionByRcopiaID", GET_PRESCRIPTION_BY_RCOPIAID, ex);
                MDVLogger.SendExcepToDB(ex, "DALAllergies::getPrescriptionByRcopiaID", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert Drug"
        public DSClinicalMedication insertDrug(DSClinicalMedication ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersDrug(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DRUG_INSERT, ds, ds.Drug.TableName);
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::PROC_DRUG_INSERT", PROC_DRUG_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::PROC_DRUG_INSERT", null);
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

        //CreatePharamacyParameter

        #region "Insert Pharamacy"
        public DSClinicalMedication InsertPharamacy(DSClinicalMedication ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePharamacyParameter(dbManager, ds, true);
                ds = (DSClinicalMedication)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHARAMACY_INSERT, ds, ds.Pharmacy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::PROC_PHARAMACY_INSERT", PROC_PHARAMACY_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::PROC_PHARAMACY_INSERT", null);
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
        /// Purpose:  Getting Dataset of Medications Soap Text
        /// Date : January 15, 2016
        #region "Medications For Notes Soap"
        public DSClinicalMedication loadMedicationsForSoap(string medicationID, long patientId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MEDICATION_ID, medicationID);
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECTT_FORSOAPTEXT_OP, ds, ds.Medication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedicationsForSoap", PROC_MEDICATION_SELECTT_FORSOAPTEXT_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose:  Attach Medications with Notes
        /// Date : January 15, 2016
        #region "Attach Medications With Notes"
        public DSClinicalMedication attachMedicationsWithNotes(string medicationID, long notesID)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSClinicalMedication ds = new DSClinicalMedication();

                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MEDICATION_ID, medicationID);
                dbManager.AddParameters(1, PARM_NOTE_ID, notesID);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_MEDICATION_WITH_NOTES_OP, ds, ds.Medication.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::attachMedicationWithNotes", PROC_ATTACH_MEDICATION_WITH_NOTES_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose:  Detach Medications with Notes
        /// Date : January 18, 2016
        #region "Detach Medications From Notes"
        public string detachMedicationsFromNotes(string medicationID, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MEDICATION_ID, medicationID);
                dbManager.AddParameters(1, PARM_NOTE_ID, notesId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_MEDICATION_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::detachMedicationWithNotes", PROC_DETACH_MEDICATION_FROM_NOTES, ex);
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
        #region "Prescriptions For Notes Soap"


        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to load Medications For Soap.
        /// Date : 18 january 2016
        /// </summary>
        /// <param name="prescriptionId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public DSClinicalMedication loadPrescriptionsForSoap(string prescriptionId, long noteId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, prescriptionId);
                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRESCRIPTION_SELECT_FORSOAPTEXT_OP, ds, ds.Prescription.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadPrescriptionsForSoap", PROC_PRESCRIPTION_SELECT_FORSOAPTEXT_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<Model.Clinical.Notes.PrescriptionModel> GetNotePrescriptionByPatientId(long PatientId, long noteId)
        {
            List<PrescriptionModel> modelList = new List<PrescriptionModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_NOTES_ID, noteId));

                using (var reader = dbManager.ExecuteReader(PROC_GET_LATEST_PRESCRIPTION_FORSOAPTEXT, parameters))
                {
                    while (reader.Read())
                    {
                        PrescriptionModel model = new PrescriptionModel();

                        var properties = typeof(PrescriptionModel).GetProperties();

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

                        modelList.Add(model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::GetNotePrescriptionByPatientId", PROC_GET_LATEST_PRESCRIPTION_FORSOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to get Latest Prescription For Soap.
        /// Date : 18 january 2016
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public DSClinicalMedication getLatestPrescriptionByPatientId(long PatientId, long noteId)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (noteId == 0)
                    dbManager.AddParameters(1, PARM_NOTES_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_NOTES_ID, noteId);

                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_PRESCRIPTION_BY_PATIENTID, ds, ds.Prescription.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::getLatestPrescriptionByPatientId", PROC_GET_LATEST_PRESCRIPTION_BY_PATIENTID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region Attach and Detach Prescriptions
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to detach Prescriptions from notes.
        /// Date : 18 january 2016
        /// </summary>
        /// <param name="prescriptionId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        public string detachPrescriptionsFromNotes(string prescriptionId, long notesId, string RcopiaId = "")
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);

                if (prescriptionId == "")
                {
                    dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, prescriptionId);
                }

                if (notesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTES_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTES_ID, notesId);
                }
                if (RcopiaId == "")
                {
                    dbManager.AddParameters(2, PARM_RCOPIA, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_RCOPIA, RcopiaId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PRESCRIPTION_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALMedications::detachPrescriptionsFromNotes", PROC_DETACH_PRESCRIPTION_FROM_NOTES, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::detachPrescriptionsFromNotes", null);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to attach prescriptions with notes.
        /// Date : 18 january 2016
        /// </summary>
        /// <param name="prescriptionId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        public DSClinicalMedication attachPrescriptionsWithNotes(string prescriptionId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSClinicalMedication ds = new DSClinicalMedication();

                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_PRESCRIPTION_ID, prescriptionId);
                dbManager.AddParameters(1, PARM_NOTES_ID, notesId);



                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PRESCRIPTION_WITH_NOTES_OP, ds, ds.Prescription.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::attachPrescriptionsWithNotes", PROC_ATTACH_PRESCRIPTION_WITH_NOTES_OP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        /// Author: ZeeshanAK
        /// Purpose:  to load Medications for Batch Patient List
        /// Date : April 06, 2016
        public DSClinicalMedication LookupMedications(int patientId, int mdedicationId = -1, string medicationName = null)
        {
            DSClinicalMedication ds = new DSClinicalMedication();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (patientId <= 0)
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                }
                if (mdedicationId <= 0)
                {
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_MEDICATION_ID, mdedicationId);
                }
                if (string.IsNullOrEmpty(medicationName))
                {
                    dbManager.AddParameters(2, PARM_MEDICATION_NAME, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_MEDICATION_NAME, medicationName);
                }
                ds = (DSClinicalMedication)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MEDICATION_SELECT_LOOKUP, ds, ds.Medication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadMedications", PROC_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MedicationLookupModel> LookupMedicationsReprot()
        {
            List<MedicationLookupModel> listModel = new List<MedicationLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MEDICATION_REPORT_LOOKUP);
                MedicationLookupModel modelFill = null;
                while (reader.Read())
                {
                    modelFill = new MedicationLookupModel();
                    modelFill.MedicationID = Convert.ToInt64(reader["MedicationID"]);
                    modelFill.MedicationName = MDVUtility.CheckStringNull(reader["MedicationName"]);
                    listModel.Add(modelFill);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LookupMedicationsReprot", PROC_MEDICATION_REPORT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<CustomModel> LoadMedicationRoutesLookUp()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                return dbManager.ExecuteReaders<CustomModel>(PROC_MEDICATION_ANTIMICROBIAL_ROUTE_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatient::LoadMedicationRoutesLookUp", PROC_MEDICATION_ANTIMICROBIAL_ROUTE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private const string PROC_VACCINE_REFUSAL_REASON_LOOKUP = "Clinical.sp_VaccineRefusalReasonLookup";

        public List<CustomModel> LookupNegationReason()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                return dbManager.ExecuteReaders<CustomModel>(PROC_MEDICATION_NEGATION_REASON_LOOKUP);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::LookupNegationReason", PROC_MEDICATION_NEGATION_REASON_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region Legacy Notes

        public List<MedicationHx> NotesMedicationsSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MedicationHx> objList_MedicationHx = new List<MedicationHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_MEDICATION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        MedicationHx model = new MedicationHx();
                        var properties = typeof(MedicationHx).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                                object safeValue = (reader[prop.Name] == null) ? null : Convert.ChangeType(reader[prop.Name], t);

                                prop.SetValue(model, safeValue, null);
                                //prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_MedicationHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::NotesMedicationsSelect", PROC_NOTES_MEDICATION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_MedicationHx;
        }

        #endregion Legacy Notes

    }
}
