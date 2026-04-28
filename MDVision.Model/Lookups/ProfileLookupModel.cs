using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Lookups
{
   public class ProfileLookupModel
    {
       public string ProviderId { get; set; }
       public string FacilityId { get; set; }
       public string SpecialityId { get; set; }
       public string PatientId { get; set; }
       public string ShortName { get; set; }
       public string EntityShortName { get; set; }
    }
}
