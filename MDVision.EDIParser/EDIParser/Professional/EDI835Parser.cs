namespace EDIParser.Professional
{
    using EDIParser;
    using Microsoft.CSharp.RuntimeBinder;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using MDVision.Common.Utilities;
    public class EDI835Parser
    {
        private string BPR04_Value = "";
        private int ClaimId = 0;
        private IContainer components;
        private long CurrentBatchNumber = 0L;
        private long CurrentClaimNumber = 0L;
        private long CurrentERA835Number = 0L;
        private long CurrentRemitNumber = 0L;
        private dsCache ds835Parser = new dsCache();
        private string FileName = "";
        private StringBuilder FinalStr835;
        private string PayerAndPayeeQualifier;
        private DateTime ReportDate;
        private long ReportId;
        private DateTime ReportTime;
        private dsCache.TableBatchRow rowBatch;
        private dsCache.Table_BL_Remittance_CacheRow rowCache;
        private dsCache.Table_BL_ERA_835_ClaimRow rowClaim;
        private dsCache.Table_BL_ERA_835Row rowERA;
        private dsCache.Table_BL_Remittance_Other_Claim_RelatedRow rowOtherRef;
        private dsCache.Table_BL_Remittance_WriteOffRow rowWriteOff;
        private dsCache.Table_BL_Provider_AdjustmentRow rowProviderAdjustment;
        string ProviderIdentifier = string.Empty;
        string ProviderDate = string.Empty;
        private string UserName = "";

        public EDI835Parser()
        {
            this.InitializeComponent();
        }

        private string AK101(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "HP"))
            {
                return "Health Care Claim Payment/Advice (835)";
            }
            return "### Unknown AK101 ###";
        }

        private string AK201(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "835"))
            {
                return "Health Care Claim Payment/Advice";
            }
            return "### Unknown AK201 ###";
        }

        private string AK304(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Unrecognized segment ID";

                case "2":
                    return "Unexpected segment";

                case "3":
                    return "Mandatory segment missing";

                case "4":
                    return "Loop Occurs Over Maximum Times";

                
            }
            return "### Unknown AK304 ###";
        }

        private string AK403(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Mandatory data element missing";

                case "2":
                    return "Conditional required data element missing.";

                case "3":
                    return "Too many data elements.";

                case "4":
                    return "Data element too short.";

                case "5":
                    return "Data element too long.";

                case "6":
                    return "Invalid character in data element.";

            
            }
            return "### Unknown AK403 ###";
        }

        private string AK501(string Code)
        {
            switch (Code)
            {
                case "A":
                    return "Accepted ADVISED";

                case "E":
                    return "Accepted But Errors Were Noted";

                case "M":
                    return "Rejected, Message Authentication Code (MAC) Failed";

                case "R":
                    return "Rejected ADVISED";

                case "W":
                    return "Rejected, Assurance Failed Validity Tests";

                case "X":
                    return "Rejected, Content After Decryption Could Not Be Analyzed";
            }
            return "### Unknown AK501 ###";
        }

        private string AK502(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Transaction Set Not Supported";

                case "2":
                    return "Transaction Set Trailer Missing";

                case "3":
                    return "Transaction Set Control Number in Header and Trailer Do Not Match";

                case "4":
                    return "Number of Included Segments Does Not Match Actual Count";

                case "5":
                    return "One or More Segments in Error";

                case "6":
                    return "Missing or Invalid Transaction Set Identifier";

                case "7":
                    return "Missing or Invalid Transaction Set Control Number";

                case "8":
                    return "Authentication Key Name Unknown";

                case "9":
                    return "Encryption Key Name Unknown";

                case "10":
                    return "Requested Service (Authentication or Encrypted)Not Available";

                case "11":
                    return "Unknown Security Recipient";

                case "12":
                    return "Incorrect Message Length (Encryption Only)";

                case "13":
                    return "Message Authentication Code Failed";

                case "14":
                    return "Accepted But Errors Were Noted";

                case "15":
                    return "Unknown Security Originator";

                case "16":
                    return "Syntax Error in Decrypted Text";

                case "17":
                    return "Security Not Supported";

               
            }
            return "### Unknown AK502 ###";
        }

        private string AK901(string Code)
        {
            switch (Code)
            {
                case "A":
                    return "Accepted ADVISED";

                case "E":
                    return "Accepted But Errors Were Noted";

                case "M":
                    return "Rejected, Message Authentication Code (MAC) Failed";

                case "R":
                    return "Rejected ADVISED";

                case "W":
                    return "Rejected, Assurance Failed Validity Tests";

                case "X":
                    return "Rejected, Content After Decryption Could Not Be Analyzed";
            }
            return "### Unknown AK901 ###";
        }

        private string AK905(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Functional Group Not Supported";

                case "2":
                    return "Functional Group Version Not Supported";

                case "3":
                    return "Functional Group Trailer Missing";

                case "4":
                    return "Group Control Number in the Functional Group Header and Trailer Do Not Agree";

                case "5":
                    return "Number of Included Transaction Sets Does Not Match Actual Count";

                case "6":
                    return "Group Control Number Violates Syntax";

                case "10":
                    return "Authentication Key Name Unknown";

                case "11":
                    return "Encryption Key Name Unknown";

                case "12":
                    return "Requested Service (Authentication or Encryption) Not Available";

                case "13":
                    return "Unknown Security Recipient";

                case "14":
                    return "Unknown Security Originator";

                case "15":
                    return "Syntax Error in Decrypted Text";

                case "16":
                    return "Security Not Supported";

                case "17":
                    return "Incorrect Message Length (Encryption Only)";

                case "18":
                    return "Message Authentication Code Failed";

               
            }
            return "### Unknown AK905 ###";
        }

        private string AMT(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "AMT")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string AMT01(string Code)
        {
            switch (Code)
            {
                case "AU":
                    return "Coverage Amount";

                case "D8":
                    return "Discount Amount";

                case "DY":
                    return "Per Day Limit";

                case "F5":
                    return "Patient Amount Paid";

                case "I":
                    return "Interest";

                case "NL":
                    return "Negative Ledger Balance";

                case "T":
                    return "Tax";

                case "T2":
                    return "Total Claim Before Taxes";

                case "ZK":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 1";

                case "ZL":
                    return "Federal Medicare or Medicaid Payment Mandate -Category 2";

                case "ZM":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 3";

                
            }
            return "### Unknown AMT01 ###";
        }

        private string BPR(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "BPR")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                    0x11, 0x12, 0x13, 20, 0x15
                 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    DateTime time;
                    int num2 = numArray[index];
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                builder.Append(this.BPR01(strArray[num2]));
                                continue;
                            }
                        case 2:
                            {
                                builder.Append("\r\n");
                                builder.Append("Total Actual Provider Payment Amount: ");
                                builder.Append(strArray[num2]);
                                this.rowERA.FIELD_ERA835_CHECK_EFT_AMOUNT = strArray[num2];
                                this.rowBatch.FIELD_BATCH_AMOUNT = Convert.ToDecimal(strArray[num2]);
                                continue;
                            }
                        case 3:
                            {
                                builder.Append('\t');
                                builder.Append(this.BPR03(strArray[num2]));
                                this.rowERA.FIELD_ERA835_CHECK_EFT_TYPE = this.BPR03(strArray[num2]);
                                continue;
                            }
                     

                        default:
                            goto Label_0540;
                    }
                    if (this.BPR04_Value == "CHK")
                    {
                        builder.Append("Check Issue Date: ");
                        this.rowERA.FIELD_ERA835_CHECK_EFT_TYPE = this.BPR04_Value;
                    }
                    else if (this.BPR04_Value == "FWT")
                    {
                        builder.Append("Payer anticipates the money to move Date: ");
                        this.rowERA.FIELD_ERA835_CHECK_EFT_TYPE = this.BPR04_Value;
                    }
                    else
                    {
                        builder.Append("Check Issue or EFT Effective Date: ");
                        this.rowERA.FIELD_ERA835_CHECK_EFT_TYPE = this.BPR04_Value;
                    }
              
                Label_0540:
                    builder.Append("\r\n");
                    builder.Append("## this BPR segment not in use ##");
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string BPR01(string Code)
        {
            switch (Code)
            {
                case "C":
                    return "Payment Accompanies Remittance Advice";

                case "D":
                    return "Make Payment Only";

                case "H":
                    return "Notification Only";

                case "I":
                    return "Remittance Information Only";

                case "P":
                    return "Prenotification of Future Transfers";

                case "U":
                    return "Split Payment and Remittance";

                case "X":
                    return "Handling Party’s Option to Split Payment and Remittance";
            }
            return "### Unknown BPR01 ###";
        }

        private string BPR03(string Code)
        {
            switch (Code)
            {
                case "C":
                    return "Credit";

                case "D":
                    return "Debit";
            }
            return "### Unknown BPR03 ###";
        }

        private string BPR04(string Code)
        {
            switch (Code)
            {
                case "ACH":
                    return "Automated Clearing House (ACH)";

                case "BOP":
                    return "Financial Institution Option";

                case "CHK":
                    return "Check";

                case "FWT":
                    return "Federal Reserve Funds/Wire Transfer - Nonrepetitive";

                case "NON":
                    return "Non-Payment Data";
            }
            return "### Unknown BPR04 ###";
        }

        private string BPR05(string Code)
        {
            switch (Code)
            {
                case "CCP":
                    return "Cash Concentration/Disbursement plus Addenda (CCD+) (ACH)";

                case "CTX":
                    return "Corporate Trade Exchange (CTX) (ACH)";
            }
            return "### Unknown BPR05 ###";
        }

        private string BPR06(string Code)
        {
            switch (Code)
            {
                case "01":
                    return "ABA Transit Routing Number Including Check Digits (9 digits)";

                case "04":
                    return "Canadian Bank Branch and Institution Number";
            }
            return "### Unknown BPR06 ###";
        }

        private string BPR08(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "DA"))
            {
                return "Demand Deposit";
            }
            return "### Unknown BPR08 ###";
        }

        private string BPR12(string Code)
        {
            switch (Code)
            {
                case "01":
                    return "ABA Transit Routing Number Including Check Digits (9 digits)";

                case "04":
                    return "Canadian Bank Branch and Institution Number";
            }
            return "### Unknown BPR12 ###";
        }

        private string BPR14(string Code)
        {
            switch (Code)
            {
                case "DA":
                    return "Demand Deposit";

                case "SG":
                    return "Savings";
            }
            return "### Unknown BPR14 ###";
        }

        private string CAS(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "CAS")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                    0x11, 0x12, 0x13
                 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append("Claim Adjustment Group: ");
                                builder.Append(this.CAS01(strArray[index]));
                                 break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                               
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity: ");
                                builder.Append(strArray[index]);
                                break;

                            case 5:
                                this.rowWriteOff = this.GetNewWriteOffRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                               
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                if (!(strArray[1] == "PR"))
                                {
                                    goto Label_04D3;
                                }
                                if (!(strArray[index - 1] == "1"))
                                {
                                    goto Label_043E;
                                }
                                this.rowCache.FIELD_REMIT_DEDUCTIBLE = Convert.ToDecimal(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_DEDUCTIBLE = strArray[index];
                                goto Label_04C0;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity ");
                                builder.Append(strArray[index]);
                                break;

                            case 8:
                                this.rowWriteOff = this.GetNewWriteOffRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                               
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                if (!(strArray[1] == "PR"))
                                {
                                    goto Label_06D2;
                                }
                                if (!(strArray[index - 1] == "1"))
                                {
                                    goto Label_063D;
                                }
                                this.rowCache.FIELD_REMIT_DEDUCTIBLE = Convert.ToDecimal(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_DEDUCTIBLE = strArray[index];
                                goto Label_06BF;

                            case 10:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity ");
                                builder.Append(strArray[index]);
                                break;

                            case 11:
                                this.rowWriteOff = this.GetNewWriteOffRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                               
                                break;

                            case 12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                if (!(strArray[1] == "PR"))
                                {
                                    goto Label_08D1;
                                }
                                if (!(strArray[index - 1] == "1"))
                                {
                                    goto Label_083C;
                                }
                                this.rowCache.FIELD_REMIT_DEDUCTIBLE = Convert.ToDecimal(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_DEDUCTIBLE = strArray[index];
                                goto Label_08BE;

                            case 13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity ");
                                builder.Append(strArray[index]);
                                break;

                            case 14:
                                this.rowWriteOff = this.GetNewWriteOffRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                               
                                break;

                            case 15:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                if (!(strArray[1] == "PR"))
                                {
                                    goto Label_0AD0;
                                }
                                if (!(strArray[index - 1] == "1"))
                                {
                                    goto Label_0A3B;
                                }
                                this.rowCache.FIELD_REMIT_DEDUCTIBLE = Convert.ToDecimal(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_DEDUCTIBLE = strArray[index];
                                goto Label_0ABD;

                            case 0x10:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x11:
                                this.rowWriteOff = this.GetNewWriteOffRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Reason: ");
                                builder.Append(this.ClaimAdjustmentCode(strArray[index]));
                                this.rowWriteOff.FIELD_WRITE_OFF_GROUP_CODE = strArray[1];
                                this.rowWriteOff.FIELD_WRITE_OFF_GROUP_DESCRIPTION = "{ " + strArray[1] + " } -  " + this.CAS01(strArray[1]);
                                this.rowWriteOff.FIELD_WRITE_OFF_REASON = "{ " + strArray[index] + " } -  " + this.ClaimAdjustmentCode(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_REASON_CODE = strArray[index];
                                break;

                            case 0x12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                if (!(strArray[1] == "PR"))
                                {
                                    goto Label_0CCF;
                                }
                                if (!(strArray[index - 1] == "1"))
                                {
                                    goto Label_0C3A;
                                }
                                this.rowCache.FIELD_REMIT_DEDUCTIBLE = Convert.ToDecimal(strArray[index]);
                                this.rowWriteOff.FIELD_WRITE_OFF_DEDUCTIBLE = strArray[index];
                                goto Label_0CBC;

                            case 0x13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Adjustment Quantity ");
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                    continue;
               
                Label_043E:
                    if (strArray[index - 1] == "2")
                    {
                        this.rowCache.FIELD_REMIT_COINSURANCE = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COINSURANCE = strArray[index];
                    }
                    else if (strArray[index - 1] == "3")
                    {
                        this.rowCache.FIELD_REMIT_COPAYMENT = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COPAYMENT = strArray[index];
                    }
                Label_04C0:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_04D3:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_063D:
                    if (strArray[index - 1] == "2")
                    {
                        this.rowCache.FIELD_REMIT_COINSURANCE = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COINSURANCE = strArray[index];
                    }
                    else if (strArray[index - 1] == "3")
                    {
                        this.rowCache.FIELD_REMIT_COPAYMENT = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COPAYMENT = strArray[index];
                    }
                Label_06BF:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_06D2:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_083C:
                    if (strArray[index - 1] == "2")
                    {
                        this.rowCache.FIELD_REMIT_COINSURANCE = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COINSURANCE = strArray[index];
                    }
                    else if (strArray[index - 1] == "3")
                    {
                        this.rowCache.FIELD_REMIT_COPAYMENT = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COPAYMENT = strArray[index];
                    }
                Label_08BE:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_08D1:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_0A3B:
                    if (strArray[index - 1] == "2")
                    {
                        this.rowCache.FIELD_REMIT_COINSURANCE = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COINSURANCE = strArray[index];
                    }
                    else if (strArray[index - 1] == "3")
                    {
                        this.rowCache.FIELD_REMIT_COPAYMENT = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COPAYMENT = strArray[index];
                    }
                Label_0ABD:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_0AD0:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_0C3A:
                    if (strArray[index - 1] == "2")
                    {
                        this.rowCache.FIELD_REMIT_COINSURANCE = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COINSURANCE = strArray[index];
                    }
                    else if (strArray[index - 1] == "3")
                    {
                        this.rowCache.FIELD_REMIT_COPAYMENT = Convert.ToDecimal(strArray[index]);
                        this.rowWriteOff.FIELD_WRITE_OFF_COPAYMENT = strArray[index];
                    }
                Label_0CBC:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                    continue;
                Label_0CCF:
                    this.rowWriteOff.FIELD_WRITE_OFF = strArray[index];
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string CAS01(string Code)
        {
            switch (Code)
            {
                case "CO":
                    return "Contractual Obligations";

                case "CR":
                    return "Correction and Reversals";

                case "OA":
                    return "Other Adjustment";

                case "PI":
                    return "Payer Initiated Reductions";

                case "PR":
                    return "Patient Responsibility";
            }
            return "### Unknown CAS01 ###";
        }

        private string ClaimAdjustmentCode(string Code)
        {
            string str = "Unknown";
            try
            {
                switch (Code)
                {
                    case "1":
                        str = "Deductible Amount";
                        break;

                    case "2":
                        str = "Coinsurance Amount";
                        break;

                    case "3":
                        str = "Co-payment Amount";
                        break;

                    case "4":
                        str = "The procedure code is inconsistent with the modifier used or a required modifier is missing.";
                        break;
                    case "253":
                        str = "Sequestration - reduction in federal payment";
                        break;
                    case "237":
                        str = "Legislated/Regulatory Penalty. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;
                    case "5":
                        str = "The procedure code/bill type is inconsistent with the place of service.";
                        break;

                    case "6":
                        str = "The procedure/revenue code is inconsistent with the patient's age.";
                        break;

                    case "7":
                        str = "The procedure/revenue code is inconsistent with the patient's gender.";
                        break;

                    case "8":
                        str = "The procedure code is inconsistent with the provider type/specialty (taxonomy).";
                        break;

                    case "9":
                        str = "The diagnosis is inconsistent with the patient's age.";
                        break;

                    case "10":
                        str = "The diagnosis is inconsistent with the patient's gender.";
                        break;

                    case "11":
                        str = "The diagnosis is inconsistent with the procedure.";
                        break;

                    case "12":
                        str = "The diagnosis is inconsistent with the provider type.";
                        break;

                    case "13":
                        str = "The date of death precedes the date of service.";
                        break;

                    case "14":
                        str = "The date of birth follows the date of service.";
                        break;

                    case "15":
                        str = "Payment adjusted because the submitted authorization number is missing, invalid, or does not apply to the billed services or provider.";
                        break;

                    case "16":
                        str = "Claim/service lacks information which is needed for adjudication. Additional information is supplied using remittance advice remarks codes whenever appropriate";
                        break;

                    case "17":
                        str = "Payment adjusted because requested information was not provided or was insufficient/incomplete. Additional information is supplied using the remittance advice remarks codes whenever appropriate. Note: Changed as of 2/02";
                        break;

                    case "18":
                        str = "Duplicate claim/service.";
                        break;

                    case "19":
                        str = "Claim denied because this is a work-related injury/illness and thus the liability of the Worker's Compensation Carrier.";
                        break;

                    case "20":
                        str = "Claim denied because this injury/illness is covered by the liability carrier.";
                        break;

                    case "21":
                        str = "Claim denied because this injury/illness is the liability of the no-fault carrier.";
                        break;

                    case "22":
                        str = "Payment adjusted because this care may be covered by another payer per coordination of benefits.";
                        break;

                    case "23":
                        str = "Payment adjusted due to the impact of prior payer(s) adjudication including payments and/or adjustments";
                        break;

                    case "24":
                        str = "Payment for charges adjusted. Charges are covered under a capitation agreement/managed care plan.";
                        break;

                    case "25":
                        str = "Payment denied. Your Stop loss deductible has not been met.";
                        break;

                    case "26":
                        str = "Expenses incurred prior to coverage.";
                        break;

                    case "27":
                        str = "Expenses incurred after coverage terminated.";
                        break;

                    case "28":
                        str = "Coverage not in effect at the time the service was provided.";
                        break;

                    case "29":
                        str = "The time limit for filing has expired.";
                        break;

                    case "30":
                        str = "Payment adjusted because the patient has not met the required eligibility, spend down, waiting, or residency requirements.";
                        break;

                    case "31":
                        str = "Claim denied as patient cannot be identified as our insured.";
                        break;

                    case "32":
                        str = "Our records indicate that this dependent is not an eligible dependent as defined.";
                        break;

                    case "33":
                        str = "Claim denied. Insured has no dependent coverage.";
                        break;

                    case "34":
                        str = "Claim denied. Insured has no coverage for newborns.";
                        break;

                    case "35":
                        str = "Lifetime benefit maximum has been reached.";
                        break;

                    case "36":
                        str = "Balance does not exceed co-payment amount.";
                        break;

                    case "37":
                        str = "Balance does not exceed deductible.";
                        break;

                    case "38":
                        str = "Services not provided or authorized by designated (network/primary care) providers.";
                        break;

                    case "39":
                        str = "Services denied at the time authorization/pre-certification was requested.";
                        break;

                    case "40":
                        str = "Charges do not meet qualifications for emergent/urgent care.";
                        break;

                    case "41":
                        str = "Discount agreed to in Preferred Provider contract.";
                        break;

                    case "42":
                        str = "Charges exceed our fee schedule or maximum allowable amount.";
                        break;

                    case "43":
                        str = "Gramm-Rudman reduction.";
                        break;

                    case "44":
                        str = "Prompt-pay discount.";
                        break;

                    case "45":
                        str = "Charges exceed your contracted/ legislated fee arrangement.";
                        break;

                    case "46":
                        str = "This (these) service(s) is (are) not covered.";
                        break;

                    case "47":
                        str = "This (these) diagnosis(es) is (are) not covered, missing, or are invalid.";
                        break;

                    case "48":
                        str = "This (these) procedure(s) is (are) not covered.";
                        break;

                    case "49":
                        str = "These are non-covered services because this is a routine exam or screening procedure done in conjunction with a routine exam.";
                        break;

                    case "50":
                        str = "These are non-covered services because this is not deemed a `medical necessity' by the payer.";
                        break;

                    case "51":
                        str = "These are non-covered services because this is a pre-existing condition";
                        break;

                    case "52":
                        str = "The referring/prescribing/rendering provider is not eligible to refer/prescribe/order/perform the service billed.";
                        break;

                    case "53":
                        str = "Services by an immediate relative or a member of the same household are not covered.";
                        break;

                    case "54":
                        str = "Multiple physicians/assistants are not covered in this case .";
                        break;

                    case "55":
                        str = "Claim/service denied because procedure/treatment is deemed experimental/investigational by the payer.";
                        break;

                    case "56":
                        str = "Claim/service denied because procedure/treatment has not been deemed `proven to be effective' by the payer.";
                        break;

                    case "57":
                        str = "Payment denied/reduced because the payer deems the information submitted does not support this level of service, this many services, this length of service, this dosage, or this day's supply.";
                        break;

                    case "58":
                        str = "Payment adjusted because treatment was deemed by the payer to have been rendered in an inappropriate or invalid place of service.";
                        break;

                    case "59":
                        str = "Charges are adjusted based on multiple surgery rules or concurrent anesthesia rules.";
                        break;

                    case "60":
                        str = "Charges for outpatient services with this proximity to inpatient services are not covered.";
                        break;

                    case "61":
                        str = "Charges adjusted as penalty for failure to obtain second surgical opinion.";
                        break;

                    case "62":
                        str = "Payment denied/reduced for absence of, or exceeded, pre-certification/authorization.";
                        break;

                    case "63":
                        str = "Correction to a prior claim.";
                        break;

                    case "64":
                        str = "Denial reversed per Medical Review.";
                        break;

                    case "65":
                        str = "Procedure code was incorrect. This payment reflects the correct code. ";
                        break;

                    case "66":
                        str = "Blood Deductible.";
                        break;

                    case "67":
                        str = "Lifetime reserve days. (Handled in QTY, QTY01=LA)";
                        break;

                    case "68":
                        str = "DRG weight. (Handled in CLP12)";
                        break;

                    case "69":
                        str = "Day outlier amount.";
                        break;

                    case "70":
                        str = "Cost outlier - Adjustment to compensate for additional costs.";
                        break;

                    case "71":
                        str = "Primary Payer amount.";
                        break;

                    case "72":
                        str = "Coinsurance day. (Handled in QTY, QTY01=CD)";
                        break;

                    case "73":
                        str = "Administrative days.";
                        break;

                    case "74":
                        str = "Indirect Medical Education Adjustment.";
                        break;

                    case "75":
                        str = "Direct Medical Education Adjustment.";
                        break;

                    case "76":
                        str = "Disproportionate Share Adjustment.";
                        break;

                    case "77":
                        str = "Covered days. (Handled in QTY, QTY01=CA)";
                        break;

                    case "78":
                        str = "Non-Covered days/Room charge adjustment.";
                        break;

                    case "79":
                        str = "Cost Report days. (Handled in MIA15)";
                        break;

                    case "80":
                        str = "Outlier days. (Handled in QTY, QTY01=OU)";
                        break;

                    case "81":
                        str = "Discharges.";
                        break;

                    case "82":
                        str = "PIP days.";
                        break;

                    case "83":
                        str = "Total visits.";
                        break;

                    case "84":
                        str = "Capital Adjustment. (Handled in MIA)";
                        break;

                    case "85":
                        str = "Interest amount.";
                        break;

                    case "86":
                        str = "Statutory Adjustment.";
                        break;

                    case "87":
                        str = "Transfer amount.";
                        break;

                    case "88":
                        str = "Adjustment amount represents collection against receivable created in prior overpayment.";
                        break;

                    case "89":
                        str = "Professional fees removed from charges.";
                        break;

                    case "90":
                        str = "Ingredient cost adjustment.";
                        break;

                    case "91":
                        str = "Dispensing fee adjustment.";
                        break;

                    case "92":
                        str = "Claim Paid in full.";
                        break;

                    case "93":
                        str = "No Claim level Adjustments.";
                        break;

                    case "94":
                        str = "Processed in Excess of charges.";
                        break;

                    case "95":
                        str = "Benefits adjusted. Plan procedures not followed.";
                        break;

                    case "96":
                        str = "Non-covered charge(s).";
                        break;

                    case "97":
                        str = "Payment is included in the allowance for another service/procedure.";
                        break;

                    case "98":
                        str = "The hospital must file the Medicare claim for this inpatient non-physician service.";
                        break;

                    case "99":
                        str = "Medicare Secondary Payer Adjustment Amount.";
                        break;

                    case "100":
                        str = "Payment made to patient/insured/responsible party.";
                        break;

                    case "101":
                        str = "Predetermination: anticipated payment upon completion of services or claim adjudication.";
                        break;

                    case "102":
                        str = "Major Medical Adjustment.";
                        break;

                    case "103":
                        str = "Provider promotional discount (e.g., Senior citizen discount).";
                        break;

                    case "104":
                        str = "Managed care withholding.";
                        break;

                    case "105":
                        str = "Tax withholding.";
                        break;

                    case "106":
                        str = "Patient payment option/election not in effect.";
                        break;

                    case "107":
                        str = "Claim/service denied because the related or qualifying claim/service was not previously paid or identified on this claim.";
                        break;

                    case "108":
                        str = "Payment adjusted because rent/purchase guidelines were not met.";
                        break;

                    case "109":
                        str = "Claim not covered by this payer/contractor. You must send the claim to the correct payer/contractor.";
                        break;

                    case "110":
                        str = "Billing date predates service date.";
                        break;

                    case "111":
                        str = "Not covered unless the provider accepts assignment.";
                        break;

                    case "112":
                        str = "Payment adjusted as not furnished directly to the patient and/or not documented.";
                        break;

                    case "113":
                        str = "Payment denied because service/procedure was provided outside the United States or as a result of war.";
                        break;

                    case "114":
                        str = "Procedure/product not approved by the Food and Drug Administration.";
                        break;

                    case "115":
                        str = "Payment adjusted as procedure postponed or canceled.";
                        break;

                    case "116":
                        str = "Payment denied. The advance indemnification notice signed by the patient did not comply with requirements.";
                        break;

                    case "117":
                        str = "Payment adjusted because transportation is only covered to the closest facility that can provide the necessary care.";
                        break;

                    case "118":
                        str = "Charges reduced for ESRD network support.";
                        break;

                    case "119":
                        str = "Benefit maximum for this time period or occurrence has been reached.";
                        break;

                    case "120":
                        str = "Patient is covered by a managed care plan.";
                        break;

                    case "121":
                        str = "Indemnification adjustment.";
                        break;

                    case "122":
                        str = "Psychiatric reduction.";
                        break;

                    case "123":
                        str = "Payer refund due to overpayment.";
                        break;

                    case "124":
                        str = "Payer refund amount - not our patient.";
                        break;

                    case "125":
                        str = "Payment adjusted due to a submission/billing error(s). Additional information is supplied using the remittance advice remarks codes whenever appropriate.";
                        break;

                    case "126":
                        str = "Deductible -- Major Medical";
                        break;

                    case "127":
                        str = "Coinsurance -- Major Medical";
                        break;

                    case "128":
                        str = "Newborn's services are covered in the mother's Allowance.";
                        break;

                    case "129":
                        str = "Payment denied - Prior processing information appears incorrect.";
                        break;

                    case "130":
                        str = "Claim submission fee.";
                        break;

                    case "131":
                        str = "Claim specific negotiated discount.";
                        break;

                    case "132":
                        str = "Prearranged demonstration project adjustment.";
                        break;

                    case "133":
                        str = "The disposition of this claim/service is pending further review.";
                        break;

                    case "134":
                        str = "Technical fees removed from charges.";
                        break;

                    case "135":
                        str = "Claim denied. Interim bills cannot be processed.";
                        break;

                    case "136":
                        str = "Claim Adjusted. Plan procedures of a prior payer were not followed.";
                        break;

                    case "137":
                        str = "Payment/Reduction for Regulatory Surcharges, Assessments, Allowances or Health Related Taxes.";
                        break;

                    case "138":
                        str = "Claim/service denied. Appeal procedures not followed or time limits not met.";
                        break;

                    case "139":
                        str = "Contracted funding agreement - Subscriber is employed by the provider of services.";
                        break;

                    case "140":
                        str = "Patient/Insured health identification number and name do not match.";
                        break;

                    case "141":
                        str = "Claim adjustment because the claim spans eligible and ineligible periods of coverage.";
                        break;

                    case "142":
                        str = "Claim adjusted by the monthly Medicaid patient liability amount.";
                        break;

                    case "143":
                        str = "Portion of payment deferred.";
                        break;

                    case "144":
                        str = "Incentive adjustment, e.g. preferred product/service.";
                        break;

                    case "145":
                        str = "Premium payment withholding";
                        break;

                    case "146":
                        str = "Payment denied because the diagnosis was invalid for the date(s) of service reported.";
                        break;

                    case "147":
                        str = "Provider contracted/negotiated rate expired or not on file.";
                        break;

                    case "148":
                        str = "Claim/service rejected at this time because information from another provider was not provided or was insufficient/incomplete.";
                        break;

                    case "149":
                        str = "Lifetime benefit maximum has been reached for this service/benefit category.";
                        break;

                    case "150":
                        str = "Payment adjusted because the payer deems the information submitted does not support this level of service.";
                        break;

                    case "151":
                        str = "Payment adjusted because the payer deems the information submitted does not support this many services.";
                        break;

                    case "152":
                        str = "Payment adjusted because the payer deems the information submitted does not support this length of service.";
                        break;

                    case "153":
                        str = "Payment adjusted because the payer deems the information submitted does not support this dosage.";
                        break;

                    case "154":
                        str = "Payment adjusted because the payer deems the information submitted does not support this day's supply.";
                        break;

                    case "155":
                        str = "This claim is denied because the patient refused the service/procedure.";
                        break;

                    case "156":
                        str = "Flexible spending account payments";
                        break;

                    case "157":
                        str = "Payment denied/reduced because service/procedure was provided as a result of an act of war.";
                        break;

                    case "158":
                        str = "Payment denied/reduced because the service/procedure was provided outside of the United States.";
                        break;

                    case "159":
                        str = "Payment denied/reduced because the service/procedure was provided as a result of terrorism.";
                        break;

                    case "160":
                        str = "Payment denied/reduced because injury/illness was the result of an activity that is a benefit exclusion.";
                        break;

                    case "161":
                        str = "Provider performance bonus";
                        break;

                    case "162":
                        str = "State-mandated Requirement for Property and Casualty, see Claim Payment Remarks Code for specific explanation.";
                        break;

                    case "163":
                        str = "Claim/Service adjusted because the attachment referenced on the claim was not received.";
                        break;

                    case "164":
                        str = "Claim/Service adjusted because the attachment referenced on the claim was not received in a timely fashion.";
                        break;

                    case "165":
                        str = "Payment denied /reduced for absence of, or exceeded referral";
                        break;

                    case "166":
                        str = "These services were submitted after this payers responsibility for processing claims under this plan ended.";
                        break;

                    case "167":
                        str = "This (these) diagnosis(es) is (are) not covered. ";
                        break;

                    case "168":
                        str = "Payment denied as Service(s) have been considered under the patient's medical plan. Benefits are not available under this dental plan";
                        break;

                    case "169":
                        str = "Payment adjusted because an alternate benefit has been provided";
                        break;

                    case "170":
                        str = "Payment is denied when performed/billed by this type of provider.";
                        break;

                    case "171":
                        str = "Payment is denied when performed/billed by this type of provider in this type of facility.";
                        break;

                    case "172":
                        str = "Payment is adjusted when performed/billed by a provider of this specialty ";
                        break;

                    case "173":
                        str = "Payment adjusted because this service was not prescribed by a physician";
                        break;

                    case "174":
                        str = "Payment denied because this service was not prescribed prior to delivery";
                        break;

                    case "175":
                        str = "Payment denied because the prescription is incomplete";
                        break;

                    case "176":
                        str = "Payment denied because the prescription is not current";
                        break;

                    case "177":
                        str = "Payment denied because the patient has not met the required eligibility requirements";
                        break;

                    case "178":
                        str = "Payment adjusted because the patient has not met the required spend down requirements.";
                        break;

                    case "179":
                        str = "Payment adjusted because the patient has not met the required waiting requirements";
                        break;

                    case "180":
                        str = "Payment adjusted because the patient has not met the required residency requirements";
                        break;

                    case "181":
                        str = "Payment adjusted because this procedure code was invalid on the date of service";
                        break;

                    case "182":
                        str = "Payment adjusted because the procedure modifier was invalid on the date of service";
                        break;

                    case "183":
                        str = "The referring provider is not eligible to refer the service billed.";
                        break;

                    case "184":
                        str = "The prescribing/ordering provider is not eligible to prescribe/order the service billed. ";
                        break;

                    case "185":
                        str = "The rendering provider is not eligible to perform the service billed.";
                        break;

                    case "186":
                        str = "Payment adjusted since the level of care changed ";
                        break;

                    case "187":
                        str = "Health Savings account payments ";
                        break;

                    case "188":
                        str = "This product/procedure is only covered when used according to FDA recommendations.";
                        break;

                    case "189":
                        str = "Not otherwise classified or unlisted procedure code (CPT/HCPCS) was billed when there is a specific procedure code for this procedure/service";
                        break;

                    case "190":
                        str = "Payment is included in the allowance for a Skilled Nursing Facility (SNF) qualified stay.";
                        break;

                    case "191":
                        str = "Claim denied because this is not a work related injury/illness and thus not the liability of the workers’ compensation carrier.";
                        break;

                    case "192":
                        str = "Non standard adjustment code from paper remittance advice.";
                        break;

                    case "193":
                        str = "Original payment decision is being maintained. This claim was processed properly the first time.";
                        break;

                    case "194":
                        str = "Payment adjusted when anesthesia is performed by the operating physician, the assistant surgeon or the attending physician";
                        break;

                    case "195":
                        str = "Payment denied/reduced due to a refund issued to an erroneous priority payer for this claim/service";
                        break;

                    case "A0":
                        str = "Patient refund amount.";
                        break;

                    case "A1":
                        str = "Claim denied charges.";
                        break;

                    case "A2":
                        str = "Contractual adjustment.";
                        break;

                    case "A3":
                        str = "Medicare Secondary Payer liability met.";
                        break;

                    case "A4":
                        str = "Medicare Claim PPS Capital Day Outlier Amount.";
                        break;

                    case "A5":
                        str = "Medicare Claim PPS Capital Cost Outlier Amount.";
                        break;

                    case "A6":
                        str = "Prior hospitalization or 30 day transfer requirement not met.";
                        break;

                    case "A7":
                        str = "Presumptive Payment Adjustment";
                        break;

                    case "A8":
                        str = "Claim denied; ungroupable DRG";
                        break;

                    case "B1":
                        str = "Non-covered visits.";
                        break;

                    case "B2":
                        str = "Covered visits.";
                        break;

                    case "B3":
                        str = "Covered charges.";
                        break;

                    case "B4":
                        str = "Late filing penalty.";
                        break;

                    case "B5":
                        str = "Payment adjusted because coverage/program guidelines were not met or were exceeded.";
                        break;

                    case "B6":
                        str = "This payment is adjusted when performed/billed by this type of provider, by this type of provider in this type of facility, or by a provider of this specialty.";
                        break;

                    case "B7":
                        str = "This provider was not certified/eligible to be paid for this procedure/service on this date of service.";
                        break;

                    case "B8":
                        str = "Claim/service not covered/reduced because alternative services were available, and should have been utilized.";
                        break;

                    case "B9":
                        str = "Services not covered because the patient is enrolled in a Hospice.";
                        break;

                    case "B10":
                        str = "Allowed amount has been reduced because a component of the basic procedure/test was paid. The beneficiary is not liable for more than the charge limit for the basic procedure/test.";
                        break;

                    case "B11":
                        str = "The claim/service has been transferred to the proper payer/processor for processing. Claim/service not covered by this payer/processor.";
                        break;

                    case "B12":
                        str = "Services not documented in patients' medical records.";
                        break;

                    case "B13":
                        str = "Previously paid. Payment for this claim/service may have been provided in a previous payment.";
                        break;

                    case "B14":
                        str = "Payment denied because only one visit or consultation per physician per day is covered.";
                        break;

                    case "B15":
                        str = "Payment adjusted because this procedure/service is not paid separately.";
                        break;

                    case "B16":
                        str = "Payment adjusted because `New Patient' qualifications were not met.";
                        break;

                    case "B17":
                        str = "Payment adjusted because this service was not prescribed by a physician, not prescribed prior to delivery, the prescription is incomplete, or the prescription is not current.";
                        break;

                    case "B18":
                        str = "Payment adjusted because this procedure code and modifier were invalid on the date of service";
                        break;

                    case "B19":
                        str = "Claim/service adjusted because of the finding of a Review Organization.";
                        break;

                    case "B20":
                        str = "Payment adjusted because procedure/service was partially or fully furnished by another provider.";
                        break;

                    case "B21":
                        str = "The charges were reduced because the service/care was partially furnished by another physician.";
                        break;

                    case "B22":
                        str = "This payment is adjusted based on the diagnosis.";
                        break;

                    case "B23":
                        str = "Payment denied because this provider has failed an aspect of a proficiency testing program.";
                        break;

                    case "D1":
                        str = "Claim/service denied. Level of subluxation is missing or inadequate.";
                        break;

                    case "D2":
                        str = "Claim lacks the name, strength, or dosage of the drug furnished.";
                        break;

                    case "D3":
                        str = "Claim/service denied because information to indicate if the patient owns the equipment that requires the part or supply was missing.";
                        break;

                    case "D4":
                        str = "Claim/service does not indicate the period of time for which this will be needed.";
                        break;

                    case "D5":
                        str = "Claim/service denied. Claim lacks individual lab codes included in the test.";
                        break;

                    case "D6":
                        str = "Claim/service denied. Claim did not include patient's medical record for the service.";
                        break;

                    case "D7":
                        str = "Claim/service denied. Claim lacks date of patient's most recent physician visit.";
                        break;

                    case "D8":
                        str = "Claim/service denied. Claim lacks indicator that `x-ray is available for review.";
                        break;

                    case "D9":
                        str = "Claim/service denied. Claim lacks invoice or statement certifying the actual cost of the lens, less discounts or the type of intraocular lens used.";
                        break;

                    case "D10":
                        str = "Claim/service denied. Completed physician financial relationship form not on file.";
                        break;

                    case "D11":
                        str = "Claim lacks completed pacemaker registration form.";
                        break;

                    case "D12":
                        str = "Claim/service denied. Claim does not identify who performed the purchased diagnostic test or the amount you were charged for the test.";
                        break;

                    case "D13":
                        str = "Claim/service denied. Performed by a facility/supplier in which the ordering/referring physician has a financial interest.";
                        break;

                    case "D14":
                        str = "Claim lacks indication that plan of treatment is on file.";
                        break;

                    case "D15":
                        str = "Claim lacks indication that service was supervised or evaluated by a physician.";
                        break;

                    case "D16":
                        str = "Claim lacks prior payer payment information.";
                        break;

                    case "D17":
                        str = "Claim/Service has invalid non-covered days.";
                        break;

                    case "D18":
                        str = "Claim/Service has missing diagnosis information.";
                        break;

                    case "D19":
                        str = "Claim/Service lacks Physician/Operative or other supporting documentation";
                        break;

                    case "D20":
                        str = "Claim/Service missing service/product information.";
                        break;

                    case "D21":
                        str = "This (these) diagnosis(es) is (are) missing or are invalid";
                        break;

                    case "W1":
                        str = "Workers Compensation State Fee Schedule Adjustment";
                        break;

                    case "242":
                        str = "Services not provided by network/primary care providers.";
                        break;

                    case "204":
                        str = "This service/equipment/drug is not covered under the patient’s current benefit plan.";
                        break;

                    case "197":
                        str = "Precertification/authorization/notification absent.";
                        break;

                    case "249":
                        str = "This claim has been identified as a readmission. (Use only with Group Code CO).";
                        break;

                    case "252":
                        str = "An attachment/other documentation is required to adjudicate this claim/service. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT).";
                        break;

                    case "209":
                        str = "Per regulatory or other agreement. The provider cannot collect this amount from the patient. However, this amount may be billed to subsequent payer. Refund to patient if collected. (Use only with Group code OA).";
                        break;

                    case "198":
                        str = "Precertification/authorization exceeded.";
                        break;

                    case "199":
                        str = "Revenue code and Procedure code do not match.";
                        break;

                    case "200":
                        str = "Expenses incurred during lapse in coverage.";
                        break;

                    case "201":
                        str = "Patient is responsible for amount of this claim/service through 'set aside arrangement' or other agreement. (Use only with Group Code PR) At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;

                    case "202":
                        str = "Non-covered personal comfort or convenience services.";
                        break;

                    case "203":
                        str = "Discontinued or reduced service.";
                        break;

                    case "205":
                        str = "Pharmacy discount card processing fee.";
                        break;

                    case "206":
                        str = "National Provider Identifier - missing.";
                        break;

                    case "207":
                        str = "National Provider identifier - Invalid format.";
                        break;

                    case "208":
                        str = "National Provider Identifier - Not matched.";
                        break;

                    case "210":
                        str = "Payment adjusted because pre-certification/authorization not received in a timely fashion.";
                        break;

                    case "211":
                        str = "National Drug Codes (NDC) not eligible for rebate, are not covered.";
                        break;

                    case "212":
                        str = "Administrative surcharges are not covered.";
                        break;

                    case "213":
                        str = "Non-compliance with the physician self referral prohibition legislation or payer policy.";
                        break;

                    case "215":
                        str = "Based on subrogation of a third party settlement.";
                        break;

                    case "216":
                        str = "Based on the findings of a review organization.";
                        break;

                    case "219":
                        str = "Based on extent of injury.";
                        break;

                    case "222":
                        str = "Exceeds the contracted maximum number of hours/days/units by this provider for this period. This is not patient specific.";
                        break;

                    case "223":
                        str = "Adjustment code for mandated federal, state or local law/regulation that is not already covered by another code and is mandated before a new code can be created.";
                        break;

                    case "224":
                        str = "Patient identification compromised by identity theft. Identity verification required for processing this and future claims.";
                        break;

                    case "225":
                        str = "Penalty or Interest Payment by Payer (Only used for plan to plan encounter reporting within the 837)";
                        break;

                    case "226":
                        str = "Information requested from the Billing/Rendering Provider was not provided or not provided timely or was insufficient/incomplete. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;

                    case "227":
                        str = "Information requested from the patient/insured/responsible party was not provided or was insufficient/incomplete. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;

                    case "228":
                        str = "Denied for failure of this provider, another provider or the subscriber to supply requested information to a previous payer for their adjudication.";
                        break;

                    case "229":
                        str = "Partial charge amount not considered by Medicare due to the initial claim Type of Bill being 12X.";
                        break;

                    case "231":
                        str = "Mutually exclusive procedures cannot be done in the same day/setting.";
                        break;

                    case "234":
                        str = "This procedure is not paid separately. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;

                    case "235":
                        str = "Sales Tax.";
                        break;

                    case "236":
                        str = "This procedure or procedure/modifier combination is not compatible with another procedure or procedure/modifier combination provided on the same day according to the National Correct Coding Initiative or workers compensation state regulations/ fee schedule requirements.";
                        break;

                    case "238":
                        str = "Claim spans eligible and ineligible periods of coverage, this is the reduction for the ineligible period. (Use only with Group Code PR)";
                        break;

                    case "239":
                        str = "Claim spans eligible and ineligible periods of coverage. Rebill separate claims.";
                        break;

                    case "240":
                        str = "The diagnosis is inconsistent with the patient's birth weight.";
                        break;

                    case "241":
                        str = "Low Income Subsidy (LIS) Co-payment Amount.";
                        break;

                    case "243":
                        str = "Services not authorized by network/primary care providers.";
                        break;

                    case "245":
                        str = "Provider performance program withhold.";
                        break;

                    case "246":
                        str = "This non-payable code is for required reporting only.";
                        break;

                    case "247":
                        str = "Deductible for Professional service rendered in an Institutional setting and billed on an Institutional claim.";
                        break;

                    case "248":
                        str = "Coinsurance for Professional service rendered in an Institutional setting and billed on an Institutional claim.";
                        break;

                    case "250":
                        str = "The attachment/other documentation that was received was the incorrect attachment/document. The expected attachment/document is still missing. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT).";
                        break;

                    case "251":
                        str = "The attachment/other documentation that was received was incomplete or deficient. The necessary information is still needed to process the claim. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT).";
                        break;

                    case "254":
                        str = "Claim received by the dental plan, but benefits not available under this plan. Submit these services to the patient's medical plan for further consideration.";
                        break;

                    case "256":
                        str = "Service not payable per managed care contract.";
                        break;

                    case "257":
                        str = "The disposition of the claim/service is undetermined during the premium payment grace period, per Health Insurance Exchange requirements. This claim/service will be reversed and corrected when the grace period ends (due to premium payment or lack of premium payment). (Use only with Group Code OA)";
                        break;

                    case "258":
                        str = "Claim/service not covered when patient is in custody/incarcerated. Applicable federal, state or local authority may cover the claim/service.";
                        break;

                    case "259":
                        str = "Additional payment for Dental/Vision service utilization.";
                        break;

                    case "260":
                        str = "Processed under Medicaid ACA Enhanced Fee Schedule.";
                        break;

                    case "261":
                        str = "The procedure or service is inconsistent with the patient's history.";
                        break;

                    case "262":
                        str = "Adjustment for delivery cost. Usage: To be used for pharmaceuticals only.";
                        break;

                    case "263":
                        str = "Adjustment for shipping cost. Usage: To be used for pharmaceuticals only.";
                        break;

                    case "264":
                        str = "Adjustment for postage cost. Usage: To be used for pharmaceuticals only.";
                        break;

                    case "265":
                        str = "Adjustment for administrative cost. Usage: To be used for pharmaceuticals only.";
                        break;

                    case "266":
                        str = "Adjustment for compound preparation cost. Usage: To be used for pharmaceuticals only.";
                        break;

                    case "267":
                        str = "Claim/service spans multiple months. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)";
                        break;

                    case "268":
                        str = "The Claim spans two calendar years. Please resubmit one claim per calendar year.";
                        break;

                    case "269":
                        str = "Anesthesia not covered for this service/procedure.";
                        break;

                    case "270":
                        str = "Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient’s dental plan for further consideration.";
                        break;

                    case "271":
                        str = "Prior contractual reductions related to a current periodic payment as part of a contractual payment schedule when deferred amounts have been previously reported. (Use only with group code OA)";
                        break;

                    case "272":
                        str = "Coverage/program guidelines were not met.";
                        break;

                    case "273":
                        str = "Coverage/program guidelines were exceeded.";
                        break;

                    case "274":
                        str = "Fee/Service not payable per patient Care Coordination arrangement.";
                        break;

                    case "275":
                        str = "Prior payer's (or payers') patient responsibility (deductible, coinsurance, co-payment) not covered. (Use only with Group Code PR)";
                        break;

                    case "276":
                        str = "Services denied by the prior payer(s) are not covered by this payer.";
                        break;

                    case "277":
                        str = "The disposition of the claim/service is undetermined during the premium payment grace period, per Health Insurance SHOP Exchange requirements. This claim/service will be reversed and corrected when the grace period ends (due to premium payment or lack of premium payment). (Use only with Group Code OA)";
                        break;

                    case "278":
                        str = "Performance program proficiency requirements not met. (Use only with Group Codes CO or PI).";
                        break;

                    case "279":
                        str = "Services not provided by Preferred network providers. Usage: Use this code when there are member network limitations. For example, using contracted providers not in the member's 'narrow' network.";
                        break;

                    case "280":
                        str = "Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient's Pharmacy plan for further consideration.";
                        break;

                    case "281":
                        str = "Deductible waived per contractual agreement. Use only with Group Code CO.";
                        break;

                    case "282":
                        str = "The procedure/revenue code is inconsistent with the type of bill.";
                        break;

                    case "283":
                        str = "Attending provider is not eligible to provide direction of care.";
                        break;

                    case "284":
                        str = "Precertification/authorization/notification/pre-treatment number may be valid but does not apply to the billed services.";
                        break;

                    case "285":
                        str = "Appeal procedures not followed.";
                        break;

                    case "286":
                        str = "Appeal time limits not met.";
                        break;

                    case "287":
                        str = "Referral exceeded.";
                        break;

                    case "288":
                        str = "Referral absent.";
                        break;

                    case "289":
                        str = "Services considered under the dental and medical plans, benefits not available.";
                        break;

                    case "290":
                        str = "Claim received by the dental plan, but benefits not available under this plan. Claim has been forwarded to the patient's medical plan for further consideration.";
                        break;

                    case "291":
                        str = "Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's dental plan for further consideration.";
                        break;

                    case "292":
                        str = "Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's pharmacy plan for further consideration.";
                        break;

                    case "293":
                        str = "Payment made to employer.";
                        break;

                    case "294":
                        str = "Payment made to attorney.";
                        break;

                    case "P1":
                        str = "State-mandated Requirement for Property and Casualty, see Claim Payment Remarks Code for specific explanation. To be used for Property and Casualty only.";
                        break;

                    case "P2":
                        str = "Not a work related injury/illness and thus not the liability of the workers' compensation carrier.";
                        break;

                    case "P3":
                        str = "Workers' Compensation case settled. Patient is responsible for amount of this claim/service through WC 'Medicare set aside arrangement' or other agreement. To be used for Workers' Compensation only. (Use only with Group Code PR)";
                        break;

                    case "P4":
                        str = "Workers' Compensation claim adjudicated as non-compensable. This Payer not liable for claim or service/treatment.";
                        break;

                    case "P5":
                        str = "Based on payer reasonable and customary fees. No maximum allowable defined by legislated fee arrangement. To be used for Property and Casualty only.";
                        break;

                    case "P6":
                        str = "Based on entitlement to benefits.";
                        break;

                    case "P7":
                        str = "The applicable fee schedule/fee database does not contain the billed code. Please resubmit a bill with the appropriate fee schedule/fee database code(s) that best describe the service(s) provided and supporting documentation if required. To be used for Property and Casualty only.";
                        break;

                    case "P8":
                        str = "Claim is under investigation.";
                        break;

                    case "P9":
                        str = "No available or correlating CPT/HCPCS code to describe this service. To be used for Property and Casualty only.";
                        break;

                    case "P10":
                        str = "Payment reduced to zero due to litigation. Additional information will be sent following the conclusion of litigation. To be used for Property and Casualty only.";
                        break;

                    case "P11":
                        str = "The disposition of the related Property & Casualty claim (injury or illness) is pending due to litigation. To be used for Property and Casualty only. (Use only with Group Code OA)";
                        break;

                    case "P12":
                        str = "Workers' compensation jurisdictional fee schedule adjustment.";
                        break;

                    case "P13":
                        str = "Payment reduced or denied based on workers' compensation jurisdictional regulations or payment policies, use only if no other code is applicable.";
                        break;

                    case "P14":
                        str = "The Benefit for this Service is included in the payment/allowance for another service/procedure that has been performed on the same day.";
                        break;

                    case "P15":
                        str = "Workers' Compensation Medical Treatment Guideline Adjustment. To be used for Workers' Compensation only.";
                        break;

                    case "P16":
                        str = "Medical provider not authorized/certified to provide treatment to injured workers in this jurisdiction. To be used for Workers' Compensation only. (Use with Group Code CO or OA)";
                        break;

                    case "P17":
                        str = "Referral not authorized by attending physician per regulatory requirement. To be used for Property and Casualty only.";
                        break;

                    case "P18":
                        str = "Procedure is not listed in the jurisdiction fee schedule. An allowance has been made for a comparable service. To be used for Property and Casualty only.";
                        break;

                    case "P19":
                        str = "Procedure has a relative value of zero in the jurisdiction fee schedule, therefore no payment is due. To be used for Property and Casualty only.";
                        break;

                    case "P20":
                        str = "Service not paid under jurisdiction allowed outpatient facility fee schedule. To be used for Property and Casualty only.";
                        break;

                    case "P21":
                        str = "Payment denied based on Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional regulations or payment policies, use only if no other code is applicable.";
                        break;

                    case "P22":
                        str = "Payment adjusted based on Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional regulations or payment policies, use only if no other code is applicable.";
                        break;

                    case "P23":
                        str = "Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional fee schedule adjustment.";
                        break;

                    case "P24":
                        str = "Payment adjusted based on Preferred Provider Organization (PPO).";
                        break;

                    case "P25":
                        str = "Payment adjusted based on Medical Provider Network (MPN).";
                        break;

                    case "P26":
                        str = "Payment adjusted based on Voluntary Provider network (VPN).";
                        break;

                    case "P27":
                        str = "Payment denied based on the Liability Coverage Benefits jurisdictional regulations and/or payment policies.";
                        break;

                    case "P28":
                        str = "Payment adjusted based on the Liability Coverage Benefits jurisdictional regulations and/or payment policies.";
                        break;

                    case "P29":
                        str = "Liability Benefits jurisdictional fee schedule adjustment.";
                        break;

                    default:
                        str = "### Unknown ClaimAdjustmentCode ###";
                        break;
                }
                return str;
            }
            catch (Exception)
            {
                return str;
            }
        }

        private string CLP(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "CLP")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                this.rowCache = this.GetNewCacheRow();
                                builder.Append("Patient Control Number: ");
                                builder.Append(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_ACNT = strArray[index];
                                this.rowCache.FIELD_REMIT_PATIENT_ACCOUNT_NO = strArray[index];
                                break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Status: ");
                                builder.Append(this.CLP02(strArray[index]));
                                this.rowClaim.FIELD_CLAIM_STATUS = this.CLP02(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_STATUS_CODE = strArray[index];
                                this.rowCache.FIELD_REMIT_CLAIM_STATUS = this.CLP02(strArray[index]);
                                this.rowCache.FIELD_REMIT_CLAIM_STATUS_CODE = strArray[index];
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Claim Charge Amount: ");
                                builder.Append(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_BILLED_AMOUNT = Convert.ToDecimal(strArray[index]);
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Payment Amount: ");
                                builder.Append(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_PAID_AMOUNT = Convert.ToDecimal(strArray[index]);
                                break;

                            case 5:
                                builder.Append(Environment.NewLine);
                                builder.Append("Patient Responsibility Amount: ");
                                builder.Append(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_PR_AMOUNT = strArray[index];
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Filing Indicator Code: ");
                                builder.Append(this.CLP06(strArray[index]));
                                break;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Payer Claim Control Number: ");
                                builder.Append(strArray[index]);
                                this.rowClaim.FIELD_CLAIM_ICN = strArray[index];
                                this.rowCache.FIELD_REMIT_ICN = strArray[index];
                                break;

                            case 8:
                                builder.Append(Environment.NewLine);
                                builder.Append("Facility Type: ");
                                this.rowClaim.FIELD_CLAIM_POS = strArray[index];
                                this.rowCache.FIELD_REMIT_CLAIM_POS = strArray[index];
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Frequency Code: ");
                                builder.Append(strArray[index]);
                                break;

                            case 11:
                                builder.Append(Environment.NewLine);
                                builder.Append("Diagnosis Related Group (DRG) Code: ");
                                builder.Append(strArray[index]);
                                break;

                            case 12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Diagnosis Related Group (DRG) Weight: ");
                                builder.Append(strArray[index]);
                                break;

                            case 13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Discharge Fraction: ");
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public string CLP02(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Processed as Primary";

                case "2":
                    return "Processed as Secondary";

                case "3":
                    return "Processed as Tertiary";

                case "4":
                    return "Denied";

                case "5":
                    return "Pended";

                case "10":
                    return "Received, but not in process";

                case "13":
                    return "Suspended";

                case "15":
                    return "Suspended - investigation with field";

                case "16":
                    return "Suspended - return with material";

                case "17":
                    return "Suspended - review pending";

                case "19":
                    return "Processed as Primary, Forwarded to Additional Payer(s)";

                case "20":
                    return "Processed as Secondary, Forwarded to Additional Payer(s)";

                case "21":
                    return "Processed as Tertiary, Forwarded to Additional  Payer(s)";

                case "22":
                    return "Reversal of Previous Payment";

                case "23":
                    return "Not Our Claim, Forwarded to Additional Payer(s)";

                case "25":
                    return "Predetermination Pricing Only - No Payment";

                case "27":
                    return "Reviewed";
            }
            return "### Unknown CLP02 ###";
        }

        public string CLP02Status(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "ACCEPTED";

                case "2":
                    return "ACCEPTED";

                case "3":
                    return "ACCEPTED";

                case "4":
                    return "REJECTED";

                case "5":
                    return "PENDING";

                case "10":
                    return "ACCEPTED";

                case "13":
                    return "PENDING";

                case "15":
                    return "PENDING";

                case "16":
                    return "PENDING";

                case "17":
                    return "PENDING";

                case "19":
                    return "ACCEPTED";

                case "20":
                    return "ACCEPTED";

                case "21":
                    return "ACCEPTED";

                case "22":
                    return "ACCEPTED";

                case "23":
                    return "ACCEPTED";

                case "25":
                    return "ACCEPTED";

                case "27":
                    return "REVIEWED";
            }
            return "### Unknown CLP02 ###";
        }

        private string CLP06(string Code)
        {
            switch (Code)
            {
                case "12":
                    return "Preferred Provider Organization (PPO)";

                case "13":
                    return "Point of Service (POS)";

                case "14":
                    return "Exclusive Provider Organization (EPO)";

                case "15":
                    return "Indemnity Insurance";

                case "16":
                    return "Health Maintenance Organization (HMO) Medicare Risk";

                case "AM":
                    return "Automobile Medical";

                case "CH":
                    return "Champus";

                case "DS":
                    return "Disability";

                case "HM":
                    return "Health Maintenance Organization";

                case "LM":
                    return "Liability Medical";

                case "MA":
                    return "Medicare Part A";

                case "MB":
                    return "Medicare Part B";

                case "MC":
                    return "Medicaid";

                case "OF":
                    return "Other Federal Program";

                case "TV":
                    return "Title V";

                case "VA":
                    return "Veterans Affairs Plan";

                case "WC":
                    return "Workers Compensation Health Claim";
            }
            return "### Unknown CLP06 ###";
        }

        private string CUR(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "CUR")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.CUR01(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 2:
                                builder.Append('\t');
                                builder.Append(strArray[num2]);
                                break;

                            case 3:
                                builder.Append('\t');
                                builder.Append(strArray[num2]);
                                break;
                        }
                    }
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string CUR01(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "PR"))
            {
                return "Payer";
            }
            return "### Unknown CUR01 ###";
        }

        private string DTM(string StrIn)
        {
            string str;
            try
            {
                DateTime time;
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "DTM")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append(this.DTM01(strArray[index]));
                                builder.Append(": ");
                                break;

                            case 2:
                                time = MDVUtility.StringToDate(strArray[index]);
                                builder.Append(time.ToShortDateString());
                                break;
                        }
                    }
                }
                if (strArray.Length > 1)
                {
                    if (strArray[1] == "405")
                    {
                        this.rowERA.FIELD_ERA835_PRODUCTION_QUALIFIER = this.DTM01(strArray[1]);
                        time = MDVUtility.StringToDate(strArray[2]);
                        this.rowERA.FIELD_ERA835_PRODUCTION_DATE = time.ToShortDateString();
                    }
                    else if (strArray[1] == "050")
                    {
                        time = MDVUtility.StringToDate(strArray[2]);
                        this.rowClaim.FIELD_CLAIM_DATE = time.ToShortDateString();
                    }
                    else if (strArray[1] == "232")
                    {
                        time = MDVUtility.StringToDate(strArray[2]);
                        this.rowClaim.FIELD_CLAIM_FROM_DATE = time.ToShortDateString();
                        this.rowCache.FIELD_REMIT_CLAIM_FROM_DATE = time.ToShortDateString();
                    }
                    else if (strArray[1] == "233")
                    {
                        this.rowClaim.FIELD_CLAIM_TO_DATE = MDVUtility.StringToDate(strArray[2]).ToShortDateString();
                    }
                    else if (strArray[1] == "472")
                    {
                        this.rowCache.FIELD_REMIT_SERVICE_FROM_DATE = strArray[2];
                        this.rowCache.FIELD_REMIT_SERVICE_TO_DATE = strArray[2];
                    }
                    else if (strArray[1] == "150")
                    {
                        this.rowCache.FIELD_REMIT_SERVICE_FROM_DATE = strArray[2];
                    }
                    else if (strArray[1] == "151")
                    {
                        this.rowCache.FIELD_REMIT_SERVICE_TO_DATE = strArray[2];
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string DTM01(string Code)
        {
            switch (Code)
            {
                case "405":
                    return "Production";

                case "036":
                    return "Expiration";

                case "050":
                    return "Received";

                case "232":
                    return "Claim Statement Period Start";

                case "233":
                    return "Claim Statement Period End";

                case "150":
                    return "Service Period Start";

                case "151":
                    return "Service Period End";

                case "472":
                    return "Service";
            }
            return "### Unknown DTM01 ###";
        }

        private string EIN(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                int index = 0;
                for (index = 0; index <= (strArray.Length - 2); index++)
                {
                    string[] strArray2;
                    string str2 = strArray[index].Substring(0, strArray[index].IndexOf(MDVUtility.D_ELMT));
                    if (str2 != null)
                    {
                        if (!(str2 == "N1"))
                        {
                            if (str2 == "REF")
                            {
                                goto Label_00B3;
                            }
                        }
                        else
                        {
                            strArray2 = strArray[index].Split(new char[] { MDVUtility.D_ELMT });
                            if ((strArray2.Length > 3) && (strArray2[3] == "FI"))
                            {
                                return strArray2[4];
                            }
                        }
                    }
                    continue;
                Label_00B3: ;
                    strArray2 = strArray[index].Split(new char[] { MDVUtility.D_ELMT });
                    if ((strArray2.Length > 1) && (strArray2[1] == "TJ"))
                    {
                        return strArray2[2];
                    }
                }
                str = "";
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public string GetHumanReadable835(string EDI835, ref dsCache ds, long ReportId, string Filename, string UserName)
        {
            string str;
            try
            {
                MDVUtility.SetDelimiters(EDI835);
                EDI835 = EDI835.Replace("\r\n", "");
                if ((EDI835.Trim().Length > 0) && EDICommon.IsEDI(EDI835))
                {
                    this.FileName = Filename;
                    this.UserName = UserName;
                    this.ReportId = ReportId;
                    this.FinalStr835 = new StringBuilder();
                    this.ds835Parser = ds;
                    this.Parse835(EDI835);
                    return this.FinalStr835.ToString();
                }
                str = "Not an EDI : " + EDI835;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private dsCache.TableBatchRow GetNewBatchRow()
        {
            dsCache.TableBatchRow row2;
            try
            {
                dsCache.TableBatchRow row = this.ds835Parser.TableBatch.NewTableBatchRow();
                row.FIELD_BATCH_UPDATED_BY = this.UserName;
                row.FIELD_BATCH_UPDATED_DATE = DateTime.Now.ToString();
                row.FIELD_BATCH_STATUS = false;
                this.ds835Parser.TableBatch.Rows.Add(row);
                this.CurrentBatchNumber = Convert.ToInt64(row.FIELD_BATCH_BATCH_ID_PK);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_Remittance_CacheRow GetNewCacheRow()
        {
            dsCache.Table_BL_Remittance_CacheRow row2;
            try
            {
                dsCache.Table_BL_Remittance_CacheRow row = this.ds835Parser.Table_BL_Remittance_Cache.NewTable_BL_Remittance_CacheRow();
                row.FIELD_REMIT_BATCH_ID_FK = this.CurrentBatchNumber;
                row.FIELD_REMIT_CLAIM_ID_FK = this.CurrentClaimNumber;
                row.FIELD_REMIT_REPORT_ID_FK = this.ReportId;
                row.FIELD_REMIT_SUBSCRIBER_ID = "SELF";
                if (this.rowCache == null)
                {
                    row.FIELD_REMIT_HCPCS = string.Empty;
                    row.FIELD_REMIT_CHARGE_AMOUNT = 0M;
                    row.FIELD_REMIT_PAID_AMOUNT = 0M;
                    row.FIELD_REMIT_LATE_FILING_CHARGE = 0M;
                    row.FIELD_REMIT_INTEREST = 0M;
                    row.FIELD_REMIT_ALLOWED = 0M;
                    row.FIELD_REMIT_SERVICE_FROM_DATE = string.Empty;
                    row.FIELD_REMIT_SERVICE_TO_DATE = string.Empty;
                    row.FIELD_REMIT_CLAIM_FROM_DATE = string.Empty;
                }
                row.FIELD_REMIT_CLAIM_FROM_DATE = this.rowClaim.FIELD_CLAIM_FROM_DATE;
                row.FIELD_REMIT_PATIENT_ACCOUNT_NO = this.rowClaim.FIELD_CLAIM_ACNT;
                row.FIELD_REMIT_CLAIM_STATUS = this.rowClaim.FIELD_CLAIM_STATUS;
                row.FIELD_REMIT_CLAIM_POS = this.rowClaim.FIELD_CLAIM_POS;
                row.FIELD_REMIT_CLAIM_STATUS_CODE = this.rowClaim.FIELD_CLAIM_STATUS_CODE;
                row.FIELD_REMIT_PATIENT_NAME = this.rowClaim.FIELD_CLAIM_PATIENT_NAME;
                row.FIELD_REMIT_REND_PROVIDER_NPI = this.rowClaim.FIELD_CLAIM_REND_PROVIDER_NPI;
                row.FIELD_REMIT_HICN = this.rowClaim.FIELD_CLAIM_HICN;
                row.FIELD_REMIT_HICN_QUALIFIER = this.rowClaim.FIELD_CLAIM_HICN_QUALIFIER;
                row.FIELD_REMIT_ICN = this.rowClaim.FIELD_CLAIM_ICN;
                if (this.rowClaim.FIELD_CLAIM_SUBSCRIBER_ID.ToString().Trim() != "SELF")
                {
                    row.FIELD_REMIT_SUBSCRIBER_ID = this.rowClaim.FIELD_CLAIM_SUBSCRIBER_ID;
                }
                else
                {
                    row.FIELD_REMIT_SUBSCRIBER_ID = "SELF";
                }

              

                row.FIELD_REMIT_FORWARD_PAYER = this.rowClaim.FIELD_CLAIM_FORWARD_PAYER;
                row.FIELD_REMIT_SEC_SUBSCRIBER_ID = this.rowClaim.FIELD_CLAIM_SEC_SUBSCRIBER_ID;
                row.FIELD_REMIT_PATIENT_LAST_NAME = this.rowClaim.FIELD_CLAIM_PATIENT_LAST_NAME;
                row.FIELD_REMIT_PATIENT_FIRST_NAME = this.rowClaim.FIELD_CLAIM_PATIENT_FIRST_NAME;

                row.FIELD_REMIT_SUBSCRIBER_FIRST_NAME = this.rowClaim.FIELD_CLAIM_SUBSCRIBER_FIRST_NAME;
                row.FIELD_REMIT_SUBSCRIBER_LAST_NAME = this.rowClaim.FIELD_CLAIM_SUBSCRIBER_LAST_NAME;

                row.FIELD_REMIT_PATIENT_MIDDLE_INITIAL = this.rowClaim.FIELD_CLAIM_PATIENT_MIDDLE_INITIAL;
                row.FIELD_REMIT_PAYMENT_DATE = this.rowERA.FIELD_ERA835_CHECK_DATE;
                this.ds835Parser.Table_BL_Remittance_Cache.Rows.Add(row);
                this.CurrentRemitNumber = Convert.ToInt64(row.FIELD_REMIT_BL_REMITTANCE_ID_PK);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_ERA_835Row GetNewERA835Row()
        {
            dsCache.Table_BL_ERA_835Row row2;
            try
            {
                dsCache.Table_BL_ERA_835Row row = this.ds835Parser.Table_BL_ERA_835.NewTable_BL_ERA_835Row();
                row.FIELD_ERA835_BATCH_ID_FK = this.CurrentBatchNumber;
                this.ds835Parser.Table_BL_ERA_835.Rows.Add(row);
                this.CurrentERA835Number = Convert.ToInt64(row.FIELD_ERA835_ID_PK);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_ERA_835_ClaimRow GetNewERAClaimRow()
        {
            dsCache.Table_BL_ERA_835_ClaimRow row2;
            try
            {
                dsCache.Table_BL_ERA_835_ClaimRow row = this.ds835Parser.Table_BL_ERA_835_Claim.NewTable_BL_ERA_835_ClaimRow();
                row.FIELD_CLAIM_ERA835_ID_FK = this.CurrentERA835Number;
                row.FIELD_CLAIM_SUBSCRIBER_ID = "SELF";
                row.FIELD_CLAIM_PATIENT_LAST_NAME = "";
                row.FIELD_CLAIM_PATIENT_FIRST_NAME = "";
                row.FIELD_CLAIM_SUBSCRIBER_LAST_NAME = "";
                row.FIELD_CLAIM_SUBSCRIBER_FIRST_NAME = "";


                row.FIELD_CLAIM_PATIENT_MIDDLE_INITIAL = "";
                row.FIELD_CLAIM_HICN_QUALIFIER = "";
                row.FIELD_CLAIM_STATUS = "";
                row.FIELD_CLAIM_STATUS_CODE = "";
                row.FIELD_CLAIM_POS = "";
                row.FIELD_CLAIM_ICN = "";
                row.FIELD_CLAIM_INTEREST = 0M;
                row.FIELD_CLAIM_PAID_AMOUNT = 0M;
                this.ds835Parser.Table_BL_ERA_835_Claim.Rows.Add(row);
                this.CurrentClaimNumber = Convert.ToInt64(row.FIELD_CLAIM_ID_PK);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_Remittance_Other_Claim_RelatedRow GetNewOtherRefRow()
        {
            dsCache.Table_BL_Remittance_Other_Claim_RelatedRow row2;
            try
            {
                dsCache.Table_BL_Remittance_Other_Claim_RelatedRow row = this.ds835Parser.Table_BL_Remittance_Other_Claim_Related.NewTable_BL_Remittance_Other_Claim_RelatedRow();
                row.FIELD_OTHER_CLAIM_ID_FK = this.CurrentClaimNumber;
                this.ds835Parser.Table_BL_Remittance_Other_Claim_Related.Rows.Add(row);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_Remittance_WriteOffRow GetNewWriteOffRow()
        {
            dsCache.Table_BL_Remittance_WriteOffRow row2;
            try
            {
                dsCache.Table_BL_Remittance_WriteOffRow row = this.ds835Parser.Table_BL_Remittance_WriteOff.NewTable_BL_Remittance_WriteOffRow();
                row.FIELD_WRITE_OFF_BL_REMITTANCE_ID_FK = this.CurrentRemitNumber;
                this.ds835Parser.Table_BL_Remittance_WriteOff.Rows.Add(row);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private dsCache.Table_BL_Provider_AdjustmentRow GetNewProvideradjustmentRow()
        {
            dsCache.Table_BL_Provider_AdjustmentRow row2;
            try
            {
                dsCache.Table_BL_Provider_AdjustmentRow row = this.ds835Parser.Table_BL_Provider_Adjustment.NewTable_BL_Provider_AdjustmentRow();
                row.FIELD_PLB_AMOUNT = 0;
                row.FIELD_PLB_REFRENCE_IDENTIFIER = this.ProviderIdentifier;
                row.FIELD_PLB_DATE = this.ProviderDate;
                row.FIELD_PLB_REASON_CODE = string.Empty;
                row.FIELD_PLB_DESCRIPTION = string.Empty;
                row.FIELD_REMIT_BATCH_ID_FK = this.CurrentBatchNumber;
                this.ds835Parser.Table_BL_Provider_Adjustment.Rows.Add(row);
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }


        public void GetStatusAndClaimNo(string StrIn, ref ArrayList ClaimIdAndStatus)
        {
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                int index = 0;
                for (index = 0; index <= (strArray.Length - 2); index++)
                {
                    string str4 = strArray[index].Substring(0, strArray[index].IndexOf(MDVUtility.D_ELMT));
                    if ((str4 != null) && (str4 == "CLP"))
                    {
                        string[] strArray2 = strArray[index].Split(new char[] { MDVUtility.D_ELMT });
                        if (strArray2[0] != "CLP")
                        {
                            return;
                        }
                        string pAN = "";
                        string str2 = "";
                        string statusDescription = "";
                        if (strArray2.Length > 1)
                        {
                            pAN = strArray2[1];
                        }
                        if (strArray2.Length > 2)
                        {
                            statusDescription = this.CLP02(strArray2[2]);
                            str2 = this.CLP02Status(strArray2[2]);
                        }
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this.PushStatusAndClaimNo(ref ClaimIdAndStatus, pAN, str2, statusDescription);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private string GS(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "GS")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 4:
                                this.ReportDate = MDVUtility.StringToDate(strArray[index]);
                                break;

                            case 5:
                                this.ReportTime = MDVUtility.StringToTime(strArray[index]);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GS01(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "HP"))
            {
                return "Health Care Claim Payment/Advice (835)";
            }
            return "### Unknown GS01 ###";
        }

        private string GS07(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "X"))
            {
                return "Accredited Standards Committee X12";
            }
            return "### Unknown GS07 ###";
        }

        private string GS08(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "004010X091"))
            {
                return "Draft Standards Approved for Publication by ASC X12 Procedures Review Board through October 1997, as published in this implementation guide.";
            }
            return "### Unknown GS08 ###";
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private string ISA01(string Code)
        {
            string obj2 = "Unknown";
            string str2 = Code;
            if (str2 != null)
            {
                if (!(str2 == "00"))
                {
                    if (str2 == "03")
                    {
                        obj2 = "Additional Data Identification";
                        goto Label_0040;
                    }
                }
                else
                {
                    obj2 = "No Authorization Information Present (No Meaningful Information in I02)";
                    goto Label_0040;
                }
            }
            else
                obj2 = "### Unknown ISA01 ###";

        Label_0040:
            //if (<ISA01>o__SiteContainer0.<>p__Site1 == null)
            //{
            //    <ISA01>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EDI835Parser)));
            //}
            //return <ISA01>o__SiteContainer0.<>p__Site1.Target(<ISA01>o__SiteContainer0.<>p__Site1, obj2);

            return obj2;
        }

        private string ISA03(string Code)
        {
            string obj2 = "Unknown";
            string str2 = Code;
            if (str2 != null)
            {
                if (!(str2 == "00"))
                {
                    if (str2 == "01")
                    {
                        obj2 = "Password";
                        goto Label_0040;
                    }
                }
                else
                {
                    obj2 = "No Security Information Present (No Meaningful Information in I04)";
                    goto Label_0040;
                }
            }
            else
                obj2 = "### Unknown ISA03 ###";


        Label_0040:
            //    if (<ISA03>o__SiteContainer2.<>p__Site3 == null)
            //    {
            //        <ISA03>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EDI835Parser)));
            //    }
            //    return <ISA03>o__SiteContainer2.<>p__Site3.Target(<ISA03>o__SiteContainer2.<>p__Site3, obj2);

            return obj2;
        }

        private string ISA05(string Code)
        {
            switch (Code)
            {
                case "01":
                    return "Duns (Dun & Bradstreet)";

                case "14":
                    return "Duns Plus Suffix";

                case "20":
                    return "Health Industry Number (HIN)";

                case "27":
                    return "Carrier Identification Number as assigned by Health Care Financing Administration (HCFA)";

                case "28":
                    return "Fiscal Intermediary Identification Number as assigned by Health Care Financing Administration (HCFA)";

                case "29":
                    return "Medicare Provider and Supplier Identification Number as assigned by Health Care Financing Administration(HCFA)";

                case "30":
                    return "U.S. Federal Tax Identification Number";

                case "33":
                    return "National Association of Insurance Commissioners Company Code(NAIC)";

                case "ZZ":
                    return "Mutually Defined";
            }
            return "### Unknown ISA05 ###";
        }

        private string ISA07(string Code)
        {
            switch (Code)
            {
                case "01":
                    return "Duns (Dun & Bradstreet)";

                case "14":
                    return "Duns Plus Suffix";

                case "20":
                    return "Health Industry Number (HIN)";

                case "27":
                    return "Carrier Identification Number as assigned by Health Care Financing Administration (HCFA)";

                case "28":
                    return "Fiscal Intermediary Identification Number as assigned by Health Care Financing Administration (HCFA)";

                case "29":
                    return "Medicare Provider and Supplier Identification Number as assigned by Health Care Financing Administration(HCFA)";

                case "30":
                    return "U.S. Federal Tax Identification Number";

                case "33":
                    return "National Association of Insurance Commissioners Company(Code(NAIC)";

                case "ZZ":
                    return "Mutually Defined";
            }
            return "### Unknown ISA07 ###";
        }

        private string ISA11(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "U"))
            {
                return " U.S. EDI Community of ASC X12, TDCC, and UCS";
            }
            return "### Unknown ISA11 ###";
        }

        private string ISA12(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "00401"))
            {
                return "Draft Standards for Trial Use Approved for Publication by ASC X12 Procedures Review Board through October 1997";
            }
            return "### Unknown ISA12 ###";
        }

        private string ISA14(string Code)
        {
            switch (Code)
            {
                case "0":
                    return "No Interchange Acknowledgment Requested";

                case "1":
                    return "Interchange Acknowledgment Requested (TA1)";
            }
            return "### Unknown ISA14 ###";
        }

        private string ISA15(string Code)
        {
            switch (Code)
            {
                case "P":
                    return "Production Data";

                case "T":
                    return "Test Data";
            }
            return "### Unknown ISA15 ###";
        }

        private string LQ(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "LQ")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append(this.LQ01(strArray[index]));
                                builder.Append(": ");
                                break;

                            case 2:
                                if (!(strArray[index - 1] == "HE"))
                                {
                                    goto Label_0136;
                                }
                                this.rowCache.FIELD_REMIT_HC_CODE = strArray[index];
                                this.rowCache.FIELD_REMIT_HC_REMARK_CODE = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;
                        }
                    }
                    continue;
                Label_0136:
                    if (strArray[index - 1] == "RX")
                    {
                        builder.Append(strArray[index]);
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string LQ01(string Code)
        {
            switch (Code)
            {
                case "HE":
                    return "Claim Payment Remark";

                case "RX":
                    return "National Council for Prescription Drug Programs Reject/Payment Codes";
            }
            return "### Unknown LQ01 ###";
        }

        private string LX(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "LX")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                {
                                    if (strArray[index] == "1")
                                    {
                                        this.rowERA.FIELD_ERA835_ASG = "Y";
                                    }
                                    else
                                    {
                                        this.rowERA.FIELD_ERA835_ASG = "N";
                                    }
                                }
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string MIA(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "MIA")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                    0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18
                 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append("Covered Days or Visits Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS Operating Outlier Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Lifetime Psychiatric Days Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 5:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Disproportionate Share Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim MSP Pass-through Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 8:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim PPS Capital Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Capital FSP DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 10:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Capital HSP DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 11:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Capital DSH DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Old Capital Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 13:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Capital IME amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 14:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Operating Hospital Specific DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 15:
                                builder.Append(Environment.NewLine);
                                builder.Append("Cost Report Day Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x10:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Operating Federal Specific DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x11:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim PPS Capital Outlier Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim Indirect Teaching Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Nonpayable Professional Component Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 20:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;

                            case 0x15:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;

                            case 0x16:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;

                            case 0x17:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                break;

                            case 0x18:
                                builder.Append(Environment.NewLine);
                                builder.Append("PPS-Capital Exception Amount: ");
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string MOA(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "MOA")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append("Reimbursement Rate Percent: ");
                                builder.Append(strArray[index]);
                                break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim HCPCS Payable Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                this.rowClaim.FIELD_CLAIM_MOA_CODE = strArray[index];
                                this.rowClaim.FIELD_CLAIM_MOA_REMARKS = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                if (string.IsNullOrEmpty(this.rowClaim.FIELD_CLAIM_MOA_CODE.ToString()))
                                {
                                    goto Label_023D;
                                }
                                this.rowClaim.FIELD_CLAIM_MOA_CODE = this.rowClaim.FIELD_CLAIM_MOA_CODE + " " + strArray[index];
                                this.rowClaim.FIELD_CLAIM_MOA_REMARKS = this.rowClaim.FIELD_CLAIM_MOA_REMARKS + Environment.NewLine + "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                break;

                            case 5:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                if (string.IsNullOrEmpty(this.rowClaim.FIELD_CLAIM_MOA_CODE.ToString()))
                                {
                                    goto Label_0348;
                                }
                                this.rowClaim.FIELD_CLAIM_MOA_CODE = this.rowClaim.FIELD_CLAIM_MOA_CODE + " " + strArray[index];
                                this.rowClaim.FIELD_CLAIM_MOA_REMARKS = this.rowClaim.FIELD_CLAIM_MOA_REMARKS + Environment.NewLine + "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                if (string.IsNullOrEmpty(this.rowClaim.FIELD_CLAIM_MOA_CODE.ToString()))
                                {
                                    goto Label_0453;
                                }
                                this.rowClaim.FIELD_CLAIM_MOA_CODE = this.rowClaim.FIELD_CLAIM_MOA_CODE + " " + strArray[index];
                                this.rowClaim.FIELD_CLAIM_MOA_REMARKS = this.rowClaim.FIELD_CLAIM_MOA_REMARKS + Environment.NewLine + "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                break;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Remark: ");
                                builder.Append(this.RemittanceRemarkCode(strArray[index]));
                                if (string.IsNullOrEmpty(this.rowClaim.FIELD_CLAIM_MOA_CODE.ToString()))
                                {
                                    goto Label_055E;
                                }
                                this.rowClaim.FIELD_CLAIM_MOA_CODE = this.rowClaim.FIELD_CLAIM_MOA_CODE + " " + strArray[index];
                                this.rowClaim.FIELD_CLAIM_MOA_REMARKS = this.rowClaim.FIELD_CLAIM_MOA_REMARKS + Environment.NewLine + "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                                break;

                            case 8:
                                builder.Append(Environment.NewLine);
                                builder.Append("Claim ESRD Payment Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("Nonpayable Professional Component Amount: ");
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                    continue;
                Label_023D:
                    this.rowClaim.FIELD_CLAIM_MOA_CODE = strArray[index];
                    this.rowClaim.FIELD_CLAIM_MOA_REMARKS = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                    continue;
                Label_0348:
                    this.rowClaim.FIELD_CLAIM_MOA_CODE = strArray[index];
                    this.rowClaim.FIELD_CLAIM_MOA_REMARKS = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                    continue;
                Label_0453:
                    this.rowClaim.FIELD_CLAIM_MOA_CODE = strArray[index];
                    this.rowClaim.FIELD_CLAIM_MOA_REMARKS = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                    continue;
                Label_055E:
                    this.rowClaim.FIELD_CLAIM_MOA_CODE = strArray[index];
                    this.rowClaim.FIELD_CLAIM_MOA_REMARKS = "{ " + strArray[index] + " } -  " + this.RemittanceRemarkCode(strArray[index]);
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string N1(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "N1")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                {
                                    builder.Append(this.N101(strArray[index]));
                                    builder.Append(": ");
                                    continue;
                                }
                            case 3:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append(this.N103(strArray[index]));
                                    builder.Append(": ");
                                    continue;
                                }
                        }
                        builder.Append(strArray[index]);
                        builder.Append(" ");
                    }
                }
                if (strArray.Length > 1)
                {
                    this.PayerAndPayeeQualifier = strArray[1];
                    if (strArray[1] == "PR")
                    {
                        this.rowERA.FIELD_ERA835_PAYER_NAME_QUALIFIER = strArray[1];
                        this.rowERA.FIELD_ERA835_PAYER_NAME = strArray[2];
                    }
                    else if (strArray[1] == "PE")
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_NAME_QUALIFIER = strArray[1];
                        this.rowERA.FIELD_ERA835_PAYEE_NAME = strArray[2];
                    }
                    if ((strArray.Length > 3) && (strArray[1] == "PR"))
                    {
                        this.rowERA.FIELD_ERA835_PAYER_ID_QUALIFIER = strArray[3];
                        this.rowERA.FIELD_ERA835_PAYER_ID = strArray[4];
                    }
                    if ((strArray.Length > 3) && (strArray[1] == "PE"))
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_ID_QUALIFIER = strArray[3];
                        this.rowERA.FIELD_ERA835_PAYEE_ID = strArray[4];
                        if (strArray[3] == "FI")
                        {
                            this.rowBatch.FIELD_BATCH_PROVIDER_TAXID = strArray[4];
                            this.rowERA.FIELD_ERA835_PAYEE_EIN_NO = strArray[4];
                        }
                        else if (strArray[3] == "XX")
                        {
                            this.rowBatch.FIELD_BATCH_BILLING_PROVIDER_NPI = strArray[4];
                            this.rowERA.FIELD_ERA835_BILLING_PROVIDER_NPI = strArray[4];
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string N101(string Code)
        {
            switch (Code)
            {
                case "PR":
                    return "Payer";

                case "PE":
                    return "Payee";
            }
            return "### Unknown N101 ###";
        }

        private string N103(string Code)
        {
            switch (Code)
            {
                case "XV":
                    return "Health Care Financing Administration National PlanID";

                case "FI":
                    return "Federal Taxpayer’s Identification Number";

                case "XX":
                    return "National Provider Identifier";
            }
            return "### Unknown N103 ###";
        }

        private string N3(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "N3")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                builder.Append("Address : ");
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        builder.Append(strArray[index]);
                        builder.Append(Environment.NewLine);
                    }
                }
                if (strArray.Length > 1)
                {
                    if (this.PayerAndPayeeQualifier == "PR")
                    {
                        this.rowERA.FIELD_ERA835_PAYER_ADDRESS = strArray[1];
                    }
                    if (this.PayerAndPayeeQualifier == "PE")
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_ADDRESS = strArray[1];
                    }
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string N4(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "N4")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        if (index == 3)
                        {
                            builder.Append(MDVUtility.ZipCode(strArray[index]));
                        } 
                        else
                        {
                            builder.Append(strArray[index]);
                            builder.Append(" ");
                        }
                    }
                }
                if (strArray.Length > 2)
                {
                    if (this.PayerAndPayeeQualifier == "PR")
                    {
                        this.rowERA.FIELD_ERA835_PAYER_CITY = strArray[1];
                        this.rowERA.FIELD_ERA835_PAYER_STATE = strArray[2];
                        this.rowERA.FIELD_ERA835_PAYER_POSTAL_CODE = strArray[3];
                    }
                    if (this.PayerAndPayeeQualifier == "PE")
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_CITY = strArray[1];
                        this.rowERA.FIELD_ERA835_PAYEE_STATE = strArray[2];
                        this.rowERA.FIELD_ERA835_PAYEE_POSTAL_CODE = strArray[3];
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string NM1(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "NM1")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 6, 3, 4, 5, 7, 8, 9, 10, 11, 12 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                builder.Append(this.NM101(strArray[num2]));
                                builder.Append(": ");
                                continue;
                            }
                        case 3:
                            {
                                if (strArray[1] == "QC")
                                {
                                    this.rowClaim.FIELD_CLAIM_PATIENT_LAST_NAME = strArray[num2];
                                    this.rowCache.FIELD_REMIT_PATIENT_LAST_NAME = strArray[num2];
                                }
                                else if (strArray[1] == "IL" || strArray[1] == "74")
                                {
                                    this.rowClaim.FIELD_CLAIM_SUBSCRIBER_LAST_NAME = strArray[num2];
                                    this.rowCache.FIELD_REMIT_SUBSCRIBER_LAST_NAME = strArray[num2];
                                }
                               
                                continue;
                            }
                        case 4:
                            {
                                if (strArray[1] == "QC")
                                {
                                    if (strArray[num2] != "")
                                    {
                                        this.rowClaim.FIELD_CLAIM_PATIENT_NAME = strArray[3] + ", " + strArray[num2];
                                    }
                                    this.rowClaim.FIELD_CLAIM_PATIENT_FIRST_NAME = strArray[num2];
                                    this.rowCache.FIELD_REMIT_PATIENT_FIRST_NAME = strArray[num2];
                                }
                                else if (strArray[1] == "IL" || strArray[1] == "74")
                                {
                                    this.rowClaim.FIELD_CLAIM_SUBSCRIBER_FIRST_NAME = strArray[num2];
                                    this.rowCache.FIELD_REMIT_SUBSCRIBER_FIRST_NAME = strArray[num2];
                                }
                              
                                continue;
                            }
                        case 5:
                            {
                                if (strArray[1] == "QC")
                                {
                                    this.rowClaim.FIELD_CLAIM_PATIENT_NAME = strArray[3] + ", " + strArray[4] + " " + strArray[5];
                                    this.rowCache.FIELD_REMIT_PATIENT_NAME = strArray[3] + ", " + strArray[4] + " " + strArray[5];
                                    this.rowCache.FIELD_REMIT_PATIENT_MIDDLE_INITIAL = strArray[num2];
                                    this.rowClaim.FIELD_CLAIM_PATIENT_MIDDLE_INITIAL = strArray[num2];
                                }
                                continue;
                            }
                        case 8:
                            {
                                builder.Append("\r\n");
                                builder.Append(this.NM108(strArray[num2]));
                                builder.Append(": ");
                                continue;
                            }
                        case 9:
                            if (!(strArray[1] == "QC") || !(((((strArray[8] == "MI") | (strArray[8] == "HN")) | (strArray[8] == "II")) | (strArray[8] == "34")) | (strArray[8] == "MR")))
                            {
                                goto Label_0372;
                            }
                            if (!(strArray[8] == "34"))
                            {
                                break;
                            }
                            this.rowClaim.FIELD_CLAIM_HICN_QUALIFIER = "SSN";
                            goto Label_0339;

                        default:
                            goto Label_0480;
                    }
                    if (strArray[8] == "HN")
                    {
                        this.rowClaim.FIELD_CLAIM_HICN_QUALIFIER = "HIC";
                    }
                    else
                    {
                        this.rowClaim.FIELD_CLAIM_HICN_QUALIFIER = strArray[8];
                    }
                Label_0339:
                    this.rowCache.FIELD_REMIT_HICN_QUALIFIER = this.rowClaim.FIELD_CLAIM_HICN_QUALIFIER;
                    this.rowClaim.FIELD_CLAIM_HICN = strArray[9];
                    this.rowCache.FIELD_REMIT_HICN = strArray[9];
                Label_0372:
                    if ((strArray[1] == "82") & (strArray[8] == "XX"))
                    {
                        this.rowClaim.FIELD_CLAIM_REND_PROVIDER_NPI = strArray[9];
                        this.rowBatch.FIELD_BATCH_REND_PROVIDER_NPI = strArray[9];
                        this.rowCache.FIELD_REMIT_REND_PROVIDER_NPI = strArray[9];
                    }
                    if (strArray[1] == "TT")
                    {
                        this.rowClaim.FIELD_CLAIM_FORWARD_PAYER = strArray[3];
                        this.rowClaim.FIELD_CLAIM_FORWARD_PAYER_CODE = strArray[9];
                        this.rowCache.FIELD_REMIT_FORWARD_PAYER = strArray[3];
                    }
                    if (strArray[1] == "IL" || strArray[1] == "74")
                    {
                        this.rowClaim.FIELD_CLAIM_SUBSCRIBER_ID = strArray[9];
                        this.rowCache.FIELD_REMIT_SUBSCRIBER_ID = strArray[9];
                    }
                    
                    if (strArray[1] == "GB")
                    {
                        this.rowClaim.FIELD_CLAIM_SEC_SUBSCRIBER_ID = strArray[9];
                        this.rowCache.FIELD_REMIT_SEC_SUBSCRIBER_ID = strArray[9];
                    }
                    continue;
                Label_0480:
                    builder.Append(strArray[num2]);
                    builder.Append(" ");
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string NM101(string Code)
        {
            switch (Code)
            {
                case "QC":
                    return "Patient";

                case "IL":
                    return "Insured or Subscriber";

                case "74":
                    return "Corrected Insured";

                case "82":
                    return "Rendering Provider";

                case "TT":
                    return "Transfer To";

                case "PR":
                    return "Payer";
            }
            return "### Unknown NM101 ###";
        }

        private string NM102(string Code)
        {
            switch (Code)
            {
                case "1":
                    return "Person";

                case "2":
                    return "Non-Person Entity";
            }
            return "### Unknown NM102 ###";
        }

        private string NM103(string Code)
        {
            switch (Code)
            {
                case "FI":
                    return "Federal Taxpayer’s Identification Number";

                case "XV":
                    return "Centers for Medicare and Medicaid Services PlanID";

                case "XX":
                    return "National Provider Identifier";
            }
            return "### Unknown NM103 ###";
        }

        private string NM108(string Code)
        {
            switch (Code)
            {
                case "34":
                    return "Social Security Number";

                case "HN":
                    return "HICN";

                case "II":
                    return "Standard Unique Health Identifier for each Individual in the United States";

                case "MI":
                    return "Member Identification Number";

                case "MR":
                    return "Medicaid Recipient Identification Number";

                case "C":
                    return "Insured’s Changed Unique Identification Number";

                case "BD":
                    return "Blue Cross Provider Number";

                case "BS":
                    return "Blue Shield Provider Number";

                case "FI":
                    return "Federal Taxpayer’s Identification Number";

                case "MC":
                    return "Medicaid Provider Number";

                case "PC":
                    return "Provider Commercial Number";

                case "SL":
                    return "State License Number";

                case "UP":
                    return "Unique Physician Identification Number (UPIN)";

                case "XX":
                    return "National Provider Identifier";

                case "AD":
                    return "Blue Cross Blue Shield Association Plan Code";

                case "NI":
                    return "National Association of Insurance Commissioners (NAIC) Identification";

                case "PI":
                    return "Payor Identification";

                case "PP":
                    return "Pharmacy Processor Number";

                case "XV":
                    return "Centers for Medicare and Medicaid Services PlanID";
            }
            return "### Unknown NM108 ###";
        }

        private void Parse835(string StrIn)
        {
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                int index = 0;
                for (index = 0; index <= (strArray.Length - 2); index++)
                {
                    switch (strArray[index].Substring(0, strArray[index].IndexOf(MDVUtility.D_ELMT)))
                    {
                        case "ISA":
                            this.FinalStr835.Append("ISA ##########################################################################");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                        case "IEA":
                            this.FinalStr835.Append("IEA ##########################################################################");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                        case "GS":
                            this.FinalStr835.Append("GS  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                            this.GS(strArray[index]);
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                       

                        case "CLP":
                            this.rowClaim = this.GetNewERAClaimRow();
                            this.ClaimId++;
                            this.FinalStr835.Append(this.CLP(strArray[index]));
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            this.rowERA.FIELD_ERA835_CLAIM_NO = this.ClaimId;
                            break;

                      
                        case "NM1":
                            this.FinalStr835.Append(this.NM1(strArray[index]));
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                        case "MIA":
                            this.FinalStr835.Append(this.MIA(strArray[index]));
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                        case "MOA":
                            this.FinalStr835.Append(this.MOA(strArray[index]));
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;

                       

                        default:
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            this.FinalStr835.Append(strArray[index]);
                            this.FinalStr835.Append("----------------------------------------------------------------");
                            this.FinalStr835.Append(Environment.NewLine);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private string PER(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "PER")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.PER01(strArray[num2]));
                                builder.Append(Environment.NewLine);
                                break;

                            case 2:
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                if (strArray[1] == "BL")
                                {
                                    this.rowERA.FIELD_ERA835_PAYER_TEC_NAME = strArray[num2];
                                }
                                break;

                            case 3:
                                builder.Append(this.PER03(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 4:
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                if (strArray[1] == "CX")
                                {
                                    this.rowERA.FIELD_ERA835_PAYER_PHONE_NO = MDVUtility.PhoneNo(strArray[num2]);
                                } 
                                if (strArray[1] == "BL")
                                {
                                    this.rowERA.FIELD_ERA835_PAYER_TEC_NO = MDVUtility.PhoneNo(strArray[num2]);
                                }
                                break;

                            case 5:
                                builder.Append(this.PER05(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 6:
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                break;

                            case 7:
                                builder.Append(this.PER07(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 8:
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string PER01(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "CX"))
            {
                return "Payers Claim Office";
            }
            return "### Unknown PER01 ###";
        }

        private string PER03(string Code)
        {
            switch (Code)
            {
                case "EM":
                    return "Electronic Mail";

                case "FX":
                    return "Facsimile";

                case "TE":
                    return "Telephone";
            }
            return "### Unknown PER03 ###";
        }

        private string PER05(string Code)
        {
            switch (Code)
            {
                case "EM":
                    return "Electronic Mail";

                case "EX":
                    return "Telephone Extension";

                case "FX":
                    return "Facsimile";

                case "TE":
                    return "Telephone";
            }
            return "### Unknown PER05 ###";
        }

        private string PER07(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "EX"))
            {
                return "Telephone Extension";
            }
            return "### Unknown PER07 ###";
        }

        private string PLB(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "PLB")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    string[] strArray2;
                    int num3;
                    string[] strArray3;
                    string[] strArray4;
                    string[] strArray5;
                    string[] strArray6;
                    string[] strArray7;
                    int index = numArray[i];
             
                    continue;
                Label_015C:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray2[num3];
                        builder.Append(this.PLB03_1(strArray2[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray2[num3]);
                    }
                    num3++;
                Label_01B0:
                    if (num3 <= (strArray2.Length - 1))
                    {
                        goto Label_015C;
                    }
                    continue;
                Label_021F:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray3[num3];
                        builder.Append(this.PLB03_1(strArray3[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray3[num3]);
                    }
                    num3++;
                Label_0273:
                    if (num3 <= (strArray3.Length - 1))
                    {
                        goto Label_021F;
                    }
                    continue;
                Label_02E2:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray4[num3];
                        builder.Append(this.PLB03_1(strArray4[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray4[num3]);
                    }
                    num3++;
                Label_0336:
                    if (num3 <= (strArray4.Length - 1))
                    {
                        goto Label_02E2;
                    }
                    continue;
                Label_03A5:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray5[num3];
                        builder.Append(this.PLB03_1(strArray5[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray5[num3]);
                    }
                    num3++;
                Label_03F9:
                    if (num3 <= (strArray5.Length - 1))
                    {
                        goto Label_03A5;
                    }
                    continue;
                Label_0468:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray6[num3];
                        builder.Append(this.PLB03_1(strArray6[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray6[num3]);
                    }
                    num3++;
                Label_04BC:
                    if (num3 <= (strArray6.Length - 1))
                    {
                        goto Label_0468;
                    }
                    continue;
                Label_052B:
                    if (num3 == 0)
                    {
                        this.rowProviderAdjustment = this.GetNewProvideradjustmentRow();
                        this.rowProviderAdjustment.FIELD_PLB_REASON_CODE = strArray7[num3];
                        builder.Append(this.PLB03_1(strArray7[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append(strArray7[num3]);
                    }
                    num3++;
                Label_057F:
                    if (num3 <= (strArray7.Length - 1))
                    {
                        goto Label_052B;
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string PLB03_1(string Code)
        {
            string Outstr = string.Empty;

            switch (Code)
            {
                case "50 ":
                    Outstr = "Late Charge"; break;

                case "51":
                    Outstr = "Interest Penalty Charge"; break;

                case "72":
                    Outstr = "Authorized Return"; break;

                case "90":
                    Outstr = "Early Payment Allowance"; break;

                case "AM":
                    Outstr = "Applied to Borrower’s Account"; break;

                case "AP":
                    Outstr = "Acceleration of Benefits"; break;

                case "B2":
                    Outstr = "Rebate"; break;

                case "B3":
                    Outstr = "Recovery Allowance"; break;

                case "BD":
                    Outstr = "Bad Debt Adjustment"; break;

                
                case "ZZ":
                    Outstr = "Mutually Defined";break;

                default:
                    Outstr = "### Unknown PLB03 - 1 ###";break;
            }

            

            if (!string.IsNullOrEmpty(Outstr))
                this.rowProviderAdjustment.FIELD_PLB_DESCRIPTION = Outstr;

            return Outstr;
        }

        public string ProductionDate(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                int index = 0;
                
                str = "";
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private void PushStatusAndClaimNo(ref ArrayList ClaimIdAndStatus, string PAN, string Status, string StatusDescription)
        {
            StructReportId id = new StructReportId
            {
                ID = PAN,
                Status = Status.ToUpper(),
                StatusDescription = StatusDescription
            };
            ClaimIdAndStatus.Add(id);
        }

        private string QTY(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "QTY")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append(this.QTY01(strArray[index]));
                                builder.Append(": ");
                                break;

                            case 2:
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string QTY01(string Code)
        {
            switch (Code)
            {
                case "CA":
                    return "Covered - Actual";

                case "CD":
                    return "Co - insured - Actual";

                case "LA":
                    return "Life-time Reserve - Actual";

                case "LE":
                    return "Life-time Reserve - Estimated";

                case "NA":
                    return "Number of Non-covered Days";

                case "NE":
                    return "Non - Covered - Estimated";

                case " NR":
                    return "Not Replaced Blood Units";

                case "OU":
                    return "Outlier Days";

                case "PS":
                    return "Prescription";

                case "VS":
                    return "Visits";

                case "ZK":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 1";

                case "ZL":
                    return "Federal Medicare or Medicaid Payment Mandate - Category2";

                case "ZM":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 3";

                case " ZN":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 4";

                case "ZO":
                    return "Federal Medicare or Medicaid Payment Mandate - Category 5";
            }
            return "### Unknown QTY01 ###";
        }

        private string REF(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "REF")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.REF01(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 2:
                                builder.Append(strArray[num2]);
                                if ((((((((((((((strArray[1] == "1L") | (strArray[1] == "1W")) | (strArray[1] == "28")) | (strArray[1] == "6P")) | (strArray[1] == "9A")) | (strArray[1] == "9C")) | (strArray[1] == "BB")) | (strArray[1] == "CE")) | (strArray[1] == "EA")) | (strArray[1] == "F8")) | (strArray[1] == "G1")) | (strArray[1] == "G3")) | (strArray[1] == "IG")) | (strArray[1] == "SY"))
                                {
                                    this.rowOtherRef = this.GetNewOtherRefRow();
                                    this.rowOtherRef.FIELD_OTHER_CLAIM_REL = strArray[num2];
                                    this.rowOtherRef.FIELD_OTHER_CLAIM_REL_QUALIFIER = strArray[1];
                                }
                                break;
                        }
                    }
                }
                if (strArray.Length > 1)
                {
                    if (strArray[1] == "2U")
                    {
                        this.rowERA.FIELD_ERA835_PAYER_ID_QUALIFIER = this.REF01(strArray[1]);
                        this.rowERA.FIELD_ERA835_PAYER_ID = strArray[2];
                        this.rowBatch.FIELD_BATCH_PAYER_MCID = strArray[2];
                    }
                    else if (strArray[1] == "TJ")
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_EIN_NO = strArray[2];
                        this.rowBatch.FIELD_BATCH_PROVIDER_TAXID = strArray[2];
                    }
                    else if (strArray[1] == "EV")
                    {
                        this.rowERA.FIELD_ERA835_RECEIVER_IDENTIFIER_QUALIFIER = this.REF01(strArray[1]);
                        this.rowERA.FIELD_ERA835_RECEIVER_IDENTIFIER = strArray[2];
                    }
                    else if (strArray[1] == "1C")
                    {
                        this.rowERA.FIELD_ERA835_PROVIDER_NO = strArray[2];
                    }
                    else if (strArray[1] == "1D")
                    {
                        this.rowERA.FIELD_ERA835_PROVIDER_NO = strArray[2];
                    }
                    else if (strArray[1] == "LU")
                    {
                        this.rowCache.FIELD_REMIT_SERVICE_POS = strArray[2];
                    }
                    else if (strArray[1] == "6R")
                    {
                        this.rowCache.FIELD_REMIT_CHARGE_NUMBER = strArray[2];
                    }
                    else if (strArray[1] == "PQ")
                    {
                        this.rowERA.FIELD_ERA835_PAYEE_IDENTIFICATION = strArray[2];
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string REF01(string Code)
        {
            switch (Code)
            {
                case "EV":
                    return "Receiver Identification Number";

                case "F2":
                    return "Version Code - Local";

                case "2U":
                    return "Payer Identification Number";

                case "EO":
                    return "Submitter Identification Number";

                case "HI":
                    return "Health Industry Number (HIN)";

                case "NF":
                    return "National Association of Insurance Commissioners (NAIC) Code";

                case "0B":
                    return "State License Number";

                case "1A":
                    return "Blue Cross Provider Number";

                case "1B":
                    return "Blue Shield Provider Number";

                case "1C":
                    return "Medicare Provider Number";

                case "1D":
                    return "Medicaid Provider Number";

                case "1E":
                    return "Dentist License Number";

                case "1F":
                    return "Anesthesia License Number";

                case "1G":
                    return "Provider UPIN Number";

                case "D3":
                    return "National Council for Prescription Drug Programs";

                case "PQ":
                    return "Payee Identification";

                case "TJ":
                    return "Federal Taxpayer's Identification Number";

                case "1L":
                    return "Group or Policy Number";

                case "1W":
                    return "Member Identification Number";

                case "28":
                    return "Employee Identification Number";

                case "6P":
                    return "Group Number";

                case "9A":
                    return "Repriced Claim Reference Number";

                case "9C":
                    return "Adjusted Repriced Claim Reference Number";

                case "BB":
                    return "Authorization Number";

                case "CE":
                    return "Class of Contract Code";

               
                case "HPI":
                    return "Centers for Medicare and Medicaid Services National Provider Identifier";
            }
            return "### Unknown REF01 ###";
        }

        private string RemittanceRemarkCode(string Code)
        {
            string str = "Unknown";
            try
            {
                switch (Code)
                {
                    case "M1":
                        str = "X-ray not taken within the past 12 months or near enough to the start of treatment";
                        break;

                    case "M2":
                        str = " \tNot paid separately when the patient is an inpatient";
                        break;

                    case "M3":
                        str = "Equipment is the same or similar to equipment already being used";
                        break;

                    case "M4":
                        str = "This is the last monthly installment payment for this durable medical equipment";
                        break;

                    case "M5":
                        str = "Monthly rental payments can continue until the earlier of the 15th month from the first rental month, or the month when the equipment is no longer needed";
                        break;

                    case "M6":
                        str = "You must furnish and service this item for as long as the patient continues to need it. We can pay for maintenance and/or servicing for every 6 month period after the end of the 15th paid rental month or the end of the warranty period";
                        break;

                    case "M7":
                        str = "No rental payments after the item is purchased, or after the total of issued rental payments equals the purchase price";
                        break;

                    case "M8":
                        str = "We do not accept blood gas tests results when the test was conducted by a medical supplier or taken while the patient is on oxygen";
                        break;

                    case "M9":
                        str = "This is the tenth rental month. You must offer the patient the choice of changing the rental to a purchase agreement";
                        break;

                    case "M10":
                        str = "Equipment purchases are limited to the first or the tenth month of medical necessity";
                        break;

                    case "M11":
                        str = "DME, orthotics and prosthetics must be billed to the DME carrier who services the patient's zip code";
                        break;

                    case "M12":
                        str = "Diagnostic tests performed by a physician must indicate whether purchased services are included on the claim";
                        break;

                    case "M13":
                        str = "Only one initial visit is covered per specialty per medical group";
                        break;

                    case "M14":
                        str = "No separate payment for an injection administered during an office visit, and no payment for a full office visit if the patient only received an injection";
                        break;

                    case "M15":
                        str = "Separately billed services/tests have been bundled as they are considered components of the same procedure. Separate payment is not allowed";
                        break;

                    case "M16":
                        str = "Please see our web site, mailings, or bulletins for more details concerning this policy/procedure/decision";
                        break;

                    case "M17":
                        str = "Payment approved as you did not know, and could not reasonably have been expected to know, that this would not normally have been covered for this patient. In the future, you will be liable for charges for the same service(s) under the same or similar conditions";
                        break;

                    case "M18":
                        str = "Certain services may be approved for home use. Neither a hospital nor a Skilled Nursing Facility (SNF) is considered to be a patient's home";
                        break;

                    case "M19":
                        str = "Missing oxygen certification/re-certification";
                        break;

                    case "M20":
                        str = "Missing/incomplete/invalid HCPCS";
                        break;

                    case "M21":
                        str = "Missing/incomplete/invalid place of residence for this service/item provided in a home";
                        break;

                    case "M22":
                        str = "Missing/incomplete/invalid number of miles traveled";
                        break;

                    case "M23":
                        str = "Missing invoice";
                        break;

                    case "M24":
                        str = "Missing/incomplete/invalid number of doses per vial";
                        break;

                    case "M25":
                        str = "Payment has been adjusted because the information furnished does not substantiate the need for this level of service. If you believe the service should have been fully covered as billed, or if you did not know and could not reasonably have been expected to know that we would not pay for this level of service, or if you notified the patient in writing in advance that we would not pay for this level of service and he/she agreed in writing to pay, ask us to review your claim within 120 days of the date of this notice. If you do not request a appeal, we will, upon application from the patient, reimburse him/her for the amount you have collected from him/her in excess of any deductible and coinsurance amounts. We will recover the reimbursement from you as an overpayment";
                        break;

                    case "M26":
                        str = "Payment has been adjusted because the information furnished does not substantiate the need for this level of service. If you have collected any amount from the patient for this level of service /any amount that exceeds the limiting charge for the less extensive service, the law requires you to refund that amount to the patient within 30 days of receiving this notice. The requirements for refund are in 1824(I) of the Social Security Act and 42CFR411.408. The section specifies that physicians who knowingly and willfully fail to make appropriate refunds may be subject to civil monetary penalties and/or exclusion from the program. If you have any questions about this notice, please contact this office";
                        break;

                    case "M27":
                        str = "The patient has been relieved of liability of payment of these items and services under the limitation of liability provision of the law. You, the provider, are ultimately liable for the patient's waived charges, including any charges for coinsurance, since the items or services were not reasonable and necessary or constituted custodial care, and you knew or could reasonably have been expected to know, that they were not covered. You may appeal this determination. You may ask for an appeal regarding both the coverage determination and the issue of whether you exercised due care. The appeal request must be filed within 120 days of the date you receive this notice. You must make the request through this office";
                        break;

                    case "M28":
                        str = "This does not qualify for payment under Part B when Part A coverage is exhausted or not otherwise available";
                        break;

                    case "M29":
                        str = "Missing operative report";
                        break;

                    case "M30":
                        str = "Missing pathology report";
                        break;

                    case "M31":
                        str = "Missing radiology report";
                        break;

                    case "M32":
                        str = "This is a conditional payment made pending a decision on this service by the patient's primary payer. This payment may be subject to refund upon your receipt of any additional payment for this service from another payer. You must contact this office immediately upon receipt of an additional payment for this service";
                        break;

                    case "M33":
                        str = "Missing/incomplete/invalid UPIN for the ordering/referring/performing provider";
                        break;

                    case "M34":
                        str = "Claim lacks the CLIA certification number";
                        break;

                    case "M35":
                        str = "Missing/incomplete/invalid pre-operative photos or visual field results";
                        break;

                    case "M36":
                        str = "This is the 11th rental month. We cannot pay for this until you indicate that the patient has been given the option of changing the rental to a purchase";
                        break;

                    case "M37":
                        str = "Service not covered when the patient is under age 35";
                        break;

                    case "M38":
                        str = "The patient is liable for the charges for this service as you informed the patient in writing before the service was furnished that we would not pay for it, and the patient agreed to pay";
                        break;

                    case "M39":
                        str = "The patient is not liable for payment for this service as the advance notice of non-coverage you provided the patient did not comply with program requirements";
                        break;

                    case "M40":
                        str = "Claim must be assigned and must be filed by the practitioner's employer";
                        break;

                    case "M41":
                        str = "We do not pay for this as the patient has no legal obligation to pay for this";
                        break;

                    case "M42":
                        str = "The medical necessity form must be personally signed by the attending physician";
                        break;

                    case "M43":
                        str = "Payment for this service previously issued to you or another provider by another carrier/intermediary";
                        break;

                    case "M44":
                        str = "Missing/incomplete/invalid condition code";
                        break;

                    case "M45":
                        str = "Missing/incomplete/invalid occurrence code(s)";
                        break;

                    case "M46":
                        str = "Missing/incomplete/invalid occurrence span code(s)";
                        break;

                    case "M47":
                        str = "Missing/incomplete/invalid internal or document control number";
                        break;

                    case "M48":
                        str = "Payment for services furnished to hospital inpatients (other than professional services of physicians) can only be made to the hospital. You must request payment from the hospital rather than the patient for this service";
                        break;

                    case "M49":
                        str = "Missing/incomplete/invalid value code(s) or amount(s)";
                        break;

                    case "M50":
                        str = "Missing/incomplete/invalid revenue code(s)";
                        break;

                    case "M51":
                        str = "Missing/incomplete/invalid procedure code(s)";
                        break;

                    case "M52":
                        str = "Missing/incomplete/invalid from date(s) of service";
                        break;

                    case "M53":
                        str = "Missing/incomplete/invalid days or units of service";
                        break;

                    case "M54":
                        str = "Missing/incomplete/invalid total charges";
                        break;

                    case "M55":
                        str = "We do not pay for self-administered anti-emetic drugs that are not administered with a covered oral anti-cancer drug";
                        break;

                    case "M56":
                        str = "Missing/incomplete/invalid payer identifier";
                        break;

                    case "M57":
                        str = "Missing/incomplete/invalid provider identifier";
                        break;

                    case "M58":
                        str = "Missing/incomplete/invalid claim information. Resubmit claim after corrections";
                        break;

                    case "M59":
                        str = "Missing/incomplete/invalid  To  date(s) of service";
                        break;

                    case "M60":
                        str = "Missing Certificate of Medical Necessity";
                        break;

                    case "M61":
                        str = "We cannot pay for this as the approval period for the FDA clinical trial has expired";
                        break;

                    case "M62":
                        str = "Missing/incomplete/invalid treatment authorization code";
                        break;

                    case "M63":
                        str = "We do not pay for more than one of these on the same day";
                        break;

                    case "M64":
                        str = "Missing/incomplete/invalid other diagnosis";
                        break;

                    case "M65":
                        str = "One interpreting physician charge can be submitted per claim when a purchased diagnostic test is indicated. Please submit a separate claim for each interpreting physician";
                        break;

                    case "M66":
                        str = "Our records indicate that you billed diagnostic tests subject to price limitations and the procedure code submitted includes a professional component. Only the technical component is subject to price limitations. Please submit the technical and professional components of this service as separate line items";
                        break;

                    case "M67":
                        str = "Missing/incomplete/invalid other procedure code(s)";
                        break;

                    case "M68":
                        str = "Missing/incomplete/invalid attending, ordering, rendering, supervising or referring physician identification";
                        break;

                    case "M69":
                        str = "Paid at the regular rate as you did not submit documentation to justify the modified procedure code";
                        break;

                    case "M70":
                        str = "NDC code submitted for this service was translated to a HCPCS code for processing, but please continue to submit the NDC on future claims for this item";
                        break;

                    case "M71":
                        str = "Total payment reduced due to overlap of tests billed";
                        break;

                    case "M72":
                        str = "Did not enter full 8-digit date (MM/DD/CCYY)";
                        break;

                    case "M73":
                        str = "The HPSA/Physician Scarcity bonus can only be paid on the professional component of this service. Rebill as separate professional and technical components";
                        break;

                    case "M74":
                        str = "This service does not qualify for a HPSA/Physician Scarcity bonus payment";
                        break;

                    case "M75":
                        str = "Allowed amount adjusted. Multiple automated multichannel tests performed on the same day combined for payment";
                        break;

                    case "M76":
                        str = "Missing/incomplete/invalid diagnosis or condition";
                        break;

                    case "M77":
                        str = "Missing/incomplete/invalid place of service";
                        break;

                    case "M78":
                        str = "Missing/incomplete/invalid HCPCS modifier";
                        break;

                    case "M79":
                        str = "Missing/incomplete/invalid charge";
                        break;

                    case "M80":
                        str = "Not covered when performed during the same session/date as a previously processed service for the patient";
                        break;

                    case "M81":
                        str = "You are required to code to the highest level of specificity";
                        break;

                    case "M82":
                        str = "Service is not covered when patient is under age 50";
                        break;

                    case "M83":
                        str = "Service is not covered unless the patient is classified as at high risk";
                        break;

                    case "M84":
                        str = "Medical code sets used must be the codes in effect at the time of service";
                        break;

                    case "M85":
                        str = "Subjected to review of physician evaluation and management services";
                        break;

                    case "M86":
                        str = "Service denied because payment already made for same/similar procedure within set time frame";
                        break;

                    case "M87":
                        str = "Claim/service(s) subjected to CFO-CAP prepayment review";
                        break;

                    case "M88":
                        str = "We cannot pay for laboratory tests unless billed by the laboratory that did the work";
                        break;

                    case "M89":
                        str = "Not covered more than once under age 40";
                        break;

                    case "M90":
                        str = "Not covered more than once in a 12 month period";
                        break;

                    case "M91":
                        str = "Lab procedures with different CLIA certification numbers must be billed on separate claims";
                        break;

                    case "M92":
                        str = "Services subjected to review under the Home Health Medical Review Initiative";
                        break;

                    case "M93":
                        str = "Information supplied supports a break in therapy. A new capped rental period began with delivery of this equipment";
                        break;

                    case "M94":
                        str = "Information supplied does not support a break in therapy. A new capped rental period will not begin";
                        break;

                    case "M95":
                        str = "Services subjected to Home Health Initiative medical review/cost report audit";
                        break;

                    case "M96":
                        str = "The technical component of a service furnished to an inpatient may only be billed by that inpatient facility. You must contact the inpatient facility for technical component reimbursement. If not already billed, you should bill us for the professional component only";
                        break;

                    case "M97":
                        str = "Not paid to practitioner when provided to patient in this place of service. Payment included in the reimbursement issued the facility";
                        break;

                    case "M98":
                        str = "Begin to report the Universal Product Number on claims for items of this type. We will soon begin to deny payment for items of this type if billed without the correct UPN";
                        break;

                    case "M99":
                        str = "Missing/incomplete/invalid Universal Product Number/Serial Number";
                        break;

                    case "M100":
                        str = "We do not pay for an oral anti-emetic drug that is not administered for use immediately before, at, or within 48 hours of administration of a covered chemotherapy drug";
                        break;

                    case "M101":
                        str = "Begin to report a G1-G5 modifier with this HCPCS. We will soon begin to deny payment for this service if billed without a G1-G5 modifier";
                        break;

                    case "M102":
                        str = "Service not performed on equipment approved by the FDA for this purpose";
                        break;

                    case "M103":
                        str = "Information supplied supports a break in therapy. However, the medical information we have for this patient does not support the need for this item as billed. We have approved payment for this item at a reduced level, and a new capped rental period will begin with the delivery of this equipment";
                        break;

                    case "M104":
                        str = "Information supplied supports a break in therapy. A new capped rental period will begin with delivery of the equipment. This is the maximum approved under the fee schedule for this item or service";
                        break;

                    case "M105":
                        str = "Information supplied does not support a break in therapy. The medical information we have for this patient does not support the need for this item as billed. We have approved payment for this item at a reduced level, and a new capped rental period will not begin";
                        break;

                    case "M106":
                        str = "Information supplied does not support a break in therapy. A new capped rental period will not begin. This is the maximum approved under the fee schedule for this item or service";
                        break;

                    case "M107":
                        str = "Payment reduced as 90-day rolling average hematocrit for ESRD patient exceeded 36.5%";
                        break;

                    case "M108":
                        str = "Missing/incomplete/invalid provider identifier for the provider who interpreted the diagnostic test";
                        break;

                    case "M109":
                        str = "We have provided you with a bundled payment for a teleconsultation. You must send 25 percent of the teleconsultation payment to the referring practitioner";
                        break;

                    case "M110":
                        str = "Missing/incomplete/invalid provider identifier for the provider from whom you purchased interpretation services";
                        break;

                    case "M111":
                        str = "We do not pay for chiropractic manipulative treatment when the patient refuses to have an x-ray taken";
                        break;

                    case "M112":
                        str = "The approved amount is based on the maximum allowance for this item under the DMEPOS Competitive Bidding Demonstration";
                        break;

                    case "M113":
                        str = "Our records indicate that this patient began using this service(s) prior to the current round of the DMEPOS Competitive Bidding Demonstration. Therefore, the approved amount is based on the allowance in effect prior to this round of bidding for this item";
                        break;

                    case "M114":
                        str = "This service was processed in accordance with rules and guidelines under the Competitive Bidding Demonstration Project. If you would like more information regarding this project, you may phone 1-888-289-0710";
                        break;

                    case "M115":
                        str = "This item is denied when provided to this patient by a non-demonstration supplier";
                        break;

                    case "M116":
                        str = "Paid under the Competitive Bidding Demonstration project. Project is ending, and future services may not be paid under this project";
                        break;

                    case "M117":
                        str = "Not covered unless submitted via electronic claim";
                        break;

                    case "M118":
                        str = "Letter to follow containing further information";
                        break;

                    case "M119":
                        str = "Missing/incomplete/invalid/ deactivated/withdrawn National Drug Code (NDC)";
                        break;

                    case "M120":
                        str = "Missing/incomplete/invalid provider identifier for the substituting physician who furnished the service(s) under a reciprocal billing or locum tenens arrangement";
                        break;

                    case "M121":
                        str = "We pay for this service only when performed with a covered cryosurgical ablation";
                        break;

                    case "M122":
                        str = "Missing/incomplete/invalid level of subluxation";
                        break;

                    case "M123":
                        str = "Missing/incomplete/invalid name, strength, or dosage of the drug furnished";
                        break;

                    case "M124":
                        str = "Missing indication of whether the patient owns the equipment that requires the part or supply";
                        break;

                    case "M125":
                        str = "Missing/incomplete/invalid information on the period of time for which the service/supply/equipment will be needed";
                        break;

                    case "M126":
                        str = "Missing/incomplete/invalid individual lab codes included in the test";
                        break;

                    case "M127":
                        str = "Missing patient medical record for this service";
                        break;

                    case "M128":
                        str = "Missing/incomplete/invalid date of the patient’s last physician visit";
                        break;

                    case "M129":
                        str = "Missing/incomplete/invalid indicator of x-ray availability for review";
                        break;

                    case "M130":
                        str = "Missing invoice or statement certifying the actual cost of the lens, less discounts, and/or the type of intraocular lens used";
                        break;

                    case "M131":
                        str = "Missing physician financial relationship form";
                        break;

                    case "M132":
                        str = "Missing pacemaker registration form";
                        break;

                    case "M133":
                        str = "Claim did not identify who performed the purchased diagnostic test or the amount you were charged for the test";
                        break;

                    case "M134":
                        str = "Performed by a facility/supplier in which the provider has a financial interest";
                        break;

                    case "M135":
                        str = "Missing/incomplete/invalid plan of treatment";
                        break;

                    case "M136":
                        str = "Missing/incomplete/invalid indication that the service was supervised or evaluated by a physician";
                        break;

                    case "M137":
                        str = "Part B coinsurance under a demonstration project";
                        break;

                    case "M138":
                        str = "Patient identified as a demonstration participant but the patient was not enrolled in the demonstration at the time services were rendered. Coverage is limited to demonstration participants";
                        break;

                    case "M139":
                        str = "Denied services exceed the coverage limit for the demonstration";
                        break;

                    case "M140":
                        str = "Service not covered until after the patient’s 50th birthday, i.e., no coverage prior to the day after the 50th birthday";
                        break;

                    case "M141":
                        str = "Missing physician certified plan of care";
                        break;

                    case "M142":
                        str = "Missing American Diabetes Association Certificate of Recognition";
                        break;

                    case "M143":
                        str = "We have no record that you are licensed to dispensed drugs in the State where located";
                        break;

                    case "M144":
                        str = "Pre-/post-operative care payment is included in the allowance for the surgery/procedure";
                        break;

                    case "MA01":
                        str = "If you do not agree with what we approved for these services, you may appeal our decision. To make sure that we are fair to you, we require another individual that did not process your initial claim to conduct the appeal. However, in order to be eligible for an appeal, you must write to us within 120 days of the date you received this notice, unless you have a good reason for being late";
                        break;

                    case "MA02":
                        str = "If you do not agree with this determination, you have the right to appeal. You must file a written request for an appeal within 180 days of the date you receive this notice. Decisions made by a Quality Improvement Organization (QIO) must be appealed to that QIO within 60 days";
                        break;

                    case "MA03":
                        str = "If you do not agree with the approved amounts and $100 or more is in dispute (less deductible and coinsurance), you may ask for a hearing within six months of the date of this notice. To meet the $100, you may combine amounts on other claims that have been denied, including reopened appeals if you received a revised decision. You must appeal each claim on time";
                        break;

                    case "MA04":
                        str = "Secondary payment cannot be considered without the identity of or payment information from the primary payer. The information was either not reported or was illegible";
                        break;

                    case "MA05":
                        str = "Incorrect admission date patient status or type of bill entry on claim";
                        break;

                    case "MA06":
                        str = "Missing/incomplete/invalid beginning and/or ending date(s)";
                        break;

                    case "MA07":
                        str = "The claim information has also been forwarded to Medicaid for review";
                        break;

                    case "MA08":
                        str = "You should also submit this claim to the patient's other insurer for potential payment of supplemental benefits. We did not forward the claim information as the supplemental coverage is not with a Medigap plan, or you do not participate in Medicare";
                        break;

                    case "MA09":
                        str = "Claim submitted as unassigned but processed as assigned. You agreed to accept assignment for all claims";
                        break;

                    case "MA10":
                        str = "The patient's payment was in excess of the amount owed. You must refund the overpayment to the patient";
                        break;

                    case "MA11":
                        str = "Payment is being issued on a conditional basis. If no-fault insurance, liability insurance, Workers' Compensation, Department of Veterans Affairs, or a group health plan for employees and dependents also covers this claim, a refund may be due us. Please contact us if the patient is covered by any of these sources";
                        break;

                    case "MA12":
                        str = "You have not established that you have the right under the law to bill for services furnished by the person(s) that furnished this (these) service(s)";
                        break;

                    case "MA13":
                        str = "You may be subject to penalties if you bill the patient for amounts not reported with the PR (patient responsibility) group code";
                        break;

                    case "MA14":
                        str = "Patient is a member of an employer-sponsored prepaid health plan. Services from outside that health plan are not covered. However, as you were not previously notified of this, we are paying this time. In the future, we will not pay you for non-plan services";
                        break;

                    case "MA15":
                        str = "Your claim has been separated to expedite handling. You will receive a separate notice for the other services reported";
                        break;

                    case "MA16":
                        str = "The patient is covered by the Black Lung Program. Send this claim to the Department of Labor, Federal Black Lung Program, P.O. Box 828, Lanham-Seabrook MD 20703";
                        break;

                    case "MA17":
                        str = "We are the primary payer and have paid at the primary rate. You must contact the patient's other insurer to refund any excess it may have paid due to its erroneous primary payment";
                        break;

                    case "MA18":
                        str = "The claim information is also being forwarded to the patient's supplemental insurer. Send any questions regarding supplemental benefits to them";
                        break;

                    case "MA19":
                        str = "Information was not sent to the Medigap insurer due to incorrect/invalid information you submitted concerning that insurer. Please verify your information and submit your secondary claim directly to that insurer";
                        break;

                    case "MA20":
                        str = "Skilled Nursing Facility (SNF) stay not covered when care is primarily related to the use of an urethral catheter for convenience or the control of incontinence";
                        break;

                    case "MA21":
                        str = "SSA records indicate mismatch with name and gender";
                        break;

                    case "MA22":
                        str = "Payment of less than $1.00 suppressed";
                        break;

                    case "MA23":
                        str = "Demand bill approved as result of medical review";
                        break;

                    case "MA24":
                        str = "Christian Science Sanitarium/ Skilled Nursing Facility (SNF) bill in the same benefit period";
                        break;

                    case "MA25":
                        str = "A patient may not elect to change a hospice provider more than once in a benefit period";
                        break;

                    case "MA26":
                        str = "Our records indicate that you were previously informed of this rule";
                        break;

                    case "MA27":
                        str = "Missing/incomplete/invalid entitlement number or name shown on the claim";
                        break;

                    case "MA28":
                        str = "Receipt of this notice by a physician or supplier who did not accept assignment is for information only and does not make the physician or supplier a party to the determination. No additional rights to appeal this decision, above those rights already provided for by regulation/instruction, are conferred by receipt of this notice";
                        break;

                    case "MA29":
                        str = "Missing/incomplete/invalid provider name, city, state, or zip code";
                        break;

                    case "MA30":
                        str = "Missing/incomplete/invalid type of bill";
                        break;

                    case "MA31":
                        str = "Missing/incomplete/invalid beginning and ending dates of the period billed";
                        break;

                    case "MA32":
                        str = "Missing/incomplete/invalid number of covered days during the billing period";
                        break;

                    case "MA33":
                        str = "Missing/incomplete/invalid noncovered days during the billing period";
                        break;

                    case "MA34":
                        str = "Missing/incomplete/invalid number of coinsurance days during the billing period";
                        break;

                    case "MA35":
                        str = "Missing/incomplete/invalid number of lifetime reserve days";
                        break;

                    case "MA36":
                        str = "Missing/incomplete/invalid patient name";
                        break;

                    case "MA37":
                        str = "Missing/incomplete/invalid patient's address";
                        break;

                    case "MA38":
                        str = "Missing/incomplete/invalid birth date";
                        break;

                    case "MA39":
                        str = "Missing/incomplete/invalid gender";
                        break;

                    case "MA40":
                        str = "Missing/incomplete/invalid admission date";
                        break;

                    case "MA41":
                        str = "Missing/incomplete/invalid admission type";
                        break;

                    case "MA42":
                        str = "Missing/incomplete/invalid admission source";
                        break;

                    case "MA43":
                        str = "Missing/incomplete/invalid patient status";
                        break;

                    case "MA44":
                        str = "No appeal rights. Adjudicative decision based on law";
                        break;

                    case "MA45":
                        str = "As previously advised, a portion or all of your payment is being held in a special account";
                        break;

                    case "MA46":
                        str = "The new information was considered, however, additional payment cannot be issued. Please review the information listed for the explanation";
                        break;

                    case "MA47":
                        str = "Our records show you have opted out of Medicare, agreeing with the patient not to bill Medicare for services/tests/supplies furnished. As result, we cannot pay this claim. The patient is responsible for payment";
                        break;

                    case "MA48":
                        str = "Missing/incomplete/invalid name or address of responsible party or primary payer";
                        break;

                    case "MA49":
                        str = "Missing/incomplete/invalid six-digit provider identifier for home health agency or hospice for physician(s) performing care plan oversight services";
                        break;

                    case "MA50":
                        str = "Missing/incomplete/invalid Investigational Device Exemption number for FDA-approved clinical trial services";
                        break;

                    case "MA51":
                        str = "Missing/incomplete/invalid CLIA certification number for laboratory services billed by physician office laboratory";
                        break;

                    case "MA52":
                        str = "Missing/incomplete/invalid date";
                        break;

                    case "MA53":
                        str = "Missing/incomplete/invalid Competitive Bidding Demonstration Project identification";
                        break;

                    case "MA54":
                        str = "Physician certification or election consent for hospice care not received timely";
                        break;

                    case "MA55":
                        str = "Not covered as patient received medical health care services, automatically revoking his/her election to receive religious non-medical health care services";
                        break;

                    case "MA56":
                        str = "Our records show you have opted out of Medicare, agreeing with the patient not to bill Medicare for services/tests/supplies furnished. As result, we cannot pay this claim. The patient is responsible for payment, but under Federal law, you cannot charge the patient more than the limiting charge amount";
                        break;

                    case "MA57":
                        str = "Patient submitted written request to revoke his/her election for religious non-medical health care services";
                        break;

                    case "MA58":
                        str = "Missing/incomplete/invalid release of information indicator";
                        break;

                    case "MA59":
                        str = "The patient overpaid you for these services. You must issue the patient a refund within 30 days for the difference between his/her payment and the total amount shown as patient responsibility on this notice";
                        break;

                    case "MA60":
                        str = "Missing/incomplete/invalid patient relationship to insured";
                        break;

                    case "MA61":
                        str = "Missing/incomplete/invalid social security number or health insurance claim number";
                        break;

                    case "MA62":
                        str = "Telephone review decision";
                        break;

                    case "MA63":
                        str = "Missing/incomplete/invalid principal diagnosis";
                        break;

                    case "MA64":
                        str = "Our records indicate that we should be the third payer for this claim. We cannot process this claim until we have received payment information from the primary and secondary payers";
                        break;

                    case "MA65":
                        str = "Missing/incomplete/invalid admitting diagnosis";
                        break;

                    case "MA66":
                        str = "Missing/incomplete/invalid principal procedure code";
                        break;

                    case "MA67":
                        str = "Correction to a prior claim";
                        break;

                    case "MA68":
                        str = "We did not crossover this claim because the secondary insurance information on the claim was incomplete. Please supply complete information or use the PLANID of the insurer to assure correct and timely routing of the claim";
                        break;

                    case "MA69":
                        str = "Missing/incomplete/invalid remarks";
                        break;

                    case "MA70":
                        str = "Missing/incomplete/invalid provider representative signature";
                        break;

                    case "MA71":
                        str = "Missing/incomplete/invalid provider representative signature date";
                        break;

                    case "MA72":
                        str = "The patient overpaid you for these assigned services. You must issue the patient a refund within 30 days for the difference between his/her payment to you and the total of the amount shown as patient responsibility and as paid to the patient on this notice";
                        break;

                    case "MA73":
                        str = "Informational remittance associated with a Medicare demonstration. No payment issued under fee-for-service Medicare as patient has elected managed care";
                        break;

                    case "MA74":
                        str = "This payment replaces an earlier payment for this claim that was either lost, damaged or returned";
                        break;

                    case "MA75":
                        str = "Missing/incomplete/invalid patient or authorized representative signature";
                        break;

                    case "MA76":
                        str = "Missing/incomplete/invalid provider identifier for home health agency or hospice when physician is performing care plan oversight services";
                        break;

                    case "MA77":
                        str = "The patient overpaid you. You must issue the patient a refund within 30 days for the difference between the patient’s payment less the total of our and other payer payments and the amount shown as patient responsibility on this notice";
                        break;

                    case "MA78":
                        str = "The patient overpaid you. You must issue the patient a refund within 30 days for the difference between our allowed amount total and the amount paid by the patient";
                        break;

                    case "MA79":
                        str = "Billed in excess of interim rate";
                        break;

                    case "MA80":
                        str = "Informational notice. No payment issued for this claim with this notice. Payment issued to the hospital by its intermediary for all services for this encounter under a demonstration project";
                        break;

                    case "MA81":
                        str = "Missing/incomplete/invalid provider/supplier signature";
                        break;

                    case "MA82":
                        str = "Missing/incomplete/invalid provider/supplier billing number/identifier or billing name, address, city, state, zip code, or phone number";
                        break;

                    case "MA83":
                        str = "Did not indicate whether we are the primary or secondary payer";
                        break;

                    case "MA84":
                        str = "Patient identified as participating in the National Emphysema Treatment Trial but our records indicate that this patient is either not a participant, or has not yet been approved for this phase of the study. Contact Johns Hopkins University, the study coordinator, to resolve if there was a discrepancy";
                        break;

                    case "MA85":
                        str = "Our records indicate that a primary payer exists (other than ourselves); however, you did not complete or enter accurately the insurance plan/group/program name or identification number. Enter the PlanID when effective";
                        break;

                    case "MA86":
                        str = "Missing/incomplete/invalid group or policy number of the insured for the primary coverage";
                        break;

                    case "MA87":
                        str = "Missing/incomplete/invalid insured's name for the primary payer";
                        break;

                    case "MA88":
                        str = "Missing/incomplete/invalid insured's address and/or telephone number for the primary payer";
                        break;

                    case "MA89":
                        str = "Missing/incomplete/invalid patient's relationship to the insured for the primary payer";
                        break;

                    case "MA90":
                        str = "Missing/incomplete/invalid employment status code for the primary insured";
                        break;

                    case "MA91":
                        str = "This determination is the result of the appeal you filed";
                        break;

                    case "MA92":
                        str = "Missing plan information for other insurance";
                        break;

                    case "MA93":
                        str = "Non-PIP (Periodic Interim Payment) claim";
                        break;

                    case "MA94":
                        str = "Did not enter the statement Attending physician not hospice employee on the claim form to certify that the rendering physician is not an employee of the hospice.  Note: (Reactivated 4/1/04, Modified 8/1/05)";
                        break;

                    case "MA95":
                        str = "De-activate and refer to M51";
                        break;

                    case "MA96":
                        str = "Claim rejected. Coded as a Medicare Managed Care Demonstration but patient is not enrolled in a Medicare managed care plan";
                        break;

                    case "MA97":
                        str = "Missing/incomplete/invalid Medicare Managed Care Demonstration contract number";
                        break;

                    case "MA98":
                        str = "Claim Rejected. Does not contain the correct Medicare Managed Care Demonstration contract number for this beneficiary";
                        break;

                    case "MA99":
                        str = "Missing/incomplete/invalid Medigap information";
                        break;

                    case "MA100":
                        str = "Missing/incomplete/invalid date of current illness or symptoms";
                        break;

                    case "MA101":
                        str = "A Skilled Nursing Facility (SNF) is responsible for payment of outside providers who furnish these services/supplies to residents";
                        break;

                    case "MA102":
                        str = "Missing/incomplete/invalid name or provider identifier for the rendering/referring/ ordering/ supervising provider";
                        break;

                    case "MA103":
                        str = "Hemophilia Add On";
                        break;

                    case "MA104":
                        str = "Missing/incomplete/invalid date the patient was last seen or the provider identifier of the attending physician";
                        break;

                    case "MA105":
                        str = "Missing/incomplete/invalid provider number for this place of service";
                        break;

                    case "MA106":
                        str = "PIP (Periodic Interim Payment) claim";
                        break;

                    case "MA107":
                        str = "Paper claim contains more than three separate data items in field 19";
                        break;

                    case "MA108":
                        str = "Paper claim contains more than one data item in field 23";
                        break;

                    case "MA109":
                        str = "Claim processed in accordance with ambulatory surgical guidelines";
                        break;

                    case "MA110":
                        str = "Missing/incomplete/invalid information on whether the diagnostic test(s) were performed by an outside entity or if no purchased tests are included on the claim";
                        break;

                    case "MA111":
                        str = "Missing/incomplete/invalid purchase price of the test(s) and/or the performing laboratory's name and address";
                        break;

                    case "MA112":
                        str = "Missing/incomplete/invalid group practice information";
                        break;

                    case "MA113":
                        str = "Incomplete/invalid taxpayer identification number (TIN) submitted by you per the Internal Revenue Service. Your claims cannot be processed without your correct TIN, and you may not bill the patient pending correction of your TIN. There are no appeal rights for unprocessable claims, but you may resubmit this claim after you have notified this office of your correct TIN";
                        break;

                    case "MA114":
                        str = "Missing/incomplete/invalid information on where the services were furnished";
                        break;

                    case "MA115":
                        str = "Missing/incomplete/invalid physical location (name and address, or PIN) where the service(s) were rendered in a Health Professional Shortage Area (HPSA)";
                        break;

                    case "MA116":
                        str = "Did not complete the statement Homebound on the claim to validate whether laboratory services were performed at home or in an institution";
                        break;

                    case "MA117":
                        str = "This claim has been assessed a $1.00 user fee";
                        break;

                    case "MA118":
                        str = "Coinsurance and/or deductible amounts apply to a claim for services or supplies furnished to a Medicare-eligible veteran through a facility of the Department of Veterans Affairs. No Medicare payment issued";
                        break;

                    case "MA119":
                        str = "Provider level adjustment for late claim filing applies to this claim";
                        break;

                    case "MA120":
                        str = "Missing/incomplete/invalid CLIA certification number";
                        break;

                    case "MA121":
                        str = "Missing/incomplete/invalid x-ray date";
                        break;

                    case "MA122":
                        str = "Missing/incomplete/invalid initial treatment date";
                        break;

                    case "MA123":
                        str = "Your center was not selected to participate in this study, therefore, we cannot pay for these services";
                        break;

                    case "MA124":
                        str = "Processed for IME only";
                        break;

                    case "MA125":
                        str = "Per legislation governing this program, payment constitutes payment in full";
                        break;

                    case "MA126":
                        str = "Pancreas transplant not covered unless kidney transplant performed";
                        break;

                    case "MA127":
                        str = "Reserved for future use";
                        break;

                    case "MA128":
                        str = "Missing/incomplete/invalid FDA approval number";
                        break;

                    case "MA129":
                        str = "This provider was not certified for this procedure on this date of service";
                        break;

                    case "MA130":
                        str = "Your claim contains incomplete and/or invalid information, and no appeal rights are afforded because the claim is unprocessable. Please submit a new claim with the complete/correct information";
                        break;

                    case "MA131":
                        str = "Physician already paid for services in conjunction with this demonstration claim. You must have the physician withdraw that claim and refund the payment before we can process your claim";
                        break;

                    case "MA132":
                        str = "Adjustment to the pre-demonstration rate";
                        break;

                    case "MA133":
                        str = "Claim overlaps inpatient stay. Rebill only those services rendered outside the inpatient stay";
                        break;

                    case "MA134":
                        str = "Missing/incomplete/invalid provider number of the facility where the patient resides";
                        break;

                    case "N1":
                        str = "You may appeal this decision in writing within the required time limits following receipt of this notice by following the instructions included in your contract or plan benefit documents";
                        break;

                    case "N2":
                        str = "This allowance has been made in accordance with the most appropriate course of treatment provision of the plan";
                        break;

                    case "N3":
                        str = "Missing consent form";
                        break;

                    case "N4":
                        str = "Missing/incomplete/invalid prior insurance carrier EOB";
                        break;

                    case "N5":
                        str = "EOB received from previous payer. Claim not on file";
                        break;

                    case "N6":
                        str = "Under FEHB law (U.S.C. 8904(b)), we cannot pay more for covered care than the amount Medicare would have allowed if the patient were enrolled in Medicare Part A and/or Medicare Part B";
                        break;

                    case "N7":
                        str = "Processing of this claim/service has included consideration under Major Medical provisions";
                        break;

                    case "N8":
                        str = "Crossover claim denied by previous payer and complete claim data not forwarded. Resubmit this claim to this payer to provide adequate data for adjudication";
                        break;

                    case "N9":
                        str = "Adjustment represents the estimated amount a previous payer may pay";
                        break;

                    case "N10":
                        str = "Claim/service adjusted based on the findings of a review organization/professional consult/manual adjudication/medical or dental advisor";
                        break;

                    case "N11":
                        str = "Denial reversed because of medical review";
                        break;

                    case "N12":
                        str = "Policy provides coverage supplemental to Medicare. As member does not appear to be enrolled in Medicare Part B, the member is responsible for payment of the portion of the charge that would have been covered by Medicare";
                        break;

                    case "N13":
                        str = "Payment based on professional/technical component modifier(s)";
                        break;

                    case "N14":
                        str = "Payment based on a contractual amount or agreement, fee schedule, or maximum allowable amount";
                        break;

                    case "N15":
                        str = "Services for a newborn must be billed separately";
                        break;

                    case "N16":
                        str = "Family/member Out-of-Pocket maximum has been met. Payment based on a higher percentage";
                        break;

                    case "N17":
                        str = "Per admission deductible";
                        break;

                    case "N18":
                        str = "Payment based on the Medicare allowed amount";
                        break;

                    case "N19":
                        str = "Procedure code incidental to primary procedure";
                        break;

                    case "N20":
                        str = "Service not payable with other service rendered on the same date";
                        break;

                    case "N21":
                        str = "Your line item has been separated into multiple lines to expedite handling";
                        break;

                    case "N22":
                        str = "This procedure code was added/changed because it more accurately describes the services rendered";
                        break;

                    case "N23":
                        str = "Patient liability may be affected due to coordination of benefits with other carriers and/or maximum benefit provisions";
                        break;

                    case "N24":
                        str = "Missing/incomplete/invalid Electronic Funds Transfer (EFT) banking information";
                        break;

                    case "N25":
                        str = "This company has been contracted by your benefit plan to provide administrative claims payment services only. This company does not assume financial risk or obligation with respect to claims processed on behalf of your benefit plan";
                        break;

                    case "N26":
                        str = "Missing itemized bill";
                        break;

                    case "N27":
                        str = "Missing/incomplete/invalid treatment number";
                        break;

                    case "N28":
                        str = "Consent form requirements not fulfilled";
                        break;

                    case "N29":
                        str = "Missing documentation/orders/notes/summary/report/chart";
                        break;

                    case "N30":
                        str = "Patient ineligible for this service";
                        break;

                    case "N31":
                        str = "Missing/incomplete/invalid prescribing provider identifier";
                        break;

                    case "N32":
                        str = "Claim must be submitted by the provider who rendered the service";
                        break;

                    case "N33":
                        str = "No record of health check prior to initiation of treatment";
                        break;

                    case "N34":
                        str = "Incorrect claim form/format for this service";
                        break;

                    case "N35":
                        str = "Program integrity/utilization review decision";
                        break;

                    case "N36":
                        str = "Claim must meet primary payer’s processing requirements before we can consider payment";
                        break;

                    case "N37":
                        str = "Missing/incomplete/invalid tooth number/letter";
                        break;

                    case "N38":
                        str = "Missing/incomplete/invalid place of service";
                        break;

                    case "N39":
                        str = "Procedure code is not compatible with tooth number/letter";
                        break;

                    case "N40":
                        str = "Missing x-ray";
                        break;

                    case "N41":
                        str = "Authorization request denied";
                        break;

                    case "N42":
                        str = "No record of mental health assessment";
                        break;

                    case "N43":
                        str = "Bed hold or leave days exceeded";
                        break;

                    case "N44":
                        str = "Payer’s share of regulatory surcharges, assessments, allowances or health care-related taxes paid directly to the regulatory authority";
                        break;

                    case "N45":
                        str = "Payment based on authorized amount";
                        break;

                    case "N46":
                        str = "Missing/incomplete/invalid admission hour";
                        break;

                    case "N47":
                        str = "Claim conflicts with another inpatient stay";
                        break;

                    case "N48":
                        str = "Claim information does not agree with information received from other insurance carrier";
                        break;

                    case "N49":
                        str = "Court ordered coverage information needs validation";
                        break;

                    case "N50":
                        str = "Missing/incomplete/invalid discharge information";
                        break;

                    case "N51":
                        str = "Electronic interchange agreement not on file for provider/submitter";
                        break;

                    case "N52":
                        str = "Patient not enrolled in the billing provider's managed care plan on the date of service";
                        break;

                    case "N53":
                        str = "Missing/incomplete/invalid point of pick-up address";
                        break;

                    case "N54":
                        str = "Claim information is inconsistent with pre-certified/authorized services";
                        break;

                    case "N55":
                        str = "Procedures for billing with group/referring/performing providers were not followed";
                        break;

                    case "N56":
                        str = "Procedure code billed is not correct/valid for the services billed or the date of service billed";
                        break;

                    case "N57":
                        str = "Missing/incomplete/invalid prescribing date";
                        break;

                    case "N58":
                        str = "Missing/incomplete/invalid patient liability amount";
                        break;

                    case "N59":
                        str = "Please refer to your provider manual for additional program and provider information";
                        break;

                    case "N60":
                        str = "A valid NDC is required for payment of drug claims effective October 02";
                        break;

                    case "N61":
                        str = "Rebill services on separate claims";
                        break;

                    case "N62":
                        str = "Inpatient admission spans multiple rate periods. Resubmit separate claims";
                        break;

                    case "N63":
                        str = "Rebill services on separate claim lines";
                        break;

                    case "N64":
                        str = "The from and to dates must be different";
                        break;

                    case "N65":
                        str = "Procedure code or procedure rate count cannot be determined, or was not on file, for the date of service/provider";
                        break;

                    case "N66":
                        str = "Missing/incomplete/invalid documentation";
                        break;

                    case "N67":
                        str = "Professional provider services not paid separately. Included in facility payment under a demonstration project. Apply to that facility for payment, or resubmit your claim if: the facility notifies you the patient was excluded from this demonstration; or if you furnished these services in another location on the date of the patient’s admission or discharge from a demonstration hospital. If services were furnished in a facility not involved in the demonstration on the same date the patient was discharged from or admitted to a demonstration facility, you must report the provider ID number for the non-demonstration facility on the new claim";
                        break;

                    case "N68":
                        str = "Prior payment being cancelled as we were subsequently notified this patient was covered by a demonstration project in this site of service. Professional services were included in the payment made to the facility. You must contact the facility for your payment. Prior payment made to you by the patient or another insurer for this claim must be refunded to the payer within 30 days";
                        break;

                    case "N69":
                        str = "PPS (Prospective Payment System) code changed by claims processing system. Insufficient visits or therapies";
                        break;

                    case "N70":
                        str = "Home health consolidated billing and payment applies";
                        break;

                    case "N71":
                        str = "Your unassigned claim for a drug or biological, clinical diagnostic laboratory services or ambulance service was processed as an assigned claim. You are required by law to accept assignment for these types of claims";
                        break;

                    case "N72":
                        str = "PPS (Prospective Payment System) code changed by medical reviewers. Not supported by clinical records";
                        break;

                    case "N73":
                        str = "A Skilled Nursing Facility is responsible for payment of outside providers who furnish these services/supplies under arrangement to its residents";
                        break;

                    case "N74":
                        str = "Resubmit with multiple claims, each claim covering services provided in only one calendar month";
                        break;

                    case "N75":
                        str = "Missing/incomplete/invalid tooth surface information";
                        break;

                    case "N76":
                        str = "Missing/incomplete/invalid number of riders";
                        break;

                    case "N77":
                        str = "Missing/incomplete/invalid designated provider number";
                        break;

                    case "N78":
                        str = "The necessary components of the child and teen checkup (EPSDT) were not completed";
                        break;

                    case "N79":
                        str = "Service billed is not compatible with patient location information";
                        break;

                    case "N80":
                        str = "Missing/incomplete/invalid prenatal screening information";
                        break;

                    case "N81":
                        str = "Procedure billed is not compatible with tooth surface code";
                        break;

                    case "N82":
                        str = "Provider must accept insurance payment as payment in full when a third party payer contract specifies full reimbursement";
                        break;

                    case "N83":
                        str = "No appeal rights. Adjudicative decision based on the provisions of a demonstration project";
                        break;

                    case "N84":
                        str = "Further installment payments forthcoming";
                        break;

                    case "N85":
                        str = "Final installment payment";
                        break;

                    case "N86":
                        str = "A failed trial of pelvic muscle exercise training is required in order for biofeedback training for the treatment of urinary incontinence to be covered";
                        break;

                    case "N87":
                        str = "Home use of biofeedback therapy is not covered";
                        break;

                    case "N88":
                        str = "This payment is being made conditionally. An HHA episode of care notice has been filed for this patient. When a patient is treated under a HHA episode of care, consolidated billing requires that certain therapy services and supplies, such as this, be included in the HHA's payment. This payment will need to be recouped from you if we establish that the patient is concurrently receiving treatment under a HHA episode of care";
                        break;

                    case "N89":
                        str = "Payment information for this claim has been forwarded to more than one other payer, but format limitations permit only one of the secondary payers to be identified in this remittance advice";
                        break;

                    case "N90":
                        str = "Covered only when performed by the attending physician";
                        break;

                    case "N91":
                        str = "Services not included in the appeal review";
                        break;

                    case "N92":
                        str = "This facility is not certified for digital mammography";
                        break;

                    case "N93":
                        str = "A separate claim must be submitted for each place of service. Services furnished at multiple sites may not be billed in the same claim";
                        break;

                    case "N94":
                        str = "Claim/Service denied because a more specific taxonomy code is required for adjudication";
                        break;

                    case "N95":
                        str = "This provider type/provider specialty may not bill this service";
                        break;

                    case "N96":
                        str = "Patient must be refractory to conventional therapy (documented behavioral, pharmacologic and/or surgical corrective therapy) and be an appropriate surgical candidate such that implantation with anesthesia can occur";
                        break;

                    case "N97":
                        str = "Patients with stress incontinence, urinary obstruction, and specific neurologic diseases (e.g., diabetes with peripheral nerve involvement) which are associated with secondary manifestations of the above three indications are excluded";
                        break;

                    case "N98":
                        str = "Patient must have had a successful test stimulation in order to support subsequent implantation. Before a patient is eligible for permanent implantation, he/she must demonstrate a 50 percent or greater improvement through test stimulation. Improvement is measured through voiding diaries";
                        break;

                    case "N99":
                        str = "Patient must be able to demonstrate adequate ability to record voiding diary data such that clinical results of the implant procedure can be properly evaluated";
                        break;

                    case "N100":
                        str = "PPS (Prospect Payment System) code corrected during adjudication";
                        break;

                    case "N101":
                        str = "Additional information is needed in order to process this claim. Please resubmit the claim with the identification number of the provider where this service took place. The Medicare number of the site of service provider should be preceded with the letters HSP and entered into item #32 on the claim form. You may bill only one site of service provider number per claim";
                        break;

                    case "N102":
                        str = "This claim has been denied without reviewing the medical record because the requested records were not received or were not received timely";
                        break;

                    case "N103":
                        str = "Social Security records indicate that this patient was a prisoner when the service was rendered. This payer does not cover items and services furnished to an individual while they are in State or local custody under a penal authority, unless under State or local law, the individual is personally liable for the cost of his or her health care while incarcerated and the State or local government pursues such debt in the same way and with the same vigor as any other debt";
                        break;

                    case "N104":
                        str = "This claim/service is not payable under our claims jurisdiction area. You can identify the correct Medicare contractor to process this claim/service through the CMS website at www.cms.hhs.gov";
                        break;

                    case "N105":
                        str = "This is a misdirected claim/service for an RRB beneficiary. Submit paper claims to the RRB carrier: Palmetto GBA, P.O. Box 10066, Augusta, GA 30999. Call 866-749-4301 for RRB EDI information for electronic claims processing";
                        break;

                    case "N106":
                        str = "Payment for services furnished to Skilled Nursing Facility (SNF) inpatients (except for excluded services) can only be made to the SNF. You must request payment from the SNF rather than the patient for this service";
                        break;

                    case "N107":
                        str = "Services furnished to Skilled Nursing Facility (SNF) inpatients must be billed on the inpatient claim. They cannot be billed separately as outpatient services";
                        break;

                    case "N108":
                        str = "Missing/incomplete/invalid upgrade information";
                        break;

                    case "N109":
                        str = "This claim was chosen for complex review and was denied after reviewing the medical records";
                        break;

                    case "N110":
                        str = "This facility is not certified for film mammography";
                        break;

                    case "N111":
                        str = "No appeal right except duplicate claim/service issue. This service was included in a claim that has been previously billed and adjudicated";
                        break;

                    case "N112":
                        str = "This claim is excluded from your electronic remittance advice";
                        break;

                    case "N113":
                        str = "Only one initial visit is covered per physician, group practice or provider";
                        break;

                    case "N114":
                        str = "During the transition to the Ambulance Fee Schedule, payment is based on the lesser of a blended amount calculated using a percentage of the reasonable charge/cost and fee schedule amounts, or the submitted charge for the service. You will be notified yearly what the percentages for the blended payment calculation will be";
                        break;

                    case "N115":
                        str = "This decision was based on a local medical review policy (LMRP) or Local Coverage Determination (LCD).An LMRP/LCD provides a guide to assist in determining whether a particular item or service is covered. A copy of this policy is available at http://www.cms.hhs.gov/mcd, or if you do not have web access, you may contact the contractor to request a copy of the LMRP/LCD";
                        break;

                    case "N116":
                        str = "This payment is being made conditionally because the service was provided in the home, and it is possible that the patient is under a home health episode of care. When a patient is treated under a home health episode of care, consolidated billing requires that certain therapy services and supplies, such as this, be included in the home health agency’s (HHA’s) payment. This payment will need to be recouped from you if we establish that the patient is concurrently receiving treatment under an HHA episode of care";
                        break;

                    case "N117":
                        str = "This service is paid only once in a patient’s lifetime";
                        break;

                    case "N118":
                        str = "This service is not paid if billed more than once every 28 days";
                        break;

                    case "N119":
                        str = "This service is not paid if billed once every 28 days, and the patient has spent 5 or more consecutive days in any inpatient or Skilled /nursing Facility (SNF) within those 28 days";
                        break;

                    case "N120":
                        str = "Payment is subject to home health prospective payment system partial episode payment adjustment. Patient was transferred/discharged/readmitted during payment episode";
                        break;

                    case "N121":
                        str = "Medicare Part B does not pay for items or services provided by this type of practitioner for beneficiaries in a Medicare Part A covered Skilled Nursing Facility (SNF) stay";
                        break;

                    case "N122":
                        str = "Add-on code cannot be billed by itself";
                        break;

                    case "N123":
                        str = "This is a split service and represents a portion of the units from the originally submitted service";
                        break;

                    case "N124":
                        str = "Payment has been denied for the/made only for a less extensive service/item because the information furnished does not substantiate the need for the (more extensive) service/item. The patient is liable for the charges for this service/item as you informed the patient in writing before the service/item was furnished that we would not pay for it, and the patient agreed to pay";
                        break;

                    case "N125":
                        str = "Payment has been (denied for the/made only for a less extensive) service/item because the information furnished does not substantiate the need for the (more extensive) service/item. If you have collected any amount from the patient, you must refund that amount to the patient within 30 days of receiving this notice. The requirements for a refund are in \x00a71834(a)(18) of the Social Security Act (and in \x00a7\x00a71834(j)(4) and 1879(h) by cross-reference to \x00a71834(a)(18)). Section 1834(a)(18)(B) specifies that suppliers which knowingly and willfully fail to make appropriate refunds may be subject to civil money penalties and/or exclusion from the Medicare program. If you have any questions about this notice, please contact this office";
                        break;

                    case "N126":
                        str = "Social Security Records indicate that this individual has been deported. This payer does not cover items and services furnished to individuals who have been deported";
                        break;

                    case "N127":
                        str = "This is a misdirected claim/service for a United Mine Workers of America (UMWA) beneficiary. Please submit claims to them";
                        break;

                    case "N128":
                        str = "This amount represents the prior to coverage portion of the allowance";
                        break;

                    case "N129":
                        str = "This amount represents the dollar amount not eligible due to the patient's age";
                        break;

                    case "N130":
                        str = "Consult plan benefit documents for information about restrictions for this service";
                        break;

                    case "N131":
                        str = "Total payments under multiple contracts cannot exceed the allowance for this service";
                        break;

                    case "N132":
                        str = "Payments will cease for services rendered by this US Government debarred or excluded provider after the 30 day grace period as previously notified";
                        break;

                    case "N133":
                        str = "Services for predetermination and services requesting payment are being processed separately";
                        break;

                    case "N134":
                        str = "This represents your scheduled payment for this service. If treatment has been discontinued, please contact Customer Service";
                        break;

                    case "N135":
                        str = "Record fees are the patient's responsibility and limited to the specified co-payment";
                        break;

                    case "N136":
                        str = "To obtain information on the process to file an appeal in Arizona, call the Department's Consumer Assistance Office at (602) 912-8444 or (800) 325-2548";
                        break;

                    case "N137":
                        str = "The provider acting on the Member's behalf, may file an appeal with the Payer. The provider, acting on the Member's behalf, may file a complaint with the State Insurance Regulatory Authority without first filing an appeal, if the coverage decision involves an urgent condition for which care has not been rendered. The address may be obtained from the State Insurance Regulatory Authority";
                        break;

                    case "N138":
                        str = "In the event you disagree with the Dental Advisor's opinion and have additional information relative to the case, you may submit radiographs to the Dental Advisor Unit at the subscriber's dental insurance carrier for a second Independent Dental Advisor Review";
                        break;

                    case "N139":
                        str = "Under the Code of Federal Regulations, Chapter 32, Section 199.13 a non-participating provider is not an appropriate appealing party. Therefore, if you disagree with the Dental Advisor's opinion, you may appeal the determination if appointed in writing, by the beneficiary, to act as his/her representative. Should you be appointed as a representative, submit a copy of this letter, a signed statement explaining the matter in which you disagree, and any radiographs and relevant information to the subscriber's Dental insurance carrier within 90 days from the date of this letter";
                        break;

                    case "N140":
                        str = "You have not been designated as an authorized OCONUS provider therefore are not considered an appropriate appealing party. If the beneficiary has appointed you, in writing, to act as his/her representative and you disagree with the Dental Advisor's opinion, you may appeal by submitting a copy of this letter, a signed statement explaining the matter in which you disagree, and any relevant information to the subscriber's Dental insurance carrier within 90 days from the date of this letter";
                        break;

                    case "N141":
                        str = "The patient was not residing in a long-term care facility during all or part of the service dates billed";
                        break;

                    case "N142":
                        str = "The original claim was denied. Resubmit a new claim, not a replacement claim";
                        break;

                    case "N143":
                        str = "The patient was not in a hospice program during all or part of the service dates billed";
                        break;

                    case "N144":
                        str = "The rate changed during the dates of service billed";
                        break;

                    case "N145":
                        str = "Missing/incomplete/invalid provider identifier for this place of service";
                        break;

                    case "N146":
                        str = "Missing screening document";
                        break;

                    case "N147":
                        str = "Long term care case mix or per diem rate cannot be determined because the patient ID number is missing, incomplete, or invalid on the assignment request";
                        break;

                    case "N148":
                        str = "Missing/incomplete/invalid date of last menstrual period";
                        break;

                    case "N149":
                        str = "Rebill all applicable services on a single claim";
                        break;

                    case "N150":
                        str = "Missing/incomplete/invalid model number";
                        break;

                    case "N151":
                        str = "Telephone contact services will not be paid until the face-to-face contact requirement has been met";
                        break;

                    case "N152":
                        str = "Missing/incomplete/invalid replacement claim information";
                        break;

                    case "N153":
                        str = "Missing/incomplete/invalid room and board rate";
                        break;

                    case "N154":
                        str = "This payment was delayed for correction of provider's mailing address";
                        break;

                    case "N155":
                        str = "Our records do not indicate that other insurance is on file. Please submit other insurance information for our records";
                        break;

                    case "N156":
                        str = "The patient is responsible for the difference between the approved treatment and the elective treatment";
                        break;

                    case "N157":
                        str = "Transportation to/from this destination is not covered";
                        break;

                    case "N158":
                        str = "Transportation in a vehicle other than an ambulance is not covered";
                        break;

                    case "N159":
                        str = "Payment denied/reduced because mileage is not covered when the patient is not in the ambulance";
                        break;

                    case "N160":
                        str = "The patient must choose an option before a payment can be made for this procedure/ equipment/ supply/ service";
                        break;

                    case "N161":
                        str = "This drug/service/supply is covered only when the associated service is covered";
                        break;

                    case "N162":
                        str = "This is an alert. Although your claim was paid, you have billed for a test/specialty not included in your Laboratory Certification. Your failure to correct the laboratory certification information will result in a denial of payment in the near future";
                        break;

                    case "N163":
                        str = "Medical record does not support code billed per the code definition";
                        break;

                    case "N164":
                        str = "Transportation to/from this destination is not covered";
                        break;

                    case "N165":
                        str = "Transportation in a vehicle other than an ambulance is not covered";
                        break;

                    case "N166":
                        str = "Payment denied/reduced because mileage is not covered when the patient is not in the ambulance";
                        break;

                    case "N167":
                        str = "Charges exceed the post-transplant coverage limit";
                        break;

                    case "N168":
                        str = "The patient must choose an option before a payment can be made for this procedure/ equipment/ supply/ service";
                        break;

                    case "N169":
                        str = "This drug/service/supply is covered only when the associated service is covered";
                        break;

                    case "N170":
                        str = "A new/revised/renewed certificate of medical necessity is needed";
                        break;

                    case "N171":
                        str = "Payment for repair or replacement is not covered or has exceeded the purchase price";
                        break;

                    case "N172":
                        str = "The patient is not liable for the denied/adjusted charge(s) for receiving any updated service/item";
                        break;

                    case "N173":
                        str = "No qualifying hospital stay dates were provided for this episode of care";
                        break;

                    case "N174":
                        str = "This is not a covered service/procedure/ equipment/bed, however patient liability is limited to amounts shown in the adjustments under group PR";
                        break;

                    case "N175":
                        str = "Missing Review Organization Approval";
                        break;

                    
                    case "N236":
                        str = "Incomplete/invalid pathology report";
                        break;

                    case "N237":
                        str = "Incomplete/invalid patient medical record for this service";
                        break;

                    case "N238":
                        str = "Incomplete/invalid physician certified plan of care";
                        break;

                    case "N239":
                        str = "Incomplete/invalid physician financial relationship form";
                        break;

                    case "N240":
                        str = "Incomplete/invalid radiology report";
                        break;

                    case "N241":
                        str = "Incomplete/invalid Review Organization Approval";
                        break;

                    case "N242":
                        str = "Incomplete/invalid x-ray";
                        break;

                    case "N243":
                        str = "Incomplete/invalid/not approved screening document";
                        break;

                    case "N244":
                        str = "Incomplete/invalid pre-operative photos/visual field results";
                        break;

                    case "N245":
                        str = "Incomplete/invalid plan information for other insurance";
                        break;

                    case "N246":
                        str = "State regulated patient payment limitations apply to this service";
                        break;

                    case "N247":
                        str = "Missing/incomplete/invalid assistant surgeon taxonomy";
                        break;

                    case "N248":
                        str = "Missing/incomplete/invalid assistant surgeon name";
                        break;

                    case "N249":
                        str = "Missing/incomplete/invalid assistant surgeon primary identifier";
                        break;

                    case "N250":
                        str = "Missing/incomplete/invalid assistant surgeon secondary identifier";
                        break;

                    case "N251":
                        str = "Missing/incomplete/invalid attending provider taxonomy";
                        break;

                    case "N252":
                        str = "Missing/incomplete/invalid attending provider name";
                        break;

                    case "N253":
                        str = "Missing/incomplete/invalid attending provider primary identifier";
                        break;

                    case "N254":
                        str = "Missing/incomplete/invalid attending provider secondary identifier";
                        break;

                    case "N255":
                        str = "Missing/incomplete/invalid billing provider taxonomy";
                        break;

                    case "N256":
                        str = "Missing/incomplete/invalid billing provider/supplier name";
                        break;

                    case "N257":
                        str = "Missing/incomplete/invalid billing provider/supplier primary identifier";
                        break;

                    case "N258":
                        str = "Missing/incomplete/invalid billing provider/supplier address";
                        break;

                    case "N259":
                        str = "Missing/incomplete/invalid billing provider/supplier secondary identifier";
                        break;

                    case "N260":
                        str = "Missing/incomplete/invalid billing provider/supplier contact information";
                        break;

                    case "N261":
                        str = "Missing/incomplete/invalid operating provider name";
                        break;

                    case "N262":
                        str = "Missing/incomplete/invalid operating provider primary identifier";
                        break;

                    case "N263":
                        str = "Missing/incomplete/invalid operating provider secondary identifier";
                        break;

                    case "N264":
                        str = "Missing/incomplete/invalid ordering provider name";
                        break;

                    case "N265":
                        str = "Missing/incomplete/invalid ordering provider primary identifier";
                        break;

                    case "N266":
                        str = "Missing/incomplete/invalid ordering provider address";
                        break;

                    case "N267":
                        str = "Missing/incomplete/invalid ordering provider secondary identifier";
                        break;

                    case "N268":
                        str = "Missing/incomplete/invalid ordering provider contact information";
                        break;

                    case "N269":
                        str = "Missing/incomplete/invalid other provider name";
                        break;

                    case "N270":
                        str = "Missing/incomplete/invalid other provider primary identifier";
                        break;

                    case "N271":
                        str = "Missing/incomplete/invalid other provider secondary identifier";
                        break;

                    case "N272":
                        str = "Missing/incomplete/invalid other payer attending provider identifier";
                        break;

                    case "N273":
                        str = "Missing/incomplete/invalid other payer operating provider identifier";
                        break;

                    case "N274":
                        str = "Missing/incomplete/invalid other payer other provider identifier";
                        break;

                    case "N275":
                        str = "Missing/incomplete/invalid other payer purchased service provider identifier";
                        break;

                    case "N276":
                        str = "Missing/incomplete/invalid other payer referring provider identifier";
                        break;

                    case "N277":
                        str = "Missing/incomplete/invalid other payer rendering provider identifier";
                        break;

                    case "N278":
                        str = "Missing/incomplete/invalid other payer service facility provider identifier";
                        break;

                    case "N279":
                        str = "Missing/incomplete/invalid pay-to provider name";
                        break;

                    case "N280":
                        str = "Missing/incomplete/invalid pay-to provider primary identifier";
                        break;

                    case "N281":
                        str = "Missing/incomplete/invalid pay-to provider address";
                        break;

                    case "N282":
                        str = "Missing/incomplete/invalid pay-to provider secondary identifier";
                        break;

                    case "N283":
                        str = "Missing/incomplete/invalid purchased service provider identifier";
                        break;

                    case "N284":
                        str = "Missing/incomplete/invalid referring provider taxonomy";
                        break;

                    case "N285":
                        str = "Missing/incomplete/invalid referring provider name";
                        break;

                    case "N286":
                        str = "Missing/incomplete/invalid referring provider primary identifier";
                        break;

                    case "N287":
                        str = "Missing/incomplete/invalid referring provider secondary identifier";
                        break;

                    case "N288":
                        str = "Missing/incomplete/invalid rendering provider taxonomy";
                        break;

                    case "N289":
                        str = "Missing/incomplete/invalid rendering provider name";
                        break;

                    case "N290":
                        str = "Missing/incomplete/invalid rendering provider primary identifier";
                        break;

                    case "N291":
                        str = "Missing/incomplete/invalid rending provider secondary identifier";
                        break;

                    case "N292":
                        str = "Missing/incomplete/invalid service facility name";
                        break;

                    case "N293":
                        str = "Missing/incomplete/invalid service facility primary identifier";
                        break;

                    case "N294":
                        str = "Missing/incomplete/invalid service facility primary address";
                        break;

                    case "N295":
                        str = "Missing/incomplete/invalid service facility secondary identifier";
                        break;

                    case "N296":
                        str = "Missing/incomplete/invalid supervising provider name";
                        break;

                    case "N297":
                        str = "Missing/incomplete/invalid supervising provider primary identifier";
                        break;

                    case "N298":
                        str = "Missing/incomplete/invalid supervising provider secondary identifier";
                        break;

                    case "N299":
                        str = "Missing/incomplete/invalid occurrence date(s)";
                        break;

                    case "N300":
                        str = "Missing/incomplete/invalid occurrence span date(s)";
                        break;

                    case "N301":
                        str = "Missing/incomplete/invalid procedure date(s)";
                        break;

                    case "N302":
                        str = "Missing/incomplete/invalid other procedure date(s)";
                        break;

                    case "N303":
                        str = "Missing/incomplete/invalid principal procedure date";
                        break;

                    case "N304":
                        str = "Missing/incomplete/invalid dispensed date";
                        break;

                    case "N305":
                        str = "Missing/incomplete/invalid accident date";
                        break;

                    case "N306":
                        str = "Missing/incomplete/invalid acute manifestation date";
                        break;

                    case "N307":
                        str = "Missing/incomplete/invalid adjudication or payment date";
                        break;

                    case "N308":
                        str = "Missing/incomplete/invalid appliance placement date";
                        break;

                    case "N309":
                        str = "Missing/incomplete/invalid assessment date";
                        break;

                    case "N310":
                        str = "Missing/incomplete/invalid assumed or relinquished care date";
                        break;

                    case "N311":
                        str = "Missing/incomplete/invalid authorized to return to work date";
                        break;

                    case "N312":
                        str = "Missing/incomplete/invalid begin therapy date";
                        break;

                    case "N313":
                        str = "Missing/incomplete/invalid certification revision date";
                        break;

                    case "N314":
                        str = "Missing/incomplete/invalid diagnosis date";
                        break;

                    case "N315":
                        str = "Missing/incomplete/invalid disability from date";
                        break;

                    case "N316":
                        str = "Missing/incomplete/invalid disability to date";
                        break;

                    case "N317":
                        str = "Missing/incomplete/invalid discharge hour";
                        break;

                    case "N318":
                        str = "Missing/incomplete/invalid discharge or end of care date";
                        break;

                    case "N319":
                        str = "Missing/incomplete/invalid hearing or vision prescription date";
                        break;

                    case "N320":
                        str = "Missing/incomplete/invalid Home Health Certification Period";
                        break;

                    case "N321":
                        str = "Missing/incomplete/invalid last admission period";
                        break;

                    case "N322":
                        str = "Missing/incomplete/invalid last certification date";
                        break;

                    case "N323":
                        str = "Missing/incomplete/invalid last contact date";
                        break;

                    case "N324":
                        str = "Missing/incomplete/invalid last seen/visit date";
                        break;

                    case "N325":
                        str = "Missing/incomplete/invalid last worked date";
                        break;

                    case "N326":
                        str = "Missing/incomplete/invalide last x-ray date";
                        break;

                    case "N327":
                        str = "Missing/incomplete/invalid other insured birth date";
                        break;

                    case "N328":
                        str = "Missing/incomplete/invalid Oxygen Saturation Test date";
                        break;

                    case "N329":
                        str = "Missing/incomplete/invalid patient birth date";
                        break;

                    case "N330":
                        str = "Missing/incomplete/invalid patient death date";
                        break;

                    case "N331":
                        str = "Missing/incomplete/invalid physician order date";
                        break;

                    case "N332":
                        str = "Missing/incomplete/invalid prior hospital discharge date";
                        break;

                    case "N333":
                        str = "Missing/incomplete/invalid prior placement date";
                        break;

                    case "N334":
                        str = "Missing/incomplete/invalid re-evaluation date";
                        break;

                    case "N335":
                        str = "Missing/incomplete/invalid referral date";
                        break;

                    case "N336":
                        str = "Missing/incomplete/invalid replacement date";
                        break;

                    case "N337":
                        str = "Missing/incomplete/invalid secondary diagnosis date";
                        break;

                    case "N338":
                        str = "Missing/incomplete/invalid shipped date";
                        break;

                    case "N339":
                        str = "Missing/incomplete/invalid similar illness or symptom date";
                        break;

                    case "N340":
                        str = "Missing/incomplete/invalid subscriber birth date";
                        break;

                    case "N341":
                        str = "Missing/incomplete/invalid surgery date";
                        break;

                    case "N342":
                        str = "Missing/incomplete/invalid test performed date";
                        break;

                    case "N343":
                        str = "Missing/incomplete/invalid Transcutaneous Electrical Nerve Stimulator (TENS) trial start date";
                        break;

                    case "N344":
                        str = "Missing/incomplete/invalid Transcutaneous Electrical Nerve Stimulator (TENS) trial end date";
                        break;

                    case "N345":
                        str = "Date range not valid with units submitted";
                        break;

                    case "N346":
                        str = "Missing/incomplete/invalid oral cavity designation code";
                        break;

                    case "N347":
                        str = "Your claim for a referred or purchased service cannot be paid because payment has already been made for this same service to another provider by a payment contractor representing the payer";
                        break;

                    case "N348":
                        str = "You chose that this service/supply/drug would be rendered/supplied and billed by a different practitioner/supplier";
                        break;

                    case "N349":
                        str = "The administration method and drug must be reported to adjudicate this service";
                        break;

                    case "N350":
                        str = "Missing/incomplete/invalid description of service for a Not Otherwise Classified (NOC) code or an Unlisted procedure";
                        break;

                    case "N351":
                        str = "Service date outside of the approved treatment plan service dates";
                        break;

                    case "N352":
                        str = "There are no scheduled payments for this service. Submit a claim for each patient visit";
                        break;

                    case "N353":
                        str = "Benefits have been estimated, when the actual services have been rendered, additional payment will be considered based on the submitted claim";
                        break;

                    case "N354":
                        str = "Incomplete/invalid invoice";
                        break;

                    case "N355":
                        str = "The law permits exceptions to the refund requirement in two cases: - If you did not know, and could not have reasonably been expected to know, that we would not pay for this service; or - If you notified the patient in writing before providing the service that you believed that we were likely to deny the service, and the patient signed a statement agreeing to pay for the service. If you come within either exception, or if you believe the carrier was wrong in its determination that we do not pay for this service, you should request appeal of this determination within 30 days of the date of this notice. Your request for review should include any additional information necessary to support your position. If you request an appeal within 30 days of receiving this notice, you may delay refunding the amount to the patient until you receive the results of the review. If the review decision is favorable to you, you do not need to make any refund. If, however, the review is unfavorable, the law specifies that you must make the refund within 15 days of receiving the unfavorable review decision. The law also permits you to request an appeal at any time within 120 days of the date you receive this notice. However, an appeal request that is received more than 30 days after the date of this notice, does not permit you to delay making the refund. Regardless of when a review is requested, the patient will be notified that you have requested one, and will receive a copy of the determination. The patient has received a separate notice of this denial decision. The notice advises that he/she may be entitled to a refund of any amounts paid, if you should have known that we would not pay and did not tell him/her. It also instructs the patient to contact our office if he/she does not hear anything about a refund within 30 days";
                        break;

                    case "N356":
                        str = "This service is not covered when performed with, or subsequent to, a non-covered service";
                        break;

                    case "N357":
                        str = "Time frame requirements between this service/procedure/supply and a related service/procedure/supply have not been met";
                        break;

                    case "N358":
                        str = "This decision may be reviewed if additional documentation as described in the contract or plan benefit documents is submitted";
                        break;

                    case "N359":
                        str = "Missing/incomplete/invalid height";
                        break;

                    case "N360":
                        str = "Coordination of benefits has not been calculated when estimating benefits for this pre-determination. Submit payment information from the primary payer with the secondary claim";
                        break;

                    case "N361":
                        str = "Charges are adjusted based on multiple diagnostic imaging procedure rules";
                        break;

                    case "N362":
                        str = "The number of Days or Units of Service exceeds our acceptable maximum";
                        break;

                    case "N363":
                        str = "Alert: in the near future we are implementing new policies/procedures that would affect this determination";
                        break;

                    case "N364":
                        str = "According to our agreement, you must waive the deductible and/or coinsurance amounts";
                        break;

                    case "N366":
                        str = "Requested information not provided. The claim will be reopened if the information previously requested is submitted within one year after the date of this denial notice.";
                        break;

                    case "N367":
                        str = "Alert: The claim information has been forwarded to a Consumer Spending Account processor for review; for example, flexible spending account or health savings account.";
                        break;
                    case "N368":
                        str = "You must appeal the determination of the previously adjudicated claim.";
                        break;
                    case "N369":
                        str = "Alert: Although this claim has been processed, it is deficient according to state legislation / regulation.";
                        break;
                    case "N370":
                        str = "Billing exceeds the rental months covered / approved by the payer.";
                        break;
                    case "N371":
                        str = "Alert: title of this equipment must be transferred to the patient.";
                        break;
                    case "N372":
                        str = "Only reasonable and necessary maintenance / service charges are covered.";
                        break;
                    case "N373":
                        str = "It has been determined that another payer paid the services as primary when they were not the primary payer.Therefore, we are refunding to the payer that paid as primary on your behalf.";
                        break;
                    case "N374":
                        str = "Primary Medicare Part A insurance has been exhausted and a Part B Remittance Advice is required.";
                        break;
                    case "N375":
                        str = "Missing / incomplete / invalid questionnaire / information required to determine dependent eligibility.";
                        break;
                    case "N376":
                        str = "Subscriber / patient is assigned to active military duty, therefore primary coverage may be TRICARE.";
                        break;
                    case "N377":
                        str = "Payment based on a processed replacement claim.";
                        break;
                    case "N378":
                        str = "Missing / incomplete / invalid prescription quantity.";
                        break;
                    case "N379":
                        str = "Claim level information does not match line level information.";
                        break;
                    case "N380":
                        str = "The original claim has been processed, submit a corrected claim.";
                        break;
                    case "N381":
                        str = "Alert: Consult our contractual agreement for restrictions / billing / payment information related to these charges.";
                        break;
                    case "N382":
                        str = "Missing / incomplete / invalid patient identifier.";
                        break;
                    case "N383":
                        str = "Not covered when deemed cosmetic.";
                        break;
                    case "N384":
                        str = "Records indicate that the referenced body part / tooth has been removed in a previous procedure.";
                        break;
                    case "N385":
                        str = "Notification of admission was not timely according to published plan procedures.";
                        break;
                    case "N386":
                        str = "This decision was based on a National Coverage Determination(NCD).An NCD provides a coverage determination as to whether a particular item or service is covered.A copy of this policy is available at www.cms.gov / mcd / search.asp.If you do not have web access, you may contact the contractor to request a copy of the NCD.";
                        break;
                    case "N387":
                        str = "Alert: Submit this claim to the patient's other insurer for potential payment of supplemental benefits. We did not forward the claim information.";
                        break;
                    case "N388":
                        str = "Missing / incomplete / invalid prescription number.";
                        break;
                    case "N389":
                        str = "Duplicate prescription number submitted.";
                        break;
                    case "N390":
                        str = "This service / report cannot be billed separately.";
                        break;

                    case "N391":
                        str = "Missing emergency department records.";
                        break;
                    case "N392":
                        str = "Incomplete/ invalid emergency department records.";
                        break;
                    case "N393":
                        str = "Missing progress notes/ report.";
                        break;
                    case "N394":
                        str = "Incomplete / invalid progress notes/ report.";
                        break;
                    case "N395":
                        str = "Missing laboratory report.";
                        break;
                    case "N396":
                        str = "Incomplete/ invalid laboratory report.";
                        break;
                    case "N397":
                        str = "Benefits are not available for incomplete service(s) / undelivered item(s).";
                        break;
                    case "N398":
                        str = "Missing elective consent form.";
                        break;
                    case "N399":
                        str = "Incomplete / invalid elective consent form.";
                        break;
                    case "N400":
                        str = "Alert: Electronically enabled providers should submit claims electronically.";
                        break;
                    case "N401":
                        str = "Missing periodontal charting.";
                        break;
                    case "N402":
                        str = "Incomplete / invalid periodontal charting.";
                        break;
                    case "N403":
                        str = "Missing facility certification.";
                        break;
                    case "N404":
                        str = "Incomplete / invalid facility certification.";
                        break;
                    case "N405":
                        str = "This service is only covered when the donor's insurer(s) do not provide coverage for the service.";
                        break;
                    case "N406":
                        str = "This service is only covered when the recipient's insurer(s) do not provide coverage for the service.";
                        break;
                    case "N407":
                        str = "You are not an approved submitter for this transmission format.";
                        break;
                    case "N408":
                        str = "This payer does not cover deductibles assessed by a previous payer.";
                        break;
                    case "N409":
                        str = "This service is related to an accidental injury and is not covered unless provided within a specific time frame from the date of the accident.";
                        break;
                    case "N410":
                        str = "Not covered unless the prescription changes.";
                        break;
                    case "N411":
                        str = "This service is allowed one time in a 6 - month period.";
                        break;
                    case "N412":
                        str = "This service is allowed 2 times in a 12 - month period.";
                        break;
                    case "N413":
                        str = "This service is allowed 2 times in a benefit year.";
                        break;
                    case "N414":
                        str = "This service is allowed 4 times in a 12 - month period.";
                        break;
                    case "N415":
                        str = "This service is allowed 1 time in an 18 - month period.";
                        break;
                    case "N416":
                        str = "This service is allowed 1 time in a 3 - year period.";
                        break;
                    case "N417":
                        str = "This service is allowed 1 time in a 5 - year period.";
                        break;
                    case "N418":
                        str = "Misrouted claim.See the payer's claim submission instructions.";
                        break;
                    case "N419":
                        str = "Claim payment was the result of a payer's retroactive adjustment due to a retroactive rate change.";
                        break;
                    case "N420":
                        str = "Claim payment was the result of a payer's retroactive adjustment due to a Coordination of Benefits or Third Party Liability Recovery.";
                        break;
                    case "N421":
                        str = "Claim payment was the result of a payer's retroactive adjustment due to a review organization decision.";
                        break;
                    case "N422":
                        str = "Claim payment was the result of a payer's retroactive adjustment due to a payer's contract incentive program.";
                        break;
                    case "N423":
                        str = "Claim payment was the result of a payer's retroactive adjustment due to a non standard program.";
                        break;
                    case "N424":
                        str = "Patient does not reside in the geographic area required for this type of payment.";
                        break;
                    case "N425":
                        str = "Statutorily excluded service(s).";
                        break;
                    case "N426":
                        str = "No coverage when self - administered.";
                        break;
                    case "N427":
                        str = "Payment for eyeglasses or contact lenses can be made only after cataract surgery.";
                        break;
                    case "N428":
                        str = "Not covered when performed in this place of service.";
                        break;
                    case "N429":
                        str = "Not covered when considered routine.";
                        break;
                    case "N430":
                        str = "Procedure code is inconsistent with the units billed.";
                        break;
                    case "N431":
                        str = "Not covered with this procedure.";
                        break;
                    case "N432":
                        str = "Alert: Adjustment based on a Recovery Audit.";
                        break;
                    case "N433":
                        str = "Resubmit this claim using only your National Provider Identifier(NPI).";
                        break;
                    case "N434":
                        str = "Missing / Incomplete / Invalid Present on Admission indicator.";
                        break;
                    case "N435":
                        str = "Exceeds number / frequency approved / allowed within time period without support documentation.";
                        break;
                    case "N436":
                        str = "The injury claim has not been accepted and a mandatory medical reimbursement has been made.";
                        break;
                    case "N437":
                        str = "Alert: If the injury claim is accepted, these charges will be reconsidered.";
                        break;
                    case "N438":
                        str = "This jurisdiction only accepts paper claims.";
                        break;
                    case "N439":
                        str = "Missing anesthesia physical status report/ indicators.";
                        break;
                    case "N440":
                        str = "Incomplete / invalid anesthesia physical status report/ indicators.";
                        break;
                    case "N441":
                        str = "This missed/ cancelled appointment is not covered.";
                        break;
                    case "N442":
                        str = "Payment based on an alternate fee schedule.";
                        break;
                    case "N443":
                        str = "Missing/ incomplete / invalid total time or begin/ end time.";
                        break;
                    case "N444":
                        str = "Alert: This facility has not filed the Election for High Cost Outlier form with the Division of Workers' Compensation.";
                        break;
                    case "N445":
                        str = "Missing document for actual cost or paid amount.";
                        break;
                    case "N446":
                        str = "Incomplete / invalid document for actual cost or paid amount.";
                        break;
                    case "N447":
                        str = "Payment is based on a generic equivalent as required documentation was not provided.";
                        break;
                    case "N448":
                        str = "This drug / service / supply is not included in the fee schedule or contracted / legislated fee arrangement.";
                        break;
                    case "N449":
                        str = "Payment based on a comparable drug / service / supply.";
                        break;
                    case "N450":
                        str = "Covered only when performed by the primary treating physician or the designee.";
                        break;
                    case "N451":
                        str = "Missing Admission Summary Report.";
                        break;
                    case "N452":
                        str = "Incomplete / invalid Admission Summary Report.";
                        break;
                    case "N453":
                        str = "Missing Consultation Report.";
                        break;
                    case "N454":
                        str = "Incomplete / invalid Consultation Report.";
                        break;
                    case "N455":
                        str = "Missing Physician Order.";
                        break;
                    case "N456":
                        str = "Incomplete / invalid Physician Order.";
                        break;
                    case "N457":
                        str = "Missing Diagnostic Report.";
                        break;
                    case "N458":
                        str = "Incomplete / invalid Diagnostic Report.";
                        break;
                    case "N459":
                        str = "Missing Discharge Summary.";
                        break;
                    case "N460":
                        str = "Incomplete / invalid Discharge Summary.";
                        break;
                    case "N461":
                        str = "Missing Nursing Notes.";
                        break;
                    case "N462":
                        str = "Incomplete / invalid Nursing Notes.";
                        break;
                    case "N463":
                        str = "Missing support data for claim.";
                        break;
                    case "N464":
                        str = "Incomplete/ invalid support data for claim.";
                        break;
                    case "N465":
                        str = "Missing Physical Therapy Notes / Report.";
                        break;
                    case "N466":
                        str = "Incomplete / invalid Physical Therapy Notes / Report.";
                        break;
                    case "N467":
                        str = "Missing Tests and Analysis Report.";
                        break;
                    case "N468":
                        str = "Incomplete / invalid Report of Tests and Analysis Report.";
                        break;
                    case "N469":
                        str = "Alert: Claim / Service(s) subject to appeal process, see section 935 of Medicare Prescription Drug, Improvement, and Modernization Act of 2003(MMA).";
                        break;
                    case "N470":
                        str = "This payment will complete the mandatory medical reimbursement limit.";
                        break;
                    case "N471":
                        str = "Missing / incomplete / invalid HIPPS Rate Code.";
                        break;
                    case "N472":
                        str = "Payment for this service has been issued to another provider.";
                        break;
                    case "N473":
                        str = "Missing certification.";
                        break;
                    case "N474":
                        str = "Incomplete / invalid certification.";
                        break;
                    case "N475":
                        str = "Missing completed referral form.";
                        break;
                    case "N476":
                        str = "Incomplete / invalid completed referral form.";
                        break;
                    case "N477":
                        str = "Missing Dental Models.";
                        break;
                    case "N478":
                        str = "Incomplete / invalid Dental Models.";
                        break;

                    case "N480":
                        str = "Incomplete/ invalid Explanation of Benefits(Coordination of Benefits or Medicare Secondary Payer).";
                        break;
                    case "N481":
                        str = "Missing Models.";
                        break;
                    case "N482":
                        str = "Incomplete/ invalid Models.";
                        break;
                    case "N485":
                        str = "Missing Physical Therapy Certification.";
                        break;
                    case "N486":
                        str = "Incomplete/ invalid Physical Therapy Certification.";
                        break;
                    case "N487":
                        str = "Missing Prosthetics or Orthotics Certification.";
                        break;
                    case "N488":
                        str = "Incomplete/ invalid Prosthetics or Orthotics Certification.";
                        break;
                    case "N489":
                        str = "Missing referral form.";
                        break;
                    case "N490":
                        str = "Incomplete/ invalid referral form.";
                        break;
                    case "N491":
                        str = "Missing/ Incomplete / Invalid Exclusionary Rider Condition.";
                        break;
                    case "N492":
                        str = "Alert: A network provider may bill the member for this service if the member requested the service and agreed in writing, prior to receiving the service, to be financially responsible for the billed charge.";
                        break;
                    case "N493":
                        str = "Missing Doctor First Report of Injury.";
                        break;
                    case "N494":
                        str = "Incomplete / invalid Doctor First Report of Injury.";
                        break;
                    case "N495":
                        str = "Missing Supplemental Medical Report.";
                        break;
                    case "N496":
                        str = "Incomplete / invalid Supplemental Medical Report.";
                        break;
                    case "N497":
                        str = "Missing Medical Permanent Impairment or Disability Report.";
                        break;
                    case "N498":
                        str = "Incomplete / invalid Medical Permanent Impairment or Disability Report.";
                        break;
                    case "N499":
                        str = "Missing Medical Legal Report.";
                        break;
                    case "N500":
                        str = "Incomplete / invalid Medical Legal Report.";
                        break;
                    case "N501":
                        str = "Missing Vocational Report.";
                        break;
                    case "N502":
                        str = "Incomplete / invalid Vocational Report.";
                        break;
                    case "N503":
                        str = "Missing Work Status Report.";
                        break;
                    case "N504":
                        str = "Incomplete / invalid Work Status Report.";
                        break;
                    case "N505":
                        str = "Alert: This response includes only services that could be estimated in real - time.No estimate will be provided for the services that could not be estimated in real - time.";
                        break;
                    case "N506":
                        str = "Alert: This is an estimate of the member’s liability based on the information available at the time the estimate was processed.Actual coverage and member liability amounts will be determined when the claim is processed.This is not a pre - authorization or a guarantee of payment.";
                        break;
                    case "N507":
                        str = "Plan distance requirements have not been met.";
                        break;
                    case "N508":
                        str = "Alert: This real - time claim adjudication response represents the member responsibility to the provider for services reported. The member will receive an Explanation of Benefits electronically or in the mail.Contact the insurer if there are any questions.";
                        break;
                    case "N509":
                        str = "Alert: A current inquiry shows the member’s Consumer Spending Account contains sufficient funds to cover the member liability for this claim / service.Actual payment from the Consumer Spending Account will depend on the availability of funds and determination of eligible services at the time of payment processing.";
                        break;
                    case "N510":
                        str = "Alert: A current inquiry shows the member’s Consumer Spending Account does not contain sufficient funds to cover the member's liability for this claim/service. Actual payment from the Consumer Spending Account will depend on the availability of funds and determination of eligible services at the time of payment processing.";
                        break;
                    case "N511":
                        str = "Alert: Information on the availability of Consumer Spending Account funds to cover the member liability on this claim / service is not available at this time.";
                        break;
                    case "N512":
                        str = "Alert: This is the initial remit of a non - NCPDP claim originally submitted real - time without change to the adjudication.";
                        break;
                    case "N513":
                        str = "Alert: This is the initial remit of a non - NCPDP claim originally submitted real - time with a change to the adjudication.";
                        break;
                    case "N516":
                        str = "Records indicate a mismatch between the submitted NPI and EIN.";
                        break;
                    case "N517":
                        str = "Resubmit a new claim with the requested information.";
                        break;
                    case "N518":
                        str = "No separate payment for accessories when furnished for use with oxygen equipment.";
                        break;
                    case "N519":
                        str = "Invalid combination of HCPCS modifiers.";
                        break;
                    case "N520":
                        str = "Alert: Payment made from a Consumer Spending Account.";
                        break;
                    case "N521":
                        str = "Mismatch between the submitted provider information and the provider information stored in our system.";
                        break;
                    case "N523":
                        str = "The limitation on outlier payments defined by this payer for this service period has been met.The outlier payment otherwise applicable to this claim has not been paid.";
                        break;
                    case "N524":
                        str = "Based on policy this payment constitutes payment in full.";
                        break;
                    case "N525":
                        str = "These services are not covered when performed within the global period of another service.";
                        break;
                    case "N526":
                        str = "Not qualified for recovery based on employer size.";
                        break;
                    case "N527":
                        str = "We processed this claim as the primary payer prior to receiving the recovery demand.";
                        break;
                    case "N528":
                        str = "Patient is entitled to benefits for Institutional Services only.";
                        break;
                    case "N529":
                        str = "Patient is entitled to benefits for Professional Services only.";
                        break;
                    case "N530":
                        str = "Not Qualified for Recovery based on enrollment information.";
                        break;
                    case "N531":
                        str = "Not qualified for recovery based on direct payment of premium.";
                        break;
                    case "N532":
                        str = "Not qualified for recovery based on disability and working status.";
                        break;
                    case "N533":
                        str = "Services performed in an Indian Health Services facility under a self - insured tribal Group Health Plan.";
                        break;
                    case "N534":
                        str = "This is an individual policy, the employer does not participate in plan sponsorship.";
                        break;
                    case "N536":
                        str = "We are not changing the prior payer's determination of patient responsibility, which you may collect, as this service is not covered by us.";
                        break;
                    case "N537":
                        str = "We have examined claims history and no records of the services have been found.";
                        break;
                    case "N538":
                        str = "A facility is responsible for payment to outside providers who furnish these services / supplies / drugs to its patients / residents.";
                        break;
                    case "N539":
                        str = "Alert: We processed appeals / waiver requests on your behalf and that request has been denied.";
                        break;
                    case "N540":
                        str = "Payment adjusted based on the interrupted stay policy.";
                        break;
                    case "N541":
                        str = "Mismatch between the submitted insurance type code and the information stored in our system.";
                        break;
                    case "N542":
                        str = "Missing income verification.";
                        break;
                    case "N543":
                        str = "Incomplete / invalid income verification.";
                        break;
                    case "N544":
                        str = "Alert: Although this was paid, you have billed with a referring / ordering provider that does not match our system record.Unless corrected this will not be paid in the future.";
                        break;
                    case "N545":
                        str = "Payment reduced based on status as an unsuccessful eprescriber per the Electronic Prescribing(eRx) Incentive Program.";
                        break;
                    case "N546":
                        str = "Payment represents a previous reduction based on the Electronic Prescribing(eRx) Incentive Program.";
                        break;
                    case "N547":
                        str = "A refund request(Frequency Type Code 8) was processed previously.";
                        break;
                    case "N548":
                        str = "Alert: Patient's calendar year deductible has been met.";
                        break;
                    case "N549":
                        str = "Alert: Patient's calendar year out-of-pocket maximum has been met.";
                        break;
                    case "N550":
                        str = "Alert: You have not responded to requests to revalidate your provider / supplier enrollment information.Your failure to revalidate your enrollment information will result in a payment hold in the near future.";
                        break;
                    case "N551":
                        str = "Payment adjusted based on the Ambulatory Surgical Center(ASC) Quality Reporting Program.";
                        break;
                    case "N552":
                        str = "Payment adjusted to reverse a previous withhold / bonus amount.";
                        break;
                    case "N554":
                        str = "Missing / Incomplete / Invalid Family Planning Indicator.";
                        break;
                    case "N555":
                        str = "Missing medication list.";
                        break;
                    case "N556":
                        str = "Incomplete / invalid medication list.";
                        break;
                    case "N557":
                        str = "This claim / service is not payable under our service area.The claim must be filed to the Payer / Plan in whose service area the specimen was collected.";
                        break;
                    case "N558":
                        str = "This claim / service is not payable under our service area.The claim must be filed to the Payer / Plan in whose service area the equipment was received.";
                        break;
                    case "N559":
                        str = "This claim / service is not payable under our service area.The claim must be filed to the Payer / Plan in whose service area the Ordering Physician is located.";
                        break;
                    case "N560":
                        str = "The pilot program requires an interim or final claim within 60 days of the Notice of Admission.A claim was not received.";
                        break;
                    case "N561":
                        str = "The bundled claim originally submitted for this episode of care includes related readmissions.You may resubmit the original claim to receive a corrected payment based on this readmission.";
                        break;
                    case "N562":
                        str = "The provider number of your incoming claim does not match the provider number on the processed Notice of Admission(NOA) for this bundled payment.";
                        break;
                    case "N563":
                        str = "Alert: Missing required provider / supplier issuance of advance patient notice of non - coverage.The patient is not liable for payment for this service.";
                        break;
                    case "N564":
                        str = "Patient did not meet the inclusion criteria for the demonstration project or pilot program.";
                        break;
                    case "N565":
                        str = "Alert: This non - payable reporting code requires a modifier.Future claims containing this non - payable reporting code must include an appropriate modifier for the claim to be processed.";
                        break;
                    case "N566":
                        str = "Alert: This procedure code requires functional reporting.Future claims containing this procedure code must include an applicable non - payable code and appropriate modifiers for the claim to be processed.";
                        break;
                    case "N567":
                        str = "Not covered when considered preventative.";
                        break;
                    case "N568":
                        str = "Alert: Initial payment based on the Notice of Admission(NOA) under the Bundled Payment Model IV initiative.";
                        break;
                    case "N569":
                        str = "Not covered when performed for the reported diagnosis.";
                        break;
                    case "N570":
                        str = "Missing / incomplete / invalid credentialing data.";
                        break;
                    case "N571":
                        str = "Alert: Payment will be issued quarterly by another payer / contractor.";
                        break;
                    case "N573":
                        str = "Alert: You have been overpaid and must refund the overpayment.The refund will be requested separately by another payer / contractor.";
                        break;
                    case "N574":
                        str = "Our records indicate the ordering / referring provider is of a type/ specialty that cannot order or refer.Please verify that the claim ordering/ referring provider information is accurate or contact the ordering/ referring provider.";
                        break;
                    case "N575":
                        str = "Mismatch between the submitted ordering/ referring provider name and the ordering / referring provider name stored in our records.";
                        break;
                    case "N576":
                        str = "Services not related to the specific incident/ claim / accident / loss being reported.";
                        break;
                    case "N577":
                        str = "Personal Injury Protection (PIP)Coverage.";
                        break;
                    case "N578":
                        str = "Coverages do not apply to this loss.";
                        break;
                    case "N579":
                        str = "Medical Payments Coverage (MPC).";
                        break;
                    case "N580":
                        str = "Determination based on the provisions of the insurance policy.";
                        break;
                    case "N581":
                        str = "Investigation of coverage eligibility is pending.";
                        break;
                    case "N582":
                        str = "Benefits suspended pending the patient's cooperation.";
                        break;
                    case "N583":
                        str = "Patient was not an occupant of our insured vehicle and therefore, is not an eligible injured person.";
                        break;
                    case "N584":
                        str = "Not covered based on the insured's noncompliance with policy or statutory conditions.";
                        break;
                    case "N585":
                        str = "Benefits are no longer available based on a final injury settlement.";
                        break;
                    case "N586":
                        str = "The injured party does not qualify for benefits.";
                        break;
                    case "N587":
                        str = "Policy benefits have been exhausted.";
                        break;
                    case "N588":
                        str = "The patient has instructed that medical claims / bills are not to be paid.";
                        break;
                    case "N589":
                        str = "Coverage is excluded to any person injured as a result of operating a motor vehicle while in an intoxicated condition or while the ability to operate such a vehicle is impaired by the use of a drug.";
                        break;
                    case "N590":
                        str = "Missing independent medical exam detailing the cause of injuries sustained and medical necessity of services rendered.";
                        break;
                    case "N591":
                        str = "Payment based on an Independent Medical Examination (IME)or Utilization Review(UR).";
                        break;
                    case "N592":
                        str = "Adjusted because this is not the initial prescription or exceeds the amount allowed for the initial prescription.";
                        break;
                    case "N593":
                        str = "Not covered based on failure to attend a scheduled Independent Medical Exam(IME).";
                        break;
                    case "N594":
                        str = "Records reflect the injured party did not complete an Application for Benefits for this loss.";
                        break;
                    case "N595":
                        str = "Records reflect the injured party did not complete an Assignment of Benefits for this loss.";
                        break;
                    case "N596":
                        str = "Records reflect the injured party did not complete a Medical Authorization for this loss.";
                        break;
                    case "N597":
                        str = "Adjusted based on a medical / dental provider's apportionment of care between related injuries and other unrelated medical/dental conditions/injuries.";
                        break;
                    case "N598":
                        str = "Health care policy coverage is primary.";
                        break;
                    case "N599":
                        str = "Our payment for this service is based upon a reasonable amount pursuant to both the terms and conditions of the policy of insurance under which the subject claim is being made as well as the Florida No - Fault Statute, which permits, when determining a reasonable charge for a service, an insurer to consider usual and customary charges and payments accepted by the provider, reimbursement levels in the community and various federal and state fee schedules applicable to automobile and other insurance coverages, and other information relevant to the reasonableness of the reimbursement for the service. The payment for this service is based upon 200 % of the Participating Level of Medicare Part B fee schedule for the locale in which the services were rendered.";
                        break;
                    case "N600":
                        str = "Adjusted based on the applicable fee schedule for the region in which the service was rendered.";
                        break;
                    case "N601":
                        str = "In accordance with Hawaii Administrative Rules, Title 16, Chapter 23 Motor Vehicle Insurance Law payment is recommended based on Medicare Resource Based Relative Value Scale System applicable to Hawaii.";
                        break;
                    case "N602":
                        str = "Adjusted based on the Redbook maximum allowance.";
                        break;
                    case "N603":
                        str = "This fee is calculated according to the New Jersey medical fee schedules for Automobile Personal Injury Protection and Motor Bus Medical Expense Insurance Coverage.";
                        break;
                    case "N604":
                        str = "In accordance with New York No - Fault Law, Regulation 68, this base fee was calculated according to the New York Workers' Compensation Board Schedule of Medical Fees, pursuant to Regulation 83 and / or Appendix 17-C of 11 NYCRR.";
                        break;
                    case "N605":
                        str = "This fee was calculated based upon New York All Patients Refined Diagnosis Related Groups(APR - DRG), pursuant to Regulation 68.";
                        break;
                    case "N606":
                        str = "The Oregon allowed amount for this procedure is based upon the Workers Compensation Fee Schedule(OAR 436 - 009).The allowed amount has been calculated in accordance with Section 4 of ORS 742.524.";
                        break;
                    case "N607":
                        str = "Service provided for non - compensable condition(s).";
                        break;
                    case "N608":
                        str = "The fee schedule amount allowed is calculated at 110 % of the Medicare Fee Schedule for this region, specialty and type of service.This fee is calculated in compliance with Act 6.";
                        break;
                    case "N609":
                        str = "80 % of the provider's billed amount is being recommended for payment according to Act 6.";
                        break;
                    case "N610":
                        str = "Alert: Payment based on an appropriate level of care.";
                        break;
                    case "N611":
                        str = "Claim in litigation.Contact insurer for more information.";
                        break;
                    case "N612":
                        str = "Medical provider not authorized / certified to provide treatment to injured workers in this jurisdiction.";
                        break;
                    case "N613":
                        str = "Alert: Although this was paid, you have billed with an ordering provider that needs to update their enrollment record.Please verify that the ordering provider information you submitted on the claim is accurate and if it is, contact the ordering provider instructing them to update their enrollment record.Unless corrected, a claim with this ordering provider will not be paid in the future.";
                        break;
                    case "N614":
                        str = "Alert: Additional information is included in the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information).";
                        break;
                    case "N615":
                        str = "Alert: This enrollee receiving advance payments of the premium tax credit is in the grace period of three consecutive months for non - payment of premium.Under 45 CFR 156.270, a Qualified Health Plan issuer must pay all appropriate claims for services rendered to the enrollee during the first month of the grace period and may pend claims for services rendered to the enrollee in the second and third months of the grace period.";
                        break;
                    case "N616":
                        str = "Alert: This enrollee is in the first month of the advance premium tax credit grace period.";
                        break;
                    case "N617":
                        str = "This enrollee is in the second or third month of the advance premium tax credit grace period.";
                        break;
                    case "N618":
                        str = "Alert: This claim will automatically be reprocessed if the enrollee pays their premiums.";
                        break;
                    case "N619":
                        str = "Coverage terminated for non - payment of premium.";
                        break;
                    case "N621":
                        str = "Charges for Jurisdiction required forms, reports, or chart notes are not payable.";
                        break;
                    case "N622":
                        str = "Not covered based on the date of injury / accident.";
                        break;
                    case "N623":
                        str = "Not covered when deemed unscientific / unproven / outmoded / experimental / excessive / inappropriate.";
                        break;
                    case "N624":
                        str = "The associated Workers' Compensation claim has been withdrawn.";
                        break;
                    case "N625":
                        str = "Missing / Incomplete / Invalid Workers' Compensation Claim Number.";
                        break;
                    case "N626":
                        str = "New or established patient E / M codes are not payable with chiropractic care codes.";
                        break;
                    case "N628":
                        str = "Out - patient follow up visits on the same date of service as a scheduled test or treatment is disallowed.";
                        break;
                    case "N629":
                        str = "Reviews / documentation / notes / summaries / reports / charts not requested.";
                        break;
                    case "N630":
                        str = "Referral not authorized by attending physician.";
                        break;
                    case "N631":
                        str = "Medical Fee Schedule does not list this code.An allowance was made for a comparable service.";
                        break;
                    case "N633":
                        str = "Additional anesthesia time units are not allowed.";
                        break;
                    case "N634":
                        str = "The allowance is calculated based on anesthesia time units.";
                        break;
                    case "N635":
                        str = "The Allowance is calculated based on the anesthesia base units plus time.";
                        break;
                    case "N636":
                        str = "Adjusted because this is reimbursable only once per injury.";
                        break;
                    case "N637":
                        str = "Consultations are not allowed once treatment has been rendered by the same provider.";
                        break;
                    case "N638":
                        str = "Reimbursement has been made according to the home health fee schedule.";
                        break;
                    case "N639":
                        str = "Reimbursement has been made according to the inpatient rehabilitation facilities fee schedule.";
                        break;
                    case "N640":
                        str = "Exceeds number / frequency approved / allowed within time period.";
                        break;
                    case "N641":
                        str = "Reimbursement has been based on the number of body areas rated.";
                        break;
                    case "N642":
                        str = "Adjusted when billed as individual tests instead of as a panel.";
                        break;
                    case "N643":
                        str = "The services billed are considered Not Covered or Non - Covered(NC) in the applicable state fee schedule.";
                        break;
                    case "N644":
                        str = "Reimbursement has been made according to the bilateral procedure rule.";
                        break;
                    case "N645":
                        str = "Mark - up allowance.";
                        break;
                    case "N646":
                        str = "Reimbursement has been adjusted based on the guidelines for an assistant.";
                        break;
                    case "N647":
                        str = "Adjusted based on diagnosis - related group(DRG).";
                        break;
                    case "N648":
                        str = "Adjusted based on Stop Loss.";
                        break;
                    case "N649":
                        str = "Payment based on invoice.";
                        break;
                    case "N651":
                        str = "No Personal Injury Protection / Medical Payments Coverage on the policy at the time of the loss.";
                        break;
                    case "N652":
                        str = "The date of service is before the date of loss.";
                        break;
                    case "N653":
                        str = "The date of injury does not match the reported date of loss.";
                        break;
                    case "N654":
                        str = "Adjusted based on achievement of maximum medical improvement(MMI).";
                        break;
                    case "N655":
                        str = "Payment based on provider's geographic region.";
                        break;
                    case "N656":
                        str = "An interest payment is being made because benefits are being paid outside the statutory requirement.";
                        break;
                    case "N657":
                        str = "This should be billed with the appropriate code for these services.";
                        break;
                    case "N658":
                        str = "The billed service(s) are not considered medical expenses.";
                        break;
                    case "N659":
                        str = "This item is exempt from sales tax.";
                        break;
                    case "N660":
                        str = "Sales tax has been included in the reimbursement.";
                        break;
                    case "N661":
                        str = "Documentation does not support that the services rendered were medically necessary.";
                        break;
                    case "N662":
                        str = "Alert: Consideration of payment will be made upon receipt of a final bill.";
                        break;
                    case "N663":
                        str = "Adjusted based on an agreed amount.";
                        break;
                    case "N664":
                        str = "Adjusted based on a legal settlement.";
                        break;
                    case "N665":
                        str = "Services by an unlicensed provider are not reimbursable.";
                        break;
                    case "N666":
                        str = "Only one evaluation and management code at this service level is covered during the course of care.";
                        break;
                    case "N667":
                        str = "Missing prescription.";
                        break;
                    case "N668":
                        str = "Incomplete / invalid prescription.";
                        break;
                    case "N669":
                        str = "Adjusted based on the Medicare fee schedule.";
                        break;
                    case "N670":
                        str = "This service code has been identified as the primary procedure code subject to the Medicare Multiple Procedure Payment Reduction(MPPR) rule.";
                        break;
                    case "N671":
                        str = "Payment based on a jurisdiction cost - charge ratio.";
                        break;
                    case "N672":
                        str = "Alert: Amount applied to Health Insurance Offset.";
                        break;
                    case "N673":
                        str = "Reimbursement has been calculated based on an outpatient per diem or an outpatient factor and / or fee schedule amount.";
                        break;
                    case "N674":
                        str = "Not covered unless a pre - requisite procedure / service has been provided.";
                        break;
                    case "N675":
                        str = "Additional information is required from the injured party.";
                        break;
                    case "N676":
                        str = "Service does not qualify for payment under the Outpatient Facility Fee Schedule.";
                        break;
                    case "N677":
                        str = "Alert: Films / Images will not be returned.";
                        break;
                    case "N678":
                        str = "Missing post - operative images / visual field results.";
                        break;
                    case "N679":
                        str = "Incomplete / Invalid post - operative images / visual field results.";
                        break;
                    case "N680":
                        str = "Missing / Incomplete / Invalid date of previous dental extractions.";
                        break;
                    case "N681":
                        str = "Missing / Incomplete / Invalid full arch series.";
                        break;
                    case "N682":
                        str = "Missing / Incomplete / Invalid history of prior periodontal therapy / maintenance.";
                        break;
                    case "N683":
                        str = "Missing / Incomplete / Invalid prior treatment documentation.";
                        break;
                    case "N684":
                        str = "Payment denied as this is a specialty claim submitted as a general claim.";
                        break;
                    case "N685":
                        str = "Missing / Incomplete / Invalid Prosthesis, Crown or Inlay Code.";
                        break;
                    case "N686":
                        str = "Missing / incomplete / Invalid questionnaire needed to complete payment determination.";
                        break;
                    case "N687":
                        str = "Alert: This reversal is due to a retroactive disenrollment.";
                        break;
                    case "N688":
                        str = "Alert: This reversal is due to a medical or utilization review decision.";
                        break;
                    case "N689":
                        str = "Alert: This reversal is due to a retroactive rate change.";
                        break;
                    case "N690":
                        str = "Alert: This reversal is due to a provider submitted appeal.";
                        break;
                    case "N691":
                        str = "Alert: This reversal is due to a patient submitted appeal.";
                        break;
                    case "N692":
                        str = "Alert: This reversal is due to an incorrect rate on the initial adjudication.";
                        break;
                    case "N693":
                        str = "Alert: This reversal is due to a cancellation of the claim by the provider.";
                        break;
                    case "N694":
                        str = "Alert: This reversal is due to a resubmission / change to the claim by the provider.";
                        break;
                    case "N695":
                        str = "Alert: This reversal is due to incorrect patient financial responsibility information on the initial adjudication.";
                        break;
                    case "N696":
                        str = "Alert: This reversal is due to a Coordination of Benefits or Third Party Liability Recovery retroactive adjustment.";
                        break;
                    case "N697":
                        str = "Alert: This reversal is due to a payer's retroactive contract incentive program adjustment.";
                        break;
                    case "N698":
                        str = "Alert: This reversal is due to non - payment of the health insurance premiums(Health Insurance Exchange or other) by the end of the premium payment grace period, resulting in loss of coverage.";
                        break;
                    case "N701":
                        str = "Payment adjusted based on the Value - based Payment Modifier.";
                        break;
                    case "N702":
                        str = "Decision based on review of previously adjudicated claims or for claims in process for the same/ similar type of services.";
                        break;
                    case "N703":
                        str = "This service is incompatible with previously adjudicated claims or claims in process.";
                        break;
                    case "N704":
                        str = "Alert: You may not appeal this decision but can resubmit this claim / service with corrected information if warranted.";
                        break;
                    case "N705":
                        str = "Incomplete / invalid documentation.";
                        break;
                    case "N707":
                        str = "Incomplete/ invalid orders.";
                        break;
                    case "N708":
                        str = "Missing orders.";
                        break;
                    case "N709":
                        str = "Incomplete/ invalid notes.";
                        break;
                    case "N710":
                        str = "Missing notes.";
                        break;
                    case "N711":
                        str = "Incomplete/ invalid summary.";
                        break;
                    case "N712":
                        str = "Missing summary.";
                        break;
                    case "N713":
                        str = "Incomplete/ invalid report.";
                        break;
                    case "N714":
                        str = "Missing report.";
                        break;
                    case "N715":
                        str = "Incomplete/ invalid chart.";
                        break;
                    case "N716":
                        str = "Missing chart.";
                        break;
                    case "N717":
                        str = "Incomplete/ Invalid documentation of face - to - face examination.";
                        break;
                    case "N718":
                        str = "Missing documentation of face - to - face examination.";
                        break;
                    case "N719":
                        str = "Penalty applied based on plan requirements not being met.";
                        break;
                    case "N720":
                        str = "Alert: The patient overpaid you. You may need to issue the patient a refund for the difference between the patient’s payment and the amount shown as patient responsibility on this notice.";
                        break;
                    case "N721":
                        str = "This service is only covered when performed as part of a clinical trial.";
                        break;
                    case "N722":
                        str = "Patient must use Workers' Compensation Set-Aside (WCSA) funds to pay for the medical service or item.";
                        break;
                    case "N723":
                        str = "Patient must use Liability set - aside(LSA) funds to pay for the medical service or item.";
                        break;
                    case "N724":
                        str = "Patient must use No - Fault set - aside(NFSA) funds to pay for the medical service or item.";
                        break;
                    case "N725":
                        str = "A liability insurer has reported having ongoing responsibility for medical services (ORM) for this diagnosis.";
                        break;
                    case "N726":
                        str = "A conditional payment is not allowed.";
                        break;
                    case "N727":
                        str = "A no - fault insurer has reported having ongoing responsibility for medical services (ORM) for this diagnosis.";
                        break;
                    case "N728":
                        str = "A workers' compensation insurer has reported having ongoing responsibility for medical services (ORM) for this diagnosis.";
                        break;
                    case "N729":
                        str = "Missing patient medical / dental record for this service.";
                        break;
                    case "N730":
                        str = "Incomplete / invalid patient medical / dental record for this service.";
                        break;
                    case "N731":
                        str = "Incomplete / Invalid mental health assessment.";
                        break;
                    case "N732":
                        str = "Services performed at an unlicensed facility are not reimbursable.";
                        break;
                    case "N733":
                        str = "Regulatory surcharges are paid directly to the state.";
                        break;
                    case "N734":
                        str = "The patient is eligible for these medical services only when unable to work or perform normal activities due to an illness or injury.";
                        break;
                    case "N736":
                        str = "Incomplete / invalid Sleep Study Report.";
                        break;
                    case "N737":
                        str = "Missing Sleep Study Report.";
                        break;
                    case "N738":
                        str = "Incomplete / invalid Vein Study Report.";
                        break;
                    case "N739":
                        str = "Missing Vein Study Report.";
                        break;
                    case "N740":
                        str = "The member's Consumer Spending Account does not contain sufficient funds to cover the member's liability for this claim / service.";
                        break;
                    case "N741":
                        str = "This is a site neutral payment.";
                        break;
                    case "N743":
                        str = "Adjusted because the services may be related to an employment accident.";
                        break;
                    case "N744":
                        str = "Adjusted because the services may be related to an auto / other accident.";
                        break;
                    case "N745":
                        str = "Missing Ambulance Report.";
                        break;
                    case "N746":
                        str = "Incomplete / invalid Ambulance Report.";
                        break;
                    case "N747":
                        str = "This is a misdirected claim / service.Submit the claim to the payer / plan where the patient resides.";
                        break;
                    case "N748":
                        str = "Adjusted because the related hospital charges have not been received.";
                        break;
                    case "N749":
                        str = "Missing Blood Gas Report.";
                        break;
                    case "N750":
                        str = "Incomplete / invalid Blood Gas Report.";
                        break;
                    case "N751":
                        str = "Adjusted because the drug is covered under a Medicare Part D plan.";
                        break;
                    case "N752":
                        str = "Missing / incomplete / invalid HIPPS Treatment Authorization Code(TAC).";
                        break;
                    case "N753":
                        str = "Missing / incomplete / invalid Attachment Control Number.";
                        break;
                    case "N754":
                        str = "Missing / incomplete / invalid Referring Provider or Other Source Qualifier on the 1500 Claim Form.";
                        break;
                    case "N755":
                        str = "Missing / incomplete / invalid ICD Indicator.";
                        break;
                    case "N756":
                        str = "Missing / incomplete / invalid point of drop - off address.";
                        break;
                    case "N757":
                        str = "Adjusted based on the Federal Indian Fees schedule(MLR).";
                        break;
                    case "N758":
                        str = "Adjusted based on the prior authorization decision.";
                        break;
                    case "N759":
                        str = "Payment adjusted based on the National Electrical Manufacturers Association(NEMA) Standard XR - 29 - 2013.";
                        break;
                    case "N760":
                        str = "This facility is not authorized to receive payment for the service(s).";
                        break;
                    case "N761":
                        str = "This provider is not authorized to receive payment for the service(s).";
                        break;
                    case "N762":
                        str = "This facility is not certified for Tomosynthesis(3 - D) mammography.";
                        break;
                    case "N763":
                        str = "The demonstration code is not appropriate for this claim; resubmit without a demonstration code.";
                        break;
                    case "N764":
                        str = "Missing / incomplete / invalid Hematocrit(HCT) value.";
                        break;
                    case "N765":
                        str = "This payer does not cover co - insurance assessed by a previous payer.";
                        break;
                    case "N766":
                        str = "This payer does not cover co - payment assessed by a previous payer.";
                        break;
                    case "N767":
                        str = "The Medicaid state requires provider to be enrolled in the member’s Medicaid state program prior to any claim benefits being processed.";
                        break;
                    case "N768":
                        str = "Incomplete / invalid initial evaluation report.";
                        break;
                    case "N769":
                        str = "A lateral diagnosis is required.";
                        break;
                    case "N770":
                        str = "The adjustment request received from the provider has been processed.Your original claim has been adjusted based on the information received.";
                        break;
                    case "N771":
                        str = "Alert: Under Federal law you cannot charge more than the limiting charge amount.";
                        break;
                    case "N772":
                        str = "Alert: Rebill urgent / emergent and ancillary services separately.";
                        break;
                    case "N773":
                        str = "Drug supplied not obtained from specialty vendor.";
                        break;
                    case "N774":
                        str = "Alert: Refer to your Third Party Processor Agreement for specific information on fees associated with this payment type.";
                        break;
                    case "N775":
                        str = "Payment adjusted based on x - ray radiograph on film.";
                        break;
                    case "N776":
                        str = "This service is not a covered Telehealth service.";
                        break;
                    case "N777":
                        str = "Missing Assignment of Benefits Indicator.";
                        break;
                    case "N778":
                        str = "Missing Primary Care Physician Information.";
                        break;
                    case "N779":
                        str = "Replacement / Void claims cannot be submitted until the original claim has finalized.Please resubmit once payment or denial is received.";
                        break;
                    case "N780":
                        str = "Missing / incomplete / invalid end therapy date.";
                        break;
                    case "N781":
                        str = "Alert: No deductible may be collected as patient is a Medicaid / Qualified Medicare Beneficiary.Review your records for any wrongfully collected deductible.";
                        break;
                    case "N783":
                        str = "Alert: No co-payment may be collected as patient is a Medicaid / Qualified Medicare Beneficiary. Review your records for any wrongfully collected co - payment.";
                        break;
                    case "N784":
                        str = "Missing comprehensive procedure code.";
                        break;
                    case "N785":
                        str = "Missing current radiology film / images.";
                        break;
                    case "N786":
                        str = "Benefit limitation for the orthodontic active and / or retention phase of treatment.";
                        break;
                    case "N788":
                        str = "The third party administrator / review organization did not receive the requested information.";
                        break;
                    case "N787":
                        str = "Alert: Under 42 CFR 410.43, an eligible Partial Hospitalization Program(PHP) patient / beneficiary requires a minimum of 20 hours of PHP services per week, as evidenced in the plan of care.PHP services must be furnished in accordance with the plan of care.";
                        break;
                    case "N699":
                        str = "Payment adjusted based on the Physician Quality Reporting System (PQRS) Incentive Program.";
                        break;

                    case "N620":
                        str = "This procedure code is for quality reporting/informational purposes only.";
                        break;

                    case "N650":
                        str = "This policy was not in effect for this date of loss. No coverage is available.";
                        break;

                    case "N572":
                        str = "This procedure is not payable unless appropriate non-payable reporting codes and associated modifiers are submitted.";
                        break;
                    case "N522":
                        str = "Duplicate of a claim processed, or to be processed, as a crossover claim.";
                        break;
                    case "N535":
                        str = "Payment is adjusted when procedure is performed in this place of service based on the submitted procedure code and place of service.";
                        break;
                    case "N700":
                        str = "Payment adjusted based on the Electronic Health Records (EHR) Incentive Program.";
                        break;

                    case "N706":
                        str = "Missing documentation.";
                        break;

                    case "N479":
                        str = "Missing Explanation of Benefits (Coordination of Benefits or Medicare Secondary Payer).";
                        break;

                    case "N782":
                        str = "No coinsurance may be collected as patient is a Medicaid/Qualified Medicare Beneficiary. Review your records for any wrongfully collected coinsurance.";
                        break;

                    default:
                        str = "### Unknown Remittance Remark Code ###";
                        break;
                }
                return str;
            }
            catch (Exception)
            {
                return str;
            }
        }

        private string ST01(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if ((str3 != null) && (str3 == "004010X091"))
            {
                str = "Draft Standards Approved for Publication by ASC X12 Procedures Review Board through October 1997, as published in this implementation guide.";
            }
            else
            {
                return "### Unknown ST01 ###";
            }
            return str;
        }

        private string SVC(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "SVC")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4, 5, 6, 7 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    string[] strArray2;
                    int num3;
                    string[] strArray3;
                    int index = numArray[i];
                    if ((index >= strArray.Length) || (strArray[index].Length <= 0))
                    {
                        continue;
                    }
                    switch (index)
                    {
                        case 1:
                            strArray2 = strArray[index].Split(new char[] { MDVUtility.D_S_ELMT });
                            num3 = 0;
                            goto Label_024C;

                        case 2:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Line Item Charge Amount: ");
                                builder.Append(strArray[index]);
                                this.rowCache.FIELD_REMIT_CHARGE_AMOUNT = Convert.ToDecimal(strArray[index]);
                                continue;
                            }
                        case 3:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Line Item Provider Payment Amount: ");
                                builder.Append(strArray[index]);
                                this.rowCache.FIELD_REMIT_PAID_AMOUNT = Convert.ToDecimal(strArray[index]);
                                continue;
                            }
                        case 4:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("NUBC: ");
                                builder.Append(strArray[index]);
                                continue;
                            }
                        case 5:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Units of Service Paid Count: ");
                                builder.Append(strArray[index]);
                                this.rowCache.FIELD_REMIT_SERVICE_QTY = strArray[index];
                                continue;
                            }
                        case 6:
                            strArray3 = strArray[index].Split(new char[] { MDVUtility.D_S_ELMT });
                            num3 = 0;
                            goto Label_0435;

                        case 7:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Original Units of Service Count: ");
                                builder.Append(strArray[index]);
                                this.rowCache.FIELD_REMIT_SERVICE_ORG_QTY = strArray[index];
                                continue;
                            }
                        default:
                            goto Label_0482;
                    }
                Label_00E4:
                    switch (num3)
                    {
                        case 0:
                            builder.Append(this.SVC01_1(strArray2[num3]));
                            builder.Append(": ");
                            break;

                        case 1:
                            builder.Append(strArray2[num3]);
                            if (this.rowCache.FIELD_REMIT_HCPCS.ToString().Trim() != string.Empty)
                            {
                                this.rowCache = this.GetNewCacheRow();
                            }
                            this.rowCache.FIELD_REMIT_HCPCS = strArray2[num3];
                            break;

                        default:
                            if ((num3 > 1) & (num3 < 6))
                            {
                                builder.Append("\t");
                                builder.Append(strArray2[num3]);
                                if (this.rowCache.FIELD_REMIT_MODS.ToString() == "")
                                {
                                    this.rowCache.FIELD_REMIT_MODS = strArray2[num3];
                                }
                                else
                                {
                                    this.rowCache.FIELD_REMIT_MODS = this.rowCache.FIELD_REMIT_MODS.ToString() + "," + strArray2[num3];
                                }
                            }
                            else if (num3 == 6)
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Description: ");
                                builder.Append(strArray2[num3]);
                            }
                            break;
                    }
                    num3++;
                Label_024C:
                    if (num3 <= (strArray2.Length - 1))
                    {
                        goto Label_00E4;
                    }
                    continue;
                Label_0367:
                    if (num3 == 0)
                    {
                        builder.Append(this.SVC06_1(strArray3[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        this.rowCache.FIELD_REMIT_COMPOSITE_HCPCS = strArray3[num3];
                        builder.Append(strArray3[num3]);
                    }
                    else if ((num3 > 1) & (num3 < 6))
                    {
                        builder.Append("\t");
                        builder.Append(strArray3[num3]);
                    }
                    else if (num3 == 6)
                    {
                        builder.Append(Environment.NewLine);
                        builder.Append("Description: ");
                        builder.Append(strArray3[num3]);
                    }
                    num3++;
                Label_0435:
                    if (num3 <= (strArray3.Length - 1))
                    {
                        goto Label_0367;
                    }
                    continue;
                Label_0482:
                    builder.Append("## Error ## ");
                    builder.Append(strArray[index]);
                    builder.Append(Environment.NewLine);
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string SVC01_1(string Code)
        {
            switch (Code)
            {
                case "AD":
                    return "American Dental Association Codes";

                case "ER":
                    return "Jurisdiction Specific Procedure and Supply Codes 1403 This is specific to Workman’s Compensation Claims.";

                case "HC":
                    return "HCPCS Codes";

                case "ID":
                    return "International Classification of Diseases Clinical Modification (ICD-9-CM) - Procedure";

                case "IV":
                    return "Home Infusion EDI Coalition (HIEC) Product/Service Code";

                case "HP":
                    return " Health Insurance Prospective Payment System (HIPPS) Skilled Nursing Facility Rate Code";

                case "N4":
                    return "National Drug Code in 5-4-2 Format";

                case "NU":
                    return "National Uniform Billing Committee (NUBC) UB92 Codes";

                case "RB":
                    return "National Uniform Billing Committee (NUBC) UB82 Codes";

                case "ZZ":
                    return "Mutually Defined";
            }
            return "### Unknown SVC01 - 1 ###";
        }

        private string SVC06_1(string Code)
        {
            switch (Code)
            {
                case "AD":
                    return "American Dental Association Codes";

                case "ER":
                    return "Jurisdiction Specific Procedure and Supply Codes 1403 This is specific to Workman’s Compensation Claims.";

                case "HC":
                    return "Health Care Financing Administration Common Procedural Coding System (HCPCS) Codes";

                case "ID":
                    return "International Classification of Diseases Clinical Modification (ICD-9-CM) - Procedure";

                case "IV":
                    return "Home Infusion EDI Coalition (HIEC) Product/Service Code";

                case "AH":
                    return "Origination Fee";

                case "N4":
                    return "National Drug Code in 5-4-2 Format";

                case "NU":
                    return "National Uniform Billing Committee (NUBC) UB92 Codes";

                case "RB":
                    return "National Uniform Billing Committee (NUBC) UB82 Codes";

                case "ZZ":
                    return "Mutually Defined";
            }
            return "### Unknown SVC06 - 1 ###";
        }

        private string TA104(string Code)
        {
            switch (Code)
            {
                case "A":
                    return "The Transmitted Interchange Control Structure Header and Trailer Have Been Received and Have No Errors.";

                case "E":
                    return "The Transmitted Interchange Control Structure Header and Trailer Have Been Received and Are Accepted But Errors Are Noted. This Means the Sender Must Not Resend This Data.";

                case "R":
                    return "The Transmitted Interchange Control Structure Header and Trailer are Rejected Because of Errors.";
            }
            return "### Unknown TA104 ###";
        }

        private string TA105(string CODE)
        {
            switch (CODE)
            {
                case "000":
                    return "No error";

                case "001":
                    return "The Interchange Control Number in the Header and Trailer Do Not Match. The Value From the Header is Used in the Acknowledgment.";

                case "002":
                    return "This Standard as Noted in the Control Standards Identifier is Not Supported.";

                case "003":
                    return "This Version of the Controls is Not Supported";

                case "004":
                    return "The Segment Terminator is Invalid";

                case "006":
                    return "Invalid Interchange Sender ID";

                case "007":
                    return "Invalid Interchange ID Qualifier for Receiver";

                case "008":
                    return "Invalid Interchange Receiver ID";

                case "009":
                    return "Unknown Interchange Receiver ID";

                case "010":
                    return "Invalid Authorization Information Qualifier Value";

                case "011":
                    return "Invalid Authorization Information Value";

                case "012":
                    return "Invalid Security Information Qualifier Value";

                case "014":
                    return "Invalid Interchange Date Value";

                case "015":
                    return "Invalid Interchange Time Value";

                case "016":
                    return "Invalid Interchange Standards Identifier Value";

                case "017":
                    return "Invalid Interchange Version ID Value";

                case "018":
                    return "Invalid Interchange Control Number Value";

                case "019":
                    return "Invalid Acknowledgment Requested Value";

                case "020":
                    return "Invalid Test Indicator Value";

                case "021":
                    return "Invalid Number of Included Groups Value";

                case "022":
                    return "Invalid Control Structure";

                case "023":
                    return "Improper (Premature) End-of-File (Transmission)";

                case "024":
                    return "Invalid Interchange Content (e.g., Invalid GS Segment)";

                case "025":
                    return "Duplicate Interchange Control Number";

                case "026":
                    return "Invalid Data arrElmt Separator";

                case "027":
                    return "Invalid Component arrElmt Separator";

                case "028":
                    return "Invalid Delivery Date in Deferred Delivery Request";

                case "029":
                    return "Invalid Delivery Time in Deferred Delivery Request";

                case "030":
                    return "Invalid Delivery Time Code in Deferred Delivery Request";

                case "031":
                    return "Invalid Grade of Service Code";
            }
            return "### Unknown TA105 ###";
        }

        private string TRN(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "TRN")
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 1, 2, 3, 4 };
                int index = 0;
                for (index = 0; index <= (numArray.Length - 1); index++)
                {
                    int num2 = numArray[index];
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.TRN01(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 2:
                                builder.Append(strArray[num2]);
                                this.rowERA.FIELD_ERA835_CHEQUE_NO = strArray[num2];
                                this.rowBatch.FIELD_BATCH_CHEQUE_NO = strArray[num2];
                                break;

                            case 3:
                                builder.Append("\r\n");
                                builder.Append("Payer Identifier: ");
                                builder.Append(strArray[num2]);
                                break;

                            case 4:
                                builder.Append('\t');
                                builder.Append(strArray[num2]);
                                break;
                        }
                    }
                }
                builder.Append("\r\n");
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string TRN01(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "1"))
            {
                return "Current Transaction Trace Numbers";
            }
            return "### Unknown TRN01 ###";
        }

        private string TS2(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "TS2")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                    0x11, 0x12, 0x13
                 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append("Total DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Federal Specific Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Hospital Specific Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Disproportionate Share Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 5:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Capital Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Indirect Medical Education Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Outlier Day Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 8:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Day Outlier Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Cost Outlier Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 10:
                                builder.Append(Environment.NewLine);
                                builder.Append("Average DRG Length of Stay: ");
                                builder.Append(strArray[index]);
                                break;

                            case 11:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Discharge Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Cost Report Day Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Covered Day Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 14:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Noncovered Day Count: ");
                                builder.Append(strArray[index]);
                                break;

                          

                            case 0x13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total PPS DSH DRG Amount: ");
                                builder.Append(strArray[index]);
                                break;
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string TS3(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "TS3")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                int[] numArray = new int[] { 
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10, 
                    0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18
                 };
                for (int i = 0; i <= (numArray.Length - 1); i++)
                {
                    int index = numArray[i];
                    if ((index < strArray.Length) && (strArray[index].Length > 0))
                    {
                        switch (index)
                        {
                            case 1:
                                builder.Append("Provider Identifier: ");
                                builder.Append(strArray[index]);
                                break;

                            case 2:
                                builder.Append(Environment.NewLine);
                                builder.Append("Facility Type: ");
                                break;

                            case 3:
                                builder.Append(Environment.NewLine);
                                builder.Append("Fiscal Period Date: ");
                                builder.Append(MDVUtility.StringToDate(strArray[index]).ToShortDateString());
                                break;

                            case 4:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Claim Count: ");
                                builder.Append(strArray[index]);
                                break;

                            case 5:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Claim Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 6:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Covered Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 7:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Noncovered Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 8:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Denied Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 9:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Provider Payment Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 10:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Interest Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 11:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Contractual Adjustment Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 12:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Gramm-Rudman Reduction Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 13:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total MSP Payer Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 14:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Blood Deductible Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 15:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Non-Lab Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x10:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total Coinsurance Amount: ");
                                builder.Append(strArray[index]);
                                break;

                            case 0x11:
                                builder.Append(Environment.NewLine);
                                builder.Append("Total HCPCS Reported Charge Amount: ");
                                builder.Append(strArray[index]);
                                break;

                           
                        }
                    }
                }
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StructReportId
        {
            public string ID;
            public string Status;
            public string StatusDescription;
        }
    }
}

