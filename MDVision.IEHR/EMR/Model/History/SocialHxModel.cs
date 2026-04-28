using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class SocialHxModel
    {
        public string SocialHxId { get; set; }
        public string UserId { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        
        public string SocialHxDate { get; set; }
        public string SocialHxUnremarkable { get; set; }
        public string SocialComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }
        public string Status { get; set; }
        public string TabbacooData { get; set; }
        public string AlcoholData { get; set; }
        public string DrugAbuseData { get; set; }
        public string SexualData { get; set; }
        public string OccupationData { get; set; }
        public string SleepData { get; set; }
        public string ExerciseData { get; set; }
        public string HousingData { get; set; }
        public string CaffeineData { get; set; }
        public string TravelData { get; set; }
        public string IsSynced { get; set; }
        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: This property is being used to attach Social History to Progress note
             Created Date: Dec 15, 2015
         */
        public long NotesId { get; set; }

        // Date: 14/01/2016
        // Author: Muhammad Irfan
        // Overview: This property is used for sorting miscHx sorting
        public string MiscComponentSortedOrder { get; set; }
        public List<SocialHxTobaccoModel> lstTobaccoModel { get; set; }
        public List<SocialHxAlcoholModel> lstAlcoholModel { get; set; }
        public List<SocialHxDrugAbuseModel> lstDrugAbuseModel { get; set; }
        public List<SocialHxSexualHxModel> lstSexualHxModel { get; set; }
        public List<SocialHxMiscHxOccupationModel> lstOccupationHxModel { get; set; }
        public List<SocialHxMiscHxSleepModel> lstSleepHxModel { get; set; }
        public List<SocialHxMiscHxExercisesModel> lstExercisesHxModel { get; set; }
        public List<SocialHxMiscHxHousingModel> lstHousingHxModel { get; set; }
        public List<SocialHxMiscHxCaffeineIntakeModel> lstCaffeineIntakHxModel { get; set; }
    }

}