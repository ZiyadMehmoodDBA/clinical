using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace MDVision.Business.BCommon
{
    public class Barcode39
    {


        private const short WIDEBAR_WIDTH = 2;
        private const short NARROWBAR_WIDTH = 1;

        private const int NUM_CHARACTERS = 43;
        private Hashtable mEncoding = new Hashtable();

        char[] mCodeValue = new char[NUM_CHARACTERS];

        //Additional properties 
        public bool ShowString = false;
        public bool IncludeCheckSumDigit = false;
        public Font TextFont = new Font("Courier New", 7);
        public Color TextColor = Color.Black;


        public Barcode39()
        {
            mEncoding.Add("*", "bWbwBwBwb");
            mEncoding.Add("-", "bWbwbwBwB");
            mEncoding.Add("$", "bWbWbWbwb");
            mEncoding.Add("%", "bwbWbWbWb");
            mEncoding.Add(" ", "bWBwbwBwb");
            mEncoding.Add(".", "BWbwbwBwb");
            mEncoding.Add("/", "bWbWbwbWb");
            mEncoding.Add("+", "bWbwbWbWb");
            mEncoding.Add("0", "bwbWBwBwb");
            mEncoding.Add("1", "BwbWbwbwB");
            mEncoding.Add("2", "bwBWbwbwB");
            mEncoding.Add("3", "BwBWbwbwb");
            mEncoding.Add("4", "bwbWBwbwB");
            mEncoding.Add("5", "BwbWBwbwb");
            mEncoding.Add("6", "bwBWBwbwb");
            mEncoding.Add("7", "bwbWbwBwB");
            mEncoding.Add("8", "BwbWbwBwb");
            mEncoding.Add("9", "bwBWbwBwb");
            mEncoding.Add("A", "BwbwbWbwB");
            mEncoding.Add("B", "bwBwbWbwB");
            mEncoding.Add("C", "BwBwbWbwb");
            mEncoding.Add("D", "bwbwBWbwB");
            mEncoding.Add("E", "BwbwBWbwb");
            mEncoding.Add("F", "bwBwBWbwb");
            mEncoding.Add("G", "bwbwbWBwB");
            mEncoding.Add("H", "BwbwbWBwb");
            mEncoding.Add("I", "bwBwbWBwb");
            mEncoding.Add("J", "bwbwBWBwb");
            mEncoding.Add("K", "BwbwbwbWB");
            mEncoding.Add("L", "bwBwbwbWB");
            mEncoding.Add("M", "BwBwbwbWb");
            mEncoding.Add("N", "bwbwBwbWB");
            mEncoding.Add("O", "BwbwBwbWb");
            mEncoding.Add("P", "bwBwBwbWb");
            mEncoding.Add("Q", "bwbwbwBWB");
            mEncoding.Add("R", "BwbwbwBWb");
            mEncoding.Add("S", "bwBwbwBWb");
            mEncoding.Add("T", "bwbwBwBWb");
            mEncoding.Add("U", "BWbwbwbwB");
            mEncoding.Add("V", "bWBwbwbwB");
            mEncoding.Add("W", "BWBwbwbwb");
            mEncoding.Add("X", "bWbwBwbwB");
            mEncoding.Add("Y", "BWbwBwbwb");
            mEncoding.Add("Z", "bWBwBwbwb");

            mCodeValue[0] = '0';
            mCodeValue[1] = '1';
            mCodeValue[2] = '2';
            mCodeValue[3] = '3';
            mCodeValue[4] = '4';
            mCodeValue[5] = '5';
            mCodeValue[6] = '6';
            mCodeValue[7] = '7';
            mCodeValue[8] = '8';
            mCodeValue[9] = '9';
            mCodeValue[10] = 'A';
            mCodeValue[11] = 'B';
            mCodeValue[12] = 'C';
            mCodeValue[13] = 'D';
            mCodeValue[14] = 'E';
            mCodeValue[15] = 'F';
            mCodeValue[16] = 'G';
            mCodeValue[17] = 'H';
            mCodeValue[18] = 'I';
            mCodeValue[19] = 'J';
            mCodeValue[20] = 'K';
            mCodeValue[21] = 'L';
            mCodeValue[22] = 'M';
            mCodeValue[23] = 'N';
            mCodeValue[24] = 'O';
            mCodeValue[25] = 'P';
            mCodeValue[26] = 'Q';
            mCodeValue[27] = 'R';
            mCodeValue[28] = 'S';
            mCodeValue[29] = 'T';
            mCodeValue[30] = 'U';
            mCodeValue[31] = 'V';
            mCodeValue[32] = 'W';
            mCodeValue[33] = 'X';
            mCodeValue[34] = 'Y';
            mCodeValue[35] = 'Z';
            mCodeValue[36] = '-';
            mCodeValue[37] = '.';
            mCodeValue[38] = ' ';
            mCodeValue[39] = '$';
            mCodeValue[40] = '/';
            mCodeValue[41] = '+';
            mCodeValue[42] = '%';

        }

        private string ExtendedString(string s)
        {

            string retVal = "";

            byte[] asciiBytes = Encoding.ASCII.GetBytes(s);

            foreach (byte KeyChar in asciiBytes)
            {

                if (KeyChar == 0)
                {
                    retVal += "%U";
                }
                else if (KeyChar >= 1 && KeyChar <= 26)
                {
                    retVal += "$" + Char.ConvertFromUtf32(64 + KeyChar);
                }
                else if (KeyChar >= 27 && KeyChar <= 31)
                {
                    retVal += "%" + Char.ConvertFromUtf32(65 - 27 + KeyChar);
                }
                else if (KeyChar >= 33 && KeyChar <= 44)
                {
                    retVal += "/" + Char.ConvertFromUtf32(65 - 33 + KeyChar);
                }
                else if (KeyChar == 47)
                {
                    retVal += "/O";
                }
                else if (KeyChar == 58)
                {
                    retVal += "/Z";
                }
                else if (KeyChar >= 59 && KeyChar <= 63)
                {
                    retVal += "%" + Char.ConvertFromUtf32(70 - 59 + KeyChar);
                }
                else if (KeyChar == 64)
                {
                    retVal += "%V";
                }
                else if (KeyChar >= 91 && KeyChar <= 95)
                {
                    retVal += "%" + Char.ConvertFromUtf32(75 - 91 + KeyChar);
                }
                else if (KeyChar == 96)
                {
                    retVal += "%W";
                }
                else if (KeyChar >= 97 && KeyChar <= 122)
                {
                    retVal += "+" + Char.ConvertFromUtf32(65 - 97 + KeyChar);
                }
                else if (KeyChar >= 123 && KeyChar <= 127)
                {
                    retVal += "%" + Char.ConvertFromUtf32(80 - 123 + KeyChar);
                }
                else
                {
                    retVal += Char.ConvertFromUtf32(KeyChar);
                }
            }
            return retVal;
        }//ExtendedString



        public Image GenerateBarcodeImage(int ImageWidth, int ImageHeight, string OriginalString)
        {

            //-- create a image where to paint the bars
            System.Windows.Forms.PictureBox pb = new System.Windows.Forms.PictureBox();

            pb.Width = ImageWidth;
            pb.Height = ImageHeight;
            pb.Image = new Bitmap(pb.Width, pb.Height);
            //---------------------

            //clear the image and set it to white background
            Graphics g = Graphics.FromImage(pb.Image);
            g.Clear(Color.White);


            //get the extended string
            string ExtString = null;
            ExtString = ExtendedString(OriginalString);


            //-- This part format the sring that will be encoded
            //-- The string needs to be surrounded by asterisks 
            //-- to make it a valid Code39 barcode
            string EncodedString = null;
            int ChkSum = 0;
            if (IncludeCheckSumDigit == false)
            {
                EncodedString = string.Format("{0}{1}{0}", "*", ExtString);
            }
            else
            {
                ChkSum = CheckSum(ExtString);

                EncodedString = string.Format("{0}{1}{2}{0}", "*", ExtString, mCodeValue[ChkSum]);
            }
            //----------------------

            //-- write the original string at the bottom if ShowString = True
            SolidBrush textBrush = new SolidBrush(TextColor);
            if (ShowString)
            {
                if ((TextFont != null))
                {
                    //calculates the height of the string
                    float H = g.MeasureString(OriginalString, TextFont).Height;
                    g.DrawString(OriginalString, TextFont, textBrush, 0, ImageHeight - H);
                    ImageHeight = ImageHeight - Convert.ToInt16(H);
                }
            }
            //----------------------------------------

            //THIS IS WHERE THE BARCODE DRAWING HAPPENS
            DrawBarcode(g, EncodedString, ImageHeight);

            //IMAGE OBJECT IS RETURNED
            return pb.Image;


        }


        private void DrawBarcode(Graphics g, string EncodedString, int Height)
        {
            //Start drawing at 0, 0
            int XPosition = 0;
            int YPosition = 0;


            char[] CurrentSymbol = EncodedString.ToCharArray();
            string EncodedSymbol;

            //-- draw the bars
            for (short j = 0; j <= Convert.ToInt16(EncodedString.Length - 1); j++)
            {

                //check if the symbol can be used

                EncodedSymbol = mEncoding[CurrentSymbol[j].ToString()].ToString();

                for (short i = 0; i <= Convert.ToInt16(EncodedSymbol.Length - 1); i++)
                {
                    //Dim CurrentCode As String = EncodedSymbol.Substring(i, 1)
                    char CurrentCode = EncodedSymbol.ToCharArray()[i];

                    g.FillRectangle(getBCSymbolColor(CurrentCode), XPosition, YPosition, getBCSymbolWidth(CurrentCode), Height);

                    XPosition = XPosition + getBCSymbolWidth(CurrentCode);
                }

                //After each written full symbol we need a whitespace (narrow width)
                g.FillRectangle(getBCSymbolColor('w'), XPosition, YPosition, getBCSymbolWidth('w'), Height);
                XPosition = XPosition + getBCSymbolWidth('w');

            }
            //--------------------------

        }

        private System.Drawing.Brush getBCSymbolColor(char symbol)
        {
            if (symbol == 'W' | symbol == 'w')
            {
                return Brushes.White;
            }
            else
            {
                return Brushes.Black;
            }
        }

        private int getBCSymbolWidth(char symbol)
        {
            if (symbol == 'B' | symbol == 'W')
            {
                return WIDEBAR_WIDTH;
            }
            else
            {
                return NARROWBAR_WIDTH;
            }
        }


        #region CheckSum

        private int CheckSum(string sCode)
        {
            int Chk = 0;

            char[] ax = sCode.ToCharArray();

            for (int j = 0; j <= sCode.Length - 1; j++)
            {

                Chk += GetSymbolValue(ax[j]);
            }
            return Chk % (NUM_CHARACTERS);
        }

        private int GetSymbolValue(char s)
        {
            int k = 0;

            for (k = 0; k <= NUM_CHARACTERS; k++)
            {
                if (mCodeValue[k] == s)
                {
                    return k;
                }
            }
            return -1;
        }

        #endregion


    }//class
}
