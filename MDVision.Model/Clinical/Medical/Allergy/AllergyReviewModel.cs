using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Medical.Allergy
{
    public class AllergyReviewModel : IBaseModel
    {
        public AllergyReviewModel()
        {
        }
        public string AllergyReviewID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PatientId { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            AllergyReviewID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergyReviewID", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            PatientId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PatientId", incommingColumnList));
            ReviewedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedBy", incommingColumnList));
            ReviewedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReviewedOn", incommingColumnList));
        }
    }
}
