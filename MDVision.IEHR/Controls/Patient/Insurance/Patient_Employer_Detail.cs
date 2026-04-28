using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Employer_Detail
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Employer_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Employer_Detail _obj = null;
        public static Patient_Employer_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_Employer_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the employer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveEmployer(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = new DSPatientProfile();
                DSPatientProfile.EmployerRow dr = dsProfile.Employer.NewEmployerRow();

                dr.EmployerName = SearchedfieldsJSON["txtName"];
                dr.ContactName = SearchedfieldsJSON["txtContact"];
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["txtState"];
                dr.ZipCode = SearchedfieldsJSON["txtZip"];
                dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                dr.FaxNo = SearchedfieldsJSON["txtFax"];
                dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsProfile.Employer.AddEmployerRow(dr);
                BLObject<DSPatientProfile> obj = BLLPatientObj.InsertEmployer(dsProfile);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        EmployerId = dsProfile.Tables[dsProfile.Employer.TableName].Rows[0][dsProfile.Employer.EmployerIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the employer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <returns></returns>
        private string UpdateEmployer(string fieldsJSON, Int64 EmployerId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (EmployerId > 0)
                {

                    DSPatientProfile dsProfile = new DSPatientProfile();
                    //DSPatientProfile.EmployerRow dr = dsProfile.Employer.NewEmployerRow();
                    BLObject<DSPatientProfile> objLoad = BLLPatientObj.LoadEmployer(EmployerId, null, null, null, null, null, null);
                    dsProfile = objLoad.Data;

                    foreach (DSPatientProfile.EmployerRow dr in dsProfile.Tables[dsProfile.Employer.TableName].Rows)
                    {
                        //dr.EmployerId = EmployerId;
                        dr.EmployerName = SearchedfieldsJSON["txtName"];
                        dr.ContactName = SearchedfieldsJSON["txtContact"];
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZipCode = SearchedfieldsJSON["txtZip"];
                        dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.FaxNo = SearchedfieldsJSON["txtFax"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProfile.Employer.AddEmployerRow(dr);
                    //dsProfile.Employer.AcceptChanges();

                    if (dsProfile.Tables[dsProfile.Employer.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Employer.Rows[0].SetModified();
                        BLObject<DSPatientProfile> obj = BLLPatientObj.UpdateEmployer(dsProfile);
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
                        Message = "Employer not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        /// <summary>
        /// Fills the employer.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <returns></returns>
        private string FillEmployer(Int64 EmployerId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EmployerId)))
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
                    DSPatientProfile dsProfile = null;
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadEmployer(EmployerId, null, null, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.Employer.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.Employer.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtName", MDVUtility.ToStr(dr[dsProfile.Employer.EmployerNameColumn.ColumnName])},
                            { "txtContact", MDVUtility.ToStr(dr[dsProfile.Employer.ContactNameColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsProfile.Employer.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsProfile.Employer.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Employer.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Employer.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Employer.ZipCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsProfile.Employer.ZipExtColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Employer.PhoneNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsProfile.Employer.FaxNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Employer.EmailAddressColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Employer.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                EmployerFill_JSON = js.Serialize(keyValues)
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
                            Message = obj.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes the employer.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <returns></returns>
        private string DeleteEmployer(Int64 EmployerId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EmployerId)))
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
                    BLObject<string> obj = BLLPatientObj.DeleteEmployer(MDVUtility.ToStr(EmployerId));
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Updates the employer is active.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateEmployerIsActive(Int64 EmployerId, Int64 IsActive)
        {
            try
            {
                if (EmployerId > 0)
                {

                    DSPatientProfile dsProfile = null;
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadEmployer(EmployerId, null, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Employer.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Employer.TableName].Rows[0];
                        dr[dsProfile.Employer.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientProfile> objEmployer = BLLPatientObj.UpdateEmployer(dsProfile);
                        string successMsg;
                        if (objEmployer.Data != null)
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
                                Message = objEmployer.Message
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Employer not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Employer Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_EMPLOYER":
                    {
                        string fieldsJSON = context.Request["EmployerData"];
                        string strJSONData = SaveEmployer(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EMPLOYER":
                    {
                        string strEmployerId = context.Request["EmployerID"];
                        string strJSONData = FillEmployer(MDVUtility.ToInt64(strEmployerId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EMPLOYER":
                    {
                        string strEmployerId = context.Request["EmployerID"];
                        string strJSONData = DeleteEmployer(MDVUtility.ToInt64(strEmployerId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EMPLOYER":
                    {
                        string fieldsJSON = context.Request["EmployerData"];
                        Int64 EmployerID = MDVUtility.ToInt64(context.Request["EmployerID"]);
                        string strJSONData = UpdateEmployer(fieldsJSON, EmployerID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EMPLOYER_ACTIVE_INACTIVE":
                    {
                        Int64 EmployerID = MDVUtility.ToInt64(context.Request["EmployerID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEmployerIsActive(EmployerID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}