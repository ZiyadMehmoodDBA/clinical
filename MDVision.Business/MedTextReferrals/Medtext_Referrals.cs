using Jose;
using MDVision.Business.MedTextReferrals.RequestModels;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace MDVision.Business.MedTextReferrals
{
    public class Medtext_Referrals
    {

        public string getMedtextToken()
        {
            string token = string.Empty;
            try
            {

                //Secret Key
                var secretKey = WebConfigurationManager.AppSettings["Medtext_JWTSecret"];

                //Email
                var email = WebConfigurationManager.AppSettings["Medtext_Email"];

                //Header
                var headers = new Dictionary<string, object>()
                            {
                                 { "type", "JWT" },
                                 { "alg", JwsAlgorithm.HS256.ToString() },
                            };

                //-- PayLoad
                var issued_at = DateTime.UtcNow;
                var unix_seconds = (Int32)(issued_at.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var jti_raw = string.Format(Guid.NewGuid() + ":{0}", issued_at.TimeOfDay);
                var expiration = unix_seconds + 300;
                var jti = MDVUtility.EncryptToSHA256(jti_raw, "MedtextJWT");

                var payload = new Dictionary<string, object>()
                            {
                                { "iat", unix_seconds },
                                { "jti", jti },
                                { "exp", expiration},
                                { "email", email}
                            };

                //Token
                byte[] toBytes = Encoding.ASCII.GetBytes(secretKey);
                token = JWT.Encode(payload, toBytes, JwsAlgorithm.HS256, headers);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("Medtext_Referrals::getMedtextToken", ex);
                throw ex;
            }


            return token;
        }
        public string getMedtextCheckOutURL()
        {
            string dashboard_url = string.Empty;
            try
            {
                //URL
                dashboard_url = WebConfigurationManager.AppSettings["Medtext_URL"]
                            + WebConfigurationManager.AppSettings["Medtext_CheckOut"] +"{MedTextReferralId}"
                            + "?token=" + getMedtextToken();
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("Medtext_Referrals::getMedtextURL", ex);
                throw ex;
            }


            return dashboard_url;
        }

        public ResponseModels.checkInResponse checkInPatient(DSPatient.PatientsRow patient,string ProviderNPI,string RefProviderNPI, string notes, string visitType, string reason, List<problems> problem_list, List<procedures> procedure_list, string token)
        {
            ResponseModels.checkInResponse response_obj = new ResponseModels.checkInResponse();
            RequestModels.mdvision_medtext obj = new RequestModels.mdvision_medtext();
            obj.mdvision_appointment.mdvision_id = new Random().Next(1, 10000);
            obj.mdvision_appointment.patient.mdvision_id = patient.PatientId;
            obj.mdvision_appointment.patient.account_number = patient.AccountNumber;
            obj.mdvision_appointment.patient.first_name = patient.FirstName;
            obj.mdvision_appointment.patient.last_name = patient.LastName;
            obj.mdvision_appointment.patient.phone = MDVUtility.FormatPhoneNumber(patient.HomePhoneNo);
            obj.mdvision_appointment.patient.email = patient.EmailAddress;
            obj.mdvision_appointment.patient.gender = patient.Gender;
            obj.mdvision_appointment.patient.dob = patient.DOB.ToString("yyyy-MM-dd");
            obj.mdvision_appointment.patient.address.street = patient.Address1;
            obj.mdvision_appointment.patient.address.street2 = patient.Address2;
            obj.mdvision_appointment.patient.address.city = patient.City;
            obj.mdvision_appointment.patient.address.state = patient.State;
            obj.mdvision_appointment.patient.address.zip = patient.ZIPCode;
            obj.mdvision_appointment.patient.zip_code = patient.ZIPCode;

            obj.mdvision_appointment.patient.primary_insurance_provider.name = patient.InsuranceName;
          
           // obj.mdvision_appointment.referring_provider.npi = RefProviderNPI;
         
            obj.mdvision_appointment.patient.problems = problem_list;
            obj.mdvision_appointment.patient.procedures = procedure_list;

            obj.mdvision_appointment.attending_provider.npi = ProviderNPI;
            //obj.mdvision_appointment.referring_provider.npi = ProviderNPI;
            referrals reff = new referrals();
            reff.receiving_provider.npi = RefProviderNPI;
            reff.notes = notes;
            reff.reason = reason;
            reff.visit_type = visitType;
            obj.mdvision_appointment.referrals.Add(reff);



            obj.mdvision_appointment.location.id = 8011;// patient.FacilityId.ToString();


            try
            {
                //URL
                var url = WebConfigurationManager.AppSettings["Medtext_URL"] + WebConfigurationManager.AppSettings["Medtext_CheckIn"];
                url += "?token=" + token;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";

                using (Stream writer = webRequest.GetRequestStream())
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string data = jss.Serialize(obj);
                    byte[] bytes = new ASCIIEncoding().GetBytes(data);
                    writer.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse res = webRequest.GetResponse())
                {
                    using (Stream stream = res.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        response_obj.ResponseString = reader.ReadToEnd();
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        response_obj.checkIn = jss.Deserialize<ResponseModels.checkIn>(response_obj.ResponseString);
                        response_obj.IsCheckedIn = true;
                        response_obj.CheckOutURL = WebConfigurationManager.AppSettings["Medtext_URL"]
                            + WebConfigurationManager.AppSettings["Medtext_CheckOut"] + response_obj.checkIn.id
                            + "?token=" + token;
                    }
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("Medtext_Referrals::checkInPateint", ex);
                response_obj.ErrorMessage = ex.Message;
                response_obj.IsCheckedIn = false;
            }


            return response_obj;
        }

    }

}
