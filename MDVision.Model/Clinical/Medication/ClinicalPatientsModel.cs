using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{

    public class ClinicalPatientsModel : IBaseModel
    {

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string IsActive { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedOn { get; set; }

        public string PatientId { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
        }
    }
}
