using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class BirthHx
    {
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string BirthHxDate { get; set; }
        public string BirthHxUnremarkable { get; set; }
        public string BirthHxComments { get; set; }
        public string BirthHxSoapText { get; set; }
         
        public string IsGeneralUpdate { get; set; }
        public string IsNewbornUpdate { get; set; }
        public string IsDeliveryUpdate { get; set; } 



        public string commandType { get; set; }

        public string BirthHxType { get; set; }
        public string birthHxSection { get; set; }

        public string NotesId { get; set; }

    
    }

    //public class BirthHxResponseModel
    //{
    //    public BirthHxResponseModel()
    //    {
    //        BirthHxId = -1;
    //        GeneralId = -1;
    //        MaternalDeliveryId = -1;
    //        NewbornId = -1;
    //    }
    //    public long BirthHxId { get; set; }
    //    public long GeneralId { get; set; }
    //    public long MaternalDeliveryId { get; set; }
    //    public long NewbornId { get; set; }
    //    public string Message { get; set; }
    //    public bool status { get; set; }
    //}

}