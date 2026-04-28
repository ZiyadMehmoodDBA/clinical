using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Utilities;
namespace EDIParser.Professional
{
    public class EDI277Parser
    {
        //private long AcceptedClaims = 0;
        //private long RejectedClaims = 0;
        //private decimal AcceptedCharges = 0M;
        //private decimal RejectedCharges = 0M;
        private DS277 ds277Parser = new DS277();
        private StringBuilder FinalStr277;
        private DS277.EDI277HeaderRow rowHeader;
        private DS277.EDI277NamesRow rowName;
        private DS277.EDI277StatusRow rowStatus;
        private DS277.EDI277ServiceLineRow rowServiceLine;
        private DS277.EDI277StaticsRow rowStatics;

        private DataSet dsCodes = new DataSet();
        private string ClaimStatusCategoryCodeTable = string.Empty;
        private string ClaimStatusCodeTable = string.Empty;
        private string CodeColumn = string.Empty;
        private string DescriptionColumn = string.Empty;

        private long NameId = 0;
        private long ServiceId = 0;
        private bool IsNameItem = true;
        private bool IsSTC = false;


        public string GetHumanReadable277(string EDI277, ref DS277 ds, DataSet dsCodes, string ClaimStatusCategoryCodeTable, string ClaimStatusCodeTable, string Code, string Description)
        {
            string str;
            try
            {
                MDVUtility.SetDelimiters(EDI277);
                EDI277 = EDI277.Replace("\r\n", "");
                if ((EDI277.Trim().Length > 0) && EDICommon.IsEDI(EDI277))
                {
                    this.ClaimStatusCategoryCodeTable = ClaimStatusCategoryCodeTable;
                    this.ClaimStatusCodeTable = ClaimStatusCodeTable;
                    this.CodeColumn = Code;
                    this.DescriptionColumn = Description;
                    this.dsCodes = dsCodes;
                    this.ds277Parser = ds;
                    this.rowHeader = this.ds277Parser.EDI277Header.NewEDI277HeaderRow();
                    this.ds277Parser.EDI277Header.Rows.Add(this.rowHeader);
                    this.FinalStr277 = new StringBuilder();
                    this.Parse277(EDI277);
                    return this.FinalStr277.ToString();
                }
                str = "Not an EDI : " + EDI277;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private void Parse277(string StrIn)
        {
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                for (int i = 0; i < (strArray.Length - 2); i++)
                {
                    switch (strArray[i].Substring(0, strArray[i].IndexOf(MDVUtility.D_ELMT)))
                    {
                        case "ISA":
                            this.FinalStr277.Append("ISA ##########################################################################");
                            this.FinalStr277.Append(this.ISA(strArray[i]));
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "IEA":
                            this.FinalStr277.Append("IEA ##########################################################################");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "GS":
                            this.FinalStr277.Append("GS  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                            this.FinalStr277.Append(this.GS(strArray[i]));
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "GE":
                            this.FinalStr277.Append("GE  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "ST":
                            this.FinalStr277.Append("ST  **************************************************************************");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "SE":
                            this.FinalStr277.Append("SE  **************************************************************************");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "HL":
                            this.FinalStr277.Append(this.HL(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "NM1":
                            this.FinalStr277.Append(this.NM1(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "PER":
                            this.FinalStr277.Append(this.PER(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "TRN":
                            this.FinalStr277.Append(this.TRN(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "STC":
                            this.FinalStr277.Append(this.STC(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "QTY":
                            this.FinalStr277.Append(this.QTY(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "AMT":
                            this.FinalStr277.Append(this.AMT(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "SVC":
                            this.FinalStr277.Append("SVC  ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
                            this.FinalStr277.Append(this.SVC(strArray[i]));
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "REF":
                            this.FinalStr277.Append(this.REF(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        case "DTP":
                            this.FinalStr277.Append(this.DTP(strArray[i]));
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;

                        default:
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            this.FinalStr277.Append(strArray[i]);
                            this.FinalStr277.Append("----------------------------------------------------------------");
                            this.FinalStr277.Append(Environment.NewLine);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private string ISA(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "ISA")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 13, 15 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 13:
                                    {
                                        // this.rowHeader.ControlNumber = arrElmt[ElementNo];
                                    }
                                    break;
                                case 15:
                                    {
                                        this.rowHeader.TorP = arrElmt[ElementNo];
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GS(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "GS")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 4 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 4:
                                    {
                                        this.rowHeader.ReportDate = MDVUtility.ToStr(MDVUtility.StringToDate(arrElmt[ElementNo]).ToShortDateString());
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DS277.EDI277NamesRow GetNew277NameRow(string Code = "")
        {
            DS277.EDI277NamesRow row2;
            try
            {
                DS277.EDI277NamesRow row = this.ds277Parser.EDI277Names.NewEDI277NamesRow();
                if (Code == "PT" && !this.rowName.IsParentNameIdNull() && this.rowName.ParentNameId != 0)
                    row.ParentNameId = this.rowName.ParentNameId;
                else if (Code == "PT")
                    row.ParentNameId = this.NameId;
                else
                    row.ParentNameId = 0;

                this.ds277Parser.EDI277Names.Rows.Add(row);
                this.NameId = Convert.ToInt64(row.EDI277NameId);
                row2 = row;

                this.IsNameItem = true;
                this.IsSTC = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row2;
        }

        private DS277.EDI277StatusRow GetNew277StatusRow(long NameId = 0, long ServiceId = 0)
        {
            DS277.EDI277StatusRow row2;
            try
            {
                DS277.EDI277StatusRow row = this.ds277Parser.EDI277Status.NewEDI277StatusRow();
                row.EDI277NameId = NameId;
                row.EDI277ServiceLineId = ServiceId;
                this.ds277Parser.EDI277Status.Rows.Add(row);
                row2 = row;
                this.IsSTC = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row2;
        }

        private DS277.EDI277ServiceLineRow GetNew277ServiceLineRow()
        {
            DS277.EDI277ServiceLineRow row2;
            try
            {
                DS277.EDI277ServiceLineRow row = this.ds277Parser.EDI277ServiceLine.NewEDI277ServiceLineRow();
                row.EDI277NameId = this.NameId;
                this.ds277Parser.EDI277ServiceLine.Rows.Add(row);
                this.ServiceId = Convert.ToInt64(row.EDI277ServiceLineId);
                row2 = row;
                this.IsNameItem = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row2;
        }

        private DS277.EDI277StaticsRow GetNew277EDI277StaticsRow()
        {
            DS277.EDI277StaticsRow row2;
            try
            {
                DS277.EDI277StaticsRow row = this.ds277Parser.EDI277Statics.NewEDI277StaticsRow();
                this.ds277Parser.EDI277Statics.Rows.Add(row);
                row.AcceptedCharges = 0;
                row.RejectedCharges = 0;
                row.AcceptedCount = 0;
                row.RejectedCount = 0;
                row2 = row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row2;
        }

        private void RecordProviderStatics(long AcceptedCount, long RejectedCount, double AccepetedCharges, double RejectedCharges, string NPI, string Name)
        {
            DataRow[] drs = this.ds277Parser.EDI277Statics.Select("" + ds277Parser.EDI277Statics.NPIColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( NPI) + " AND " + ds277Parser.EDI277Statics.NameColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(Name) );
            if (drs.Count() > 0)
            {
                DS277.EDI277StaticsRow dr = (DS277.EDI277StaticsRow)drs[0];
                dr.AcceptedCharges += AccepetedCharges;
                dr.RejectedCharges += RejectedCharges;
                dr.AcceptedCount += AcceptedCount;
                dr.RejectedCount += RejectedCount;
            }
            else
            {
                this.rowStatics.Name = Name;
                this.rowStatics.NPI = NPI;
                this.rowStatics.AcceptedCharges += AccepetedCharges;
                this.rowStatics.RejectedCharges += RejectedCharges;
                this.rowStatics.AcceptedCount += AcceptedCount;
                this.rowStatics.RejectedCount += RejectedCount;
            }
        }

        #region " Segments "
        private string HL(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "HL")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 3 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(Environment.NewLine);
                                        StrOut.Append(arrElmt[ElementNo]);
                                        StrOut.Append("  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                                    }
                                    break;
                                case 3:
                                    {
                                        StrOut.Append(Environment.NewLine);
                                        StrOut.Append("Hierarchical Level: ");
                                        StrOut.Append(HL03(arrElmt[ElementNo]));
                                    }
                                    break;
                                default:
                                    {
                                        StrOut.Append(Environment.NewLine);
                                        StrOut.Append("## this HL segment not in use ##");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string HL03(string Code)
        {
            string StrOut = "Unknown";
            switch (Code)
            {
                case "20":
                    StrOut = "Information Source"; break;
                case "21":
                    StrOut = "Information Receiver"; break;
                case "19":
                    StrOut = "Information Service Provider"; break;
                case "22":
                    StrOut = "Subscriber"; break;
                case "23":
                    StrOut = "Dependent"; break;
                case "PT":
                    StrOut = "Patient"; break;

                default:
                    StrOut = "### Unknown HL03 ###"; break;
            }
            string[] codes = { "20", "21", "22", "23", "19", "PT" };
            if (codes.Contains(Code))
            {
                this.rowName = this.GetNew277NameRow(Code);
            }

            if (Code == "19" || Code == "21")
                this.rowStatics = GetNew277EDI277StaticsRow();

            return StrOut;
        }

        private string NM1(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "NM1")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 6, 3, 4, 5, 7, 8, 9, 10, 11, 12 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                string strOut = this.NM101(strArray[num2]);
                                this.rowName.NM101 = strOut;
                                this.rowName.NM101_QUL = strArray[num2];
                                builder.Append(strOut);
                                builder.Append(": ");
                                break;
                            }
                        case 3:
                            {
                                if (this.rowName.NM101_QUL == "41")
                                    this.rowHeader.SubmitterName = strArray[num2];
                                else if (this.rowName.NM101_QUL == "AY" || this.rowName.NM101_QUL == "PR")
                                    this.rowHeader.ReceiverName = strArray[num2];

                                this.rowName.NM103 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(": ");

                                break;
                            }
                        case 8:
                            {
                                builder.Append("\r\n");
                                string strOut = this.NM108(strArray[num2]);
                                this.rowName.NM108 = strOut;
                                builder.Append(strOut);
                                builder.Append(": ");
                                break;
                            }
                        case 9:
                            {
                                if (this.rowName.NM101_QUL == "41")
                                    this.rowHeader.SubmitterID = strArray[num2];

                                this.rowName.NM109 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(": ");
                                break;
                            }
                        default:
                            builder.Append(strArray[num2]);
                            builder.Append(" ");
                            break;
                    }
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string NM101(string Code)
        {
            string str = "";
            switch (Code)
            {
                case "QC":
                    str = "Patient"; break;

                case "IL":
                    str = "Insured or Subscriber"; break;

                case "74":
                    str = "Corrected Insured"; break;

                case "82":
                    str = "Rendering Provider"; break;

                case "85":
                    str = "Billing Provider"; break;

                case "41":
                    str = "Submitter"; break;

                case "1P":
                    str = "Provider"; break;

                case "TT":
                    str = "Transfer To"; break;

                case "PR":
                    str = "Payer"; break;

                case "AY":
                    str = "Clearinghouse"; break;
            }

            if (str == "")
                str = "### Unknown NM101 ###";

            return str;
        }

        private string NM102(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Person";

                case "2":
                    return "Non-Person Entity";
            }
            return "### Unknown NM102 ###";
        }

        private string NM108(string Code)
        {
            switch (Code)
            {
                case "34":
                    return "Social Security Number";

                case "46":
                    return "Electronic Transmitter Identification Number (ETIN)";

                case "HN":
                    return "HICN";

                case "II":
                    return "Standard Unique Health Identifier for each Individual in the United States";

                case "MI":
                    return "Member Identification Number";

                case "MR":
                    return "Medicaid Recipient Identification Number";

                case "C":
                    return "Insured’s Changed Unique Identification Number";

                case "BD":
                    return "Blue Cross Provider Number";

                case "BS":
                    return "Blue Shield Provider Number";

                case "FI":
                    return "Federal Taxpayer’s Identification Number";

                case "MC":
                    return "Medicaid Provider Number";

                case "PC":
                    return "Provider Commercial Number";

                case "SL":
                    return "State License Number";

                case "UP":
                    return "Unique Physician Identification Number (UPIN)";

                case "XX":
                    return "National Provider Identifier";

                case "AD":
                    return "Blue Cross Blue Shield Association Plan Code";

                case "NI":
                    return "National Association of Insurance Commissioners (NAIC) Identification";

                case "PI":
                    return "Payor Identification";

                case "PP":
                    return "Pharmacy Processor Number";

                case "XV":
                    return "Centers for Medicare and Medicaid Services PlanID";

                case "ZZ":
                    return "Mutually Defined";
            }
            return "### Unknown NM108 ###";
        }

        private string PER(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "PER")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 7, 8, 9 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                string strOut = this.PER101(strArray[num2]);
                                this.rowName.PER01 = strOut;
                                builder.Append(strOut);
                                builder.Append(": ");
                                break;
                            }
                        case 2:
                            {
                                this.rowName.PER02 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(": ");
                                break;
                            }
                        case 3:
                            {
                                string strOut = this.PER103(strArray[num2]);
                                this.rowName.PER03 = strOut;
                                builder.Append(strOut);
                                builder.Append(": ");
                                break;
                            }
                        case 4:
                            this.rowName.PER04 = strArray[num2];
                            builder.Append(strArray[num2]);
                            builder.Append(": ");
                            break;
                        case 5:
                            string strOut5 = this.GetPERQULER(strArray[num2]);
                            this.rowName.PER05 = strOut5;
                            builder.Append(strOut5);
                            builder.Append(": ");
                            break;
                        case 6:
                            this.rowName.PER06 = strArray[num2];
                            builder.Append(strArray[num2]);
                            builder.Append(": ");
                            break;
                        case 7:
                            string strOut7 = this.GetPERQULER(strArray[num2]);
                            this.rowName.PER07 = strOut7;
                            builder.Append(strOut7);
                            builder.Append(": ");
                            break;
                        case 8:
                            this.rowName.PER08 = strArray[num2];
                            builder.Append(strArray[num2]);
                            builder.Append(": ");
                            break;
                        case 9:
                            this.rowName.PER09 = strArray[num2];
                            builder.Append(strArray[num2]);
                            builder.Append(": ");
                            break;

                        default:
                            builder.Append(strArray[num2]);
                            builder.Append(" ");
                            break;
                    }
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetPERQULER(string Code)
        {
            switch (Code)
            {
                case "EX":
                    return "Telephone Extension";
                case "FX":
                    return "Facsimile";
            }
            return "### Unknown PERQUAL ###";
        }

        private string PER103(string Code)
        {
            switch (Code)
            {
                case "ED":
                    return "Electronic Data Interchange Access Number";
                case "EM":
                    return "Electronic Mail";
                case "TE":
                    return "Telephone";
            }
            return "### Unknown PER103 ###";
        }

        private string PER101(string Code)
        {
            switch (Code)
            {
                case "IC":
                    return "Information Contact";
            }
            return "### Unknown PER101 ###";
        }

        private string TRN(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "TRN")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2, 4 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(TRN01(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    StrOut.Append(arrElmt[ElementNo]);
                                    if (!this.IsSTC && this.rowName != null)
                                        this.rowName.TRN02 = arrElmt[ElementNo];
                                    else if (this.IsSTC && this.rowStatus != null)
                                        this.rowStatus.TRN02 = arrElmt[ElementNo];

                                    if (this.rowName.NM101_QUL == "41")
                                        this.rowHeader.ControlNumber = arrElmt[ElementNo];

                                    break;
                                case 4:
                                    {
                                        StrOut.Append("\t");
                                        StrOut.Append(arrElmt[ElementNo]);
                                        if (!this.IsSTC && this.rowName != null)
                                            this.rowName.TRN04 = arrElmt[ElementNo];
                                        else if (this.IsSTC && this.rowStatus != null)
                                            this.rowStatus.TRN04 = arrElmt[ElementNo];
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string TRN01(string Code)
        {
            string StrOut = "Unknown";
            switch (Code)
            {
                case "1":
                    StrOut = "Current Transaction Trace Numbers"; break;
                case "2":
                    StrOut = "Referenced Transaction Trace Numbers"; break;

                default:
                    StrOut = "### Unknown TRN01 ###"; break;
            }

            if (!this.IsSTC && this.rowName != null)
                this.rowName.TRN01 = StrOut;
            else if (this.IsSTC && this.rowStatus != null)
                this.rowStatus.TRN01 = StrOut;

            return StrOut;
        }

        private string STC(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "STC")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(STC01(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        string strOut = MDVUtility.StringToDate(arrElmt[ElementNo]).ToShortDateString();
                                        this.rowStatus.STC02 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 3:
                                    {
                                        StrOut.Append(STC03(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 4:
                                    {
                                        string strOut = MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo]));
                                        this.rowStatus.STC04 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 5:
                                    {
                                        string strOut = MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo]));
                                        this.rowStatus.STC05 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 6:
                                    {
                                        string strOut = MDVUtility.StringToDate(arrElmt[ElementNo]).ToShortDateString();
                                        this.rowStatus.STC06 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 7:
                                    {
                                        string strOut = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        this.rowStatus.STC07 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 8:
                                    {
                                        string strOut = MDVUtility.StringToDate(arrElmt[ElementNo]).ToShortDateString();
                                        this.rowStatus.STC08 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 9:
                                    {
                                        string strOut = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        this.rowStatus.STC09 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 12:
                                    {
                                        string strOut = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        this.rowStatus.STC12 = strOut;
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string STC01(string Code)
        {
            StringBuilder StrOut = new StringBuilder();
            string[] codes = Code.Split(MDVUtility.D_S_ELMT);

            int[] order = { 0, 1, 2 };
            for (int i = 0; i < order.Length; i++)
            {
                int ElementNo = order[i];

                if (ElementNo < codes.Length)
                {
                    if (codes[ElementNo].Length > 0)
                    {
                        switch (ElementNo)
                        {
                            case 0:
                                {
                                    StrOut.Append(this.STC01_1(codes[ElementNo]));
                                    StrOut.Append(": ");
                                }
                                break;
                            case 1:
                                {
                                    StrOut.Append(this.STC01_2(codes[ElementNo]));
                                    StrOut.Append(": ");
                                }
                                break;
                            case 2:
                                {
                                    StrOut.Append(this.STC01_3(codes[ElementNo]));
                                    StrOut.Append(": ");
                                }
                                break;
                        }
                    }
                }
            }

            return StrOut.ToString();
        }

        private string STC01_1(string Code)
        {
            string strOut = "";
            DataRow[] drs = dsCodes.Tables[ClaimStatusCategoryCodeTable].Select("" + this.CodeColumn + "=" + MDVUtility.ToLINQFormatString(Code ));
            if (drs.Count() > 0)
            {
                DataRow dr = drs[0];
                strOut = MDVUtility.ToStr(dr[this.DescriptionColumn]);
            }

            if (!string.IsNullOrEmpty(strOut))
            {
                if (this.IsNameItem)
                    this.rowStatus = this.GetNew277StatusRow(NameId, 0);
                else
                    this.rowStatus = this.GetNew277StatusRow(0, ServiceId);

                this.rowStatus.STC01_1 = strOut;
                this.rowStatus.STC01_1_QUL = Code;
                return strOut;
            }
            else
                return "### Unknown STC01_1 ###";
        }

        private string STC01_2(string Code)
        {
            string strOut = "";
            DataRow[] drs = dsCodes.Tables[ClaimStatusCodeTable].Select("" + this.CodeColumn + "=" + MDVUtility.ToLINQFormatString(Code) );
            if (drs.Count() > 0)
            {
                DataRow dr = drs[0];
                strOut = MDVUtility.ToStr(dr[this.DescriptionColumn]);
            }

            this.rowStatus.STC01_2 = strOut;
            this.rowStatus.STC01_2_QUL = Code;
            return strOut;
        }

        private string STC01_3(string Code)
        {
            string strOut = "";
            switch (Code)
            {
                case "13":
                    strOut = "Contracted Service Provider"; break;
                case "17":
                    strOut = "Consultant’s Office"; break;
                case "1E":
                    strOut = "Health Maintenance Organization (HMO)"; break;
                case "1G":
                    strOut = "Oncology Center"; break;
                case "1H":
                    strOut = "Kidney Dialysis Unit"; break;
                case "1I":
                    strOut = "Preferred Provider Organization (PPO)"; break;
                case "1O":
                    strOut = "Acute Care Hospital"; break;
                case "1P":
                    strOut = "Provider"; break;
                case "1Q":
                    strOut = "Military Facility"; break;
                case "1R":
                    strOut = "University, College or School"; break;
                case "1S":
                    strOut = "Outpatient Surgicenter"; break;
                case "1T":
                    strOut = "Physician, Clinic or Group Practice"; break;
                case "1U":
                    strOut = "Long Term Care Facility"; break;
                case "1":
                    strOut = "Extended Care Facility"; break;
                case "1W":
                    strOut = "Psychiatric Health Facility"; break;
                case "1X":
                    strOut = "Laboratory"; break;
                case "1Y":
                    strOut = "Retail Pharmacy"; break;
                case "1Z":
                    strOut = "Home Health Care"; break;
                case "28":
                    strOut = "Subcontractor"; break;
                case "2A":
                    strOut = "Federal, State, County or City Facility"; break;
                case "2B":
                    strOut = "Third-Party Administrator"; break;
                case "2E":
                    strOut = "Non-Health Care Miscellaneous Facility"; break;
                case "2I":
                    strOut = "Church Operated Facility"; break;
                case "2K":
                    strOut = "Partnership"; break;
                case "2P":
                    strOut = "Public Health Service Facility"; break;
                case "2Q":
                    strOut = "Veterans Administration Facility"; break;
                case "2S":
                    strOut = "Public Health Service Indian Service Facility"; break;
                case "2Z":
                    strOut = "Hospital Unit of an Institution (prison hospital, college infirmary, etc.)"; break;
                case "30":
                    strOut = "Service Supplier"; break;
                case "36":
                    strOut = "Employer"; break;
                case "3A":
                    strOut = "Hospital Unit Within an Institution for the Mentally Retarded"; break;
                case "3C":
                    strOut = "Tuberculosis and Other Respiratory Diseases Facility"; break;
                case "3D":
                    strOut = "Obstetrics and Gynecology Facility"; break;
                case "3E":
                    strOut = "Eye, Ear, Nose and Throat Facility"; break;
                case "3F":
                    strOut = "Rehabilitation Facility"; break;
                case "3G":
                    strOut = "Orthopedic Facility"; break;
                case "3H":
                    strOut = "Chronic Disease Facility"; break;
                case "3I":
                    strOut = "Other Specialty Facility"; break;
                case "3J":
                    strOut = "Children’s General Facility"; break;
                case "3K":
                    strOut = "Children’s Hospital Unit of an Institution"; break;
                case "3L":
                    strOut = "Children’s Psychiatric Facility"; break;
                case "3M":
                    strOut = "Children’s Tuberculosis and Other Respiratory Diseases Facility"; break;
                case "3N":
                    strOut = "Children’s Eye, Ear, Nose and Throat Facility"; break;
                case "3O":
                    strOut = "Children’s Rehabilitiaion Facility"; break;
                case "3P":
                    strOut = "Children’s Orthopedic Facility"; break;
                case "3Q":
                    strOut = "Children’s Chronic Disease Facility"; break;
                case "3R":
                    strOut = "Children’s Other Specialty Facility"; break;
                case "3S":
                    strOut = "Institution for Mental Retardation"; break;
                case "3T":
                    strOut = "Alcoholism and Other Chemical Dependency Facility"; break;
                case "3U":
                    strOut = "General Inpatient Care for AIDS/ARC Facility"; break;
                case "3V":
                    strOut = "AIDS/ARC Unit"; break;
                case "3W":
                    strOut = "Specialized Outpatient Program for AIDS/ARC"; break;
                case "3X":
                    strOut = "Alcohol/Drug Abuse or Dependency Inpatient Unit"; break;
                case "3Y":
                    strOut = "Alcohol/Drug Abuse or Dependency Outpatient Services"; break;
                case "3Z":
                    strOut = "Arthritis Treatment Center"; break;
                case "40":
                    strOut = "Receiver"; break;
                case "43":
                    strOut = "Claimant Authorized Representative"; break;
                case "44":
                    strOut = "Data Processing Service Bureau"; break;
                case "4A":
                    strOut = "Birthing Room/LDRP Room"; break;
                case "3B":
                    strOut = "Burn Care Unit"; break;
                case "4C":
                    strOut = "Cardiac Catherization Laboratory"; break;
                case "4D":
                    strOut = "Open-Heart Surgery Facility"; break;
                case "4E":
                    strOut = "Cardiac Intensive Care Unit"; break;
                case "4F":
                    strOut = "Angioplasty Facility"; break;
                case "4G":
                    strOut = "Chronic Obstructive Pulmonary Disease Service Facility"; break;
                case "4H":
                    strOut = "Emergency Department"; break;
                case "4I":
                    strOut = "Trauma Center (Certified)"; break;
                case "4J":
                    strOut = "Extracorporeal Shock-Wave Lithotripter (ESWL) Un"; break;
                case "4L":
                    strOut = "Genetic Counseling/Screening Services"; break;
                case "4M":
                    strOut = "Adult Day Care Program Facility"; break;
                case "4N":
                    strOut = "Alzheimer’s Diagnostic/Assessment Services"; break;
                case "4O":
                    strOut = "Comprehensive Geriatric Assessment Facility"; break;
                case "4P":
                    strOut = "Emergency Response (Geriatric) Unit"; break;
                case "4Q":
                    strOut = "Geriatric Acute Care Unit"; break;
                case "4R":
                    strOut = "Geriatric Clinics"; break;
                case "4S":
                    strOut = "Respite Care Facility"; break;
                case "4U":
                    strOut = "Patient Education Unit"; break;
                case "4V":
                    strOut = "Community Health Promotion Facility"; break;
                case "4W":
                    strOut = "Worksite Health Promotion Facility"; break;
                case "4X":
                    strOut = "Hemodialysis Facility"; break;
                case "4Y":
                    strOut = "Home Health Services"; break;
                case "4Z":
                    strOut = "Hospice"; break;
                case "5A":
                    strOut = "Medical Surgical or Other Intensive Care Unit"; break;
                case "5B":
                    strOut = "Hisopathology Laboratory"; break;
                case "5C":
                    strOut = "Blood Bank"; break;
                case "5D":
                    strOut = "Neonatal Intensive Care Unit"; break;
                case "5E":
                    strOut = "Obstetrics Unit"; break;
                case "5F":
                    strOut = "Occupational Health Services"; break;
                case "5G":
                    strOut = "Organized Outpatient Services"; break;
                case "5H":
                    strOut = "Pediatric Acute Inpatient Unit"; break;
                case "5I":
                    strOut = "Psychiatric Child/Adolescent Services"; break;
                case "5J":
                    strOut = "Psychiatric Consultation-Liaison Services"; break;
                case "5K":
                    strOut = "Psychiatric Education Services"; break;
                case "5L":
                    strOut = "Psychiatric Emergency Services"; break;
                case "5M":
                    strOut = "Psychiatric Geriatric Services"; break;
                case "5N":
                    strOut = "Psychiatric Inpatient Unit"; break;
                case "5O":
                    strOut = "Psychiatric Outpatient Services"; break;
                case "5P":
                    strOut = "Psychiatric Partial Hospitalization Program"; break;
                case "5Q":
                    strOut = "Megavoltage Radiation Therapy Unit"; break;
                case "5R":
                    strOut = "Radioactive Implants Unit"; break;
                case "5S":
                    strOut = "Theraputic Radioisotope Facility"; break;
                case "5T":
                    strOut = "X-Ray Radiation Therapy Unit"; break;
                case "5U":
                    strOut = "CT Scanner Unit"; break;
                case "5V":
                    strOut = "Diagnostic Radioisotope Facility"; break;
                case "5W":
                    strOut = "Magnetic Resonance Imaging (MRI) Facility"; break;
                case "5X":
                    strOut = "Ultrasound Unit"; break;
                case "5Y":
                    strOut = "Rehabilitation Inpatient Unit"; break;
                case "5Z":
                    strOut = "Rehabilitation Outpatient Services"; break;
                case "61":
                    strOut = "Performed At"; break;
                case "6A":
                    strOut = "Reproductive Health Services"; break;
                case "6B":
                    strOut = "Skilled Nursing or Other Long-Term Care Unit"; break;
                case "6C":
                    strOut = "Single Photon Emission Computerized Tomography (SPECT) Unit"; break;
                case "6D":
                    strOut = "Organized Social Work Service Facility"; break;
                case "6E":
                    strOut = "Outpatient Social Work Services"; break;
                case "6F":
                    strOut = "Emergency Department Social Work Services"; break;
                case "6G":
                    strOut = "Sports Medicine Clinic/Services"; break;
                case "6H":
                    strOut = "Hospital Auxiliary Unit"; break;
                case "6I":
                    strOut = "Patient Representative Services"; break;
                case "6J":
                    strOut = "Volunteer Services Department"; break;
                case "6K":
                    strOut = "Outpatient Surgery Services"; break;
                case "6L":
                    strOut = "Organ/Tissue Transplant Unit"; break;
                case "6M":
                    strOut = "Orthopedic Surgery Facility"; break;
                case "6N":
                    strOut = "Occupational Therapy Services"; break;
                case "6O":
                    strOut = "Physical Therapy Services"; break;
                case "6P":
                    strOut = "Recreational Therapy Services"; break;
                case "6Q":
                    strOut = "Respiratory Therapy Services"; break;
                case "6R":
                    strOut = "Speech Therapy Services"; break;
                case "6S":
                    strOut = "Women’s Health Center/Services"; break;
                case "6U":
                    strOut = "Cardiac Rehabilitation Program Facility"; break;
                case "6V":
                    strOut = "Non-Invasive Cardiac Assessment Services"; break;
                case "6W":
                    strOut = "Emergency Medical Technician"; break;
                case "6X":
                    strOut = "Disciplinary Contact"; break;
                case "6Y":
                    strOut = "Case Manager"; break;
                case "71":
                    strOut = "Attending Physician"; break;
                case "72":
                    strOut = "Operating Physician"; break;
                case "73":
                    strOut = "Other Physician"; break;
                case "74":
                    strOut = "Corrected Insured"; break;
                case "77":
                    strOut = "Service Location"; break;
                case "7C":
                    strOut = "Place of Occurrence"; break;
                case "80":
                    strOut = "Hospital"; break;
                case "82":
                    strOut = "Rendering Provider"; break;
                case "84":
                    strOut = "Subscriber’s Employer"; break;
                case "85":
                    strOut = "Billing Provider"; break;
                case "87":
                    strOut = "Pay-to Provider"; break;
                case "95":
                    strOut = "Research Institute"; break;
                case "CK":
                    strOut = "Pharmacist"; break;
                case "CZ":
                    strOut = "Admitting Surgeon"; break;
                case "D2":
                    strOut = "Commercial Insurer"; break;
                case "DD":
                    strOut = "Assistant Surgeon"; break;
                case "DJ":
                    strOut = "Consulting Physician"; break;
                case "DK":
                    strOut = "Ordering Physician"; break;
                case "DN":
                    strOut = "Referring Provider"; break;
                case "DO":
                    strOut = "Dependent Name"; break;
                case "DQ":
                    strOut = "Supervising Physician"; break;
                case "E1":
                    strOut = "Person or Other Entity Legally Responsible for a Child"; break;
                case "E2":
                    strOut = "Person or Other Entity With Whom a Child Resides"; break;
                case "E7":
                    strOut = "Previous Employer"; break;
                case "E9":
                    strOut = "Participating Laboratory"; break;
                case "FA":
                    strOut = "Facility"; break;
                case "FD":
                    strOut = "Physical Address"; break;
                case "FE":
                    strOut = "Mail Address"; break;
                case "G0":
                    strOut = "Dependent Insured"; break;
                case "G3":
                    strOut = "Clinic"; break;
                case "GB":
                    strOut = "Other Insured"; break;
                case "GD":
                    strOut = "Guardian"; break;
                case "GI":
                    strOut = "Paramedic"; break;
                case "GK":
                    strOut = "Previous Insured"; break;
                case "GM":
                    strOut = "Spouse Insured"; break;
                case "GY":
                    strOut = "Treatment Facility"; break;
                case "HF":
                    strOut = "Healthcare Professional Shortage Area (HPSA) Facility"; break;
                case "HH":
                    strOut = "Home Health Agency"; break;
                case "I3":
                    strOut = "Independent Physicians Association (IPA)"; break;
                case "IJ":
                    strOut = "Injection Point"; break;
                case "IL":
                    strOut = "Insured or Subscriber"; break;
                case "IN":
                    strOut = "Insurer"; break;
                case "LI":
                    strOut = "Independent Lab"; break;
                case "LR":
                    strOut = "Legal Representative"; break;
                case "MR":
                    strOut = "Medical Insurance Carrier"; break;
                case "OB":
                    strOut = "Ordered By"; break;
                case "OD":
                    strOut = "Doctor of Optometry"; break;
                case "OX":
                    strOut = "Oxygen Therapy Facility"; break;
                case "P0":
                    strOut = "Patient Facility"; break;
                case "P2":
                    strOut = "Primary Insured or Subscriber"; break;
                case "P3":
                    strOut = "Primary Care Provider"; break;
                case "P4":
                    strOut = "Prior Insurance Carrier"; break;
                case "P6":
                    strOut = "Third Party Reviewing Preferred Provider Organization (PPO)"; break;
                case "P7":
                    strOut = "Third Party Repricing Preferred Provider Organization (PPO)"; break;
                case "PT":
                    strOut = "Party to Receive Test Report"; break;
                case "PV":
                    strOut = "Party performing certification"; break;
                case "PW":
                    strOut = "Pick Up Address"; break;
                case "QA":
                    strOut = "Pharmacy"; break;
                case "QB":
                    strOut = "Purchase Service Provider"; break;
                case "QC":
                    strOut = "Patient"; break;
                case "QD":
                    strOut = "Responsible Party"; break;
                case "QE":
                    strOut = "Policyholder"; break;
                case "QH":
                    strOut = "Physician"; break;
                case "QK":
                    strOut = "Managed Care"; break;
                case "QL":
                    strOut = "Chiropractor"; break;
                case "QN":
                    strOut = "Dentist"; break;
                case "QO":
                    strOut = "Doctor of Osteopathy"; break;
                case "QS":
                    strOut = "Podiatrist"; break;
                case "QV":
                    strOut = "Group Practice"; break;
                case "QY":
                    strOut = "Medical Doctor"; break;
                case "RC":
                    strOut = "Receiving Location"; break;
                case "RW":
                    strOut = "Rural Health Clinic"; break;
                case "S4":
                    strOut = "Skilled Nursing Facility"; break;
                case "SJ":
                    strOut = "Service Provider"; break;
                case "SU":
                    strOut = "Supplier/Manufacturer"; break;
                case "T4":
                    strOut = "Transfer Point Used to identify the geographic location where a patient is transferred or deverted."; break;
                case "TQ":
                    strOut = "Third Party Reviewing Organization (TPO)"; break;
                case "TT":
                    strOut = "Transfer To"; break;
                case "TU":
                    strOut = "Third Party Repricing Organization (TPO)"; break;
                case "UH":
                    strOut = "Nursing Home"; break;
                case "X3":
                    strOut = "Utilization Management Organization"; break;
                case "X4":
                    strOut = "Spouse"; break;
                case "X5":
                    strOut = "Durable Medical Equipment Supplier"; break;
                case "ZZ":
                    strOut = "Mutually Defined"; break;
                case "PR":
                    strOut = "Payer"; break;
                case "MSC":
                    strOut = "Mammography Screening Center"; break;
                default:
                    strOut = "### Unknown STC01_3 ###";
                    break;
            }

            this.rowStatus.STC01_3 = strOut;
            this.rowStatus.STC01_3_QUL = Code;
            return strOut;
        }

        private string STC03(string Code)
        {

            string strOut = "";

            switch (Code)
            {
                case "U":
                    strOut = "Reject";
                    break;
                case "WQ":
                    strOut = "Accept";
                    break;
                default:
                    strOut = "### Unknown STC03 ###";
                    break;
            }

            this.rowStatus.STC03 = strOut;
            return strOut;
        }

        private string QTY(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "QTY")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        string strOut = QTY01(arrElmt[ElementNo]);
                                        this.rowName.QTY01 = arrElmt[ElementNo];
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        this.rowName.QTY02 = arrElmt[ElementNo];
                                        if (this.rowName.QTY01 == "90" && this.rowName.NM101_QUL == "41")
                                        {
                                            this.rowStatics.Name = this.rowName.NM101_QUL;
                                            this.rowStatics.AcceptedCount = MDVUtility.ToLong(arrElmt[ElementNo]);
                                        }
                                        else if (this.rowName.QTY01 == "AA" && this.rowName.NM101_QUL == "41")
                                        {
                                            this.rowStatics.Name = this.rowName.NM101_QUL;
                                            this.rowStatics.RejectedCount = MDVUtility.ToLong(arrElmt[ElementNo]);
                                        }
                                        else if (this.rowName.QTY01 == "QA" && this.rowName.NM101_QUL == "85")
                                        {
                                            this.RecordProviderStatics(MDVUtility.ToLong(arrElmt[ElementNo]), 0, 0, 0, this.rowName.NM109, this.rowName.NM101_QUL);
                                            //this.rowStatics.Name = this.rowName.NM101_QUL;
                                            //this.rowStatics.NPI = this.rowName.NM109;
                                            //this.rowStatics.AcceptedCount = arrElmt[ElementNo];
                                        }
                                        else if (this.rowName.QTY01 == "QC" && this.rowName.NM101_QUL == "85")
                                        {
                                            this.RecordProviderStatics(0, MDVUtility.ToLong(arrElmt[ElementNo]), 0, 0, this.rowName.NM109, this.rowName.NM101_QUL);
                                            //this.rowStatics.Name = this.rowName.NM101_QUL;
                                            //this.rowStatics.NPI = this.rowName.NM109;
                                            //this.rowStatics.RejectedCount = arrElmt[ElementNo];
                                        }

                                        StrOut.Append(arrElmt[ElementNo]);
                                        StrOut.Append(": ");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string QTY01(string Code)
        {
            string strOut = "";
            switch (Code)
            {
                case "90":
                    strOut = "Acknowledged Quantity";
                    break;
                case "AA":
                    strOut = "Unacknowledged Quantity";
                    break;
                case "QA":
                    strOut = "Quantity Approved";
                    break;
                case "QC":
                    strOut = "Quantity Disapproved";
                    break;

                default:
                    strOut = "### Unknown QTY01 ###";
                    break;
            }

            return strOut;
        }

        private string AMT(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "AMT")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        string strOut = AMT01(arrElmt[ElementNo]);
                                        this.rowName.AMT01 = arrElmt[ElementNo];
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        this.rowName.AMT02 = MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo]));
                                        StrOut.Append(MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo])));
                                        StrOut.Append(": ");

                                        if (this.rowName.AMT01 == "YU" && this.rowName.NM101_QUL == "41")
                                        {
                                            this.rowStatics.Name = this.rowName.NM101_QUL;
                                            this.rowStatics.AcceptedCharges = MDVUtility.ToDouble(arrElmt[ElementNo]);
                                        }
                                        else if (this.rowName.AMT01 == "YY" && this.rowName.NM101_QUL == "41")
                                        {
                                            this.rowStatics.Name = this.rowName.NM101_QUL;
                                            this.rowStatics.RejectedCharges = MDVUtility.ToDouble(arrElmt[ElementNo]);
                                        }
                                        else if (this.rowName.AMT01 == "YU" && this.rowName.NM101_QUL == "85")
                                        {
                                            this.RecordProviderStatics(0, 0, MDVUtility.ToDouble(arrElmt[ElementNo]), 0, this.rowName.NM109, this.rowName.NM101_QUL);
                                            //this.rowStatics.Name = this.rowName.NM101_QUL;
                                            //this.rowStatics.AcceptedCharges = arrElmt[ElementNo];
                                        }
                                        else if (this.rowName.AMT01 == "YY" && this.rowName.NM101_QUL == "85")
                                        {
                                            this.RecordProviderStatics(0, 0, 0, MDVUtility.ToDouble(arrElmt[ElementNo]), this.rowName.NM109, this.rowName.NM101_QUL);
                                            //this.rowStatics.Name = this.rowName.NM101_QUL;
                                            //this.rowStatics.RejectedCharges = arrElmt[ElementNo];
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string AMT01(string Code)
        {
            string strOut = "";
            switch (Code)
            {
                case "T3":
                    strOut = "Total Submitted Charges";
                    break;
                case "YY":
                    strOut = "Total rejected  amount.";
                    break;
                case "YU":
                    strOut = "Total accepted  amount.";
                    break;

                default:
                    strOut = "### Unknown AMT01 ###";
                    break;
            }

            return strOut;
        }

        private string SVC(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "SVC")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2, 3, 4, 5, 6, 7 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(SVC01(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        this.rowServiceLine.SVC02 = MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo]));
                                        StrOut.Append(MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo])));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 3:
                                    {
                                        this.rowServiceLine.SVC03 = MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo]));
                                        StrOut.Append(MDVUtility.ToStr(MDVUtility.ToDecimal(arrElmt[ElementNo])));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 4:
                                    {
                                        this.rowServiceLine.SVC04 = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        StrOut.Append(MDVUtility.ToStr(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 5:
                                    {
                                        this.rowServiceLine.SVC05 = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        StrOut.Append(MDVUtility.ToStr(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 6:
                                    {
                                        this.rowServiceLine.SVC06 = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        StrOut.Append(MDVUtility.ToStr(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 7:
                                    {
                                        this.rowServiceLine.SVC07 = MDVUtility.ToStr(arrElmt[ElementNo]);
                                        StrOut.Append(MDVUtility.ToStr(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string SVC01(string Code)
        {
            StringBuilder StrOut = new StringBuilder();
            string[] codes = Code.Split(MDVUtility.D_S_ELMT);

            int[] order = { 0, 1, 2 };
            for (int i = 0; i < order.Length; i++)
            {
                int ElementNo = order[i];

                if (ElementNo < codes.Length)
                {
                    if (codes[ElementNo].Length > 0)
                    {
                        switch (ElementNo)
                        {
                            case 0:
                                {
                                    StrOut.Append(this.SVC01_1(codes[ElementNo]));
                                    StrOut.Append(": ");
                                }
                                break;
                            case 1:
                                {
                                    this.rowServiceLine.SVC01_2 = codes[ElementNo];
                                    StrOut.Append(codes[ElementNo]);
                                    StrOut.Append(": ");
                                }
                                break;
                            case 2:
                                {
                                    this.rowServiceLine.SVC01_3 = codes[ElementNo];
                                    StrOut.Append(codes[ElementNo]);
                                    StrOut.Append(": ");
                                }
                                break;
                        }
                    }
                }
            }

            return StrOut.ToString();
        }

        private string SVC01_1(string Code)
        {
            string strOut = "";
            switch (Code)
            {
                case "AD":
                    strOut = "American Dental Association Codes"; break;
                case "CI":
                    strOut = "Common Language Equipment Identifier (CLEI)"; break;
                case "HC":
                    strOut = "Health Care Financing Administration Common Procedural Coding System (HCPCS) Codes"; break;
                case "ID":
                    strOut = "International Classification of Diseases Clinical Modification (ICD-9-CM) - Procedure"; break;
                case "IV":
                    strOut = "Home Infusion EDI Coalition (HIEC) Product/Service Code"; break;
                case "N1":
                    strOut = "National Drug Code in 4-4-2 Format"; break;
                case "N2":
                    strOut = "National Drug Code in 5-3-2 Format"; break;
                case "N3":
                    strOut = "National Drug Code in 5-4-1 Format"; break;
                case "N4":
                    strOut = "National Drug Code in 5-4-2 Format"; break;
                case "ND":
                    strOut = "National Drug Code (NDC)"; break;
                case "NH":
                    strOut = "National Health Related Item Code"; break;
                case "NU":
                    strOut = "National Uniform Billing Committee (NUBC) UB92 Codes"; break;
                case "RB":
                    strOut = "National Uniform Billing Committee (NUBC) UB82 Codes"; break;
                default:
                    strOut = "### Unknown SVC01_1 ###";
                    break;
            }

            this.rowServiceLine = this.GetNew277ServiceLineRow();
            this.rowServiceLine.SVC01_1 = strOut;
            return strOut;


        }

        private string REF(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "REF")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(REF01(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        if (!this.IsSTC && this.rowName != null)
                                            this.rowName.REF02 = arrElmt[ElementNo];
                                        else if (this.IsSTC && this.rowStatus != null)
                                            this.rowStatus.REF02 = arrElmt[ElementNo];

                                        StrOut.Append(arrElmt[ElementNo]);
                                        StrOut.Append(": ");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string REF01(string Code)
        {

            string strOut = "";
            switch (Code)
            {
                case "1K":
                    strOut = "Payor’s Claim Number"; break;
                case "BLT":
                    strOut = "Billing Type"; break;
                case "EA":
                    strOut = "Medical Record Identification Number"; break;
                case "FJ":
                    strOut = "Line Item Control Number"; break;
                case "D9":
                    strOut = "Claim Number"; break;
                default:
                    strOut = "### Unknown REF01 ###";
                    break;
            }


            if (!this.IsSTC && this.rowName != null)
                this.rowName.REF01 = strOut;
            else if (this.IsSTC && this.rowStatus != null)
                this.rowStatus.REF01 = strOut;

            return strOut;
        }

        private string DTP(string StrIn)
        {
            try
            {
                string[] arrElmt = StrIn.Split((new char[] { MDVUtility.D_ELMT }));

                if (arrElmt[0] != "DTP")
                    return String.Empty;

                StringBuilder StrOut = new StringBuilder();
                int[] order = { 1, 2, 3 };
                for (int i = 0; i < order.Length; i++)
                {
                    int ElementNo = order[i];

                    if (ElementNo < arrElmt.Length)
                    {
                        if (arrElmt[ElementNo].Length > 0)
                        {
                            switch (ElementNo)
                            {
                                case 1:
                                    {
                                        StrOut.Append(DTP01(arrElmt[ElementNo]));
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 2:
                                    {
                                        if (!this.IsSTC && this.rowName != null)
                                            this.rowName.DTP02 = arrElmt[ElementNo];
                                        else if (this.IsSTC && this.rowStatus != null)
                                            this.rowStatus.DTP02 = arrElmt[ElementNo];

                                        StrOut.Append(arrElmt[ElementNo]);
                                        StrOut.Append(": ");
                                    }
                                    break;
                                case 3:
                                    {
                                        string strOut = "";
                                        if (!this.IsSTC && this.rowName != null)
                                        {
                                            strOut = this.GetRangeDate(this.rowName.DTP02, arrElmt, ElementNo);
                                            this.rowName.DTP03 = strOut;
                                        }
                                        else if (this.IsSTC && this.rowStatus != null)
                                        {
                                            strOut = this.GetRangeDate(this.rowStatus.DTP02, arrElmt, ElementNo);
                                            this.rowStatus.DTP03 = strOut;
                                        }
                                        StrOut.Append(strOut);
                                        StrOut.Append(": ");
                                    }
                                    break;
                            }
                        }
                    }
                }

                StrOut.Append(Environment.NewLine);
                return StrOut.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string DTP01(string Code)
        {

            string strOut = "";
            switch (Code)
            {
                case "232":
                    strOut = "Claim Statement Period Start"; break;
                case "050":
                    strOut = ""; break;
                case "009":
                    strOut = ""; break;
                default:
                    strOut = "### Unknown DTP01 ###";
                    break;
            }


            if (!this.IsSTC && this.rowName != null)
                this.rowName.DTP01 = strOut;
            else if (this.IsSTC && this.rowStatus != null)
                this.rowStatus.DTP01 = strOut;

            return strOut;
        }

        private string GetRangeDate(string key, string[] StrIn, int ElementNo)
        {
            DateTime time;
            string strOut = "";

            if (key == "D8")
            {
                time = MDVUtility.StringToDate(StrIn[ElementNo]);
                strOut = time.ToShortDateString();
            }
            else if (key == "RD8")
            {
                string[] strArray2 = StrIn[ElementNo].Split(new char[] { '-' });
                for (int i = 0; i < strArray2.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            time = MDVUtility.StringToDate(StrIn[0]);
                            strOut += time.ToShortDateString();
                            break;
                        case 1:
                            strOut += "-";
                            time = MDVUtility.StringToDate(StrIn[1]);
                            strOut += time.ToShortDateString();
                            break;
                    }
                }
            }
            return strOut;

        }

        #endregion


    }


}
