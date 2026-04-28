using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Clinical.LetterDesign
{
    public class Design_LetterPrinting
    {
        #region Singleton
        private static Design_LetterPrinting _obj = null;
        public static Design_LetterPrinting Instance()
        {
            if (_obj == null)
                _obj = new Design_LetterPrinting();
            return _obj;
        }
        #endregion

        #region "Private Functions"

        private string FindPrinters()
        {
            try
            {
                List<string> list = new List<string>();

                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {

                    list.Add(printer);
                }
                var response = new
                {
                    printersList = list,
                    status = true,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SetPrinter(string printerName)
        {
            myPrinters.SetDefaultPrinter(printerName);
            return "";
        }

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "FIND_PRINTERS":
                    {

                        string strJSONData = FindPrinters();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SET_PRINTER":
                    {
                        string printerName = MDVUtility.ToStr(context.Request["PrinterName"]);
                        string strJSONData = SetPrinter(printerName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion
    }

    public static class myPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
}