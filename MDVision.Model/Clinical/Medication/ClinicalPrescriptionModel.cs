using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{
	
	public class ClinicalPrescriptionModel:IBaseModel
	{
		 
		public long PrescriptionID	{ get; set; }
		public string RcopiaID	{ get; set; }
		public string PatientID	{ get; set; }
		public string ProviderID	{ get; set; }
		public string Preparer_UserID	{ get; set; }
		public string PharmacyID	{ get; set; }
		public string CreatedDate	{ get; set; }
		public string CompletedDate	{ get; set; }
		public string SignedDate	{ get; set; }
		public string StopDate	{ get; set; }
		public string LastModifiedBy	{ get; set; }
		public string LastModifiedDate	{ get; set; }
		public string SendMethod	{ get; set; }
		public string Refill	{ get; set; }
		public string Cancelleddate	{ get; set; }
		public string IsActive	{ get; set; }
		public string CreatedBy	{ get; set; }
		public string CreatedOn	{ get; set; }
		public string ModifiedBy	{ get; set; }
		public string ModifiedOn	{ get; set; }
		public string RecordCount	{ get; set; }
		public string IsNoteLinked	{ get; set; }
		public string ProviderName	{ get; set; }
		public string MedicationName	{ get; set; }
		public string IsDeleted	{ get; set; }
		public string Voided	{ get; set; }
		public string Denied	{ get; set; }
		public string SubstitutionPermitted	{ get; set; }
		public string OtherNotes	{ get; set; }
		public string PatientNotes	{ get; set; }
		public string Comments	{ get; set; }
		public string PharmacyZip	{ get; set; }
		public string PharmacyState	{ get; set; }
		public string PharmacyCity	{ get; set; }
		public string PharmacyAddress	{ get; set; }
		public string CompletionAction	{ get; set; }
		public string IntendedUse	{ get; set; }
		public string Action	{ get; set; }
		public string Dose	{ get; set; }
		public string DoseUnit	{ get; set; }
		public string Routeby	{ get; set; }
		public string DoseTiming	{ get; set; }
		public string DoseOther	{ get; set; }
		public string Duration	{ get; set; }
		public string Quantity	{ get; set; }
		public string QuantityUnit	{ get; set; }
		public string Substitution	{ get; set; }
		public string DrugRcopiaID	{ get; set; }
		public string PharmacyRcopiaId	{ get; set; }
		public string IsNewRow	{ get; set; }
		public string DrugDescription	{ get; set; }
		public string NoteId	{ get; set; }
        public string NDCID { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            PrescriptionID = ModelUtility.ToInt64(ModelUtility.MapValue(reader, "PrescriptionID", incommingColumnList));
            RcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaID", incommingColumnList));
            PatientID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientID", incommingColumnList));
            ProviderID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderID", incommingColumnList));
            Preparer_UserID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Preparer_UserID", incommingColumnList));
            PharmacyID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyID", incommingColumnList));
            CreatedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedDate", incommingColumnList));
            CompletedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CompletedDate", incommingColumnList));
            SignedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SignedDate", incommingColumnList));
            StopDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StopDate", incommingColumnList));
            LastModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastModifiedBy", incommingColumnList));
            LastModifiedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastModifiedDate", incommingColumnList));
            SendMethod = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SendMethod", incommingColumnList));
            Refill = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Refill", incommingColumnList));
            Cancelleddate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Cancelleddate", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            IsNoteLinked = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsNoteLinked", incommingColumnList));
            ProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderName", incommingColumnList));
            MedicationName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationName", incommingColumnList));
            IsDeleted = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsDeleted", incommingColumnList));
            Voided = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Voided", incommingColumnList));
            Denied = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Denied", incommingColumnList));
            SubstitutionPermitted = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SubstitutionPermitted", incommingColumnList));
            OtherNotes = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OtherNotes", incommingColumnList));
            PatientNotes = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientNotes", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            PharmacyZip = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyZip", incommingColumnList));
            PharmacyState = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyState", incommingColumnList));
            PharmacyCity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyCity", incommingColumnList));
            PharmacyAddress = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyAddress", incommingColumnList));
            CompletionAction = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CompletionAction", incommingColumnList));
            IntendedUse = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IntendedUse", incommingColumnList));
            Action = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Action", incommingColumnList));
            Dose = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Dose", incommingColumnList));
            DoseUnit = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DoseUnit", incommingColumnList));
            Routeby = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Routeby", incommingColumnList));
            DoseTiming = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DoseTiming", incommingColumnList));
            DoseOther = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DoseOther", incommingColumnList));
            Duration = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Duration", incommingColumnList));
            Quantity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Quantity", incommingColumnList));
            QuantityUnit = ModelUtility.ToStr(ModelUtility.MapValue(reader, "QuantityUnit", incommingColumnList));
            Substitution = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Substitution", incommingColumnList));
            DrugRcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugRcopiaID", incommingColumnList));
            PharmacyRcopiaId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyRcopiaId", incommingColumnList));
            IsNewRow = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsNewRow", incommingColumnList));
            DrugDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugDescription", incommingColumnList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", incommingColumnList));
            NDCID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NDCID", incommingColumnList));
        }
    }
}
