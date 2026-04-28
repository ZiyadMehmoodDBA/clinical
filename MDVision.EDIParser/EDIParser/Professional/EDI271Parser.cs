namespace EDIParser.Professional
{
    using EDIParser;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using MDVision.Common.Utilities;

    public class EDI271Parser
    {
        private long BenefitsId = 0L;
        private IContainer components;
        private decimal Copay = 0M;
        private char D_S_S_ELMT = '^';
        private decimal Deductible = 0M;
        private DS271 ds271Parser = new DS271();
        private DS271 ds271tempParser = new DS271();
        private string DTP02_Value;
        private string EB01_Value;
        private string EB02_Value;
        private bool Is_AAA03_Checked = false;
        private string FirstEB01_Value = "";
        private string FirstEB03_Value = "";
        private string FirstAAA01_value = "";
        private string FirstAAA03_value = "";
       
        private bool EB03_Equal = false;
        private StringBuilder FinalStr271;
        private string III01_Value;
        private string MPI06_Value;
        private long NameId = 0L;
        private DS271.EDI271BenefitsRow rowBenefits;
        private DS271.EDI271BenefitsDetailRow rowBenefitsDetail;
        private DS271.EDI271HeaderRow rowHeader;
        private DS271.EDI271NamesRow rowName;
        private string ServiceTypeCode = string.Empty;

        public EDI271Parser()
        {
            this.InitializeComponent();
        }

        private string AAA(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "AAA")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                {
                                    builder.Append("Valid Request Indicator: ");
                                    builder.Append(this.AAA01(strArray[num2]));
                                    this.rowName.AAA01 = strArray[num2];
                                    if (string.IsNullOrEmpty(FirstAAA01_value))
                                    {
                                        FirstAAA01_value = strArray[num2];
                                    }
                                    continue;
                                }
                            case 2:
                                {
                                    builder.Append(strArray[num2]);
                                    this.rowName.AAA02 = strArray[num2];
                                    continue;
                                }
                            case 3:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Reject Reason: ");
                                    builder.Append(this.AAA03(strArray[num2]));
                                    if (string.IsNullOrEmpty(FirstAAA03_value) && Is_AAA03_Checked == false)
                                    {
                                        FirstAAA03_value = strArray[num2];
                                        Is_AAA03_Checked = true;
                                    }
                                    continue;
                                }
                            case 4:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Follow-up Action: ");
                                    builder.Append(this.AAA04(strArray[num2]));
                                    continue;
                                }
                        }
                        builder.Append(Environment.NewLine);
                        builder.Append("## this AAA segment not in use ##");
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

        private string AAA01(string Code)
        {
            
            return "### Unknown AAA01 ###";
        }

        private string AAA03(string Code)
        {
            string str = "Unknown";
            
            return str;
        }

        private string AAA04(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "C":
                    str = "Please Correct and Resubmit";
                    break;

                case "N":
                    str = "Resubmission Not Allowed";
                    break;

                case "P":
                    str = "Please Resubmit Original Transaction";
                    break;

                case "R":
                    str = "Resubmission Allowed";
                    break;

                case "S":
                    str = "Do Not Resubmit; Inquiry Initiated to a Third Party";
                    break;

                case "W":
                    str = "Please Wait 30 Days and Resubmit";
                    break;

                case "X":
                    str = "Please Wait 10 Days and Resubmit";
                    break;

                case "Y":
                    str = "Do Not Resubmit; We Will Hold Your Request and Respond Again Shortly";
                    break;

                default:
                    str = "### Unknown AAA04 ###";
                    break;
            }
            this.rowName.AAA04 = str;
            return str;
        }

        private string BHT(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "BHT")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
            
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string BHT01(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "0022"))
            {
                return "Information Source, Information Receiver,Subscriber, Dependent";
            }
            return "### Unknown BHT01 ###";
        }

        private string BHT02(string Code)
        {
            string str3 = Code;
            if ((str3 != null) && (str3 == "11"))
            {
                return "Response";
            }
            return "### Unknown BHT02 ###";
        }

        private string DMG(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "DMG")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
           
                builder.Append(Environment.NewLine);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string DMG03(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "F"))
                {
                    if (str3 == "M")
                    {
                        str = "Male";
                        goto Label_0055;
                    }
                    if (str3 == "U")
                    {
                        str = "Unknown";
                        goto Label_0055;
                    }
                }
                else
                {
                    str = "Female";
                    goto Label_0055;
                }
            }
            str = "### Unknown DMG03 ###";
        Label_0055:
            this.rowName.DMG03 = str;
            return str;
        }

        private string DTP(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "DTP")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3 })
                {
                    DateTime time;
                  
                Label_0100:
                    this.rowName.DTP02 = this.DTP02_Value;
                    continue;
                Label_0170:
                    this.rowName.DTP03 = time.ToShortDateString();
                    continue;
                Label_0189:
                    if (this.DTP02_Value == "RD8")
                    {
                        string[] strArray2 = strArray[num2].Split(new char[] { '-' });
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    time = MDVUtility.StringToDate(strArray[0]);
                                    builder.Append(time.ToShortDateString());
                                    if (this.IsBenefitsDTP())
                                    {
                                        this.rowBenefits.DTP03 = time.ToShortDateString();
                                    }
                                    else
                                    {
                                        this.rowName.DTP03 = time.ToShortDateString();
                                    }
                                    break;

                                case 1:
                                    builder.Append("-");
                                    time = MDVUtility.StringToDate(strArray[1]);
                                    builder.Append(time.ToShortDateString());
                                    if (this.IsBenefitsDTP())
                                    {
                                        this.rowBenefits.DTP03 = this.rowBenefits.DTP03 + " - " + time.ToShortDateString();
                                    }
                                    else
                                    {
                                        this.rowName.DTP03 = this.rowName.DTP03 + " - " + time.ToShortDateString();
                                    }
                                    break;
                            }
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

        private string DTP01(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "096":
                    str = "Discharge";
                    break;

                case "102":
                    str = "Issue";
                    break;

                case "152":
                    str = "Effective Date of Change";
                    break;

                case "291":
                    str = "Plan";
                    break;

                case "307":
                    str = "Eligibility";
                    break;

                case "318":
                    str = "Added";
                    break;

                case "340":
                    str = "Consolidated Omnibus Budget Reconciliation Act (COBRA) Begin";
                    break;

                case "341":
                    str = "Consolidated Omnibus Budget Reconciliation Act (COBRA) End";
                    break;

                case "342":
                    str = "Premium Paid to Date Begin";
                    break;

                case "343":
                    str = "Premium Paid to Date End";
                    break;

                case "346":
                    str = "Plan Begin";
                    break;

                case "347":
                    str = "Plan End";
                    break;

                case "356":
                    str = "Eligibility Begin";
                    break;

                case "357":
                    str = "Eligibility End";
                    break;

                case "382":
                    str = "Enrollment";
                    break;

                case "435":
                    str = "Admission";
                    break;

                case "442":
                    str = "Date of Death";
                    break;

                case "458":
                    str = "Certification";
                    break;

                case "472":
                    str = "Service";
                    break;

                case "539":
                    str = "Policy Effective";
                    break;

                case "540":
                    str = "Policy Expiration";
                    break;

                case "636":
                    str = "Date of Last Update";
                    break;

                case "771":
                    str = "Status";
                    break;

                case "193":
                    str = "Period Start";
                    break;

                case "194":
                    str = "Period End";
                    break;

                case "198":
                    str = "Completion";
                    break;

                case "290":
                    str = "Coordination of Benefits";
                    break;

                case "292":
                    str = "Benefit";
                    break;

                case "295":
                    str = "Primary Care Provider";
                    break;

                case "304":
                    str = "Latest Visit or Consultation";
                    break;

                case "348":
                    str = "Benefit Begin";
                    break;

                case "349":
                    str = "Benefit End";
                    break;

                default:
                    str = "### Unknown DTP01 ###";
                    break;
            }
            if (this.IsBenefitsDTP())
            {
                this.rowBenefits.DTP01 = str;
                return str;
            }
            this.rowName.DTP01 = str;
            return str;
        }

        private string EB(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "EB")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })
                {
                    string[] strArray2;
                    int num3;
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                this.rowBenefits = this.GetNew271BenefitsRow();
                                builder.Append(Environment.NewLine);
                                builder.Append("Eligibility or Benefit Information: ");
                                builder.Append(this.EB01(strArray[num2]));
                                if (string.IsNullOrEmpty(FirstEB01_Value))
                                {
                                    FirstEB01_Value = strArray[num2];
                                }
                                continue;
                            }
                        case 2:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Coverage Level: ");
                                builder.Append(this.EB02(strArray[num2]));
                                this.EB02_Value = strArray[num2];
                                continue;
                            }
                        case 3:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Service Type: ");
                                builder.Append(this.EB03(strArray[num2]));
                                if (string.IsNullOrEmpty(FirstEB03_Value))
                                {
                                    FirstEB03_Value = strArray[num2];
                                }
                                continue;
                            }
                        case 4:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Insurance Type: ");
                                builder.Append(this.EB04(strArray[num2]));
                                continue;
                            }
                        case 5:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Plan Coverage Description: ");
                                builder.Append(strArray[num2]);
                                this.rowBenefits.EB05 = strArray[num2];
                                continue;
                            }
                        case 6:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Time Period Qualifier: ");
                                builder.Append(this.EB06(strArray[num2]));
                                continue;
                            }
                        case 7:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Benefit Amount: ");
                                builder.Append(strArray[num2]);
                                this.rowBenefits.EB07 = strArray[num2];
                                
                                //Co-payment and Deductible amount.
                                if (EB01_Value == "B" && (this.EB02_Value == "IND") && EB03_Equal)
                                    Copay += MDVUtility.ToDecimal(strArray[num2]);
                                else if (EB01_Value == "C" && (this.EB02_Value == "IND") && EB03_Equal)
                                    Deductible += MDVUtility.ToDecimal(strArray[num2]);
                                //if ((this.EB01_Value == "C") && (this.EB02_Value == "IND") && this.EB03_Equal)
                                //{
                                //    this.Deductible += Convert.ToDecimal(strArray[num2]);
                                //}
                                //continue;

                                //if (!(this.EB01_Value == "B") || !this.EB03_Equal)
                                //{
                                //    break;
                                //}
                                //this.Copay += Convert.ToDecimal(strArray[num2]);
                                continue;
                            }
                        case 8:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Benefit Percent: ");
                                builder.Append(strArray[num2]);
                                this.rowBenefits.EB08 = strArray[num2];
                                continue;
                            }
                        case 9:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append(this.EB09(strArray[num2]));
                                builder.Append(": ");
                                continue;
                            }
                        case 10:
                            {
                                builder.Append(strArray[num2]);
                                this.rowBenefits.EB10 = strArray[num2];
                                continue;
                            }
                        case 11:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Authorization or Certification Indicator: ");
                                builder.Append(this.EB11(strArray[num2]));
                                continue;
                            }
                        case 12:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("In Plan Network Indicator: ");
                                builder.Append(this.EB12(strArray[num2]));
                                continue;
                            }
                        case 13:
                            builder.Append(Environment.NewLine);
                            strArray2 = strArray[num2].Split(new char[] { MDVUtility.D_S_ELMT });
                            num3 = 0;
                            num3 = 0;
                            goto Label_05C4;

                        default:
                            goto Label_05DC;
                    }
                   
                Label_03DA:
                    if (num3 == 0)
                    {
                        builder.Append(this.EB13_1(strArray2[num3]));
                        builder.Append(": ");
                    }
                    else if (num3 == 1)
                    {
                        builder.Append("\t");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = strArray2[num3];
                    }
                    else if (num3 == 2)
                    {
                        builder.Append("\t");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = this.rowBenefits.EB13_2 + strArray2[num3];
                    }
                    else if (num3 == 3)
                    {
                        builder.Append("\t");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = this.rowBenefits.EB13_2 + strArray2[num3];
                    }
                    else if (num3 == 4)
                    {
                        builder.Append("\t");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = this.rowBenefits.EB13_2 + strArray2[num3];
                    }
                    else if (num3 == 5)
                    {
                        builder.Append("\t");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = this.rowBenefits.EB13_2 + strArray2[num3];
                    }
                    else if (num3 == 6)
                    {
                        builder.Append(Environment.NewLine);
                        builder.Append("Description: ");
                        builder.Append(strArray2[num3]);
                        this.rowBenefits.EB13_2 = this.rowBenefits.EB13_2 + strArray2[num3];
                    }
                    num3++;
                Label_05C4:
                    if (num3 <= (strArray2.Length - 1))
                    {
                        goto Label_03DA;
                    }
                    continue;
                Label_05DC:
                    builder.Append(Environment.NewLine);
                    builder.Append("## this EB segment not in use ##");
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

        private string EB01(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "1":
                    str = "Active Coverage";
                    break;

                case "2":
                    str = "Active - Full Risk Capitation";
                    break;

                case "3":
                    str = "Active - Services Capitated";
                    break;

                case "4":
                    str = "Active - Services Capitated to Primary Care Physician";
                    break;

                case "5":
                    str = "Active - Pending Investigation";
                    break;

                case "6":
                    str = "Inactive";
                    break;

                case "7":
                    str = "Inactive - Pending Eligibility Update";
                    break;

                case "8":
                    str = "Inactive - Pending Investigation";
                    break;

                case "A":
                    str = "Co-Insurance";
                    break;

                case "B":
                    str = "Co-Payment";
                    break;

                case "C":
                    str = "Deductible";
                    break;

                case "CB":
                    str = "Coverage Basis";
                    break;

                case "D":
                    str = "Benefit Description";
                    break;

                case "E":
                    str = "Exclusions";
                    break;

                case "F":
                    str = "Limitations";
                    break;

                case "G":
                    str = "Out of Pocket (Stop Loss)";
                    break;

                case "H":
                    str = "Unlimited";
                    break;

                case "I":
                    str = "Non-Covered";
                    break;

                case "J":
                    str = "Cost Containment";
                    break;

                case "K":
                    str = "Reserve";
                    break;

                case "L":
                    str = "Primary Care Provider";
                    break;

                case "M":
                    str = "Pre-existing Condition";
                    break;

                case "MC":
                    str = "Managed Care Coordinator";
                    break;

                case "N":
                    str = "Services Restricted to Following Provider";
                    break;

                case "O":
                    str = "Not Deemed a Medical Necessity";
                    break;

                case "P":
                    str = "Benefit Disclaimer";
                    break;

                case "Q":
                    str = "Second Surgical Opinion Required";
                    break;

                case "R":
                    str = "Other or Additional Payor";
                    break;

                case "S":
                    str = "Prior Year(s) History";
                    break;

                case "T":
                    str = "Card(s) Reported Lost/Stolen";
                    break;

                case "U":
                    str = "Contact Following Entity for Eligibility or Benefit Information";
                    break;

                case "V":
                    str = "Cannot Process";
                    break;

                case "W":
                    str = "Other Source of Data";
                    break;

                case "X":
                    str = "Health Care Facility";
                    break;

                case "Y":
                    str = "Spend Down";
                    break;

                default:
                    str = "### Unknown EB01 ###";
                    break;
            }
            this.rowBenefits.EB01 = str;
            return str;
        }

        private string EB02(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "CHD":
                    str = "Children Only";
                    break;

                case "DEP":
                    str = "Dependents Only";
                    break;

                case "ECH":
                    str = "Employee and Children";
                    break;

                case "EMP":
                    str = "Employee Only";
                    break;

                case "ESP":
                    str = "Employee and Spouse";
                    break;

                case "FAM":
                    str = "Family";
                    break;

                case "IND":
                    str = "Individual";
                    break;

                case "SPC":
                    str = "Spouse and Children";
                    break;

                case "SPO":
                    str = "Spouse Only";
                    break;

                default:
                    str = "### Unknown EB02 ###";
                    break;
            }
            this.rowBenefits.EB02 = str;
            return str;
        }

        private string EB03(string Code)
        {
            string str = "";
            string[] source = Code.Split(new char[] { this.D_S_S_ELMT });

            foreach (string str2 in source)
            {
                switch (str2)
                {
                    case "1":
                        str = str + "Medical Care, ";
                        break;

                    case "2":
                        str = str + "Surgical, ";
                        break;

                    case "3":
                        str = str + "Consultation, ";
                        break;

                    case "4":
                        str = str + "Diagnostic X-Ray, ";
                        break;

                    case "5":
                        str = str + "Diagnostic Lab, ";
                        break;

                    case "6":
                        str = str + "Radiation Therapy, ";
                        break;

                    case "7":
                        str = str + "Anesthesia, ";
                        break;

                    case "8":
                        str = str + "Surgical Assistance, ";
                        break;

                    case "9":
                        str = str + "Other Medical, ";
                        break;

                    case "10":
                        str = str + "Blood Charges, ";
                        break;

                    case "11":
                        str = str + "Used Durable Medical Equipment, ";
                        break;

                    case "12":
                        str = str + "Durable Medical Equipment Purchase, ";
                        break;

                    case "13":
                        str = str + "Ambulatory Service Center Facility, ";
                        break;

                    case "14":
                        str = str + "Renal Supplies in the Home, ";
                        break;

                    case "15":
                        str = str + "Alternate Method Dialysis, ";
                        break;

                    case "16":
                        str = str + "Chronic Renal Disease (CRD) Equipment, ";
                        break;

                    case "17":
                        str = str + "Pre-Admission Testing, ";
                        break;

                    case "18":
                        str = str + "Durable Medical Equipment Rental, ";
                        break;

                    case "19":
                        str = str + "Pneumonia Vaccine, ";
                        break;

                    case "20":
                        str = str + "Second Surgical Opinion, ";
                        break;

                    case "21":
                        str = str + "Third Surgical Opinion, ";
                        break;

                    case "22":
                        str = str + "Social Work, ";
                        break;

                    case "23":
                        str = str + "Diagnostic Dental, ";
                        break;

                    case "24":
                        str = str + "Periodontics, ";
                        break;

                    case "25":
                        str = str + "Restorative, ";
                        break;

                    case "26":
                        str = str + "Endodontics, ";
                        break;

                    case "27":
                        str = str + "Maxillofacial Prosthetics, ";
                        break;

                    case "28":
                        str = str + "Adjunctive Dental Services, ";
                        break;

                    case "30":
                        str = str + "Health Benefit Plan Coverage, ";
                        break;

                    case "32":
                        str = str + "Plan Waiting Period, ";
                        break;

                    case "33":
                        str = str + "Chiropractic, ";
                        break;

                    case "34":
                        str = str + "Chiropractic Office Visits, ";
                        break;

                    case "35":
                        str = str + "Dental Care, ";
                        break;

                    case "36":
                        str = str + "Dental Crowns, ";
                        break;

                    case "37":
                        str = str + "Dental Accident, ";
                        break;

                    case "38":
                        str = str + "Orthodontics, ";
                        break;

                    case "39":
                        str = str + "Prosthodontics, ";
                        break;

                    case "40":
                        str = str + "Oral Surgery, ";
                        break;

                    case "41":
                        str = str + "Routine (Preventive) Dental, ";
                        break;

                    case "42":
                        str = str + "Home Health Care, ";
                        break;

                    case "43":
                        str = str + "Home Health Prescriptions, ";
                        break;

                    case "44":
                        str = str + "Home Health Visits, ";
                        break;

                    case "45":
                        str = str + "Hospice, ";
                        break;

                    case "46":
                        str = str + "Respite Care, ";
                        break;

                    case "47":
                        str = str + "Hospital, ";
                        break;

                    case "48":
                        str = str + "Hospital - Inpatient, ";
                        break;

                    case "49":
                        str = str + "Hospital - Room and Board, ";
                        break;

                    case "50":
                        str = str + "Hospital - Outpatient, ";
                        break;

                    case "51":
                        str = str + "Hospital - Emergency Accident, ";
                        break;

                    case "52":
                        str = str + "Hospital - Emergency Medical, ";
                        break;

                    case "53":
                        str = str + "Hospital - Ambulatory Surgical, ";
                        break;

                    case "54":
                        str = str + "Long Term Care, ";
                        break;

                    case "55":
                        str = str + "Major Medical, ";
                        break;

                    case "56":
                        str = str + "Medically Related Transportation, ";
                        break;

                    case "57":
                        str = str + "Air Transportation, ";
                        break;

                    case "58":
                        str = str + "Cabulance, ";
                        break;

                    case "59":
                        str = str + "Licensed Ambulance, ";
                        break;

                    case "60":
                        str = str + "General Benefits, ";
                        break;

                    case "61":
                        str = str + "In-vitro Fertilization, ";
                        break;

                    case "62":
                        str = str + "MRI/CAT Scan, ";
                        break;

                    case "63":
                        str = str + "Donor Procedures, ";
                        break;

                    case "64":
                        str = str + "Acupuncture, ";
                        break;

                    case "65":
                        str = str + "Newborn Care, ";
                        break;

                    case "66":
                        str = str + "Pathology, ";
                        break;

                    case "67":
                        str = str + "Smoking Cessation, ";
                        break;

                    case "68":
                        str = str + "Well Baby Care, ";
                        break;

                    case "69":
                        str = str + "Maternity, ";
                        break;

                    case "70":
                        str = str + "Transplants, ";
                        break;

                    case "71":
                        str = str + "Audiology Exam, ";
                        break;

                    case "72":
                        str = str + "Inhalation Therapy, ";
                        break;

                    case "73":
                        str = str + "Diagnostic Medical, ";
                        break;

                    case "74":
                        str = str + "Private Duty Nursing, ";
                        break;

                    case "75":
                        str = str + "Prosthetic Device, ";
                        break;

                    case "76":
                        str = str + "Dialysis, ";
                        break;

                    case "77":
                        str = str + "Otological Exam, ";
                        break;

                    case "78":
                        str = str + "Chemotherapy, ";
                        break;

                    case "79":
                        str = str + "Allergy Testing, ";
                        break;

                    case "80":
                        str = str + "Immunizations, ";
                        break;

                    case "81":
                        str = str + "Routine Physical, ";
                        break;

                    case "82":
                        str = str + "Family Planning, ";
                        break;

                    case "83":
                        str = str + "Infertility, ";
                        break;

                    case "84":
                        str = str + "Abortion, ";
                        break;

                    case "85":
                        str = str + "AIDS, ";
                        break;

                    case "86":
                        str = str + "Emergency Services, ";
                        break;

                    case "87":
                        str = str + "Cancer, ";
                        break;

                    case "88":
                        str = str + "Pharmacy, ";
                        break;

                    case "89":
                        str = str + "Free Standing Prescription Drug, ";
                        break;

                    case "90":
                        str = str + "Mail Order Prescription Drug, ";
                        break;

                    case "91":
                        str = str + "Brand Name Prescription Drug, ";
                        break;

                    case "92":
                        str = str + "Generic Prescription Drug, ";
                        break;

                    case "93":
                        str = str + "Podiatry, ";
                        break;

                    case "94":
                        str = str + "Podiatry - Office Visits, ";
                        break;

                    case "95":
                        str = str + "Podiatry - Nursing Home Visits, ";
                        break;

                    case "96":
                        str = str + "Professional (Physician), ";
                        break;

                    case "97":
                        str = str + "Anesthesiologist, ";
                        break;

                    case "98":
                        str = str + "Professional (Physician) Visit - Office, ";
                        break;

                    case "99":
                        str = str + "Professional (Physician) Visit - Inpatient, ";
                        break;

                    case "A0":
                        str = str + "Professional (Physician) Visit - Outpatient, ";
                        break;

                    case "A1":
                        str = str + "Professional (Physician) Visit - Nursing Home, ";
                        break;

                    case "A2":
                        str = str + "Professional (Physician) Visit - Skilled Nursing Facility, ";
                        break;

                    case "A3":
                        str = str + "Professional (Physician) Visit - Home, ";
                        break;

                    case "A4":
                        str = str + "Psychiatric, ";
                        break;

                    case "A5":
                        str = str + "Psychiatric - Room and Board, ";
                        break;

                    case "A6":
                        str = str + "Psychotherapy, ";
                        break;

                    case "A7":
                        str = str + "Psychiatric - Inpatient, ";
                        break;

                    case "A8":
                        str = str + "Psychiatric - Outpatient, ";
                        break;

                    case "A9":
                        str = str + "Rehabilitation, ";
                        break;

                    case "AA":
                        str = str + "Rehabilitation - Room and Board, ";
                        break;

                    case "AB":
                        str = str + "Rehabilitation - Inpatient, ";
                        break;

                    case "AC":
                        str = str + "Rehabilitation - Outpatient, ";
                        break;

                    case "AD":
                        str = str + "Occupational Therapy, ";
                        break;

                    case "AE":
                        str = str + "Physical Medicine, ";
                        break;

                    case "AF":
                        str = str + "Speech Therapy, ";
                        break;

                    case "AG":
                        str = str + "Skilled Nursing Care, ";
                        break;

                    case "AH":
                        str = str + "Skilled Nursing Care - Room and Board, ";
                        break;

                    case "AI":
                        str = str + "Substance Abuse, ";
                        break;

                    case "AJ":
                        str = str + "Alcoholism, ";
                        break;

                    case "AK":
                        str = str + "Drug Addiction, ";
                        break;

                    case "AL":
                        str = str + "Vision (Optometry), ";
                        break;

                    case "AM":
                        str = str + "Frames, ";
                        break;

                    case "AN":
                        str = str + "Routine Exam, ";
                        break;

                    case "AO":
                        str = str + "Lenses, ";
                        break;

                    case "AQ":
                        str = str + "Nonmedically Necessary Physical, ";
                        break;

                    case "AR":
                        str = str + "Experimental Drug Therapy, ";
                        break;

                    case "B1":
                        str = str + "Burn Care, ";
                        break;

                    case "B2":
                        str = str + "Brand Name Prescription Drug - Formulary, ";
                        break;

                    case "B3":
                        str = str + "Brand Name Prescription Drug - Non-Formulary, ";
                        break;

                    case "BA":
                        str = str + "Independent Medical Evaluation, ";
                        break;

                    case "BB":
                        str = str + "Partial Hospitalization (Psychiatric), ";
                        break;

                    case "BC":
                        str = str + "Day Care (Psychiatric), ";
                        break;

                    case "BD":
                        str = str + "Cognitive Therapy, ";
                        break;

                    case "BE":
                        str = str + "Massage Therapy, ";
                        break;

                    case "BF":
                        str = str + "Pulmonary Rehabilitation, ";
                        break;

                    case "BG":
                        str = str + "Cardiac Rehabilitation, ";
                        break;

                    case "BH":
                        str = str + "Pediatric, ";
                        break;

                    case "BI":
                        str = str + "Nursery, ";
                        break;

                    case "BJ":
                        str = str + "Skin, ";
                        break;

                    case "BK":
                        str = str + "Orthopedic, ";
                        break;

                    case "BL":
                        str = str + "Cardiac, ";
                        break;

                    case "BM":
                        str = str + "Lymphatic, ";
                        break;

                    case "BN":
                        str = str + "Gastrointestinal, ";
                        break;

                    case "BP":
                        str = str + "Endocrine, ";
                        break;

                    case "BQ":
                        str = str + "Neurology, ";
                        break;

                    case "BR":
                        str = str + "Eye, ";
                        break;

                    case "BS":
                        str = str + "Invasive Procedures, ";
                        break;

                    case "BT":
                        str = str + "Gynecological, ";
                        break;

                    case "BU":
                        str = str + "Obstetrical, ";
                        break;

                    case "BV":
                        str = str + "Obstetrical/Gynecological, ";
                        break;

                    case "BW":
                        str = str + "Mail Order Prescription Drug: Brand Name, ";
                        break;

                    case "BX":
                        str = str + "Mail Order Prescription Drug: Generic, ";
                        break;

                    case "BY":
                        str = str + "Physician Visit - Office: Sick, ";
                        break;

                    case "BZ":
                        str = str + "Physician Visit - Office: Well, ";
                        break;

                    case "C1":
                        str = str + "Coronary Care, ";
                        break;

                    case "CA":
                        str = str + "Private Duty Nursing - Inpatient, ";
                        break;

                    case "CB":
                        str = str + "Private Duty Nursing - Home, ";
                        break;

                    case "CC":
                        str = str + "Surgical Benefits - Professional (Physician), ";
                        break;

                    case "CD":
                        str = str + "Surgical Benefits - Facility, ";
                        break;

                    case "CE":
                        str = str + "Mental Health Provider - Inpatient, ";
                        break;

                    case "CF":
                        str = str + "Mental Health Provider - Outpatient, ";
                        break;

                    case "CG":
                        str = str + "Mental Health Facility - Inpatient, ";
                        break;

                    case "CH":
                        str = str + "Mental Health Facility - Outpatient, ";
                        break;

                    case "CI":
                        str = str + "Substance Abuse Facility - Inpatient, ";
                        break;

                    case "CJ":
                        str = str + "Substance Abuse Facility - Outpatient, ";
                        break;

                    case "CK":
                        str = str + "Screening X-ray, ";
                        break;

                    case "CL":
                        str = str + "Screening laboratory, ";
                        break;

                    case "CM":
                        str = str + "Mammogram, High Risk Patient, ";
                        break;

                    case "CN":
                        str = str + "Mammogram, Low Risk Patient, ";
                        break;

                    case "CO":
                        str = str + "Flu Vaccination, ";
                        break;

                    case "CP":
                        str = str + "Eyewear and Eyewear Accessories, ";
                        break;

                    case "CQ":
                        str = str + "Case Management,";
                        break;

                    case "DG":
                        str = str + "Dermatology,";
                        break;

                    case "DM":
                        str = str + "Durable Medical Equipment,";
                        break;

                    case "DS":
                        str = str + "Diabetic Supplies,";
                        break;

                    case "GF":
                        str = str + "Generic Prescription Drug - Formulary,";
                        break;

                    case "GN":
                        str = str + "Generic Prescription Drug - Non-Formulary,";
                        break;

                    case "GY":
                        str = str + "Allergy,";
                        break;

                    case "IC":
                        str = str + "Intensive Care,";
                        break;

                    case "MH":
                        str = str + "Mental Health,";
                        break;

                    case "NI":
                        str = str + "Neonatal Intensive Care,";
                        break;

                    case "ON":
                        str = str + "Oncology,";
                        break;

                    case "PT":
                        str = str + "Physical Therapy,";
                        break;

                    case "PU":
                        str = str + "Pulmonary,";
                        break;

                    case "RN":
                        str = str + "Renal,";
                        break;

                    case "RT":
                        str = str + "Residential Psychiatric Treatment,";
                        break;

                    case "TC":
                        str = str + "Transitional Care,";
                        break;

                    case "TN":
                        str = str + "Transitional Nursery Care,";
                        break;

                    case "UC":
                        str = str + "Urgent Care,";
                        break;

                    default:
                        str = str + "### Unknown EB03 ###,";
                        break;
                }
            }
            str = str.Remove(str.LastIndexOf(','), 1);
            if (this.rowBenefits != null)
            {
                //if (!(string.IsNullOrEmpty(this.ServiceTypeCode) || (!(this.EB01_Value == "1") || !source.Contains<string>(this.ServiceTypeCode))))
                //{
                //    this.rowHeader.IsEligible = true;
                //}

                //if (this.rowName.AAA01 == "N")
                //{
                //    this.rowHeader.IsEligible = "Waiting";
                //}
                //else if (this.rowName.AAA01 == "Y")
                //{
                //    if (!(string.IsNullOrEmpty(this.ServiceTypeCode)) || !source.Contains<string>(this.ServiceTypeCode) || this.EB01_Value == "1" || this.EB01_Value == "2" || this.EB01_Value == "3" || this.EB01_Value == "4" || this.EB01_Value == "5")
                //    {
                //        this.rowHeader.IsEligible = "Active";
                //    }
                //    else if (this.EB01_Value == "6" || this.EB01_Value == "7" || this.EB01_Value == "8")
                //    {
                //        this.rowHeader.IsEligible = "InActive";
                //    }
                //}
                //else
                //{
                //    if (!(string.IsNullOrEmpty(this.ServiceTypeCode)) || !source.Contains<string>(this.ServiceTypeCode) || this.EB01_Value == "1" || this.EB01_Value == "2" || this.EB01_Value == "3" || this.EB01_Value == "4" || this.EB01_Value == "5")
                //    {
                //        this.rowHeader.IsEligible = "Active";
                //    }
                //    else if (this.EB01_Value == "6" || this.EB01_Value == "7" || this.EB01_Value == "8")
                //    {
                //        this.rowHeader.IsEligible = "InActive";
                //    }
                //}


                if (!(string.IsNullOrEmpty(this.ServiceTypeCode) || !source.Contains<string>(this.ServiceTypeCode)))
                {
                    this.EB03_Equal = true;
                }
                else
                {
                    this.EB03_Equal = false;
                }
                this.rowBenefits.EB03 = str;
                this.rowBenefits.ServiceTypeCode = Code;
            }
            return str;
        }

        private string EB04(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "12":
                    str = "Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan";
                    break;

                case "13":
                    str = "Medicare Secondary End-Stage Renal Disease Beneficiary in the 12 month coordination period with an employer's group health plan";
                    break;

                case "14":
                    str = "Medicare Secondary, No-fault Insurance including Auto is Primary";
                    break;

                case "15":
                    str = "Medicare Secondary Worker’s Compensation";
                    break;

                case "16":
                    str = "Medicare Secondary Private Health Service (PHS)or Other Federal Agency";
                    break;

                case "41":
                    str = "Medicare Secondary Black Lung";
                    break;

                case "42":
                    str = "Medicare Secondary Veteran’s Administration";
                    break;

                case "43":
                    str = "Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)";
                    break;

                case "47":
                    str = "Medicare Secondary, Other Liability Insurance is Primary";
                    break;

                case "AP":
                    str = "Auto Insurance Policy";
                    break;

                case "C1":
                    str = "Commercial";
                    break;

                case "CO":
                    str = "Consolidated Omnibus Budget Reconciliation Act (COBRA)";
                    break;

                case "CP":
                    str = "Medicare Conditionally Primary";
                    break;

                case "D":
                    str = "Disability";
                    break;

                case "DB":
                    str = "Disability Benefits";
                    break;

                case "EP":
                    str = "Exclusive Provider Organization";
                    break;

                case "FF":
                    str = "Family or Friends";
                    break;

                case "GP":
                    str = "Group Policy";
                    break;

                case "HM":
                    str = "Health Maintenance Organization (HMO)";
                    break;

                case "HN":
                    str = "Health Maintenance Organization (HMO) - Medicare Risk";
                    break;

                case "HS":
                    str = "Special Low Income Medicare Beneficiary";
                    break;

                case "IN":
                    str = "Indemnity";
                    break;

                case "IP":
                    str = "Individual Policy";
                    break;

                case "LC":
                    str = "Long Term Care";
                    break;

                case "LD":
                    str = "Long Term Policy";
                    break;

                case "LI":
                    str = "Life Insurance";
                    break;

                case "LT":
                    str = "Litigation";
                    break;

                case "MA":
                    str = "Medicare Part A";
                    break;

                case "MB":
                    str = "Medicare Part B";
                    break;

                case "MC":
                    str = "Medicaid";
                    break;

                case "MH":
                    str = "Medigap Part A";
                    break;

                case "MI":
                    str = "Medigap Part B";
                    break;

                case "MP":
                    str = "Medicare Primary";
                    break;

                case "OT":
                    str = "Other";
                    break;

                case "PE":
                    str = "Property Insurance - Personal";
                    break;

                case "PL":
                    str = "Personal";
                    break;

                case "PP":
                    str = "Personal Payment (Cash - No Insurance)";
                    break;

                case "PR":
                    str = "Preferred Provider Organization (PPO)";
                    break;

                case "PS":
                    str = "Point of Service (POS)";
                    break;

                case "QM":
                    str = "Qualified Medicare Beneficiary";
                    break;

                case "RP":
                    str = "Property Insurance - Real";
                    break;

                case "SP":
                    str = "Supplemental Policy";
                    break;

                case "TF":
                    str = "Tax Equity Fiscal Responsibility Act (TEFRA)";
                    break;

                case "WC":
                    str = "Workers Compensation";
                    break;

                case "WU":
                    str = "Wrap Up Policy";
                    break;

                default:
                    str = "### Unknown EB04 ###";
                    break;
            }
            this.rowBenefits.EB04 = str;
            return str;
        }

        private string EB06(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "6":
                    str = "Hour";
                    break;

                case "7":
                    str = "Day";
                    break;

                case "21":
                    str = "Years";
                    break;

                case "22":
                    str = "Service Year";
                    break;

                case "23":
                    str = "Calendar Year";
                    break;

                case "24":
                    str = "Year to Date";
                    break;

                case "25":
                    str = "Contract";
                    break;

                case "26":
                    str = "Episode";
                    break;

                case "27":
                    str = "Visit";
                    break;

                case "28":
                    str = "Outlier";
                    break;

                case "29":
                    str = "Remaining";
                    break;

                case "30":
                    str = "Exceeded";
                    break;

                case "31":
                    str = "Not Exceeded";
                    break;

                case "32":
                    str = "Lifetime";
                    break;

                case "33":
                    str = "Lifetime Remaining";
                    break;

                case "34":
                    str = "Month";
                    break;

                case "35":
                    str = "Week";
                    break;

                case "36":
                    str = "Admisson";
                    break;

                case "13":
                    str = "24 Hours";
                    break;

                default:
                    str = "### Unknown EB06 ###";
                    break;
            }
            this.rowBenefits.EB06 = str;
            return str;
        }

        private string EB09(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "99":
                    str = "Quantity Used";
                    break;

                case "CA":
                    str = "Covered - Actual";
                    break;

                case "CE":
                    str = "Covered - Estimated";
                    break;

                case "DB":
                    str = "Deductible Blood Units";
                    break;

                case "DY":
                    str = "Days";
                    break;

                case "HS":
                    str = "Hours";
                    break;

                case "LA":
                    str = "Life-time Reserve - Actual";
                    break;

                case "LE":
                    str = "Life-time Reserve - Estimated";
                    break;

                case "MN":
                    str = "Month";
                    break;

                case "P6":
                    str = "Number of Services or Procedures";
                    break;

                case "QA":
                    str = "Quantity Approved";
                    break;

                case "S7":
                    str = "Age, High Value";
                    break;

                case "S8":
                    str = "Age, Low Value";
                    break;

                case "VS":
                    str = "Visits";
                    break;

                case "YY":
                    str = "Years";
                    break;

                default:
                    str = "### Unknown EB09 ###";
                    break;
            }
            this.rowBenefits.EB09 = str;
            return str;
        }

        private string EB11(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "N"))
                {
                    if (str3 == "U")
                    {
                        str = "It is unknown whether the plan provisions require an authorization or certification";
                        goto Label_0055;
                    }
                    if (str3 == "Y")
                    {
                        str = "Authorization or certification is required per plan provisions";
                        goto Label_0055;
                    }
                }
                else
                {
                    str = "Authorization or certification is not required per plan provisions";
                    goto Label_0055;
                }
            }
            str = "### Unknown EB11 ###";
        Label_0055:
            this.rowBenefits.EB11 = str;
            return str;
        }

        private string EB12(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "N"))
                {
                    if (str3 == "U")
                    {
                        str = "Unknown";
                        goto Label_006A;
                    }
                    if (str3 == "Y")
                    {
                        str = "Yes";
                        goto Label_006A;
                    }
                    if (str3 == "W")
                    {
                        str = "Not Applicable";
                        goto Label_006A;
                    }
                }
                else
                {
                    str = "No";
                    goto Label_006A;
                }
            }
            str = "### Unknown EB12 ###";
        Label_006A:
            this.rowBenefits.EB12 = str;
            return str;
        }

        private string EB13_1(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "AD":
                    str = "American Dental Association Codes";
                    break;

                case "CJ":
                    str = "Current Procedural Terminology (CPT) Codes";
                    break;

                case "HC":
                    str = "Health Care Financing Administration Common Procedural Coding System (HCPCS) Codes";
                    break;

                case "ID":
                    str = "International Classification of Diseases Clinical Modification (ICD-9-CM) - Procedure";
                    break;

                case "ND":
                    str = "National Drug Code (NDC)";
                    break;

                case "ZZ":
                    str = "Mutually Defined";
                    break;

                case "IV":
                    str = "Home Infusion EDI Coalition (HIEC) Product/Service Code";
                    break;

                case "N4":
                    str = "National Drug Code in 5-4-2 Format";
                    break;

                default:
                    str = "### Unknown EB13 - 1 ###";
                    break;
            }
            this.rowBenefits.EB13_1 = str;
            return str;
        }

        public string GetHumanReadable271(string EDI271, ref DS271 ds, string ServiceTypeCode)
        {
            string str;
            try
            {
                MDVUtility.SetDelimiters(EDI271);
                EDI271 = EDI271.Replace("\r\n", "");
                if ((EDI271.Trim().Length > 0) && EDICommon.IsEDI(EDI271))
                {
                    this.ds271Parser = ds;
                    this.ServiceTypeCode = ServiceTypeCode;
                    this.rowHeader = this.ds271Parser.EDI271Header.NewEDI271HeaderRow();
                    this.rowHeader.IsEligible = "false";
                    this.ds271Parser.EDI271Header.Rows.Add(this.rowHeader);
                    this.FinalStr271 = new StringBuilder();
                    this.Parse271(EDI271);
                    
                    CheckEligibilityStatus(ServiceTypeCode);
                    return this.FinalStr271.ToString();
                }
                str = "Not an EDI : " + EDI271;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private void CheckEligibilityStatus(string ServiceTypeCode)
        {

            //Get service type codes
            string[] source = FirstEB03_Value.Split(new char[] { this.D_S_S_ELMT });
            
            if (this.rowBenefits != null)
            {

                if (FirstAAA01_value == "N")
                {
                    this.rowHeader.IsEligible = "Waiting";
                }
                else if (FirstAAA01_value == "Y")
                {
                    if (string.IsNullOrEmpty(FirstAAA03_value))
                    {
                        if ( FirstEB01_Value == "1" || FirstEB01_Value == "2" || FirstEB01_Value == "3" || FirstEB01_Value == "4" || FirstEB01_Value == "5")
                        {
                            this.rowHeader.IsEligible = "Active";
                        }
                        else if (FirstEB01_Value == "6" || FirstEB01_Value == "7" || FirstEB01_Value == "8")
                        {
                            this.rowHeader.IsEligible = "InActive";
                        }
                    }
                    else
                    {
                        this.rowHeader.IsEligible = "Waiting";
                    }
                    
                }
                else
                {
                    if ( FirstEB01_Value == "1" || FirstEB01_Value == "2" || FirstEB01_Value == "3" || FirstEB01_Value == "4" || FirstEB01_Value == "5")
                    {
                        this.rowHeader.IsEligible = "Active";
                    }
                    else if (FirstEB01_Value == "6" || FirstEB01_Value == "7" || FirstEB01_Value == "8")
                    {
                        this.rowHeader.IsEligible = "InActive";
                    }
                    else
                    {
                        this.rowHeader.IsEligible = "Waiting";
                    }
                }

            }
        }

        private DS271.EDI271BenefitsDetailRow GetNew271BenefitDetailRow()
        {
            DS271.EDI271BenefitsDetailRow row;
            try
            {
                DS271.EDI271BenefitsDetailRow row2 = this.ds271Parser.EDI271BenefitsDetail.NewEDI271BenefitsDetailRow();
                row2.EDI271BenefitId = this.BenefitsId;
                this.ds271Parser.EDI271BenefitsDetail.Rows.Add(row2);
                row = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row;
        }

        private DS271.EDI271BenefitsRow GetNew271BenefitsRow()
        {
            DS271.EDI271BenefitsRow row;
            try
            {
                DS271.EDI271BenefitsRow row2 = this.ds271Parser.EDI271Benefits.NewEDI271BenefitsRow();
                row2.EDI271NameId = this.NameId;
                this.ds271Parser.EDI271Benefits.Rows.Add(row2);
                this.BenefitsId = Convert.ToInt64(row2.EDI271BenefitId);
                row = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row;
        }

        private DS271.EDI271NamesRow GetNew271NameRow()
        {
            DS271.EDI271NamesRow row;
            try
            {
                DS271.EDI271NamesRow row2 = this.ds271Parser.EDI271Names.NewEDI271NamesRow();
                this.ds271Parser.EDI271Names.Rows.Add(row2);
                this.NameId = Convert.ToInt64(row2.EDI271NameId);
                row = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row;
        }

        private DS271.EDI271NamesRow GetNew271TempNameRow()
        {
            DS271.EDI271NamesRow row;
            try
            {
                DS271.EDI271NamesRow row2 = this.ds271tempParser.EDI271Names.NewEDI271NamesRow();
                this.ds271tempParser.EDI271Names.Rows.Add(row2);
                row = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row;
        }

        private string HL(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "HL")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 3 })
                {
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append(strArray[num2]);
                                builder.Append("  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                                continue;
                            }
                        case 3:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Hierarchical Level: ");
                                builder.Append(this.HL03(strArray[num2]));
                                continue;
                            }
                    }
                    builder.Append(Environment.NewLine);
                    builder.Append("## this HL segment not in use ##");
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

        private string HL03(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "20"))
                {
                    if (str3 == "21")
                    {
                        str = "Information Receiver";
                        goto Label_006A;
                    }
                    if (str3 == "22")
                    {
                        str = "Subscriber";
                        goto Label_006A;
                    }
                    if (str3 == "23")
                    {
                        str = "Dependent";
                        goto Label_006A;
                    }
                }
                else
                {
                    str = "Information Source";
                    goto Label_006A;
                }
            }
            str = "### Unknown HL03 ###";
        Label_006A: ;
            string[] source = new string[] { "20", "21", "22", "23" };
            if (source.Contains<string>(Code))
            {
                this.rowName = this.GetNew271NameRow();
            }
            return str;
        }

        private string HSD(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "HSD")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append(this.HSD01(strArray[num2]));
                                    builder.Append(": ");
                                    continue;
                                }
                            case 2:
                                {
                                    builder.Append(strArray[num2]);
                                    this.rowBenefitsDetail.HSD02 = strArray[num2];
                                    continue;
                                }
                            case 3:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Unit or Basis for Measurement: ");
                                    builder.Append(this.HSD03(strArray[num2]));
                                    continue;
                                }
                            case 4:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Sample Selection Modulus: ");
                                    builder.Append(strArray[num2]);
                                    this.rowBenefitsDetail.HSD03 = strArray[num2];
                                    continue;
                                }
                            case 5:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append(this.HSD05(strArray[num2]));
                                    builder.Append(": ");
                                    continue;
                                }
                            case 6:
                                {
                                    builder.Append(strArray[num2]);
                                    this.rowBenefitsDetail.HSD06 = strArray[num2];
                                    continue;
                                }
                            case 7:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Delivery Frequency: ");
                                    builder.Append(this.HSD07(strArray[num2]));
                                    continue;
                                }
                            case 8:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Delivery Pattern Time: ");
                                    builder.Append(this.HSD08(strArray[num2]));
                                    continue;
                                }
                        }
                        builder.Append(Environment.NewLine);
                        builder.Append("## this HSD segment not in use ##");
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

        private string HSD01(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "DY"))
                {
                    if (str3 == "FL")
                    {
                        str = "Units";
                        goto Label_007F;
                    }
                    if (str3 == "HS")
                    {
                        str = "Hours";
                        goto Label_007F;
                    }
                    if (str3 == "MN")
                    {
                        str = "Month";
                        goto Label_007F;
                    }
                    if (str3 == "VS")
                    {
                        str = "Visits";
                        goto Label_007F;
                    }
                }
                else
                {
                    str = "Days";
                    goto Label_007F;
                }
            }
            str = "### Unknown HSD01 ###";
        Label_007F:
            this.rowBenefitsDetail.HSD01 = str;
            return str;
        }

        private string HSD03(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "DA"))
                {
                    if (str3 == "MO")
                    {
                        str = "Month";
                        goto Label_007F;
                    }
                    if (str3 == "VS")
                    {
                        str = "Visits";
                        goto Label_007F;
                    }
                    if (str3 == "WK")
                    {
                        str = "Week";
                        goto Label_007F;
                    }
                    if (str3 == "YR")
                    {
                        str = "Years";
                        goto Label_007F;
                    }
                }
                else
                {
                    str = "Days";
                    goto Label_007F;
                }
            }
            str = "### Unknown HSD03 ###";
        Label_007F:
            this.rowBenefitsDetail.HSD03 = str;
            return str;
        }

        private string HSD05(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "6":
                    str = "Hour";
                    break;

                case "7":
                    str = "Day";
                    break;

                case "21":
                    str = "Years";
                    break;

                case "22":
                    str = "Service Year";
                    break;

                case "23":
                    str = "Calendar Year";
                    break;

                case "24":
                    str = "Year to Date";
                    break;

                case "25":
                    str = "Contract";
                    break;

                case "26":
                    str = "Episode";
                    break;

                case "27":
                    str = "Visit";
                    break;

                case "28":
                    str = "Outlier";
                    break;

                case "29":
                    str = "Remaining";
                    break;

                case "30":
                    str = "Exceeded";
                    break;

                case "31":
                    str = "Not Exceeded";
                    break;

                case "32":
                    str = "Lifetime";
                    break;

                case "33":
                    str = "Lifetime Remaining";
                    break;

                case "34":
                    str = "Month";
                    break;

                case "35":
                    str = "Week";
                    break;

                default:
                    str = "### Unknown HSD05 ###";
                    break;
            }
            this.rowBenefitsDetail.HSD05 = str;
            return str;
        }

        private string HSD07(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "1":
                    str = "1st Week of the Month";
                    break;

                case "2":
                    str = "2nd Week of the Month";
                    break;

                case "3":
                    str = "3rd Week of the Month";
                    break;

                case "4":
                    str = "4th Week of the Month";
                    break;

                case "5":
                    str = "5th Week of the Month";
                    break;

                case "6":
                    str = "1st & 3rd Weeks of the Month";
                    break;

                case "7":
                    str = "2nd & 4th Weeks of the Month";
                    break;

                case "8":
                    str = "1st Working Day of Period";
                    break;

                case "9":
                    str = "Last Working Day of Period";
                    break;

                case "A":
                    str = "Monday through Friday";
                    break;

                case "B":
                    str = "Monday through Saturday";
                    break;

                case "C":
                    str = "Monday through Sunday";
                    break;

                case "D":
                    str = "Monday";
                    break;

                case "E":
                    str = "Tuesday";
                    break;

                case "F":
                    str = "Wednesday";
                    break;

                case "G":
                    str = "Thursday";
                    break;

                case "H":
                    str = "Friday";
                    break;

                case "J":
                    str = "Saturday";
                    break;

                case "K":
                    str = "Sunday";
                    break;

                case "L":
                    str = "Monday through Thursday";
                    break;

                case "M":
                    str = "Immediately";
                    break;

                case "N":
                    str = "As Directed";
                    break;

                case "O":
                    str = "Daily Mon. through Fri";
                    break;

                case "P":
                    str = "1/2 Mon. & 1/2 Thurs.";
                    break;

                case "Q":
                    str = "1/2 Tues. & 1/2 Thurs.";
                    break;

                case "R":
                    str = "1/2 Wed. & 1/2 Fri.";
                    break;

                case "S":
                    str = "Once Anytime Mon. through Fri.";
                    break;

                case "SG":
                    str = "Tuesday through Friday";
                    break;

                case "SL":
                    str = "Monday, Tuesday and Thursday";
                    break;

                case "SP":
                    str = "Monday, Tuesday and Friday";
                    break;

                case "SX":
                    str = "Wednesday and Thursday";
                    break;

                case "SY":
                    str = "Monday, Wednesday and Thursday";
                    break;

                case "SZ":
                    str = "Tuesday, Thursday and Friday";
                    break;

                case "T":
                    str = "1/2 Tue. & 1/2 Fri.";
                    break;

                case "U":
                    str = "1/2 Mon. & 1/2 Wed.";
                    break;

                case "V":
                    str = "1/3 Mon., 1/3 Wed., 1/3 Fri.";
                    break;

                case "W":
                    str = "Whenever Necessary";
                    break;

                case "X":
                    str = "1/2 By Wed., Bal. By Fri.";
                    break;

                case "Y":
                    str = "None (Also Used to Cancel or Override a Previous Pattern";
                    break;

                default:
                    str = "### Unknown HSD07 ###";
                    break;
            }
            this.rowBenefitsDetail.HSD07 = str;
            return str;
        }

        private string HSD08(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "A":
                    str = "1st Shift (Normal Working Hours)";
                    break;

                case "B":
                    str = "2nd Shift";
                    break;

                case "C":
                    str = "3rd Shift";
                    break;

                case "D":
                    str = "A.M.";
                    break;

                case "E":
                    str = "P.M.";
                    break;

                case "F":
                    str = "As Directed";
                    break;

                case "G":
                    str = "Any Shift";
                    break;

                case "Y":
                    str = "None (Also Used to Cancel or Override a Previous Pattern)";
                    break;

                default:
                    str = "### Unknown HSD08 ###";
                    break;
            }
            this.rowBenefitsDetail.HSD08 = str;
            return str;
        }

        private string III(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "III")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2 })
                {
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append(this.III01(strArray[num2]));
                                this.III01_Value = strArray[num2];
                                continue;
                            }
                        case 2:
                            {
                                if (!(this.III01_Value == "ZZ"))
                                {
                                    break;
                                }
                                continue;
                            }
                        default:
                            goto Label_00FF;
                    }
                    builder.Append(strArray[num2]);
                    continue;
                Label_00FF:
                    builder.Append(Environment.NewLine);
                    builder.Append("## this III segment not in use ##");
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

        private string III01(string Code)
        {
            switch (Code)
            {
                case "BF":
                    return "Diagnosis";

                case "BK":
                    return "Principal Diagnosis";

                case "ZZ":
                    return "Place of Service";
            }
            return "### Unknown III01 ###";
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private string INS(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "INS")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4, 9, 10, 0x11 })
                {
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Insured Indicator: ");
                                builder.Append(this.INS01(strArray[num2]));
                                this.rowName.INS01 = strArray[num2];
                                continue;
                            }
                        case 2:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Individual Relationship: ");
                                builder.Append(this.INS02(strArray[num2]));
                                continue;
                            }
                        case 3:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Maintenance Type: ");
                                builder.Append(this.INS03(strArray[num2]));
                                continue;
                            }
                        case 4:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Maintenance Reason: ");
                                builder.Append(this.INS04(strArray[num2]));
                                continue;
                            }
                        case 9:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Student Status: ");
                                builder.Append(this.INS09(strArray[num2]));
                                continue;
                            }
                        case 10:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Handicap Indicator: ");
                                builder.Append(this.INS10(strArray[num2]));
                                this.rowName.INS10 = strArray[num2];
                                continue;
                            }
                        case 0x11:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append("Birth Sequence Number: ");
                                builder.Append(strArray[num2]);
                                this.rowName.INS17 = strArray[num2];
                                continue;
                            }
                    }
                    builder.Append(Environment.NewLine);
                    builder.Append("## this INS segment not in use ##");
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

        private string INS01(string Code)
        {
            switch (Code)
            {
                case "Y":
                    return "Insured is a subscriber";

                case "N":
                    return "Insured is a dependent";
            }
            return "### Unknown INS01 ###";
        }

        private string INS02(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "18"))
                {
                    if (str3 == "01")
                    {
                        str = "Spouse";
                        goto Label_007F;
                    }
                    if (str3 == "19")
                    {
                        str = "Child";
                        goto Label_007F;
                    }
                    if (str3 == "21")
                    {
                        str = "Unknown";
                        goto Label_007F;
                    }
                    if (str3 == "34")
                    {
                        str = "Other Adult";
                        goto Label_007F;
                    }
                }
                else
                {
                    str = "Self";
                    goto Label_007F;
                }
            }
            str = "### Unknown INS02 ###";
        Label_007F:
            this.rowName.INS02 = str;
            return str;
        }

        private string INS03(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if ((str3 != null) && (str3 == "001"))
            {
                str = "Change";
            }
            else
            {
                str = "### Unknown INS03 ###";
            }
            this.rowName.INS03 = str;
            return str;
        }

        private string INS04(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if ((str3 != null) && (str3 == "25"))
            {
                str = "Change in Indentifying Data Elements";
            }
            else
            {
                str = "### Unknown INS04 ###";
            }
            this.rowName.INS04 = str;
            return str;
        }

        private string INS09(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if (str3 != null)
            {
                if (!(str3 == "F"))
                {
                    if (str3 == "N")
                    {
                        str = "Not a Student";
                        goto Label_0055;
                    }
                    if (str3 == "P")
                    {
                        str = "Part-time";
                        goto Label_0055;
                    }
                }
                else
                {
                    str = "Full-time";
                    goto Label_0055;
                }
            }
            str = "### Unknown INS09 ###";
        Label_0055:
            this.rowName.INS09 = str;
            return str;
        }

        private string INS10(string Code)
        {
            switch (Code)
            {
                case "N":
                    return "Individual is not handicapped";

                case "Y":
                    return "Individual is handicapped";
            }
            return "### Unknown INS10 ###";
        }

        private bool IsBenefitsDTP()
        {
            return ((this.rowBenefits != null) && (this.NameId == this.rowBenefits.EDI271NameId));
        }

        private string LS(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "LS")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        if (num2 == 1)
                        {
                            if ((this.rowBenefits != null) && (this.EB01_Value == "L"))
                            {
                                this.rowName = this.GetNew271NameRow();
                            }
                            else
                            {
                                this.rowName = this.GetNew271TempNameRow();
                            }
                            builder.Append(Environment.NewLine);
                            builder.Append(strArray[num2]);
                            builder.Append("  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                        }
                        else
                        {
                            builder.Append(Environment.NewLine);
                            builder.Append("## this HL segment not in use ##");
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

        private string MPI(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "MPI")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4, 5, 6, 7 })
                {
                    DateTime time;
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.MPI01(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 2:
                                builder.Append(this.MPI02(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 3:
                                builder.Append(this.MPI03(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 4:
                                builder.Append(strArray[num2]);
                                this.rowName.MPI04 = strArray[num2];
                                builder.Append(": ");
                                break;

                            case 5:
                                builder.Append(this.MPI05(strArray[num2]));
                                builder.Append(": ");
                                break;

                            case 6:
                                this.MPI06_Value = strArray[num2];
                                builder.Append(this.MPI06_Value);
                                this.rowName.MPI06 = strArray[num2];
                                builder.Append(": ");
                                break;

                            case 7:
                                if (!(this.MPI06_Value == "D8"))
                                {
                                    goto Label_0203;
                                }
                                time = MDVUtility.StringToDate(strArray[num2]);
                                builder.Append(time.ToShortDateString());
                                this.rowName.MPI07 = time.ToShortDateString();
                                break;
                        }
                    }
                    continue;
                Label_0203:
                    if (this.MPI06_Value == "RD8")
                    {
                        string[] strArray2 = strArray[num2].Split(new char[] { '-' });
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    time = MDVUtility.StringToDate(strArray[0]);
                                    builder.Append(time.ToShortDateString());
                                    this.rowName.MPI07 = time.ToShortDateString();
                                    break;

                                case 1:
                                    builder.Append(" - ");
                                    time = MDVUtility.StringToDate(strArray[1]);
                                    builder.Append(time.ToShortDateString());
                                    this.rowName.MPI07 = this.rowName.MPI07 + " - " + time.ToShortDateString();
                                    break;
                            }
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

        private string MPI01(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "A":
                    str = "Partial";
                    break;

                case "C":
                    str = "Current";
                    break;

                case "L":
                    str = "Latest";
                    break;

                case "O":
                    str = "Oldest";
                    break;

                case "P":
                    str = "Prior";
                    break;

                case "S":
                    str = "Second Most Current";
                    break;

                case "T":
                    str = "Third Most Current";
                    break;

                default:
                    str = "### Unknown MPI01 ###";
                    break;
            }
            this.rowName.MPI01 = str;
            return str;
        }

        private string MPI02(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "AE":
                    str = "Active Reserve";
                    break;

                case "AO":
                    str = "Active Military - Overseas";
                    break;

                case "AS":
                    str = "Academy Student";
                    break;

                case "AT":
                    str = "Presidential Appointee";
                    break;

                case "AU":
                    str = "Active Military - USA";
                    break;

                case "CC":
                    str = "Contractor";
                    break;

                case "DD":
                    str = "Dishonorably Discharged";
                    break;

                case "HD":
                    str = "Honorably Discharged";
                    break;

                case "IR":
                    str = "Inactive Reserves";
                    break;

                case "LX":
                    str = "Leave of Absence: Military";
                    break;

                case "PE":
                    str = "Plan to Enlist";
                    break;

                case "RE":
                    str = "Recommissioned";
                    break;

                case "RM":
                    str = "Retired Military - Overseas";
                    break;

                case "RR":
                    str = "Retired Without Recall";
                    break;

                case "RU":
                    str = "Retired Military - USA";
                    break;

                default:
                    str = "### Unknown MPI02 ###";
                    break;
            }
            this.rowName.MPI02 = str;
            return str;
        }

        private string MPI03(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "A":
                    str = "Air Force";
                    break;

                case "B":
                    str = "Air Force Reserves";
                    break;

                case "C":
                    str = "Army";
                    break;

                case "D":
                    str = "Army Reserves";
                    break;

                case "E":
                    str = "Coast Guard";
                    break;

                case "F":
                    str = "Marine Corps";
                    break;

                case "G":
                    str = "Marine Corps Reserves";
                    break;

                case "H":
                    str = "National Guard";
                    break;

                case "I":
                    str = "Navy";
                    break;

                case "J":
                    str = "Navy Reserves";
                    break;

                case "K":
                    str = "Other";
                    break;

                case "L":
                    str = "Peace Corp";
                    break;

                case "M":
                    str = "Regular Armed Forces";
                    break;

                case "N":
                    str = "Reserves";
                    break;

                case "O":
                    str = "U.S. Public Health Service";
                    break;

                case "Q":
                    str = "Foreign Military";
                    break;

                case "R":
                    str = "American Red Cross";
                    break;

                case "S":
                    str = "Department of Defense";
                    break;

                case "U":
                    str = "United Services Organization";
                    break;

                case "W":
                    str = "Military Sealift Command";
                    break;

                default:
                    str = "### Unknown MPI03 ###";
                    break;
            }
            this.rowName.MPI03 = str;
            return str;
        }

        private string MPI04(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
            if ((str3 != null) && (str3 == "096"))
            {
                str = "Discharge";
            }
            else
            {
                str = "### Unknown MPI04 ###";
            }
            this.rowName.MPI04 = str;
            return str;
        }

        private string MPI05(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "A1":
                    str = "Admiral";
                    break;

                case "A2":
                    str = "Airman";
                    break;

                case "A3":
                    str = "Airman First Class";
                    break;

                case "B1":
                    str = "Basic Airman";
                    break;

                case "B2":
                    str = "Brigadier General";
                    break;

                case "C1":
                    str = "Captain";
                    break;

                case "C2":
                    str = "Chief Master Sergeant";
                    break;

                case "C3":
                    str = "Chief Petty Officer";
                    break;

                case "C4":
                    str = "Chief Warrant";
                    break;

                case "C5":
                    str = "Colonel";
                    break;

                case "C6":
                    str = "Commander";
                    break;

                case "C7":
                    str = "Commodore";
                    break;

                case "C8":
                    str = "Corporal";
                    break;

                case "C9":
                    str = "Corporal Specialist 4";
                    break;

                case "E1":
                    str = "Ensign";
                    break;

                case "F1":
                    str = "First Lieutenant";
                    break;

                case "F2":
                    str = "First Sergeant";
                    break;

                case "F3":
                    str = "First Sergeant-Master Sergeant";
                    break;

                case "F4":
                    str = "Fleet Admiral";
                    break;

                case "G1":
                    str = "General";
                    break;

                case "G4":
                    str = "General Sergeant";
                    break;

                case "L1":
                    str = "Lance Corporal";
                    break;

                case "L2":
                    str = "Lieutenant";
                    break;

                case "L3":
                    str = "Lieutenant Colonel";
                    break;

                case "L4":
                    str = "Lieutenant Commodore";
                    break;

                case "L5":
                    str = "Lieutenant General";
                    break;

                case "L6":
                    str = "Lieutenant Junior Grade";
                    break;

                case "M1":
                    str = "Major";
                    break;

                case "M2":
                    str = "Major General";
                    break;

                case "M3":
                    str = "Master Chief Petty Officer";
                    break;

                case "M4":
                    str = "Master Gunnery Sergeant Major";
                    break;

                case "M5":
                    str = "Master Sergeant";
                    break;

                case "M6":
                    str = "Master Sergeant Specialist 8";
                    break;

                case "P1":
                    str = "Petty Officer First Class";
                    break;

                case "P2":
                    str = "Petty Officer Second Class";
                    break;

                case "P3":
                    str = "Petty Officer Third Class";
                    break;

                case "P4":
                    str = "Private";
                    break;

                case "P5":
                    str = "Private First Class";
                    break;

                case "R1":
                    str = "Rear Admiral";
                    break;

                case "R2":
                    str = "Recruit";
                    break;

                case "S1":
                    str = "Seaman";
                    break;

                case "S2":
                    str = "Seaman Apprentice";
                    break;

                case "S3":
                    str = "Seaman Recruit";
                    break;

                case "S4":
                    str = "Second Lieutenant";
                    break;

                case "S5":
                    str = "Senior Chief Petty Officer";
                    break;

                case "S6":
                    str = "Senior Master Sergeant";
                    break;

                case "S7":
                    str = "Sergeant";
                    break;

                case "S8":
                    str = "Sergeant First Class Specialist 7";
                    break;

                case "S9":
                    str = "Sergeant Major Specialist 9";
                    break;

                case "SA":
                    str = "Sergeant Specialist 5";
                    break;

                case "SB":
                    str = "Staff Sergeant";
                    break;

                case "SC":
                    str = "Staff Sergeant Specialist 6";
                    break;

                case "T1":
                    str = "Technical Sergeant";
                    break;

                case "V1":
                    str = "Vice Admiral";
                    break;

                case "W1":
                    str = "Warrant Officer";
                    break;

                default:
                    str = "### Unknown MPI05 ###";
                    break;
            }
            this.rowName.MPI05 = str;
            return str;
        }

        private string MSG(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "MSG")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        if (num2 == 1)
                        {
                            builder.Append(Environment.NewLine);
                            builder.Append("Free Form Message Text: ");
                            builder.Append(strArray[num2]);
                            this.rowBenefits.MSG01 = strArray[num2];
                        }
                        else
                        {
                            builder.Append(Environment.NewLine);
                            builder.Append("## this MSG segment not in use ##");
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
                foreach (int num2 in new int[] { 1, 2 })
                {
                    if ((num2 >= strArray.Length) || (strArray[num2].Length <= 0))
                    {
                        continue;
                    }
                    switch (num2)
                    {
                        case 1:
                            this.rowName.N301 = strArray[num2];
                            break;

                        case 2:
                            this.rowName.N302 = strArray[num2];
                            break;
                    }
                    builder.Append(strArray[num2]);
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
                foreach (int num2 in new int[] { 1, 2, 3 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                this.rowName.N401 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(" ");
                                break;

                            case 2:
                                this.rowName.N402 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(" ");
                                break;

                            case 3:
                                this.rowName.N403 = strArray[num2];
                                builder.Append(strArray[num2]);
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

        private string NM1(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "NM1")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 6, 3, 4, 5, 7, 8, 9, 10, 11, 12 })
                {
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
                                this.rowName.NM103 = strArray[num2];
                                continue;
                            }
                        case 4:
                            {
                                this.rowName.NM104 = strArray[num2];
                                continue;
                            }
                        case 5:
                            {
                                this.rowName.NM105 = strArray[num2];
                                continue;
                            }
                        case 6:
                            {
                                this.rowName.NM106 = strArray[num2];
                                continue;
                            }
                        case 8:
                            {
                                builder.Append(Environment.NewLine);
                                builder.Append(this.NM108(strArray[num2]));
                                builder.Append(": ");
                                continue;
                            }
                        case 9:
                            {
                                this.rowName.NM109 = strArray[num2];
                                continue;
                            }
                    }
                    builder.Append(strArray[num2]);
                    builder.Append(" ");
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

        private string NM101(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "2B":
                    str = "Third-Party Administrator";
                    break;

                case "36":
                    str = "Employer";
                    break;

                case "GP":
                    str = "Gateway Provider";
                    break;

                case "P5":
                    str = "Plan Sponsor";
                    break;

                case "PR":
                    str = "Payer";
                    break;

                case "1P":
                    str = "Provider";
                    break;

                case "80":
                    str = "Hospital";
                    break;

                case "FA":
                    str = "Facility";
                    break;

                case "IL":
                    str = "Insured or Subscriber";
                    break;

                case "13":
                    str = "Contracted Service Provider";
                    break;

                case "73":
                    str = "Other Physician";
                    break;

                case "LR":
                    str = "Legal Representative";
                    break;

                case "P3":
                    str = "Primary Care Provider";
                    break;

                case "P4":
                    str = "Prior Insurance Carrier";
                    break;

                case "PRP":
                    str = "Primary Payer";
                    break;

                case "SEP":
                    str = "Secondary Payer";
                    break;

                case "TTP":
                    str = "Tertiary Payer";
                    break;

                case "VN":
                    str = "Vendor";
                    break;

                case "X3":
                    str = "Utilization Management Organization";
                    break;

                case "03":
                    str = "Dependent";
                    break;

                default:
                    str = "### Unknown NM101 ###";
                    break;
            }
            this.rowName.NM101 = str;
            this.rowName.NM101Code = Code;
            return str;
        }

        private string NM108(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "24":
                    str = "Employer’s Identification Number";
                    break;

                case "46":
                    str = "Electronic Transmitter Identification Number (ETIN)";
                    break;

                case "FI":
                    str = "Federal Taxpayer’s Identification Number";
                    break;

                case "NI":
                    str = "National Association of Insurance Commissioners (NAIC) Identification";
                    break;

                case "PI":
                    str = "Payor Identification";
                    break;

                case "XV":
                    str = "Health Care Financing Administration National PlanID";
                    break;

                case "XX":
                    str = "Health Care Financing Administration National Provider Identifier";
                    break;

                case "34":
                    str = "Social Security Number";
                    break;

                case "PP":
                    str = "Pharmacy Processor Number";
                    break;

                case "SV":
                    str = "Service Provider Number";
                    break;

                case "MI":
                    str = "Member Identification Number";
                    break;

                case "ZZ":
                    str = "Mutually Defined";
                    break;

                case "FA":
                    str = "Facility Identification";
                    break;

                case "N4":
                    str = "National Drug Code in 5-4-2 Format";
                    break;

                default:
                    str = "### Unknown NM108 ###";
                    break;
            }
            this.rowName.NM108 = str;
            return str;
        }

        private void Parse271(string StrIn)
        {
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_SGMT });
                for (int i = 0; i < (strArray.Length - 2); i++)
                {
                   

                        switch (strArray[i].Substring(0, strArray[i].IndexOf(MDVUtility.D_ELMT)))
                        {
                            case "ISA":
                                this.FinalStr271.Append("ISA ##########################################################################");
                                this.FinalStr271.Append(Environment.NewLine);
                                break;

                            case "IEA":
                                this.FinalStr271.Append("IEA ##########################################################################");
                                this.FinalStr271.Append(Environment.NewLine);
                                break;

                            case "GS":
                                this.FinalStr271.Append("GS  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                                this.FinalStr271.Append(Environment.NewLine);
                                break;

                            case "GE":
                                this.FinalStr271.Append("GE  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                                this.FinalStr271.Append(Environment.NewLine);
                                break;

                            

                            default:
                                this.FinalStr271.Append("----------------------------------------------------------------");
                                this.FinalStr271.Append(Environment.NewLine);
                                this.FinalStr271.Append(strArray[i]);
                                this.FinalStr271.Append("----------------------------------------------------------------");
                                this.FinalStr271.Append(Environment.NewLine);
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
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                builder.Append(this.PER01(strArray[num2]));
                                builder.Append(Environment.NewLine);
                                break;

                            case 2:
                                this.rowName.PER02 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                break;

                            case 3:
                                this.rowName.PER03 = strArray[num2];
                                builder.Append(this.PER03(strArray[num2]));
                                builder.Append(Environment.NewLine);
                                break;

                            case 4:
                                this.rowName.PER04 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                break;

                            case 5:
                                this.rowName.PER05 = strArray[num2];
                                builder.Append(this.PER05(strArray[num2]));
                                builder.Append(Environment.NewLine);
                                break;

                            case 6:
                                this.rowName.PER06 = strArray[num2];
                                builder.Append(strArray[num2]);
                                builder.Append(Environment.NewLine);
                                break;

                            case 7:
                                this.rowName.PER07 = strArray[num2];
                                builder.Append(this.PER07(strArray[num2]));
                                builder.Append(Environment.NewLine);
                                break;

                            case 8:
                                this.rowName.PER08 = strArray[num2];
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
            string str = "Unknown";
            string str3 = Code;
            if ((str3 != null) && (str3 == "IC"))
            {
                str = "Information Contact";
            }
            else
            {
                str = "### Unknown PER01 ###";
            }
            this.rowName.PER01 = str;
            return str;
        }

        private string PER03(string Code)
        {
            switch (Code)
            {
                case "ED":
                    return "Electronic Data Interchange Access Number";

                case "EM":
                    return "Electronic Mail";

                case "FX":
                    return "Facsimile";

                case "TE":
                    return "Telephone";

                case "UR":
                    return "Uniform Resource Locater (URL)";

                case "HP":
                    return "Home Phone Number";

                case "WP":
                    return "Work Phone Number";
            }
            return "### Unknown PER03 ###";
        }

        private string PER05(string Code)
        {
            switch (Code)
            {
                case "ED":
                    return "Electronic Data Interchange Access Number";

                case "EM":
                    return "Electronic Mail";

                case "EX":
                    return "Telephone Extension";

                case "FX":
                    return "Facsimile";

                case "HP":
                    return "Home Phone Number";

                case "TE":
                    return "Telephone";

                case "UR":
                    return "Uniform Resource Locater (URL)";

                case "WP":
                    return "Work Phone Number";
            }
            return "### Unknown PER05 ###";
        }

        private string PER07(string Code)
        {
            switch (Code)
            {
                case "ED":
                    return "Electronic Data Interchange Access Number";

                case "EM":
                    return "Electronic Mail";

                case "EX":
                    return "Telephone Extension";

                case "FX":
                    return "Facsimile";

                case "TE":
                    return "Telephone";

                case "HP":
                    return "Home Phone Number";

                case "UR":
                    return "Uniform Resource Locater (URL)";

                case "WP":
                    return "Work Phone Number";
            }
            return "### Unknown PER07 ###";
        }

        private string PRV(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "PRV")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3 })
                {
                    if ((num2 < strArray.Length) && (strArray[num2].Length > 0))
                    {
                        switch (num2)
                        {
                            case 1:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append("Provider: ");
                                    builder.Append(this.PRV01(strArray[num2]));
                                    continue;
                                }
                            case 2:
                                {
                                    builder.Append(Environment.NewLine);
                                    builder.Append(this.PRV02(strArray[num2]));
                                    builder.Append(": ");
                                    continue;
                                }
                            case 3:
                                {
                                    builder.Append(strArray[num2]);
                                    this.rowName.PRV03 = strArray[num2];
                                    continue;
                                }
                        }
                        builder.Append(Environment.NewLine);
                        builder.Append("## this PRV segment not in use ##");
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

        private string PRV01(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "AD":
                    str = "Admitting";
                    break;

                case "AT":
                    str = "Attending";
                    break;

                case "BI":
                    str = "Billing";
                    break;

                case "CO":
                    str = "Consulting";
                    break;

                case "CV":
                    str = "Covering";
                    break;

                case "H":
                    str = "Hospital";
                    break;

                case "HH":
                    str = "Home Health Care";
                    break;

                case "LA":
                    str = "Laboratory";
                    break;

                case "OT":
                    str = "Other Physician";
                    break;

                case "P1":
                    str = "Pharmacist";
                    break;

                case "P2":
                    str = "Pharmacy";
                    break;

                case "PE":
                    str = "Performing";
                    break;

                case "R":
                    str = "Rural Health Clinic";
                    break;

                case "RF":
                    str = "Referring";
                    break;

                case "SK":
                    str = "Skilled Nursing Facility";
                    break;

                case "PC":
                    str = "Primary Care Physician";
                    break;

                case "SB":
                    str = "Submitting";
                    break;

                case "SU":
                    str = "Supervising";
                    break;

                default:
                    str = "### Unknown PRV01 ###";
                    break;
            }
            this.rowName.PRV01 = str;
            return str;
        }

        private string PRV02(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "9K":
                    str = "Servicer";
                    break;

                case "D3":
                    str = "National Association of Boards of Pharmacy Number";
                    break;

                case "EI":
                    str = "Employer’s Identification Number";
                    break;

                case "HPI":
                    str = "Health Care Financing Administration National Provider Identifier";
                    break;

                case "SY":
                    str = "Social Security Number";
                    break;

                case "TJ":
                    str = "Federal Taxpayer’s Identification Number";
                    break;

                case "ZZ":
                    str = "Taxonomy Code";
                    break;

                case "PXC":
                    str = "Health Care Provider Taxonomy Code";
                    break;

                default:
                    str = "### Unknown PRV02 ###";
                    break;
            }
            this.rowName.PRV02 = str;
            return str;
        }

        private string REF(string StrIn)
        {
            string str;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "REF")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3 })
                {
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
                                this.rowName.REF02 += strArray[num2] + ":";
                                break;

                            case 3:
                                builder.Append(strArray[num2]);
                                this.rowName.REF03 += strArray[num2] + ":";
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

        private string REF01(string Code)
        {
            string str = "Unknown";
            switch (Code)
            {
                case "18":
                    str = "Plan Number";
                    break;

                case "55":
                    str = "Sequence Number";
                    break;

                case "0B":
                    str = "State License Number";
                    break;

                case "1C":
                    str = "Medicare Provider Number";
                    break;

                case "1D":
                    str = "Medicaid Provider Number";
                    break;

                case "1J":
                    str = "Facility ID Number";
                    break;

                case "4A":
                    str = "Personal Identification Number (PIN)";
                    break;

                case "CT":
                    str = "Contract Number";
                    break;

                case "EL":
                    str = "Electronic device pin number";
                    break;

                case "EO":
                    str = "Submitter Identification Number";
                    break;

                case "HPI":
                    str = "Health Care Financing Administration National Provider Identifier";
                    break;

                case "JD":
                    str = "User Identification";
                    break;

                case "N5":
                    str = "Provider Plan Network Identification Number";
                    break;

                case "N7":
                    str = "Facility Network Identification Number";
                    break;

                case "SY":
                    str = "Social Security Number";
                    break;

                case "TJ":
                    str = "Federal Taxpayer’s Identification Number";
                    break;

                case "1L":
                    str = "Group or Policy Number";
                    break;

                case "1W":
                    str = "Member Identification Number";
                    break;

                case "3H":
                    str = "Case Number";
                    break;

                case "49":
                    str = "Family Unit Number";
                    break;

                case "6P":
                    str = "Group Number";
                    break;

                case "A6":
                    str = "Employee Identification Number";
                    break;

                case "EA":
                    str = "Medical Record Identification Number";
                    break;

                case "EJ":
                    str = "Patient Account Number";
                    break;

                case "F6":
                    str = "Health Insurance Claim (HIC) Number";
                    break;

                case "FO":
                    str = "Drug Formulary Number";
                    break;

                case "GH":
                    str = "Identification Card Serial Number";
                    break;

                case "HJ":
                    str = "Identity Card Number";
                    break;

                case "IF":
                    str = "Issue Number";
                    break;

                case "IG":
                    str = "Insurance Policy Number";
                    break;

                case "ML":
                    str = "Military Rank/Civilian Pay Grade Number";
                    break;

                case "N6":
                    str = "Plan Network Identification Number";
                    break;

                case "NQ":
                    str = "Medicaid Recipient Identification Number";
                    break;

                case "Q4":
                    str = "Prior Identifier Number";
                    break;

                case "G1":
                    str = "Prior Authorization Number";
                    break;

                case "9F":
                    str = "Referral Number";
                    break;

                case "M7":
                    str = "Medical Assistance Category";
                    break;

                case "IV":
                    str = "Home Infusion EDI Coalition (HIEC) Product/Service Code";
                    break;

                case "N4":
                    str = "National Drug Code in 5-4-2 Format";
                    break;

                case "Y4":
                    str = "Agency Claim Number";
                    break;

                default:
                    str = "### Unknown REF01 ###";
                    break;
            }

            this.rowName.REF01 += str + ":";
            return str;
        }

        private string TRN(string StrIn)
        {
            string str3;
            try
            {
                string[] strArray = StrIn.Split(new char[] { MDVUtility.D_ELMT });
                if (strArray[0] != "TRN")
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int num2 in new int[] { 1, 2, 3, 4 })
                {
                    string str;
                    string str2;
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
                                this.rowName.TRN02 = strArray[num2];
                                break;

                            case 3:
                                str = strArray[num2].Substring(0, 1);
                                str2 = strArray[num2].Substring(1);
                                builder.Append(Environment.NewLine);
                                builder.Append("Originating Company Identifier");
                                if (!(str == "1"))
                                {
                                    goto Label_015D;
                                }
                                builder.Append("(EIN)");
                                this.rowName.TRN03 = strArray[num2];
                                goto Label_01C5;

                            case 4:
                                builder.Append("\t");
                                builder.Append(strArray[num2]);
                                this.rowName.TRN04 = strArray[num2];
                                break;
                        }
                    }
                    continue;
                Label_015D:
                    switch (str)
                    {
                        case "3":
                            builder.Append("(DUNS)");
                            this.rowName.TRN03 = strArray[num2];
                            break;

                        case "9":
                            builder.Append("(User Assigned Identifier)");
                            this.rowName.TRN03 = strArray[num2];
                            break;
                    }
                Label_01C5:
                    builder.Append(": ");
                    builder.Append(str2);
                    this.rowName.TRN03 = this.rowName.TRN03 + " " + str2;
                }
                builder.Append(Environment.NewLine);
                str3 = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str3;
        }

        private string TRN01(string Code)
        {
            string str = "Unknown";
            string str3 = Code;
           
            str = "### Unknown TRN01 ###";
        Label_0040:
            this.rowName.TRN01 = str;
            return str;
        }
    }
}

