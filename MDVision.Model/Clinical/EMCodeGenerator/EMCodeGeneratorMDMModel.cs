using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.Clinical.EMCodeGenerator
{

    public class EMCodeGeneratorMDMModel
    {

       public Boolean HasLabOrderOrResult { get; set; }
       public Boolean HasLabResultRemarksOrComments { get; set; }
	   public Boolean HasRadiologyOrderOrResult { get; set; }
       public Boolean HasRadiologyResultRemarksOrComments { get; set; }
	   public int NoOfICD { get; set; }
    }
}