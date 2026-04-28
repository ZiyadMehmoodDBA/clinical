using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Document
{
  public  class PatientVisitDOS
    {


        public string VisitId { get; set; } = string.Empty;
        public string DOSFrom { get; set; } = string.Empty;
        public int TotalVisit { get; set; } = 0;
    }
}
