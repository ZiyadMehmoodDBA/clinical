using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxMiscHxOccupationModel
    {
        public string OccupationHxId { get; set; }
        public string MiscHxId { get; set; }
        public string SocialHxId { get; set; }
        public string SocialHxDate { get; set; }
        public string SocialComments { get; set; }
        public string SocialHxUnremarkable { get; set; }
        public string OccupationHxDetails { get; set; }
        public string MiscChildStatus { get; set; }
        public string OccupationHxStartDate { get; set; }
        public string OccupationHxEndDate { get; set; }
        public string PatientId { get; set; }
        public long RowsPerPage { get; set; }
        public long PageNumber { get; set; }
        //Start 08/01/2016 Syed Zia Code, for Status Text
        public string MiscChildStatusText { get; set; }
        //Start 08/01/2016 Syed Zia Code, for Status Text
        public string OccupationPresent { get; set; }
        public string OccupationPast { get; set; }
        public string OccupationComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }
        public bool IsLast { get; set; }
        public string RadOccupation { get; set; } //0 means Present 1 means Past
        public List<DataChangeRequest> DataChangeRequest { get; set; }

    }
}