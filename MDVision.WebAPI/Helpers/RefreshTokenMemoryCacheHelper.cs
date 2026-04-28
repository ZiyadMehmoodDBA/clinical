using MDVision.Common.Logging;
using MDVision.WebAPI.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace MDVision.WebAPI.Helpers
{
    public class RefreshTokenMemoryCacheHelper
    {
        MemoryCache _cache;
        const string RegionName = null;
        CacheItemPolicy _cachePolicy;
        public RefreshTokenMemoryCacheHelper()
        {
            _cache = MemoryCache.Default;
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(24);
            //_cachePolicy.SlidingExpiration = TimeSpan.FromHours(3);
        }

        public bool AddRefreshToken(string key, RefreshToken token)
        {
            string serializeToken = JsonConvert.SerializeObject(token);

            string Result = "Key is " + key + "-------Insert Token Request";
            Exception ex = new Exception(Result);

            MDVLogger.SendExcepToDBForMobileAPI(ex, "MobileAPI::InsertToken", "AddRefreshToken");

            return _cache.Add(key, serializeToken, _cachePolicy, RegionName); ;

           
             
        }
        public RefreshToken GetRefreshToken(string key)
        {
            string serializeToken = Convert.ToString(_cache.Get(key, RegionName));
            if (!string.IsNullOrEmpty(serializeToken))
            {
                string Result = "Key is " + key + " ----------Get Request success " ;
                Exception ex = new Exception(Result);

                MDVLogger.SendExcepToDBForMobileAPI(ex, "MobileAPI::GetToken", "GetRefreshToken");
                return JsonConvert.DeserializeObject<RefreshToken>(serializeToken);
            }
            else
            {
                string Result = "Key is " + key + " -Get Request Failed---Key Not Found";
                Exception ex = new Exception(Result);

                MDVLogger.SendExcepToDBForMobileAPI(ex, "MobileAPI::GetToken", "GetRequestFailed");
                return null;
            }
        }
        public object RemoveRefreshToken(string key)
        {
            string Result = "Key is " + key + "----Remove Token Request" ; 
            Exception ex = new Exception(Result);

            MDVLogger.SendExcepToDBForMobileAPI(ex, "MobileAPI::RemoveToken", "RemoveRefreshToken");
            return _cache.Remove(key, RegionName); ;
        }
    }
}