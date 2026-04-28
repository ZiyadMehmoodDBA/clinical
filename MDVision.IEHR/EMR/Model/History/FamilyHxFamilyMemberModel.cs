/*
 * Date: 20/01/2016
 * Author: Muhammad Irfan
 * Overview: This file is created to define the properties of FamilyHx Family Member table
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class FamilyHxFamilyMemberModel
    {
        public string MemberDetailId { get; set; }
        public string DiseaseId { get; set; }
        public string MemberId { get; set; }
        public string HealthStatus { get; set; }
        public string YearofBirth { get; set; }
        public string RadRelativeDied { get; set; }
        public string AgeAtDeath { get; set; }
        public string AgeAtDiagnosis { get; set; }
        public string FamilyMemberComments { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
        //Start//27/01/2016//Ahmad Raza//properties for soap text
        public string DiseaseText { get; set; }
        public string FamilyMemberText { get; set; }
        public string HealthStatusText { get; set; }
        //End//27/01/2016//Ahmad Raza//properties for soap text
		//Start 03-11-2016 Humaira Yousaf
        public string FamilyHxId { get; set; }



        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ICDID { get; set; }
        public string AddedFromMobileApp { get; set; }
        //Start 03-11-2016 Humaira Yousaf

        public List<FamilyHxDiseaseModel> FamilyMemberDiseases { get; set; }
        public string PatientId { get; set; }
    }
}