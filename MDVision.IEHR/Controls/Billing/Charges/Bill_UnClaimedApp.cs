using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.BusinessWrapper;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Billing.Charges
{
    public class Bill_UnClaimed_Appointment
    {
        #region Singleton
        private static Bill_UnClaimed_Appointment _obj = null;
        public static Bill_UnClaimed_Appointment Instance()
        {
            if (_obj == null)
                _obj = new Bill_UnClaimed_Appointment();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string UnClaimedAppSearch(string fieldsJSON)
        {

            string facility;
            string provider;
            string insurance;
            string appstatus;
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (SearchedfieldsJSON["ddlFacility"] == "")
                    facility = "0";
                else
                    facility = SearchedfieldsJSON["ddlFacility"];

                
                if (SearchedfieldsJSON["ddlProvider"] == "")
                    provider = "0";
                else
                    provider = SearchedfieldsJSON["ddlProvider"];

                if (SearchedfieldsJSON["ddlInsurancePlan"] == "")
                    insurance = "0";
                else
                    insurance = SearchedfieldsJSON["ddlInsurancePlan"];

               



                DateTime? dtfrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSfrm"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSfrm"]);
                DateTime? dtto = String.IsNullOrEmpty(SearchedfieldsJSON["dpDOSto"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpDOSto"]);

                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;
                if (SearchedfieldsJSON == null)

                    obj = BusinessWrapper.Billing.BusinessObj.LoadUnClaimedAppointments("", "", 0, 0, "", 0, null, null);
                //string LastName, string FirstName, long FacilityId, long ProviderId, long ResourceId, string Claimed, int AppointmentStatus, long PatInsuranceId, DateTime? DOSFrom = null, DateTime? DOSTo = null
                else
                    obj = BusinessWrapper.Billing.BusinessObj.LoadUnClaimedAppointments(SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["txtFirstName"], Utility.ToInt64(facility), Utility.ToInt64(provider), SearchedfieldsJSON["ddlClaimed"], Utility.ToInt64(insurance), dtfrom, dtto);
                dsCharge = obj.Data;
                var response = new
                {
                    status = true,
                    UnClaimedAppCount = dsCharge.Tables[dsCharge.UnClaimedAppointments.TableName].Rows.Count,
                    UnClaimedAppLoad_JSON = Utility.JSON_DataTable(dsCharge.Tables[dsCharge.UnClaimedAppointments.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_UNCLAIMED_APP":
                    {
                        string fieldsJSON = context.Request["UnClaimedAppData"];
                        string strJSONData = UnClaimedAppSearch(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}