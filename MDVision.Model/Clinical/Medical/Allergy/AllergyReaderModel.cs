using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Medical.Allergy
{
    public class AllergyReaderModel : IBaseModel
    {
        public AllergyReaderModel()
        {
        }
        public string AllergyId { get; set; }
        public string Allergen { get; set; }
        public string Type { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string OnSetDate { get; set; }
        public string LastModified { get; set; }
        public string IsActive { get; set; }
        public string Comments { get; set; }
        public string NoteId { get; set; }
        public string InActiveCheckBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string PatientId { get; set; }
        public string RecordCount { get; set; }
        public string IsNoteLinked { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string RcopiaID { get; set; }
        public string IsDeleted { get; set; }
        public string RxnormID { get; set; }
        public string RxnormIDType { get; set; }
        public string IsNewRow { get; set; }
        public string LookupId { get; set; }
        public string Name { get; set; }
        public string AllergenId { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            AllergyId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergyId", incommingColumnList));
            Allergen = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Allergen", incommingColumnList));
            Type = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Type", incommingColumnList));
            Reaction = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Reaction", incommingColumnList));
            Severity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Severity", incommingColumnList));
            OnSetDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OnSetDate", incommingColumnList));
            LastModified = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastModified", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", incommingColumnList));
            InActiveCheckBoxValue = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveCheckBoxValue", incommingColumnList));
            InActiveReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveReason", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            IsNoteLinked = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsNoteLinked", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            RcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaID", incommingColumnList));
            IsDeleted = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsDeleted", incommingColumnList));
            RxnormID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormID", incommingColumnList));
            RxnormIDType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormIDType", incommingColumnList));
            IsNewRow = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsNewRow", incommingColumnList));
            LookupId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LookupId", incommingColumnList));
            Name = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Name", incommingColumnList));
            AllergenId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergenId", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));

        }
    }
}
