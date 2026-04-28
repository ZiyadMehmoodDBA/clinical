using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Immunization
{
    public class Manufacturer
    {
        public string ManufacturerId { get; set; }
        public string VaccineId { get; set; }
        public string TherapeuticId { get; set; }
        public string ManufacturerName { get; set; }
        public string MVXCode { get; set; }
        public string Status { get; set; }
        public string commandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
