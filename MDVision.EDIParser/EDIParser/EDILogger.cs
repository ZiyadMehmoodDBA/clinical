namespace EDIParser
{
    using System;
    using System.IO;

    public class EDILogger
    {
        public static void LogErrorMessage(string MethodName, Exception exception)
        {
            FileInfo info = null;
            StreamWriter writer = null;
            try
            {
                string path = string.Empty;
                string str2 = "";
                if (str2 == "")
                {
                    path = Path.GetTempPath();
                }
                else
                {
                    path = str2;
                }
                path = path + @"\EDIErrorLog\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "EDIErrorLog.txt";
                info = new FileInfo(path);
                if (File.Exists(path))
                {
                    if (info.Length < 0xf4240L)
                    {
                        writer = info.AppendText();
                    }
                    else
                    {
                        writer = info.CreateText();
                    }
                }
                else
                {
                    writer = info.AppendText();
                }
                writer.WriteLine();
                writer.WriteLine("TIME_STAMP: " + DateTime.Now.ToString());
                writer.WriteLine("METHOD_NAME: " + MethodName);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Error Message:");
                writer.WriteLine(exception.Message);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Inner Exception:");
                writer.WriteLine(exception.InnerException);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Stack Trace:");
                writer.WriteLine(exception.StackTrace);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Source:");
                writer.WriteLine(exception.Source);
                writer.WriteLine("####################################################");
                writer.Close();
            }
            catch (Exception)
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}

