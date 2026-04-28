using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CDS
{
    public class CDSProblemListModel
    {
        public long CDSProblemId { get; set; }
        public string CDSId { get; set; }
        public string Problem { get; set; }
        public string Comments { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string CDSProblemQuery { get; set; }
        public string ProblemOperator { get; set; }
        public string ProblemForQuery { get; set; }
    }
}