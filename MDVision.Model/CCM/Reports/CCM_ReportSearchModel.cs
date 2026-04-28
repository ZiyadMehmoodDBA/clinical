using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.Reports
{
    public class CCM_ReportSearchModel
    {
        public string AccountNumber { get; set; }
        public bool AdvancedSearch { get; set; }
        public string CareCoordinator { get; set; }
        public string CareGiver { get; set; }
        public string CareManager { get; set; }
        public string ConsentStatus { get; set; }
        public string DOB { get; set; }
        public string FacilityIds { get; set; }
        public string FromDate { get; set; }
        public string Gender { get; set; }
       
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PracticeIds { get; set; }
        public string ProblemNameValue { get; set; }
        public string ProgramStatus { get; set; }
        public string ProviderIds { get; set; }
        public bool SummaryReport { get; set; }
        public string ToTimeCompleted { get; set; }
        public string fromTimeCompleted { get; set; }
        public string NoOfProblemTo { get; set; }
        public string NoOfProblemFrom { get; set; }
        public string ToDate { get; set; }
        public string ProgramType { get; set; }
        public string ProgramType_text { get; set; }
    }
}
