using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{

    public class ClinicalMedicationReviewModel : IBaseModel
    {


        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string IsActive { get; set; }

        public string MedicationReviewID { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedOn { get; set; }

        public string PatientId { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {

            MedicationReviewID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MedicationReviewID", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            ReviewedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedBy", incommingColumnList));
            ReviewedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedOn", incommingColumnList));

        }
    }
}
