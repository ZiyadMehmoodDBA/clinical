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

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_EmergencyContact_Detail
    {
         private BLLPatient BLLPatientObj = null;
         public Patient_EmergencyContact_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_EmergencyContact_Detail _obj = null;
        public static Patient_EmergencyContact_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_EmergencyContact_Detail();
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
                    dr.RelationShipId = MDVUtility.ToConvertInt32(SearchedfieldsJSON["ddlRelation"]);
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
                            EmergencyContactID = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0][dsPatient.EmergencyContacts.ContactIdColumn.ColumnName]
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
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMiddleInitial"]))
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
                        if (SearchedfieldsJSON["txtHomeTel"] == "" && SearchedfieldsJSON["txtCell"] == "")
                        {
                            if (MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True")
                            {
                                dr.IsPrimary = false;
                            }
                        }
                        else
                        {
                            dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True" ? true : false;
                        }
                        //dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["hfIsActive"]) == "True" ? true : false;
                        dr.RelationShipId = MDVUtility.ToConvertInt32(SearchedfieldsJSON["ddlRelation"]);
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

        /// <summary>
        /// Fills the patient information.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="IsFromSearch">The is from search.</param>
        /// <returns>System.String.</returns>
        private string FillPatientInfo(Int64 PatientId, Int64 IsFromSearch)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
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
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, "EmergencyContact");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string> { };
                            if (IsFromSearch == 1)
                            {
                                keyValues = new Dictionary<string, string> 
                            {
                                { "txtLastName", MDVUtility.ToStr(dr[dsPatient.Patients.LastNameColumn.ColumnName])},
                                { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.Patients.FirstNameColumn.ColumnName])},
                                { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.Patients.MIColumn.ColumnName])},
                                { "dtpDOB", MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToShortDateString():"")},
                                { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                                { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                                { "txtCity", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                                { "txtState", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                                { "txtZip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                                { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                                { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                                { "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                                { "txtExt", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                                { "txtCell", MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName])},
                                { "txtFax", MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName])},
                                { "txtEmail", MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName])}
                            };
                            }
                            else
                            {
                                keyValues = new Dictionary<string, string>
                            {
                                { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                                { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                                { "txtCity", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                                { "txtState", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                                { "txtZip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                                { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                                { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                                //{ "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                                //{ "txtExt", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                                //{ "txtCell", MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName])},
                                //{ "txtFax", MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName])},
                                //{ "txtEmail", MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName])}
                            };
                            }

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientInfoFill_JSON = js.Serialize(keyValues)
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
                            { "dtpDOB", MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.DOBColumn.ColumnName]))},
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
                            { "ddlRelation", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.RelationShipIdColumn.ColumnName])},
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
                case "SAVE_EMERGENCYCONTACT":
                    {
                        string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SaveEmergencyContact(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EMERGENCYCONTACT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string strJSONData = FillEmergencyContact(PatientID, EmergencyContactID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_EMERGENCYCONTACT":
                    {
                        string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string strJSONData = UpdateEmergencyContact(fieldsJSON, PatientID, EmergencyContactID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_PATIENT_INFO":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 IsFromSearch = MDVUtility.ToInt64(context.Request["IsFromSearch"]);
                        string strJSONData = FillPatientInfo(PatientID, IsFromSearch);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion

    }
}
