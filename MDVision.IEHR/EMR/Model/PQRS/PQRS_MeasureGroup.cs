using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PQRS
{
    public class PQRS_MeasureGroupSearchModel
    {
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public string commandType { get; set; }
        public string MeasureGroupName { get; set; }
        public string ProviderIds { get; set; }
        public string IsActive { get; set; }
        public Int64 MeasureGroupId { get; set; }
    }

    public class PQRS_MeasureGroupFillModel
    {
        public string MeasureGroupId { get; set; }
        public string MeasureGroupsName { get; set; }
        public string SubmissionYear { get; set; }
        public Int64 EntityId { get; set; }
        public bool IsReported { get; set; }
        public bool IsActive { get; set; }
        public string ProviderIds { get; set; }
        public string MeasureIds { get; set; }
        public string PracticeIds { get; set; }
        public string SpecialtyIds { get; set; }
        public string CQMMeasureIds { get; set; }
        public string MeasureType { get; set; }
    }

    public class PQRS_MeasureGroupSelectModel
    {
        public string measureGroupName { get; set; }
        public string providersName { get; set; }
        public string createdOn { get; set; }
        public Int64 measureGroupId { get; set; }
        public bool IsActive { get; set; }
        public bool IsReported { get; set; }
    }
    public class PQRS_MeasureSearchModel {
        public Int64? MeasureId { get; set; }
        //public string MeasureNumber { get; set; }
        //public string MeasureTitle { get; set; }
        //public string NQSDomain { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        //public Int64 EntityId { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedOn { get; set; }

        //public string ModifiedBy { get; set; }
        //public string ModifiedOn { get; set; }
        public string commandType { get; set; }
        //public bool IsActive { get; set; }
        public string MeasureNumber { get; set; }
    }


    public class PQRS_MeasureSelectModel
    {
        public Int64? MeasureId { get; set; }
        public string MeasureNumber { get; set; }
        public string MeasureTitle { get; set; }
        public string NQSDomain { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string MeasureType { get; set; }
        public string ActivityWeighting { get; set; }
        public string HighPriority { get; set; }
        
    }

    public class PQRS_GetPatientsFromVisits
    {
        public string DOB { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public long VisitId { get; set; }
        public long PatientId { get; set; }
    }
    public class PQRS_GetMeasureReasons
    {
        public string CodeType { get; internal set; }
        public string Reason { get; internal set; }
        public string ReasonCode { get; internal set; }
        public long ReasonId { get; internal set; }
    }
}