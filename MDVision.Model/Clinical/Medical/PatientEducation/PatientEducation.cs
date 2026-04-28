using MDVision.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Medical.PatientEducation
{
    public class PatientEducation : IBaseModel
    {
        public PatientEducation()
        {
        }

        public string PatEducationId { get; set; }
        public string PatDocId { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
        public string FileStream { get; set; }
        public string Comments { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string FileType { get; set; }
        public string Pages { get; set; }
        public string RecordCount { get; set; }
        public string PatientId { get; set; }
        public string DocType { get; set; }
        public string InfoDoc { get; set; }
        public string NonInfoDoc { get; set; }
        public string IsNoteLinked { get; set; }
        public string NoteId { get; set; }
        public string IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedByName { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            PatEducationId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatEducationId", incommingColumnList));
            PatDocId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatDocId", incommingColumnList));
            DocumentName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DocumentName", incommingColumnList));
            FilePath = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FilePath", incommingColumnList));
            FileStream = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FileStream", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            FileType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FileType", incommingColumnList));
            Pages = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Pages", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            DocType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DocType", incommingColumnList));
            InfoDoc = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InfoDoc", incommingColumnList));
            NonInfoDoc = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NonInfoDoc", incommingColumnList));
            IsNoteLinked = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsNoteLinked", incommingColumnList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            CreatedByName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedByName", incommingColumnList));
        }
    }
}
