using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.PatientStatement
{
    public class ClaimInfo
    {

        public Int64 PatientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Int64 FacilityId { get; set; }
        public DateTime? DOSFrom { get; set; }
        public DateTime? DOSTo { get; set; }
        public Int64 EntityId { get; set; }
        public Int64 SubmittedStatementId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime? StmntDate { get; set; }
        public string RelationShip { get; set; }

    }
}
