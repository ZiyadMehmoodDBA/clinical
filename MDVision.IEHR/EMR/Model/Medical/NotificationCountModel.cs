using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class NotificationCountModel
    {
        public long NoficationID { get; set; }
        public int RefillPrescriptionCount { get; set; }
        public int PendingPrescriptionCount { get; set; }
    }
}