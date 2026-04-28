using MDVision.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Notes.Notes
{
    public class Notes : IBaseModel
    {
        public Notes()
        {
        }
        public string NotesId { get; set; }
        public string Description { get; set; }
        public string NoteText { get; set; }
        public string TemplateTypeId { get; set; }
        public string TemplateId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string NoteStatus { get; set; }
        public string RecordCount { get; set; }
        public string PatientId { get; set; }
        public string AppointmentId { get; set; }
        public string PrevNoteDescription { get; set; }
        public string VisitReason { get; set; }
        public string ProviderName { get; set; }
        public string ProviderId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string RoomNo { get; set; }
        public string RefProviderName { get; set; }
        public string RefProviderId { get; set; }
        public string LinkedAppointment { get; set; }
        public string EntityId { get; set; }
        public string ShortName { get; set; }
        public string VisitReasonId { get; set; }
        public string RoomsId { get; set; }
        public string PatientName { get; set; }
        public string VisitId { get; set; }
        public string ChiefComplaint { get; set; }
        public string Comments { get; set; }
        public string SignedBy { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateName { get; set; }
        public string PrevNotesId { get; set; }
        public string VisitType { get; set; }
        public string FacilityPOSCode { get; set; }
        public string FacilityPOSDesc { get; set; }
        public string BillingInfoId { get; set; }
        public string PatientTypeId { get; set; }
        public string SchReasonId { get; set; }
        public string IsPhoneEncounter { get; set; }
        public string Duration { get; set; }
        public string CPTCode { get; set; }
        public string DraftNotesCount { get; set; }
        public string SignedNotesCount { get; set; }
        public string bMedReconciled { get; set; }
        public string MedReconciledId { get; set; }
        public string HxtabOrder { get; set; }
        public string ComeFromCopyNote { get; set; }
        public string IsNonBilable { get; set; }
        public string VisitReasonComments { get; set; }
        public string ROSDataTemptId { get; set; }
        public string PEDataTemptId { get; set; }
        public string PETemplateId { get; set; }
        public string ROSTemplateId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string EncounterType { get; set; }
        public string Caller { get; set; }
        public string Receiver { get; set; }
        public string EncounterTypeName { get; set; }
        public string UnSignedStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string IsReviewed { get; set; }
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceProviderId { get; set; }
        public string ResourceProviderName { get; set; }
        public string NoteType { get; set; }
        public string RoomName { get; set; }
        public string NoteTempType { get; set; }
        public string POS { get; set; }
        public string AppointmentDate { get; set; }
        public string NoteDate { get; set; }
        public string OrderSetId { get; set; }
        public string OrderSetName { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            NotesId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NotesId", incommingColumnList));
            Description = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Description", incommingColumnList));
            NoteText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteText", incommingColumnList));
            TemplateTypeId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemplateTypeId", incommingColumnList));
            TemplateId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemplateId", incommingColumnList));
            VisitDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitDate", incommingColumnList));
            VisitTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitTime", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            NoteStatus = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteStatus", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            AppointmentId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AppointmentId", incommingColumnList));
            PrevNoteDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PrevNoteDescription", incommingColumnList));
            VisitReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitReason", incommingColumnList));
            ProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderName", incommingColumnList));
            ProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderId", incommingColumnList));
            FacilityName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FacilityName", incommingColumnList));
            FacilityId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FacilityId", incommingColumnList));
            RoomNo = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RoomNo", incommingColumnList));
            RefProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RefProviderName", incommingColumnList));
            RefProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RefProviderId", incommingColumnList));
            LinkedAppointment = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LinkedAppointment", incommingColumnList));
            EntityId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EntityId", incommingColumnList));
            ShortName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ShortName", incommingColumnList));
            VisitReasonId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitReasonId", incommingColumnList));
            RoomsId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RoomsId", incommingColumnList));
            PatientName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientName", incommingColumnList));
            VisitId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitId", incommingColumnList));
            ChiefComplaint = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ChiefComplaint", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            SignedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SignedBy", incommingColumnList));
            TemplateTypeName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemplateTypeName", incommingColumnList));
            TemplateName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemplateName", incommingColumnList));
            PrevNotesId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PrevNotesId", incommingColumnList));
            VisitType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitType", incommingColumnList));
            FacilityPOSCode = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FacilityPOSCode", incommingColumnList));
            FacilityPOSDesc = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FacilityPOSDesc", incommingColumnList));
            BillingInfoId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BillingInfoId", incommingColumnList));
            PatientTypeId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientTypeId", incommingColumnList));
            SchReasonId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SchReasonId", incommingColumnList));
            IsPhoneEncounter = ModelUtility.ToByteString(ModelUtility.MapValue(reader, "IsPhoneEncounter", incommingColumnList));
            Duration = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Duration", incommingColumnList));
            CPTCode = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CPTCode", incommingColumnList));
            DraftNotesCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DraftNotesCount", incommingColumnList));
            SignedNotesCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SignedNotesCount", incommingColumnList));
            bMedReconciled = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "bMedReconciled", incommingColumnList));
            MedReconciledId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedReconciledId", incommingColumnList));
            HxtabOrder = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HxtabOrder", incommingColumnList));
            ComeFromCopyNote = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "ComeFromCopyNote", incommingColumnList));
            IsNonBilable = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsNonBilable", incommingColumnList));
            VisitReasonComments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitReasonComments", incommingColumnList));
            ROSDataTemptId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ROSDataTemptId", incommingColumnList));
            PEDataTemptId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PEDataTemptId", incommingColumnList));
            PETemplateId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PETemplateId", incommingColumnList));
            ROSTemplateId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ROSTemplateId", incommingColumnList));
            UserName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "UserName", incommingColumnList));
            UserId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "UserId", incommingColumnList));
            EncounterType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EncounterType", incommingColumnList));
            Caller = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Caller", incommingColumnList));
            Receiver = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Receiver", incommingColumnList));
            EncounterTypeName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EncounterTypeName", incommingColumnList));
            UnSignedStatus = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "UnSignedStatus", incommingColumnList));
            ErrorMessage = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ErrorMessage", incommingColumnList));
            IsReviewed = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsReviewed", incommingColumnList));
            ResourceId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ResourceId", incommingColumnList));
            ResourceName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ResourceName", incommingColumnList));
            ResourceProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ResourceProviderId", incommingColumnList));
            ResourceProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ResourceProviderName", incommingColumnList));
            ResourceProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ResourceProviderName", incommingColumnList));

            NoteType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteType", incommingColumnList));
            RoomName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RoomName", incommingColumnList));
            NoteTempType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteTempType", incommingColumnList));
            POS = ModelUtility.ToStr(ModelUtility.MapValue(reader, "POS", incommingColumnList));
            AppointmentDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AppointmentDate", incommingColumnList));
            NoteDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteDate", incommingColumnList));
            OrderSetId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OrderSetId", incommingColumnList));
            OrderSetName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OrderSetName", incommingColumnList));

        }
    }
}
