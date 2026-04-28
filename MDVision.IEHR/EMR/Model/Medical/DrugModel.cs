using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class DrugModel
    {
        public string RcopiaID { get; set; }
        public string NDCID { get; set; }
        public string FirstDataBankMedID { get; set; }
        public string DrugDescription { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Schedule { get; set; }
        public string BrandType { get; set; }
        public string LegendStatus { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public string Strength { get; set; }
        public string RxnormID { get; set; }
        public string RxnormIDType { get; set; }
        public string Message { get; set; }

    }
}