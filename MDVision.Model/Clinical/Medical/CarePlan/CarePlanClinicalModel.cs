using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical.CarePlan
{
    public class CarePlanClinicalModel
    {
        public string CarePlanId { get; set; }
        public long PatientId { get; set; }
        public string Comments { get; set; }
        public long ProviderId { get; set; }
        public long CareTeamId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        public string CarePlanType { get; set; }
        public List<CarePlanNoteModel> ItemsForNoteList { get; set; }
        public List<CarePlanNoteModel> DetachItemsNoteList { get; set; }
        public string NotesId { get; set; }
        public List<CarePlanGoalsModel> GoalsModelList { get; set; }
        public List<CarePlanHealthConcernsModel> ConcernsModelList { get; set; }
        public List<CarePlanInterventionsModel> InterventionsModelList { get; set; }
        public List<CarePlanOutcomesModel> OutcomesModelList { get; set; }


    }
    public class CarePlanResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string CarePlanId { get; set; }
    }

    public class CarePlanNoteModel
    {
        public string Id { get; set; }
        public string CarePlanValue { get; set; }
        public string CarePlanType { get; set; }
    }
}
