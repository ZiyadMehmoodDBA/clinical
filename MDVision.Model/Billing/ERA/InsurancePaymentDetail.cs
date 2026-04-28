using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.ERA
{
   public class InsurancePaymentDetail
    {

        public long Id { get; set; }
        public string PayerName { get; set; }
        public int InsurancePlanId { get; set; }
        public string FacilityId { get; set; }
        public string CheckNo { get; set; }
        public string CheckDate { get; set; }
        public string CheckDepositDate { get; set; }
        public decimal CheckAmount { get; set; }
        public Int32 PostedStatusId { get; set; }
        public decimal PostedAmount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ToEntryDate { get; set; }
        public string FromEntryDate { get; set; }
        public string FromCheckDate { get; set; }
        public string ToCheckDate { get; set; }
        public string StatusName { get; set; }


    }
}
