using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSClinical : DataSet
    {
        private ClinicalMenuSettingsDataTable _tClinicalMenuSettings;
        private CognitiveDataTable _tCognitive;
        private ComplaintDataTable _tComplaint;
        private DrugDataTable _tDrug;

        public DSClinical()
        {
            DataSetName = "DSClinical";
            _tClinicalMenuSettings = new ClinicalMenuSettingsDataTable();
            _tCognitive = new CognitiveDataTable();
            _tComplaint = new ComplaintDataTable();
            _tDrug = new DrugDataTable();
            Tables.Add(_tClinicalMenuSettings);
            Tables.Add(_tCognitive);
            Tables.Add(_tComplaint);
            Tables.Add(_tDrug);
        }

        public ClinicalMenuSettingsDataTable ClinicalMenuSettings { get { return _tClinicalMenuSettings; } }
        public CognitiveDataTable Cognitive { get { return _tCognitive; } }
        public ComplaintDataTable Complaint { get { return _tComplaint; } }
        public DrugDataTable Drug { get { return _tDrug; } }

        public class ClinicalMenuSettingsDataTable : DataTable
        {
            public ClinicalMenuSettingsDataTable() : base("ClinicalMenuSettings")
            {
                var id = new DataColumn("ClinicalMenuSettingId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "MenuName","IsVisible","DisplayOrder","EntityId","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "ClinicalMenuSettingsId","UserId","ClinicalMenuHTML" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ClinicalMenuSettingsRow NewClinicalMenuSettingsRow() { return (ClinicalMenuSettingsRow)NewRow(); }
            public void AddClinicalMenuSettingsRow(ClinicalMenuSettingsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ClinicalMenuSettingsRow(b); }
            protected override Type GetRowType() { return typeof(ClinicalMenuSettingsRow); }

            public DataColumn ClinicalMenuSettingIdColumn { get { return Columns["ClinicalMenuSettingId"]; } }
            public DataColumn MenuNameColumn { get { return Columns["MenuName"]; } }
            public DataColumn IsVisibleColumn { get { return Columns["IsVisible"]; } }
            public DataColumn DisplayOrderColumn { get { return Columns["DisplayOrder"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ClinicalMenuSettingsIdColumn { get { return Columns["ClinicalMenuSettingsId"]; } }
            public DataColumn UserIdColumn       { get { return Columns["UserId"]; } }
            public DataColumn ClinicalMenuHTMLColumn { get { return Columns["ClinicalMenuHTML"]; } }
        }

        public class ClinicalMenuSettingsRow : DataRow
        {
            internal ClinicalMenuSettingsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   ClinicalMenuSettingId  { get { var v = this["ClinicalMenuSettingId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long   ClinicalMenuSettingsId { get { var v = this["ClinicalMenuSettingsId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ClinicalMenuSettingsId"] = value.ToString(); } }
            public long   UserId                 { get { var v = this["UserId"];                 return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["UserId"]                 = value.ToString(); } }
            public string MenuName               { get { return G("MenuName"); }                 set { S("MenuName",        value); } }
            public string IsVisible              { get { return G("IsVisible"); }                set { S("IsVisible",       value); } }
            public string DisplayOrder           { get { return G("DisplayOrder"); }             set { S("DisplayOrder",    value); } }
            public string EntityId               { get { return G("EntityId"); }                 set { S("EntityId",        value); } }
            public string IsActive               { get { return G("IsActive"); }                 set { S("IsActive",        value); } }
            public string CreatedBy              { get { return G("CreatedBy"); }                set { S("CreatedBy",       value); } }
            public DateTime CreatedOn            { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy             { get { return G("ModifiedBy"); }               set { S("ModifiedBy",      value); } }
            public DateTime ModifiedOn           { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string ClinicalMenuHTML       { get { return G("ClinicalMenuHTML"); }         set { S("ClinicalMenuHTML", value); } }
        }

        public class CognitiveDataTable : DataTable
        {
            public CognitiveDataTable() : base("Cognitive")
            {
                var id = new DataColumn("CognitiveId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","CognitiveText","SoapText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public CognitiveRow NewCognitiveRow() { return (CognitiveRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new CognitiveRow(b); }
            protected override Type GetRowType() { return typeof(CognitiveRow); }

            public DataColumn CognitiveIdColumn { get { return Columns["CognitiveId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn CognitiveTextColumn { get { return Columns["CognitiveText"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class CognitiveRow : DataRow
        {
            internal CognitiveRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long CognitiveId { get { var v = this["CognitiveId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string CognitiveText { get { return G("CognitiveText"); } set { S("CognitiveText", value); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class ComplaintDataTable : DataTable
        {
            public ComplaintDataTable() : base("Complaint")
            {
                var id = new DataColumn("ComplaintId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","NoteId","ComplaintText","SoapText","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ComplaintRow NewComplaintRow() { return (ComplaintRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ComplaintRow(b); }
            protected override Type GetRowType() { return typeof(ComplaintRow); }

            public DataColumn ComplaintIdColumn { get { return Columns["ComplaintId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn ComplaintTextColumn { get { return Columns["ComplaintText"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class ComplaintRow : DataRow
        {
            internal ComplaintRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ComplaintId { get { var v = this["ComplaintId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string NoteId { get { return G("NoteId"); } set { S("NoteId", value); } }
            public string ComplaintText { get { return G("ComplaintText"); } set { S("ComplaintText", value); } }
            public string SoapText { get { return G("SoapText"); } set { S("SoapText", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class DrugDataTable : DataTable
        {
            public DrugDataTable() : base("Drug")
            {
                var id = new DataColumn("DrugId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "DrugName","NDC","RxnormID","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public DrugRow NewDrugRow() { return (DrugRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new DrugRow(b); }
            protected override Type GetRowType() { return typeof(DrugRow); }

            public DataColumn DrugIdColumn { get { return Columns["DrugId"]; } }
            public DataColumn DrugNameColumn { get { return Columns["DrugName"]; } }
            public DataColumn NDCColumn { get { return Columns["NDC"]; } }
            public DataColumn RxnormIDColumn { get { return Columns["RxnormID"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class DrugRow : DataRow
        {
            internal DrugRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long DrugId { get { var v = this["DrugId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string DrugName { get { return G("DrugName"); } set { S("DrugName", value); } }
            public string NDC { get { return G("NDC"); } set { S("NDC", value); } }
            public string RxnormID { get { return G("RxnormID"); } set { S("RxnormID", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }
    }
}
