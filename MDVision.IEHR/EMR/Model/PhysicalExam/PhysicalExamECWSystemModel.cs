using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PhysicalExamECWSystemModel
    {
        public string commandType { get; set; }
        public string PESystemId { get; set; }
        public string PEObservationIds { get; set; }
        public string Observations { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string IsGlobal { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string PETemplateID { get; set; }
        public string PETemplateSystemId { get; set; }

    }

    public class PESystemModelResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}