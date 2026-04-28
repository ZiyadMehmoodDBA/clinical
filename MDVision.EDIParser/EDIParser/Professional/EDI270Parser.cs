namespace EDIParser.Professional
{
    using EDIParser;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class EDI270Parser
    {
        private IContainer components;
        public const string D_ELMT = "*";
        public const string D_S_ELMT = ":";
        public const string D_SGMT = "~";
        private const string Dependent = "03";
        private DS270.EDI270NamesRow DependentRow;
        private DS270.EDI270HeaderRow HeaderRow;
        private const string Payer = "PR";
        private DS270.EDI270NamesRow PayerRow;
        private const string Provider = "1P";
        private DS270.EDI270NamesRow ProviderRow;
        private const string Subscriber = "IL";
        private DS270.EDI270NamesRow SubscriberRow;

        public EDI270Parser(DS270 dsEligibility)
        {
            this.InitializeComponent();
       
           
        }

        public string Get270()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.GetHeader());
                builder.Append(this.GetPayerSegments());
                builder.Append(this.GetTrailer());
                str = builder.ToString().ToUpper();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetBHT()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("BHT*" + this.HeaderRow.BHT01 + "*");
                builder.Append(this.HeaderRow.BHT02);
                builder.Append("*");
                builder.Append(this.HeaderRow.BHT03);
                builder.Append("*");
                
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetDependentSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((this.DependentRow != null) && this.DependentRow.IsDependent)
                {
                    builder.Append(this.HL(this.ParentSubscriber.ToString(), "23", "0"));
                    builder.Append(this.GetTRN(this.DependentRow));
                    builder.Append(this.GetNM1(this.DependentRow));
                    builder.Append(this.GetN3(this.DependentRow));
                   
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetDMG(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DMG");
                builder.Append("*");
                builder.Append(row.DMG01);
                builder.Append("*");
                builder.Append(row.DMG02);
                builder.Append("*");
                builder.Append(row.DMG03);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetDTP(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(row.DTP01);
                builder.Append("*");
                builder.Append(row.DTP02);
                builder.Append("*");
                builder.Append(row.DTP03);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetEQ(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("EQ");
                builder.Append("*");
                builder.Append(row.EQ01);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetGE()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("GE*");
                builder.Append("1*");
                builder.Append(this.HeaderRow.ISA13);
                builder.Append("~");
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetGS()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("GS*" + this.HeaderRow.GS01 + "*");
                builder.Append(this.HeaderRow.GS02);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS03);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS04);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS05);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS06);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS07);
                builder.Append("*");
                builder.Append(this.HeaderRow.GS08);
                builder.Append("~");
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetHeader()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.GetISA());
                builder.Append(this.GetGS());
                builder.Append(this.GetST());
                builder.Append(this.GetBHT());
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                EDILogger.LogErrorMessage("EDI270Parser::GetHeader", exception);
                throw exception;
            }
            return str;
        }

        private string GetIEA()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("IEA*");
                builder.Append("1*");
                builder.Append(this.HeaderRow.ISA13);
                builder.Append("~");
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetISA()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ISA*" + this.HeaderRow.ISA01 + "*" + this.HeaderRow.ISA02 + "*" + this.HeaderRow.ISA03 + "*" + this.HeaderRow.ISA04 + "*" + this.HeaderRow.ISA05 + "*");
                builder.Append(this.HeaderRow.ISA06.Trim().PadRight(15));
                builder.Append("*" + this.HeaderRow.ISA07 + "*");
                builder.Append(this.HeaderRow.ISA08.Trim().PadRight(15));
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA09);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA10);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA11);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA12);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA13);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA14);
                builder.Append("*");
                builder.Append(this.HeaderRow.ISA15.ToUpper());
                builder.Append("*:~");
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetN3(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("N3");
                builder.Append("*");
                builder.Append(row.N301);
              
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetN4(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("N4");
                builder.Append("*");
                builder.Append(row.N401);
                builder.Append("*");
                builder.Append(row.N402);
                builder.Append("*");
                builder.Append(row.N403);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetNM1(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("NM1*");
                builder.Append(row.NM101);
                builder.Append("*");
                builder.Append(row.NM102);
                builder.Append("*");
                builder.Append(row.NM103);
                builder.Append("*");
                builder.Append(row.NM104);
                builder.Append("*");
                builder.Append(row.NM105);
                builder.Append("*");
                builder.Append(row.NM106);
                builder.Append("*");
                builder.Append(row.NM107);
                builder.Append("*");
                builder.Append(row.NM108);
                builder.Append("*");
                builder.Append(row.NM109);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetPayerSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.HL("", "20", "1"));
                builder.Append(this.GetNM1(this.PayerRow));
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetProviderSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (this.ProviderRow != null)
                {
                    builder.Append(this.HL("1", "21", "1"));
                    builder.Append(this.GetNM1(this.ProviderRow));
                    builder.Append(this.GetN3(this.ProviderRow));
                    builder.Append(this.GetN4(this.ProviderRow));
                    this.ParentProvider = this.HLCount;
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetSE()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SE*");
                builder.Append((int) (this.SegCount + 1));
                builder.Append("*");
                builder.Append(this.HeaderRow.ST02);
                builder.Append("~");
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public DS270.EDI270NamesRow GetSegmentRow(string NameType)
        {
            DS270.EDI270NamesRow row;
            try
            {
                DS270.EDI270NamesRow row2 = null;
                for (int i = this.dsEligibility.EDI270Names.Rows.Count - 1; i >= 0; i--)
                {
                    row2 = (DS270.EDI270NamesRow) this.dsEligibility.EDI270Names.Rows[i];
                    if (row2.NM101 == NameType)
                    {
                        row2 = (DS270.EDI270NamesRow) this.dsEligibility.EDI270Names.Rows[i];
                        break;
                    }
                    row2 = null;
                }
                row = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row;
        }

        private string GetST()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ST*" + this.HeaderRow.ST01 + "*");
                builder.Append(this.HeaderRow.ST02);
                builder.Append("*");
                builder.Append(this.HeaderRow.ST03);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetSubscriberSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((this.SubscriberRow != null) && this.SubscriberRow.IsSubscriber)
                {
                    if (this.DependentRow != null)
                    {
                        builder.Append(this.HL(this.ParentProvider.ToString(), "22", "1"));
                    }
                    else
                    {
                        builder.Append(this.HL(this.ParentProvider.ToString(), "22", "0"));
                    }
                    builder.Append(this.GetTRN(this.SubscriberRow));
                    builder.Append(this.GetNM1(this.SubscriberRow));
                    builder.Append(this.GetN3(this.SubscriberRow));
                    builder.Append(this.GetN4(this.SubscriberRow));
                    builder.Append(this.GetDMG(this.SubscriberRow));
                    builder.Append(this.GetDTP(this.SubscriberRow));
                    //in case of self then EQ segment will append and in case of dependent EQ segment will not append with subscriber.
                    if ((this.DependentRow == null))
                    {
                        builder.Append(this.GetEQ(this.SubscriberRow));
                    }
                   this.ParentSubscriber = this.HLCount;
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetTrailer()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.GetSE());
                builder.Append(this.GetGE());
                builder.Append(this.GetIEA());
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetTRN(DS270.EDI270NamesRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("TRN*");
                builder.Append(row.TRN01);
                builder.Append("*");
                builder.Append(row.TRN02);
                builder.Append("*");
                builder.Append(row.TRN03);
                builder.Append("~");
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    this.SegCount++;
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string HL(string ParentId, string HLCode, string ChildCount)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("HL*");
                this.HLCount++;
                
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private void Initialization()
        {
            try
            {
                this.HeaderRow = (DS270.EDI270HeaderRow) this.dsEligibility.EDI270Header.Rows[0];
                this.PayerRow = this.GetSegmentRow("PR");
                this.ProviderRow = this.GetSegmentRow("1P");
               
                this.URL = this.HeaderRow.URL;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public DS270 dsEligibility { get; set; }

        public int HLCount { get; set; }

        public int ParentProvider { get; set; }

        public int ParentSubscriber { get; set; }

        public int SegCount { get; set; }

        public string SubmitterId { get; private set; }

        public string SubmitterPassword { get; private set; }

        public string URL { get; private set; }
    }
}

