using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data;
using MDVision.Common.Shared;

namespace MDVision.Common.Logging
{
    public class MDVLogger
    {
        private static string connetionString = "Data Source=" + System.Configuration.ConfigurationManager.AppSettings["Data Source"] + ";Initial Catalog=" + System.Configuration.ConfigurationManager.AppSettings["Initial Catalog"] + ";Persist Security Info=False;Integrated Security=True";

        public static void SendExcepToDB(Exception exdb, string MethodName, string StrQuery, bool isLogged = true)
        {
            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.ExceptionLoggingToDataBase";
                    command.CommandTimeout = 180;
                    if (MDVSession.Current != null)
                        command.Parameters.AddWithValue("@UserId", MDVSession.Current.AppUserId);
                    else
                        command.Parameters.AddWithValue("@UserId", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    if (string.IsNullOrEmpty(MethodName))
                    {
                        command.Parameters.AddWithValue("@MethodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MethodName", MethodName);
                    }
                    if (string.IsNullOrEmpty(exdb.Message))
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
                    }
                    if (exdb.GetType() == null || string.IsNullOrEmpty(exdb.GetType().Name))
                    {
                        command.Parameters.AddWithValue("@ExceptionType", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
                    }
                    if (exdb.InnerException == null || string.IsNullOrEmpty(exdb.InnerException.ToString()))
                    {
                        command.Parameters.AddWithValue("@InnerException", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InnerException", exdb.InnerException.ToString());
                    }
                    if (string.IsNullOrEmpty(exdb.StackTrace))
                    {
                        command.Parameters.AddWithValue("@StackTrace", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StackTrace", exdb.StackTrace);
                    }
                    if (string.IsNullOrEmpty(exdb.Source))
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", exdb.Source);
                    }
                    if (string.IsNullOrEmpty(StrQuery))
                    {
                        command.Parameters.AddWithValue("@StrQuery", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StrQuery", StrQuery);
                    }
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }

            if (isLogged)
            {
                ExceptionErrorLog(MethodName, StrQuery, exdb, @"\DataAccessErrorLog\");
            }


        }
        public static void SendExcepToDBForMobileAPI(Exception exdb, string MethodName, string StrQuery)
        {

            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.ExceptionLoggingToDataBase";
                    command.CommandTimeout = 180;


                    command.Parameters.AddWithValue("@UserId", DBNull.Value);

                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    if (string.IsNullOrEmpty(MethodName))
                    {
                        command.Parameters.AddWithValue("@MethodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MethodName", MethodName);
                    }


                    command.Parameters.AddWithValue("@ExceptionMsg", exdb.ToString());


                    command.Parameters.AddWithValue("@ExceptionType", DBNull.Value);


                    command.Parameters.AddWithValue("@InnerException", DBNull.Value);


                    command.Parameters.AddWithValue("@StackTrace", DBNull.Value);


                    command.Parameters.AddWithValue("@ExceptionSource", DBNull.Value);

                    if (string.IsNullOrEmpty(StrQuery))
                    {
                        command.Parameters.AddWithValue("@StrQuery", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StrQuery", StrQuery);
                    }
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }


        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <param name="exdb"></param>
        /// <param name="MethodName"></param>
        /// <param name="StrQuery"></param>
        public static void SendExcepToDB(SharedVariable SharedVariable, Exception exdb, string MethodName, string StrQuery)
        {

            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.ExceptionLoggingToDataBase";
                    command.CommandTimeout = 180;
                    command.Parameters.AddWithValue("@UserId", SharedVariable.AppUserId);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    if (string.IsNullOrEmpty(MethodName))
                    {
                        command.Parameters.AddWithValue("@MethodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MethodName", MethodName);
                    }
                    if (string.IsNullOrEmpty(exdb.Message))
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
                    }
                    if (exdb.GetType() == null || string.IsNullOrEmpty(exdb.GetType().Name))
                    {
                        command.Parameters.AddWithValue("@ExceptionType", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
                    }
                    if (exdb.InnerException == null || string.IsNullOrEmpty(exdb.InnerException.ToString()))
                    {
                        command.Parameters.AddWithValue("@InnerException", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InnerException", exdb.InnerException.ToString());
                    }
                    if (string.IsNullOrEmpty(exdb.StackTrace))
                    {
                        command.Parameters.AddWithValue("@StackTrace", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StackTrace", exdb.StackTrace);
                    }
                    if (string.IsNullOrEmpty(exdb.Source))
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", exdb.Source);
                    }
                    if (string.IsNullOrEmpty(StrQuery))
                    {
                        command.Parameters.AddWithValue("@StrQuery", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StrQuery", StrQuery);
                    }
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }


        }

        public static void SendExcepToDB(long AppUserId, Exception exdb, string MethodName, string StrQuery)
        {
            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.ExceptionLoggingToDataBase";
                    command.CommandTimeout = 180;
                    command.Parameters.AddWithValue("@UserId", AppUserId);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    if (string.IsNullOrEmpty(MethodName))
                    {
                        command.Parameters.AddWithValue("@MethodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MethodName", MethodName);
                    }

                    if (string.IsNullOrEmpty(exdb.Message))
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
                    }

                    if (exdb.GetType() == null || string.IsNullOrEmpty(exdb.GetType().Name))
                    {
                        command.Parameters.AddWithValue("@ExceptionType", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
                    }

                    if (exdb.InnerException == null || string.IsNullOrEmpty(exdb.InnerException.ToString()))
                    {
                        command.Parameters.AddWithValue("@InnerException", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InnerException", exdb.InnerException.ToString());
                    }

                    if (string.IsNullOrEmpty(exdb.StackTrace))
                    {
                        command.Parameters.AddWithValue("@StackTrace", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StackTrace", exdb.StackTrace);
                    }

                    if (string.IsNullOrEmpty(exdb.Source))
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionSource", exdb.Source);
                    }

                    if (string.IsNullOrEmpty(StrQuery))
                    {
                        command.Parameters.AddWithValue("@StrQuery", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StrQuery", StrQuery);
                    }

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }
        }

        public static void SendRcopiaExcepToDB(string CommandName, string RcopiaID, string Status, string InteractionXML, [Optional]string error, [Optional]int errorcount, Int64 UserId = 0)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.sp_RcopiaErrorLogToDataBase";
                    command.CommandTimeout = 180;
                    if (UserId != 0)
                    {
                        command.Parameters.AddWithValue("@UserId", UserId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@UserId", MDVSession.Current.AppUserId);
                    }
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    if (string.IsNullOrEmpty(CommandName))
                    {
                        command.Parameters.AddWithValue("@MethodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MethodName", CommandName);
                    }
                    if (string.IsNullOrEmpty(RcopiaID))
                    {
                        command.Parameters.AddWithValue("@RocpiaId", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RocpiaId", RcopiaID);
                    }
                    if (string.IsNullOrEmpty(Status))
                    {
                        command.Parameters.AddWithValue("@Status", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Status", Status);
                    }
                    if (string.IsNullOrEmpty(InteractionXML))
                    {
                        command.Parameters.AddWithValue("@InteractionXML", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InteractionXML", InteractionXML);
                    }

                    if (errorcount > 0)
                    {
                        command.Parameters.AddWithValue("@ErrorCount", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ErrorCount", errorcount);
                    }
                    if (string.IsNullOrEmpty(error))
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExceptionMsg", error);
                    }
                    command.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        #region Presentation Logs

        public static void PresentationErrorLog(string MethodName, Exception exception, string UserName, bool LogInDb = true)
        {

            SendExcepToDB(exception, MethodName, null, false);
            ExceptionErrorLog(MethodName, "", exception, @"\PresentationErrorLog\");
        }



        #endregion

        #region Business Logs

        public static void BLLErrorLog(string MethodName, Exception exception, bool LogInDb = true)
        {
            SendExcepToDB(exception, MethodName, null, false);
            ExceptionErrorLog(MethodName, "", exception, @"\BusinessErrorLog\");
        }

        public static void BLLErrorLog(SharedVariable SharedVariable, string MethodName, Exception exception, bool LogInDb = true)
        {
            SendExcepToDB(SharedVariable, exception, MethodName, null);
            ExceptionErrorLog(MethodName, "", exception, @"\BusinessErrorLog\");
        }
        #endregion

        #region DataAccess Logs

        public static void DALErrorLog(string MethodName, string StrQuery, Exception exception, bool LogInDb = true)
        {
            SendExcepToDB(exception, MethodName, StrQuery, false);
            ExceptionErrorLog(MethodName, StrQuery, exception, @"\DataAccessErrorLog\");
        }

        public static void DALErrorLog(SharedVariable SharedVariable, string MethodName, string StrQuery, Exception exception, bool LogInDb = true)
        {
            SendExcepToDB(SharedVariable, exception, MethodName, StrQuery);
            ExceptionErrorLog(MethodName, StrQuery, exception, @"\DataAccessErrorLog\");
        }

        public static void DALErrorLog(long AppUserId, string MethodName, string StrQuery, Exception exception, bool LogInDb = true)
        {
            SendExcepToDB(AppUserId, exception, MethodName, StrQuery);
            ExceptionErrorLog(MethodName, StrQuery, exception, @"\DataAccessErrorLog\");
        }

        public static void ExceptionErrorLog(string MethodName, string StrQuery, Exception exception, string LayerName = @"\DataAccessErrorLog\")
        {

            FileInfo info = null;
            StreamWriter writer = null;

            try
            {
                string path = string.Empty;
                string str2 = "";

                if (ConfigurationManager.AppSettings["Loging"] != null)
                {
                    str2 = ConfigurationManager.AppSettings["Loging"].ToString();
                }

                if (str2 == "")
                {
                    string tempPath = Path.GetTempPath();
                    path = Path.GetTempPath();
                }
                else
                {
                    path = str2;
                }

                path = path + LayerName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                path = path + "ErrorLog_" + currentDate + ".txt";
                info = new FileInfo(path);

                if (File.Exists(path))
                {
                    if (info.Length < 2056192)
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
                writer.WriteLine("SQL_QUERY: " + StrQuery);
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
        public static void LogMobileLoginException(string ExceptionMessage, string UserName)
        {
            FileInfo info = null;
            StreamWriter writer = null;

            try
            {
                string path = string.Empty;
                string str2 = "";

                if (ConfigurationManager.AppSettings["Loging"] != null)
                {
                    str2 = ConfigurationManager.AppSettings["Loging"].ToString();
                }

                if (str2 == "")
                {
                    string tempPath = Path.GetTempPath();
                    path = Path.GetTempPath();
                }
                else
                {
                    path = str2;
                }

                path = path + @"\MobileLoginException\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                path = path + "Login_Exception_Date_" + currentDate + ".txt";
                info = new FileInfo(path);

                if (File.Exists(path))
                {


                    writer = info.AppendText();

                }
                else
                {
                    writer = info.AppendText();
                }
                writer.WriteLine();
                writer.WriteLine("TIME_STAMP: " + DateTime.Now.ToString());
                writer.WriteLine("------------------------------------");
                writer.WriteLine("ExceptionMessage: " + ExceptionMessage);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("UserName: " +  UserName);
                writer.WriteLine("------------------------------------");         
                writer.WriteLine("Issued UTC:");
                writer.WriteLine(DateTime.UtcNow);
                writer.WriteLine("------------------------------------");

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
        public static void PresentationLog(string ControlName, string MethodName, string Message, string UserName, string ControlType, bool LogInDb = true)
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

                strPath += @"\PresentationLog\";

                if (!System.IO.Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                if (ControlType == "lookup")
                {
                    strPath += "MDVisionLookup" + UserName + "_Log.txt";
                }
                else
                {
                    strPath += "MDVision" + UserName + "_Log.txt";
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
                if (Message != "")

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

        public static void RcopiaLogMessage(string CommandName, string RcopiaID, string Status, string InteractionXML, [Optional]string error, [Optional]int errorcount, bool LogInDb = true, Int64 UserId = 0)
        {

            SendRcopiaExcepToDB(CommandName, RcopiaID, Status, InteractionXML, error, errorcount, UserId);

            FileInfo info = null;
            StreamWriter writer = null;
            try
            {
                string path = string.Empty;
                string str2 = "";
                if (ConfigurationManager.AppSettings["Loging"] != null)
                {
                    str2 = ConfigurationManager.AppSettings["Loging"].ToString();
                }
                if (str2 == "")
                {
                    string tempPath = Path.GetTempPath();
                    path = Path.GetTempPath();
                }
                else
                {
                    path = str2;
                }
                path = path + @"\DataAccessErrorLog\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "MDVisionRcopiaLog.txt";
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
                writer.WriteLine(CommandName);
                writer.WriteLine("FOR RCOPIA: " + RcopiaID);
                writer.WriteLine(Status);
                writer.WriteLine("at" + DateTime.Now);
                writer.WriteLine("------------------------------------");
                writer.WriteLine(InteractionXML);
                if (error != string.Empty)
                {
                    writer.WriteLine("Try={0} and Error Message= {1}", errorcount, error);
                    writer.WriteLine("------------------------------------");
                }
                else
                {

                }
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

        #endregion
    }
}