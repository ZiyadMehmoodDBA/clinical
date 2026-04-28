using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
    public class PatientConsentVM : NativeBaseModel
    {
        public string PatientConsentId { get; set; }
        public string PatientId { get; set; }
        public string ERxConsentSign { get; set; }
        public string ERxConsentDate { get; set; }
        public string NPPConsentSign { get; set; }
        public string NPPConsentDate { get; set; }
        public string FPConsentSign { get; set; }
        public string FPConsentDate { get; set; }
        public string ABNConsentSign { get; set; }
        public string ABNConsentDate { get; set; }
        public string Notifier { get; set; }
        public string IdentificationNo { get; set; }
        public string PatientName { get; set; }
        public bool EKG { get; set; }
        public bool Homeccult { get; set; }
        public bool Cultures { get; set; }
        public bool SupplierAndMaterials { get; set; }
        public bool LabWork { get; set; }
        public bool Vaccine { get; set; }
        public bool PFT { get; set; }
        public bool UA { get; set; }
        public bool Others { get; set; }
        public bool MedicareReason1 { get; set; }
        public bool MedicareReason2 { get; set; }
        public bool MedicareReason3 { get; set; }
        public bool MedicareReason4 { get; set; }
        public bool UpTo50 { get; set; }
        public bool UpTo50To100 { get; set; }
        public bool UpTo100To200 { get; set; }
        public bool UpTo200To300 { get; set; }
        public bool MoreThan300 { get; set; }
        public bool Option1 { get; set; }
        public bool Option2 { get; set; }
        public bool Option3 { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string DimmyPatientId { get; set; }

    }
}
