/* Author: Farooq Ahmad
 * Created Date: 07/03/2016
 * OverView: Created to Model Physical Exam Templates Sections
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class PhysExamSecTemplateModel
    {
        public string TemplateId { get; set; }
        public string TemplateSectionId { get; set; }
        public string SystemId { get; set; }
        public string SectionId { get; set; }
        public string SectionName { get; set; }
        public string IsChecked { get; set; }

        public string IsModified { get; set; }
        public List<PhysExamCharTemplateModel> Characteristics { get; set; }
    }
}