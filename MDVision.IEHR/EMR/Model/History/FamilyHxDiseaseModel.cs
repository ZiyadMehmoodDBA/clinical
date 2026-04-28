/*
 * Date: 20/01/2016
 * Author: Muhammad Irfan
 * Overview: This file is created to define the properties of FamilyHx Disease table
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Model.Native;

namespace MDVision.IEHR.EMR.Model.History
{
    public class FamilyHxDiseaseModel
    {
        public FamilyHxDiseaseModel()
        {
            DataChangeRequest = new List<DataChangeRequest>();
        }
       
        public string DiseaseId { get; set; }
        public string FamilyHxId { get; set; }
        public string ICD9Code { get; set; }
        public string ICD9CodeDescription { get; set; }
        public string ICD10Code { get; set; }
        public string ICD10CodeDescription { get; set; }
        public string SNOMEDID { get; set; }
        public string SNOMEDDescription { get; set; }
        public string LexiCode { get; set; }
        public string LexiCodeDescription { get; set; }        
        public string MedicalDiseaseComments { get; set; }
        public string commandType { get; set; }
        public long NotesId { get; set; }
		
        //Start//27/01/2016//Ahmad Raza//properties for soap text
        public string DiseaseText { get; set; }
        public string FamilyMemberText { get; set; }
        public string HealthStatusText { get; set; }
		//End//27/01/2016//Ahmad Raza//properties for soap text
		//Start 04-11-2016 Humaira Yousaf
        public string FamilyMemberId { get; set; }
		//Start 04-11-2016 Humaira Yousaf
        public string FreeTextICD { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ICDID { get; set; }
        public string AddedFromMobileApp { get; set; }
        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}