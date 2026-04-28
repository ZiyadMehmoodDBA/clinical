using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model.CCDA
{
    public class CCDAModels
    {


    }

    public class CodeModel
    {
        public string Code { get; set; }
        public string CPTCode { get; set; }
        public string SNOMEDID { get; set; }
        public string CodeType { get; set; }
        public string Description { get; set; }
        public bool NegationIndex { get; set; }
        public string NegationReason { get; set; }
        public string ActionPerformed { get; set; }
        public string PHQScore { get; set; }
    }
    public class TimeModel
    {
        public string low { get; set; }
        public string high { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ValidateModel
    {
        public string XMLContent { get; set; }
        public string FileType { get; set; }
        public string DocfileType { get; set; }
        public string DocfileName { get; set; }
        public bool IsFile { get; set; }
    }
}