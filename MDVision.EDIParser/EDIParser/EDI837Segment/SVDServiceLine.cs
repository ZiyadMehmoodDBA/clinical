namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Text;

    public class SVDServiceLine
    {
        private string CAS_A_Val;
        private string CAS_B_Val;
        private string CAS_C_Val;
        private string CAS_D_Val;
        private string DTP_A_Val;
        private DSHCFA._837SVDServiceLineRow row;
        private DSHCFA._837ServiceLineRow rowService;
        private string SVD_Val;

        public SVDServiceLine(DSHCFA._837SVDServiceLineRow rowSVDServiceLine, DSHCFA._837ServiceLineRow rowServiceLine)
        {
            this.row = rowSVDServiceLine;
            this.rowService = rowServiceLine;
            this.SetSVD();
            this.SetCAS_A();
            this.SetCAS_B();
            this.SetCAS_C();
            this.SetCAS_D();
            this.SetDTP_A();
        }

        private void SetCAS_A()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CAS*");
                builder.Append(this.row.CAS01_A);
                builder.Append("*");
                builder.Append(this.row.CAS02_A);
                builder.Append("*");
                builder.Append(this.row.CAS03_A);
                builder.Append("~");
                if (!string.IsNullOrEmpty(this.row.CAS03_A))
                {
                    if (this.row.CAS03_A != "0")
                        this.CAS_A_Val = builder.ToString().Trim();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetCAS_B()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CAS*");
                builder.Append(this.row.CAS01_B);
                builder.Append("*");
                builder.Append(this.row.CAS02_B);
                builder.Append("*");
                builder.Append(this.row.CAS03_B);
                builder.Append("~");
                if (!string.IsNullOrEmpty(this.row.CAS03_B))
                {
                    if (this.row.CAS03_B != "0")
                        this.CAS_B_Val = builder.ToString().Trim();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetCAS_C()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CAS*");
                builder.Append(this.row.CAS01_C);
                builder.Append("*");
                builder.Append(this.row.CAS02_C);
                builder.Append("*");
                builder.Append(this.row.CAS03_C);
                builder.Append("~");
                if (!string.IsNullOrEmpty(this.row.CAS03_C))
                {
                    if (this.row.CAS03_C != "0")
                        this.CAS_C_Val = builder.ToString().Trim();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetCAS_D()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CAS*");
                builder.Append(this.row.CAS01_D);
                builder.Append("*");
                builder.Append(this.row.CAS02_D);
                builder.Append("*");
                builder.Append(this.row.CAS03_D);
                builder.Append("~");
                if (!string.IsNullOrEmpty(this.row.CAS03_D))
                {
                    if (this.row.CAS03_D != "0")
                        this.CAS_D_Val = builder.ToString().Trim();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDTP_A()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP*");
                builder.Append(this.row.DTP01_A);
                builder.Append("*");
                builder.Append(this.row.DTP02_A);
                builder.Append("*");
                builder.Append(this.row.DTP03_A);
                builder.Append("~");
                this.DTP_A_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetSVD()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SVD");
                builder.Append("*");
                builder.Append(this.row.SVD01);
                builder.Append("*");
                builder.Append(this.row.AMT02);
                builder.Append("*");
                builder.Append(this.row.SVD03_1);
                builder.Append(":");
                builder.Append(this.row.SVD03_2);
                builder.Append(":");
                builder.Append(this.rowService.SV101_3);
                builder.Append(":");
                builder.Append(this.rowService.SV101_4);
                builder.Append(":");
                builder.Append(this.rowService.SV101_5);
                builder.Append(":");
                builder.Append(this.rowService.SV101_6);
                builder.Append(":");
                builder.Append(this.rowService.SV101_7);
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.rowService.SV104);
                builder.Append("~");
                this.SVD_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string CAS_A
        {
            get
            {
                EDICommon.SegCount++;
                return this.CAS_A_Val;
            }
        }

        public string CAS_B
        {
            get
            {
                EDICommon.SegCount++;
                return this.CAS_B_Val;
            }
        }

        public string CAS_C
        {
            get
            {
                EDICommon.SegCount++;
                return this.CAS_C_Val;
            }
        }

        public string CAS_D
        {
            get
            {
                EDICommon.SegCount++;
                return this.CAS_D_Val;
            }
        }

        public string DTP_A
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_A_Val;
            }
        }

        public string SVD
        {
            get
            {
                EDICommon.SegCount++;
                return this.SVD_Val;
            }
        }
    }
}

