using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class MemberLoad
    {
        public string MemberDetailId { get; set; }

        public string DiseaseId { get; set; }

        public string MemberId { get; set; }

        public string HealthStatusId { get; set; }

        public string BirthYear { get; set; }

        public string IsRelativeDied { get; set; }

        public string AgeAtDeath { get; set; }
        public string AgeAtDiagnosis { get; set; }

        public string Comments { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }

        public string ModifiedOn { get; set; }

        public string SoapText { get; set; }
    }
}