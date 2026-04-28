using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SocialHx
    {
        public string SocialHxId { get; set; }
        //public string PatientId { get; set; }
        public string SocialHxDate { get; set; }
        public string SocialHxSoapText { get; set; }
        public string SocialComments { get; set; }
        public string SoapText { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
        //public string ModifiedBy { get; set; }
        //public string ModifiedOn { get; set; }
        //public string SoapText { get; set; }
        //public string bDrugExist { get; set; }
        //public string bTobaccoExist { get; set; }
        //public string bAlcoholExist { get; set; }
        //public string bSexualExist { get; set; }
        //public string bMiscHxExist { get; set; }
    }
}