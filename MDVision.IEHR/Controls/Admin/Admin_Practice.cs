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
    public class Admin_Practice
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Practice()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton

        private static Admin_Practice _instance = null;
        public static Admin_Practice Instance()
        {
            if (_instance == null)
                _instance = new Admin_Practice();
            return _instance;
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the practice.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PracticeID">The practice identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadPractice(string fieldsJSON, Int64 PracticeID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Practice", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsEntity = null;
                    BLObject<DSProfile> objPractice;


                    if (SearchedfieldsJSON == null)
                    {
                        objPractice = BLLAdminProfileObj.LoadPractice(PracticeID, null, null, null, null, null);

                    }
                    else
                        objPractice = BLLAdminProfileObj.LoadPractice(PracticeID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["txtEIN"], SearchedfieldsJSON["chkActive"], PageNumber, RowsPerPage);

                    dsEntity = objPractice.Data;
                    if (objPractice.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PracticeCount = dsEntity.Tables[dsEntity.Practice.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.Practice.Rows.Count > 0) ? dsEntity.Practice.Rows[0][dsEntity.Practice.RecordCountColumn.ColumnName] : 0,

                            PracticeLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.Practice.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PracticeCount = 0,
                            Message = objPractice.Message
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

        private string LoadPracticeLookUp(string shortName)
        {
            try
            {
                DSProfileLookup dsPracticeLookup = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupPractice("1", null, shortName);
                if (obj.Data != null)
                {
                    dsPracticeLookup = obj.Data;
                    if (dsPracticeLookup.Tables[dsPracticeLookup.Practice.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PracticeCount = dsPracticeLookup.Tables[dsPracticeLookup.Practice.TableName].Rows.Count,
                            PracticeLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPracticeLookup.Tables[dsPracticeLookup.Practice.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PracticeCount = 0,
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
        /// Handle the Practice Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_PRACTICE":
                    {
                        string fieldsJSON = context.Request["PracticeData"];
                        Int64 PracticeID = MDVUtility.ToInt64(context.Request["PracticeID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);

                        string strJSONData = LoadPractice(fieldsJSON, PracticeID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PRACTICE_LOOKUP":
                    {
                        string ShortName = context.Request["ShortName"];
                        string strJSONData = LoadPracticeLookUp(ShortName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}