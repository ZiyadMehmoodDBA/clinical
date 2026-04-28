using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Procedures
{

    public class ProcedureHistoryModel : IBaseModel
    {

        public string ProcedureId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string CPTCode { get; set; }
        public string CPT_DESCRIPTION { get; set; }
        public string ICD9_DESCRIPTION { get; set; }
        public string ICD10_DESCRIPTION { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string RecordCount { get; set; }
        public string HistoryId { get; set; }
        public string ProblemListId { get; set; }
        public string Modifier { get; set; }
        public string Unit { get; set; }
        public string Fee { get; set; }
        public bool ProcedureTemplateSoapTextExists { get; set; }
        public string Surgical { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            ProcedureId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProcedureId", incommingColumnList));
            StartDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StartDate", incommingColumnList));
            EndDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EndDate", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            InActiveChkBoxValue = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveChkBoxValue", incommingColumnList));
            InActiveReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveReason", incommingColumnList));
            ICD9 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD9", incommingColumnList));
            ICD10 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD10", incommingColumnList));
            SNOMEDID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SNOMEDID", incommingColumnList));
            SNOMED_DESCRIPTION = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SNOMED_DESCRIPTION", incommingColumnList));
            CPTCode = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CPTCode", incommingColumnList));
            CPT_DESCRIPTION = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CPT_DESCRIPTION", incommingColumnList));
            ICD9_DESCRIPTION = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD9_DESCRIPTION", incommingColumnList));
            ICD10_DESCRIPTION = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD10_DESCRIPTION", incommingColumnList));
            RowsPerPage = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RowsPerPage", incommingColumnList));
            PageNumber = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PageNumber", incommingColumnList));
            HistoryId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HistoryId", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            ProblemListId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProblemListId", incommingColumnList));
            Modifier = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Modifier", incommingColumnList));
            Unit = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Unit", incommingColumnList));
            Fee = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Fee", incommingColumnList));
            ProcedureTemplateSoapTextExists = ModelUtility.ToBool(ModelUtility.MapValue(reader, "ProcedureTemplateSoapTextExists", incommingColumnList));
            Surgical = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Surgical", incommingColumnList));
        }
    }
}
