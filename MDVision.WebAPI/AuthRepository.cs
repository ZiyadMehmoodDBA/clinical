using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Model.Security;
using MDVision.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace MDVision.WebAPI
{

    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;
        BLLCommon BLLCommonObj;

        public AuthRepository()
        {

            BLLCommonObj = new BLLCommon();
            //    objPatientLogin = new DALPatientLogin();
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        //fixme
        public async Task<List<SoftwareCustomerInfoModel>> VerifyUser(string userName, string password)
        {
            List<SoftwareCustomerInfoModel> SoftwareCustomerInfoList = new List<SoftwareCustomerInfoModel>();

            SoftwareCustomerInfoList = BLLCommonObj.GetCustomerSettingsNativeForCheckInApp(userName, password);
            return SoftwareCustomerInfoList;
        }
        public async Task<string> GetIssFullSSN(long userId, string userName, string entityId, string isActive,long SecurityUserId)
        {
            string isfullSSN = "";

            isfullSSN = BLLCommonObj.GetIsFullSSNNative(userId, userName, entityId, isActive,SecurityUserId);
            return isfullSSN;
        }
        public Client FindClient(string clientId)
        {
            Client client = null;
            client = BLLCommonObj.GetClientInformationNative(clientId);

            return client;
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {


        }
    }
}