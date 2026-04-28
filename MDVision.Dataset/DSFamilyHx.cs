using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSFamilyHx : DataSet
    {
        private FamilyHxDataTable _tFamilyHx;
        private FamilyHx_DiseaseDataTable _tFamilyHx_Disease;
        private FamilyHx_FamilyMemberDataTable _tFamilyHx_FamilyMember;
        private FamilyHx_FamilyMemberDetailDataTable _tFamilyHx_FamilyMemberDetail;
        private FamilyHx_FamilyMemberHasDiseaseDataTable _tFamilyHx_FamilyMemberHasDisease;
        private FamilyHx_HealthStatusDataTable _tFamilyHx_HealthStatus;

        public DSFamilyHx()
        {
            DataSetName = "DSFamilyHx";
            _tFamilyHx = new FamilyHxDataTable();
            _tFamilyHx_Disease = new FamilyHx_DiseaseDataTable();
            _tFamilyHx_FamilyMember = new FamilyHx_FamilyMemberDataTable();
            _tFamilyHx_FamilyMemberDetail = new FamilyHx_FamilyMemberDetailDataTable();
            _tFamilyHx_FamilyMemberHasDisease = new FamilyHx_FamilyMemberHasDiseaseDataTable();
            _tFamilyHx_HealthStatus = new FamilyHx_HealthStatusDataTable();
            Tables.Add(_tFamilyHx);
            Tables.Add(_tFamilyHx_Disease);
            Tables.Add(_tFamilyHx_FamilyMember);
            Tables.Add(_tFamilyHx_FamilyMemberDetail);
            Tables.Add(_tFamilyHx_FamilyMemberHasDisease);
            Tables.Add(_tFamilyHx_HealthStatus);
        }

        public FamilyHxDataTable FamilyHx { get { return _tFamilyHx; } }
        public FamilyHx_DiseaseDataTable FamilyHx_Disease { get { return _tFamilyHx_Disease; } }
        public FamilyHx_FamilyMemberDataTable FamilyHx_FamilyMember { get { return _tFamilyHx_FamilyMember; } }
        public FamilyHx_FamilyMemberDetailDataTable FamilyHx_FamilyMemberDetail { get { return _tFamilyHx_FamilyMemberDetail; } }
        public FamilyHx_FamilyMemberHasDiseaseDataTable FamilyHx_FamilyMemberHasDisease { get { return _tFamilyHx_FamilyMemberHasDisease; } }
        public FamilyHx_HealthStatusDataTable FamilyHx_HealthStatus { get { return _tFamilyHx_HealthStatus; } }

        public class FamilyHxDataTable : DataTable
        {
            public FamilyHxDataTable() : base("FamilyHx")
            {
                var id = new DataColumn("FamilyHxId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","FamilyHxDate","bUnremarkable","Comments","IsActive",
                    "CreatedBy","ModifiedBy","CreatedOn","ModifiedOn","NoteId","SoapText" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHxRow NewFamilyHxRow() { return (FamilyHxRow)NewRow(); }
            public void AddFamilyHxRow(FamilyHxRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHxRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHxRow); }

            public DataColumn FamilyHxIdColumn { get { return Columns["FamilyHxId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn FamilyHxDateColumn { get { return Columns["FamilyHxDate"]; } }
            public DataColumn bUnremarkableColumn { get { return Columns["bUnremarkable"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
        }

        public class FamilyHxRow : DataRow
        {
            internal FamilyHxRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long FamilyHxId { get { var v = this["FamilyHxId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public bool bUnremarkable { get { var v = this["bUnremarkable"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["bUnremarkable"] = value.ToString(); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public DateTime FamilyHxDate { get { var v = this["FamilyHxDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["FamilyHxDate"] = value.ToString(); } }
        }

        public class FamilyHx_DiseaseDataTable : DataTable
        {
            public FamilyHx_DiseaseDataTable() : base("FamilyHx_Disease")
            {
                var id = new DataColumn("DiseaseId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "FamilyMemberId","FamilyHxId","LexiCode","LexiCodeDescription","SNOMEDID","SNOMEDDescription",
                    "Comments","IsActive","CreatedBy","ModifiedBy","CreatedOn","ModifiedOn","SoapText",
                    "TempICDID","FreeTextICD","AddedFromMobileApp","ICD9Code","ICD10Code",
                    "ICD9CodeDescription","ICD10CodeDescription" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHx_DiseaseRow NewFamilyHx_DiseaseRow() { return (FamilyHx_DiseaseRow)NewRow(); }
            public void AddFamilyHx_DiseaseRow(FamilyHx_DiseaseRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHx_DiseaseRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHx_DiseaseRow); }
            public System.Collections.Generic.IEnumerator<FamilyHx_DiseaseRow> GetEnumerator() { foreach (DataRow r in Rows) yield return (FamilyHx_DiseaseRow)r; }

            public DataColumn DiseaseIdColumn { get { return Columns["DiseaseId"]; } }
            public DataColumn FamilyMemberIdColumn { get { return Columns["FamilyMemberId"]; } }
            public DataColumn FamilyHxIdColumn { get { return Columns["FamilyHxId"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn TempICDIDColumn { get { return Columns["TempICDID"]; } }
            public DataColumn FreeTextICDColumn { get { return Columns["FreeTextICD"]; } }
            public DataColumn AddedFromMobileAppColumn { get { return Columns["AddedFromMobileApp"]; } }
            public DataColumn ICD9CodeColumn            { get { return Columns["ICD9Code"]; } }
            public DataColumn ICD10CodeColumn           { get { return Columns["ICD10Code"]; } }
            public DataColumn ICD9CodeDescriptionColumn  { get { return Columns["ICD9CodeDescription"]; } }
            public DataColumn ICD10CodeDescriptionColumn { get { return Columns["ICD10CodeDescription"]; } }
        }

        public class FamilyHx_DiseaseRow : DataRow
        {
            internal FamilyHx_DiseaseRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long DiseaseId { get { var v = this["DiseaseId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["DiseaseId"] = value; } }
            public long FamilyMemberId { get { var v = this["FamilyMemberId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["FamilyMemberId"] = value.ToString(); } }
            public long FamilyHxId { get { var v = this["FamilyHxId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["FamilyHxId"] = value.ToString(); } }
            public string LexiCode { get { return G("LexiCode"); } set { S("LexiCode", value); } }
            public string LexiCodeDescription { get { return G("LexiCodeDescription"); } set { S("LexiCodeDescription", value); } }
            public string SNOMEDID { get { return G("SNOMEDID"); } set { S("SNOMEDID", value); } }
            public string SNOMEDDescription { get { return G("SNOMEDDescription"); } set { S("SNOMEDDescription", value); } }
            public string ICD9Code { get { return G("ICD9Code"); } set { S("ICD9Code", value); } }
            public string ICD9CodeDescription { get { return G("ICD9CodeDescription"); } set { S("ICD9CodeDescription", value); } }
            public string ICD10Code { get { return G("ICD10Code"); } set { S("ICD10Code", value); } }
            public string ICD10CodeDescription { get { return G("ICD10CodeDescription"); } set { S("ICD10CodeDescription", value); } }
            public string FreeTextICD { get { return G("FreeTextICD"); } set { S("FreeTextICD", value); } }
            public string AddedFromMobileApp { get { return G("AddedFromMobileApp"); } set { S("AddedFromMobileApp", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class FamilyHx_FamilyMemberDataTable : DataTable
        {
            public FamilyHx_FamilyMemberDataTable() : base("FamilyHx_FamilyMember")
            {
                var id = new DataColumn("MemberId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "FamilyHxId","RelationshipId","MemberName","IsActive","CreatedBy","ModifiedBy","CreatedOn","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHx_FamilyMemberRow NewFamilyHx_FamilyMemberRow() { return (FamilyHx_FamilyMemberRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHx_FamilyMemberRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHx_FamilyMemberRow); }

            public DataColumn MemberIdColumn { get { return Columns["MemberId"]; } }
            public DataColumn FamilyHxIdColumn { get { return Columns["FamilyHxId"]; } }
            public DataColumn RelationshipIdColumn { get { return Columns["RelationshipId"]; } }
            public DataColumn MemberNameColumn { get { return Columns["MemberName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class FamilyHx_FamilyMemberRow : DataRow
        {
            internal FamilyHx_FamilyMemberRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MemberId { get { var v = this["MemberId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string FamilyHxId { get { return G("FamilyHxId"); } set { S("FamilyHxId", value); } }
            public string MemberName { get { return G("MemberName"); } set { S("MemberName", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
        }

        public class FamilyHx_FamilyMemberDetailDataTable : DataTable
        {
            public FamilyHx_FamilyMemberDetailDataTable() : base("FamilyHx_FamilyMemberDetail")
            {
                var id = new DataColumn("MemberDetailId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "MemberId","PatientId","DiseaseId","HealthStatusId","AgeAtDeath","AgeAtDiagnosis",
                    "BirthYear","IsRelativeDied","Comments","IsActive","CreatedBy","ModifiedBy",
                    "CreatedOn","ModifiedOn","SoapText","Relationship","RelationshipSNOMEDId","HealthStatus" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHx_FamilyMemberDetailRow NewFamilyHx_FamilyMemberDetailRow() { return (FamilyHx_FamilyMemberDetailRow)NewRow(); }
            public void AddFamilyHx_FamilyMemberDetailRow(FamilyHx_FamilyMemberDetailRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHx_FamilyMemberDetailRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHx_FamilyMemberDetailRow); }

            public DataColumn MemberDetailIdColumn { get { return Columns["MemberDetailId"]; } }
            public DataColumn MemberIdColumn { get { return Columns["MemberId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn DiseaseIdColumn { get { return Columns["DiseaseId"]; } }
            public DataColumn HealthStatusIdColumn { get { return Columns["HealthStatusId"]; } }
            public DataColumn AgeAtDeathColumn { get { return Columns["AgeAtDeath"]; } }
            public DataColumn AgeAtDiagnosisColumn { get { return Columns["AgeAtDiagnosis"]; } }
            public DataColumn BirthYearColumn { get { return Columns["BirthYear"]; } }
            public DataColumn IsRelativeDiedColumn { get { return Columns["IsRelativeDied"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn RelationshipColumn { get { return Columns["Relationship"]; } }
            public DataColumn RelationshipSNOMEDIdColumn { get { return Columns["RelationshipSNOMEDId"]; } }
            public DataColumn HealthStatusColumn { get { return Columns["HealthStatus"]; } }
        }

        public class FamilyHx_FamilyMemberDetailRow : DataRow
        {
            internal FamilyHx_FamilyMemberDetailRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MemberDetailId { get { var v = this["MemberDetailId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["MemberDetailId"] = value; } }
            public long MemberId { get { var v = this["MemberId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["MemberId"] = value.ToString(); } }
            public long PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public long DiseaseId { get { var v = this["DiseaseId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["DiseaseId"] = value.ToString(); } }
            public int HealthStatusId { get { var v = this["HealthStatusId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["HealthStatusId"] = value.ToString(); } }
            public string BirthYear { get { return G("BirthYear"); } set { S("BirthYear", value); } }
            public string AgeAtDeath { get { return G("AgeAtDeath"); } set { S("AgeAtDeath", value); } }
            public string AgeAtDiagnosis { get { return G("AgeAtDiagnosis"); } set { S("AgeAtDiagnosis", value); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public bool IsRelativeDied { get { var v = this["IsRelativeDied"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsRelativeDied"] = value.ToString(); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class FamilyHx_FamilyMemberHasDiseaseDataTable : DataTable
        {
            public FamilyHx_FamilyMemberHasDiseaseDataTable() : base("FamilyHx_FamilyMemberHasDisease")
            {
                Columns.Add(new DataColumn("MemberHasDiseaseId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "MemberId", "DiseaseId", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHx_FamilyMemberHasDiseaseRow NewFamilyHx_FamilyMemberHasDiseaseRow() { return (FamilyHx_FamilyMemberHasDiseaseRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHx_FamilyMemberHasDiseaseRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHx_FamilyMemberHasDiseaseRow); }

            public DataColumn MemberHasDiseaseIdColumn { get { return Columns["MemberHasDiseaseId"]; } }
            public DataColumn MemberIdColumn { get { return Columns["MemberId"]; } }
            public DataColumn DiseaseIdColumn { get { return Columns["DiseaseId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class FamilyHx_FamilyMemberHasDiseaseRow : DataRow
        {
            internal FamilyHx_FamilyMemberHasDiseaseRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MemberHasDiseaseId { get { var v = this["MemberHasDiseaseId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string MemberId { get { return G("MemberId"); } set { S("MemberId", value); } }
            public string DiseaseId { get { return G("DiseaseId"); } set { S("DiseaseId", value); } }
        }

        public class FamilyHx_HealthStatusDataTable : DataTable
        {
            public FamilyHx_HealthStatusDataTable() : base("FamilyHx_HealthStatus")
            {
                Columns.Add(new DataColumn("HealthStatusId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "StatusName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FamilyHx_HealthStatusRow NewFamilyHx_HealthStatusRow() { return (FamilyHx_HealthStatusRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FamilyHx_HealthStatusRow(b); }
            protected override Type GetRowType() { return typeof(FamilyHx_HealthStatusRow); }

            public DataColumn HealthStatusIdColumn { get { return Columns["HealthStatusId"]; } }
            public DataColumn StatusNameColumn { get { return Columns["StatusName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class FamilyHx_HealthStatusRow : DataRow
        {
            internal FamilyHx_HealthStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long HealthStatusId { get { var v = this["HealthStatusId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string StatusName { get { return G("StatusName"); } set { S("StatusName", value); } }
        }
    }
}
