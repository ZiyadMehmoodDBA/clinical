using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSCharge : DataSet
    {
        private PatientChargesDataTable _tPatientCharges;
        private VisitChargesDataTable _tVisitCharges;
        private ChargesICDPointersDataTable _tChargesICDPointers;
        private UnClaimedAppointmentsDataTable _tUnClaimedAppointments;
        private BactheLookupssDataTable _tBactheLookups;
        private PatientAndInsuranceChargesDataTable _tPatientAndInsuranceCharges;
        private VisitStatusDataTable _tVisitStatus;

        public DSCharge()
        {
            DataSetName = "DSCharge";
            _tPatientCharges = new PatientChargesDataTable();
            _tVisitCharges = new VisitChargesDataTable();
            _tChargesICDPointers = new ChargesICDPointersDataTable();
            _tUnClaimedAppointments = new UnClaimedAppointmentsDataTable();
            _tBactheLookups = new BactheLookupssDataTable();
            _tPatientAndInsuranceCharges = new PatientAndInsuranceChargesDataTable();
            _tVisitStatus = new VisitStatusDataTable();
            Tables.Add(_tPatientCharges);
            Tables.Add(_tVisitCharges);
            Tables.Add(_tChargesICDPointers);
            Tables.Add(_tUnClaimedAppointments);
            Tables.Add(_tBactheLookups);
            Tables.Add(_tPatientAndInsuranceCharges);
            Tables.Add(_tVisitStatus);
        }

        public PatientChargesDataTable PatientCharges { get { return _tPatientCharges; } }
        public VisitChargesDataTable VisitCharges { get { return _tVisitCharges; } }
        public ChargesICDPointersDataTable ChargesICDPointers { get { return _tChargesICDPointers; } }
        public UnClaimedAppointmentsDataTable UnClaimedAppointments { get { return _tUnClaimedAppointments; } }
        public BactheLookupssDataTable BactheLookups { get { return _tBactheLookups; } }
        public PatientAndInsuranceChargesDataTable PatientAndInsuranceCharges { get { return _tPatientAndInsuranceCharges; } }
        public VisitStatusDataTable VisitStatus { get { return _tVisitStatus; } }

        public class PatientChargesDataTable : DataTable
        {
            public PatientChargesDataTable() : base("PatientCharges")
            {
                var id = new DataColumn("ChargeCapId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "AllowedAmt","AuthorizeId","BaseUnits","CLIA","CPTCode","CPTDescription","ChargeOrder",
                    "Copay","CreatedBy","CreatedOn","DOSFrom","DOSTo","DrugCodeCost","EMG","EOD","EndTime",
                    "ExpectedFee","Fee","InsCharges","IsActive","IsReportCPTDesc","LineNotes","MasterChargeId",
                    "ModifiedBy","ModifiedOn","NDC","NDCDescription","NDCMeasurCodeId","NDCUnit","NDCUnitPrice",
                    "PAN","POSCode","ParentChargeId","PatCharges","RecordCount","RiskUnits","RoundBilledUnitsId",
                    "ServiceDescription","StartTime","Status","TOSCode","TimeUnits","TotalMinutes","TotalUnits",
                    "Units","VisitId","Modifier1","Modifier2","Modifier3","Modifier4","ICDPointer1","ICDPointer2",
                    "ICDPointer3","ICDPointer4",
                    "HoldDays","ICD10Code1","ICD10Code1Description","ICD10Code2","ICD10Code2Description",
                    "ICD10Code3","ICD10Code3Description","ICD10Code4","ICD10Code4Description",
                    "ICDCode1","ICDCode1Description","ICDCode2","ICDCode2Description",
                    "ICDCode3","ICDCode3Description","ICDCode4","ICDCode4Description",
                    "IsHold","LexiCode","LexiCodeDescription","SNOMEDDescription","SNOMEDID",
                    "OldChargeCapId","StatusId","PatientId","IsPrimary",
                    "PrimaryFee","FacilityId","FacilityName","ProviderId","ProviderName",
                    "PatientInsuranceId","InsurancePlanName","EnteredByFullName",
                    "InsBalance","InsPaid","InsWriteOff","BatchNumber","SubmittedDate",
                    "SubimittedByFullName","CopayPaid","CopayDiscount","CopayBalance",
                    "PatDiscount","PatPaid","PatBalance","IsLocked","LockedOn",
                    "VNCVisitId","VNCChargesId","IsVNC","Billed","TotalBal","AssignBenefits" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientChargesRow NewPatientChargesRow() { return (PatientChargesRow)NewRow(); }
            public void AddPatientChargesRow(PatientChargesRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientChargesRow(b); }
            protected override Type GetRowType() { return typeof(PatientChargesRow); }

            public DataColumn ChargeCapIdColumn { get { return Columns["ChargeCapId"]; } }
            public DataColumn AllowedAmtColumn { get { return Columns["AllowedAmt"]; } }
            public DataColumn AuthorizeIdColumn { get { return Columns["AuthorizeId"]; } }
            public DataColumn BaseUnitsColumn { get { return Columns["BaseUnits"]; } }
            public DataColumn CLIAColumn { get { return Columns["CLIA"]; } }
            public DataColumn CPTCodeColumn { get { return Columns["CPTCode"]; } }
            public DataColumn CPTDescriptionColumn { get { return Columns["CPTDescription"]; } }
            public DataColumn ChargeOrderColumn { get { return Columns["ChargeOrder"]; } }
            public DataColumn CopayColumn { get { return Columns["Copay"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn DOSFromColumn { get { return Columns["DOSFrom"]; } }
            public DataColumn DOSToColumn { get { return Columns["DOSTo"]; } }
            public DataColumn DrugCodeCostColumn { get { return Columns["DrugCodeCost"]; } }
            public DataColumn EMGColumn { get { return Columns["EMG"]; } }
            public DataColumn EODColumn { get { return Columns["EOD"]; } }
            public DataColumn EndTimeColumn { get { return Columns["EndTime"]; } }
            public DataColumn ExpectedFeeColumn { get { return Columns["ExpectedFee"]; } }
            public DataColumn FeeColumn { get { return Columns["Fee"]; } }
            public DataColumn InsChargesColumn { get { return Columns["InsCharges"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn IsReportCPTDescColumn { get { return Columns["IsReportCPTDesc"]; } }
            public DataColumn LineNotesColumn { get { return Columns["LineNotes"]; } }
            public DataColumn MasterChargeIdColumn { get { return Columns["MasterChargeId"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn NDCColumn { get { return Columns["NDC"]; } }
            public DataColumn NDCDescriptionColumn { get { return Columns["NDCDescription"]; } }
            public DataColumn NDCMeasurCodeIdColumn { get { return Columns["NDCMeasurCodeId"]; } }
            public DataColumn NDCUnitColumn { get { return Columns["NDCUnit"]; } }
            public DataColumn NDCUnitPriceColumn { get { return Columns["NDCUnitPrice"]; } }
            public DataColumn PANColumn { get { return Columns["PAN"]; } }
            public DataColumn POSCodeColumn { get { return Columns["POSCode"]; } }
            public DataColumn ParentChargeIdColumn { get { return Columns["ParentChargeId"]; } }
            public DataColumn PatChargesColumn { get { return Columns["PatCharges"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
            public DataColumn RiskUnitsColumn { get { return Columns["RiskUnits"]; } }
            public DataColumn RoundBilledUnitsIdColumn { get { return Columns["RoundBilledUnitsId"]; } }
            public DataColumn ServiceDescriptionColumn { get { return Columns["ServiceDescription"]; } }
            public DataColumn StartTimeColumn { get { return Columns["StartTime"]; } }
            public DataColumn StatusColumn { get { return Columns["Status"]; } }
            public DataColumn TOSCodeColumn { get { return Columns["TOSCode"]; } }
            public DataColumn TimeUnitsColumn { get { return Columns["TimeUnits"]; } }
            public DataColumn TotalMinutesColumn { get { return Columns["TotalMinutes"]; } }
            public DataColumn TotalUnitsColumn { get { return Columns["TotalUnits"]; } }
            public DataColumn UnitsColumn { get { return Columns["Units"]; } }
            public DataColumn VisitIdColumn { get { return Columns["VisitId"]; } }
            public DataColumn Modifier1Column { get { return Columns["Modifier1"]; } }
            public DataColumn Modifier2Column { get { return Columns["Modifier2"]; } }
            public DataColumn Modifier3Column { get { return Columns["Modifier3"]; } }
            public DataColumn Modifier4Column { get { return Columns["Modifier4"]; } }
            public DataColumn ICDPointer1Column { get { return Columns["ICDPointer1"]; } }
            public DataColumn ICDPointer2Column { get { return Columns["ICDPointer2"]; } }
            public DataColumn ICDPointer3Column { get { return Columns["ICDPointer3"]; } }
            public DataColumn ICDPointer4Column { get { return Columns["ICDPointer4"]; } }
            public DataColumn HoldDaysColumn { get { return Columns["HoldDays"]; } }
            public DataColumn ICD10Code1Column { get { return Columns["ICD10Code1"]; } }
            public DataColumn ICD10Code1DescriptionColumn { get { return Columns["ICD10Code1Description"]; } }
            public DataColumn ICD10Code2Column { get { return Columns["ICD10Code2"]; } }
            public DataColumn ICD10Code2DescriptionColumn { get { return Columns["ICD10Code2Description"]; } }
            public DataColumn ICD10Code3Column { get { return Columns["ICD10Code3"]; } }
            public DataColumn ICD10Code3DescriptionColumn { get { return Columns["ICD10Code3Description"]; } }
            public DataColumn ICD10Code4Column { get { return Columns["ICD10Code4"]; } }
            public DataColumn ICD10Code4DescriptionColumn { get { return Columns["ICD10Code4Description"]; } }
            public DataColumn ICDCode1Column { get { return Columns["ICDCode1"]; } }
            public DataColumn ICDCode1DescriptionColumn { get { return Columns["ICDCode1Description"]; } }
            public DataColumn ICDCode2Column { get { return Columns["ICDCode2"]; } }
            public DataColumn ICDCode2DescriptionColumn { get { return Columns["ICDCode2Description"]; } }
            public DataColumn ICDCode3Column { get { return Columns["ICDCode3"]; } }
            public DataColumn ICDCode3DescriptionColumn { get { return Columns["ICDCode3Description"]; } }
            public DataColumn ICDCode4Column { get { return Columns["ICDCode4"]; } }
            public DataColumn ICDCode4DescriptionColumn { get { return Columns["ICDCode4Description"]; } }
            public DataColumn IsHoldColumn { get { return Columns["IsHold"]; } }
            public DataColumn LexiCodeColumn { get { return Columns["LexiCode"]; } }
            public DataColumn LexiCodeDescriptionColumn { get { return Columns["LexiCodeDescription"]; } }
            public DataColumn SNOMEDDescriptionColumn { get { return Columns["SNOMEDDescription"]; } }
            public DataColumn SNOMEDIDColumn { get { return Columns["SNOMEDID"]; } }
            public DataColumn OldChargeCapIdColumn { get { return Columns["OldChargeCapId"]; } }
            public DataColumn StatusIdColumn            { get { return Columns["StatusId"]; } }
            public DataColumn PatientIdColumn           { get { return Columns["PatientId"]; } }
            public DataColumn IsPrimaryColumn           { get { return Columns["IsPrimary"]; } }
            public DataColumn PrimaryFeeColumn          { get { return Columns["PrimaryFee"]; } }
            public DataColumn FacilityIdColumn          { get { return Columns["FacilityId"]; } }
            public DataColumn FacilityNameColumn        { get { return Columns["FacilityName"]; } }
            public DataColumn ProviderIdColumn          { get { return Columns["ProviderId"]; } }
            public DataColumn ProviderNameColumn        { get { return Columns["ProviderName"]; } }
            public DataColumn PatientInsuranceIdColumn  { get { return Columns["PatientInsuranceId"]; } }
            public DataColumn InsurancePlanNameColumn   { get { return Columns["InsurancePlanName"]; } }
            public DataColumn EnteredByFullNameColumn   { get { return Columns["EnteredByFullName"]; } }
            public DataColumn InsBalanceColumn          { get { return Columns["InsBalance"]; } }
            public DataColumn InsPaidColumn             { get { return Columns["InsPaid"]; } }
            public DataColumn InsWriteOffColumn         { get { return Columns["InsWriteOff"]; } }
            public DataColumn BatchNumberColumn         { get { return Columns["BatchNumber"]; } }
            public DataColumn SubmittedDateColumn       { get { return Columns["SubmittedDate"]; } }
            public DataColumn SubimittedByFullNameColumn { get { return Columns["SubimittedByFullName"]; } }
            public DataColumn CopayPaidColumn           { get { return Columns["CopayPaid"]; } }
            public DataColumn CopayDiscountColumn       { get { return Columns["CopayDiscount"]; } }
            public DataColumn CopayBalanceColumn        { get { return Columns["CopayBalance"]; } }
            public DataColumn PatDiscountColumn         { get { return Columns["PatDiscount"]; } }
            public DataColumn PatPaidColumn             { get { return Columns["PatPaid"]; } }
            public DataColumn PatBalanceColumn          { get { return Columns["PatBalance"]; } }
            public DataColumn IsLockedColumn            { get { return Columns["IsLocked"]; } }
            public DataColumn LockedOnColumn            { get { return Columns["LockedOn"]; } }
            public DataColumn VNCVisitIdColumn          { get { return Columns["VNCVisitId"]; } }
            public DataColumn VNCChargesIdColumn        { get { return Columns["VNCChargesId"]; } }
            public DataColumn IsVNCColumn               { get { return Columns["IsVNC"]; } }
            public DataColumn BilledColumn              { get { return Columns["Billed"]; } }
            public DataColumn TotalBalColumn            { get { return Columns["TotalBal"]; } }
            public DataColumn AssignBenefitsColumn      { get { return Columns["AssignBenefits"]; } }
        }

        public class PatientChargesRow : DataRow
        {
            internal PatientChargesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long     ChargeCapId      { get { var v = this["ChargeCapId"];      return v == DBNull.Value ? 0L   : Convert.ToInt64(v);   } }
            public long     VisitId          { get { var v = this["VisitId"];          return v == DBNull.Value ? 0L   : Convert.ToInt64(v);   } set { this["VisitId"]          = value.ToString(); } }
            public long     PatientId        { get { var v = this["PatientId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PatientId"] = value.ToString(); } }
            public long     OldChargeCapId   { get { var v = this["OldChargeCapId"];   return v == DBNull.Value ? 0L   : Convert.ToInt64(v);   } set { this["OldChargeCapId"]   = value.ToString(); } }
            public long     MasterChargeId   { get { var v = this["MasterChargeId"];   return v == DBNull.Value ? 0L   : Convert.ToInt64(v);   } set { this["MasterChargeId"]   = value.ToString(); } }
            public long     NDCMeasurCodeId  { get { var v = this["NDCMeasurCodeId"];  return v == DBNull.Value ? 0L   : Convert.ToInt64(v);   } set { this["NDCMeasurCodeId"]  = value.ToString(); } }
            public int      StatusId         { get { var v = this["StatusId"];         return v == DBNull.Value ? 0    : Convert.ToInt32(v);    } set { this["StatusId"]         = value.ToString(); } }
            public int      ChargeOrder      { get { var v = this["ChargeOrder"];      return v == DBNull.Value ? 0    : Convert.ToInt32(v);    } set { this["ChargeOrder"]      = value.ToString(); } }
            public double   Units            { get { var v = this["Units"];            return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["Units"]            = value.ToString(); } }
            public double   Fee              { get { var v = this["Fee"];              return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["Fee"]              = value.ToString(); } }
            public double   InsCharges       { get { var v = this["InsCharges"];       return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["InsCharges"]       = value.ToString(); } }
            public double   PatCharges       { get { var v = this["PatCharges"];       return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["PatCharges"]       = value.ToString(); } }
            public double   Copay            { get { var v = this["Copay"];            return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["Copay"]            = value.ToString(); } }
            public double   ExpectedFee      { get { var v = this["ExpectedFee"];      return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["ExpectedFee"]      = value.ToString(); } }
            public double   DrugCodeCost     { get { var v = this["DrugCodeCost"];     return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["DrugCodeCost"]     = value.ToString(); } }
            public double   NDCUnit          { get { var v = this["NDCUnit"];          return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["NDCUnit"]          = value.ToString(); } }
            public double   NDCUnitPrice     { get { var v = this["NDCUnitPrice"];     return v == DBNull.Value ? 0.0  : Convert.ToDouble(v);   } set { this["NDCUnitPrice"]     = value.ToString(); } }
            public bool     IsActive         { get { var v = this["IsActive"];         return v != DBNull.Value && Convert.ToBoolean(v);        } set { this["IsActive"]         = value.ToString(); } }
            public bool     EMG              { get { var v = this["EMG"];              return v != DBNull.Value && Convert.ToBoolean(v);        } set { this["EMG"]              = value.ToString(); } }
            public bool     IsReportCPTDesc  { get { var v = this["IsReportCPTDesc"];  return v != DBNull.Value && Convert.ToBoolean(v);        } set { this["IsReportCPTDesc"]  = value.ToString(); } }
            public bool     IsPrimary        { get { var v = this["IsPrimary"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsPrimary"] = value.ToString(); } }
            public DateTime CreatedOn        { get { var v = this["CreatedOn"];        return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public DateTime ModifiedOn       { get { var v = this["ModifiedOn"];       return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public DateTime DOSFrom          { get { var v = this["DOSFrom"];          return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["DOSFrom"]    = value.ToString(); } }
            public DateTime DOSTo            { get { var v = this["DOSTo"];            return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["DOSTo"]      = value.ToString(); } }
            public DateTime EOD              { get { var v = this["EOD"];              return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["EOD"]        = value.ToString(); } }
            public string CPTCode           { get { return G("CPTCode"); }           set { S("CPTCode",           value); } }
            public string CPTDescription    { get { return G("CPTDescription"); }    set { S("CPTDescription",    value); } }
            public string Modifier1         { get { return G("Modifier1"); }         set { S("Modifier1",         value); } }
            public string Modifier2         { get { return G("Modifier2"); }         set { S("Modifier2",         value); } }
            public string Modifier3         { get { return G("Modifier3"); }         set { S("Modifier3",         value); } }
            public string Modifier4         { get { return G("Modifier4"); }         set { S("Modifier4",         value); } }
            public string ICDPointer1       { get { return G("ICDPointer1"); }       set { S("ICDPointer1",       value); } }
            public string ICDPointer2       { get { return G("ICDPointer2"); }       set { S("ICDPointer2",       value); } }
            public string ICDPointer3       { get { return G("ICDPointer3"); }       set { S("ICDPointer3",       value); } }
            public string ICDPointer4       { get { return G("ICDPointer4"); }       set { S("ICDPointer4",       value); } }
            public string TOSCode           { get { return G("TOSCode"); }           set { S("TOSCode",           value); } }
            public string POSCode           { get { return G("POSCode"); }           set { S("POSCode",           value); } }
            public string NDC               { get { return G("NDC"); }               set { S("NDC",               value); } }
            public string NDCDescription    { get { return G("NDCDescription"); }    set { S("NDCDescription",    value); } }
            public string CLIA              { get { return G("CLIA"); }              set { S("CLIA",              value); } }
            public string PAN               { get { return G("PAN"); }               set { S("PAN",               value); } }
            public string AuthorizeId       { get { return G("AuthorizeId"); }       set { S("AuthorizeId",       value); } }
            public string LineNotes         { get { return G("LineNotes"); }         set { S("LineNotes",         value); } }
            public string StartTime         { get { return G("StartTime"); }         set { S("StartTime",         value); } }
            public string EndTime           { get { return G("EndTime"); }           set { S("EndTime",           value); } }
            public string TimeUnits         { get { return G("TimeUnits"); }         set { S("TimeUnits",         value); } }
            public string BaseUnits         { get { return G("BaseUnits"); }         set { S("BaseUnits",         value); } }
            public string RiskUnits         { get { return G("RiskUnits"); }         set { S("RiskUnits",         value); } }
            public string TotalMinutes      { get { return G("TotalMinutes"); }      set { S("TotalMinutes",      value); } }
            public string TotalUnits        { get { return G("TotalUnits"); }        set { S("TotalUnits",        value); } }
            public string RoundBilledUnitsId { get { return G("RoundBilledUnitsId"); } set { S("RoundBilledUnitsId", value); } }
            public string Status            { get { return G("Status"); }            set { S("Status",            value); } }
            public string CreatedBy         { get { return G("CreatedBy"); }         set { S("CreatedBy",         value); } }
            public string ModifiedBy        { get { return G("ModifiedBy"); }        set { S("ModifiedBy",        value); } }
            public string RecordCount       { get { return G("RecordCount"); }       set { S("RecordCount",       value); } }
        }

        public class VisitChargesDataTable : DataTable
        {
            public VisitChargesDataTable() : base("VisitCharges")
            {
                Columns.Add(new DataColumn("VisitChargeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "VisitId", "ChargeCapId", "RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VisitChargesRow NewVisitChargesRow() { return (VisitChargesRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VisitChargesRow(b); }
            protected override Type GetRowType() { return typeof(VisitChargesRow); }

            public DataColumn VisitChargeIdColumn { get { return Columns["VisitChargeId"]; } }
            public DataColumn VisitIdColumn { get { return Columns["VisitId"]; } }
            public DataColumn ChargeCapIdColumn { get { return Columns["ChargeCapId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class VisitChargesRow : DataRow
        {
            internal VisitChargesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long VisitChargeId { get { var v = this["VisitChargeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string VisitId { get { return G("VisitId"); } set { S("VisitId", value); } }
            public string ChargeCapId { get { return G("ChargeCapId"); } set { S("ChargeCapId", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class ChargesICDPointersDataTable : DataTable
        {
            public ChargesICDPointersDataTable() : base("ChargesICDPointers")
            {
                Columns.Add(new DataColumn("ICDPointerId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "ChargeCapId", "ICDPointerId", "Order", "PVICDId" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public ChargesICDPointersRow NewChargesICDPointersRow() { return (ChargesICDPointersRow)NewRow(); }
            public void AddChargesICDPointersRow(ChargesICDPointersRow row) { Rows.Add(row); }
            public System.Collections.Generic.IEnumerator<ChargesICDPointersRow> GetEnumerator() { foreach (DataRow r in Rows) yield return (ChargesICDPointersRow)r; }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new ChargesICDPointersRow(b); }
            protected override Type GetRowType() { return typeof(ChargesICDPointersRow); }

            public DataColumn ICDPointerIdColumn { get { return Columns["ICDPointerId"]; } }
            public DataColumn ChargeCapIdColumn { get { return Columns["ChargeCapId"]; } }
            public DataColumn OrderColumn { get { return Columns["Order"]; } }
            public DataColumn PVICDIdColumn { get { return Columns["PVICDId"]; } }
        }

        public class ChargesICDPointersRow : DataRow
        {
            internal ChargesICDPointersRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long ICDPointerId { get { var v = this["ICDPointerId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long ChargeCapId { get { var v = this["ChargeCapId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["ChargeCapId"] = value.ToString(); } }
            public int  Order       { get { var v = this["Order"];       return v == DBNull.Value ? 0  : Convert.ToInt32(v);  } set { this["Order"]       = value.ToString(); } }
            public long PVICDId     { get { var v = this["PVICDId"];     return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PVICDId"]     = value.ToString(); } }
        }

        public class UnClaimedAppointmentsDataTable : DataTable
        {
            public UnClaimedAppointmentsDataTable() : base("UnClaimedAppointments")
            {
                Columns.Add(new DataColumn("AppointmentId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "PatientId", "AppointmentDate", "ProviderId", "RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public UnClaimedAppointmentsRow NewUnClaimedAppointmentsRow() { return (UnClaimedAppointmentsRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new UnClaimedAppointmentsRow(b); }
            protected override Type GetRowType() { return typeof(UnClaimedAppointmentsRow); }

            public DataColumn AppointmentIdColumn { get { return Columns["AppointmentId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn AppointmentDateColumn { get { return Columns["AppointmentDate"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class UnClaimedAppointmentsRow : DataRow
        {
            internal UnClaimedAppointmentsRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long AppointmentId { get { var v = this["AppointmentId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string PatientId { get { return G("PatientId"); } set { S("PatientId", value); } }
            public string AppointmentDate { get { return G("AppointmentDate"); } set { S("AppointmentDate", value); } }
            public string ProviderId { get { return G("ProviderId"); } set { S("ProviderId", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class BactheLookupssDataTable : DataTable
        {
            public BactheLookupssDataTable() : base("BactheLookups")
            {
                Columns.Add(new DataColumn("BatchLookupId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "LookupName", "LookupValue", "IsActive", "BatchId", "BatchNumber" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public BactheLookupssRow NewBactheLookupssRow() { return (BactheLookupssRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new BactheLookupssRow(b); }
            protected override Type GetRowType() { return typeof(BactheLookupssRow); }

            public DataColumn BatchLookupIdColumn { get { return Columns["BatchLookupId"]; } }
            public DataColumn LookupNameColumn { get { return Columns["LookupName"]; } }
            public DataColumn LookupValueColumn { get { return Columns["LookupValue"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn BatchIdColumn { get { return Columns["BatchId"]; } }
            public DataColumn BatchNumberColumn { get { return Columns["BatchNumber"]; } }
        }

        public class BactheLookupssRow : DataRow
        {
            internal BactheLookupssRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long BatchLookupId { get { var v = this["BatchLookupId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string LookupName { get { return G("LookupName"); } set { S("LookupName", value); } }
            public string LookupValue { get { return G("LookupValue"); } set { S("LookupValue", value); } }
            public string IsActive { get { return G("IsActive"); } set { S("IsActive", value); } }
        }

        public class PatientAndInsuranceChargesDataTable : DataTable
        {
            public PatientAndInsuranceChargesDataTable() : base("PatientAndInsuranceCharges")
            {
                Columns.Add(new DataColumn("PatientAndInsuranceChargeId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 });
                foreach (var c in new[] { "ChargeCapId", "PatCharges", "InsCharges", "RecordCount" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public PatientAndInsuranceChargesRow NewPatientAndInsuranceChargesRow() { return (PatientAndInsuranceChargesRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new PatientAndInsuranceChargesRow(b); }
            protected override Type GetRowType() { return typeof(PatientAndInsuranceChargesRow); }

            public DataColumn PatientAndInsuranceChargeIdColumn { get { return Columns["PatientAndInsuranceChargeId"]; } }
            public DataColumn ChargeCapIdColumn { get { return Columns["ChargeCapId"]; } }
            public DataColumn PatChargesColumn { get { return Columns["PatCharges"]; } }
            public DataColumn InsChargesColumn { get { return Columns["InsCharges"]; } }
            public DataColumn RecordCountColumn { get { return Columns["RecordCount"]; } }
        }

        public class PatientAndInsuranceChargesRow : DataRow
        {
            internal PatientAndInsuranceChargesRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long PatientAndInsuranceChargeId { get { var v = this["PatientAndInsuranceChargeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public string ChargeCapId { get { return G("ChargeCapId"); } set { S("ChargeCapId", value); } }
            public string PatCharges { get { return G("PatCharges"); } set { S("PatCharges", value); } }
            public string InsCharges { get { return G("InsCharges"); } set { S("InsCharges", value); } }
            public string RecordCount { get { return G("RecordCount"); } set { S("RecordCount", value); } }
        }

        public class VisitStatusDataTable : DataTable
        {
            public VisitStatusDataTable() : base("VisitStatus")
            {
                var id = new DataColumn("VStatusId", typeof(int)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] { "ShortName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public VisitStatusRow NewVisitStatusRow() { return (VisitStatusRow)NewRow(); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new VisitStatusRow(b); }
            protected override Type GetRowType() { return typeof(VisitStatusRow); }

            public DataColumn VStatusIdColumn { get { return Columns["VStatusId"]; } }
            public DataColumn ShortNameColumn { get { return Columns["ShortName"]; } }
        }

        public class VisitStatusRow : DataRow
        {
            internal VisitStatusRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public int VStatusId { get { var v = this["VStatusId"]; return v == DBNull.Value ? 0 : Convert.ToInt32(v); } }
            public string ShortName { get { return G("ShortName"); } set { S("ShortName", value); } }
        }
    }
}
