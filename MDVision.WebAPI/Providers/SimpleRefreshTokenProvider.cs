using MDVision.MDVision.WebAPI;
using MDVision.WebAPI.Entities;
using MDVision.WebAPI.Helpers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MDVision.WebAPI.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        RefreshTokenMemoryCacheHelper _cache; 
        public SimpleRefreshTokenProvider()
        {
            _cache = new RefreshTokenMemoryCacheHelper();
        }
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");
            string refreshTokenIdHashed = Helper.GetHash(refreshTokenId);
            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime"); 
               
                var token = new RefreshToken() 
                { 
                    Id = Helper.GetHash(refreshTokenIdHashed),
                    ClientId = clientid, 
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)) 
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
                
                token.ProtectedTicket = context.SerializeTicket();

                var result = _cache.AddRefreshToken(refreshTokenIdHashed, token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
                LogRefreshToken(refreshTokenId,token.Id,token.ClientId,token.IssuedUtc);

            }
        }
        private void LogRefreshToken(string Token,string Id,string ClientId,DateTime IssuedUTC)
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

                path = path + @"\RefreshTokenPath\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                path = path + "MDVisionWebAPIRefreshTokenLog_" + currentDate + ".txt";
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
                writer.WriteLine("Refresh Token: " + Token);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Hashed_Id: " + Id);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("ClientId:");
                writer.WriteLine(ClientId);
                writer.WriteLine("------------------------------------");
                writer.WriteLine("Issued UTC:");
                writer.WriteLine(IssuedUTC);
                writer.WriteLine("------------------------------------");
               
                writer.Close();

            }
            //catch (Exception ex)
            //{
              

            //    throw  ex.InnerException;

            //}
            catch (Exception)
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            var refreshToken = _cache.GetRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                _cache.RemoveRefreshToken(hashedTokenId);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).Wait();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).Wait();
        }
    }
}