namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Name
    {
        private string AMT_Val;
        private string DMG_Val;
        private string N3_Val;
        private string N4_Val;
        private string NM1_Val;
        private string OI_Val;
        private string PAT_Val;
        private string PRV_Val;
        private string REF_Val;
        private DSHCFA._837NameRow row;
        private string SBR_Val;

        public Name(DSHCFA._837NameRow rowName, bool IsSBR02 = false)
        {
            this.row = rowName;
            this.SetSBR(IsSBR02);
            this.SetNM1();
            this.SetN3();
            this.SetN4();
            this.SetDMG();
            this.SetPRV();
            this.SetPAT();
            this.SetREF();
            this.SetAMT();
            this.SetOI();
        }

        private void SetAMT()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("AMT*");
                builder.Append(this.row.AMT01);
                builder.Append("*");
                builder.Append(this.row.AMT02);
                builder.Append("~");
                this.AMT_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDMG()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DMG");
                builder.Append("*");
                builder.Append(this.row.DMG01);
                builder.Append("*");
                builder.Append(this.row.DMG02);
                builder.Append("*");
                builder.Append(this.row.DMG03);
                builder.Append("~");
                this.DMG_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetN3()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("N3");
                builder.Append("*");
                builder.Append(this.row.N301);
                if (this.row.N302 != "")
                {
                    builder.Append("*");
                    builder.Append(this.row.N302);
                }
                builder.Append("~");
                this.N3_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetN4()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("N4");
                builder.Append("*");
                builder.Append(this.row.N401);
                builder.Append("*");
                builder.Append(this.row.N402);
                builder.Append("*");
                builder.Append(this.row.N403);
                builder.Append("~");
                this.N4_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetNM1()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("NM1*");
                builder.Append(this.row.NM101);
                builder.Append("*");
                builder.Append(this.row.NM102);
                builder.Append("*");
                builder.Append(this.row.NM103);
                builder.Append("*");
                builder.Append(this.row.NM104);
                builder.Append("*");
                builder.Append(this.row.NM105);
                builder.Append("*");
                builder.Append(this.row.NM106);
                builder.Append("*");
                builder.Append(this.row.NM107);
                builder.Append("*");
                builder.Append(this.row.NM108);
                builder.Append("*");
                builder.Append(this.row.NM109);
                builder.Append("~");
                this.NM1_Val = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetOI()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("OI*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.OI03);
                builder.Append("*");
                builder.Append(this.row.OI04);
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.OI06);
                builder.Append("~");
                this.OI_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetPAT()
        {
            try
            {
                List <string> codes = new List<string>(new string[] { "01", "19", "20", "21", "39", "40", "53", "G8" });
                if (!codes.Contains(this.row.PAT01))
                    this.row.PAT01 = "G8";

                    StringBuilder builder = new StringBuilder();
                    builder.Append("PAT*");
                    builder.Append(this.row.PAT01);
                    builder.Append("~");
                    this.PAT_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetPRV()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("PRV*");
                builder.Append(this.row.PRV01);
                builder.Append("*");
                builder.Append(this.row.PRV02);
                builder.Append("*");
                builder.Append(this.row.PRV03);
                builder.Append("~");
                this.PRV_Val = builder.ToString().Trim();
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
                builder.Append("REF*");
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

        private void SetSBR(bool IsSBR02 = false)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SBR*");
                builder.Append(this.row.SBR01);
                builder.Append("*");
                if (IsSBR02)
                    builder.Append("");
                else
                    builder.Append(this.row.SBR02);
                builder.Append("*");
                builder.Append(this.row.SBR03);
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.SBR05);
                builder.Append("****");
                builder.Append(this.row.SBR09);
                builder.Append("~");
                this.SBR_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string AMT
        {
            get
            {
                EDICommon.SegCount++;
                return this.AMT_Val;
            }
        }

        public string DMG
        {
            get
            {
                EDICommon.SegCount++;
                return this.DMG_Val;
            }
        }

        public string N3
        {
            get
            {
                EDICommon.SegCount++;
                return this.N3_Val;
            }
        }

        public string N4
        {
            get
            {
                EDICommon.SegCount++;
                return this.N4_Val;
            }
        }

        public string NM1
        {
            get
            {
                EDICommon.SegCount++;
                return this.NM1_Val;
            }
        }

        public string OI
        {
            get
            {
                EDICommon.SegCount++;
                return this.OI_Val;
            }
        }

        public string PAT
        {
            get
            {
                EDICommon.SegCount++;
                return this.PAT_Val;
            }
        }

        public string PRV
        {
            get
            {
                EDICommon.SegCount++;
                return this.PRV_Val;
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

        public string SBR
        {
            get
            {
                EDICommon.SegCount++;
                return this.SBR_Val;
            }
        }
    }
}

