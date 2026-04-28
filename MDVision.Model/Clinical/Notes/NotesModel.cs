using MDVision.Model.Common;
using System.Collections.Generic;
using System.Data;

namespace MDVision.Model.Clinical.Notes
{
    public class NotesModel
    {
        public NotesModel()
        {
        }
        public string NotesId { get; set; }
        public string AccountNumber { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoteStatus { get; set; }
        public string RecordCount { get; set; }
        public string PatientId { get; set; }
        public string AppointmentId { get; set; }
        public string VisitReason { get; set; }
        public string ProviderName { get; set; }
        public string ProviderId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string RefProviderName { get; set; }
        public string RefProviderId { get; set; }
        public string LinkedAppointment { get; set; }
        public string EntityId { get; set; }
        public string ShortName { get; set; }
        public string VisitReasonId { get; set; }
        public string PatientName { get; set; }
        public string VisitId { get; set; }
        public string VisitType { get; set; }
        public string BillingInfoId { get; set; }
        public string PatientTypeId { get; set; }
        public string IsNonBilable { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string EncounterType { get; set; }
        public string NoteType { get; set; }
        public string AppointmentDate { get; set; }
        public string NoteDate { get; set; }
        public string POS { get; set; }
        public string IsPhoneEncounter { get; set; }
        public string BillingStatus { get; set; }
        public string AppReason { get; set; }
        public string MissingInfo { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string NoteText { get; set; }
        public string SignedBy { get; set; }
        public string SignedByName { get; set; }
        public string Comments { get; set; }
        public string RefProviderAddress { get; set; }
        public string PrevNoteDescription { get; set; }
        public string RoomName { get; set; }
        public string ChiefComplaint { get; set; }
        public string TemplateName { get; set; }

        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PacilityPOSCode { get; set; }
        public string TemplateTypeName { get; set; }
        public string FacilityPOSDesc { get; set; }
        public string VisitReasonComments { get; set; }

        public string CDSIds { get; set; }


        //public void Map(IDataReader reader, List<string> incommingColumnList)
        //{
        //    NotesId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NotesId", incommingColumnList));
        //    AccountNumber = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AccountNumber", incommingColumnList));
        //    PatientName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientName", incommingColumnList));
        //    VisitId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitId", incommingColumnList));
        //    ProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderId", incommingColumnList));
        //    ProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Provider", incommingColumnList));
        //    FacilityName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Facility", incommingColumnList));
        //    VisitDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitDate", incommingColumnList));
        //    VisitReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Reason", incommingColumnList));
        //    NoteType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteType", incommingColumnList));
        //    NoteStatus = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteStatus", incommingColumnList));
        //    IsNonBilable = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsNonBilable", incommingColumnList));
        //    BillingInfoId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BillingInfoId", incommingColumnList));
        //    RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));



        //    //VisitTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitTime", incommingColumnList));
        //    //PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
        //    //AppointmentId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AppointmentId", incommingColumnList));
        //    //FacilityId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FacilityId", incommingColumnList));
        //    //RefProviderName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RefProviderName", incommingColumnList));
        //    //RefProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RefProviderId", incommingColumnList));
        //    //LinkedAppointment = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LinkedAppointment", incommingColumnList));
        //    //EntityId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EntityId", incommingColumnList));
        //    //ShortName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ShortName", incommingColumnList));
        //    //VisitReasonId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitReasonId", incommingColumnList));
        //    //VisitType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitType", incommingColumnList));
        //    //PatientTypeId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientTypeId", incommingColumnList));
        //    //UserName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "UserName", incommingColumnList));
        //    //UserId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "UserId", incommingColumnList));
        //    //EncounterType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EncounterType", incommingColumnList));

        //    //AppointmentDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AppointmentDate", incommingColumnList));
        //    //NoteDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteDate", incommingColumnList));


        //}
    }
}
