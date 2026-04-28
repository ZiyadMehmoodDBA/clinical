using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class ClinicalNotesExtraInfoModel
    {
        public string MedicalReconciliation { get; set; }
        public string AllergiesReconciliation { get; set; }
        public string ProblemsReconciliation { get; set; }
        public string LocateCCDA { get; set; }
        public string PatientEducation { get; set; }
        public string SummaryOfCareA { get; set; }
        public string SummaryOfCareB { get; set; }
        public string SummaryOfCareC { get; set; }
        public string VDTTimelyAccess { get; set; }
        public string SecureMessage { get; set; }
        public string VDTAPITimelyAccess { get; set; }
        public string TransitionsOfCare { get; set; }
        public string VDTAPIPatient { get; set; }
        public string VDTPatient { get; set; }
        public string IsActive { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string ValueSettingId { get; set; }
        public string Description { get; set; }
        public string commandType { get; set; }

    }
}