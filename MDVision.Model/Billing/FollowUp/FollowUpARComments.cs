using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.FollowUp
{
   public class FollowUpARComments
    {
        public long Id { get; set; }
        public long VisitId { get; set; }
        public string followUpComments { get; set; }
        public bool IsFromClaim { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
