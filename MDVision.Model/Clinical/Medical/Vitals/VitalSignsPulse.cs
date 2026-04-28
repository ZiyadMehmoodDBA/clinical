using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Vitals
{
	
	public class VitalSignsPulse : IBaseModel
	{
		public VitalSignsPulse()
		{
		}
		public string PulseId	{ get; set; }
		public string VitalSignId	{ get; set; }
		public string Result	{ get; set; }
		public string RhythmId	{ get; set; }
		public string Time	{ get; set; }
		public string CreatedBy	{ get; set; }
		public string CreatedOn	{ get; set; }
		public string ModifiedBy	{ get; set; }
		public string ModifiedOn	{ get; set; }
        public string IsActive { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            PulseId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PulseId", incommingColumnList));
            VitalSignId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignId", incommingColumnList));
            Result = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Result", incommingColumnList));
            RhythmId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RhythmId", incommingColumnList));
            Time = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Time", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));

        }
    }
}
