using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Model.CCDA;
namespace MDVision.Model.CCDA
{
    public class HistoriesModel
    {
        public HistoriesModel()
        {
            this.Code = new CodeModel();
            this.Time = new TimeModel();
            this.Result = new HistoriesResult();
        } 
        public CodeModel Code { get; set; }
        public TimeModel Time { get; set; }
        public string StatusCode { get; set; }
        public string text { get; set; }
        public string VisitDate { get; set; }
        public string NegationValueset { get; set; }


        public HistoriesResult Result { get; set; }
        
    }
    public class HistoriesResult
    {
        public HistoriesResult() {
            this.ResultCode = new CodeModel();
        }
        public CodeModel ResultCode;
    }
}