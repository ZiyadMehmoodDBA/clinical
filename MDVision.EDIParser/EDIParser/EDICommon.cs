namespace EDIParser
{
    using EDIParser.Professional;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using MDVision.Common.Utilities;

    public static class EDICommon
    {
        public const string D_ELMT = "*";
        public const string D_S_ELMT = ":";
        public const string D_SGMT = "~";
        private static int HL_Billing_Provider;
        private static int HL_Subscriber;
        private static int HLCounter;
        private static int SegmentCounter;

        public static bool IsEDI(string StrIn)
        {
            return (StrIn.Substring(0, 3) == "ISA");
        }

        public static string ParseEDI(string EDIStr, long ReportId, string FileName, ref dsCache ds835Parser, string UserName, string ERAClaimNumber = "", string ERAChargeNumber = "", bool IsFilterCharges = false, bool IsEOB = false, string CheckNo = "")
        {
            string str3;
            try
            {
                string str = null;
                
                MDVUtility.SetDelimiters(EDIStr);
                EDIStr = EDIStr.Replace("\r\n", "");
                if (EDIStr.Trim().Length > 0)
                {
                    if (IsEDI(EDIStr))
                    {
                        string str2 = MDVUtility.WhichEDI(EDIStr);
                        if (str2 != "271")
                        {
                            if (str2 == "835")
                            {
                                new EDI835Parser().GetHumanReadable835(EDIStr, ref ds835Parser, ReportId, FileName, MDVUtility.DecryptFrom64(UserName));

                                if (!string.IsNullOrEmpty(CheckNo))
                                {
                                    //filter check no
                                    DataRow[] rows;
                                    rows = ds835Parser.Table_BL_ERA_835.Select(ds835Parser.Table_BL_ERA_835.FIELD_ERA835_CHEQUE_NOColumn.ColumnName + " <> " + MDVUtility.ToLINQFormatString( CheckNo) );
                                    foreach (DataRow dr in rows)
                                        dr.Delete();
                                }

                                if (ERAClaimNumber != "" && IsEOB)
                                {
                                    //filter Claim
                                    //Remove all FIELD_CLAIM_ACNT!=ERAClaimNumber from ds835Parser.Table_BL_ERA_835_Claim
                                    DataRow[] claim_rows = ds835Parser.Table_BL_ERA_835_Claim.Select(ds835Parser.Table_BL_ERA_835_Claim.FIELD_CLAIM_ACNTColumn.ColumnName + " <> " + MDVUtility.ToLINQFormatString( ERAClaimNumber) );
                                    foreach (DataRow claim_dr in claim_rows)
                                    {
                                        dsCache.Table_BL_ERA_835_ClaimRow row = (dsCache.Table_BL_ERA_835_ClaimRow)claim_dr;
                                        string claimId = MDVUtility.ToStr(row.FIELD_CLAIM_ID_PK);
                                        claim_dr.Delete();

                                        // filter charges.
                                        if (IsFilterCharges)
                                        {
                                            dsCache.Table_BL_Remittance_CacheRow[] charge_rows = (dsCache.Table_BL_Remittance_CacheRow[])ds835Parser.Table_BL_Remittance_Cache.Select(ds835Parser.Table_BL_Remittance_Cache.FIELD_REMIT_CLAIM_ID_FKColumn.ColumnName + "='" + claimId + "'");
                                            foreach (DataRow charge_dr in charge_rows)
                                                charge_dr.Delete();
                                        }
                                    }

                                    if (IsFilterCharges)
                                    {
                                        dsCache.Table_BL_Remittance_CacheRow[] charge_rows = (dsCache.Table_BL_Remittance_CacheRow[])ds835Parser.Table_BL_Remittance_Cache.Select(ds835Parser.Table_BL_Remittance_Cache.FIELD_REMIT_CHARGE_NUMBERColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( ERAChargeNumber) );
                                        foreach (DataRow charge_dr in charge_rows)
                                            charge_dr.Delete();

                                    }


                                }


                                str = ReportFormat835(ds835Parser);
                            }
                            else if ((str2 != "997") && (str2 != "277"))
                            {
                                str = "Unknown EDI";
                            }
                        }
                    }
                    else
                    {
                        str = "Not an EDI : " + EDIStr;
                    }
                }
                str3 = str;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str3;
        }

        private static string ReportFormat835(dsCache ds835Parser)
        {
            string str36;
            try
            {
                string str = null;
                string str2 = null;
                Stack stack = new Stack();
                string str3 = "MD Vision";
                string str4 = "PROVIDER ELECTRONIC REMITTANCE ADVICE";
                string str5 = DateTime.Now.Date.ToShortDateString();
                string str6 = DateTime.Now.Date.ToShortTimeString();
                str2 = str3.PadLeft(50) + "\r\n" + str4.PadLeft(0x3f) + "REPORT DATE: ".PadLeft(30) + str5 + "\r\n".PadRight(0x52) + "PRINT TIME: " + str6;
                foreach (dsCache.Table_BL_ERA_835Row row in ds835Parser.Table_BL_ERA_835.Rows)
                {
                    string str7 = null;
                    string str8 = null;
                    string str9 = null;
                    string str10 = null;
                    string str11 = null;
                    string str12 = row.FIELD_ERA835_ID_PK.ToString();
                    dsCache.Table_BL_ERA_835Row[] rowArray = (dsCache.Table_BL_ERA_835Row[])ds835Parser.Table_BL_ERA_835.Select(ds835Parser.Table_BL_ERA_835.FIELD_ERA835_ID_PKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( str12));
                    foreach (dsCache.Table_BL_ERA_835Row row2 in rowArray)
                    {
                        string str13 = "";
                        string str14 = "";
                        string str15 = row2.FIELD_ERA835_PAYER_NAME;
                        string str16 = row2.FIELD_ERA835_PAYER_ADDRESS + "\r\n" + row2.FIELD_ERA835_PAYER_CITY.ToString() + ", " + row2.FIELD_ERA835_PAYER_STATE.ToString() + " " + row2.FIELD_ERA835_PAYER_POSTAL_CODE.ToString();
                        if (row2.FIELD_ERA835_PAYER_PHONE_NO != "")
                        {
                            str14 = "\r\n\r\nPAYER BUSINESS CONTACT INFORMATION:\r\n" + row2.FIELD_ERA835_PAYER_PHONE_NO + "\r\n";
                        }
                        if (row2.FIELD_ERA835_PAYER_TEC_NO != "")
                        {
                            str13 = "\r\nPAYER TECHNICAL CONTACT INFORMATION:\r\n" + row2.FIELD_ERA835_PAYER_TEC_NAME + "\r\n" + row2.FIELD_ERA835_PAYER_TEC_NO + "\r\n";
                        }
                        string str17 = row2.FIELD_ERA835_PAYEE_NAME.ToString();
                        string str18 = row2.FIELD_ERA835_PAYEE_ADDRESS.ToString();
                        string str19 = row2.FIELD_ERA835_PAYEE_CITY.ToString() + ", " + row2.FIELD_ERA835_PAYEE_STATE.ToString() + " " + row2.FIELD_ERA835_PAYEE_POSTAL_CODE.ToString();
                        if (row2.FIELD_ERA835_PROVIDER_NO.ToString() != "")
                        {
                            str7 = row2.FIELD_ERA835_PROVIDER_NO.ToString();
                        }
                        else
                        {
                            str8 = row2.FIELD_ERA835_BILLING_PROVIDER_NPI.ToString();
                        }
                        if (str8 != "")
                        {
                            str7 = "NPI #: " + str8;
                        }
                        else
                        {
                            str7 = "PROVIDER #: " + str7;
                        }
                        string str20 = row2.FIELD_ERA835_CHECK_DATE;
                        string str21 = row2.FIELD_ERA835_CHEQUE_NO.ToString();
                        decimal totaolAmount = 0;
                        if (!row2.IsFIELD_ERA835_CHECK_EFT_AMOUNTNull())
                        {
                            totaolAmount = Convert.ToDecimal(row2.FIELD_ERA835_CHECK_EFT_AMOUNT);
                        }
                        str9 = "\r\n" + str15.ToString() + "\r\n" + str16 + str14 + str13 + "\r\n\r\n\r\n" + str17.ToString().PadRight(80) + str7 + "\r\n" + str18.ToString().PadRight(80) + "DATE: " + str20 + "\r\n" + str19.PadRight(80) + "CHECK/EFT #: " + str21 + "\r\n".PadRight(0x52) + "Total Amount: " + string.Format("{0:0.00}", totaolAmount);
                        dsCache.Table_BL_ERA_835_ClaimRow[] rowArray2 = (dsCache.Table_BL_ERA_835_ClaimRow[])ds835Parser.Table_BL_ERA_835_Claim.Select(ds835Parser.Table_BL_ERA_835_Claim.FIELD_CLAIM_ERA835_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( str12) );
                        foreach (dsCache.Table_BL_ERA_835_ClaimRow row3 in rowArray2)
                        {
                            decimal num = 0M;
                            decimal num2 = 0M;
                            decimal num3 = 0M;
                            decimal num4 = 0M;
                            decimal num5 = 0M;
                            string str22 = null;
                            str11 = "REND PROV  SERV DATE   POS NOS   PROC   MODS        BILLED    ALLOWED  DEDUCT    COINS   GRP/RC-AMT         PROV PD\r\n____________________________________________________________________________________________________________________";
                            str11 = str11 + "\r\nNAME:" + row3.FIELD_CLAIM_PATIENT_NAME.ToString().PadRight(0x1c) + row3.FIELD_CLAIM_HICN_QUALIFIER.ToString() + ":" + row3.FIELD_CLAIM_HICN.ToString().PadRight(15) + "ACNT:" + row3.FIELD_CLAIM_ACNT.ToString().PadRight(0x15) + "ICN:" + row3.FIELD_CLAIM_ICN.ToString().PadRight(0x11) + "ASG:" + row2.FIELD_ERA835_ASG.PadRight(4) + "MOA:" + row3.FIELD_CLAIM_MOA_CODE.ToString();
                            string[] strArray = null;
                            int index = 0;
                            string[] separator = new string[] { "\r\n" };
                            strArray = row3.FIELD_CLAIM_MOA_REMARKS.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            for (index = 0; index <= (strArray.Length - 1); index++)
                            {
                                if ((strArray[index].ToString().Trim() != "") && !stack.Contains(strArray[index].ToString().Trim()))
                                {
                                    stack.Push(strArray[index].ToString().Trim());
                                }
                            }
                            dsCache.Table_BL_Remittance_CacheRow[] rowArray3 = (dsCache.Table_BL_Remittance_CacheRow[])ds835Parser.Table_BL_Remittance_Cache.Select(ds835Parser.Table_BL_Remittance_Cache.FIELD_REMIT_CLAIM_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( row3.FIELD_CLAIM_ID_PK.ToString()) );
                            foreach (dsCache.Table_BL_Remittance_CacheRow row4 in rowArray3)
                            {
                                string str23 = "";
                                string str24 = "";
                                string str25 = "";
                                string str26 = "";
                                string str27 = "";
                                dsCache.Table_BL_Remittance_WriteOffRow[] rowArray4 = (dsCache.Table_BL_Remittance_WriteOffRow[])ds835Parser.Table_BL_Remittance_WriteOff.Select(ds835Parser.Table_BL_Remittance_WriteOff.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(row4.FIELD_REMIT_BL_REMITTANCE_ID_PK.ToString()));
                                if ((row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim() != "") && !stack.Contains(row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim()))
                                {
                                    stack.Push(row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim());
                                } 
                                foreach (dsCache.Table_BL_Remittance_WriteOffRow row5 in rowArray4)
                                {
                                    if ((row5.FIELD_WRITE_OFF_GROUP_DESCRIPTION.ToString().Trim() != "") && !stack.Contains(row5.FIELD_WRITE_OFF_GROUP_DESCRIPTION.ToString().Trim()))
                                    {
                                        stack.Push(row5.FIELD_WRITE_OFF_GROUP_DESCRIPTION.ToString().Trim());
                                    }
                                    if ((row5.FIELD_WRITE_OFF_REASON.ToString().Trim() != "") && !stack.Contains(row5.FIELD_WRITE_OFF_REASON.ToString().Trim()))
                                    {
                                        stack.Push(row5.FIELD_WRITE_OFF_REASON.ToString().Trim());
                                    }
                                    if (row5.FIELD_WRITE_OFF.ToString() != "")
                                    {
                                        num4 += Convert.ToDecimal(row5.FIELD_WRITE_OFF.ToString());
                                    }
                                    if (row5.FIELD_WRITE_OFF_GROUP_CODE.ToString() != "")
                                    {
                                        if (string.IsNullOrEmpty(str23))
                                        {
                                            str23 = row5.FIELD_WRITE_OFF_GROUP_CODE.ToString() + "-" + row5.FIELD_WRITE_OFF_REASON_CODE.ToString().PadRight(4) + row5.FIELD_WRITE_OFF.ToString();
                                        }
                                        else if (string.IsNullOrEmpty(str24))
                                        {
                                            str24 = row5.FIELD_WRITE_OFF_GROUP_CODE.ToString() + "-" + row5.FIELD_WRITE_OFF_REASON_CODE.ToString().PadRight(4) + row5.FIELD_WRITE_OFF.ToString();
                                        }
                                        else
                                        {
                                            str24 = str24 + "\r\n" + "".PadRight(0x59) + row5.FIELD_WRITE_OFF_GROUP_CODE.ToString() + "-" + row5.FIELD_WRITE_OFF_REASON_CODE.ToString().PadRight(4) + row5.FIELD_WRITE_OFF.ToString();
                                        }
                                    }
                                }
                                if (row4.FIELD_REMIT_REND_PROVIDER_NPI.ToString() != "")
                                {
                                    str8 = row4.FIELD_REMIT_REND_PROVIDER_NPI.ToString();
                                }
                                string str28 = "";
                                string str29 = "";
                                if (row4.FIELD_REMIT_SERVICE_FROM_DATE.ToString() != "")
                                {
                                    str28 = row4.FIELD_REMIT_SERVICE_FROM_DATE.ToString().Substring(4, 4);
                                    str29 = row4.FIELD_REMIT_SERVICE_FROM_DATE.ToString().Substring(4, 4) + row4.FIELD_REMIT_SERVICE_FROM_DATE.ToString().Substring(2, 2);
                                }
                                decimal num7 = 0M;
                                if (row4.FIELD_REMIT_SERVICE_QTY.ToString() != "")
                                {
                                    num7 = Convert.ToDecimal(row4.FIELD_REMIT_SERVICE_QTY.ToString());
                                }
                                if (!row4.IsFIELD_REMIT_ALLOWEDNull())
                                {
                                    num2 += Convert.ToDecimal(row4.FIELD_REMIT_ALLOWED.ToString());
                                }
                                if (!row4.IsFIELD_REMIT_COINSURANCENull())
                                {
                                    num3 += Convert.ToDecimal(row4.FIELD_REMIT_COINSURANCE.ToString());
                                }
                                if (!row4.IsFIELD_REMIT_DEDUCTIBLENull())
                                {
                                    num += Convert.ToDecimal(row4.FIELD_REMIT_DEDUCTIBLE.ToString());
                                }
                                if (!row4.IsFIELD_REMIT_LATE_FILING_CHARGENull())
                                {
                                    num5 += Convert.ToDecimal(row4.FIELD_REMIT_LATE_FILING_CHARGE.ToString());
                                }
                                string str30 = "";
                                string str31 = "";
                                string str32 = "";
                                string str33 = "";
                                if (row4.IsFIELD_REMIT_CHARGE_AMOUNTNull())
                                {
                                    str30 = "0.00";
                                }
                                else
                                {
                                    str30 = string.Format("{0:0.00}", row4.FIELD_REMIT_CHARGE_AMOUNT);
                                }
                                if (row4.IsFIELD_REMIT_ALLOWEDNull())
                                {
                                    str31 = "0.00";
                                }
                                else
                                {
                                    str31 = string.Format("{0:0.00}", row4.FIELD_REMIT_ALLOWED);
                                }
                                if (row4.IsFIELD_REMIT_DEDUCTIBLENull())
                                {
                                    str32 = "0.00";
                                }
                                else
                                {
                                    str32 = string.Format("{0:0.00}", row4.FIELD_REMIT_DEDUCTIBLE);
                                }
                                if (row4.IsFIELD_REMIT_COINSURANCENull())
                                {
                                    str33 = "0.00";
                                }
                                else
                                {
                                    str33 = string.Format("{0:0.00}", row4.FIELD_REMIT_COINSURANCE);
                                }
                                str11 = str11 + "\r\n" + str8.PadRight(11) + str28.PadRight(5) + str29.PadRight(7) + row4.FIELD_REMIT_SERVICE_POS.ToString().PadRight(5) + num7.ToString().PadRight(5) + row4.FIELD_REMIT_HCPCS.ToString().PadRight(7) + row4.FIELD_REMIT_MODS.ToString().PadRight(12) + str30.PadRight(10) + str31.PadRight(9) + str32.PadRight(10) + str33.PadRight(8) + str23.PadRight(0x13) + string.Format("{0:0.00}", row4.FIELD_REMIT_PAID_AMOUNT);
                                if (row4.FIELD_REMIT_HC_CODE.ToString().Trim() != "")
                                {
                                    str25 = "REM:".PadRight(5) + row4.FIELD_REMIT_HC_CODE.ToString();
                                    if ((row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim() != "") && !stack.Contains(row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim()))
                                    {
                                        stack.Push(row4.FIELD_REMIT_HC_REMARK_CODE.ToString().Trim());
                                    }
                                }
                                if (row4.FIELD_REMIT_COMPOSITE_HCPCS.ToString().Trim() != "")
                                {
                                    str26 = "(" + row4.FIELD_REMIT_COMPOSITE_HCPCS.ToString() + ")";
                                }
                                if (row4.FIELD_REMIT_SERVICE_ORG_QTY.ToString() != "")
                                {
                                    str27 = "SUB NOS:".PadRight(11) + row4.FIELD_REMIT_SERVICE_ORG_QTY.ToString();
                                }
                                if ((!string.IsNullOrEmpty(str27) | !string.IsNullOrEmpty(str25)) | !string.IsNullOrEmpty(str24))
                                {
                                    str11 = str11 + "\r\n" + "".PadLeft(0x11) + str27.PadRight(0x10) + str26.PadRight(8) + str25.PadRight(0x30) + str24 + "\r\nCNTL #: " + row4.FIELD_REMIT_CHARGE_NUMBER.ToString();
                                }
                            }
                            string str34 = "0.00";
                            if (row3.FIELD_CLAIM_PR_AMOUNT.ToString().Trim() != "")
                            {
                                str34 = row3.FIELD_CLAIM_PR_AMOUNT.ToString().Trim();
                            }
                            str22 = "PT RESP".PadRight(13) + str34.ToString().PadRight(0x12) + "CLAIM TOTALS".PadRight(0x15) + string.Format("{0:0.00}", row3.FIELD_CLAIM_BILLED_AMOUNT).PadRight(10) + string.Format("{0:0.00}", num2).PadRight(9) + string.Format("{0:0.00}", num).PadRight(10) + string.Format("{0:0.00}", num3).PadRight(15) + string.Format("{0:0.00}", num4).PadRight(12) + string.Format("{0:0.00}", row3.FIELD_CLAIM_PAID_AMOUNT);
                            str22 = str22 + "\r\n" + "ADJ TO TOTALS: PREV PD".PadRight(0x1f) + "INTEREST   " + string.Format("{0:0.00}", row3.FIELD_CLAIM_INTEREST).PadRight(15) + "LATE FILLING CHARGE   " + string.Format("{0:0.00}", num5).PadRight(0x15) + "NET".PadRight(8) + string.Format("{0:0.00}", row3.FIELD_CLAIM_PAID_AMOUNT + row3.FIELD_CLAIM_INTEREST);
                            if (row3.FIELD_CLAIM_FORWARD_PAYER.ToString() != "")
                            {
                                str22 = str22 + "\r\n" + "CLAIM INFORMATION FORWARDED TO:".PadRight(0x22) + row3.FIELD_CLAIM_FORWARD_PAYER.ToString().PadRight(0x19) + " " + row3.FIELD_CLAIM_FORWARD_PAYER_CODE.ToString();
                            }
                            dsCache.Table_BL_Remittance_Other_Claim_RelatedRow[] rowArray5 = (dsCache.Table_BL_Remittance_Other_Claim_RelatedRow[])ds835Parser.Table_BL_Remittance_Other_Claim_Related.Select(ds835Parser.Table_BL_Remittance_Other_Claim_Related.FIELD_OTHER_CLAIM_ID_FKColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString( row3.FIELD_CLAIM_ID_PK.ToString() ));
                            string str35 = "";
                            foreach (dsCache.Table_BL_Remittance_Other_Claim_RelatedRow row6 in rowArray5)
                            {
                                if (string.IsNullOrEmpty(str35))
                                {
                                    str35 = "(" + row6.FIELD_OTHER_CLAIM_REL_QUALIFIER.ToString() + ") " + row6.FIELD_OTHER_CLAIM_REL.ToString();
                                }
                                else
                                {
                                    str35 = str35 + "\r\n" + "".PadRight(0x22) + "(" + row6.FIELD_OTHER_CLAIM_REL_QUALIFIER.ToString() + ") " + row6.FIELD_OTHER_CLAIM_REL.ToString();
                                }
                            }
                            if (str35 != "")
                            {
                                str22 = str22 + "\r\n" + "OTHER CLAIM REL IDENTIFICATION:".PadRight(0x22) + str35;
                            }
                            str10 = str10 + "\r\n\r\n" + str11 + "\r\n" + str22;
                        }
                        str10 = str9 + "\r\n" + str10 + "\r\n====================================================================================================================\n" + str;

                    }


                    string str37 = string.Empty;
                    string str37_header = "PLB ADJUSTMENT DETAILS \n" + "REASON          FCN/OTHER IDENTIFIER                            AMOUNT"
                        + "\r\n____________________________________________________________________________________________________________________\n";

                    dsCache.Table_BL_Provider_AdjustmentRow[] ProviderArray = (dsCache.Table_BL_Provider_AdjustmentRow[])ds835Parser.Table_BL_Provider_Adjustment.Select(ds835Parser.Table_BL_Provider_Adjustment.FIELD_REMIT_BATCH_ID_FKColumn.ColumnName + "="  + MDVUtility.ToLINQFormatString( str12 ));
                    foreach (dsCache.Table_BL_Provider_AdjustmentRow provider_row in ProviderArray)
                    {
                        str37 += provider_row.FIELD_PLB_REASON_CODE.PadRight(0x10) + provider_row.FIELD_PLB_REFRENCE_IDENTIFIER.PadRight(0x30) + provider_row.FIELD_PLB_AMOUNT + "\n";
                    }

                    if (str37 != "")
                    {
                        str10 += "\r\n\n" + str37_header + "\r\n" + str37;
                        str10 += "\r\n====================================================================================================================\r\n\r\n";
                    }

                    str = str10;
                }
                str = str2 + str + "\r\n\r\n";
                int count = stack.Count;
                for (int i = 1; i <= count; i++)
                {
                    str = str + BreakContent(stack.Pop().ToString()) + "\r\n";
                }
                str36 = str;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str36;
        }

        private static IEnumerable<string> SplitEvery(this string s, int n)
        {
            int index = 0;
            return s.GroupBy(_ => index++ / n).Select(g => new string(g.ToArray()));
        }

        private static string BreakContent(string str_input)
        {
            string str_output = "";
            try
            {

                string str_temp = "";
                if (str_input.Length > 120)
                {
                    var res2 = str_input.SplitEvery(119).ToList();
                    foreach (var item in res2)
                    {
                        string temp = item + "\n";
                        str_temp += temp;
                    }
                }
                else
                    str_temp = str_input;

                str_output = str_temp;
            }
            catch (Exception)
            {
            }

            return str_output;
        }

        public static string StrRev(string Str)
        {
            string str;
            try
            {
                int index = 0;
                char[] array = null;
                StringBuilder builder = new StringBuilder();
                array = Str.ToCharArray();
                Array.Reverse(array);
                for (index = 0; index <= (array.Length - 1); index++)
                {
                    builder.Append(array[index]);
                }
                str = builder.ToString().Trim();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public static int HLBillingProvider
        {
            get
            {
                return HL_Billing_Provider;
            }
            set
            {
                HL_Billing_Provider = value;
            }
        }

        public static int HLCount
        {
            get
            {
                return HLCounter;
            }
            set
            {
                HLCounter = value;
            }
        }

        public static int HLSubscriber
        {
            get
            {
                return HL_Subscriber;
            }
            set
            {
                HL_Subscriber = value;
            }
        }

        public static int SegCount
        {
            get
            {
                return SegmentCounter;
            }
            set
            {
                SegmentCounter = value;
            }
        }
    }
}

