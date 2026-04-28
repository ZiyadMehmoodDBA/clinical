using System;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.Common;
using MDVision.Model.Patient;
using Newtonsoft.Json;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient
{
    public class Patient_Search
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Search()
        {

            BLLPatientObj = new BLLPatient();
        }

        #region Singleton
        private static Patient_Search _obj = null;
        public static Patient_Search Instance()
        {
            if (_obj == null)
                _obj = new Patient_Search();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Load all the LoadPatient for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The BasicFeeGroup identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchPatient(string fieldsJSON, Int64 PatientID, Int64 pageNo, Int64 rpp)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow drPatient = dsPatient.Patients.NewPatientsRow();
                BLObject<DSPatient> obj;
                if (SearchedfieldsJSON == null)
                {
                    if (PatientID > 0)
                        drPatient.PatientId = PatientID;
                    drPatient.PageNumber = MDVUtility.ToInt32(pageNo);
                    drPatient.RowspPage = MDVUtility.ToInt32(rpp);
                    if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != AppPrivileges.DefaultUser.ToUpper())
                    {
                        drPatient.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    }
                }
                else
                {
                    if (PatientID > 0)
                        drPatient.PatientId = PatientID;
                    drPatient.FirstName = SearchedfieldsJSON["txtSearchFirstName"];
                    drPatient.LastName = SearchedfieldsJSON["txtSearchLastName"];
                    drPatient.AccountNumber = SearchedfieldsJSON["txtSearchAccountNo"];
                    drPatient.SSN = SearchedfieldsJSON["txtSSNSearch"];
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                    //    drPatient.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]) == "1" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                        drPatient.IsActive = MDVUtility.ToInt32(SearchedfieldsJSON["ddlActive"]);
                    if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != AppPrivileges.DefaultUser.ToUpper())
                        drPatient.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    drPatient.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpSearchDOB"]);
                    drPatient.MRNumber = SearchedfieldsJSON["txtMRN"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSearchProvider"]))
                        drPatient.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSearchProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSearchFacility"]))
                        drPatient.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSearchFacility"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSearchPractice"]))
                        drPatient.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSearchPractice"]);
                    drPatient.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                    drPatient.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    drPatient.BadAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkBadAddress"]) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSearchSex"]))
                        drPatient.Gender = MDVUtility.ToStr(SearchedfieldsJSON["ddlSearchSex_text"]);
                    drPatient.PageNumber = MDVUtility.ToInt32(pageNo);
                    drPatient.RowspPage = MDVUtility.ToInt32(rpp);
                    drPatient.IsSearch = true;
                    // Begin 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                    drPatient.CoverageType = SearchedfieldsJSON["ddlCoverageType"];
                    // End 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                    // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                    drPatient.ClaimNo = SearchedfieldsJSON["txtClaimno"];
                    // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                }

                dsPatient.Patients.AddPatientsRow(drPatient);

                obj = BLLPatientObj.PatientsSearch(dsPatient);
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatient.Patients.Rows[0][dsPatient.Patients.RecordCountColumn.ColumnName],
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = "Record not found."
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
        public string SearchPatient(PatientModel model)
        {
            try
            {

                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow drPatient = dsPatient.Patients.NewPatientsRow();
                BLObject<DSPatient> obj;

                if (MDVUtility.ToInt32(model.PatientID) > 0)
                    drPatient.PatientId = MDVUtility.ToInt32(model.PatientID);
                drPatient.FirstName = model.FirstName;
                drPatient.LastName = model.LastName;
                drPatient.AccountNumber = model.AccountNo;
                drPatient.SSN = model.SSN;

                if (string.IsNullOrEmpty(model.Active))
                    drPatient[dsPatient.Patients.IsActiveColumn] = DBNull.Value;
                else
                    //drPatient.IsActive = model.Active == "1" ? true : false;
                    drPatient.IsActive = Convert.ToInt32(model.Active);
                if (!string.IsNullOrEmpty(model.DOB))
                {
                    drPatient.DOB = MDVUtility.ToDateTime(model.DOB);
                }
                drPatient.MRNumber = model.MRN;

                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != AppPrivileges.DefaultUser.ToUpper())
                    drPatient.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                if (!string.IsNullOrEmpty(model.ProviderID))
                    drPatient.ProviderId = MDVUtility.ToInt64(model.ProviderID);
                if (!string.IsNullOrEmpty(model.FacilityID))
                    drPatient.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                if (!string.IsNullOrEmpty(model.PracticeID))
                    drPatient.PracticeId = MDVUtility.ToInt64(model.PracticeID);
                if (!string.IsNullOrEmpty(model.InsurancePlanID))
                    drPatient.InsurancePlanId = MDVUtility.ToInt64(model.InsurancePlanID);
                if (!string.IsNullOrEmpty(model.GuarantorId))
                {
                    drPatient.GuarantorId = model.GuarantorId;
                }
                drPatient.HomePhoneNo = model.HomePhone;
                drPatient.EmailAddress = model.Email;
                if (!string.IsNullOrEmpty(model.BadAddress))
                    drPatient.BadAddress = model.BadAddress.ToLower() == "true" ? true : false;
                if (model.Sex == "0")
                {
                    drPatient.Gender = "Male";
                }
                else if (model.Sex == "1")
                {

                    drPatient.Gender = "Female";
                }
                else if (model.Sex == "2")
                {

                    drPatient.Gender = "Unknown";
                }
                else
                {

                    drPatient.Gender = model.Sex;
                }

                drPatient.PageNumber = MDVUtility.ToInt32(model.PageNo);
                drPatient.RowspPage = MDVUtility.ToInt32(model.rpp);
                drPatient.IsSearch = true;
                // Begin 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                drPatient.CoverageType = model.CoverageType;
                // End 05-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3136
                // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                drPatient.ClaimNo = model.ClaimNumber;
                // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018


                if (string.IsNullOrEmpty(model.IsRecentPatients))
                    drPatient[dsPatient.Patients.IsRecentPatientColumn] = DBNull.Value;
                else
                    drPatient.IsRecentPatient = model.IsRecentPatients == "1" ? true : false;

                if (!string.IsNullOrEmpty(model.AppointmentDate))
                {
                    drPatient.AppointmentDate = MDVUtility.ToDateTime(model.AppointmentDate);
                }
                if (model.IncompleteDemographics != null)

                    drPatient.IncompleteDemographics = model.IncompleteDemographics.ToLower() == "true" ? 1 : 0;
                else
                    drPatient.IncompleteDemographics = 0;

                dsPatient.Patients.AddPatientsRow(drPatient);
                obj = BLLPatientObj.PatientsSearch(dsPatient);
                //dsPatient = new DSPatient();
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatient.Patients.Rows[0][dsPatient.Patients.RecordCountColumn.ColumnName],
                            //(DSModuleForm.ModulesRow dr in dsModuleForm.Modules.Rows
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = "Record not found."
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
        private string UpdatePatientIsActive(Int64 PatientID, Int64 pageNo, Int64 rpp, Int64 IsActive)
        {
            try
            {
                if (PatientID > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient.PatientsRow drPatient = dsPatient.Patients.NewPatientsRow();
                    if (PatientID > 0)
                        drPatient.PatientId = PatientID;
                    drPatient.IsFileStream = true;
                    drPatient.PageNumber = MDVUtility.ToInt32(pageNo);
                    drPatient.RowspPage = MDVUtility.ToInt32(rpp);
                    if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != AppPrivileges.DefaultUser.ToUpper())
                    {
                        drPatient.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    }
                    dsPatient.Patients.AddPatientsRow(drPatient);

                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatient(dsPatient);
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                        if (Convert.ToInt32(dr[dsPatient.Patients.IsActiveColumn.ColumnName]) == 2)
                        {
                            dr[dsPatient.Patients.DODColumn.ColumnName] = DBNull.Value;
                            dr[dsPatient.Patients.CauseOfDeathColumn.ColumnName] = DBNull.Value;
                        }
                        dr[dsPatient.Patients.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatient> objPatient = BLLPatientObj.UpdatePatient(dsPatient);
                        string successMsg;
                        if (objPatient.Data != null)
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
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objPatient.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string DeletePatient(long PatientID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeletePatient(MDVUtility.ToStr(PatientID));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Recent Access Patient
        public string SaveRecentPatient(PatientModel model)
        {
            try
            {
                DSPatient dsPatient = new DSPatient();
                DSPatient.RecentAccessedPatientsRow dr = dsPatient.RecentAccessedPatients.NewRecentAccessedPatientsRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientID);
                dr.UserId = MDVSession.Current.AppUserId;
                dr.AccessedOn = DateTime.Now;
                #region Database Insertion
                dsPatient.RecentAccessedPatients.AddRecentAccessedPatientsRow(dr);
                BLObject<DSPatient> obj = BLLPatientObj.InsertRecentPatient(dsPatient);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        RecPatId = dsPatient.Tables[dsPatient.RecentAccessedPatients.TableName].Rows[0][dsPatient.RecentAccessedPatients.AccessedPatientIdColumn.ColumnName],

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
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
        /// Handle the Patient Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "SEARCH_PATIENT":
                    {
                        string fieldsJSON = context.Request["PatientData"];
                        string page = context.Request["page"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        Int64 BasicFeeGroupID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SearchPatient(fieldsJSON, BasicFeeGroupID, pageNo, rpp);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_ACTIVE_INACTIVE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Demographic", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                            Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                            Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);
                            strJSONData = UpdatePatientIsActive(PatientID, pageNo, rpp, IsActive);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = DeletePatient(PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
