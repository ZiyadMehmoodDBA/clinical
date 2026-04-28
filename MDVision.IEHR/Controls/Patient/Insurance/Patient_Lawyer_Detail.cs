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
    public class Patient_Lawyer_Detail
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Lawyer_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Lawyer_Detail _obj = null;
        public static Patient_Lawyer_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_Lawyer_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the lawyer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveLawyer(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = new DSPatientProfile();
                DSPatientProfile.LawyerRow dr = dsProfile.Lawyer.NewLawyerRow();

                dr.LawyerName = SearchedfieldsJSON["txtLawyerName"];
                dr.FirmName = SearchedfieldsJSON["txtFirmName"];
                dr.LicenseNo = SearchedfieldsJSON["txtLicense"];
                dr.ContactNo = SearchedfieldsJSON["txtContact"];
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["txtState"];
                dr.ZipCode = SearchedfieldsJSON["txtZip"];
                dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsProfile.Lawyer.AddLawyerRow(dr);
                BLObject<DSPatientProfile> obj = BLLPatientObj.InsertLawyer(dsProfile);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LawyerId = dsProfile.Tables[dsProfile.Lawyer.TableName].Rows[0][dsProfile.Lawyer.LawyerIdColumn.ColumnName]
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
        /// Updates the lawyer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <returns></returns>
        private string UpdateLawyer(string fieldsJSON, Int64 LawyerId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (LawyerId > 0)
                {

                    DSPatientProfile dsProfile = new DSPatientProfile();
                    //DSPatientProfile.LawyerRow dr = dsProfile.Lawyer.NewLawyerRow();
                    BLObject<DSPatientProfile> objLoad = BLLPatientObj.LoadLawyer(LawyerId, null, null, null, null, null);
                    dsProfile = objLoad.Data;

                    foreach (DSPatientProfile.LawyerRow dr in dsProfile.Tables[dsProfile.Lawyer.TableName].Rows)
                    {
                        //dr.LawyerId = LawyerId;
                        dr.LawyerName = SearchedfieldsJSON["txtLawyerName"];
                        dr.FirmName = SearchedfieldsJSON["txtFirmName"];
                        dr.LicenseNo = SearchedfieldsJSON["txtLicense"];
                        dr.ContactNo = SearchedfieldsJSON["txtContact"];
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZipCode = SearchedfieldsJSON["txtZip"];
                        dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsProfile.Lawyer.AddLawyerRow(dr);
                    //dsProfile.Lawyer.AcceptChanges();

                    if (dsProfile.Tables[dsProfile.Lawyer.TableName].Rows.Count > 0)
                    {
                        //dsProfile.Lawyer.Rows[0].SetModified();
                        BLObject<DSPatientProfile> obj = BLLPatientObj.UpdateLawyer(dsProfile);
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
                        Message = "Lawyer not found."
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
        /// Fills the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <returns></returns>
        private string FillLawyer(Int64 LawyerId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LawyerId)))
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
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadLawyer(LawyerId, null, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.Lawyer.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.Lawyer.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtLawyerName", MDVUtility.ToStr(dr[dsProfile.Lawyer.LawyerNameColumn.ColumnName])},
                            { "txtFirmName", MDVUtility.ToStr(dr[dsProfile.Lawyer.FirmNameColumn.ColumnName])},
                            { "txtLicense", MDVUtility.ToStr(dr[dsProfile.Lawyer.LicenseNoColumn.ColumnName])},
                            { "txtContact", MDVUtility.ToStr(dr[dsProfile.Lawyer.ContactNoColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsProfile.Lawyer.Address1Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Lawyer.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Lawyer.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Lawyer.ZipCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsProfile.Lawyer.ZipExtColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Lawyer.PhoneNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Lawyer.EmailAddressColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Lawyer.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                LawyerFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <returns></returns>
        private string DeleteLawyer(Int64 LawyerId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LawyerId)))
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
                    BLObject<string> obj = BLLPatientObj.DeleteLawyer(MDVUtility.ToStr(LawyerId));
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
        /// Updates the lawyer is active.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateLawyerIsActive(Int64 LawyerId, Int64 IsActive)
        {
            try
            {
                if (LawyerId > 0)
                {

                    DSPatientProfile dsProfile = null;
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadLawyer(LawyerId, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Lawyer.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Lawyer.TableName].Rows[0];
                        dr[dsProfile.Lawyer.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientProfile> objLawyer = BLLPatientObj.UpdateLawyer(dsProfile);
                        string successMsg;
                        if (objLawyer.Data != null)
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
                                Message = objLawyer.Message
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
                        Message = "Lawyer not found."
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
        /// Handle the Lawyer Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_LAWYER":
                    {
                        string fieldsJSON = context.Request["LawyerData"];
                        string strJSONData = SaveLawyer(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_LAWYER":
                    {
                        string strLawyerId = context.Request["LawyerID"];
                        string strJSONData = FillLawyer(MDVUtility.ToInt64(strLawyerId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_LAWYER":
                    {
                        string strLawyerId = context.Request["LawyerID"];
                        string strJSONData = DeleteLawyer(MDVUtility.ToInt64(strLawyerId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_LAWYER":
                    {
                        string fieldsJSON = context.Request["LawyerData"];
                        Int64 LawyerID = MDVUtility.ToInt64(context.Request["LawyerID"]);
                        string strJSONData = UpdateLawyer(fieldsJSON, LawyerID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_LAWYER_ACTIVE_INACTIVE":
                    {
                        Int64 LawyerID = MDVUtility.ToInt64(context.Request["LawyerID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateLawyerIsActive(LawyerID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}