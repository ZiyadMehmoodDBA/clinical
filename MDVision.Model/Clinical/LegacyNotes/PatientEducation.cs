using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class PatientEducation
    {
        public string Type { get; set; }
        public Int64 PatEducationId { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
