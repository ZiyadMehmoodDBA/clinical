using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.HPI
{
    public class HPIFindingsModel
    {
        public string HPIFindingsId { get; set; }
        public string HPISymptomFindingsId { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public string RecordCount { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string FindingOrder { get; set; }

    }
}