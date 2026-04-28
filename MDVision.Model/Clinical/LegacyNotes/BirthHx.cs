using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class BirthHx
    {
        public Int64 BirthHxId { get; set; }
        public int ID { get; set; }
        public string BirthHxName { get; set; }
        public string HospitalName { get; set; }
        public DateTime? PatientDOB { get; set; }
        public string LengthStayatHospital { get; set; }
        public DateTime? DateAdmitted { get; set; }
        public string ObstetricianName { get; set; }
        public string PediatricianName { get; set; }
        public string ResponsiblePhysicianName { get; set; }
        public string Gestation { get; set; }
        public string NumberOfFetuses { get; set; }
        public string NumberOfLivingFetuses { get; set; }
        public string LaborLength { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryPresentation { get; set; }
        public string MaternalHistory { get; set; }
        public string HeadCircumference { get; set; }
        public string ChestCircumference { get; set; }
        public string WeightAtBirth { get; set; }
        public string LengthAtBirth { get; set; }
        public string ApgarAtBirth { get; set; }
        public string ApgarAt5Minutes { get; set; }
        public string WeightReleased { get; set; }
        public string PatientBloodType { get; set; }
        public string ProblemsAtBirth { get; set; }
        public bool bFetalDistress { get; set; }
        public string Comments { get; set; }
        public string SoapText { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
