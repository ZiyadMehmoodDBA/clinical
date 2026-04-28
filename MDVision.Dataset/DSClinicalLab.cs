using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSClinicalLab : DataSet
    {
        private LabDataTable _tLab;
        private LabTestDataTable _tLabTest;
        private LabTestAttributesDataTable _tLabTestAttributes;
        private LabTestAttributeResultDataTable _tLabTestAttributeResult;
        private LabLookupDataTable _tLabLookup;
        private LabCategoryDataTable _tLabCategory;
        private LabTypeDataTable _tLabType;
        private LabCodeSystemDataTable _tLabCodeSystem;
        private LabRequisitionTemplateDataTable _tLabRequisitionTemplate;

        public DSClinicalLab()
        {
            DataSetName = "DSClinicalLab";
            _tLab = new LabDataTable();
            _tLabTest = new LabTestDataTable();
            _tLabTestAttributes = new LabTestAttributesDataTable();
            _tLabTestAttributeResult = new LabTestAttributeResultDataTable();
            _tLabLookup = new LabLookupDataTable();
            _tLabCategory = new LabCategoryDataTable();
            _tLabType = new LabTypeDataTable();
            _tLabCodeSystem = new LabCodeSystemDataTable();
            _tLabRequisitionTemplate = new LabRequisitionTemplateDataTable();
            Tables.Add(_tLab);
            Tables.Add(_tLabTest);
            Tables.Add(_tLabTestAttributes);
            Tables.Add(_tLabTestAttributeResult);
            Tables.Add(_tLabLookup);
            Tables.Add(_tLabCategory);
            Tables.Add(_tLabType);
            Tables.Add(_tLabCodeSystem);
            Tables.Add(_tLabRequisitionTemplate);
        }

        public LabDataTable Lab { get { return _tLab; } }
        public LabTestDataTable LabTest { get { return _tLabTest; } }
        public LabTestAttributesDataTable LabTestAttributes { get { return _tLabTestAttributes; } }
        public LabTestAttributeResultDataTable LabTestAttributeResult { get { return _tLabTestAttributeResult; } }
        public LabLookupDataTable LabLookup { get { return _tLabLookup; } }
        public LabCategoryDataTable LabCategory { get { return _tLabCategory; } }
        public LabTypeDataTable LabType { get { return _tLabType; } }
        public LabCodeSystemDataTable LabCodeSystem { get { return _tLabCodeSystem; } }
        public LabRequisitionTemplateDataTable LabRequisitionTemplate { get { return _tLabRequisitionTemplate; } }

        public class LabDataTable : DataTable
        {
            public LabDataTable() : base("Lab")
            {
                var id = new DataColumn("LabId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "Name","Type","EntityId","Comments","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "ClientNo","CategoryId","CodeSystemId","RequisitionTemplateId","LabTypeId","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabRow NewLabRow() { return (LabRow)NewRow(); }
            public void AddLabRow(LabRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabRow(b); }
            protected override Type GetRowType() { return typeof(LabRow); }

            public DataColumn LabIdColumn { get { return Columns["LabId"]; } }
            public DataColumn NameColumn { get { return Columns["Name"]; } }
            public DataColumn TypeColumn { get { return Columns["Type"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ClientNoColumn { get { return Columns["ClientNo"]; } }
            public DataColumn CategoryIdColumn { get { return Columns["CategoryId"]; } }
            public DataColumn CodeSystemIdColumn { get { return Columns["CodeSystemId"]; } }
            public DataColumn RequisitionTemplateIdColumn { get { return Columns["RequisitionTemplateId"]; } }
            public DataColumn LabTypeIdColumn { get { return Columns["LabTypeId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class LabRow : DataRow
        {
            internal LabRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabId { get { var v = this["LabId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["LabId"] = value; } }
            public string Name { get { return G("Name"); } set { S("Name", value); } }
            public string Type { get { return G("Type"); } set { S("Type", value); } }
            public long EntityId { get { var v = this["EntityId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["EntityId"] = value.ToString(); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
            public long CategoryId { get { var v = this["CategoryId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["CategoryId"] = value.ToString(); } }
            public string ClientNo { get { return G("ClientNo"); } set { S("ClientNo", value); } }
            public long CodeSystemId { get { var v = this["CodeSystemId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["CodeSystemId"] = value.ToString(); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public long LabTypeId { get { var v = this["LabTypeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["LabTypeId"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public long RequisitionTemplateId { get { var v = this["RequisitionTemplateId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RequisitionTemplateId"] = value.ToString(); } }
        }

        public class LabTestDataTable : DataTable
        {
            public LabTestDataTable() : base("LabTest")
            {
                var id = new DataColumn("LabTestId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "LabId","LOINC","LOINCDescription","IsTemplate","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabTestRow NewLabTestRow() { return (LabTestRow)NewRow(); }
            public void AddLabTestRow(LabTestRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabTestRow(b); }
            protected override Type GetRowType() { return typeof(LabTestRow); }

            public DataColumn LabTestIdColumn { get { return Columns["LabTestId"]; } }
            public DataColumn LabIdColumn { get { return Columns["LabId"]; } }
            public DataColumn LOINCColumn { get { return Columns["LOINC"]; } }
            public DataColumn LOINCDescriptionColumn { get { return Columns["LOINCDescription"]; } }
            public DataColumn IsTemplateColumn { get { return Columns["IsTemplate"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class LabTestRow : DataRow
        {
            internal LabTestRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabTestId { get { var v = this["LabTestId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public int LabId { get { var v = this["LabId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["LabId"] = value.ToString(); } }
            public string LOINC { get { return G("LOINC"); } set { S("LOINC", value); } }
            public string LOINCDescription { get { return G("LOINCDescription"); } set { S("LOINCDescription", value); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public bool IsTemplate { get { var v = this["IsTemplate"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsTemplate"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class LabTestAttributesDataTable : DataTable
        {
            public LabTestAttributesDataTable() : base("LabTestAttributes")
            {
                var id = new DataColumn("LabTestAttributeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "LabTestId","AttributeName","UoM","Range","Description","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabTestAttributesRow NewLabTestAttributesRow() { return (LabTestAttributesRow)NewRow(); }
            public void AddLabTestAttributesRow(LabTestAttributesRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabTestAttributesRow(b); }
            protected override Type GetRowType() { return typeof(LabTestAttributesRow); }

            public DataColumn LabTestAttributeIdColumn { get { return Columns["LabTestAttributeId"]; } }
            public DataColumn LabTestIdColumn { get { return Columns["LabTestId"]; } }
            public DataColumn AttributeNameColumn { get { return Columns["AttributeName"]; } }
            public DataColumn UoMColumn { get { return Columns["UoM"]; } }
            public DataColumn RangeColumn { get { return Columns["Range"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class LabTestAttributesRow : DataRow
        {
            internal LabTestAttributesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabTestAttributeId { get { var v = this["LabTestAttributeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long LabTestId { get { var v = this["LabTestId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["LabTestId"] = value.ToString(); } }
            public string AttributeName { get { return G("AttributeName"); } set { S("AttributeName", value); } }
            public string UoM { get { return G("UoM"); } set { S("UoM", value); } }
            public string Range { get { return G("Range"); } set { S("Range", value); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public bool IsActive { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
        }

        public class LabTestAttributeResultDataTable : DataTable
        {
            public LabTestAttributeResultDataTable() : base("LabTestAttributeResult")
            {
                var id = new DataColumn("LabTestAttributeResultId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "LabTestAttributeId", "ResultName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabTestAttributeResultRow NewLabTestAttributeResultRow() { return (LabTestAttributeResultRow)NewRow(); }
            public void AddLabTestAttributeResultRow(LabTestAttributeResultRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabTestAttributeResultRow(b); }
            protected override Type GetRowType() { return typeof(LabTestAttributeResultRow); }

            public DataColumn LabTestAttributeResultIdColumn { get { return Columns["LabTestAttributeResultId"]; } }
            public DataColumn LabTestAttributeIdColumn { get { return Columns["LabTestAttributeId"]; } }
            public DataColumn ResultNameColumn { get { return Columns["ResultName"]; } }
        }

        public class LabTestAttributeResultRow : DataRow
        {
            internal LabTestAttributeResultRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabTestAttributeResultId { get { var v = this["LabTestAttributeResultId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long LabTestAttributeId { get { var v = this["LabTestAttributeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["LabTestAttributeId"] = value.ToString(); } }
            public string ResultName { get { return G("ResultName"); } set { S("ResultName", value); } }
        }

        public class LabLookupDataTable : DataTable
        {
            public LabLookupDataTable() : base("LabLookup")
            {
                Columns.Add(new DataColumn("LabLookupId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "LookupName", "IsActive", "LabId", "Name" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabLookupRow NewLabLookupRow() { return (LabLookupRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabLookupRow(b); }
            protected override Type GetRowType() { return typeof(LabLookupRow); }

            public DataColumn LabLookupIdColumn { get { return Columns["LabLookupId"]; } }
            public DataColumn LookupNameColumn { get { return Columns["LookupName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn LabIdColumn { get { return Columns["LabId"]; } }
            public DataColumn NameColumn { get { return Columns["Name"]; } }
        }

        public class LabLookupRow : DataRow
        {
            internal LabLookupRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabLookupId { get { var v = this["LabLookupId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string LookupName { get { return G("LookupName"); } set { S("LookupName", value); } }
        }

        public class LabCategoryDataTable : DataTable
        {
            public LabCategoryDataTable() : base("LabCategory")
            {
                Columns.Add(new DataColumn("LabCategoryId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "CategoryName", "IsActive", "CategoryId" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabCategoryRow NewLabCategoryRow() { return (LabCategoryRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabCategoryRow(b); }
            protected override Type GetRowType() { return typeof(LabCategoryRow); }

            public DataColumn LabCategoryIdColumn { get { return Columns["LabCategoryId"]; } }
            public DataColumn CategoryNameColumn { get { return Columns["CategoryName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CategoryIdColumn { get { return Columns["CategoryId"]; } }
        }

        public class LabCategoryRow : DataRow
        {
            internal LabCategoryRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabCategoryId { get { var v = this["LabCategoryId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string CategoryName { get { return G("CategoryName"); } set { S("CategoryName", value); } }
        }

        public class LabTypeDataTable : DataTable
        {
            public LabTypeDataTable() : base("LabType")
            {
                Columns.Add(new DataColumn("LabTypeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "TypeName", "IsActive", "LabTypeName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabTypeRow NewLabTypeRow() { return (LabTypeRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabTypeRow(b); }
            protected override Type GetRowType() { return typeof(LabTypeRow); }

            public DataColumn LabTypeIdColumn { get { return Columns["LabTypeId"]; } }
            public DataColumn TypeNameColumn { get { return Columns["TypeName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn LabTypeNameColumn { get { return Columns["LabTypeName"]; } }
        }

        public class LabTypeRow : DataRow
        {
            internal LabTypeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LabTypeId { get { var v = this["LabTypeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TypeName { get { return G("TypeName"); } set { S("TypeName", value); } }
        }

        public class LabCodeSystemDataTable : DataTable
        {
            public LabCodeSystemDataTable() : base("LabCodeSystem")
            {
                Columns.Add(new DataColumn("CodeSystemId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "SystemName", "CodeSystemName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabCodeSystemRow NewLabCodeSystemRow() { return (LabCodeSystemRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabCodeSystemRow(b); }
            protected override Type GetRowType() { return typeof(LabCodeSystemRow); }

            public DataColumn CodeSystemIdColumn { get { return Columns["CodeSystemId"]; } }
            public DataColumn SystemNameColumn { get { return Columns["SystemName"]; } }
            public DataColumn CodeSystemNameColumn { get { return Columns["CodeSystemName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class LabCodeSystemRow : DataRow
        {
            internal LabCodeSystemRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long CodeSystemId { get { var v = this["CodeSystemId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string SystemName { get { return G("SystemName"); } set { S("SystemName", value); } }
        }

        public class LabRequisitionTemplateDataTable : DataTable
        {
            public LabRequisitionTemplateDataTable() : base("LabRequisitionTemplate")
            {
                Columns.Add(new DataColumn("RequisitionTemplateId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "TemplateName", "RequisitionTemplateName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public LabRequisitionTemplateRow NewLabRequisitionTemplateRow() { return (LabRequisitionTemplateRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new LabRequisitionTemplateRow(b); }
            protected override Type GetRowType() { return typeof(LabRequisitionTemplateRow); }

            public DataColumn RequisitionTemplateIdColumn { get { return Columns["RequisitionTemplateId"]; } }
            public DataColumn TemplateNameColumn { get { return Columns["TemplateName"]; } }
            public DataColumn RequisitionTemplateNameColumn { get { return Columns["RequisitionTemplateName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class LabRequisitionTemplateRow : DataRow
        {
            internal LabRequisitionTemplateRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long RequisitionTemplateId { get { var v = this["RequisitionTemplateId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TemplateName { get { return G("TemplateName"); } set { S("TemplateName", value); } }
        }
    }
}
