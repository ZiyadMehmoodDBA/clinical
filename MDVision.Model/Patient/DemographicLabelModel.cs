using MDVision.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
    public class DemographicLabelModel
    {
        public DemographicLabelModel()
        {
            Settings = new EntityUserOptions();
        }
        public string PatientName { get; set; }
        public string PatientDOB { get; set; }
        public string PatientAddress { get; set; }
        public string InsurancePlan { get; set; }
        public string SubscriberID { get; set; }
        public string AccountNumber { get; set; }
        public string ProviderName { get; set; }
        public EntityUserOptions Settings; 
    }
}
