/*This DTO is used for BirthHx New Born in Birth History, 
  Author: Khaleel Ur Rehman
  Date: January 06, 2016*/
using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class BirthHxNewbornModel
    {
       public BirthHxNewbornModel()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
            lstChangedColumns = new List<ChangedColumnsNative>();
        }

        public long NewbornId { get; set; }
        public long BirthHxId { get; set; }
        public float? HeadCircumference { get; set; }
        public float? ChestCircumference { get; set; }
        public float? WeightAtBirth { get; set; }
        public float? LengthAtBirth { get; set; }
        public string ApgarAtBirth { get; set; }
        public string ApgarAt5Minutes { get; set; }
        public float? WeightReleased { get; set; }
        public int? PatientBloodTypeId { get; set; }
        public int? ProblemsAtBirthId { get; set; }
        public bool? bFetalDistress { get; set; }
        public bool bFetalDistressYes { get; set; }
        public bool bFetalDistressNo { get; set; }
        public string NewbornComments { get; set; }
        public string SoapText { get; set; }

        public string PatientBloodTypeId_text { get; set; }

        public string ProblemsAtBirthId_text { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddFromMobile { get; set; }

        public string listChangedColumns { get; set; }
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }

}