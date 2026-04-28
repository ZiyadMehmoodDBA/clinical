using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Vitals
{

    public class VitalSignsBloodPressure : IBaseModel
    {
        public VitalSignsBloodPressure()
        {
        }
        public string BPId { get; set; }
        public string VitalSignId { get; set; }
        public string Systolic { get; set; }
        public string Diastolic { get; set; }
        public string Time { get; set; }
        public string PositionId { get; set; }
        public string CuffLocationId { get; set; }
        public string CuffSizeId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Position { get; set; }
        public string CuffLocation { get; set; }
        public string CuffSize { get; set; }
        public string IsActive { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            BPId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BPId", incommingColumnList));
            VitalSignId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "VitalSignId", incommingColumnList));
            Systolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Systolic", incommingColumnList));
            Diastolic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Diastolic", incommingColumnList));
            Time = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Time", incommingColumnList));
            PositionId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PositionId", incommingColumnList));
            CuffLocationId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CuffLocationId", incommingColumnList));
            CuffSizeId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CuffSizeId", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            Position = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Position", incommingColumnList));
            CuffLocation = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CuffLocation", incommingColumnList));
            CuffSize = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CuffSize", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));

        }
    }
}
