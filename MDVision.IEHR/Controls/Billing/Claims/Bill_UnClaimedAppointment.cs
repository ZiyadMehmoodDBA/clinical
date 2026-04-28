using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_UnClaimed_Appointment
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_UnClaimed_Appointment()
        {
            BLLBillingObj = new BLLBilling();
        }
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

        private string UnClaimedAppSearch(string fieldsJSON, int PageNumber, int RowsPerPage)
        {

            string facility;
            string provider;
            string insurance;
            string appstatus;
            string claimno;
            string patientAccount;
          //  string ClaimStatus;
            int? SubmitStatusId=null; 
            int? AppointmentStatusId = null; 
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (SearchedfieldsJSON["txtUnclaimedFacility"] == "")
                    facility = "0";
                else
                    facility = SearchedfieldsJSON["hfFacility"];


                if (SearchedfieldsJSON["txtUnClaimedProvider"] == "")
                    provider = "0";
                else
                    provider = SearchedfieldsJSON["hfProvider"];

                if (SearchedfieldsJSON["hfInsurancePlan"] == "")
                    insurance = "0";
                else
                    insurance = SearchedfieldsJSON["hfInsurancePlan"];

                if (SearchedfieldsJSON.ContainsKey("txtClaimno"))
                {
                    if (SearchedfieldsJSON["txtClaimno"] == "")
                        claimno = "";
                    else
                        claimno = SearchedfieldsJSON["txtClaimno"];
                }
                else
                {
                    if (SearchedfieldsJSON["txtClaimNumber"] == "")
                        claimno = "";
                    else
                        claimno = SearchedfieldsJSON["txtClaimNumber"];
                }

                patientAccount = SearchedfieldsJSON["txtPatientName"];

                if (SearchedfieldsJSON.ContainsKey("ddlSubmitStatus"))
                        SubmitStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlSubmitStatus"]);

                //if (SearchedfieldsJSON["ClaimStatus"] == "")
                //    ClaimStatus = null;
                //else
                //    ClaimStatus = SearchedfieldsJSON["ClaimStatus"];
                if (SearchedfieldsJSON.ContainsKey("ddlAppointmentStatus"))
                    AppointmentStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAppointmentStatus"]);

                DateTime? dtfrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpUnclaimedDOSFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpUnclaimedDOSFrom"]);
                DateTime? dtto = String.IsNullOrEmpty(SearchedfieldsJSON["dpUnclaimedDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpUnclaimedDOSTo"]);
                if (dtto == null)
                    dtto = dtfrom;
                if (dtfrom == null)
                    dtfrom = dtto;
                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;
                if (SearchedfieldsJSON == null)

                    obj =BLLBillingObj.UnClaimedAppointmentsSelect("", "", 0, 0, "", 0, null, null, null, null, null, null,PageNumber, RowsPerPage);
                //string LastName, string FirstName, long FacilityId, long ProviderId, long ResourceId, string Claimed, int AppointmentStatus, long PatInsuranceId, DateTime? DOSFrom = null, DateTime? DOSTo = null
                else
                    obj = BLLBillingObj.UnClaimedAppointmentsSelect(SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["txtFirstName"], MDVUtility.ToInt64(facility), MDVUtility.ToInt64(provider), SearchedfieldsJSON["ddlClaimed"], MDVUtility.ToInt64(insurance), claimno, dtfrom, dtto, patientAccount, SubmitStatusId, AppointmentStatusId, PageNumber, RowsPerPage);
                dsCharge = obj.Data;
                if (obj.Data != null) 
                {
                    if (dsCharge.Tables[dsCharge.UnClaimedAppointments.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UnClaimedAppCount = dsCharge.Tables[dsCharge.UnClaimedAppointments.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCharge.UnClaimedAppointments.Rows[0][dsCharge.UnClaimedAppointments.RecordCountColumn.ColumnName],
                            UnClaimedAppLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.UnClaimedAppointments.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UnClaimedAppCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else 
                {
                    var response = new
                    {
                        status = false,
                        UnClaimedAppCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
               
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

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_UNCLAIMED_APP":
                    {
                        string fieldsJSON = context.Request["UnClaimedAppData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = UnClaimedAppSearch(fieldsJSON, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

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