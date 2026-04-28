using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class BirthHx_NewBornHx
    {
        public long NewbornId { get; set; }
        public long BirthHxId { get; set; }
        public float? HeadCircumference { get; set; }
        public float? ChestCircumference { get; set; }
        public float? WeightAtBirth { get; set; }
        public float? LengthAtBirth { get; set; }
        public string ApgarAtBirth { get; set; }
        public string ApgarAt5Minutes { get; set; }
        public float? WeightReleased { get; set; }
        public int? PatientBloodTypeId { get; set; }
        public int? ProblemsAtBirthId { get; set; }
        public bool? bFetalDistress { get; set; }
        public bool bFetalDistressYes { get; set; }
        public bool bFetalDistressNo { get; set; }
        public string NewbornComments { get; set; }
        public string SoapText { get; set; }

        public string PatientBloodTypeId_text { get; set; }

        public string ProblemsAtBirthId_text { get; set; }
    }
}