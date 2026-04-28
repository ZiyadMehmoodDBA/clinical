using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.BatchFax;
using MDVision.Model.Batch.Fax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MDVision.Business.BLL
{
    public class BLLBatchFax
    {
        private string access_id;
        private string access_pwd;
        private string serverUrl = "https://www.srfax.com/SRF_SecWebSvc.php";

        private bool lastStatus;
        private string lastResponse;

        public BLLBatchFax()
        {
            access_id = WebConfigurationManager.AppSettings["access_id"];
            access_pwd = WebConfigurationManager.AppSettings["access_pwd"];

            //access_id = "77137";
            //access_pwd = "MDVision1";

        }

        #region "Fax Service Methods"
        public string Queue_Fax(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            if (!string.IsNullOrEmpty(MDVApplication.FaxNotifyURL))
            {
                parameters["sNotifyURL"] = MDVApplication.FaxNotifyURL;
            }
            Regex rgx = new Regex(@"(\s?)(\(?)(\)?)(\-?)");

            string[] requiredFields = { "sCallerID", "sSenderEmail", "sFaxType", "sToFaxNumber" };
            string[] optionalFields = {"sResponseFormat", "sAccountCode", "sRetries", "sCoverPage", "sCPFromName", "sCPToName", "sCPOrganization",
                                   "sCPSubject", "sCPComments", "sFileName_*", "sFileContent_*", "sNotifyURL", "sFaxFromHeader", "sQueueFaxDate", "sQueueFaxTime" };

            parameters["sSenderEmail"] = "no_reply@sovms.com";
            parameters["sFaxType"] = "SINGLE";
            parameters["sCallerID"] = rgx.Replace(parameters["sCallerID"], "");




            var toFaxNumber = parameters["sToFaxNumber"].Split('|');
            var sentToName = parameters["SentToName"].Split('|');
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (var item in toFaxNumber)
            {
                Dictionary<string, string> input = new Dictionary<string, string>(parameters);

                input["sToFaxNumber"] = item;
                _validateRequiredVariables(requiredFields, input);

                Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, input);

                postVariables.Add("action", "Queue_Fax");
                postVariables.Add("access_id", access_id);
                postVariables.Add("access_pwd", access_pwd);


                tasks.Add(Task.Factory.StartNew<string>(() => _processRequest(postVariables)));
            }
            Task.WaitAll(tasks.ToArray());
            int i = 0;
            foreach (Task<string> t in tasks)
            {
                parameters["SentToName"] = sentToName[i];
                parameters["sToFaxNumber"] = toFaxNumber[i];
                QueueFaxFromDB(parameters, t.Result);
                i++;
            }
            return tasks[0].Result;

        }

        public string Get_FaxStatus(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sFaxDetailsID" };
            string[] optionalFields = { "sResponseFormat" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Get_FaxStatus");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            //_processResponse(result);

            return result;
        }

        public string Get_MultiFaxStatus(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sFaxDetailsID" };
            string[] optionalFields = { "sResponseFormat" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Get_MultiFaxStatus");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            _processResponse(result);

            return result;
        }


        public string Get_Fax_Inbox(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { };
            string[] optionalFields = { "sResponseFormat", "sPeriod", "sStartDate", "sEndDate", "sViewedStatus", "sIncludeSubUsers", "sFaxDetailsID" };


            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Get_Fax_Inbox");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            //_processResponse(result);

            return result;

        }

        public string Get_Fax_Outbox(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { };
            string[] optionalFields = { "sResponseFormat", "sPeriod", "sStartDate", "sEndDate", "sIncludeSubUsers", "sFaxDetailsID" };


            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Get_Fax_Outbox");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            //_processResponse(result);

            return result;

        }

        public string Retrieve_Fax(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sFaxFileName|sFaxDetailsID", "sDirection" };
            string[] optionalFields = { "sFaxFormat", "sMarkasViewed", "sResponseFormat", "sSubUserID" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Retrieve_Fax");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            _processResponse(result);

            return result;
        }

        public bool Update_Viewed_Status(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sFaxFileName|sFaxDetailsID", "sDirection", "sMarkasViewed" };
            string[] optionalFields = { "sResponseFormat" };

            parameters["sMarkasViewed"] = "Y";

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Update_Viewed_Status");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            _processResponse(result);

            return lastStatus;

        }

        public string Delete_Fax(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sDirection", "sFaxFileName_*|sFaxDetailsID_*" };
            string[] optionalFields = { "sResponseFormat" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Delete_Fax");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            //_processResponse(result);
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var JSONObj = ser.Deserialize<dynamic>(result);
            if (JSONObj.ContainsKey("Status") && JSONObj["Status"] == "Success")
                DeleteFaxFromDB(parameters["sFaxDetailsID_1"]);

            return result;

        }

        public bool Stop_Fax(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { "sFaxDetailsID" };
            string[] optionalFields = { "sResponseFormat" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Stop_Fax");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            _processResponse(result);

            return lastStatus;

        }

        public bool Get_Fax_Usage(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string[] requiredFields = { };
            string[] optionalFields = { "sResponseFormat", "sPeriod", "sStartDate", "sEndDate", "sIncludeSubUsers" };

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Get_Fax_Usage");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            _processResponse(result);

            return lastStatus;

        }

        public string Get_Last_Response()
        {
            return lastResponse;
        }

        public bool Get_Last_Status()
        {
            return lastStatus;
        }

        public string Forward_Fax(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            if (!string.IsNullOrEmpty(MDVApplication.FaxNotifyURL))
            {
                parameters["sNotifyURL"] = MDVApplication.FaxNotifyURL;
            }
            List<FaxOutboxDetailModel> obj = new DALBatchFax().LoadFaxOutboxDetail(MDVUtility.ToInt64(parameters["sFaxDetailsID"]), "", "", 0, 0);

            Regex rgx = new Regex(@"(\s?)(\(?)(\)?)(\-?)");

            string[] requiredFields = { "sCallerID", "sSenderEmail", "sFaxType", "sToFaxNumber", "sDirection", "sFaxDetailsID" };
            string[] optionalFields = { "sResponseFormat", "sAccountCode", "sRetries", "sNotifyURL", "sFaxFromHeader", "sQueueFaxDate", "sQueueFaxTime" };

            parameters["sSenderEmail"] = "no_reply@sovms.com";
            parameters["sFaxType"] = "SINGLE";
            parameters["sCallerID"] = rgx.Replace(obj[0].CallerID, "");
            parameters["sDirection"] = "OUT";
            parameters["sToFaxNumber"] = obj[0].ToFaxNumber;

            _validateRequiredVariables(requiredFields, parameters);

            Dictionary<string, string> postVariables = _preparePostVariables(requiredFields, optionalFields, parameters);

            postVariables.Add("action", "Forward_Fax");
            postVariables.Add("access_id", access_id);
            postVariables.Add("access_pwd", access_pwd);

            string result = _processRequest(postVariables);

            //_processResponse(result);
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var JSONObj = ser.Deserialize<dynamic>(result);
            if (JSONObj.ContainsKey("Status") && JSONObj["Status"] == "Success")
            {
                parameters["ProviderId"] = obj[0].ProviderId;
                parameters["SentToName"] = obj[0].SentToName;
                parameters["sCPSubject"] = obj[0].Subject;
                QueueFaxFromDB(parameters, result);
                InactiveFaxFromDB(parameters["sFaxDetailsID"]);
            }
            return result;

        }

        #endregion

        #region "Private Methods"
        private void _processResponse(string response)
        {
            Console.WriteLine(response);

            if (response.IndexOf("Success") != -1)
            {
                lastStatus = true;
            }
            else
            {
                lastStatus = false;
            }

            lastResponse = response;

            return;

        }

        private string _processRequest(Dictionary<string, string> postVariables)
        {
            string queryString = _prepareQueryString(postVariables);
            string result = "";
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(serverUrl, "POST", queryString);
            }

            return result;
        }


        private string _prepareQueryString(Dictionary<string, string> postVariables)
        {
            string queryString = "";

            foreach (KeyValuePair<string, string> entry in postVariables)
            {
                queryString += entry.Key.ToString() + "=" + UrlEncode(entry.Value.ToString()) + "&";
            }

            // remove last &
            queryString = queryString.Remove(queryString.Length - 1);

            return queryString;

        }

        public static string UrlEncode(string str)

        {

            StringBuilder sb = new StringBuilder();

            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);

            for (int i = 0; i < byStr.Length; i++)

            {

                sb.Append(@"%" + byStr[i].ToString("X2"));

            }

            return (sb.ToString());

        }

        private Dictionary<string, string> _preparePostVariables(string[] requiredFields, string[] optionalFields, Dictionary<string, string> parameters)
        {
            Dictionary<string, string> postVariables = new Dictionary<string, string>();

            List<string> list = new List<string>();
            list.AddRange(requiredFields);
            list.AddRange(optionalFields);

            string[] inputVariables = list.ToArray();


            foreach (string field in inputVariables)
            {
                if (field.EndsWith("*") && field.IndexOf('|') == -1) // non-piped wildcard
                {
                    string fieldPrefix = field.Replace("*", "");
                    Dictionary<string, string> wildCards = _getWildcardVaribles(fieldPrefix, parameters);
                    postVariables = _mergeDictionaries(postVariables, wildCards);

                }
                else
                {
                    if (field.IndexOf('|') != -1) // piped, non-wildcard
                    {
                        string[] pipedFields = field.Split('|');

                        foreach (string pipedField in pipedFields)
                        {
                            if (pipedField.EndsWith("*")) // piped wildcard
                            {
                                string fieldPrefix = pipedField.Replace("*", "");
                                Dictionary<string, string> wildCards = _getWildcardVaribles(fieldPrefix, parameters);
                                postVariables = _mergeDictionaries(postVariables, wildCards);
                            }
                            else
                            {
                                if (parameters.ContainsKey(pipedField))
                                {
                                    string value = parameters[pipedField];
                                    if (value.Length > 0)
                                    {
                                        postVariables.Add(pipedField, value);
                                    }
                                }
                            }
                        }


                    }
                    else //non-special fieldname
                    {
                        if (parameters.ContainsKey(field))
                        {
                            postVariables.Add(field, parameters[field]);
                        }
                    }
                }

            }


            return postVariables;
        }


        private Dictionary<string, string> _getWildcardVaribles(string fieldPrefix, Dictionary<string, string> parameters)
        {
            Dictionary<string, string> wildCards = new Dictionary<string, string>();
            bool done = false;
            int suffix = 1;

            while (!done)
            {
                string field = fieldPrefix + suffix;

                if (parameters.ContainsKey(field))
                {
                    string value = parameters[field];

                    if (value.Length > 0) // add variable to the collection
                    {
                        wildCards.Add(field, value);
                    }
                    else // field value is empty, so finish
                    {
                        done = true;
                    }
                }
                else
                {
                    done = true;
                }

                suffix++;

                // fail safe to ensure no infinite loops
                if (suffix > 1000)
                {
                    done = true;
                }

            }

            return wildCards;

        }

        private void _validateRequiredVariables(string[] requiredVariables, Dictionary<string, string> parameters)
        {

            foreach (string field in requiredVariables)
            {

                string error = "";

                if (field.EndsWith("*") && field.IndexOf("|") == -1) // non piped wildcard variable.  check for first instance
                {
                    string fieldPrefix = field.Replace("*", "");
                    string wildCard = fieldPrefix + "1";

                    if (!parameters.ContainsKey(wildCard))
                    {
                        error = "Required Field missing.  No values for " + fieldPrefix;
                    }
                    else
                    {
                        string value = parameters[wildCard];
                        if (value.Length <= 0)
                        {
                            error = "Required Field missing.  No values for " + fieldPrefix;
                        }
                    }


                }
                else
                {

                    if (field.IndexOf("|") != -1) // piped separated variable.  At lease 1 must be present.
                    {
                        string[] pipedFields = field.Split('|');
                        bool checkSuccessful = false;

                        foreach (string pipedField in pipedFields)
                        {
                            string trimmedPipedField = pipedField.Trim();

                            if (trimmedPipedField.EndsWith("*")) // piped value has a wildcard, look for first value
                            {
                                string prefix = trimmedPipedField.Replace("*", "");
                                string wildcard = prefix + "1";

                                if (parameters.ContainsKey(wildcard)) // parameter exists, check to make sure it has a value
                                {
                                    string pVal = parameters[wildcard];
                                    if (pVal.Length > 0)
                                    {
                                        checkSuccessful = true;
                                    }

                                }

                            }
                            else
                            {
                                if (parameters.ContainsKey(trimmedPipedField))
                                {
                                    string pVal = parameters[trimmedPipedField];
                                    if (pVal.Length > 0)
                                    {
                                        checkSuccessful = true;
                                    }
                                }
                            }
                        }

                        if (!checkSuccessful)
                        {

                            error = "Required field missing.  You must provide at lease 1 of the following: " + string.Join(",", pipedFields);
                        }

                    }
                    else // standard field, check if it exists
                    {

                        if (!parameters.ContainsKey(field))
                        {
                            error = "Required field " + field + " is missing!";
                        }
                        else // ensure field value is not empty
                        {
                            string value = parameters[field];
                            if (value.Length <= 0)
                            {
                                error = "Required field " + field + " is missing!";
                            }
                        }

                    }
                }


                if (error.Length > 0)
                {
                    throw (new Exception(error));
                    return;
                }
            }



            return;
        }

        // merges 2 dictionaries into one, stops duplicates
        private Dictionary<string, string> _mergeDictionaries(Dictionary<string, string> d1, Dictionary<string, string> d2)
        {
            Dictionary<string, string> mergedDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> entry in d1)
            {
                if (!mergedDictionary.ContainsKey(entry.Key))
                {
                    mergedDictionary.Add(entry.Key, entry.Value);
                }
            }

            foreach (KeyValuePair<string, string> entry in d2)
            {
                if (!mergedDictionary.ContainsKey(entry.Key))
                {
                    mergedDictionary.Add(entry.Key, entry.Value);
                }
            }

            return mergedDictionary;
        }

        private void Get_MultiFaxStatus(List<FaxOutboxDetailModel> list)
        {
            var filteredList = list.Where(x => x.SentStatus == "In Progress").Select(x => x.FaxDetailsID).ToArray();
            if (filteredList.Length > 0)
            {
                var array = string.Join("|", filteredList);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("sFaxDetailsID", array);
                var result = Get_MultiFaxStatus(parameters);
                if (result.IndexOf("Success") != -1)
                {
                    DALBatchFax dalBatchFax = new DALBatchFax();
                    JObject jobj = JObject.Parse(result);
                    List<FaxOutboxDetailModel> models = JsonConvert.DeserializeObject<List<FaxOutboxDetailModel>>(Convert.ToString(jobj["Result"]));
                    foreach (var item in models)
                    {
                        FaxOutboxDetailModel faxDetail = list.Where(x => x.FaxDetailsID == item.FaxDetailsID).FirstOrDefault();
                        item.PatientId = faxDetail.PatientId;
                        item.FaxDetailsID = item.FileName.Split('|')[1];
                        item.ModifiedBy = "FaxNotify";
                        item.ModifiedOn = DateTime.Now.ToString();  
                                              
                        dalBatchFax.UpdateFaxOutboxDetail(item);
                    }
                }
            }
        }

        #endregion

        #region "DB Methods"

        private void QueueFaxFromDB(Dictionary<string, string> parameters, string result)
        {
            FaxOutboxDetailModel model = new FaxOutboxDetailModel();
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var JSONObj = ser.Deserialize<dynamic>(result);
            if (JSONObj.ContainsKey("Status") && JSONObj["Status"] == "Success" && JSONObj.ContainsKey("Result"))
            {
                model.CallerID = MDVUtility.ToStr(parameters["sCallerID"]);
                model.FaxDetailsID = MDVUtility.ToStr(JSONObj["Result"]);
                model.ProviderId = MDVUtility.ToStr(parameters["ProviderId"]);
                model.SentStatus = "In Progress";
                model.SentToName = MDVUtility.ToStr(parameters["SentToName"]);
                model.Subject = MDVUtility.ToStr(parameters["sCPSubject"]);
                model.ToFaxNumber = MDVUtility.ToStr(parameters["sToFaxNumber"]);
                model.UserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                model.PatientId = MDVUtility.ToStr(parameters["PatientId"]);
                new DALBatchFax().SaveFaxOutboxDetail(model);
            }
        }

        public string GetFaxOutboxFromDB(Dictionary<string, string> parameters)
        {
            try
            {
                int PageNumber = MDVUtility.ToInt32(parameters["PageNumber"]);
                int ResultPerPage = MDVUtility.ToInt32(parameters["ResultPerPage"]);
                string StartDate = "", EndDate = "";
                long UserId = 0, ProviderId = 0;
                if (parameters.ContainsKey("sStartDate") && !string.IsNullOrEmpty(parameters["sStartDate"]))
                {
                    StartDate = MDVUtility.ToStr(parameters["sStartDate"]);
                }
                if (parameters.ContainsKey("sEndDate") && !string.IsNullOrEmpty(parameters["sEndDate"]))
                {
                    EndDate = MDVUtility.ToStr(parameters["sEndDate"]);
                }
                if (parameters.ContainsKey("ProviderId") && !string.IsNullOrEmpty(parameters["ProviderId"]))
                {
                    ProviderId = MDVUtility.ToLong(parameters["ProviderId"]);
                }

                UserId = MDVSession.Current.AppUserId;

                var obj = new DALBatchFax().LoadFaxOutboxDetail(0, StartDate, EndDate, PageNumber, ResultPerPage, UserId,ProviderId);
                Get_MultiFaxStatus(obj);
                if (obj.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        Result = obj,
                        Batch_FaxOutboxCount = obj.Count,
                        iTotalDisplayRecords = obj[0].RecordCount,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Batch_FaxOutboxCount = 0,
                        iTotalDisplayRecords = 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private void DeleteFaxFromDB(string FaxDetailsID)
        {
            new DALBatchFax().DeleteFaxOutboxDetail(FaxDetailsID);
        }

        private void InactiveFaxFromDB(string FaxDetailsID)
        {
            new DALBatchFax().InactiveFaxOutboxDetail(FaxDetailsID);
        }

        #endregion
    }
}
