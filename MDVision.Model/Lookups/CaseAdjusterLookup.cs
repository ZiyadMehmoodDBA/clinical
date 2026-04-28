using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Lookups
{
    public class CaseAdjusterLookup
    {
        public string CaseAdjusterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
