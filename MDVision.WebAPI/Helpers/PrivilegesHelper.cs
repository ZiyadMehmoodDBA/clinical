using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Jose;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Net;

using System.Timers;

namespace MDVision.WebAPI.Helpers
{
    public class PrivilegesHelper
    {
        private BLLAdminSecurity bllAdminSecurity;
        BLLMobileLogin BLLMobileLoginObj = new BLLMobileLogin();
        public PrivilegesHelper()
        {
            bllAdminSecurity = new BLLAdminSecurity();
        }

        public ResponseObject GetUserPrivileges()
        {
            try
            {
                var userPrivileges = bllAdminSecurity.LoadUserPrivilegesNative(MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                return new ResponseObject { status = true, message = "", data = userPrivileges };
            }
            catch (Exception ex)
            {
                return new ResponseObject { status = false, message = ex.Message };
            }
        }
        //  public  CngKey GetPrivateKey()
        //public string GetProviderToken()
        //{
        //    string Path = "~/AuthKey/AuthKey_98U3FY2885.p8";
        //    Path = System.Web.HttpContext.Current.Server.MapPath(Path);
        //    string KeyId = "98U3FY2885";
        //    string TeamId = "XB9GY3D9M7";

        //    //using (var reader = File.OpenText(Path))
        //    //{
        //    //    var ecPrivateKeyParameters = (ECPrivateKeyParameters)new PemReader(reader).ReadObject();
        //    //    var x = ecPrivateKeyParameters.Parameters.G.GetEncoded();
        //    //    var y = ecPrivateKeyParameters.Parameters.G.GetEncoded();
        //    //    var d = ecPrivateKeyParameters.D.ToByteArrayUnsigned();
        //    //    return EccKey.New(x, y, d);

        //    //}

        //    var privateKeyContent = System.IO.File.ReadAllText(Path);
        //    var privateKey = privateKeyContent.Split('\n')[1];


        //    var secretKeyFile = Convert.FromBase64String(privateKey);
        //    var secretKey = CngKey.Import(secretKeyFile, CngKeyBlobFormat.Pkcs8PrivateBlob);


        //    var expiration = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    var expirationSeconds = (long)expiration.TotalSeconds;


        //    var payload = new Dictionary<string, object>()
        //    {
        //      { "iss", TeamId },
        //      { "iat", expirationSeconds }
        //    };
        //    var header = new Dictionary<string, object>()
        //    {
        //        { "alg", "ES256"},
        //      { "kid", KeyId }
        //    };


        //    string accessToken = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.ES256, header);
        //    return accessToken;
        //}

        
        private static  bool ValidateRemoteCertificate(
   object sender,
   X509Certificate certificate,
   X509Chain chain,
   SslPolicyErrors policyErrors)
        {

            return true;
        }
       
        private byte[] HexStringToByteArray(string hexString)
        {
            if (hexString == null)
                return null;

            if (hexString.Length % 2 == 1)
                hexString = '0' + hexString; // Up to you whether to pad the first or last byte

            byte[] data = new byte[hexString.Length / 2];

            for (int i = 0; i < data.Length; i++)
                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return data;
        }
        public string Logout(string deviceId)
        {
          
            try
            {

              string Result=  BLLMobileLoginObj.LogOutMobileLogin(deviceId);

                if (Result=="")
                {
                    var response = new
                    {
                        status = true,
                        Message = "User Logout Successfully",
                        
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else 
                {
                    var response = new
                    {
                        status = false,
                        Message = "No User Found"
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
           
        }


        public  string pushMessage(string deviceId)
        {
        
            int port = 2195;


         //   string certificatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/AuthKey/Developer_Certificates.p12");//test
         //   string hostname = "gateway.sandbox.push.apple.com"; // Test


            string certificatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/AuthKey/Certificates.p12");//real
            string hostname = "gateway.push.apple.com";// Real.


            string certificatePassword = "";
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath));
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateRemoteCertificate), null);

            try
            {
              //  = SecurityProtocolType.Ssl3 | SslProtocols.Ssl2 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, true);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);
                //   deviceId = "0CC521AD53CCA5221CCBA93363A5FEC4D783A6D5E3558D9306F1EE2553417819";
              //  String deviceId = "FD0307DC7C5D3054FD0D78E116F741C3F09C3A294B3780529B226778FF46DF75";
                
                //   var secretKeyFile = Convert.FromBase64String(deviceId);
                //  writer.Write(ToByteArray(deviceId.ToUpper()));
                byte[] myByte = HexStringToByteArray(deviceId.ToUpper());
                writer.Write(myByte);
                String payload = "{\"aps\":{\"alert\":\"" + "Hi,, This Is a Sample Push Notification For IPhone.." + "\",\"badge\":1,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                return "done";
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                return ex.ToString();
            }
            catch (Exception e)
            {
                client.Close();
                return e.ToString();
            }

           
        }
       
        //    public string GetProviderToken()
        //    {
        //        string KeyId = "98U3FY2885";
        //        string TeamId = "XB9GY3D9M7";
        //        var expiration = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //       var expirationSeconds = (long)expiration.TotalSeconds;
        //      //  var epochNow = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //        var payload = new Dictionary<string, object>()
        //{
        //    {"iss", TeamId},
        //    {"iat", expirationSeconds}
        //};
        //        var extraHeaders = new Dictionary<string, object>()
        //{
        //    {"kid", KeyId}
        //};
        //        var privateKey = GetPrivateKey();
        //        return Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.ES256, extraHeaders);
        //    }
    }
}