using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSDocument : DataSet
    {
        public DSDocument()
        {
            DataSetName = "DSDocument";
            Tables.Add(new DocumentsDataTable());
            Tables.Add(new DocumentTypeDataTable());
            Tables.Add(new FolderDocumentDataTable());
            Tables.Add(new FaxDocumentsDataTable());
            Tables.Add(new MedicalDocumentsDataTable());
            Tables.Add(new FacilityFaxContactsDataTable());
            Tables.Add(new ProviderFaxContactsDataTable());
        }

        public DocumentsDataTable Documents { get { return (DocumentsDataTable)Tables["Documents"]; } }
        public DocumentTypeDataTable DocumentType { get { return (DocumentTypeDataTable)Tables["DocumentType"]; } }
        public FolderDocumentDataTable FolderDocument { get { return (FolderDocumentDataTable)Tables["FolderDocument"]; } }
        public FaxDocumentsDataTable FaxDocuments { get { return (FaxDocumentsDataTable)Tables["FaxDocuments"]; } }
        public MedicalDocumentsDataTable MedicalDocuments { get { return (MedicalDocumentsDataTable)Tables["MedicalDocuments"]; } }
        public FacilityFaxContactsDataTable FacilityFaxContacts { get { return (FacilityFaxContactsDataTable)Tables["FacilityFaxContacts"]; } }
        public ProviderFaxContactsDataTable ProviderFaxContacts { get { return (ProviderFaxContactsDataTable)Tables["ProviderFaxContacts"]; } }

        public class DocumentsDataTable : DataTable
        {
            public DocumentsDataTable() : base("Documents")
            {
                var id = new DataColumn("DocId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ShortName","DocTypeId","BarCodeValue","EntityId","Description",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","ErrorMessage","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public DocumentsRow NewDocumentsRow() { return (DocumentsRow)NewRow(); }
            public void AddDocumentsRow(DocumentsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new DocumentsRow(b); }
            protected override Type GetRowType() { return typeof(DocumentsRow); }
            public DataColumn DocIdColumn { get { return Columns["DocId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn DocTypeIdColumn { get { return Columns["DocTypeId"]; } }
            public DataColumn BarCodeValueColumn { get { return Columns["BarCodeValue"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ErrorMessageColumn { get { return Columns["ErrorMessage"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class DocumentsRow : DataRow
        {
            internal DocumentsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   DocId       { get { var v = this["DocId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName   { get { return G("ShortName"); }   set { S("ShortName", value); } }
            public int    DocTypeId   { get { var v = this["DocTypeId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["DocTypeId"] = value.ToString(); } }
            public string BarCodeValue { get { return G("BarCodeValue"); } set { S("BarCodeValue", value); } }
            public bool   IsActive    { get { var v = this["IsActive"];  return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"]  = value.ToString(); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public string CreatedBy   { get { return G("CreatedBy"); }   set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy  { get { return G("ModifiedBy"); }  set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn{ get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class DocumentTypeDataTable : DataTable
        {
            public DocumentTypeDataTable() : base("DocumentType")
            {
                var id = new DataColumn("DoctypeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "ShortName","Description","EntityId","IsActive","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn","ErrorMessage","RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public DocumentTypeRow NewDocumentTypeRow() { return (DocumentTypeRow)NewRow(); }
            public void AddDocumentTypeRow(DocumentTypeRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new DocumentTypeRow(b); }
            protected override Type GetRowType() { return typeof(DocumentTypeRow); }
            public DataColumn DoctypeIdColumn { get { return Columns["DoctypeId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn ErrorMessageColumn { get { return Columns["ErrorMessage"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }
        public class DocumentTypeRow : DataRow
        {
            internal DocumentTypeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   DoctypeId   { get { var v = this["DoctypeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ShortName   { get { return G("ShortName"); }   set { S("ShortName", value); } }
            public string Description { get { return G("Description"); } set { S("Description", value); } }
            public long   EntityId    { get { var v = this["EntityId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["EntityId"]  = value.ToString(); } }
            public bool   IsActive    { get { var v = this["IsActive"];  return v != DBNull.Value && Convert.ToBoolean(v); }  set { this["IsActive"]  = value.ToString(); } }
            public string CreatedBy   { get { return G("CreatedBy"); }   set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy  { get { return G("ModifiedBy"); }  set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn{ get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class FolderDocumentDataTable : DataTable
        {
            public FolderDocumentDataTable() : base("FolderDocument")
            {
                var id = new DataColumn("FolderDocumentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "FolderId","DocId","EntityId","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FolderDocumentRow NewFolderDocumentRow() { return (FolderDocumentRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FolderDocumentRow(b); }
            protected override Type GetRowType() { return typeof(FolderDocumentRow); }
            public DataColumn FolderDocumentIdColumn { get { return Columns["FolderDocumentId"]; } }
            public DataColumn FolderIdColumn { get { return Columns["FolderId"]; } }
            public DataColumn DocIdColumn { get { return Columns["DocId"]; } }
            public DataColumn EntityIdColumn { get { return Columns["EntityId"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class FolderDocumentRow : DataRow
        {
            internal FolderDocumentRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long FolderDocumentId { get { var v = this["FolderDocumentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string DocId { get { return G("DocId"); } set { S("DocId", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class FaxDocumentsDataTable : DataTable
        {
            public FaxDocumentsDataTable() : base("FaxDocuments")
            {
                var id = new DataColumn("FaxId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "DocId","FaxDocumentPath","FilePath","FileStream","FileType","Pages","IsActive",
                    "CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","AssignedByUserId","Comments",
                    "UserId","IsConfidential" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FaxDocumentsRow NewFaxDocumentsRow() { return (FaxDocumentsRow)NewRow(); }
            public void AddFaxDocumentsRow(FaxDocumentsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FaxDocumentsRow(b); }
            protected override Type GetRowType() { return typeof(FaxDocumentsRow); }
            public DataColumn FaxIdColumn { get { return Columns["FaxId"]; } }
            public DataColumn DocIdColumn { get { return Columns["DocId"]; } }
            public DataColumn FaxDocumentPathColumn { get { return Columns["FaxDocumentPath"]; } }
            public DataColumn FilePathColumn { get { return Columns["FilePath"]; } }
            public DataColumn FileStreamColumn { get { return Columns["FileStream"]; } }
            public DataColumn FileTypeColumn { get { return Columns["FileType"]; } }
            public DataColumn PagesColumn { get { return Columns["Pages"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn AssignedByUserIdColumn { get { return Columns["AssignedByUserId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn UserIdColumn { get { return Columns["UserId"]; } }
            public DataColumn IsConfidentialColumn { get { return Columns["IsConfidential"]; } }
        }
        public class FaxDocumentsRow : DataRow
        {
            internal FaxDocumentsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long FaxId { get { var v = this["FaxId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["FaxId"] = value; } }
            public string DocId { get { return G("DocId"); } set { S("DocId", value); } }
            public string FilePath { get { return G("FilePath"); } set { S("FilePath", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
            public long UserId { get { var v = this["UserId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["UserId"] = value.ToString(); } }
            public string FileType { get { return G("FileType"); } set { S("FileType", value); } }
            public string FileStream { get { return G("FileStream"); } set { S("FileStream", value == null ? "" : value.ToString()); } }
            public string FaxDocumentPath { get { return G("FaxDocumentPath"); } set { S("FaxDocumentPath", value); } }
            public long AssignedByUserId { get { var v = this["AssignedByUserId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["AssignedByUserId"] = value.ToString(); } }
            public string Comments { get { return G("Comments"); } set { S("Comments", value); } }
            public bool IsConfidential { get { var v = this["IsConfidential"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsConfidential"] = value.ToString(); } }
            public int Pages { get { var v = this["Pages"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["Pages"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn { get { var v = this["CreatedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"] = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class MedicalDocumentsDataTable : DataTable
        {
            public MedicalDocumentsDataTable() : base("MedicalDocuments")
            {
                var id = new DataColumn("MedicalDocId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "FilePath","FileStream","FileType","Pages","PatientId","TransitionId" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public MedicalDocumentsRow NewMedicalDocumentsRow() { return (MedicalDocumentsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new MedicalDocumentsRow(b); }
            protected override Type GetRowType() { return typeof(MedicalDocumentsRow); }
            public DataColumn MedicalDocIdColumn { get { return Columns["MedicalDocId"]; } }
            public DataColumn FilePathColumn { get { return Columns["FilePath"]; } }
            public DataColumn FileStreamColumn { get { return Columns["FileStream"]; } }
            public DataColumn FileTypeColumn { get { return Columns["FileType"]; } }
            public DataColumn PagesColumn { get { return Columns["Pages"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn TransitionIdColumn { get { return Columns["TransitionId"]; } }
        }
        public class MedicalDocumentsRow : DataRow
        {
            internal MedicalDocumentsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long MedicalDocId { get { var v = this["MedicalDocId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string FilePath { get { return G("FilePath"); } set { S("FilePath", value); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
        }

        public class FacilityFaxContactsDataTable : DataTable
        {
            public FacilityFaxContactsDataTable() : base("FacilityFaxContacts")
            {
                var id = new DataColumn("FacilityFaxContactId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "FacilityId","ContactName","FaxNumber","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public FacilityFaxContactsRow NewFacilityFaxContactsRow() { return (FacilityFaxContactsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new FacilityFaxContactsRow(b); }
            protected override Type GetRowType() { return typeof(FacilityFaxContactsRow); }
            public DataColumn FacilityFaxContactIdColumn { get { return Columns["FacilityFaxContactId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn ContactNameColumn { get { return Columns["ContactName"]; } }
            public DataColumn FaxNumberColumn { get { return Columns["FaxNumber"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }
        public class FacilityFaxContactsRow : DataRow
        {
            internal FacilityFaxContactsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long FacilityFaxContactId { get { var v = this["FacilityFaxContactId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ContactName { get { return G("ContactName"); } set { S("ContactName", value); } }
            public string FaxNumber { get { return G("FaxNumber"); } set { S("FaxNumber", value); } }
        }

        public class ProviderFaxContactsDataTable : DataTable
        {
            public ProviderFaxContactsDataTable() : base("ProviderFaxContacts")
            {
                var id = new DataColumn("ContactId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id); PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "ProviderId","ContactName","FaxNumber","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ProviderFaxContactsRow NewProviderFaxContactsRow() { return (ProviderFaxContactsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ProviderFaxContactsRow(b); }
            protected override Type GetRowType() { return typeof(ProviderFaxContactsRow); }
            public DataColumn ContactIdColumn { get { return Columns["ContactId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn ContactNameColumn { get { return Columns["ContactName"]; } }
            public DataColumn FaxNumberColumn { get { return Columns["FaxNumber"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }
        public class ProviderFaxContactsRow : DataRow
        {
            internal ProviderFaxContactsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ContactId { get { var v = this["ContactId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ContactName { get { return G("ContactName"); } set { S("ContactName", value); } }
            public string FaxNumber { get { return G("FaxNumber"); } set { S("FaxNumber", value); } }
        }
    }
}
