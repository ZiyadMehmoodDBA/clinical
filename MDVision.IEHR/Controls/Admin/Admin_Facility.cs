using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Facility
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Facility()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Facility _obj = null;
        public static Admin_Facility Instance()
        {
            if (_obj == null)
                _obj = new Admin_Facility();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the facilities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string LoadFacility(string fieldsJSON, Int64 FacilityID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Facility", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = null;
                    BLObject<DSProfile> objFacility;
                    long ProviderId = 0;
                    if (SearchedfieldsJSON != null && SearchedfieldsJSON.ContainsKey("ProviderId") && MDVUtility.ToInt64(SearchedfieldsJSON["ProviderId"]) > 0)
                        ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ProviderId"]);
                    if (SearchedfieldsJSON == null)
                        objFacility = BLLAdminProfileObj.LoadFacility(FacilityID, null, null, null, null, null);
                    else if (SearchedfieldsJSON.ContainsKey("RefForm") && (SearchedfieldsJSON["RefForm"] == "frmClinicalRadiologyOrderDetail") && SearchedfieldsJSON.ContainsKey("LoadAllFacility") && SearchedfieldsJSON["LoadAllFacility"] == "True" && ProviderId > 0)
                        objFacility = BLLAdminProfileObj.LoadProviderDiagnosticImagingFacility(FacilityID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["lstPractice"], SearchedfieldsJSON["lstPos"], SearchedfieldsJSON["chkActive"], ProviderId, PageNumber, RowsPerPage);
                    else if (SearchedfieldsJSON.ContainsKey("RefForm") && (SearchedfieldsJSON["RefForm"] == "frmPatientReferralsOutgoingDetail" || SearchedfieldsJSON["RefForm"] == "frmClinicalRadiologyOrderDetail" || SearchedfieldsJSON["RefForm"] == "frmSSRSReports") && SearchedfieldsJSON.ContainsKey("LoadAllFacility") && SearchedfieldsJSON["LoadAllFacility"] == "True")
                        objFacility = BLLAdminProfileObj.LoadFacilityOutgoingReferral(FacilityID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["lstPractice"], SearchedfieldsJSON["lstPos"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    else
                        objFacility = BLLAdminProfileObj.LoadFacility(FacilityID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["lstPractice"], SearchedfieldsJSON["lstPos"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);

                    dsEntity = objFacility.Data;
                    if (objFacility.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = dsEntity.Tables[dsEntity.Facility.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.Facility.Rows.Count > 0) ? dsEntity.Facility.Rows[0][dsEntity.Facility.RecordCountColumn.ColumnName] : 0,
                            FacilityLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Facility.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = 0,
                            Message = objFacility.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return (JsonConvert.SerializeObject(response));
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

        private string LoadFacilityLookUp(string shortName)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupFacility("1", null, shortName);
                if (obj.Data != null)
                {
                    dsProfileLookup = obj.Data;
                    if (dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count,
                            FacilityLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.Facility.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = 0,
                            Message = obj.Message
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
        private string ProvidersDiagnosticImagingFacilityLookUp(Int64 ProviderId, string shortName)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.ProvidersDiagnosticImagingFacilityLookUp(ProviderId, "1", null, shortName);
                if (obj.Data != null)
                {
                    dsProfileLookup = obj.Data;
                    if (dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count,
                            FacilityLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.Facility.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = 0,
                            Message = obj.Message
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

        private string LoadFacilityDescriptionLookUp(string shortName)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupFacilityDescription("1", null, shortName);
                if (obj.Data != null)
                {
                    dsProfileLookup = obj.Data;
                    if (dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = dsProfileLookup.Tables[dsProfileLookup.Facility.TableName].Rows.Count,
                            FacilityLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.Facility.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FacilityCount = 0,
                            Message = obj.Message
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
        /// <summary>
        /// Handle the Facility Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityData"];
                        Int64 FacilityID = MDVUtility.ToInt64(context.Request["FacilityID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadFacility(fieldsJSON, FacilityID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_FACILITY_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        string strJSONData = LoadFacilityLookUp(ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_FACILITY_DESCRIPTION_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        string strJSONData = LoadFacilityDescriptionLookUp(ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PROVIDERS_DIAGNOSTIC_IMAGING_FACILITY_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        Int64 ProviderId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        string strJSONData = ProvidersDiagnosticImagingFacilityLookUp(ProviderId, ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        #endregion
    }
}