using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSClinicalNoteTemplate : DataSet
    {
        private NotesTemplateDataTable _tNotesTemplate;
        private NotesTemplateLookupDataTable _tNotesTemplateLookup;
        private NotesTemplateTypeDataTable _tNotesTemplateType;
        private NotesTagCategoryDataTable _tNotesTagCategory;
        private NotesTagNameDataTable _tNotesTagName;

        public DSClinicalNoteTemplate()
        {
            DataSetName = "DSClinicalNoteTemplate";
            _tNotesTemplate = new NotesTemplateDataTable();
            _tNotesTemplateLookup = new NotesTemplateLookupDataTable();
            _tNotesTemplateType = new NotesTemplateTypeDataTable();
            _tNotesTagCategory = new NotesTagCategoryDataTable();
            _tNotesTagName = new NotesTagNameDataTable();
            Tables.Add(_tNotesTemplate);
            Tables.Add(_tNotesTemplateLookup);
            Tables.Add(_tNotesTemplateType);
            Tables.Add(_tNotesTagCategory);
            Tables.Add(_tNotesTagName);
        }

        public NotesTemplateDataTable NotesTemplate { get { return _tNotesTemplate; } }
        public NotesTemplateLookupDataTable NotesTemplateLookup { get { return _tNotesTemplateLookup; } }
        public NotesTemplateTypeDataTable NotesTemplateType { get { return _tNotesTemplateType; } }
        public NotesTagCategoryDataTable NotesTagCategory { get { return _tNotesTagCategory; } }
        public NotesTagNameDataTable NotesTagName { get { return _tNotesTagName; } }

        public class NotesTemplateDataTable : DataTable
        {
            public NotesTemplateDataTable() : base("NotesTemplate")
            {
                var id = new DataColumn("NotesTemplateId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "TemplateName","TemplateTypeId","EntityId","ProviderId","TemplateContent",
                    "IsActive","IsDefault","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","RecordCount",
                    "HPITemplateId","HTMLTemplate","NotesTagNameIds","NoteTemplateName","OrderSetId",
                    "PEDataTemptId","ProviderIds","ROSDataTemptId","SpecialtyIds",
                    "SpecialtyNames","TemplateTypeName","CreatedByName","ModifiedByName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public NotesTemplateRow NewNotesTemplateRow() { return (NotesTemplateRow)NewRow(); }
            public void AddNotesTemplateRow(NotesTemplateRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new NotesTemplateRow(b); }
            protected override Type GetRowType() { return typeof(NotesTemplateRow); }

            public DataColumn NotesTemplateIdColumn { get { return Columns["NotesTemplateId"]; } }
            public DataColumn TemplateNameColumn { get { return Columns["TemplateName"]; } }
            public DataColumn TemplateTypeIdColumn { get { return Columns["TemplateTypeId"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn TemplateContentColumn { get { return Columns["TemplateContent"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn IsDefaultColumn { get { return Columns["IsDefault"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn RecordCountColumn    { get { return Columns["RecordCount"]; } }
            public DataColumn HPITemplateIdColumn   { get { return Columns["HPITemplateId"]; } }
            public DataColumn HTMLTemplateColumn    { get { return Columns["HTMLTemplate"]; } }
            public DataColumn NotesTagNameIdsColumn { get { return Columns["NotesTagNameIds"]; } }
            public DataColumn NoteTemplateNameColumn { get { return Columns["NoteTemplateName"]; } }
            public DataColumn OrderSetIdColumn      { get { return Columns["OrderSetId"]; } }
            public DataColumn PEDataTemptIdColumn   { get { return Columns["PEDataTemptId"]; } }
            public DataColumn ProviderIdsColumn     { get { return Columns["ProviderIds"]; } }
            public DataColumn ROSDataTemptIdColumn  { get { return Columns["ROSDataTemptId"]; } }
            public DataColumn SpecialtyIdsColumn    { get { return Columns["SpecialtyIds"]; } }
            public DataColumn SpecialtyNamesColumn   { get { return Columns["SpecialtyNames"]; } }
            public DataColumn TemplateTypeNameColumn { get { return Columns["TemplateTypeName"]; } }
            public DataColumn CreatedByNameColumn    { get { return Columns["CreatedByName"]; } }
            public DataColumn ModifiedByNameColumn   { get { return Columns["ModifiedByName"]; } }
        }

        public class NotesTemplateRow : DataRow
        {
            internal NotesTemplateRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long NotesTemplateId { get { var v = this["NotesTemplateId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["NotesTemplateId"] = value; } }
            public string TemplateName { get { return G("TemplateName"); } set { S("TemplateName", value); } }
            public string NoteTemplateName { get { return G("NoteTemplateName"); } set { S("NoteTemplateName", value); } }
            public int TemplateTypeId { get { var v = this["TemplateTypeId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["TemplateTypeId"] = value.ToString(); } }
            public string SpecialtyIds { get { return G("SpecialtyIds"); } set { S("SpecialtyIds", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string ProviderIds { get { return G("ProviderIds"); } set { S("ProviderIds", value); } }
            public long ROSDataTemptId { get { var v = this["ROSDataTemptId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ROSDataTemptId"] = value.ToString(); } }
            public long PEDataTemptId { get { var v = this["PEDataTemptId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PEDataTemptId"] = value.ToString(); } }
            public long HPITemplateId { get { var v = this["HPITemplateId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["HPITemplateId"] = value.ToString(); } }
            public int EntityId { get { var v = this["EntityId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["EntityId"] = value.ToString(); } }
            public string NotesTagNameIds { get { return G("NotesTagNameIds"); } set { S("NotesTagNameIds", value); } }
            public string HTMLTemplate { get { return G("HTMLTemplate"); } set { S("HTMLTemplate", value); } }
            public string TemplateContent { get { return G("TemplateContent"); } set { S("TemplateContent", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class NotesTemplateLookupDataTable : DataTable
        {
            public NotesTemplateLookupDataTable() : base("NotesTemplateLookup")
            {
                Columns.Add(new DataColumn("NotesTemplateLookupId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "LookupName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public NotesTemplateLookupRow NewNotesTemplateLookupRow() { return (NotesTemplateLookupRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new NotesTemplateLookupRow(b); }
            protected override Type GetRowType() { return typeof(NotesTemplateLookupRow); }

            public DataColumn NotesTemplateLookupIdColumn { get { return Columns["NotesTemplateLookupId"]; } }
            public DataColumn LookupNameColumn { get { return Columns["LookupName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class NotesTemplateLookupRow : DataRow
        {
            internal NotesTemplateLookupRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long NotesTemplateLookupId { get { var v = this["NotesTemplateLookupId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string LookupName { get { return G("LookupName"); } set { S("LookupName", value); } }
        }

        public class NotesTemplateTypeDataTable : DataTable
        {
            public NotesTemplateTypeDataTable() : base("NotesTemplateType")
            {
                Columns.Add(new DataColumn("TemplateTypeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "TypeName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public NotesTemplateTypeRow NewNotesTemplateTypeRow() { return (NotesTemplateTypeRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new NotesTemplateTypeRow(b); }
            protected override Type GetRowType() { return typeof(NotesTemplateTypeRow); }

            public DataColumn TemplateTypeIdColumn { get { return Columns["TemplateTypeId"]; } }
            public DataColumn TypeNameColumn { get { return Columns["TypeName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class NotesTemplateTypeRow : DataRow
        {
            internal NotesTemplateTypeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long TemplateTypeId { get { var v = this["TemplateTypeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TypeName { get { return G("TypeName"); } set { S("TypeName", value); } }
        }

        public class NotesTagCategoryDataTable : DataTable
        {
            public NotesTagCategoryDataTable() : base("NotesTagCategory")
            {
                Columns.Add(new DataColumn("NotesTagCategoryId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "CategoryName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public NotesTagCategoryRow NewNotesTagCategoryRow() { return (NotesTagCategoryRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new NotesTagCategoryRow(b); }
            protected override Type GetRowType() { return typeof(NotesTagCategoryRow); }

            public DataColumn NotesTagCategoryIdColumn { get { return Columns["NotesTagCategoryId"]; } }
            public DataColumn CategoryNameColumn { get { return Columns["CategoryName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class NotesTagCategoryRow : DataRow
        {
            internal NotesTagCategoryRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long NotesTagCategoryId { get { var v = this["NotesTagCategoryId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string CategoryName { get { return G("CategoryName"); } set { S("CategoryName", value); } }
        }

        public class NotesTagNameDataTable : DataTable
        {
            public NotesTagNameDataTable() : base("NotesTagName")
            {
                Columns.Add(new DataColumn("NotesTagNameId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "TagName", "NotesTagCategoryId", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public NotesTagNameRow NewNotesTagNameRow() { return (NotesTagNameRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new NotesTagNameRow(b); }
            protected override Type GetRowType() { return typeof(NotesTagNameRow); }

            public DataColumn NotesTagNameIdColumn { get { return Columns["NotesTagNameId"]; } }
            public DataColumn TagNameColumn { get { return Columns["TagName"]; } }
            public DataColumn NotesTagCategoryIdColumn { get { return Columns["NotesTagCategoryId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class NotesTagNameRow : DataRow
        {
            internal NotesTagNameRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long NotesTagNameId { get { var v = this["NotesTagNameId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TagName { get { return G("TagName"); } set { S("TagName", value); } }
            public string NotesTagCategoryId { get { return G("NotesTagCategoryId"); } set { S("NotesTagCategoryId", value); } }
        }
    }
}
