using System;
using System.Data;

namespace EDIParser
{
    public class DS271 : DataSet
    {
        private EDI271HeaderDataTable _tableHeader;
        private EDI271NamesDataTable _tableNames;
        private EDI271BenefitsDataTable _tableBenefits;
        private EDI271BenefitsDetailDataTable _tableBenefitsDetail;

        public DS271()
        {
            DataSetName = "DS271";
            _tableHeader = new EDI271HeaderDataTable();
            _tableNames = new EDI271NamesDataTable();
            _tableBenefits = new EDI271BenefitsDataTable();
            _tableBenefitsDetail = new EDI271BenefitsDetailDataTable();
            Tables.Add(_tableHeader);
            Tables.Add(_tableNames);
            Tables.Add(_tableBenefits);
            Tables.Add(_tableBenefitsDetail);
        }

        public EDI271HeaderDataTable EDI271Header { get { return _tableHeader; } }
        public EDI271NamesDataTable EDI271Names { get { return _tableNames; } }
        public EDI271BenefitsDataTable EDI271Benefits { get { return _tableBenefits; } }
        public EDI271BenefitsDetailDataTable EDI271BenefitsDetail { get { return _tableBenefitsDetail; } }

        // ── EDI271Header ────────────────────────────────────────────────────────
        public class EDI271HeaderDataTable : DataTable
        {
            public EDI271HeaderDataTable() : base("EDI271Header")
            {
                Columns.Add(new DataColumn("IsEligible", typeof(string)) { DefaultValue = "" });
                Columns.Add(new DataColumn("Copay", typeof(string)) { DefaultValue = "" });
                Columns.Add(new DataColumn("Deductible", typeof(string)) { DefaultValue = "" });
            }
            public EDI271HeaderRow NewEDI271HeaderRow() { return (EDI271HeaderRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) { return new EDI271HeaderRow(builder); }
            protected override Type GetRowType() { return typeof(EDI271HeaderRow); }
            public DataColumn IsEligibleColumn  { get { return Columns["IsEligible"]; } }
            public DataColumn CopayColumn       { get { return Columns["Copay"]; } }
            public DataColumn DeductibleColumn  { get { return Columns["Deductible"]; } }
        }

        public class EDI271HeaderRow : DataRow
        {
            internal EDI271HeaderRow(DataRowBuilder rb) : base(rb) { }
            public string IsEligible   { get { return this["IsEligible"] as string ?? ""; }   set { this["IsEligible"] = value ?? ""; } }
            public string Copay        { get { return this["Copay"] as string ?? ""; }        set { this["Copay"] = value ?? ""; } }
            public string Deductible   { get { return this["Deductible"] as string ?? ""; }   set { this["Deductible"] = value ?? ""; } }
        }

        // ── EDI271Names ─────────────────────────────────────────────────────────
        public class EDI271NamesDataTable : DataTable
        {
            public EDI271NamesDataTable() : base("EDI271Names")
            {
                var idCol = new DataColumn("EDI271NameId", typeof(long))
                {
                    AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1
                };
                Columns.Add(idCol);
                PrimaryKey = new DataColumn[] { idCol };
                string[] cols = {
                    "AAA01","AAA02","AAA04",
                    "DMG03",
                    "DTP01","DTP02","DTP03",
                    "INS01","INS02","INS03","INS04","INS09","INS10","INS17",
                    "MPI01","MPI02","MPI03","MPI04","MPI05","MPI06","MPI07",
                    "N301","N302","N401","N402","N403",
                    "NM101","NM101Code","NM103","NM104","NM105","NM106","NM108","NM109",
                    "PER01","PER02","PER03","PER04","PER05","PER06","PER07","PER08",
                    "PRV01","PRV02","PRV03",
                    "REF01","REF02","REF03",
                    "TRN01","TRN02","TRN03","TRN04"
                };
                foreach (var c in cols)
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public EDI271NamesRow NewEDI271NamesRow() { return (EDI271NamesRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) { return new EDI271NamesRow(builder); }
            protected override Type GetRowType() { return typeof(EDI271NamesRow); }
        }

        public class EDI271NamesRow : DataRow
        {
            internal EDI271NamesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { return this[c] as string ?? ""; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long EDI271NameId { get { return Convert.ToInt64(this["EDI271NameId"]); } }
            public string AAA01 { get { return G("AAA01"); } set { S("AAA01", value); } }
            public string AAA02 { get { return G("AAA02"); } set { S("AAA02", value); } }
            public string AAA04 { get { return G("AAA04"); } set { S("AAA04", value); } }
            public string DMG03 { get { return G("DMG03"); } set { S("DMG03", value); } }
            public string DTP01 { get { return G("DTP01"); } set { S("DTP01", value); } }
            public string DTP02 { get { return G("DTP02"); } set { S("DTP02", value); } }
            public string DTP03 { get { return G("DTP03"); } set { S("DTP03", value); } }
            public string INS01 { get { return G("INS01"); } set { S("INS01", value); } }
            public string INS02 { get { return G("INS02"); } set { S("INS02", value); } }
            public string INS03 { get { return G("INS03"); } set { S("INS03", value); } }
            public string INS04 { get { return G("INS04"); } set { S("INS04", value); } }
            public string INS09 { get { return G("INS09"); } set { S("INS09", value); } }
            public string INS10 { get { return G("INS10"); } set { S("INS10", value); } }
            public string INS17 { get { return G("INS17"); } set { S("INS17", value); } }
            public string MPI01 { get { return G("MPI01"); } set { S("MPI01", value); } }
            public string MPI02 { get { return G("MPI02"); } set { S("MPI02", value); } }
            public string MPI03 { get { return G("MPI03"); } set { S("MPI03", value); } }
            public string MPI04 { get { return G("MPI04"); } set { S("MPI04", value); } }
            public string MPI05 { get { return G("MPI05"); } set { S("MPI05", value); } }
            public string MPI06 { get { return G("MPI06"); } set { S("MPI06", value); } }
            public string MPI07 { get { return G("MPI07"); } set { S("MPI07", value); } }
            public string N301 { get { return G("N301"); } set { S("N301", value); } }
            public string N302 { get { return G("N302"); } set { S("N302", value); } }
            public string N401 { get { return G("N401"); } set { S("N401", value); } }
            public string N402 { get { return G("N402"); } set { S("N402", value); } }
            public string N403 { get { return G("N403"); } set { S("N403", value); } }
            public string NM101 { get { return G("NM101"); } set { S("NM101", value); } }
            public string NM101Code { get { return G("NM101Code"); } set { S("NM101Code", value); } }
            public string NM103 { get { return G("NM103"); } set { S("NM103", value); } }
            public string NM104 { get { return G("NM104"); } set { S("NM104", value); } }
            public string NM105 { get { return G("NM105"); } set { S("NM105", value); } }
            public string NM106 { get { return G("NM106"); } set { S("NM106", value); } }
            public string NM108 { get { return G("NM108"); } set { S("NM108", value); } }
            public string NM109 { get { return G("NM109"); } set { S("NM109", value); } }
            public string PER01 { get { return G("PER01"); } set { S("PER01", value); } }
            public string PER02 { get { return G("PER02"); } set { S("PER02", value); } }
            public string PER03 { get { return G("PER03"); } set { S("PER03", value); } }
            public string PER04 { get { return G("PER04"); } set { S("PER04", value); } }
            public string PER05 { get { return G("PER05"); } set { S("PER05", value); } }
            public string PER06 { get { return G("PER06"); } set { S("PER06", value); } }
            public string PER07 { get { return G("PER07"); } set { S("PER07", value); } }
            public string PER08 { get { return G("PER08"); } set { S("PER08", value); } }
            public string PRV01 { get { return G("PRV01"); } set { S("PRV01", value); } }
            public string PRV02 { get { return G("PRV02"); } set { S("PRV02", value); } }
            public string PRV03 { get { return G("PRV03"); } set { S("PRV03", value); } }
            public string REF01 { get { return G("REF01"); } set { S("REF01", value); } }
            public string REF02 { get { return G("REF02"); } set { S("REF02", value); } }
            public string REF03 { get { return G("REF03"); } set { S("REF03", value); } }
            public string TRN01 { get { return G("TRN01"); } set { S("TRN01", value); } }
            public string TRN02 { get { return G("TRN02"); } set { S("TRN02", value); } }
            public string TRN03 { get { return G("TRN03"); } set { S("TRN03", value); } }
            public string TRN04 { get { return G("TRN04"); } set { S("TRN04", value); } }
        }

        // ── EDI271Benefits ──────────────────────────────────────────────────────
        public class EDI271BenefitsDataTable : DataTable
        {
            public EDI271BenefitsDataTable() : base("EDI271Benefits")
            {
                var idCol = new DataColumn("EDI271BenefitId", typeof(long))
                {
                    AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1
                };
                Columns.Add(idCol);
                PrimaryKey = new DataColumn[] { idCol };
                Columns.Add(new DataColumn("EDI271NameId", typeof(long)) { DefaultValue = 0L });
                string[] cols = {
                    "DTP01","DTP03",
                    "EB01","EB02","EB03","EB04","EB05","EB06","EB07","EB08",
                    "EB09","EB10","EB11","EB12","EB13_1","EB13_2",
                    "ServiceTypeCode","MSG01"
                };
                foreach (var c in cols)
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public EDI271BenefitsRow NewEDI271BenefitsRow() { return (EDI271BenefitsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) { return new EDI271BenefitsRow(builder); }
            protected override Type GetRowType() { return typeof(EDI271BenefitsRow); }
            public DataColumn EDI271BenefitIdColumn { get { return Columns["EDI271BenefitId"]; } }
            public DataColumn EDI271NameIdColumn { get { return Columns["EDI271NameId"]; } }
            public DataColumn EB01Column { get { return Columns["EB01"]; } }
            public DataColumn EB02Column { get { return Columns["EB02"]; } }
            public DataColumn EB03Column { get { return Columns["EB03"]; } }
            public DataColumn ServiceTypeCodeColumn { get { return Columns["ServiceTypeCode"]; } }
            public DataColumn MSG01Column { get { return Columns["MSG01"]; } }
        }

        public class EDI271BenefitsRow : DataRow
        {
            internal EDI271BenefitsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { return this[c] as string ?? ""; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long EDI271BenefitId { get { return Convert.ToInt64(this["EDI271BenefitId"]); } }
            public long EDI271NameId
            {
                get { return Convert.ToInt64(this["EDI271NameId"]); }
                set { this["EDI271NameId"] = value; }
            }
            public string DTP01 { get { return G("DTP01"); } set { S("DTP01", value); } }
            public string DTP03 { get { return G("DTP03"); } set { S("DTP03", value); } }
            public string EB01 { get { return G("EB01"); } set { S("EB01", value); } }
            public string EB02 { get { return G("EB02"); } set { S("EB02", value); } }
            public string EB03 { get { return G("EB03"); } set { S("EB03", value); } }
            public string EB04 { get { return G("EB04"); } set { S("EB04", value); } }
            public string EB05 { get { return G("EB05"); } set { S("EB05", value); } }
            public string EB06 { get { return G("EB06"); } set { S("EB06", value); } }
            public string EB07 { get { return G("EB07"); } set { S("EB07", value); } }
            public string EB08 { get { return G("EB08"); } set { S("EB08", value); } }
            public string EB09 { get { return G("EB09"); } set { S("EB09", value); } }
            public string EB10 { get { return G("EB10"); } set { S("EB10", value); } }
            public string EB11 { get { return G("EB11"); } set { S("EB11", value); } }
            public string EB12 { get { return G("EB12"); } set { S("EB12", value); } }
            public string EB13_1 { get { return G("EB13_1"); } set { S("EB13_1", value); } }
            public string EB13_2 { get { return G("EB13_2"); } set { S("EB13_2", value); } }
            public string ServiceTypeCode { get { return G("ServiceTypeCode"); } set { S("ServiceTypeCode", value); } }
            public string MSG01 { get { return G("MSG01"); } set { S("MSG01", value); } }
        }

        // ── EDI271BenefitsDetail ────────────────────────────────────────────────
        public class EDI271BenefitsDetailDataTable : DataTable
        {
            public EDI271BenefitsDetailDataTable() : base("EDI271BenefitsDetail")
            {
                Columns.Add(new DataColumn("EDI271BenefitId", typeof(long)) { DefaultValue = 0L });
                string[] cols = { "HSD01","HSD02","HSD03","HSD05","HSD06","HSD07","HSD08" };
                foreach (var c in cols)
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public EDI271BenefitsDetailRow NewEDI271BenefitsDetailRow() { return (EDI271BenefitsDetailRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) { return new EDI271BenefitsDetailRow(builder); }
            protected override Type GetRowType() { return typeof(EDI271BenefitsDetailRow); }
        }

        public class EDI271BenefitsDetailRow : DataRow
        {
            internal EDI271BenefitsDetailRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { return this[c] as string ?? ""; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long EDI271BenefitId
            {
                get { return Convert.ToInt64(this["EDI271BenefitId"]); }
                set { this["EDI271BenefitId"] = value; }
            }
            public string HSD01 { get { return G("HSD01"); } set { S("HSD01", value); } }
            public string HSD02 { get { return G("HSD02"); } set { S("HSD02", value); } }
            public string HSD03 { get { return G("HSD03"); } set { S("HSD03", value); } }
            public string HSD05 { get { return G("HSD05"); } set { S("HSD05", value); } }
            public string HSD06 { get { return G("HSD06"); } set { S("HSD06", value); } }
            public string HSD07 { get { return G("HSD07"); } set { S("HSD07", value); } }
            public string HSD08 { get { return G("HSD08"); } set { S("HSD08", value); } }
        }
    }
}
