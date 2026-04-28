/*
 * Date: 20/01/2016
 * Author: Muhammad Irfan
 * Overview: This file is created to define the properties of FamilyHx table
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.History
{
    public class FamilyHxModel
    {

        public string FamilyHxId { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string FamilyHxDate { get; set; }
        public string FamilyHxUnremarkable { get; set; }
        public string FamilyOverallComments { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
        public long DiseaseId { get; set; }
		//Start 03-11-2016 Humaira Yousaf
        public int FamilyMemberId { get; set; }
        //End 03-11-2016 Humaira Yousaf
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string RequestStatus { get; set; }

        public string AddedFromMobileApp { get; set; }

        public List<FamilyHxFamilyMemberModel> FamilyMemberList { get; set; }
        public List<FamilyHxDiseaseModel> FamilyMemberDisease { get; set; }
        public List<FamilyHxFamilyMemberModel> FamilyMemberDiseaseDetail { get; set; }



        //public List<FamilyHxDiseaseModel> FamilyMemberDisease { get; set; }

    }
}