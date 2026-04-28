using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_InsurancePlan
    {
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Admin_InsurancePlan()
        {
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Admin_InsurancePlan _obj = null;
        public static Admin_InsurancePlan Instance()
        {
            if (_obj == null)
                _obj = new Admin_InsurancePlan();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the insurance plan.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="InsurancePlanID">The insurance plan identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadInsurancePlan(string fieldsJSON, Int64 InsurancePlanID, string SubScriberID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Insurance Plan", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSInsurance dsInsurance = null;
                    BLObject<DSInsurance> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminInsuranceObj.SubscriberMatching(SubScriberID);
                    else
                        obj = BLLAdminInsuranceObj.LoadInsurancePlan(InsurancePlanID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["hfAdminInsurancePlan"], SearchedfieldsJSON["ddlPlanCategory"], SearchedfieldsJSON["ddlPlanType"], SearchedfieldsJSON["ddlClaimFlag"], SearchedfieldsJSON["ddlClaimType"], SubScriberID, SearchedfieldsJSON["chkActive"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["txtAddress"], PageNumber, RowsPerPage);
                    dsInsurance = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            InsurancePlanCount = dsInsurance.Tables[dsInsurance.InsurancePlan.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsInsurance.InsurancePlan.Rows.Count > 0) ? dsInsurance.InsurancePlan.Rows[0][dsInsurance.InsurancePlan.RecordCountColumn.ColumnName] : 0,
                            InsurancePlanLoad_JSON = MDVUtility.JSON_DataTable(dsInsurance.Tables[dsInsurance.InsurancePlan.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            InsurancePlanCount = 0,
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

        private string LoadInsuranceLookUp(string shortName)
        {
            try
            {
                DSInsuranceLookup dsPlanLookup = null;
                BLObject<DSInsuranceLookup> obj;
                obj = BLLAdminInsuranceObj.LookupInsurancePlan("1", null, shortName);
                if (obj.Data != null)
                {
                    dsPlanLookup = obj.Data;
                    if (dsPlanLookup.Tables[dsPlanLookup.InsurancePlan.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PlanCount = dsPlanLookup.Tables[dsPlanLookup.InsurancePlan.TableName].Rows.Count,
                            PlanLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPlanLookup.Tables[dsPlanLookup.InsurancePlan.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PlanCount = 0,
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
        /// Handle the Insurance Plan Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_INSURANCE_PLAN":
                    {
                        string fieldsJSON = context.Request["InsurancePlanData"];
                        Int64 InsurancePlanID = MDVUtility.ToInt64(context.Request["InsurancePlanID"]);
                        string SubScriberID = "";
                        if (context.Request["SubScriberId"] != null)
                        {
                            SubScriberID = context.Request["SubScriberId"];
                            SubScriberID = SubScriberID.Replace("!per", "%");
                        }

                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadInsurancePlan(fieldsJSON, InsurancePlanID, SubScriberID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_INSURANCE_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        string strJSONData = LoadInsuranceLookUp(ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}