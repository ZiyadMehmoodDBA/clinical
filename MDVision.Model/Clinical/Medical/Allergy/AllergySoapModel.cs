using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Model.Clinical.Medical.Allergy
{
    public class AllergySoapModel : IBaseModel
    {
        public AllergySoapModel()
        {
        }
        public string AllergyId { get; set; }
        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string OnSetDate { get; set; }
        public string Comments { get; set; }
        public string NoteId { get; set; }
        public string RxnormID { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            AllergyId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "AllergyId", incommingColumnList));
            Allergen = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Allergen", incommingColumnList));
            Reaction = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Reaction", incommingColumnList));
            Severity = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Severity", incommingColumnList));
            OnSetDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "OnSetDate", incommingColumnList));
            Comments = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Comments", incommingColumnList));
            NoteId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NoteId", incommingColumnList));
            RxnormID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormID", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));

        }
    }
}
