using MDVision.WebAPI.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Sockets;
using MDVision.Business.BLL;
using System.Data;


[assembly: OwinStartup(typeof(MDVision.WebAPI.Startup))]

namespace MDVision.WebAPI
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        //public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        //public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            //  Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Migrations.Configuration>());


            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);


        }
        public static System.Timers.Timer timer = new System.Timers.Timer(60000); // This will raise the event every one minute.
        
         void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Do Your Stuff
            BLLMobileLogin ll = new BLLMobileLogin();

            DataSet ds;
            string DeviceId = "";

            DateTimeOffset  CurrentUTCTime = DateTime.UtcNow;

            try
            {
                ds = new DataSet();
             ds=   new BLLMobileLogin().LoadDeviceIds();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTimeOffset DeviceIdExpirationTime =DateTimeOffset.Parse(Convert.ToString(row[1]));
                    TimeSpan duration =   DeviceIdExpirationTime- CurrentUTCTime;
                    double Minutes = Math.Round(duration.TotalMinutes);

                    if (Minutes == 2 )
                    {
                        DeviceId = Convert.ToString(row[0]);
                        pushMessage(DeviceId);
                    }
                }
            }
            catch (Exception ex)
            {
              //  throw ex;
            }
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
        private static bool ValidateRemoteCertificate(
  object sender,
  X509Certificate certificate,
  X509Chain chain,
  SslPolicyErrors policyErrors)
        {

            return true;
        }
        public void pushMessage(string deviceId)
        {

            int port = 2195;


             //  string certificatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/AuthKey/Developer_Certificates.p12");//test
             // string hostname = "gateway.sandbox.push.apple.com"; // Test


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
                String payload = "{\"aps\":{\"alert\":\"" + "You will be logged out automatically in 2 minutes!" + "\",\"badge\":1,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                
            }
            catch (Exception e)
            {
                client.Close();
             //   return e.ToString();
            }


        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
            
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            ////Configure Google External Login
            //googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "xxxxxx",
            //    ClientSecret = "xxxxxx",
            //    Provider = new GoogleAuthProvider()
            //};
            //app.UseGoogleAuthentication(googleAuthOptions);

            ////Configure Facebook External Login
            //facebookAuthOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "xxxxxx",
            //    AppSecret = "xxxxxx",
            //    Provider = new FacebookAuthProvider()
            //};
            //app.UseFacebookAuthentication(facebookAuthOptions);

        }
              }

}