using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.HPI
{
    public class HPISymptomsModel
    {
        public string HPISymptomsId { get; set; }
        public string HPIFindingsIds { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string IsGlobal { get; set; }
        public string HPISymptomsAnswersId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public string RecordCount { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string HPITemplateSymptomId { get; set; }
        public string HPITemplateId { get; set; }
        public string SymptomOrder { get; set; }
        public string FindingOrder { get; set; }

    }

    public class HPISymptomsModelResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class HPISymptomFindingModel
    {
        public string HPISymptomFindingsId { get; set; }
        public string HPIFindingsId { get; set; }
        public string HPISymptomsId { get; set; }
        public string HPIFindingsIds { get; set; }
        public string HPITemplateSymptomId { get; set; }
        public string FindingOrder { get; set; }

    }
}