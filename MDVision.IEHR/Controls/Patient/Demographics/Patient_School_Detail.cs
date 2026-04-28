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
    public class Patient_School_Detail
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_School_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_School_Detail _obj = null;
        public static Patient_School_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_School_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the school.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveSchool(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = new DSPatientProfile();
                DSPatientProfile.SchoolRow dr = dsProfile.School.NewSchoolRow();

                dr.SchoolName = SearchedfieldsJSON["txtSchoolName"];
                dr.ContactName = SearchedfieldsJSON["txtContactName"];
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
                dsProfile.School.AddSchoolRow(dr);
                BLObject<DSPatientProfile> obj = BLLPatientObj.InsertSchool(dsProfile);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        SchoolId = dsProfile.Tables[dsProfile.School.TableName].Rows[0][dsProfile.School.SchoolIdColumn.ColumnName]
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
        /// Updates the school.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SchoolId">The school identifier.</param>
        /// <returns></returns>
        private string UpdateSchool(string fieldsJSON, Int64 SchoolId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SchoolId > 0)
                {

                    DSPatientProfile dsProfile = new DSPatientProfile();
                    //DSPatientProfile.SchoolRow dr = dsProfile.School.NewSchoolRow();
                    BLObject<DSPatientProfile> objLoad = BLLPatientObj.LoadSchool(SchoolId, null, null, null, null, null, null);
                    dsProfile = objLoad.Data;

                    foreach (DSPatientProfile.SchoolRow dr in dsProfile.Tables[dsProfile.School.TableName].Rows)
                    {
                        //dr.SchoolId = SchoolId;
                        dr.SchoolName = SearchedfieldsJSON["txtSchoolName"];
                        dr.ContactName = SearchedfieldsJSON["txtContactName"];
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZipCode = SearchedfieldsJSON["txtZip"];
                        dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                        dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    }

                    #region Database Updation
                    //dsProfile.School.AddSchoolRow(dr);
                    //dsProfile.School.AcceptChanges();
                    if (dsProfile.Tables[dsProfile.School.TableName].Rows.Count > 0)
                    {
                        //dsProfile.School.Rows[0].SetModified();
                        BLObject<DSPatientProfile> obj = BLLPatientObj.UpdateSchool(dsProfile);
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
                        Message = "School not found."
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
        /// Fills the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <returns></returns>
        private string FillSchool(Int64 SchoolId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SchoolId)))
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
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadSchool(SchoolId, null, null, null, null, null, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.School.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.School.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtSchoolName", MDVUtility.ToStr(dr[dsProfile.School.SchoolNameColumn.ColumnName])},
                            { "txtContactName", MDVUtility.ToStr(dr[dsProfile.School.ContactNameColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.School.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.School.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.School.ZipCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsProfile.School.ZipExtColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.School.PhoneNoColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsProfile.School.Address1Column.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.School.EmailAddressColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.School.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                SchoolFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <returns></returns>
        private string DeleteSchool(Int64 SchoolId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SchoolId)))
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
                    BLObject<string> obj = BLLPatientObj.DeleteSchool(MDVUtility.ToStr(SchoolId));
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
        /// Updates the school is active.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdateSchoolIsActive(Int64 SchoolId, Int64 IsActive)
        {
            try
            {
                if (SchoolId > 0)
                {

                    DSPatientProfile dsProfile = null;
                    BLObject<DSPatientProfile> obj = BLLPatientObj.LoadSchool(SchoolId, null, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.School.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.School.TableName].Rows[0];
                        dr[dsProfile.School.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientProfile> objSchool = BLLPatientObj.UpdateSchool(dsProfile);
                        string successMsg;
                        if (objSchool.Data != null)
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
                                Message = objSchool.Message
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
                        Message = "School not found."
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
        /// Handle the School Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_SCHOOL":
                    {
                        string fieldsJSON = context.Request["SchoolData"];
                        string strJSONData = SaveSchool(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SCHOOL":
                    {
                        string strSchoolId = context.Request["SchoolID"];
                        string strJSONData = FillSchool(MDVUtility.ToInt64(strSchoolId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SCHOOL":
                    {
                        string strSchoolId = context.Request["SchoolID"];
                        string strJSONData = DeleteSchool(MDVUtility.ToInt64(strSchoolId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHOOL":
                    {
                        string fieldsJSON = context.Request["SchoolData"];
                        Int64 SchoolID = MDVUtility.ToInt64(context.Request["SchoolID"]);
                        string strJSONData = UpdateSchool(fieldsJSON, SchoolID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCHOOL_ACTIVE_INACTIVE":
                    {
                        Int64 SchoolID = MDVUtility.ToInt64(context.Request["SchoolID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateSchoolIsActive(SchoolID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}