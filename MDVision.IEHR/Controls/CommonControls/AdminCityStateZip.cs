using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class AdminCityStateZip
    {
        private BLLCityStateZip BLLCityStateZipObj = null;
        public AdminCityStateZip()
        {
            BLLCityStateZipObj = new BLLCityStateZip();
        }
        #region Singleton
        private static AdminCityStateZip _obj = null;
        public static AdminCityStateZip Instance()
        {
            if (_obj == null)
                _obj = new AdminCityStateZip();
            return _obj;
        }
        #endregion



        #region City/State and Zip Code support function
        /// <summary>
        /// Gets the state of the city.
        /// </summary>
        /// <param name="zipcode">The zipcode.</param>
        /// <param name="cityname">The cityname.</param>
        /// <param name="statename">The statename.</param>
        /// <returns></returns>
        private string GetCityState(string zipcode, string cityname, string statename)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(zipcode)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("Please select a record first")
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSCityStateZip dsCityStatzip = null;
                    BLObject<DSCityStateZip> obj = new BLLCityStateZip().GetCityState(zipcode, cityname);
                    dsCityStatzip = obj.Data;

                    if (obj.Data != null)
                    {
                        if (dsCityStatzip.Tables[dsCityStatzip.CityState.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsCityStatzip.Tables[dsCityStatzip.CityState.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtCity", MDVUtility.ToStr(dr["City"])},
                                { "txtState", MDVUtility.ToStr(dr["State"])},
                                { "txtZip", MDVUtility.ToStr(dr["Zip"])},
                                { "ddlCountry", MDVUtility.ToStr(dr["Country"])},
                                { "hfCountry", MDVUtility.ToStr(dr["CountryId"])},

                                //{ "txtPayCity", MDVUtility.ToStr(dr["City"])},
                                //{ "txtPayState", MDVUtility.ToStr(dr["State"])},
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CITYSTATE_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                //return "";
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Handle the Commands of City / State.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            switch (cammandAction)
            {
                case "CITYSTATE":
                    {
                        string zipcode = context.Request["zipcode"];
                        string cityname = context.Request["cityname"];
                        string statename = context.Request["statename"];
                        string strJSONData = GetCityState(zipcode, cityname, statename);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}