using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.Model
{
    public class GlobalQuestionModel
    {

        public string QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string commandType { get; set; }
        public string QuestionHTML { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
        public long PageNumber { get; set; }
        public long RowsPerPage { get; set; }
        public int? EntityId { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class GlobalQuestionResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string QuestionId { get; set; }
    }
}