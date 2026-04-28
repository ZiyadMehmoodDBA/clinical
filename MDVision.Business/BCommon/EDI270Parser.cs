//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MDVision.Datasets;
//using EDIParser;
//using System.Data;

//namespace MDVision.Business.BCommon
//{
//    public class EDI270Parser
//    {
//        public int HLCount { get; set; }
//        public int SegCount { get; set; }
//        public int ParentProvider { get; set; }
//        public int ParentSubscriber { get; set; }

//        public string SubmitterId { get; private set; }
//        public string SubmitterPassword { get;private  set; }
//        public string URL { get; private set; }

//        private const string Payer = "PR";
//        private const string Provider = "1P";
//        private const string Subscriber = "IL";
//        private const string Dependent = "03";

//        public const string D_SGMT = "~";
//        public const string D_ELMT = "*";
//        public const string D_S_ELMT = ":";

//        private DS270.EDI270HeaderRow HeaderRow;
//        private DS270.EDI270NamesRow PayerRow;
//        private DS270.EDI270NamesRow ProviderRow;
//        private DS270.EDI270NamesRow SubscriberRow;
//        private DS270.EDI270NamesRow DependentRow;

//        public DS270 dsEligibility { get; set; }

//        public EDI270Parser(DS270 dsEligibility)
//        {
//            this.SegCount = 0;
//            this.HLCount = 0;
//            this.ParentProvider = 0;
//            this.ParentSubscriber = 0;
//            this.HeaderRow = null;
//            this.PayerRow = null;
//            this.ProviderRow = null;
//            this.SubscriberRow = null;
//            this.DependentRow = null;
//            this.dsEligibility = dsEligibility;
//            this.Initialization();
//        }

//        private void Initialization()
//        {
//            try
//            {
//                this.HeaderRow = (DS270.EDI270HeaderRow)this.dsEligibility.EDI270Header.Rows[0];
//                this.PayerRow = this.GetSegmentRow(Payer);
//                this.ProviderRow = this.GetSegmentRow(Provider);
//                this.SubscriberRow = this.GetSegmentRow(Subscriber);
//                this.DependentRow = this.GetSegmentRow(Dependent);

//                this.SubmitterId = this.HeaderRow.SubmitterID;
//                this.SubmitterPassword = this.HeaderRow.SubmitterPassword;
//                this.URL = this.HeaderRow.URL;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public string Get270()
//        {
//            string str;
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append(this.GetHeader());
//                builder.Append(this.GetPayerSegments());
//                builder.Append(this.GetProviderSegments());
//                builder.Append(this.GetSubscriberSegments());
//                builder.Append(this.GetDependentSegments());
//                builder.Append(this.GetTrailer());
//                str = builder.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return str;
//        }

//        #region " Loops "

//        private string GetHeader()
//        {
//            string str;
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append(this.GetISA());
//                builder.Append(this.GetGS());
//                builder.Append(this.GetST());
//                builder.Append(this.GetBHT());
//                str = builder.ToString();
//            }
//            catch (Exception exception)
//            {
//                EDILogger.LogErrorMessage("EDI270Parser::GetHeader", exception);
//                throw exception;
//            }
//            return str;
//        }

//        private string GetPayerSegments()
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                if (Payer != null)
//                {
//                    Str.Append(this.HL("", "20", "1"));
//                    Str.Append(GetNM1(PayerRow));
//                    return Str.ToString();
//                }
//                else
//                    throw new Exception("Payer does not exist.");


//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private string GetProviderSegments()
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();

//                if (ProviderRow != null)
//                {
//                    Str.Append(this.HL("1", "21", "1"));
//                    Str.Append(GetNM1(ProviderRow));
//                    Str.Append(GetN3(ProviderRow));
//                    Str.Append(GetN4(ProviderRow));

//                    //Set Parent Provider
//                    ParentProvider = HLCount;
//                }
//                return Str.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private string GetSubscriberSegments()
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();

//                if (SubscriberRow != null)
//                {
//                    if (SubscriberRow.IsSubscriber)
//                    {
//                        if (DependentRow != null)
//                            Str.Append(this.HL(ParentProvider.ToString(), "22", "1"));
//                        else
//                            Str.Append(this.HL(ParentProvider.ToString(), "22", "0"));

//                        Str.Append(GetTRN(SubscriberRow));
//                        Str.Append(GetNM1(SubscriberRow));
//                        Str.Append(GetN3(SubscriberRow));
//                        Str.Append(GetN4(SubscriberRow));
//                        Str.Append(GetDMG(SubscriberRow));
//                        Str.Append(GetDTP(SubscriberRow));
//                        Str.Append(GetEQ(SubscriberRow));

//                        //Sent Parent Subscriber
//                        ParentSubscriber = HLCount;
//                    }
//                }

//                return Str.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private string GetDependentSegments()
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();

//                if (DependentRow != null)
//                {
//                    if (DependentRow.IsDependent)
//                    {
//                        Str.Append(this.HL(ParentSubscriber.ToString(), "23", "0"));
//                        Str.Append(GetTRN(DependentRow));
//                        Str.Append(GetNM1(DependentRow));
//                        Str.Append(GetN3(DependentRow));
//                        Str.Append(GetN4(DependentRow));
//                        Str.Append(GetDMG(SubscriberRow));
//                        Str.Append(GetDTP(SubscriberRow));
//                        Str.Append(GetEQ(SubscriberRow));
//                    }
//                }

//                return Str.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private string GetTrailer()
//        {
//            string str;
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append(this.GetSE());
//                builder.Append(this.GetGE());
//                builder.Append(this.GetIEA());
//                str = builder.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return str;
//        }

//        #endregion

//        #region " Segments "

//        private string GetISA()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("ISA" + D_ELMT + this.HeaderRow.ISA01 + D_ELMT + this.HeaderRow.ISA02 + D_ELMT + this.HeaderRow.ISA03 + D_ELMT + this.HeaderRow.ISA04 + D_ELMT + this.HeaderRow.ISA05 + D_ELMT);
//                builder.Append(this.HeaderRow.ISA06.Trim().PadRight(15));
//                builder.Append(D_ELMT + this.HeaderRow.ISA07 + D_ELMT);
//                builder.Append(this.HeaderRow.ISA08.Trim().PadRight(15));
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA09);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA10);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA11);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA12);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA13);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA14);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ISA15.ToUpper());
//                builder.Append(D_ELMT + D_S_ELMT + D_SGMT);
//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetGS()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("GS" + D_ELMT + this.HeaderRow.GS01 + D_ELMT);
//                builder.Append(this.HeaderRow.GS02);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS03);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS04);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS05);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS06);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS07);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.GS08);
//                builder.Append(D_SGMT);
//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetST()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("ST" + D_ELMT + this.HeaderRow.ST01 + D_ELMT);
//                builder.Append(this.HeaderRow.ST02);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ST03);
//                builder.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(builder.ToString()))
//                    SegCount = SegCount + 1;

//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetBHT()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("BHT" + D_ELMT + this.HeaderRow.BHT01 + D_ELMT);
//                builder.Append(this.HeaderRow.BHT02);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.BHT03);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.BHT04);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.BHT05);
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.BHT06);
//                builder.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(builder.ToString()))
//                    SegCount = SegCount + 1;

//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetSE()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("SE" + D_ELMT);
//                builder.Append((int)(this.SegCount + 1));
//                builder.Append(D_ELMT);
//                builder.Append(this.HeaderRow.ST02);
//                builder.Append(D_SGMT);
//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetGE()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("GE" + D_ELMT);
//                builder.Append("1" + D_ELMT);
//                builder.Append(this.HeaderRow.ISA13);
//                builder.Append(D_SGMT);
//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }
//        private string GetIEA()
//        {
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("IEA" + D_ELMT);
//                builder.Append("1" + D_ELMT);
//                builder.Append(this.HeaderRow.ISA13);
//                builder.Append(D_SGMT);
//                return builder.ToString().Trim();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private string GetNM1(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                Str.Append("NM1" + D_ELMT);
//                Str.Append(row.NM101);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM102);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM103);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM104);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM105);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM106);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM107);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM108);
//                Str.Append(D_ELMT);
//                Str.Append(row.NM109);

//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;

//                return Str.ToString();

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetN3(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                Str.Append("N3");
//                Str.Append(D_ELMT);
//                Str.Append(row.N301);
//                Str.Append(D_ELMT);
//                Str.Append(row.N302);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;
//                return Str.ToString().Trim();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetN4(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                Str.Append("N4");
//                Str.Append(D_ELMT);
//                Str.Append(row.N401);
//                Str.Append(D_ELMT);
//                Str.Append(row.N402);
//                Str.Append(D_ELMT);
//                Str.Append(row.N403);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;
//                return Str.ToString().Trim();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetDMG(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                Str.Append("DMG");
//                Str.Append(D_ELMT);
//                Str.Append(row.DMG01);
//                Str.Append(D_ELMT);
//                Str.Append(row.DMG02);
//                Str.Append(D_ELMT);
//                Str.Append(row.DMG03);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;

//                return Str.ToString().Trim();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetTRN(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();

//                Str.Append("TRN" + D_ELMT);
//                Str.Append(row.TRN01);
//                Str.Append(D_ELMT);
//                Str.Append(row.TRN02);
//                Str.Append(D_ELMT);
//                Str.Append(row.TRN03);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;

//                return Str.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetDTP(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();
//                Str.Append("DTP");
//                Str.Append(D_ELMT);
//                Str.Append(row.DTP01);
//                Str.Append(D_ELMT);
//                Str.Append(row.DTP02);
//                Str.Append(D_ELMT);
//                Str.Append(row.DTP03);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;

//                return Str.ToString().Trim();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string GetEQ(DS270.EDI270NamesRow row)
//        {
//            try
//            {
//                System.Text.StringBuilder Str = new System.Text.StringBuilder();

//                Str.Append("EQ");
//                Str.Append(D_ELMT);
//                Str.Append(row.EQ01);
//                Str.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(Str.ToString()))
//                    SegCount = SegCount + 1;

//                return Str.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        private string HL(string ParentId, string HLCode, string ChildCount)
//        {
//            string str;
//            try
//            {
//                StringBuilder builder = new StringBuilder();
//                builder.Append("HL" + D_ELMT);
//                this.HLCount++;
//                builder.Append(this.HLCount);
//                builder.Append(D_ELMT + ParentId + D_ELMT + HLCode + D_ELMT + ChildCount);
//                builder.Append(D_SGMT);

//                if (!string.IsNullOrEmpty(builder.ToString()))
//                    SegCount = SegCount + 1;

//                str = builder.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return str;
//        }

//        #endregion

//        #region " Supporting Methods "

//        public DS270.EDI270NamesRow GetSegmentRow(string NameType)
//        {
//            DS270.EDI270NamesRow row2;
//            try
//            {
//                DS270.EDI270NamesRow row = null;
//                for (int i = this.dsEligibility.EDI270Names.Rows.Count - 1; i >= 0; i--)
//                {
//                    row = (DS270.EDI270NamesRow)this.dsEligibility.EDI270Names.Rows[i];
//                    if (row.NM101 == NameType)
//                    {
//                        row = (DS270.EDI270NamesRow)this.dsEligibility.EDI270Names.Rows[i];
//                        break;
//                    }
//                    row = null;
//                }
//                row2 = row;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return row2;
//        }

//        #endregion
//    }
//}
