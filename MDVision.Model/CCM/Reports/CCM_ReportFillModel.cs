using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.Reports
{
    public class CCM_ReportFillModel
    {
        public string AccountNumber { get; set; }
        public string CareCoordinator { get; set; }
        public string CareGiver { get; set; }
        public string CareManager { get; set; }
        public string ChronicConditionsCount { get; set; }
        public string ConsentStatus { get; set; }
        public string DOB { get; set; }
        public string FacilityName { get; set; }
        public string Gender { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string PracticeName { get; set; }
        public string Problems { get; set; }
        public string ProgramStatus { get; set; }
        public string Provider { get; set; }
        public string TimeCompleted { get; set; }
        public string ProgramType { get; set; }
    }
}
