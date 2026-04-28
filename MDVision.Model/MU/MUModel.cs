using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.MU
{
    public class MUModel
    {
        public List<MUAlertsModel> MUAlerts { get; set; }
        public string CommandType { get; set; }
        public long PatientId { get; set; }
        public bool IsShowAlert { get; set; }
        public string Type { get; set; }
        public string ProfileName { get; set; }
        public bool IsFromNote { get; set; }
        public MUModel()
        {
            MUAlerts = new List<MUAlertsModel>();
        }
    }
}
