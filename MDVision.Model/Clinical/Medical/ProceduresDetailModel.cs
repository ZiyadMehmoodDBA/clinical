using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical
{
    public class ProceduresDetailModel
    {
        public string ProcedureId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NotesId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string ProblemListId { get; set; }
        public string Unit { get; set; }
        public string Modifier { get; set; }
        public string CPTCode { get; set; }
        public string CPT_DESCRIPTION { get; set; }
        public string ProblemListId_text { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMED_DESCRIPTION { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string VaccineHxId { get; set; }
        public string ImmTherInjectionId { get; set; }

        public string CPTId { get; set; }
        public string IsFromSupperBill { get; set; }
        public bool IsLabBasedCPT { get; set; }
        public string Surgical { get; set; }
        public string ReasonId { get; set; }
        public string CQMEncounterTypeId { get; set; }
    }
}
