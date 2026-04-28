using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class FamilyHx
    {
         public string FamilyHxDate { get; set; }
         public string FamilyHxId { get; set; }
         public string FamilyHxUnremarkable { get; set; }
         public string FamilyOverallComments { get; set; }
         public string SoapText { get; set; }
         public string IsCreatedOrModified { get; set; }
         public string LastUpdated { get; set; }
    }
}