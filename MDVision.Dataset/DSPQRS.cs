using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSPQRS : DataSet
    {
        public DSPQRS()
        {
            DataSetName = "DSPQRS";
            // Primary tables with typed access
            Tables.Add(new PQRSMeasureDataTable());
            Tables.Add(new PQRSReportsDataTable());
            Tables.Add(new PQRSMeasureDataDataTable());
            Tables.Add(new PQRS_MeasureReasonsDataTable());
            Tables.Add(new PQRS_PatientFromVisitsDataTable());
            Tables.Add(new PatientPQRSDataTable());
                Tables.Add(new IndividualReportingDataTable());
            Tables.Add(new IndividualReportingListDataTable());
            Tables.Add(new MeasureGroupLookupDataTable());
            Tables.Add(new MeasureGroupsDataTable());
            Tables.Add(new MeasureGroupsListDataTable());
            Tables.Add(new MeasuresDataTable());
            Tables.Add(new PatientListDataTable());
            Tables.Add(new PQRS_PatientsListDataTable());
            // Stub tables used as TableName for ExecuteDataSet
            foreach (var name in new[] {
                "PQRS","PQRS_MeasureSection","PQRS_PQRS_Details",
                "PQRSCodes","Providers_PQRS","ProviderToProvider",
                "CatagoryIII_PopulationValueSet","DiagnosisActiveConcernAct","EncounterPerformed",
                "FamilyHx","MedicationActive","MedicationAdministered","MedicationAllergy",
                "MedicationOrder","PhysicalExam","ProcedureOrder","ProcedurePerformed",
                "RiskCatagoryAssesment","TobbacoUser" })
            {
                var t = new DataTable(name);
                t.Columns.Add(new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                t.Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
                Tables.Add(t);
            }
        }

        public PQRSMeasureDataTable PQRSMeasure { get { return (PQRSMeasureDataTable)Tables["PQRSMeasure"]; } }
        public PQRSReportsDataTable PQRSReports { get { return (PQRSReportsDataTable)Tables["PQRSReports"]; } }
        public PQRSMeasureDataDataTable PQRSMeasureData { get { return (PQRSMeasureDataDataTable)Tables["PQRSMeasureData"]; } }
        public PQRS_MeasureReasonsDataTable PQRS_MeasureReasons { get { return (PQRS_MeasureReasonsDataTable)Tables["PQRS_MeasureReasons"]; } }
        public PQRS_PatientFromVisitsDataTable PQRS_PatientFromVisits { get { return (PQRS_PatientFromVisitsDataTable)Tables["PQRS_PatientFromVisits"]; } }
        public PatientPQRSDataTable PatientPQRS { get { return (PatientPQRSDataTable)Tables["PatientPQRS"]; } }
        public IndividualReportingDataTable IndividualReporting { get { return (IndividualReportingDataTable)Tables["IndividualReporting"]; } }
        public IndividualReportingListDataTable IndividualReportingList { get { return (IndividualReportingListDataTable)Tables["IndividualReportingList"]; } }
        public MeasureGroupLookupDataTable MeasureGroupLookup { get { return (MeasureGroupLookupDataTable)Tables["MeasureGroupLookup"]; } }
        public MeasureGroupsDataTable MeasureGroups { get { return (MeasureGroupsDataTable)Tables["MeasureGroups"]; } }
        public MeasureGroupsListDataTable MeasureGroupsList { get { return (MeasureGroupsListDataTable)Tables["MeasureGroupsList"]; } }
        public MeasuresDataTable Measures { get { return (MeasuresDataTable)Tables["Measures"]; } }
        public PatientListDataTable PatientList { get { return (PatientListDataTable)Tables["PatientList"]; } }
        public PQRS_PatientsListDataTable PQRS_PatientsList { get { return (PQRS_PatientsListDataTable)Tables["PQRS_PatientsList"]; } }
        public DataTable PQRS { get { return Tables["PQRS"]; } }

        public class PQRSMeasureDataTable : DataTable
        {
            public PQRSMeasureDataTable() : base("PQRSMeasure")
            {
                var id = new DataColumn("PQRSMeasureId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "MeasureName","MeasureNumber","Description","CPTCode","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PQRSMeasureRow NewPQRSMeasureRow() { return (PQRSMeasureRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PQRSMeasureRow(b); }
            protected override Type GetRowType() { return typeof(PQRSMeasureRow); }
            public DataColumn PQRSMeasureIdColumn { get { return Columns["PQRSMeasureId"]; } }
            public DataColumn MeasureNameColumn { get { return Columns["MeasureName"]; } }
            public DataColumn CPTCodeColumn { get { return Columns["CPTCode"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class PQRSMeasureRow : DataRow
        {
            internal PQRSMeasureRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PQRSMeasureId { get { var v = this["PQRSMeasureId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string MeasureName { get { return G("MeasureName"); } set { S("MeasureName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class PQRSReportsDataTable : DataTable
        {
            public PQRSReportsDataTable() : base("PQRSReports")
            {
                var id = new DataColumn("PQRSReportId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PatientId","VisitId","NoteId","PQRSMeasureId","ReportDate","Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","NPI","ProviderName","TIN","Patients","MeasureNumber" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PQRSReportsRow NewPQRSReportsRow() { return (PQRSReportsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PQRSReportsRow(b); }
            protected override Type GetRowType() { return typeof(PQRSReportsRow); }
            public DataColumn PQRSReportIdColumn { get { return Columns["PQRSReportId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn NPIColumn { get { return Columns["NPI"]; } }
            public DataColumn ProviderNameColumn { get { return Columns["ProviderName"]; } }
            public DataColumn TINColumn { get { return Columns["TIN"]; } }
            public DataColumn PatientsColumn { get { return Columns["Patients"]; } }
            public DataColumn MeasureNumberColumn { get { return Columns["MeasureNumber"]; } }
        }
        public class PQRSReportsRow : DataRow
        {
            internal PQRSReportsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PQRSReportId { get { var v = this["PQRSReportId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class PQRSMeasureDataDataTable : DataTable
        {
            public PQRSMeasureDataDataTable() : base("PQRSMeasureData")
            {
                var id = new DataColumn("PQRSMeasureDataId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PQRSMeasureId","DataValue","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PQRSMeasureDataRow NewPQRSMeasureDataRow() { return (PQRSMeasureDataRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PQRSMeasureDataRow(b); }
            protected override Type GetRowType() { return typeof(PQRSMeasureDataRow); }
            public DataColumn PQRSMeasureDataIdColumn { get { return Columns["PQRSMeasureDataId"]; } }
            public DataColumn PQRSMeasureIdColumn { get { return Columns["PQRSMeasureId"]; } }
            public DataColumn DataValueColumn { get { return Columns["DataValue"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class PQRSMeasureDataRow : DataRow
        {
            internal PQRSMeasureDataRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PQRSMeasureDataId { get { var v = this["PQRSMeasureDataId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PQRSMeasureId { get { return G("PQRSMeasureId"); } set { S("PQRSMeasureId", value); } }
        }

        public class PQRS_MeasureReasonsDataTable : DataTable
        {
            public PQRS_MeasureReasonsDataTable() : base("PQRS_MeasureReasons")
            {
                var id = new DataColumn("ReasonId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PQRSMeasureId","ReasonCode","ReasonDescription","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PQRS_MeasureReasonsRow NewPQRS_MeasureReasonsRow() { return (PQRS_MeasureReasonsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PQRS_MeasureReasonsRow(b); }
            protected override Type GetRowType() { return typeof(PQRS_MeasureReasonsRow); }
            public DataColumn ReasonIdColumn { get { return Columns["ReasonId"]; } }
            public DataColumn PQRSMeasureIdColumn { get { return Columns["PQRSMeasureId"]; } }
            public DataColumn ReasonCodeColumn { get { return Columns["ReasonCode"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class PQRS_MeasureReasonsRow : DataRow
        {
            internal PQRS_MeasureReasonsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ReasonId { get { var v = this["ReasonId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PQRSMeasureId { get { return G("PQRSMeasureId"); } set { S("PQRSMeasureId", value); } }
        }

        public class PQRS_PatientFromVisitsDataTable : DataTable
        {
            public PQRS_PatientFromVisitsDataTable() : base("PQRS_PatientFromVisits")
            {
                var id = new DataColumn("VisitPatientId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PatientId","VisitId","VisitDate","ProviderId","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PQRS_PatientFromVisitsRow NewPQRS_PatientFromVisitsRow() { return (PQRS_PatientFromVisitsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PQRS_PatientFromVisitsRow(b); }
            protected override Type GetRowType() { return typeof(PQRS_PatientFromVisitsRow); }
            public DataColumn VisitPatientIdColumn { get { return Columns["VisitPatientId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn VisitIdColumn { get { return Columns["VisitId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class PQRS_PatientFromVisitsRow : DataRow
        {
            internal PQRS_PatientFromVisitsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long VisitPatientId { get { var v = this["VisitPatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
        }

        public class PatientPQRSDataTable : DataTable
        {
            public PatientPQRSDataTable() : base("PatientPQRS")
            {
                var id = new DataColumn("PatientPQRSId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","PQRSMeasureId","Value","IsActive",
                    "FirstName","LastName","HomePhoneNo","RaceCode","RaceDescription",
                    "EthnicityCode","EthnicityDescription","AccountNumber","Address1","City",
                    "State","ZIPCode","Gender","DOB","MaritialStatus","LanguageCode" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientPQRSRow NewPatientPQRSRow() { return (PatientPQRSRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientPQRSRow(b); }
            protected override Type GetRowType() { return typeof(PatientPQRSRow); }
            public DataColumn PatientPQRSIdColumn { get { return Columns["PatientPQRSId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn PQRSMeasureIdColumn { get { return Columns["PQRSMeasureId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn FirstNameColumn { get { return Columns["FirstName"]; } }
            public DataColumn LastNameColumn { get { return Columns["LastName"]; } }
            public DataColumn HomePhoneNoColumn { get { return Columns["HomePhoneNo"]; } }
            public DataColumn RaceCodeColumn { get { return Columns["RaceCode"]; } }
            public DataColumn RaceDescriptionColumn { get { return Columns["RaceDescription"]; } }
            public DataColumn EthnicityCodeColumn { get { return Columns["EthnicityCode"]; } }
            public DataColumn EthnicityDescriptionColumn { get { return Columns["EthnicityDescription"]; } }
            public DataColumn AccountNumberColumn { get { return Columns["AccountNumber"]; } }
            public DataColumn Address1Column { get { return Columns["Address1"]; } }
            public DataColumn CityColumn { get { return Columns["City"]; } }
            public DataColumn StateColumn { get { return Columns["State"]; } }
            public DataColumn ZIPCodeColumn { get { return Columns["ZIPCode"]; } }
            public DataColumn GenderColumn { get { return Columns["Gender"]; } }
            public DataColumn DOBColumn { get { return Columns["DOB"]; } }
            public DataColumn MaritialStatusColumn { get { return Columns["MaritialStatus"]; } }
            public DataColumn LanguageCodeColumn { get { return Columns["LanguageCode"]; } }
        }
        public class PatientPQRSRow : DataRow
        {
            internal PatientPQRSRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PatientPQRSId { get { var v = this["PatientPQRSId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
        }

        // ── IndividualReporting ──────────────────────────────────────────────────
        public class IndividualReportingDataTable : DataTable
        {
            public IndividualReportingDataTable() : base("IndividualReporting")
            {
                var id = new DataColumn("MeasureIndividualId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ProviderId","SpecialityId","SubmissionYear","EntityId","IsReported",
                    "MeasureIds","PracticeIds","IsActive","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn","ErrorMessage","MeasureGroupId",
                    "ProviderMembers","MeasureGroupName","PerformanceYear",
                    "IAMeasureIds","CQMMeasureIds","ProviderIds" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public IndividualReportingRow NewIndividualReportingRow() { return (IndividualReportingRow)NewRow(); }
            public void AddIndividualReportingRow(IndividualReportingRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new IndividualReportingRow(b); }
            protected override Type GetRowType() { return typeof(IndividualReportingRow); }
            public DataColumn MeasureIndividualIdColumn { get { return Columns["MeasureIndividualId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn SpecialityIdColumn { get { return Columns["SpecialityId"]; } }
            public DataColumn SubmissionYearColumn { get { return Columns["SubmissionYear"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn IsReportedColumn { get { return Columns["IsReported"]; } }
            public DataColumn MeasureIdsColumn { get { return Columns["MeasureIds"]; } }
            public DataColumn PracticeIdsColumn { get { return Columns["PracticeIds"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ErrorMessageColumn { get { return Columns["ErrorMessage"]; } }
            public DataColumn MeasureGroupIdColumn { get { return Columns["MeasureGroupId"]; } }
            public DataColumn ProviderMembersColumn { get { return Columns["ProviderMembers"]; } }
            public DataColumn MeasureGroupNameColumn { get { return Columns["MeasureGroupName"]; } }
            public DataColumn PerformanceYearColumn { get { return Columns["PerformanceYear"]; } }
            public DataColumn IAMeasureIdsColumn { get { return Columns["IAMeasureIds"]; } }
            public DataColumn CQMMeasureIdsColumn { get { return Columns["CQMMeasureIds"]; } }
            public DataColumn ProviderIdsColumn { get { return Columns["ProviderIds"]; } }
        }
        public class IndividualReportingRow : DataRow
        {
            internal IndividualReportingRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MeasureIndividualId { get { var v = this["MeasureIndividualId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string SpecialityId { get { return G("SpecialityId"); } set { S("SpecialityId", value); } }
            public string SubmissionYear { get { return G("SubmissionYear"); } set { S("SubmissionYear", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public long EntityId { get { var v = this["EntityId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["EntityId"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── IndividualReportingList ──────────────────────────────────────────────
        public class IndividualReportingListDataTable : DataTable
        {
            public IndividualReportingListDataTable() : base("IndividualReportingList")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public IndividualReportingListRow NewIndividualReportingListRow() { return (IndividualReportingListRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new IndividualReportingListRow(b); }
            protected override Type GetRowType() { return typeof(IndividualReportingListRow); }
            public DataColumn IdColumn { get { return Columns["Id"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class IndividualReportingListRow : DataRow
        {
            internal IndividualReportingListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        // ── MeasureGroupLookup ───────────────────────────────────────────────────
        public class MeasureGroupLookupDataTable : DataTable
        {
            public MeasureGroupLookupDataTable() : base("MeasureGroupLookup")
            {
                var id = new DataColumn("MeasureGroupId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                Columns.Add(new DataColumn("ShortName", typeof(string)) { DefaultValue = "" });
            }
            public MeasureGroupLookupRow NewMeasureGroupLookupRow() { return (MeasureGroupLookupRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MeasureGroupLookupRow(b); }
            protected override Type GetRowType() { return typeof(MeasureGroupLookupRow); }
            public DataColumn MeasureGroupIdColumn { get { return Columns["MeasureGroupId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
        }
        public class MeasureGroupLookupRow : DataRow
        {
            internal MeasureGroupLookupRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MeasureGroupId { get { var v = this["MeasureGroupId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName { get { return G("ShortName"); } set { S("ShortName", value); } }
        }

        // ── MeasureGroups ────────────────────────────────────────────────────────
        public class MeasureGroupsDataTable : DataTable
        {
            public MeasureGroupsDataTable() : base("MeasureGroups")
            {
                var id = new DataColumn("MeasureGroupId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ShortName","SubmissionYear","IsReported","ProviderIds","MeasureIds",
                    "PracticeIds","SpecialtyIds","IsActive","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn","EntityId","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public MeasureGroupsRow NewMeasureGroupsRow() { return (MeasureGroupsRow)NewRow(); }
            public void AddMeasureGroupsRow(MeasureGroupsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MeasureGroupsRow(b); }
            protected override Type GetRowType() { return typeof(MeasureGroupsRow); }
            public DataColumn MeasureGroupIdColumn { get { return Columns["MeasureGroupId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn SubmissionYearColumn { get { return Columns["SubmissionYear"]; } }
            public DataColumn IsReportedColumn { get { return Columns["IsReported"]; } }
            public DataColumn ProviderIdsColumn { get { return Columns["ProviderIds"]; } }
            public DataColumn MeasureIdsColumn { get { return Columns["MeasureIds"]; } }
            public DataColumn PracticeIdsColumn { get { return Columns["PracticeIds"]; } }
            public DataColumn SpecialtyIdsColumn { get { return Columns["SpecialtyIds"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class MeasureGroupsRow : DataRow
        {
            internal MeasureGroupsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MeasureGroupId { get { var v = this["MeasureGroupId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName { get { return G("ShortName"); } set { S("ShortName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public long EntityId { get { var v = this["EntityId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["EntityId"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── MeasureGroupsList ────────────────────────────────────────────────────
        public class MeasureGroupsListDataTable : DataTable
        {
            public MeasureGroupsListDataTable() : base("MeasureGroupsList")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public MeasureGroupsListRow NewMeasureGroupsListRow() { return (MeasureGroupsListRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MeasureGroupsListRow(b); }
            protected override Type GetRowType() { return typeof(MeasureGroupsListRow); }
            public DataColumn IdColumn { get { return Columns["Id"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class MeasureGroupsListRow : DataRow
        {
            internal MeasureGroupsListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        // ── Measures ─────────────────────────────────────────────────────────────
        public class MeasuresDataTable : DataTable
        {
            public MeasuresDataTable() : base("Measures")
            {
                var id = new DataColumn("MeasureId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "DocumentPath","DocumentName","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public MeasuresRow NewMeasuresRow() { return (MeasuresRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MeasuresRow(b); }
            protected override Type GetRowType() { return typeof(MeasuresRow); }
            public DataColumn MeasureIdColumn { get { return Columns["MeasureId"]; } }
            public DataColumn DocumentPathColumn { get { return Columns["DocumentPath"]; } }
            public DataColumn DocumentNameColumn { get { return Columns["DocumentName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class MeasuresRow : DataRow
        {
            internal MeasuresRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MeasureId { get { var v = this["MeasureId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string DocumentPath { get { return G("DocumentPath"); } set { S("DocumentPath", value); } }
            public string DocumentName { get { return G("DocumentName"); } set { S("DocumentName", value); } }
        }

        // ── PQRS_PatientsList ────────────────────────────────────────────────────
        public class PQRS_PatientsListDataTable : DataTable
        {
            public PQRS_PatientsListDataTable() : base("PQRS_PatientsList")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                foreach (var c in new[] {
                    "PatientId","FirstName","LastName","DOB","Gender","AccountNumber",
                    "HomePhoneNo","Address1","City","State","ZIPCode",
                    "RaceCode","RaceDescription","EthnicityCode","EthnicityDescription",
                    "LanguageCode","MaritialStatus","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public DataColumn IdColumn { get { return Columns["Id"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn FirstNameColumn { get { return Columns["FirstName"]; } }
            public DataColumn LastNameColumn { get { return Columns["LastName"]; } }
            public DataColumn DOBColumn { get { return Columns["DOB"]; } }
            public DataColumn GenderColumn { get { return Columns["Gender"]; } }
            public DataColumn AccountNumberColumn { get { return Columns["AccountNumber"]; } }
            public DataColumn HomePhoneNoColumn { get { return Columns["HomePhoneNo"]; } }
            public DataColumn Address1Column { get { return Columns["Address1"]; } }
            public DataColumn CityColumn { get { return Columns["City"]; } }
            public DataColumn StateColumn { get { return Columns["State"]; } }
            public DataColumn ZIPCodeColumn { get { return Columns["ZIPCode"]; } }
            public DataColumn RaceCodeColumn { get { return Columns["RaceCode"]; } }
            public DataColumn RaceDescriptionColumn { get { return Columns["RaceDescription"]; } }
            public DataColumn EthnicityCodeColumn { get { return Columns["EthnicityCode"]; } }
            public DataColumn EthnicityDescriptionColumn { get { return Columns["EthnicityDescription"]; } }
            public DataColumn LanguageCodeColumn { get { return Columns["LanguageCode"]; } }
            public DataColumn MaritialStatusColumn { get { return Columns["MaritialStatus"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        // ── PatientList ──────────────────────────────────────────────────────────
        public class PatientListDataTable : DataTable
        {
            public PatientListDataTable() : base("PatientList")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","VisitId","TreatmentTypeId","Measure",
                    "ReasonComments","ReasonTypeId","CreatedOn","CreatedBy" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientListRow NewPatientListRow() { return (PatientListRow)NewRow(); }
            public void AddPatientListRow(PatientListRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientListRow(b); }
            protected override Type GetRowType() { return typeof(PatientListRow); }
            public DataColumn IdColumn { get { return Columns["Id"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn VisitIdColumn { get { return Columns["VisitId"]; } }
            public DataColumn TreatmentTypeIdColumn { get { return Columns["TreatmentTypeId"]; } }
            public DataColumn MeasureColumn { get { return Columns["Measure"]; } }
            public DataColumn ReasonCommentsColumn { get { return Columns["ReasonComments"]; } }
            public DataColumn ReasonTypeIdColumn { get { return Columns["ReasonTypeId"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
        }
        public class PatientListRow : DataRow
        {
            internal PatientListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string VisitId { get { return G("VisitId"); } set { S("VisitId", value); } }
            public string Measure { get { return G("Measure"); } set { S("Measure", value); } }
            public string ReasonComments { get { return G("ReasonComments"); } set { S("ReasonComments", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
        }
    }
}
