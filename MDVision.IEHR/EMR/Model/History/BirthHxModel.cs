/*This DTO is used for BirthHx in Birth History, 
  Author: Khaleel Ur Rehman
  Date: January 06, 2016*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class BirthHxModel
    {
        public long BirthHxId { get; set; }
        public long PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string BirthHxDate { get; set; }
        public bool BirthHxUnremarkable { get; set; }
        public string BirthHxComments { get; set; }
        public string BirthHxSoapText { get; set; }

        public string IsGeneralUpdate { get; set; }
        public string IsNewbornUpdate { get; set; }
        public string IsDeliveryUpdate { get; set; }

        public string commandType { get; set; }

        public string BirthHxType { get; set; }
        public string birthHxSection { get; set; }

        public long NotesId { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string AddFromMobile { get; set; }

        public BirthHxGeneralModel BirthHxGeneral { get; set; }
        public BirthHxMaternalDeliveryModel BirthHxMaternalDelivery { get; set; }
        public BirthHxNewbornModel BirthHxNewborn { get; set; }
    }

    public class BirthHxResponseModel {
        public BirthHxResponseModel()
        {
            BirthHxId = -1;
            GeneralId = -1;
            MaternalDeliveryId = -1;
            NewbornId = -1;
        }
        public long BirthHxId { get; set; }
        public long GeneralId { get; set; }
        public long MaternalDeliveryId { get; set; }
        public long NewbornId { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
    }
}