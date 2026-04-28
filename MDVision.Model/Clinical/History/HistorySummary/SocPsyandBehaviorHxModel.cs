using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.History.HistorySummary
{
    public class SocPsyandBehaviorHxModel
    {
        public string Question { get; set; }
        public string QuestionnaireID { get; set; }
        public string commandType { get; set; }
        public string RecordCount { get; set; }
        public string Sequence { get; set; }
        public string AllAnswerIds { get; set; }
        public string PHQAnswerIds { get; set; }
        public string TotalScore { get; set; }
        public string PHQScore { get; set; }
        public string SocialandBehaviorHxId { get; set; }
        public string PatientId { get; set; }
        public string SocialBehaviorDate { get; set; }
        public string Unremarkable { get; set; }
        public string Comments { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public List<SocPsyQuestionAnswerModel> QuestionAnswerArray { get; set; }
        public string XMLQuestionAnswer { get; set; }
        public string PageNumber { get; set; }
        public string RowspPage { get; set; }
        public string Current { get; set; }
        public string NotesId { get; set; }
        public string SocialandBehaviorHxHistoryId { get; set; }
        public string ShowDetail { get; set; }
        public string SoapText { get; set; }
        public string AlcoholAnswerIds { get; set; }
        public string SocConnAndIsolAnswerIds { get; set; }
        public string ExposToViolIds { get; set; }
        public string AlcoholScore { get; set; }
        public string SocConnAndIsolScore { get; set; }
        public string ExposToViolScore { get; set; }


    }


    public class SocPsyQuestionAnswerModel
    {
        public string SocialandBehaviorQAId { get; set; }
        public string SocialandBehaviorHxId { get; set; }
        public string AnswerId { get; set; }
        public string QuestionnaireID { get; set; }
    }
}
