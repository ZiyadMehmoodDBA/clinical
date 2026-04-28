using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Vitals
{
	
	public class VitalSignsChild : IBaseModel
	{
		public VitalSignsChild()
		{
		}
		public string VitalChildId	{ get; set; }
		public string VitalSignId	{ get; set; }
		public string BPId	{ get; set; }
		public string Systolic	{ get; set; }
		public string Diastolic	{ get; set; }
		public string PulseId	{ get; set; }
		public string PulseResult	{ get; set; }
		public string Pulserhythm	{ get; set; }
		public string TemperatureId	{ get; set; }
		public string TemperatureResult	{ get; set; }
		public string tempratureRhytnm	{ get; set; }
		public string RespirationId	{ get; set; }
		public string RespirationResult	{ get; set; }
		public string RespRhythm	{ get; set; }
		public string BPModifiedBy	{ get; set; }
		public string BPModifiedOn	{ get; set; }
		public string PulseModifiedBy	{ get; set; }
		public string PulseModifiedOn	{ get; set; }
		public string TempModifiedBy	{ get; set; }
		public string TempModifiedOn	{ get; set; }
		public string RespModifiedBy	{ get; set; }
		public string RespModifiedOn	{ get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get;set;}

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            VitalChildId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalChildId", incommingColumnList));
            VitalSignId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignId", incommingColumnList));
            BPId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BPId", incommingColumnList));
            Systolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Systolic", incommingColumnList));
            Diastolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Diastolic", incommingColumnList));
            PulseId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseId", incommingColumnList));
            PulseResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseResult", incommingColumnList));
            Pulserhythm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Pulserhythm", incommingColumnList));
            TemperatureId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemperatureId", incommingColumnList));
            TemperatureResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TemperatureResult", incommingColumnList));
            tempratureRhytnm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "tempratureRhytnm", incommingColumnList));
            RespirationId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespirationId", incommingColumnList));
            RespirationResult = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespirationResult", incommingColumnList));
            RespRhythm = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespRhythm", incommingColumnList));
            BPModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BPModifiedBy", incommingColumnList));
            BPModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BPModifiedOn", incommingColumnList));
            PulseModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseModifiedBy", incommingColumnList));
            PulseModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseModifiedOn", incommingColumnList));
            TempModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TempModifiedBy", incommingColumnList));
            TempModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "TempModifiedOn", incommingColumnList));
            RespModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespModifiedBy", incommingColumnList));
            RespModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RespModifiedOn", incommingColumnList));
            IsActive = ModelUtility.ToBoolString(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));

        }
    }
}
