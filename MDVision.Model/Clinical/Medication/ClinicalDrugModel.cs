using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{

    public class ClinicalDrugModel : IBaseModel
    {

        public string DrugId { get; set; }
        public string RcopiaID { get; set; }
        public string NDCID { get; set; }
        public string FirstDataBankMedID { get; set; }
        public string DrugDescription { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Schedule { get; set; }
        public string BrandType { get; set; }
        public string LegendStatus { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public string Strength { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RxnormID { get; set; }
        public string RxnormIDType { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            DrugId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugId", incommingColumnList));
            RcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaID", incommingColumnList));
            NDCID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NDCID", incommingColumnList));
            FirstDataBankMedID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FirstDataBankMedID", incommingColumnList));
            DrugDescription = ModelUtility.ToStr(ModelUtility.MapValue(reader, "DrugDescription", incommingColumnList));
            BrandName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BrandName", incommingColumnList));
            GenericName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "GenericName", incommingColumnList));
            Schedule = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Schedule", incommingColumnList));
            BrandType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "BrandType", incommingColumnList));
            LegendStatus = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LegendStatus", incommingColumnList));
            Route = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Route", incommingColumnList));
            Form = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Form", incommingColumnList));
            Strength = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Strength", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            RxnormID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormID", incommingColumnList));
            RxnormIDType = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RxnormIDType", incommingColumnList));
        }
    }
}
