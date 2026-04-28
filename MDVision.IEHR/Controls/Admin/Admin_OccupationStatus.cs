using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_OccupationStatus
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_OccupationStatus()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }

        #region Singleton
        private static Admin_OccupationStatus _obj = null;
        public static Admin_OccupationStatus Instance()
        {
            if (_obj == null)
                _obj = new Admin_OccupationStatus();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Occupation Status.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveOccupationStatus(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Occupation Status", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSClinicalSummary dsOccupationStatus = new DSClinicalSummary();
                    DSClinicalSummary.OccupationStatusRow dr = dsOccupationStatus.OccupationStatus.NewOccupationStatusRow();

                    if (SearchedfieldsJSON["hfStatusId"] != "" && SearchedfieldsJSON["hfStatusId"] != "0" && SearchedfieldsJSON["hfStatusId"] != "-1")
                    {
                        dr.StatusId = MDVUtility.ToInt32(SearchedfieldsJSON["hfStatusId"]);
                    }
                    dr.ConceptCode = SearchedfieldsJSON["txtConceptCode"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsOccupation = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]) == "True" ? true : false;

                    #region Database Insertion
                    dsOccupationStatus.OccupationStatus.AddOccupationStatusRow(dr);
                    BLObject<DSClinicalSummary> obj = BLLAdminCodesObj.InsertOccupationStatus(ref dsOccupationStatus);
                    dsOccupationStatus = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            StatusId = dsOccupationStatus.Tables[dsOccupationStatus.OccupationStatus.TableName].Rows[0][dsOccupationStatus.OccupationStatus.IdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
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

        private string LoadOccupationStatus(string fieldsJSON, Int64 StatusID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Occupation Status", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSClinicalSummary dsOccupationStatus = null;
                    BLObject<DSClinicalSummary> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminCodesObj.LoadOccupationStatus(StatusID, null, null, null);
                    else
                        obj = BLLAdminCodesObj.LoadOccupationStatus(StatusID, SearchedfieldsJSON["txtConceptCode"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["lstOccupation"], PageNumber, RowsPerPage);
                    dsOccupationStatus = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            OccupationStatusCount = dsOccupationStatus.Tables[dsOccupationStatus.OccupationStatus.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsOccupationStatus.OccupationStatus.Rows.Count > 0) ? dsOccupationStatus.OccupationStatus.Rows[0][dsOccupationStatus.OccupationStatus.RecordCountColumn.ColumnName] : 0,
                            OccupationStatusLoad_JSON = MDVUtility.JSON_DataTable(dsOccupationStatus.Tables[dsOccupationStatus.OccupationStatus.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            RevenueCodeCount = 0,
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

        private string FillOccupationStatus(Int64 StatusID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Occupation Status", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatusID)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        DSClinicalSummary dsOccupationStatus = null;
                        BLObject<DSClinicalSummary> obj = BLLAdminCodesObj.LoadOccupationStatus(StatusID, null, null, null);
                        if (obj.Data != null)
                        {
                            dsOccupationStatus = obj.Data;
                            if (dsOccupationStatus.Tables[dsOccupationStatus.OccupationStatus.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsOccupationStatus.Tables[dsOccupationStatus.OccupationStatus.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "txtConceptCode", MDVUtility.ToStr(dr[dsOccupationStatus.OccupationStatus.ConceptCodeColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsOccupationStatus.OccupationStatus.DescriptionColumn.ColumnName])},
                          { "IsOccupation", MDVUtility.ToStr(dr[dsOccupationStatus.OccupationStatus.IsOccupationColumn.ColumnName])},
                          { "hfStatusId", MDVUtility.ToStr(dr[dsOccupationStatus.OccupationStatus.StatusIdColumn.ColumnName])},
                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    OccupationStatus_JSON = js.Serialize(keyValues)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = Common.AppPrivileges.No_Record_Message,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = obj.Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }


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

        private string DeleteOccupationStatus(Int64 StatusID)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Occupation Status", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatusID)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        BLObject<string> obj = BLLAdminCodesObj.DeleteOccupationStatus(MDVUtility.ToStr(StatusID));
                        if (obj.Data == "")
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Delete_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = obj.Data
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
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
        /// Handle the Revenue Code Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_OCCUPATIONSTATUS_CODE":
                    {
                        string fieldsJSON = context.Request["OccupationStatusData"];
                        string strJSONData = SaveOccupationStatus(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_OCCUPATIONSTATUS_CODE":
                    {
                        string fieldsJSON = context.Request["OccupationStatusData"];
                        Int64 StatusID = MDVUtility.ToInt64(context.Request["StatusID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadOccupationStatus(fieldsJSON, StatusID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_OCCUPATION_STATUS":
                    {
                        string strStatusID = context.Request["StatusID"];
                        string strJSONData = FillOccupationStatus(MDVUtility.ToInt64(strStatusID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_OCCUPATION_STATUS":
                    {
                        Int64 StatusId = MDVUtility.ToInt64(context.Request["StatusId"]);
                        string strJSONData = DeleteOccupationStatus(StatusId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion

    }
}