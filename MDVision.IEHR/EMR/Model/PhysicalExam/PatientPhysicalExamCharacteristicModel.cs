/*
 * Author: Muhammad Arshad
 * Created Date: 12/02/2016
 * Created to define properties of a characteristic in PatientPhysicalExam          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamCharacteristicModel
    {
        public string SystemId { get; set; }
        public string SectionId { get; set; }
        public string CharacteristicId { get; set; }
        public string CharacteristicDetail { get; set; }
        public string IsCharacteristicPositive { get; set; }

        private bool? _IsForComments;

        public bool IsForComments
        {
            get
            {
                if (!_IsForComments.HasValue)
                    return false;
                return _IsForComments.Value;
            }
            set { _IsForComments = value; }
        }


        public string PhysicalExamCharacteristicId { get; set; }
        public string Comments { get; set; }
        public string IsPositive { get; set; }
        public string SectionCharacteristicId { get; set; }

        public string Id { get; set; }
        public PatientPhysicalExamCharacteristicDetailModel SectionCharacteristicDetailModel { get; set; }

        public List<PatientPhysicalExamSubCharacteristicModel> SubCharacteristics { get; set; }

        public PatientPhysicalExamCharacteristicModel()
        {
            SectionCharacteristicDetailModel = new PatientPhysicalExamCharacteristicDetailModel();
            SubCharacteristics = new List<PatientPhysicalExamSubCharacteristicModel>();
        }
    }
}