using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class PhiMailConnector : IDisposable
    {
        /// <summary>Major and minor version of connector class</summary>
        public const string VERSION = "1.0";

        /// <summary>Build number of connector class</summary>
        public const string BUILD = "27b";

        /// <summary>Version of API implemented by connector class</summary>
        public const string API_VERSION = "1.3.1";

        /// <summary>Read timeout in milliseconds for connection to server</summary>
        public static int PHIMAIL_READ_TIMEOUT = 25000;

        private static bool checkCertificateRevocation = true;
        private static X509Certificate trustAnchorCertificate = null;
        private static X509CertificateCollection defaultClientCertificates = null;
        private X509CertificateCollection clientCertificates;

        private SslStream _SslStream = null;
        private TcpClient Client = null;

        private StreamReader StreamIn = null;
        private StreamWriter StreamOut = null;

        /// <summary>
        /// This class holds the result of a Send operation.
        /// </summary>
        public class SendResult
        {
            /// <summary>The Direct Address of the recipient</summary>
            public string Recipient;
            /// <summary>True if Send operation succeeded, false if failed</summary>
            public bool Succeeded;
            /// <summary>On success, the unique ID assigned by the server to the outgoing message</summary>
            public string MessageId = null;
            /// <summary>On failure, any additional information avaiable regarding the failure</summary>
            public string ErrorText = null;

            /// <summary>
            /// Constructor used internally by connector class
            /// </summary>
            public SendResult(string r, bool s, string m)
            {
                Recipient = r;
                Succeeded = s;
                if (s)
                    MessageId = m;
                else
                    ErrorText = m;
            }
        }

        /// <summary>
        /// This class holds the result of a Check operation.
        /// </summary>
        public class CheckResult
        {
            /// <summary>True if this result represents a regular Direct message, false if it is a status notification.</summary>
            public bool Mail;

            /// <summary>The unique message ID. For regular messages, this ID is assigned by the sender.
            /// For status notifications, this will match the ID originally returned by the Send operation.</summary>
            public string MessageId;

            /// <summary>For status notifications, the message disposition code, e.g. "dispatched" or "failed".</summary>
            public string StatusCode;

            /// <summary>For status notifications, any additional information available regarding the message disposition.</summary>
            public string Info;

            /// <summary>For regular Direct messages, the intended recipient's Direct Address.</summary>
            public string Recipient;

            /// <summary>For regular Direct messages, the sender's Direct Address.</summary>
            public string Sender;

            /// <summary>For regular Direct messages, the number of attachments included in the message.</summary>
            public int NumAttachments;

            // for status messages
            /// <summary>
            /// Constructor used internally by connector class
            /// </summary>
            public CheckResult(string id, string status, string info)
            {
                Mail = false;
                MessageId = id;
                StatusCode = status;
                this.Info = info;
                Recipient = null;
                Sender = null;
                NumAttachments = 0;
            }

            // for mail messages
            /// <summary>
            /// Constructor used internally by connector class
            /// </summary>
            public CheckResult(string r, string s, int numAttach, string id)
            {
                Mail = true;
                MessageId = id;
                StatusCode = null;
                Info = null;
                Recipient = r;
                Sender = s;
                NumAttachments = numAttach;
            }

            /// <summary>
            /// Test if Check returned a regular Direct message.
            /// </summary>
            /// <returns>True if Check returned a regular Direct message, false otherwise</returns>
            public bool IsMail()
            {
                return Mail;
            }

            /// <summary>
            /// Test if Check returned a status notification.
            /// </summary>
            /// <returns>True if Check returned a status notification, false otherwise</returns>
            public bool IsStatus()
            {
                return !Mail;
            }
        }

        /// <summary>
        /// This class holds the result of a Show operation.
        /// </summary>
        public class ShowResult
        {
            /// <summary>The message part number represented by this result. 0 = main message body, 1 = first attachment, etc.</summary>
            public int PartNum;

            /// <summary>A list of message headers, such as From: ..., Date: ..., etc.</summary>
            public List<string> Headers;

            /// <summary>For attachments, the filename associated with the attachment.</summary>
            public string Filename;

            /// <summary>For attachments, the MIME type associated with the attachment.</summary>
            public string MimeType;

            /// <summary>The length in bytes of the content included in the Data array.</summary>
            public int Length;

            /// <summary>The content of this message part.</summary>
            public byte[] Data;

            /// <summary>For the main message body (part 0), an array of AttachmentInfo objects.</summary>
            public AttachmentInfo[] AttachmentInfo;

            /// <summary>
            /// Constructor used internally by connector class
            /// </summary>
            public ShowResult(int p, List<string> h, string f, string m, int l,
                    byte[] d, AttachmentInfo[] ai)
            {
                PartNum = p;
                Headers = h;
                Filename = f;
                MimeType = m;
                Length = l;
                Data = d;
                AttachmentInfo = ai;
            }
        }

        /// <summary>
        /// This class holds information about a message attachment.
        /// </summary>
        public class AttachmentInfo
        {
            /// <summary>The filename associated with this attachment.</summary>
            public string Filename;

            /// <summary>The MIME type of this attachment.</summary>
            public string MimeType;

            /// <summary>The description of this attachment, if provided by sender.</summary>
            public string Description;

            /// <summary>
            /// Constructor used internally by connector class
            /// </summary>
            public AttachmentInfo(string filename, string mimeType, string description)
            {
                this.Filename = filename;
                this.MimeType = mimeType;
                this.Description = description;
            }
        }

        /// <summary>
        /// Open new connector.
        /// </summary>
        /// <param name="host">hostname of the phiMail server</param>
        /// <param name="port">server port for connection</param>
        public PhiMailConnector(string host, int port)
        {
            RemoteCertificateValidationCallback remoteValidationCallback =
              new RemoteCertificateValidationCallback(ServerValidationCallback);
            LocalCertificateSelectionCallback localSelectionCallback =
              new LocalCertificateSelectionCallback(ClientCertificateSelectionCallback);

            clientCertificates = defaultClientCertificates;
            Initialize(host, port, remoteValidationCallback, localSelectionCallback);
        }

        /// <summary>
        /// Open new connector.
        /// </summary>
        /// <param name="host">hostname of the phiMail server</param>
        /// <param name="port">server port for connection</param>
        /// <param name="clientCertificate">client certificate for mutual TLS authentication</param>
        /// <exception cref="System.InvalidOperationException">Thrown when client certificate does not include private key.</exception>
        public PhiMailConnector(string host, int port, X509Certificate2 clientCertificate)
        {
            RemoteCertificateValidationCallback remoteValidationCallback =
              new RemoteCertificateValidationCallback(ServerValidationCallback);
            LocalCertificateSelectionCallback localSelectionCallback =
              new LocalCertificateSelectionCallback(ClientCertificateSelectionCallback);

            if (clientCertificate == null) clientCertificates = null;
            else if (clientCertificate.HasPrivateKey)
            {
                clientCertificates = new X509Certificate2Collection(clientCertificate);
            }
            else
            {
                throw new InvalidOperationException("Invalid certificate; client certificate requires a private key.");
            }

            Initialize(host, port, remoteValidationCallback, localSelectionCallback);
        }

        /// <summary>
        /// Open new connector.
        /// </summary>
        /// <param name="host">hostname of the phiMail server</param>
        /// <param name="port">server port for connection</param>
        /// <param name="remoteValidationCallback">custom remote certificate validation callback</param>
        /// <param name="localSelectionCallback">custom local certificate selection callback</param>
        public PhiMailConnector(string host, int port,
            RemoteCertificateValidationCallback remoteValidationCallback,
            LocalCertificateSelectionCallback localSelectionCallback)
        {
            clientCertificates = defaultClientCertificates;
            Initialize(host, port, remoteValidationCallback, localSelectionCallback);
            String response = SendCommand("INFO VER .NET/C# " + VERSION + " . " + BUILD);
        }

        private void Initialize(string host, int port,
            RemoteCertificateValidationCallback remoteValidationCallback,
            LocalCertificateSelectionCallback localSelectionCallback)
        {
            bool leaveInnerStreamOpen = false;

            Client = new TcpClient(host, port);

            //create the SSL stream starting from the NetworkStream associated with Client
            _SslStream = new SslStream(Client.GetStream(), leaveInnerStreamOpen,
                remoteValidationCallback, localSelectionCallback);
            _SslStream.ReadTimeout = PHIMAIL_READ_TIMEOUT;

            //start the authentication process. If it doesn't succeed an AuthenticationException is thrown
            ClientSideHandshake(host);

            StreamIn = new StreamReader(_SslStream, System.Text.Encoding.UTF8);
            StreamOut = new StreamWriter(_SslStream, new UTF8Encoding(false, true));

        }

        private void ClientSideHandshake(string targetHost)
        {
            SslProtocols sslProtocol = SslProtocols.Tls;
            //Start the handshake
            _SslStream.AuthenticateAsClient(targetHost, defaultClientCertificates, sslProtocol, checkCertificateRevocation);
        }

        /// <summary>
        /// The default remote certificate validation callback.
        /// </summary>
        public static bool ServerValidationCallback
               (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            //if an anchor was specified, it must be in the certificate chain
            if (trustAnchorCertificate != null)
            {
                bool foundAnchor = false;
                foreach (X509ChainElement element in chain.ChainElements)
                {
                    if (element.Certificate.Equals(trustAnchorCertificate))
                    {
                        foundAnchor = true;
                        break;
                    }
                }
                if (!foundAnchor) throw new InvalidOperationException(
                    "Server certificate failed validation. Certificate does not chain to specified anchor.");

                //if anchor was matched, allow untrusted root error when this is the only error
                X509ChainStatusFlags statusFlags = X509ChainStatusFlags.NoError;
                foreach (X509ChainStatus status in chain.ChainStatus) statusFlags |= status.Status;
                if (statusFlags == X509ChainStatusFlags.UntrustedRoot)
                {
                    sslPolicyErrors &= ~SslPolicyErrors.RemoteCertificateChainErrors;
                }
            }

            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            //otherwise provide a meaningful exception
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != SslPolicyErrors.None)
            {
                throw new InvalidOperationException("phiMail Server certificate not available.");
            }
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != SslPolicyErrors.None)
            {
                StringBuilder sb = new StringBuilder("phiMail Server certificate failed validation.");
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    if ((status.Status & X509ChainStatusFlags.UntrustedRoot) != X509ChainStatusFlags.NoError)
                    {
                        sb.Append(" The certificate does not chain to a Trusted Root CA certificate on your system.")
                            .Append(" This exception usually occurs when the correct root certificate is not properly installed on your system.");
                    }
                    else sb.Append(" ").Append(status.StatusInformation.Trim());
                }
                throw new InvalidOperationException(sb.ToString());
            }
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != SslPolicyErrors.None)
            {
                throw new InvalidOperationException("phiMail Server name mismatch.");
            }

            return false;

        }

        /// <summary>
        /// Set a trusted phiMail Server SSL anchor using the anchor's X509 certificate. This trust anchor 
        /// will be allowed even if it is not in the system's trusted certificate store.
        /// </summary>
        /// <param name="anchorCert">The trust anchor certificate.</param>
        public static void SetServerCertificate(X509Certificate anchorCert)
        {
            trustAnchorCertificate = anchorCert;
        }

        /// <summary>
        /// Alias for SetServerCertificate.
        /// 
        /// 
        /// 

        /// <param name="filename">The name of the file containing the trust anchor certificate.</param>
        public static void SetServerCertificate(string filename)
        {
            trustAnchorCertificate = new X509Certificate2(filename);
        }

        /// <summary>
        /// Alias for SetServerCertificate.
        /// </summary>
        [Obsolete("use SetServerCertificate instead.")]
        public static void SetTrustAnchor(string filename)
        {
            SetServerCertificate(filename);
        }

        /// <summary>
        /// Set the phiMail Server SSL connection certificate revocation checking flag.
        /// </summary>
        /// <param name="desiredCheckRevocation">True to check revocation status, false otherwise.</param>
        public static void SetCheckRevocation(bool desiredCheckRevocation)
        {
            checkCertificateRevocation = desiredCheckRevocation;
        }

        /// <summary>
        /// Set the phiMail Server SSL connection client certificate for mutual TLS authentication.
        /// </summary>
        /// <param name="clientCertificate">client certificate for mutual TLS authentication</param>
        /// <exception cref="System.InvalidOperationException">Thrown when certificate does not include private key.</exception>
        public static void SetClientCertificate(X509Certificate2 clientCertificate)
        {
            if (clientCertificate == null) defaultClientCertificates = null;
            else if (clientCertificate.HasPrivateKey)
            {
                defaultClientCertificates = new X509Certificate2Collection(clientCertificate);
            }
            else
            {
                throw new InvalidOperationException("Invalid certificate; client certificate requires a private key.");
            }
        }

        /// <summary>
        /// The default client certificate selection callback.
        /// </summary>
        public static X509Certificate ClientCertificateSelectionCallback(object sender, string targetHost,
            X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            if (localCertificates == null || localCertificates.Count == 0) return null;
            return localCertificates[0];
        }

        /// <summary>
        /// Send API command to server. Generally, this method is only used in internally by the connector class,
        /// but is made public in case it is needed when adding custom API functionality.
        /// </summary>
        /// <param name="command">The API command to send to the server.</param>
        /// <returns>Returns the result code returned by the server.</returns>
        public string SendCommand(string command)
        {
            if (command.Contains("\n") || command.Contains("\r"))
            {
                SendCommand("INFO ERR illegal characters in command string.");
                return ("FAIL illegal characters in command string.");
            }
            StreamOut.Write(command);
            StreamOut.Write("\n");
            StreamOut.Flush();
            return StreamIn.ReadLine();
        }

        /// <summary>
        /// Close connector.
        /// </summary>
        public void Close()
        {
            if (StreamIn != null)
            {
                StreamIn.Close();
                StreamIn = null;
            }
            if (StreamOut != null)
            {
                StreamOut.Close();
                StreamOut = null;
            }
            if (_SslStream != null)
            {
                _SslStream.Close();
                _SslStream = null;
            }
            if (Client != null)
            {
                Client.Close();
                Client = null;
            }
        }

        /// <summary>
        /// Authenticate user to server.
        /// </summary>
        /// <param name="user">The user's user name on the server.</param>
        /// <param name="pass">The user's password; may be null or empty.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when authentication fails.</exception>
        public void AuthenticateUser(string user, string pass)
        {
            string response = SendCommand("AUTH " + user
                + (pass != null && pass.Length > 0 ? " " + pass : ""));
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Authentication failed: " + response);
        }

        /// <summary>
        /// Change password on server for currently authenticated user.
        /// </summary>
        /// <param name="pass">The user's new password.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when password change fails.</exception>
        public void ChangePassword(string pass)
        {
            string response = SendCommand("PASS " + pass);
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Password change failed: " + response);
        }

        /// <summary>
        /// Add a recipient to outgoing message.
        /// </summary>
        /// <param name="recipient">The Direct Address of the intended recipient.</param>
        /// <returns>A string containing the information found in the corresponding public certificate.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the recipient cannot be added.</exception>
        public string AddRecipient(string recipient)
        {
            string response = SendCommand("TO " + recipient);
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Add recipient failed: " + response);
            //get recipient info
            return StreamIn.ReadLine();
        }

        /// <summary>
        /// Add a CC recipient to outgoing message.
        /// </summary>
        /// <param name="recipient">The Direct Address of the intended CC recipient.</param>
        /// <returns>A string containing the information found in the corresponding public certificate.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the CC recipient cannot be added.</exception>
        public string AddCCRecipient(string recipient)
        {
            string response = SendCommand("CC " + recipient);
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Add CC recipient failed: " + response);
            //get recipient info
            return StreamIn.ReadLine();
        }

        /// <summary>
        /// Clear content and headers of outgoing message.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void Clear()
        {
            string response = SendCommand("CLEAR");
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Clear failed: " + response);
        }

        /// <summary>
        /// Logout currently authenticated user.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void Logout()
        {
            string response = SendCommand("LOGOUT");
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Logout failed: " + response);
        }

        /// <summary>
        /// Send goodbye to server.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        [Obsolete("use Close() without calling Bye() first.")]
        public void Bye()
        {
            string response = SendCommand("BYE");
            if (!response.Equals("BYE"))
                throw new InvalidOperationException("Bye failed: " + response);
        }

        /// <summary>
        /// Add a byte array as a MIME part to the current outgoing message.
        /// </summary>
        /// <param name="dataBytes">The MIME part content</param>
        /// <param name="dataType">The MIME type of the content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        private void AddData(byte[] dataBytes, string dataType, string filename)
        {
            string response = SendCommand(dataType + " " + dataBytes.Length +
                (filename != null && filename.Length > 0 ? " " + filename : ""));
            if (!response.Equals("BEGIN"))
                throw new InvalidOperationException("Add " + dataType + " failed: " + response);
            _SslStream.Write(dataBytes, 0, dataBytes.Length);
            _SslStream.Flush();
            response = StreamIn.ReadLine();
            if (response == null || !response.Equals("OK"))
                throw new InvalidOperationException("Add " + dataType + " failed: " +
                        (response == null ? "" : response));
        }

        /// <summary>
        /// Add a string as a MIME part to the current outgoing message.
        /// </summary>
        /// <param name="data">The MIME part content</param>
        /// <param name="dataType">The MIME type of the content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        private void AddData(string data, string dataType, string filename)
        {
            AddData(Encoding.UTF8.GetBytes(data), dataType, filename);
        }

        /// <summary>
        /// Add a preformed MIME part to the current outgoing message. This can be
        /// a simple MIME part or a MIME multipart and must be 7-bit encoded for SMTP transport.
        /// </summary>
        /// <param name="data">The MIME part content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddMIME(string data)
        {
            AddData(data, "ADD MIME", null);
        }

        /// <summary>
        /// Add an XML CDA document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddCDA(string data)
        {
            AddData(data, "ADD CDA", null);
        }

        /// <summary>
        /// Add an XML document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddXML(string data)
        {
            AddData(data, "ADD CDA", null);
        }

        /// <summary>
        /// Add an XML CCR document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        [Obsolete("use AddXML instead.")]
        public void AddCCR(string data)
        {
            AddData(data, "ADD CCR", null);
        }

        /// <summary>
        /// Add a text document to the current outgoing message.
        /// </summary>
        /// <param name="data">The text content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddText(string data)
        {
            AddData(data, "ADD TEXT", null);
        }

        /// <summary>
        /// Add binary data to the current outgoing message.
        /// </summary>
        /// <param name="dataBytes">The text content</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddRaw(byte[] dataBytes)
        {
            AddData(dataBytes, "ADD RAW", null);
        }

        /// <summary>
        /// Add an XML CDA document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddCDA(string data, string filename)
        {
            AddData(data, "ADD CDA", filename);
        }

        /// <summary>
        /// Add an XML document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddXML(string data, string filename)
        {
            AddData(data, "ADD CDA", filename);
        }

        /// <summary>
        /// Add an XML CCR document to the current outgoing message.
        /// </summary>
        /// <param name="data">The XML content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        [Obsolete("use AddXML instead.")]
        public void AddCCR(string data, string filename)
        {
            AddData(data, "ADD CCR", filename);
        }

        /// <summary>
        /// Add a text document to the current outgoing message.
        /// </summary>
        /// <param name="data">The text content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddText(string data, string filename)
        {
            AddData(data, "ADD TEXT", filename);
        }

        /// <summary>
        /// Add binary data to the current outgoing message.
        /// </summary>
        /// <param name="dataBytes">The text content</param>
        /// <param name="filename">The filename to associate with the attachment</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AddRaw(byte[] dataBytes, string filename)
        {
            AddData(dataBytes, "ADD RAW", filename);
        }

        /// <summary>
        /// Set (or unset) Subject of the current outgoing message.
        /// </summary>
        /// <param name="data">The new subject for the message; null or empty string resets to default</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void SetSubject(string data)
        {
            string response = SendCommand("SUBJECT" +
                    (data != null && data.Length > 0 ? " " + data : ""));
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Set subject failed: " + response);
        }

        /// <summary>
        /// Set (or unset) request for final delivery notification from recipient's system. This will
        /// override the server default.
        /// </summary>
        /// <param name="value">true to set or false to unset the request for notification.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void SetDeliveryNotification(bool value)
        {
            string response = SendCommand("SET FINAL " + (value ? "1" : "0"));
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Set delivery notification failed: " + response);
        }

        /// <summary>
        /// Send current outgoing message.
        /// </summary>
        /// <returns>A List of send results, one entry per message recipient</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public List<SendResult> Send()
        {
            string response = SendCommand("SEND");
            var output = new List<SendResult>();
            if (response.StartsWith("FAIL"))
                throw new InvalidOperationException("send failed " + response);
            while (response != null && !response.Equals("OK"))
            {
                string[] rExplode = response.Split(new char[] { ' ' }, 3);
                switch (rExplode[0])
                {
                    case "ERROR":
                        output.Add(new SendResult(rExplode[1], false, rExplode[2]));
                        break;
                    case "QUEUED":
                        output.Add(new SendResult(rExplode[1], true, rExplode[2]));
                        break;
                    default:
                        //unrecognized
                        throw new InvalidOperationException("Send failed with unexpected response: " + response);
                }
                response = SendCommand("OK");
            }
            return output;
        }

        /// <summary>
        /// Get next message or status update from incoming queue.
        /// </summary>
        /// <returns>First message or status update on queue, or null if none.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public CheckResult Check()
        {
            string response = SendCommand("CHECK");
            if (response.Equals("NONE")) return null;
            if (response.StartsWith("FAIL"))
                throw new InvalidOperationException("Check failed: " + response);
            if (response.StartsWith("STATUS"))
            {
                String[] rExplode = response.Split(new char[] { ' ' }, 4);
                return new CheckResult(rExplode[1], rExplode[2],
                        rExplode.Length == 4 ? rExplode[3] : null);
            }
            else if (response.StartsWith("MAIL"))
            {
                String[] rExplode = response.Split(new char[] { ' ' }, 5);
                int numAttach = Convert.ToInt32(rExplode[3]);
                return new CheckResult(rExplode[1], rExplode[2], numAttach, rExplode[4]);
            }
            else
                throw new InvalidOperationException("Check failed with unexpected response: " + response);
        }

        /// <summary>
        /// Acknowledge status message was received. Server will remove from incoming queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AcknowledgeStatus()
        {
            string response = SendCommand("OK");
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Status acknowledgement failed: " + response);
        }

        /// <summary>
        /// Retry a failed message.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        [Obsolete("Retry is disabled on server side by default.")]
        public void Retry()
        {
            string response = SendCommand("RETRY");
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Retry failed: " + response);
        }

        /// <summary>
        /// Acknowledge incoming message was received. Server will remove from incoming queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public void AcknowledgeMessage()
        {
            string response = SendCommand("DONE");
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Message acknowledgement failed: " + response);
        }

        /// <summary>
        /// Request a content part of an incoming message.
        /// </summary>
        /// <param name="messagePart">The message part to retrieve, 0=main message body, 1..N=attachments</param>
        /// <returns>A ShowResult instance containing the requested content</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        public ShowResult Show(int messagePart)
        {
            string response = SendCommand("SHOW " + messagePart);
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Show " + messagePart + " failed: " + response);
            List<string> headers = null;
            string filename = null;
            if (messagePart == 0)
            {
                //get headers for part==0
                headers = new List<string>();
                while ((response = StreamIn.ReadLine()) != null && response.Length > 0)
                {
                    headers.Add(response);
                }
            }
            else
            {
                //get filename for part>0
                filename = StreamIn.ReadLine();
            }

            string mimeType = StreamIn.ReadLine();
            var length = Convert.ToInt32(StreamIn.ReadLine());
            byte[] buf = new byte[length];

            int bytesRead = 0;
            DateTime timeout = DateTime.Now.AddMilliseconds(PHIMAIL_READ_TIMEOUT);
            while (bytesRead < length)
            {
                int readThisTime = _SslStream.Read(buf, bytesRead, length - bytesRead);
                if (readThisTime > 0)
                {
                    bytesRead += readThisTime;
                    timeout = DateTime.Now.AddMilliseconds(PHIMAIL_READ_TIMEOUT);
                }
                else
                {
                    if (DateTime.Now > timeout)
                    {
                        throw new InvalidOperationException("Show timed out receiving content.");
                    }
                }
            }

            AttachmentInfo[] ai = null;
            if (messagePart == 0)
            {
                //get attachment data
                int numAttach = Convert.ToInt32(StreamIn.ReadLine());
                ai = new AttachmentInfo[numAttach];
                for (int n = 0; n < numAttach; n++)
                {
                    String aFilename = StreamIn.ReadLine();
                    String aMimeType = StreamIn.ReadLine();
                    String aDescription = StreamIn.ReadLine();
                    ai[n] = new AttachmentInfo(aFilename, aMimeType, aDescription);
                }

            }

            return new ShowResult(messagePart, headers, filename, mimeType, length,
                    buf, ai);
        }

        /// <summary>
        /// Search the directory.
        /// </summary>
        /// <param name="searchFilter">The search filter, using LDAP filter syntax</param>
        /// <returns>A List of matching JSON-encoded directory entries</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the server signals a failure.</exception>
        /// 
        public List<string> SearchDirectory(string searchFilter)
        {
            string response = SendCommand("LOOKUP JSON " + searchFilter);
            if (!response.Equals("OK"))
                throw new InvalidOperationException("Directory search failed: " + response);

            int numResults = Convert.ToInt32(StreamIn.ReadLine());
            var searchResults = new List<string>(numResults);
            while (numResults-- > 0) searchResults.Add(StreamIn.ReadLine());
            return searchResults;
        }

        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}