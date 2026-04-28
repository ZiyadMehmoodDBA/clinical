using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace MDVision.Model.Clinical.Medical.ProblemLists
{
    public class ProblemHistory : IBaseModel
    {
        public string HistoryId { get; set; }
        public string ProblemName { get; set; }
        public string Description { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string ProblemId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string RecordCount { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string ICD9_Description { get; set; }
        public string ICD10_Description { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string IsCancerDisease { get; set; }
        public string ModifiedByName { get; set; }
        public string IsTobaccoConfig { get; set; }
        public string IsDiabetesConfig { get; set; }
        public string IsDepressionConfig { get; set; }
        public string ProviderId { get; set; }
        public void Map(IDataReader reader, List<string> columnsList)
        {
            HistoryId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HistoryId", columnsList));
            ProblemName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProblemName", columnsList));
            Description = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Description", columnsList));
            ChronicityLevel = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ChronicityLevel", columnsList));
            Severity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Severity", columnsList));
            StartDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "StartDate", columnsList));
            EndDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EndDate", columnsList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", columnsList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", columnsList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", columnsList));
            ProblemId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProblemId", columnsList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", columnsList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", columnsList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", columnsList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", columnsList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", columnsList));
            InActiveChkBoxValue = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveChkBoxValue", columnsList));
            InActiveReason = ModelUtility.ToStr(ModelUtility.MapValue(reader, "InActiveReason", columnsList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", columnsList));
            ICD9 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD9", columnsList));
            ICD10 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD10", columnsList));
            ICD9_Description = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD9_Description", columnsList));
            ICD10_Description = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ICD10_Description", columnsList));
            SNOMEDID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SNOMEDID", columnsList));
            SNOMED_DESCRIPTION = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SNOMED_DESCRIPTION", columnsList));
            IsCancerDisease = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsCancerDisease", columnsList));
            ModifiedByName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedByName", columnsList));
            IsTobaccoConfig = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsTobaccoConfig", columnsList));
            IsDiabetesConfig = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsDiabetesConfig", columnsList));
            IsDepressionConfig = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsDepressionConfig", columnsList));
            ProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProviderId", columnsList));
        }
    }
}
