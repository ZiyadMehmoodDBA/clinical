using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSBillingInformationLookup : DataSet
    {
        private BillingInfoTypeDataTable _tBillingInfoType;
        private BillingInfoTimeDataTable _tBillingInfoTime;

        public DSBillingInformationLookup()
        {
            DataSetName = "DSBillingInformationLookup";
            _tBillingInfoType = new BillingInfoTypeDataTable();
            _tBillingInfoTime = new BillingInfoTimeDataTable();
            Tables.Add(_tBillingInfoType);
            Tables.Add(_tBillingInfoTime);
        }

        public BillingInfoTypeDataTable BillingInfoType { get { return _tBillingInfoType; } }
        public BillingInfoTimeDataTable BillingInfoTime { get { return _tBillingInfoTime; } }

        public class BillingInfoTypeDataTable : DataTable
        {
            public BillingInfoTypeDataTable() : base("BillingInfoType")
            {
                var id = new DataColumn("BillingInfoTypeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "TypeName", "IsActive", "Description" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public BillingInfoTypeRow NewBillingInfoTypeRow() { return (BillingInfoTypeRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new BillingInfoTypeRow(b); }
            protected override Type GetRowType() { return typeof(BillingInfoTypeRow); }

            public DataColumn BillingInfoTypeIdColumn { get { return Columns["BillingInfoTypeId"]; } }
            public DataColumn TypeNameColumn { get { return Columns["TypeName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn DescriptionColumn { get { return Columns["Description"]; } }
        }

        public class BillingInfoTypeRow : DataRow
        {
            internal BillingInfoTypeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long BillingInfoTypeId { get { var v = this["BillingInfoTypeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TypeName { get { return G("TypeName"); } set { S("TypeName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class BillingInfoTimeDataTable : DataTable
        {
            public BillingInfoTimeDataTable() : base("BillingInfoTime")
            {
                var id = new DataColumn("BillingInfoTimeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "TimeName", "IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public BillingInfoTimeRow NewBillingInfoTimeRow() { return (BillingInfoTimeRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new BillingInfoTimeRow(b); }
            protected override Type GetRowType() { return typeof(BillingInfoTimeRow); }

            public DataColumn BillingInfoTimeIdColumn { get { return Columns["BillingInfoTimeId"]; } }
            public DataColumn TimeNameColumn { get { return Columns["TimeName"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }

        public class BillingInfoTimeRow : DataRow
        {
            internal BillingInfoTimeRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long BillingInfoTimeId { get { var v = this["BillingInfoTimeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string TimeName { get { return G("TimeName"); } set { S("TimeName", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }
    }
}
