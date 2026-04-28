/*  
    Author: Muhammad Azhar Shahzad
    Creation Date: 12 April 2017
    OverView:This File is created for VBP Question Functionality
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class VBPModel
    {
        public string MeasureId { get; set; }
        public string QuestionAnswersId { get; set; }
        public string ProviderId { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string MeasureQuestionnaireId { get; set; }
        public string CPT { get; set; }
        public string Score { get; set; }
        public string IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string QuestionnaireID { get; set; }
        public string AnswerID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string MeasureType { get; set; }
        public string Comments { get; set; }
    }
}