using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxMiscHxExercisesModel
    {
        public string ExercisesHxId { get; set; }
        public string MiscHxId { get; set; }
        public string MiscChildStatus { get; set; }

        public string MiscChildStatusText { get; set; }
        public string ExercisesTypeText { get; set; }

        public string ExercisesType { get; set; }
        public string ExercisesDietText { get; set; }

       
        public string ExercisesDiet { get; set; }
        public string ExercisesComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }
        public bool IsLast { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}