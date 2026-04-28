using MDVision.Common.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using WinSCP;

namespace MDVision.Business.BCommon.SFTP
{
    public class SFTP
    {

        // Setup session options
        //public SessionOptions sessionOptions = new SessionOptions
        //        {
        //            Protocol = Protocol.Sftp,
        //            HostName = "192.168.0.19",
        //            UserName = "sftp1",
        //            Password = "mdvision",
        //            PortNumber = 22,
        //            SshHostKeyFingerprint = "ssh-rsa 1024 a7:dd:10:56:4f:95:db:bf:f0:08:2f:50:17:98:06:b7"
        //        };
        public class clsSFTP
        {

            #region "Class Variable Declarations"

            private string m_sRemoteHostKey;
            private string m_sRemotePath;
            private string m_sRemoteHost;
            private string m_sRemoteUser;
            private string m_sRemotePassword;

            private string m_sMessageString;
            private bool m_bLoggedIn;



            #endregion

            /// <summary>
            /// Default Constructer
            /// </summary>
            public clsSFTP()
            {
                m_sRemoteHost = "microsoft";
                m_sRemotePath = ".";
                m_sRemoteUser = "anonymous";
                m_sRemotePassword = "";
                m_sMessageString = "";
                m_bLoggedIn = false;
            }

            /// <summary>
            /// Parametrized Constructer
            /// </summary>
            /// <param name="sRemoteHost"></param>
            /// <param name="sRemoteUser"></param>
            /// <param name="sRemotePassword"></param>
            /// <param name="iRemotePort"></param>
            /// <param name="hostKey"></param>
            public clsSFTP(string sRemoteHost, string sRemoteUser, string sRemotePassword, string iRemotePort, string hostKey)
            {
                m_sRemoteHost = sRemoteHost;
                m_sRemoteHostKey = hostKey;
                m_sRemoteUser = sRemoteUser;
                m_sRemotePassword = sRemotePassword;
                m_sMessageString = "";
                m_bLoggedIn = false;
            }

            /// <summary>
            /// SFTP Login Authentication With HostKey
            /// </summary>
            /// <param name="hostName"></param>
            /// <param name="userName"></param>
            /// <param name="userPassword"></param>
            /// <param name="portNo"></param>
            /// <param name="sshHostKeyFingerprint"></param>
            /// <returns></returns>
            public bool UserLoginAuthorization_SFTP(string hostName, string userName, string userPassword, string portNo, string sshHostKeyFingerprint, bool isFromService)
            {
                bool sessionOpen;
                SessionOptions sessionOptions = new SessionOptions();
                var with = sessionOptions;
                with.Protocol = Protocol.Sftp;
                with.HostName = hostName;
                with.UserName = userName;
                with.Password = userPassword;
                with.PortNumber = Convert.ToInt32(portNo);
                with.SshHostKeyFingerprint = sshHostKeyFingerprint;

                try
                {
                    using (Session session = new Session())
                    {
                        //session.ExecutablePath = AppDomain.CurrentDomain.BaseDirectory + "obj\\Debug\\winscp.exe";

                        //session.ExecutablePath = AppDomain.CurrentDomain.BaseDirectory + "DLL\\winscp.exe";

                        if (isFromService)
                        {
                            //var winSCPExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                            // if (winSCPExecutablePath.Contains("bin\\Debug\\"))
                            //{
                            //    winSCPExecutablePath = winSCPExecutablePath.Replace("bin\\Debug\\", "");
                            // }
                            //session.ExecutablePath = AppDomain.CurrentDomain.BaseDirectory + "winscp.exe"; // +"WinSCP.exe";
                            session.ExecutablePath = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];
                        }
                        else
                        {
                            var winSCPExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                            if (winSCPExecutablePath.Contains("bin\\Debug\\"))
                            {
                                winSCPExecutablePath = winSCPExecutablePath.Replace("bin\\Debug\\", "");
                            }
                            session.ExecutablePath = winSCPExecutablePath + "DLL\\winscp.exe";
                        }
                        session.Open(sessionOptions);
                        sessionOpen = session.Opened;
                        session.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    sessionOpen = false;
                }
                return sessionOpen;
            }

            /// <summary>
            /// Get List of files in an SFTP Directory
            /// </summary>
            /// <param name="userName"></param>
            /// <param name="userPassword"></param>
            /// <param name="hostName"></param>
            /// <param name="portNo"></param>
            /// <param name="hostkey"></param>
            /// <param name="sDestFolder"></param>
            /// <returns></returns>
            public List<string> GetFiles(string userName, string userPassword, string hostName, Int32 portNo, string hostkey, string sDestFolder, bool isFromService)
            {
                List<string> files = new List<string>();
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = hostName,
                    UserName = userName,
                    Password = userPassword,
                    SshHostKeyFingerprint = hostkey,
                    PortNumber = Convert.ToInt32(portNo)
                };
                using (Session session = new Session())
                {
                    //session.ExecutablePath = AppDomain.CurrentDomain.BaseDirectory + "DLL\\winscp.exe";

                    if (isFromService)
                    {
                        // var winSCPExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                        // if (winSCPExecutablePath.Contains("bin\\Debug\\"))
                        // {
                        //    winSCPExecutablePath = winSCPExecutablePath.Replace("bin\\Debug\\", "");
                        //}
                        //session.ExecutablePath = AppDomain.CurrentDomain.BaseDirectory + "winscp.exe";// +"WinSCP.exe";
                        session.ExecutablePath = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];
                    }
                    else
                    {
                        var winSCPExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                        if (winSCPExecutablePath.Contains("bin\\Debug\\"))
                        {
                            winSCPExecutablePath = winSCPExecutablePath.Replace("bin\\Debug\\", "");
                        }
                        session.ExecutablePath = winSCPExecutablePath + "DLL\\winscp.exe";
                    }
                    session.Open(sessionOptions);
                    RemoteDirectoryInfo directory = session.ListDirectory(sDestFolder);
                    foreach (RemoteFileInfo fileInfo in directory.Files)
                    {
                        files.Add(fileInfo.Name);
                    }
                }
                return files;
            }
            public List<string> GetfilesTeleVox(string HostName, string UserName, string Password, string SshHostKeyFingerprint, string sFTPPath, string localPath)
            {
                List<string> files = new List<string>();
                bool sessionOpen;
                SessionOptions sessionOptions = new SessionOptions();
                var with = sessionOptions;
                with.Protocol = Protocol.Sftp;
                with.HostName = HostName;
                with.UserName = UserName;
                with.Password = Password;
                with.PortNumber = Convert.ToInt32(22);
                with.SshHostKeyFingerprint = SshHostKeyFingerprint;

                try
                {
                    using (Session session = new Session())
                    {
                        //session.ExecutablePath = @"c:\users\mkhawer\documents\visual studio 2015\Projects\TeleVox\TeleVox\DLL\WinSCP.exe";
                        session.ExecutablePath = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];
                        // Download files
                        session.Open(sessionOptions);
                        sessionOpen = session.Opened;

                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        transferResult = session.GetFiles(sFTPPath, localPath, false, transferOptions);
                        transferResult.Check();
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            files.Add(transfer.FileName);
                            //Console.WriteLine("Download of {0} succeeded", transfer.FileName);
                        }
                    }
                    return files;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error: {0}", e);
                    return files;
                }
            }
        }
    }
}
