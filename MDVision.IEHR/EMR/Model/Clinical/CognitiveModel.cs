using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class CognitiveModel
    {
        public string commandType { get; set; }
        public string CognitiveId { get; set; }
        public string NoteId { get; set; }
        public string PatientId { get; set; }
        public string SoapText { get; set; }
        public string CognitiveStatusId { get; set; }
        public string FunctionalStatusId { get; set; }
        public string MentalStatusId { get; set; }
        public string IsFromNote { get; set; }
        public List<CognitiveStatusModel> CognitiveStatusModel { get; set; }
        public List<FunctionalStatusModel> FunctionalStatusModel { get; set; }
        public List<MentalStatusModel> MentalStatusModel { get; set; }

        public CognitiveModel()
        {
            CognitiveStatusModel = new List<CognitiveStatusModel>();
            FunctionalStatusModel = new List<FunctionalStatusModel>();
            MentalStatusModel = new List<MentalStatusModel>();
        }
    }
}