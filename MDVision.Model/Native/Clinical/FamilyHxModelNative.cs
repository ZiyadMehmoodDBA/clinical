using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Clinical
{
    public class FamilyHxModelNative : NativeBaseModel
    {
        public string FamilyHxId { get; set; }
        public string PatientId { get; set; }
        public string FamilyHxDate { get; set; }
        public string FamilyHxUnremarkable { get; set; }
        public string FamilyOverallComments { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
        public long DiseaseId { get; set; }
        //Start 03-11-2016 Humaira Yousaf
        public int FamilyMemberId { get; set; }
        //End 03-11-2016 Humaira Yousaf
    }
}
