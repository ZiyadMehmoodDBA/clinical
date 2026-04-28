namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Text;

    public class Header
    {
        private string BHT_Val;
        private string GS_Val;
        private string ISA_Val;
        private string NM1_Rec_Val;
        private string NM1_Sub_Val;
        private string PER_Sub_Val;
        private DSHCFA._837HeaderRow row;
        private string ST_Val;

        public Header(DSHCFA._837HeaderRow rowHeader)
        {
            this.row = rowHeader;
            this.SetISA();
            this.SetGS();
            this.SetST();
            this.SetBHT();
            this.SetNM1_Sub();
            this.SetPER_Sub();
            this.SetNM1_Rec();
        }

        private void SetBHT()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("BHT*" + this.row.BHT01 + "*");
                builder.Append(this.row.BHT02);
                builder.Append("*");
                builder.Append(this.row.BHT03);
                builder.Append("*");
                builder.Append(this.row.BHT04);
                builder.Append("*");
                builder.Append(this.row.BHT05);
                builder.Append("*");
                builder.Append(this.row.BHT06);
                builder.Append("~");
                this.BHT_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetGS()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("GS*" + this.row.GS01 + "*");
                builder.Append(this.row.GS02);
                builder.Append("*");
                builder.Append(this.row.GS03);
                builder.Append("*");
                builder.Append(this.row.GS04);
                builder.Append("*");
                builder.Append(this.row.GS05);
                builder.Append("*");
                builder.Append(this.row.GS06);
                builder.Append("*");
                builder.Append(this.row.GS07);
                builder.Append("*");
                builder.Append(this.row.GS08);
                builder.Append("~");
                this.GS_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetISA()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ISA*" + this.row.ISA01 + "*" + this.row.ISA02 + "*" + this.row.ISA03 + "*" + this.row.ISA04 + "*" + this.row.ISA05 + "*");
                builder.Append(this.row.ISA06.Trim().PadRight(15));
                builder.Append("*" + this.row.ISA07 + "*");
                builder.Append(this.row.ISA08.Trim().PadRight(15));
                builder.Append("*");
                builder.Append(this.row.ISA09);
                builder.Append("*");
                builder.Append(this.row.ISA10);
                builder.Append("*");
                builder.Append(this.row.ISA11);
                builder.Append("*");
                builder.Append(this.row.ISA12);
                builder.Append("*");
                builder.Append(this.row.ISA13);
                builder.Append("*");
                builder.Append(this.row.ISA14);
                builder.Append("*");
                builder.Append(this.row.ISA15.ToUpper());
                builder.Append("*:~");
                this.ISA_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetNM1_Rec()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("NM1*" + this.row.NM101_REC + "*");
                builder.Append(this.row.NM102_REC);
                builder.Append("*");
                builder.Append(this.row.NM103_REC);
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.NM108_REC);
                builder.Append("*");
                builder.Append(this.row.NM109_REC);
                builder.Append("~");
                this.NM1_Rec_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetNM1_Sub()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("NM1*" + this.row.NM101_SUB + "*");
                builder.Append(this.row.NM102_SUB);
                builder.Append("*");
                builder.Append(this.row.NM103_SUB);
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.NM108_SUB);
                builder.Append("*");
                builder.Append(this.row.NM109_SUB);
                builder.Append("~");
                this.NM1_Sub_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetPER_Sub()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("PER*" + this.row.PER01_SUB + "*");
                builder.Append(this.row.PER02_SUB);
                builder.Append("*");
                builder.Append(this.row.PER03_SUB);
                builder.Append("*");
                builder.Append(this.row.PER04_SUB);
                builder.Append("~");
                this.PER_Sub_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetST()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ST*" + this.row.ST01 + "*");
                builder.Append(this.row.ST02);
                builder.Append("*");
                builder.Append(this.row.ST03);
                builder.Append("~");
                this.ST_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string BHT
        {
            get
            {
                EDICommon.SegCount++;
                return this.BHT_Val;
            }
        }

        public string GS
        {
            get
            {
                return this.GS_Val;
            }
        }

        public string ISA
        {
            get
            {
                return this.ISA_Val;
            }
        }

        public string NM1_Rec
        {
            get
            {
                EDICommon.SegCount++;
                return this.NM1_Rec_Val;
            }
        }

        public string NM1_Sub
        {
            get
            {
                EDICommon.SegCount++;
                return this.NM1_Sub_Val;
            }
        }

        public string PER_Sub
        {
            get
            {
                EDICommon.SegCount++;
                return this.PER_Sub_Val;
            }
        }

        public string ST
        {
            get
            {
                EDICommon.SegCount++;
                return this.ST_Val;
            }
        }
    }
}

