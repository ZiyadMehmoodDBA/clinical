using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FaceSheet
{
    public class FaceSheetModel
    {
        public string FaceSheetId { get; set; }
        public string UserId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentOrder { get; set; }
        public string RecordCount { get; set; }
        public string SortOrder { get; set; }
        public string FromComponentName { get; set; }
        public string ToComponentName { get; set; }
        public string commandType { get; set; }
        public string PatientId { get; set; }
        public string FaceSheetComponentSorted { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public List<string> PrintComponents { get; set; }
    }
}