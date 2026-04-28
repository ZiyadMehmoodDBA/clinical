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

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    /// <summary>
    /// Class Patient_EmergencyContact.
    /// </summary>
    public class Patient_EmergencyContact
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_EmergencyContact()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_EmergencyContact _obj = null;
        public static Patient_EmergencyContact Instance()
        {
            if (_obj == null)
                _obj = new Patient_EmergencyContact();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveEmergencyContact(string fieldsJSON, Int64 PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient.EmergencyContactsRow dr = dsPatient.EmergencyContacts.NewEmergencyContactsRow();

                    dr.PatientId = MDVUtility.ToInt64(PatientId);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtLastName"]))
                        dr.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFirstName"]))
                        dr.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMiddleInitial"]))
                        dr.MI = MDVUtility.ToStr(SearchedfieldsJSON["txtMiddleInitial"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                        dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                    dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.Zipcode = SearchedfieldsJSON["txtZip"];
                    dr.Zipext = SearchedfieldsJSON["txtZipExt"];
                    dr.HomePhone = SearchedfieldsJSON["txtHomeTel"];
                    dr.WorkPhone = SearchedfieldsJSON["txtWorkTel"];
                    dr.WorkPhext = SearchedfieldsJSON["txtExt"];
                    dr.CellNo = SearchedfieldsJSON["txtCell"];
                    dr.FaxNo = SearchedfieldsJSON["txtFax"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True" ? true : false;
                    dr.IsActive = true;//Utility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPatient.EmergencyContacts.AddEmergencyContactsRow(dr);
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientEmergencyContact(dsPatient);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PatientEmergencyContactId = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0][dsPatient.EmergencyContacts.ContactIdColumn.ColumnName]
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
                    throw new Exception("Patient not found.");
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
        /// Updates the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EmergencyContactID">The emergency contact identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateEmergencyContact(string fieldsJSON, Int64 PatientId, Int64 EmergencyContactID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (EmergencyContactID > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    //DSPatient.EmergencyContactsRow dr = dsPatient.EmergencyContacts.NewEmergencyContactsRow();
                    BLObject<DSPatient> objLoad = BLLPatientObj.LoadPatientEmergencyContact(PatientId, EmergencyContactID);
                    dsPatient = objLoad.Data;
                    foreach (DSPatient.EmergencyContactsRow dr in dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows)
                    {
                        //dr.ContactId = EmergencyContactID;
                        dr.PatientId = MDVUtility.ToInt64(PatientId);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtLastName"]))
                            dr.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFirstName"]))
                            dr.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMiddleInitial"]))
                            dr.MI = MDVUtility.ToStr(SearchedfieldsJSON["txtMiddleInitial"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                            dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        else
                            dr[dsPatient.EmergencyContacts.DOBColumn] = DBNull.Value;
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.Zipcode = SearchedfieldsJSON["txtZip"];
                        dr.Zipext = SearchedfieldsJSON["txtZipExt"];
                        dr.HomePhone = SearchedfieldsJSON["txtHomeTel"];
                        dr.WorkPhone = SearchedfieldsJSON["txtWorkTel"];
                        dr.WorkPhext = SearchedfieldsJSON["txtExt"];
                        dr.CellNo = SearchedfieldsJSON["txtCell"];
                        dr.FaxNo = SearchedfieldsJSON["txtFax"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["hfIsActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }


                    #region Database Updation
                    //dsPatient.EmergencyContacts.AddEmergencyContactsRow(dr);
                    //dsPatient.Patients.AddPatientsRow(dr);
                    //dsPatient.EmergencyContacts.AcceptChanges();

                    if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                    {
                        //dsPatient.EmergencyContacts.Rows[0].SetModified();
                        BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientEmergencyContact(dsPatient);
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
                        Message = "Emergency Contact not found."
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
            //return "";
        }

        private string UpdateEmergencyContactIsActive(Int64 PatientId, Int64 EmergencyContactID, Int64 IsActive)
        {
            try
            {

                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = BLLPatientObj.LoadPatientEmergencyContact(PatientId, EmergencyContactID);
                dsPatient = obj.Data;
                if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0];
                    dr[dsPatient.EmergencyContacts.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSPatient> objEmergencyContact = BLLPatientObj.UpdatePatientEmergencyContact(dsPatient);
                    string successMsg;
                    if (objEmergencyContact.Data != null)
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
                            Message = objEmergencyContact.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateEmergencyContactIsPrimary(Int64 PatientId, Int64 EmergencyContactID, Int64 IsPrimary)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = BLLPatientObj.LoadPatientEmergencyContact(PatientId, EmergencyContactID);
                dsPatient = obj.Data;
                if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0];
                    dr[dsPatient.EmergencyContacts.IsPrimaryColumn.ColumnName] = IsPrimary;

                    BLObject<DSPatient> objEmergencyContact = BLLPatientObj.UpdatePatientEmergencyContact(dsPatient);
                    string successMsg;
                    if (objEmergencyContact.Data != null)
                    {
                        //successMsg = Common.AppPrivileges.IsPrimary_Contact_Message;
                        if (IsPrimary == 0)
                            successMsg = Common.AppPrivileges.IsNonPrimary_Contact_Message;
                        else
                            successMsg = Common.AppPrivileges.IsPrimary_Contact_Message;
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
                            Message = objEmergencyContact.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string LoadPatientEmergencyContacts(long PatientID, int pageNumber, int rowsPerPage)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
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
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.loadPatientEmergencyContacts(PatientID, pageNumber, rowsPerPage);
                    dsPatient = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EmergencyContactsCount = dsPatient.Tables[dsPatient.EmergencyContactsSearch.TableName].Rows.Count > 0 ? dsPatient.Tables[dsPatient.EmergencyContactsSearch.TableName].Rows.Count : 0,
                            EmergencyContactsLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.EmergencyContactsSearch.TableName]),
                            iTotalDisplayRecords = (dsPatient.EmergencyContactsSearch.Rows.Count > 0) ? dsPatient.EmergencyContactsSearch.Rows[0][dsPatient.EmergencyContactsSearch.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EmergencyContactsCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                return "";
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
        /// Fills the emergency contact.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillEmergencyContact(long PatientID, long EmergencyContactID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
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
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientEmergencyContact(PatientID, EmergencyContactID);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtLastName", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.LastNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.FirstNameColumn.ColumnName])},
                            { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.MIColumn.ColumnName])},
                            { "dtpDOB", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.DOBColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.ZipcodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.ZipextColumn.ColumnName])},
                            { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.HomePhoneColumn.ColumnName])},
                            { "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.WorkPhoneColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.WorkPhextColumn.ColumnName])},
                            { "txtCell", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.CellNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.FaxNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.EmailAddressColumn.ColumnName])},
                            { "hfIsPrimary", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.IsPrimaryColumn.ColumnName])},
                            { "hfIsActive", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                EmergencyContactsFill_JSON = js.Serialize(keyValues)
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// Deletes the emergency contact.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string DeleteEmergencyContact(long EmergencyContactID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EmergencyContactID)))
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
                    BLObject<string> obj = BLLPatientObj.DeletePatientEmergencyContact(MDVUtility.ToStr(EmergencyContactID));
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

                case "LOAD_EMERGENCYCONTACTS":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int32 pageNumber = MDVUtility.ToInt32(context.Request["pageNumber"]);
                        Int32 rowsPerPage = MDVUtility.ToInt32(context.Request["rowsPerPage"]);
                        string strJSONData = LoadPatientEmergencyContacts(PatientID, pageNumber, rowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_EMERGENCYCONTACT":
                    {
                        string EmergencyContactID = context.Request["EmergencyContactID"];
                        string strJSONData = DeleteEmergencyContact(MDVUtility.ToInt64(EmergencyContactID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_EMERGENCYCONTACT_ACTIVE_INACTIVE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateEmergencyContactIsActive(PatientID, EmergencyContactID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EMERGENCYCONTACT_IS_PRIMARY":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        Int64 IsPrimary = MDVUtility.ToInt64(context.Request["IsPrimary"]);
                        string strJSONData = UpdateEmergencyContactIsPrimary(PatientID, EmergencyContactID, IsPrimary);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }

        #endregion
    }
}