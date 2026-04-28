using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.LabResult
{
    public class LabOrderResultExternalPDFModel
    {
      public long LabOrderResultExternalPDFId { get; set; }
      public long LabOrderResultId { get; set; }
      public string FileName { get; set; }
      public string FileBase64 { get; set; }
      public string Status { get; set; }
      public string Lab { get; set; }
    }
}