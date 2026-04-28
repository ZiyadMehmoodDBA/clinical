using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medication
{

    public class ClinicalPharmacyModel : IBaseModel
    {

        public string PharmacyId { get; set; }
        public string RcopiaID { get; set; }
        public string RcopiaMasterID { get; set; }
        public string NCPDPID { get; set; }
        public string PharmacyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Is24Hour { get; set; }
        public string Level3 { get; set; }
        public string Electronic { get; set; }
        public string MailOrder { get; set; }
        public string Retail { get; set; }
        public string stringTermCare { get; set; }
        public string Specialty { get; set; }
        public string CanReceiveControlledSubstance { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            PharmacyId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyId", incommingColumnList));
            RcopiaID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaID", incommingColumnList));
            RcopiaMasterID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RcopiaMasterID", incommingColumnList));
            NCPDPID = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NCPDPID", incommingColumnList));
            PharmacyName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PharmacyName", incommingColumnList));
            Address = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Address", incommingColumnList));
            City = ModelUtility.ToStr(ModelUtility.MapValue(reader, "City", incommingColumnList));
            State = ModelUtility.ToStr(ModelUtility.MapValue(reader, "State", incommingColumnList));
            Zip = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Zip", incommingColumnList));
            Phone = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Phone", incommingColumnList));
            Fax = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Fax", incommingColumnList));
            Is24Hour = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Is24Hour", incommingColumnList));
            Level3 = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Level3", incommingColumnList));
            Electronic = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Electronic", incommingColumnList));
            MailOrder = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MailOrder", incommingColumnList));
            Retail = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Retail", incommingColumnList));
            stringTermCare = ModelUtility.ToStr(ModelUtility.MapValue(reader, "stringTermCare", incommingColumnList));
            Specialty = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Specialty", incommingColumnList));
            CanReceiveControlledSubstance = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CanReceiveControlledSubstance", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            CreatedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedBy", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedBy = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedBy", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
        }
    }
}
