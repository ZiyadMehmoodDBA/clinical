using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Vitals
{
	
	public class VitalSignSoap : IBaseModel
	{
		public VitalSignSoap()
		{
		}
		public string VitalSignId	{ get; set; }
		public string Height	{ get; set; }
		public string Weight	{ get; set; }
		public string Comments	{ get; set; }
		public string OxygenSource	{ get; set; }
		public string PeakFlow	{ get; set; }
		public string BMI	{ get; set; }
		public string BSA	{ get; set; }
		public string BP	{ get; set; }
		public string PulseResult	{ get; set; }
		public string TemperatureResult	{ get; set; }
		public string TemperatureRhythm	{ get; set; }
		public string RespirationResult	{ get; set; }
		public string RespirationRateRhythm	{ get; set; }
		public string SPO2	{ get; set; }
		public string SeverityofPain	{ get; set; }
		public string SmokingStatus	{ get; set; }
		public string PulseRhythm	{ get; set; }
		public string CreatedBy	{ get; set; }
		public string BPId	{ get; set; }
		public string Systolic	{ get; set; }
		public string Diastolic	{ get; set; }
		public string PulseId	{ get; set; }
		public string TemperatureId	{ get; set; }
		public string RespirationId	{ get; set; }
		public string VitalSignDate	{ get; set; }
		public string VitalSignTime	{ get; set; }
		public string RecordCount	{ get; set; }
		public string IsActive	{ get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            VitalSignId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignId", incommingColumnList));
            Height = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Height", incommingColumnList));
            Weight = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Weight", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            OxygenSource = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OxygenSource", incommingColumnList));
            PeakFlow = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PeakFlow", incommingColumnList));
            BMI = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BMI", incommingColumnList));
            BSA = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BSA", incommingColumnList));
            BP = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BP", incommingColumnList));
            PulseResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseResult", incommingColumnList));
            TemperatureResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemperatureResult", incommingColumnList));
            TemperatureRhythm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemperatureRhythm", incommingColumnList));
            RespirationResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespirationResult", incommingColumnList));
            RespirationRateRhythm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespirationRateRhythm", incommingColumnList));
            SPO2 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SPO2", incommingColumnList));
            SeverityofPain = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SeverityofPain", incommingColumnList));
            SmokingStatus = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SmokingStatus", incommingColumnList));
            PulseRhythm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseRhythm", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            BPId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BPId", incommingColumnList));
            Systolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Systolic", incommingColumnList));
            Diastolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Diastolic", incommingColumnList));
            PulseId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseId", incommingColumnList));
            TemperatureId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemperatureId", incommingColumnList));
            RespirationId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespirationId", incommingColumnList));
            VitalSignDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignDate", incommingColumnList));
            VitalSignTime = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignTime", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));


        }
    }
}
