using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_Appointment_Summary
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_Appointment_Summary()
        {
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static Scheduling_Appointment_Summary _obj = null;
        public static Scheduling_Appointment_Summary Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_Appointment_Summary();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string SearchAppointmentSummary(string appointmentSearchData)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(appointmentSearchData);
                DSAppointment dsAppointmentSummary = new DSAppointment();
                BLObject<DSAppointment> obj;
                DataTable dtFacility = new DataTable();
                DataTable dtResource = new DataTable();
                DataTable dtProvider = new DataTable();
                string FacilityIds = string.Empty, ProviderIds = string.Empty, ResourceIds = string.Empty, FromDate = string.Empty, ToDate = string.Empty;
                if (SearchedfieldsJSON != null)
                {

                    if (SearchedfieldsJSON.ContainsKey("ProviderId"))
                        ProviderIds = Convert.ToString(SearchedfieldsJSON["ProviderId"]);
                    if (SearchedfieldsJSON.ContainsKey("FacilityId"))
                        FacilityIds = Convert.ToString(SearchedfieldsJSON["FacilityId"]);
                    if (SearchedfieldsJSON.ContainsKey("ResourceId"))
                        ResourceIds = Convert.ToString(SearchedfieldsJSON["ResourceId"]);
                    if (SearchedfieldsJSON.ContainsKey("ToDate"))
                        ToDate = Convert.ToString(SearchedfieldsJSON["ToDate"]);
                    if (SearchedfieldsJSON.ContainsKey("FromDate"))
                        FromDate = Convert.ToString(SearchedfieldsJSON["FromDate"]);

                    dtFacility = MDVUtility.ConvertCommaSepatedValuesToDataTable(FacilityIds);
                    dtProvider = MDVUtility.ConvertCommaSepatedValuesToDataTable(ProviderIds);
                    dtResource = MDVUtility.ConvertCommaSepatedValuesToDataTable(ResourceIds);
                }
                obj = BLLScheduleObj.LoadAppointmentSummary(ref dtFacility, ref dtProvider, FromDate, ToDate, ref dtResource);
                if (obj.Data != null)
                {
                    dsAppointmentSummary = obj.Data;
                    if (dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentCount = dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName].Rows.Count,
                            AppointmentSummaryLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SearchBlockAppSummary(string appointmentSearchData)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(appointmentSearchData);
                DSAppointment dsAppointmentSummary = new DSAppointment();
                BLObject<DSAppointment> obj;
                DataTable dtFacility = new DataTable();
                DataTable dtResource = new DataTable();
                DataTable dtProvider = new DataTable();
                string FacilityIds = string.Empty, ProviderIds = string.Empty, ResourceIds = string.Empty, FromDate = string.Empty, ToDate = string.Empty;
                if (SearchedfieldsJSON != null)
                {
                    if (SearchedfieldsJSON.ContainsKey("ProviderId"))
                        ProviderIds = Convert.ToString(SearchedfieldsJSON["ProviderId"]);
                    if (SearchedfieldsJSON.ContainsKey("FacilityId"))
                        FacilityIds = Convert.ToString(SearchedfieldsJSON["FacilityId"]);
                    if (SearchedfieldsJSON.ContainsKey("ResourceId"))
                        ResourceIds = Convert.ToString(SearchedfieldsJSON["ResourceId"]);
                    if (SearchedfieldsJSON.ContainsKey("ToDate"))
                        ToDate = Convert.ToString(SearchedfieldsJSON["ToDate"]);
                    if (SearchedfieldsJSON.ContainsKey("FromDate"))
                        FromDate = Convert.ToString(SearchedfieldsJSON["FromDate"]);

                    dtFacility = MDVUtility.ConvertCommaSepatedValuesToDataTable(FacilityIds);
                    dtProvider = MDVUtility.ConvertCommaSepatedValuesToDataTable(ProviderIds);
                    dtResource = MDVUtility.ConvertCommaSepatedValuesToDataTable(ResourceIds);
                }
                obj = BLLScheduleObj.LoadBlockAppointmentSummary(ref dtFacility, ref dtProvider, FromDate, ToDate, ref dtResource);
                if (obj.Data != null)
                {
                    dsAppointmentSummary = obj.Data;
                    if (dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentCount = dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName].Rows.Count,
                            AppointmentSummaryLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsAppointmentSummary.Tables[dsAppointmentSummary.AppointmentSummary.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AppointmentCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SEARCH_APPOINTMENT_SUMMARY":
                    {
                        string appointmentSearchData = context.Request["SearchSummaryData"];
                        string strJSONData = SearchAppointmentSummary(appointmentSearchData);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SCHEDULING_BLOCK_APP_SUMMARY":
                    {
                        string appointmentSearchData = context.Request["SearchBlockedAppSummaryData"];
                        string strJSONData = SearchBlockAppSummary(appointmentSearchData);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}