using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Model.Dashboard
{
    public class DUserMessageModel
    {
        public string UserMessagesId { get; set; }
        public string AssignedToId { get; set; }
        public string Subject { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string RowspPage { get; set; }
        public string RecordCount { get; set; }
        public string PriorityName { get; set; }
    }
}
