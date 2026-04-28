using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSClinicalSummary : DataSet
    {
        public DSClinicalSummary()
        {
            DataSetName = "DSClinicalSummary";
            Tables.Add(new ComplaintDetailDataTable());
            Tables.Add(new PlanofCareDataTable());
            Tables.Add(new PlanOfCareGoalDataTable());
            Tables.Add(new ImmunizationDataTable());
            Tables.Add(new OutPatientEncounterDataTable());
            Tables.Add(new AUP_ReportDataTable());
            Tables.Add(new AllergyDataTable());
            Tables.Add(new CognitiveDataTable());
            Tables.Add(new Cognitive_StatusDataTable());
            Tables.Add(new Cognitive_FunctionalStatusDataTable());
            Tables.Add(new Cognitive_MentalStatusDataTable());
            Tables.Add(new PatientMiscellaneousStatusDataTable());
            Tables.Add(new ProblemListDataTable());
            Tables.Add(new ConsultationOrderDataTable());
            Tables.Add(new LabOrderResultDataTable());
            Tables.Add(new LabOrderResultDetailDataTable());
            Tables.Add(new ProceduresDataTable());
            Tables.Add(new ProviderDataTable());
            Tables.Add(new OccupationStatusDataTable());
            Tables.Add(new EncounterProblemListDataTable());
            Tables.Add(new FunctionalStatusDataTable());
            Tables.Add(new MentalStatus_CCDADataTable());
            Tables.Add(new ARO_FacilitySpecimenDataTable());
            Tables.Add(new ARO_AntimicrobialDataTable());
            Tables.Add(new ARO_ObservationsDataTable());
            Tables.Add(new LanguagesDataTable());
            Tables.Add(new RaceDataTable());
            Tables.Add(new EthnicityDataTable());
        }

        public ComplaintDetailDataTable ComplaintDetail { get { return (ComplaintDetailDataTable)Tables["ComplaintDetail"]; } }
        public PlanofCareDataTable PlanofCare { get { return (PlanofCareDataTable)Tables["PlanofCare"]; } }
        public PlanOfCareGoalDataTable PlanOfCareGoal { get { return (PlanOfCareGoalDataTable)Tables["PlanOfCareGoal"]; } }
        public ImmunizationDataTable Immunization { get { return (ImmunizationDataTable)Tables["Immunization"]; } }
        public OutPatientEncounterDataTable OutPatientEncounter { get { return (OutPatientEncounterDataTable)Tables["OutPatientEncounter"]; } }
        public AUP_ReportDataTable AUP_Report { get { return (AUP_ReportDataTable)Tables["AUP_Report"]; } }
        public AllergyDataTable Allergy { get { return (AllergyDataTable)Tables["Allergy"]; } }
        public CognitiveDataTable Cognitive { get { return (CognitiveDataTable)Tables["Cognitive"]; } }
        public Cognitive_StatusDataTable Cognitive_Status { get { return (Cognitive_StatusDataTable)Tables["Cognitive_Status"]; } }
        public Cognitive_FunctionalStatusDataTable Cognitive_FunctionalStatus { get { return (Cognitive_FunctionalStatusDataTable)Tables["Cognitive_FunctionalStatus"]; } }
        public Cognitive_MentalStatusDataTable Cognitive_MentalStatus { get { return (Cognitive_MentalStatusDataTable)Tables["Cognitive_MentalStatus"]; } }
        public PatientMiscellaneousStatusDataTable PatientMiscellaneousStatus { get { return (PatientMiscellaneousStatusDataTable)Tables["PatientMiscellaneousStatus"]; } }
        public ProblemListDataTable ProblemList { get { return (ProblemListDataTable)Tables["ProblemList"]; } }
        public ConsultationOrderDataTable ConsultationOrder { get { return (ConsultationOrderDataTable)Tables["ConsultationOrder"]; } }
        public LabOrderResultDataTable LabOrderResult { get { return (LabOrderResultDataTable)Tables["LabOrderResult"]; } }
        public LabOrderResultDetailDataTable LabOrderResultDetail { get { return (LabOrderResultDetailDataTable)Tables["LabOrderResultDetail"]; } }
        public ProceduresDataTable Procedures { get { return (ProceduresDataTable)Tables["Procedures"]; } }
        public ProviderDataTable Provider { get { return (ProviderDataTable)Tables["Provider"]; } }
        public OccupationStatusDataTable OccupationStatus { get { return (OccupationStatusDataTable)Tables["OccupationStatus"]; } }
        public EncounterProblemListDataTable EncounterProblemList { get { return (EncounterProblemListDataTable)Tables["EncounterProblemList"]; } }
        public FunctionalStatusDataTable FunctionalStatus { get { return (FunctionalStatusDataTable)Tables["FunctionalStatus"]; } }
        public MentalStatus_CCDADataTable MentalStatus_CCDA { get { return (MentalStatus_CCDADataTable)Tables["MentalStatus_CCDA"]; } }
        public ARO_FacilitySpecimenDataTable ARO_FacilitySpecimen { get { return (ARO_FacilitySpecimenDataTable)Tables["ARO_FacilitySpecimen"]; } }
        public ARO_AntimicrobialDataTable ARO_Antimicrobial { get { return (ARO_AntimicrobialDataTable)Tables["ARO_Antimicrobial"]; } }
        public ARO_ObservationsDataTable ARO_Observations { get { return (ARO_ObservationsDataTable)Tables["ARO_Observations"]; } }
        public LanguagesDataTable Languages { get { return (LanguagesDataTable)Tables["Languages"]; } }
        public RaceDataTable Race { get { return (RaceDataTable)Tables["Race"]; } }
        public EthnicityDataTable Ethnicity { get { return (EthnicityDataTable)Tables["Ethnicity"]; } }

        // ── ComplaintDetail ─────────────────────────────────────────────────────
        public class ComplaintDetailDataTable : DataTable
        {
            public ComplaintDetailDataTable() : base("ComplaintDetail")
            {
                var id = new DataColumn("ComplaintDetailId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PatientId","NoteId","ComplaintText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "ComplaintDescription","SNOMEDID","SNOMEDDescription","ICD10Code","ICD10CodeDescription" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ComplaintDetailRow NewComplaintDetailRow() { return (ComplaintDetailRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ComplaintDetailRow(b); }
            protected override Type GetRowType() { return typeof(ComplaintDetailRow); }
            public DataColumn ComplaintDetailIdColumn { get { return Columns["ComplaintDetailId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn ComplaintTextColumn { get { return Columns["ComplaintText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ComplaintDescriptionColumn { get { return Columns["ComplaintDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn ICD10CodeColumn { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
        }
        public class ComplaintDetailRow : DataRow
        {
            internal ComplaintDetailRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ComplaintDetailId { get { var v = this["ComplaintDetailId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string ComplaintText { get { return G("ComplaintText"); } set { S("ComplaintText", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── PlanofCare ──────────────────────────────────────────────────────────
        public class PlanofCareDataTable : DataTable
        {
            public PlanofCareDataTable() : base("PlanofCare")
            {
                var id = new DataColumn("PlanofCareId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","ClinicalInstruction","FutureInstruction","Goals","Comments",
                    "SoapText","PatientDecisionAid","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PlanofCareRow NewPlanofCareRow() { return (PlanofCareRow)NewRow(); }
            public void AddPlanofCareRow(PlanofCareRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PlanofCareRow(b); }
            protected override Type GetRowType() { return typeof(PlanofCareRow); }
            public DataColumn PlanofCareIdColumn { get { return Columns["PlanofCareId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn ClinicalInstructionColumn { get { return Columns["ClinicalInstruction"]; } }
            public DataColumn FutureInstructionColumn { get { return Columns["FutureInstruction"]; } }
            public DataColumn GoalsColumn { get { return Columns["Goals"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn PatientDecisionAidColumn { get { return Columns["PatientDecisionAid"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class PlanofCareRow : DataRow
        {
            internal PlanofCareRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PlanofCareId { get { var v = this["PlanofCareId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public long NoteId { get { var v = this["NoteId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["NoteId"] = value.ToString(); } }
            public string ClinicalInstruction { get { return G("ClinicalInstruction"); } set { S("ClinicalInstruction", value); } }
            public string FutureInstruction { get { return G("FutureInstruction"); } set { S("FutureInstruction", value); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public string PatientDecisionAid { get { return G("PatientDecisionAid"); } set { S("PatientDecisionAid", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
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
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
        }
        public class PlanOfCareGoalRow : DataRow
        {
            internal PlanOfCareGoalRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long GoalId { get { var v = this["GoalId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["GoalId"] = value; } }
            public long PlanOfCareId { get { var v = this["PlanOfCareId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PlanOfCareId"] = value.ToString(); } }
            public string Instruction { get { return G("Instruction"); } set { S("Instruction", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string ICD9Code { get { return G("ICD9Code"); } set { S("ICD9Code", value); } }
            public string ICD9CodeDescription { get { return G("ICD9CodeDescription"); } set { S("ICD9CodeDescription", value); } }
            public string ICD10Code { get { return G("ICD10Code"); } set { S("ICD10Code", value); } }
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

        // ── Immunization ────────────────────────────────────────────────────────
        public class ImmunizationDataTable : DataTable
        {
            public ImmunizationDataTable() : base("Immunization")
            {
                var id = new DataColumn("ImmunizationId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","CVXCode","VaccineName","AdministrationDate","LotNumber",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "VaccineStatus","CVX","CompletionStatusCode","ExpiryDate" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ImmunizationRow NewImmunizationRow() { return (ImmunizationRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ImmunizationRow(b); }
            protected override Type GetRowType() { return typeof(ImmunizationRow); }
            public DataColumn ImmunizationIdColumn { get { return Columns["ImmunizationId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn CVXCodeColumn { get { return Columns["CVXCode"]; } }
            public DataColumn VaccineNameColumn { get { return Columns["VaccineName"]; } }
            public DataColumn AdministrationDateColumn { get { return Columns["AdministrationDate"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn VaccineStatusColumn { get { return Columns["VaccineStatus"]; } }
            public DataColumn CVXColumn { get { return Columns["CVX"]; } }
            public DataColumn CompletionStatusCodeColumn { get { return Columns["CompletionStatusCode"]; } }
            public DataColumn ExpiryDateColumn { get { return Columns["ExpiryDate"]; } }
        }
        public class ImmunizationRow : DataRow
        {
            internal ImmunizationRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ImmunizationId { get { var v = this["ImmunizationId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string CVXCode { get { return G("CVXCode"); } set { S("CVXCode", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── OutPatientEncounter ─────────────────────────────────────────────────
        public class OutPatientEncounterDataTable : DataTable
        {
            public OutPatientEncounterDataTable() : base("OutPatientEncounter")
            {
                var id = new DataColumn("OutPatientEncounterId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","EncounterDate","EncounterText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "SNOMED_Description","SNOMEDID","VisitDate","IsFollowUp","IsNewPatient","IsPatientRefferd","TotalVisit" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public OutPatientEncounterRow NewOutPatientEncounterRow() { return (OutPatientEncounterRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new OutPatientEncounterRow(b); }
            protected override Type GetRowType() { return typeof(OutPatientEncounterRow); }
            public DataColumn OutPatientEncounterIdColumn { get { return Columns["OutPatientEncounterId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn EncounterDateColumn { get { return Columns["EncounterDate"]; } }
            public DataColumn EncounterTextColumn { get { return Columns["EncounterText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn SNOMED_DescriptionColumn { get { return Columns["SNOMED_Description"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn VisitDateColumn { get { return Columns["VisitDate"]; } }
            public DataColumn IsFollowUpColumn { get { return Columns["IsFollowUp"]; } }
            public DataColumn IsNewPatientColumn { get { return Columns["IsNewPatient"]; } }
            public DataColumn IsPatientRefferdColumn { get { return Columns["IsPatientRefferd"]; } }
            public DataColumn TotalVisitColumn { get { return Columns["TotalVisit"]; } }
        }
        public class OutPatientEncounterRow : DataRow
        {
            internal OutPatientEncounterRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long OutPatientEncounterId { get { var v = this["OutPatientEncounterId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── AUP_Report ──────────────────────────────────────────────────────────
        public class AUP_ReportDataTable : DataTable
        {
            public AUP_ReportDataTable() : base("AUP_Report")
            {
                var id = new DataColumn("AUP_ReportId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","ReportText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "FacilityOrgId","FacilityName","Location","AntimicrobialDays","RouteIM","RouteIV",
                    "RouteDigestive","RouteRespiratory","RxnormID","Medicine" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AUP_ReportRow NewAUP_ReportRow() { return (AUP_ReportRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AUP_ReportRow(b); }
            protected override Type GetRowType() { return typeof(AUP_ReportRow); }
            public DataColumn AUP_ReportIdColumn { get { return Columns["AUP_ReportId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ReportTextColumn { get { return Columns["ReportText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn FacilityOrgIdColumn { get { return Columns["FacilityOrgId"]; } }
            public DataColumn FacilityNameColumn { get { return Columns["FacilityName"]; } }
            public DataColumn LocationColumn { get { return Columns["Location"]; } }
            public DataColumn AntimicrobialDaysColumn { get { return Columns["AntimicrobialDays"]; } }
            public DataColumn RouteIMColumn { get { return Columns["RouteIM"]; } }
            public DataColumn RouteIVColumn { get { return Columns["RouteIV"]; } }
            public DataColumn RouteDigestiveColumn { get { return Columns["RouteDigestive"]; } }
            public DataColumn RouteRespiratoryColumn { get { return Columns["RouteRespiratory"]; } }
            public DataColumn RxnormIDColumn { get { return Columns["RxnormID"]; } }
            public DataColumn MedicineColumn { get { return Columns["Medicine"]; } }
        }
        public class AUP_ReportRow : DataRow
        {
            internal AUP_ReportRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AUP_ReportId { get { var v = this["AUP_ReportId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ReportText { get { return G("ReportText"); } set { S("ReportText", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── Allergy ─────────────────────────────────────────────────────────────
        public class AllergyDataTable : DataTable
        {
            public AllergyDataTable() : base("Allergy")
            {
                var id = new DataColumn("AllergyId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "Allergen","Type","Reaction","Severity","OnSetDate","LastModified","IsActive","Comments",
                    "NoteId","InActiveCheckBoxValue","InActiveReason","PatientId","ModifiedBy","CreatedBy",
                    "RcopiaID","IsDeleted","RxnormID","RxnormIDType","IsNewRow","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AllergyRow NewAllergyRow() { return (AllergyRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AllergyRow(b); }
            protected override Type GetRowType() { return typeof(AllergyRow); }
            public DataColumn AllergyIdColumn { get { return Columns["AllergyId"]; } }
            public DataColumn AllergenColumn { get { return Columns["Allergen"]; } }
            public DataColumn TypeColumn { get { return Columns["Type"]; } }
            public DataColumn ReactionColumn { get { return Columns["Reaction"]; } }
            public DataColumn SeverityColumn { get { return Columns["Severity"]; } }
            public DataColumn OnSetDateColumn { get { return Columns["OnSetDate"]; } }
            public DataColumn LastModifiedColumn { get { return Columns["LastModified"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn InActiveCheckBoxValueColumn { get { return Columns["InActiveCheckBoxValue"]; } }
            public DataColumn InActiveReasonColumn { get { return Columns["InActiveReason"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn RcopiaIDColumn { get { return Columns["RcopiaID"]; } }
            public DataColumn IsDeletedColumn { get { return Columns["IsDeleted"]; } }
            public DataColumn RxnormIDColumn { get { return Columns["RxnormID"]; } }
            public DataColumn RxnormIDTypeColumn { get { return Columns["RxnormIDType"]; } }
            public DataColumn IsNewRowColumn { get { return Columns["IsNewRow"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class AllergyRow : DataRow
        {
            internal AllergyRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AllergyId { get { var v = this["AllergyId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Allergen { get { return G("Allergen"); } set { S("Allergen", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        // ── Cognitive ───────────────────────────────────────────────────────────
        public class CognitiveDataTable : DataTable
        {
            public CognitiveDataTable() : base("Cognitive")
            {
                var id = new DataColumn("CognitiveId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText","IsAttatchedWithNote" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public CognitiveRow NewCognitiveRow() { return (CognitiveRow)NewRow(); }
            public void AddCognitiveRow(CognitiveRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new CognitiveRow(b); }
            protected override Type GetRowType() { return typeof(CognitiveRow); }
            public DataColumn CognitiveIdColumn { get { return Columns["CognitiveId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn IsAttatchedWithNoteColumn { get { return Columns["IsAttatchedWithNote"]; } }
        }
        public class CognitiveRow : DataRow
        {
            internal CognitiveRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long CognitiveId { get { var v = this["CognitiveId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long   PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public long   NoteId   { get { var v = this["NoteId"];    return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["NoteId"]    = value.ToString(); } }
            public bool   IsActive  { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string CreatedBy  { get { return G("CreatedBy"); }  set { S("CreatedBy",  value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string SoapText   { get { return G("SoapText"); }   set { S("SoapText",   value); } }
        }

        // ── Cognitive_Status ────────────────────────────────────────────────────
        public class Cognitive_StatusDataTable : DataTable
        {
            public Cognitive_StatusDataTable() : base("Cognitive_Status")
            {
                var id = new DataColumn("StatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "CognitiveId","ICD9Code","ICD9CodeDescription","ICD10Code","ICD10CodeDescription",
                    "SNOMEDID","SNOMEDDescription","LexiCode","LexiCodeDescription","Instruction",
                    "Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public Cognitive_StatusRow NewCognitive_StatusRow() { return (Cognitive_StatusRow)NewRow(); }
            public void AddCognitive_StatusRow(Cognitive_StatusRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new Cognitive_StatusRow(b); }
            protected override Type GetRowType() { return typeof(Cognitive_StatusRow); }
            public DataColumn StatusIdColumn { get { return Columns["StatusId"]; } }
            public DataColumn CognitiveIdColumn { get { return Columns["CognitiveId"]; } }
            public DataColumn ICD9CodeColumn { get { return Columns["ICD9Code"]; } }
            public DataColumn ICD9CodeDescriptionColumn { get { return Columns["ICD9CodeDescription"]; } }
            public DataColumn ICD10CodeColumn { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn InstructionColumn { get { return Columns["Instruction"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
        }
        public class Cognitive_StatusRow : DataRow
        {
            internal Cognitive_StatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   StatusId    { get { var v = this["StatusId"];    return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["StatusId"]    = value; } }
            public long   CognitiveId { get { var v = this["CognitiveId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["CognitiveId"] = value.ToString(); } }
            public bool   IsActive    { get { var v = this["IsActive"];    return v != DBNull.Value && Convert.ToBoolean(v); }    set { this["IsActive"]    = value.ToString(); } }
            public string ICD9Code              { get { return G("ICD9Code"); }              set { S("ICD9Code",              value); } }
            public string ICD9CodeDescription   { get { return G("ICD9CodeDescription"); }   set { S("ICD9CodeDescription",   value); } }
            public string ICD10Code             { get { return G("ICD10Code"); }             set { S("ICD10Code",             value); } }
            public string ICD10CodeDescription  { get { return G("ICD10CodeDescription"); }  set { S("ICD10CodeDescription",  value); } }
            public string SNOMEDID              { get { return G("SNOMEDID"); }              set { S("SNOMEDID",              value); } }
            public string SNOMEDDescription     { get { return G("SNOMEDDescription"); }     set { S("SNOMEDDescription",     value); } }
            public string LexiCode              { get { return G("LexiCode"); }              set { S("LexiCode",              value); } }
            public string LexiCodeDescription   { get { return G("LexiCodeDescription"); }   set { S("LexiCodeDescription",   value); } }
            public string Instruction           { get { return G("Instruction"); }           set { S("Instruction",           value); } }
            public string CreatedBy             { get { return G("CreatedBy"); }             set { S("CreatedBy",             value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy            { get { return G("ModifiedBy"); }            set { S("ModifiedBy",            value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── Cognitive_FunctionalStatus ──────────────────────────────────────────
        public class Cognitive_FunctionalStatusDataTable : DataTable
        {
            public Cognitive_FunctionalStatusDataTable() : base("Cognitive_FunctionalStatus")
            {
                var id = new DataColumn("FunctionalStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "CognitiveId","ICD9Code","ICD9CodeDescription","ICD10Code","ICD10CodeDescription",
                    "SNOMEDID","SNOMEDDescription","LexiCode","LexiCodeDescription","Instruction",
                    "Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public Cognitive_FunctionalStatusRow NewCognitive_FunctionalStatusRow() { return (Cognitive_FunctionalStatusRow)NewRow(); }
            public void AddCognitive_FunctionalStatusRow(Cognitive_FunctionalStatusRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new Cognitive_FunctionalStatusRow(b); }
            protected override Type GetRowType() { return typeof(Cognitive_FunctionalStatusRow); }
            public DataColumn FunctionalStatusIdColumn { get { return Columns["FunctionalStatusId"]; } }
            public DataColumn CognitiveIdColumn { get { return Columns["CognitiveId"]; } }
            public DataColumn ICD9CodeColumn { get { return Columns["ICD9Code"]; } }
            public DataColumn ICD9CodeDescriptionColumn { get { return Columns["ICD9CodeDescription"]; } }
            public DataColumn ICD10CodeColumn { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn InstructionColumn { get { return Columns["Instruction"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
        }
        public class Cognitive_FunctionalStatusRow : DataRow
        {
            internal Cognitive_FunctionalStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   FunctionalStatusId { get { var v = this["FunctionalStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["FunctionalStatusId"] = value; } }
            public long   CognitiveId { get { var v = this["CognitiveId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["CognitiveId"] = value.ToString(); } }
            public bool   IsActive    { get { var v = this["IsActive"];    return v != DBNull.Value && Convert.ToBoolean(v); }    set { this["IsActive"]    = value.ToString(); } }
            public string ICD9Code              { get { return G("ICD9Code"); }              set { S("ICD9Code",              value); } }
            public string ICD9CodeDescription   { get { return G("ICD9CodeDescription"); }   set { S("ICD9CodeDescription",   value); } }
            public string ICD10Code             { get { return G("ICD10Code"); }             set { S("ICD10Code",             value); } }
            public string ICD10CodeDescription  { get { return G("ICD10CodeDescription"); }  set { S("ICD10CodeDescription",  value); } }
            public string SNOMEDID              { get { return G("SNOMEDID"); }              set { S("SNOMEDID",              value); } }
            public string SNOMEDDescription     { get { return G("SNOMEDDescription"); }     set { S("SNOMEDDescription",     value); } }
            public string LexiCode              { get { return G("LexiCode"); }              set { S("LexiCode",              value); } }
            public string LexiCodeDescription   { get { return G("LexiCodeDescription"); }   set { S("LexiCodeDescription",   value); } }
            public string Instruction           { get { return G("Instruction"); }           set { S("Instruction",           value); } }
            public string CreatedBy             { get { return G("CreatedBy"); }             set { S("CreatedBy",             value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy            { get { return G("ModifiedBy"); }            set { S("ModifiedBy",            value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── Cognitive_MentalStatus ──────────────────────────────────────────────
        public class Cognitive_MentalStatusDataTable : DataTable
        {
            public Cognitive_MentalStatusDataTable() : base("Cognitive_MentalStatus")
            {
                var id = new DataColumn("MentalStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "CognitiveId","ICD9Code","ICD9CodeDescription","ICD10Code","ICD10CodeDescription",
                    "SNOMEDID","SNOMEDDescription","LexiCode","LexiCodeDescription","Instruction",
                    "Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText","FreeTextICD" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public Cognitive_MentalStatusRow NewCognitive_MentalStatusRow() { return (Cognitive_MentalStatusRow)NewRow(); }
            public void AddCognitive_MentalStatusRow(Cognitive_MentalStatusRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new Cognitive_MentalStatusRow(b); }
            protected override Type GetRowType() { return typeof(Cognitive_MentalStatusRow); }
            public DataColumn MentalStatusIdColumn { get { return Columns["MentalStatusId"]; } }
            public DataColumn CognitiveIdColumn { get { return Columns["CognitiveId"]; } }
            public DataColumn ICD9CodeColumn { get { return Columns["ICD9Code"]; } }
            public DataColumn ICD9CodeDescriptionColumn { get { return Columns["ICD9CodeDescription"]; } }
            public DataColumn ICD10CodeColumn { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn InstructionColumn { get { return Columns["Instruction"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn FreeTextICDColumn { get { return Columns["FreeTextICD"]; } }
        }
        public class Cognitive_MentalStatusRow : DataRow
        {
            internal Cognitive_MentalStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   MentalStatusId { get { var v = this["MentalStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["MentalStatusId"] = value; } }
            public long   CognitiveId { get { var v = this["CognitiveId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["CognitiveId"] = value.ToString(); } }
            public bool   IsActive    { get { var v = this["IsActive"];    return v != DBNull.Value && Convert.ToBoolean(v); }    set { this["IsActive"]    = value.ToString(); } }
            public string ICD9Code              { get { return G("ICD9Code"); }              set { S("ICD9Code",              value); } }
            public string ICD9CodeDescription   { get { return G("ICD9CodeDescription"); }   set { S("ICD9CodeDescription",   value); } }
            public string ICD10Code             { get { return G("ICD10Code"); }             set { S("ICD10Code",             value); } }
            public string ICD10CodeDescription  { get { return G("ICD10CodeDescription"); }  set { S("ICD10CodeDescription",  value); } }
            public string SNOMEDID              { get { return G("SNOMEDID"); }              set { S("SNOMEDID",              value); } }
            public string SNOMEDDescription     { get { return G("SNOMEDDescription"); }     set { S("SNOMEDDescription",     value); } }
            public string LexiCode              { get { return G("LexiCode"); }              set { S("LexiCode",              value); } }
            public string LexiCodeDescription   { get { return G("LexiCodeDescription"); }   set { S("LexiCodeDescription",   value); } }
            public string Instruction           { get { return G("Instruction"); }           set { S("Instruction",           value); } }
            public string FreeTextICD           { get { return G("FreeTextICD"); }           set { S("FreeTextICD",           value); } }
            public string CreatedBy             { get { return G("CreatedBy"); }             set { S("CreatedBy",             value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy            { get { return G("ModifiedBy"); }            set { S("ModifiedBy",            value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── PatientMiscellaneousStatus ──────────────────────────────────────────
        public class PatientMiscellaneousStatusDataTable : DataTable
        {
            public PatientMiscellaneousStatusDataTable() : base("PatientMiscellaneousStatus")
            {
                var id = new DataColumn("PatientMiscellaneousStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PatientId","ReferenceDataId","Value","DocumentName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientMiscellaneousStatusRow NewPatientMiscellaneousStatusRow() { return (PatientMiscellaneousStatusRow)NewRow(); }
            public void AddPatientMiscellaneousStatusRow(PatientMiscellaneousStatusRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientMiscellaneousStatusRow(b); }
            protected override Type GetRowType() { return typeof(PatientMiscellaneousStatusRow); }
            public DataColumn PatientMiscellaneousStatusIdColumn { get { return Columns["PatientMiscellaneousStatusId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ReferenceDataIdColumn { get { return Columns["ReferenceDataId"]; } }
            public DataColumn ValueColumn { get { return Columns["Value"]; } }
            public DataColumn DocumentNameColumn { get { return Columns["DocumentName"]; } }
        }
        public class PatientMiscellaneousStatusRow : DataRow
        {
            internal PatientMiscellaneousStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PatientMiscellaneousStatusId { get { var v = this["PatientMiscellaneousStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientMiscellaneousStatusId"] = value; } }
            public long   PatientId     { get { var v = this["PatientId"];     return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"]     = value.ToString(); } }
            public int    ReferenceDataId { get { var v = this["ReferenceDataId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["ReferenceDataId"] = value.ToString(); } }
            public bool   Value         { get { var v = this["Value"];         return v != DBNull.Value && Convert.ToBoolean(v); }      set { this["Value"]         = value.ToString(); } }
            public string DocumentName  { get { return G("DocumentName"); }    set { S("DocumentName", value); } }
        }

        // ── ProblemList ─────────────────────────────────────────────────────────
        public class ProblemListDataTable : DataTable
        {
            public ProblemListDataTable() : base("ProblemList")
            {
                var id = new DataColumn("ProblemListId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","ProblemName","Description","NoteId","SNOMEDID","SNOMED_DESCRIPTION",
                    "ChronicityLevel","Severity","StartDate","EndDate","InActiveChkBoxValue","InActiveReason",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ProblemListRow NewProblemListRow() { return (ProblemListRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ProblemListRow(b); }
            protected override Type GetRowType() { return typeof(ProblemListRow); }
            public DataColumn ProblemListIdColumn { get { return Columns["ProblemListId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ProblemNameColumn { get { return Columns["ProblemName"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMED_DESCRIPTIONColumn { get { return Columns["SNOMED_DESCRIPTION"]; } }
            public DataColumn ChronicityLevelColumn { get { return Columns["ChronicityLevel"]; } }
            public DataColumn SeverityColumn { get { return Columns["Severity"]; } }
            public DataColumn StartDateColumn { get { return Columns["StartDate"]; } }
            public DataColumn EndDateColumn { get { return Columns["EndDate"]; } }
            public DataColumn InActiveChkBoxValueColumn { get { return Columns["InActiveChkBoxValue"]; } }
            public DataColumn InActiveReasonColumn { get { return Columns["InActiveReason"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class ProblemListRow : DataRow
        {
            internal ProblemListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ProblemListId { get { var v = this["ProblemListId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ProblemName { get { return G("ProblemName"); } set { S("ProblemName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        // ── Stub tables (RecordCount only) ──────────────────────────────────────
        public class ConsultationOrderDataTable : DataTable
        {
            public ConsultationOrderDataTable() : base("ConsultationOrder")
            {
                Columns.Add(new DataColumn("ConsultationOrderId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public DataRow NewConsultationOrderRow() { return NewRow(); }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class LabOrderResultDataTable : DataTable
        {
            public LabOrderResultDataTable() : base("LabOrderResult")
            {
                Columns.Add(new DataColumn("LabOrderResultId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public DataRow NewLabOrderResultRow() { return NewRow(); }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class LabOrderResultDetailDataTable : DataTable
        {
            public LabOrderResultDetailDataTable() : base("LabOrderResultDetail")
            {
                Columns.Add(new DataColumn("LabOrderResultDetailId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public DataRow NewLabOrderResultDetailRow() { return NewRow(); }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class ProceduresDataTable : DataTable
        {
            public ProceduresDataTable() : base("Procedures")
            {
                Columns.Add(new DataColumn("ProcedureId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public DataRow NewProceduresRow() { return NewRow(); }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class ProviderDataTable : DataTable
        {
            public ProviderDataTable() : base("Provider")
            {
                Columns.Add(new DataColumn("ProviderId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                Columns.Add(new DataColumn("RecordCount", typeof(string)) { DefaultValue = "" });
            }
            public DataRow NewProviderRow() { return NewRow(); }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        // ── OccupationStatus ────────────────────────────────────────────────────
        public class OccupationStatusDataTable : DataTable
        {
            public OccupationStatusDataTable() : base("OccupationStatus")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "StatusId","Description","ConceptCode","IsOccupation","RecordCount",
                    "Present","Past","StartDate","EndDate","OccupationDetail",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public OccupationStatusRow NewOccupationStatusRow() { return (OccupationStatusRow)NewRow(); }
            public void AddOccupationStatusRow(OccupationStatusRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new OccupationStatusRow(b); }
            protected override Type GetRowType() { return typeof(OccupationStatusRow); }
            public DataColumn IdColumn          { get { return Columns["Id"]; } }
            public DataColumn StatusIdColumn    { get { return Columns["StatusId"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn ConceptCodeColumn { get { return Columns["ConceptCode"]; } }
            public DataColumn IsOccupationColumn { get { return Columns["IsOccupation"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn PresentColumn     { get { return Columns["Present"]; } }
            public DataColumn PastColumn        { get { return Columns["Past"]; } }
            public DataColumn StartDateColumn   { get { return Columns["StartDate"]; } }
            public DataColumn EndDateColumn     { get { return Columns["EndDate"]; } }
            public DataColumn OccupationDetailColumn { get { return Columns["OccupationDetail"]; } }
            public DataColumn IsActiveColumn    { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn   { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn   { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn  { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn  { get { return Columns["ModifiedOn"]; } }
        }
        public class OccupationStatusRow : DataRow
        {
            internal OccupationStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public int StatusId { get { var v = this["StatusId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["StatusId"] = value.ToString(); } }
            public string Description       { get { return G("Description"); }       set { S("Description", value); } }
            public string ConceptCode       { get { return G("ConceptCode"); }       set { S("ConceptCode", value); } }
            public bool IsOccupation { get { var v = this["IsOccupation"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsOccupation"] = value.ToString(); } }
            public string RecordCount       { get { return G("RecordCount"); }       set { S("RecordCount", value); } }
            public string Present           { get { return G("Present"); }           set { S("Present", value); } }
            public string Past              { get { return G("Past"); }              set { S("Past", value); } }
            public string StartDate         { get { return G("StartDate"); }         set { S("StartDate", value); } }
            public string EndDate           { get { return G("EndDate"); }           set { S("EndDate", value); } }
            public string OccupationDetail  { get { return G("OccupationDetail"); }  set { S("OccupationDetail", value); } }
            public string IsActive          { get { return G("IsActive"); }          set { S("IsActive", value); } }
        }

        // ── EncounterProblemList ────────────────────────────────────────────────
        public class EncounterProblemListDataTable : DataTable
        {
            public EncounterProblemListDataTable() : base("EncounterProblemList")
            {
                var id = new DataColumn("ProblemListId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "SNOMED_DESCRIPTION","StartDate","IsActive","SNOMEDID","NoteDate" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public EncounterProblemListRow NewEncounterProblemListRow() { return (EncounterProblemListRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new EncounterProblemListRow(b); }
            protected override Type GetRowType() { return typeof(EncounterProblemListRow); }
            public DataColumn ProblemListIdColumn       { get { return Columns["ProblemListId"]; } }
            public DataColumn SNOMED_DESCRIPTIONColumn  { get { return Columns["SNOMED_DESCRIPTION"]; } }
            public DataColumn StartDateColumn           { get { return Columns["StartDate"]; } }
            public DataColumn IsActiveColumn            { get { return Columns["IsActive"]; } }
            public DataColumn SNOMEDIDColumn            { get { return Columns["SNOMEDID"]; } }
            public DataColumn NoteDateColumn            { get { return Columns["NoteDate"]; } }
        }
        public class EncounterProblemListRow : DataRow
        {
            internal EncounterProblemListRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ProblemListId { get { var v = this["ProblemListId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string SNOMED_DESCRIPTION { get { return G("SNOMED_DESCRIPTION"); } set { S("SNOMED_DESCRIPTION", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
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
            public DataColumn NameColumn               { get { return Columns["Name"]; } }
            public DataColumn SNOMEDIDColumn           { get { return Columns["SNOMEDID"]; } }
            public DataColumn EffectiveDateColumn      { get { return Columns["EffectiveDate"]; } }
            public DataColumn TypeColumn               { get { return Columns["Type"]; } }
            public DataColumn IsActiveColumn           { get { return Columns["IsActive"]; } }
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
            public DataColumn MentalStatus_CcdaIdColumn  { get { return Columns["MentalStatus_CcdaId"]; } }
            public DataColumn CreatedOnColumn            { get { return Columns["CreatedOn"]; } }
            public DataColumn SNOMEDIDColumn             { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn    { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn InstructionColumn          { get { return Columns["Instruction"]; } }
        }
        public class MentalStatus_CCDARow : DataRow
        {
            internal MentalStatus_CCDARow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MentalStatus_CcdaId { get { var v = this["MentalStatus_CcdaId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Instruction { get { return G("Instruction"); } set { S("Instruction", value); } }
        }

        // ── ARO_FacilitySpecimen ────────────────────────────────────────────────
        public class ARO_FacilitySpecimenDataTable : DataTable
        {
            public ARO_FacilitySpecimenDataTable() : base("ARO_FacilitySpecimen")
            {
                var id = new DataColumn("ARO_FacilitySpecimenId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "FacilityId","FacilityName","Location","SpecimenGroup","DateSpecimenCollected","AROPathogenCategory","SNOMEDID" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ARO_FacilitySpecimenRow NewARO_FacilitySpecimenRow() { return (ARO_FacilitySpecimenRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ARO_FacilitySpecimenRow(b); }
            protected override Type GetRowType() { return typeof(ARO_FacilitySpecimenRow); }
            public DataColumn ARO_FacilitySpecimenIdColumn   { get { return Columns["ARO_FacilitySpecimenId"]; } }
            public DataColumn FacilityIdColumn               { get { return Columns["FacilityId"]; } }
            public DataColumn FacilityNameColumn             { get { return Columns["FacilityName"]; } }
            public DataColumn LocationColumn                 { get { return Columns["Location"]; } }
            public DataColumn SpecimenGroupColumn            { get { return Columns["SpecimenGroup"]; } }
            public DataColumn DateSpecimenCollectedColumn    { get { return Columns["DateSpecimenCollected"]; } }
            public DataColumn AROPathogenCategoryColumn      { get { return Columns["AROPathogenCategory"]; } }
            public DataColumn SNOMEDIDColumn                 { get { return Columns["SNOMEDID"]; } }
        }
        public class ARO_FacilitySpecimenRow : DataRow
        {
            internal ARO_FacilitySpecimenRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ARO_FacilitySpecimenId { get { var v = this["ARO_FacilitySpecimenId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string FacilityId { get { return G("FacilityId"); } set { S("FacilityId", value); } }
        }

        // ── ARO_Antimicrobial ───────────────────────────────────────────────────
        public class ARO_AntimicrobialDataTable : DataTable
        {
            public ARO_AntimicrobialDataTable() : base("ARO_Antimicrobial")
            {
                var id = new DataColumn("ARO_AntimicrobialId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "RxnormID","AntimicrobialId","Brandname","Finalinterpretation" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ARO_AntimicrobialRow NewARO_AntimicrobialRow() { return (ARO_AntimicrobialRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ARO_AntimicrobialRow(b); }
            protected override Type GetRowType() { return typeof(ARO_AntimicrobialRow); }
            public DataColumn ARO_AntimicrobialIdColumn  { get { return Columns["ARO_AntimicrobialId"]; } }
            public DataColumn RxnormIDColumn             { get { return Columns["RxnormID"]; } }
            public DataColumn AntimicrobialIdColumn      { get { return Columns["AntimicrobialId"]; } }
            public DataColumn BrandnameColumn            { get { return Columns["Brandname"]; } }
            public DataColumn FinalinterpretationColumn  { get { return Columns["Finalinterpretation"]; } }
        }
        public class ARO_AntimicrobialRow : DataRow
        {
            internal ARO_AntimicrobialRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ARO_AntimicrobialId { get { var v = this["ARO_AntimicrobialId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string RxnormID { get { return G("RxnormID"); } set { S("RxnormID", value); } }
        }

        // ── ARO_Observations ────────────────────────────────────────────────────
        public class ARO_ObservationsDataTable : DataTable
        {
            public ARO_ObservationsDataTable() : base("ARO_Observations")
            {
                var id = new DataColumn("ARO_ObservationsId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "Result","Flag","UOM","ConditionStatement","LOINC","AntimicrobialId","LOINCDescription" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ARO_ObservationsRow NewARO_ObservationsRow() { return (ARO_ObservationsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ARO_ObservationsRow(b); }
            protected override Type GetRowType() { return typeof(ARO_ObservationsRow); }
            public DataColumn ARO_ObservationsIdColumn   { get { return Columns["ARO_ObservationsId"]; } }
            public DataColumn ResultColumn               { get { return Columns["Result"]; } }
            public DataColumn FlagColumn                 { get { return Columns["Flag"]; } }
            public DataColumn UOMColumn                  { get { return Columns["UOM"]; } }
            public DataColumn ConditionStatementColumn   { get { return Columns["ConditionStatement"]; } }
            public DataColumn LOINCColumn                { get { return Columns["LOINC"]; } }
            public DataColumn AntimicrobialIdColumn      { get { return Columns["AntimicrobialId"]; } }
            public DataColumn LOINCDescriptionColumn     { get { return Columns["LOINCDescription"]; } }
        }
        public class ARO_ObservationsRow : DataRow
        {
            internal ARO_ObservationsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ARO_ObservationsId { get { var v = this["ARO_ObservationsId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Result { get { return G("Result"); } set { S("Result", value); } }
        }

        // ── Languages ───────────────────────────────────────────────────────────
        public class LanguagesDataTable : DataTable
        {
            public LanguagesDataTable() : base("Languages")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "Description","Code" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LanguagesRow NewLanguagesRow() { return (LanguagesRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LanguagesRow(b); }
            protected override Type GetRowType() { return typeof(LanguagesRow); }
            public DataColumn IdColumn          { get { return Columns["Id"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn CodeColumn        { get { return Columns["Code"]; } }
        }
        public class LanguagesRow : DataRow
        {
            internal LanguagesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public string Code { get { return G("Code"); } set { S("Code", value); } }
        }

        // ── Race ────────────────────────────────────────────────────────────────
        public class RaceDataTable : DataTable
        {
            public RaceDataTable() : base("Race")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "Description","RaceParentId","Code" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public RaceRow NewRaceRow() { return (RaceRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new RaceRow(b); }
            protected override Type GetRowType() { return typeof(RaceRow); }
            public DataColumn IdColumn           { get { return Columns["Id"]; } }
            public DataColumn DescriptionColumn  { get { return Columns["Description"]; } }
            public DataColumn RaceParentIdColumn { get { return Columns["RaceParentId"]; } }
            public DataColumn CodeColumn         { get { return Columns["Code"]; } }
        }
        public class RaceRow : DataRow
        {
            internal RaceRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public string Code { get { return G("Code"); } set { S("Code", value); } }
        }

        // ── Ethnicity ───────────────────────────────────────────────────────────
        public class EthnicityDataTable : DataTable
        {
            public EthnicityDataTable() : base("Ethnicity")
            {
                var id = new DataColumn("Id", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "Description","Code" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public EthnicityRow NewEthnicityRow() { return (EthnicityRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new EthnicityRow(b); }
            protected override Type GetRowType() { return typeof(EthnicityRow); }
            public DataColumn IdColumn          { get { return Columns["Id"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn CodeColumn        { get { return Columns["Code"]; } }
        }
        public class EthnicityRow : DataRow
        {
            internal EthnicityRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long Id { get { var v = this["Id"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public string Code { get { return G("Code"); } set { S("Code", value); } }
        }
    }
}
