using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Document
{
    public class DocumentActivity
    {

        public string DocumentActivityLogId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string CreatedBy { get; set; }= string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
        public string RecordCount { get; set; } = string.Empty;
         
    }
}
