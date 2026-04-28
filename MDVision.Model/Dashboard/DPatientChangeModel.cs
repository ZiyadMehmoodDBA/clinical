using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DPatientChangeModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string ProfileName { get; set; }
        public string RecordCount { get; set; }
        public string DBAuditAction { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserName { get; set; }
        public string CreatedDate { get; set; }
        public string ProviderName { get; set; }
    }
}
