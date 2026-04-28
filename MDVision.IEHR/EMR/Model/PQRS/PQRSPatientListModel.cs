using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PQRS
{
    public class PQRSPatientListModel
    {
        public string PatientId { get; set; }
        public string Measure { get; set; }
       // public string MissingData { get; set; }
        public string CommandType { get; set; }

        public string json { get; set; }

        public List<MissingData> missingDataList = new List<MissingData>();
    }


    public  class MissingData
    {
        public string  TreatmentType { get; set; }
        public string  ReasonType { get; set; }
        public string ReasonComments { get; set; }
    }
}