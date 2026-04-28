using MDVision.WebAPI.Filters;
using MDVision.WebAPI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MDVision.WebAPI.Entities;
using System.Reflection;
using System.ComponentModel;

namespace MDVision.WebAPI.Controllers
{
    [SetSessionProperties]
    [RoutePrefix("api/Lookups")]
    public class APILookupsController : ApiController
    {
        public string IsActive = "true";
        public APILookups apiLookups;
        public APILookupsController()
        {
            apiLookups = new APILookups();
        }

        //[HttpGet]
        //[SetSessionFromHeader]
        //[Route("GetFacility")]
        //public string GetFacility()
        //{
        //    return FormatLookupResponce(apiLookups.GetFacility(IsActive));
        //}

        //[HttpGet]
        //[SetSessionFromHeader]
        //[Route("GetProvider")]
        //public string GetProvider()
        //{
        //    return FormatLookupResponce(apiLookups.GetProvider(IsActive));
        //}


        [HttpGet]
        [SetSessionFromHeader]
        [Route("GetLookups")]
        public string GetLookups(string methods)
        {
            string response = "";
            try
            {
                Dictionary<string, HashSet<NameValuePair>> dic = new Dictionary<string, HashSet<NameValuePair>>();
                string[] lookupMethods = methods.Split(',');

                foreach (string method in lookupMethods)
                {
                    string[] splitMethod = method.Split('?');
                    HashSet<NameValuePair> nameValuePairList = GetLookUpData(splitMethod);
                    dic.Add(splitMethod[0], nameValuePairList);
                }

                response = JsonConvert.SerializeObject(
                             new
                             {
                                 status = true,
                                 message = "",
                                 data = dic
                             });
            }
            catch (Exception ex)
            {
                response = JsonConvert.SerializeObject(
                             new
                             {
                                 status = true,
                                 message = ex.Message,
                                 data = ""
                             });
            }
            return response;
        }

        #region HelperMethod

        public HashSet<NameValuePair> GetLookUpData(string[] splitMethod)
        {
            //DbLookUpTest lookup = new DbLookUpTest();
            Dictionary<string, object> namedParameters = new Dictionary<string, object>();
            if (splitMethod.Length > 1)
            {
                string[] parametersWithValue = splitMethod[1].Split('&');
                foreach (string paramerterWithValue in parametersWithValue)
                {
                    string[] parameterAndValue = paramerterWithValue.Split('=');
                    namedParameters.Add(parameterAndValue[0], parameterAndValue[1]);
                };
            }
            //var methodnamewithassemble = lookup.GetType().FullName + "." + ;
            MethodBase methodbase = apiLookups.GetType().GetMethod(splitMethod[0]);

            object[] parameters = MapParameters(methodbase, namedParameters);
            return (HashSet<NameValuePair>)methodbase.Invoke(apiLookups, parameters);
        }

        public object[] MapParameters(MethodBase methodbase, IDictionary<string, object> namedParameters)
        {
            ParameterInfo[] paramNames = methodbase.GetParameters();//.Select(p => p.Name).ToArray();
            object[] parameters = new object[paramNames.Length];
            //for (int i = 0; i < parameters.Length; ++i) // for optional parameters
            //{
            //    parameters[i] = Type.Missing;
            //}
            foreach (var param in paramNames)
            {
                //var paramName = item.Key;
                //var paramIndex = Array.IndexOf(paramNames, paramName);
                if (namedParameters.ContainsKey(param.Name))
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(param.ParameterType);
                    object propValue = typeConverter.ConvertFromString(Convert.ToString(namedParameters[param.Name]));
                    parameters[param.Position] = propValue;
                }
            }
            return parameters;
        }

        //public string FormatLookupResponce(HashSet<NameValuePair> list)
        //{
        //    if(list.Count > 0)
        //    {
        //        return JsonConvert.SerializeObject(new
        //        {
        //            status = true,
        //            message = "",
        //            data = list

        //        });
        //    }
        //    else
        //    {
        //        return JsonConvert.SerializeObject(new
        //        {
        //            status = false,
        //            message = "No data found.",
        //            data = list

        //        });
        //    }

        //}
        #endregion
    }
}
