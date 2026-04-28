using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class PharamacyModel
    {
            public long PharmacyId{  get; set; }
            public string RcopiaID {  get; set; }
            public string NPI { get; set; }
            public string       RcopiaMasterID { get; set; }
            public  string NCPDPID { get; set; }
            public string PharmacyName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public  string State { get; set; }
            public string Zip { get; set; }
            public  string Phone{ get; set; }
            public string  Fax { get; set; }
            public  string Is24Hour{ get; set; }
            public string Level3 { get; set; }
            public string Electronic { get; set; }
            public string MailOrder { get; set; }
            public  string Retail{ get; set; }
            public  string LongTermCare{ get; set; }
            public string Specialty { get; set; }
            public  string CanReceiveControlledSubstance{ get; set; }
            public string IsActive { get; set; }
            public  string CreatedBy{ get; set; }
            public DateTime CreatedOn { get; set; }
            public string ModifiedBy  { get; set; }
            public DateTime ModifiedOn { get; set; }
    }
}