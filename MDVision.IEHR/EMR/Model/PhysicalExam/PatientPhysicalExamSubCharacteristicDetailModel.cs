/*
 * Author: Muhammad Arshad
 * Created Date: 12/02/2016
 * Created to define properties of SubCharacteristic Detail against a sub-characteristic in PatientPhysicalExam          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamSubCharacteristicDetailModel
    {
        public string DetailId { get; set; }
        public string SubCharacteristicId { get; set; }
        public string PhysicalExamPreviousHistory { get; set; }
        public string PhysicalExamStatus { get; set; }
        public string PhysicalExamOnset { get; set; }
        public string PhysicalExamDurationLength { get; set; }
        public string PhysicalExamDurationPeriod { get; set; }
        public string PhysicalExamPattern { get; set; }
        public string PhysicalExamSeverity { get; set; }
        public string PhysicalExamCourse { get; set; }
        public string PhysicalExamRadiation { get; set; }
        public string PhysicalExamFrequency { get; set; }
        public string PhysicalExamContext { get; set; }
        public string PhysicalExamCharacter { get; set; }
        public string PhysicalExamAgggravatedby { get; set; }
        public string PhysicalExamRelievedby { get; set; }
        public string PhysicalExamLocation { get; set; }
        public string PhysicalExamPercipitatedby { get; set; }
        public string PhysicalExamAssociatedwith { get; set; }

        //Start//16-02-2016//Ahmad Raza//Properties added to get text value of dropdowns
        public string PhysicalExamStatus_text { get; set; }
        public string PhysicalExamContext_text { get; set; }
        public string PhysicalExamDurationPeriod_text { get; set; }
        public string PhysicalExamPattern_text { get; set; }
        public string PhysicalExamSeverity_text { get; set; }
        public string PhysicalExamCourse_text { get; set; }
        public string PhysicalExamCharacter_text { get; set; }
        public string PhysicalExamAgggravatedby_text { get; set; }
        public string PhysicalExamRelievedby_text { get; set; }
        public string PhysicalExamRadiation_text { get; set; }
        public string PhysicalExamFrequency_text { get; set; }
        //End//16-02-2016//Ahmad Raza//Properties added to get text value of dropdowns

        public string SystemId { get; set; }
        public string SectionId { get; set; }
        public string CharacteristicId { get; set; }
        public string IsCharacteristicPositive { get; set; }
        public string IsSubCharacteristicPositive { get; set; }




    }
}