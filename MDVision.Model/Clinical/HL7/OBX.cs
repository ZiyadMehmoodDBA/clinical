using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.HL7
{
    public class OBX
    {
        public string SetIDOBX { get; set; }
        public string ValueType { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
        public string QuestionCodeType { get; set; }

        public string AnswerCode { get; set; }
        public string Answer { get; set; }
        public string AnswerCodeType { get; set; }
    }
}
