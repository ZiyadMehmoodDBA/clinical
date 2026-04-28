using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.DrFirst
{
    public class DownloadResponseModel
    {
        public bool AllergyDownloadSuccessfully { get; set; }
        public bool IsAllergyDownload { get; set; }
        public string SavedAllergyIds { get; set; }
        
        public bool MedicationDownloadSuccessfully  { get; set; }
        public bool IsMedicationDownload { get; set; }
        public string SavedMedicationIds { get; set; }   
        public bool PrescriptionDownloadSuccessfully { get; set; }
        public bool IsPrescriptionDownload { get; set; }
        public string SavedPrescriptionIds { get; set; }
        public bool IsPrescriptionDeleted { get; set; }
        public Int64 AllergyReviewID { get; set; }
        public Int64 MedicationReviewID { get; set; }
        public string Component { get; set; }
        public bool status { get; set; }
        public string Message { get; set; }
        
    }
}
