using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSCaseReports : DataSet
    {
        private PlanofCareDataTable _tPlanofCare;
        private CaseReportDataTable _tCaseReport;
        private PlanOfCareGoalDataTable _tPlanOfCareGoal;
        private FunctionalStatusDataTable _tFunctionalStatus;
        private MentalStatus_CCDADataTable _tMentalStatus_CCDA;
        private ImmunizationDataTable _tImmunization;

        public DSCaseReports()
        {
            DataSetName = "DSCaseReports";
            _tPlanofCare = new PlanofCareDataTable();
            _tCaseReport = new CaseReportDataTable();
            _tPlanOfCareGoal = new PlanOfCareGoalDataTable();
            _tFunctionalStatus = new FunctionalStatusDataTable();
            _tMentalStatus_CCDA = new MentalStatus_CCDADataTable();
            _tImmunization = new ImmunizationDataTable();
            Tables.Add(_tPlanofCare);
            Tables.Add(_tCaseReport);
            Tables.Add(_tPlanOfCareGoal);
            Tables.Add(_tFunctionalStatus);
            Tables.Add(_tMentalStatus_CCDA);
            Tables.Add(_tImmunization);
        }

        public PlanofCareDataTable PlanofCare { get { return _tPlanofCare; } }
        public CaseReportDataTable CaseReport { get { return _tCaseReport; } }
        public PlanOfCareGoalDataTable PlanOfCareGoal { get { return _tPlanOfCareGoal; } }
        public FunctionalStatusDataTable FunctionalStatus { get { return _tFunctionalStatus; } }
        public MentalStatus_CCDADataTable MentalStatus_CCDA { get { return _tMentalStatus_CCDA; } }
        public ImmunizationDataTable Immunization { get { return _tImmunization; } }

        public class PlanofCareDataTable : DataTable
        {
            public PlanofCareDataTable() : base("PlanofCare")
            {
                var id = new DataColumn("PlanofCareId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","ClinicalInstruction","FutureInstruction","Goals","Comments",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","RecordCount","PatientDecisionAid" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PlanofCareRow NewPlanofCareRow() { return (PlanofCareRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PlanofCareRow(b); }
            protected override Type GetRowType() { return typeof(PlanofCareRow); }

            public DataColumn PlanofCareIdColumn { get { return Columns["PlanofCareId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn ClinicalInstructionColumn { get { return Columns["ClinicalInstruction"]; } }
            public DataColumn FutureInstructionColumn { get { return Columns["FutureInstruction"]; } }
            public DataColumn GoalsColumn { get { return Columns["Goals"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn PatientDecisionAidColumn { get { return Columns["PatientDecisionAid"]; } }
        }

        public class PlanofCareRow : DataRow
        {
            internal PlanofCareRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PlanofCareId { get { var v = this["PlanofCareId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string ClinicalInstruction { get { return G("ClinicalInstruction"); } set { S("ClinicalInstruction", value); } }
            public string FutureInstruction { get { return G("FutureInstruction"); } set { S("FutureInstruction", value); } }
            public string Goals { get { return G("Goals"); } set { S("Goals", value); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string CreatedOn { get { return G("CreatedOn"); } set { S("CreatedOn", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public string ModifiedOn { get { return G("ModifiedOn"); } set { S("ModifiedOn", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class CaseReportDataTable : DataTable
        {
            public CaseReportDataTable() : base("CaseReport")
            {
                var id = new DataColumn("CaseReportId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","ReportText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public CaseReportRow NewCaseReportRow() { return (CaseReportRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new CaseReportRow(b); }
            protected override Type GetRowType() { return typeof(CaseReportRow); }

            public DataColumn CaseReportIdColumn { get { return Columns["CaseReportId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn ReportTextColumn { get { return Columns["ReportText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class CaseReportRow : DataRow
        {
            internal CaseReportRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long CaseReportId { get { var v = this["CaseReportId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string ReportText { get { return G("ReportText"); } set { S("ReportText", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string CreatedOn { get { return G("CreatedOn"); } set { S("CreatedOn", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public string ModifiedOn { get { return G("ModifiedOn"); } set { S("ModifiedOn", value); } }
        }

        // ── PlanOfCareGoal ──────────────────────────────────────────────────────
        public class PlanOfCareGoalDataTable : DataTable
        {
            public PlanOfCareGoalDataTable() : base("PlanOfCareGoal")
            {
                var id = new DataColumn("GoalId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PlanOfCareId","ICD9Code","ICD9CodeDescription","ICD10Code","ICD10CodeDescription",
                    "SNOMEDID","SNOMEDDescription","LexiCode","LexiCodeDescription","Instruction",
                    "Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PlanOfCareGoalRow NewPlanOfCareGoalRow() { return (PlanOfCareGoalRow)NewRow(); }
            public void AddPlanOfCareGoalRow(PlanOfCareGoalRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PlanOfCareGoalRow(b); }
            protected override Type GetRowType() { return typeof(PlanOfCareGoalRow); }
            public DataColumn GoalIdColumn { get { return Columns["GoalId"]; } }
            public DataColumn PlanOfCareIdColumn { get { return Columns["PlanOfCareId"]; } }
            public DataColumn ICD9CodeColumn { get { return Columns["ICD9Code"]; } }
            public DataColumn ICD9CodeDescriptionColumn { get { return Columns["ICD9CodeDescription"]; } }
            public DataColumn ICD10CodeColumn { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn InstructionColumn { get { return Columns["Instruction"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }
        public class PlanOfCareGoalRow : DataRow
        {
            internal PlanOfCareGoalRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long GoalId { get { var v = this["GoalId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["GoalId"] = value; } }
            public string PlanOfCareId { get { return G("PlanOfCareId"); } set { S("PlanOfCareId", value); } }
            public string Instruction { get { return G("Instruction"); } set { S("Instruction", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string ICD10CodeDescription { get { return G("ICD10CodeDescription"); } set { S("ICD10CodeDescription", value); } }
            public string SNOMEDID { get { return G("SNOMEDID"); } set { S("SNOMEDID", value); } }
            public string SNOMEDDescription { get { return G("SNOMEDDescription"); } set { S("SNOMEDDescription", value); } }
            public string LexiCode { get { return G("LexiCode"); } set { S("LexiCode", value); } }
            public string LexiCodeDescription { get { return G("LexiCodeDescription"); } set { S("LexiCodeDescription", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── FunctionalStatus ────────────────────────────────────────────────────
        public class FunctionalStatusDataTable : DataTable
        {
            public FunctionalStatusDataTable() : base("FunctionalStatus")
            {
                var id = new DataColumn("FunctionalStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "Name","SNOMEDID","EffectiveDate","Type","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FunctionalStatusRow NewFunctionalStatusRow() { return (FunctionalStatusRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FunctionalStatusRow(b); }
            protected override Type GetRowType() { return typeof(FunctionalStatusRow); }
            public DataColumn FunctionalStatusIdColumn { get { return Columns["FunctionalStatusId"]; } }
            public DataColumn NameColumn { get { return Columns["Name"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn EffectiveDateColumn { get { return Columns["EffectiveDate"]; } }
            public DataColumn TypeColumn { get { return Columns["Type"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class FunctionalStatusRow : DataRow
        {
            internal FunctionalStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long FunctionalStatusId { get { var v = this["FunctionalStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Name { get { return G("Name"); } set { S("Name", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── MentalStatus_CCDA ───────────────────────────────────────────────────
        public class MentalStatus_CCDADataTable : DataTable
        {
            public MentalStatus_CCDADataTable() : base("MentalStatus_CCDA")
            {
                var id = new DataColumn("MentalStatus_CcdaId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "CreatedOn","SNOMEDID","SNOMEDDescription","Instruction" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public MentalStatus_CCDARow NewMentalStatus_CCDARow() { return (MentalStatus_CCDARow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MentalStatus_CCDARow(b); }
            protected override Type GetRowType() { return typeof(MentalStatus_CCDARow); }
            public DataColumn MentalStatus_CcdaIdColumn { get { return Columns["MentalStatus_CcdaId"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn InstructionColumn { get { return Columns["Instruction"]; } }
        }
        public class MentalStatus_CCDARow : DataRow
        {
            internal MentalStatus_CCDARow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MentalStatus_CcdaId { get { var v = this["MentalStatus_CcdaId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Instruction { get { return G("Instruction"); } set { S("Instruction", value); } }
        }

        // ── Immunization ────────────────────────────────────────────────────────
        public class ImmunizationDataTable : DataTable
        {
            public ImmunizationDataTable() : base("Immunization")
            {
                var id = new DataColumn("ImmunizationId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","CVXCode","CVX","VaccineName","AdministrationDate",
                    "VaccineStatus","CompletionStatusCode","ExpiryDate",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ImmunizationRow NewImmunizationRow() { return (ImmunizationRow)NewRow(); }
            public void AddImmunizationRow(ImmunizationRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ImmunizationRow(b); }
            protected override Type GetRowType() { return typeof(ImmunizationRow); }
            public DataColumn ImmunizationIdColumn { get { return Columns["ImmunizationId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn CVXCodeColumn { get { return Columns["CVXCode"]; } }
            public DataColumn CVXColumn { get { return Columns["CVX"]; } }
            public DataColumn VaccineNameColumn { get { return Columns["VaccineName"]; } }
            public DataColumn AdministrationDateColumn { get { return Columns["AdministrationDate"]; } }
            public DataColumn VaccineStatusColumn { get { return Columns["VaccineStatus"]; } }
            public DataColumn CompletionStatusCodeColumn { get { return Columns["CompletionStatusCode"]; } }
            public DataColumn ExpiryDateColumn { get { return Columns["ExpiryDate"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }
        public class ImmunizationRow : DataRow
        {
            internal ImmunizationRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ImmunizationId { get { var v = this["ImmunizationId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string CVXCode { get { return G("CVXCode"); } set { S("CVXCode", value); } }
            public string CVX { get { return G("CVX"); } set { S("CVX", value); } }
            public string VaccineName { get { return G("VaccineName"); } set { S("VaccineName", value); } }
            public string AdministrationDate { get { return G("AdministrationDate"); } set { S("AdministrationDate", value); } }
            public string VaccineStatus { get { return G("VaccineStatus"); } set { S("VaccineStatus", value); } }
            public string CompletionStatusCode { get { return G("CompletionStatusCode"); } set { S("CompletionStatusCode", value); } }
            public string ExpiryDate { get { return G("ExpiryDate"); } set { S("ExpiryDate", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string CreatedOn { get { return G("CreatedOn"); } set { S("CreatedOn", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public string ModifiedOn { get { return G("ModifiedOn"); } set { S("ModifiedOn", value); } }
        }
    }
}
