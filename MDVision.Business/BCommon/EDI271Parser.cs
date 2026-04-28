//using EDIParser;
//using MDVision.Datasets;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MDVision.Business.BCommon
//{
//    public class EDI271Parser
//    {
//        #region " Declaration "


//        private char D_S_S_ELMT = '^';
//        private string III01_Value;
//        private string DTP02_Value;
//        private string MPI06_Value;
//        private string EB01_Value;
//        private bool EB03_Equal = false;
//        private StringBuilder FinalStr271;
//        private DS271 ds271Parser = new DS271();
//        private DS271 ds271tempParser = new DS271();
//        private long NameId = 0;
//        private long BenefitsId = 0;
//        string ServiceTypeCode = string.Empty;


//        private DS271.EDI271HeaderRow rowHeader;
//        private DS271.EDI271NamesRow rowName;
//        private DS271.EDI271BenefitsRow rowBenefits;
//        private DS271.EDI271BenefitsDetailRow rowBenefitsDetail;

//        private float Copay = 0;
//        private float Deductible = 0;

//        #endregion

//        #region " Segments "

//        private string MPI(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "MPI")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 5, 6, 7 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(MPI01(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 2:
//                                    {
//                                        StrOut.Append(MPI02(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        StrOut.Append(MPI03(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 4:
//                                    {
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        this.rowName.MPI04 = arrElmt[ElementNo];
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 5:
//                                    {
//                                        StrOut.Append(MPI05(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 6:
//                                    {
//                                        MPI06_Value = arrElmt[ElementNo];
//                                        StrOut.Append(MPI06_Value);
//                                        this.rowName.MPI06 = arrElmt[ElementNo];
//                                        StrOut.Append(": ");
//                                    }
//                                    break;

//                                case 7:
//                                    {
//                                        DateTime dt;
//                                        if (MPI06_Value == "D8")
//                                        {
//                                            dt = EDIUtility.StringToDate(arrElmt[ElementNo]);
//                                            StrOut.Append(dt.ToShortDateString());
//                                            this.rowName.MPI07 = dt.ToShortDateString();

//                                        }
//                                        else if (MPI06_Value == "RD8")
//                                        {
//                                            string[] arrSubElmt = arrElmt[ElementNo].Split('-');
//                                            for (int SubElmtNo = 0; SubElmtNo < arrSubElmt.Length; SubElmtNo++)
//                                            {
//                                                if (SubElmtNo == 0)
//                                                {
//                                                    dt = EDIUtility.StringToDate(arrElmt[0]);
//                                                    StrOut.Append(dt.ToShortDateString());
//                                                    this.rowName.MPI07 = dt.ToShortDateString();
//                                                }
//                                                else if (SubElmtNo == 1)
//                                                {
//                                                    StrOut.Append(" - ");
//                                                    dt = EDIUtility.StringToDate(arrElmt[1]);
//                                                    StrOut.Append(dt.ToShortDateString());
//                                                    this.rowName.MPI07 += " - " + dt.ToShortDateString();
//                                                }

//                                            }
//                                        }
//                                    }
//                                    break;
//                            }
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string DTP(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "DTP")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(DTP01(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 2:
//                                    {
//                                        DTP02_Value = arrElmt[ElementNo];
//                                        if (IsBenefitsDTP())
//                                            this.rowBenefits.DTP02 = DTP02_Value;
//                                        else
//                                            this.rowName.DTP02 = DTP02_Value;
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        DateTime dt;
//                                        if (DTP02_Value == "D8")
//                                        {
//                                            dt = EDIUtility.StringToDate(arrElmt[ElementNo]);
//                                            StrOut.Append(dt.ToShortDateString());
//                                            if (IsBenefitsDTP())
//                                                this.rowBenefits.DTP03 = dt.ToShortDateString();
//                                            else
//                                                this.rowName.DTP03 = dt.ToShortDateString();

//                                        }
//                                        else if (DTP02_Value == "RD8")
//                                        {
//                                            string[] arrSubElmt = arrElmt[ElementNo].Split('-');
//                                            for (int SubElmtNo = 0; SubElmtNo < arrSubElmt.Length; SubElmtNo++)
//                                            {
//                                                if (SubElmtNo == 0)
//                                                {
//                                                    dt = EDIUtility.StringToDate(arrElmt[0]);
//                                                    StrOut.Append(dt.ToShortDateString());
//                                                    if (IsBenefitsDTP())
//                                                        this.rowBenefits.DTP03 = dt.ToShortDateString();
//                                                    else
//                                                        this.rowName.DTP03 = dt.ToShortDateString();
//                                                }
//                                                else if (SubElmtNo == 1)
//                                                {
//                                                    StrOut.Append("-");
//                                                    dt = EDIUtility.StringToDate(arrElmt[1]);
//                                                    StrOut.Append(dt.ToShortDateString());
//                                                    if (IsBenefitsDTP())
//                                                        this.rowBenefits.DTP03 += " - " + dt.ToShortDateString();
//                                                    else
//                                                        this.rowName.DTP03 += " - " + dt.ToShortDateString();
//                                                }

//                                            }
//                                        }
//                                    }
//                                    break;
//                            }
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string NM1(string StrIn)
//        {

//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "NM1")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 6, 3, 4, 5, 7, 8, 9, 10, 11, 12 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(NM101(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        this.rowName.NM103 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                                case 4:
//                                    {
//                                        this.rowName.NM104 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                                case 5:
//                                    {
//                                        this.rowName.NM105 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                                case 6:
//                                    {
//                                        this.rowName.NM106 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                                case 8:
//                                    {
//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append(NM108(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 9:
//                                    {
//                                        this.rowName.NM109 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                                default:
//                                    {
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(" ");
//                                    }
//                                    break;
//                            }
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }


//        }

//        private string N4(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "N4")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        this.rowName.N401 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(" ");
//                                    }
//                                    break;
//                                case 2:
//                                    {
//                                        this.rowName.N402 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(" ");
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        this.rowName.N403 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                    }
//                                    break;
//                            }
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        private string N3(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "N3")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    this.rowName.N301 = arrElmt[ElementNo];
//                                    break;
//                                case 2:
//                                    this.rowName.N302 = arrElmt[ElementNo];
//                                    break;
//                            }

//                            StrOut.Append(arrElmt[ElementNo]);
//                            StrOut.Append(Environment.NewLine);
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        private string PER(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "PER")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 5, 6, 7, 8 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(PER01(arrElmt[ElementNo]));
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 2:
//                                    {
//                                        this.rowName.PER02 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        this.rowName.PER03 = arrElmt[ElementNo];
//                                        StrOut.Append(PER03(arrElmt[ElementNo]));
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 4:
//                                    {
//                                        this.rowName.PER04 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 5:
//                                    {
//                                        this.rowName.PER05 = arrElmt[ElementNo];
//                                        StrOut.Append(PER05(arrElmt[ElementNo]));
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 6:
//                                    {
//                                        this.rowName.PER06 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 7:
//                                    {
//                                        this.rowName.PER07 = arrElmt[ElementNo];
//                                        StrOut.Append(PER07(arrElmt[ElementNo]));
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                                case 8:
//                                    {
//                                        this.rowName.PER08 = arrElmt[ElementNo];
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append(Environment.NewLine);
//                                    }
//                                    break;
//                            }
//                        }
//                    }

//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        private string REF(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "REF")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(REF01(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 2:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowName.REF02 = arrElmt[ElementNo];
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string TRN(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "TRN")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(TRN01(arrElmt[ElementNo]));
//                                        StrOut.Append(": ");
//                                    }
//                                    break;
//                                case 2:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowName.TRN02 = arrElmt[ElementNo];
//                                    break;
//                                case 3:
//                                    {
//                                        string PreText = arrElmt[ElementNo].Substring(0, 1);
//                                        string PostText = arrElmt[ElementNo].Substring(1);
//                                        StrOut.Append(Environment.NewLine);

//                                        StrOut.Append("Originating Company Identifier");

//                                        if (PreText == "1")
//                                        { StrOut.Append("(EIN)"); this.rowName.TRN03 = arrElmt[ElementNo]; }
//                                        else if (PreText == "3")
//                                        { StrOut.Append("(DUNS)"); this.rowName.TRN03 = arrElmt[ElementNo]; }
//                                        else if (PreText == "9")
//                                        { StrOut.Append("(User Assigned Identifier)"); this.rowName.TRN03 = arrElmt[ElementNo]; }

//                                        StrOut.Append(": ");
//                                        StrOut.Append(PostText);
//                                        this.rowName.TRN03 += " " + PostText;
//                                    }
//                                    break;
//                                case 4:
//                                    {
//                                        StrOut.Append("\t");
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        this.rowName.TRN04 = arrElmt[ElementNo];
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string HL(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "HL")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 3 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append("  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
//                                    }
//                                    break;
//                                case 3:
//                                    {
//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append("Hierarchical Level: ");
//                                        StrOut.Append(HL03(arrElmt[ElementNo]));
//                                    }
//                                    break;
//                                default:
//                                    {
//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append("## this HL segment not in use ##");
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string PRV(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "PRV")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Provider: ");
//                                    StrOut.Append(PRV01(arrElmt[ElementNo]));

//                                    break;
//                                case 2:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append(PRV02(arrElmt[ElementNo]));
//                                    StrOut.Append(": ");

//                                    break;
//                                case 3:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowName.PRV03 = arrElmt[ElementNo];
//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this PRV segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string III(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "III")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append(III01(arrElmt[ElementNo]));
//                                    III01_Value = arrElmt[ElementNo];

//                                    break;
//                                case 2:
//                                    if (III01_Value == "ZZ")
//                                    {
//                                        //BLClaimPlaceOfService POS = new BLClaimPlaceOfService();
//                                        //StrOut.Append(POS.GetName(arrElmt[ElementNo]));
//                                    }
//                                    else
//                                    {
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                    }

//                                    break;

//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this III segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string MSG(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "MSG")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Free Form Message Text: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefits.MSG01 = arrElmt[ElementNo];
//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this MSG segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string HSD(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "HSD")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 5, 6, 7, 8 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append(HSD01(arrElmt[ElementNo]));
//                                    StrOut.Append(": ");

//                                    break;
//                                case 2:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefitsDetail.HSD02 = arrElmt[ElementNo];
//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Unit or Basis for Measurement: ");
//                                    StrOut.Append(HSD03(arrElmt[ElementNo]));

//                                    break;
//                                case 4:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Sample Selection Modulus: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefitsDetail.HSD03 = arrElmt[ElementNo];
//                                    break;
//                                case 5:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append(HSD05(arrElmt[ElementNo]));
//                                    StrOut.Append(": ");

//                                    break;
//                                case 6:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefitsDetail.HSD06 = arrElmt[ElementNo];
//                                    break;
//                                case 7:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Delivery Frequency: ");
//                                    StrOut.Append(HSD07(arrElmt[ElementNo]));

//                                    break;
//                                case 8:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Delivery Pattern Time: ");
//                                    StrOut.Append(HSD08(arrElmt[ElementNo]));

//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this HSD segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string EB(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "EB")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    this.rowBenefits = GetNew271BenefitsRow();
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Eligibility or Benefit Information: ");
//                                    StrOut.Append(EB01(arrElmt[ElementNo]));
//                                    EB01_Value = arrElmt[ElementNo];
//                                    break;
//                                case 2:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Coverage Level: ");
//                                    StrOut.Append(EB02(arrElmt[ElementNo]));

//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Service Type: ");
//                                    StrOut.Append(EB03(arrElmt[ElementNo]));

//                                    break;
//                                case 4:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Insurance Type: ");
//                                    StrOut.Append(EB04(arrElmt[ElementNo]));

//                                    break;
//                                case 5:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Plan Coverage Description: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefits.EB05 = arrElmt[ElementNo];
//                                    break;
//                                case 6:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Time Period Qualifier: ");
//                                    StrOut.Append(EB06(arrElmt[ElementNo]));

//                                    break;
//                                case 7:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Benefit Amount: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefits.EB07 = arrElmt[ElementNo];

//                                    //Co-payment and Deductible amount.
//                                    if (EB01_Value == "B" && EB03_Equal)
//                                        Copay += MDVUtility.Tofloat(arrElmt[ElementNo]);
//                                    else if (EB01_Value == "C" && EB03_Equal)
//                                        Deductible += MDVUtility.Tofloat(arrElmt[ElementNo]);

//                                    break;
//                                case 8:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Benefit Percent: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefits.EB08 = arrElmt[ElementNo];
//                                    break;
//                                case 9:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append(EB09(arrElmt[ElementNo]));
//                                    StrOut.Append(": ");

//                                    break;
//                                case 10:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowBenefits.EB10 = arrElmt[ElementNo];
//                                    break;
//                                case 11:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Authorization or Certification Indicator: ");
//                                    StrOut.Append(EB11(arrElmt[ElementNo]));

//                                    break;
//                                case 12:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("In Plan Network Indicator: ");
//                                    StrOut.Append(EB12(arrElmt[ElementNo]));

//                                    break;
//                                case 13:
//                                    StrOut.Append(Environment.NewLine);
//                                    string[] arrSubElmt = arrElmt[ElementNo].Split(EDIUtility.D_S_ELMT);
//                                    int SubElmtNo = 0;
//                                    for (SubElmtNo = 0; SubElmtNo <= arrSubElmt.Length - 1; SubElmtNo++)
//                                    {
//                                        if (SubElmtNo == 0)
//                                        {
//                                            StrOut.Append(EB13_1(arrSubElmt[SubElmtNo]));
//                                            StrOut.Append(": ");

//                                        }
//                                        else if (SubElmtNo == 1)
//                                        {
//                                            StrOut.Append("\t");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 = arrSubElmt[SubElmtNo];

//                                        }
//                                        else if (SubElmtNo == 2)
//                                        {
//                                            StrOut.Append("\t");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 += arrSubElmt[SubElmtNo];
//                                        }
//                                        else if (SubElmtNo == 3)
//                                        {
//                                            StrOut.Append("\t");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 += arrSubElmt[SubElmtNo];
//                                        }
//                                        else if (SubElmtNo == 4)
//                                        {
//                                            StrOut.Append("\t");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 += arrSubElmt[SubElmtNo];
//                                        }
//                                        else if (SubElmtNo == 5)
//                                        {
//                                            StrOut.Append("\t");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 += arrSubElmt[SubElmtNo];
//                                        }
//                                        else if (SubElmtNo == 6)
//                                        {
//                                            StrOut.Append(Environment.NewLine);
//                                            StrOut.Append("Description: ");
//                                            StrOut.Append(arrSubElmt[SubElmtNo]);
//                                            this.rowBenefits.EB13_2 += arrSubElmt[SubElmtNo];
//                                        }
//                                    }


//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this EB segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string INS(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "INS")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 9, 10, 17 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Insured Indicator: ");
//                                    StrOut.Append(INS01(arrElmt[ElementNo]));
//                                    this.rowName.INS01 = arrElmt[ElementNo];
//                                    break;
//                                case 2:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Individual Relationship: ");
//                                    StrOut.Append(INS02(arrElmt[ElementNo]));

//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Maintenance Type: ");
//                                    StrOut.Append(INS03(arrElmt[ElementNo]));

//                                    break;
//                                case 4:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Maintenance Reason: ");
//                                    StrOut.Append(INS04(arrElmt[ElementNo]));

//                                    break;
//                                case 9:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Student Status: ");
//                                    StrOut.Append(INS09(arrElmt[ElementNo]));

//                                    break;
//                                case 10:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Handicap Indicator: ");
//                                    StrOut.Append(INS10(arrElmt[ElementNo]));
//                                    this.rowName.INS10 = arrElmt[ElementNo];
//                                    break;
//                                case 17:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Birth Sequence Number: ");
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowName.INS17 = arrElmt[ElementNo];
//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this INS segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string DMG(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "DMG")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 2, 3 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 2:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Subscriber Birth Date: ");
//                                    System.DateTime dt = EDIUtility.StringToDate(arrElmt[ElementNo]);
//                                    StrOut.Append(dt.ToShortDateString());
//                                    this.rowName.DMG02 = dt.ToShortDateString();
//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Subscriber Gender: ");
//                                    StrOut.Append(DMG03(arrElmt[ElementNo]));
//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this DMG segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string AAA(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "AAA")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append("Valid Request Indicator: ");
//                                    StrOut.Append(AAA01(arrElmt[ElementNo]));
//                                    this.rowName.AAA01 = arrElmt[ElementNo];
//                                    break;
//                                case 2:
//                                    StrOut.Append(arrElmt[ElementNo]);
//                                    this.rowName.AAA02 = arrElmt[ElementNo];
//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Reject Reason: ");
//                                    StrOut.Append(AAA03(arrElmt[ElementNo]));

//                                    break;
//                                case 4:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Follow-up Action: ");
//                                    StrOut.Append(AAA04(arrElmt[ElementNo]));

//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this AAA segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string BHT(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "BHT")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1, 2, 3, 4, 5 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Hierarchical Structure: ");
//                                    StrOut.Append(BHT01(arrElmt[ElementNo]));

//                                    break;
//                                case 2:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Transaction Set Purpose: ");
//                                    StrOut.Append(BHT02(arrElmt[ElementNo]));

//                                    break;
//                                case 3:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Submitter Transaction Identifier: ");
//                                    StrOut.Append(arrElmt[ElementNo]);

//                                    break;
//                                case 4:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Transaction Set Creation Date: ");
//                                    System.DateTime dt = EDIUtility.StringToDate(arrElmt[ElementNo]);
//                                    StrOut.Append(dt.ToShortDateString());

//                                    break;
//                                case 5:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("Transaction Set Creation Time: ");
//                                    StrOut.Append(arrElmt[ElementNo]);

//                                    break;
//                                default:
//                                    StrOut.Append(Environment.NewLine);
//                                    StrOut.Append("## this BHT segment not in use ##");
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private string LS(string StrIn)
//        {
//            try
//            {
//                string[] arrElmt = StrIn.Split((new char[] { EDIUtility.D_ELMT }));

//                if (arrElmt[0] != "LS")
//                    return String.Empty;

//                StringBuilder StrOut = new StringBuilder();
//                int[] order = { 1 };
//                for (int i = 0; i < order.Length; i++)
//                {
//                    int ElementNo = order[i];

//                    if (ElementNo < arrElmt.Length)
//                    {
//                        if (arrElmt[ElementNo].Length > 0)
//                        {
//                            switch (ElementNo)
//                            {
//                                case 1:
//                                    {
//                                        if (this.rowBenefits != null && EB01_Value == "L")
//                                        {

//                                            this.rowName = this.GetNew271NameRow();
//                                        }
//                                        else
//                                            this.rowName = this.GetNew271TempNameRow();

//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append(arrElmt[ElementNo]);
//                                        StrOut.Append("  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
//                                    }
//                                    break;
//                                default:
//                                    {
//                                        StrOut.Append(Environment.NewLine);
//                                        StrOut.Append("## this HL segment not in use ##");
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                }

//                StrOut.Append(Environment.NewLine);
//                return StrOut.ToString();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        #endregion

//        #region " Elements "

//        private string MPI05(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "A1":
//                    StrOut = "Admiral";
//                    break;
//                case "A2":
//                    StrOut = "Airman";
//                    break;
//                case "A3":
//                    StrOut = "Airman First Class";
//                    break;
//                case "B1":
//                    StrOut = "Basic Airman";
//                    break;
//                case "B2":
//                    StrOut = "Brigadier General";
//                    break;
//                case "C1":
//                    StrOut = "Captain";
//                    break;
//                case "C2":
//                    StrOut = "Chief Master Sergeant";
//                    break;
//                case "C3":
//                    StrOut = "Chief Petty Officer";
//                    break;
//                case "C4":
//                    StrOut = "Chief Warrant";
//                    break;
//                case "C5":
//                    StrOut = "Colonel";
//                    break;
//                case "C6":
//                    StrOut = "Commander";
//                    break;
//                case "C7":
//                    StrOut = "Commodore";
//                    break;
//                case "C8":
//                    StrOut = "Corporal";
//                    break;
//                case "C9":
//                    StrOut = "Corporal Specialist 4";
//                    break;
//                case "E1":
//                    StrOut = "Ensign";
//                    break;
//                case "F1":
//                    StrOut = "First Lieutenant";
//                    break;
//                case "F2":
//                    StrOut = "First Sergeant";
//                    break;
//                case "F3":
//                    StrOut = "First Sergeant-Master Sergeant";
//                    break;
//                case "F4":
//                    StrOut = "Fleet Admiral";
//                    break;
//                case "G1":
//                    StrOut = "General";
//                    break;
//                case "G4":
//                    StrOut = "General Sergeant";
//                    break;
//                case "L1":
//                    StrOut = "Lance Corporal";
//                    break;
//                case "L2":
//                    StrOut = "Lieutenant";
//                    break;
//                case "L3":
//                    StrOut = "Lieutenant Colonel";
//                    break;
//                case "L4":
//                    StrOut = "Lieutenant Commodore";
//                    break;
//                case "L5":
//                    StrOut = "Lieutenant General";
//                    break;
//                case "L6":
//                    StrOut = "Lieutenant Junior Grade";
//                    break;
//                case "M1":
//                    StrOut = "Major";
//                    break;
//                case "M2":
//                    StrOut = "Major General";
//                    break;
//                case "M3":
//                    StrOut = "Master Chief Petty Officer";
//                    break;
//                case "M4":
//                    StrOut = "Master Gunnery Sergeant Major";
//                    break;
//                case "M5":
//                    StrOut = "Master Sergeant";
//                    break;
//                case "M6":
//                    StrOut = "Master Sergeant Specialist 8";
//                    break;
//                case "P1":
//                    StrOut = "Petty Officer First Class";
//                    break;
//                case "P2":
//                    StrOut = "Petty Officer Second Class";
//                    break;
//                case "P3":
//                    StrOut = "Petty Officer Third Class";
//                    break;
//                case "P4":
//                    StrOut = "Private";
//                    break;
//                case "P5":
//                    StrOut = "Private First Class";
//                    break;
//                case "R1":
//                    StrOut = "Rear Admiral";
//                    break;
//                case "R2":
//                    StrOut = "Recruit";
//                    break;
//                case "S1":
//                    StrOut = "Seaman";
//                    break;
//                case "S2":
//                    StrOut = "Seaman Apprentice";
//                    break;
//                case "S3":
//                    StrOut = "Seaman Recruit";
//                    break;
//                case "S4":
//                    StrOut = "Second Lieutenant";
//                    break;
//                case "S5":
//                    StrOut = "Senior Chief Petty Officer";
//                    break;
//                case "S6":
//                    StrOut = "Senior Master Sergeant";
//                    break;
//                case "S7":
//                    StrOut = "Sergeant";
//                    break;
//                case "S8":
//                    StrOut = "Sergeant First Class Specialist 7";
//                    break;
//                case "S9":
//                    StrOut = "Sergeant Major Specialist 9";
//                    break;
//                case "SA":
//                    StrOut = "Sergeant Specialist 5";
//                    break;
//                case "SB":
//                    StrOut = "Staff Sergeant";
//                    break;
//                case "SC":
//                    StrOut = "Staff Sergeant Specialist 6";
//                    break;
//                case "T1":
//                    StrOut = "Technical Sergeant";
//                    break;
//                case "V1":
//                    StrOut = "Vice Admiral";
//                    break;
//                case "W1":
//                    StrOut = "Warrant Officer";
//                    break;

//                default:
//                    StrOut = "### Unknown MPI05 ###";
//                    break;
//            }

//            this.rowName.MPI05 = StrOut;
//            return StrOut;
//        }

//        private string MPI04(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "096":
//                    StrOut = "Discharge";
//                    break;
//                default:
//                    StrOut = "### Unknown MPI04 ###";
//                    break;
//            }

//            this.rowName.MPI04 = StrOut;
//            return StrOut;
//        }

//        private string MPI03(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "A":
//                    StrOut = "Air Force";
//                    break;
//                case "B":
//                    StrOut = "Air Force Reserves";
//                    break;
//                case "C":
//                    StrOut = "Army";
//                    break;
//                case "D":
//                    StrOut = "Army Reserves";
//                    break;
//                case "E":
//                    StrOut = "Coast Guard";
//                    break;
//                case "F":
//                    StrOut = "Marine Corps";
//                    break;
//                case "G":
//                    StrOut = "Marine Corps Reserves";
//                    break;
//                case "H":
//                    StrOut = "National Guard";
//                    break;
//                case "I":
//                    StrOut = "Navy";
//                    break;
//                case "J":
//                    StrOut = "Navy Reserves";
//                    break;
//                case "K":
//                    StrOut = "Other";
//                    break;
//                case "L":
//                    StrOut = "Peace Corp";
//                    break;
//                case "M":
//                    StrOut = "Regular Armed Forces";
//                    break;
//                case "N":
//                    StrOut = "Reserves";
//                    break;
//                case "O":
//                    StrOut = "U.S. Public Health Service";
//                    break;
//                case "Q":
//                    StrOut = "Foreign Military";
//                    break;
//                case "R":
//                    StrOut = "American Red Cross";
//                    break;
//                case "S":
//                    StrOut = "Department of Defense";
//                    break;
//                case "U":
//                    StrOut = "United Services Organization";
//                    break;
//                case "W":
//                    StrOut = "Military Sealift Command";
//                    break;

//                default:
//                    StrOut = "### Unknown MPI03 ###";
//                    break;
//            }

//            this.rowName.MPI03 = StrOut;
//            return StrOut;
//        }

//        private string MPI02(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "AE":
//                    StrOut = "Active Reserve";
//                    break;
//                case "AO":
//                    StrOut = "Active Military - Overseas";
//                    break;
//                case "AS":
//                    StrOut = "Academy Student";
//                    break;
//                case "AT":
//                    StrOut = "Presidential Appointee";
//                    break;
//                case "AU":
//                    StrOut = "Active Military - USA";
//                    break;
//                case "CC":
//                    StrOut = "Contractor";
//                    break;
//                case "DD":
//                    StrOut = "Dishonorably Discharged";
//                    break;
//                case "HD":
//                    StrOut = "Honorably Discharged";
//                    break;
//                case "IR":
//                    StrOut = "Inactive Reserves";
//                    break;
//                case "LX":
//                    StrOut = "Leave of Absence: Military";
//                    break;
//                case "PE":
//                    StrOut = "Plan to Enlist";
//                    break;
//                case "RE":
//                    StrOut = "Recommissioned";
//                    break;
//                case "RM":
//                    StrOut = "Retired Military - Overseas";
//                    break;
//                case "RR":
//                    StrOut = "Retired Without Recall";
//                    break;
//                case "RU":
//                    StrOut = "Retired Military - USA";
//                    break;
//                default:
//                    StrOut = "### Unknown MPI02 ###";
//                    break;
//            }

//            this.rowName.MPI02 = StrOut;
//            return StrOut;
//        }

//        private string MPI01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "A":
//                    StrOut = "Partial";
//                    break;
//                case "C":
//                    StrOut = "Current";
//                    break;
//                case "L":
//                    StrOut = "Latest";
//                    break;
//                case "O":
//                    StrOut = "Oldest";
//                    break;
//                case "P":
//                    StrOut = "Prior";
//                    break;
//                case "S":
//                    StrOut = "Second Most Current";
//                    break;
//                case "T":
//                    StrOut = "Third Most Current";
//                    break;
//                default:
//                    StrOut = "### Unknown MPI01 ###";
//                    break;
//            }

//            this.rowName.MPI01 = StrOut;
//            return StrOut;
//        }
//        private string DTP01(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "096":
//                    StrOut = "Discharge";
//                    break;
//                case "102":
//                    StrOut = "Issue";
//                    break;
//                case "152":
//                    StrOut = "Effective Date of Change";
//                    break;
//                case "291":
//                    StrOut = "Plan";
//                    break;
//                case "307":
//                    StrOut = "Eligibility";
//                    break;
//                case "318":
//                    StrOut = "Added";
//                    break;
//                case "340":
//                    StrOut = "Consolidated Omnibus Budget Reconciliation Act (COBRA) Begin";
//                    break;
//                case "341":
//                    StrOut = "Consolidated Omnibus Budget Reconciliation Act (COBRA) End";
//                    break;
//                case "342":
//                    StrOut = "Premium Paid to Date Begin";
//                    break;
//                case "343":
//                    StrOut = "Premium Paid to Date End";
//                    break;
//                case "346":
//                    StrOut = "Plan Begin";
//                    break;
//                case "347":
//                    StrOut = "Plan End";
//                    break;
//                case "356":
//                    StrOut = "Eligibility Begin";
//                    break;
//                case "357":
//                    StrOut = "Eligibility End";
//                    break;
//                case "382":
//                    StrOut = "Enrollment";
//                    break;
//                case "435":
//                    StrOut = "Admission";
//                    break;
//                case "442":
//                    StrOut = "Date of Death";
//                    break;
//                case "458":
//                    StrOut = "Certification";
//                    break;
//                case "472":
//                    StrOut = "Service";
//                    break;
//                case "539":
//                    StrOut = "Policy Effective";
//                    break;
//                case "540":
//                    StrOut = "Policy Expiration";
//                    break;
//                case "636":
//                    StrOut = "Date of Last Update";
//                    break;
//                case "771":
//                    StrOut = "Status";
//                    break;
//                case "193":
//                    StrOut = "Period Start";
//                    break;
//                case "194":
//                    StrOut = "Period End";
//                    break;
//                case "198":
//                    StrOut = "Completion";
//                    break;
//                case "290":
//                    StrOut = "Coordination of Benefits";
//                    break;
//                case "292":
//                    StrOut = "Benefit";
//                    break;
//                case "295":
//                    StrOut = "Primary Care Provider";
//                    break;
//                case "304":
//                    StrOut = "Latest Visit or Consultation";
//                    break;
//                case "348":
//                    StrOut = "Benefit Begin";
//                    break;
//                case "349":
//                    StrOut = "Benefit End";
//                    break;

//                default:
//                    StrOut = "### Unknown DTP01 ###";
//                    break;
//            }

//            if (IsBenefitsDTP())
//                this.rowBenefits.DTP01 = StrOut;
//            else
//                this.rowName.DTP01 = StrOut;
//            return StrOut;
//        }

//        private string NM101(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "2B":
//                    StrOut = "Third-Party Administrator"; break;
//                case "36":
//                    StrOut = "Employer"; break;
//                case "GP":
//                    StrOut = "Gateway Provider"; break;
//                case "P5":
//                    StrOut = "Plan Sponsor"; break;
//                case "PR":
//                    StrOut = "Payer"; break;
//                case "1P":
//                    StrOut = "Provider"; break;
//                case "80":
//                    StrOut = "Hospital"; break;
//                case "FA":
//                    StrOut = "Facility"; break;
//                case "IL":
//                    StrOut = "Insured or Subscriber"; break;
//                case "13":
//                    StrOut = "Contracted Service Provider"; break;
//                case "73":
//                    StrOut = "Other Physician"; break;
//                case "LR":
//                    StrOut = "Legal Representative"; break;
//                case "P3":
//                    StrOut = "Primary Care Provider"; break;
//                case "P4":
//                    StrOut = "Prior Insurance Carrier"; break;
//                case "PRP":
//                    StrOut = "Primary Payer"; break;
//                case "SEP":
//                    StrOut = "Secondary Payer"; break;
//                case "TTP":
//                    StrOut = "Tertiary Payer"; break;
//                case "VN":
//                    StrOut = "Vendor"; break;
//                case "X3":
//                    StrOut = "Utilization Management Organization"; break;
//                case "03":
//                    StrOut = "Dependent"; break;

//                default:
//                    StrOut = "### Unknown NM101 ###";
//                    break;
//            }
//            rowName.NM101 = StrOut;
//            rowName.NM101Code = Code;
//            return StrOut;

//        }

//        private string NM108(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "24":
//                    StrOut = "Employer’s Identification Number"; break;
//                case "46":
//                    StrOut = "Electronic Transmitter Identification Number (ETIN)"; break;
//                case "FI":
//                    StrOut = "Federal Taxpayer’s Identification Number"; break;
//                case "NI":
//                    StrOut = "National Association of Insurance Commissioners (NAIC) Identification"; break;
//                case "PI":
//                    StrOut = "Payor Identification"; break;
//                case "XV":
//                    StrOut = "Health Care Financing Administration National PlanID"; break;
//                case "XX":
//                    StrOut = "Health Care Financing Administration National Provider Identifier"; break;
//                case "34":
//                    StrOut = "Social Security Number"; break;
//                case "PP":
//                    StrOut = "Pharmacy Processor Number"; break;
//                case "SV":
//                    StrOut = "Service Provider Number"; break;
//                case "MI":
//                    StrOut = "Member Identification Number"; break;
//                case "ZZ":
//                    StrOut = "Mutually Defined"; break;
//                case "FA":
//                    StrOut = "Facility Identification"; break;
//                case "N4":
//                    StrOut = "National Drug Code in 5-4-2 Format"; break;
//                default:
//                    StrOut = "### Unknown NM108 ###";
//                    break;
//            }

//            this.rowName.NM108 = StrOut;
//            return StrOut;

//        }

//        private string PER07(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "ED":
//                    StrOut = "Electronic Data Interchange Access Number"; break;
//                case "EM":
//                    StrOut = "Electronic Mail"; break;
//                case "EX":
//                    StrOut = "Telephone Extension"; break;
//                case "FX":
//                    StrOut = "Facsimile"; break;
//                case "TE":
//                    StrOut = "Telephone"; break;
//                case "HP":
//                    StrOut = "Home Phone Number"; break;
//                case "UR":
//                    StrOut = "Uniform Resource Locater (URL)"; break;
//                case "WP":
//                    StrOut = "Work Phone Number"; break;

//                default:
//                    StrOut = "### Unknown PER07 ###"; break;
//            }
//            return StrOut;
//        }

//        private string PER05(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "ED":
//                    StrOut = "Electronic Data Interchange Access Number"; break;
//                case "EM":
//                    StrOut = "Electronic Mail"; break;
//                case "EX":
//                    StrOut = "Telephone Extension"; break;
//                case "FX":
//                    StrOut = "Facsimile"; break;
//                case "HP":
//                    StrOut = "Home Phone Number"; break;
//                case "TE":
//                    StrOut = "Telephone"; break;
//                case "UR":
//                    StrOut = "Uniform Resource Locater (URL)"; break;
//                case "WP":
//                    StrOut = "Work Phone Number"; break;

//                default:
//                    StrOut = "### Unknown PER05 ###"; break;
//            }
//            return StrOut;
//        }

//        private string PER03(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "ED":
//                    StrOut = "Electronic Data Interchange Access Number"; break;
//                case "EM":
//                    StrOut = "Electronic Mail"; break;
//                case "FX":
//                    StrOut = "Facsimile"; break;
//                case "TE":
//                    StrOut = "Telephone"; break;
//                case "UR":
//                    StrOut = "Uniform Resource Locater (URL)"; break;
//                case "HP":
//                    StrOut = "Home Phone Number"; break;
//                case "WP":
//                    StrOut = "Work Phone Number"; break;

//                default:
//                    StrOut = "### Unknown PER03 ###"; break;
//            }
//            return StrOut;
//        }

//        private string PER01(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "IC":
//                    StrOut = "Information Contact"; break;

//                default:
//                    StrOut = "### Unknown PER01 ###"; break;
//            }

//            this.rowName.PER01 = StrOut;
//            return StrOut;
//        }

//        private string REF01(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "18":
//                    StrOut = "Plan Number"; break;
//                case "55":
//                    StrOut = "Sequence Number"; break;
//                case "0B":
//                    StrOut = "State License Number"; break;
//                case "1C":
//                    StrOut = "Medicare Provider Number"; break;
//                case "1D":
//                    StrOut = "Medicaid Provider Number"; break;
//                case "1J":
//                    StrOut = "Facility ID Number"; break;
//                case "4A":
//                    StrOut = "Personal Identification Number (PIN)"; break;
//                case "CT":
//                    StrOut = "Contract Number"; break;
//                case "EL":
//                    StrOut = "Electronic device pin number"; break;
//                case "EO":
//                    StrOut = "Submitter Identification Number"; break;
//                case "HPI":
//                    StrOut = "Health Care Financing Administration National Provider Identifier"; break;
//                case "JD":
//                    StrOut = "User Identification"; break;
//                case "N5":
//                    StrOut = "Provider Plan Network Identification Number"; break;
//                case "N7":
//                    StrOut = "Facility Network Identification Number"; break;
//                case "SY":
//                    StrOut = "Social Security Number"; break;
//                case "TJ":
//                    StrOut = "Federal Taxpayer’s Identification Number"; break;
//                case "1L":
//                    StrOut = "Group or Policy Number"; break;
//                case "1W":
//                    StrOut = "Member Identification Number"; break;
//                case "3H":
//                    StrOut = "Case Number"; break;
//                case "49":
//                    StrOut = "Family Unit Number"; break;
//                case "6P":
//                    StrOut = "Group Number"; break;
//                case "A6":
//                    StrOut = "Employee Identification Number"; break;
//                case "EA":
//                    StrOut = "Medical Record Identification Number"; break;
//                case "EJ":
//                    StrOut = "Patient Account Number"; break;
//                case "F6":
//                    StrOut = "Health Insurance Claim (HIC) Number"; break;
//                case "FO":
//                    StrOut = "Drug Formulary Number"; break;
//                case "GH":
//                    StrOut = "Identification Card Serial Number"; break;
//                case "HJ":
//                    StrOut = "Identity Card Number"; break;
//                case "IF":
//                    StrOut = "Issue Number"; break;
//                case "IG":
//                    StrOut = "Insurance Policy Number"; break;
//                case "ML":
//                    StrOut = "Military Rank/Civilian Pay Grade Number"; break;
//                case "N6":
//                    StrOut = "Plan Network Identification Number"; break;
//                case "NQ":
//                    StrOut = "Medicaid Recipient Identification Number"; break;
//                case "Q4":
//                    StrOut = "Prior Identifier Number"; break;
//                case "G1":
//                    StrOut = "Prior Authorization Number"; break;
//                case "9F":
//                    StrOut = "Referral Number"; break;
//                case "M7":
//                    StrOut = "Medical Assistance Category"; break;
//                case "IV":
//                    StrOut = "Home Infusion EDI Coalition (HIEC) Product/Service Code"; break;
//                case "N4":
//                    StrOut = "National Drug Code in 5-4-2 Format"; break;
//                case "Y4":
//                    StrOut = "Agency Claim Number"; break;

//                default:
//                    StrOut = "### Unknown REF01 ###"; break;
//            }

//            this.rowName.REF01 = StrOut;
//            return StrOut;
//        }

//        private string TRN01(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "1":
//                    StrOut = "Current Transaction Trace Numbers"; break;
//                case "2":
//                    StrOut = "Referenced Transaction Trace Numbers"; break;

//                default:
//                    StrOut = "### Unknown TRN01 ###"; break;
//            }

//            this.rowName.TRN01 = StrOut;
//            return StrOut;
//        }

//        private string HL03(string Code)
//        {
//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "20":
//                    StrOut = "Information Source"; break;
//                case "21":
//                    StrOut = "Information Receiver"; break;
//                case "22":
//                    StrOut = "Subscriber"; break;
//                case "23":
//                    StrOut = "Dependent"; break;
//                default:
//                    StrOut = "### Unknown HL03 ###"; break;
//            }
//            string[] codes = { "20", "21", "22", "23" };
//            if (codes.Contains(Code))
//                this.rowName = this.GetNew271NameRow();
//            return StrOut;
//        }

//        private string PRV02(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "9K":
//                    StrOut = "Servicer";
//                    break;
//                case "D3":
//                    StrOut = "National Association of Boards of Pharmacy Number";
//                    break;
//                case "EI":
//                    StrOut = "Employer’s Identification Number";
//                    break;
//                case "HPI":
//                    StrOut = "Health Care Financing Administration National Provider Identifier";
//                    break;
//                case "SY":
//                    StrOut = "Social Security Number";
//                    break;
//                case "TJ":
//                    StrOut = "Federal Taxpayer’s Identification Number";
//                    break;
//                case "ZZ":
//                    StrOut = "Taxonomy Code";
//                    break;
//                case "PXC":
//                    StrOut = "Health Care Provider Taxonomy Code";
//                    break;
//                default:
//                    StrOut = "### Unknown PRV02 ###";
//                    break;
//            }

//            this.rowName.PRV02 = StrOut;
//            return StrOut;

//        }

//        private string PRV01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "AD":
//                    StrOut = "Admitting";
//                    break;
//                case "AT":
//                    StrOut = "Attending";
//                    break;
//                case "BI":
//                    StrOut = "Billing";
//                    break;
//                case "CO":
//                    StrOut = "Consulting";
//                    break;
//                case "CV":
//                    StrOut = "Covering";
//                    break;
//                case "H":
//                    StrOut = "Hospital";
//                    break;
//                case "HH":
//                    StrOut = "Home Health Care";
//                    break;
//                case "LA":
//                    StrOut = "Laboratory";
//                    break;
//                case "OT":
//                    StrOut = "Other Physician";
//                    break;
//                case "P1":
//                    StrOut = "Pharmacist";
//                    break;
//                case "P2":
//                    StrOut = "Pharmacy";
//                    break;
//                case "PE":
//                    StrOut = "Performing";
//                    break;
//                case "R":
//                    StrOut = "Rural Health Clinic";
//                    break;
//                case "RF":
//                    StrOut = "Referring";
//                    break;
//                case "SK":
//                    StrOut = "Skilled Nursing Facility";
//                    break;
//                case "PC":
//                    StrOut = "Primary Care Physician";
//                    break;
//                case "SB":
//                    StrOut = "Submitting";
//                    break;
//                case "SU":
//                    StrOut = "Supervising";

//                    break;
//                default:
//                    StrOut = "### Unknown PRV01 ###";
//                    break;
//            }

//            this.rowName.PRV01 = StrOut;
//            return StrOut;

//        }

//        private string III01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "BF":
//                    StrOut = "Diagnosis";
//                    break;
//                case "BK":
//                    StrOut = "Principal Diagnosis";
//                    break;
//                case "ZZ":
//                    StrOut = "Place of Service";
//                    break;
//                default:
//                    StrOut = "### Unknown III01 ###";
//                    break;
//            }
//            return StrOut;

//        }

//        private string HSD01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "DY":
//                    StrOut = "Days";
//                    break;
//                case "FL":
//                    StrOut = "Units";
//                    break;
//                case "HS":
//                    StrOut = "Hours";
//                    break;
//                case "MN":
//                    StrOut = "Month";
//                    break;
//                case "VS":
//                    StrOut = "Visits";

//                    break;
//                default:
//                    StrOut = "### Unknown HSD01 ###";
//                    break;
//            }

//            this.rowBenefitsDetail.HSD01 = StrOut;
//            return StrOut;

//        }

//        private string HSD03(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "DA":
//                    StrOut = "Days";
//                    break;
//                case "MO":
//                    StrOut = "Month";
//                    break;
//                case "VS":
//                    StrOut = "Visits";
//                    break;
//                case "WK":
//                    StrOut = "Week";
//                    break;
//                case "YR":
//                    StrOut = "Years";

//                    break;
//                default:
//                    StrOut = "### Unknown HSD03 ###";
//                    break;
//            }

//            this.rowBenefitsDetail.HSD03 = StrOut;
//            return StrOut;

//        }

//        private string HSD05(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "6":
//                    StrOut = "Hour";
//                    break;
//                case "7":
//                    StrOut = "Day";
//                    break;
//                case "21":
//                    StrOut = "Years";
//                    break;
//                case "22":
//                    StrOut = "Service Year";
//                    break;
//                case "23":
//                    StrOut = "Calendar Year";
//                    break;
//                case "24":
//                    StrOut = "Year to Date";
//                    break;
//                case "25":
//                    StrOut = "Contract";
//                    break;
//                case "26":
//                    StrOut = "Episode";
//                    break;
//                case "27":
//                    StrOut = "Visit";
//                    break;
//                case "28":
//                    StrOut = "Outlier";
//                    break;
//                case "29":
//                    StrOut = "Remaining";
//                    break;
//                case "30":
//                    StrOut = "Exceeded";
//                    break;
//                case "31":
//                    StrOut = "Not Exceeded";
//                    break;
//                case "32":
//                    StrOut = "Lifetime";
//                    break;
//                case "33":
//                    StrOut = "Lifetime Remaining";
//                    break;
//                case "34":
//                    StrOut = "Month";
//                    break;
//                case "35":
//                    StrOut = "Week";

//                    break;
//                default:
//                    StrOut = "### Unknown HSD05 ###";
//                    break;
//            }

//            this.rowBenefitsDetail.HSD05 = StrOut;
//            return StrOut;

//        }

//        private string HSD07(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "1":
//                    StrOut = "1st Week of the Month";
//                    break;
//                case "2":
//                    StrOut = "2nd Week of the Month";
//                    break;
//                case "3":
//                    StrOut = "3rd Week of the Month";
//                    break;
//                case "4":
//                    StrOut = "4th Week of the Month";
//                    break;
//                case "5":
//                    StrOut = "5th Week of the Month";
//                    break;
//                case "6":
//                    StrOut = "1st & 3rd Weeks of the Month";
//                    break;
//                case "7":
//                    StrOut = "2nd & 4th Weeks of the Month";
//                    break;
//                case "8":
//                    StrOut = "1st Working Day of Period";
//                    break;
//                case "9":
//                    StrOut = "Last Working Day of Period";
//                    break;
//                case "A":
//                    StrOut = "Monday through Friday";
//                    break;
//                case "B":
//                    StrOut = "Monday through Saturday";
//                    break;
//                case "C":
//                    StrOut = "Monday through Sunday";
//                    break;
//                case "D":
//                    StrOut = "Monday";
//                    break;
//                case "E":
//                    StrOut = "Tuesday";
//                    break;
//                case "F":
//                    StrOut = "Wednesday";
//                    break;
//                case "G":
//                    StrOut = "Thursday";
//                    break;
//                case "H":
//                    StrOut = "Friday";
//                    break;
//                case "J":
//                    StrOut = "Saturday";
//                    break;
//                case "K":
//                    StrOut = "Sunday";
//                    break;
//                case "L":
//                    StrOut = "Monday through Thursday";
//                    break;
//                case "M":
//                    StrOut = "Immediately";
//                    break;
//                case "N":
//                    StrOut = "As Directed";
//                    break;
//                case "O":
//                    StrOut = "Daily Mon. through Fri";
//                    break;
//                case "P":
//                    StrOut = "1/2 Mon. & 1/2 Thurs.";
//                    break;
//                case "Q":
//                    StrOut = "1/2 Tues. & 1/2 Thurs.";
//                    break;
//                case "R":
//                    StrOut = "1/2 Wed. & 1/2 Fri.";
//                    break;
//                case "S":
//                    StrOut = "Once Anytime Mon. through Fri.";
//                    break;
//                case "SG":
//                    StrOut = "Tuesday through Friday";
//                    break;
//                case "SL":
//                    StrOut = "Monday, Tuesday and Thursday";
//                    break;
//                case "SP":
//                    StrOut = "Monday, Tuesday and Friday";
//                    break;
//                case "SX":
//                    StrOut = "Wednesday and Thursday";
//                    break;
//                case "SY":
//                    StrOut = "Monday, Wednesday and Thursday";
//                    break;
//                case "SZ":
//                    StrOut = "Tuesday, Thursday and Friday";
//                    break;
//                case "T":
//                    StrOut = "1/2 Tue. & 1/2 Fri.";
//                    break;
//                case "U":
//                    StrOut = "1/2 Mon. & 1/2 Wed.";
//                    break;
//                case "V":
//                    StrOut = "1/3 Mon., 1/3 Wed., 1/3 Fri.";
//                    break;
//                case "W":
//                    StrOut = "Whenever Necessary";
//                    break;
//                case "X":
//                    StrOut = "1/2 By Wed., Bal. By Fri.";
//                    break;
//                case "Y":
//                    StrOut = "None (Also Used to Cancel or Override a Previous Pattern";

//                    break;
//                default:
//                    StrOut = "### Unknown HSD07 ###";
//                    break;
//            }

//            this.rowBenefitsDetail.HSD07 = StrOut;
//            return StrOut;

//        }

//        private string HSD08(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "A":
//                    StrOut = "1st Shift (Normal Working Hours)";
//                    break;
//                case "B":
//                    StrOut = "2nd Shift";
//                    break;
//                case "C":
//                    StrOut = "3rd Shift";
//                    break;
//                case "D":
//                    StrOut = "A.M.";
//                    break;
//                case "E":
//                    StrOut = "P.M.";
//                    break;
//                case "F":
//                    StrOut = "As Directed";
//                    break;
//                case "G":
//                    StrOut = "Any Shift";
//                    break;
//                case "Y":
//                    StrOut = "None (Also Used to Cancel or Override a Previous Pattern)";

//                    break;
//                default:
//                    StrOut = "### Unknown HSD08 ###";
//                    break;
//            }

//            this.rowBenefitsDetail.HSD08 = StrOut;
//            return StrOut;

//        }

//        private string EB01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "1":
//                    StrOut = "Active Coverage";
//                    break;
//                case "2":
//                    StrOut = "Active - Full Risk Capitation";
//                    break;
//                case "3":
//                    StrOut = "Active - Services Capitated";
//                    break;
//                case "4":
//                    StrOut = "Active - Services Capitated to Primary Care Physician";
//                    break;
//                case "5":
//                    StrOut = "Active - Pending Investigation";
//                    break;
//                case "6":
//                    StrOut = "Inactive";
//                    break;
//                case "7":
//                    StrOut = "Inactive - Pending Eligibility Update";
//                    break;
//                case "8":
//                    StrOut = "Inactive - Pending Investigation";
//                    break;
//                case "A":
//                    StrOut = "Co-Insurance";
//                    break;
//                case "B":
//                    StrOut = "Co-Payment";
//                    break;
//                case "C":
//                    StrOut = "Deductible";
//                    break;
//                case "CB":
//                    StrOut = "Coverage Basis";
//                    break;
//                case "D":
//                    StrOut = "Benefit Description";
//                    break;
//                case "E":
//                    StrOut = "Exclusions";
//                    break;
//                case "F":
//                    StrOut = "Limitations";
//                    break;
//                case "G":
//                    StrOut = "Out of Pocket (Stop Loss)";
//                    break;
//                case "H":
//                    StrOut = "Unlimited";
//                    break;
//                case "I":
//                    StrOut = "Non-Covered";
//                    break;
//                case "J":
//                    StrOut = "Cost Containment";
//                    break;
//                case "K":
//                    StrOut = "Reserve";
//                    break;
//                case "L":
//                    StrOut = "Primary Care Provider";
//                    break;
//                case "M":
//                    StrOut = "Pre-existing Condition";
//                    break;
//                case "MC":
//                    StrOut = "Managed Care Coordinator";
//                    break;
//                case "N":
//                    StrOut = "Services Restricted to Following Provider";
//                    break;
//                case "O":
//                    StrOut = "Not Deemed a Medical Necessity";
//                    break;
//                case "P":
//                    StrOut = "Benefit Disclaimer";
//                    break;
//                case "Q":
//                    StrOut = "Second Surgical Opinion Required";
//                    break;
//                case "R":
//                    StrOut = "Other or Additional Payor";
//                    break;
//                case "S":
//                    StrOut = "Prior Year(s) History";
//                    break;
//                case "T":
//                    StrOut = "Card(s) Reported Lost/Stolen";
//                    break;
//                case "U":
//                    StrOut = "Contact Following Entity for Eligibility or Benefit Information";
//                    break;
//                case "V":
//                    StrOut = "Cannot Process";
//                    break;
//                case "W":
//                    StrOut = "Other Source of Data";
//                    break;
//                case "X":
//                    StrOut = "Health Care Facility";
//                    break;
//                case "Y":
//                    StrOut = "Spend Down";

//                    break;
//                default:
//                    StrOut = "### Unknown EB01 ###";
//                    break;
//            }

//            this.rowBenefits.EB01 = StrOut;
//            return StrOut;

//        }

//        private string EB02(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "CHD":
//                    StrOut = "Children Only";
//                    break;
//                case "DEP":
//                    StrOut = "Dependents Only";
//                    break;
//                case "ECH":
//                    StrOut = "Employee and Children";
//                    break;
//                case "EMP":
//                    StrOut = "Employee Only";
//                    break;
//                case "ESP":
//                    StrOut = "Employee and Spouse";
//                    break;
//                case "FAM":
//                    StrOut = "Family";
//                    break;
//                case "IND":
//                    StrOut = "Individual";
//                    break;
//                case "SPC":
//                    StrOut = "Spouse and Children";
//                    break;
//                case "SPO":
//                    StrOut = "Spouse Only";
//                    break;
//                default:
//                    StrOut = "### Unknown EB02 ###";
//                    break;
//            }

//            this.rowBenefits.EB02 = StrOut;
//            return StrOut;

//        }

//        private string EB03(string Code)
//        {
//            string StrOut = "";
//            string[] codes = Code.Split(D_S_S_ELMT);
//            foreach (var item in codes)
//            {
//                switch (item)
//                {
//                    case "1":
//                        StrOut += "Medical Care, ";
//                        break;
//                    case "2":
//                        StrOut += "Surgical, ";
//                        break;
//                    case "3":
//                        StrOut += "Consultation, ";
//                        break;
//                    case "4":
//                        StrOut += "Diagnostic X-Ray, ";
//                        break;
//                    case "5":
//                        StrOut += "Diagnostic Lab, ";
//                        break;
//                    case "6":
//                        StrOut += "Radiation Therapy, ";
//                        break;
//                    case "7":
//                        StrOut += "Anesthesia, ";
//                        break;
//                    case "8":
//                        StrOut += "Surgical Assistance, ";
//                        break;
//                    case "9":
//                        StrOut += "Other Medical, ";
//                        break;
//                    case "10":
//                        StrOut += "Blood Charges, ";
//                        break;
//                    case "11":
//                        StrOut += "Used Durable Medical Equipment, ";
//                        break;
//                    case "12":
//                        StrOut += "Durable Medical Equipment Purchase, ";
//                        break;
//                    case "13":
//                        StrOut += "Ambulatory Service Center Facility, ";
//                        break;
//                    case "14":
//                        StrOut += "Renal Supplies in the Home, ";
//                        break;
//                    case "15":
//                        StrOut += "Alternate Method Dialysis, ";
//                        break;
//                    case "16":
//                        StrOut += "Chronic Renal Disease (CRD) Equipment, ";
//                        break;
//                    case "17":
//                        StrOut += "Pre-Admission Testing, ";
//                        break;
//                    case "18":
//                        StrOut += "Durable Medical Equipment Rental, ";
//                        break;
//                    case "19":
//                        StrOut += "Pneumonia Vaccine, ";
//                        break;
//                    case "20":
//                        StrOut += "Second Surgical Opinion, ";
//                        break;
//                    case "21":
//                        StrOut += "Third Surgical Opinion, ";
//                        break;
//                    case "22":
//                        StrOut += "Social Work, ";
//                        break;
//                    case "23":
//                        StrOut += "Diagnostic Dental, ";
//                        break;
//                    case "24":
//                        StrOut += "Periodontics, ";
//                        break;
//                    case "25":
//                        StrOut += "Restorative, ";
//                        break;
//                    case "26":
//                        StrOut += "Endodontics, ";
//                        break;
//                    case "27":
//                        StrOut += "Maxillofacial Prosthetics, ";
//                        break;
//                    case "28":
//                        StrOut += "Adjunctive Dental Services, ";
//                        break;
//                    case "30":
//                        StrOut += "Health Benefit Plan Coverage, ";
//                        break;
//                    case "32":
//                        StrOut += "Plan Waiting Period, ";
//                        break;
//                    case "33":
//                        StrOut += "Chiropractic, ";
//                        break;
//                    case "34":
//                        StrOut += "Chiropractic Office Visits, ";
//                        break;
//                    case "35":
//                        StrOut += "Dental Care, ";
//                        break;
//                    case "36":
//                        StrOut += "Dental Crowns, ";
//                        break;
//                    case "37":
//                        StrOut += "Dental Accident, ";
//                        break;
//                    case "38":
//                        StrOut += "Orthodontics, ";
//                        break;
//                    case "39":
//                        StrOut += "Prosthodontics, ";
//                        break;
//                    case "40":
//                        StrOut += "Oral Surgery, ";
//                        break;
//                    case "41":
//                        StrOut += "Routine (Preventive) Dental, ";
//                        break;
//                    case "42":
//                        StrOut += "Home Health Care, ";
//                        break;
//                    case "43":
//                        StrOut += "Home Health Prescriptions, ";
//                        break;
//                    case "44":
//                        StrOut += "Home Health Visits, ";
//                        break;
//                    case "45":
//                        StrOut += "Hospice, ";
//                        break;
//                    case "46":
//                        StrOut += "Respite Care, ";
//                        break;
//                    case "47":
//                        StrOut += "Hospital, ";
//                        break;
//                    case "48":
//                        StrOut += "Hospital - Inpatient, ";
//                        break;
//                    case "49":
//                        StrOut += "Hospital - Room and Board, ";
//                        break;
//                    case "50":
//                        StrOut += "Hospital - Outpatient, ";
//                        break;
//                    case "51":
//                        StrOut += "Hospital - Emergency Accident, ";
//                        break;
//                    case "52":
//                        StrOut += "Hospital - Emergency Medical, ";
//                        break;
//                    case "53":
//                        StrOut += "Hospital - Ambulatory Surgical, ";
//                        break;
//                    case "54":
//                        StrOut += "Long Term Care, ";
//                        break;
//                    case "55":
//                        StrOut += "Major Medical, ";
//                        break;
//                    case "56":
//                        StrOut += "Medically Related Transportation, ";
//                        break;
//                    case "57":
//                        StrOut += "Air Transportation, ";
//                        break;
//                    case "58":
//                        StrOut += "Cabulance, ";
//                        break;
//                    case "59":
//                        StrOut += "Licensed Ambulance, ";
//                        break;
//                    case "60":
//                        StrOut += "General Benefits, ";
//                        break;
//                    case "61":
//                        StrOut += "In-vitro Fertilization, ";
//                        break;
//                    case "62":
//                        StrOut += "MRI/CAT Scan, ";
//                        break;
//                    case "63":
//                        StrOut += "Donor Procedures, ";
//                        break;
//                    case "64":
//                        StrOut += "Acupuncture, ";
//                        break;
//                    case "65":
//                        StrOut += "Newborn Care, ";
//                        break;
//                    case "66":
//                        StrOut += "Pathology, ";
//                        break;
//                    case "67":
//                        StrOut += "Smoking Cessation, ";
//                        break;
//                    case "68":
//                        StrOut += "Well Baby Care, ";
//                        break;
//                    case "69":
//                        StrOut += "Maternity, ";
//                        break;
//                    case "70":
//                        StrOut += "Transplants, ";
//                        break;
//                    case "71":
//                        StrOut += "Audiology Exam, ";
//                        break;
//                    case "72":
//                        StrOut += "Inhalation Therapy, ";
//                        break;
//                    case "73":
//                        StrOut += "Diagnostic Medical, ";
//                        break;
//                    case "74":
//                        StrOut += "Private Duty Nursing, ";
//                        break;
//                    case "75":
//                        StrOut += "Prosthetic Device, ";
//                        break;
//                    case "76":
//                        StrOut += "Dialysis, ";
//                        break;
//                    case "77":
//                        StrOut += "Otological Exam, ";
//                        break;
//                    case "78":
//                        StrOut += "Chemotherapy, ";
//                        break;
//                    case "79":
//                        StrOut += "Allergy Testing, ";
//                        break;
//                    case "80":
//                        StrOut += "Immunizations, ";
//                        break;
//                    case "81":
//                        StrOut += "Routine Physical, ";
//                        break;
//                    case "82":
//                        StrOut += "Family Planning, ";
//                        break;
//                    case "83":
//                        StrOut += "Infertility, ";
//                        break;
//                    case "84":
//                        StrOut += "Abortion, ";
//                        break;
//                    case "85":
//                        StrOut += "AIDS, ";
//                        break;
//                    case "86":
//                        StrOut += "Emergency Services, ";
//                        break;
//                    case "87":
//                        StrOut += "Cancer, ";
//                        break;
//                    case "88":
//                        StrOut += "Pharmacy, ";
//                        break;
//                    case "89":
//                        StrOut += "Free Standing Prescription Drug, ";
//                        break;
//                    case "90":
//                        StrOut += "Mail Order Prescription Drug, ";
//                        break;
//                    case "91":
//                        StrOut += "Brand Name Prescription Drug, ";
//                        break;
//                    case "92":
//                        StrOut += "Generic Prescription Drug, ";
//                        break;
//                    case "93":
//                        StrOut += "Podiatry, ";
//                        break;
//                    case "94":
//                        StrOut += "Podiatry - Office Visits, ";
//                        break;
//                    case "95":
//                        StrOut += "Podiatry - Nursing Home Visits, ";
//                        break;
//                    case "96":
//                        StrOut += "Professional (Physician), ";
//                        break;
//                    case "97":
//                        StrOut += "Anesthesiologist, ";
//                        break;
//                    case "98":
//                        StrOut += "Professional (Physician) Visit - Office, ";
//                        break;
//                    case "99":
//                        StrOut += "Professional (Physician) Visit - Inpatient, ";
//                        break;
//                    case "A0":
//                        StrOut += "Professional (Physician) Visit - Outpatient, ";
//                        break;
//                    case "A1":
//                        StrOut += "Professional (Physician) Visit - Nursing Home, ";
//                        break;
//                    case "A2":
//                        StrOut += "Professional (Physician) Visit - Skilled Nursing Facility, ";
//                        break;
//                    case "A3":
//                        StrOut += "Professional (Physician) Visit - Home, ";
//                        break;
//                    case "A4":
//                        StrOut += "Psychiatric, ";
//                        break;
//                    case "A5":
//                        StrOut += "Psychiatric - Room and Board, ";
//                        break;
//                    case "A6":
//                        StrOut += "Psychotherapy, ";
//                        break;
//                    case "A7":
//                        StrOut += "Psychiatric - Inpatient, ";
//                        break;
//                    case "A8":
//                        StrOut += "Psychiatric - Outpatient, ";
//                        break;
//                    case "A9":
//                        StrOut += "Rehabilitation, ";
//                        break;
//                    case "AA":
//                        StrOut += "Rehabilitation - Room and Board, ";
//                        break;
//                    case "AB":
//                        StrOut += "Rehabilitation - Inpatient, ";
//                        break;
//                    case "AC":
//                        StrOut += "Rehabilitation - Outpatient, ";
//                        break;
//                    case "AD":
//                        StrOut += "Occupational Therapy, ";
//                        break;
//                    case "AE":
//                        StrOut += "Physical Medicine, ";
//                        break;
//                    case "AF":
//                        StrOut += "Speech Therapy, ";
//                        break;
//                    case "AG":
//                        StrOut += "Skilled Nursing Care, ";
//                        break;
//                    case "AH":
//                        StrOut += "Skilled Nursing Care - Room and Board, ";
//                        break;
//                    case "AI":
//                        StrOut += "Substance Abuse, ";
//                        break;
//                    case "AJ":
//                        StrOut += "Alcoholism, ";
//                        break;
//                    case "AK":
//                        StrOut += "Drug Addiction, ";
//                        break;
//                    case "AL":
//                        StrOut += "Vision (Optometry), ";
//                        break;
//                    case "AM":
//                        StrOut += "Frames, ";
//                        break;
//                    case "AN":
//                        StrOut += "Routine Exam, ";
//                        break;
//                    case "AO":
//                        StrOut += "Lenses, ";
//                        break;
//                    case "AQ":
//                        StrOut += "Nonmedically Necessary Physical, ";
//                        break;
//                    case "AR":
//                        StrOut += "Experimental Drug Therapy, ";
//                        break;
//                    case "B1":
//                        StrOut += "Burn Care, ";
//                        break;
//                    case "B2":
//                        StrOut += "Brand Name Prescription Drug - Formulary, ";
//                        break;
//                    case "B3":
//                        StrOut += "Brand Name Prescription Drug - Non-Formulary, ";
//                        break;
//                    case "BA":
//                        StrOut += "Independent Medical Evaluation, ";
//                        break;
//                    case "BB":
//                        StrOut += "Partial Hospitalization (Psychiatric), ";
//                        break;
//                    case "BC":
//                        StrOut += "Day Care (Psychiatric), ";
//                        break;
//                    case "BD":
//                        StrOut += "Cognitive Therapy, ";
//                        break;
//                    case "BE":
//                        StrOut += "Massage Therapy, ";
//                        break;
//                    case "BF":
//                        StrOut += "Pulmonary Rehabilitation, ";
//                        break;
//                    case "BG":
//                        StrOut += "Cardiac Rehabilitation, ";
//                        break;
//                    case "BH":
//                        StrOut += "Pediatric, ";
//                        break;
//                    case "BI":
//                        StrOut += "Nursery, ";
//                        break;
//                    case "BJ":
//                        StrOut += "Skin, ";
//                        break;
//                    case "BK":
//                        StrOut += "Orthopedic, ";
//                        break;
//                    case "BL":
//                        StrOut += "Cardiac, ";
//                        break;
//                    case "BM":
//                        StrOut += "Lymphatic, ";
//                        break;
//                    case "BN":
//                        StrOut += "Gastrointestinal, ";
//                        break;
//                    case "BP":
//                        StrOut += "Endocrine, ";
//                        break;
//                    case "BQ":
//                        StrOut += "Neurology, ";
//                        break;
//                    case "BR":
//                        StrOut += "Eye, ";
//                        break;
//                    case "BS":
//                        StrOut += "Invasive Procedures, ";
//                        break;
//                    case "BT":
//                        StrOut += "Gynecological, "; break;
//                    case "BU":
//                        StrOut += "Obstetrical, "; break;
//                    case "BV":
//                        StrOut += "Obstetrical/Gynecological, "; break;
//                    case "BW":
//                        StrOut += "Mail Order Prescription Drug: Brand Name, "; break;
//                    case "BX":
//                        StrOut += "Mail Order Prescription Drug: Generic, "; break;
//                    case "BY":
//                        StrOut += "Physician Visit - Office: Sick, "; break;
//                    case "BZ":
//                        StrOut += "Physician Visit - Office: Well, "; break;
//                    case "C1":
//                        StrOut += "Coronary Care, "; break;
//                    case "CA":
//                        StrOut += "Private Duty Nursing - Inpatient, "; break;
//                    case "CB":
//                        StrOut += "Private Duty Nursing - Home, "; break;
//                    case "CC":
//                        StrOut += "Surgical Benefits - Professional (Physician), "; break;
//                    case "CD":
//                        StrOut += "Surgical Benefits - Facility, "; break;
//                    case "CE":
//                        StrOut += "Mental Health Provider - Inpatient, "; break;
//                    case "CF":
//                        StrOut += "Mental Health Provider - Outpatient, "; break;
//                    case "CG":
//                        StrOut += "Mental Health Facility - Inpatient, "; break;
//                    case "CH":
//                        StrOut += "Mental Health Facility - Outpatient, "; break;
//                    case "CI":
//                        StrOut += "Substance Abuse Facility - Inpatient, "; break;
//                    case "CJ":
//                        StrOut += "Substance Abuse Facility - Outpatient, "; break;
//                    case "CK":
//                        StrOut += "Screening X-ray, "; break;
//                    case "CL":
//                        StrOut += "Screening laboratory, "; break;
//                    case "CM":
//                        StrOut += "Mammogram, High Risk Patient, "; break;
//                    case "CN":
//                        StrOut += "Mammogram, Low Risk Patient, "; break;
//                    case "CO":
//                        StrOut += "Flu Vaccination, "; break;
//                    case "CP":
//                        StrOut += "Eyewear and Eyewear Accessories, "; break;
//                    case "CQ":
//                        StrOut += "Case Management,"; break;
//                    case "DG":
//                        StrOut += "Dermatology,"; break;
//                    case "DM":
//                        StrOut += "Durable Medical Equipment,"; break;
//                    case "DS":
//                        StrOut += "Diabetic Supplies,"; break;
//                    case "GF":
//                        StrOut += "Generic Prescription Drug - Formulary,"; break;
//                    case "GN":
//                        StrOut += "Generic Prescription Drug - Non-Formulary,"; break;
//                    case "GY":
//                        StrOut += "Allergy,"; break;
//                    case "IC":
//                        StrOut += "Intensive Care,"; break;
//                    case "MH":
//                        StrOut += "Mental Health,"; break;
//                    case "NI":
//                        StrOut += "Neonatal Intensive Care,"; break;
//                    case "ON":
//                        StrOut += "Oncology,"; break;
//                    case "PT":
//                        StrOut += "Physical Therapy,"; break;
//                    case "PU":
//                        StrOut += "Pulmonary,"; break;
//                    case "RN":
//                        StrOut += "Renal,"; break;
//                    case "RT":
//                        StrOut += "Residential Psychiatric Treatment,"; break;
//                    case "TC":
//                        StrOut += "Transitional Care,"; break;
//                    case "TN":
//                        StrOut += "Transitional Nursery Care,"; break;
//                    case "UC":
//                        StrOut += "Urgent Care,"; break;
//                    default:
//                        StrOut += "### Unknown EB03 ###,";
//                        break;
//                }
//            }



//            StrOut = StrOut.Remove(StrOut.LastIndexOf(','), 1);

//            if (this.rowBenefits != null)
//            {
//                if (!string.IsNullOrEmpty(ServiceTypeCode) && (EB01_Value == "1" && codes.Contains(ServiceTypeCode)))
//                    this.rowHeader.IsEligible = true;

//                if (!string.IsNullOrEmpty(ServiceTypeCode) && (codes.Contains(ServiceTypeCode)))
//                    this.EB03_Equal = true;
//                else
//                    this.EB03_Equal = false;

//                this.rowBenefits.EB03 = StrOut;
//                this.rowBenefits.ServiceTypeCode = Code;
//            }


//            return StrOut;

//        }

//        private string EB04(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "12":
//                    StrOut = "Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan";
//                    break;
//                case "13":
//                    StrOut = "Medicare Secondary End-Stage Renal Disease Beneficiary in the 12 month coordination period with an employer's group health plan";
//                    break;
//                case "14":
//                    StrOut = "Medicare Secondary, No-fault Insurance including Auto is Primary";
//                    break;
//                case "15":
//                    StrOut = "Medicare Secondary Worker’s Compensation";
//                    break;
//                case "16":
//                    StrOut = "Medicare Secondary Private Health Service (PHS)or Other Federal Agency";
//                    break;
//                case "41":
//                    StrOut = "Medicare Secondary Black Lung";
//                    break;
//                case "42":
//                    StrOut = "Medicare Secondary Veteran’s Administration";
//                    break;
//                case "43":
//                    StrOut = "Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)";
//                    break;
//                case "47":
//                    StrOut = "Medicare Secondary, Other Liability Insurance is Primary";
//                    break;
//                case "AP":
//                    StrOut = "Auto Insurance Policy";
//                    break;
//                case "C1":
//                    StrOut = "Commercial";
//                    break;
//                case "CO":
//                    StrOut = "Consolidated Omnibus Budget Reconciliation Act (COBRA)";
//                    break;
//                case "CP":
//                    StrOut = "Medicare Conditionally Primary";
//                    break;
//                case "D":
//                    StrOut = "Disability";
//                    break;
//                case "DB":
//                    StrOut = "Disability Benefits";
//                    break;
//                case "EP":
//                    StrOut = "Exclusive Provider Organization";
//                    break;
//                case "FF":
//                    StrOut = "Family or Friends";
//                    break;
//                case "GP":
//                    StrOut = "Group Policy";
//                    break;
//                case "HM":
//                    StrOut = "Health Maintenance Organization (HMO)";
//                    break;
//                case "HN":
//                    StrOut = "Health Maintenance Organization (HMO) - Medicare Risk";
//                    break;
//                case "HS":
//                    StrOut = "Special Low Income Medicare Beneficiary";
//                    break;
//                case "IN":
//                    StrOut = "Indemnity";
//                    break;
//                case "IP":
//                    StrOut = "Individual Policy";
//                    break;
//                case "LC":
//                    StrOut = "Long Term Care";
//                    break;
//                case "LD":
//                    StrOut = "Long Term Policy";
//                    break;
//                case "LI":
//                    StrOut = "Life Insurance";
//                    break;
//                case "LT":
//                    StrOut = "Litigation";
//                    break;
//                case "MA":
//                    StrOut = "Medicare Part A";
//                    break;
//                case "MB":
//                    StrOut = "Medicare Part B";
//                    break;
//                case "MC":
//                    StrOut = "Medicaid";
//                    break;
//                case "MH":
//                    StrOut = "Medigap Part A";
//                    break;
//                case "MI":
//                    StrOut = "Medigap Part B";
//                    break;
//                case "MP":
//                    StrOut = "Medicare Primary";
//                    break;
//                case "OT":
//                    StrOut = "Other";
//                    break;
//                case "PE":
//                    StrOut = "Property Insurance - Personal";
//                    break;
//                case "PL":
//                    StrOut = "Personal";
//                    break;
//                case "PP":
//                    StrOut = "Personal Payment (Cash - No Insurance)";
//                    break;
//                case "PR":
//                    StrOut = "Preferred Provider Organization (PPO)";
//                    break;
//                case "PS":
//                    StrOut = "Point of Service (POS)";
//                    break;
//                case "QM":
//                    StrOut = "Qualified Medicare Beneficiary";
//                    break;
//                case "RP":
//                    StrOut = "Property Insurance - Real";
//                    break;
//                case "SP":
//                    StrOut = "Supplemental Policy";
//                    break;
//                case "TF":
//                    StrOut = "Tax Equity Fiscal Responsibility Act (TEFRA)";
//                    break;
//                case "WC":
//                    StrOut = "Workers Compensation";
//                    break;
//                case "WU":
//                    StrOut = "Wrap Up Policy";
//                    break;
//                default:
//                    StrOut = "### Unknown EB04 ###";
//                    break;
//            }

//            this.rowBenefits.EB04 = StrOut;
//            return StrOut;

//        }

//        private string EB06(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "6":
//                    StrOut = "Hour";
//                    break;
//                case "7":
//                    StrOut = "Day";
//                    break;
//                //case "24":
//                //    StrOut = "Hours";
//                //    break;
//                case "21":
//                    StrOut = "Years";
//                    break;
//                case "22":
//                    StrOut = "Service Year";
//                    break;
//                case "23":
//                    StrOut = "Calendar Year";
//                    break;
//                case "24":
//                    StrOut = "Year to Date";
//                    break;
//                case "25":
//                    StrOut = "Contract";
//                    break;
//                case "26":
//                    StrOut = "Episode";
//                    break;
//                case "27":
//                    StrOut = "Visit";
//                    break;
//                case "28":
//                    StrOut = "Outlier";
//                    break;
//                case "29":
//                    StrOut = "Remaining";
//                    break;
//                case "30":
//                    StrOut = "Exceeded";
//                    break;
//                case "31":
//                    StrOut = "Not Exceeded";
//                    break;
//                case "32":
//                    StrOut = "Lifetime";
//                    break;
//                case "33":
//                    StrOut = "Lifetime Remaining";
//                    break;
//                case "34":
//                    StrOut = "Month";
//                    break;
//                case "35":
//                    StrOut = "Week";
//                    break;
//                case "36":
//                    StrOut = "Admisson";
//                    break;
//                case "13":
//                    StrOut = "24 Hours";

//                    break;
//                default:
//                    StrOut = "### Unknown EB06 ###";
//                    break;
//            }

//            this.rowBenefits.EB06 = StrOut;
//            return StrOut;

//        }

//        private string EB09(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "99":
//                    StrOut = "Quantity Used";
//                    break;
//                case "CA":
//                    StrOut = "Covered - Actual";
//                    break;
//                case "CE":
//                    StrOut = "Covered - Estimated";
//                    break;
//                case "DB":
//                    StrOut = "Deductible Blood Units";
//                    break;
//                case "DY":
//                    StrOut = "Days";
//                    break;
//                case "HS":
//                    StrOut = "Hours";
//                    break;
//                case "LA":
//                    StrOut = "Life-time Reserve - Actual";
//                    break;
//                case "LE":
//                    StrOut = "Life-time Reserve - Estimated";
//                    break;
//                case "MN":
//                    StrOut = "Month";
//                    break;
//                case "P6":
//                    StrOut = "Number of Services or Procedures";
//                    break;
//                case "QA":
//                    StrOut = "Quantity Approved";
//                    break;
//                case "S7":
//                    StrOut = "Age, High Value";
//                    break;
//                case "S8":
//                    StrOut = "Age, Low Value";
//                    break;
//                case "VS":
//                    StrOut = "Visits";
//                    break;
//                case "YY":
//                    StrOut = "Years";

//                    break;
//                default:
//                    StrOut = "### Unknown EB09 ###";
//                    break;
//            }

//            this.rowBenefits.EB09 = StrOut;
//            return StrOut;

//        }

//        private string EB11(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "N":
//                    StrOut = "Authorization or certification is not required per plan provisions";
//                    break;
//                case "U":
//                    StrOut = "It is unknown whether the plan provisions require an authorization or certification";
//                    break;
//                case "Y":
//                    StrOut = "Authorization or certification is required per plan provisions";

//                    break;
//                default:
//                    StrOut = "### Unknown EB11 ###";
//                    break;
//            }

//            this.rowBenefits.EB11 = StrOut;
//            return StrOut;

//        }

//        private string EB12(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "N":
//                    StrOut = "No";
//                    break;
//                case "U":
//                    StrOut = "Unknown";
//                    break;
//                case "Y":
//                    StrOut = "Yes";
//                    break;
//                case "W":
//                    StrOut = "Not Applicable";
//                    break;
//                default:
//                    StrOut = "### Unknown EB12 ###";
//                    break;
//            }

//            this.rowBenefits.EB12 = StrOut;
//            return StrOut;

//        }

//        private string EB13_1(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "AD":
//                    StrOut = "American Dental Association Codes";
//                    break;
//                case "CJ":
//                    StrOut = "Current Procedural Terminology (CPT) Codes";
//                    break;
//                case "HC":
//                    StrOut = "Health Care Financing Administration Common Procedural Coding System (HCPCS) Codes";
//                    break;
//                case "ID":
//                    StrOut = "International Classification of Diseases Clinical Modification (ICD-9-CM) - Procedure";
//                    break;
//                case "ND":
//                    StrOut = "National Drug Code (NDC)";
//                    break;
//                case "ZZ":
//                    StrOut = "Mutually Defined";
//                    break;
//                case "IV":
//                    StrOut = "Home Infusion EDI Coalition (HIEC) Product/Service Code";
//                    break;
//                case "N4":
//                    StrOut = "National Drug Code in 5-4-2 Format";

//                    break;
//                default:
//                    StrOut = "### Unknown EB13 - 1 ###";
//                    break;
//            }

//            this.rowBenefits.EB13_1 = StrOut;
//            return StrOut;

//        }

//        private string INS01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "Y":
//                    StrOut = "Insured is a subscriber";
//                    break;
//                case "N":
//                    StrOut = "Insured is a dependent";

//                    break;
//                default:
//                    StrOut = "### Unknown INS01 ###";
//                    break;
//            }
//            return StrOut;

//        }

//        private string INS02(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "18":
//                    StrOut = "Self";
//                    break;
//                case "01":
//                    StrOut = "Spouse";
//                    break;
//                case "19":
//                    StrOut = "Child";
//                    break;
//                case "21":
//                    StrOut = "Unknown";
//                    break;
//                case "34":
//                    StrOut = "Other Adult";

//                    break;
//                default:
//                    StrOut = "### Unknown INS02 ###";
//                    break;
//            }

//            this.rowName.INS02 = StrOut;
//            return StrOut;

//        }

//        private string INS03(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "001":
//                    StrOut = "Change";

//                    break;
//                default:
//                    StrOut = "### Unknown INS03 ###";
//                    break;
//            }
//            this.rowName.INS03 = StrOut;
//            return StrOut;

//        }

//        private string INS04(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "25":
//                    StrOut = "Change in Indentifying Data Elements";

//                    break;
//                default:
//                    StrOut = "### Unknown INS04 ###";
//                    break;
//            }

//            this.rowName.INS04 = StrOut;
//            return StrOut;

//        }

//        private string INS09(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "F":
//                    StrOut = "Full-time";
//                    break;
//                case "N":
//                    StrOut = "Not a Student";
//                    break;
//                case "P":
//                    StrOut = "Part-time";

//                    break;
//                default:
//                    StrOut = "### Unknown INS09 ###";
//                    break;
//            }

//            this.rowName.INS09 = StrOut;
//            return StrOut;

//        }

//        private string INS10(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "N":
//                    StrOut = "Individual is not handicapped";
//                    break;
//                case "Y":
//                    StrOut = "Individual is handicapped";

//                    break;
//                default:
//                    StrOut = "### Unknown INS10 ###";
//                    break;
//            }
//            return StrOut;

//        }

//        private string DMG03(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "F":
//                    StrOut = "Female";
//                    break;
//                case "M":
//                    StrOut = "Male";
//                    break;
//                case "U":
//                    StrOut = "Unknown";

//                    break;
//                default:
//                    StrOut = "### Unknown DMG03 ###";
//                    break;
//            }

//            this.rowName.DMG03 = StrOut;
//            return StrOut;

//        }

//        private string AAA01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "N":
//                    StrOut = "code is invalid";
//                    break;
//                case "Y":
//                    StrOut = "code is valid";
//                    break;
//                default:
//                    StrOut = "### Unknown AAA01 ###";
//                    break;
//            }
//            return StrOut;

//        }

//        private string AAA03(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "04":
//                    StrOut = "04- Authorized Quantity Exceeded";
//                    break;
//                case "41":
//                    StrOut = "41- Authorization/Access Restrictions";
//                    StrOut = StrOut + Environment.NewLine + "Use this code to indicate that the entity identified in GS02 is not authorized";
//                    StrOut = StrOut + "to submit 270 transactions to the entity identified in either ISA08 or GS03. This is";
//                    StrOut = StrOut + "not to be used to indicate Authorization/Access Restrictions as related to the Information Source";
//                    StrOut = StrOut + "Identified in Loop 2100A.";

//                    break;
//                case "42":
//                    StrOut = "42- Unable to Respond at Current Time";
//                    break;
//                case "15":
//                    StrOut = "Required application data missing";
//                    break;
//                case "43":
//                    StrOut = "Invalid/Missing Provider Identification";
//                    break;
//                case "44":
//                    StrOut = "Invalid/Missing Provider Name";
//                    break;
//                case "45":
//                    StrOut = "Invalid/Missing Provider Specialty";
//                    break;
//                case "47":
//                    StrOut = "Invalid/Missing Provider State";
//                    break;
//                case "48":
//                    StrOut = "Invalid/Missing Referring Provider Identification Number";
//                    break;
//                case "50":
//                    StrOut = "Provider Ineligible for Inquiries";
//                    break;
//                case "51":
//                    StrOut = "Provider Not on File";
//                    break;
//                case "46":
//                    StrOut = "Invalid/Missing Provider Phone Number";
//                    break;
//                case "79":
//                    StrOut = "Invalid Participant Identification";
//                    break;
//                case "80":
//                    StrOut = "No Response received - Transaction Terminated";
//                    break;
//                case "97":
//                    StrOut = "Invalid or Missing Provider Address";
//                    break;
//                case "T4":
//                    StrOut = "Payer Name or Identifier Missing";
//                    break;
//                case "49":
//                    StrOut = "Provider is Not Primary Care Physician";
//                    break;
//                case "52":
//                    StrOut = "Service Dates Not Within Provider Plan Enrollment";
//                    break;
//                case "56":
//                    StrOut = "Inappropriate Date";
//                    break;
//                case "57":
//                    StrOut = "Invalid/Missing Date(s) of Service";
//                    break;
//                case "58":
//                    StrOut = "Invalid/Missing Date-of-Birth";
//                    break;
//                case "60":
//                    StrOut = "Date of Birth Follows Date(s) of Service";
//                    break;
//                case "61":
//                    StrOut = "Date of Death Precedes Date(s) of Service";
//                    break;
//                case "62":
//                    StrOut = "Date of Service Not Within Allowable Inquiry Period";
//                    break;
//                case "63":
//                    StrOut = "Date of Service in Future";
//                    break;
//                case "64":
//                    StrOut = "Invalid/Missing Patient ID";
//                    break;
//                case "65":
//                    StrOut = "Invalid/Missing Patient Name";
//                    break;
//                case "66":
//                    StrOut = "Invalid/Missing Patient Gender Code";
//                    break;
//                case "67":
//                    StrOut = "Patient Not Found";
//                    break;
//                case "68":
//                    StrOut = "Duplicate Patient ID Number";
//                    break;
//                case "71":
//                    StrOut = "Patient Birth Date Does Not Match That for the Patient on the Database";
//                    break;
//                case "72":
//                    StrOut = "Invalid/Missing Subscriber/Insured ID";
//                    break;
//                case "73":
//                    StrOut = "Invalid/Missing Subscriber/Insured Name";
//                    break;
//                case "74":
//                    StrOut = "Invalid/Missing Subscriber/Insured Gender Code";
//                    break;
//                case "75":
//                    StrOut = "Subscriber/Insured Not Found";
//                    break;
//                case "76":
//                    StrOut = "Duplicate Subscriber/Insured ID Number";
//                    break;
//                case "77":
//                    StrOut = "Subscriber Found, Patient Not Found";
//                    break;
//                case "78":
//                    StrOut = "Subscriber/Insured Not in Group/Plan Identified";
//                    break;
//                case "53":
//                    StrOut = "Inquired Benefit Inconsistent with Provider Type";
//                    break;
//                case "54":
//                    StrOut = "Inappropriate Product/Service ID Qualifier";
//                    break;
//                case "55":
//                    StrOut = "Inappropriate Product/Service ID";
//                    break;
//                case "69":
//                    StrOut = "Inconsistent with Patient’s Age";
//                    break;
//                case "70":
//                    StrOut = "Inconsistent with Patient’s Gender";
//                    break;
//                case "35":
//                    StrOut = "Out of Network";
//                    break;
//                case "33":
//                    StrOut = "Input Errors";
//                    break;
//                case "AA":
//                    StrOut = "Authorization Number Not Found";
//                    break;
//                case "AE":
//                    StrOut = "Requires Primary Care Physician Authorization";
//                    break;
//                case "AF":
//                    StrOut = "Invalid/Missing Diagnosis Code(s)";
//                    break;
//                case "AG":
//                    StrOut = "Invalid/Missing Procedure Code(s)";
//                    break;
//                case "AO":
//                    StrOut = "Additional Patient Condition Information Required";
//                    break;
//                case "CI":
//                    StrOut = "Certification Information Does Not Match Patient";
//                    break;
//                case "E8":
//                    StrOut = "Requires Medical Review";
//                    break;
//                case "IA":
//                    StrOut = "Invalid Authorization Number Format";
//                    break;
//                case "MA":
//                    StrOut = "Missing Authorization Number";
//                    break;
//                default:
//                    StrOut = "### Unknown AAA03 ###";
//                    break;
//            }

//            this.rowName.AAA03 = StrOut;
//            return StrOut;

//        }

//        private string AAA04(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "C":
//                    StrOut = "Please Correct and Resubmit";
//                    break;
//                case "N":
//                    StrOut = "Resubmission Not Allowed";
//                    break;
//                case "P":
//                    StrOut = "Please Resubmit Original Transaction";
//                    break;
//                case "R":
//                    StrOut = "Resubmission Allowed";
//                    break;
//                case "S":
//                    StrOut = "Do Not Resubmit; Inquiry Initiated to a Third Party";
//                    break;
//                case "W":
//                    StrOut = "Please Wait 30 Days and Resubmit";
//                    break;
//                case "X":
//                    StrOut = "Please Wait 10 Days and Resubmit";
//                    break;
//                case "Y":
//                    StrOut = "Do Not Resubmit; We Will Hold Your Request and Respond Again Shortly";
//                    break;
//                default:
//                    StrOut = "### Unknown AAA04 ###";
//                    break;
//            }

//            this.rowName.AAA04 = StrOut;
//            return StrOut;

//        }

//        private string BHT01(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "0022":
//                    StrOut = "Information Source, Information Receiver,Subscriber, Dependent";
//                    break;
//                default:
//                    StrOut = "### Unknown BHT01 ###";
//                    break;
//            }
//            return StrOut;
//        }

//        private string BHT02(string Code)
//        {

//            string StrOut = "Unknown";
//            switch (Code)
//            {
//                case "11":
//                    StrOut = "Response";
//                    break;
//                default:
//                    StrOut = "### Unknown BHT02 ###";
//                    break;
//            }
//            return StrOut;

//        }

//        #endregion

//        #region Supporting functions
//        public string GetHumanReadable271(string EDI271, ref DS271 ds, string ServiceTypeCode)
//        {
//            try
//            {
//                this.ds271Parser = ds;
//                this.ServiceTypeCode = ServiceTypeCode;
//                this.rowHeader = this.ds271Parser.EDI271Header.NewEDI271HeaderRow();
//                rowHeader.IsEligible = false;
//                this.ds271Parser.EDI271Header.Rows.Add(rowHeader);

//                FinalStr271 = new System.Text.StringBuilder();
//                Parse271(EDI271);

//                //Set Header Values
//                this.rowHeader.Readable271 = FinalStr271.ToString();
//                this.rowHeader.Copay = Copay;
//                this.rowHeader.Deductible = Deductible;
//                this.rowHeader.ServiceTypeCode = ServiceTypeCode;
//                this.rowHeader.ServiceTypeName = EB03(ServiceTypeCode);


//                return FinalStr271.ToString();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//        }

//        private void Parse271(string StrIn)
//        {
//            try
//            {
//                string[] Segments = StrIn.Split((new char[] { EDIUtility.D_SGMT }));

//                for (int i = 0; i < Segments.Length - 2; i++)
//                {
//                    string match = Segments[i].Substring(0, Segments[i].IndexOf(EDIUtility.D_ELMT));
//                    switch (match)
//                    {
//                        case "ISA":
//                            {
//                                FinalStr271.Append("ISA ##########################################################################");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "IEA":
//                            {
//                                FinalStr271.Append("IEA ##########################################################################");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "GS":
//                            {
//                                FinalStr271.Append("GS  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "GE":
//                            {
//                                FinalStr271.Append("GE  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "ST":
//                            {
//                                FinalStr271.Append("ST  **************************************************************************");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "SE":
//                            {
//                                FinalStr271.Append("SE  **************************************************************************");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "BHT":
//                            {
//                                FinalStr271.Append(BHT(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "AAA":
//                            {
//                                FinalStr271.Append(AAA(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "DMG":
//                            {
//                                FinalStr271.Append(DMG(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "INS":
//                            {
//                                FinalStr271.Append(INS(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "EB":
//                            {
//                                FinalStr271.Append(EB(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "HSD":
//                            {
//                                this.rowBenefitsDetail = GetNew271BenefitDetailRow();
//                                FinalStr271.Append(HSD(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "MSG":
//                            {
//                                FinalStr271.Append(MSG(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "III":
//                            {
//                                FinalStr271.Append(III(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "LS":
//                            {
//                                FinalStr271.Append("LS  ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
//                                FinalStr271.Append(LS(Segments[i]));
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "LE":
//                            {
//                                FinalStr271.Append("LE  ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "PRV":
//                            {
//                                FinalStr271.Append(PRV(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "HL":
//                            {
//                                FinalStr271.Append(HL(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "TRN":
//                            {
//                                FinalStr271.Append(TRN(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "REF":
//                            {
//                                FinalStr271.Append(REF(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "PER":
//                            {
//                                FinalStr271.Append(PER(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "N3":
//                            {
//                                FinalStr271.Append(N3(Segments[i]));
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "N4":
//                            {
//                                FinalStr271.Append(N4(Segments[i]));
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "NM1":
//                            {
//                                FinalStr271.Append(NM1(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "LX":
//                            {
//                                FinalStr271.Append(Environment.NewLine);
//                                FinalStr271.Append("LX  ..........................................................................");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "DTP":
//                            {
//                                FinalStr271.Append(DTP(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        case "MPI":
//                            {
//                                FinalStr271.Append(MPI(Segments[i]));
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                        default:
//                            {
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                                FinalStr271.Append(Segments[i]);
//                                FinalStr271.Append("----------------------------------------------------------------");
//                                FinalStr271.Append(Environment.NewLine);
//                            }
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private DS271.EDI271BenefitsDetailRow GetNew271BenefitDetailRow()
//        {
//            DS271.EDI271BenefitsDetailRow row2;
//            try
//            {
//                DS271.EDI271BenefitsDetailRow row = this.ds271Parser.EDI271BenefitsDetail.NewEDI271BenefitsDetailRow();
//                row.EDI271BenefitId = this.BenefitsId;
//                this.ds271Parser.EDI271BenefitsDetail.Rows.Add(row);
//                row2 = row;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return row2;
//        }

//        private DS271.EDI271BenefitsRow GetNew271BenefitsRow()
//        {
//            DS271.EDI271BenefitsRow row2;
//            try
//            {
//                DS271.EDI271BenefitsRow row = this.ds271Parser.EDI271Benefits.NewEDI271BenefitsRow();
//                row.EDI271NameId = NameId;
//                this.ds271Parser.EDI271Benefits.Rows.Add(row);
//                this.BenefitsId = Convert.ToInt64(row.EDI271BenefitId);
//                row2 = row;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return row2;
//        }

//        private DS271.EDI271NamesRow GetNew271NameRow()
//        {
//            DS271.EDI271NamesRow row2;
//            try
//            {
//                DS271.EDI271NamesRow row = this.ds271Parser.EDI271Names.NewEDI271NamesRow();
//                this.ds271Parser.EDI271Names.Rows.Add(row);
//                this.NameId = Convert.ToInt64(row.EDI271NameId);
//                row2 = row;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return row2;
//        }

//        private DS271.EDI271NamesRow GetNew271TempNameRow()
//        {
//            DS271.EDI271NamesRow row2;
//            try
//            {
//                DS271.EDI271NamesRow row = this.ds271tempParser.EDI271Names.NewEDI271NamesRow();
//                this.ds271tempParser.EDI271Names.Rows.Add(row);
//                row2 = row;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//            }
//            return row2;
//        }

//        private bool IsBenefitsDTP()
//        {
//            if (this.rowBenefits != null && this.NameId == this.rowBenefits.EDI271NameId)
//                return true;
//            else
//                return false;
//        }

//        #endregion
//    }
//}
