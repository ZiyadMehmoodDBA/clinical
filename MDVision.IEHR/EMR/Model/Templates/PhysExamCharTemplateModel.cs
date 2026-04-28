/* Author: Farooq Ahmad
 * Created Date: 10/03/2016
 * OverView: Created to Model Physical Exam Templates Characteristics
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class PhysExamCharTemplateModel
    {
        public string TemplateId { get; set; }
        public string TemplateCharId { get; set; }
        public string CharacteristicId { get; set; }
        public string SectionId { get; set; }
        public string CharName { get; set; }
        public string IsChecked { get; set; }

        public string IsModified { get; set; }
        public List<PhysExamSubCharTemplateModel> SubCharacteristics { get; set; }
    }
}