using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Medical.CDS
{
    public class CDSForAlerts : IBaseModel
    {
        public CDSForAlerts()
        {
        }
        public string CDSId { get; set; }
        public string Title { get; set; }
        public string Developer { get; set; }
        public string FundingSource { get; set; }
        public string ReferenceURL { get; set; }
        public string Release { get; set; }
        public string RevisionDate { get; set; }
        public string TriggerLocation { get; set; }
        public string Role { get; set; }
        public string RuleType { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string Language { get; set; }
        public string ReminderLength { get; set; }
        public string ReminderPeriodId { get; set; }
        public string IsRecursive { get; set; }
        public string ProblemListOperator { get; set; }
        public string CDSProblemListQuery { get; set; }
        public string AllergiesOperator { get; set; }
        public string AllergiesQuery { get; set; }
        public string MedicationsOperator { get; set; }
        public string MedicationsQuery { get; set; }
        public string LabsOperator { get; set; }
        public string LabsQuery { get; set; }
        public string VitalsOperator { get; set; }
        public string VitalsQuery { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSQuery { get; set; }
        public string RecordCount { get; set; }
        public string Status { get; set; }
        public string EndDate { get; set; }
        public string AgeConditionId { get; set; }
        public string FromAge { get; set; }
        public string ToAge { get; set; }
        public string RuleTypeDes { get; set; }
        public string UserRoleQuery { get; set; }
        public string PatientId { get; set; }
        public string RecursiveLength { get; set; }
        public string RecursivePeriodId { get; set; }
        public string OrderSetIds { get; set; }
        public string QuestionnaireHTML { get; set; }
        public string CDSPatientStatusId { get; set; }

        public string EthnicityName { get; set; }
        public string RaceName { get; set; }
        public string LocationDescription { get; set; }
        
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            CDSId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CDSId", incommingColumnList));
            Title = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Title", incommingColumnList));
            Developer = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Developer", incommingColumnList));
            FundingSource = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FundingSource", incommingColumnList));
            ReferenceURL = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReferenceURL", incommingColumnList));
            Release = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Release", incommingColumnList));
            RevisionDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RevisionDate", incommingColumnList));
            TriggerLocation = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TriggerLocation", incommingColumnList));
            Role = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Role", incommingColumnList));
            RuleType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RuleType", incommingColumnList));
            Gender = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Gender", incommingColumnList));
            Ethnicity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Ethnicity", incommingColumnList));
            Race = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Race", incommingColumnList));
            Language = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Language", incommingColumnList));
            ReminderLength = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReminderLength", incommingColumnList));
            ReminderPeriodId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReminderPeriodId", incommingColumnList));
            IsRecursive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsRecursive", incommingColumnList));
            ProblemListOperator = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ProblemListOperator", incommingColumnList));
            CDSProblemListQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CDSProblemListQuery", incommingColumnList));
            AllergiesOperator = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergiesOperator", incommingColumnList));
            AllergiesQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergiesQuery", incommingColumnList));
            MedicationsOperator = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationsOperator", incommingColumnList));
            MedicationsQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationsQuery", incommingColumnList));
            LabsOperator = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LabsOperator", incommingColumnList));
            LabsQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LabsQuery", incommingColumnList));
            VitalsOperator = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalsOperator", incommingColumnList));
            VitalsQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalsQuery", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            CDSQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CDSQuery", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            Status = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Status", incommingColumnList));
            EndDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EndDate", incommingColumnList));
            AgeConditionId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AgeConditionId", incommingColumnList));
            FromAge = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FromAge", incommingColumnList));
            ToAge = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ToAge", incommingColumnList));
            RuleTypeDes = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RuleTypeDes", incommingColumnList));
            UserRoleQuery = ModelUtility.ToStr(ModelUtility.MapValue(reader, "UserRoleQuery", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            RecursiveLength = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecursiveLength", incommingColumnList));
            RecursivePeriodId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecursivePeriodId", incommingColumnList));
            OrderSetIds = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OrderSetIds", incommingColumnList));
            QuestionnaireHTML = ModelUtility.ToStr(ModelUtility.MapValue(reader, "QuestionnaireHTML", incommingColumnList));
            CDSPatientStatusId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CDSPatientStatusId", incommingColumnList));
            EthnicityName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EthnicityName", incommingColumnList));
            RaceName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RaceName", incommingColumnList));
            LocationDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LocationDescription", incommingColumnList));
        }
    }
}
