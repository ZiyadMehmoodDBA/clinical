using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;


namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Family
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Family()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Family _obj = null;
        public static Patient_Family Instance()
        {
            if (_obj == null)
                _obj = new Patient_Family();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string UpdateFamilyIsActive(Int64 PatientId, Int64 FamilyID, Int64 IsActive)
        {
            try
            {
                if (FamilyID > 0)
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientFamily(PatientId, FamilyID);
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows[0];
                        dr[dsPatient.PatientFamily.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatient> objFamily = BLLPatientObj.UpdatePatientFamily(dsPatient);
                        string successMsg;
                        if (objFamily.Data != null)
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
                                Message = objFamily.Message
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
                        Message = "Family not found."
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

        private string LoadPatientFamilies(long PatientID, long PatientRepresentativeId, int pageNumber, int rowsPerPage, string fieldsJSON)
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
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj;

                    obj = BLLPatientObj.searchPatientFamily(PatientID, PatientRepresentativeId, pageNumber, rowsPerPage, SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["txtAccountNo"], SearchedfieldsJSON["txtPhone"]);

                    dsPatient = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PatientFamilyCount = dsPatient.Tables[dsPatient.PatientFamilySearch.TableName].Rows.Count > 0 ? dsPatient.Tables[dsPatient.PatientFamilySearch.TableName].Rows.Count : 0,
                            PatientFamilyLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientFamilySearch.TableName]),
                            iTotalDisplayRecords = (dsPatient.PatientFamilySearch.Rows.Count > 0) ? dsPatient.PatientFamilySearch.Rows[0][dsPatient.PatientFamilySearch.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes the emergency contact.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string DeleteFamily(long FamilyID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(FamilyID)))
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
                    BLObject<string> obj = BLLPatientObj.DeletePatientFamily(MDVUtility.ToStr(FamilyID));
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

        public string LookupPatientFamily(string PatientID, string AccountNo, string FirstName, string LastName)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.LookupPatientFamily(PatientID, AccountNo, FirstName, LastName);
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FamilyCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            FamilyLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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
                return (JsonConvert.SerializeObject(response));
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

                case "LOAD_PATIENTFAMILY":
                    {
                        string fieldsJSON = context.Request["FamilyData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int32 pageNumber = MDVUtility.ToInt32(context.Request["pageNumber"]);
                        Int32 rowsPerPage = MDVUtility.ToInt32(context.Request["rowsPerPage"]);
                        Int64 PatientRepresentativeId = MDVUtility.ToInt32(context.Request["PatientRepresentativeId"]);
                        
                        string strJSONData = LoadPatientFamilies(PatientID,PatientRepresentativeId, pageNumber, rowsPerPage, fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_FAMILY":
                    {
                        string FamilyID = context.Request["FamilyID"];
                        string strJSONData = DeleteFamily(MDVUtility.ToInt64(FamilyID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_FAMILY_ACTIVE_INACTIVE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 FamilyID = MDVUtility.ToInt64(context.Request["FamilyID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateFamilyIsActive(PatientID, FamilyID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOOKUP_PATIENT_FAMILY":
                    {
                        string PatientID = context.Request["PatientID"];
                        string AccountNo = context.Request["AccountNo"];
                        string FirstName = context.Request["FirstName"];
                        string LastName = context.Request["LastName"];

                        string strJSONData = LookupPatientFamily(PatientID, AccountNo, FirstName, LastName);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}