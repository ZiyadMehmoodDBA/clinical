using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Specialty
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Specialty()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_Specialty _obj = null;
        public static Admin_Specialty Instance()
        {
            if (_obj == null)
                _obj = new Admin_Specialty();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadSpecialty(string fieldsJSON, Int64 SpecialtyID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Specialty", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj;
                    if (SearchedfieldsJSON == null)
                    {
                        obj = BLLAdminProfileObj.LoadSpecialty(SpecialtyID, null, null, null, null);
                    }
                    else if (!SearchedfieldsJSON.ContainsKey("ddlEntity"))
                    {
                        obj = BLLAdminProfileObj.LoadSpecialtyAll(SpecialtyID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    }
                    else
                    {
                        obj = BLLAdminProfileObj.LoadSpecialty(SpecialtyID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);
                    }
                    dsProfile = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            SpecialtyCount = dsProfile.Tables[dsProfile.Specialty.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsProfile.Specialty.Rows.Count > 0) ? dsProfile.Specialty.Rows[0][dsProfile.Specialty.RecordCountColumn.ColumnName] : 0,
                            SpecialtyLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Specialty.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SpecialtyCount = 0,
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Specialty Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SPECIALTY":
                    {
                        string fieldsJSON = context.Request["SpecialtyData"];
                        Int64 SpecialtyID = MDVUtility.ToInt64(context.Request["SpecialtyID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadSpecialty(fieldsJSON, SpecialtyID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    //case "APPLY_SECURITY":
                    //    {
                    //        string formname = "Specialty";
                    //        string strJSONData = AppPrivileges.GetFormSecurity(formname);

                    //        context.Response.ContentType = "text/plain";
                    //        context.Response.Write(strJSONData);
                    //    }
                    //break;
            }
        }
        #endregion
    }
}