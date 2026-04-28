using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace MDVision.IEHR.Common
{
     class PresentationLogger_1
    {
        private static void LogErrorMessage(string MethodName, Exception exception,string UserName)
        {
            System.IO.FileInfo objfile = null;

            System.IO.StreamWriter FileWriter = null;

            try
            {
                string strPath = string.Empty;
                string Loging = "";
                if (ConfigurationManager.AppSettings["Loging"] != null)
                    Loging = ConfigurationManager.AppSettings["Loging"].ToString();
                if (Loging == "")
                {
                   

                    strPath = System.IO.Path.GetTempPath();
                }
                else
                {
                    strPath = Loging;

                }

                strPath += @"\ALIAWAN\PresentationErrorLog\";

                if (!System.IO.Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                strPath += "MDVision_SESSION" + UserName + "_ErrorLog.txt";

                objfile = new System.IO.FileInfo(strPath);

                if (System.IO.File.Exists(strPath))
                {
                    if (objfile.Length < 1000000)
                    {
                        FileWriter = objfile.AppendText();
                    }
                    else
                    {
                        FileWriter = objfile.CreateText();
                    }
                }
                else
                {
                    FileWriter = objfile.AppendText();
                }



                FileWriter.WriteLine();
                FileWriter.WriteLine("TIME_STAMP: " + System.DateTime.Now.ToString());
                FileWriter.WriteLine("METHOD_NAME: " + MethodName);
                FileWriter.WriteLine("------------------------------------");
                FileWriter.WriteLine("Error Message:");
                FileWriter.WriteLine(exception.Message);
                FileWriter.WriteLine("------------------------------------");
                FileWriter.WriteLine("Inner Exception:");
                FileWriter.WriteLine(exception.InnerException);
                FileWriter.WriteLine("------------------------------------");
                FileWriter.WriteLine("Stack Trace:");
                FileWriter.WriteLine(exception.StackTrace);
                FileWriter.WriteLine("------------------------------------");
                FileWriter.WriteLine("Source:");
                FileWriter.WriteLine(exception.Source);

                FileWriter.WriteLine("####################################################");
                FileWriter.Close();

            }
            catch (Exception ex)
            {
                if (FileWriter != null)
                {
                    FileWriter.Close();
                }
            }
        }
        private static void Log(string ControlName, string MethodName, string Message, string UserName, string ControlType)
        {
            System.IO.FileInfo objfile = null;

            System.IO.StreamWriter FileWriter = null;

            try
            {
                string strPath = string.Empty;
                string Loging = "";
                if (ConfigurationManager.AppSettings["Loging"] != null)
                    Loging = ConfigurationManager.AppSettings["Loging"].ToString();
                if (Loging == "")
                {
                    

                    strPath = System.IO.Path.GetTempPath();
                }
                else
                {
                    strPath = Loging;

                }

                strPath += @"\ALIAWAN\PresentationLog\";

                if (!System.IO.Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                if (ControlType == "lookup")
                {
                    strPath += "MDVisionLookup_SESSION" + UserName + "_Log.txt";
                }
                else
                {
                    strPath += "MDVision_SESSION" + UserName + "_Log.txt";
                }

                objfile = new System.IO.FileInfo(strPath);

                if (System.IO.File.Exists(strPath))
                {
                    if (objfile.Length < 1000000)
                    {
                        FileWriter = objfile.AppendText();
                    }
                    else
                    {
                        FileWriter = objfile.CreateText();
                    }
                }
                else
                {
                    FileWriter = objfile.AppendText();
                }

                string messgae = "";
                if (Message!="")

                 messgae = string.Format("Request-Time:{0} - Control Name={1} - Method={2} - Message={3}", DateTime.Now.ToString(), ControlName, MethodName, Message);
                else
                  messgae = string.Format("Request-Time:{0} - Control Name={1} - Method={2}", DateTime.Now.ToString(), ControlName, MethodName);

                FileWriter.WriteLine(messgae);
                FileWriter.WriteLine("--------------------------------------------------------------------------------");
                FileWriter.Close();

            }
            catch (Exception ex)
            {
                if (FileWriter != null)
                {
                    FileWriter.Close();
                }
            }
        }
    }
}