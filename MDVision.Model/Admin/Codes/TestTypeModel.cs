using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Admin.Codes
{
    public class TestTypeModel
    {
        public string TestTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CodeSystem { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
    }
}
