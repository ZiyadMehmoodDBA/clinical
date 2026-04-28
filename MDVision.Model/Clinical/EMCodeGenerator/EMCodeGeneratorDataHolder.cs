using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.EMCodeGenerator
{
    public class EMCodeGeneratorDataHolder
    {
        public EMCodeGeneratorDataHolder()
        {

            this._EMCodeHistory = new EMCodeGeneratorHistoryModel();
            this._EMCodeExam = new List<EMCodeGeneratorExamModel>();
            this._EMCodeMDM = new EMCodeGeneratorMDMModel();
        }
        public string ErrorMessage { get; set; }
        public EMCodeGeneratorHistoryModel _EMCodeHistory;
        public List<EMCodeGeneratorExamModel> _EMCodeExam;
        public EMCodeGeneratorMDMModel _EMCodeMDM;
  
    }
}
