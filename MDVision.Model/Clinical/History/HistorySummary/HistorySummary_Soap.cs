using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.History.HistorySummary
{
    public class HistorySummary_Soap : IBaseModel
    {
        public HistorySummary_Soap()
        {
        }
        public string HistoryId { get; set; }
        public string SocialHxId { get; set; }
        public string SurgicalHxId { get; set; }
        public string FamilyHxId { get; set; }
        public string HospitalizationHxId { get; set; }
        public string BirthHxId { get; set; }
        public string MedicalHxId { get; set; }
        public string SocialHxSoapText { get; set; }
        public string FamilyHxSoapText { get; set; }
        public string MedicalHxSoapText { get; set; }
        public string SurgicalHxSoapText { get; set; }
        public string EnvironmentalHxSoapText { get; set; }
        public string BirthHxSoapText { get; set; }
        public string HospitalizationHxSoapText { get; set; }
        public string PatientId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SocialandBehaviorHxId { get; set; }
        public string SocPsyandBehaviorHxSoapText { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            HistoryId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HistoryId", incommingColumnList));
            SocialHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SocialHxId", incommingColumnList));
            SurgicalHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SurgicalHxId", incommingColumnList));
            FamilyHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FamilyHxId", incommingColumnList));
            HospitalizationHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HospitalizationHxId", incommingColumnList));
            BirthHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BirthHxId", incommingColumnList));
            MedicalHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicalHxId", incommingColumnList));
            SocialHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SocialHxSoapText", incommingColumnList));
            FamilyHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FamilyHxSoapText", incommingColumnList));
            MedicalHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicalHxSoapText", incommingColumnList));
            SurgicalHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SurgicalHxSoapText", incommingColumnList));
            EnvironmentalHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EnvironmentalHxSoapText", incommingColumnList));
            BirthHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BirthHxSoapText", incommingColumnList));
            HospitalizationHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HospitalizationHxSoapText", incommingColumnList));
            SocialandBehaviorHxId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SocialandBehaviorHxId", incommingColumnList));
            SocPsyandBehaviorHxSoapText = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SocPsyandBehaviorHxSoapText", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
        }
    }
}
