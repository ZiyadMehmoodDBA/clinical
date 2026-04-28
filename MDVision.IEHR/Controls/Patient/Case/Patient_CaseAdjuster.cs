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

namespace MDVision.IEHR.Controls.Patient.Case
{
    public class Patient_CaseAdjuster
    {

        private BLLPatient BLLPatientObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Patient_CaseAdjuster()
        {
            BLLPatientObj = new BLLPatient();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Patient_CaseAdjuster _obj = null;
        public static Patient_CaseAdjuster Instance()
        {
            if (_obj == null)
                _obj = new Patient_CaseAdjuster();
            return _obj;
        }
        #endregion


        #region Private Functions

        /// <summary>
        /// Saves the Case.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveCaseAdjuster(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSCase dsCase = new DSCase();
                DSCase.CaseAdjusterRow dr = dsCase.CaseAdjuster.NewCaseAdjusterRow();
                dr.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                dr.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                    dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                else
                    dr[dsCase.CaseAdjuster.DOBColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAddress1"]))
                    dr.Address1 = MDVUtility.ToStr(SearchedfieldsJSON["txtAddress1"]);
                else
                    dr[dsCase.CaseAdjuster.Address1Column] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAddress2"]))
                    dr.Address2 = MDVUtility.ToStr(SearchedfieldsJSON["txtAddress2"]);
                else
                    dr[dsCase.CaseAdjuster.Address2Column] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCity"]))
                    dr.City = MDVUtility.ToStr(SearchedfieldsJSON["txtCity"]);
                else
                    dr[dsCase.CaseAdjuster.CityColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtState"]))
                    dr.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);
                else
                    dr[dsCase.CaseAdjuster.StateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCity"]))
                    dr.City = MDVUtility.ToStr(SearchedfieldsJSON["txtCity"]);
                else
                    dr[dsCase.CaseAdjuster.CityColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtZip"]))
                    dr.Zip = MDVUtility.ToStr(SearchedfieldsJSON["txtZip"]);
                else
                    dr[dsCase.CaseAdjuster.ZipColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPhone"]))
                    dr.Phone = MDVUtility.ToStr(SearchedfieldsJSON["txtPhone"]);
                else
                    dr[dsCase.CaseAdjuster.PhoneColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtExt"]))
                    dr.Extention = MDVUtility.ToStr(SearchedfieldsJSON["txtExt"]);
                else
                    dr[dsCase.CaseAdjuster.ExtentionColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFax"]))
                    dr.Fax = MDVUtility.ToStr(SearchedfieldsJSON["txtFax"]);
                else
                    dr[dsCase.CaseAdjuster.FaxColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEmail"]))
                    dr.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtEmail"]);
                else
                    dr[dsCase.CaseAdjuster.EmailColumn] = DBNull.Value;
                if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterPhone"]))
                    dr.Preference = "Phone";
                else if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterFax"]))
                    dr.Preference = "Fax";
                else if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterEmail"]))
                    dr.Preference = "Email";
                else
                    dr[dsCase.CaseAdjuster.PreferenceColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNote"]))
                    dr.Notes = MDVUtility.ToStr(SearchedfieldsJSON["txtNote"]);
                else
                    dr[dsCase.CaseAdjuster.NotesColumn] = DBNull.Value;
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;


                #region Database Insertion
                dsCase.CaseAdjuster.AddCaseAdjusterRow(dr);
                BLObject<DSCase> obj = BLLPatientObj.InsertCaseAdjuster(dsCase);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        CaseAdjusterId = dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows[0][dsCase.CaseAdjuster.CaseAdjusterIdColumn.ColumnName]
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
            #endregion

        }

        /// Load all the Case for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseId">The Case identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchCaseAdjuster(long CaseAdjusterId, string fieldsJSON, int PageNumber, int RowsPerPage)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSCase dsCase = null;
                BLObject<DSCase> obj = null;
                if (SearchedfieldsJSON == null || SearchedfieldsJSON.Count==0)
                    obj = BLLPatientObj.LoadCaseAdjuster(CaseAdjusterId, null, null, null, null, null, null, null, null);
                else
                    obj = BLLPatientObj.LoadCaseAdjuster(CaseAdjusterId, SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["txtAddress"], SearchedfieldsJSON["txtCity"], SearchedfieldsJSON["txtState"], SearchedfieldsJSON["txtZip"], SearchedfieldsJSON["txtExtention"], SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                if (obj.Data != null)
                {
                    dsCase = obj.Data;
                    if (dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CaseAdjusterCount = dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCase.CaseAdjuster.Rows[0][dsCase.CaseAdjuster.RecordCountColumn.ColumnName],
                            CaseAdjusterLoad_JSON = MDVUtility.JSON_DataTable(dsCase.Tables[dsCase.CaseAdjuster.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CaseCount = 0,
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

        /// <summary>
        /// Updates the Case.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="CaseAdjusterId">The Case identifier.</param>
        /// <returns></returns>
        private string UpdateCaseAdjuster(string fieldsJSON, Int64 CaseAdjusterId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (CaseAdjusterId > 0)
                {
                    DSCase dsCase = new DSCase();
                    BLObject<DSCase> objLoad = BLLPatientObj.LoadCaseAdjuster(CaseAdjusterId, null, null, null, null, null, null, null, null);
                    dsCase = objLoad.Data;
                    // dsCase.Tables[0] = objLoad.Data.Tables[0];
                    foreach (DSCase.CaseAdjusterRow drCase in dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFirstName"]))
                            drCase.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtLastName"]))
                            drCase.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                            drCase.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        else
                            drCase[dsCase.CaseAdjuster.DOBColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAddress1"]))
                            drCase.Address1 = MDVUtility.ToStr(SearchedfieldsJSON["txtAddress1"]);
                        else
                            drCase[dsCase.CaseAdjuster.Address1Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAddress2"]))
                            drCase.Address2 = MDVUtility.ToStr(SearchedfieldsJSON["txtAddress2"]);
                        else
                            drCase[dsCase.CaseAdjuster.Address2Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCity"]))
                            drCase.City = MDVUtility.ToStr(SearchedfieldsJSON["txtCity"]);
                        else
                            drCase[dsCase.CaseAdjuster.CityColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtState"]))
                            drCase.State = MDVUtility.ToStr(SearchedfieldsJSON["txtState"]);
                        else
                            drCase[dsCase.CaseAdjuster.StateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCity"]))
                            drCase.City = MDVUtility.ToStr(SearchedfieldsJSON["txtCity"]);
                        else
                            drCase[dsCase.CaseAdjuster.CityColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtZip"]))
                            drCase.Zip = MDVUtility.ToStr(SearchedfieldsJSON["txtZip"]);
                        else
                            drCase[dsCase.CaseAdjuster.ZipColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPhone"]))
                            drCase.Phone = MDVUtility.ToStr(SearchedfieldsJSON["txtPhone"]);
                        else
                            drCase[dsCase.CaseAdjuster.PhoneColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtExt"]))
                            drCase.Extention = MDVUtility.ToStr(SearchedfieldsJSON["txtExt"]);
                        else
                            drCase[dsCase.CaseAdjuster.ExtentionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFax"]))
                            drCase.Fax = MDVUtility.ToStr(SearchedfieldsJSON["txtFax"]);
                        else
                            drCase[dsCase.CaseAdjuster.FaxColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtEmail"]))
                            drCase.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtEmail"]);
                        else
                            drCase[dsCase.CaseAdjuster.EmailColumn] = DBNull.Value;
                        if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterPhone"]))
                            drCase.Preference = "Phone";
                        else if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterFax"]))
                            drCase.Preference = "Fax";
                        else if (Convert.ToBoolean(SearchedfieldsJSON["RadCaseAdjusterEmail"]))
                            drCase.Preference = "Email";
                        else
                            drCase[dsCase.CaseAdjuster.PreferenceColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNote"]))
                            drCase.Notes = MDVUtility.ToStr(SearchedfieldsJSON["txtNote"]);
                        else
                            drCase[dsCase.CaseAdjuster.NotesColumn] = DBNull.Value;
                        drCase.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        drCase.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drCase.ModifiedOn = DateTime.Now;
                    }

                    //  dsCase.AcceptChanges();
                    #region Database Updation

                    if (dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows.Count > 0)
                    {

                        BLObject<DSCase> obj = null;
                        obj = BLLPatientObj.UpdateCaseAdjuster(dsCase);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
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
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
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
                        Message = "Case not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        /// <summary>
        /// Updates the Case IsActive.
        /// </summary>

        /// <param name="CaseAdjusterId">The Case identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateCaseAdjusterIsActive(Int64 CaseAdjusterId, Int64 IsActive)
        {
            try
            {
                DSCase dsCase = null;
                BLObject<DSCase> obj = BLLPatientObj.LoadCaseAdjuster(CaseAdjusterId, null, null, null, null, null, null, null, null);
                dsCase = obj.Data;
                if (dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows[0];
                    dr[dsCase.CaseAdjuster.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSCase> objCase = BLLPatientObj.UpdateCaseAdjuster(dsCase);
                    string successMsg;
                    if (objCase.Data != null)
                    {
                        if (IsActive == 0)
                            successMsg = Common.AppPrivileges.Inactive_Message;
                        else
                            successMsg = Common.AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            message = successMsg
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCase.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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

        /// <summary>
        /// Deletes the Case Adjuster.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string DeleteCaseAdjuster(long CaseAdjusterId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CaseAdjusterId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeleteAdjuster(MDVUtility.ToStr(CaseAdjusterId));
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

        /// <summary>
        /// Fills the Case Adjuster
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillCaseAdjuster(long CaseAdjusterId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CaseAdjusterId)))
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
                    DSCase dsCase = null;
                    BLObject<DSCase> obj = BLLPatientObj.LoadCaseAdjuster(CaseAdjusterId, null, null, null, null, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsCase = obj.Data;
                        if (dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows.Count > 0)
                        {

                            DataRow dr = dsCase.Tables[dsCase.CaseAdjuster.TableName].Rows[0];
                            string Phone = "false";
                            string Fax = "false";
                            string Email = "false";
                            if (dr[dsCase.CaseAdjuster.PreferenceColumn] != DBNull.Value)
                            {
                                if (dr[dsCase.CaseAdjuster.PreferenceColumn].ToString() == "Phone")
                                    Phone = "true";
                                else if (dr[dsCase.CaseAdjuster.PreferenceColumn].ToString() == "Fax")
                                    Fax = "true";
                                else
                                {
                                    Email = "true";
                                }
                            }
                            var keyValues = new Dictionary<string, string>
                        {
                            {"txtFirstName", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.FirstNameColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.LastNameColumn.ColumnName])},
                            { "dtpDOB", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.DOBColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.ZipColumn.ColumnName])},
                            { "txtPhone", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.PhoneColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.FaxColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.ExtentionColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.EmailColumn.ColumnName])},
                            { "txtPreference", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.PreferenceColumn.ColumnName])},
                            { "txtNote", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.NotesColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsCase.CaseAdjuster.IsActiveColumn.ColumnName])},
                             { "RadCaseAdjusterPhone", Phone},
                            { "RadCaseAdjusterFax", Fax},
                            { "RadCaseAdjusterEmail",Email},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                CaseFill_JSON = js.Serialize(keyValues)
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_CASE_ADJUSTER":
                    {
                        string fieldsJSON = context.Request["CaseAdjusterData"];
                        string strJSONData = SaveCaseAdjuster(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_CASE":
                    {
                        string fieldsJSON = context.Request["CaseAdjusterData"];
                        Int64 CaseAdjusterID = MDVUtility.ToInt64(context.Request["CaseAdjusterId"]);
                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = SearchCaseAdjuster(CaseAdjusterID, fieldsJSON, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_CASE":
                    {

                        Int64 CaseAdjusterID = MDVUtility.ToInt64(context.Request["CaseAdjusterId"]);
                        string strJSONData = FillCaseAdjuster(CaseAdjusterID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CASE":
                    {
                        Int64 CaseAdjusterID = MDVUtility.ToInt64(context.Request["CaseAdjusterId"]);
                        string strJSONData = DeleteCaseAdjuster(CaseAdjusterID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CASE":
                    {
                        string fieldsJSON = context.Request["CaseData"];
                        Int64 CaseAdjusterID = MDVUtility.ToInt64(context.Request["CaseAdjusterId"]);
                        string strJSONData = UpdateCaseAdjuster(fieldsJSON, CaseAdjusterID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_CASE_ACTIVE_INACTIVE":
                    {

                        Int64 CaseID = MDVUtility.ToInt64(context.Request["CaseAdjusterId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateCaseAdjusterIsActive(CaseID, IsActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}