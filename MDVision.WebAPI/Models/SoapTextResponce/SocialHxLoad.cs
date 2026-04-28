using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SocialHxLoad
    {
        public string SocialHxId { get; set; }
        public string PatientId { get; set; }
        public string SocialHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string SoapText { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string bDrugExist { get; set; }
        public string bTobaccoExist { get; set; }
        public string bAlcoholExist { get; set; }
        public string bSexualExist { get; set; }
        public string bMiscHxExist { get; set; }
    }
}