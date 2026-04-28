using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Templates.OrderSets
{
    public class OS_ProceduresModel
    {
        public string commandType { get; set; }
        public string ProcedureId { get; set; }
        public long OrderSetId { get; set; }
    
        public List<OS_ProceduresDetailModel> procedureDetailModel { get; set; }
        public string IsActive { get; set; }
    }
}
