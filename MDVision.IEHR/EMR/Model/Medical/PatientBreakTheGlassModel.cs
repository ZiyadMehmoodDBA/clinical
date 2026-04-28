/// Author: ZeeshanAK
/// Purpose:  Created to handel Break The Glass
/// Date : April 16, 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Patient
{
    public class PatientBreakTheGlassModel
    {
        public Int64 PatUserBreakGlassId { get; set; }
        public Int64 UserId { get; set; }

        public bool IsBreakGlassAllow { get; set; }
        public string PatientId { get; set; }
        
        public string commandType { get; set; }
        public string InActiveChkBoxValue { get; set; }
        public string InActiveReason { get; set; }
        public string IsActiveRecord { get; set; }
        public string IsActive { get; set; }

        public string VisitId { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string BreakGlassReason { get; set; }
    }
}