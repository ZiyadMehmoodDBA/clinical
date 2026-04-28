using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Common
{
    public class CommonSearch
    {
        public Int64 NotesId { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 ProviderId { get; set; }
        public Int64 TemplateId { get; set; }
        public Int64 EntityID { get; set; }
        public Int64 ROSDataTemplateId { get; set; }
        public Int64 UserID { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
