using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM
{
    public class CCMTemplateModel
    {
        public long TemplateId { get; set; }
        public long TempLookupId { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int RecordCount { get; set; }
        public string ICDGroupIds { get; set; }
        public string  commandType { get; set; }

    }
    public class QuestionModel
    {
        public long QuestionId { get; set; }
        public string Description { get; set; }
        public string QuestionHTML { get; set; }
        public long ParentQuestId { get; set; }

    }
    public class SectionModel
    {
        public long SectionId { get; set; }
        public string ShortName { get; set; }

    }
    public class SectionQuestionsModel
    {
        public long SectionId { get; set; }
        public long QuestionId { get; set; }

    }
}
