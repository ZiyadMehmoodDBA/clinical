using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSAppointment : DataSet
    {
        private PatientAppointmentsDataTable _tPatientAppointments;
        private WaitListDataTable _tWaitList;
        private AppointmentSummaryDataTable _tAppointmentSummary;
        private AppVisitStatusLookupDataTable _tAppVisitStatusLookup;
        private AppointmentCheckoutDataTable _tAppointmentCheckout;
        private AppointmentsVisitsDataTable _tAppointmentsVisits;
        private DaySlotsDataTable _tDaySlots;
        private MonthlyAppointmentDataTable _tMonthlyAppointment;
        private PatientBalanceDataTable _tPatientBalance;
        private PortalAppRequestDataTable _tPortalAppRequest;
        private PreferredTimeDataTable _tPreferredTime;
        private SchAppointmentDataTable _tSchAppointment;
        private SchedulingFromTimeLookUpDataTable _tSchedulingFromTimeLookUp;
        private TimeSlotsDetailDataTable _tTimeSlotsDetail;
        private WaitListStatusDataTable _tWaitListStatus;
        private WeeklyAppointmentDataTable _tWeeklyAppointment;

        public DSAppointment()
        {
            DataSetName = "DSAppointment";
            _tPatientAppointments = new PatientAppointmentsDataTable();
            _tWaitList = new WaitListDataTable();
            _tAppointmentSummary = new AppointmentSummaryDataTable();
            _tAppVisitStatusLookup = new AppVisitStatusLookupDataTable();
            _tAppointmentCheckout = new AppointmentCheckoutDataTable();
            _tAppointmentsVisits = new AppointmentsVisitsDataTable();
            _tDaySlots = new DaySlotsDataTable();
            _tMonthlyAppointment = new MonthlyAppointmentDataTable();
            _tPatientBalance = new PatientBalanceDataTable();
            _tPortalAppRequest = new PortalAppRequestDataTable();
            _tPreferredTime = new PreferredTimeDataTable();
            _tSchAppointment = new SchAppointmentDataTable();
            _tSchedulingFromTimeLookUp = new SchedulingFromTimeLookUpDataTable();
            _tTimeSlotsDetail = new TimeSlotsDetailDataTable();
            _tWaitListStatus = new WaitListStatusDataTable();
            _tWeeklyAppointment = new WeeklyAppointmentDataTable();
            Tables.Add(_tPatientAppointments);
            Tables.Add(_tWaitList);
            Tables.Add(_tAppointmentSummary);
            Tables.Add(_tAppVisitStatusLookup);
            Tables.Add(_tAppointmentCheckout);
            Tables.Add(_tAppointmentsVisits);
            Tables.Add(_tDaySlots);
            Tables.Add(_tMonthlyAppointment);
            Tables.Add(_tPatientBalance);
            Tables.Add(_tPortalAppRequest);
            Tables.Add(_tPreferredTime);
            Tables.Add(_tSchAppointment);
            Tables.Add(_tSchedulingFromTimeLookUp);
            Tables.Add(_tTimeSlotsDetail);
            Tables.Add(_tWaitListStatus);
            Tables.Add(_tWeeklyAppointment);
        }

        public PatientAppointmentsDataTable PatientAppointments { get { return _tPatientAppointments; } }
        public WaitListDataTable WaitList { get { return _tWaitList; } }
        public AppointmentSummaryDataTable AppointmentSummary { get { return _tAppointmentSummary; } }
        public AppVisitStatusLookupDataTable AppVisitStatusLookup { get { return _tAppVisitStatusLookup; } }
        public AppointmentCheckoutDataTable AppointmentCheckout { get { return _tAppointmentCheckout; } }
        public AppointmentsVisitsDataTable AppointmentsVisits { get { return _tAppointmentsVisits; } }
        public DaySlotsDataTable DaySlots { get { return _tDaySlots; } }
        public MonthlyAppointmentDataTable MonthlyAppointment { get { return _tMonthlyAppointment; } }
        public PatientBalanceDataTable PatientBalance { get { return _tPatientBalance; } }
        public PortalAppRequestDataTable PortalAppRequest { get { return _tPortalAppRequest; } }
        public PreferredTimeDataTable PreferredTime { get { return _tPreferredTime; } }
        public SchAppointmentDataTable SchAppointment { get { return _tSchAppointment; } }
        public SchedulingFromTimeLookUpDataTable SchedulingFromTimeLookUp { get { return _tSchedulingFromTimeLookUp; } }
        public TimeSlotsDetailDataTable TimeSlotsDetail { get { return _tTimeSlotsDetail; } }
        public WaitListStatusDataTable WaitListStatus { get { return _tWaitListStatus; } }
        public WeeklyAppointmentDataTable WeeklyAppointment { get { return _tWeeklyAppointment; } }

        public class PatientAppointmentsDataTable : DataTable
        {
            public PatientAppointmentsDataTable() : base("PatientAppointments")
            {
                var id = new DataColumn("AppointmentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentDate","AppointmentStatus","BillingComments","CancellationReason","CheckInReason",
                    "Comments","Copayment","CreatedBy","CreatedOn","Duration","ECWAppointmentID","FacilityId",
                    "FromFollowUp","IsActive","IsAllVisitUsed","IsCopayAlert","IsNonBilable","IsReminderSent",
                    "IsReschedule","IsSpecialist","ModifiedBy","ModifiedOn","NotesId","PatientAllowed",
                    "PatientBalance","PatientId","PatientInsuranceId","PatientReferralId","PatientTypeID",
                    "ProviderId","ReasonComments","ReasonCommentsTypeName","RefProviderId","ReferralId",
                    "ReferralNo","ResourceId","SchReasonId","SchStatusId","SnomedCode","SnomedCodeDescription",
                    "TimeFrom","TimeTo","VisitTypeID","WaitListId",
                    "ICDCode10","ICDCode10Description","VisitType","Reason",
                    "FaclityPhoneNo","PatientName",
                    "FacilityName","ProviderName",
                    "TimeSlotId","TimeSlotDtlId","PatternEvery","Value",
                    "PatternDays","PatternWeeks","PatternMonths","EndByDate","EndByAppointment",
                    "ResourceName","RefProviderName","Status","isNoteCreated","ProblemListId","AccountNumber" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientAppointmentsRow NewPatientAppointmentsRow() { return (PatientAppointmentsRow)NewRow(); }
            public void AddPatientAppointmentsRow(PatientAppointmentsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientAppointmentsRow(b); }
            protected override Type GetRowType() { return typeof(PatientAppointmentsRow); }
            public System.Collections.Generic.IEnumerator<PatientAppointmentsRow> GetEnumerator() { foreach (DataRow r in Rows) yield return (PatientAppointmentsRow)r; }

            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn AppointmentStatusColumn { get { return Columns["AppointmentStatus"]; } }
            public DataColumn BillingCommentsColumn { get { return Columns["BillingComments"]; } }
            public DataColumn CancellationReasonColumn { get { return Columns["CancellationReason"]; } }
            public DataColumn CheckInReasonColumn { get { return Columns["CheckInReason"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn CopaymentColumn { get { return Columns["Copayment"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn DurationColumn { get { return Columns["Duration"]; } }
            public DataColumn ECWAppointmentIDColumn { get { return Columns["ECWAppointmentID"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn FromFollowUpColumn { get { return Columns["FromFollowUp"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn IsAllVisitUsedColumn { get { return Columns["IsAllVisitUsed"]; } }
            public DataColumn IsCopayAlertColumn { get { return Columns["IsCopayAlert"]; } }
            public DataColumn IsNonBilableColumn { get { return Columns["IsNonBilable"]; } }
            public DataColumn IsReminderSentColumn { get { return Columns["IsReminderSent"]; } }
            public DataColumn IsRescheduleColumn { get { return Columns["IsReschedule"]; } }
            public DataColumn IsSpecialistColumn { get { return Columns["IsSpecialist"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn NotesIdColumn { get { return Columns["NotesId"]; } }
            public DataColumn PatientAllowedColumn { get { return Columns["PatientAllowed"]; } }
            public DataColumn PatientBalanceColumn { get { return Columns["PatientBalance"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn PatientInsuranceIdColumn { get { return Columns["PatientInsuranceId"]; } }
            public DataColumn PatientReferralIdColumn { get { return Columns["PatientReferralId"]; } }
            public DataColumn PatientTypeIDColumn { get { return Columns["PatientTypeID"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn ReasonCommentsColumn { get { return Columns["ReasonComments"]; } }
            public DataColumn ReasonCommentsTypeNameColumn { get { return Columns["ReasonCommentsTypeName"]; } }
            public DataColumn RefProviderIdColumn { get { return Columns["RefProviderId"]; } }
            public DataColumn ReferralIdColumn { get { return Columns["ReferralId"]; } }
            public DataColumn ReferralNoColumn { get { return Columns["ReferralNo"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn SchReasonIdColumn { get { return Columns["SchReasonId"]; } }
            public DataColumn SchStatusIdColumn { get { return Columns["SchStatusId"]; } }
            public DataColumn SnomedCodeColumn { get { return Columns["SnomedCode"]; } }
            public DataColumn SnomedCodeDescriptionColumn { get { return Columns["SnomedCodeDescription"]; } }
            public DataColumn TimeFromColumn { get { return Columns["TimeFrom"]; } }
            public DataColumn TimeToColumn { get { return Columns["TimeTo"]; } }
            public DataColumn VisitTypeIDColumn { get { return Columns["VisitTypeID"]; } }
            public DataColumn WaitListIdColumn { get { return Columns["WaitListId"]; } }
            public DataColumn ICDCode10Column { get { return Columns["ICDCode10"]; } }
            public DataColumn ICDCode10DescriptionColumn { get { return Columns["ICDCode10Description"]; } }
            public DataColumn VisitTypeColumn     { get { return Columns["VisitType"]; } }
            public DataColumn ReasonColumn        { get { return Columns["Reason"]; } }
            public DataColumn FaclityPhoneNoColumn{ get { return Columns["FaclityPhoneNo"]; } }
            public DataColumn PatientNameColumn   { get { return Columns["PatientName"]; } }
            public DataColumn FacilityNameColumn  { get { return Columns["FacilityName"]; } }
            public DataColumn ProviderNameColumn  { get { return Columns["ProviderName"]; } }
            public DataColumn TimeSlotIdColumn    { get { return Columns["TimeSlotId"]; } }
            public DataColumn TimeSlotDtlIdColumn { get { return Columns["TimeSlotDtlId"]; } }
            public DataColumn PatternEveryColumn  { get { return Columns["PatternEvery"]; } }
            public DataColumn ValueColumn         { get { return Columns["Value"]; } }
            public DataColumn PatternDaysColumn   { get { return Columns["PatternDays"]; } }
            public DataColumn PatternWeeksColumn  { get { return Columns["PatternWeeks"]; } }
            public DataColumn PatternMonthsColumn { get { return Columns["PatternMonths"]; } }
            public DataColumn EndByDateColumn     { get { return Columns["EndByDate"]; } }
            public DataColumn EndByAppointmentColumn { get { return Columns["EndByAppointment"]; } }
            public DataColumn ResourceNameColumn    { get { return Columns["ResourceName"]; } }
            public DataColumn RefProviderNameColumn { get { return Columns["RefProviderName"]; } }
            public DataColumn StatusColumn          { get { return Columns["Status"]; } }
            public DataColumn isNoteCreatedColumn   { get { return Columns["isNoteCreated"]; } }
            public DataColumn ProblemListIdColumn   { get { return Columns["ProblemListId"]; } }
            public DataColumn AccountNumberColumn   { get { return Columns["AccountNumber"]; } }

            public int Count { get { return Rows.Count; } }
            public PatientAppointmentsRow this[int index] { get { return (PatientAppointmentsRow)Rows[index]; } }
        }

        public class PatientAppointmentsRow : DataRow
        {
            internal PatientAppointmentsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   AppointmentId   { get { var v = this["AppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentDate { get { return G("AppointmentDate"); } set { S("AppointmentDate", value); } }
            public string AppointmentStatus { get { return G("AppointmentStatus"); } set { S("AppointmentStatus", value); } }
            public long   PatientId       { get { var v = this["PatientId"];       return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"]       = value.ToString(); } }
            public long   ProviderId      { get { var v = this["ProviderId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ProviderId"]      = value.ToString(); } }
            public long   FacilityId      { get { var v = this["FacilityId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["FacilityId"]      = value.ToString(); } }
            public string SchReasonId     { get { return G("SchReasonId"); }       set { S("SchReasonId", value); } }
            public long   SchStatusId     { get { var v = this["SchStatusId"];     return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["SchStatusId"]     = value.ToString(); } }
            public string TimeFrom        { get { return G("TimeFrom"); }          set { S("TimeFrom", value); } }
            public string TimeTo          { get { return G("TimeTo"); }            set { S("TimeTo", value); } }
            public float  Copayment       { get { var v = this["Copayment"];       return v == DBNull.Value ? 0f : Convert.ToSingle(v); } set { this["Copayment"]       = value.ToString(); } }
            public bool   IsActive        { get { var v = this["IsActive"];        return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["IsActive"]        = value.ToString(); } }
            public bool   IsReminderSent  { get { var v = this["IsReminderSent"];  return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["IsReminderSent"]  = value.ToString(); } }
            public bool   IsCopayAlert    { get { var v = this["IsCopayAlert"];    return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["IsCopayAlert"]    = value.ToString(); } }
            public bool   IsSpecialist    { get { var v = this["IsSpecialist"];    return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["IsSpecialist"]    = value.ToString(); } }
            public bool   IsNonBilable    { get { var v = this["IsNonBilable"];    return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["IsNonBilable"]    = value.ToString(); } }
            public bool   FromFollowUp    { get { var v = this["FromFollowUp"];    return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["FromFollowUp"]    = value.ToString(); } }
            public bool   PatternEvery    { get { var v = this["PatternEvery"];    return v != DBNull.Value && Convert.ToBoolean(v); }   set { this["PatternEvery"]    = value.ToString(); } }
            public string Comments        { get { return G("Comments"); }          set { S("Comments", value); } }
            public string CreatedBy       { get { return G("CreatedBy"); }         set { S("CreatedBy", value); } }
            public DateTime CreatedOn     { get { var v = this["CreatedOn"];       return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy      { get { return G("ModifiedBy"); }        set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn    { get { var v = this["ModifiedOn"];      return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public long   NotesId         { get { var v = this["NotesId"];         return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["NotesId"]         = value.ToString(); } }
            public long   ResourceId      { get { var v = this["ResourceId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ResourceId"]      = value.ToString(); } }
            public long   RefProviderId   { get { var v = this["RefProviderId"];   return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RefProviderId"]   = value.ToString(); } }
            public long   PatientInsuranceId { get { var v = this["PatientInsuranceId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientInsuranceId"] = value.ToString(); } }
            public long   PatientReferralId  { get { var v = this["PatientReferralId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientReferralId"]  = value.ToString(); } }
            public long   ReferralId      { get { var v = this["ReferralId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ReferralId"]      = value.ToString(); } }
            public long   WaitListId      { get { var v = this["WaitListId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["WaitListId"]      = value.ToString(); } }
            public long   PatientTypeID   { get { var v = this["PatientTypeID"];   return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientTypeID"]   = value.ToString(); } }
            public long   VisitTypeID     { get { var v = this["VisitTypeID"];     return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["VisitTypeID"]     = value.ToString(); } }
            public double PatientBalance  { get { var v = this["PatientBalance"];  return v == DBNull.Value ? 0.0 : Convert.ToDouble(v); } set { this["PatientBalance"] = value.ToString(); } }
            public int    TimeSlotId      { get { var v = this["TimeSlotId"];      return v == DBNull.Value ? 0 : Convert.ToInt32(v); }  set { this["TimeSlotId"]      = value.ToString(); } }
            public int    TimeSlotDtlId   { get { var v = this["TimeSlotDtlId"];   return v == DBNull.Value ? 0 : Convert.ToInt32(v); }  set { this["TimeSlotDtlId"]   = value.ToString(); } }
            public int    Duration        { get { var v = this["Duration"];         return v == DBNull.Value ? 0 : Convert.ToInt32(v); }  set { this["Duration"]        = value.ToString(); } }
            public string Value           { get { return G("Value"); }             set { S("Value", value); } }
            public string PatternDays     { get { return G("PatternDays"); }       set { S("PatternDays", value); } }
            public string PatternWeeks    { get { return G("PatternWeeks"); }      set { S("PatternWeeks", value); } }
            public string PatternMonths   { get { return G("PatternMonths"); }     set { S("PatternMonths", value); } }
            public DateTime EndByDate     { get { var v = this["EndByDate"];       return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["EndByDate"] = value.ToString(); } }
            public string EndByAppointment{ get { return G("EndByAppointment"); }  set { S("EndByAppointment", value); } }
            public string CancellationReason { get { return G("CancellationReason"); } set { S("CancellationReason", value); } }
            public string ReasonComments  { get { return G("ReasonComments"); }    set { S("ReasonComments", value); } }
            public string ReasonCommentsTypeName { get { return G("ReasonCommentsTypeName"); } set { S("ReasonCommentsTypeName", value); } }
            public string ICDCode10       { get { return G("ICDCode10"); }         set { S("ICDCode10", value); } }
            public string ICDCode10Description { get { return G("ICDCode10Description"); } set { S("ICDCode10Description", value); } }
            public string SnomedCode      { get { return G("SnomedCode"); }        set { S("SnomedCode", value); } }
            public string SnomedCodeDescription { get { return G("SnomedCodeDescription"); } set { S("SnomedCodeDescription", value); } }
            public string ReferralNo      { get { return G("ReferralNo"); }        set { S("ReferralNo", value); } }
            public string FaclityPhoneNo  { get { return G("FaclityPhoneNo"); }    set { S("FaclityPhoneNo", value); } }
            public string PatientName     { get { return G("PatientName"); }       set { S("PatientName", value); } }
            public string FacilityName    { get { return G("FacilityName"); }      set { S("FacilityName", value); } }
            public string ProviderName    { get { return G("ProviderName"); }      set { S("ProviderName", value); } }
            public string ResourceName    { get { return G("ResourceName"); }      set { S("ResourceName", value); } }
            public string RefProviderName { get { return G("RefProviderName"); }   set { S("RefProviderName", value); } }
            public string Status          { get { return G("Status"); }            set { S("Status", value); } }
            public string isNoteCreated   { get { return G("isNoteCreated"); }     set { S("isNoteCreated", value); } }
            public long   ProblemListId   { get { var v = this["ProblemListId"];   return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ProblemListId"] = value.ToString(); } }
            public string AccountNumber   { get { return G("AccountNumber"); }     set { S("AccountNumber", value); } }
        }

        public class WaitListDataTable : DataTable
        {
            public WaitListDataTable() : base("WaitList")
            {
                var id = new DataColumn("WaitListId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AccountNumber","Comments","CreatedBy","CreatedOn","FacilityId","FromDate","IsActive","IsPreferredDay",
                    "IsSelected","ModifiedBy","ModifiedOn","PAN","PatientId","PreferredDate","PreferredDay",
                    "PrfTimeId","ProviderId","ReasonName","RecordCount","RefProviderId","ResourceId",
                    "ScheduleReasonId","ToDate","WtStatusId",
                    "FacilityName","PatientName","ProviderName","RefProvName","ResourceName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public WaitListRow NewWaitListRow() { return (WaitListRow)NewRow(); }
            public void AddWaitListRow(WaitListRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new WaitListRow(b); }
            protected override Type GetRowType() { return typeof(WaitListRow); }

            public DataColumn WaitListIdColumn { get { return Columns["WaitListId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn FromDateColumn { get { return Columns["FromDate"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn IsPreferredDayColumn { get { return Columns["IsPreferredDay"]; } }
            public DataColumn IsSelectedColumn { get { return Columns["IsSelected"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn PANColumn { get { return Columns["PAN"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn PreferredDateColumn { get { return Columns["PreferredDate"]; } }
            public DataColumn PreferredDayColumn { get { return Columns["PreferredDay"]; } }
            public DataColumn PrfTimeIdColumn { get { return Columns["PrfTimeId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn ReasonNameColumn { get { return Columns["ReasonName"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn RefProviderIdColumn { get { return Columns["RefProviderId"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn ScheduleReasonIdColumn { get { return Columns["ScheduleReasonId"]; } }
            public DataColumn ToDateColumn { get { return Columns["ToDate"]; } }
            public DataColumn WtStatusIdColumn { get { return Columns["WtStatusId"]; } }
            public DataColumn FacilityNameColumn { get { return Columns["FacilityName"]; } }
            public DataColumn PatientNameColumn { get { return Columns["PatientName"]; } }
            public DataColumn ProviderNameColumn { get { return Columns["ProviderName"]; } }
            public DataColumn RefProvNameColumn { get { return Columns["RefProvName"]; } }
            public DataColumn ResourceNameColumn { get { return Columns["ResourceName"]; } }
            public DataColumn AccountNumberColumn { get { return Columns["AccountNumber"]; } }
        }

        public class WaitListRow : DataRow
        {
            internal WaitListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long WaitListId { get { var v = this["WaitListId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
            public int IsActive { get { var v = this["IsActive"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["IsActive"] = value.ToString(); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
            public long ResourceId { get { var v = this["ResourceId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ResourceId"] = value.ToString(); } }
            public long RefProviderId { get { var v = this["RefProviderId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RefProviderId"] = value.ToString(); } }
            public string ReasonName { get { return G("ReasonName"); } set { S("ReasonName", value); } }
            public int WtStatusId { get { var v = this["WtStatusId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["WtStatusId"] = value.ToString(); } }
            public DateTime FromDate { get { var v = this["FromDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["FromDate"] = value.ToString(); } }
            public DateTime ToDate { get { var v = this["ToDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ToDate"] = value.ToString(); } }
            public DateTime PreferredDate { get { var v = this["PreferredDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["PreferredDate"] = value.ToString(); } }
            public string PAN { get { return G("PAN"); } set { S("PAN", value); } }
            public int PrfTimeId { get { var v = this["PrfTimeId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["PrfTimeId"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string IsPreferredDay { get { return G("IsPreferredDay"); } set { S("IsPreferredDay", value); } }
            public string PreferredDay { get { return G("PreferredDay"); } set { S("PreferredDay", value); } }
            public string FacilityName { get { return G("FacilityName"); } set { S("FacilityName", value); } }
            public string PatientName { get { return G("PatientName"); } set { S("PatientName", value); } }
            public string ProviderName { get { return G("ProviderName"); } set { S("ProviderName", value); } }
            public string RefProvName { get { return G("RefProvName"); } set { S("RefProvName", value); } }
            public string ResourceName { get { return G("ResourceName"); } set { S("ResourceName", value); } }
        }

        public class AppointmentSummaryDataTable : DataTable
        {
            public AppointmentSummaryDataTable() : base("AppointmentSummary")
            {
                Columns.Add(new DataColumn("AppointmentSummaryId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "AppointmentId", "Summary", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AppointmentSummaryRow NewAppointmentSummaryRow() { return (AppointmentSummaryRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AppointmentSummaryRow(b); }
            protected override Type GetRowType() { return typeof(AppointmentSummaryRow); }

            public DataColumn AppointmentSummaryIdColumn { get { return Columns["AppointmentSummaryId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn SummaryColumn { get { return Columns["Summary"]; } }
        }

        public class AppointmentSummaryRow : DataRow
        {
            internal AppointmentSummaryRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AppointmentSummaryId { get { var v = this["AppointmentSummaryId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string Summary { get { return G("Summary"); } set { S("Summary", value); } }
        }

        public class AppVisitStatusLookupDataTable : DataTable
        {
            public AppVisitStatusLookupDataTable() : base("AppVisitStatusLookup")
            {
                Columns.Add(new DataColumn("AppVisitStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "StatusName", "ShortName", "IsActive", "StatusId" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AppVisitStatusLookupRow NewAppVisitStatusLookupRow() { return (AppVisitStatusLookupRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AppVisitStatusLookupRow(b); }
            protected override Type GetRowType() { return typeof(AppVisitStatusLookupRow); }

            public DataColumn AppVisitStatusIdColumn { get { return Columns["AppVisitStatusId"]; } }
            public DataColumn StatusNameColumn { get { return Columns["StatusName"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn StatusIdColumn { get { return Columns["StatusId"]; } }
        }

        public class AppVisitStatusLookupRow : DataRow
        {
            internal AppVisitStatusLookupRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AppVisitStatusId { get { var v = this["AppVisitStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string StatusName { get { return G("StatusName"); } set { S("StatusName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── AppointmentCheckout ──────────────────────────────────────────────────
        public class AppointmentCheckoutDataTable : DataTable
        {
            public AppointmentCheckoutDataTable() : base("AppointmentCheckout")
            {
                var id = new DataColumn("AppointmentCheckoutId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentId","PatientId","ProviderId","FacilityId","CheckoutDate",
                    "CheckoutTime","Comments","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AppointmentCheckoutRow NewAppointmentCheckoutRow() { return (AppointmentCheckoutRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AppointmentCheckoutRow(b); }
            protected override Type GetRowType() { return typeof(AppointmentCheckoutRow); }

            public DataColumn AppointmentCheckoutIdColumn { get { return Columns["AppointmentCheckoutId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn CheckoutDateColumn { get { return Columns["CheckoutDate"]; } }
            public DataColumn CheckoutTimeColumn { get { return Columns["CheckoutTime"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class AppointmentCheckoutRow : DataRow
        {
            internal AppointmentCheckoutRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AppointmentCheckoutId { get { var v = this["AppointmentCheckoutId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── AppointmentsVisits ──────────────────────────────────────────────────
        public class AppointmentsVisitsDataTable : DataTable
        {
            public AppointmentsVisitsDataTable() : base("AppointmentsVisits")
            {
                var id = new DataColumn("AppointmentsVisitsId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentId","PatientId","ProviderId","ProviderName","FacilityId",
                    "AppointmentDate","TimeFrom","TimeTo","AppointmentStatus","VisitTypeID",
                    "SchReasonId","NotesId","RecordCount","IsActive","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn","DraftCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AppointmentsVisitsRow NewAppointmentsVisitsRow() { return (AppointmentsVisitsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AppointmentsVisitsRow(b); }
            protected override Type GetRowType() { return typeof(AppointmentsVisitsRow); }

            public DataColumn AppointmentsVisitsIdColumn { get { return Columns["AppointmentsVisitsId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn ProviderNameColumn { get { return Columns["ProviderName"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn TimeFromColumn { get { return Columns["TimeFrom"]; } }
            public DataColumn TimeToColumn { get { return Columns["TimeTo"]; } }
            public DataColumn AppointmentStatusColumn { get { return Columns["AppointmentStatus"]; } }
            public DataColumn VisitTypeIDColumn { get { return Columns["VisitTypeID"]; } }
            public DataColumn SchReasonIdColumn { get { return Columns["SchReasonId"]; } }
            public DataColumn NotesIdColumn { get { return Columns["NotesId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn DraftCountColumn { get { return Columns["DraftCount"]; } }
        }

        public class AppointmentsVisitsRow : DataRow
        {
            internal AppointmentsVisitsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AppointmentsVisitsId { get { var v = this["AppointmentsVisitsId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string ProviderName { get { return G("ProviderName"); } set { S("ProviderName", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        // ── DaySlots ────────────────────────────────────────────────────────────
        public class DaySlotsDataTable : DataTable
        {
            public DaySlotsDataTable() : base("DaySlots")
            {
                var id = new DataColumn("DaySlotsId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ProviderId","FacilityId","ResourceId","SlotDate","TimeFrom","TimeTo",
                    "AppDtl","MaxCountApp","SlotMinutes","IsActive","Day" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public DaySlotsRow NewDaySlotsRow() { return (DaySlotsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new DaySlotsRow(b); }
            protected override Type GetRowType() { return typeof(DaySlotsRow); }

            public DataColumn DaySlotsIdColumn { get { return Columns["DaySlotsId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn SlotDateColumn { get { return Columns["SlotDate"]; } }
            public DataColumn TimeFromColumn { get { return Columns["TimeFrom"]; } }
            public DataColumn TimeToColumn { get { return Columns["TimeTo"]; } }
            public DataColumn AppDtlColumn { get { return Columns["AppDtl"]; } }
            public DataColumn MaxCountAppColumn { get { return Columns["MaxCountApp"]; } }
            public DataColumn SlotMinutesColumn { get { return Columns["SlotMinutes"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn DayColumn { get { return Columns["Day"]; } }
        }

        public class DaySlotsRow : DataRow
        {
            internal DaySlotsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long DaySlotsId { get { var v = this["DaySlotsId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
            public string AppDtl { get { return G("AppDtl"); } set { S("AppDtl", value); } }
            public string MaxCountApp { get { return G("MaxCountApp"); } set { S("MaxCountApp", value); } }
            public string Day { get { return G("Day"); } set { S("Day", value); } }
        }

        // ── MonthlyAppointment ──────────────────────────────────────────────────
        public class MonthlyAppointmentDataTable : DataTable
        {
            public MonthlyAppointmentDataTable() : base("MonthlyAppointment")
            {
                var id = new DataColumn("MonthlyAppointmentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentId","AppointmentDate","ProviderId","FacilityId","ResourceId",
                    "PatientId","TimeFrom","TimeTo","AppointmentStatus","VisitTypeID","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public MonthlyAppointmentRow NewMonthlyAppointmentRow() { return (MonthlyAppointmentRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MonthlyAppointmentRow(b); }
            protected override Type GetRowType() { return typeof(MonthlyAppointmentRow); }

            public DataColumn MonthlyAppointmentIdColumn { get { return Columns["MonthlyAppointmentId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn AppointmentStatusColumn { get { return Columns["AppointmentStatus"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class MonthlyAppointmentRow : DataRow
        {
            internal MonthlyAppointmentRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MonthlyAppointmentId { get { var v = this["MonthlyAppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string AppointmentDate { get { return G("AppointmentDate"); } set { S("AppointmentDate", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
        }

        // ── PatientBalance ──────────────────────────────────────────────────────
        public class PatientBalanceDataTable : DataTable
        {
            public PatientBalanceDataTable() : base("PatientBalance")
            {
                var id = new DataColumn("PatientBalanceId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","TotalBalance","InsuranceBalance","PatientResponsibility","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientBalanceRow NewPatientBalanceRow() { return (PatientBalanceRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientBalanceRow(b); }
            protected override Type GetRowType() { return typeof(PatientBalanceRow); }

            public DataColumn PatientBalanceIdColumn { get { return Columns["PatientBalanceId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn TotalBalanceColumn { get { return Columns["TotalBalance"]; } }
            public DataColumn InsuranceBalanceColumn { get { return Columns["InsuranceBalance"]; } }
            public DataColumn PatientResponsibilityColumn { get { return Columns["PatientResponsibility"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class PatientBalanceRow : DataRow
        {
            internal PatientBalanceRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PatientBalanceId { get { var v = this["PatientBalanceId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string TotalBalance { get { return G("TotalBalance"); } set { S("TotalBalance", value); } }
        }

        // ── PortalAppRequest ────────────────────────────────────────────────────
        public class PortalAppRequestDataTable : DataTable
        {
            public PortalAppRequestDataTable() : base("PortalAppRequest")
            {
                var id = new DataColumn("PortalAppRequestId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ProviderId","FacilityId","PatientId","AppDate","TimeSlotId","TimeSlotDtlId",
                    "RequestStatus","SchReasonId","SchReasonName","AppointmentId","CreatedOn",
                    "ModifiedOn","FromTime","ToTime","Duration","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PortalAppRequestRow NewPortalAppRequestRow() { return (PortalAppRequestRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PortalAppRequestRow(b); }
            protected override Type GetRowType() { return typeof(PortalAppRequestRow); }

            public DataColumn PortalAppRequestIdColumn { get { return Columns["PortalAppRequestId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn AppDateColumn { get { return Columns["AppDate"]; } }
            public DataColumn TimeSlotIdColumn { get { return Columns["TimeSlotId"]; } }
            public DataColumn TimeSlotDtlIdColumn { get { return Columns["TimeSlotDtlId"]; } }
            public DataColumn RequestStatusColumn { get { return Columns["RequestStatus"]; } }
            public DataColumn SchReasonIdColumn { get { return Columns["SchReasonId"]; } }
            public DataColumn SchReasonNameColumn { get { return Columns["SchReasonName"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn FromTimeColumn { get { return Columns["FromTime"]; } }
            public DataColumn ToTimeColumn { get { return Columns["ToTime"]; } }
            public DataColumn DurationColumn { get { return Columns["Duration"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class PortalAppRequestRow : DataRow
        {
            internal PortalAppRequestRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PortalAppRequestId { get { var v = this["PortalAppRequestId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string AppDate { get { return G("AppDate"); } set { S("AppDate", value); } }
            public string RequestStatus { get { return G("RequestStatus"); } set { S("RequestStatus", value); } }
            public long   AppointmentId { get { var v = this["AppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["AppointmentId"] = value.ToString(); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── PreferredTime ───────────────────────────────────────────────────────
        public class PreferredTimeDataTable : DataTable
        {
            public PreferredTimeDataTable() : base("PreferredTime")
            {
                var id = new DataColumn("PrfTimeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "ShortName","LongName","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PreferredTimeRow NewPreferredTimeRow() { return (PreferredTimeRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PreferredTimeRow(b); }
            protected override Type GetRowType() { return typeof(PreferredTimeRow); }

            public DataColumn PrfTimeIdColumn { get { return Columns["PrfTimeId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn LongNameColumn { get { return Columns["LongName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class PreferredTimeRow : DataRow
        {
            internal PreferredTimeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PrfTimeId { get { var v = this["PrfTimeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName { get { return G("ShortName"); } set { S("ShortName", value); } }
            public string LongName { get { return G("LongName"); } set { S("LongName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── SchAppointment ──────────────────────────────────────────────────────
        public class SchAppointmentDataTable : DataTable
        {
            public SchAppointmentDataTable() : base("SchAppointment")
            {
                var id = new DataColumn("SchAppointmentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentId","PatientId","ProviderId","FacilityId","ResourceId",
                    "AppointmentDate","TimeFrom","TimeTo","AppointmentStatus","VisitTypeID",
                    "SchReasonId","NotesId","RecordCount","IsActive","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public SchAppointmentRow NewSchAppointmentRow() { return (SchAppointmentRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new SchAppointmentRow(b); }
            protected override Type GetRowType() { return typeof(SchAppointmentRow); }

            public DataColumn SchAppointmentIdColumn { get { return Columns["SchAppointmentId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn TimeFromColumn { get { return Columns["TimeFrom"]; } }
            public DataColumn TimeToColumn { get { return Columns["TimeTo"]; } }
            public DataColumn AppointmentStatusColumn { get { return Columns["AppointmentStatus"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class SchAppointmentRow : DataRow
        {
            internal SchAppointmentRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long SchAppointmentId { get { var v = this["SchAppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        // ── SchedulingFromTimeLookUp ────────────────────────────────────────────
        public class SchedulingFromTimeLookUpDataTable : DataTable
        {
            public SchedulingFromTimeLookUpDataTable() : base("SchedulingFromTimeLookUp")
            {
                var id = new DataColumn("SchedulingFromTimeLookUpId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "TimeValue","TimeDisplay","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public SchedulingFromTimeLookUpRow NewSchedulingFromTimeLookUpRow() { return (SchedulingFromTimeLookUpRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new SchedulingFromTimeLookUpRow(b); }
            protected override Type GetRowType() { return typeof(SchedulingFromTimeLookUpRow); }

            public DataColumn SchedulingFromTimeLookUpIdColumn { get { return Columns["SchedulingFromTimeLookUpId"]; } }
            public DataColumn TimeValueColumn { get { return Columns["TimeValue"]; } }
            public DataColumn TimeDisplayColumn { get { return Columns["TimeDisplay"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class SchedulingFromTimeLookUpRow : DataRow
        {
            internal SchedulingFromTimeLookUpRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long SchedulingFromTimeLookUpId { get { var v = this["SchedulingFromTimeLookUpId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TimeValue { get { return G("TimeValue"); } set { S("TimeValue", value); } }
            public string TimeDisplay { get { return G("TimeDisplay"); } set { S("TimeDisplay", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── TimeSlotsDetail ─────────────────────────────────────────────────────
        public class TimeSlotsDetailDataTable : DataTable
        {
            public TimeSlotsDetailDataTable() : base("TimeSlotsDetail")
            {
                var id = new DataColumn("TimeSlotsDetailId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ProviderId","ProviderName","FacilityId","FacilityName","ResourceId","ResourceName",
                    "ScheduleDate","FromTimeSlots","ToTimeSlots","SlotMinutes","PatientAllowed",
                    "PatientBooked","OverBookAllowed","ScheduleReasonId","Reasons","Comments",
                    "RecordCount","ErrorMessage","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public TimeSlotsDetailRow NewTimeSlotsDetailRow() { return (TimeSlotsDetailRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new TimeSlotsDetailRow(b); }
            protected override Type GetRowType() { return typeof(TimeSlotsDetailRow); }

            public DataColumn TimeSlotsDetailIdColumn { get { return Columns["TimeSlotsDetailId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn ProviderNameColumn { get { return Columns["ProviderName"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn FacilityNameColumn { get { return Columns["FacilityName"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn ResourceNameColumn { get { return Columns["ResourceName"]; } }
            public DataColumn ScheduleDateColumn { get { return Columns["ScheduleDate"]; } }
            public DataColumn FromTimeSlotsColumn { get { return Columns["FromTimeSlots"]; } }
            public DataColumn ToTimeSlotsColumn { get { return Columns["ToTimeSlots"]; } }
            public DataColumn SlotMinutesColumn { get { return Columns["SlotMinutes"]; } }
            public DataColumn PatientAllowedColumn { get { return Columns["PatientAllowed"]; } }
            public DataColumn PatientBookedColumn { get { return Columns["PatientBooked"]; } }
            public DataColumn OverBookAllowedColumn { get { return Columns["OverBookAllowed"]; } }
            public DataColumn ScheduleReasonIdColumn { get { return Columns["ScheduleReasonId"]; } }
            public DataColumn ReasonsColumn { get { return Columns["Reasons"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn ErrorMessageColumn { get { return Columns["ErrorMessage"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class TimeSlotsDetailRow : DataRow
        {
            internal TimeSlotsDetailRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long TimeSlotsDetailId { get { var v = this["TimeSlotsDetailId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string ProviderName { get { return G("ProviderName"); } set { S("ProviderName", value); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
            public string ScheduleDate { get { return G("ScheduleDate"); } set { S("ScheduleDate", value); } }
            public string FromTimeSlots { get { return G("FromTimeSlots"); } set { S("FromTimeSlots", value); } }
            public string ToTimeSlots { get { return G("ToTimeSlots"); } set { S("ToTimeSlots", value); } }
            public string SlotMinutes { get { return G("SlotMinutes"); } set { S("SlotMinutes", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
            public string ErrorMessage { get { return G("ErrorMessage"); } set { S("ErrorMessage", value); } }
        }

        // ── WaitListStatus ──────────────────────────────────────────────────────
        public class WaitListStatusDataTable : DataTable
        {
            public WaitListStatusDataTable() : base("WaitListStatus")
            {
                var id = new DataColumn("wtListStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "ShortName","LongName","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public WaitListStatusRow NewWaitListStatusRow() { return (WaitListStatusRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new WaitListStatusRow(b); }
            protected override Type GetRowType() { return typeof(WaitListStatusRow); }

            public DataColumn wtListStatusIdColumn { get { return Columns["wtListStatusId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn LongNameColumn { get { return Columns["LongName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class WaitListStatusRow : DataRow
        {
            internal WaitListStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long wtListStatusId { get { var v = this["wtListStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName { get { return G("ShortName"); } set { S("ShortName", value); } }
            public string LongName { get { return G("LongName"); } set { S("LongName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── WeeklyAppointment ───────────────────────────────────────────────────
        public class WeeklyAppointmentDataTable : DataTable
        {
            public WeeklyAppointmentDataTable() : base("WeeklyAppointment")
            {
                var id = new DataColumn("WeeklyAppointmentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AppointmentId","AppointmentDate","ProviderId","FacilityId","ResourceId",
                    "PatientId","TimeFrom","TimeTo","AppointmentStatus","VisitTypeID",
                    "DayOfWeek","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public WeeklyAppointmentRow NewWeeklyAppointmentRow() { return (WeeklyAppointmentRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new WeeklyAppointmentRow(b); }
            protected override Type GetRowType() { return typeof(WeeklyAppointmentRow); }

            public DataColumn WeeklyAppointmentIdColumn { get { return Columns["WeeklyAppointmentId"]; } }
            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn ResourceIdColumn { get { return Columns["ResourceId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn DayOfWeekColumn { get { return Columns["DayOfWeek"]; } }
            public DataColumn AppointmentStatusColumn { get { return Columns["AppointmentStatus"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class WeeklyAppointmentRow : DataRow
        {
            internal WeeklyAppointmentRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long WeeklyAppointmentId { get { var v = this["WeeklyAppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AppointmentId { get { return G("AppointmentId"); } set { S("AppointmentId", value); } }
            public string AppointmentDate { get { return G("AppointmentDate"); } set { S("AppointmentDate", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string DayOfWeek { get { return G("DayOfWeek"); } set { S("DayOfWeek", value); } }
        }
    }
}
