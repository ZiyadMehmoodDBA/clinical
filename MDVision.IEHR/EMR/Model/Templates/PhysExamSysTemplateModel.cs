/* Author: Farooq Ahmad
 * Created Date: 10/03/2016
 * OverView: Created to Model Physical Exam Templates System
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class PhysExamSysTemplateModel
    {
        public string TemplateId { get; set; }
        public string TemplateSysId { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string IsChecked { get; set; }

        public string IsModified { get; set; }
        public List<PhysExamSecTemplateModel> Sections { get; set; }
        public String NotesId { get; set; }
    }
}