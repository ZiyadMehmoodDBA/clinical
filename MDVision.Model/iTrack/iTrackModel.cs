using MDVision.Model.Common;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace MDVision.Model.iTrack
{
    public class iTrackModel
    {
        public string stage { get; set; }
        public string commandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }

    }

    public class Dashboard : IBaseModel
    {
        public string ProviderId { get; set; }
        public string Year { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string commandType { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string Category { get; set; }
        public string value { get; set; }
        public string points { get; set; }
        public string color { get; set; }
        public string GroupId { get; set; }
        public string TIN { get; set; }
        public string ReportingMethod { get; set; }


        public void Map(IDataReader reader, List<string> incommingColumnList)
        {

            Category = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Category", incommingColumnList));
            value = ModelUtility.ToStr(ModelUtility.MapValue(reader, "value", incommingColumnList));
            color = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Color", incommingColumnList));
            points = ModelUtility.ToStr(ModelUtility.MapValue(reader, "points", incommingColumnList));
        }

    }
    public class IndvidualProvider : IBaseModel
    {
        [XmlIgnore]
        public string GroupId { get; set; }
        [XmlIgnore]
        public string ShortName { get; set; }
        [XmlIgnore]
        public string GroupName { get; set; }
        [XmlIgnore]
        public string GroupTIN { get; set; }
        [XmlIgnore]
        public string GroupComments { get; set; }
        [XmlIgnore]
        public string TIN { get; set; }
        public string ProviderName { get; set; }
        [XmlIgnore]
        public string PracticeId { get; set; }
        [XmlIgnore]
        public string ReportingReason { get; set; }
        [XmlIgnore]
        public string IsActive { get; set; }
        [XmlIgnore]
        public string ModifiedBy { get; set; }
        [XmlIgnore]
        public string ModifiedOn { get; set; }
        [XmlIgnore]
        public string IsReporting { get; set; }
        [XmlIgnore]
        public string PracticeType { get; set; }
        [XmlIgnore]
        public string OtherComments { get; set; }
        [XmlIgnore]
        public string SubmissionYear { get; set; }
        [XmlIgnore]
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string xml { get; set; }
        [XmlIgnore]
        public string ReportingMethod { get; set; }
        [XmlIgnore]
        public string PageNumber { get; set; }
        [XmlIgnore]
        public string RowsPerPage { get; set; }
        [XmlIgnore]
        public string RecordCount { get; set; }
        [XmlIgnore]
        public string PerformanceYear { get; set; }
        [XmlIgnore]
        public string CreatedOn { get; set; }
        [XmlIgnore]
        public string CreatedBy { get; set; }
        [XmlIgnore]
        public string commandType { get; set; }
        [XmlIgnore]
        public string ProviderId { get; set; }
        [XmlIgnore]
        public string ReportingYear { get; set; }
        
        public List<MIPSGroupDetail> MIPSCatData { get; set; }
        [XmlIgnore]
        public string LastName { get; set; }
        [XmlIgnore]
        public string FirstName { get; set; }
        [XmlIgnore]
        public string Specialty { get; set; }
        [XmlIgnore]
        public string NPI { get; set; }
        [XmlIgnore]
        public string PracticeName { get; set; }
        [XmlIgnore]
        public string Facility { get; set; }
        [XmlIgnore]
        public string MIPSEligibilityStatus { get; set; }
        [XmlIgnore]
        public string JoiningDate { get; set; }
        [XmlIgnore]
        public string LeavingDate { get; set; }
        [XmlIgnore]
        public string MIPSProviderId { get; set; }
        [XmlIgnore]
        public string IsEligibile { get; set; }
        [XmlIgnore]
        public string ReportingType { get; set; }
        [XmlIgnore]
        public string InEligibileReason { get; set; }
        public string ReporitngMIPS { get; set; }
        public List<InEligibleReasonList> InEligibileReasonIds { get; set; }
        [XmlIgnore]
        public string JoiningReason { get; set; }
        [XmlIgnore]
        public string JoiningComments { get; set; }
        [XmlIgnore]
        public string ObjectId { get; set; }
        [XmlIgnore]
        public string CategoryId { get; set; }
        [XmlIgnore]
        public string IsFullYear { get; set; }
        [XmlIgnore]
        public string StartDate { get; set; }
        [XmlIgnore]
        public string EndDate { get; set; }
        [XmlIgnore]
        public string ObjectType { get; set; }
        [XmlIgnore]
        public string CategoryName { get; set; }
        [XmlIgnore]
        public string NotReportingReason { get; set; }
        [XmlIgnore]
        public string OtherReason { get; set; }
        [XmlIgnore]
        public string ParticipatingId { get; set; }
        [XmlIgnore]
        public string ParticipatingReason { get; set; }
        [XmlIgnore]
        public string iTrackReportingType { get; set; }

        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            IsEligibile = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsEligibile", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            CreatedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            ModifiedOn = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ModifiedOn", incommingColumnList));
            IsActive = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IsActive", incommingColumnList));
            LastName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LastName", incommingColumnList));
            FirstName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "FirstName", incommingColumnList));
            Specialty = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Specialty", incommingColumnList));
            NPI = ModelUtility.ToStr(ModelUtility.MapValue(reader, "NPI", incommingColumnList));
            TIN = ModelUtility.ToStr(ModelUtility.MapValue(reader, "CreatedOn", incommingColumnList));
            PracticeId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PracticeId", incommingColumnList));
            PracticeName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "PracticeName", incommingColumnList));
            Facility = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Facility", incommingColumnList));
            MIPSEligibilityStatus = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MIPSEligibilityStatus", incommingColumnList));
            JoiningDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "JoiningDate", incommingColumnList));
            LeavingDate = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LeavingDate", incommingColumnList));
            ReportingMethod = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ReportingMethod", incommingColumnList));
            MIPSProviderId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "MIPSProviderId", incommingColumnList));
            RecordCount = ModelUtility.ToStr(ModelUtility.MapValue(reader, "RecordCount", incommingColumnList));
            GroupId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "GroupId", incommingColumnList));
            GroupName = ModelUtility.ToStr(ModelUtility.MapValue(reader, "GroupName", incommingColumnList));
            LookupId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "LookupId", incommingColumnList));
            Name = ModelUtility.ToStr(ModelUtility.MapValue(reader, "Name", incommingColumnList));
        }
        [XmlIgnore]
        public string LookupId { get; set; }
        [XmlIgnore]
        public string Name { get; set; }
    }
    public class MIPSGroupDetail
    {
        public string ReportingCat { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string GroupId { get; set; }
        public string IsFullYear { get; set; }
        public string ProviderName { get; set; }
        public string ProviderId { get; set; }


    }
    public class InEligibleReasonList {
        public string Reason { get; set; }
        public string IsActive { get; set; }
    }
    public class MIPSGrouupPreferenceList
    {
        public List<IndvidualProvider> Groups { get; set; }
        public List<IndvidualProvider> GroupsDetail { get; set; }
        public List<MIPSGroupDetail> GroupDetail { get; set; }

    }



}