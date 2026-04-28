using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class BirthHxLoad
    {
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string BirthHxDate { get; set; }
        public string bUnremarkable { get; set; }
        public string Comments { get; set; }
        public string SoapText { get; set; }

        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedOn { get; set; }
        public string GeneralId { get; set; }
        public string MaternalDeliveryId { get; set; }
        public string NewbornId { get; set; }

        public string NotesId { get; set; }
       
    }
}