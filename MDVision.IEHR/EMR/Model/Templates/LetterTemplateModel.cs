/*
By: Khaleel Ur Rehman
Purpose : Model for Letter Template
Date : 02-03-2016
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Templates
{
    public class LetterTemplateModel
    {

        public long TemplateLetterId { get; set; }
        public string SignedText { get; set; }
        public string TemplateLetterIds { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public long EntityId { get; set; }
        public string TemplateContent { get; set; }
        public string TagIds{get; set;}
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int RowsPerPage { get; set; }
        public int PageNumber { get; set; }
        public string ddlCategory { get; set; }
        public string commandType { get; set; }

        //---------For Patient Template Letter
        
        public long Patient_Letter_Id { get; set; }
        public long PatientId { get; set; }
        public string mode { get; set; }
        public string PatientLetterContent { get; set; }
        public string Status { get; set; }
        public long ProviderId { get; set; }

        public byte[] Base64Content { get; set; }
        public string Base64String { get; set; }
        //------------------------------------
        public string ProviderNames { get; set; }
        public string ProviderIds { get; set; }
        public string SpecialtyNames { get; set; }
        public string SpecialtyIds { get; set; }
        public string LabOrderResultId { get; set; }
        public string LOINC { get; set; }
        public string LabLetterBase64 { get; set; }
        public string LabResultBase64 { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string VisitDateTo { get; set; }
        public long NotesId { get; set; }
    }
}