/* Author: M Ahmad Imran
 * OverView: Model for Complaints Detail
 * Date : Feb 11, 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class ComplaintDetailModel
    {
        public string ComplaintId { get; set; }
        public string PreviousHistory { get; set; }
        public string IsChiefComplaint { get; set; }
        public string Complaint_CaseId { get; set; }
        public string Complaint_LocationIds { get; set; }
        public string Complaint_RadiationId { get; set; }
        public string Complaint_QualityId { get; set; }
        public string Complaint_SeverityId { get; set; }
        public string Onset { get; set; }
        public string Complaint_DurationId { get; set; }
        public string Duration { get; set; }
        public string Complaint_FrequencyId { get; set; }
        public string Complaint_ContextId { get; set; }
        public string Complaint_CharacterIds { get; set; }
        public string AssociatedWith { get; set; }
        public string PrecipitatedBy { get; set; }
        public string Complaint_AggravatedById { get; set; }
        public string Complaint_RelievedById { get; set; }
        public string Comments { get; set; }
        public string ComplaintDescription { get; set; }
        public string ComplaintDetailId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string SoapText { get; set; }
        public string NotesComplaintId { get; set; }

        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public string SNOMED { get; set; }
        public string ICD9Description { get; set; }
        public string ICD10Description { get; set; }
        public string SNOMEDDescription { get; set; }
        public string ProblemListId { get; set; }
        public string IsReported { get; set; }
        public string IsBodyPart { get; set; }
    }
}