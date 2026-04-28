using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{

    public class ClinicalMedicationsModel : IBaseModel
    {

        public string MedicationID { get; set; }
        public string RcopiaID { get; set; }
        public string PatientID { get; set; }
        public string PrescriptionID { get; set; }
        public string ProviderID { get; set; }
        public string Preparer_UserID { get; set; }
        public string DrugID { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public string Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Substitution { get; set; }
        public string OtherNote { get; set; }
        public string PatientNotes { get; set; }
        public string Comments { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string StopReason { get; set; }
        public string SigChangedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string IntendedUse { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
        public string IsNoteLinked { get; set; }
        public string MedicationName { get; set; }
        public string ProviderName { get; set; }
        public string PrescriptionRcopiaID { get; set; }
        public string Refill { get; set; }
        public string FillDate { get; set; }
        public string IsDeleted { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewedBy { get; set; }
        public string RxnormID { get; set; }
        public string RxnormIDType { get; set; }
        public string IsNewRow { get; set; }
        public string DrugDescription { get; set; }
        public string ICD10 { get; set; }
        public string Renew { get; set; }
        public string NDCID { get; set; }
        public string RouteId { get; set; }
        public string NegationReasonId { get; set; }
        public string PrimaryKey { get { return MedicationID; } }
        public string ViewAction { get { return "View"; } }


        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            MedicationID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationID", incommingColumnList));
            RcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaID", incommingColumnList));
            PatientID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientID", incommingColumnList));
            PrescriptionID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PrescriptionID", incommingColumnList));
            ProviderID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderID", incommingColumnList));
            Preparer_UserID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Preparer_UserID", incommingColumnList));
            DrugID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugID", incommingColumnList));
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
            OtherNote = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OtherNote", incommingColumnList));

            PatientNotes = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientNotes", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            StartDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StartDate", incommingColumnList));
            StopDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StopDate", incommingColumnList));
            StopReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StopReason", incommingColumnList));
            SigChangedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SigChangedDate", incommingColumnList));
            LastModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastModifiedBy", incommingColumnList));
            LastModifiedDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastModifiedDate", incommingColumnList));
            IntendedUse = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IntendedUse", incommingColumnList));
            Number = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Number", incommingColumnList));
            Status = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Status", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            IsNoteLinked = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsNoteLinked", incommingColumnList));

            MedicationName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationName", incommingColumnList));
            ProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderName", incommingColumnList));
            PrescriptionRcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PrescriptionRcopiaID", incommingColumnList));
            Refill = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Refill", incommingColumnList));
            FillDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FillDate", incommingColumnList));
            IsDeleted = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsDeleted", incommingColumnList));
            ReviewedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedOn", incommingColumnList));
            ReviewedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedBy", incommingColumnList));
            RxnormID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormID", incommingColumnList));
            RxnormIDType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormIDType", incommingColumnList));
            IsNewRow = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsNewRow", incommingColumnList));
            DrugDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugDescription", incommingColumnList));
            ICD10 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD10", incommingColumnList));
            Renew = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Renew", incommingColumnList));
            NDCID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NDCID", incommingColumnList));
            RouteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RouteId", incommingColumnList));
            NegationReasonId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NegationReasonId", incommingColumnList));
        }
    }


}
