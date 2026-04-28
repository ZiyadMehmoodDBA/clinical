using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Vitals
{
	
	public class VitalSignsReader : IBaseModel
	{
		public VitalSignsReader()
		{
		}
		public string VitalSignId	{ get; set; }
		public string PatientId	{ get; set; }
		public string VisitId	{ get; set; }
		public string Height	{ get; set; }
		public string Weight	{ get; set; }
		public string IsActive	{ get; set; }
		public string CreatedBy	{ get; set; }
		public string CreatedOn	{ get; set; }
		public string ModifiedBy	{ get; set; }
		public string ModifiedOn	{ get; set; }
		public string RecordCount	{ get; set; }
		public string NotesId	{ get; set; }
		public string CopyParentId	{ get; set; }
		public string SPO2	{ get; set; }
		public string OxygenSource	{ get; set; }
		public string PeakFlow	{ get; set; }
		public string PainId	{ get; set; }
		public string SmokeStatusId	{ get; set; }
		public string Comments	{ get; set; }
		public string VitalSignDate	{ get; set; }
		public string VitalSignTime	{ get; set; }
		public string BMI	{ get; set; }
		public string BSA	{ get; set; }
		public string HeadCr	{ get; set; }
		public string BloodType	{ get; set; }
		public string DeleteComments	{ get; set; }
		public string BloodGroup	{ get; set; }
		public string RiskAssessmentId	{ get; set; }
		public string IsFromNote	{ get; set; }
        
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            VitalSignId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignId", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            VisitId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VisitId", incommingColumnList));
            Height = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Height", incommingColumnList));
            Weight = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Weight", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            NotesId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NotesId", incommingColumnList));
            CopyParentId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CopyParentId", incommingColumnList));
            SPO2 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SPO2", incommingColumnList));
            OxygenSource = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OxygenSource", incommingColumnList));
            PeakFlow = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PeakFlow", incommingColumnList));
            PainId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PainId", incommingColumnList));
            SmokeStatusId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SmokeStatusId", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            VitalSignDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignDate", incommingColumnList));
            VitalSignTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignTime", incommingColumnList));
            BMI = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BMI", incommingColumnList));
            BSA = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BSA", incommingColumnList));
            HeadCr = ModelUtility.ToStr(ModelUtility.MapValue(reader, "HeadCr", incommingColumnList));
            BloodType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BloodType", incommingColumnList));
            DeleteComments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DeleteComments", incommingColumnList));
            BloodGroup = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BloodGroup", incommingColumnList));
            RiskAssessmentId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RiskAssessmentId", incommingColumnList));
            IsFromNote = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsFromNote", incommingColumnList));

        }
    }
}
