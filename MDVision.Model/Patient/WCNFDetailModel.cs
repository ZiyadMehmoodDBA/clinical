using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
   public class WCNFDetailModel
    {
        public int WCNFDetailId { get; set; }
        public int CaseMgmtId { get; set; }
        public int PatientId { get; set; }
        public Nullable<int> CaseAdjusterId { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PreAuth { get; set; }
        public Nullable<DateTime> InjuryDate { get; set; }
        public string Referral { get; set; }
        public Nullable<bool> EmployementRelated { get; set; }
        public string CauseOfAccident { get; set; }
        public string Accident { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Hour { get; set; }
        public Nullable<DateTime> HCFAFIELD16DATEFROM { get; set; }
        public Nullable <DateTime> HCFAFIELD16DATETO { get; set; }
        public Nullable<DateTime> HCFAFIELD18DATEFROM { get; set; }
        public Nullable<DateTime> HCFAFIELD18DATETO { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        public Nullable< DateTime> ModifiedOn { get; set; }

    }
}
