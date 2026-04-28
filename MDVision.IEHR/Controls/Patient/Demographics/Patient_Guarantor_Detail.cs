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
    public class Patient_Guarantor_Detail
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Guarantor_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Guarantor_Detail _obj = null;
        public static Patient_Guarantor_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_Guarantor_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the guarantor.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveGuarantor(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = new DSPatientProfile();
                DSPatientProfile.GuarantorRow dr = dsProfile.Guarantor.NewGuarantorRow();

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                    dr.RelationId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                    dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                dr.LastName = SearchedfieldsJSON["txtLastName"];
                dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["txtState"];
                dr.ZipCode = SearchedfieldsJSON["txtZip"];
                dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);

                #region Database Insertion
                dsProfile.Guarantor.AddGuarantorRow(dr);
                BLObject<DSPatientProfile> obj = BLLPatientObj.InsertGuarantor(dsProfile);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        GuarantorId = dsProfile.Tables[dsProfile.Guarantor.TableName].Rows[0][dsProfile.Guarantor.GuarantorIdColumn.ColumnName]
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
        /// Updates the guarantor.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <returns></returns>
        private string UpdateGuarantor(string fieldsJSON, Int64 GuarantorId, Int64 PatientID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (GuarantorId > 0)
                {

                    DSPatientProfile dsProfile = new DSPatientProfile();
                    BLObject<DSPatientProfile> objLoad = BLLPatientObj.LoadGuarantor(GuarantorId, null, null, null, null, PatientID);
                    //DSPatientProfile.GuarantorRow dr = dsProfile.Guarantor.NewGuarantorRow();
                    dsProfile = objLoad.Data;

                    foreach (DSPatientProfile.GuarantorRow dr in dsProfile.Tables[dsProfile.Guarantor.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                            dr[dsProfile.Guarantor.RelationIdColumn] = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                        if (SearchedfieldsJSON["dtpDOB"] == "")
                            dr[dsProfile.Guarantor.DOBColumn] = DBNull.Value;
                        else
                            dr[dsProfile.Guarantor.DOBColumn] = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);

                        dr[dsProfile.Guarantor.LastNameColumn] = SearchedfieldsJSON["txtLastName"];
                        dr[dsProfile.Guarantor.FirstNameColumn] = SearchedfieldsJSON["txtFirstName"];
                        dr[dsProfile.Guarantor.Address1Column] = SearchedfieldsJSON["txtAddress1"];
                        dr[dsProfile.Guarantor.CityColumn] = SearchedfieldsJSON["txtCity"];
                        dr[dsProfile.Guarantor.StateColumn] = SearchedfieldsJSON["txtState"];
                        dr[dsProfile.Guarantor.ZipCodeColumn] = SearchedfieldsJSON["txtZip"];
                        dr[dsProfile.Guarantor.ZipExtColumn] = SearchedfieldsJSON["txtZipExt"];
                        dr[dsProfile.Guarantor.PhoneNoColumn] = SearchedfieldsJSON["txtTelephone"];
                        dr[dsProfile.Guarantor.EmailAddressColumn] = SearchedfieldsJSON["txtEmail"];
                        dr[dsProfile.Guarantor.IsActiveColumn] = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr[dsProfile.Guarantor.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsProfile.Guarantor.ModifiedOnColumn] = DateTime.Now;
                        dr[dsProfile.Guarantor.EntityIdColumn] = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        dr[dsProfile.Guarantor.PatientIdColumn] = PatientID;
                    }

                    //dr.GuarantorId = GuarantorId;


                    #region Database Updation
                    //dsProfile.Guarantor.AddGuarantorRow(dr);
                    //dsProfile.Guarantor.AcceptChanges();

                    if (dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Guarantor.Rows[0].SetModified();
                        BLObject<DSPatientProfile> obj = BLLPatientObj.UpdateGuarantor(dsProfile);
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
                        Message = "Guarantor not found."
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
        /// Fills the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <returns></returns>
        private string FillGuarantor(Int64 GuarantorId,Int64 PatientID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(GuarantorId)))
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
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadGuarantor(GuarantorId, null, null, null, null,PatientID);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.Guarantor.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddlRelation", MDVUtility.ToStr(dr[dsProfile.Guarantor.RelationIdColumn.ColumnName])},
                            { "dtpDOB",String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsProfile.Guarantor.DOBColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsProfile.Guarantor.DOBColumn.ColumnName]).ToShortDateString()},
                            { "txtLastName", MDVUtility.ToStr(dr[dsProfile.Guarantor.LastNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsProfile.Guarantor.FirstNameColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Guarantor.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Guarantor.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Guarantor.ZipCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsProfile.Guarantor.ZipExtColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Guarantor.PhoneNoColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsProfile.Guarantor.Address1Column.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Guarantor.EmailAddressColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Guarantor.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                GuarantorFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <returns></returns>
        private string DeleteGuarantor(Int64 GuarantorId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(GuarantorId)))
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
                    BLObject<string> obj = BLLPatientObj.DeleteGuarantor(MDVUtility.ToStr(GuarantorId));
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
        /// Updates the guarantor is active.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateGuarantorIsActive(Int64 GuarantorId, Int64 IsActive)
        {
            try
            {
                if (GuarantorId > 0)
                {

                    DSPatientProfile dsProfile = null;
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadGuarantor(GuarantorId, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Guarantor.TableName].Rows[0];
                        dr[dsProfile.Guarantor.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientProfile> objGuarantor = BLLPatientObj.UpdateGuarantor(dsProfile);
                        string successMsg;
                        if (objGuarantor.Data != null)
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
                                Message = objGuarantor.Message
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
                        Message = "Guarantor not found."
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


        public string SearchGurantors(string name)
        {
            try
            {
                DSPatientLookups dsPatientLookup = null;
                BLObject<DSPatientLookups> obj;
                obj = BLLPatientObj.LookupGuarantor(name);
                //if (dsPatientLookup.Tables[dsPatientLookup.Guarantor.TableName].Rows.Count > 0)


                dsPatientLookup = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientLookup.Tables[dsPatientLookup.Guarantor.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            GurantorCount = dsPatientLookup.Tables[dsPatientLookup.Guarantor.TableName].Rows.Count,
                            GurantorLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatientLookup.Tables[dsPatientLookup.Guarantor.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            GurantorCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Guarantor Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_GUARANTOR":
                    {
                        string fieldsJSON = context.Request["GuarantorData"];
                        string strJSONData = SaveGuarantor(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_GUARANTOR":
                    {
                        string strGuarantorId = context.Request["GuarantorID"];
                        Int64 PatientID= MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = FillGuarantor(MDVUtility.ToInt64(strGuarantorId), PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_GUARANTOR":
                    {
                        string strGuarantorId = context.Request["GuarantorID"];
                        string strJSONData = DeleteGuarantor(MDVUtility.ToInt64(strGuarantorId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_GUARANTOR":
                    {
                        string fieldsJSON = context.Request["GuarantorData"];
                        Int64 GuarantorID = MDVUtility.ToInt64(context.Request["GuarantorID"]);
                        Int64 PatientID= MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = UpdateGuarantor(fieldsJSON, GuarantorID, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_GUARANTOR_ACTIVE_INACTIVE":
                    {
                        Int64 GuarantorID = MDVUtility.ToInt64(context.Request["GuarantorID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateGuarantorIsActive(GuarantorID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_GUARANTOR":
                    {
                        string name = MDVUtility.ToStr(context.Request["name"]);
                        string strJSONData = SearchGurantors(name);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion
    }
}