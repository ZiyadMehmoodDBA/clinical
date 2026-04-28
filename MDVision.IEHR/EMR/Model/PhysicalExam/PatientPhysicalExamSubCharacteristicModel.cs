/*
 * Author: Muhammad Arshad
 * Created Date: 12/02/2016
 * Created to define properties of a sub-characteristic in PatientPhysicalExam          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamSubCharacteristicModel
    {
        public string SystemId { get; set; }
        public string SectionId { get; set; }
        public string IsPositive { get; set; }
        public string CharacteristicId { get; set; }
        public string SubCharacteristicId { get; set; }

        public string PatientPhysicalSubCharacteristicId { get; set; }
        public string SubCharacteristicDetail { get; set; }
        public string IsSubCharacteristicPositive { get; set; }

        private bool? _IsForComments;

        public bool? IsForComments
        {
            get
            {
                if (!_IsForComments.HasValue)
                    return false;
                return _IsForComments;
            }
            set { _IsForComments = value; }
        }

        public string Comments { get; set; }

        public PatientPhysicalExamSubCharacteristicDetailModel SubCharacteristicDetailModel { get; set; }

        public PatientPhysicalExamSubCharacteristicModel()
        {
            SubCharacteristicDetailModel = new PatientPhysicalExamSubCharacteristicDetailModel();
        }
    }
}