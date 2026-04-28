using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.EMCodeGenerator
{
    public class EMCodeGeneratorExamModel
    {

    public long PatientPhysicalExamSystemId { get; set; }
    public int NoOfElements { get; set; }
    public string SystemName { get; set; }
    public string SystemDescription { get; set; }
    public int TotalNoOfElementsInSystem { get; set; }
    }
}