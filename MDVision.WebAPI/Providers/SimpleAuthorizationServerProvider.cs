using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.MDVision.WebAPI;
using MDVision.Model.Common;
using MDVision.Model.Security;
using MDVision.WebAPI.Entities;
using MDVision.WebAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using MDVision.Business.BLL;
using MDVision.DataAccess.DCommon;

using System.Web.Http;

namespace MDVision.WebAPI.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    { 
        private List<SoftwareCustomerInfoModel> UserLoginData =null;
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string deviceId = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                context.SetError("Missing Client Id", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (AuthRepository _repo = new AuthRepository())
            {
                client = _repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("Invalid Client Id", "Client Id  is not registered in the system.");
                return Task.FromResult<object>(null);
            }

            if (client.appType == Constants.ApplicationType.NATIVE)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("Missing Client Secret", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != MDVUtility.EncryptToSHA256(clientSecret, clientId))
                    {
                        context.SetError("Invalid Client Secret", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("InActive Client", "Client is InActive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", MDVUtility.ToStr(client.RefreshTokenLifeTime));

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            List<SoftwareCustomerInfoModel> softwareCustomerInfoList = new List<SoftwareCustomerInfoModel>();


            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            using (AuthRepository _repo = new AuthRepository())
            {
                string userName = "";
                string userPassword = "";
                try
                {
                    userName = MDVUtility.DecryptFrom64(context.UserName);
                    userPassword = MDVUtility.EncryptToSHA256(MDVUtility.DecryptFrom64(context.Password), userName);
                }
                catch (Exception)
                {
                    context.SetError("Unauthenticated", "incorrect Username or Password");
                    return;
                }
                softwareCustomerInfoList = await _repo.VerifyUser(userName, userPassword);
                if (softwareCustomerInfoList.Count == 0)
                {
                    context.SetError("Unauthenticated", "incorrect Username or Password");
                    return;
                }
                else
                {
                    //here is place for Login rights code
                    var FirstItem = softwareCustomerInfoList.First();
                    if (FirstItem.IsMobileLogin == "False")
                    {
                        context.SetError("Unauthorized", "User Is Unauthorized");
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                        return;
                    }
                    //context.Options.AccessTokenExpireTimeSpan = new TimeSpan(0, int.Parse(FirstItem.MobSessionExpTime), 0);
                    context.Options.AccessTokenExpireTimeSpan = new TimeSpan(0, 20160, 0);
                    UserLoginData = softwareCustomerInfoList;

                    //string IsFullSSN = await _repo.GetIssFullSSN(0, context.UserName, FirstItem.EntityId, "1", MDVUtility.ToInt64(FirstItem.UserId));
                    //foreach (var item in softwareCustomerInfoList)
                    //{
                    //    item.IsFullSSN = IsFullSSN;
                    //}
                    DataAccess.DAL.Admin.DALCustomers.Instance.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, "Mobile App Login", true, false, context.UserName, "Log in user");
                }
            }
            var deviceId = "";
            Microsoft.Owin.IFormCollection parameters = await context.Request.ReadFormAsync();

            if (parameters != null)
            {
                deviceId = parameters.Get("device_Id");

                // DeviceId set as optional by Faizan Ameen on Request of Usman Javed on 18/10/2017

                //if (string.IsNullOrEmpty(deviceId))
                //{
                //    context.SetError("invalid_grant", "parameter 'device_Id' is required");
                //    return;
                //}

            }

            List<Model.User.EntityUserOptions> Settings = new BLLMobileLogin().LoadEntityUserOptionForService(context.UserName, null, null);

            var userDefaultSettings = (from u in Settings
                                       select new
                                       {
                                           ProviderId = u.ProviderId,
                                           ProviderName = u.ProviderName,
                                           FacilityId = u.FacilityId,
                                           FacilityName = u.FacilityName
                                       }).FirstOrDefault();
            //get desired parameter

            //check if it was send

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
               // ,
                    //{
                    //    "UserLoginData", JsonConvert.SerializeObject(softwareCustomerInfoList)  
                    //}
                    ,
                     {
                    "userDefaultSettings",JsonConvert.SerializeObject(userDefaultSettings)
                    }


                });
            
            var ticket = new AuthenticationTicket(identity, props);
            // zia
            context.Validated(ticket);

        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {


            string ExpireTime = "";
            var deviceId = "";
            var grantType = "";

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);

                if (property.Key == ".expires")
                {
                    ExpireTime = property.Value;
                }
            }

            context.AdditionalResponseParameters.Add("UserLoginData", JsonConvert.SerializeObject(UserLoginData));

            deviceId = context.TokenEndpointRequest.Parameters["device_Id"];
            grantType = context.TokenEndpointRequest.Parameters["grant_type"];

            // DeviceId insertion set as optional by Faizan Ameen on Request of Usman Javed on 18/10/2017
            if (!string.IsNullOrEmpty(deviceId))
                InsertUpdateRecordInDataBase(grantType, deviceId, ExpireTime);

            return Task.FromResult<object>(null);
        }

        BLLMobileLogin BLLMobileLoginObj = new BLLMobileLogin();

        public void InsertUpdateRecordInDataBase(string grantType, string DeviceId, string ExpirationTime)
        {
            try
            {
                DateTimeOffset expiryTime = DateTimeOffset.Parse(ExpirationTime);
                BLLMobileLoginObj.InsertUpdateMobileLogin(DeviceId, expiryTime, grantType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}