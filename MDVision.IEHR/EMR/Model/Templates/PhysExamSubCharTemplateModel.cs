/* Author: Farooq Ahmad
 * Created Date: 07/03/2016
 * OverView: Created to Model Physical Exam Templates Sub Characteristics
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class PhysExamSubCharTemplateModel
    {
        public string TemplateId { get; set; }
        public string TemplateSubCharId { get; set; }
        public string CharacteristicId { get; set; }
        public string SubCharacteristicId { get; set; }
        public string SubCharName { get; set; }

        public string IsModified { get; set; }
        public string IsChecked { get; set; }
        
    }
}