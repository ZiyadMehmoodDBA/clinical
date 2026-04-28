

// This class permits you to perform direct connections to FTP sites in .NET.
// The class supports the following FTP commands:
//   - Upload a file
//   - Download a file
///''   - Create a directory
///''   - Remove a directory
//   - Change directory
///''   - Remove a file
///''   - Rename a file
//   - Set the user name of the remote user
//   - Set the password of the remote user

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace MDVision.Business.BCommon.FTP
{
    

    //public class cls837BatchFile
    //{



    //    //Private Const sBatchFileExtension As String = ".dat"

    //    public cls837BatchFile()
    //    {
    //        //CreateDirectory()

    //    }

				

    //    ////public string CreateBatchFile(string sBatchFileData, string ctrlNo, string TorP)
    //    ////{

    //    ////    try {
    //    ////        if (string.IsNullOrEmpty(sBatchFileData.Trim)) {
    //    ////            throw new Exception("Enter batch file data.");
    //    ////        }

    //    ////        string sFileName = null;
    //    ////        string sBatchFileExtension = ".dat";
    //    ////        sFileName = ctrlNo;
    //    ////        sFileName = sFileName + sBatchFileExtension;

    //    ////        clsPGPEncryptDecrypt objEnc = new clsPGPEncryptDecrypt();
    //    ////        clsPGPEncryptDecrypt.ServerFolders path = default(clsPGPEncryptDecrypt.ServerFolders);

    //    ////        objEnc.PublicKey = BLLCustomers.PublicKeyPathClearingHouse;
    //    ////        objEnc.SecretKey = BLLCustomers.SecretKeyPathCustomer;
    //    ////        string destfile = objEnc.UploadFile(sFileName, sBatchFileData, path.testclaims, TorP);
    //    ////        return destfile;

    //    ////    } catch (Exception ex) {
    //    ////        throw ex;
    //    ////    }

    //    ////}


		


    //}



	//FTP Class
	//Donot Modify this class
	public class clsFTP
	{

		#region "Class Variable Declarations"
		private string m_sRemoteHost;
		private string m_sRemotePath;
		private string m_sRemoteUser;
		private string m_sRemotePassword;
		private string m_sMess;
		private Int32 m_iRemotePort;
		private Int32 m_iBytes;

		private Socket m_objClientSocket;
		private Int32 m_iRetValue;
		private bool m_bLoggedIn;
		private string m_sMes;

		private string m_sReply;
		//Set the size of the packet that is used to read and to write data to the FTP server
		//to the following specified size.
		public const Int32 BLOCK_SIZE = 512;
		private byte[] m_aBuffer = new byte[BLOCK_SIZE + 1];
		private Encoding ASCII = Encoding.ASCII;
		public bool flag_bool;

        public string[] Dir_IN = { "IN", "IN//STATEMENTS" };
        public string[] Dir_OUT = { "OUT//REPORTS", "OUT//277", "OUT//271", "OUT//997", "OUT//835" };
        
		//General variable declaration
			#endregion
		private string m_sMessageString;

		#region "Class Constructors"

		// Main class constructor
		public clsFTP()
		{
			m_sRemoteHost = "microsoft";
			m_sRemotePath = ".";
			m_sRemoteUser = "anonymous";
			m_sRemotePassword = "";
			m_sMessageString = "";
			m_iRemotePort = 21;
			m_bLoggedIn = false;
		}

		// Parameterized constructor
		public clsFTP(string sRemoteHost, string sRemotePath, string sRemoteUser, string sRemotePassword, Int32 iRemotePort)
		{
			m_sRemoteHost = sRemoteHost;
			m_sRemotePath = sRemotePath;
			m_sRemoteUser = sRemoteUser;
			m_sRemotePassword = sRemotePassword;
			m_sMessageString = "";
            m_iRemotePort = iRemotePort;
			m_bLoggedIn = false;
		}
		#endregion

		#region "Public Properties"

		//Set or Get the name of the FTP server that you want to connect to.
		public string RemoteHostFTPServer {
			//Get the name of the FTP server.
			get { return m_sRemoteHost; }
			//Set the name of the FTP server.
			set { m_sRemoteHost = value; }
		}

		//Set or Get the FTP port number of the FTP server that you want to connect to.
		public Int32 RemotePort {
			//Get the FTP port number.
			get { return m_iRemotePort; }
			//Set the FTP port number.

			set { m_iRemotePort = value; }
		}

		//Set or Get the remote path of the FTP server that you want to connect to.
		public string RemotePath {
			//Get the remote path.
			get { return m_sRemotePath; }
			//Set the remote path.
			set { m_sRemotePath = value; }
		}

		//Set the remote password of the FTP server that you want to connect to.
		public string RemotePassword {
			get { return m_sRemotePassword; }
			set { m_sRemotePassword = value; }
		}

		//Set or Get the remote user of the FTP server that you want to connect to.
		public string RemoteUser {
			get { return m_sRemoteUser; }
			set { m_sRemoteUser = value; }
		}

		//Set the class messagestring.
		public string MessageString {
			get { return m_sMessageString; }
			set { m_sMessageString = value; }
		}

		#endregion

		#region "Public Subs and Functions"

		//Return a list of files from the file system. Return these files in a string() array.
		public string[] GetFileList(string sMask)
		{
			Socket cSocket = default(Socket);
			Int32 bytes = default(Int32);

            char seperator = ControlChars.Lf;
			string[] mess = null;

			m_sMes = "";
			//Check to see if you are logged on to the FTP server.
			if (!(m_bLoggedIn)) {
				Login();
			}

			cSocket = CreateDataSocket();
			//Send an FTP command.
			SendCommand("NLST" + sMask);

			if (!(m_iRetValue == 150 || m_iRetValue == 125)) {
				//MsgBox(m_iRetValue)
				MessageString = m_sReply;
				//If m_iRetValue = 226 Then
				//cSocket.Close()

				//End If
				//MsgBox(m_sReply.Substring(4))
				throw new IOException(m_sReply);
				//.Substring(4))
			}

			m_sMes = "";
			while (true) {
               
                Array.Clear(m_aBuffer, 0, m_aBuffer.Length);
				bytes = cSocket.Receive(m_aBuffer, m_aBuffer.Length, 0);
				m_sMes += ASCII.GetString(m_aBuffer, 0, bytes);

				if (bytes < m_aBuffer.Length) {
					break; // TODO: might not be correct. Was : Exit Do
				}
			}

			mess = m_sMes.Split(seperator);
			cSocket.Close();
			ReadReply();

			if (m_iRetValue != 226) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

			return mess;
		}
		// Get the size of the file on the FTP server.
		public long GetFileSize(string sFileName)
		{
			long size = 0;

			if ((!(m_bLoggedIn))) {
				Login();
			}
			//Send an FTP command.
			SendCommand("SIZE " + sFileName);
			size = 0;

			if (m_iRetValue == 213) {
				size = Int64.Parse(m_sReply.Substring(4));
			} else {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

			return size;
		}


		//Log on to the FTP server.
		public bool Login()
		{

			m_objClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(m_sRemoteHost).AddressList[0], m_iRemotePort);
         
			try {
				m_objClientSocket.Connect(ep);
			} catch (Exception ex) {
				MessageString = m_sReply;
				throw new IOException("Cannot connect to the remote server");

			}

			ReadReply();
			if ((m_iRetValue != 220)) {
				CloseConnection();
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}
			//Send an FTP command to send a user logon ID to the server.
			SendCommand("USER " + m_sRemoteUser);
			if (!(m_iRetValue == 331 | m_iRetValue == 230)) {
				Cleanup();
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

			if (m_iRetValue != 230) {
				//Send an FTP command to send a user logon password to the server.
				SendCommand("PASS " + m_sRemotePassword);
				if (!(m_iRetValue == 230 | m_iRetValue == 202)) {
					Cleanup();
					MessageString = m_sReply;
					throw new IOException(m_sReply.Substring(4));
				}
			}

			m_bLoggedIn = true;
			//Call the ChangeDirectory user-defined function to change the directory to the
			//remote FTP folder that is mapped.
			ChangeDirectory(m_sRemotePath);

			//Return the final result.
			return m_bLoggedIn;
		}

		//If the value of mode is true, set binary mode for downloads. Otherwise, set ASCII mode.

		public void SetBinaryMode(bool bMode)
		{
			if (bMode) {
				//Send the FTP command to set the binary mode.
				//(TYPE is an FTP command that is used to specify representation type.)
				SendCommand("TYPE I");
			} else {
				//Send the FTP command to set ASCII mode.
				//(TYPE is an FTP command that is used to specify representation type.)
				SendCommand("TYPE A");
			}

			if (m_iRetValue != 200) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}
		}
		// Download a file to the local directory of the assembly. Keep the same file name.
		public void DownloadFile(string sFileName)
		{
			DownloadFile(sFileName, "", false);
		}
		// Download a remote file to the local directory of the Assembly. Keep the same file name.
		public void DownloadFile(string sFileName, bool bResume)
		{
			DownloadFile(sFileName, "", bResume);
		}
		//Download a remote file to a local file name. You must include a path.
		//The local file name will be created or will be overwritten, but the path must exist.
		public void DownloadFile(string sFileName, string sLocalFileName)
		{
			DownloadFile(sFileName, sLocalFileName, false);
		}
		// Download a remote file to a local file name. You must include a path. Set the
		// resume flag. The local file name will be created or will be overwritten, but the path must exist.
		public void DownloadFile(string sFileName, string sLocalFileName, bool bResume)
		{
			Stream st = default(Stream);
			FileStream output = default(FileStream);
			Socket cSocket = default(Socket);
			long offset = 0;
			long npos = 0;

			if (!(m_bLoggedIn)) {
				Login();
			}

			SetBinaryMode(true);

			if (sLocalFileName.Equals("")) {
				sLocalFileName = sFileName;
			}

          //  NetworkSecurity.Instance.SharedObj = SharedObj;
            using (NetworkSecurity.Instance.Login())
            {
                if (!(File.Exists(sLocalFileName)))
                {
                    st = File.Create(sLocalFileName);
                    st.Close();
                }

                //output = New FileStream(sLocalFileName, FileMode.Open )
                output = new FileStream(sLocalFileName, FileMode.Create);
                cSocket = CreateDataSocket();
                offset = 0;

                if (bResume)
                {
                    offset = output.Length;

                    if (offset > 0)
                    {
                        //Send an FTP command to restart.
                        SendCommand("REST " + offset);
                        if (m_iRetValue != 350)
                        {
                            offset = 0;
                        }
                    }

                    if (offset > 0)
                    {
                        npos = output.Seek(offset, SeekOrigin.Begin);
                    }
                }
                //Send an FTP command to retrieve a file.
                SendCommand("RETR " + sFileName);

                if (!(m_iRetValue == 150 | m_iRetValue == 125))
                {
                    MessageString = m_sReply;
                    throw new IOException(m_sReply.Substring(4));
                }

                while (true)
                {
                    Array.Clear(m_aBuffer, 0, m_aBuffer.Length);
                    m_iBytes = cSocket.Receive(m_aBuffer, m_aBuffer.Length, 0);
                    output.Write(m_aBuffer, 0, m_iBytes);

                    if (m_iBytes <= 0)
                    {
                        break; // TODO: might not be correct. Was : Exit Do
                    }
                }

                output.Close();
            }
			if (cSocket.Connected) {
				cSocket.Close();
			}

			ReadReply();
			if (!(m_iRetValue == 226 | m_iRetValue == 250)) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

		}
		//This is a function that is used to upload a file from your local hard disk to your FTP site.
		public void UploadFile(string sFileName)
		{
			UploadFile(sFileName, false);
		}
		// This is a function that is used to upload a file from your local hard disk to your FTP site
		// and then set the resume flag.
		public void UploadFile(string sFileName, bool bResume)
		{
			Socket cSocket = default(Socket);
			long offset = 0;
			FileStream input = default(FileStream);
			bool bFileNotFound = false;

			if (!(m_bLoggedIn)) {
				Login();
			}

			cSocket = CreateDataSocket();
			offset = 0;

			if (bResume) {
				try {
					SetBinaryMode(true);
					offset = GetFileSize(sFileName);
				} catch (Exception ex) {
					offset = 0;
				}
			}

			if (offset > 0) {
				SendCommand("REST " + offset);

				if (m_iRetValue != 350) {
					//The remote server may not support resuming.
					offset = 0;
				}
			}
			//Send an FTP command to store a file.
			SendCommand("STOR " + Path.GetFileName(sFileName));
			if (!(m_iRetValue == 125 | m_iRetValue == 150)) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

			//Check to see if the file exists before the upload.
			bFileNotFound = false;
            //fixme  NetworkSecurity.Instance.SharedObj = SharedObj;
              using (NetworkSecurity.Instance.Login())
              {
                  if (File.Exists(sFileName))
                  {
                      // Open the input stream to read the source file.
                      input = new FileStream(sFileName, FileMode.Open);
                      if (offset != 0)
                      {
                          input.Seek(offset, SeekOrigin.Begin);
                      }

                      //Upload the file.
                      m_iBytes = input.Read(m_aBuffer, 0, m_aBuffer.Length);
                      while (m_iBytes > 0)
                      {
                          cSocket.Send(m_aBuffer, m_iBytes, 0);
                          m_iBytes = input.Read(m_aBuffer, 0, m_aBuffer.Length);
                      }
                      input.Close();
                  }
                  else
                  {
                      bFileNotFound = true;
                  }
              }
			if (cSocket.Connected) {
				cSocket.Close();
			}

			//Check the return value if the file was not found.
			if (bFileNotFound) {
				MessageString = m_sReply;
				throw new IOException("The file: " + sFileName + " was not found. " + "Cannot upload the file to the FTP site");
			}

			ReadReply();
			if (!(m_iRetValue == 226 | m_iRetValue == 250)) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}
		}
		// Delete a file from the remote FTP server.
		public bool DeleteFile(string sFileName)
		{
			bool bResult = false;

			bResult = true;
			if (!(m_bLoggedIn)) {
				Login();
			}
			//Send an FTP command to delete a file.
			SendCommand("DELE " + sFileName);
			if (m_iRetValue != 250) {
				bResult = false;
				MessageString = m_sReply;
			}

			// Return the final result.
			return bResult;
		}
		// Rename a file on the remote FTP server.
		//Public Function RenameFile(ByVal sOldFileName As String, _
		//                           ByVal sNewFileName As String) As Boolean
		//    Dim bResult As Boolean

		///'    bResult = True
		///'    If (Not (m_bLoggedIn)) Then
		///'        Login()
		///'    End If
		///'    'Send an FTP command to rename from a file.
		///'    SendCommand("RNFR " & sOldFileName)
		///'    If (m_iRetValue <> 350) Then
		///'        MessageString = m_sReply
		///'        Throw New IOException(m_sReply.Substring(4))
		///'    End If

		///'    'Send an FTP command to rename a file to a new file name.
		///'    'It will overwrite if newFileName exists.
		///'    SendCommand("RNTO " & sNewFileName)
		///'    If (m_iRetValue <> 250) Then
		///'        MessageString = m_sReply
		///'        Throw New IOException(m_sReply.Substring(4))
		///'    End If
		///'    ' Return the final result.
		///'    Return bResult
		///'End Function

		///'This is a function that is used to create a directory on the remote FTP server.
		///'Public Function CreateDirectory(ByVal sDirName As String) As Boolean
		///'    Dim bResult As Boolean

		///'    bResult = True
		///'    If (Not (m_bLoggedIn)) Then
		///'        Login()
		///'    End If
		///'    'Send an FTP command to make directory on the FTP server.
		///'    SendCommand("MKD " & sDirName)
		///'    If (m_iRetValue <> 257) Then
		///'        bResult = False
		///'        MessageString = m_sReply
		///'    End If

		///'    ' Return the final result.
		///'    Return bResult
		///'End Function
		///' This is a function that is used to delete a directory on the remote FTP server.
		///'Public Function RemoveDirectory(ByVal sDirName As String) As Boolean
		///'    Dim bResult As Boolean

		///'    bResult = True
		///'    'Check if logged on to the FTP server
		///'    If (Not (m_bLoggedIn)) Then
		///'        Login()
		///'    End If
		///'    'Send an FTP command to remove directory on the FTP server.
		///'    SendCommand("RMD " & sDirName)
		///'    If (m_iRetValue <> 250) Then
		///'        bResult = False
		///'        MessageString = m_sReply
		///'    End If

		///'    ' Return the final result.
		///'    Return bResult
		///'End Function
		//This is a function that is used to change the current working directory on the remote FTP server.
        public bool CreateDirectory(string sDirName)
        {

            bool bResult = false;

            bResult = true;
            //Check if you are in the root directory.
            if (sDirName.Equals("."))
            {
                return false;
            }
            //Check if logged on to the FTP server
            if (!m_bLoggedIn)
            {
                Login();
            }
            //Send an FTP command to make directory on the FTP server.
            SendCommand("MKD " + sDirName);
            if (m_iRetValue != 257)
            {
                bResult = false;
                MessageString = m_sReply;
            }

            // Return the final result.
            return bResult;

        }
		public bool ChangeDirectory(string sDirName)
		{

			
            bool bResult = false;

			bResult = true;
			//Check if you are in the root directory.
			if (sDirName.Equals(".")) {
				return false;
			}
			//Check if logged on to the FTP server
			if (!m_bLoggedIn) {
				Login();
			}
			//Send an FTP command to change directory on the FTP server.
			SendCommand("CWD " + sDirName);
			if (m_iRetValue != 250) {
				bResult = false;
				MessageString = m_sReply;
			}


			this.m_sRemotePath = sDirName;

			// Return the final result.
			return bResult;
			
		}
		// Close the FTP connection of the remote server.
		public void CloseConnection()
		{
			if (m_objClientSocket != null) {
				//Send an FTP command to end an FTP server system.
				SendCommand("QUIT");
			}

			Cleanup();
		}

		#endregion

		#region "Private Subs and Functions"
		// Read the reply from the FTP server.
		private void ReadReply()
		{
			m_sMes = "";
			m_sReply = ReadLine();
			m_iRetValue = Int32.Parse(m_sReply.Substring(0, 3));
		}

		// Clean up some variables.
		private void Cleanup()
		{
			if (m_objClientSocket != null) {
				m_objClientSocket.Close();
				m_objClientSocket = null;
			}

			m_bLoggedIn = false;
		}
		// Read a line from the FTP server.
		private string ReadLine(bool bClearMes = false)
		{
			char seperator = ControlChars.Lf;
			string[] mess = null;

			if (bClearMes) {
				m_sMes = "";
			}
			while (true) {
				Array.Clear(m_aBuffer, 0, BLOCK_SIZE);
				m_iBytes = m_objClientSocket.Receive(m_aBuffer, m_aBuffer.Length, 0);
				m_sMes += ASCII.GetString(m_aBuffer, 0, m_iBytes);
				if (m_iBytes < m_aBuffer.Length) {
					break; // TODO: might not be correct. Was : Exit Do
				}
			}

			mess = m_sMes.Split(seperator);
			if (m_sMes.Length > 2) {
				m_sMes = mess[mess.Length - 2];
			} else {
				m_sMes = mess[0];
			}

			if (!(m_sMes.Substring(3, 1).Equals(" "))) {
				return ReadLine(true);
			}

			return m_sMes;
		}
		// This is a function that is used to send a command to the FTP server that you are connected to.
		private void SendCommand(string sCommand)
		{
			sCommand = sCommand + ControlChars.CrLf;
			byte[] cmdbytes = ASCII.GetBytes(sCommand);
			m_objClientSocket.Send(cmdbytes, cmdbytes.Length, 0);
			ReadReply();
		}
		// Create a data socket.
		private Socket CreateDataSocket()
		{
			Int32 index1 ;
			Int32 index2 ;
			Int32 len;
			Int32 partCount;
			Int32 i  ;
			Int32 port ;
			string ipData = null;
			string buf = null;
			string ipAddress = null;
			Int32[] parts = new Int32[7];
			char ch = '\0';
			Socket s = default(Socket);
			IPEndPoint ep = default(IPEndPoint);
			//Send an FTP command to use passive data connection.
			SendCommand("PASV");
			if (m_iRetValue != 227) {
				MessageString = m_sReply;
				throw new IOException(m_sReply.Substring(4));
			}

			index1 = m_sReply.IndexOf("(");
			index2 = m_sReply.IndexOf(")");
			ipData = m_sReply.Substring(index1 + 1, index2 - index1 - 1);

			len = ipData.Length;
			partCount = 0;
			buf = "";
         
			for (i = 0; i <= (len - 1) && partCount <= 6; i++) {
				ch = char.Parse(ipData.Substring(i, 1));
				if (char.IsDigit(ch)) {
					buf += ch;
				} else if (ch != ',') {
					MessageString = m_sReply;
					throw new IOException("Malformed PASV reply: " + m_sReply);
				}

				if ((ch == ',') || (i + 1 == len)) {
					try {
						parts[partCount] = Int32.Parse(buf);
						partCount += 1;
						buf = "";
					} catch (Exception ex) {
						MessageString = m_sReply;
						throw new IOException("Malformed PASV reply: " + m_sReply);
					}
				}
			}

			ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];

			// Make this call in Visual Basic .NET 2002. You want to
			// bitshift the number by 8 bits. In Visual Basic .NET 2002 you must
			// multiply the number by 2 to the power of 8.
			//port = parts(4) * (2 ^ 8)

			// Make this call and then comment out the previous line for Visual Basic .NET 2003.
			port = parts[4] << 8;

			// Determine the data port number.
			port = port + parts[5];


			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ep = new IPEndPoint(Dns.GetHostEntry(ipAddress).AddressList[0], port);

			try {
				s.Connect(ep);
			} catch (Exception ex) {
				MessageString = m_sReply;
				throw new IOException("Cannot connect to remote server.");
				//If you cannot connect to the FTP server that is
				//specified, make the boolean variable false.
				flag_bool = false;
			}
			//If you can connect to the FTP server that is specified, make the boolean variable true.
			flag_bool = true;
			return s;
		}

		#endregion

	}

	public class clsFTPDirectory : clsFTP
	{
        private const string Dir_IN = "IN";
        private const string Dir_IN_STATEMENTS = "IN//STATEMENTS";
        private const string Dir_OUT_REPORTS = "OUT//REPORTS";
         private const string Dir_OUT_277 = "OUT//277";
         private const string Dir_OUT_271 = "OUT//271";
         private const string Dir_OUT_997 = "OUT//997";
         private const string Dir_OUT_835 = "OUT//835";
  



			//directory for claims status inquires for Providers.
		private const string Dir_csi = "csi";
			//directory for claims status files being submitted by a Payer 
		private const string Dir_cstin = "cstin";
			//directory for ERA files submitted by a Payer.
		private const string Dir_era = "era";
			//used for testing hospital files for the commercial system
		private const string Dir_testhcds = "testhcds";
			//production medical files for the commercial system
		private const string Dir_mcds = "mcds";
			//used for testing medical and dental claims for the commercial system
		private const string Dir_testmcds = "testmcds";
			//used for OKC processing system 
		private const string Dir_claims = "claims";
			//used for testing through the OKC processing system   
		private const string Dir_testclaims = "testclaims";
			//used to retrieve all mail including reports and acknowledgements
		private const string Dir_mail = "mail";
			//production hospital files for the commercial system 
		private const string Dir_hcds = "hcds";
			//copies of all mail previously received
		private const string Dir_oldmail = "oldmail";
			//local file name plus the assigned Envoy reference number from previous submissions  
		private const string Dir_received = "received";


        public enum FTPDirectory
        {
            csi,
            cstin,
            era,
            testhcds,
            mcds,
            testmcds,
            claims,
            testclaims,
            mail,
            hcds,
            oldmail,
            received,
            In,
            statements,
             reports,
                status277,
                eligibility271,
            ack,
               
        }

		// Parameterized constructor

		public clsFTPDirectory(string sRemoteHost, string sRemotePath, string sRemoteUser, string sRemotePassword, Int32 iRemotePort) : base(sRemoteHost, sRemotePath, sRemoteUser, sRemotePassword, iRemotePort)
		{
		}

		private string GetDirectoryName(FTPDirectory DirectoryName)
		{
			switch (DirectoryName) {

                //case FTPDirectory.csi:

                //    return Dir_csi;
                //case FTPDirectory.cstin:

                //    return Dir_cstin;
                //case FTPDirectory.era:

                //    return Dir_era;
                //case FTPDirectory.testhcds:

                //    return Dir_testhcds;
                //case FTPDirectory.mcds:

                //    return Dir_mcds;
                //case FTPDirectory.testmcds:

                //    return Dir_testmcds;
                //case FTPDirectory.claims:

                //    return Dir_claims;
                //case FTPDirectory.testclaims:

                //    return Dir_testclaims;
                //case FTPDirectory.mail:

                //    return Dir_mail;
                //case FTPDirectory.hcds:

                //    return Dir_hcds;
                //case FTPDirectory.oldmail:

                //    return Dir_oldmail;
                //case FTPDirectory.received:

                //    return Dir_received;
                case FTPDirectory.In :

                    return Dir_IN;
                case FTPDirectory.reports:

                    return Dir_OUT_REPORTS;
                case FTPDirectory.statements:

                    return Dir_IN_STATEMENTS;

                case FTPDirectory.status277:

                    return Dir_OUT_277;
                case FTPDirectory.eligibility271:

                    return Dir_OUT_271;
                case FTPDirectory.ack:

                    return Dir_OUT_997;
                case FTPDirectory.era:

                    return Dir_OUT_835;
			}
            return null;
		}


        //public bool UploadFile(string sFileName, FTPDirectory ServerDirectory)
        //{
        //    try {
        //        if (base.ChangeDirectory(GetDirectoryName(ServerDirectory)) == true) {
        //            base.UploadFile(sFileName);
        //            return true;
        //        } else {
        //            return false;
        //        }

        //    } catch (Exception ex) {
        //        throw ex;
        //    }
        //}


		public bool DownloadFile(string sServerFileName, string sLocalFileName, FTPDirectory ServerDirectory)
		{

			try {
				if (base.ChangeDirectory(GetDirectoryName(ServerDirectory)) == true) {
					base.DownloadFile(sServerFileName, sLocalFileName);
					return true;
				} else {
					return false;
				}

			} catch (Exception ex) {
				throw ex;
			}

		}


		public bool DownloadFiles(FTPDirectory ServerDirectory, string sLocalDirectory)
		{

			try {
				if (base.ChangeDirectory(GetDirectoryName(ServerDirectory)) == true) {
					string[] sDirectoryFiles = base.GetFileList("");
					//("net.txt")
					string sFile = null;
					foreach (string sFile_loopVariable in sDirectoryFiles) {
						sFile = sFile_loopVariable;
						if (!string.IsNullOrEmpty(sFile)) {
							base.DownloadFile(sFile, sLocalDirectory + sFile);
						}
					}
					return true;
				} else {
					return false;
				}

			} catch (Exception ex) {
				throw ex;
			}
		}

	}

    public sealed class ControlChars
    {
        public const char Back = '\b';
        public const char Cr = '\r';
        public const string CrLf = "\r\n";
        public const char FormFeed = '\f';
        public const char Lf = '\n';
        public const string NewLine = "\r\n";
        public const char NullChar = '\0';
        public const char Quote = '"';
        public const char Tab = '\t';
        public const char VerticalTab = '\v';
        public const char Comma = ',';

        //public string CreateBatchFile(string FinalStr837, string BtachCtrlNo)
        //{

        //    try
        //    {


        //        clsPGPEncryptDecrypt objEnc = new clsPGPEncryptDecrypt();
        //        clsPGPEncryptDecrypt.ServerFolders path = default(clsPGPEncryptDecrypt.ServerFolders);

        //        objEnc.PublicKey = BLLCustomers.PublicKeyPathClearingHouse;
        //        objEnc.SecretKey = BLLCustomers.SecretKeyPathCustomer;
        //        string destfile = objEnc.UploadFile(sFileName, sBatchFileData, path.testclaims, TorP);
        //        return destfile;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //Dim FileName As String
        //      FileName = obj.CreateBatchFile(FinalStr837, ctrlNo, sStart)

        //      'If System.Configuration.ConfigurationSettings.AppSettings("TestEClaim") = 1 Then
        //      '    FileName = obj.CreateBatchFile(BatchId, FinalStr837, "T", ctrlNo)
        //      'ElseIf System.Configuration.ConfigurationSettings.AppSettings("TestEClaim") = 0 Then
        //      '    FileName = obj.CreateBatchFile(BatchId, FinalStr837, "P", ctrlNo)
        //      'End If

        //      'GetFileName
        //      Dim ind As Integer
        //      ind = FileName.LastIndexOf("\")
        //      ind = ind + 1 'eliminate \ from string
        //      FileName = FileName.Substring(ind, FileName.Length - ind)


        //    public string Upload837File(string FinalStr837, string BtachCtrlNo, string pDestFile)
        //    {
        //        if (FinalStr837.Trim() == "")
        //        {
        //            throw new Exception("Enter batch file data.");
        //        }


        //        string sFileName = null;
        //        string sBatchFileExtension = ".dat";
        //        sFileName = BtachCtrlNo;
        //        sFileName = sFileName + sBatchFileExtension;

        //        string functionReturnValue = null;
        //        try
        //        {

        //            //pDestFile = "\\192.168.0.253\\ehrdocs$$\\EDI\\" + EntityId + "\\IN\\" + sFileName;


        //            if (pDestFile!="" && FinalStr837!="")
        //            {

        //                FileServer Obj = new FileServer();
        //                Obj.FileWrite(pDestFile, FinalStr837, sFileName);
        //               // CreateBatchFile(pDestFile, FinalStr837);
        //            }
        //            else
        //            {
        //                throw new Exception("Path is not vaild or file does'nt contain data.");
        //                return functionReturnValue;
        //            }



        //            string source = "";

        //            if (!System.IO.File.Exists(pDestFile))
        //            {
        //                throw new Exception("Source file '" + pDestFile + "' not found");
        //                return functionReturnValue;
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(pDestFile))
        //                {
        //                    source = pDestFile;
        //                }
        //                else
        //                {
        //                    throw new Exception("Source file not selected.");
        //                    return functionReturnValue;
        //                }

        //            }


        //            try
        //            {

        //                //===========================================================================================================
        //                bool check = ConnectToFTP();
        //                  if (Login == true) Then
        //            objftp.CloseConnection()
        //            Return True
        //        End If


        //                if (check == true && !string.IsNullOrEmpty(DestFile))
        //                {
        //                    //Dim filename As String = Me.GetFileNameByPath(DestFile)
        //                    check = Upload(DestFile, TorP);
        //                    if (check == true)
        //                    {
        //                        FileInfo FI1 = new FileInfo(DestFile);
        //                        FI1.Delete();
        //                        //Dim FI2 As New FileInfo(source)
        //                        //FI2.Delete()
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Fail to upload file to remote server.");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Fail to connect to remote server.");
        //                }

        //                return DestFile;

        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return functionReturnValue;
        //    }



        //} 

        //    private bool ConnectToFTP()
        //{

        //    try {
        //        objftp = new FTP.clsFTP(HostName, FolderPath, UserName, Password, Port);

        //        root = FolderPath;

        //        if (objftp.Login == true) {
        //            objftp.CloseConnection();
        //            return true;
        //        }
        //    } catch (Exception ex) {
        //        throw ex;
        //    }
        //}




    }
}




