using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Templates.OrderSets
{
    public class OS_ProblemListModel
    {
        public string ProblemListId { get; set; }
        public string EntityId { get; set; }
        public string ProblemName { get; set; }
        public string Description { get; set; }
        public string ChronicityLevel { get; set; }
        public string Severity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string OrderSetId { get; set; }
     
        public string commandType { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string IsActiveRecord { get; set; }
        public string IsActive { get; set; }

        public string VisitId { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string ICD9_Description { get; set; }
        public string ICD10_Description { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string IsChiefComplaint { get; set; }
        public string UserId { get; set; }
    }
}
