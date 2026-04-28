namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ServiceLine
    {
        private string DTP_Val;
        private int index;
        private string LX_Val;
        private string REF_PAN_Val;
        private string LIN_Val;
        private string CTP_Val;
        private string REF_Val;
        private string REF_CLIA_Val;
        private DSHCFA._837ServiceLineRow row;
        private string SV1_Val;

        public ServiceLine(DSHCFA._837ServiceLineRow rowServiceLine, int indexVal = 0)
        {
            this.row = rowServiceLine;
            this.index = indexVal + 1;
            this.SetLX();
            this.SetSV1();
            this.SetDTP();
            this.SetREF_PAN();
            this.SetREF();
            this.SetREF_CLIA();
            this.SetLIN();
            this.SetCTP();
        }

        private void SetDTP()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP*");
                builder.Append(this.row.DTP01);
                builder.Append("*");
                builder.Append(this.row.DTP02);
                builder.Append("*");
                builder.Append(this.row.DTP03);
                builder.Append("~");
                this.DTP_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetLX()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("LX*");
                builder.Append(this.index.ToString());
                builder.Append("~");
                this.LX_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetREF()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("REF");
                builder.Append("*");
                builder.Append(this.row.REF01);
                builder.Append("*");
                builder.Append(this.row.REF02);
                builder.Append("~");
                this.REF_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetREF_PAN()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("REF");
                builder.Append("*");
                builder.Append(this.row.REF01_PAN);
                builder.Append("*");
                builder.Append(this.row.REF02_PAN);
                builder.Append("~");
                this.REF_PAN_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetLIN()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("LIN");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.LIN02);
                builder.Append("*");
                builder.Append(this.row.LIN03);
                builder.Append("~");
                this.LIN_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private void SetCTP()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CTP");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.CTP04);
                builder.Append("*");
                builder.Append(this.row.CTP05);
                builder.Append("~");
                this.CTP_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetSV1()
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("SV1*");
                builder.Append(this.row.SV101_1);
                builder.Append(":");
                builder.Append(this.row.SV101_2);
                //start PMS-1665
                //if modifier is empty and Description is not empty in SV101_7 and we have to report it on EDI then ':' will append to add discription to its respective chunk.
                //else in case of of modifier is not empty and description doesnt want not report in edi then only that modifier value will append.
                if (string.IsNullOrEmpty(this.row.SV101_3) && !string.IsNullOrEmpty(this.row.SV101_7) && this.row.IsReportCPTDesc == true)
                {
                    builder.Append(":");
                }
                else if (!string.IsNullOrEmpty(this.row.SV101_3))
                {
                    builder.Append(":");
                    builder.Append(this.row.SV101_3);
                }

                if (string.IsNullOrEmpty(this.row.SV101_4) && !string.IsNullOrEmpty(this.row.SV101_7) && this.row.IsReportCPTDesc == true)
                {
                    builder.Append(":");
                }
                else if (!string.IsNullOrEmpty(this.row.SV101_4))
                {
                    builder.Append(":");
                    builder.Append(this.row.SV101_4);
                }

                if (string.IsNullOrEmpty(this.row.SV101_5) && !string.IsNullOrEmpty(this.row.SV101_7) && this.row.IsReportCPTDesc == true)
                {
                    builder.Append(":");
                }
                else if (!string.IsNullOrEmpty(this.row.SV101_5))
                {
                    builder.Append(":");
                    builder.Append(this.row.SV101_5);
                }

                if (string.IsNullOrEmpty(this.row.SV101_6) && !string.IsNullOrEmpty(this.row.SV101_7) && this.row.IsReportCPTDesc == true)
                {
                    builder.Append(":");
                }
                else if (!string.IsNullOrEmpty(this.row.SV101_6))
                {
                    builder.Append(":");
                    builder.Append(this.row.SV101_6);
                }

                if (!string.IsNullOrEmpty(this.row.SV101_7) && this.row.IsReportCPTDesc == true)
                {
                    builder.Append(":");
                    builder.Append(this.row.SV101_7);
                }
                //end PMS-1665

                builder.Append("*");
                builder.Append(this.row.SV102);
                builder.Append("*");
                builder.Append(this.row.SV103);
                builder.Append("*");
                builder.Append(this.row.SV104);
                builder.Append("*");
                builder.Append(this.row.SV105);
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.SV107);
                if (this.row.SV109 != "")
                {
                    builder.Append("*");
                    builder.Append("*");
                    builder.Append(this.row.SV109);
                }
                builder.Append("~");
                this.SV1_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string DTP
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_Val;
            }
        }

        public string LX
        {
            get
            {
                EDICommon.SegCount++;
                return this.LX_Val;
            }
        }
        public string LIN
        {
            get
            {
                EDICommon.SegCount++;
                return this.LIN_Val;
            }
        }
        public string CTP
        {
            get
            {
                EDICommon.SegCount++;
                return this.CTP_Val;
            }
        }

        private void SetREF_CLIA()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("REF");
                builder.Append("*");
                builder.Append(this.row.REF01_CLIA);
                builder.Append("*");
                builder.Append(this.row.REF02_CLIA);
                builder.Append("~");
                this.REF_CLIA_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string REF
        {
            get
            {
                EDICommon.SegCount++;
                return this.REF_Val;
            }
        }

        public string REF_CLIA
        {
            get
            {
                EDICommon.SegCount++;
                return this.REF_CLIA_Val;
            }
        }

        public string REF_PAN
        {
            get
            {
                EDICommon.SegCount++;
                return this.REF_PAN_Val;
            }
        }

        public string SV1
        {
            get
            {
                EDICommon.SegCount++;
                return this.SV1_Val;
            }
        }
    }
}

