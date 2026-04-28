using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Immunization
{
    public class ImmunizationQueryModel
    {
        public string PatientId { get; set; }
        public string QueryId { get; set; }
        public string MrnStateIDRegisteryId { get; set; }
        public string MrnStateIDRegisteryData { get; set; }
        public string BirthIndicator { get; set; }
        public string BirthOrder { get; set; }
        public string HistoryForecastTime { get; set; }
        public string RequestDateTime { get; set; }
        public string GivenBy { get; set; }
        public string Status { get; set; }
        public string HL7Message { get; set; }
        public string pageNumber { get; set; }
        public string rowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string fileData { get; set; }
        public string fileName { get; set; }
        public string AcknowledgementFile { get; set; }
        public string VaccineHxId { get; set; }
    }
}
