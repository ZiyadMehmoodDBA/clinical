using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Clinical
{
    public class BirthHxMaternalDeliveryModelNative
    {
        public BirthHxMaternalDeliveryModelNative()
        {
            DataChangeRequest = new List<MDVision.Model.Native.DataChangeRequest>();
            lstChangedColumns = new List<ChangedColumnsNative>();
        }
        public string MaternalDeliveryId { get; set; }
        public string BirthHxId { get; set; }
        public string PatientId { get; set; }
        public string Gestation { get; set; }
        public string NumberOfFetuses { get; set; }
        public string NumberOfLivingFetuses { get; set; }
        public string LaborLength { get; set; }
        public string DeliveryMethodId { get; set; }
        public string DeliveryPresentationId { get; set; }
        public string MaternalHistoryId { get; set; }

        public string SoapText { get; set; }

        public string MaternalHistoryId_text { get; set; }

        public string DeliveryPresentationId_text { get; set; }

        public string DeliveryMethodId_text { get; set; }

        public string MaternalDeliveryComments { get; set; }

        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ResponsiblePhysicianId_text { get; set; }

        public string listChangedColumns { get; set; }
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
