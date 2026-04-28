using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Clinical
{
    public class BirthHxNewbornModelNative
    {
        public BirthHxNewbornModelNative()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
            lstChangedColumns = new List<ChangedColumnsNative>();
        }

        public string NewbornId { get; set; }
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string HeadCircumference { get; set; }
        public string ChestCircumference { get; set; }
        public string WeightAtBirth { get; set; }
        public string LengthAtBirth { get; set; }
        public string ApgarAtBirth { get; set; }
        public string ApgarAt5Minutes { get; set; }
        public string WeightReleased { get; set; }
        public string PatientBloodTypeId { get; set; }
        public string ProblemsAtBirthId { get; set; }
        public string bFetalDistress { get; set; }
        public string bFetalDistressYes { get; set; }
        public string bFetalDistressNo { get; set; }
        public string NewbornComments { get; set; }
        public string SoapText { get; set; }

        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }

        public string PatientBloodTypeId_text { get; set; }

        public string ProblemsAtBirthId_text { get; set; }

        public List<ChangedColumnsNative> lstChangedColumns { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
