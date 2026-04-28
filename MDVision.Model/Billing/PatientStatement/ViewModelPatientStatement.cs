using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.PatientStatement
{
    public class ViewModelPatientStatement
    {

        public List<StatementHeader> statementHeader { get; set; }
        public List<StatementFooter> statementFooter { get; set; }
        public List<StatementDetail> statementDetail { get; set; }

    }
}
