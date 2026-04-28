using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PhysicalExamECWObservationModel
    {
        public string commandType { get; set; }
        public string PESystemId { get; set; }
        public string PESystemObservationId { get; set; }
        public string PETemplateSystemId { get; set; }
        public string PEObservationId { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
    }
}