using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.EMCodeGenerator
{
    public class EMCodes
    {

        public EMCodes()
        {
            this._99201 = 0;
            this._99202 = 0;
            this._99203 = 0;
            this._99204 = 0;
            this._99205 = 0;
            this._99211 = 0;
            this._99212 = 0;
            this._99213 = 0;
            this._99214 = 0;
            this._99215 = 0;
        }
        private int _99201 { get; set; }
        private int _99202 { get; set; }
        private int _99203 { get; set; }
        private int _99204 { get; set; }
        private int _99205 { get; set; }
        private int _99211 { get; set; }
        private int _99212 { get; set; }
        private int _99213 { get; set; }
        private int _99214 { get; set; }
        private int _99215 { get; set; }

        public void increaseProbability(string code)
        {
            if (code == "99201")
            {
                this._99201 = this._99201 + 1;
            }
            if (code == "99202")
            {
                this._99202 = this._99202 + 1;
            }
            if (code == "99203")
            {
                this._99203 = this._99203 + 1;
            }
            if (code == "99204")
            {
                this._99204 = this._99204 + 1;
            }
            if (code == "99205")
            {
                this._99205 = this._99205 + 1;
            }
            if (code == "99211")
            {
                this._99211 = this._99211 + 1;
            }
            if (code == "99212")
            {
                this._99212 = this._99212 + 1;
            }
            if (code == "99213")
            {
                this._99213 = this._99213 + 1;
            }
            if (code == "99214")
            {
                this._99214 = this._99214 + 1;
            }
            if (code == "99215")
            {
                this._99215 = this._99215 + 1;
            }
        }
        public string getNewPatientCodeSuggestion()
        {
            List<int> Codes = new List<int>();
            Codes.Add(this._99201);
            Codes.Add(this._99202);
            Codes.Add(this._99203);
            Codes.Add(this._99204);
            Codes.Add(this._99205);
            int indexMax = Codes.IndexOf(Codes.Max());
            switch(indexMax)
            {
                case 0:
                    return "99201";

                case 1:
                    return "99202";

                case 2:
                    return "99203";

                case 3:
                    return "99204";

                case 4:
                    return "99205";
                default:
                    return "-1";
            }
        }
        public string getEstablishedPatientCodeSuggestion()
        {
            List<int> Codes = new List<int>();
            Codes.Add(this._99211);
            Codes.Add(this._99212);
            Codes.Add(this._99213);
            Codes.Add(this._99214);
            Codes.Add(this._99215);
            int indexMax = Codes.IndexOf(Codes.Max());
            switch (indexMax)
            {
                case 0:
                    return "99211";

                case 1:
                    return "99212";

                case 2:
                    return "99213";

                case 3:
                    return "99214";

                case 4:
                    return "99215";
                default:
                    return "-1";
            }
        }
    }


}
