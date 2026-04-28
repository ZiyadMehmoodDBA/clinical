namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxMiscHxTravelHxModel
    {
        public string MiscHxId { get; set; }
        public string SocialHxId { get; set; }
        public string TravelHxId { get; set; }
        public string MiscChildStatus { get; set; }
        public string MiscChildStatusText { get; set; }
        public string TravelHxFromDate { get; set; }
        public string TravelHxToDate { get; set; }
        public string TravelHxComments { get; set; }
        public string TravelHxLocation { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }
        public string PatientId { get; set; }
        public string SocialHxDate { get; set; }
        public string SocialHxUnremarkable { get; set; }
        public string SocialComments { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public string SoapText { get; set; }
    }
}