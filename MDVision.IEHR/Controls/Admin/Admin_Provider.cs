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
    public class Admin_Provider
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Provider()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Provider _obj = null;
        public static Admin_Provider Instance()
        {
            if (_obj == null)
                _obj = new Admin_Provider();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the providers for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The provider identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadProvider(string fieldsJSON, Int64 ProviderID, int PageNumber, int RowsPerPage, string ParentCtrl = "")
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = null;
                    BLObject<DSProfile> obj;
                    if (SearchedfieldsJSON == null)
                    {
                        obj = BLLAdminProfileObj.LoadProvider(ProviderID, null, null, null, null, null, null, null);
                    }
                    else if (ParentCtrl == "Patient_Referrals_Outgoing_Detail")
                    {
                        obj = BLLAdminProfileObj.LoadProvider(ProviderID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["ddlSpeciality"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage, ParentCtrl);
                    }
                    else
                    {
                        obj = BLLAdminProfileObj.LoadProvider(ProviderID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["ddlSpeciality"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    }
                    dsEntity = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = dsEntity.Tables[dsEntity.Provider.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.Provider.Rows.Count > 0) ? dsEntity.Provider.Rows[0][dsEntity.Provider.RecordCountColumn.ColumnName] : 0,
                            ProviderLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Provider.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = 0,
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

        private string LoadProviderEntityBased(string fieldsJSON, Int64 ProviderID, int PageNumber, int RowsPerPage, string ParentCtrl = "")
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSProfile dsEntity = null;
                BLObject<DSProfile> obj;
                if (SearchedfieldsJSON == null)
                {
                    obj = BLLAdminProfileObj.LoadProviderEntityBased(ProviderID, null, null, null, null, null, null, null);
                }
                else if (ParentCtrl == "Patient_Referrals_Outgoing_Detail")
                {
                    obj = BLLAdminProfileObj.LoadProviderEntityBased(ProviderID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["ddlSpeciality"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage, ParentCtrl);
                }
                else
                {
                    obj = BLLAdminProfileObj.LoadProviderEntityBased(ProviderID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["ddlSpeciality"], SearchedfieldsJSON["txtNPI"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                }
                dsEntity = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ProviderCount = dsEntity.Tables[dsEntity.Provider.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsEntity.Provider.Rows.Count > 0) ? dsEntity.Provider.Rows[0][dsEntity.Provider.RecordCountColumn.ColumnName] : 0,
                        ProviderLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Provider.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ProviderCount = 0,
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

        private string LoadProviderLookUp(string shortName)
        {
            try
            {
                DSProfileLookup dsProfileLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupProvider("1", false, shortName);
                if (obj.Data != null)
                {
                    dsProfileLookup = obj.Data;
                    if (dsProfileLookup.Tables[dsProfileLookup.Provider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = dsProfileLookup.Tables[dsProfileLookup.Provider.TableName].Rows.Count,
                            ProviderLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProfileLookup.Tables[dsProfileLookup.Provider.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProviderCount = 0,
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
        /// Handle the Provider Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            //System.Diagnostics.Debugger.Break();
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderData"];
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string ParentCtrl = MDVUtility.ToStr(context.Request["ParentCtrl"]);

                        string strJSONData = LoadProvider(fieldsJSON, ProviderID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), ParentCtrl);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PROVIDER_ENTITYBASED":
                    {
                        string fieldsJSON = context.Request["ProviderData"];
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string ParentCtrl = MDVUtility.ToStr(context.Request["ParentCtrl"]);

                        string strJSONData = LoadProviderEntityBased(fieldsJSON, ProviderID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage), ParentCtrl);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PROVIDER_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        string strJSONData = LoadProviderLookUp(ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}