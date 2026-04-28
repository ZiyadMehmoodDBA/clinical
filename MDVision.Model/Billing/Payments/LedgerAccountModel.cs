using MDVision.Model.Common;
using System.Collections.Generic;
using System.Data;

namespace MDVision.Model.Billing.Payments
{
    public class LedgerAccountModel : IBaseModel
    {
        public string LedgerAccountId { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ApplyTo { get; set; }

        public string Type { get; set; }
        public string SystemCategory { get; set; }
        public string EntityId { get; set; }
        public string IsSystem { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            LedgerAccountId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LedgerAccountId", incommingColumnList));
            ShortName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ShortName", incommingColumnList));
            Description = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Description", incommingColumnList));
            ApplyTo = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ApplyTo", incommingColumnList));
            Type = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Type", incommingColumnList));
            SystemCategory = ModelUtility.ToStr(ModelUtility.MapValue(reader, "SystemCategory", incommingColumnList));
            EntityId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "EntityId", incommingColumnList));
            IsSystem = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsSystem", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
        }
    }
}
