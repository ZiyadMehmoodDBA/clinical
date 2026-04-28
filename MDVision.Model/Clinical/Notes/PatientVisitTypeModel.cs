using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
  public   class PatientVisitTypeModel
    {
         public string VisitTypeID { get; set; }
        public string PatientTypeID { get; set; }
        public string PatientType { get; set; }
        public string VisitType { get; set; }
        public string Duration { get; set; }
    }
}
