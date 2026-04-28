using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class BirthHx_GeneralHx
    {
        public long GeneralId { get; set; }
        public long BirthHxId { get; set; }
        public string HospitalName { get; set; }
        public string PatientDOB { get; set; }
        public string LengthStayatHospital { get; set; }
        public string DateAdmitted { get; set; }
        public string ObstetricianName { get; set; }
        public string PediatricianName { get; set; }
        public int? ResponsiblePhysicianId { get; set; }
        public string GeneralComments { get; set; }
        public string SoapText { get; set; }

        public string ResponsiblePhysicianId_text { get; set; }
    }
}