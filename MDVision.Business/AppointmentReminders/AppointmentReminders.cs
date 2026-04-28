using MDVision.Business.AppointmentReminders.Response;
using MDVision.Business.AppointmentReminders.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Model.Admin.Reminder;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Common.Shared;

namespace MDVision.Business.AppointmentReminders
{
    public class AppointmentReminder
    {
        #region " Text Reminders "

        /// <summary>
        /// Send Text Reminder to a Patient
        /// </summary>
        /// <param name="TextId"></param>
        /// <param name="Number">  10-digit number to deliver call to. Must be exactly 10 digits</param>
        /// <param name="Message"></param>
        /// <param name="DeliveryDate"> Delivery timestamp. 24-hour time, including UTC offset. Format: YYYY-MM-DD HH:MM-ZZ </param>
        /// <param name="CalleName"></param>
        /// <returns></returns>
        public response SendTextReminder(long TextId, string Number, string Message, DateTime DeliveryDate, long ProviderId,string CalleeName, SharedVariable SharedVariable = null, string Prefix = "SMS")
        {
            ReminderSetting model = null;
            if (SharedVariable != null)
                model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, ProviderId);
            else
                model = new DALReminders().loadReminderSetting(ProviderId);

            if (model != null)
            {
                //YYYY-MM-DD HH:MM-ZZ
                string RequestXML = "";
                request request_text = new request(model.APIKey);
                request_text.text.id = Prefix + TextId;
                request_text.text.delivery = String.Format("{0:yyyy-MM-dd HH:mm}", DeliveryDate) + model.TimeZone;
                request_text.text.message = Message;// + ReminderConfiguration.TextConfirmMessage; + ReminderConfiguration.TextCancelMessage;
                request_text.text.number = MDVUtility.FormatPhoneNumber(Number);
                request_text.text.name = CalleeName;

                RequestXML = GetRequestXml(typeof(request), request_text);
                RequestXML = AddAttributes(RequestXML, "text", "action", "create");
                RequestXML = RemoveProperty(RequestXML, "status,call,fax,preference,error,email");
                response response_text = SendRequest(RequestXML);

                if (string.IsNullOrEmpty(GetAllErrors(response_text.errors.error)))
                    return response_text;
                else
                    throw new Exception(GetAllErrors(response_text.errors.error));
            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }
        }

        /// <summary>
        /// Get follow up response of a Text
        /// </summary>
        /// <param name="TextId"></param>
        /// <returns></returns>
        public response GetTextReminderResponse(long TextId, long ProviderId)
        {
            ReminderSetting model = new DALReminders().loadReminderSetting(ProviderId);
            if (model != null)
            {
                string RequestXML = "";
                request request_text_status = new request(model.APIKey);
                request_text_status.status.text.Add("SMS" + TextId);

                RequestXML = GetRequestXml(typeof(request), request_text_status);
                RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");

                response response_text = SendRequest(RequestXML);
                if (string.IsNullOrEmpty(GetAllErrors(response_text.errors.error)))
                    return response_text;
                else
                    throw new Exception("GetTextReminderResponse::" + GetAllErrors(response_text.errors.error));
            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }
        }

        /// <summary>
        /// Get follow up response of multiple Text's
        /// </summary>
        /// <param name="TextReminderIDs">List of Text Id's</param>
        /// <returns></returns>
        public response GetTextReminderResponse(SharedVariable SharedVariable, List<RemidnerProviderModel> reminder_list)
        {
            response response_ = new response();
            foreach (var item in reminder_list)
            {
                try
                {
                    ReminderSetting model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, item.ProviderId);
                    if (model != null)
                    {
                        string RequestXML = "";
                        request request_text_status = new request(model.APIKey);
                        foreach (var TextId in item.ReminderIds)
                        {
                            request_text_status.status.text.Add("SMS" + TextId);
                        }

                        RequestXML = GetRequestXml(typeof(request), request_text_status);
                        RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");
                        response response_text = SendRequest(RequestXML);
                        if (string.IsNullOrEmpty(GetAllErrors(response_text.errors.error)))
                            response_.status.text.AddRange(response_text.status.text.ToList());
                        else
                            throw new Exception("GetTextReminderResponse::" + GetAllErrors(response_text.errors.error));
                    }
                    else
                    {
                        throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
                    }
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "AppointmentReminder::GetTextReminderResponse::Windows Service", ex);
                }
            }

            return response_;
        }

        #endregion

        #region " Call Reminders "
        /// <summary>
        /// Send Call Reminder to a Patient
        /// </summary>
        /// <param name="CallId"></param>
        /// <param name="DeliveryDate">Delivery timestamp. 24-hour time, including UTC offset. Format: YYYY-MM-DD HH:MM-ZZ</param>
        /// <param name="Number"></param>
        /// <param name="Message"></param>
        /// <param name="CalleName"></param>
        /// <param name="CallerId"></param>
        /// <param name="Retries">0-3. Number of times an unanswered call should be retried before marking as "failed."</param>
        /// <param name="Priority">0-2. Used for prioritizing calls within your own queue</param>
        /// <returns></returns>
        public response SendCallReminder(long CallId, DateTime DeliveryDate, string Number, string Message, long ProviderId,string CalleeName, SharedVariable SharedVariable = null, string CallerId = null, int Retries = 0, int Priority = 0)
        {
            ReminderSetting model = null;
            if (SharedVariable != null)
                model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, ProviderId);
            else
                model = new DALReminders().loadReminderSetting(ProviderId);

            if (model != null)
            {
                Number = MDVUtility.FormatPhoneNumber(Number);
                if (!string.IsNullOrEmpty(Number))
                {
                    string RequestXML = "";
                    request request_call = new request(model.APIKey);
                    request_call.call.id = "CALL" + CallId;
                    request_call.call.delivery = String.Format("{0:yyyy-MM-dd HH:mm}", DeliveryDate) + model.TimeZone;
                    request_call.call.message = Message; //+ReminderConfiguration.CallConfirmMessage; + ReminderConfiguration.CallCancelMessage;
                    request_call.call.number = MDVUtility.FormatPhoneNumber(Number);
                    request_call.call.name = CallerId;
                    request_call.call.callerid = CalleeName;
                    if (Retries > 0)
                        request_call.call.retries = Retries.ToString();
                    if (Priority > 0)
                        request_call.call.priority = Priority.ToString();

                    RequestXML = GetRequestXml(typeof(request), request_call);
                    RequestXML = AddAttributes(RequestXML, "call", "action", "create");
                    RequestXML = RemoveProperty(RequestXML, "status,text,fax,preference,error,email");
                    response response_call = SendRequest(RequestXML);

                    if (string.IsNullOrEmpty(GetAllErrors(response_call.errors.error)))
                        return response_call;
                    else
                        throw new Exception(GetAllErrors(response_call.errors.error));
                }
                else
                {
                    throw new Exception("Phone Number is not well formatted.");
                }
            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }


        }

        /// <summary>
        /// Get follow up response of a call
        /// </summary>
        /// <param name="CallId"></param>
        /// <returns></returns>
        public response GetCallReminderResponse(long CallId, long ProviderId)
        {
            ReminderSetting model = new DALReminders().loadReminderSetting(ProviderId);
            if (model != null)
            {
                string RequestXML = "";
                request request_call_status = new request(model.APIKey);
                request_call_status.status.call.Add("CALL" + CallId);

                RequestXML = GetRequestXml(typeof(request), request_call_status);
                RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");

                response response_call = SendRequest(RequestXML);
                if (string.IsNullOrEmpty(GetAllErrors(response_call.errors.error)))
                    return response_call;
                else
                    throw new Exception("GetCallReminderResponse::" + GetAllErrors(response_call.errors.error));
            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }
        }

        /// <summary>
        /// Get follow up response of multiple calls
        /// </summary>
        /// <param name="CallReminderIDs">List of Call id's</param>
        /// <returns></returns>
        public response GetCallReminderResponse(SharedVariable SharedVariable, List<RemidnerProviderModel> reminder_list)
        {
            response response_ = new response();
            foreach (var item in reminder_list)
            {
                try
                {
                    ReminderSetting model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, item.ProviderId);
                    if (model != null)
                    {
                        string RequestXML = "";
                        request request_call_status = new request(model.APIKey);
                        foreach (var CallId in item.ReminderIds)
                        {
                            request_call_status.status.call.Add("CALL" + CallId);
                        }

                        RequestXML = GetRequestXml(typeof(request), request_call_status);
                        RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");

                        response response_call = SendRequest(RequestXML);
                        if (string.IsNullOrEmpty(GetAllErrors(response_call.errors.error)))
                            response_.status.call.AddRange(response_call.status.call.ToList());
                        else
                            throw new Exception("GetCallReminderResponse::" + GetAllErrors(response_call.errors.error));
                    }
                    else
                    {
                        throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
                    }
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "AppointmentReminder::GetCallReminderResponse::Windows Service", ex);
                }
            }

            return response_;
        }

        #endregion

        #region " Email Reminders "

        public response SendEmailReminder(long EmailId, DateTime DeliveryDate, string FromName, string ToName, string ToEmail, string Subject, DateTime Event, string Body, long ProviderId, SharedVariable SharedVariable = null, string Template = "-1")
        {
            ReminderSetting model = null;
            if (SharedVariable != null)
                model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, ProviderId);
            else
                model = new DALReminders().loadReminderSetting(ProviderId);

            if (model != null)
            {
                string RequestXML = "";
                request request_email = new request(model.APIKey);
                request_email.email.id = "EMAIL" + EmailId;
                request_email.email.delivery = String.Format("{0:yyyy-MM-dd HH:mm}", DeliveryDate) + model.TimeZone;
                request_email.email.subject = Subject;
                request_email.email.append = Body;
                request_email.email.event_ = String.Format("{0:yyyy-MM-dd HH:mm}", Event) + model.TimeZone;
                request_email.email.template = Template;
                request_email.email.from.name = FromName;
                request_email.email.to.name = ToName;
                request_email.email.to.email = ToEmail;



                RequestXML = GetRequestXml(typeof(request), request_email);
                RequestXML = AddAttributes(RequestXML, "email", "action", "create");
                RequestXML = RemoveProperty(RequestXML, "status,text,call,fax,preference,error");
                response response_email = SendRequest(RequestXML);

                if (string.IsNullOrEmpty(GetAllErrors(response_email.errors.error)) && string.IsNullOrEmpty(GetAllEmailNotices(response_email.status.email)))
                    return response_email;
                else
                {
                    string str = GetAllErrors(response_email.errors.error) + GetAllEmailNotices(response_email.status.email);
                    throw new Exception(str);
                }

            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }

        }

        public response GetEmailReminderResponse(long EmailId, long ProviderId)
        {
            ReminderSetting model = new DALReminders().loadReminderSetting(ProviderId);
            if (model != null)
            {
                string RequestXML = "";
                request request_email_status = new request(model.APIKey);
                request_email_status.status.email.Add("EMAIL" + EmailId);

                RequestXML = GetRequestXml(typeof(request), request_email_status);
                RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");

                response response_email = SendRequest(RequestXML);
                if (string.IsNullOrEmpty(GetAllErrors(response_email.errors.error)))
                    return response_email;
                else
                    throw new Exception("GetEmailReminderResponse::" + GetAllErrors(response_email.errors.error));
            }
            else
            {
                throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
            }
        }

        /// <summary>
        /// Get follow up response of multiple Text's
        /// </summary>
        /// <param name="EmailReminderIDs">List of Email Id's</param>
        /// <returns></returns>
        public response GetEmailReminderResponse(SharedVariable SharedVariable, List<RemidnerProviderModel> reminder_list)
        {
            response response_ = new response();
            foreach (var item in reminder_list)
            {
                try
                {
                    ReminderSetting model = new DALReminders(SharedVariable).loadReminderSetting(SharedVariable, item.ProviderId);
                    if (model != null)
                    {
                        string RequestXML = "";
                        request request_email_status = new request(model.APIKey);
                        foreach (var EmailId in item.ReminderIds)
                        {
                            request_email_status.status.email.Add("EMAIL" + EmailId);
                        }

                        RequestXML = GetRequestXml(typeof(request), request_email_status);
                        RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");
                        response response_email = SendRequest(RequestXML);
                        if (string.IsNullOrEmpty(GetAllErrors(response_email.errors.error)))
                            response_.status.email.AddRange(response_email.status.email.ToList());
                        else
                            throw new Exception("GetEmailReminderResponse::" + GetAllErrors(response_email.errors.error));
                    }
                    else
                    {
                        throw new Exception("Provider's reminder settings are not configured. please contact to administrator. ");
                    }
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "AppointmentReminder::GetEmailReminderResponse::Windows Service", ex);
                }
            }

            return response_;
        }

        #endregion

        #region " Common "

        /// <summary>
        /// Get follow up response of multiple calls and texts all at once
        /// </summary>
        /// <param name="CallReminderIDs">List of Call id's</param>
        /// <param name="TextReminderIDs">List of Text id's</param>
        /// <returns></returns>
        //public response GetCallAndTextReminderResponse(List<string> CallReminderIDs, List<string> TextReminderIDs, List<string> EmailReminderIDs)
        //{
        //    string RequestXML = "";
        //    request request_ = new request(ReminderConfiguration.Key);
        //    foreach (var CallId in CallReminderIDs)
        //    {
        //        request_.status.text.Add("CALL" + CallId);
        //    }
        //    foreach (var TextId in TextReminderIDs)
        //    {
        //        request_.status.text.Add("SMS" + TextId);
        //    }
        //    foreach (var EmailId in EmailReminderIDs)
        //    {
        //        request_.status.email.Add("EMAIL" + EmailId);
        //    }

        //    RequestXML = GetRequestXml(typeof(request), request_);
        //    RequestXML = RemoveProperty(RequestXML, "text,call,fax,preference,error,email");

        //    response response_ = SendRequest(RequestXML);
        //    if (string.IsNullOrEmpty(GetAllErrors(response_.errors.error)))
        //        return response_;
        //    else
        //        throw new Exception("GetCallAndTextReminderResponse::" + GetAllErrors(response_.errors.error));
        //}

        #endregion

        #region " Support Functions "

        private string GetAllErrors(List<error> errors)
        {
            string error = string.Empty;
            foreach (error item in errors)
            {
                if (!string.IsNullOrEmpty(item.message))
                {
                    if (string.IsNullOrEmpty(error))
                        error += item.message;
                    else
                        error += "," + item.message;

                }

            }

            return error;
        }

        private string GetAllEmailNotices(List<Response_email> emails)
        {
            string notice = string.Empty;
            foreach (Response_email item in emails)
            {
                if (!string.IsNullOrEmpty(item.notice))
                {
                    if (string.IsNullOrEmpty(notice))
                        notice += item.notice;
                    else
                        notice += "," + item.notice;
                }
            }

            return notice;
        }

        public response SendRequest(string RequestXML)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ReminderConfiguration.URL);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Accept = "application/xml";


                byte[] bytes = Encoding.UTF8.GetBytes(RequestXML);

                request.ContentLength = bytes.Length;

                using (Stream putStream = request.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);
                }

                response response_obj = new response();
                string responseString = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        MemoryStream strm = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
                        response_obj = (response)new XmlSerializer(typeof(response)).Deserialize(strm);
                    }
                    //response_obj = (response)new XmlSerializer(typeof(response)).Deserialize(stream1);
                }

                if (response_obj.errors.error.Count == 0 && response_obj.status.call.Count == 0 && response_obj.status.text.Count == 0 && response_obj.status.fax.Count == 0 && response_obj.status.email.Count == 0)
                {
                    MDVLogger.BLLErrorLog("AppointmentReminders::SendRequest", new Exception("API Respond with some errors." + responseString));
                    throw new Exception("API Respond with some errors.");
                }
                else
                    return response_obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetRequestXml(Type type, object obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(type);
            StringWriter sww = new StringWriter();
            XmlWriterSettings xmlsettings = new XmlWriterSettings();
            xmlsettings.Encoding = Encoding.UTF8;

            XmlWriter writer = XmlWriter.Create(sww, xmlsettings);
            xsSubmit.Serialize(writer, obj);
            return stripNS(XElement.Parse(sww.ToString())).ToString();
        }

        public XElement stripNS(XElement root)
        {
            return new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => stripNS(el)) :
                    (object)root.Value
            );
        }

        public string AddAttributes(string root, string property, string attribute_name, string attribute_value, bool isAddProperty = false)
        {

            XElement element = XElement.Parse(root.ToString());

            if (element.HasElements)
            {
                XElement xel = element.Elements()
                               .FirstOrDefault(x => x.Name.LocalName == property);

                if (isAddProperty && xel == null)
                {
                    xel = new XElement(property);
                    xel.Add(new XAttribute(attribute_name, attribute_value));
                    element.Add(xel);
                }
                else
                {
                    xel.Add(new XAttribute(attribute_name, attribute_value));
                    element.Elements().FirstOrDefault(p => p == xel).ReplaceWith(xel);
                }

            }

            return element.ToString();

        }

        public string RemoveProperty(string root, string properties)
        {
            XElement element = XElement.Parse(root.ToString());

            foreach (var property in properties.Split(','))
            {
                if (element != null && element.HasElements)
                {
                    XElement xel = element.Elements()
                                   .FirstOrDefault(x => x.Name.LocalName == property);

                    if (xel != null && xel.HasElements)
                    {
                        xel.Elements().Remove();
                    }

                    if (xel != null)
                        element.Elements().FirstOrDefault(p => p == xel).Remove();
                }
            }


            return element.ToString();

        }

        #endregion

        #region "Patient Check-in App"
        public string TwoStepVerfication(string MobileNumber,string Code,string SMSId) {
            string RequestXML = "";
            request request_text = new request(System.Configuration.ConfigurationManager.AppSettings["CheckInApp_AccountKey"]);
            request_text.text.id = DateTime.Now.ToString();
            request_text.text.delivery = String.Format("{0:yyyy-MM-dd HH:mm}", DateTime.Now) + System.Configuration.ConfigurationManager.AppSettings["CheckInApp_TimeZone"];
            request_text.text.message = Code;// + ReminderConfiguration.TextConfirmMessage; + ReminderConfiguration.TextCancelMessage;
            request_text.text.number = MDVUtility.FormatPhoneNumber(MobileNumber);
            request_text.text.name = System.Configuration.ConfigurationManager.AppSettings["CheckInApp_SenderName"];

            RequestXML = GetRequestXml(typeof(request), request_text);
            RequestXML = AddAttributes(RequestXML, "text", "action", "create");
            RequestXML = RemoveProperty(RequestXML, "status,call,fax,preference,error,email");
            response response_text = SendRequest(RequestXML);

            if (string.IsNullOrEmpty(GetAllErrors(response_text.errors.error)))
                return response_text.ToString();
            else
                throw new Exception(GetAllErrors(response_text.errors.error));

          
        }

        #endregion
    }
}
