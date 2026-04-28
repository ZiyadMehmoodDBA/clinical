using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Lookups
{
  public  class ProviderParticipationStatusModel
    {

        public string ProviderParticipentStatusId { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
