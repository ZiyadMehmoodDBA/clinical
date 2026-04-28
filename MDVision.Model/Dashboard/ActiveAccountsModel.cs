using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Dashboard
{
    public class ActiveAccountsModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime DOB { get; set; }
        public string AccountNo { get; set; }
        public string Insurance { get; set; }
        public string Provider { get; set; }
        public string PatientPortalAccountsCount { get; set; }
    }

    public class PatientPortalAccounts
    {
        public List<ActiveAccountsModel> listActiveAccounts { get; set; }
    }
}
