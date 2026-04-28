/*
 * Author: Muhammad Arshad
 * Created Date: 08/12/2015
 * Created to define properties of SexualHx in SocialHx          
 */

using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxSexualHxModel
    {
        public string SexualHxId { get; set; }
        public string SocialHxId { get; set; }
        public string SexualStatus { get; set; }
        public string SexualPreferences { get; set; }
        public string SexualbUsingProtection { get; set; }
        public string SexualProtectionMethod { get; set; }
        public string SexualProtectionPeriod { get; set; }
        public string RadSexualPainWithIntercourse { get; set; }
       // public string RadSexualNoPainWithIntercourse { get; set; }
        public string SexualComplaints { get; set; }
        public string SexualExposedToSTD { get; set; }
        public string SexualSTD { get; set; }
        public string SexualCessationPeriod { get; set; }
        public string RadSexualAbusedSexually { get; set; }
        public string RadSexualPregnant { get; set; }
        public string SexualHxPregnancyDuration { get; set; }
        //public string RadSexualNoAbusedSexually { get; set; }
        public string SexualLMP { get; set; }
        public string SexualComments { get; set; }

        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: These properties are being used in Progress note Soap Text Creation
             Created Date: Dec 15, 2015
         */
        public string Status { get; set; }

        public string ProtectionMethod { get; set; }

        public bool bPainWithIntercourse { get; set; }

        public string Preference { get; set; }

        public string Complaint { get; set; }

        public bool bSexuallyAbused { get; set; }

        public bool bExposedToSTD { get; set; }

        public bool bUSingProtection { get; set; }

        public string LMP { get; set; }

        public string Comments { get; set; }

        
        public string SexualStatus_text { get; set; }

        public string SexualSTD_text { get; set; }

        public string SexualProtectionMethod_text { get; set; }

        public string SexualProtectionPeriod_text { get; set; }

        public string SexualPreferences_text { get; set; }

        public string SexualComplaints_text { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}