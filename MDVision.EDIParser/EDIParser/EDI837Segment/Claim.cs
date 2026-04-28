namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Collections;
    using System.Text;

    public class Claim
    {
        private string CLM_Val;
        private string DTP_A_Val;
        private string DTP_B_Val;
        private string DTP_C_Val;
        private string DTP_D_Val;
        private string DTP_E_Val;
        private string DTP_F_Val;
        private string DX_Val;
        private string NTE_Val;
        private string PWK_Val;
        private string REF_CLIA_Val;
        private string REF_PAYER_CLAIM_CtrlNo_Val;
        private string REF_PRIOR_AUTH_Val;
        public string CurrentVisitId;
        private DSHCFA._837ClaimRow row;
        private DSHCFA._837ServiceLineRow[] rowsDX;
        public DSHCFA.VisitICDsRow[] VisitICDs;

        public Claim(DSHCFA._837ClaimRow rowClaim, DSHCFA._837ServiceLineRow[] rowService, DSHCFA.VisitICDsDataTable VisitICDs)
        {
            this.row = rowClaim;
            this.rowsDX = rowService;
            CurrentVisitId = this.row.VisitId;
            DSHCFA ds = new DSHCFA();
            this.VisitICDs = (DSHCFA.VisitICDsRow[])VisitICDs.Select(ds.VisitICDs.VisitIdColumn.ColumnName + "=" + "'" + CurrentVisitId + "'");
            this.SetCLM();
            this.SetDTP_A();
            this.SetDTP_B();
            this.SetDTP_C();
            this.SetDTP_D();
            this.SetDTP_E();
            this.SetDTP_F();
            this.SetPWK();
            this.SetREF_PRIOR_AUTH();
            this.SetREF_PAYER_CLAIM_CtrlNo();
            this.SetREF_CLIA();
            this.SetNTE();
            this.SetDX();
        }

        private void SetCLM()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("CLM*");
                builder.Append(this.row.CLM01);
                builder.Append("*");
                builder.Append(this.row.CLM02);
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.CLM05_1);
                builder.Append(":");
                builder.Append(this.row.CLM05_2);
                builder.Append(":");
                builder.Append(this.row.CLM05_3);
                builder.Append("*");
                builder.Append(this.row.CLM06);
                builder.Append("*");
                if (this.row.CLM07.ToString() == "Y")
                {
                    builder.Append("A");
                }
                else if (this.row.CLM07.ToString() == "N")
                {
                    builder.Append("C");
                }
                builder.Append("*");
                builder.Append(this.row.CLM08);
                builder.Append("*");
                builder.Append(this.row.CLM09);
                bool flag = false;
                int num = 0;
                bool flag2 = false;

                // if CLM11 have to report then append * from CLM10 which is Patient Signature Source Code 'P' in our case it is empty.
                if (!string.IsNullOrEmpty(this.row.CLM11_1A) || !string.IsNullOrEmpty(this.row.CLM11_1B) || !string.IsNullOrEmpty(this.row.CLM11_1C))
                    builder.Append("*");

                if (this.row.CLM11_1A != "")
                {
                    builder.Append("*");
                    builder.Append(this.row.CLM11_1A);
                    flag = true;
                }
                else if (this.row.CLM11_1B != "")
                {
                    if (flag)
                    {
                        builder.Append(":");
                        num++;
                    }
                    else
                    {
                        builder.Append("*");
                        flag = true;
                    }
                    builder.Append(this.row.CLM11_1B);
                    flag2 = true;
                }
                else if (this.row.CLM11_1C != "")
                {
                    if (flag)
                    {
                        builder.Append(":");
                        num++;
                    }
                    else
                    {
                        builder.Append("*");
                        flag = true;
                    }
                    builder.Append(this.row.CLM11_1C);
                }
                if (flag2)
                {
                    short num2 = 0;
                    for (num2 = 0; num2 <= ((3 - num) - 1); num2 = (short)(num2 + 1))
                    {
                        builder.Append(":");
                    }
                    builder.Append(this.row.CLM11_4);
                }
                builder.Append("~");
                this.CLM_Val = builder.ToString().Trim();
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
                builder.Append("DTP");
                builder.Append("*");
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

        private void SetDTP_B()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(this.row.DTP01_B);
                builder.Append("*");
                builder.Append(this.row.DTP02_B);
                builder.Append("*");
                builder.Append(this.row.DTP03_B);
                builder.Append("~");
                this.DTP_B_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDTP_C()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(this.row.DTP01_C);
                builder.Append("*");
                builder.Append(this.row.DTP02_C);
                builder.Append("*");
                builder.Append(this.row.DTP03_C);
                builder.Append("~");
                this.DTP_C_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDTP_D()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(this.row.DTP01_D);
                builder.Append("*");
                builder.Append(this.row.DTP02_D);
                builder.Append("*");
                builder.Append(this.row.DTP03_D);
                builder.Append("~");
                this.DTP_D_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDTP_E()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(this.row.DTP01_E);
                builder.Append("*");
                builder.Append(this.row.DTP02_E);
                builder.Append("*");
                builder.Append(this.row.DTP03_E);
                builder.Append("~");
                this.DTP_E_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDTP_F()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("DTP");
                builder.Append("*");
                builder.Append(this.row.DTP01_F);
                builder.Append("*");
                builder.Append(this.row.DTP02_F);
                builder.Append("*");
                builder.Append(this.row.DTP03_F);
                builder.Append("~");
                this.DTP_F_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetDX()
        {
            try
            {
                Diagnosis diagnosis = new Diagnosis(this.VisitICDs);

                StringBuilder builder = new StringBuilder();
                builder.Append(diagnosis.HI);
                builder.Append(diagnosis.BK);

                if (!string.IsNullOrEmpty(diagnosis.BF))
                {
                    string str = builder.ToString();
                    if (str[str.Length - 1] != '*')
                        builder.Append("*");
                    builder.Append(diagnosis.BF);
                }

                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("~");
                this.DX_Val = builder.ToString();


                // Set Row Pointers
                for (int i = 0; i <= (this.rowsDX.Length - 1); i++)
                {
                    if (!string.IsNullOrEmpty(this.rowsDX[i].ICDPointer1))
                        this.rowsDX[i].SV107 = this.rowsDX[i].ICDPointer1;
                    if (!string.IsNullOrEmpty(this.rowsDX[i].ICDPointer2))
                        this.rowsDX[i].SV107 += ":" + this.rowsDX[i].ICDPointer2;
                    if (!string.IsNullOrEmpty(this.rowsDX[i].ICDPointer3))
                        this.rowsDX[i].SV107 += ":" + this.rowsDX[i].ICDPointer3;
                    if (!string.IsNullOrEmpty(this.rowsDX[i].ICDPointer4))
                        this.rowsDX[i].SV107 += ":" + this.rowsDX[i].ICDPointer4;
                }

                //StringBuilder builder = new StringBuilder();
                //ArrayList iCD = new ArrayList();
                //for (int i = 0; i <= (this.rowsDX.Length - 1); i++)
                //{
                //    DSHCFA.VisitICDsRow rows= VisitsICDs
                //Diagnosis diagnosis = new Diagnosis(ref this.rowsDX[i], this.VisitsICDs);
                //    if (i == 0)
                //    {
                //        builder.Append(diagnosis.HI);
                //        builder.Append(diagnosis.BK);
                //    }
                //    if (!string.IsNullOrEmpty(diagnosis.BF))
                //    {
                //        string str = builder.ToString();
                //        if (str[str.Length - 1] != '*')
                //            builder.Append("*");
                //        builder.Append(diagnosis.BF);
                //    }

                //}
                //if (!string.IsNullOrEmpty(builder.ToString()))
                //    builder.Append("~");
                //this.DX_Val = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetNTE()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("NTE*");
                builder.Append(this.row.NTE01);
                builder.Append("*");

                if (!string.IsNullOrEmpty(this.row.NTE02) && !string.IsNullOrEmpty(this.row.NTE03))
                {
                    builder.Append(this.row.NTE02 + " " + this.row.NTE03);
                }
                else if (!string.IsNullOrEmpty(this.row.NTE02) && string.IsNullOrEmpty(this.row.NTE03))
                {
                    builder.Append(this.row.NTE02);
                }
                else if (string.IsNullOrEmpty(this.row.NTE02) && !string.IsNullOrEmpty(this.row.NTE03))
                {
                    builder.Append(this.row.NTE03);
                }

                builder.Append("~");
                this.NTE_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetPWK()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("PWK");
                builder.Append("*");
                builder.Append(this.row.PWK01);
                builder.Append("*");
                builder.Append(this.row.PWK02);
                builder.Append("*");
                builder.Append("*");
                builder.Append("*");
                builder.Append(this.row.PWK05);
                builder.Append("*");
                builder.Append(this.row.PWK06);
                builder.Append("~");
                this.PWK_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
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

        private void SetREF_PAYER_CLAIM_CtrlNo()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("REF");
                builder.Append("*");
                builder.Append(this.row.REF01_PayerClmCtrlNo);
                builder.Append("*");
                builder.Append(this.row.REF02_PayerClmCtrlNo);
                builder.Append("~");
                this.REF_PAYER_CLAIM_CtrlNo_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SetREF_PRIOR_AUTH()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("REF");
                builder.Append("*");
                builder.Append(this.row.REF01_PRIOR_AUTH);
                builder.Append("*");
                builder.Append(this.row.REF02_PRIOR_AUTH);
                builder.Append("~");
                this.REF_PRIOR_AUTH_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string CLM
        {
            get
            {
                EDICommon.SegCount++;
                return this.CLM_Val;
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

        public string DTP_B
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_B_Val;
            }
        }

        public string DTP_C
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_C_Val;
            }
        }

        public string DTP_D
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_D_Val;
            }
        }

        public string DTP_E
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_E_Val;
            }
        }

        public string DTP_F
        {
            get
            {
                EDICommon.SegCount++;
                return this.DTP_F_Val;
            }
        }

        public string DX
        {
            get
            {
                EDICommon.SegCount++;
                return this.DX_Val;
            }
        }

        public string NTE
        {
            get
            {
                EDICommon.SegCount++;
                return this.NTE_Val;
            }
        }

        public string PWK
        {
            get
            {
                EDICommon.SegCount++;
                return this.PWK_Val;
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

        public string REF_PAYER_CLAIM_CtrlNo
        {
            get
            {
                EDICommon.SegCount++;
                return this.REF_PAYER_CLAIM_CtrlNo_Val;
            }
        }

        public string REF_PRIOR_AUTH
        {
            get
            {
                EDICommon.SegCount++;
                return this.REF_PRIOR_AUTH_Val;
            }
        }
    }
}

