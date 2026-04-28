using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class BirthHx_MaternalDeliveryHx
    {
        public long MaternalDeliveryId { get; set; }
        public long BirthHxId { get; set; }
        public string Gestation { get; set; }
        public string NumberOfFetuses { get; set; }
        public string NumberOfLivingFetuses { get; set; }
        public string LaborLength { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? DeliveryPresentationId { get; set; }
        public int? MaternalHistoryId { get; set; }

        public string SoapText { get; set; }

        public string MaternalHistoryId_text { get; set; }

        public string DeliveryPresentationId_text { get; set; }

        public string DeliveryMethodId_text { get; set; }

        public string MaternalDeliveryComments { get; set; }
    }
}