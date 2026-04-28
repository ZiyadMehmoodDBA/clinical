using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Lab
{
    public class ClinicalLabModel
    {
        public Int64 LabId { get; set; }
        public string LabName { get; set; }
        public string LabType { get; set; }
        public Int64 EntityId { get; set; }
        public string Comments { get; set; }    
        public Int64 iTotalDisplayRecords { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public string ClientNo { get; set; }

        public Int64 CategoryId { get; set; }

        public Int64 CodeSystemId { get; set; }

        public Int64 RequisitionTemplateId { get; set; }

        public Int64 LabTypeId { get; set; }
        public string moduleName { get; set; }
    }
}