using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSVitals : DataSet
    {
        private VitalSignsDataTable _tVitalSigns;
        private VitalSignsPulseDataTable _tVitalSignsPulse;
        private VitalSignsTemperatureDataTable _tVitalSignsTemperature;
        private VitalSignsRespirationDataTable _tVitalSignsRespiration;
        private VitalSignsBloodPressureDataTable _tVitalSignsBloodPressure;
        private VitalSignsChildDataTable _tVitalSignsChild;
        private VitalSignsLookupDataTable _tVitalSignsLookup;
        private VitalSignSoapDataTable _tVitalSignSoap;

        public DSVitals()
        {
            DataSetName = "DSVitals";
            _tVitalSigns = new VitalSignsDataTable();
            _tVitalSignsPulse = new VitalSignsPulseDataTable();
            _tVitalSignsTemperature = new VitalSignsTemperatureDataTable();
            _tVitalSignsRespiration = new VitalSignsRespirationDataTable();
            _tVitalSignsBloodPressure = new VitalSignsBloodPressureDataTable();
            _tVitalSignsChild = new VitalSignsChildDataTable();
            _tVitalSignsLookup = new VitalSignsLookupDataTable();
            _tVitalSignSoap = new VitalSignSoapDataTable();
            Tables.Add(_tVitalSigns);
            Tables.Add(_tVitalSignsPulse);
            Tables.Add(_tVitalSignsTemperature);
            Tables.Add(_tVitalSignsRespiration);
            Tables.Add(_tVitalSignsBloodPressure);
            Tables.Add(_tVitalSignsChild);
            Tables.Add(_tVitalSignsLookup);
            Tables.Add(_tVitalSignSoap);
        }

        public VitalSignsDataTable VitalSigns { get { return _tVitalSigns; } }
        public VitalSignsPulseDataTable VitalSignsPulse { get { return _tVitalSignsPulse; } }
        public VitalSignsTemperatureDataTable VitalSignsTemperature { get { return _tVitalSignsTemperature; } }
        public VitalSignsRespirationDataTable VitalSignsRespiration { get { return _tVitalSignsRespiration; } }
        public VitalSignsBloodPressureDataTable VitalSignsBloodPressure { get { return _tVitalSignsBloodPressure; } }
        public VitalSignsChildDataTable VitalSignsChild { get { return _tVitalSignsChild; } }
        public VitalSignsLookupDataTable VitalSignsLookup { get { return _tVitalSignsLookup; } }
        public VitalSignSoapDataTable VitalSignSoap { get { return _tVitalSignSoap; } }

        public class VitalSignsDataTable : DataTable
        {
            public VitalSignsDataTable() : base("VitalSigns")
            {
                var id = new DataColumn("VitalSignId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "PatientId","VisitId","Height","Weight","IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn",
                    "NotesId","CopyParentId","SPO2","OxygenSource","PeakFlow","PainId","SmokeStatusId","Comments",
                    "VitalSignDate","VitalSignTime","BMI","BSA","HeadCr","BloodType","DeleteComments",
                    "RiskAssessmentId","IsFromNote","InhaledO2Concentration","NegationReasonId","RecordCount",
                    "PulseResult","RespirationResult","TemperatureResult" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsRow NewVitalSignsRow() { return (VitalSignsRow)NewRow(); }
            public void AddVitalSignsRow(VitalSignsRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsRow); }

            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn VisitIdColumn { get { return Columns["VisitId"]; } }
            public DataColumn HeightColumn { get { return Columns["Height"]; } }
            public DataColumn WeightColumn { get { return Columns["Weight"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn NotesIdColumn { get { return Columns["NotesId"]; } }
            public DataColumn CopyParentIdColumn { get { return Columns["CopyParentId"]; } }
            public DataColumn SPO2Column { get { return Columns["SPO2"]; } }
            public DataColumn OxygenSourceColumn { get { return Columns["OxygenSource"]; } }
            public DataColumn PeakFlowColumn { get { return Columns["PeakFlow"]; } }
            public DataColumn PainIdColumn { get { return Columns["PainId"]; } }
            public DataColumn SmokeStatusIdColumn { get { return Columns["SmokeStatusId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn VitalSignDateColumn { get { return Columns["VitalSignDate"]; } }
            public DataColumn VitalSignTimeColumn { get { return Columns["VitalSignTime"]; } }
            public DataColumn BMIColumn { get { return Columns["BMI"]; } }
            public DataColumn BSAColumn { get { return Columns["BSA"]; } }
            public DataColumn HeadCrColumn { get { return Columns["HeadCr"]; } }
            public DataColumn BloodTypeColumn { get { return Columns["BloodType"]; } }
            public DataColumn DeleteCommentsColumn { get { return Columns["DeleteComments"]; } }
            public DataColumn RiskAssessmentIdColumn { get { return Columns["RiskAssessmentId"]; } }
            public DataColumn IsFromNoteColumn { get { return Columns["IsFromNote"]; } }
            public DataColumn InhaledO2ConcentrationColumn { get { return Columns["InhaledO2Concentration"]; } }
            public DataColumn NegationReasonIdColumn { get { return Columns["NegationReasonId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn PulseResultColumn { get { return Columns["PulseResult"]; } }
            public DataColumn RespirationResultColumn { get { return Columns["RespirationResult"]; } }
            public DataColumn TemperatureResultColumn { get { return Columns["TemperatureResult"]; } }
        }

        public class VitalSignsRow : DataRow
        {
            internal VitalSignsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long VitalSignId { get { var v = this["VitalSignId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long   PatientId       { get { var v = this["PatientId"];       return v == DBNull.Value ? 0L    : Convert.ToInt64(v);    } set { this["PatientId"]       = value.ToString(); } }
            public string VisitId         { get { return G("VisitId"); }         set { S("VisitId", value); } }
            public string Height          { get { return G("Height"); }          set { S("Height", value); } }
            public double Weight          { get { var v = this["Weight"];        return v == DBNull.Value ? 0.0   : Convert.ToDouble(v);    } set { this["Weight"]         = value.ToString(); } }
            public string SPO2            { get { return G("SPO2"); }            set { S("SPO2", value); } }
            public double BMI             { get { var v = this["BMI"];           return v == DBNull.Value ? 0.0   : Convert.ToDouble(v);    } set { this["BMI"]           = value.ToString(); } }
            public double BSA             { get { var v = this["BSA"];           return v == DBNull.Value ? 0.0   : Convert.ToDouble(v);    } set { this["BSA"]           = value.ToString(); } }
            public int    PainId          { get { var v = this["PainId"];        return v == DBNull.Value ? 0     : Convert.ToInt32(v);     } set { this["PainId"]        = value.ToString(); } }
            public long   RiskAssessmentId{ get { var v = this["RiskAssessmentId"]; return v == DBNull.Value ? 0L  : Convert.ToInt64(v);   } set { this["RiskAssessmentId"] = value.ToString(); } }
            public bool   IsActive        { get { var v = this["IsActive"];      return v != DBNull.Value && Convert.ToBoolean(v);          } set { this["IsActive"]       = value.ToString(); } }
            public string CreatedBy       { get { return G("CreatedBy"); }       set { S("CreatedBy", value); } }
            public DateTime CreatedOn     { get { var v = this["CreatedOn"];     return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy      { get { return G("ModifiedBy"); }      set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn    { get { var v = this["ModifiedOn"];    return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string RecordCount     { get { return G("RecordCount"); }     set { S("RecordCount", value); } }
            public long   NotesId         { get { var v = this["NotesId"];       return v == DBNull.Value ? 0L    : Convert.ToInt64(v);    } set { this["NotesId"]        = value.ToString(); } }
            public string OxygenSource    { get { return G("OxygenSource"); }    set { S("OxygenSource", value); } }
            public string PeakFlow        { get { return G("PeakFlow"); }        set { S("PeakFlow", value); } }
            public int    SmokeStatusId   { get { var v = this["SmokeStatusId"]; return v == DBNull.Value ? 0     : Convert.ToInt32(v);    } set { this["SmokeStatusId"] = value.ToString(); } }
            public string Comments        { get { return G("Comments"); }        set { S("Comments", value); } }
            public DateTime VitalSignDate { get { var v = this["VitalSignDate"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["VitalSignDate"] = value.ToString(); } }
            public string VitalSignTime   { get { return G("VitalSignTime"); }   set { S("VitalSignTime", value); } }
            public double HeadCr          { get { var v = this["HeadCr"];        return v == DBNull.Value ? 0.0   : Convert.ToDouble(v);   } set { this["HeadCr"]        = value.ToString(); } }
            public int    BloodType       { get { var v = this["BloodType"];     return v == DBNull.Value ? 0     : Convert.ToInt32(v);    } set { this["BloodType"]     = value.ToString(); } }
            public bool   IsFromNote      { get { var v = this["IsFromNote"];    return v != DBNull.Value && Convert.ToBoolean(v);          } set { this["IsFromNote"]    = value.ToString(); } }
            public string InhaledO2Concentration { get { return G("InhaledO2Concentration"); } set { S("InhaledO2Concentration", value); } }
            public string NegationReasonId { get { return G("NegationReasonId"); } set { S("NegationReasonId", value); } }
            public string DeleteComments { get { return G("DeleteComments"); } set { S("DeleteComments", value); } }
        }

        public class VitalSignsPulseDataTable : DataTable
        {
            public VitalSignsPulseDataTable() : base("VitalSignsPulse")
            {
                var id = new DataColumn("PulseId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignId","Result","RhythmId","Time","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsPulseRow NewVitalSignsPulseRow() { return (VitalSignsPulseRow)NewRow(); }
            public void AddVitalSignsPulseRow(VitalSignsPulseRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsPulseRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsPulseRow); }

            public DataColumn PulseIdColumn { get { return Columns["PulseId"]; } }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn ResultColumn { get { return Columns["Result"]; } }
            public DataColumn RhythmIdColumn { get { return Columns["RhythmId"]; } }
            public DataColumn TimeColumn { get { return Columns["Time"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class VitalSignsPulseRow : DataRow
        {
            internal VitalSignsPulseRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   PulseId    { get { var v = this["PulseId"];    return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PulseId"]    = value; } }
            public long   VitalSignId{ get { var v = this["VitalSignId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["VitalSignId"] = value.ToString(); } }
            public string Result    { get { return G("Result"); }    set { S("Result", value); } }
            public short  RhythmId  { get { var v = this["RhythmId"]; return v == DBNull.Value ? (short)0 : Convert.ToInt16(v); } set { this["RhythmId"] = value.ToString(); } }
            public string Time      { get { return G("Time"); }      set { S("Time", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy{ get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class VitalSignsTemperatureDataTable : DataTable
        {
            public VitalSignsTemperatureDataTable() : base("VitalSignsTemperature")
            {
                var id = new DataColumn("TempratureId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignId","Result","MethodId","Time","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsTemperatureRow NewVitalSignsTemperatureRow() { return (VitalSignsTemperatureRow)NewRow(); }
            public void AddVitalSignsTemperatureRow(VitalSignsTemperatureRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsTemperatureRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsTemperatureRow); }

            public DataColumn TempratureIdColumn { get { return Columns["TempratureId"]; } }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn ResultColumn { get { return Columns["Result"]; } }
            public DataColumn MethodIdColumn { get { return Columns["MethodId"]; } }
            public DataColumn TimeColumn { get { return Columns["Time"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class VitalSignsTemperatureRow : DataRow
        {
            internal VitalSignsTemperatureRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   TempratureId{ get { var v = this["TempratureId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["TempratureId"] = value; } }
            public long   VitalSignId { get { var v = this["VitalSignId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["VitalSignId"] = value.ToString(); } }
            public string Result    { get { return G("Result"); }    set { S("Result", value); } }
            public short  MethodId  { get { var v = this["MethodId"]; return v == DBNull.Value ? (short)0 : Convert.ToInt16(v); } set { this["MethodId"] = value.ToString(); } }
            public string Time      { get { return G("Time"); }      set { S("Time", value); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy{ get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class VitalSignsRespirationDataTable : DataTable
        {
            public VitalSignsRespirationDataTable() : base("VitalSignsRespiration")
            {
                var id = new DataColumn("RespirationId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignId","Result","PatternId","Time","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsRespirationRow NewVitalSignsRespirationRow() { return (VitalSignsRespirationRow)NewRow(); }
            public void AddVitalSignsRespirationRow(VitalSignsRespirationRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsRespirationRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsRespirationRow); }

            public DataColumn RespirationIdColumn { get { return Columns["RespirationId"]; } }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn ResultColumn { get { return Columns["Result"]; } }
            public DataColumn PatternIdColumn { get { return Columns["PatternId"]; } }
            public DataColumn TimeColumn { get { return Columns["Time"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
        }

        public class VitalSignsRespirationRow : DataRow
        {
            internal VitalSignsRespirationRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   RespirationId{ get { var v = this["RespirationId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RespirationId"] = value; } }
            public long   VitalSignId  { get { var v = this["VitalSignId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["VitalSignId"]  = value.ToString(); } }
            public string Result     { get { return G("Result"); }     set { S("Result", value); } }
            public int    PatternId  { get { var v = this["PatternId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["PatternId"] = value.ToString(); } }
            public string Time       { get { return G("Time"); }       set { S("Time", value); } }
            public string CreatedBy  { get { return G("CreatedBy"); }  set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class VitalSignsBloodPressureDataTable : DataTable
        {
            public VitalSignsBloodPressureDataTable() : base("VitalSignsBloodPressure")
            {
                var id = new DataColumn("BPId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignId","Systolic","Diastolic","PositionId","CuffSizeId","CuffLocationId",
                    "Time","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","BPNegationReasonId" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsBloodPressureRow NewVitalSignsBloodPressureRow() { return (VitalSignsBloodPressureRow)NewRow(); }
            public void AddVitalSignsBloodPressureRow(VitalSignsBloodPressureRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsBloodPressureRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsBloodPressureRow); }

            public DataColumn BPIdColumn { get { return Columns["BPId"]; } }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn SystolicColumn { get { return Columns["Systolic"]; } }
            public DataColumn DiastolicColumn { get { return Columns["Diastolic"]; } }
            public DataColumn PositionIdColumn { get { return Columns["PositionId"]; } }
            public DataColumn CuffSizeIdColumn { get { return Columns["CuffSizeId"]; } }
            public DataColumn CuffLocationIdColumn { get { return Columns["CuffLocationId"]; } }
            public DataColumn TimeColumn { get { return Columns["Time"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn BPNegationReasonIdColumn { get { return Columns["BPNegationReasonId"]; } }
        }

        public class VitalSignsBloodPressureRow : DataRow
        {
            internal VitalSignsBloodPressureRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   BPId           { get { var v = this["BPId"];           return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["BPId"] = value; } }
            public long   VitalSignId    { get { var v = this["VitalSignId"];    return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["VitalSignId"]    = value.ToString(); } }
            public short  Systolic       { get { var v = this["Systolic"];    return v == DBNull.Value ? (short)0 : Convert.ToInt16(v); } set { this["Systolic"]    = value.ToString(); } }
            public short  Diastolic      { get { var v = this["Diastolic"];   return v == DBNull.Value ? (short)0 : Convert.ToInt16(v); } set { this["Diastolic"]   = value.ToString(); } }
            public int    PositionId     { get { var v = this["PositionId"];  return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["PositionId"]  = value.ToString(); } }
            public int    CuffSizeId     { get { var v = this["CuffSizeId"];  return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["CuffSizeId"]  = value.ToString(); } }
            public int    CuffLocationId { get { var v = this["CuffLocationId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } set { this["CuffLocationId"] = value.ToString(); } }
            public string BPNegationReasonId { get { return G("BPNegationReasonId"); } set { S("BPNegationReasonId", value); } }
            public string Time       { get { return G("Time"); }       set { S("Time", value); } }
            public string CreatedBy  { get { return G("CreatedBy"); }  set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        // ── VitalSignsChild ──────────────────────────────────────────────────────
        public class VitalSignsChildDataTable : DataTable
        {
            public VitalSignsChildDataTable() : base("VitalSignsChild")
            {
                var id = new DataColumn("VitalChildId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignId",
                    "BPId","Systolic","Diastolic","BPModifiedBy","BPModifiedOn","BPNegationReasonId",
                    "PulseId","PulseResult","PulseModifiedBy","PulseModifiedOn",
                    "TemperatureId","TemperatureResult","TempModifiedBy","TempModifiedOn",
                    "RespirationId","RespirationResult","RespModifiedBy","RespModifiedOn" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsChildRow NewVitalSignsChildRow() { return (VitalSignsChildRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsChildRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsChildRow); }
            public DataColumn VitalChildIdColumn { get { return Columns["VitalChildId"]; } }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn BPIdColumn { get { return Columns["BPId"]; } }
            public DataColumn SystolicColumn { get { return Columns["Systolic"]; } }
            public DataColumn DiastolicColumn { get { return Columns["Diastolic"]; } }
            public DataColumn BPModifiedByColumn { get { return Columns["BPModifiedBy"]; } }
            public DataColumn BPModifiedOnColumn { get { return Columns["BPModifiedOn"]; } }
            public DataColumn BPNegationReasonIdColumn { get { return Columns["BPNegationReasonId"]; } }
            public DataColumn PulseIdColumn { get { return Columns["PulseId"]; } }
            public DataColumn PulseResultColumn { get { return Columns["PulseResult"]; } }
            public DataColumn PulseModifiedByColumn { get { return Columns["PulseModifiedBy"]; } }
            public DataColumn PulseModifiedOnColumn { get { return Columns["PulseModifiedOn"]; } }
            public DataColumn TemperatureIdColumn { get { return Columns["TemperatureId"]; } }
            public DataColumn TemperatureResultColumn { get { return Columns["TemperatureResult"]; } }
            public DataColumn TempModifiedByColumn { get { return Columns["TempModifiedBy"]; } }
            public DataColumn TempModifiedOnColumn { get { return Columns["TempModifiedOn"]; } }
            public DataColumn RespirationIdColumn { get { return Columns["RespirationId"]; } }
            public DataColumn RespirationResultColumn { get { return Columns["RespirationResult"]; } }
            public DataColumn RespModifiedByColumn { get { return Columns["RespModifiedBy"]; } }
            public DataColumn RespModifiedOnColumn { get { return Columns["RespModifiedOn"]; } }
        }
        public class VitalSignsChildRow : DataRow
        {
            internal VitalSignsChildRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long VitalChildId { get { var v = this["VitalChildId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string VitalSignId { get { return G("VitalSignId"); } set { S("VitalSignId", value); } }
            public string BPId { get { return G("BPId"); } set { S("BPId", value); } }
            public string Systolic { get { return G("Systolic"); } set { S("Systolic", value); } }
            public string Diastolic { get { return G("Diastolic"); } set { S("Diastolic", value); } }
            public string PulseId { get { return G("PulseId"); } set { S("PulseId", value); } }
            public string PulseResult { get { return G("PulseResult"); } set { S("PulseResult", value); } }
            public string TemperatureId { get { return G("TemperatureId"); } set { S("TemperatureId", value); } }
            public string TemperatureResult { get { return G("TemperatureResult"); } set { S("TemperatureResult", value); } }
            public string RespirationId { get { return G("RespirationId"); } set { S("RespirationId", value); } }
            public string RespirationResult { get { return G("RespirationResult"); } set { S("RespirationResult", value); } }
        }

        // ── VitalSignsLookup ─────────────────────────────────────────────────────
        public class VitalSignsLookupDataTable : DataTable
        {
            public VitalSignsLookupDataTable() : base("VitalSignsLookup")
            {
                var id = new DataColumn("LookUpId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                Columns.Add(new DataColumn("Value", typeof(string)) { DefaultValue = "" });
            }
            public VitalSignsLookupRow NewVitalSignsLookupRow() { return (VitalSignsLookupRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignsLookupRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignsLookupRow); }
            public DataColumn LookUpIdColumn { get { return Columns["LookUpId"]; } }
            public DataColumn ValueColumn { get { return Columns["Value"]; } }
        }
        public class VitalSignsLookupRow : DataRow
        {
            internal VitalSignsLookupRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long LookUpId { get { var v = this["LookUpId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string Value { get { return G("Value"); } set { S("Value", value); } }
        }

        // ── VitalSignSoap ────────────────────────────────────────────────────────
        public class VitalSignSoapDataTable : DataTable
        {
            public VitalSignSoapDataTable() : base("VitalSignSoap")
            {
                var id = new DataColumn("VitalSignId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "VitalSignDate","Systolic","Diastolic","TemperatureResult","BMI",
                    "BPId","TemperatureId","PulseId","RespirationId","RecordCount",
                    "Height","Weight","IsActive" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VitalSignSoapRow NewVitalSignSoapRow() { return (VitalSignSoapRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VitalSignSoapRow(b); }
            protected override Type GetRowType() { return typeof(VitalSignSoapRow); }
            public DataColumn VitalSignIdColumn { get { return Columns["VitalSignId"]; } }
            public DataColumn VitalSignDateColumn { get { return Columns["VitalSignDate"]; } }
            public DataColumn SystolicColumn { get { return Columns["Systolic"]; } }
            public DataColumn DiastolicColumn { get { return Columns["Diastolic"]; } }
            public DataColumn TemperatureResultColumn { get { return Columns["TemperatureResult"]; } }
            public DataColumn BMIColumn { get { return Columns["BMI"]; } }
            public DataColumn BPIdColumn { get { return Columns["BPId"]; } }
            public DataColumn TemperatureIdColumn { get { return Columns["TemperatureId"]; } }
            public DataColumn PulseIdColumn { get { return Columns["PulseId"]; } }
            public DataColumn RespirationIdColumn { get { return Columns["RespirationId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn HeightColumn { get { return Columns["Height"]; } }
            public DataColumn WeightColumn { get { return Columns["Weight"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
        }
        public class VitalSignSoapRow : DataRow
        {
            internal VitalSignSoapRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long VitalSignId { get { var v = this["VitalSignId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string VitalSignDate { get { return G("VitalSignDate"); } set { S("VitalSignDate", value); } }
            public string Systolic { get { return G("Systolic"); } set { S("Systolic", value); } }
            public string Diastolic { get { return G("Diastolic"); } set { S("Diastolic", value); } }
            public string TemperatureResult { get { return G("TemperatureResult"); } set { S("TemperatureResult", value); } }
            public string BMI { get { return G("BMI"); } set { S("BMI", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
            public string Height { get { return G("Height"); } set { S("Height", value); } }
            public string Weight { get { return G("Weight"); } set { S("Weight", value); } }
            public string PulseResult { get { return G("PulseResult"); } set { S("PulseResult", value); } }
            public string RespirationResult { get { return G("RespirationResult"); } set { S("RespirationResult", value); } }
            public string SPO2 { get { return G("SPO2"); } set { S("SPO2", value); } }
        }
    }
}
