using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Document
{
    public class DocumentTag
    {

        public string TagId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }= string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
        public string RecordCount { get; set; } = string.Empty;
         
    }
}
