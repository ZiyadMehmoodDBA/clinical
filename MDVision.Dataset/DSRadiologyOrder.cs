using System;
using System.Data;

namespace MDVision.Datasets
{
    public class DSRadiologyOrder : DataSet
    {
        private RadiologyOrderDataTable _tRadiologyOrder;
        private RadiologyOrderProblemDataTable _tRadiologyOrderProblem;
        private RadiologyOrderTestDataTable _tRadiologyOrderTest;

        public DSRadiologyOrder()
        {
            DataSetName = "DSRadiologyOrder";
            _tRadiologyOrder = new RadiologyOrderDataTable();
            _tRadiologyOrderProblem = new RadiologyOrderProblemDataTable();
            _tRadiologyOrderTest = new RadiologyOrderTestDataTable();
            Tables.Add(_tRadiologyOrder);
            Tables.Add(_tRadiologyOrderProblem);
            Tables.Add(_tRadiologyOrderTest);
        }

        public RadiologyOrderDataTable RadiologyOrder { get { return _tRadiologyOrder; } }
        public RadiologyOrderProblemDataTable RadiologyOrderProblem { get { return _tRadiologyOrderProblem; } }
        public RadiologyOrderTestDataTable RadiologyOrderTest { get { return _tRadiologyOrderTest; } }

        public class RadiologyOrderDataTable : DataTable
        {
            public RadiologyOrderDataTable() : base("RadiologyOrder")
            {
                var id = new DataColumn("RadiologyOrderId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "LabId","PatientId","ProviderId","FacilityId","FacilityTo","Status","OrderDate","OrderTime",
                    "NoteId","Comments","IncludeComments","NegationReasonId","CreatedBy","CreatedOn",
                    "ModifiedBy","ModifiedOn","IsActive","BillingTypeId","GuarantorId","PrimaryInsuraceId",
                    "SecondaryInsuraceId","TertiaryInsuraceId","RelationShipId","AssigneeId","SoapText","RecordCount",
                    "FacilityToName","Facility","LabName","ModifiedByName","OrderNo","PrimaryInsurace","Provider",
                    "RelationShip","Test","GuarantorAddress","GuarantorCity","GuarantorFirstName",
                    "GuarantorLastName","GuarantorState","GuarantorZipCode",
                    "bResultExists","bResultAcknowledged" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public RadiologyOrderRow NewRadiologyOrderRow() { return (RadiologyOrderRow)NewRow(); }
            public void AddRadiologyOrderRow(RadiologyOrderRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new RadiologyOrderRow(b); }
            protected override Type GetRowType() { return typeof(RadiologyOrderRow); }

            public DataColumn RadiologyOrderIdColumn { get { return Columns["RadiologyOrderId"]; } }
            public DataColumn LabIdColumn { get { return Columns["LabId"]; } }
            public DataColumn PatientIdColumn { get { return Columns["PatientId"]; } }
            public DataColumn ProviderIdColumn { get { return Columns["ProviderId"]; } }
            public DataColumn FacilityIdColumn { get { return Columns["FacilityId"]; } }
            public DataColumn FacilityToColumn { get { return Columns["FacilityTo"]; } }
            public DataColumn StatusColumn { get { return Columns["Status"]; } }
            public DataColumn OrderDateColumn { get { return Columns["OrderDate"]; } }
            public DataColumn OrderTimeColumn { get { return Columns["OrderTime"]; } }
            public DataColumn NoteIdColumn { get { return Columns["NoteId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn IncludeCommentsColumn { get { return Columns["IncludeComments"]; } }
            public DataColumn NegationReasonIdColumn { get { return Columns["NegationReasonId"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn BillingTypeIdColumn { get { return Columns["BillingTypeId"]; } }
            public DataColumn GuarantorIdColumn { get { return Columns["GuarantorId"]; } }
            public DataColumn PrimaryInsuraceIdColumn { get { return Columns["PrimaryInsuraceId"]; } }
            public DataColumn SecondaryInsuraceIdColumn { get { return Columns["SecondaryInsuraceId"]; } }
            public DataColumn TertiaryInsuraceIdColumn { get { return Columns["TertiaryInsuraceId"]; } }
            public DataColumn RelationShipIdColumn { get { return Columns["RelationShipId"]; } }
            public DataColumn AssigneeIdColumn { get { return Columns["AssigneeId"]; } }
            public DataColumn SoapTextColumn          { get { return Columns["SoapText"]; } }
            public DataColumn RecordCountColumn       { get { return Columns["RecordCount"]; } }
            public DataColumn FacilityToNameColumn    { get { return Columns["FacilityToName"]; } }
            public DataColumn FacilityColumn          { get { return Columns["Facility"]; } }
            public DataColumn LabNameColumn           { get { return Columns["LabName"]; } }
            public DataColumn ModifiedByNameColumn    { get { return Columns["ModifiedByName"]; } }
            public DataColumn OrderNoColumn           { get { return Columns["OrderNo"]; } }
            public DataColumn PrimaryInsuraceColumn   { get { return Columns["PrimaryInsurace"]; } }
            public DataColumn ProviderColumn          { get { return Columns["Provider"]; } }
            public DataColumn RelationShipColumn      { get { return Columns["RelationShip"]; } }
            public DataColumn TestColumn              { get { return Columns["Test"]; } }
            public DataColumn GuarantorAddressColumn  { get { return Columns["GuarantorAddress"]; } }
            public DataColumn GuarantorCityColumn     { get { return Columns["GuarantorCity"]; } }
            public DataColumn GuarantorFirstNameColumn{ get { return Columns["GuarantorFirstName"]; } }
            public DataColumn GuarantorLastNameColumn { get { return Columns["GuarantorLastName"]; } }
            public DataColumn GuarantorStateColumn    { get { return Columns["GuarantorState"]; } }
            public DataColumn GuarantorZipCodeColumn  { get { return Columns["GuarantorZipCode"]; } }
            public DataColumn bResultExistsColumn     { get { return Columns["bResultExists"]; } }
            public DataColumn bResultAcknowledgedColumn { get { return Columns["bResultAcknowledged"]; } }
        }

        public class RadiologyOrderRow : DataRow
        {
            internal RadiologyOrderRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long RadiologyOrderId { get { var v = this["RadiologyOrderId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RadiologyOrderId"] = value; } }
            public long   LabId          { get { var v = this["LabId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["LabId"]      = value.ToString(); } }
            public long   PatientId      { get { var v = this["PatientId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["PatientId"]  = value.ToString(); } }
            public long   ProviderId     { get { var v = this["ProviderId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["ProviderId"] = value.ToString(); } }
            public long   FacilityId     { get { var v = this["FacilityId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["FacilityId"] = value.ToString(); } }
            public long   FacilityTo     { get { var v = this["FacilityTo"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["FacilityTo"] = value.ToString(); } }
            public long   NoteId         { get { var v = this["NoteId"];     return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["NoteId"]     = value.ToString(); } }
            public long   AssigneeId     { get { var v = this["AssigneeId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v);  } set { this["AssigneeId"] = value.ToString(); } }
            public long   BillingTypeId      { get { var v = this["BillingTypeId"];      return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["BillingTypeId"]      = value.ToString(); } }
            public long   PrimaryInsuraceId  { get { var v = this["PrimaryInsuraceId"];  return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["PrimaryInsuraceId"]  = value.ToString(); } }
            public long   SecondaryInsuraceId{ get { var v = this["SecondaryInsuraceId"];return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["SecondaryInsuraceId"]= value.ToString(); } }
            public long   TertiaryInsuraceId { get { var v = this["TertiaryInsuraceId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["TertiaryInsuraceId"] = value.ToString(); } }
            public long   GuarantorId        { get { var v = this["GuarantorId"];        return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["GuarantorId"]        = value.ToString(); } }
            public int    RelationShipId     { get { var v = this["RelationShipId"];     return v == DBNull.Value ? 0  : Convert.ToInt32(v);  } set { this["RelationShipId"]     = value.ToString(); } }
            public bool   IsActive       { get { var v = this["IsActive"];       return v != DBNull.Value && Convert.ToBoolean(v);                             } set { this["IsActive"]       = value.ToString(); } }
            public bool   IncludeComments{ get { var v = this["IncludeComments"]; return v != DBNull.Value && Convert.ToBoolean(v);                            } set { this["IncludeComments"]= value.ToString(); } }
            public DateTime OrderDate    { get { var v = this["OrderDate"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["OrderDate"]  = value.ToString(); } }
            public DateTime CreatedOn    { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public DateTime ModifiedOn   { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
            public string Status          { get { return G("Status"); }          set { S("Status", value); } }
            public string OrderTime       { get { return G("OrderTime"); }       set { S("OrderTime", value); } }
            public string Comments        { get { return G("Comments"); }        set { S("Comments", value); } }
            public string NegationReasonId{ get { return G("NegationReasonId"); }set { S("NegationReasonId", value); } }
            public string CreatedBy       { get { return G("CreatedBy"); }       set { S("CreatedBy", value); } }
            public string ModifiedBy      { get { return G("ModifiedBy"); }      set { S("ModifiedBy", value); } }
            public string SoapText        { get { return G("SoapText"); }        set { S("SoapText", value); } }
            public string RecordCount     { get { return G("RecordCount"); }     set { S("RecordCount", value); } }
        }

        public class RadiologyOrderProblemDataTable : DataTable
        {
            public RadiologyOrderProblemDataTable() : base("RadiologyOrderProblem")
            {
                var id = new DataColumn("RadiologyOrderProblemId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "RadiologyOrderId","ProblemId","Comments","SoapText","IsActive",
                    "CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","RecordCount","ProblemName" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public RadiologyOrderProblemRow NewRadiologyOrderProblemRow() { return (RadiologyOrderProblemRow)NewRow(); }
            public void AddRadiologyOrderProblemRow(RadiologyOrderProblemRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new RadiologyOrderProblemRow(b); }
            protected override Type GetRowType() { return typeof(RadiologyOrderProblemRow); }

            public DataColumn RadiologyOrderProblemIdColumn { get { return Columns["RadiologyOrderProblemId"]; } }
            public DataColumn RadiologyOrderIdColumn { get { return Columns["RadiologyOrderId"]; } }
            public DataColumn ProblemIdColumn { get { return Columns["ProblemId"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn RecordCountColumn  { get { return Columns["RecordCount"]; } }
            public DataColumn ProblemNameColumn  { get { return Columns["ProblemName"]; } }
        }

        public class RadiologyOrderProblemRow : DataRow
        {
            internal RadiologyOrderProblemRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long RadiologyOrderProblemId { get { var v = this["RadiologyOrderProblemId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } }
            public long   RadiologyOrderId { get { var v = this["RadiologyOrderId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RadiologyOrderId"] = value.ToString(); } }
            public int    ProblemId        { get { var v = this["ProblemId"];        return v == DBNull.Value ? 0  : Convert.ToInt32(v);  } set { this["ProblemId"]        = value.ToString(); } }
            public bool   IsActive         { get { var v = this["IsActive"];         return v != DBNull.Value && Convert.ToBoolean(v);      } set { this["IsActive"]         = value.ToString(); } }
            public string Comments   { get { return G("Comments"); }   set { S("Comments", value); } }
            public string CreatedBy  { get { return G("CreatedBy"); }  set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy { get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }

        public class RadiologyOrderTestDataTable : DataTable
        {
            public RadiologyOrderTestDataTable() : base("RadiologyOrderTest")
            {
                var id = new DataColumn("RadiologyOrderTestId", typeof(long)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 };
                Columns.Add(id);
                PrimaryKey = new DataColumn[] { id };
                foreach (var c in new[] {
                    "RadiologyOrderId","CPTCode","CPTCodeDescription","CPTSNOMEDDescription","CPTSNOMEDDescription",
                    "BodySite","SpecimenId","CollectedAtId","UrgencyId","VolumeId","VolumeLength",
                    "FillerInstruction","PatientInstruction","Reason","Comments","TestDate","TestTime",
                    "IsActive","CreatedBy","CreatedOn","ModifiedBy","ModifiedOn","SoapText","CPTSNOMEDID" })
                    Columns.Add(new DataColumn(c, typeof(string)) { DefaultValue = "" });
            }
            public RadiologyOrderTestRow NewRadiologyOrderTestRow() { return (RadiologyOrderTestRow)NewRow(); }
            public void AddRadiologyOrderTestRow(RadiologyOrderTestRow row) { Rows.Add(row); }
            protected override DataRow NewRowFromBuilder(DataRowBuilder b) { return new RadiologyOrderTestRow(b); }
            protected override Type GetRowType() { return typeof(RadiologyOrderTestRow); }

            public DataColumn RadiologyOrderTestIdColumn { get { return Columns["RadiologyOrderTestId"]; } }
            public DataColumn RadiologyOrderIdColumn { get { return Columns["RadiologyOrderId"]; } }
            public DataColumn CPTCodeColumn { get { return Columns["CPTCode"]; } }
            public DataColumn CPTCodeDescriptionColumn { get { return Columns["CPTCodeDescription"]; } }
            public DataColumn CPTSNOMEDIDColumn { get { return Columns["CPTSNOMEDID"]; } }
            public DataColumn CPTSNOMEDDescriptionColumn { get { return Columns["CPTSNOMEDDescription"]; } }
            public DataColumn BodySiteColumn { get { return Columns["BodySite"]; } }
            public DataColumn SpecimenIdColumn { get { return Columns["SpecimenId"]; } }
            public DataColumn CollectedAtIdColumn { get { return Columns["CollectedAtId"]; } }
            public DataColumn UrgencyIdColumn { get { return Columns["UrgencyId"]; } }
            public DataColumn VolumeIdColumn { get { return Columns["VolumeId"]; } }
            public DataColumn VolumeLengthColumn { get { return Columns["VolumeLength"]; } }
            public DataColumn FillerInstructionColumn { get { return Columns["FillerInstruction"]; } }
            public DataColumn PatientInstructionColumn { get { return Columns["PatientInstruction"]; } }
            public DataColumn ReasonColumn { get { return Columns["Reason"]; } }
            public DataColumn CommentsColumn { get { return Columns["Comments"]; } }
            public DataColumn TestDateColumn { get { return Columns["TestDate"]; } }
            public DataColumn TestTimeColumn { get { return Columns["TestTime"]; } }
            public DataColumn IsActiveColumn { get { return Columns["IsActive"]; } }
            public DataColumn CreatedByColumn { get { return Columns["CreatedBy"]; } }
            public DataColumn CreatedOnColumn { get { return Columns["CreatedOn"]; } }
            public DataColumn ModifiedByColumn { get { return Columns["ModifiedBy"]; } }
            public DataColumn ModifiedOnColumn { get { return Columns["ModifiedOn"]; } }
            public DataColumn SoapTextColumn { get { return Columns["SoapText"]; } }
        }

        public class RadiologyOrderTestRow : DataRow
        {
            internal RadiologyOrderTestRow(DataRowBuilder rb) : base(rb) { }
            private string G(string c) { var v = this[c]; return v == DBNull.Value ? "" : (string)v; }
            private void S(string c, string v) { this[c] = v ?? ""; }
            public long   RadiologyOrderTestId { get { var v = this["RadiologyOrderTestId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RadiologyOrderTestId"] = value; } }
            public long   RadiologyOrderId { get { var v = this["RadiologyOrderId"]; return v == DBNull.Value ? 0L : Convert.ToInt64(v); } set { this["RadiologyOrderId"] = value.ToString(); } }
            public string CPTCode   { get { return G("CPTCode"); }   set { S("CPTCode", value); } }
            public bool   IsActive  { get { var v = this["IsActive"]; return v != DBNull.Value && Convert.ToBoolean(v); } set { this["IsActive"] = value.ToString(); } }
            public string CreatedBy { get { return G("CreatedBy"); } set { S("CreatedBy", value); } }
            public DateTime CreatedOn  { get { var v = this["CreatedOn"];  return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["CreatedOn"]  = value.ToString(); } }
            public string ModifiedBy{ get { return G("ModifiedBy"); } set { S("ModifiedBy", value); } }
            public DateTime ModifiedOn { get { var v = this["ModifiedOn"]; return v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v); } set { this["ModifiedOn"] = value.ToString(); } }
        }
    }
}
