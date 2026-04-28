using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.PMSSchedule
{
    public class AppointmentStatusOptionModel
    {
        public string Status { get; set; }
        public string PossibleOption { get; set; }
        public string Color { get; set; }
        public string DestinationStatusId { get; set; }
    }
}
