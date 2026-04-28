namespace EDIParser.EDI837Segment
{
    using EDIParser;
    using System;
    using System.Collections;
    using System.Text;

    public class Diagnosis
    {
        private string BF_Val;
        private string BK_Val;
        private string CodePointerStr = "";
        private string HI_Val;
        public DSHCFA.VisitICDsRow[] VisitsICDs;
        private string BK_Qualifier = "BK:";
        private string BF_Qualifier = "BF:";

        public Diagnosis(DSHCFA.VisitICDsRow[] VisitsICDs)
        {
            int counter = 0;
            foreach (DSHCFA.VisitICDsRow ds_icd in VisitsICDs)
            {
                if (ds_icd.IsICD10 && counter == 0)
                {
                    this.BK_Qualifier = "ABK:";
                    this.BF_Qualifier = "ABF:";
                }

                // First ICD is BKF and set HI 
                if (counter == 0)
                {
                    this.SetHI();

                    //Set BK
                    StringBuilder builder = new StringBuilder();
                    if (!string.IsNullOrEmpty(ds_icd.ICDCode))
                    {
                        builder.Append(this.BK_Qualifier);
                        builder.Append(ds_icd.ICDCode);
                        if (VisitsICDs.Length > 1)
                            builder.Append("*");
                    }
                    this.BK_Val = builder.ToString().Trim();
                }
                else
                {
                    //Set BF
                    StringBuilder builder = new StringBuilder();
                    if (!string.IsNullOrEmpty(ds_icd.ICDCode))
                    {
                        builder.Append(this.BF_Qualifier);
                        builder.Append(ds_icd.ICDCode);
                        if ((VisitsICDs.Length > 1) && counter < VisitsICDs.Length-1)
                            builder.Append("*");
                    }
                    this.BF_Val += builder.ToString().Trim();
                }
                counter++;
            }

            //this.row = rowDiagnosis;
            //this.ICDArray = ICD;
            //if (this.row.IsICD10)
            //{
            //    this.BK_Qualifier = "ABK:";
            //    this.BF_Qualifier = "ABF:";
            //}

            //if (ICD.Count <= 0)
            //{
            //    this.SetHI();
            //    this.SetBK();
            //}
            //this.SetBF();
        }

        //private void SetBF()
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        if (!this.ICDArray.Contains(this.row.ICDCode1) && !this.row.ICDCode1.Equals(""))
        //        {
        //            this.ICDArray.Add(this.row.ICDCode1);
        //            builder.Append(this.BF_Qualifier);
        //            builder.Append(this.row.ICDCode1);
        //            if (((this.row.ICDCode2 != "") || (this.row.ICDCode3 != "")) || (this.row.ICDCode4 != ""))
        //            {
        //                builder.Append("*");
        //            }
        //            if (this.CodePointerStr.Length == 0)
        //            {
        //                this.CodePointerStr = this.CodePointerStr + (this.ICDArray.IndexOf(this.row.ICDCode1) + 1);
        //            }
        //            else
        //            {
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode1) + 1);
        //            }
        //        }
        //        else if (this.CodePointerStr != "1")
        //        {
        //            if (this.CodePointerStr.Length == 0)
        //            {
        //                this.CodePointerStr = this.CodePointerStr + (this.ICDArray.IndexOf(this.row.ICDCode1) + 1);
        //            }
        //            else
        //            {
        //                if (this.ICDArray.IndexOf(this.row.ICDCode1) + 1 > 0)
        //                    this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode1) + 1);
        //            }
        //        }

        //        if (!this.ICDArray.Contains(this.row.ICDCode2) && !this.row.ICDCode2.Equals(""))
        //        {
        //            this.ICDArray.Add(this.row.ICDCode2);
        //            builder.Append(this.BF_Qualifier);
        //            builder.Append(this.row.ICDCode2);
        //            if ((this.row.ICDCode3 != "") || (this.row.ICDCode4 != ""))
        //            {
        //                builder.Append("*");
        //            }
        //            if (this.CodePointerStr.Length == 0)
        //            {
        //                this.CodePointerStr = this.CodePointerStr + (this.ICDArray.IndexOf(this.row.ICDCode2) + 1);
        //            }
        //            else
        //            {
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode2) + 1);
        //            }
        //        }
        //        else
        //        {
        //            if (this.ICDArray.IndexOf(this.row.ICDCode2) + 1 > 0)
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode2) + 1);
        //        }


        //        if (!this.ICDArray.Contains(this.row.ICDCode3) && !this.row.ICDCode3.Equals(""))
        //        {
        //            this.ICDArray.Add(this.row.ICDCode3);
        //            builder.Append(this.BF_Qualifier);
        //            builder.Append(this.row.ICDCode3);
        //            if (this.row.ICDCode4 != "")
        //            {
        //                builder.Append("*");
        //            }
        //            if (this.CodePointerStr.Length == 0)
        //            {
        //                this.CodePointerStr = this.CodePointerStr + (this.ICDArray.IndexOf(this.row.ICDCode3) + 1);
        //            }
        //            else
        //            {
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode3) + 1);
        //            }
        //        }
        //        else
        //        {
        //            if (this.ICDArray.IndexOf(this.row.ICDCode3) + 1 > 0)
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode3) + 1);
        //        }


        //        if (!this.ICDArray.Contains(this.row.ICDCode4) && !this.row.ICDCode4.Equals(""))
        //        {
        //            this.ICDArray.Add(this.row.ICDCode4);
        //            builder.Append(this.BF_Qualifier);
        //            builder.Append(this.row.ICDCode4);
        //            if (this.CodePointerStr.Length == 0)
        //            {
        //                this.CodePointerStr = this.CodePointerStr + (this.ICDArray.IndexOf(this.row.ICDCode4) + 1);
        //            }
        //            else
        //            {
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode4) + 1);
        //            }
        //        }
        //        else
        //        {
        //            if (this.ICDArray.IndexOf(this.row.ICDCode4) + 1 > 0)
        //                this.CodePointerStr = this.CodePointerStr + ":" + (this.ICDArray.IndexOf(this.row.ICDCode4) + 1);
        //        }


        //        this.BF_Val = builder.ToString().Trim();

        //        // this.row.SV107 = this.CodePointerStr;
        //        if (!string.IsNullOrEmpty(this.row.ICDPointer1))
        //            this.row.SV107 = this.row.ICDPointer1;
        //        if (!string.IsNullOrEmpty(this.row.ICDPointer2))
        //            this.row.SV107 += ":" + this.row.ICDPointer2;
        //        if (!string.IsNullOrEmpty(this.row.ICDPointer3))
        //            this.row.SV107 += ":" + this.row.ICDPointer3;
        //        if (!string.IsNullOrEmpty(this.row.ICDPointer4))
        //            this.row.SV107 += ":" + this.row.ICDPointer4;

        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        //private void SetBK(DSHCFA.VisitICDsRow dr_icd)
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        if (!this.ICDArray.Contains(this.row.ICDCode1) && !this.row.ICDCode1.Equals(""))
        //        {
        //            this.ICDArray.Add(this.row.ICDCode1);
        //            builder.Append(this.BK_Qualifier);
        //            builder.Append(this.row.ICDCode1);
        //            if (((this.row.ICDCode2 != "") || (this.row.ICDCode3 != "")) || (this.row.ICDCode4 != ""))
        //            {
        //                builder.Append("*");
        //            }
        //        }
        //        this.BK_Val = builder.ToString().Trim();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        private void SetHI()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("HI*");
                this.HI_Val = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string BF
        {
            get
            {
                return this.BF_Val;
            }
        }

        public string BK
        {
            get
            {
                return this.BK_Val;
            }
        }

        public string HI
        {
            get
            {
                return this.HI_Val;
            }
        }
    }
}

