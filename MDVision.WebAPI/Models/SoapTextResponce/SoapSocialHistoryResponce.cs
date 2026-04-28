using MDVision.WebAPI.Models.SoapTextResponce;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class SoapSocialHistoryResponce
    {
        public bool status { get; set; }
        public string SocialHxFill_JSON { get; set; }

        public string TobaccoHxFill_JSON { get; set; }


        //                   TobaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
        //                   AlcoholHxFill_JSON = js.Serialize(lstAlcoholHx),
        //                   DrugAbuseFill_JSON = js.Serialize(lstDrugAbuse),
        //                   SexualHxFill_JSON = js.Serialize(lstSexualHx),
        //                   OccupationHxFill_JSON = js.Serialize(lstMiscHxOccupationHx),
        //                   //Start Farooq Ahmad 11/01/2016 Adding to Json Array
        //                   SleepHxFill_JSON = js.Serialize(lstMiscHxSleepHx),
        //                   ExercisesHxFill_JSON = js.Serialize(lstMiscHxExercisesHx),
        //                   HousingHxFill_JSON = js.Serialize(lstMiscHxHousingHx),
        //                   //End Farooq Ahmad 11/01/2016 Adding to Json Array
        //                   CaffeineIntakeHxFill_JSON = js.Serialize(lstMiscHxCaffeineIntakeHx),

        public string socialHxLoad_JSON { get; set; }
        public string socialHxMiscHxComponentLoad_JSON { get; set; }
        public string SoapText { get; set; }
        public string IsCreatedOrModified { get; set; }
        public string LastUpdated { get; set; }
        public string AlcoholHxFill_JSON { get; set; }
        public string DrugAbuseFill_JSON { get; set; }
        public string SexualHxFill_JSON { get; set; }

        // Faizan Ameen . Misceleneous History. 

        public string OccupationHxFill_JSON { get; set; }
        public string SleepHxFill_JSON { get; set; }
        public string ExercisesHxFill_JSON { get; set; }
        public string HousingHxFill_JSON { get; set; }
        public string CaffeineIntakeHxFill_JSON { get; set; }



    }
}