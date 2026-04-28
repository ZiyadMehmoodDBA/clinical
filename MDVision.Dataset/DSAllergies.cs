using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSAllergies : DataSet
    {
        private AllergyDataTable _tAllergy;
        private AllergyReviewDataTable _tAllergyReview;
        private AllergySoapDataTable _tAllergySoap;
        private AllergyHistoryDataTable _tAllergyHistory;

        public DSAllergies()
        {
            DataSetName = "DSAllergies";
            _tAllergy = new AllergyDataTable();
            _tAllergyReview = new AllergyReviewDataTable();
            _tAllergySoap = new AllergySoapDataTable();
            _tAllergyHistory = new AllergyHistoryDataTable();
            Tables.Add(_tAllergy);
            Tables.Add(_tAllergyReview);
            Tables.Add(_tAllergySoap);
            Tables.Add(_tAllergyHistory);
        }

        public AllergyDataTable Allergy { get { return _tAllergy; } }
        public AllergyReviewDataTable AllergyReview { get { return _tAllergyReview; } }
        public AllergySoapDataTable AllergySoap { get { return _tAllergySoap; } }
        public AllergyHistoryDataTable AllergyHistory { get { return _tAllergyHistory; } }

        public class AllergyDataTable : DataTable
        {
            public AllergyDataTable() : base("Allergy")
            {
                var id = new DataColumn("AllergyId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "Allergen","Type","Reaction","Severity","OnSetDate","LastModified","IsActive","Comments",
                    "NoteId","InActiveCheckBoxValue","InActiveReason","PatientId","ModifiedBy","CreatedBy",
                    "RcopiaID","IsDeleted","RxnormID","RxnormIDType","IsNewRow","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AllergyRow NewAllergyRow() { return (AllergyRow)NewRow(); }
            public void AddAllergyRow(AllergyRow row) { Rows.Add(row); }
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
            public string Type { get { return G("Type"); } set { S("Type", value); } }
            public string Reaction { get { return G("Reaction"); } set { S("Reaction", value); } }
            public string Severity { get { return G("Severity"); } set { S("Severity", value); } }
            public DateTime OnSetDate { get { var v = this["OnSetDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["OnSetDate"] = value.ToString(); } }
            public string LastModified { get { return G("LastModified"); } set { S("LastModified", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string InActiveCheckBoxValue { get { return G("InActiveCheckBoxValue"); } set { S("InActiveCheckBoxValue", value); } }
            public string InActiveReason { get { return G("InActiveReason"); } set { S("InActiveReason", value); } }
            public long PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string RcopiaID { get { return G("RcopiaID"); } set { S("RcopiaID", value); } }
            public bool IsDeleted { get { var v = this["IsDeleted"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsDeleted"] = value.ToString(); } }
            public string RxnormID { get { return G("RxnormID"); } set { S("RxnormID", value); } }
            public string RxnormIDType { get { return G("RxnormIDType"); } set { S("RxnormIDType", value); } }
            public string IsNewRow { get { return G("IsNewRow"); } set { S("IsNewRow", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class AllergyReviewDataTable : DataTable
        {
            public AllergyReviewDataTable() : base("AllergyReview")
            {
                var id = new DataColumn("AllergyReviewID", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "PatientId", "ReviewedBy", "ReviewedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AllergyReviewRow NewAllergyReviewRow() { return (AllergyReviewRow)NewRow(); }
            public void AddAllergyReviewRow(AllergyReviewRow row) { Rows.Add(row); }
            public int Count { get { return Rows.Count; } }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AllergyReviewRow(b); }
            protected override Type GetRowType() { return typeof(AllergyReviewRow); }

            public DataColumn AllergyReviewIDColumn { get { return Columns["AllergyReviewID"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ReviewedByColumn { get { return Columns["ReviewedBy"]; } }
            public DataColumn ReviewedOnColumn { get { return Columns["ReviewedOn"]; } }
        }

        public class AllergyReviewRow : DataRow
        {
            internal AllergyReviewRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AllergyReviewID { get { var v = this["AllergyReviewID"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["AllergyReviewID"] = value; } }
            public long PatientId { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public string ReviewedBy { get { return G("ReviewedBy"); } set { S("ReviewedBy", value); } }
            public DateTime ReviewedOn { get { var v = this["ReviewedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ReviewedOn"] = value.ToString(); } }
        }

        public class AllergySoapDataTable : DataTable
        {
            public AllergySoapDataTable() : base("AllergySoap")
            {
                var id = new DataColumn("AllergySoapId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "AllergyId", "NoteId", "SoapText", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AllergySoapRow NewAllergySoapRow() { return (AllergySoapRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AllergySoapRow(b); }
            protected override Type GetRowType() { return typeof(AllergySoapRow); }

            public DataColumn AllergySoapIdColumn { get { return Columns["AllergySoapId"]; } }
            public DataColumn AllergyIdColumn { get { return Columns["AllergyId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class AllergySoapRow : DataRow
        {
            internal AllergySoapRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AllergySoapId { get { var v = this["AllergySoapId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AllergyId { get { return G("AllergyId"); } set { S("AllergyId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public string CreatedOn { get { return G("CreatedOn"); } set { S("CreatedOn", value); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public string ModifiedOn { get { return G("ModifiedOn"); } set { S("ModifiedOn", value); } }
        }

        public class AllergyHistoryDataTable : DataTable
        {
            public AllergyHistoryDataTable() : base("AllergyHistory")
            {
                var id = new DataColumn("AllergyHistoryId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "AllergyId", "PatientId", "ChangeType", "ChangedBy", "ChangedOn", "Comments" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public AllergyHistoryRow NewAllergyHistoryRow() { return (AllergyHistoryRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new AllergyHistoryRow(b); }
            protected override Type GetRowType() { return typeof(AllergyHistoryRow); }

            public DataColumn AllergyHistoryIdColumn { get { return Columns["AllergyHistoryId"]; } }
            public DataColumn AllergyIdColumn { get { return Columns["AllergyId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ChangeTypeColumn { get { return Columns["ChangeType"]; } }
            public DataColumn ChangedByColumn { get { return Columns["ChangedBy"]; } }
            public DataColumn ChangedOnColumn { get { return Columns["ChangedOn"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
        }

        public class AllergyHistoryRow : DataRow
        {
            internal AllergyHistoryRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AllergyHistoryId { get { var v = this["AllergyHistoryId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string AllergyId { get { return G("AllergyId"); } set { S("AllergyId", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string ChangeType { get { return G("ChangeType"); } set { S("ChangeType", value); } }
            public string ChangedBy { get { return G("ChangedBy"); } set { S("ChangedBy", value); } }
            public string ChangedOn { get { return G("ChangedOn"); } set { S("ChangedOn", value); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
        }
    }
}
