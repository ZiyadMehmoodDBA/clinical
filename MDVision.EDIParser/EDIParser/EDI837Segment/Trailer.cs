namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Text;

    public class Trailer
    {
        private string GE_Val;
        private string IEA_Val;
        private DSHCFA._837HeaderRow row;
        private string SE_Val;

        public Trailer(DSHCFA._837HeaderRow rowFooter)
        {
            this.row = rowFooter;
            this.SetSE();
            this.SetGE();
            this.SetIEA();
        }

        private void SetGE()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("GE*");
                builder.Append("1*");
                builder.Append(this.row.ISA13);
                builder.Append("~");
                this.GE_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetIEA()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("IEA*");
                builder.Append("1*");
                builder.Append(this.row.ISA13);
                builder.Append("~");
                this.IEA_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetSE()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SE*");
                builder.Append((int) (EDICommon.SegCount + 1));
                builder.Append("*");
                builder.Append(this.row.ST02);
                builder.Append("~");
                this.SE_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GE
        {
            get
            {
                return this.GE_Val;
            }
        }

        public string IEA
        {
            get
            {
                return this.IEA_Val;
            }
        }

        public string SE
        {
            get
            {
                EDICommon.SegCount++;
                return this.SE_Val;
            }
        }
    }
}

