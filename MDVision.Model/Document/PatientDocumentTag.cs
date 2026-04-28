using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Document
{
   public class PatientDocumentTag
    {

        public Int64 Id { get; set; }
        public Int64 PatDocId { get; set; }
        public Int64 TagId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
